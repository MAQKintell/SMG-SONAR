using System;
using System.Data;
using System.Web.UI;
using Iberdrola.Commons.Configuration;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.BLL;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Security;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalHistoricoVisita : FrmBase
    {
        private string _filtro;

        /// <summary>
        /// Evento de inicialización de la página.
        /// Carga los datos de la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            bool resultadoOK = true;
            try
            {
                String Contrato= Request.QueryString["COD_CONTRATO"];
                String Visita = Request.QueryString["COD_VISITA"];

                Visitas visitas = new Visitas();
                grdDatos.DataSource = visitas.ObtenerHistoricoVisitaPorIdVisita(Contrato,Visita,(Int16)usuarioDTO.IdIdioma);
                grdDatos.DataBind();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

            if (!resultadoOK)
            {
                this.CerrarVentanaModal();
            }
        }

        /// <summary>
        /// Trata de cerrar la ventana modal
        /// </summary>
        protected void CerrarVentanaModal()
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


        /// <summary>
        /// Evento del botón cancelar de la pantalla.
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBtnCancelar_Click(object sender, EventArgs e)
        {
            this.CerrarVentanaModal();
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

    }
}
