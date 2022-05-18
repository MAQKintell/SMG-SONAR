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
using System.Collections.Generic;
using Iberdrola.Commons.DataAccess;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de BaseDB
    /// </summary>
    public abstract class BaseDB: IDisposable
    {
        bool disposed = false;
        private static string  _SMG_DB = "";

        private static string _Recampa_DB_PRO = "";
        private static string _Valcred_DB_PRO = "";
        private static string _Cerener_DB_PRO = "";
        private static string _GesSolar_DB_PRO = "";
        private static string _Banticip_DB_PRO = "";
        private static string _BSOC_DB_PRO = "";
        private static string _AGP_DB_PRO = "";

        public static string SMG_DB
        {
            get { return BaseDB._SMG_DB; }
        }

        public static string Recampa_DB_PRO
        {
            get { return BaseDB._Recampa_DB_PRO; }
        }

        public static string Valcred_DB_PRO
        {
            get { return BaseDB._Valcred_DB_PRO; }
        }

        public static string Cerener_DB_PRO
        {
            get { return BaseDB._Cerener_DB_PRO; }
        }

        public static string GesSolar_DB_PRO
        {
            get { return BaseDB._GesSolar_DB_PRO; }
        }

        public static string Banticip_DB_PRO
        {
            get { return BaseDB._Banticip_DB_PRO; }
        }

        public static string BSOC_DB_PRO
        {
            get { return BaseDB._BSOC_DB_PRO; }
        }

        public static string AGP_DB_PRO
        {
            get { return BaseDB._AGP_DB_PRO; }
        }
        //protected abstract List<BaseDTO> ObtenerTodos();    

        static BaseDB()
        {
            BaseDB._SMG_DB = ConfigurationManager.AppSettings["SMG_DATA_BASE_NAME"];

            BaseDB._Recampa_DB_PRO = ConfigurationManager.AppSettings["RECAMPA_DATA_BASE_NAME"];
            BaseDB._Valcred_DB_PRO = ConfigurationManager.AppSettings["VALCRED_DATA_BASE_NAME"];
            BaseDB._Cerener_DB_PRO = ConfigurationManager.AppSettings["CERENER_DATA_BASE_NAME"];
            BaseDB._GesSolar_DB_PRO = ConfigurationManager.AppSettings["GESSOLAR_DATA_BASE_NAME"];
            BaseDB._Banticip_DB_PRO = ConfigurationManager.AppSettings["BANTICIP_DATA_BASE_NAME"];
            BaseDB._BSOC_DB_PRO = ConfigurationManager.AppSettings["BSOC_DATA_BASE_NAME"];
            BaseDB._AGP_DB_PRO = ConfigurationManager.AppSettings["AGP_DATA_BASE_NAME"];
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
            }

            disposed = true;
        }

    }
}
