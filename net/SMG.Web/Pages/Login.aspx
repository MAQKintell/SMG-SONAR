<%@ Page ValidateRequest="false" Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Pages_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/master.css" type="text/css" rel="stylesheet" />
    <link href="../css/formularios.css" type="text/css" rel="stylesheet" />
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> <!--[if lt IE 7]><LINK 
    href="css/ventan-modal-ie.css" type=text/css rel=stylesheet><![endif]-->
    <script src="../js/ventana-modal.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 423px;
            height: 141px;
        }
        .style2
        {
            width: 388px;
        }
        .style4
        {
            width: 900px;
            border: 1px solid #003399;
            background-color: #FFFFCC;
        }
        .style6
        {
            height: 30px;
        }
        .style7
        {
            width: 900px;
            height: 38px;
        }
        .style8
        {
            height: 36px;
        }
        .style9
        {
            width: 900px;
        }
        .style10
        {
            width: 182px;
        }
        .style11
        {
            width: 120px;
        }
        .style12
        {
            width: 138px;
        }
        .style13
        {
            width: 148px;
        }
        .style14
        {
            width: 106px;
        }
        .style15
        {
            width: 242px;
        }
        .style16
        {
            width: 88px;
        }
    </style>
    <link href="../Estilos.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript">
		function AbrirVentana(NombreVentana){
            //Alert(NombreVentana);
		    abrirVentanaLocalizacion(NombreVentana, 700, 630, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_ELEGIR_ACCION%>", "0");
		}
//		function aaa(){
//		    Alert('aaa');
//		    window.open ('http://localhost/GesSMG/Pages/excel/Solicitudes_300609_1106.xls','excel');
//		}
	</script>
</head>
<body onload=""> 

    <div id="div1" 
        style="position:absolute;top:86px; left:-48px; z-index:0; height: 402px;">
        <img alt="" src="../Imagenes/iberdrola.jpg" 
            style="height: 421px; width: 813px" />
    </div>
    
    <form id="form1" runat="server">
    
        
    <div id="divCabecera">
       <div id="cabeceraIzda"></div>
        <div id="cabeceraCentro">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr> 
                        <td><img src="../Imagenes/logoIberdrola.jpg" alt="logo IBERDROLA"/></td>
                        <td></td>
                        <td class="textoTituloCabecera" align="left"> - GESTIÓN DE GAS - </td>
                        <td></td>
                    </tr>
                </table>
        </div>
        <div id="cabeceraDcha"></div>    
    </div>
        
        
        
   <div style="position:absolute; top: 48px; left: 10px; width: 19px;">
        <table style="position:absolute; top: 80px; left: 489px;" class="style1" 
            border="5" cellpadding="0" cellspacing="0">
            <tr>
                <td width="450">
                    <table align="center" class="style2">
                        <tr>
                            <td width="100">
                                &nbsp;Usuario:</td>
                            <td width="150">
                                &nbsp;<asp:TextBox ID="txt_Login" runat="server" CssClass="TEXTBOX"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="100">
                                &nbsp;Contraseña:</td>
                            <td width="150">
                                &nbsp;<asp:TextBox ID="txt_Password" runat="server" CssClass="TEXTBOX" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="style8" colspan="2">
                                <br />
                                <br />
                                <asp:Button ID="btn_Entrar" runat="server" Text="Entrar" 
                                    CssClass="BUTTON" CausesValidation="False" Width="378px" />
                            </td>
                        </tr>
                         <tr>
                            <td class="style8" colspan="2">
                                <asp:Label ID="lbl_error" runat="server" Text="Usuario y/o contraseña no validos"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>              
            </tr>
        </table>
    </form>   
    
    
    
</body>
</html>

