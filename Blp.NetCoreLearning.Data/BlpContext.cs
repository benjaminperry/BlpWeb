using Blp.NetCoreLearning.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Blp.NetCoreLearning.Data
{
    public class BlpContext : CoreContext
    {
        public BlpContext(DbContextOptions<BlpContext> options, IEnumerable<IBuildModel> modelBuilders) : base(options, modelBuilders)
        {
            
        }
    }
}
