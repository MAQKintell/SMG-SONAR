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


namespace Iberdrola.SMG.WS
{
    public class ContratoVigenteWS : WSBase
    {
        /// <summary>
        /// Cierra la visita con los datos pasados por parámetro
        /// </summary>
        /// <param name="cierreVisita">Objeto de tipo CierreVisitaRequest con los datos 
        /// necesarios para realizar el cierre de la vista</param>
        /// <returns>Objeto de tipo CierreVisitaResponse con el resultado del cierre de la respuesta</returns>

        public Authentication ServiceCredentials;


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public ContratoVigenteResponse getContratoVigente(ContratoVigenteRequest contratoVigente)
        {
            ContratoVigenteResponse response = new ContratoVigenteResponse();

            //Boolean habilitarWS = false;
            //ConfiguracionDTO confHabilitarWS = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_WS);
            //if (confHabilitarWS != null && !string.IsNullOrEmpty(confHabilitarWS.Valor) && Boolean.Parse(confHabilitarWS.Valor))
            //{
            //    habilitarWS = Boolean.Parse(confHabilitarWS.Valor);
            //}

            Boolean habilitarWS = ValidarwebServiceActivo(Enumerados.Configuracion.HABILITAR_CONTRATOVIGENTE_WS.ToString());

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
                                this.GetDatosContratoVigente(contratoVigente, response, ServiceCredentials.UserName);

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
                            LogHelper.Error("Error en la llamada al contratoVigenteWS.getContratoVigente", LogHelper.Category.BussinessLogic, ex);
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
                    LogHelper.Error("Error en la llamada al contratoVigenteWS.getContratoVigente", LogHelper.Category.BussinessLogic, ex);
                    // TODO: GGB  ver si enviamos un email. en caso de ponerlo parametrizarlo con una variable de 
                    // configuración que indique si está activo el email de errores de WS.
                    // MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data, "ERROR NO DEFINIDO" + usuario, "maquintela@gfi.es", true);
                    response.AddError(CommonWSError.ErrorNoDefinido);
                }
            }
            //// Si no tiene error devolvemos que todo ha ido bien.
            //if (!response.TieneError)
            //{
            //    // TODO: GGB cargar descripciones errores.
            //    response.AddError(CommonWSError.VisitaActualizadaCorrectamente);
            //}

