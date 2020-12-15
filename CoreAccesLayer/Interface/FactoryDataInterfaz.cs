using CoreAccesLayer.Implement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccesLayer.Interface
{
    public static class FactoryDataInterfaz
    {
        public static IRepository CreateRepository<T>() where T : DbContext, new()
        {
            IRepository repository = new PostgreSQLRepository<T>();
            return repository;
        }
    }
}
