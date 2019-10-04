using Microsoft.EntityFrameworkCore;

namespace Blp.NetCoreLearning.Entities
{
    public interface IBuildModel
    {
        void Build(ModelBuilder modelBuilder);
    }
}
