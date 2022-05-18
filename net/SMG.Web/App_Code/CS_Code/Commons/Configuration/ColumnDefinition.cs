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

namespace Iberdrola.Commons.Configuration
{
    /// <summary>
    /// Descripción breve de ColumDefinition
    /// </summary>
    public class ColumnDefinition
    {
        private String _id;
        private String _name;
        private Int32 _width;
        private String _style;

        public String Id
        {
            get { return _id; }
            set { this._id = value; }
        }

        public String Name
        {
            get { return _name; }
            set { this._name = value; }
        }

        public Int32 Width
        {
            get { return _width; }
            set { this._width = value; }
        }

        public String Style
        {
            get { return _style; }
            set { this._style = value; }
        }

        public ColumnDefinition(String id, String name, Int32 width, String style)
	    {
            this._id = id;
            this._name = name;
            this._width = width;
            this._style = style;
	    }

        public override bool Equals(object obj)
        {
            return this._id == ((ColumnDefinition)obj).Id;
        }
    }
}
