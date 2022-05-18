﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Web;
using System.Collections;
using System.Data;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using System.Threading;
using Iberdrola.Commons.Reporting;
using System.Globalization;
using System.Web.Configuration;
using Iberdrola.Commons.Configuration;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using System.Drawing;
using Iberdrola.Commons.DataAccess;



namespace Iberdrola.SMG.UI
{
    public partial class FrmCambioContraseniaSinMenu : FrmBaseListado
    {
        #region implementación de los eventos


        protected void Deshabilitar()
        {
            foreach (Control c in Page.Controls)
            {
                RecControles(c, Page);
            }
        }
        public void RecControles(Control control, Page Pagina)
        {
            foreach (Control contHijo in control.Controls)
            {
                if (contHijo.HasControls()) { RecControles(contHijo, Pagina); }
                //Proceso(contHijo, Pagina);
                if (contHijo is LinkButton)
                {
                    LinkButton a = (LinkButton)contHijo;
                    if (a.Text != "desconectar")
                    {
                        a.Enabled = false;
                    }
                }
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //AsignarCultura Cultur = new AsignarCultura();
                //Cultur.Asignar(Page);
                string mostrarCaducidad = Request.QueryString["MostrarCaducidad"];

                string mostrarAlerta = Request.QueryString["MostrarAlerta"];
                if (mostrarAlerta != null || mostrarCaducidad != null)
                {
                    Deshabilitar();
                }

            }

            if (!Page.IsPostBack)
            {
                txtContrasenia.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnCambiar.UniqueID + "').click();return false;}} else {return true}; ");
                txtRepetir.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnCambiar.UniqueID + "').click();return false;}} else {return true}; ");

                string mostrarAlerta = Request.QueryString["MostrarAlerta"];
                if (mostrarAlerta != null)
                {
                    this.placeHolderAlerta.Visible = true;
                }
                else
                {
                    string mostrarCaducidad = Request.QueryString["MostrarCaducidad"];
                    if (mostrarCaducidad != null)
                    {
                        this.placeHolderCaducidad.Visible = true;
                    }
                }
            }
        }

        protected void OnBtnCambiar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidatePage())
                {
                    if (this.txtContrasenia.Text.ToString() == this.txtRepetir.Text.ToString())
                    {
                        UsuarioDTO DatosUsuario = Usuarios.ObtenerUsuarioLogeado();


                        String Login = CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_USUARIO_VALIDO).ToString();
                        ////usuarioPrincipal.

                        //                        Usuarios usuarioDb = new UsuariosDB();

                        ConfiguracionDTO configPasswords = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.PASSWORDS_ENCRIPTADAS);
                        if (configPasswords != null && configPasswords.Valor != null && bool.Parse(configPasswords.Valor))
                        {
                            Usuarios.CambiarPassword(DatosUsuario.Login.ToString(), EncryptHelper.Encrypt(this.txtContrasenia.Text.ToString()), Usuarios.ObtenerUsuarioLogeado().Login);
                        }
                        else
                        {
                            Usuarios.CambiarPassword(DatosUsuario.Login.ToString(), this.txtContrasenia.Text.ToString(), Usuarios.ObtenerUsuarioLogeado().Login);
                        }

                        lblContratoBuscar.Enabled = false;
                        Label1.Enabled = false;
                        txtContrasenia.Enabled = false;
                        txtRepetir.Enabled = false;
                        btnCambiar.Enabled = false;
                        //lblSolicitudModificada.Visible = true;
                        this.txtContraseniaCustomValidator.Enabled = false;
                        this.txtRepetirCustomValidator.Enabled = false;
                        //this.ShowMessage("La contraseña ha sido modificada correctamente.");
                        this.ShowMessage(Resources.TextosJavaScript.TEXTO_CONTRASENHIA_MODIFICADA_CORRECTAMENTE);

                        AppPrincipal usuarioPrincipal = (AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                        usuarioPrincipal.IsSecurePassword = true;
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_USUARIO, usuarioPrincipal);

                        //this.ShowMessage("La contraseña ha sido modificada correctamente.");
                        this.ShowMessage(Resources.TextosJavaScript.TEXTO_CONTRASENHIA_MODIFICADA_CORRECTAMENTE);

                        String Script = "<Script>abrirVentana();</Script>";//CerrarVentanaModal()
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ARBOL", Script, false);
                    }
                    else
                    {
                        //this.ShowMessage("NO COINCIDEN los valores");
                        this.ShowMessage(Resources.TextosJavaScript.TEXTO_NO_COINCIDEN_LOS_VALORES);
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnBtnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                this.DeleteSessionData();
                NavigationController.Backward();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        #endregion


        protected void OnTxtContrasenia_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (txtContrasenia.Text.Length == 0)
                {
                    e.IsValid = false;
                    txtContraseniaCustomValidator.ToolTip = "La contraseña no puede estar vacía.";
                }
                else if (txtContrasenia.Text.Length < PasswordUtils.PASSWORD_LENGHT)
                {
                    e.IsValid = false;
                    txtContraseniaCustomValidator.ToolTip = "La contraseña tiene que tener al menos " + PasswordUtils.PASSWORD_LENGHT + " caracteres.";
                }
                else if (Usuarios.ObtenerUsuarioLogeado().Password.Equals(txtContrasenia.Text))
                {
                    e.IsValid = false;
                    txtContraseniaCustomValidator.ToolTip = "La nueva contraseña no puede ser la misma que la actual.";
                }
                else if (!PasswordUtils.ValidatePassword(txtContrasenia.Text, PasswordUtils.PASSWORD_NUMERIC_CHAR_COUNT))
                {
                    e.IsValid = false;
                    txtContraseniaCustomValidator.ToolTip = "La contraseña tiene que tener al menos " + PasswordUtils.PASSWORD_NUMERIC_CHAR_COUNT + " caracteres numéricos.";
                }
                else if (!PasswordUtils.ValidatePassword(txtContrasenia.Text, PasswordUtils.PASSWORD_UPPER_CASE_CHAR_COUNT, PasswordUtils.PASSWORD_LOWER_CASE_CHAR_COUNT))
                {
                    e.IsValid = false;
                    txtContraseniaCustomValidator.ToolTip = "La contraseña tiene que tener al menos " + PasswordUtils.PASSWORD_UPPER_CASE_CHAR_COUNT + " caracteres en mayúsculas y " + PasswordUtils.PASSWORD_LOWER_CASE_CHAR_COUNT + " en minúsculas.";
                }
                else if (!PasswordUtils.ValidatePasswordAlphanumeric(txtContrasenia.Text, PasswordUtils.PASSWORD_MAX_NON_ALPHANUMERIC_CHARS))
                {
                    e.IsValid = false;
                    txtContraseniaCustomValidator.ToolTip = "La contraseña tiene " + PasswordUtils.PASSWORD_MAX_NON_ALPHANUMERIC_CHARS + " caracteres no alphanumericos como máximo.";
                }

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtRepetir_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (txtRepetir.Text.Length == 0)
                {
                    e.IsValid = false;
                    txtRepetirCustomValidator.ToolTip = "La confirmación de la contraseña no puede estar vacía.";
                }
                if (txtRepetir.Text.Length < 6)
                {
                    e.IsValid = false;
                    txtRepetirCustomValidator.ToolTip = "La contraseña debe de tener por lo menos 6 caracteres.";
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

        }
        public override void SaveSessionData()
        {

        }
        public override void DeleteSessionData()
        {

        }
        #endregion
    }
}