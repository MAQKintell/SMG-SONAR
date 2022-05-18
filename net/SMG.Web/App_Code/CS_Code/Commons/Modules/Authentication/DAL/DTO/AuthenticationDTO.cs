//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using Iberdrola.Commons.BaseClasses;

namespace Iberdrola.Commons.Modules.Authentication.DAL.DTO
{
    /// <summary>
    /// Clase con los datos del usuario.
    /// </summary>
    public class AuthenticationDTO : BaseDTO
    {
        /// <summary>
        /// Obtiene o Establece el identificador del usuario.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Obtiene o Establece el identificador del usuario.
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// Obtiene o Establece el código de las condición especial pendiente del usuario.
        /// </summary>
        public string CodVersion { get; set; }

        /// <summary>
        /// Obtiene o Establece el texto de las condición especial pendiente del usuario.
        /// </summary>
        public string TextoCondField { get; set; }
    }
}