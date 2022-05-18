//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using System.Web.UI.WebControls;
using Iberdrola.SMG.BLL;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de ManejadorFicheros
    /// </summary>
    public class ManejadorFicheros
    {
        /// <summary>
        /// Obtiene el objto Impersonator para poder acceder a rutas del servidor con un usuario distinto al del sistema
        /// </summary>
        /// <returns>Impersonator con los datos de acceso</returns>
        public static Impersonator GetImpersonator()
        {
            if (ConfigurationManager.AppSettings[Constantes.APP_SETTING_IMPERSONATE_USER] != null)
            {
                bool bImpersonate = bool.Parse(ConfigurationManager.AppSettings[Constantes.APP_SETTING_IMPERSONATE_USER]);
                if (bImpersonate)
                {
                    return new Impersonator(
                                    ConfigurationManager.AppSettings[Constantes.APP_SETTING_USUARIO_RED_IMPERSONATE],
                                    ConfigurationManager.AppSettings[Constantes.APP_SETTING_DOMINIO_RED_IMPERSONATE],
                                    ConfigurationManager.AppSettings[Constantes.APP_SETTING_PASSWORD_RED_IMPERSONATE]
                                    );
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }            
        }

        /// <summary>
        /// Indica si el fichero indicado por la ruta existe o no
        /// </summary>
        /// <param name="path">Ruta del fichero a comprobar</param>
        /// <returns>Booleano indicando si existe o no el fichero</returns>
        public static bool ExisteFichero(string path)
        {
            return FileUtils.FileExist(GetImpersonator(), path);
        }

        /// <summary>
        /// Crea el directorio indicado
        /// </summary>
        /// <param name="path">Ruta del directorio a crear</param>
        public static void CrearDirectorio(string path)
        {
            FileUtils.DirectoryCreate(GetImpersonator(), path);
        }

        /// <summary>
        /// Obtiene todos los ficheros de la ruta, nombre y extensión indicados
        /// </summary>
        /// <param name="path">Ruta en la que buscar los ficheros</param>
        /// <param name="nombreFichero">Nombre del fichero a buscar</param>
        /// <param name="extension">Extensión de los ficheros</param>
        /// <returns>Lista con las direcciones completas de los ficheros localizados</returns>
        public static List<string> ObtenerFicheros(string path, string nombreFichero, string extension)
        {
            try
            {
                return FileUtils.FileGetAll(GetImpersonator(), path, nombreFichero, extension);
            }
            catch (Exception ex)
            {
                //ManejadorEmailsError.EnviarMensajeExcepcion("Error al intentar recuperar los ficheros", ex);

                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el último fichero modificado que coincida con la ruta, nombre y extensión proporcionados
        /// </summary>
        /// <param name="path">Ruta en la que buscar los ficheros</param>
        /// <param name="nombreFichero">Nombre del fichero a buscar</param>
        /// <param name="extension">Extensión de los ficheros</param>
        /// <returns>Ruta completa del fichero localizado</returns>
        public static string ObtenerFicheroUltimoModificado(string path, string nombreFichero, string extension)
        {
            try
            {
                return FileUtils.FileGetLastEdited(GetImpersonator(), path, nombreFichero, extension);
            }
            catch (Exception ex)
            {
                //ManejadorEmailsError.EnviarMensajeExcepcion("Error al intentar recuperar los ficheros", ex);

                throw ex;
            }
        }


        /// <summary>
        /// Mueve el fichero origen proporcionado a la ruta indicada.
        /// </summary>
        /// <param name="rutaFicheroOrigen">Ruta de todo el fichero path+nombrefichero+extension.</param>
        /// <param name="rutaDestino">Ruta destino (solo la ruta del directorio).</param>
        /// <param name="historico">Si se añade la fecha al mover el fichero.</param>
        public static string MoverFichero(string rutaFicheroOrigen, string rutaDestino, bool historico)
        {
            try
            {
                 return FileUtils.FileMove(GetImpersonator(), rutaFicheroOrigen, rutaDestino, historico);
            }
            catch (Exception ex)
            {
               // ManejadorEmailsError.EnviarMensajeExcepcion("Error al intentar mover un fichero", ex);

                throw ex;
            }
        }

        /// <summary>
        /// Copia el fichero origen proporcionado a la ruta indicada.
        /// </summary>
        /// <param name="rutaFicheroOrigen">Ruta de todo el fichero path+nombrefichero+extension.</param>
        /// <param name="rutaDestino">Ruta destino (solo la ruta del directorio).</param>
        /// <param name="historico">Si se añade la fecha al mover el fichero.</param>
        public static string CopiarFichero(string rutaFicheroOrigen, string rutaDestino, bool historico)
        {
            try
            {
                return FileUtils.FileMove(GetImpersonator(), rutaFicheroOrigen, rutaDestino, historico);
            }
            catch (Exception ex)
            {
                // ManejadorEmailsError.EnviarMensajeExcepcion("Error al intentar mover un fichero", ex);

                throw ex;
            }
        }

        /// <summary>
        /// Elimina el fichero indicado
        /// </summary>
        /// <param name="path">Ruta del fichero a eliminar</param>
        public static void BorrarFichero(string path)
        {
            FileUtils.FileDelete(GetImpersonator(), path);
        }

        /// <summary>
        /// Combina las rutas proporcionadas
        /// </summary>
        /// <param name="path1">Ruta que se colocará a la izquierda</param>
        /// <param name="path2">Ruta que se colocará a la derecha</param>
        /// <returns>String con las dos rutas combinadas</returns>
        public static string CombinarRutas(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// Devuelve la extensión del fichero proporcionado
        /// </summary>
        /// <param name="path">Ruta completa del fichero</param>
        /// <returns>Extensión del fichero</returns>
        public static string ObtenerExtensionFichero(string path)
        {
            return Path.GetExtension(path).Replace(".", string.Empty).ToLower();
        }

        /// <summary>
        /// Sube el fichero cargado en el objeto FileUpload
        /// </summary>
        /// <param name="file">FileUpload con los datos del fichero a subir</param>
        /// <param name="path">Ruta en la que guardar el fichero</param>
        public static bool SubirFichero(FileUpload file, string path)
        {
            bool subido = false;
            if (!ExisteFichero(path))
            {
                using (ManejadorFicheros.GetImpersonator())
                {
                    //Si el directorio destino del fichero no existe lo creamos
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    }

                    file.SaveAs(path);
                    subido = true;
                }
            }

            return subido;
        }



    }
}