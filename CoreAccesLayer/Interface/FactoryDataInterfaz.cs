using CoreAccesLayer.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccesLayer.Interface
{
    public static class FactoryDataInterfaz
    {        
        public static IRepository CreateRepository<T>()
        {
            IRepository repository = new PostgreSQLRepository();
            return repository;
        }
    }
}
