using System.Collections.Generic;

namespace Blp.NetCoreLearning.Entities
{
    public class TestEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<TestEntityNote> TestEntityNotes { get; set; }
    }
}