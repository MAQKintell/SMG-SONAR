using System;

namespace Iberdrola.SMG.WS
{
    /// <summary>
    /// Contiene datos de entrada para DatosSolicitud
    /// </summary>
    public class DatosSolicitudRequest : WSRequest
    {
        public string CodigoContrato { get; set; }
       

    }
}
