using System;
using System.Runtime.Serialization;

namespace Iberdrola.SMG.DAL.DTO
{
    /// <summary>
    /// Atributos y Propiedades de la entidad Documento
    /// </summary>
    public partial class DocumentoDTO : BaseDTO
    {
        #region atributos
        private decimal _idDocumento;
        private string _codContrato;
        private int? _codVisita;
        private int? _idSolicitud;
        private string _nombreDocumento;
        private int? _idTipoDocumento;
        private DateTime? _fechaBaja;
        private DateTime? _fechaEnvioDelta;

        //20210111 BGN ADD BEG R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        private string _codTipoDocumento;
        private string _descTipoDocumento;
        //20210111 BGN ADD END R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental

        //20210118 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        private bool _enviarADelta;
        private string _idEnvioEdatalia;
        private DateTime? _fechaEnvioEdatalia;
        private string _idDocEdatalia;
        private int? _idEstadoEdatalia;
        private DateTime? _fechaRecibidoEdatalia;
        //20210118 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital

        #endregion

        #region propiedades
        /// <summary>
        /// Obtiene/Establece el _idDocumento
        /// 
        /// </summary>
        public decimal IdDocumento
        {
            get { return this._idDocumento; }
            set { this._idDocumento = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _codContrato
        /// 
        /// </summary>
        public string CodContrato
        {
            get { return this._codContrato; }
            set { this._codContrato = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _codVisita
        /// 
        /// </summary>
        public int? CodVisita
        {
            get { return this._codVisita; }
            set { this._codVisita = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _idSolicitud
        /// 
        /// </summary>
        public int? IdSolicitud
        {
            get { return this._idSolicitud; }
            set { this._idSolicitud = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _nombreDocumento
        /// 
        /// </summary>
        public string NombreDocumento
        {
            get { return this._nombreDocumento; }
            set { this._nombreDocumento = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _idTipoDocumento
        /// 
        /// </summary>
        public int? IdTipoDocumento
        {
            get { return this._idTipoDocumento; }
            set { this._idTipoDocumento = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _fechaBaja
        /// 
        /// </summary>
        public DateTime? FechaBaja
        {
            get { return this._fechaBaja; }
            set { this._fechaBaja = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _fechaEnvioDelta
        /// 
        /// </summary>
        public DateTime? FechaEnvioDelta
        {
            get { return this._fechaEnvioDelta; }
            set { this._fechaEnvioDelta = value; }
        }

        //20210111 BGN ADD BEG R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        /// <summary>
        /// Obtiene/Establece el _codTipoDocumento
        /// 
        /// </summary>
        public string CodTipoDocumento
        {
            get { return this._codTipoDocumento; }
            set { this._codTipoDocumento = value; }
        }
        
        /// <summary>
        /// Obtiene/Establece el _descTipoDocumento
        /// 
        /// </summary>
        public string DescTipoDocumento
        {
            get { return this._descTipoDocumento; }
            set { this._descTipoDocumento = value; }
        }
        //20210111 BGN ADD END R#22132: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental

        //20210118 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        
        /// <summary>
        /// Obtiene/Establece el _enviarADelta
        /// 
        /// </summary>
        public bool EnviarADelta
        {
            get { return this._enviarADelta; }
            set { this._enviarADelta = value; }
        }
        
        /// <summary>
        /// Obtiene/Establece el _idEnvioEdatalia
        /// 
        /// </summary>
        public string IdEnvioEdatalia
        {
            get { return this._idEnvioEdatalia; }
            set { this._idEnvioEdatalia = value; }
        }
        
        /// <summary>
        /// Obtiene/Establece el _fechaEnvioEdatalia
        /// 
        /// </summary>
        public DateTime? FechaEnvioEdatalia
        {
            get { return this._fechaEnvioEdatalia; }
            set { this._fechaEnvioEdatalia = value; }
        }
        
        /// <summary>
        /// Obtiene/Establece el _idDocEdatalia
        /// 
        /// </summary>
        public string IdDocEdatalia
        {
            get { return this._idDocEdatalia; }
            set { this._idDocEdatalia = value; }
        }

        /// <summary>
        /// Obtiene/Establece el _idEstadoEdatalia
        /// 
        /// </summary>
        public int? IdEstadoEdatalia
        {
            get { return this._idEstadoEdatalia; }
            set { this._idEstadoEdatalia = value; }
        }

        /// <summary>
        /// Obtiene/Establece el _fechaRecibidoEdatalia
        /// 
        /// </summary>
        public DateTime? FechaRecibidoEdatalia
        {
            get { return this._fechaRecibidoEdatalia; }
            set { this._fechaRecibidoEdatalia = value; }
        }
        //20210118 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

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