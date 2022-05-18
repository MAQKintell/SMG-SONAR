<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" CodeFile="FrmModalActualizarNumeroFactura.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalActualizarNumeroFactura" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalActualizarNumeroFactura" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
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
                        Text=" " meta:resourcekey="lbladver1Resource1"></asp:Label>
                        <asp:Label ID="lblVisitasIncluidas" runat="server" 
                        CssClass="labelFormularioValor" meta:resourcekey="lblVisitasIncluidasResource1"></asp:Label>    
                    <asp:Label ID="lbladver11" runat="server" CssClass="labelFormulario" 
                        Text="Se utilizará el mismo número de factura, para todas las visitas" 
                        meta:resourcekey="lbladver11Resource1"></asp:Label>
                </td>
            </tr>
            <asp:HiddenField ID="hdnID" runat="server" />
            <asp:HiddenField ID="hdnFecha" runat="server" />
            <asp:HiddenField ID="hdnProveedor" runat="server" />
            <tr>
                <td class="labelTabla" style="max-width: 69px; padding-left:35px" rowspan="2" >
                <asp:Image ID="Image1" runat="server" ImageUrl="~/UI/HTML/Images/warning.png" 
            Width="24px" Height="26px" meta:resourcekey="Image1Resource1"/>
                </td>
                <td class="campoTabla">
                    &nbsp;
                    <asp:Label ID="lbladver2" runat="server" CssClass="labelFormulario" 
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
                        meta:resourcekey="lbladver3Resource1"></asp:Label>
                    <asp:Label ID="lblVisitasNoIncluidas" runat="server" 
                        CssClass="labelFormularioValor" 
                        meta:resourcekey="lblVisitasNoIncluidasResource1"></asp:Label>    
                    
                </td>
            </tr>
            <tr><td class="labelTabla" style="width: 89px">
                    <asp:Label CssClass="labelFormulario" ID="lblTextoLote" runat="server" 
                        Text="Número factura: " meta:resourcekey="lblTextoLoteResource1"></asp:Label>
                </td>
                <td class="campoTabla">
                      <asp:TextBox ID="txtNumFactura" runat="server" Maxlenght="255" Width="292px" 
                          TabIndex="1" meta:resourcekey="txtNumFacturaResource1"></asp:TextBox>
                </td></tr>
            
           
        </table>
    </div>
    <div id="divBotonGenerar" 
        
        
        
        style="position:absolute; width: 130px; top: 133px; left: 179px; height: 55px; margin-right: 1px;">
        <asp:Button ID="btnGenerar" runat="server" CssClass="botonFormulario" 
                    Text="Actualizar Num. Factura" Width="139px" runat="server" onclick="OnBtnGenerar_Click" 
            Height="21px"  OnClientClick="return ConfirmacionActualizar()" 
            TabIndex="2" meta:resourcekey="btnGenerarResource1" />
            
                    
    </div>
    <div id="divBotonCancelar"       
        
        
        style="position:absolute; width: 130px; top: 133px; left: 354px; height: 22px; margin-right: 1px;">
        <asp:Button ID="btnCancelar" runat="server" CssClass="botonFormulario" 
                    Text="Cancelar" Width="115px" Height="21px" runat="server" 
            OnClick="OnBtnCancelar_Click" TabIndex="3" 
            meta:resourcekey="btnCancelarResource1"/>
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
       <script type="text/javascript">
        function ConfirmacionActualizar()
        {
            var Respuesta= confirm("Va a actualizar los datos , ¿Desea Continuar?");
//            if (Respuesta ==false){ OcultarCapaEspera(); }
//            else{MostrarCapaEspera();}
            return Respuesta;
        }

  </script>
</asp:Content>