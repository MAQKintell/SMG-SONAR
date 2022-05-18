<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmUltimaVisita.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmUltimaVisita" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmUltimaVisita" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Datos última visita a consultar" 
            meta:resourcekey="lblTituloVentanaResource1" ></asp:Label>
    </div>
    <br />
    <div>
        <table> 
            <tr>
                <td class="labelTabla">
                    <asp:Label CssClass="labelFormulario" ID="lblCodContrato" runat="server" 
                        Text="Código del contrato: " meta:resourcekey="lblCodContratoResource1"></asp:Label>
                </td>
                <td class="campoTabla">
                    <asp:TextBox ID="txtCodContrato" runat="server" Width="60px" TabIndex="1" 
                        MaxLength="10" meta:resourcekey="txtCodContratoResource1" ></asp:TextBox>
                    <asp:CustomValidator ID="txtCodContratoCustomrValidator" runat="server" 
                        CssClass="errorFormulario" ControlToValidate="txtCodContrato"  ErrorMessage="*" 
                        ToolTip="Rellene el código del contrato." 
                        OnServerValidate="onTxtCodContrato_ServerValidate" ValidateEmptyText="True" 
                        meta:resourcekey="txtCodContratoCustomrValidatorResource1"></asp:CustomValidator>
                     
                </td>
            </tr>
        </table>
    </div>
    
     <div id="divBotonAceptar" style="position:absolute; top: 95px; left: 116px;">
        <asp:Button CssClass="botonFormulario" ID="btnAceptar" runat="server" 
             Text="Aceptar" OnClick="OnBtbAceptar_Click" TabIndex="2" 
             meta:resourcekey="btnAceptarResource1"/>
    </div>
    <div id="divBotonVolver">
        <asp:Button CssClass="botonFormulario" ID="btnVolver" runat="server" 
            Text="Volver" OnClick="OnBtbVolver_Click" TabIndex="3" 
            meta:resourcekey="btnVolverResource1"/>
    </div>
    

</asp:Content>
