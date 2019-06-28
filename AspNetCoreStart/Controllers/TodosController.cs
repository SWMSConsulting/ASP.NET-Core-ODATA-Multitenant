using AspNetCoreStart.Context;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SWMS.FieldService.Portal.Controllers
{
    [Produces("application/json")]
    public class TodosController : ODataController
    {
        private readonly MyAppDbContext context;

        public TodosController(MyAppDbContext context)
        {
            this.context = context;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(context.Todos.AsQueryable());
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(context.Todos.FirstOrDefault(c => c.Id == key));
        }
    }
}