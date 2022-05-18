using System;
using Iberdrola.Commons.Utils;

/// <summary>
/// Descripción breve de Enumerados
/// </summary>
public class Enumerados
{
    /// <summary>
    /// Tiene todos los tipos de documentos a subir en el cierre de las visitas
    /// </summary>
    public enum TipoDocumentoAnexarfichero : int
    {
        [StringValue("97")]
        InformesMantenimientoGAS=97,
        [StringValue("106")]
        SMG_Reparaciones=106,
        [StringValue("107")]
        SMG_Averias=107,
        [StringValue("42")]
        InformeRevisionPorPrecinte=42,
        [StringValue("8")]
        ConductoHumos = 8
    };

    /// <summary>
    /// Tiene todos los estados en los que puede estar una visita
    /// El String Value tiene el mismo valor que la tabla en la base de datos
    /// </summary>
    public enum EstadosVisita : int
    {
        [StringValue("01")]
        EnEjecución=1,
        [StringValue("02")]
        Cerrada = 2,
        [StringValue("03")]
        Pendiente=3,
        [StringValue("04")]
        Aplazada=4,
        [StringValue("05")]
        CanceladaQuiereDarseDeBaja=5,
        [StringValue("06")]
        CanceladaClienteNoLocalizable=6,
        [StringValue("07")]
        CanceladaClienteNoDeseaVisita=7,
        [StringValue("08")]
        CanceladaAusentePorSegundaVez=8,
        [StringValue("09")]
        CerradaPendienteRealizarReparacion=9,
        [StringValue("10")]
        SistemaCanceladaPorSistema=10,
        [StringValue("11")]
        CerradaPuestaEnMarcha=11,
        [StringValue("13")]
        visitaErronea = 13
    };


    /// <summary>
    /// Tiene todos los estados en los que puede estar una visita
    /// El String Value tiene el mismo valor que la tabla en la base de datos
    /// </summary>
    public enum SubtipoSolicitud : int
    {
        [StringValue("001")] AveriaMantenimientodeGas=1,
        [StringValue("002")] SolicitudDeVisitaSMG=2,
        [StringValue("003")] VisitaIncorrecta=3,
        [StringValue("004")] AveriaIncorrecta=4,
        [StringValue("005")] RevisionPorPrecinte=5,
        [StringValue("006")] VisitaSupervision=6,
        [StringValue("007")] AceptacionDePresupuesto=7,
        [StringValue("008")] GasConfort=8,
        [StringValue("009")] InformacionPorReclamacion=9,
        [StringValue("010")] SolicitudDeAveriaNoResueltaTrasLlamadaDeCortesia=10,
        [StringValue("011")] InstalacionTermostatoInteligente=11,
        [StringValue("012")] AveriaGasConfort=12,
        [StringValue("013")] AveriaIncorrectaGasConfort=13,
        [StringValue("014")] SolicitudDeVisitaGasConfort=14,
        [StringValue("015")] VisitaIncorrectaGasConfort=15,
        [StringValue("016")] SolicitudInspeccionGas=16,
        [StringValue("018")] AveriaSupervision=18
    };


