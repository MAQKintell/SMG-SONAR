using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Iberdrola.Commons.Exceptions;

using System.Resources;
using System.Globalization;
using System.IO;

namespace Iberdrola.Commons.Web
{

    /// <summary>
    /// Descripción breve de AsignarCultura
    /// </summary>
    public class AsignarCultura
    {

        //public void Prueba()
        //{
        //    String a = "";
        //}

        //public void RecControles(Control control, Page Pagina)
        //{
        //    String A = "";
        //    //Recorremos con un ciclo for each cada control que hay en la colección Controls
        //    foreach (Control contHijo in control.Controls)
        //    {
        //        //Preguntamos si el control tiene uno o mas controles dentro del mismo con la propiedad 'HasChildren'
        //        //Si el control tiene 1 o más controles, entonces llamamos al procedimiento de forma recursiva, para que siga recorriendo los demás controles 
        //        if (contHijo.HasControls()) { RecControles(contHijo,Pagina); }
        //        Proceso(contHijo, Pagina);
        //    }
        //}

        //private void Proceso(Control childc,Page Pagina)
        //{
        //    CultureInfo culture;
        //    ResourceManager manager;
        //    culture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0]);//"FR-fr");
        //    manager = (ResourceManager)Resources.Cultura.ResourceManager;

        //    String[] Nombre = ((System.Web.UI.TemplateControl)(Pagina)).AppRelativeVirtualPath.ToString().Split('/');

        //    if (childc is RequiredFieldValidator)
        //    {
        //        RequiredFieldValidator a = (RequiredFieldValidator)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + ((System.Web.UI.Control)(a)).ID.ToString();
        //        EscribirRecursos(Variable, a.ErrorMessage);
        //        a.ErrorMessage = manager.GetString(Variable, culture);
        //    }

        //    if (childc is Label)
        //    {
        //        Label a = (Label)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + ((System.Web.UI.Control)(a)).ID.ToString();
        //        EscribirRecursos(Variable, a.Text);
        //        a.Text = manager.GetString(Variable, culture);

        //        EscribirRecursos(Variable + "_ToolTip", a.ToolTip);
        //        a.ToolTip = manager.GetString(Variable + "_ToolTip", culture);
        //    }

        //    if (childc is Button)
        //    {
        //        Button b = (Button)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + ((System.Web.UI.Control)(b)).ID.ToString();
        //        EscribirRecursos(Variable, b.Text);
        //        b.Text = manager.GetString(Variable, culture);

        //        EscribirRecursos(Variable + "_ToolTip", b.ToolTip);
        //        b.ToolTip = manager.GetString(Variable + "_ToolTip", culture);
        //    }

        //    if (childc is ImageButton)
        //    {
        //        ImageButton b = (ImageButton)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + ((System.Web.UI.Control)(b)).ID.ToString();
        //        EscribirRecursos(Variable + "_ToolTip", b.ToolTip);
        //        b.ToolTip = manager.GetString(Variable + "_ToolTip", culture);
        //    }

        //    if (childc is HtmlTitle)
        //    {
        //        HtmlTitle b = (HtmlTitle)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_TITULO";// ((System.Web.UI.Control)(b)).ID.ToString();
        //        EscribirRecursos(Variable, b.Text);
        //        b.Text = manager.GetString(Variable, culture);
        //    }

        //    if (childc is CheckBox)
        //    {
        //        CheckBox b = (CheckBox)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + ((System.Web.UI.Control)(b)).ID.ToString();
        //        EscribirRecursos(Variable, b.Text);
        //        b.Text = manager.GetString(Variable, culture);

        //        EscribirRecursos(Variable + "_ToolTip", b.ToolTip);
        //        b.ToolTip = manager.GetString(Variable + "_ToolTip", culture);
        //    }

        //    if (childc is HtmlButton)
        //    {
        //        HtmlButton b = (HtmlButton)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + ((System.Web.UI.Control)(b)).ID.ToString();
        //        EscribirRecursos(Variable, b.InnerText);
        //        b.InnerText = manager.GetString(Variable, culture);

        //    }

        //    if (childc is HtmlInputButton)
        //    {
        //        HtmlInputButton b = (HtmlInputButton)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + ((System.Web.UI.Control)(b)).ID.ToString();
        //        EscribirRecursos(Variable, b.Value);
        //        b.Value = manager.GetString(Variable, culture);

        //    }

        //    if (childc is RadioButtonList)
        //    {
        //        RadioButtonList b1 = (RadioButtonList)childc;
        //        for (int i = 0; i < b1.Items.Count; i++)
        //        {

        //            String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + b1.Items[i].Text;//((System.Web.UI.Control)(b1.Items[i])).ID.ToString();
        //            EscribirRecursos(Variable, b1.Items[i].Text);
        //            b1.Items[i].Text = manager.GetString(Variable, culture);
        //        }

        //    }

        //    if (childc is Panel)
        //    {
        //        Panel b = (Panel)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + ((System.Web.UI.Control)(b)).ID.ToString();
        //        EscribirRecursos(Variable, b.GroupingText);
        //        b.GroupingText = manager.GetString(Variable, culture);
        //    }

        //    if (childc is LinkButton)
        //    {
        //        LinkButton a = (LinkButton)childc;
        //        String Variable = Nombre[Nombre.Length - 1].ToString().Replace(".", "_") + "_" + ((System.Web.UI.Control)(a)).ID.ToString();
        //        EscribirRecursos(Variable, a.Text);
        //        a.Text = manager.GetString(Variable, culture);

