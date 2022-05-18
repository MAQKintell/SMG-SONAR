<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmModalCodificacionAverias.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalCodificacionAverias" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<html>
<head>
</head>
<body>
<form id="Form1" runat="server">
    <asp:HiddenField ID="hdnCodEFV" runat="server" />
    
    <div style="position:absolute;left:580px;top:10px">
        <img alt="Cerrar Pantalla" style="cursor:pointer;" src='../UI/HTML/Images/cerrarModal.gif' title='Cerrar ventana' onclick='parent.VentanaModal.cerrarVentana()' />
    </div>

    <div id="divInformacioncodificacionaverias" style="position:absolute; width: 437px; top: 8px; left: 7px; height: 266px; margin-right: 1px; ">
        <asp:Label ID="lblTitulo" Font-Names="Trebuchet MS" Font-Size="12pt" 
            runat="server" Font-Underline="False" ForeColor="Orange" 
            meta:resourcekey="lblTituloResource1"></asp:Label>
        <br />
        <asp:ListBox ID="lstCodigos" runat="server" Width="580px" Height="370px" 
            Font-Names="Trebuchet MS" Font-Size="8pt" ForeColor="Black" 
            meta:resourcekey="lstCodigosResource1"></asp:ListBox>
    </div>
</form>
</body>
</html>