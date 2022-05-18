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
    /// Descripción breve de DocumentosDB
    /// </summary>
    public class DocumentosDB : BaseDB
    {
        public DocumentosDB()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        public Boolean ObtenerFicheroExigible(String Subtipo, String EstadoDestino, String EstadoOrigen)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[3];
                Parametros[0] = "@pSubtipo";
                Parametros[1] = "@pEstadoDestino";
                Parametros[2] = "@pEstadoOrigen";

                DbType[] TiposValoresParametros = new DbType[3];
                TiposValoresParametros[0] = DbType.String;
                TiposValoresParametros[1] = DbType.String;
                TiposValoresParametros[2] = DbType.String;

                Object[] ValoresParametros = new Object[3];
                ValoresParametros[0] = Subtipo;
                ValoresParametros[1] = EstadoDestino;
                ValoresParametros[2] = EstadoOrigen;

                return Convert.ToBoolean(db.RunProcEscalar("spSMGFicheroExigiblePorSubtipoSolicitud", Parametros, TiposValoresParametros, ValoresParametros));
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //public Int32 ObtenerRegistroFicherosPorNombre(String nombreFichero)
        //{
        //    try
        //    {
        //        DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
        //        string[] Parametros = new string[1];
        //        Parametros[0] = "@pNOMBRE_DOCUMENTO";

        //        DbType[] TiposValoresParametros = new DbType[1];
        //        TiposValoresParametros[0] = DbType.String;

        //        Object[] ValoresParametros = new Object[1];
        //        ValoresParametros[0] = nombreFichero;

        //        return Int32.Parse(db.RunProcEscalar("spSMGDocumentoFindNumReg", Parametros, TiposValoresParametros, ValoresParametros).ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public String ObtenerRutaFicheros()
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[0];
                DbType[] TiposValoresParametros = new DbType[0];
                Object[] ValoresParametros = new Object[0];


                return db.RunProcEscalar("spSMGDocumentoFindPath", Parametros, TiposValoresParametros, ValoresParametros).ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public String ObtenerRutaFicherosInspeccion()
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[0];
                DbType[] TiposValoresParametros = new DbType[0];
                Object[] ValoresParametros = new Object[0];


                return db.RunProcEscalar("spSMGDocumentoInspeccionFindPath", Parametros, TiposValoresParametros, ValoresParametros).ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        

        public String ObtenerCCBBPorContrato(string Contrato)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[1];
                DbType[] TiposValoresParametros = new DbType[1];
                Object[] ValoresParametros = new Object[1];

                Parametros[0] = "@CONTRATO";
                TiposValoresParametros[0] = DbType.String;
                ValoresParametros[0] = Contrato;

                return db.RunProcEscalar("spSMGCCBBGetByContrato", Parametros, TiposValoresParametros, ValoresParametros).ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public String ObtenerRutaFicherosTemporal()
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                string[] Parametros = new string[0];
                DbType[] TiposValoresParametros = new DbType[0];
                Object[] ValoresParametros = new Object[0];


                return db.RunProcEscalar("spSMGDocumentoFindTemporalPath", Parametros, TiposValoresParametros, ValoresParametros).ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
    }
}