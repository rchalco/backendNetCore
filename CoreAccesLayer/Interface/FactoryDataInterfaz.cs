using CoreAccesLayer.Implement;
using CoreAccesLayer.Implement.MySQL;
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
        public static IRepository CreateRepository<T>(string provider) where T : DbContext, new()
        {
            IRepository repository = new PostgreSQLRepository<T>();
            switch (provider)
            {
                case "postgresql":
                    repository = new PostgreSQLRepository<T>();
                    break;
                case "mysql":
                    repository = new MySQLRepository<T>();
                    break;
                default:
                    repository = new PostgreSQLRepository<T>();
                    break;
            }
            
            return repository;
        }

    }
}