        //        EscribirRecursos(Variable + "_ToolTip", a.ToolTip);
        //        a.ToolTip = manager.GetString(Variable + "_ToolTip", culture);
        //    }

        //}
        //public void Asignar(Page Pagina)
        //{
        //    foreach (Control c in Pagina.Controls)
        //    {
        //        RecControles(c,Pagina);
        //    }
        //    //EscribirFinal();
        //    //EscribirFinalFrances();
        //}

        protected void EscribirFinal()
        {
            StreamWriter sw = null;
            string strNombreCompletoFichero = null;
            StreamReader streamReader = null;

            try
            {
                strNombreCompletoFichero = "C:\\Proyectos\\IBERDR13\\SMG\\DESARROLLO\\SMG.Web\\App_GlobalResources\\Cultura.resx.txt";
                strNombreCompletoFichero = "C:\\Cultura.resx.txt";

                // Open a file for reading

                streamReader = File.OpenText(strNombreCompletoFichero);
                // Now, read the entire file into a strin
                string contents = streamReader.ReadToEnd();
                streamReader.Close();


                StreamWriter streamWriter = File.CreateText(strNombreCompletoFichero);
                streamWriter.Write(contents.Replace("</root>", ""));
                streamWriter.Close();

                sw = new StreamWriter(strNombreCompletoFichero, true);
                sw.WriteLine("</root>");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // Si se ha creado el stream de escritura lo cerramos.
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }

                //EscribirRecursosFRANCES(Clave, Valor);
            }
        }

        protected void EscribirFinalFrances()
        {
            StreamWriter sw = null;
            string strNombreCompletoFichero = null;
            StreamReader streamReader = null;

            try
            {
                strNombreCompletoFichero = "C:\\Proyectos\\IBERDR13\\SMG\\DESARROLLO\\SMG.Web\\App_GlobalResources\\Cultura.FR-fr.resx.txt";
                strNombreCompletoFichero = "C:\\Cultura.FR-fr.resx.txt";

                // Open a file for reading

                streamReader = File.OpenText(strNombreCompletoFichero);
                // Now, read the entire file into a strin
                string contents = streamReader.ReadToEnd();
                streamReader.Close();


                StreamWriter streamWriter = File.CreateText(strNombreCompletoFichero);
                streamWriter.Write(contents.Replace("</root>", ""));
                streamWriter.Close();

                sw = new StreamWriter(strNombreCompletoFichero, true);
                sw.WriteLine("</root>");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // Si se ha creado el stream de escritura lo cerramos.
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }

                //EscribirRecursosFRANCES(Clave, Valor);
            }
        }

        protected void EscribirRecursos(String Clave, string Valor)
        {
            //return;
            StreamWriter sw = null;
            string strNombreCompletoFichero = null;
            StreamReader streamReader = null;

            try
            {
                strNombreCompletoFichero = "C:\\Proyectos\\IBERDR13\\SMG\\DESARROLLO\\SMG.Web\\App_GlobalResources\\Cultura.resx.txt";
                strNombreCompletoFichero = "C:\\Cultura.resx.txt";

                // Open a file for reading

                //streamReader = File.OpenText(strNombreCompletoFichero);
                // Now, read the entire file into a strin
                //string contents = streamReader.ReadToEnd();
                //streamReader.Close();


                //StreamWriter streamWriter = File.CreateText(strNombreCompletoFichero);
                //streamWriter.Write(contents.Replace("</root>", ""));
                //streamWriter.Close();



                sw = new StreamWriter(strNombreCompletoFichero, true);
                String strFila = "<data name='" + Clave + "' xml:space='preserve'><value>" + Valor + "</value></data>";
                sw.WriteLine(strFila.ToString());
                //sw.WriteLine("</root>");

            }
            catch (Exception ex)
            {
            }
            finally
            {
                // Si se ha creado el stream de escritura lo cerramos.
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }

                EscribirRecursosFRANCES(Clave, Valor);
            }
        }

        protected void EscribirRecursosFRANCES(String Clave, string Valor)
        {
            Valor = Valor + "_FRANCES";
            StreamWriter sw = null;
            string strNombreCompletoFicheroFRANCES = null;
            StreamReader streamReader = null;

            try
            {
                strNombreCompletoFicheroFRANCES = "C:\\Proyectos\\IBERDR13\\SMG\\DESARROLLO\\SMG.Web\\App_GlobalResources\\Cultura.FR-fr.resx.txt";
                strNombreCompletoFicheroFRANCES = "C:\\Cultura.FR-fr.resx.txt";

                // Open a file for reading

                //streamReader = File.OpenText(strNombreCompletoFicheroFRANCES);
                //// Now, read the entire file into a strin
                //string contents = streamReader.ReadToEnd();
                //streamReader.Close();
                //// Write the modification into the same fil
                //StreamWriter streamWriter = File.CreateText(strNombreCompletoFicheroFRANCES);
                //streamWriter.Write(contents.Replace("</root>", ""));
                //streamWriter.Close();



                sw = new StreamWriter(strNombreCompletoFicheroFRANCES, true);
                String strFila = "<data name='" + Clave + "' xml:space='preserve'><value>" + Valor + "</value></data>";
                sw.WriteLine(strFila.ToString());
                //sw.WriteLine("</root>");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // Si se ha creado el stream de escritura lo cerramos.
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
        }
    }
}