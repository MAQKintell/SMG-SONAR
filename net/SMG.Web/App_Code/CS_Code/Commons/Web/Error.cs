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
using Iberdrola.Commons.Exceptions;

namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Almacena la información de un error.
    /// </summary>
    public class Error
    {
        public enum SeverityLevel
        {
            Error,
            Warning,
            Info
        }

        private Boolean _genericError = false;
        private String _errorCode = String.Empty;
        private Error.SeverityLevel _severity = Error.SeverityLevel.Error;
        private String _message;


        public String ErrorCode
        {
            get { return this._errorCode; }
        }

        public SeverityLevel Severity
        {
            get { return this._severity; }
        }

        public String Message
        {
            get
            {
                try
                {
                    if (!this._genericError)
                    {
                        if (this._errorCode.Equals(String.Empty))
                        {
                            return String.Empty;
                        }
                        else
                        {
                            return Resources.MensajesError.ResourceManager.GetString(this._errorCode);
                        }
                    }
                    else
                    {
                        return this._message;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArqException(true, ex, "1001");
                }
            }
        }

        public Error(String message)
        {
            this._message = message;
            this._genericError = true;
        }

        public Error(String errorCode, SeverityLevel severity)
        {
            this._errorCode = errorCode;
            this._severity = severity;
        }

        public static Error GetError(BaseException be)
        {
            return new Error(be.ErrorCode, be.Recuperable ? SeverityLevel.Warning : SeverityLevel.Error);
        }
    }
}
