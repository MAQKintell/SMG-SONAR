<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmDetalleContrato.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmDetalleContrato"  EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmDetalleContrato" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <style>
        th {
            position:relative;
            /*top: expression(document.getElementById("div-datagrid").scrollTop-2); /*IE5+ only*/
            }
    </style>

    <div style="position:absolute; top: 50px">
        <table cellpadding="0" cellspacing="0" border="0" width="99%">
            <tr>
                <td width="45%">          
                    <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
                        Text="Listado de visitas" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
                </td>
                <td width="54%" align="right">
                    <asp:Label CssClass="labelFormulario" ID="lblContrato" runat="server" 
                        Text="Id. Contrato: " meta:resourcekey="lblContratoResource1"></asp:Label>
                    <asp:Label CssClass="labelFormularioValor" ID="lblIdContrato" runat="server" 
                        meta:resourcekey="lblIdContratoResource1"></asp:Label>        
                </td>
             </tr>
         </table>
    </div>  
    <div id= "divBotonBuscar"
        style="position:absolute; top: 12px; left: 240px; height: 19px; width: 63px;" 
        align="left">
        <asp:Button ID="btnBuscar" CssClass="botonFormulario" runat="server" 
            Text="Buscar" OnClick="BtnBuscar_OnClick" OnClientClick="MostrarCapaEspera();" 
            TabIndex="4" meta:resourcekey="btnBuscarResource1"/>
     </div>
     
<div id="divContr" style="position:absolute; top: 12px; left: 3px;z-index:1000">
            <asp:PlaceHolder ID="placeControlesOp9" runat="server"> 
                <asp:Label ID="Label2" CssClass="labelFormulario" runat="server" 
                    Text="Código Contrato: " meta:resourcekey="Label2Resource1"></asp:Label>
                   <asp:TextBox ID="txtCodigoContrato" CssClass="campoFormulario" width="120px" 
                    runat="server" MaxLength="20" meta:resourcekey="txtCodigoContratoResource1" ></asp:TextBox>
                <asp:Label ID="Label3" Visible="false" runat="server" 
                    CssClass="labelFormulario" ForeColor="Red" Font-Bold="true" 
                    Text=" Para PEGAR un contrato pulse a la vez las teclas(Ctrl + V)." 
                    meta:resourcekey="Label3Resource1"></asp:Label>
                
            </asp:PlaceHolder>
            </div>
            
            
    <div style="height: 475px; position:absolute; top: 75px">
    <!--<asp:Panel ID="Panel1" runat="server" Height="293px" ScrollBars="Auto" 
        Width="954px" CssClass="panelTabla" HorizontalAlign="Left" Wrap="False">-->
        
        <div style="border: 1px solid #000000; overflow:scroll; height:466px;">
        <asp:GridView ID="dgVisitas" runat="server" AutoGenerateColumns="False" CssClass="contenidoTabla" 
                OnRowDataBound="OnGrdListado_RowDataBound" EnableModelValidation="True" 
                meta:resourcekey="dgVisitasResource1"> 
            <RowStyle CssClass="filaNormal"/>
            <AlternatingRowStyle CssClass="filaAlterna"/>
            <Columns >
                <asp:TemplateField HeaderText="Cód. Visita" SortExpression="COD_VISITA" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="65px" 
                    HeaderStyle-Width="65px" meta:resourcekey="TemplateFieldResource1">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk2" runat="server" Text='<%# Bind("COD_VISITA") %>' 
                            OnClick="OnRowSelected_Click" meta:resourcekey="lnk2Resource1"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>   
                <asp:TemplateField HeaderText="Fecha Visita" SortExpression="FEC_VISITA" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="65px" 
                    HeaderStyle-Width="65px" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:Label ID="FEC_VISITA" runat="server" Text='<%# Bind("FEC_VISITA", "{0:dd/MM/yyyy}") %>'
                            CssClass="campoFormulario" meta:resourcekey="FEC_VISITAResource1"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>   
                <asp:BoundField DataField="DES_ESTADO" HeaderText="Estado Visita" 
                    SortExpression="DES_ESTADO" ItemStyle-HorizontalAlign="Center" 
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="160px" 
                    HeaderStyle-Width="160px" meta:resourcekey="BoundFieldResource1">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="REPARACION" HeaderText="Reparación" 
                    SortExpression="REPARACION" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="75px" 
                    HeaderStyle-Width="75px" meta:resourcekey="BoundFieldResource2">
<HeaderStyle HorizontalAlign="Center" Width="75px"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" Width="75px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="IMPORTE_REPARACION" HeaderText="Imp. Reparación" 
                    SortExpression="IMPORTE_REPARACION"  ItemStyle-HorizontalAlign="Right"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="95px" 
                    HeaderStyle-Width="95px" meta:resourcekey="BoundFieldResource3">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" 
                    SortExpression="OBSERVACIONES" ItemStyle-HorizontalAlign="Left" 
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="465px" 
                    HeaderStyle-Width="401px" meta:resourcekey="BoundFieldResource4">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
            </Columns>
            <FooterStyle CssClass="cabeceraTabla"  />
            <PagerStyle  CssClass="cabeceraTabla" />
            <HeaderStyle CssClass="cabeceraTabla" />
        </asp:GridView>
        </div>
      <!--</asp:Panel>-->
    </div>
    <div id="divContador" style="text-align: center">
        
        <asp:Label  CssClass="labelFormularioValor" ID="lbContador" runat="server" style="text-align: center" 
            Text="x" meta:resourcekey="lbContadorResource1"></asp:Label>
        <asp:Label  CssClass="labelFormulario" ID="Label1" runat="server" style="text-align: center" 
            Text=" visitas encontradas." meta:resourcekey="Label1Resource1"></asp:Label>
        
    </div>

    <asp:HiddenField ID="hdnNumeroVisitas" runat="server" />

    <div id="divBotonVolver">
        <asp:Button CssClass="botonFormulario" ID="btnVolver" runat="server" 
            Text="Volver" OnClick="OnBtbVolver_Click" TabIndex="1" 
            meta:resourcekey="btnVolverResource1" />
    </div>
</asp:Content>
