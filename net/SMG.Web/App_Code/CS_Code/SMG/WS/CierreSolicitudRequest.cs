using System;

namespace Iberdrola.SMG.WS
{
    /// <summary>
    /// Contiene todos los datos necesarios para que el WS de cierre de visita pueda realizar
    /// el cierre de la visita.
    /// </summary>
    public class CierreSolicitudRequest : WSRequest
    {
        public int idSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }
        public string MotivoCancelacion { get; set; }
        public string ObservacionesSolicitud { get; set; }
        public int? IdMarcaCaldera { get; set; }
        public string ModeloCaldera { get; set; }
        public string CodigosCaracteristicas { get; set; }
        public string ValoresCaracteristicas { get; set; }
        public int TipoLugarAveria { get; set; }

        // DATOS FICHERO
        public string NombreFichero { get; set; }
        public byte[] ContenidoFichero { get; set; }

        public string NombreFicheroFactura { get; set; }
        public byte[] ContenidoFicheroFactura { get; set; }

        //20201021 BUA ADD BEG [R#23245]: Guardar el Ticket de combustión
        //DATOS TICKETS COMBUSTION
        public string TicketCombustion { get; set; } //S/N
        public string TipoEquipo { get; set; } //CALENTADOR, CALDERA ATMOSFÉRICA, CALDERA DE CONDENSACIÓN
        public decimal? TemperaturaPDC { get; set; }
        public decimal? COCorregido { get; set; }
        public decimal? Tiro { get; set; }
        public decimal? COAmbiente { get; set; }
        public decimal? O2 { get; set; }
        public decimal? CO { get; set; }
        public decimal? CO2 { get; set; }
        public decimal? Lambda { get; set; }
        public decimal? Rendimiento { get; set; }
        public string NombreFicheroConductoHumos { get; set; }
        public byte[] FicheroConductoHumos { get; set; }   //Foto
        public string Comentarios { get; set; }
        public decimal? TemperaturaMaxACS { get; set; }
        public decimal? CaudalACS { get; set; }
        public decimal? PotenciaUtil { get; set; }
        public decimal? TemperaturaEntradaACS { get; set; }
        public decimal? TemperaturaSalidaACS { get; set; }
        //20201021 BUA ADD END [R#23245]: Guardar el Ticket de combustión
    }
}
