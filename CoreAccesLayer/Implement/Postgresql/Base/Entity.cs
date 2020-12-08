using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Core.Base
{
    public enum StateEntity
    {
        none = 0,
        add = 1,
        modify = 2,
        remove = 3
    }
    [DataContract]
    public abstract class Entity
    {
        public StateEntity stateEntity { get; set; }
    }
}
