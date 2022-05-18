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
    public class UsuarioDTO : BaseDTO
    {
        #region atributos
            private String _login;
            private String _nombre;
            private String _password;
            private String _email;
            private Decimal? _idperfil;
            private Boolean? _permiso;
            private List<PerfilDTO> _listaRoles;
            // El campo proveedor no es de la tabla pero lo sacamos para que esté accesible
            private String _codProveedor;
            private String _nombreProveedor;
            private String _activo;
            private Boolean? _BAJA_AUTOMATICA;
            private int _idioma;
            private String _pais;
        #endregion

        #region propiedades
            /// <summary>
            /// Idioma del usuario.
            /// </summary>
            /// <value>El valor del atributo _idioma.</value>
            public int IdIdioma
            {
                get { return this._idioma; }
                set { this._idioma = value; }
            }
            public String Pais
            {
                get { return this._pais; }
                set { this._pais = value; }
            }

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
            public String Password
            {
                get { return this._password; }
                set { this._password = value; }
            }
            public String Email
            {
                get { return this._email; }
                set { this._email = value; }
            }
            public Decimal? Id_Perfil
            {
                get { return this._idperfil; }
                set { this._idperfil = value; }
            }
            public Boolean? Permiso
            {
                get { return this._permiso; }
                set { this._permiso = value; }
            }
            public List<PerfilDTO> ListaRoles
            {
                get { return this._listaRoles; }
                set { this._listaRoles = value; }
            }
            public String CodProveedor
            {
                get { return this._codProveedor; }
                set { this._codProveedor = value; }
            }
            public String NombreProveedor
            {
                get { return this._nombreProveedor; }
                set { this._nombreProveedor = value; }
            }
            public String Activo
            {
                get { return this._activo; }
                set { this._activo = value; }
            }
            public Boolean? BAJA_AUTOMATICA
            {
                get { return this._BAJA_AUTOMATICA; }
                set { this._BAJA_AUTOMATICA = value; }
            }
        #endregion

        #region constructores
        public UsuarioDTO()
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