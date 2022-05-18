using Iberdrola.Commons.Logging;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Iberdrola.Commons.DataAccess;
using System.Data;

namespace Iberdrola.SMG.WS
{
    /// <summary>
    /// Descripción breve de EscuchaFirmaRemotaWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class EscuchaFirmaRemotaWS : WSBase
    {
        public Authentication ServiceCredentials;

        public EscuchaFirmaRemotaWS()
        {

            //Elimine la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
            //InitializeComponent(); 
        }

        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public WSResponse UpdateDocumentSet(string description, string iddossier, string status, string idstatus, string dateDossier, string refExterna, string reason)
        {
            WSResponse response = new WSResponse();

            //Boolean habilitarWS = false;
            //ConfiguracionDTO confHabilitarWS = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_WS);
            //if (confHabilitarWS != null && !string.IsNullOrEmpty(confHabilitarWS.Valor) && Boolean.Parse(confHabilitarWS.Valor))
            //{
            //    habilitarWS = Boolean.Parse(confHabilitarWS.Valor);
            //}

            Boolean habilitarWS = ValidarwebServiceActivo(Enumerados.Configuracion.HABILITAR_ESCUCHAFIRMAREMOTA_WS.ToString());

            if (!habilitarWS)
            {
                response.AddError(CommonWSError.ErrorWSInactivo);
            }
            else
            {
                try
                {
                    // Primero guardamos la traza de la llamada.
                    GuardarTrazaLlamada(description, iddossier, status, idstatus, dateDossier, refExterna, "WS_EDATA", reason);

                    if (this.Context.Request.Headers["Authorization"] != null)
                    {
                        string[] usuario = GetHeaderAuthorization(this.Context.Request.Headers["Authorization"].ToString());
                        ServiceCredentials = new Authentication();
                        ServiceCredentials.UserName = usuario[0].ToString();
                        ServiceCredentials.Password = usuario[1].ToString();
                        if (this.ValidarUsuario(usuario[0].ToString(), usuario[1].ToString()))
                        {
                            try
                            {
                                if (this._idTraza.HasValue && !ServiceCredentials.UserName.Equals("WS_EDATA"))
                                {
                                    TrazaDB trazaDB = new TrazaDB();
                                    trazaDB.InsertarUsuarioTraza(this._idTraza.ToString(), ServiceCredentials.UserName);
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }

                            DocumentoDTO docfiltro = new DocumentoDTO();
                            docfiltro.IdEnvioEdatalia = refExterna;
                            Pagination pag = new Pagination();
                            pag.CampoOrden = "FECHA_ENVIO_EDATALIA";
                            pag.DireccionOrden = "ASC";
                            pag.PaginaActual = 0;
                            List<DocumentoDTO> lDoc = Documento.Buscar(docfiltro, pag);
                            if (lDoc.Count == 0)
                            {
                                //Si no encontramos en la tabla documentos el documento de Edatalia devolveremos error
                                response.AddError(UpdateDocumentSetWSError.ErrorDocumentoNoEncontrado);
                            }
                            else
                            {
                                try
                                {
                                    DocumentoDTO doc = lDoc[0];
                                    int idSolicitud = doc.IdSolicitud.Value;
                                    //Antes de hacer el cambio de estado comprobamos en que estado esta la solicitud
                                    SolicitudesDB solDB = new SolicitudesDB();
                                    DataSet ds = solDB.GetSolicitudesPorIDSolicitud(idSolicitud.ToString(), 1);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string cod_estado_sol = ds.Tables[0].Rows[0].ItemArray[8].ToString();
                                        //Si el estado de la solicitud sigue en “Oferta Presentada Pdte. Firma” haremos los cambios
                                        if (cod_estado_sol.Equals("080"))
                                        {
                                            if (String.IsNullOrEmpty(doc.IdDocEdatalia))
                                            {
                                                doc.IdDocEdatalia = iddossier;
                                            }
                                            /*
                                            //20210209 Acordamos no hacer nada, mantenemos el original ya que no deberia darse este caso.
                                            else if (!doc.IdDocEdatalia.Equals(iddossier))
                                            {
                                                //Si el idDocumento de Edatalia no coincide con el que tenemos en nuestro sistema devolvemos error
                                                response.AddError(UpdateDocumentSetWSError.ErrorIdDocumentoNoCoincide);
                                            }
                                            */
                                            doc.IdEstadoEdatalia = int.Parse(idstatus);
                                            Documento.Actualizar(doc, ServiceCredentials.UserName);

                                            //20210708 BGN MOD BEG R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos
                                            /* Dependiendo del idstatus cambiaremos el estado de la solicitud al estado que corresponda
                                            Devuelto 4 -> "Pdte. corregir e-mail"
                                            Caducado 2 -> “Oferta Rechazada Pdte. Confirmación” 
                                            Rechazado 1 -> “Oferta Rechazada Pdte. Confirmación” 
                                            Firmado 0 -> “Oferta Aceptada”
                                            */
                                            Solicitud soli = new Solicitud();
                                            switch (idstatus)
                                            {
                                                case "4":
                                                    {
                                                        soli.CambioEstadoSolicitudeHistorico(idSolicitud.ToString(), "083", ServiceCredentials.UserName, "Cambio estado Edatalia.");
                                                        break;
                                                    }
                                                case "2":
                                                case "1":
                                                    {
                                                        int? idMovimiento = soli.CambioEstadoSolicitudeHistorico(idSolicitud.ToString(), "081", ServiceCredentials.UserName, "Cambio estado Edatalia.");
                                                        if (idMovimiento != null && !string.IsNullOrEmpty(reason))
                                                        {
                                                            //190 - Motivo rechazo contrato
                                                            AddCaracteristica(idSolicitud.ToString(), idMovimiento.ToString(), "190", reason);
                                                        }
                                                        break;
                                                    }
                                                case "0":
                                                    {
                                                        soli.CambioEstadoSolicitudeHistorico(idSolicitud.ToString(), "060", ServiceCredentials.UserName, "Cambio estado Edatalia.");
                                                        break;
                                                    }
                                            }
                                            //20210708 BGN MOD END R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos
                                        }
                                        else
                                        {
                                            //Si la solicitud ya se encuentra en otro estado no hacemos nada
                                            response.AddError(UpdateDocumentSetWSError.ErrorEstadoSolicitudIncorrecto);
                                        }
                                    }
                                    else
                                    {
                                        //No se ha encontrado la solicitud del documento
                                        response.AddError(UpdateDocumentSetWSError.ErrorSolicitudDocumentoIncorrecta);
                                    }                                    
                                }
                                catch (Exception ex2)
                                {
                                    response.AddError(UpdateDocumentSetWSError.ErrorActualizarEstadoDocumento);
                                    UtilidadesMail.EnviarMensajeError("Error en la llamada al UpdateDocumentSet. Actualizar Documento", "IdEnvioEdatalia: " + refExterna + ". Message:" + ex2.Message + " _Trace:" + ex2.StackTrace + "_" + ex2.Source + "_" + ex2.Data + "_");
                                }
                            }
                        }
                        else
                        {
                            response.AddError(CommonWSError.ErrorCredencialesUsuario);
                        }
                    }
                    else
                    {
                        response.AddError(CommonWSError.ErrorCredencialesUsuario);
                    }

                    // Guardamos el resultado en la traza de la llamada.
                    this.GuardarTrazaResultadoLlamada(response);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("Error en la llamada al UpdateDocumentSet", LogHelper.Category.BussinessLogic, ex);
                    UtilidadesMail.EnviarMensajeError("Error en la llamada al UpdateDocumentSet", "Message:" + ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");
                    response.AddError(CommonWSError.ErrorNoDefinido);
                }
            }
            return response;
        }

        //20210708 BGN ADD BEG R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos
        protected void AddCaracteristica(string idSolicitud, string id_movimiento, string tipCar, string valor)
        {
            try
            {
                CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
                HistoricoDB objHistoricoDB = new HistoricoDB();
                DateTime fechaHora = DateTime.Now;

                int idCaracteristica = objCaracteristicasDB.AddCaracteristica(idSolicitud, tipCar, valor, fechaHora.ToShortDateString());
                objHistoricoDB.AddHistoricoCaracteristicas(idCaracteristica, id_movimiento, tipCar, valor);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en la llamada al UpdateDocumentSet - AddCaracteristica. IdSolicitud: " + idSolicitud + "; IdMovimiento; " + id_movimiento + "; TipCar; " + tipCar + "; Valor; " + valor, LogHelper.Category.BussinessLogic, ex);
                UtilidadesMail.EnviarMensajeError("Error en la llamada al UpdateDocumentSet - AddCaracteristica", "IdSolicitud: " + idSolicitud + "; IdMovimiento; " + id_movimiento + "; TipCar; " + tipCar + "; Valor; " + valor + ". Message:" + ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");
            }
        }
        //20210708 BGN ADD END R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos

        protected void GuardarTrazaLlamada(string description, string iddossier, string status, string idstatus, string dateDossier, string refExterna, string usuario, string reason)
        {
            try
            {
                string request = "description: "+ description + ";iddossier: " + iddossier + ";status: " + status + ";idstatus: " + idstatus + ";dateDossier: " + dateDossier + ";refExterna: " + refExterna + ";reason: " + reason + ";usuario: " + usuario;
                TrazaDB trazaDB = new TrazaDB();
                this._idTraza = trazaDB.InsertarTrazaWSCierreVisita("DATOS RECIBIDOS", request, "UpdateDocumentSet", usuario);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error GuardarTrazaLlamada UpdateDocumentSet", LogHelper.Category.BussinessLogic, ex);
                UtilidadesMail.EnviarMensajeError("Error GuardarTrazaLlamada UpdateDocumentSet", "Message:" + ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");
            }
        }
        protected override void GuardarTrazaLlamada(WSRequest request)
        {
            throw new NotImplementedException();
        }

        protected override void GuardarTrazaResultadoLlamada(WSResponse response)
        {
            try
            {
                if (this._idTraza.HasValue)
                {
                    TrazaDB trazaDB = new TrazaDB();
                    if (response.TieneError)
                    {
                        // Si tiene errores.
                        trazaDB.ActualizarTraza(this._idTraza.ToString(), response.ListaErrores[0].Descripcion.ToString());
                    }
                    else
                    {
                        // Si todo ha ido bien.
                        trazaDB.ActualizarTraza(this._idTraza.ToString(), "TODO CORRECTO");
                    }
                }
                else
                {
                    throw new Exception("Error al guardar el resultado de la traza. No se ha encontrato el identificativo de la traza de la llamada al WS.");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error GuardarTrazaLlamada UpdateDocumentSet", LogHelper.Category.BussinessLogic, ex);
                UtilidadesMail.EnviarMensajeError("Error GuardarTrazaLlamada UpdateDocumentSet", "Message:" + ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");
            }
        }

        protected override bool ValidarDatosEntrada(WSRequest request, WSResponse response)
        {
            throw new NotImplementedException();
        }
    }
}