using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.DataAccess;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;


namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de ContratoDB
    /// </summary>
    public class ContratoDB : BaseDB
    {
        #region constructores
        public ContratoDB()
        {
        }
        #endregion

        #region metodos
        public Int16? ObtenerCodigoUltimaVisita(String codContrato)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                String[] aNombres = new String[1] { "@COD_CONTRATO" };
                DbType[] aTipos = new DbType[1] { DbType.String };
                String[] aValores = new String[1] { codContrato };

                object resultado = db.RunProcEscalar("SP_GET_COD_ULTIMA_VISITA", aNombres, aTipos, aValores);
                if (resultado != null && !resultado.Equals(DBNull.Value))
                {
                    return (Int16?)resultado;
                }
                else
                {
                    return 0;
                }
                 
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2001");
            }
        }

        public Boolean ComprobarSociedad(String contrato)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                String[] aNombres = new String[1] { "@COD_CONTRATO" };
                DbType[] aTipos = new DbType[1] { DbType.String };
                String[] aValores = new String[1] { contrato };

                int resultado = int.Parse(db.RunProcEscalar("spComprobarSociedadPorContrato", aNombres, aTipos, aValores).ToString());

                if (resultado > 0) { return true; }
                else { return false; }
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2001");
            }
        }
        //20200127 BGN ADD BEG [R#20083] Nueva página para informar los contratos a NO Facturar la baja
        public bool FacturacionBajasRegularizacion(string contrato)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[1];
                DbType[] aTipo = new DbType[1];
                string[] aValores = new string[1];

                aNombre[0] = "@COD_CONTRATO";
                aTipo[0] = DbType.String;
                aValores[0] = contrato;

                object result = db.RunProcEscalar("SP_MACRO_FACTURACION_BAJAS_A_REGULARIZAR", aNombre, aTipo, aValores);

                // Retornamos True (ya ha sido facturado) si el SP nos devuelve 1.
                return ((int)result == 1);
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

        public void InsertFacturacionBajasRegularizadas(string contrato, string fichero)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[2];
                DbType[] aTipo = new DbType[2];
                string[] aValores = new string[2];

                aNombre[0] = "@COD_CONTRATO";
                aTipo[0] = DbType.String;
                aValores[0] = contrato;

                aNombre[1] = "@NOMBRE_FICHERO";
                aTipo[1] = DbType.String;
                aValores[1] = fichero;

                db.RunProcEscalar("SP_INSERT_FACTURACION_BAJAS_REGULARIZADAS", aNombre, aTipo, aValores);

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
        //20200127 BGN ADD END [R#20083] Nueva página para informar los contratos a NO Facturar la baja
        #endregion

        public void InsertFacturacionBajasYMarcarContratoParaDarBaja(string contrato,string Usuario, string MotivoBaja)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[3];
                DbType[] aTipo = new DbType[3];
                string[] aValores = new string[3];

                aNombre[0] = "@COD_CONTRATO";
                aTipo[0] = DbType.String;
                aValores[0] = contrato;

                aNombre[1] = "@USUARIO";
                aTipo[1] = DbType.String;
                aValores[1] = Usuario;

                aNombre[2] = "@MOTIVO_BAJA";
                aTipo[2] = DbType.String;
                aValores[2] = MotivoBaja;

                db.RunProcEscalar("spInsertFacturacionBajasYMarcarContratoParaBaja", aNombre, aTipo, aValores);

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

        //20210719 BGN ADD BEG
        public DataTable ObtenerFacturacionBajasPorContrato(string Contrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] aNombre = new string[1];
            aNombre[0] = "@COD_CONTRATO";

            DbType[] aTipo = new DbType[1];
            aTipo[0] = DbType.String;

            Object[] aValores = new Object[1];
            aValores[0] = Contrato;


            DataTable dt = db.RunProcDataTable("SP_GET_FACTURACION_BAJAS", aNombre, aTipo, aValores);

            return dt;
        }
        //20210719 BGN ADD END

        //29122021 KIN BEG
        public DataTable ObtenerEFVRepairAndCarePorId(string codEFV)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] aNombre = new string[1];
            aNombre[0] = "@IDEFV";

            DbType[] aTipo = new DbType[1];
            aTipo[0] = DbType.String;

            Object[] aValores = new Object[1];
            aValores[0] = codEFV;


            DataTable dt = db.RunProcDataTable("spComprobarEFVRepariAndCare", aNombre, aTipo, aValores);

            return dt;
        }

        public DataTable ObtenerEFVGCTemporal(string codEFV)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] aNombre = new string[1];
            aNombre[0] = "@IDEFV";

            DbType[] aTipo = new DbType[1];
            aTipo[0] = DbType.String;

            Object[] aValores = new Object[1];
            aValores[0] = codEFV;


            DataTable dt = db.RunProcDataTable("spComprobarEFVGCTemporal", aNombre, aTipo, aValores);

            return dt;
        }

        public DataTable ObtenerExisteEFVYPermitirAlta(string codEFV)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] aNombre = new string[1];
            aNombre[0] = "@IDEFV";

            DbType[] aTipo = new DbType[1];
            aTipo[0] = DbType.String;

            Object[] aValores = new Object[1];
            aValores[0] = codEFV;


            DataTable dt = db.RunProcDataTable("getSiExisteEFVYPermitirAltaPorWeb", aNombre, aTipo, aValores);

            return dt;
        }
        

        public void AltaContratoEnOpera( MantenimientoDTO mantenimiento,string EFV, string codSociedad)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[27];
                aNombre[0] = "@COD_CONTRATO_SIC";
                aNombre[1] = "@NOM_TITULAR";
                aNombre[2] = "@APELLIDO1";
                aNombre[3] = "@APELLIDO2";
                aNombre[4] = "@COD_PROVINCIA";
                aNombre[5] = "@COD_POBLACION";
                aNombre[6] = "@NOM_CALLE";
                aNombre[7] = "@TIP_VIA_PUBLICA";
                aNombre[8] = "@COD_FINCA";
                aNombre[9] = "@TIP_BIS";
                aNombre[10] = "@COD_PORTAL";
                aNombre[11] = "@TIP_ESCALERA";
                aNombre[12] = "@TIP_PISO";
                aNombre[13] = "@TIP_MANO";
                aNombre[14] = "@COD_POSTAL";
                aNombre[15] = "@ESTADO_CONTRATO";
                aNombre[16] = "@COD_CLIENTE";
                aNombre[17] = "@DNI";
                aNombre[18] = "@COD_RECEPTOR";
                aNombre[19] = "@PAIS";
                aNombre[20] = "@FEC_ALTA_SERVICIO";
                aNombre[21] = "@FEC_BAJA_SERVICIO";
                aNombre[22] = "@NUM_TEL_CLIENTE";
                aNombre[23] = "@NUM_TEL_PS";
                aNombre[24] = "@EFV";
                aNombre[25] = "@COD_SOCIDEDAD";
                aNombre[26] = "@MAIL";

                DbType[] aTipo = new DbType[27];
                aTipo[0] = DbType.String;
                aTipo[1] = DbType.String;
                aTipo[2] = DbType.String;
                aTipo[3] = DbType.String;
                aTipo[4] = DbType.String;
                aTipo[5] = DbType.String;
                aTipo[6] = DbType.String;
                aTipo[7] = DbType.String;
                aTipo[8] = DbType.String;
                aTipo[9] = DbType.String;
                aTipo[10] = DbType.String;
                aTipo[11] = DbType.String;
                aTipo[12] = DbType.String;
                aTipo[13] = DbType.String;
                aTipo[14] = DbType.String;
                aTipo[15] = DbType.String;
                aTipo[16] = DbType.String;
                aTipo[17] = DbType.String;
                aTipo[18] = DbType.String;
                aTipo[19] = DbType.String;
                aTipo[20] = DbType.String;
                aTipo[21] = DbType.String;
                aTipo[22] = DbType.String;
                aTipo[23] = DbType.String;
                aTipo[24] = DbType.String;
                aTipo[25] = DbType.String;
                aTipo[26] = DbType.String;


                string fecAlta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (mantenimiento.FEC_ALTA_SERVICIO != null) { fecAlta = mantenimiento.FEC_ALTA_SERVICIO.ToString(); }
                string fecBaja = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (mantenimiento.FEC_BAJA_SERVICIO != null) { fecBaja = mantenimiento.FEC_BAJA_SERVICIO.ToString(); }

                Object[] aValores = new Object[27];
                aValores[0] = mantenimiento.COD_CONTRATO_SIC;
                aValores[1] = mantenimiento.NOM_TITULAR;
                aValores[2] = mantenimiento.APELLIDO1;
                aValores[3] = mantenimiento.APELLIDO2;
                aValores[4] = mantenimiento.COD_PROVINCIA;
                aValores[5] = mantenimiento.COD_POBLACION;
                aValores[6] = mantenimiento.NOM_CALLE;
                aValores[7] = mantenimiento.TIP_VIA_PUBLICA;
                aValores[8] = mantenimiento.COD_FINCA;
                aValores[9] = mantenimiento.TIP_BIS;
                aValores[10] = mantenimiento.COD_PORTAL;
                aValores[11] = mantenimiento.TIP_ESCALERA;
                aValores[12] = mantenimiento.TIP_PISO;
                aValores[13] = mantenimiento.TIP_MANO;
                aValores[14] = mantenimiento.COD_POSTAL;
                aValores[15] = mantenimiento.ESTADO_CONTRATO;
                aValores[16] = mantenimiento.COD_CLIENTE;
                aValores[17] = mantenimiento.DNI;
                aValores[18] = mantenimiento.COD_RECEPTOR; 
                aValores[19] = mantenimiento.PAIS; 
                aValores[20] = DateTime.Parse(fecAlta).ToString("yyyy-MM-dd HH:mm:ss");
                aValores[21] = DateTime.Parse(fecBaja).ToString("yyyy-MM-dd HH:mm:ss");
                aValores[22] = mantenimiento.NUM_TEL_CLIENTE;
                aValores[23] = mantenimiento.NUM_TEL_PS;
                aValores[24] = EFV;
                aValores[25] = codSociedad;
                aValores[26] = mantenimiento.EMAIL;

                db.RunProcEscalar("spAltaContratoRepairAndCare", aNombre, aTipo, aValores);

            }
            //catch (BaseException)
            //{
            //    throw;
            //}
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2006");
            }
        }

        public void AltaSolicitudInspeccion(String Contrato, String CUI, String Fecha, String Horario, String ORD)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] aNombre = new string[5];
                aNombre[0] = "@CONTRATO";
                aNombre[1] = "@CUI";
                aNombre[2] = "@FECHA";
                aNombre[3] = "@HORARIO";
                aNombre[4] = "@ORD";
              
                DbType[] aTipo = new DbType[5];
                aTipo[0] = DbType.String;
                aTipo[1] = DbType.String;
                aTipo[2] = DbType.String;
                aTipo[3] = DbType.String;
                aTipo[4] = DbType.String;
              
                Object[] aValores = new Object[5];
                aValores[0] = Contrato;
                aValores[1] = CUI;
                aValores[2] = DateTime.Parse(Fecha).ToString("yyyy-MM-dd HH:mm:ss");
                aValores[3] = Horario;
                aValores[4] = ORD;
              

                db.RunProcEscalar("spInsertarSolicitudInspeccion", aNombre, aTipo, aValores);

            }
            //catch (BaseException)
            //{
            //    throw;
            //}
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2006");
            }
        }

        
        //29122021 KIN END

    }
}
