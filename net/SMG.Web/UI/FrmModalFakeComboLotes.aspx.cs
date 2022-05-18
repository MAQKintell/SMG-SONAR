using System;
using System.Web.UI;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Exceptions;
using Iberdrola.SMG.BLL;
using System.Data;
using System.Web.UI.WebControls;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalFakeComboLotes : FrmBase
    {
        private IDataReader CargarListado(string descLote)
        {
            LoteDTO lote = new LoteDTO();
            lote.Descripcion = descLote;
            IDataReader dr = Lotes.ObtenerLotesDescripcion(lote);
            return dr;
        }
        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["DESC_LOTE"] != null)
                    {
                        txtDescLote.Text = Request.QueryString["DESC_LOTE"];
                    }

                    grdLotes.DataSource = CargarListado(txtDescLote.Text);
                    grdLotes.DataBind();
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        
        protected void OnBtnBuscarLotes_Click(object sender, EventArgs e)
        {
            try
            {
                grdLotes.DataSource = CargarListado(txtDescLote.Text);
                grdLotes.DataBind();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }


        protected void OnRowSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().FullName.ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                {
                    LinkButton lbtn = (LinkButton)sender;
                    MasterPageBase mp = (MasterPageBase)this.Master;

                    LoteDTO lote = new LoteDTO();
                    lote.Id = Decimal.Parse(lbtn.Text);

                    String idLote = "";
                    String descLote = "";
                    if (lote.Id.HasValue)
                    {
                        if (Lotes.BuscarLoteID(lote))
                        {
                            idLote = lote.Id.Value.ToString();
                            descLote = lote.Descripcion;
                        }
                    }
                    mp.PlaceHolderScript.Controls.Add(new LiteralControl("<script>SeleccionarLote('" + idLote + "','" + descLote + "')</script>"));
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
