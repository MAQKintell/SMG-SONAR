using Iberdrola.Commons.DataAccess;
using System.Data;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de ProveedorDB
    /// </summary>
    public class ProveedorDB : BaseDB
    {
        public ProveedorDB()
        {
        }

        public IDataReader GetProveedorAltaReclamacion(string pais)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@PAIS";
            ParamsValue[0] = pais;
            ParamsType[0] = DbType.String;

            IDataReader dr = db.RunProcDataReader("SP_GET_PROVEEDOR_ALTA_RECLAMACION", ParamsName, ParamsType, ParamsValue);

            return dr;
        }
    }
}