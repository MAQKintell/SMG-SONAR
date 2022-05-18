using System;
using System.IO;
using System.Web;

namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Represents a class that can be used to render a javascript object that contains resource keys and values
    /// of a specific global resX file dependant on the CurrentUI culture.
    /// </summary>
    public class GlobalResourcesToJavaScript : ResourcesToJavaScriptBase
    {
        /// <summary>
        /// The name of the Global ResX file (ex: "Resource1" if the ResX file is "Resource1.resx")
        /// </summary>
        /*public string GlobalResXFileName
        {
            get;
            set;
        }*/

        private string _javaScriptObjectName;
        /// <summary>
        /// Sets and Gets the generated JavaScript object name. if not set it will return the normalized GlobalResXFileName.
        /// </summary>
        public override string JavaScriptObjectName
        {
            set
            {
                _javaScriptObjectName = value;
            }
            get
            {
                //if (!string.IsNullOrEmpty(_javaScriptObjectName) && _javaScriptObjectName.Trim() != string.Empty)
                //{
                return NormalizeVariableName(_javaScriptObjectName);
                //}
                //return NormalizeVariableName(GlobalResXFileName);
            }
        }

        //protected override string GetResourceValue(string key)
        //{
        //    string value = HttpContext.GetGlobalResourceObject(GlobalResXFileName, key) as string;
        //    return value == null ? string.Empty : value;
        //}

        /*protected override string GetResXFilePath()
        {
            //return Page.MapPath(Path.Combine("~//App_GlobalResources",  GlobalResXFileName + ".resx"));
            //return Page.MapPath(Path.Combine("./App_GlobalResources", GlobalResXFileName + ".resx"));
            return Path.Combine(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory.ToString(), "App_GlobalResources"), GlobalResXFileName + ".resx");
        }*/

        //protected override void ValidateBeforeRender(System.Web.UI.HtmlTextWriter writer)
        //{
        //    if (string.IsNullOrEmpty(GlobalResXFileName) || GlobalResXFileName.Trim() == string.Empty)
        //    {
        //        writer.Write("GlobalResourcesToJavaScript: " + this.ClientID + ": Please specify GlobalResXFileName");
        //        return;
        //    }
        //    base.ValidateBeforeRender(writer);
        //}
    }
}