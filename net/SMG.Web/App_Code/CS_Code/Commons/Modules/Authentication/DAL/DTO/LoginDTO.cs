//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System;
using System.Collections.Generic;
using Iberdrola.Commons.BaseClasses;


namespace Iberdrola.Commons.Modules.Authentication.DAL.DTO
{
    /// <summary>
    /// Clase con los datos del usuario.
    /// </summary>
    public class LoginDTO : BaseDTO
    {
        /// <summary>
        /// UserId del usuario.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Password del usuario.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Número de documento de identidad del usuario.
        /// </summary>
        public string IdNumber { get; set; }
    }
}