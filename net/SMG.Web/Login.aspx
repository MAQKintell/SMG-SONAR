<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Iberdrola.SMG.UI.Login" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="UI/HTML/Css/master.css" type="text/css" rel="stylesheet" />
    <link href="UI/HTML/Css/tablas.css" type="text/css" rel="stylesheet" />
    <link href="UI/HTML/Css/formularios.css" type="text/css" rel="stylesheet" />
    <link href="UI/HTML/Css/ventana-modal.css" type="text/css" rel="stylesheet" />
    <script src="UI/HTML/Js/ventana-modal.js" type="text/javascript"></script>
    <!--[if lt IE 7]><LINK href="css/ventan-modal-ie.css" type=text/css rel=stylesheet><![endif]-->
    <script type="text/javascript">

        
    
    

    //alert('<%=Resources.TextosJavaScript.TEXTO_DATOS_ARGUMENTARIO%>');


//function Alert(Texto)
//{


//    var Clave=Texto.toString().replace(" ","_");
//    
//    language=navigator.browserLanguage;
//    language=language.substring(0,2);
//    alert(language);


//    alert('ooo');
//}

        function MostrarCapaEspera() {

            var div = document.getElementById("divSplashScreen");
            
            div.style.visibility = "visible";

            var div1 = document.getElementById("divbotonIniciarSesion");
            div1.style.visibility = "hidden";
        }

		function AbrirVentana(NombreVentana){
            abrirVentanaLocalizacion(NombreVentana, 700, 630, "ventana-modal","ELEGIR ACCIÓN","0");
        }

        function AbrirVentanaSolicitudesCalderasConContrato(codContrato) {
            //abrirVentanaLocalizacion("UI/FrmSolicitudesCalderas.aspx?Contrato=" + codContrato, 700, 630, "ventana-modal", "ELEGIR ACCIÓN", "0");
            window.location.href = "UI/FrmSolicitudesCalderas.aspx?Contrato=" + codContrato;
        }
	</script>
	<style>
     #divSplashScreen
    {
        position:absolute;
        top:175px;
        left:175px;
        height:100px;
        width:200px;
        border: solid 1px #000000;
        background-color:#ffffff;
        padding:5px 5px 5px 5px;
        z-index:10001;
        visibility:hidden;
        cursor:wait;
    }
</style>

	
	<title>IBERDROLA - Opera SMG </title>