    /// <summary>
    /// Tiene todos los estados en los que puede estar una visita
    /// El String Value tiene el mismo valor que la tabla en la base de datos
    /// </summary>
    public enum EstadosSolicitudes : int
    {
        [StringValue("001")]
        Pendientedecontactar=1,
        [StringValue("002")]
        Pendientedeconcretarcita=2,
        [StringValue("004")]
        Pendientedeacudiracliente=4,
        [StringValue("005")]
        PresupuestopendientedeCliente=5,
        [StringValue("006")]
        Pendientederepararsincitaconcertada=6,
        [StringValue("007")]
        Pendientederepararconcitaconcertada=7,
        [StringValue("010")]
        Reparada=10,
        [StringValue("011")]
        Reparadaportelefono=11,
        [StringValue("012")]
        Cancelada=12,
        [StringValue("014")]
        VisitaRealizada=14,
        [StringValue("016")]
        Pdtdeacudirporaplazoausenciadelcliente=16,
        [StringValue("017")]
        Pendienteconcretarcitacliente=17,
        [StringValue("018")]
        BajaServicio=18,
        [StringValue("019")]
        PdtdeacudirporaplazoausenciadelclienteVisitaIncorrecta=19,
        [StringValue("020")]
        Cerradassinaceptarpresupuesto=20,
        [StringValue("021")]
        ReparadaconDocumentacion=21,
        [StringValue("022")]
        CanceladaporReasignacion=22,
        [StringValue("023")]
        CanceladotransferidoaSAT=23,
        [StringValue("024")]
        TransferidoaSAT=24,
        [StringValue("025")]
        CanceladaSupervision=25,
        [StringValue("026")]
        Visitarealizada=26,
        [StringValue("027")]
        ProveedortransferidoaSAT=27,
        [StringValue("028")]
        Canceladapresupuestonovalido=28,
        [StringValue("029")]
        ProveedorTransferidoaSATfinalizada=29,
        [StringValue("030")]
        Trabajoinvalido=30,
        [StringValue("031")]
        PTEPieza=31,
        [StringValue("032")]
        SolucionadoProveedortransferidoaSAT=32,
        [StringValue("033")]
        ClienteVisitado=33,
        [StringValue("034")]
        PdteScoring=34,
        [StringValue("035")]
        PdteOferta=35,
        [StringValue("036")]
        Ofertarechazada=36,
        [StringValue("037")]
        FinanciaciónrechazadaporIB=37,
        [StringValue("038")]
        OfertaaceptadaPdteinstalacion=38,
        [StringValue("039")]
        Pdtematerial=39,
        [StringValue("040")]
        PdteSAT=40,
        [StringValue("041")]
        Pdteaceptaciónfintrabajos=41,
        [StringValue("042")]
        Incidenciaeninstalación=42,
        [StringValue("043")]
        Conformefinalcliente=43,
        [StringValue("044")]
        Ofertapresentadapdtedecliente=44,
        [StringValue("045")]
        OfertaGasConfort=45,
        [StringValue("046")]
        PdteRespuesta=46,
        [StringValue("047")]
        Cerrada=47,
        [StringValue("048")]
        Pdtecierre=48,
        [StringValue("049")]
        Malcerradaaveríasinreparar=49,
        [StringValue("050")]
        Malcerradanoaceptópresupuesto=50,
        [StringValue("051")]
        Malcerradaalcontactarfuncionabacorrectamente=51,
        [StringValue("052")]
        Persisteavería=52,
        [StringValue("053")]
        Reparadaconindicacionesalcliente=53,
        [StringValue("054")]
        Conformeconreparación=54,
        [StringValue("055")]
        Clienteilocalizable=55,
        [StringValue("056")]
        CanceladaSolicitud=56,
        [StringValue("057")]
        InstalaciónRealizada=57,
        [StringValue("058")]
        PdteConformidadCliente=58,
        [StringValue("059")]
        OfertaaceptadaPdtecontrato=59,
        [StringValue("060")]
        OfertaAceptada=60,
        [StringValue("061")]
        OfertaAceptadaPdteDepago=61,
        [StringValue("062")]
        JustificanteRecibido=62,
        [StringValue("063")]
        InstalaciónAutorizada=63,
        [StringValue("064")]
        ImportePagadoIncorrecto=64,
        [StringValue("065")]
        Pendiente=65,
        [StringValue("066")]
        PresupuestoAceptado = 66,
        [StringValue("067")]
        PdteActudirCliente = 67,
        [StringValue("068")]
        RechazoPresupuestoTemporal = 68,
        [StringValue("069")]
        InspeccionPendiente= 69,
        [StringValue("070")]	
        Inspeccionpdtedereparacioncondefectocritico= 70,
        [StringValue("071")]	
        InspeccionPdteReparacionConDftoNOcroiico= 71,
        [StringValue("072")]	
        InspeccionEjecutada= 72,
        [StringValue("073")]	
        InspeccionEjecutadaDonDefecto= 73,
        [StringValue("074")]	
        InspeccionCancelada= 74,
        [StringValue("075")]	
        CerradoporCambioDeCaldera= 75,
        [StringValue("076")]	
        BajaPorRenovacionCaldera= 76,
        [StringValue("078")]	
        ConfirmaFechayPeriodo= 78,
        [StringValue("079")]	
        NOConfirmadoFechayPeriodo= 79,
        //20210120 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        [StringValue("080")]
        OfertaPresentadaPdteFirma = 80,
        [StringValue("081")]
        OfertaRechazadaPdteConfirmacion = 81,
        //20210120 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        [StringValue("082")]
        visitaErronea = 82,
        [StringValue("999")]	
        GCInstaladodeBaja= 999
    };

    public enum EstadosGestion : int
    {
        Visualizando = 0, // Es están mostrando los datos pero no hay registro seleccionado.
        Seleccionando = 1, // Es están mostrando los datos y SÍ hay registro seleccionado.
        Modificando = 2,  // Se está editando un registro.
        Aniadiendo = 3, // Se están editando los datos de un nuevo registro.
        Eliminando = 4  // Se están visualizando los datos de un registro para que se elimine.
    }

