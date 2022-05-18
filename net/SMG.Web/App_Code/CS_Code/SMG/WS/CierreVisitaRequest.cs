using System;

namespace Iberdrola.SMG.WS
{
    /// <summary>
    /// Contiene todos los datos necesarios para que el WS de cierre de visita pueda realizar
    /// el cierre de la visita.
    /// </summary>
    public class CierreVisitaRequest : WSRequest
    {
        public string CodigoContrato { get; set; } 
        public int CodigoVisita { get; set; }
        public string TelefonoContacto1 { get; set; }
        public string TelefonoContacto2 { get; set; }
        public int? IdTipoCaldera { get; set; }
        public int? IdMarcaCaldera { get; set; }
        public string ModeloCaldera { get; set; }
        public int? Uso { get; set; }
        public string Potencia { get; set; }
        public int? Anio { get; set; }
        public string DecripcionMarcaCaldera { get; set; }
        public string EstadoVisita { get; set; }
        public DateTime? FechaVisita { get; set; }
        public string ObservacionesVisita { get; set; }
        public bool? RecepcionComprobante { get; set; }
        public bool? FacturadoProveedor { get; set; }
        public DateTime? FechaFactura { get; set; }
        public string NumFactura { get; set; }
        public string CodigoBarrasVisita { get; set; }
        public bool? CartaEnviada { get; set; }
        public DateTime? FechaEnvioCarta { get; set; }
        public string TipoVisita { get; set; } // Aquí llega SIN DEFECTOS, ...
        public int? IdTecnico { get; set; } //public int16 Tecnico { get; set; } Nos llega el nombre

        // Duda, ver si lo mantenemos o lo cogemos del usuario que hace la petición.
        public string Proveedor { get; set; } //public short Tecnico { get; set; } Nos llega el nombre, no es necesario.
        public string ObservacionesTecnico { get; set; }
        public bool? ContadorInterno { get; set; }
        public bool? TieneReparacion { get; set; }
        public int? IdTipoReparacion { get; set; }
        public DateTime? FechaReparacion { get; set; }
        public int? IdTipoTiempoManoObra { get; set; }
        public decimal? CosteMateriales { get; set; }  //--> public decimal? ImporteMaterialesAdicional { get ;set ;}
        public decimal? ImporteManoObra { get; set; } // --> public decimal? ImporteManoObraAdicional { get; set; }
        public decimal? CosteMaterialesCliente { get; set; } // --> public decimal? ImporteReparacion { get ;set ;} 
        public DateTime? FechaFacturaReparacion { get; set; }
        public string NumeroFacturaReparacion { get; set; }
        public string CodigoBarrasReparacion { get; set; }
        public string UsuarioCierreVisita { get; set; }   // VisitaDTO --> CodigoInterno


        // DATOS EQUIPAMIENTO
        public string TipoEquipamiento { get; set; }
        public string PotenciaEquipamiento { get; set; }
        public string TipoProceso { get; set; }


        // DATOS FICHERO
        public string NombreFichero { get; set; }
        public byte[] ContenidoFichero { get; set; }

        // DATOS FECHA PREVISTA
        public string FechaPrevistaVisita { get; set; }
        public string HoraDesdePrevistaVisita { get; set; }
        public string HoraHastaPrevistaVisita { get; set; }

        //20200915 BGN ADD BEG [R#23245]: Guardar el Ticket de combustión
        //DATOS TICKETS COMBUSTION
        public string TicketCombustion { get; set; }    //S/N
        public string TipoEquipo { get; set; }  //CALENTADOR, CALDERA ATMOSFÉRICA, CALDERA DE CONDENSACIÓN
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

        //20210624 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
        public decimal? IdSolicitudAveria { get; set; }
        //20210624 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion

        //20200915 BGN ADD END [R#23245]: Guardar el Ticket de combustión


    }
}
