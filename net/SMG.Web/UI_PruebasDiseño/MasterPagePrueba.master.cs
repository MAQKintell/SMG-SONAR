using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Iberdrola.Commons.Security;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Web;
using Iberdrola.Commons.Logging;
using Iberdrola.SMG.DAL.DTO;

using Iberdrola.SMG.BLL;
namespace Iberdrola.SMG.UI_PruebasDiseño
{
    public partial class MasterPagePrueba : MasterPageBase
    {
        public MasterPagePrueba()
        {

        }

        public override PlaceHolder PlaceHolderScript
        {
            get { return this._PlaceHolderScript; }
        }
     
     
    }
}
