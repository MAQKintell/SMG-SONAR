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
    /// Descripción breve de CalderaDTO
    /// </summary>
    public class CalderaDTO : BaseDTO
    {
        #region atributos
        private String _cod_contrato;
        private Decimal? _id_tipo_caldera;
        private Decimal? _id_marca_caldera;
        private String _modelo_caldera;
        private Decimal? _uso;
        private String _potencia;
        private Int32? _anio;
        /// <summary>
        /// Ya que la marca de la caldera además de una opción puede ser un texto libre
        /// lo almacenamos en el DTO aunque en la tabla no exista ese campo
        /// Se utilizará para el método de insertar o actualizar caldera y si la descripción 
        /// de la marca de la caldera no existe se insertará una nueva marca de caldera en la
        /// tabla de la marca de calderas
        /// </summary>
        private String _desc_marca_caldera;


        #endregion

        #region propiedades
        public String CodigoContrato
        {
            get { return this._cod_contrato; }
            set { this._cod_contrato = value; }
        }
        public Decimal? IdTipoCaldera
        {
            get { return this._id_tipo_caldera; }
            set { this._id_tipo_caldera = value; }
        }
        public Decimal? IdMarcaCaldera
        {
            get { return this._id_marca_caldera; }
            set { this._id_marca_caldera = value; }
        }
        public String ModeloCaldera
        {
            get { return this._modelo_caldera; }
            set { this._modelo_caldera = value; }
        }
        public Decimal? Uso
        {
            get { return this._uso; }
            set { this._uso = value; }
        }
        public String Potencia
        {
            get { return this._potencia; }
            set { this._potencia = value; }
        }
        public Int32? Anio
        {
            get { return this._anio; }
            set { this._anio = value; }
        }

        public String DecripcionMarcaCaldera
        {
            set { this._desc_marca_caldera = value; }
            get { return this._desc_marca_caldera; }
        }


        #endregion

        #region constructores
        public CalderaDTO()
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