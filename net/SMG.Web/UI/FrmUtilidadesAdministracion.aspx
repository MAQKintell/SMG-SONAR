<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" CodeFile="FrmUtilidadesAdministracion.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmUtilidadesAdministracion" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="Iberdrola.Commons.Web" %>

<asp:Content ID="FrmUtilidadesAdministracion" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">   

    <div id="divTituloVentana">
        <asp:Label Text="Utilidades Administración" ID="lblTituloVentana" 
            CssClass="divTituloVentana" runat="server" 
            meta:resourcekey="lblTituloVentanaResource1"/>
    </div>
         
    <div id="divBotonesUtilidades" style="position: absolute; top: 20px; left: 75px;">
        <asp:Panel ID="pnProcesos" runat="server" 
            Height="137px" width="562px" meta:resourcekey="pnProcesosResource1" >
            <table>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>                                
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Button CssClass="botonFormulario" ID="btnEncriptarPasswords" 
                            runat="server" Text="Encriptar Passwords" Width="150px" 
                            OnClientClick="return confirm('Va a encriptar las contraseñas que no estén encriptadas. ¿Desea continuar?')" 
                            ToolTip="Encriptar contraseñas." OnClick="OnBtnEncriptarPasswords_Click" 
                            meta:resourcekey="btnEncriptarPasswordsResource1" />
                    </td>
                </tr>  
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Button CssClass="botonFormulario" ID="btnDesencriptarPasswords" 
                            runat="server" Text="Desncriptar Passwords" Width="150px" 
                            OnClientClick="return confirm('Va a desencriptar las contraseñas que estén encriptadas. ¿Desea continuar?')" 
                            ToolTip="Desencriptar contraseñas." OnClick="OnBtnDesencriptarPasswords_Click" 
                            meta:resourcekey="btnDesencriptarPasswordsResource1" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Button CssClass="botonFormulario" ID="btnRefrescarConfiguracionColumnas" 
                            runat="server" Text="Refrescar Configuración Columnas" Width="150px" 
                            OnClick="OnBtnRefrescarConfiguracionColumnas_Click" 
                            meta:resourcekey="btnRefrescarConfiguracionColumnasResource1" />
                    </td>
                </tr>
            </table>      
       </asp:Panel>        
    </div>
    
    <div id="divBotonVolver">        
        <asp:Button CssClass="botonFormulario" ID="btnVolver" runat="server" 
            Text="Volver" OnClick="OnBtnVolver_Click" 
            meta:resourcekey="btnVolverResource1" />
    </div>

</asp:Content>
