<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="UI/HTML/Css/master.css" type="text/css" rel="stylesheet" />
    <link href="UI/HTML/Css/tablas.css" type="text/css" rel="stylesheet" />
    <link href="UI/HTML/Css/formularios.css" type="text/css" rel="stylesheet" />
    <!--[if lt IE 7]><LINK href="css/ventan-modal-ie.css" type=text/css rel=stylesheet><![endif]-->
    <script type="text/javascript">
		function AbrirVentana(NombreVentana){
		    abrirVentanaLocalizacion(NombreVentana, 700, 630, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_ELEGIR_ACCION%>", "0");
		}
	</script>
	
	<title>IBERDROLA - Mantenimiento de Gas </title>
    <style type="text/css">
        #form1
        {
            width: 250px;
        }
    </style>
</head>
<body>
     <div id="divCabecera">
       <div id="cabeceraIzda"></div>
        <div id="cabeceraCentro">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr> 
                        <td><img src="UI/HTML/Images/logoIberdrola.jpg" alt="logo IBERDROLA"/></td>
                        <td></td>
                        <td class="textoTituloCabecera" align="left"> - MANTENIMIENTO DE GAS - </td>
                        <td></td>
                    </tr>
                </table>
        </div>
        <div id="cabeceraDcha"></div>    
    </div>
    

    <form id="form1" runat="server">
    <!--
        <div style="position:absolute;top:164px; left:465px; z-index:10">
            <table width="100%">
                <tr>
                <td><a href="./Login.aspx" class="linkFormulario">Formularios</a></td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td><a href="./Pages/Login.aspx" class="linkFormulario">Web Teléfono</a></td>
                </tr>
            </table>
         </div>
         -->
         <div style="position:absolute;top:140px; left:650px; z-index:10; width:300px">
            <span class="labelFormularioValor">Opciones Disponibles:</span>
        </div>
         <div style="position:absolute;top:164px; left:650px; z-index:10">
            <asp:TreeView ID="treeViewMenu" runat="server" style="margin-right: 17px" ShowLines="true">
            </asp:TreeView>
        </div>
  </form>

    
   <div id="divImagenLogo" style="position:absolute;top:150px;left:40px;z-index:0">
        <img alt="" src="UI/HTML/Images/iberdrola.jpg" />
    </div>

    </body>
</html>