using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;


public class Authentication:SoapHeader
{
    private string _userName;
    private string _password;

    public string Password
    {
        get { return _password; }
        set { _password = value; }
    }

    public string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }
}
