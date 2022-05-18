using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Iberdrola.Commons.Reporting;
using Iberdrola.Commons.Web;

using System.Text;
using System.IO;


using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.BLL;

namespace Iberdrola.SMG.UI
{
    public partial class AvisoSolicitudes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
                //AsignarCultura Cultur = new AsignarCultura();
                //Cultur.Asignar(Page);

                hdnSolicitud.Value = Request.QueryString["Solicitud"];
                hdnMail.Value = Request.QueryString["mail"];


                IDataReader Datos = Mantenimiento.ObtenerAvisoSolicitud(hdnSolicitud.Value.ToString());
                while (Datos.Read())
                {
                    String Aviso = (String)DataBaseUtils.GetDataReaderColumnValue(Datos, "Aviso");
                    txtAvisosAnteriores.Text += Aviso + Environment.NewLine;
                    if (hdnMail.Value == "") { hdnMail.Value = (String)DataBaseUtils.GetDataReaderColumnValue(Datos, "email_aviso"); }
                    hdnContrato.Value = (String)DataBaseUtils.GetDataReaderColumnValue(Datos, "Contrato");
                }
                Datos.Close();
                
			}
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            String usuario = CurrentSession.GetAttribute("usuarioValido").ToString();
            Mantenimiento.InsertarAvisoSolicitud(hdnSolicitud.Value, "[" + DateTime.Now.ToString() + "] " + txtAviso.Text, usuario);

            String Destino = hdnMail.Value;
            if (Destino != "")
            {
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                //MandarMail(txtAviso.Text + ".<br /> <br /> Se ha dado de alta un nuevo aviso para el Solicitud: " + hdnSolicitud.Value.ToString() + " - Contrato " + hdnContrato.Value.ToString(), "Nuevo Aviso - Contrato " + hdnContrato.Value.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(hdnContrato.Value.ToString()), Destino, true);
                string asunto = "Nuevo Aviso - Contrato " + hdnContrato.Value.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(hdnContrato.Value.ToString());
                string mensaje = txtAviso.Text + ".<br /> <br /> Se ha dado de alta un nuevo aviso para el Solicitud: " + hdnSolicitud.Value.ToString() + " - Contrato " + hdnContrato.Value.ToString();
                UtilidadesMail.EnviarAviso(asunto, mensaje, "tcservicios@iberdrola.es", Destino);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
            String Script = "<Script>alert('Proceso Completado');parent.VentanaModal.cerrar();</Script>";//CerrarVentanaModal()
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", Script, false);
        }

    }
}
