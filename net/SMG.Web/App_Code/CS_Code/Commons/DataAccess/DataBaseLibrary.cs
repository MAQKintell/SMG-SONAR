using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using Iberdrola.Commons.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Data;


using System.IO;

namespace Iberdrola.Commons.DataAccess
{
    public class DataBaseLibrary : IDisposable
    {
        private Database _db;
        private String _name = String.Empty;

        private static Hashtable dbTypeTable;

        public static DbType ConvertToDbType(object obj)
        {
            if (dbTypeTable == null)
            {
                dbTypeTable = new Hashtable();
                dbTypeTable.Add(typeof(System.Boolean), DbType.Boolean);
                dbTypeTable.Add(typeof(System.Int16), DbType.Int16);
                dbTypeTable.Add(typeof(System.Int32), DbType.Int32);
                dbTypeTable.Add(typeof(System.Int64), DbType.Int64);
                dbTypeTable.Add(typeof(System.Double), DbType.Double);
                dbTypeTable.Add(typeof(System.Decimal), DbType.Decimal);
                dbTypeTable.Add(typeof(System.String), DbType.String);
                dbTypeTable.Add(typeof(System.DateTime), DbType.DateTime);
                dbTypeTable.Add(typeof(System.Byte), DbType.Byte);
                dbTypeTable.Add(typeof(System.Guid), DbType.Guid);
                
            }
            DbType dbtype;
            try
            {
                if (obj != null)
                {
                    dbtype = (DbType)dbTypeTable[obj.GetType()];
                }
                else
                {
                    dbtype = DbType.Object;
                }
                
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1008");
            }
            return dbtype;
        }

        public String Name
        {
            get { return this._name; }
        }

        public DataBaseLibrary(String name)
        {

            if (String.Empty.Equals(name))
            {
                throw new ArqException(false, "1002");
            }

            try
            {
                this._name = name;
                this._db = DatabaseFactory.CreateDatabase(_name);
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1009");
            }
        }

        public int RunQueryNonValue(string query)
        {
            try
            {
                DbCommand dbCommand = this._db.GetSqlStringCommand(query);
                dbCommand.CommandTimeout=0;
                return this._db.ExecuteNonQuery(dbCommand);
            }
            catch (ArqException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1010");
            }
        }

