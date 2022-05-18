using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.Serialization;

namespace Iberdrola.SMG.DAL.DTO
{
    /// <summary>
    /// Descripción breve de ContratoDTO
    /// </summary>
    public class ContratoDTO : BaseDTO
    {
        #region atributos
        private String id;
        #endregion

        #region propiedades
        public String Id 
        {
            get { return this.id; }
            set { this.id = value; }
        }

        #endregion

        #region constructores
        public ContratoDTO()
        {

        }
        #endregion

        #region metodos
        public string Imprimir()
        {
            return this.id;
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