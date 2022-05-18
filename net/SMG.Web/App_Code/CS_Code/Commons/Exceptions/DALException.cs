using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Iberdrola.Commons.Exceptions
{
    /// <summary>
    /// Excepciones que lanzarán las cases de arquitectura
    /// </summary>
    public class DALException : BaseException
    {

        #region constructors
        public DALException()
        {
        }

        public DALException( String errorCode)
            : base(errorCode)
        {
        }

        public DALException(Boolean recuperable, String errorCode)
            : base(recuperable, errorCode)
        {
        }

        public DALException(Boolean recuperable, Exception innerEx, String errorCode)
            : base(recuperable, innerEx, errorCode)
        {
        }
        #endregion
    }
}
