<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmEspera.aspx.cs" Inherits="Pages_frmEspera" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Página sin título</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="position:absolute; top: 84px; left: 10px;">
        <asp:Image ID="Image1" runat="server" 
            ImageUrl="~/Imagenes/emblem-important.png" />

    </div>
    <div style="position:absolute; top: 142px; left: 145px;">
            <asp:Label ID="Label1" runat="server" Text="CARGANDO DATOS ESPERE POR FAVOR..." 
            Font-Bold="True"></asp:Label>
    </div>
    </form>
</body>
</html>
