using BlpData;
using BlpEntities;
using BlpWebApp.Models;
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
            DelegateTest();
            Console.WriteLine("Press enter to terminate");
            Console.ReadLine();
        }

        private static void DelegateTest()
        {
            Func<int, int, int> func = (int x, int y) =>  x + y;
            int z = func(1, 2);
            Console.WriteLine($"The result is {z}.");
        }


        //private static void EntityTest01()
        //{
        //    using (BlpData.BlpContext context = CreateBlpWebContextContext())
        //    {
        //        IOrderedQueryable<BuddAccount> buddAccounts;


        //    }
        //}

        //private static BlpContext CreateBlpWebContextContext()
        //{
        //    var builder = new DbContextOptionsBuilder<BlpData.BlpContext>();
        //    builder.UseSqlServer(connectionString);
        //    return new BlpWebApp.Models.BlpContext(builder.Options);
        //}
    }
}
