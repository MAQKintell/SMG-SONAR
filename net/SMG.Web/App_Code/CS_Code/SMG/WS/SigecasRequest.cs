using System;

namespace Iberdrola.SMG.WS
{
    /// <summary>
    /// Contiene todos los datos necesarios para que el WS de cierre de visita pueda realizar
    /// el cierre de la visita.
    /// </summary>
    public class SigecasRequest : WSRequest
    {
        public string Aplicacion { get; set; }
        public string Usuario { get; set; }

        public string Email { get; set; }
        public Decimal? Id_Perfil { get; set; }
        public string Nombre { get; set; }
        public string Responsable { get; set; }
    }
}
