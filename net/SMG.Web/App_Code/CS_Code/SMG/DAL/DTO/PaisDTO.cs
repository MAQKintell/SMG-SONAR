using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Iberdrola.SMG.DAL.DTO;
using System.Runtime.Serialization;

/// <summary>
/// Descripción breve de PaisDTO
/// </summary>
public class PaisDTO: BaseDTO
    {
        /// <summary>
        /// Id del país.
        /// </summary>
        private decimal _id;

        /// <summary>
        /// Código del país. Se puede utilizar para el código internacional del país
        /// Para el código de Delta.... para lo que se necesite.
        /// </summary>
        private string _codigo;

        /// <summary>
        /// Descripcion del pais.
        /// </summary>
        private string _descripcion;

        /// <summary>
        /// Idioma del pais.
        /// </summary>
        private int _idIdioma;
               
        /// <summary>
        /// El constructor de la clase.
        /// </summary>
        public PaisDTO()
        {
        }

        /// <summary>
        /// Id del lote.
        /// </summary>
        /// <value>El valor del atributo _id.</value>
        public decimal Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// Nombre del lote.
        /// </summary>
        /// <value>El valor del atributo _codigo.</value>
        public string Codigo 
        {
            get { return this._codigo; }
            set { this._codigo = value; }
        }

        /// <summary>
        /// Descripción del lote.
        /// </summary>
        /// <value>El valor del atributo _descripcion.</value>
        public string Descripcion
        {
            get { return this._descripcion; }
            set { this._descripcion = value; }
        }

        /// <summary>
        /// Idioma por defecto del país
        /// </summary>
        /// <value>El valor del atributo _idIdioma.</value>
        public int IdIdioma
        {
            get { return this._idIdioma; }
            set { this._idIdioma = value; }
        }
               
        /// <summary>
        /// Metodo CompareTo.
        /// </summary>
        /// <param name="obj" > Parametro Objeto.</param>
        /// <returns>El valor devuelto será true o false.</returns>
        public int CompareTo(object obj)
        {
            // TODO implementar el método
            return -1;
        }

        /// <summary>
        /// Metodo Clone.
        /// </summary>
        /// <returns>El valor devuelto será true o false.</returns>
        public object Clone()
        {
            // TODO implementar el método
            return null;
        }

        /// <summary>
        /// Metodo GetDataObject.
        /// </summary>
        /// <param name="info" > Parametro SerializationInfo.</param>
        /// <param name="context" > Parametro StreamingContext.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // TODO implementar el método
        }
  }