    public static EstadosGestion ObtenerEstadoGestion(string strEstadoGestion)
    {
        Int32 intEstado = Int32.Parse(strEstadoGestion);
        switch (intEstado)
        {
            case 0:
                return EstadosGestion.Visualizando;
            case 1:
                return EstadosGestion.Seleccionando;
            case 2:
                return EstadosGestion.Modificando;
            case 3:
                return EstadosGestion.Aniadiendo;
            case 4:
                return EstadosGestion.Eliminando;
            default:
                return EstadosGestion.Visualizando;
        }
    }

    public enum AccionesGestion : int
    {
        Seleccionar,
        Aniadir,
        Eliminar,
        Modificar,
        Aceptar,
        Cancelar
    }

    public enum PermisosFacturacionVisitas :int
    {
       Visitas = 1,
       Reparacion = 2,
       Penalizacion = 3
    }

    public enum TipoVistaContratoCompleto
    {
        SinFiltrar,
        FechaUltimaVisita,
        Facturadas,
        EstadoVisita,
        PorLote,
        NoCerradasUrgencia,
        EjecucionOAplazadas,
        MantenimientoGasCalefaccion,
        MantenimientoGas,
        PorContrato
    }

    //2094_Pago Anticipado
    public enum TipoLugarAveria : int
    {
        Equipo = 1,
        Instalacion = 2,
        NA=3
    }

    public enum TipoRazonModificacionsolicitud : int
    {
        Modificar = 1,
        Eliminar = 2
    }

    public enum ParametrosContratoCompleto
    {
        fechaDesde,
        fechaHasta,
        CodigoVisita,
        CodigoLote,
        CodigoUrgencia,
        Contrato
    }

    /// <summary>
    /// Enumerado para identificar las entradas de configuración (tabla P_CONFIG).
    /// </summary>
    public enum Configuracion : int
    {
        /// <summary>
        /// Ruta del certificado para las llamadas desde Delta
        /// </summary>
        [StringValue("IT_PATH_CERTIFICADO_DELTA")]
        IT_PATH_CERTIFICADO_DELTA,
        [StringValue("HABILITAR_GENERACION_PASSWORDS")]
        HABILITAR_GENERACION_PASSWORDS = 1,
        [StringValue("PASSWORDS_ENCRIPTADAS")]
        PASSWORDS_ENCRIPTADAS = 2,
        [StringValue("RUTA_DESTINO_DOCUMENTOS_VISITAS")]
        RUTA_DESTINO_DOCUMENTOS_VISITAS = 3,
        [StringValue("RUTA_DESTINO_DOCUMENTOS")]
        RUTA_DESTINO_DOCUMENTOS = 4,
        [StringValue("RUTA_DESTINO_DOCUMENTOS_GESDOCOR")]
        RUTA_DESTINO_DOCUMENTOS_GESDOCOR = 5,
        [StringValue("RUTA_DESTINO_DOCUMENTOS_ENVIADOS")]
        RUTA_DESTINO_DOCUMENTOS_ENVIADOS = 6,
        [StringValue("EXTENSIONES_DOCUMENTOS")]
        EXTENSIONES_DOCUMENTOS = 6,
        /// <summary>
        /// Indica cuales son los usuarios que son Iberdrola.
        /// </summary>
        [StringValue ("CONF_USUARIOS_IBERDROLA")]
        CONF_USUARIOS_IBERDROLA,
        /// <summary>
        /// Indica cuales son los usuarios que no se deben validar contra LDAP
        /// </summary>
        [StringValue("CONF_USUARIOS_EXCEPCION")]
        CONF_USUARIOS_EXCEPCION,
        /// <summary>
        /// Indica si se muestra el mensaje en la pantalla de login.
        /// </summary>
        [StringValue("MOSTRAR_AVISO_EN_LOGIN")]
        MOSTRAR_AVISO_EN_LOGIN,
        /// <summary>
        /// Indica el contenido del aviso de la pantalla de login.
        /// </summary>
        [StringValue("CONTENIDO_AVISO_EN_LOGIN")]
        CONTENIDO_AVISO_EN_LOGIN,
        /// <summary>
        /// Indica si el lofeo es contra el LDAP
        /// </summary>
        [StringValue("LDAP_COMPROBACION_HABILITADA")]
        LDAP_COMPROBACION_HABILITADA,
        /// <summary>
        /// Indica si se comprueban las caracteristicas generales del usuario.
        /// </summary>
        [StringValue("LDAP_COMPROBACION_CONDICIONES_HABILITADA")]
        LDAP_COMPROBACION_CONDICIONES_HABILITADA,
        /// <summary>
        /// BASE DN para la conexión con LDAP.
        /// </summary>
        [StringValue("LDAP_CONFIGURATION_BASE_DN")]
        LDAP_CONFIGURATION_BASE_DN,
        /// <summary>
        /// USER DN para la búsqueda del usuario.
        /// </summary>
        [StringValue("LDAP_CONFIGURATION_USER_DN")]
        LDAP_CONFIGURATION_USER_DN,
        /// <summary>
        /// USER DN para la búsqueda de cuentas de servicio.
        /// </summary>
        [StringValue("LDAP_CONFIGURATION_USER_DN_CUENTAS_SERVICIO")]
        LDAP_CONFIGURATION_USER_DN_CUENTAS_SERVICIO,
        /// <summary>
        /// Servidor LDAP.
        /// </summary>
        [StringValue("LDAP_CONFIGURATION_SERVER")]
        LDAP_CONFIGURATION_SERVER,
        /// <summary>
        /// Puerto del LDAP.
        /// </summary>
        [StringValue("LDAP_CONFIGURATION_PORT")]
        LDAP_CONFIGURATION_PORT,
        /// <summary>
        /// Url de cosulta de las condiciones Generales del usuario.
        /// </summary>
        [StringValue("ARQ_COND_NUEVAS_URI")]
        ARQ_COND_NUEVAS_URI,
        /// <summary>
        /// Número de intentos máximo que se permite introducir la contraseña erroneamente.
        /// </summary>
        [StringValue("CONF_NUM_INTENTOS_PASSWORD")]
        CONF_NUM_INTENTOS_PASSWORD,
        /// <summary>
        /// Indica cuales son los usuarios que tiene que saltarse la validación de firma de política.
        /// </summary>
        [StringValue("USUARIOS_SALTAR_FIRMA_POLITICA")]
        USUARIOS_SALTAR_FIRMA_POLITICA,

