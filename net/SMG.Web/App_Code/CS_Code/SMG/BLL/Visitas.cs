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
using Iberdrola.Commons.Utils;
using System.Collections.Generic;
using System.Security.Principal;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Security;
using System.Collections;
using System.Data.SqlClient;
using Iberdrola.Commons.Web;
using System.Transactions;
using Iberdrola.Commons.DataAccess;

/// <summary>
/// Descripción breve de Visitas
/// </summary>
public class Visitas
{

    private static List<String> _listaEstadosVisitaCancelada;
    private static List<String> _listaEstadosConsideradosVisitaCerrada;
    private static List<String> _listaEstadosConsideradosVisitaSinVisita;

    private static String _EstadoVisitaCerrada;

    /// <summary>
    /// Constructor estático, carga las listas de estaco cerradas y canceladas
    /// </summary>
    static Visitas()
    {
        // inicialización de la lista de estados cancelados
        _listaEstadosVisitaCancelada = new List<string>();
        _listaEstadosVisitaCancelada.Add(StringEnum.GetStringValue(Enumerados.EstadosVisita.CanceladaAusentePorSegundaVez));
        _listaEstadosVisitaCancelada.Add(StringEnum.GetStringValue(Enumerados.EstadosVisita.CanceladaClienteNoDeseaVisita));
        _listaEstadosVisitaCancelada.Add(StringEnum.GetStringValue(Enumerados.EstadosVisita.CanceladaClienteNoLocalizable));
        _listaEstadosVisitaCancelada.Add(StringEnum.GetStringValue(Enumerados.EstadosVisita.CanceladaQuiereDarseDeBaja));

        // inicialización de la lista de estados cerrados
        _listaEstadosConsideradosVisitaCerrada = new List<string>();
        _listaEstadosConsideradosVisitaCerrada.Add(StringEnum.GetStringValue(Enumerados.EstadosVisita.Cerrada));
        _listaEstadosConsideradosVisitaCerrada.Add(StringEnum.GetStringValue(Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion));
        _listaEstadosConsideradosVisitaCerrada.Add(StringEnum.GetStringValue(Enumerados.EstadosVisita.CerradaPuestaEnMarcha));


        _EstadoVisitaCerrada = (StringEnum.GetStringValue(Enumerados.EstadosVisita.Cerrada));

        //inicialización de la lista de estados sin visita
        //_listaEstadosConsideradosVisitaSinVisita.Add(); 

    }

    public static Boolean EsEstadoVisitaConsideradaCerrada(Enumerados.EstadosVisita estado)
    {
        String strEstado = StringEnum.GetStringValue(estado);
        return (Visitas._listaEstadosConsideradosVisitaCerrada.Contains(strEstado));
    }

    public static Boolean EsEstadoVisitaConsideradaCerrada(String strEstado)
    {
        return (Visitas._listaEstadosConsideradosVisitaCerrada.Contains(strEstado));
    }

    public static Boolean EsEstadoVisitaCerrada(String strEstado)
    {
        return Visitas._EstadoVisitaCerrada.Equals(strEstado);
    }

    public static Boolean EsEstadoVisitaCancelada(String strEstado)
    {
        return (Visitas._listaEstadosVisitaCancelada.Contains(strEstado));
    }

    public IDataReader ObtenerTipoUrgencia(string CodUrgencia,int idIdioma)
    {
         VisitasDB visitaDb = new VisitasDB();

         IDataReader visitaDR = visitaDb.ObtenerTipoUrgencia(CodUrgencia, idIdioma);
         return visitaDR;
    }
    public IDataReader ObtenerDatosReparacion(Decimal IdReparacion)
    {
        VisitasDB visitaDb = new VisitasDB();

        IDataReader visitaDR = visitaDb.ObtenerDatosReparacion(IdReparacion);
        return visitaDR;
    }
    public IDataReader ObtenerDescLote(string CodLote)
    {
        VisitasDB visitaDb = new VisitasDB();

        IDataReader visitaDR = visitaDb.ObtenerDescLote(CodLote);
        return visitaDR;
    }

    public DataTable ObtenerHistoricoVisitaPorIdVisita(string Contrato, string Visita, Int16 idIdioma)
    {
        VisitasDB visitaDb = new VisitasDB();

       return visitaDb.ObtenerHistoricoVisitaPorIdVisita(Contrato,Visita,idIdioma);
       
    }

    public IDataReader ObtenerHistoricoVisita(string Contrato,Int16 idIdioma)
    {
        VisitasDB visitaDb = new VisitasDB();

        IDataReader visitaDR = visitaDb.ObtenerHistoricoVisita(Contrato, idIdioma);
        return visitaDR;
    }

    public Int32 ObtenerCartasEnviar(Boolean Enviada)
    {
        VisitasDB visitaDb = new VisitasDB();

        return Int32.Parse(visitaDb.ObtenerCartasEnviar(Enviada).ToString());
    }

    public IDataReader  ObtenerContador_Visitas_Dia_por_Tecnico(Int32 nIDTecnico,DateTime fechaVisita)
    {
        VisitasDB visitaDb = new VisitasDB();

        return visitaDb.ObtenerContador_Visitas_Dia_por_Tecnico(nIDTecnico, fechaVisita);
    }



    public IDataReader ObtenerCartasEnviarDatos(Boolean Enviada,Int32 Desde, Int32 Hasta)
    {
        VisitasDB visitaDb = new VisitasDB();

        return visitaDb.ObtenerCartasEnviarDatos(Enviada,Desde,Hasta);
    }

    public IDataReader ObtenerVisitasVistaTelefono(String Contrato, String CodigoProveedor, String CodSubtipo)
    {
        VisitasDB visitaDb = new VisitasDB();

        return visitaDb.ObtenerVisitasVistaTelefono(Contrato, CodigoProveedor, CodSubtipo);
    }

