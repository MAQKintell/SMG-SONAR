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
        /// Obtiene todos DocumentoDTO que no estén de baja
        /// </summary>
        /// <returns>Lista de DocumentoDTO con todos los objetos</returns>
        public List<DocumentoDTO> ObtenerNoEnviados()
        {
            IDataReader dr = null;            
            List<DocumentoDTO> lista = null;
            
            try
            {
                using (DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB))
                {
                    lista = new List<DocumentoDTO>();

                    // Hacemos la llamada a la base de datos
                    dr = db.RunProcDataReader("spSMGDocumentoFindNoEnviados", null, null, null);

                    // Recuperamos la lista de objetos encontrados.
                    while (dr.Read())
                    {
                        lista.Add(ObtenerDocumentoDTO(dr));
                    }
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

        public int ObtenerIdTipoDocumento(string codTipoDocumento)
        {
            IDataReader dr = null;
            int idTipoDoc = -1;

            try
            {
                using (DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB))
                {
                    string[] aNombres = new string[1];
                    DbType[] aTipos = new DbType[1];
                    object[] aValores = new object[1];

                    aNombres[0] = "@pCOD_DOCUMENTO";
                    aTipos[0] = DbType.String;
                    aValores[0] = codTipoDocumento;

                    // Hacemos la llamada a la base de datos
                    dr = db.RunProcDataReader("spSMGTipoDocumentoGet", aNombres, aTipos, aValores);

                    // Recuperamos la lista de objetos encontrados.
                    while (dr.Read())
                    {
                        if (DataBaseUtils.HasColumn(dr, "ID_TIPO_DOCUMENTO"))
                        {
                            idTipoDoc = (int)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_DOCUMENTO");
                        }
                    }
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

            // retornamos el ID del objeto buscado, si no se ha encontrato nada se retorna -1
            return idTipoDoc;
        }

        //20200520 BGN ADD BEG R#21754: Colgar facturas en información de reclamación
        public List<DocumentoDTO> ObtenerDocumentosPorIdSolicitud(int idSolicitud)
        {
            List<DocumentoDTO> lista = null;
            IDataReader dr = null;
            try
            {
                lista = new List<DocumentoDTO>();
                using (DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB))
                {
                    string[] aNombres = new string[1];
                    DbType[] aTipos = new DbType[1];
                    object[] aValores = new object[1];

                    aNombres[0] = "@pID_SOLICITUD";
                    aTipos[0] = DbType.Int32;
                    aValores[0] = idSolicitud;

                    dr = db.RunProcDataReader("spSMGDocumentosGetBySolicitud", aNombres, aTipos, aValores);

                    while (dr.Read())
                    {
                        lista.Add(ObtenerDocumentoDTO(dr));
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20200520 BGN ADD END R#21754: Colgar facturas en información de reclamación

        //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
        public List<DocumentoDTO> ObtenerDocumentosPorTicketCombustion(string codContrato, decimal? idSolicitud, int? codVisita)
        {
            List<DocumentoDTO> lista = null;
            IDataReader dr = null;
            try
            {
                lista = new List<DocumentoDTO>();
                using (DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB))
                {
                    string[] aNombres = new string[3];
                    DbType[] aTipos = new DbType[3];
                    object[] aValores = new object[3];

                    aNombres[0] = "@pCOD_CONTRATO";
                    aTipos[0] = DbType.String;
                    aValores[0] = codContrato;

                    aNombres[1] = "@pID_SOLICITUD";
                    aTipos[1] = DbType.Decimal;
                    aValores[1] = idSolicitud;

                    aNombres[2] = "@pCOD_VISITA";
                    aTipos[2] = DbType.Int16;
                    aValores[2] = codVisita;

                    dr = db.RunProcDataReader("spSMGDocumentosGetByTicketCombustion", aNombres, aTipos, aValores);

                    while (dr.Read())
                    {
                        lista.Add(ObtenerDocumentoDTO(dr));
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20201013 BUA END R#23245: Guardar el Ticket de combustión

        //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
        public DocumentoDTO ObtenerPorNombreDocumento(string codContrato, decimal? idSolicitud, int? codVisita, string nombreFichero)
        {
            IDataReader dr = null;
            DocumentoDTO dto = null;

            try
            {
                using (DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB))
                {
                    string[] aNombres = new string[4];
                    DbType[] aTipos = new DbType[4];
                    object[] aValores = new object[4];

                    aNombres[0] = "@pCOD_CONTRATO";
                    aTipos[0] = DbType.String;
                    aValores[0] = codContrato;

                    aNombres[1] = "@pID_SOLICITUD";
                    aTipos[1] = DbType.Decimal;
                    aValores[1] = idSolicitud;

                    aNombres[2] = "@pCOD_VISITA";
                    aTipos[2] = DbType.Int16;
                    aValores[2] = codVisita;

                    aNombres[3] = "@pNOMBRE_DOCUMENTO";
                    aTipos[3] = DbType.String;
                    aValores[3] = nombreFichero;

                    dr = db.RunProcDataReader("spSMGDocumentosGetByNombre", aNombres, aTipos, aValores);

                    if (dr.Read())
                    {
                        dto = ObtenerDocumentoDTO(dr);
                    }

                    return dto;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20201013 BUA END R#23245: Guardar el Ticket de combustión

        //20210111 BGN ADD BEG R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        public List<DocumentoDTO> ObtenerDocumentosPorCodContrato(string codContrato)
        {
            List<DocumentoDTO> lista = null;
            IDataReader dr = null;
            try
            {
                lista = new List<DocumentoDTO>();
                using (DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB))
                {
                    string[] aNombres = new string[1];
                    DbType[] aTipos = new DbType[1];
                    object[] aValores = new object[1];

                    aNombres[0] = "@pCOD_CONTRATO";
                    aTipos[0] = DbType.String;
                    aValores[0] = codContrato;

                    dr = db.RunProcDataReader("spSMGDocumentosGetByContrato", aNombres, aTipos, aValores);

                    while (dr.Read())
                    {
                        lista.Add(ObtenerDocumentoDTO(dr));
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<DocumentoDTO> ObtenerDocumentosGDPorIdSolicitud(int idSolicitud)
        {
            List<DocumentoDTO> lista = null;
            IDataReader dr = null;
            try
            {
                lista = new List<DocumentoDTO>();
                using (DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB))
                {
                    string[] aNombres = new string[1];
                    DbType[] aTipos = new DbType[1];
                    object[] aValores = new object[1];

                    aNombres[0] = "@pID_SOLICITUD";
                    aTipos[0] = DbType.Int32;
                    aValores[0] = idSolicitud;

                    dr = db.RunProcDataReader("spSMGDocumentosGetByIdSolicitud", aNombres, aTipos, aValores);

                    while (dr.Read())
                    {
                        lista.Add(ObtenerDocumentoDTO(dr));
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20210111 BGN ADD END R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
    }
}