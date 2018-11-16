using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlpWebApp.Models
{
    public class BuddAccountDetailModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Number")]
        public string Number { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
