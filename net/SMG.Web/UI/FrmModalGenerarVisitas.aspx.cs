using System;
using System.Web.UI;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Exceptions;




using System.Data;


namespace Iberdrola.SMG.UI
{
    public partial class FrmModalGenerarVisitas : FrmBase
    { 
        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            try
            {
                if (!Page.IsPostBack)
                {
                    int intVisitasCerradas = 0;
                    int intVisitasCanceladas = 0;
                    int intVisitasSinVisita = 0;
                    int intVisitasLote = 0;

                    List<VisitaDTO> listaVisitas = (List<VisitaDTO>)CurrentSession.GetAttribute(SessionVariables.LISTA_VISITAS_GENERAR_LOTE);
                    if (listaVisitas != null && listaVisitas.Count > 0)
                    {
                        // Mostrar error de que no hay visitas seleccionadas
                        intVisitasCerradas = Lotes.ObtenerNumeroVisitasCerradas(listaVisitas);
                        intVisitasCanceladas = Lotes.ObtenerNumeroVisitasCanceladas(listaVisitas);
                        intVisitasSinVisita = Lotes.ObtenerNumeroVisitasContratoSinVisita(listaVisitas);
                        intVisitasLote = listaVisitas.Count - intVisitasCerradas - intVisitasSinVisita;

                    }

                    this.lblVisitasCanceladas.Text = intVisitasCanceladas.ToString();
                    this.lblVisitasIncluidas.Text = intVisitasLote.ToString();
                    this.lblVisitasNoIncluidas.Text = intVisitasCerradas.ToString();

                    if (intVisitasLote == 0)
                    {
                        this.btnGenerar.Enabled = false;
                    }
                    else
                    {
                        this.btnGenerar.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

        }
        protected void OnBtnCancelar_Click(object sender, EventArgs e)
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
        protected void OnBtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtTextoLote.Enabled = false;
                this.btnCancelar.Enabled = false;
                this.btnGenerar.Enabled = false;

                List<VisitaDTO> listaVisitas = (List<VisitaDTO>)CurrentSession.GetAttribute(SessionVariables.LISTA_VISITAS_GENERAR_LOTE);
                Decimal? codLote = Lotes.GenerarLote(txtTextoLote.Text, listaVisitas,CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_USUARIO_VALIDO).ToString());

                if (codLote.HasValue)
                {
                    this.lbMensajeFinal.Text = "El lote ha sido generado correctamente. El número de lote asignado es: ";
                    this.lbIdLote.Text = codLote.Value.ToString();
                    this.lbIdLote.Visible = true;
                }
                else
                {
                    this.lbMensajeFinal.Text = "El lote NO ha sido generado correctamente. ";
                    this.lbIdLote.Text = "";
                }
                this.btnAceptar.Visible = true;
                this.lbMensajeFinal.Visible = true;
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
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
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
    }
}
