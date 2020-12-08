using DataAccess.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccesLayer.Interface
{
    public interface IRepository
    {
        bool SaveObject<T>(T entity) where T : Entity;
        bool CallProcedure<T>(string nameProcedure, params object[] parameters) where T : class;
        List<T> GetDataByProcedure<T>(string nameProcedure, params object[] parameters) where T : new();

    }
}
