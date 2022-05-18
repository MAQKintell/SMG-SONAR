using System.Web;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.DataAccess;
using System.Collections.Generic;
using System;
using Iberdrola.Commons.Exceptions;

namespace Iberdrola.SMG.DAL.DB
{
    public class CaracteristicaHistoricoDB : BaseDB
    {
        public DataTable GetCaracteristicaHistoricoSolicitud(string Solicitud,Int16 idIdioma)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@ID_SOLICITUD";
            ParamsValue[0] = Solicitud;
            ParamsType[0] = DbType.Int64;

            ParamsName[1] = "@idIDIOMA";
            ParamsValue[1] = idIdioma;
            ParamsType[1] = DbType.Int16;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataTable("SP_GET_HISTORICO_CARACTERISTICAS_SOLICITUD", ParamsName, ParamsType, ParamsValue);
        }


        public void AddCaracteristicaCierreVisita(string codContrato, Int32 codvisita, int tipCar, string Valor)
        {
            try
            {
                string[] ParamsName = new string[4];
                DbType[] ParamsType = new DbType[4];
                object[] ParamsValue = new object[4];

                ParamsName[0] = "@pCOD_CONTRATO";
                ParamsValue[0] = codContrato;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@pCOD_VISITA";
                ParamsValue[1] = codvisita;
                ParamsType[1] = DbType.Int32;

                ParamsName[2] = "@pTIP_CAR";
                ParamsValue[2] = tipCar;
                ParamsType[2] = DbType.Int16;

                ParamsName[3] = "@pVALOR";
                ParamsValue[3] = Valor;
                ParamsType[3] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                db.RunProcEscalar("spSMGCaracteristicaCierreVisitaInsert", ParamsName, ParamsType, ParamsValue);
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

        //13/01/2020 BGN BEG R#17876 Heredar Nº Serie en Averias GC en Aparato
        public string GetNumSerieGC(string codContrato)
        {
            try
            {
                string[] ParamsName = new string[1];
                DbType[] ParamsType = new DbType[1];
                object[] ParamsValue = new object[1];

                ParamsName[0] = "@pCOD_CONTRATO";
                ParamsValue[0] = codContrato;
                ParamsType[0] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                object resultado = db.RunProcEscalar("spSMGGetNumSerieGC", ParamsName, ParamsType, ParamsValue);
                if (resultado != null && !resultado.Equals(DBNull.Value))
                {
                    return resultado.ToString();
                }
                else
                {
                    return string.Empty;
                }
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

        //20210120 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        public string GetCaracteristicaValor(int idSolicitud, string tipCar)
        {
            try
            {
                string[] ParamsName = new string[2];
                DbType[] ParamsType = new DbType[2];
                object[] ParamsValue = new object[2];

                ParamsName[0] = "@ID_SOLICITUD";
                ParamsValue[0] = idSolicitud;
                ParamsType[0] = DbType.Int32;

                ParamsName[1] = "@TIP_CAR";
                ParamsValue[1] = tipCar;
                ParamsType[1] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                object resultado = db.RunProcEscalar("GET_CARACTERISTICA_VALOR", ParamsName, ParamsType, ParamsValue);
                if (resultado != null && !resultado.Equals(DBNull.Value))
                {
                    return resultado.ToString();
                }
                else
                {
                    return string.Empty;
                }
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
        //20210120 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

        //20210624 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
        public string GetCaracteristicaNumeroSolicitudByCodContratoCodVisitaAndTipCar(string codContrato, Int32 codvisita, string tipCar)
        {
            try
            {
                string[] ParamsName = new string[3];
                DbType[] ParamsType = new DbType[3];
                object[] ParamsValue = new object[3];

                ParamsName[0] = "@pCOD_CONTRATO";
                ParamsValue[0] = codContrato;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@pCOD_VISITA";
                ParamsValue[1] = codvisita;
                ParamsType[1] = DbType.Int32;

                ParamsName[2] = "@pTIP_CAR";
                ParamsValue[2] = tipCar;
                ParamsType[2] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                object resultado = db.RunProcEscalar("GET_CARACTERISTICA_ID_SOLICITUD_BY_COD_VISITA", ParamsName, ParamsType, ParamsValue);

                if (resultado != null && !resultado.Equals(DBNull.Value))
                {
                    return resultado.ToString();
                }
                else
                {
                    return string.Empty;
                }
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
        //20210624 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion
    }
}