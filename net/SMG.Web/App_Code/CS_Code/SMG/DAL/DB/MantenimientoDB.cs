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

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de MantenimientoDB
    /// </summary>
    public class MantenimientoDB : BaseDB
    {
        public MantenimientoDB()
        {

        }
        public IDataReader ObtenerMantenimiento(string Contrato, string Pais)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            //string[] aNombre = new string[1] { "@COD_CONTRATO" };
            //DbType[] aTipos = new DbType[1] { DbType.String };
            //string[] aValores = new string[1] { Contrato };

            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@COD_CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            if (Pais == "1")
            {
                Pais = "ES";
            }
            if (Pais == "2")
            {
                Pais = "PT";
            }

            ParamsName[1] = "@PAIS";
            ParamsValue[1] = Pais;
            ParamsType[1] = DbType.String;

            IDataReader dr = db.RunProcDataReader("[SP_GET_MANTENIMIENTO]", ParamsName, ParamsType, ParamsValue);

            return dr;
        }
        public IDataReader ObtenerMantenimientoSinPais(string Contrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            //string[] aNombre = new string[1] { "@COD_CONTRATO" };
            //DbType[] aTipos = new DbType[1] { DbType.String };
            //string[] aValores = new string[1] { Contrato };

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@COD_CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            IDataReader dr = db.RunProcDataReader("[SP_GET_MANTENIMIENTO_SIN_PAIS]", ParamsName, ParamsType, ParamsValue);

            return dr;
        }

        public IDataReader ObtenerMantenimientoPorCodigoReceptor(string codContrato,string codReceptor,string codSociedad)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            //string[] aNombre = new string[1] { "@COD_CONTRATO" };
            //DbType[] aTipos = new DbType[1] { DbType.String };
            //string[] aValores = new string[1] { Contrato };

            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@COD_CONTRATO";
            ParamsValue[0] = codContrato;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@COD_RECEPTOR";
            ParamsValue[1] = codReceptor;
            ParamsType[1] = DbType.String;

            ParamsName[2] = "@COD_SOCIEDAD";
            ParamsValue[2] = codSociedad;
            ParamsType[2] = DbType.String;
            
            

            IDataReader dr = db.RunProcDataReader("spGetMantenimientoPorCodReceptor", ParamsName, ParamsType, ParamsValue);

            return dr;
        }
        

        public IDataReader ObtenerDatosMantenimientoPorcontrato(string Contrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            //string[] aNombre = new string[1] { "@COD_CONTRATO" };
            //DbType[] aTipos = new DbType[1] { DbType.String };
            //string[] aValores = new string[1] { Contrato };

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@COD_CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            IDataReader dr = db.RunProcDataReader("[SP_GET_DATOSMANTENIMIENTO]", ParamsName, ParamsType, ParamsValue);

            return dr;
        }
         
        public IDataReader GetSolicitudesPorContrato(string Contrato, int idIdioma)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            //string[] aNombre = new string[1] { "@COD_CONTRATO" };
            //DbType[] aTipos = new DbType[1] { DbType.String };
            //string[] aValores = new string[1] { Contrato };


            //IDataReader dr = db.RunProcDataReader("", aNombre, aTipos, aValores);
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@COD_CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@idIDIOMA";
            ParamsValue[1] = idIdioma;
            ParamsType[1] = DbType.Int16;

            IDataReader dr = db.RunProcDataReader("MTOGASBD_GetSolicitudesPorContrato", ParamsName, ParamsType, ParamsValue);

            return dr;
        }

        public String GetContratoPorSolicitud(string Solicitud)
        {
            IDataReader dr = null;

            try
            {
                string[] ParamsName = new string[1];
                DbType[] ParamsType = new DbType[1];
                object[] ParamsValue = new object[1];

                ParamsName[0] = "@ID_SOLICITUD";
                ParamsValue[0] = Solicitud;
                ParamsType[0] = DbType.Int64;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                dr = db.RunProcDataReader("SP_GET_CONTRATO_POR_SOLICITUD", ParamsName, ParamsType, ParamsValue);

                if (dr.Read())
                {
                    return (String)DataBaseUtils.GetDataReaderColumnValue(dr, "Cod_contrato");
                }
                else
                {
                    return string.Empty;
                }
            }
            catch 
            {
                return string.Empty;
            }
            finally 
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }

        public String GetContratoPorDNICUPS(string DNI, string CUPS)
        {
            IDataReader dr = null;

            try
            {
                string[] ParamsName = new string[2];
                DbType[] ParamsType = new DbType[2];
                object[] ParamsValue = new object[2];

                ParamsName[0] = "@DNI";
                ParamsValue[0] = DNI;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@CUPS";
                ParamsValue[1] = CUPS;
                ParamsType[1] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                dr = db.RunProcDataReader("SP_GET_CONTRATO_POR_DNICUPS", ParamsName, ParamsType, ParamsValue);

                if (dr.Read())
                {
                    return (String)DataBaseUtils.GetDataReaderColumnValue(dr, "Cod_contrato_SIC");
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }
        
        public Int64 ActualizarTelefonosMantenimiento(String Contrato, String Telefono1,String Telefono2)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@TELEFONO1";
            ParamsValue[1] = Telefono1;
            ParamsType[1] = DbType.String;

            ParamsName[2] = "@TELEFONO2";
            ParamsValue[2] = Telefono2;
            ParamsType[2] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_ACTUALIZAR_TELEFONOS_MANTENIMIENTO", ParamsName, ParamsType, ParamsValue);
            return 0;

        }

        public Int64 ActualizarFechaBajaContrato(String Contrato)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_ACTUALIZAR_FECHABAJA_MANTENIMIENTO", ParamsName, ParamsType, ParamsValue);
            return 0;

        }

        public Int64 Insertar_EmailGCEnviados(int idSolicitud)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@ID_SOLICTUD";
            ParamsValue[0] = idSolicitud;
            ParamsType[0] = DbType.Int32;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_MACRO_INSERT_EmailGCEnviados", ParamsName, ParamsType, ParamsValue);
            return 0;

        }


        public string Get_EmailGCEnviados(int idSolicitud)
        {
            IDataReader dr = null;

            try
            {
                string[] ParamsName = new string[1];
                DbType[] ParamsType = new DbType[1];
                object[] ParamsValue = new object[1];

                ParamsName[0] = "@ID_SOLICTUD";
                ParamsValue[0] = idSolicitud;
                ParamsType[0] = DbType.Int32;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                dr = db.RunProcDataReader("SP_MACRO_GET_EmailGCEnviados", ParamsName, ParamsType, ParamsValue);

                if (dr.Read())
                {
                    return DataBaseUtils.GetDataReaderColumnValue(dr, "fecha_envio").ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }


        public Int64 ActualizarFechaHastaFacturacion(String Contrato)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_ACTUALIZAR_FECHAFACTURACION_MANTENIMIENTO", ParamsName, ParamsType, ParamsValue);
            return 0;

        }

        public Int64 ActualizarFechaBajaServicio(String Contrato,String Usuario)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@USUARIO";
            ParamsValue[1] = Usuario;
            ParamsType[1] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_ACTUALIZAR_FECHABAJASERVICIO_MANTENIMIENTO", ParamsName, ParamsType, ParamsValue);
            return 0;

        }

        public Int64 InsertarAviso(String Contrato, Int16 CodVisita, String Aviso, String Usuario)
        {
            try
            {
                string[] ParamsName = new string[4];
                DbType[] ParamsType = new DbType[4];
                object[] ParamsValue = new object[4];

                ParamsName[0] = "@COD_CONTRATO";
                ParamsValue[0] = Contrato;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@COD_VISITA";
                ParamsValue[1] = CodVisita;
                ParamsType[1] = DbType.Int16;

                ParamsName[2] = "@AVISO";
                ParamsValue[2] = Aviso;
                ParamsType[2] = DbType.String;

                ParamsName[3] = "@USUARIO";
                ParamsValue[3] = Usuario;
                ParamsType[3] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                db.RunProcEscalar("SP_INSERT_VISITA_AVISO", ParamsName, ParamsType, ParamsValue);
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            
                
            
        }

        public Int64 InsertarFacturacionBajas(String Contrato, Int32 CodVisita)
        {
            try
            {
                string[] ParamsName = new string[2];
                DbType[] ParamsType = new DbType[2];
                object[] ParamsValue = new object[2];

                ParamsName[0] = "@COD_CONTRATO";
                ParamsValue[0] = Contrato;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@COD_VISITA";
                ParamsValue[1] = CodVisita;
                ParamsType[1] = DbType.Int16;


                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                db.RunProcEscalar("SP_INSERT_FACTURACION_BAJAS", ParamsName, ParamsType, ParamsValue);
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 0;

        }

        public Int64 InsertarAvisoSolicitud(string idSolicitud,string aviso,string usuario)
        {
            try
            {
                string[] ParamsName = new string[3];
                DbType[] ParamsType = new DbType[3];
                object[] ParamsValue = new object[3];

                ParamsName[0] = "@ID_SOLICITUD";
                ParamsValue[0] = idSolicitud;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@AVISO";
                ParamsValue[1] = aviso;
                ParamsType[1] = DbType.String;

                ParamsName[2] = "@USUARIO";
                ParamsValue[2] = usuario;
                ParamsType[2] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                db.RunProcEscalar("SP_INSERT_SOLICITUD_AVISO", ParamsName, ParamsType, ParamsValue);
            }
            catch (Exception ex)
            {

            }
            
            return 0;
        }

        public string InsertarSolicitudCaldera(string contrato, string usuario)
        {
            try
            {
                string[] ParamsName = new string[2];
                DbType[] ParamsType = new DbType[2];
                object[] ParamsValue = new object[2];

                ParamsName[0] = "@Cod_contrato";
                ParamsValue[0] = contrato;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@USUARIO";
                ParamsValue[1] = usuario;
                ParamsType[1] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                return db.RunProcEscalar("SP_MACRO_INSERTAR_SOLICITUD_CALDERAS", ParamsName, ParamsType, ParamsValue).ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
            
        }

        public void InsertarSolicitudWSCreadaPorWebEnEnviados(string contrato, string idSolicitud)
        {
            try
            {
                string[] ParamsName = new string[2];
                DbType[] ParamsType = new DbType[2];
                object[] ParamsValue = new object[2];

                ParamsName[0] = "@contrato";
                ParamsValue[0] = contrato;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@idSolicitud";
                ParamsValue[1] = idSolicitud;
                ParamsType[1] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                db.RunProcEscalar("spInsertSolicitudesGCCreadasPorWebEnviadasAlWSDelProveedor", ParamsName, ParamsType, ParamsValue).ToString();
            }
            catch (Exception ex)
            {
                
            }
        }
        
        public string IdSolicitudCalderaOperativa(string contrato)
        {
            try
            {
                string[] ParamsName = new string[1];
                DbType[] ParamsType = new DbType[1];
                object[] ParamsValue = new object[1];

                ParamsName[0] = "@Cod_contrato";
                ParamsValue[0] = contrato;
                ParamsType[0] = DbType.String;


                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                return db.RunProcEscalar("SP_GET_SOLICITUD_CALDERAS_OPERATIVA", ParamsName, ParamsType, ParamsValue).ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }

        }

        public IDataReader ObtenerAviso(String Contrato,Int16 Visita)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@COD_CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@COD_VISITA";
            ParamsValue[1] = Visita;
            ParamsType[1] = DbType.Int16;


            IDataReader dr = db.RunProcDataReader("SP_OBTENER_VISITA_AVISO", ParamsName, ParamsType, ParamsValue);

            return dr;
        }


        public IDataReader ObtenerAvisoSolicitud(string idSolicitud)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@ID_SOLICITUD";
            ParamsValue[0] = idSolicitud;
            ParamsType[0] = DbType.String;


            IDataReader dr = db.RunProcDataReader("SP_OBTENER_SOLICITUD_AVISO", ParamsName, ParamsType, ParamsValue);

            return dr;
        }

        public String ObtenerProvinciaPorContrato(String Contrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODCONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            IDataReader dr = db.RunProcDataReader("SP_GET_NOMPROVICIA_POR_CONTRATO", ParamsName, ParamsType, ParamsValue);

            String Provincia = "";
            while (dr.Read())
            {
                Provincia = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE");
            }
            
            return Provincia;
        }

        public Int64 InsertarValoracioncuadroMando(String Cod_proveedor, Double ValoracionVisita, Double ValoracionAveria, Double CarteraAmpliado, Double CarteraGasConfort)
        {
            try
            {
                string[] ParamsName = new string[5];
                DbType[] ParamsType = new DbType[5];
                object[] ParamsValue = new object[5];

                ParamsName[0] = "@COD_PROVEEDOR";
                ParamsValue[0] = Cod_proveedor;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@VALORACIONVISITA";
                ParamsValue[1] = ValoracionVisita;
                ParamsType[1] = DbType.Double;

                ParamsName[2] = "@CARTERAAMPLIADO";
                ParamsValue[2] = CarteraAmpliado;
                ParamsType[2] = DbType.Double;

                ParamsName[3] = "@CARTERAGASCONFORT";
                ParamsValue[3] = CarteraGasConfort;
                ParamsType[3] = DbType.Double;

                ParamsName[4] = "@VALORACIONAVERIA";
                ParamsValue[4] = ValoracionAveria;
                ParamsType[4] = DbType.Double;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                db.RunProcEscalar("LIQUIDACION_INSERT_VALORACION_CUADRO_MANDO", ParamsName, ParamsType, ParamsValue);
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public IDataReader ObtenerDatosMantenimientoWS(string Contrato, string idSolicitud)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@COD_CONTRATO";
            ParamsValue[0] = Contrato;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@ID_SOLICITUD";
            ParamsValue[1] = idSolicitud;
            ParamsType[1] = DbType.Int32;

            IDataReader dr = db.RunProcDataReader("[SP_GET_DATOS_MANTENIMIENTO_WS]", ParamsName, ParamsType, ParamsValue);

            return dr;
        }

        public IDataReader GetSubtipoSolicitudesProteccionGasTelefono(int idIdioma)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@idIDIOMA";
            ParamsValue[0] = idIdioma;
            ParamsType[0] = DbType.Int32;

            IDataReader dr = db.RunProcDataReader("[SP_GET_SUBTIPO_SOLIC_PROTEC_GAS_TELEF]", ParamsName, ParamsType, ParamsValue);

            return dr;
        }

        public IDataReader GetSubtipoSolicitudesInspeccion(int idIdioma)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@idIDIOMA";
            ParamsValue[0] = idIdioma;
            ParamsType[0] = DbType.Int32;

            IDataReader dr = db.RunProcDataReader("SP_GET_SUBTIPO_SOLIC_INSPECCION", ParamsName, ParamsType, ParamsValue);

            return dr;
        }

        public IDataReader GetSubtipoSolicitudesAGI(int idIdioma)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@idIDIOMA";
            ParamsValue[0] = idIdioma;
            ParamsType[0] = DbType.Int32;

            IDataReader dr = db.RunProcDataReader("SP_GET_SUBTIPO_SOLIC_AGI", ParamsName, ParamsType, ParamsValue);

            return dr;
        }

        //20200131 BGN ADD BEG [R#21821]: Parametrizar envio mails por la web
        public Int64 Insertar_EmailGC_NoEnviado(int idSolicitud)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@ID_SOLICTUD";
            ParamsValue[0] = idSolicitud;
            ParamsType[0] = DbType.Int32;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_MACRO_INSERT_EMAIL_GC_NO_ENVIADO", ParamsName, ParamsType, ParamsValue);
            return 0;

        }
        //20200131 BGN ADD END [R#21821]: Parametrizar envio mails por la web

        //20200629 BGN ADD BEG [R#24254] Permitir modificar carencia
        public Boolean GetCarenciaPorContrato(string contrato)
        {
            try
            {
                string[] ParamsName = new string[1];
                DbType[] ParamsType = new DbType[1];
                object[] ParamsValue = new object[1];

                ParamsName[0] = "@CODCONTRATO";
                ParamsValue[0] = contrato;
                ParamsType[0] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                return Convert.ToBoolean(db.RunProcEscalar("SP_GET_CARENCIA_POR_CONTRATO", ParamsName, ParamsType, ParamsValue).ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //20200629 BGN ADD END [R#24254] Permitir modificar carencia

        //20200629 BGN ADD BEG [R#24255] Permitir modificar carencia desde la web
        public Int64 ActualizarCarenciaContrato(String Contrato, Boolean carencia, string usuario)
        {
            try
            {
                string[] ParamsName = new string[3];
                DbType[] ParamsType = new DbType[3];
                object[] ParamsValue = new object[3];

                ParamsName[0] = "@CODCONTRATO";
                ParamsValue[0] = Contrato;
                ParamsType[0] = DbType.String;

                ParamsName[1] = "@CARENCIA";
                ParamsValue[1] = carencia;
                ParamsType[1] = DbType.Boolean;

                ParamsName[2] = "@USUARIO";
                ParamsValue[2] = usuario;
                ParamsType[2] = DbType.String;

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                db.RunProcEscalar("SP_ACTUALIZAR_CARENCIA_POR_CONTRATO", ParamsName, ParamsType, ParamsValue);
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        //20200629 BGN ADD END [R#24255] Permitir modificar carencia desde la web


        //20210624 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
        public Boolean esGasConfort(string codContrato)
        {           
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] aNombre = new string[1];
            DbType[] aTipo = new DbType[1];
            string[] aValores = new string[1];

            aNombre[0] = "@pCOD_CONTRATO";
            aTipo[0] = DbType.String;
            aValores[0] = codContrato;

            IDataReader idrDatosEFV = db.RunProcDataReader("SP_ES_GAS_CONFORT", aNombre, aTipo, aValores);

            Boolean esGasConfort = false;
            while (idrDatosEFV.Read())
            {
                esGasConfort = Boolean.Parse(DataBaseUtils.GetDataReaderColumnValue(idrDatosEFV, "GASCONFORT").ToString());
            }

            return esGasConfort;
        }
        //20210624 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion
    }
}