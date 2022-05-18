<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Busqueda.aspx.vb" Inherits="Pages_Busqueda" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/master.css" type="text/css" rel="stylesheet" />
    <link href="../css/formularios.css" type="text/css" rel="stylesheet" />
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 900px;
            height: 28px;
        }
        .style3
        {
            height: 30px;
        }
        .style4
        {
            width: 900px;
            border: 1px solid #003399;
            background-color: #dce4d9;/*#FFFFCC;*/
        }
        .style7
        {
            height: 28px;
            width: 900px;
        }
        .style8
        {
            width: 325px;
        }
        .style9
        {
            height: 30px;
            width: 73px;
        }
        .style10
        {
            height: 30px;
            width: 74px;
        }
    </style>
    <link href="../Estilos.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
        function UnLoad()
        { 
            if (event.clientY < 0)
            {
               // Alert("Cerrando Web Explorer");
            }
        }
    
        function Resaltar_On(GridView)
        {
            if(GridView != null)
            {
                GridView.originalBgColor = GridView.style.backgroundColor;
                GridView.style.backgroundColor="#DBE7F6";
            }
        }

        function Resaltar_Off(GridView)
        {
            if(GridView != null)
            {
                GridView.style.backgroundColor = GridView.originalBgColor;
            }
        }

    </script>
</head>
<body onunload="javascript:UnLoad();">
    <form id="form1" runat="server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
        ShowMessageBox="True" ShowSummary="False" />
    <div id="divCabecera">
       <div id="cabeceraIzda"></div>
        <div id="cabeceraCentro">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr> 
                        <td><img src="../Imagenes/logoIberdrola.jpg" alt="logo IBERDROLA"/></td>
                        <td></td>
                        <td class="textoTituloCabecera" align="left"> - OPERA SMG - </td>
                        <td></td>
                    </tr>
                </table>
        </div>
        <div id="cabeceraDcha"></div>    
    </div>
        <div style="position:absolute; top: 61px; left: 10px;">
    <table cellpadding="0" cellspacing="0" class="style7">
        <tr>
            <td valign="top" class="style8">
                <asp:Label ID="lbl_titulo" runat="server" CssClass="TITULO1" 
                    Text="Servicio mantenimiento de Gas"></asp:Label>
            </td>
            
          
            <td align="center">
            

            
            </td>
        </tr>
    </table>    
        <table cellpadding="0" cellspacing="0" class="style1">
        <tr>
            <td width="250">
                &nbsp;<asp:Label ID="lbl_Busqueda" runat="server" Text="Búsqueda avanzada de clientes" 
                                    CssClass="TITULO2"></asp:Label></td>
                <td align="right">
                <asp:Label ID="lbl_Registros" runat="server" 
                                    CssClass="MENSAJE" ForeColor="#990000"></asp:Label>&nbsp;&nbsp;
                </td>
        </tr>
    </table>
    <table class="style4">
        <tr>
            <td>
        <table cellpadding="0" cellspacing="1" class="style6">


            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" class="style1">
                        <tr>
                            <td class="style9">
                                <asp:Label ID="Label1" runat="server" Text="Nombre:"></asp:Label></td>
                            <td class="style3" width="160">
                                <asp:TextBox ID="txt_NombreBus" runat="server" CssClass="TEXTBOX" Width="125px"></asp:TextBox>
                            </td>
                            <td class="style9"><asp:Label ID="Label111" runat="server" Text="Apellido1:"></asp:Label></td>
                            <td class="style3" width="160">
                                <asp:TextBox ID="txt_Apellido1Bus" runat="server" CssClass="TEXTBOX" 
                                    Width="125px"></asp:TextBox>
                            </td>
                            <td class="style10"><asp:Label ID="Label112" runat="server" Text="Apellido2:"></asp:Label></td>
                            <td class="style3" align="left">
                                <asp:TextBox ID="txt_Apellido2Bus" runat="server" CssClass="TEXTBOX" 
                                    Width="125px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style9"><asp:Label ID="Label113" runat="server" Text="Calle:"></asp:Label></td>
                            <td class="style3">
                                <asp:TextBox ID="txt_CalleBus" runat="server" CssClass="TEXTBOX" Width="125px"></asp:TextBox></td>
                            <td class="style9"><asp:Label ID="Label114" runat="server" Text="Población:"></asp:Label>
                            </td>
                            <td class="style3">
                            <asp:TextBox ID="txt_PoblacionBus" runat="server" CssClass="TEXTBOX" Width="125px"></asp:TextBox>
                                </td>
                            <td class="style10"><asp:Label ID="Label115" runat="server" Text="Contrato:"></asp:Label></td>
                            <td class="style3" align="left">
                            <asp:TextBox ID="txt_ContratoBus0" runat="server" CssClass="TEXTBOX" Width="125px"></asp:TextBox>
                            </td>
                        </tr>
                                                <tr>
                            <td class="style9">
                               </td>
                            <td class="style3">
                                &nbsp;</td>
                            <td class="style9">
                                
                            </td>
                            <td class="style3">
                                &nbsp;</td>
                            <td class="style10">
                                
                            </td>
                            <td class="style3">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btn_Busqueda" runat="server" Text="Buscar cliente" 
                                    CssClass="BUTTON" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>                
        </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" class="style1">
        <tr>
            <td class="TEXTBOX">
                <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    AutoGenerateColumns="False" Width="900px" AllowPaging="True">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" Text=">" CommandName="Select" 
                            ImageUrl="~/Imagenes/flecha.gif">
                        <ControlStyle CssClass="BUTTON" Height="15px" Width="15px" />
                        </asp:ButtonField>
                      <asp:BoundField DataField="COD_CONTRATO_SIC" HeaderText="Contrato"  ReadOnly="True">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="NOM_TITULAR" HeaderText="Nombre"  ReadOnly="True">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="APELLIDO1" HeaderText="Apellido1"  ReadOnly="True">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="APELLIDO2" HeaderText="Apellido2"  ReadOnly="True">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="NOM_CALLE" HeaderText="Calle"  ReadOnly="True">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="NOM_POBLACION" HeaderText="Población"  ReadOnly="True">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" class="style7">
        <tr>
            <td valign="top" class="style8">
                &nbsp;</td>
            
          
            <td align="right">
            
                                <asp:Button ID="btn_linkVisitas" runat="server" CssClass="BUTTON" 
                    Text="Visitas" ToolTip="Ir a visitas sin consultar" TabIndex="100" Visible="False" />
            
            &nbsp;
            
            &nbsp;
            
            </td>
        </tr>
    </table>  
    </div>
