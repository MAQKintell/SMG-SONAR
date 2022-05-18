using System.Configuration;

namespace Iberdrola.Commons.BaseClasses
{
    /// <summary>
    /// Clase base de la que extenderán todas las clases de acceso a base de datos.
    /// </summary>
    public abstract class BaseDB
    {
        /// <summary>
        /// Atributo en el que se guardará el nombre de la base de datos.
        /// Este valor se utiliza para coger la cadena de conexión.
        /// </summary>
        private static string _DB_NAME = string.Empty;

        /// <summary>
        /// Constructor estático. Obtiene de la configuración del webconfig 
        /// el nombre de la base de datos.
        /// </summary>
        static BaseDB()
        {
            // Cogemos de la propiedad DATA_BASE_NAME del web.config el nombre que tendrá
            // la base de datos. Este valor se utiliza para coger la cadena de conexión.
            BaseDB._DB_NAME = ConfigurationManager.AppSettings["DATA_BASE_NAME"];
        }

        /// <summary>
        /// Propiedad de acceso al atributo _DB_NAME. Que tiene el nombre de la base de datos
        /// a la que accederá la clase. Este valor se utiliza para coger la cadena de conexión.        
        /// </summary>
        /// <value> El valor del atributo _DB_NAME. </value>
        public static string DB_NAME
        {
            get { return BaseDB._DB_NAME; }
        }
    }
}
