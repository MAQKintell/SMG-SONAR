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
using System.Text;
using System.Web.Mail;


namespace Iberdrola.SMG.WS
{
    public class SigecasWS : WSBase
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
        public SigecasResponse BorrarUsuario(SigecasRequest usuarioSigecas)
        {
            SigecasResponse response = new SigecasResponse();

            try
            {
                //MandarMail("ACCESO", "WS PROO_10", "maquintela@gfi.es", true);
                // Primero guardamos la traza de la llamada.
                //this.GuardarTrazaLlamada(usuarioSigecas);
                // Realizamos la validación del usuario.
                //MandarMail("ACCESO", "WS PROO_11", "maquintela@gfi.es", true);

                Boolean habilitarWS = ValidarwebServiceActivo(Enumerados.Configuracion.HABILITAR_SIGECAS_WS.ToString());

                if (!habilitarWS)
                {
                    response.AddError(CommonWSError.ErrorWSInactivo);
                }
                else
                {
                    if (this.ValidarUsuario(ServiceCredentials.UserName, ServiceCredentials.Password))
                    {
                        //MandarMail("ACCESO", "WS PROO_12", "maquintela@gfi.es", true);
                        // Realizamos la validación de los parámetros de entrada.
                        if (this.ValidarDatosEntrada(usuarioSigecas, response))
                        {
                            this.BorrarUsuarioSigecas(usuarioSigecas, response);
                        }
                    }
                    else
                    {
                        response.AddError(CommonWSError.ErrorCredencialesUsuario);
                    }

                    // Guardamos el resultado en la traza de la llamada.
                    //this.GuardarTrazaResultadoLlamada(response);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en la llamada al SigecasWS.AperturaSolicitud", LogHelper.Category.BussinessLogic, ex);
                response.AddError(CommonWSError.ErrorNoDefinido);
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


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public SigecasResponse ReactivarUsuario(SigecasRequest usuarioSigecas)
        {
            SigecasResponse response = new SigecasResponse();

            try
            {
                //MandarMail("ACCESO", "WS PROO_10", "maquintela@gfi.es", true);
                // Primero guardamos la traza de la llamada.
                //this.GuardarTrazaLlamada(usuarioSigecas);
                // Realizamos la validación del usuario.
                //MandarMail("ACCESO", "WS PROO_11", "maquintela@gfi.es", true);
                if (this.ValidarUsuario(ServiceCredentials.UserName, ServiceCredentials.Password))
                {
                    //MandarMail("ACCESO", "WS PROO_12", "maquintela@gfi.es", true);
                    // Realizamos la validación de los parámetros de entrada.
                    if (this.ValidarDatosEntrada(usuarioSigecas, response))
                    {
                        this.ReactivarUsuarioSigecas(usuarioSigecas, response);
                    }
                }
                else
                {
                    response.AddError(CommonWSError.ErrorCredencialesUsuario);
                }

                // Guardamos el resultado en la traza de la llamada.
                //this.GuardarTrazaResultadoLlamada(response);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en la llamada al SigecasWS.AperturaSolicitud", LogHelper.Category.BussinessLogic, ex);
                response.AddError(CommonWSError.ErrorNoDefinido);
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


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public SigecasResponse CrearUsuario(SigecasRequest usuarioSigecas)
        {
            SigecasResponse response = new SigecasResponse();

            try
            {
                //MandarMail("ACCESO", "WS PROO_10", "maquintela@gfi.es", true);
                // Primero guardamos la traza de la llamada.
                //this.GuardarTrazaLlamada(usuarioSigecas);
                // Realizamos la validación del usuario.
                //MandarMail("ACCESO", "WS PROO_11", "maquintela@gfi.es", true);
                if (this.ValidarUsuario(ServiceCredentials.UserName, ServiceCredentials.Password))
                {
                    //MandarMail("ACCESO", "WS PROO_12", "maquintela@gfi.es", true);
                    // Realizamos la validación de los parámetros de entrada.
                    if (this.ValidarDatosEntradaCrear(usuarioSigecas, response))
                    {
                        this.CrearUsuarioSigecas(usuarioSigecas, response);
                    }
                }
                else
                {
                    response.AddError(CommonWSError.ErrorCredencialesUsuario);
                }

                // Guardamos el resultado en la traza de la llamada.
                //this.GuardarTrazaResultadoLlamada(response);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error en la llamada al SigecasWS.AperturaSolicitud", LogHelper.Category.BussinessLogic, ex);
                response.AddError(CommonWSError.ErrorNoDefinido);
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

        protected override bool ValidarDatosEntrada(WSRequest request, WSResponse response)
        {
            SigecasRequest usuarioSigecas = (SigecasRequest)request;

            // Validamos que tengamos datos de entrada.
            if (usuarioSigecas == null)
            {
                response.AddErrorSigecas(SigecasWSError.ErrorDatosVacios);
                return false;
            }

            // Validamos que la aplicación venga informada
            if ((DBNull.Value.Equals(usuarioSigecas.Aplicacion)) || (usuarioSigecas.Aplicacion.ToString() == ""))
            {
                response.AddErrorSigecas(SigecasWSError.ErrorFaltanDatosObligatoriosSigecas);
                return false;
            }


            // Validamos que el usuario venga informado
            if ((DBNull.Value.Equals(usuarioSigecas.Usuario)) || (usuarioSigecas.Usuario.ToString() == ""))
            {
                response.AddErrorSigecas(SigecasWSError.ErrorFaltanDatosObligatoriosSigecas);
                return false;
            }

            // Si llegamos a este punto hemos superado todas las validaciones y retornamos true.
            return true;
        }


        protected override void GuardarTrazaLlamada(WSRequest request)
        {
        }

        protected override void GuardarTrazaResultadoLlamada(WSResponse response)
        {
        }

        protected bool ValidarDatosEntradaCrear(WSRequest request, WSResponse response)
        {
            SigecasRequest usuarioSigecas = (SigecasRequest)request;

            bool validado = ValidarDatosEntrada(request, response);
            if (!validado)
            {
                return false;
            }

            // Validamos que el Email venga informado
            if ((DBNull.Value.Equals(usuarioSigecas.Email)) || (usuarioSigecas.Email.ToString() == ""))
            {
                response.AddErrorSigecas(SigecasWSError.ErrorFaltanDatosObligatoriosSigecas);
                return false;
            }


            // Validamos que el perfil venga informado
            if ((DBNull.Value.Equals(usuarioSigecas.Id_Perfil)) || (usuarioSigecas.Id_Perfil.ToString() == ""))
            {
                response.AddErrorSigecas(SigecasWSError.ErrorFaltanDatosObligatoriosSigecas);
                return false;
            }

            // Validamos que el nombre venga informado
            if ((DBNull.Value.Equals(usuarioSigecas.Nombre)) || (usuarioSigecas.Nombre.ToString() == ""))
            {
                response.AddErrorSigecas(SigecasWSError.ErrorFaltanDatosObligatoriosSigecas);
                return false;
            }

            // Si llegamos a este punto hemos superado todas las validaciones y retornamos true.
            return true;
        }

        protected void BorrarUsuarioSigecas(SigecasRequest usuarioSigecas, SigecasResponse response)
        {
            try
            {
                this.Baja(usuarioSigecas.Usuario, "SigecasWS", usuarioSigecas.Aplicacion);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Erroral borrar el usuario WS:" + ex.Message, LogHelper.Category.BussinessLogic);
                response.AddErrorSigecas(SigecasWSError.ErrorBorrarUsuario);
            }
        }

        protected void ReactivarUsuarioSigecas(SigecasRequest usuarioSigecas, SigecasResponse response)
        {
            try
            {
                this.Reactivar(usuarioSigecas.Usuario, "SigecasWS", usuarioSigecas.Aplicacion);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Erroral borrar el usuario WS:" + ex.Message, LogHelper.Category.BussinessLogic);
                response.AddErrorSigecas(SigecasWSError.ErrorBorrarUsuario);
            }
        }

        protected void CrearUsuarioSigecas(SigecasRequest usuarioSigecas, SigecasResponse response)
        {
            try
            {
                UsuarioSigecasDTO usuario = new UsuarioSigecasDTO();
                usuario.Email = usuarioSigecas.Email;
                usuario.Id_Perfil = usuarioSigecas.Id_Perfil;
                usuario.Login = usuarioSigecas.Usuario;
                usuario.Nombre = usuarioSigecas.Nombre;
                usuario.Responsable = usuarioSigecas.Responsable;

                this.Alta(usuarioSigecas.Aplicacion, usuario);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Erroral borrar el usuario WS:" + ex.Message, LogHelper.Category.BussinessLogic);
                response.AddErrorSigecas(SigecasWSError.ErrorBorrarUsuario);
            }
        }

        //protected override void GuardarTrazaLlamada(WSRequest request)
        //{
        //    AperturaSolicitudRequest aperturaSolicitud = (AperturaSolicitudRequest)request;
        //    // Generamos el fichero xnl, y guardamos la ruta del mismo en la BB.DD.
        //    AperturaSolicitudDB aperturaSolicitudDB = new AperturaSolicitudDB();
        //    string Proveedor = aperturaSolicitudDB.ObtenerProveedorAveriaPorContrato(aperturaSolicitud.CodigoContrato);
        //    string xml = ToXML(aperturaSolicitud, Proveedor);
        //    //MandarMail("ACCESO", "WS PROO_2", "maquintela@gfi.es", true);
        //    // Guardar la traza y dejar el id de la traza guardada.
        //    TrazaDB trazaDB = new TrazaDB();
        //    //MandarMail("ACCESO", "WS PROO_3", "maquintela@gfi.es", true);
        //    this._idTraza = trazaDB.InsertarYObteneridTrazaAperturaSolicitud("DATOS RECIBIDOS", xml, aperturaSolicitud);
        //}

        //public string ToXML(Object oObject, string proveedor)
        //{
        //    //MandarMail("ACCESO", "WS PROO_4444", "maquintela@gfi.es", true);
        //    XmlDocument xmlDoc = new XmlDocument();
        //    //MandarMail("ACCESO", "WS PROO_44441", "maquintela@gfi.es", true);
        //    XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
        //    //MandarMail("ACCESO", "WS PROO_44442", "maquintela@gfi.es", true);
        //    using (MemoryStream xmlStream = new MemoryStream())
        //    {
        //        //MandarMail("ACCESO", "WS PROO_44443", "maquintela@gfi.es", true);
        //        xmlSerializer.Serialize(xmlStream, oObject);
        //        xmlStream.Position = 0;
        //        xmlDoc.Load(xmlStream);
        //        //MandarMail("ACCESO", "WS PROO_44444", "maquintela@gfi.es", true);
        //        ////String Destino =Server.MapPath(ConfigurationManager.AppSettings.Get("rutaFicheroXMLWS"));
        //        //String Destino = ConfigurationManager.AppSettings.Get("rutaFicheroXMLWS");
        //        //// Creamos un fichero xml con los datos enviados por el proveedor.
        //        ////xmlDoc.Save(Destino + "_" + proveedor + "_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xml");
        //        //MandarMail("ACCESO", "WS PROO_44445", "maquintela@gfi.es", true);
        //        String Destino = ConfigurationManager.AppSettings.Get("rutaFicheroXMLWS");
        //        //MandarMail("ACCESO", "WS PROO_44446", "maquintela@gfi.es", true);
        //        Destino = Destino + "_" + proveedor + "_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xml";
        //        //MandarMail("ACCESO", "WS PROO_44447", "maquintela@gfi.es", true);
        //        // Creamos un fichero xml con los datos enviados por el proveedor.
        //        //xmlDoc.Save(Destino);
        //        // GENERAMOS EL FICHERO.
        //        using (ManejadorFicheros.GetImpersonator())
        //        {
        //            //MandarMail("ACCESO", "WS PROO_44448", "maquintela@gfi.es", true);
        //            //xmlDoc.Save(Destino);
        //            //File.WriteAllBytes(rutaFichero, cierreVisita.ContenidoFichero);
        //        }

        //        // Devolvemos la ruta del fichero generado.
        //        return Destino;// +"_" + proveedor + "_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xml";// xmlDoc.InnerXml;
        //    }
        //}

        //protected override void GuardarTrazaResultadoLlamada(WSResponse response)
        //{
        //    AperturaSolicitudResponse AperturaSolicitudResponse = (AperturaSolicitudResponse)response;
        //    if (this._idTraza.HasValue)
        //    {
        //        // Actualizar la traza con el resultado de la operación.
        //        TrazaDB trazaDB = new TrazaDB();
        //        if (AperturaSolicitudResponse.TieneError)
        //        {
        //            // Si tiene errores.
        //            trazaDB.ActualizarTraza(this._idTraza.ToString(), AperturaSolicitudResponse.ListaErrores[0].Descripcion.ToString());
        //        }
        //        else
        //        {
        //            // Si todo ha ido bien.
        //            trazaDB.ActualizarTraza(this._idTraza.ToString(), "TODO CORRECTO");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Error al guardar el resultado de la traza. No se ha encontrato el identificativo de la traza de la llamada al WS.");
        //    }
        //}


        private void Alta(string APP, UsuarioSigecasDTO usuario)
        {
            try
            {
                //ParametrosConfiguracion.Default.EntornoSigecas = "Produccion";
                //ParametrosConfiguracion.Default.Save();

                string[] aplicacionesConLDAP = new string[3];
                aplicacionesConLDAP[0] = "Valcred";
                aplicacionesConLDAP[1] = "Banticip";
                aplicacionesConLDAP[2] = "BSOC";

                bool ldap = false;
                foreach (string sApp in aplicacionesConLDAP)
                {
                    if (sApp.ToUpper() == APP.ToUpper())
                    {
                        ldap = true;
                        break;
                    }
                }

                string pass = APP.ToUpper();
                // Ver si va encriptada o no.
                string sql = "SELECT [VAL] FROM [P_CONFIG] WHERE [COD] = 'PASSWORDS_ENCRIPTADAS'";
                if (APP == "Cerener" || APP == "GesSolar")
                {
                    sql = "SELECT [VAL] FROM [P_CONFIG] WHERE [COD_config] = 'PASSWORD_ENCRIPTADAS'";
                }

                DataTable dtEncriptado = SigecasDB.DevolverQuery(APP, sql);
                Boolean encriptar = false;
                for (int j = 0; j < dtEncriptado.Rows.Count; j++)
                {
                    encriptar = Boolean.Parse(dtEncriptado.Rows[j].ItemArray[0].ToString());
                }

                pass = EncryptHelper.Encrypt(pass);

                //AGP	spCommonsUsuarioInsert	spCommonsUsuarioDelete	spCommonsUsuarioUpdatePassword
                //BSOC	SP_ANIADIR_USUARIO	SP_ELIMINAR_USUARIO	SP_MODIFICAR_USUARIO
                //CERENER	spCommonsUsuarioInsert	spCommonsUsuarioDelete	spCommonsUsuarioUpdatePassword
                //GESSOLARspCommonsUsuarioInsert	spCommonsUsuarioDelete	spCommonsUsuarioUpdatePassword
                //RECAMPA	spCommonsUsuarioInsert	spCommonsUsuarioDelete	spCommonsUsuarioUpdatePassword
                //VALCRED	spCommonsUsuarioInsert	spCommonsUsuarioDelete	spCommonsUsuarioUpdatePassword
                // Aplicaciones con LDAP

                // Ejemplo Lanzamiento PROC.
                //Int64 res = GesvulDB.RunProcEscalar("[spComporbacionesDiariasGesVulsolicitudesAnuladas]");
                // Ejemplo lanzamiento Query.
                //string sql = "DELETE FROM [gesvul].dbo.MOTIVO WHERE ID_TIPO_ACCION = 8 AND ID_ESTADO = 3 AND FEC_ALTA_CND_ESP> GETDATE()";
                //GesvulDB.EjecutarQuery(sql);


                //AGP, BSOC, GESSOLAR, CERENER
                if (!String.IsNullOrEmpty(usuario.Email))
                {
                    SigecasDB.CrearUsuario(APP, usuario, pass);

                    if (ldap == false)
                    {
                        EnviarMailUsuarioRegistrado(APP, APP.ToUpper(), usuario.Login, usuario.Email);
                    }
                    EscribirLOGSigecas("Crear Usuario OK");
                }


            }
            catch (Exception ex)
            {
                EscribirLOGSigecas("ERROR ------ " + ex.Message.ToString());
                EscribirLOGSigecas("ERROR ------ " + ex.TargetSite.ToString());
                EscribirLOGSigecas("ERROR ------ " + ex.StackTrace.ToString());
            }

        }


        private void Baja(string APP, string Login, string Responsable)
        {
            try
            {
                //ParametrosConfiguracion.Default.EntornoSigecas = "Produccion";
                //ParametrosConfiguracion.Default.Save();


                //Valcred
                //Banticip
                //BSOC
                //Recampa
                //Cerener
                //GesSolar
                //AGP

                if (APP == "Recampa")
                {
                    // Comprobamos si tiene lotes.
                    if (SigecasDB.ObtenerNumLotesPendientesRecampa(Login).Equals(0))
                    {
                        SigecasDB.EliminarUsuario(Login, Responsable, APP);
                    }
                    else
                    {
                        // MANDAMOS MAIL DE QUE TIENE LOTES ASIGNADOS.
                        string strTexto = "Se trata del último usuario de una plataforma para la que hay lotes pendientes de cerrar. Los tendríais que cerrar o indicar a que otra plataforma asignarlos antes de solicitar la baja del usuario.";

                    }
                }
                else
                {
                    SigecasDB.EliminarUsuario(Login, Responsable, APP);
                }
                // Ejemplo Lanzamiento PROC.
                //Int64 res = GesvulDB.RunProcEscalar("[spComporbacionesDiariasGesVulsolicitudesAnuladas]");
                // Ejemplo lanzamiento Query.
                //string sql = "DELETE FROM [gesvul].dbo.MOTIVO WHERE ID_TIPO_ACCION = 8 AND ID_ESTADO = 3 AND FEC_ALTA_CND_ESP> GETDATE()";
                //GesvulDB.EjecutarQuery(sql);

            }
            catch (Exception ex)
            {
                EscribirLOGSigecas("ERROR ------ " + ex.Message.ToString());
                EscribirLOGSigecas("ERROR ------ " + ex.TargetSite.ToString());
                EscribirLOGSigecas("ERROR ------ " + ex.StackTrace.ToString());
            }

        }

        private void Reactivar(string APP, string Login, string Responsable)
        {
            try
            {
                //ParametrosConfiguracion.Default.EntornoSigecas = "Produccion";
                //ParametrosConfiguracion.Default.Save();

                string[] aplicacionesConLDAP = new string[3];
                aplicacionesConLDAP[0] = "Valcred";
                aplicacionesConLDAP[1] = "Banticip";
                aplicacionesConLDAP[2] = "BSOC";
                //Recampa
                //Cerener
                //GesSolar
                //AGP

                //for (int i = 0; i < aplicacionesConLDAP.Count(); i++)
                //{
                //    if (aplicacionesConLDAP[i] == APP)
                //    {
                //        // No hacemos nada, al ir por LDAP
                //    }
                //}
                string pass = APP.ToUpper();
                // Ver si va encriptada o no.
                string sql = "SELECT [VAL] FROM [P_CONFIG] WHERE [COD] = 'PASSWORDS_ENCRIPTADAS'";
                if (APP == "Cerener" || APP == "GesSolar")
                {
                    sql = "SELECT [VAL] FROM [P_CONFIG] WHERE [COD_config] = 'PASSWORD_ENCRIPTADAS'";
                }

                DataTable dtEncriptado = SigecasDB.DevolverQuery(APP, sql);
                Boolean encriptar = false;
                for (int j = 0; j < dtEncriptado.Rows.Count; j++)
                {
                    encriptar = Boolean.Parse(dtEncriptado.Rows[j].ItemArray[0].ToString());
                }

                pass = EncryptHelper.Encrypt(pass);

                sql = "SELECT EMAIL FROM USUARIO WHERE LOGIN='" + Login + "'";
                DataTable dtEmail = SigecasDB.DevolverQuery(APP, sql);
                string email = "";
                for (int k = 0; k < dtEmail.Rows.Count; k++)
                {
                    email = dtEmail.Rows[k].ItemArray[0].ToString();
                }
                Boolean enviarMail = true;
                for (int k = 0; k < aplicacionesConLDAP.Length; k++)
                {
                    if (aplicacionesConLDAP[k].ToUpper() == APP)
                    {
                        enviarMail = false;
                    }
                }
                // Comprobamos si es necesario enviar mail.
                if (!enviarMail)
                {
                    // Si es necesario, comprobamos que tenemos mail a enviar.
                    if (email != "")
                    {
                        SigecasDB.CambiarPassword(Login, pass, Responsable, APP);

                        EnviarMailPasswordReseteada(APP, APP.ToUpper(), Login, email);

                    }
                    else
                    {
                        // Falta mail del usuario.
                    }

                }
                else
                {
                    // si no es necesario cambiamos password pero no enviamos mail.
                    SigecasDB.CambiarPassword(Login, pass, Responsable, APP);
                }


                // Ejemplo Lanzamiento PROC.
                //Int64 res = GesvulDB.RunProcEscalar("[spComporbacionesDiariasGesVulsolicitudesAnuladas]");
                // Ejemplo lanzamiento Query.
                //string sql = "DELETE FROM [gesvul].dbo.MOTIVO WHERE ID_TIPO_ACCION = 8 AND ID_ESTADO = 3 AND FEC_ALTA_CND_ESP> GETDATE()";
                //GesvulDB.EjecutarQuery(sql);

            }
            catch (Exception ex)
            {
                EscribirLOGSigecas("ERROR ------ " + ex.Message.ToString());
                EscribirLOGSigecas("ERROR ------ " + ex.TargetSite.ToString());
                EscribirLOGSigecas("ERROR ------ " + ex.StackTrace.ToString());
            }

        }

        public static void EnviarMailPasswordReseteada(string APP, string contraseniaEnviar, string Login, string correo)
        {
            // montamos el cuerpo del mail
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append("<br><br>");
            strBuilder.Append("Se ha reseteado la contraseña del usuario en la aplicación.");

            strBuilder.Append("<br><br>");
            strBuilder.Append("Login: <b>" + Login + "</b><br>");

            strBuilder.Append("Password: <b>" + contraseniaEnviar + "</b><br>");

            System.Web.Mail.MailMessage objMail = new System.Web.Mail.MailMessage();
            objMail.From = "PROCO-Departamental@iberdrola.es";//rlopezh@iberdrola.es";
            objMail.To = correo;
            objMail.Subject = APP + ": Cambio de contraseña";
            objMail.BodyFormat = System.Web.Mail.MailFormat.Html;
            objMail.Body = strBuilder.ToString();


            System.Web.Mail.SmtpMail.SmtpServer = "smtpcluster1.iberdrola.es";
            try
            {
                System.Web.Mail.SmtpMail.Send(objMail);
            }
            catch (Exception ex)
            {
                //EscribirLog("11");
                objMail = new System.Web.Mail.MailMessage();
                objMail.From = "rlopezh@iberdrola.es";
                objMail.To = correo;
                objMail.Subject = APP + ": Cambio de contraseña";
                objMail.BodyFormat = System.Web.Mail.MailFormat.Html;
                objMail.Body = strBuilder.ToString();

                System.Web.Mail.SmtpMail.SmtpServer = "smtpcluster1.iberdrola.es";
                //System.Web.Mail.SmtpMail.SmtpServer = "smtpcluster2.iberdrola.es";
                try
                {
                    System.Web.Mail.SmtpMail.Send(objMail);
                }
                catch (Exception exx)
                {
                    //EscribirLog("11");

                }
                ex = ex.GetBaseException();
            }
        }

        public static void EnviarMailUsuarioRegistrado(string APP, string contraseniaEnviar, string Login, string correo)
        {
            // montamos el cuerpo del mail
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append("<br><br>");
            strBuilder.Append("Se ha registrado el siguiente usuario en la aplicación.");

            strBuilder.Append("<br><br>");
            strBuilder.Append("Login: <b>" + Login + "</b><br>");
            strBuilder.Append("Password: <b>" + contraseniaEnviar + "</b><br>");

            System.Web.Mail.MailMessage objMail = new System.Web.Mail.MailMessage();
            objMail.From = "PROCO-Departamental@iberdrola.es";//rlopezh@iberdrola.es";
            objMail.To = correo;
            objMail.Subject = APP + ": Usuario Registrado";
            objMail.BodyFormat = System.Web.Mail.MailFormat.Html;
            objMail.Body = strBuilder.ToString();


            System.Web.Mail.SmtpMail.SmtpServer = "smtpcluster1.iberdrola.es";
            try
            {
                System.Web.Mail.SmtpMail.Send(objMail);
            }
            catch (Exception ex)
            {
                //EscribirLog("11");
                objMail = new System.Web.Mail.MailMessage();
                objMail.From = "rlopezh@iberdrola.es";
                objMail.To = correo;
                objMail.Subject = APP + ": Usuario Registrado";
                objMail.BodyFormat = System.Web.Mail.MailFormat.Html;
                objMail.Body = strBuilder.ToString();

                System.Web.Mail.SmtpMail.SmtpServer = "smtpcluster1.iberdrola.es";
                //System.Web.Mail.SmtpMail.SmtpServer = "smtpcluster2.iberdrola.es";
                try
                {
                    System.Web.Mail.SmtpMail.Send(objMail);
                }
                catch (Exception exx)
                {
                    //EscribirLog("11");

                }
                ex = ex.GetBaseException();
            }
        }

        private static void EscribirLOGSigecas(String linea)
        {
            StreamWriter sw = new StreamWriter("LogSIGECAS" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
            //StreamWriter sw = new StreamWriter(DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
            sw.WriteLine(DateTime.Now.ToString() + " " + linea);
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strAsunto">Asunto del correo</param>
        /// <param name="strTexto">Cuerpo del correo</param>
        public static void EnviarMailSigecas(string strAsunto, string strTexto)
        {
            bool EnvioMailSigecas = false;
            if (EnvioMailSigecas)
            {
                string strEmailDestino = "";
                //Se prepara un nuevo mensaje
                System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();

                //Se establece el destinatario
                msg.To = strEmailDestino;

                //Se establece el remitente, asi como el nombre que aparecerá en la 
                //bandeja de entrada, así como el formato de codificación
                msg.From = "ADMINISTRACION SIGECAS";

                //Se establece el asunto del mail
                msg.Subject = strAsunto;

                //Formato de codificación del strAsunto
                //msg.SubjectEncoding = System.Text.Encoding.UTF8;

                //Se establece el cuerpo del mail
                msg.Body = strTexto;

                //Se establece la codificación del Cuerpo
                msg.BodyEncoding = System.Text.Encoding.UTF8;//.Unicode;
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", "smtp.gmail.com");
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", "465");
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", true);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout", "10");
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "smggfi1@gmail.com");
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "iberdrola");
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", true);

                //Se prepara el envio del mail
                SmtpMail.SmtpServer = "smtp.gmail.com";

                try
                {
                    SmtpMail.Send(msg);
                }
                catch (Exception e)
                {
                    System.Web.Mail.MailMessage objMail = new System.Web.Mail.MailMessage();
                    objMail.From = "rlopezh@iberdrola.es";
                    objMail.To = strEmailDestino;
                    objMail.Subject = strAsunto;
                    objMail.BodyFormat = System.Web.Mail.MailFormat.Html;
                    objMail.Body = strTexto;

                    System.Web.Mail.SmtpMail.SmtpServer = "smtpcluster1.iberdrola.es";
                    //System.Web.Mail.SmtpMail.SmtpServer = "smtpcluster2.iberdrola.es";
                    System.Web.Mail.SmtpMail.Send(objMail);

                    e = e.GetBaseException();
                }
            }
        }

        public static DataTable ObtenerPerfilesAplicaciones(string app)
        {
            return SigecasDB.ObtenerPerfiles(app);
        }

    }
}