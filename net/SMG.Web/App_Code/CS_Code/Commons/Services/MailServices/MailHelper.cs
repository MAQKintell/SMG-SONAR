//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Logging;
using Iberdrola.Commons.Messages;
using System.Security.Cryptography.X509Certificates;
using Iberdrola.Commons.Services.MailServices.SecureMail;
using System.IO;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.Commons.Services.MailServices
{
    /// <summary>
    /// Descripción breve de MailHelper.
    /// </summary>
    public class MailHelper : MailMessage
    {
        /// <summary>
        /// Constante para acceder al web.config para obtener la url del servidor proxy.
        /// </summary>
        private const string MAIL_urlproxyserver = "MAIL_urlproxyserver";

        /// <summary>
        /// Constante para acceder al web.config para obtener el puerto del servidor proxy.
        /// </summary>
        private const string MAIL_proxyserverport = "MAIL_proxyserverport";

        /// <summary>
        /// Constante para acceder al web.config para obtener la url del servidor SMTP.
        /// </summary>
        private const string MAIL_smtpserver = "MAIL_smtpserver";

        /// <summary>
        /// Constante para acceder al web.config para obtener el parámetro sendusing.
        /// </summary>
        private const string MAIL_sendusing = "MAIL_sendusing";

        /// <summary>
        /// Constante para acceder al web.config para obtener el puerto del servidor SMTP.
        /// </summary>
        private const string MAIL_smtpserverport = "MAIL_smtpserverport";

        /// <summary>
        /// Constante para acceder al web.config para obtener si se usa SSL o no.
        /// </summary>
        private const string MAIL_smtpusessl = "MAIL_smtpusessl";

        /// <summary>
        /// Constante para acceder al web.config para obtener el indicador de 
        /// si hay que hacer autenticación con el servidor smtp.
        /// </summary>
        private const string MAIL_smtpauthenticate = "MAIL_smtpauthenticate";

        /// <summary>
        /// Constante para acceder al web.config para obtener el nombre del usuario 
        /// que envía el email.
        /// </summary>
        private const string MAIL_sendusername = "MAIL_sendusername";

        /// <summary>
        /// Constante para acceder al web.config para obtener la password 
        /// de la dirección de correo desde la que se envía el email.
        /// </summary>
        private const string MAIL_sendpassword = "MAIL_sendpassword";


        /// <summary>
        /// Constante para acceder al web.config para obtener la dirección 
        /// de correo desde la que se envía el email.
        /// </summary>
        private const string MAIL_senderaccount = "MAIL_senderaccount";

        /// <summary>
        /// Variable que indica si se está en la oficina (true) o si se está en los servidores
        /// corporativos de Iberdrola (false) influye para la forma de realizar la llamada.
        /// </summary>
        private const string MAIL_OFFICE = "MAIL_OFFICE";

        /// <summary>
        /// Lista de receptores del email.
        /// </summary>
        private List<string> _recipients = new List<string>();

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public MailHelper()
        {
            // establecemos la configuración por defecto.
            this.IsBodyHtml = true;
        }

        /// <summary>
        /// Propiedad para acceder a _recipients.
        /// </summary>
        public List<string> Recipients
        {
            get { return this._recipients; }
            set { this._recipients = value; }
        }

        /// <summary>
        /// Añade un archivo adjunto al email.
        /// </summary>
        /// <param name="strFullFilePath">Ruta completa del fichero a anexar.</param>
        public void AddAttachment(string strFullFilePath)
        {
            Attachment data = new Attachment(strFullFilePath);

            // ContentDisposition disposition = data.ContentDisposition;
            // disposition.CreationDate = System.IO.File.GetCreationTime(strFullFilePath);
            // disposition.ModificationDate = System.IO.File.GetLastWriteTime(strFullFilePath);
            // disposition.ReadDate = System.IO.File.GetLastAccessTime(strFullFilePath);            
            this.Attachments.Add(data);
        }

        /// <summary>
        /// Añade un archivo adjunto al email.
        /// </summary>
        /// <param name="strFullFilePath">Ruta completa del fichero a anexar.</param>
        public void AddAttachmentStream(Stream pdfStream, string nombreFichero)
        {
            Attachment data = new Attachment(pdfStream, nombreFichero);

            // ContentDisposition disposition = data.ContentDisposition;
            // disposition.CreationDate = System.IO.File.GetCreationTime(strFullFilePath);
            // disposition.ModificationDate = System.IO.File.GetLastWriteTime(strFullFilePath);
            // disposition.ReadDate = System.IO.File.GetLastAccessTime(strFullFilePath);            
            this.Attachments.Add(data);
        }



        /// <summary>
        /// Envía el email.
        /// </summary>
        public void SendMail()
        {
            try
            {
                this.SendMail(ConfigurationManager.AppSettings[MAIL_senderaccount]);
            }
            catch (Exception ex)
            {
                // Registramos el error producido.
                LogHelper.Error("Error el enviar el correo: " + ex.Message, LogHelper.Category.Architecture);
                LogHelper.Error("Error el enviar el correo: " + ex.StackTrace, LogHelper.Category.Architecture);

                // Lanzamos la excepción para que el llamador sepa que no se ha mandado el correo.
                throw new ArqException(true, ex, MessagesManager.CommonErrorMessages.ErrorEnviarEmail.GetHashCode().ToString());
            }
        }

        /// <summary>
        /// Envía el email.
        /// </summary>
        /// <param name="host">Servidor que envía los mensajes</param>
        public void SendMail(string address)
        {
            string host = string.Empty;
            int port = 25;
            bool enableSsl = false;
            System.Net.ICredentialsByHost smtpCredentials = null;
            string smtpUserName = string.Empty;
            string smtpPassword = string.Empty;

            try
            {
                this.BuildBasicMail(address);

                // Host
                host = ConfigurationManager.AppSettings[MAIL_smtpserver];

                // Port
                string szPort = ConfigurationManager.AppSettings[MAIL_smtpserverport];
                if ((!string.IsNullOrEmpty(szPort)) && (!string.IsNullOrEmpty(szPort.Trim())))
                {
                    if (!int.TryParse(szPort, out port))
                    {
                        port = 25;
                    }
                }

                // Use SSL
                string szEnableSsl = ConfigurationManager.AppSettings[MAIL_smtpusessl];
                if ((!string.IsNullOrEmpty(szEnableSsl)) && (!string.IsNullOrEmpty(szEnableSsl.Trim())))
                {
                    if (!bool.TryParse(szEnableSsl, out enableSsl))
                    {
                        enableSsl = false;
                    }
                }

                // Credentials
                smtpUserName = ConfigurationManager.AppSettings[MAIL_sendusername];
                if (string.IsNullOrEmpty(smtpUserName))
                {
                    smtpUserName = string.Empty;
                }
                else
                {
                    smtpUserName = smtpUserName.Trim();
                }

                if (!string.IsNullOrEmpty(smtpUserName))
                {
                    smtpPassword = ConfigurationManager.AppSettings[MAIL_sendpassword];
                    if (string.IsNullOrEmpty(smtpPassword))
                    {
                        smtpPassword = string.Empty;
                    }
                    else
                    {
                        smtpPassword = smtpPassword.Trim();
                    }

                    smtpCredentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
                }

                SmtpClient smtp = new SmtpClient(host, port)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = enableSsl,
                };

                if (smtpCredentials != null)
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = smtpCredentials;
                }

                smtp.Send(this);
            }
            catch (Exception ex)
            {
                // Registramos el error producido.
                LogHelper.Error("Error el enviar el correo: " + ex.Message, LogHelper.Category.Architecture);
                LogHelper.Error("Error el enviar el correo: " + ex.StackTrace, LogHelper.Category.Architecture);

                // Lanzamos la excepción para que el llamador sepa que no se ha mandado el correo.
                throw new ArqException(true, ex, MessagesManager.CommonErrorMessages.ErrorEnviarEmail.GetHashCode().ToString());
            }
        }        

        private void BuildBasicMail(string address)
        {
            this.From = new MailAddress(address);

            foreach (string strRecipient in this._recipients)
            {
                //this.Recipients.Add(strRecipient);
                this.CC.Add(strRecipient);
            }
        }
        
        /*
        /// <summary>
        /// Envía el email certificado.
        /// </summary>
        public void SendCertMail()
        {
            try
            {
                var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                var certificates = store.Certificates;

                string serialCertEncrypt = string.Empty;
                ConfiguracionDTO confSerialCertEncrypt = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CONF_SERIAL_CERT_ENCRYPT);
                if (confSerialCertEncrypt != null && !string.IsNullOrEmpty(confSerialCertEncrypt.Valor))
                {
                    serialCertEncrypt = confSerialCertEncrypt.Valor;
                }
                else
                {
                    throw new Exception("No se ha encontrado la variable de configuración CONF_SERIAL_CERT_ENCRYPT");
                }

                string serialCertSign = string.Empty;
                ConfiguracionDTO confSerialCertSign = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CONF_SERIAL_CERT_SIGN);
                if (confSerialCertSign != null && !string.IsNullOrEmpty(confSerialCertSign.Valor))
                {
                    serialCertSign = confSerialCertSign.Valor;
                }
                else
                {
                    throw new Exception("No se ha encontrado la variable de configuración CONF_SERIAL_CERT_SIGN");
                }

                X509Certificate2 certEncrypt = CryptoHelper.FindCertificate(serialCertEncrypt);
                X509Certificate2 certSign = CryptoHelper.FindCertificate(serialCertSign);

                SecureMailMessage smail = new SecureMailMessage();
                smail.IsBodyHtml = true;
                string from = ConfigurationManager.AppSettings[MAIL_senderaccount];
                smail.From = new SecureMailAddress(
                    from, from, certEncrypt, certSign);
                foreach(MailAddress address in this.To)
                {
                    smail.To.Add(new SecureMailAddress(
                    address.ToString(), address.User, certEncrypt));
                }
                foreach (string recp in this.Recipients)
                {
                    smail.CC.Add(new SecureMailAddress(
                    recp, recp, certEncrypt));
                }
                smail.Subject = this.Subject;
                //mail.Subject = DBConfig.GetConfiguration(DBConfig.Config.MAIL_INICIO_ASUNTO).Value;

                if (this.AlternateViews.Count > 0) {
                    var stream = this.AlternateViews[0].ContentStream;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        smail.Body = reader.ReadToEnd();
                    }
                }else
                {
                    smail.Body = this.Body;
                }

                if (this.Attachments.Count > 0)
                {
                    foreach(Attachment adjunto in this.Attachments)
                    {
                        SecureAttachment sAdjunto = new SecureAttachment(adjunto.ContentStream,adjunto.Name);
                        smail.Attachments.Add(sAdjunto);
                    }
                }


                smail.IsEncrypted = true;
                smail.IsSigned = true;
                

                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings[MAIL_smtpserver]);
                smtp.Send(smail);
            }
            catch (Exception ex)
            {
                // Registramos el error producido.
                LogHelper.Error("Error el enviar el correo: " + ex.Message, LogHelper.Category.Architecture);
                LogHelper.Error("Error el enviar el correo: " + ex.StackTrace, LogHelper.Category.Architecture);

                // Lanzamos la excepción para que el llamador sepa que no se ha mandado el correo.
                throw new ArqException(true, ex, MessagesManager.CommonErrorMessages.ErrorEnviarEmail.GetHashCode().ToString());
            }
        }
        */
    }
}
