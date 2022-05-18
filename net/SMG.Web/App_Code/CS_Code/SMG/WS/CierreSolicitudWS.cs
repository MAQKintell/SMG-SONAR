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
using Iberdrola.SMG.DAL;
using System.Linq;


namespace Iberdrola.SMG.WS
{
    public class CierreSolicitudWS : WSBase
    {
        /// <summary>
        /// Cierra la visita con los datos pasados por parámetro
        /// </summary>
        /// <param name="cierreVisita">Objeto de tipo CierreVisitaRequest con los datos 
        /// necesarios para realizar el cierre de la vista</param>
        /// <returns>Objeto de tipo CierreVisitaResponse con el resultado del cierre de la respuesta</returns>

        public Authentication ServiceCredentials;

        private string _SUBTIPO_SOLICITUD = "";
        private string _ESTADO_ACTUAL_SOLICITUD = "";
        private string _PROVEEDOR = "";
        private string _CONTRATO = "";
        private DateTime _FECHA_CREACION;

        public string SUBTIPO_SOLICITUD { get { return _SUBTIPO_SOLICITUD; } set { _SUBTIPO_SOLICITUD = value; } }
        public string ESTADO_ACTUAL_SOLICITUD { get { return _ESTADO_ACTUAL_SOLICITUD; } set { _ESTADO_ACTUAL_SOLICITUD = value; } }
        public string PROVEEDOR { get { return _PROVEEDOR; } set { _PROVEEDOR = value; } }
        public string CONTRATO { get { return _CONTRATO; } set { _CONTRATO = value; } }
        public DateTime FECHA_CREACION { get { return _FECHA_CREACION; } set { _FECHA_CREACION = value; } }

        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public CierreSolicitudResponse CerrarSolicitud(CierreSolicitudRequest cierreSolicitud)
        {
            CierreSolicitudResponse response = new CierreSolicitudResponse();

            //Boolean habilitarWS = false;
            //ConfiguracionDTO confHabilitarWS = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_WS);
            //if (confHabilitarWS != null && !string.IsNullOrEmpty(confHabilitarWS.Valor) && Boolean.Parse(confHabilitarWS.Valor))
            //{
            //    habilitarWS = Boolean.Parse(confHabilitarWS.Valor);
            //}

            Boolean habilitarWS = ValidarwebServiceActivo(Enumerados.Configuracion.HABILITAR_CIERRESOLICITUD_WS.ToString());

            if (!habilitarWS)
            {
                response.AddError(CommonWSError.ErrorWSInactivo);
            }
            else
            {
                try
                {
                    CargarDatosActuales(cierreSolicitud.idSolicitud.ToString(), response);

                    //MandarMail("ACCESO", "WS PROO_10", "maquintela@gfi.es", true);
                    // Primero guardamos la traza de la llamada.
                    this.GuardarTrazaLlamada(cierreSolicitud);

                    if (!response.TieneError)
                    {
                        // Realizamos la validación del usuario.
                        //MandarMail("ACCESO", "WS PROO_11", "maquintela@gfi.es", true);
                        if (this.Context.Request.Headers["Authorization"] != null)
                        {
                            string[] usuario = GetHeaderAuthorization(this.Context.Request.Headers["Authorization"].ToString());
                            //MandarMail(usuario[0].ToString() + "_" + usuario[1].ToString(), "ERROR NO DEFINIDO", "maquintela@gfi.es", true);

                            ServiceCredentials = new Authentication();
                            ServiceCredentials.UserName = usuario[0].ToString();
                            ServiceCredentials.Password = usuario[1].ToString();

                            if (this.ValidarUsuario(usuario[0].ToString(), usuario[1].ToString()))//ServiceCredentials.UserName, ServiceCredentials.Password))
                            {
                                if (this.ValidarCamposObligatorios(cierreSolicitud, response))
                                {
                                    bool isGuardarTicketCombustion = false;

                                    //if (CargarDatosActuales(cierreSolicitud.idSolicitud.ToString(), response))
                                    //{
                                    //MandarMail("ACCESO", "WS PROO_12", "maquintela@gfi.es", true);
                                    // Realizamos la validación de los parámetros de entrada.
                                    if (this.ValidarDatosEntrada(cierreSolicitud, response))
                                    {
                                        CierreSolicitudResponse responseTicket = new CierreSolicitudResponse();
                                        TicketCombustionDTO ticketCombustionDTO = new TicketCombustionDTO();

                                        //Incluimos el codigo contrato pq no vienen en la request y el .
                                        //Inicializamos los siguientes datos.
                                        ticketCombustionDTO.CodigoContrato = CONTRATO;
                                        ticketCombustionDTO.SubTipoSolicitud = int.Parse(SUBTIPO_SOLICITUD);
                                        ticketCombustionDTO.EstadoActualSolicitud = int.Parse(ESTADO_ACTUAL_SOLICITUD);
                                        ticketCombustionDTO.IdSolicitud = cierreSolicitud.idSolicitud;

                                        //Obtenemos datos necesarios de mantenimiento
                                        ticketCombustionDTO = ObtenerDatosMantenimiento(ticketCombustionDTO);

                                        //20201021 BUA ADD R#23245: Guardar el Ticket de combustión
                                        if (!response.TieneError && HayQueProcesarTicketCombustion(ticketCombustionDTO, cierreSolicitud, response))
                                        {
                                            ConfiguracionDTO confValidacionesActiva = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_VALIDACION_ACTIVA_SOLICITUDES);

                                            //Creamos el objeto ticketCombustion a partir de los datos que vienen del ws
                                            ticketCombustionDTO = TicketCombustion.ObtenerTicketCombustionDTOFromRequest(ticketCombustionDTO, cierreSolicitud, ServiceCredentials.UserName);

                                            //Validamos los datos del ticket de combustion
                                            isGuardarTicketCombustion = PValidacionesTicketCombustion.ValidarDatosEntradaTicketCombustion(ticketCombustionDTO
                                                                                                                                            , responseTicket
                                                                                                                                            , "CierreVisita" //confValidacionesActiva.Valor 
                                                                                                                                            , ServiceCredentials.UserName
                                                                                                                                            , SUBTIPO_SOLICITUD
                                                                                                                                            , string.Empty);

                                            //Si el proceso de las validaciones del ticket de combustion devuelven un error prevalece sobre el actual.
                                            if (!isGuardarTicketCombustion && responseTicket.TieneError)
                                                response = responseTicket;
                                        }

                                        if ((!response.TieneError && !responseTicket.TieneError && !responseTicket.TieneAviso) //Para los casos que no tienen ticket de combustion
                                            || (isGuardarTicketCombustion && !responseTicket.TieneError && !responseTicket.TieneAviso)) //Para los casos que si tiene ticket de combustion pero no tiene ningun error o aviso.
                                        {
                                            this.ModificarSolicitud(cierreSolicitud, response, ServiceCredentials.UserName);
                                            this.ActualizarCaldera(cierreSolicitud, response);
                                            this.GuardarFichero(cierreSolicitud, response, ServiceCredentials.UserName);

                                            //20210208 BUA ADD R#23245: Guardar el Ticket de combustión
                                            //Una vez cerrada la solicitud de tipo VisitaIncorrecta o RevisionPorPrecinte, cerramos la visita o solicitud que se cerro como visita erronea
                                            //if (isGuardarTicketCombustion)
                                            //PValidacionesTicketCombustion.CerrarVisitaSolicitudErronea(ticketCombustionDTO, response, ServiceCredentials.UserName, cierreSolicitud.EstadoSolicitud);
                                            //Comprobamos si el estado de la solicitud es un estado final de los parametrizados que incorporan el ticket de combustion, miramos si tiene una visita asociada en estado 13 para cerrarla.
                                            PValidacionesTicketCombustion.CerrarVisitaEnEstadoErroneaCuandoSolicitudResuelta(decimal.Parse(ticketCombustionDTO.IdSolicitud.ToString()), ticketCombustionDTO.SubTipoSolicitud.ToString(), cierreSolicitud.EstadoSolicitud, ServiceCredentials.UserName);
                                            //END 20210208 BUA ADD R#23245: Guardar el Ticket de combustión
                                        }

                                        //Guardamos los datos relacionados con el ticket de combustion
                                        if (!response.TieneError && isGuardarTicketCombustion)
                                        {
                                            //Guardamos los datos relacionados con el ticket de combustion
                                            this.GuardarTicketCombustionRequest(cierreSolicitud, ticketCombustionDTO, response, ServiceCredentials.UserName);

                                            //Si el proceso de las validaciones del ticket de combustion devuelven un error prevalece sobre el actual.
                                            if (responseTicket.TieneError || responseTicket.TieneAviso)
                                                response = responseTicket;

                                            //Enviamos el correo con el informe de revision de mantenimiento.
                                            //if (!response.TieneError)
                                            //    PValidacionesTicketCombustion.EnvioCorreoInformeRevisionMantenimiento(ticketCombustionDTO);
                                        }
                                        //END 20201021 BUA ADD R#23245: Guardar el Ticket de combustión

                                        //20210120 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                                        if (!response.TieneError)
                                        {
                                            //Comprobamos que si se trata de una solicitud de GC y en estado 'Oferta Presentada Pdte. Firma' (080) 
                                            //si tenemos informadas las características de Email, tenemos que generar el contrato de GC y mandarlo a EDatalia
                                            GeneracionContratoGC_EnvioEdatalia(cierreSolicitud);
                                        }
                                        //20210423 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                                    }
                                    //}
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
                                //response.AddError(CommonWSError.ErrorCredencialesUsuario);
                                // UtilidadesMail.EnviarMensajeError(" ERROR ID TRAZA : " + usuario[0].ToString(), ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");
                            }
                        }
                        else
                        {
                            response.AddError(CommonWSError.ErrorCredencialesUsuario);
                            //response.AddRespuestaCustom("ERROR CABECERA");
                        }
                    }
                    //string user = ServiceCredentials.UserName;

                    // Guardamos el resultado en la traza de la llamada.
                    this.GuardarTrazaResultadoLlamada(response);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("Error en la llamada al CierreVisitaWS.CerrarVisita", LogHelper.Category.BussinessLogic, ex);
                    // TODO: GGB  ver si enviamos un email. en caso de ponerlo parametrizarlo con una variable de 
                    // configuración que indique si está activo el email de errores de WS.
                    //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                    //MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_", "ERROR NO DEFINIDO", "maquintela@gfi.es", true);
                    UtilidadesMail.EnviarMensajeError(" ERROR NO DEFINIDO", ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");
                    //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
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

        private TicketCombustionDTO ObtenerDatosMantenimiento(TicketCombustionDTO requesTicketCombustiontDTO)
        {
            //Obtenemos el CUPS del mantenimiento
            IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(requesTicketCombustiontDTO.CodigoContrato);

            if (datosMantenimiento != null)
            {
                while (datosMantenimiento.Read())
                {

                    if (string.IsNullOrEmpty(requesTicketCombustiontDTO.Proveedor))
                        requesTicketCombustiontDTO.Proveedor = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "proveedor_averia");

                    //20220218 BGN MOD BEG R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
                    if (string.IsNullOrEmpty(requesTicketCombustiontDTO.TelefonoContacto1))
                        requesTicketCombustiontDTO.TelefonoContacto1 = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NUM_TEL_CLIENTE");
                    //20220218 BGN MOD END R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente

                    if (string.IsNullOrEmpty(requesTicketCombustiontDTO.PersonaContacto))
                        requesTicketCombustiontDTO.PersonaContacto = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_TITULAR");
                }
            }

            return requesTicketCombustiontDTO;
        }

        private Boolean HayQueProcesarTicketCombustion(TicketCombustionDTO ticketCombustionDTO
                                                        , CierreSolicitudRequest cierreSolicitud
                                                        , WSResponse response)
        {
            bool tickeCombustionProc = false;
            bool existSubTipoSol = false;
            bool existEstadoSol = false;
            bool existClaveExcepcion = false;
            bool existProveedor = false;
            bool activoProcTicketCombustion = false;

            //if (!string.IsNullOrEmpty(cierreSolicitud.TicketCombustion))
            //{
            //    if (cierreSolicitud.TicketCombustion.Trim() == "S")
            //    {
            //        if (int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.VisitaIncorrecta
            //            || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.AveriaIncorrecta
            //            || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.AveriaMantenimientodeGas
            //            || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.AveriaGasConfort
            //            || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.GasConfort
            //            || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.VisitaSupervision
            //            || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.AveriaSupervision
            //            || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.RevisionPorPrecinte
            //            || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.SolicitudDeVisitaSMG)
            //        {
            //            if (int.Parse(cierreSolicitud.EstadoSolicitud.ToString()) == (int)Enumerados.EstadosSolicitudes.VisitaRealizada
            //                || int.Parse(cierreSolicitud.EstadoSolicitud.ToString()) == (int)Enumerados.EstadosSolicitudes.Reparada
            //                || int.Parse(cierreSolicitud.EstadoSolicitud.ToString()) == (int)Enumerados.EstadosSolicitudes.ReparadaconDocumentacion
            //                || int.Parse(cierreSolicitud.EstadoSolicitud.ToString()) == (int)Enumerados.EstadosSolicitudes.Reparadaportelefono
            //                || int.Parse(cierreSolicitud.EstadoSolicitud.ToString()) == (int)Enumerados.EstadosSolicitudes.InstalaciónRealizada)
            //            {
            //                result = true;
            //            }
            //        }
            //    }
            //}

            //ConfiguracionDTO confSubtipoSolicitud = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_SUB_TIPO_SOLICITUDES_A_TRATAR);
            //ConfiguracionDTO confEstadosSol = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_ESTADOS_SOLICITUDES_A_TRATAR);
            ConfiguracionDTO confExcepcionProcTicket = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_SUB_TIPO_SOL_CLAVE_EXEPCION);
            ConfiguracionDTO confProveedores = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_PROVEEDORES_A_TRATAR);
            ConfiguracionDTO confProcesarValidaciones = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_ACTIVAR_VALIDACIONES_SOLICITUDES);

            activoProcTicketCombustion = bool.Parse(confProcesarValidaciones.Valor);


            //string[] arrSubtipoSolicitud = confSubtipoSolicitud.Valor.Split(';');
            //string[] arrEstadosSol = confEstadosSol.Valor.Split(';');
            string[] arrSubTipoSolClaveExcepcion = confExcepcionProcTicket.Valor.Split(';');
            string[] arrProveedoresTratar = confProveedores.Valor.Split(';');

            //Aqui comprobamos si la clave de excepcion existe en el campo comentarios del ticket de combustion.
            foreach (string subTipoSolClaveExcepcion in arrSubTipoSolClaveExcepcion)
            {
                string subTipoSol = subTipoSolClaveExcepcion.Split(':')[0];
                string claveExepcion = subTipoSolClaveExcepcion.Split(':')[1];

                if (Enum.IsDefined(typeof(Enumerados.SubtipoSolicitud), subTipoSol))
                {
                    Enumerados.SubtipoSolicitud enumSubTipoSol = (Enumerados.SubtipoSolicitud)Enum.Parse(typeof(Enumerados.SubtipoSolicitud), subTipoSol);

                    if (!string.IsNullOrEmpty(cierreSolicitud.Comentarios))
                    {
                        if ((!string.IsNullOrEmpty(enumSubTipoSol.ToString()) && int.Parse(SUBTIPO_SOLICITUD) == (int)enumSubTipoSol)
                                && !string.IsNullOrEmpty(claveExepcion) && cierreSolicitud.Comentarios.Contains(claveExepcion))
                            existClaveExcepcion = true;
                    }
                }
            }

            //Miramos que el proveedor que viene en la request sea uno de los proveedores parametrizados en el p_config
            foreach (string proveedor in arrProveedoresTratar)
            {
                if (proveedor.ToUpper() == ticketCombustionDTO.Proveedor.ToUpper())
                    existProveedor = true;
            }


            //Miramos que el sub tipo solicitud que viene en la request sea unos de sub tipo solilicitu parametrizado en el p_config
            existSubTipoSol = PValidacionesTicketCombustion.ComprobarSiSeDebeProcesarSubTipoSolicitud(SUBTIPO_SOLICITUD);

            //Miramos que el estado de la visita que viene en la request sea uno de los estados parametrizado en el p_config
            existEstadoSol = PValidacionesTicketCombustion.ComprobarSiExisteEstadoSolicitud(cierreSolicitud.EstadoSolicitud.ToString());



            //Miramos que el sub tipo solicitud que viene en la request sea unos de sub tipo solilicitu parametrizado en el p_config
            //foreach (string subTipoSol in arrSubtipoSolicitud)
            //{
            //    if (Enum.IsDefined(typeof(Enumerados.SubtipoSolicitud), subTipoSol))
            //    {
            //        Enumerados.SubtipoSolicitud enumSubTipoSol = (Enumerados.SubtipoSolicitud)Enum.Parse(typeof(Enumerados.SubtipoSolicitud), subTipoSol);

            //        if (!string.IsNullOrEmpty(enumSubTipoSol.ToString()) && int.Parse(SUBTIPO_SOLICITUD) == (int)enumSubTipoSol)
            //            existSubTipoSol = true;

            //    }
            //}

            //Miramos que el estado de solicitud que viene en la request sea uno de los estados parametrizado en el p_config
            //foreach (string estadoSol in arrEstadosSol)
            //{
            //    if (Enum.IsDefined(typeof(Enumerados.EstadosSolicitudes), estadoSol))
            //    {
            //        Enumerados.EstadosSolicitudes enumEstadosSol = (Enumerados.EstadosSolicitudes)Enum.Parse(typeof(Enumerados.EstadosSolicitudes), estadoSol);

            //        if (!string.IsNullOrEmpty(enumEstadosSol.ToString()) && int.Parse(cierreSolicitud.EstadoSolicitud.ToString()) == (int)enumEstadosSol)
            //            existEstadoSol = true;
            //    }
            //}

            if (!activoProcTicketCombustion || (activoProcTicketCombustion && existProveedor && existSubTipoSol && existEstadoSol))
            {
                if (!string.IsNullOrEmpty(cierreSolicitud.TicketCombustion) && cierreSolicitud.TicketCombustion.Trim() == "S")
                {
                    tickeCombustionProc = true;

                    //Aunque le corresponda procesar ticket de combustion, si en el campo Comentario viene una clave en concreta(Especificada en p_config) no se procesa el ticket de combustion.
                    if (existClaveExcepcion)
                        tickeCombustionProc = false;
                }
                else
                    if (activoProcTicketCombustion)
                    response.AddError(TicketCombustionError.TicketCombustionObligatorioEnEsteEstado);
            }

            return tickeCombustionProc;
        }

        private Boolean ComprobarNombreFichero(String nombre)
        {
            //14/03/2018 Nomenclatura documentos
            //CodContrato_AAAAMMDD_proveedor_TipoDocumento.extension
            //CCCCCCCCCC_AAAAMMDD_PRO_DOC.PDF/TIF
            string[] sNombreFichero = nombre.Split('_');

            if (!String.IsNullOrEmpty(nombre))
            {
                // Kintell 21/05/2019 Exigir fichero en GC, con nomenclatura de: CCBB_contrato.pdf.
                // Petición redmine #17248.
                if (int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.GasConfort
                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.RevisionPorPrecinte
                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.InstalacionTermostatoInteligente)
                {
                    // COMPROBAMOS QUE TENGA 2 PARAMETROS EN EL NOMBRE.
                    if (sNombreFichero.Length == 2)
                    {
                        string codContratoSinExtension = sNombreFichero[1].ToString();
                        int posicionPunto = codContratoSinExtension.IndexOf(".");
                        codContratoSinExtension = codContratoSinExtension.Substring(0, posicionPunto);

                        // COMPROBAMOS QUE EL PARAMETRO DEL CONTRATO TENGA 10 CARACTERES.
                        if (codContratoSinExtension.Length == 10)
                        {
                            //20201008 R#27307: Comprobar que el prametro del CCBB no sea CCBB
                            string ccbb = sNombreFichero[0].ToString();
                            if (ccbb.ToUpper().Equals("CCBB"))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
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
                    // COMPROBAMOS QUE TENGA 4 PARAMETROS EN EL NOMBRE.
                    if (sNombreFichero.Length == 4)
                    {
                        // COMPROBAMOS QUE EL PARAMETRO DEL CONTRATO TENGA 10 CARACTERES.
                        if (sNombreFichero[0].Length == 10)
                        {
                            // COMPROBAMOS QUE EL PARAMTERO DEL TIPO DEL DOCUMENTO SEA ALGUNO DE LOS ACEPTADOS.
                            string TipoDocumentoSinExtension = sNombreFichero[3].ToString();
                            int posicionPunto = TipoDocumentoSinExtension.IndexOf(".");
                            TipoDocumentoSinExtension = TipoDocumentoSinExtension.Substring(0, posicionPunto);
                            int idTipoDocumento = Documento.ObtenerIdTipoDocumento(TipoDocumentoSinExtension);
                            if (idTipoDocumento > 0)
                            {
                                return true;
                            }
                            else
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
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private void GuardarFichero(CierreSolicitudRequest cierreSolicitud, CierreSolicitudResponse response, string usuarioCambio)
        {
            try
            {
                if (int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.SolicitudInspeccionGas && !String.IsNullOrEmpty(cierreSolicitud.NombreFichero))
                {
                    String Destino = (string)Documentos.ObtenerRutaFicheros();
                    string rutaFichero = (string)Documentos.ObtenerRutaFicherosTemporal() + cierreSolicitud.NombreFichero.Trim();
                    string rutaFicheroFactura = (string)Documentos.ObtenerRutaFicherosTemporal() + cierreSolicitud.NombreFicheroFactura.Trim();
                    SolicitudesDB objSolicitudesDB1 = new SolicitudesDB();

                    // COPIAMOS EL FICHERO.
                    try
                    {
                        string rutaDestinoFichero = (string)Documentos.ObtenerRutaFicherosInspeccion() + cierreSolicitud.NombreFichero.Trim();
                        using (ManejadorFicheros.GetImpersonator())
                        {
                            // GENERAMOS EL FICHERO.
                            byte[] base64Convert = new byte[cierreSolicitud.ContenidoFichero.Length];
                            base64Convert = cierreSolicitud.ContenidoFichero;

                            FileStream streamWriter = new FileStream(rutaDestinoFichero, FileMode.Create);
                            streamWriter.Write(base64Convert, 0, cierreSolicitud.ContenidoFichero.Length);
                            streamWriter.Close();
                        }

                        // INSERTAMOS LA CARACTERISTICA E HISTORICO...
                        // 165
                        objSolicitudesDB1.GuardarCaracteristicaWS("165", cierreSolicitud.NombreFichero.ToString(), cierreSolicitud.idSolicitud);
                        //Insertar documento tipo C111
                        DocumentoDTO documentoDto = new DocumentoDTO();
                        documentoDto.CodContrato = CONTRATO;
                        documentoDto.IdSolicitud = cierreSolicitud.idSolicitud;
                        documentoDto.NombreDocumento = cierreSolicitud.NombreFichero.Trim();
                        documentoDto.IdTipoDocumento = 111;
                        documentoDto.EnviarADelta = true;
                        documentoDto = Documento.Insertar(documentoDto, usuarioCambio);

                        if (!string.IsNullOrEmpty(cierreSolicitud.NombreFicheroFactura))
                        {
                            String rutaDestinoFactura = (string)Documentos.ObtenerRutaFicherosInspeccion() + cierreSolicitud.NombreFicheroFactura.Trim();
                            using (ManejadorFicheros.GetImpersonator())
                            {
                                // GENERAMOS EL FICHERO FACTURA.
                                byte[] base64ConvertFactura = new byte[cierreSolicitud.ContenidoFicheroFactura.Length];
                                base64ConvertFactura = cierreSolicitud.ContenidoFicheroFactura;

                                FileStream streamWriterFactura = new FileStream(rutaDestinoFactura, FileMode.Create);
                                streamWriterFactura.Write(base64ConvertFactura, 0, cierreSolicitud.ContenidoFicheroFactura.Length);
                                streamWriterFactura.Close();
                            }
                            // INSERTAMOS LA CARACTERISTICA E HISTORICO...
                            // 166
                            objSolicitudesDB1.GuardarCaracteristicaWS("166", cierreSolicitud.NombreFicheroFactura.ToString(), cierreSolicitud.idSolicitud);
                            //Insertar documento tipo C112
                            DocumentoDTO documentoDtoFact = new DocumentoDTO();
                            documentoDtoFact.CodContrato = CONTRATO;
                            documentoDtoFact.IdSolicitud = cierreSolicitud.idSolicitud;
                            documentoDtoFact.NombreDocumento = cierreSolicitud.NombreFicheroFactura.Trim();
                            documentoDtoFact.IdTipoDocumento = 112;
                            documentoDtoFact.EnviarADelta = true;
                            documentoDtoFact = Documento.Insertar(documentoDtoFact, usuarioCambio);
                        }
                    }
                    catch (Exception ex)
                    {
                        //MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_2000_" + rutaFichero + "_" + Destino + "_", "ERROR SUBIDA FICHERO", "maquintela@gfi.es;eaceves@gfi.es", true);
                        response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorInsertFicheroAdjunto);
                    }
                }
                else
                {

                    if (!String.IsNullOrEmpty(cierreSolicitud.NombreFichero)
                        && int.Parse(SUBTIPO_SOLICITUD) != (int)Enumerados.SubtipoSolicitud.GasConfort
                        && int.Parse(SUBTIPO_SOLICITUD) != (int)Enumerados.SubtipoSolicitud.InstalacionTermostatoInteligente
                        && int.Parse(SUBTIPO_SOLICITUD) != (int)Enumerados.SubtipoSolicitud.RevisionPorPrecinte)
                    {

                        string[] sNombreFichero = cierreSolicitud.NombreFichero.Trim().Split('_');
                        string contrato = sNombreFichero[0].ToString();
                        string TipoDocumentoSinExtension = sNombreFichero[3].ToString();
                        int posicionPunto = TipoDocumentoSinExtension.IndexOf(".");
                        TipoDocumentoSinExtension = TipoDocumentoSinExtension.Substring(0, posicionPunto);
                        int idTipoDocumento = Documento.ObtenerIdTipoDocumento(TipoDocumentoSinExtension);

                        String rutaDestino = (string)Documentos.ObtenerRutaFicheros();
                        
                        // Nomenclatura Contrato+Fecha+Proveedor+TipoDoucmento.ext
                        string nombreficheroPRO = contrato.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + PROVEEDOR.Substring(0, 3) + "_" + TipoDocumentoSinExtension;
                        // Añadimos la extension.
                        if (cierreSolicitud.NombreFichero.IndexOf(".pdf") >= 0)
                        {
                            nombreficheroPRO = nombreficheroPRO + ".pdf";
                        }
                        else
                        {
                            nombreficheroPRO = nombreficheroPRO + ".tiff";
                        }
                        rutaDestino = rutaDestino + nombreficheroPRO.Trim();

                        // COPIAMOS EL FICHERO.
                        try
                        {
                            using (ManejadorFicheros.GetImpersonator())
                            {
                                // GENERAMOS EL FICHERO.
                                byte[] base64Convert = new byte[cierreSolicitud.ContenidoFichero.Length];
                                base64Convert = cierreSolicitud.ContenidoFichero;

                                FileStream streamWriter = new FileStream(rutaDestino, FileMode.Create);
                                streamWriter.Write(base64Convert, 0, cierreSolicitud.ContenidoFichero.Length);
                                streamWriter.Close();
                            }

                            // INSERTAMOS REGISTRO EN LA BB.DD.
                            //20210119 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                            //Documentos.InsertaDocumentoCargado(contrato, null, cierreSolicitud.idSolicitud, nombreficheroPRO.Trim(), idTipoDocumento, null, null, Login);
                            DocumentoDTO documentoDto = new DocumentoDTO();
                            documentoDto.CodContrato = CONTRATO;
                            documentoDto.IdSolicitud = cierreSolicitud.idSolicitud;
                            documentoDto.NombreDocumento = nombreficheroPRO.Trim();
                            documentoDto.IdTipoDocumento = idTipoDocumento;
                            documentoDto.EnviarADelta = true;
                            documentoDto = Documento.Insertar(documentoDto, usuarioCambio);
                            //20210119 MOD ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital                            

                            //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
                            cierreSolicitud.NombreFichero = nombreficheroPRO.Trim();
                            //20201013 BUA END R#23245: Guardar el Ticket de combustión

                            // SI VIENE CON DOCUMENTACION Y EL ESTADO A REPARADA AUTOMÁTICAMENTE LO PASAMOS A REPARADA CON DOCUMENTACION.
                            if (int.Parse(cierreSolicitud.EstadoSolicitud.ToString()) == (int)Enumerados.EstadosSolicitudes.Reparada)
                            {
                                if (int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.AveriaMantenimientodeGas
                                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.SolicitudDeVisitaSMG
                                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.AveriaIncorrecta
                                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.RevisionPorPrecinte
                                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.AveriaGasConfort
                                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.AveriaIncorrectaGasConfort
                                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.SolicitudDeVisitaGasConfort)
                                {
                                    cierreSolicitud.EstadoSolicitud = StringEnum.GetStringValue(Enumerados.EstadosSolicitudes.ReparadaconDocumentacion);
                                    this.ModificarSolicitud(cierreSolicitud, response, usuarioCambio);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_2000_" + rutaFichero + "_" + Destino + "_", "ERROR SUBIDA FICHERO", "maquintela@gfi.es;eaceves@gfi.es", true);
                            response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorInsertFicheroAdjunto);
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(cierreSolicitud.NombreFichero)
                            &&
                                (
                                    int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.GasConfort
                                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.RevisionPorPrecinte
                                    || int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.InstalacionTermostatoInteligente
                                )
                            )
                        {
                            // Kintell 23/05/2019.- Guardar contrato GC.R#17248.
                            // Nomenclatura CCBB_CONTRATO
                            String rutaDestino = (string)Documentos.ObtenerRutaFicheros();
                            string nombreficheroPRO = cierreSolicitud.NombreFichero;
                            rutaDestino = rutaDestino + nombreficheroPRO.Trim();

                            // COPIAMOS EL FICHERO.
                            try
                            {
                                using (ManejadorFicheros.GetImpersonator())
                                {
                                    // GENERAMOS EL FICHERO.
                                    byte[] base64Convert = new byte[cierreSolicitud.ContenidoFichero.Length];
                                    base64Convert = cierreSolicitud.ContenidoFichero;

                                    FileStream streamWriter = new FileStream(rutaDestino, FileMode.Create);
                                    streamWriter.Write(base64Convert, 0, cierreSolicitud.ContenidoFichero.Length);
                                    streamWriter.Close();
                                }

                                // INSERTAMOS REGISTRO EN LA BB.DD.
                                //20210119 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                                //Documentos.InsertaDocumentoCargado(contrato, null, cierreSolicitud.idSolicitud, nombreficheroPRO.Trim(), 1, null, null, Login);                                
                                //Si es GC comprobamos que no haya ya un fichero en la tabla DOCUMENTO para la solicitud enviado a edatalia sin respuesta, si lo hay modificaremos el registro existente
                                DocumentoDTO docEncontrato = null;
                                if (int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.GasConfort)
                                {
                                    List<DocumentoDTO> lDoc = Documento.ObtenerDocumentosPorIdSolicitud(cierreSolicitud.idSolicitud);
                                    foreach (DocumentoDTO doc in lDoc)
                                    {
                                        if ((doc.EnviarADelta == false) && (doc.FechaEnvioEdatalia != null) && doc.IdEstadoEdatalia == null)
                                        {
                                            docEncontrato = doc;
                                            break;
                                        }
                                    }
                                }
                                if (docEncontrato != null)
                                {
                                    DocumentoDTO documentoDto = docEncontrato;
                                    documentoDto.NombreDocumento = nombreficheroPRO.Trim();
                                    documentoDto.EnviarADelta = true;
                                    Documento.Actualizar(documentoDto, usuarioCambio);
                                }
                                else
                                {
                                    DocumentoDTO documentoDto = new DocumentoDTO();
                                    documentoDto.CodContrato = CONTRATO;
                                    documentoDto.IdSolicitud = cierreSolicitud.idSolicitud;
                                    documentoDto.NombreDocumento = nombreficheroPRO.Trim();
                                    documentoDto.IdTipoDocumento = 1;
                                    documentoDto.EnviarADelta = true;
                                    documentoDto = Documento.Insertar(documentoDto, usuarioCambio);

                                }
                                //20210119 BGN MOD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital 

                                //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
                                cierreSolicitud.NombreFichero = nombreficheroPRO.Trim();
                                //20201013 BUA END R#23245: Guardar el Ticket de combustión
                            }
                            catch (Exception ex)
                            {
                                //MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_2000_" + rutaFichero + "_" + Destino + "_", "ERROR SUBIDA FICHERO", "maquintela@gfi.es;eaceves@gfi.es", true);
                                response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorInsertFicheroAdjunto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //UtilidadesMail.EnviarAviso("ERROR", ex.Message + ex.StackTrace, "tcservicios@iberdrola.es", "miguel-angel.quintela@gfi.world");
                LogHelper.Error("Error en el cierre de solicitud WS, guardar fichero:" + ex.Message, LogHelper.Category.BussinessLogic);
                response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorDatosFichero);
            }
        }

        private void ActualizarCaldera(CierreSolicitudRequest request, CierreSolicitudResponse response)
        {
            try
            {
                CalderasDB objCalderasDB = new CalderasDB();
                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                DataSet ds = new DataSet();
                ds = objCalderasDB.GetCalderasPorContrato(CONTRATO);
                if (ds != null && ds.Tables[0].Rows.Count == 0 && !string.IsNullOrEmpty(request.ModeloCaldera) && request.IdMarcaCaldera != null)
                {
                    objSolicitudesDB.InsertarCalderaContrato(CONTRATO, int.Parse(request.IdMarcaCaldera.ToString()), request.ModeloCaldera.ToString());
                }
                else
                {
                    if (request.ModeloCaldera == null)
                    {
                        request.ModeloCaldera = "DESCONOCIDO";
                    }
                    if (request.IdMarcaCaldera == 0 || (request.IdMarcaCaldera == null))
                    {
                        request.IdMarcaCaldera = 106;
                    }
                    objSolicitudesDB.ActualizarCalderaContrato(CONTRATO, int.Parse(request.IdMarcaCaldera.ToString()), request.ModeloCaldera.ToString(), "", "", 0, 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en el cierre de solicitud WS, actualizar caldera:" + ex.Message, LogHelper.Category.BussinessLogic);
                response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorDatosCalderaIncorrectos);
            }
        }

        private bool ModificarSolicitud(CierreSolicitudRequest request, CierreSolicitudResponse response, string usuario)
        {
            if (!response.TieneError)
            {
                try
                {
                    SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                    // Si el estado seleccionado es Reparada o Reparada por Telefono
                    if (int.Parse(request.EstadoSolicitud.ToString()) == (int)Enumerados.EstadosSolicitudes.Reparada || int.Parse(request.EstadoSolicitud.ToString()) == (int)Enumerados.EstadosSolicitudes.Reparadaportelefono)
                    {
                        //Actualizamos el estado y restamos 1 al contador de averias, si la averia es en el Equipo.
                        objSolicitudesDB.UpdateEstadoSolicitud(request.idSolicitud.ToString(), request.EstadoSolicitud, request.TipoLugarAveria);
                    }
                    else
                    {
                        //Metemos un valor 3, que hemos definido en la tabla como un N/A
                        objSolicitudesDB.UpdateEstadoSolicitud(request.idSolicitud.ToString(), request.EstadoSolicitud, (int)Enumerados.TipoLugarAveria.NA);
                    }
                    //Si el estado seleccionado es Cancelada
                    if (int.Parse(request.EstadoSolicitud.ToString()) == (int)EnumeradosWS.EstadosSolicitud.Cancelada
                    || int.Parse(request.EstadoSolicitud.ToString()) == (int)EnumeradosWS.EstadosSolicitud.Cancelada25
                    || int.Parse(request.EstadoSolicitud.ToString()) == (int)EnumeradosWS.EstadosSolicitud.Cancelada56)
                    {
                        SolicitudDB objSolicitudDB = new SolicitudDB();
                        objSolicitudDB.ActualizarMotivoCancelacionSolicitud(Int32.Parse(request.idSolicitud.ToString()), request.MotivoCancelacion.ToString().Trim());
                    }

                    Solicitud soli = new Solicitud();
                    string cod_contrato = string.Empty;
                    string sub_tipo = string.Empty;
                    string cod_averia = string.Empty;
                    string cod_visita = "0";
                    string observ_anteriores = string.Empty;
                    string observac_final = string.Empty;
                    DataSet ds = objSolicitudesDB.GetSolicitudesPorIDSolicitud(request.idSolicitud.ToString(), 1);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cod_contrato = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        sub_tipo = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                        cod_averia = ds.Tables[0].Rows[0].ItemArray[14].ToString();
                        observ_anteriores = ds.Tables[0].Rows[0].ItemArray[15].ToString();
                    }
                    if (!String.IsNullOrEmpty(request.ObservacionesSolicitud))
                    {
                        DateTime fechaHora = DateTime.Now;
                        string Horas = fechaHora.Hour.ToString();
                        if (Horas.Length == 1) Horas = "0" + Horas;
                        string Minutos = fechaHora.Minute.ToString();
                        if (Minutos.Length == 1) Minutos = "0" + Minutos;
                        observac_final = "[" + fechaHora.ToString().Substring(0, 10) + "-" + Horas + ":" + Minutos + "] " + usuario + ": " + request.ObservacionesSolicitud;
                        objSolicitudesDB.UpdateObservacionesSolicitud(request.idSolicitud.ToString(), observac_final + (char)(13) + observ_anteriores);
                    }
                    soli.ActualizarHistoricoSolicitud(cod_contrato, request.idSolicitud.ToString(), "002", usuario, request.EstadoSolicitud, observac_final, "", sub_tipo, cod_averia, cod_visita, "M");

                    // DAR DE ALTA LAS CARACTERISTICAS.
                    try
                    {
                        CierreSolicitudRequest cierreSolicitud = (CierreSolicitudRequest)request;

                        String[] TipCar = new String[50];//dsCaracteristicas.Tables[0].Rows.Count];
                        String[] ValCar = new String[50];//dsCaracteristicas.Tables[0].Rows.Count];
                        String[] codigosCaracteristicas = cierreSolicitud.CodigosCaracteristicas.Split(';');
                        String[] valoresCaracteristicas = cierreSolicitud.ValoresCaracteristicas.Split(';');

                        Int64 ContCarac = 0;
                        for (int i = 0; i <= codigosCaracteristicas.Length - 1; i += 1)
                        {
                            String Codigo = codigosCaracteristicas[i].ToString();
                            String Valor = valoresCaracteristicas[i].ToString();

                            TipCar[ContCarac] = Codigo;
                            ValCar[ContCarac] = Valor;

                            ContCarac += 1;
                        }

                        String averiaEn = String.Empty;
                        //Ponemos a activo = false, por si ya existen las características.
                        SolicitudesDB objSolicitudesDB1 = new SolicitudesDB();
                        //Metemos Características.
                        for (int j = 0; j < ContCarac; j++)
                        {
                            //R#19128
                            objSolicitudesDB1.GuardarCaracteristicaWS(TipCar[j], ValCar[j], cierreSolicitud.idSolicitud);
                            //13/01/2020 BGN BEG R#17876 Heredar Nº Serie en Averias GC en Aparato
                            if (TipCar[j].Equals("127"))
                            {
                                averiaEn = ValCar[j];
                            }
                        }
                        //13/01/2020 BGN BEG R#17876 Heredar Nº Serie en Averias GC en Aparato
                        if (sub_tipo == "012" && request.EstadoSolicitud.Equals("010") && averiaEn.Equals("Aparato"))
                        {
                            string numSerie = CaracteristicaHistorico.GetNumSerieGC(cod_contrato);
                            if (!String.IsNullOrEmpty(numSerie))
                            {
                                objSolicitudesDB1.GuardarCaracteristicaWS("169", numSerie, cierreSolicitud.idSolicitud);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorInsertarCaracteristicas);
                        return false;
                    }
                }
                catch
                {
                    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorModificarSolicitud);
                }
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

        protected bool ValidarCamposObligatorios(WSRequest request, WSResponse response)
        {
            CierreSolicitudRequest cierreSolicitud = (CierreSolicitudRequest)request;

            try
            {
                // Validamos que tengamos datos de entrada.
                if (cierreSolicitud == null)
                {
                    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorDatosVacios);
                    return false;
                }

                // Validamos que el contrato venga informado.
                if (DBNull.Value.Equals(cierreSolicitud.idSolicitud))
                {
                    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorFaltanDatosObligatoriosCierreSolicitud);
                    return false;
                }

                // Validamos que el contrato venga informado.
                if (DBNull.Value.Equals(cierreSolicitud.EstadoSolicitud))
                {
                    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorFaltanDatosObligatoriosCierreSolicitud);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorFaltanDatosObligatoriosCierreSolicitud);
                return false;
            }
        }

        protected override bool ValidarDatosEntrada(WSRequest request, WSResponse response)
        {
            CierreSolicitudRequest cierreSolicitud = (CierreSolicitudRequest)request;
            try
            {
                #region validacion estados

                // Comprobamos que la visita no esté en estado visita erronea
                //if (int.Parse(ESTADO_ACTUAL_SOLICITUD) == (int)Enumerados.EstadosSolicitudes.visitaErronea)
                //{
                //    response.AddError(CierreVisitaWSError.ErrorEstadoActualVisitaErroneaPorAnomaliaCritica);
                //    return false;
                //}

                //20180216 BGN Redmine 9324 - Permitir modificar observaciones sin cambio de estado
                if (ESTADO_ACTUAL_SOLICITUD != cierreSolicitud.EstadoSolicitud)
                {
                    // Validamos que el estado solicitud exista en la BB.DD.
                    if (!ValidarEstadoSolicitud(cierreSolicitud.EstadoSolicitud.ToString()))
                    {
                        response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorEstadoSolicitud);
                        return false;
                    }
                }

                // Comprobamos que si el estado nuevo es cancelada, nos llegue el motivo de cancelación.
                if (int.Parse(cierreSolicitud.EstadoSolicitud) == (int)EnumeradosWS.EstadosSolicitud.Cancelada
                    || int.Parse(cierreSolicitud.EstadoSolicitud) == (int)EnumeradosWS.EstadosSolicitud.Cancelada25
                    || int.Parse(cierreSolicitud.EstadoSolicitud) == (int)EnumeradosWS.EstadosSolicitud.Cancelada56)
                {
                    if (DBNull.Value.Equals(cierreSolicitud.MotivoCancelacion))
                    {
                        response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorMotivoCancelacion);
                        return false;
                    }
                }

                /*
				//20180216 BGN Redmine 9324 - Permitir modificar observaciones sin cambio de estado
				// Comprobamos que hay cambio de estado.
				if (ESTADO_ACTUAL_SOLICITUD == cierreSolicitud.EstadoSolicitud)
				{
					response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorSinCambioEstado);
					return false;
				}
				*/

                #endregion validacion estados

                #region validacion caracteristicas
                //// TEMA CARACERISTICAS
                //Tenemos k comprobar también si nos viene las caracteristicas que nos tienen que venir...
                //Obtenemos las características para este estado y el numero de ellas...
                Int64 ContCaracExigibles = 0;
                Boolean Correcto = true;
                CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
                DataSet dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasPorTipo("001", SUBTIPO_SOLICITUD, cierreSolicitud.EstadoSolicitud, 1);

                String[] TipCarExigibles = new String[60];//dsCaracteristicas.Tables[0].Rows.Count];
                String[] TipCar = new String[60];//dsCaracteristicas.Tables[0].Rows.Count];
                String[] ValCar = new String[60];//dsCaracteristicas.Tables[0].Rows.Count];
                for (int i = 0; i < dsCaracteristicas.Tables[0].Rows.Count; i++)
                {
                    //tip_car,descripcion
                    TipCarExigibles[ContCaracExigibles] = dsCaracteristicas.Tables[0].Rows[i].ItemArray[0].ToString();
                    if (dsCaracteristicas.Tables[0].Rows[i]["obligatorio"].ToString() == "True" && dsCaracteristicas.Tables[0].Rows[i]["tipodato"].ToString() != "Fichero")
                    {
                        ContCaracExigibles += 1;
                    }
                }

                // Si es supervisión pedimos también la fecha cita supervisión, que no es obligatoria vía web, pero we need it.
                if (SUBTIPO_SOLICITUD == "006")
                {
                    TipCarExigibles[ContCaracExigibles] = "019";
                    ContCaracExigibles += 1;
                }


                // Cogemos las características que nos mandan y vemos que hay tantos códigos como valores
                String[] codigosCaracteristicas = cierreSolicitud.CodigosCaracteristicas.Split(';');
                String[] valoresCaracteristicas = cierreSolicitud.ValoresCaracteristicas.Split(';');

                if (codigosCaracteristicas.Length != valoresCaracteristicas.Length)
                {
                    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorNumeroCaracteristicasYValoresCarateristicas);
                    return false;
                }

                // Cogemos las caracerísticas que nos mandan.
                //String[] SQLInsertCaracteristicas = new String[70];
                //String[] SQLUpdateCaracteristicas = new String[70];
                Int64 ContCarac = 0;
                for (int i = 0; i <= codigosCaracteristicas.Length - 1; i += 1)
                {
                    //20200909 BEG BGN R#26373: Quitar tabulaciones del codigo de caracteristica
                    String Codigo = codigosCaracteristicas[i].ToString().Trim();
                    //20200909 END BGN R#26373: Quitar tabulaciones del codigo de caracteristica
                    String Valor = valoresCaracteristicas[i].ToString();

                    TipCar[ContCarac] = Codigo;
                    ValCar[ContCarac] = Valor;

                    ContCarac += 1;

                    //String SQLCarac;
                    //SQLCarac = "INSERT INTO caracteristicas_solicitud ";
                    //SQLCarac += "(ID_solicitud";
                    //SQLCarac += ",Tip_car";
                    //SQLCarac += ",Valor";
                    //SQLCarac += ",Fecha_car";
                    //SQLCarac += ",Activo) ";
                    //SQLCarac += "VALUES ";
                    //SQLCarac += "(" + cierreSolicitud.idSolicitud;
                    //SQLCarac += ",'" + Codigo + "'";
                    //SQLCarac += ",'" + Valor + "'";
                    //SQLCarac += ",'" + DateTime.Now.ToString("yyyy/MM/dd") + "'";
                    //SQLCarac += ",1)";
                    ////**********************************************************************
                    //SQLInsertCaracteristicas[ContCarac] = SQLCarac;
                    //SQLUpdateCaracteristicas[ContCarac] = "UPDATE CARACTERISTICAS_SOLICITUD SET ACTIVO=0 WHERE ID_SOLICITUD=" + cierreSolicitud.idSolicitud;// +" AND TIP_CAR='" + Codigo + "'";

                }

                //21/10/2019 R#20024 Financiado BBVA Comparamos el valor de las caracteristicas Financiado BBVA y Modo Pago para que sean coherentes.
                int iCaract = Array.IndexOf(TipCar, "167");
                if (iCaract >= 0)
                {
                    if ((ValCar[iCaract] == "1") || (ValCar[iCaract].ToUpper() == "SI") || (ValCar[iCaract].ToUpper() == "SÍ"))
                    {
                        int iCaractModoPago = Array.IndexOf(TipCar, "084");
                        if (iCaractModoPago >= 0)
                        {
                            if (ValCar[iCaractModoPago] == "Pago Fraccionado")
                            {
                                // Comentamos porque Alex quiere habilitar el fraccionado de momento 23/10/2019.
                                //response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorModoPagoIncorrecto);
                                //return false;
                            }
                        }
                    }
                }
                //TipCar[].IndexOf("163");
                // Kintell R#36525 Begin
                iCaract = 0;
                iCaract = Array.IndexOf(TipCar, "192");
                if (iCaract >= 0)
                {
                    if ((ValCar[iCaract] == "1") || (ValCar[iCaract].ToUpper() == "SI") || (ValCar[iCaract].ToUpper() == "SÍ"))
                    {
                        int iCaractMarcaTermostato = Array.IndexOf(TipCar, "193");
                        if (iCaractMarcaTermostato <= 0)
                        {
                            response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorFaltanCaracteristicasExigibles);
                            Correcto = false;
                            return false;
                        }
                    }
                }
                // END

                //Comprobamos que el número es el mismo.
                if (ContCaracExigibles <= ContCarac)
                {
                    //Comprobamos Que son las que tienen que ser.
                    Boolean Existe;
                    Int16 CorrectoCondicionFechas;
                    for (int j = 0; j < ContCaracExigibles; j++)
                    {
                        Existe = false;
                        CorrectoCondicionFechas = 0;
                        for (int k = 0; k < ContCarac; k++)
                        {
                            if (TipCarExigibles[j].ToString() == TipCar[k].ToString())
                            {
                                //Comprobamos k si es fecha

                                Existe = true;
                                if (TipCar[k].ToString() == "001" || TipCar[k].ToString() == "002" || TipCar[k].ToString() == "003" || TipCar[k].ToString() == "010")
                                {
                                    //comprobar exactamente si es "visita realizada" o "Reparada"
                                    if (cierreSolicitud.EstadoSolicitud != "014" && cierreSolicitud.EstadoSolicitud != "010")
                                    {
                                        DateTime Fecha = DateTime.Parse(ValCar[k].ToString());
                                        if (Fecha.Date >= FECHA_CREACION.Date && Fecha.Date <= FECHA_CREACION.Date.AddDays(90))
                                        {
                                            CorrectoCondicionFechas = 0;
                                        }
                                        else
                                        {
                                            CorrectoCondicionFechas = 1;
                                        }
                                    }
                                    else
                                    {
                                        DateTime Fecha = DateTime.Parse(ValCar[k].ToString());
                                        if (Fecha.Date >= FECHA_CREACION.Date && Fecha.Date <= DateTime.Now.Date)
                                        {
                                            CorrectoCondicionFechas = 0;
                                        }
                                        else
                                        {
                                            CorrectoCondicionFechas = 2;
                                        }
                                    }
                                }

                                //break; //Salimos del for?
                            }
                        }

                        if (Existe == false)
                        {
                            //Falta una característica exigible.
                            //EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";FALTA UNA CARACTERISTICA EXIGIBLE", nombreFicheroLog);
                            response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorFaltanCaracteristicasExigibles);
                            Correcto = false;
                            return false;
                            //break; //Salimos del for?
                        }
                        else
                        {
                            if (CorrectoCondicionFechas != 0)
                            {
                                //Error a la hora de meter la fecha en características.
                                if (CorrectoCondicionFechas == 1)
                                {
                                    //EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";LA FECHA INTRODUCIDA DEBE DE SER MAYOR O IGUAL A LA FECHA DE CREACION Y NO SUPERIOR A 90 DIAS", nombreFicheroLog);
                                    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorLAFECHAINTRODUCIDADEBEDESERMAYOROIGUALALAFECHADECREACIONYNOSUPERIORA90DIAS);
                                }
                                else
                                {
                                    //EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";LA FECHA INTRODUCIDA DEBE DE SER MAYOR O IGUAL A LA FECHA DE CREACION Y NO SUPERIOR AL DIA DE HOY", nombreFicheroLog);
                                    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorLAFECHAINTRODUCIDADEBEDESERMAYOROIGUALALAFECHADECREACIONYNOSUPERIORALDIADEHOY);
                                }
                                Correcto = false;
                                return false;
                            }
                        }
                    }
                    //// DAR DE ALTA LAS CARACTERISTICAS.
                    //try
                    //{
                    //    if (Correcto)
                    //    {
                    //        //Ponemos a activo = false, por si ya existen las características.
                    //        SolicitudesDB objSolicitudesDB1 = new SolicitudesDB();
                    //        // Ejecutamos el update a false, una vez porque no hace falta mas.
                    //        //objSolicitudesDB1.EjecutarSentencia(SQLUpdateCaracteristicas[1].ToString());
                    //        //Metemos Características.
                    //        for (int j = 1; j <= ContCarac; j++)
                    //        {
                    //            //R#19128
                    //            objSolicitudesDB1.GuardarCaracteristicaWS(TipCar[j], ValCar[j], cierreSolicitud.idSolicitud);
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorInsertarCaracteristicas);
                    //    return false;
                    //}
                }
                else
                {
                    //FALTAN O VIENEN DEMASIADAS CARACTERISTICAS.
                    Correcto = false;
                    //EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";FALTA O VIENEN DEMASIADAS CARACTERISTICAS PARA ESTE ESTADO", nombreFicheroLog);
                    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorFALTAOVIENENDEMASIADASCARACTERISTICASPARAESTEESTADO);
                    return false;
                }

                #endregion validacion caracteristicas

                #region validacion documento
                if (ESTADO_ACTUAL_SOLICITUD != cierreSolicitud.EstadoSolicitud)
                {
                    //20210423 BGN MOD BEG R#28313 Cambio Flujo Gas Confort por digitalización contrato, cambiamos InstalaciónRealizada por OfertaAceptada

                    // 27/08/2019 Kintell y Bea. Parametrizar los estados con fichero requerido.
                    Boolean exigirFichero = Documentos.ObtenerFicheroExigible(SUBTIPO_SOLICITUD, cierreSolicitud.EstadoSolicitud.ToString(), ESTADO_ACTUAL_SOLICITUD);
                    if (String.IsNullOrEmpty(cierreSolicitud.NombreFichero) && exigirFichero)
                    {
                        response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorEstadosolicitudConFicheroAdjunto);
                        return false;
                    }

                    if (!String.IsNullOrEmpty(cierreSolicitud.NombreFichero) && !exigirFichero)
                    {
                        response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorEstadosolicitudSinFicheroAdjunto);
                        return false;
                    }

                    if (exigirFichero)
                    {
                        //Comprobar nombre fichero
                        if (String.IsNullOrEmpty(cierreSolicitud.NombreFichero) || !ComprobarNombreFichero(cierreSolicitud.NombreFichero.Trim()))
                        {
                            response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorNombreFicheroAdjunto);
                            return false;
                        }
                        // COMPROBAMOS EL TAMAÑO DEL FICHERO.
                        int maxSize = 2409238;// 1713718;
                        int minSize = 0;
                        if (cierreSolicitud.ContenidoFichero == null || cierreSolicitud.ContenidoFichero.Length > maxSize || cierreSolicitud.ContenidoFichero.Length <= minSize)
                        {
                            response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorLongitudFichero);
                            return false;
                        }

                        // COMPROBAMOS QUE NO EXISTA UN FICHERO CON ESE NOMBRE EN LA BB.DD. salvo por el estado Oferta Aceptada de GC que puede marchacar el de edatalia por el del proveedor
                        if (int.Parse(cierreSolicitud.EstadoSolicitud.ToString()) != (int)Enumerados.EstadosSolicitudes.OfertaAceptada)
                        {
                            //20210310 BGN/Kintell Comprobamos que el fichero no exista ya, para la misma solicitud o visita, porque pueden mandarnos el mismo fichero al cerrar visita y solicitud de averia a la vez
                            //Paco: "cuando hacen la avería si la dejan reparada y el mantenimiento está abierto la app se lo dice y aprovechan para hacerlo"
                            DocumentoDTO docExistente = Documento.ObtenerPorNombreDocumento(CONTRATO, cierreSolicitud.idSolicitud, null, cierreSolicitud.NombreFichero.Trim());
                            if (docExistente != null)
                            {
                                response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorFicheroYaExistente);
                                return false;
                            }
                        }
                    }

