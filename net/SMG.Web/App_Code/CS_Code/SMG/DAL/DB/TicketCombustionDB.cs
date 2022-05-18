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
using Iberdrola.SMG.DAL;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.DataAccess;
using Iberdrola.Commons.Exceptions;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de UsuarioDB
    /// </summary>
    public class TicketCombustionDB
    {
        public TicketCombustionDB()
	    {

	    }

        /// <summary>
        /// Obtiene el TicketCombustionDTO que cumple con la PK y no esté de baja
        /// </summary>
        /// <param name="idTicketCombustion"></param>
        /// <returns>TicketCombustionDTO que cumple con la PK, null si no lo encuentra.</returns>
        public TicketCombustionDTO Obtener(decimal idTicketCombustion)
        {
            IDataReader dr = null;
            TicketCombustionDTO dto = null;

            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombres = new string[1];
                DbType[] aTipos = new DbType[1];
                object[] aValores = new object[1];

                aNombres[0] = "@pID_TICKET_COMBUSTION";
                aTipos[0] = DbType.Decimal;
                aValores[0] = idTicketCombustion;



                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spSMGTicketCombustionGet", aNombres, aTipos, aValores);

                // Rebuperamos el objeto buscado
                if (dr.Read())
                {
                    dto = ObtenerTicketCombustionDTO(dr);
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
        /// Obtiene todos TicketCombustionDTO que no estén de baja
        /// </summary>
        /// <returns>Lista de TicketCombustionDTO con todos los objetos</returns>
        public List<TicketCombustionDTO> ObtenerTodos()
        {
            IDataReader dr = null;
            List<TicketCombustionDTO> lista = null;

            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                lista = new List<TicketCombustionDTO>();

                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spGESVULTicketCombustionGetAll", null, null, null);

                // Recuperamos la lista de objetos encontrados.
                while (dr.Read())
                {
                    lista.Add(ObtenerTicketCombustionDTO(dr));
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
        /// Guardar el TicketCombustionDTO
        /// </summary>
        /// <param name="dto">Datos a guardar</param>
        /// <param name="codUsuario">Nombre de usuario que registra la peticion</param>
        /// <returns>TicketCombustionDTO con la clave autogenerada informada (si la tuviera)</returns>
        public TicketCombustionDTO GuardarTicketCombustion(TicketCombustionDTO dto, string codUsuario)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombres = new string[22];
                DbType[] aTipos = new DbType[22];
                object[] aValores = new object[22];

                aNombres[0] = "@pCOD_CONTRATO";
                aTipos[0] = DbType.String;
                aValores[0] = dto.CodigoContrato;

                aNombres[1] = "@pID_SOLICITUD";
                aTipos[1] = DbType.Decimal;
                aValores[1] = dto.IdSolicitud;

                aNombres[2] = "@pCOD_VISITA";
                aTipos[2] = DbType.Int32;
                aValores[2] = dto.CodigoVisita;

                aNombres[3] = "@pTIPO_EQUIPO";
                aTipos[3] = DbType.Int16;
                aValores[3] = dto.TipoEquipo;

                aNombres[4] = "@pTEMPERATURA_PDC";
                aTipos[4] = DbType.Decimal;
                aValores[4] = dto.TemperaturaPDC;

                aNombres[5] = "@pCO_CORREGIDO";
                aTipos[5] = DbType.Decimal;
                aValores[5] = dto.COCorregido;

                aNombres[6] = "@pTIRO";
                aTipos[6] = DbType.Decimal;
                aValores[6] = dto.Tiro;

                aNombres[7] = "@pCO_AMBIENTE";
                aTipos[7] = DbType.Decimal;
                aValores[7] = dto.COAmbiente;

                aNombres[8] = "@pO2";
                aTipos[8] = DbType.Decimal;
                aValores[8] = dto.O2;

                aNombres[9] = "@pCO";
                aTipos[9] = DbType.Decimal;
                aValores[9] = dto.CO;

                aNombres[10] = "@pCO2";
                aTipos[10] = DbType.Decimal;
                aValores[10] = dto.CO2;

                aNombres[11] = "@pLAMBDA";
                aTipos[11] = DbType.Decimal;
                aValores[11] = dto.Lambda;

                aNombres[12] = "@pRENDIMIENTO";
                aTipos[12] = DbType.Decimal;
                aValores[12] = dto.Rendimiento;

                aNombres[13] = "@pTEMPERATURA_MAX_ACS";
                aTipos[13] = DbType.Decimal;
                aValores[13] = dto.TemperaturaMaxACS;

                aNombres[14] = "@pCAUDAL_ACS";
                aTipos[14] = DbType.Decimal;
                aValores[14] = dto.CaudalACS;

                aNombres[15] = "@pPOTENCIA_UTIL";
                aTipos[15] = DbType.Decimal;
                aValores[15] = dto.PotenciaUtil;

                aNombres[16] = "@pTEMPERATURA_ENTRADA_ACS";
                aTipos[16] = DbType.Decimal;
                aValores[16] = dto.TemperaturaEntradaACS;

                aNombres[17] = "@pTEMPERATURA_SALIDA_ACS";
                aTipos[17] = DbType.Decimal;
                aValores[17] = dto.TemperaturaSalidaACS;

                aNombres[18] = "@pID_FICHERO_TICKET_COMBUSTION";
                aTipos[18] = DbType.Int64;
                aValores[18] = dto.IdFicheroTicketCombustion;

                aNombres[19] = "@pID_FICHERO_CONDUCTO_HUMOS";
                aTipos[19] = DbType.Int64;
                aValores[19] = dto.IdFicheroConductoHumos;

                aNombres[20] = "@pCOMENTARIOS";
                aTipos[20] = DbType.String;
                aValores[20] = dto.Comentarios;

                aNombres[21] = "@pCOD_USUARIO";
                aTipos[21] = DbType.String;
                aValores[21] = codUsuario;


                // Hacemos la llamada a la base de datos
                object result = db.RunProcEscalar("spSMGTicketCombustionInsertUpdate", aNombres, aTipos, aValores);
                dto.IdTicketCombustion = Convert.ToDecimal(result);

            }
            catch (BaseException ex)
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
        /// Obtiene el TicketCombustionDTO que cumple con la PK y no esté de baja
        /// </summary>
        /// <param name="codContrato"></param>
        /// <param name="codVisita"></param>
        /// <returns>TicketCombustionDTO que cumple con la PK, null si no lo encuentra.</returns>
        public List<TicketCombustionDTO> ObtenerPorCodContratoYCodvisitaOIdSolicitud(string codContrato, decimal? idSolicitud, int? codVisita)
        {
            IDataReader dr = null;
            List<TicketCombustionDTO> lista = new List<TicketCombustionDTO>();

            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

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

                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spSMGTicketCombustionGetByCodContrato", aNombres, aTipos, aValores);

                // Recuperamos la lista de objetos encontrados.
                while (dr.Read())
                {
                    lista.Add(ObtenerTicketCombustionDTO(dr));
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
            return lista;
        }

        /// <summary>
        /// Obtiene los ticket combustion para su visualizacion en un grid view
        /// </summary>
        /// <param name="codContrato"></param>
        /// <param name="idSolicitud"></param>
        /// <param name="codVisita"></param>
        /// <returns>IDataReader que cumple con la PK, null si no lo encuentra.</returns>
        public DataTable dtObtenerPorCodContratoYCodvisitaOIdSolicitud(string codContrato, decimal? idSolicitud, int? codVisita)
        {
            DataTable dt = null;

            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

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

                // Hacemos la llamada a la base de datos
                dt = db.RunProcDataTable("spSMGTicketCombustionGetByCodContrato", aNombres, aTipos, aValores);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
            }

            // retornamos el objeto buscado, si no se ha encontrato nada se retorna null
            return dt;
        }

        /// <summary>
        /// Obtiene los ticket combustion para su visualizacion en un grid view
        /// </summary>
        /// <param name="codContrato"></param>
        /// <param name="idSolicitud"></param>
        /// <param name="codVisita"></param>
        /// <param name="idioma"></param>
        /// <returns>IDataReader que cumple con la PK, null si no lo encuentra.</returns>
        public IDataReader ObtenerTodosPorCodContratoGridView(string codContrato, decimal? idSolicitud, int? codVisita, int idioma)
        {
            IDataReader dr = null;

            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

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

                aNombres[3] = "@pID_IDIOMA";
                aTipos[3] = DbType.Int16;
                aValores[3] = idioma;

                // Hacemos la llamada a la base de datos
                dr = db.RunProcDataReader("spSMGTicketCombustionGetAllGridView", aNombres, aTipos, aValores);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
            }
            //finally
            //{
            //    if (dr != null)
            //    {
            //        dr.Close();
            //    }
            //}

            // retornamos el objeto buscado, si no se ha encontrato nada se retorna null
            return dr;
        }

        /// <summary>
        /// Obtenemos una lista de objetos de tipo TipoCalderaDTO correspondiente a los ticket de combustion
        /// </summary>
        /// <param name="IdIdioma">Idioma correspondiente al tipo de calcera</param>
        /// <param name="tipoPeticion">Tipo de peticion correspondiente al web service</param>
        /// <returns>Devuelve lista de TipoCalderaDTO</returns>
        public static List<TipoCalderaDTO> ObtenerTiposCalderaTicketCombustion(Int16 IdIdioma, string tipoPeticion)
        {
            try
            {
                TipoCalderaDB tipoDB = new TipoCalderaDB();
                return tipoDB.ObtenerTiposCalderaTicketCombustion(IdIdioma, tipoPeticion);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException(false, ex, "2004");
            }
        }

        /// <summary>
        /// Carga los datos del IDataReader en un objeto TicketCombustionDTO
        /// </summary>
        /// <param name="dr">DataReader con los datos obtenidos de la BBDD</param>
        /// <returns>TicketCombustionDTO con los datos cargados</returns>
        private TicketCombustionDTO ObtenerTicketCombustionDTO(IDataReader dr)
        {
            TicketCombustionDTO dto = new TicketCombustionDTO();
            if (DataBaseUtils.HasColumn(dr, "ID_TICKET_COMBUSTION"))
            {
                dto.IdTicketCombustion = (decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TICKET_COMBUSTION");
            }
            if (DataBaseUtils.HasColumn(dr, "COD_CONTRATO"))
            {
                dto.CodigoContrato = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_CONTRATO");
            }
            if (DataBaseUtils.HasColumn(dr, "ID_SOLICITUD"))
            {
                dto.IdSolicitud = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_SOLICITUD");
            }
            if (DataBaseUtils.HasColumn(dr, "COD_VISITA"))
            {
                dto.CodigoVisita = (int?)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_VISITA");
            }
            if (DataBaseUtils.HasColumn(dr, "TIPO_EQUIPO"))
            {
                dto.TipoEquipo = (int?)DataBaseUtils.GetDataReaderColumnValue(dr, "TIPO_EQUIPO");
            }
            if (DataBaseUtils.HasColumn(dr, "TEMPERATURA_PDC"))
            {
                dto.TemperaturaPDC = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "TEMPERATURA_PDC");
            }
            if (DataBaseUtils.HasColumn(dr, "CO_CORREGIDO"))
            {
                dto.COCorregido = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "CO_CORREGIDO");
            }
            if (DataBaseUtils.HasColumn(dr, "TIRO"))
            {
                dto.Tiro = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "TIRO");
            }
            if (DataBaseUtils.HasColumn(dr, "CO_AMBIENTE"))
            {
                dto.COAmbiente = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "CO_AMBIENTE");
            }
            if (DataBaseUtils.HasColumn(dr, "O2"))
            {
                dto.O2 = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "O2");
            }
            if (DataBaseUtils.HasColumn(dr, "CO"))
            {
                dto.CO = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "CO");
            }
            if (DataBaseUtils.HasColumn(dr, "CO2"))
            {
                dto.CO2 = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "CO2");
            }
            if (DataBaseUtils.HasColumn(dr, "LAMBDA"))
            {
                dto.Lambda = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "LAMBDA");
            }
            if (DataBaseUtils.HasColumn(dr, "RENDIMIENTO"))
            {
                dto.Rendimiento = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "RENDIMIENTO");
            }
            if (DataBaseUtils.HasColumn(dr, "TEMPERATURA_MAX_ACS"))
            {
                dto.TemperaturaMaxACS = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "TEMPERATURA_MAX_ACS");
            }
            if (DataBaseUtils.HasColumn(dr, "CAUDAL_ACS"))
            {
                dto.CaudalACS = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "CAUDAL_ACS");
            }
            if (DataBaseUtils.HasColumn(dr, "POTENCIA_UTIL"))
            {
                dto.PotenciaUtil = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "POTENCIA_UTIL");
            }
            if (DataBaseUtils.HasColumn(dr, "TEMPERATURA_ENTRADA_ACS"))
            {
                dto.TemperaturaEntradaACS = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "TEMPERATURA_ENTRADA_ACS");
            }
            if (DataBaseUtils.HasColumn(dr, "TEMPERATURA_SALIDA_ACS"))
            {
                dto.TemperaturaSalidaACS = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "TEMPERATURA_SALIDA_ACS");
            }
            if (DataBaseUtils.HasColumn(dr, "ID_FICHERO_TICKET_COMBUSTION"))
            {
                dto.IdFicheroTicketCombustion = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_FICHERO_TICKET_COMBUSTION");
            }
            if (DataBaseUtils.HasColumn(dr, "ID_FICHERO_CONDUCTO_HUMOS"))
            {
                dto.IdFicheroConductoHumos = (decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_FICHERO_CONDUCTO_HUMOS");
            }
            if (DataBaseUtils.HasColumn(dr, "COMENTARIOS"))
            {
                dto.Comentarios = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "COMENTARIOS");
            }

            return dto;
        }
    }
}
