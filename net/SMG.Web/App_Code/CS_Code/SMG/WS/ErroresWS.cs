using Iberdrola.Commons.Utils;

namespace Iberdrola.SMG.WS
{
    public enum Idioma : int
    {
        [StringValue("Castellano")]
        Castellano = 2,

        [StringValue("Ingles")]
        Ingles = 9
    }

    // Errores que empiezan por 233100
    public enum CommonWSError: int
    {
        [StringValue("ErrorCredencialesUsuario")]
        ErrorCredencialesUsuario = 233101,

        [StringValue("ErrorNoDefinido")]
        ErrorNoDefinido = 233199,

        [StringValue("ErrorWSInactivo")]
        ErrorWSInactivo = 233200,

        [StringValue("VisitaActualizadaCorrectamente")]
        VisitaActualizadaCorrectamente = 0
    }

    // Errores que empiezan por 233200
    public enum CierreVisitaWSError : int
    {
        [StringValue("ErrorCodigoVisitaIncorrecto")]  //
        ErrorCodigoVisitaIncorrecto = 233201,
        [StringValue("ErrorVisitaNoEncontrada")]  //
        ErrorVisitaNoEncontrada = 233202,
        [StringValue("BadVisitNotFound")]  //
        BadVisitCode = 933202,
        [StringValue("ErrorCodigoEstadoVisitaIncorrecto")] // 
        ErrorCodigoEstadoVisitaIncorrecto = 233203,
        [StringValue("ErrorHistoricoVisitaNoEncontrado")] // NO TIENE MOVIMIENTO DE HISTORICO PARA ESA VISITA
        ErrorHistoricoVisitaNoEncontrado = 233204,
        [StringValue("ErrorTecnicoSuperaLimiteVisitasDia")] // ESTE TÉCNICO HA ALCANZADO EL LÍMITE DE 12 VISITAS/DÍA
        ErrorTecnicoSuperaLimiteVisitasDia = 233205,
        [StringValue("ErrorTelefonoContacto1Posiciones")] // EL TELEFONO DE CONTACTO 1 NO TIENE 12 POSICIONES
        ErrorTelefonoContacto1Posiciones = 233206,
        [StringValue("ErrorCodigoBarrasVisita")] // EL CODIGO DE BARRAS DE LA VISITA DEBE SER NUMERICO Y TENER 13 o 14 DÍGITOS
        ErrorCodigoBarrasVisita = 233207,
        [StringValue("ErrorCodigoBarrasReparacion")] // EL CODIGO DE BARRAS DE LA REPARCION DEBE SER NUMERICO Y TENER 13 o 14 DÍGITOS
        ErrorCodigoBarrasReparacion = 233208,
        [StringValue("ErrorCodigoVisitaNoCoincideUltimaVisitaContrato")] // EL CODIGO DE LA VISITA NO COINCIDE CON LA ULTIMA VISITA DEL CONTRATO, NO SE PERMITE LA MODIFICACION
        ErrorCodigoVisitaNoCoincideUltimaVisitaContrato = 233209,
        [StringValue("ErrorCodigoVisitaYaCerrada")] // VISITA ACTUALMENTE YA CERRADA
        ErrorCodigoVisitaYaCerrada = 233210,
        [StringValue("ErrorTecnicoIncorrecto")] // TECNICO INCORRECTO O NO APORTADO PARA EL ESTADO DE LA VISITA
        ErrorTecnicoIncorrecto = 233211,
        [StringValue("ErrorTipoVisitaIncorrecto")] // TIPO DE VISITA INCORRECTO O NO APORTADO PARA EL ESTADO DE LA VISITA
        ErrorTipoVisitaIncorrecto = 233212,
        [StringValue("ErrorFechaVisitaSuperaFechaLimite")] // NO SE PERMITE EL CAMBIO AL HABERSE SUPERADO LA FECHA LÍMITE DE LA VISITA
        ErrorFechaVisitaSuperaFechaLimite = 233213,
        [StringValue("ErrorFaltanDatosObligatoriosCierreVisita")] //FALTAN DATOS OLBIGATORIOS PARA CERRADA O CERRADA PENDIENTE DE REPARACION (COMPRUEBE RECPCIONCOMPROBANTE,FACTURADOPROVEEDOR,FECHAFACTURA,CODIGOBARRAS Y TIPOCALDERA
        ErrorFaltanDatosObligatoriosCierreVisita = 233214,
        [StringValue("ErrorFaltanDatosObligatoriosAusenteSegundaVez")] //FALTAN DATOS OBLIGATORIOS PARA AUSENTE POR SEGUNDA VEZ (COMPRUEBE FECHAVISITA,RECEPCIONCOMPROBANTE,FECHAFACTURA Y CODIGOBARRAS
        ErrorFaltanDatosObligatoriosAusenteSegundaVez = 233215,
        [StringValue("ErrorReparacionNoPermitidaFechaSuperada")] ////No dejamos meter reparación si han pasado mas de 3 dias desde el cambio a cerrada
        ErrorReparacionNoPermitidaFechaSuperada = 233216,
        [StringValue("ErrorSuperaLimiteDiasPendienteRealizarReparacion")] // HAN PASADO MAS DE 60 DIAS DESDE EL CAMBIO A CERRADA PENDIENTE DE REALIZAR REPARACION
        ErrorSuperaLimiteDiasPendienteRealizarReparacion = 233217,
        [StringValue("ErrorEstadoIncorrectoEnCerradaPendienteRealizarReparacion")] // EN ESTE CASO NO SE PERMITE OTRO ESTADO QUE NO SEA CERRADA (CerradaPendienteRealizarReparacion)
        ErrorEstadoIncorrectoEnCerradaPendienteRealizarReparacion = 233218,
        [StringValue("ErrorSuperaLimiteDiasCanceladaSistema")] // HAN PASADO MAS DE 12 DIAS DESDE EL CAMBIO A CANCELADA POR SISTEMA
        ErrorSuperaLimiteDiasCanceladaSistema = 233219,
        [StringValue("ErrorEstadoIncorrectoEnCerradaCanceladaPorSistema")] //EN ESTE CASO NO SE PERMITE OTRO ESTADO QUE NO SEA CERRADA (SistemaCanceladaPorSistema)
        ErrorEstadoIncorrectoEnCerradaCanceladaPorSistema = 233220,
        [StringValue("ErrorDatosVisitaIncorrectos")] //DATOS VISITA INCORRECTOS
        ErrorDatosVisitaIncorrectos = 233221,
        [StringValue("ErrorDatosCalderaIncorrectos")] //DATOS CALDERA INCORRECTOS
        ErrorDatosCalderaIncorrectos = 233222,
        [StringValue("ErrorDatosReparacionIncorrectos")] //Error en los datos de la reparacion
        ErrorDatosReparacionIncorrectos = 233223,