        public IDataReader RunQueryDataReader(string query, String[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            try
            {
                if (paramsName != null && paramsType != null && paramsValue != null)
                {
                    DbCommand dbCommand = this._db.GetSqlStringCommand(query);
                    dbCommand.CommandTimeout = 0;
                    for (int i = 0; i < paramsName.Length; i++)
                    {
                        this._db.AddInParameter(dbCommand, paramsName[i], paramsType[i], paramsValue[i]);
                    }
                    return this._db.ExecuteReader(dbCommand);
                }
                else
                {
                    return this.RunQueryDataReader(query);
                }
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1010");
            }
        }

        public IDataReader RunQueryDataReader(string query)
        {
            try
            {
                DbCommand dbCommand = this._db.GetSqlStringCommand(query);
                dbCommand.CommandTimeout = 0;
                return this._db.ExecuteReader(dbCommand);
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1010");
            }
        }

        public DataTable RunQueryDataTable(string query, String[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            try
            {
                if (paramsName != null && paramsType != null && paramsValue != null)
                {
                    DbCommand dbCommand = this._db.GetSqlStringCommand(query);
                    dbCommand.CommandTimeout = 0;
                    for (int i = 0; i < paramsName.Length; i++)
                    {
                        this._db.AddInParameter(dbCommand, paramsName[i], paramsType[i], paramsValue[i]);
                    }
                    DataSet ds = this._db.ExecuteDataSet(dbCommand);
                    if ((ds != null) && (ds.Tables.Count > 0))
                    {
                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return this.RunQueryDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1010");
            }
        }

        public DataTable RunQueryDataTable(string query)
        {
            try
            {
                DbCommand dbCommand = this._db.GetSqlStringCommand(query);
                dbCommand.CommandTimeout = 0;
                DataSet ds = this._db.ExecuteDataSet(dbCommand);
                if ((ds != null) && (ds.Tables.Count > 0))
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1010");
            }
        }

        public Object RunQueryDataScalar(string query, String[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            try
            {
                if (paramsName != null && paramsType != null && paramsValue != null)
                {
                    DbCommand dbCommand = this._db.GetSqlStringCommand(query);
                    dbCommand.CommandTimeout = 0;
                    for (int i = 0; i < paramsName.Length; i++)
                    {
                        this._db.AddInParameter(dbCommand, paramsName[i], paramsType[i], paramsValue[i]);
                    }
                    return this._db.ExecuteScalar(dbCommand);
                }
                else
                {
                    return RunQueryDataScalar(query);
                }

            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1010");
            }
        }

        public Object RunQueryDataScalar(string query)
        {
            try
            {
                DbCommand dbCommand = this._db.GetSqlStringCommand(query);
                dbCommand.CommandTimeout = 0;
                return this._db.ExecuteScalar(dbCommand);

            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1010");
            }
        }

        public object RunProcEscalar(string procName)
        {
            try
            {
                DbCommand dbCommand = this._db.GetStoredProcCommand(procName);
                dbCommand.CommandTimeout = 0;
                return this._db.ExecuteScalar(dbCommand);
            }
            catch (ArqException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1003");
            }
        }

        public object RunProcEscalar(string procName, String[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            try
            {
                if (paramsName != null && paramsType != null && paramsValue != null)
                {
                    DbCommand dbCommand = this._db.GetStoredProcCommand(procName);
                    dbCommand.CommandTimeout = 0;
                    for (int i = 0; i < paramsName.Length; i++)
                    {
                        this._db.AddInParameter(dbCommand, paramsName[i], paramsType[i], paramsValue[i]);
                    }
                    return this._db.ExecuteScalar(dbCommand);
                }
                else
                {
                    return this.RunProcEscalar(procName);
                }
            }
            catch (ArqException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1003");
            }

        }

        public IDataReader RunProcDataReader(string procName)
        {
            try
            {
                DbCommand dbCommand = this._db.GetStoredProcCommand(procName);
                dbCommand.CommandTimeout = 0;
                return this._db.ExecuteReader(dbCommand);
            }
            catch (ArqException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1003");
            }
        }

        public IDataReader RunProcDataReader(string procName, String[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            try
            {
                if (paramsName != null && paramsType != null && paramsValue != null)
                {
                    DbCommand dbCommand = this._db.GetStoredProcCommand(procName);
                    dbCommand.CommandTimeout = 0;
                    for (int i = 0; i < paramsName.Length; i++)
                    {
                        this._db.AddInParameter(dbCommand, paramsName[i], paramsType[i], paramsValue[i]);
                    }
                    return this._db.ExecuteReader(dbCommand);
                }
                else
                {
                    return this.RunProcDataReader(procName);
                }
            }
            catch (ArqException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1003");
            }

        }

        /// <summary>
        /// Ejecuta una un procedimiento almacenado que no devuelve resultado con parámetros.
        /// </summary>
        /// <param name="query">Query a ejecutar.</param>
        /// <param name="paramsName">Array con los nombres de los parámetros.</param>
        /// <param name="paramsType">Array con los tipos de los parámetros.</param>
        /// <param name="paramsValue">Array con los valores de los parámetros.</param>
        public void RunProcNonQuery(string query, string[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            try
            {
                // Preparamos la llamada al procedimiento.
                DbCommand dbCommand = this._db.GetSqlStringCommand(query);

                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 0;

                // Si tenemos parámetros lo añadimos al dbCommand.
                if (paramsName != null)
                {
                    for (int i = 0; i < paramsName.Length; i++)
                    {
                        this._db.AddInParameter(dbCommand, paramsName[i], paramsType[i], paramsValue[i]);
                    }
                }

                // Ejecutamos el procedimento
                this._db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1010");
            }
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado con parámetros.
        /// </summary>
        /// <param name="procName">Nombre del procedimiento almacenado.</param>
        /// <param name="paramsName">Array con los nombres de los parámetros.</param>
        /// <param name="paramsType">Array con los tipos de los parámetros.</param>
        /// <param name="paramsValue">Array con los valores de los parámetros.</param>
        /// <returns>DataTable con el resultado de la ejecución de la consulta.</returns>
        public DataTable RunProcDataTable(string procName, string[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            try
            {
                // Preparamos la llamada a la consulta.
                DbCommand dbCommand = this._db.GetStoredProcCommand(procName);
                dbCommand.CommandTimeout = 0;

                // Si tenemos parámetros lo añadimos al dbCommand.
                if (paramsName != null)
                {
                    for (int i = 0; i < paramsName.Length; i++)
                    {
                        this._db.AddInParameter(dbCommand, paramsName[i], paramsType[i], paramsValue[i]);
                    }
                }

                // Realizamos la ejecución del procedimiento almacenado.
                DataSet ds = this._db.ExecuteDataSet(dbCommand);

                // Si hemos encontrado datos devolvemos el DataTable.
                if ((ds != null) && (ds.Tables.Count > 0))
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (ArqException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1003");
            }
        }

        public void Dispose()
        {
            try
            {
                this._db = null;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1005");
            }

        }

    }
}