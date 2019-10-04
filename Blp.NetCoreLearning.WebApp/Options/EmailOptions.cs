using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blp.NetCoreLearning.WebApp.Options
{
    public class EmailOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
