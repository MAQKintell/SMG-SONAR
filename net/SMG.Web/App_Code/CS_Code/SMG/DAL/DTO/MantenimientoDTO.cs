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
    /// Descripción breve de MantenimientoDTO
    /// </summary>
    public class MantenimientoDTO : BaseDTO
    {
        #region Atributos
            private String _COD_CONTRATO_SIC;
            private String _NOM_TITULAR;
            private String _APELLIDO1;
            private String _APELLIDO2;
            private String _DNI;
            private String _COD_PROVINCIA;
            private String _COD_POBLACION;
            private String _NOM_CALLE;
            private String _TIP_VIA_PUBLICA;

            private String _TELEFONO1;
            private String _TELEFONO2;


            private String _COD_FINCA;
            private String _TIP_BIS;
            private String _COD_PORTAL;
            private String _TIP_ESCALERA;
            private String _TIP_PISO;
            private String _TIP_MANO;
            private String _COD_POSTAL;
            private String _NUM_TEL_CLIENTE;
            private String _NUM_TEL_PS;
            private DateTime? _FEC_LIMITE_VISITA;
            private String _ESTADO_CONTRATO;
            private DateTime? _FEC_ALTA_CONTRATO;
            private DateTime? _FEC_BAJA_CONTRATO;
            private DateTime? _FEC_ALTA_SERVICIO;
            private DateTime? _FEC_BAJA_SERVICIO;
            private String _COD_TARIFA;
            private String _T1;
            private String _T2;
            private String _T5;
            private String _REVISION;
            private Boolean? _FACTURADO_PROVEEDOR;
            private Boolean _BAJA_SOLICITADA;
        
            private DateTime? _FEC_RECLAMACION;
            private Boolean? _RECLAMACION;
            private String _PROVEEDOR;
            private String _PROVEEDORAVERIA;
            private String _PROVEEDORINSPECCION;
            //TEL:
            //private String _NUM_TELEFONO1;
            //private String _NUM_TELEFONO2;
            //private String _NUM_TELEFONO3;
            //private String _NUM_TELEFONO4;
            //private String _NUM_TELEFONO5;
            private Int16? _COD_ULTIMA_VISITA;

            private String _CUPS;
            private String _OBSERVACIONES;
            private String _BCS;
            private DateTime? _FECHA_HASTA_FACTURACION;
            private String _DESEFV;
            private String _CODEFV;
            private String _SCORING;
            private String _NUM_MOVIL;
            private String _COD_RECEPTOR;

            private Double _PAGAR_SI_BAJA;

            //#2095 Pago Anticipado
            private String _SOLCENT;
            private String _PAIS;
            
            //20180711 BGN WS
            private String _COD_CLIENTE;
            private Decimal _CONTADOR_AVERIAS;
            private String _HORARIO_CONTACTO;

            //20191111 BGN ADD BEG [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
            private String _EMAIL;
            //20191111 BGN ADD END [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email

            private string _SOCIEDAD;
        #endregion

        #region Propiedades

        public String PAIS
            {
                get { return this._PAIS; }
                set { this._PAIS = value; }
            }
            public String CUPS
            {
                get { return this._CUPS; }
                set { this._CUPS = value; }
            }
            public String DESEFV
            {
                get { return this._DESEFV; }
                set { this._DESEFV = value; }
            }
            public String CODEFV
            {
                get { return this._CODEFV; }
                set { this._CODEFV = value; }
            }
            public String SCORING
            {
                get { return this._SCORING; }
                set { this._SCORING = value; }
            }
            public String OBSERVACIONES
            {
                get { return this._OBSERVACIONES; }
                set { this._OBSERVACIONES = value; }
            }
        
            public String BCS
            {
                get { return this._BCS; }
                set { this._BCS = value; }
            }
            public DateTime? FECHA_HASTA_FACTURACION
            {
                get { return this._FECHA_HASTA_FACTURACION; }
                set { this._FECHA_HASTA_FACTURACION = value; }
            }

            public String COD_CONTRATO_SIC
            {
                get { return this._COD_CONTRATO_SIC; }
                set { this._COD_CONTRATO_SIC = value; }
            }

            public string NOM_TITULAR
            {
                get { return this._NOM_TITULAR; }
                set { this._NOM_TITULAR = value; }
            }

            public string APELLIDO1
            {
                get { return this._APELLIDO1; }
                set { this._APELLIDO1 = value; }
            }
            public string APELLIDO2
            {
                get { return this._APELLIDO2; }
                set { this._APELLIDO2 = value; }
            }
            public string DNI
            {
                get { return this._DNI; }
                set { this._DNI = value; }
            }
            public string COD_PROVINCIA
            {
                get { return this._COD_PROVINCIA; }
                set { this._COD_PROVINCIA = value; }
            }
            public string COD_POBLACION
            {
                get { return this._COD_POBLACION; }
                set { this._COD_POBLACION = value; }
            }
            public string NOM_CALLE
            {
                get { return this._NOM_CALLE; }
                set { this._NOM_CALLE = value; }
            }
            public string TIP_VIA_PUBLICA
            {
                get { return this._TIP_VIA_PUBLICA; }
                set { this._TIP_VIA_PUBLICA = value; }
            }


            public string TELEFONO1
            {
                get { return this._TELEFONO1; }
                set { this._TELEFONO1 = value; }
            }
            public string TELEFONO2
            {
                get { return this._TELEFONO2; }
                set { this._TELEFONO2 = value; }
            }



            public string COD_FINCA
            {
                get { return this._COD_FINCA; }
                set { this._COD_FINCA = value; }
            }
            public string TIP_BIS
            {
                get { return this._TIP_BIS; }
                set { this._TIP_BIS = value; }
            }
            public string COD_PORTAL
            {
                get { return this._COD_PORTAL; }
                set { this._COD_PORTAL = value; }
            }
            public string TIP_ESCALERA
            {
                get { return this._TIP_ESCALERA; }
                set { this._TIP_ESCALERA = value; }
            }
            public string TIP_PISO
            {
                get { return this._TIP_PISO; }
                set { this._TIP_PISO = value; }
            }
            public string TIP_MANO
            {
                get { return this._TIP_MANO; }
                set { this._TIP_MANO = value; }
            }
            public string COD_POSTAL
            {
                get { return this._COD_POSTAL; }
                set { this._COD_POSTAL = value; }
            }
            public string NUM_TEL_CLIENTE
            {
                get { return this._NUM_TEL_CLIENTE; }
                set { this._NUM_TEL_CLIENTE = value; }
            }
            public string NUM_TEL_PS
            {
                get { return this._NUM_TEL_PS; }
                set { this._NUM_TEL_PS = value; }
            }
            public DateTime? FEC_LIMITE_VISITA
            {
                get { return this._FEC_LIMITE_VISITA; }
                set { this._FEC_LIMITE_VISITA = value; }
            }
            public string ESTADO_CONTRATO
            {
                get { return this._ESTADO_CONTRATO; }
                set { this._ESTADO_CONTRATO = value; }
            }
            public DateTime? FEC_ALTA_CONTRATO
            {
                get { return this._FEC_ALTA_CONTRATO; }
                set { this._FEC_ALTA_CONTRATO = value; }
            }
            public DateTime? FEC_BAJA_CONTRATO
            {
                get { return this._FEC_BAJA_CONTRATO; }
                set { this._FEC_BAJA_CONTRATO = value; }
            }
            public DateTime? FEC_ALTA_SERVICIO
            {
                get { return this._FEC_ALTA_SERVICIO; }
                set { this._FEC_ALTA_SERVICIO = value; }
            }
            public DateTime? FEC_BAJA_SERVICIO
            {
                get { return this._FEC_BAJA_SERVICIO; }
                set { this._FEC_BAJA_SERVICIO = value; }
            }
            public string COD_TARIFA
            {
                get { return this._COD_TARIFA; }
                set { this._COD_TARIFA = value; }
            }
            public string T1
            {
                get { return this._T1; }
                set { this._T1 = value; }
            }
            public string T2
            {
                get { return this._T2; }
                set { this._T2 = value; }
            }
            public string T5
            {
                get { return this._T5; }
                set { this._T5 = value; }
            }
            public string REVISION
            {
                get { return this._REVISION; }
                set { this._REVISION = value; }
            }
            public Boolean? FACTURADO_PROVEEDOR
            {
                get { return this._FACTURADO_PROVEEDOR; }
                set { this._FACTURADO_PROVEEDOR = value; }
            }
            public Boolean BAJA_SOLICITADA
            {
                get { return this._BAJA_SOLICITADA; }
                set { this._BAJA_SOLICITADA = value; }
            }
            public DateTime? FEC_RECLAMACION
            {
                get { return this._FEC_RECLAMACION; }
                set { this._FEC_RECLAMACION = value; }
            }
            public Boolean? RECLAMACION
            {
                get { return this._RECLAMACION; }
                set { this._RECLAMACION = value; }
            }
            public string PROVEEDOR
            {
                get { return this._PROVEEDOR; }
                set { this._PROVEEDOR = value; }
            }
            public string PROVEEDOR_AVERIA
            {
                get { return this._PROVEEDORAVERIA; }
                set { this._PROVEEDORAVERIA = value; }
            }
            public string PROVEEDOR_INSPECCION
            {
                get { return this._PROVEEDORINSPECCION; }
                set { this._PROVEEDORINSPECCION = value; }
            }

            //TEL:
            //public string NUM_TELEFONO1
            //{
            //    get { return this._NUM_TELEFONO1; }
            //    set { this._NUM_TELEFONO1 = value; }
            //}
            //public string NUM_TELEFONO2
            //{
            //    get { return this._NUM_TELEFONO2; }
            //    set { this._NUM_TELEFONO2 = value; }
            //}
            //public string NUM_TELEFONO3
            //{
            //    get { return this._NUM_TELEFONO3; }
            //    set { this._NUM_TELEFONO3 = value; }
            //}
            //public string NUM_TELEFONO4
            //{
            //    get { return this._NUM_TELEFONO4; }
            //    set { this._NUM_TELEFONO4 = value; }
            //}
            //public string NUM_TELEFONO5
            //{
            //    get { return this._NUM_TELEFONO5; }
            //    set { this._NUM_TELEFONO5 = value; }
            //}

            public string NUM_MOVIL
            {
                get { return this._NUM_MOVIL; }
                set { this._NUM_MOVIL = value; }
            }
        
            public Int16? COD_ULTIMA_VISITA
            {
                get { return this._COD_ULTIMA_VISITA; }
                set { this._COD_ULTIMA_VISITA = value; }
            }

            public String COD_RECEPTOR
            {
                get { return this._COD_RECEPTOR; }
                set { this._COD_RECEPTOR = value; }
            }

            public Double PAGAR_SI_BAJA
            {
                get { return this._PAGAR_SI_BAJA; }
                set { this._PAGAR_SI_BAJA = value; }
            }

            //#2095 Pago Anticipado
            public String SOLCENT
            {
                get { return this._SOLCENT; }
                set { this._SOLCENT = value; }
            }
        
            public String COD_CLIENTE
            {
                get { return this._COD_CLIENTE; }
                set { this._COD_CLIENTE = value; }
            }

            public Decimal CONTADOR_AVERIAS
            {
                get { return this._CONTADOR_AVERIAS; }
                set { this._CONTADOR_AVERIAS = value; }
            }

            public String HORARIO_CONTACTO
            {
                get { return this._HORARIO_CONTACTO; }
                set { this._HORARIO_CONTACTO = value; }
            }

            //20191111 BGN ADD BEG [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
            public String EMAIL
            {
                get { return this._EMAIL; }
                set { this._EMAIL = value; }
            }
            //20191111 BGN ADD END [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email

            public String SOCIEDAD
            {
                get { return this._SOCIEDAD; }
                set { this._SOCIEDAD = value; }
            }
        #endregion

        #region Constructores
        public MantenimientoDTO()
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