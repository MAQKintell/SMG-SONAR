<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmModalCuadroDeMando.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalCuadroDeMando" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<html>
<head>

    <script src="HTML/Js/ventana-modal.js" type="text/javascript"></script>
    <script src="HTML/Js/calendar.js" type="text/javascript"></script>
    <script src="HTML/Js/calendar-setup.js" type="text/javascript"></script>
    <script src="HTML/Js/utf8calendar-es.js" type="text/javascript"></script>
    <script src="HTML/Js/formularios.js" type="text/javascript"></script>
    
    <link href="HTML/Css/master.css" type="text/css" rel="stylesheet" />
    <link href="HTML/Css/tablas.css" type="text/css" rel="stylesheet" />
    <link href="HTML/Css/formularios.css" type="text/css" rel="stylesheet" />
    <link href="HTML/Css/ventana-modal.css" type="text/css" rel="stylesheet" />
    <link href="HTML/Css/calendar-blue.css" type="text/css" rel="stylesheet" />
</head>
<body>
<form id="Form1" runat="server">
    
    <div style="position:absolute;left:565px; top:26px">
        <img alt="Cerrar Pantalla" style="cursor:pointer;" src='../UI/HTML/Images/cerrarModal.gif' title='Cerrar ventana' onclick='parent.VentanaModal.cerrarVentana()' />
    </div>

 <div style="position:absolute; top: 20px; left: 21px; height: 181px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/Valoracion.gif" 
                ForeColor="#3399FF" meta:resourcekey="Image1Resource1"  />
        </div>  
        
    <div id="divInformacioncoberturaServicio" 
        
        style="position:absolute; width: 397px; top: 19px; left: 193px; height: 182px; margin-right: 1px; ">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblTitulo" Font-Bold="True" Font-Names="Trebuchet MS" 
            Font-Size="12pt" runat="server" Font-Underline="True" ForeColor="Orange" 
            Text="VALORACION CUADRO DE MANDO" meta:resourcekey="lblTituloResource1"></asp:Label>
        <br /><br />
        <asp:Label Font-Names="Trebuchet MS" Font-Bold="True" Font-Size="8pt" 
            ID="lblProveedor" runat="server" ForeColor="Black" Width="122px" 
            Text="PROVEEDOR:" meta:resourcekey="lblProveedorResource1"></asp:Label>
        <asp:DropDownList ID="cmbProveedor" 
            runat="server" meta:resourcekey="cmbProveedorResource1" ></asp:DropDownList>
        <br />
        <br />
        <asp:Label Font-Names="Trebuchet MS" Font-Bold="True" Font-Size="8pt" ID="lblValoracion"  
            runat="server" ForeColor="#0066FF" Width="132px" 
            Text="VALORACION VISITAS:" meta:resourcekey="lblValoracionResource1"></asp:Label>
        <asp:TextBox ID="txtValoracionVisitas" runat="server" Width="58px" 
            meta:resourcekey="txtValoracionVisitasResource1"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="errorFormulario" 
            ToolTip="Debe Rellenar datos para la valoración de Visitas" 
            ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
            ControlToValidate="txtValoracionVisitas" 
            meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
        <br />
        <asp:Label Font-Names="Trebuchet MS" Font-Bold="True" Font-Size="8pt" ID="Label1"  
            runat="server" ForeColor="#009933" Width="132px" 
            Text="VALORACION AVERIAS:" meta:resourcekey="Label1Resource1"></asp:Label>
        <asp:TextBox ID="txtValoracionAverias" runat="server" Width="58px" 
            meta:resourcekey="txtValoracionAveriasResource1"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="errorFormulario" 
            ToolTip="Debe Rellenar datos para la valoración de Averias" 
            ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" 
            ControlToValidate="txtValoracionAverias" 
            meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
        </div>
 <div style="position:absolute; top: 123px; left: 394px; height: 20px;">
     <asp:Label Font-Names="Trebuchet MS" Font-Bold="True" Font-Size="8pt" ID="Label2"  
            runat="server" ForeColor="#009933" Width="132px" 
         Text="CARTERA NUEVO SMG:" meta:resourcekey="Label2Resource1"></asp:Label>
        &nbsp;<asp:TextBox ID="txtCarteraNuevoSMG" runat="server" Width="58px" 
         meta:resourcekey="txtCarteraNuevoSMGResource1"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="errorFormulario" 
         ToolTip="Debe Rellenar datos para la cartera del Nuevo SMG" 
         ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" 
         ControlToValidate="txtCarteraNuevoSMG" 
         meta:resourcekey="RequiredFieldValidator3Resource1"></asp:RequiredFieldValidator>
<br />
        <asp:Label Font-Names="Trebuchet MS" Font-Bold="True" Font-Size="8pt" ID="Label3"  
            runat="server" ForeColor="#009933" Width="132px" 
            Text="CARTERA GAS CONFORT:" meta:resourcekey="Label3Resource1"></asp:Label>
        &nbsp;<asp:TextBox ID="txtCarteraGasConfort" runat="server" Width="58px" 
         meta:resourcekey="txtCarteraGasConfortResource1"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="errorFormulario" 
         ToolTip="Debe Rellenar datos para la cartera del Gas Confort" 
         ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" 
         ControlToValidate="txtCarteraGasConfort" 
         meta:resourcekey="RequiredFieldValidator4Resource1"></asp:RequiredFieldValidator>
    </div>
    <div style="position:absolute; top: 82px; left: 194px; height: 20px; width: 389px;">
        <hr style="width: 392px" /></div>
    <div id="divBotonesCuadroMando" 
        style="position:absolute; top: 174px; left: 483px; width: 52px;" >
        <asp:Button ID="btnAceptar" CssClass="botonFormulario" runat="server" 
            Text="Aceptar" onclick="btnAceptar_Click" Width="109px" 
            meta:resourcekey="btnAceptarResource1" />
</form>
</body>
</html>