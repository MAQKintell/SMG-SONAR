//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System.Collections.Generic;
using Iberdrola.Commons.DataAccess;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Métodos de acceso a base de datos para la entidad Documento
    /// </summary>
    public partial class Documento
    {
        /// <summary>
        /// Obtiene el DocumentoDTO que cumple con la PK y no esté de baja
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns>DocumentoDTO que cumple con la PK, null si no lo encuentra.</returns>
        public static DocumentoDTO Obtener(decimal idDocumento)
        {
            DocumentoDB db = new DocumentoDB();
            return db.Obtener(idDocumento);
        }
        
        /// <summary>
        /// Obtiene todos DocumentoDTO que no estén de baja
        /// </summary>
        /// <returns>Lista de DocumentoDTO con todos los objetos</returns>
        public static List<DocumentoDTO> ObtenerTodos()
        {
            DocumentoDB db = new DocumentoDB();
            return db.ObtenerTodos();
        }
        
        /// <summary>
        /// Inserta el DocumentoDTO
        /// </summary>
        /// <param name="dto">Datos a guardar</param>
        /// <param name="strCodUsuario">Usuario que realiza la acción</param>
        /// <returns>DocumentoDTO con la clave autogenerada informada (si la tuviera)</returns>
        public static DocumentoDTO Insertar(DocumentoDTO dto, string strCodUsuario)
        {
            DocumentoDB db = new DocumentoDB();
            return db.Insertar(dto, strCodUsuario);
        }
        
        /// <summary>
        /// Actualiza el DocumentoDTO
        /// </summary>
        /// <param name="dto">Datos a actualizar</param>
        /// <param name="strCodUsuario">Usuario que realiza la acción</param>
        /// <returns>int con el número de filas actualizadas</returns>
        public static int Actualizar(DocumentoDTO dto, string strCodUsuario)
        {
            DocumentoDB db = new DocumentoDB();
            return db.Actualizar(dto, strCodUsuario);
        }
        
        /// <summary>
        /// Elimina el DocumentoDTO. si el objeto tiene un campo de fecha baja
        /// para hacer bajas lógicas realiza un borrado lógico
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <param name="strCodUsuario">Usuario que realiza la acción</param>
        /// <returns>int con el número de filas eliminadas</returns>
        public static int Eliminar(decimal idDocumento, string strCodUsuario)
        {
            DocumentoDB db = new DocumentoDB();
            return db.Eliminar(idDocumento, strCodUsuario);
        }

        /// <summary>
        /// Obtiene los DocumentoDTO que cumple con la el criterio de búsqueda y no está de baja
        /// </summary>
        /// <param name="dtoFiltro">Filtro de Documento a aplicar</param>
        /// <param name="paginacion">Datos de la paginación</param>
        /// <returns>Lista de DocumentoDTO que cumplen con los filtros indicados.</returns>
        public static List<DocumentoDTO> Buscar(DocumentoDTO dtoFiltro, Pagination paginacion)
        {
            DocumentoDB db = new DocumentoDB();
            return db.Buscar(dtoFiltro, paginacion);
        } 

          
    }
}