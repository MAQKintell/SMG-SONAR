//*******************************************************************
// Copyright © 2016 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Métodos de acceso a base de datos para la entidad Documento
    /// </summary>
    public partial class Documento
    {
        #region Private Methods
        /// <summary>
        /// Indica si el nombre del documento es correcto o no
        /// </summary>
        /// <param name="dto">Documento a validar</param>
        /// <returns>true si es válida false si no</returns>
        private static bool NombreFicheroCorrecto(DocumentoDTO dto, string strCCBB)
        {
            StringBuilder str = new StringBuilder();
            str.Append(strCCBB);
            str.Append("-");
            str.Append(dto.CodContrato);
            str.Append("-");
            str.Append(strCCBB);

            return !string.IsNullOrEmpty(dto.NombreDocumento) && dto.NombreDocumento.Contains(str.ToString());
        }

        /// <summary>
        /// Indica si la extensión del documento está aceptada según la configuración
        /// </summary>
        /// <param name="dto">Documento a validar</param>
        /// <returns>true si es válida false si no</returns>
        private static bool ExtensionValida(DocumentoDTO dto)
        {
            // extarct and store the file extension into another variable
            string extension = ManejadorFicheros.ObtenerExtensionFichero(dto.NombreDocumento).ToUpper();

            ConfiguracionDTO cfgExtensiones = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.EXTENSIONES_DOCUMENTOS);
            if (cfgExtensiones != null && !string.IsNullOrEmpty(cfgExtensiones.Valor))
            {
                string[] extensionesValidas = cfgExtensiones.Valor.ToUpper().Split(';');

                // loop over the array of valid file extensions to compare them with uploaded file
                foreach (string extensionValida in extensionesValidas)
                {
                    if (extensionValida == extension)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Mueve el documento a la ruta de destino de Gestión Documental y actualiza la fecha de envío
        /// en el registro de la tabla DOCUMENTO. Si está configurada la variable de configuración
        /// de guardar los enviados en la ruta de envíados de SMG lo copia a esa ruta renombrando con
        /// fecha y hora.
        /// </summary>
        /// <param name="dto">DTO con todos los datos del documento a enviar</param>
        /// <param name="strUsuario">Usuario que realiza la acción</param>
        private static void EnviarAGestionDocumental(DocumentoDTO dto, string strUsuario)
        {
            ConfiguracionDTO cgfRutaDestinoSmg = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_DESTINO_DOCUMENTOS);
            ConfiguracionDTO cgfRutaDestinoSmgEnviados = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_DESTINO_DOCUMENTOS_ENVIADOS);
            ConfiguracionDTO cgfRutaDestinoGesdocor = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_DESTINO_DOCUMENTOS_GESDOCOR);

            if (cgfRutaDestinoSmg != null && !string.IsNullOrEmpty(cgfRutaDestinoSmg.Valor)
                &&
                cgfRutaDestinoGesdocor != null && !string.IsNullOrEmpty(cgfRutaDestinoGesdocor.Valor))
            {
                string strRutaSmg = ManejadorFicheros.CombinarRutas(cgfRutaDestinoSmg.Valor, dto.NombreDocumento);
                string strRutaGesdocor = ManejadorFicheros.CombinarRutas(cgfRutaDestinoGesdocor.Valor, dto.NombreDocumento);

                // Si tenemos configurada la ruta de backup de enviados primero lo copiamos ahí
                if (cgfRutaDestinoSmgEnviados != null && !string.IsNullOrEmpty(cgfRutaDestinoSmgEnviados.Valor))
                {
                    string strRutaSmgEnviados = ManejadorFicheros.CombinarRutas(cgfRutaDestinoSmgEnviados.Valor, dto.NombreDocumento);
                    ManejadorFicheros.CopiarFichero(strRutaSmg, strRutaSmgEnviados, true);
                }

                //Movemos el fichero a la ruta de destino
                ManejadorFicheros.MoverFichero(strRutaSmg, strRutaGesdocor, false);

                // Actualizamos la fecha de envío del documento.
                dto.FechaEnvioDelta = DateTime.Now;
                Documento.Actualizar(dto, strUsuario);
            }
        }

        /// <summary>
        /// Obtiene los ficheros que están pendientes de enviar a gestión documental
        /// </summary>
        /// <returns>Lista de documentos</returns>
        private static List<DocumentoDTO> ObtenerNoEnviados()
        {
            DocumentoDB docDB = new DocumentoDB();
            return docDB.ObtenerNoEnviados();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Indica si el nombre y la extesión del fichero son validos.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="strCCBB"></param>
        /// <returns></returns>
        public static bool EsNombreValido(DocumentoDTO dto, string strCCBB)
        {
            // validamos que el nombre del fichero sea correcto
            if (!NombreFicheroCorrecto(dto, strCCBB))
            {
                return false;
            }            
            else if (!ExtensionValida(dto))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retorna el patron del nombre de fichero válido.
        /// </summary>
        /// <returns></returns>
        public static string PatronNombreValido()
        {
            string strFormato = "";
            ConfiguracionDTO cfgExtensiones = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.EXTENSIONES_DOCUMENTOS);
            if (cfgExtensiones != null && !string.IsNullOrEmpty(cfgExtensiones.Valor)) 
            {
                strFormato = cfgExtensiones.Valor.ToUpper();
            }

            return "CCBB_CONTRATO_TIPODOCUMENTO.[" + strFormato + "]";
        }

        //TODO: ELIMINAR CODIGO COMENTADO
        ///// <summary>
        ///// Sube el documento a la ruta de destino de SMG y guarda el registro en DOCUMENTO
        ///// </summary>
        ///// <param name="dto">DTO con todos los datos del documento a guadar</param>
        ///// <param name="strUsuario">Usuario que realiza la acción</param>
        //public static void GuardarDocumento(DocumentoDTO dto, string strUsuario)
        //{
        //    ConfiguracionDTO cgfRutaDestinoSmg = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_DESTINO_DOCUMENTOS);
            
        //    if (cgfRutaDestinoSmg != null && !string.IsNullOrEmpty(cgfRutaDestinoSmg.Valor))
        //    {
        //        // Subimos el documento a la ruta de destino
        //        string strRuta = ManejadorFicheros.CombinarRutas(cgfRutaDestinoSmg.Valor, dto.NombreDocumento);
        //        ManejadorFicheros.SubirFichero(dto.File, strRuta);

        //        // Insertamos el documento
        //        Documento.Insertar(dto, strUsuario);
        //    }
        //}

        /// <summary>
        /// Envía los ficheros pendientes a la ruta de gestión documental
        /// </summary>
        /// <param name="strUsuario">Usuario que realiza la acción</param>
        /// <returns>True si ha procesado algún fichero false si no</returns>
        public static bool EnviarNoEnviadosAGestionDocumental(string strUsuario)
        {
            bool bAlgunoPendiente = false;
            List<DocumentoDTO> listaDocumentos = Documento.ObtenerNoEnviados();
            if (listaDocumentos != null && listaDocumentos.Count > 0)
            { 
                bAlgunoPendiente = true;
                foreach (DocumentoDTO docDto in listaDocumentos)
                {
                    EnviarAGestionDocumental(docDto, strUsuario);
                }
            }

            return bAlgunoPendiente;
        }

        public static int ObtenerIdTipoDocumento(string codTipoDocumento)
        {
            DocumentoDB docDB = new DocumentoDB();
            return docDB.ObtenerIdTipoDocumento(codTipoDocumento);
        }

        //20200520 BGN ADD BEG R#21754: Colgar facturas en información de reclamación
        public static List<DocumentoDTO> ObtenerDocumentosPorIdSolicitud(int idSolicitud)
        {
            DocumentoDB docDB = new DocumentoDB();
            return docDB.ObtenerDocumentosPorIdSolicitud(idSolicitud);
        }
        //20200520 BGN ADD END R#21754: Colgar facturas en información de reclamación

        //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
        public static List<DocumentoDTO> ObtenerDocumentosPorTicketCombustion(string codContrato, decimal? idSolicitud, int? codVisita)
        {
            DocumentoDB docDB = new DocumentoDB();
            return docDB.ObtenerDocumentosPorTicketCombustion(codContrato, idSolicitud, codVisita);
        }
        //20201013 BUA END R#23245: Guardar el Ticket de combustión

        //20201013 BUA ADD R#23245: Guardar el Ticket de combustión
        public static DocumentoDTO ObtenerPorNombreDocumento(string codContrato, decimal? idSolicitud, int? codVisita, string nombreFichero) 
        {
            DocumentoDB docDB = new DocumentoDB();
            return docDB.ObtenerPorNombreDocumento(codContrato, idSolicitud, codVisita, nombreFichero);
        }
        //20201013 BUA END R#23245: Guardar el Ticket de combustión

        //20210111 BGN ADD BEG R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        public static List<DocumentoDTO> ObtenerDocumentosPorCodContrato(string codContrato)
        {
            DocumentoDB docDB = new DocumentoDB();
            return docDB.ObtenerDocumentosPorCodContrato(codContrato);
        }

        public static List<DocumentoDTO> ObtenerDocumentosGDPorIdSolicitud(int idSolicitud)
        {
            DocumentoDB docDB = new DocumentoDB();
            return docDB.ObtenerDocumentosGDPorIdSolicitud(idSolicitud);
        }
        //20210111 BGN ADD END R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental

        #endregion
    }
}