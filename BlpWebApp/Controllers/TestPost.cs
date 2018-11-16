using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlpWebApp.Controllers
{
    [Route("TestPost")]
    [ApiController]
    public class TestPost : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Test" };
        }

        [HttpPost]
        public ActionResult Post([FromBody]PostData postData)
        {
            return RedirectToAction("Get");
        }
    }

    public class PostData
    {
        public PostData()
        {
            Field1 = "";
            Field2 = "";
            Field3 = "";
            PostChildData = new PostChildData();
        }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public PostChildData PostChildData { get; set; }

    }

    public class PostChildData
    {
        public PostChildData()
        {
            ChildField1 = "";
            ChildField2 = "";
            ChildField3 = "";
        }

        public string ChildField1 { get; set; }
        public string ChildField2 { get; set; }
        public string ChildField3 { get; set; }
    }
}
