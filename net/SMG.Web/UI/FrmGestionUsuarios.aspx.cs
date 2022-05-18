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

using Iberdrola.Commons.Security;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.UI
{
    public partial class frmGestionUsuarios : FrmBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (!IsPostBack)
                //{
                //    AsignarCultura Cultur = new AsignarCultura();
                //    Cultur.Asignar(Page);
                //}
    			
                if (!NavigationController.IsBackward())
                {
                    // Lo primero que se hace es mirar si el usuario está autenticado
                    // si no es así se redirige al login.
                    ValidarUsuario();
                    
                }
                if (!IsPostBack)
                {
                    this.cboPerfiles.DataValueField = "ID_Perfil";
                    this.cboPerfiles.DataTextField = "Desc_Perfil";
                    Usuarios Usuarios = new Usuarios();
                    this.cboPerfiles.DataSource = Usuarios.ObtenerPerfiles();
                    this.cboPerfiles.DataBind();

                    cmbPais.DataValueField = "ID_PAIS";
                    cmbPais.DataTextField = "DESC_PAIS";
                    Paises paises = new Paises();
                    cmbPais.DataSource= paises.GetTodosLosPaises();
                    cmbPais.DataBind();
                    
                }
                this.txtIDPerfil.Text = this.cboPerfiles.SelectedValue;
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        /// <summary>
        /// La pagina modal ya realiza la validacion, pero solo de si se encuentra autenticado. 
        /// En esta pagina solo pueden entrar los usuarios con permisos, por lo que se comprueba de nuevo
        /// si los usuarios tienen o no permisos.
        /// </summary>
        private void ValidarUsuario()
        {
            
            if (Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_USUARIO_AUTENTIFICADO) != null)
            {
                Boolean UsuarioAutentificado = (Boolean)(Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_USUARIO_AUTENTIFICADO));
                if (UsuarioAutentificado == true)
                {
                    AppPrincipal usuarioPrincipal = (AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                    if (!usuarioDTO.Permiso.HasValue || !usuarioDTO.Permiso.Value)
                    {
                        //Response.Redirect("../MenuAcceso.aspx");
                        
                    }
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
            else
            {
                Response.Redirect("../Login.aspx");
            }
        }

        #region Implementacion metodos abstractos
        public override void LoadSessionData()
        {

        }
        public override void SaveSessionData()
        {

        }
        public override void DeleteSessionData()
        {

        }
        #endregion Iimplementacion metodos abstractos


        protected void bAniadir_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidatePage())
                {
                    UsuarioDTO usuario = new UsuarioDTO();

                    usuario.Login = txtCodigo.Text.Trim();
                    usuario.Nombre = txtNombre.Text.Trim();
                    usuario.Password = txtContrasenia.Text.Trim();// EncryptHelper.Encrypt(txtContrasenia.Text.Trim());
                    usuario.Email = txtEmail.Text.Trim();
                    usuario.Id_Perfil = Int64.Parse(txtIDPerfil.Text.Trim());
                    usuario.Permiso = cbPermiso.Checked;
                    usuario.Pais = cmbPais.SelectedValue;

                    Usuarios.AniadirUsuario(usuario, Usuarios.ObtenerUsuarioLogeado().Login);

                    this.ShowMessage("Usuario dado de alta");
                    LimpiarControles();
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void hdnBtnCargarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioDTO usuario = Usuarios.ObtenerUsuario(hdnUserID.Value);

                txtCodigo.Text = usuario.Login;
                txtNombre.Text = usuario.Nombre;
                txtContrasenia.Text = EncryptHelper.Decrypt(usuario.Password.ToString(),true);
                txtEmail.Text = usuario.Email;
                this.cboPerfiles.SelectedValue = usuario.Id_Perfil.Value.ToString();
                txtIDPerfil.Text = usuario.Id_Perfil.Value.ToString();
                cbPermiso.Checked = usuario.Permiso.Value;
                if (string.IsNullOrEmpty(usuario.Activo))
                {
                    chkActivo.Checked = true;
                    this.lblUsuarioBaja.Text = "";
                    this.lblUsuarioBaja.Visible = false;
                    this.lblBajaInfo.Visible = false;
                }
                else
                {
                    chkActivo.Checked = false;                
                    this.lblUsuarioBaja.Text = Usuarios.UsuarioTramitadorBaja(usuario.Login);
                    this.lblUsuarioBaja.Visible = true;
                    this.lblBajaInfo.Visible = true;
                }
                this.cmbPais.SelectedValue = usuario.Pais;

                bAniadir.Enabled = false;
                bEliminar.Enabled = true;
                bModificar.Enabled = true;
                txtCodigo.Enabled = false;

                if (bool.Parse(usuario.BAJA_AUTOMATICA.ToString()))
                {
                    lblBajaAutomatica.Text = "USUARIO DADO DE BAJA AUTOMÁTICAMENTE";
                    lblBajaAutomatica.Visible = true;
                }
                else
                {
                    lblBajaAutomatica.Text = "";
                    lblBajaAutomatica.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }

        }
        protected void bEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioDTO usuario = new UsuarioDTO();

                usuario.Login = txtCodigo.Text.Trim();
                usuario.Nombre = txtNombre.Text.Trim();
                usuario.Password = EncryptHelper.Encrypt(txtContrasenia.Text.Trim());
                usuario.Email = txtEmail.Text.Trim();
                usuario.Id_Perfil = Int64.Parse(txtIDPerfil.Text.Trim());
                usuario.Permiso = cbPermiso.Checked;

                Usuarios.EliminarUsuario(usuario, Usuarios.ObtenerUsuarioLogeado().Login);

                this.ShowMessage("Usuario Eliminado");
                LimpiarControles();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

        }
        protected void bModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidatePage())
                {
                    UsuarioDTO usuario = new UsuarioDTO();

                    usuario.Login = txtCodigo.Text.Trim();
                    usuario.Nombre = txtNombre.Text.Trim();
                    usuario.Password = EncryptHelper.Encrypt(txtContrasenia.Text.Trim());
                    usuario.Email = txtEmail.Text.Trim();
                    usuario.Id_Perfil = Int64.Parse(txtIDPerfil.Text.Trim());
                    usuario.Permiso = cbPermiso.Checked;
                    usuario.Activo = chkActivo.Checked.ToString();
                    usuario.Pais = cmbPais.SelectedValue;

                    Usuarios.ModificarUsuario(usuario, Usuarios.ObtenerUsuarioLogeado().Login);

                    this.ShowMessage("Usuario Modificado");
                    LimpiarControles();
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void LimpiarControles()
        {
            this.txtCodigo.Text ="";
            this.txtCodigo.Enabled =true;
            this.txtNombre.Text="";
            this.txtContrasenia.Text="";
            this.txtEmail.Text="";
            this.txtIDPerfil.Text="";
            this.cbPermiso.Checked =false;
            this.bAniadir.Enabled =true;
            this.bEliminar.Enabled =false;
            this.bModificar.Enabled =false;
            this.hdnUserID.Value ="";
            this.chkActivo.Checked = false;
            this.lblUsuarioBaja.Text = "";
            this.lblUsuarioBaja.Visible = false;
            this.lblBajaInfo.Visible = false;

            for (int i = 0; i < this.cboPerfiles.Items.Count; i++)
            {
                this.cboPerfiles.Items[i].Selected = false;
            }
            this.cboPerfiles.Items[0].Selected=true;
            this.txtIDPerfil.Text = this.cboPerfiles.SelectedValue;
        }

        protected void cboPerfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtIDPerfil.Text = this.cboPerfiles.SelectedValue;
        }

        protected void txtCodigo_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.TextBoxHasValue(txtCodigo,true,(BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtNombre_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.TextBoxHasValue(txtNombre, true, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtContrasenia_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.TextBoxHasValue(txtContrasenia, true, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
    }
}
