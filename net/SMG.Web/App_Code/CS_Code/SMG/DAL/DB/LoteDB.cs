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
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Transactions;
using System.Text;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de UsuarioDB
    /// </summary>
    public class LoteDB
    {
        public LoteDB()
	    {
	    }

        public List<LoteDTO> ObtenerLotes()
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<LoteDTO> lista = new List<LoteDTO>();

            IDataReader dr = db.RunProcDataReader("SP_GET_LOTES");
            
            while (dr.Read())
            {
                LoteDTO lote = new LoteDTO();
                lote.Id = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_LOTE");
                lote.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_LOTE");
                //lote.FechaLote = (DateTime) DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_LOTE");
                lista.Add(lote);
            }
            return lista;
        }

     
        public LoteDTO GenerarLote(LoteDTO loteDto, List<VisitaDTO> lista,String Usuario)
        {
            try
            {
                // ejecutamos todo dentro de una transacción
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                    // crear el lote
                    String[] aNombresLote = new String[2] { 
                                    "@DESC_LOTE", 
                                    "@FECHA_LOTE"};

                    DbType[] aTipoLote = new DbType[2] {
                                    DbType.String,
                                    DbType.DateTime    
                                    };

                    object[] aValoresLote = new object[2] { 
                                    loteDto.Descripcion,
                                    loteDto.FechaLote};

                    loteDto.Id = (Decimal?)db.RunProcEscalar("SP_INSERT_LOTE", aNombresLote, aTipoLote, aValoresLote);

                    // si la generación del lote ha ido bien, actualizamos las visitas
                    if (loteDto.Id.HasValue)
                    {
                        // actualizar el número de lote a las visitas del lote
                        foreach (VisitaDTO visita in lista)
                        {
                            if (!visita.CodigoVisita.HasValue)
                            {
                                continue;
                            }
                            visita.IdLote = loteDto.Id;
                            String[] aNombresVisita = new String[3] {
                                        "@COD_CONTRATO",
                                        "@COD_VISITA",
                                        "@ID_LOTE"};

                            DbType[] aTipoDatosVisita = new DbType[3] {
                                        DbType.String,
                                        DbType.Decimal,
                                        DbType.Decimal
                                        };

                            object[] aValoresVisita = new object[3] { 
                                        visita.CodigoContrato,
                                        visita.CodigoVisita,
                                        visita.IdLote.Value};

                            db.RunProcEscalar("SP_UPDATE_VISITA_LOTE", aNombresVisita, aTipoDatosVisita, aValoresVisita);
                            
                            
                            VisitaHistoricoDB visitaHistoricoDB = new VisitaHistoricoDB();
                            VisitaHistoricoDTO visitaHistoricoDTO = new VisitaHistoricoDTO(visita.CodigoContrato,
                                    visita.CodigoVisita.Value,loteDto.FechaLote, StringEnum.GetStringValue(Enumerados.EstadosVisita.EnEjecución));
                            visitaHistoricoDB.SalvarHistorico(visitaHistoricoDTO,Usuario);

                        }
                    }
                    else
                    {
                        throw new DALException(false, "2003");
                    }
                    // realizamos el  commit de la transaccion.
                    transactionScope.Complete();
                }

            }
            catch (BaseException be2)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2003");
            }
            // retornamos el lote con el id del lote generado
            return loteDto;
        }

        public int AsociarLoteAVisitas(LoteDTO loteDTO, List<VisitaDTO> lista)
        {
            return 0;
        }

        public Boolean BuscarPorId(LoteDTO loteDTO)
        {
            StringBuilder strSQL = new StringBuilder("SELECT DESC_LOTE FROM LOTE where ID_LOTE =" + loteDTO.Id);
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            IDataReader dr = db.RunQueryDataReader(strSQL.ToString());

            if (dr.Read())
            {
                loteDTO.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_LOTE");
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean BuscarPorDescripcion(LoteDTO loteDTO)
        {
            StringBuilder strSQL = new StringBuilder("SELECT ID_LOTE, DESC_LOTE FROM LOTE where DESC_LOTE LIKE '%" + loteDTO.Descripcion + "%'");
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            IDataReader dr = db.RunQueryDataReader(strSQL.ToString());
            

            if (dr.Read())
            {
                loteDTO.Id = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_LOTE");
                loteDTO.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_LOTE");
                // si viene más de una fila no nos vale el resultado, porque sólo tenemos que coger una.
                if (dr.Read())
                {
                    return false;
                }
                else
                {
                    return true;
                }
                
            }
            else
            {
                return false;
            }
        }

        public IDataReader ObtenerLotesDescripcion(LoteDTO loteDTO)
        {
            StringBuilder strSQL = new StringBuilder("SELECT ID_LOTE, DESC_LOTE FROM LOTE where DESC_LOTE LIKE '%" + loteDTO.Descripcion + "%'");
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunQueryDataReader(strSQL.ToString());
        }
    }
}
