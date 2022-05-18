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
    public partial class MasterPageModal : MasterPageBase
    {
        public override PlaceHolder PlaceHolderScript
        {
            get { return _PlaceHolderScript; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			

        }


        public void CerrarVentanaModal()
        {
           this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>parent.VentanaModal.cerrar();</script>"));
        }
    }
}
