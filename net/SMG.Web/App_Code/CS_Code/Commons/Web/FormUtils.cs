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
using Iberdrola.Commons.Utils;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Configuration;
using System.Xml;

namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Descripción breve de FormUtils
    /// </summary>
    public class FormUtils
    {
        public FormUtils()
        {
           
        }

        /// <summary>
        /// Carga un data reader en un datagrid, añadiendo las columnas de forma dinámica.
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="dr"></param>
        public static void CargarDataGrid(GridView grd, IDataReader dr, FrmBaseListado form)
        {
            CargarDataGrid(grd, null, dr, form);
        }

        public static void CargarDataGrid(GridView grd, GridView grdCabecera, IDataReader dr, FrmBaseListado form)
        {


            //eliminamos las columnas
            grd.Columns.Clear();
            grdCabecera.Columns.Clear();

            if (dr != null)
            {
                DataTable dt = dr.GetSchemaTable();
                foreach (DataRow drow in dt.Rows)
                {
                    BoundField bc = new BoundField();
                    bc.HeaderText = (String)drow[0];
                    bc.SortExpression = (String)drow[0];
                    bc.DataField = (String)drow[0];
                    grd.Columns.Add(bc);


                    BoundField bcCabecera = new BoundField();
                    bcCabecera.HeaderText = (String)drow[0];
                    bcCabecera.SortExpression = (String)drow[0];
                    bcCabecera.DataField = (String)drow[0];
                    grdCabecera.Columns.Add(bcCabecera);
                }

                grdCabecera.DataSource = dr;
                grdCabecera.DataBind();

            }
        }

        public static void CargarDataGrid(GridView grd, GridView grdCabecera, DataTable dt, FrmBaseListado form)
        {
            int j = 0;
            String ColumnName;
            Int32 ColumnWidth;
            String ColumnStyle;
            Boolean IsVisible;
            grd.Columns.Clear();
            grdCabecera.Columns.Clear();

            Int32 gridWidth = 0;

            int _temp = 0;
            try
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    _temp += 1;
                    ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla(grd.ID,dc.ColumnName);
                    ColumnName = columnDefinition.Name;
                    ColumnWidth = columnDefinition.Width;
                    ColumnStyle = columnDefinition.Style;

                    gridWidth += ColumnWidth;

                    BoundField bc = new BoundField();
                    bc.HeaderText = ColumnName;
                    bc.SortExpression = (String)dc.ColumnName;
                    bc.DataField = (String)dc.ColumnName;
                    bc.HeaderStyle.Width = Unit.Pixel(ColumnWidth);
                    bc.ItemStyle.Width = Unit.Pixel(ColumnWidth);
                    bc.ItemStyle.CssClass = ColumnStyle;
                    FormUtils.FormatearColumnaDataGrid(dc, ref bc);
                    grd.Columns.Add(bc);

                    BoundField bcHeader = new BoundField();
                    bcHeader.HeaderText = ColumnName;
                    bcHeader.SortExpression = (String)dc.ColumnName;
                    bcHeader.DataField = (String)dc.ColumnName;
                    bcHeader.HeaderStyle.Width = Unit.Pixel(ColumnWidth);
                    bcHeader.ItemStyle.Width = Unit.Pixel(ColumnWidth);
                    FormUtils.FormatearColumnaDataGrid(dc, ref bcHeader);
                    grdCabecera.Columns.Add(bcHeader);
                }

                grd.DataSource = dt;
                grd.DataBind();
                grd.Width = Unit.Pixel(gridWidth);

                grdCabecera.DataSource = dt;
                grdCabecera.DataBind();
                grdCabecera.Width = Unit.Pixel(gridWidth);
            }
            catch (Exception)
            {
                throw;
            }
        }


        private static void FormatearColumnaDataGrid(DataColumn dc, ref BoundField bc)
        {
            bc.ItemStyle.Wrap = false;
            bc.HeaderStyle.Wrap = false;
            switch (dc.DataType.ToString())
            {
                case "System.DateTime":
                    bc.DataFormatString = "{0:d}";
                    break;
            }
        }   

        /// <summary>
        /// Agrega la opción por defecto a un combo, si es que no la tiene ya añadida.
        /// La inserta siempre en la primera posición.
        /// </summary>
        /// <param name="cmb"></param>
        public static void AddDefaultItem(DropDownList cmb)
        {
            ListItem li = new ListItem(Resources.ArchitectureConfiguration.ComboDefaultItemText, Resources.ArchitectureConfiguration.ComboDefaultItemValue);
            if (cmb.Items.FindByValue(Resources.ArchitectureConfiguration.ComboDefaultItemValue) == null)
            {
                cmb.Items.Insert(0, li);
            }
        }

        /// <summary>
        /// Indica si un combo tiene una opción seleccionada.
        /// Si la opción seleccionada es la opción por defecto retorna false también.
        /// </summary>
        /// <param name="cmb"></param>
        /// <returns></returns>
        public static Boolean HasValue(DropDownList cmb)
        {
            if (cmb == null)
            {
                return false;
            }
            if (cmb.SelectedIndex == -1)
            {
                return false;
            }
            else
            {
                if (cmb.SelectedItem.Value.Equals(Resources.ArchitectureConfiguration.ComboDefaultItemValue))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static Boolean HasValue(TextBox txt)
        {
            if (txt == null || txt.Text == null || txt.Text.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static Int32? GetInt(TextBox txt)
        {
            if (txt != null && txt.Text != null && txt.Text.Length > 0)
            {
                if (NumberUtils.IsInteger(txt.Text))
                {
                    return NumberUtils.GetInt(txt.Text);
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

        public static Decimal? GetDecimal(TextBox txt)
        {
            if (txt != null && txt.Text != null && txt.Text.Length > 0)
            {
                if (NumberUtils.IsInteger(txt.Text))
                {
                    return NumberUtils.GetInt(txt.Text);
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

        public static Decimal? GetDecimal(Label txt)
        {
            if (txt != null && txt.Text != null && txt.Text.Length > 0)
            {
                if (NumberUtils.IsInteger(txt.Text))
                {
                    return NumberUtils.GetInt(txt.Text);
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

        public static Decimal? GetDecimal(string txt)
        {
            if (txt != null && txt.Length > 0)
            {
                if (NumberUtils.IsDecimal(txt))
                {
                    return NumberUtils.GetDecimal(txt);
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

        public static Decimal? GetDecimal(DropDownList cmb)
        {
            if (HasValue(cmb))
            {
                if (NumberUtils.IsDecimal(cmb.SelectedItem.Value))
                {
                    return NumberUtils.GetDecimal(cmb.SelectedItem.Value);
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

        public static String GetString(TextBox txt)
        {
            if (txt != null )
            {
                return txt.Text;
            }
            else
            {
                return null;
            }
        }

        public static String GetString(Label txt)
        {
            if (txt != null)
            {
                return txt.Text;
            }
            else
            {
                return null;
            }
        }

        public static Int32? GetInt(DropDownList cmb)
        {
            if (HasValue(cmb))
            {
                if (NumberUtils.IsInteger(cmb.SelectedItem.Value))
                {
                    return NumberUtils.GetInt(cmb.SelectedItem.Value);
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

        public static Object GetValue(DropDownList cmb)
        {
            if (HasValue(cmb))
            {
                return cmb.SelectedItem.Value;
            }
            else
            {
                return null;
            }
        }

        public static String GetString(DropDownList cmb)
        {
            if (HasValue(cmb))
            {
                return cmb.SelectedItem.Text;
            }
            else
            {
                return null;
            }
        }

        public static Double? GetDouble(TextBox txt)
        {
            if (txt != null && txt.Text != null && txt.Text.Length > 0)
            {
                if (NumberUtils.IsNumber(txt.Text, 1, 2))
                {
                    return NumberUtils.GetDouble(txt.Text);
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

        public static DateTime? GetDateTime(TextBox txt)
        {
            if (txt != null && txt.Text != null && txt.Text.Length > 0)
            {
                if (Utils.DateUtils.IsDateTimeHTML(txt.Text))
                {
                    return Utils.DateUtils.ParseDateTime(txt.Text);
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

    }
}
