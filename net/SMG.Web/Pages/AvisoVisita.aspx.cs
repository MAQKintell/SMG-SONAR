﻿using System;
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
    public partial class AvisoVisita : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
                //AsignarCultura Cultur = new AsignarCultura();
                //Cultur.Asignar(Page);

                hdnContrato.Value = Request.QueryString["Contrato"];
                hdnVisita.Value = Request.QueryString["CodVisita"];


                IDataReader Datos = Mantenimiento.ObtenerAviso(hdnContrato.Value, Int16.Parse(hdnVisita.Value.ToString()));
                while (Datos.Read())
                {
                    String Aviso = (String)DataBaseUtils.GetDataReaderColumnValue(Datos, "Aviso");
                    txtAvisosAnteriores.Text += Aviso + Environment.NewLine;
                    hdnMail.Value = (String)DataBaseUtils.GetDataReaderColumnValue(Datos, "email_aviso");
                }
                Datos.Close();
                
			}
        }
       
        protected void Button1_Click(object sender, EventArgs e)
        {
            String usuario = CurrentSession.GetAttribute("usuarioValido").ToString();
            Int64 Procesado = Mantenimiento.InsertarAviso(hdnContrato.Value, Int16.Parse(hdnVisita.Value.ToString()), "[" + DateTime.Now.ToString() + "] " + txtAviso.Text, usuario);
            String Script = "";
            if (Procesado >= 0)
            {
                String Destino = hdnMail.Value;

                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                //MandarMail(txtAviso.Text + ".<br /> <br /> Se ha dado de alta un nuevo aviso para el Contrato: " + hdnContrato.Value.ToString() + " y Visita: " + hdnVisita.Value.ToString(), "Nuevo Aviso - " + hdnContrato.Value.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(hdnContrato.Value.ToString()), Destino, true);
                string asunto = "Nuevo Aviso - " + hdnContrato.Value.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(hdnContrato.Value.ToString());
                string mensaje = txtAviso.Text + ".<br /> <br /> Se ha dado de alta un nuevo aviso para el Contrato: " + hdnContrato.Value.ToString() + " y Visita: " + hdnVisita.Value.ToString();
                UtilidadesMail.EnviarAviso(asunto, mensaje, "tcservicios@iberdrola.es", Destino);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web

                Script = "<Script>alert('Proceso Completado');parent.VentanaModal.cerrar();</Script>";//CerrarVentanaModal()
            }
            else
            {
                Script = "<Script>alert('Proceso NO Completado Correctamente');parent.VentanaModal.cerrar();</Script>";//CerrarVentanaModal()
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", Script, false);
        }

    }
}