        [StringValue("ErrorDatosEntradaCierreVisita")]
        ErrorDatosEntradaCierreVisita = 233299,

        [StringValue("ErrorFicheroAdjunto")] //Error en el fichero adjuntado
        ErrorFicheroAdjunto = 233224,
        [StringValue("ErrorNombreFicheroAdjunto")] //Error en el nombre del fichero adjuntado
        ErrorNombreFicheroAdjunto = 233225,
        [StringValue("ErrorInsertFicheroAdjunto")] //Error en la inserción del registro en la BB.DD.
        ErrorInsertFicheroAdjunto = 233226,
        [StringValue("ErrorEstadoVisitaConFicheroAdjunto")] //Error en la copia del fichero al llegar el estado otro que no sea cerrada o cerrada pendiente reparación.
        ErrorEstadoVisitaConFicheroAdjunto = 233227,
        [StringValue("ErrorFicheroYaExistente")] //Error de que el fichero ya existe.
        ErrorFicheroYaExistente = 233228,
        [StringValue("ErrorLongitudFichero")] //Error en la longitud del fichero.
        ErrorLongitudFichero = 233229 ,
        [StringValue("ErrorDatoVisitaCanceladaPorSistema")] //Error en los datos de la visita a cerrar por sistema.
        ErrorDatoVisitaCanceladaPorSistema = 233230,
        [StringValue("ErrorFaltaMovimientoEnHistoricoDeCanceladaPorSistema")] //Error falta movimiento en el historico de la visita para cancelada por sistema.
        ErrorFaltaMovimientoEnHistoricoDeCanceladaPorSistema = 233231,
        [StringValue("ErroralActualizarLosTelefonosDeContacto")] //Error falta movimiento en el historico de la visita para cancelada por sistema.
        ErroralActualizarLosTelefonosDeContacto = 233232,

        [StringValue("ErrorDatosEquipamiento")] //Error en el equipamiento.
        ErrorDatosEquipamiento = 233233,

        [StringValue("ErrorDatosFechaPrevistaVisitaIncorrectos")] //DATOS FECHA PREVISTA VISITA INCORRECTOS
        ErrorDatosFechaPrevistaVisitaIncorrectos = 233234,

