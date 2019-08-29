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
        private BlpWebBaseContext blpWebContext;

        public TestEntityController(BlpWebBaseContext blpWebContext)
        {
            this.blpWebContext = blpWebContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(blpWebContext.TestEntities.Where(x => true).ToList());
        }
    }
}
