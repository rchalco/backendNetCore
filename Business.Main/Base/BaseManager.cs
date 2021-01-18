using Business.Main.DbContextMySQL;
using Business.Main.DbContextSample;
using CoreAccesLayer.Interface;
using Domain.Main.Wraper;
using PlumbingProps.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Base
{
    public abstract class BaseManager
    {
        internal IRepository repositoryPostreSql { get; set; } = null;
        internal IRepository repositoryMySql { get; set; } = null;
        public BaseManager()
        {
            repositoryPostreSql = FactoryDataInterfaz.CreateRepository<sample_dbContext>("postgresql");
            repositoryMySql = FactoryDataInterfaz.CreateRepository<ibnorca_mokContext>("mysql");
        }

        public string ProcessError(Exception ex)
        {
            ManagerException managerException = new ManagerException();
            return managerException.ProcessException(ex);
        }

        public string ProcessError(Exception ex, Response response)
        {
            ManagerException managerException = new ManagerException();
            response.State = ResponseType.Error;
            response.Message = managerException.ProcessException(ex);
            return managerException.ProcessException(ex);
        }
    }

}
