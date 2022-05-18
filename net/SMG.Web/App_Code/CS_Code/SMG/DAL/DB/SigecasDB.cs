using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System.IO;
using Iberdrola.Commons.DataAccess;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.DAL.DB
{
    public class SigecasDB
    {
        public static DataTable DevolverQuery(string APP,String SQL)
        {
            try
            {
                //DataBaseLibrary db = BaseDB.GetDataBaseLibrarySigecas(APP, "Produccion");
                //return db.RunQueryDataTable(SQL);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void EjecutarQuery(string APP, String SQL)
        {
            try
            {
                //DataBaseLibrary db = BaseDB.GetDataBaseLibrarySigecas(APP, "Produccion");
                //db.RunQueryNonValue(SQL);
            }
            catch (Exception ex)
            {
                //
            }
        }

        public static Int64 RunProcEscalar(string APP, string procName)
        {
            try
            {
                //string[] ParamsName = new string[0];
                //DbType[] ParamsType = new DbType[0];
                //object[] ParamsValue = new object[0];

                //DataBaseLibrary db = BaseDB.GetDataBaseLibrarySigecas(APP, "Produccion");
                //db.RunProcEscalar(procName, ParamsName, ParamsType, ParamsValue);
                return 0;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        public static void CambiarPassword(string Login, string Password, string usuarioLogueado,string APP)
        {
            try
            {
                //DataBaseLibrary db = BaseDB.GetDataBaseLibrarySigecas(APP, "Produccion");

                //string[] parametros = new string[0];
                //DbType[] tipos = new DbType[0];
                //string[] valoresparametros = new string[0];

                //if (APP != "VALCRED")
                //{
                //    parametros = new string[3];
                //    tipos = new DbType[3];
                //    valoresparametros = new string[3];

                //    parametros[0] = "@pLOGIN";
                //    tipos[0] = DbType.String;
                //    valoresparametros[0] = Login;

                //    parametros[1] = "@pPASSWORD";
                //    tipos[1] = DbType.String;
                //    valoresparametros[1] = Password;

                //    parametros[2] = "@pUSUARIO_LOGUEADO";
                //    tipos[2] = DbType.String;
                //    valoresparametros[2] = usuarioLogueado;
                //}
                //else
                //{
                //    parametros = new string[2];
                //    tipos = new DbType[2];
                //    valoresparametros = new string[2];

                //    parametros[0] = "@LOGIN";
                //    tipos[0] = DbType.String;
                //    valoresparametros[0] = Login;

                //    parametros[1] = "@PASSWORD";
                //    tipos[1] = DbType.String;
                //    valoresparametros[1] = Password;
                //}
                //db.RunProcNonQuery("spCommonsUsuarioUpdatePassword", parametros, tipos, valoresparametros);
            }
            catch (Exception ex)
            {
                
            }
        }

        public static void EliminarUsuario(string Login, string loginUsuarioLogueado, string APP)
        {
            try
            {
                //DataBaseLibrary db = BaseDB.GetDataBaseLibrarySigecas(APP, "Produccion");

                //string[] parametros = new string[2];
                //DbType[] tipos = new DbType[2];
                //object[] valoresparametros = new object[2];

                //if (APP.ToUpper() != "AGP" && APP.ToUpper() != "CERENER" && APP.ToUpper() != "GESSOLAR")
                //{
                //    parametros[0] = "@UserID";
                //    tipos[0] = DbType.String;
                //    valoresparametros[0] = Login;

                //    parametros[1] = "@LoginUsuarioResponsable";
                //    tipos[1] = DbType.String;
                //    valoresparametros[1] = loginUsuarioLogueado;
                //}
                //else 
                //{
                //    parametros[0] = "@pLOGIN";
                //    tipos[0] = DbType.String;
                //    valoresparametros[0] = Login;

                //    parametros[1] = "@pUSUARIO_LOGUEADO";
                //    tipos[1] = DbType.String;
                //    valoresparametros[1] = loginUsuarioLogueado;
                //}

                //if (APP.ToUpper() != "BSOC")
                //{

                //    db.RunProcNonQuery("spCommonsUsuarioDelete", parametros, tipos, valoresparametros);
                //}
                //else
                //{
                //    db.RunProcNonQuery("SP_ELIMINAR_USUARIO", parametros, tipos, valoresparametros);
                //}
            }
            catch (Exception ex)
            {
                
            }
        }

        public static int ObtenerNumLotesPendientesRecampa(string Usuario)
        {
            try
            {
                //DataBaseLibrary db = BaseDB.GetDataBaseLibrarySigecas("Recampa", "Produccion");

                //string[] parametros = new string[1];
                //DbType[] tipos = new DbType[1];
                //object[] valores = new object[1];

                //parametros[0] = "@pUSUARIO";
                //tipos[0] = DbType.String;
                //valores[0] = Usuario;

                //return (int)db.RunProcEscalar("spRecampaLoteFindNumPendientesPorUsuario", parametros, tipos, valores);
                return 1;
            }
            catch (Exception ex)
            {
            //    throw new DALException(false, ex, "2005"); // Error al insertar o actualizar datos de la Base de Datos
                return 0;
            }
        }

        public static DataTable ObtenerPerfiles(string APP)
        {
            try
            {
                //DataBaseLibrary db = BaseDB.GetDataBaseLibrarySigecas(APP, "Produccion");

                //if (APP.ToUpper() == "CERENER" || APP.ToUpper() == "AGP" || APP.ToUpper() == "GESSOLAR")
                //{
                //    return db.RunQueryDataTable("spCommonsPerfilGet");
                //}
                //else if (APP.ToUpper() == "BSOC")
                //{
                //    return db.RunQueryDataTable("SP_GET_PERFILES");
                //}
                //else
                //{
                //    return null;
                //}
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void CrearUsuario(string APP, UsuarioSigecasDTO usuario, string password)
        {
            try
            {
                //DataBaseLibrary db = BaseDB.GetDataBaseLibrarySigecas(APP, "Produccion");

                //if (APP.ToUpper() == "CERENER" || APP.ToUpper() == "GESSOLAR" || APP.ToUpper() == "AGP")
                //{
                //    string[] parametros = new string[7];
                //    DbType[] tipos = new DbType[7];
                //    object[] valoresparametros = new object[7];

                //    parametros[0] = "@pLOGIN";
                //    tipos[0] = DbType.String;
                //    valoresparametros[0] = usuario.Login;

                //    parametros[1] = "@pNOMBRE";
                //    tipos[1] = DbType.String;
                //    valoresparametros[1] = usuario.Nombre;

                //    parametros[2] = "@pPASSWORD";
                //    tipos[2] = DbType.String;
                //    valoresparametros[2] = password;

                //    parametros[3] = "@pEMAIL";
                //    tipos[3] = DbType.String;
                //    valoresparametros[3] = usuario.Email;

                //    parametros[4] = "@pID_PERFIL";
                //    tipos[4] = DbType.Int32;
                //    valoresparametros[4] = usuario.Id_Perfil;

                //    if (APP.ToUpper() == "CERENER" || APP.ToUpper() == "GESSOLAR")
                //    {
                //        parametros[5] = "@pID_IDIOMA";
                //        tipos[5] = DbType.Int32;
                //        valoresparametros[5] = 1;
                //    }
                //    else if (APP.ToUpper() == "AGP")
                //    {
                //        parametros[5] = "@pID_PROVEEDOR";
                //        tipos[5] = DbType.Int32;
                //        valoresparametros[5] = null;
                        
                //    }

                //    parametros[6] = "@pUSUARIO_LOGUEADO";
                //    tipos[6] = DbType.String;
                //    valoresparametros[6] = "E013104";

                //    db.RunProcNonQuery("spCommonsUsuarioInsert", parametros, tipos, valoresparametros);
                //}
                //else if (APP.ToUpper() == "BSOC")
                //{
                //    string[] parametros = new string[6];
                //    DbType[] tipos = new DbType[6];
                //    object[] valoresparametros = new object[6];

                //    parametros[0] = "@UserID";
                //    tipos[0] = DbType.String;
                //    valoresparametros[0] = usuario.Login;

                //    parametros[1] = "@Name";
                //    tipos[1] = DbType.String;
                //    valoresparametros[1] = usuario.Nombre;

                //    parametros[2] = "@Password";
                //    tipos[2] = DbType.String;
                //    valoresparametros[2] = password;

                //    parametros[3] = "@Email";
                //    tipos[3] = DbType.String;
                //    valoresparametros[3] = usuario.Email;

                //    parametros[4] = "@ID_Perfil";
                //    tipos[4] = DbType.Int32;
                //    valoresparametros[4] = usuario.Id_Perfil;

                //    parametros[5] = "@LoginUsuarioResponsable";
                //    tipos[5] = DbType.String;
                //    valoresparametros[5] = "E013104";

                //    db.RunProcNonQuery("SP_ANIADIR_USUARIO", parametros, tipos, valoresparametros);
                //}
            }
            catch (Exception ex)
            {
            }
        }
    }
}
