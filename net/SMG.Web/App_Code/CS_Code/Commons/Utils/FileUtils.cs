﻿//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Logging;
using Iberdrola.Commons.Security;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.BLL;

namespace Iberdrola.Commons.Utils
{
    /// <summary>
    /// Clase que contiene métodos de utilidad para el tratamiento de ficheros.
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// Indica si el fichero indicado por la ruta existe o no
        /// </summary>
        /// <param name="impersonator">Objeto Impersonator con los datos del usuario que accede a las rutas</param>
        /// <param name="path">Ruta del fichero a comprobar</param>
        /// <returns>Booleano indicando si existe o no el fichero</returns>
        public static bool FileExist(Impersonator impersonator, string path)
        {
            // Obtenemos los permisos para conectarnos a la unidad compartida.
            using (impersonator)
            {
                //Este método no funciona con * en la extensión
                //return File.Exists(path);
                if (Directory.Exists(Path.GetDirectoryName(path)))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(path));
                    FileInfo[] fileInfoArray = dirInfo.GetFiles(Path.GetFileName(path), SearchOption.TopDirectoryOnly);
                    return fileInfoArray.Length > 0;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Crea el directorio indicado
        /// </summary>
        /// <param name="impersonator">Objeto Impersonator con los datos del usuario que accede a las rutas</param>
        /// <param name="path">Ruta del directorio a crear</param>
        public static void DirectoryCreate(Impersonator impersonator, string path)
        {
            using (impersonator)
            {
                //Si el directorio destino del fichero no existe lo creamos
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
            }
        }

        /// <summary>
        /// Obtiene todos los ficheros de la ruta, nombre y extensión indicados
        /// </summary>
        /// <param name="impersonator">Objeto Impersonator con los datos del usuario que accede a las rutas</param>
        /// <param name="path">Ruta en la que buscar los ficheros</param>
        /// <param name="nombreFichero">Nombre del fichero a buscar</param>
        /// <param name="extension">Extensión de los ficheros</param>
        /// <returns>Lista con las direcciones completas de los ficheros localizados</returns>
        public static List<string> FileGetAll(Impersonator impersonator, string path, string nombreFichero, string extension)
        {
            try
            {
                List<string> listaFicheros = new List<string>();
                // Obtenemos los permisos para poder conectarnos a la unidad compartida.
                using (impersonator)
                {
                    if (Directory.Exists(path))
                    {
                        // Buscamos los ficheros.
                        DirectoryInfo dirInfo = new DirectoryInfo(path);
                        FileInfo[] fileInfoArray = dirInfo.GetFiles(nombreFichero + "." + extension, SearchOption.TopDirectoryOnly);

                        foreach (FileInfo fileInfo in fileInfoArray)
                        {
                            listaFicheros.Add(fileInfo.FullName);
                        }
                    }
                }
                return listaFicheros;
            }
            catch (BLLException ex1)
            {
                LogHelper.Error("Error al intentar recuperar los ficheros: " + ex1.Message, LogHelper.Category.BussinessLogic);
                LogHelper.Error("Error al intentar recuperar los ficheros: " + ex1.StackTrace, LogHelper.Category.BussinessLogic);

                throw ex1;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Excepción al intentar recuperar los ficheros:" + ex.Message, LogHelper.Category.BussinessLogic);
                LogHelper.Error("Excepción al intentar recuperar los ficheros:" + ex.StackTrace, LogHelper.Category.BussinessLogic);

                throw new ArqException(false, ex, "");
            }
        }

        /// <summary>
        /// Obtiene el último fichero modificado que coincida con la ruta, nombre y extensión proporcionados
        /// </summary>
        /// <param name="impersonator">Objeto Impersonator con los datos del usuario que accede a las rutas</param>
        /// <param name="path">Ruta en la que buscar los ficheros</param>
        /// <param name="nombreFichero">Nombre del fichero a buscar</param>
        /// <param name="extension">Extensión de los ficheros</param>
        /// <returns>Ruta completa del fichero localizado</returns>
        public static string FileGetLastEdited(Impersonator impersonator, string path, string nombreFichero, string extension)
        {
            try
            {
                string rutaFichero = "";
                DateTime fechaFichero = DateTime.Now.AddYears(-888);
                // Obtenemos los permisos para poder conectarnos a la unidad compartida.
                using (impersonator)
                {
                    if (Directory.Exists(path))
                    {
                        // Buscamos los ficheros.
                        DirectoryInfo dirInfo = new DirectoryInfo(path);
                        FileInfo[] fileInfoArray = dirInfo.GetFiles(nombreFichero + "." + extension, SearchOption.TopDirectoryOnly);

                        foreach (FileInfo fileInfo in fileInfoArray)
                        {
                            if (fechaFichero < fileInfo.CreationTime)
                            {
                                rutaFichero = fileInfo.FullName;
                                fechaFichero = fileInfo.CreationTime;
                            }
                        }
                    }
                }
                return rutaFichero;
            }
            catch (BLLException ex1)
            {
                LogHelper.Error("Error al intentar recuperar los ficheros: " + ex1.Message, LogHelper.Category.BussinessLogic);
                LogHelper.Error("Error al intentar recuperar los ficheros: " + ex1.StackTrace, LogHelper.Category.BussinessLogic);

                throw ex1;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Excepción al intentar recuperar los ficheros:" + ex.Message, LogHelper.Category.BussinessLogic);
                LogHelper.Error("Excepción al intentar recuperar los ficheros:" + ex.StackTrace, LogHelper.Category.BussinessLogic);

                throw new ArqException(false, ex, "");
            }
        }


        /// <summary>
        /// Copia el fichero origen proporcionado a la ruta indicada.
        /// </summary>
        /// <param name="impersonator">Objeto Impersonator con los datos del usuario que accede a las rutas</param>
        /// <param name="rutaFicheroOrigen">Ruta de todo el fichero path+nombrefichero+extension.</param>
        /// <param name="rutaDestino">Ruta destino (solo la ruta del directorio).</param>
        /// <param name="historico">Si se añade la fecha al mover el fichero.</param>
        public static string FileMove(Impersonator impersonator, string rutaFicheroOrigen, string rutaDestino, bool historico)
        {
            string nuevoNombre = "";
            try
            {
                // Obtenemos los permisos para conectarnos a la unidad compartida.
                using (impersonator)
                {
                    if (File.Exists(rutaFicheroOrigen))
                    {
                        //Si el directorio destino del fichero no existe lo creamos
                        if (!Directory.Exists(Path.GetDirectoryName(rutaDestino)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(rutaDestino));
                        }

                        nuevoNombre = Path.GetFileNameWithoutExtension(rutaFicheroOrigen);

                        if (historico)
                        {                            
                            nuevoNombre += "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");                            
                        }
                        nuevoNombre += Path.GetExtension(rutaFicheroOrigen);
                        rutaDestino = Path.Combine(Path.GetDirectoryName(rutaDestino), nuevoNombre);

                        File.Move(rutaFicheroOrigen, rutaDestino);
                    }
                    else
                    {
                        //CUENTA_CORREO_ADMINISTRADOR_SERVICIO
                        //Error al mover el fichero, el fichero no existe.
                        //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                        UtilidadesMail.EnviarMensajeError(" ERROR SUBIDA FICHERO", "Fichero no existe." + rutaFicheroOrigen + "_" + rutaDestino);
                        //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
                        throw new ArqException("");
                    }

                    return nuevoNombre;
                }
            }
            catch (Exception ex)
            {
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                UtilidadesMail.EnviarMensajeError(" ERROR SUBIDA FICHERO", ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_2000_");
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                LogHelper.Error("Error al intentar mover un fichero:" + ex.Message, LogHelper.Category.BussinessLogic);
                LogHelper.Error("Error al intentar mover un fichero:" + ex.StackTrace, LogHelper.Category.BussinessLogic);

                throw new ArqException("");
            }
        }

        //20210223 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        public static string FileMoveRewrite(Impersonator impersonator, string rutaFicheroOrigen, string rutaDestino)
        {
            string nuevoNombre = "";
            try
            {
                // Obtenemos los permisos para conectarnos a la unidad compartida.
                using (impersonator)
                {
                    if (File.Exists(rutaFicheroOrigen))
                    {
                        //Si el directorio destino del fichero no existe lo creamos
                        if (!Directory.Exists(Path.GetDirectoryName(rutaDestino)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(rutaDestino));
                        }

                        nuevoNombre = Path.GetFileName(rutaFicheroOrigen);

                        rutaDestino = Path.Combine(Path.GetDirectoryName(rutaDestino), nuevoNombre);

                        if (File.Exists(rutaDestino))
                        {
                            File.Delete(rutaDestino);
                        }

                        File.Move(rutaFicheroOrigen, rutaDestino);
                    }
                    else
                    {
                        //CUENTA_CORREO_ADMINISTRADOR_SERVICIO
                        //Error al mover el fichero, el fichero origen no existe.
                        UtilidadesMail.EnviarMensajeError(" ERROR SUBIDA FICHERO", "Fichero origen no existe." + rutaFicheroOrigen + "_" + rutaDestino);
                        throw new ArqException("");
                    }

                    return nuevoNombre;
                }
            }
            catch (Exception ex)
            {
                UtilidadesMail.EnviarMensajeError(" ERROR SUBIDA FICHERO", ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_2000_");
                LogHelper.Error("Error al intentar mover un fichero:" + ex.Message, LogHelper.Category.BussinessLogic);
                LogHelper.Error("Error al intentar mover un fichero:" + ex.StackTrace, LogHelper.Category.BussinessLogic);

                throw new ArqException("");
            }
        }
        //20210223 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

        /// <summary>
        /// Copia el fichero origen proporcionado a la ruta indicada.
        /// </summary>
        /// <param name="impersonator">Objeto Impersonator con los datos del usuario que accede a las rutas</param>
        /// <param name="rutaFicheroOrigen">Ruta de todo el fichero path+nombrefichero+extension.</param>
        /// <param name="rutaDestino">Ruta destino (solo la ruta del directorio).</param>
        /// <param name="historico">Si se añade la fecha al mover el fichero.</param>
        public static string FileCopy(Impersonator impersonator, string rutaFicheroOrigen, string rutaDestino, bool historico)
        {
            string nuevoNombre = "";
            try
            {
                // Obtenemos los permisos para conectarnos a la unidad compartida.
                using (impersonator)
                {
                    if (File.Exists(rutaFicheroOrigen))
                    {
                        //Si el directorio destino del fichero no existe lo creamos
                        if (!Directory.Exists(Path.GetDirectoryName(rutaDestino)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(rutaDestino));
                        }

                        //20201008 Corregir ruta de donde cogemos el nombre final destino
                        nuevoNombre = Path.GetFileNameWithoutExtension(rutaDestino);

                        if (historico)
                        {
                            nuevoNombre += "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        }
                        nuevoNombre += Path.GetExtension(rutaDestino);
                        rutaDestino = Path.Combine(Path.GetDirectoryName(rutaDestino), nuevoNombre);

                        File.Copy(rutaFicheroOrigen, rutaDestino,true);
                    }
                    else
                    {
                        //Error al mover el fichero, el fichero no existe.
                        throw new ArqException("");
                    }

                    return nuevoNombre;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error al intentar mover un fichero:" + ex.Message, LogHelper.Category.BussinessLogic);
                LogHelper.Error("Error al intentar mover un fichero:" + ex.StackTrace, LogHelper.Category.BussinessLogic);

                throw new ArqException("");
            }
        }

        /// <summary>
        /// Elimina el fichero indicado
        /// </summary>
        /// <param name="impersonator">Objeto Impersonator con los datos del usuario que accede a las rutas</param>
        /// <param name="path">Ruta del fichero a eliminar</param>
        public static void FileDelete(Impersonator impersonator, string path)
        {
            using (impersonator)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        ///// <summary>
        ///// Devuelve la ruta base de los ficheros de la aplicación
        ///// </summary>
        ///// <param name="pathRelativoWeb">Indica si se trata de los ficheros web o no</param>
        ///// <returns>Ruta de la base solicitada</returns>
        //public static string RootPathGet(bool pathRelativoWeb)
        //{
        //    string strPath = DBConfig.GetConfiguration(DBConfig.Config.PATH_FTP_ROOT).Value;

        //    if (pathRelativoWeb)
        //    {
        //        // En este caso la ruta web cuelga de la ruta base de los ficheros, con lo que solo hay que añadir el nivel de la ruta web
        //        strPath = Path.Combine(strPath, DBConfig.GetConfiguration(DBConfig.Config.PATH_FICHEROS_WEB).Value);
        //    }

        //    return strPath;
        //}
    }
}
