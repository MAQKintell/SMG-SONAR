﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.DataAccess;
using System.Collections.Generic;
using System.Collections;
using System.Data.Common;
using System.Text;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Exceptions;
using System.Data.SqlClient;
using System.Web.Configuration;

using System.IO;
using System.Linq;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Validations;

namespace Iberdrola.SMG.UI
{
    public partial class FrmDetalleVisita : FrmBase
    {
        String strIdContrato = null;
        String strIdVisita = null;

        private void CargarCombos()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            // Estados de la visita
            this.cmbEstadoVisita.DataSource = TablasReferencia.ObtenerTiposEstadoVisita((Int16) usuarioDTO.IdIdioma);
            this.cmbEstadoVisita.DataValueField = "Codigo";
            this.cmbEstadoVisita.DataTextField = "Descripcion";
            this.cmbEstadoVisita.DataBind();
            FormUtils.AddDefaultItem(cmbEstadoVisita);

            // Tipos de reparación
            this.cmbTipoReparacion.DataSource = TablasReferencia.ObtenerTiposReparacion((Int16)usuarioDTO.IdIdioma);
            this.cmbTipoReparacion.DataValueField = "Id";
            this.cmbTipoReparacion.DataTextField = "Descripcion";
            this.cmbTipoReparacion.DataBind();
            FormUtils.AddDefaultItem(cmbTipoReparacion);

            // Tiempos de mano de obra
            this.cmbTiempoManoObra.DataSource = TablasReferencia.ObtenerTiposTiempoManoDeObra((Int16)usuarioDTO.IdIdioma);
            this.cmbTiempoManoObra.DataValueField = "Id";
            this.cmbTiempoManoObra.DataTextField = "Descripcion";
            this.cmbTiempoManoObra.DataBind();
            FormUtils.AddDefaultItem(cmbTiempoManoObra);

            
            this.cboTecnico.DataSource =Usuarios.ObtenerTecnicoEmpresa (usuarioDTO.NombreProveedor.ToString());
            this.cboTecnico.DataValueField = "ID_TECNICOEMPRESA";
            this.cboTecnico.DataTextField = "NOMBRE_TECNICOEMPRESA";
            this.cboTecnico.DataBind();
            //FormUtils.AddDefaultItem(cboTecnico);
        }

