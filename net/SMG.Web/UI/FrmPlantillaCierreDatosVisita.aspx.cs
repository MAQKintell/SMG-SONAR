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
using System.Collections;
using System.Data.Common;
using System.Text;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Exceptions;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;


public partial class UI_FrmPlantillaCierreDatosVisita : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Cargamos el contrato y el código de la visita.

    }

    #region BOTONES
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            // Guardamos las caracteristicas.
            GuardarCaracteristicas();
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            String script = "<script language='Javascript'>window.close();</script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CerrarVentanaCierreDatosvisita", script, false);
        }
    #endregion

    #region Funciones
        protected void GuardarCaracteristicas()
        {
            int tipCar = 0;
            String valor = "";
            try
            {
                // Checkbox, caracteristicas duplicadas, comprobar si esta seleccionada la que tiene SI o la que tiene NO.
                ////2//48//49//50//54//52//57//89//58//88//59//90//60//56//61
                // En estos casos, metemos registro en la BBDD solo si el valor del checked es true, porque al estar duplicadas nos valdria con guardar las de true.
                // Si es checkbox, EN GENERAL, solo guardamos las que tienen a true el checked, asi nos libramos de posibles contradicciones.
                foreach (Control c in form1.Controls)
                {
                    if (c.ID != null)
                    {
                        if (c.ID.ToString().Substring(0, 3) == "txt")
                        {
                            if (c.ID.ToString().IndexOf('_') >= 0)
                            {
                                TextBox texto = (TextBox)c;
                                string[] idControles = c.ID.ToString().Split('_');
                                tipCar = int.Parse(idControles[idControles.Length - 1].ToString());

                                if (FormValidation.TextBoxHasValue(texto))
                                {
                                    valor = texto.Text;
                                }
                            }
                        }
                        if (c.ID.ToString().Substring(0, 3) == "chk")
                        {
                            if (c.ID.ToString().IndexOf('_') >= 0)
                            {
                                CheckBox check = (CheckBox)c;
                                string[] idControles = c.ID.ToString().Split('_');
                                tipCar = int.Parse(idControles[idControles.Length - 1].ToString());

                                if (check.Checked)
                                {
                                    valor = "True";
                                }
                            }
                        }
                        if (c.ID.ToString().Substring(0, 3) == "cmb")
                        {
                            if (c.ID.ToString().IndexOf('_') >= 0)
                            {
                                DropDownList combo = (DropDownList)c;
                                string[] idControles = c.ID.ToString().Split('_');
                                tipCar = int.Parse(idControles[idControles.Length - 1].ToString());

                                if (combo.SelectedIndex != -1)
                                {
                                    valor = combo.SelectedValue;
                                }
                            }
                        }

                    }
                    if (valor != "")
                    {
                        CaracteristicaHistorico.AddCaracteristicaCierreVisita(txtcontrato.Text, int.Parse(hdnCodVisita.Value.ToString()), tipCar, valor);
                    }
                    valor = "";
                    tipCar = 0;
                }
            }
            catch (Exception ex)
            {
            }
        }
    #endregion
}