<asp:Label ID="lbl_Mensaje" runat="server" Visible="False"></asp:Label><br />

    <asp:RegularExpressionValidator ID="val_NombreREV" runat="server" 
        ErrorMessage="El nombre debe contener al menos 4 caracteres" 
        ControlToValidate="txt_NombreBus" Display="None" 
        ValidationExpression="^[a-zA-Z&quot;|á|é|í|ó|ú|ñ|Ñ'\s]{4,50}$"></asp:RegularExpressionValidator>
<br />
    <asp:RegularExpressionValidator ID="val_apellido1REV" runat="server" 
        ErrorMessage="El apellido1 debe contener al menos 4 caracteres" 
        ControlToValidate="txt_Apellido1Bus" Display="None" 
        ValidationExpression="^[a-zA-Z&quot;|á|é|í|ó|ú|ñ|Ñ'\s]{4,50}$"></asp:RegularExpressionValidator>
 <br />
    <asp:RegularExpressionValidator ID="val_apellido2REV" runat="server" 
        ErrorMessage="El apellido2 debe contener al menos 4 caracteres" 
        ControlToValidate="txt_Apellido2Bus" Display="None" 
        ValidationExpression="^[a-zA-Z&quot;|á|é|í|ó|ú|ñ|Ñ'\s]{4,50}$"></asp:RegularExpressionValidator>   

 <br />
    <asp:RegularExpressionValidator ID="val_calleREV" runat="server" 
        ErrorMessage="La calle debe contener al menos 4 caracteres" 
        ControlToValidate="txt_CalleBus" Display="None" 
        ValidationExpression="^[a-zA-Z&quot;|á|é|í|ó|ú|ñ|Ñ'\s]{4,50}$"></asp:RegularExpressionValidator> 
        
         <br />
    <asp:RegularExpressionValidator ID="val_poblacionREV" runat="server" 
        ErrorMessage="La población debe contener al menos 4 caracteres" 
        ControlToValidate="txt_PoblacionBus" Display="None" 
        ValidationExpression="^[a-zA-Z&quot;|á|é|í|ó|ú|ñ|Ñ'\s]{4,50}$"></asp:RegularExpressionValidator>
                 <br />
                 
    <asp:RegularExpressionValidator ID="val_contratoREV" runat="server" 
        ErrorMessage="El contrato debe contener al menos 6 digitos" 
        ControlToValidate="txt_ContratoBus0" Display="None" 
        ValidationExpression="^[0-9]{6,12}$"></asp:RegularExpressionValidator>            
    </form>





</body>
</html>
