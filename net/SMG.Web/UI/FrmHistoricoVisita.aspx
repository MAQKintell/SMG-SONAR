<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmHistoricoVisita.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmHistoricoVisita"  EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmHistoricoVisita" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <style>
        th {
            position:relative;
            /*top: expression(document.getElementById("div-datagrid").scrollTop-2); /*IE5+ only*/
            }
    </style>

    <div>
        <table cellpadding="0" cellspacing="0" border="0" width="99%">
            <tr>
                <td width="45%">          
                    <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
                        Text="Historico de visitas" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
                </td>
                <td width="54%" align="right">

                </td>
             </tr>
         </table>
    </div>  
    <div>
         <table>
            <tr>
                <td>          
                    <asp:Label CssClass="labelFormulario" ID="Label2" runat="server" 
                        Text="Contrato: " meta:resourcekey="Label2Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtContrato"  runat="server" 
                        meta:resourcekey="txtContratoResource1"></asp:TextBox>
                    <asp:CustomValidator ID="txtContratoCustomValidator" runat="server" 
                            ControlToValidate="txtContrato" CssClass="errorFormulario" 
                        ErrorMessage="*" ToolTip="Introduzca un código de contrato." 
                            ValidateEmptyText="True" 
                        onservervalidate="txtContratoCustomValidator_ServerValidate" 
                        meta:resourcekey="txtContratoCustomValidatorResource1"></asp:CustomValidator>

                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                        CssClass="botonFormulario" onclick="btnBuscar_Click" 
                        meta:resourcekey="btnBuscarResource1" />
                </td>
             </tr>
         </table>
         
    </div>
    <div style="height: 475px">
    <!--<asp:Panel ID="Panel1" runat="server" Height="293px" ScrollBars="Auto" 
        Width="954px" CssClass="panelTabla" HorizontalAlign="Left" Wrap="False">-->
        
        <div style="border: 1px solid #000000; overflow:scroll; height:466px">
        <asp:GridView ID="dgVisitas" runat="server" CssClass="contenidoTabla" 
                EnableModelValidation="True" meta:resourcekey="dgVisitasResource1" > 
            <RowStyle CssClass="filaNormal"/>
            <AlternatingRowStyle CssClass="filaAlterna"/>
            
            <FooterStyle CssClass="cabeceraTabla"  />
            <PagerStyle  CssClass="cabeceraTabla" />
            <HeaderStyle CssClass="cabeceraTabla" />
        </asp:GridView>
        </div>
      <!--</asp:Panel>-->
    </div>
    <div id="divContador" style="text-align: center">
        

        
    </div>

    <asp:HiddenField ID="hdnNumeroVisitas" runat="server" />

    <div id="divBotonVolver">
        <asp:Button CssClass="botonFormulario" ID="btnVolver" runat="server" 
            Text="Volver" OnClick="OnBtbVolver_Click" TabIndex="1" 
            meta:resourcekey="btnVolverResource1" />
    </div>
</asp:Content>
