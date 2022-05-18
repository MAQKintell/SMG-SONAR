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
using Iberdrola.SMG.WS;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de VisitasDB
    /// </summary>
    public partial class TrazaDB : BaseDB
    {
        public TrazaDB()
        {

        }

        public Int32 InsertarYObteneridTrazaCierreSolicitud(string Valor, string ficheroXML, CierreSolicitudRequest cierreSolicitud,string codContrato)
        {
            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[10];
                Parametros[0] = "@Valor";
                Parametros[1] = "@ficheroXML";
                Parametros[2] = "@CodigoContrato";
                Parametros[3] = "@idSolicitud";
                Parametros[4] = "@estadoSolicitud";
                Parametros[5] = "@ObservacionesSolicitud";
                Parametros[6] = "@IdMarcaCaldera";
                Parametros[7] = "@ModeloCaldera";
                Parametros[8] = "@MotivoCancelacion";
                Parametros[9] = "@TipoLugarAveria";

                DbType[] TiposValoresParametros = new DbType[10];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;
                TiposValoresParametros[3] = DbType.String;
                TiposValoresParametros[4] = DbType.String;
                TiposValoresParametros[5] = DbType.String;
                TiposValoresParametros[6] = DbType.String;
                TiposValoresParametros[7] = DbType.String;
                TiposValoresParametros[8] = DbType.String;
                TiposValoresParametros[9] = DbType.String;

                Object[] ValoresParametros = new Object[10];
                ValoresParametros[0] = Valor;
                ValoresParametros[1] = ficheroXML;
                ValoresParametros[2] = codContrato; 
                ValoresParametros[3] = cierreSolicitud.idSolicitud;
                ValoresParametros[4] = cierreSolicitud.EstadoSolicitud;
                ValoresParametros[5] = cierreSolicitud.ObservacionesSolicitud;
                ValoresParametros[6] = cierreSolicitud.IdMarcaCaldera;
                ValoresParametros[7] = cierreSolicitud.ModeloCaldera;
                ValoresParametros[8] = cierreSolicitud.MotivoCancelacion;
                ValoresParametros[9] = cierreSolicitud.TipoLugarAveria;


                return Int32.Parse(db.RunProcEscalar("spInsertGetTrazaWSCierreSolicitud", Parametros, TiposValoresParametros, ValoresParametros).ToString());
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 InsertarYObteneridTrazaAperturaSolicitud(string Valor, string ficheroXML, AperturaSolicitudRequest aperturaSolicitud)
        {
            try
            {
                string CodigoTipoAveriaTipoVisita = "";
                if (!string.IsNullOrEmpty(aperturaSolicitud.TipoAveria))
                {
                    CodigoTipoAveriaTipoVisita = aperturaSolicitud.TipoAveria;
                }
                else
                {
                    CodigoTipoAveriaTipoVisita = aperturaSolicitud.TipoVisita;
                }

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[11];
                Parametros[0] = "@Valor";
                Parametros[1] = "@ficheroXML";

                Parametros[2] = "@CodigoContrato";
                Parametros[3] = "@SubtipoSolicitud";
                Parametros[4] = "@PersonaContacto";
                Parametros[5] = "@TelefonoContacto";
                Parametros[6] = "@Horario";
                Parametros[7] = "@ObservacionesSolicitud";
                Parametros[8] = "@TipoAveriaVisita";
                Parametros[9] = "@IdMarcaCaldera";
                Parametros[10] = "@ModeloCaldera";

                DbType[] TiposValoresParametros = new DbType[11];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;
                TiposValoresParametros[3] = DbType.String;
                TiposValoresParametros[4] = DbType.String;
                TiposValoresParametros[5] = DbType.String;
                TiposValoresParametros[6] = DbType.String;
                TiposValoresParametros[7] = DbType.String;
                TiposValoresParametros[8] = DbType.String;
                TiposValoresParametros[9] = DbType.String;
                TiposValoresParametros[10] = DbType.String;

                Object[] ValoresParametros = new Object[11];
                ValoresParametros[0] = Valor;
                ValoresParametros[1] = ficheroXML;
                ValoresParametros[2] = aperturaSolicitud.CodigoContrato;
                ValoresParametros[3] = aperturaSolicitud.SubtipoSolicitud;
                ValoresParametros[4] = aperturaSolicitud.PersonaContacto;
                ValoresParametros[5] = aperturaSolicitud.TelefonoContacto;
                ValoresParametros[6] = aperturaSolicitud.Horario;
                ValoresParametros[7] = aperturaSolicitud.ObservacionesSolicitud;
                ValoresParametros[8] = CodigoTipoAveriaTipoVisita;
                ValoresParametros[9] = aperturaSolicitud.IdMarcaCaldera;
                ValoresParametros[10] = aperturaSolicitud.ModeloCaldera;


                return Int32.Parse(db.RunProcEscalar("spInsertGetTrazaWSAperturaSolicitud", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Int32 InsertarYObteneridTrazaContratoVigente(string Valor, string ficheroXML, ContratoVigenteRequest contratoVigente)
        {
            try
            {
                

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@Valor";
                Parametros[1] = "@ficheroXML";
                Parametros[2] = "@CodigoContrato";
               

                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;
                

                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Valor;
                ValoresParametros[1] = ficheroXML;
                ValoresParametros[2] = contratoVigente.CodigoContrato;
                
                return Int32.Parse(db.RunProcEscalar("spInsertGetTrazaWSContratoVigente", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public Int32 InsertarYObteneridTrazaDatosVisita(string Valor, string ficheroXML, DatosVisitaRequest contratoVigente)
        {
            try
            {


                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@Valor";
                Parametros[1] = "@ficheroXML";
                Parametros[2] = "@CodigoContrato";


                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;


                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Valor;
                ValoresParametros[1] = ficheroXML;
                ValoresParametros[2] = contratoVigente.CodigoContrato;

                return Int32.Parse(db.RunProcEscalar("spInsertGetTrazaWSDatosVisita", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Int32 InsertarYObteneridTrazaDatosSolicitud(string Valor, string ficheroXML, DatosSolicitudRequest contratoVigente)
        {
            try
            {


                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@Valor";
                Parametros[1] = "@ficheroXML";
                Parametros[2] = "@CodigoContrato";


                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;


                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Valor;
                ValoresParametros[1] = ficheroXML;
                ValoresParametros[2] = contratoVigente.CodigoContrato;

                return Int32.Parse(db.RunProcEscalar("spInsertGetTrazaWSDatosSolicitud", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public Int32 InsertarYObteneridTraza(string Valor, string ficheroXML, CierreVisitaRequest cierreVisita)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[44];
                Parametros[0] = "@Valor";
                Parametros[1] = "@ficheroXML";

                Parametros[2] = "@CodigoContrato";
                Parametros[3] = "@CodigoVisita";
                Parametros[4] = "@TelefonoContacto1";
                Parametros[5] = "@TelefonoContacto2";
                Parametros[6] = "@IdTipoCaldera";
                Parametros[7] = "@IdMarcaCaldera";
                Parametros[8] = "@ModeloCaldera";
                Parametros[9] = "@Uso";
                Parametros[10] = "@Potencia";
                Parametros[11] = "@Anio";
                Parametros[12] = "@DecripcionMarcaCaldera";
                Parametros[13] = "@EstadoVisita";
                Parametros[14] = "@FechaVisita";
                Parametros[15] = "@ObservacionesVisita";
                Parametros[16] = "@RecepcionComprobante";
                Parametros[17] = "@FacturadoProveedor";
                Parametros[18] = "@FechaFactura";
                Parametros[19] = "@NumFactura";
                Parametros[20] = "@CodigoBarrasVisita";
                Parametros[21] = "@CartaEnviada";
                Parametros[22] = "@FechaEnvioCarta";
                Parametros[23] = "@TipoVisita";
                Parametros[24] = "@IdTecnico";
                Parametros[25] = "@Proveedor";
                Parametros[26] = "@ObservacionesTecnico";
                Parametros[27] = "@ContadorInterno";
                Parametros[28] = "@TieneReparacion";
                Parametros[29] = "@IdTipoReparacion";
                Parametros[30] = "@FechaReparacion";
                Parametros[31] = "@IdTipoTiempoManoObra";
                Parametros[32] = "@CosteMateriales";
                Parametros[33] = "@ImporteManoObra";
                Parametros[34] = "@CosteMaterialesCliente";
                Parametros[35] = "@FechaFacturaReparacion";
                Parametros[36] = "@NumeroFacturaReparacion";
                Parametros[37] = "@CodigoBarrasReparacion";
                Parametros[38] = "@UsuarioCierreVisita";
                Parametros[39] = "@TipoEquipamiento";
                Parametros[40] = "@PotenciaEquipamiento";
                Parametros[41] = "@TipoProceso";
                Parametros[42] = "@NombreFichero";
                Parametros[43] = "@ContenidoFichero";

                DbType[] TiposValoresParametros = new DbType[44];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;
                TiposValoresParametros[3] = DbType.String;
                TiposValoresParametros[4] = DbType.String;
                TiposValoresParametros[5] = DbType.String;
                TiposValoresParametros[6] = DbType.String;
                TiposValoresParametros[7] = DbType.String;
                TiposValoresParametros[8] = DbType.String;
                TiposValoresParametros[9] = DbType.String;
                TiposValoresParametros[10] = DbType.String;
                TiposValoresParametros[11] = DbType.String;
                TiposValoresParametros[12] = DbType.String;
                TiposValoresParametros[13] = DbType.String;
                TiposValoresParametros[14] = DbType.String;
                TiposValoresParametros[15] = DbType.String;
                TiposValoresParametros[16] = DbType.String;
                TiposValoresParametros[17] = DbType.String;
                TiposValoresParametros[18] = DbType.String;
                TiposValoresParametros[19] = DbType.String;
                TiposValoresParametros[20] = DbType.String;
                TiposValoresParametros[21] = DbType.String;
                TiposValoresParametros[22] = DbType.String;
                TiposValoresParametros[23] = DbType.String;
                TiposValoresParametros[24] = DbType.String;
                TiposValoresParametros[25] = DbType.String;
                TiposValoresParametros[26] = DbType.String;
                TiposValoresParametros[27] = DbType.String;
                TiposValoresParametros[28] = DbType.String;
                TiposValoresParametros[29] = DbType.String;
                TiposValoresParametros[30] = DbType.String;
                TiposValoresParametros[31] = DbType.String;
                TiposValoresParametros[32] = DbType.String;
                TiposValoresParametros[33] = DbType.String;
                TiposValoresParametros[34] = DbType.String;
                TiposValoresParametros[35] = DbType.String;
                TiposValoresParametros[36] = DbType.String;
                TiposValoresParametros[37] = DbType.String;
                TiposValoresParametros[38] = DbType.String;
                TiposValoresParametros[39] = DbType.String;
                TiposValoresParametros[40] = DbType.String;
                TiposValoresParametros[41] = DbType.String;
                TiposValoresParametros[42] = DbType.String;
                TiposValoresParametros[43] = DbType.String;


                Object[] ValoresParametros = new Object[44];
                ValoresParametros[0] = Valor;
                ValoresParametros[1] = ficheroXML;
                ValoresParametros[2] = cierreVisita.CodigoContrato;
                ValoresParametros[3] = cierreVisita.CodigoVisita;
                ValoresParametros[4] = cierreVisita.TelefonoContacto1;
                ValoresParametros[5] = cierreVisita.TelefonoContacto2;
                ValoresParametros[6] = cierreVisita.IdTipoCaldera;
                ValoresParametros[7] = cierreVisita.IdMarcaCaldera;
                ValoresParametros[8] = cierreVisita.ModeloCaldera;
                ValoresParametros[9] = cierreVisita.Uso;
                ValoresParametros[10] = cierreVisita.Potencia;
                ValoresParametros[11] = cierreVisita.Anio;
                ValoresParametros[12] = cierreVisita.DecripcionMarcaCaldera;
                ValoresParametros[13] = cierreVisita.EstadoVisita;
                ValoresParametros[14] = cierreVisita.FechaVisita;
                ValoresParametros[15] = cierreVisita.ObservacionesVisita;
                ValoresParametros[16] = cierreVisita.RecepcionComprobante;
                ValoresParametros[17] = cierreVisita.FacturadoProveedor;
                ValoresParametros[18] = cierreVisita.FechaFactura;
                ValoresParametros[19] = cierreVisita.NumFactura;
                ValoresParametros[20] = cierreVisita.CodigoBarrasVisita;
                ValoresParametros[21] = cierreVisita.CartaEnviada;
                ValoresParametros[22] = cierreVisita.FechaEnvioCarta;
                ValoresParametros[23] = cierreVisita.TipoVisita;
                ValoresParametros[24] = cierreVisita.IdTecnico;
                ValoresParametros[25] = cierreVisita.Proveedor;
                ValoresParametros[26] = cierreVisita.ObservacionesTecnico;
                ValoresParametros[27] = cierreVisita.ContadorInterno;
                ValoresParametros[28] = cierreVisita.TieneReparacion;
                ValoresParametros[29] = cierreVisita.IdTipoReparacion;
                ValoresParametros[30] = cierreVisita.FechaReparacion;
                ValoresParametros[31] = cierreVisita.IdTipoTiempoManoObra;
                ValoresParametros[32] = cierreVisita.CosteMateriales;
                ValoresParametros[33] = cierreVisita.ImporteManoObra;
                ValoresParametros[34] = cierreVisita.CosteMaterialesCliente;
                ValoresParametros[35] = cierreVisita.FechaFacturaReparacion;
                ValoresParametros[36] = cierreVisita.NumeroFacturaReparacion;
                ValoresParametros[37] = cierreVisita.CodigoBarrasReparacion;
                ValoresParametros[38] = cierreVisita.UsuarioCierreVisita;
                ValoresParametros[39] = cierreVisita.TipoEquipamiento;
                ValoresParametros[40] = cierreVisita.PotenciaEquipamiento;
                ValoresParametros[41] = cierreVisita.TipoProceso;
                ValoresParametros[42] = cierreVisita.NombreFichero;
                ValoresParametros[43] = "";// cierreVisita.ContenidoFichero;


                return Int32.Parse(db.RunProcEscalar("spInsertGetTrazaWSCierreVisita", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ActualizarTraza(String idTraza, string Valor)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@IDTRAZA";
                Parametros[1] = "@VALOR";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = idTraza;
                ValoresParametros[1] = Valor;

                db.RunProcEscalar("SpActualizarTraza", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

		public void InsertarUsuarioTraza(String idTraza, string Valor)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@IDTRAZA";
                Parametros[1] = "@USUARIO";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = idTraza;
                ValoresParametros[1] = Valor;

                db.RunProcEscalar("SpInsertarUsuarioTraza", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 InsertarTrazaWS(String nombreWS, String request)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@nombreWS";
                Parametros[1] = "@request";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = nombreWS;
                ValoresParametros[1] = request;

                return Int32.Parse(db.RunProcEscalar("spInsertTrazaWS", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ActualizarTrazaWS(Int32 idTraza, String resultado)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@idTraza";
                Parametros[1] = "@resultado";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.Int32;
                TiposValoresParametros[1] = DbType.String;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = idTraza;
                ValoresParametros[1] = resultado;

                db.RunProcEscalar("spUpdateTrazaWS", Parametros, TiposValoresParametros, ValoresParametros);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
        public Int32 InsertarTrazaRequestXML(Decimal? idTraza, string xml)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[2];
                Parametros[0] = "@pID_TRAZA";
                Parametros[1] = "@pXML";

                DbType[] TiposValoresParametros = new DbType[2];
                TiposValoresParametros[0] = DbType.Int32;
                TiposValoresParametros[1] = DbType.String;

                Object[] ValoresParametros = new Object[2];
                ValoresParametros[0] = idTraza;
                ValoresParametros[1] = xml;

                return Int32.Parse(db.RunProcEscalar("spSMGTrazaRequestXMLInsert", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20201013 BUA END R#23245: Guardar el Ticket de combustión

        //20210129 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        public Int32 InsertarTrazaWSCierreVisita(string valor, string xml, string nombreWS, string usuario)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[4];
                Parametros[0] = "@Valor";
                Parametros[1] = "@ficheroXML";
                Parametros[2] = "@nombreWS";
                Parametros[3] = "@usuario";

                DbType[] TiposValoresParametros = new DbType[4];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;
                TiposValoresParametros[3] = DbType.String;

                Object[] ValoresParametros = new Object[4];
                ValoresParametros[0] = valor;
                ValoresParametros[1] = xml;
                ValoresParametros[2] = nombreWS;
                ValoresParametros[3] = usuario;

                return Int32.Parse(db.RunProcEscalar("spInsertTrazaWSCierreVisita", Parametros, TiposValoresParametros, ValoresParametros).ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //20210129 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital
    }
}