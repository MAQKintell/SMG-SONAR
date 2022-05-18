using System.Collections.Generic;
namespace Iberdrola.SMG.WS
{


    public class WSContratoVigente
    {
        public string CONTRATO_VIGENTE { get; set; }
        public string INDICATIVO_ALTA { get; set; }
        public string NUM_AVERIAS_REALIZADAS { get; set; }
        public string NUM_AVERIAS_SOPORTADAS { get; set; }
        public string IND_GENERACION_INCIDENCIA_AUTOMATICA { get; set; }
        public string TIPO_SOLICITUD_GENERAR { get; set; }
        public string IND_GENERACION_VISITA_AUTOMATICA { get; set; }
        public string TIPO_VISITA_GENERAR { get; set; }
        public string MARCA_CALDERA { get; set; }
        public string MODELO_CALDERA { get; set; }
        public string SOLICITUD_VISITA_ABIERTA { get; set; }
        public string ESTADO_VISITA { get; set; }

        public string COBERTURA { get; set; }
        public string COD_COBERTURA { get; set; }
        public string FECHA_RENOVACION { get; set; }
        public string NUMERO_SERIE_TERMOSTATO { get; set; }
        public string NUMERO_SERIE_CALDERA { get; set; }
        public string MODO_PAGO { get; set; }

        //20210628 BGN BEG R#31332 Modificar WS con App para añadir campos que indiquen si se puede solicitar averías o no
        public string ABRIR_AVERIA { get; set; }
        //20210628 BGN END R#31332 Modificar WS con App para añadir campos que indiquen si se puede solicitar averías o no
    }

    public class WSContratoVigenteNoExistente
    {
        public string CONTRATO_VIGENTE { get; set; }
        public string INDICATIVO_ALTA { get; set; }
        public string NUM_AVERIAS_REALIZADAS { get; set; }
        public string NUM_AVERIAS_SOPORTADAS { get; set; }
    }
    /// <summary>
    /// Clase que contiene la respuesta a la llamada de
    /// </summary>
    public class ContratoVigenteResponse : WSResponse
    {

        //private string _CONTRATO_VIGENTE = " ";
        //public string CONTRATO_VIGENTE { get { return _CONTRATO_VIGENTE; } set { _CONTRATO_VIGENTE = value; } }


        //private string _INDICATIVO_ALTA = " ";
        //public string INDICATIVO_ALTA { get { return _INDICATIVO_ALTA; } set { _INDICATIVO_ALTA = value; } }

        //private string _NUM_AVERIAS_REALIZADAS = " ";
        //public string NUM_AVERIAS_REALIZADAS { get { return _NUM_AVERIAS_REALIZADAS; } set { _NUM_AVERIAS_REALIZADAS = value; } }

        //private string _NUM_AVERIAS_SOPORTADAS = " ";
        //public string NUM_AVERIAS_SOPORTADAS { get { return _NUM_AVERIAS_SOPORTADAS; } set { _NUM_AVERIAS_SOPORTADAS = value; } }

        //private string _IND_GENERACION_INCIDENCIA_AUTOMATICA = " ";
        //public string IND_GENERACION_INCIDENCIA_AUTOMATICA { get { return _IND_GENERACION_INCIDENCIA_AUTOMATICA; } set { _IND_GENERACION_INCIDENCIA_AUTOMATICA = value; } }

        //private string _TIPO_SOLICITUD_GENERAR = " ";
        //public string TIPO_SOLICITUD_GENERAR { get { return _TIPO_SOLICITUD_GENERAR; } set { _TIPO_SOLICITUD_GENERAR = value; } }

        //private string _IND_GENERACION_VISITA_AUTOMATICA = " ";
        //public string IND_GENERACION_VISITA_AUTOMATICA { get { return _IND_GENERACION_VISITA_AUTOMATICA; } set { _IND_GENERACION_VISITA_AUTOMATICA = value; } }

        //private string _TIPO_VISITA_GENERAR = " ";
        //public string TIPO_VISITA_GENERAR { get { return _TIPO_VISITA_GENERAR; } set { _TIPO_VISITA_GENERAR = value; } }

        //private string _MARCA_CALDERA;
        //public string MARCA_CALDERA { get { return _MARCA_CALDERA; } set { _MARCA_CALDERA = value; } }

