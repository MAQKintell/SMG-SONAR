using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Iberdrola.Commons.Exceptions;

namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Descripción breve de CurrentSession
    /// </summary>
    public class CurrentSession
    {
        
        public const string SESSION_ERROR_VARIABLE = "SESSION_ERRROR_VARIABLE";
        public const string SESSION_USUARIO_AUTENTIFICADO = "SESSION_USUARIO_AUTENTIFICADO";
        public const string SESSION_DATOS_USUARIO = "SESSION_DATOS_USUARIO";
        public const string SESSION_DATOS_VISITA = "SESSION_DATOS_VISITA";
        public const string SESSION_DATOS_MANTENIMIENTO = "SESSION_DATOS_MANTENIMIENTO";
        public const string SESSION_EXCEL_HELPER = "SESSION_EXCEL_HELPER";
        public const string SESSION_ORDEN_DGCONTRATOS = "SESSION_ORDEN_DGCONTRATOS";
        public const string SESSION_USUARIO_VALIDO = "usuarioValido";
        public const string SESSION_ID_PERFIL = "IdPerfil";
        public const string SESSION_MENU_HIJO = "MenuHijo";
        //public const string SESSION_CODIGO_PADRE_MENU = "CodigoPadreMenu";
        public const string SESSION_DATOS_EXCEL = "SESSION_DATOS_EXCEL";
        public const string SESSION_DATAREADER_EXCEL = "SESSION_DATAREADER_EXCEL";
        public const string SESSION_DATAREADER_EXCEL_FILTROS = "SESSION_DATAREADER_EXCEL_FILTROS";

        public const string SESSION_DATOS_EXCEL_REPARACIONES = "SESSION_DATOS_EXCEL_REPARACIONES";
        public const string SESSION_DATAREADER_EXCEL_REPARACIONES = "SESSION_DATAREADER_EXCEL_REPARACIONES";
        public const string SESSION_DATAREADER_EXCEL_FILTROS_REPARACIONES = "SESSION_DATAREADER_EXCEL_FILTROS_REPARACIONES";
        //
        public const string SESSION_FILTROS_MENU_DERECHO = "SESSION_FILTROS_MENU_DERECHO";

        public const string SESSION_COD_PROVINCIA = "SESSION_COD_PROVINCIA";

        public const string SESSION_VECES_CLIENTE_AUSENTE = "SESSION_VECES_CLIENTE_AUSENTE";

        public const string SESSION_TITULO_DATOS_EXCEL = "SESSION_TITULO_DATOS_EXCEL";
        public const string SESSION_TITULO_DATOS_EXCEL_REPARACIONES = "SESSION_TITULO_DATOS_EXCEL_REPARACIONES";

        public const string SESSION_LIMITE_DATOS_EXCEL = "SESSION_LIMITE_DATOS_EXCEL";
        public const string SESSION_LIMITE_DATOS_EXCEL_REPARACIONES = "SESSION_LIMITE_DATOS_EXCEL_REPARACIONES";

        public const string SESSION_RUTA_DOCUMENTO_CIERRE_VISITAS = "SESSION_RUTA_DOCUMENTO_CIERRE_VISITAS";

        /// <summary>
        /// Constante con el nombre de la variable de sesión para 
        /// almacenar el identificador del perfil del usuario.
        /// </summary>
        public const string SESSION_USUARIO_CULTURA = "SESSION_USUARIO_CULTURA";

        public const string SESSION_BUSQUEDA_DESDE_DELTA = "SESSION_BUSQUEDA_DESDE_DELTA";

        #region constructor
        /// <summary>
        /// Se pone el constructor como privado para que no se creen instancias de la clase.
        /// </summary>
        private CurrentSession()
        {
        }
        #endregion

        #region metodos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetAttribute(String key)
        {

            if ((HttpContext.Current.Session[key] != null))
            {
                return HttpContext.Current.Session[key];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetMandatoryAttribute(String key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return HttpContext.Current.Session[key];
            }
            else
            {
                throw new ArqException(false, "1016");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void SetAttribute(String key, object obj)
        {
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[key] != null)
                {
                    HttpContext.Current.Session.Remove(key);
                }
                HttpContext.Current.Session.Add(key, obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void SetMandatoryAttribute(String key, object obj)
        {
            if (obj != null)
            {
                CurrentSession.SetAttribute(key, obj);
            }
            else
            {
                throw new ArqException(false, "1015");
            }
        }

        /// <summary>
        /// Elimina un elemento de sesión
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveAttribute(String key)
        {
            if ((HttpContext.Current.Session[key] != null))
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        #endregion
    }
}
