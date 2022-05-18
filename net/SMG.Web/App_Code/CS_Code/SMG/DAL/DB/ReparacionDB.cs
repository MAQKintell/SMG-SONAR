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



namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de ReparacionDB
    /// </summary>
    public class ReparacionDB
    {
        public ReparacionDB()
        {

        }

        public static ReparacionDTO ObtenerReparacion(Decimal id)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            String[] ParamName = new String[1] { "@REPARACION" };
            DbType[] ParamType = new DbType[1] { DbType.Int32 };
            Object[] ParamObject = new Object[1] { id };

            IDataReader dr = db.RunProcDataReader("SP_GET_REPARACION",ParamName, ParamType, ParamObject);

            ReparacionDTO reparacionDTO = new ReparacionDTO();

            while (dr.Read())
            {
                reparacionDTO.Id = (Decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_REPARACION");
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_REPARACION") != null)
                {
                    reparacionDTO.FechaReparacion = (DateTime)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_REPARACION");
                }
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_REPARACION") != null)
                {
                    reparacionDTO.IdTipoReparacion = (Decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_REPARACION");
                }
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_TIEMPO_MANO_OBRA") != null)
                {
                    reparacionDTO.IdTipoTiempoManoObra = (Decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_TIEMPO_MANO_OBRA");
                }
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "IMPORTE_REPARACION") != null)
                {
                    reparacionDTO.ImporteReparacion = Decimal.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "IMPORTE_REPARACION").ToString());
                }
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "IMPORTE_MANO_OBRA_ADICIONAL") != null)
                {
                    reparacionDTO.ImporteManoObraAdicional = Decimal.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "IMPORTE_MANO_OBRA_ADICIONAL").ToString());
                }
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "IMPORTE_MATERIALES_ADICIONAL") != null)
                {
                    reparacionDTO.ImporteMaterialesAdicional = Decimal.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "IMPORTE_MATERIALES_ADICIONAL").ToString());
                }
                //Kintell 11/01/10.
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_FACTURA_REPARACION") != null)
                {
                    reparacionDTO.FechaFactura = (DateTime)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_FACTURA_REPARACION");
                }
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "NUMERO_FACTURA_REPARACION") != null)
                {
                    reparacionDTO.NumeroFacttura =DataBaseUtils.GetDataReaderColumnValue(dr, "NUMERO_FACTURA_REPARACION").ToString();
                }
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "CODIGO_BARRAS_REPARACION") != null)
                {
                    reparacionDTO.CodigoBarrasReparacion  = DataBaseUtils.GetDataReaderColumnValue(dr, "CODIGO_BARRAS_REPARACION").ToString();
                }

                reparacionDTO.InformacionRecibida = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "INFORMACION_RECIBIDA");
            }
            return reparacionDTO;

        }

        public static void ActualizarReparacion(ReparacionDTO reparacionDTO)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            String[] ParamName = new String[10] { "@ID_REPARACION",
                                                 "@ID_TIPO_REPARACION",
                                                 "@FECHA_REPARACION",
                                                 "@ID_TIPO_TIEMPO_MANO_OBRA",
                                                 "@IMPORTE_REPARACION",
                                                 "@IMPORTE_MANO_OBRA_ADICIONAL",
                                                 "@IMPORTE_MATERIALES_ADICIONAL",
                                                 "@FECHA_FACTURA",
	                                             "@NUMERO_FACTURA",
                                                 "@CODIGO_BARRAS_REPARACION"
                                                };

            DbType[] ParamType = new DbType[10] { DbType.Int32,
                                                 DbType.Int32,
                                                 DbType.DateTime,
                                                 DbType.Int32,
                                                 DbType.Double,
                                                 DbType.Double,
                                                 DbType.Double,
                                                 DbType.DateTime,
                                                 DbType.String,
                                                 DbType.String};

            Object[] ParamObject = new Object[10] { reparacionDTO.Id,
                                                   reparacionDTO.IdTipoReparacion,
                                                   reparacionDTO.FechaReparacion,
                                                   reparacionDTO.IdTipoTiempoManoObra,
                                                   reparacionDTO.ImporteReparacion,
                                                   reparacionDTO.ImporteManoObraAdicional,
                                                   reparacionDTO.ImporteMaterialesAdicional,
                                                   reparacionDTO.FechaFactura,
                                                   reparacionDTO.NumeroFacttura,
                                                   reparacionDTO.CodigoBarrasReparacion };

            db.RunProcEscalar("SP_UPDATE_REPARACION", ParamName, ParamType, ParamObject);

        }

        public static void InsertarReparacion(ReparacionDTO reparacionDTO)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            String[] ParamName = new String[11] { "@COD_CONTRATO",
                                                 "@COD_VISITA",
                                                 "@FECHA_REPARACION",
                                                 "@ID_TIPO_REPARACION",
                                                 "@ID_TIPO_TIEMPO_MANO_OBRA",
                                                 "@IMPORTE_REPARACION",
                                                 "@IMPORTE_MANO_OBRA_ADICIONAL",
                                                 "@IMPORTE_MATERIALES_ADICIONAL",
                                                 "@FECHA_FACTURA",
	                                             "@NUMERO_FACTURA",
                                                 "@CODIGO_BARRAS_REPARACION"
                                                };

            DbType[] ParamType = new DbType[11] { DbType.String,
                                                 DbType.Int16,  
                                                 DbType.DateTime,
                                                 DbType.Int32,
                                                 DbType.Int32,
                                                 DbType.Double,
                                                 DbType.Double,
                                                 DbType.Double,
                                                 DbType.DateTime,
                                                 DbType.String,
                                                 DbType.String};

            Object[] ParamObject = new Object[11] { reparacionDTO.CodContrato,
                                                   reparacionDTO.CodVisita,
                                                   reparacionDTO.FechaReparacion,
                                                   reparacionDTO.IdTipoReparacion,
                                                   reparacionDTO.IdTipoTiempoManoObra,
                                                   reparacionDTO.ImporteReparacion,
                                                   reparacionDTO.ImporteManoObraAdicional,
                                                   reparacionDTO.ImporteMaterialesAdicional,
                                                   reparacionDTO.FechaFactura,
                                                   reparacionDTO.NumeroFacttura,
                                                   reparacionDTO.CodigoBarrasReparacion};

            db.RunProcEscalar("SP_INSERT_REPARACION", ParamName, ParamType, ParamObject);

        }

        public static void BorrarReparacion(ReparacionDTO reparacionDTO)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            String[] ParamName = new String[3] { "@COD_CONTRATO",
                                                 "@COD_VISITA",
                                                 "@ID_REPARACION" };

            DbType[] ParamType = new DbType[3] { DbType.String,
                                                 DbType.Int16,                                            
                                                 DbType.Int32
                                                 };

            Object[] ParamObject = new Object[3] { reparacionDTO.CodContrato,
                                                   reparacionDTO.CodVisita,
                                                   reparacionDTO.Id };

            db.RunProcEscalar("SP_DELETE_REPARACION", ParamName, ParamType, ParamObject);

        }
    }
}
