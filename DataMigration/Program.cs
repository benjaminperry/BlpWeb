using BlpEntities;
using Microsoft.EntityFrameworkCore;
using StructureMap;
using System;
using System.Linq;

namespace DataMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running EF Migrations");
            try
            {
                var container = new Container(new DataMigrationServicesRegistry());

                using (DataMigrationContext migrationContext = container.GetInstance<DataMigrationContext>())
                {
                    Console.WriteLine();
                    Console.WriteLine($"New migrations:");

                    var newMigrations = migrationContext.Database.GetPendingMigrations();
                    if (!newMigrations.Any())
                        Console.WriteLine("There are no new migrations to apply");

                    foreach (var newMigration in newMigrations)
                        Console.WriteLine($"  - {newMigration}");

                    migrationContext.Database.Migrate();

                    Console.WriteLine();
                    Console.WriteLine($"Applied migrations:");
                    foreach (var appliedMigration in migrationContext.Database.GetAppliedMigrations())
                        Console.WriteLine($"  - {appliedMigration}");

                    Console.WriteLine();
                    Console.WriteLine("EF Migrationss complete");

                    if (!migrationContext.TestEntities.Any())
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


                        migrationContext.TestEntities.Add(testEntity01);
                        migrationContext.TestEntities.Add(testEntity02);
                        migrationContext.TestEntities.Add(testEntity03);
                        migrationContext.TestEntities.Add(testEntity04);
                        migrationContext.TestEntities.Add(testEntity05);
                        migrationContext.TestEntities.Add(testEntity06);
                        migrationContext.TestEntities.Add(testEntity07);
                        migrationContext.TestEntities.Add(testEntity08);
                        migrationContext.TestEntities.Add(testEntity09);
                        migrationContext.TestEntities.Add(testEntity10);

                        migrationContext.TestEntityNotes.Add(testEntityNote01);
                        migrationContext.TestEntityNotes.Add(testEntityNote02);
                        migrationContext.TestEntityNotes.Add(testEntityNote03);
                        migrationContext.TestEntityNotes.Add(testEntityNote04);
                        migrationContext.TestEntityNotes.Add(testEntityNote05);

                        migrationContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine("Press enter to terminate");
            Console.ReadLine();

        }
    }
}
