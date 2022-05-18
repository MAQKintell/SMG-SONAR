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
    /// Descripción breve de TipoMarcaCalderaDTO
    /// </summary>
    public class TipoUsoCalderaDTO : BaseDTO
    {
        #region atributos
        private Decimal? _id;
        private String _descripcion;
        #endregion

        #region propiedades
        public Decimal? Id
        {
            get { return this._id; }
            set { this._id = value; }
        }
        public String Descripcion 
        {
            get { return this._descripcion; }
            set { this._descripcion = value; }
        }

        #endregion

        #region constructores
        public TipoUsoCalderaDTO()
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