using BES.Database.Entities;
using BlpEntities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataMigration
{
    public class DataMigrationContext : CoreContext
    {
        public DataMigrationContext(DbContextOptions<DataMigrationContext> options, IEnumerable<IBuildModel> modelBuilders) : base(options, modelBuilders)
        { 
            
        }
    }
}
