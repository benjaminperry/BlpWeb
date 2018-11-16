using BlpData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMigration
{
    class BlpWebMigrationContext : BlpWebBaseContext
    {
        public BlpWebMigrationContext(DbContextOptions<BlpWebBaseContext> options) : base(options)
        { }
    }
}
