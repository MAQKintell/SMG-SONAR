using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;


namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Acciones que tienen que ver con la configuración.
    /// </summary>
    public class Configuracion
    {

        public Configuracion()
        {
        }

        /// <summary>
        /// Obtenemos la entrada de configuración seleccionada.
        /// </summary>
        /// <param name="configuracion">Enumerado con la configuración deseada.</param>
        /// <returns>Objeto con la configuración seleccionada.</returns>
        public static ConfiguracionDTO ObtenerConfiguracion(Enumerados.Configuracion codConfiguracion)
        {
            ConfiguracionDB cDb = new ConfiguracionDB();

            return cDb.ObtenerConfiguracion(codConfiguracion);
        }

        /// <summary>
        /// Obtenemos la entrada de configuración seleccionada.
        /// </summary>
        /// <param name="configuracion">Enumerado con la configuración deseada.</param>
        /// <returns>Objeto con la configuración seleccionada.</returns>
        public static ConfiguracionDTO ObtenerConfiguracion(Enumerados.ConexionesYRutas codConfiguracion)
        {
            ConfiguracionDB cDb = new ConfiguracionDB();

            return cDb.ObtenerConfiguracion(codConfiguracion);
        }
    }
}