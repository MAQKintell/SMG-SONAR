<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmModalCoberturaServicio.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalCoberturaServicio" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<html>
<head>
</head>
<body style="background-color:white;">
<form id="Form1" runat="server">
    <asp:HiddenField ID="hdnCodEFV" runat="server" />
    
    <div style="position:absolute;left:580px;top:10px">
        <img alt="Cerrar Pantalla" style="cursor:pointer;" src='../UI/HTML/Images/cerrarModal.gif' title='Cerrar ventana' onclick='parent.VentanaModal.cerrarVentana()' />
    </div>

    <div id="divInformacioncoberturaServicio" style="position:absolute; width: 437px; top: 8px; left: 7px; height: 266px; margin-right: 1px; ">
        <asp:Label ID="lblTitulo" Font-Names="Trebuchet MS" Font-Size="12pt" 
            runat="server" Font-Underline="False" ForeColor="Orange" 
            meta:resourcekey="lblTituloResource1"></asp:Label>
        <br /><br /><asp:Label Font-Names="Trebuchet MS" Font-Size="8pt" ID="lblTexto1" 
            runat="server" ForeColor="Black" Width="600px" 
            meta:resourcekey="lblTexto1Resource1"></asp:Label>
        <br /><asp:Label Font-Names="Trebuchet MS" Font-Size="8pt" ID="lblTexto2"  
            runat="server" ForeColor="Black" Width="600px" 
            meta:resourcekey="lblTexto2Resource1"></asp:Label>
        <br /><asp:Label Font-Names="Trebuchet MS" Font-Size="8pt" ID="lblTexto3"  
            runat="server" ForeColor="Black" Width="600px" 
            meta:resourcekey="lblTexto3Resource1"></asp:Label>
        <br /><asp:Label Font-Names="Trebuchet MS" Font-Size="8pt" ID="lblTexto4"  
            runat="server" ForeColor="Black" Width="600px" 
            meta:resourcekey="lblTexto4Resource1"></asp:Label>
    </div>
</form>
</body>
</html>