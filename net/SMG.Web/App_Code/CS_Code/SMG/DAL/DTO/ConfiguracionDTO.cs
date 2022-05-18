using System.Runtime.Serialization;
using Iberdrola.SMG.BLL;


namespace Iberdrola.SMG.DAL.DTO
{
    /// <summary>
    /// Clase para las configuraciones.
    /// </summary>
    public class ConfiguracionDTO : BaseDTO
    {
        /// <summary>
        /// Código de la configuración.
        /// </summary>
        private Enumerados.Configuracion _codConfig;
        private Enumerados.ConexionesYRutas _codConexionYRutas;

        /// <summary>
        /// Valor de la configuración.
        /// </summary>
        private string _val;

        /// <summary>
        /// Tipo de dato.
        /// </summary>
        private Enumerados.TipoDatos _tipoDato;

        /// <summary>
        /// Descripción de la configuración.
        /// </summary>
        private string _descripcion;
            

        /// <summary>
        /// El constructor de la clase.
        /// </summary>
        public ConfiguracionDTO()
        {
        }

        /// <summary>
        /// Código de la configuración.
        /// </summary>
        /// <value>El valor del atributo _codConfig.</value>
        public Enumerados.Configuracion CodConfig
        {
            get { return this._codConfig; }
            set { this._codConfig = value; }
        }

        /// <summary>
        /// Código de la configuración.
        /// </summary>
        /// <value>El valor del atributo _codConfig.</value>
        public Enumerados.ConexionesYRutas CodConexionYRutas
        {
            get { return this._codConexionYRutas; }
            set { this._codConexionYRutas = value; }
        }

        /// <summary>
        /// Valor de la configuración.
        /// </summary>
        /// <value>El valor del atributo _val.</value>
        public string Valor
        {
            get { return this._val; }
            set { this._val = value; }
        }

        /// <summary>
        /// Tipo de dato.
        /// </summary>
        /// <value>El valor del atributo _tipoDato.</value>
        public Enumerados.TipoDatos TipoDato
        {
            get { return this._tipoDato; }
            set { this._tipoDato = value; }
        }

        /// <summary>
        /// Descripción de la configuración.
        /// </summary>
        /// <value>El valor del atributo _descripcion.</value>
        public string Descripcion
        {
            get { return this._descripcion; }
            set { this._descripcion = value; }
        }
        
        /// <summary>
        /// Método CompareTo.
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
}