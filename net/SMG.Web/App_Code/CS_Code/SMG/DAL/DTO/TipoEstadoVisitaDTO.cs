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
    /// Descripción breve de TipoEstadoVisitaDTO
    /// </summary>
    public class TipoEstadoVisitaDTO : BaseDTO
    {
        #region atributos
        private String _codigo;
        private String _descripcion;

        #endregion

        #region propiedades
        public String Codigo
        {
            get { return this._codigo; }
            set { this._codigo = value; }
        }
        public String Descripcion 
        {
            get { return this._descripcion; }
            set { this._descripcion = value; }
        }

        #endregion

        #region constructores
        public TipoEstadoVisitaDTO()
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