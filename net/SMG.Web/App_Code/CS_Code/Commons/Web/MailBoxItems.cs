﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public class MailBoxItems
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ArrayList FetchMailItems()
    {
        ArrayList mailItems = new ArrayList();
        mailItems.Add(new { Name = "Mailbox", ImageUrl = "_assets/img/mailbox.gif" });
        mailItems.Add(new { Name = "Inbox", ImageUrl = "_assets/img/inbox.gif" });
        mailItems.Add(new { Name = "Drafts", ImageUrl = "_assets/img/drafts.gif" });
        mailItems.Add(new { Name = "Outbox", ImageUrl = "_assets/img/outbox.gif" });
        mailItems.Add(new { Name = "Junk Mail", ImageUrl = "_assets/img/junk.gif" });
        mailItems.Add(new { Name = "Deleted Items", ImageUrl = "_assets/img/deleted.gif" });
        mailItems.Add(new { Name = "Search Folders", ImageUrl = "_assets/img/search.gif" });
        mailItems.Add(new { Name = "Sent Items", ImageUrl = "_assets/img/sentitems.gif" });

        return mailItems;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ArrayList FetchNoteItems()
    {
        ArrayList mailItems = new ArrayList();
        mailItems.Add(new { Name = "Icons", ImageUrl = "_assets/img/note_small.gif" });
        mailItems.Add(new { Name = "Note List", ImageUrl = "_assets/img/note_small.gif" });
        mailItems.Add(new { Name = "Last Seven Days", ImageUrl = "_assets/img/note_small.gif" });
        mailItems.Add(new { Name = "By Category", ImageUrl = "_assets/img/note_small.gif" });
        mailItems.Add(new { Name = "By Color", ImageUrl = "_assets/img/note_small.gif" });

        return mailItems;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ArrayList FetchContactItems()
    {
        ArrayList mailItems = new ArrayList();
        mailItems.Add(new { Name = "Address Cards", ImageUrl = "_assets/img/contact_small.gif" });
        mailItems.Add(new { Name = "Detailed Address List", ImageUrl = "_assets/img/contact_small.gif" });
        mailItems.Add(new { Name = "By Category", ImageUrl = "_assets/img/contact_small.gif" });
        mailItems.Add(new { Name = "By Company", ImageUrl = "_assets/img/contact_small.gif" });
        mailItems.Add(new { Name = "By Follow-up Flag", ImageUrl = "_assets/img/contact_small.gif" });

        return mailItems;
    }
}
