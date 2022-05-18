using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Resources;
using System.Globalization;

namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Represents a class that can be used to render a javascript object that contains resource keys and values
    /// of a specific resX file dependant on the CurrentUI culture.
    /// </summary>
    public abstract class ResourcesToJavaScriptBase : Control
    {
        /// <summary>
        /// Gets the full resX file path.
        /// </summary>
        /// <returns></returns>
        //protected abstract string GetResXFilePath();

        /// <summary>
        /// Sets and Gets the generated JavaScript object name.
        /// </summary>
        public abstract string JavaScriptObjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Get the resource value of specific key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        //protected abstract string GetResourceValue(string key);

        /*protected virtual void ValidateBeforeRender(System.Web.UI.HtmlTextWriter writer)
        {
            if (!File.Exists(GetResXFilePath()))
            {
                writer.Write("GlobalResourcesToJavaScript: " + this.ClientID + ": Can't find the file " + GetResXFilePath());
                return;
            }
            return;
        }*/
        protected override void OnLoad(EventArgs e)
        {
            if (!string.IsNullOrEmpty(JavaScriptObjectName) &&
                !Page.ClientScript.IsClientScriptBlockRegistered(GetType(), JavaScriptObjectName))
            {
                StringBuilder script = new StringBuilder();
                script.Append("<script type=\"text/javascript\"> ");

                Resources.TextosJavaScript.ResourceManager.ReleaseAllResources();
                ResourceSet resourceSet = Resources.TextosJavaScript.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

                //using ()
                //{

                script.Append(" var " + JavaScriptObjectName + " = { ");
                bool first = true;
                foreach (DictionaryEntry entry in resourceSet)
                {
                    if (first)
                        first = false;
                    else
                        script.Append(" , ");

                    script.Append(NormalizeVariableName(entry.Key as string));
                    script.Append(":");
                    script.Append("'" + entry.Value + "'");
                }
                script.Append(" }; ");
                //}

                //  using (ResourceSet resourceSet = Resources.TextosJavaScript.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
                // {

                Regex re = new Regex("#[0-9]#+");
                // GGB: ahora pasamos a añadirle parámetros a las variables que tengan el patrón #[0-9]# en su valor.
                foreach (DictionaryEntry entry in resourceSet)
                {
                    if (re.IsMatch((string)entry.Value))
                    {
                        script.Append(JavaScriptObjectName + "." + NormalizeVariableName(entry.Key as string) + "=	function (){");
                        script.Append(" var txt = '" + entry.Value + "';");
                        script.Append(" for (var i = 1; i <= arguments.length; i++){");
                        script.Append(" txt = txt.replace('#' + i + '#', arguments[i-1]);");
                        script.Append(" }");
                        script.Append(" return txt;");
                        script.Append("};");
                    }
                }
                //}

                script.Append("</script>");
                Page.ClientScript.RegisterClientScriptBlock(GetType(), JavaScriptObjectName, script.ToString(), false);
                // 19072016 IDIOMA, KINTELL, COMENTAMOS PARA QUE NOS DEJE ACCEDR A EL.
                //resourceSet.Close();
            }

            base.OnLoad(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //ValidateBeforeRender(writer);
            base.Render(writer);
        }

        /// <summary>
        /// Normalizes the variable names to be used as JavaScript variable names
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected static string NormalizeVariableName(string key)
        {
            return key.Replace('.', '_');
        }
    }

}