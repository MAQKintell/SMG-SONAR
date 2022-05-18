using System;
using System.Web.UI;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Security;
using System.Data;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalFiltrosAvanzados : FrmBase
    {
        // Constante para almacenar los datos del filtro en sesion
        public int indiceUrgencia;

        /// <summary>
        /// Carga los combos de la ventana, a excepción del combo de poblaciones
        /// que se carga cuando se ha seleccionado una provincia
        /// </summary>
        private void CargarCombos()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            //carga de provincias
            //cmbProvincias.DataSource = TablasReferencia.ObtenerProvincias();
            //cmbProvincias.DataValueField = "CodProvincia";
            //cmbProvincias.DataTextField = "Nombre";
            //cmbProvincias.DataBind();
            //FormUtils.AddDefaultItem(cmbProvincias);
            chkProvincias.DataSource = TablasReferencia.ObtenerProvincias();
            chkProvincias.DataValueField = "CodProvincia";
            chkProvincias.DataTextField = "Nombre";
            chkProvincias.DataBind();
            hdnContadorProvincia.Value = chkProvincias.Items.Count.ToString();

            //carga de campañas
            cmbCampania.DataSource = TablasReferencia.ObtenerCampanias();
            cmbCampania.DataValueField = "Campania";
            cmbCampania.DataTextField = "Campania";
            cmbCampania.DataBind();
            FormUtils.AddDefaultItem(cmbCampania);

            //carga de tipos de urgencia
            cmbTiposUrgencia.DataSource = TablasReferencia.ObtenerTiposUrgencia((Int16)usuarioDTO.IdIdioma);
            cmbTiposUrgencia.DataValueField = "Id";
            cmbTiposUrgencia.DataTextField = "Descripcion";
            cmbTiposUrgencia.DataBind();
            //FormUtils.AddDefaultItem(cmbTiposUrgencia);

            //Kintell 30/06/2011
            //Estados.
            cblFiltrosColumna.DataSource = TablasReferencia.ObtenerTiposEstadoVisita((Int16)usuarioDTO.IdIdioma);
            cblFiltrosColumna.DataValueField = "Codigo";
            cblFiltrosColumna.DataTextField = "Descripcion";
            cblFiltrosColumna.DataBind();
            hdnContadorEstado.Value = cblFiltrosColumna.Items.Count.ToString();

        }


        private void CargarComboPoblaciones(String CodProvincia)
        {
            ProvinciaDTO prov = new ProvinciaDTO();
            prov.CodProvincia = CodProvincia;
            //List<PoblacionDTO> lista = TablasReferencia.ObtenerPoblacionesProvincia(prov);
            ////PoblacionDTO poblacionVacia = new PoblacionDTO();
            ////lista.Insert(0, poblacionVacia);
            //cmbPoblaciones.DataSource = lista;
            //cmbPoblaciones.DataValueField = "CodPoblacion";
            //cmbPoblaciones.DataTextField = "Nombre";
            //cmbPoblaciones.DataBind();
            chkPoblaciones.DataSource = TablasReferencia.ObtenerPoblacionesProvincia(prov);
            chkPoblaciones.DataValueField = "CodPoblacion";
            chkPoblaciones.DataTextField = "Nombre";
            chkPoblaciones.DataBind();
            hdnContadorPoblacion.Value = chkPoblaciones.Items.Count.ToString();
            VistaContratoCompletoDTO filtrosDTO = null;

            // Si ya existe el filtro en la sesion se obtiene para mantener asi los filtros de las columnas
            if (CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO) != null)
            {
                filtrosDTO = (VistaContratoCompletoDTO)CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO);
            }
            else
            {
                filtrosDTO = new VistaContratoCompletoDTO();
            }

            if (filtrosDTO.CodigoDePoblacion != null && filtrosDTO.CodigoDePoblacion != "")
            {
                String[] Poblacion = filtrosDTO.CodigoDePoblacion.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Poblacion.Length - 1; i++)
                {
                    for (int j = 0; j <= this.chkPoblaciones.Items.Count - 1; j++)
                    {
                        if (chkPoblaciones.Items[j].Value == Poblacion[i])
                        {
                            chkPoblaciones.Items[j].Selected = true;
                        }
                    }
                }
            }
 

            
            
        }

        /// <summary>
        /// Evento de inicialización de la página.
        /// Carga los datos de la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            try
            {
                // Sólo cargamos las combos y los datos de sesión la primera vez que se entra en
                // la pantalla.
                Page.SetFocus(this.hdnControlFoco.Value);
                if (!IsPostBack)
                {
                    CargarCombos();
                    CargarDatosDeSesion();

                    AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                    if (usuarioDTO.NombreProveedor != "ADICO" )
                    {
                        this.txtProveedor.Text = usuarioDTO.NombreProveedor;
                        this.txtProveedor.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        /// <summary>
        /// Evento del botón aceptar de la pantalla.
        /// Guarda las opciones de filtro en la sesión y cierra la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidatePage())
                {
                    GuardarFiltrosEnSesion();
                    //Kintell 30/06/2011 eliminar filtros excell, 
                    //poner filtro estado aki.
                    MasterPageModal mpm = (MasterPageModal)this.Master;
                    mpm.CerrarVentanaModal();
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnBtnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtCodPostal.Text = "";
                this.txtContrato.Text = "";
                this.txtInfoApellido1.Text = "";
                this.txtInfoApellido2.Text = "";
                this.txtInfoNombre.Text = "";
                this.txtProveedor.Text = "";
                this.txtLote.Text = "";

                this.txtFechaVisitaDesde.Text = "";
                this.txtFechaVisitaHasta.Text = "";
                this.txtFechaLoteDesde.Text = "";
                this.txtFechaLoteHasta.Text = "";
                this.txtFechaServicioDesde.Text = "";
                this.txtFechaServicioHasta.Text = "";
                
                //this.cmbPoblaciones.Items.Clear();

                for (int i = 0; i <= this.chkProvincias.Items.Count - 1; i++)
                {
                    chkProvincias.Items[i].Selected = false;
                }

                for (int i = 0; i <= this.cblFiltrosColumna.Items.Count - 1; i++)
                {
                    cblFiltrosColumna.Items[i].Selected = false;
                }

                for (int i = 0; i <= this.chkPoblaciones.Items.Count - 1; i++)
                {
                    chkPoblaciones.Items[i].Selected = false;
                }
                chkPoblaciones.Items.Clear();

                //*********************
                VistaContratoCompletoDTO filtrosDTO = null;

                // Si ya existe el filtro en la sesion se obtiene para mantener asi los filtros de las columnas
                if (CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO) != null)
                {
                    filtrosDTO = (VistaContratoCompletoDTO)CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO);
                }
                else
                {
                    filtrosDTO = new VistaContratoCompletoDTO();
                }
                filtrosDTO.CodigoDePoblacion = "";


                //*********************

                this.cmbCampania.SelectedIndex = -1;
                //this.cmbPoblaciones.SelectedIndex = -1;
                //this.cmbProvincias.SelectedIndex = -1;
                this.cmbTiposUrgencia.SelectedIndex = -1;
                this.txtLoteCustomValidator.IsValid = true;
                this.txtCodPostalCustomValidator.IsValid = true;
                this.txtContrato.Focus();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        /// <summary>
        /// Evento de cambio de provincia.
        /// Cuando se selecciona una provincia se carga el combo de poblaciones
        /// asociadas a esa provincia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbProvincias_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int seleccionadas = 0;
                for (int i = 0; i <= this.chkProvincias.Items.Count - 1; i++)
                {
                    if (chkProvincias.Items[i].Selected) { seleccionadas = seleccionadas + 1; }
                }
                if (seleccionadas == 1 && chkProvincias.SelectedValue != "00")
                {
                    CargarComboPoblaciones(chkProvincias.SelectedValue);
                }
                else
                {
//                    cmbPoblaciones.Items.Clear();
                    //FormUtils.AddDefaultItem(cmbPoblaciones);
                    VistaContratoCompletoDTO filtrosDTO = null;
                    // Si ya existe el filtro en la sesion se obtiene para mantener asi los filtros de las columnas
                    if (CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO) != null)
                    {
                        filtrosDTO = (VistaContratoCompletoDTO)CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO);
                    }
                    else
                    {
                        filtrosDTO = new VistaContratoCompletoDTO();
                    }

                    filtrosDTO.CodigoDePoblacion = "";
                    for (int i = 0; i <= this.chkPoblaciones.Items.Count - 1; i++)
                    {
                        chkPoblaciones.Items[i].Selected = false;
                    }
                    chkPoblaciones.Items.Clear();
                }

              
                //if (cmbProvincias.SelectedIndex != -1)
                //{
                //    CargarComboPoblaciones((String)cmbProvincias.SelectedItem.Value);
                //}
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        /// <summary>
        /// Guarda en un hashtable en sesión los datos del filtro tomándolos de los controles
        /// </summary>
        public void GuardarFiltrosEnSesion()
        {

            try
            {
                VistaContratoCompletoDTO filtrosDTO = null;

                // Si ya existe el filtro en la sesion se obtiene para mantener asi los filtros de las columnas
                //if (CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO) != null)
                //{
                //    filtrosDTO = (VistaContratoCompletoDTO)CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO);
                //}
                //else
                //{
                    filtrosDTO = new VistaContratoCompletoDTO();
                //}

                #region carga valores filtro
                if (!txtProveedor.Text.Equals(String.Empty))
                {
                    filtrosDTO.Proveedor = txtProveedor.Text;
                }
                else
                {
                    filtrosDTO.Proveedor = null;
                }

                if (!txtContrato.Text.Equals(String.Empty))
                {
                    filtrosDTO.CodigoContrato = txtContrato.Text;
                }
                else
                {
                    filtrosDTO.CodigoContrato = null;
                }

                if (!txtInfoNombre.Text.Equals(String.Empty))
                {
                    filtrosDTO.Nombre = txtInfoNombre.Text;
                }
                else
                {
                    filtrosDTO.Nombre = null;
                }

                if (!txtInfoApellido1.Text.Equals(String.Empty))
                {
                    filtrosDTO.Apellido1 = txtInfoApellido1.Text;

                }
                else
                {
                    filtrosDTO.Apellido1 = null;
                }

                if (!txtInfoApellido2.Text.Equals(String.Empty))
                {
                    filtrosDTO.Apellido2 = txtInfoApellido2.Text;

                }
                else
                {
                    filtrosDTO.Apellido2 = null;
                }

                //if (FormUtils.HasValue(cmbProvincias))
                //{
                //    filtrosDTO.CodigoDeProvincia = cmbProvincias.SelectedValue;
                //}
                //else
                //{
                //    filtrosDTO.CodigoDeProvincia = null;
                //}
                Boolean Esta = false;
                for (int i = 0; i <= this.chkProvincias.Items.Count - 1; i++)
                {
                    Esta = true;
                    if (this.chkProvincias.Items[i].Selected)
                    {
                        if (this.chkProvincias.Items[i].Value == "00")
                        {
                            for (int j = 0; j <= chkProvincias.Items.Count - 1; j++)
                            {
                                filtrosDTO.CodigoDeProvincia = this.chkProvincias.Items[j].Text + ";" + filtrosDTO.CodigoDeProvincia;
                            }
                            //break;
                        }
                        else
                        {
                            filtrosDTO.CodigoDeProvincia = this.chkProvincias.Items[i].Text + ";" + filtrosDTO.CodigoDeProvincia;
                        }

                    }
                }
                if (Esta == false)
                {
                    filtrosDTO.CodigoDeProvincia = null;
                }






                //if (FormUtils.HasValue(cmbPoblaciones))
                //{
                //    filtrosDTO.CodigoDePoblacion = cmbPoblaciones.SelectedValue;
                //    CurrentSession.SetAttribute(CurrentSession.SESSION_COD_PROVINCIA, cmbPoblaciones.SelectedValue);
                //}

                Esta = false;
                for (int i = 0; i <= this.chkPoblaciones.Items.Count - 1; i++)
                {
                    Esta = true;
                    if (this.chkPoblaciones.Items[i].Selected)
                    {
                        if (this.chkPoblaciones.Items[i].Value == "00")
                        {
                            for (int j = 0; j <= chkPoblaciones.Items.Count - 1; j++)
                            {
                                filtrosDTO.CodigoDePoblacion = this.chkPoblaciones.Items[j].Value + ";" + filtrosDTO.CodigoDePoblacion;
                            }
                            //break;
                        }
                        else
                        {
                            filtrosDTO.CodigoDePoblacion = this.chkPoblaciones.Items[i].Value + ";" + filtrosDTO.CodigoDePoblacion;
                        }

                    }
                }
                if (Esta == false)
                {
                    filtrosDTO.CodigoDePoblacion = null;
                    CurrentSession.SetAttribute(CurrentSession.SESSION_COD_PROVINCIA, "");
                }



                //******************************************************************************************************************
                //Kintell 01/07/2011
                Esta = false;
                for (int i = 0; i <= this.cblFiltrosColumna.Items.Count - 1; i++)
                {
                    Esta = true;
                    if (this.cblFiltrosColumna.Items[i].Selected)
                    {
                        if (this.cblFiltrosColumna.Items[i].Value == "00")
                        {
                            for (int j = 0; j <= cblFiltrosColumna.Items.Count - 1; j++)
                            {
                                filtrosDTO.CodigoDeEstadoVisita = this.cblFiltrosColumna.Items[j].Text + ";" + filtrosDTO.CodigoDeEstadoVisita;
                            }
                            //break;
                        }
                        else
                        {
                            filtrosDTO.CodigoDeEstadoVisita = this.cblFiltrosColumna.Items[i].Text + ";" + filtrosDTO.CodigoDeEstadoVisita;
                        }

                    }
                }
                if (Esta == false)
                {
                    filtrosDTO.CodigoDeEstadoVisita = null;
                }
                //******************************************************************************************************************



                if (!txtCodPostal.Text.Equals(String.Empty))
                {
                    filtrosDTO.CodigoPostal = txtCodPostal.Text;

                }
                else
                {
                    //filtrosDTO.Apellido2 = null;
                    filtrosDTO.CodigoPostal = null;
                }



                for (int i = 0; i <= this.cmbTiposUrgencia.Items.Count - 1; i++)
                {
                    Esta = true;
                    if (this.cmbTiposUrgencia.Items[i].Selected)
                    {
                        if (this.cmbTiposUrgencia.Items[i].Value == "00")
                        {
                            for (int j = 0; j <= cmbTiposUrgencia.Items.Count - 1; j++)
                            {
                                filtrosDTO.DescripcionTipoUrgencia = this.cmbTiposUrgencia.Items[j].Text + ";" + filtrosDTO.DescripcionTipoUrgencia;
                            }
                            //break;
                        }
                        else
                        {
                            filtrosDTO.DescripcionTipoUrgencia = this.cmbTiposUrgencia.Items[i].Text + ";" + filtrosDTO.DescripcionTipoUrgencia;
                        }

                    }
                }
                if (Esta == false)
                {
                    filtrosDTO.AuxGuardaIndex = null;
                }
                //if (FormUtils.HasValue(cmbTiposUrgencia))
                //{
                //    filtrosDTO.DescripcionTipoUrgencia = cmbTiposUrgencia.SelectedItem.Text;
                //    filtrosDTO.AuxGuardaIndex = cmbTiposUrgencia.SelectedValue;
                //}
                //else
                //{
                //    filtrosDTO.DescripcionTipoUrgencia = null;
                //}





                if (FormUtils.HasValue(cmbCampania))
                {
                    filtrosDTO.Campania = Int32.Parse(cmbCampania.SelectedValue);
                }
                else
                {
                    filtrosDTO.Campania = null;
                }

                if (!txtLote.Text.Equals(String.Empty))
                {
                    filtrosDTO.IdLote = new Decimal(Double.Parse(txtLote.Text));
                }
                else
                {
                    filtrosDTO.IdLote = null;
                }

                if (!string.IsNullOrEmpty(txtFechaVisitaDesde.Text))
                {
                    filtrosDTO.FechaMinimaLimiteVisita = DateTime.Parse(txtFechaVisitaDesde.Text);
                }
                else
                {
                    filtrosDTO.FechaMinimaLimiteVisita = null;
                }
                if (!string.IsNullOrEmpty(txtFechaVisitaHasta.Text))
                {
                    filtrosDTO.FechaMaximaLimiteVisita = DateTime.Parse(txtFechaVisitaHasta.Text);
                }
                else
                {
                    filtrosDTO.FechaMaximaLimiteVisita = null;
                }

                if (!string.IsNullOrEmpty(txtFechaLoteDesde.Text))
                {
                    filtrosDTO.FechaMinimaLote = DateTime.Parse(txtFechaLoteDesde.Text);
                }
                else
                {
                    filtrosDTO.FechaMinimaLote = null;
                }
                if (!string.IsNullOrEmpty(txtFechaLoteHasta.Text))
                {
                    filtrosDTO.FechaMaximaLote = DateTime.Parse(txtFechaLoteHasta.Text);
                }
                else
                {
                    filtrosDTO.FechaMaximaLote = null;
                }

                if (!string.IsNullOrEmpty(txtFechaServicioDesde.Text))
                {
                    filtrosDTO.FechaMinimaAltaServicio = DateTime.Parse(txtFechaServicioDesde.Text);
                }
                else
                {
                    filtrosDTO.FechaMinimaAltaServicio = null;
                }
                if (!string.IsNullOrEmpty(txtFechaServicioHasta.Text))
                {
                    filtrosDTO.FechaMaximaAltaServicio = DateTime.Parse(txtFechaServicioHasta.Text);
                }
                else
                {
                    filtrosDTO.FechaMaximaAltaServicio = null;
                }
                #endregion

                CurrentSession.SetAttribute(SessionVariables.FILTRO_AVANZADO, filtrosDTO);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        /// <summary>
        /// Carga los datos del filtro de sesión y los pone en los controles
        /// </summary>
        public void CargarDatosDeSesion()
        {
            try
            {
                if (CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO) != null)
                {
                    VistaContratoCompletoDTO valoresFiltro = (VistaContratoCompletoDTO)CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO);

                    #region carga de valores del filtro en el formulario
                    if (valoresFiltro.Proveedor != null)
                    {
                        this.txtProveedor.Text = (String)valoresFiltro.Proveedor;
                    }
                    if (valoresFiltro.CodigoContrato != null)
                    {
                        txtContrato.Text = (String)valoresFiltro.CodigoContrato;
                    }
                    if (valoresFiltro.Nombre != null)
                    {
                        this.txtInfoNombre.Text = (String)valoresFiltro.Nombre;
                    }
                    if (valoresFiltro.Apellido1 != null)
                    {
                        this.txtInfoApellido1.Text = (String)valoresFiltro.Apellido1;
                    }
                    if (valoresFiltro.Apellido2 != null)
                    {
                        this.txtInfoApellido2.Text = (String)valoresFiltro.Apellido2;
                    }
                    if (valoresFiltro.CodigoDeProvincia != null)
                    {
                        String[] Provincias = valoresFiltro.CodigoDeProvincia.Split(';');
                        Boolean Esta = false;
                        String Filtropro = "";
                        int seleccionadas = 0;
                        for (int i = 0; i <= Provincias.Length - 1; i++)
                        {
                            for (int j = 0; j <= chkProvincias.Items.Count - 1; j++)
                            {
                                if (chkProvincias.Items[j].Text == Provincias[i])
                                {
                                    chkProvincias.Items[j].Selected = true;
                                    seleccionadas = seleccionadas + 1;
                                }
                            }
                        }
                        if (seleccionadas == 1 && chkProvincias.SelectedValue != "00")
                        {
                            CargarComboPoblaciones(chkProvincias.SelectedValue);
                        }
                        else
                        {
                            //cmbPoblaciones.Items.Clear();
                            //FormUtils.AddDefaultItem(cmbPoblaciones);
                        }
                        ////cmbProvincias.SelectedValue = valoresFiltro.CodigoDeProvincia;
                        ////CargarComboPoblaciones((String)cmbProvincias.SelectedItem.Value);
                        //cmbPoblaciones.SelectedValue = valoresFiltro.CodigoDePoblacion;
                    }
                    if (valoresFiltro.CodigoPostal != null)
                    {
                        txtCodPostal.Text = (String)valoresFiltro.CodigoPostal;
                    }

                    //************************************
                    if (valoresFiltro.DescripcionTipoUrgencia != null)
                    {
                        String[] Urgencia = valoresFiltro.DescripcionTipoUrgencia.Split(';');
                        int seleccionadas = 0;
                        for (int i = 0; i <= Urgencia.Length - 1; i++)
                        {
                            for (int j = 0; j <= cmbTiposUrgencia.Items.Count - 1; j++)
                            {
                                if (cmbTiposUrgencia.Items[j].Text == Urgencia[i])
                                {
                                    cmbTiposUrgencia.Items[j].Selected = true;
                                    seleccionadas = seleccionadas + 1;
                                }
                            }
                        }
                        //if (seleccionadas == 1 && cmbTiposUrgencia.SelectedValue != "00")
                        //{
                        //    CargarComboPoblaciones(cmbTiposUrgencia.SelectedValue);
                        //}
                        //else
                        //{
                        //    //cmbPoblaciones.Items.Clear();
                        //    //FormUtils.AddDefaultItem(cmbPoblaciones);
                        //}
                        ////cmbProvincias.SelectedValue = valoresFiltro.CodigoDeProvincia;
                        ////CargarComboPoblaciones((String)cmbProvincias.SelectedItem.Value);
                        //cmbPoblaciones.SelectedValue = valoresFiltro.CodigoDePoblacion;
                    }
                    //if (valoresFiltro.DescripcionTipoUrgencia != null)
                    //{
                    //    cmbTiposUrgencia.SelectedValue = valoresFiltro.AuxGuardaIndex;
                    //}
                    if (valoresFiltro.Campania.HasValue)
                    {
                        cmbCampania.SelectedValue = valoresFiltro.Campania.Value.ToString();
                    }
                    if (valoresFiltro.IdLote != null)
                    {
                        txtLote.Text = valoresFiltro.IdLote.ToString();
                    }
                    if (valoresFiltro.FechaMinimaLimiteVisita.HasValue)
                    {
                        txtFechaVisitaDesde.Text = valoresFiltro.FechaMinimaLimiteVisita.Value.ToShortDateString();
                    }
                    if (valoresFiltro.FechaMaximaLimiteVisita.HasValue)
                    {
                        txtFechaVisitaHasta.Text = valoresFiltro.FechaMaximaLimiteVisita.Value.ToShortDateString();
                    }
                    if (valoresFiltro.FechaMinimaLote.HasValue)
                    {
                        txtFechaLoteDesde.Text = valoresFiltro.FechaMinimaLote.Value.ToShortDateString();
                    }
                    if (valoresFiltro.FechaMaximaLote.HasValue)
                    {
                        txtFechaLoteHasta.Text = valoresFiltro.FechaMaximaLote.Value.ToShortDateString();
                    }
                    if (valoresFiltro.FechaMinimaAltaServicio.HasValue)
                    {
                        txtFechaServicioDesde.Text = valoresFiltro.FechaMinimaAltaServicio.Value.ToShortDateString();
                    }
                    if (valoresFiltro.FechaMaximaAltaServicio.HasValue)
                    {
                        txtFechaServicioHasta.Text = valoresFiltro.FechaMaximaAltaServicio.Value.ToShortDateString();
                    }
                    //Kintell 01/07/2011
                    if (valoresFiltro.CodigoDeEstadoVisita != null)
                    {
                        String[] Estados = valoresFiltro.CodigoDeEstadoVisita.Split(';');
                        Boolean Esta = false;
                        String Filtropro = "";
                        for (int i = 0; i <= Estados.Length - 1; i++)
                        {
                            for (int j = 0; j <= cblFiltrosColumna.Items.Count - 1; j++)
                            {
                                if (cblFiltrosColumna.Items[j].Text == Estados[i])
                                {
                                    cblFiltrosColumna.Items[j].Selected = true;
                                }
                            }
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        #region Validacion de campos

        protected void OnCodPostal_ServerValidate(object sender, ServerValidateEventArgs e)
        {

            try
            {
                e.IsValid = true;// FormValidation.ValidateNumberAndLongTextBox(txtCodPostal, false, (BaseValidator)sender, 5);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

        }
        protected void OnNumeroLote_ServerValidate(object sender, ServerValidateEventArgs e)
        {

            try
            {
                e.IsValid = FormValidation.ValidateNumberTextBox(txtLote, false, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

        }

        #endregion

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
            // eliminar de sesión los valores del formulario si existían
        }
        #endregion

        protected void OnTxtFechaVisitaDesde_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFechaVisitaDesde, false, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtFechaVisitaHasta_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFechaVisitaHasta, false, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtFechaLoteDesde_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFechaLoteDesde, false, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtFechaLoteHasta_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFechaLoteHasta, false, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtFechaServicioDesde_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFechaServicioDesde, false, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnTxtFechaServicioHasta_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ValidateDateTextBox(txtFechaServicioHasta, false, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
}
}
