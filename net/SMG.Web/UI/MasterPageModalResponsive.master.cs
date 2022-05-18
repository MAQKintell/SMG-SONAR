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
using Iberdrola.Commons.Web;

namespace Iberdrola.SMG.UI
{
    public partial class MasterPageModalResponsive : MasterPageBase
    {
        private string m_Titulo;
        public string Titulo
        {
            get
            {
                return m_Titulo;
            }
            set
            {
                m_Titulo = value;
                this.tituloVentana.Text = value;
            }
        }

        public MasterPageModalResponsive()
        {

        }

        public override PlaceHolder PlaceHolderScript
        {
            get { return this._PlaceHolderScript; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			

        }

        public void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
        }

        public void CerrarVentanaModal()
        {
           this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>parent.VentanaModal.cerrar();</script>"));
        }
    }
}
