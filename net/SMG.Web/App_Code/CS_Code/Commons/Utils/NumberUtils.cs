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

/// <summary>
/// Descripción breve de NumberUtils
/// </summary>
public class NumberUtils
{
    private static String _cultureString;
    
    static NumberUtils()
	{
        _cultureString = Resources.ArchitectureConfiguration.ApplicationCulture;
	}

    public static Boolean IsInteger(string strInt)
    {
        try
        {
            IFormatProvider culture = new CultureInfo(_cultureString, true);
            Int32.Parse(strInt, culture);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static Boolean IsNumber(string strNumber, int integerLenght, int decimalLenght)
    {
        try
        {
            //TODO: Validar que los decimales y los enteros coincidan con
            // los parámetros pasados
            IFormatProvider culture = new CultureInfo(_cultureString, true);
            Double dbl = Double.Parse(strNumber, culture);
            string[] sNumero = strNumber.Split(',');
            if (sNumero.Length == 2)
            {
                if ((sNumero[0].Length == 2) && (sNumero[1].Length == 2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        catch
        {
            return false;
        }
    }

    public static Boolean IsDecimal(string strNumber)
    {
        try
        {
            //TODO: Validar que los decimales y los enteros coincidan con
            // los parámetros pasados
            IFormatProvider culture = new CultureInfo(_cultureString, true);
            Decimal dbl = Decimal.Parse(strNumber, culture);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static Int32? GetInt(string strInt)
    {
        try
        {
            IFormatProvider culture = new CultureInfo(_cultureString, true);
            return Int32.Parse(strInt, culture);
        }
        catch
        {
            return null;
        }
    }

    public static Decimal? GetDecimal(string strInt)
    {
        try
        {
            IFormatProvider culture = new CultureInfo(_cultureString, true);
            return Decimal.Parse(strInt, culture);
        }
        catch
        {
            return null;
        }
    }

    public static Double? GetDouble(string strInt)
    {
        try
        {
            IFormatProvider culture = new CultureInfo(_cultureString, true);
            return Double.Parse(strInt, culture);
        }
        catch
        {
            return null;
        }
    }

}
