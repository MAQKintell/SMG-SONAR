using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Principal;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Security;
using System.Collections;
using System.Data.SqlClient;
using Iberdrola.Commons.Web;
using System.Collections.Generic;
using Iberdrola.SMG.UI;
using Iberdrola.Commons.Modules.Authentication.BLL;
using Iberdrola.Commons.Modules.Authentication.DAL.DTO;
using System.DirectoryServices.Protocols;
using System.Globalization;
using Iberdrola.Commons.Messages;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de Usuarios
    /// </summary>
    public class Usuarios: IDisposable
    {
        bool disposed = false;
        
        private const int COD_PERFIL_ADM = 4;
        private const int COD_PERFIL_ADM_POR = 18;
        private const int COD_PERFIL_TLF = 3;
        private const int COD_PERFIL_TLF_POR = 15;
        private const int COD_PERFIL_REC = 12;
        private const int COD_PERFIL_WS = 19;


        public AppPrincipal GetPrincipal(String userName, String password)
        {
            try
            {
                UsuarioDB usuarioDb = new UsuarioDB();
                UsuarioDTO usuarioDto = null;

                ConfiguracionDTO configPasswords = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.PASSWORDS_ENCRIPTADAS);
                if (password != "DESDEDELTA")
                {
                    if (configPasswords != null && configPasswords.Valor != null && bool.Parse(configPasswords.Valor))
                    {
                        usuarioDto = usuarioDb.ObtenerUsuarioCompleto(userName, EncryptHelper.Encrypt(password));
                    }
                    else
                    {
                        usuarioDto = usuarioDb.ObtenerUsuarioCompleto(userName, password);
                    }
                }
                else
                {
                    usuarioDto = usuarioDb.ObtenerUsuarioCompleto(userName, password);
                }
                

                if (usuarioDto != null)
                {
                    if (configPasswords != null && configPasswords.Valor != null && bool.Parse(configPasswords.Valor))
                    {
                        usuarioDto.Password = EncryptHelper.Decrypt(usuarioDto.Password, true);
                    }

                    usuarioDb.InsertarAccesoUsuario(usuarioDto);

                    CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_USUARIO, usuarioDto);
                    return new AppPrincipal(this.ObtenerIdentity(usuarioDto), this.ObtenerRoles(usuarioDto));

                }
                else
                {
                    if (usuarioDb.ExisteUsuario(userName))
                    {
                        throw new BLLException("2001"); // password incorrecta
                    }
                    else
                    {
                        throw new BLLException("2002"); // usuario no existe
                    }
                }
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException(false, ex, "");
            }
        }

        public AppPrincipal GetPrincipalLDAP(String userName, String password, bool ldap)
        {
            try
            {
                UsuarioDB usuarioDb = new UsuarioDB();
                UsuarioDTO usuarioDto = null;

                ConfiguracionDTO configPasswords = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.PASSWORDS_ENCRIPTADAS);
                if (!ldap)
                {
                    if (configPasswords != null && configPasswords.Valor != null && bool.Parse(configPasswords.Valor))
                    {
                        usuarioDto = usuarioDb.ObtenerUsuarioCompleto(userName, EncryptHelper.Encrypt(password));
                    }
                    else
                    {
                        usuarioDto = usuarioDb.ObtenerUsuarioCompleto(userName, password);
                    }
                }
                else
                {
                    usuarioDto = usuarioDb.ObtenerUsuarioLDAP(userName);
                }

                if (usuarioDto != null)
                {
                    if (configPasswords != null && configPasswords.Valor != null && bool.Parse(configPasswords.Valor))
                    {
                        usuarioDto.Password = EncryptHelper.Decrypt(usuarioDto.Password, true);
                    }

                    usuarioDb.InsertarAccesoUsuario(usuarioDto);

                    CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_USUARIO, usuarioDto);
                    return new AppPrincipal(this.ObtenerIdentity(usuarioDto), this.ObtenerRoles(usuarioDto));

                }
                else
                {
                    if (usuarioDb.ExisteUsuario(userName))
                    {
                        throw new BLLException("2001"); // password incorrecta
                    }
                    else
                    {
                        throw new BLLException("2002"); // usuario no existe
                    }
                }
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException(false, ex, "");
            }
        }

        public AppPrincipal GetPrincipalWS(String userName, String password)
        {
            try
            {
                // Kintell R#35636
                AppPrincipal appPrincipalLdap = null;
                if (userName.Substring(0,2)=="S0")
                { 
                    appPrincipalLdap = AccesoPorLdap(userName, password);
                }
                if (appPrincipalLdap == null)
                {
                    UsuarioDB usuarioDb = new UsuarioDB();
                    UsuarioDTO usuarioDto = null;

                    ConfiguracionDTO configPasswords = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.PASSWORDS_ENCRIPTADAS);
                    if (configPasswords != null && configPasswords.Valor != null && bool.Parse(configPasswords.Valor))
                    {
                        usuarioDto = usuarioDb.ObtenerUsuarioCompletoWS(userName, EncryptHelper.Encrypt(password));
                    }
                    else
                    {
                        usuarioDto = usuarioDb.ObtenerUsuarioCompletoWS(userName, password);
                    }


                    if (usuarioDto != null)
                    {
                        if (configPasswords != null && configPasswords.Valor != null && bool.Parse(configPasswords.Valor))
                        {
                            usuarioDto.Password = EncryptHelper.Decrypt(usuarioDto.Password, true);
                        }

                        usuarioDb.InsertarAccesoUsuario(usuarioDto);

                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_USUARIO, usuarioDto);
                        return new AppPrincipal(this.ObtenerIdentity(usuarioDto), this.ObtenerRoles(usuarioDto));

                    }
                    else
                    {
                        if (usuarioDb.ExisteUsuario(userName))
                        {
                            throw new BLLException("2001"); // password incorrecta
                        }
                        else
                        {
                            throw new BLLException("2002"); // usuario no existe
                        }
                    }
                }
                else
                {
                    return appPrincipalLdap;
                }
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException(false, ex, "");
            }
       
        }

        private AppPrincipal AccesoPorLdap(String userName, String password)
        {

            bool esUsuarioExcepcion = Usuarios.EsUsuarioExcepcion(userName);
            bool autenticado = false;
            bool ldap = true;
            bool ldapActivo = true;
            LoginDTO lg = new LoginDTO();
            lg.IdNumber = "";

            lg.UserId = userName;
            lg.Password = password;
            // Cargamos el tipo de logeo.
            ConfiguracionDTO confTipoLogin = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LDAP_COMPROBACION_HABILITADA);
            if (confTipoLogin != null && !String.IsNullOrEmpty(confTipoLogin.Valor))
            {
                ldapActivo= Convert.ToBoolean(confTipoLogin.Valor);
            }

            //// Cargamos si se habilitan las condiciones especiales.
            //ConfiguracionDTO confCondiciones = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LDAP_COMPROBACION_CONDICIONES_HABILITADA);
            //if (confCondiciones != null && !String.IsNullOrEmpty(confCondiciones.Valor))
            //{
            //    CurrentSession.SetAttribute(SessionVariables.SESSION_USUARIO_CONDICIONES, confCondiciones.Valor);
            //}

            if (Convert.ToBoolean(ldapActivo) && !esUsuarioExcepcion)
            {
                // el logeo es a través de LDAP.
                try
                {
                    LdapHelper ldapH = new LdapHelper();
                    autenticado = ldapH.IsAuthenticated(lg);
                }
                catch (LdapException ldapException)
                {
                    //if (ldapException.ErrorCode == 49) //La credencial proporcionada no es válida
                    //{
                        return null;
                    //}
                }
                catch (DirectoryOperationException dirOpException)
                {
                    // Objeto no existe
                    return null;
                }
            }

            Usuarios usuarios = new Usuarios();
            AppPrincipal appPrincipal = usuarios.GetPrincipalLDAP(lg.UserId, lg.Password, ldap);
            return appPrincipal;

        }

        private IIdentity ObtenerIdentity(UsuarioDTO usuario)
        {
            IIdentity appIdentity = new AppIdentity(usuario.Nombre, usuario);
            return appIdentity;
        }

        private string[] ObtenerRoles(UsuarioDTO usuario)
        {
            ArrayList aListaRoles = new ArrayList();
            if (usuario.ListaRoles != null)
            {
                string strRol = "";
                foreach (PerfilDTO perfil in usuario.ListaRoles)
                {
                    strRol = perfil.Nombre;
                    aListaRoles.Add(strRol);
                }
            }
            return (String[])aListaRoles.ToArray(typeof(string));
        }

        public static Boolean EsAdministrador(UsuarioDTO usuario)
        {
            if (usuario.Id_Perfil != null && usuario.Id_Perfil.ToString().Equals("3"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static IDataReader ObtenerPerfiles()
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerPerfiles();
        }

        public static int ObtenerCantidadUsuariosPorNombre(string Nombre, string Apellido1, string Apellido2,String Contrato,string Subtipo,string DNI, string CUI)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerCantidadUsuariosPorNombre(Nombre, Apellido1, Apellido2,Contrato,Subtipo,DNI, CUI);
        }

        public static int ObtenerCantidadUsuariosCalderaPorNombre(string Nombre, string Apellido1, string Apellido2, String Contrato, string Subtipo)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerCantidadUsuariosCalderaPorNombre(Nombre, Apellido1, Apellido2, Contrato, Subtipo);
        }

        public IDataReader ObtenerUsuariosPorNombre(string Nombre, string Apellido1, string Apellido2,string Subtipo)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerUsuariosPorNombre(Nombre, Apellido1, Apellido2, Subtipo);
        }

        public static Boolean AniadirUsuario(UsuarioDTO usuario, string strUsuarioConectado)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            if (usuarioDb.ExisteUsuario(usuario.Login))
            {
                //throw new BLLException("2008"); // el usuario ya existe
                return false;
            }
            else
            {
                // Guardamos la password encryptada si está habilitada la encriptación.
                ConfiguracionDTO configPasswords = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.PASSWORDS_ENCRIPTADAS);
                if (!string.IsNullOrEmpty(configPasswords.Valor) && bool.Parse(configPasswords.Valor))
                {
                    usuario.Password = EncryptHelper.Encrypt(usuario.Password);
                }

                return usuarioDb.AniadirUsuario(usuario, strUsuarioConectado);
            }
        }

        public static Boolean EliminarUsuario(UsuarioDTO usuario, string strUsuario)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            if (!usuarioDb.ExisteUsuario(usuario.Login))
            {
                throw new BLLException("2002"); // el usuario NO existe
            }

            return usuarioDb.EliminarUsuario(usuario, strUsuario);
        }

        public static Boolean ModificarUsuario(UsuarioDTO usuario, string strUsuarioConectado)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            //if (!usuarioDb.ExisteUsuario(usuario.Login))
            //{
            //    throw new BLLException("2002"); // el usuario NO existe
            //}

            return usuarioDb.ModificarUsuario(usuario, strUsuarioConectado);
        }

        public static Boolean ActivarUsuario(UsuarioDTO usuario, string strUsuarioConectado)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            if (!usuarioDb.ExisteUsuario(usuario.Login))
            {
                throw new BLLException("2002"); // el usuario NO existe
            }

            return usuarioDb.ActivarUsuario(usuario, strUsuarioConectado);
        }

        public static DataTable ObtenerUsuarios(string columna, string valor)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerUsuarios(columna, valor);
        }

        public static UsuarioDTO ObtenerUsuario(string userID)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerUsuario(userID);
        }

        public static string UsuarioTramitadorBaja(string userID)
        {
            UsuarioDB usuarioDb = new UsuarioDB();
            return usuarioDb.UsuarioTramitadorBaja(userID);
        }

        public static IDataReader ObtenerProveedores()
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerProveedores();
        }

        public static IDataReader ObtenerTecnicoEmpresa(String Proveedor)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerTecnicoEmpresa(Proveedor);
        }

        public static String ObtenerEmpresaPorTecnico(Int16 idTecnico)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerEmpresaPorTecnico(idTecnico);
        }

        public static String ObtenerNombreTecnicoPorTecnico(Int16 idTecnico)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerNombreTecnicoPorTecnico(idTecnico);
        }

        /// <summary>
        /// Indica si la contraseña de un usuario se ha caducado o no. Para ello utiliza el 
        /// valor de la tabla de configuración P_CONFIG que indica el número de días en los que és
        /// válida una contraseña.
        /// </summary>
        /// <param name="strLogin">Login del usuario que quiere entrar en la aplicación.</param>
        /// <returns> True si está caducada, false si es válida </returns>
        public static bool ContraseniaCaducada(string strLogin)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ContraseniaCaducada(strLogin);
        }

        public static UsuarioDTO ObtenerUsuarioLogeado()
        {
            if (HttpContext.Current != null)
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                if (usuarioPrincipal != null)
                {
                    return (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                }
            }

            return null;
        }


        public static int ObtenerIdiomaUsuarioLogeado()
        {
            UsuarioDTO u = ObtenerUsuarioLogeado();

            if (u != null)
            {
                return u.IdIdioma;
            }
            else
            {
                // Si no hay ningún usuario devolvemos el lenguaje por defecto 1 - Castellano
                return 1;
            }
        }


        /// <summary>
        /// Indica si el cod perfil pasado es de un adminsitrador
        /// </summary>
        /// <param name="intCodPerfil"></param>
        /// <returns></returns>
        public static bool EsAdministrador(int intCodPerfil)
        {
            if (intCodPerfil == COD_PERFIL_ADM || intCodPerfil == COD_PERFIL_ADM_POR)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indica si el cod perfil pasado es del teléfono
        /// </summary>
        /// <param name="intCodPerfil"></param>
        /// <returns></returns>
        public static bool EsTelefono(int intCodPerfil)
        {
            if (intCodPerfil == COD_PERFIL_TLF || intCodPerfil == COD_PERFIL_REC || intCodPerfil == COD_PERFIL_TLF_POR)
            {
                return true; //intCodPerfil == COD_PERFIL_TLF || COD_PERFIL_REC;
            }
            else { return false; }
        }

        /// <summary>
        /// Indica si el cod perfil pasado es de Web Service
        /// </summary>
        /// <param name="intCodPerfil"></param>
        /// <returns></returns>
        public static bool EsWebService(int intCodPerfil)
        {
            if (intCodPerfil == COD_PERFIL_WS)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indica si el cod perfil pasado es de Reclamación
        /// </summary>
        /// <param name="intCodPerfil"></param>
        /// <returns></returns>
        public static bool EsReclamacion(int intCodPerfil)
        {
            return intCodPerfil == COD_PERFIL_REC;
        }

        /// <summary>
        /// Indica si el cod perfil pasado es de un proveedor
        /// </summary>
        /// <param name="intCodPerfil"></param>
        /// <returns></returns>
        public static bool EsProveedor(int intCodPerfil)
        {
            if (EsAdministrador(intCodPerfil) || EsTelefono(intCodPerfil) || EsWebService(intCodPerfil))
            {
                return false;
            }
            else
            {
                // Si es 0 tampoco es proveedor, sería como NULL
                if (intCodPerfil == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static DataTable ObtenerUsuarios(string columna, string valor, bool bajas, string login)
        {
            UsuarioDB usuarioDb = new UsuarioDB();

            return usuarioDb.ObtenerUsuarios(columna, valor, bajas, login);
        }

        /// <summary>
        /// Cambia la password del usuario en la base de datos
        /// </summary>
        /// <param name="strLogin">Código de usuarios que quiere cambiar la password</param>
        /// <param name="strPassword">Nueva password</param>
        /// <param name="strUsuarioConectado">Login del usuario conectado</param>
        public static void CambiarPassword(string login, string password, string strUsuarioConectado)
        {
            UsuarioDB usuarioDb = new UsuarioDB();
            usuarioDb.CambiarPassword(login, password, strUsuarioConectado);
        }

        /// <summary>
        /// Obtiene todos los usuarios de la base de datos, independientemente de que estén activos
        /// o no. Se utilizará principalmente para encriptar o desencriptar la passwords
        /// </summary>
        /// <returns>List<UsuarioDTO> con los datos de los usuarios recuperados.</returns>
        public static List<UsuarioDTO> ObtenerUsuarios()
        {
            UsuarioDB usuarioDb = new UsuarioDB();
            return usuarioDb.ObtenerUsuarios();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
            }

            disposed = true;
        }

        public static bool EsUsuarioIberdrola(string usuario)
        {
            bool usuarioIberdrola = false;

            if (!String.IsNullOrEmpty(usuario))
            {
                UsuarioDB usuarioDB = new UsuarioDB();


                ConfiguracionDTO confUsuIberdrola = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CONF_USUARIOS_IBERDROLA);
                if (confUsuIberdrola != null && confUsuIberdrola.Valor != "")
                {
                    string[] usuariosIberdrola = confUsuIberdrola.Valor.Split(';');

                    foreach (var usIb in usuariosIberdrola)
                    {
                        if (usuario.ToString().ToUpper().StartsWith(usIb))
                        {
                            usuarioIberdrola = true;
                            break;
                        }
                    }

                }
            }
            return usuarioIberdrola;
        }

        public static bool EsUsuarioExcepcion(string usuario)
        {
            bool usuarioExcepcion = false;

            if (!String.IsNullOrEmpty(usuario))
            {
                UsuarioDB usuarioDB = new UsuarioDB();


                ConfiguracionDTO confUsuExcepcion = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CONF_USUARIOS_EXCEPCION);
                if (confUsuExcepcion != null && confUsuExcepcion.Valor != "")
                {
                    string[] usuariosExcepcion = confUsuExcepcion.Valor.Split(';');

                    foreach (var usEx in usuariosExcepcion)
                    {
                        if (usuario.ToString().ToUpper() == usEx)
                        {
                            usuarioExcepcion = true;
                            break;
                        }
                    }

                }
            }
            return usuarioExcepcion;
        }       



        public static bool EsUsuarioNoPolitica(string usuario)
        {
            bool usuarioNoPolitica = false;

            if (!String.IsNullOrEmpty(usuario))
            {
                UsuarioDB usuarioDB = new UsuarioDB();

                ConfiguracionDTO confUsuNoPolitica = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.USUARIOS_SALTAR_FIRMA_POLITICA);
                if (confUsuNoPolitica != null && confUsuNoPolitica.Valor != "")
                {
                    string[] usuariosNoPolitica = confUsuNoPolitica.Valor.Split(';');

                    foreach (var usNP in usuariosNoPolitica)
                    {
                        if (usuario.ToString().ToUpper().StartsWith(usNP))
                        {
                            usuarioNoPolitica = true;
                            break;
                        }
                    }
                }
            }
            return usuarioNoPolitica;
        }
    }
}