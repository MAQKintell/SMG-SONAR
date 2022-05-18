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
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;
using System.Collections.Generic;
using Iberdrola.SMG.WS;
using System.Reflection;
using Iberdrola.Commons.Utils;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de Calderas
    /// </summary>
    public class TicketCombustion
    {
        public TicketCombustion()
        {
        }

        /// <summary>
        /// Obtiene el TicketCombustionDTO que cumple con la PK y no esté de baja
        /// </summary>
        /// <param name="idTicketCombustion"></param>
        /// <returns>TicketCombustionDTO que cumple con la PK, null si no lo encuentra.</returns>
        public static TicketCombustionDTO Obtener(decimal idTicketCombustion)
        {
            TicketCombustionDB db = new TicketCombustionDB();
            return db.Obtener(idTicketCombustion);
        }

        /// <summary>
        /// Obtiene todos TicketCombustionDTO que no estén de baja
        /// </summary>
        /// <returns>Lista de TicketCombustionDTO con todos los objetos</returns>
        public static List<TicketCombustionDTO> ObtenerTodos()
        {
            TicketCombustionDB db = new TicketCombustionDB();
            return db.ObtenerTodos();
        }

        /// <summary>
        /// Obtiene el TicketCombustionDTO que cumple con la PK y no esté de baja
        /// </summary>
        /// <param name="codContrato"></param>
        /// <param name="codVisita"></param>
        /// <returns>TicketCombustionDTO que cumple con la PK, null si no lo encuentra.</returns>
        public static List<TicketCombustionDTO> ObtenerPorCodContratoYCodvisitaOIdSolicitud(string codContrato, decimal? idSolicitud, int? codVisita)
        {
            TicketCombustionDB db = new TicketCombustionDB();
            return db.ObtenerPorCodContratoYCodvisitaOIdSolicitud(codContrato, idSolicitud, codVisita);
        }

        /// <summary>
        /// Obtiene los ticket combustion para su visualizacion en un grid view
        /// </summary>
        /// <param name="codContrato"></param>
        /// <param name="idSolicitud"></param>
        /// <param name="codVisita"></param>
        /// <param name="idioma"></param>
        /// <returns>IDataReader que cumple con la PK, null si no lo encuentra.</returns>
        public static IDataReader ObtenerTodosPorCodContratoGridView(string codContrato, decimal? idSolicitud, int? codVisita, int idioma)
        {
            TicketCombustionDB db = new TicketCombustionDB();
            return db.ObtenerTodosPorCodContratoGridView(codContrato, idSolicitud, codVisita, idioma);
        }

        /// <summary>
        /// Obtiene los ticket combustion para su visualizacion en un grid view
        /// </summary>
        /// <param name="codContrato"></param>
        /// <param name="idSolicitud"></param>
        /// <param name="codVisita"></param>
        /// <returns>IDataReader que cumple con la PK, null si no lo encuentra.</returns>
        public static DataTable dtObtenerPorCodContratoYCodvisitaOIdSolicitud(string codContrato, decimal? idSolicitud, int? codVisita)
        {
            TicketCombustionDB db = new TicketCombustionDB();
            return db.dtObtenerPorCodContratoYCodvisitaOIdSolicitud(codContrato, idSolicitud, codVisita);
        }

        /// <summary>
        /// Guardar el TicketCombustionDTO
        /// </summary>
        /// <param name="dto">Datos a guardar</param>
        /// <param name="codUsuario">Nombre de usuario que registra la peticion</param>
        /// <returns>TicketCombustionDTO con la clave autogenerada informada (si la tuviera)</returns>
        public static TicketCombustionDTO GuardarTicketCombustion(TicketCombustionDTO dto, string codUsuario)
        {
            TicketCombustionDB db = new TicketCombustionDB();

            //Corregimos los ids por si se han guardado con 0
            dto.IdSolicitud = (dto.IdSolicitud == 0 ? null : dto.IdSolicitud);
            dto.CodigoVisita = (dto.CodigoVisita == 0 ? null : dto.CodigoVisita);

            return db.GuardarTicketCombustion(dto, codUsuario);
        }

        /// <summary>
        /// Obtenemos los datos del ws correspondientes al ticket de combustion
        /// </summary>
        /// <param name="cierreRequest">Datos del web service</param>
        /// <param name="codUsuario">Nombre de usuario que registra la peticion</param>
        /// <returns>Devolvemos el ticketCombustionDTO a guardar</returns>
        public static TicketCombustionDTO ObtenerTicketCombustionDTOFromRequest(TicketCombustionDTO requesTicketCombustiontDTO
                                                                                , Object cierreRequest
                                                                                , string usuarioRequest)
        {
            try
            {
                Type Datos = typeof(TicketCombustionDTO);

                //Nos recorremos todas las propiedades de la clase TicketCombustionDTO
                foreach (PropertyInfo pInfo in Datos.GetProperties())
                {
                    //Obtenemos la propiedad que viene en al request. 
                    PropertyInfo CampoRequest = cierreRequest.GetType().GetProperty(pInfo.Name);

                    if (CampoRequest != null)
                    {
                        if (cierreRequest.GetType().GetProperty(pInfo.Name) != null)
                        {
                            object dValor = null;
                            dValor = cierreRequest.GetType().GetProperty(pInfo.Name).GetValue(cierreRequest, null);

                            if(dValor != null && !string.IsNullOrEmpty(dValor.ToString()))
                            { 
                                //Convertimos la descripcion del tipo de equipo que viene del ws a numerico para guardarlo.
                                if (pInfo.Name == "TipoEquipo")
                                {
                                    int tipoEquipo = int.Parse(dValor.ToString());

                                    dValor = tipoEquipo;
                                }

                                requesTicketCombustiontDTO.GetType().GetProperty(pInfo.Name).SetValue(requesTicketCombustiontDTO, dValor, null);
                            }
                        }
                    }
                }

                //Obtenemos el CUPS del mantenimiento
                IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(requesTicketCombustiontDTO.CodigoContrato);

                if (datosMantenimiento != null)
                {
                    while (datosMantenimiento.Read())
                    {

                        if (string.IsNullOrEmpty(requesTicketCombustiontDTO.Proveedor))
                            requesTicketCombustiontDTO.Proveedor = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "proveedor_averia");

                        //20220218 BGN MOD BEG R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
                        if (string.IsNullOrEmpty(requesTicketCombustiontDTO.TelefonoContacto1))
                            requesTicketCombustiontDTO.TelefonoContacto1 = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NUM_TEL_CLIENTE");
                        //20220218 BGN MOD END R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente

                        if (string.IsNullOrEmpty(requesTicketCombustiontDTO.PersonaContacto))
                            requesTicketCombustiontDTO.PersonaContacto = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_TITULAR");
                    }
                }
            }
            catch (Exception ex)
            {
                requesTicketCombustiontDTO = null;
                throw;
            }

            return requesTicketCombustiontDTO;
        }

        /// <summary>
        /// Obtenemos una lista de objetos de tipo TipoCalderaDTO correspondiente a los ticket de combustion
        /// </summary>
        /// <param name="IdIdioma">Idioma correspondiente al tipo de calcera</param>
        /// <param name="tipoPeticion">Tipo de peticion correspondiente al web service</param>
        /// <returns>Devuelve lista de TipoCalderaDTO</returns>
        public static List<TipoCalderaDTO> ObtenerTiposCalderaTicketCombustion(Int16 IdIdioma, string tipoPeticion)
        {
            TipoCalderaDB tipoDB = new TipoCalderaDB();
            return tipoDB.ObtenerTiposCalderaTicketCombustion(IdIdioma, tipoPeticion);
        }

        //public static TicketCombustionDTO ObtenerTicketCombustion(string codContrato, decimal? codVisita)
        //{
        //    VisitaCaracteristica visitaCaracteristica = new VisitaCaracteristica();
        //    TicketCombustionDTO ticketCombustionDTO = new TicketCombustionDTO();
        //    List<VisitaCaracteristicaDTO> lVisitaCaracteristicaDTO = new List<VisitaCaracteristicaDTO>();

        //    //Obtenemos la correlacion de las caracteristicas con los campos de la request.
        //    ConfiguracionDTO confCamposWS = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_CAMPOS_REQUEST);
        //    List<string> lRequestCampos = confCamposWS.Valor.Split(';').ToList();

        //    //Obtenemos las caracteristicas relacionadas con el ticket de combustion
        //    lVisitaCaracteristicaDTO = visitaCaracteristica.ObtenerVisitaCaracteristicasTicketCombustion(codContrato, codVisita);

        //    if (lVisitaCaracteristicaDTO == null)
        //        return null;

        //    foreach (string campo in lRequestCampos)
        //    {
        //        string tipCar = campo.Split(':')[0];
        //        string nomCampoRequest = campo.Split(':')[1];

        //        PropertyInfo myPropInfo = ticketCombustionDTO.GetType().GetProperty(nomCampoRequest);

        //        foreach (VisitaCaracteristicaDTO visitaCaracteristicaDTO in lVisitaCaracteristicaDTO)
        //        {
        //            if (tipCar == visitaCaracteristicaDTO.TipCar)
        //            {
        //                dynamic d1 = null;
        //                d1 = visitaCaracteristicaDTO.Valor;

        //                if (myPropInfo.PropertyType == typeof(decimal) || myPropInfo.PropertyType == typeof(decimal?))
        //                {
        //                    d1 = Convert.ToDecimal(visitaCaracteristicaDTO.Valor);
        //                }

        //                ticketCombustionDTO.GetType().GetProperty(nomCampoRequest).SetValue(ticketCombustionDTO, d1, null);
        //            }
        //        }
        //    }

        //    return ticketCombustionDTO;
        //}

        //public static void GuardarTicketCombustionRequest(CierreVisitaRequest cierreVisita, string usuarioRequest)
        //{
        //    VisitaCaracteristica visitaCaracteristica = new VisitaCaracteristica();
        //    List<VisitaCaracteristicaDTO> lVisitaCaracteristicaDTO = new List<VisitaCaracteristicaDTO>();

        //    //Obtenemos la correlacion de las caracteristicas con los campos de la request.
        //    ConfiguracionDTO confCamposWS = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_CAMPOS_REQUEST);
        //    List<string> lRequestCampos = confCamposWS.Valor.Split(';').ToList();

        //    Type myType = cierreVisita.GetType();

        //    foreach (string campo in lRequestCampos)
        //    {
        //        VisitaCaracteristicaDTO visitaCaracteristicaDTO = new VisitaCaracteristicaDTO(); ;

        //        string tipCar = campo.Split(':')[0];
        //        string nomCampoRequest = campo.Split(':')[1];

        //        visitaCaracteristicaDTO.IdCaracteristica = null;
        //        visitaCaracteristicaDTO.CodContrato = cierreVisita.CodigoContrato;
        //        visitaCaracteristicaDTO.CodVisita = cierreVisita.CodigoVisita;
        //        visitaCaracteristicaDTO.TipCar = tipCar;

        //        PropertyInfo myPropInfo = myType.GetProperty(nomCampoRequest);
        //        Object objValor = myPropInfo.GetValue(cierreVisita, null);

        //        visitaCaracteristicaDTO.Valor = objValor.ToString();

        //        lVisitaCaracteristicaDTO.Add(visitaCaracteristicaDTO);
        //    }

        //    //Guardamos todas las caracteristicas
        //    visitaCaracteristica.GuardarListaVisitaCaracteristica(lVisitaCaracteristicaDTO, usuarioRequest);


        //    //return dto;
        //}

    }
}