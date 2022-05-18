using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Iberdrola.Commons.DataAccess;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Exceptions;

public class DataBaseUtils
{
	private DataBaseUtils()
	{
	}

    public static object GetDataReaderColumnValue(IDataReader dr, String columnName)
    {
        if (!dr[columnName].Equals(DBNull.Value))
        {
            return dr[columnName];
        }
        else
        {
            return null;
        }
    }

    public static object GetDataRowColumnValue(DataRow dr, String columnName)
    {
        if (!dr[columnName].Equals(DBNull.Value))
        {
            return dr[columnName];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Retorna si el IDataReader contiene la columna o no
    /// </summary>
    /// <param name="dr">IDataReader con los datos entre los que buscar.</param>
    /// <param name="columnName">Nombre de la columna a buscar.</param>
    /// <returns>booleano que indica si el IDataReader contiene o no la columna buscada</returns>
    public static bool HasColumn(IDataReader dr, string columnName)
    {
        try
        {
            return dr.GetOrdinal(columnName) >= 0;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
    }


    public static DataTable DevolverQuery(String SQL)
    {
        try
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunQueryDataTable(SQL);
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DALException(false, ex, "2006");
        }
    }
}
