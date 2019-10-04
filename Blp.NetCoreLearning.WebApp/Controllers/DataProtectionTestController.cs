using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blp.NetCoreLearning.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DataProtectionTestController : Controller
    {
        private IDataProtector _DataProtector;

        public DataProtectionTestController(IDataProtectionProvider provider)
        {
            _DataProtector = provider.CreateProtector("DataProtectionTest");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            string x = "This will be protected";
            string y = _DataProtector.Protect(x);
            string z = _DataProtector.Unprotect(y);

            return Ok(new string[] { "Original: " + x, "Protected: " + y, "Unprotected: " + z });
        }
    }
}