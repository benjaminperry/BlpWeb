using Blp.NetCoreLearning.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blp.NetCoreLearning.WebApp.Controllers
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
