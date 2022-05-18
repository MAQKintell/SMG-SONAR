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
using System.Security.Principal;
using System.Security.Authentication;
using System.Security.AccessControl;

namespace Iberdrola.Commons.Security
{
    /// <summary>
    /// User identity
    /// </summary>
    public class AppIdentity : GenericIdentity
    {
        private object _userObject = null;


        public AppIdentity(string name, object userObj)
            :base(name)

        {
            _userObject = userObj;
        }

        public object UserObject
        {
            get { return _userObject; }
        }
        
        public override string ToString()
        {
            if (_userObject != null)
            {
                return _userObject.ToString();
            }
            else
            {
                return "";
            }
        }

    }
}
