using System;
using System.Collections.Generic;
using System.Text;

namespace BlpEntities
{
    public class TestEntityNote
    {
        public int Id { get; set; }
        public TestEntity TestEntity { get; set; }
        public string Note { get; set; }
    }
}
