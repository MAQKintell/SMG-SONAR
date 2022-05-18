using System;
using Iberdrola.Commons.DataAccess;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Exceptions;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Iberdrola.Commons.Utils;


namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de Consultas
    /// </summary>
    public class Consultas : BaseBLL
    {
        public Consultas()
        {

        }
        public static IDataReader ConsultasVisitasaFacturarPorPagina(TextBox Fecha, String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaVisitasaFacturarPorPagina(Fecha, Proveedor,Desde,Hasta);
        }
        public static IDataReader ObtenerDatosDesdeSentencia(String SQL)
        {
            ConsultasDB consultaDB = new ConsultasDB();
            return consultaDB.ObtenerDatosDesdeSentencia(SQL);
        }
        
        public static IDataReader ConsultasVisitasaFacturar(TextBox Fecha, String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaVisitasaFacturar(Fecha, Proveedor);
        }
        public static IDataReader ConsultasReparacionesaFacturar(TextBox Fecha, String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaReparacionesaFacturar(Fecha, Proveedor);
        }
        public static IDataReader ConsultasReparacionesaFacturarPorPagina(TextBox Fecha, String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaReparacionesaFacturarPorPagina(Fecha, Proveedor,Desde,Hasta);
        }

        public static IDataReader ObtenerPermisoFacturacionPorUsuario(String UserID)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerPermisoFacturacionPorUsuario(UserID);
        }
        public static IDataReader ObtenerSolicitudesBonificables(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesBonificables(Proveedor);
        }
        public static IDataReader ObtenerSolicitudesBonificablesBusqueda(String NumFactura,String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesBonificablesBusqueda(NumFactura,Proveedor);
        }

        public static IDataReader ObtenerSolicitudesSupervisionBusqueda(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesSupervisionBusqueda(Proveedor);
        }

        public static IDataReader ObtenerSolicitudesSupervisionBusquedaPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesSupervisionBusquedaPorPagina(Proveedor, Desde, Hasta);
        }

        public static IDataReader ObtenerVisitasModificadasPorcambioTelefono(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasModificadasPorcambioTelefono(Proveedor);
        }

        public static IDataReader ObtenerVisitasModificadasPorcambioTelefonoPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasModificadasPorcambioTelefonoPorPagina(Proveedor, Desde, Hasta);
        }

        public static IDataReader ObtenerSolicitudesSupervisionAPagar(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesSupervisionAPagar(Proveedor);
        }

        public static IDataReader ObtenerSolicitudesSupervisionAPagarPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesSupervisionAPagarPorPagina(Proveedor, Desde, Hasta);
        }



        public static IDataReader ObtenerVisitasPenalizablesBonificables(String Proveedor,int Bonificable)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasPenalizablesBonificables(Proveedor, Bonificable);
        }
        public static IDataReader ObtenerVisitasPenalizablesBonificablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta, int Bonificable)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasPenalizablesBonificablesPorPagina(Proveedor,Desde,Hasta,Bonificable);
        }

        public static IDataReader ObtenerVisitasAPenalizaBonificar(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasAPenalizaBonificar(Proveedor);
        }
        public static IDataReader ObtenerVisitasAPenalizaBonificarPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasAPenalizaBonificarPorPagina(Proveedor, Desde, Hasta);
        }






        public static IDataReader ObtenerVisitasINCORRECTASPenalizablesBonificables(String Proveedor, int Bonificable)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasINCORRECTASPenalizablesBonificables(Proveedor, Bonificable);
        }
        public static IDataReader ObtenerVisitasINCORRECTASPenalizablesBonificablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta, int Bonificable)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasINCORRECTASPenalizablesBonificablesPorPagina(Proveedor, Desde, Hasta, Bonificable);
        }

        public static IDataReader ObtenerVisitasINCORRECTASAPenalizaBonificar(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasINCORRECTASAPenalizaBonificar(Proveedor);
        }
        public static IDataReader ObtenerVisitasINCORRECTASAPenalizaBonificarPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerVisitasINCORRECTASAPenalizaBonificarPorPagina(Proveedor, Desde, Hasta);
        }



        

        public static IDataReader ObtenerSolicitudesBonificablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesBonificablesPorPagina(Proveedor, Desde, Hasta);
        }
        public static IDataReader ObtenerSolicitudesBonificablesPorPaginaBusqueda(String NumFactura,String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesBonificablesPorPaginaBusqueda(NumFactura,Proveedor, Desde, Hasta);
        }
        public static IDataReader ObtenerSolicitudesNOBonificablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesNOBonificablesPorPagina(Proveedor, Desde, Hasta);
        }
        public static IDataReader ObtenerSolicitudesNOBonificablesPorPaginaBusqueda(String NumFactura, String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesNOBonificablesPorPaginaBusqueda(NumFactura, Proveedor, Desde, Hasta);
        }

        public static Int64 ActualizarSolicitudesBonificables(String Proveedor, String NumFactura)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ActualizarSolicitudesBonificables(Proveedor,NumFactura);
        }

        public static Int64 ActualizarVisitasAPenalizarBonificar(int IdSolicitud, Boolean Bonificable)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ActualizarVisitasAPenalizarBonificar(IdSolicitud, Bonificable);
        }

        public static Int64 ActualizarNumeroFactura(String Proveedor, String NumeroFactura,Int16 idIdioma)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ActualizarNumeroFactura(Proveedor, NumeroFactura, idIdioma);
        }

        public static Int64 ActualizarSolicitudesNoBonificables(String Proveedor, String NumFactura)
        {
            //RCC
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ActualizarSolicitudesNoBonificables(Proveedor, NumFactura);
        }

        public static Int64 InsertarAccesoPantalla(String Usuario, String URL)
        {
            //RCC
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.InsertarAccesoPantalla(Usuario, URL);
        }

        public static Int64 ActualizarSolicitudesSupervision(String Proveedor,Int16 idIdioma)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ActualizarSolicitudesSupervision(Proveedor, idIdioma);
        }

        public static IDataReader ObtenerSolicitudesNOBonificables(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesNOBonificables(Proveedor);
        }

        public static IDataReader ObtenerSolicitudesTecnicos(String Proveedor,String Nombre,String Apellido)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesTecnicos(Proveedor,Nombre,Apellido);
        }

        public static IDataReader ObtenerSolicitudesTecnicosPorPagina(String Proveedor, Int64 Desde, Int64 Hasta, String Nombre, String Apellido)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesTecnicosPorPagina(Proveedor,Desde,Hasta,Nombre,Apellido);
        }
        
        public static IDataReader ObtenerProveedoresAveriasVisitasPorPagina(String Provincia, String Poblacion, String CP, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerProveedoresAveriasVisitasPorPagina(Provincia, Poblacion, CP, Desde, Hasta);
        }

        public static IDataReader ObtenerProveedoresAveriasVisitas(String Provincia, String Poblacion, String CP)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerProveedoresAveriasVisitas(Provincia, Poblacion, CP);
        }

        public static IDataReader ObtenerSolicitudesRevisionPrecinteRealizadas(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesRevisionPrecinteRealizadas(Proveedor);
        }

        public static IDataReader ObtenerSolicitudesRevisionPrecintePenalizables(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesRevisionPrecintePenalizables(Proveedor);
        }

        public static IDataReader ObtenerSolicitudesSupervisionPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesSupervisionPorPagina(Proveedor, Desde, Hasta);
        }
        
        public static IDataReader ObtenerSolicitudesSupervision(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesSupervision(Proveedor);
        }

        public static IDataReader ObtenerSolicitudesRevisionPrecinteRealizadasPorPagina(String Proveedor,Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesRevisionPrecinteRealizadasPorPagina(Proveedor,Desde,Hasta);
        }

        public static IDataReader ObtenerSolicitudesRevisionPrecintePenalizablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesRevisionPrecintePenalizablesPorPagina(Proveedor, Desde, Hasta);
        }

        public static IDataReader ObtenerSolicitudesNOBonificablesBusqueda(String NumFactura,String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerSolicitudesNOBonificablesBusqueda(NumFactura, Proveedor);
        }
        

        public static IDataReader ConsultasVisitasVencidas(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaVisitasVencidas(Proveedor);
        }
        public static IDataReader ConsultasVisitasVencidasPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaVisitasVencidasPorPagina(Proveedor,Desde,Hasta);
        }
        
        public static IDataReader ConsultasVisitasPenalizables(String Proveedor)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaVisitasPenalizables (Proveedor);
        }

        public static IDataReader ConsultasVisitasPenalizablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaVisitasPenalizablesPorPagina(Proveedor,Desde,Hasta);
        }


        public static IDataReader ConsultasContratosBaja(VistaContratoCompletoDTO filtro,TextBox txt)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultasContratosBaja(filtro,txt);
        }
        public static IDataReader ConsultasContratosBajaPORFECHAS(VistaContratoCompletoDTO filtro, TextBox txtDesde, TextBox TxtHasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultasContratosBajaPORFECHAS(filtro, txtDesde,TxtHasta);
        }

        public static IDataReader ConsultasContratosBajaCONVISITAPORFECHAS(VistaContratoCompletoDTO filtro, TextBox txtDesde, TextBox TxtHasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultasContratosBajaCONVISITASPORFECHAS(filtro, txtDesde,TxtHasta);
        }

        public static IDataReader VisitasConPRL(TextBox txtDesde, TextBox TxtHasta)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.VisitasConPRL(txtDesde, TxtHasta);
        }
        
        public static IDataReader ConsultasMantenimientoProvincia(VistaContratoCompletoDTO filtro)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultasMantenimientoProvincia(filtro);
        }
        public static IDataReader ConsultaCambioTelefonos(VistaContratoCompletoDTO filtro)
        {
             ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaCambioTelefonos(filtro);
        }
        public static IDataReader ConsultaContratosSinTelefonos(VistaContratoCompletoDTO filtro)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaContratosSinTelefonos(filtro);
        }
        public static IDataReader ConsultaVencidosPorMes(VistaContratoCompletoDTO filtro)
        {
             ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerConsultaVencidosPorMes(filtro);
        }
        public static IDataReader ObtenerConsultaFacturadasProveedor(VistaContratoCompletoDTO filtro)
        {
             ConsultasDB vistaDB = new ConsultasDB();
             return vistaDB.ObtenerConsultaFacturadasProveedor(filtro);
        }

        public static string ObtenerCaracteristicaPorDescripcion(string descripcion,Int16 idIdioma)
        {
            ConsultasDB vistaDB = new ConsultasDB();
            return vistaDB.ObtenerCaracteristicaPorDescripcion(descripcion, idIdioma);
        }

    }
}