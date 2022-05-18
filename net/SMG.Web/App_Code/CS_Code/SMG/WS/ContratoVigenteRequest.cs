using System;

namespace Iberdrola.SMG.WS
{
    /// <summary>
    /// Contiene todos los datos necesarios para que el WS de Contrato Vigente
    /// </summary>
    public class ContratoVigenteRequest : WSRequest
    {
        public string CodigoContrato { get; set; }
       

    }
}