        //private string _MODELO_CALDERA;
        //public string MODELO_CALDERA { get { return _MODELO_CALDERA; } set { _MODELO_CALDERA = value; } }
        
        public ContratoVigenteResponse() : base() { }

        public List<WSContratoVigente> listaContratoVigente = new List<WSContratoVigente>();

        public void AddDatosContratoVigente(string sContrato,
                                            string sIndicativoAlta,
                                            string sNumAveriasRealizadas,
                                            string sNumAveriasSoportadas,
                                            string sIndGenIncidenciaAutomatica,
                                            string sTipoSolicitud,
                                            string sIndGenVisitaAutomatica,
                                            string sTipoVisita,
                                            string sMarca,
                                            string sModelo,
                                            string sSolicitudVisitaAbierta,
                                            string sEstadoVisita,
                                            string sCobertura,
                                            string sFECHARENOVACION,
                                            string sNUMEROSERIETERMOSTATO,
                                            string sNUMEROSERIECALDERA,
                                            string sMODOPAGO,
                                            string sCodCobertura,
                                            string sAbrirAveria
                                            )
        {
            WSContratoVigente  lista = new WSContratoVigente();

            lista.CONTRATO_VIGENTE  = sContrato;
            lista.INDICATIVO_ALTA   = sIndicativoAlta;
            lista.NUM_AVERIAS_REALIZADAS  = sNumAveriasRealizadas;
            lista.NUM_AVERIAS_SOPORTADAS = sNumAveriasSoportadas;
            lista.IND_GENERACION_INCIDENCIA_AUTOMATICA  = sIndGenIncidenciaAutomatica;
            lista.TIPO_SOLICITUD_GENERAR  = sTipoSolicitud;
            lista.IND_GENERACION_VISITA_AUTOMATICA = sIndGenVisitaAutomatica;
            lista.TIPO_VISITA_GENERAR = sTipoVisita;
            lista.MARCA_CALDERA  = sMarca;
            lista.MODELO_CALDERA  = sModelo;
            lista.MODO_PAGO = sMODOPAGO;
            // KINTELL 29/12/2016 SACAMOS SI TIENE SOLICITUD DE VISITA ABIERTA Y EL ESTADO DE LA VISITA.
            lista.SOLICITUD_VISITA_ABIERTA = sSolicitudVisitaAbierta;
            lista.ESTADO_VISITA = sEstadoVisita;
            // 17/12/2018 Cambios en Tu Iberdrola P&S.
            lista.COBERTURA = sCobertura;
            lista.COD_COBERTURA = sCodCobertura;
            lista.FECHA_RENOVACION = sFECHARENOVACION;
            lista.NUMERO_SERIE_TERMOSTATO = sNUMEROSERIETERMOSTATO;
            lista.NUMERO_SERIE_CALDERA = sNUMEROSERIECALDERA;

            //20210628 BGN BEG R#31332 Modificar WS con App para añadir campos que indiquen si se puede solicitar averías o no
            lista.ABRIR_AVERIA = sAbrirAveria;
            //20210628 BGN END R#31332 Modificar WS con App para añadir campos que indiquen si se puede solicitar averías o no

            this.listaContratoVigente.Add(lista);
        }

        //Lista que devuelve en caso de que no exista ningún servicio para ese contrato

        public List<WSContratoVigenteNoExistente> listaContratoVigenteNoExistente = new List<WSContratoVigenteNoExistente>();

        public void AddDatosContratoVigenteNoExistente( string sContrato,
                                                        string sIndicativoAlta,
                                                        string sNumAveriasRealizadas,
                                                        string sNumAveriasSoportadas
                                                        )
        {
            WSContratoVigenteNoExistente lista = new WSContratoVigenteNoExistente();

            lista.CONTRATO_VIGENTE = sContrato;
            lista.INDICATIVO_ALTA = sIndicativoAlta;
            lista.NUM_AVERIAS_REALIZADAS = sNumAveriasRealizadas;
            lista.NUM_AVERIAS_SOPORTADAS = sNumAveriasSoportadas;
            

            this.listaContratoVigenteNoExistente.Add(lista);
        }

    }
}