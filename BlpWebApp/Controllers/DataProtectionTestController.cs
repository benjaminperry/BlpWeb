using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlpWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DataProtectionTestController : Controller
    {
        IDataProtector _protector;

        public DataProtectionTestController(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("DataProtectionTest");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> Get()
        {
            string x = "This will be protected";
            string y = _protector.Protect(x);

            return new string[] { "Unprotected: " + x, "Protected: " + y };
        }
    }
}