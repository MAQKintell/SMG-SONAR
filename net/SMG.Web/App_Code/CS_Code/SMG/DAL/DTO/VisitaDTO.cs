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
    /// Descripción breve de CalderaDTO
    /// </summary>
    public class VisitaDTO : BaseDTO
    {
        #region atributos
        private String _codContrato;
        private Decimal? _codVisita;
        private Decimal? _campania;
        private DateTime? _fechaVisita;
        private DateTime? _fechaLimiteVisita;
        private Decimal? _tipoUrgencia;
        private String _codEstadoVisita;
        private Decimal? _idReparacion;
        private String _observaciones;
        private Decimal? _idLote;
        private String _CUPS;
        private String _COD_RECEPTOR;
        private Boolean? _recepcionComprobante;
        private Boolean? _facturadoProveedor;
        private Boolean? _contadorinterno;
        private String _codigoInterno;
        private String _numFactura;
        private DateTime? _fechaFactura;
        private Boolean? _cartaEnviada;
        private DateTime? _fechaEnviadoCarta;
        private String _codigoBarras;
        private Int16 _Tecnico;
        private String _ObservacionesVisita;
        private String _TipoVisita;
        private DateTime? _fechaGeneracionCodigoBarras;
        private DateTime? _fechaIntroduccionCodigoBarras;
        private DateTime? _fechaBonificacion;
        private Boolean _InformacionRecibida;
        private String _Aviso;
        private String _CategoriaVisita;
        //BGN ADD INI R##25266: Visualizar si una visita tiene PRL por Web
        private Boolean? _prl;
        //BGN ADD FIN R##25266: Visualizar si una visita tiene PRL por Web

        #endregion

        #region propiedades
        public Boolean InformacionRecibida
        {
            get { return this._InformacionRecibida; }
            set { this._InformacionRecibida = value; }
        }
        public String CodigoContrato
        {
            get { return this._codContrato; }
            set { this._codContrato = value; }
        }
        public String CategoriaVisita
        {
            get { return this._CategoriaVisita; }
            set { this._CategoriaVisita = value; }
        }
        public String Aviso
        {
            get { return this._codigoInterno; }
            set { this._codigoInterno = value; }
        }
        public String CodigoInterno
        {
            get { return this._Aviso; }
            set { this._Aviso = value; }
        }
        public Decimal? CodigoVisita
        {
            get { return this._codVisita; }
            set { this._codVisita = value; }
        }
        public Decimal? Campania
        {
            get { return this._campania; }
            set { this._campania = value; }
        }
        public DateTime? FechaVisita
        {
            get { return this._fechaVisita; }
            set { this._fechaVisita = value; }
        }
        public DateTime? FechaLimiteVisita
        {
            get { return this._fechaLimiteVisita; }
            set { this._fechaLimiteVisita = value; }
        }   
        public Decimal? TipoUrgencia
        {
            get { return this._tipoUrgencia; }
            set { this._tipoUrgencia = value; }
        }
        public String CodigoEstadoVisita
        {
            get { return this._codEstadoVisita; }
            set { this._codEstadoVisita = value; }
        }      
        public Decimal? IdReparacion
        {
            get { return this._idReparacion; }
            set { this._idReparacion = value; }
        }
        public String Observaciones
        {
            get { return this._observaciones; }
            set { this._observaciones = value; }
        }      
        public Decimal? IdLote
        {
            get { return this._idLote; }
            set { this._idLote = value; }
        }
        public String CUPS
        {
            get { return this._CUPS; }
            set { this._CUPS = value; }
        }
        public String COD_RECEPTOR
        {
            get { return this._COD_RECEPTOR; }
            set { this._COD_RECEPTOR = value; }
        }
        public Boolean? RecepcionComprobante
        {
            get { return this._recepcionComprobante; }
            set { this._recepcionComprobante = value; }
        }
        public Boolean? FacturadoProveedor
        {
            get { return this._facturadoProveedor; }
            set { this._facturadoProveedor = value; }
        }
        public String NumFactura
        {
            get { return this._numFactura; }
            set { this._numFactura = value; }
        }
        public DateTime? FechaFactura
        {
            get { return this._fechaFactura; }
            set { this._fechaFactura = value; }
        }
        public Boolean? CartaEnviada
        {
            get { return this._cartaEnviada; }
            set { this._cartaEnviada = value; }
        }
        public Boolean? ContadorInterno
        {
            get { return this._contadorinterno; }
            set { this._contadorinterno = value; }
        }
        public DateTime? FechaEnviadoCarta
        {
            get { return this._fechaEnviadoCarta; }
            set { this._fechaEnviadoCarta = value; }
        }
        public String CodigoBarras
        {
            get { return this._codigoBarras; }
            set { this._codigoBarras = value; }
        }
        public Int16 Tecnico
        {
            get { return this._Tecnico; }
            set { this._Tecnico = value; }
        }
        public String ObservacionesVisita
        {
            get { return this._ObservacionesVisita; }
            set { this._ObservacionesVisita = value; }
        }
        public String TipoVisita
        {
            get { return this._TipoVisita; }
            set { this._TipoVisita = value; }
        } 
        public DateTime? FechaGeneracionCodigoBarras
        {
            get { return this._fechaGeneracionCodigoBarras; }
            set { this._fechaGeneracionCodigoBarras = value; }
        }
        public DateTime? FechaIntroduccionCodigoBarras
        {
            get { return this._fechaIntroduccionCodigoBarras; }
            set { this._fechaIntroduccionCodigoBarras = value; }
        }
        public DateTime? FechaBonificacion
        {
            get { return this._fechaBonificacion; }
            set { this._fechaBonificacion = value; }
        }

        //BGN ADD INI R#25266: Visualizar si una visita tiene PRL por Web
        public Boolean? PRL
        {
            get { return this._prl; }
            set { this._prl = value; }
        }
        //BGN ADD FIN R#25266: Visualizar si una visita tiene PRL por Web

        #endregion

        #region constructores
        public VisitaDTO()
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