using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BlpWebApp.Controllers
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
            ThrowOuter();
            
            return View();
        }

        private void ThowInnerInner()
        {
            throw new ApplicationException("Exception from ThrowInnerInner");
        }

        private void ThrowInner()
        {
            try
            {
                ThowInnerInner();
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Exception from ThrowInner", ex);
            }
        }

        private void ThrowOuter()
        {
            try
            {
                ThrowInner();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception from ThrowOuter", ex);
            }
        }
    }
}
