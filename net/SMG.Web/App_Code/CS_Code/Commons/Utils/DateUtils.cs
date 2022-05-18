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
    /// Descripción breve de DateUtils
    /// </summary>
    public class DateUtils
    {
        public DateUtils()
        {

        }

        public static Boolean IsDateTimeHTML(String strDate)
        {
            if (strDate != null && strDate.Length != 0)
            {
                try
                {
                    DateUtils.ParseDateTime(strDate);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public static DateTime ParseDateTime(String strDate)
        {
            IFormatProvider culture = new CultureInfo("es-ES", true);
            if (strDate == null)
            {
                return DateTime.Today;
            }
            else
            {
                return DateTime.Parse(strDate, culture);
            }
        }

        public static string DateTimeToSQL(DateTime date)
        {
            string strDate = "";
            strDate = date.Year + "-" + date.Month + "-" + date.Day;
            return strDate;
        }

        public static string DateTimeToHTML(DateTime date)
        {
            string strDate = "";
            strDate = date.Day + "-" + date.Month + "-" + date.Year;
            return strDate;
        }
    }
}
