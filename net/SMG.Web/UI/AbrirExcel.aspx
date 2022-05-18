<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AbrirExcel.aspx.cs" Inherits="Iberdrola.SMG.UI.AbrirExcel" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"%>
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
            //alert('QQ');
            var div = document.getElementById("divSplashScreen");
            div.style.visibility = "hidden";
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

<body style="background-image: url('../../Imagenes/fondoExcel.PNG');background-repeat: repeat-x; background-color: #FFFFFF;">
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
    
        <div style="position:absolute; top: 20px; left: 21px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/Excel.png" 
                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" meta:resourcekey="Image1Resource1" 
                 />
        </div>        
        <div style="position:absolute; top: 143px; left: 343px;">
            <asp:Button id="btn_exportar0" runat="server" Text="Exportar excel" 
                CausesValidation="False"  onclick="btn_exportar0_Click" 
                class="botonFormulario" meta:resourcekey="btn_exportar0Resource1" 
                 />
            <br />
            <br />
            <asp:LinkButton ID="lnk_Excel" runat="server" meta:resourcekey="lnk_ExcelResource1" 
                ></asp:LinkButton>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Exportar excel CON FORMATO" 
                class="botonFormulario" CausesValidation="False" Width="172px" Height="32px" 
                onclick="Button1_Click" meta:resourcekey="Button1Resource1"  Visible="False" />
         </div>
    </form>
</body>
</html>
