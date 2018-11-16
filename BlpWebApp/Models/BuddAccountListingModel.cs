using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlpWebApi.Models
{
    public class BuddAccountListingModel
    {
        public IList<BuddAccountDetailModel> Listing { get; set; }

    }
}
