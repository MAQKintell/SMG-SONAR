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
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using System.Text;


namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Descripción breve de Errors
    /// </summary>
    public class Errors
    {
        private List<Error> _errors;
        private List<Error> _warnings;
        private List<Error> _infos;

        public void Add(Error err)
        {
            if (err == null)
            {
                throw new ArqException(false, "1007");
            }
            switch (err.Severity)
            {
                case Error.SeverityLevel.Info:
                    this.AddInfo(err);
                    break;
                case Error.SeverityLevel.Warning:
                    this.AddWarning(err);
                    break;
                default:
                    this.AddError(err);
                    break;
            }
        }

        private void AddInfo(Error err)
        {
            if (this._infos == null)
            {
                this._infos = new List<Error>();
            }
            this._infos.Add(err);
        }

        private void AddWarning(Error err)
        {
            if (this._warnings == null)
            {
                this._warnings = new List<Error>();
            }
            this._warnings.Add(err);
        }

        private void AddError(Error err)
        {
            if (this._errors == null)
            {
                this._errors = new List<Error>();
            }
            this._errors.Add(err);
        }

        public String ToString(Boolean showInfo, Boolean showWarnings, Boolean showErrors)
        {
            if (!showInfo && !showWarnings && !showErrors)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            if (showInfo && this._infos != null)
            {
                sb.Append("Info:\r\n");
                foreach (Error info in this._infos)
                {
                    sb.Append("\t");
                    sb.Append(info.Message);
                }
            }
            if (showWarnings && this._warnings != null)
            {
                sb.Append("Warning:\r\n");
                foreach (Error warning in this._warnings)
                {
                    sb.Append("\t");
                    sb.Append(warning.Message);
                }
            }
            if (showErrors && this._errors != null)
            {
                sb.Append("Error:\r\n");
                foreach (Error error in this._errors)
                {
                    sb.Append("\t");
                    sb.Append(error.Message);
                }
            }

            return sb.ToString();
        }

    }
}
