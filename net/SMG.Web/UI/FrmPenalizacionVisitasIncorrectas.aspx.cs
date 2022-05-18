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
    public partial class  FrmPenalizacionVisitasIncorrectas : FrmBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
            //Response.Write(DateTime.Now);
            if (!IsPostBack)
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                UsuarioDB usuarioDB = new UsuarioDB();

                this.cmbProveedorBusqueda.DataSource = usuarioDB.ObtenerTodosProveedoresEnSolicitudes();
                cmbProveedorBusqueda.DataTextField = "PROVEEDOR";
                cmbProveedorBusqueda.DataValueField = "PROVEEDOR";
                this.cmbProveedorBusqueda.DataBind();


                FormUtils.AddDefaultItem(cmbProveedorBusqueda);

                rdbBusquedas.Attributes.Add("onclick", "Busquedas()");
            }
            else
            {
                this.btnProcesar.Enabled = false;
                if (rdbBusquedas0.Items[0].Selected)// || rdbBusquedas0.Items[1].Selected)
                {
                    this.btnProcesar.Enabled = true;
                }
                else
                {
                    this.btnProcesar.Enabled = false;
                }
            }
            txtNumFactura.Enabled = this.btnProcesar.Enabled;
            rdbBusquedas.Attributes.Add("onclick", "Busquedas()");
            rdbBusquedas0.Attributes.Add("onclick", "Consultas()");
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
                        ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarAverias", columnName);
                        strWidth = (columnDefinition.Width).ToString();

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
        


        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", alerta, false);
        }

        
        
        protected void btnRealizarBusqueda_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnProcesar.Enabled = false;
                if (rdbBusquedas0.Items[0].Selected)// || rdbBusquedas0.Items[1].Selected)
                {
                    this.btnProcesar.Enabled = true;
                }
                else
                {
                    this.btnProcesar.Enabled = false;
                }
                this.hdnPageIndex.Value = "1";
                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                CargarDatosBusqueda(intPageIndex);
            }
            catch (Exception ex)
            {
            }
        }
        protected String acorta(Object texto)
        {
            if(texto.ToString().Length > 31)
            {            
                return texto.ToString().Substring(0, 30) + "...";
            }
            else
            {
                return texto.ToString();
            }
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
                    proveedor = this.cmbProveedorBusqueda.SelectedValue;
                    if (proveedor == "-1") { proveedor = "ADI"; }
                }
                switch (rdbBusquedas.SelectedIndex)
                {
                    case 1:
                        //BONIFICABLES.
                        drExcel = Consultas.ObtenerVisitasINCORRECTASPenalizablesBonificables(proveedor, 1);
                        dtExcel.Load(drExcel);
                        lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();

                        datos = null;
                        dt = new DataTable();
                        datos = Consultas.ObtenerVisitasINCORRECTASPenalizablesBonificablesPorPagina(proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50, 1);// Consultas.ObtenerSolicitudesBonificables(proveedor);
                        dt.Load(datos);
                        
                        break;
                    case 0:
                        //PENALIZABLES.
                        drExcel = Consultas.ObtenerVisitasINCORRECTASPenalizablesBonificables(proveedor, 0);
                        dtExcel.Load(drExcel);
                        lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();

                        datos = null;
                        dt = new DataTable();
                        datos = Consultas.ObtenerVisitasINCORRECTASPenalizablesBonificablesPorPagina(proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50, 0);// Consultas.ObtenerSolicitudesBonificables(proveedor);
                        dt.Load(datos);

                        break;
                    case -1:
                        switch (rdbBusquedas0.SelectedIndex)
                        {
                            case 1:
                                //PROCESO BONIFICABLES.
                                drExcel = Consultas.ObtenerVisitasINCORRECTASAPenalizaBonificar(proveedor);
                                dtExcel.Load(drExcel);
                                lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();

                                datos = null;
                                dt = new DataTable();
                                datos = Consultas.ObtenerVisitasINCORRECTASAPenalizaBonificarPorPagina(proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50);// Consultas.ObtenerSolicitudesBonificables(proveedor);
                                dt.Load(datos);
                                break;
                            case 0:
                                //PROCESO PENALIZABLES.
                                drExcel = Consultas.ObtenerVisitasINCORRECTASAPenalizaBonificar(proveedor);
                                dtExcel.Load(drExcel);
                                lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();

                                datos = null;
                                dt = new DataTable();
                                datos = Consultas.ObtenerVisitasINCORRECTASAPenalizaBonificarPorPagina(proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50);// Consultas.ObtenerSolicitudesBonificables(proveedor);
                                dt.Load(datos);
                                break;
                        }
                        break;
                }

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

                if (rdbBusquedas.SelectedIndex == -1)
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, this.rdbBusquedas0.SelectedItem.Text.ToString());
                }
                else
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, this.rdbBusquedas.SelectedItem.Text.ToString());
                }
                


                Int32 numReg = int.Parse(this.lblNumEncontrados.Text.ToString());


                this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                this.hdnPageIndex.Value = "1";

                CargarPaginador(int.Parse(NumPagina.ToString()));

                if (lblNumEncontrados.Text == "0") { MostrarMensaje("No se han encontrado datos"); }
                txtNumFactura.Enabled = this.btnProcesar.Enabled;
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            String proveedor = usuarioDTO.NombreProveedor;
            if (proveedor == "ADICO")
            {
                proveedor = this.cmbProveedorBusqueda.SelectedValue;
                if (proveedor == "-1") { proveedor = "ADI"; }
            }

            Consultas.ActualizarNumeroFactura(proveedor, txtNumFactura.Text,(Int16)usuarioDTO.IdIdioma);

            //DataTable dtDatos = new DataTable(); 
            //dtDatos=(DataTable) CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_EXCEL);

            //for (int i = 0; i < dtDatos.Rows.Count; i++)
            //{
            //    //CheckBox Chk = (CheckBox)grdListado.Rows[i].Cells[0].Controls[1];
            //    //if (Chk.Checked == true)
            //    //{
            //        //Actualizamos el codigo de remesa en 
            //        //HiddenField HdnID = (HiddenField)grdListado.Rows[i].Cells[1].FindControl("hdnIdSolicitud");
            //        Int32 Id = Int32.Parse(dtDatos.Rows[i].ItemArray[0].ToString());//HdnID.Value.ToString());//Int32.Parse(grdFacturas.Rows[i].Cells[26].Text);
            //        //AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            //        //UsuarioDTO DatosUsuario = new UsuarioDTO();
            //        //DatosUsuario = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            //        //if (rdbBusquedas0.Items[0].Selected)
            //        //{
            //        //    //Actualizamos a bonificable.
            //        //    Consultas.ActualizarVisitasAPenalizarBonificar(Id, true);
            //        //}
            //        //else
            //        //{
            //            //Actualizamos a penalizable.
            //            Consultas.ActualizarVisitasAPenalizarBonificar(Id, false);
            //            //Consultas.ActualizarNumeroFactura(Id, txtNumFactura.Text);

            //        //}
                    
            //    //}
            //}

            MostrarMensaje("Proceso Completado");

            CargarDatosBusqueda(1);

        }
        protected void grdListado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    if (this.rdbBusquedas0.Items[0].Selected)// || this.rdbBusquedas0.Items[1].Selected)
            //    {
            //        grdListado.Columns[0].Visible = true;
            //    }
            //    else
            //    {
            //        grdListado.Columns[0].Visible = false;
            //    }
                
            //}
            //catch (Exception ex)
            //{
            //}
        }
        protected void rdbBusquedas0_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdbBusquedas.SelectedIndex = -1;
        }
        protected void rdbBusquedas_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdbBusquedas0.SelectedIndex = -1;
        }
}
}
