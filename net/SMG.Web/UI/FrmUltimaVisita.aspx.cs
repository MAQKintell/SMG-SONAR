using System;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.Commons.Exceptions;
using System.Web.UI.WebControls;

namespace Iberdrola.SMG.UI
{
    public partial class FrmUltimaVisita : FrmBase
    {
        #region metodos privados
        #endregion

        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            this.txtCodContrato.Focus();  
            try
            {
                if (!Page.IsPostBack)
                {
                    if (NavigationController.IsBackward())
                    {
                        this.LoadSessionData();
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnBtbAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidatePage())
                {
                    String strCodContrato = this.txtCodContrato.Text;
                    Int16? intCodVisita = Contrato.ObtenerCodigoUltimaVisita(strCodContrato);
                    if (!intCodVisita.HasValue)
                    {
                        //this.ShowMessage("No se ha encontrado la visita del contrato. Compruebe el código introducido");
                        ShowMessage(Resources.TextosJavaScript.TEXTO_ERROR_VISITA_NO_ENCONTRADA);
                    }
                    else
                    {
                        this.SaveSessionData();
                        NavigationController.Forward("./FrmDetalleVisita.aspx?idContrato=" + strCodContrato + "&idVisita=" + intCodVisita.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnBtbVolver_Click(object sender, EventArgs e)
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

        protected void onTxtCodContrato_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                    e.IsValid = FormValidation.ValidateNumberTextBox(txtCodContrato, true, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        #endregion

        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
                String strCodContrato = (String)CurrentSession.GetAttribute("FRM_ULTIMA_VISITA_ID_CONTRATO");
                // cargar los valores en el formulario
                if (strCodContrato != null)
                {
                    this.txtCodContrato.Text = strCodContrato;
                }
                else
                {
                    this.txtCodContrato.Text = "";
                }
                // eliminar de sesión los valores del formulario
        }

        public override void DeleteSessionData()
        {
            CurrentSession.RemoveAttribute("FRM_ULTIMA_VISITA_ID_CONTRATO");
        }

        public override void SaveSessionData()
        {
            // eliminar de sesión los valores del formulario si existían

            // coger los valores en el formulario
            CurrentSession.SetAttribute("FRM_ULTIMA_VISITA_ID_CONTRATO", this.txtCodContrato.Text);
            // cargar los valores en sessión
        }
        #endregion
    }
}
