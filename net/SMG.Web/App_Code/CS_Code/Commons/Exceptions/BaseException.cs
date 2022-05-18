using System;
using System.Text;


namespace Iberdrola.Commons.Exceptions
{
    /// <summary>
    /// Excepción base de las que extenderán todas las excepciones.
    /// </summary>
    public class BaseException : Exception
    {
        //TODO: ver si merece la pena usar enterprise library para las excepciones
        // no me gusta la idea de que por configuración se pueda cambiar el funcionamiento
        // de las excepciones y por tanto del programa
        public const String GENERAL_POLICY = "GENERAL_POLICY";

        #region atributes
        private Boolean _recuperable;
        private String _errorCode;
        #endregion

        #region properties
        public Boolean Recuperable 
        {
            set { this._recuperable = value; }
            get { return this._recuperable; }
        }

        public String ErrorCode
        {
            set { this._errorCode = value; }
            get { return this._errorCode; }
        }
        #endregion

        #region constructors
        public BaseException()
        {
        }

        public BaseException(String errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public BaseException(Boolean recuperable,  String errorCode)
        {
            this.Recuperable = recuperable;
            this.ErrorCode = errorCode;
        }

        public BaseException(Boolean recuperable, Exception innerEx, String errorCode)
            : base(innerEx != null ? innerEx.Message : "", innerEx)
        {
            this.Recuperable = recuperable;
            this.ErrorCode = errorCode;
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

            return message.ToString();
        }
    }
}