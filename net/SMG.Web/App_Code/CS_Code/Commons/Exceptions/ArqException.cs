using System;


namespace Iberdrola.Commons.Exceptions
{
    /// <summary>
    /// Excepciones que lanzarán las cases de arquitectura
    /// </summary>
    public class ArqException : BaseException
    {
        #region constructors
        public ArqException()
        {
        }

        public ArqException(String errorCode)
            : base(errorCode)
        {
        }

        public ArqException(Boolean recuperable,  String errorCode)
            : base(recuperable, errorCode)
        {
        }

        public ArqException(Boolean recuperable, Exception innerEx, String errorCode)
            : base(recuperable, innerEx, errorCode)
        {
        }
        #endregion
    }
}