        //Ticket de combustion
        [StringValue("ErrorFaltanDatosObligatoriosTicketCombustion")] //FALTAN DATOS OLBIGATORIOS del ticket de combustion
        ErrorFaltanDatosObligatoriosTicketCombustion = 233235,

        [StringValue("ErrorNoCumpleNumeroElementosTicketCombustion")] //En request han mandado mas de 3 tickets de combustion
        ErrorNoCumpleNumeroElementosTicketCombustion = 233236,

        [StringValue("ErrorCodigoTipoEquipoNoCorrecto")] //El tipo de equipo no es correcto
        ErrorCodigoTipoEquipoNoCorrecto = 233237,

        [StringValue("ErrorGuardadoTicketCombustion")] //
        ErrorGuardadoTicketCombustion = 233238,

        [StringValue("VISITA EN ESTADO ERRONEA. SOLICITUD AVERIA PDTE. DE CERRAR")] //ErrorEstadoActualVisitaErroneaPorAnomaliaCritica SOLICITUD
        ErrorEstadoActualVisitaErroneaPorAnomaliaCritica = 233239,

        //20220218 BGN ADD BEG R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
        [StringValue("ErrorFormatoTelefonoContactoConPrefijo")]
        ErrorFormatoTelefonoContactoConPrefijo = 233240,
        //20220218 BGN ADD END R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
    }
    // Errores que empiezan por 433200
    public enum AperturaSolicitudWSError : int
    {
        [StringValue("ErrorCodigoSubtipoSolicitud")]  //
        ErrorCodigoSubtipoSolicitud = 433201,
        [StringValue("ErrorFaltanDatosObligatoriosAperturaSolicitud")] //FALTAN DATOS OLBIGATORIOS PARA ABRIR LA SOLICITUD
        ErrorFaltanDatosObligatoriosAperturaSolicitud = 433214,
        [StringValue("ErrorDatosErroneosHorario")]
        ErrorDatosErroneosHorario = 433203,
        [StringValue("ErrorDatosErroneosHorario")]  //
        ErrorDatosVacios = 433202,
        [StringValue("ErrorTipoAveria")]  //
        ErrorTipoAveria = 433204,
        [StringValue("ErrorTipovisita")]  //
        ErrorTipovisita = 433204,
        [StringValue("ErrorDatosCalderaIncorrectos")] //DATOS CALDERA INCORRECTOS
        ErrorDatosCalderaIncorrectos = 433222,
        [StringValue("ErrorAltaSolicitud")] //ERROR AL DAR DE ALTA LA SOLICITUD
        ErrorAltaSolicitud = 433223,
        [StringValue("ErrorSolicitudYaAbierta")] //ERROR DE SOLICITUD YA ABIERTA
        ErrorSolicitudYaAbierta = 433224,
        [StringValue("ErrorSociedadDiferente")] //ERROR DE SOCIEDAD DIFERENTE
        ErrorSociedadDiferente = 433225,
        [StringValue("EFVNoValidoParaGeneracionDeInspeccion")] //ERROR EFV NO DE INSPECCION
        EFVNoValidoParaGeneracionDeInspeccion = 433226,
        //20220311 BGN ADD BEG R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
        [StringValue("ErrorFormatoTelefonoContactoConPrefijo")]
        ErrorFormatoTelefonoContactoConPrefijo = 433227,
        //20220311 BGN ADD END R#35162 Adaptar la macro a los cambios en los campos de teléfono del cliente
    }
    // Errores que empiezan por 533200
    public enum CierreSolicitudWSError : int
    {
        [StringValue("ErrorCodigoSubtipoSolicitud")]  //
        ErrorCodigoSubtipoSolicitud = 533201,
        [StringValue("ErrorFaltanDatosObligatoriosCierreSolicitud")] //FALTAN DATOS OLBIGATORIOS PARA CERRAR LA SOLICITUD
        ErrorFaltanDatosObligatoriosCierreSolicitud = 533214,
        [StringValue("ErrorDatosVacios")]  //
        ErrorDatosVacios = 533202,
        [StringValue("ErrorTipoAveria")]  //
        ErrorTipoAveria = 533204,
        [StringValue("ErrorTipovisita")]  //
        ErrorTipovisita = 533204,
        [StringValue("ErrorDatosCalderaIncorrectos")] //DATOS CALDERA INCORRECTOS
        ErrorDatosCalderaIncorrectos = 533222,
        [StringValue("ErrorCierreSolicitud")] //ERRO AL CERRAR LA SOLICITUD
        ErrorCierreSolicitud = 533223,
        [StringValue("ErrorEstadoSolicitud")]  //
        ErrorEstadoSolicitud = 533224,
        [StringValue("ErrorObtenerDatosActuales")]  //
        ErrorObtenerDatosActuales = 533225,
        [StringValue("ErrorMotivoCancelacion")]  //
        ErrorMotivoCancelacion = 533226,
        [StringValue("ErrorSinCambioEstado")]  //
        ErrorSinCambioEstado = 533227,
        [StringValue("ErrorNumeroCaracteristicasYValoresCarateristicas")]  //
        ErrorNumeroCaracteristicasYValoresCarateristicas = 533228,
        [StringValue("ErrorFaltanCaracteristicasExigibles")]  //
        ErrorFaltanCaracteristicasExigibles = 533229,
        [StringValue("ErrorLAFECHAINTRODUCIDADEBEDESERMAYOROIGUALALAFECHADECREACIONYNOSUPERIORA90DIAS")]  //
        ErrorLAFECHAINTRODUCIDADEBEDESERMAYOROIGUALALAFECHADECREACIONYNOSUPERIORA90DIAS = 533230,
        [StringValue("ErrorLAFECHAINTRODUCIDADEBEDESERMAYOROIGUALALAFECHADECREACIONYNOSUPERIORALDIADEHOY")]  //
        ErrorLAFECHAINTRODUCIDADEBEDESERMAYOROIGUALALAFECHADECREACIONYNOSUPERIORALDIADEHOY = 533231,
        [StringValue("ErrorFALTAOVIENENDEMASIADASCARACTERISTICASPARAESTEESTADO")]  //
        ErrorFALTAOVIENENDEMASIADASCARACTERISTICASPARAESTEESTADO = 533232,
        [StringValue("ErrorInsertarCaracteristicas")]  //
        ErrorInsertarCaracteristicas = 533233,
        [StringValue("ErrorDatosFichero")] //DATOS FICHERO INCORRECTOS
        ErrorDatosFichero = 533234,
        [StringValue("ErrorEstadosolicitudConFicheroAdjunto")] //Error estado solicitud con fichero exigible
        ErrorEstadosolicitudConFicheroAdjunto = 533235,
        [StringValue("ErrorNombreFicheroAdjunto")] //Error en el nombre del fichero adjuntado
        ErrorNombreFicheroAdjunto = 533236,
        [StringValue("ErrorInsertFicheroAdjunto")] //Error en la inserción del registro en la BB.DD.
        ErrorInsertFicheroAdjunto = 533237,
        [StringValue("ErrorFicheroYaExistente")] //Error de que el fichero ya existe.
        ErrorFicheroYaExistente = 533238,
        [StringValue("ErrorLongitudFichero")] //Error en la longitud del fichero.
        ErrorLongitudFichero = 533239,
        [StringValue("ErrorModoPagoIncorrecto")] //Error en el modo de pago seleccionado.
        ErrorModoPagoIncorrecto = 533240,


