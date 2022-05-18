using System;
using System.Data;
using System.Web.UI;
using Iberdrola.Commons.Configuration;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.BLL;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Exceptions;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalBuscarUsuario : FrmBase
    {
        private string _filtro;

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
			
            bool resultadoOK = true;
            try
            {
                //Se comprueban la descripcion de la columna seleccionada y el tipo de vista selecionado
                // Son necesarios para poder obtener los valores
                if (Request.QueryString["OPCION"] == null)
                {
                    this.ShowMessage("Error en la llamada, vuelva a intentarlo.");
                    resultadoOK = false;
                }
                else
                {
                    switch (int.Parse(Request.QueryString["OPCION"]))
                    {
                        case 1:
                            pnBuscarPor.GroupingText = "Buscar Usuario por Login de Usuario";
                            _filtro = "UserID";
                            break;
                        case 2:
                            pnBuscarPor.GroupingText = "Buscar Usuario por Nombre de Usuario";
                            _filtro = "Name";
                            break;
                    }
                }

                if (!IsPostBack)
                {
                    if (resultadoOK)
                    {
                        //Se cargan los datos de la columna
                        //resultadoOK = CargarFiltrosDeColumna();
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

            if (!resultadoOK)
            {
                this.CerrarVentanaModal();
            }
        }

        /// <summary>
        /// Trata de cerrar la ventana modal
        /// </summary>
        protected void CerrarVentanaModal()
        {
            try
            {
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
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
                // Se guardan los filtros avanzados
                //GuardarFiltrosDeColumna();

                //this.CerrarVentanaModal();
                if (ListBox1.SelectedIndex >= 0)
                {
                    // Lanza el evento de la ventana padre para que se aplique el nuevo filtro
                    MasterPageModal mpm = (MasterPageModal)this.Master;
                    mpm.CerrarVentanaModal();

                    mpm.PlaceHolderScript.Controls.Add(new LiteralControl("<script>parent.document.getElementById('ctl00_ContentPlaceHolderContenido_hdnUserID').value='" + ListBox1.SelectedValue + "';</script>"));
                    mpm.PlaceHolderScript.Controls.Add(new LiteralControl("<script>parent.document.getElementById('ctl00_ContentPlaceHolderContenido_hdnBtnCargarUsuario').click();</script>"));
                }
                else 
                {
                    this.ShowMessage("Debe de seleccionar un resultado de la busqueda");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        /// <summary>
        /// Evento del botón cancelar de la pantalla.
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBtnCancelar_Click(object sender, EventArgs e)
        {
            this.CerrarVentanaModal();
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


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox1.Text.Trim()))
            {
                ShowMessage("Introduzca algún dato para poder realizar la búsqueda.");
            }
            else
            {
                DataTable usuarios = Usuarios.ObtenerUsuarios(_filtro, TextBox1.Text.Trim());
                this.ListBox1.DataSource = usuarios;
                ListBox1.DataTextField = "Descripcion";
                ListBox1.DataValueField = "UserID";
                ListBox1.DataBind();
            }
        }
}
}
