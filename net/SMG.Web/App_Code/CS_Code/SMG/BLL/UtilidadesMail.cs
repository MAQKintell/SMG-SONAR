using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Iberdrola.Commons.Services.MailServices;
using System.Text;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Logging;
using System.IO;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de UtilidadesMail
    /// </summary>
    public class UtilidadesMail
    {
        public UtilidadesMail()
        {
        }

        public class Adjunto
        {
            public string NombreFichero { get; set; }
            public Object Fichero { get; set; }

            public Adjunto(string NombreFichero, Object Fichero)
            {
                this.NombreFichero = NombreFichero;
                this.Fichero = Fichero;
            }
        }

        /// <summary>
        /// Crea el gestor del mensaje y le asigna el inicio del asunto
        /// </summary>
        /// <returns>Gestor del mensaje inicializado</returns>
        public static MailHelper CrearMensajeBasico()
        {
            MailHelper mail = new MailHelper();
            ConfiguracionDTO confAsunto = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.MAIL_INICIO_ASUNTO);
            if (confAsunto != null && !string.IsNullOrEmpty(confAsunto.Valor))
            {
                mail.Subject = confAsunto.Valor;
            }
            return mail;
        }

		public static bool EnviarMensajeErrorLlamadaWS(string strNombreWS)
        {
            bool mensajeEnviado = false;

            try
            {
                // creamos el mail
                MailHelper mail = CrearMensajeBasico();

                //establecemos en subject
                mail.Subject += ": Error Llamada WS";

                // montamos el cuerpo del mail
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.Append("<p>");
                strBuilder.Append("Se ha producido un error al llamar al WS: " + strNombreWS);
                strBuilder.Append("</p>");

                mail.Body = strBuilder.ToString();

                mail.To.Add(ObtenerEmailsResponsablesServicio());

                mail.SendMail();
                // Actualizamos la variable de enviada a true
                mensajeEnviado = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ERROR al EnviarMensajeErrorLlamadaWS " + strNombreWS + ". ERROR: " + ex.Message, LogHelper.Category.BussinessLogic);
            }

            return mensajeEnviado;
        }

        private static string ObtenerEmailsResponsablesServicio()
        {
            ConfiguracionDTO confDireccion = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CUENTA_CORREO_ADMINISTRADOR_SERVICIO);
            if (confDireccion != null && !string.IsNullOrEmpty(confDireccion.Valor))
            {
                return confDireccion.Valor;
            }
            else
            {
                return null;
            }
        }

        //20200123 BGN ADD BEG [R#21821]: Parametrizar envio mails por la web
        public static bool EnviarMensajeError(string asunto, string mensaje)
        {
            bool mensajeEnviado = false;

            try
            {
                // creamos el mail
                MailHelper mail = CrearMensajeBasico();

                mail.Subject += asunto;

                mail.Body = mensaje;

                mail.To.Add(ObtenerEmailsResponsablesServicio());
                
                ConfiguracionDTO confFrom = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CUENTA_CORREO_ERRORES_FROM);
                if (confFrom != null && !string.IsNullOrEmpty(confFrom.Valor))
                {
                    mail.SendMail(confFrom.Valor);

                    // Actualizamos la variable de enviada a true
                    mensajeEnviado = true;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("ERROR al EnviarMensajeError. ERROR: " + ex.Message, LogHelper.Category.BussinessLogic);
            }

            return mensajeEnviado;
        }

        public static bool EnviarAviso(string asunto, string mensaje, string from, string to)
        {
            bool mensajeEnviado = false;

            try
            {
                // creamos el mail
                MailHelper mail = CrearMensajeBasico();

                mail.Subject = asunto;

                mail.Body = mensaje;

                mail.To.Add(to);

                if (from != null && !String.IsNullOrEmpty(from))
                {
                    mail.SendMail(from);
                }
                else
                {
                    mail.SendMail();
                }

                // Actualizamos la variable de enviada a true
                mensajeEnviado = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ERROR al EnviarAviso. ERROR: " + ex.Message, LogHelper.Category.BussinessLogic);
            }

            return mensajeEnviado;
        }
        //20200123 BGN ADD BEG [R#21821]: Parametrizar envio mails por la web
        
        public static bool EnviarAvisoConAdjunto(string asunto, string mensaje, string from, string to, List<Adjunto> lAdjuntos)
        {
            bool mensajeEnviado = false;

            try
            {
                // creamos el mail
                MailHelper mail = CrearMensajeBasico();

                mail.Subject = asunto;

                mail.Body = mensaje;

                mail.To.Add(to);

                if (lAdjuntos != null && lAdjuntos.Count > 0)
                {
                    foreach (Adjunto adjunto in lAdjuntos)
                    {
                        string nombreFichero = adjunto.NombreFichero;
                        object objAdjunto = adjunto.Fichero;
                        string strTypeObjeto = objAdjunto.GetType().Name;

                        switch (strTypeObjeto)
                        {
                            case "MemoryStream":
                                mail.AddAttachmentStream((Stream)objAdjunto, nombreFichero); //Insertamos el fichero
                                break;
                            case "String":
                                mail.AddAttachment((String)objAdjunto); //Se rcoge el fichero de ruta + nombre fichero
                                break;
                                //default:
                                //    resultDescarga = "Descarga OK.";
                                //    bErrorDescargas = false;
                                //    break;
                        }
                    }
                }

                if (from != null && !String.IsNullOrEmpty(from))
                    mail.SendMail(from);
                else
                    mail.SendMail();

                // Actualizamos la variable de enviada a true
                mensajeEnviado = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ERROR al EnviarAviso. ERROR: " + ex.Message, LogHelper.Category.BussinessLogic);
            }

            return mensajeEnviado;
        }

        //20210923 BGN BEG R#33847 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        public static bool EnviarMailConAdjunto(string asunto, string mensaje, string to, string rutaAdjunto)
        {
            bool mensajeEnviado = false;

            try
            {
                // creamos el mail
                MailHelper mail = CrearMensajeBasico();

                mail.Subject += asunto;

                // montamos el cuerpo del mail
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.Append("<p>");
                strBuilder.Append(mensaje);
                strBuilder.Append("</p>");

                mail.Body = strBuilder.ToString();

                mail.To.Add(to);

                mail.AddAttachment(rutaAdjunto);

                mail.SendMail();

                // Actualizamos la variable de enviada a true
                mensajeEnviado = true;

            }
            catch (Exception ex)
            {
                LogHelper.Error("ERROR al EnviarMailConAdjunto. ERROR: " + ex.Message, LogHelper.Category.BussinessLogic);
            }

            return mensajeEnviado;
        }
    }
    //20210923 BGN END R#33847 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental

}