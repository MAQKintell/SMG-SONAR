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
using System.Data.SqlClient;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de VisitasDB
    /// </summary>
    public partial class AperturaSolicitudDB : BaseDB
    {

        //public DataTable DatosVisitaParaCierreGet(string contrato, int codVisita)
        //{
        //    try
        //    {
        //        DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
        //        string[] parametros = new string[2];
        //        DbType[] tiposValoresParametros = new DbType[2];
        //        Object[] valoresParametros = new Object[2];
        //        parametros[0] = "@pCOD_CONTRATO";
        //        tiposValoresParametros[0] = DbType.String;
        //        valoresParametros[0] = contrato;

        //        parametros[1] = "@pCOD_VISITA";
        //        tiposValoresParametros[1] = DbType.Int32;
        //        valoresParametros[1] = codVisita;

        //        return db.RunProcDataTable("spSMGDatosVisitaParaCierreGet", parametros, tiposValoresParametros, valoresParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("Error al buscar los datos de la visita para el cierre :: " + ex.Message, LogHelper.Category.DataAccess, ex);
        //        throw;
        //    }
        //}

        public string ObtenerProveedorAveriaPorContrato(string contrato)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] parametros = new string[1];
                DbType[] tiposValoresParametros = new DbType[1];
                Object[] valoresParametros = new Object[1];
                parametros[0] = "@COD_CONTRATO";
                tiposValoresParametros[0] = DbType.String;
                valoresParametros[0] = contrato;

                object result = db.RunProcEscalar("spSMGObtenerProveedorAveriaPorContrato", parametros, tiposValoresParametros, valoresParametros);

                if (!DBNull.Value.Equals(result) && result != null)
                {
                    return result.ToString();
                }
                else
                {
                    // Sin no se ha encontrado nada el contador es 0.
                    return "";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al buscar el proveedor de averia por contrato: " + ex.Message, LogHelper.Category.DataAccess, ex);
                throw;
            }
        }

        public int AddSolicitudWS(string cod_contrato, string cod_tipo_solicitud, string cod_subtipo_solicitud, string cod_estado, string telef_contacto, string pers_contacto, string cod_averia, string observaciones, string proveedor, bool Urgente,bool Retencion)
        {
            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[12];
                DbType[] tiposValoresParametros = new DbType[12];
                Object[] valoresParametros = new Object[12];
                parametros[0] = "@COD_CONTRATO";
                tiposValoresParametros[0] = DbType.String;
                valoresParametros[0] = cod_contrato;

                parametros[1] = "@Tipo_solicitud";
                valoresParametros[1] = cod_tipo_solicitud;
                tiposValoresParametros[1] = DbType.String;

	            parametros[2] = "@subtipo_solicitud";
                valoresParametros[2] = cod_subtipo_solicitud;
                tiposValoresParametros[2] = DbType.String;

	            parametros[3] = "@Fecha_creacion";
                valoresParametros[3] = DateTime.Now;
                tiposValoresParametros[3] = DbType.DateTime;

	            parametros[4] = "@Estado_solicitud";
                valoresParametros[4] = cod_estado;
                tiposValoresParametros[4] = DbType.String;

	            parametros[5] = "@telefono_contacto";
                valoresParametros[5] = telef_contacto;
                tiposValoresParametros[5] = DbType.String;

	            parametros[6] = "@Persona_contacto";
                valoresParametros[6] = pers_contacto;
                tiposValoresParametros[6] = DbType.String;

	            parametros[7] = "@Cod_averia";
                valoresParametros[7] = cod_averia;
                tiposValoresParametros[7] = DbType.String;

	            parametros[8] = "@Observaciones";
                valoresParametros[8] = observaciones;
                tiposValoresParametros[8] = DbType.String;

	            parametros[9] = "@Proveedor";
                valoresParametros[9] = proveedor;
                tiposValoresParametros[9] = DbType.String;

	            parametros[10] = "@Urgente";
                valoresParametros[10] = Urgente;
                tiposValoresParametros[10] = DbType.Boolean;


	            parametros[11] = "@Retencion";
                valoresParametros[11] = Retencion;
                tiposValoresParametros[11] = DbType.Boolean;

                return int.Parse(db.RunProcEscalar("MTOGASBD_AddSolicitudesWS", parametros, tiposValoresParametros, valoresParametros).ToString());

            }
            catch (Exception ex)
            {
                return -1;
            }

        }
        

     }
}