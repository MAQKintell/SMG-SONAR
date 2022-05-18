//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using Iberdrola.Commons.DataAccess;
using Iberdrola.Commons.Exceptions;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Métodos de acceso a base de datos para la entidad Documento
    /// </summary>
    public partial class DocumentoDB
    {
        /// <summary>
        /// Obtiene el DocumentoDTO que cumple con la PK y no esté de baja
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns>DocumentoDTO que cumple con la PK, null si no lo encuentra.</returns>
        public DocumentoDTO Obtener(decimal idDocumento)
        {
            IDataReader dr = null;
            DocumentoDTO dto = null;
            
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombres = new string[1];
                DbType[] aTipos = new DbType[1];
                object[] aValores = new object[1];
                
                aNombres[0] = "@pID_DOCUMENTO";
                aTipos[0] = DbType.Decimal;
                aValores[0] = idDocumento;

                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spSMGDocumentoGet", aNombres, aTipos, aValores);
                
                // Rebuperamos el objeto buscado
                if (dr.Read())
                {
                    dto = ObtenerDocumentoDTO(dr);
                }
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
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
        /// Obtiene todos DocumentoDTO que no estén de baja
        /// </summary>
        /// <returns>Lista de DocumentoDTO con todos los objetos</returns>
        public List<DocumentoDTO> ObtenerTodos()
        {
            IDataReader dr = null;            
            List<DocumentoDTO> lista = null;
            
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                lista = new List<DocumentoDTO>();

                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spSMGDocumentoGetAll", null, null, null);

                // Recuperamos la lista de objetos encontrados.
                while (dr.Read())
                {
                    lista.Add(ObtenerDocumentoDTO(dr));
                }
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
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
        /// Inserta el DocumentoDTO
        /// </summary>
        /// <param name="dto">Datos a guardar</param>
        /// <param name="strCodUsuario">Usuario que realiza la acción</param>
        /// <returns>DocumentoDTO con la clave autogenerada informada (si la tuviera)</returns>
        public DocumentoDTO Insertar(DocumentoDTO dto, string strCodUsuario)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                
                string[] aNombres = new string[14];
                DbType[] aTipos = new DbType[14];
                object[] aValores = new object[14];
                
                aNombres[0] = "@pCOD_CONTRATO";
                aTipos[0] = DbType.String;
                aValores[0] = dto.CodContrato;
                
                aNombres[1] = "@pCOD_VISITA";
                aTipos[1] = DbType.Int32;
                aValores[1] = dto.CodVisita;
                
                aNombres[2] = "@pID_SOLICITUD";
                aTipos[2] = DbType.Int32;
                aValores[2] = dto.IdSolicitud;
                
                aNombres[3] = "@pNOMBRE_DOCUMENTO";
                aTipos[3] = DbType.String;
                aValores[3] = dto.NombreDocumento;
                
                aNombres[4] = "@pID_TIPO_DOCUMENTO";
                aTipos[4] = DbType.Int32;
                aValores[4] = dto.IdTipoDocumento;
                
                aNombres[5] = "@pFECHA_BAJA";
                aTipos[5] = DbType.DateTime;
                aValores[5] = dto.FechaBaja;
                
                aNombres[6] = "@pFECHA_ENVIO_DELTA";
                aTipos[6] = DbType.DateTime;
                aValores[6] = dto.FechaEnvioDelta;
                
                aNombres[7] = "@pCodUsuario";
                aTipos[7] = DbType.String;
                aValores[7] = strCodUsuario;

                aNombres[8] = "@pENVIAR_A_DELTA";
                aTipos[8] = DbType.Boolean;
                aValores[8] = dto.EnviarADelta;

                aNombres[9] = "@pID_ENVIO_EDATALIA";
                aTipos[9] = DbType.String;
                aValores[9] = dto.IdEnvioEdatalia;

                aNombres[10] = "@pFECHA_ENVIO_EDATALIA";
                aTipos[10] = DbType.DateTime;
                aValores[10] = dto.FechaEnvioEdatalia;

                aNombres[11] = "@pID_DOC_EDATALIA";
                aTipos[11] = DbType.String;
                aValores[11] = dto.IdDocEdatalia;

                aNombres[12] = "@pID_ESTADO_EDATALIA";
                aTipos[12] = DbType.Int32;
                aValores[12] = dto.IdEstadoEdatalia;

                aNombres[13] = "@pFECHA_RECIBIDO_EDATALIA";
                aTipos[13] = DbType.DateTime;
                aValores[13] = dto.FechaRecibidoEdatalia;


                // Hacemos la llamada a la base de datos
                object result = db.RunProcEscalar("spSMGDocumentoInsert", aNombres, aTipos, aValores);
                dto.IdDocumento = (decimal)result;

            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005");
            }
            return dto;
        }
                
        /// <summary>
        /// Actualiza el DocumentoDTO
        /// </summary>
        /// <param name="dto">Datos a actualizar</param>
        /// <param name="strCodUsuario">Usuario que realiza la acción</param>
        /// <returns>int con el número de filas actualizadas</returns>
        public int Actualizar(DocumentoDTO dto, string strCodUsuario)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                
                string[] aNombres = new string[15];
                DbType[] aTipos = new DbType[15];
                object[] aValores = new object[15];
                
                aNombres[0] = "@pID_DOCUMENTO";
                aTipos[0] = DbType.Decimal;
                aValores[0] = dto.IdDocumento;
                
                aNombres[1] = "@pCOD_CONTRATO";
                aTipos[1] = DbType.String;
                aValores[1] = dto.CodContrato;
                
                aNombres[2] = "@pCOD_VISITA";
                aTipos[2] = DbType.Int32;
                aValores[2] = dto.CodVisita;
                
                aNombres[3] = "@pID_SOLICITUD";
                aTipos[3] = DbType.Int32;
                aValores[3] = dto.IdSolicitud;
                
                aNombres[4] = "@pNOMBRE_DOCUMENTO";
                aTipos[4] = DbType.String;
                aValores[4] = dto.NombreDocumento;
                
                aNombres[5] = "@pID_TIPO_DOCUMENTO";
                aTipos[5] = DbType.Int32;
                aValores[5] = dto.IdTipoDocumento;
                
                aNombres[6] = "@pFECHA_BAJA";
                aTipos[6] = DbType.DateTime;
                aValores[6] = dto.FechaBaja;
                
                aNombres[7] = "@pFECHA_ENVIO_DELTA";
                aTipos[7] = DbType.DateTime;
                aValores[7] = dto.FechaEnvioDelta;
                
                aNombres[8] = "@pCodUsuario";
                aTipos[8] = DbType.String;
                aValores[8] = strCodUsuario;

                aNombres[9] = "@pENVIAR_A_DELTA";
                aTipos[9] = DbType.Boolean;
                aValores[9] = dto.EnviarADelta;

                aNombres[10] = "@pID_ENVIO_EDATALIA";
                aTipos[10] = DbType.String;
                aValores[10] = dto.IdEnvioEdatalia;

                aNombres[11] = "@pFECHA_ENVIO_EDATALIA";
                aTipos[11] = DbType.DateTime;
                aValores[11] = dto.FechaEnvioEdatalia;

                aNombres[12] = "@pID_DOC_EDATALIA";
                aTipos[12] = DbType.String;
                aValores[12] = dto.IdDocEdatalia;

                aNombres[13] = "@pID_ESTADO_EDATALIA";
                aTipos[13] = DbType.Int32;
                aValores[13] = dto.IdEstadoEdatalia;

                aNombres[14] = "@pFECHA_RECIBIDO_EDATALIA";
                aTipos[14] = DbType.DateTime;
                aValores[14] = dto.FechaRecibidoEdatalia;

                // Hacemos la llamada a la base de datos
                object result = db.RunProcEscalar("spSMGDocumentoUpdate", aNombres, aTipos, aValores);
                
                // Retornamos el número de filas afectadas.
                return (int) result;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005");
            }
        }
        
        /// <summary>
        /// Elimina el DocumentoDTO. si el objeto tiene un campo de fecha baja
        /// para hacer bajas lógicas realiza un borrado lógico
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <param name="strCodUsuario">Usuario que realiza la acción</param>
        /// <returns>int con el número de filas eliminadas</returns>
        public int Eliminar(decimal idDocumento, string strCodUsuario)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombres = new string[2];
                DbType[] aTipos = new DbType[2];
                object[] aValores = new object[2];
                
                aNombres[0] = "@pID_DOCUMENTO";
                aTipos[0] = DbType.Decimal;
                aValores[0] = idDocumento;
                
                aNombres[1] = "@pCodUsuario";
                aTipos[1] = DbType.String;
                aValores[1] = strCodUsuario;
                
                

                // Hacemos la llamada a la base de datos
                object result = db.RunProcEscalar("spSMGDocumentoDelete", aNombres, aTipos, aValores);

                // Retornamos el número de filas afectadas.
                return (int) result;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
            }
        }

        /// <summary>
        /// Obtiene los DocumentoDTO que cumple con la el criterio de búsqueda y no está de baja
        /// </summary>
        /// <param name="dtoFiltro">Filtro de Documento a aplicar</param>
        /// <param name="paginacion">Datos de la paginación</param>
        /// <returns>Lista de DocumentoDTO que cumplen con los filtros indicados.</returns>
        public List<DocumentoDTO> Buscar(DocumentoDTO dtoFiltro, Pagination paginacion)
        {

            IDataReader dr = null;            
            List<DocumentoDTO> lista = null;
            
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                lista = new List<DocumentoDTO>();

                string[] aNombres = new string[16];
                DbType[] aTipos = new DbType[16];
                object[] aValores = new object[16];
                
                aNombres[0] = "@pCOD_CONTRATO";
                aTipos[0] = DbType.String;
                aValores[0] = dtoFiltro.CodContrato;
                
                aNombres[1] = "@pCOD_VISITA";
                aTipos[1] = DbType.Int32;
                aValores[1] = dtoFiltro.CodVisita;
                
                aNombres[2] = "@pID_SOLICITUD";
                aTipos[2] = DbType.Int32;
                aValores[2] = dtoFiltro.IdSolicitud;
                
                aNombres[3] = "@pNOMBRE_DOCUMENTO";
                aTipos[3] = DbType.String;
                aValores[3] = dtoFiltro.NombreDocumento;
                
                aNombres[4] = "@pID_TIPO_DOCUMENTO";
                aTipos[4] = DbType.Int32;
                aValores[4] = dtoFiltro.IdTipoDocumento;
                
                aNombres[5] = "@pFECHA_BAJA";
                aTipos[5] = DbType.DateTime;
                aValores[5] = dtoFiltro.FechaBaja;
                
                aNombres[6] = "@pFECHA_ENVIO_DELTA";
                aTipos[6] = DbType.DateTime;
                aValores[6] = dtoFiltro.FechaEnvioDelta;
                
                aNombres[7] = "@pPAGE_INDEX";
                aTipos[7] = DbType.Int32;
                aValores[7] = paginacion.PaginaActual;
                
                aNombres[8] = "@pSORT_FIELD";
                aTipos[8] = DbType.String;
                aValores[8] = paginacion.CampoOrden;
                
                aNombres[9] = "@pSORT_ORDER";
                aTipos[9] = DbType.String;
                aValores[9] = paginacion.DireccionOrden;

                aNombres[10] = "@pENVIAR_A_DELTA";
                aTipos[10] = DbType.Boolean;
                aValores[10] = dtoFiltro.EnviarADelta;

                aNombres[11] = "@pID_ENVIO_EDATALIA";
                aTipos[11] = DbType.String;
                aValores[11] = dtoFiltro.IdEnvioEdatalia;

                aNombres[12] = "@pFECHA_ENVIO_EDATALIA";
                aTipos[12] = DbType.DateTime;
                aValores[12] = dtoFiltro.FechaEnvioEdatalia;

                aNombres[13] = "@pID_DOC_EDATALIA";
                aTipos[13] = DbType.String;
                aValores[13] = dtoFiltro.IdDocEdatalia;

                aNombres[14] = "@pID_ESTADO_EDATALIA";
                aTipos[14] = DbType.Int32;
                aValores[14] = dtoFiltro.IdEstadoEdatalia;

                aNombres[15] = "@pFECHA_RECIBIDO_EDATALIA";
                aTipos[15] = DbType.DateTime;
                aValores[15] = dtoFiltro.FechaRecibidoEdatalia;


                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spSMGDocumentoFind", aNombres, aTipos, aValores);

                // Recuperamos la lista de objetos encontrados.
                while (dr.Read())
                {
                    lista.Add(ObtenerDocumentoDTO(dr));
                }
                return lista;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
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
        /// Carga los datos del IDataReader en un objeto DocumentoDTO
        /// </summary>
        /// <param name="dr">DataReader con los datos obtenidos de la BBDD</param>
        /// <returns>DocumentoDTO con los datos cargados</returns>
        private DocumentoDTO ObtenerDocumentoDTO(IDataReader dr)
        {
            DocumentoDTO dto = new DocumentoDTO();
            if (DataBaseUtils.HasColumn(dr, "ID_DOCUMENTO"))
            {
				dto.IdDocumento = (decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_DOCUMENTO");
            }         
			if (DataBaseUtils.HasColumn(dr, "COD_CONTRATO"))
            {
				dto.CodContrato = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_CONTRATO");
            }         
			if (DataBaseUtils.HasColumn(dr, "COD_VISITA"))
            {
				dto.CodVisita = (int?)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_VISITA");
            }         
			if (DataBaseUtils.HasColumn(dr, "ID_SOLICITUD"))
            {
				dto.IdSolicitud = (int?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_SOLICITUD");
            }         
			if (DataBaseUtils.HasColumn(dr, "NOMBRE_DOCUMENTO"))
            {
				dto.NombreDocumento = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE_DOCUMENTO");
            }         
			if (DataBaseUtils.HasColumn(dr, "ID_TIPO_DOCUMENTO"))
            {
				dto.IdTipoDocumento = (int?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_DOCUMENTO");
            }         
			if (DataBaseUtils.HasColumn(dr, "FECHA_BAJA"))
            {
				dto.FechaBaja = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_BAJA");
            }         
			if (DataBaseUtils.HasColumn(dr, "FECHA_ENVIO_DELTA"))
            {
				dto.FechaEnvioDelta = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_ENVIO_DELTA");
            }

            //20210111 BGN ADD BEG R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
            if (DataBaseUtils.HasColumn(dr, "COD_DOCUMENTO"))
            {
                dto.CodTipoDocumento = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_DOCUMENTO");
            }
            if (DataBaseUtils.HasColumn(dr, "DESC_DOCUMENTO"))
            {
                dto.DescTipoDocumento = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_DOCUMENTO");
            }
            //20210111 BGN ADD END R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental

            //20210118 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
            if (DataBaseUtils.HasColumn(dr, "ENVIAR_A_DELTA"))
            {
                dto.EnviarADelta = (bool)DataBaseUtils.GetDataReaderColumnValue(dr, "ENVIAR_A_DELTA");
            }
            if (DataBaseUtils.HasColumn(dr, "ID_ENVIO_EDATALIA"))
            {
                dto.IdEnvioEdatalia = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_ENVIO_EDATALIA");
            }
            if (DataBaseUtils.HasColumn(dr, "FECHA_ENVIO_EDATALIA"))
            {
                dto.FechaEnvioEdatalia = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_ENVIO_EDATALIA");
            }
            if (DataBaseUtils.HasColumn(dr, "ID_DOC_EDATALIA"))
            {
                dto.IdDocEdatalia = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_DOC_EDATALIA");
            }
            if (DataBaseUtils.HasColumn(dr, "ID_ESTADO_EDATALIA"))
            {
                dto.IdEstadoEdatalia = (int?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_ESTADO_EDATALIA");
            }
            if (DataBaseUtils.HasColumn(dr, "FECHA_RECIBIDO_EDATALIA"))
            {
                dto.FechaRecibidoEdatalia = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_RECIBIDO_EDATALIA");
            }
            //20210118 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital

            return dto;
        }

    }
}