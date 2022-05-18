<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true"
    CodeFile="FrmModalHistoricoVisita.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalHistoricoVisita" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalHistoricoVisita" ContentPlaceHolderID="ContentPlaceHolderContenido"
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
                
                

                <asp:GridView ID="grdDatos" runat="server" Height="17px" Width="540px" 
                    BackColor="WhiteSmoke" BorderColor="Black" BorderStyle="Solid" 
                    BorderWidth="1px" EnableModelValidation="True" 
                    meta:resourcekey="grdDatosResource1">
                </asp:GridView>
                
                
            
        </asp:Panel>
        </asp:Panel>

        
    </div>

  
    <div id="divBotonAceptar" style="position: absolute; width: 161px; top: 270px; left: 225px;
        height: 22px; margin-right: 1px;">
        <asp:Button ID="btnCancelar" runat="server" CssClass="botonFormulario" Text="Cerrar"
            Width="75px" OnClick="OnBtnCancelar_Click" TabIndex="11" 
            meta:resourcekey="btnCancelarResource1" />
    </div>
</asp:Content>
