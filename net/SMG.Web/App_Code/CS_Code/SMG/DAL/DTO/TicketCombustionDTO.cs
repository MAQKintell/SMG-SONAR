using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Iberdrola.SMG.DAL.DTO
{
    /// <summary>
    /// Descripción breve de TicketCombustionDTO
    /// </summary>
    public class TicketCombustionDTO : BaseDTO
    {
        #region atributos

        private decimal? _idTicketCombustion;
        private string _codigoContrato;
        private decimal? _idSolicitud;
        private int? _codigoVisita;
        private int? _tipoEquipo;
        private decimal? _temperaturaPDC;
        private decimal? _cOCorregido;
        private decimal? _tiro;
        private decimal? _cOAmbiente;
        private decimal? _o2;
        private decimal? _cO;
        private decimal? _cO2;
        private decimal? _lambda;
        private decimal? _rendimiento;
        private decimal? _temperaturaMaxACS;
        private decimal? _caudalACS;
        private decimal? _potenciaUtil;
        private decimal? _temperaturaEntradaACS;
        private decimal? _temperaturaSalidaACS;
        private decimal? _idFicheroTicketCombustion;
        private decimal? _idFicheroConductoHumos;
        private string _comentarios;
        private string _nombreFicheroConductoHumos;
        private byte[] _ficheroConductoHumos;
        private string _nombreFichero;
        private byte[] _contenidoFichero;
        private string _proveedor;
        private string _telefonoContacto1;
        private string _personaContacto;
        private int? _subTipoSolicitud;
        private int? _estadoActualSolicitud;
        //20210624 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
        private string _tipoVisita;
        private decimal? _idSolicitudAveria;
        private bool _esGasConfort;
        //20210624 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion

        #endregion

        #region propiedades
        public decimal? IdTicketCombustion
        {
            get { return this._idTicketCombustion; }
            set { this._idTicketCombustion = value; }
        }
        public String CodigoContrato
        {
            get { return this._codigoContrato; }
            set { this._codigoContrato = value; }
        }
        public decimal? IdSolicitud
        {
            get { return this._idSolicitud; }
            set { this._idSolicitud = value; }
        }
        public int? CodigoVisita
        {
            get { return this._codigoVisita; }
            set { this._codigoVisita = value; }
        }
        public int? TipoEquipo
        {
            get { return this._tipoEquipo; }
            set { this._tipoEquipo = value; }
        }
        public decimal? TemperaturaPDC
        {
            get { return this._temperaturaPDC; }
            set { this._temperaturaPDC = value; }
        }
        public decimal? COCorregido
        {
            get { return this._cOCorregido; }
            set { this._cOCorregido = value; }
        }
        public decimal? Tiro
        {
            get { return this._tiro; }
            set { this._tiro = value; }
        }
        public decimal? COAmbiente
        {
            get { return this._cOAmbiente; }
            set { this._cOAmbiente = value; }
        }
        public decimal? O2
        {
            get { return this._o2; }
            set { this._o2 = value; }
        }
        public decimal? CO
        {
            get { return this._cO; }
            set { this._cO = value; }
        }
        public decimal? CO2
        {
            get { return this._cO2; }
            set { this._cO2 = value; }
        }
        public decimal? Lambda
        {
            get { return this._lambda; }
            set { this._lambda = value; }
        }
        public decimal? Rendimiento
        {
            get { return this._rendimiento; }
            set { this._rendimiento = value; }
        }
        public decimal? TemperaturaMaxACS
        {
            get { return this._temperaturaMaxACS; }
            set { this._temperaturaMaxACS = value; }
        }
        public decimal? CaudalACS
        {
            get { return this._caudalACS; }
            set { this._caudalACS = value; }
        }
        public decimal? PotenciaUtil
        {
            get { return this._potenciaUtil; }
            set { this._potenciaUtil = value; }
        }
        public decimal? TemperaturaEntradaACS
        {
            get { return this._temperaturaEntradaACS; }
            set { this._temperaturaEntradaACS = value; }
        }
        public decimal? TemperaturaSalidaACS
        {
            get { return this._temperaturaSalidaACS; }
            set { this._temperaturaSalidaACS = value; }
        }
        public decimal? IdFicheroTicketCombustion
        {
            get { return this._idFicheroTicketCombustion; }
            set { this._idFicheroTicketCombustion = value; }
        }
        public decimal? IdFicheroConductoHumos
        {
            get { return this._idFicheroConductoHumos; }
            set { this._idFicheroConductoHumos = value; }
        }
        public string Comentarios
        {
            get { return this._comentarios; }
            set { this._comentarios = value; }
        }
        public string NombreFicheroConductoHumos
        {
            get { return this._nombreFicheroConductoHumos; }
            set { this._nombreFicheroConductoHumos = value; }
        }
        public byte[] FicheroConductoHumos
        {
            get { return this._ficheroConductoHumos; }
            set { this._ficheroConductoHumos = value; }
        }
        public string NombreFichero
        {
            get { return this._nombreFichero; }
            set { this._nombreFichero = value; }
        }
        public byte[] ContenidoFichero
        {
            get { return this._contenidoFichero; }
            set { this._contenidoFichero = value; }
        }
        public string Proveedor
        {
            get { return this._proveedor; }
            set { this._proveedor = value; }
        }
        public string TelefonoContacto1
        {
            get { return this._telefonoContacto1; }
            set { this._telefonoContacto1 = value; }
        }
        public string PersonaContacto
        {
            get { return this._personaContacto; }
            set { this._personaContacto = value; }
        }

        public int? SubTipoSolicitud
        {
            get { return this._subTipoSolicitud; }
            set { this._subTipoSolicitud = value; }
        }

        public int? EstadoActualSolicitud
        {
            get { return this._estadoActualSolicitud; }
            set { this._estadoActualSolicitud = value; }
        }

        public string TipoVisita
        {
            get { return this._tipoVisita; }
            set { this._tipoVisita = value; }
        }
        //20210624 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
        public decimal? IdSolicitudAveria
        {
            get { return this._idSolicitudAveria; }
            set { this._idSolicitudAveria = value; }
        }
        public bool EsGasConfort
        {
            get { return this._esGasConfort; }
            set { this._esGasConfort = value; }
        }
        //20210624 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion
        #endregion

        #region constructores
        public TicketCombustionDTO()
        {

        }
        #endregion

        #region  implementación de metodos heredados de BaseDTO
        public int CompareTo(object obj)
        {
            //TODO implementar el método
            return -1;
        }

        public object Clone()
        {
            //TODO implementar el método
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //TODO implementar el método
        }

        #endregion
    }
}