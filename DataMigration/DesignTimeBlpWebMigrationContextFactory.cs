using Microsoft.EntityFrameworkCore.Design;
using StructureMap;

namespace DataMigration
{
    public class DesignTimeBlpWebMigrationContextFactory : IDesignTimeDbContextFactory<DataMigrationContext>
    {
        public DataMigrationContext CreateDbContext(string[] args)
        {
#pragma warning disable IDE0067 // Dispose objects before losing scope
            return new Container(new DataMigrationServicesRegistry()).GetInstance<DataMigrationContext>();
#pragma warning restore IDE0067 // Dispose objects before losing scope
        }
    }
}
