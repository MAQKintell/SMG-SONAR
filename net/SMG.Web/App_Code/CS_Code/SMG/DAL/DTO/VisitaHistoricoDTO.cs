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
using System.Collections.Generic;
using System.Runtime.Serialization;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Web;

namespace Iberdrola.SMG.DAL.DTO
{
    /// <summary>
    /// Descripción breve de VisitaHistoricoDTO
    /// </summary>
    public class VisitaHistoricoDTO : BaseDTO
    {
        #region Atributos
        private String _codContrato;
        private Decimal _codVisita;
        private DateTime _fecha;
        private String _usuario;
        private String _codEstadoVisita;
        private Decimal? _idLote;
        #endregion

        #region Propiedades
        public String CodContrato
        {
            get { return this._codContrato; }
            set { this._codContrato = value; }
        }
        public Decimal CodVisita
        {
            get { return this._codVisita; }
            set { this._codVisita = value; }
        }
        public DateTime Fecha
        {
            get { return this._fecha; }
            set { this._fecha = value; }
        }
        public String Usuario
        {
            get { return this._usuario; }
            set { this._usuario = value; }
        }
        public String CodEstadoVisita
        {
            get { return this._codEstadoVisita; }
            set { this._codEstadoVisita = value; }
        }
        public Decimal? IdLote
        {
            get { return this._idLote; }
            set { this._idLote = value; }
        }
        #endregion

        #region constructores
        public VisitaHistoricoDTO(String CodContrato, Decimal CodVisita, DateTime? Fecha,
                                String CodEstadoVisita)
        {
            _codContrato = CodContrato;
            _codVisita = CodVisita;
            if (Fecha.HasValue)
            {
                _fecha = Fecha.Value;
            }
            else
            {
                _fecha = DateTime.Now;
            }
            AppPrincipal usuarioPrincipal = (AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            _usuario = usuarioPrincipal.Identity.Name;
            _codEstadoVisita = CodEstadoVisita;
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