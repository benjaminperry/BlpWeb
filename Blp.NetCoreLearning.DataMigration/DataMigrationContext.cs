using Blp.NetCoreLearning.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Blp.NetCoreLearning.DataMigration
{
    public class DataMigrationContext : CoreContext
    {
        public DataMigrationContext(DbContextOptions<DataMigrationContext> options, IEnumerable<IBuildModel> modelBuilders) : base(options, modelBuilders)
        { 
            
        }
    }
}
