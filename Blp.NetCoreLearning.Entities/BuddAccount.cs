using System.ComponentModel.DataAnnotations;

namespace Blp.NetCoreLearning.Entities
{
    public class BuddAccount
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Number { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
