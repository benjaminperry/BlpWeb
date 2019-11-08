using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blp.NetCoreLearning.WebApp.V2.Features
{
    [Authorize(Roles = IdentityRoles.Administrator)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            ValuesController x = this;
            
            return Ok(new string[] {"API Version 2.0", "value1", "value2" });
        }

        // GET api/values/5
/// <summary>
/// 
/// </summary>
/// <param name="id">This is an XML comment for the id param.</param>
/// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            
            return Ok("value");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
