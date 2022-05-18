using System;
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
    public partial class FrmModalFiltrosColumna : FrmBase
    {
        //Las pruebas realizadas muestran que a partir de esta cifra hay dificultades con la pagina
        private static int _NUMERO_REGISTROS_MAXIMOS = 20000;

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
			
            //Variable que controla el estado de la carga
            bool resultadoOK = true;
            try
            {
                //Se comprueban la descripcion de la columna seleccionada y el tipo de vista selecionado
                // Son necesarios para poder obtener los valores
                if (Request.QueryString["NUM_COLUMNA_FILTRO"] == null || Request.QueryString["DESCRIPCION_COLUMNA_FILTRO"] == null || CurrentSession.GetAttribute("TipoUltimaVista") == null)
                {
                    this.ShowMessage("Error en la llamada, vuelva a intentarlo.");
                    resultadoOK = false;
                }

                if (!IsPostBack)
                {
                    if (resultadoOK)
                    {
                        //Se cargan los datos de la columna
                        resultadoOK = CargarFiltrosDeColumna();
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
                GuardarFiltrosDeColumna();

                //this.CerrarVentanaModal();

                // Lanza el evento de la ventana padre para que se aplique el nuevo filtro
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();

                mpm.PlaceHolderScript.Controls.Add(new LiteralControl("<script>parent.document.getElementById('ctl00_ContentPlaceHolderContenido_hdnBtnBuscar').click();</script>"));
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

        /// <summary>
        /// Carga los distintos datos de la columna seleccionada.
        /// La carga tiene un limite <see cref="_NUMERO_REGISTROS_MAXIMOS"/>
        /// </summary>
        /// <returns> Devuelve si la ejecución ha sido correcta o no</returns>
        public bool CargarFiltrosDeColumna()
        {
            // Variable que controla el estado de la carga
            bool resultadoOK = true;

            try
            {
                // Se cargan los filtros desde la sesion
                VistaContratoCompletoDTO filtrosDTO = (VistaContratoCompletoDTO)CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO);
                List<ObjetoTextoValorDTO> lista = null;

                //Se carga la descripcion de la columna seleccionada, necesaria pues es la columna sobre la que se obtendran los datos
                String columna = DefinicionColumnas.GetNombreColumnaTabla("grdListado", Request.QueryString["DESCRIPCION_COLUMNA_FILTRO"]);

                //Se carga el tipo de vista seleccionado, necesario para poder obtener los valores de la columna
                Enumerados.TipoVistaContratoCompleto tipoVista = (Enumerados.TipoVistaContratoCompleto)CurrentSession.GetAttribute("TipoUltimaVista");
                Hashtable parametros = (Hashtable)CurrentSession.GetAttribute("ParametrosUltimaVista");


                // Si la lista ya existe se obtiene de la sesion, sino se carga desde la BBDD
                if (filtrosDTO != null && filtrosDTO.ListasFiltrosColumna.ContainsKey(columna))
                {
                    lista = (List<ObjetoTextoValorDTO>)filtrosDTO.ListasFiltrosColumna[columna];
                }
                else
                {
                    // Si no existen los filtros se crean
                    if (filtrosDTO == null)
                    {
                        filtrosDTO = new VistaContratoCompletoDTO();
                    }

                    // Se obtiene el numero de datos distintos de la columna
                    int numReg = Contrato.ObtenerNumRegDatosDistintosColumnaContrato(tipoVista, parametros, filtrosDTO, columna);

                    if (numReg == 1)
                    {
                        this.ShowMessage("No se puede aplicar ningún filtro a una columna con un solo valor");
                        resultadoOK = false;
                    }
                    else
                    {
                        // Si el numero de registros esta por debajo de lo permitido se obtiene la lista de la BBDD
                        if (numReg < FrmModalFiltrosColumna._NUMERO_REGISTROS_MAXIMOS)
                        {
                            // Se obtiene la lista de la BBDD
                            lista = Contrato.ObtenerValoresDistintos(tipoVista, parametros, filtrosDTO, columna);
                            // Se aniade la lista a los filtros de la sesion para que la proxima vez no haya que realizar de nuevo la consulta a la BBDD
                            filtrosDTO.AniadirFiltroColumna(columna, lista);
                            CurrentSession.SetAttribute(SessionVariables.FILTRO_AVANZADO, filtrosDTO);
                        }
                        else
                        {
                            this.ShowMessage("Está tratando de filtrar demasiados valores distintos. Utilice los filtros de información o los avanzados para reducir el número.");
                            resultadoOK = false;
                        }
                    }
                }

                // Si se ha obtenido una lista y no hay problemas se carga en el control de la ventana
                if (lista != null && resultadoOK)
                {
                    // Se recorre la lista obtenida cargandola en el control
                    foreach (ObjetoTextoValorDTO objeto in lista)
                    {
                        this.cblFiltrosColumna.Items.Add(objeto.getListItem());
                    }
                    this.cblFiltrosColumna.DataBind();

                    // Se muestra el numero de datos cargados
                    this.pnFiltroColumna.GroupingText += " (" + cblFiltrosColumna.Items.Count + " Valores diferentes)";
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

            return resultadoOK;
        }

        /// <summary>
        /// Guarda en un hashtable en sesión la lista
        /// </summary>
        public void GuardarFiltrosDeColumna()
        {

            try
            {
                //Se carga la descripcion de la columna seleccionada, necesaria pues es la columna sobre la que se obtendran los datos
                String columna = DefinicionColumnas.GetNombreColumnaTabla("grdListado", Request.QueryString["DESCRIPCION_COLUMNA_FILTRO"]);

                // Se obtienen los filtros de la sesion y si no existen se crean unos nuevos
                VistaContratoCompletoDTO filtrosDTO = (VistaContratoCompletoDTO)CurrentSession.GetAttribute(SessionVariables.FILTRO_AVANZADO);
                if (filtrosDTO == null)
                {
                    filtrosDTO = new VistaContratoCompletoDTO();
                }

                // Se obtiene la lista guardada anteriormente
                List<ObjetoTextoValorDTO> lista = (List<ObjetoTextoValorDTO>)filtrosDTO.ListasFiltrosColumna[columna];

                bool todosSeleccionados = true;
                bool todosNoSeleccionados = true;
                // Se guarda si el valor ha sido o no seleccionado
                // Puesto que se ha cargado la lista manualmente el orden de ambas es el mismo
                for (int contador = 0; contador < lista.Count; contador++)
                {
                    lista[contador].seleccionado = this.cblFiltrosColumna.Items[contador].Selected;
                    if (lista[contador].seleccionado)
                    {
                        todosNoSeleccionados = false;
                    }
                    else
                    {
                        todosSeleccionados = false;
                    }
                }

                // Se guarda la lista actualizada en la sesion
                filtrosDTO.AniadirFiltroColumna(columna, lista);
                if (!todosSeleccionados && !todosNoSeleccionados)
                {
                    filtrosDTO.AniadirNumColumnaFiltro(int.Parse(Request.QueryString["NUM_COLUMNA_FILTRO"]));
                }
                else
                {
                    filtrosDTO.EliminarNumColumnaFiltro(int.Parse(Request.QueryString["NUM_COLUMNA_FILTRO"]));
                }
                CurrentSession.SetAttribute(SessionVariables.FILTRO_AVANZADO, filtrosDTO);
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
