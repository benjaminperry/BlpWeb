using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlpWebApp.Controllers
{
    [Route("RequestInfo")]
    [ApiController]
    public class RequestInfoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string protocol = Request.Protocol;
            System.Net.IPAddress remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            int remotePort = Request.HttpContext.Connection.RemotePort;
            System.Net.IPAddress localIpAddress = Request.HttpContext.Connection.LocalIpAddress;
            int localPort = Request.HttpContext.Connection.LocalPort;
            IHeaderDictionary headerDictionary = Request.Headers;
            string method = Request.Method;

            List<string> ret = new List<string>();

            ret.Add($"Protocol: { protocol}");
            ret.Add($"Remote IP address: {remoteIpAddress.ToString()}");
            ret.Add($"Remote port: {remotePort.ToString()}");
            ret.Add($"Local IP address: {localIpAddress.ToString()}");
            ret.Add($"Local port: {localPort.ToString()}");
            ret.Add($"Method: {method}");

            ret.AddRange(headerDictionary.Select(h => h.ToString()));

            return ret;
        }
    }
}
