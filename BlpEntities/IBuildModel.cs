using Microsoft.EntityFrameworkCore;

namespace BES.Database.Entities
{
    public interface IBuildModel
    {
        void Build(ModelBuilder modelBuilder);
    }
}