        //Ticket de combustion
        [StringValue("ErrorFaltanDatosObligatoriosTicketCombustion")] //FALTAN DATOS OLBIGATORIOS del ticket de combustion
        ErrorFaltanDatosObligatoriosTicketCombustion = 533241,

        [StringValue("ErrorNoCumpleNumeroElementosTicketCombustion")] //En request han mandado mas de 3 tickets de combustion
        ErrorNoCumpleNumeroElementosTicketCombustion = 533242,

        [StringValue("ErrorCodigoTipoEquipoNoCorrecto")] //El tipo de equipo no es correcto
        ErrorCodigoTipoEquipoNoCorrecto = 533243,

        [StringValue("ErrorGuardadoTicketCombustion")] //El tipo de equipo no es correcto
        ErrorGuardadoTicketCombustion = 533244,

        [StringValue("ErrorEstadosolicitudSinFicheroAdjunto")] //Error estado solicitud sin fichero 
        ErrorEstadosolicitudSinFicheroAdjunto = 533245,
        [StringValue("ErrorModificarSolicitud")] //Error al modificar la solicitud
        ErrorModificarSolicitud = 533246
    }

    public enum TicketCombustionError : int
    {
        /// <summary>
        /// Errores relacionados con el ticket de combustion
        /// </summary>
        //Ticket de combustion
        [StringValue("TICKET COMBUSTION OBLIGATORIO EN ESTE ESTADO")] //FALTAN DATOS OLBIGATORIOS del ticket de combustion
        TicketCombustionObligatorioEnEsteEstado = 100,
        [StringValue("TicketCombustion")]
        TicketCombustion_NoInformado = 101,
        [StringValue("TIPO EQUIPO NO INFORMADO")]
        TipoEquipo_NoInformado = 102,
        [StringValue("TEMPERATURA PDC NO INFORMADO")]
        TemperaturaPDC_NoInformado = 103,
        [StringValue("CO CORREGIDO NO INFORMADO")]
        COCorregido_NoInformado = 104,
        [StringValue("TIRO NO INFORMADO")]
        Tiro_NoInformado = 105,
        [StringValue("CO AMBIENTE NO INFORMADO")]
        COAmbiente_NoInformado = 106,
        [StringValue("O2 NO INFORMADO")]
        O2_NoInformado = 107,
        [StringValue("CO NO INFORMADO")]
        CO_NoInformado = 108,
        [StringValue("CO2 NO INFORMADO")]
        CO2_NoInformado = 109,
        [StringValue("LAMBDA NO INFORMADO")]
        Lambda_NoInformado = 110,
        [StringValue("RENDIMIENTO NO INFORMADO")]
        Rendimiento_NoInformado = 111,

