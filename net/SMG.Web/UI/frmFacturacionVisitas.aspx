<%@ Page Language="C#"  AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/UI/MasterPageMntoGas.master" CodeFile="frmFacturacionVisitas.aspx.cs" Inherits="Iberdrola.SMG.UI.frmFacturacionVisitas" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="Iberdrola.Commons.Web" %>

<asp:Content ID="FrmFacturacionVisitas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
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
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Facturación Visitas" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
    <asp:ScriptManager ID="SM" runat="server"></asp:ScriptManager>
 
 
 
 
 
 
 
 
    <asp:UpdatePanel ID="Resultados" runat="server">
        <ContentTemplate>
            <div id="divScroll" style="position:absolute;overflow:scroll;width: 961px;height: 363px; top: 149px; left: 6px; border: 1px solid #000000; z-index:9995" runat="server">
                <asp:GridView ID="grdListado" runat="server" AutoGenerateColumns="False" 
                        CssClass="contenidoTabla" 
                        style="z-index:99999;position:relative; top: -1px; left: 0px; height: 6px; border:1px solid #000000;" 
                         OnRowDataBound="OnGrdListado_RowDataBound" 
                    onsorting="grdListado_Sorting" EnableModelValidation="True" 
                    meta:resourcekey="grdListadoResource1">
                         
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
                    GroupingText="Filtros de la información" Width="964px" 
                    meta:resourcekey="Panel2Resource1">
                    <asp:RadioButtonList ID="RadioConsultas" runat="server" AutoPostBack="True" 
                        CssClass="campoFormulario" Height="32px" 
                        onselectedindexchanged="RadioConsultas_SelectedIndexChanged" 
                        meta:resourcekey="RadioConsultasResource1">
                        <asp:ListItem Selected="True" Value="1" meta:resourcekey="ListItemResource1">Visitas a Facturar</asp:ListItem>
                        <asp:ListItem Value="2" meta:resourcekey="ListItemResource2">Reparaciones a Facturar</asp:ListItem>
                        <asp:ListItem Value="3" meta:resourcekey="ListItemResource3">Visitas Vencidas</asp:ListItem>
                        <asp:ListItem Value="4" meta:resourcekey="ListItemResource4">Visitas Penalizables</asp:ListItem>
                    </asp:RadioButtonList>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        <br />
        <br />
        <br />
        
               
        
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                    
                    
                    
            
        
        
                 <div id="divBotonPenalizar" 
                     
                     
                     
                     
                    style="position:absolute; top: 545px; left: 512px; height: 23px; width: 134px;">
                    <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible="False">
                        <asp:Button ID="btnPenalizar" runat="server" CssClass="botonFormulario" 
                                Text="Penalizar Visitas" onclick="btnPenalizar_Click" 
                            OnClientClick="return ConfirmacionPenalizar()" 
                            meta:resourcekey="btnPenalizarResource1" />
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PlaceHolder4" runat="server">
                         <asp:Button Width="130px" ID="btnActualizar" runat="server" CssClass="botonFormulario" 
                                Text="Cambiar Num. Factura" onclick="btnActualizar_Click" 
                             meta:resourcekey="btnActualizarResource1" />
                    </asp:PlaceHolder>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        
        
        
        
        
         <div ID="div3" style="position: absolute; top: 43px; left: 817px; width: 138px;">
            <asp:PlaceHolder ID="PlaceHolder5" runat="server">
                <asp:Label ID="Label6" CssClass="labelFormulario" visible="true" runat="server" 
                    Text="Proveedor: " meta:resourcekey="Label6Resource1"></asp:Label>
                <asp:DropDownList ID="cmbProveedor" runat="server" 
                    meta:resourcekey="cmbProveedorResource1">
                </asp:DropDownList>
            </asp:PlaceHolder>
        </div>
        
        
        
        
        
         <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="div2" style="position: absolute; top: 70px; left: 424px">
                    <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="False">
                        <asp:Label ID="Label3" CssClass="labelFormulario" runat="server" Text="Fecha: " 
                            meta:resourcekey="Label3Resource1"></asp:Label>
                        <asp:TextBox ID="txtFecha0" CssClass="campoFormulario" Width="60px" runat="server"
                            MaxLength="10" meta:resourcekey="txtFecha0Resource1"></asp:TextBox>
                            <img id="img3" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" 
                            onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtFecha0');" >
                        
                        
                        <asp:CustomValidator CssClass="errorFormulario" ID="txtFechaHastaOp2Validator" 
                            runat="server" ErrorMessage="*" 
                            OnServerValidate="TxtFechaHastaOp2CustomValidate" 
                            meta:resourcekey="txtFechaHastaOp2ValidatorResource1"></asp:CustomValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        
                        <asp:Label ID="Label4" visible="False" runat="server" Text="Num. Factura: " 
                            meta:resourcekey="Label4Resource1"></asp:Label>
                        <asp:TextBox ID="txtNumFactura0" runat="server" CssClass="campoFormulario" 
                            MaxLength="10" Width="60px" visible="False" 
                            meta:resourcekey="txtNumFactura0Resource1"></asp:TextBox>
                    </asp:PlaceHolder>
                </div>
                <div ID="div1" style="position: absolute; top: 43px; left: 425px">
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                        <asp:Label CssClass="labelFormulario" ID="Label1" runat="server" Text="Fecha: " 
                            meta:resourcekey="Label1Resource1"></asp:Label>
                        <asp:TextBox ID="txtFecha" runat="server" CssClass="campoFormulario" 
                            MaxLength="10" Width="60px" meta:resourcekey="txtFechaResource1"></asp:TextBox>
                        <img ID="img2" alt="Calendario" 
                            onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtFecha');" 
                            src="HTML/IMAGES/imagenCalendario.gif">
                        
                        <asp:CustomValidator CssClass="errorFormulario" ID="txtFechaHastaOp1Validator" 
                            runat="server" ErrorMessage="*" 
                            OnServerValidate="TxtFechaHastaOp1CustomValidate" 
                            meta:resourcekey="txtFechaHastaOp1ValidatorResource1"></asp:CustomValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        
                        <asp:Label ID="Label2" visible="False" runat="server" Text="Num. Factura: " 
                            meta:resourcekey="Label2Resource1"></asp:Label>
                        <asp:TextBox ID="txtNumFactura" visible="False" runat="server" CssClass="campoFormulario" 
                            MaxLength="10" Width="60px" meta:resourcekey="txtNumFacturaResource1"></asp:TextBox>
                    </asp:PlaceHolder>
                </div>
            
        
    <div id="divBotonRealizarConsulta" 
        style="position: absolute; top: 110px; left: 836px;">
        <asp:Button CssClass="botonFormulario" ID="btnRealizarConsulta" runat="server" Text="Buscar"
            TabIndex="1" onclick="btnRealizarConsulta_Click" 
            OnClientClick="MostrarCapaEspera();" 
            meta:resourcekey="btnRealizarConsultaResource1"/>
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
        style="position:absolute; top: 545px; left: 369px; height: 23px; width: 134px;">
         <input type=button id="btnInputExcel" class="botonFormulario" value="Exportar a Excel" onclick="javascript:ExportarExcel();return false;" tabindex="5" runat="server" />
    &nbsp;</div>
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
    </script>
         
   
</asp:Content>

