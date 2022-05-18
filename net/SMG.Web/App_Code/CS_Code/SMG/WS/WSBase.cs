using System.Web.Services;
using System.Web.Services.Protocols;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.WS;
using Iberdrola.Commons.Security;
using System;
using Iberdrola.Commons.Logging;

namespace Iberdrola.SMG.WS
{
    public class AuthHeader:SoapHeader    
    {
        public string UserName { set; get; }
        public string Password { set; get; }
    }

    public abstract class WSBase : System.Web.Services.WebService
    {
        public AuthHeader Credentials;

        protected decimal? _idTraza;

        public string[] GetHeaderAuthorization(string authorization)
        {
            string autorizacion = authorization.Replace("Basic ", "");
            string autorizDecodificada = Base64Decode(autorizacion);
            return autorizDecodificada.Split(':');
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public bool ValidarUsuario(string usuario, string password)
        {
            bool bEncontrado = false;

            try
            {
                using (Usuarios usuarios = new Usuarios())
                {
                    AppPrincipal appPrincipal = usuarios.GetPrincipalWS(usuario, password);

                    bEncontrado = appPrincipal != null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error validando usuario en WS : " + ex.Message, LogHelper.Category.BussinessLogic, ex);
                bEncontrado = false;
            }

            return bEncontrado;
        }

        public bool ValidarwebServiceActivo(string nomWebService)
        {
            Boolean habilitarGenerarWS = false;
            Boolean bHabilitarWS = false;

            ConfiguracionDTO confHabilitarWSGeneral = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_WS);

            Enumerados.Configuracion enumConfws = (Enumerados.Configuracion)Enum.Parse(typeof(Enumerados.Configuracion), nomWebService);
            ConfiguracionDTO confHabilitarWS = Configuracion.ObtenerConfiguracion(enumConfws);

            if (confHabilitarWSGeneral != null && !string.IsNullOrEmpty(confHabilitarWSGeneral.Valor) && Boolean.Parse(confHabilitarWSGeneral.Valor))
                habilitarGenerarWS = Boolean.Parse(confHabilitarWSGeneral.Valor);

            if (confHabilitarWS != null && !string.IsNullOrEmpty(confHabilitarWS.Valor) && Boolean.Parse(confHabilitarWS.Valor))
                bHabilitarWS = Boolean.Parse(confHabilitarWS.Valor);

            if (habilitarGenerarWS && bHabilitarWS)
                return true;
            else
                return false;
        }

        protected abstract bool ValidarDatosEntrada(WSRequest request, WSResponse response);

        protected abstract void GuardarTrazaLlamada(WSRequest request);

        protected abstract void GuardarTrazaResultadoLlamada(WSResponse response);
        
    }
}