        [StringValue("IMAGEN CONDUCTO HUMOS NO INCLUIDA")]
        NombreFicheroConductoHumos_NoInformado = 112,
        [StringValue("IMAGEN CONDUCTO HUMOS NO INCLUIDA")]
        FicheroConductoHumos_NoInformado = 113,
        [StringValue("FORMATO DE IMAGEN CONDUCTO HUMOS INCORRECTO")]
        NombreFicheroConductoHumos_FormatoIncorrecto = 114,
        [StringValue("IMAGEN CONDUCTO HUMOS YA EXISTE")]
        NombreFicheroConductoHumos_YaExisteEnBBDD = 115,
        [StringValue("IMAGEN CONDUCTO HUMOS ERROR LONGITUD FICHERO")]
        NombreFicheroConductoHumos_ErrorLongitudFichero = 116,

        [StringValue("IMAGEN TICKET DE COMBUSTION NO INCLUIDA")]
        NombreFichero_NoInformado = 117,
        [StringValue("IMAGEN TICKET DE COMBUSTION NO INCLUIDA")]
        ContenidoFichero_NoInformado = 118,
        [StringValue("FORMATO DE IMAGEN TICKET DE COMBUSTION INCORRECTO")]
        NombreFichero_FormatoIncorrecto = 119,
        [StringValue("IMAGEN TICKET DE COMBUSTION YA EXISTE")]
        NombreFichero_YaExisteEnBBDD = 120,
        [StringValue("IMAGEN TICKET DE COMBUSTION ERROR LONGITUD FICHERO")]
        NombreFichero_ErrorLongitudFichero = 121,        


        [StringValue("NECESARIO RELLENAR COMENTARIOS")]
        Comentarios_NoInformado = 122,
        [StringValue("TEMPERATURA MAX ACS NO INFORMADO")]
        TemperaturaMaxACS_NoInformado = 123,
        [StringValue("CAUDAL ACS NO INFORMADO")]
        CaudalACS_NoInformado = 124,
        [StringValue("POTENCIA UTIL NO INFORMADO")]
        PotenciaUtil_NoInformado = 125,
        [StringValue("TEMPERATURA ENTRADA ACS NO INFORMADO")]
        TemperaturaEntradaACS_NoInformado = 126,
        [StringValue("TEMPERATURA SALIDA ACS NO INFORMADO")]
        TemperaturaSalidaACS_NoInformado = 127,

        [StringValue("IMAGEN TICKET DE COMBUSTION_TIPO DOCUMENTO INCORRECTO")]
        NombreFichero_TipoDocumentoIncorrecto = 128,
        [StringValue("IMAGEN CONDUCTO HUMOS_TIPO DOCUMENTO INCORRECTO")]
        NombreFicheroConductoHumos_TipoDocumentoIncorrecto = 129,

        //[StringValue("TIPO EQUIPAMIENTO OBLIGATORIO SI VISITA EN OPERA ES COCINA")]
        [StringValue("TIPO EQUIPAMIENTO OBLIGATORIO")]
        TipoEquipamientoObligatorio = 130,

