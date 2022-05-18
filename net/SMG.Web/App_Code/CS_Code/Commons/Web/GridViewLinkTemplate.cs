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

namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Descripción breve de GridViewLinkTemplate
    /// </summary>
    public class GridViewLinkTemplate : ITemplate

    {
        private DataControlRowType templateType;
        private String columnCaption;
        private String columnName;
        private FrmBaseListado formulario;

        public String ColumName
        {
            get { return this.columnName; }
        }

        public String ColumnCaption
        {
            get { return this.columnCaption; }
        }

        public GridViewLinkTemplate(DataControlRowType type, string colname, string colcaption, FrmBaseListado form)
        {
            templateType = type;
            columnName = colname;
            columnCaption = colcaption;
            formulario = form;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            // Create the content for the different row types.
            switch (templateType)
            {
                case DataControlRowType.Header:
                    // Create the controls to put in the header
                    // section and set their properties.
                    Literal lc = new Literal();
                    lc.Text = columnCaption;

                    // Add the controls to the Controls collection
                    // of the container.
                    container.Controls.Add(lc);
                    break;
                case DataControlRowType.DataRow:
                    // Create the controls to put in a data row
                    // section and set their properties.
                    LinkButton lnkBtn = new LinkButton();
                    HyperLink hyperLink = new HyperLink();


                    // To support data binding, register the event-handling methods
                    // to perform the data binding. Each control needs its own event
                    // handler.
                    hyperLink.DataBinding += new EventHandler(this.HyperLink_DataBinding);
                    container.Controls.Add(hyperLink);
                    break;

                default:
                    // Insert code to handle unexpected values.
                    break;
            }
        }

        private void Link_DataBinding(Object sender, EventArgs e)
        {
            // Get the Label control to bind the value. The Label control
            // is contained in the object that raised the DataBinding 
            // event (the sender parameter).
            LinkButton l = (LinkButton)sender;

            // Get the GridViewRow object that contains the Label control. 
            GridViewRow row = (GridViewRow)l.NamingContainer;

            // Get the field value from the GridViewRow object and 
            // assign it to the Text property of the Label control.
            l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
            l.OnClientClick = "return OnDataGridLink_ClientClick('" + DataBinder.Eval(row.DataItem, columnName).ToString() + "');";
        }

        private void HyperLink_DataBinding(Object sender, EventArgs e)
        {
            // Get the Label control to bind the value. The Label control
            // is contained in the object that raised the DataBinding 
            // event (the sender parameter).
            HyperLink l = (HyperLink)sender;

            // Get the GridViewRow object that contains the Label control. 
            GridViewRow row = (GridViewRow)l.NamingContainer;

            // Get the field value from the GridViewRow object and 
            // assign it to the Text property of the Label control.
            l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
            l.NavigateUrl = "#";
            l.Attributes.Add("onclick", "return OnDataGridLink_ClientClick('" + DataBinder.Eval(row.DataItem, columnName).ToString() + "');");
        }
    }
}