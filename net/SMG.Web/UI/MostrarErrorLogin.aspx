<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MostrarErrorLogin.aspx.cs" Inherits="Iberdrola.SMG.UI.MostrarErrorLogin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="HTML/Css/master.css" type="text/css" rel="stylesheet" />
    <link href="HTML/Css/tablas.css" type="text/css" rel="stylesheet" />
    <link href="HTML/Css/formularios.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
		function AbrirVentana(NombreVentana){
            abrirVentanaLocalizacion(NombreVentana, 700, 630, "ventana-modal","ELEGIR ACCIÓN","0");
		}
	</script>
	
	<title>IBERDROLA - Opera SMG </title>
</head>
<body>
   <div id="divImagenLogo" style="position:absolute;top:150px;left:40px;z-index:0">
        <img alt="" src="HTML/Images/iberdrola.jpg" />
    </div>
    <form id="form1" runat="server" autocomplete="off">
    <asp:HiddenField runat="server" ID="hdnNumReintentos" Value="0" />
      <div id="divCabecera">
       <div id="cabeceraIzda"></div>
        <div id="cabeceraCentro">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr> 
                        <td><img src="HTML/Images/logoIberdrola.jpg" alt="logo IBERDROLA"/></td>
                        <td></td>
                        <td class="textoTituloCabecera" align="left">
                            <asp:Label CssClass="textoTituloCabecera" ID="lblTitulo" runat="server" 
                       text=" - OPERA SMG - " meta:resourcekey="lblTituloResource1"></asp:Label></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td class="" align="left"><font color="white" size="1">V 4.0</font></td>
                    </tr>
                </table>
        </div>
        <div id="cabeceraDcha"></div>    
    </div>
    <div id="divDatos" style="position:absolute; top: 167px; left: 610px; width: 390px;">
            <div class="divTituloVentana">
                <asp:Label ID="lblTituloVentana" runat="server" Text="AVISO:" />
            </div>
            <asp:Label ID="lblTextoMensajeError" CssClass="labelFormulario" runat="server" Text="Error interno de la a" />
    </div>
    
    <div id="divbotonVolverIniciarSesion" style="position:absolute; top: 253px; left: 710px; width: 98px;">
        <asp:Button ID="btnVolverIniciarSesion" runat="server" Text="Volver" Width="95px" CssClass="botonFormulario botonVolver ui-button ui-widget ui-state-default ui-corner-all" onclick="BtnVolverIniciarSesion_Click"/>
    </div>
    
    <!-- Pie -->
    <div id="divPie">
       <table cellspacing="0" cellpadding="0">
            <tr>
                <td><div id="pieIzda"></div></td>
                <td><div id="pieCentro"></div></td>
                <td><div id="pieDcha"></div></td>
            </tr>
        </table>
    </div>
    </form>
   <script type="text/javascript">

        function abrirVentana() {
            var window_width = 1010;
            var window_height = 700;
            var newfeatures= 'scrollbars=NO, menubar=NO, toolbar=NO,resizable=no';
            var window_top = 0;
            var window_left = (screen.width-window_width)/2;
            
            window.location.href = "./UI/FrmPrincipal.aspx";
        }
        function abrirCambioPassword(alerta, caducidad) {
            var window_width = 1010;
            var window_height = 700;
            var newfeatures= 'scrollbars=NO, menubar=NO, toolbar=NO,resizable=no';
            //var window_top = (screen.height-window_height)/2;
            var window_top = 0;
            var window_left = (screen.width-window_width)/2;
            
            if (alerta)
            {
                window.location.href = "./UI/FrmCambioContrasenia.aspx?MostrarAlerta=1";
            }
            else
            {
                if (caducidad)
                {
                    window.location.href = "./UI/FrmCambioContrasenia.aspx?MostrarCaducidad=1";
                }
            }
        }
    </script>
    <asp:PlaceHolder ID="_PlaceHolderScript" runat="server"></asp:PlaceHolder>
</body>
</html>