        [StringValue("TIPO DE EQUIPO ERRÓNEO")]
        TipoEquipo_FueraRangos = 201,
        [StringValue("TEMPERATURA PDC FUERA DE RANGO")]
        TemperaturaPDC_FueraRangos = 202,
        [StringValue("CO CORREGIDO ALTO / ANOMALÍA SECUNDARIA")]
        COCorregido_FueraRangos_Anomalia_Secundaria = 203,
        [StringValue("CO CORREGIDO EXCESIVO / DEFECTO PRINCIPAL")]
        COCorregido_FueraRangos_Defecto_Principal = 204,
        [StringValue("TIRO POSITIVO INCORRECTO")]
        Tiro_FueraRangos = 205,
        [StringValue("CO AMBIENTE ALTO / ANOMALÍA SECUNDARIA")]
        COAmbiente_FueraRangos_Anomalia_Secundaria = 206,
        [StringValue("CO AMBIENTE EXCESIVO / DEFECTO PRINCIPAL")]
        COAmbiente_FueraRangos_Defecto_Principal = 207,
        [StringValue("O2 FUERA DE RANGO")]
        O2_FueraRangos = 208,
        [StringValue("O2 FUERA DE RANGO NO JUSTIFICADOS")]
        O2_FueraRangos_No_Justificado = 209,
        [StringValue("CO FUERA DE RANGO")]
        CO_FueraRangos = 210,
        [StringValue("CO FUERA DE RANGO NO JUSTIFICADOS")]
        CO_FueraRangos_No_Justificado = 211,
        [StringValue("CO2 FUERA DE RANGO")]
        CO2_FueraRangos = 212,
        [StringValue("CO2 FUERA DE RANGO NO JUSTIFICADOS")]
        CO2_FueraRangos_No_Justificado = 213,
        [StringValue("LAMBDA FUERA DE RANGO")]
        Lambda_FueraRangos = 214,
        [StringValue("LAMBDA FUERA DE RANGO NO JUSTIFICADOS")]
        Lambda_FueraRangos_No_Justificado = 215,
        [StringValue("RENDIMIENTO FUERA DE RANGO")]
        Rendimiento_FueraRangos = 216,
        [StringValue("RENDIMIENTO FUERA DE RANGO NO JUSTIFICADOS")]
        Rendimiento_FueraRangos_No_Justificado = 217,

        //20210624 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
        [StringValue("TEMPERATURA PDC FUERA DE RANGO NO JUSTIFICADOS")]
        TemperaturaPDC_FueraRangos_No_Justificado = 218,

        [StringValue("TIRO POSITIVO INCORRECTO / ANOMALÍA SECUNDARIA")]
        Tiro_FueraRangos_Excepcion_AnomaliaSecundaria = 219,

        [StringValue("CO CORREGIDO ALTO / ANOMALÍA SECUNDARIA")]
        COCorregido_FueraRangos_Excepcion_AnomaliaSecundaria = 220,
        [StringValue("CO AMBIENTE ALTO / ANOMALÍA SECUNDARIA")]
        COAmbiente_FueraRangos_Excepcion_AnomaliaSecundaria = 221,
        [StringValue("CO CORREGIDO EXCESIVO / DEFECTO PRINCIPAL")]
        COCorregido_FueraRangos_Excepcion_DefectoPrincipal = 222,
        [StringValue("CO AMBIENTE EXCESIVO / DEFECTO PRINCIPAL")]
        COAmbiente_FueraRangos_Excepcion_DefectoPrincipal = 223,

        [StringValue("CO CORREGIDO ALTO / ANOMALÍA SECUNDARIA / ID_SOLICITUD AVERIA PROPORCIONADO NO CORRESPONDE CON SOLICITUD AVERIA CREADA PREVIAMENTE")]
        COCorregido_FueraRangos_Excepcion_AnomaliaSecundaria_Id_Solicitud_Proporcionado_No_Correcto = 224,
        [StringValue("CO CORREGIDO EXCESIVO / DEFECTO PRINCIPAL / ID_SOLICITUD AVERIA PROPORCIONADO NO CORRESPONDE CON SOLICITUD AVERIA CREADA PREVIAMENTE")]
        COCorregido_FueraRangos_Excepcion_DefectoPrincipal_Id_Solicitud_Proporcionado_No_Correcto = 225,
        [StringValue("CO AMBIENTE ALTO / ANOMALÍA SECUNDARIA / ID_SOLICITUD AVERIA PROPORCIONADO NO CORRESPONDE CON SOLICITUD AVERIA CREADA PREVIAMENTE")]
        COAmbiente_FueraRangos_Excepcion_AnomaliaSecundaria_Id_Solicitud_Proporcionado_No_Correcto = 226,
        [StringValue("CO AMBIENTE EXCESIVO / DEFECTO PRINCIPAL / ID_SOLICITUD AVERIA PROPORCIONADO NO CORRESPONDE CON SOLICITUD AVERIA CREADA PREVIAMENTE")]
        COAmbiente_FueraRangos_Excepcion_DefectoPrincipal_Id_Solicitud_Proporcionado_No_Correcto = 227,
        [StringValue("TIRO POSITIVO INCORRECTO / ANOMALÍA SECUNDARIA / ID_SOLICITUD AVERIA PROPORCIONADO NO CORRESPONDE CON SOLICITUD AVERIA CREADA PREVIAMENTE")]
        Tiro_FueraRangos_Excepcion_AnomaliaSecundaria_Id_Solicitud_Proporcionado_No_Correcto = 228,

