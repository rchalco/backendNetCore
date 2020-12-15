using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DbContextSample
{
    public partial class Person
    {
        public long PersonId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
