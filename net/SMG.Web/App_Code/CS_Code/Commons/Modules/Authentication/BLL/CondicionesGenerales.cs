using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Protocols;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Logging;
using Iberdrola.Commons.Modules.Authentication.DAL.DTO;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Modules.Authentication.BLL;
using Commons.Modules.Authentication.BLL;

namespace Iberdrola.Commons.Modules.Authentication.BLL
{
    /// <summary>
    /// Clase de utilidad para consultar y aceptar las condiciones generales
    /// </summary>
    public class CondicionesGenerales
    {
        private AuthenticationDTO _authentication { get; set; }
        private SoapException _faultNuevasCondiciones { get; set;}
        private SoapException _faultAceptarCondiciones;

        /// <summary>
        /// Obtiene el objeto con la información de autenticación de las condiciones generales
        /// </summary>
        /// <returns></returns>
        public AuthenticationDTO Authentication
        {
            get { return this._authentication; }
        }

        /// <summary>
        /// Obtiene el objeto con la información error nuevas condiciones
        /// </summary>
        /// <returns></returns>
        public SoapException FaultNuevasCondiciones
        {
            get { return this._faultNuevasCondiciones; }
        }

        /// <summary>
        /// Datos necesarios para la conexión con el WS de condiciones generales
        /// ** Es necesario que en P_CONFIG exista una entrada por cada clave
        /// </summary>
        private enum Configuration
        {
            //********************************************
            // --> Movidas a Enumerados.Configuracion
            //********************************************

            /// <summary>
            /// Tipo de cabecera de autenticación que hay que proporcionar en la llamada del WS.
            /// </summary>
            [StringValue("ARQ_COND_WS_AUT_HEADER")]
            ARQ_COND_WS_AUT_HEADER,
            /// <summary>
            /// Valor de idioma a informar en la llamada a los WS.
            /// </summary>
            [StringValue("ARQ_COND_WS_IDIOMA")]
            ARQ_COND_WS_IDIOMA,
            /// <summary>
            /// Valor de login necesario para acceder al WS de condiciones generales.
            /// </summary>
            [StringValue("ARQ_COND_WS_LOGIN")]
            ARQ_COND_WS_LOGIN,
            /// <summary>
            /// Valor de necogio a informar en la llamada a los WS.
            /// </summary>
            [StringValue("ARQ_COND_WS_NEGOCIO")]
            ARQ_COND_WS_NEGOCIO,
            /// <summary>
            /// Valor de la clave necesaria para acceder al WS de condiciones generales.
            /// </summary>
            [StringValue("ARQ_COND_WS_PASS")]
            ARQ_COND_WS_PASS,
            /// <summary>
            /// Códigos de error del WS para los que mostramos los mensajes de error devueltos por el WS de Obtener condiciones.
            /// </summary>
            [StringValue("ARQ_COND_WS_CODIGOS_ERROR_OBTENER_CONDICIONES")]
            ARQ_COND_WS_CODIGOS_ERROR_OBTENER_CONDICIONES,
            /// <summary>
            /// Códigos de error del WS para los que mostramos los mensajes de error devueltos por el WS de Aceptar condiciones.
            /// </summary>
            [StringValue("ARQ_COND_WS_CODIGOS_ERROR_ACEPTAR_CONDICIONES")]
            ARQ_COND_WS_CODIGOS_ERROR_ACEPTAR_CONDICIONES,
        }

        private string _AutHeader;
        /// <summary>
        /// Tipo de cabecera de autenticación que hay que proporcionar en la llamada del WS.
        /// </summary>
        /// <returns></returns>
        private string AutHeader()
        {
            if (this._AutHeader == null)
            {
                ConfiguracionDTO autHeader = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ARQ_COND_WS_AUT_HEADER);
                if (autHeader != null && !string.IsNullOrEmpty(autHeader.Valor))
                {
                    this._AutHeader = autHeader.Valor;
                }
            }
            return this._AutHeader;
        }

