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
using Iberdrola.SMG.WS;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de VisitasDB
    /// </summary>
    public partial class SolicitudDB : BaseDB
    {
        public SolicitudDB()
        {

        }

        public Int64 AltaSolicitudGCContratoFicticio(string contratoActual,string valoracion,string DNI)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@CONTRATO";
                Parametros[1] = "@VALORACION";
                Parametros[2] = "@DNI";

                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;

                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = contratoActual;
                ValoresParametros[1] = valoracion;
                ValoresParametros[2] = DNI;

                return int.Parse(db.RunProcEscalar("spInsertarSolicitudCalderasContratoFicticio", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                // Error.
                throw;
            }
        }

        public Int64 AltaSolicitudGC(string contratoActual)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[1];
                Parametros[0] = "@CONTRATO";

                DbType[] TiposValoresParametros = new DbType[1];
                TiposValoresParametros[0] = DbType.String;

                Object[] ValoresParametros = new Object[1];
                ValoresParametros[0] = contratoActual;

                return int.Parse(db.RunProcEscalar("spInsertarSolicitudGC", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                // Error.
                throw;
            }
        }
        

        public void ActualizarMotivoCancelacionSolicitud(int idSolicitud, string motivoCancelacion)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@ID_SOLICITUD";
                Parametros[1] = "@MOTIVO";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.Int32;
                TiposValoresParametros[1] = DbType.String;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = idSolicitud;
                ValoresParametros[1] = motivoCancelacion;

                db.RunProcEscalar("spActualizarMotivoCancelacionSolicitud", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public DataTable ObtenerSolicitudes(string proveedor, 
             string cod_contrato , 
                 string consultaPendietes,
                     string cod_tipo , 
                         string cod_subtipo , 
                             string cod_estado , 
                                 string fechaDesde , 
                                     string fechaHasta , 
                                         string Provincia , 
                                             string Urgente , 
                                                 string Desde , 
                                                     string Hasta , 
                                                         string Perfil , 
                                                             string Id_Solicitud , 
                                                                  string Nombre , 
                                                                      string Apellido1, 
                                                                          string Apellido2, 
                                                                              string Colores, 
                                                                                  string idIdioma , 
                                                                                    string PAIS,
                                                                                        string DNI,
                                                                                            string CUI) 
        {
            try
            {
                if (Colores=="") { Colores="0"; }
                if (idIdioma=="") { idIdioma="0"; }
                if (PAIS == "") { PAIS = "0"; }

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[22];
                DbType[] aTipo = new DbType[22];
                string[] aValores = new string[22];

                aNombre[0] = "@proveedor";
                aTipo[0] = DbType.String;
                aValores[0] = proveedor.ToString();

                aNombre[1] = "@cod_contrato";
                aTipo[1] = DbType.String;
                aValores[1] = cod_contrato.ToString();

                aNombre[2] = "@consultaPendietes";
                aTipo[2] = DbType.String;
                aValores[2] = consultaPendietes.ToString();

                aNombre[3] = "@cod_tipo";
                aTipo[3] = DbType.String;
                aValores[3] = cod_tipo.ToString();

                aNombre[4] = "@cod_subtipo";
                aTipo[4] = DbType.String;
                aValores[4] = cod_subtipo.ToString();

                aNombre[5] = "@cod_estado";
                aTipo[5] = DbType.String;
                aValores[5] = cod_estado.ToString();

                aNombre[6] = "@fechaDesde";
                aTipo[6] = DbType.String;
                aValores[6] = fechaDesde.ToString();

                aNombre[7] = "@fechaHasta";
                aTipo[7] = DbType.String;
                aValores[7] = fechaHasta.ToString();

                aNombre[8] = "@Provincia";
                aTipo[8] = DbType.String;
                aValores[8] = Provincia.ToString();

                aNombre[9] = "@Urgente";
                aTipo[9] = DbType.String;
                aValores[9] = Urgente.ToString();

                aNombre[10] = "@Desde";
                aTipo[10] = DbType.String;
                aValores[10] = Desde.ToString();

                aNombre[11] = "@Hasta";
                aTipo[11] = DbType.String;
                aValores[11] = Hasta.ToString();

                aNombre[12] = "@Perfil";
                aTipo[12] = DbType.String;
                aValores[12] = Perfil;

                aNombre[13] = "@Id_Solicitud";
                aTipo[13] = DbType.String;
                aValores[13] = Id_Solicitud.ToString();

                aNombre[14] = "@Nombre";
                aTipo[14] = DbType.String;
                aValores[14] = Nombre.ToString();

                aNombre[15] = "@Apellido1";
                aTipo[15] = DbType.String;
                aValores[15] = Apellido1.ToString();

                aNombre[16] = "@Apellido2";
                aTipo[16] = DbType.String;
                aValores[16] = Apellido2.ToString();

                aNombre[17] = "@Colores";
                aTipo[17] = DbType.String;
                aValores[17] = Colores.ToString();

                aNombre[18] = "@idIdioma";
                aTipo[18] = DbType.String;
                aValores[18] = idIdioma.ToString();

                aNombre[19] = "@PAIS";
                aTipo[19] = DbType.String;
                aValores[19] = PAIS.ToString();

                aNombre[20] = "@DNI";
                aTipo[20] = DbType.String;
                aValores[20] = DNI.ToString();

                aNombre[21] = "@CUI";
                aTipo[21] = DbType.String;
                aValores[21] = CUI.ToString();

                return db.RunProcDataTable("spGetSolicitudesParaLaWeb", aNombre, aTipo, aValores);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable ObtenerSolicitudesExcel(string proveedor, 
             string cod_contrato , 
                 string consultaPendietes,
                     string cod_tipo , 
                         string cod_subtipo , 
                             string cod_estado , 
                                 string fechaDesde , 
                                     string fechaHasta , 
                                         string Provincia , 
                                             string Urgente , 
                                                 string Desde , 
                                                     string Hasta , 
                                                         string Perfil , 
                                                             string Id_Solicitud , 
                                                                  string Nombre , 
                                                                      string Apellido1, 
                                                                          string Apellido2, 
                                                                              string Colores, 
                                                                                  string idIdioma , 
                                                                                    string PAIS,
                                                                                        string DNI,
                                                                                            string CUI) 
        {
            try
            {
                if (Colores=="") { Colores="0"; }
                if (idIdioma=="") { idIdioma="0"; }
                if (PAIS == "") { PAIS = "0"; }

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[22];
                DbType[] aTipo = new DbType[22];
                string[] aValores = new string[22];

                aNombre[0] = "@proveedor";
                aTipo[0] = DbType.String;
                aValores[0] = proveedor.ToString();

                aNombre[1] = "@cod_contrato";
                aTipo[1] = DbType.String;
                aValores[1] = cod_contrato.ToString();

                aNombre[2] = "@consultaPendietes";
                aTipo[2] = DbType.String;
                aValores[2] = consultaPendietes.ToString();

                aNombre[3] = "@cod_tipo";
                aTipo[3] = DbType.String;
                aValores[3] = cod_tipo.ToString();

                aNombre[4] = "@cod_subtipo";
                aTipo[4] = DbType.String;
                aValores[4] = cod_subtipo.ToString();

                aNombre[5] = "@cod_estado";
                aTipo[5] = DbType.String;
                aValores[5] = cod_estado.ToString();

                aNombre[6] = "@fechaDesde";
                aTipo[6] = DbType.String;
                aValores[6] = fechaDesde.ToString();

                aNombre[7] = "@fechaHasta";
                aTipo[7] = DbType.String;
                aValores[7] = fechaHasta.ToString();

                aNombre[8] = "@Provincia";
                aTipo[8] = DbType.String;
                aValores[8] = Provincia.ToString();

                aNombre[9] = "@Urgente";
                aTipo[9] = DbType.String;
                aValores[9] = Urgente.ToString();

                aNombre[10] = "@Desde";
                aTipo[10] = DbType.String;
                aValores[10] = Desde.ToString();

                aNombre[11] = "@Hasta";
                aTipo[11] = DbType.String;
                aValores[11] = Hasta.ToString();

                aNombre[12] = "@Perfil";
                aTipo[12] = DbType.String;
                aValores[12] = Perfil;

                aNombre[13] = "@Id_Solicitud";
                aTipo[13] = DbType.String;
                aValores[13] = Id_Solicitud.ToString();

                aNombre[14] = "@Nombre";
                aTipo[14] = DbType.String;
                aValores[14] = Nombre.ToString();

                aNombre[15] = "@Apellido1";
                aTipo[15] = DbType.String;
                aValores[15] = Apellido1.ToString();

                aNombre[16] = "@Apellido2";
                aTipo[16] = DbType.String;
                aValores[16] = Apellido2.ToString();

                aNombre[17] = "@Colores";
                aTipo[17] = DbType.String;
                aValores[17] = Colores.ToString();

                aNombre[18] = "@idIdioma";
                aTipo[18] = DbType.String;
                aValores[18] = idIdioma.ToString();

                aNombre[19] = "@PAIS";
                aTipo[19] = DbType.String;
                aValores[19] = PAIS.ToString();

                aNombre[20] = "@DNI";
                aTipo[20] = DbType.String;
                aValores[20] = DNI.ToString();

                aNombre[21] = "@CUI";
                aTipo[21] = DbType.String;
                aValores[21] = CUI.ToString();

                return db.RunProcDataTable("spGetSolicitudesExcelParaLaWeb", aNombre, aTipo, aValores);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //20200226 BGN BEG R#22129 Nueva pantalla con solicitudes pendientes de tramitar para el proveedor
        public DataTable ObtenerSolicitudesAbiertasPorProveedor(string proveedor, int idIdioma)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[2];
                DbType[] aTipo = new DbType[2];
                string[] aValores = new string[2];

                aNombre[0] = "@PROVEEDOR";
                aTipo[0] = DbType.String;
                aValores[0] = proveedor.ToString();

                aNombre[1] = "@ID_IDIOMA";
                aTipo[1] = DbType.Int32;
                aValores[1] = idIdioma.ToString();

                return db.RunProcDataTable("SP_GET_SOLICITUDES_ABIERTAS_POR_PROVEEDOR", aNombre, aTipo, aValores);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20200226 BGN END R#22129 Nueva pantalla con solicitudes pendientes de tramitar para el proveedor

        //20200629 BGN BEG R#25049 Modificar callback Inspeccions de Portugal - Carga Valor en caracteristicas
        public DataTable GetCaracteristicasSolicitudPorTipo(string cod_tipo, String cod_subtipo, String cod_estado, int idIdioma, string idSolicitud)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[5];
                DbType[] aTipo = new DbType[5];
                string[] aValores = new string[5];

                aNombre[0] = "@cod_tipo";
                aTipo[0] = DbType.String;
                aValores[0] = cod_tipo.ToString();

                aNombre[1] = "@cod_subtipo";
                aTipo[1] = DbType.String;
                aValores[1] = cod_subtipo.ToString();

                aNombre[2] = "@cod_estado";
                aTipo[2] = DbType.String;
                aValores[2] = cod_estado.ToString();

                aNombre[3] = "@idIDIOMA";
                aTipo[3] = DbType.Int32;
                aValores[3] = idIdioma.ToString();

                aNombre[4] = "@id_solicitud";
                aTipo[4] = DbType.Int32;
                aValores[4] = idSolicitud.ToString();

                return db.RunProcDataTable("MTOGASBD_GetTipoCarasteristicasPorTipoSol_Valor", aNombre, aTipo, aValores);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20200629 BGN END R#25049 Modificar callback Inspeccions de Portugal - Carga Valor en caracteristicas

        public DataTable ObtenerHorarioInspeccionPorContrato(string codContrato)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[1];
                DbType[] aTipo = new DbType[1];
                string[] aValores = new string[1];

                aNombre[0] = "@cod_contrato";
                aTipo[0] = DbType.String;
                aValores[0] = codContrato.ToString();


                return db.RunProcDataTable("GetHorarioInspeccionPorContrato", aNombre, aTipo, aValores);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //20210624 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
        public DataTable ObtenerSolicitudesPorContratoYSubTipoSolicitud(string cod_contrato
                                                                        , string cod_subTipoSol
                                                                        , int? esEstadoFinal
                                                                        , int id_Idioma)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[4];
                DbType[] aTipo = new DbType[4];
                string[] aValores = new string[4];

                aNombre[0] = "@pCOD_CONTRATO";
                aTipo[0] = DbType.String;
                aValores[0] = cod_contrato.ToString();

                aNombre[1] = "@pSUBTIPO_SOLICITUD";
                aTipo[1] = DbType.String;
                aValores[1] = (string.IsNullOrEmpty(cod_subTipoSol) ? null : cod_subTipoSol.ToString());
                
                aNombre[2] = "@pESTADO_FINAL";
                aTipo[2] = DbType.Int16;
                aValores[2] = (esEstadoFinal == null ? null : esEstadoFinal.ToString());

                aNombre[3] = "@pID_IDIOMA";
                aTipo[3] = DbType.Int16;
                aValores[3] = id_Idioma.ToString();

                return db.RunProcDataTable("SP_GET_SOLICITUDES_POR_CONTRATO_Y_SUBTIPO", aNombre, aTipo, aValores);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20210624 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion

        //20210924 BGN ADD BEG R#31319 - No permitir reabrir solicitudes de GC ya enviadas a anular o activar.
        public Boolean SolicitudGCEnviadaDelta(string idSolicitud)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[1];
                DbType[] aTipo = new DbType[1];
                string[] aValores = new string[1];

                aNombre[0] = "@idSolicitud";
                aTipo[0] = DbType.String;
                aValores[0] = idSolicitud;

                return Boolean.Parse(db.RunProcEscalar("spComprobarSiSolicitudEnviadaDelta", aNombre, aTipo, aValores).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20210924 BGN ADD END R#31319 - No permitir reabrir solicitudes de GC ya enviadas a anular o activar.
    }
}