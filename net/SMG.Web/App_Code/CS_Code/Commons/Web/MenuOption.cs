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

/// <summary>
/// Descripción breve de MenuItem
/// </summary>
public class MenuOption
{

    #region atributos
    private String _id;
    private String _order;
    private String _text;
    private Int32 _level;
    private String _linkUrl;
    private String _description;
    private List<MenuOption> _children;
    #endregion

    #region propiedades
    public String Id
    {
        set { this._order = value; }
        get { return this._order; }
    }
    public String Order
    {
        set { this._id = value; }
        get { return this._id; }
    }
    public String Text
    {
        set { this._text = value; }
        get { return this._text; }
    }
    public Int32 Level
    {
        set { this._level = value; }
        get { return this._level; }
    }
    public String LinkUrl
    {
        set { this._linkUrl = value; }
        get { return this._linkUrl; }
    }
    public List<MenuOption> Children
    {
        set { this._children = value; }
        get { return this._children; }
    }
    public String Description
    {
        set { this._description = value; }
        get { return this._description; }
    }
    #endregion

    public MenuOption()
	{
        _children = new List<MenuOption>();
	}

    public void AddChild(MenuOption child)
    {
        if (this.Level + 1 == child.Level)
        {
            if (child.Order.Contains(this.Order))
            {
                this.Children.Add(child);
            }
        }
        else
        {
            foreach (MenuOption menuOp in this.Children)
            {
                if (menuOp.Level + 1 == child.Level)
                {
                    if (child.Order.Contains(menuOp.Order))
                    {
                        menuOp.Children.Add(child);
                    }
                }
                else
                {
                    foreach (MenuOption menuOp2 in menuOp.Children)
                    {
                        if (menuOp2.Level + 1 == child.Level)
                        {
                            if (child.Order.Contains(menuOp2.Order))
                            {
                                menuOp2.Children.Add(child);
                            }
                        }
                    }
                }
            }
        }
    }

}
