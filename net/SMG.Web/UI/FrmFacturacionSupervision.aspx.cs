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
    public partial class FrmFacturacionRevisionPrecinte : FrmBaseListado
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
            
                if (this.hdnPageIndex.Value == "") { this.hdnPageIndex.Value = "1"; }
                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                CargarDatos(intPageIndex);
            
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
        
        protected String acorta(Object texto)
        {
            if (texto.ToString().Length > 31)
            {
                return texto.ToString().Substring(0, 30) + "...";
            }
            else
            {
                return texto.ToString();
            }
        }

        protected void CargarDatos(int NumPagina)
        {
            try
            {
                IDataReader drDatos;
                IDataReader drExcel;
                DataTable dtExcel = new DataTable();
                DataTable dt = new DataTable();
                String Proveedor = this.cmbProveedorBusqueda.SelectedValue;
                if (Proveedor == "-1") { Proveedor = "ADICO"; }

                if (NumPagina == 1)
                {
                    drExcel = Consultas.ObtenerSolicitudesRevisionPrecinteRealizadas(Proveedor);
                    dtExcel.Load(drExcel);
                    lblNumEncontrados.Text = dtExcel.Rows.Count.ToString();
                }
                drDatos = Consultas.ObtenerSolicitudesRevisionPrecinteRealizadasPorPagina(Proveedor, (NumPagina - 1) * 50, ((NumPagina - 1) * 50) + 50);
                dt.Load(drDatos);
                grdListado.DataSource = dt;
                grdListado.DataBind();
                

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

        private void CargarCombos()
        {
            //carga de Proveedores
            UsuarioDB usuarioDB = new UsuarioDB();
            this.cmbProveedorBusqueda.DataSource = usuarioDB.ObtenerTodosProveedoresEnSolicitudes();
            cmbProveedorBusqueda.DataTextField = "PROVEEDOR";
            cmbProveedorBusqueda.DataValueField = "PROVEEDOR";
            this.cmbProveedorBusqueda.DataBind();

            FormUtils.AddDefaultItem(cmbProveedorBusqueda);
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

        protected void btnRealizarBusqueda_Click(object sender, EventArgs e)
        {
            CargarDatos(1);
        }
    }
}