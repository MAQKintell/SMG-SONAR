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

public partial class _Default : System.Web.UI.Page
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
            }
            catch(Exception)
            {
                Response.Redirect("./Login.aspx");
            }


            List<MenuOption> listaMenus = Menus.ObtenerMenu(0, (Int32)usuarioDTO.Id_Perfil, usuarioDTO.Permiso,1);
            this.treeViewMenu.CssClass = "linkFormulario";
            CrearArbol(listaMenus, this.treeViewMenu.Nodes);
        }
    }

    private void CrearArbol(List<MenuOption> listaOpciones, TreeNodeCollection nodeCollection)
    {
        foreach (MenuOption menu in listaOpciones)
        {
            TreeNode newNode = new TreeNode();
            newNode.Text = menu.Text;
            newNode.NavigateUrl = menu.LinkUrl;
            newNode.ToolTip = menu.LinkUrl;

            nodeCollection.Add(newNode);

            if (menu.Children != null && menu.Children.Count > 0)
            {
                CrearArbol(menu.Children, newNode.ChildNodes);
            }
        }
    }
}