            // Retornamos la respuesta
            return response;

        }


        /// <summary>
        /// GetDatosContratoVigente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="usuario"></param>
        private void GetDatosContratoVigente(ContratoVigenteRequest request, ContratoVigenteResponse response, string usuario)
        {

            string contrato = request.CodigoContrato;
            try
            {
                DataTable dtContratoVigente = null;


                // A partir del contrato Obtenemos los datos que necesitaran
                ContratoVigenteDB objContratoVigenteDB = new ContratoVigenteDB();
                dtContratoVigente = objContratoVigenteDB.GetDatosContratoVigente(contrato);
                
                // Kintell R#35439
                // BGN
                if (dtContratoVigente.Rows[0].ItemArray.Length == 5)
                {
                    //Contrato no encontrato, porque si lo encontrase devolveria mas datos.
                    UtilidadesWebServices uw = new UtilidadesWebServices();
                    String resultadoLlamadaObtenerDatosContratoDelta = uw.llamadaWSobtenerDatosCtoOperaSMG(contrato);
                    XmlNode datos = uw.StringAXMLRepairAndCare(resultadoLlamadaObtenerDatosContratoDelta, contrato);
                    Mantenimiento mantenimiento = new Mantenimiento();
                    MantenimientoDTO mantenimientoDTO = mantenimiento.DatosMantenimientoDesdeDeltaYAltaContrato(datos, true);

                    dtContratoVigente = objContratoVigenteDB.GetDatosContratoVigente(contrato);
                }
                //END

                DataRow[] foundRowsContratoVigente = dtContratoVigente.Select("INCIDENCIA_AUTOMATICA = 'S'");

                //Si no encuentra la linea con S, rellenamos con la fila que tenga N
                if (foundRowsContratoVigente.Length == 0)
                {
                    foundRowsContratoVigente = dtContratoVigente.Select("INCIDENCIA_AUTOMATICA = 'N'");
                }

                string CONTRATO_VIGENTE = foundRowsContratoVigente[0].ItemArray[0].ToString();
                string INDICATIVO_ALTA = foundRowsContratoVigente[0].ItemArray[1].ToString();
                string NUM_AVERIAS_REALIZADAS = foundRowsContratoVigente[0].ItemArray[2].ToString();
                string NUM_AVERIAS_SOPORTADAS = foundRowsContratoVigente[0].ItemArray[3].ToString();
                string IND_GENERACION_INCIDENCIA_AUTOMATICA = foundRowsContratoVigente[0].ItemArray[4].ToString();
                string TIPO_SOLICITUD_GENERAR = foundRowsContratoVigente[0].ItemArray[5].ToString();
                string IND_GENERACION_VISITA_AUTOMATICA = foundRowsContratoVigente[0].ItemArray[6].ToString();
                string TIPO_VISITA_GENERAR = foundRowsContratoVigente[0].ItemArray[7].ToString();
                string MODELO_CALDERA = foundRowsContratoVigente[0].ItemArray[8].ToString();
                string MARCA_CALDERA = foundRowsContratoVigente[0].ItemArray[9].ToString();

                // KINTELL 29/12/2016 SACAMOS SI TIENE SOLICITUD DE VISITA ABIERTA Y EL ESTADO DE LA VISITA.
                string SOLICITUD_VISITA_ABIERTA = foundRowsContratoVigente[0].ItemArray[10].ToString();
                string ESTADO_VISITA = foundRowsContratoVigente[0].ItemArray[11].ToString();
                //17/12/2018 Cambio en Tu Iberdola P&S.
                string COBERTURA = foundRowsContratoVigente[0].ItemArray[12].ToString();
                string FECHARENOVACION = foundRowsContratoVigente[0].ItemArray[13].ToString();
                string NUMEROSERIETERMOSTATO = foundRowsContratoVigente[0].ItemArray[14].ToString();
                string NUMEROSERIECALDERA = foundRowsContratoVigente[0].ItemArray[15].ToString();
                string MODOPAGO = foundRowsContratoVigente[0].ItemArray[16].ToString();
                string COD_COBERTURA = foundRowsContratoVigente[0].ItemArray[17].ToString();
                string ABRIR_AVERIA = ComprobarSiPuedeAbrirAverias(contrato);

                if (String.IsNullOrEmpty(MARCA_CALDERA)) { MARCA_CALDERA = "Desconocido"; }
                if (String.IsNullOrEmpty(MODELO_CALDERA)) { MODELO_CALDERA = "Desconocido"; }

                //Si esta de alta, ademas devolvemos los siguientes campos
                if (INDICATIVO_ALTA == "S")
                {
                    //En caso de tener una averia abierta en los ultimos 30 días, generamos el tipo averia incorrecta
                    //independientemente de que tenga una visita creada en los ultimos 21 días.
                    if (IND_GENERACION_INCIDENCIA_AUTOMATICA == "S")
                    {
                        IND_GENERACION_VISITA_AUTOMATICA = "N";
                        TIPO_VISITA_GENERAR = " ";
                    }

                    //20210628 BGN BEG R#31332 Modificar WS con App para añadir campos que indiquen si se puede solicitar averías o no
                    response.AddDatosContratoVigente(CONTRATO_VIGENTE,
                                                         INDICATIVO_ALTA,
                                                         NUM_AVERIAS_REALIZADAS,
                                                         NUM_AVERIAS_SOPORTADAS,
                                                         IND_GENERACION_INCIDENCIA_AUTOMATICA,
                                                         TIPO_SOLICITUD_GENERAR,
                                                         IND_GENERACION_VISITA_AUTOMATICA,
                                                         TIPO_VISITA_GENERAR,
                                                         MODELO_CALDERA,
                                                         MARCA_CALDERA,
                                                         SOLICITUD_VISITA_ABIERTA,
                                                         ESTADO_VISITA,
                                                         COBERTURA,
                                                         FECHARENOVACION,
                                                         NUMEROSERIETERMOSTATO,
                                                         NUMEROSERIECALDERA,
                                                         MODOPAGO,
                                                         COD_COBERTURA,
                                                         ABRIR_AVERIA);
                    //20210628 BGN END R#31332 Modificar WS con App para añadir campos que indiquen si se puede solicitar averías o no
                }

            }
            catch (Exception ex)
            {
                ////ERROR CONTRATO VIGENTE
                //response.AddErrorContratoVigente(ContratoVigenteWSError.ErrorObtenerDatos);

                response.AddDatosContratoVigenteNoExistente(contrato,
                                                            "N",
                                                             " ",
                                                             " ");
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
            ContratoVigenteRequest contratoVigente = (ContratoVigenteRequest)request;

            // Validamos que tengamos datos de entrada.
            if (contratoVigente == null)
            {
                response.AddErrorContratoVigente(ContratoVigenteWSError.ErrorDatosVacios);
                return false;
            }

            // Si llegamos a este punto hemos superado todas las validaciones y retornamos true.
            return true;
        }


        protected override void GuardarTrazaLlamada(WSRequest request)
        {
            ContratoVigenteRequest contratoVigente = (ContratoVigenteRequest)request;
            // Generamos el fichero xnl, y guardamos la ruta del mismo en la BB.DD.
            ContratoVigenteDB ContratoVigenteDB = new ContratoVigenteDB();
            string xml = ToXML(contratoVigente);
            // Guardar la traza y dejar el id de la traza guardada.
            TrazaDB trazaDB = new TrazaDB();
            this._idTraza = trazaDB.InsertarYObteneridTrazaContratoVigente("DATOS RECIBIDOS", xml, contratoVigente);
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
            ContratoVigenteResponse contratoVigenteResponse = (ContratoVigenteResponse)response;
            if (this._idTraza.HasValue)
            {
                // Actualizar la traza con el resultado de la operación.
                TrazaDB trazaDB = new TrazaDB();
                if (contratoVigenteResponse.TieneError)
                {
                    // Si tiene errores.
                    trazaDB.ActualizarTraza(this._idTraza.ToString(), contratoVigenteResponse.ListaErrores[0].Descripcion.ToString());
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

        //20210629 BGN BEG R#31332 Modificar WS con App para añadir campos que indiquen si se puede solicitar averías o no
        protected string ComprobarSiPuedeAbrirAverias(string codContrato)
        {
            IDataReader drFechaVisitaFechaCierreSolicitud = null;
            try
            {
                Mantenimiento mantenimiento = new Mantenimiento();
                MantenimientoDTO mantenimientoDTO2 = mantenimiento.DatosMantenimientoSinPais(codContrato);
                if (!String.IsNullOrEmpty(mantenimientoDTO2.CODEFV))
                {
                    SolicitudesDB dbSol = new SolicitudesDB();
                    DataSet datosEFV = dbSol.ObtenerDatosDesdeSentencia("SELECT SMGPROTECCIONGAS,GASCONFORT,SMGPROTECCIONGASINDEPENDIENTE from TIPO_EFV where COD_EFV='" + mantenimientoDTO2.CODEFV.ToString() + "'");
                    Boolean esProteccionGas = Boolean.Parse(datosEFV.Tables[0].Rows[0].ItemArray[0].ToString());
                    Boolean esGasConfort = Boolean.Parse(datosEFV.Tables[0].Rows[0].ItemArray[1].ToString());
                    Boolean esProteccionGasIndependiente = Boolean.Parse(datosEFV.Tables[0].Rows[0].ItemArray[2].ToString());
                    // Si es Protección Gas o Protección Gas Independiente no puede abrir averias
                    if (esProteccionGas || esProteccionGasIndependiente)
                    {
                        return "N";
                    }
                    //Si para el contrato tiene una solicitud de averia o avería incorrecta ya abierta no dejamos generar otra avería.
                    string subTipoAveria = "001";
                    string subTipoAveriaIncorrecta = "004";
                    if (esGasConfort)
                    {
                        subTipoAveria = "012";
                        subTipoAveriaIncorrecta = "013";
                    }
                    SolicitudDB solDB = new SolicitudDB();
                    DataTable dtSolAveriaAbiertas = solDB.ObtenerSolicitudesPorContratoYSubTipoSolicitud(codContrato, subTipoAveria, 0, 1);
                    if (dtSolAveriaAbiertas.Rows.Count > 0)
                    {
                        return "N";
                    }
                    DataTable dtSolAveriaIncorrectaAbiertas = solDB.ObtenerSolicitudesPorContratoYSubTipoSolicitud(codContrato, subTipoAveriaIncorrecta, 0, 1);
                    if (dtSolAveriaIncorrectaAbiertas.Rows.Count > 0)
                    {
                        return "N";
                    }
                }

                Int16? intCodVisita = Contrato.ObtenerCodigoUltimaVisita(codContrato);

                VisitasDB objVisitasDB = new VisitasDB();
                drFechaVisitaFechaCierreSolicitud = objVisitasDB.ObtenerFechaVisitaFechaCierreSolicitudAveria(codContrato, intCodVisita.ToString(), "");

                DateTime fechaVisita = DateTime.Parse("01/01/1900");
                string estadoSolicitud = "";
                DateTime fechaCierreSolicitud = DateTime.Parse("01/01/1900");
                string estadoVisita = "";
                int categoriaVisita = 1;

                while (drFechaVisitaFechaCierreSolicitud.Read())
                {
                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_ESTADO_VISITA") != null)
                    {
                        estadoVisita = DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_ESTADO_VISITA").ToString();
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "FEC_VISITA") != null)
                    {
                        fechaVisita = DateTime.Parse(DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "FEC_VISITA").ToString());
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "Estado_solicitud") != null)
                    {
                        estadoSolicitud = DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "Estado_solicitud").ToString();
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "Valor") != null)
                    {
                        fechaCierreSolicitud = DateTime.Parse(DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "Valor").ToString());
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_CATEGORIA_VISITA") != null)
                    {
                        categoriaVisita = int.Parse(DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_CATEGORIA_VISITA").ToString());
                    }


                    // Si han pasado menos de 21 días desde que se hizo la visita no se permite generar averías ni averías incorrectas.
                    if (fechaVisita.Year.ToString() != "1900")
                    {
                        if (fechaVisita.AddDays(21) > DateTime.Now)
                        {
                            if (estadoVisita == "02" || estadoVisita == "09")
                            {
                                return "N";
                            }
                        }
                    }

                    // Si han pasado menos de 30 días desde que se ha reparado una avería no dejamos generar avería.
                    if (estadoSolicitud != "")
                    {
                        if (fechaCierreSolicitud.Year.ToString() != "1900")
                        {
                            if (fechaCierreSolicitud.AddDays(30) > DateTime.Now)
                            {
                                return "N";
                            }
                        }
                    }                    
                }
                
                return "S";
            }
            finally
            {
                if (drFechaVisitaFechaCierreSolicitud != null)
                {
                    drFechaVisitaFechaCierreSolicitud.Close();
                }
            }
        }
        //20210629 BGN END R#31332 Modificar WS con App para añadir campos que indiquen si se puede solicitar averías o no

    }
}