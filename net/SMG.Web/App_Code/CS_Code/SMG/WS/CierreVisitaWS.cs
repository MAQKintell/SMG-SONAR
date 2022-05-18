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
using System.Reflection;
using System.Linq;
using Iberdrola.Commons.Validations;

namespace Iberdrola.SMG.WS
{
    public class CierreVisitaWS : WSBase
    {
        /// <summary>
        /// Cierra la visita con los datos pasados por parámetro
        /// </summary>
        /// <param name="cierreVisita">Objeto de tipo CierreVisitaRequest con los datos 
        /// necesarios para realizar el cierre de la vista</param>
        /// <returns>Objeto de tipo CierreVisitaResponse con el resultado del cierre de la respuesta</returns>

        public Authentication ServiceCredentials;
        
        
        [WebMethod]
        //[SoapHeader("ServiceCredentials")]
        public CierreVisitaResponse CerrarVisita(CierreVisitaRequest cierreVisita, string usuario, string Password)
        {
            //string a = ServiceCredentials.UserName;
            //MandarMail("ACCESO", "WS PRO_-1", "maquintela@gfi.es", true);
            //List<ProvinciaDTO> dt = TablasReferencia.ObtenerProvincias();
            //MandarMail(cierreVisita == null ? "Sí Null" : "No null", "WS PROO_1", "maquintela@gfi.es", true);
            //MandarMail("ACCESO", "WS PROO", "maquintela@gfi.es", true);
            CierreVisitaResponse response = new CierreVisitaResponse();
            //response.AddError(CommonWSError.ErrorCredencialesUsuario);
            //return response;
             
            //Boolean habilitarWS = false;
            //ConfiguracionDTO confHabilitarWS = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_WS);
            //if (confHabilitarWS != null && !string.IsNullOrEmpty(confHabilitarWS.Valor) && Boolean.Parse(confHabilitarWS.Valor))
            //{
            //    habilitarWS = Boolean.Parse(confHabilitarWS.Valor);
            //}

            Boolean habilitarWS = ValidarwebServiceActivo(Enumerados.Configuracion.HABILITAR_CIERREVISITA_WS.ToString());            

            if (!habilitarWS)
            {
                response.AddError(CommonWSError.ErrorWSInactivo);
            }
            else
            {
                TicketCombustionDTO ticketCombustionDTO = new TicketCombustionDTO();

                try
                {
                    bool isGuardarTicketCombustion = false;

                    //MandarMail("ACCESO", "WS PROO_10", "maquintela@gfi.es", true);
                    // Primero guardamos la traza de la llamada.
                    this.GuardarTrazaLlamada(cierreVisita);

                    // Realizamos la validación del usuario.
                    //MandarMail("ACCESO", "WS PROO_11", "maquintela@gfi.es", true);
                    if (this.ValidarUsuario(usuario, Password))
                    {
                        //20201013 BUA ADD R#23245: Guardar el Ticket de combustión

                        //Creamos el objeto ticketCombustion a partir de los datos que vienen del ws
                        ticketCombustionDTO = TicketCombustion.ObtenerTicketCombustionDTOFromRequest(ticketCombustionDTO, cierreVisita, usuario);

                        //MandarMail("ACCESO", "WS PROO_12", "maquintela@gfi.es", true);
                        // Realizamos la validación de los parámetros de entrada.
                        if (this.ValidarDatosEntrada(cierreVisita, response))
                        {
                            CierreVisitaResponse responseTicket = new CierreVisitaResponse();
                            //TicketCombustionDTO ticketCombustionDTO = new TicketCombustionDTO();
                            MantenimientoDB mantenimientoDB = new MantenimientoDB();

                            //Validamos los datos del ticket de combustion
                            if (!response.TieneError && HayQueProcesarTicketCombustion(cierreVisita, response))
                            {
                                ConfiguracionDTO confValidacionesActiva = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_VALIDACION_ACTIVA_VISITAS);

                                //Creamos el objeto ticketCombustion a partir de los datos que vienen del ws
                                //ticketCombustionDTO = TicketCombustion.ObtenerTicketCombustionDTOFromRequest(ticketCombustionDTO, cierreVisita, usuario);

                                //Obtenemos si el contrato es de gasconfort o no
                                ticketCombustionDTO.EsGasConfort = mantenimientoDB.esGasConfort(ticketCombustionDTO.CodigoContrato);

                                isGuardarTicketCombustion = PValidacionesTicketCombustion.ValidarDatosEntradaTicketCombustion(ticketCombustionDTO
                                                                                                                                , responseTicket
                                                                                                                                , confValidacionesActiva.Valor //"CierreVisita"
                                                                                                                                , usuario
                                                                                                                                , null
                                                                                                                                , string.Empty);

                                //Si el proceso de las validaciones del ticket de combustion devuelven un error prevalece sobre el actual.
                                if (!isGuardarTicketCombustion && responseTicket.TieneError)
                                    response = responseTicket;
                            }
                                                        
                            if ((!response.TieneError && !responseTicket.TieneError && !responseTicket.TieneAviso) //Para los casos que no tienen ticket de combustion
                                || (isGuardarTicketCombustion && !responseTicket.TieneError && !responseTicket.TieneAviso) //Para los casos que si tiene ticket de combustion pero no tiene ningun error o aviso.
                                || (responseTicket.TieneAviso && responseTicket.ListaAvisos[0].Descripcion.ToString() == "RENDIMIENTO FUERA DE RANGO")) //Caso especial. En el caso del Rendimiento si no cumple las validaciones, se devuelve un aviso y se puede cerrar la visita.
                            {
                                //MandarMail("ACCESO", "WS PROO_13", "maquintela@gfi.es", true);
                                // Realizamos el cierre de la visita.
                                this.RealizarCierreVisita(cierreVisita, response, usuario);

                                //MandarMail("ACCESO", "WS PROO_14", "maquintela@gfi.es", true);
                                this.InformarEquipamiento(cierreVisita, response);
                            }

                            //Guardamos el ficheros de conducto de humos
                            if (!response.TieneError && 
                                (!string.IsNullOrEmpty(cierreVisita.TipoEquipamiento) && cierreVisita.TipoEquipamiento.ToString() == "1")
                                    || isGuardarTicketCombustion)
                            {
                                DocumentoDTO documentoDTOConductoHumos = null;
                                documentoDTOConductoHumos = this.GuardarFicheroPorNombre(cierreVisita, response, "ConductoHumos"); //Guardamos el fichero de conducto de humos
                                if (documentoDTOConductoHumos != null)
                                    ticketCombustionDTO.IdFicheroConductoHumos = documentoDTOConductoHumos.IdDocumento;


                                //Obtenemos el documento correspondiente al ticket de combustion para guardar su correspondiente id
                                DocumentoDTO documentoDTOTicket = Documento.ObtenerPorNombreDocumento(ticketCombustionDTO.CodigoContrato, null, ticketCombustionDTO.CodigoVisita, cierreVisita.NombreFichero);

                                if (documentoDTOTicket == null)
                                    documentoDTOTicket = this.GuardarFicheroPorNombre(cierreVisita, response, ""); //Guardamos el fichero Ticket de Combustion

                                if (documentoDTOTicket != null)
                                    ticketCombustionDTO.IdFicheroTicketCombustion = documentoDTOTicket.IdDocumento;
                            }

                            //Guardamos los datos relacionados con el ticket de combustion
                            if (!response.TieneError && isGuardarTicketCombustion)
                            {
                                this.GuardarTicketCombustionRequest(cierreVisita, ticketCombustionDTO, response, usuario);

                                //Si el proceso de las validaciones del ticket de combustion devuelven un error prevalece sobre el actual.
                                if (responseTicket.TieneError || responseTicket.TieneAviso)
                                    response = responseTicket;

                                //BUA ADD R#29496: Guardar el Ticket de combustión Fase 5, fichero y mail al cliente.
                                //Enviamos el correo con el informe de revision de mantenimiento.
                                //if (!response.TieneError)
                                //    PValidacionesTicketCombustion.EnvioCorreoInformeRevisionMantenimiento(ticketCombustionDTO);
                                //BUA END R#29496: Guardar el Ticket de combustión Fase 5, fichero y mail al cliente.
                            }
                            //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
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
                    LogHelper.Error("Error en la llamada al CierreVisitaWS.CerrarVisita", LogHelper.Category.BussinessLogic, ex);
                    // TODO: GGB  ver si enviamos un email. en caso de ponerlo parametrizarlo con una variable de 
                    // configuración que indique si está activo el email de errores de WS.
                    //MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data, "ERROR NO DEFINIDO" + usuario, "maquintela@gfi.es;eaceves@gfi.es", true);


                    //BUA
                    //Dejar en fichero el error que se ha producido.
                    //string path = @"\\CLBILANAS01\Gesdocor\Comercial\GD_SMG\PRODUCCION\CONTRATO_GAS_CONFORT\CONTRATOS_GENERADOS\LOG_TEMPORAL_WS\id_traza_" + this._idTraza.ToString() + ".txt";

                    //string texto = "Id_traza: " + this._idTraza + ". " + ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data;
                    //if (!File.Exists(path))
                    //{
                    //    // Create a file to write to.
                    //    using (StreamWriter sw = File.CreateText(path))
                    //    {
                    //        sw.WriteLine(texto);
                    //    }
                    //}

                    //// Open the file to read from.
                    //using (StreamReader sr = File.OpenText(path))
                    //{
                    //    string s = "";
                    //    while ((s = sr.ReadLine()) != null)
                    //    {
                    //        Console.WriteLine(s);
                    //    }
                    //}

                    //END BUA

                    response.AddError(CommonWSError.ErrorNoDefinido);
                }
                finally
                {
                    string resultado = "TODO CORRECTO";

                    if (response.TieneError)
                        resultado =  response.ListaErrores[0].Descripcion.ToString();
                    else if (response.TieneAviso)
                        resultado = response.ListaAvisos[0].Descripcion.ToString();

                    //Guardamos en la tabla TICKET_COMBUSTION_RESUMEN_PROCESADOS el ticket de combustion enviado
                    decimal? id = PValidacionesTicketCombustion.GuardarTicketCombustionResumenResultado(ticketCombustionDTO, "WS", this._idTraza, usuario, resultado);
                }

                //// Si no tiene error devolvemos que todo ha ido bien.
                //if (!response.TieneError)
                //{
                //    // TODO: GGB cargar descripciones errores.
                //    response.AddError(CommonWSError.VisitaActualizadaCorrectamente);
                //}
            }
            // Establecemos el idioma para sacar los mensajes.
            //establecerIdioma(response);

            // Retornamos la respuesta
            return response;

        }

        private void establecerIdioma(CierreVisitaResponse response)
        {
            //response.Castellano = true;
        }

        //private void GuardarFichero(CierreVisitaRequest cierreVisita, CierreVisitaResponse response)
        //{
        //    String Destino = (string)Documentos.ObtenerRutaFicheros();
        //    //string rutaFichero = "\\\\Clbilanas01\\ADMON_PYS\\PRODUCTOS Y SERVICIOS\\08 SMG\\" + cierreVisita.NombreFichero;
        //    //string rutaFichero = (string)Documentos.ObtenerRutaFicherosTemporal() + "\\" + cierreVisita.NombreFichero;
        //    string rutaFichero = (string)Documentos.ObtenerRutaFicherosTemporal() + cierreVisita.NombreFichero;
        //    //string rutaFichero = Server.MapPath("~/Excel") + "\\" + cierreVisita.NombreFichero;
        //    // COMPROBAMOS QUE LA VISITA SE QUIERA PASAR A CERRADA O CERRADA PEDIENTE REALIZAR REPARACION.
        //    if (int.Parse(cierreVisita.EstadoVisita) == (int)Enumerados.EstadosVisita.Cerrada || int.Parse(cierreVisita.EstadoVisita) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)
        //    {
        //        //cerrada (o cerrada pendiente de reparación)
        //        // COMPROBAMOS EL NOMBRE DEL FICHERO
        //        if (!ComprobarNombreFichero(cierreVisita.NombreFichero))
        //        {
        //            response.AddError(CierreVisitaWSError.ErrorNombreFicheroAdjunto);
        //        }
        //        else
        //        {
        //            // COMPROBAMOS EL TAMAÑO DEL FICHERO.
        //            int maxSize = 1713718;
        //            int minSize = 100;

        //            if (cierreVisita.ContenidoFichero.Length <= maxSize && cierreVisita.ContenidoFichero.Length >= minSize)
        //            {
        //                //// COMPROBAMOS QUE NO EXISTA UN FICHERO CON ESE NOMBRE EN LA BB.DD.
        //                //Int32 regFicheros = Documentos.ObtenerRegistroFicherosPorNombre(cierreVisita.NombreFichero);
        //                //if (regFicheros <= 0)
        //                //{

        //                    // COPIAMOS EL FICHERO.
        //                    try
        //                    {
        //                        long tamanhio = 0;
        //                        using (ManejadorFicheros.GetImpersonator())
        //                        {
        //                            // GENERAMOS EL FICHERO.
        //                            //File.WriteAllBytes(rutaFichero, cierreVisita.ContenidoFichero);
        //                            //converting back to tiff
        //                            //StreamReader reader = new StreamReader(cierreVisita.ContenidoFichero);
        //                            byte[] base64Convert = new byte[cierreVisita.ContenidoFichero.Length];
        //                            base64Convert = cierreVisita.ContenidoFichero;

        //                            FileStream streamWriter = new FileStream(rutaFichero, FileMode.Create);
        //                            streamWriter.Write(base64Convert, 0, cierreVisita.ContenidoFichero.Length);
        //                            streamWriter.Close();
                                    
        //                            FileInfo info = new System.IO.FileInfo(rutaFichero);
        //                            tamanhio = info.Length;
        //                        }

        //                        if (tamanhio <= 0)
        //                        {
        //                            response.AddError(CierreVisitaWSError.ErrorLongitudFichero);
        //                        }
        //                        else
        //                        {
        //                            // MOVEMOS EL FICHERO A NUESTRO DIRECTORIO.
                                   
        //                            //try
        //                            //{
        //                            // INSERTAMOS REGISTRO EN LA BB.DD.
        //                            String Login = cierreVisita.UsuarioCierreVisita;
        //                            string[] sNombreFichero = cierreVisita.NombreFichero.Split('_');
        //                            string TipoDocumentoSinExtension = sNombreFichero[3].ToString();
        //                            int posicionPunto = TipoDocumentoSinExtension.IndexOf(".");

        //                            TipoDocumentoSinExtension = TipoDocumentoSinExtension.Substring(0, posicionPunto);

        //                            int tipoDocumento = Int32.Parse(TipoDocumentoSinExtension);

        //                            string nombreficheroPRO = cierreVisita.CodigoContrato.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + cierreVisita.Proveedor.Substring(0, 3) + "_" + TipoDocumentoSinExtension;
        //                            // Si la visita tiene CCBB, le ponemos como nombre el CCBB, si no dejamos el deContrato+Fecha+Proveedor+TipoDoucmento.
        //                            //string CCBB = (string)Documentos.ObtenerCCBBPorContrato(cierreVisita.CodigoContrato);
        //                            //if (!string.IsNullOrEmpty(CCBB))
        //                            //{
        //                            //    nombreficheroPRO = CCBB;
        //                            //}

        //                            // Añadimos la extension.
        //                            if (cierreVisita.NombreFichero.IndexOf(".pdf") >= 0)
        //                            {
        //                                nombreficheroPRO = nombreficheroPRO + ".pdf";
        //                            }
        //                            else
        //                            {
        //                                nombreficheroPRO = nombreficheroPRO + ".tiff";
        //                            }

        //                            //20210310 BGN/Kintell Comprobamos que el fichero no exista ya para la misma solicitud o visita, porque pueden mandarnos el mismo fichero al cerrar visita y solicitud de averia a la vez
        //                            //Paco: "cuando hacen la avería si la dejan reparada y el mantenimiento está abierto la app se lo dice y aprovechan para hacerlo"
        //                            DocumentoDTO docExistente = Documento.ObtenerPorNombreDocumento(cierreVisita.CodigoContrato, null, cierreVisita.CodigoVisita, nombreficheroPRO);
        //                            if (docExistente == null)
        //                            {
        //                                Destino = Destino + nombreficheroPRO.Trim();

        //                                FileUtils.FileCopy(ManejadorFicheros.GetImpersonator(), rutaFichero, Destino, false);
        //                                //FileUtils.FileMove(ManejadorFicheros.GetImpersonator(), rutaFichero, Destino + cierreVisita.NombreFichero, false);

        //                                //File.WriteAllBytes(Destino + cierreVisita.NombreFichero, cierreVisita.ContenidoFichero);
        //                                //}
        //                                //catch
        //                                //{
        //                                //    response.AddError(CierreVisitaWSError.ErrorFicheroAdjunto);
        //                                //}

        //                            //20210119 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        //                            //Documentos.InsertaDocumentoCargado(cierreVisita.CodigoContrato, cierreVisita.CodigoVisita, null, nombreficheroPRO, tipoDocumento, null, null, Login);
        //                            DocumentoDTO documentoDto = new DocumentoDTO();
        //                            documentoDto.CodContrato = cierreVisita.CodigoContrato;
        //                            documentoDto.CodVisita = cierreVisita.CodigoVisita;
        //                            documentoDto.NombreDocumento = nombreficheroPRO.Trim();
        //                            documentoDto.IdTipoDocumento = tipoDocumento;
        //                            documentoDto.EnviarADelta = true;
        //                            documentoDto = Documento.Insertar(documentoDto, Login);
        //                            //20210119 BGN MOD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital 

        //                            //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
        //                            //Guardamos el nombre del fichero en la llamada para poder recuperarlo posteriormente 
        //                            //en el proceso de guardado del ticket de combustion
        //                            cierreVisita.NombreFichero = nombreficheroPRO;
        //                                //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
        //                            }
        //                            else
        //                            {
        //                                response.AddError(CierreVisitaWSError.ErrorFicheroYaExistente);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        //MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_2000_" + rutaFichero + "_" + Destino + "_", "ERROR SUBIDA FICHERO", "maquintela@gfi.es;eaceves@gfi.es", true);
        //                        response.AddError(CierreVisitaWSError.ErrorInsertFicheroAdjunto);
        //                    }
        //                //}
        //                //else
        //                //{
        //                //    response.AddError(CierreVisitaWSError.ErrorFicheroYaExistente);
        //                //}
        //            }
        //            else
        //            {
        //                response.AddError(CierreVisitaWSError.ErrorLongitudFichero);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //MandarMail("ACCESO", "WS PROO_3", "maquintela@gfi.es;eaceves@gfi.es", true);
        //        // EN EL CASO DE QUE NOS LLEGUE FICHERO PERO EL ESTADO NO SEA CERRADA O CERRADA PENDIENTE REPARACION, SACAMOS ERROR.
        //        if (cierreVisita.ContenidoFichero != null && cierreVisita.ContenidoFichero.Length != 0)
        //        {
        //            response.AddError(CierreVisitaWSError.ErrorEstadoVisitaConFicheroAdjunto);
        //        }
        //    }
        //}

        //private DocumentoDTO GuardarFicheroPorNombreTEMP(CierreVisitaRequest cierreVisita
        //                                        , CierreVisitaResponse response
        //                                        , string pNombreFichero)
        //{
        //    try
        //    {
        //        //Comprobamos que existan los campos relacionados con el fichero a procesar.
        //        string prefijo = (pNombreFichero == "ConductoHumos" ? "Fichero" : "ContenidoFichero");

        //        Object objNombreCampo = cierreVisita.GetType().GetProperty("NombreFichero" + pNombreFichero).GetValue(cierreVisita, null);
        //        Object objValorCampo = cierreVisita.GetType().GetProperty(prefijo + pNombreFichero).GetValue(cierreVisita, null);

        //        if (objNombreCampo == null || string.IsNullOrEmpty(objNombreCampo.ToString())
        //            || objValorCampo == null || string.IsNullOrEmpty(objValorCampo.ToString()))
        //        {
        //            return null;
        //        }

        //        //Recuperamos el nombre del fichero
        //        string nombreFichero = cierreVisita.GetType().GetProperty("NombreFichero" + pNombreFichero).GetValue(cierreVisita, null).ToString();

        //        string rutaOrigen = (string)Documentos.ObtenerRutaFicherosTemporal() + nombreFichero;
        //        String rutaDestino = (string)Documentos.ObtenerRutaFicheros();

        //        String Login = cierreVisita.UsuarioCierreVisita;
        //        string[] sNombreFichero = nombreFichero.Split('_');
        //        string TipoDocumentoSinExtension = sNombreFichero[3].ToString();
        //        int posicionPunto = TipoDocumentoSinExtension.IndexOf(".");

        //        TipoDocumentoSinExtension = TipoDocumentoSinExtension.Substring(0, posicionPunto);

        //        int idTipoDocumento = Documento.ObtenerIdTipoDocumento(TipoDocumentoSinExtension);

        //        string nombreficheroPRO = cierreVisita.CodigoContrato.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + cierreVisita.Proveedor.Substring(0, 3) + "_" + TipoDocumentoSinExtension;

        //        // Si la visita tiene CCBB, le ponemos como nombre el CCBB, si no dejamos el deContrato+Fecha+Proveedor+TipoDoucmento.
        //        //string CCBB = (string)Documentos.ObtenerCCBBPorContrato(cierreVisita.CodigoContrato);

        //        //if (!string.IsNullOrEmpty(CCBB))
        //        //{
        //        //    nombreficheroPRO = CCBB;
        //        //}

        //        // Añadimos la extension.
        //        if (nombreFichero.IndexOf(".pdf") >= 0)
        //            nombreficheroPRO = nombreficheroPRO + ".pdf";
        //        else
        //            nombreficheroPRO = nombreficheroPRO + ".tiff";

        //        //Comprobamos que no exista un fichero con ese nombre en la BBDD.
        //        //20210310 BGN/Kintell Comprobamos que el fichero no exista ya para la misma solicitud o visita, porque pueden mandarnos el mismo fichero al cerrar visita y solicitud de averia a la vez
        //        //Paco: "cuando hacen la avería si la dejan reparada y el mantenimiento está abierto la app se lo dice y aprovechan para hacerlo"
        //        DocumentoDTO docExistente = Documento.ObtenerPorNombreDocumento(cierreVisita.CodigoContrato, null, cierreVisita.CodigoVisita, nombreficheroPRO);
        //        if (docExistente != null)
        //        {
        //            response.AddError(CierreVisitaWSError.ErrorFicheroYaExistente);
        //            return null;
        //        }

        //        //Comprobamos que nombre del fichero tenga formato correcto.
        //        //if (!ComprobarNombreFichero(nombreFichero))
        //        //{
        //        //    response.AddError(CierreVisitaWSError.ErrorNombreFicheroAdjunto);
        //        //    return null;
        //        //}

        //        // COMPROBAMOS EL TAMAÑO DEL FICHERO.
        //        //Recuperamos el contenido del fichero

        //        //string prefijo = (pNombreFichero == "ConductoHumos" ? "Fichero" : "ContenidoFichero");
        //        byte[] tempContenidoFichero = (byte[])cierreVisita.GetType().GetProperty(prefijo + pNombreFichero).GetValue(cierreVisita, null);
                
        //        byte[] contenidoFichero = new byte[tempContenidoFichero.Length];
        //        contenidoFichero = tempContenidoFichero;

        //        int minSize = 100;
        //        int maxSize = 1713718;

        //        if (!(contenidoFichero.Length >= minSize && contenidoFichero.Length <= maxSize))
        //        {
        //            response.AddError(CierreVisitaWSError.ErrorLongitudFichero);
        //            return null;
        //        }

        //        // Generamos el fichero a partir del codigo base64 en la ruta temporal.

        //        long tamanhio = 0;
        //        using (ManejadorFicheros.GetImpersonator())
        //        {
        //            // GENERAMOS EL FICHERO.
        //            FileStream streamWriter = new FileStream(rutaOrigen, FileMode.Create);
        //            streamWriter.Write(contenidoFichero, 0, contenidoFichero.Length);
        //            streamWriter.Close();

        //            FileInfo info = new System.IO.FileInfo(rutaOrigen);
        //            tamanhio = info.Length;
        //        }

        //        if (tamanhio <= 0)
        //        {
        //            response.AddError(CierreVisitaWSError.ErrorLongitudFichero);
        //            return null;
        //        }

        //        // MOVEMOS EL FICHERO AL DIRECTORIO FINAL.
        //        rutaDestino = rutaDestino + nombreficheroPRO.Trim();
        //        FileUtils.FileCopy(ManejadorFicheros.GetImpersonator(), rutaOrigen, rutaDestino, false);
                
        //        //Insertamos los datos del documento en la tabla documento
        //        DocumentoDTO documentoDto = new DocumentoDTO();
        //        documentoDto.CodContrato = cierreVisita.CodigoContrato;
        //        documentoDto.CodVisita = cierreVisita.CodigoVisita;
        //        documentoDto.NombreDocumento = nombreficheroPRO;
        //        documentoDto.IdTipoDocumento = idTipoDocumento;
        //        //20210118 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        //        documentoDto.EnviarADelta = true;
        //        //20210118 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

        //        documentoDto = Documento.Insertar(documentoDto, cierreVisita.UsuarioCierreVisita);

        //        return documentoDto;

        //    }
        //    catch (Exception ex)
        //    {
        //        response.AddError(CierreVisitaWSError.ErrorInsertFicheroAdjunto);

        //        return null;
        //    }
        //}

        private DocumentoDTO GuardarFicheroPorNombre(CierreVisitaRequest cierreVisita
                                                , CierreVisitaResponse response
                                                , string pNombreFichero)
        {
            try
            {
                //string rutaOrigen = (string)Documentos.ObtenerRutaFicherosTemporal() + nombreFichero;
                String rutaDestino = (string)Documentos.ObtenerRutaFicheros();

                //Comprobamos que existan los campos relacionados con el fichero a procesar.
                string prefijo = (pNombreFichero == "ConductoHumos" ? "Fichero" : "ContenidoFichero");

                Object objNombreCampo = cierreVisita.GetType().GetProperty("NombreFichero" + pNombreFichero).GetValue(cierreVisita, null);
                Object objValorCampo = cierreVisita.GetType().GetProperty(prefijo + pNombreFichero).GetValue(cierreVisita, null);

                if (objNombreCampo == null || string.IsNullOrEmpty(objNombreCampo.ToString())
                    || objValorCampo == null || string.IsNullOrEmpty(objValorCampo.ToString()))
                {
                    return null;
                }

                //Recuperamos el nombre del fichero
                string nombreFichero = cierreVisita.GetType().GetProperty("NombreFichero" + pNombreFichero).GetValue(cierreVisita, null).ToString();

                //Descomponemos el nombre de fichero
                String Login = cierreVisita.UsuarioCierreVisita;
                string[] sNombreFichero = nombreFichero.Split('_');
                string TipoDocumentoSinExtension = sNombreFichero[3].ToString();
                int posicionPunto = TipoDocumentoSinExtension.IndexOf(".");

                TipoDocumentoSinExtension = TipoDocumentoSinExtension.Substring(0, posicionPunto);

                int idTipoDocumento = Documento.ObtenerIdTipoDocumento(TipoDocumentoSinExtension);

                string nombreficheroPRO = cierreVisita.CodigoContrato.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + cierreVisita.Proveedor.Substring(0, 3) + "_" + TipoDocumentoSinExtension;

                // Añadimos la extension.
                if (nombreFichero.IndexOf(".pdf") >= 0)
                    nombreficheroPRO = nombreficheroPRO + ".pdf";
                else
                    nombreficheroPRO = nombreficheroPRO + ".tiff";

                //Construimos la ruta + nombrefichero final
                rutaDestino = rutaDestino + nombreficheroPRO.Trim();

                //Comprobamos que nombre del fichero tenga formato correcto.
                //if (!ComprobarNombreFichero(nombreFichero))
                //{
                //    response.AddError(CierreVisitaWSError.ErrorNombreFicheroAdjunto);
                //    return null;
                //}

                //Recuperamos el contenido del fichero
                byte[] tempContenidoFichero = (byte[])cierreVisita.GetType().GetProperty(prefijo + pNombreFichero).GetValue(cierreVisita, null);
                byte[] contenidoFichero = new byte[tempContenidoFichero.Length];
                contenidoFichero = tempContenidoFichero;

                //Generamos el fichero a partir del codigo base64 en la ruta temporal.
                long tamanhio = 0;
                using (ManejadorFicheros.GetImpersonator())
                {
                    // GENERAMOS EL FICHERO.
                    FileStream streamWriter = new FileStream(rutaDestino, FileMode.Create);
                    streamWriter.Write(contenidoFichero, 0, contenidoFichero.Length);
                    streamWriter.Close();

                    FileInfo info = new System.IO.FileInfo(rutaDestino);
                    tamanhio = info.Length;
                }

                if (tamanhio <= 0)
                {
                    response.AddError(CierreVisitaWSError.ErrorLongitudFichero);
                    return null;
                }

                //Insertamos los datos del documento en la tabla documento
                DocumentoDTO documentoDto = new DocumentoDTO();
                documentoDto.CodContrato = cierreVisita.CodigoContrato;
                documentoDto.CodVisita = cierreVisita.CodigoVisita;
                documentoDto.NombreDocumento = nombreficheroPRO;
                documentoDto.IdTipoDocumento = idTipoDocumento;
                //20210118 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                documentoDto.EnviarADelta = true;
                //20210118 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

                documentoDto = Documento.Insertar(documentoDto, cierreVisita.UsuarioCierreVisita);

                return documentoDto;

            }
            catch (Exception ex)
            {
                response.AddError(CierreVisitaWSError.ErrorInsertFicheroAdjunto);

                return null;
            }
        }



        private Boolean HayQueProcesarTicketCombustion(CierreVisitaRequest cierreVisita
                                                        , WSResponse response)
        {
            bool result = false;
            bool existEstado = false;
            bool existProveedor = false;
            bool activoProcTicketCombustion = false;

            //Si la visita es de tipo Cocina o cuando en la request viene en tipoequipamiento un 1(Cocina) no se debe de validar ni guardar el ticket de combustion aunque le corresponda por estado
            //Se ha detectado que estan cerrando visitas a traves del ws sin enviar el tipoequipamiento=1, cuando la visita es de tipo equipamiento
            //bool isCocina = false;

            //List<EquipamientoDTO> listaEquipamiento = Equipamientos.ObtenerEquipamientos(cierreVisita.CodigoContrato, 1);
            //if (listaEquipamiento != null && listaEquipamiento.Count > 0)
            //    isCocina = listaEquipamiento.Last().IdTipoEquipamiento == 1;

            if (string.IsNullOrEmpty(cierreVisita.TipoEquipamiento))
            {
                response.AddError(TicketCombustionError.TipoEquipamientoObligatorio);
                return result;
            }

            if (!string.IsNullOrEmpty(cierreVisita.TipoEquipamiento) && cierreVisita.TipoEquipamiento.ToString() == "1")
                return false;

            ConfiguracionDTO confEstadosVisita = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_ESTADOS_VISITAS_A_TRATAR);
            ConfiguracionDTO confProveedores = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_PROVEEDORES_A_TRATAR);

