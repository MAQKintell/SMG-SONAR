using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Web;
using System.Collections;
using System.Data;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using System.Threading;
using Iberdrola.Commons.Reporting;
using System.Globalization;
using System.Web.Configuration;
using Iberdrola.Commons.Configuration;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using System.Drawing;
using Iberdrola.Commons.DataAccess;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Reflection;
using Iberdrola.SMG.UI;

namespace Iberdrola.SMG.UI_PruebasDiseño
{
    public partial class frmPrueba : FrmBaseListado
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        #region implementación métodos abstractos
        public override void LoadSessionData()
        {

        }
        public override void SaveSessionData()
        {

        }
        public override void DeleteSessionData()
        {

        }
        #endregion
    }
}