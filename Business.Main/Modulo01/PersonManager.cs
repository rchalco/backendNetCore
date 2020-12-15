using Business.Main.Base;
using Business.Main.DbContextSample;
using CoreAccesLayer.Wraper;
using Domain.Main.Modulo01;
using Domain.Main.Wraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modulo01
{
    public class PersonManager : BaseManager
    {
        public ResponseObject<Person> RegisterPerson(Person person)
        {
            ResponseObject<Person> response = new ResponseObject<Person>();
            try
            {
                //Logica del negocio
                if (person == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "el parametro de la persona a registrar no debe ser nulo";
                    response.Object = null;
                    response.Code = "404";
                    return response;
                }
                if (person.Name.Length < 2 || person.LastName.Length < 2)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "el nombre y apellido deben tener mas de dos caracteres";
                    response.Object = null;
                    response.Code = "403";
                    return response;
                }
                Entity<Person> entity = new Entity<Person> { EntityDB = person, stateEntity = StateEntity.add };
                if (person.PersonId != 0)
                {
                    entity.stateEntity = StateEntity.modify;
                }
                repository.SaveObject<Person>(entity);
                response.State = ResponseType.Success;
                response.Message = "La persona fue registrada correctamente";
                response.Object = person;
                response.Code = "000";
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseObject<Person> DeletePerson(Person person)
        {
            ResponseObject<Person> response = new ResponseObject<Person>();
            try
            {
                //Logica del negocio
                if (person == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "el parametro de la persona a registrar no debe ser nulo";
                    response.Object = null;
                    response.Code = "404";
                    return response;
                }
                Entity<Person> entity = new Entity<Person> { EntityDB = person, stateEntity = StateEntity.remove };
                repository.SaveObject<Person>(entity);
                response.State = ResponseType.Success;
                response.Message = "La persona fue eliminada correctamente";
                response.Object = person;
                response.Code = "000";
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<PersonReport> GetPersons(string name)
        {
            ResponseQuery<PersonReport> response = new ResponseQuery<PersonReport>();
            try
            {
                //Logica del negocio
                response.ListEntities =  repository.GetDataByProcedure<PersonReport>("\"getPersons\"", name);
                response.State = ResponseType.Success;
                response.Message = "La persona fue eliminada correctamente";                
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
    }
}
