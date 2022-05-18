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
using System.Transactions;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de UsuarioDB
    /// </summary>
    public class CalderaDB
    {
        public CalderaDB()
	    {

	    }

        public List<CalderaDTO> ObtenerCalderas()
        {

            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                List<CalderaDTO> lista = new List<CalderaDTO>();

                IDataReader dr = db.RunProcDataReader("SP_GET_CALDERAS");


                while (dr.Read())
                {
                    CalderaDTO caldera = new CalderaDTO();

                    caldera.CodigoContrato = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_CONTRATO");
                    caldera.IdTipoCaldera = (Int32?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_CALDERA");
                    caldera.IdMarcaCaldera = (Int32?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_MARCA_CALDERA");
                    caldera.ModeloCaldera = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "MODELO_CALDERA");
                    caldera.Uso = (Int32?)DataBaseUtils.GetDataReaderColumnValue(dr, "USO");
                    caldera.Potencia = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "POTENCIA");
                    caldera.Anio = (Int32?)DataBaseUtils.GetDataReaderColumnValue(dr, "ANIO");

                    lista.Add(caldera);
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
        }

        public CalderaDTO ObtenerCaldera(CalderaDTO caldera)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                String[] aNombres = new String[1] { "@COD_CONTRATO" };
                DbType[] aTipos = new DbType[1] { DbType.String };
                String[] aValores = new String[1] { caldera.CodigoContrato };

                IDataReader dr = db.RunProcDataReader("SP_GET_CALDERA", aNombres, aTipos, aValores);

                if (dr.Read())
                {
                    caldera.CodigoContrato = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_CONTRATO");
                    caldera.IdTipoCaldera = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_CALDERA");
                    caldera.IdMarcaCaldera = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_MARCA_CALDERA");
                    caldera.ModeloCaldera = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "MODELO_CALDERA");
                    caldera.Uso = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "USO");
                    caldera.Potencia = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "POTENCIA");
                    caldera.Anio = (Int32?)DataBaseUtils.GetDataReaderColumnValue(dr, "ANIO");

                    return caldera;
                }
                else
                {
                    return null;
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
        }

        public DataTable dtObtenerCaldera(string codContrato)
        {
            DataTable dt = null;

            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                String[] aNombres = new String[1] { "@COD_CONTRATO" };
                DbType[] aTipos = new DbType[1] { DbType.String };
                String[] aValores = new String[1] { codContrato };

                dt = db.RunProcDataTable("SP_GET_CALDERA", aNombres, aTipos, aValores);

                return dt;
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

        public void EliminarCaldera(CalderaDTO caldera)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                String[] aNombresCaldera = new String[1] { "@COD_CONTRATO" };
                DbType[] aTipos = new DbType[1] { DbType.String };
                String[] aValoresCaldera = new String[1] { caldera.CodigoContrato };
                db.RunProcEscalar("SP_DELETE_CALDERA", aNombresCaldera, aTipos, aValoresCaldera);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2006");
            }
        }

        public void ActualizarInsertarCaldera(CalderaDTO caldera)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                String[] aNombresCaldera = new String[8] { 
                                    "@COD_CONTRATO",
                                    "@ID_TIPO_CALDERA",
                                    "@ID_MARCA_CALDERA",
                                    "@MODELO_CALDERA",
                                    "@USO",
                                    "@POTENCIA",
                                    "@ANIO",
                                    "@DESC_MARCA_CALDERA"
                                    };

                DbType[] aTiposDatosCaldera = new DbType[8] {
                                    DbType.String,
                                    DbType.Decimal,
                                    DbType.Decimal,
                                    DbType.String,
                                    DbType.Decimal,
                                    DbType.String,
                                    DbType.Int32,
                                    DbType.String
                                    };

                Object[] aValoresCaldera = new Object[8] { 
                                    caldera.CodigoContrato,
                                    caldera.IdTipoCaldera,
                                    caldera.IdMarcaCaldera,
                                    caldera.ModeloCaldera,
                                    caldera.Uso,
                                    caldera.Potencia,
                                    caldera.Anio,
                                    caldera.DecripcionMarcaCaldera
                                    };

                db.RunProcEscalar("SP_INSERT_UPDATE_CALDERA", aNombresCaldera, aTiposDatosCaldera, aValoresCaldera);
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

        public DataTable GetTipologiaCalderaPorIdSolicitud(string idSolicitud)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@id_solicitud";
            ParamsValue[0] = idSolicitud;
            ParamsType[0] = DbType.String;

            return db.RunProcDataTable("SP_OBTENER_TIPOLOGIA_CALDERA_POR_IDSOLICITUD", ParamsName, ParamsType, ParamsValue);
        }
        
    }
}
