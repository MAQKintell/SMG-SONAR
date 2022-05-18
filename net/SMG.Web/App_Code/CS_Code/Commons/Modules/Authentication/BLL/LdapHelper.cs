using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Modules.Authentication.DAL.DTO;
using System.DirectoryServices.Protocols;
using System.Net;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.BLL;

namespace Iberdrola.Commons.Modules.Authentication.BLL
{
    public class LdapHelper
    {

        /// <summary>
        /// Permisos obligatorios que tiene que haber en P_PERMISO.
        /// ** Es necesario que en P_PERMISO exista una entrada por cada clave y con RESTRICCION = 0
        /// </summary>
        private enum LdapConfiguration
        {
        }

        private string _baseDN;
        private string _userDN;
        private string _userDNService;
        private string _server;
        private int _port;
        
        /// <summary>
        /// Recuperamos de la configuración los datos necesarios para establecer la conexión
        /// </summary>
        public LdapHelper()
        {
            // Recuperamos el base DN
            ConfiguracionDTO confBaseDN = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LDAP_CONFIGURATION_BASE_DN);
            if (confBaseDN != null && !string.IsNullOrEmpty(confBaseDN.Valor))
            {
                this._baseDN = confBaseDN.Valor;
            }
            else
            {
                //throw new ArqException();
                throw new Exception("No se ha encontrado la variable de configuración LDAP_CONFIGURATION_BASE_DN");
            }

            // Recuperamos el user DN
            ConfiguracionDTO confUserDN = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LDAP_CONFIGURATION_USER_DN);
            if (confUserDN != null && !string.IsNullOrEmpty(confUserDN.Valor))
            {
                this._userDN = confUserDN.Valor;
            }
            
            // Recuperamos el user DN para cuentas de Servicio
            ConfiguracionDTO confUserDNServicio = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LDAP_CONFIGURATION_USER_DN_CUENTAS_SERVICIO);
            if (confUserDNServicio != null && !string.IsNullOrEmpty(confUserDNServicio.Valor))
            {
                this._userDNService = confUserDNServicio.Valor;
            }


            // Recuperamos el servidor
            ConfiguracionDTO confServer = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LDAP_CONFIGURATION_SERVER);
            if (confServer != null && !string.IsNullOrEmpty(confServer.Valor))
            {
                this._server = confServer.Valor;
            }

            // Recuperamos el puerto
            ConfiguracionDTO confPort = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LDAP_CONFIGURATION_PORT);
            if (confPort != null && !string.IsNullOrEmpty(confPort.Valor))
            {
                this._port = int.Parse(confPort.Valor);
            }

        }

        /// <summary>
        /// Busca el usuario en LDAP
        /// </summary>
        /// <param name="login">Objeto con los datos de autenticación del usuario</param>
        /// <returns>true si las credenciales son correctas, false si no</returns>
        public bool IsAuthenticated(LoginDTO login)
        {

            LdapConnection ldapCon = null;
            try
            {
                ldapCon = new LdapConnection(new LdapDirectoryIdentifier(this._server, this._port));
                ldapCon.AuthType = AuthType.Basic;
                string sCredential = "";
                if (login.UserId.Substring(0, 1) == "S")
                {
                    sCredential = this._userDNService.Replace("##", login.UserId);
                }
                else
                {
                    sCredential = this._userDN.Replace("##", login.UserId);
                }

                ldapCon.Credential = new NetworkCredential(sCredential, login.Password);
                ldapCon.Bind();
                ldapCon.Timeout = new TimeSpan(1, 0, 0);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
