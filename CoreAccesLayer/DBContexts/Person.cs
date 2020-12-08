using System;
using System.Collections.Generic;

namespace CoreAccesLayer.DbContexts
{
    public partial class Person
    {
        public long PersonId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
