using System;
using System.Runtime.Serialization;

namespace DataAccess.Core.Decorates
{
    [DataContract]
    public class ExcludeQuery : Attribute
    {
        public bool Insert { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}
