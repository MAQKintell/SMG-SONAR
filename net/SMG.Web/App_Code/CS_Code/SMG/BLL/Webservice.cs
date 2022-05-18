using System.Collections.Generic;
using Iberdrola.Commons.DataAccess;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Métodos de acceso a base de datos para la entidad PWebservice
    /// </summary>
    public partial class Webservice
    {
        /// <summary>
        /// Obtiene el WebserviceDTO que cumple con la PK y no esté de baja
        /// </summary>
        /// <param name="idWebservice">INT( Interaccion) , LLP(Llamada Proveedor)</param>
        /// <returns>PWebserviceDTO que cumple con la PK, null si no lo encuentra.</returns>
        public static WebserviceDTO Obtener(decimal idWebservice)
        {
            WebserviceDB db = new WebserviceDB();
            return db.Obtener(idWebservice);
        }

        /// <summary>
        /// Obtiene todos PWebserviceDTO que no estén de baja
        /// </summary>
        /// <returns>Lista de PWebserviceDTO con todos los objetos</returns>
        public static List<WebserviceDTO> ObtenerTodos()
        {
            WebserviceDB db = new WebserviceDB();
            return db.ObtenerTodos();
        }

        /// <summary>
        /// Obtiene los PWebserviceDTO que cumple con la el criterio de búsqueda y no está de baja
        /// </summary>
        /// <param name="dtoFiltro">Filtro de PWebservice a aplicar</param>
        /// <returns>Lista de PWebserviceDTO que cumplen con los filtros indicados.</returns>
        public static List<WebserviceDTO> Buscar(WebserviceDTO dtoFiltro)
        {
            WebserviceDB db = new WebserviceDB();
            return db.Buscar(dtoFiltro);
        }

    }
}