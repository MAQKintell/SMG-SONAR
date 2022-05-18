using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.Serialization;

namespace Iberdrola.SMG.DAL.DTO
{
    /// <summary>
    /// Descripción breve de ContratoDTO
    /// </summary>
    public class VistaContratoCompletoDTO : BaseDTO
    {
        #region atributos

        private bool _FILTROS_AVANZADOS_ACTIVOS;
        private String _PROVEEDOR_ENTRADA;
        private String _COD_CONTRATO_SIC;
        private DateTime? _FEC_VISITA;
        private DateTime? _FEC_LIMITE_VISITA;
        private String _DESC_TIPO_URGENCIA;
        private String _DES_ESTADO;
        private String _T1;
        private String _T2;
        private String _T5;
        private String _DESC_MARCA_CALDERA;
        private String _NOM_TITULAR;
        private String _APELLIDO1;
        private String _APELLIDO2;
        private String _COD_PROVINCIA;
        private String _COD_ESTADOVISITA;
        private String _NOMBRE;
        private String _COD_POBLACION;
        private String _Expr2;
        private String _TIP_VIA_PUBLICA;
        private String _NOM_CALLE;
        private String _COD_FINCA;
        private String _TIP_BIS;
        private String _COD_PORTAL;
        private String _TIP_ESCALERA;
        private String _TIP_PISO;
        private String _TIP_MANO;
        private String _COD_POSTAL;
        private String _NUM_TEL_CLIENTE;
        private String _NUM_TEL_PS;
        private String _COD_TARIFA;
        private String _Expr1;
        private Int32? _CAMPANIA;
        private String _OBSERVACIONES;
        private Decimal? _ID_LOTE;
        private String _DESC_LOTE;
        private String _PROVEEDOR;
        private DateTime? _FEC_ALTA_CONTRATO;
        private DateTime? _FEC_BAJA_CONTRATO;
        private DateTime? _FEC_ALTA_SERVICIO;
        private DateTime? _FEC_BAJA_SERVICIO;
        private Boolean? _FACTURADO_PROVEEDOR;
        private String _NUM_FACTURA;
        private Boolean? _ENVIADA_CARTA;
        private DateTime? _FECHA_ENVIO_CARTA;
        private String _NUM_TELEFONO1;
        private String _NUM_TELEFONO2;
        private String _NUM_TELEFONO3;
        private String _NUM_TELEFONO4;
        private String _NUM_TELEFONO5;
        private String _NUM_TELEFONO6;
        //atributos para el filtrado de columnas
        private Hashtable _LISTAS_FILTROS_COLUMNAS;
        private List<int> _LISTA_NUM_COLUMNA_FILTRADA;
        //auxiliar que guardara el index del combobox de la ventana modal de filtros avanzados
        private string _AUXGUARDAINDEX;

        private Boolean? _RECLAMACION;
        private DateTime? _FEC_RECLAMACION;


        //campos para el filtrado de fechas
        private DateTime? _FEC_MAX_ALTA_SERVICIO;
        private DateTime? _FEC_MIN_ALTA_SERVICIO;
        private DateTime? _FEC_MAX_LOTE;
        private DateTime? _FEC_MIN_LOTE;
        private DateTime? _FEC_MAX_LIMITE_VISITA;
        private DateTime? _FEC_MIN_LIMITE_VISITA;



        #endregion atributos

        #region propiedades
        public Boolean? Reclamacion
        {
            get { return this._RECLAMACION; }
            set { this._RECLAMACION = value; }
        }
        public DateTime? FechaReclamacion
        {
            get { return this._FEC_RECLAMACION; }
            set { this._FEC_RECLAMACION = value; }
        }
        /// <summary>
        /// Propiedad para obtener o modificar si los filtros estan activos
        /// </summary>
        public bool FiltrosAvanzadosActivos
        {
            get { return this._FILTROS_AVANZADOS_ACTIVOS; }
            set { this._FILTROS_AVANZADOS_ACTIVOS = value; }
        }

