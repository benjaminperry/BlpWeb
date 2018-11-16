using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BlpWebApi.Controllers
{
    [Authorize]
    public class ExceptionTestController : Controller
    {
        ILogger logger;

        public ExceptionTestController(ILogger<ExceptionTestController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            ApplicationException exInnerInner = new ApplicationException("Test inner inner exception.");
            exInnerInner.Data.Add("Test1", "Data1");
            exInnerInner.Data.Add("Test2", "Data2");
            exInnerInner.Data.Add("Test3", "Data3");

            ApplicationException exInner = new ApplicationException("Test inner exception.", exInnerInner);
            exInner.Data.Add("Test1", "Data1");
            exInner.Data.Add("Test2", "Data2");
            exInner.Data.Add("Test3", "Data3");

            ApplicationException exOuter = new ApplicationException("Test exception.", exInner);
            exOuter.Data.Add("Test1", "Data1");
            exOuter.Data.Add("Test2", "Data2");
            exOuter.Data.Add("Test3", "Data3");

            
            throw exOuter;
            
            return View();
        }
    }
}
