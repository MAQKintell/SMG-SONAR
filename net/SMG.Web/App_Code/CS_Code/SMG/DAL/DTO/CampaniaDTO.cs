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
    /// Descripción breve de ContratoDTO
    /// </summary>
    public class CampaniaDTO : BaseDTO
    {
        #region atributos
        private Int32? _campania;
        private String _observaciones;
        #endregion

        #region propiedades
        public Int32? Campania
        {
            get { return this._campania; }
            set { this._campania = value; }
        }
        public String Observaciones 
        {
            get { return this._observaciones; }
            set { this._observaciones = value; }
        }


        #endregion

        #region constructores
        public CampaniaDTO()
        {

        }
        #endregion

        #region  implementación de metodos heredados de BaseDTO
        public int CompareTo(object obj)
        {
            //TODO implementar el método
            return -1;
        }

        public object Clone()
        {
            //TODO implementar el método
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //TODO implementar el método
        }

        #endregion
    }
}