        public String ProveedorEntrada
        {
            get { return this._PROVEEDOR_ENTRADA; }
            set { this._PROVEEDOR_ENTRADA = value; }
        }
        public String CodigoContrato
        {
            get { return this._COD_CONTRATO_SIC; }
            set { this._COD_CONTRATO_SIC = value; }
        }
        public DateTime? FechaVisita
        {
            get { return this._FEC_VISITA; }
            set { this._FEC_VISITA = value; }
        }
        public DateTime? FechaLimiteVisita
        {
            get { return this._FEC_LIMITE_VISITA; }
            set { this._FEC_LIMITE_VISITA = value; }
        }
        public String DescripcionTipoUrgencia
        {
            get { return this._DESC_TIPO_URGENCIA; }
            set { this._DESC_TIPO_URGENCIA = value; }
        }
        public String DescripcionEstado
        {
            get { return this._DES_ESTADO; }
            set { this._DES_ESTADO = value; }
        }
        public String T1
        {
            get { return this._T1; }
            set { this._T1 = value; }
        }
        public String T2
        {
            get { return this._T2; }
            set { this._T2 = value; }
        }
        public String T5
        {
            get { return this._T5; }
            set { this._T5 = value; }
        }
        public String DescripcionMarcaCaldera
        {
            get { return this._DESC_MARCA_CALDERA; }
            set { this._DESC_MARCA_CALDERA = value; }
        }
        public String NombreTitular
        {
            get { return this._NOM_TITULAR; }
            set { this._NOM_TITULAR = value; }
        }
        public String Apellido1
        {
            get { return this._APELLIDO1; }
            set { this._APELLIDO1 = value; }
        }
        public String Apellido2
        {
            get { return this._APELLIDO2; }
            set { this._APELLIDO2 = value; }

        }
        public String CodigoDeProvincia
        {
            get { return this._COD_PROVINCIA; }
            set { this._COD_PROVINCIA = value; }
        }
        public String CodigoDeEstadoVisita
        {
            get { return this._COD_ESTADOVISITA; }
            set { this._COD_ESTADOVISITA = value; }
        }
        public String Nombre
        {
            get { return this._NOMBRE; }
            set { this._NOMBRE = value; }
        }
        public String CodigoDePoblacion
        {
            get { return this._COD_POBLACION; }
            set { this._COD_POBLACION = value; }
        }
        public String Expr2
        {
            get { return this._Expr2; }
            set { this._Expr2 = value; }

        }
        public String TipoViaPublica
        {
            get { return this._TIP_VIA_PUBLICA; }
            set { this._TIP_VIA_PUBLICA = value; }
        }
        public String NombreCalle
        {
            get { return this._NOM_CALLE; }
            set { this._NOM_CALLE = value; }
        }
        public String CodigoFinca
        {
            get { return this._COD_FINCA; }
            set { this._COD_FINCA = value; }
        }
        public String TipoBis
        {
            get { return this._TIP_BIS; }
            set { this._TIP_BIS = value; }
        }
        public String CodigoDelPortal
        {
            get { return this._COD_PORTAL; }
            set { this._COD_PORTAL = value; }
        }
        public String TipoEscalera
        {
            get { return this._TIP_ESCALERA; }
            set { this._TIP_ESCALERA = value; }
        }
        public String TipoPiso
        {
            get { return this._TIP_PISO; }
            set { this._TIP_PISO = value; }
        }
        public String TipoMano
        {
            get { return this._TIP_MANO; }
            set { this._TIP_MANO = value; }
        }
        public String CodigoPostal
        {
            get { return this._COD_POSTAL; }
            set { this._COD_POSTAL = value; }
        }
        public String NumeroTelefonoCliente
        {
            get { return this._NUM_TEL_CLIENTE; }
            set { this._NUM_TEL_CLIENTE = value; }
        }
        public String NumeroTelefonoPS
        {
            get { return this._NUM_TEL_PS; }
            set { this._NUM_TEL_PS = value; }
        }
        public String CodigoTarifa
        {
            get { return this._COD_TARIFA; }
            set { this._COD_TARIFA = value; }
        }
        public String Expr1
        {
            get { return this._Expr1; }
            set { this._Expr1 = value; }
        }
        public Int32? Campania
        {
            get { return this._CAMPANIA; }
            set { this._CAMPANIA = value; }
        }
        public String Observaciones
        {
            get { return this._OBSERVACIONES; }
            set { this._OBSERVACIONES = value; }
        }
        public Decimal? IdLote
        {
            get { return this._ID_LOTE; }
            set { this._ID_LOTE = value; }
        }
        public String DescripcionLote
        {
            get { return this._DESC_LOTE; }
            set { this._DESC_LOTE = value; }
        }
        public String Proveedor
        {
            get { return this._PROVEEDOR; }
            set { this._PROVEEDOR = value; }
        }
        public DateTime? FechaAltaContrato
        {
            get { return this._FEC_ALTA_CONTRATO; }
            set { this._FEC_ALTA_CONTRATO = value; }
        }
        public DateTime? FechaBajaContrato
        {
            get { return this._FEC_BAJA_CONTRATO; }
            set { this._FEC_BAJA_CONTRATO = value; }
        }
        public DateTime? FechaAltaServicio
        {
            get { return this._FEC_ALTA_SERVICIO; }
            set { this._FEC_ALTA_SERVICIO = value; }
        }
        public DateTime? FechaBajaServicio
        {
            get { return this._FEC_BAJA_SERVICIO; }
            set { this._FEC_BAJA_SERVICIO = value; }
        }
        public Boolean? FacturadoPorProveedor
        {
            get { return this._FACTURADO_PROVEEDOR; }
            set { this._FACTURADO_PROVEEDOR = value; }
        }
        public String NumeroFactura
        {
            get { return this._NUM_FACTURA; }
            set { this._NUM_FACTURA = value; }
        }
        public Boolean? CartaEnviada
        {
            get { return this._ENVIADA_CARTA; }
            set { this._ENVIADA_CARTA = value; }
        }
        public DateTime? FechaDeEnvioCarta
        {
            get { return this._FECHA_ENVIO_CARTA; }
            set { this._FECHA_ENVIO_CARTA = value; }
        }
        public String NumeroTelefono1
        {
            get { return this._NUM_TELEFONO1; }
            set { this._NUM_TELEFONO1 = value; }
        }
        public String NumeroTelefono2
        {
            get { return this._NUM_TELEFONO2; }
            set { this._NUM_TELEFONO2 = value; }
        }
        public String NumeroTelefono3
        {
            get { return this._NUM_TELEFONO3; }
            set { this._NUM_TELEFONO3 = value; }
        }
        public String NumeroTelefono4
        {
            get { return this._NUM_TELEFONO4; }
            set { this._NUM_TELEFONO4 = value; }
        }
        public String NumeroTelefono5
        {
            get { return this._NUM_TELEFONO5; }
            set { this._NUM_TELEFONO5 = value; }
        }
        public String NumeroTelefono6
        {
            get { return this._NUM_TELEFONO6; }
            set { this._NUM_TELEFONO6 = value; }
        }
        public String AuxGuardaIndex
        {
            get { return this._AUXGUARDAINDEX; }
            set { this._AUXGUARDAINDEX = value; }
        }

