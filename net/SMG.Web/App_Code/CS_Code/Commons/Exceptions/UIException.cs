using System;
using System.Text;

namespace Iberdrola.Commons.Exceptions
{
    /// <summary>
    /// Excepciones que lanzarán las cases de arquitectura
    /// </summary>
    public class UIException : BaseException
    {
        #region atributes
        private String _url;
        private String _previousUrl;
        private String _sessionId;
        private String _user;
        #endregion

        #region properties
        public String Url
        {
            set { this._url = value; }
            get { return this._url; }
        }

        public String PreviousUrl
        {
            set { this._previousUrl = value; }
            get { return this._previousUrl; }
        }

        public String SessionId
        {
            set { this._sessionId = value; }
            get { return this._sessionId; }
        }

        public String User
        {
            set { this._user = value; }
            get { return this._user; }
        }
        #endregion

        #region constructors
        public UIException()
        {
        }

        public UIException( String errorCode)
            : base(errorCode)
        {
        }

        public UIException(Boolean recuperable, String errorCode)
            : base(recuperable, errorCode)
        {
        }

        public UIException(Boolean recuperable, Exception innerEx, String errorCode)
            : base(recuperable, innerEx, errorCode)
        {
        }

#endregion

        public override String ToString()
        {
            StringBuilder message = new StringBuilder();
            message.Append(this.Recuperable ? "" : "NON ");
            message.Append("recuperable exception. \r\n");

            message.Append("Error code: \r\n");
            message.Append(this.ErrorCode);
            message.Append("\r\n");

            message.Append("InnerException:");
            message.Append(this.InnerException);
            message.Append("\r\n");

            message.Append("User:");
            message.Append(this.User);
            message.Append("\r\n");

            message.Append("Session ID:");
            message.Append(this.SessionId);
            message.Append("\r\n");

            message.Append("URL:");
            message.Append(this.Url);
            message.Append("\r\n");

            message.Append("Previuos URL:");
            message.Append(this.PreviousUrl);
            message.Append("\r\n");

            return message.ToString();
        }
    }
}
