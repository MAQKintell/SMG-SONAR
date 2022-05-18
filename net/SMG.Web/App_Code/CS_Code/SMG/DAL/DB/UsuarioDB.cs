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
using Iberdrola.SMG.DAL;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.DataAccess;
using Iberdrola.Commons.Exceptions;

using System.IO;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Clase para realizar todas las operaciones sobre usuarios
    /// en la base de datos.
    /// </summary>
    public class UsuarioDB
    {
        private void ErrorLog(String sPathName, String sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt", true);
            sw.WriteLine(DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> " + sErrMsg);
            sw.Flush();
            sw.Close();
        }

        public IDataReader ObtenerPerfiles()
        {
            try
            {
                //*************************************************************************
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                //UsuarioDTO usuario = new UsuarioDTO();

                string[] Parametros = new string[0];

                DbType[] Tipos = new DbType[0];

                string[] ValoresParametros = new string[0];

                IDataReader dr = db.RunProcDataReader("SP_GET_PERFILES", Parametros, Tipos, ValoresParametros);
                return dr;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007");
            }
        }
       
        public UsuarioDTO ObtenerUsuarioCompleto(String strUsuario, String password)
        {
            try
            {
                //*************************************************************************
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                //UsuarioDTO usuario = new UsuarioDTO();

                string[] Parametros = new string[2];
                Parametros[0] = "@LOGIN";
                Parametros[1] = "@PASSWORD";

                DbType[] Tipos = new DbType[2];
                Tipos[0] = DbType.String;
                Tipos[1] = DbType.String;

                string[] ValoresParametros = new string[2];
                ValoresParametros[0] = strUsuario;
                ValoresParametros[1] = password;

                IDataReader dr = db.RunProcDataReader("SP_CHECK_USER", Parametros, Tipos, ValoresParametros);
                
                while (dr.Read())
                {
                    UsuarioDTO usuario = new UsuarioDTO();

                    usuario.Login = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "LOGIN");
                    usuario.Password = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PASSWORD");
                    usuario.Nombre = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE");
                    usuario.Email = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "EMAIL");
                    usuario.Id_Perfil = Int64.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "ID_PERFIL").ToString());
                    usuario.Permiso = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "PERMISO");
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PROVEEDOR") != null)
                    {
                        usuario.CodProveedor = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PROVEEDOR");
                        usuario.NombreProveedor = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE_PROVEEDOR");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "ID_IDIOMA") != null)
                    {
                        usuario.IdIdioma = (int)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_IDIOMA");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PAIS") != null)
                    {
                        usuario.Pais = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PAIS");
                    }

                    return usuario;
                }
                return null;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007");
            }
       
            //UsuarioDTO usuario = new UsuarioDTO();
            //usuario.Id = strUsuario;
            //usuario.Nombre = "Nombre del usuario";

            //PerfilDTO perfil = new PerfilDTO();
            //perfil.Id = "1";
            //perfil.Nombre = "Admin";

            //List<PerfilDTO> listaPerfiles = new List<PerfilDTO>();
            //listaPerfiles.Add(perfil);

            //usuario.ListaRoles = listaPerfiles;
            
            //return usuario;
        }

        public UsuarioDTO ObtenerUsuarioCompletoWS(String strUsuario, String password)
        {
            try
            {
                //*************************************************************************
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                //UsuarioDTO usuario = new UsuarioDTO();

                string[] Parametros = new string[2];
                Parametros[0] = "@LOGIN";
                Parametros[1] = "@PASSWORD";

                DbType[] Tipos = new DbType[2];
                Tipos[0] = DbType.String;
                Tipos[1] = DbType.String;

                string[] ValoresParametros = new string[2];
                ValoresParametros[0] = strUsuario;
                ValoresParametros[1] = password;

                IDataReader dr = db.RunProcDataReader("SP_CHECK_USER_WS", Parametros, Tipos, ValoresParametros);

                while (dr.Read())
                {
                    UsuarioDTO usuario = new UsuarioDTO();

                    usuario.Login = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "LOGIN");
                    usuario.Password = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PASSWORD");
                    usuario.Nombre = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE");
                    usuario.Email = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "EMAIL");
                    usuario.Id_Perfil = Int64.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "ID_PERFIL").ToString());
                    usuario.Permiso = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "PERMISO");
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PROVEEDOR") != null)
                    {
                        usuario.CodProveedor = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PROVEEDOR");
                        usuario.NombreProveedor = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE_PROVEEDOR");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "ID_IDIOMA") != null)
                    {
                        usuario.IdIdioma = (int)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_IDIOMA");
                    }
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PAIS") != null)
                    {
                        usuario.Pais = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PAIS");
                    }

                    return usuario;
                }
                return null;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007");
            }

            //UsuarioDTO usuario = new UsuarioDTO();
            //usuario.Id = strUsuario;
            //usuario.Nombre = "Nombre del usuario";

            //PerfilDTO perfil = new PerfilDTO();
            //perfil.Id = "1";
            //perfil.Nombre = "Admin";

            //List<PerfilDTO> listaPerfiles = new List<PerfilDTO>();
            //listaPerfiles.Add(perfil);

            //usuario.ListaRoles = listaPerfiles;

            //return usuario;
        }

        public Boolean ExisteUsuario(String strUsuario)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                Boolean Existe = false;

                string[] Parametros = new string[1];
                Parametros[0] = "@LOGIN";

                DbType[] Tipos = new DbType[1];
                Tipos[0] = DbType.String;

                string[] ValoresParametros = new string[1];
                ValoresParametros[0] = strUsuario;


                IDataReader dr = db.RunProcDataReader("SP_CHECK_USER_EXISTS", Parametros, Tipos, ValoresParametros);

                while (dr.Read())
                {
                    if ((int)DataBaseUtils.GetDataReaderColumnValue(dr, "") != 0)
                    {
                        Existe = true;
                    }

                }
                return Existe;
            }
            catch (BaseException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new DALException(false, ex, "2002");
            }

        }

        public Boolean CambioPassword(string Login, string Password, string strUsuarioConectado)
        {
            Boolean Correcto = false;
            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[3];
                Parametros[0] = "@LOGIN";
                Parametros[1] = "@PASSWORD";
                Parametros[2] = "@USUARIO";

                DbType[] Tipos = new DbType[3];
                Tipos[0] = DbType.String;
                Tipos[1] = DbType.String;
                Tipos[2] = DbType.String;

                string[] ValoresParametros = new string[3];
                ValoresParametros[0] = Login;
                ValoresParametros[1] = Password;
                ValoresParametros[2] = strUsuarioConectado;

                IDataReader dr = db.RunProcDataReader("SP_ACTUALIZAR_PASSWORD", Parametros, Tipos, ValoresParametros);

                Correcto = true;
                return Correcto;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2002");
            }

        }

        public bool AniadirUsuario(UsuarioDTO usuario, string strUsuarioConectado)
        {
            Boolean Correcto = false;
            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[8];
                Parametros[0] = "@UserID";
                Parametros[1] = "@Name";
                Parametros[2] = "@Password";
                Parametros[3] = "@Email";
                Parametros[4] = "@ID_Perfil";
                Parametros[5] = "@Permiso";
                Parametros[6] = "@USUARIO";
                Parametros[7] = "@ID_PAIS";

                DbType[] Tipos = new DbType[8];
                Tipos[0] = DbType.String;
                Tipos[1] = DbType.String;
                Tipos[2] = DbType.String;
                Tipos[3] = DbType.String;
                Tipos[4] = DbType.Decimal;
                Tipos[5] = DbType.Boolean;
                Tipos[6] = DbType.String;
                Tipos[7] = DbType.Int16;

                object[] ValoresParametros = new object[8];
                ValoresParametros[0] = usuario.Login;
                ValoresParametros[1] = usuario.Nombre;
                ValoresParametros[2] = usuario.Password;
                ValoresParametros[3] = usuario.Email;
                ValoresParametros[4] = usuario.Id_Perfil;
                ValoresParametros[5] = usuario.Permiso;
                ValoresParametros[6] = strUsuarioConectado;
                ValoresParametros[7] = usuario.Pais;

                IDataReader dr = db.RunProcDataReader("SP_ANIADIR_USUARIO", Parametros, Tipos, ValoresParametros);

                Correcto = true;
                return Correcto;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public bool EliminarUsuario(UsuarioDTO usuario, string strUsuarioConectado)
        {
            Boolean Correcto = false;
            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[2];
                Parametros[0] = "@UserID";
                Parametros[1] = "@USUARIO";

                DbType[] Tipos = new DbType[2];
                Tipos[0] = DbType.String;
                Tipos[1] = DbType.String;

                object[] ValoresParametros = new object[2];
                ValoresParametros[0] = usuario.Login;
                ValoresParametros[1] = strUsuarioConectado;

                IDataReader dr = db.RunProcDataReader("SP_ELIMINAR_USUARIO", Parametros, Tipos, ValoresParametros);

                Correcto = true;
                return Correcto;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public bool ActivarUsuario(UsuarioDTO usuario, string strUsuario)
        {
            Boolean Correcto = false;
            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[2];
                Parametros[0] = "@UserID";
                Parametros[1] = "@USUARIO";

                DbType[] Tipos = new DbType[2];
                Tipos[0] = DbType.String;
                Tipos[1] = DbType.String;

                object[] ValoresParametros = new object[2];
                ValoresParametros[0] = usuario.Login;
                ValoresParametros[1] = strUsuario;

                IDataReader dr = db.RunProcDataReader("SP_ACTIVAR_USUARIO", Parametros, Tipos, ValoresParametros);

                Correcto = true;
                return Correcto;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }
        
        public bool ModificarUsuario(UsuarioDTO usuario, string strUsuarioConectado)
        {
            Boolean Correcto = false;
            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[9];
                Parametros[0] = "@UserID";
                Parametros[1] = "@Name";
                Parametros[2] = "@Password";
                Parametros[3] = "@Email";
                Parametros[4] = "@ID_Perfil";
                Parametros[5] = "@Permiso";
                Parametros[6] = "@FechaBaja";
                Parametros[7] = "@USUARIO";
                Parametros[8] = "@ID_PAIS";

                DbType[] Tipos = new DbType[9];
                Tipos[0] = DbType.String;
                Tipos[1] = DbType.String;
                Tipos[2] = DbType.String;
                Tipos[3] = DbType.String;
                Tipos[4] = DbType.Decimal;
                Tipos[5] = DbType.Boolean;
                Tipos[6] = DbType.DateTime;
                Tipos[7] = DbType.String;
                Tipos[8] = DbType.Int16;

                object[] ValoresParametros = new object[9];
                ValoresParametros[0] = usuario.Login;
                ValoresParametros[1] = usuario.Nombre;
                ValoresParametros[2] = usuario.Password;
                ValoresParametros[3] = usuario.Email;
                ValoresParametros[4] = usuario.Id_Perfil;
                ValoresParametros[5] = usuario.Permiso;

                if (usuario.Activo == "False")
                {
                    ValoresParametros[6] = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    ValoresParametros[6] = null;
                }

                ValoresParametros[7] = strUsuarioConectado;
                ValoresParametros[8] = usuario.Pais;

                IDataReader dr = db.RunProcDataReader("SP_MODIFICAR_USUARIO", Parametros, Tipos, ValoresParametros);

                Correcto = true;
                return Correcto;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public DataTable ObtenerUsuarios(string columna, string valor)
        {
        
            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[2];
                Parametros[0] = "@COLUMNA";
                Parametros[1] = "@VALOR";

                DbType[] Tipos = new DbType[2];
                Tipos[0] = DbType.String;
                Tipos[1] = DbType.String;


                object[] ValoresParametros = new object[6];
                ValoresParametros[0] = columna;
                ValoresParametros[1] = valor;

                IDataReader dr = db.RunProcDataReader("SP_GET_USUARIOS", Parametros, Tipos, ValoresParametros);
                
                DataTable tabla = new DataTable();
                tabla.Columns.Add(new DataColumn("UserID"));
                tabla.Columns.Add(new DataColumn("Descripcion"));
                while (dr.Read())
                {
                    DataRow fila = tabla.NewRow();
                    fila[0] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "UserID");
                    fila[1] = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "Descripcion");

                    tabla.Rows.Add(fila);
                }
                
                return tabla;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }


        public UsuarioDTO ObtenerUsuario(String userID)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[1];
                Parametros[0] = "@LOGIN";

                DbType[] Tipos = new DbType[1];
                Tipos[0] = DbType.String;

                string[] ValoresParametros = new string[1];
                ValoresParametros[0] = userID;

                IDataReader dr = db.RunProcDataReader("SP_GET_USUARIO", Parametros, Tipos, ValoresParametros);

                while (dr.Read())
                {
                    UsuarioDTO usuario = new UsuarioDTO();

                    usuario.Login = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "UserID");
                    usuario.Password = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PASSWORD");
                    usuario.Nombre = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NAME");
                    usuario.Email = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "EMAIL");
                    usuario.Id_Perfil = Int64.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "ID_PERFIL").ToString());
                    usuario.Permiso = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "PERMISO");
                    usuario.BAJA_AUTOMATICA = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "BAJA_AUTOMATICA");
                    
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "FECHABAJA") == null)
                    {
                        usuario.Activo = "";
                    }
                    else
                    {
                        usuario.Activo = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHABAJA").ToString();
                    }

                    //usuario.IdIdioma = (int)1;

                    usuario.Pais = DataBaseUtils.GetDataReaderColumnValue(dr, "ID_PAIS").ToString();
                    return usuario;
                }
                return null;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007");
            }

            //UsuarioDTO usuario = new UsuarioDTO();
            //usuario.Id = strUsuario;
            //usuario.Nombre = "Nombre del usuario";

            //PerfilDTO perfil = new PerfilDTO();
            //perfil.Id = "1";
            //perfil.Nombre = "Admin";

            //List<PerfilDTO> listaPerfiles = new List<PerfilDTO>();
            //listaPerfiles.Add(perfil);

            //usuario.ListaRoles = listaPerfiles;

            //return usuario;
        }


        public UsuarioDTO ObtenerUsuarioLDAP(String userID)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[1];
                Parametros[0] = "@LOGIN";

                DbType[] Tipos = new DbType[1];
                Tipos[0] = DbType.String;

                string[] ValoresParametros = new string[1];
                ValoresParametros[0] = userID;

                IDataReader dr = db.RunProcDataReader("SP_GET_USUARIO_LDAP", Parametros, Tipos, ValoresParametros);

                while (dr.Read())
                {
                    UsuarioDTO usuario = new UsuarioDTO();

                    usuario.Login = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "UserID");
                    usuario.Password = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PASSWORD");
                    usuario.Nombre = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NAME");
                    usuario.Email = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "EMAIL");
                    usuario.Id_Perfil = Int64.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "ID_PERFIL").ToString());
                    usuario.Permiso = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "PERMISO");
                    usuario.BAJA_AUTOMATICA = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "BAJA_AUTOMATICA");

                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "FECHABAJA") == null)
                    {
                        usuario.Activo = "";
                    }
                    else
                    {
                        usuario.Activo = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHABAJA").ToString();
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "ID_IDIOMA") != null)
                    {
                        usuario.IdIdioma = (int)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_IDIOMA");
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PAIS") != null)
                    {
                        usuario.Pais = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PAIS");
                    }
                    
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PROVEEDOR") != null)
                    {
                        usuario.CodProveedor = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PROVEEDOR");
                        usuario.NombreProveedor = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE_PROVEEDOR");
                    }
                  
                    
                    return usuario;
                }
                return null;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007");
            }
        }

        public string UsuarioTramitadorBaja(string userID)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[1];
                Parametros[0] = "@LOGIN";

                DbType[] Tipos = new DbType[1];
                Tipos[0] = DbType.String;

                string[] ValoresParametros = new string[1];
                ValoresParametros[0] = userID;

                return (string)db.RunProcEscalar("SP_USUARIO_GET_TRAMITADOR_BAJA", Parametros, Tipos, ValoresParametros);
                
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007");
            }

            //UsuarioDTO usuario = new UsuarioDTO();
            //usuario.Id = strUsuario;
            //usuario.Nombre = "Nombre del usuario";

            //PerfilDTO perfil = new PerfilDTO();
            //perfil.Id = "1";
            //perfil.Nombre = "Admin";

            //List<PerfilDTO> listaPerfiles = new List<PerfilDTO>();
            //listaPerfiles.Add(perfil);

            //usuario.ListaRoles = listaPerfiles;

            //return usuario;
        }

        public IDataReader ObtenerProveedores()
        {

            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[0];
                DbType[] Tipos = new DbType[0];
                object[] ValoresParametros = new object[0];

                IDataReader dr = db.RunProcDataReader("SP_OBTENER_PROVEEDORES", Parametros, Tipos, ValoresParametros);

                
                return dr;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public IDataReader ObtenerTecnicoEmpresa(String Proveedor)
        {

            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[1];
                Parametros[0] = "@PROVEEDOR";
                DbType[] Tipos = new DbType[1];
                Tipos[0] = DbType.String;
                object[] ValoresParametros = new object[1];
                ValoresParametros[0] = Proveedor;

                IDataReader dr = db.RunProcDataReader("SP_GET_TECNICOS_EMPRESA", Parametros, Tipos, ValoresParametros);

                
                return dr;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }
        
        public String ObtenerEmpresaPorTecnico(Int16 idTecnico)
        {

            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[1];
                Parametros[0] = "@ID";
                DbType[] Tipos = new DbType[1];
                Tipos[0] = DbType.Int16;
                object[] ValoresParametros = new object[1];
                ValoresParametros[0] = idTecnico;

                IDataReader dr = db.RunProcDataReader("SP_GET_EMPRESA_POR_TECNICO", Parametros, Tipos, ValoresParametros);

                while (dr.Read())
                {
                    return (String)DataBaseUtils.GetDataReaderColumnValue(dr, "EMPRESA_TECNICOEMPRESA");
                }

                return "";
                
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public String ObtenerNombreTecnicoPorTecnico(Int16 idTecnico)
        {

            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[1];
                Parametros[0] = "@ID";
                DbType[] Tipos = new DbType[1];
                Tipos[0] = DbType.Int16;
                object[] ValoresParametros = new object[1];
                ValoresParametros[0] = idTecnico;

                IDataReader dr = db.RunProcDataReader("SP_GET_EMPRESA_POR_TECNICO", Parametros, Tipos, ValoresParametros);

                while (dr.Read())
                {
                    return (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE_TECNICOEMPRESA");
                }

                return "";
                
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }
        
        public IDataReader ObtenerTodosProveedoresEnSolicitudes()
        {

            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[0];
                DbType[] Tipos = new DbType[0];
                object[] ValoresParametros = new object[0];

                IDataReader dr = db.RunProcDataReader("SP_OBTENER_TODOS_PROVEEDORES_ENSOLICITUDES", Parametros, Tipos, ValoresParametros);


                return dr;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public IDataReader ObtenerNombreTodosProveedoresEnSolicitudes()
        {

            try
            {

                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] Parametros = new string[0];
                DbType[] Tipos = new DbType[0];
                object[] ValoresParametros = new object[0];

                IDataReader dr = db.RunProcDataReader("SP_OBTENER_TODOS_PROVEEDORES", Parametros, Tipos, ValoresParametros);


                return dr;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public void InsertarAccesoUsuario(UsuarioDTO usuario)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[1];
                DbType[] tipos = new DbType[1];
                object[] valoresParametros = new object[1];

                parametros[0] = "@pCOD_USUARIO";
                tipos[0] = DbType.String;
                valoresParametros[0] = usuario.Login;

                db.RunProcNonQuery("spCommonsAccesoUsuarioInsert", parametros, tipos, valoresParametros);
            }
            catch (BaseException ba)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        /// <summary>
        /// Indica si la contraseña de un usuario se ha caducado o no. Para ello utiliza el 
        /// valor de la tabla de configuración P_CONFIG que indica el número de días en los que és
        /// válida una contraseña.
        /// </summary>
        /// <param name="strLogin">Login del usuario que quiere entrar en la aplicación.</param>
        /// <returns> True si está caducada, false si es válida </returns>
        public bool ContraseniaCaducada(string strUsuario)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[1];
                DbType[] tipos = new DbType[1];
                object[] valoresParametros = new object[1];

                parametros[0] = "@LOGIN";
                tipos[0] = DbType.String;
                valoresParametros[0] = strUsuario;

                int intResultado = (int)db.RunProcEscalar("spCommonsUsuarioContraseniaCaducada", parametros, tipos, valoresParametros);
                return (intResultado == 1);
            }
            catch (BaseException ba)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public int ObtenerCantidadUsuariosPorNombre(string Nombre,string Apellido1,string Apellido2,String Contrato,string Subtipo,string DNI, string CUI)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[7];
                DbType[] tipos = new DbType[7];
                object[] valoresParametros = new object[7];

                parametros[0] = "@NOMBRE";
                tipos[0] = DbType.String;
                valoresParametros[0] = Nombre;

                parametros[1] = "@APELLIDO1";
                tipos[1] = DbType.String;
                valoresParametros[1] = Apellido1;

                parametros[2] = "@APELLIDO2";
                tipos[2] = DbType.String;
                valoresParametros[2] = Apellido2;

                parametros[3] = "@CONTRATO";
                tipos[3] = DbType.String;
                valoresParametros[3] = Contrato;

                parametros[4] = "@SUBTIPO";
                tipos[4] = DbType.String;
                valoresParametros[4] = Subtipo;

                parametros[5] = "@DNI";
                tipos[5] = DbType.String;
                valoresParametros[5] = DNI;

                parametros[6] = "@CUI";
                tipos[6] = DbType.String;
                valoresParametros[6] = CUI;

                int intResultado = (int)db.RunProcEscalar("SP_OBTENER_CANTIDAD_USUARIOS_PORNOMBRE", parametros, tipos, valoresParametros);
                return intResultado;
            }
            catch (BaseException ba)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public int ObtenerCantidadUsuariosCalderaPorNombre(string Nombre, string Apellido1, string Apellido2, String Contrato, string Subtipo)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[5];
                DbType[] tipos = new DbType[5];
                object[] valoresParametros = new object[5];

                parametros[0] = "@NOMBRE";
                tipos[0] = DbType.String;
                valoresParametros[0] = Nombre;

                parametros[1] = "@APELLIDO1";
                tipos[1] = DbType.String;
                valoresParametros[1] = Apellido1;

                parametros[2] = "@APELLIDO2";
                tipos[2] = DbType.String;
                valoresParametros[2] = Apellido2;

                parametros[3] = "@CONTRATO";
                tipos[3] = DbType.String;
                valoresParametros[3] = Contrato;

                parametros[4] = "@SUBTIPO";
                tipos[4] = DbType.String;
                valoresParametros[4] = Subtipo;

                int intResultado = (int)db.RunProcEscalar("SP_OBTENER_CANTIDAD_USUARIOS_CALDERA_PORNOMBRE", parametros, tipos, valoresParametros);
                return intResultado;
            }
            catch (BaseException ba)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        public IDataReader ObtenerUsuariosPorNombre(string Nombre, string Apellido1, string Apellido2,String Subtipo)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[4];
                DbType[] tipos = new DbType[4];
                object[] valoresParametros = new object[4];

                parametros[0] = "@NOMBRE";
                tipos[0] = DbType.String;
                valoresParametros[0] = Nombre;

                parametros[1] = "@APELLIDO1";
                tipos[1] = DbType.String;
                valoresParametros[1] = Apellido1;

                parametros[2] = "@APELLIDO2";
                tipos[2] = DbType.String;
                valoresParametros[2] = Apellido2;

                parametros[3] = "@SUBTIPO";
                tipos[3] = DbType.String;
                valoresParametros[3] = Subtipo;

                IDataReader dr = db.RunProcDataReader("SP_OBTENER_USUARIOS_PORNOMBRE", parametros, tipos, valoresParametros);


                return dr;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); //Error al insertar o actualizar datos de la Base de Datos
            }
        }

        /// <summary>
        /// Cambiamos el password de un usuario.
        /// </summary>
        /// <param name="login">Login del usuario a cambiar.</param>
        /// <param name="password">Password nueva a asignar.</param>
        /// <param name="strUsuarioConectado">Login del usuario conectado</param>
        public void CambiarPassword(string login, string password, string strUsuarioConectado)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[3];
                DbType[] tipos = new DbType[3];
                string[] valoresparametros = new string[3];

                parametros[0] = "@pLOGIN";
                tipos[0] = DbType.String;
                valoresparametros[0] = login;

                parametros[1] = "@pPASSWORD";
                tipos[1] = DbType.String;
                valoresparametros[1] = password;

                parametros[2] = "@USUARIO";
                tipos[2] = DbType.String;
                valoresparametros[2] = strUsuarioConectado;

                db.RunProcNonQuery("spCommonsUsuarioUpdatePassword", parametros, tipos, valoresparametros);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2002");
            }
        }

        /// <summary>
        /// Buscamos los datos de un usuario por login o nombre.
        /// </summary>
        /// <param name="columna">Le pasamos si queremos que nos busque o en el login o en el nombre.</param>
        /// <param name="valor">Le pasamos el valor que queremos buscar.</param>
        /// <param name="bajas">Indica si tiene que devolver o no los usuarios dados de baja.</param>
        /// <param name="login">Usuario que realiza la búsqueda.</param>
        /// <returns>Nos devuelve un DataTable con los registros encontrados.</returns>
        public DataTable ObtenerUsuarios(string columna, string valor, bool bajas, string login)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[4];
                DbType[] tipos = new DbType[4];
                object[] valoresparametros = new object[4];

                parametros[0] = "@pCOLUMNA";
                tipos[0] = DbType.String;
                valoresparametros[0] = columna;

                parametros[1] = "@pVALOR";
                tipos[1] = DbType.String;
                valoresparametros[1] = valor;

                parametros[2] = "@pBAJAS";
                tipos[2] = DbType.Boolean;
                valoresparametros[2] = bajas;

                parametros[3] = "@pLOGIN";
                tipos[3] = DbType.String;
                valoresparametros[3] = login;

                return db.RunProcDataTable("spCommonsUsuarioFindAllByLoginOrNombre", parametros, tipos, valoresparametros);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005"); // Error al insertar o actualizar datos de la Base de Datos
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios de la base de datos.
        /// No discrimina entre los dados de baja o no. 
        /// Principalmente se utilizará para la encriptación/desencriptación
        /// masiva de las passwords.
        /// </summary>
        /// <returns>List<UsuarioDTO> con los datos de los usuarios recuperados de la base de datos</returns>
        public List<UsuarioDTO> ObtenerUsuarios()
        {
            IDataReader dr = null;
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                dr = db.RunProcDataReader("SP_GET_USUARIO_ALL", null, null, null);

                List<UsuarioDTO> listaUsuarios = new List<UsuarioDTO>();

                while (dr.Read())
                {
                    UsuarioDTO usuario = new UsuarioDTO();

                    usuario.Login = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "UserID");
                    usuario.Password = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "PASSWORD");
                    usuario.Nombre = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NAME");
                    usuario.Email = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "EMAIL");
                    
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "ID_PERFIL") != null)
                    {
                        usuario.Id_Perfil = Int64.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "ID_PERFIL").ToString());
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "PERMISO") != null)
                    {
                        usuario.Permiso = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "PERMISO");
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "FECHABAJA") == null)
                    {
                        usuario.Activo = "";
                    }
                    else
                    {
                        usuario.Activo = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "FECHABAJA").ToString();
                    }
                    usuario.Pais = DataBaseUtils.GetDataReaderColumnValue(dr, "ID_PAIS").ToString();
                    // Se añade el usuario recuperado a la lista
                    listaUsuarios.Add(usuario);
                }
                // Se retorna la lista de usuarios generada
                return listaUsuarios;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007");
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();

                }
            }
        }
    }
}
