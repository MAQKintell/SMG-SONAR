<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Argumentario.aspx.vb" Inherits="Pages_Argumentario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Página sin título</title>
    <link href="../css/master.css" type="text/css" rel="stylesheet" />
    <link href="../css/formularios.css" type="text/css" rel="stylesheet" />
    <script language="javascript">
        if (document.all)
        { 
            document.onkeydown = function ()
            {
                //var key_f5 = 116; // 116 = F5 
                if (event.keyCode>17)
                {
                    if (event.keyCode==37)
                        return false;
                    if (event.keyCode==38)
                        return false;
                    if (event.keyCode==39)
                        return false;
                    if (event.keyCode==40)
                        return false;
                }
            }
        }
    </script>
    <style>
        .labelright{text-align:center;}
    </style>
    
    <style type="text/css">
        #form1
        {
            height: 30px;
            width: 0px;
        }
        

    </style>
    <link href="../Estilos.css" rel="stylesheet" type="text/css" />
</head>
<body>
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
    <div style="position:absolute; top: 61px; left: 162px; width: 656px;">
        <asp:Label ID="Label1" runat="server" Text="Contrato"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox 
            ID="txtContrato" runat="server" Width="257px" Enabled="False"></asp:TextBox>
        &nbsp;&nbsp;<asp:CheckBox ID="chkVisita" runat="server" Text="Visita realizada" Visible="true"/>    
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" Visible="False" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="btnAtras" runat="server" 
            ImageUrl="~/Imagenes/icono_atras.gif" ToolTip="Volver atrás" Visible="False" />
    </div>
    <div style="position:absolute; top: 20px; left: 200px; width: 777px;">
        <asp:Label ID="lblLeyenda" runat="server" Text="Label" Font-Bold="True" 
            Font-Size="Medium" ForeColor="Red" style="float:left; width:450px;
text-align:center;"></asp:Label>
    </div>
    <div style="position:absolute; top: 94px; left: 10px; width: 777px;">
        <asp:RadioButtonList ID="rdbPreguntas" runat="server" AutoPostBack="True">
        </asp:RadioButtonList>
    </div>
    
    <div style="border-style: double; position:absolute; top: 152px; left: 342px; width: 410px; height: 235px;">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblObservaciones" Visible="true" runat="server" Text="Observaciones" ForeColor="Red"></asp:Label><br />
        &nbsp;<asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Height="213px" 
            Width="399px" Visible="true"></asp:TextBox>
    </div>
    <div style="position:absolute; top: 67px; left: 13px;visibility:hidden">
    PROVEEDOR:&nbsp;<asp:Label ID="lblProveedor" runat="server" Text="Label" Font-Bold="True" 
             ForeColor="Red"></asp:Label></div>
 </form>
</body>
</html>