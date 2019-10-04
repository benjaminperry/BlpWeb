using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Blp.NetCoreLearning.Entities
{
    public abstract class CoreContext : IdentityDbContext
    {
        private readonly IEnumerable<IBuildModel> _modelBuilders;

        public DbSet<TestEntity> TestEntities { get; set; }
        public DbSet<TestEntityNote> TestEntityNotes { get; set; }
        public DbSet<BuddAccount> BuddAccounts { get; set; }

        public CoreContext(DbContextOptions options, IEnumerable<IBuildModel> modelBuilders) : base(options)
        {
            Console.WriteLine($"{this.GetType().Name.ToUpper()} CONTRUCTED AT: {DateTime.Now}");
            _modelBuilders = modelBuilders;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Any Entity that has special builder commands should be marked with IBuildModel
            foreach (var entityBuilder in _modelBuilders)
            {
                entityBuilder.Build(modelBuilder);
            }
        }
    }
}
