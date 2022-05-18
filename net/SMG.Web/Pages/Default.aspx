<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/master.css" type="text/css" rel="stylesheet" />
    <link href="../css/formularios.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .style1
        {
            width: 900px;
            height: 30px;
        }
        .style2
        {
            width: 450px;
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
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> <!--[if lt IE 7]><LINK 
href="../css/ventan-modal-ie.css" type=text/css rel=stylesheet><![endif]-->
	<script src="../js/ventana-modal.js" type="text/javascript"></script>
		
    <script type="text/javascript" >
function AbrirArgumentario() {
    //Alert('qqq');
    abrirVentanaLocalizacion("DatosArgumentario.aspx", 700, 400, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_DATOS_ARGUMENTARIO%>", "0", "1");
}




</script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="background-color: #ffffff;position:absolute; top: 164px; left: 33px; width: 70px; height: 25px;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/HOJAS.JPG" 
                    Height="297px" Width="615px" />
        </div>
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
        <div style="position:absolute; top: 98px; left: 10px;">
    <table cellpadding="0" cellspacing="0" class="style7" align="center">
       <tr>
       <td>
        <table class="style1" border="0" cellpadding="0" cellspacing="0" align="center">
            <tr>
                <td>
                    <table align="center" class="style2">
                        <tr>
                            <td>
                                <a href="info_visita.aspx" title="Info Visita">Ir a Info_visita</a>
                            </td>
                            <td>
                                <a href="Proveedores.aspx" title="Proveedores">Ir a Proveedores</a>
                            </td>
                            <td>
                                <a href="javascript:AbrirArgumentario();" title="Argumentario">Datos Argumentario</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </td>
        </tr>
        </table>
        </div>
    </form>   
</body>
</html>