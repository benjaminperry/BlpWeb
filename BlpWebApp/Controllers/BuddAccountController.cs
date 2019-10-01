using BlpData;
using BlpEntities;
using BlpWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlpWebApp.Controllers
{
    [Authorize]
    public class BuddAccountController : Controller
    {
        private BlpContext blpContext;

        public BuddAccountController(BlpContext blpContext)
        {
            this.blpContext = blpContext;
        }
        
        public async Task<ActionResult> Listing(string sortOrder)
        {
            // Default sort is Name Asc
            ViewData["CurrentSortOrder"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrWhiteSpace(sortOrder) ? "Name_desc" : "";
            ViewData["NumberSortParam"] = sortOrder == "Number" ? "Number_desc" : "Number";
            ViewData["IsActiveSortParam"] = sortOrder == "IsActive" ? "IsActive_desc" : "IsActive";

            string sortProp = "Name";
            bool asc = true;

            if (!string.IsNullOrWhiteSpace(sortOrder))
            {

                string[] sortOrderParts = sortOrder.Split("_");
                sortProp = sortOrderParts[0];

                if (! new string[] { "Name", "Number", "IsActive" }.Contains(sortProp))
                {
                    return BadRequest();
                }

                if(sortOrderParts.Length > 1)
                {
                    if(sortOrderParts[1] == "desc")
                    {
                        asc = false;
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            
            IOrderedQueryable<BuddAccount> buddAccounts;

            if(asc)
            {
                buddAccounts = blpContext.BuddAccounts.OrderBy(e => EF.Property<object>(e, sortProp));
            }
            else
            {
                buddAccounts = blpContext.BuddAccounts.OrderByDescending(e => EF.Property<object>(e, sortProp));
            }

            IList<BuddAccountDetailModel> forView = await buddAccounts.
                AsNoTracking().
                Select((x) => new BuddAccountDetailModel {Id = x.Id, Name = x.Name, Number = x.Number, IsActive = x.IsActive}).
                ToListAsync();

            BuddAccountListingModel model = new BuddAccountListingModel();
            model.Listing = forView;

            return View(model);
        }

        public async Task<ActionResult> Detail(int id, string sortOrder)
        {
            ViewData["CurrentSortOrder"] = sortOrder;

            BuddAccount fromDb = await blpContext.BuddAccounts.
                AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == id);

            if(fromDb == null)
            {
                return NotFound();
            }

            BuddAccountDetailModel forView = 
                new BuddAccountDetailModel {Id = fromDb.Id, Name = fromDb.Name, Number = fromDb.Number, IsActive = fromDb.IsActive };

            return View(forView);
        }
    }
}
