﻿using System;
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
    /// Descripción breve de ConsultasDB
    /// </summary>
    public class ConsultasDB : BaseDB
    {
        public ConsultasDB()
        {

        }

        /// <summary>
        /// Obtenemos los datos para el informe de Beretta
        /// </summary>
        /// <returns></returns>
        public DataTable GetReport_WEBGasConfort(string PAIS, string PROVEEDOR)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            if (PROVEEDOR == "d")
            {
                string[] ParamsName = new string[1];
                DbType[] ParamsType = new DbType[1];
                object[] ParamsValue = new object[1];

                ParamsName[0] = "@PAIS";
                ParamsValue[0] = PAIS;
                ParamsType[0] = DbType.String;
            
                //return db.RunProcDataTable("MTOGASBD_Get_Report_GasConfort", ParamsName, ParamsType, ParamsValue);
            
                return db.RunProcDataTable("MACRO_Report_Averia_Averia_Incorrecta_GasConfort", ParamsName, ParamsType, ParamsValue);
            }
            else
            {
                string[] ParamsName = new string[2];
                DbType[] ParamsType = new DbType[2];
                object[] ParamsValue = new object[2];

                ParamsName[0] = "@PAIS";
                ParamsValue[0] = PAIS;
                ParamsType[0] = DbType.String;

                PROVEEDOR = PROVEEDOR.Substring(0, 3);
                ParamsName[1] = "@Proveedor";
                ParamsValue[1] = PROVEEDOR;
                ParamsType[1] = DbType.String;
                
                return db.RunProcDataTable("MACRO_Report_Averia_Averia_Incorrecta_GasConfort_desdeWeb", ParamsName, ParamsType, ParamsValue);
            }
        }



        private void ObtenerFiltroDeProveedor(VistaContratoCompletoDTO filtros, 
                                        ref string[] ParamsName, 
                                        ref DbType[] ParamsType,
                                        ref object[] ParamsValue)
        {
            ParamsName[0] = "@PROVEEDOR";
            ParamsType[0] = DbType.String;
            ParamsValue[0] = filtros.Proveedor;
        }

        public IDataReader ObtenerConsultaVisitasaFacturar(TextBox Fecha,String Proveedor)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@PROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@FECHA";
            String[] FechaAComprobar = new String[3];
            FechaAComprobar = Fecha.Text.Split('/');

            ParamsValue[1] = FechaAComprobar[2] + '-' + FechaAComprobar[1] + '-' + FechaAComprobar[0];
            ParamsType[1] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_VISITAS_A_FACTURAR", ParamsName, ParamsType, ParamsValue);
        }
        public IDataReader ObtenerConsultaVisitasaFacturarPorPagina(TextBox Fecha, String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[4];
            DbType[] ParamsType = new DbType[4];
            object[] ParamsValue = new object[4];

            ParamsName[0] = "@PROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@FECHA";
            String[] FechaAComprobar = new String[3];
            FechaAComprobar = Fecha.Text.Split('/');

            ParamsValue[1] = FechaAComprobar[2] + '-' + FechaAComprobar[1] + '-' + FechaAComprobar[0];
            ParamsType[1] = DbType.String;

            ParamsName[2] = "@DESDE";
            ParamsValue[2] = Desde;
            ParamsType[2] = DbType.Int64;

            ParamsName[3] = "@HASTA";
            ParamsValue[3] = Hasta;
            ParamsType[3] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_VISITAS_A_FACTURAR_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerDatosDesdeSentencia(String SQL)
        {
            StringBuilder strSQL = new StringBuilder(SQL);
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            IDataReader dr = db.RunQueryDataReader(strSQL.ToString());
            return dr;
        }


        public IDataReader ObtenerConsultaReparacionesaFacturar(TextBox Fecha,String Proveedor)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@PROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@FECHA";
            String[] FechaAComprobar = new String[3];
            FechaAComprobar = Fecha.Text.Split('/');

            ParamsValue[1] = FechaAComprobar[2] + '-' + FechaAComprobar[1] + '-' + FechaAComprobar[0];
            ParamsType[1] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_REPARACIONES_A_FACTURAR", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerPermisoFacturacionPorUsuario(String UserID)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@USERID";
            ParamsValue[0] = UserID;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_OBTENER_PERMISO_FACTURACION_VISITAS", ParamsName, ParamsType, ParamsValue);
        }

        public Int64 ActualizarSolicitudesBonificables(String Proveedor,String NumFactura)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@NUMFACTURA";
            ParamsValue[1] = NumFactura;
            ParamsType[1] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_ACTUALIZAR_SOLICITUDES_BONIFICABLES", ParamsName, ParamsType, ParamsValue);
            return 0;
        
        }

        public Int64 ActualizarVisitasAPenalizarBonificar(int IdSolicitud, Boolean Bonificable)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@IDSOLICITUD";
            ParamsValue[0] = IdSolicitud;
            ParamsType[0] = DbType.Int32;

            ParamsName[1] = "@BONIFICABLE";
            ParamsValue[1] = Bonificable;
            ParamsType[1] = DbType.Boolean;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_ACTUALIZAR_VISITAS_A_BONIFICAR_PENALIZAR", ParamsName, ParamsType, ParamsValue);
            return 0;
        
        }

        public Int64 ActualizarNumeroFactura(String Proveedor, String NumFactura, Int16 idIdioma)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@NUMFACTURA";
            ParamsValue[1] = NumFactura;
            ParamsType[1] = DbType.String;

            ParamsName[2] = "@idIDIOMA";
            ParamsValue[2] = idIdioma;
            ParamsType[2] = DbType.Int16;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_ACTUALIZAR_NUM_FACTURA_VISITA", ParamsName, ParamsType, ParamsValue);
            return 0;
        
        }

        public Int64 InsertarAccesoPantalla(String Usuario, String URL)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@USUARIO";
            ParamsValue[0] = Usuario;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@URL";
            ParamsValue[1] = URL;
            ParamsType[1] = DbType.String;
            
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("spInsertAccesoPantalla", ParamsName, ParamsType, ParamsValue);
            return 0;

        }
        

        public Int64 ActualizarSolicitudesNoBonificables(String Proveedor, String NumFactura)
        {
            //RCC
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@NUMFACTURA";
            ParamsValue[1] = NumFactura;
            ParamsType[1] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_ACTUALIZAR_SOLICITUDES_NOBONIFICABLES", ParamsName, ParamsType, ParamsValue);
            return 0;

        }

        public Int64 ActualizarSolicitudesSupervision(String Proveedor,Int16 idIdioma)
        {
            //RCC
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@idIDIOMA";
            ParamsValue[1] = idIdioma;
            ParamsType[1] = DbType.Int16;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            db.RunProcEscalar("SP_ACTUALIZAR_SOLICITUDES_SUPERVISION_APAGAR", ParamsName, ParamsType, ParamsValue);
            return 0;

        }
        public IDataReader ObtenerSolicitudesBonificables(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_BONIFICABLES", ParamsName, ParamsType, ParamsValue);
        }
        public IDataReader ObtenerSolicitudesBonificablesBusqueda(String NumFactura,String Proveedor)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@NUMFACTURA";
            ParamsValue[1] = NumFactura;
            ParamsType[1] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_BONIFICABLES_BUSQUEDA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesSupervisionBusqueda(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_SUPERVISION_BUSQUEDA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesSupervisionBusquedaPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_SUPERVISION_BUSQUEDA_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerVisitasModificadasPorcambioTelefono(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_VISITAS_MODIFICADAS_POR_CAMBIO_TELEFONO", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerVisitasModificadasPorcambioTelefonoPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_VISITAS_MODIFICADAS_POR_CAMBIO_TELEFONO_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesSupervisionAPagar(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_SUPERVISION_APAGAR", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesSupervisionAPagarPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_SUPERVISION_APAGAR_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }



        public IDataReader ObtenerVisitasPenalizablesBonificables(String Proveedor,int Bonificable)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            if (Bonificable == 1)
            {
                return db.RunProcDataReader("SP_GET_VISITAS_BONIFICABLES_BUSQUEDA", ParamsName, ParamsType, ParamsValue);
            }
            else
            {
                return db.RunProcDataReader("SP_GET_VISITAS_PENALIZABLES_BUSQUEDA", ParamsName, ParamsType, ParamsValue);
            }
        }

        public IDataReader ObtenerVisitasPenalizablesBonificablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta, int Bonificable)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            if (Bonificable == 1)
            {
                return db.RunProcDataReader("SP_GET_VISITAS_BONIFICABLES_BUSQUEDA_POR_PAGINA", ParamsName, ParamsType, ParamsValue);
            }
            else
            {
                return db.RunProcDataReader("SP_GET_VISITAS_PENALIZABLES_BUSQUEDA_POR_PAGINA", ParamsName, ParamsType, ParamsValue);
            }
        }


        public IDataReader ObtenerVisitasAPenalizaBonificar(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            return db.RunProcDataReader("SP_GET_VISITAS_A_PENALIZAR_BONIFICAR", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerVisitasAPenalizaBonificarPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_VISITAS_A_PENALIZAR_BONIFICAR_POR_PAGINA", ParamsName, ParamsType, ParamsValue);
            
        }






        public IDataReader ObtenerVisitasINCORRECTASPenalizablesBonificables(String Proveedor, int Bonificable)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            if (Bonificable == 1)
            {
                return db.RunProcDataReader("SP_GET_VISITAS_INCORRECTAS_BONIFICABLES_BUSQUEDA", ParamsName, ParamsType, ParamsValue);
            }
            else
            {
                return db.RunProcDataReader("SP_GET_VISITAS_INCORRECTAS_PENALIZABLES_BUSQUEDA", ParamsName, ParamsType, ParamsValue);
            }
        }

        public IDataReader ObtenerVisitasINCORRECTASPenalizablesBonificablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta, int Bonificable)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            if (Bonificable == 1)
            {
                return db.RunProcDataReader("SP_GET_VISITAS_INCORRECTAS_BONIFICABLES_BUSQUEDA_POR_PAGINA", ParamsName, ParamsType, ParamsValue);
            }
            else
            {
                return db.RunProcDataReader("SP_GET_VISITAS_INCORRECTAS_PENALIZABLES_BUSQUEDA_POR_PAGINA", ParamsName, ParamsType, ParamsValue);
            }
        }


        public IDataReader ObtenerVisitasINCORRECTASAPenalizaBonificar(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            return db.RunProcDataReader("SP_GET_VISITAS_INCORRECTAS_A_PENALIZAR_BONIFICAR", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerVisitasINCORRECTASAPenalizaBonificarPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_VISITAS_INCORRECTAS_A_PENALIZAR_BONIFICAR_POR_PAGINA", ParamsName, ParamsType, ParamsValue);

        }
        

        public IDataReader ObtenerSolicitudesBonificablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_BONIFICABLES_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }
        public IDataReader ObtenerSolicitudesBonificablesPorPaginaBusqueda(String NumFactura,String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[4];
            DbType[] ParamsType = new DbType[4];
            object[] ParamsValue = new object[4];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            ParamsName[3] = "@NUMFACTURA";
            ParamsValue[3] = NumFactura;
            ParamsType[3] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_BONIFICABLES_PORPAGINA_BUSQUEDA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesNOBonificablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_NOBONIFICABLES_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }
        public IDataReader ObtenerSolicitudesNOBonificablesPorPaginaBusqueda(String NumFactura,String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[4];
            DbType[] ParamsType = new DbType[4];
            object[] ParamsValue = new object[4];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            ParamsName[3] = "@NUMFACTURA";
            ParamsValue[3] = NumFactura;
            ParamsType[3] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_NOBONIFICABLES_PORPAGINA_BUSQUEDA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesNOBonificables(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_NOBONIFICABLES", ParamsName, ParamsType, ParamsValue);
        }
        
        public IDataReader ObtenerSolicitudesTecnicos(String Proveedor, String Nombre, String Apellido)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@NOMBRE";
            ParamsValue[1] = "%" + Nombre + "%" + Apellido + "%";
            ParamsType[1] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_TECNICOS_PROVEEDOR", ParamsName, ParamsType, ParamsValue);
        }
        public IDataReader ObtenerSolicitudesTecnicosPorPagina(String Proveedor, Int64 Desde, Int64 Hasta, String Nombre, String Apellido)
        {
            string[] ParamsName = new string[4];
            DbType[] ParamsType = new DbType[4];
            object[] ParamsValue = new object[4];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            ParamsName[3] = "@NOMBRE";
            ParamsValue[3] ="%" + Nombre + "%" + Apellido + "%";
            ParamsType[3] = DbType.String;


            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_TECNICOS_PROVEEDOR_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesRevisionPrecinteRealizadas(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_REVISION_PRECINTE_REALIZADAS", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesRevisionPrecintePenalizables(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_REVISION_PRECINTE_PENALIZABLES", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesSupervision(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_SUPERVISION", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesSupervisionPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_SUPERVISION_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesRevisionPrecinteRealizadasPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_REVISION_PRECINTE_REALIZADAS_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerSolicitudesRevisionPrecintePenalizablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_REVISION_PRECINTE_PENALIZABLES_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerProveedoresAveriasVisitas(String Provincia, String Poblacion, String CP)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@CODPROVINCIA";
            ParamsValue[0] = Provincia;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@CODPOBLACION";
            ParamsValue[1] = Poblacion;
            ParamsType[1] = DbType.String;

            ParamsName[2] = "@CODPOSTAL";
            ParamsValue[2] = CP;
            ParamsType[2] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_PROVEEDORESAVERIAVISITA", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerProveedoresAveriasVisitasPorPagina(String Provincia, String Poblacion, String CP, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[5];
            DbType[] ParamsType = new DbType[5];
            object[] ParamsValue = new object[5];

            ParamsName[0] = "@CODPROVINCIA";
            ParamsValue[0] = Provincia;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@CODPOBLACION";
            ParamsValue[1] = Poblacion;
            ParamsType[1] = DbType.String;

            ParamsName[2] = "@CODPOSTAL";
            ParamsValue[2] = CP;
            ParamsType[2] = DbType.String;

            ParamsName[3] = "@DESDE";
            ParamsValue[3] = Desde;
            ParamsType[3] = DbType.Int64;

            ParamsName[4] = "@HASTA";
            ParamsValue[4] = Hasta;
            ParamsType[4] = DbType.Int64;


            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_PROVEEDORESAVERIAVISITA_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }


        public IDataReader ObtenerSolicitudesNOBonificablesBusqueda(String NumFactura,String Proveedor)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@CODPROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@NUMFACTURA";
            ParamsValue[1] = NumFactura;
            ParamsType[1] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_GET_SOLICITUDES_NOBONIFICABLES_BUSQUEDA", ParamsName, ParamsType, ParamsValue);
        }
        
        

        public IDataReader ObtenerConsultaReparacionesaFacturarPorPagina(TextBox Fecha, String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[4];
            DbType[] ParamsType = new DbType[4];
            object[] ParamsValue = new object[4];

            ParamsName[0] = "@PROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@FECHA";
            String[] FechaAComprobar = new String[3];
            FechaAComprobar = Fecha.Text.Split('/');

            ParamsValue[1] = FechaAComprobar[2] + '-' + FechaAComprobar[1] + '-' + FechaAComprobar[0];
            ParamsType[1] = DbType.String;

            ParamsName[2] = "@DESDE";
            ParamsValue[2] = Desde;
            ParamsType[2] = DbType.Int64;

            ParamsName[3] = "@HASTA";
            ParamsValue[3] = Hasta;
            ParamsType[3] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_REPARACIONES_A_FACTURAR_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }
        

        public IDataReader ObtenerConsultaVisitasVencidas(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@PROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;
            
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_VISITAS_VENCIDAS", ParamsName, ParamsType, ParamsValue);
        }
        public IDataReader ObtenerConsultaVisitasVencidasPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@PROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_VISITAS_VENCIDAS_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }
        public IDataReader ObtenerConsultaVisitasPenalizables(String Proveedor)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@PROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_VISITAS_PENALIZABLES", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerConsultaVisitasPenalizablesPorPagina(String Proveedor, Int64 Desde, Int64 Hasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@PROVEEDOR";
            ParamsValue[0] = Proveedor;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@DESDE";
            ParamsValue[1] = Desde;
            ParamsType[1] = DbType.Int64;

            ParamsName[2] = "@HASTA";
            ParamsValue[2] = Hasta;
            ParamsType[2] = DbType.Int64;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_VISITAS_PENALIZABLES_PORPAGINA", ParamsName, ParamsType, ParamsValue);
        }
        

        public IDataReader ObtenerConsultasContratosBaja(VistaContratoCompletoDTO filtros,TextBox txt)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[1] = "@CODCONTRATO";
            ParamsValue[1] = txt.Text;
            if (filtros != null)
            {
                ObtenerFiltroDeProveedor(filtros, ref ParamsName, ref ParamsType, ref ParamsValue);
            }

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_CONTRATOS_EN_ESTADO_DE_BAJA", ParamsName, ParamsType,ParamsValue);
        }
        public IDataReader ObtenerConsultasContratosBajaPORFECHAS(VistaContratoCompletoDTO filtros, TextBox txtDesde,TextBox txtHasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[1] = "@DESDE";
            ParamsName[2] = "@HASTA";
            string[] DatosDesde= new string[3];
            string[] DatosHasta= new string[3];
            DatosDesde = txtDesde.Text.ToString().Split('/');
            DatosHasta = txtHasta.Text.ToString().Split('/');
            String Desde="";
            String Hasta="";
            Desde = DatosDesde[2] + "/" + DatosDesde[1] + "/" + DatosDesde[0];
            Hasta = DatosHasta[2] + "/" + DatosHasta[1] + "/" + DatosHasta[0];
            ParamsValue[1] = Desde;
            ParamsValue[2] = Hasta;
            if (filtros != null)
            {
                ObtenerFiltroDeProveedor(filtros, ref ParamsName, ref ParamsType, ref ParamsValue);
            }

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_CONTRATOS_EN_ESTADO_DE_BAJA_PORFECHAS", ParamsName, ParamsType, ParamsValue);
        }  

        public IDataReader ObtenerConsultasContratosBajaCONVISITASPORFECHAS(VistaContratoCompletoDTO filtros, TextBox txtDesde,TextBox txtHasta)
        {
            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[1] = "@DESDE";
            ParamsName[2] = "@HASTA";
            string[] DatosDesde= new string[3];
            string[] DatosHasta= new string[3];
            DatosDesde = txtDesde.Text.ToString().Split('/');
            DatosHasta = txtHasta.Text.ToString().Split('/');
            String Desde="";
            String Hasta="";
            Desde = DatosDesde[2] + "/" + DatosDesde[1] + "/" + DatosDesde[0];
            Hasta = DatosHasta[2] + "/" + DatosHasta[1] + "/" + DatosHasta[0];
            ParamsValue[1] = Desde;
            ParamsValue[2] = Hasta;
            if (filtros != null)
            {
                ObtenerFiltroDeProveedor(filtros, ref ParamsName, ref ParamsType, ref ParamsValue);
            }

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_CONTRATOS_EN_ESTADO_DE_BAJA_CON_VISITA_PORFECHAS", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader VisitasConPRL(TextBox txtDesde, TextBox txtHasta)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];
            ParamsName[0] = "@DESDE";
            ParamsName[1] = "@HASTA";

            string[] DatosDesde = new string[3];
            string[] DatosHasta = new string[3];
            DatosDesde = txtDesde.Text.ToString().Split('/');
            DatosHasta = txtHasta.Text.ToString().Split('/');
            String Desde = "";
            String Hasta = "";
            Desde = DatosDesde[2] + "/" + DatosDesde[1] + "/" + DatosDesde[0];
            Hasta = DatosHasta[2] + "/" + DatosHasta[1] + "/" + DatosHasta[0];
            ParamsValue[0] = Desde;
            ParamsValue[1] = Hasta;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("spReportVisitasConPRL", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerConsultasMantenimientoProvincia(VistaContratoCompletoDTO filtros)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            if (filtros != null)
            {
                ObtenerFiltroDeProveedor(filtros, ref ParamsName, ref ParamsType, ref ParamsValue);
            }

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_MANTENIMIENTOS_POR_PROVINCIA", ParamsName, ParamsType, ParamsValue);
        }      

        public IDataReader ObtenerConsultaCambioTelefonos(VistaContratoCompletoDTO filtros)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            if (filtros != null)
            {
                ObtenerFiltroDeProveedor(filtros, ref ParamsName, ref ParamsType, ref ParamsValue);
            }

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_CAMBIO_TELEFONOS_VISITAS_CANCELADAS", ParamsName,ParamsType, ParamsValue);
        }
        public IDataReader ObtenerConsultaContratosSinTelefonos(VistaContratoCompletoDTO filtros)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            if (filtros != null)
            {
                ObtenerFiltroDeProveedor(filtros, ref ParamsName, ref ParamsType, ref ParamsValue);
            }

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_CONTRATOS_SIN_TELEFONO", ParamsName, ParamsType, ParamsValue);
        }
        

        public IDataReader ObtenerConsultaVencidosPorMes(VistaContratoCompletoDTO filtros)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            if (filtros != null)
            {
                ObtenerFiltroDeProveedor(filtros, ref ParamsName, ref ParamsType, ref ParamsValue);
            }

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_VENCIDOS_POSIBLES_POR_MES", ParamsName, ParamsType, ParamsValue);
        }

        public IDataReader ObtenerConsultaFacturadasProveedor(VistaContratoCompletoDTO filtros)
        {
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            if (filtros != null)
            {
                ObtenerFiltroDeProveedor(filtros, ref ParamsName, ref ParamsType, ref ParamsValue);
            }

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            return db.RunProcDataReader("SP_REPORT_CONTRATOS_FACTURADOS_A_PROVEEDOR", ParamsName, ParamsType, ParamsValue);
        }

        public string ObtenerCaracteristicaPorDescripcion(string descripcion, Int16 idIdioma)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@DESCRIPCION";
            ParamsValue[0] = descripcion;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@idIDIOMA";
            ParamsValue[1] = idIdioma;
            ParamsType[1] = DbType.Int16;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string intResultado = db.RunProcEscalar("SP_OBTENER_IDCARACTERISTICA_PORDESCRIPCION", ParamsName, ParamsType, ParamsValue).ToString();
            return intResultado;
        }


        public string ObtenerActivoCaracteristicaPorDescripcion(string descripcion,string cod_subtipo,string cod_estado,string idsolicitud, Int16 idIdioma )
        {
            string[] ParamsName = new string[5];
            DbType[] ParamsType = new DbType[5];
            object[] ParamsValue = new object[5];

            ParamsName[0] = "@DESCRIPCION";
            ParamsValue[0] = descripcion;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@cod_subtipo";
            ParamsValue[1] = cod_subtipo;
            ParamsType[1] = DbType.String;

            ParamsName[2] = "@cod_estado";
            ParamsValue[2] = cod_estado;
            ParamsType[2] = DbType.String;

            ParamsName[3] = "@id_solicitud";
            ParamsValue[3] = idsolicitud;
            ParamsType[3] = DbType.Int32;

            ParamsName[4] = "@idIDIOMA";
            ParamsValue[4] = idIdioma;
            ParamsType[4] = DbType.Int16;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            var value = db.RunProcEscalar("SP_OBTENER_ACTIVOCARACTERISTICA_PORDESCRIPCION", ParamsName, ParamsType, ParamsValue);
            string intResultado = "";
            if (value == null)
            {
                intResultado = "True";
            }
            else
            {
                intResultado = value.ToString();
            }
            return intResultado;
        }

        public Boolean ObtenerObligatoriedadPorDescripcion(string descripcion, Int16 idIdioma)
        {
            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@DESCRIPCION";
            ParamsValue[0] = descripcion;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@idIDIOMA";
            ParamsValue[1] = idIdioma;
            ParamsType[1] = DbType.Int16;


            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            Boolean intResultado = Boolean.Parse(db.RunProcEscalar("SP_OBTENER_OBLIGATORIO_PORDESCRIPCION", ParamsName, ParamsType, ParamsValue).ToString());
            return intResultado;
        }

        public string ObtenerProveedorAveriaPorContrato(string contrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] parametros = new string[1];
            DbType[] tiposValoresParametros = new DbType[1];
            Object[] valoresParametros = new Object[1];
            parametros[0] = "@COD_CONTRATO";
            tiposValoresParametros[0] = DbType.String;
            valoresParametros[0] = contrato;

            object result = db.RunProcEscalar("spSMGObtenerProveedorAveriaPorContrato", parametros, tiposValoresParametros, valoresParametros);

            if (!DBNull.Value.Equals(result) && result != null)
            {
                return result.ToString();
            }
            else
            {
                // Sin no se ha encontrado nada el contador es 0.
                return "";
            }
          
        }

        public string ObtenerProveedorInspeccionGasPorContrato(string contrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] parametros = new string[1];
            DbType[] tiposValoresParametros = new DbType[1];
            Object[] valoresParametros = new Object[1];
            parametros[0] = "@COD_CONTRATO";
            tiposValoresParametros[0] = DbType.String;
            valoresParametros[0] = contrato;

            object result = db.RunProcEscalar("spSMGObtenerProveedorInspeccionGasPorContrato", parametros, tiposValoresParametros, valoresParametros);

            if (!DBNull.Value.Equals(result) && result != null)
            {
                return result.ToString();
            }
            else
            {
                // Sin no se ha encontrado nada el contador es 0.
                return "";
            }

        }

        public DataTable ObtenerValoresTablaPorTipoCaracteristica(string tipCar, Int16 idIdioma)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[2];
            DbType[] ParamsType = new DbType[2];
            object[] ParamsValue = new object[2];

            ParamsName[0] = "@tipoCaracteristica";
            ParamsValue[0] = tipCar;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@idIDIOMA";
            ParamsValue[1] = idIdioma;
            ParamsType[1] = DbType.Int16;

            return db.RunProcDataTable("spSMGGetValoresTablaPorTipoCaracteristica", ParamsName, ParamsType, ParamsValue);
            
        }

        public DataTable ObtenerValoresPTableValuePorParentIdTableYParentCodTable(string idTable, string codTable,int idIdioma)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[3];
            DbType[] ParamsType = new DbType[3];
            object[] ParamsValue = new object[3];

            ParamsName[0] = "@idTable";
            ParamsValue[0] = idTable;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@codTable";
            ParamsValue[1] = codTable;
            ParamsType[1] = DbType.Int16;

            ParamsName[2] = "@idIdioma";
            ParamsValue[2] = idIdioma;
            ParamsType[2] = DbType.Int16;

            return db.RunProcDataTable("spSMGGetValoresPtablevaluePorParentIdTable_ParentCodTable", ParamsName, ParamsType, ParamsValue);

        }

        public DataTable ObtenerValoresActualesYAnterioresPTableValuePorIdTableYCodTable(string idTable, string codTable, int idIdioma, string tipCar, string codEstado,string codSubtipo)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[6];
            DbType[] ParamsType = new DbType[6];
            object[] ParamsValue = new object[6];

            ParamsName[0] = "@idTable";
            ParamsValue[0] = idTable;
            ParamsType[0] = DbType.String;

            ParamsName[1] = "@codTable";
            ParamsValue[1] = codTable;
            ParamsType[1] = DbType.String;

            ParamsName[2] = "@idIdioma";
            ParamsValue[2] = idIdioma;
            ParamsType[2] = DbType.Int16;

            ParamsName[3] = "@tipCar";
            ParamsValue[3] = tipCar;
            ParamsType[3] = DbType.String;

            ParamsName[4] = "@codEstado";
            ParamsValue[4] = codEstado;
            ParamsType[4] = DbType.String;

            ParamsName[5] = "@codSubtipo";
            ParamsValue[5] = codSubtipo;
            ParamsType[5] = DbType.String;

            return db.RunProcDataTable("spSMGGetValoresPtablevaluePorIdTable_CodTable", ParamsName, ParamsType, ParamsValue);

        }

        public Boolean esInspeccion(string codContrato)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@codContrato";
            ParamsValue[0] = codContrato;
            ParamsType[0] = DbType.String;
            

            DataTable dt= db.RunProcDataTable("spGetEsInspeccionPorContrato", ParamsName, ParamsType, ParamsValue);
            if(dt.Rows.Count==0)
            {
                return false;
            }
            else
            {
                return Boolean.Parse(dt.Rows[0].ItemArray[0].ToString());
            }

        }
    }
}