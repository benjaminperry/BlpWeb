using System;
using System.Collections.Generic;
using System.Text;

namespace BlpEntities
{
    public class TestEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<TestEntityNote> TestEntityNotes { get; set; }
    }
}