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
using Iberdrola.Commons.Utils;
using System.Collections.Generic;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de VisitasDB
    /// </summary>
    public partial class VisitasDB : BaseDB
    {
        public VisitasDB()
        {

        }

        public IDataReader ObtenerVisitas(string Contrato,Int16 idIdioma)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] aNombre = new string[2];
            aNombre[0] = "@COD_CONTRATO";
            aNombre[1] = "@idIDIOMA";

            DbType[] aTipo = new DbType[2];
            aTipo[0] = DbType.String;
            aTipo[1] = DbType.Int16;

            Object[] aValores = new Object[2];
            aValores[0] = Contrato;
            aValores[1] = idIdioma;
            

            IDataReader dr = db.RunProcDataReader("[SP_GET_VISITAS]",aNombre, aTipo, aValores);

            return dr;
        }

        public IDataReader ObtenerDatosVisitas(string Contrato,string Visita,string Proveedor, Int16 idIdioma)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] Parametros = new string[4];
            Parametros[0] = "@VISITA";
            Parametros[1] = "@CONTRATO";
            Parametros[2] = "@PROVEEDOR";
            Parametros[3] = "@idIDIOMA";

            DbType[] TiposParametros = new DbType[4];
            TiposParametros[0] = DbType.String;
            TiposParametros[1] = DbType.String;
            TiposParametros[2] = DbType.String;
            TiposParametros[3] = DbType.Int16;

            Object[] ValoresParametros = new Object[4];
            ValoresParametros[0] = Visita;
            ValoresParametros[1] = Contrato;
            ValoresParametros[2] = Proveedor;
            ValoresParametros[3] = idIdioma;

            IDataReader dr = db.RunProcDataReader("SP_GET_DATOS_VISITA", Parametros, TiposParametros, ValoresParametros);

            return dr;
        }

        public IDataReader ObtenerFechaVisitaFechaCierreSolicitudAveria(string Contrato, string Visita, string Proveedor)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] Parametros = new string[3];
            Parametros[0] = "@VISITA";
            Parametros[1] = "@CONTRATO";
            Parametros[2] = "@PROVEEDOR";

            DbType[] TiposParametros = new DbType[3];
            TiposParametros[0] = DbType.String;
            TiposParametros[1] = DbType.String;
            TiposParametros[2] = DbType.String;

            string[] ValoresParametros = new string[3];
            ValoresParametros[0] = Visita;
            ValoresParametros[1] = Contrato;
            ValoresParametros[2] = Proveedor;

            IDataReader dr = db.RunProcDataReader("GET_FECHA_VISITA_FECHA_CIERRE_SOLICITUD_AVERIA", Parametros, TiposParametros, ValoresParametros);

            return dr;
        }

        public Boolean esProteccionGas(string Contrato)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[1];
                DbType[] Tipos = new DbType[1];
                object[] ValoresParametros = new object[1];

                Parametros[0] = "@CODCONTRATO";
                Tipos[0] = DbType.String;
                ValoresParametros[0] = Contrato;

                return Boolean.Parse(db.RunProcEscalar("spComprobarSiEsProteccionGasPorcontrato", Parametros, Tipos, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        
        }

        public Int64 ComprobarSisolicitudInspeccionGasAbiertaEnLosUltimosCincoAños(string Contrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] Parametros = new string[1];
            Parametros[0] = "@CONTRATO";

            DbType[] TiposParametros = new DbType[1];
            TiposParametros[0] = DbType.String;

            string[] ValoresParametros = new string[1];
            ValoresParametros[0] = Contrato;

            return Int64.Parse(db.RunProcEscalar("SMG_solicitudInspeccionGasAbiertaEnLosUltimosCincoAños_GET", Parametros, TiposParametros, ValoresParametros).ToString());
        }

        public Boolean comprobarSiTieneQuePagarInspeccion(string Contrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] Parametros = new string[1];
            Parametros[0] = "@CONTRATO";

            DbType[] TiposParametros = new DbType[1];
            TiposParametros[0] = DbType.String;

            string[] ValoresParametros = new string[1];
            ValoresParametros[0] = Contrato;

            return Boolean.Parse(db.RunProcEscalar("spComprobarSiTieneQuePagarLaInspeccion", Parametros, TiposParametros, ValoresParametros).ToString());

            
        }

        public Boolean comprobarSiTieneSolicitudesAnterioresParaLaCampania(string Contrato,string Campania)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] Parametros = new string[2];
            Parametros[0] = "@CONTRATO";
            Parametros[1] = "@CAMPANIA";

            DbType[] TiposParametros = new DbType[2];
            TiposParametros[0] = DbType.String;
            TiposParametros[1] = DbType.String;

            string[] ValoresParametros = new string[2];
            ValoresParametros[0] = Contrato;
            ValoresParametros[1] = Campania;

            return Boolean.Parse(db.RunProcEscalar("spComprobarSiTieneSolicitudesParaCampania", Parametros, TiposParametros, ValoresParametros).ToString());
        }
        
        public Int32 comprobarSiTieneOtraInspeccionAbierta(string Contrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] Parametros = new string[1];
            Parametros[0] = "@CONTRATO";

            DbType[] TiposParametros = new DbType[1];
            TiposParametros[0] = DbType.String;

            string[] ValoresParametros = new string[1];
            ValoresParametros[0] = Contrato;

            return Int32.Parse(db.RunProcEscalar("spComprobarSiTieneOtraInspeccionAbierta", Parametros, TiposParametros, ValoresParametros).ToString());


        }


        public IDataReader ObtenerTipoUrgencia(string Codigo,int idIdioma)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            String Sql;
            
            Sql="Select DESC_TIPO_URGENCIA from TIPO_URGENCIA where ID_TIPO_URGENCIA=" + Codigo + " and id_idioma=" + idIdioma;
            IDataReader dr = db.RunQueryDataReader(Sql);
            return dr;
        }

        public IDataReader ObtenerDatosReparacion(Decimal Codigo)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            String Sql;

            Sql = "Select ID_TIPO_REPARACION,ID_TIPO_TIEMPO_MANO_OBRA,IMPORTE_REPARACION,IMPORTE_MANO_OBRA_ADICIONAL,IMPORTE_MATERIALES_ADICIONAL from REPARACION where ID_REPARACION=" + Codigo;
            IDataReader dr = db.RunQueryDataReader(Sql);
            return dr;
        }

        public IDataReader ObtenerDescLote(string Codigo)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            String Sql;
            Sql = "Select DESC_LOTE from LOTE where ID_LOTE=" + Codigo;
            IDataReader dr = db.RunQueryDataReader(Sql);
            return dr;
        }

        public void ActualizarPenalizacionVisita(String Proveedor,String Fecha)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@PROVEEDOR";
                Parametros[1] = "@FECHA";
                
                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = Proveedor;
                ValoresParametros[1] = Fecha;

                db.RunProcEscalar("SP_ACTUALIZAR_PENALIZACIONES_VISITAS_VENCIDAS", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public DataTable ObtenerHistoricoVisitaPorIdVisita(String Contrato, String Visita,Int16 idIdioma)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@CONTRATO";
                Parametros[1] = "@VISITA";
                Parametros[2] = "@idIDIOMA";
                
                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.Int16;

                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Contrato;
                ValoresParametros[1] = Visita;
                ValoresParametros[2] = idIdioma;

                return db.RunProcDataTable ("SP_GET_HISTORICO_VISITA_PORIDVISITA", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Contrato"></param>
        /// <param name="sCodVisita"></param>
        /// <returns></returns>
        public IDataReader ObtenerHistoricoVisita(String Contrato,Int16 idIdioma)
        {
            try
            {
                
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@CONTRATO";
                Parametros[1] = "@idIDIOMA";
               

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.Int16;
               
                
                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = Contrato;
                ValoresParametros[1] = idIdioma;
              

                return db.RunProcDataReader("SP_GET_HISTORICO_VISITA", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public void ActualizarEnvioCarta(String Contrato,Int16 Visita, DateTime Fecha)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@CONTRATO";
                Parametros[1] = "@IDVISITA";
                Parametros[2] = "@FECHA";

                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.Int16;
                TiposValoresParametros[2] = DbType.DateTime;

                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Contrato;
                ValoresParametros[1] = Visita;
                ValoresParametros[2] = Fecha;

                db.RunProcEscalar("SP_ACTUALIZAR_CARTAS_ENVIADAS", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ObtenerCartasEnviar(Boolean Enviada)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[1];
                Parametros[0] = "@ENVIADA";
                
                DbType[] TiposValoresParametros = new DbType[1];
                TiposValoresParametros[0] = DbType.Boolean;

                Object[] ValoresParametros = new Object[1];
                ValoresParametros[0] = Enviada;

                return Int32.Parse(db.RunProcEscalar("SP_GET_CARTAS_A_ENVIAR", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ObtenerDescripcionEstadovisitaPorCodigo(Int64 codEstadovisita,int Idioma)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@ESTADO";
                Parametros[1] = "@IDIOMA";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.Int64;
                TiposValoresParametros[1] = DbType.Int16;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = codEstadovisita;
                ValoresParametros[1] = Idioma;

                return db.RunProcEscalar("SP_GET_DESCRIPCION_ESTADO_VISITA_POR_COD_ESTADO", Parametros, TiposValoresParametros, ValoresParametros).ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IDataReader ObtenerContador_Visitas_Dia_por_Tecnico(Int32 nIDTecnico,DateTime fechaVisita)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@ID_TECNICO";
                Parametros[1] = "@FECVISITA";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.Int32;
                TiposValoresParametros[1] = DbType.DateTime;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = nIDTecnico;
                ValoresParametros[1] = fechaVisita;

                return db.RunProcDataReader("MTOGASBD_GET_CONTADOR_VISITAS_DIAS_POR_TECNICO", Parametros, TiposValoresParametros, ValoresParametros);

                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IDataReader ObtenerCartasEnviarDatos(Boolean Enviada,Int32 Desde,Int32 Hasta)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@ENVIADA";
                Parametros[1] = "@DESDE";
                Parametros[2] = "@HASTA";
                
                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.Boolean;
                TiposValoresParametros[1] = DbType.Int32;
                TiposValoresParametros[2] = DbType.Int32;

                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Enviada;
                ValoresParametros[1] = Desde;
                ValoresParametros[2] = Hasta;

                return db.RunProcDataReader("SP_GET_DATOS_CARTAS_A_ENVIAR", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable ObtenerCartasEnviarDatosENDATATABLE(Boolean Enviada, Int32 Desde, Int32 Hasta)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@ENVIADA";
                Parametros[1] = "@DESDE";
                Parametros[2] = "@HASTA";
                
                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.Boolean;
                TiposValoresParametros[1] = DbType.Int32;
                TiposValoresParametros[2] = DbType.Int32;

                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Enviada;
                ValoresParametros[1] = Desde;
                ValoresParametros[2] = Hasta;

                IDataReader dr=db.RunProcDataReader("SP_GET_DATOS_CARTAS_A_ENVIAR", Parametros, TiposValoresParametros, ValoresParametros);

                DataTable tabla = new DataTable();
                tabla.Columns.Add(new DataColumn("FILA"));
                tabla.Columns.Add(new DataColumn("CONTRATO"));
                tabla.Columns.Add(new DataColumn("VISITA"));
                tabla.Columns.Add(new DataColumn("NOMBRE"));
                tabla.Columns.Add(new DataColumn("APELLIDO1"));
                tabla.Columns.Add(new DataColumn("APELLIDO2"));
                tabla.Columns.Add(new DataColumn("CALLE"));
                tabla.Columns.Add(new DataColumn("PORTAL"));
                tabla.Columns.Add(new DataColumn("TELEFONO"));
                tabla.Columns.Add(new DataColumn("PROVINCIA"));
                tabla.Columns.Add(new DataColumn("POBLACION"));
                tabla.Columns.Add(new DataColumn("PROVEEDOR"));
                tabla.Columns.Add(new DataColumn("CUPS"));
                tabla.Columns.Add(new DataColumn("COD_RECEPTOR"));
                while (dr.Read())
                {
                    DataRow fila = tabla.NewRow();
                    fila[0] = (Int64)DataBaseUtils.GetDataReaderColumnValue(dr, "FILA");
                    fila[1] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "CONTRATO");
                    fila[2] = (Int16)DataBaseUtils.GetDataReaderColumnValue(dr, "VISITA");
                    fila[3] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE");
                    fila[4] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "APELLIDO1");
                    fila[5] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "APELLIDO2");
                    fila[6] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "CALLE");
                    fila[7] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PORTAL");
                    fila[8] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "TELEFONO");
                    fila[9] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PROVINCIA");
                    fila[10] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "POBLACION");
                    fila[11] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PROVEEDOR");
                    fila[12] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "CUPS");
                    fila[12] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_RECEPTOR");

                    tabla.Rows.Add(fila);
                }
                dr.Close();
                return tabla;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ActualizarVisitasaFacturar(String Proveedor, String Fecha,String NumFactura)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@PROVEEDOR";
                Parametros[1] = "@FECHA";
                Parametros[2] = "@NUMFACTURA";

                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;

                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Proveedor;
                ValoresParametros[1] = Fecha;
                ValoresParametros[2] = NumFactura;

                db.RunProcEscalar("SP_ACTUALIZAR_VISITAS_A_FACTURAR", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ActualizarReparacionesaFacturar(String Proveedor, String Fecha, String NumFactura)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@PROVEEDOR";
                Parametros[1] = "@FECHA";
                Parametros[2] = "@NUMFACTURA";

                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;

                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Proveedor;
                ValoresParametros[1] = Fecha;
                ValoresParametros[2] = NumFactura;

                db.RunProcEscalar("SP_ACTUALIZAR_REPARACIONES_A_FACTURAR", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void ActualizarFechaPrevistaVisitaWS(string fechaPrevistaVisita, string horaDesdePrevistaVisita, string horaHastaPrevistaVisita,string codContrato, int codVisita)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[5];
                Parametros[0] = "@FECHA_PREVISTA";
	            Parametros[1] = "@HORADESDE_PREVISTA";
	            Parametros[2] = "@HORAHASTA_PREVISTA";
	            Parametros[3] = "@COD_CONTRATO";
                Parametros[4] = "@COD_VISITA";
                
                DbType[] TiposValoresParametros = new DbType[5];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;
                TiposValoresParametros[3] = DbType.String;
                TiposValoresParametros[4] = DbType.Int16;

                Object[] ValoresParametros = new Object[5];
                ValoresParametros[0] = fechaPrevistaVisita;
                ValoresParametros[1] = horaDesdePrevistaVisita;
                ValoresParametros[2] = horaHastaPrevistaVisita;
                ValoresParametros[3] = codContrato;
                ValoresParametros[4] = codVisita;

                db.RunProcEscalar("SP_UPDATE_FECHA_PREVISTA_VISITA", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public void ActualizarDatosVisita(VisitaDTO visitaDTO,String Usuario)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[18];
                Parametros[0] = "@COD_CONTRATO";
                Parametros[1] = "@COD_VISITA";
                Parametros[2] = "@FEC_VISITA";
                Parametros[3] = "@COD_ESTADO_VISITA";
                Parametros[4] = "@OBSERVACIONES";
                Parametros[5] = "@RECEPCION_COMPROBANTE";
                Parametros[6] = "@FACTURADO_PROVEEDOR";
                Parametros[7] = "@NUM_FACTURA";
                Parametros[8] = "@FECHA_FACTURA";
                Parametros[9] = "@ENVIADA_CARTA";
                Parametros[10] = "@FECHA_ENVIO_CARTA";
                Parametros[11] = "@COD_BARRAS";
                Parametros[12] = "@TECNICO";
                Parametros[13] = "@OBSERVACIONESVISITA";
                Parametros[14] = "@TIPOVISITA";
                Parametros[15] = "@USUARIO";
                Parametros[16] = "@CONTADOR";
                Parametros[17] = "@CODIGOINTERNO";

                DbType[] TiposValoresParametros = new DbType[18];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.DateTime;
                TiposValoresParametros[3] = DbType.String;
                TiposValoresParametros[4] = DbType.String;
                TiposValoresParametros[5] = DbType.Boolean;
                TiposValoresParametros[6] = DbType.Boolean;
                TiposValoresParametros[7] = DbType.String;
                TiposValoresParametros[8] = DbType.DateTime;
                TiposValoresParametros[9] = DbType.Boolean;
                TiposValoresParametros[10] = DbType.DateTime;
                TiposValoresParametros[11] = DbType.String;
                TiposValoresParametros[12] = DbType.Int16;
                TiposValoresParametros[13] = DbType.String;
                TiposValoresParametros[14] = DbType.String;
                TiposValoresParametros[15] = DbType.String;
                TiposValoresParametros[16] = DbType.Boolean;
                TiposValoresParametros[17] = DbType.String;
                
                Object[] ValoresParametros = new Object[18];
                ValoresParametros[0] = visitaDTO.CodigoContrato;
                ValoresParametros[1] = visitaDTO.CodigoVisita;
                ValoresParametros[2] = visitaDTO.FechaVisita;
                ValoresParametros[3] = visitaDTO.CodigoEstadoVisita;
                ValoresParametros[4] = visitaDTO.Observaciones;
                ValoresParametros[5] = visitaDTO.RecepcionComprobante;
                ValoresParametros[6] = visitaDTO.FacturadoProveedor;
                ValoresParametros[7] = visitaDTO.NumFactura;
                ValoresParametros[8] = visitaDTO.FechaFactura;
                ValoresParametros[9] = visitaDTO.CartaEnviada;
                ValoresParametros[10] = visitaDTO.FechaEnviadoCarta;
                ValoresParametros[11] = visitaDTO.CodigoBarras;
                ValoresParametros[12] = visitaDTO.Tecnico;
                ValoresParametros[13] = visitaDTO.ObservacionesVisita;
                ValoresParametros[14] = visitaDTO.TipoVisita;
                ValoresParametros[15] = Usuario;
                ValoresParametros[16] = visitaDTO.ContadorInterno;
                ValoresParametros[17] = visitaDTO.CodigoInterno;

                db.RunProcEscalar("SP_UPDATE_DATOS_VISITA", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IDataReader ObtenerVisitasVistaTelefono(String Contrato, String CodigoProveedor, String CodSubtipo)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            try
            {
                string selectSQL = "SELECT DISTINCT MANTENIMIENTO.COD_CONTRATO_SIC, ";
                selectSQL += "MANTENIMIENTO.NOM_TITULAR, MANTENIMIENTO.APELLIDO1, ";
                selectSQL += "MANTENIMIENTO.APELLIDO2,MANTENIMIENTO.TIP_VIA_PUBLICA, ";
                selectSQL += "MANTENIMIENTO.NOM_CALLE, poblacion.nombre as NOM_POBLACION, ";
                selectSQL += "provincia.nombre as NOM_PROVINCIA, MANTENIMIENTO.COD_PORTAL, ";
                selectSQL += "MANTENIMIENTO.TIP_PISO, MANTENIMIENTO.TIP_MANO, ";
                selectSQL += "MANTENIMIENTO.COD_POSTAL, MANTENIMIENTO.PROVEEDOR, ";
                selectSQL += "MANTENIMIENTO.ESTADO_CONTRATO as DES_ESTADO, tipo_urgencia.DESC_TIPO_URGENCIA as URGENCIA, ";
                selectSQL += "MANTENIMIENTO.FEC_LIMITE_VISITA, MANTENIMIENTO.T1, ";
                selectSQL += "MANTENIMIENTO.T2, MANTENIMIENTO.T5, ";
                selectSQL += "MANTENIMIENTO.FEC_ALTA_SERVICIO, MANTENIMIENTO.FEC_BAJA_SERVICIO, ";
                selectSQL += "MANTENIMIENTO.OBSERVACIONES ";
                selectSQL += ",MANTENIMIENTO.PROVEEDOR_AVERIA ";
                selectSQL += ",VISITA.FEC_LIMITE_VISITA ";
                selectSQL += ",MANTENIMIENTO.FECHA_HASTA_FACTURACION ";
                selectSQL += ",MANTENIMIENTO.BCS as BCS ";
                selectSQL += ",MANTENIMIENTO.CUPS as CUPS ";
                selectSQL += ",MANTENIMIENTO.COD_RECEPTOR ";
                selectSQL += "FROM MANTENIMIENTO ";
                selectSQL += "LEFT JOIN VISITA ON dbo.VISITA.COD_CONTRATO = MANTENIMIENTO.COD_CONTRATO_SIC ";
                selectSQL += "inner join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia ";
                selectSQL += "inner join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia) ";
                selectSQL += "left join tipo_urgencia on tipo_urgencia.ID_TIPO_URGENCIA=visita.ID_TIPO_URGENCIA WHERE";
                //***********************************************************************************************
                if (Contrato != "") { selectSQL += " MANTENIMIENTO.COD_CONTRATO_SIC = '" + Contrato + "' AND "; }
                if (CodSubtipo != "-1" && CodSubtipo != "")
                {
                    selectSQL += "[Solicitudes].[subtipo_solicitud] = '" + CodSubtipo + "' AND ";
                }
                if (CodigoProveedor != "" && (CodigoProveedor.ToUpper()) != "D") { selectSQL += "MANTENIMIENTO.PROVEEDOR = '" + CodigoProveedor + "' AND "; }
                selectSQL = selectSQL.Substring(0, selectSQL.Length - 5);
                selectSQL += " ORDER BY VISITA.FEC_LIMITE_VISITA DESC";

                IDataReader dr = db.RunQueryDataReader(selectSQL);
                return dr;

            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public Boolean ComprobarTecnicoExistente(int idTecnico, string Proveedor)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[2];
                DbType[] Tipos = new DbType[2];
                object[] ValoresParametros = new object[2];

                Parametros[0] = "@IDTECNICO";
                Tipos[0] = DbType.Int32;
                ValoresParametros[0] = idTecnico;

                Parametros[1] = "@PROVEEDOR";
                Tipos[1] = DbType.String;
                ValoresParametros[1] = Proveedor;

                if (db.RunProcEscalar("spComprobarTecnicoExiste", Parametros, Tipos, ValoresParametros) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int ObtenerContadorVisitasTecnicoPorDia(Int32 nIDTecnico, DateTime dFecVisita)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[2];
                Parametros[0] = "@ID_TECNICO";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.Int32;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = nIDTecnico;

                Parametros[1] = "@FECHAVISITA";


                TiposValoresParametros[1] = DbType.DateTime;


                ValoresParametros[1] = dFecVisita;

                string numeroRegistros = db.RunProcEscalar("spObtenerNumeroVisitasTecnico", Parametros, TiposValoresParametros, ValoresParametros).ToString();
                if (numeroRegistros == "")
                {
                    return 0;
                }
                else
                {
                    return int.Parse(numeroRegistros);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DateTime? ObtenerFechaPasoACerrada(string codContrato, string codCOntratoActual, int codVisita)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[3];
                DbType[] Tipos = new DbType[3];
                object[] ValoresParametros = new object[3];

                Parametros[0] = "@COD_CONTRATO";
                Tipos[0] = DbType.String;
                ValoresParametros[0] = codContrato;

                Parametros[1] = "@COD_CONTRATOACTUAL";
                Tipos[1] = DbType.String;
                ValoresParametros[1] = codCOntratoActual;

                Parametros[2] = "@COD_VISITA";
                Tipos[2] = DbType.Int16;
                ValoresParametros[2] = codVisita;

                DateTime? fechaMovimiento= (DateTime?)db.RunProcEscalar("spObtenerFechaPasoACerradaDeUnaVisita", Parametros, Tipos, ValoresParametros);

                if (fechaMovimiento != null)
                {
                    return fechaMovimiento;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
          
        }

        public DateTime? ObtenerFechaPasoACanceladaPorSistema(string codContrato, string codCOntratoActual, int codVisita)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[3];
                DbType[] Tipos = new DbType[3];
                object[] ValoresParametros = new object[3];

                Parametros[0] = "@COD_CONTRATO";
                Tipos[0] = DbType.String;
                ValoresParametros[0] = codContrato;

                Parametros[1] = "@COD_CONTRATOACTUAL";
                Tipos[1] = DbType.String;
                ValoresParametros[1] = codCOntratoActual;

                Parametros[2] = "@COD_VISITA";
                Tipos[2] = DbType.Int16;
                ValoresParametros[2] = codVisita;

                DateTime? fechaMovimiento = (DateTime?)db.RunProcEscalar("spObtenerFechaPasoACanceladaPorSistemaDeUnaVisita", Parametros, Tipos, ValoresParametros);

                if (fechaMovimiento != null)
                {
                    return fechaMovimiento;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
          
        }

        //20191119 BGN ADD BEG R#20616: Procedimientos cancelados no localizables
        public Int32 ObtenerNumCartasEnviadas(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@FECHA_DESDE";
                Parametros[1] = "@FECHA_HASTA";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.DateTime;
                TiposValoresParametros[1] = DbType.DateTime;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = fechaDesde;
                ValoresParametros[1] = fechaHasta;

                return Int32.Parse(db.RunProcEscalar("SP_GET_NUM_CARTAS_ENVIADAS", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IDataReader ObtenerCartasEnviadas(DateTime fechaDesde, DateTime fechaHasta, Int32 Desde, Int32 Hasta)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[4];
                Parametros[0] = "@FECHA_DESDE";
                Parametros[1] = "@FECHA_HASTA";
                Parametros[2] = "@DESDE";
                Parametros[3] = "@HASTA";

                DbType[] TiposValoresParametros = new DbType[4];
                TiposValoresParametros[0] = DbType.DateTime;
                TiposValoresParametros[1] = DbType.DateTime;
                TiposValoresParametros[2] = DbType.Int32;
                TiposValoresParametros[3] = DbType.Int32;

                Object[] ValoresParametros = new Object[4];
                ValoresParametros[0] = fechaDesde;
                ValoresParametros[1] = fechaHasta;
                ValoresParametros[2] = Desde;
                ValoresParametros[3] = Hasta;

                return db.RunProcDataReader("SP_GET_CARTAS_ENVIADAS", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable ObtenerCartasEnviadasEnDataTable(DateTime fechaDesde, DateTime fechaHasta, Int32 Desde, Int32 Hasta)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[4];
                Parametros[0] = "@FECHA_DESDE";
                Parametros[1] = "@FECHA_HASTA";
                Parametros[2] = "@DESDE";
                Parametros[3] = "@HASTA";

                DbType[] TiposValoresParametros = new DbType[4];
                TiposValoresParametros[0] = DbType.DateTime;
                TiposValoresParametros[1] = DbType.DateTime;
                TiposValoresParametros[2] = DbType.Int32;
                TiposValoresParametros[3] = DbType.Int32;

                Object[] ValoresParametros = new Object[4];
                ValoresParametros[0] = fechaDesde;
                ValoresParametros[1] = fechaHasta;
                ValoresParametros[2] = Desde;
                ValoresParametros[3] = Hasta;

                IDataReader dr = db.RunProcDataReader("SP_GET_CARTAS_ENVIADAS", Parametros, TiposValoresParametros, ValoresParametros);

                DataTable tabla = new DataTable();
                tabla.Columns.Add(new DataColumn("FILA"));
                tabla.Columns.Add(new DataColumn("CONTRATO"));
                tabla.Columns.Add(new DataColumn("VISITA"));
                tabla.Columns.Add(new DataColumn("NOMBRE"));
                tabla.Columns.Add(new DataColumn("APELLIDO1"));
                tabla.Columns.Add(new DataColumn("APELLIDO2"));
                tabla.Columns.Add(new DataColumn("CALLE"));
                tabla.Columns.Add(new DataColumn("PORTAL"));
                tabla.Columns.Add(new DataColumn("TELEFONO"));
                tabla.Columns.Add(new DataColumn("PROVINCIA"));
                tabla.Columns.Add(new DataColumn("POBLACION"));
                tabla.Columns.Add(new DataColumn("PROVEEDOR"));
                tabla.Columns.Add(new DataColumn("CUPS"));
                tabla.Columns.Add(new DataColumn("COD_RECEPTOR"));
                tabla.Columns.Add(new DataColumn("FECHA_ENVIO_CARTA"));
                while (dr.Read())
                {
                    DataRow fila = tabla.NewRow();
                    fila[0] = (Int64)DataBaseUtils.GetDataReaderColumnValue(dr, "FILA");
                    fila[1] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "CONTRATO");
                    fila[2] = (Int16)DataBaseUtils.GetDataReaderColumnValue(dr, "VISITA");
                    fila[3] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE");
                    fila[4] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "APELLIDO1");
                    fila[5] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "APELLIDO2");
                    fila[6] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "CALLE");
                    fila[7] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PORTAL");
                    fila[8] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "TELEFONO");
                    fila[9] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PROVINCIA");
                    fila[10] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "POBLACION");
                    fila[11] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PROVEEDOR");
                    fila[12] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "CUPS");
                    fila[13] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_RECEPTOR");
                    fila[14] = (DateTime)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHA_ENVIO_CARTA");

                    tabla.Rows.Add(fila);
                }
                dr.Close();
                return tabla;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20191119 BGN ADD END R#20616: Procedimientos cancelados no localizables
    }
}