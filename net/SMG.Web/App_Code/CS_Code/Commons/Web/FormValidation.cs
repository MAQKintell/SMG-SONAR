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
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.DataAccess;



namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Descripción breve de FormValidation
    /// </summary>
    public class FormValidation
    {
        public const string MENSAJE_CAMPO_INFORMADO = "El campo debe ser informado";

        public static Boolean TextBoxHasValue(TextBox txt)
        {
            if (txt != null && txt.Text != null && txt.Text.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean TextBoxHasValue(TextBox txt, Boolean mandatory, BaseValidator validator)
        {
            // Si es obligatorio y no tiene valor: error
            if (mandatory && !FormValidation.TextBoxHasValue(txt))
            {
                validator.ToolTip = "El campo debe ser informado";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static Boolean ComboBoxHasValue(DropDownList cmb, BaseValidator validator)
        {
            if (cmb == null)
            {
                validator.ToolTip = "Debe estar seleccionada una opción";
                return false;
            }
            if (cmb.SelectedIndex == -1)
            {
                validator.ToolTip = "Debe estar seleccionada una opción";
                return false;
            }
            else
            {
                if (cmb.SelectedItem.Value.Equals(Resources.ArchitectureConfiguration.ComboDefaultItemValue))
                {
                    validator.ToolTip = "Debe estar seleccionada una opción";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static Boolean ValidateDateTextBox(TextBox txt, Boolean mandatory, BaseValidator validator)
        {
            // Si es obligatorio y no tiene valor: error
            if (mandatory && !FormValidation.TextBoxHasValue(txt))
            {
                validator.ToolTip = "La fecha debe ser informada";
                return false;
            }

            // Si NO es obligatorio y no tiene valor: está bien, no hay
            // que comprobar el formato
            if (!FormValidation.TextBoxHasValue(txt) && !mandatory)
            {
                return true;
            }
            
            // Llegado a este punto la fecha sí tiene valor, por lo que
            // hay que comprobar su formato.
            if (!DateUtils.IsDateTimeHTML(txt.Text))
            {
                validator.ToolTip = "Formato de fecha incorrecto";
                return false;
            }
            else
            {
                return true;
            }

        }

        public static Boolean ValidateDateTextBoxTo(TextBox txtFrom, TextBox txtTo, Boolean mandatory, BaseValidator validator)
        {
            if (FormValidation.ValidateDateTextBox(txtTo, true, validator))
            {
                if (FormValidation.TextBoxHasValue(txtFrom) && DateUtils.IsDateTimeHTML(txtFrom.Text))
                {
                    DateTime fechaDesde = DateUtils.ParseDateTime(txtFrom.Text);
                    DateTime fechaHasta = DateUtils.ParseDateTime(txtTo.Text);
                    if (fechaHasta.CompareTo(fechaDesde) >= 0)
                    {
                        return true;
                    }
                    else
                    {    
                        validator.ToolTip = "La fecha hasta debe ser mayor o igual que la fecha desde.";
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                // no hace falta poner el texto porque ya lo ha puesto la llamada a ValidateDateTextBox
                return false;
            }
        }

        public static Boolean ValidateIntegerTextBox(TextBox txt, Boolean mandatory, BaseValidator validator)
        {
            // Si es obligatorio y no tiene valor: error
            if (mandatory && !FormValidation.TextBoxHasValue(txt))
            {
                validator.ToolTip = "El campo debe ser informado";
                return false;
            }

            // Si NO es obligatorio y no tiene valor: está bien, no hay
            // que comprobar el formato
            if (!FormValidation.TextBoxHasValue(txt) && !mandatory)
            {
                return true;
            }

            // Llegado a este punto la fecha sí tiene valor, por lo que
            // hay que comprobar su formato.
            if (!NumberUtils.IsInteger(txt.Text))
            {
                validator.ToolTip = "Formato de número incorrecto";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static Boolean ValidateNumberTextBox(TextBox txt, Boolean mandatory, BaseValidator validator)
        {
            // Si es obligatorio y no tiene valor: error
            if (mandatory && !FormValidation.TextBoxHasValue(txt))
            {
                validator.ToolTip = "El campo debe ser informado";
                return false;
            }

            // Si NO es obligatorio y no tiene valor: está bien, no hay
            // que comprobar el formato
            if (!FormValidation.TextBoxHasValue(txt) && !mandatory)
            {
                return true;
            }

            // Llegado a este punto la fecha sí tiene valor, por lo que
            // hay que comprobar su formato.
            if (!NumberUtils.IsDecimal(txt.Text))
            {
                validator.ToolTip = "Formato de número incorrecto";
                return false;
            }
            else
            {
                return true;
            }

        }

        public static Boolean ValidateNumberTextBox(TextBox txt, int integerLenght, int decimalLenght, Boolean mandatory, BaseValidator validator)
        {
            // Si es obligatorio y no tiene valor: error
            if (mandatory && !FormValidation.TextBoxHasValue(txt))
            {
                validator.ToolTip = "El campo debe ser informado";
                return false;
            }

            // Si NO es obligatorio y no tiene valor: está bien, no hay
            // que comprobar el formato
            if (!FormValidation.TextBoxHasValue(txt) && !mandatory)
            {
                return true;
            }

            // Llegado a este punto la fecha sí tiene valor, por lo que
            // hay que comprobar su formato.
            if (!NumberUtils.IsNumber(txt.Text,integerLenght,decimalLenght))
            {
                validator.ToolTip = "Formato de número incorrecto";
                return false;
            }
            else
            {
                return true;
            }

        }
        public static Boolean ValidateNumberAndLongTextBox(TextBox txt, Boolean mandatory, BaseValidator validator, Decimal longmax)
        {
            // Si es obligatorio y no tiene valor: error
            if (mandatory && !FormValidation.TextBoxHasValue(txt))
            {
                validator.ToolTip = "El campo debe ser informado";
                return false;
            }

            // Si NO es obligatorio y no tiene valor: está bien, no hay
            // que comprobar el formato
            if (!FormValidation.TextBoxHasValue(txt) && !mandatory)
            {
                return true;
            }
            if (txt.Text.Length != longmax)
            {
                validator.ToolTip = "Debe ser un número de 5 dígitos";
                return false;
            }

            // Llegado a este punto la fecha sí tiene valor, por lo que
            // hay que comprobar su formato.
      
                 if (!NumberUtils.IsDecimal(txt.Text))
                 {
                     validator.ToolTip = "Formato de número incorrecto";
                    return false;
                 }
                 else
                 {
                    return true;
                 }
           

        }

        public static Boolean ValidateNumberEstadoTextBox(TextBox txt, Boolean mandatory, BaseValidator validator,Boolean estado)
        {
           
            // Si es obligatorio y no tiene valor: error
            if (mandatory && !FormValidation.TextBoxHasValue(txt))
            {
                validator.ToolTip = "El campo debe ser informado";
                return false;
            }

            // Si NO es obligatorio y no tiene valor: está bien, no hay
            // que comprobar el formato
            if (!FormValidation.TextBoxHasValue(txt) && !mandatory)
            {
                return true;
            }
            
            if (estado == false)
            {
                validator.ToolTip = "El contrato no está dado de baja";
                return false;
            }

            // Llegado a este punto la fecha sí tiene valor, por lo que
            // hay que comprobar su formato.
            if (!NumberUtils.IsDecimal(txt.Text))
            {
                validator.ToolTip = "Formato de número incorrecto";
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
