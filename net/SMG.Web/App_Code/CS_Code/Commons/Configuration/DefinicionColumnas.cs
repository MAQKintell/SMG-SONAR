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
using System.Xml;
using System.IO;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;

namespace Iberdrola.Commons.Configuration
{
    /// <summary>
    /// Descripción breve de DefinicionColumnas
    /// </summary>
    public static class DefinicionColumnas
    {

        private static XmlDocument _XMLDoc;

        static DefinicionColumnas()
        {
            string ruta = HttpContext.Current.Server.MapPath(Resources.SMGConfiguration.XMLConfiguracionColumnasGrids);
            FileStream FS = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.Read);
            _XMLDoc = new XmlDocument();
            _XMLDoc.Load(FS);
        }

        public static void Refresh()
        {
            string ruta = HttpContext.Current.Server.MapPath(Resources.SMGConfiguration.XMLConfiguracionColumnasGrids);
            FileStream FS = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.Read);
            _XMLDoc = new XmlDocument();
            _XMLDoc.Load(FS);
        }

        public static ColumnDefinition GetColumnaTabla(String NombreTabla, String NombreColumna)
        {
            XmlNodeList xmlNodeList = _XMLDoc.SelectNodes("/SMG/" + NombreTabla + "/Columna[@nombre='" + NombreColumna + "']");

            ColumnDefinition columnDefinition;

            if (xmlNodeList.Count == 1)
            {
                try
                {
                    columnDefinition = new ColumnDefinition(xmlNodeList[0].Attributes[0].Value,
                    xmlNodeList[0].SelectNodes("Name").Item(0).InnerText,
                    Convert.ToInt32(xmlNodeList[0].SelectNodes("Width").Item(0).InnerText),
                    xmlNodeList[0].SelectNodes("Style").Item(0).InnerText);
                }
                catch (Exception ex)
                {
                    throw new ArqException(false, "1019");
                }
            }
            else
            {
                throw new ArqException(false, "1018");
            }

            return columnDefinition;
        }

        /// <summary>
        /// Busca el nombre de la columna a partir del nombre descriptivo 
        /// </summary>
        /// <param name="NombreTabla">Nombre de la tabla/control al que pertenece la columna</param>
        /// <param name="NombreCabeceraColumna">Nombre descriptivo de la columna a buscar (el mostrado en la cabecera)</param>
        /// <exception cref="Iberdrola.Commons.Exceptions.ArqException"></exception>
        /// <returns value="String">Nombre de la columna</returns>
        public static String GetNombreColumnaTabla(String NombreTabla, String NombreCabeceraColumna)
        {
            XmlNodeList xmlNodeList = _XMLDoc.SelectNodes("/SMG/" + NombreTabla);

            String nombreColumna = "";

            if (xmlNodeList.Count == 1)
            {
                bool encontrado = false;

                for (int counter = 0; counter < xmlNodeList[0].ChildNodes.Count - 1; counter++)
                {
                    try
                    {
                        if (xmlNodeList[0].ChildNodes[counter].SelectNodes("Name").Item(0).InnerText == NombreCabeceraColumna)
                        {
                            if (encontrado)
                            {
                                //La columna esta mas de una vez
                                throw new ArqException(false, "1022");
                            }
                            else
                            {
                                encontrado = true;
                                nombreColumna = xmlNodeList[0].ChildNodes[counter].Attributes[0].Value;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArqException(false, "1019");
                    }
                }
                if (!encontrado)
                {
                    //Excepción no se ha encontrado la columna informada
                    throw new ArqException(false, "1021");
                }
            }
            else
            {
                throw new ArqException(false, "1020");
            }

            return nombreColumna;
        }
    }
}