        private void CargarDatosFormulario(string CodContrato, string CodVisita)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            IDataReader Datos = null;
            try
            {
                //****************************************************************************
                //Kintell 07/06/2011
                Datos = Mantenimiento.GetSolicitudesPorContrato(CodContrato, usuarioDTO.IdIdioma);
                Boolean Existe = false;
                while (Datos.Read())
                {
                    String Estado = (String)DataBaseUtils.GetDataReaderColumnValue(Datos, "Estado_solicitud");
                    if (Estado != "010" && Estado != "011" && Estado != "012" && Estado != "014" && Estado != "020" && Estado != "021" && Estado != "022" && Estado != "023" && Estado != "024")//Estado_solicitud
                    {
                        Existe = true;
                    }
                }
                if (Existe) { MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_ABIERTA_ACTUALMENTE); }//"Este contrato tiene una solicitud abierta actualmente."); }
            }
            finally
            {
                if (Datos != null)
                {
                    Datos.Close();
                }
            }
            
            //****************************************************************************

            
            
            Visitas visitas = new Visitas();

            //Boolean Acceso = usuarios.ObtenerUsuarioCompleto(this.txtUsuario.Text, this.txtPassword.Text);
            VisitaDTO visitasDTO = new VisitaDTO();
            visitasDTO = visitas.DatosVisitas(CodContrato, CodVisita);

            if (visitasDTO.CodigoContrato == null)
            {
                //No se ha encontrado ese contrato con ese código de visita.

                //string alerta = "<script type='text/javascript'>Alert('No tiene permisos para ver los datos de este contrato');</script>";

                //Response.Write(alerta);
                
                NavigationController.Backward();

                
                //MasterPageBase mp1 = (MasterPageBase)this.Master;
                //mp1.PlaceHolderScript.Controls.Add(new LiteralControl(alerta));

                
                return;
            }
            

            this.hdnEstadoAnteriorVisita.Value = (String)visitasDTO.CodigoEstadoVisita;
            if (visitasDTO.Campania.HasValue)
            {
                this.lblIdCampana.Text = (String)visitasDTO.Campania.ToString();
            }


                this.txtCategoriaVisita.Text = (String)visitasDTO.CategoriaVisita.ToString();
      

            
            if (visitasDTO.IdLote.HasValue)
            {
                this.lblIdLote.Text = (String)visitasDTO.IdLote.ToString();
            }

            if (visitasDTO.Aviso != null)
            {
                this.lblAviso.Text = (String)visitasDTO.Aviso.ToString();
            }

            if (visitasDTO.COD_RECEPTOR != null)
            {
                this.lblCodReceptor.Text = (String)visitasDTO.COD_RECEPTOR.ToString();
            }

            if (visitasDTO.FechaLimiteVisita.HasValue)
            {
                this.txtFechaLimite.Text = (String)visitasDTO.FechaLimiteVisita.ToString();
            }
            if (this.txtFechaLimite.Text != String.Empty)
            {
                this.txtFechaLimite.Text = this.txtFechaLimite.Text.Substring(0, 10);
            }

            if (visitasDTO.FechaFactura.HasValue)
            {
                this.txtFechaFactura.Text = (String)visitasDTO.FechaFactura.ToString();
            }
            if (this.txtFechaFactura.Text != String.Empty)
            {
                this.txtFechaFactura.Text = this.txtFechaFactura.Text.Substring(0, 10);
            }

            if (visitasDTO.CodigoInterno != null)
            {
                this.txtCodigoAlfanumerico.Text = (String)visitasDTO.CodigoInterno.ToString();
            } 

            if (visitasDTO.FechaVisita.HasValue)
            {
                this.txtFechaVisita.Text = (String)visitasDTO.FechaVisita.ToString();
            }
            if (this.txtFechaVisita.Text != String.Empty)
            {
                this.txtFechaVisita.Text = this.txtFechaVisita.Text.Substring(0, 10);
            }

            //Poner en controles los valores.

            if (visitasDTO.Tecnico != 0)
            {
                IDataReader drContVisitas = null;
                try
                {
                    this.cboTecnico.SelectedValue = visitasDTO.Tecnico.ToString();
                    this.txtTecnico.Text = this.cboTecnico.SelectedItem.Text.ToString();
                    this.txtEmpresa.Text = Usuarios.ObtenerEmpresaPorTecnico(Int16.Parse(this.cboTecnico.SelectedValue.ToString()));

                    //Metemos en en id oculto el valor del contador, para saber cuantas vistas lleva ese técnico en el dia.
                    Int32 nContadorVisDiaTecnico = 0;

                    if (this.txtFechaVisita.Text != String.Empty)
                    {

                        if (!this.cboTecnico.SelectedValue.ToString().Equals(""))
                        {
                            drContVisitas = visitas.ObtenerContador_Visitas_Dia_por_Tecnico(Int16.Parse(this.cboTecnico.SelectedValue.ToString()), DateTime.Parse(this.txtFechaVisita.Text));


                            while (drContVisitas.Read())
                            {
                                nContadorVisDiaTecnico = Int32.Parse(DataBaseUtils.GetDataReaderColumnValue(drContVisitas, "CONTADOR_VISITA_DIA").ToString());
                            }

                            this.hdnTecnicoValido.Value = (String)nContadorVisDiaTecnico.ToString();
                        }
                    }

                }
                catch (Exception ex)
                {
                    String Nombre = Usuarios.ObtenerNombreTecnicoPorTecnico(Int16.Parse(visitasDTO.Tecnico.ToString()));
                    ListItem Valor = new ListItem(Nombre, visitasDTO.Tecnico.ToString());
                    this.cboTecnico.Items.Add(Valor);
                }
                finally
                {
                    if (drContVisitas != null)
                    {
                        drContVisitas.Close();
                    }

                    this.cboTecnico.SelectedValue = visitasDTO.Tecnico.ToString();
                    this.txtTecnico.Text = this.cboTecnico.SelectedItem.Text.ToString();
                    this.txtEmpresa.Text = Usuarios.ObtenerEmpresaPorTecnico(Int16.Parse(this.cboTecnico.SelectedValue.ToString()));
                }
            }

            if (visitasDTO.ObservacionesVisita != null)
            {
                this.txtObservacionesVisita.Text = (String)visitasDTO.ObservacionesVisita.ToString();
            }
            if (visitasDTO.TipoVisita != null)
            {
                this.cboTipoVisita.SelectedValue = visitasDTO.TipoVisita.ToString();
            }



            if (visitasDTO.NumFactura != null)
            {
                this.txtNumFactura.Text = (String)visitasDTO.NumFactura.ToString();
            }

            if (visitasDTO.Observaciones != null)
            {
                this.txtObservaciones.Text = (String)visitasDTO.Observaciones;
            }

            if (visitasDTO.CodigoBarras != null)
            {
                this.txtInfoCodigoBarras.Text = (String)visitasDTO.CodigoBarras.ToString();
            }

            if (visitasDTO.IdReparacion.HasValue)
            {
                this.hdnIdReparacion.Value = (String)visitasDTO.IdReparacion.ToString();
            }

            if (visitasDTO.FechaEnviadoCarta.HasValue)
            {
                this.txtFechaEnvioCarta.Text = (String)visitasDTO.FechaEnviadoCarta.ToString();
            }
            if (this.txtFechaEnvioCarta.Text != String.Empty)
            {
                this.txtFechaEnvioCarta.Text = this.txtFechaEnvioCarta.Text.Substring(0, 10);
            }
            
            if (visitasDTO.RecepcionComprobante == true)
            {
                this.chkRecepcionComprobante.Checked = true;
            }
            else
            {
                this.chkRecepcionComprobante.Checked = false;
            }

            if (visitasDTO.InformacionRecibida == true)
            {
                this.chkInformacionVisita.Checked = true;
            }
            else
            {
                this.chkInformacionVisita.Checked = false;
            }
            //*************************************************
            //22-02-2012
            if (visitasDTO.ContadorInterno == true)
            {
                this.cmbContador.Items[1].Selected = true;
            }
            else
            {
                this.cmbContador.Items[2].Selected = true;
            }
            //**************************************************
            if (visitasDTO.CodigoEstadoVisita != null)
            {
                hdnEstadoAnterior.Value = visitasDTO.CodigoEstadoVisita.ToString();// cmbEstadoVisita.SelectedIndex.ToString();
                if (visitasDTO.CodigoEstadoVisita == "10")
                {
                    ListItem nuevo=new ListItem();
                    nuevo.Value="10";
                    nuevo.Text = Resources.TextosJavaScript.TEXTO_CANCELADA_POR_SISTEMA;//"Sistema: Cancelada por sistema";
                    cmbEstadoVisita.Items.Add(nuevo);
                }
                
                cmbEstadoVisita.SelectedValue = visitasDTO.CodigoEstadoVisita;
            }

            if (visitasDTO.FacturadoProveedor == true)
            {
                this.chkFacturadoProveedor.Checked = true;
            }
            else
            {
                this.chkFacturadoProveedor.Checked = false;
            }

            if (visitasDTO.CartaEnviada == true)
            {
                chkListCartaEnviada.Items[0].Selected = true;
            }
            else
            {
                chkListCartaEnviada.Items[0].Selected = false;
            }

            //BGN ADD INI R#25266: Visualizar si una visita tiene PRL por Web
            if (visitasDTO.PRL != null && visitasDTO.PRL == true)
            {
                this.chkPRL.Checked = true;
            }
            else
            {
                this.chkPRL.Checked = false;
            }
            //BGN ADD FIN R#25266: Visualizar si una visita tiene PRL por Web

            if (visitasDTO.TipoUrgencia.HasValue)
            {
                IDataReader drUrgencia = null;
                try
                {
                    drUrgencia = visitas.ObtenerTipoUrgencia(visitasDTO.TipoUrgencia.ToString(), (int)usuarioDTO.IdIdioma);
                    while (drUrgencia.Read())
                    {
                        this.lblValorUrgencia.Text = (string)DataBaseUtils.GetDataReaderColumnValue(drUrgencia, "DESC_TIPO_URGENCIA");
                    }
                }
                finally
                {
                    if (drUrgencia != null)
                    {
                        drUrgencia.Close();
                    }
                }
            }

            if (visitasDTO.IdLote.HasValue)
            {
                IDataReader drLote = null;
                try
                {
                    drLote = visitas.ObtenerDescLote(visitasDTO.IdLote.ToString());
                    while (drLote.Read())
                    {
                        this.lblIdLote.Text = this.lblIdLote.Text + " " + (string)DataBaseUtils.GetDataReaderColumnValue(drLote, "DESC_LOTE");
                    }
                }
                finally
                {
                    if (drLote != null)
                    {
                        drLote.Close();
                    }
                }
            }
            //Obtenemos datos del titular.
            Mantenimiento mantenimiento = new Mantenimiento();
            MantenimientoDTO mantenimientoDTO = new MantenimientoDTO();
            mantenimientoDTO = mantenimiento.DatosMantenimiento(CodContrato,usuarioDTO.Pais);
            // Si esta de baja deshabilitamos todo.
            if (mantenimientoDTO.FEC_BAJA_SERVICIO != null) { DeshabilitarControlesContratoBaja(); }

            if (mantenimientoDTO.T1 == "S")
            {
                lblInfoTipoMantenimiento.Text = Resources.TextosJavaScript.TEXTO_MANTENIMIENTO_GAS_CALEFACCION;// "Mantenimiento gas calefaccción";
            }
            else
            {
                lblInfoTipoMantenimiento.Text = Resources.TextosJavaScript.TEXTO_MANTENIMIENTO_GAS;// "Mantenimiento gas";
            }

            if (string.IsNullOrEmpty(mantenimientoDTO.SCORING))
            {
                lblScoring.Text = "N";
            }
            else
            {
                lblScoring.Text = mantenimientoDTO.SCORING.ToString();
            }


            if (mantenimientoDTO.COD_CONTRATO_SIC != null)
            {
                this.lblIdContrato.Text = mantenimientoDTO.COD_CONTRATO_SIC;
                strIdContrato = lblIdContrato.Text;
            }
            // 09/04/2013 Display the EFV instead of the "ordinary" text.
            lblInfoTipoMantenimiento.Text = "";
            if(mantenimientoDTO.DESEFV != null)
            {
                ldlEFV.Text = mantenimientoDTO.DESEFV;
                lblInfoTipoMantenimiento.Text = mantenimientoDTO.DESEFV;
            }
            //BGN 20191217 Proteccion Gas (EMTOGASCAN) tiene que permitir 2 años
            if (mantenimientoDTO.CODEFV.Equals("EMTOGASCAN"))
            {
                hdnEsProteccion.Value = "1";
            }
            else
            {
                hdnEsProteccion.Value = "0";
            }
            
            //if (mantenimientoDTO.COD_ULTIMA_VISITA != null)
            //{
            //    this.lblIdVisita.Text = mantenimientoDTO.COD_ULTIMA_VISITA.ToString();
            //    strIdVisita = lblIdVisita.Text;
            //}

            if (mantenimientoDTO.NOM_TITULAR != null)
            {
                this.lblInfoTitular.Text = mantenimientoDTO.NOM_TITULAR + " " + mantenimientoDTO.APELLIDO1 + " " + mantenimientoDTO.APELLIDO2;
                if (mantenimientoDTO.DNI != null) { this.lblInfoTitular.Text += " " + mantenimientoDTO.DNI.ToString(); }
            }

            if (mantenimientoDTO.TELEFONO1 != null)
            {
                this.txtTelefono1.Text = mantenimientoDTO.TELEFONO1;
            }
            if (mantenimientoDTO.TELEFONO2 != null)
            {
                this.txtTelefono2.Text = mantenimientoDTO.TELEFONO2;
            }
            

            if ((mantenimientoDTO.TIP_VIA_PUBLICA != null) && (mantenimientoDTO.NOM_CALLE != null) && (mantenimientoDTO.COD_PORTAL != null) && (mantenimientoDTO.TIP_PISO != null) && (mantenimientoDTO.TIP_MANO != null))
            {
                this.lblInfoDomicilio.Text = mantenimientoDTO.TIP_VIA_PUBLICA + " " + mantenimientoDTO.NOM_CALLE + " FINCA: " + mantenimientoDTO.COD_FINCA + " PORTAL: " + mantenimientoDTO.COD_PORTAL + " PISO: " + mantenimientoDTO.TIP_PISO + " MANO: " + mantenimientoDTO.TIP_MANO;
            }

            //TEL:
            if (!string.IsNullOrEmpty(mantenimientoDTO.NUM_TEL_CLIENTE))
            {
                this.lblInfoTelefono.Text = mantenimientoDTO.NUM_TEL_CLIENTE;
            }

            if (!string.IsNullOrEmpty(mantenimientoDTO.NUM_TEL_PS))
            {
                if (!string.IsNullOrEmpty(this.lblInfoTelefono.Text))
                {
                    this.lblInfoTelefono.Text += "/";
                }
                this.lblInfoTelefono.Text += mantenimientoDTO.NUM_TEL_PS;
            }

            if (!string.IsNullOrEmpty(mantenimientoDTO.NUM_MOVIL))
            {
                if (!string.IsNullOrEmpty(this.lblInfoTelefono.Text))
                {
                    this.lblInfoTelefono.Text += "/";
                }
                this.lblInfoTelefono.Text += mantenimientoDTO.NUM_MOVIL;
            }


            if (mantenimientoDTO.COD_PROVINCIA != null)
            {
                this.lblInfoProvincia.Text = mantenimientoDTO.COD_PROVINCIA;
            }
            if (mantenimientoDTO.COD_POBLACION != null)
            {
                this.lblInfoPoblacion.Text = mantenimientoDTO.COD_POBLACION;
            }
            //Datos de la reparacion.
            if (visitasDTO.IdReparacion.HasValue && visitasDTO.IdReparacion.Value != decimal.Parse("0"))
            {
                this.rdbReparacion.Items[1].Selected = true;

                ReparacionDTO reparacionDTO = new ReparacionDTO();

                reparacionDTO = Reparacion.ObtenerReparacion(visitasDTO.IdReparacion.Value);
                if (reparacionDTO.FechaReparacion.HasValue)
                {
                    this.txtFechaReparacion.Text = (String)reparacionDTO.FechaReparacion.ToString();
                }
                if (this.txtFechaReparacion.Text != String.Empty)
                {
                    this.txtFechaReparacion.Text = this.txtFechaReparacion.Text.Substring(0, 10);
                }

                if (reparacionDTO.IdTipoReparacion.HasValue)
                {
                    this.cmbTipoReparacion.SelectedValue = (String)reparacionDTO.IdTipoReparacion.ToString();
                }

                if (reparacionDTO.IdTipoTiempoManoObra.HasValue)
                {
                    this.cmbTiempoManoObra.SelectedValue = (String)reparacionDTO.IdTipoTiempoManoObra.ToString();
                }
                if (reparacionDTO.ImporteReparacion != null)
                {
                    this.txtCosteMateriales.Text = (String)reparacionDTO.ImporteReparacion.ToString();
                }
                if (reparacionDTO.ImporteManoObraAdicional != null)
                {
                    this.txtImporteManoObra.Text = (String)reparacionDTO.ImporteManoObraAdicional.ToString();
                }
                if (reparacionDTO.ImporteMaterialesAdicional != null)
                {
                    this.txtCosteMaterialesAdicional.Text = (String)reparacionDTO.ImporteMaterialesAdicional.ToString();
                }
                //Kintell 11/01/10
                if (reparacionDTO.FechaFactura.HasValue)
                {
                    this.txtFechaFacturaReparacion.Text = (String)reparacionDTO.FechaFactura.ToString();
                }
                if (this.txtFechaFacturaReparacion.Text != String.Empty)
                {
                    this.txtFechaFacturaReparacion.Text = this.txtFechaFacturaReparacion.Text.Substring(0, 10);
                }
                if (reparacionDTO.NumeroFacttura != null)
                {
                    this.txtNumeroFacturaReparacion.Text = (String)reparacionDTO.NumeroFacttura.ToString();
                }
                if (reparacionDTO.CodigoBarrasReparacion != null)
                {
                    this.txtCodigoBarrasReparacion.Text = (String)reparacionDTO.CodigoBarrasReparacion.ToString();
                }

                if (reparacionDTO.InformacionRecibida == true)
                {
                    this.chkInformacionReparacion.Checked = true;
                }
                else
                {
                    this.chkInformacionReparacion.Checked = false;
                }
            }
            else
            {
                this.rdbReparacion.Items[0].Selected = true;
            }
            //LogHelper.Debug("Usuario Correctamente validado: " + strUsuario, LogHelper.Category.UserInterface);
            //this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirVentana();</script>"));
            MasterPageBase mp = (MasterPageBase)this.Master;
            mp.PlaceHolderScript.Controls.Add(new LiteralControl("<script>PosicionarTecnico();</script>"));
        }


        private void DeshabilitarControlesContratoBaja()
        {
            MostrarMensaje("CONTRATO DE BAJA"); 
            this.panel1.Enabled = false;
            // deshabilitamos además los controles del panel, ya que en realidad
            // estos no son hijos suyos por problemas de maquetación.

            this.cmbEstadoVisita.Enabled = false;
            this.txtFechaVisita.Enabled = false;
            this.txtObservaciones.Enabled = false;
            this.txtFechaFactura.Enabled = false;
            this.txtNumFactura.Enabled = false;
            this.txtInfoCodigoBarras.Enabled = false;
            this.chkFacturadoProveedor.Enabled = false;
            this.rdbReparacion.Items[0].Selected = true;
            this.rdbReparacion.Enabled = false;

            //Kintell 18/12/2009.
            this.lblTipoReparacion.Enabled = this.rdbReparacion.Enabled;
            this.cmbTipoReparacion.Enabled = this.rdbReparacion.Enabled;
            this.lblFechaReparacion.Enabled = this.rdbReparacion.Enabled;
            this.txtFechaReparacion.Enabled = this.rdbReparacion.Enabled;
            this.cmbTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
            this.txtCosteMateriales.Enabled = this.rdbReparacion.Enabled;
            this.txtImporteManoObra.Enabled = this.rdbReparacion.Enabled;
            this.txtCosteMaterialesAdicional.Enabled = this.rdbReparacion.Enabled;
            this.lblTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
            this.lblTiempoManoObraCliente.Enabled = this.rdbReparacion.Enabled;
            this.lblCosteMariales.Enabled = this.rdbReparacion.Enabled;
            this.lblCosteMaterialesCliente.Enabled = this.rdbReparacion.Enabled;
            this.lblServicio.Enabled = this.rdbReparacion.Enabled;
            this.lblAdicionalCliente.Enabled = this.rdbReparacion.Enabled;

            //Kintell 11/01/10
            this.lblFactura.Enabled = this.rdbReparacion.Enabled;
            //this.txtFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
            this.txtNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
            this.lblFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
            this.lblNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
            this.txtCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
            this.lblCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;

            this.lblCodReceptor.Text += " CONTRATO DE BAJA";
            this.panelVisita.GroupingText += " CONTRATO DE BAJA";
            MostrarMensaje("CONTRATO DE BAJA"); 
        }
        private void DeshabilitarControles()
        {
            //Obligamos a meter datos de la visita al ser 2ª vez que pasa a ausente por ausencia del cliente.
            //IDataReader DatosAusente = Visitas.ObtenerFechasMovimientoDeClienteAusente((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));
            int Cont = 0;
            //while (DatosAusente.Read())
            //{
            //    Cont = Cont + 1;
            //}

            if(cmbEstadoVisita.SelectedItem.Value =="08")
            {
                Cont=2;
            }
            CurrentSession.SetAttribute(CurrentSession.SESSION_VECES_CLIENTE_AUSENTE, Cont);
            
            //*************************************************************
            String strEstadoVisita = "";
            if (FormUtils.HasValue(cmbEstadoVisita))
            {
                strEstadoVisita = this.cmbEstadoVisita.SelectedItem.Value;
            }


            Boolean esVisitaConsideradaCerrada = Visitas.EsEstadoVisitaConsideradaCerrada(strEstadoVisita);

            // Recepción de comprobante solo estará habilitado si la visita es cerrada
            if (esVisitaConsideradaCerrada)
            {
                this.chkRecepcionComprobante.Enabled = true;
            }
            else
            {
                if (Cont == 0)
                {
                    this.chkRecepcionComprobante.Enabled = false;
                    this.chkRecepcionComprobante.Checked = false;
                }
            }

            // Facturado proveedor no se podrá modificar si la visita es cerrada y ya tiene 
            // número de factura
            if (esVisitaConsideradaCerrada && this.txtNumFactura.Text.Length > 0)
            {
                this.chkFacturadoProveedor.Enabled = false;
            }
            else
            {
                this.chkFacturadoProveedor.Enabled = true;
            }

            // MMarcos 18/01/10
            // No se debe permitir cambiar de cerrada a ningún otro estado si la visita esta facturada. (tiene fecha de factura)
            if (esVisitaConsideradaCerrada && this.txtFechaFactura.Text.Length > 0)
            {
                if (this.txtFechaFactura.Text != "")
                {
                    string mes = DateTime.Now.AddMonths(0).ToString("MMMM");
                    DateTime fechaLiquidacion = DateTime.Parse(this.txtFechaFactura.Text);
                    DateTime fechaActualMenos20dias = DateTime.Now.AddDays(-20);
                    DateTime fechaLiquidacionMenos20dias = fechaLiquidacion.AddDays(-20);
                    TimeSpan dias = fechaActualMenos20dias.Subtract(fechaLiquidacionMenos20dias);

                    if (dias.Days > 20)
                    //if (this.txtNumFactura.Text != "V_" + mes + "_2014")
                    {
                        this.cmbEstadoVisita.Enabled = false;
                        if (DateUtils.IsDateTimeHTML(txtFechaVisita.Text))
                        {
                            this.txtFechaVisita.Enabled = false;
                        }
                    }
                }
            }

            // Fecha de factura: sólo habilitado si Facturado Proveedor tiene valor sí
            if (this.chkFacturadoProveedor.Checked)
            {
                //this.txtFechaFactura.Enabled = true;
            }
            else
            {
                this.txtFechaFactura.Enabled = false;
                this.txtFechaFactura.Text = String.Empty;
            }

            // Número de factura: sólo habilitado si Facturado Proveedor tiene valor sí
            // y además el usuario es administrador
            if (this.chkFacturadoProveedor.Checked)
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                if (Usuarios.EsAdministrador(usuarioDTO))
                {
                    //this.txtNumFactura.Enabled = true;
                }
                else
                {
                    this.txtNumFactura.Enabled = false;
                }
            }
            else
            {
                this.txtNumFactura.Enabled = false;
            }

            // Reparación está habilitado si estado visita es cerrada y además
            // estado anterior visita es Pendiente reparacion
            // GGB : --> modificación. Ahora siempre está habilitado, pero sólo es obligatorio
            // informar todos los campos cuando se de esa condición

            //if (Visitas.EsEstadoVisitaCerrada(strEstadoVisita) && this.hdnEstadoAnteriorVisita.Value != null &&
            //    this.hdnEstadoAnteriorVisita.Value.ToString().Equals(StringEnum.GetStringValue(Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)))
            //{
            //    this.rdbReparacion.Enabled = true;
            //}
            //else
            //{
            //    this.rdbReparacion.Enabled = false;
            //}

            this.rdbReparacion.Enabled = true;

            //RequiredFieldValidator1.Enabled = false;
            txtFechaReparacionValidator.Enabled = this.rdbReparacion.Items[1].Selected;
            RegularExpressionValidator3.Enabled = false;
            RangeValidator1.Enabled = false;

            // Tipo de avería y tiempo mano obra sólo habilitado si campo reparación tiene valor sí
            if (this.rdbReparacion.Items[1].Selected)
            {
                Boolean reparacionObligatoria = false;
                if (Visitas.EsEstadoVisitaCerrada(strEstadoVisita) && this.hdnEstadoAnteriorVisita.Value != null &&
                    this.hdnEstadoAnteriorVisita.Value.ToString().Equals(StringEnum.GetStringValue(Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)))
                {
                    reparacionObligatoria = true;
                }
                else
                {
                    reparacionObligatoria = false;
                }

                reparacionObligatoria = true;

                //RequiredFieldValidator1.Enabled = reparacionObligatoria;
                //RequiredFieldValidator2.Enabled = reparacionObligatoria;
                RegularExpressionValidator3.Enabled = reparacionObligatoria;
                RangeValidator1.Enabled = reparacionObligatoria;


                this.lblTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                this.cmbTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                this.lblFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.txtFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.cmbTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                this.txtCosteMateriales.Enabled = this.rdbReparacion.Enabled;
                this.txtImporteManoObra.Enabled = this.rdbReparacion.Enabled;
                this.txtCosteMaterialesAdicional.Enabled = this.rdbReparacion.Enabled;
                this.lblTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                this.lblTiempoManoObraCliente.Enabled = this.rdbReparacion.Enabled;
                this.lblCosteMariales.Enabled = this.rdbReparacion.Enabled;
                this.lblCosteMaterialesCliente.Enabled = this.rdbReparacion.Enabled;
                this.lblServicio.Enabled = this.rdbReparacion.Enabled;
                this.lblAdicionalCliente.Enabled = this.rdbReparacion.Enabled;

                //Kintell 11/01/10
                //this.lblFactura.Enabled = this.rdbReparacion.Enabled;
                //this.txtFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.txtNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.lblFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.lblNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.txtCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
                this.lblCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
            }
            else
            {
                this.lblTipoReparacion.Enabled = false;
                this.cmbTipoReparacion.Enabled = false;
                this.cmbTipoReparacion.ClearSelection();
                this.lblFechaReparacion.Enabled = false;
                this.txtFechaReparacion.Enabled = false;
                this.txtFechaReparacion.Text = String.Empty;
                this.cmbTiempoManoObra.Enabled = false;
                this.cmbTiempoManoObra.ClearSelection();
                this.txtCosteMateriales.Enabled = false;
                this.txtCosteMateriales.Text = String.Empty;
                this.txtImporteManoObra.Text = String.Empty;
                this.txtImporteManoObra.Enabled = false;
                this.txtCosteMaterialesAdicional.Enabled = false;
                this.txtCosteMaterialesAdicional.Text = String.Empty;
                this.lblTiempoManoObra.Enabled = false;
                this.lblTiempoManoObraCliente.Enabled = false;
                this.lblCosteMariales.Enabled = false;
                this.lblCosteMaterialesCliente.Enabled = false;
                this.lblServicio.Enabled = false;
                this.lblAdicionalCliente.Enabled = false;

                //Kintell 11/01/10
                this.lblFactura.Enabled = false;
                this.txtFechaFacturaReparacion.Enabled = false;
                this.txtNumeroFacturaReparacion.Enabled = false;
                this.lblFechaFacturaReparacion.Enabled = false;
                this.lblNumeroFacturaReparacion.Enabled = false;
                this.txtCodigoBarrasReparacion.Enabled = false;
                this.lblCodigoBarrasReparacion.Enabled = false;

                this.txtFechaFacturaReparacion.Text = "";
                this.txtNumeroFacturaReparacion.Text = "";
                this.txtCodigoBarrasReparacion.Text = "";

            }

            // Carta Enviada: solo habilitada cuando estado de la visita es 
            // cancelada cliente no localizable
            if (strEstadoVisita.Equals(StringEnum.GetStringValue(Enumerados.EstadosVisita.CanceladaClienteNoLocalizable)))
            {
                this.chkListCartaEnviada.Items[0].Enabled = true;
                //this.txtFechaEnvioCarta.Enabled = true;
            }
            else
            {
                this.chkListCartaEnviada.Items[0].Enabled = false;
                //this.txtFechaEnvioCarta.Enabled = false;
            }

            // Si la visita está en estado pendiente, no se permite modificar nada más que la caldera. 
            // Ya que ni tan siquiera tiene un lote.
            if (StringEnum.GetStringValue(Enumerados.EstadosVisita.Pendiente).Equals(strEstadoVisita))
            {
                // deshabilitamos el panel
                this.panel1.Enabled = false;
                // deshabilitamos además los controles del panel, ya que en realidad
                // estos no son hijos suyos por problemas de maquetación.

                this.cmbEstadoVisita.Enabled = false;
                this.txtFechaVisita.Enabled = false;
                this.txtObservaciones.Enabled = false;
                this.txtFechaFactura.Enabled = false;
                this.txtNumFactura.Enabled = false;
                this.txtInfoCodigoBarras.Enabled = false;
                this.chkFacturadoProveedor.Enabled = false;
                this.rdbReparacion.Items[0].Selected = true;
                this.rdbReparacion.Enabled = false;

                //Kintell 18/12/2009.
                this.lblTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                this.cmbTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                this.lblFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.txtFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.cmbTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                this.txtCosteMateriales.Enabled = this.rdbReparacion.Enabled;
                this.txtImporteManoObra.Enabled = this.rdbReparacion.Enabled;
                this.txtCosteMaterialesAdicional.Enabled = this.rdbReparacion.Enabled;
                this.lblTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                this.lblTiempoManoObraCliente.Enabled = this.rdbReparacion.Enabled;
                this.lblCosteMariales.Enabled = this.rdbReparacion.Enabled;
                this.lblCosteMaterialesCliente.Enabled = this.rdbReparacion.Enabled;
                this.lblServicio.Enabled = this.rdbReparacion.Enabled;
                this.lblAdicionalCliente.Enabled = this.rdbReparacion.Enabled;

                //Kintell 11/01/10
                this.lblFactura.Enabled = this.rdbReparacion.Enabled;
                //this.txtFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.txtNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.lblFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.lblNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                this.txtCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
                this.lblCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
            }
            //Kintell 13/01/10
            if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.Cerrada && int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion && int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.CanceladaAusentePorSegundaVez)//cmbEstadoVisita.SelectedIndex != 6 && cmbEstadoVisita.SelectedIndex != 7 && cmbEstadoVisita.SelectedIndex != 2)
            {
                this.chkFacturadoProveedor.Checked = false;
                this.chkFacturadoProveedor.Enabled = false;
            }
            
            AppPrincipal usuarioPrincipal1 = (AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO1 = (UsuarioDTO)((AppIdentity)usuarioPrincipal1.Identity).UserObject;

            if (usuarioDTO1.Id_Perfil == 4)
            {
                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.CanceladaClienteNoLocalizable)//cmbEstadoVisita.SelectedIndex != 4)
                {
                    this.chkListCartaEnviada.Items[0].Enabled = false;
                    //this.chkListCartaEnviada.Items[0].Selected = false;
                }
            }
            else
            {
                this.chkListCartaEnviada.Items[0].Enabled = false;
                //this.chkListCartaEnviada.Items[0].Selected = false;
            }
            if (this.chkFacturadoProveedor.Checked == true)
            {
                //this.txtNumFactura.Enabled = true;
            }

            //*************************************************************

            //Obligamos ha meter datos de la visita al ser 2ª vez que pasa a ausente por ausencia del cliente.
            if (Cont > 0)
            {
                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CanceladaClienteNoLocalizable)//cmbEstadoVisita.SelectedIndex == 4)
                {
                    txtFechaVisita.Enabled =true;
                    chkRecepcionComprobante.Enabled =true;
                    ////chkRecepcionComprobante.Checked = true;
                    ////txtFechaFactura.Enabled =true;
                    //txtNumFactura.Enabled =true;
                    txtInfoCodigoBarras.Enabled = true;
                }
            }


            //13/04/2010 Deshabilitamos el número factura y fecha factura de reparación porke se mete por el nuevo formulario.
            txtNumeroFacturaReparacion.Enabled = false;
            txtFechaFacturaReparacion.Enabled = false;
        }

        private void DeshabilitarControlesInicio(String strCodVisita, String strCodContrato)
        {
            // si la visita que estamos editanto no es la última visita del contrato
            // NO se puede modificar el estado
            Int16? codUltimaVisita = Contrato.ObtenerCodigoUltimaVisita(strCodContrato);
            if (codUltimaVisita.HasValue && !codUltimaVisita.Value.ToString().Equals(strCodVisita))
            {
                this.cmbEstadoVisita.Enabled = false;
            }
            // Si la visita que estamos editando está pendiente ( no tiene ningún lote generado )
            // no se le puede modificar el estado
        }

        private void MarcarControlesObligatorios()
        {
            // si el estado de la visita es cerrada la Fecha de la visita es obligatoria
            String strEstadoVisita = this.cmbEstadoVisita.SelectedItem.Value;
            if (Visitas.EsEstadoVisitaConsideradaCerrada(strEstadoVisita))
            {
                this.txtFechaVisita.Attributes.Add("class", "campoFormularioObligatorio");
            }
        }

        private VisitaDTO RecuperarInformacionVisitaFormulario()
        {
            VisitaDTO visitaDTO = new VisitaDTO();
            visitaDTO.CodigoContrato = FormUtils.GetString(lblIdContrato);
            visitaDTO.CodigoVisita = FormUtils.GetDecimal(lblIdVisita);
            visitaDTO.CodigoEstadoVisita = FormUtils.GetValue(cmbEstadoVisita).ToString();
            visitaDTO.FechaVisita = FormUtils.GetDateTime(this.txtFechaVisita);
            visitaDTO.FechaLimiteVisita = FormUtils.GetDateTime(this.txtFechaLimite);
            visitaDTO.Observaciones = FormUtils.GetString(this.txtObservaciones);
            visitaDTO.FechaFactura = FormUtils.GetDateTime(this.txtFechaFactura);
            visitaDTO.NumFactura = FormUtils.GetString(this.txtNumFactura);
            visitaDTO.CodigoBarras = FormUtils.GetString(this.txtInfoCodigoBarras);
            visitaDTO.RecepcionComprobante = chkRecepcionComprobante.Checked;
            visitaDTO.FacturadoProveedor = chkFacturadoProveedor.Checked;
            //*****************************************************************
            //22-02-2012
            Boolean Contador = false;
            if (this.cmbContador.Items[1].Selected) { Contador = true; }
            visitaDTO.ContadorInterno = Contador;
            visitaDTO.CodigoInterno = FormUtils.GetString(this.txtCodigoAlfanumerico);
            //*****************************************************************
            visitaDTO.CartaEnviada = this.chkListCartaEnviada.Items[0].Selected;
            visitaDTO.FechaEnviadoCarta = FormUtils.GetDateTime(this.txtFechaEnvioCarta);
            visitaDTO.InformacionRecibida = chkInformacionVisita.Checked;
            if (this.cboTecnico.SelectedValue.ToString() != "")
            {
                visitaDTO.Tecnico = Int16.Parse(this.cboTecnico.SelectedValue.ToString());
            }
            else
            {
                visitaDTO.Tecnico = 0;
            }
            
            visitaDTO.ObservacionesVisita =this.txtObservacionesVisita.Text;
            visitaDTO.TipoVisita =this.cboTipoVisita.SelectedValue;
            return visitaDTO;
        }

        private ReparacionDTO RecuperarInformacionReparacionFormulario(bool Completo)
        {
            ReparacionDTO reparacionDTO = new ReparacionDTO();
            reparacionDTO.CodContrato = FormUtils.GetString(lblIdContrato);
            reparacionDTO.CodVisita = FormUtils.GetDecimal(lblIdVisita);
            reparacionDTO.Id = FormUtils.GetDecimal(hdnIdReparacion.Value);

            if (this.txtCosteMateriales.Text == "") { this.txtCosteMateriales.Text = "0"; }
            if (this.txtImporteManoObra.Text == "") { this.txtImporteManoObra.Text = "0"; }
            if (this.txtCosteMaterialesAdicional.Text == "") { this.txtCosteMaterialesAdicional.Text = "0"; }

            if (Completo)
            {
                reparacionDTO.FechaReparacion = FormUtils.GetDateTime(this.txtFechaReparacion);
                reparacionDTO.IdTipoReparacion = FormUtils.GetDecimal((this.cmbTipoReparacion));
                reparacionDTO.IdTipoTiempoManoObra = FormUtils.GetDecimal(this.cmbTiempoManoObra);
                reparacionDTO.ImporteReparacion = Decimal.Parse (this.txtCosteMateriales.Text);
                reparacionDTO.ImporteManoObraAdicional = Decimal.Parse(this.txtImporteManoObra.Text);
                reparacionDTO.ImporteMaterialesAdicional = Decimal.Parse(this.txtCosteMaterialesAdicional.Text);
                //Kintell 11/01/10.
                reparacionDTO.FechaFactura = FormUtils.GetDateTime(this.txtFechaFacturaReparacion);
                reparacionDTO.NumeroFacttura = FormUtils.GetString(this.txtNumeroFacturaReparacion);
                reparacionDTO.CodigoBarrasReparacion = FormUtils.GetString(this.txtCodigoBarrasReparacion);
                reparacionDTO.InformacionRecibida = chkInformacionReparacion.Checked;
            }
            return reparacionDTO;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            try
            {
                 
                if (this.hdnControlFoco.Value != null && this.hdnControlFoco.Value.Length > 0)
                {
                    Page.SetFocus(this.hdnControlFoco.Value);
                }
                
                if (!this.IsPostBack)
                {
                    Boolean Correcto = true;
                    if (Request.QueryString["idVisita"] != null)
                    {
                        lblIdVisita.Text = Request.QueryString["idVisita"];
                        strIdVisita = lblIdVisita.Text;
                    }
                    else
                    {
                        Correcto = false;
                    }
                    if (Request.QueryString["idContrato"] != null)
                    {
                        lblIdContrato.Text = Request.QueryString["idContrato"];
                        strIdContrato = lblIdContrato.Text;
                    }
                    else
                    {
                        Correcto = false;
                    }

                    if (Correcto == true)
                    {
                        try
                        {
                            if (Int16.Parse(lblIdVisita.Text.ToString()) != Int16.Parse(Contrato.ObtenerCodigoUltimaVisita(lblIdContrato.Text).ToString()))
                            {
                                this.btnAceptar.Enabled = false;
                            }
                            else
                            {
                                this.btnAceptar.Enabled = true;
                            }

                            string CodVisita = this.lblIdVisita.Text;
                            string CodContrato = this.lblIdContrato.Text;

                            DeshabilitarControlesInicio(CodVisita, CodContrato);

                            CargarCombos();

                            CargarDatosFormulario(CodContrato,CodVisita);

                        }
                        catch (BaseException ex)
                        {
                            ManageException(ex);
                        }
                    }
                }
                else
                {
                    //TODO revisar qué hay que hacer en este else
                }

                MarcarControlesObligatorios();
                DeshabilitarControles();

                // MMarcos 18/01/10
                if (Visitas.EsEstadoVisitaConsideradaCerrada(cmbEstadoVisita.SelectedValue) && this.txtFechaFactura.Text.Length <= 0)
                {
                    string script = "<script type='text/javascript'>mostrarConfirm = true;</script>";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SCRIPT", script, false);
                }
                else
                {
                    string script = "<script type='text/javascript'>mostrarConfirm = false;</script>";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SCRIPT", script, false);
                }

                if (!IsPostBack)
                {
                    ////No dejamos introducir Reparaciones si desde la última vez en que modifico el estado de la visita, han pasado mas de 7 dias.
                    IDataReader Datos = null;// Visitas.ObtenerFechasMovimiento((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));
                    IDataReader DatosCancelada = null;
                    //while (Datos.Read())
                    //{
                    //    DateTime fecha = (DateTime)DataBaseUtils.GetDataReaderColumnValue(Datos, "fecha");
                    //    if (fecha.AddDays(7) >= DateTime.Now)
                    //    {
                    //        rdbReparacion.Enabled = true;
                    //    }
                    //    else
                    //    {
                    //        rdbReparacion.Enabled = false;
                    //        MostrarMensaje("No puede introducir Reparaciones debido a que desde la última vez en que modifico el estado de la visita, han pasado mas de 7 dias");
                    //    }
                    //    break;
                    //}
                    //Si esta en cancelada por sistema. No dejamos cambiar el estado mas que a cerrada si no han pasado 21 dias desde el cambio.
                    try
                    {
                        if (!string.IsNullOrEmpty(hdnEstadoAnterior.Value.ToString()))
                        {
                            if (hdnEstadoAnterior.Value.ToString() != "")
                            {
                                if (int.Parse(hdnEstadoAnterior.Value.ToString()) == 10)//cmbEstadoVisita.SelectedIndex == 10)
                                {
                                    DatosCancelada = Visitas.ObtenerFechasMovimientoDeCanceladaPorSistema((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));
                                    while (DatosCancelada.Read())
                                    {
                                        DateTime UltimaFecha = (DateTime)DataBaseUtils.GetDataReaderColumnValue(DatosCancelada, "fecha");
                                        if (UltimaFecha.AddDays(29) >= DateTime.Now)
                                        {
                                            //Permitir solo la opción de cerrada.
                                            for (int i = 0; i < cmbEstadoVisita.Items.Count; i++)
                                            {
                                                if (cmbEstadoVisita.Items[i].Value != "02" && cmbEstadoVisita.Items[i].Value != "10") { cmbEstadoVisita.Items[i].Enabled = false; }
                                            }
                                            cmbEstadoVisita.SelectedIndex = 10;
                                        }
                                        else
                                        {
                                            //Eliminar todas las opciones de estado.
                                            cmbEstadoVisita.Enabled = false;
                                            MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_MODIFICAR_MAS_DE_21_DIAS);//"No puede modificar el estado de la visita, debido a que desde la última vez en que modifico el estado de la visita, han pasado mas de 21 dias");
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        //Si esta en cerrada. No dejamos meter reparacion si han pasado + de 3 dias desde el cambio a cerrada.
                        //Habilitado el 13/04/2010.
                        lbl3Dias.Visible = false;
                        if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada)//cmbEstadoVisita.SelectedIndex == 6)// || cmbEstadoVisita.SelectedIndex == 7)
                        {
                            Datos = Visitas.ObtenerFechasMovimientoParaCerradas((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));
                            while (Datos.Read())
                            {
                                DateTime fecha = (DateTime)DataBaseUtils.GetDataReaderColumnValue(Datos, "fecha");
                                if (fecha.AddDays(3) >= DateTime.Now)
                                {
                                    rdbReparacion.Enabled = true;
                                }
                                else
                                {
                                    rdbReparacion.Enabled = false;
                                    //MostrarMensaje("No puede introducir Reparaciones debido a que desde la última vez en que modifico el estado de la visita a cerrada, han pasado mas de 3 dias");
                                    lbl3Dias.Visible = true;
                                }
                                break;
                            }
                        }

                        //Obligamos ha meter datos de la visita al ser 2ª vez que pasa a ausente por ausencia del cliente.
                        //IDataReader DatosAusente = Visitas.ObtenerFechasMovimientoDeClienteAusente((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));
                        int Cont = 0;
                        //while (DatosAusente.Read())
                        //{
                        //    Cont = Cont + 1;
                        //}
                        if (cmbEstadoVisita.SelectedItem.Value == "08")
                        {
                            Cont = 2;
                        }
                        CurrentSession.SetAttribute(CurrentSession.SESSION_VECES_CLIENTE_AUSENTE, Cont);
                    }
                    finally
                    {
                        if(Datos != null)
                        {
                            Datos.Close();
                        }

                        if(DatosCancelada != null)
                        {
                            DatosCancelada.Close();
                        }
                    }
                }

                if (!IsPostBack)
                {
                    //25-03-2010
                    if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada
                        || int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion
                        || int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CanceladaAusentePorSegundaVez)
                        // cmbEstadoVisita.SelectedIndex == 6 || cmbEstadoVisita.SelectedIndex == 7 || cmbEstadoVisita.SelectedIndex == 2)
                    {
                        if (txtNumFactura.Text != "" && txtNumFactura.Text != "V_mayo_2014")
                        {
                            this.chkFacturadoProveedor.Enabled = false;
                            this.chkRecepcionComprobante.Enabled = false;
                            this.lblTipoReparacion.Enabled = false;
                            this.cmbTipoReparacion.Enabled = false;
                            this.lblFechaReparacion.Enabled = false;
                            this.txtFechaReparacion.Enabled = false;
                            this.cmbTiempoManoObra.Enabled = false;
                            this.txtCosteMateriales.Enabled = false;
                            this.txtImporteManoObra.Enabled = false;
                            this.txtCosteMaterialesAdicional.Enabled = false;
                            this.lblTiempoManoObra.Enabled = false;
                            this.lblTiempoManoObraCliente.Enabled = false;
                            this.lblCosteMariales.Enabled = false;
                            this.lblCosteMaterialesCliente.Enabled = false;
                            this.lblServicio.Enabled = false;
                            this.lblAdicionalCliente.Enabled = false;

                            this.txtNumFactura.Enabled = false;
                            this.txtInfoCodigoBarras.Enabled = false;
                            txtFechaFactura.Enabled = false;
                        }
                    }
                }

                
                //Visita en estado pendiente de reparacion, 25/03/2010. Kintell
                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)//cmbEstadoVisita.SelectedIndex == 7)
                {
                    rdbReparacion.Items[0].Selected=true;
                    rdbReparacion.Enabled = false       ;
                }
                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada)//cmbEstadoVisita.SelectedIndex == 6)
                {
                    if (txtNumFactura.Text != "")
                    {
                        this.chkFacturadoProveedor.Enabled = false      ;
                        this.chkRecepcionComprobante.Enabled = false    ;
                        //this.lblTipoReparacion.Enabled = false          ;
                        //this.cmbTipoReparacion.Enabled = false          ;
                        //this.lblFechaReparacion.Enabled = false         ;
                        //this.txtFechaReparacion.Enabled = false         ;
                        //this.cmbTiempoManoObra.Enabled = false          ;
                        //this.txtCosteMateriales.Enabled = false         ;
                        //this.txtImporteManoObra.Enabled = false         ;
                        //this.txtCosteMaterialesAdicional.Enabled = false;
                        //this.lblTiempoManoObra.Enabled = false          ;
                        //this.lblTiempoManoObraCliente.Enabled = false   ;
                        //this.lblCosteMariales.Enabled = false           ;
                        //this.lblCosteMaterialesCliente.Enabled = false  ;
                        //this.lblServicio.Enabled = false                ;
                        //this.lblAdicionalCliente.Enabled = false        ;

                        this.txtNumFactura.Enabled = false              ;
                        this.txtInfoCodigoBarras.Enabled = false        ;
                        txtFechaFactura.Enabled = false                 ;

                        //rdbReparacion.Enabled = false                   ;
                    }

                    if (txtNumeroFacturaReparacion.Text != "")
                    {
                        this.chkFacturadoProveedor.Enabled = false      ;
                        this.chkRecepcionComprobante.Enabled = false    ;
                        this.lblTipoReparacion.Enabled = false          ;
                        this.cmbTipoReparacion.Enabled = false          ;
                        this.lblFechaReparacion.Enabled = false         ;
                        this.txtFechaReparacion.Enabled = false         ;
                        this.cmbTiempoManoObra.Enabled = false          ;
                        this.txtCosteMateriales.Enabled = false         ;
                        this.txtImporteManoObra.Enabled = false         ;
                        this.txtCosteMaterialesAdicional.Enabled = false;
                        this.lblTiempoManoObra.Enabled = false          ;
                        this.lblTiempoManoObraCliente.Enabled = false   ;
                        this.lblCosteMariales.Enabled = false           ;
                        this.lblCosteMaterialesCliente.Enabled = false  ;
                        this.lblServicio.Enabled = false                ;
                        this.lblAdicionalCliente.Enabled = false        ;

                        this.txtNumFactura.Enabled = false              ;
                        this.txtInfoCodigoBarras.Enabled = false        ;
                        txtFechaFactura.Enabled = false                 ;

                        rdbReparacion.Enabled = false                   ;
                    }
                }

                //Si el estado actual es cerrada pendiente de reparación solo se debe permitir modificar a 
                //cerrada si no han pasado más de 60 días desde el cambio de estado a 
                //cerrada pendiente de reparación.
                DateTime fechaLiquidacion;
                DateTime fechaActualMenos20dias;
                DateTime fechaLiquidacionMenos20dias;
                TimeSpan dias = DateTime.Now.Subtract(DateTime.Now); ;
                if (this.txtFechaFactura.Text != "")
                {
                    fechaLiquidacion = DateTime.Parse(this.txtFechaFactura.Text);
                    fechaActualMenos20dias = DateTime.Now.AddDays(-20);
                    fechaLiquidacionMenos20dias = fechaLiquidacion.AddDays(-20);
                    dias = fechaActualMenos20dias.Subtract(fechaLiquidacionMenos20dias);
                }
                if (!IsPostBack)
                {
                    IDataReader Datos = null;
                    try
                    {
                        if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)//cmbEstadoVisita.SelectedIndex == 7)
                        {
                            Datos = Visitas.ObtenerFechasMovimiento((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));
                            while (Datos.Read())
                            {
                                DateTime fecha = (DateTime)DataBaseUtils.GetDataReaderColumnValue(Datos, "fecha");
                                if (fecha.AddDays(60) >= DateTime.Now)
                                {
                                    //SOLO Puede cerrar.
                                    cmbEstadoVisita.Enabled = true;

                                    cmbEstadoVisita.Items[0].Enabled = false;
                                    cmbEstadoVisita.Items[1].Enabled = false;
                                    cmbEstadoVisita.Items[2].Enabled = false;
                                    cmbEstadoVisita.Items[3].Enabled = false;
                                    cmbEstadoVisita.Items[4].Enabled = false;
                                    cmbEstadoVisita.Items[5].Enabled = false;
                                    cmbEstadoVisita.Items[8].Enabled = false;
                                    cmbEstadoVisita.Items[9].Enabled = false;
                                    if (cmbEstadoVisita.Items.Count > 10) { cmbEstadoVisita.Items[10].Enabled = false; }
                                    cmbEstadoVisita.Items[6].Enabled = true;
                                    cmbEstadoVisita.Items[7].Enabled = true;

                                    //string mes = DateTime.Now.AddMonths(0).ToString("MMMM");
                                    //DateTime fechaLiquidacion = DateTime.Parse(this.txtFechaFactura.Text);
                                    //DateTime fechaActualMenos20dias = DateTime.Now.AddDays(-20);
                                    //DateTime fechaLiquidacionMenos20dias = fechaLiquidacion.AddDays(-20);
                                    //TimeSpan dias = fechaActualMenos20dias.Subtract(fechaLiquidacionMenos20dias);
                                    if (this.txtFechaFactura.Text != "")
                                    {
                                        if (dias.Days > 20)
                                        //if (this.txtNumFactura.Text != "V_" + mes + "_2014")
                                        {
                                            //Si estado actual de la visita es cerrada pendiente de reparación y tiene num factura informado 
                                            //deshabilitamos todos los campos de la visita excepto el estado. 
                                            //Que se podrá pasar a cerrada según la validación anterior. No debemos permitir pasarlo a ningún otro estado.

                                            //Deshabilitamos los campos de la visita.
                                            this.txtFechaVisita.Enabled = false;
                                            this.txtObservaciones.Enabled = false;
                                            this.txtFechaFactura.Enabled = false;
                                            this.txtNumFactura.Enabled = false;
                                            this.txtInfoCodigoBarras.Enabled = false;
                                            this.chkFacturadoProveedor.Enabled = false;
                                            txtFechaLimite.Enabled = false;
                                            chkRecepcionComprobante.Enabled = false;
                                            chkFacturadoProveedor.Enabled = false;
                                            chkListCartaEnviada.Enabled = false;
                                            txtFechaEnvioCarta.Enabled = false;
                                        }
                                    }
                                }
                                else
                                {
                                    //NO puede cambiar a ningún estado.
                                    cmbEstadoVisita.Enabled = false;
                                }
                                break;
                            }
                        }
                    }
                    finally
                    {
                        if (Datos != null)
                        {
                            Datos.Close();
                        }
                    }
                }

                //Kintell 03/04/2010
                //Deshabilitar todo si no es la última visita o Cerrada - Pendiente realizar reparación (que solo pueda ver)
                //Y solo Adico puede....
                //el perfil distinto de adico no puede modificar ninguna vista que no sea la última. 
                //El de adico la ultima y de las anteriores la que este en cerrada pendiente de reparación.
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                if (!IsPostBack)
                {
                    //if (Request.QueryString["TotalVisitas"] != Request.QueryString["idVisita"] && cmbEstadoVisita.SelectedIndex != 7)
                    //{
                    //    if (usuarioDTO.NombreProveedor != "ADICO")
                    //    {
                    //        //Deshabilitar.
                    //        this.panel1.Enabled = false;
                    //        // deshabilitamos además los controles del panel, ya que en realidad
                    //        // estos no son hijos suyos por problemas de maquetación.

                    //        this.cmbEstadoVisita.Enabled = false;
                    //        this.txtFechaVisita.Enabled = false;
                    //        this.txtObservaciones.Enabled = false;
                    //        this.txtFechaFactura.Enabled = false;
                    //        this.txtNumFactura.Enabled = false;
                    //        this.txtInfoCodigoBarras.Enabled = false;
                    //        this.chkFacturadoProveedor.Enabled = false;
                    //        this.rdbReparacion.Items[0].Selected = true;
                    //        this.rdbReparacion.Enabled = false;
                    //        this.btnEquipamiento.Enabled = false;
                    //        this.lblTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                    //        this.cmbTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                    //        this.txtFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                    //        this.cmbTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                    //        this.txtCosteMateriales.Enabled = this.rdbReparacion.Enabled;
                    //        this.txtImporteManoObra.Enabled = this.rdbReparacion.Enabled;
                    //        this.txtCosteMaterialesAdicional.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblTiempoManoObraCliente.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblCosteMariales.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblCosteMaterialesCliente.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblServicio.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblAdicionalCliente.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblFactura.Enabled = this.rdbReparacion.Enabled;
                    //        this.chkListCartaEnviada.Enabled = false;
                    //        //this.txtFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                    //        this.txtNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                    //        this.txtCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
                    //        this.lblCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
                    //    }
                    //}

                    if (Request.QueryString["TotalVisitas"] != null)
                    {
                        String connectionString = WebConfigurationManager.ConnectionStrings["OPCOMCMD"].ConnectionString;
                        SqlConnection con = new SqlConnection(connectionString);
                        SqlCommand cmd = new SqlCommand("select count(*) from visita where cod_contrato in (select cod_contrato_sic from relacion_cups_contrato where cod_receptor in (select cod_receptor from relacion_cups_contrato where cod_contrato_sic='" + (string)this.lblIdContrato.Text + "'))", con);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //' Fill the DataSet.
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "UltimaVisita");

                        if (Request.QueryString["TotalVisitas"] != ds.Tables[0].Rows[0].ItemArray[0].ToString())//equest.QueryString["idVisita"])
                        {
                            if (usuarioDTO.NombreProveedor != "ADICO")
                            {
                                //Deshabilitar.
                                this.panel1.Enabled = false;
                                // deshabilitamos además los controles del panel, ya que en realidad
                                // estos no son hijos suyos por problemas de maquetación.

                                this.cmbEstadoVisita.Enabled = false;
                                this.txtFechaVisita.Enabled = false;
                                this.txtObservaciones.Enabled = false;
                                this.txtFechaFactura.Enabled = false;
                                this.txtNumFactura.Enabled = false;
                                this.txtInfoCodigoBarras.Enabled = false;
                                this.chkFacturadoProveedor.Enabled = false;
                                this.rdbReparacion.Items[0].Selected = true;
                                this.rdbReparacion.Enabled = false;
                                this.btnEquipamiento.Enabled = false;
                                this.lblTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                                this.cmbTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                                this.lblFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                                this.txtFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                                this.cmbTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                                this.txtCosteMateriales.Enabled = this.rdbReparacion.Enabled;
                                this.txtImporteManoObra.Enabled = this.rdbReparacion.Enabled;
                                this.txtCosteMaterialesAdicional.Enabled = this.rdbReparacion.Enabled;
                                this.lblTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                                this.lblTiempoManoObraCliente.Enabled = this.rdbReparacion.Enabled;
                                this.lblCosteMariales.Enabled = this.rdbReparacion.Enabled;
                                this.lblCosteMaterialesCliente.Enabled = this.rdbReparacion.Enabled;
                                this.lblServicio.Enabled = this.rdbReparacion.Enabled;
                                this.lblAdicionalCliente.Enabled = this.rdbReparacion.Enabled;
                                this.lblFactura.Enabled = this.rdbReparacion.Enabled;
                                this.chkListCartaEnviada.Enabled = false;
                                //this.txtFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                                this.txtNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                                this.lblFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                                this.lblNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                                this.txtCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
                                this.lblCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
                            }
                            else
                            {
                                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)//cmbEstadoVisita.SelectedIndex != 7)
                                {
                                    //Deshabilitar.
                                    this.panel1.Enabled = false;
                                    // deshabilitamos además los controles del panel, ya que en realidad
                                    // estos no son hijos suyos por problemas de maquetación.

                                    this.cmbEstadoVisita.Enabled = false;
                                    this.txtFechaVisita.Enabled = false;
                                    this.txtObservaciones.Enabled = false;
                                    this.txtFechaFactura.Enabled = false;
                                    this.txtNumFactura.Enabled = false;
                                    this.txtInfoCodigoBarras.Enabled = false;
                                    this.chkFacturadoProveedor.Enabled = false;
                                    this.rdbReparacion.Items[0].Selected = true;
                                    this.rdbReparacion.Enabled = false;
                                    this.btnEquipamiento.Enabled = false;
                                    this.lblTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                                    this.cmbTipoReparacion.Enabled = this.rdbReparacion.Enabled;
                                    this.lblFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                                    this.txtFechaReparacion.Enabled = this.rdbReparacion.Enabled;
                                    this.cmbTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                                    this.txtCosteMateriales.Enabled = this.rdbReparacion.Enabled;
                                    this.txtImporteManoObra.Enabled = this.rdbReparacion.Enabled;
                                    this.txtCosteMaterialesAdicional.Enabled = this.rdbReparacion.Enabled;
                                    this.lblTiempoManoObra.Enabled = this.rdbReparacion.Enabled;
                                    this.lblTiempoManoObraCliente.Enabled = this.rdbReparacion.Enabled;
                                    this.lblCosteMariales.Enabled = this.rdbReparacion.Enabled;
                                    this.lblCosteMaterialesCliente.Enabled = this.rdbReparacion.Enabled;
                                    this.lblServicio.Enabled = this.rdbReparacion.Enabled;
                                    this.lblAdicionalCliente.Enabled = this.rdbReparacion.Enabled;
                                    this.lblFactura.Enabled = this.rdbReparacion.Enabled;
                                    this.chkListCartaEnviada.Enabled = false;
                                    //this.txtFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                                    this.txtNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                                    this.lblFechaFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                                    this.lblNumeroFacturaReparacion.Enabled = this.rdbReparacion.Enabled;
                                    this.txtCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
                                    this.lblCodigoBarrasReparacion.Enabled = this.rdbReparacion.Enabled;
                                }
                            }
                        }
                    }
                }

                if (!IsPostBack)
                {
                    if (cmbEstadoVisita.Enabled == false)
                    {
                        cboTecnico.Enabled = false;
                        cboTipoVisita.Enabled = false;
                        txtTecnico.Enabled = false;
                        txtEmpresa.Enabled = false;
                        // Comentado el 24/09/2012 debido a que no les dejaría ver los comentarios de los técnicos 
                        // una vez cargados estos mediante el proceso BATCH.
                        //txtObservacionesVisita.Enabled = false;
                    }
                }
                //string mes1 = DateTime.Now.AddMonths(0).ToString("MMMM");
                //fechaLiquidacion = DateTime.Parse(this.txtFechaFactura.Text);
                //fechaActualMenos20dias = DateTime.Now.AddDays(-20);
                //fechaLiquidacionMenos20dias = fechaLiquidacion.AddDays(-20);
                //dias = fechaActualMenos20dias.Subtract(fechaLiquidacionMenos20dias);
                if (this.txtFechaFactura.Text != "")
                {
                    if (dias.Days > 20)
                    //if (txtNumFactura.Text != "" && txtNumFactura.Text != "V_" + mes1 + "_2014")
                    {
                        cmbEstadoVisita.Enabled = false;
                    }
                }

                //20211011 BUA ADD R#34195 - Ticket de combustión priorización anomalía principal o secundaria
                //Si la visita que se esta cerrando tiene asociada una solicitud de averia abierta(Creada por anomalia a la hora de procesar el ticket) se valida que que la visita se encuentra en estado "visita erronea"
                PValidacionesTicketCombustionDB pValidacionesTicketCombustionDB = new PValidacionesTicketCombustionDB();
                DataTable dtSol = pValidacionesTicketCombustionDB.ObtenerSolicitudAveriaAsociadaVisita((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));

                Visitas visitas = new Visitas();
                VisitaDTO visitasDTO = new VisitaDTO();
                visitasDTO = visitas.DatosVisitas((string)this.lblIdContrato.Text, this.lblIdVisita.Text);

                //Si la visita se encuentra en estado visitaErronea y a su vez tiene un solicitud de averia abierta no se le permite cambiar el estado de la visita, ya que la propia solicitud pondra en estado cerrada la visis cuando se solucione la averia.
                if ((int.Parse(visitasDTO.CodigoEstadoVisita) == (int)Enumerados.EstadosVisita.visitaErronea
                        && int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.visitaErronea)
                    || 
                    (int.Parse(visitasDTO.CodigoEstadoVisita) == (int)Enumerados.EstadosVisita.visitaErronea
                        && int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.visitaErronea))
                {
                    if (dtSol != null && dtSol.Rows.Count > 0)
                    {
                        cmbEstadoVisita.SelectedValue = visitasDTO.CodigoEstadoVisita;

                        cmbEstadoVisita.Enabled = false;
                        btnAceptar.Enabled = false;
                        //MostrarMensaje("Se ha producido una " + cboTipoVisita.SelectedValue + ". No se puede modificar el mantenimiento hasta que se resuelva la averia generada.");
                        //return;
                    }
                    else
                        cmbEstadoVisita.Enabled = true;
                }

                //Si la visita se encuentra en estado visitaErrones y a su vez tiene un solicitud de averia abierta no se le permite cambiar el estado de la visita, ya que la propia solicitud pondra en estado cerrada la visis cuando se solucione la averia.
                //if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.visitaErronea)
                //{
                //    if (dtSol != null && dtSol.Rows.Count > 0)
                //    {
                //        cmbEstadoVisita.Enabled = false;
                //        btnAceptar.Enabled = false;
                //    }
                //    else
                //        cmbEstadoVisita.Enabled = true;
                //}
                //20211011 BUA END R#34195 - Ticket de combustión priorización anomalía principal o secundaria


                ////21/09/2016 Activamos el fichero si esta en "cerrada" o en "cerrada. Pdte. de reparacion".
                //string scriptFichero = "";
                //if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada
                //    || int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)
                ////(cmbEstadoVisita.SelectedIndex == 6 || cmbEstadoVisita.SelectedIndex == 7))// || cmbEstadoVisita.SelectedIndex == 2))
                //{
                //    CustomValidator1.Enabled = true;
                //    ////04052016 Tema subir fichero
                //    //scriptFichero = "<script>document.getElementById('trFichero').style.visibility = 'visible';</script>";
                //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "ACTIVARSUBIRFICHERO", scriptFichero, false);
                //    ////lblFichero.Visible = true;
                //    ////flFicheroVisita.Visible = true;

                //}
                //else
                //{
                //    CustomValidator1.Enabled = false;
                //    ////04052016 Tema subir fichero
                //    //scriptFichero = "<script>document.getElementById('trFichero').style.visibility = 'hidden';</script>";
                //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "DESACTIVARSUBIRFICHERO", scriptFichero, false);
                //    ////lblFichero.Visible = false;
                //    ////flFicheroVisita.Visible = false;
                //}
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

        protected void OnBtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean Correcto = true;
                if (this.cboTecnico.SelectedIndex < 0 && (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada || int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion))
                    //cmbEstadoVisita.SelectedIndex == 6 || cmbEstadoVisita.SelectedIndex == 7))
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_TECNICO);//"Debe seleccionar un técnico");
                    return;
                }

                if (this.cboTipoVisita.SelectedValue == "0" && (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada || int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion))//(cmbEstadoVisita.SelectedIndex == 6 || cmbEstadoVisita.SelectedIndex == 7))// && rdbReparacion.Items[1].Selected)
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_TIPO_VISITA);//"Debe seleccionar un Tipo de Visita");
                    return;
                }

                //20210505 BRU R#29497 - Guardar el Ticket de combustión Fase 6, datos por Web
                //Si la visita actual se encuntra en estado visitaCerrada por anomalia critica y NO tiene pendiente una solicitud de averia, solo se puede pasar a estado cerrada.
                Visitas visitas = new Visitas();

                VisitaDTO visitasDTO = new VisitaDTO();
                visitasDTO = visitas.DatosVisitas((string)this.lblIdContrato.Text, this.lblIdVisita.Text);

                PValidacionesTicketCombustionDB pValidacionesTicketCombustionDB = new PValidacionesTicketCombustionDB();
                DataTable dtSol = pValidacionesTicketCombustionDB.ObtenerSolicitudAveriaAsociadaVisita((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));

                //Si la visita se encuentra en estado visitaErronea y a su vez tiene un solicitud de averia abierta no se le permite cambiar el estado de la visita, ya que la propia solicitud pondra en estado cerrada la visis cuando se solucione la averia.
                if (int.Parse(visitasDTO.CodigoEstadoVisita) == (int)Enumerados.EstadosVisita.visitaErronea
                    && int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.Cerrada)
                {
                    if (dtSol != null && dtSol.Rows.Count == 0)
                    {
                        MostrarMensaje("Solo se permite pasar a estado Cerrada.");
                        return;
                    }
                    else if(dtSol != null && dtSol.Rows.Count > 0)
                    {
                        MostrarMensaje("Se ha producido una " + cboTipoVisita.SelectedValue + ". Existe la averia " + dtSol.Rows[0]["ID_solicitud"].ToString() + ", que debe ser resuelta.");
                        return;
                    }
                }

                PValidacionesTicketCombustion pValidacionesTicketCombustion = new PValidacionesTicketCombustion();
                string msgErrorTicket = string.Empty;
                int estadoCambiaVisita = int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value);

                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                //Comprobamos si tiene que tener ticket de combustion
                Boolean isDebeTenerTicket = pValidacionesTicketCombustion.HayQueProcesarTicketCombustionDesdeWEB_Visita(usuarioDTO
                                                                                                                        , lblIdContrato.Text
                                                                                                                        , lblIdVisita.Text
                                                                                                                        , estadoCambiaVisita
                                                                                                                        , ref msgErrorTicket);

                //Antes de guardar verificamos si le corresponde Ticket de combustion. De ser obligatorio mostramos la pantalla para insertar los datos.
                if (isDebeTenerTicket)
                {
                    String strUrl = "./FrmModalTicketCombustion.aspx" + "?COD_CONTRATO=" + lblIdContrato.Text + "&COD_VISITA=" + lblIdVisita.Text + "&EDICION=true&TIPO_VISITA=" + cboTipoVisita.SelectedValue;
                    this.OpenWindow(strUrl, "ModalTicketCombustion", "Ticket de combustion");

                    return;
                }

                /*
                Visitas visita = new Visitas();
                VisitaDTO visitasDTO = null;

                visitasDTO = visita.DatosVisitas(lblIdContrato.Text, lblIdVisita.Text);

                if (int.Parse(visitasDTO.CodigoEstadoVisita) == (int)Enumerados.EstadosVisita.visitaErronea)
                {
                    //CaracteristicaHistoricoDB caracteristicaHistoricoDB = new CaracteristicaHistoricoDB();
                    //string idSol = caracteristicaHistoricoDB.GetCaracteristicaNumeroSolicitudByCodContratoCodVisitaAndTipCar(codContrato, codVisita, "188");
                    //if (!string.IsNullOrEmpty(idSol))
                    //{
                    //}

                    MostrarMensaje("No se puede modificar la visita por encontrarse en estado visitaErronea. Consulte la solcitud de averia creada."); //"Visita en estado visitaErronea");
                    return;
                }
                */

                //END

                Int32 nContadorVisDiaTecnico = 0;
                if (this.txtFechaVisita.Text != String.Empty && this.cboTecnico.SelectedValue.ToString() != "")
                {
                    IDataReader drContVisitas =null;
                    try
                    {
                        drContVisitas = visitas.ObtenerContador_Visitas_Dia_por_Tecnico(Int16.Parse(this.cboTecnico.SelectedValue.ToString()), DateTime.Parse(this.txtFechaVisita.Text));


                        while (drContVisitas.Read())
                        {
                            nContadorVisDiaTecnico = Int32.Parse(DataBaseUtils.GetDataReaderColumnValue(drContVisitas, "CONTADOR_VISITA_DIA").ToString());
                        }
                    }
                    finally
                    {
                        if(drContVisitas!=null)
                        { 
                            drContVisitas.Close(); 
                        }
                    }


                    this.hdnTecnicoValido.Value = (string)nContadorVisDiaTecnico.ToString();

                    if (this.hdnTecnicoValido.Value != "")
                    {
                        ConfiguracionDTO confNumeroVisitasCierreDia = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.NUMERO_VISITAS_CIERRE_DIA);
                        
                        if (Int32.Parse(this.hdnTecnicoValido.Value.ToString()) >= int.Parse(confNumeroVisitasCierreDia.Valor))//12)
                        {
                            MostrarMensaje(Resources.TextosJavaScript.TEXTO_TECNICO_SUPERA_LIMITE_VISITAS_DIA);//"Este técnico ha alcanzado el límite de 12 visitas/día .");
                            return;
                        }
                    }

                }
                //else
                //{
                //    MostrarMensaje("Debe indicar la fecha de la visita antes de seleccionar el técnico."); 
                //}

                
                if (this.ValidatePage())
                {
                    if ((int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada || int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion))//cmbEstadoVisita.SelectedIndex == 6 || cmbEstadoVisita.SelectedIndex == 7)
                    {
                        //No se debe permitir cambiar a cerrada (o cerrada pendiente de reparación) o guardar los cambios 
                        //si la visita esta en alguno de los dos estados y esta sin rellenar la recepción del comprobante, el check de facturado proveedor, el código de barras y la fecha de factura.
                        
                        //Y la caldera debe de estar seleccionada.
                        CalderaDTO caldera = Calderas.ObtenerCaldera(lblIdContrato.Text.ToString());
                        
                        if (caldera == null || this.chkFacturadoProveedor.Checked == false || this.chkRecepcionComprobante.Checked == false || this.txtFechaFactura.Text == "" || txtInfoCodigoBarras.Text == "")
                        {
                            MostrarMensaje(Resources.TextosJavaScript.TEXTO_COMPRUEBE_DATOS_OBLIGATORIOS);//"Compruebe: La recepción del comprobante, El código de barras de la visita, El check de facturado proveedor, La fecha de factura y La caldera seleccionada");
                        }
                        else
                        {
                            ModificarVisita();
                            if (this.rdbReparacion.Items[1].Selected)
                            {
                                if (this.hdnIdReparacion.Value.Equals(String.Empty) || this.hdnIdReparacion.Value.Equals("0")) //Ahora hay reparacion y antes no
                                {
                                    InsertarReparacion();
                                }
                                else //Ahora hay reparacion y antes también.
                                {
                                    ModificarReparacion();
                                }
                            }
                            else
                            {
                                if (!this.hdnIdReparacion.Value.Equals(String.Empty)) //Ahora no hay reparacion y antes sí
                                {
                                    BorrarReparacion();
                                }
                            }
                            //this.SaveSessionData();
                            //NavigationController.Forward("./FrmDetalleContrato.aspx?idContrato=" + this.strIdContrato);


                            // GUARDAR FICHERO
                            // 04/05/2016
                            // COMPROBAMOS QUE EXISTA EL FICHERO.
                            //if (flFicheroVisita.HasFile)
                            //{
                            //    // COMPROBAMOS NOMBRE DEL FICHERO.
                            //    if (ComprobarNombreFichero(flFicheroVisita.FileName))
                            //    {
                            //        // COMPROBAMOS EL TAMAÑO (TOPE DE 4 MEGAS).
                            //        int maxSize = 4097151;
                            //        int minSize = 0;
                            //        if (flFicheroVisita.PostedFile.ContentLength >= minSize && flFicheroVisita.PostedFile.ContentLength <= maxSize)
                            //        {
                            //            // COMPROBAMOS QUE NO EXISTA UN FICHERO CON ESE NOMBRE EN LA BB.DD.
                            //            Int32 regFicheros = Documentos.ObtenerRegistroFicherosPorNombre(flFicheroVisita.FileName);
                            //            if (regFicheros <= 0)
                            //            {
                            //                SubirFichero();
                            //            }
                            //            else
                            //            {
                            //                MostrarMensaje(Resources.TextosJavaScript.TEXTO_FICHERO_EXISTENTE_BBDD);//"Fichero ya existente en la BB.DD.");
                            //                return;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MostrarMensaje(Resources.TextosJavaScript.TEXTO_TAMANIO_FICHERO_INCORRECTO);//"Tamaño del fichero incorrecto");
                            //            return;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        MostrarMensaje(Resources.TextosJavaScript.TEXTO_NOMBREFICHERO_INCORRECTO);//"Nombre del fichero incorrecto");
                            //        return;
                            //    }
                            //}
                            //else
                            //{
                            //   // De momento no lo ponemos obligatorio (17/05/2016).
                            //    MostrarMensaje(Resources.TextosJavaScript.TEXTO_FALTA_FICHERO_OBLIGATORIO);//"Falta el fichero obligatorio");
                            //    return;
                            //}


                            if (Correcto)
                            {
                                this.DeleteSessionData();
                                NavigationController.Backward();
                            }
                        }
                    }
                    else
                    {
                        if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CanceladaAusentePorSegundaVez)//cmbEstadoVisita.SelectedIndex == 2)
                        {
                            //Si es 2ª vez que pone cancelada por cliente ausente obligamos a meter:
                            //fecha visita, recepcion del comprobante fecha factura visita y codigo de barras de la visita.
                            int Cont =int.Parse ( CurrentSession.GetAttribute(CurrentSession.SESSION_VECES_CLIENTE_AUSENTE).ToString ());

                            if (Cont > 0)
                            {
                                //Si es 2ª vez que pone cancelada por cliente ausente obligamos a meter:
                                //fecha visita, recepcion del comprobante fecha factura visita y codigo de barras de la visita.
                                if (txtFechaVisita.Text == "" || chkRecepcionComprobante.Checked == false || txtFechaFactura.Text == "" || txtInfoCodigoBarras.Text == "")// || txtNumFactura.Text == "")
                                {
                                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_SEGUNDA_VEZ_AUSENTE);
                                }
                                else
                                {
                                    ModificarVisita();
                                    if (this.rdbReparacion.Items[1].Selected)
                                    {
                                        if (this.hdnIdReparacion.Value.Equals(String.Empty) || this.hdnIdReparacion.Value.Equals("0")) //Ahora hay reparacion y antes no
                                        {
                                            InsertarReparacion();
                                        }
                                        else //Ahora hay reparacion y antes también.
                                        {
                                            ModificarReparacion();
                                        }
                                    }
                                    else
                                    {
                                        if (!this.hdnIdReparacion.Value.Equals(String.Empty)) //Ahora no hay reparacion y antes sí
                                        {
                                            BorrarReparacion();
                                        }
                                    }
                                    //this.SaveSessionData();
                                    //NavigationController.Forward("./FrmDetalleContrato.aspx?idContrato=" + this.strIdContrato);

                                    if (Correcto)
                                    {
                                        this.DeleteSessionData();
                                        NavigationController.Backward();
                                    }
                                }
                            }
                            else
                            {
                                ModificarVisita();
                                if (this.rdbReparacion.Items[1].Selected)
                                {
                                    if (this.hdnIdReparacion.Value.Equals(String.Empty) || this.hdnIdReparacion.Value.Equals("0")) //Ahora hay reparacion y antes no
                                    {
                                        InsertarReparacion();
                                    }
                                    else //Ahora hay reparacion y antes también.
                                    {
                                        ModificarReparacion();
                                    }
                                }
                                else
                                {
                                    if (!this.hdnIdReparacion.Value.Equals(String.Empty)) //Ahora no hay reparacion y antes sí
                                    {
                                        BorrarReparacion();
                                    }
                                }
                                //this.SaveSessionData();
                                //NavigationController.Forward("./FrmDetalleContrato.aspx?idContrato=" + this.strIdContrato);

                                if (Correcto)
                                {
                                    this.DeleteSessionData();
                                    NavigationController.Backward();
                                }
                            }
                        }
                        else
                        {
                            ModificarVisita();
                            if (this.rdbReparacion.Items[1].Selected)
                            {
                                if (this.hdnIdReparacion.Value.Equals(String.Empty) || this.hdnIdReparacion.Value.Equals("0")) //Ahora hay reparacion y antes no
                                {
                                    InsertarReparacion();
                                }
                                else //Ahora hay reparacion y antes también.
                                {
                                    ModificarReparacion();
                                }
                            }
                            else
                            {
                                if (!this.hdnIdReparacion.Value.Equals(String.Empty)) //Ahora no hay reparacion y antes sí
                                {
                                    BorrarReparacion();
                                }
                            }
                            //this.SaveSessionData();
                            //NavigationController.Forward("./FrmDetalleContrato.aspx?idContrato=" + this.strIdContrato);

                            if (Correcto)
                            {
                                this.DeleteSessionData();
                                NavigationController.Backward();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data, "Error al grabar información.");
                ManageException(ex);
            }
        }
        
        private Boolean ComprobarNombreFichero(String nombre)
        {
            // 04/05/2016
            // Formato=CCBB_COD-CONTRATO_TIPO-DOCUMENTO
            // QUITAMOS LA EXTENSION.
            nombre = nombre.Substring(0, nombre.Length - 4);
            string[] sNombreFichero = nombre.Split('_');
            // COMPROBAMOS QUE TENGA 3 PARAMETROS EN EL NOMBRE.
            if (sNombreFichero.Length == 3)
            {
                // COMPROBAMOS QUE EL PARAMETRO DEL CCBB TENGA 13 O 14 CARACTERES.
                if (sNombreFichero[0].Length == 14 || sNombreFichero[0].Length == 13)
                {
                    // COMPROBAMOS QUE EL PARAMETRO DEL CONTRATO TENGA 10 CARACTERES.
                    if (sNombreFichero[1].Length == 10)
                    {
                        // COMPROBAMOS QUE EL PARAMETRO DEL TIPO DEL DOCUMENTO SEA ALGUNO DE LOS ACEPTADOS.
                        if (
                            sNombreFichero[2] == StringEnum.GetStringValue(Enumerados.TipoDocumentoAnexarfichero.InformeRevisionPorPrecinte) ||
                            sNombreFichero[2] == StringEnum.GetStringValue(Enumerados.TipoDocumentoAnexarfichero.InformesMantenimientoGAS) ||
                            sNombreFichero[2] == StringEnum.GetStringValue(Enumerados.TipoDocumentoAnexarfichero.SMG_Averias) ||
                            sNombreFichero[2] == StringEnum.GetStringValue(Enumerados.TipoDocumentoAnexarfichero.SMG_Reparaciones)
                            )                        
                        {

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void SubirFichero()
        {
            try
            {
                // 04/05/2016
                // SUBIMOS EL FICHERO A LOCAL PARA LUEGO PODER MOVERLO A LA RUTA DEFINITIVA, PORQUE SI NO, DA ERROR DE PERMISOS.
                String Destino = (string)CurrentSession.GetAttribute(CurrentSession.SESSION_RUTA_DOCUMENTO_CIERRE_VISITAS);// ConfigurationManager.AppSettings.Get("RUTA_DOCUMENTOS");
                string rutaFichero = Server.MapPath("~/Excel") + "\\";
                flFicheroVisita.PostedFile.SaveAs(rutaFichero + flFicheroVisita.FileName);
                // COPIAMOS EL FICHERO A LA RUTA DEFINITIVA, NO LO MOVEMOS PORQUE NO DEJA POR TEMA DE PERMISOS, SOLO DEJA COPIARLO.
                FileUtils.FileCopy(ManejadorFicheros.GetImpersonator(), rutaFichero + flFicheroVisita.FileName, Destino + flFicheroVisita.FileName, false);
                // INSERTAMOS REGISTRO EN LA BB.DD.
                String Login = CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_USUARIO_VALIDO).ToString();
                // SI HEMOS LLEGADO AKI, SIGNIFICA QUE EL NOMBRE DEL FICHERO ESTA BIEN.
                // Formato=CCBB_COD-CONTRATO_TIPO-DOCUMENTO
                string[] sNombreFichero = flFicheroVisita.FileName.Split('_');
                int tipoDocumento = Int32.Parse(sNombreFichero[2].ToString().Substring(0, sNombreFichero[2].ToString().Length - 4)); //Int16.Parse(sNombreFichero[2].ToString());
                
                //20210119 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                //Documentos.InsertaDocumentoCargado(lblIdContrato.Text, int.Parse(lblIdVisita.Text), 0, flFicheroVisita.FileName, tipoDocumento, null, null, Login);
                DocumentoDTO documentoDto = new DocumentoDTO();
                documentoDto.CodContrato = lblIdContrato.Text;
                documentoDto.CodVisita = int.Parse(lblIdVisita.Text);
                documentoDto.NombreDocumento = flFicheroVisita.FileName;
                documentoDto.IdTipoDocumento = tipoDocumento;
                documentoDto.EnviarADelta = true;
                documentoDto = Documento.Insertar(documentoDto, Login);
                //20210119 MOD ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital 
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void ModificarVisita()
        {
            String Login = CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_USUARIO_VALIDO).ToString();
            UsuarioDTO usuario = Usuarios.ObtenerUsuario(Login);
            Login = usuario.Nombre.ToString();
            VisitaDTO visitaDTO = RecuperarInformacionVisitaFormulario();
            Visitas.ActualizarDatosVisita(visitaDTO, hdnEstadoAnteriorVisita.Value != visitaDTO.CodigoEstadoVisita,Login);
        }


        private void ModificarReparacion()
        {
            ReparacionDTO reparacionDTO = RecuperarInformacionReparacionFormulario(true);
            Reparacion.ActualizarReparacion(reparacionDTO);
        }
         

        private void InsertarReparacion()
        {
            ReparacionDTO reparacionDTO = RecuperarInformacionReparacionFormulario(true);
            Reparacion.InsertarReparacion(reparacionDTO);
        }

        private void BorrarReparacion()
        {
            ReparacionDTO reparacionDTO = RecuperarInformacionReparacionFormulario(false);
            Reparacion.BorrarReparacion(reparacionDTO);
        }

        protected void OnBtnEquipamiento_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (lblIdContrato.Text == null || lblIdContrato.Text.Length == 0)
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_HAY_CODIGO_CONTRATO);//"No hay código de contrato");
                }
                else
                {
                    String strUrl = "./FrmModalEquipamiento.aspx" + "?COD_CONTRATO=" + lblIdContrato.Text;
                    this.OpenWindow(strUrl, "ModalDetalleEquipamiento", "Detalle Equipamiento");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void chkListCartaEnviada_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                if (usuarioDTO.Id_Perfil == 4)
                {
                    if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CanceladaClienteNoLocalizable)//cmbEstadoVisita.SelectedIndex == 4)
                    {
                        if (this.chkListCartaEnviada.Items[0].Selected)
                        {
                            this.txtFechaEnvioCarta.Text = System.DateTime.Now.ToString();
                            this.txtFechaEnvioCarta.Text = this.txtFechaEnvioCarta.Text.Substring(0, 10);
                        }
                        else
                        {
                            this.txtFechaEnvioCarta.Text = "";
                        }
                    }
                    else
                    {
                        this.chkListCartaEnviada.Items[0].Enabled = false;
                        this.chkListCartaEnviada.Items[0].Selected = false;
                        this.txtFechaEnvioCarta.Text = "";
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_SE_PERMITE_ENVIO_CARTA);//"No se permite seleccionar envio de carta con este estado de la visita");
                    }
                }
                else
                {
                    this.chkListCartaEnviada.Items[0].Enabled = false;
                    this.chkListCartaEnviada.Items[0].Selected = false;
                    this.txtFechaEnvioCarta.Text = "";
                    //MostrarMensaje("No tiene permisos necesarios para seleccionar el envio de carta");
                }

            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnCmbEstadoVisita_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ComboBoxHasValue(cmbEstadoVisita, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void cmbEstadoVisita_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cmbEstadoVisitaCustomValidator.Validate();

                //Kintell 13/01/10
                if ((int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada || int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion) && txtNumFactura.Text =="")
                //cmbEstadoVisita.SelectedIndex == 6 || cmbEstadoVisita.SelectedIndex == 7) 
                {
                    this.chkFacturadoProveedor.Enabled = true;
                }
                else
                {
                    this.chkFacturadoProveedor.Enabled = false;
                }

                AppPrincipal usuarioPrincipal = (AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                if (usuarioDTO.Id_Perfil == 4)
                {
                    this.chkListCartaEnviada.Items[0].Enabled = true;
                    if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CanceladaClienteNoLocalizable)//cmbEstadoVisita.SelectedIndex == 4)
                    {
                        this.chkListCartaEnviada.Items[0].Enabled = true;
                    }
                    else
                    {
                        this.chkListCartaEnviada.Items[0].Enabled = false;
                    }
                }
                else
                {
                    this.chkListCartaEnviada.Items[0].Enabled = false;
                }


                //int Cont =int.Parse ( CurrentSession.GetAttribute(CurrentSession.SESSION_VECES_CLIENTE_AUSENTE).ToString ());

                //if (Cont > 0)
                //{
                //    if (cmbEstadoVisita.SelectedIndex == 4)
                //    {
                //        this.chkRecepcionComprobante.Enabled = true;
                //    }
                //}
                // Se resetea el valor
                hdnConfirm.Value = "true";

                //Kintell 03/04/2010
                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CanceladaAusentePorSegundaVez)//cmbEstadoVisita.SelectedIndex == 2)
                {
                    this.chkFacturadoProveedor.Enabled = true;
                    this.chkRecepcionComprobante.Enabled = true;
                }
                string script = "";
                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada || int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)//(cmbEstadoVisita.SelectedIndex == 6 || cmbEstadoVisita.SelectedIndex == 7))// || cmbEstadoVisita.SelectedIndex == 2))
                {
                    CustomValidator1.Enabled = true;
                    //04052016 Tema subir fichero
                    //script = "<script>document.getElementById('trFichero').style.visibility = 'visible';</script>";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "ACTIVARSUBIRFICHERO", script, false);
                    //lblFichero.Visible = true;
                    //flFicheroVisita.Visible = true;
                    //flFicheroVisita.Enabled = true;
                }
                else
                {
                    CustomValidator1.Enabled = false;
                    //04052016 Tema subir fichero
                    //script = "<script>document.getElementById('trFichero').style.visibility = 'hidden';</script>";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "DESACTIVARSUBIRFICHERO", script, false);
                    //lblFichero.Visible = false;
                    //flFicheroVisita.Visible = false;
                    //flFicheroVisita.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnCmbTipoReparacion_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (this.rdbReparacion.Items[1].Selected)
                {
                    e.IsValid = FormValidation.ComboBoxHasValue(cmbTipoReparacion, (BaseValidator)sender);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void cmbTipoReparacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    this.CmbTipoReparacionCustomValidator.Validate();
            //}
            //catch (Exception ex)
            //{
            //    ManageException(ex);
            //}
        }

        protected void OnTiempoManoObra_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {

                if (this.rdbReparacion.Items[1].Selected)
                {
                    e.IsValid = FormValidation.ComboBoxHasValue(cmbTiempoManoObra, (BaseValidator)sender);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void cmbTiempoManoObra_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    this.TiempoManoObraCustomValidator.Validate();
            //}
            //catch (Exception ex)
            //{
            //    ManageException(ex);
            //}
        }


        protected void OnTxtFechaFactura_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFechaFactura, false, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtFechaFactura_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtFechaFacturaCustomValidator.Validate();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtFechaVisita_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                String strEstadoVisita = "";
                if (FormUtils.HasValue(cmbEstadoVisita))
                {
                    strEstadoVisita = this.cmbEstadoVisita.SelectedItem.Value;
                }

                if (Visitas.EsEstadoVisitaConsideradaCerrada(strEstadoVisita))
                {
                    e.IsValid = FormValidation.ValidateDateTextBox(txtFechaVisita, true, (BaseValidator)sender);
                }
                else
                {
                    e.IsValid = FormValidation.ValidateDateTextBox(txtFechaVisita, false, (BaseValidator)sender);
                }

                // además comprobamos que la fecha no sea mayor que hoy
                if (e.IsValid && txtFechaVisita.Text.Length > 0)
                {
                    DateTime fechaVisita = DateUtils.ParseDateTime(txtFechaVisita.Text);
                    if (DateTime.Compare(fechaVisita, DateTime.Today) > 0)
                    {
                        e.IsValid = false;
                        ((BaseValidator)sender).ToolTip = Resources.TextosJavaScript.TEXTO_FECHA_VISITA_NO_PUEDE_SER_MAYOR;//"La fecha de la visita no puede ser mayor que la fecha actual.";
                    }

                    // 18/01/2011 Kintell.
                    DateTime fechaLimiteSuperiorVisita = DateUtils.ParseDateTime(txtFechaLimite.Text);
                    if (fechaVisita.Date > fechaLimiteSuperiorVisita.Date)
                    {
                        e.IsValid = false;
                        ((BaseValidator)sender).ToolTip = Resources.TextosJavaScript.TEXTO_FECHA_VISITA_NO_PUEDE_SER_SUPERIOR;//"La fecha de la visita no puede ser superior a la fecha limite.";
                    }
                    //BGN 20191217 Proteccion Gas (EMTOGASCAN) tiene que permitir 2 años
                    int anyo = -1;
                    if (hdnEsProteccion.Value.Equals("1"))
                    {
                        anyo = -2;
                    }
                    DateTime fechaLimiteInferiorVisita = DateUtils.ParseDateTime(txtFechaLimite.Text).AddYears(anyo);
                    if (fechaVisita.Date < fechaLimiteInferiorVisita.Date)
                    {
                        e.IsValid = false;
                        ((BaseValidator)sender).ToolTip = Resources.TextosJavaScript.TEXTO_FECHA_VISITA_NO_PUEDE_SER_INFERIOR;//"La fecha de la visita no puede ser inferior a la fecha limite menos un año.";
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtFechaVisita_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtFechaVisitaCustomValidator.Validate();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtFechaReparacion_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                Boolean reparacionObligatoria = esReparacionObligatoria();
                if (this.rdbReparacion.Items[1].Selected)
                {
                    e.IsValid = FormValidation.ValidateDateTextBox(txtFechaReparacion, reparacionObligatoria, (BaseValidator)sender);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtFechaReparacion_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    this.txtFechaReparacionCustomValidator.Validate();
            //}
            //catch (Exception ex)
            //{
            //    ManageException(ex);
            //}
        }

        protected void OnTxtCosteMateriales_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                Boolean reparacionObligatoria = esReparacionObligatoria();
                if (this.rdbReparacion.Items[1].Selected)
                {
                    e.IsValid = FormValidation.ValidateNumberTextBox(txtCosteMateriales, reparacionObligatoria, (BaseValidator)sender);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtCosteMateriales_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    this.txtCosteMaterialesCustomValidator.Validate();
            //}
            //catch (Exception ex)
            //{
            //    ManageException(ex);
            //}
        }

        protected void OnTxtImporteManoObra_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                Boolean reparacionObligatoria = esReparacionObligatoria();

                if (this.rdbReparacion.Items[1].Selected )
                {
                    e.IsValid = FormValidation.ValidateNumberTextBox(txtImporteManoObra, reparacionObligatoria, (BaseValidator)sender);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtImporteManoObra_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    this.txtImporteManoObraCustomValidator.Validate();
            //}
            //catch (Exception ex)
            //{
            //    ManageException(ex);
            //}
        }

        protected void OnTxtCosteMaterialesAdicional_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                Boolean reparacionObligatoria = esReparacionObligatoria();
                if (this.rdbReparacion.Items[1].Selected)
                {
                    e.IsValid = FormValidation.ValidateNumberTextBox(txtCosteMaterialesAdicional, reparacionObligatoria, (BaseValidator)sender);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtCosteMaterialesAdicional_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    this.txtCosteMaterialesAdicionalCustomValidator.Validate();
            //}
            //catch (Exception ex)
            //{
            //    ManageException(ex);
            //}
        }

        private Boolean esReparacionObligatoria()
        {
            Boolean reparacionObligatoria = false;

            String strEstadoVisita = "";
            if (FormUtils.HasValue(cmbEstadoVisita))
            {
                strEstadoVisita = this.cmbEstadoVisita.SelectedItem.Value;
            }

            if (Visitas.EsEstadoVisitaCerrada(strEstadoVisita) && this.hdnEstadoAnteriorVisita.Value != null &&
                this.hdnEstadoAnteriorVisita.Value.ToString().Equals(StringEnum.GetStringValue(Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)))
            {
                reparacionObligatoria = true;
            }
            else
            {
                reparacionObligatoria = false;
            }
            return reparacionObligatoria;
        }

        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            // coger los valores de sessión

            // cargar los valores en el formulario

            // eliminar de sesión los valores del formulario
        }

        public override void SaveSessionData()
        {
            // eliminar de sesión los valores del formulario si existían

            // coger los valores en el formulario

            // cargar los valores en sessión
        }

        public override void DeleteSessionData()
        {

        }
        #endregion

        protected void chkFacturadoProveedor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkFacturadoProveedor.Enabled = true;
                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.CanceladaAusentePorSegundaVez && int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.Cerrada && int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)//cmbEstadoVisita.SelectedIndex != 6 && cmbEstadoVisita.SelectedIndex != 7 && cmbEstadoVisita.SelectedItem.Value != "08")
                {
                    this.chkFacturadoProveedor.Checked = false;
                    txtNumFactura.Text = "";
                    txtFechaFactura.Text = "";
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_FACTURAR_VISITA_SI_NO_CERRADA);//"No se puede facturar la visita si el estado de esta no es cerrada");
                }
                else
                {
                    if (chkFacturadoProveedor.Checked == false)
                    {
                        if (txtNumFactura.Text != "")
                        {
                            chkFacturadoProveedor.Checked = true;
                            chkFacturadoProveedor.Enabled = true;
                            //txtNumFactura.Enabled = true;
                            txtFechaFactura.Enabled = true;
                            MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_DESCHEQUEAR_FACTURADO_PROVEEDOR);//"Con número factura informado no se puede deschequear facturado por el proveedor");
                        }
                    }
                    else
                    {
                        //txtFechaFactura.Enabled = true;
                        //txtNumFactura.Enabled = true;
                        chkFacturadoProveedor.Enabled = true;
                    }
                //    txtNumFactura.Enabled = true;
                //    if (txtNumFactura.Text != "")
                //    {
                //        chkFacturadoProveedor.Enabled = true;
                //        txtNumFactura.Enabled = true;
                //        txtFechaFactura.Enabled = true;
                //        this.chkFacturadoProveedor.Checked = true;
                //        MostrarMensaje("Con número factura informado no se puede deschequear facturado por el proveedor");
                //    }
                }

                if (this.chkFacturadoProveedor.Checked)
                {
                    string MES = DateTime.Now.AddMonths(1).Month.ToString();
                    if (MES.Length == 1) { MES = "0" + MES; }
                    String Anhio=DateTime.Now.Year.ToString();
                    if (MES == "01") { Anhio = DateTime.Now.AddYears(1).Year.ToString(); }
                    this.txtFechaFactura.Text = "01/" + MES+ "/" + Anhio;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
        }
        private void OcultarDivs()
        {
            string alerta = "<script type='text/javascript'>verDatos();</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
        }

        //private void MostrarMensajeExclamacion(String Mensaje)
        //{
        //    MessageBox.Show(message, caption,
        //                         MessageBoxButtons.YesNo,
        //                         MessageBoxIcon.Question);


        //}


        protected void rdbReparacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                //this.rdbReparacion.Items[0].Selected 
                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.Cerrada && int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) != (int)Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion)//cmbEstadoVisita.SelectedIndex != 6 && cmbEstadoVisita.SelectedIndex != 7)
                {
                    rdbReparacion.Items[1].Selected = false;
                    rdbReparacion.Items[0].Selected = true;
                    DeshabilitarControles();
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_PERMITIDO_INDICAR_REPARACION);//"No esta permitido indicar la reparación si la visita no esta cerrada");
                }
                if( rdbReparacion.Items[1].Selected)
                {
                    string MES = DateTime.Now.AddMonths(1).Month.ToString();
                    if (MES.Length == 1) { MES = "0" + MES; }
                    String Anhio1 = DateTime.Now.Year.ToString();
                    if (MES == "01") { Anhio1 = DateTime.Now.AddYears(1).Year.ToString(); }
                    this.txtFechaFacturaReparacion.Text = "01/" + MES + "/" + Anhio1;
                    
                }

                OcultarDivs();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtFechaReparacionValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.TextBoxHasValue(txtFechaReparacion);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        
        protected void OntxtFechaFacturaReparacion_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                Boolean reparacionObligatoria = esReparacionObligatoria();
                if (this.rdbReparacion.Items[1].Selected)
                {
                    e.IsValid = FormValidation.ValidateDateTextBox(txtFechaFacturaReparacion, reparacionObligatoria, (BaseValidator)sender);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtFechaFacturaReparacion_TextChanged(object sender, EventArgs e)
        {

        }

        protected void OnBtnActualizarTelefonos_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (lblIdContrato.Text == null || lblIdContrato.Text.Length == 0)
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_HAY_CODIGO_CONTRATO);//"No hay código de contrato");
                }
                else
                {
                    //20220311 BGN MOD BEG R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
                    PhoneValidator telefVal = new PhoneValidator();
                    bool guardarTelef1 = false;
                    bool guardarTelef2 = false;
                    if (!String.IsNullOrEmpty(txtTelefono1.Text) && !telefVal.Validate(txtTelefono1.Text.Trim()))
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO + " (" + txtTelefono1.Text + ")");
                    }
                    else
                    {
                        guardarTelef1 = true;
                    }
                    if (!String.IsNullOrEmpty(txtTelefono2.Text) && !telefVal.Validate(txtTelefono2.Text.Trim()))
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO + " (" + txtTelefono2.Text + ")");
                    }
                    else
                    {
                        guardarTelef2 = true;
                    }
                    if (guardarTelef1 && guardarTelef2)
                    {
                        Mantenimiento.ActualizarTelefonosMantenimiento(this.lblIdContrato.Text, this.txtTelefono1.Text.Trim(), this.txtTelefono2.Text.Trim());
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONOS_ACTUALIZADOS);//"Telefonos Actualizados");
                    }                        
                    //20220311 BGN MOD END R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnbtnHistoricoVisita_Click(object sender, System.EventArgs e)
        {
            try
            {
                String Pagina="FrmModalHistoricoVisita.aspx?COD_CONTRATO=" + this.lblIdContrato.Text  + "&COD_VISITA=" + this.lblIdVisita.Text;

                string alerta = "<script type='text/javascript'>abrirVentanaLocalizacion('" + Pagina + "', '600', '350', 'ventana-modal','HISTORICO VISITA','0','0');</script>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnbtnTicketCombustion2222222_Click(object sender, System.EventArgs e)
        {
            try
            {
                String Pagina = "FrmModalTicketCombustion.aspx?COD_CONTRATO=" + this.lblIdContrato.Text + "&COD_VISITA=" + this.lblIdVisita.Text;

                string alerta = "<script type='text/javascript'>abrirVentanaLocalizacion('" + Pagina + "', '600', '350', 'ventana-modal','TICKET COMBUSTION','0','0');</script>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnbtnTicketCombustion_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (lblIdContrato.Text == null || lblIdContrato.Text.Length == 0)
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_HAY_CODIGO_CONTRATO);//"No hay código de contrato");
                }
                else
                {
                    //AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    //UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                    ////obtenemos el equipamiento de la visita para comprobar si el ultimo equipamiento es un cocina
                    //bool existCocina = false;

                    //List<EquipamientoDTO> listaEquipamiento = Equipamientos.ObtenerEquipamientos(this.lblIdContrato.Text, (Int16)usuarioDTO.IdIdioma);
                    //if (listaEquipamiento != null && listaEquipamiento.Count > 0)
                    //    existCocina = listaEquipamiento.Last().IdTipoEquipamiento == 1;

                    ////Si el tipo equipamiento es un 1(Cocina) no se debe de validar ni guardar el ticket de combustion aunque le corresponda por estado
                    //if (existCocina)
                    //{
                    //    MostrarMensaje("Esta visita es de tipo equipamiento Cocina, no requiere ticket de combustion.");
                    //}
                    //else
                    //{
                        String strUrl = "./FrmModalTicketCombustion.aspx" + "?COD_CONTRATO=" + this.lblIdContrato.Text + "&COD_VISITA=" + this.lblIdVisita.Text + "&EDICION=false&TIPO_VISITA=" + cboTipoVisita.SelectedValue;
                        this.OpenWindow(strUrl, "ModalTicketCombustion", "Ticket de combustion");
                    //}
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnbtnTicketCombustionNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (lblIdContrato.Text == null || lblIdContrato.Text.Length == 0)
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_HAY_CODIGO_CONTRATO);//"No hay código de contrato");
                }
                else
                {
                    String strUrl = "./FrmModalTicketCombustionNew.aspx" + "?COD_CONTRATO=" + this.lblIdContrato.Text + "&COD_VISITA=" + this.lblIdVisita.Text;
                    this.OpenWindow(strUrl, "ModalTicketCombustion", "Ticket de combustion");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void cboTecnico_SelectedIndexChanged(object sender, EventArgs e)
        {
            IDataReader Datos = null;
            try
            {
                this.txtTecnico.Text = cboTecnico.SelectedItem.Text.ToString();
                this.txtEmpresa.Text = Usuarios.ObtenerEmpresaPorTecnico(Int16.Parse(this.cboTecnico.SelectedValue.ToString()));

                if (int.Parse(cmbEstadoVisita.Items[cmbEstadoVisita.SelectedIndex].Value) == (int)Enumerados.EstadosVisita.Cerrada)//cmbEstadoVisita.SelectedIndex == 6)// || cmbEstadoVisita.SelectedIndex == 7)
                {
                    //Datos = Visitas.ObtenerFechasMovimiento((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));
                    Datos = Visitas.ObtenerFechasMovimientoParaCerradas((string)this.lblIdContrato.Text, int.Parse(this.lblIdVisita.Text));
                    while (Datos.Read())
                    {
                        DateTime fecha = (DateTime)DataBaseUtils.GetDataReaderColumnValue(Datos, "fecha");
                        if (fecha.AddDays(3) >= DateTime.Now)
                        {
                            rdbReparacion.Enabled = true;
                        }
                        else
                        {
                            rdbReparacion.Enabled = false;
                            //MostrarMensaje("No puede introducir Reparaciones debido a que desde la última vez en que modifico el estado de la visita a cerrada, han pasado mas de 3 dias");
                            lbl3Dias.Visible = true;
                        }
                        break;
                    }
                }

                MasterPageBase mp = (MasterPageBase)this.Master;
                mp.PlaceHolderScript.Controls.Add(new LiteralControl("<script>PosicionarTecnico1()</script>"));

                //string script = "<script type='text/javascript'>verTecnico();</script>";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "SCRIPT", script, false);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
            finally 
            {
                if (Datos != null)
                {
                    Datos.Close();
                }
            }
        }
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (cmbContador.SelectedValue == "...")
            {
                args.IsValid = false;
                ((CustomValidator)source).ToolTip = "Debe de seleccionar si el contador es interno o externo";
            }
        }


        protected void OnBtnSubirFichero_Click(object sender, EventArgs e)
        {
            FileStream objfilestream = new FileStream("c:\\bono.pdf", FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            Byte[] mybytearray = new Byte[len];

            objfilestream.Read(mybytearray, 0, len);

            //get file
            StreamReader reader = new StreamReader("c:\\bono.pdf");
            BinaryReader binReader = new BinaryReader(reader.BaseStream);

            //read file
            byte[] binFile = binReader.ReadBytes(Convert.ToInt32(binReader.BaseStream.Length));

            //close reader
            binReader.Close();
            reader.Close();


            //String imageAsString = Base64.encodeToString(binFile, Base64.DEFAULT);
            string a=System.Convert.ToBase64String(binFile);
        }

    }
}