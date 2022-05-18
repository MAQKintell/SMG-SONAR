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
    public class MenuDTO : BaseDTO
    {
        #region atributos
        private Decimal? _id_menu;
        private Int32? _modulo;
        private String _orden;
        private String _texto;
        private String _desc_menu;
        private String _link;
        #endregion

        #region propiedades
        public Decimal? IdMenu
        {
            get { return this._id_menu; }
            set { this._id_menu = value; }
        }
        public Int32? Modulo
        {
            get { return this._modulo; }
            set { this._modulo = value; }
        }
        public String Orden
        {
            get { return this._orden; }
            set { this._orden = value; }
        }
        public String Texto
        {
            get { return this._texto; }
            set { this._texto = value; }
        }

        public String DescMenu
        {
            get { return this._desc_menu; }
            set { this._desc_menu = value; }
        }

        public String Link
        {
            get { return this._link; }
            set { this._link = value; }
        }

        #endregion

        #region constructores
        public MenuDTO()
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