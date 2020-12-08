using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Core.Decorates
{
    [DataContract]
    public class FieldValidation : Attribute
    {
        [DataMember]
        public bool IsRequired { get; set; }
        [DataMember]
        public int MinValue { get; set; }
        [DataMember]
        public int MaxValue { get; set; }
        [DataMember]
        public int MinLength { get; set; }
        [DataMember]
        public int MaxLength { get; set; }
    }
}