        [StringValue("ARQ_COND_ACEPTADAS_URI")]
        ARQ_COND_ACEPTADAS_URI,

        [StringValue("ARQ_COND_WS_AUT_HEADER")]
        ARQ_COND_WS_AUT_HEADER,

        [StringValue("ARQ_COND_WS_IDIOMA")]
        ARQ_COND_WS_IDIOMA,

        [StringValue("ARQ_COND_WS_LOGIN")]
        ARQ_COND_WS_LOGIN,

        [StringValue("ARQ_COND_WS_NEGOCIO")]
        ARQ_COND_WS_NEGOCIO,

        [StringValue("ARQ_COND_WS_PASS")]
        ARQ_COND_WS_PASS,

        [StringValue("ARQ_COND_WS_CODIGOS_ERROR_OBTENER_CONDICIONES")]
        ARQ_COND_WS_CODIGOS_ERROR_OBTENER_CONDICIONES,

        [StringValue("CONF_SERIAL_CERT_ENCRYPT")]
        CONF_SERIAL_CERT_ENCRYPT,

        [StringValue("CONF_SERIAL_CERT_SIGN")]
        CONF_SERIAL_CERT_SIGN,

        [StringValue("MAIL_INICIO_ASUNTO")]
        MAIL_INICIO_ASUNTO,

        [StringValue("CUENTA_CORREO_ADMINISTRADOR_SERVICIO")]
        CUENTA_CORREO_ADMINISTRADOR_SERVICIO,
        
        [StringValue("WS_METHOD_REQUEST")]
        WS_METHOD_REQUEST,
        [StringValue("WS_CONTENT_TYPE_REQUEST")]
        WS_CONTENT_TYPE_REQUEST,
        [StringValue("WS_ACCEPT_REQUEST")]
        WS_ACCEPT_REQUEST,
        [StringValue("WS_AUT_HEADER")]
        WS_AUT_HEADER,

        [StringValue("WS_PROXY_URL")]
        WS_PROXY_URL,
        [StringValue("WS_PROXY_USER")]
        WS_PROXY_USER,
        [StringValue("WS_PROXY_PASS")]
        WS_PROXY_PASS,
        /// <summary>
        /// Indica si hay que mostrar la página de obras
        /// </summary>
        [StringValue("MOSTRAR_PAGINA_OBRAS")]
        MOSTRAR_PAGINA_OBRAS,
        /// <summary>
        /// Indica la url de la página de obras a mostrar
        /// </summary>
        [StringValue("URL_PAGINA_OBRAS")]
        URL_PAGINA_OBRAS,
        /// <summary>
        /// Indica la url de la página de obras a mostrar
        /// </summary>
        [StringValue("HABILITAR_WS")]
        HABILITAR_WS,

