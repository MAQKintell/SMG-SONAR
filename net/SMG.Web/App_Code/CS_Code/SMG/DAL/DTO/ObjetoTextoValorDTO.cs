using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Iberdrola.SMG.DAL.DTO
{

    /// <summary>
    /// Objeto para almacenar un nombre visible, el valor y si esta seleccionado o no
    /// </summary>
    public class ObjetoTextoValorDTO
    {
        private string _texto;
        private object _valor;
        private bool _seleccionado;

        /// <summary>
        /// Constructor pasando solo el valor.
        /// Por defecto se asignara como texto la propiedad ToString() del objeto (salvo las fechas que tendran ToShortDateString)
        /// Por defecto se asignara como seleccionado a false
        /// </summary>
        /// <param name="valor"></param>
        public ObjetoTextoValorDTO(object valor)
        {
            this.objetoTextoValorDTO(valor, null, false);
        }

        /// <summary>
        /// Constructor pasando el valor y si esta seleccionado o no
        /// Por defecto se asignara como texto la propiedad ToString() del objeto (salvo las fechas que tendran ToShortDateString)
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="seleccionado"></param>
        public ObjetoTextoValorDTO(object valor, bool seleccionado)
        {
            this.objetoTextoValorDTO(valor, null, seleccionado);
        }

        /// <summary>
        /// Constructor pasando el valor y el texto
        /// Por defecto se asignara como seleccionado a false
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="seleccionado"></param>
        public ObjetoTextoValorDTO(object valor, string texto)
        {
            this.objetoTextoValorDTO(valor, texto, false);
        }

        /// <summary>
        /// Constructor pasando todos los valores
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="seleccionado"></param>
        public ObjetoTextoValorDTO(object valor, string texto, bool seleccionado)
        {
            this.objetoTextoValorDTO(valor, texto, seleccionado);
        }

        /// <summary>
        /// Metodo privado para asignar los valores (agrupando la asignacion en un unico metodo)
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="texto"></param>
        /// <param name="seleccionado"></param>
        private void objetoTextoValorDTO(object valor, string texto, bool seleccionado)
        {
            if (texto != null)
            {
                _texto = texto;
            }
            else
            {
                if (valor.GetType() == typeof(System.DateTime))
                {
                    _texto = ((DateTime)valor).ToShortDateString();
                }
                else
                {
                    _texto = valor.ToString();
                }
            }
            _valor = valor;
            _seleccionado = seleccionado;
        }

        /// <summary>
        /// Sobrescritura del metodo ToString() para controlar lo devuelto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        { return _texto; }

        /// <summary>
        /// Propiedad para obtener el texto
        /// </summary>
        public string texto
        {
            get { return _texto; }
        }
        
        /// <summary>
        /// Propiedad para obtener el valor
        /// </summary>
        public object valor
        {
            get { return _valor; }
        }

        /// <summary>
        /// Propiedad para obtener o establecer si esta seleccionado
        /// </summary>
        public bool seleccionado
        {
            get { return _seleccionado; }
            set { _seleccionado = value; }
        }

        /// <summary>
        /// Devuelve un objeto de tipo <see cref="ListItem"/> con los valores del objeto.
        /// El valor no se asigna pues en la clase <see cref="ListItem"/> solo puede ser de tipo <see cref="String"/>
        /// </summary>
        /// <returns></returns>
        public ListItem getListItem()
        {
            ListItem objetoLista = new ListItem(_texto);
            objetoLista.Selected = seleccionado;
            return objetoLista;
        }
    }
}