        //20210624 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion
    }

    /// <summary>
    /// Avisos relacionados con el ticket de combustion
    /// </summary>
    public enum TicketCombustionAviso : int
    {
        [StringValue("CO CORREGIDO ALTO / ANOMALÍA SECUNDARIA")]
        COCorregido_FueraRangos_Anomalia_Secundaria = 301,
        [StringValue("CO CORREGIDO EXCESIVO / DEFECTO PRINCIPAL")]
        COCorregido_FueraRangos_Defecto_Principal = 302,
        [StringValue("CO AMBIENTE ALTO / ANOMALÍA SECUNDARIA")]
        COAmbiente_FueraRangos_Anomalia_Secundaria = 303,
        [StringValue("CO AMBIENTE EXCESIVO / DEFECTO PRINCIPAL")]
        COAmbiente_FueraRangos_Defecto_Principal = 304,
        [StringValue("RENDIMIENTO FUERA DE RANGO")]
        Rendimiento_FueraRangos_Generar_Ticket_Devolver_Aviso = 305,

        //20210624 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
        [StringValue("CO CORREGIDO ALTO / ANOMALÍA SECUNDARIA / FUERA RANGO")] //VALOR DE CO CORREGIDO ALTO FUERA DE RANGO
        COCorregido_FueraRangos_Excepcion_AnomaliaSecundaria = 306,
        [StringValue("CO CORREGIDO EXCESIVO / DEFECTO PRINCIPAL")]
        COCorregido_FueraRangos_Excepcion_DefectoPrincipal = 307,
        [StringValue("CO AMBIENTE ALTO / ANOMALÍA SECUNDARIA / FUERA DE RANGO")]
        COAmbiente_FueraRangos_Excepcion_AnomaliaSecundaria = 308,
        [StringValue("CO AMBIENTE EXCESIVO / DEFECTO PRINCIPAL")] //VALOR DE CO CORREGIDO ALTO FUERA DE RANGO
        COAmbiente_FueraRangos_Excepcion_DefectoPrincipal = 309,
        [StringValue("CO CORREGIDO ALTO / ANOMALÍA SECUNDARIA / Avanzar avería a 'Presupuesto pendiente de Cliente'")]
        COCorregido_FueraRangos_Excepcion_AnomaliaSecundaria_Avanzar_Presupuesto_Presentado = 310,
        [StringValue("CO CORREGIDO EXCESIVO / DEFECTO PRINCIPAL / Avanzar avería a 'Presupuesto pendiente de Cliente'")]
        COCorregido_FueraRangos_Excepcion_DefectoPrincipal_Avanzar_Presupuesto_Presentado = 311,
        [StringValue("CO AMBIENTE ALTO / ANOMALÍA SECUNDARIA / Avanzar avería a 'Presupuesto pendiente de Cliente'")]
        COAmbiente_FueraRangos_Excepcion_AnomaliaSecundaria_Avanzar_Presupuesto_Presentado = 312,
        [StringValue("CO AMBIENTE EXCESIVO / DEFECTO PRINCIPAL / Avanzar avería a 'Presupuesto pendiente de Cliente'")]
        COAmbiente_FueraRangos_Excepcion_DefectoPrincipal_Avanzar_Presupuesto_Presentado = 313,

        [StringValue("TIRO POSITIVO ERRONEO / ANOMALÍA SECUNDARIA")]
        Tiro_FueraRangos_Excepcion_AnomaliaSecundaria = 314,
        [StringValue("TIRO POSITIVO ERRONEO / ANOMALÍA SECUNDARIA / Avanzar avería a 'Presupuesto pendiente de Cliente'")]
        Tiro_FueraRangos_Excepcion_AnomaliaSecundaria_Avanzar_Presupuesto_Presentado = 315,

