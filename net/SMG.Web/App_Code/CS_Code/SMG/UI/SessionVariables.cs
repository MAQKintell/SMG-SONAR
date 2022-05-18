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
using Iberdrola.Commons.Utils;

namespace Iberdrola.SMG.UI
{
    /// <summary>
    /// Descripción breve de SessionVariables
    /// </summary>
    public class SessionVariables
    {
        /// <summary>
        /// Almacenará una lista de contratos para la pantalla de generación del lote.
        /// La lista será List<VisitaDTO>
        /// </summary>
        public const string LISTA_VISITAS_GENERAR_LOTE = "LISTA_VISITAS_GENERAR_LOTE";

        /// <summary>
        /// Almacenara la lista de equipamientos de un contrato
        /// La lista será List<EquipamientoDTO>
        /// </summary>
        public const string LISTA_EQUIPAMIENTO = "LISTA_EQUIPAMIENTO";

        /// <summary>
        /// Almacenara la lista de equipamientos de un contrato que se van a eliminar
        /// La lista será List<EquipamientoDTO>
        /// </summary>
        public const string LISTA_EQUIPAMIENTO_BORRAR = "LISTA_EQUIPAMIENTO_BORRAR";

        /// <summary>
        /// Almacenara el filtro VistaContratoCompletoDTO en la sesion
        /// </summary>
        public const string FILTRO_AVANZADO = "FILTRO_AVANZADO";

        /// <summary>
        /// Clave en la que gusardar si se van a comprobar las condiciones generales del usuario.
        /// <b>True</b> se comprueba las condiciones generales del usuario.
        /// <b>False</b> no se comprueban las condiciones generales del usuario.
        /// </summary>
        public const string SESSION_USUARIO_CONDICIONES = "SESSION_CONDICIONES_GENERALES";

        /// <summary>
        /// Clave en la que guardar si el logeo va a ser contra el LDAP.
        /// <b>True</b> la comprobación se hace contra el LDAP.
        /// <b>False</b> la comprobación se hace contra las tablas de la aplicación. 
        /// </summary>
        public const string SESSION_USUARIO_TIPO_LOGEO = "SESSION_TIPO_LOGEO";

        public const string SESSION_USUARIO_AUTENTIFICADO = "SESSION_USUARIO_AUTENTIFICADO";

        public const string SESSION_AUTENTIFICACION_USUARIO = "SESSION_AUTENTIFICACION_USUARIO";
    }
}
