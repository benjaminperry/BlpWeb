using BlpData;
using BlpEntities;
using BlpWebApp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace QuickTests
{
    class Program
    {
        const string connectionString = "Server=10.20.0.1;Initial Catalog=blpweb;Integrated Security=True";

        static void Main(string[] args)
        {

            Console.WriteLine("Press enter to terminate");
            Console.ReadLine();
        }

        private static void EntityTest01()
        {
            using (BlpWebBaseContext context = CreateBlpWebContextContext())
            {
                IOrderedQueryable<BuddAccount> buddAccounts;


            }
        }

        private static BlpWebBaseContext CreateBlpWebContextContext()
        {
            var builder = new DbContextOptionsBuilder<BlpWebBaseContext>();
            builder.UseSqlServer(connectionString);
            return new BlpWebContext(builder.Options);
        }
    }
}
