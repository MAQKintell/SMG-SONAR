using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Iberdrola.Commons.Logging
{
    /// <summary>
    /// Clase que encapsula la clase Logger de Enterprise Library
    /// </summary>
    public class LogHelper
    {
        public enum Category
        {
            Architecture,
            DataAccess,
            BussinessLogic,
            UserInterface,
            General
        }

        public enum Level
        {
            Error,
            Warning,
            Debug
        }


        private void Initilize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        private static ICollection<string> GetStringCategories(ICollection<Category> categories)
        {
            ICollection<string> stringList = new List<string>();
            foreach (Category categ in categories)
            {
                stringList.Add(categ.ToString());
            }
            return stringList;
        }

        /// <summary>
        /// Obtiene la entrada de log
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="categories"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static LogEntry GetLogEntry(string msg, ICollection<Category> categories, Exception ex, Level level)
        {
            LogEntry le = new LogEntry();

            if (!String.IsNullOrEmpty(msg))
            {
                le.Message = msg;
            }

            if (ex != null)
            {
                StringBuilder message = new StringBuilder(le.Message);
                message.Append("\r\n");
                message.Append(ex.GetType().ToString());
                message.Append(": ");
                message.Append(ex.Message);
                if (ex.InnerException != null)
                {
                    message.Append("\r\n");
                    message.Append(ex.InnerException.GetType().ToString());
                    message.Append(": ");
                    message.Append(ex.InnerException.Message);
                }
                message.Append("\r\n");
                message.Append(ex.StackTrace);

                le.Message = message.ToString();
            }

            if (categories != null)
            {
                ICollection<string> stringCategories = GetStringCategories(categories);
                le.Categories = stringCategories;
            }

            switch (level)
            {
                case Level.Error: le.Severity = System.Diagnostics.TraceEventType.Error;
                    break;
                case Level.Warning: le.Severity = System.Diagnostics.TraceEventType.Warning;
                    break;
                case Level.Debug: le.Severity = System.Diagnostics.TraceEventType.Verbose;
                    break;
                default: le.Severity = System.Diagnostics.TraceEventType.Error;
                    break;
            }

            return le;
        }

        public static void Debug(string msg)
        {
            Logger.Write(LogHelper.GetLogEntry(msg, null, null, Level.Debug));
        }

        #region Métodos debug
        public static void Debug(string msg, Category categ)
        {
            ICollection<Category> icCateg = null;
            icCateg = new List<Category>();
            icCateg.Add(categ);
            Logger.Write(LogHelper.GetLogEntry(msg, icCateg, null, Level.Debug));
        }

        public static void Debug(string msg, List<Category> categories)
        {
            Logger.Write(LogHelper.GetLogEntry(msg, categories, null, Level.Debug));
        }
        public static void Debug(string msg, List<Category> categories, Exception ex)
        {
            Logger.Write(LogHelper.GetLogEntry(msg, categories, ex, Level.Debug));
        }

        public static void Debug(string msg, Category category, Exception ex)
        {
            List<Category> categories = new List<Category>();
            categories.Add(category);
            Logger.Write(LogHelper.GetLogEntry(msg, categories, ex, Level.Debug));
        }
        #endregion

        #region Métodos de Error
        public static void Error(string msg, Category categ)
        {
            ICollection<Category> icCateg = null;
            icCateg = new List<Category>();
            icCateg.Add(categ);
            Logger.Write(LogHelper.GetLogEntry(msg, icCateg, null, Level.Error));
        }

        public static void Error(string msg, List<Category> categories)
        {
            Logger.Write(LogHelper.GetLogEntry(msg, categories, null, Level.Error));
        }

        public static void Error(string msg, List<Category> categories, Exception ex)
        {
            Logger.Write(LogHelper.GetLogEntry(msg, categories, ex, Level.Error));
        }

        public static void Error(string msg, Category category, Exception ex)
        {
            List<Category> categories = new List<Category>();
            categories.Add(category);
            Logger.Write(LogHelper.GetLogEntry(msg, categories, ex, Level.Error));
        }
        #endregion

        #region Métodos de Warning
        public static void Warn(string msg, Category categ)
        {
            ICollection<Category> icCateg = null;
            icCateg = new List<Category>();
            icCateg.Add(categ);
            Logger.Write(LogHelper.GetLogEntry(msg, icCateg, null, Level.Warning));
        }

        public static void Warn(string msg, List<Category> categories)
        {
            Logger.Write(LogHelper.GetLogEntry(msg, categories, null, Level.Warning));
        }

        public static void Warn(string msg, List<Category> categories, Exception ex)
        {
            Logger.Write(LogHelper.GetLogEntry(msg, categories, ex, Level.Warning));
        }

        public static void Warn(string msg, Category category, Exception ex)
        {
            List<Category> categories = new List<Category>();
            categories.Add(category);
            Logger.Write(LogHelper.GetLogEntry(msg, categories, ex, Level.Warning));
        }
        #endregion
    }
}