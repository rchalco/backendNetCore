using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DbContextMySQL
{
    public partial class Person
    {
        public int Idperson { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
    }
}
