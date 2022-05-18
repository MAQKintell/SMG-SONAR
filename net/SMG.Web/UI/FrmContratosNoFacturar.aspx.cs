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
    public partial class FrmContratosNoFacturar : FrmBaseListado
    {
        
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

        public  MasterPageResponsive _Pagina;
        public  MasterPageResponsive Pagina
        {
            get
            {
                return _Pagina;
            }
            set
            {
                _Pagina = (MasterPageResponsive)this.Master; 
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Pagina = (MasterPageResponsive)this.Master;
            if (!IsPostBack)
            {
                //Pagina = (MasterPageResponsive)this.Master;
                Pagina.Titulo = Resources.TitulosVentanas.ContratosNoFacturarBaja;
                inicializarValores();
            }
        }

        private void inicializarValores()
        {
            rpn_main_pnpConsulta_txtReferencia_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNombreCli_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtTipoEFV_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNif_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtDirSuministro_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtCPostal_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtProvincia_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtPoblacion_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaAltaServicio_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaBajaServicio_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtMotivoBaja_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtMotivoBaja_I.Enabled = false;
            btnAceptar.Enabled = false;

        }
        protected void rpn_main_pnpConsulta_btnConsultar_I_Click(object sender, EventArgs e)
        {
            string contrato = rpn_main_pnpConsulta_txtReferencia_I.Text;

            IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(contrato);

            if (datosMantenimiento != null)
            {
                Boolean pasado = false;
                while (datosMantenimiento.Read())
                {
                    DateTime? FEC_BAJA_SERVICIO= (DateTime?)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "FEC_BAJA_SERVICIO");
                    
                    pasado = true;
                    string nombreCliente = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_TITULAR") + " " + (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO1") + " " + (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO2");
                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNombreCli_I.Text = nombreCliente;
                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtTipoEFV_I.Text = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DESEFV");
                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNif_I.Text = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DNI");

                    string Suministro = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_VIA_PUBLICA") + " "
                        + (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_CALLE") + " "
                        + (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_PORTAL") + ", "
                        + (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_BIS") + ", "
                        + (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_ESCALERA") + ", "
                        + (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_PISO") + " "
                        + (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TIP_MANO");


                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtDirSuministro_I.Text = Suministro;

                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtCPostal_I.Text = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_POSTAL");
                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtProvincia_I.Text = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_PROVINCIA");
                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtPoblacion_I.Text = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "COD_POBLACION");

                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaAltaServicio_I.Text = ((DateTime)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "FEC_ALTA_SERVICIO")).ToShortDateString();
                    if (FEC_BAJA_SERVICIO.HasValue)
                    {
                        rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaBajaServicio_I.Text = FEC_BAJA_SERVICIO.Value.ToShortDateString();
                    }                    

                    if (FEC_BAJA_SERVICIO == null)
                    {
                        //20210719 BGN ADD BEG: Si se ha emitido ya la factura de la baja mostrar mensaje, pero permitir que de la baja inmediata del contrato
                        DataTable dtFacturaBajaContrato = Contrato.ObtenerFacturacionBajasPorContrato(contrato);
                        if (dtFacturaBajaContrato != null && dtFacturaBajaContrato.Rows.Count>0)
                        {
                            Pagina.MostrarMensaje(Resources.TextosJavaScript.TEXTO_BAJA_YA_FACTURADA);
                        }
                        //20210719 BGN ADD END
                        btnAceptar.Enabled = true;
                        rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtMotivoBaja_I.Enabled = true;
                    }
                    else
                    {
                        Pagina.MostrarMensaje(Resources.TextosJavaScript.SERVICIO_DADO_DE_BAJA);
                        btnAceptar.Enabled = false;
                        rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtMotivoBaja_I.Enabled = false;
                    }
                }
                if(!pasado)
                {
                    //MasterPageResponsive mp = (MasterPageResponsive)this.Master;
                    Pagina.MostrarMensaje(Resources.TextosJavaScript.CONTRATO_NO_ENCONTRADO);
                    btnAceptar.Enabled = false;
                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtMotivoBaja_I.Enabled = false;
                    inicializarValores();
                }
            }
            
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                // meter en factu bajas y dar de baja cuando llegue la baja.
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                Contrato contrato = new Contrato();
                contrato.InsertFacturacionBajasYMarcarContratoParaDarBaja(rpn_main_pnpConsulta_txtReferencia_I.Text,usuarioDTO.Login, rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtMotivoBaja_I.Text);

                //MasterPageResponsive mp = (MasterPageResponsive)this.Master;
                Pagina.MostrarMensaje(Resources.TextosJavaScript.PROCESO_COMPLETADO);
                inicializarValores();
            }
            catch(Exception ex)
            {
                Pagina.MostrarMensaje(Resources.TextosJavaScript.TEXTO_ERROR_PROCESO);
            }
        }
    }
}
