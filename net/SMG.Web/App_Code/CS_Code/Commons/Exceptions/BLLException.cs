using System;

namespace Iberdrola.Commons.Exceptions
{
    /// <summary>
    /// Excepciones que lanzarán las cases de arquitectura
    /// </summary>
    public class BLLException : BaseException
    {
        #region constructors
        public BLLException()
        {
        }

        public BLLException(String errorCode)
            : base(errorCode)
        {
        }

        public BLLException(Boolean recuperable, String errorCode)
            : base(recuperable, errorCode)
        {
        }

        public BLLException(Boolean recuperable, Exception innerEx, String errorCode)
            : base(recuperable, innerEx, errorCode)
        {
        }
        #endregion
    }
}
