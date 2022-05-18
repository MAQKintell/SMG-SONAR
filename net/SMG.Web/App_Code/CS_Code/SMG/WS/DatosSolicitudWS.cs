using System.Web.Services;
using System.Web.Services.Protocols;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.WS;
using System;
using Iberdrola.Commons.Logging;
using System.Data;
using Iberdrola.Commons.Utils;
using Iberdrola.SMG.DAL.DB;
using System.Configuration;
using System.IO;
using Iberdrola.Commons.Web;
using Iberdrola.Commons.Security;
using System.Xml;
using System.Xml.Serialization;
using System.Data.Common;
using System.Collections.Generic;
using System.Net;


namespace Iberdrola.SMG.WS
{
    public class DatosSolicitudWS : WSBase
    {
        /// <summary>
        /// Obtenemos los datos de la visita en base a un contrato
        /// </summary>
        /// <param name="cierreVisita">Objeto de tipo DatosVisitaRequest con los datos 
        /// necesarios para realizar el cierre de la vista</param>
        /// <returns>Objeto de tipo DatosVisitaResponse</returns>

        public Authentication ServiceCredentials;


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public DatosSolicitudResponse getDatosSolicitud(DatosSolicitudRequest contratoVigente)
        {
            DatosSolicitudResponse response = new DatosSolicitudResponse();

            // Boolean habilitarWS = false;
            //ConfiguracionDTO confHabilitarWS = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_WS);
            //if (confHabilitarWS != null && !string.IsNullOrEmpty(confHabilitarWS.Valor) && Boolean.Parse(confHabilitarWS.Valor))
            //{
            //    habilitarWS = Boolean.Parse(confHabilitarWS.Valor);
            //}

            Boolean habilitarWS = ValidarwebServiceActivo(Enumerados.Configuracion.HABILITAR_DATOSSOLICITUD_WS.ToString());

            if (!habilitarWS)
            {
                response.AddError(CommonWSError.ErrorWSInactivo);
            }
            else
            {
                //  response.CONTRATO_VIGENTE = "5"; 
                try
                {


                    // Primero guardamos la traza de la llamada.
                    this.GuardarTrazaLlamada(contratoVigente);
                    // Realizamos la validación del usuario.
                    //MandarMail("ACCESO", "WS PROO_11", "maquintela@gfi.es", true);
                    if (this.Context.Request.Headers["Authorization"] != null)
                    {
                        string[] usuario = GetHeaderAuthorization(this.Context.Request.Headers["Authorization"].ToString());
                        //MandarMail(usuario[0].ToString() + "_" + usuario[1].ToString(), "ERROR NO DEFINIDO", "maquintela@gfi.es", true);
                        Authentication ServiceCredentials = new Authentication();
                        ServiceCredentials.UserName = usuario[0].ToString();
                        ServiceCredentials.Password = usuario[1].ToString();

                        if (this.ValidarUsuario(ServiceCredentials.UserName, ServiceCredentials.Password))
                        {
                            //MandarMail("ACCESO", "WS PROO_12", "maquintela@gfi.es", true);
                            // Realizamos la validación de los parámetros de entrada.
                            if (this.ValidarDatosEntrada(contratoVigente, response))
                            {
                                this.GetDatosSolicitud(contratoVigente, response, ServiceCredentials.UserName);
                            }
                        }
                        else
                        {
                            response.AddError(CommonWSError.ErrorCredencialesUsuario);
                        }

                        try
                        {
                            if (this._idTraza.HasValue)
                            {
                                TrazaDB trazaDB = new TrazaDB();
                                trazaDB.InsertarUsuarioTraza(this._idTraza.ToString(), usuario[0].ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("Error en la llamada al DatosSolicitudWS.getDatosSolicitud", LogHelper.Category.BussinessLogic, ex);
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
                    LogHelper.Error("Error en la llamada al DatosSolicitudWS.getDatosSolicitud", LogHelper.Category.BussinessLogic, ex);
                    // TODO: GGB  ver si enviamos un email. en caso de ponerlo parametrizarlo con una variable de 
                    // configuración que indique si está activo el email de errores de WS.
                    // MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data, "ERROR NO DEFINIDO" + usuario, "maquintela@gfi.es", true);
                    response.AddError(CommonWSError.ErrorNoDefinido);
                }

                //// Si no tiene error devolvemos que todo ha ido bien.
                //if (!response.TieneError)
                //{
                //    // TODO: GGB cargar descripciones errores.
                //    response.AddError(CommonWSError.VisitaActualizadaCorrectamente);
                //}
            }
            // Retornamos la respuesta
            return response;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private void GetDatosSolicitud(DatosSolicitudRequest request, DatosSolicitudResponse response, string usuario)
        {
            
                try
                {
                    DataTable dtDatosSolicitud = null;
                    string contrato = request.CodigoContrato;

                    //A partir del contrato Obtenemos los datos que necesitaran
                    DatosSolicitudDB objDatosSolicitudDB = new DatosSolicitudDB();
                    dtDatosSolicitud = objDatosSolicitudDB.GetDatosSolicitud(contrato);

                    //DataRow[] foundRowsContratoVigente = dtContratoVigente.Select("INCIDENCIA_AUTOMATICA = 'S'");

                    //if (foundRowsContratoVigente.Length == 0)
                    //{
                    //    foundRowsContratoVigente = dtContratoVigente.Select("INCIDENCIA_AUTOMATICA = 'N'");
                    //}
                    string sCodContrato = "";
                    string sNumSolicitud = "";
                    string sTipoSolicitud = "";
                    string sFecha_Creacion = "";
                    string sEstadoSolicitud = "";
                    string sFecha_Cierre = "";
                    string sCod_Barras = "";
                    string sMotivoCancelacion = "";
                    // A la espera de lo k nos diga Ruben 21/11/2017
                    string sTipoAveria = "";
                    string sDescripcionAveria = "";
                    

                    //Si tiene solicitudes
                    if (dtDatosSolicitud.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDatosSolicitud.Rows.Count; i++)
                        {
                            sCodContrato = dtDatosSolicitud.Rows[i].ItemArray[0].ToString();
                            sNumSolicitud = dtDatosSolicitud.Rows[i].ItemArray[1].ToString();
                            sTipoSolicitud = dtDatosSolicitud.Rows[i].ItemArray[2].ToString();
                            sFecha_Creacion = dtDatosSolicitud.Rows[i].ItemArray[3].ToString();
                            sEstadoSolicitud = dtDatosSolicitud.Rows[i].ItemArray[4].ToString();
                            sFecha_Cierre = dtDatosSolicitud.Rows[i].ItemArray[5].ToString();
                            sCod_Barras = dtDatosSolicitud.Rows[i].ItemArray[6].ToString();
                            sMotivoCancelacion = dtDatosSolicitud.Rows[i].ItemArray[7].ToString();
                            // A la espera de lo k nos diga Ruben 21/11/2017
                            sTipoAveria = dtDatosSolicitud.Rows[i].ItemArray[8].ToString();
                            sDescripcionAveria = dtDatosSolicitud.Rows[i].ItemArray[9].ToString();

                            response.AddDatosSolicitud(sCodContrato, sNumSolicitud, sTipoSolicitud, sFecha_Creacion, sEstadoSolicitud, sFecha_Cierre, sCod_Barras, sMotivoCancelacion, sTipoAveria, sDescripcionAveria);
                        }
                     // En caso de no devolver solicitudes devolvemos un error indicandolo
                    }
                    else
                    {
                        response.AddErrorDatosSolicitud(DatosSolicitudWSError.ErrorContratoSinSolicitudes);
                    }
                }
                catch
                {
                    ////ERROR DATOS SOLICITUD
                    response.AddErrorDatosSolicitud(DatosSolicitudWSError.ErrorDatosVaciosSolicitud);
                }
            }

          
        

        /// <summary>
        /// validamos los datos de entrada del Contrato Vigente.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        protected override bool ValidarDatosEntrada(WSRequest request, WSResponse response)
        {
            DatosSolicitudRequest contratoVigente = (DatosSolicitudRequest)request;

            // Validamos que tengamos datos de entrada.
            if (contratoVigente == null)
            {
                response.AddErrorDatosSolicitud(DatosSolicitudWSError.ErrorDatosVaciosSolicitud);
                return false;
            }

            // Si llegamos a este punto hemos superado todas las validaciones y retornamos true.
            return true;
        }

        
        protected override void GuardarTrazaLlamada(WSRequest request)
        {
            DatosSolicitudRequest contratoVigente = (DatosSolicitudRequest)request;
            // Generamos el fichero xnl, y guardamos la ruta del mismo en la BB.DD.
            DatosSolicitudDB DatosDB = new DatosSolicitudDB();
            string xml = ToXML(contratoVigente);
            // Guardar la traza y dejar el id de la traza guardada.
            TrazaDB trazaDB = new TrazaDB();
            this._idTraza = trazaDB.InsertarYObteneridTrazaDatosSolicitud("DATOS RECIBIDOS", xml, contratoVigente);
        }

        public string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                // Devolvemos el contenido del xml.
                return xmlDoc.InnerXml;
            }
        }

        protected override void GuardarTrazaResultadoLlamada(WSResponse response)
        {
            DatosSolicitudResponse DatosSolicitudResponse = (DatosSolicitudResponse)response;
            if (this._idTraza.HasValue)
            {
                // Actualizar la traza con el resultado de la operación.
                TrazaDB trazaDB = new TrazaDB();
                if (DatosSolicitudResponse.TieneError)
                {
                    // Si tiene errores.
                    trazaDB.ActualizarTraza(this._idTraza.ToString(), DatosSolicitudResponse.ListaErrores[0].Descripcion.ToString());
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


    }
}