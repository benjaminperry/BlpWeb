using BlpData;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlpWebApp.Models
{
    public class BlpWebContext : BlpWebBaseContext
    {
        public BlpWebContext(DbContextOptions<BlpWebBaseContext> options) : base(options)
        {
            Console.WriteLine("BLPWEBCONTEXT CONTRUCTED AT: " + DateTime.Now);
        }
    }
}
