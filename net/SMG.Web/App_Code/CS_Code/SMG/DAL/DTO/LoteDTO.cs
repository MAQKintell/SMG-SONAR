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
    public class LoteDTO : BaseDTO
    {
        #region atributos
        private Decimal? _id;
        private String _descripcion;
        private DateTime? _fechaLote;
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
        public DateTime? FechaLote
        {
            get { return this._fechaLote; }
            set { this._fechaLote = value; }
        }

        public String CodigoDescripcion
        {
            get 
            {
                if (this._id.HasValue)
                {
                    return this._id.Value.ToString() + " - " + this._descripcion;
                }
                else
                {
                    return  "     " + this._descripcion;
                }
            }
        }


        #endregion

        #region constructores
        public LoteDTO()
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