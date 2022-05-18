<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true"
    CodeFile="FrmModalSeleccionarContrato.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalSeleccionarContrato" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalHistoricoVisita" ContentPlaceHolderID="ContentPlaceHolderContenido"
    runat="server">

    <asp:ScriptManager ID="SM" runat="server">
    </asp:ScriptManager>
    <div id="div1" class="divSubTituloVentana" style="visibility:visible;width: 780px; position:absolute; top: 0px; left: 5px; height: 20px;">
        Contratos encontrados en la búsqueda
    </div>
    
     <div id="divContratos" style="visibility:visible;width: 780px; position:absolute; top: 15px; left: 0px; height: 399px;">
        <div style="position:absolute;overflow:scroll;top:5px;left:5px;height:390px;width:780px;border:solid 1px #000000" class="panelTabla">
                <asp:GridView ID="gridContratos" runat="server" Width="764px"  
                    CssClass="contenidoTabla" EnableModelValidation="True" 
                    meta:resourcekey="gridContratosResource1" >
                    <RowStyle CssClass="filaNormal" BorderColor ="Transparent" />
                    <AlternatingRowStyle CssClass="filaAlterna"/>
                    <FooterStyle CssClass="cabeceraTabla" />
                    <PagerStyle  CssClass="cabeceraTabla" />
                    <HeaderStyle CssClass="cabeceraTabla" BorderColor ="Transparent" />     
                </asp:GridView>
         </div>
         
      <div id="divBotonesEditar" style="position:absolute; top: 425px; left: 275px;">
        <asp:Button ID="btnAceptar" runat="server"  Text="Aceptar" 
              CssClass="botonFormulario" meta:resourcekey="btnAceptarResource1" />
        <asp:Button ID="btnCancelar" runat="server"  Text="Cancelar" 
              CssClass="botonFormulario" meta:resourcekey="btnCancelarResource1" />
      </div>
</asp:Content>
