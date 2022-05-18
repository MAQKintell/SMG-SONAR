<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProcesoCalderas.aspx.vb" Inherits="Pages_ProcesoCalderas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Opera SMG</title>
    <link href="../css/master.css" type="text/css" rel="stylesheet" />
    <link href="../css/formularios.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .style1
        {
            width: 900px;
            height: 30px;
        }
        .style4
        {
            width: 900px;
            border: 1px solid #003700;
            background-color: #FFFFCC;/*#003399; #FFFFCC;*/
        }
        .style8
        {
            width: 900px;
            height: 30px;
        }
        .style10
        {
            width: 283px;
        }
        #form1
        {
            height: 16px;
            width: 321px;
        }
        </style>
    <link href="../Estilos.css" rel="stylesheet" type="text/css" />
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> <!--[if lt IE 7]><LINK 
href="../css/ventan-modal-ie.css" type=text/css rel=stylesheet><![endif]-->
	
<script language="javascript" type="text/javascript">
// <!CDATA[

function Button1_onclick() {
    parent.VentanaModal.cerrar();//cerrarVentana();
}

// ]]>
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="position:absolute;height: 17px; width: 217px; top: 3px; left: 31px; right: 682px;">
    
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Italic="True" 
            ForeColor="Red" Text="CONTRATO:"></asp:Label>
        <asp:Label ID="lblCodContrato" runat="server" Font-Bold="True"></asp:Label>
    
    </div>
    
    <div runat="server" id="PANELNO" 
        
        style="background-color:Black; position:absolute; top: 34px; left: 20px; height: 262px; width: 559px;">
    <div style="position:absolute; top: 147px; left: 10px;">
        <asp:Image ImageUrl="../Imagenes/Alerta.jpg" runat="server"/>
        &nbsp;<asp:Label ID="Label2" runat="server" 
            Font-Bold="True" Font-Italic="True" Font-Size ="Large" 
            ForeColor="YELLOW" Text="NO HAY CALDERA ASOCIADA A ESTE CONTRATO"></asp:Label>
            <asp:Image ID="Image1" ImageUrl="../Imagenes/Alerta.jpg" runat="server"/>
    </div>
    </div>
    <div runat="server" id="PANELSI" 
        
        style="background-color:white; position:absolute; top: 34px; left: 13px; height: 264px; width: 559px;">
            <div style="border: thin double #000000; position:absolute; top: 25px; left: 16px; width: 471px;">
            <table>
            <tr>
                <td class="style1"><asp:Label ID="Label5" runat="server" Text="TIPO:"></asp:Label>
                </td>
                <td class="style1">
                <asp:TextBox ID="txtTipo" runat="server" Enabled=false Width="369px" ></asp:TextBox>
                </td>
                </tr>
                <tr><td class="style7"><asp:Label ID="Label51" runat="server" Text="MARCA:"></asp:Label>
                </td>
                <td class="style2">
                <asp:TextBox ID="txtMarca" runat="server" Enabled=false Width="369px" ></asp:TextBox>
                </td>
               
                </tr>
                <tr> <td class="style8"><asp:Label ID="Label52" runat="server" Text="MODELO:"></asp:Label></td>
                <td class="style3">
                <asp:TextBox ID="txtModelo" runat="server" Enabled=false Width="369px" ></asp:TextBox>
                </td>
                
                </tr>
                <tr><td class="style7"><asp:Label ID="Label53" runat="server" Text="USO:"></asp:Label>
                </td>
                <td class="style2">
                <asp:TextBox ID="txtUso" runat="server" Enabled=false Width="369px"></asp:TextBox>
                </td>
               
                </tr>
                <tr> <td class="style9"><asp:Label ID="Label54" runat="server" Text="POTENCIA:"></asp:Label>
                </td>
                <td class="style2">
                <asp:TextBox ID="txtPotencia" runat="server" Enabled="false" Width="369px" ></asp:TextBox>
                </td>
                
                </tr>
                <tr><td class="style10"><asp:Label ID="Label55" runat="server" Text="AÑO:"></asp:Label>
                </td>
                <td class="style5">
                <asp:TextBox ID="txtAño" runat="server" Enabled="false" Width="369px" ></asp:TextBox>
                </td>
                
                </tr>
                <tr></tr>
                <br />
                </table>
            </div>
            
           
           </div>
           
           <div style="position:absolute; top: 265px; left: 216px; width: 83px;">
            <input id="Button1" type="button" runat="server" value="CERRAR" onclick="return Button1_onclick()" 
                    style="font-weight: bold" />
           </div>
    </form>
</body>
</html>
