using System;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using System.Data;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Configuration;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Security;

namespace Iberdrola.SMG.UI
{
    public partial class FrmDetalleContrato : FrmBase
    {
        public Int32 ContVisitas;

        #region constructores
        #endregion

        #region métodos privados
        private IDataReader CargarListado(string C)
        {
            UsuarioDTO usuarioDTO = null;
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            IDataReader dr = Contrato.ObtenerVisitas(C,(Int16)usuarioDTO.IdIdioma);
            return dr;
        }
        #endregion

        #region implementacion de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
        //    if (!IsPostBack)
        //    {
        //    AsignarCultura Cultur = new AsignarCultura();
        //    Cultur.Asignar(Page);
        //    }

            try
            {
                if (!IsPostBack)
                {
                    if (NavigationController.IsBackward())
                    {
                        this.LoadSessionData();
                    }
                    else
                    {
                        if (Request.QueryString["COD_CONTRATO"] != null)
                        {
                            lblIdContrato.Text = Request.QueryString["COD_CONTRATO"];
                        }
                    }
                    if (lblIdContrato.Text != null && lblIdContrato.Text.Length > 0)
                    {
                        dgVisitas.DataSource = CargarListado(lblIdContrato.Text);
                        dgVisitas.DataBind();
                        lbContador.Text = lbContador.Text.Replace("x", dgVisitas.Rows.Count.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnGrdListado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ContVisitas += 1;
                    //if (e.Row.DataItemIndex == 0)
                    //{
                        e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                        e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");

                        e.Row.Attributes["OnClick"] =
                            Page.ClientScript.GetPostBackClientHyperlink(this.dgVisitas, "Select$" + e.Row.RowIndex.ToString());

                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {

                            //    e.Row.Cells(i).ToolTip = 
                            //        Tildes(DirectCast(DirectCast(DirectCast(e.Row.Cells[15], 
                            //                        System.Web.UI.WebControls.TableCell).Controls[1],
                            //                        System.Web.UI.Control), 
                            //               System.Web.UI.WebControls.Label).ToolTip);

                            //e.Row.Cells[i].Attributes["style"] += "cursor:hand;";

                            String columnName = null;
                            String strWidth = "80";
                            if (typeof(System.Web.UI.WebControls.BoundField) == this.dgVisitas.Columns[i].GetType())
                            {
                                columnName = ((System.Web.UI.WebControls.BoundField)(dgVisitas.Columns[i])).DataField;
                                ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla(dgVisitas.ID, columnName);
                                strWidth = (columnDefinition.Width).ToString();

                                e.Row.Cells[i].Attributes["style"] += "cursor:pointer;";
                                e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                                e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + e.Row.Cells[i].Text.Trim() + "</div>";
                                //e.Row.Cells[i].Text = "<div style='width:100px;white-space: nowrap; overflow:hidden;'>" + e.Row.Cells[i].Text.Trim() + "</div>";

                            }
                        }
                    //}
                    ////else
                    ////{
                    ////    for (int i = 0; i < e.Row.Cells.Count; i++)
                    ////    {
                    ////        if (e.Row.Cells[2].Text != "Cerrada - Pendiente realizar reparación")
                    ////        {
                    ////            e.Row.Cells[0].Enabled = false;
                    ////        }
                    ////    }
                    ////}
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
            finally 
            {
                hdnNumeroVisitas.Value = ContVisitas.ToString();
            }
        }
        protected void OnRowSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().FullName.ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                {
                    LinkButton lbtn = (LinkButton)sender;
                    this.SaveSessionData();
                    NavigationController.Forward("./FrmDetalleVisita.aspx?idContrato=" + this.lblIdContrato.Text + "&idVisita=" + lbtn.Text + "&TotalVisitas=" + hdnNumeroVisitas.Value);
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
        #endregion

        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            // coger los valores de sessión
            String strCodContrato = (String)CurrentSession.GetAttribute("FRM_CONTRATO_ID_CONTRATO");
            // cargar los valores en el formulario
            if (strCodContrato != null)
            {
                this.lblIdContrato.Text = strCodContrato;
            }
            else
            {
                this.lblIdContrato.Text = "";
            }

            // eliminar de sesión los valores del formulario
        }

        public override void SaveSessionData()
        {
            // eliminar de sesión los valores del formulario si existían

            // coger los valores en el formulario
            CurrentSession.SetAttribute("FRM_CONTRATO_ID_CONTRATO", this.lblIdContrato.Text);
        }
        public override void DeleteSessionData()
        {
           
        }
        #endregion
        protected void BtnBuscar_OnClick(object sender, EventArgs e)
        {
            dgVisitas.DataSource = CargarListado(txtCodigoContrato.Text);
            dgVisitas.DataBind();
            lblIdContrato.Text = txtCodigoContrato.Text;
            lbContador.Text = lbContador.Text.Replace("x", dgVisitas.Rows.Count.ToString());
        }
}
}