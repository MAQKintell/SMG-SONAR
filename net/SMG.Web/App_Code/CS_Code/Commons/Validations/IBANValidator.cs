//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System;
using System.Text.RegularExpressions;
using Iberdrola.Commons.Utils;
using System.Text;
//using System.Numeric;
using System.Collections.Generic;
//using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Iberdrola.Commons.Validations
{
    /// <summary>
    /// Clase de utilidad para la validación de cadenas como número de cuentas IBAN
    /// </summary>
    public class IbanValidator
    {
        /// <summary>
        /// Indica si el texto proporcionado corresponde a un número de cuenta IBAN
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <returns>Booleano que indica si se trata o no de una cuenta IBAN</returns>
        public bool Validate(string value)
        {
            bool result = true;
            try
            {
                // Check if value is missing
                if (string.IsNullOrEmpty(value))
                    return false;

                if (value.Length < 2)
                    return false;

                var countryCode = value.Substring(0, 2).ToUpper();
                var lengthForCountryCode = _lengths[countryCode];
                // Check length.
                if (value.Length < lengthForCountryCode)
                    return false;

                if (value.Length > lengthForCountryCode)
                    return false;

                value = value.ToUpper();
                var newIban = value.Substring(4) + value.Substring(0, 4);

                newIban = Regex.Replace(newIban, @"\D", match => ((int)match.Value[0] - 55).ToString());

                var remainder = Decimal.Parse(newIban) % 97;

                if (remainder != 1)
                    return false;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        private static Dictionary<string, int> _lengths = new Dictionary<string, int>
        {
            {"AL", 28},
            {"AD", 24},
            {"AT", 20},
            {"AZ", 28},
            {"BE", 16},
            {"BH", 22},
            {"BA", 20},
            {"BR", 29},
            {"BG", 22},
            {"CR", 21},
            {"HR", 21},
            {"CY", 28},
            {"CZ", 24},
            {"DK", 18},
            {"DO", 28},
            {"EE", 20},
            {"FO", 18},
            {"FI", 18},
            {"FR", 27},
            {"GE", 22},
            {"DE", 22},
            {"GI", 23},
            {"GR", 27},
            {"GL", 18},
            {"GT", 28},
            {"HU", 28},
            {"IS", 26},
            {"IE", 22},
            {"IL", 23},
            {"IT", 27},
            {"KZ", 20},
            {"KW", 30},
            {"LV", 21},
            {"LB", 28},
            {"LI", 21},
            {"LT", 20},
            {"LU", 20},
            {"MK", 19},
            {"MT", 31},
            {"MR", 27},
            {"MU", 30},
            {"MC", 27},
            {"MD", 24},
            {"ME", 22},
            {"NL", 18},
            {"NO", 15},
            {"PK", 24},
            {"PS", 29},
            {"PL", 28},
            {"PT", 25},
            {"RO", 24},
            {"SM", 27},
            {"SA", 24},
            {"RS", 22},
            {"SK", 24},
            {"SI", 19},
            {"ES", 24},
            {"SE", 24},
            {"CH", 21},
            {"TN", 24},
            {"TR", 26},
            {"AE", 23},
            {"GB", 22},
            {"VG", 24}
        };
    }
}