        /// <summary>
        /// Activa/Desactiva el web service
        /// </summary>
        [StringValue("HABILITAR_APERTURASOLICITUD_WS")]
        HABILITAR_APERTURASOLICITUD_WS,

        /// <summary>
        /// Activa/Desactiva el web service
        /// </summary>
        [StringValue("HABILITAR_CIERRESOLICITUD_WS")]
        HABILITAR_CIERRESOLICITUD_WS,

        /// <summary>
        /// Activa/Desactiva el web service
        /// </summary>
        [StringValue("HABILITAR_CIERREVISITA_WS")]
        HABILITAR_CIERREVISITA_WS,

        /// <summary>
        /// Activa/Desactiva el web service
        /// </summary>
        [StringValue("HABILITAR_CONTRATOVIGENTE_WS")]
        HABILITAR_CONTRATOVIGENTE_WS,

        /// <summary>
        /// Activa/Desactiva el web service
        /// </summary>
        [StringValue("HABILITAR_DATOSSOLICITUD_WS")]
        HABILITAR_DATOSSOLICITUD_WS,

        /// <summary>
        /// Activa/Desactiva el web service
        /// </summary>
        [StringValue("HABILITAR_DATOSVISITA_WS")]
        HABILITAR_DATOSVISITA_WS,

        /// <summary>
        /// Activa/Desactiva el web service
        /// </summary>
        [StringValue("HABILITAR_ESCUCHAFIRMAREMOTA_WS")]
        HABILITAR_ESCUCHAFIRMAREMOTA_WS,

        /// <summary>
        /// Activa/Desactiva el web service
        /// </summary>
        [StringValue("HABILITAR_SIGECAS_WS")]
        HABILITAR_SIGECAS_WS,

        /// <summary>
        /// Indica si es obligatorio meter el fichero del contrato de GC
        /// </summary>
        [StringValue("OBLIGATORIO_FICHERO_GC")]
        OBLIGATORIO_FICHERO_GC,
        /// <summary>
        /// Indica si esta activo el RepairAndCare
        /// </summary>
        [StringValue("ACTIVO_REPAIR_AND_CARE")]
        ACTIVOREPARIANDCARE,
        /// <summary>
        /// Indica si esta activo el alta GC Ficticio.
        /// </summary>
        [StringValue("ACTIVO_ALTA_GC_FICTICIO")]
        ACTIVO_ALTA_GC_FICTICIO,
        /// <summary>
        /// Indica si esta activo el TLS12
        /// </summary>
        [StringValue("ACTIVO_TLS12")]
        ACTIVOTLS12,
        /// <summary>
        /// Indica si esta activo la llamada al Ws del proveedor para solicitudes de GC Ficticio.
        /// </summary>
        [StringValue("ActivoLlamadaWSProveedorAltaGC")]
        ActivoLlamadaWSProveedorAltaGC,

        
        /// <summary>
        /// Indica si s emuestra o no el file para cargar el fichero de GC.
        /// </summary>
        [StringValue("MOSTRAR_FICHERO_GC_A_GUARDAR")]
        MOSTRAR_FICHERO_GC_A_GUARDAR,
        /// <summary>
        /// Indica los ficheros en los que mostrar el file para subir el fichero de GC.
        /// </summary>
        [StringValue("ESTADOS_FICHERO_GC")]
        ESTADOS_FICHERO_GC,
        //20200120 BGN ADD BEG [R#21764]: Habilitar/Deshabilitar el acceso por Delta a SMG
        /// <summary>
        /// Indica si se realiza la comprobacion del certificado para las llamdas desde Delta
        /// </summary>
        [StringValue("HABILITAR_CERTIFICADO_DELTA")]
        HABILITAR_CERTIFICADO_DELTA,
        //20200120 BGN ADD END [R#21764]: Habilitar/Deshabilitar el acceso por Delta a SMG
        //20200123 BGN ADD BEG [R#21821]: Parametrizar envio mails por la web
        [StringValue("CUENTA_CORREO_GC_PAGO_ADELANTADO")]
        CUENTA_CORREO_GC_PAGO_ADELANTADO,
        [StringValue("CUENTA_CORREO_ERRORES_FROM")]
        CUENTA_CORREO_ERRORES_FROM,
        //20200123 BGN ADD END [R#21821]: Parametrizar envio mails por la web
        [StringValue("NUMERO_VISITAS_CIERRE_DIA")]
        NUMERO_VISITAS_CIERRE_DIA,
        // 20052020 Kintell
        [StringValue("IMPERSONATOR_ACTIVO")]
        IMPERSONATOR_ACTIVO,
        [StringValue("RUTA_CARTASENVIADAS")]
        RUTA_CARTASENVIADAS,
        //20200520 BGN ADD BEG R#21754: Colgar facturas en información de reclamación
        [StringValue("RUTA_FICHEROS_RECLAMACION")]
        RUTA_FICHEROS_RECLAMACION,
        [StringValue("ID_TIPO_DOCUMENTO_RECLAMACION")]
        ID_TIPO_DOCUMENTO_RECLAMACION,
        //20200520 BGN ADD END R#21754: Colgar facturas en información de reclamación

