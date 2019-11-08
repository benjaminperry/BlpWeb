using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blp.NetCoreLearning.WebApp.Controllers
{
    [Authorize]
    public class ApiDocumentationController : Controller
    {
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        public ApiDocumentationController(IApiDescriptionGroupCollectionProvider apiExplorer)
        {
            _apiExplorer = apiExplorer;
        }

        public IActionResult Index()
        {
            return View(_apiExplorer);
        }
    }

}
