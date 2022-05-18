﻿<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmCambioContrasenia.aspx.cs" Inherits="Iberdrola.SMG.UI.frmCambioContrasenia" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Import Namespace="Iberdrola.Commons.Web" %>



<asp:Content ID="FrmContratos" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
        
   
       <script language ="javascript">
        function abrirVentana() 
        {
            window.location.href = "../MenuAcceso.aspx";
        }
       </script>
        
        <script type="text/javascript">
        <% 
            if(NavigationController.IsBackward())
            { 
        %>
                MostrarCapaEspera();
        <%
            }
        %>
        </script>
    
<div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Cambio de Contraseña" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
    
    <asp:PlaceHolder ID="placeHolderAlerta" Visible="False" runat="server">
    <div id="divImagenAdvertencia" style="position: absolute; top: 40px; left: 5px;">
        <img src="HTML/Images/warning.png" width="40px" height="40px" />
    </div>
    <div id="divMensajeAdvertencia" style="position: absolute; top: 52px; left: 45px;" class="contenidoTabla">
        <%=Resources.TextosJavaScript.TEXTO_CONTRASENHIA_POCO_SEGURA%>
    </div>
    </asp:PlaceHolder>
    
    <asp:PlaceHolder ID="placeHolderCaducidad" Visible="False" runat="server">
    <div id="divMensajeCaducado" style="position: absolute; top: 52px; left: 45px;" class="contenidoTabla">
        <%=Resources.TextosJavaScript.TEXZTO_CONTRASENHIA_CADUCADA%>
    </div>
    </asp:PlaceHolder>
    
    <div id="div1" style="position: absolute; top: 100px; left: 45px;" class="contenidoTabla">
        <%=Resources.TextosJavaScript.TEXTO_CONTRASENHIA_DEBE_CUMPLIR%>
        <ul>
            <li><%=Resources.TextosJavaScript.TEXTO_CONTRASENHIA_ENTRE_6Y8%></li>
            <li><%=Resources.TextosJavaScript.TEXTO_CONTRASENHIA_INCLUIR_MAYUS_MINUS%></li>
            <li><%=Resources.TextosJavaScript.TEXTO_CONTRASENHIA_MINIMO_2_CARACTERES_NUMERICOS%></li>
            <li><%=Resources.TextosJavaScript.TEXTO_CONTRASENHIA_CARACTERES_ALFANUMERICOS%></li>
        </ul>
    </div>
    
    
    <div id="divFiltroBusqueda" style="position: absolute; top: 180px; left: 325px;">
     <asp:Panel ID="pnBusqueda" runat="server" 
            Height="137px" width="562px" CssClass="labelFormulario" 
            meta:resourcekey="pnBusquedaResource1">

            <table>
            <tr>
            <td colspan="2">&nbsp;</tr>
            <tr>
                <td colspan="2">
                    &nbsp;</td>
                <tr>
                    <td>
                        <asp:Label ID="lblContratoBuscar" runat="server" CssClass="campoFormulario" 
                            Text="Nueva Contraseña:" meta:resourcekey="lblContratoBuscarResource1"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtContrasenia" runat="server" CssClass="campoFormulario" 
                            MaxLength="10" TextMode="Password" Width="60px" 
                            meta:resourcekey="txtContraseniaResource1"></asp:TextBox>
                        
                        <asp:CustomValidator ID="txtContraseniaCustomValidator" runat="server" 
                            ControlToValidate="txtContrasenia" CssClass="errorFormulario" ErrorMessage="*" 
                            OnServerValidate="OnTxtContrasenia_ServerValidate" 
                            ValidateEmptyText="True" 
                            meta:resourcekey="txtContraseniaCustomValidatorResource1"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="campoFormulario" 
                            Text="Repetir Contraseña: " meta:resourcekey="Label1Resource1"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRepetir" runat="server" CssClass="campoFormulario" 
                            MaxLength="10" TextMode="Password" Width="60px" 
                            meta:resourcekey="txtRepetirResource1"></asp:TextBox>
                        <asp:CustomValidator ID="txtRepetirCustomValidator" runat="server" 
                            ControlToValidate="txtRepetir" CssClass="errorFormulario" ErrorMessage="*" 
                            OnServerValidate="OnTxtRepetir_ServerValidate" ValidateEmptyText="True" 
                            meta:resourcekey="txtRepetirCustomValidatorResource1"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnCambiar" runat="server" CssClass="botonFormulario" 
                            OnClick="OnBtnCambiar_Click" OnClientClick="MostrarCapaEspera();" TabIndex="1" 
                            Text="Cambiar Contraseña" meta:resourcekey="btnCambiarResource1" />
                    </td>
                </tr>
            </table>


                   
    </div>
        
     </asp:Panel>
     <div id="divInformacionRegistro" 
        style="position: absolute; top: 500px; left: 206px; width: 554px;">
             <asp:Label ID="lblSolicitudModificada" CssClass="labelFormulario" runat="server" 
                        Text="La contraseña ha sido modificada correctamente." 
                 ForeColor="#0033CC" Visible ="False" Font-Bold="True" Font-Size="Medium" 
                 meta:resourcekey="lblSolicitudModificadaResource1"></asp:Label>
    </div>
</asp:Content>