        [StringValue("ACTIVOTLS12ICI_ACT")]
        ACTIVOTLS12ICI_ACT,
        [StringValue("ACTIVOTLS12SIEL")]
        ACTIVOTLS12SIEL,
        [StringValue("ACTIVOTLS12MAPFRE")]
        ACTIVOTLS12MAPFRE,
        [StringValue("ACTIVOTLS12INTERACCION")]
        ACTIVOTLS12INTERACCION,
        [StringValue("ACTIVOTLS12EDATALIAMAIL")]
        ACTIVOTLS12EDATALIAMAIL,
        [StringValue("ACTIVOTLS12VALCRED")]
        ACTIVOTLS12VALCRED,
        [StringValue("ACTIVOTLS12EDAMOVIL")]
        ACTIVOTLS12EDAMOVIL,
        [StringValue("ACTIVOTLS12DATOSCONTRATO")]
        ACTIVOTLS12DATOSCONTRATO,
        [StringValue("ACTIVOTLS12CONDOC")]
        ACTIVOTLS12CONDOC,





        //20200922 BRU R#23245: Guardar el Ticket de combustión
        [StringValue("TICKET_COMBUSTION_CAMPOS_REQUEST")]
        TICKET_COMBUSTION_CAMPOS_REQUEST,
        [StringValue("TICKET_COMBUSTION_CAMPOS_REQUEST_OBLIGATORIOS")]
        TICKET_COMBUSTION_CAMPOS_REQUEST_OBLIGATORIOS,

        //20210107 BRU R#26718: Guardar el Ticket de combustión Fase 3. Validaciones
        [StringValue("TICKET_COMBUSTION_CUENTA_CORREO_ANOMALIA_CRITICA")]
        TICKET_COMBUSTION_CUENTA_CORREO_ANOMALIA_CRITICA,
        [StringValue("TICKET_COMBUSTION_SUB_TIPO_SOLICITUDES_A_TRATAR")]
        TICKET_COMBUSTION_SUB_TIPO_SOLICITUDES_A_TRATAR,
        [StringValue("TICKET_COMBUSTION_ESTADOS_SOLICITUDES_A_TRATAR")]
        TICKET_COMBUSTION_ESTADOS_SOLICITUDES_A_TRATAR,
        [StringValue("TICKET_COMBUSTION_ESTADOS_VISITAS_A_TRATAR")]
        TICKET_COMBUSTION_ESTADOS_VISITAS_A_TRATAR,
        [StringValue("TICKET_COMBUSTION_SUB_TIPO_SOL_CLAVE_EXEPCION")]
        TICKET_COMBUSTION_SUB_TIPO_SOL_CLAVE_EXEPCION,
        [StringValue("TICKET_COMBUSTION_PROVEEDORES_A_TRATAR")]
        TICKET_COMBUSTION_PROVEEDORES_A_TRATAR,
        [StringValue("TICKET_COMBUSTION_ACTIVAR_VALIDACIONES")]
        TICKET_COMBUSTION_ACTIVAR_VALIDACIONES,
        [StringValue("TICKET_COMBUSTION_ACTIVAR_VALIDACIONES_VISITAS")]
        TICKET_COMBUSTION_ACTIVAR_VALIDACIONES_VISITAS,
        [StringValue("TICKET_COMBUSTION_ACTIVAR_VALIDACIONES_SOLICITUDES")]
        TICKET_COMBUSTION_ACTIVAR_VALIDACIONES_SOLICITUDES,


