<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmMostrarError.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmMostrarError" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmPrincipal" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <div id="divTituloVentana" style="position:absolute; top: -77px; left: -15px;">
        <asp:Image ImageUrl="HTML/Images/Error.gif" runat="server" 
            meta:resourcekey="ImageResource1" /> 
    </div>
    <asp:Label ID="lblTextoMensajeError" CssClass="labelFormulario" runat="server" 
        Text="Label" meta:resourcekey="lblTextoMensajeErrorResource1"></asp:Label>
    <div id="divBotonVolver" style="z-index:99">
        <asp:Button CssClass="botonFormulario" ID="btnVolver" runat="server" 
            Text="Volver" OnClick="OnBtnVolver_Click" 
            meta:resourcekey="btnVolverResource1" />
    </div>
    
    <%--<div id="div1" 
        style="position:absolute; top: 124px; left: 778px; width: 118px;">
        <link href="../Login.aspx" /> VOLVER
    </div>--%>
    
</asp:Content>