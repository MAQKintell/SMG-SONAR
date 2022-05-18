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

using System.Linq;

using System.Xml.Linq;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Logging;

namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Descripción breve de MasterPageBase
    /// </summary>
    public abstract class MasterPageBase : System.Web.UI.MasterPage
    {
        public abstract PlaceHolder PlaceHolderScript
        {
            get;
        }

        #region metodos para mostrar errores
        public void MostrarErrorAlert(String message)
        {
            //message = "{fr: '" + message + "_FRANCES', es:'" + message + "'} ['" + HttpContext.Current.Request.UserLanguages[0].Substring(0, 2) + "']";
            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('" + message + "')</script>"));
        }

        public void ExecuteScript(string strScript)
        {
            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>" + strScript + "</script>"));
        }

        public void MostrarErrorAlert(BaseException be)
        {
            String mensaje = Resources.MensajesError.ResourceManager.GetString(be.ErrorCode);
            //mensaje = "{fr: '" + mensaje + "_FRANCES', es:'" + mensaje + "'} ['" + HttpContext.Current.Request.UserLanguages[0].Substring(0, 2) + "']";
            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('" + mensaje + "')</script>"));
        }

        public void MostrarErroresValidacionAlert()
        {
            String mensaje = "Los datos no son correctos. Revise los campos erróneos.";
            //mensaje = "{fr: '" + mensaje + "_FRANCES', es:'" + mensaje + "'} ['" + HttpContext.Current.Request.UserLanguages[0].Substring(0, 2) + "']";

            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('" + mensaje + "')</script>"));
        }

        public void MostrarErrorAlert(Errors listaErrores)
        {
            String mensaje = listaErrores.ToString(true, true, true);
            //mensaje = "{fr: '" + mensaje + "_FRANCES', es:'" + mensaje + "'} ['" + HttpContext.Current.Request.UserLanguages[0].Substring(0, 2) + "']";

            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('" + mensaje + "')</script>"));
        }

        public void OpenWindow(String url, String windowName, String windowTitle)
        {
            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirVentanaLocalizacion('" + url + "', '650', '450', '" + windowName + "', '" + windowTitle + "' ,'', false);</script>"));
        }

        public void OpenExternalWindow(String url, String windowName, String windowTitle)
        {
            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>window.open('" + url + "', '650', '450', '" + windowName + "', '" + windowTitle + "' ,'', false);</script>"));
        }

        public void OpenExternalWindowTamanio(String url, String windowName, String windowTitle,String Alto,String Ancho)
        {
            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>window.open('" + url + "', '" + Alto + "', '" + Ancho + "', '" + windowName + "', '" + windowTitle + "' ,'', false);</script>"));
        }

        public void MostrarPaginaError(Exception ex)
        {
            if (typeof(BaseException) == ex.GetType().BaseType)
            {
                MostrarPaginaErrorBase((BaseException)ex);
            }
            else
            {
                LogHelper.Category logCategory;
                logCategory = LogHelper.Category.General;
                LogHelper.Error(ex.Message, logCategory, ex);

                Iberdrola.Commons.Web.Error err = new Iberdrola.Commons.Web.Error(ex.Message);
                Iberdrola.Commons.Web.Errors errs = new Errors();
                errs.Add(err);
                Iberdrola.Commons.Web.CurrentSession.SetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_ERROR_VARIABLE, errs);
                Iberdrola.Commons.Web.NavigationController.Forward("./FrmMostrarError.aspx");
            }
        }

        public void MostrarPaginaErrorBase(BaseException be)
        {
            // Escribir el error en el Log
            LogHelper.Category logCategory;

            if (typeof(ArqException) == be.GetType())
            {
                logCategory = LogHelper.Category.Architecture;
            }
            else if (typeof(DALException) == be.GetType())
            {
                logCategory = LogHelper.Category.DataAccess;
            }
            else if (typeof(BLLException) == be.GetType())
            {
                logCategory = LogHelper.Category.BussinessLogic;
            }
            else if (typeof(UIException) == be.GetType())
            {
                logCategory = LogHelper.Category.UserInterface;
            }
            else
            {
                logCategory = LogHelper.Category.Architecture;
            }

            if (be.Recuperable)
            {
                LogHelper.Warn(be.Message, logCategory, be);
            }
            else
            {
                LogHelper.Error(be.Message, logCategory, be);
            }

            // Mostrar la página de error
            Iberdrola.Commons.Web.Error err = Iberdrola.Commons.Web.Error.GetError(be);
            Iberdrola.Commons.Web.Errors errs = new Errors();
            errs.Add(err);
            Iberdrola.Commons.Web.CurrentSession.SetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_ERROR_VARIABLE, errs);
            //Iberdrola.Commons.Web.NavigationController.Forward("./FrmMostrarError.aspx", (FrmBase)this.ContentPlaceHolderContenido.Page);
            //Iberdrola.Commons.Web.NavigationController.Forward("./FrmMostrarError.aspx", this.ViewState);
            Iberdrola.Commons.Web.NavigationController.Forward("./FrmMostrarError.aspx");
        }

        public void MostrarPaginaError(Errors listaErrores)
        {
        }
        #endregion

        public void MostrarPantallaEspera()
        {
            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>MostrarCapaEspera()</script>"));
        }
        public void OcultarPantallaEspera()
        {
            this.PlaceHolderScript.Controls.Add(new LiteralControl("<script>OcultarCapaEspera()</script>"));
        }
    }
}
