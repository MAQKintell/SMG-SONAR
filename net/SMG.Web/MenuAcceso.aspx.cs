using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Iberdrola.SMG.BLL;
using System.Collections.Generic;
using Iberdrola.Commons.Web;
using Iberdrola.Commons.Security;
using Iberdrola.SMG.DAL.DTO;



using System.IO;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Threading;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.UI;

public partial class MenuAcceso : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            UsuarioDTO usuarioDTO = null;
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                Boolean UsuarioAutentificado = (Boolean)(Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_USUARIO_AUTENTIFICADO));
                if (UsuarioAutentificado == true)
                {
                    lblUsuarioConectado.Text = (string)usuarioDTO.Nombre;
                }
            }
            catch(Exception)
            {
                Response.Redirect("./Login.aspx");
            }

            try
            {
                CurrentSession.SetAttribute(CurrentSession.SESSION_ID_PERFIL, (Int32)usuarioDTO.Id_Perfil);
                List<MenuOption> listaMenus = Menus.ObtenerMenu(0, (Int32)usuarioDTO.Id_Perfil, usuarioDTO.Permiso,usuarioDTO.IdIdioma);
                this.treeViewMenu.CssClass = "linkFormulario";
                if (listaMenus == null || listaMenus.Count == 0)
                {
                    lblErrorPerfil.Text = "No existen opciones para el perfil del usuario";
                }
                else
                {
                    //CurrentSession.SetAttribute(CurrentSession.SESSION_VECES_PASADO, "0");
                    //Session("VecesPasado") = 0;
                    CurrentSession.SetAttribute(CurrentSession.SESSION_MENU_HIJO, false);

                    // 18/05/2016 (KINTELL) ESTABLECEMOS LA RUTA DONDE DEJAMOS LOS DOCUMENTOS DEL CIERRE DE LAS VISITAS.
                    CurrentSession.SetAttribute(CurrentSession.SESSION_RUTA_DOCUMENTO_CIERRE_VISITAS,Documentos.ObtenerRutaFicheros());

                    HiddenField1.Value = "<script language=javascript>";
                    CrearArbol(listaMenus, this.treeViewMenu.Nodes);
                    HiddenField1.Value +="</Script>";
                    Page.RegisterStartupScript("FocusScript", HiddenField1.Value);
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("./Login.aspx");
            }
            //20200226 BGN BEG R#22129 Nueva pantalla con solicitudes pendientes de tramitar para el proveedor
            try
            {
                CargarSolicitudesAbiertas(usuarioDTO);
            }
            catch (Exception ex1)
            {
                divImagenLogo.Visible = true;
                divSolicitudesAbiertas.Visible = false;
            }
            //20200226 BGN END R#22129 Nueva pantalla con solicitudes pendientes de tramitar para el proveedor

            //20200528 BGN BEG R#22132 Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
            try
            {
                bool permisoGestionDocumental = false;
                string expediente = usuarioDTO.Login;
                ConfiguracionDTO confExpedientesGestionDoc = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.EXPEDIENTES_GESTION_DOCUMENTAL);
                if (confExpedientesGestionDoc != null && !string.IsNullOrEmpty(confExpedientesGestionDoc.Valor))
                {
                    string[] expedientes = confExpedientesGestionDoc.Valor.Split(';');
                    if (expedientes.Contains(expediente))
                        permisoGestionDocumental = true;
                }
                if (permisoGestionDocumental)
                    btnGestionDoc.Visible = true;
                else
                    btnGestionDoc.Visible = false;

            }
            catch (Exception ex2)
            {
                btnGestionDoc.Visible = false;
            }
            //20200528 BGN END R#22132 Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental

        }

    }

    protected void MenuAlternativo(String Menu,String URL,String Leyenda)
    {
        //SavePageStateToPersistenceMedium (
        //ViewState
        //String Script = "<script language='JavaScript'>";
        //int i = 0;
        //for (i = 0; i <= listaMenus.Count - 1; i++)
        //{
        Boolean Hijo = (bool)CurrentSession.GetAttribute(CurrentSession.SESSION_MENU_HIJO);
        switch (Menu)//listaMenus[i].Text)
            {
                case "Formularios":
                    HiddenField1.Value += "element = document.getElementById('2');\r\n";
                    HiddenField1.Value += "element.style.visibility='visible';\r\n";
                    //HiddenField1.Value += "element.onClick='" + Url + "';";
                    HiddenField1.Value += "element.title='" + Leyenda + "';\r\n";
                    if (URL.Substring(0, 6).ToString().ToUpper() == "WINDOW")
                    {
                        HiddenField1.Value += "document.getElementById('2').attachEvent('onclick',function() {" + URL + ";})";
                    }
                    else
                    {
                        HiddenField1.Value += "document.getElementById('2').attachEvent('onclick',function() {document.URL ='" + URL + "';})";
                    }
                    HiddenField1.Value += "\r\n";

                    if (Hijo) HiddenField1.Value += "document.getElementById('Label2').style.color='Blue';\r\n";

                    break;
                case "Proveedores"://"Web Teléfono":
                    HiddenField1.Value += "element1 = document.getElementById('3');\r\n";
                    HiddenField1.Value += "element1.style.visibility='visible';\r\n";
                    HiddenField1.Value += "element1.title='" + Leyenda + "';\r\n";
                    if (URL.Substring(0, 6).ToString().ToUpper() == "WINDOW")
                    {
                        HiddenField1.Value += "document.getElementById('3').attachEvent('onclick',function() {" + URL + ";})";
                    }
                    else
                    {
                        HiddenField1.Value += "document.getElementById('3').attachEvent('onclick',function() {document.URL ='" + URL + "';})";
                    }
                    HiddenField1.Value += "\r\n";
                    if (Hijo) HiddenField1.Value += "document.getElementById('Label3').style.color='Blue';\r\n";
                    break;
                case "Solicitudes"://"Proveedores":
                    HiddenField1.Value += "element2 = document.getElementById('4');\r\n";
                    HiddenField1.Value += "element2.style.visibility='visible';\r\n";
                    HiddenField1.Value += "element2.title='" + Leyenda + "';\r\n";
                    if (URL.Substring(0, 6).ToString().ToUpper() == "WINDOW")
                    {
                        HiddenField1.Value += "document.getElementById('4').attachEvent('onclick',function() {" + URL + ";})";
                    }
                    else
                    {
                        HiddenField1.Value += "document.getElementById('4').attachEvent('onclick',function() {document.URL ='" + URL + "';})";
                    }
                    HiddenField1.Value += "\r\n";
                    if (Hijo) HiddenField1.Value += "document.getElementById('Label4').style.color='Blue';\r\n";
                    break;
                case "Datos Argumentario"://"Solicitudes":
                    HiddenField1.Value += "element3 = document.getElementById('5');\r\n";
                    HiddenField1.Value += "element3.style.visibility='visible';\r\n";
                    HiddenField1.Value += "element3.title='" + Leyenda + "';\r\n";
                    //if (URL.Substring(0, 6).ToString().ToUpper() == "WINDOW")
                    //{
                        HiddenField1.Value += "document.getElementById('5').attachEvent('onclick',function() {" + URL + ";})";
                    //}
                    //else
                    //{
                    //    HiddenField1.Value += "document.getElementById('5').attachEvent('onclick',function() {document.URL ='" + URL + "';})";
                    //}
                    HiddenField1.Value += "\r\n";
                    if (Hijo) HiddenField1.Value += "document.getElementById('Label5').style.color='Blue';\r\n";
                    break;
                case "Envio SMS"://"Datos Argumentario":
                    HiddenField1.Value += "element4 = document.getElementById('6');\r\n";
                    HiddenField1.Value += "element4.style.visibility='visible';\r\n";
                
                    //HiddenField1.Value += "element4.style.left=500px;\r\n";

                    HiddenField1.Value += "element4.title='" + Leyenda + "';\r\n";
                    if (URL.Substring(0, 6).ToString().ToUpper() == "WINDOW")
                    {
                        HiddenField1.Value += "document.getElementById('6').attachEvent('onclick',function() {" + URL + ";})";
                    }
                    else
                    {
                        HiddenField1.Value += "document.getElementById('6').attachEvent('onclick',function() {document.URL ='" + URL + "';})";
                    }
                    HiddenField1.Value += "\r\n";
                    if (Hijo) HiddenField1.Value += "document.getElementById('Label6').style.color='Blue';\r\n";
                    break;
                //case "Apartado para el administrador":
                //    HiddenField1.Value += "element5 = document.getElementById('7');";
                //    HiddenField1.Value += "element5.style.visibility='visible';";
                //    HiddenField1.Value += "document.getElementById('7').attachEvent('onclick',function() {document.URL ='" + URL + "';})";
                //    HiddenField1.Value += "\r\n";
                //    break;
                //case "Envio SMS":
                //    HiddenField1.Value += "element6 = document.getElementById('8');";
                //    HiddenField1.Value += "element6.style.visibility='visible';";
                //    HiddenField1.Value += "document.getElementById('8').attachEvent('onclick',function() {document.URL ='" + URL + "';})";
                //    HiddenField1.Value += "\r\n";
                //    break;
            }
            //HiddenField1.Value += "element.onClick='" + listaMenus[i].LinkUrl + "';";
        //}
        //Script += "</script>";
        //Page.RegisterStartupScript("FocusScript", Script);
    }

    protected void btnDesconectar_Click(object sender, EventArgs e)
    {
        HttpContext.Current.User = null;

        CurrentSession.Clear();

        NavigationController.ClearHistory();
        Response.Redirect("./Login.aspx");
    }

    private void CrearArbol(List<MenuOption> listaOpciones, TreeNodeCollection nodeCollection)
    {
        foreach (MenuOption menu in listaOpciones)
        {
            TreeNode newNode = new TreeNode();
            newNode.Text = menu.Text;
            newNode.NavigateUrl = menu.LinkUrl;
            newNode.ToolTip = menu.Description;
            //newNode.ImageUrl = "~/Imagenes/flecha.gif";
            nodeCollection.Add(newNode);
            ////CurrentSession.SetAttribute(CurrentSession.SESSION_VECES_PASADO, (int)CurrentSession.GetAttribute(CurrentSession.SESSION_VECES_PASADO)+1);// = Session("VecesPasado")+1;

            if (menu.Children != null && menu.Children.Count > 0)
            {
                CrearArbol(menu.Children, newNode.ChildNodes);
            }

            //if (menu.Order.Contains("."))
            //{
            //    UsuarioDTO usuarioDTO = null;
            //    AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            //    usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            //    List<MenuOption> listaMenus = Menus.ObtenerMenu(0, (Int32)usuarioDTO.Id_Perfil);
            //    string[] Valores = menu.Order.ToString().Split('.');

            //    int i;
            //    for (i = 0; i <= listaMenus.Count - 1; i++)
            //    {
            //        if (listaMenus[i].Order.ToString() == Valores[0])
            //        {
            //            //Encontrado el padre.
            //            menu.Description = menu.Description + " PERTENECIENTE A: " + listaMenus[i].Text;
            //        }
            //    }
            //    //Valores[0];
            //    //CurrentSession.SetAttribute(CurrentSession.SESSION_CODIGO_PADRE_MENU, Valores[0]);
            //    CurrentSession.SetAttribute(CurrentSession.SESSION_MENU_HIJO, true);// = Session("VecesPasado")+1;
            //}

            //MenuAlternativo(menu.Text, menu.LinkUrl, menu.Description);
            //CurrentSession.SetAttribute(CurrentSession.SESSION_MENU_HIJO, false);
        }
    }

    //20200226 BGN BEG R#22129 Nueva pantalla con solicitudes pendientes de tramitar para el proveedor
    private void CargarSolicitudesAbiertas(UsuarioDTO user)
    {
        string proveedor = string.Empty;
        bool esTelefono = false;
        if (user != null)
        {
            //Siempre va a venir el código del perfil informado                
            if (user.Id_Perfil.HasValue)
            {
                int idPerfil = (int)user.Id_Perfil.Value;
                //Solo nos interesa el codigo de proveedor si es perfil proveedor
                if (Usuarios.EsProveedor(idPerfil))
                {
                    if (!string.IsNullOrEmpty(user.CodProveedor))
                    {
                        proveedor = user.CodProveedor;
                    }
                }
                else if (Usuarios.EsTelefono(idPerfil))
                {
                    esTelefono = true;
                }
            }
            //Si es telefono no recuperamos las solicitudes, si es proveedor recuperamos las suyas y si es administrador dejamos proveedor vacío para recuperar las de todos
            if (!esTelefono)
            {
                SolicitudDB objSol = new SolicitudDB();
                DataTable dtSolicitudes = objSol.ObtenerSolicitudesAbiertasPorProveedor(proveedor, user.IdIdioma);
                if (dtSolicitudes.Rows.Count > 0)
                {
                    gvSolicitudes.DataSource = dtSolicitudes;
                    gvSolicitudes.DataBind();

                    divImagenLogo.Visible = false;
                    divSolicitudesAbiertas.Visible = true;
                }
                else
                {
                    divImagenLogo.Visible = true;
                    divSolicitudesAbiertas.Visible = false;
                    this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SIN_SOLICITUDES_EN_CURSO);
                }
            }
            else
            {
                divImagenLogo.Visible = true;
                divSolicitudesAbiertas.Visible = false;
            }
        }
    }

    protected void gvSolicitudes_DataBinding(object sender, EventArgs e)
    {
        int height = 20;
        int numRows = gvSolicitudes.Rows.Count;
        if (numRows <= 30)
        {
            height = height * numRows;
            gvSolicitudes.Height = Unit.Pixel(height);
        }
        else
        {
            gvSolicitudes.Height = Unit.Percentage(100);
        }        
    }

    private void MostrarMensaje(String Mensaje)
    {
        string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

        ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
    }
    //20200226 BGN END R#22129 Nueva pantalla con solicitudes pendientes de tramitar para el proveedor

    //20200528 BGN BEG R#22132 Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
    protected void btnGestionDoc_Click(object sender, EventArgs e)
    {
        Response.Redirect("./UI/FrmGestionDocumental.aspx");
    }
    //20200528 BGN END R#22132 Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
}
