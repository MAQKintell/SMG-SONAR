<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" CodeFile="FrmModalAltaReclamacion.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalAltaReclamacion" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="FrmModalGenerarVisitas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <asp:HiddenField ID="hdnIdSolicitud" runat="server" />
    <asp:HiddenField ID="hdnEstado" runat="server" />
    <div id="divInformacionLote" style="position: absolute; width: 437px; top: 8px; left: 7px; height: 350px; margin-right: 1px;">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td class="campoTabla">
                    &nbsp;
                    <asp:Label ID="lblContrato" runat="server" ForeColor="Red" meta:resourcekey="lblContratoResource1"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lbladver3" runat="server" CssClass="labelFormulario" Text="OBSERVACIONES:" meta:resourcekey="lbladver3Resource1"></asp:Label>
                    &nbsp;
                    &nbsp;
                    <asp:Label CssClass="labelFormulario" ID="Label1" runat="server" Text="Proveedor:" meta:resourcekey="Label1Resource1"></asp:Label>
                    &nbsp;
                    <asp:DropDownList ID="cmbProveedor" runat="server" meta:resourcekey="cmbProveedorResource1"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="campoTabla">
                    <asp:TextBox ID="txtObservacionesAnteriores" runat="server" Maxlenght="8000" Width="433px"
                        TabIndex="1" Height="137px" TextMode="MultiLine" BackColor="#FFFFCC" Enabled="false"
                        meta:resourcekey="txtObservacionesAnterioresResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="campoTabla">
                    <asp:TextBox ID="txtObservaciones" runat="server" Maxlenght="4000" Width="433px"
                        TabIndex="1" Height="87px" TextMode="MultiLine"
                        meta:resourcekey="txtObservacionesResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="campoTabla">
                    <div id="divReclamacion" runat="server">
                        <asp:FileUpload class="file_1" ID="FileUpload1" runat="server" Width="199px" BackColor="White" CssClass="file_1" ForeColor="Black" meta:resourcekey="FileUpload1Resource1" />
                        &nbsp;
                        <asp:Button ID="btnAdjuntarFichero" runat="server" Text="Adjuntar fichero" onclick="btnAdjuntarFichero_Click" />
                    </div>
                    <div id="divProveedor" runat="server">
                        <asp:PlaceHolder ID="phDocumentos" runat="server">
                        </asp:PlaceHolder>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divBotonCancelar" style="position: absolute; width: 92px; top: 350px; left: 240px; height: 22px; margin-right: 1px;">
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="90px" OnClick="OnBtnCancelar_Click" TabIndex="5"
            meta:resourcekey="btnCancelarResource1" />
    </div>
    <div id="divBotonAceptar" style="position: absolute; width: 92px; top: 350px; left: 140px; height: 22px; margin-right: 1px;">
        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" Width="90px" OnClick="OnBtnAceptar_Click" TabIndex="4"
            meta:resourcekey="btnAceptarResource1" />
    </div>
    <div id="divBotonCerrar" style="position: absolute; width: 117px; top: 350px; left: 7px; height: 22px; margin-right: 1px;">
        <asp:Button ID="btnDevolver" runat="server" Text="Devolver a Proveedor" Width="115px" OnClick="btnCerrar_Click" TabIndex="3"
            meta:resourcekey="btnDevolverResource1" />
    </div>

    <div id="divBotonCerrarReclamacion" style="position: absolute; width: 92px; top: 350px; left: 340px; height: 22px; margin-right: 1px;">
        <asp:Button ID="btnCerrarReclamacion" runat="server" Text="Cerrar" Width="90px" OnClick="btnCerrarReclamacion_Click" TabIndex="6" BackColor="red" ForeColor="White" />
    </div>
</asp:Content>
