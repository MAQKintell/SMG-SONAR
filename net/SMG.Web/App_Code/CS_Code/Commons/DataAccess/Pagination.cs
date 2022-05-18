//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System;

namespace Iberdrola.Commons.DataAccess
{
    /// <summary>
    /// Clase para almacenar los datos de paginación
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// El constructor de la clase.
        /// </summary>
        public Pagination()
        {
            //Añadimos por defecto la página actual como la 1 porque la página 0 no existe y es el valor por defecto de un int
            _PaginaActual = 1;
        }

        private string _DireccionOrden;

        /// <summary>
        /// Obtiene/Establece la dirección de la ordenación
        /// </summary>
        public string DireccionOrden
        {
            get { return _DireccionOrden; }
            set { _DireccionOrden = value; }
        }

        private string _CampoOrden;
        /// <summary>
        /// Obtiene/Establece el campo por el que se ordena
        /// </summary>
        public string CampoOrden
        {
            get { return _CampoOrden; }
            set { _CampoOrden = value; }
        }        

        private Int32 _PaginasTotales;
        /// <summary>
        /// Obtiene/Establece el número de páginas que hay
        /// </summary>
        public Int32 PaginasTotales
        {
            get { return _PaginasTotales; }
            set { _PaginasTotales = value; }
        }
        private Int32 _PaginaActual;
        /// <summary>
        /// Obtiene/Establece el número de página que se está visualizando.
        /// VALOR 0: indica que no aplicará ninguna paginación (con lo que devolverá todos los valores)
        /// </summary>
        public Int32 PaginaActual
        {
            get { return _PaginaActual; }
            set { _PaginaActual = value; }
        }

        /// <summary>
        /// Obtiene una copia del objeto actual
        /// </summary>
        /// <returns>Nuevo objeto Pagination con los datos de la actual</returns>
        public Pagination Clone()
        {
            Pagination copia = new Pagination();
            copia.DireccionOrden = this.DireccionOrden;
            copia.CampoOrden = this.CampoOrden;
            copia.PaginasTotales = this.PaginasTotales;
            copia.PaginaActual = this.PaginaActual;

            return copia;
        }
    }
}