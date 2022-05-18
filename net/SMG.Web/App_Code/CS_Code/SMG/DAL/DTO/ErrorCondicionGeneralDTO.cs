using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Iberdrola.SMG.DAL.DTO
{
    /// <summary>
    /// Descripción breve de ErrorCondicionGeneralDTO
    /// </summary>
    public class ErrorCondicionGeneralDTO : BaseDTO
    {
        #region atributos
        private Int32 _idError;
        private Int32 _codigo;
        private Boolean _casoGeneral;
        private Boolean _casoT10T11;
        #endregion

        #region propiedades
        public Int32 IdError 
        {
            get { return this._idError; }
            set { this._idError = value; }
        }

        public Int32 Codigo 
        {
            get { return this._codigo; }
            set { this._codigo = value; }
        }

        public Boolean CasoGeneral
        {
            get { return this._casoGeneral; }
            set { this._casoGeneral = value; }
        }

        public Boolean CasoT10T11
        {
            get { return this._casoT10T11; }
            set { this._casoT10T11 = value; }
        }

        #endregion

        #region constructores
        public ErrorCondicionGeneralDTO()
        {

        }
        #endregion

        #region  implementación de metodos heredados de BaseDTO
        public int CompareTo(object obj)
        {
            
            return -1;
        }

        public object Clone()
        {
            
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            
        }

        #endregion
    }
}