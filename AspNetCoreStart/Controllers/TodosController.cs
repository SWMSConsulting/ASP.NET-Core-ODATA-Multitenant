using AspNetCoreStart.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace AspNetCoreStart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ODataEFController<Todo>
    {
        public TodosController(ApplicationDbContext context) : base(context)
        {

        }
    }
}