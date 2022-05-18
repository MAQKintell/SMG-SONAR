<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGasSinMenu.master" AutoEventWireup="true" CodeFile="FrmListadoProveedores.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmListadoProveedores" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="Iberdrola.Commons.Web" %>

<asp:Content ID="FrmListadoProveedores" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> 
    <LINK href="../css/ventan-modal-ie.css" type="text/css" rel="stylesheet">
	<script src="../js/ventana-modal.js" type="text/javascript"></script>
    
    <script type="text/javascript">
    function ExportarExcel() {
        //alert('qqq');
        //alert('II');
        var lblRegistros = document.getElementById("ctl00_ContentPlaceHolderContenido_lblNumEncontrados");
        if(lblRegistros.innerText<=0)
        {
            alert("No se puede exportar a excel porque no se han encontrado registros.");
        }
        else
        {
            abrirVentanaLocalizacion("../UI/AbrirExcel.aspx", 600, 350, "ventana-modal","EXPORTAR EXCEL","0","1");
        }
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
            Text="Listado de Técnicos" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
    <asp:ScriptManager ID="SM" runat="server"></asp:ScriptManager>
    
    
        <asp:UpdatePanel ID="Resultados" runat="server">
        <ContentTemplate>
            <div id="divScroll" 
                style="position:absolute;overflow:scroll;width: 961px;height: 463px; top: 38px; left: 2px; border: 1px solid #000000; z-index:9995" 
                runat="server">
                <asp:GridView ID="grdListado" runat="server" 
                        CssClass="contenidoTabla" 
                        style="z-index:99999;position:relative; top: -1px; left: 1px; height: 6px; border:1px solid #000000;" 
                         OnRowDataBound="OnGrdListado_RowDataBound" 
                    EnableModelValidation="True" meta:resourcekey="grdListadoResource1" >
                         
                         <HeaderStyle HorizontalAlign="Center" CssClass="cabeceraTabla"/>
                         <RowStyle CssClass="filaNormal" />
                         <AlternatingRowStyle CssClass="filaAlterna" />
                         <SelectedRowStyle CssClass="filaSeleccionada" />
                         <FooterStyle CssClass="cabeceraTabla" />
                         <PagerStyle  CssClass="cabeceraTabla" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="div3" style="position: absolute; top: 8px; left: 310px; width: 464px;">
            <asp:PlaceHolder ID="PlaceHolder5" runat="server">
            <table ID="Table1" runat="server">
            <tr>
                <td><asp:Label ID="Label1" CssClass="labelFormulario" visible="true" runat="server" Text="Nombre: "></asp:Label></td>
                <td><asp:TextBox ID="txtNombre" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="Label2" CssClass="labelFormulario" visible="true" runat="server" Text="Apellido: "></asp:Label></td>
                <td><asp:TextBox ID="txtApellido" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="Label6" CssClass="labelFormulario" visible="false" runat="server" Text="Proveedor: "></asp:Label></td>
                <td><asp:DropDownList Visible="false" ID="cmbProveedor" runat="server"></asp:DropDownList></td>
            </tr>
            </table>
            </asp:PlaceHolder>
        </div>
     <div id="divBotonRealizarConsulta" 
        style="position: absolute; top: 10px; left: 790px;">
        <asp:Button CssClass="botonFormulario" ID="btnRealizarConsulta" runat="server" Text="Buscar"
            TabIndex="1"  OnClientClick="MostrarCapaEspera();" 
            onclick="btnRealizarConsulta_Click" 
             meta:resourcekey="btnRealizarConsultaResource1"/>
    </div>
    <div id="divContador" 
        style="position:absolute; top: 514px; left: 3px; height: 51px; width: 329px" 
        align="center">
        <table width="100%" align="left">
            <tr>
                <td align="left" colspan ="2">
                    <asp:PlaceHolder ID="placeHolderPaginacion" runat="server">
                    &nbsp;  
                    </asp:PlaceHolder>
                </td>
            </tr>
             <tr>
                <td align="left">
                    <asp:Label CssClass="labelFormularioValor" ID="lblNumEncontrados" 
                        runat="server" Text="0" meta:resourcekey="lblNumEncontradosResource1"></asp:Label>
                    <asp:Label CssClass="labelFormulario" ID="lblContador" runat="server" 
                        Text="regs. encontrados." meta:resourcekey="lblContadorResource1"></asp:Label>
                    
                </td>
                <td align="left" style="width:40px">        
                </td>

            </tr>
        </table> 
        <asp:HiddenField ID="hdnSortDirection" value="ASC" runat="server" />
        <asp:HiddenField ID="hdnSortColumn" runat="server" />
        <asp:HiddenField ID="hdnPageCount" runat="server" />
        <asp:HiddenField ID="hdnPageIndex" runat="server" />
        <asp:HiddenField ID="hdnRowIndex" runat="server" />
        <asp:HiddenField ID="hdnClickedCellValue" runat="server" />
        <asp:HiddenField ID="hdnRecargarUltimaConsulta" runat="server" />
        <asp:Button ID="btnPaginacion" runat="server"  OnClick="OnBtnPaginacion_Click" 
            Width="0px" meta:resourcekey="btnPaginacionResource1"/> 
    </div>
    <div id="divBotonExcel" 
        style="position:absolute; top: 534px; left: 369px; height: 23px; width: 258px;">
         <input type="button" id="btnInputExcel" name="btnInputExcel" class="botonFormulario" value="Exportar a Excel" onclick="javascript:ExportarExcel();return false;" tabindex="5" runat="server" />
    </div>
         
     <script type="text/javascript">
        function ClickPaginacion(ident)
        {
            MostrarCapaEspera();
            var botonPaginacion = document.getElementById("ctl00$ContentPlaceHolderContenido$btnPaginacion");
            var hiddenPaginacion = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnPageIndex");
            hiddenPaginacion.value = ident;
            botonPaginacion.click();
            return false;
        }
        function btnInputExcel_onclick() {

        }
    </script>
</asp:Content>