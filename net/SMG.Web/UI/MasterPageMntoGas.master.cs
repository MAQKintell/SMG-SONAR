using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Iberdrola.Commons.Security;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Web;
using Iberdrola.Commons.Logging;
using Iberdrola.SMG.DAL.DTO;

using Iberdrola.SMG.BLL;


namespace Iberdrola.SMG.UI
{
    public partial class MasterPageMntoGas : MasterPageBase
    {
        public MasterPageMntoGas()
        {

        }

        public override PlaceHolder PlaceHolderScript
        {
            get { return this._PlaceHolderScript; }
        }
     
        #region atributos
        private static string strVentanaActiva = "";
        #endregion

        #region propiedades
        public string VentanaActiva
        {
            set { MasterPageMntoGas.strVentanaActiva = value; }
            get { return MasterPageMntoGas.strVentanaActiva; }
        }

        #endregion

        #region metodos
        public bool EsVentanaActiva(string nombreVentana)
        {
            return MasterPageMntoGas.strVentanaActiva.Equals(nombreVentana);
        }
        #endregion

        #region metodos abstractos
        //public abstract List<String> ValidateForm();
        #endregion   

        #region Métodos de los eventos
        private void ValidarUsuario()
        {
            if (Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_USUARIO_AUTENTIFICADO) != null)
            {
                Boolean UsuarioAutentificado = (Boolean)(Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_USUARIO_AUTENTIFICADO));
                if (UsuarioAutentificado == true)
                {
                    AppPrincipal usuarioPrincipal = (AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                    lblUsuarioConectado.Text = (string) usuarioDTO.Nombre;
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
            else
            {
                Response.Redirect("../Login.aspx");
            }
        }

        private void DestacarOpcionMenuSeleccionada()
        {
            if (MasterPageMntoGas.strVentanaActiva.Equals("FrmContratos"))
            {
                this.btnContratos.CssClass = "botonMenuActivoPrimero";
                this.btnAccesoUltimaVisita.CssClass = "botonMenuNormal";
                this.btnConsultas.CssClass = "botonMenuNormal";
                this.btnCambioPassword.CssClass = "botonMenuNormal";
            }
            else if (MasterPageMntoGas.strVentanaActiva.Equals("FrmUltimaVisita"))
            {
                this.btnContratos.CssClass = "botonMenuNormalPrimero";
                this.btnAccesoUltimaVisita.CssClass = "botonMenuActivo";
                this.btnConsultas.CssClass = "botonMenuNormal";
                this.btnCambioPassword.CssClass = "botonMenuNormal";
            }
            else if (MasterPageMntoGas.strVentanaActiva.Equals("FrmConsultas"))
            {
                this.btnContratos.CssClass = "botonMenuNormalPrimero";
                this.btnAccesoUltimaVisita.CssClass = "botonMenuNormal";
                this.btnConsultas.CssClass = "botonMenuActivo";
                this.btnCambioPassword.CssClass = "botonMenuNormal";
            }
            else if (MasterPageMntoGas.strVentanaActiva.Equals("FrmCambioContrasenia"))
            {
                this.btnContratos.CssClass = "botonMenuNormalPrimero";
                this.btnAccesoUltimaVisita.CssClass = "botonMenuNormal";
                this.btnConsultas.CssClass = "botonMenuNormal";
                this.btnCambioPassword.CssClass = "botonMenuActivo";
            }
            else
            {
                this.btnContratos.CssClass = "botonMenuNormalPrimero";
                this.btnAccesoUltimaVisita.CssClass = "botonMenuNormal";
                this.btnConsultas.CssClass = "botonMenuNormal";
                this.btnCambioPassword.CssClass = "botonMenuNormal";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            if (!NavigationController.IsBackward())
            {
                // Lo primero que se hace es mirar si el usuario está autenticado
                // si no es así se redirige al login.
                ValidarUsuario();

                // Se destaca la opción del menú en la que estamos.
                DestacarOpcionMenuSeleccionada();

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

                    btnFacturacionVisitas.Visible = false;
                    if ((Int32)usuarioDTO.Id_Perfil == 4)
                    {
                        btnFacturacionVisitas.Visible = true;
                    }
                    //if (usuarioDTO.Id_Perfil == 4)
                    //{
                    //    btnFacturacionVisitas.Visible = true;
                    //}
                    //else
                    //{
                    //    btnFacturacionVisitas.Visible = false;
                    //}

                }
                catch (Exception)
                {
                    Response.Redirect("./Login.aspx");
                }

                //GenerarArbol();
            }

            
            
        }

        #region " Generar Arbol "
        //private void GenerarArbol()
        //{
        //    string Inicio;
        //    Inicio = "<div id='divContenidoMenu'>\r\n";
        //    Inicio += "<asp:ContentPlaceHolder id='ContentPlaceHolderMenu1' runat='server'>\r\n";
        //    Inicio += "<table cellpadding='0' cellspacing='0'>\r\n";
        //    Inicio += "<tbody>\r\n";
        //    Inicio += "<tr>\r\n";
        //    Inicio += "<td id='left'>\r\n";
        //    Inicio += "<table id='bsmenu'>\r\n";
        //    Inicio += "<tbody>\r\n";

        //    string Menu = ObtenerArbol();

        //    string Final;
        //    Final = "</tbody>\r\n";
        //    Final += "</table>\r\n";
        //    Final += "</div>\r\n";
        //    Final += "</tr>\r\n";
        //    Final += "</td>\r\n";
        //    Final += "</tr>\r\n";
        //    Final += "</tbody>\r\n";
        //    Final += "</table>\r\n";

        //    Final += "</div>\r\n";

        //    string ScriptInicio = "";
        //    ScriptInicio = "<script type='text/javascript'>\r\n";
        //    ScriptInicio += "var stretchers = $$('div.accordion');\r\n";
        //    ScriptInicio += "window.onload = function(){ //safari cannot get style if window isnt fully loaded\r\n";
        //    ScriptInicio += "var togglers = $$('div.bsmenu');	\r\n";
        //    ScriptInicio += "var myAccordion = new Fx.Accordion(togglers, stretchers, { opacity: false, start: false, transition: Fx.Transitions.quadOut	});\r\n";
        //    ScriptInicio += "//anchors\r\n";
        //    ScriptInicio += "function checkHash(){\r\n";
        //    ScriptInicio += "var found = false;\r\n";
        //    ScriptInicio += "$$('div.bsmenu a').each(function(link, i){\r\n";
        //    ScriptInicio += "if (window.location.hash.test(link.hash)){\r\n";
        //    ScriptInicio += "myAccordion.showThisHideOpen(i);\r\n";
        //    ScriptInicio += "found = true;\r\n";
        //    ScriptInicio += "}\r\n";
        //    ScriptInicio += "});\r\n";
        //    ScriptInicio += "return found;\r\n";
        //    ScriptInicio += "}\r\n";
        //    ScriptInicio += "if (!checkHash()) myAccordion.showThisHideOpen(0);};\r\n";
        //    ScriptInicio += "</script>\r\n";

        //    string Script = Inicio + Menu + Final + ScriptInicio;
        //    //Page.RegisterStartupScript("MENU", Script);
        //    ContentPlaceHolderMenu1.Controls.AddAt(0, new LiteralControl(Script));
        //    //ContentPlaceHolderMenu.Controls.Add(new LiteralControl(Script));
        //}
        //private string ObtenerArbol()
        //{
        //    string Menu = "";
        //    int i = 1;

        //    UsuarioDTO usuarioDTO = null;
        //    AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
        //    usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
        //    CurrentSession.SetAttribute(CurrentSession.SESSION_ID_PERFIL, (Int32)usuarioDTO.Id_Perfil);
        //    List<MenuOption> listaMenus = Menus.ObtenerMenu(0, (Int32)usuarioDTO.Id_Perfil,true);

        //    String opcionSeleccionada = this.hdnIdLinkSeleccionado.Value;
        //    if (opcionSeleccionada == null)
        //    {
        //        opcionSeleccionada = "";
        //    }

        //    for (i = 1; i <= listaMenus.Count; i++)
        //    {
        //        string Texto = listaMenus[i - 1].Text;
        //        string DescripcionPadre = listaMenus[i - 1].Description;

        //        //Padre.
        //        Menu += "<tr>\r\n";
        //        Menu += "<td class='bsmenu_item'>\r\n";
        //        Menu += "<div class='bsmenu'>";


        //        Menu += "<a class='bsmenu' title='" + DescripcionPadre + "'\r\n";



        //        Menu += "href='#" + i + "'>\r\n";
        //        Menu += "<table class='bsitem_menu_text'>\r\n";
        //        Menu += "<tbody>\r\n";
        //        Menu += "<tr>\r\n";
        //        Menu += "<td>\r\n";
        //        Menu += "<table class='bsitem_menu_text'>\r\n";
        //        Menu += "<tbody>\r\n";
        //        Menu += "<tr style='CURSOR: pointer;background-image:url(HTML/Images/cabeceraCentro.gif);'>\r\n";
        //        Menu += "<td style='color:White;'>&nbsp;" + Texto + "</td>\r\n";
        //        Menu += "<td class='bsmenu_arrow'><img alt='Ir a " + DescripcionPadre + "' src='HTML/Images/menuArrowVerde1.png'/></td></tr></tbody></table></td></tr></tbody></table>";

        //        Menu += "</a></div>\r\n";




        //        Menu += "<div class='accordion'>\r\n";
        //        Menu += "<table>\r\n";
        //        Menu += "<tbody>\r\n";

        //        //Hijo.
        //        int j = 10;
        //        if (listaMenus[i - 1].Children.Count == 0)
        //        {
        //            String strSelected = "";
        //            if (opcionSeleccionada.Equals(i + "_" + j))
        //            {
        //                strSelected = "_selected";
        //            }

        //            string link = listaMenus[i - 1].LinkUrl;// "";
        //            string Descripcion = listaMenus[i - 1].Description;
        //            Texto = listaMenus[i - 1].Text;

        //            Menu += "<tr>\r\n";
        //            Menu += "<td class='bsitem_menu_point" + strSelected + "' id='bsitem_menu_point_" + i + "_" + j + "' \r\n";
        //            Menu += "><img \r\n";
        //            Menu += "alt='Ir a " + Descripcion + "' \r\n";
        //            Menu += "src='HTML/Images/arrow.gif' />&nbsp; <a title='" + Descripcion + "' class='linkMenu" + strSelected + "'\r\n";
        //            Menu += "id='idLink_bsitem_menu_point_" + i + "_" + j + "' \r\n";


        //            if (link != null && link.ToLower().IndexOf("javascript:") > -1)
        //            {
        //                Menu += "onclick='" + link + ";return false;'\r\n";
        //                Menu += "href='#'>" + Texto + "</a>\r\n";
        //            }
        //            else
        //            {
        //                Menu += "onclick=\"OnMenuClick('" + listaMenus.Count + "', '" + i + "_" + j + "', '" + link + "#" + i + "');return false;\"\r\n";
        //                Menu += "href='#'>" + Texto + "</a>\r\n";
        //            }

        //            //Menu += "href='" + link + "#" + i + "'>" + Texto + "</a>\r\n";
        //            Menu += "</td></tr>\r\n";
        //        }
        //        else
        //        {

        //            for (j = 10; j < (listaMenus[i - 1].Children.Count + 10); j++)
        //            {

        //                String strSelected = "";
        //                if (opcionSeleccionada.Equals(i + "_" + j))
        //                {
        //                    strSelected = "_selected";
        //                }

        //                string link = listaMenus[i - 1].Children[j - 10].LinkUrl;// "";
        //                string Descripcion = listaMenus[i - 1].Children[j - 10].Description;
        //                Texto = listaMenus[i - 1].Children[j - 10].Text;

        //                Menu += "<tr>\r\n";
        //                Menu += "<td class='bsitem_menu_point" + strSelected + "' id='bsitem_menu_point_" + i + "_" + j + "' \r\n";
        //                Menu += "><img \r\n";
        //                Menu += "alt='Ir a " + Descripcion + "' \r\n";
        //                Menu += "src='HTML/Images/arrow.gif' />&nbsp; <a title='" + Descripcion + "' class='linkMenu" + strSelected + "'\r\n";
        //                Menu += "id='idLink_bsitem_menu_point_" + i + "_" + j + "' \r\n";


        //                if (link != null && link.ToLower().IndexOf("javascript:") > -1)
        //                {
        //                    Menu += "onclick='" + link + ";return false;'\r\n";
        //                    Menu += "href='#'>" + Texto + "</a>\r\n";
        //                }
        //                else
        //                {
        //                    Menu += "onclick=\"OnMenuClick('" + listaMenus.Count + "', '" + i + "_" + j + "', '" + link + "#" + i + "');return false;\"\r\n";
        //                    Menu += "href='#'>" + Texto + "</a>\r\n";
        //                }

        //                //Menu += "href='" + link + "#" + i + "'>" + Texto + "</a>\r\n";
        //                Menu += "</td></tr>\r\n";
        //            }
        //        }
        //        Menu += "</tbody></table></div></td></tr>\r\n";
        //    }
        //    return Menu;
        //}
        //protected void OnBtnMenu_Click(object sender, EventArgs e)
        //{
        //    NavigationController.ClearHistory();
        //    NavigationController.Forward(this.hdnLinkUrl.Value, "./FrmPrincipal.aspx");
        //}
        #endregion

        protected void BtnOpcionContratos_Click(object sender, EventArgs e)
        {
            MasterPageMntoGas.strVentanaActiva = "FrmDetalleContrato";
            NavigationController.ClearHistory();
            NavigationController.Forward("./FrmDetalleContrato.aspx", "./FrmPrincipal.aspx");
        }
        protected void BtnAccesoUltimaVisita_Click(object sender, EventArgs e)
        {
            MasterPageMntoGas.strVentanaActiva = "FrmUltimaVisita";
            NavigationController.ClearHistory();
            NavigationController.Forward("./FrmUltimaVisita.aspx", "./FrmPrincipal.aspx");
        }

        protected void btnDesconectar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.User = null;

            CurrentSession.Clear();

            MasterPageMntoGas.strVentanaActiva = "";
            NavigationController.ClearHistory();
            Response.Redirect("../Login.aspx");
        }

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            MasterPageMntoGas.strVentanaActiva = "";
            NavigationController.ClearHistory();
            Response.Redirect("../MenuAcceso.aspx");
        }


        
        protected void BtnConsultas_Click(object sender, EventArgs e)
        {
            MasterPageMntoGas.strVentanaActiva = "FrmConsultas";
            NavigationController.ClearHistory();
            NavigationController.Forward("./FrmConsultas.aspx", "./FrmPrincipal.aspx");
        }

        protected void BtnCambioPassword_Click(object sender, EventArgs e)
        {
            MasterPageMntoGas.strVentanaActiva = "FrmCambioContrasenia";
            NavigationController.ClearHistory();
            NavigationController.Forward("./FrmCambioContrasenia.aspx", "./FrmPrincipal.aspx");
        }

        protected void BtnFacturacionVisitas_Click(object sender, EventArgs e)
        {
            MasterPageMntoGas.strVentanaActiva = "FrmFacturacionVisitas";
            NavigationController.ClearHistory();
            NavigationController.Forward("./FrmFacturacionVisitas.aspx", "./FrmPrincipal.aspx");
        }

        protected void btnHistoricoVisita_Click(object sender, EventArgs e)
        {
            MasterPageMntoGas.strVentanaActiva = "FrmHistoricoVisita";
            NavigationController.ClearHistory();
            NavigationController.Forward("./FrmHistoricoVisita.aspx", "./FrmPrincipal.aspx");
        }
        #endregion
        
}
}
