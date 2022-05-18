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
    public class DatosVisitaWS : WSBase
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
        public DatosVisitaResponse getDatosVisita(DatosVisitaRequest contratoVigente)
        {
            DatosVisitaResponse response = new DatosVisitaResponse();

            // Boolean habilitarWS = false;
            //ConfiguracionDTO confHabilitarWS = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_WS);
            //if (confHabilitarWS != null && !string.IsNullOrEmpty(confHabilitarWS.Valor) && Boolean.Parse(confHabilitarWS.Valor))
            //{
            //    habilitarWS = Boolean.Parse(confHabilitarWS.Valor);
            //}

            Boolean habilitarWS = ValidarwebServiceActivo(Enumerados.Configuracion.HABILITAR_DATOSVISITA_WS.ToString());

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
                                this.GetDatosVisita(contratoVigente, response, ServiceCredentials.UserName);
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
                            LogHelper.Error("Error en la llamada al DatosVisitaWS.getDatosVisita", LogHelper.Category.BussinessLogic, ex);
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
                    LogHelper.Error("Error en la llamada al DatosVisitaWS.getDatosVisita", LogHelper.Category.BussinessLogic, ex);
                    // TODO: GGB  ver si enviamos un email. en caso de ponerlo parametrizarlo con una variable de 
                    // configuración que indique si está activo el email de errores de WS.
                    UtilidadesMail.EnviarMensajeError("ERROR NO DEFINIDO en la llamada al DatosVisitaWS.getDatosVisita", ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");
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
        private void GetDatosVisita(DatosVisitaRequest request, DatosVisitaResponse response, string usuario)
        {

            try
            {
                DataTable dtDatosVisita = null;
                string contrato = request.CodigoContrato;

                //A partir del contrato Obtenemos los datos que necesitaran
                DatosVisitaDB objDatosVisitaDB = new DatosVisitaDB();
                dtDatosVisita = objDatosVisitaDB.GetDatosVisita(contrato);

                string COD_CONTRATO = "";
                string COD_VISITA = "";
                string ALIAS = "";
                string PERIODO = "";
                string FECHA_LIMITE_VISITA = "";
                string FECHA_CIERRE = "";
                string COD_BARRAS = "";
                string FECHA_PREVISTA_VISITA = "";
                //20220210 BGN MOD BEG R#36755: Añadir CCBB del CCM en WSDatosVisita
                string CCBB_DOC_VISITA_REALIZADA = "";

                for (int i = 0; i < dtDatosVisita.Rows.Count; i++)
                {
                    COD_CONTRATO = dtDatosVisita.Rows[i].ItemArray[0].ToString();
                    COD_VISITA = dtDatosVisita.Rows[i].ItemArray[1].ToString();
                    ALIAS = dtDatosVisita.Rows[i].ItemArray[2].ToString();
                    PERIODO = dtDatosVisita.Rows[i].ItemArray[3].ToString();
                    FECHA_LIMITE_VISITA = dtDatosVisita.Rows[i].ItemArray[4].ToString();
                    FECHA_CIERRE = dtDatosVisita.Rows[i].ItemArray[5].ToString();
                    COD_BARRAS = dtDatosVisita.Rows[i].ItemArray[6].ToString();
                    FECHA_PREVISTA_VISITA = dtDatosVisita.Rows[i].ItemArray[7].ToString();
                    CCBB_DOC_VISITA_REALIZADA = dtDatosVisita.Rows[i].ItemArray[8].ToString();

                    response.AddDatosVisita(COD_CONTRATO, COD_VISITA, ALIAS, PERIODO, FECHA_LIMITE_VISITA, FECHA_CIERRE, COD_BARRAS, FECHA_PREVISTA_VISITA, CCBB_DOC_VISITA_REALIZADA);
                }
                //20220210 BGN END BEG R#36755: Añadir CCBB del CCM en WSDatosVisita
            }
            catch
            {
                ////ERROR CONTRATO VIGENTE
                response.AddErrorDatosVisita(DatosVisitaWSError.ErrorDatosVaciosVisita);
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
            DatosVisitaRequest contratoVigente = (DatosVisitaRequest)request;

            // Validamos que tengamos datos de entrada.
            if (contratoVigente == null)
            {
                response.AddErrorDatosVisita(DatosVisitaWSError.ErrorDatosVaciosVisita);
                return false;
            }

            // Si llegamos a este punto hemos superado todas las validaciones y retornamos true.
            return true;
        }
        
        //20220211 BGN MOD BEG R#36755: Añadir CCBB del CCM en WSDatosVisita
        protected override void GuardarTrazaLlamada(WSRequest request)
        {
            DatosVisitaRequest contratoVigente = (DatosVisitaRequest)request;
            string xml = ToXML(contratoVigente);
            // Guardar la traza y dejar el id de la traza guardada.
            TrazaDB trazaDB = new TrazaDB();
            this._idTraza = trazaDB.InsertarYObteneridTrazaDatosVisita("DATOS RECIBIDOS", xml, contratoVigente);
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
        //20220211 BGN MOD END R#36755: Añadir CCBB del CCM en WSDatosVisita

        protected override void GuardarTrazaResultadoLlamada(WSResponse response)
        {
            DatosVisitaResponse DatosVisitaResponse = (DatosVisitaResponse)response;
            if (this._idTraza.HasValue)
            {
                // Actualizar la traza con el resultado de la operación.
                TrazaDB trazaDB = new TrazaDB();
                if (DatosVisitaResponse.TieneError)
                {
                    // Si tiene errores.
                    trazaDB.ActualizarTraza(this._idTraza.ToString(), DatosVisitaResponse.ListaErrores[0].Descripcion.ToString());
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