            ConfiguracionDTO confProcesarValidaciones = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_ACTIVAR_VALIDACIONES_VISITAS);

            activoProcTicketCombustion = bool.Parse(confProcesarValidaciones.Valor);

            string[] arrEstados = confEstadosVisita.Valor.Split(';');
            string[] arrProveedoresTratar = confProveedores.Valor.Split(';');

            //Miramos que el proveedor que viene en la request sea uno de los proveedores parametrizados en el p_config
            foreach (string proveedor in arrProveedoresTratar)
            {
                if (proveedor.ToUpper() == cierreVisita.Proveedor.ToUpper())
                    existProveedor = true;
            }

            //Miramos que el estado de la visita que viene en la request sea uno de los estados parametrizado en el p_config
            foreach (string estado in arrEstados)
            {
                if (Enum.IsDefined(typeof(Enumerados.EstadosVisita), estado))
                {
                    Enumerados.EstadosVisita enumEstados = (Enumerados.EstadosVisita)Enum.Parse(typeof(Enumerados.EstadosVisita), estado);

                    if (!string.IsNullOrEmpty(enumEstados.ToString()) && int.Parse(cierreVisita.EstadoVisita.ToString()) == (int)enumEstados)
                        existEstado = true;
                }
            }

            if (!activoProcTicketCombustion || (activoProcTicketCombustion && existProveedor && existEstado))
            {
                if (!string.IsNullOrEmpty(cierreVisita.TicketCombustion) && cierreVisita.TicketCombustion.Trim() == "S")
                    result = true;
                else
                    if (activoProcTicketCombustion)
                        response.AddError(TicketCombustionError.TicketCombustionObligatorioEnEsteEstado);
            }
              
