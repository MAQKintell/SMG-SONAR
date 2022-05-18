using System;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Security;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalEquipamiento : FrmBase
    { 
        #region métodos privados
        private EquipamientoDTO ObtenerEquipamientoSeleccionado()
        {
            List<EquipamientoDTO> lista = (List<EquipamientoDTO>)CurrentSession.GetAttribute(SessionVariables.LISTA_EQUIPAMIENTO);
            int index = -1;
            if (hdnIndexEquipamientoSeleccionado.Value != null && hdnIndexEquipamientoSeleccionado.Value.Length > 0)
            {
                index = Int32.Parse(hdnIndexEquipamientoSeleccionado.Value);
            }

            if (lista != null && index >= 0)
            {
                return lista[index];
            }
            return null;

        }
        private void CargarCombos(Decimal? intIdMarcaCaldera)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            //Carga de las marcas de la caldera
            cmbMarcasCaldera.DataSource = TablasReferencia.ObtenerTiposMarcaCaldera(intIdMarcaCaldera);
            cmbMarcasCaldera.DataValueField = "Id";
            cmbMarcasCaldera.DataTextField = "Descripcion";
            cmbMarcasCaldera.DataBind();
            FormUtils.AddDefaultItem(cmbMarcasCaldera);

            //Carga del uso de la caldera
            cmbUsoCaldera.DataSource = TablasReferencia.ObtenerTiposUsoCaldera();
            cmbUsoCaldera.DataValueField = "Id";
            cmbUsoCaldera.DataTextField = "Descripcion";
            cmbUsoCaldera.DataBind();
            FormUtils.AddDefaultItem(cmbUsoCaldera);

            //Carga del tipo de la caldera
            cmbTiposCaldera.DataSource = TablasReferencia.ObtenerTiposTipoCaldera((Int16)usuarioDTO.IdIdioma);
            cmbTiposCaldera.DataValueField = "Id";
            cmbTiposCaldera.DataTextField = "Descripcion";
            cmbTiposCaldera.DataBind();
            FormUtils.AddDefaultItem(cmbTiposCaldera);

            //Carga del tipo de equipamiento
            cmbTipoEquipamiento.DataSource = TablasReferencia.ObtenerTiposEquipamiento((Int16)usuarioDTO.IdIdioma);
            cmbTipoEquipamiento.DataValueField = "Id";
            cmbTipoEquipamiento.DataTextField = "Descripcion";
            cmbTipoEquipamiento.DataBind();
            FormUtils.AddDefaultItem(cmbTipoEquipamiento);

            //Carga del tipo potencia
            cmbTipoPotencia.DataSource = Equipamientos.ObtenerTipoPotencia();
            cmbTipoPotencia.DataValueField = "Desc_tipo_potencia";
            cmbTipoPotencia.DataTextField = "Desc_tipo_potencia";
            cmbTipoPotencia.DataBind();
            FormUtils.AddDefaultItem(cmbTipoPotencia);
        }
        private string FormatoPotencia(string Valor)
        {
            try
            {
                int i;
                string[] valores = new string[2];
                valores = Valor.Split(',');
                if (Valor.Length < 5)
                {
                    if (valores[0].Length < 2)
                    {
                        //Comprobamos que viene con dos valores en la unidad.
                        for (i = valores[0].Length; i < 2; i++)
                        {
                            valores[0] = "0" + valores[0];
                        }
                    }
                    if (valores.Length > 1)
                    {
                        if (valores[1].Length < 2)
                        {
                            //Comporbamos que viene con dos valores en los decimales.
                            for (i = valores[0].Length; i < 2; i++)
                            {
                                valores[0] = valores[0] + "0";
                            }
                        }
                        Valor = valores[0] + "," + valores[1];
                    }
                    else
                    {
                        Valor = valores[0] + ",00";
                    }
                }

               
                return Valor;
            }
            catch (Exception ex)
            {
                ManageException(ex);
                return ex.Message ;
            }

        }
        private void CargarDatosCaldera(CalderaDTO caldera)
        { 
            if (caldera != null)
            {
                this.txtAnio.Text = caldera.Anio.ToString();
                this.txtModeloCaldera.Text = caldera.ModeloCaldera;
                if (caldera.Potencia != null)
                {
                    if (caldera.Potencia.ToString() != "")
                    {
                        this.txtPotencia.Text = FormatoPotencia(caldera.Potencia.ToString());
                    }
                    else
                    {
                        this.txtPotencia.Text = caldera.Potencia.ToString();
                    }
                }



                if (caldera.IdTipoCaldera.HasValue)
                {
                    this.cmbTiposCaldera.SelectedValue = caldera.IdTipoCaldera.Value.ToString();
                }
                if (caldera.Uso.HasValue)
                {
                    this.cmbUsoCaldera.SelectedValue = caldera.Uso.Value.ToString();
                }
                if (caldera.IdMarcaCaldera.HasValue)
                {
                    this.cmbMarcasCaldera.SelectedValue = caldera.IdMarcaCaldera.Value.ToString();
                    this.txtMarcasCaldera.Text = this.cmbMarcasCaldera.SelectedItem.Text;
                }
                this.hdnTieneCaldera.Value = "1";
            }
            else
            {
                // Indicamos que no tiene caldera para
                // que en al guardar se haga la validación de una forma u otras
                this.hdnTieneCaldera.Value = "0";
            }


        }


        private void CargarDatosEquipamiento(String strContrato)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            List<EquipamientoDTO> listaEquipamiento = Equipamientos.ObtenerEquipamientos(strContrato,(Int16)usuarioDTO.IdIdioma);
            this.grdEquipamientos.DataSource = listaEquipamiento;
            this.grdEquipamientos.DataBind();
            this.grdEquipamientos.Columns[0].Visible = false;
            this.lblNumeroEquipamientosValor.Text = listaEquipamiento.Count.ToString();

            CurrentSession.SetAttribute(SessionVariables.LISTA_EQUIPAMIENTO, listaEquipamiento);
        }
        private void CambiarEstadoVentana(Enumerados.AccionesGestion accion)
        {
            Enumerados.EstadosGestion estadoVentana = Enumerados.ObtenerEstadoGestion(this.hdnEstadoEquipamiento.Value);

            switch (accion)
            {
                case Enumerados.AccionesGestion.Seleccionar:
                    AccionSeleccionar(estadoVentana);
                    break;
                case Enumerados.AccionesGestion.Aniadir:
                    AccionAniadir(estadoVentana);
                    break;
                case Enumerados.AccionesGestion.Cancelar:
                    AccionCancelar(estadoVentana);
                    break;
                case Enumerados.AccionesGestion.Aceptar:
                    AccionAceptar(estadoVentana);
                    // para dejar los controles en el estado correcto
                    AccionCancelar(estadoVentana);
                    break;
                case Enumerados.AccionesGestion.Modificar:
                    AccionModificar(estadoVentana);
                    break;
                case Enumerados.AccionesGestion.Eliminar:
                    AccionEliminar(estadoVentana);
                    break;
                default:
                    throw new UIException(true, "4001");
            }
        }
        private void AccionCancelar(Enumerados.EstadosGestion estadoVentana)
        {
            this.btAnadirEquipamiento.Enabled = true;
            if (this.hdnIndexEquipamientoSeleccionado.Value != null && this.hdnIndexEquipamientoSeleccionado.Value.Length > 0)
            {
                this.btnModificarEquipamiento.Enabled = true;
                this.btnBorrarEquipamiento.Enabled = true;
                this.hdnEstadoEquipamiento.Value = ((int)Enumerados.EstadosGestion.Seleccionando).ToString();
            }
            else
            {
                this.btnModificarEquipamiento.Enabled = false;
                this.btnBorrarEquipamiento.Enabled = false;
                this.hdnEstadoEquipamiento.Value = ((int)Enumerados.EstadosGestion.Visualizando).ToString();
            }
            this.pnDetalleEquipamiento.Visible = false;
            DesactivarValidacionEquipamiento();
        }
        private void AccionAceptar(Enumerados.EstadosGestion estadoVentana)
        {
            if (this.txtPotenciaEquipamiento.Text == "") { this.txtPotenciaEquipamiento.Text = "0"; }
            if (NumberUtils.IsDecimal(this.txtPotenciaEquipamiento.Text))
            {


                List<EquipamientoDTO> listaEquipamiento = (List<EquipamientoDTO>)CurrentSession.GetAttribute(SessionVariables.LISTA_EQUIPAMIENTO);
                switch (estadoVentana)
                {
                    case Enumerados.EstadosGestion.Aniadiendo:
                        EquipamientoDTO equipamientoNuevo = new EquipamientoDTO();
                        equipamientoNuevo.IdTipoEquipamiento = Decimal.Parse(this.cmbTipoEquipamiento.SelectedItem.Value);
                        equipamientoNuevo.DescripcionTipoEquipamiento = this.cmbTipoEquipamiento.SelectedItem.Text;
                        equipamientoNuevo.Potencia = Double.Parse(this.txtPotenciaEquipamiento.Text);
                        equipamientoNuevo.CodContrato = this.hdnCodigoContrato.Value.ToString();
                        listaEquipamiento.Add(equipamientoNuevo);
                        break;
                    case Enumerados.EstadosGestion.Modificando:
                        EquipamientoDTO equipamiento = listaEquipamiento[Int32.Parse(this.hdnIndexEquipamientoSeleccionado.Value)];
                        equipamiento.IdTipoEquipamiento = Decimal.Parse(this.cmbTipoEquipamiento.SelectedItem.Value);
                        equipamiento.DescripcionTipoEquipamiento = this.cmbTipoEquipamiento.SelectedItem.Text;
                        equipamiento.Potencia = Double.Parse(this.txtPotenciaEquipamiento.Text);
                        break;
                    case Enumerados.EstadosGestion.Eliminando:
                        EquipamientoDTO equipamientoEliminado = listaEquipamiento[Int32.Parse(this.hdnIndexEquipamientoSeleccionado.Value)];
                        List<EquipamientoDTO> listaBorrar = (List<EquipamientoDTO>)CurrentSession.GetAttribute(SessionVariables.LISTA_EQUIPAMIENTO_BORRAR);
                        if (listaBorrar == null)
                        {
                            listaBorrar = new List<EquipamientoDTO>();
                        }
                        // solo añadimos el equipamiento a la lista de eliminar equipamientos 
                        // si tiene id de equipamiento, es decir si ya existía en la base de datos.
                        if (equipamientoEliminado.Id.HasValue)
                        {
                            listaBorrar.Add(equipamientoEliminado);
                        }

                        CurrentSession.SetAttribute(SessionVariables.LISTA_EQUIPAMIENTO_BORRAR, listaBorrar);
                        listaEquipamiento.RemoveAt(Int32.Parse(this.hdnIndexEquipamientoSeleccionado.Value));
                        break;
                    default:
                        throw new UIException(true, "4001");
                }
                DesactivarValidacionEquipamiento();
                // actualizamos la lista en sesión
                CurrentSession.SetAttribute(SessionVariables.LISTA_EQUIPAMIENTO, listaEquipamiento);
                // actualizamos el grid
                this.grdEquipamientos.DataSource = listaEquipamiento;
                this.grdEquipamientos.DataBind();

                // Actualizamos el estado en el que queda la ventana, y deseleccionamos el grid
                this.hdnEstadoEquipamiento.Value = ((int)Enumerados.EstadosGestion.Visualizando).ToString();
                this.lblNumeroEquipamientosValor.Text = listaEquipamiento.Count.ToString();
                this.hdnIndexEquipamientoSeleccionado.Value = "";
                this.grdEquipamientos.SelectedIndex = -1;
            }
            else
            {
                
            }
        }
        private void AccionAniadir(Enumerados.EstadosGestion estadoVentana)
        {
            switch (estadoVentana)
            {
                case Enumerados.EstadosGestion.Visualizando:
                case Enumerados.EstadosGestion.Seleccionando:
                    this.hdnEstadoEquipamiento.Value = ((int)Enumerados.EstadosGestion.Aniadiendo).ToString();
                    this.btnModificarEquipamiento.Enabled = false;
                    this.btnBorrarEquipamiento.Enabled = false;
                    this.btAnadirEquipamiento.Enabled = false;
                    this.pnDetalleEquipamiento.Visible = true;
                    this.btnAceptarEquipamiento.Text = "Guardar";
                    this.btnAceptarEquipamiento.Enabled = true;
                    this.btnCancelarEquipamiento.Enabled = true;
                    this.grdEquipamientos.Enabled = false;
                    this.txtPotenciaEquipamiento.Enabled = true;
                    this.cmbTipoEquipamiento.Enabled = true;
                    this.txtPotenciaEquipamiento.Text = "";
                    this.cmbTipoEquipamiento.SelectedIndex = -1;
                    //this.cmbTipoEquipamientoCustomValidator.Enabled = true;
                    //this.txtPotenciaEquipamientoCustomValidator.Enabled = true;
                    ActivarValidacionEquipamiento();
                    break;
                default:
                    throw new UIException(true, "4001");
            }
        }
        private void AccionSeleccionar(Enumerados.EstadosGestion estadoVentana)
        {
            this.hdnEstadoEquipamiento.Value = ((int)Enumerados.EstadosGestion.Seleccionando).ToString();
            switch (estadoVentana)
            {
                case Enumerados.EstadosGestion.Visualizando:
                case Enumerados.EstadosGestion.Seleccionando:
                    this.btnBorrarEquipamiento.Enabled = true;
                    this.btnModificarEquipamiento.Enabled = true;
                    this.btAnadirEquipamiento.Enabled = true;
                    break;
                default:
                    throw new UIException(true, "4001");
            }
        }
        private void AccionModificar(Enumerados.EstadosGestion estadoVentana)
        {
            switch (estadoVentana)
            {
                case Enumerados.EstadosGestion.Seleccionando:

                    EquipamientoDTO equipamiento = ObtenerEquipamientoSeleccionado();
                    this.hdnEstadoEquipamiento.Value = ((int)Enumerados.EstadosGestion.Modificando).ToString();
                    this.btnModificarEquipamiento.Enabled = false;
                    this.btnBorrarEquipamiento.Enabled = false;
                    this.btAnadirEquipamiento.Enabled = false;
                    this.pnDetalleEquipamiento.Visible = true;
                    this.btnAceptarEquipamiento.Text = "Actualizar";
                    this.btnAceptarEquipamiento.Enabled = true;
                    this.btnCancelarEquipamiento.Enabled = true;
                    this.grdEquipamientos.Enabled = false;
                    this.txtPotenciaEquipamiento.Enabled = true;
                    this.cmbTipoEquipamiento.Enabled = true;
                    if (equipamiento.Potencia != null)
                    {
                        this.txtPotenciaEquipamiento.Text = equipamiento.Potencia.ToString();
                    }
                    else
                    {
                        this.txtPotenciaEquipamiento.Text = "";
                    }
                    
                    this.cmbTipoEquipamiento.SelectedValue = equipamiento.IdTipoEquipamiento.ToString();
                    ActivarValidacionEquipamiento();
                    break;
                default:
                    throw new UIException(true, "4001");
            }
        }
        private void AccionEliminar(Enumerados.EstadosGestion estadoVentana)
        {
            switch (estadoVentana)
            {
                case Enumerados.EstadosGestion.Seleccionando:
                    EquipamientoDTO equipamiento = ObtenerEquipamientoSeleccionado();
                    this.hdnEstadoEquipamiento.Value = ((int)Enumerados.EstadosGestion.Eliminando).ToString();
                    this.btnModificarEquipamiento.Enabled = false;
                    this.btnBorrarEquipamiento.Enabled = false;
                    this.btAnadirEquipamiento.Enabled = false;
                    this.pnDetalleEquipamiento.Visible = true;
                    this.btnAceptarEquipamiento.Text = "Borrar";
                    this.btnAceptarEquipamiento.Enabled = true;
                    this.btnCancelarEquipamiento.Enabled = true;
                    this.txtPotenciaEquipamiento.Enabled = false;
                    this.grdEquipamientos.Enabled = false;
                    this.cmbTipoEquipamiento.Enabled = false;
                    if (equipamiento.Potencia.HasValue)
                    {
                        this.txtPotenciaEquipamiento.Text = equipamiento.Potencia.Value.ToString();
                    }
                    else
                    {
                        this.txtPotenciaEquipamiento.Text = "";
                    }
                    this.cmbTipoEquipamiento.SelectedValue = equipamiento.IdTipoEquipamiento.ToString();
                    break;
                default:
                    throw new UIException(true, "4001");
            }
        }
        private CalderaDTO RecuperarInformacionFormulario()
        {
            CalderaDTO caldera = new CalderaDTO();
            caldera.Anio = FormUtils.GetInt(this.txtAnio);
            caldera.CodigoContrato = this.hdnCodigoContrato.Value;
            caldera.ModeloCaldera = this.txtModeloCaldera.Text;
            caldera.Potencia = this.txtPotencia.Text;
            caldera.Uso = FormUtils.GetInt(this.cmbUsoCaldera);
            caldera.IdTipoCaldera = FormUtils.GetInt(this.cmbTiposCaldera);
            caldera.IdMarcaCaldera = FormUtils.GetInt(this.cmbMarcasCaldera);
            caldera.DecripcionMarcaCaldera = this.cmbMarcasCaldera.SelectedItem.Text;// this.txtMarcasCaldera.Text;

            return caldera;
        }
        private void ActualizarCaldera(CalderaDTO caldera)
        {
            if (caldera.ModeloCaldera == null || caldera.ModeloCaldera.Length == 0)
            {
                caldera.ModeloCaldera = "Desconocido";
            }
            if (Calderas.EsCalderaValida(caldera))
            {
                Calderas.ActualizarInsertarCaldera(caldera);
            }
        }
        private bool TieneCaldera()
        {
            if (
                (FormValidation.TextBoxHasValue(txtMarcasCaldera) ||
                (cmbTiposCaldera.SelectedIndex != 0)) &&
                (FormValidation.TextBoxHasValue(txtPotencia) ||
                (cmbUsoCaldera.SelectedIndex != 0)) &&
                //(FormValidation.TextBoxHasValue(txtModeloCaldera)) ||
                (FormValidation.TextBoxHasValue(txtAnio))
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ActivarValidacionCaldera()
        {
            //txtAnioCustomValidator.Enabled = true;
            //txtAnioCustomValidator.Validate();
            txtPotenciaCustomValidator.Enabled = true;
            txtPotenciaCustomValidator.Validate();
            //txtMarcasCalderaCustomValidator.Enabled = true;
            //txtMarcasCalderaCustomValidator.Validate();
            cmbUsoCalderaCustomValidator.Enabled = true;
            cmbUsoCalderaCustomValidator.Validate();
            cmbTiposCalderaCustomValidator.Enabled = true;
            cmbTiposCalderaCustomValidator.Validate();
            
        }
        private void DesactivarValidacionCaldera()
        {
            txtPotenciaCustomValidator.Enabled = false;
            txtMarcasCalderaCustomValidator.Enabled = false;
            cmbUsoCalderaCustomValidator.Enabled = false;
            cmbTiposCalderaCustomValidator.Enabled = false;
            
        }
        private void ActivarValidacionEquipamiento()
        {
            cmbTipoEquipamientoCustomValidator.Enabled = true;
            //cmbTipoEquipamientoCustomValidator.Validate();
            txtPotenciaEquipamientoCustomValidator.Enabled = true;
            //txtPotenciaEquipamientoCustomValidator.Validate();
        }
        private void DesactivarValidacionEquipamiento()
        {
            cmbTipoEquipamientoCustomValidator.Enabled = false;
            txtPotenciaEquipamientoCustomValidator.Enabled = false;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			   
            try
            {
                this.cmbMarcasCaldera.Attributes.Add("onChange", "DisplayText();");
                if (!Page.IsPostBack)
                {
                    // ponemos el estado a visualizando
                    this.hdnEstadoEquipamiento.Value = ((int)Enumerados.EstadosGestion.Visualizando).ToString();

                    // cargamos los datos de la caldera y de los equipamientos
                    String strContrato = (String)Request.QueryString["COD_CONTRATO"];
                    if (strContrato == null)
                    {
                        CargarCombos(null);
                    }
                    else
                    {
                        this.hdnCodigoContrato.Value = strContrato;
                        CalderaDTO caldera = Calderas.ObtenerCaldera(strContrato);
                        if (caldera != null)
                        {
                            CargarCombos(caldera.IdMarcaCaldera);
                        }
                        else
                        {
                            CargarCombos(null);
                        }

                        CargarDatosCaldera(caldera);
                        CargarDatosEquipamiento(strContrato);
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnGrdEquipamientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                    e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");
                    e.Row.Attributes["OnClick"] =
                        Page.ClientScript.GetPostBackClientHyperlink(this.grdEquipamientos, "Select$" + e.Row.RowIndex.ToString());
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        //    e.Row.Cells(i).ToolTip = 
                        //        Tildes(DirectCast(DirectCast(DirectCast(e.Row.Cells[15], 
                        //                        System.Web.UI.WebControls.TableCell).Controls[1],
                        //                        System.Web.UI.Control), 
                        //               System.Web.UI.WebControls.Label).ToolTip);

                        e.Row.Cells[i].Attributes["style"] += "cursor:pointer;";
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        #region eventos de la gestión de la lista de equipamientos
        protected void OnRowSelected_Click(object sender, EventArgs e)
        {
            switch (Enumerados.ObtenerEstadoGestion(this.hdnEstadoEquipamiento.Value) )
            {
                case Enumerados.EstadosGestion.Seleccionando:
                case Enumerados.EstadosGestion.Visualizando:
            
                      // ponemos el id del equipamiento seleccionado en el hidden
                    if (this.grdEquipamientos.SelectedIndex != -1)
                    {
                        this.hdnIndexEquipamientoSeleccionado.Value = this.grdEquipamientos.SelectedIndex.ToString();
                    }
                    else
                    {
                        this.hdnIndexEquipamientoSeleccionado.Value = "";
                    }
                    // llamamos a la transición de estados necesaria
                    this.CambiarEstadoVentana(Enumerados.AccionesGestion.Seleccionar);
                    break;

                default:
                    if (this.hdnIndexEquipamientoSeleccionado.Value != null && this.hdnIndexEquipamientoSeleccionado.Value.Length > 0)
                    {
                        this.grdEquipamientos.SelectedIndex = Int32.Parse(this.hdnIndexEquipamientoSeleccionado.Value);
                    }
                    break;
            }
        }
        protected void CmbTipoPotencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtPotencia.Text = cmbTipoPotencia.SelectedValue;
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void CmbTipoEquipamiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTipoEquipamiento.SelectedValue == "1")
                {
                    txtPotenciaEquipamiento.Enabled = true;
                    this.Label8.Enabled = true;
                }
                else
                {
                    txtPotenciaEquipamiento.Enabled = false;
                    this.Label8.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OncmbTipoEquipamiento_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ComboBoxHasValue(cmbTipoEquipamiento, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void txtPotenciaEquipamiento_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                //e.IsValid = FormValidation.ValidateNumberTextBox(txtPotenciaEquipamiento, 2, 0, true, (BaseValidator)sender);
                e.IsValid = true;
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnBtnAceptarEquipamiento_Click(object sender, EventArgs e)
        {
            if (this.ValidatePage())
            {
                this.CambiarEstadoVentana(Enumerados.AccionesGestion.Aceptar);
                this.grdEquipamientos.Enabled = true;
            }
        }
        protected void OnBtnCancelarEquipamiento_Click(object sender, EventArgs e)
        {
            this.CambiarEstadoVentana(Enumerados.AccionesGestion.Cancelar);
            this.grdEquipamientos.Enabled = true;
        }
        protected void btAnadirEquipamiento_Click(object sender, EventArgs e)
        {
            this.CambiarEstadoVentana(Enumerados.AccionesGestion.Aniadir);
        }
        protected void btModificarEquipamiento_Click(object sender, EventArgs e)
        {
            this.CambiarEstadoVentana(Enumerados.AccionesGestion.Modificar);
        }
        protected void btnBorrarEquipamiento_Click(object sender, EventArgs e)
        {
            this.CambiarEstadoVentana(Enumerados.AccionesGestion.Eliminar);
        }
        #endregion

        #region métodos de la parte de calderas

        protected void OnBntLimpiarCaldera_Click(object sender, EventArgs e)
        {
            try
            {
                // actualizamos el control que miraremos para ver si eliminamos
                // la caldera o no cuando se de a salvar cambios.
                this.hdnCalderaEliminada.Value = "1";

                // eliminamos los valores de los campos de la caldera
                this.txtAnio.Text = "";
                this.txtModeloCaldera.Text = "";
                this.txtPotencia.Text = "";
                // TODO seleccionar la opción nula
                this.cmbTiposCaldera.SelectedIndex = 0;
                // TODO seleccionar la opción nula
                this.cmbUsoCaldera.SelectedIndex = 0;
                // TODO seleccionar la opción nula
                this.cmbMarcasCaldera.SelectedIndex = 0;
                this.txtMarcasCaldera.Text = "";
                DesactivarValidacionCaldera();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnTxtMarcasCaldera_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.TextBoxHasValue(this.txtMarcasCaldera);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OncmbTiposCaldera_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ComboBoxHasValue(cmbTiposCaldera, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OncmbUsoCaldera_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = FormValidation.ComboBoxHasValue(cmbUsoCaldera, (BaseValidator)sender);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OntxtPotencia_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                //e.IsValid = FormValidation.ValidateNumberTextBox(txtPotencia, 2, 2, true, (BaseValidator)sender);
                e.IsValid = true;
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OntxtAnio_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (FormValidation.ValidateNumberTextBox(txtAnio, false, (BaseValidator)sender) && txtAnio.Text.Length == 4)
                {
                    e.IsValid = true;
                }
                else
                {
                    e.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        #endregion

        protected void OnBtnAceptar_Click(object sender, EventArgs e)
        {

            Boolean datosInformados = false;
            //comprobamos que tiene los campos informados.
            if (TieneCaldera())
            {
                ActivarValidacionCaldera();
                datosInformados = true;
            }
            else
            {
                DesactivarValidacionCaldera();
                datosInformados = false;
            }

            if (this.ValidatePage())
            {
                MasterPageModal mpm = (MasterPageModal)this.Master;
                //Comprobamos si lo que quiere es darla de baja o cambiarla.
                if (this.hdnCalderaEliminada.Value != null && this.hdnCalderaEliminada.Value.Equals("1") && (!TieneCaldera()))
                {
                    // eliminamos la caldera
                    Calderas.EliminarCaldera(this.hdnCodigoContrato.Value);

                    // actualizar la lista de equipamiento
                    if (pnDetalleEquipamiento.Visible)
                    {
                        OnBtnAceptarEquipamiento_Click(sender, e);
                    }
                    List<EquipamientoDTO> listaEquipamientos = (List<EquipamientoDTO>)CurrentSession.GetAttribute(SessionVariables.LISTA_EQUIPAMIENTO);
                    List<EquipamientoDTO> listaEquipamientosBorrar = (List<EquipamientoDTO>)CurrentSession.GetAttribute(SessionVariables.LISTA_EQUIPAMIENTO_BORRAR);

                    Equipamientos.ActualizarEquipamientos(listaEquipamientos, listaEquipamientosBorrar);

                    mpm.CerrarVentanaModal();
                }
                else
                {
                    //Si no la quiere eliminar comprobamos que tiene los datos informados.
                    if (datosInformados)
                    {
                        CalderaDTO caldera = RecuperarInformacionFormulario();
                        ActualizarCaldera(caldera);

                        // actualizar la lista de equipamiento
                        if (pnDetalleEquipamiento.Visible)
                        {
                            OnBtnAceptarEquipamiento_Click(sender, e);
                        }
                        List<EquipamientoDTO> listaEquipamientos = (List<EquipamientoDTO>)CurrentSession.GetAttribute(SessionVariables.LISTA_EQUIPAMIENTO);
                        List<EquipamientoDTO> listaEquipamientosBorrar = (List<EquipamientoDTO>)CurrentSession.GetAttribute(SessionVariables.LISTA_EQUIPAMIENTO_BORRAR);

                        Equipamientos.ActualizarEquipamientos(listaEquipamientos, listaEquipamientosBorrar);

                        mpm.CerrarVentanaModal();
                    }
                    else
                    {
                        mpm.MostrarErrorAlert(Resources.TextosJavaScript.TEXTO_FALTAN_DATOS_OBLIGATORIOS_CALDERA);
                    }
                }
            }
        }
        protected void OnBtnCancelar_Click(object sender, EventArgs e)
        {
            MasterPageModal mpm = (MasterPageModal)this.Master;
            mpm.CerrarVentanaModal();
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
            // eliminar de sesión los valores del formulario si existían
        }
        #endregion

    }
}
