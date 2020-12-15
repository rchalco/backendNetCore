using CoreAccesLayer.Implement.Postgresql;
using CoreAccesLayer.Interface;
using CoreAccesLayer.Wraper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccesLayer.Implement
{
    public class PostgreSQLRepository<TDbContext> : IRepository
        where TDbContext : DbContext, new()
    {
        PostgreSQLDataInterface postgreSQLDataInterface = new PostgreSQLDataInterface
        {
            ConnectionString = "Host=localhost;Database=sample_db;Username=postgres;Password=admin123"
        };
        TDbContext _dbContext = new TDbContext();

        public bool CallProcedure<T>(string nameProcedure, params object[] parameters) where T : class, new()
        {
            ///TODO: en postgres no existe los procedimientos almacenados
            throw new NotImplementedException("El motor de Postgres no implementa los procedimientos almacenados");
        }

        public List<T> GetDataByProcedure<T>(string nameProcedure, params object[] parameters) where T : class, new()
        {
            return postgreSQLDataInterface.GetListByProcedure<T>(nameProcedure, parameters);
        }

        public bool SaveObject<T>(Entity<T> entity) where T : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentException("el parametro a operar no puede ser nulo");
            }
            else if (entity.stateEntity == StateEntity.none)
            {
                throw new ArgumentException("no se definio un estado para la entidad");
            }
            else if (entity.EntityDB == null)
            {
                throw new ArgumentException("no se tiene una entidad valida!, entidad interna nula");
            }
            else if (entity.stateEntity == StateEntity.add)
            {
                _dbContext.Add(entity.EntityDB);
            }
            else if (entity.stateEntity == StateEntity.modify)
            {
                _dbContext.Update(entity.EntityDB);
            }
            else if (entity.stateEntity == StateEntity.remove)
            {
                _dbContext.Remove(entity.EntityDB);
            }
            _dbContext.SaveChanges();
            return true;
        }
    }
}
