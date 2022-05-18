using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de Paises
/// </summary>
public class Paises
{
	
        public Paises()
	    {
	    }

        /// <summary>
        /// Obtenemos el país que corresponde al id proporcionado
        /// </summary>
        /// <param name="idPais">Id del país a obtener</param>
        /// <returns>PaisDTO</returns>
        public static PaisDTO ObtenerPais(int idPais)
        {
            PaisDB paisDB = new PaisDB();
            return paisDB.ObtenerPais(idPais);
        }

        public IDataReader GetTodosLosPaises()
        {
            PaisDB paisDB = new PaisDB();
            return paisDB.GetTodosLosPaises();
        }
        /// <summary>
        /// Obtenemos una lista con los datos de los paises a los que tiene acceso el usuario.
        /// </summary>
        /// <returns>Devuelve una lista de "clases" PaisDTO con los datos de los lotes existentes.</returns>
        public static List<PaisDTO> ObtenerPaises(string strLoginUsuario)
        {
            PaisDB paisDB = new PaisDB();
            return paisDB.ObtenerPaises(strLoginUsuario);
        }

        /// <summary>
        /// Obtenemos una lista con los datos de los paises a los que tiene acceso el usuario.
        /// </summary>
        /// <returns>Devuelve una lista de "clases" PaisDTO con los datos de los lotes existentes.</returns>
        public static List<IdiomaDTO> ObtenerIdiomas()
        {
            PaisDB paisDB = new PaisDB();
            return paisDB.ObtenerIdiomas();
        }

        public static IdiomaDTO ObtenerIdioma(decimal idIdioma)
        {
            PaisDB paisDB = new PaisDB();
            return paisDB.ObtenerIdioma(idIdioma);
        }
}