            return result;
        }
               

        private Boolean ComprobarNombreFichero(String nombre)
        {
            // 04/05/2016
            // Formato=CCBB_COD-CONTRATO_PROVEEDOR_TIPO-DOCUMENTO
            // QUITAMOS LA EXTENSION.

            // 13/12/2016 CAMBIO EN LA NOMENCLATURA, AHORA ES:
            //COD BARRAS = 101400867320161212097
            //CONTRATO = 1014008673
            //TIPO DOC = 97
            //“1019799850_20170206_MAP_097.pdf”

            nombre = nombre.Substring(0, nombre.Length - 4);
            string[] sNombreFichero = nombre.Split('_');
            // COMPROBAMOS QUE TENGA 4 PARAMETROS EN EL NOMBRE.
            if (sNombreFichero.Length == 4)
            {
                //// COMPROBAMOS QUE EL PARAMETRO DEL CCBB TENGA 13 O 14 CARACTERES.
                //if (sNombreFichero[0].Length == 14 || sNombreFichero[0].Length == 13)
                //{
                    // COMPROBAMOS QUE EL PARAMETRO DEL CONTRATO TENGA 10 CARACTERES.
                    if (sNombreFichero[0].Length == 10)
                    {
                        // COMPROBAMOS QUE EL PARAMTERO DEL TIPO DEL DOCUMENTO SEA ALGUNO DE LOS ACEPTADOS.
                        if (int.Parse(sNombreFichero[3].ToString().Substring(0, 2)) == (int)Enumerados.TipoDocumentoAnexarfichero.InformeRevisionPorPrecinte ||
                             int.Parse(sNombreFichero[3].ToString().Substring(0, 2)) == (int)Enumerados.TipoDocumentoAnexarfichero.InformesMantenimientoGAS ||
                             int.Parse(sNombreFichero[3].ToString().Substring(0, 2)) == (int)Enumerados.TipoDocumentoAnexarfichero.SMG_Averias ||
                             int.Parse(sNombreFichero[3].ToString().Substring(0, 2)) == (int)Enumerados.TipoDocumentoAnexarfichero.SMG_Reparaciones)
                        {
                            return true;
                        }
                        else
                        {
                            //07/02/2017
                            //return false;
                            // Pongo esto para saltarnos la comprobación del tipo documento, porke no creo k la este haciendo bien, excepto si nos llega un 97
                            // Porque si nos llega 3 digitos, esto no puede estar haciendolo bien, asi que quito esta comprobación hasta que se ponga una nomenclatura
                            // para todos los proveedores.
                            return true;
                        }
                        
                    }
                    else
                    {
                        return false;
                    }
                //}
                //else
                //{
                //    return false;
                //}
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Se realiza la validación de los datos de entrada.
        /// </summary>
        /// <param name="cierreVisita"></param>
        /// <returns></returns>
        protected override bool ValidarDatosEntrada(WSRequest request, WSResponse response)
        {
            CierreVisitaRequest cierreVisita = (CierreVisitaRequest)request;
            
            // Validamos que tengamos datos de entrada.
            if (cierreVisita == null)
            {
                response.AddError(CierreVisitaWSError.ErrorDatosEntradaCierreVisita);
                return false;
            }

            // Validamos que el código de la visita esté en un rango de valores
            if (cierreVisita.CodigoVisita < 0 || cierreVisita.CodigoVisita > 999)
            {
                response.AddError(CierreVisitaWSError.ErrorCodigoVisitaIncorrecto);
                return false;
            }

            // Validamos que el código de estado de la visita sea correcto.
            using (VisitasWSDB visita = new VisitasWSDB())
            {
                if (!visita.EstadoVisitaExiste(cierreVisita.EstadoVisita))
                {
                    response.AddError(CierreVisitaWSError.ErrorCodigoEstadoVisitaIncorrecto);
                    return false;
                }
            }

            // Validamos que el proveedor no ha cerrado más de 12 visitas en el día
            // Esa validación sólo la hacemos cuando está informada la fecha de la visita
            // si no está informada es porque se cierra la visita por otro motivo, no porque
            // haya asistido el técnico.
            if (this.TecnicoSuperaLimiteVisitasDia(cierreVisita))
            {
                response.AddError(CierreVisitaWSError.ErrorTecnicoSuperaLimiteVisitasDia);
                return false;
            }

            // Si el teléfono está informado validamos que tenga 12 dígitos.
            if (!this.ValidarTelefonoContacto1(cierreVisita.TelefonoContacto1))
            {
                response.AddError(CierreVisitaWSError.ErrorFormatoTelefonoContactoConPrefijo);
                return false;
            }
            //20220218 BGN ADD BEG R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
            if (!this.ValidarTelefonoContacto1(cierreVisita.TelefonoContacto2))
            {
                response.AddError(CierreVisitaWSError.ErrorFormatoTelefonoContactoConPrefijo);
                return false;
            }
            //20220218 BGN ADD END R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente

            //// Si el estado de la visita es cerrada validamos que el código de barras esté informado
            //// y tenga 13 dígitos.
            //if (!this.ValidarCodigoBarrasVisita(int.Parse(cierreVisita.EstadoVisita), cierreVisita.CodigoBarrasVisita))
            //{
            //    response.AddError(CierreVisitaWSError.ErrorCodigoBarrasVisita);
            //    return false;
            //}

            // Si el estado de la visita es cerrada validamos que el código de barras esté informado
            // y tenga 13 dígitos.
            if (!this.ValidarCodigoBarrasReparacion((bool)cierreVisita.TieneReparacion, cierreVisita.CodigoBarrasReparacion))
            {
                response.AddError(CierreVisitaWSError.ErrorCodigoBarrasReparacion);
                return false;
            }


            DataTable datosVisita = ObtenerDatosVisitas(cierreVisita.CodigoContrato, cierreVisita.CodigoVisita);

            // Comprobamos que la visita Existe
            if (datosVisita == null || datosVisita.Rows.Count == 0)
            {
                response.AddError(CierreVisitaWSError.ErrorVisitaNoEncontrada);
                return false;
            }
            // Comprobamos que la visita que nos viene es la última
            if (cierreVisita.CodigoVisita != int.Parse(datosVisita.Rows[0]["COD_VISITA"].ToString()))
            {
                response.AddError(CierreVisitaWSError.ErrorCodigoVisitaNoCoincideUltimaVisitaContrato);
                return false;
            }

            int intEstadoVisitaActual = int.Parse(datosVisita.Rows[0]["COD_ESTADO_VISITA"].ToString());

            // Comprobamos que la visita no esté ya cerrada
            if (intEstadoVisitaActual == (int)Enumerados.EstadosVisita.Cerrada)
            {
                response.AddError(CierreVisitaWSError.ErrorCodigoVisitaYaCerrada);
                return false;
            }

            // Obtenemos cual será la siguiente fecha de movimiento, 
            // si no hay fecha de movimiento anterior, se trata de un error
            DateTime? fechaUltimoHistorico = null;
            if (!ObtenerFechaUltimoVisitaHistorico(cierreVisita, ref fechaUltimoHistorico))
            {
                response.AddError(CierreVisitaWSError.ErrorHistoricoVisitaNoEncontrado);
                return false;
            }

            // Comprobamos que el técnico es correcto
            DateTime fechaVistia = DateTime.Parse("01/01/1900");
            if (cierreVisita.FechaVisita != null) { fechaVistia = (DateTime)cierreVisita.FechaVisita; }
            if (!this.ValidarTecnico(cierreVisita.IdTecnico, int.Parse(cierreVisita.EstadoVisita), cierreVisita.Proveedor, fechaVistia))
            {
                response.AddError(CierreVisitaWSError.ErrorTecnicoIncorrecto);
                return false;
            }

            // Comprobamos que el tipo de la visita es correcto
            //Se migra la comprobacion al metodo ValidarDatosEntrada
            if (!this.ValidarTipoVisita(cierreVisita.TipoVisita, int.Parse(cierreVisita.EstadoVisita)))
            {
                response.AddError(CierreVisitaWSError.ErrorTipoVisitaIncorrecto);
                return false;
            }

            //Obtenemos la fecha de límite de visita
            DateTime fechaLimiteVisitaActual = this.ObtenerFechaVisita(datosVisita);

            // Comprobamos la fecha de realización de la fecha de la visita.
            if (!this.ValidarFechaCierreVisita(cierreVisita.FechaVisita, fechaLimiteVisitaActual, int.Parse(cierreVisita.EstadoVisita)))
            {
                response.AddError(CierreVisitaWSError.ErrorFechaVisitaSuperaFechaLimite);
                return false;
            }

            // Comprobamos la fecha de realización de la factura.
            if (!this.ValidarDatosFacturaCierreVisita(cierreVisita))
            {
                response.AddError(CierreVisitaWSError.ErrorFaltanDatosObligatoriosCierreVisita);
                return false;
            }

            // Comprobamos la cancelación por segunda vez
            if (!this.ValidarDatosFacturaCanceladaDosVeces(cierreVisita))
            {
                response.AddError(CierreVisitaWSError.ErrorFaltanDatosObligatoriosAusenteSegundaVez);
                return false;
            }
            
            //Si la visita que se esta cerrando tiene asociada una solicitud de averia abierta(Creada por anomalia a la hora de procesar el ticket) se valida que que la visita se encuentra en estado "visita erronea"
            PValidacionesTicketCombustionDB pValidacionesTicketCombustionDB = new PValidacionesTicketCombustionDB();
            DataTable dtSol = pValidacionesTicketCombustionDB.ObtenerSolicitudAveriaAsociadaVisita(cierreVisita.CodigoContrato, cierreVisita.CodigoVisita);

            if (dtSol != null && dtSol.Rows.Count > 0)
            {
                // Comprobamos que la visita no esté en estado visita erronea
                if (intEstadoVisitaActual == (int)Enumerados.EstadosVisita.visitaErronea)
                {
                    response.AddError(CierreVisitaWSError.ErrorEstadoActualVisitaErroneaPorAnomaliaCritica);
                    return false;
                }
            }


            // Si llegamos a este punto hemos superado todas las validaciones y retornamos true.
            return true;
        }        

        protected override void GuardarTrazaLlamada(WSRequest request)
        {
            CierreVisitaRequest cierreVisita = (CierreVisitaRequest)request;
            //MandarMail(cierreVisita == null?"Sí Null": "No null", "WS PROO_1", "maquintela@gfi.es", true);
            // Generamos el fichero xnl, y guardamos la ruta del mismo en la BB.DD.
            string xml = ToXML(cierreVisita, cierreVisita.Proveedor);
            //MandarMail("ACCESO", "WS PROO_2", "maquintela@gfi.es", true);
            // Guardar la traza y dejar el id de la traza guardada.
            TrazaDB trazaDB = new TrazaDB();
            //MandarMail("ACCESO", "WS PROO_3", "maquintela@gfi.es", true);
            this._idTraza = trazaDB.InsertarYObteneridTraza("DATOS RECIBIDOS", xml, cierreVisita);
            //MandarMail("ACCESO", "WS PROO_4", "maquintela@gfi.es", true);

            //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
            //Guardamos el XML completo de la request en bloques de 60000 caracteres.
            if (_idTraza != null && _idTraza > 0)
            {
                string XMLTemp = xml;

                while (XMLTemp.Length > 0)
                {
                    //Cortamos en bloques de 60.000 caracteres
                    Int32 numCorte = XMLTemp.Length < 60000 ? XMLTemp.Length : 60000;

                    string cadena = XMLTemp.Substring(0, numCorte);
                    XMLTemp = XMLTemp.Remove(0, numCorte);

                    Int32 numReg = trazaDB.InsertarTrazaRequestXML(_idTraza, cadena);
                }
            }
            //20201013 BUA END R#23245: Guardar el Ticket de combustión
        }

        public string ToXML(Object oObject, string proveedor)
        {
            //MandarMail("ACCESO", "WS PROO_4444", "maquintela@gfi.es", true);
            XmlDocument xmlDoc = new XmlDocument();
            //MandarMail("ACCESO", "WS PROO_44441", "maquintela@gfi.es", true);
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            //MandarMail("ACCESO", "WS PROO_44442", "maquintela@gfi.es", true);
            using (MemoryStream xmlStream = new MemoryStream())
            {
                //MandarMail("ACCESO", "WS PROO_44443", "maquintela@gfi.es", true);
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                //MandarMail("ACCESO", "WS PROO_44444", "maquintela@gfi.es", true);
                ////String Destino =Server.MapPath(ConfigurationManager.AppSettings.Get("rutaFicheroXMLWS"));
                //String Destino = ConfigurationManager.AppSettings.Get("rutaFicheroXMLWS");
                //// Creamos un fichero xml con los datos enviados por el proveedor.
                ////xmlDoc.Save(Destino + "_" + proveedor + "_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xml");
                //MandarMail("ACCESO", "WS PROO_44445", "maquintela@gfi.es", true);
                String Destino = ConfigurationManager.AppSettings.Get("rutaFicheroXMLWS");
                //MandarMail("ACCESO", "WS PROO_44446", "maquintela@gfi.es", true);
                Destino = Destino + "_" + proveedor + "_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xml";
                //MandarMail("ACCESO", "WS PROO_44447", "maquintela@gfi.es", true);
                // Creamos un fichero xml con los datos enviados por el proveedor.
                //xmlDoc.Save(Destino);
                // GENERAMOS EL FICHERO.
                using (ManejadorFicheros.GetImpersonator())
                {
                    //MandarMail("ACCESO", "WS PROO_44448", "maquintela@gfi.es", true);
                    //xmlDoc.Save(Destino);
                    //File.WriteAllBytes(rutaFichero, cierreVisita.ContenidoFichero);
                }

                // Devolvemos la ruta del fichero generado.
                return xmlDoc.InnerXml; //Destino;// +"_" + proveedor + "_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xml";// xmlDoc.InnerXml;
            }
        }
        protected override void GuardarTrazaResultadoLlamada(WSResponse response)
        {
            CierreVisitaResponse cierreVisitaResponse = (CierreVisitaResponse)response;

            if (this._idTraza.HasValue)
            {
                // Actualizar la traza con el resultado de la operación.
                TrazaDB trazaDB = new TrazaDB();
                if (cierreVisitaResponse.TieneError)
                {
                    // Si tiene errores.
                    trazaDB.ActualizarTraza(this._idTraza.ToString(), cierreVisitaResponse.ListaErrores[0].Descripcion.ToString());
                }
                else if (cierreVisitaResponse.TieneAviso)
                {
                    // Si tiene avisos.
                    trazaDB.ActualizarTraza(this._idTraza.ToString(), cierreVisitaResponse.ListaAvisos[0].Descripcion.ToString());
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

        private bool InformarEquipamiento(CierreVisitaRequest request, CierreVisitaResponse response)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.TipoEquipamiento))
                {
                    //if (request.TipoEquipamiento != "0")
                    //{
                    string Potencia = (string.IsNullOrEmpty(request.PotenciaEquipamiento) ? "0" : request.PotenciaEquipamiento);

                    //int IdEquipamientoActual = ObtenerIdEquipamientoActual(request.CodigoContrato.ToString(), request.PotenciaEquipamiento.ToString(), int.Parse(request.TipoEquipamiento.ToString()));
                    int IdEquipamientoActual = ObtenerIdEquipamientoActual(request.CodigoContrato.ToString(), Potencia, int.Parse(request.TipoEquipamiento.ToString()));

                    if (request.TipoProceso == "1")
                    {
                        if (IdEquipamientoActual > 0)
                        {
                            EliminarEquipamiento(IdEquipamientoActual, request.CodigoContrato);
                        }
                    }
                    else
                    {
                        //string Potencia = request.PotenciaEquipamiento.Replace(".", ",");
                        Potencia = Potencia.Replace(".", ",");
                        ActualizarEquipamientos(IdEquipamientoActual, request.CodigoContrato.ToString(), int.Parse(request.TipoEquipamiento.ToString()), Double.Parse(Potencia));
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                response.AddError(CierreVisitaWSError.ErrorDatosEquipamiento);
            }
            // Si hubiera algún error en la response se le devolverán al usuario.
            if (!response.TieneError)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
        private void GuardarTicketCombustionRequest(CierreVisitaRequest request,
                                                    TicketCombustionDTO requestTicketCombustionDTO
                                                    , CierreVisitaResponse response
                                                    , string usuario)
        {
            DocumentoDTO documentoDTOTicket = null;

            try
            {
                if (requestTicketCombustionDTO != null)
                {
                    //Obtenemos el documento correspondientes al ticket de combustion para guardar su correspondiente id
                    //documentoDTOTicket = Documento.ObtenerPorNombreDocumento(request.CodigoContrato, null, request.CodigoVisita, request.NombreFichero);
                    //if (documentoDTOTicket != null)
                    //    requestTicketCombustionDTO.IdFicheroTicketCombustion = documentoDTOTicket.IdDocumento;

                    TicketCombustion.GuardarTicketCombustion(requestTicketCombustionDTO, usuario);
                }
                else
                {
                    //DATOS VISITA INCORRECTOS
                    response.AddError(CierreVisitaWSError.ErrorGuardadoTicketCombustion);
                }
            }
            catch
            {
                //DATOS VISITA INCORRECTOS
                response.AddError(CierreVisitaWSError.ErrorGuardadoTicketCombustion);
            }
        }
        //20201013 BUA END R#23245: Guardar el Ticket de combustión

        private bool RealizarCierreVisita(CierreVisitaRequest request, CierreVisitaResponse response, string usuario)
        {
            DataTable datosVisita = ObtenerDatosVisitas(request.CodigoContrato, request.CodigoVisita);

            // Comprobamos que la visita Existe

            //Se migra la comprobacion al metodo ValidarDatosEntrada
            //if (datosVisita == null || datosVisita.Rows.Count == 0)
            //{
            //    response.AddError(CierreVisitaWSError.ErrorVisitaNoEncontrada);
            //    return false;
            //}

            //Se migra la comprobacion al metodo ValidarDatosEntrada
            // Comprobamos que la visita que nos viene es la última
            //if (request.CodigoVisita != int.Parse(datosVisita.Rows[0]["COD_VISITA"].ToString()))
            //{
            //    response.AddError(CierreVisitaWSError.ErrorCodigoVisitaNoCoincideUltimaVisitaContrato);
            //    return false;
            //}

            int intEstadoVisitaActual = int.Parse(datosVisita.Rows[0]["COD_ESTADO_VISITA"].ToString());

            //Se migra la comprobacion al metodo ValidarDatosEntrada
            // Comprobamos que la visita no esté ya cerrada
            //if (intEstadoVisitaActual == (int)Enumerados.EstadosVisita.Cerrada)
            //{
            //    response.AddError(CierreVisitaWSError.ErrorCodigoVisitaYaCerrada);
            //    return false;
            //}

            // Antes de realizar más operaciones:
            //Actualizamos el contrato que tenemos con el recuperado de la consulta de la visita
            //ya que por relación cups contrato puede ser otro el contrato con el que tengamos que 
            //operar 
            request.CodigoContrato = (string)datosVisita.Rows[0]["COD_CONTRATO"];

            //Se migra la comprobacion al metodo ValidarDatosEntrada
            // Obtenemos cual será la siguiente fecha de movimiento, 
            // si no hay fecha de movimiento anterior, se trata de un error
            DateTime? fechaUltimoHistorico = null;
            ObtenerFechaUltimoVisitaHistorico(request, ref fechaUltimoHistorico);

            //if (!ObtenerFechaUltimoVisitaHistorico(request, ref fechaUltimoHistorico))
            //{
            //    response.AddError(CierreVisitaWSError.ErrorHistoricoVisitaNoEncontrado);
            //    return false;
            //}

            // Comprobamos que el técnico es correcto
            //Se migra la comprobacion al metodo ValidarDatosEntrada
            //DateTime fechaVistia = DateTime.Parse("01/01/1900");
            //if (request.FechaVisita != null) { fechaVistia = (DateTime)request.FechaVisita; }
            //if (!this.ValidarTecnico(request.IdTecnico, int.Parse(request.EstadoVisita), request.Proveedor, fechaVistia))
            //{
            //    response.AddError(CierreVisitaWSError.ErrorTecnicoIncorrecto);
            //    return false;
            //}

            // Comprobamos que el tipo de la visita es correcto
            //Se migra la comprobacion al metodo ValidarDatosEntrada
            //if (!this.ValidarTipoVisita(request.TipoVisita, int.Parse(request.EstadoVisita)))
            //{
            //    response.AddError(CierreVisitaWSError.ErrorTipoVisitaIncorrecto);
            //    return false;
            //}

            //Obtenemos la fecha de límite de visita
            //DateTime fechaLimiteVisitaActual = this.ObtenerFechaVisita(datosVisita);

            // Comprobamos la fecha de realización de la fecha de la visita.
            //Se migra la comprobacion al metodo ValidarDatosEntrada
            //if (!this.ValidarFechaCierreVisita(request.FechaVisita, fechaLimiteVisitaActual, int.Parse(request.EstadoVisita)))
            //{
            //    response.AddError(CierreVisitaWSError.ErrorFechaVisitaSuperaFechaLimite);
            //    return false;
            //}

            // Comprobamos la fecha de realización de la factura.
            //Se migra la comprobacion al metodo ValidarDatosEntrada
            //if (!this.ValidarDatosFacturaCierreVisita(request))
            //{
            //    response.AddError(CierreVisitaWSError.ErrorFaltanDatosObligatoriosCierreVisita);
            //    return false;
            //}

            // Comprobamos la cancelación por segunda vez
            //Se migra la comprobacion al metodo ValidarDatosEntrada
            //if (!this.ValidarDatosFacturaCanceladaDosVeces(request))
            //{
            //    response.AddError(CierreVisitaWSError.ErrorFaltanDatosObligatoriosAusenteSegundaVez);
            //    return false;
            //}

            bool bPermitirReparacion = true;

            // Comprobamos que se permite la reparación en el caso de tenerla.
            if (!this.ValidarReparacionEnVisitaCerrada(request.CodigoContrato, datosVisita.Rows[0]["COD_CONTRATO"].ToString(), request.CodigoVisita, int.Parse(request.EstadoVisita), response))
            {
                //No dejamos meter reparación si han pasado mas de 3 dias desde el cambio a cerrada
                bPermitirReparacion = false;
            }

            if (int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)
            {
                //No dejamos meter reparación si la visita esta en estado pendiente de reparacion.
                // TODO: GGB en este caso no hay traza de error???
                // KINTELL: 11/05/2016 no hay traza, porque no es un error, simplemente una comprobación para no grabar la reparación.
                bPermitirReparacion = false;
            }

            bool bSaltarVisita = false;

            if (!ValidarDatosCerrarVisitaCanceladaPorSistema(request.CodigoContrato, datosVisita.Rows[0]["COD_CONTRATO"].ToString(), request.CodigoVisita, intEstadoVisitaActual, request, response))
            {
                //response.AddError(CierreVisitaWSError.ErrorDatoVisitaCanceladaPorSistema);
                return false;
            }
            

            if (CerrarVisitaCerradaPendienteReparacion(request, response, intEstadoVisitaActual, fechaUltimoHistorico.Value, ref bSaltarVisita, usuario))
            {
                if (intEstadoVisitaActual == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)
                {
                    if (request.NombreFichero != "" && request.NombreFichero != null)
                    {
                        // Si no devuelve error es que ya esta actualizada la visita, por lo que no hay que volver a actualizar.
                        // Comprobar nombre del fichero, estado de la visita, si viene el fichero...
                        //this.GuardarFichero(request, response);
                        //Guardamos el fichero TicketCombustion. En el nombre del fichero va con solo con "", es correcto
                        // COMPROBAMOS QUE LA VISITA SE QUIERA PASAR A CERRADA O CERRADA PEDIENTE REALIZAR REPARACION.
                        if (int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.Cerrada || int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)
                        {
                            DocumentoDTO documentoDTO = this.GuardarFicheroPorNombre(request, response, "");

                            //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
                            //Guardamos el nombre del fichero en la llamada para poder recuperarlo posteriormente 
                            //en el proceso de guardado del ticket de combustion
                            if (documentoDTO != null)
                                request.NombreFichero = documentoDTO.NombreDocumento;
                            //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
                        }
                        else
                        {
                            //MandarMail("ACCESO", "WS PROO_3", "maquintela@gfi.es;eaceves@gfi.es", true);
                            // EN EL CASO DE QUE NOS LLEGUE FICHERO PERO EL ESTADO NO SEA CERRADA O CERRADA PENDIENTE REPARACION, SACAMOS ERROR.
                            if (request.ContenidoFichero != null && request.ContenidoFichero.Length != 0)
                            {
                                response.AddError(CierreVisitaWSError.ErrorEstadoVisitaConFicheroAdjunto);
                            }
                        }

                        bSaltarVisita = true;
                    }
                }
            }

            if (!response.TieneError && !bSaltarVisita)
            {
                try
                {
                    if (request.NombreFichero != "" && request.NombreFichero != null)
                    {
                        // Comprobar nombre del fichero, estado de la visita, si viene el fichero...
                        //this.GuardarFichero(request, response);
                        //Guardamos el fichero TicketCombustion. En el nombre del fichero va con solo con "", es correcto
                        if (int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.Cerrada || int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)
                        {
                            DocumentoDTO documentoDTO = this.GuardarFicheroPorNombre(request, response, "");

                            //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
                            //Guardamos el nombre del fichero en la llamada para poder recuperarlo posteriormente 
                            //en el proceso de guardado del ticket de combustion
                            if (documentoDTO != null)
                                request.NombreFichero = documentoDTO.NombreDocumento;
                            //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
                        }
                        else
                        {
                            //MandarMail("ACCESO", "WS PROO_3", "maquintela@gfi.es;eaceves@gfi.es", true);
                            // EN EL CASO DE QUE NOS LLEGUE FICHERO PERO EL ESTADO NO SEA CERRADA O CERRADA PENDIENTE REPARACION, SACAMOS ERROR.
                            if (request.ContenidoFichero != null && request.ContenidoFichero.Length != 0)
                            {
                                response.AddError(CierreVisitaWSError.ErrorEstadoVisitaConFicheroAdjunto);
                            }
                        }
                    }

                    if (!response.TieneError) { ActualizarDatosVisita(request, usuario, (bool)datosVisita.Rows[0]["ENVIADA_CARTA"], usuario); }
                }
                catch
                {
                    //DATOS VISITA INCORRECTOS
                    response.AddError(CierreVisitaWSError.ErrorDatosVisitaIncorrectos);
                }
            }

            ActualizarTelefonosManteminiento(request, response);

            ActualizarCaldera(request, ref bPermitirReparacion, response);

            if (!string.IsNullOrEmpty(request.FechaPrevistaVisita) && !string.IsNullOrEmpty(request.HoraDesdePrevistaVisita))
            {
                ActualizarFechaPrevistaVisita(response, request.FechaPrevistaVisita.ToString(), request.HoraDesdePrevistaVisita.ToString(), request.HoraHastaPrevistaVisita.ToString(), request.CodigoContrato.ToString(), request.CodigoVisita);
            }

            if (bPermitirReparacion)
            {
                decimal? idReparacionActual = null;
                // Comprobamos que no viene a null el campo.
                if (datosVisita.Rows[0]["ID_REPARACION"].ToString() != "")
                {
                    idReparacionActual = (decimal?)datosVisita.Rows[0]["ID_REPARACION"];
                }
                RealizarReparacion(request, idReparacionActual, response);
            }

            // Si hemos llegado aquí, se ha realizado el proceso completo de cierre de visita.
            // Hubiera algún error en la response se le devolverán al usuario
            if (!response.TieneError)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        private string ObtenerFechaFactura()
        {
            string MES = DateTime.Now.AddMonths(1).Month.ToString();

            // TODO: GGB ver si lo siguiente se puede sustituir por un PadLeft
            //MES.PadLeft(2, '0');
            if (MES.Length == 1)
            {
                MES = "0" + MES;
            }

            String Anhio1 = DateTime.Now.Year.ToString();
            if (MES == "01")
            {
                Anhio1 = DateTime.Now.AddYears(1).Year.ToString();
            }

            return "01/" + MES + "/" + Anhio1;
        }

        private bool ObtenerFechaUltimoVisitaHistorico(CierreVisitaRequest cierreVisita, ref DateTime? fechaUltimoHistorico)
        {
            fechaUltimoHistorico = this.ObtenerFechaUltimoVisitaHistorico(cierreVisita.CodigoContrato, cierreVisita.CodigoVisita);
            return fechaUltimoHistorico.HasValue;
        }

        private DateTime ObtenerFechaVisita(DataTable DatosVisita)
        {
            DateTime fechaLimiteVisitaActual = DateTime.Now;
            if (!string.IsNullOrEmpty((string)DatosVisita.Rows[0]["FEC_LIMITE_VISITA"].ToString()))
            {
                fechaLimiteVisitaActual = DateTime.Parse((string)DatosVisita.Rows[0]["FEC_LIMITE_VISITA"].ToString());
            }

            return fechaLimiteVisitaActual;
        }

        private DataTable ObtenerDatosVisitas(string codContrato, int codVisita)
        {
            using (VisitasWSDB visitasDB = new VisitasWSDB())
            {
                return visitasDB.DatosVisitaParaCierreGet(codContrato, codVisita);
            }
        }

        private int ObtenerIdEquipamientoActual(string codContrato, string potencia, int TipoEquipamiento)
        {
            using (VisitasWSDB visitasDB = new VisitasWSDB())
            {
                return visitasDB.ObtenerIdEquipamientoActual(codContrato, potencia, TipoEquipamiento);
            }
        }

        public static void ActualizarEquipamientos(Decimal IdEquipamientoActual, String Contrato, Decimal IdTipoEquipamiento, Double Potencia)
        {
            using (VisitasWSDB visitasDB = new VisitasWSDB())
            {
                visitasDB.ActualizarEquipamientos(IdEquipamientoActual, Contrato, IdTipoEquipamiento, Potencia);
            }
        }

        public static void EliminarEquipamiento(Decimal IdEquipamientoActual, String Contrato)
        {
            using (VisitasWSDB visitasDB = new VisitasWSDB())
            {
                visitasDB.EliminarEquipamiento(IdEquipamientoActual, Contrato);
            }
        }

        private DateTime? ObtenerFechaUltimoVisitaHistorico(string codContrato, int codVisita)
        {
            //string SQL = "SELECT top (1) fecha FROM relacion_cups_contrato rcc inner join mantenimiento m on m.cod_receptor=rcc.cod_receptor inner join VISITA_HISTORICO vh on m.cod_contrato_sic=vh.cod_contrato WHERE rcc.COD_CONTRATO_sic in ('" + codContrato + "') AND COD_VISITA='" + codVisita + "' ORDER BY FECHA DESC";

            using (VisitasWSDB visitasDB = new VisitasWSDB())
            {
                return visitasDB.ObtenerUltimaFechaMovimientoVisitaHistorico(codContrato, codVisita);
            }
        }

        private bool TecnicoSuperaLimiteVisitasDia(CierreVisitaRequest cierreVisita)
        {
            // Validamos que el proveedor no ha cerrado más de 12 visitas en el día
            // Esa validación sólo la hacemos cuando está informada la fecha de la visita
            // si no está informada es porque se cierra la visita por otro motivo, no porque
            // haya asistido el técnico.
            if (cierreVisita.FechaVisita.HasValue)
            {
                using (VisitasWSDB visitaDB = new VisitasWSDB())
                {
                    int numVisitasTecnico = 0;

                    if (cierreVisita.IdTecnico.HasValue)
                    {
                        numVisitasTecnico = visitaDB.ObtenerNumeroVisitasTecnicoDia(cierreVisita.IdTecnico.Value, cierreVisita.FechaVisita.Value);
                    }
                    else
                    {
                        // TODO: GGB Habría que devolver aquí algún error?
                        // Kintell: No, porque ya tenemos un error controlado de que el técnico sea correcto, en caso de venir en blanco daría error de 
                        // técnico incorrecto.
                    }

                    // TODO: GGB coger el valor 12 de un parámetro de parametrización.
                    ConfiguracionDTO confNumeroVisitasCierreDia = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.NUMERO_VISITAS_CIERRE_DIA);
                    return numVisitasTecnico > int.Parse(confNumeroVisitasCierreDia.Valor);//12;
                }

            }
            else
            {
                return false;
            }
        }

        private bool ValidarTelefonoContacto1(string telefono)
        {
            if (!string.IsNullOrEmpty(telefono))
            {
                //20220218 BGN MOD BEG R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
                PhoneValidator telefVal = new PhoneValidator();
                if (!telefVal.Validate(telefono))
                {
                    return false;
                }
                //20220218 BGN MOD END R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
            }

            return true;
        }

        private bool ValidarCodigoBarrasVisita(int estadoVisita, string codigoBarras)
        {
            // Comprobamos que el estado de la visita sea cerrada
            // Si es cerrada hay que comprobar que el código de barras esté informado

            if (estadoVisita == (int)Enumerados.EstadosVisita.Cerrada)
            {
                // Hacemos la comprobación si está informado
                if (!string.IsNullOrEmpty(codigoBarras))
                {
                    // Comprobamos que la longitud sea 13 o 14 dígitos
                    if (!codigoBarras.Length.Equals(13) && !codigoBarras.Length.Equals(14))
                    {
                     //   return false;
                    }

                    // Comprobamos que todos los dígitos seas numéricos
                    try
                    {
                       // Int64 result = Int64.Parse(codigoBarras);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private bool ValidarCodigoBarrasReparacion(bool tieneReparacion, string codigoBarras)
        {
            // Comprobamos que el la visita tiene reparación

            if (tieneReparacion)
            {
                // Hacemos la comprobación si está informado
                if (!string.IsNullOrEmpty(codigoBarras))
                {
                    // Comprobamos que la longitud sea 13 o 14 dígitos
                    if (!codigoBarras.Length.Equals(13) && !codigoBarras.Length.Equals(14))
                    {
                       // return false;
                    }

                    // Comprobamos que todos los dígitos seas numéricos
                    try
                    {
                       // Int64 result = Int64.Parse(codigoBarras);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private bool ValidarTecnico(int? tecnico, int estadoVisita, string proveedor, DateTime FechaVisita)
        {
            // Validamos que el ténico esté informado cuando el estado sea cerrado
            if (estadoVisita == (int)Enumerados.EstadosVisita.Cerrada)
            {
                if (tecnico.HasValue)
                {
                    // Comprobamos que el técnico existe.
                    if (Visitas.ComprobarTecnicoExistente((int)tecnico, proveedor))
                    {
                        // Comprobamos que no haya cerrado mas de 12 visitas un mismo día.
                        int numRegistros = Visitas.ObtenerContadorVisitasTecnicoPorDia(Int32.Parse(tecnico.ToString()), FechaVisita);
						ConfiguracionDTO confNumeroVisitasCierreDia = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.NUMERO_VISITAS_CIERRE_DIA);
                        //return numVisitasTecnico > int.Parse(confNumeroVisitasCierreDia.Valor);//12;
                        if (numRegistros < int.Parse(confNumeroVisitasCierreDia.Valor))//12;
                        {
                            return true;
                        }
                        else
                        {
                            // MAS DE 12 DIAS.
                            return false;
                        }
                    }
                    else
                    {
                        // TÉCNICO INEXISTENTE EN LA BB.DD.
                        return false;
                    }
                }
                else
                {
                    // SIN TÉCNICO PESE A SER ESTADO "CERRADA".
                    return false;
                }
            }

            return true;
        }

        private bool ValidarTipoVisita(string tipoVisita, int estadoVisita)
        {
            // Validamos que el tipo de la visita esté informado cuando el estado sea cerrado
            if (estadoVisita == (int)Enumerados.EstadosVisita.Cerrada)
            {
                if (tipoVisita == "" || tipoVisita == null)
                {
                    return false;
                }
                else
                {
                    //TODO: GGB validar que el código de la visita es correcto, es decir.
                    //que hay algún código en la tabla..
                    // 10/05/2016 KINTELL: ESTO NO HACE FALTA HACER.
                    if (tipoVisita.ToUpper() == "DEFECTOS MENORES" ||
                        tipoVisita.ToUpper() == "ANOMALIA SECUNDARIA" ||
                        tipoVisita.ToUpper() == "PRECINTADO DE INSTALACION" ||
                        tipoVisita.ToUpper() == "SIN DEFECTOS" ||
                        tipoVisita.ToUpper() == "VISITA SIN ANOMALIA CON VALORES FUERA DE RANGO")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ValidarFechaCierreVisita(DateTime? fechaCierreVisita, DateTime fechaLimiteVisita, int estadoVisita)
        {
            // Validamos que la fecha cierre de la visita esté informado cuando el estado sea cerrado.
            if (estadoVisita == (int)Enumerados.EstadosVisita.Cerrada)
            {
                if (!fechaCierreVisita.HasValue)
                {
                    return false;
                }
                else
                {
                    // Si la fecha está informada validamos que sea inferior a la fecha límite de visita.
                    if (fechaCierreVisita.Value > fechaLimiteVisita)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ValidarDatosFacturaCierreVisita(CierreVisitaRequest request)
        {
            // Validamos que los datos de la factura estén informados si se quiere cerrar la visita
            if (int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.Cerrada
                || int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion
                )
            {
                if (!request.RecepcionComprobante.HasValue
                    || !request.FacturadoProveedor.HasValue
                    || !request.FechaFactura.HasValue
                    || string.IsNullOrEmpty(request.CodigoBarrasVisita)
                    )
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidarDatosFacturaCanceladaDosVeces(CierreVisitaRequest request)
        {
            // Validamos que los datos de la factura estén informados si se quiere cerrar la visita
            if (int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.CanceladaAusentePorSegundaVez)
            {
                if ( //!request.FechaVisita.HasValue
                    //|| 
                    !request.RecepcionComprobante.HasValue
                    || !request.FechaFactura.HasValue
                    || string.IsNullOrEmpty(request.CodigoBarrasVisita)
                    )
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidarReparacionEnVisitaCerrada(string codContrato, string codContratoActual, int idVisita, int estadoVisita, CierreVisitaResponse response)
        {
            DateTime? fechaMovimientoACerrada = Visitas.ObtenerFechaPasoACerrada(codContrato, codContratoActual, idVisita);
            //Si esta en cerrada. No dejamos meter reparacion si han pasado + de 3 dias desde el cambio a cerrada.
            if (estadoVisita == (int)Enumerados.EstadosVisita.Cerrada)
            {
                if (!fechaMovimientoACerrada.HasValue)
                {
                    fechaMovimientoACerrada = DateTime.Now; //return false;
                }
                //else
                //{
                DateTime fechaMovimientoATratar = (DateTime)fechaMovimientoACerrada;
                if (fechaMovimientoATratar.AddDays(3) < DateTime.Now)
                {
                    //response.AddError(CierreVisitaWSError.ErrorReparacionNoPermitidaFechaSuperada);
                    return false;
                }
                //}
            }
            return true;
        }

        private bool CerrarVisitaCerradaPendienteReparacion(CierreVisitaRequest request, CierreVisitaResponse response, int estadoVisitaActual, DateTime fechaUltimoHistorico, ref bool bSaltarVisita, string usuario)
        {
            if (estadoVisitaActual == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)
            {
                if (int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.Cerrada)
                {
                    if (DateTime.Now < fechaUltimoHistorico.AddDays(60))
                    {
                        if (!string.IsNullOrEmpty(request.NumFactura))
                        {
                            // Obtenemos los datos actuales de la visita.
                            Visitas visitas = new Visitas();
                            VisitaDTO visitasDTO = new VisitaDTO();
                            visitasDTO = visitas.DatosVisitasWS(request.CodigoContrato, request.CodigoVisita.ToString(), request.Proveedor);
                            // Cambiamos los datos en el DTO que queremos actualizar.
                            visitasDTO.CodigoEstadoVisita = request.EstadoVisita.ToString();
                            // Actualizamos la visita e insertamos movimiento del historico.
                            Visitas.ActualizarDatosVisitaYVisitaHistoricoWS(visitasDTO, usuario);
                        }
                    }
                    else
                    {
                        response.AddError(CierreVisitaWSError.ErrorSuperaLimiteDiasPendienteRealizarReparacion);
                        return false;
                    }
                }
                else
                {
                    //EN ESTE CASO NO SE PERMITE OTRO ESTADO QUE NO SEA CERRADA (CerradaPendienteRealizarReparacion)
                    response.AddError(CierreVisitaWSError.ErrorEstadoIncorrectoEnCerradaPendienteRealizarReparacion);
                    return false;
                }
            }

            return true;
        }

        private bool ValidarDatosCerrarVisitaCanceladaPorSistema(String codContrato, String codContratoActual, int idVisita, int estadoVisitaActual, CierreVisitaRequest request, CierreVisitaResponse response)
        {
            if (estadoVisitaActual == (int)Enumerados.EstadosVisita.SistemaCanceladaPorSistema)
            {
                DateTime? fechaMovimientoACanceladaPorSistema = Visitas.ObtenerFechaPasoACanceladaPorSistema(codContrato, codContratoActual, idVisita);
                if (int.Parse(request.EstadoVisita) == (int)Enumerados.EstadosVisita.Cerrada)
                {
                    if (fechaMovimientoACanceladaPorSistema.HasValue)
                    {
                        DateTime fechaMovimientoATratar = (DateTime)fechaMovimientoACanceladaPorSistema;
                        if (DateTime.Now >= fechaMovimientoATratar.AddDays(15))
                        {
                            response.AddError(CierreVisitaWSError.ErrorSuperaLimiteDiasCanceladaSistema);
                            return false;
                        }
                    }
                    else
                    {
                        // Falta movimiento en el historico del paso a cancelada por sistema.
                        response.AddError(CierreVisitaWSError.ErrorFaltaMovimientoEnHistoricoDeCanceladaPorSistema);
                        return false;
                    }
                }
                else
                {
                    //EN ESTE CASO NO SE PERMITE OTRO ESTADO QUE NO SEA CERRADA (CerradaPendienteRealizarReparacion)
                    response.AddError(CierreVisitaWSError.ErrorEstadoIncorrectoEnCerradaCanceladaPorSistema);
                    return false;
                }
            }
            return true;
        }

        private void ActualizarDatosVisita(CierreVisitaRequest request, string strUsuario, bool bEnviadaCartaActual, string usuario)
        {
            // Obtenemos los datos actuales de la visita.
            Visitas visitas = new Visitas();
            VisitaDTO visitasDTO = new VisitaDTO();
            visitasDTO = visitas.DatosVisitasWS(request.CodigoContrato, request.CodigoVisita.ToString(), request.Proveedor);
            // Cambiamos los datos en el DTO que queremos actualizar.
            visitasDTO.CodigoContrato = request.CodigoContrato;
            visitasDTO.CodigoVisita = request.CodigoVisita;
            visitasDTO.FechaVisita = request.FechaVisita;
            visitasDTO.CodigoEstadoVisita = request.EstadoVisita.ToString();
            visitasDTO.ObservacionesVisita = request.ObservacionesTecnico;
            if (request.RecepcionComprobante != null)
            {
                visitasDTO.RecepcionComprobante = request.RecepcionComprobante;
            }
            else
            {
                visitasDTO.RecepcionComprobante = false;
            }
            if (request.FacturadoProveedor != null)
            {
                visitasDTO.FacturadoProveedor = request.FacturadoProveedor;
            }
            else
            {
                visitasDTO.FacturadoProveedor = false;
            }
            visitasDTO.NumFactura = request.NumFactura;
            visitasDTO.FechaFactura = request.FechaFactura;
            visitasDTO.CartaEnviada = request.CartaEnviada;
            visitasDTO.FechaEnviadoCarta = request.FechaEnvioCarta;
            visitasDTO.CodigoBarras = request.CodigoBarrasVisita;
            if (request.IdTecnico != null)
            {
                visitasDTO.Tecnico = (short)request.IdTecnico;
            }
            visitasDTO.Observaciones = request.ObservacionesVisita;
            visitasDTO.TipoVisita = request.TipoVisita;
            visitasDTO.ContadorInterno = request.ContadorInterno;
            visitasDTO.CodigoInterno = request.ContadorInterno.ToString();
            // Actualizamos la visita e insertamos movimiento del historico.
            Visitas.ActualizarDatosVisitaYVisitaHistoricoWS(visitasDTO, usuario);

        }

        private void ActualizarTelefonosManteminiento(CierreVisitaRequest request, CierreVisitaResponse response)
        {
            try
            {
                Mantenimiento.ActualizarTelefonosMantenimiento(request.CodigoContrato, request.TelefonoContacto1, request.TelefonoContacto2);
            }
            catch (Exception ex)
            {
                response.AddError(CierreVisitaWSError.ErroralActualizarLosTelefonosDeContacto);
            }
        }
        private void ActualizarFechaPrevistaVisita(CierreVisitaResponse response,string fechaPrevistaVisita,string horadesdePrevistavisita, string horaHastaPrevistaVisita, string codContrato, int codVisita)
        {
            try
            {
                Visitas.ActualizarFechaPrevistaVisitaWS(fechaPrevistaVisita,horadesdePrevistavisita,horaHastaPrevistaVisita,codContrato,codVisita);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en el cierre de visita WS, fecha prevista visita:" + ex.Message, LogHelper.Category.BussinessLogic);
                response.AddError(CierreVisitaWSError.ErrorDatosFechaPrevistaVisitaIncorrectos);
            }
        }
        
        private void ActualizarCaldera(CierreVisitaRequest request, ref bool bPermitirReparacion, CierreVisitaResponse response)
        {
            //bool hayError = false
            try
            {
                if (!request.IdTipoCaldera.HasValue)
                {
                    //Tenemos k borrar la caldera porque viene vacia en el txt, lo que significa o bien que la han quitado o que no la han informado nunca.
                    Calderas.EliminarCaldera(request.CodigoContrato);
                }
                else
                {
                    if (!request.Anio.HasValue)
                    {
                        request.Anio = 0;
                    }

                    CalderaDTO calderasDTO = new CalderaDTO();
                    calderasDTO.CodigoContrato = request.CodigoContrato;
                    calderasDTO.IdTipoCaldera = request.IdTipoCaldera;
                    calderasDTO.IdMarcaCaldera = request.IdMarcaCaldera;
                    calderasDTO.ModeloCaldera = request.ModeloCaldera;
                    calderasDTO.Uso = request.Uso;
                    calderasDTO.Potencia = request.Potencia;
                    calderasDTO.Anio = request.Anio;
                    calderasDTO.DecripcionMarcaCaldera = request.DecripcionMarcaCaldera;

                    Calderas.ActualizarInsertarCaldera(calderasDTO);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en el cierre de visita WS, actualizar caldera:" + ex.Message, LogHelper.Category.BussinessLogic);
                response.AddError(CierreVisitaWSError.ErrorDatosCalderaIncorrectos);
                bPermitirReparacion = false;
            }
        }

        private void RealizarReparacion(CierreVisitaRequest request, decimal? idReparacionActual, CierreVisitaResponse response)
        {
            try
            {
                // CARGAMOS LOS DATOS DE LA REPARACIÓN.
                ReparacionDTO reparacionDTO = new ReparacionDTO();
                reparacionDTO.CodContrato = request.CodigoContrato;
                reparacionDTO.CodVisita = request.CodigoVisita;
                reparacionDTO.Id = idReparacionActual;
                reparacionDTO.IdTipoReparacion = request.IdTipoReparacion;
                reparacionDTO.FechaReparacion = request.FechaReparacion;
                reparacionDTO.IdTipoTiempoManoObra = request.IdTipoTiempoManoObra;
                reparacionDTO.ImporteReparacion = request.CosteMateriales;
                reparacionDTO.ImporteMaterialesAdicional = request.CosteMaterialesCliente;
                reparacionDTO.ImporteManoObraAdicional = request.ImporteManoObra;
                //reparacionDTO.CosteMaterialesCliente = request.CosteMaterialesCliente;
                reparacionDTO.FechaFactura = request.FechaFacturaReparacion;
                reparacionDTO.NumeroFacttura = request.NumeroFacturaReparacion;
                reparacionDTO.CodigoBarrasReparacion = request.CodigoBarrasReparacion;

                // MIRAMOS QUE ACCION TENEMOS QUE PROCESAR.
                if (!request.TieneReparacion.HasValue || !request.TieneReparacion.Value)
                //if (!request.TieneReparacion.HasValue || !(bool)request.TieneReparacion)
                {
                    //Si no viene reparación borramos la existente.
                    Reparacion.BorrarReparacion(reparacionDTO);
                }
                else
                {

                    if (idReparacionActual.HasValue && idReparacionActual.Value != 0)
                    {
                        //Si hay reparación anterior.
                        Reparacion.ActualizarReparacion(reparacionDTO);
                    }
                    else
                    {
                        //Si no hay reparación anterior.
                        Reparacion.InsertarReparacion(reparacionDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                response.AddError(CierreVisitaWSError.ErrorDatosReparacionIncorrectos);
            }
        }
    }

    
}