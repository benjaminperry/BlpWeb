using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlpData;
using Microsoft.EntityFrameworkCore;

namespace BlpWebApi
{
    public class BlpWebContext : BlpWebBaseContext
    {
        public BlpWebContext(DbContextOptions<BlpWebBaseContext> options) : base(options)
        {
            Console.WriteLine("BLPWEBCONTEXT CONTRUCTED AT: " + DateTime.Now);
        }
    }
}
