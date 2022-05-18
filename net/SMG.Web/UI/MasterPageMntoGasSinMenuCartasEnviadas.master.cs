using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Iberdrola.Commons.Security;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Web;
using Iberdrola.Commons.Logging;
using Iberdrola.SMG.DAL.DTO;

using Iberdrola.SMG.BLL;
namespace Iberdrola.SMG.UI
{
    public partial class MasterPageMntoGasSinMenuCartasEnviadas : MasterPageBase
    {
        public MasterPageMntoGasSinMenuCartasEnviadas()
        {

        }

        public override PlaceHolder PlaceHolderScript
        {
            get { return this._PlaceHolderScript; }
        }
     
        #region atributos
        private static string strVentanaActiva = "";
        #endregion

        #region propiedades
        public string VentanaActiva
        {
            set { MasterPageMntoGasSinMenu.strVentanaActiva = value; }
            get { return MasterPageMntoGasSinMenu.strVentanaActiva; }
        }

        #endregion

        #region metodos
        public bool EsVentanaActiva(string nombreVentana)
        {
            return MasterPageMntoGasSinMenu.strVentanaActiva.Equals(nombreVentana);
        }
        #endregion

        #region metodos abstractos
        //public abstract List<String> ValidateForm();
        #endregion   

        #region Métodos de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
        }

        protected void btnDesconectar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.User = null;

            CurrentSession.Clear();

            MasterPageMntoGasSinMenu.strVentanaActiva = "";
            NavigationController.ClearHistory();
            Response.Redirect("../Login.aspx");
        }

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            MasterPageMntoGasSinMenu.strVentanaActiva = "";
            NavigationController.ClearHistory();
            Response.Redirect("../MenuAcceso.aspx");
        }        

        #endregion
    }
}
