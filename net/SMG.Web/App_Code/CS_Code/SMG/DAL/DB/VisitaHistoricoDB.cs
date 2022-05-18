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
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.DataAccess;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de VisitaHistoricoDB
    /// </summary>
    public class VisitaHistoricoDB: BaseDB
    {
        public VisitaHistoricoDB()
        {

        }

        public void SalvarHistorico(VisitaHistoricoDTO visitaHistoricoDTO,String Usuario)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] Parametros = new string[5];
            Parametros[0] = "@COD_CONTRATO";
            Parametros[1] = "@COD_VISITA";
            Parametros[2] = "@FECHA";
            Parametros[3] = "@USUARIO";
            Parametros[4] = "@COD_ESTADO_NUEVO";

            DbType[] TiposValoresParametros = new DbType[5];
            TiposValoresParametros[0] = DbType.String;
            TiposValoresParametros[1] = DbType.Decimal;
            TiposValoresParametros[2] = DbType.DateTime;
            TiposValoresParametros[3] = DbType.String;
            TiposValoresParametros[4] = DbType.String;

            Object[] ValoresParametros = new Object[5];
            ValoresParametros[0] = visitaHistoricoDTO.CodContrato;
            ValoresParametros[1] = visitaHistoricoDTO.CodVisita;
            ValoresParametros[2] = visitaHistoricoDTO.Fecha;
            ValoresParametros[3] = Usuario;
            ValoresParametros[4] = visitaHistoricoDTO.CodEstadoVisita;

            db.RunProcEscalar("SP_INSERT_VISITA_HISTORICO", Parametros, TiposValoresParametros, ValoresParametros);
        }


        public void SalvarHistoricoWS(VisitaHistoricoDTOWS visitaHistoricoDTO, String Usuario)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] Parametros = new string[5];
            Parametros[0] = "@COD_CONTRATO";
            Parametros[1] = "@COD_VISITA";
            Parametros[2] = "@FECHA";
            Parametros[3] = "@USUARIO";
            Parametros[4] = "@COD_ESTADO_NUEVO";

            DbType[] TiposValoresParametros = new DbType[5];
            TiposValoresParametros[0] = DbType.String;
            TiposValoresParametros[1] = DbType.Decimal;
            TiposValoresParametros[2] = DbType.DateTime;
            TiposValoresParametros[3] = DbType.String;
            TiposValoresParametros[4] = DbType.String;

            Object[] ValoresParametros = new Object[5];
            ValoresParametros[0] = visitaHistoricoDTO.CodContrato;
            ValoresParametros[1] = visitaHistoricoDTO.CodVisita;
            ValoresParametros[2] = visitaHistoricoDTO.Fecha;
            ValoresParametros[3] = Usuario;
            ValoresParametros[4] = visitaHistoricoDTO.CodEstadoVisita;

            db.RunProcEscalar("SP_INSERT_VISITA_HISTORICO", Parametros, TiposValoresParametros, ValoresParametros);
        }
        public IDataReader ObtenerFechasMovimiento(String Contrato, int Visita)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] Parametros = new string[2];
            Parametros[0] = "@COD_CONTRATO";
            Parametros[1] = "@COD_VISITA";

            DbType[] TiposValoresParametros = new DbType[2];
            TiposValoresParametros[0] = DbType.String;
            TiposValoresParametros[1] = DbType.Int16;

            Object[] ValoresParametros = new Object[2];
            ValoresParametros[0] = Contrato;
            ValoresParametros[1] = Visita;

            IDataReader dr = db.RunProcDataReader("SP_GET_FECHA_ULTIMO_MOVIMIENTO_VISITA", Parametros, TiposValoresParametros, ValoresParametros);
            
            return dr;
        }

        public string ObtenerProveedorUltimoMovimiento(String Contrato, int Visita)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] Parametros = new string[2];
            Parametros[0] = "@COD_CONTRATO";
            Parametros[1] = "@COD_VISITA";

            DbType[] TiposValoresParametros = new DbType[2];
            TiposValoresParametros[0] = DbType.String;
            TiposValoresParametros[1] = DbType.Int16;

            Object[] ValoresParametros = new Object[2];
            ValoresParametros[0] = Contrato;
            ValoresParametros[1] = Visita;

            IDataReader dr = db.RunProcDataReader("SP_GET_PROVEEDOR_ULTIMO_MOVIMIENTO_VISITA", Parametros, TiposValoresParametros, ValoresParametros);

            while (dr.Read())
            {
                return (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PROVEEDOR");
            }

            return "";
            
            
        }

        public IDataReader ObtenerFechasMovimientoParaCerrada(String Contrato, int Visita)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] Parametros = new string[2];
            Parametros[0] = "@COD_CONTRATO";
            Parametros[1] = "@COD_VISITA";

            DbType[] TiposValoresParametros = new DbType[2];
            TiposValoresParametros[0] = DbType.String;
            TiposValoresParametros[1] = DbType.Int16;

            Object[] ValoresParametros = new Object[2];
            ValoresParametros[0] = Contrato;
            ValoresParametros[1] = Visita;

            IDataReader dr = db.RunProcDataReader("SP_GET_FECHA_ULTIMO_MOVIMIENTO_VISITA_PARA_CERRADA", Parametros, TiposValoresParametros, ValoresParametros);
            
            return dr;
        }
        
        public IDataReader ObtenerFechasMovimientoDeCanceladaPorSistema(String Contrato, int Visita)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] Parametros = new string[2];
            Parametros[0] = "@COD_CONTRATO";
            Parametros[1] = "@COD_VISITA";

            DbType[] TiposValoresParametros = new DbType[2];
            TiposValoresParametros[0] = DbType.String;
            TiposValoresParametros[1] = DbType.Int16;

            Object[] ValoresParametros = new Object[2];
            ValoresParametros[0] = Contrato;
            ValoresParametros[1] = Visita;

            IDataReader dr = db.RunProcDataReader("SP_GET_FECHA_ULTIMO_MOVIMIENTO_VISITA_CANCELADAS_POR_SISTEMA", Parametros, TiposValoresParametros, ValoresParametros);
            
            return dr;
        }
        public IDataReader ObtenerFechasMovimientoDeClienteAusente(String Contrato, int Visita)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] Parametros = new string[2];
            Parametros[0] = "@COD_CONTRATO";
            Parametros[1] = "@COD_VISITA";

            DbType[] TiposValoresParametros = new DbType[2];
            TiposValoresParametros[0] = DbType.String;
            TiposValoresParametros[1] = DbType.Int16;

            Object[] ValoresParametros = new Object[2];
            ValoresParametros[0] = Contrato;
            ValoresParametros[1] = Visita;

            IDataReader dr = db.RunProcDataReader("SP_GET_FECHA_ULTIMO_MOVIMIENTO_VISITA_CANCELADAS_POR_CLIENTE_NO_LOCALIZABLE", Parametros, TiposValoresParametros, ValoresParametros);

            return dr;
        }
        

    }
}