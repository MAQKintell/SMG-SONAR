using System;
using Iberdrola.Commons.BaseClasses;
using System.Runtime.Serialization;

namespace Iberdrola.SMG.DAL.DTO
{
    /// <summary>
    /// Atributos y Propiedades de la entidad PWebservice
    /// </summary>
    public partial class WebserviceDTO : BaseDTO
    {
        #region atributos
        private decimal _idWebservice;
        private string _nombreWebservice;
        private string _urlWebservice;
        private string _metodoWebservice;
        private string _codProveedorWebservice;
        private string _usuarioWebservice;
        private string _passwordWebservice;
        private string _plantillaWebservice;
        private string _tipoWebservice;
        private bool _activoWebservice;
        private bool _ProxyActivoWebService;

        #endregion
        
        #region propiedades
		/// <summary>
        /// Obtiene/Establece el _idWebservice
        /// INT( Interaccion) , LLP(Llamada Proveedor)
        /// </summary>
        public decimal IdWebservice
        {
            get { return this._idWebservice; }
            set { this._idWebservice = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _nombreWebservice
        /// 
        /// </summary>
        public string NombreWebservice
        {
            get { return this._nombreWebservice; }
            set { this._nombreWebservice = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _urlWebservice
        /// 
        /// </summary>
        public string UrlWebservice
        {
            get { return this._urlWebservice; }
            set { this._urlWebservice = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _metodoWebservice
        /// 
        /// </summary>
        public string MetodoWebservice
        {
            get { return this._metodoWebservice; }
            set { this._metodoWebservice = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _codProveedorWebservice
        /// 
        /// </summary>
        public string CodProveedorWebservice
        {
            get { return this._codProveedorWebservice; }
            set { this._codProveedorWebservice = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _usuarioWebservice
        /// 
        /// </summary>
        public string UsuarioWebservice
        {
            get { return this._usuarioWebservice; }
            set { this._usuarioWebservice = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _passwordWebservice
        /// 
        /// </summary>
        public string PasswordWebservice
        {
            get { return this._passwordWebservice; }
            set { this._passwordWebservice = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _plantillaWebservice
        /// 
        /// </summary>
        public string PlantillaWebservice
        {
            get { return this._plantillaWebservice; }
            set { this._plantillaWebservice = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _tipoWebservice
        /// 
        /// </summary>
        public string TipoWebservice
        {
            get { return this._tipoWebservice; }
            set { this._tipoWebservice = value; }
        }
        
		/// <summary>
        /// Obtiene/Establece el _activoWebservice
        /// 
        /// </summary>
        public bool ActivoWebservice
        {
            get { return this._activoWebservice; }
            set { this._activoWebservice = value; }
        }
        

        /// <summary>
        /// Obtiene/Establece el _ProxyActivoWebService
        /// 
        /// </summary>
        public bool ProxyActivoWebService
        {
            get { return this._ProxyActivoWebService; }
            set { this._ProxyActivoWebService = value; }
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