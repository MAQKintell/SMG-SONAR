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
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Web;
using System.Collections.Generic;


namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de Calderas
    /// </summary>
    public class Menus
    {
        public Menus()
        {
        }

        public static List<MenuOption> ObtenerMenu(Int32 modulo, Int32 idPerfil, bool? permiso, Int32 idIdioma)
        {

            MenuDB menuDB = new MenuDB();
            List<MenuDTO> listaMenuDTO = menuDB.ObtenerMenu(modulo, idPerfil, permiso, idIdioma);

            MenuOption ultimoNivel1 = null;
            if (listaMenuDTO != null && listaMenuDTO.Count > 0)
            {
                List<MenuOption> listaMenuOption = new List<MenuOption>();
                foreach (MenuDTO menuDto in listaMenuDTO)
                {
                    MenuOption menuOption = new MenuOption();
                    if (menuDto.IdMenu.HasValue)
                    {
                        menuOption.Id = menuDto.IdMenu.Value.ToString();
                    }
                    menuOption.Text = menuDto.Texto;
                    menuOption.LinkUrl = menuDto.Link;
                    menuOption.Order = menuDto.Orden;
                    menuOption.Description = menuDto.DescMenu;

                    String orden = menuDto.Orden;
                    if (orden == null)
                    {
                        orden = "";
                    }
                    if (orden.IndexOf(".") == -1)
                    {
                        menuOption.Level = 1;
                        ultimoNivel1 = menuOption;
                        listaMenuOption.Add(menuOption);
                        //ultimoNivel1 = menuOption;
                    }
                    else
                    {
                        String[] niveles = orden.Split('.');

                        //if (niveles.Length > 2)
                        //{
                        //    menuOption.Level = 3;
                        //    ultimoNivel1.Children.Add(menuOption);
                        //}
                        //else
                        //{
                            menuOption.Level = niveles.Length;
                            ultimoNivel1.Children.Add(menuOption);
                        //}
                        ////ultimoNivel1.AddChild(menuOption);

                        //ultimoNivel1.Children.Add(menuOption);
                        ////listaMenuOption.Add(ultimoNivel1);
                    }
                    
                }
                return listaMenuOption;
            }
            return null;

        }

        private Boolean ExisteMenuOption( List<MenuOption> listaMenuOption, MenuOption menuOption)
        {
            foreach (MenuOption menu in listaMenuOption)
            {
                if (menu.Id.Equals(menuOption.Id))
                {
                    return true;
                }
            }
            return false;
        }

        
    }
}