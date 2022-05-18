using System;
using System.Web.UI;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Exceptions;
using System.Web.UI.WebControls;
using System.Data;
using Iberdrola.SMG.BLL;
using Iberdrola.Commons.Security;


namespace Iberdrola.SMG.UI
{
    public partial class FrmModalCoberturaServicio : FrmBase
    {
        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    hdnCodEFV.Value = Request.QueryString["codEFV"];
                    SolicitudesDB solDB = new SolicitudesDB();
                    AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    UsuarioDTO usuarioDTO = null;
                    usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                    DataSet coberturaServicio = solDB.GetCoberturaServicio(hdnCodEFV.Value,usuarioDTO.IdIdioma);
                    if (coberturaServicio.Tables[0].Rows.Count > 0)
                    {
                        //TP.TITULO
                        lblTitulo.Text = coberturaServicio.Tables[0].Rows[0].ItemArray[0].ToString();
                        //TP.TEXTO1
                        if (coberturaServicio.Tables[0].Rows[0].ItemArray[1].ToString() != "") { lblTexto1.Text = "º " + coberturaServicio.Tables[0].Rows[0].ItemArray[1].ToString(); }
                        //TP.TEXTO2
                        if (coberturaServicio.Tables[0].Rows[0].ItemArray[2].ToString() != "") { lblTexto2.Text = "º " + coberturaServicio.Tables[0].Rows[0].ItemArray[2].ToString(); }
                        //TP.TEXTO3
                        if (coberturaServicio.Tables[0].Rows[0].ItemArray[3].ToString() != "") { lblTexto3.Text = "º " + coberturaServicio.Tables[0].Rows[0].ItemArray[3].ToString(); }
                        //TP.TEXTO4
                        if (coberturaServicio.Tables[0].Rows[0].ItemArray[4].ToString() != "") { lblTexto4.Text = "º " + coberturaServicio.Tables[0].Rows[0].ItemArray[4].ToString(); }
                    }
                }
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
