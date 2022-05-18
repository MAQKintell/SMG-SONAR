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
    public class UsuarioSigecasDTO
    {
        #region atributos
        private String _login;
        private String _nombre;
        private String _email;
        private String _responsable;
        private Decimal? _idperfil;
        private DateTime? _fechaBaja;
        #endregion

        #region propiedades
        public String Login 
        {
            get { return this._login; }
            set { this._login = value; }
        }
        public String Nombre
        {
            get { return this._nombre; }
            set { this._nombre = value; }
        }
        public String Email
        {
            get { return this._email; }
            set { this._email = value; }
        }
        public String Responsable
        {
            get { return this._responsable; }
            set { this._responsable = value; }
        }
        public Decimal? Id_Perfil
        {
            get { return this._idperfil; }
            set { this._idperfil = value; }
        }

        #endregion

        #region constructores
        public UsuarioSigecasDTO()
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