        //20210505 BRU R#29497 - Guardar el Ticket de combustión Fase 6, datos por Web
        [StringValue("TICKET_COMBUSTION_ACTIVAR_FORMULARIO_EDICION")]
        TICKET_COMBUSTION_ACTIVAR_FORMULARIO_EDICION,
        [StringValue("TICKET_COMBUSTION_ESTADOS_VISITAS_A_TRATAR_OPERA")]
        TICKET_COMBUSTION_ESTADOS_VISITAS_A_TRATAR_OPERA,

        //20210520 BRU R#29496: Guardar el Ticket de combustión Fase 5, fichero y mail al cliente
        [StringValue("TICKET_COMBUSTION_RUTA_PLANTILLAS_MAILING")]
        TICKET_COMBUSTION_RUTA_PLANTILLAS_MAILING,
        [StringValue("TICKET_COMBUSTION_RUTA_FICHEROS_MAILING")]
        TICKET_COMBUSTION_RUTA_FICHEROS_MAILING,
        [StringValue("TICKET_COMBUSTION_ACTIVAR_GUARDADO_INFORME")]
        TICKET_COMBUSTION_ACTIVAR_GUARDADO_INFORME,
        [StringValue("TICKET_COMBUSTION_RUTA_INF_REVISION_MANTENIMIENTO")]
        TICKET_COMBUSTION_RUTA_INF_REVISION_MANTENIMIENTO,
        [StringValue("TICKET_COMBUSTION_ACTIVAR_ENVIO_INFORME")]
        TICKET_COMBUSTION_ACTIVAR_ENVIO_INFORME,

        //20210820 BRU R#31134 - Excepciones procesamiento peticiones Ticket Combustion
        [StringValue("TICKET_COMBUSTION_VALIDACION_ACTIVA_VISITAS")]
        TICKET_COMBUSTION_VALIDACION_ACTIVA_VISITAS,
        [StringValue("TICKET_COMBUSTION_VALIDACION_ACTIVA_SOLICITUDES")]
        TICKET_COMBUSTION_VALIDACION_ACTIVA_SOLICITUDES,

        //20201202 BRU R#28313: Cambio Flujo Gas Confort por digitalización contrato
        [StringValue("RUTA_PLANTILLA_CONTRATO_GAS_CONFORT")]
        RUTA_PLANTILLA_CONTRATO_GAS_CONFORT,
        [StringValue("RUTA_DESTINO_CONTRATOS_GENERADOS_GAS_CONFORT")]
        RUTA_DESTINO_CONTRATOS_GENERADOS_GAS_CONFORT,

        /// <summary>
        /// Indica si se realiza la comprobacion del certificado para las llamdas desde Delta
        /// </summary>
        [StringValue("HABILITAR_BUSQUEDA_CAMPANIA_SIDAT")]
        HABILITAR_BUSQUEDA_CAMPANIA_SIDAT,

        //20201221 BGN BEG R#21644 Activar WS Interacciones
        [StringValue("WS_INTERACCIONES_USUARIOS")]
        WS_INTERACCIONES_USUARIOS,
        //20201221 BGN END R#21644 Activar WS Interacciones

        //20210120 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        [StringValue("RUTA_FICHEROS_EDATALIA")]
        RUTA_FICHEROS_EDATALIA,
        [StringValue("RUTA_FICHEROS_EDATALIA_ENVIADOS")]
        RUTA_FICHEROS_EDATALIA_ENVIADOS,
        [StringValue("RUTA_FICHEROS_EDATALIA_FIRMADOS")]
        RUTA_FICHEROS_EDATALIA_FIRMADOS,
        //20210120 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

        //20200528 BGN BEG R#22132 Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        [StringValue("EXPEDIENTES_GESTION_DOCUMENTAL")]
        EXPEDIENTES_GESTION_DOCUMENTAL,
        //20200528 BGN END R#22132 Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental

        //20210923 BGN BEG R#33847 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        [StringValue("HABILITAR_ENVIO_DOC_MAIL")]
        HABILITAR_ENVIO_DOC_MAIL,
        [StringValue("RUTA_DESTINO_DOCUMENTOS_DESCARGA_DELTA")]
        RUTA_DESTINO_DOCUMENTOS_DESCARGA_DELTA,
        //20210923 BGN END R#33847 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        [StringValue("PAIS_BUSQUEDA_POR_DNI")]
        PAIS_BUSQUEDA_POR_DNI,

