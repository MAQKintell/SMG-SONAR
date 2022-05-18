using System;
using Iberdrola.Commons.Utils;

/// <summary>
/// Descripción breve de Enumerados
/// </summary>
public class EnumeradosWS
{
  
    /// <summary>
    /// Tiene todos los estados en los que puede estar una solicitud
    /// El String Value tiene el mismo valor que la tabla en la base de datos
    /// </summary>
    public enum EstadosSolicitud : int
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
[StringValue("001")]	Pendientedecontactar=1,
[StringValue("002")]	Pendientedeconcretarcita=2,
[StringValue("004")]	Pendientedeacudiracliente=4,
[StringValue("005")]	PresupuestopendientedeCliente=5,
[StringValue("006")]	Pendientederepararsincitaconcertada=6,
[StringValue("007")]	Pendientederepararconcitaconcertada=7,
[StringValue("010")]	Reparada=10,
[StringValue("011")]	Reparadaportelefono=11,
[StringValue("012")]	Cancelada=12,
[StringValue("014")]	VisitaRealizada=14,
[StringValue("015")]	Pendienteconcretarcitacliente=15,
[StringValue("016")]	Pdtdeacudirporaplazoausenciadelcliente=16,
[StringValue("017")]	Pendienteconcretarcitacliente17=17,
[StringValue("018")]	BajaServicio=18,
[StringValue("019")]	Pdtdeacudirporaplazoausenciadelcliente19=19,
[StringValue("020")]	Cerradassinaceptarpresupuesto=20,
[StringValue("021")]	ReparadaconDocumentacion=21,
[StringValue("022")]	CanceladaporReasignacion=22,
[StringValue("023")]	CanceladotransferidoaSAT=23,
[StringValue("024")]	TransferidoaSAT=24,
[StringValue("025")]	Cancelada25=25,
[StringValue("026")]	Visitarealizada=26,
[StringValue("027")]	ProveedortransferidoaSAT=27,
[StringValue("028")]	Canceladapresupuestonovalido=28,
[StringValue("029")]	ProveedorTransferidoaSATfinalizada  =29,
[StringValue("030")]	Trabajoinvalido=30,
[StringValue("031")]	PTEPieza=31,
[StringValue("032")]	SolucionadoProveedortransferidoaSAT=32,
[StringValue("033")]	ClienteVisitado=33,
[StringValue("034")]	PdteScoring=34,
[StringValue("035")]	PdteOferta=35,
[StringValue("036")]	Ofertarechazada=36,
[StringValue("037")]	FinanciaciónrechazadaporIB=37,
[StringValue("038")]	OfertaaceptadaPdteinstalación=38,
[StringValue("039")]	Pdtematerial=39,
[StringValue("040")]	PdteSAT=40,
[StringValue("041")]	Pdteaceptaciónfintrabajos=41,
[StringValue("042")]	Incidenciaeninstalación=42,
[StringValue("043")]	Conformefinalcliente=43,
[StringValue("044")]	Ofertapresentadapdtedecliente=44,
[StringValue("045")]	OfertaGasConfort=45,
[StringValue("046")]	PdteRespuesta=46,
[StringValue("047")]	Cerrada47=47,
[StringValue("048")]	Pdtecierre=48,
[StringValue("049")]	Malcerradaaveríasinreparar=49,
[StringValue("050")]	Malcerradanoaceptópresupuesto=50,
[StringValue("051")]	Malcerradaalcontactarfuncionabacorrectamente=51,
[StringValue("052")]	Persisteavería=52,
[StringValue("053")]	Reparadaconindicacionesalcliente=53,
[StringValue("054")]	Conformeconreparación=54,
[StringValue("055")]	Clienteilocalizable=55,
[StringValue("056")]	Cancelada56=56,
[StringValue("057")]	InstalacionRealizada=57,
[StringValue("058")]	PdteConformidadCliente=58,
[StringValue("059")]	OfertaaceptadaPdtecontrato=59,
[StringValue("060")]	OfertaAceptada=60,
[StringValue("061")]	OfertaAceptadaPdteDepago=61,
[StringValue("062")]	JustificanteRecibido=62,
[StringValue("063")]	InstalaciónAutorizada=63,
[StringValue("064")]	ImportePagadoIncorrecto=64,
[StringValue("065")]	Pendiente65=65
    };


}
