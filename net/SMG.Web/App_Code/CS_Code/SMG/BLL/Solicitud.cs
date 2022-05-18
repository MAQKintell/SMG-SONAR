using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Logging;
using System.Data;
using Iberdrola.Commons.DataAccess;

/// <summary>
/// Descripción breve de Solicitud
/// </summary>
namespace Iberdrola.SMG.BLL
{
    public partial class Solicitud
    {
        public int ActualizarHistoricoSolicitud(string cod_contrato, string idSolicitud, string tipoMovimiento, string usuario, string codEstadoSol, string observaciones, string proveedor, string codSubtipoSolicitud, string codAveria, string codVisita, string tipoOperacion)
        {
            try
            {
                int id_movimiento = 0;
                HistoricoDB objHistoricoDB = new HistoricoDB();
                string sociedad = "";
                id_movimiento = objHistoricoDB.AddHistoricoSolicitud(idSolicitud, tipoMovimiento, usuario, codEstadoSol, observaciones, proveedor);

                // LLAMAMOS AL WEB SERVICE DE ALTA INTERACCION.
                IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoWS(cod_contrato, idSolicitud);
                MantenimientoDTO mantenimiento = new MantenimientoDTO();
                string subTipo = "";
                string urgencia = "";
                string averiasRealizadas = "";
                string codCategoriaVisita = "";
                string categoriaVisita = "";
                if (string.IsNullOrEmpty(codAveria))
                {
                    subTipo = codVisita.Trim();
                }
                else
                {
                    subTipo = codAveria.Trim();
                }
                if (datosMantenimiento != null)
                {
                    while (datosMantenimiento.Read())
                    {
                        mantenimiento.COD_CLIENTE = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_CLIENTE");
                        mantenimiento.CODEFV = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "EFV");
                        mantenimiento.DESEFV = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DES_EFV");
                        //codReceptor = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_RECEPTOR");
                        mantenimiento.COD_RECEPTOR = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_RECEPTOR");
                        mantenimiento.HORARIO_CONTACTO = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "HORARIO_CONTACTO");
                        if (String.IsNullOrEmpty(proveedor))
                        {
                            proveedor = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "PROVEEDOR");
                        }
                        mantenimiento.NOM_TITULAR = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_TITULAR");
                        mantenimiento.APELLIDO1 = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO1");
                        mantenimiento.APELLIDO2 = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO2");
                        mantenimiento.COD_PROVINCIA = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_PROVINCIA");
                        mantenimiento.COD_POBLACION = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_POBLACION");
                        mantenimiento.TIP_VIA_PUBLICA = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_VIA_PUBLICA");
                        mantenimiento.NOM_CALLE = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_CALLE");
                        mantenimiento.COD_PORTAL = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_PORTAL");
                        mantenimiento.COD_FINCA = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_FINCA");
                        mantenimiento.TIP_BIS = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_BIS");
                        mantenimiento.TIP_ESCALERA = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_ESCALERA");
                        mantenimiento.TIP_PISO = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_PISO");
                        mantenimiento.TIP_MANO = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_MANO");
                        mantenimiento.COD_POSTAL = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_POSTAL");
                        mantenimiento.TELEFONO1 = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TELEFONO_CLIENTE1");
                        mantenimiento.TELEFONO2 = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TELEFONO_CLIENTE2");
                        mantenimiento.NUM_TEL_PS = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NUM_TEL_PS");
                        mantenimiento.NUM_MOVIL = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NUM_MOVIL");
                        mantenimiento.DNI = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DNI");
                        mantenimiento.FEC_ALTA_SERVICIO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "FEC_ALTA_SERVICIO");
                        urgencia = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "URGENCIA");
                        Int32 iAveriasRealizadas = (Int32)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "AVERIAS_REALIZADAS");
                        if (iAveriasRealizadas != null)
                        {
                            averiasRealizadas = iAveriasRealizadas.ToString();
                        }
                        mantenimiento.CONTADOR_AVERIAS = (Decimal)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "CONTADOR_AVERIAS");
                        Decimal icodCategoriaVisita = (Decimal)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_CATEGORIA_VISITA");
                        if (icodCategoriaVisita != null)
                        {
                            codCategoriaVisita = icodCategoriaVisita.ToString();
                        }
                        categoriaVisita = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DESC_CATEGORIA_VISITA");
                        // Kintell CUR
                        mantenimiento.SOCIEDAD = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "SOCIEDAD");
                        sociedad = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "SOCIEDAD");
                    }
                }
                UtilidadesWebServices uw = new UtilidadesWebServices();
                String resultadoLlamadaInteraccion = uw.llamadaInteraccionAperturaSolicitud(cod_contrato, mantenimiento.CODEFV, mantenimiento.COD_CLIENTE, observaciones, codSubtipoSolicitud, subTipo, usuario,sociedad);
                if (!String.IsNullOrEmpty(resultadoLlamadaInteraccion))
                {
                    // MANDAR MAIL ERROR
                    //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                    //MandarMailError("Se ha producido un error al llamar al WS InteraccionAperturaSolicitud. CodContrato: " + cod_contrato + " IdSolicitud: " + idSolicitud + ". Message: " + resultadoLlamadaInteraccion, "[SMG] Error llamada WS InteraccionAperturaSolicitud");
                    UtilidadesMail.EnviarMensajeError(" Error llamada WS InteraccionAperturaSolicitud", "Se ha producido un error al llamar al WS InteraccionAperturaSolicitud. CodContrato: " + cod_contrato + " IdSolicitud: " + idSolicitud + ". Message: " + resultadoLlamadaInteraccion);
                    //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
                }
                //Si el usuario que realiza el cambio no es del proveedor, llamar al WS de AperturaSolicitud del proveedor que corresponda.
                bool esProveedor = false;
                UsuarioDTO usuDto = Usuarios.ObtenerUsuario(usuario);
                if (usuDto != null && usuDto.Id_Perfil.HasValue)
                {
                    esProveedor = Usuarios.EsProveedor(int.Parse(usuDto.Id_Perfil.Value.ToString()));
                }
                if (!esProveedor || tipoOperacion.Equals("A"))
                {
                    //Obtenemos datos de la solicitud que necesitamos para realizar la llamada
                    String personaContacto = string.Empty;
                    String telefonoContacto = string.Empty;

                    SolicitudesDB solDB = new SolicitudesDB();
                    DataSet dsSolicitud = solDB.GetSolicitudesPorIDSolicitud(idSolicitud, 1);
                    if (dsSolicitud.Tables[0].Rows.Count > 0)
                    {
                        telefonoContacto = dsSolicitud.Tables[0].Rows[0].ItemArray[11].ToString();
                        personaContacto = dsSolicitud.Tables[0].Rows[0].ItemArray[12].ToString();
                    }
                    if (!string.IsNullOrEmpty(proveedor) && proveedor.Length >= 3)
                    {
                        String resultadoLlamadaWSAperturaSolic = String.Empty;
                        String sProv = proveedor.Substring(0, 3).ToUpper();
                        switch (sProv)
                        {
                            case "ICI":
                            case "TRA":
                            case "ACT":
                                {
                                    resultadoLlamadaWSAperturaSolic = uw.llamadaWSAperturaSolicitudActivais(cod_contrato, idSolicitud, codSubtipoSolicitud, codEstadoSol, personaContacto, telefonoContacto, observaciones, sProv, tipoOperacion, mantenimiento, codCategoriaVisita, sociedad);
                                    break;
                                }
                            case "SIE":
                                {
                                    resultadoLlamadaWSAperturaSolic = uw.llamadaWSAperturaSolicitudSiel(cod_contrato, idSolicitud, codSubtipoSolicitud, codEstadoSol, personaContacto, telefonoContacto, observaciones, tipoOperacion, mantenimiento, categoriaVisita, sociedad);
                                    break;
                                }
                            case "MAP":
                                {
                                    resultadoLlamadaWSAperturaSolic = uw.llamadaWSAperturaSolicitudMapfre(cod_contrato, idSolicitud, codSubtipoSolicitud, codEstadoSol, personaContacto, telefonoContacto, observaciones, tipoOperacion, mantenimiento, urgencia, averiasRealizadas, sociedad);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        if (!String.IsNullOrEmpty(resultadoLlamadaWSAperturaSolic))
                        {
                            // MANDAR MAIL ERROR
                            //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                            //MandarMailError("Se ha producido un error al llamar al WS AperturaSolicitud de " + proveedor + ". CodContrato: " + cod_contrato + " IdSolicitud: " + idSolicitud + ". Message: " + resultadoLlamadaWSAperturaSolic, "[SMG] Error llamada WS AperturaSolicitudProveedor");
                            UtilidadesMail.EnviarMensajeError(" Error llamada WS AperturaSolicitudProveedor", "Se ha producido un error al llamar al WS AperturaSolicitud de " + proveedor + ". CodContrato: " + cod_contrato + " IdSolicitud: " + idSolicitud + ". Message: " + resultadoLlamadaWSAperturaSolic);
                            //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
                        }
                    }
                }
                return id_movimiento;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en ActualizarHistoricoSolicitud", LogHelper.Category.BussinessLogic, ex);
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                //MandarMailError("Error en ActualizarHistoricoSolicitud. CodContrato: " + cod_contrato + " IdSolicitud: " + idSolicitud + " Proveedor: " + proveedor + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace, "[SMG] Error en ActualizarHistoricoSolicitud");
                UtilidadesMail.EnviarMensajeError(" Error en ActualizarHistoricoSolicitud", "Error en ActualizarHistoricoSolicitud. CodContrato: " + cod_contrato + " IdSolicitud: " + idSolicitud + " Proveedor: " + proveedor + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
                throw ex;
            }
        }


        /// <summary>
        /// Modifica el estado de la solicitud, dejando el registro del cambio en el histórico
        /// </summary>
        /// <param name="intIdSolicitud">Id de solicitud.</param>
        /// <param name="strEstado">Estado al que se pasa la solicitud</param>
        /// <param name="strUsuario">Usuario Responsable del cambio</param>
        public void CambiarEstadoSolicitud(Int32 intIdSolicitud, string strEstado, string strUsuario)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] parametros = new string[3];
            DbType[] tipos = new DbType[3];
            object[] valoresParametros = new object[3];

            parametros[0] = "@pID_SOLICITUD";
            tipos[0] = DbType.Int32;
            valoresParametros[0] = intIdSolicitud;

            parametros[1] = "@pID_ESTADO";
            tipos[1] = DbType.String;
            valoresParametros[1] = strEstado;

            parametros[2] = "@pCodUsuario";
            tipos[2] = DbType.String;
            valoresParametros[2] = strUsuario;

            db.RunProcNonQuery("spSMGSolicitudUpdateEstado", parametros, tipos, valoresParametros);
        }

        //20210708 BGN ADD BEG R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos
        public int? CambioEstadoSolicitudeHistorico(string idSolicitud, string estado, string usuario, string observaciones)
        {
            try
            {
                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                objSolicitudesDB.UpdateEstadoSolicitud(idSolicitud, estado, (int)Enumerados.TipoLugarAveria.NA);
                string cod_contrato = string.Empty;
                string sub_tipo = string.Empty;
                string cod_averia = string.Empty;
                string cod_visita = "0";
                string observ_anteriores = string.Empty;
                string observac_final = string.Empty;
                DataSet ds = objSolicitudesDB.GetSolicitudesPorIDSolicitud(idSolicitud, 1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cod_contrato = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    sub_tipo = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                    cod_averia = ds.Tables[0].Rows[0].ItemArray[14].ToString();
                    observ_anteriores = ds.Tables[0].Rows[0].ItemArray[15].ToString();
                }
                DateTime fechaHora = DateTime.Now;
                string Horas = fechaHora.Hour.ToString();
                if (Horas.Length == 1) Horas = "0" + Horas;
                string Minutos = fechaHora.Minute.ToString();
                if (Minutos.Length == 1) Minutos = "0" + Minutos;
                observac_final = "[" + fechaHora.ToString().Substring(0, 10) + "-" + Horas + ":" + Minutos + "] " + usuario + ": " + observaciones;
                objSolicitudesDB.UpdateObservacionesSolicitud(idSolicitud, observac_final + (char)(13) + observ_anteriores);
                return this.ActualizarHistoricoSolicitud(cod_contrato, idSolicitud, "002", usuario, estado, observac_final, "", sub_tipo, cod_averia, cod_visita, "M");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en CambioEstadoSolicitud. IdSolicitud: " + idSolicitud + "; IdEstado; " + estado + "; Usuario; " + usuario + "; Observaciones; " + observaciones, LogHelper.Category.BussinessLogic, ex);
                UtilidadesMail.EnviarMensajeError("Error en CambioEstadoSolicitud", "IdSolicitud: " + idSolicitud + "; IdEstado; " + estado + "; Usuario; " + usuario + "; Observaciones; " + observaciones + ". Message:" + ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");
                return null;
            }
        }
        //20210708 BGN ADD END R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos

        //20210924 BGN ADD BEG R#31319 - No permitir reabrir solicitudes de GC ya enviadas a anular o activar.
        public Boolean SolicitudGCEnviadaDelta(string idSolicitud)
        {
            Boolean enviada = false;
            try
            {
                SolicitudDB objSolicitudDB = new SolicitudDB();
                enviada = objSolicitudDB.SolicitudGCEnviadaDelta(idSolicitud);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en SolicitudGCEnviadaDelta. IdSolicitud: " + idSolicitud, LogHelper.Category.BussinessLogic, ex);
                UtilidadesMail.EnviarMensajeError("Error en SolicitudGCEnviadaDelta", "IdSolicitud: " + idSolicitud + ". Message:" + ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");                
            }
            return enviada;
        }
        //20210924 BGN END BEG R#31319 - No permitir reabrir solicitudes de GC ya enviadas a anular o activar.
    }
}