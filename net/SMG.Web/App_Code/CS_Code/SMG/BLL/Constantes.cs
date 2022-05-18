namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Clase en la que se encuentran las constantes a utilizar por la aplicación, a nuvel
    /// de lógica de negocio.
    /// </summary>
    public class Constantes
    {
        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Constantes()
        {
        }

        /// <summary>
        /// Id del usuario del sistema.
        /// </summary>
        public const string USUARIO_SISTEMA = "SISTEMA";

        /// <summary>
        /// Variable del Web.config donde se almacena el subject de los correos que se envíen.
        /// </summary>
        public const string INICIO_ASUNTO_EMAIL = "INICIO_ASUNTO_EMAIL";



        /// <summary>
        /// Clave para obtener el usuario que utilizar en el impersonate
        /// </summary>
        public const string APP_SETTING_USUARIO_RED_IMPERSONATE = "USUARIO_RED_IMPERSONATE";
        /// <summary>
        /// Clave para obtener el dominio de usuario que utilizar en el impersonate
        /// </summary>
        public const string APP_SETTING_DOMINIO_RED_IMPERSONATE = "DOMINIO_RED_IMPERSONATE";
        /// <summary>
        /// Clave para obtener la contraseña de usuario que utilizar en el impersonate
        /// </summary>
        public const string APP_SETTING_PASSWORD_RED_IMPERSONATE = "PASSWORD_RED_IMPERSONATE";
        /// <summary>
        /// Clave para obtener si se utilizará o no el impersonate
        /// </summary>
        public const string APP_SETTING_IMPERSONATE_USER = "IMPERSONATE_USER";



    }
}
