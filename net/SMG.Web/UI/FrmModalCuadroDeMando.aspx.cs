using Iberdrola.Commons.Configuration;
using System.Drawing;

using System;
using System.Collections.Generic;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Logging;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Collections;
using System.Data;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Reporting;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Globalization;
using System.Web.UI.HtmlControls;
using Iberdrola.Commons.DataAccess;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalCuadroDeMando : FrmBase
    {
        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    UsuarioDB usuarioDB = new UsuarioDB();
                    CargaComboProveedores();
                    this.cmbProveedor.DataBind();
                }
            }
            catch (Exception ex)
            {
                //ManageException(ex);
            }
        }
        #endregion

        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            // coger los valores de sessión

            // cargar los valores en el formulario

            // eliminar de sesión los valores del formulario
        }

        public override void SaveSessionData()
        {
            // eliminar de sesión los valores del formulario si existían

            // coger los valores en el formulario

            // cargar los valores en sessión
        }
        public override void DeleteSessionData()
        {
            // eliminar de sesión los valores del formulario si existían
        }
        #endregion

        private void CargaComboProveedores()
        {
            cmbProveedor.Items.Clear();

            ListItem defaultItem = new ListItem();

            //'SIEL
            defaultItem = new ListItem();
            defaultItem.Value = "SIE";
            defaultItem.Text = "SIEL";
            cmbProveedor.Items.Insert(0, defaultItem);
            //'MGA
            defaultItem = new ListItem();
            defaultItem.Value = "MGA";
            defaultItem.Text = "MGA";
            cmbProveedor.Items.Insert(0, defaultItem);
            //'MAPFRE
            defaultItem = new ListItem();
            defaultItem.Value = "MAP";
            defaultItem.Text = "MAPFRE";
            cmbProveedor.Items.Insert(0, defaultItem);
            //'ICISA
            defaultItem = new ListItem();
            defaultItem.Value = "ICI";
            defaultItem.Text = "ICISA";
            cmbProveedor.Items.Insert(0, defaultItem);
            //'ACTIVAIS
            defaultItem = new ListItem();
            defaultItem.Value = "ACT";
            defaultItem.Text = "ACTIVAIS";
            cmbProveedor.Items.Insert(0, defaultItem);
            //'APPLUS
            //defaultItem = new ListItem();
            //defaultItem.Value = "APP";
            //defaultItem.Text = "APPLUS";
            //cmbProveedor.Items.Insert(0, defaultItem);
            //'PORTUGAL MAPFRE
            defaultItem = new ListItem();
            defaultItem.Value = "PMA";
            defaultItem.Text = "PMA";
            cmbProveedor.Items.Insert(0, defaultItem);

            defaultItem = new ListItem();
            defaultItem.Value = "TRA";
            defaultItem.Text = "TRADIVEL";
            cmbProveedor.Items.Insert(0, defaultItem);
            //''Vacio
            //'defaultItem = New ListItem
            //'defaultItem.Value = "-1"
            //'defaultItem.Text = "Seleccione un proveedor"
            //'cmbProveedor.Items.Insert(0, defaultItem)
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                //public Int64 InsertarValoracioncuadroMando(Cod_proveedor, Valoracion, CarteraAmpliado, Int16 TipoValoracion, Double CarteraGasConfort)
                Mantenimiento.InsertarValoracioncuadroMando(cmbProveedor.SelectedValue, Double.Parse(txtValoracionVisitas.Text.ToString().Replace(".", ",")), Double.Parse(txtValoracionAverias.Text.ToString().Replace(".", ",")), Double.Parse(txtCarteraNuevoSMG.Text), Double.Parse(txtCarteraGasConfort.Text));
                string Script = "<Script>alert('Proceso Completado');parent.VentanaModal.cerrar();</Script>";//CerrarVentanaModal()
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", Script, false);
            }
            catch (Exception ex)
            {
                string Script1 = "<Script>alert('Compruebe los datos informados);</Script>";//CerrarVentanaModal()
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", Script1, false);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //MasterPageModal mpm = (MasterPageModal)this.Master;
            //mpm.CerrarVentanaModal();
        }

    }
}