</head>
<body>
    <div id="divSplashScreen">
        <table border="0" cellpadding="0" cellspacing="0" width="1%" align="center">
            <tr>
                <td align="center">
                     <img src="UI/HTML/Images/loading.gif" alt="Cargando..."/>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <span class="labelFormularioValor"> Cargando... </span>
                </td>
            </tr>
        </table>
    </div>
    <form id="form1" runat="server" autocomplete="off">
    <asp:HiddenField runat="server" ID="hdnNumReintentos" Value="0" />      
    <div id="divImagenLogo" style="position:absolute;top:150px;left:40px;z-index:0">
        <img alt="" src="UI/HTML/Images/iberdrola.jpg" />
    </div>
    
     <div id="divCabecera">
       <div id="cabeceraIzda"></div>
        <div id="cabeceraCentro">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr> 
                        <td><img src="UI/HTML/Images/logoIberdrola.jpg" alt="logo IBERDROLA"/></td>
                        <td></td>
                        <td class="textoTituloCabecera" align="left">
                            <asp:Label CssClass="textoTituloCabecera" ID="lblTitulo" runat="server" 
                       text=" - OPERA SMG - " meta:resourcekey="lblTituloResource1"></asp:Label></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td class="" align="left"><font color="white" size="1">V 5.3</font></td>
                    </tr>
                </table>
        </div>
        <div id="cabeceraDcha"></div>    
    </div>
    <div id="divAvisos" runat="server" 
        style="position:absolute; top: 108px; left: 392px; width: 580px;">      
    </div>
    <div id="divDatos" 
        style="position:absolute; top: 172px; left: 539px; width: 373px;">
        <table cellpadding="0" cellspacing="0" style="width: 129%">
            <tr>
                <td class="labelTabla">
                    <asp:Label CssClass="labelFormulario" ID="lblUsuario" runat="server" 
                       text="Usuario:" meta:resourcekey="lblUsuarioResource1" ></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td class="campoTabla">
                    <asp:TextBox ID="txtUsuario" runat="server" Width="141px"
                        onkeyup="javascript:MostrarNIF(this);" 
                        onkeypress="javascript:MostrarNIF(this);" 
                        meta:resourcekey="txtUsuarioResource1"></asp:TextBox>
                    <asp:CustomValidator CssClass="errorFormulario" runat="server" ErrorMessage="*" ToolTip="*" OnServerValidate="OnRequired_ServerValidate" ValidateEmptyText="true" ID="CustomValidator1" ControlToValidate="txtUsuario" />
                    <%--<asp:RequiredFieldValidator CssClass="errorFormulario" 
                        ToolTip="Debe Rellenar datos para el Usuario" ID="RequiredFieldValidator1" 
                        runat="server" ErrorMessage="*" ControlToValidate="txtUsuario" 
                        meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>--%>
                </td>
                   <td>&nbsp;</td>
                <td class="labelTabla">
                    <asp:Label CssClass="labelFormulario" ID="lblNIF" runat="server" Text="NIF/NIE:"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td class="campoTabla">
                    <asp:TextBox ID="txtNIF" runat="server" Width="139px" MaxLength="10" ></asp:TextBox>
                <asp:CustomValidator CssClass="errorFormulario" runat="server" ErrorMessage="*"
                ToolTip="*" OnServerValidate="OnNIF_ServerValidate" ValidateEmptyText="true"
                    ID="txtNIFValidator" ControlToValidate="txtNIF" />
                </td>
            </tr>
             <tr>
                <td class="labelTabla"></td>
                <td>&nbsp;</td>
                <td class="campoTabla"></td>
            </tr>
            <tr>
                <td class="labelTabla">
                    <asp:Label CssClass="labelFormulario" ID="lblContrasena" runat="server" 
                        Text="Contraseña: " meta:resourcekey="lblContrasenaResource1"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td class="campoTabla">
                    <asp:TextBox ID="txtPassword" runat="server" Width="141px" TextMode="Password" 
                        meta:resourcekey="txtPasswordResource1"></asp:TextBox>
                    <asp:CustomValidator CssClass="errorFormulario" runat="server" ErrorMessage="*" ToolTip="*" OnServerValidate="OnRequired_ServerValidate" ValidateEmptyText="true" ID="CustomValidator2" ControlToValidate="txtPassword" />
                    <%--<asp:RequiredFieldValidator CssClass="errorFormulario" 
                        ToolTip="Debe Rellenar datos para el Password" ID="RequiredFieldValidator2" 
                        runat="server" ErrorMessage="*" ControlToValidate="txtPassword" 
                        meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
        </table>
    </div>

    <div id="divbotonIniciarSesion" style="position:absolute; top: 253px; left: 710px; width: 98px;">
        <asp:Button ID="Button1" runat="server" Text="Iniciar sesión" Width="95px" 
            CssClass="botonFormulario" onclick="Button1_Click" 
            meta:resourcekey="Button1Resource2"/>
        <%--<asp:Button ID="Button2" runat="server" Text="ContratoGC" Width="95px" style="visibility:visible"
            CssClass="botonFormulario"  
            meta:resourcekey="Button1Resource2" OnClick="Button2_Click"/>--%>
    </div>

    <div id="divbotonIniciarSesion0" style="position:absolute; top: 500px; left: 882px; width: 130px;visibility:hidden">
        <asp:ImageButton ID="ImageButton1" runat="server" 
            ImageUrl="~/Imagenes/Proveedores1_1.jpg" 
            onMouseEnter="javascript:Entrar();" onMouseLeave="javascript:Salir();" 
            AlternateText="Recordar Contraseña" BorderStyle="Solid" BorderWidth="1px" 
            Height="17px" ToolTip="Recordar Contraseña" Width="124px" 
            onclick="ImageButton1_Click" meta:resourcekey="ImageButton1Resource1" />
        <br />
        <div style="position:absolute; top: 1px; left: 4px;">
            <asp:Label ID="Label1" style="cursor:pointer" runat="server" CssClass="copyRight" ForeColor="White" 
                Text="Recordar Contraseña" Width ="124px" Font-Bold="True" 
                onMouseEnter="javascript:Entrar();" onMouseLeave="javascript:Salir();" 
                meta:resourcekey="Label1Resource1"></asp:Label>
        </div>
    </div>
    
    <div class="copyRight" style="position:absolute; top: 500px; left: 100px;visibility:hidden">
        <asp:Label ID="Label2" runat="server" Text="Se ha incluido una gestión de usuarios en la Base de Datos. 
        Si no dispones de un usuario, por favor manda un correo a Label" 
            meta:resourcekey="Label2Resource1"></asp:Label>
        <a href="mailto:a.lazaro@iberdrola.es">a.lazaro@iberdrola.es</a>.
        <br />
        <br />
        <asp:Label ID="Label22" runat="server" 
            Text="Si se le ha olvidado la contraseña, por favor mande un correo a " 
            meta:resourcekey="Label22Resource1"></asp:Label>
        <a href="mailto:a.lazaro@iberdrola.es">a.lazaro@iberdrola.es</a>.
    </div>

    <!-- Pie -->
    <div id="divPie">
       <table cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td>
                  <div id="pieIzda"></div>
                    <div id="pieCentro"></div>
                    <div id="pieDcha"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <!--<div class="copyRight"><asp:Label ID="lblcopyright" runat="server" Text="© Iberdrola S.A. 2009 todos los derechos reservados."></asp:Label></div>-->
                </td>
            </tr>
        </table>
    </div>
   
   <script type="text/javascript">
        function Entrar()
        {
            //Alert(document.getElementById("ImageButton1").src);
            document.getElementById("ImageButton1").src=document.getElementById("ImageButton1").src.replace('Proveedores1_1', 'Proveedores_1');//'~/Imagenes/Proveedores_1.jpg'

            document.getElementById("Label1").style.color='Black';
        }
        function Salir()
        {
            //Alert('Salir');
            document.getElementById("ImageButton1").src=document.getElementById("ImageButton1").src.replace('Proveedores_1', 'Proveedores1_1');//'~/Imagenes/Proveedores_1.jpg'
            
            document.getElementById("Label1").style.color='White';
        }

        function abrirVentana() {
            //var window_width = 1010;
            //var window_height = 700;
            //var newfeatures= 'scrollbars=NO, menubar=NO, toolbar=NO,resizable=no';
            //var window_top = (screen.height-window_height)/2;
            //var window_top = 0;
            //var window_left = (screen.width-window_width)/2;
            window.location.href = "MenuAcceso.aspx";
            //newWindow=window.open("MenuAcceso.aspx", "MantenimientoGas",'width=' + window_width + ',height=' + window_height + ',top=' + window_top + ',left=' + window_left + ',features=' + newfeatures + '');
            //// cerramos la ventana actual si no venimos del MantenimientoGas
            //var ventana = window.self;
            //ventana.opener = window.self;
            //if (!window.opener.name || window.opener.name != "MantenimientoGas") {
            //    ventana.close();
            //}
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

        function MostrarNIF(texto) {
            var condiciones = '<%= Session["SESSION_CONDICIONES_GENERALES"] %>';

            if (condiciones.toUpperCase() == "TRUE") {

                if (EsUsuarioIberdrola(texto) || texto.value.length == 0 || EsUsuarioExcepcion(texto)) {
                    document.getElementById('txtNIF').style.visibility = "hidden";
                    document.getElementById('txtNIF').value = "";
                    document.getElementById('lblNIF').style.visibility = "hidden";
                    document.getElementById('txtNIFValidator').style.visibility = "hidden";
                }
                else
                {
                    document.getElementById('txtNIF').style.visibility = "visible";
                    document.getElementById('lblNIF').style.visibility = "visible";
                }
            }
        }

        function EsUsuarioIberdrola(texto) {
            
            if (texto != null && texto != '') {
                var usuariosIberdrola = '<%= Iberdrola.SMG.BLL.Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CONF_USUARIOS_IBERDROLA).Valor %>'.split(';');

                if (usuariosIberdrola != null && usuariosIberdrola != '' && texto.value.length > 0) {
                    for (var i = 0; i < usuariosIberdrola.length; i++) {
                        if (texto.value.substring(0, 1).toUpperCase() == usuariosIberdrola[i]) {
                            return true;
                            break;
                        }
                    }
                }
            }
        }

        function EsUsuarioExcepcion(texto) {

            if (texto != null && texto != '') {
                var usuariosExcepcion = '<%= Iberdrola.SMG.BLL.Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CONF_USUARIOS_EXCEPCION).Valor %>'.split(';');

                if (usuariosExcepcion != null && usuariosExcepcion != '' && texto.value.length > 0) {
                    for (var i = 0; i < usuariosExcepcion.length; i++) {
                        if (texto.value.toUpperCase() == usuariosExcepcion[i]) {
                            return true;
                            break;
                        }
                    }
                }
            }
        }
    </script>
   </form>
   <asp:PlaceHolder ID="_PlaceHolderScript" runat="server"></asp:PlaceHolder>
   </body>
</html>