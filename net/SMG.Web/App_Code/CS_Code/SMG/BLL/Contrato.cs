using System;
using Iberdrola.Commons.DataAccess;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Exceptions;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de Contrato
    /// </summary>
    public class Contrato : BaseBLL
    {
        public Contrato()
        {

        }

        #region Obtener Contratos

        public static Int32 ObtenerNumRegTotalContratosUltimaVisita(VistaContratoCompletoDTO filtrosDTO)
        {
            VistaContratoCompletoDB vistaDB = new VistaContratoCompletoDB();
            return vistaDB.ObtenerNumRegTotalContratosUltimaVisita(filtrosDTO);
        }

        #region Comun

        /// <summary>
        /// Devuelve el numero de registros que satisfacen todas las condiciones
        /// </summary>
        /// <param name="tipoVista"></param>
        /// <param name="parametros"></param>
        /// <param name="filtros"></param>
        /// <returns></returns>
        public static Int32 ObtenerNumRegEncontrados(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtrosDTO)
        {
            VistaContratoCompletoDB vistaDB = new VistaContratoCompletoDB();
            return vistaDB.ObtenerNumRegEncontrados(tipoVista, parametros, filtrosDTO);
        }
        /// <summary>
        /// Devuelve un <see cref="DataTable"/> con los datos de la vista seleccionada
        /// </summary>
        /// <param name="tipoVista"></param>
        /// <param name="parametros"></param>
        /// <param name="filtros"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="sortColumn"></param>
        /// <returns></returns>
        public static DataTable ObtenerVistaContratos(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtrosDTO, Int32? intPageIndex, String sortColumn)
        {
            VistaContratoCompletoDB vistaDB = new VistaContratoCompletoDB();
            return vistaDB.ObtenerVistaContratos(tipoVista, parametros, filtrosDTO, intPageIndex, sortColumn);
        }
        /// <summary>
        /// Devuelve una <see cref="List"/> de tipo <see cref="VisitaDTO"/> con los datos de la vista seleccionada
        /// </summary>
        /// <param name="tipoVista"></param>
        /// <param name="parametros"></param>
        /// <param name="filtros"></param>
        /// <returns></returns>
        public static List<VisitaDTO> ObtenerVistaContratos(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtrosDTO)
        {
            VistaContratoCompletoDB vistaDB = new VistaContratoCompletoDB();
            return vistaDB.ObtenerVistaContratos(tipoVista, parametros, filtrosDTO);
        }

        /// <summary>
        /// Devuelve los datos distintos que existen en una columna dada
        /// </summary>
        /// <param name="columna"> Columna de la que se quiere obtener los datos distintos</param>
        /// <returns></returns>
        public static List<ObjetoTextoValorDTO> ObtenerValoresDistintos(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtrosDTO, String nombreColumna)
        {
            VistaContratoCompletoDB vistaDB = new VistaContratoCompletoDB();

            return vistaDB.ObtenerDatosDistintosColumnaContrato(tipoVista, parametros, filtrosDTO, nombreColumna);
        }

        /// <summary>
        /// Devuelve el numero de datos distintos que existe en una columna dada
        /// </summary>
        /// <param name="columna"> Columna que se desea consultar </param>
        /// <returns></returns>
        public static Int32 ObtenerNumRegDatosDistintosColumnaContrato(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtrosDTO, String nombreColumna)
        {
            VistaContratoCompletoDB vistaDB = new VistaContratoCompletoDB();

            return vistaDB.ObtenerNumRegDatosDistintosColumnaContrato(tipoVista, parametros, filtrosDTO, nombreColumna);
        }

        #endregion Comun


        #endregion

        public IDataReader obtenerFiltrosAvanzadosExcelProvincia(VistaContratoCompletoDTO filtrosDTO)
        {
            VistaContratoCompletoDB vistaDB = new VistaContratoCompletoDB();
            return vistaDB.obtenerFiltrosAvanzadosExcelProvinciaDTO(filtrosDTO);
        }

        public IDataReader obtenerFiltrosAvanzadosExcelPoblacion(VistaContratoCompletoDTO filtrosDTO)
        {
            VistaContratoCompletoDB vistaDB = new VistaContratoCompletoDB();
            return vistaDB.obtenerFiltrosAvanzadosExcelPoblacionDTO(filtrosDTO);
        }
        public static IDataReader ObtenerVisitas(string Contrato,Int16 ididioma)
        {
            VisitasDB DetalleVisitaDB = new VisitasDB();
            return DetalleVisitaDB.ObtenerVisitas(Contrato, ididioma);
        }
        public static Int16? ObtenerCodigoUltimaVisita(String strContrato)
        {
            ContratoDB contratoDB = new ContratoDB();
            return contratoDB.ObtenerCodigoUltimaVisita(strContrato);
        }

        public static Boolean ComprobarSociedad(string contrato)
        {
            ContratoDB contratoDB = new ContratoDB();
            return contratoDB.ComprobarSociedad(contrato);
        }
        //20200127 BGN ADD BEG [R#20083] Nueva página para informar los contratos a NO Facturar la baja
        public bool FacturacionBajasRegularizacion(string contrato)
        {
            ContratoDB contratoDB = new ContratoDB();
            return contratoDB.FacturacionBajasRegularizacion(contrato);
        }

        public void InsertFacturacionBajasRegularizadas(string contrato, string fichero)
        {
            ContratoDB contratoDB = new ContratoDB();
            contratoDB.InsertFacturacionBajasRegularizadas(contrato, fichero);
        }
        //20200127 BGN ADD END [R#20083] Nueva página para informar los contratos a NO Facturar la baja


        public void InsertFacturacionBajasYMarcarContratoParaDarBaja(string contrato,string Usuario, string MotivoBaja)
        {
            ContratoDB contratoDB = new ContratoDB();
            contratoDB.InsertFacturacionBajasYMarcarContratoParaDarBaja(contrato,Usuario,MotivoBaja);
        }

        //20210719 BGN ADD BEG
        public static DataTable ObtenerFacturacionBajasPorContrato(string Contrato)
        {
            ContratoDB contratoDB = new ContratoDB();
            return contratoDB.ObtenerFacturacionBajasPorContrato(Contrato);
        }
        //20210719 BGN ADD END
    }
}
