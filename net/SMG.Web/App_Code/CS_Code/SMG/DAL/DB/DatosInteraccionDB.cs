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
using Iberdrola.SMG.DAL;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.DataAccess;
using Iberdrola.Commons.Logging;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de TipoCalderaDB
    /// </summary>
    public class DatosInteraccionDB
    {
        public DatosInteraccionDB()
        {

        }

        public DataTable ObtenerTiposInteraccion(String tipo, String tipoAveria)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[2];
                DbType[] tiposValoresParametros = new DbType[2];
                Object[] valoresParametros = new Object[2];

                parametros[0] = "@TIPO";
                tiposValoresParametros[0] = DbType.String;
                valoresParametros[0] = tipo;

                parametros[1] = "@TIPO_AVERIA";
                tiposValoresParametros[1] = DbType.String;
                valoresParametros[1] = tipoAveria;

                return db.RunProcDataTable("SP_GET_DATOS_INTERACCION", parametros, tiposValoresParametros, valoresParametros);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al buscar los tipos de interaccion :: " + ex.Message, LogHelper.Category.DataAccess, ex);
                throw;
            }
        }
    }
}
