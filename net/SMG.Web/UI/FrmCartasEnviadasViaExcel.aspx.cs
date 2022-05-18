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
using System.IO;

using System.Configuration;
using System.Linq;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.Common;
using OfficeOpenXml;

namespace Iberdrola.SMG.UI
{
    public partial class FrmCartasEnviadasViaExcel : FrmBaseListado
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            //if (!IsPostBack) { Leer("CartasEnviadas.xlsx"); }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Kintell 20052020.
                ConfiguracionDTO rutaCartasEnviadas = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_CARTASENVIADAS);
                string ruta = Server.MapPath("~/Excel");
                if (rutaCartasEnviadas != null && !string.IsNullOrEmpty(rutaCartasEnviadas.Valor) && rutaCartasEnviadas.Valor != "...")
                {
                    ruta = rutaCartasEnviadas.Valor;//@"\\172.20.46.45\ADMON_PYS\PRODUCTOS Y SERVICIOS\08 SMG\";
                }

                ConfiguracionDTO confImpersonatorActivado = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.IMPERSONATOR_ACTIVO);

                if (confImpersonatorActivado != null && !string.IsNullOrEmpty(confImpersonatorActivado.Valor) && Convert.ToBoolean(confImpersonatorActivado.Valor) == true)
                {
                    using (ManejadorFicheros.GetImpersonator())
                    {
                        
                        //ruta = Server.MapPath("~/Excel");
                        if (!File.Exists(ruta + "\\" + FileUpload1.FileName))
                        {
                            FileUpload1.SaveAs(ruta + "\\" + FileUpload1.FileName);
                        }
                        Leer(FileUpload1.FileName);

                        //if (chkBorrar.Checked == true) 
                        //{ 
                        File.Delete(ruta + "\\" + FileUpload1.FileName);
                        //}
                    }
                }
                else
                {
                    //string ruta = @"\\172.20.46.45\ADMON_PYS\PRODUCTOS Y SERVICIOS\08 SMG\";
                    //ruta = Server.MapPath("~/Excel");
                    if (!File.Exists(ruta + "\\" + FileUpload1.FileName))
                    {
                        FileUpload1.SaveAs(ruta + "\\" + FileUpload1.FileName);
                    }
                    Leer(FileUpload1.FileName);

                    //if (chkBorrar.Checked == true) 
                    //{ 
                    File.Delete(ruta + "\\" + FileUpload1.FileName);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message + "_" + ex.StackTrace);
            }

        }

        protected void Leer(String NombreArchivo)
        {
            try
            {
                //String ruta = Server.MapPath("~/Excel") + "\\" + NombreArchivo;
                ConfiguracionDTO rutaCartasEnviadas = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_CARTASENVIADAS);
                string ruta1 = Server.MapPath("~/Excel");
                if (rutaCartasEnviadas != null && !string.IsNullOrEmpty(rutaCartasEnviadas.Valor) && rutaCartasEnviadas.Valor != "...")
                {
                    ruta1 = rutaCartasEnviadas.Valor;//@"\\172.20.46.45\ADMON_PYS\PRODUCTOS Y SERVICIOS\08 SMG\";
                }

                //string ruta1 = @"\\172.20.46.45\ADMON_PYS\PRODUCTOS Y SERVICIOS\08 SMG\";
                String ruta = ruta1 + "\\" + NombreArchivo;

                FileInfo fNewFile = new FileInfo(ruta);
                using (ExcelPackage excelPackage = new ExcelPackage(fNewFile))
                {
                    ExcelWorksheet hojaEs = excelPackage.Workbook.Worksheets["Hoja1"];
                    if (hojaEs != null)
                    {
                        for (int i = 1; i <= hojaEs.Dimension.Rows; i++)
                        {
                            VisitasDB visitas = new VisitasDB();
                            visitas.ActualizarEnvioCarta(hojaEs.Cells[i, 1].Text, Int16.Parse(hojaEs.Cells[i, 2].Text), DateTime.Parse(hojaEs.Cells[i, 3].Text));
                        }
                    }
                    excelPackage.Dispose();
                 }
                 MostrarMensaje("Proceso Completado");
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message);
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

        //20191119 BGN ADD BEG R#20616: Procedimientos cancelados no localizables
        protected void OnBtnPaginacion_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fecDesde = Convert.ToDateTime(txtFechaDesde.Text);
                DateTime fecHasta = Convert.ToDateTime(txtFechaHasta.Text);

                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                Int32 Desde = (PageSize * (intPageIndex - 1));
                Int32 Hasta = PageSize * intPageIndex;


                CargarPaginador();
                Visitas visitas = new Visitas();
                dgDatos.DataSource = visitas.ObtenerCartasEnviadas(fecDesde, fecHasta, Desde, Hasta);
                dgDatos.DataBind();

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        //20191119 BGN ADD END R#20616: Procedimientos cancelados no localizables


        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", alerta, false);
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

        //20191119 BGN MOD BEG R#20616: Procedimientos cancelados no localizables
        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fecDesde = Convert.ToDateTime(txtFechaDesde.Text);
                DateTime fecHasta = Convert.ToDateTime(txtFechaHasta.Text);

                Visitas visitas = new Visitas();
                Int32 numReg = visitas.ObtenerNumCartasEnviadas(fecDesde, fecHasta);
                this.lblNumEncontrados.Text = numReg.ToString();

                this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                this.hdnPageIndex.Value = "1";

                CargarPaginador();

                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                Int32 Desde = (PageSize * (intPageIndex - 1));
                Int32 Hasta = PageSize * intPageIndex;
                
                dgDatos.DataSource = visitas.ObtenerCartasEnviadas(fecDesde, fecHasta, Desde, Hasta);
                dgDatos.DataBind();


                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, visitas.ObtenerCartasEnviadasEnDataTable(fecDesde, fecHasta, 1, numReg+1));
                CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "Cartas a enviar");
                ExcelHelper excel = new ExcelHelper("Cartas a enviar", HttpContext.Current.Server.MapPath(Resources.SMGConfiguration.XsltConsultas));
                ExcelHeaderAttribute headerAttrib = new ExcelHeaderAttribute("Nombre", DateTime.Now.ToShortDateString());
                excel.TableName = dgDatos.ID;
                excel.Attributtes.Add(headerAttrib);
                CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message);
            }
        }
        //20191119 BGN MOD END R#20616: Procedimientos cancelados no localizables

    }
}