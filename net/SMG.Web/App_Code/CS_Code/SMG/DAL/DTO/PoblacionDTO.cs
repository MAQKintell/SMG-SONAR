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
    public class PoblacionDTO : BaseDTO
    {
        #region atributos
        private String _cod_poblacion;
        private String _cod_provincia;
        private String _nombre;

        #endregion

        #region propiedades
        public String CodPoblacion
        {
            get { return this._cod_poblacion; }
            set { this._cod_poblacion = value; }
        }
        public String CodProvincia 
        {
            get { return this._cod_provincia; }
            set { this._cod_provincia = value; }
        }
        public String Nombre
        {
            get { return this._nombre; }
            set { this._nombre = value; }
        }
        #endregion

        #region constructores
        public PoblacionDTO()
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