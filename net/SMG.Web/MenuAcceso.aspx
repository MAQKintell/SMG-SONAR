<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuAcceso.aspx.cs" Inherits="MenuAcceso" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="UI/HTML/Css/master.css" type="text/css" rel="stylesheet" />
    <link href="UI/HTML/Css/tablas.css" type="text/css" rel="stylesheet" />
    <link href="UI/HTML/Css/formularios.css" type="text/css" rel="stylesheet" />
    <link href="Css/ventana-modal.css" type="text/css" rel="stylesheet" />
    <link href="css/ventan-modal-ie.css" type="text/css" rel="stylesheet" />
    <script src="js/ventana-modal.js" type="text/javascript"></script>
    
    <!--[if lt IE 7]><LINK href="css/ventan-modal-ie.css" type=text/css rel=stylesheet><![endif]-->
    <script type="text/javascript">
        //self.onerror = suppress() {return true}
        window.onerror =function() {}
		function AbrirVentana(NombreVentana){
		    abrirVentanaLocalizacion(NombreVentana, 700, 630, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_ELEGIR_ACCION%>", "0");
		}

        function AbrirArgumentario() {
            //alert('qqq');
            abrirVentanaLocalizacion1("Pages/PruebaDatosArgumentario.aspx", 700, 435, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_DATOS_ARGUMENTARIO%>", "0", "1");
        }

        function AbrirCuadroMando() {
            //alert('qqq');
            //abrirVentanaLocalizacion1("UI/PruebaDatosArgumentario.aspx", 700, 435, "ventana-modal", Resources.TEXTO_DATOS_ARGUMENTARIO, "0", "1");
            abrirVentanaLocalizacion1("UI/FrmModalCuadroDeMando.aspx", 630, 200, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_VALORACION_CUADRO_MANDO%>", "0", "1");
        }
    </script>

    <link href="css/css_cached.css" type="text/css" rel="stylesheet" />

    <SCRIPT src="js/prototype.js" type="text/javascript"></SCRIPT>
    <SCRIPT src="js/portal.js" type="text/javascript"></SCRIPT>

	<title>IBERDROLA - Opera SMG </title>
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
                            <td class="textoTituloCabecera" align="left"> - OPERA SMG -                                                                         _</td>
                            <td></td>
                            <td align="right"></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td align="right">
                            </td>
                        </tr>
                    </table>
            </div>
        <div id="cabeceraDcha"></div>    
    </div>
    
<form id="form1" runat="server" style="width:0px">
    <div id="divUsuarioConectado" style="position:absolute;top:15px;left:705px;width:350px">
        <asp:Label class="textoUsuarioConectado" ID="lblUsuarioConectado" 
            runat="server" meta:resourcekey="lblUsuarioConectadoResource1"></asp:Label>
    </div>
   
   <div id="div1" style="position:absolute;top:17px;left:918px;">
        <asp:LinkButton ID="btnDesconectar" runat="server" class="desconectar" 
            onclick="btnDesconectar_Click" Text="desconectar" 
            meta:resourcekey="btnDesconectarResource1"></asp:LinkButton>
   </div>
   
                                  
    
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
        <div style="position:absolute;top:110px; left:700px; z-index:10; width:300px">
            <asp:Button runat="server" ID="btnGestionDoc" CssClass="botonFormulario" OnClick="btnGestionDoc_Click" Text="Gestión Documental" />
         </div>
         <div style="position:absolute;top:140px; left:700px; z-index:10; width:300px">
            <span class="labelFormularioValor" 
                 style="color: #008000; font-weight: bold; text-decoration: underline;">
             <asp:Label style="color: #008000; font-weight: bold; text-decoration: underline;" 
                 CssClass="labelFormularioValor" ID="lblOpciones" runat="server" 
                 text="Opciones Disponibles:" meta:resourcekey="lblOpcionesResource1"></asp:Label></span> <br />
             <asp:Label ID="lblErrorPerfil" CssClass="mensajeError" runat="server" 
                 meta:resourcekey="lblErrorPerfilResource1"></asp:Label>
        </div>
        <div style="position:absolute;top:164px; left:727px; z-index:10;visibility:visible">
            <asp:TreeView ID="treeViewMenu" runat="server" style="margin-right: 17px" 
                ShowLines="false" meta:resourcekey="treeViewMenuResource1" ShowExpandCollapse="false" >
                    <RootNodeStyle ImageUrl="Imagenes/flecha.gif" />
                    <ParentNodeStyle ImageUrl="Imagenes/flecha.gif" />
                    <LeafNodeStyle ImageUrl="Imagenes/flecha.gif"  />

            </asp:TreeView>
        </div>
   
<div style="POSITION: absolute; TOP: 120px; left: 571px; width: 148px; height: 9px;visibility:hidden">
    <!--*******************************************************************************************left:220-->
	<div id="layout-outer-side-decoration" style="WIDTH: 105px; HEIGHT: 9.02%;visibility:hidden">
			<div id="layout-inner-side-decoration;visibility:hidden">
				<div ID="portal-dock" style="Z-INDEX: 999; POSITION: relative;visibility:hidden">
					<div id="portal-dock-my-places" style="Z-INDEX: 999; POSITION: absolute; left:180px;top:53px;visibility:hidden">
                        <div id="1" class="portal-dock-box">
							<div title="Opciones Disponibles" style="BACKGROUND-POSITION: center center; BACKGROUND-IMAGE: url(Imagenes/Verde2.png); WIDTH: 50px; BACKGROUND-REPEAT: no-repeat; HEIGHT: 50px; font-weight: bold;">
                                <asp:Image ID="Image1" Visible="False" runat="server" 
                                    ImageUrl="~/Imagenes/images2.jpg" meta:resourcekey="Image1Resource1" />
							</div>
                            <div style="position:absolute; top: 11px; left: 63px; background-color :White;">
                                <asp:Label ID="Label1" runat="server" Text="Opciones Disponibles  ." 
                                    Width="150px" Font-Size="Small" Font-Bold="True" Font-Underline="False" 
                                    meta:resourcekey="Label1Resource1"></asp:Label></div>
						</div>
						<div id="2" title="Formularios" class="portal-dock-box" onmouseover="LiferayDock.showObject('portal-dock-my-places', 10)" style="visibility:hidden">
							<div style="BACKGROUND-POSITION: center center; BACKGROUND-IMAGE: url(Imagenes/Verde2.png); WIDTH: 50px; BACKGROUND-REPEAT: no-repeat; HEIGHT: 50px"></div>
    						<div style="position:absolute; top: 11px; left: 63px; background-color :White;">
                                <asp:Label ID="Label2" runat="server" Text="Formularios" Font-Size="Small" 
                                    Width="130px" meta:resourcekey="Label2Resource1"></asp:Label></div>
						</div>
						<div id="6" title="Envio SMS" class="portal-dock-box" onmouseover="LiferayDock.showObject('portal-dock-search', 10); $('portal-dock-search').getElementsByTagName('input')[0].focus()" style="visibility:hidden">
							<div style="BACKGROUND-POSITION: center center; BACKGROUND-IMAGE: url(Imagenes/Sobre.png); WIDTH: 50px; BACKGROUND-REPEAT: no-repeat; HEIGHT: 50px">
							<asp:Image ID="Image2" Visible="False" runat="server" 
                                    ImageUrl="~/Imagenes/sobre.png" Height="50px" Width="50px" 
                                    meta:resourcekey="Image2Resource1" />
							</div>
						    <div style="position:absolute; top: 11px; left: 63px; background-color :White;">
                                <asp:Label ID="Label6" runat="server" Text="Envio SMS" Width="130px" 
                                    Font-Size="Small" meta:resourcekey="Label6Resource1"></asp:Label></div>
						</div>
						<div id="4" title="Solicitudes" class="portal-dock-box"  style="visibility:hidden">
							<div style="BACKGROUND-POSITION: center center; BACKGROUND-IMAGE: url(Imagenes/Verde2.png); WIDTH: 50px; BACKGROUND-REPEAT: no-repeat; HEIGHT: 50px">
							    <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/solicitud.gif" />--%>
							</div>
						    <div style="position:absolute; top: 11px; left: 63px; background-color :White;">
                                <asp:Label ID="Label4" runat="server" Text="Solicitudes" Width="130px" 
                                    Font-Size="Small" meta:resourcekey="Label4Resource1"></asp:Label></div>
						</div>
						<div id="3" title="Proveedores" class="portal-dock-box" style="visibility:hidden">
							<div  style="BACKGROUND-POSITION: center center; BACKGROUND-IMAGE: url(Imagenes/Verde2.png); WIDTH: 50px; BACKGROUND-REPEAT: no-repeat; HEIGHT: 50px"></div>
						    <div style="position:absolute; top: 11px; left: 63px; background-color :White;">
                                <asp:Label ID="Label3" runat="server" Text="Proveedores" Width="130px" 
                                    Font-Size="Small" meta:resourcekey="Label3Resource1"></asp:Label></div>
						</div>
						
						<div id="5" title="Datos Argumentario" class="portal-dock-box" onmouseover="LiferayDock.showObject('portal-dock-search', 10); $('portal-dock-search').getElementsByTagName('input')[0].focus()" style="visibility:hidden">
							<div  style="BACKGROUND-POSITION: center center; BACKGROUND-IMAGE: url(Imagenes/Verde2.png); WIDTH: 50px; BACKGROUND-REPEAT: no-repeat; HEIGHT: 50px">
							
							</div>
						    <div style="position:absolute; top: 11px; left: 63px; background-color :White;">
                                <asp:Label ID="Label5" runat="server" Text="Datos Argumentario" Width="130px" 
                                    Font-Size="Small" meta:resourcekey="Label5Resource1"></asp:Label></div>
						</div>
						
						
						
					</div>
				</div>
				<script type="text/javascript">
					LiferayDock.initialize("");//\u0057\u0065\u006c\u0063\u006f\u006d\u0065\u002c\u0020\u004d\u0069\u0067\u0075\u0065\u006c\u0020\u0041\u006e\u0067\u0065\u006c\u0020\u0051\u0075\u0069\u006e\u0074\u0065\u006c\u0061\u0021");
				</script>
			</div>
		</div>
    <!--*******************************************************************************************-->
</div>


  
<asp:HiddenField ID="HiddenField1" runat="server" />
    
   <div id="divImagenLogo" runat="server" style="position:absolute;top:150px;left:40px;z-index:0">
        <img alt="" src="UI/HTML/Images/iberdrola.jpg" />
    </div>
    <div id="divSolicitudesAbiertas" runat="server"  style="position:absolute;top:100px;left:25px;z-index:0;height:500px;width:650px;overflow-y:scroll;overflow-x:hidden;border: 1px solid black;">
        <asp:GridView ID="gvSolicitudes" runat="server" CssClass="contenidoTabla" Width="97%" Height="100%" EnableModelValidation="True" OnDataBinding="gvSolicitudes_DataBinding" > 
            <RowStyle CssClass="filaNormal"/>
            <AlternatingRowStyle CssClass="filaNormal"/>
            <FooterStyle CssClass="cabeceraTabla"  />
            <PagerStyle  CssClass="cabeceraTabla" />
            <HeaderStyle CssClass="cabeceraTablaResumen" />
        </asp:GridView>
    </div>
    
    </form>
    <!-- Pie -->
    <div id="divPie">
            <div id="pieIzda"></div>
            <div id="pieCentro"></div>
            <div id="pieDcha"></div>
            <!--<div id="copyRight" style="position:absolute; top: 29px; left: 3px;" ><asp:Label ID="lblcopyright" runat="server" Text="© Iberdrola S.A. 2009 todos los derechos reservados."></asp:Label></div>-->
    </div>
    </body>
  
    <%--<script language='Javascript'>
    document.getElementById('2').style.left =200;
    </script>--%>
</html>