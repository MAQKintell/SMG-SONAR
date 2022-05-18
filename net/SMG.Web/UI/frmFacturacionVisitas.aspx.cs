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
    public partial class frmFacturacionVisitas : FrmBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //AsignarCultura Cultur = new AsignarCultura();
            //Cultur.Asignar(Page);
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
                    this.cmbProveedor.DataSource = usuarioDB.ObtenerProveedores();
                    cmbProveedor.DataTextField = "PROVEEDOR";
                    cmbProveedor.DataValueField  = "PROVEEDOR";
                    this.cmbProveedor.DataBind();
                }

                ExcelHelper excel = new ExcelHelper("NO", "NO");
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL, "");
                CurrentSession.SetAttribute(CurrentSession.SESSION_EXCEL_HELPER, excel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, "");

                PlaceHolder3.Visible = false;
                PlaceHolder4.Visible = false;
                String Login = CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_USUARIO_VALIDO).ToString();
                IDataReader datos = Consultas.ObtenerPermisoFacturacionPorUsuario(Login);
                while (datos.Read())
                {
                    Int64 Permiso = Int64.Parse(DataBaseUtils.GetDataReaderColumnValue(datos, "TIPO_PERMISO_FACTURACION_VISITAS").ToString());
                    switch (Permiso)
                    {
                        case (Int64)Enumerados.PermisosFacturacionVisitas.Penalizacion:
                            if (this.RadioConsultas.SelectedIndex == 2)
                            {
                                PlaceHolder3.Visible = true;
                                PlaceHolder4.Visible = false;
                                return;
                            }
                            else
                            {
                                PlaceHolder3.Visible = false;
                            }

                            break;
                        case (Int64)Enumerados.PermisosFacturacionVisitas.Reparacion:
                            if (this.RadioConsultas.SelectedIndex == 1)
                            {
                                PlaceHolder4.Visible = true;
                                PlaceHolder3.Visible = false;
                                return;
                            }
                            else
                            {
                                PlaceHolder4.Visible = false;
                            }
                            break;
                        case (Int64)Enumerados.PermisosFacturacionVisitas.Visitas:
                            if (this.RadioConsultas.SelectedIndex == 0)
                            {
                                PlaceHolder4.Visible = true;
                                PlaceHolder3.Visible = false;
                                return;
                            }
                            else
                            {
                                PlaceHolder4.Visible = false;
                            }
                            break;
                        //default:
                        //    PlaceHolder3.Visible = false;
                        //    PlaceHolder4.Visible = false;
                        //    break;

                    }
                }
            }
        }

        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            // coger los valores de sessión
            Int32? radioSelectedIndex = (Int32?)CurrentSession.GetAttribute("FRM_CONSULTAS_RADIO_VISITAS_FACTURAR");
            // cargar los valores en el formulario
            if (radioSelectedIndex.HasValue)
            {
                this.RadioConsultas.SelectedIndex = radioSelectedIndex.Value;
            }

            // eliminar de sesión los valores del formulario
            CurrentSession.RemoveAttribute("FRM_CONSULTAS_RADIO_VISITAS_FACTURAR");
        }

        public override void SaveSessionData()
        {
            // cargar los valores en sessión
            CurrentSession.SetAttribute("FRM_CONSULTAS_RADIO_VISITAS_FACTURAR", this.RadioConsultas.SelectedIndex);
        }

        public override void DeleteSessionData()
        {
            CurrentSession.RemoveAttribute("FRM_CONSULTAS_RADIO_VISITAS_FACTURAR");
        }
        #endregion
        protected void RadioConsultas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //MostrarOcultarControlesOpciones();
                //PlaceHolder5.Visible = false;
                //String Script = "<script language='javascript'>var sURL = unescape(window.location.pathname);window.location.href = sURL;</script>";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "RECARGAR", Script, false);
                InicializarGrid();

                switch (this.RadioConsultas.SelectedIndex)
                {
                    case 0:
                        this.PlaceHolder1.Visible = true;
                        this.PlaceHolder2.Visible = false;
                        PlaceHolder3.Visible = false;
                        PlaceHolder4.Visible = true;
                        break;
                    case 1:
                        this.PlaceHolder1.Visible = false;
                        this.PlaceHolder2.Visible = true;
                        PlaceHolder3.Visible = false;
                        PlaceHolder4.Visible = true;
                        break;
                    case 2:
                        PlaceHolder3.Visible = true;
                        this.PlaceHolder1.Visible = false;
                        this.PlaceHolder2.Visible = false;
                        PlaceHolder4.Visible = false;
                        break;
                    default:
                        this.PlaceHolder1.Visible = false;
                        this.PlaceHolder2.Visible = false;
                        PlaceHolder3.Visible = false;
                        PlaceHolder4.Visible = false;
                        break;
                }


                PlaceHolder3.Visible = false;
                PlaceHolder4.Visible = false;
                String Login = CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_USUARIO_VALIDO).ToString();
                IDataReader datos = Consultas.ObtenerPermisoFacturacionPorUsuario(Login);
                while (datos.Read())
                {
                    Int64 Permiso = Int64.Parse(DataBaseUtils.GetDataReaderColumnValue(datos, "TIPO_PERMISO_FACTURACION_VISITAS").ToString());
                    switch (Permiso)
                    {
                        case (Int64)Enumerados.PermisosFacturacionVisitas.Penalizacion:
                            if (this.RadioConsultas.SelectedIndex == 2)
                            {
                                PlaceHolder3.Visible = true;
                                PlaceHolder4.Visible = false;
                                return;
                            }
                            else
                            {
                                PlaceHolder3.Visible = false;
                            }

                            break;
                        case (Int64)Enumerados.PermisosFacturacionVisitas.Reparacion:
                            if (this.RadioConsultas.SelectedIndex == 1)
                            {
                                PlaceHolder4.Visible = true;
                                PlaceHolder3.Visible = false;
                                return;
                            }
                            else
                            {
                                PlaceHolder4.Visible = false;
                            }
                            break;
                        case (Int64)Enumerados.PermisosFacturacionVisitas.Visitas:
                            if (this.RadioConsultas.SelectedIndex == 0)
                            {
                                PlaceHolder4.Visible = true;
                                PlaceHolder3.Visible = false;
                                return;
                            }
                            else
                            {
                                PlaceHolder4.Visible = false;
                            }
                            break;
                    }
                }
                


            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
            //finally
            //{
            //    String Script = "<script language='javascript'>OcultarCapaEspera();</script>";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTAR", Script, false);
            //}
        }

        protected void TxtFechaHastaOp2CustomValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox (txtFecha0 , true, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void TxtFechaHastaOp1CustomValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFecha, true, (BaseValidator)sender);
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
            CargarDatos(intPageIndex);
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
                        if (typeof(System.Web.UI.WebControls.BoundField) == grdListado.Columns[i].GetType())
                        {
                            columnName = ((System.Web.UI.WebControls.BoundField)(grdListado.Columns[i])).DataField;
                            columnValue = e.Row.Cells[i].Text.Trim();
                            ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarVisitas", columnName);
                            strWidth = (columnDefinition.Width).ToString();


                            //e.Row.Attributes["style"] += "value:" + e.Row.RowIndex + ";";//value" + i + ": " + e.Row.Cells[i].Text + ";";

                            //e.Row.Cells[i].Attributes.Add("OnMouseOver", "ObtenerCelda(" + i + ");");

                            e.Row.Cells[i].Attributes["style"] += "cursor:default;value: " + e.Row.Cells[i].Text;
                            e.Row.Cells[i].ToolTip = formatString(e.Row.Cells[i].Text);

                            ////FORMATO CELDA RECLAMACIÓN.
                            //if (i == 48)
                            //{
                            //    if (e.Row.Cells[i].Text == "True")
                            //    {
                            //        e.Row.Cells[i].Text = "ABIERTA";
                            //        e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                            //        e.Row.Cells[i].BackColor = Color.Green;
                            //    }
                            //    else
                            //    {
                            //        e.Row.Cells[i].Text = "CERRADA";
                            //        e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                            //        e.Row.Cells[i].BackColor = Color.Red;
                            //    }
                            //    e.Row.Cells[i].ForeColor = Color.White;
                            //}

                            ////FORMATO CELDA CONTRATO.
                            //if (columnName == "COD_CONTRATO_SIC")
                            //{
                            //    e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'><a href='#' onclick='GridViewRowClick(\"" + columnValue + "\")'>" + columnValue + "</a></div>";
                            //}
                            //else
                            //{
                            //    e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + columnValue + "</div>";
                            //}

                            e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + columnValue + "</div>";
                            //FORMATO CELDA TIPO URGENCIA.
                            if (columnName == "TIPO URGENCIA")
                            {
                                
                                switch (columnValue)
                                {
                                    case "VENCIDO":
                                        e.Row.Cells[i].BackColor = Color.Red;
                                        break;
                                    case "MUY URGENTE":
                                        e.Row.Cells[i].BackColor = Color.Yellow;
                                        break;
                                    case "URGENTE":
                                        e.Row.Cells[i].BackColor = Color.Orange;
                                        break;
                                    case "BONIFICABLE":
                                        e.Row.Cells[i].BackColor = Color.GreenYellow;
                                        break;
                                    case "PENALIZABLE":
                                        e.Row.Cells[i].BackColor = Color.Fuchsia;
                                        break;
                                }
                            }


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void btnPenalizar_Click(object sender, EventArgs e)
        {
            try
            {

                // validamos si hay registros seleccionados y si el número de registros no supera el límite.
                String strNumReg = this.lblNumEncontrados.Text;
                Int32 intNumReg = 0;
                if (strNumReg != null && strNumReg.Length > 0)
                {
                    intNumReg = Int32.Parse(strNumReg);
                }

                if (intNumReg == 0)
                {
                    MostrarMensaje("No se pueden penalizar visitas porque no se han encontrado registros.");
                    return;
                }

                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                String proveedor = usuarioDTO.NombreProveedor;
                if (proveedor == "ADICO")
                {
                    proveedor = this.cmbProveedor.SelectedValue;
                }
                String Fecha;
                Fecha = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                VisitasDB Visita= new VisitasDB();
                Visita.ActualizarPenalizacionVisita(proveedor, Fecha);
                

                //InicalizarGrid();
                MostrarMensaje("Proceso Completado");
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        private void InicializarGrid()
        {
            //String Script = "<script language='javascript'>MostrarCapaEspera();</script>";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "MOSTRAR", Script, false);
            ////grdListado.DataSource = null;

            //////this.grdListado.Columns.Clear();
            //grdListado.EnableViewState = false;
            ////grdListado.DataBind();
            for (int i = 0; i < grdListado.Columns.Count; i++)
            {
                grdListado.Columns[i].Visible = false;
            }

            //grdListado.Visible = false;
            this.txtFecha.Text = "";
            this.txtFecha0.Text = "";

            lblNumEncontrados.Text = "0";

            this.lblNumEncontrados.Text = "";
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
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            //try
            //{
            try
            {
                this.grdListado.EnableViewState = true;
                // validamos si hay registros seleccionados y si el número de registros no supera el límite.
                String strNumReg = this.lblNumEncontrados.Text;
                Int32 intNumReg = 0;
                if (strNumReg != null && strNumReg.Length > 0)
                {
                    intNumReg = Int32.Parse(strNumReg);
                }

                if (intNumReg == 0)
                {
                    MostrarMensaje("No se puede cambiar el número de factura porque no se han encontrado registros.");
                    return;
                }

                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                String proveedor = usuarioDTO.NombreProveedor;
                if (proveedor == "ADICO")
                {
                    proveedor = this.cmbProveedor.SelectedValue;
                }

                switch (RadioConsultas.SelectedIndex)
                {
                    case 0:
                        AbrirPagina("./FrmModalActualizarNumeroFactura.aspx?Id=0&Fecha=" + this.txtFecha.Text + "&Proveedor=" + proveedor);
                        break;
                    case 1:
                        AbrirPagina("./FrmModalActualizarNumeroFactura.aspx?Id=1&Fecha=" + this.txtFecha0.Text + "&Proveedor=" + proveedor);
                        break;
                }

                //InicalizarGrid();

                //}
                //catch (BaseException ex)
                //{

                //}
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void AbrirPagina(String Pagina)
        {
            string alerta = "<script type='text/javascript'>abrirVentanaLocalizacion('" + Pagina + "', '600', '350', 'ventana-modal','ACTUALIZAR NUMERO FACTURA','0','0');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", alerta, false);
        }

        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", alerta, false);
        }
        protected void grdListado_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = new DataTable();// = grdListado.DataSource as DataTable;
            IDataReader dr;
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            String proveedor = usuarioDTO.NombreProveedor;

            switch (RadioConsultas.SelectedIndex)
            {
                case 0:
                    dr = Consultas.ConsultasVisitasaFacturar(txtFecha, proveedor);
                    dt.Load(dr);
                    break;
                case 1:
                    dr = Consultas.ConsultasReparacionesaFacturar(txtFecha0, proveedor);
                    dt.Load(dr);
                    break;
                case 2:
                    dr = Consultas.ConsultasVisitasVencidas(proveedor);
                    dt.Load(dr);
                    break;
                case 3:
                    dr = Consultas.ConsultasVisitasPenalizables(proveedor);
                    dt.Load(dr);
                    break;
                default:
                    break;
            }
            if (dt != null)
            {
                DataView dataView = new DataView(dt);
                //dataView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                 for (int i = 0; i < grdListado.Columns.Count; i++)
                    {
                        grdListado.Columns[i].Visible = false;
                    }

                foreach (DataColumn dc in dt.Columns)
                {
                    int j = 0;
                    String ColumnName;
                    Int32 ColumnWidth;
                    String ColumnStyle;
                    Boolean IsVisible;


                    Int32 gridWidth = 0;

                    int _temp = 0;
                    _temp += 1;
                    
                    ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarVisitas", dc.ColumnName);
                    ColumnName = columnDefinition.Name;
                    ColumnWidth = columnDefinition.Width;
                    ColumnStyle = columnDefinition.Style;
                    //if (e.SortExpression == dc.ColumnName) { e.SortExpression = ColumnName; }

                    gridWidth += ColumnWidth;


                    BoundField bc = new BoundField();
                    bc.HeaderText = ColumnName;
                    bc.SortExpression = (String)dc.ColumnName;
                    bc.DataField = (String)dc.ColumnName;
                    bc.HeaderStyle.Width = Unit.Pixel(ColumnWidth);
                    bc.ItemStyle.Width = Unit.Pixel(ColumnWidth);
                    bc.ItemStyle.CssClass = ColumnStyle;
                    
                    FormatearColumnaDataGrid(dc, ref bc);
                    grdListado.Columns.Add(bc);
                }
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);// dataView;
                grdListado.DataSource = dt.DefaultView;// dataView;
                grdListado.DataBind();
            }
        }
        public string SortExpression
        {
            get { return (ViewState["SortExpression"] == null ? string.Empty : ViewState["SortExpression"].ToString()); }
            set { ViewState["SortExpression"] = value; }
        }

        public string SortDirection
        {
            get { return (ViewState["SortDirection"] == null ? string.Empty : ViewState["SortDirection"].ToString()); }
            set { ViewState["SortDirection"] = value; }
        }

        private string GetSortDirection(string sortExpression)
        {
            if (SortExpression == sortExpression)
            {
                if (SortDirection == "ASC")
                    SortDirection = "DESC";
                else if (SortDirection == "DESC")
                    SortDirection = "ASC";
                return SortDirection;
            }
            else
            {
                SortExpression = sortExpression;
                SortDirection = "ASC";
                return SortDirection;
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

        private void CargarDatos(Int64 NumPagina)
        {
            try
            {
                if (this.ValidatePage())
                {
                    
                    for (int i = 0; i < grdListado.Columns.Count; i++)
                    {
                        grdListado.Columns[i].Visible = false;
                    }
                    
                    //PlaceHolder5.Visible = true;

                    AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                    String proveedor = usuarioDTO.NombreProveedor;
                    if (proveedor == "ADICO")
                    {
                        proveedor = this.cmbProveedor.SelectedValue;
                    }
                    IDataReader dr;
                    IDataReader drExcel;
                    DataTable dt = new DataTable();
                    DataTable dtExcel = new DataTable();
                    //grdListado.EnableViewState = false;
                    switch (RadioConsultas.SelectedIndex)
                    {
                        case 0:
                            if (NumPagina <= 1)
                            {
                                //dr = Consultas.ConsultasVisitasaFacturar(txtFecha, proveedor);
                                drExcel = Consultas.ConsultasVisitasaFacturar(txtFecha, proveedor);
                                //dt.Load(dr);
                                dtExcel.Load(drExcel);

                                lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();
                            }
                            dr = null;
                            dt = new DataTable();
                            dr = Consultas.ConsultasVisitasaFacturarPorPagina(txtFecha, proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50);
                            dt.Load(dr);
                            
                            foreach (DataColumn dc in dt.Columns)
                            {
                                String ColumnName;
                                Int32 ColumnWidth;
                                String ColumnStyle;
                                Int32 gridWidth = 0;

                                int _temp = 0;
                                _temp += 1;
                                ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarVisitas", dc.ColumnName);
                                ColumnName = columnDefinition.Name;
                                ColumnWidth = columnDefinition.Width;
                                ColumnStyle = columnDefinition.Style;

                                gridWidth += ColumnWidth;
                                BoundField bc = new BoundField();
                                bc.HeaderText = ColumnName;
                                bc.SortExpression = (String)dc.ColumnName;
                                bc.DataField = (String)dc.ColumnName;
                                bc.HeaderStyle.Width = Unit.Pixel(ColumnWidth);
                                bc.ItemStyle.Width = Unit.Pixel(ColumnWidth);
                                bc.ItemStyle.CssClass = ColumnStyle;
                                FormatearColumnaDataGrid(dc, ref bc);
                                grdListado.Columns.Add(bc);
                            }
                            grdListado.DataSource = dt;
                            grdListado.DataBind();
                            
                            CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL, dr);
                            break;
                        case 1:
                            if (NumPagina <= 1)
                            {
                                //dr = Consultas.ConsultasReparacionesaFacturar(txtFecha0, proveedor);
                                drExcel = Consultas.ConsultasReparacionesaFacturar(txtFecha0, proveedor);
                                //dt.Load(dr);
                                dtExcel.Load(drExcel);
                                lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();
                            }
                            dr = null;
                            dt = new DataTable();
                            dr = Consultas.ConsultasReparacionesaFacturarPorPagina(txtFecha0, proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50);
                            dt.Load(dr);
                            foreach (DataColumn dc in dt.Columns)
                            {
                                String ColumnName;
                                Int32 ColumnWidth;
                                String ColumnStyle;
                                Int32 gridWidth = 0;

                                int _temp = 0;
                                _temp += 1;
                                ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarVisitas", dc.ColumnName);
                                ColumnName = columnDefinition.Name;
                                ColumnWidth = columnDefinition.Width;
                                ColumnStyle = columnDefinition.Style;

                                gridWidth += ColumnWidth;

                                BoundField bc = new BoundField();
                                bc.HeaderText = ColumnName;
                                bc.SortExpression = (String)dc.ColumnName;
                                bc.DataField = (String)dc.ColumnName;
                                bc.HeaderStyle.Width = Unit.Pixel(ColumnWidth);
                                bc.ItemStyle.Width = Unit.Pixel(ColumnWidth);
                                bc.ItemStyle.CssClass = ColumnStyle;
                                FormatearColumnaDataGrid(dc, ref bc);
                                grdListado.Columns.Add(bc);
                            }

                            grdListado.DataSource = dt;
                            grdListado.DataBind();
                            
                            CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL, dr);
                            break;
                        case 2:
                            if (NumPagina <= 1)
                            {
                                //dr = Consultas.ConsultasVisitasVencidas(proveedor);
                                drExcel = Consultas.ConsultasVisitasVencidas(proveedor);
                                //dt.Load(dr);
                                dtExcel.Load(drExcel);
                                lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();
                            }
                            dr = null;
                            dt=new DataTable();
                            dr = Consultas.ConsultasVisitasVencidasPorPagina(proveedor, (NumPagina-1) * 50, ((NumPagina-1) * 50) + 50);
                            dt.Load(dr);
                            
                            foreach (DataColumn dc in dt.Columns)
                            {
                                String ColumnName;
                                Int32 ColumnWidth;
                                String ColumnStyle;
                                Int32 gridWidth = 0;

                                int _temp = 0;
                                _temp += 1;
                                ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarVisitas", dc.ColumnName);
                                ColumnName = columnDefinition.Name;
                                ColumnWidth = columnDefinition.Width;
                                ColumnStyle = columnDefinition.Style;

                                gridWidth += ColumnWidth;

                                BoundField bc = new BoundField();
                                bc.HeaderText = ColumnName;
                                bc.SortExpression = (String)dc.ColumnName;
                                bc.DataField = (String)dc.ColumnName;
                                bc.HeaderStyle.Width = Unit.Pixel(ColumnWidth);
                                bc.ItemStyle.Width = Unit.Pixel(ColumnWidth);
                                bc.ItemStyle.CssClass = ColumnStyle;
                                FormatearColumnaDataGrid(dc, ref bc);
                                grdListado.Columns.Add(bc);
                            }
                            grdListado.DataSource = dt;
                            grdListado.DataBind();
                            
                            CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL, dr);
                            break;
                        case 3:
                            if (NumPagina <= 1)
                            {
                                //dr = Consultas.ConsultasVisitasPenalizables(proveedor);
                                drExcel = Consultas.ConsultasVisitasPenalizables(proveedor);
                                //dt.Load(dr);
                                dtExcel.Load(drExcel);
                                lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();
                            }
                            dr = null;
                            dt = new DataTable();
                            dr = Consultas.ConsultasVisitasPenalizablesPorPagina(proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50);
                            dt.Load(dr);
                            foreach (DataColumn dc in dt.Columns)
                            {
                                int j = 0;
                                String ColumnName;
                                Int32 ColumnWidth;
                                String ColumnStyle;
                                Boolean IsVisible;


                                Int32 gridWidth = 0;

                                int _temp = 0;
                                _temp += 1;
                                ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarVisitas", dc.ColumnName);
                                ColumnName = columnDefinition.Name;
                                ColumnWidth = columnDefinition.Width;
                                ColumnStyle = columnDefinition.Style;

                                gridWidth += ColumnWidth;

                                BoundField bc = new BoundField();
                                bc.HeaderText = ColumnName;
                                bc.SortExpression = (String)dc.ColumnName;
                                bc.DataField = (String)dc.ColumnName;
                                bc.HeaderStyle.Width = Unit.Pixel(ColumnWidth);
                                bc.ItemStyle.Width = Unit.Pixel(ColumnWidth);
                                bc.ItemStyle.CssClass = ColumnStyle;
                                FormatearColumnaDataGrid(dc, ref bc);
                                grdListado.Columns.Add(bc);
                            }
                            grdListado.DataSource = dt;
                            grdListado.DataBind();
                            lblNumEncontrados.Text = dt.Rows.Count.ToString();
                            CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL, dr);
                            break;
                        default:
                            break;
                    }
                    grdListado.Visible = true;

                    if (NumPagina <= 1)
                    {
                        CurrentSession.SetAttribute(CurrentSession.SESSION_LIMITE_DATOS_EXCEL, "SI");
                        ExcelHelper excel = new ExcelHelper("NO", "NO");
                        //if (dtExcel.Rows.Count >= 10000)
                        //{
                        //    CurrentSession.SetAttribute(CurrentSession.SESSION_LIMITE_DATOS_EXCEL, "NO");
                        //}

                        CurrentSession.SetAttribute(CurrentSession.SESSION_EXCEL_HELPER, excel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dtExcel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, this.RadioConsultas.SelectedItem.Text.ToString());
                    }

                    Int32 numReg = int.Parse(this.lblNumEncontrados.Text.ToString());


                    this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                    this.hdnPageIndex.Value = "1";
                    
                    CargarPaginador(int.Parse(NumPagina.ToString()));

                    if (lblNumEncontrados.Text == "0") { MostrarMensaje("No se han encontrado datos"); }
                }
                else
                {
                    MostrarMensaje("Corrija los campos inválidos antes de continuar");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
            finally
            {
                String Script = "<script language='javascript'>OcultarCapaEspera();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTARCAPA", Script, false);

                ////grdListado.EnableViewState = true;
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
       
}

}