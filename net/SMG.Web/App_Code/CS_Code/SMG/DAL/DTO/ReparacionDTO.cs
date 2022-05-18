using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Iberdrola.SMG.DAL.DTO
{

    /// <summary>
    /// Descripción breve de ReparacionDTO
    /// </summary>
    public class ReparacionDTO: BaseDTO
    {

        #region atributos

        private String _codContrato;
        private Decimal? _codVisita;
        private Decimal? _id = null;
        private Decimal? _idTipoReparacion = null;
        private DateTime? _fechaReparacion = null;
        private Decimal? _idTipoTiempoManoObra = null;
        private Decimal? _importeReparacion;
        private Decimal? _importeManoObraAdicional;
        private Decimal? _importeMaterialesAdicional;
        private DateTime? _fechafactura;
        private String _numerofactura;
        private String _CodigoBarrasReparacion;
        private Boolean _InformacionRecibida;
        #endregion

        #region propiedades

        public String CodContrato
        {
            get { return this._codContrato; }
            set { this._codContrato = value; }
        }
        public String CodigoBarrasReparacion
        {
            get { return this._CodigoBarrasReparacion; }
            set { this._CodigoBarrasReparacion = value; }
        }
        public Boolean InformacionRecibida
        {
            get { return this._InformacionRecibida; }
            set { this._InformacionRecibida = value; }
        }
        public String NumeroFacttura
        {
            get { return this._numerofactura; }
            set { this._numerofactura = value; }
        }

        public Decimal? CodVisita
        {
            get { return this._codVisita; }
            set { this._codVisita = value; }
        }

        public Decimal? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Decimal? IdTipoReparacion
        {
            get { return _idTipoReparacion; }
            set { _idTipoReparacion = value; }
        }

        public DateTime? FechaReparacion
        {
            get { return _fechaReparacion; }
            set { _fechaReparacion = value; }
        }

        public DateTime? FechaFactura
        {
            get { return _fechafactura; }
            set { _fechafactura = value; }
        }

        public Decimal? IdTipoTiempoManoObra
        {
            get { return _idTipoTiempoManoObra; }
            set { _idTipoTiempoManoObra = value; }
        }

        public Decimal? ImporteReparacion
        {
            get { return _importeReparacion; }
            set { _importeReparacion = value; }
        }

        public Decimal? ImporteManoObraAdicional
        {
            get { return _importeManoObraAdicional; }
            set { _importeManoObraAdicional = value; }
        }

        public Decimal? ImporteMaterialesAdicional
        {
            get { return _importeMaterialesAdicional; }
            set { _importeMaterialesAdicional = value; }
        }

        #endregion

        #region Constructores
        public ReparacionDTO()
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
