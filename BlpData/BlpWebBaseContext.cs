using Microsoft.EntityFrameworkCore;
using System;
using BlpEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlpData
{
    public class BlpWebBaseContext : IdentityDbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }
        public DbSet<TestEntityNote> TestEntityNotes { get; set; }
        public DbSet<BuddAccount> BuddAccounts { get; set; }

        public BlpWebBaseContext(DbContextOptions<BlpWebBaseContext> options) : base(options)
        { }
    }
}
