using Iberdrola.Commons.Configuration;
using System.Drawing;

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
    public partial class FrmVisitasModificadasPorCambioTelefono : FrmBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            if (!IsPostBack)
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                if (usuarioDTO.NombreProveedor != "ADICO")
                {
                    this.PlaceHolder5.Visible = false;
                }
                else
                {
                    UsuarioDB usuarioDB = new UsuarioDB();
                    this.cmbProveedor.DataSource = usuarioDB.ObtenerTodosProveedoresEnSolicitudes();
                    cmbProveedor.DataTextField = "PROVEEDOR";
                    cmbProveedor.DataValueField = "PROVEEDOR";
                    this.cmbProveedor.DataBind();
                }
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

        private string formatString(string s)
        {
            s = s.Replace("&#225;", "á");
            s = s.Replace("&#233;", "é");
            s = s.Replace("&#237;", "í");
            s = s.Replace("&#243;", "ó");
            s = s.Replace("&#250;", "ú");

            return s;
        }

        protected void OnGrdListado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //e.Row.Attributes.Add("OnMouseOver", "Resaltar_On1(this);");
                    //e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");

                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        String columnName = null;
                        String columnValue = null;
                        String strWidth = "80";
                        //if (typeof(System.Web.UI.WebControls.BoundField) == grdListado.Columns[i].GetType())
                        //{


                        columnName = ((System.Data.DataRowView)(e.Row.DataItem)).Row.Table.Columns[i].ToString(); //((System.Web.UI.WebControls.BoundField)(grdListado.Columns[i])).DataField;
                        columnValue = e.Row.Cells[i].Text.Trim();
                        //ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarAverias", columnName);
                        strWidth = "150";// (columnDefinition.Width).ToString();

                        e.Row.Cells[i].Attributes["style"] += "cursor:default;value: " + e.Row.Cells[i].Text;
                        e.Row.Cells[i].ToolTip = formatString(e.Row.Cells[i].Text);

                        e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + e.Row.Cells[i].Text.Trim() + "</div>";
                        //}
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

        public Int32 ObtenerNumPaginas(Int32 numRegs)
        {
            if (numRegs > 0)
            {
                Int32 resto;
                Int32 resultado = Math.DivRem(numRegs, Int32.Parse(Resources.SMGConfiguration.GridViewPageSize), out resto);
                if (resto > 0)
                {
                    resultado++;
                }
                return resultado;
            }
            else
            {
                return 0;
            }
        }

        private void CargarPaginador(Int32 NumPagina)
        {
            Int32 intNumPaginas = Int32.Parse(this.hdnPageCount.Value);
            Int32 intPaginaActual = NumPagina;
            int numPaginasMostradas = 10;
            int rangoActual = intPaginaActual / numPaginasMostradas;
            int limiteSuperior = (rangoActual + 1) * numPaginasMostradas;

            if (limiteSuperior > intNumPaginas)
            {
                limiteSuperior = intNumPaginas;
            }

            int limiteInferior = (rangoActual * numPaginasMostradas) + 1;

            if (limiteInferior == intPaginaActual && intPaginaActual > 1)
            {
                limiteInferior -= numPaginasMostradas;
                if (limiteInferior < 0)
                {
                    limiteInferior = 1;
                }
                limiteSuperior -= numPaginasMostradas;
            }

            this.placeHolderPaginacion.Controls.Clear();

            if ((limiteInferior) > numPaginasMostradas)
            {
                Button lb = new Button();
                lb.ID = "hlPaginacion1";
                lb.CommandName = "hlPaginacion1";
                lb.CommandArgument = 1.ToString();
                lb.OnClientClick = "return ClickPaginacion('1')";
                lb.CssClass = "linkPaginacion";
                lb.Text = ("<<").ToString();
                this.placeHolderPaginacion.Controls.Add(lb);

            }

            for (int i = limiteInferior; i <= limiteSuperior; i++)
            {
                if (i == intPaginaActual)
                {
                    Label lb = new Label();
                    lb.ID = "hlPaginacion" + i;
                    lb.CssClass = "linkPaginacionDeshabilitado";
                    lb.Text = (i).ToString();
                    this.placeHolderPaginacion.Controls.Add(lb);
                }
                else
                {
                    Button lb = new Button();
                    lb.ID = "hlPaginacion" + i;
                    lb.CommandName = "hlPaginacion" + i;
                    lb.CommandArgument = i.ToString();
                    lb.OnClientClick = "return ClickPaginacion('" + i + "')";
                    lb.CssClass = "linkPaginacion";
                    lb.Text = (i).ToString();
                    this.placeHolderPaginacion.Controls.Add(lb);
                }

            }

            if (limiteSuperior < intNumPaginas - 10)
            {
                Button lb = new Button();
                lb.ID = "hlPaginacion" + intNumPaginas;
                lb.CommandName = "hlPaginacion" + intNumPaginas;
                lb.CommandArgument = intNumPaginas.ToString();
                lb.OnClientClick = "return ClickPaginacion('" + intNumPaginas + "')";
                lb.CssClass = "linkPaginacion";
                lb.Text = (">>").ToString();
                this.placeHolderPaginacion.Controls.Add(lb);
            }

        }

        protected void OnBtnPaginacion_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                CargarDatosBusqueda(intPageIndex); 
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        
        protected void btnRealizarConsulta_Click(object sender, EventArgs e)
        {
            if (this.hdnPageIndex.Value == "") { this.hdnPageIndex.Value = "1"; }
            Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
            CargarDatosBusqueda(intPageIndex);
        }
        private static void FormatearColumnaDataGrid(DataColumn dc, ref BoundField bc)
        {
            bc.ItemStyle.Wrap = false;
            bc.HeaderStyle.Wrap = false;
            switch (dc.DataType.ToString())
            {
                case "System.DateTime":
                    bc.DataFormatString = "{0:d}";
                    break;
            }
        }
        protected void CargarDatos(Int64 NumPagina)
        {
            try
            {
                IDataReader datos;
                IDataReader drExcel;
                DataTable dtExcel = new DataTable();
                DataTable dt = new DataTable();
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                String proveedor = usuarioDTO.NombreProveedor;
                if (proveedor == "ADICO")
                {
                    proveedor = this.cmbProveedor.SelectedValue;
                    if (proveedor == "-1") { proveedor = "ADI"; }
                }
                
                drExcel = Consultas.ObtenerSolicitudesBonificables(proveedor);
                dtExcel.Load(drExcel);
                lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();
                
                datos = null;
                dt = new DataTable();
                datos =Consultas.ObtenerSolicitudesBonificablesPorPagina(proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50);// Consultas.ObtenerSolicitudesBonificables(proveedor);
                dt.Load(datos);

                grdListado.DataSource = dt;
                grdListado.DataBind();
                //lblNumEncontrados.Text = dt.Rows.Count.ToString();

                CurrentSession.SetAttribute(CurrentSession.SESSION_LIMITE_DATOS_EXCEL, "SI");
                ExcelHelper excel = new ExcelHelper("NO", "NO");
                if (dtExcel.Rows.Count >= 10000)
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_LIMITE_DATOS_EXCEL, "NO");
                }

                CurrentSession.SetAttribute(CurrentSession.SESSION_EXCEL_HELPER, excel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dtExcel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "VISITAS MODIFICADAS");


                Int32 numReg = int.Parse(this.lblNumEncontrados.Text.ToString());


                this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                this.hdnPageIndex.Value = "1";

                CargarPaginador(int.Parse(NumPagina.ToString()));

                if (lblNumEncontrados.Text == "0") { MostrarMensaje("No se han encontrado datos"); }

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", alerta, false);
        }

        protected void rdbBusquedas_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRealizarConsulta.Enabled = false;

            String Script = "<script>document.getElementById('ctl00$ContentPlaceHolderContenido$btnRealizarConsulta').disabled='true';</script>";
            Script += "<script>document.getElementById('ctl00$ContentPlaceHolderContenido$btnRealizarBusqueda').removeAttribute('disabled');</script>";
            Script += "<script>document.getElementById('ctl00_ContentPlaceHolderContenido_btnPagar').disabled='true';</script>";

            Page.RegisterStartupScript("INI", Script);
        }

        protected void CargarDatosBusqueda(Int64 NumPagina)
        {
            try
            {
                IDataReader datos;
                IDataReader drExcel;
                DataTable dtExcel = new DataTable();
                DataTable dt = new DataTable();
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                String proveedor = usuarioDTO.NombreProveedor;
                if (proveedor == "ADICO")
                {
                    proveedor = this.cmbProveedor.SelectedValue;
                    if (proveedor == "-1") { proveedor = "ADI"; }
                }

                drExcel = Consultas.ObtenerVisitasModificadasPorcambioTelefono(proveedor);
                dtExcel.Load(drExcel);
                lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();

                datos = null;
                dt = new DataTable();
                datos = Consultas.ObtenerVisitasModificadasPorcambioTelefonoPorPagina(proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50);// Consultas.ObtenerSolicitudesBonificables(proveedor);
                dt.Load(datos);


                grdListado.DataSource = dt;
                grdListado.DataBind();
                //lblNumEncontrados.Text = dt.Rows.Count.ToString();

                CurrentSession.SetAttribute(CurrentSession.SESSION_LIMITE_DATOS_EXCEL, "SI");
                ExcelHelper excel = new ExcelHelper("NO", "NO");
                if (dtExcel.Rows.Count >= 10000)
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_LIMITE_DATOS_EXCEL, "NO");
                }

                CurrentSession.SetAttribute(CurrentSession.SESSION_EXCEL_HELPER, excel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dtExcel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "VISITAS CAMBIADAS");


                Int32 numReg = int.Parse(this.lblNumEncontrados.Text.ToString());


                this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                this.hdnPageIndex.Value = "1";

                CargarPaginador(int.Parse(NumPagina.ToString()));

                if (lblNumEncontrados.Text == "0") { MostrarMensaje("No se han encontrado datos"); }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

    }
}
