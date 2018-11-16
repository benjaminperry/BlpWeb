using Microsoft.EntityFrameworkCore;
using System;
using BlpEntities;

namespace BlpData
{
    public class BlpWebBaseContext : DbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }
        public DbSet<TestEntityNote> TestEntityNotes { get; set; }
        public DbSet<BuddAccount> BuddAccounts { get; set; }

        public BlpWebBaseContext(DbContextOptions<BlpWebBaseContext> options) : base(options)
        { }
    }
}