        public DateTime? FechaMaximaAltaServicio
        {
            get { return this._FEC_MAX_ALTA_SERVICIO; }
            set { this._FEC_MAX_ALTA_SERVICIO = value; }
        }
        public DateTime? FechaMinimaAltaServicio
        {
            get { return this._FEC_MIN_ALTA_SERVICIO; }
            set { this._FEC_MIN_ALTA_SERVICIO = value; }
        }

        public DateTime? FechaMaximaLote
        {
            get { return this._FEC_MAX_LOTE; }
            set { this._FEC_MAX_LOTE = value; }
        }
        public DateTime? FechaMinimaLote
        {
            get { return this._FEC_MIN_LOTE; }
            set { this._FEC_MIN_LOTE = value; }
        }

        public DateTime? FechaMaximaLimiteVisita
        {
            get { return this._FEC_MAX_LIMITE_VISITA; }
            set { this._FEC_MAX_LIMITE_VISITA = value; }
        }
        public DateTime? FechaMinimaLimiteVisita
        {
            get { return this._FEC_MIN_LIMITE_VISITA; }
            set { this._FEC_MIN_LIMITE_VISITA = value; }
        }            
            

        /// <summary>
        /// Propiedad que devuelve la <see cref="Hashtable"/> con los filtros aplicados a las columnas
        /// </summary>
        public Hashtable ListasFiltrosColumna
        {
            get { return this._LISTAS_FILTROS_COLUMNAS; }
        }
        #endregion propiedades

        #region constructores
        public VistaContratoCompletoDTO()
        {
            this._LISTAS_FILTROS_COLUMNAS = new Hashtable();
            this._LISTA_NUM_COLUMNA_FILTRADA = new List<int>();
        }
        #endregion constructores

        #region metodos

        /// <summary>
        /// Aniade el numero de columna a la lista de columnas filtradas
        /// </summary>
        /// <param name="columna"></param>
        public void AniadirNumColumnaFiltro(int columna)
        {
            if (!_LISTA_NUM_COLUMNA_FILTRADA.Contains(columna))
            {
                this._LISTA_NUM_COLUMNA_FILTRADA.Add(columna);
            }
        }

        /// <summary>
        /// Elimina el numero de columna de la lista de columnas filtradas
        /// </summary>
        /// <param name="columna"></param>
        public void EliminarNumColumnaFiltro(int columna)
        {
            if (_LISTA_NUM_COLUMNA_FILTRADA.Contains(columna))
            {
                _LISTA_NUM_COLUMNA_FILTRADA.Remove(columna);
            }
        }

        /// <summary>
        /// Devuelve si esta o no filtrada la columna
        /// </summary>
        /// <param name="columna"></param>
        /// <returns></returns>
        public bool EstaFiltradaColumna(int columna)
        {
            return _LISTA_NUM_COLUMNA_FILTRADA.Contains(columna);
        }

        /// <summary>
        /// Aniade la lista con los filtros de la columna dada
        /// </summary>
        /// <param name="nombreColumna"></param>
        /// <param name="lista"></param>
        public void AniadirFiltroColumna(String nombreColumna, List<ObjetoTextoValorDTO> lista)
        {
            // Guarda la nueva lista, si ya existia una la sustituye
            if (this._LISTAS_FILTROS_COLUMNAS.ContainsKey(nombreColumna))
            {
                this._LISTAS_FILTROS_COLUMNAS[nombreColumna] = lista;
            }
            else
            {
                this._LISTAS_FILTROS_COLUMNAS.Add(nombreColumna, lista);
            }
        }

        /// <summary>
        /// Borra todos los filtros de columna existentenes
        /// </summary>
        public void BorrarFiltrosColumna()
        {
            this._LISTAS_FILTROS_COLUMNAS.Clear();
            this._LISTA_NUM_COLUMNA_FILTRADA.Clear();
        }

        #endregion metodos

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

        #endregion implementación de metodos heredados de BaseDTO
    }
}