//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

namespace Iberdrola.Commons.Utils
{
    /// <summary>
    /// Clase para con utilidades para trabajar con strings.
    /// </summary>
    public class BooleanUtils
    {
        /// <summary>
        /// Devuelve el valor booleano que está en la cadena proporcionada
        /// </summary>
        /// <param name="strBoolean">Cadena a convertir a booleano</param>
        /// <returns>Booleano con el valor convertido</returns>
        public static bool Parse(string strBoolean)
        {
            if (string.IsNullOrEmpty(strBoolean))
            {
                return false;
            }

            strBoolean = strBoolean.ToUpper();

            if (
                strBoolean.Equals("1")
                ||
                strBoolean.Equals("SI")
                ||
                strBoolean.Equals("SÍ")
                ||
                strBoolean.Equals("S")
                ||
                strBoolean.Equals("TRUE")
                ||
                strBoolean.Equals("T")
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}