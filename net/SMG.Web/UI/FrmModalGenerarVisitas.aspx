<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" CodeFile="FrmModalGenerarVisitas.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalGenerarVisitas" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalGenerarVisitas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <div id="divInformacionLote" 
        
        style="position:absolute; width: 437px; top: 42px; left: 108px; height: 126px; margin-right: 1px;">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td class="labelTabla" style="width: 69px">
                    <asp:Label ID="lblAdvertencias" runat="server" CssClass="labelFormulario" 
                        Text="Advertencias:" ForeColor="Red" 
                        meta:resourcekey="lblAdvertenciasResource1"></asp:Label>
                </td>
                <td class="campoTabla">
                    &nbsp;
                    <asp:Label ID="lbladver1" runat="server" CssClass="labelFormulario" 
                        Text="El lote contendrá " meta:resourcekey="lbladver1Resource1"></asp:Label>
                        <asp:Label ID="lblVisitasIncluidas" runat="server" 
                        CssClass="labelFormularioValor" meta:resourcekey="lblVisitasIncluidasResource1"></asp:Label>    
                    <asp:Label ID="lbladver11" runat="server" CssClass="labelFormulario" 
                        Text=" visitas." meta:resourcekey="lbladver11Resource1"></asp:Label>
                </td>
            </tr>
           
            <tr>
                <td class="labelTabla" style="max-width: 69px; padding-left:35px" rowspan="2" >
                <asp:Image ID="Image1" runat="server" ImageUrl="~/UI/HTML/Images/warning.png" 
            Width="24px" Height="26px" meta:resourcekey="Image1Resource1"/>
                </td>
                <td class="campoTabla">
                    &nbsp;
                    <asp:Label ID="lbladver2" runat="server" CssClass="labelFormulario" 
                        Text="Visitas canceladas incluidas en el lote:" 
                        meta:resourcekey="lbladver2Resource1"></asp:Label>
                    <asp:Label ID="lblVisitasCanceladas" runat="server" 
                        CssClass="labelFormularioValor" 
                        meta:resourcekey="lblVisitasCanceladasResource1"></asp:Label>    
                    
                </td>
            </tr>
           
            <tr>
             
                <td class="campoTabla">
                    &nbsp;
                    <asp:Label ID="lbladver3" runat="server" CssClass="labelFormulario" 
                        Text="Visitas cerradas que no seran incluidas en el lote:" 
                        meta:resourcekey="lbladver3Resource1"></asp:Label>
                    <asp:Label ID="lblVisitasNoIncluidas" runat="server" 
                        CssClass="labelFormularioValor" 
                        meta:resourcekey="lblVisitasNoIncluidasResource1"></asp:Label>    
                    
                </td>
            </tr>
            <tr><td class="labelTabla" style="width: 69px">
                    <asp:Label CssClass="labelFormulario" ID="lblTextoLote" runat="server" 
                        Text="Texto Lote: " meta:resourcekey="lblTextoLoteResource1"></asp:Label>
                </td>
                <td class="campoTabla">
                      <asp:TextBox ID="txtTextoLote" runat="server" Maxlenght="255" Width="292px" 
                          TabIndex="1" meta:resourcekey="txtTextoLoteResource1"></asp:TextBox>
                </td></tr>
            
           
        </table>
    </div>
    <div id="divBotonGenerar" 
        
        
        
        style="position:absolute; width: 130px; top: 133px; left: 179px; height: 55px; margin-right: 1px;">
        <asp:Button ID="btnGenerar" runat="server" CssClass="botonFormulario" 
                    Text="Generar Lote" Width="115px" runat="server" onclick="OnBtnGenerar_Click" 
            Height="21px"  OnClientClick="return CombrobarTexto()" TabIndex="2" 
            meta:resourcekey="btnGenerarResource1" />
            
                    
    </div>
    <div id="divBotonCancelar"       
        
        
        style="position:absolute; width: 130px; top: 133px; left: 354px; height: 22px; margin-right: 1px;">
        <asp:Button ID="btnCancelar" runat="server" CssClass="botonFormulario" 
            Text="Cancelar" Width="115px" Height="21px" runat="server" 
            OnClick="OnBtnCancelar_Click" TabIndex="3" 
            meta:resourcekey="btnCancelarResource1"/>
    </div>
  <div id="divMensajeFinal"       
        
        
        style="position:absolute; width: 514px; top: 180px; left: 115px; height: 2px; margin-right: 1px;" 
        align="center">
        <asp:Label CssClass="labelFormulario" ID="lbMensajeFinal" runat="server" 
            Visible="False" meta:resourcekey="lbMensajeFinalResource1"></asp:Label>
        <asp:Label CssClass="labelFormularioValor" ID="lbIdLote" runat="server" 
            Visible="False" meta:resourcekey="lbIdLoteResource1"></asp:Label>
    </div>
   <div id="divBotonAceptar"       
        style="position:absolute; width: 113px; top: 217px; left: 179px; height: 22px; margin-right: 1px;">
        <asp:Button ID="btnAceptar" runat="server" CssClass="botonFormulario" 
                    Text="Aceptar" Width="115px" Visible="False" runat="server" 
            OnClick="OnBtnAceptar_Click" TabIndex="4" 
            meta:resourcekey="btnAceptarResource1"/>
    </div>
   <script type="text/javascript">
    function CombrobarTexto()
    {
        var objText = document.getElementById("ctl00$ContentPlaceHolderContenido$txtTextoLote")
        if (objText && objText.value.length == 0)
        {
            return confirm("No se ha introducido la descripción del lote. ¿Desea continuar?");
        }
        return true;
    }
   </script>
</asp:Content>