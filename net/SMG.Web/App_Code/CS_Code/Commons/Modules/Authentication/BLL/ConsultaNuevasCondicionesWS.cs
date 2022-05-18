using System;
using System.Net;
using System.Text;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.BLL;
using Commons.Modules.Authentication.BLL;

namespace Iberdrola.Commons.Modules.Authentication.BLL
{
    /// <summary>
    /// Clase de utilidad para consultar y aceptar las condiciones generales
    /// </summary>
    public class ConsultaNuevasCondicionesWS : ConsultaNuevasCondicionesWSService
    {
        /// <summary>
        /// Método para controlar el acceso al web service que se quiere consultar.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected override System.Net.WebRequest GetWebRequest(Uri uri)
        {
            HttpWebRequest request;

            // Se obtiene la url del web service desde Base de Datos.
            ConfiguracionDTO uriCN = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ARQ_COND_NUEVAS_URI);

            if (uriCN != null && uriCN.Valor != uri.AbsoluteUri.ToString())
            {
                uri = new Uri(uriCN.Valor);
            }

            request = (HttpWebRequest)base.GetWebRequest(uri);
            

            request.ContentType = "text/xml; charset=utf8";
            
            NetworkCredential networkCredentials =
                Credentials.GetCredential(uri, "Basic");

            if (networkCredentials != null)
            {
                byte[] credentialBuffer = new UTF8Encoding().GetBytes(
                    networkCredentials.UserName + ":" +
                    networkCredentials.Password);
                request.Headers["Authorization"] =
                    "Basic " + Convert.ToBase64String(credentialBuffer);
            }
            else
            {
                throw new ApplicationException("No network credentials");
            }
            return request;
        }

        /// <summary>
        /// Método para cambiar la codificación de la respuesta del web service.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = base.GetWebResponse(request);
            response.Headers["Content-Type"] = "text/xml; charset=utf-8"; 
            return response;

        }
    }
}
