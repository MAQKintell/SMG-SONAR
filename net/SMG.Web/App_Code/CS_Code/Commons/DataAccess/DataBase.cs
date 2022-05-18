using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Iberdrola.Commons.Exceptions;

namespace Iberdrola.Commons.DataAccess
{
    /// <summary>
    /// ADO.NET data access using the SQL Server Managed Provider.
    /// </summary>
    public class DataBase : IDisposable
    {
        // connection to data source
        private SqlConnection _con;
        private String _connectionString = String.Empty;

        public String ConnectionString
        {
            get { return this._connectionString; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <exception cref="Iberdrola.Commons.Exceptions.ArqException"> Si el connectionString
        /// es vacío
        /// </exception>
        public DataBase(String connectionString)
        {
            if (String.Empty.Equals(connectionString))
            {
                throw new ArqException(false, "1002");
            }
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <returns>Stored procedure return value.</returns>
        public int RunProc(string procName)
        {
            try
            {
                SqlCommand cmd = CreateCommand(procName, null);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["ReturnValue"].Value;
            }
            catch (ArqException exArq)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1003");
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <returns>Stored procedure return value.</returns>
        public int RunProc(string procName, SqlParameter[] prams)
        {
            try
            {
                SqlCommand cmd = CreateCommand(procName, prams);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["ReturnValue"].Value;
            }
            catch (ArqException exArq)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1003");
            }
            finally
            {
                this.Close();
            }

        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public void RunProc(string procName, out SqlDataReader dataReader)
        {
            try
            {
                SqlCommand cmd = CreateCommand(procName, null);
                dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (ArqException exArq)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1003");
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public void RunProc(string procName, SqlParameter[] prams, out SqlDataReader dataReader)
        {

            try
            {
                SqlCommand cmd = CreateCommand(procName, prams);
                dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (ArqException exArq)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1003");
            }
            finally
            {
                this.Close();
            }
        }


        /// <summary>
        /// Create command object used to call stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Params to stored procedure.</param>
        /// <returns>Command object.</returns>
        private SqlCommand CreateCommand(string procName, SqlParameter[] prams)
        {
            int i = 0;
            int j = 1;
            int x = j / i;
            // make sure connection is open
            Open();

            SqlCommand cmd = new SqlCommand(procName, _con);
            cmd.CommandType = CommandType.StoredProcedure;
            

            // add proc parameters
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }

            // return param
            cmd.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));

            return cmd;
        }

        /// <summary>
        /// Open the connection.
        /// </summary>
        private void Open()
        {
            try
            {
                // open connection
                if (this._con == null)
                {
                    this._con = new SqlConnection(this._connectionString);
                    this._con.Open();
                }
            }
            catch(Exception ex)
            {
                throw new ArqException(false, ex, "1004");
            }
        }

        /// <summary>
        /// Close the connection.
        /// </summary>
        public void Close()
        {
            try
            {
                if (this._con != null && this._con.State != ConnectionState.Closed)
                {
                    this._con.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1005");
            }
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                // make sure connection is closed
                this.Close();
                this._con.Dispose();
                this._con = null;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1005");
            }

        }

        /// <summary>
        /// Make input param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// Make input param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// Make stored procedure param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Direction">Parm direction.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size,
         ParameterDirection Direction, object Value)
        {
            try
            {
                SqlParameter param;

                if (Size > 0)
                {
                    param = new SqlParameter(ParamName, DbType, Size);
                }
                else
                {
                    param = new SqlParameter(ParamName, DbType);
                }
                param.Direction = Direction;

                if (!(Direction == ParameterDirection.Output && Value == null))
                {
                    param.Value = Value;
                }
                return param;
            }
            catch (Exception ex)
            {
                throw new ArqException(false, ex, "1006");
            }
        }

        public DataTable GetDataTable(string Sql, string ConnectString)
        {
            
            SqlDataAdapter da = new SqlDataAdapter(Sql, ConnectString);
            DataTable dt = new DataTable();

            int rows = da.Fill(dt);

            return dt;
        }
    }
}

