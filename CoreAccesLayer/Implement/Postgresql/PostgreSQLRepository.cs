using CoreAccesLayer.DbContexts;
using CoreAccesLayer.Implement.Postgresql;
using CoreAccesLayer.Interface;
using DataAccess.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccesLayer.Implement
{
    public class PostgreSQLRepository : IRepository
    {
        PostgreSQLDataInterface postgreSQLDataInterface = new PostgreSQLDataInterface
        {
            ConnectionString = "Host=localhost;Database=sample_db;Username=postgres;Password=admin123"
        };
        sample_dbContext _sample_DbContext = new sample_dbContext();

        public bool CallProcedure<T>(string nameProcedure, params object[] parameters) where T : class
        {
            throw new NotImplementedException();
        }

        public List<T> GetDataByProcedure<T>(string nameProcedure, params object[] parameters) where T : new()
        {
            return postgreSQLDataInterface.GetListByProcedure<T>(nameProcedure, parameters);
        }

        public bool SaveObject<T>(T entity) where T : Entity
        {
            if (entity == null)
            {
                throw new ArgumentException("el parametro a operar no puede ser nulo");
            }
            else if (entity.stateEntity == StateEntity.none)
            {
                throw new ArgumentException("no se definio un estado para la entidad");
            }
            else if (entity.stateEntity == StateEntity.add)
            {
                _sample_DbContext.Add(entity);
            }
            else if (entity.stateEntity == StateEntity.modify)
            {
                _sample_DbContext.Update(entity);
            }
            else if (entity.stateEntity == StateEntity.remove)
            {
                _sample_DbContext.Remove(entity);
            }
            _sample_DbContext.SaveChanges();
            return true;
        }
    }
}
