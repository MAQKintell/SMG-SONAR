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
    public class EquipamientoDTO : BaseDTO
    {
        #region atributos
        private Decimal? _id = null;
        private String _codContrato;
        private Decimal? _idTipoEquipamiento = null;
        private Double? _potencia = null;
        /// <summary>
        /// El campo _descripcionTipoEquipamiento no está en la tabla, pero se pone en el DTO 
        /// ya que es necesario tener tanto el id del tipo del equipamiento como su descripción
        /// en la pantalla de equipamientos
        /// </summary>
        private String _descripcionTipoEquipamiento;

        #endregion

        #region propiedades
        public Decimal? Id
        {
            get { return this._id; }
            set { this._id = value; }
        }
        public String CodContrato 
        {
            get { return this._codContrato; }
            set { this._codContrato = value; }
        }
        public Decimal? IdTipoEquipamiento
        {
            get { return this._idTipoEquipamiento; }
            set { this._idTipoEquipamiento = value; }
        }
        public Double? Potencia
        {
            get { return this._potencia; }
            set { this._potencia = value; }
        }
        public String DescripcionTipoEquipamiento
        {
            get { return this._descripcionTipoEquipamiento; }
            set { this._descripcionTipoEquipamiento = value; }
        }
        #endregion

        #region constructores
        public EquipamientoDTO()
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