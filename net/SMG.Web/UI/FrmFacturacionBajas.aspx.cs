using Iberdrola.Commons.Security;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.UI;

public partial class UI_FrmNoFacturarBaja : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnProcesar_Click(object sender, EventArgs e)
    {
        string codContrato = txtContrato.Text.Trim();
        try
        {
            Contrato contrato = new Contrato();
            bool facturado = contrato.FacturacionBajasRegularizacion(codContrato);
            if (facturado)
            {
                MostrarMensaje("La factura del contrato " + codContrato + " ya ha sido emitida.");
            }
            else
            {
                contrato.InsertFacturacionBajasRegularizadas(codContrato, "FrmNoFacturarBaja");
                txtContrato.Text = string.Empty;
                MostrarMensaje("El contrato " + codContrato + " ha sido procesado.");
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message);
        }
    }

    protected void btnProcesarFichero_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(fuFicheroBajas.FileName))
        {
            if (!File.Exists(Server.MapPath("~/Excel") + "\\" + fuFicheroBajas.FileName))
            {
                fuFicheroBajas.SaveAs(Server.MapPath("~/Excel") + "\\" + fuFicheroBajas.FileName);
            }
            ProcesarFichero(fuFicheroBajas.FileName);

            File.Delete(Server.MapPath("~/Excel") + "\\" + fuFicheroBajas.FileName);
        }
        else
        {
            MostrarMensaje("No se ha seleccionado ningun fichero.");
        }
    }

    protected void ProcesarFichero(String NombreArchivo)
    {
        List<string> lContratos = new List<string>();
        try
        {
            string texto = String.Empty;
            String ruta = Server.MapPath("~/Excel") + "\\" + NombreArchivo;
            FileInfo fNewFile = new FileInfo(ruta);
            using (ExcelPackage excelPackage = new ExcelPackage(fNewFile))
            {
                ExcelWorksheet hojaEs = excelPackage.Workbook.Worksheets["España"];
                if (hojaEs != null)
                {
                    for (int i = 1; i <= hojaEs.Dimension.Rows; i++)
                    {
                        texto = hojaEs.Cells[i, 1].Text;
                        if ((!String.IsNullOrEmpty(texto)) && (!texto.ToUpper().Trim().Equals("CONTRATO")))
                        {
                            lContratos.Add(texto);
                        }
                    }
                }
                ExcelWorksheet hojaPt = excelPackage.Workbook.Worksheets["Portugal"];
                if (hojaPt != null)
                {
                    for (int i = 1; i <= hojaPt.Dimension.Rows; i++)
                    {
                        texto = hojaPt.Cells[i, 1].Text;
                        if ((!String.IsNullOrEmpty(texto)) && (!texto.ToUpper().Trim().Equals("CONTRATO")))
                        {
                            lContratos.Add(texto);
                        }
                    }
                }
                excelPackage.Dispose();
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message);
        }

        try
        {
            DataTable dtProcesados = new DataTable();
            dtProcesados.Columns.Add("Contrato");
            dtProcesados.Columns.Add("Procesado");
            foreach (string codContrato in lContratos)
            {
                Contrato contrato = new Contrato();
                bool facturado = contrato.FacturacionBajasRegularizacion(codContrato);
                if (facturado)
                {
                    dtProcesados.Rows.Add(new Object[] { codContrato, "No" });
                }
                else
                {
                    contrato.InsertFacturacionBajasRegularizadas(codContrato, "FrmNoFacturarBaja");
                    dtProcesados.Rows.Add(new Object[] { codContrato, "Si" });
                }
            }

            gvProceso.DataSource = dtProcesados;
            gvProceso.DataBind();
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message);
        }
        MostrarMensaje("Proceso Completado");
    }

    private void MostrarMensaje(String Mensaje)
    {
        string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", alerta, false);
    }

    //BGN ADD BEG 20/10/2020: [Redmine:#24255] Permitir modificar carencia desde la web
    protected void btnModificarCarencia_Click(object sender, EventArgs e)
    {
        string codContrato = txtContratoCarencia.Text.Trim();
        try
        {
            MantenimientoDB mtoDB = new MantenimientoDB();
            Boolean carencia = mtoDB.GetCarenciaPorContrato(codContrato);
            if (carencia == chk_Carencia.Checked)
            {
                MostrarMensaje("La carencia del contrato " + codContrato + " no se ha modificado porque ya era la indicada.");
            }
            else
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                
                long resultado = mtoDB.ActualizarCarenciaContrato(codContrato, chk_Carencia.Checked, usuarioDTO.Login);
                if (resultado == 0)
                {
                    MostrarMensaje("La carencia del contrato " + codContrato + " ha sido modificada.");
                }
                else
                {
                    MostrarMensaje("La carencia del contrato " + codContrato + " no se ha modificado debido a un error inesperado.");
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message);
        }
    }
    //BGN ADD END 20/10/2020: [Redmine:#24255] Permitir modificar carencia desde la web

}