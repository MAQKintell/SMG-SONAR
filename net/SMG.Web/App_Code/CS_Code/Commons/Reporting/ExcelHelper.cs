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
using System.Collections.Generic;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using Iberdrola.Commons.Configuration;
using Iberdrola.Commons.Utils;

namespace Iberdrola.Commons.Reporting
{
    // TODO capturar las excepciones posibles y lanzar exceptiones de Arquitectura.

    /// <summary>
    /// Clase para sacar por la Response un documento Excel que tiene el contenido
    /// de in data reader y que se le aplica la plantilla XSL que se le indique
    /// </summary>
    public class ExcelHelper
    {
        #region atributos
        private String _tittle = String.Empty;
        private String _xslTemplate = String.Empty;
        private String _TableName = String.Empty;
        private List<ExcelHeaderAttribute> _attributeList;
        private IDataReader _dataReader;
        #endregion

        #region propiedades

        /// <summary>
        /// El título del formulario
        /// </summary>
        public String Tittle
        {
            get { return this._tittle; }
        }

        /// <summary>
        /// La plantilla Xsl que te utilizará para dar formato al formulario
        /// </summary>
        public String XslTemplate
        {
            get { return this._xslTemplate; }
        }

        public String TableName
        {
            set { this._TableName = value; }
        }

        /// <summary>
        /// La lista de atributos de cabecera del formulario
        /// </summary>
        public List<ExcelHeaderAttribute> Attributtes
        {
            get { return this._attributeList; }
        }

        #endregion

        #region constructores
        public ExcelHelper(String title, String xslTemplate)
        {
            _attributeList = new List<ExcelHeaderAttribute>();
            this._tittle = title;
            this._xslTemplate = xslTemplate;
        }
        #endregion

        #region métodos privados

        private List<String> GetTableColumNames(DataTable dt)
        {
            List<String> colNamesList = new List<string>();
            foreach (DataRow drow in dt.Rows)
            {
                // cogemos el nombre de la columna.
                colNamesList.Add((String)drow[0]);
            }
            return colNamesList;
        }

        private XmlNode GenerateHeaderNode(XmlDataDocument xmlDoc)
        {
            XmlElement xmlElementHeader = xmlDoc.CreateElement("Header");
            XmlElement xmlElementTittle = xmlDoc.CreateElement("Tittle");
            xmlElementTittle.InnerText = this._tittle;
            xmlElementHeader.AppendChild(xmlElementTittle);

            XmlElement xmlElementAttributes = xmlDoc.CreateElement("HeaderAttributes");

            foreach(ExcelHeaderAttribute attrib in this._attributeList)
            {
                XmlElement xmlElementAttribute = xmlDoc.CreateElement("HeaderAttribute");
                XmlAttribute xmlAttributeName = xmlDoc.CreateAttribute("name");
                xmlAttributeName.Value = attrib.Name;
                xmlElementAttribute.InnerText = attrib.Value;
                xmlElementAttribute.Attributes.Append(xmlAttributeName);
                xmlElementAttributes.AppendChild(xmlElementAttribute);
            }
            xmlElementHeader.AppendChild(xmlElementAttributes);
            return xmlElementHeader;
        }
        
        private XmlNode GenerateDataNode(XmlDataDocument xmlDoc)
        {
            // recorrer el data reader y generar el XML
            XmlElement xmlElementData = xmlDoc.CreateElement("Data");
            XmlElement xmlElementDataHeader = null;
            XmlElement xmlElementDataRows = null;

            Boolean dataHeaderCreado = false;

            List<String> listaNombresColumna = GetTableColumNames(this._dataReader.GetSchemaTable());

            xmlElementDataHeader = xmlDoc.CreateElement("DataHeader");
            xmlElementDataRows = xmlDoc.CreateElement("DataRows");

            while (this._dataReader.Read())
            {
                //Row
                XmlElement xmlElementDataRow = xmlDoc.CreateElement("Row");
                foreach (String strColName in listaNombresColumna)
                {                
                    // DataHeader
                    if (!dataHeaderCreado)
                    {
                        //HeaderColumn
                        XmlElement xmlElementColumnHeader = xmlDoc.CreateElement("HeaderColumn");
                        XmlAttribute xmlAttributeColumnHeaderName = xmlDoc.CreateAttribute("name");
                        xmlAttributeColumnHeaderName.InnerText = DefinicionColumnas.GetColumnaTabla(_TableName, strColName).Name;
                        xmlElementColumnHeader.Attributes.Append(xmlAttributeColumnHeaderName);
                        xmlElementDataHeader.AppendChild(xmlElementColumnHeader);
                    }
 
                    // RowColumn
                    XmlElement xmlElementRowColumn = xmlDoc.CreateElement("RowColumn");
                    if (!this._dataReader[strColName].Equals(DBNull.Value))
                    {
                        if (typeof(DateTime)==this._dataReader[strColName].GetType())
                        {
                            //xmlElementRowColumn.InnerText = DateUtils.DateTimeToHTML((DateTime)(this._dataReader[strColName]));
                            xmlElementRowColumn.InnerText = this._dataReader[strColName].ToString().Substring(0,10);
                        }
                        else
                        {
                            xmlElementRowColumn.InnerText = this._dataReader[strColName].ToString();
                        }
                    }                   
                    xmlElementDataRow.AppendChild(xmlElementRowColumn);
                }
                if (!dataHeaderCreado)
                {
                    dataHeaderCreado = true;
                }
                xmlElementDataRows.AppendChild(xmlElementDataRow);
            }

            xmlElementData.AppendChild(xmlElementDataHeader);
            xmlElementData.AppendChild(xmlElementDataRows);

            return xmlElementData;
        }

        private XmlDataDocument GenerateXML()
        {
            XmlDataDocument xmlDoc = new XmlDataDocument();
            XmlElement xmlElementReport = xmlDoc.CreateElement("Report");

            XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "iso-8859-1", "yes");
            xmlDoc.AppendChild(xmlDec);

            xmlElementReport.AppendChild(GenerateHeaderNode(xmlDoc));
            xmlElementReport.AppendChild(GenerateDataNode(xmlDoc));

            xmlDoc.AppendChild(xmlElementReport);

            return xmlDoc;
        }
        #endregion

        #region métodos públicos
        /// <summary>
        /// Carga el contenido del Data reader.
        /// </summary>
        /// <param name="dr"></param>
        public void LoadData(DataTable dr)
        {
            // mientras que no encontremos otra forma nos cargamos el data reader 
            // de forma que no leamos el original, así nos vale
            // si el original ya viene leido y para cuando lo leamos
            // no lleguemos al final y se cirre el original
            //DataTable dt = new DataTable();
            //dt.Load(dr);
            this._dataReader = dr.CreateDataReader();
        }

        /// <summary>
        /// Genera el documento excel y lo escribe en la salida
        /// Hace un response.end al llamarle.
        /// </summary>
        /// <param name="response"></param>
        public void GenerateExcel(HttpResponse response)
        {
            // ponemos el conent-type
            response.ContentType = "application/vnd.ms-excel";
            response.Charset = "";

            // generamos el XML
            XmlDataDocument xml = GenerateXML();

            // cargamos la plantilla
            XslCompiledTransform xt = new XslCompiledTransform();
            xt.Load(this._xslTemplate);

            // transformamos el xml y escribimos el resultado en la salida
            xt.Transform(xml, null, response.OutputStream);
        }
        #endregion
    }
}
