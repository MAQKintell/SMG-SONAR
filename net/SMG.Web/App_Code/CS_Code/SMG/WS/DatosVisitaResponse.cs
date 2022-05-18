using System;
using System.Collections.Generic;

   
namespace Iberdrola.SMG.WS
{
    public class WSDatosVisita
    {
        public string CodContrato { get; set; }
        public string NumVisita { get; set; }
        public string EstadoVisita { get; set; }
		public string Periodo { get; set; }
		public string Fecha_Limite_Visita { get; set; }
		public string Fecha_Cierre { get; set; }
        public string Cod_Barras { get; set; }
        public string FECHA_PREVISTA_VISITA { get; set; }

        //20220210 BGN ADD BEG R#36755: Añadir CCBB del CCM en WSDatosVisita
        public string CCBB_DOC_VISITA_REALIZADA { get; set; }
        //20220210 BGN ADD END R#36755: Añadir CCBB del CCM en WSDatosVisita

    }
    /// <summary>
    /// Clase que contiene la respuesta a la llamada de
    /// </summary>
    public class DatosVisitaResponse : WSResponse
    {

        //private string _CONTRATO_VIGENTE = " ";
        //public string CONTRATO_VIGENTE { get { return _CONTRATO_VIGENTE; } set { _CONTRATO_VIGENTE = value; } }


        //private string _NUM_VISITA = " ";
        //public string NUM_VISITA { get { return _NUM_VISITA; } set { _NUM_VISITA = value; } }

        //private string _ESTADO_VISITA = " ";
        //public string ESTADO_VISITA { get { return _ESTADO_VISITA; } set { _ESTADO_VISITA = value; } }

        //private string _PERIODO_VISITA = " ";
        //public string PERIODO_VISITA { get { return _PERIODO_VISITA; } set { _PERIODO_VISITA = value; } }

        //private DateTime _FECHA_CIERRE = DateTime.Now;
        //public DateTime FECHA_CIERRE { get { return _FECHA_CIERRE; } set { _FECHA_CIERRE = value; } }

        

        public DatosVisitaResponse() : base() {  }

        public List<WSDatosVisita> listaDatosVisita = new List<WSDatosVisita>();

        //20220210 BGN MOD BEG R#36755: Añadir CCBB del CCM en WSDatosVisita
        public void AddDatosVisita(string sContrato, 
                                    string sNumVisita,
            		                string ALIAS,
					                string PERIODO,
					                string FECHA_LIMITE_VISITA,
					                string FECHA_CIERRE,
					                string COD_BARRAS,
                                    string FECHA_PREVISTA_VISITA,
                                    string CCBB_DOC_VISITA_REALIZADA)
        {
            WSDatosVisita lista = new WSDatosVisita();

            lista.CodContrato = sContrato;
            lista.NumVisita  = sNumVisita;
            lista.EstadoVisita = ALIAS;
            lista.Periodo  = PERIODO;
            lista.Fecha_Limite_Visita  = FECHA_LIMITE_VISITA;
            lista.Fecha_Cierre  = FECHA_CIERRE;
            lista.Cod_Barras  = COD_BARRAS;
            lista.FECHA_PREVISTA_VISITA = FECHA_PREVISTA_VISITA;
            lista.CCBB_DOC_VISITA_REALIZADA = CCBB_DOC_VISITA_REALIZADA;

            this.listaDatosVisita.Add(lista);        
        }
        //20220210 BGN MOD END R#36755: Añadir CCBB del CCM en WSDatosVisita
    }
}