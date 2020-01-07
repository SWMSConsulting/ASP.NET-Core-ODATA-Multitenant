using AspNetCoreStart.Context;
using AspNetCoreStart.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreStart.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TodosController : ODataEFController<Todo>
    {
        public TodosController(ApplicationDbContext context, IMessageBroadcast message) : base(context, message)
        {

        }
    }
}