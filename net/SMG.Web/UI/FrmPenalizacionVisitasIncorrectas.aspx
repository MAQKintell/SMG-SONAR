<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/UI/MasterPageMntoGasSinMenuFacturacionAverias.master" CodeFile="FrmPenalizacionVisitasIncorrectas.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmPenalizacionVisitasIncorrectas" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
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
        
        function Busquedas()
        {
            var rd=document.getElementById("ctl00_ContentPlaceHolderContenido_rdbBusquedas0_0");
            rd.checked=false;
            var rd1=document.getElementById("ctl00_ContentPlaceHolderContenido_rdbBusquedas0_1");
            rd1.checked=false;
            
            document.getElementById("ctl00$ContentPlaceHolderContenido$btnProcesar").disabled='true';
        }
        function Consultas()
        {
            var rd=document.getElementById("ctl00_ContentPlaceHolderContenido_rdbBusquedas_0");
            rd.checked=false;
            var rd1=document.getElementById("ctl00_ContentPlaceHolderContenido_rdbBusquedas_1");
            rd1.checked=false;

            document.getElementById("ctl00$ContentPlaceHolderContenido$btnProcesar").removeAttribute('disabled');
        }
        </script>
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Visitas INCORRECTAS Bonificadas/Penalizadas" 
            meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
    <asp:ScriptManager ID="SM" runat="server"></asp:ScriptManager>

 
            <div style="position:absolute; top: 34px; left: 5px; width: 760px; height: 61px;" 
                __designer:mapid="21">
                <asp:Panel ID="Panel2" CssClass="labelFormulario" runat="server" 
                    GroupingText="PROCESOS" Width="964px" Height="67px" 
                    meta:resourcekey="Panel2Resource1">
                    <asp:RadioButtonList ID="rdbBusquedas0" runat="server" 
                        CssClass="campoFormulario" Height="32px" 
                        onselectedindexchanged="rdbBusquedas0_SelectedIndexChanged" 
                        meta:resourcekey="rdbBusquedas0Resource1" >
                        <asp:ListItem Value="4" Selected="True" meta:resourcekey="ListItemResource1">Visitas INCORRECTAS Penalizables</asp:ListItem>
                    </asp:RadioButtonList>
                </asp:Panel>
            </div>

 
    <asp:UpdatePanel ID="Resultados" runat="server">
        <ContentTemplate>
            <div id="divScroll" 
                style="position:absolute;overflow:scroll;width: 961px;height: 327px; top: 172px; left: 6px; border: 1px solid #000000; z-index:9995" 
                runat="server">
                <asp:GridView ID="grdListado" runat="server" AutoGenerateColumns="False" 
                        CssClass="contenidoTabla" 
                        
                    
                    style="z-index:99999;position:relative; top: -1px; left: 1px; height: 6px; border:1px solid #000000; width: 959px;" 
                    onrowdatabound="grdListado_RowDataBound" EnableModelValidation="True" 
                    meta:resourcekey="grdListadoResource1" >
                         <Columns>
                            
                            <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnIdSolicitud" runat="server" 
                                        Value='<%# Eval("ID_SOLICITUD") %>' />
                                </ItemTemplate> 
                            </asp:TemplateField>
                
                
                              <asp:BoundField DataField="ID_solicitud" HeaderText="COD. Solicitud"  
                                    ReadOnly="True" meta:resourcekey="BoundFieldResource1">
                               <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              
                              
                              
                              <asp:BoundField DataField="Cod_contrato" HeaderText="Código contrato"  
                                 ReadOnly="True" meta:resourcekey="BoundFieldResource2">
                               <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              
                              <asp:BoundField DataField="Descripcion_solicitud" HeaderText="Estado"  
                                 ReadOnly="True" meta:resourcekey="BoundFieldResource3">
                              <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              
                              
                              <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha creación"  
                                 ReadOnly="True" meta:resourcekey="BoundFieldResource4">
                              <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              <asp:BoundField DataField="Fecha_cierre" 
                                 HeaderText="Fecha ultimo movimiento (cierre)"  ReadOnly="True" 
                                 meta:resourcekey="BoundFieldResource5">
                              <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              
                              <asp:BoundField DataField="telefono_contacto" HeaderText="Teléfono contacto"  
                                    ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource6">
                              <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                                <asp:BoundField DataField="Persona_contacto" HeaderText="Persona contacto"  
                                    ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource7">
                              <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              
                              
                              
                              
                              <asp:TemplateField HeaderText="Observaciones" 
                                 meta:resourcekey="TemplateFieldResource2">
                                     <ItemTemplate>   
                                        <asp:Label ID="lb_Observaciones" runat="server" 
                                             ToolTip='<%# Eval("Observaciones") %>' 
                                             Text='<%# acorta(Eval("Observaciones")) %>'></asp:Label>  
                                     </ItemTemplate>
                              </asp:TemplateField> 
                             
                              <asp:CheckBoxField DataField="Urgente" HeaderText="URGENTE" 
                                 meta:resourcekey="CheckBoxFieldResource1" />
                              <asp:CheckBoxField DataField="BONIFICABLE" HeaderText="BONIFICABLE" 
                                 meta:resourcekey="CheckBoxFieldResource2" />
                              <asp:BoundField DataField="Proveedor" HeaderText="PROVEEDOR" 
                                 meta:resourcekey="BoundFieldResource8" />
                              
                              <asp:BoundField DataField="PROVINCIA" HeaderText="PROVINCIA"  
                                    ReadOnly="True" meta:resourcekey="BoundFieldResource9">
                                    <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              
                               <asp:BoundField DataField="POBLACION" HeaderText="POBLACION"  
                                    ReadOnly="True" meta:resourcekey="BoundFieldResource10">
                                    <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              
                              <asp:BoundField DataField="num_factura_visitas" HeaderText="FACTURA"  
                                    ReadOnly="True" meta:resourcekey="BoundFieldResource11">
                                    <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>

                         </Columns>
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

            <div style="position:absolute; top: 102px; left: 5px; width: 760px; height: 61px;">
                <asp:Panel ID="Panel1" CssClass="labelFormulario" runat="server" 
                    GroupingText="BUSQUEDAS" Width="964px" Height="67px" 
                    meta:resourcekey="Panel1Resource1">
                    <asp:RadioButtonList ID="rdbBusquedas" runat="server" 
                        CssClass="campoFormulario" Height="32px" 
                        onselectedindexchanged="rdbBusquedas_SelectedIndexChanged" 
                        meta:resourcekey="rdbBusquedasResource1" >
                        <asp:ListItem Value="4" meta:resourcekey="ListItemResource2">Visitas INCORRECTAS Penalizadas</asp:ListItem>
                    </asp:RadioButtonList>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        <br />
        <br />
        <br />
        
        <div id="div4" 
        style="position: absolute; top: 9px; left: 817px; width: 138px;">
            <asp:PlaceHolder ID="PlaceHolder3" runat="server">
                <asp:Label ID="Label3" CssClass="labelFormulario" visible="true" runat="server" 
                    Text="Proveedor: " meta:resourcekey="Label3Resource1"></asp:Label>
                <asp:DropDownList ID="cmbProveedorBusqueda" runat="server" 
                    meta:resourcekey="cmbProveedorBusquedaResource1">
                </asp:DropDownList>
            </asp:PlaceHolder>
        </div>
        
    
 
    <div id="divBotonRealizarBusqueda" 
        style="position: absolute; top: 13px; left: 685px;">
        <asp:Button CssClass="botonFormulario" ID="btnRealizarBusqueda" runat="server" Text="Buscar"
            TabIndex="1"  OnClientClick="MostrarCapaEspera();"
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
    
    <div id="div1" style="position: absolute; top: 44px; left: 342px; width: 206px;">
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <asp:Label ID="Label1" CssClass="labelFormulario" visible="true" runat="server" 
                Text="Núm. Factura: " Font-Bold="True" meta:resourcekey="Label1Resource1"></asp:Label>
            <asp:TextBox ID="txtNumFactura" runat="server" 
                meta:resourcekey="txtNumFacturaResource1"></asp:TextBox>
        </asp:PlaceHolder>
    </div>
    
    <div id="divBotonExcel" style="position:absolute; top: 545px; left: 369px; height: 23px; width: 258px;">
         <input type="button" id="btnInputExcel" name="btnInputExcel" class="botonFormulario" value="Exportar a Excel" onclick="javascript:ExportarExcel();return false;" tabindex="5" runat="server" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnProcesar" runat="server" Text="Procesar" 
             class="botonFormulario"  tabindex="6" onclick="btnProcesar_Click" 
             meta:resourcekey="btnProcesarResource1" />
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
