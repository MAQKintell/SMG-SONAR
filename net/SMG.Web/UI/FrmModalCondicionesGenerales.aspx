<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" CodeFile="FrmModalCondicionesGenerales.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalCondicionesGenerales" %>

<asp:Content ID="FrmModalCondicionesGenerales" ContentPlaceHolderID="ContentPlaceHolderContenido"
	runat="server">
    
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" Text="Aceptación Condiciones Generales" />
    </div>

    <div id="divDatosGenerales" style="position: absolute; top: 30px; left: 20px;">
        <asp:Panel id="pnDatosGenerales" runat="server">
            <asp:TextBox ID="txtCondicionesGenerales" runat="server" Text="" TextMode="MultiLine" ReadOnly="true"
                Width="600px" Height="325px" />
            <asp:CheckBox ID="chkAceptarCondiciones" runat="server" CssClass="campoFormulario"
                Text="He leído y acepto las condiciones generales." />
            <asp:CustomValidator CssClass="errorFormulario" runat="server" 
                ErrorMessage="*" OnServerValidate="OnChkAceptarCondiciones_ServerValidate" ValidateEmptyText="true"
                ID="chkAceptarCondicionesValidator" ControlToValidate="txtCondicionesGenerales" />
        </asp:Panel>
    </div>
  
    <div id="divBotonAceptar" style="position: absolute; width: 161px; top: 382px; left: 225px;
        height: 22px; margin-right: 1px;">
        <center>
            <asp:Button ID="btnAceptar" runat="server" CssClass="botonFormulario" Text="Aceptar" Width="75px" OnClick="OnBtnAceptar_Click" />
            <asp:Button ID="btnCancelar" runat="server" CssClass="botonFormulario" Text="Cancelar" Width="75px" OnClick="OnBtnCancelar_Click"  />
        </center>
    </div>
</asp:Content>
