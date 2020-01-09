using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreStart.Context;
using AspNetCoreStart.Messaging;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;

namespace AspNetCoreStart.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ODataEFController<T> : ODataController where T: class, IIndexedModel, ISecurityModel
    {
        private readonly ApplicationDbContext context;
        private readonly IMessageBroadcast message;
        private readonly string userId;
        private readonly SecurityBaseStrategy securityBaseStrategy;

        public ODataEFController(ApplicationDbContext context, IMessageBroadcast message, IHttpContextAccessor httpContextAccessor, SecurityBaseStrategy securityBaseStrategy)
        {
            this.context = context;
            this.message = message;
            this.userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            this.securityBaseStrategy = securityBaseStrategy;
        }

        [EnableQuery]
        public IQueryable<T> Get()
        {
            switch (securityBaseStrategy)
            {
                case SecurityBaseStrategy.allow:
                    return TableForT().Where<T>(x => !x.SecurityEntries.Any(x => x._SecurityTask == SecurityStrategyEnum.deny && x._SecurityUser == userId));
                    break;
                case SecurityBaseStrategy.deny:
                    return TableForT().Where<T>(x => x.SecurityEntries.Any(x => x._SecurityTask == SecurityStrategyEnum.allow && x._SecurityUser == userId));
                    break;
                default:
                    return TableForT();
            }
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        public SingleResult<T> Get([FromODataUri] int key)
        {
            IQueryable<T> result;
            switch (securityBaseStrategy)
            {
                case SecurityBaseStrategy.allow:
                    result = Get().Where(p => p.Id == key && !p.SecurityEntries.Any(x => x._SecurityTask == SecurityStrategyEnum.deny && x._SecurityUser == userId));
                    return SingleResult.Create(result);
                    break;
                case SecurityBaseStrategy.deny:
                    result = Get().Where(p => p.Id == key && p.SecurityEntries.Any(x => x._SecurityTask == SecurityStrategyEnum.allow && x._SecurityUser == userId));
                    return SingleResult.Create(result);
                    break;
                default:
                    result = Get().Where(p => p.Id == key);
                    return SingleResult.Create(result);

            }

        }

        public async Task<IActionResult> Post([FromBody] T value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TableForT().Add(value);
            await context.SaveChangesAsync();
            await message.Send(TopicEnum.New, value.GetType().Name, value);
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
                await message.Send(TopicEnum.Patch, value.GetType().Name, value);
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
                await message.Send(TopicEnum.Update, update.GetType().Name, update);
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
            await message.Send(TopicEnum.Delete, entity.GetType().Name, entity);
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