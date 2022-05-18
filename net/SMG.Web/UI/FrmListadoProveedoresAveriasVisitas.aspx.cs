using System;
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
    public partial class FrmListadoProveedoresAveriasVisitas : FrmBaseListado
    {
        #region implementación de los eventos


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            if (!Page.IsPostBack)
            {
                //UsuarioDB usuarioDB = new UsuarioDB();
                //this.cmbProveedor.DataSource = usuarioDB.ObtenerNombreTodosProveedoresEnSolicitudes();
                //cmbProveedor.DataTextField = "PROVEEDOR";
                //cmbProveedor.DataValueField = "PROVEEDOR";
                //this.cmbProveedor.DataBind();
                CargarCombos();
            }

        }

        protected void btnRealizarConsulta_Click(object sender, EventArgs e)
        {
            if (txtBuscarPoblacion.Text == "" || txtBuscarProvincia.Text == "" || txtCP.Text == "")
            {
                MostrarMensaje("Debe indicar el C.P., Provincia y Poblacion");
            }
            else
            {
                if (this.hdnPageIndex.Value == "") { this.hdnPageIndex.Value = "1"; }
                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                CargarDatos(intPageIndex);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.chkPoblaciones.Items.Clear();
            txtBuscarPoblacion.Text = "";
            txtBuscarProvincia.Text = "";
            txtCP.Text = "";
            this.chkProvincias.Items.Clear();
            CargarCombos();

            txtProveedor.Text = "";
            txtProveedorAveria.Text = "";
        }

        protected void OnGrdListado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    //e.Row.Attributes.Add("OnMouseOver", "Resaltar_On1(this);");
                //    //e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");

                //    for (int i = 0; i < e.Row.Cells.Count; i++)
                //    {
                //        String columnName = null;
                //        String columnValue = null;
                //        String strWidth = "80";
                //        //if (typeof(System.Web.UI.WebControls.BoundField) == grdListado.Columns[i].GetType())
                //        //{


                //        columnName = ((System.Data.DataRowView)(e.Row.DataItem)).Row.Table.Columns[i].ToString(); //((System.Web.UI.WebControls.BoundField)(grdListado.Columns[i])).DataField;
                //        columnValue = e.Row.Cells[i].Text.Trim();
                //        //ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarAverias", columnName);
                //        //strWidth = (columnDefinition.Width).ToString();

                //        e.Row.Cells[i].Attributes["style"] += "cursor:default;value: " + e.Row.Cells[i].Text;
                //        e.Row.Cells[i].ToolTip = formatString(e.Row.Cells[i].Text);

                //        e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + e.Row.Cells[i].Text.Trim() + "</div>";
                //        //}
                //    }
                //}

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private string formatString(string s)
        {
            s = s.Replace("&#225;", "á");
            s = s.Replace("&#233;", "é");
            s = s.Replace("&#237;", "í");
            s = s.Replace("&#243;", "ó");
            s = s.Replace("&#250;", "ú");

            return s;
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
                CargarDatos(intPageIndex);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void CargarDatos(int NumPagina)
        {
            try
            {
                //IDataReader datos;
                IDataReader drExcel;
                DataTable dtExcel = new DataTable();
                DataTable dt = new DataTable();
                String Poblacion =chkPoblaciones.SelectedValue;
                String Provincia = chkProvincias.SelectedValue;
                String CP = txtCP.Text;

                if (NumPagina == 1)
                {
                    drExcel = Consultas.ObtenerProveedoresAveriasVisitas(Provincia, Poblacion, CP);
                    dtExcel.Load(drExcel);
                    lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();
                }

                //datos = null;
                //dt = new DataTable();
                //datos = Consultas.ObtenerProveedoresAveriasVisitasPorPagina(Provincia, Poblacion, CP, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50);// Consultas.ObtenerSolicitudesBonificables(proveedor);
                //dt.Load(datos);

                String Proveedor = "";
                String ProveedorAveria = "";
                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    if (dtExcel.Rows[i].ItemArray[0].ToString() != "")
                    {
                        Proveedor = dtExcel.Rows[i].ItemArray[0].ToString();
                    }
                    if (dtExcel.Rows[i].ItemArray[1].ToString() != "")
                    {
                        ProveedorAveria = dtExcel.Rows[i].ItemArray[1].ToString();
                    }
                }
                if (Proveedor == "") { Proveedor = "SIEL"; }
                if (ProveedorAveria == "") { ProveedorAveria = Proveedor; }

                txtProveedor.Text = Proveedor;
                txtProveedorAveria.Text = ProveedorAveria;

                CurrentSession.SetAttribute(CurrentSession.SESSION_LIMITE_DATOS_EXCEL, "SI");
                ExcelHelper excel = new ExcelHelper("NO", "NO");
                if (dtExcel.Rows.Count >= 10000)
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_LIMITE_DATOS_EXCEL, "NO");
                }

                CurrentSession.SetAttribute(CurrentSession.SESSION_EXCEL_HELPER, excel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dtExcel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "LISTADO TECNICOS");


                Int32 numReg = int.Parse(this.lblNumEncontrados.Text.ToString());


                this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                this.hdnPageIndex.Value = "1";

                CargarPaginador(int.Parse(NumPagina.ToString()));

                //if (lblNumEncontrados.Text == "0") { MostrarMensaje("No se han encontrado datos"); }
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

        private void CargarCombos()
        {
            //carga de provincias
            //cmbProvincias.DataSource = TablasReferencia.ObtenerProvincias();
            //cmbProvincias.DataValueField = "CodProvincia";
            //cmbProvincias.DataTextField = "Nombre";
            //cmbProvincias.DataBind();
            //FormUtils.AddDefaultItem(cmbProvincias);
            chkProvincias.DataSource = TablasReferencia.ObtenerProvincias();
            chkProvincias.DataValueField = "CodProvincia";
            chkProvincias.DataTextField = "Nombre";
            chkProvincias.DataBind();
            hdnContadorProvincia.Value = chkProvincias.Items.Count.ToString();
            chkProvincias.Items[0].Enabled = false;
        }

        private void CargarComboPoblaciones(String CodProvincia)
        {
            ProvinciaDTO prov = new ProvinciaDTO();
            prov.CodProvincia = CodProvincia;
            chkPoblaciones.DataSource = TablasReferencia.ObtenerPoblacionesProvincia(prov);
            chkPoblaciones.DataValueField = "CodPoblacion";
            chkPoblaciones.DataTextField = "Nombre";
            chkPoblaciones.DataBind();
            hdnContadorPoblacion.Value = chkPoblaciones.Items.Count.ToString();
        }

        /// <summary>
        /// Evento de cambio de provincia.
        /// Cuando se selecciona una provincia se carga el combo de poblaciones
        /// asociadas a esa provincia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbProvincias_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int seleccionada = chkProvincias.SelectedIndex;
                //String Valor = chkProvincias.SelectedValue;
                this.txtBuscarProvincia.Text = chkProvincias.Items[chkProvincias.SelectedIndex].Text;
                
                //for (int i = 0; i <= this.chkProvincias.Items.Count - 1; i++)
                //{
                //    chkProvincias.Items[i].Selected = false;
                //}

                chkPoblaciones.Items.Clear();

                CargarComboPoblaciones(chkProvincias.SelectedValue);
                //UpdatePanelPoblaciones.Update();
                ////chkProvincias.Items[seleccionada].Selected = true;
                
                string script = "<script type='text/javascript'>PosicionarProvincia();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "POSICIONAR", script, false);
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
        protected void chkPoblaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int seleccionada = chkPoblaciones.SelectedIndex;
            //String Valor = chkPoblaciones.SelectedValue;
            txtBuscarPoblacion.Text = chkPoblaciones.Items[chkPoblaciones.SelectedIndex].Text;
            //for (int i = 0; i <= this.chkPoblaciones.Items.Count - 1; i++)
            //{
            //    chkPoblaciones.Items[i].Selected = false;
            //}
            //chkPoblaciones.Items[seleccionada].Selected = true;
        }
}
}