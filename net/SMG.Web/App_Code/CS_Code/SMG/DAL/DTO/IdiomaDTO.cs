using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Iberdrola.SMG.DAL.DTO;
using System.Globalization;
using System.Runtime.Serialization;

/// <summary>
/// Descripción breve de IdiomaDTO
/// </summary>
public class IdiomaDTO: BaseDTO
    {
        /// <summary>
        /// Id del país.
        /// </summary>
        private int _id;

       /// <summary>
        /// Descripcion del pais.
        /// </summary>
        private string _descripcion;

        /// <summary>
        /// Cultura del país
        /// </summary>
        private string _cultura;
        private CultureInfo _culture;
               
        /// <summary>
        /// El constructor de la clase.
        /// </summary>
        public IdiomaDTO()
        {
        }

        /// <summary>
        /// Id del lote.
        /// </summary>
        /// <value>El valor del atributo _id.</value>
        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
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
        /// Cultura en formato string del idioma.
        /// </summary>
        /// <value>El valor del atributo _cultura.</value>
        public string Cultura
        {
            get { return this._cultura; }
            set { this._cultura = value; }
        }

        /// <summary>
        /// Cultura en formato CultureInfo del idioma.
        /// </summary>
        /// <value>El valor del atributo _culture.</value>
        public CultureInfo Culture
        {
            get
            {
                //Si el CultureInfo no está creado lo intentamos crear
                if (_culture == null)
                {
                    if (!string.IsNullOrEmpty(_cultura))
                    {
                        _culture = new CultureInfo(_cultura, false);
                    }
                }
                return _culture;
            }
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