        private string _Idioma;
        /// <summary>
        /// Valor de idioma a informar en la llamada a los WS.
        /// </summary>
        /// <returns></returns>
        private string Idioma()
        {
            if (this._Idioma == null)
            {
                ConfiguracionDTO idioma = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ARQ_COND_WS_IDIOMA);
                if (idioma != null && !string.IsNullOrEmpty(idioma.Valor))
                {
                    this._Idioma = idioma.Valor;
                }
            }
            return this._Idioma;
        }

        private string _WSLogin;
        /// <summary>
        /// Valor de login necesario para acceder al WS de condiciones generales.
        /// </summary>
        /// <returns></returns>
        private string WSLogin()
        {
            if (this._WSLogin == null)
            {
                ConfiguracionDTO wsLogin = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ARQ_COND_WS_LOGIN);
                if (wsLogin != null && !string.IsNullOrEmpty(wsLogin.Valor))
                {
                    this._WSLogin = wsLogin.Valor;
                }
            }

            if (this._WSLogin != null)
            {
                return EncryptHelper.Decrypt(this._WSLogin, true);
            }

            return this._WSLogin;
        }

        private string _Negocio;
        /// <summary>
        /// Valor de necogio a informar en la llamada a los WS.
        /// </summary>
        /// <returns></returns>
        private string Negocio()
        {
            if (this._Negocio == null)
            {
                ConfiguracionDTO negocio = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ARQ_COND_WS_NEGOCIO);
                if (negocio != null && !string.IsNullOrEmpty(negocio.Valor))
                {
                    this._Negocio = negocio.Valor;
                }
            }
            return this._Negocio;
        }

        private string _WSPass;
        /// <summary>
        /// Valor de la clave necesaria para acceder al WS de condiciones generales.
        /// </summary>
        /// <returns></returns>
        private string WSPass()
        {
            if (this._WSPass == null)
            {
                ConfiguracionDTO wsPass = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ARQ_COND_WS_PASS);
                if (wsPass != null && !string.IsNullOrEmpty(wsPass.Valor))
                {
                    this._WSPass = wsPass.Valor;
                }
            }

            if (this._WSPass != null)
            {
                return EncryptHelper.Decrypt(this._WSPass, true);
            }

            return this._WSPass;
        }

        /// <summary>
        /// Constructor de la clase de usuario e id
        /// </summary>
        /// <param name="userId">Expediente del usuario del que se comprobarán las condiciones generales</param>
        /// <param name="idNumber">DNI del usuario del que se comprobarán las condiciones generales</param>
        public CondicionesGenerales(string userId, string idNumber)
        {
            this._authentication = new AuthenticationDTO();
            this._authentication.UserId = userId;
            this._authentication.IdNumber = idNumber;
            this._authentication.CodVersion = string.Empty;
            this._authentication.TextoCondField = string.Empty;
        }

        /// <summary>
        /// Constructor de la clase con el objeto AuthenticationDTO ya cargado
        /// </summary>
        public CondicionesGenerales(AuthenticationDTO authentication)
        {
            this._authentication = authentication;
        }

        private static bool AllwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        private void CargarCondiciones()
        {
            ConsultaNuevasCondicionesWS ws;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12);
                Boolean ActivoTLS12 = false;

                if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                {
                    ActivoTLS12 = Boolean.Parse(confActivoTLS12.Valor);
                }
                if (ActivoTLS12)
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                }

                ws = new ConsultaNuevasCondicionesWS();

                ws.PreAuthenticate = true;
                ws.Credentials = new NetworkCredential(this.WSLogin(), this.WSPass());
                
                //ws.RequestEncoding = System.Text.Encoding.Default;

                //Vaciar la respuesta antes de la llamada
                this._authentication.CodVersion = string.Empty;
                this._authentication.TextoCondField = string.Empty;
                this._faultNuevasCondiciones = null;

                nuevaCondicionVO ncVO = ws.consultaNuevasCondiciones(_authentication.UserId, this.Negocio(), _authentication.IdNumber, this.Idioma());
                if (ncVO != null && ncVO.existeNueva.Equals("S"))
                {
                    this._authentication.CodVersion = ncVO.codVersion;
                    this._authentication.TextoCondField = ncVO.textoCond;
                }

            }
            catch (SoapException se)
            {
                this._faultNuevasCondiciones = se;
                LogHelper.Debug("Excepción en la llamada consultaNuevasCondiciones: '" + se.Code + "', '" + se.Message, LogHelper.Category.BussinessLogic, se);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AceptarCondiciones()
        {
            if (this.TieneNuevasCondiciones(false))
            {
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                    ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12);
                    Boolean ActivoTLS12 = false;

                    if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                    {
                        ActivoTLS12 = Boolean.Parse(confActivoTLS12.Valor);
                    }
                    if (ActivoTLS12)
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    }

                    AceptarCondicionWS ws = new AceptarCondicionWS();

                    ws.PreAuthenticate = true;
                    ws.Credentials = new NetworkCredential(this.WSLogin(), this.WSPass());

                    //Vaciar la respuesta antes de la llamada
                    this._faultAceptarCondiciones = null;

                     ws.aceptarCondicion(_authentication.UserId, this.Negocio(), this._authentication.CodVersion);
                }
                catch (SoapException se)
                {
                    this._faultAceptarCondiciones = se;
                    LogHelper.Debug("Excepción en la llamada consultaNuevasCondiciones: '" + se.Code + "', '" + se.Message, LogHelper.Category.BussinessLogic, se);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Devuelve si el usuario proporcionado tiene condiciones generales pendientes de aceptar.
        /// Si no están cargadas las condiciones las obtiene del WS.
        /// </summary>
        /// <returns>bool indicando si el cliente tiene condiciones generales pendientes de  aceptar</returns>
        public bool TieneNuevasCondiciones(bool saltarCondiciones)
        {
            if (string.IsNullOrEmpty(this._authentication.CodVersion))
            {
                CargarCondiciones();
            }

            if (saltarCondiciones)
            {
                return false;
            }
            else
            {
                return (this._faultNuevasCondiciones == null) && !string.IsNullOrEmpty(this._authentication.CodVersion);
            }
        }

        /// <summary>
        /// Obtiene el texto de las condiciones generales pendientes de aceptar.
        /// </summary>
        /// <returns>string con el texto a mostrar al usuario para que acepte las condiciones.</returns>
        public string ObtenerTextoNuevasCondiciones()
        {
            if (this.TieneNuevasCondiciones(false))
            {
                return this._authentication.TextoCondField;
            }

            return string.Empty;
        }

        /// <summary>
        /// Devuelve el texto del error producido tras la llamada al WS de ConsultaNuevasCondicionesGenerales.
        /// De no haber ningún error se devolverá null.
        /// </summary>
        /// <returns>String con el texto del error o null.</returns>
        public string ObtenerTextoErrorNuevasCondiciones()
        {
            if (this._faultNuevasCondiciones != null)
            {
                ConfiguracionDTO wsCodigoError = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ARQ_COND_WS_CODIGOS_ERROR_OBTENER_CONDICIONES);

                bool mostrarError = false;

                try
                {
                    //if (this._faultNuevasCondiciones.Detail.ChildNodes[0].SelectSingleNode("faultCode").InnerText.Equals("10"))
                    //{
                    //    throw new BLLException("2002");
                    //}
                    //else
                    //{
                        if (wsCodigoError != null)
                        {
                            foreach (string codigoError in wsCodigoError.Valor.Split(';'))
                            {
                                if (codigoError.Equals(this._faultNuevasCondiciones.Detail.ChildNodes[0].SelectSingleNode("faultCode").InnerText))
                                {
                                    mostrarError = true;
                                }
                            }
                        }
                    //}
                }
                catch (Exception ex)
                {
                    //Error Interno
                    throw new BLLException("3005");
                }

                if (mostrarError)
                {
                    return this._faultNuevasCondiciones.Message;
                }
                else
                {
                    //Error Interno
                    throw new BLLException("3005");
                }
            }

            return null;
        }

        /// <summary>
        /// Devuelve el texto del error producido tras la llamada al WS de AceptarCondicionGeneral.
        /// De no haber ningún error se devolverá null.
        /// </summary>
        /// <returns>String con el texto del error o null.</returns>
        public string ObtenerTextoErrorAceptarCondiciones()
        {
            if (this._faultAceptarCondiciones != null)
            {
                throw new BLLException("3005");
            }

            return null;
        }
    }
}
