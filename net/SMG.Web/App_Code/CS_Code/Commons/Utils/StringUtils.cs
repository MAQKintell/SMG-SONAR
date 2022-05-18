using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;

namespace Iberdrola.Commons.Utils
{
    /// <summary>
    /// Descripción breve de StringUtils
    /// </summary>
    public class StringUtils
    {
        public StringUtils()
        {

        }

        /// <summary>
        /// Acorta los textos para mostrar los textos largos en el grid
        /// de forma abreviada
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string ReduceText(Object texto)
        {
            if (texto.ToString().Length > 31)
            {
                return texto.ToString().Substring(0, 30) + "...";
            }
            else
            {
                return texto.ToString();
            }
        }

        /// <summary>
        /// Formatea una cadena para que se visualice bien en HTML
        /// </summary>
        /// <param name="s">Cadena a formatear</param>
        /// <returns></returns>
        public static string FormatStringHTML(string s)
        {
            s = s.Replace("&#225;", "á");
            s = s.Replace("&#233;", "é");
            s = s.Replace("&#237;", "í");
            s = s.Replace("&#243;", "ó");
            s = s.Replace("&#250;", "ú");

            return s;
        }

        /// <summary>
        /// Decode
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns>string decodificado UTF8</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Encode
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns>string codificado UTF8</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


    }
}
