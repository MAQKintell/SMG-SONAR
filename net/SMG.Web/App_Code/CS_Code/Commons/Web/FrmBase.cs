using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;


using Iberdrola.Commons.Configuration;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.Commons.Logging;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Reporting;
using Iberdrola.Commons.DataAccess;
using System.Reflection;
using System.Globalization;
using System.Threading;


namespace Iberdrola.Commons.Web
{
    public abstract class FrmBase : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            CultureInfo culture;

            if (CurrentSession.GetAttribute(CurrentSession.SESSION_USUARIO_CULTURA) != null)
            {
                culture = new CultureInfo(CurrentSession.GetAttribute(CurrentSession.SESSION_USUARIO_CULTURA).ToString());
            }
            else
            {
                culture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0]);
            }

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture.LCID);
            //call base class
            base.InitializeCulture();
        }

        public Boolean ValidateForm()
        {
            if (!Page.IsValid)
            {
                MasterPageBase mpb = (MasterPageBase)this.Master;
                mpb.MostrarErroresValidacionAlert();
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ManageException(Exception ex)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            String login = usuarioDTO.Login;
            String password = usuarioDTO.Password;

            //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
            //MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_2000_" + login + ":" + password, "INCIDENCIA EN OPERA SMG.");
            UtilidadesMail.EnviarMensajeError(" INCIDENCIA EN OPERA SMG.", ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_2000_" + login + ":" + password);
            //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            this.SaveSessionData();
            MasterPageBase mp = (MasterPageBase)this.Master;
            mp.MostrarPaginaError(ex);
        }

        public void ShowMessage(String message)
        {
            MasterPageBase mp = (MasterPageBase)this.Master;
            mp.MostrarErrorAlert(message);
        }


        public void ExecuteScript(string strScript)
        {
            MasterPageBase mp = (MasterPageBase)this.Master;
            mp.ExecuteScript(strScript);
        }

        public void OpenWindow(String url, String windowName, String windowTitle)
        {
            MasterPageBase mp = (MasterPageBase)this.Master;
            mp.OpenWindow(url, windowName, windowTitle);
        }

        public void OpenExternalWindow(String url, String windowName, String windowTitle)
        {
            MasterPageBase mp = (MasterPageBase)this.Master;
            mp.OpenExternalWindow(url, windowName, windowTitle);
        }

        public void OpenExternalWindowTamanio(String url, String windowName, String windowTitle,String Ancho,String Alto)
        {
            MasterPageBase mp = (MasterPageBase)this.Master;
            mp.OpenExternalWindowTamanio(url, windowName, windowTitle,Ancho,Alto);
        }
        

        public void ShowLoadingScreen()
        {
            MasterPageBase mp = (MasterPageBase)this.Master;
            mp.MostrarPantallaEspera();
        }

        
        /// <summary>
        /// Si se produce un error que no está contemplado
        /// lo tratamos en este evento.
        /// </summary>
        /// <param name="e"></param>
        //protected override void OnError(EventArgs e)
        //{
        //    HttpContext ctx = HttpContext.Current;
        //    Exception exception = ctx.Server.GetLastError();
        //    this.ManageException(exception);

        //    //base.OnError(e);
        //}

        public bool ValidatePage()
        {
            if (Page.IsValid)
            {
                return true;
            }
            else
            {
                ShowMessage(Resources.SMGConfiguration.ValidationErrorMessage);
                return false;
            }
        }

        public abstract void LoadSessionData();
        public abstract void SaveSessionData();
        public abstract void DeleteSessionData();

    }
}
