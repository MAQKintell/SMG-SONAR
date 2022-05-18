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


namespace Iberdrola.SMG.UI
{
    public partial class FrmModalCodificacionAverias : FrmBase
    {
        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    lblTitulo.Text = "CODIFICACION AVERIAS";
                    // Estados de la visita
                    this.lstCodigos.DataSource = TablasReferencia.ObtenerCodigosAveriaGasConfort();
                    this.lstCodigos.DataValueField = "Codigo";
                    this.lstCodigos.DataTextField = "Descripcion";
                    this.lstCodigos.DataBind();
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
