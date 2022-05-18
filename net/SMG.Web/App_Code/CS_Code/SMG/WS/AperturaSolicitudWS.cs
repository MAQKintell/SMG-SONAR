using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Services;
using System.Web.Services.Protocols;
using Iberdrola.Commons.Logging;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Validations;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;

namespace Iberdrola.SMG.WS
{
    public class AperturaSolicitudWS : WSBase
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
        public AperturaSolicitudResponse AbrirSolicitud(AperturaSolicitudRequest aperturaSolicitud)
        {
            AperturaSolicitudResponse response = new AperturaSolicitudResponse();

            //Boolean habilitarWS = false;
            //ConfiguracionDTO confHabilitarWS = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_WS);
            //if (confHabilitarWS != null && !string.IsNullOrEmpty(confHabilitarWS.Valor) && Boolean.Parse(confHabilitarWS.Valor))
            //{
            //    habilitarWS = Boolean.Parse(confHabilitarWS.Valor);
            //}

            Boolean habilitarWS = ValidarwebServiceActivo(Enumerados.Configuracion.HABILITAR_APERTURASOLICITUD_WS.ToString());

            if (!habilitarWS)
            {
                response.AddError(CommonWSError.ErrorWSInactivo);
            }
            else
            {
                try
                {
                    //MandarMail("ACCESO", "WS PROO_10", "maquintela@gfi.es", true);
                    // Primero guardamos la traza de la llamada.
                    this.GuardarTrazaLlamada(aperturaSolicitud);
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
                            if (this.ValidarDatosEntrada(aperturaSolicitud, response, ServiceCredentials.UserName))
                            {
                                //Kintell R#35436 BEGIN
                                string cod_subtipo_solicitud = aperturaSolicitud.SubtipoSolicitud;
                                if (cod_subtipo_solicitud == StringEnum.GetStringValue(Enumerados.SubtipoSolicitud.GasConfort)) //008
                                {
                                    string resultado = CrearSolitudGCSobreContratoFicticio(aperturaSolicitud.CodigoContrato);

                                    if (resultado.ToUpper() == "ERROR")
                                    {
                                        response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorAltaSolicitud);
                                    }
                                    else
                                    {
                                        response.AddRespuestaCustom(resultado);
                                    }
                                }
                                else
                                {
                                    //Solo si se actualiza correctamente la solicitud, actualizamos los datos de caldera
                                    if (this.RealizarAltaSolicitud(aperturaSolicitud, response, ServiceCredentials.UserName))
                                    {
                                        this.ActualizarCaldera(aperturaSolicitud, response);
                                    }
                                }
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
                            LogHelper.Error("Error en la llamada al AperturaSolicitudWS.AperturaSolicitud", LogHelper.Category.BussinessLogic, ex);
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
                    LogHelper.Error("Error en la llamada al AperturaSolicitudWS.AperturaSolicitud", LogHelper.Category.BussinessLogic, ex);
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

        private void ActualizarCaldera(AperturaSolicitudRequest request, AperturaSolicitudResponse response)
        {
            try
            {
                // Si la marca y el modelo de la caldera vienen vacios, metemos "desconocido".
                if (request.IdMarcaCaldera == null)
                {
                    request.IdMarcaCaldera=106;
                }

                if (request.ModeloCaldera==null)
                {
                    request.ModeloCaldera="DESCONOCIDO";
                }
                CalderasDB objCalderasDB = new CalderasDB();
                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                DataSet ds = new DataSet();
                ds = objCalderasDB.GetCalderasPorContrato(request.CodigoContrato);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    objSolicitudesDB.InsertarCalderaContrato(request.CodigoContrato, int.Parse(request.IdMarcaCaldera.ToString()), request.ModeloCaldera.ToString());
                }
                else
                {
                    objSolicitudesDB.ActualizarCalderaContrato(request.CodigoContrato, int.Parse(request.IdMarcaCaldera.ToString()), request.ModeloCaldera.ToString(), "", "", 0, 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en la apertura de solicitud WS, actualizar caldera:" + ex.Message, LogHelper.Category.BussinessLogic);
                response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorDatosCalderaIncorrectos);
            }
        }

        private bool RealizarAltaSolicitud(AperturaSolicitudRequest request, AperturaSolicitudResponse response, string usuario)
        {
            if (!response.TieneError)
            {
                try
                {
                    DateTime fechaHora = DateTime.Now;

                    string contrato = request.CodigoContrato;
                    string cod_tipo_solicitud = "001";
                    string cod_subtipo_solicitud = request.SubtipoSolicitud;
                    string cod_estado = "001";
                    string telef_contacto = request.TelefonoContacto;
                    string pers_contacto = request.PersonaContacto;
                    string cod_averia = "";

                    int id_movimiento = 0;

                    if (request.TipoAveria == null)
                    {
                        cod_averia = request.TipoVisita;
                    }
                    else
                    {
                        cod_averia=request.TipoAveria;
                    }
                    string observaciones = "";
                    if (request.ObservacionesSolicitud != null)
                    {
                        string Horas = fechaHora.Hour.ToString();
                        if (Horas.Length == 1) Horas = "0" + Horas;
                        string Minutos = fechaHora.Minute.ToString();
                        if (Minutos.Length == 1) Minutos = "0" + Minutos;
                        observaciones = "[" + fechaHora.ToString().Substring(0, 10) + "-" + Horas + ":" + Minutos + "] " + usuario + ": " + request.ObservacionesSolicitud;
                    }

                    //BUA 19/01/2021 R#27548 - Generación masiva de solicitudes de Inspección 
                    // Buscamos el proveedor de Averia por contrato.
                    AperturaSolicitudDB aperturaSolicitudDB = new AperturaSolicitudDB();
                    SolicitudesDB objSolicitudesDB1 = new SolicitudesDB();
                    CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
                    HistoricoDB objHistoricoDB = new HistoricoDB();
                    VisitasDB objVisitasDB = new VisitasDB();
                    Solicitud soli = new Solicitud();

                    //Obtenemos si es extraudinaria o no
                    bool esExtraordinaria = false;

                    string Proveedor = aperturaSolicitudDB.ObtenerProveedorAveriaPorContrato(request.CodigoContrato);
                    Proveedor = Proveedor.Substring(0, 3);

                    
                    if (cod_subtipo_solicitud == StringEnum.GetStringValue(Enumerados.SubtipoSolicitud.SolicitudInspeccionGas)) //016
                    {
                        Int64 numeroSolicitudes = objVisitasDB.ComprobarSisolicitudInspeccionGasAbiertaEnLosUltimosCincoAños(contrato);
                        Boolean pagoInspeccion = objVisitasDB.comprobarSiTieneQuePagarInspeccion(contrato);

                        if (numeroSolicitudes > 0 || pagoInspeccion)
                            esExtraordinaria = true;

                        cod_estado = StringEnum.GetStringValue(Enumerados.EstadosSolicitudes.InspeccionPendiente); //069
                    }

                    // Insertamos la solicitud.
                    //int id_solicitud = aperturaSolicitudDB.AddSolicitudWS(contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_averia, observaciones, Proveedor, false, false);
                    int id_solicitud = objSolicitudesDB1.AddSolicitud(contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_averia, observaciones, Proveedor, false, false, esExtraordinaria);

                    if (id_solicitud > 0)
                    {
                        // Metemos el historico.
                        //20/12/2017 BGN Capa Negocio actualizar historico para incluir llamada WS Alta Interaccion y altaSolic Proveedor
                        //HistoricoDB objHistoricoDB = new HistoricoDB();
                        //objHistoricoDB.AddHistoricoSolicitud(id_solicitud.ToString(), "001", usuario, cod_estado, observaciones, Proveedor);
                        id_movimiento = soli.ActualizarHistoricoSolicitud(contrato, id_solicitud.ToString(), "001", usuario, cod_estado, observaciones, Proveedor, cod_subtipo_solicitud, cod_averia, "0", "A");

                        if (cod_subtipo_solicitud == StringEnum.GetStringValue(Enumerados.SubtipoSolicitud.SolicitudInspeccionGas)) //016
                        {
                            //Obtenemos el CUPS del mantenimiento
                            IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(contrato);

                            string CUPS = string.Empty;
                            if (datosMantenimiento != null)
                            {
                                while (datosMantenimiento.Read())
                                {
                                    CUPS = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "CUPS");
                                }
                            }

                            //Añadir Caracteristica CUI con el valor del CUPS
                            int id_caracteristica_CUI = objCaracteristicasDB.AddCaracteristica(id_solicitud.ToString(), "160", CUPS, fechaHora.ToShortDateString());
                            // Add the hystoric of the movement...
                            objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica_CUI, id_movimiento.ToString(), "160", CUPS);

                            //Añadir Caracteristica OrdInLoco
                            string ordInLoco = (request.OrdInLoco == "1" || request.OrdInLoco.ToUpper() == "SI" || request.OrdInLoco.ToUpper() == "SIM") ? "Sim" : "Não";
                            int id_caracteristica_OrdInLoco = objCaracteristicasDB.AddCaracteristica(id_solicitud.ToString(), "161", ordInLoco, fechaHora.ToShortDateString());
                            // Add the hystoric of the movement...
                            objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica_OrdInLoco, id_movimiento.ToString(), "161", ordInLoco);

                            //Añadir Caracteristica FechaPrevista
                            int id_caracteristica_FechaPrevista = objCaracteristicasDB.AddCaracteristica(id_solicitud.ToString(), "162", request.FechaPrevista, fechaHora.ToShortDateString());
                            // Add the hystoric of the movement...
                            objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica_FechaPrevista, id_movimiento.ToString(), "162", request.FechaPrevista);

                            //Añadir Caracteristica Horario
                            int id_caracteristica_HorarioInspeccion = objCaracteristicasDB.AddCaracteristica(id_solicitud.ToString(), "163", request.HorarioInspeccion, fechaHora.ToShortDateString());
                            // Add the hystoric of the movement...
                            objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica_HorarioInspeccion, id_movimiento.ToString(), "163", request.HorarioInspeccion);

                            //Añadir Caracteristica CambioPeriodoHorario
                            string cambioPeriodoHorario = (request.CambioPeriodoHorario == "1" || request.CambioPeriodoHorario.ToUpper() == "SI" || request.CambioPeriodoHorario.ToUpper() == "SIM") ? "Sim" : "Não";
                            int id_caracteristica_CambioPeriodoHorario = objCaracteristicasDB.AddCaracteristica(id_solicitud.ToString(), "168", cambioPeriodoHorario, fechaHora.ToShortDateString());
                            // Add the hystoric of the movement...
                            objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica_CambioPeriodoHorario, id_movimiento.ToString(), "168", cambioPeriodoHorario);

                            ////BEG BGN 02/05/2022 R#36138 - Nuevo campo en las solicitudes de inspección
                            ////Añadir Caracteristica Fornecimento gás
                            //string fornecimientoGas = "Não";
                            //int id_caracteristica_fornecimientoGas = objCaracteristicasDB.AddCaracteristica(id_solicitud.ToString(), "196", fornecimientoGas, fechaHora.ToShortDateString());
                            //// Add the hystoric of the movement...
                            //objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica_fornecimientoGas, id_movimiento.ToString(), "196", fornecimientoGas);
                            ////END BGN 02/05/2022 R#36138 - Nuevo campo en las solicitudes de inspección
                        }
                        //END BUA 19/01/2021 R#27548 - Generación masiva de solicitudes de Inspección


                    }
                    else
                    {
                        //20210709 MOD BGN R#31331 - Modificar ws de alta de solicitud de la APP para no permitir solicitar averías si existe alguna abierta
                        //response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorSolicitudYaAbierta);
                        //ERROR ALTA SOLICITUD
                        response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorAltaSolicitud);
                        //20210709 MOD END R#31331 - Modificar ws de alta de solicitud de la APP para no permitir solicitar averías si existe alguna abierta
                    }

                    // Enviamos interaccion de apertura. 16/11/2017
                    
                }
                catch (Exception ex)
                {
                    //ERROR ALTA SOLICITUD
                    response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorAltaSolicitud);
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

        protected bool ValidarDatosEntrada(WSRequest request, WSResponse response, string usu)
        {
            AperturaSolicitudRequest aperturaSolicitud = (AperturaSolicitudRequest)request;

            // Validamos que tengamos datos de entrada.
            if (aperturaSolicitud == null)
            {
                response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorDatosVacios);
                return false;
            }

            // Validamos que el contrato venga informado
            if ((DBNull.Value.Equals(aperturaSolicitud.CodigoContrato)) || (aperturaSolicitud.CodigoContrato.ToString() == "" ))
            {
                response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                return false;
            }

            // Validamos que el subtipo solicitud venga informado.
            if (!DBNull.Value.Equals(aperturaSolicitud.SubtipoSolicitud))
            {
                if (aperturaSolicitud.SubtipoSolicitud.ToString() != "")
                {
                    // Validamos que el subtipo solicitud exista en la BB.DD.
                    if (!ValidarSubtipoSolicitud(aperturaSolicitud.SubtipoSolicitud))
                    {
                        response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorCodigoSubtipoSolicitud);
                        return false;
                    }
                    // Comentarlo hasta que Rubén diga cuando subirlo.
                    //else
                    //{
                    //    // Si el contrato es de protección de gas, solo puede abrir solicitudes de visita.
                    //    // Kintell 11/09/2018.
                    //    VisitasDB objVisitasDB = new VisitasDB();
                    //    if (objVisitasDB.esProteccionGas(aperturaSolicitud.CodigoContrato) && aperturaSolicitud.SubtipoSolicitud != "002")
                    //    {
                    //        response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorCodigoSubtipoSolicitud);
                    //        return false;
                    //    }
                    //}

                    //BUA 19/01/2021 R#27548 - Generación masiva de solicitudes de Inspección
                    if (aperturaSolicitud.SubtipoSolicitud == StringEnum.GetStringValue(Enumerados.SubtipoSolicitud.SolicitudInspeccionGas) //016
                        && (string.IsNullOrEmpty(aperturaSolicitud.OrdInLoco)
                            || string.IsNullOrEmpty(aperturaSolicitud.FechaPrevista)
                            || string.IsNullOrEmpty(aperturaSolicitud.HorarioInspeccion)
                            || string.IsNullOrEmpty(aperturaSolicitud.CambioPeriodoHorario)))
                    {
                        response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                        return false;
                    }
                    //END BUA 19/01/2021 R#27548 - Generación masiva de solicitudes de Inspección


                    // KINTELL 12/05/2021 No permitimos abrir solicitudes de Inspección si el EFV no es de Inspección.
                    if (aperturaSolicitud.SubtipoSolicitud == StringEnum.GetStringValue(Enumerados.SubtipoSolicitud.SolicitudInspeccionGas))
                    {
                        ConsultasDB db = new ConsultasDB();
                        if (!db.esInspeccion(aperturaSolicitud.CodigoContrato))
                        {
                            response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.EFVNoValidoParaGeneracionDeInspeccion);
                            return false;
                        }
                    }
                    //
                }
                else
                {
                    response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                    return false;
                }
            }
            else
            {
                response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                return false;
            }

            if (usu.Equals("WS_PYS") && !aperturaSolicitud.TelefonoContacto.StartsWith("+3")) 
            {
                aperturaSolicitud.TelefonoContacto = "+34" + aperturaSolicitud.TelefonoContacto;
            }
            // Validamos que el telefono venga informado y tenga 12 digitos.
            if (!this.ValidarTelefonoContacto(aperturaSolicitud.TelefonoContacto))
            {
                response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFormatoTelefonoContactoConPrefijo);
                return false;
            }

            // Validamos que la persona de contacto venga informado.
            if (DBNull.Value.Equals(aperturaSolicitud.PersonaContacto))
            {
                response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                return false;
            }

            //BUA 19/01/2021 R#27548 - Generación masiva de solicitudes de Inspección
            if (aperturaSolicitud.SubtipoSolicitud != StringEnum.GetStringValue(Enumerados.SubtipoSolicitud.SolicitudInspeccionGas))
            {
                // Validamos que el horario venga informado.
                if (DBNull.Value.Equals(aperturaSolicitud.Horario) || aperturaSolicitud.Horario.ToString().Equals("") )
                {
                    response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                    return false;
                }
                else
                {
                    if (aperturaSolicitud.Horario.ToString() != "MAÑANA" &&
                        aperturaSolicitud.Horario.ToString() != "TARDE" &&
                        aperturaSolicitud.Horario.ToString() != "NOCHE" &&
                        aperturaSolicitud.Horario.ToString() != "CUALQUIERA")
                    {
                        response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorDatosErroneosHorario);
                        return false;
                    }
                }

                // Validamos que el tipo de Averia o Visita venga informado.
                //if (DBNull.Value.Equals(aperturaSolicitud.TipoAveria) && DBNull.Value.Equals(aperturaSolicitud.TipoVisita))
                if ((aperturaSolicitud.TipoAveria == null ) && (aperturaSolicitud.TipoVisita== null ))
                {
                    response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                    return false;
                }
                else 
                {
                    if (aperturaSolicitud.TipoAveria != null)
                    {
                        // Comprobamos el valor del tipo de averia.
                        if (!ValidarTipoAveria(aperturaSolicitud.TipoAveria))
                        {
                            response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorTipoAveria);
                            return false;
                        }

                        if (aperturaSolicitud.TipoAveria.ToUpper() == "OTROS")
                        {
                            if (DBNull.Value.Equals(aperturaSolicitud.ObservacionesSolicitud) || (aperturaSolicitud.ObservacionesSolicitud == ""))
                            {
                                response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // Comprobamos el valor del tipo de visita.
                        if (!ValidarTipoVisitaYVisitaIncorrecta(aperturaSolicitud.TipoVisita))
                        {
                            response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorTipovisita);
                            return false;
                        }
                        if (aperturaSolicitud.TipoVisita.ToUpper() == "OTROS")
                        {
                            if (DBNull.Value.Equals(aperturaSolicitud.ObservacionesSolicitud) || (aperturaSolicitud.ObservacionesSolicitud == ""))
                            {
                                response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                                return false;
                            }
                        }
                    }
                }


                ///

                //// Validamos que las observaciones vengan informadas.
                //if (DBNull.Value.Equals(aperturaSolicitud.ObservacionesSolicitud) || (aperturaSolicitud.ObservacionesSolicitud == "") )
                //{
                //    response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorFaltanDatosObligatoriosAperturaSolicitud);
                //    return false;
                //}


                // 25/09/2017 Puestas las comprobaciones de 21, 30 dias y limpieza quemadores.
                if (aperturaSolicitud.SubtipoSolicitud != StringEnum.GetStringValue(Enumerados.SubtipoSolicitud.GasConfort))
                {
                    AperturaSolicitudWSError errorAveriasPuedeAbrir = ComprobarSiPuedeAbrirAverias(aperturaSolicitud);
                    if (errorAveriasPuedeAbrir.ToString() != "0")
                    {
                        response.AddErrorAperturaSolicitud(errorAveriasPuedeAbrir);
                        return false;
                    }
                }

				// Kintell PMG CUR 21/01/2021
				// Si la sociedad del contrato con el que se llama, no es la misma que la del contrato en mantenimiento damos error nuevo.
				if (!ComprobarSociedad(aperturaSolicitud.CodigoContrato))
				{
					response.AddErrorAperturaSolicitud(AperturaSolicitudWSError.ErrorSociedadDiferente);
					return false;
				}
            }
			
            // Si llegamos a este punto hemos superado todas las validaciones y retornamos true.
            return true;
        }

        protected Boolean ComprobarSociedad(string contrato)
        {
            return Contrato.ComprobarSociedad(contrato);
        }

        protected AperturaSolicitudWSError ComprobarSiPuedeAbrirAverias(AperturaSolicitudRequest request)
        {
            IDataReader drFechaVisitaFechaCierreSolicitud = null;
            try
            {
                String strCodContrato = request.CodigoContrato;

                //20210709 ADD BGN R#31331 - Modificar ws de alta de solicitud de la APP para no permitir solicitar averías si existe alguna abierta
                SolicitudesDB dbSol = new SolicitudesDB();
                DataSet datosEFV = dbSol.ObtenerDatosDesdeSentencia("SELECT TE.GASCONFORT from MANTENIMIENTO M INNER JOIN TIPO_EFV TE ON TE.COD_EFV=M.EFV WHERE M.COD_CONTRATO_SIC='" + strCodContrato + "'");
                Boolean esGasConfort = Boolean.Parse(datosEFV.Tables[0].Rows[0].ItemArray[0].ToString());
                //Si para el contrato tiene una solicitud de averia o avería incorrecta ya abierta no dejamos generar otra avería.
                string subTipoAveria = "001";
                string subTipoAveriaIncorrecta = "004";
                if (esGasConfort)
                {
                    subTipoAveria = "012";
                    subTipoAveriaIncorrecta = "013";
                }
                SolicitudDB solDB = new SolicitudDB();
                DataTable dtSolAveriaAbiertas = solDB.ObtenerSolicitudesPorContratoYSubTipoSolicitud(strCodContrato, subTipoAveria, 0, 1);
                if (dtSolAveriaAbiertas.Rows.Count > 0)
                {
                    return AperturaSolicitudWSError.ErrorSolicitudYaAbierta; //"Ya existe una avería abierta"
                }
                DataTable dtSolAveriaIncorrectaAbiertas = solDB.ObtenerSolicitudesPorContratoYSubTipoSolicitud(strCodContrato, subTipoAveriaIncorrecta, 0, 1);
                if (dtSolAveriaIncorrectaAbiertas.Rows.Count > 0)
                {
                    return AperturaSolicitudWSError.ErrorSolicitudYaAbierta; //"Ya existe una avería incorrecta abierta"
                }
                //20210709 ADD END R#31331 - Modificar ws de alta de solicitud de la APP para no permitir solicitar averías si existe alguna abierta

                Int16? intCodVisita = Contrato.ObtenerCodigoUltimaVisita(strCodContrato);

                VisitasDB objVisitasDB = new VisitasDB();
                drFechaVisitaFechaCierreSolicitud = objVisitasDB.ObtenerFechaVisitaFechaCierreSolicitudAveria(request.CodigoContrato, intCodVisita.ToString(), "");

                DateTime fechaVisita = DateTime.Parse("01/01/1900");
                string estadoSolicitud = "";
                DateTime fechaCierreSolicitud = DateTime.Parse("01/01/1900");
                string estadoVisita = "";
                int categoriaVisita = 1;

                while (drFechaVisitaFechaCierreSolicitud.Read())
                {
                    // No limitar para las de GC. (01/03/2016 Mail Alex 12:24).
                    // if (lblServicioInfo.Text.ToUpper().IndexOf("CONFORT") < 0)
                    //{
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


                    //12/02/2016
                    // Si el estado de la solicitud esta en blanco es que no tiene averia anterior por lo que no dejamos abrir avería incorrecta.
                    if (estadoSolicitud == "")
                    {
                        if (request.SubtipoSolicitud == "004")
                        {
                            return AperturaSolicitudWSError.ErrorCodigoSubtipoSolicitud;
                        }
                    }

                    // Si han pasado menos de 21 días desde que se hizo la visita no se permite generar averías ni averías incorrectas.
                    if (fechaVisita.Year.ToString() != "1900")
                    {
                        if (fechaVisita.AddDays(21) > DateTime.Now)
                        {
                            if (estadoVisita == "02" || estadoVisita == "09")
                            {
                                if (categoriaVisita != 1)
                                {
                                    if (request.SubtipoSolicitud == "012")
                                    {
                                        return AperturaSolicitudWSError.ErrorCodigoSubtipoSolicitud;
                                    }
                                }

                                if (request.SubtipoSolicitud == "013")
                                {
                                    return AperturaSolicitudWSError.ErrorCodigoSubtipoSolicitud;
                                }
                            }
                            // Si ademas es es reducida solo dejamos subtipo de TIPO DE averia de limpieza de quemadores.
                            if (categoriaVisita == 1)
                            {
                                if(request.TipoAveria !="31")
                                {
                                    return AperturaSolicitudWSError.ErrorTipoAveria;
                                }
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
                                //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Avería Mantenimiento de Gas"));
                                if (request.SubtipoSolicitud == "001")
                                {
                                    return AperturaSolicitudWSError.ErrorCodigoSubtipoSolicitud;
                                }
                            }
                        }
                    }
                }
                AperturaSolicitudWSError ase = new AperturaSolicitudWSError();
                return ase;
            }
            finally
            {
                if (drFechaVisitaFechaCierreSolicitud != null)
                {
                    drFechaVisitaFechaCierreSolicitud.Close();
                }
            }
        }

        protected bool ValidarSubtipoSolicitud(string subtipo)
        {
            TiposSolicitudDB objTipoSolicitudesDB = new TiposSolicitudDB();
            // Le paso el idioma 1, porque los códigos son los mismos, no ais als descripciones, pero los codigos si, en caso de que los códigos cambien,
            // tendremos que mirar como pasar el pais.
            // PORTUGAL 14/11/2016.
            DataSet dtsSubtipo = objTipoSolicitudesDB.GetSubtipoSolicitudesPorTipo("001", 1);
            for (int i = 0; i < dtsSubtipo.Tables[0].Rows.Count; i++)
            {
                if(dtsSubtipo.Tables[0].Rows[i].ItemArray[0].ToString()==subtipo)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ValidarTelefonoContacto(string telefono)
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
            //Si es nulo o vacio debe de devolver error
            else
            {
                return false;
            }
            //Si pasa todas las validaciones devolvemos un OK.
            return true;
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
            AperturaSolicitudRequest aperturaSolicitud = (AperturaSolicitudRequest)request;
            // Generamos el fichero xnl, y guardamos la ruta del mismo en la BB.DD.
            AperturaSolicitudDB aperturaSolicitudDB = new AperturaSolicitudDB();
            string xml = ToXML(aperturaSolicitud);
            // Guardar la traza y dejar el id de la traza guardada.
            TrazaDB trazaDB = new TrazaDB();
            //MandarMail("ACCESO", "WS PROO_3", "maquintela@gfi.es", true);
            this._idTraza = trazaDB.InsertarYObteneridTrazaAperturaSolicitud("DATOS RECIBIDOS", xml, aperturaSolicitud);

            //20201110 BUA ADD Guardamos toda la request en la tabla TRAZA_REQUEST_XML a cachos
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
            //20201110 BUA END Guardamos toda la request en la tabla TRAZA_REQUEST_XML a cachos
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
                
                // Devolvemos la ruta del fichero generado.
                return xmlDoc.InnerXml;
            }
        }

        protected override void GuardarTrazaResultadoLlamada(WSResponse response)
        {
            AperturaSolicitudResponse AperturaSolicitudResponse = (AperturaSolicitudResponse)response;
            if (this._idTraza.HasValue)
            {
                // Actualizar la traza con el resultado de la operación.
                TrazaDB trazaDB = new TrazaDB();
                if (AperturaSolicitudResponse.TieneError)
                {
                    // Si tiene errores.
                    trazaDB.ActualizarTraza(this._idTraza.ToString(), AperturaSolicitudResponse.ListaErrores[0].Descripcion.ToString());
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

        private string CrearSolitudGCSobreContratoFicticio(string contrato)
        {
            try
            {
                // KINTELL 24/02/2022 R#35436
                //R#35436
                UtilidadesWebServices uw = new UtilidadesWebServices();
                String DNI = "";
                String CP = "";
                String resultadoLlamadaObtenerDatosContratoDelta = uw.llamadaWSobtenerDatosCtoOperaSMG(contrato);
                XmlNode datosXML = uw.StringAXMLGCTemporal(resultadoLlamadaObtenerDatosContratoDelta);
                Mantenimiento mantenimiento = new Mantenimiento();
                MantenimientoDTO mantenimientoDTO = new MantenimientoDTO();

                if (datosXML != null)
                {
                    mantenimientoDTO = mantenimiento.DatosMantenimientoDesdeDeltaYAltaContrato(datosXML, false);
                    contrato = mantenimientoDTO.COD_CONTRATO_SIC;
                    DNI = mantenimientoDTO.DNI;
                    CP = mantenimientoDTO.COD_POSTAL;
                }
                else
                {
                    mantenimientoDTO = mantenimiento.DatosMantenimientoSinPais(contrato);
                    DNI = mantenimientoDTO.DNI;
                    CP = mantenimientoDTO.COD_POSTAL;
                }

                string resultado = uw.llamadaWSobtenerValcred(DNI, CP);
                XmlNode datos = uw.StringAXMLValcred(resultado);
                string valoracion = datos.InnerText;

                SolicitudDB sol = new SolicitudDB();
                Int64 idSolicitud = sol.AltaSolicitudGCContratoFicticio(contrato, valoracion, DNI);

                string mensajeValoracion = Resources.TextosJavaScript.TEXTO_SOLO_PAGO_ANTICIPADO;

                if (valoracion == "SCM09")
                {
                    // Todas las formas de pago
                    mensajeValoracion = Resources.TextosJavaScript.TEXTO_TODOS_PAGOS;
                }
                mensajeValoracion = Resources.TextosJavaScript.TEXTO_SOLICITUD_ALTA + " " + idSolicitud + "  " + mensajeValoracion;

                return mensajeValoracion;
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }

        protected override bool ValidarDatosEntrada(WSRequest request, WSResponse response)
        {
            throw new NotImplementedException();
        }
    }
}