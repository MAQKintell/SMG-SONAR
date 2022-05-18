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

using System.Threading;
using Iberdrola.Commons.Configuration;
using System.Xml.Linq;
using System.Xml;
using System.Drawing;


namespace Iberdrola.SMG.UI
{
    public partial class FrmModalActualizarNumeroFactura : FrmBaseListado
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            if (!IsPostBack)
            {
                hdnID.Value = Request.QueryString["Id"];
                hdnFecha.Value = Request.QueryString["Fecha"];
                hdnProveedor.Value = Request.QueryString["Proveedor"];
            }
        }

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


        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", alerta, false);
        }

        protected void OnBtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                String proveedor = hdnProveedor.Value.ToString();// usuarioDTO.NombreProveedor;
                String Fecha;
                String[] FechaAComprobar = new String[3];
                VisitasDB Visita = new VisitasDB();

                switch (int.Parse(hdnID.Value.ToString()))
                {
                    case 0:
                        if (txtNumFactura.Text != "")
                        {
                            FechaAComprobar = hdnFecha.Value.Split('/');
                            Fecha = FechaAComprobar[2] + '-' + FechaAComprobar[1] + '-' + FechaAComprobar[0];

                            Visita.ActualizarVisitasaFacturar(proveedor, Fecha, txtNumFactura.Text);
                            MostrarMensaje("Proceso Completado");

                            MasterPageModal mpm = (MasterPageModal)this.Master;
                            mpm.CerrarVentanaModal();
                        }
                        else
                        {
                            //Debe de meter un valor en el num factura.
                            MostrarMensaje("Debe de indicar un valor para el Número de Factura");
                        }
                        break;
                    case 1:
                        if (txtNumFactura.Text != "")
                        {
                            FechaAComprobar = hdnFecha.Value.Split('/');
                            Fecha = FechaAComprobar[2] + '-' + FechaAComprobar[1] + '-' + FechaAComprobar[0];

                            Visita.ActualizarReparacionesaFacturar(proveedor, Fecha, txtNumFactura.Text);
                            MostrarMensaje("Proceso Completado");

                            MasterPageModal mpm = (MasterPageModal)this.Master;
                            mpm.CerrarVentanaModal();
                        }
                        else
                        {
                            //Debe de meter un valor en el num factura.   
                            MostrarMensaje("Debe de indicar un valor para el Número de Factura");
                        }
                        break;
                }
             }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnBtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
    }
}
