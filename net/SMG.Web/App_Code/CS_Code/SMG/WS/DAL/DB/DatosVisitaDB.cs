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
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.DataAccess;
using Iberdrola.Commons.Utils;
using System.Collections.Generic;
using Iberdrola.Commons.Logging;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de DatosVisita
    /// </summary>
    public partial class DatosVisitaDB : BaseDB
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contrato"></param>
        /// <returns></returns>
        public DataTable GetDatosVisita(string contrato)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] parametros = new string[1];
                DbType[] tiposValoresParametros = new DbType[1];
                Object[] valoresParametros = new Object[1];
                parametros[0] = "@pCOD_CONTRATO";
                tiposValoresParametros[0] = DbType.String;
                valoresParametros[0] = contrato;

                return db.RunProcDataTable("spSMGGetDatosVisitaWS", parametros, tiposValoresParametros, valoresParametros);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al buscar los datos las ultimas 4 visitas :: " + ex.Message, LogHelper.Category.DataAccess, ex);
                throw;
            }
        }


       

     }
}