    public DataTable ObtenerCartasEnviarDatosENDATATABLE(Boolean Enviada, Int32 Desde, Int32 Hasta)
    {
        VisitasDB visitaDb = new VisitasDB();

        return visitaDb.ObtenerCartasEnviarDatosENDATATABLE(Enviada, Desde, Hasta);
    }
    public VisitaDTO DatosVisitas(string CodContrato, string CodVisita)
	{
        try
        {
            VisitasDB visitaDb = new VisitasDB();
            VisitaDTO visita = new VisitaDTO();

            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDT = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            String Proveedor=usuarioDT.NombreProveedor.ToString();
            
            IDataReader visitaDR = visitaDb.ObtenerDatosVisitas(CodContrato, CodVisita,Proveedor,(Int16)usuarioDT.IdIdioma);

            if (visitaDR != null)
            {
                while (visitaDR.Read())
                {
                    visita.CodigoContrato = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_CONTRATO");
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "DESC_CATEGORIA_VISITA") != null)
                    {
                        visita.CategoriaVisita = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "DESC_CATEGORIA_VISITA");
                    }
                    visita.CodigoVisita = (Int16?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_VISITA");
                    visita.CodigoEstadoVisita = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_ESTADO_VISITA");
                    visita.Aviso = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "VISITA_AVISO");
                    //visita.Campania = (int)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CAMPANIA");

                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CAMPANIA") != null)
                    {
                        visita.Campania = (int)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CAMPANIA");
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FEC_VISITA") == null)
                    {
                        visita.FechaVisita = null;
                    }
                    else
                    {
                        visita.FechaVisita = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FEC_VISITA");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FEC_LIMITE_VISITA") == null)
                    {
                        visita.FechaLimiteVisita = null;
                    }
                    else
                    {
                        visita.FechaLimiteVisita = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FEC_LIMITE_VISITA");
                    }
                    
                    visita.TipoUrgencia = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "ID_TIPO_URGENCIA");
                    visita.IdReparacion = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "ID_REPARACION");
                    visita.Observaciones  = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "OBSERVACIONES");
                    visita.IdLote = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "ID_LOTE");
                    //visita.CUPS = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CUPS");

                    visita.COD_RECEPTOR = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_RECEPTOR");

                    visita.RecepcionComprobante = (Boolean?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "RECEPCION_COMPROBANTE");
                    visita.FacturadoProveedor = (Boolean?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FACTURADO_PROVEEDOR");
                    
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "INFORMACION_RECIBIDA") != null)
                    {
                        visita.InformacionRecibida = (Boolean)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "INFORMACION_RECIBIDA");
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CONTADOR_INTERNO") != null)
                    {
                        visita.ContadorInterno = (Boolean)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CONTADOR_INTERNO");
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "TECNICO") == null)
                    {
                        visita.Tecnico = 0;
                    }
                    else
                    {
                        if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "TECNICO").ToString() != "0")
                        {
                            visita.Tecnico = Int16.Parse(DataBaseUtils.GetDataReaderColumnValue(visitaDR, "TECNICO").ToString());
                        }
                        else
                        {
                            visita.Tecnico = 0;
                        }
                    }
                    visita.ObservacionesVisita = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "OBSERVACIONESVISITA");
                    visita.TipoVisita = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "TIPOVISITA");
                    visita.CodigoInterno = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CODIGO_INTERNO_VISITA");// ,V.CODIGO_INTERNO_VISITA
                    visita.NumFactura  = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "NUM_FACTURA");
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_FACTURA") == null)
                    {
                        visita.FechaFactura = null;
                    }
                    else
                    {
                        visita.FechaFactura = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_FACTURA");
                    }
                    
                    
                    visita.CartaEnviada = (Boolean?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "ENVIADA_CARTA");
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_ENVIO_CARTA") == null)
                    {
                        visita.FechaEnviadoCarta = null;// (DateTime)("01/01/1900 00:00");
                    }
                    else
                    {
                        visita.FechaEnviadoCarta = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_ENVIO_CARTA");
                    }
                    
                    visita.CodigoBarras  = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_BARRAS");
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_GENERACION_BARRAS") == null)
                    {
                        visita.FechaGeneracionCodigoBarras = null;
                    }
                    else
                    {
                        visita.FechaGeneracionCodigoBarras = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_GENERACION_BARRAS");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_INTRODUCCION_COD_BARRAS") == null)
                    {
                        visita.FechaIntroduccionCodigoBarras = null;
                    }
                    else
                    {
                        visita.FechaIntroduccionCodigoBarras = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_INTRODUCCION_COD_BARRAS");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_DE_BONIFICACION") == null)
                    {
                        visita.FechaBonificacion = null;
                    }
                    else
                    {
                        visita.FechaBonificacion = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_DE_BONIFICACION");
                    }
                    //....
                    //BGN ADD INI R#25266: Visualizar si una visita tiene PRL por Web
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "PRL") != null)
                    {
                        visita.PRL = (Boolean)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "PRL");
                    }
                    //BGN ADD FIN R#25266: Visualizar si una visita tiene PRL por Web
                }


                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_VISITA, visita);
                return visita;
                
            }
            else
            {
                throw new BLLException("6000"); // password incorrecta
            }
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "");
        }
    }
      
    public VisitaDTO DatosVisitasWS(string CodContrato, string CodVisita,string proveedor)
    {
        IDataReader visitaDR = null;

        try
        {
            VisitasDB visitaDb = new VisitasDB();
            VisitaDTO visita = new VisitaDTO();
            //AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            //UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            String Proveedor = proveedor;

            visitaDR = visitaDb.ObtenerDatosVisitas(CodContrato, CodVisita, Proveedor, 1);//(Int16)usuarioDTO.IdIdioma);

            if (visitaDR != null)
            {
                while (visitaDR.Read())
                {
                    visita.CodigoContrato = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_CONTRATO");
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "DESC_CATEGORIA_VISITA") != null)
                    {
                        visita.CategoriaVisita = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "DESC_CATEGORIA_VISITA");
                    }
                    visita.CodigoVisita = (Int16?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_VISITA");
                    visita.CodigoEstadoVisita = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_ESTADO_VISITA");
                    visita.Aviso = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "VISITA_AVISO");
                    //visita.Campania = (int)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CAMPANIA");

                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CAMPANIA") != null)
                    {
                        visita.Campania = (int)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CAMPANIA");
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FEC_VISITA") == null)
                    {
                        visita.FechaVisita = null;
                    }
                    else
                    {
                        visita.FechaVisita = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FEC_VISITA");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FEC_LIMITE_VISITA") == null)
                    {
                        visita.FechaLimiteVisita = null;
                    }
                    else
                    {
                        visita.FechaLimiteVisita = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FEC_LIMITE_VISITA");
                    }

                    visita.TipoUrgencia = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "ID_TIPO_URGENCIA");
                    visita.IdReparacion = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "ID_REPARACION");
                    visita.Observaciones = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "OBSERVACIONES");
                    visita.IdLote = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "ID_LOTE");
                    //visita.CUPS = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CUPS");

                    visita.COD_RECEPTOR = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_RECEPTOR");

                    visita.RecepcionComprobante = (Boolean?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "RECEPCION_COMPROBANTE");
                    visita.FacturadoProveedor = (Boolean?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FACTURADO_PROVEEDOR");

                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "INFORMACION_RECIBIDA") != null)
                    {
                        visita.InformacionRecibida = (Boolean)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "INFORMACION_RECIBIDA");
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CONTADOR_INTERNO") != null)
                    {
                        visita.ContadorInterno = (Boolean)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CONTADOR_INTERNO");
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "TECNICO") == null)
                    {
                        visita.Tecnico = 0;
                    }
                    else
                    {
                        if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "TECNICO").ToString() != "0")
                        {
                            visita.Tecnico = Int16.Parse(DataBaseUtils.GetDataReaderColumnValue(visitaDR, "TECNICO").ToString());
                        }
                        else
                        {
                            visita.Tecnico = 0;
                        }
                    }
                    visita.ObservacionesVisita = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "OBSERVACIONESVISITA");
                    visita.TipoVisita = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "TIPOVISITA");
                    visita.CodigoInterno = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "CODIGO_INTERNO_VISITA");// ,V.CODIGO_INTERNO_VISITA
                    visita.NumFactura = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "NUM_FACTURA");
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_FACTURA") == null)
                    {
                        visita.FechaFactura = null;
                    }
                    else
                    {
                        visita.FechaFactura = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_FACTURA");
                    }


                    visita.CartaEnviada = (Boolean?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "ENVIADA_CARTA");
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_ENVIO_CARTA") == null)
                    {
                        visita.FechaEnviadoCarta = null;// (DateTime)("01/01/1900 00:00");
                    }
                    else
                    {
                        visita.FechaEnviadoCarta = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_ENVIO_CARTA");
                    }

                    visita.CodigoBarras = (String)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "COD_BARRAS");
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_GENERACION_BARRAS") == null)
                    {
                        visita.FechaGeneracionCodigoBarras = null;
                    }
                    else
                    {
                        visita.FechaGeneracionCodigoBarras = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_GENERACION_BARRAS");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_INTRODUCCION_COD_BARRAS") == null)
                    {
                        visita.FechaIntroduccionCodigoBarras = null;
                    }
                    else
                    {
                        visita.FechaIntroduccionCodigoBarras = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_INTRODUCCION_COD_BARRAS");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_DE_BONIFICACION") == null)
                    {
                        visita.FechaBonificacion = null;
                    }
                    else
                    {
                        visita.FechaBonificacion = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(visitaDR, "FECHA_DE_BONIFICACION");
                    }
                    //....
                }

                return visita;
            }
            else
            {
                throw new BLLException("6000"); // password incorrecta
            }
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "");
        }
        finally
        {
            if (visitaDR != null)
                visitaDR.Close();
        }
    }
    public static void ActualizarDatosVisita(VisitaDTO visitaDTO, bool SaveHistory,String Usuario)
    {
        VisitasDB visitasDB = new VisitasDB();
        using (TransactionScope transactionScope = new TransactionScope())
        {
            visitasDB.ActualizarDatosVisita(visitaDTO, Usuario);
            if (SaveHistory)
            {
                VisitaHistoricoDB visitaHistoricoDB = new VisitaHistoricoDB();
                VisitaHistoricoDTO visitaHistoricoDTO = new VisitaHistoricoDTO(visitaDTO.CodigoContrato,
                    visitaDTO.CodigoVisita.Value, null, visitaDTO.CodigoEstadoVisita);
                visitaHistoricoDB.SalvarHistorico(visitaHistoricoDTO,Usuario);
            }
            transactionScope.Complete();
        }
    }

    public DataTable dtDatosVisitas(string CodContrato, string CodVisita, string proveedor)
    {
        IDataReader visitaDR = null;
        DataTable dtVisita = new DataTable();

        try
        {
            VisitasDB visitaDb = new VisitasDB();

            visitaDR = visitaDb.ObtenerDatosVisitas(CodContrato, CodVisita, proveedor, 1);

            if (visitaDR != null)
                dtVisita.Load(visitaDR);

            return dtVisita;
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "");
        }
        finally
        {
            if (visitaDR != null)
                visitaDR.Close();
        }
    }

    public static void ActualizarDatosVisitaYVisitaHistoricoWS(VisitaDTO visitaDTO, String Usuario)
    {
        VisitasDB visitasDB = new VisitasDB();
        
        visitasDB.ActualizarDatosVisita(visitaDTO, Usuario);
       
    }

    public static void ActualizarFechaPrevistaVisitaWS(string fechaPrevistaVisita, string horaDesdePrevistaVisita, string horaHastaPrevistaVisita, string codContrato, int codVisita)
    {
        VisitasDB visitasDB = new VisitasDB();

        visitasDB.ActualizarFechaPrevistaVisitaWS(fechaPrevistaVisita, horaDesdePrevistaVisita, horaHastaPrevistaVisita, codContrato, codVisita);

    }
    public static IDataReader ObtenerFechasMovimiento(String Contrato, int Visita)
    {
        VisitaHistoricoDB visitaHistoricoDB = new VisitaHistoricoDB();
        return visitaHistoricoDB.ObtenerFechasMovimiento (Contrato,Visita);
    }

    public static string ObtenerProveedorUltimoMovimiento(String Contrato, int Visita)
    {
        VisitaHistoricoDB visitaHistoricoDB = new VisitaHistoricoDB();
        return visitaHistoricoDB.ObtenerProveedorUltimoMovimiento(Contrato, Visita);
    }

    public static IDataReader ObtenerFechasMovimientoParaCerradas(String Contrato, int Visita)
    {
        VisitaHistoricoDB visitaHistoricoDB = new VisitaHistoricoDB();
        return visitaHistoricoDB.ObtenerFechasMovimientoParaCerrada(Contrato, Visita);
    }

    public static IDataReader ObtenerFechasMovimientoDeCanceladaPorSistema(String Contrato, int Visita)
    {
        VisitaHistoricoDB visitaHistoricoDB = new VisitaHistoricoDB();
        return visitaHistoricoDB.ObtenerFechasMovimientoDeCanceladaPorSistema(Contrato, Visita);
    }
    public static IDataReader ObtenerFechasMovimientoDeClienteAusente(String Contrato, int Visita)
    {
        VisitaHistoricoDB visitaHistoricoDB = new VisitaHistoricoDB();
        return visitaHistoricoDB.ObtenerFechasMovimientoDeClienteAusente(Contrato, Visita);
    }
    
    public static List<VisitaDTO>ObtenerListaVisitasDesdeDataReader(IDataReader dr)
    {
        List<VisitaDTO> ListaVisitaDTO = new List<VisitaDTO>();

        if (dr != null)
        {
            while (dr.Read())
            {
                VisitaDTO visitaDTO = new VisitaDTO();
                visitaDTO.CodigoContrato = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_CONTRATO_SIC");
                visitaDTO.CodigoVisita = (Int16?)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_VISITA");
                visitaDTO.CodigoEstadoVisita = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_ESTADO");
                ListaVisitaDTO.Add(visitaDTO);
            }
        }
        return ListaVisitaDTO;
    }

    public static Boolean ComprobarTecnicoExistente(int idtecnico,string proveedor)
    {
        VisitasDB visitasDB = new VisitasDB();
        return visitasDB.ComprobarTecnicoExistente(idtecnico, proveedor);
    }

    public static int ObtenerContadorVisitasTecnicoPorDia(Int32 nIdTecnico, DateTime dFecVisita)
    {
        VisitasDB visitasDB = new VisitasDB();
        return visitasDB.ObtenerContadorVisitasTecnicoPorDia(nIdTecnico, dFecVisita);
    }

    public static DateTime? ObtenerFechaPasoACerrada(string codContrato, string codCOntratoActual, int codVisita)
    {
        VisitasDB visitasDB = new VisitasDB();
        return visitasDB.ObtenerFechaPasoACerrada(codContrato, codCOntratoActual,codVisita);
    }

    public static DateTime? ObtenerFechaPasoACanceladaPorSistema(string codContrato, string codCOntratoActual, int codVisita)
    {
        VisitasDB visitasDB = new VisitasDB();
        return visitasDB.ObtenerFechaPasoACanceladaPorSistema(codContrato, codCOntratoActual, codVisita);
    }
    //****************
    //public static IDataReader MostrarVisitasGeneraLote()
    //{
    //    VisitasDB visitasDB = new VisitasDB();
    //    return visitasDB.MostrarVisitasGeneraLoteDB();
    //}



    //public string CerrarVisita(VisitaDTO visita)
    //{ 

    //    string Contrato = visita.CodigoContrato;
    //    string ID = visita.CodigoVisita.Value.ToString();
    //    //MantenimientoDTO mantenimiento = Mantenimiento.

    //    //VISITAS.
    //    string SQL = "SELECT COD_CONTRATO,COD_VISITA,CAMPANIA,FEC_VISITA,FEC_LIMITE_VISITA,ID_TIPO_URGENCIA,COD_ESTADO_VISITA,V.ID_REPARACION,OBSERVACIONES,ID_LOTE,RECEPCION_COMPROBANTE,FACTURADO_PROVEEDOR,NUM_FACTURA,FECHA_FACTURA,ENVIADA_CARTA,FECHA_ENVIO_CARTA,COD_BARRAS,FECHA_GENERACION_BARRAS,FECHA_INTRODUCCION_COD_BARRAS,FECHA_DE_BONIFICACION,TECNICO,OBSERVACIONESVISITA,TIPOVISITA,v.INFORMACION_RECIBIDA,R.NUMERO_FACTURA_REPARACION FROM VISITA V LEFT JOIN REPARACION R ON R.ID_REPARACION=V.ID_REPARACION where cod_contrato='" + Contrato + "' and cod_visita='" + ID + "'";
    //    SQL = "SELECT COD_CONTRATO,COD_VISITA,CAMPANIA,FEC_VISITA,v.FEC_LIMITE_VISITA,ID_TIPO_URGENCIA,COD_ESTADO_VISITA,V.ID_REPARACION,v.OBSERVACIONES,ID_LOTE,RECEPCION_COMPROBANTE,v.FACTURADO_PROVEEDOR,v.NUM_FACTURA,FECHA_FACTURA,ENVIADA_CARTA,FECHA_ENVIO_CARTA,COD_BARRAS,FECHA_GENERACION_BARRAS,FECHA_INTRODUCCION_COD_BARRAS,FECHA_DE_BONIFICACION,TECNICO,OBSERVACIONESVISITA,TIPOVISITA,v.INFORMACION_RECIBIDA,R.NUMERO_FACTURA_REPARACION FROM relacion_cups_contrato rcc inner join mantenimiento m on m.cod_receptor=rcc.cod_receptor inner join VISITA V  on m.cod_contrato_sic=v.cod_contrato LEFT JOIN REPARACION R ON R.ID_REPARACION=V.ID_REPARACION where rcc.cod_contrato_sic='" + Contrato + "' and cod_visita='" + ID + "'";
        
    //    DataTable DatosVisita = DataBaseUtils.DevolverQuery(SQL);

    //    if (DatosVisita != null && DatosVisita.Rows.Count > 0)
    //    {
    //        String COD_CONTRATO_ACTUAL = DatosVisita.Rows[0].ItemArray[0].ToString();
    //        String COD_VISITA_ACTUAL = DatosVisita.Rows[0].ItemArray[1].ToString();
    //        String CAMPANIA_ACTUAL = DatosVisita.Rows[0].ItemArray[2].ToString();
            
    //        Boolean Correcto = true;
    //        // intentar evitar tema mismo cod_receptor diferente contrato.
    //        Contrato = COD_CONTRATO_ACTUAL;
    //        try
    //        {
    //            Estado = FuncionesSolicitudesVisitas.ObtnerCodEstadoPorNombreEstado(Estado).ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            return "ESTADO VISITA INEXISTENTE";
    //        }

    //        DateTime FEC_LIMITE_VISITA_ACTUAL = DateTime.Now;
    //        if (DatosVisita.Rows[0].ItemArray[4].ToString() != "")
    //        {
    //            FEC_LIMITE_VISITA_ACTUAL = DateTime.Parse(DatosVisita.Rows[0].ItemArray[4].ToString());
    //        }
    //        Int64 ID_TIPO_URGENCIA_ACTUAL = Int64.Parse(DatosVisita.Rows[0].ItemArray[5].ToString());
    //        String COD_ESTADO_VISITA_ACTUAL = DatosVisita.Rows[0].ItemArray[6].ToString();
    //        Int64 ID_REPARACION_ACTUAL = 0;
    //        if (!string.IsNullOrEmpty(DatosVisita.Rows[0].ItemArray[7].ToString()))
    //        {
    //            ID_REPARACION_ACTUAL = Int64.Parse(DatosVisita.Rows[0].ItemArray[7].ToString());
    //        }
    //        String OBSERVACIONES_ACTUAL = DatosVisita.Rows[0].ItemArray[8].ToString();
    //        Int64 ID_LOTE_ACTUAL = 0;
    //        if (!string.IsNullOrEmpty(DatosVisita.Rows[0].ItemArray[9].ToString()))
    //        {
    //            ID_LOTE_ACTUAL = Int64.Parse(DatosVisita.Rows[0].ItemArray[9].ToString());
    //        }

    //        Boolean RECEPCION_COMPROBANTE_ACTUAL = Boolean.Parse(DatosVisita.Rows[0].ItemArray[10].ToString());
    //        Boolean FACTURADO_PROVEEDOR_ACTUAL = Boolean.Parse(DatosVisita.Rows[0].ItemArray[11].ToString());
    //        String NUM_FACTURA_ACTUAL = DatosVisita.Rows[0].ItemArray[12].ToString();
    //        DateTime FECHA_FACTURA_ACTUAL;
    //        if (DatosVisita.Rows[0].ItemArray[13].ToString() != "")
    //        {
    //            FECHA_FACTURA_ACTUAL = DateTime.Parse(DatosVisita.Rows[0].ItemArray[13].ToString());
    //        }
    //        Boolean ENVIADA_CARTA_ACTUAL = Boolean.Parse(DatosVisita.Rows[0].ItemArray[14].ToString());
    //        DateTime FECHA_ENVIO_CARTA_ACTUAL;
    //        if (DatosVisita.Rows[0].ItemArray[15].ToString() != "")
    //        {
    //            FECHA_ENVIO_CARTA_ACTUAL = DateTime.Parse(DatosVisita.Rows[0].ItemArray[15].ToString());
    //        }
    //        String COD_BARRAS_ACTUAL = DatosVisita.Rows[0].ItemArray[16].ToString();
    //        DateTime FECHA_GENERACION_BARRAS_ACTUAL;
    //        if (DatosVisita.Rows[0].ItemArray[17].ToString() != "")
    //        {
    //            FECHA_GENERACION_BARRAS_ACTUAL = DateTime.Parse(DatosVisita.Rows[0].ItemArray[17].ToString());
    //        }
    //        DateTime FECHA_INTRODUCCION_COD_BARRAS_ACTUAL;
    //        if (DatosVisita.Rows[0].ItemArray[18].ToString() != "")
    //        {
    //            FECHA_INTRODUCCION_COD_BARRAS_ACTUAL = DateTime.Parse(DatosVisita.Rows[0].ItemArray[18].ToString());
    //        }
    //        DateTime FECHA_DE_BONIFICACION_ACTUAL;
    //        if (DatosVisita.Rows[0].ItemArray[19].ToString() != "")
    //        {
    //            FECHA_DE_BONIFICACION_ACTUAL = DateTime.Parse(DatosVisita.Rows[0].ItemArray[19].ToString());
    //        }
    //        String TECNICO_ACTUAL = DatosVisita.Rows[0].ItemArray[20].ToString();
    //        String OBSERVACIONESVISITA_ACTUAL = DatosVisita.Rows[0].ItemArray[21].ToString();
    //        String TIPOVISITA_ACTUAL = DatosVisita.Rows[0].ItemArray[22].ToString();
    //        Boolean INFORMACION_RECIBIDA_ACTUAL = false;// Boolean.Parse(DatosVisita.Rows[0].ItemArray[23].ToString());
    //        if (!string.IsNullOrEmpty(DatosVisita.Rows[0].ItemArray[23].ToString()))
    //        {
    //            INFORMACION_RECIBIDA_ACTUAL = Boolean.Parse(DatosVisita.Rows[0].ItemArray[23].ToString());
    //        }

    //        String NUM_FACTURA_REPARACION_ACTUAL = "";
    //        if (DatosVisita.Rows[0].ItemArray[24].ToString() != "")
    //        {
    //            NUM_FACTURA_REPARACION_ACTUAL = DatosVisita.Rows[0].ItemArray[24].ToString();
    //        }

    //        //*********************************************************

    //        SQL = "SELECT top (1) fecha FROM relacion_cups_contrato rcc inner join mantenimiento m on m.cod_receptor=rcc.cod_receptor inner join VISITA_HISTORICO vh on m.cod_contrato_sic=vh.cod_contrato WHERE rcc.COD_CONTRATO_sic in ('" + Contrato + "','" + COD_CONTRATO_ACTUAL + "') AND COD_VISITA='" + ID + "' ORDER BY FECHA DESC";
    //        DataTable DatosHistoricoVisita = DataBaseUtils.DevolverQuery(SQL);

    //        DateTime FechaUltimaModificacion = DateTime.Now;
    //        try
    //        {
    //            FechaUltimaModificacion = DateTime.Parse(DatosHistoricoVisita.Rows[0].ItemArray[0].ToString());
    //        }
    //        catch (Exception ex)
    //        {
    //            return "NO TIENE MOVIMIENTO DE HISTORICO PARA ESA VISITA";
    //        }

    //        DateTime FechaMovimiento = FechaUltimaModificacion.AddDays(2); //FechaUltimaModificacion.AddDays(2); //FechaUltimaModificacion.AddDays(1);//DateTime.Now;
    //        //*********************************************************

    //        Boolean PermitirReparacion = true;
    //        Boolean SaltarVisita = false;


    //        //Comprobamos que el técnico no ha metido mas de 15 visitas al día

    //        //llamada a sp

    //        Int32 nContadorVisDiaTecnico = 0;

    //        int codTecnico = FuncionesSolicitudesVisitas.ObtenerIdTecnicoEmpresaPorNombre(Tecnico, ProveedorAProcesar);
    //        if (codTecnico != 0)
    //        {
    //            if (FechaVisita != null && FechaVisita != "")
    //            {
    //                IDataReader drContVisitas = FuncionesSolicitudesVisitas.ObtenerContador_Visitas_Dia_por_Tecnico(Int32.Parse(codTecnico.ToString()), DateTime.Parse(FechaVisita));

    //                while (drContVisitas.Read())
    //                {
    //                    nContadorVisDiaTecnico = Int32.Parse(DataBaseUtils.GetDataReaderColumnValue(drContVisitas, "CONTADOR_VISITA_DIA").ToString());
    //                }
    //                if (nContadorVisDiaTecnico > 15)
    //                {
    //                   return "ESTE TÉCNICO HA ALCANZADO EL LÍMITE DE 15 VISITAS/DÍA";
    //                }
    //            }
    //        }

    //        //*********************************************************
    //        //Comprobamos los telefonos (9 posiciones).
    //        if (TelefonoContacto1.Length > 0 && TelefonoContacto1.Length != 9 && TelefonoContacto1 != "0")
    //        {
    //            EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";EL TELEFONO DE CONTACTO 1 NO TIENE 9 POSICIONES", nombreFicheroLog);
    //            Correcto = false;
    //        }

    //        if (int.Parse(Estado) == (int)FuncionesPublicas.EstadosVisita.Cerrada)
    //        {
    //            //Comprobamos que el código de barras sea númerico y que tenga 13 dígitos

    //            if (!CodigoBarras1.Equals(""))
    //            {
    //                try
    //                {
    //                    Int64 result = Int64.Parse(CodigoBarras1.ToString());

    //                }
    //                catch
    //                {
    //                    return "EL CODIGO DE BARRAS DEBE SER NUMÉRICO";
    //                }


    //                if (!CodigoBarras1.Length.Equals(13) && !CodigoBarras1.Length.Equals(14))
    //                {
    //                    return "EL CODIGO DE BARRAS DEBE TENER 13 o 14 DÍGITOS";
    //                }
    //            }
    //            //Comprobamos que el código de barras de reparaciones sea númerico y que tenga 14 dígitos

    //            if (!CodigoBarrasReparacion.Equals(""))
    //            {
    //                try
    //                {
    //                    Int64 result = Int64.Parse(CodigoBarrasReparacion.ToString());
    //                }
    //                catch
    //                {
    //                    return "EL CODIGO DE BARRAS DE REPARACIÓN DEBE SER NUMÉRICO";                        
    //                }


    //                if (!CodigoBarrasReparacion.Length.Equals(13) && !CodigoBarrasReparacion.Length.Equals(14))
    //                {
    //                    return "EL CODIGO DE BARRAS DE REPARACIÓN DEBE TENER 13 o 14 DÍGITOS";
    //                }
    //            }
    //        }
            
    //        //*********************************************************
    //        //Calculamos la fecha de factura.
    //        string MES = DateTime.Now.AddMonths(1).Month.ToString();
    //        if (MES.Length == 1) { MES = "0" + MES; }
    //        String Anhio1 = DateTime.Now.Year.ToString();
    //        if (MES == "01") { Anhio1 = DateTime.Now.AddYears(1).Year.ToString(); }
    //        FechaFactura = "01/" + MES + "/" + Anhio1;

    //        FechaFacturaReparacion = FechaFactura;// "01/" + MES + "/" + Anhio2;
    //        //*********************************************************
    //        //Inicializamos los numeros de factura con los actuales.
    //        NumeroFatura = NUM_FACTURA_ACTUAL;
    //        NumeroFacturaReparacion = NUM_FACTURA_REPARACION_ACTUAL;
    //        //*********************************************************
    //        //Comprobar si la visita k nos viene es la última
    //        if (ID == COD_VISITA_ACTUAL)
    //        {
    //            if (int.Parse(COD_ESTADO_VISITA_ACTUAL) != (int)FuncionesPublicas.EstadosVisita.Cerrada)
    //            {
    //                //Tiene que tener Técnico y tipo de visita.
    //                if ((Tecnico != "" && TipoVisita != "" && Tecnico != " ") || Estado == "6")
    //                {
    //                    if (FEC_LIMITE_VISITA_ACTUAL >= DateTime.Now.AddDays(-1))
    //                    {
    //                        //********************************************
    //                        //No se debe permitir cambiar a cerrada (o cerrada pendiente de reparación) o guardar los cambios si la visita esta en alguno de los dos estados y esta sin rellenar la recepción del comprobante, el check de facturado proveedor, la fecha de factura y codigo de barras. Y la caldera debe de estar seleccionada.
    //                        if (int.Parse(Estado) == (int)FuncionesPublicas.EstadosVisita.Cerrada ||
    //                            int.Parse(Estado) == (int)FuncionesPublicas.EstadosVisita.CerradaPendienteRealizarReparacion)
    //                        {
    //                            if (RecepcionComprobante != "" && FacturadoProveedor != "" && FechaFactura != "" && CodigoBarras1 != "")//&& TipoCaldera != "")
    //                            {
    //                                SQL = "";
    //                            }
    //                            else
    //                            {
    //                                //Faltan datos obligatorios si es cerrada o cerradapendientedereparacion.
    //                                return "FALTAN DATOS OLBIGATORIOS PARA CERRADA O CERRADA PENDIENTE DE REPARACION (COMPRUEBE RECPCIONCOMPROBANTE,FACTURADOPROVEEDOR,FECHAFACTURA,CODIGOBARRAS Y TIPOCALDERA)";                                    
    //                            }
    //                        }
    //                        //********************************************
    //                        //Si es 2ª vez que pone cancelada por cliente ausente obligamos a meter: fecha visita, recepcion del comprobante fecha factura visita y codigo de barras de la visita.
    //                        if (int.Parse(Estado) == (int)FuncionesPublicas.EstadosVisita.CanceladaAusentePorSegundaVez)
    //                        {
    //                            if (FechaVisita != "" && RecepcionComprobante != "" && FechaFactura != "" && CodigoBarras1 != "")
    //                            {
    //                                SQL = "";
    //                            }
    //                            else
    //                            {
    //                                //Faltan datos obligatorios para ausente por segunda vez.
    //                                return "FALTAN DATOS OBLIGATORIOS PARA AUSENTE POR SEGUNDA VEZ (COMPRUEBE FECHAVISITA,RECEPCIONCOMPROBANTE,FECHAFACTURA Y CODIGOBARRAS)";
    //                            }
    //                        }
    //                        //********************************************
    //                        //Si esta en cerrada. No dejamos meter reparacion si han pasado + de 3 dias desde el cambio a cerrada.
    //                        if (int.Parse(Estado) == (int)FuncionesPublicas.EstadosVisita.Cerrada)
    //                        {
    //                            if (FechaMovimiento < FechaUltimaModificacion.AddDays(3))
    //                            {
    //                                SQL = "";
    //                            }
    //                            else
    //                            {
    //                                //No dejamos meter reparación si han pasado mas de 3 dias desde el cambio a cerrada.
    //                                return "No dejamos meter reparación si han pasado mas de 3 dias desde el cambio a cerrada";                                    
    //                            }
    //                        }
    //                        //********************************************
    //                        //No dejamos meter reparación si la visita esta en estado pendiente de reparacion.
    //                        if (int.Parse(Estado) == (int)FuncionesPublicas.EstadosVisita.CerradaPendienteRealizarReparacion)
    //                        {
    //                            //No dejamos meter reparación si la visita esta en estado pendiente de reparacion.
    //                            PermitirReparacion = false;
    //                        }
    //                        //********************************************
    //                        //Si el estado actual es cerrada pendiente de reparación solo se debe permitir modificar a cerrada si no han pasado más de 60 días desde el cambio de estado a cerrada pendiente de reparación.
    //                        //Si estado actual de la visita es cerrada pendiente de reparación y tiene num factura informado deshabilitamos todos los campos de la visita excepto el estado. Que se podrá pasar a cerrada según la validación anterior. No debemos permitir pasarlo a ningún otro estado.
    //                        if (int.Parse(COD_ESTADO_VISITA_ACTUAL) == (int)FuncionesPublicas.EstadosVisita.CerradaPendienteRealizarReparacion)
    //                        {
    //                            if (int.Parse(Estado) == (int)FuncionesPublicas.EstadosVisita.Cerrada)
    //                            {
    //                                if (FechaMovimiento < FechaUltimaModificacion.AddDays(60))
    //                                {
    //                                    if (NumeroFatura != "")
    //                                    {
    //                                        //Deshabilitamos todos los campos de la visita excepto el estado.
    //                                        //Hacemos aki la update porke solo dejamos actualizar el estado y no los demás campos.
    //                                        SQL = "Update visita set cod_estado_visita = '" + Estado + "' where cod_contrato='" + Contrato + "' and cod_visita='" + ID + "'";
    //                                        FuncionesSolicitudesVisitas.EjecutarQuery(SQL);
    //                                        //Insertamos el Historico de la visita.
    //                                        DateTime FechaLimiteVisita = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
    //                                        MacroDB macroDB = new MacroDB();
    //                                        macroDB.InsertarHistoricoVisita(int.Parse(ID_LOTE_ACTUAL.ToString()), Contrato, int.Parse(ID), FechaLimiteVisita, Estado, "BATCH" + ProveedorAProcesar.Substring(0, 3));

    //                                        SaltarVisita = true;
    //                                    }
    //                                    else
    //                                    {
    //                                        //No deshabilitamos.....
    //                                        SQL = "";
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    //Han pasado mas de 60 dias desde el cambio a Cerrada Pendiente de Realizar Reparacion.
    //                                    return "HAN PASADO MAS DE 60 DIAS DESDE EL CAMBIO A CERRADA PENDIENTE DE REALIZAR REPARACION";
    //                                }
    //                            }
    //                            else
    //                            {
    //                                //De este estado solo se puede pasar a cerrada.
    //                                return "EN ESTE CASO NO SE PERMITE OTRO ESTADO QUE NO SEA CERRADA (CerradaPendienteRealizarReparacion)";                                    
    //                            }
    //                        }
    //                        //********************************************
    //                        //Si esta en cancelada por sistema. No dejamos cambiar el estado mas que a cerrada si no han pasado 15 dias desde el cambio.
    //                        if (int.Parse(COD_ESTADO_VISITA_ACTUAL) == (int)FuncionesPublicas.EstadosVisita.SistemaCanceladaPorSistema)
    //                        {
    //                            if (int.Parse(Estado) == (int)FuncionesPublicas.EstadosVisita.Cerrada)
    //                            {
    //                                if (FechaMovimiento < FechaUltimaModificacion.AddDays(15))
    //                                {
    //                                    SQL = "";
    //                                }
    //                                else
    //                                {
    //                                    //Han pasado mas de 15 dias desde el cambio a Cancelada por sistema.
    //                                    return "HAN PASADO MAS DE 15 DIAS DESDE EL CAMBIO A CANCELADA POR SISTEMA";
    //                                }
    //                            }
    //                            else
    //                            {
    //                                //De este estado solo se puede pasar a cerrada.
    //                                return "EN ESTE CASO NO SE PERMITE OTRO ESTADO QUE NO SEA CERRADA (SistemaCanceladaPorSistema)";
    //                            }
    //                        }
                           
    //                        //********************************************
    //                        //PROCESOS "FINALES"
    //                        Boolean CorrectoTecnico = true;
    //                        if (Correcto)
    //                        {
    //                            TodoCorrecto = true;
    //                            //Si no ha pasado por el update a pelo anterior del estado.
    //                            if (SaltarVisita == false)
    //                            {
    //                                //Actualizamos la visita.
    //                                Int16 idTecnico = 0;
    //                                // Comentado el 17/09 porque SIEL dice k es mejor que nos manden el id del técnico que el nombre.
    //                                if (Tecnico != " " && Tecnico != "" && Tecnico != "0")
    //                                {
    //                                    idTecnico = FuncionesSolicitudesVisitas.ObtenerIdTecnicoEmpresaPorNombre(Tecnico, ProveedorAProcesar);
    //                                    if (idTecnico == 0)
    //                                    {
    //                                        EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";TÉCNICO INEXISTENTE " + Tecnico, nombreFicheroLog);
    //                                        CorrectoTecnico = false;
    //                                        TodoCorrecto = false;
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    if (Estado != "6" && Estado != "5" && Estado != "7")
    //                                    {
    //                                        EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";TÉCNICO INEXISTENTE " + Tecnico, nombreFicheroLog);
    //                                        CorrectoTecnico = false;
    //                                        TodoCorrecto = false;
    //                                    }
    //                                }
    //                                // Esta línea es para el caso de que nos venga el Id en vez del nombre.
    //                                //idTecnico = Int16.Parse(Tecnico);
    //                                if (CorrectoTecnico)
    //                                {
    //                                    try
    //                                    {
    //                                        RecepcionComprobante = RecepcionComprobante.Substring(0, 1);
    //                                        RecepcionComprobante = RecepcionComprobante.Replace("0", "false");
    //                                        RecepcionComprobante = RecepcionComprobante.Replace("1", "true");
    //                                        FacturadoProveedor = FacturadoProveedor.Replace("0", "false");
    //                                        FacturadoProveedor = FacturadoProveedor.Replace("1", "true");
    //                                        ContadorInterior = ContadorInterior.Replace("0", "false");
    //                                        ContadorInterior = ContadorInterior.Replace("1", "true");
    //                                        ContadorInterior = ContadorInterior.Replace("NO", "false");
    //                                        ContadorInterior = ContadorInterior.Replace("SI", "true");
    //                                        if (ContadorInterior == "") { ContadorInterior = "true"; }
    //                                        //Ellos no puede marcar o desmarcar la carta enviada...
    //                                        //CartaEnviada = CartaEnviada.Replace("0", "false");
    //                                        //CartaEnviada = CartaEnviada.Replace("1", "true");
    //                                        String Usuario = "BATCH" + ProveedorAProcesar.Substring(0, 3);

    //                                        FuncionesSolicitudesVisitas.ActualizarDatosVisita(Contrato, ID, FechaVisita, Estado, Observaciones, Boolean.Parse(RecepcionComprobante), Boolean.Parse(FacturadoProveedor), NumeroFatura, FechaFactura, ENVIADA_CARTA_ACTUAL, FechaEnvioCarta, CodigoBarras1, idTecnico, ObservacionesTecnico, TipoVisita, Usuario, Boolean.Parse(ContadorInterior), CodigoInterno);
    //                                    }
    //                                    catch (Exception ex)
    //                                    {
    //                                        EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";DATOS VISITA INCORRECTOS", nombreFicheroLog);
    //                                    }
    //                                }
    //                            }
    //                            //Actualizamos los telefonos de mantenimiento.
    //                            FuncionesSolicitudesVisitas.ActualizarTelefonosMantenimiento(Contrato, TelefonoContacto1, telefonoContacto2);

    //                            //Metemos caldera.
    //                            if (TipoCaldera == "")
    //                            {
    //                                try
    //                                {
    //                                    //Tenemos k borrar la caldera porque viene vacia en el txt, lo que significa o bien que la han quitado o que no la han informado nunca.
    //                                    FuncionesSolicitudesVisitas.EliminarCaldera(Contrato);
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";DATOS CALDERA INCORRECTOS", nombreFicheroLog);
    //                                    PermitirReparacion = false;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                try
    //                                {
    //                                    //Actualizamos o insertamos la caldera, lo que proceda... (en el procedimiento).
    //                                    if (Anhio == "") { Anhio = "0"; }
    //                                    FuncionesSolicitudesVisitas.ActualizarInsertarCaldera(Contrato, Decimal.Parse(TipoCaldera), Decimal.Parse(MarcaCaldera), ModeloCaldera, Decimal.Parse(Uso), Potencia, int.Parse(Anhio), DescripcionMarcaCaldera);
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";DATOS CALDERA INCORRECTOS", nombreFicheroLog);
    //                                    PermitirReparacion = false;
    //                                    TodoCorrecto = false;
    //                                }
    //                            }

    //                          //Insertamos Reparación.
    //                            if (PermitirReparacion)
    //                            {
    //                                try
    //                                {
    //                                    Reparacion = Reparacion.Replace("0", "false");
    //                                    Reparacion = Reparacion.Replace("1", "true");
    //                                    if (Reparacion == "" || Boolean.Parse(Reparacion) == false)
    //                                    {
    //                                        //Si no viene reparación borramos la existente.
    //                                        FuncionesSolicitudesVisitas.BorrarReparacion(Contrato, Int16.Parse(ID), int.Parse(ID_REPARACION_ACTUAL.ToString()));
    //                                    }
    //                                    else
    //                                    {
    //                                        //Hay reparación nueva.
    //                                        if (ID_REPARACION_ACTUAL != 0)
    //                                        {
    //                                            SQL = "";
    //                                            //Si hay reparación anterior.
    //                                            FuncionesSolicitudesVisitas.ActualizarReparacion(int.Parse(ID_REPARACION_ACTUAL.ToString()), int.Parse(TipoReparacion), DateTime.Parse(FechaReaparacion), int.Parse(TiempoManoObra), Double.Parse(CosteMateriales), Double.Parse(ImporteManoObra), Double.Parse(CosteMaterialesCliente), DateTime.Parse(FechaFacturaReparacion), NumeroFacturaReparacion, CodigoBarrasReparacion);
    //                                        }
    //                                        else
    //                                        {
    //                                            SQL = "";
    //                                            //Si no hay reparación anterior.
    //                                            FuncionesSolicitudesVisitas.InsertarReparacion(Contrato, Int16.Parse(ID), DateTime.Parse(FechaReaparacion), int.Parse(TipoReparacion), int.Parse(TiempoManoObra), Double.Parse(CosteMateriales), Double.Parse(ImporteManoObra), Double.Parse(CosteMaterialesCliente), DateTime.Parse(FechaFacturaReparacion), NumeroFacturaReparacion, CodigoBarrasReparacion);
    //                                        }
    //                                    }
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";Error en los datos de la reparacion.", nombreFicheroLog);
    //                                }
    //                            }
    //                        }
    //                        //********************************************
    //                    }
    //                    else
    //                    {
    //                        // Due date superior to today.
    //                        //Falta o el técnico o el tipo de visita (Campos obligatorios).
    //                        EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";NO SE PERMITE EL CAMBIO AL HABERSE SUPERADO LA FECHA LÍMITE DE LA VISITA", nombreFicheroLog);
    //                        Correcto = false;
    //                    }
    //                }
    //                else
    //                {
    //                    //Falta o el técnico o el tipo de visita (Campos obligatorios).
    //                    EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";FALTA EL TÉCNICO O EL TIPO DE LA VISITA", nombreFicheroLog);
    //                    Correcto = false;
    //                }
    //            }
    //            else
    //            {
    //                //VISITA JADANIK ITZITA.
    //                EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";VISITA ACTUALMENTE YA CERRADA", nombreFicheroLog);
    //                Correcto = false;
    //            }
    //        }
    //        else
    //        {
    //            //No es la última visita para este contrato.
    //            EscribirLOGSolicitudesVisitasAutomatico(Contrato + ";EL CODIGO DE LA VISITA NO COINCIDE CON LA ULTIMA VISITA DEL CONTRATO, NO SE PERMITE LA MODIFICACION", nombreFicheroLog);
    //            Correcto = false;
    //        }
    //    }
    //}

    //20191119 BGN ADD BEG R#20616: Procedimientos cancelados no localizables
    public Int32 ObtenerNumCartasEnviadas(DateTime fechaDesde, DateTime fechaHasta)
    {
        VisitasDB visitaDb = new VisitasDB();
        return visitaDb.ObtenerNumCartasEnviadas(fechaDesde, fechaHasta);
    }

    public IDataReader ObtenerCartasEnviadas(DateTime fechaDesde, DateTime fechaHasta, Int32 Desde, Int32 Hasta)
    {
        VisitasDB visitaDb = new VisitasDB();
        return visitaDb.ObtenerCartasEnviadas(fechaDesde, fechaHasta, Desde, Hasta);
    }

    public DataTable ObtenerCartasEnviadasEnDataTable(DateTime fechaDesde, DateTime fechaHasta, Int32 Desde, Int32 Hasta)
    {
        VisitasDB visitaDb = new VisitasDB();
        return visitaDb.ObtenerCartasEnviadasEnDataTable(fechaDesde, fechaHasta, Desde, Hasta);
    }
    //20191119 BGN ADD END R#20616: Procedimientos cancelados no localizables
}
