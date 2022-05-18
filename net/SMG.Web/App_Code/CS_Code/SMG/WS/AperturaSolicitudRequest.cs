using System;

namespace Iberdrola.SMG.WS
{
    /// <summary>
    /// Contiene todos los datos necesarios para que el WS de cierre de visita pueda realizar
    /// el cierre de la visita.
    /// </summary>
    public class AperturaSolicitudRequest : WSRequest
    {
        public string CodigoContrato { get; set; }
        public string SubtipoSolicitud { get; set; }
        public string TelefonoContacto { get; set; }
        public string PersonaContacto { get; set; }
        public string Horario { get; set; }
        public string ObservacionesSolicitud { get; set; }
        public string TipoAveria { get; set; }
        public string TipoVisita { get; set; }
        public int? IdMarcaCaldera { get; set; }
        public string ModeloCaldera { get; set; }

        //BUA 19/01/2021 R#27548 - Generación masiva de solicitudes de Inspección
        public string OrdInLoco { get; set; }
        public string FechaPrevista { get; set; }
        public string HorarioInspeccion { get; set; }
        public string CambioPeriodoHorario { get; set; }
        //ENDBUA 19/01/2021 R#27548 - Generación masiva de solicitudes de Inspección


    }
}
