using System;
using System.Collections.Generic;
using System.Data;
using Iberdrola.Commons.BaseClasses;
using Iberdrola.Commons.DataAccess;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Messages;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Métodos de acceso a base de datos para la entidad PWebservice
    /// </summary>
    public partial class WebserviceDB
    {
        /// <summary>
        /// Obtiene el PWebserviceDTO que cumple con la PK y no esté de baja
        /// </summary>
        /// <param name="idWebservice">INT( Interaccion) , LLP(Llamada Proveedor)</param>
        /// <returns>PWebserviceDTO que cumple con la PK, null si no lo encuentra.</returns>
        public WebserviceDTO Obtener(decimal idWebservice)
        {
            IDataReader dr = null;
            WebserviceDTO dto = null;
            
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombres = new string[1];
                DbType[] aTipos = new DbType[1];
                object[] aValores = new object[1];
                
                aNombres[0] = "@pid_WebService";
                aTipos[0] = DbType.Decimal;
                aValores[0] = idWebservice;
                
                

                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spSMGPWebserviceGet", aNombres, aTipos, aValores);
                
                // Rebuperamos el objeto buscado
                if (dr.Read())
                {
                    dto = ObtenerPWebserviceDTO(dr);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
            
            // retornamos el objeto buscado, si no se ha encontrato nada se retorna null
            return dto;
        }
        
        /// <summary>
        /// Obtiene todos PWebserviceDTO que no estén de baja
        /// </summary>
        /// <returns>Lista de PWebserviceDTO con todos los objetos</returns>
        public List<WebserviceDTO> ObtenerTodos()
        {
            IDataReader dr = null;            
            List<WebserviceDTO> lista = null;
            
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                lista = new List<WebserviceDTO>();

                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spSMGPWebserviceGetAll", null, null, null);

                // Recuperamos la lista de objetos encontrados.
                while (dr.Read())
                {
                    lista.Add(ObtenerPWebserviceDTO(dr));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }

            // retornamos la lista de objetos buscados, si no se ha encontrato nada se retorna null
            return lista;
        }
        
        /// <summary>
        /// Obtiene los PWebserviceDTO que cumple con la el criterio de búsqueda y no está de baja
        /// </summary>
        /// <param name="dtoFiltro">Filtro de PWebservice a aplicar</param>
        /// <returns>Lista de PWebserviceDTO que cumplen con los filtros indicados.</returns>
        public List<WebserviceDTO> Buscar(WebserviceDTO dtoFiltro)
        {

            IDataReader dr = null;            
            List<WebserviceDTO> lista = null;
            
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                lista = new List<WebserviceDTO>();

                string[] aNombres = new string[9];
                DbType[] aTipos = new DbType[9];
                object[] aValores = new object[9];
                
                aNombres[0] = "@pNombre_WebService";
                aTipos[0] = DbType.String;
                aValores[0] = dtoFiltro.NombreWebservice;
                
                aNombres[1] = "@pURL_WebService";
                aTipos[1] = DbType.String;
                aValores[1] = dtoFiltro.UrlWebservice;
                
                aNombres[2] = "@pMetodo_WebService";
                aTipos[2] = DbType.String;
                aValores[2] = dtoFiltro.MetodoWebservice;
                
                aNombres[3] = "@pCod_Proveedor_WebService";
                aTipos[3] = DbType.String;
                aValores[3] = dtoFiltro.CodProveedorWebservice;
                
                aNombres[4] = "@pUsuario_WebService";
                aTipos[4] = DbType.String;
                aValores[4] = dtoFiltro.UsuarioWebservice;
                
                aNombres[5] = "@pPassword_WebService";
                aTipos[5] = DbType.String;
                aValores[5] = dtoFiltro.PasswordWebservice;
                
                aNombres[6] = "@pPlantilla_WebService";
                aTipos[6] = DbType.String;
                aValores[6] = dtoFiltro.PlantillaWebservice;
                
                aNombres[7] = "@pTipo_WebService";
                aTipos[7] = DbType.String;
                aValores[7] = dtoFiltro.TipoWebservice;
                
                aNombres[8] = "@pActivo_WebService";
                aTipos[8] = DbType.Boolean;
                aValores[8] = dtoFiltro.ActivoWebservice;
                

                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spSMGPWebserviceFind", aNombres, aTipos, aValores);

                // Recuperamos la lista de objetos encontrados.
                while (dr.Read())
                {
                    lista.Add(ObtenerPWebserviceDTO(dr));
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }            
        }

        /// <summary>
        /// Carga los datos del IDataReader en un objeto PWebserviceDTO
        /// </summary>
        /// <param name="dr">DataReader con los datos obtenidos de la BBDD</param>
        /// <returns>PWebserviceDTO con los datos cargados</returns>
        private WebserviceDTO ObtenerPWebserviceDTO(IDataReader dr)
        {
            WebserviceDTO dto = new WebserviceDTO();
            if (DataBaseUtils.HasColumn(dr, "id_WebService"))
            {
				dto.IdWebservice = (decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "id_WebService");
            }         
			if (DataBaseUtils.HasColumn(dr, "Nombre_WebService"))
            {
				dto.NombreWebservice = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "Nombre_WebService");
            }         
			if (DataBaseUtils.HasColumn(dr, "URL_WebService"))
            {
				dto.UrlWebservice = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "URL_WebService");
            }         
			if (DataBaseUtils.HasColumn(dr, "Metodo_WebService"))
            {
				dto.MetodoWebservice = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "Metodo_WebService");
            }         
			if (DataBaseUtils.HasColumn(dr, "Cod_Proveedor_WebService"))
            {
				dto.CodProveedorWebservice = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "Cod_Proveedor_WebService");
            }         
			if (DataBaseUtils.HasColumn(dr, "Usuario_WebService"))
            {
				dto.UsuarioWebservice = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "Usuario_WebService");
            }         
			if (DataBaseUtils.HasColumn(dr, "Password_WebService"))
            {
				dto.PasswordWebservice = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "Password_WebService");
            }         
			if (DataBaseUtils.HasColumn(dr, "Plantilla_WebService"))
            {
				dto.PlantillaWebservice = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "Plantilla_WebService");
            }         
			if (DataBaseUtils.HasColumn(dr, "Tipo_WebService"))
            {
				dto.TipoWebservice = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "Tipo_WebService");
            }         
			if (DataBaseUtils.HasColumn(dr, "Activo_WebService"))
            {
				dto.ActivoWebservice = (bool)DataBaseUtils.GetDataReaderColumnValue(dr, "Activo_WebService");
            }

            if (DataBaseUtils.HasColumn(dr, "ProxyActivo_WebService"))
            {
                dto.ProxyActivoWebService = (bool)DataBaseUtils.GetDataReaderColumnValue(dr, "ProxyActivo_WebService");
            }      
			
            return dto;
        }

    }
}