<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/UI/MasterPageMntoGasSinMenuFacturacionAverias.master" CodeFile="FrmFacturacionAverias.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmFacturacionAverias" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="Iberdrola.Commons.Web" %>


<asp:Content ID="FrmFacturacionVisitas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> 
    <LINK href="../css/ventan-modal-ie.css" type="text/css" rel="stylesheet">
	<script src="../js/ventana-modal.js" type="text/javascript"></script>
        <script type="text/javascript">

        
        function ExportarExcel() {
            //Alert('qqq');
            //Alert('II');
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
        
        function Consultas()
        {
            var rd=document.getElementById("ctl00_ContentPlaceHolderContenido_rdbBusquedas_0");
            rd.checked=false;
            var rd1=document.getElementById("ctl00_ContentPlaceHolderContenido_rdbBusquedas_1");
            rd1.checked=false;
            document.getElementById("ctl00$ContentPlaceHolderContenido$btnRealizarConsulta").removeAttribute('disabled');
            document.getElementById("ctl00$ContentPlaceHolderContenido$btnRealizarBusqueda").disabled='true';
            
            document.getElementById("ctl00$ContentPlaceHolderContenido$btnPagar").removeAttribute('disabled');
            //Alert('1');
        }
        function Busquedas()
        {
            var rd=document.getElementById("ctl00_ContentPlaceHolderContenido_RadioConsultas_0");
            rd.checked=false;
            var rd1=document.getElementById("ctl00_ContentPlaceHolderContenido_RadioConsultas_1");
            rd1.checked=false;
            document.getElementById("ctl00$ContentPlaceHolderContenido$btnRealizarConsulta").disabled='true';
            document.getElementById("ctl00$ContentPlaceHolderContenido$btnRealizarBusqueda").removeAttribute('disabled');
            
            document.getElementById("ctl00$ContentPlaceHolderContenido$btnPagar").disabled='true';
            //Alert('2');
        }
        </script>
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Facturación Averias" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
    <asp:ScriptManager ID="SM" runat="server"></asp:ScriptManager>

 
    <asp:UpdatePanel ID="Resultados" runat="server">
        <ContentTemplate>
            <div id="divScroll" 
                style="position:absolute;overflow:scroll;width: 961px;height: 339px; top: 168px; left: 6px; border: 1px solid #000000; z-index:9995" 
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
    
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div id="divFiltros" style="width: 954px; position: absolute; top: 30px; left: 6px;height: 242px;">
                <asp:Panel ID="Panel2" CssClass="labelFormulario" runat="server" 
                    GroupingText="Filtros de la información" Width="964px" Height="115px" 
                    meta:resourcekey="Panel2Resource1">
                    <asp:RadioButtonList ID="RadioConsultas" runat="server" AutoPostBack="True" 
                        CssClass="campoFormulario" Height="32px" 
                        onselectedindexchanged="RadioConsultas_SelectedIndexChanged" 
                        meta:resourcekey="RadioConsultasResource1" >
                        <asp:ListItem Selected="True" Value="1" meta:resourcekey="ListItemResource1">Averias Bonificables</asp:ListItem>
                        <asp:ListItem Value="2" meta:resourcekey="ListItemResource2">Averias NO Bonificables</asp:ListItem>
                    </asp:RadioButtonList>
                </asp:Panel>
            </div>
            <div style="position:absolute; top: 97px; left: 5px; width: 760px;">
                <asp:Panel ID="Panel1" CssClass="labelFormulario" runat="server" 
                    GroupingText="BUSQUEDAS" Width="964px" Height="115px" 
                    meta:resourcekey="Panel1Resource1">
                    <asp:RadioButtonList ID="rdbBusquedas" runat="server" AutoPostBack="True" 
                        CssClass="campoFormulario" Height="32px" 
                        onselectedindexchanged="rdbBusquedas_SelectedIndexChanged" 
                        meta:resourcekey="rdbBusquedasResource1" >
                        <asp:ListItem Value="3" meta:resourcekey="ListItemResource3">Averias Bonificables</asp:ListItem>
                        <asp:ListItem Value="4" meta:resourcekey="ListItemResource4">Averias NO Bonificables</asp:ListItem>
                    </asp:RadioButtonList>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        <br />
        <br />
        <br />
        
        <div id="div3" style="position: absolute; top: 43px; left: 817px; width: 138px;">
            <asp:PlaceHolder ID="PlaceHolder5" runat="server">
                <asp:Label ID="Label6" CssClass="labelFormulario" visible="true" runat="server" 
                    Text="Proveedor: " meta:resourcekey="Label6Resource1"></asp:Label>
                <asp:DropDownList ID="cmbProveedor" runat="server" 
                    meta:resourcekey="cmbProveedorResource1">
                </asp:DropDownList>
            </asp:PlaceHolder>
        </div>
        
        <div id="div4" 
        style="position: absolute; top: 109px; left: 817px; width: 138px;">
            <asp:PlaceHolder ID="PlaceHolder3" runat="server">
                <asp:Label ID="Label3" CssClass="labelFormulario" visible="true" runat="server" 
                    Text="Proveedor: " meta:resourcekey="Label3Resource1"></asp:Label>
                <asp:DropDownList ID="cmbProveedorBusqueda" runat="server" 
                    meta:resourcekey="cmbProveedorBusquedaResource1">
                </asp:DropDownList>
            </asp:PlaceHolder>
        </div>
        
        <div id="div1" style="position: absolute; top: 44px; left: 342px; width: 206px;">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                <asp:Label ID="Label1" CssClass="labelFormulario" visible="true" runat="server" 
                    Text="Núm. Factura: " Font-Bold="True" meta:resourcekey="Label1Resource1"></asp:Label>
                <asp:TextBox ID="txtNumFactura" runat="server" 
                    meta:resourcekey="txtNumFacturaResource1"></asp:TextBox>
            </asp:PlaceHolder>
        </div>
        
        <div id="div2" 
        style="position: absolute; top: 112px; left: 342px; width: 206px;">
            <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                <asp:Label ID="Label2" CssClass="labelFormulario" visible="true" runat="server" 
                    Text="Núm. Factura: " Font-Bold="True" meta:resourcekey="Label2Resource1"></asp:Label>
                <asp:TextBox ID="txtNumFacturaBusqueda" runat="server" 
                    meta:resourcekey="txtNumFacturaBusquedaResource1"></asp:TextBox>
            </asp:PlaceHolder>
        </div>
        
    
 
  <div id="divBotonRealizarConsulta" 
        style="position: absolute; top: 71px; left: 836px;">
        <asp:Button CssClass="botonFormulario" ID="btnRealizarConsulta" runat="server" Text="Buscar"
            TabIndex="1"  OnClientClick="MostrarCapaEspera();" 
            onclick="btnRealizarConsulta_Click" 
            meta:resourcekey="btnRealizarConsultaResource1"/>
    </div>
    <div id="divBotonRealizarBusqueda" 
        style="position: absolute; top: 136px; left: 836px;">
        <asp:Button CssClass="botonFormulario" ID="btnRealizarBusqueda" runat="server" Text="Buscar"
            TabIndex="1"  OnClientClick="MostrarCapaEspera();" Enabled="False"
            onclick="btnRealizarBusqueda_Click" 
            meta:resourcekey="btnRealizarBusquedaResource1" />
    </div>
    <div id="divBotonVolver">
        <asp:Button CssClass="botonFormulario" ID="btnVolver" runat="server" Text="Volver"
            TabIndex="2" OnClick="OnBtnVolver_Click" 
            meta:resourcekey="btnVolverResource1"/>
    </div>
    
        <div id="divContador" 
        style="position:absolute; top: 515px; left: 10px; height: 51px; width: 329px" 
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
        style="position:absolute; top: 545px; left: 369px; height: 23px; width: 258px;">
         <input type="button" id="btnInputExcel" name="btnInputExcel" class="botonFormulario" value="Exportar a Excel" onclick="javascript:ExportarExcel();return false;" tabindex="5" runat="server" />
        &nbsp;&nbsp;&nbsp;<asp:Button ID="btnPagar" runat="server" 
             class="botonFormulario" Text="Pago Averias" 
             OnClientClick="return ConfirmarPago()" OnClick="OnBtnPagar_Click" 
             meta:resourcekey="btnPagarResource1" />
    </div>
    </ContentTemplate>
            
        </asp:UpdatePanel>
            
            
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    <script type="text/javascript">
        function ConfirmarPago()
        {
            var Respuesta= confirm("Va a realizar el pago, ¿Desea Continuar?");
//            if (Respuesta ==false){ OcultarCapaEspera(); }
//            else{MostrarCapaEspera();}
            return Respuesta;
        }
        
        function ConfirmacionActualizar()
        {
            var Respuesta= confirm("Va a actualizar la fecha a día de hoy para penalizar las visitas, ¿Desea Continuar?");
//            if (Respuesta ==false){ OcultarCapaEspera(); }
//            else{MostrarCapaEspera();}
            return Respuesta;
        }
        function ConfirmacionPenalizar()
        {
            var Respuesta= confirm("Va a penalizar visitas, ¿Desea Continuar?");
//            if (Respuesta ==false){ OcultarCapaEspera(); }
//            else{MostrarCapaEspera();}
            return Respuesta;
        }
        function ConfirmacionFacturar()
        {
            
            var Respuesta= confirm("Va a facturar las visitas, ¿Desea Continuar?");
//            if (Respuesta ==false){ OcultarCapaEspera(); }
//            else{MostrarCapaEspera();}
            return Respuesta;
        }

        function GridViewRowClick(ident)
        {
            var btnRowClick = document.getElementById("ctl00$ContentPlaceHolderContenido$btnRowClick");
            var hdnClickedCellValue = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnClickedCellValue");
            hdnClickedCellValue.value = ident;
            btnRowClick.click();
            return false;
        }
         
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
