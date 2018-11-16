using BlpData;
using BlpEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace DataMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running EF Migrations");
            try
            {
                var dbContextFactory = new DesignTimeBlpWebMigrationContextFactory();

                using (BlpWebMigrationContext dbContext = dbContextFactory.CreateDbContext(null))
                {
                    dbContext.Database.Migrate();

                    foreach (var appliedMigration in dbContext.Database.GetAppliedMigrations())
                    {
                        Console.WriteLine($"Applied {appliedMigration}");
                    }

                    if (!dbContext.TestEntities.Any())
                    {
                        TestEntity testEntity01 = new TestEntity { Description = "This is test 01" };
                        TestEntity testEntity02 = new TestEntity { Description = "This is test 02" };
                        TestEntity testEntity03 = new TestEntity { Description = "This is test 03" };
                        TestEntity testEntity04 = new TestEntity { Description = "This is test 04" };
                        TestEntity testEntity05 = new TestEntity { Description = "This is test 05" };
                        TestEntity testEntity06 = new TestEntity { Description = "This is test 06" };
                        TestEntity testEntity07 = new TestEntity { Description = "This is test 07" };
                        TestEntity testEntity08 = new TestEntity { Description = "This is test 08" };
                        TestEntity testEntity09 = new TestEntity { Description = "This is test 09" };
                        TestEntity testEntity10 = new TestEntity { Description = "This is test 10" };

                        TestEntityNote testEntityNote01 = new TestEntityNote { Note = "This is a test note", TestEntity = testEntity01};
                        TestEntityNote testEntityNote02 = new TestEntityNote { Note = "This is a test note", TestEntity = testEntity01 };
                        TestEntityNote testEntityNote03 = new TestEntityNote { Note = "This is a test note", TestEntity = testEntity01 };
                        TestEntityNote testEntityNote04 = new TestEntityNote { Note = "This is a test note", TestEntity = testEntity01 };
                        TestEntityNote testEntityNote05 = new TestEntityNote { Note = "This is a test note", TestEntity = testEntity01 };


                        dbContext.TestEntities.Add(testEntity01);
                        dbContext.TestEntities.Add(testEntity02);
                        dbContext.TestEntities.Add(testEntity03);
                        dbContext.TestEntities.Add(testEntity04);
                        dbContext.TestEntities.Add(testEntity05);
                        dbContext.TestEntities.Add(testEntity06);
                        dbContext.TestEntities.Add(testEntity07);
                        dbContext.TestEntities.Add(testEntity08);
                        dbContext.TestEntities.Add(testEntity09);
                        dbContext.TestEntities.Add(testEntity10);

                        dbContext.TestEntityNotes.Add(testEntityNote01);
                        dbContext.TestEntityNotes.Add(testEntityNote02);
                        dbContext.TestEntityNotes.Add(testEntityNote03);
                        dbContext.TestEntityNotes.Add(testEntityNote04);
                        dbContext.TestEntityNotes.Add(testEntityNote05);

                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine("EF Migrations complete");
            Console.WriteLine("Press enter to terminate");
            Console.ReadLine();

        }
    }
}
