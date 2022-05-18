using System;
using System.Collections.Generic;

   
namespace Iberdrola.SMG.WS
{
    public class WSDatosSolicitud
    {
        public string CodContrato { get; set; }
        public string NumSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
		public string Fecha_Creacion { get; set; }
		public string EstadoSolicitud { get; set; }
		public string Fecha_Cierre { get; set; }
        public string Cod_Barras { get; set; }
        public string MotivoCancelacion { get; set; }
        // A la espera de lo k nos diga Ruben 21/11/2017
        public string TipoAveria { get; set; }
        public string DescripcionAveria { get; set; }
        
    }
    /// <summary>
    /// Clase que contiene la respuesta a la llamada de
    /// </summary>
    public class DatosSolicitudResponse : WSResponse
    {


        public DatosSolicitudResponse() : base() {  }

        public List<WSDatosSolicitud> listaDatosSolicitud = new List<WSDatosSolicitud>(); 
        
        public void AddDatosSolicitud(  string sContrato, 
                                        string sNumVisita,
                                        string sTipoSolicitud,
                                        string sFecha_Creacion,
					                    string sAlias,
                                        string sFecha_Cierre,
                                        string sCod_Barras,
                                        string sMotivoCancelacion
                                        // A la espera de lo k nos diga Ruben 21/11/2017
                                        ,string sTipoAveria,
                                        string sDescripcionAveria
                                        
            )
        {
            WSDatosSolicitud lista = new WSDatosSolicitud();

            lista.CodContrato = sContrato;
            lista.NumSolicitud = sNumVisita;
            lista.TipoSolicitud = sTipoSolicitud;
            lista.Fecha_Creacion = sFecha_Creacion;
            lista.EstadoSolicitud = sAlias;
            lista.Fecha_Cierre = sFecha_Cierre;
            lista.Cod_Barras = sCod_Barras;
            lista.MotivoCancelacion = sMotivoCancelacion;
            // A la espera de lo k nos diga Ruben 21/11/2017
            lista.TipoAveria = sTipoAveria;
            lista.DescripcionAveria = sDescripcionAveria;
            
            this.listaDatosSolicitud.Add(lista);
        
        
        }
    }
}