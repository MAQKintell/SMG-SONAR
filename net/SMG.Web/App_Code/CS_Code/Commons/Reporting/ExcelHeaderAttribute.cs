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

namespace Iberdrola.Commons.Reporting
{
    /// <summary>
    /// Clase que contendrá la información de un Attributo de la cabecera de Excel
    /// </summary>
    public class ExcelHeaderAttribute
    {
        #region atributes
        private String _name;
        private String _value;
        #endregion

        #region Propiedades
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion

        #region constructores
        public ExcelHeaderAttribute()
        {
        }

        public ExcelHeaderAttribute(String name, String value)
        {
            this._name = name;
            this._value = value;
        }
        #endregion
    }
}