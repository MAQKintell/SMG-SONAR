using System;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using System.Data;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Configuration;
using Iberdrola.Commons.Security;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.UI
{
    public partial class FrmHistoricoVisita : FrmBase
    {
        public Int32 ContVisitas;

        #region constructores
        #endregion

        #region implementacion de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
           
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
        #endregion

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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (this.ValidatePage())
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                Visitas visitas = new Visitas();
                dgVisitas.DataSource = visitas.ObtenerHistoricoVisita(txtContrato.Text, (Int16) usuarioDTO.IdIdioma);
                dgVisitas.DataBind();
            }
        }
        protected void txtContratoCustomValidator_ServerValidate(object source, ServerValidateEventArgs e)
        {
            try
            {
                if (txtContrato.Text.Length == 0)
                {
                    e.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
    }
}