        [StringValue("TEMPERATURA PDC FUERA DE RANGO NO JUSTIFICADOS")]
        TemperaturaPDC_FueraRangos_No_Justificado = 316,
        [StringValue("O2 FUERA DE RANGO NO JUSTIFICADOS")]
        O2_FueraRangos_No_Justificado = 317,
        [StringValue("CO FUERA DE RANGO NO JUSTIFICADOS")]
        CO_FueraRangos_No_Justificado = 318,
        [StringValue("CO2 FUERA DE RANGO NO JUSTIFICADOS")]
        CO2_FueraRangos_No_Justificado = 319,
        [StringValue("LAMBDA FUERA DE RANGO NO JUSTIFICADOS")]
        Lambda_FueraRangos_No_Justificado = 320,
        [StringValue("RENDIMIENTO FUERA DE RANGO")]
        Rendimiento_FueraRangos_No_Justificado_PermitidoCerrar = 321,

        [StringValue("CO CORREGIDO FUERA DE RANGO NO JUSTIFICADO")]
        COCorregido_FueraRangos_No_Justificado = 322,
        [StringValue("CO AMBIENTE FUERA DE RANGO NO JUSTIFICADOS")]
        COAmbiente_FueraRangos_No_Justificado = 323,
        [StringValue("TIRO INCORRECTO NO JUSTIFICADOS")]
        Tiro_FueraRangos_No_Justificado = 324,

        //20210624 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion

        //20211011 BUA ADD R#34195 - Ticket de combustión priorización anomalía principal o secundaria
        [StringValue("ANOMALIA PRINCIPAL")]
        AnomaliaPrincipalPrioritariaDetectada = 325,

        [StringValue("ANOMALIA SECUNDARIA")]
        AnomaliaSecundariaPrioritariaDetectada = 326,
        //20211011 BUA END R#34195 - Ticket de combustión priorización anomalía principal o secundaria

    }

    // Errores que empiezan por 633200
    public enum ContratoVigenteWSError : int
    {
        
        [StringValue("ErrorDatosVacios")]  //
        ErrorDatosVacios = 633201,
        [StringValue("ErrorObtenerDatos")] //ERROR AL OBTENER LOS DATOS DE LA SOLICITUD
        ErrorObtenerDatos = 633202
        
    }


    // Errores que empiezan por 733200
    public enum DatosVisitaWSError : int
    {

        [StringValue("ErrorDatosVaciosVisita")]  //
        ErrorDatosVaciosVisita = 733201,
        [StringValue("ErrorObtenerDatosVisita")] //ERRO AL OBTENER LOS DATOS DE LA VISITA
        ErrorObtenerDatosVisita = 733202

    }

    // Errores que empiezan por 833200
    public enum DatosSolicitudWSError : int
    {

        [StringValue("ErrorDatosVaciosSolicitud")]  //
        ErrorDatosVaciosSolicitud = 833201,
        [StringValue("ErrorObtenerDatosSolicitud")] //ERROR AL OBTENER LOS DATOS DE LA SOLICITUD
        ErrorObtenerDatosSolicitud = 833202,
        [StringValue("ErrorContratoSinSolicitudes")]
        ErrorContratoSinSolicitudes = 833203

    }

    // Errores que empiezan por 233400
    public enum AltaSolicitudWSError : int
    {

    }

    // Errores que empiezan por 933200
    public enum SigecasWSError : int
    {
        [StringValue("ErrorDatosVacios")]  //
        ErrorDatosVacios = 933202,
        [StringValue("ErrorFaltanDatosObligatoriosSigecas")]  //
        ErrorFaltanDatosObligatoriosSigecas = 933214,
        [StringValue("ErrorBorrarUsuario")]  //
        ErrorBorrarUsuario = 933215,
    }

    //20210128 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
    public enum UpdateDocumentSetWSError : int
    {
        [StringValue("ErrorDocumentoNoEncontrado")]
        ErrorDocumentoNoEncontrado = 333201,
        [StringValue("ErrorIdDocumentoNoCoincide")]
        ErrorIdDocumentoNoCoincide = 333202,
        [StringValue("ErrorActualizarEstadoDocumento")]
        ErrorActualizarEstadoDocumento = 333203,
        [StringValue("ErrorEstadoSolicitudIncorrecto")]
        ErrorEstadoSolicitudIncorrecto = 333204,
        [StringValue("ErrorSolicitudDocumentoIncorrecta")]
        ErrorSolicitudDocumentoIncorrecta = 333205
    }
    //20210128 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital
}