using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlpEntities;
using BlpData;

namespace BlpWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestEntityController : ControllerBase
    {
        private BlpContext blpContext;

        public TestEntityController(BlpContext blpContext)
        {
            this.blpContext = blpContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(blpContext.TestEntities.Where(x => true).ToList());
        }
    }
}
