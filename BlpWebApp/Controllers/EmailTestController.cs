using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlpWebApp.Controllers
{
    [Authorize]
    public class EmailTestController : Controller
    {
        IEmailSender _emailSender;

        public EmailTestController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<ActionResult> Index()
        {
            string body = @"
<html>
<head></head>
<body>
<div>
<h3>This is a test email</h3>
</div>
</body>
</html>
";

            await _emailSender.SendEmailAsync("benjamin361@gmail.com", "Test Email", body);
            return RedirectToAction("Index", "Home");
        }
    }
}
