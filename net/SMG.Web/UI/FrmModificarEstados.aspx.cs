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
    public partial class FrmModificarEstados : FrmBase
    {
        public string CodContrato
        {
            set { this.txtContrato.Text = value; }
            get { return this.txtContrato.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (NavigationController.IsBackward())
                    {
                        this.LoadSessionData();
                        this.DeleteSessionData();
                    }
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        protected void OnBtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.Empty.Equals(this.txtContrato.Text))
                {
                    //Completamos con cero por delante
                    txtContrato.Text = CompletarCodigoContrato(txtContrato.Text);
                }

                dgSolicitudes.DataSource = null;
                dgSolicitudes.DataBind();

                dgVisitas.DataSource = null;
                dgVisitas.DataBind();

                if (this.ValidatePage())
                {
                    //Obtenemos los datos de las solicitudes y las visitas
                    CargarDatosContrato();
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        protected void OnBtnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                txtContrato.Text = string.Empty;
                txtIdSolicitud.Text = string.Empty;

                dgSolicitudes.DataSource = null;
                dgSolicitudes.DataBind();

                dgVisitas.DataSource = null;
                dgVisitas.DataBind();

                this.DeleteSessionData();
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        /// <summary>
        /// Devuelve los datos de visita y solicitudes seleccionados
        /// </summary>
        private void CargarDatosContrato()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            DataSet ds = null;
            // Si está informado el ID de solicitud, entonces buscamos la solicitud directamente.
            if (!string.IsNullOrEmpty(this.txtIdSolicitud.Text))
            {
                
                SolicitudesDB solDB = new SolicitudesDB();
                ds = solDB.GetSolicitudesPorIDSolicitud (this.txtIdSolicitud.Text,usuarioDTO.IdIdioma);
            
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.dgSolicitudes.DataSource = ds.Tables[0];
                    this.dgSolicitudes.DataBind();
                }

            }
            //Si esta informado el contrato, entonces buscamos las solicitudes de ese contrato
            else if (!string.IsNullOrEmpty(this.txtContrato.Text))
            {
                SolicitudesDB solDB = new SolicitudesDB();
                ds = solDB.GetSolicitudesPorContrato(this.CodContrato,usuarioDTO.IdIdioma);
            
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.dgSolicitudes.DataSource = ds.Tables[0];
                    this.dgSolicitudes.DataBind();
                }

            }
            

            VisitasDB visDB = new VisitasDB();

            if (string.IsNullOrEmpty(this.txtContrato.Text))
            {
                this.CodContrato = ObtenerContratoSolicitud();
            }

            
            

            IDataReader dr = visDB.ObtenerVisitas(this.CodContrato, (Int16)usuarioDTO.IdIdioma);
            this.dgVisitas.DataSource = dr;
            this.dgVisitas.DataBind();

            if (ds.Tables[0].Rows.Count == 0 && dgVisitas.Rows.Count == 0)
            {
                this.ShowMessage("No se han encontrado solicitudes / visitas para el contrato indicado");
            }
        }

        private string ObtenerContratoSolicitud()
        {
            IDataReader dr = null;

            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = null;
                usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                SolicitudesDB solDB = new SolicitudesDB();
                dr = solDB.GetSingleSolicitudes(this.txtIdSolicitud.Text,usuarioDTO.IdIdioma);

                if (dr.Read())
                {
                    return (string)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_CONTRATO");
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }

        protected void dgSolicitudes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                    e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        protected void dgVisitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                    e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        protected void OnSolicitudesRowSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().FullName.ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                {
                    LinkButton lbtn = (LinkButton)sender;
                    hdnSolicitud.Value = lbtn.Text;
                    //this.CargarDatosContrato(txtContrato.Text, Decimal.Parse(hdnSolicitud.Value));
                    //this.CargarDatosSolicitud(Decimal.Parse(hdnSolicitud.Value));
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnVisitasRowSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().FullName.ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                {
                    LinkButton lbtn = (LinkButton)sender;
                    hdnSolicitud.Value = lbtn.Text;
                    //this.CargarDatosContrato(txtContrato.Text, Decimal.Parse(hdnSolicitud.Value));
                    //this.CargarDatosSolicitud(Decimal.Parse(hdnSolicitud.Value));
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtContrato_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtContrato.Text))
                {
                    if (!NumberUtils.IsDecimal(txtContrato.Text))
                    {
                        e.IsValid = false;
                        txtContratoCustomValidator.ToolTip = "El contrato debe ser numérico";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtIdSolicitud.Text))
                    {
                        e.IsValid = false;
                        txtContratoCustomValidator.ToolTip = "Rellene alguno de los criterios de búsqueda";
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtIdSolicitud_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtIdSolicitud.Text))
                {
                    if (!NumberUtils.IsDecimal(txtIdSolicitud.Text))
                    {
                        e.IsValid = false;
                        txtIdSolicitudCustomValidator.ToolTip = "El id de solicitud debe ser numérico";
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void dgSolicitudes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    
                    switch (e.CommandName)
                    {
                        case "Editar":
                            this.SaveSessionData();
                            //20210924 BGN MOD BEG R#31319 - No permitir reabrir solicitudes de GC ya enviadas a anular o activar.
                            string idSolicitud = e.CommandArgument.ToString();
                            Solicitud sol = new Solicitud();
                            Boolean enviada = sol.SolicitudGCEnviadaDelta(idSolicitud);
                            if (enviada)
                            {
                                this.ShowMessage("La solicitud ha sido enviada a DELTA por lo que no se puede modificar el estado. Consulte con los administradores de la aplicación.");
                            }
                            else
                            {
                                NavigationController.Forward("./FrmModificarEstadosSolicitud.aspx?IdSolicitud=" + idSolicitud);
                            }                            
                            //20210924 BGN MOD END R#31319 - No permitir reabrir solicitudes de GC ya enviadas a anular o activar.
                            break;                    
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        
        protected void dgVisitas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    //UserDTO usuario = Users.ObtenerUsuario(e.CommandArgument.ToString());

                    switch (e.CommandName)
                    {
                        case "Editar":
                            this.SaveSessionData();
                            NavigationController.Forward("./FrmModificarEstadosSolicitud.aspx?IdVisita=" + e.CommandArgument.ToString() + "&CodContrato=" + this.txtContrato.Text);
                            break;                        
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            this.txtContrato.Text = (string)CurrentSession.GetAttribute("FRM_MODIF_ESTADOS_COD_CONTRATO");

            if (!string.IsNullOrEmpty(this.txtContrato.Text))
            {
                CargarDatosContrato();
            }
        }

        public override void SaveSessionData()
        {
            CurrentSession.SetAttribute("FRM_MODIF_ESTADOS_COD_CONTRATO", this.txtContrato.Text);
        }

        public override void DeleteSessionData()
        {
            CurrentSession.RemoveAttribute("FRM_MODIF_ESTADOS_COD_CONTRATO");
        }
        #endregion

        #region Métodos privados
        private String CompletarCodigoContrato(String Contrato)
        {
            Contrato = Contrato.Trim();
            while (Contrato.Length < 10)
            {
                Contrato = String.Concat("0", Contrato);
            }
            return Contrato;
        }
        #endregion
    }
}
