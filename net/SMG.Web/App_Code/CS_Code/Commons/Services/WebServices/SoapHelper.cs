using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using Iberdrola.Commons.Utils;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.BLL;

namespace Iberdrola.Commons.Services.WebServices
{

    public class SoapHelper
    {
        public string UrlAdress { get; set; }
        public string User { get; set; }
        public string ProxyURL { get; set; }
        public string Password { get; set; }
        public string XmlContent { get; set; }
        public string Method { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPassword { get; set; }
        public string RequestContentType { get; set; }
        public string RequestAccept { get; set; }
        public string RequestMethod { get; set; }
        public string Autorizacion { get; set; }
        


        //public XmlDocument CallWebService()
        public string CallWebService(Boolean TLS2)
        {
            string result = "";
            // ===== You shoudn't need to edit the lines below =====
            
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12);
            Boolean ActivoTLS12 = false;

            if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
            {
                ActivoTLS12 = Boolean.Parse(confActivoTLS12.Valor);
            }

            if (TLS2 && ActivoTLS12)
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            }

            // Create the web request
            HttpWebRequest request = WebRequest.Create(new Uri(UrlAdress)) as HttpWebRequest;
            
            if (!string.IsNullOrEmpty(this.User))
            {
                NetworkCredential nc = new NetworkCredential(this.User, this.Password);
                request.Credentials = nc;
            }
            
            if (!string.IsNullOrEmpty(this.ProxyUser))
            {
                request.Proxy = new WebProxy(ProxyURL, true);
                NetworkCredential nc = new NetworkCredential(this.ProxyUser, this.ProxyPassword);
                request.Proxy.Credentials = nc;
            }

            if (!string.IsNullOrEmpty(this.Autorizacion))
            {
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(this.User + ":" + this.Password));
                request.Headers.Add("Authorization", this.Autorizacion + " " + svcCredentials);
            }

            request.Headers.Add("SOAPAction", this.Method);
            
            request.ContentType = this.RequestContentType;// "text/xml;charset=\"utf-8\"";
            request.Accept = this.RequestAccept;// "text/xml";
            request.Method = this.RequestMethod;// "POST";
            
            using (Stream stm = request.GetRequestStream())
            {
                 using (StreamWriter stmw = new StreamWriter(stm))
                 {
                      stmw.Write(this.XmlContent);
                 }
            }

            // Get response and return it
            XmlDocument xmlResult = new XmlDocument();
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    reader.Close();
                }
                //xmlResult.LoadXml(result);
            }
            catch (WebException wex)
            {
                result = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd().ToString();
            }
            catch (Exception e)
            {
                //xmlResult = this.CreateErrorXML(e.Message, e.StackTrace);  //TODO: returns an XML with the error message
                result = "Message: " + e.Message + ". StackTrace: " + e.StackTrace;
            }
            //return xmlResult;
            return result;
        }

        private XmlDocument CreateErrorXML(string sError, string strStackTrace)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<CallWebServiceError></CallWebServiceError>"); 

            XmlNode newElem = doc.CreateNode(XmlNodeType.Element, "Error", "");
            newElem.InnerText = sError;
            XmlElement root = doc.DocumentElement;
            root.AppendChild(newElem);

            newElem = doc.CreateNode(XmlNodeType.Element, "StackTrace", "");
            newElem.InnerText = strStackTrace;
            root.AppendChild(newElem);

            return doc;
        }

        public static string[] GetHeaderAuthorization(string authorization) {
            string autorizacion = authorization.Replace("Basic ", "");
            string autorizDecodificada = StringUtils.Base64Decode(autorizacion);
            return autorizDecodificada.Split(':');
        }

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
