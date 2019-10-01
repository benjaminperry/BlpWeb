using BES.Database.Entities;
using BlpEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BlpData
{
    public class BlpContext : CoreContext
    {
        public BlpContext(DbContextOptions<BlpContext> options, IEnumerable<IBuildModel> modelBuilders) : base(options, modelBuilders)
        {
            
        }
    }
}
