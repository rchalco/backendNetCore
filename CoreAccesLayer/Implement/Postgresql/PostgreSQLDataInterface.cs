using CoreAccesLayer.Interface;
using DataAccess.Core.Base;
using DataAccess.Core.Decorates;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreAccesLayer.Implement.Postgresql
{
    public class PostgreSQLDataInterface
    {
        public string ConnectionString { get; set; }
        private NpgsqlCommand gCommand { get; set; }
        private NpgsqlCommand gCommandForTransaction { get; set; }
        private NpgsqlConnection gConnection { get; set; }
        private NpgsqlConnection gConnectionForTransaction { get; set; }
        private NpgsqlTransaction gTransaction { get; set; }

        private void OpenConnectionForTransaction()
        {
            try
            {
                if (gConnectionForTransaction != null)
                {
                    if (gConnectionForTransaction.State == System.Data.ConnectionState.Closed)
                    {
                        gConnectionForTransaction.Open();
                    }
                    else
                    {
                        gConnectionForTransaction = new NpgsqlConnection();
                        gConnectionForTransaction.ConnectionString = ConnectionString;
                        gConnectionForTransaction.Open();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ExecuteNonQueryForTransaction(NpgsqlCommand pCommand)
        {
            bool vResultado = false;
            try
            {
                pCommand.ExecuteNonQuery();
                vResultado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vResultado;
        }

        public bool MapTypeComplex<T>(string name_type) where T : new()
        {
            bool resul = true;
            OpenConnection();
            gConnection.TypeMapper.MapComposite<T>(name_type);
            return resul;
        }
        public List<T> GetListByProcedure<T>(string nameProcedure, params object[] parameters) where T : new()
        {
            List<T> vListado = new List<T>();

            gCommand = new NpgsqlCommand();
            gCommand.CommandText = nameProcedure;
            gCommand.Connection = gConnection;
            gCommand.CommandText = "SELECT * FROM " + nameProcedure + "(";
            string nomParametro = "parametro";
            int a = 0;
            OpenConnection();
            foreach (object item in parameters)
            {


                gCommand.CommandText += "@" + nomParametro + a.ToString();

                NpgsqlParameter parametro = null;

                if (item == null)
                {
                    parametro = new NpgsqlParameter
                    {
                        ParameterName = "@" + nomParametro + a.ToString(),
                        Value = DBNull.Value,
                        Direction = ParameterDirection.Input
                    };
                    a++;
                    gCommand.CommandText += ",";
                    gCommand.Parameters.Add(parametro);
                    continue;
                }

                parametro = new NpgsqlParameter
                {
                    ParameterName = "@" + nomParametro + a.ToString(),
                    Direction = ParameterDirection.Input
                };

                string typeParameter = item.GetType().Name;
                if (item is IEnumerable)
                {
                    parametro.Value = item;
                }
                else
                {
                    switch (typeParameter)
                    {
                        case "Int32":
                            parametro.NpgsqlDbType = NpgsqlDbType.Integer;
                            parametro.Value = item;
                            break;

                        case "Int64":
                            parametro.NpgsqlDbType = NpgsqlDbType.Bigint;
                            parametro.Value = item;
                            break;

                        case "Decimal":
                            parametro.NpgsqlDbType = NpgsqlDbType.Numeric;
                            parametro.Value = item;
                            break;

                        case "String":
                            parametro.NpgsqlDbType = NpgsqlDbType.Varchar;
                            parametro.Value = item;
                            break;

                        case "Boolean":
                            parametro.NpgsqlDbType = NpgsqlDbType.Bit;
                            parametro.Value = Convert.ToBoolean((item));
                            break;

                        case "DateTime":
                            parametro.NpgsqlDbType = NpgsqlDbType.Date;
                            parametro.Value = item;
                            break;
                        case "Byte[]":
                            parametro.NpgsqlDbType = NpgsqlDbType.Bytea;
                            parametro.Value = item;
                            break;

                        default:
                            break;
                    }

                }

                gCommand.CommandText += ",";
                a++;
                gCommand.Parameters.Add(parametro);
            }
            if (parameters.Length > 0)
            {
                gCommand.CommandText = gCommand.CommandText.Substring(0, gCommand.CommandText.Length - 1);
            }
            gCommand.CommandText += "); ";
            IDataReader dr = GetList(gCommand);

            while (dr.Read())
            {
                vListado.Add(FactoryEntity<T>(ref dr));
            }
            DisposeReader(ref dr);
            DisposeConnection();


            return vListado;
        }
        public bool BulkInsert<T>(List<T> Data, string name_type) where T : new()
        {
            bool resul = true;
            OpenConnection();
            gConnection.TypeMapper.MapComposite<T>(name_type);
            gCommand = new NpgsqlCommand();
            gCommand.Connection = gConnection;
            ///TODO rescatamos los campos del tipo
            string propertysQuery = string.Empty;
            typeof(T).GetProperties().ToList().ForEach(x =>
            {
                if (x.GetCustomAttribute(typeof(ExcludeQuery)) == null ||
                !x.GetCustomAttribute<ExcludeQuery>().Insert)
                {
                    propertysQuery += $"{x.Name},";
                }
            });
            propertysQuery = propertysQuery.Substring(0, propertysQuery.Length - 1);
            gCommand.CommandText = $@"
                INSERT INTO {typeof(T).Name} ({propertysQuery}) 
                SELECT {propertysQuery} from UNNEST(@data);";
            NpgsqlParameter data = new NpgsqlParameter(
                            "data",
                            Data);
            gCommand.Parameters.Add(data);
            gCommand.ExecuteNonQuery();
            return resul;
        }
        private IDataReader GetList(NpgsqlCommand pCommand)
        {
            IDataReader vDataReader = null;
            NpgsqlCommand vCommand = pCommand;
            OpenConnection();
            vCommand.Connection = gConnection;

            try
            {
                vDataReader = vCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vDataReader;
        }
        private void OpenConnection()
        {
            try
            {
                if (gConnection != null)
                {
                    if (gConnection.State == System.Data.ConnectionState.Closed)
                    {
                        gConnection.Open();
                    }
                }
                else
                {
                    gConnection = new NpgsqlConnection();
                    gConnection.ConnectionString = ConnectionString;
                    gConnection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisposeConnection()
        {
            try
            {
                if (gConnection != null)
                {
                    if (gConnection.State != System.Data.ConnectionState.Closed)
                        gConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisposeReader(ref IDataReader pDataReader)
        {
            try
            {
                if (pDataReader != null)
                {
                    if (!pDataReader.IsClosed)
                        pDataReader.Close();
                    pDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Libera recursos del comando.
        /// </summary>
        private void DisposeCommand()
        {
            try
            {
                if (gCommand != null)
                    gCommand.Dispose();
                gCommand = null;
            }
            catch
            {
                throw;
            }
        }

        private T FactoryEntity<T>(ref IDataReader pDataReader) where T : Entity, new()
        {
            T vEntity = new T();
            var vPropiedades = vEntity.GetType().GetProperties();

            try
            {
                PropertyInfo[] propiedades = new PropertyInfo[vEntity.GetType().GetProperties().Count()];
                vEntity.GetType().GetProperties().CopyTo(propiedades, 0);
                propiedades = propiedades.Where(a => !typeof(Entity).GetProperties().Any(b => b.Name == a.Name)).ToArray();
                DataTable dtSchema = pDataReader.GetSchemaTable();

                foreach (PropertyInfo item in propiedades)
                {
                    if (!dtSchema.AsEnumerable().Any(a => a.Field<string>("ColumnName").Equals(item.Name)))
                    {
                        continue;
                    }
                    if (pDataReader[item.Name] == null ||
                        pDataReader[item.Name] == DBNull.Value)
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, null, null);
                    }
                    else if (vEntity.GetType().GetProperty(item.Name).PropertyType.FullName.Contains("Int32"))
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, Convert.ToInt32(pDataReader[item.Name].ToString()), null);
                    }
                    else if (vEntity.GetType().GetProperty(item.Name).PropertyType.FullName.Contains("Int64"))
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, Convert.ToInt64(pDataReader[item.Name].ToString()), null);
                    }
                    else if (vEntity.GetType().GetProperty(item.Name).PropertyType.FullName.Contains("Decimal"))
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, Convert.ToDecimal(pDataReader[item.Name]), null);
                    }
                    else if (vEntity.GetType().GetProperty(item.Name).PropertyType.FullName.Contains("Single"))
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, Convert.ToSingle(pDataReader[item.Name]), null);
                    }
                    else if (vEntity.GetType().GetProperty(item.Name).PropertyType.FullName.Contains("StringBuilder"))
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, new StringBuilder(pDataReader[item.Name].ToString()), null);
                    }
                    else if (vEntity.GetType().GetProperty(item.Name).PropertyType.FullName.Contains("String"))
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, pDataReader[item.Name].ToString(), null);
                    }
                    else if (vEntity.GetType().GetProperty(item.Name).PropertyType.FullName.Contains("Boolean"))
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, Convert.ToBoolean(pDataReader[item.Name]), null);
                    }
                    else if (vEntity.GetType().GetProperty(item.Name).PropertyType.FullName.Contains("DateTime"))
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, Convert.ToDateTime(pDataReader[item.Name]), null);
                    }
                    else if (vEntity.GetType().GetProperty(item.Name).PropertyType.FullName.Contains("Byte[]"))
                    {
                        vEntity.GetType().GetProperty(item.Name).SetValue(vEntity, (byte[])(pDataReader[item.Name]), null);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vEntity;
        }


    }
}
