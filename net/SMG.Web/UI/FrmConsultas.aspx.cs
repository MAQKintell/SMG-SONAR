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
    public partial class FrmConsultas : FrmBase
    {
        #region constructores
        public FrmConsultas()
        {
        }
        #endregion 

        #region métodos privados
        private void CargarListadoConsultas(VistaContratoCompletoDTO ConsultaFiltro)
        {
            IDataReader dr = null;
            String strTituloInforme = "";
            String nombreConfiguracionInforme = "";

            List<ExcelHeaderAttribute> listaAtributos = new List<ExcelHeaderAttribute>();
            ExcelHeaderAttribute headerAttribFecha = new ExcelHeaderAttribute("Fecha Informe", DateTime.Now.ToShortDateString());
            ExcelHeaderAttribute headerAttribProveedor = new ExcelHeaderAttribute("Provedor:", ConsultaFiltro.Proveedor);
            
            listaAtributos.Add(headerAttribFecha);
            listaAtributos.Add(headerAttribProveedor);
            
            if(RadioConsultas.SelectedIndex == 0)
            {
                ExcelHeaderAttribute headerAttribCodContrato = new ExcelHeaderAttribute("Codigo Contrato: ",txtCodContrato.Text);
                listaAtributos.Add(headerAttribCodContrato); 
            }
            switch (RadioConsultas.SelectedIndex)
            {
                case 0:
                    
                    dr = Consultas.ConsultasContratosBaja(ConsultaFiltro,txtCodContrato);
                    strTituloInforme = "Consultas Contratos Baja";
                    nombreConfiguracionInforme = "ConsultasContratosBaja";
                    break;
                case 1:
                    
                    dr = Consultas.ConsultasMantenimientoProvincia(ConsultaFiltro);
                    strTituloInforme = "Consultas Mantenimiento Provincia";
                    nombreConfiguracionInforme = "ConsultasMantenimientoProvincia";
                    break;
                case 2:
                    
                    dr = Consultas.ConsultaCambioTelefonos(ConsultaFiltro);
                    strTituloInforme = "Consulta Cambio Telefonos";
                    nombreConfiguracionInforme = "ConsultaCambioTelefonos";
                    break;
                case 3:
                    
                    dr = Consultas.ConsultaVencidosPorMes(ConsultaFiltro);
                    strTituloInforme = "Consulta Vencidos Por Mes";
                    nombreConfiguracionInforme = "ConsultaVencidosPorMes";
                    break;
                case 4:
                    
                    dr = Consultas.ObtenerConsultaFacturadasProveedor(ConsultaFiltro);
                    strTituloInforme = "Obtener Consulta Facturadas Proveedor";
                    nombreConfiguracionInforme = "ObtenerConsultaFacturadasProveedor";
                    break;
                case 5:

                    dr = Consultas.ConsultasContratosBajaPORFECHAS(ConsultaFiltro, txtDesde,txtHasta);
                    strTituloInforme = "Consultas Contratos Baja POR FECHAS";
                    nombreConfiguracionInforme = "ConsultasContratosBaja";
                    break;
               //TEL: ¿habría que eliminar esta consulta?
                case 6:

                    dr = Consultas.ConsultaContratosSinTelefonos(ConsultaFiltro);
                    strTituloInforme = "Consulta Contratos sin telefono de contacto";
                    nombreConfiguracionInforme = "ConsultaContratoSinTelefono";
                    break;
                case 7:

                    dr = Consultas.ConsultasContratosBajaCONVISITAPORFECHAS(ConsultaFiltro, txtDesde0, txtHasta0);
                    strTituloInforme = "Consultas Contratos Baja CON VISITAS POR FECHAS";
                    nombreConfiguracionInforme = "ConsultasContratosBaja";
                    break;
                case 8:
                    // Kintell 29/09/2020 R#26735
                    dr = Consultas.VisitasConPRL(txtDesdePRL, txtHastaPRL);
                    strTituloInforme = "Visitas con PRL";
                    nombreConfiguracionInforme = "ConsultasVisitasConPRL";
                    break;
                default:

                    break;
            }
            DataTable dt = new DataTable();
            dt.Load(dr);
            ExportarAExcel(dt, strTituloInforme, nombreConfiguracionInforme, listaAtributos);
            CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dt);
        }

        protected void OnSelectedIndexChanged_RadioConsultas(object sender, EventArgs e)
        {
            try
            {
              MostrarOcultarControlesOpciones();
              switch (this.RadioConsultas.SelectedIndex)
                {
                  case 0:
                        this.placeControlesOp1.Visible = true;
                        this.PlaceHolder1.Visible = false;
                        this.PlaceHolder2.Visible = false;
                        this.PlaceHolder3.Visible = false;
                        break;
                  case 5:
                        this.placeControlesOp1.Visible = false;
                        this.PlaceHolder1.Visible = true;
                        this.PlaceHolder2.Visible = false;
                        this.PlaceHolder3.Visible = false;
                        break;
                  case 7:
                        this.placeControlesOp1.Visible = false;
                        this.PlaceHolder1.Visible = false;
                        this.PlaceHolder2.Visible = true;
                        this.PlaceHolder3.Visible = false;
                        break;
                  case 8:
                        this.placeControlesOp1.Visible = false;
                        this.PlaceHolder1.Visible = false;
                        this.PlaceHolder2.Visible = false;
                        this.PlaceHolder3.Visible = true;
                        break;
                  default:
                        this.placeControlesOp1.Visible = false;
                        this.PlaceHolder1.Visible = false;
                        this.PlaceHolder2.Visible = false;
                        this.PlaceHolder3.Visible = false;
                        break;
                }
            }           
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        private void OcultarControlesOpciones()
        {
            this.placeControlesOp1.Visible = false;   
        }
        private void MostrarOcultarControlesOpciones()
        {
            OcultarControlesOpciones();

            switch (this.RadioConsultas.SelectedIndex)
            {
                case 0:
                    this.placeControlesOp1.Visible = true;  
                break;
            }
        }
        

        private void ExportarAExcel(DataTable dt, String strTituloInforme, String nombreConfiguracionInforme, List<ExcelHeaderAttribute> listaAtributos)
        {
            String strUrlXslt = HttpContext.Current.Server.MapPath(Resources.SMGConfiguration.XsltConsultas);
            ExcelHelper excel = new ExcelHelper(strTituloInforme, strUrlXslt);
            excel.TableName = nombreConfiguracionInforme;
            excel.Attributtes.AddRange(listaAtributos);

            excel.LoadData(dt);
            //excel.GenerateExcel(Response);

            CurrentSession.SetAttribute(CurrentSession.SESSION_EXCEL_HELPER, excel);
            CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);

            //this.OpenExternalWindowTamanio("./AbrirExcel.aspx", "AbrirExcel", "Consulta","50","50");
            ////Response.Write("<script>ExportarExcel();</script>");
            Page.RegisterStartupScript("EXCELL", "<Script language='JavaScript'>ExportarExcel();</script>");
        }
        #endregion

        #region implementación de eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            try
            {
                MostrarOcultarControlesOpciones();
                //this.btnRealizarConsulta.Attributes.Add("OnClick", "setHourglass();");
                if (NavigationController.IsBackward())
                {
                    LoadSessionData();
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnRealizarConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidateForm())
                {
                    VistaContratoCompletoDTO ConsultaFiltro = null;
                    if (ConsultaFiltro == null)
                    {
                        ConsultaFiltro = new VistaContratoCompletoDTO();
                    }

                    AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                    if (usuarioDTO.NombreProveedor != null && usuarioDTO.NombreProveedor.Length > 0)
                    {
                        ConsultaFiltro.Proveedor = usuarioDTO.NombreProveedor.ToString();
                    }
                    else
                    {
                        ConsultaFiltro.Proveedor = null;
                    }
                    CargarListadoConsultas(ConsultaFiltro);
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
        #endregion
        public static Boolean ComprobarEstadoCodigo(TextBox txt)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];
            ParamsName[0] = "@CODCONTRATO";
            ParamsType[0] = DbType.String;
            ParamsValue[0] = txt.Text;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            IDataReader dr = db.RunProcDataReader("SP_GET_CONTRATO_ESTADO", ParamsName, ParamsType, ParamsValue);
            if (dr.Read())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected void txtCodContratoCustomValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (RadioConsultas.SelectedIndex == 0)
                {
                    Boolean estado = ComprobarEstadoCodigo(txtCodContrato);
                    e.IsValid = FormValidation.ValidateNumberEstadoTextBox(txtCodContrato, true, (BaseValidator)sender,estado);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }


        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            // coger los valores de sessión
            Int32? radioSelectedIndex = (Int32?) CurrentSession.GetAttribute("FRM_CONSULTAS_RADIO");
            // cargar los valores en el formulario
            if (radioSelectedIndex.HasValue)
            {
                this.RadioConsultas.SelectedIndex = radioSelectedIndex.Value;
            }

            // eliminar de sesión los valores del formulario
            CurrentSession.RemoveAttribute("FRM_CONSULTAS_RADIO");
        }

        public override void SaveSessionData()
        {
            // cargar los valores en sessión
            CurrentSession.SetAttribute("FRM_CONSULTAS_RADIO", this.RadioConsultas.SelectedIndex);
        }

        public override void DeleteSessionData()
        {
            CurrentSession.RemoveAttribute("FRM_CONSULTAS_RADIO");
        }
        #endregion
    }
}
