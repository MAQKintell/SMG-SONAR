﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AvisoSolicitudes.aspx.cs" Inherits="Iberdrola.SMG.UI.AvisoSolicitudes"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AbrirExcel</title>
    
    <link href="../css/formularios.css" type="text/css" rel="stylesheet" />
    <script language="javascript">
        function MostrarCapaEspera()
        {
            var div = document.getElementById("divSplashScreen");
            div.style.visibility = "visible";
        }
        function OcultarCapaEspera()
        {
            //Alert('QQ');
            var div = document.getElementById("divSplashScreen");
            div.style.visibility = "hidden";
        }
        function Ocultar()
        {
            //alert(document.getElementById("Button1").Visible);
            document.getElementById("Button1").style.visibility = "hidden";
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
</head>

<body >
    <form id="form1" runat="server" style="background-image: url('../../Imagenes/fondoExcel.PNG'); background-repeat: repeat-x; background-color: #D3FFCC;">
     <div id="divSplashScreen" >
        <table border="0" cellpadding="10" cellspacing="2" width="100%" align="center">
            <tr>
                <td align="center">
                     <img src="HTML/Images/loading.gif" alt="Cargando..."/>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <span class="labelFormularioValor"> Cargando... </span>
                </td>
            </tr>
        </table>
    </div>
    <div style="position:absolute; top: 36px; left: 21px; height: 83px;">
            <asp:TextBox ID="txtAvisosAnteriores" runat="server" Height="84px" 
                Width="538px" TextMode="MultiLine" CssClass="TEXTBOX" Enabled="false"></asp:TextBox>
        </div>
        <div style="position:absolute; top: 127px; left: 21px; height: 83px;">
            <asp:TextBox ID="txtAviso" runat="server" Height="84px" Width="538px" 
                TextMode="MultiLine" CssClass="TEXTBOX"></asp:TextBox>
        </div>        
        <div style="position:absolute; top: 250px; left: 384px;">
            <asp:Button ID="Button1" runat="server" Text="GUARDAR" 
                class="botonFormulario" CausesValidation="False" Width="172px" Height="32px" 
                onclick="Button1_Click" Visible="true" OnClientClick ="Ocultar();" />
         </div>
    <input id="hdnSolicitud" runat="server" type="hidden" />
    <input id="hdnContrato" runat="server" type="hidden" />
    <input id="hdnMail" runat="server" type="hidden" />
    </form>
</body>
</html>
