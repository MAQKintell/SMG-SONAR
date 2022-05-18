<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" CodeFile="FrmModalAbrirSolicitudProveedor.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalAbrirSolicitudProveedor" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="FrmModalGenerarVisitas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <asp:HiddenField ID="hdnContrato" runat="server" />
    <asp:HiddenField ID="hdnTelefonoContacto" runat="server" />
    <asp:HiddenField ID="hdnCodVisita" runat="server" />
    
    <div id="divAltaSolicitudProveedor" style="border-style: solid; position: absolute; width: 405px; top: 0px; left: 0px; height: 205px; margin-right: 1px;">
        <table cellpadding="0" cellspacing="0" width="100%">
             <tr>
             
                <td>
                    <asp:Label ID="lblEditarSubtipo" runat="server" Text="Subtipo:" 
                            CssClass="labelFormulario" meta:resourcekey="lblEditarSubtipoResource1"></asp:Label>
                </td>
                <td>
                <asp:DropDownList ID="ddlSubtipo" BackColor="White" runat="server" 
                            CssClass="campoFormulario" AutoPostBack="True" Width="198px" 
                        OnSelectedIndexChanged="ddlSubtipo_SelectedIndexChanged"
                        meta:resourcekey="ddlSubtipoResource1">
                    </asp:DropDownList>
            </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDescrAveria" ForeColor="Firebrick" runat="server"
                            Text="Avería:" CssClass="labelFormulario" 
                            meta:resourcekey="lblDescrAveriaResource1"></asp:Label>
                </td>
                <td>
                        <asp:DropDownList ID="ddlAveria" BackColor="White" runat="server" 
                            CssClass="campoFormulario" Width="288px" 
                        meta:resourcekey="ddlAveriaResource1">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTelefono" runat="server" Text="Teléfono:" 
                        CssClass="labelFormulario" ></asp:Label>
                </td>
                <td>
                   <asp:TextBox ID="txtTelefono" BackColor="White" 
                        runat="server" Width="100px"></asp:TextBox>
            </td>
            </tr>
            <tr>
                <td>
                        <asp:Label ID="Label13" runat="server" Text="Observaciones:" 
                        CssClass="labelFormulario" meta:resourcekey="Label13Resource1"></asp:Label>
                </td>
                <td>
                        <asp:TextBox ID="txtEditarObservaciones" BackColor="White" TextMode="MultiLine" 
                            runat="server" Rows="2" Columns="50" Height="67px" Width="249px" 
                        meta:resourcekey="txtEditarObservacionesResource1"></asp:TextBox>
                </td>
                </tr>
            
           
        </table>
    
    
   <div id="divBotonAceptar" 
            style="position: absolute; width: 113px; top: 160px; left: 66px; height: 33px; margin-right: 1px;">
        <asp:Button ID="btnAceptar" runat="server" CssClass="botonFormulario" 
            Text="Aceptar" Width="115px"  
            OnClick="OnBtnAceptar_Click" TabIndex="4" Height="21px" 
            meta:resourcekey="btnAceptarResource1" />
    </div>
    <div id="divBotonCancelar"       
            style="position: absolute; width: 130px; top: 160px; left: 186px; height: 22px; margin-right: 1px;">
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="115px" CssClass="botonFormulario" 
            Height="21px" OnClick="OnBtnCancelar_Click" TabIndex="3" 
            meta:resourcekey="btnCancelarResource1" />
    </div>
    </div>
</asp:Content>
