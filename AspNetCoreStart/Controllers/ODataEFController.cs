using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNetCoreStart.Context;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;

namespace AspNetCoreStart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ODataEFController<T> : ODataController where T: class, IIndexedModel
    {
        private readonly ApplicationDbContext context;

        public ODataEFController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [EnableQuery]
        public IQueryable<T> Get()
        {
            return TableForT();
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        public SingleResult<T> Get([FromODataUri] int key)
        {
            IQueryable<T> result = Get().Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        public async Task<IActionResult> Post([FromBody] T value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TableForT().Add(value);
            await context.SaveChangesAsync();
            return Created(value);
        }

        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<T> value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await TableForT().FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            value.Patch(entity);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }

        public async Task<IActionResult> Put([FromODataUri] int key, T update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest();
            }
            context.Entry(update).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var entity = await TableForT().FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            TableForT().Remove(entity);
            await context.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        private bool EntityExists(int key)
        {
            return TableForT().Any(p => p.Id == key);
        }

        private DbSet<T> TableForT()
        {
            return context.Set<T>();
        }
    }
}