                    if (int.Parse(SUBTIPO_SOLICITUD) != (int)Enumerados.SubtipoSolicitud.SolicitudInspeccionGas && !String.IsNullOrEmpty(cierreSolicitud.NombreFicheroFactura))
                    {
                        response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorEstadosolicitudConFicheroAdjunto);
                        return false;
                    }
                    else
                    {
                        if (int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.SolicitudInspeccionGas && !String.IsNullOrEmpty(cierreSolicitud.NombreFicheroFactura) && exigirFichero)
                        {
                            // Comprobar nomenclatura.
                            if (!ComprobarNombreFichero(cierreSolicitud.NombreFicheroFactura.ToString()))
                            {
                                response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorNombreFicheroAdjunto);
                                return false;
                            }
                        }

                        if (int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.SolicitudInspeccionGas && !String.IsNullOrEmpty(cierreSolicitud.NombreFicheroFactura) && !exigirFichero)
                        {
                            response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorEstadosolicitudConFicheroAdjunto);
                            return false;
                        }
                    }
                }

                #endregion validacion documento

                // Si llegamos a este punto hemos superado todas las validaciones y retornamos true.
                return true;
            }
            catch (Exception ex)
            {
                response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorFaltanDatosObligatoriosCierreSolicitud);
                return false;
            }
        }


        //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
        private void GuardarTicketCombustionRequest(CierreSolicitudRequest request,
                                                    TicketCombustionDTO requestTicketCombustionDTO
                                                    , CierreSolicitudResponse response
                                                    , string usuario)
        {
            DocumentoDTO documentoDTOConductoHumos = null;
            DocumentoDTO documentoDTOTicket = null;

            try
            {
                //Creamos el objeto ticketCombustion a partir de los datos que vienen del ws
                //TicketCombustionDTO requestTicketCombustionDTO = TicketCombustion.ObtenerTicketCombustionDTOFromRequest(request, usuario);

                if (requestTicketCombustionDTO != null)
                {
                    //Incluimos el codigo contrato pq no me vienen en la request.
                    //requestTicketCombustionDTO.CodigoContrato = CONTRATO;
                    //requestTicketCombustionDTO.IdSolicitud = request.idSolicitud;

                    //Antes de guardar el ticket de combustion procesamos los ficheros que vienen en la request.
                    documentoDTOConductoHumos = this.GuardarFicheroPorNombre(request, response, "ConductoHumos", usuario);
                    if (documentoDTOConductoHumos != null)
                        requestTicketCombustionDTO.IdFicheroConductoHumos = documentoDTOConductoHumos.IdDocumento;

                    //Obtenemos el documento correspondientes al ticket de combustion para guardar su correspondiente id
                    documentoDTOTicket = Documento.ObtenerPorNombreDocumento(CONTRATO, request.idSolicitud, null, request.NombreFichero);
                    if (documentoDTOTicket != null)
                        requestTicketCombustionDTO.IdFicheroTicketCombustion = documentoDTOTicket.IdDocumento;

                    TicketCombustion.GuardarTicketCombustion(requestTicketCombustionDTO, usuario);
                }
                else
                {
                    //DATOS VISITA INCORRECTOS
                    response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorGuardadoTicketCombustion);
                }
            }
            catch
            {
                //DATOS VISITA INCORRECTOS
                response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorGuardadoTicketCombustion);
            }
        }

        private DocumentoDTO GuardarFicheroPorNombre(CierreSolicitudRequest cierreRequest
                                                , CierreSolicitudResponse response
                                                , string pNombreFichero, string usuario)
        {
            //Recuperamos el nombre del fichero
            string nombreFichero = cierreRequest.GetType().GetProperty("NombreFichero" + pNombreFichero).GetValue(cierreRequest, null).ToString();
            string[] sNombreFichero = nombreFichero.Split('_');
            string TipoDocumentoSinExtension = sNombreFichero[3].ToString();
            int posicionPunto = TipoDocumentoSinExtension.IndexOf(".");
            TipoDocumentoSinExtension = TipoDocumentoSinExtension.Substring(0, posicionPunto);
            int idTipoDocumento = Documento.ObtenerIdTipoDocumento(TipoDocumentoSinExtension);

            string nombreficheroPRO = CONTRATO + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + PROVEEDOR.Substring(0, 3) + "_" + TipoDocumentoSinExtension;
            // Añadimos la extension.
            if (nombreFichero.IndexOf(".pdf") >= 0)
                nombreficheroPRO = nombreficheroPRO + ".pdf";
            else
                nombreficheroPRO = nombreficheroPRO + ".tiff";

            //Comprobamos que no exista un fichero con ese nombre en la BBDD.
            //20210310 BGN/Kintell Comprobamos que el fichero no exista ya para la misma solicitud o visita, porque pueden mandarnos el mismo fichero al cerrar visita y solicitud de averia a la vez
            //Paco: "cuando hacen la avería si la dejan reparada y el mantenimiento está abierto la app se lo dice y aprovechan para hacerlo"
            DocumentoDTO docExistente = Documento.ObtenerPorNombreDocumento(CONTRATO, cierreRequest.idSolicitud, null, nombreficheroPRO);
            if (docExistente != null)
            {
                response.AddError(CierreVisitaWSError.ErrorFicheroYaExistente);
                return null;
            }

            String rutaDestino = (string)Documentos.ObtenerRutaFicheros();            
            rutaDestino = rutaDestino + nombreficheroPRO.Trim();

            //Recuperamos el contenido del fichero
            byte[] tempContenidoFichero = (byte[])cierreRequest.GetType().GetProperty("Fichero" + pNombreFichero).GetValue(cierreRequest, null);

            byte[] contenidoFichero = new byte[tempContenidoFichero.Length];
            contenidoFichero = tempContenidoFichero;

            // COMPROBAMOS EL TAMAÑO DEL FICHERO.
            int minSize = 100;
            int maxSize = 1713718;

            if (!(contenidoFichero.Length >= minSize && contenidoFichero.Length <= maxSize))
            {
                response.AddError(CierreVisitaWSError.ErrorLongitudFichero);
                return null;
            }

            // COPIAMOS EL FICHERO.
            try
            {
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
                else
                {
                    //Insertamos los datos del documento en la tabla documento
                    DocumentoDTO documentoDto = new DocumentoDTO();
                    documentoDto.CodContrato = CONTRATO;
                    documentoDto.IdSolicitud = cierreRequest.idSolicitud;
                    documentoDto.NombreDocumento = nombreficheroPRO;
                    documentoDto.IdTipoDocumento = idTipoDocumento;
                    //20210118 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                    documentoDto.EnviarADelta = true;
                    //20210118 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

                    documentoDto = Documento.Insertar(documentoDto, usuario);

                    return documentoDto;
                }
            }
            catch (Exception ex)
            {
                response.AddError(CierreVisitaWSError.ErrorInsertFicheroAdjunto);

                return null;
            }
        }
        //20201021 BUA END R#23245: Guardar el Ticket de combustión

        protected bool CargarDatosActuales(string idSolicitud, WSResponse response)
        {
            try
            {
                DataSet dsDatosSolicitud = null;
                SolicitudesDB solDB = new SolicitudesDB();
                dsDatosSolicitud = solDB.GetSolicitudesPorIDSolicitud(idSolicitud, 1);

                for (int i = 0; i < dsDatosSolicitud.Tables[0].Rows.Count; i++)
                {
                    SUBTIPO_SOLICITUD = dsDatosSolicitud.Tables[0].Rows[i].ItemArray[4].ToString().Trim();
                    ESTADO_ACTUAL_SOLICITUD = dsDatosSolicitud.Tables[0].Rows[i].ItemArray[8].ToString().Trim();
                    PROVEEDOR = dsDatosSolicitud.Tables[0].Rows[i].ItemArray[16].ToString().Trim();
                    CONTRATO = dsDatosSolicitud.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                    FECHA_CREACION = DateTime.Parse(dsDatosSolicitud.Tables[0].Rows[i].ItemArray[6].ToString());
                }

                return true;
            }
            catch (Exception ex)
            {
                response.AddErrorCierreSolicitud(CierreSolicitudWSError.ErrorObtenerDatosActuales);
                return false;
            }
        }

        protected bool ValidarEstadoSolicitud(string estadoSolicitud)
        {
            EstadosSolicitudDB objEstadoSolicitudesDB = new EstadosSolicitudDB();
            DataSet dtsEstadosFuturos = objEstadoSolicitudesDB.GetEstadosSolicitudFuturos("001", SUBTIPO_SOLICITUD, ESTADO_ACTUAL_SOLICITUD, 3, 1);
            for (int i = 0; i < dtsEstadosFuturos.Tables[0].Rows.Count; i++)
            {
                if (dtsEstadosFuturos.Tables[0].Rows[i].ItemArray[0].ToString().Trim() == estadoSolicitud)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool ValidarTipoAveria(string tipoAveria)
        {
            TiposAveriaDB objTiposAveriaDB = new TiposAveriaDB();
            // Buscamos los tipos de averias.
            DataSet dtsAveria = objTiposAveriaDB.GetTiposAveria(1);
            for (int i = 0; i < dtsAveria.Tables[0].Rows.Count; i++)
            {
                if (dtsAveria.Tables[0].Rows[i].ItemArray[0].ToString().Trim() == tipoAveria)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool ValidarTipoVisitaYVisitaIncorrecta(string tipoVisita)
        {
            TiposVisitaDB objTiposVisitaDB = new TiposVisitaDB();
            // Buscamos los tipos de visitas.
            DataSet dtsVisita = objTiposVisitaDB.GetTiposVisitaIncorrecta(1);
            for (int i = 0; i < dtsVisita.Tables[0].Rows.Count; i++)
            {
                if (dtsVisita.Tables[0].Rows[i].ItemArray[0].ToString().Trim() == tipoVisita)
                {
                    return true;
                }
            }
            // Buscamos los tipos de visitas incorrectas.
            DataSet dtsVisitaincorrecta = objTiposVisitaDB.GetTiposVisita(1);
            for (int i = 0; i < dtsVisitaincorrecta.Tables[0].Rows.Count; i++)
            {
                if (dtsVisitaincorrecta.Tables[0].Rows[i].ItemArray[0].ToString().Trim() == tipoVisita)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void GuardarTrazaLlamada(WSRequest request)
        {
            CierreSolicitudRequest cierreSolicitud = (CierreSolicitudRequest)request;
            // Generamos el fichero xml, y guardamos la ruta del mismo en la BB.DD.
            AperturaSolicitudDB aperturaSolicitudDB = new AperturaSolicitudDB();
            string xml = ToXML(cierreSolicitud, PROVEEDOR);
            //MandarMail("ACCESO", "WS PROO_2", "maquintela@gfi.es", true);
            // Guardar la traza y dejar el id de la traza guardada.
            TrazaDB trazaDB = new TrazaDB();
            //MandarMail("ACCESO", "WS PROO_3", "maquintela@gfi.es", true);
            this._idTraza = trazaDB.InsertarYObteneridTrazaCierreSolicitud("DATOS RECIBIDOS", xml, cierreSolicitud, CONTRATO);

            //20201021 BUA ADD R#23245: Guardar el Ticket de combustión
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
            //20201021 BUA END R#23245: Guardar el Ticket de combustión
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
            CierreSolicitudResponse CierreSolicitudResponse = (CierreSolicitudResponse)response;

            if (this._idTraza.HasValue)
            {
                // Actualizar la traza con el resultado de la operación.
                TrazaDB trazaDB = new TrazaDB();
                if (CierreSolicitudResponse.TieneError)
                {
                    // Si tiene errores.
                    trazaDB.ActualizarTraza(this._idTraza.ToString(), CierreSolicitudResponse.ListaErrores[0].Descripcion.ToString());
                }
                else if (CierreSolicitudResponse.TieneAviso)
                {
                    // Si tiene avisos.
                    trazaDB.ActualizarTraza(this._idTraza.ToString(), CierreSolicitudResponse.ListaAvisos[0].Descripcion.ToString());
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

        //20210120 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        protected void GeneracionContratoGC_EnvioEdatalia(CierreSolicitudRequest cierreSolicitud)
        {
            try
            {
                if (int.Parse(SUBTIPO_SOLICITUD) == (int)Enumerados.SubtipoSolicitud.GasConfort && int.Parse(cierreSolicitud.EstadoSolicitud.ToString()) == (int)Enumerados.EstadosSolicitudes.OfertaPresentadaPdteFirma)
                {
                    string email = CaracteristicaHistorico.GetCaracteristicaValor(cierreSolicitud.idSolicitud, "185");
                    if (!String.IsNullOrEmpty(email))
                    {
                        string nombreFichero = PCamposPdfContratoGasConfort.GenerarContratoGasconfort(CONTRATO, Convert.ToDecimal(cierreSolicitud.idSolicitud));
                        if (String.IsNullOrEmpty(nombreFichero))
                        {
                            throw new Exception(Resources.TextosJavaScript.ERROR_GENERAR_CONTRATO_GC_DIGITAL); //"Error al generar el contrato digital. No enviado a Edatalia"
                        }
                        else
                        {
                            string refExternaEdatalia = cierreSolicitud.idSolicitud.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string nombreCliente = String.Empty;
                            string dniCliente = String.Empty;
                            string movilCliente = String.Empty;
                            string idDocEdatalia = String.Empty;
                            IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(CONTRATO);
                            if (datosMantenimiento != null)
                            {
                                while (datosMantenimiento.Read())
                                {
                                    nombreCliente = ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_TITULAR")).TrimEnd() + " " + ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO1")).TrimEnd() + " " + ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO2")).TrimEnd();
                                    dniCliente = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DNI");
                                    movilCliente = ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NUM_MOVIL")).TrimEnd();
                                }
                            }
                            if (!String.IsNullOrEmpty(email))
                            {
                                //Llamar a WS Edatalia para que envie un email al cliente para que firme el contrato de GC
                                UtilidadesWebServices callWS = new UtilidadesWebServices();
                                idDocEdatalia = callWS.llamadaWSEdataliaAltaDocumentoPorEmail(nombreFichero, refExternaEdatalia, nombreCliente, dniCliente, email, movilCliente);

                            }
                            if (!String.IsNullOrEmpty(idDocEdatalia))
                            {
                                //20210708 BGN MOD BEG R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos
                                if (idDocEdatalia.Equals("-38"))
                                {
                                    //Pasamos la solicitud al estado 083 - Pdte. corregir e-mail
                                    Solicitud soli = new Solicitud();
                                    soli.CambioEstadoSolicitudeHistorico(cierreSolicitud.idSolicitud.ToString(), "083", "WS_EDATA", "RecipientMail format is wrong.");
                                    throw new Exception(Resources.TextosJavaScript.ERROR_ENVIO_CONTRATO_GC_DIGITAL); //"Error al enviar el contrato digital a Edatalia"
                                }
                                else
                                {
                                    //Guardar en tabla DOCUMENTO
                                    DocumentoDTO documentoDto = new DocumentoDTO();
                                    documentoDto.CodContrato = CONTRATO;
                                    documentoDto.IdSolicitud = cierreSolicitud.idSolicitud;
                                    documentoDto.NombreDocumento = nombreFichero;
                                    documentoDto.IdTipoDocumento = 1;
                                    documentoDto.EnviarADelta = false;
                                    documentoDto.IdEnvioEdatalia = refExternaEdatalia;
                                    documentoDto.FechaEnvioEdatalia = DateTime.Now;
                                    documentoDto.IdDocEdatalia = idDocEdatalia;
                                    documentoDto = Documento.Insertar(documentoDto, ServiceCredentials.UserName);
                                    //Mover fichero a RUTA_FICHEROS_EDATALIA_ENVIADOS
                                    ConfiguracionDTO rutaEdatalia = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_EDATALIA);
                                    String ficheroOrigen = rutaEdatalia.Valor + nombreFichero;
                                    ConfiguracionDTO rutaEdataliaEnviados = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_EDATALIA_ENVIADOS);
                                    FileUtils.FileMoveRewrite(ManejadorFicheros.GetImpersonator(), ficheroOrigen, rutaEdataliaEnviados.Valor);
                                }
                                //20210708 BGN MOD END R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos                                
                            }
                            else
                            {
                                throw new Exception(Resources.TextosJavaScript.ERROR_ENVIO_CONTRATO_GC_DIGITAL); //"Error al enviar el contrato digital a Edatalia"
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en la llamada al CierreVisitaWS.CerrarVisita.GeneracionContratoGC_EnvioEdatalia", LogHelper.Category.BussinessLogic, ex);
                UtilidadesMail.EnviarMensajeError("Error en la llamada al CierreVisitaWS.CerrarVisita.GeneracionContratoGC_EnvioEdatalia", "Error en la llamada al CierreVisitaWS.CerrarVisita.GeneracionContratoGC_EnvioEdatalia. " + ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_");
            }
        }
        //20210120 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        
    }
}