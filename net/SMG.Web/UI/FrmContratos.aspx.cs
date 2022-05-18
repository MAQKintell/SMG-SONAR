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
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;


namespace Iberdrola.SMG.UI
{
    public partial class FrmContratos : FrmBaseListado
    {
        private static String[] NombreColumnas=new String[55];
        private static Boolean Filtro;
        #region metodos privados
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

        private void CargarPaginador()
        {
            Int32 intNumPaginas = Int32.Parse(this.hdnPageCount.Value);
            Int32 intPaginaActual = Int32.Parse(this.hdnPageIndex.Value);
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

        /// <summary>
        /// Devuelve el tipo de vista (valor logico asociado al indice del control radioFiltros)
        /// </summary>
        /// <param name="selectedIndex"></param>
        /// <returns>
        /// Devuelve el tipo de vista segun el indice selecionado
        /// Si no existe dicho valor devuelve "SinFiltrar"
        /// </returns>
        private Enumerados.TipoVistaContratoCompleto ObtenerTipoVistaContratoRadiobutton(int indice)
        {
            Enumerados.TipoVistaContratoCompleto tipoVista;

            switch (indice)
            {
                case 0:
                    tipoVista = Enumerados.TipoVistaContratoCompleto.SinFiltrar;
                    break;
                case 1:
                    tipoVista = Enumerados.TipoVistaContratoCompleto.FechaUltimaVisita;
                    break;
                //case 2:
                //    tipoVista = Enumerados.TipoVistaContratoCompleto.Facturadas;
                //    break;
                //case 3:
                //    tipoVista = Enumerados.TipoVistaContratoCompleto.EstadoVisita;
                //    break;
                //case 4:
                //    tipoVista = Enumerados.TipoVistaContratoCompleto.MantenimientoGasCalefaccion;
                //    break;
                //case 5:
                //    tipoVista = Enumerados.TipoVistaContratoCompleto.MantenimientoGas;
                //    break;
                case 2:
                    tipoVista = Enumerados.TipoVistaContratoCompleto.PorLote;
                    break;
                case 3:
                    tipoVista = Enumerados.TipoVistaContratoCompleto.NoCerradasUrgencia;
                    break;
                case 4:
                    tipoVista = Enumerados.TipoVistaContratoCompleto.EjecucionOAplazadas;
                    break;
                case 5:
                    tipoVista = Enumerados.TipoVistaContratoCompleto.PorContrato;
                    break;
                default:
                    tipoVista = Enumerados.TipoVistaContratoCompleto.SinFiltrar;
                    break;
            }

            return tipoVista;
        }

        /// <summary>
        /// Devuelve una <see cref="Hashtable"/> con los parametros asociados al tipo de vista
        /// </summary>
        /// <param name="tipoVista"></param>
        /// <returns></returns>
        private Hashtable ObtenerParametrosTipoVista(Enumerados.TipoVistaContratoCompleto tipoVista)
        {
            Hashtable parametros = null;
            switch (tipoVista)
            {
                case Enumerados.TipoVistaContratoCompleto.SinFiltrar:
                    break;
                case Enumerados.TipoVistaContratoCompleto.FechaUltimaVisita:
                    parametros = new Hashtable();
                    parametros.Add(Enumerados.ParametrosContratoCompleto.fechaDesde, DateUtils.ParseDateTime(txtFechaDesdeOp2.Text));
                    parametros.Add(Enumerados.ParametrosContratoCompleto.fechaHasta, DateUtils.ParseDateTime(txtFechaHastaOp2.Text));
                    break;
                case Enumerados.TipoVistaContratoCompleto.Facturadas:
                    parametros = new Hashtable();
                    parametros.Add(Enumerados.ParametrosContratoCompleto.fechaDesde, DateUtils.ParseDateTime(txtFechaDesdeOp3.Text));
                    parametros.Add(Enumerados.ParametrosContratoCompleto.fechaHasta, DateUtils.ParseDateTime(txtFechaHastaOp3.Text));
                    break;
                case Enumerados.TipoVistaContratoCompleto.EstadoVisita:
                    parametros = new Hashtable();
                    parametros.Add(Enumerados.ParametrosContratoCompleto.CodigoVisita, cmbEstadoVisita.SelectedItem.Value);
                    break;
                case Enumerados.TipoVistaContratoCompleto.MantenimientoGasCalefaccion:
                    break;
                case Enumerados.TipoVistaContratoCompleto.MantenimientoGas:
                    break;
                case Enumerados.TipoVistaContratoCompleto.PorLote:
                    parametros = new Hashtable();
                    parametros.Add(Enumerados.ParametrosContratoCompleto.CodigoLote, txtIdLote.Text);
                    break;
                case Enumerados.TipoVistaContratoCompleto.NoCerradasUrgencia:
                    String codUrgencia = null;
                    if (FormUtils.HasValue(cmbTipoUrgencia))
                    {
                        codUrgencia = cmbTipoUrgencia.SelectedItem.Value.ToString();
                    }
                    parametros = new Hashtable();
                    parametros.Add(Enumerados.ParametrosContratoCompleto.CodigoUrgencia, codUrgencia);
                    break;
                case Enumerados.TipoVistaContratoCompleto.EjecucionOAplazadas:
                    break;
                case Enumerados.TipoVistaContratoCompleto.PorContrato :
                    parametros = new Hashtable();
                    parametros.Add(Enumerados.ParametrosContratoCompleto.Contrato, this.txtCodigoContrato.Text);
                    break;
            }
            return parametros;
        }


        private DataTable CargarDatos(VistaContratoCompletoDTO filtrosDTO, Int32? intPageIndex, Boolean isLoading, String sortColumn, String sortDirection)
        {
            DataTable dt = null;
            //try
            //{


                if (sortColumn == null || sortColumn.Length == 0)
                {
                    sortColumn = "COD_CONTRATO_SIC";
                    this.hdnSortColumn.Value = sortColumn;
                }

                sortColumn += " " + sortDirection;


                Enumerados.TipoVistaContratoCompleto tipoVista = this.ObtenerTipoVistaContratoRadiobutton(radioFiltros.SelectedIndex);
                Hashtable parametros = this.ObtenerParametrosTipoVista(tipoVista);

                if (isLoading)
                {
                    Int32 numReg = Contrato.ObtenerNumRegEncontrados(tipoVista, parametros, filtrosDTO);
                    Int32 numEncontrados = numReg;
                    if (numReg > 1000) { numReg = 1000; }
                    this.lblNumEncontrados.Text = numReg.ToString();// +" (" + numEncontrados + " Simi.)";
                    this.lblNumEncontrados1.Text = " (" + numEncontrados + " Simi.)";

                    this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                    this.hdnPageIndex.Value = "1";
                }
                dt = Contrato.ObtenerVistaContratos(tipoVista, parametros, filtrosDTO, intPageIndex, sortColumn);


            //}
            //catch (Exception ex)
            //{
            //    this.ShowMessage(ex.Message);
            //}
            
            return dt;
            
        }

        /// <summary>
        /// Descripcion CargarVisitas
        /// </summary>
        /// <param name="filtrosDTO"></param>
        /// <returns></returns>
        private List<VisitaDTO> CargarVisitas(VistaContratoCompletoDTO filtrosDTO)
        {
            Enumerados.TipoVistaContratoCompleto tipoVista = this.ObtenerTipoVistaContratoRadiobutton(radioFiltros.SelectedIndex);

            Hashtable parametros = this.ObtenerParametrosTipoVista(tipoVista);

            return Contrato.ObtenerVistaContratos(tipoVista, parametros, filtrosDTO);
        }

        private void CargarListado(VistaContratoCompletoDTO filtrosDTO, Int32? intPageIndex, Boolean isLoading)
        {
            if (filtrosDTO.FiltrosAvanzadosActivos == false)
            {
                filtrosDTO = new VistaContratoCompletoDTO();

                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                if (usuarioDTO.NombreProveedor != null && usuarioDTO.NombreProveedor.Length > 0)
                {
                    if (usuarioDTO.NombreProveedor.ToString() != "ADICO") { filtrosDTO.Proveedor = usuarioDTO.NombreProveedor.ToString(); }
                    filtrosDTO.ProveedorEntrada = usuarioDTO.NombreProveedor.ToString();
                }
                else
                {
                    filtrosDTO.Proveedor = null;
                }
            }
            Enumerados.TipoVistaContratoCompleto tipoVisita = this.ObtenerTipoVistaContratoRadiobutton(radioFiltros.SelectedIndex);
            Hashtable parametros = this.ObtenerParametrosTipoVista(tipoVisita);

            //Se guardan los datos de la nueva vista en sesion
            CurrentSession.SetAttribute("TipoUltimaVista", tipoVisita);
            CurrentSession.SetAttribute("ParametrosUltimaVista", parametros);

            DataTable dt = CargarDatos(filtrosDTO, intPageIndex, isLoading, this.hdnSortColumn.Value, this.hdnSortDirection.Value);
            if (intPageIndex == 1)
            {
                DataTable Datos = (DataTable)(CargarDatos(filtrosDTO, null, false, this.hdnSortColumn.Value, this.hdnSortDirection.Value));
                //IDataReader DatosReader = CargarDatos(filtrosDTO, null, false, hdnSortColumn.Value, this.hdnSortDirection.Value).CreateDataReader();
                //if (Datos.FieldCount <= Int32.Parse(Resources.SMGConfiguration.ExcelMaxRows))
                //CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL, DatosReader);
                
                
                



                //int Registros = 0;
                //while (DatosReader.Read())
                //{
                //    Registros++;
                //}

                if (Datos.Rows.Count + 1 <= Int32.Parse(Resources.SMGConfiguration.ExcelMaxRows))
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, Datos);
                    ExcelHelper excel = new ExcelHelper(Resources.TextosJavaScript.TEXTO_LISTADO_MANTENIMIENTOS, HttpContext.Current.Server.MapPath(Resources.SMGConfiguration.XsltConsultas));
                    ExcelHeaderAttribute headerAttrib = new ExcelHeaderAttribute("Fecha", DateTime.Now.ToShortDateString());
                    excel.TableName = grdListado.ID;
                    excel.Attributtes.Add(headerAttrib);
                    AniadirAtributosExcel(excel, filtrosDTO);
                    CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                }
                else
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, "NO");
                    CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL, "NO");
                }
            }

            CargarPaginador();

            FormUtils.CargarDataGrid(this.grdListado, this.DataGridCabecera, dt, this);

            if (grdListado.Rows.Count >= 10)
            {
                grdListado.ShowHeader = true;
                DataGridCabecera.Visible = true;
            }
            else
            {
                grdListado.ShowHeader = true;
                DataGridCabecera.Visible = false;
            }

            for (int i = 0; i < grdListado.Columns.Count; i++)
            {
                NombreColumnas[i] = grdListado.Columns[i].SortExpression.ToString();
            }
        }

        private void CargarCombos()
        {
            ////Carga Lotes
            //cmbCodigoLote.DataSource = TablasReferencia.ObtenerLotes();
            //cmbCodigoLote.DataValueField = "Id";
            //cmbCodigoLote.DataTextField = "CodigoDescripcion";
            //cmbCodigoLote.DataBind();
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            //carga de cmbEstadoVisita
            cmbEstadoVisita.DataSource = TablasReferencia.ObtenerTiposEstadoVisita((Int16) usuarioDTO.IdIdioma);
            cmbEstadoVisita.DataValueField = "Codigo";
            cmbEstadoVisita.DataTextField = "Descripcion";
            cmbEstadoVisita.DataBind();

            //carga de cmbTipoUrgencia
            cmbTipoUrgencia.DataSource = TablasReferencia.ObtenerTiposUrgencia((Int16)usuarioDTO.IdIdioma);
            cmbTipoUrgencia.DataValueField = "Id";
            cmbTipoUrgencia.DataTextField = "Descripcion";
            cmbTipoUrgencia.DataBind();
            FormUtils.AddDefaultItem(cmbTipoUrgencia);
        }
        private void OcultarControlesOpciones()
        {
            this.placeControlesOp2.Visible = false;
            this.placeControlesOp3.Visible = false;
            this.placeControlesOp4.Visible = false;
            //this.placeControlesOp5.Visible = false;
            this.placeControlesOp7.Visible = false;
            this.placeControlesOp8.Visible = false;
            this.placeControlesOp9.Visible = false;
        }
        private void MostrarOcultarControlesOpciones()
        {
            OcultarControlesOpciones();

            switch (this.radioFiltros.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    this.placeControlesOp2.Visible = true;
                    break;
                //case 2:
                //    this.placeControlesOp3.Visible = true;
                //    break;
                //case 3:
                //    this.placeControlesOp4.Visible = true;
                //    break;
                /*case 4:
                    this.placeControlesOp5.Visible = true; 
                    break;
                case 5:
                    this.placeControlesOp5.Visible = true; 
                    break;*/
                case 2:
                    this.placeControlesOp7.Visible = true;
                    break;
                case 3:
                    this.placeControlesOp8.Visible = true;
                    break;
                case 5:
                    this.placeControlesOp9.Visible = true;
                    break;

            }
        }
        private void CargarNumeroRegistros()
        {
            String proveedor = null;
            VistaContratoCompletoDTO filtrosDTO = new VistaContratoCompletoDTO();

            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                proveedor = usuarioDTO.NombreProveedor;
            }
            catch (BaseException be)
            {
                // do nothing
                return;
            }
            if (proveedor != "ADICO") { filtrosDTO.Proveedor = proveedor; }
            Int32 numReg = Contrato.ObtenerNumRegTotalContratosUltimaVisita(filtrosDTO);
            this.lblNumRegistros.Text = numReg.ToString();
        }
        private VistaContratoCompletoDTO ObtenerFiltros()
        {
            VistaContratoCompletoDTO filtrosDTO = null;

            filtrosDTO = (VistaContratoCompletoDTO)CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO);

            if (filtrosDTO == null)
            {
                filtrosDTO = new VistaContratoCompletoDTO();
            }

            filtrosDTO.FiltrosAvanzadosActivos = chkAplicarFiltrosAvanzados.Checked;

            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            if (usuarioDTO.NombreProveedor != null && usuarioDTO.NombreProveedor.Length > 0)
            {
                if (usuarioDTO.NombreProveedor.ToString() != "ADICO") { filtrosDTO.Proveedor = usuarioDTO.NombreProveedor.ToString(); }
                filtrosDTO.ProveedorEntrada = usuarioDTO.NombreProveedor.ToString();
            }
            else
            {
                filtrosDTO.Proveedor = null;
            }
            return filtrosDTO;

        }
        #endregion

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
                //throw new Exception("PRUEBA ERRORES GLOBALES");
                if (!IsPostBack)
                {
                    CargarCombos();
                    CargarNumeroRegistros();
                    //******************************************
                    AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                    VistaContratoCompletoDTO filtrosDTO = new VistaContratoCompletoDTO();
                    filtrosDTO.ProveedorEntrada = usuarioDTO.NombreProveedor;
                    //******************************************
                    if (NavigationController.IsBackward())
                    {
                        this.LoadSessionData();
                        MostrarOcultarControlesOpciones();
                        Int32? pageIndex = 1;
                        if (this.hdnPageIndex != null && this.hdnPageIndex.Value != null && this.hdnPageIndex.Value.Length > 0)
                        {
                            pageIndex = NumberUtils.GetInt(this.hdnPageIndex.Value);
                        }
                        CargarListado(ObtenerFiltros(), pageIndex, true);
                    }
                    else
                    {
                        this.chkAplicarFiltrosAvanzados.Checked = true;
                    }
                }
                if (this.hdnRecargarUltimaConsulta.Value.Equals("TRUE"))
                {
                    String strNumReg = this.lblNumEncontrados.Text;
                    Int32 intNumReg = 0;
                    if (strNumReg != null && strNumReg.Length > 0)
                    {
                        intNumReg = Int32.Parse(strNumReg);
                    }

                    if (intNumReg != 0)
                    {
                        CargarListado(ObtenerFiltros(), 1, true);
                    }
                    this.hdnRecargarUltimaConsulta.Value = "FALSE";
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnBtnPaginacion_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                CargarListado(ObtenerFiltros(), intPageIndex, false);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        //public override void OnRowSelected_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (sender.GetType().FullName.ToString().Equals("System.Web.UI.WebControls.LinkButton"))
        //        {
        //            LinkButton lbtn = (LinkButton)sender;
        //            this.SaveSessionData();
        //            NavigationController.Forward("./FrmDetalleContrato.aspx?COD_CONTRATO=" + lbtn.Text);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ManageException(ex);
        //    }
        //}
        public void OnBtnRowSelected_Click(object sender, EventArgs e)
        {
            try
            {
                String strIdContrato = this.hdnRowIndex.Value;
                this.SaveSessionData();
                NavigationController.Forward("./FrmDetalleContrato.aspx?COD_CONTRATO=" + strIdContrato);

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
  
        
        //protected string ExportarDatagrid(DataTable dtSolicitudes)
        //{
        ////***************
        //String nombreExcel = "Solicitudes_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xls";
        //StringBuilder sb= new StringBuilder();
        //StringWriter sw = new StringWriter(sb);
        //HtmlTextWriter htw = new HtmlTextWriter(sw);
        //DataGrid a = new DataGrid();
        //Page Page = new Page();
        //HtmlForm form = new HtmlForm();


        //a.DataSource = dtSolicitudes;
        //a.DataBind();



        
        //a.RenderControl(htw);

        //HttpContext.Current.Response.Clear();
        //HttpContext.Current.Response.Buffer = true;
        //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=fichero.xls");
        //HttpContext.Current.Response.Charset = "UTF-8";
        //HttpContext.Current.Response.ContentEncoding = Encoding.Default;
        //HttpContext.Current.Response.Write("SOLICITUDES");
        //HttpContext.Current.Response.Write(sb.ToString());
        //HttpContext.Current.Response.End();

        //return nombreExcel;
        //}

        protected void OnBtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //String ruta = ExportarDatagrid((DataTable)(CargarDatos(ObtenerFiltros(), null, false, this.hdnSortColumn.Value, this.hdnSortDirection.Value)));

                        


                // validamos si hay registros seleccionados y si el número de registros no supera el límite.
                String strNumReg = this.lblNumEncontrados.Text;
                Int32 intNumReg = 0;
                if (strNumReg != null && strNumReg.Length > 0)
                {
                    intNumReg = Int32.Parse(strNumReg);
                }

                if (intNumReg == 0)
                {
                    this.ShowMessage("No se puede generar el documento Excel porque no se han encontrado registros.");
                    return;
                }

                if (intNumReg >= Int32.Parse(Resources.SMGConfiguration.ExcelMaxRows))
                {
                    this.ShowMessage("No se puede generar el documento Excel con más de " + Resources.SMGConfiguration.ExcelMaxRows + " registros porque tardaría demasiado.");
                    return;
                }

                // si hemos llegado a este punto es que podemos generar el Excel
                //ExcelHelper excel = new ExcelHelper("Listado de Mantenimientos", HttpContext.Current.Server.MapPath(Resources.SMGConfiguration.XsltConsultas));
                //ExcelHeaderAttribute headerAttrib = new ExcelHeaderAttribute("Fecha", DateTime.Now.ToShortDateString());
                //excel.Attributtes.Add(headerAttrib);
                //VistaContratoCompletoDTO filtrosDTO = ObtenerFiltros();
                //AniadirAtributosExcel(excel, filtrosDTO);

                //excel.TableName = grdListado.ID;

                DataTable Datos=(DataTable)(CargarDatos(ObtenerFiltros(), null, false, this.hdnSortColumn.Value, this.hdnSortDirection.Value));
                //TODO: ojo que estamos pasando un data reader al excel.
                //excel.LoadData(CargarDatos(ObtenerFiltros(), null, false, hdnSortColumn.Value, this.hdnSortDirection.Value).CreateDataReader());
                IDataReader DatosReader=CargarDatos(ObtenerFiltros(), null, false, hdnSortColumn.Value, this.hdnSortDirection.Value).CreateDataReader();
                //CurrentSession.SetAttribute(CurrentSession.SESSION_EXCEL_HELPER, excel);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, Datos);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL, DatosReader);
                
                ////this.OpenExternalWindow("./AbrirExcel.aspx", "AbrirExcel", "Consulta");
                //abrirVentanaLocalizacion('../UI/AbrirExcel.aspx', 700, 400, 'ventana-modal','EXPORTAR EXCEL','0','1');
                ////Response.Write("<script language='javascript'>window.open('./AbrirExcel.aspx','EXPORTAR','menubar=1,resizable=1,width=150,height=150');</script>");
                //ClientScript.RegisterStartupScript(Page.GetType(), "PopupScript", "<script language='javascript'>window.open('./AbrirExcel.aspx','EXPORTAR','menubar=1,resizable=1,width=150,height=80');</script>");

                ////OnClientClick="javascript:ExportarExcel();return false;"
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void AniadirAtributosExcel(ExcelHelper excel, VistaContratoCompletoDTO filtrosDTO)
        {
            #region Area radiobuttons
            switch (this.radioFiltros.SelectedIndex)
            {
                case 0:
                    ExcelHeaderAttribute headerAttribSinFiltrar = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
                    excel.Attributtes.Add(headerAttribSinFiltrar);
                    break;
                case 1:
                    ExcelHeaderAttribute headerAttribUltimaVisita = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
                    ExcelHeaderAttribute headerAttribUltimaVisitaFechaDesdeOp2 = new ExcelHeaderAttribute("Fecha desde: ", this.txtFechaDesdeOp2.Text);
                    ExcelHeaderAttribute headerAttribUltimaVisitaFechaHastaOp2 = new ExcelHeaderAttribute("Fecha hasta: ", this.txtFechaHastaOp2.Text);
                    excel.Attributtes.Add(headerAttribUltimaVisita);
                    excel.Attributtes.Add(headerAttribUltimaVisitaFechaDesdeOp2);
                    excel.Attributtes.Add(headerAttribUltimaVisitaFechaHastaOp2);
                    break;
                case 2:
                    ExcelHeaderAttribute headerAttribFacturadas = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
                    ExcelHeaderAttribute headerAttribUltimaVisitaFechaDesdeOp3 = new ExcelHeaderAttribute("Fecha desde: ", this.txtFechaDesdeOp3.Text);
                    ExcelHeaderAttribute headerAttribUltimaVisitaFechaHastaOp3 = new ExcelHeaderAttribute("Fecha hasta: ", this.txtFechaHastaOp3.Text);
                    excel.Attributtes.Add(headerAttribFacturadas);
                    excel.Attributtes.Add(headerAttribUltimaVisitaFechaDesdeOp3);
                    excel.Attributtes.Add(headerAttribUltimaVisitaFechaHastaOp3);
                    break;
                case 3:
                    ExcelHeaderAttribute headerAttribEstadoVisita = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
                    ExcelHeaderAttribute headerAttribEstadoDeVisita = new ExcelHeaderAttribute("Estado de la visita: ", this.cmbEstadoVisita.SelectedItem.Text);
                    excel.Attributtes.Add(headerAttribEstadoVisita);
                    excel.Attributtes.Add(headerAttribEstadoDeVisita);
                    break;
                case 4:
                    ExcelHeaderAttribute headerAttribGasCalefaccion = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
                    excel.Attributtes.Add(headerAttribGasCalefaccion);
                    break;
                case 5:
                    ExcelHeaderAttribute headerAttribGas = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
                    excel.Attributtes.Add(headerAttribGas);
                    break;
                case 6:
                    ExcelHeaderAttribute headerAttribLote = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
                    ExcelHeaderAttribute headerAttribIdLote = new ExcelHeaderAttribute("Número del lote: ", this.txtIdLote.Text);
                    excel.Attributtes.Add(headerAttribLote);
                    excel.Attributtes.Add(headerAttribIdLote);
                    if (this.txtDescripcionLote.Text != "")
                    {
                        ExcelHeaderAttribute headerAttribDescripcionLote = new ExcelHeaderAttribute("Descripción: ", this.txtDescripcionLote.Text);
                        excel.Attributtes.Add(headerAttribDescripcionLote);
                    }
                    break;
                case 7:
                    ExcelHeaderAttribute headerAttribUrgencia = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
                    ExcelHeaderAttribute headerAttribTipoUrgencia = new ExcelHeaderAttribute("Tipo de la urgencia: ", this.cmbTipoUrgencia.SelectedItem.Text);
                    excel.Attributtes.Add(headerAttribUrgencia);
                    excel.Attributtes.Add(headerAttribTipoUrgencia);
                    break;
                case 8:
                    ExcelHeaderAttribute headerAttribAplazadasOEjecucion = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
                    excel.Attributtes.Add(headerAttribAplazadasOEjecucion);
                    break;
            }

            #endregion

            #region Area Filtros Avanzados


            if (filtrosDTO.CodigoContrato != null && filtrosDTO.CodigoContrato != "")
            {
                ExcelHeaderAttribute headerAttribAvanzadosCodContrato = new ExcelHeaderAttribute("Código del contrato: ", filtrosDTO.CodigoContrato);
                excel.Attributtes.Add(headerAttribAvanzadosCodContrato);
            }
            if (filtrosDTO.Nombre != null && filtrosDTO.Nombre != "")
            {
                ExcelHeaderAttribute headerAttribAvanzadosNombre = new ExcelHeaderAttribute("Nombre: ", filtrosDTO.Nombre);
                excel.Attributtes.Add(headerAttribAvanzadosNombre);
            }
            if (filtrosDTO.Apellido1 != null && filtrosDTO.Apellido1 != "")
            {
                ExcelHeaderAttribute headerAttribAvanzadosApellido1 = new ExcelHeaderAttribute("1º Apellido: ", filtrosDTO.Apellido1);
                excel.Attributtes.Add(headerAttribAvanzadosApellido1);
            }
            if (filtrosDTO.Apellido2 != null && filtrosDTO.Apellido2 != "")
            {
                ExcelHeaderAttribute headerAttribAvanzadosApellido2 = new ExcelHeaderAttribute("2º Apellido: ", filtrosDTO.Apellido2);
                excel.Attributtes.Add(headerAttribAvanzadosApellido2);
            }
            if (filtrosDTO.CodigoDeProvincia != null && filtrosDTO.CodigoDeProvincia != "")
            {
                Contrato contrato = new Contrato();
                //IDataReader dr = contrato.obtenerFiltrosAvanzadosExcelProvincia(filtrosDTO);
                DataTable dt=new DataTable();


                String[] Provincias = filtrosDTO.CodigoDeProvincia.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Provincias.Length - 1; i++)
                {
                    //dt.Rows.Add(Provincias[i]);
                    ExcelHeaderAttribute headerAttribAvanzadosProvincia = new ExcelHeaderAttribute("Provincia: ", Provincias[i]);
                    excel.Attributtes.Add(headerAttribAvanzadosProvincia);
                }
                
                //dr.Read();
                //ExcelHeaderAttribute headerAttribAvanzadosProvincia = new ExcelHeaderAttribute("Provincia: ", (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE"));
                //excel.Attributtes.Add(headerAttribAvanzadosProvincia);
            }
            if (filtrosDTO.CodigoDePoblacion != null && filtrosDTO.CodigoDePoblacion != "")
            {
                Contrato contrato = new Contrato();
                
                //IDataReader dr = contrato.obtenerFiltrosAvanzadosExcelProvincia(filtrosDTO);
                DataTable dt = new DataTable();


                String[] Poblacion = filtrosDTO.CodigoDePoblacion.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Poblacion.Length - 1; i++)
                {
                    //dt.Rows.Add(Provincias[i]);
                    ExcelHeaderAttribute headerAttribAvanzadosPoblacion = new ExcelHeaderAttribute("Poblacion: ", Poblacion[i]);
                    excel.Attributtes.Add(headerAttribAvanzadosPoblacion);
                }
                //IDataReader dr = contrato.obtenerFiltrosAvanzadosExcelPoblacion(filtrosDTO);
                //dr.Read();
                //ExcelHeaderAttribute headerAttribAvanzadosPoblacion = new ExcelHeaderAttribute("Poblacion: ", (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE"));
                //excel.Attributtes.Add(headerAttribAvanzadosPoblacion);
            }
            if (filtrosDTO.CodigoPostal != null && filtrosDTO.CodigoPostal != "")
            {
                ExcelHeaderAttribute headerAttribAvanzadosCodPostal = new ExcelHeaderAttribute("Código postal: ", filtrosDTO.CodigoPostal);
                excel.Attributtes.Add(headerAttribAvanzadosCodPostal);
            }
            if (filtrosDTO.DescripcionTipoUrgencia != null && filtrosDTO.DescripcionTipoUrgencia != "")
            {
                ExcelHeaderAttribute headerAttribAvanzadosDescripcionTipoUrgencia = new ExcelHeaderAttribute("Descripcion de la urgencia: ", filtrosDTO.DescripcionTipoUrgencia);
                excel.Attributtes.Add(headerAttribAvanzadosDescripcionTipoUrgencia);
            }
            if (filtrosDTO.Campania != null)
            {
                ExcelHeaderAttribute headerAttribAvanzadosCampania = new ExcelHeaderAttribute("Campaña: ", Convert.ToString(filtrosDTO.Campania));
                excel.Attributtes.Add(headerAttribAvanzadosCampania);
            }

            if (filtrosDTO.IdLote != null)
            {
                ExcelHeaderAttribute headerAttribAvanzadosIdLote = new ExcelHeaderAttribute("Código del lote: ", Convert.ToString(filtrosDTO.IdLote));
                excel.Attributtes.Add(headerAttribAvanzadosIdLote);
            }

            #endregion
        }




        protected void OnSelectedIndexChanged_RadioFiltros(object sender, EventArgs e)
        {
            try
            {
                MostrarOcultarControlesOpciones();
                switch (this.radioFiltros.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        this.placeControlesOp2.Visible = true;
                        // si no tiene valor le ponemos por defecto la fecha del día
                        if (!FormUtils.HasValue(this.txtFechaDesdeOp2))
                        {
                            this.txtFechaDesdeOp2.Text = DateTime.Now.ToShortDateString();
                        }

                        if (!FormUtils.HasValue(this.txtFechaHastaOp2))
                        {
                            this.txtFechaHastaOp2.Text = DateTime.Now.ToShortDateString();
                        }
                        break;
                    //case 2:
                    //    this.placeControlesOp3.Visible = true;
                    //    if (!FormUtils.HasValue(this.txtFechaDesdeOp3))
                    //    {
                    //        this.txtFechaDesdeOp3.Text = DateTime.Now.ToShortDateString();
                    //    }
                    //    if (!FormUtils.HasValue(this.txtFechaHastaOp3))
                    //    {
                    //        this.txtFechaHastaOp3.Text = DateTime.Now.ToShortDateString();
                    //    }
                    //    break;
                    case 5:
                        this.placeControlesOp9.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void BtnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidatePage())
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_FILTROS_MENU_DERECHO, "");
                    VistaContratoCompletoDTO filtrosDTO = ObtenerFiltros();

                    if (filtrosDTO.FiltrosAvanzadosActivos == false)
                    {
                        filtrosDTO = new VistaContratoCompletoDTO();

                        AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                        UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                        if (usuarioDTO.NombreProveedor != null && usuarioDTO.NombreProveedor.Length > 0)
                        {
                            if (usuarioDTO.NombreProveedor.ToString() != "ADICO") { filtrosDTO.Proveedor = usuarioDTO.NombreProveedor.ToString(); }
                            filtrosDTO.ProveedorEntrada = usuarioDTO.NombreProveedor.ToString();
                        }
                        else
                        {
                            filtrosDTO.Proveedor = null;
                        }
                    }

                    filtrosDTO.BorrarFiltrosColumna();
                    CargarListado(filtrosDTO, 1, true);

                    if (this.lblNumEncontrados.Text != null && this.lblNumEncontrados.Text.Equals("0"))
                    {
                        this.ShowMessage("No se han encontrado datos para los filtros indicados.");
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void BuscarFiltroColumna_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidatePage())
                {
                    CargarListado(ObtenerFiltros(), 1, true);
                    if (this.lblNumEncontrados.Text != null && this.lblNumEncontrados.Text.Equals("0"))
                    {
                        this.ShowMessage("No se han encontrado datos para los filtros indicados.");
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnTxtIdLote_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (FormUtils.HasValue(txtIdLote))
                {
                    LoteDTO lote = new LoteDTO();
                    lote.Id = FormUtils.GetDecimal(txtIdLote);

                    if (lote.Id.HasValue)
                    {
                        if (Lotes.BuscarLoteID(lote))
                        {
                            txtIdLote.Text = lote.Id.Value.ToString();
                            txtDescripcionLote.Text = lote.Descripcion;
                        }
                        else
                        {
                            txtIdLote.Text = "";
                            txtDescripcionLote.Text = "";
                        }
                    }
                    else
                    {
                        txtIdLote.Text = "";
                        txtDescripcionLote.Text = "";
                    }
                }
                else
                {
                    txtIdLote.Text = "";
                    txtDescripcionLote.Text = "";
                }

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnTxtDescripcionLote_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (FormUtils.HasValue(txtDescripcionLote))
                {
                    LoteDTO lote = new LoteDTO();
                    lote.Descripcion = txtDescripcionLote.Text;

                    if (Lotes.BuscarLoteDescripcion(lote))
                    {
                        txtIdLote.Text = lote.Id.Value.ToString();
                        txtDescripcionLote.Text = lote.Descripcion;
                    }
                    else
                    {
                        txtIdLote.Text = "";
                        txtDescripcionLote.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnBtnVisitas_Click(object sender, EventArgs e)
        {
            try
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
                        this.ShowMessage("No se puede generar la visita porque no se han encontrado registros.");
                        return;
                    }

                    List<VisitaDTO> listaVisitas = CargarVisitas(ObtenerFiltros());
                    if (listaVisitas == null || listaVisitas.Count == 0)
                    {
                        this.ShowMessage("No hay visitas seleccionadas.");
                    }
                    else
                    {
                        CurrentSession.SetAttribute(SessionVariables.LISTA_VISITAS_GENERAR_LOTE, listaVisitas);
                        this.OpenWindow("./FrmModalGenerarVisitas.aspx", "generarLote", "Generar Lote");
                    }
                }
                catch (BaseException ex)
                {

                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        public static string Mid(string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            string result = param.Substring(startIndex, length);
            //return the result of the operation
            return result;
        }
        protected void OnGrdCabecera_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                    //e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");

                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        String columnName = null;
                        String strWidth = "80";
                        if (typeof(System.Web.UI.WebControls.BoundField) == grdListado.Columns[i].GetType())
                        {
                            columnName = ((System.Web.UI.WebControls.BoundField)(grdListado.Columns[i])).DataField;
                            ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla(grdListado.ID, columnName);
                            strWidth = (columnDefinition.Width).ToString();

                            e.Row.Cells[i].Attributes["style"] += "cursor:pointer;";
                            e.Row.Cells[i].ToolTip = formatString(e.Row.Cells[i].Text);
                            e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + e.Row.Cells[i].Text.Trim() + "</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        public void OnBtnRowClick_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.hdnClickedCellValue.Value != null)
                {
                    this.SaveSessionData();
                    NavigationController.Forward("./FrmDetalleContrato.aspx?COD_CONTRATO=" + this.hdnClickedCellValue.Value);
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
                    e.Row.Attributes.Add("OnMouseOver", "Resaltar_On1(this);");
                    e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");

                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        String columnName = null;
                        String columnValue = null;
                        String strWidth = "80";
                        if (typeof(System.Web.UI.WebControls.BoundField) == grdListado.Columns[i].GetType())
                        {
                            columnName = ((System.Web.UI.WebControls.BoundField)(grdListado.Columns[i])).DataField;
                            columnValue = e.Row.Cells[i].Text.Trim();
                            ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla(grdListado.ID, columnName);
                            strWidth = (columnDefinition.Width).ToString();


                            e.Row.Attributes["style"] += "value:" + e.Row.RowIndex + ";";//value" + i + ": " + e.Row.Cells[i].Text + ";";

                            e.Row.Cells[i].Attributes.Add("OnMouseOver", "ObtenerCelda(" + i + ");");

                            e.Row.Cells[i].Attributes["style"] += "cursor:pointer;value: " + e.Row.Cells[i].Text;
                            e.Row.Cells[i].ToolTip = formatString(e.Row.Cells[i].Text);

                            //FORMATO CELDA RECLAMACIÓN.
                            if (i == 48)
                            {
                                if (e.Row.Cells[i].Text == "True")
                                {
                                    e.Row.Cells[i].Text = "ABIERTA";
                                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                                    e.Row.Cells[i].BackColor = Color.Green;
                                }
                                else
                                {
                                    e.Row.Cells[i].Text = "CERRADA";
                                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                                    e.Row.Cells[i].BackColor = Color.Red;
                                }
                                e.Row.Cells[i].ForeColor = Color.White;
                            }
                            
                            //FORMATO CELDA CONTRATO.
                            if (columnName == "COD_CONTRATO_SIC")
                            {
                                e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'><a href='#' onclick='GridViewRowClick(\"" + columnValue + "\")'>" + columnValue + "</a></div>";
                            }
                            else
                            {
                                e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + columnValue + "</div>";
                            }
                            
                            //FORMATO CELDA TIPO URGENCIA.
                            if (columnName == "DESC_TIPO_URGENCIA")
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
                else if (e.Row.RowType == DataControlRowType.Header)
                {// Al tratarse de un objeto de la cabecera se le aniaden los botones de filtrado
                    GridView_Merge_Header_RowCreated(sender, e);
                }

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        /// <summary>
        /// Funcion que modifica la cabecera aniadiendo el boton del filtro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_Merge_Header_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ////Solo se tratan los objetos de tipo cabecera
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    VistaContratoCompletoDTO filtrosDTO = (VistaContratoCompletoDTO)CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO);

            //    //Contador que indicará la columna que se está procesando
            //    int contador = 0;

            //    //Recogemos cada celda de la cabecera
            //    foreach (TableCell tc in e.Row.Cells)
            //    {
            //        if (tc.HasControls())
            //        {
            //            // Se aniade un espacio y el boton a la cabecera
            //            tc.Controls.Add(new LiteralControl(" "));

            //            string imagen;

            //            if (filtrosDTO != null && filtrosDTO.EstaFiltradaColumna(contador))
            //            {// Columna con filtro
            //                imagen = "<img src ='../UI/HTML/Images/filtro2.gif' alt='Modificar Filtro'  style='border-style: none'>";
            //            }
            //            else
            //            {// Columna no aplicado ningun filtro
            //                imagen = "<img src ='../UI/HTML/Images/filtro.gif' alt='Asignar Filtro'  style='border-style: none'>";
            //            }

            //            // Se crea el boton (imagen con link asociado)
            //            LiteralControl lc = new LiteralControl("<a href='javascript:OnBtnFiltrosColumna_Click(" + contador + ")'>" + imagen + "</a>");
            //            tc.Controls.Add(lc);

            //            //Avanzamos en la columna
            //            contador++;
            //        }
            //    }
            //}
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
        #region eventos de validaciones
        protected void TxtFechaDesdeOp2CustomValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFechaDesdeOp2, true, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void TxtFechaHastaOp2CustomValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBoxTo(txtFechaDesdeOp2, txtFechaHastaOp2, true, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void TxtFechaDesdeOp3CustomValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFechaDesdeOp3, true, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void TxtFechaHastaOp3CustomValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBoxTo(txtFechaDesdeOp3, txtFechaHastaOp3, true, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void TxtIdLoteCustomValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateNumberTextBox(txtIdLote, false, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        #endregion
        #endregion

        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            if (CurrentSession.GetAttribute("FRM_CONTRATOS_radioFiltros") != null)
            {
                this.radioFiltros.SelectedIndex = (Int32)CurrentSession.GetAttribute("FRM_CONTRATOS_radioFiltros");
            }
            if (CurrentSession.GetAttribute("FRM_CONTRATOS_chkAplicarFiltrosAvanzados") != null)
            {
                this.chkAplicarFiltrosAvanzados.Checked = (Boolean)CurrentSession.GetAttribute("FRM_CONTRATOS_chkAplicarFiltrosAvanzados");
            }
            if (CurrentSession.GetAttribute("FRM_CONTRATOS_txtFechaDesdeOp2") != null)
            {
                this.txtFechaDesdeOp2.Text = (String)CurrentSession.GetAttribute("FRM_CONTRATOS_txtFechaDesdeOp2");
            }

            if (CurrentSession.GetAttribute("FRM_CONTRATOS_txtFechaDesdeOp3") != null)
            {
                this.txtFechaDesdeOp3.Text = (String)CurrentSession.GetAttribute("FRM_CONTRATOS_txtFechaDesdeOp3");
            }

            if (CurrentSession.GetAttribute("FRM_CONTRATOS_txtFechaHastaOp2") != null)
            {
                this.txtFechaHastaOp2.Text = (String)CurrentSession.GetAttribute("FRM_CONTRATOS_txtFechaHastaOp2");
            }

            if (CurrentSession.GetAttribute("FRM_CONTRATOS_txtFechaHastaOp3") != null)
            {
                this.txtFechaHastaOp3.Text = (String)CurrentSession.GetAttribute("FRM_CONTRATOS_txtFechaHastaOp3");
            }
            //if (CurrentSession.GetAttribute("FRM_CONTRATOS_cmbCodigoLote") != null)
            //{
            //    this.cmbCodigoLote.SelectedIndex = (Int32)CurrentSession.GetAttribute("FRM_CONTRATOS_cmbCodigoLote");
            //}
            if (CurrentSession.GetAttribute("FRM_CONTRATOS_cmbEstadoVisita") != null)
            {
                this.cmbEstadoVisita.SelectedIndex = (Int32)CurrentSession.GetAttribute("FRM_CONTRATOS_cmbEstadoVisita");
            }
            if (CurrentSession.GetAttribute("FRM_CONTRATOS_cmbTipoUrgencia") != null)
            {
                this.cmbTipoUrgencia.SelectedIndex = (Int32)CurrentSession.GetAttribute("FRM_CONTRATOS_cmbTipoUrgencia");
            }
            if (CurrentSession.GetAttribute("FRM_CONTRATOS_hdnPageIndex") != null)
            {
                this.hdnPageIndex.Value = (String)CurrentSession.GetAttribute("FRM_CONTRATOS_hdnPageIndex");
            }
            if (CurrentSession.GetAttribute("FRM_CONTRATOS_hdnSortColumn") != null)
            {
                this.hdnSortColumn.Value = (String)CurrentSession.GetAttribute("FRM_CONTRATOS_hdnSortColumn");
            }
            /*if (CurrentSession.GetAttribute("FRM_CONTRATOS_hdnRecargarUltimaConsulta") != null)
            {
                this.hdnRecargarUltimaConsulta.Value = (String)CurrentSession.GetAttribute("FRM_CONTRATOS_hdnRecargarUltimaConsulta");
            }*/
        }
        public override void SaveSessionData()
        {
            CurrentSession.SetAttribute("FRM_CONTRATOS_radioFiltros", this.radioFiltros.SelectedIndex);
            CurrentSession.SetAttribute("FRM_CONTRATOS_chkAplicarFiltrosAvanzados", this.chkAplicarFiltrosAvanzados.Checked);
            CurrentSession.SetAttribute("FRM_CONTRATOS_txtFechaDesdeOp2", this.txtFechaDesdeOp2.Text);
            CurrentSession.SetAttribute("FRM_CONTRATOS_txtFechaDesdeOp3", this.txtFechaDesdeOp3.Text);
            CurrentSession.SetAttribute("FRM_CONTRATOS_txtFechaHastaOp2", this.txtFechaHastaOp2.Text);
            CurrentSession.SetAttribute("FRM_CONTRATOS_txtFechaHastaOp3", this.txtFechaHastaOp3.Text);
            //CurrentSession.SetAttribute("FRM_CONTRATOS_cmbCodigoLote", this.cmbCodigoLote.SelectedIndex);
            CurrentSession.SetAttribute("FRM_CONTRATOS_cmbEstadoVisita", this.cmbEstadoVisita.SelectedIndex);
            CurrentSession.SetAttribute("FRM_CONTRATOS_cmbTipoUrgencia", this.cmbTipoUrgencia.SelectedIndex);
            CurrentSession.SetAttribute("FRM_CONTRATOS_hdnPageIndex", this.hdnPageIndex.Value);
            CurrentSession.SetAttribute("FRM_CONTRATOS_hdnSortColumn", this.hdnSortColumn.Value);
            //CurrentSession.SetAttribute("FRM_CONTRATOS_hdnRecargarUltimaConsulta", this.hdnRecargarUltimaConsulta.Value);


        }
        public override void DeleteSessionData()
        {
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_radioFiltros");
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_chkAplicarFiltrosAvanzados");
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_txtFechaDesdeOp2");
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_txtFechaDesdeOp3");
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_txtFechaHastaOp2");
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_txtFechaHastaOp3");
            //CurrentSession.RemoveAttribute("FRM_CONTRATOS_cmbCodigoLote");
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_cmbEstadoVisita");
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_cmbTipoUrgencia");
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_hdnPageIndex");
            CurrentSession.RemoveAttribute("FRM_CONTRATOS_hdnSortColumn");
            //CurrentSession.RemoveAttribute("FRM_CONTRATOS_hdnRecargarUltimaConsulta");
        }
        #endregion

        protected void DataGridCabecera_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                // si la columna por la que ordenamos es la misma, cambiamos la dirección de la ordenación
                if (this.hdnSortColumn.Value.Equals(e.SortExpression))
                {
                    if (this.hdnSortDirection.Value.Equals("ASC"))
                    {
                        this.hdnSortDirection.Value = "DESC";
                    }
                    else
                    {
                        this.hdnSortDirection.Value = "ASC";
                    }
                }
                else
                {
                    this.hdnSortDirection.Value = "ASC";
                }

                this.hdnSortColumn.Value = e.SortExpression;


                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                CargarListado(ObtenerFiltros(), intPageIndex, false);

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        #region funciones del menu contextual
            private void OrdenarAZ(String NombreColumna)
            {
                try
                {
                    // si la columna por la que ordenamos es la misma, cambiamos la dirección de la ordenación
                    //this.hdnSortDirection.Value = "ASC";
                    //this.hdnSortColumn.Value = NombreColumna;

                    Int32 intPageIndex = 1;// Int32.Parse(this.hdnPageIndex.Value);
                    //CargarListado(ObtenerFiltros(), intPageIndex, false);

                    //DataSet Datos = ((System.Data.DataTable)(this.grdListado.DataSource)).DataSet;
                    //DataView dvRecords = Datos.Tables[0].DefaultView;
                    //dvRecords.Sort = NombreColumna + " ASC";
                    //grdListado.DataSource = dvRecords;
                    //grdListado.DataBind();
                    String Filtros = "order by " + NombreColumna + " ASC";
                    CurrentSession.SetAttribute(CurrentSession.SESSION_FILTROS_MENU_DERECHO, Filtros);
                    //CargarNumeroRegistros();
                    CargarListado(ObtenerFiltros(), intPageIndex, true);
                }
                catch (Exception ex)
                {
                    ManageException(ex);
                }
            }

            private void OrdenarZA(String NombreColumna)
            {
                try
                {
                    // si la columna por la que ordenamos es la misma, cambiamos la dirección de la ordenación
                    Int32 intPageIndex = 1;// Int32.Parse(this.hdnPageIndex.Value);
                    //CargarListado(ObtenerFiltros(), intPageIndex, false);

                    //DataSet Datos = ((System.Data.DataTable)(this.grdListado.DataSource)).DataSet;
                    //DataView dvRecords = Datos.Tables[0].DefaultView;
                    //dvRecords.Sort = NombreColumna + " DESC";
                    String Filtros = "order by " + NombreColumna + " DESC";
                    
                    //grdListado.DataSource = dvRecords;
                    //grdListado.DataBind();

                    CurrentSession.SetAttribute(CurrentSession.SESSION_FILTROS_MENU_DERECHO, Filtros);
                    //CargarNumeroRegistros(); 
                    CargarListado(ObtenerFiltros(), intPageIndex, true);

                }
                catch (Exception ex)
                {
                    ManageException(ex);
                }
            }

            private void IgualA(String Valor,String Columna)
            {
                try
                {
                    Filtro = true;
                    Int32 intPageIndex = 1;// Int32.Parse(this.hdnPageIndex.Value);
                    String Filtros = "";

                    //DataSet Datos = ((System.Data.DataTable)(this.grdListado.DataSource)).DataSet;

                    //DataView dvRecords = Datos.Tables[0].DefaultView;
                    if (Columna.IndexOf("FEC") >= 0)
                    {
                        if (Valor != " ")
                        {
                            DateTime Fecha = DateTime.ParseExact(Valor, "dd/MM/yyyy", null);
                            String Condicion_Fecha = "";
                            Condicion_Fecha = " (" + Columna.ToString() + "='" + Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString() + "')";// +" and (" + Columna + "<#" + Fecha.AddDays(1).Month.ToString() + "/" + Fecha.AddDays(1).Day.ToString() + "/" + Fecha.AddDays(1).Year.ToString() + "#)";
                            //dvRecords.RowFilter = Condicion_Fecha;
                            Filtros = Condicion_Fecha;
                        }
                    }
                    else
                    {
                        if (Valor == " ")
                        {
                            //dvRecords.RowFilter = Columna + " is null";// "DESC_TIPO_URGENCIA LIKE 'VENCIDO'";
                            Filtros = Columna + " is null";
                        }
                        else
                        {
                            //dvRecords.RowFilter = Columna + " = '" + Valor + "'";// "DESC_TIPO_URGENCIA LIKE 'VENCIDO'";
                            Filtros = Columna + " = '" + Valor + "'";
                        }
                    }

                    //grdListado.DataSource = dvRecords;
                    //grdListado.DataBind();
                    CurrentSession.SetAttribute(CurrentSession.SESSION_FILTROS_MENU_DERECHO, Filtros);
                    //CargarNumeroRegistros(); 
                    CargarListado(ObtenerFiltros(), intPageIndex, true);

                }
                catch (Exception ex)
                {
                    ManageException(ex);
                }
            }

            private void DiferenteA(String Valor, String Columna)
            {
                try
                {
                    Filtro = true;
                    Int32 intPageIndex = 1;
                    //CargarListado(ObtenerFiltros(), intPageIndex, false);
                    String FiltroMenu = "";
                    //DataSet Datos = ((System.Data.DataTable)(this.grdListado.DataSource)).DataSet;

                    //DataView dvRecords = Datos.Tables[0].DefaultView;
                    if (Columna.IndexOf("FEC") >= 0)
                    {
                        if (Valor != " ")
                        {
                            DateTime Fecha = DateTime.ParseExact(Valor, "dd/MM/yyyy", null);
                            String Condicion_Fecha = "";
                            Condicion_Fecha = " ((" + Columna.ToString() + "<'" + Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString() + "')" + " or (" + Columna + ">'" + Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString() + "'))";
                            //dvRecords.RowFilter = Condicion_Fecha;
                            FiltroMenu = Condicion_Fecha;
                        }
                    }
                    else
                    {
                        if (Valor == " ")
                        {
                            //dvRecords.RowFilter = Columna + " is not null";// "DESC_TIPO_URGENCIA LIKE 'VENCIDO'";
                            FiltroMenu =Columna + " is not null";;
                        }
                        else
                        {
                            //dvRecords.RowFilter = Columna + " <> '" + Valor + "' or " + Columna + " is null";// "DESC_TIPO_URGENCIA LIKE 'VENCIDO'";
                            FiltroMenu = "(" + Columna + " <> '" + Valor + "' or " + Columna + " is null)";
                        }
                    }

                    //grdListado.DataSource = dvRecords;
                    //grdListado.DataBind();
                    CurrentSession.SetAttribute(CurrentSession.SESSION_FILTROS_MENU_DERECHO, FiltroMenu);
                    //CargarNumeroRegistros(); 
                    CargarListado(ObtenerFiltros(), intPageIndex, true);

                }
                catch (Exception ex)
                {
                    ManageException(ex);
                }
            }

            private void EnAntesDe(String Valor, String Columna)
            {
                try
                {
                    Filtro = true;
                    Int32 intPageIndex = 1;// Int32.Parse(this.hdnPageIndex.Value);
                    //CargarListado(ObtenerFiltros(), intPageIndex, false);
                    String FiltroMenu = "";
                    //DataSet Datos = ((System.Data.DataTable)(this.grdListado.DataSource)).DataSet;

                    //DataView dvRecords = Datos.Tables[0].DefaultView;
                    if (Columna.IndexOf("FEC") >= 0)
                    {
                        if (Valor != " ")
                        {
                            DateTime Fecha = DateTime.ParseExact(Valor, "dd/MM/yyyy", null);
                            String Condicion_Fecha = "";
                            Condicion_Fecha = " ((" + Columna.ToString() + "<'" + Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString() + "')" + " or (" + Columna + "='" + Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString() + "'))";
                            //dvRecords.RowFilter = Condicion_Fecha;
                            FiltroMenu = Condicion_Fecha;
                        }
                    }
                    ////else
                    ////{
                    ////    if (Valor == " ")
                    ////    {
                    ////        dvRecords.RowFilter = Columna + " is not null";// "DESC_TIPO_URGENCIA LIKE 'VENCIDO'";
                    ////    }
                    ////    else
                    ////    {
                    ////        dvRecords.RowFilter = Columna + " <> '" + Valor + "' or " + Columna + " is null";// "DESC_TIPO_URGENCIA LIKE 'VENCIDO'";
                    ////    }
                    ////}

                    //grdListado.DataSource = dvRecords;
                    //grdListado.DataBind();
                    CurrentSession.SetAttribute(CurrentSession.SESSION_FILTROS_MENU_DERECHO, FiltroMenu);
                    //CargarNumeroRegistros(); 
                    CargarListado(ObtenerFiltros(), intPageIndex, true);

                }
                catch (Exception ex)
                {
                    ManageException(ex);
                }
            }

            private void EnDespuesDe(String Valor, String Columna)
            {
                try
                {
                    Filtro = true;
                    Int32 intPageIndex = 1;// Int32.Parse(this.hdnPageIndex.Value);
                    //CargarListado(ObtenerFiltros(), intPageIndex, false);
                    String FiltroMenu = "";

                    //DataSet Datos = ((System.Data.DataTable)(this.grdListado.DataSource)).DataSet;

                    //DataView dvRecords = Datos.Tables[0].DefaultView;
                    if (Columna.IndexOf("FEC") >= 0)
                    {
                        if (Valor != " ")
                        {
                            DateTime Fecha = DateTime.ParseExact(Valor, "dd/MM/yyyy", null);
                            String Condicion_Fecha = "";
                            Condicion_Fecha = " ((" + Columna.ToString() + "='" + Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString() + "')" + " or (" + Columna + ">'" + Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString() + "'))";
                            //dvRecords.RowFilter = Condicion_Fecha;
                            FiltroMenu = Condicion_Fecha;
                        }
                    }
                    ////else
                    ////{
                    ////    if (Valor == " ")
                    ////    {
                    ////        dvRecords.RowFilter = Columna + " is not null";// "DESC_TIPO_URGENCIA LIKE 'VENCIDO'";
                    ////    }
                    ////    else
                    ////    {
                    ////        dvRecords.RowFilter = Columna + " <> '" + Valor + "' or " + Columna + " is null";// "DESC_TIPO_URGENCIA LIKE 'VENCIDO'";
                    ////    }
                    ////}

                    //grdListado.DataSource = dvRecords;
                    //grdListado.DataBind();
                    CurrentSession.SetAttribute(CurrentSession.SESSION_FILTROS_MENU_DERECHO, FiltroMenu);
                    //CargarNumeroRegistros(); 
                    CargarListado(ObtenerFiltros(), intPageIndex,true);

                }
                catch (Exception ex)
                {
                    ManageException(ex);
                }
            }

            private void QuitarFiltro()
            {
                try
                {
                    Filtro = true;
                    Int32 intPageIndex = 1;// Int32.Parse(this.hdnPageIndex.Value);
                    CurrentSession.SetAttribute(CurrentSession.SESSION_FILTROS_MENU_DERECHO, "");
                    //CargarNumeroRegistros(); 
                    CargarListado(ObtenerFiltros(), intPageIndex, true);

                    

                    //CargarListado(ObtenerFiltros(), intPageIndex, false);

                    //DataSet Datos = ((System.Data.DataTable)(this.grdListado.DataSource)).DataSet;

                    //grdListado.DataSource = Datos;
                    //grdListado.DataBind();

                }
                catch (Exception ex)
                {
                    ManageException(ex);
                }
            }

        #endregion

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            int NumeroColumna = int.Parse(this.HDMenuSeleccionado.Value.Substring(2, this.HDMenuSeleccionado.Value.Length - 2));
            //this.grdListado.Columns[NumeroColumna]
            switch (this.HDMenuSeleccionado.Value.Substring (0,1))
            {
                case "1":
                    OrdenarAZ(NombreColumnas[NumeroColumna]);
                    break;
                case "2":
                    OrdenarZA(NombreColumnas[NumeroColumna]);
                    break;
                case "3":
                    //Igual A...
                    IgualA(this.HDValorSeleccionado.Value, NombreColumnas[NumeroColumna]);
                    break;
                case "4":
                    //NO igual A...
                    DiferenteA(this.HDValorSeleccionado.Value, NombreColumnas[NumeroColumna]);
                    break;
                case "5":
                    //En o antes de...
                    EnAntesDe(this.HDValorSeleccionado.Value, NombreColumnas[NumeroColumna]);
                    break;
                case "6":
                    //En o despues de...
                    EnDespuesDe(this.HDValorSeleccionado.Value, NombreColumnas[NumeroColumna]);
                    break;
                case "7":
                    //Quitar Filtro.
                    QuitarFiltro();
                    break;
            }  
        }
    }
}
