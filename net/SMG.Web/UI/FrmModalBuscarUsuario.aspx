<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true"
    CodeFile="FrmModalBuscarUsuario.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalBuscarUsuario" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalBuscarUsuario" ContentPlaceHolderID="ContentPlaceHolderContenido"
    runat="server">

    <asp:ScriptManager ID="SM" runat="server">
    </asp:ScriptManager>

    <div id="divPanelFiltroColumna" style="position: absolute; top: 18px; left: 20px;
        width: 605px; height: 320px;">
            <asp:Panel ID="pnBuscarPor" runat="server"
            Width="600px" CssClass="labelFormulario" Height="329px" 
                meta:resourcekey="pnBuscarPorResource1">
            <asp:Panel ID="pnLista" runat="server" Height="294px" ScrollBars="Auto" 
                Wrap="False" Width="595px" meta:resourcekey="pnListaResource1">
                
                

                <asp:TextBox ID="TextBox1" runat="server" Width="478px" 
                    meta:resourcekey="TextBox1Resource1"></asp:TextBox>
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                    CssClass="botonFormulario" onclick="btnBuscar_Click" Width="74px" 
                    meta:resourcekey="btnBuscarResource1"/>
                <br />
                <asp:ListBox ID="ListBox1" runat="server" Height="261px" Width="556px" 
                    meta:resourcekey="ListBox1Resource1">
                </asp:ListBox>
                
                
            
        </asp:Panel>
        </asp:Panel>

        
    </div>

  
    <div id="divBotonAceptar" style="position: absolute; width: 161px; top: 382px; left: 225px;
        height: 22px; margin-right: 1px;">
        <asp:Button ID="btnAceptar" runat="server" CssClass="botonFormulario" Text="Aceptar"
            Width="75px" OnClick="OnBtnAceptar_Click" TabIndex="11" 
            meta:resourcekey="btnAceptarResource1" />
        <asp:Button ID="btnCancelar" runat="server" CssClass="botonFormulario" Text="Cancelar"
            Width="75px" OnClick="OnBtnCancelar_Click" TabIndex="11" 
            meta:resourcekey="btnCancelarResource1" />
    </div>
</asp:Content>
