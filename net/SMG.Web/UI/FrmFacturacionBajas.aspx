<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmFacturacionBajas.aspx.cs" Inherits="UI_FrmNoFacturarBaja" MasterPageFile="~/UI/MasterPageAdmin.master"%>
<%@ Import Namespace="Iberdrola.Commons.Web" %>

<asp:Content ID="FrmContratoBaja" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <script src="HTML/Js/jquery.js" type="text/javascript"></script>

    <div id="divTituloVentana2">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana2" runat="server" 
            Text="Modificar Carencia Contrato"></asp:Label>
    </div>

    <div id="divCarenciaContrato">
        <table>
            <tr>
                <td><asp:Label ID="lblContratoCarencia" runat="server" Text="Contrato:" CssClass="labelFormulario"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtContratoCarencia" CssClass="campoFormulario" width="80px" runat="server" MaxLength="10" />
                    <asp:CheckBox ID="chk_Carencia" runat="server" class="campoFormulario"  />
                </td>
                <td><asp:Button ID="btnModificarCarencia" CssClass="botonFormulario" runat="server" Text="Modificar" Width="100px" OnClick="btnModificarCarencia_Click" /></td>
            </tr>
        </table>
    </div>

     <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Contratos a No Facturar la baja"></asp:Label>
    </div>

    <div id="divContratoBaja">
        <table>
            <tr>
                <td><asp:Label ID="lblContrato" runat="server" Text="Contrato:" CssClass="labelFormulario"></asp:Label></td>
                <td><asp:TextBox ID="txtContrato" CssClass="campoFormulario" width="80px" runat="server" MaxLength="10" /></td>
                <td><asp:Button ID="btnProcesar" CssClass="botonFormulario" runat="server" Text="Procesar" Width="100px" OnClick="btnProcesar_Click" /></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblFichero" runat="server" Text="Subir fichero:" CssClass="labelFormulario"></asp:Label></td>
                <td><asp:FileUpload class="file_1" ID="fuFicheroBajas" runat="server" Width="199px" BackColor="White" CssClass="file_1" ForeColor="Black" /></td>
                <td><asp:Button ID="btnProcesarFichero" CssClass="botonFormulario" runat="server" Text="Procesar" Width="100px" OnClick="btnProcesarFichero_Click" /></td>
            </tr>            
            <tr>
                <td><asp:Label ID="lblResultado" runat="server" Text="Resultado:" CssClass="labelFormulario"></asp:Label></td>
                <td colspan="2"> 
                    <!--<div id="divGridSolicitudes" style="overflow: scroll; top: 5px; left: 5px; Height:100px; width: 300px" class="panelTabla">-->
                        <asp:GridView ID="gvProceso" runat="server" CssClass="contenidoTabla" Width="300px" Height="100px" EnableModelValidation="True"> 
                            <RowStyle CssClass="filaNormal"/>
                            <AlternatingRowStyle CssClass="filaAlterna"/>
                            <FooterStyle CssClass="cabeceraTabla"  />
                            <PagerStyle  CssClass="cabeceraTabla" />
                            <HeaderStyle CssClass="cabeceraTabla" />
                        </asp:GridView>
                    <!--</div>-->
                </td>
            </tr>
        </table>        
    </div>
        
</asp:Content>
