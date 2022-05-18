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
    /// Descripción breve de VisitasDB
    /// </summary>
    public partial class VisitasDB : BaseDB
    {

        public DataTable DatosVisitaParaCierreGet(string contrato, int codVisita)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] parametros = new string[2];
                DbType[] tiposValoresParametros = new DbType[2];
                Object[] valoresParametros = new Object[2];
                parametros[0] = "@pCOD_CONTRATO";
                tiposValoresParametros[0] = DbType.String;
                valoresParametros[0] = contrato;

                parametros[1] = "@pCOD_VISITA";
                tiposValoresParametros[1] = DbType.Int32;
                valoresParametros[1] = codVisita;

                return db.RunProcDataTable("spSMGDatosVisitaParaCierreGet", parametros, tiposValoresParametros, valoresParametros);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al buscar los datos de la visita para el cierre :: " + ex.Message, LogHelper.Category.DataAccess, ex);
                throw;
            }
        }

        public int ObtenerIdEquipamientoActual(string contrato, string potencia, int TipoDocumento)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] parametros = new string[3];
                DbType[] tiposValoresParametros = new DbType[3];
                Object[] valoresParametros = new Object[3];
                parametros[0] = "@COD_CONTRATO";
                tiposValoresParametros[0] = DbType.String;
                valoresParametros[0] = contrato;
                parametros[1] = "@POTENCIA";
                tiposValoresParametros[1] = DbType.String;
                valoresParametros[1] = potencia;
                parametros[2] = "@idTIPOEQUIPAMIENTO";
                tiposValoresParametros[2] = DbType.Int16;
                valoresParametros[2] = TipoDocumento;


                object result = db.RunProcEscalar("spSMGObtenerIdEquipamientoPorContrato", parametros, tiposValoresParametros, valoresParametros);

                if (!DBNull.Value.Equals(result) && result != null)
                {
                    return int.Parse(result.ToString());
                }
                else
                {
                    // Sin no se ha encontrado nada el contador es 0.
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al buscar el equipamiento: " + ex.Message, LogHelper.Category.DataAccess, ex);
                throw;
            }
        }

        public void ActualizarEquipamientos(Decimal IdEquipamientoActual, String Contrato, Decimal IdTipoEquipamiento, Double Potencia)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                if (IdEquipamientoActual != 0)
                {
                    String[] aNombres = new String[4] { 
                                    "@ID_EQUIPAMIENTO",
                                    "@COD_CONTRATO",
                                    "@ID_TIPO_EQUIPAMIENTO",
                                    "@POTENCIA"};
                    DbType[] aTipos = new DbType[4] {
                                    DbType.Decimal,
                                    DbType.String,
                                    DbType.Decimal,
                                    DbType.Double};

                    Object[] aValores = new Object[4] { 
                                    IdEquipamientoActual,
                                    Contrato,
                                    IdTipoEquipamiento, 
                                    Potencia};
                    // actualizar equipamiento
                    db.RunProcEscalar("SP_UPDATE_EQUIPAMIENTO", aNombres, aTipos, aValores);
                }
                else
                {
                    String[] aNombres = new String[3] { 
                                    "@COD_CONTRATO",
                                    "@ID_TIPO_EQUIPAMIENTO",
                                    "@POTENCIA"};

                    DbType[] aTipos = new DbType[3] {
                                    DbType.String,
                                    DbType.Decimal,
                                    DbType.Double};

                    Object[] aValores = new Object[3] { 
                                    Contrato,
                                    IdTipoEquipamiento, 
                                    Potencia};
                    // insertar equipamiento
                    db.RunProcEscalar("SP_INSERT_EQUIPAMIENTO", aNombres, aTipos, aValores);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al actualizar el equipamiento: " + ex.Message, LogHelper.Category.DataAccess, ex);
                throw;
            }
        }

        public void EliminarEquipamiento(Decimal IdEquipamientoActual, String Contrato)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                String[] aNombres = new String[2] { "@ID_EQUIPAMIENTO", "@COD_CONTRATO" };
                DbType[] aTipos = new DbType[2] { DbType.Decimal, DbType.String };
                Object[] aValores = new Object[2] { IdEquipamientoActual, Contrato };

                if (IdEquipamientoActual != 0)
                {
                    // actualizar equipamiento
                    db.RunProcEscalar("SP_DELETE_EQUIPAMIENTO", aNombres, aTipos, aValores);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al eliminar el equipamiento: " + ex.Message, LogHelper.Category.DataAccess, ex);
                throw;
            }
        }
        
        public bool EstadoVisitaExiste(int estadoVisita)
        {
            try
            {
                // TODO
                return true;                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int ObtenerNumeroVisitasTecnicoDia(int tecnico, DateTime fechaVisita)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] parametros = new string[2];
                DbType[] tiposValoresParametros = new DbType[2];

                Object[] valoresParametros = new Object[2];
                parametros[0] = "@ID_TECNICO";
                tiposValoresParametros[0] = DbType.Int32;
                valoresParametros[0] = tecnico;

                parametros[1] = "@FECVISITA";
                tiposValoresParametros[1] = DbType.DateTime;
                valoresParametros[1] = fechaVisita;

                object result = db.RunProcEscalar("MTOGASBD_GET_CONTADOR_VISITAS_DIAS_POR_TECNICO", parametros, tiposValoresParametros, valoresParametros);

                if (!DBNull.Value.Equals(result) && result!=null)
                {
                    return int.Parse(result.ToString());
                }
                else
                {
                    // Sin no se ha encontrado nada el contador es 0.
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al buscar el contador de visitas para el técnico :: " + ex.Message, LogHelper.Category.DataAccess, ex);
                throw;
            }
        }


        public DateTime? ObtenerUltimaFechaMovimientoVisitaHistorico(string codContrato, int codVisita)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] parametros = new string[2];
                DbType[] tiposValoresParametros = new DbType[2];

                Object[] valoresParametros = new Object[2];
                parametros[0] = "@COD_CONTRATO";
                tiposValoresParametros[0] = DbType.String;
                valoresParametros[0] = codContrato;

                parametros[1] = "@COD_VISITA";
                tiposValoresParametros[1] = DbType.Int16;
                valoresParametros[1] = codVisita;

                object result = db.RunProcEscalar("SP_GET_FECHA_ULTIMO_MOVIMIENTO_VISITA_HISTORICO", parametros, tiposValoresParametros, valoresParametros);

                if (!DBNull.Value.Equals(result))
                {
                    return (DateTime)result;
                }
                else
                {
                    // Sin no se ha encontrado nada devolvemos nulo.
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al buscar el contador de visitas para el técnico :: " + ex.Message, LogHelper.Category.DataAccess, ex);
                throw;
            }
        }
   }
}