        //20211117 BUA BEG R#22788 - Cambiar los mensajes de error en la ventana del login
        [StringValue("LOGIN_PAGE_MSG_ACTIVAR_MENSAJES_PERSONALIZADOS")]
        LOGIN_PAGE_MSG_ACTIVAR_MENSAJES_PERSONALIZADOS,
        [StringValue("GUARDAD_ACCESOS_PANTALLAS")]
        GUARDAD_ACCESOS_PANTALLAS, 
        [StringValue("LOGIN_PAGE_MSG_BAJA_POR_INACTIVADAD")]
        LOGIN_PAGE_MSG_BAJA_POR_INACTIVADAD,
        [StringValue("LOGIN_PAGE_MSG_CONDICIONES_NO_FIRMADAS")]
        LOGIN_PAGE_MSG_CONDICIONES_NO_FIRMADAS,
        [StringValue("LOGIN_PAGE_MSG_ERROR_EN_LAS_CREDENCIALES")]
        LOGIN_PAGE_MSG_ERROR_EN_LAS_CREDENCIALES,
        [StringValue("LOGIN_PAGE_MSG_NO_REGISTRADO_APLICACION")]
        LOGIN_PAGE_MSG_NO_REGISTRADO_APLICACION,
        //20211117 BUA END R#22788 - Cambiar los mensajes de error en la ventana del login

        //20211221 BUA BEG R#35685 - Ticket combustion. Permitir cerrar visita erronea cuando solicitud averia asociada se cancela o Cerradas sin aceptar presupuesto
        [StringValue("TICKET_COMBUSTION_ESTADOS_SOLICITUD_VISITAERRONEA")]
        TICKET_COMBUSTION_ESTADOS_SOLICITUD_VISITAERRONEA
        //20211221 BUA END R#35685 - Ticket combustion. Permitir cerrar visita erronea cuando solicitud averia asociada se cancela o Cerradas sin aceptar presupuesto

    }

    public enum ConexionesYRutas
    {
        [StringValue("SIDAT_CADENA_CONEXION")]
        SIDAT_CADENA_CONEXION,
        [StringValue("SIDAT_QUERY_CAMPANIA")]
        SIDAT_QUERY_CAMPANIA
    }

    /// <summary>
    /// Enumerados con los tipos de datos.
    /// </summary>
    public enum TipoDatos : int
    {
        ListaMultiple = 1,
        ListaSencilla = 2,
        Fecha = 3,
        TextoLibre = 4,
        Numero = 5,
        SiNo = 6,
        Decimal = 7
    };

    public enum TipoDocumento : int
    { 
        CierreVisita = 1,
        CierreSolicitud = 2
    }

    public enum EnumTipoEquipo : int
    {
        /// <summary>
        /// Estanca(Tipo C)
        /// </summary>
        [StringValue("Estanca(Tipo C)")]
        ESTANCATIPOC = 1,
        /// <summary>
        /// Atmosferica(Tipo B)
        /// </summary>
        [StringValue("Atmosferica(Tipo B)")]
        ATMOSFERICATIPOB = 2,
        /// <summary>
        /// CondensaciOn
        /// </summary>
        [StringValue("Condensacion")]
        CONDENSACION = 6,
        /// <summary>
        /// Caldera
        /// </summary>
        [StringValue("Caldera")]
        CALDERA = 7,
        /// <summary>
        /// Calentador
        /// </summary>
        [StringValue("Calentador")]
        CALENTADOR = 8,
        /// <summary>
        /// Otras
        /// </summary>
        [StringValue("Otras")]
        OTRAS = 9
    }

    public enum EnumTipoVisita : int
    {
        /// <summary>
        /// DEFECTOS MENORES
        /// </summary>
        [StringValue("DEFECTOS MENORES")]
        DEFECTOSMENORES = 1,
        /// <summary>
        /// ANOMALIA SECUNDARIA
        /// </summary>
        [StringValue("ANOMALIA SECUNDARIA")]
        ANOMALIASECUNDARIA = 2,
        /// <summary>
        /// PRECINTADO DE INSTALACION
        /// </summary>
        [StringValue("PRECINTADO DE INSTALACION")]
        PRECINTADODEINSTALACION = 3,
        /// <summary>
        /// SIN DEFECTOS
        /// </summary>
        [StringValue("SIN DEFECTOS")]
        SINDEFECTOS = 4,
        /// <summary>
        /// VISITA SIN ANOMALIA CON VALORES FUERA DE RANGO
        /// </summary>
        [StringValue("VISITA SIN ANOMALIA CON VALORES FUERA DE RANGO")]
        VISITASINANOMALIACONVALORESFUERADERANGO = 5
    }
}
