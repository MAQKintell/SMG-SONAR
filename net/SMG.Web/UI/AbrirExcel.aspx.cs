using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Iberdrola.Commons.Reporting;
using Iberdrola.Commons.Web;

using System.Text;
using System.IO;


using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.UI
{
    public partial class AbrirExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            if (!Page.IsPostBack)
            {
                //SESSION_DATAREADER_EXCEL
                var prueba=CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_EXCEL);
                ExcelHelper excel = (ExcelHelper)CurrentSession.GetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS);
                if (Request.QueryString["tipo"] != null)
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "REPARACIONES");
                }

                if (excel != null)
                {
                    if (excel.Tittle == "NO")
                    {
                        this.Button1.Visible = false;
                        var Limite = CurrentSession.GetAttribute(CurrentSession.SESSION_LIMITE_DATOS_EXCEL);
                        if (Limite == "NO")
                        {
                            MostrarMensaje(Resources.TextosJavaScript.TEXTO_EXCEL_MAS_DE_10000);//"No se puede generar el documento Excel con más de 10000 registros porque tardaría demasiado.");
                            this.btn_exportar0.Enabled = false;
                            this.Button1.Enabled = false;
                            return;
                        }
                    }
                }

                if (prueba == "NO")
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_EXCEL_DEMASIADAS_FILAS + Resources.SMGConfiguration.ExcelMaxRows);//"No se puede generar el documento Excel con más de " + Resources.SMGConfiguration.ExcelMaxRows + " registros porque tardaría demasiado.");
                    this.btn_exportar0.Enabled = false;
                    this.Button1.Enabled = false;
                    return;
                }
             }
        }

        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
        }

        protected string ExportarDatagrid(DataTable dtSolicitudes,String Titulo)
        {
            //***************
            String nombreExcel = "Solicitudes_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xls";
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            DataGrid a = new DataGrid();
            Page Page = new Page();
            HtmlForm form = new HtmlForm();

            
            a.HeaderStyle.BackColor = System.Drawing.Color.Green;
            a.HeaderStyle.ForeColor = System.Drawing.Color.White;
            a.HeaderStyle.Font.Bold = true;
            a.DataSource = dtSolicitudes;
            a.DataBind();

            a.RenderControl(htw);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=ficheroFormularios.xls");
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            HttpContext.Current.Response.Write(Titulo);
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();

            return nombreExcel;
        }
        protected void btn_exportar0_Click(object sender, EventArgs e)
        {
            String titulo = (String)CurrentSession.GetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL);
            CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "");
            if (titulo == "" || titulo==null) { titulo = "MANTENIMIENTO"; }

            String ruta = "";
            if (titulo == "REPARACIONES") { ruta = ExportarDatagrid((DataTable)CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_EXCEL_REPARACIONES), titulo); }
            else { ruta = ExportarDatagrid((DataTable)CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_EXCEL), titulo); }

            ////lnk_Excel.OnClientClick = "javascript:window.open('excel/" + ruta + "');OcultarCapaEspera();return false;";
            //lnk_Excel.OnClientClick = "OcultarCapaEspera();";
            ////Response.Write("<script language='javascript'>window.open('excel/" & rutaExcel & "');</script>")
            //lnk_Excel.Text = ruta;
        }


        
        protected void Button1_Click(object sender, EventArgs e)
        {
            // si hemos llegado a este punto es que podemos generar el Excel
            ExcelHelper excel = (ExcelHelper)CurrentSession.GetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS);
            //ExcelHeaderAttribute headerAttrib = new ExcelHeaderAttribute("Fecha", DateTime.Now.ToShortDateString());
            //excel.Attributtes.Add(headerAttrib);
            //VistaContratoCompletoDTO filtrosDTO = 
            //AniadirAtributosExcel(excel, filtrosDTO);

            //excel.TableName = "Mantenimiento";
            if (excel.Tittle != "NO")
            {
                DataTable Datos = (DataTable)CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_EXCEL);//(IDataReader)CurrentSession.GetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL);
                excel.LoadData(Datos);

                excel.GenerateExcel(Response);
            }
            else
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_FORMATO_NO_AUTORIZADO);//"FORMATO NO AUTORIZADO EN ESTE CASO");
            }
            
            

            //ClientScript.RegisterStartupScript(Page.GetType(), "CerrarExcel", "<script language='javascript'>Alert('AAAA');</script>");

        }

        
        //private void AniadirAtributosExcel(ExcelHelper excel, VistaContratoCompletoDTO filtrosDTO)
        //{
        //    #region Area radiobuttons
        //    switch (this.radioFiltros.SelectedIndex)
        //    {
        //        case 0:
        //            ExcelHeaderAttribute headerAttribSinFiltrar = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
        //            excel.Attributtes.Add(headerAttribSinFiltrar);
        //            break;
        //        case 1:
        //            ExcelHeaderAttribute headerAttribUltimaVisita = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
        //            ExcelHeaderAttribute headerAttribUltimaVisitaFechaDesdeOp2 = new ExcelHeaderAttribute("Fecha desde: ", this.txtFechaDesdeOp2.Text);
        //            ExcelHeaderAttribute headerAttribUltimaVisitaFechaHastaOp2 = new ExcelHeaderAttribute("Fecha hasta: ", this.txtFechaHastaOp2.Text);
        //            excel.Attributtes.Add(headerAttribUltimaVisita);
        //            excel.Attributtes.Add(headerAttribUltimaVisitaFechaDesdeOp2);
        //            excel.Attributtes.Add(headerAttribUltimaVisitaFechaHastaOp2);
        //            break;
        //        case 2:
        //            ExcelHeaderAttribute headerAttribFacturadas = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
        //            ExcelHeaderAttribute headerAttribUltimaVisitaFechaDesdeOp3 = new ExcelHeaderAttribute("Fecha desde: ", this.txtFechaDesdeOp3.Text);
        //            ExcelHeaderAttribute headerAttribUltimaVisitaFechaHastaOp3 = new ExcelHeaderAttribute("Fecha hasta: ", this.txtFechaHastaOp3.Text);
        //            excel.Attributtes.Add(headerAttribFacturadas);
        //            excel.Attributtes.Add(headerAttribUltimaVisitaFechaDesdeOp3);
        //            excel.Attributtes.Add(headerAttribUltimaVisitaFechaHastaOp3);
        //            break;
        //        case 3:
        //            ExcelHeaderAttribute headerAttribEstadoVisita = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
        //            ExcelHeaderAttribute headerAttribEstadoDeVisita = new ExcelHeaderAttribute("Estado de la visita: ", this.cmbEstadoVisita.SelectedItem.Text);
        //            excel.Attributtes.Add(headerAttribEstadoVisita);
        //            excel.Attributtes.Add(headerAttribEstadoDeVisita);
        //            break;
        //        case 4:
        //            ExcelHeaderAttribute headerAttribGasCalefaccion = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
        //            excel.Attributtes.Add(headerAttribGasCalefaccion);
        //            break;
        //        case 5:
        //            ExcelHeaderAttribute headerAttribGas = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
        //            excel.Attributtes.Add(headerAttribGas);
        //            break;
        //        case 6:
        //            ExcelHeaderAttribute headerAttribLote = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
        //            ExcelHeaderAttribute headerAttribIdLote = new ExcelHeaderAttribute("Número del lote: ", this.txtIdLote.Text);
        //            excel.Attributtes.Add(headerAttribLote);
        //            excel.Attributtes.Add(headerAttribIdLote);
        //            if (this.txtDescripcionLote.Text != "")
        //            {
        //                ExcelHeaderAttribute headerAttribDescripcionLote = new ExcelHeaderAttribute("Descripción: ", this.txtDescripcionLote.Text);
        //                excel.Attributtes.Add(headerAttribDescripcionLote);
        //            }
        //            break;
        //        case 7:
        //            ExcelHeaderAttribute headerAttribUrgencia = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
        //            ExcelHeaderAttribute headerAttribTipoUrgencia = new ExcelHeaderAttribute("Tipo de la urgencia: ", this.cmbTipoUrgencia.SelectedItem.Text);
        //            excel.Attributtes.Add(headerAttribUrgencia);
        //            excel.Attributtes.Add(headerAttribTipoUrgencia);
        //            break;
        //        case 8:
        //            ExcelHeaderAttribute headerAttribAplazadasOEjecucion = new ExcelHeaderAttribute("Filtro: ", this.radioFiltros.SelectedItem.Text);
        //            excel.Attributtes.Add(headerAttribAplazadasOEjecucion);
        //            break;
        //    }

        //    #endregion

        //    #region Area Filtros Avanzados


        //    if (filtrosDTO.CodigoContrato != null && filtrosDTO.CodigoContrato != "")
        //    {
        //        ExcelHeaderAttribute headerAttribAvanzadosCodContrato = new ExcelHeaderAttribute("Código del contrato: ", filtrosDTO.CodigoContrato);
        //        excel.Attributtes.Add(headerAttribAvanzadosCodContrato);
        //    }
        //    if (filtrosDTO.Nombre != null && filtrosDTO.Nombre != "")
        //    {
        //        ExcelHeaderAttribute headerAttribAvanzadosNombre = new ExcelHeaderAttribute("Nombre: ", filtrosDTO.Nombre);
        //        excel.Attributtes.Add(headerAttribAvanzadosNombre);
        //    }
        //    if (filtrosDTO.Apellido1 != null && filtrosDTO.Apellido1 != "")
        //    {
        //        ExcelHeaderAttribute headerAttribAvanzadosApellido1 = new ExcelHeaderAttribute("1º Apellido: ", filtrosDTO.Apellido1);
        //        excel.Attributtes.Add(headerAttribAvanzadosApellido1);
        //    }
        //    if (filtrosDTO.Apellido2 != null && filtrosDTO.Apellido2 != "")
        //    {
        //        ExcelHeaderAttribute headerAttribAvanzadosApellido2 = new ExcelHeaderAttribute("2º Apellido: ", filtrosDTO.Apellido2);
        //        excel.Attributtes.Add(headerAttribAvanzadosApellido2);
        //    }
        //    if (filtrosDTO.CodigoDeProvincia != null && filtrosDTO.CodigoDeProvincia != "")
        //    {
        //        Contrato contrato = new Contrato();
        //        IDataReader dr = contrato.obtenerFiltrosAvanzadosExcelProvincia(filtrosDTO);
        //        dr.Read();
        //        ExcelHeaderAttribute headerAttribAvanzadosProvincia = new ExcelHeaderAttribute("Provincia: ", (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE"));
        //        excel.Attributtes.Add(headerAttribAvanzadosProvincia);
        //    }
        //    if (filtrosDTO.CodigoDePoblacion != null && filtrosDTO.CodigoDePoblacion != "")
        //    {
        //        Contrato contrato = new Contrato();
        //        IDataReader dr = contrato.obtenerFiltrosAvanzadosExcelPoblacion(filtrosDTO);
        //        dr.Read();
        //        ExcelHeaderAttribute headerAttribAvanzadosPoblacion = new ExcelHeaderAttribute("Poblacion: ", (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE"));
        //        excel.Attributtes.Add(headerAttribAvanzadosPoblacion);
        //    }
        //    if (filtrosDTO.CodigoPostal != null && filtrosDTO.CodigoPostal != "")
        //    {
        //        ExcelHeaderAttribute headerAttribAvanzadosCodPostal = new ExcelHeaderAttribute("Código postal: ", filtrosDTO.CodigoPostal);
        //        excel.Attributtes.Add(headerAttribAvanzadosCodPostal);
        //    }
        //    if (filtrosDTO.DescripcionTipoUrgencia != null && filtrosDTO.DescripcionTipoUrgencia != "")
        //    {
        //        ExcelHeaderAttribute headerAttribAvanzadosDescripcionTipoUrgencia = new ExcelHeaderAttribute("Descripcion de la urgencia: ", filtrosDTO.DescripcionTipoUrgencia);
        //        excel.Attributtes.Add(headerAttribAvanzadosDescripcionTipoUrgencia);
        //    }
        //    if (filtrosDTO.Campania != null)
        //    {
        //        ExcelHeaderAttribute headerAttribAvanzadosCampania = new ExcelHeaderAttribute("Campaña: ", Convert.ToString(filtrosDTO.Campania));
        //        excel.Attributtes.Add(headerAttribAvanzadosCampania);
        //    }

        //    if (filtrosDTO.IdLote != null)
        //    {
        //        ExcelHeaderAttribute headerAttribAvanzadosIdLote = new ExcelHeaderAttribute("Código del lote: ", Convert.ToString(filtrosDTO.IdLote));
        //        excel.Attributtes.Add(headerAttribAvanzadosIdLote);
        //    }

        //    #endregion
        //}
        
}
}
