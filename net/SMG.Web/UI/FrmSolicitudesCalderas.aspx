<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGasSinMenuCALDERAS.master" AutoEventWireup="true" CodeFile="FrmSolicitudesCalderas.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmSolicitudesCalderas" EnableEventValidation="false" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Import Namespace="Iberdrola.Commons.Web" %>


<asp:content id="FrmSolicitudesCalderas" contentplaceholderid="ContentPlaceHolderContenido" runat="server">
    <link rel="stylesheet" type="text/css" href="../css/styles.css" />

    <link href="../css/ventana-modal.css" type="text/css" rel="stylesheet" /> 
    <link href="../css/ventana-modal-calderas.css" type="text/css" rel="stylesheet" />
	<script src="../js/ventana-modal.js" type="text/javascript"></script>
    
    <script type="text/javascript" src="../js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../js/jquery.flip.min.js"></script>
    <script type="text/javascript" src="../js/scriptCalderas.js"></script>
    
    <link rel="stylesheet" href="HTML/Css/jquery.jgrowl.css" type="text/css" />
<%--    <script type="text/javascript" src="HTML/Js/jquery-1.4.2.js"></script>
    <script type="text/javascript" src="HTML/Js/jquery.ui.all.js"></script>--%>
    <script type="text/javascript" src="HTML/Js/jquery.jgrowl.js"></script>

<style type="text/css">
    .historic {
        float: inherit;
	    height: 530px;
	    width: 700px;
	    border: 2px outset #999999;
	    background-color: #dce4d9;
	    padding: 20px;
        color: white;
    }
</style>

<script type = "text/javascript" >

    function CargarDatosHistorico(Valor) {
        //abrirVentanaEspera("frmEspera.aspx", 620, 400, "ventana-modal","","1","1");
        //abrirVentanaAlerta(620, 400, "ventana-modal","CARGANDO DATOS...","1","1");
        MostrarCapaEspera();
        //window.form1.style.visibility ="hidden";

        var hiddenVisita = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodVisita");
        //alert(hiddenVisita.value);
        hiddenVisita.value = "";
        //alert(hiddenVisita.value);

        var hiddenSolicitud = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitudHistorico");
        hiddenSolicitud.value = Valor;
        javascript: __doPostBack('gv_HistoricoSolicitudes', Valor);

    }

       function VerEditar() {
        //var lblProceso = document.getElementById("ctl00_ContentPlaceHolderContenido_lblProceso");
        //lblProceso.innerText = '';
        //alert('aaaa');

        var divVisitas = document.getElementById("divVisitas");
        var divSolicitudes = document.getElementById("divSolicitudes");
        var divPestanias = document.getElementById("divPestanias");
        var divBotonNuevo = document.getElementById("divBotonNuevo");
        var divBtnExcel = document.getElementById("divBtnExcel");
        var divEditar = document.getElementById("divEditar");
        var divContador = document.getElementById("divContador");
        var btnVisitas = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPestanyaVisitas");
        var btnSolicitudes = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPestanyaSolicitudes");


        divVisitas.style.visibility = "hidden";
        divBotonNuevo.style.visibility = "hidden";
        //divPestanias.style.visibility = "hidden";
        divBtnExcel.style.visibility = "hidden";
        divSolicitudes.style.visibility = "hidden";
        divContador.style.visibility = "hidden";
        divEditar.style.visibility = "visible";

        btnVisitas.disabled = false;
        btnVisitas.src = "./HTML/Images/VisitasDesactivado_HORIZONTAL.jpg";


        btnSolicitudes.disabled = false;
       // btnSolicitudes.src = "./HTML/Images/SolicitudesDesactivado_HORIZONTAL.jpg";


        var hiddenPanelSeleccionado = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnPanelSeleccionado");
        hiddenPanelSeleccionado.value = "NUEVO";

    }

       function AbrirSolicitud(idSolicitud, codContrato) {
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText == undefined) {
            alert('<%=Resources.TextosJavaScript.TEXTO_ERROR_CARGA_DATOS_2%>');
        }
        else {
            abrirVentanaLocalizacion1("FrmModalAbrirSolicitudProveedor.aspx?codVisita=" + document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodVisita").value + "&telefono=" + document.getElementById("ctl00_ContentPlaceHolderContenido_lblTelefonoContacto").innerText + "&Contrato=" + document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText, 400, 200, "ventana-modal", "ALTA", "1", "0");
        }
    }

    function AbrirSolicitudInspeccionGas() {
        // alert(document.getElementById("ctl00$ContentPlaceHolderContenido$hdnCodVisita").value);
        // alert(document.getElementById("ctl00_ContentPlaceHolderContenido_lblTelefonoContacto").innerText);
        //  alert(document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText);
        var codContrato = document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo");
        //  alert(codContrato.innerText);
        //  abrirVentanaLocalizacion("FrmModalAbrirSolicitudInspeccion.aspx?telefono=" + document.getElementById("ctl00_ContentPlaceHolderContenido_lblTelefonoContacto").innerText + "&Contrato=" + document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText, 400, 200, "ventana-modal", "ALTA INSPECCION", "1", "0");
        abrirVentanaLocalizacion("frmModalModificarSolicitud.aspx?telefono=" + document.getElementById("ctl00_ContentPlaceHolderContenido_lblTelefonoContacto").innerText + "&idSolicitud=0&Inspeccion=1&Calderas=0&Contrato=" + codContrato.innerText, 800, 550, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_DATOS_SOLICITUD%>; CONTRATO: " + codContrato.innerText, "0", "1", true);

        //    return false;
    }

 
    //    changeHashOnLoad();

    //    function changeHashOnLoad() {
    //        window.location.href += "#";
    //        setTimeout("changeHashAgain()", "50");
    //    }

    //    function changeHashAgain() {
    //        window.location.href += "1";
    //    }

    //    var storedHash = window.location.hash;
    //    window.setInterval(function() {
    //        if (window.location.hash != storedHash) {
    //            window.location.hash = storedHash;
    //        }
    //    }, 50);


</script>


 <asp:HiddenField ID="hdnCodEFV" runat="server" />  
 <asp:HiddenField ID="hdnControlesPagina" runat="server" />  
    <asp:HiddenField ID="hdnDNI" runat="server" />  
    <asp:HiddenField ID="hdnCP" runat="server" />  
 
  <div style="position:absolute; top: 582px; left: 487px; z-index:99999; height: 25px;">
  <%--<a href="#" id="S_explode" style="border-style: none; border-width: 0px">--%>
    <img  id="S_explode" src="HTML/Images/icono_ventana_foto.png" alt="Mostrar Historico" 
        style="height: 18px; width: 18px;cursor:pointer;"/>
  <%--</a>--%>
  </div>
  
  <div style="position:absolute; top: 180px; left: 149px; z-index:1000; height: 25px;">
  <%--<a href="FrmContratos.aspx" id="VisitasProveedor" runat="server" style="border-style: none; border-width: 0px" visible="false">IR A VISITAS</a>--%>
    <asp:LinkButton ID="VisitasProveedor" runat="server" class="desconectar" 
          OnClientClick="javascript:MostrarCapaEspera();window.location.href ='Frmcontratos.aspx'; return false;" 
          CausesValidation="False" Text="IR A VISITAS" 
          meta:resourcekey="VisitasProveedorResource1"></asp:LinkButton>
  </div>
  
  
<div style="position:absolute; top: 10px; left: 190px; z-index:9999999">
 <div id="mostrar" class="historic" style="position:absolute; top: 10px; left: -30px; visibility:hidden;">
      <h3 align="center"><asp:Label runat="server" Font-Size="XX-Large" 
              BackColor="#006699" Font-Bold="True" ForeColor="White" 
              BorderColor="Transparent" ID="lblHistorico" Text="HISTORICO SOLICITUD" 
              meta:resourcekey="lblHistoricoResource1"></asp:Label>
      <a href="#" id="H_explode"><img id="Img2" src="HTML/Images/Expand.jpg" alt="Cerrar Historico" style="height: 18px; width: 18px;cursor:pointer;"/></a>
      </h3>
      <div id="divHistorical" style="overflow:scroll;top:255px;left:5px;height:480px;width:665px;border:solid 1px #000000" class="style32">
                <asp:GridView ID="grdHistorical" runat="server" BackColor="White" 
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                AutoGenerateColumns="False" Width="600px" CssClass="contenidoTabla"
                    onrowdatabound="gv_HistoricoSolicitudes_RowDataBound" 
                    EnableModelValidation="True" meta:resourcekey="grdHistoricalResource1" >
                                <RowStyle ForeColor="#000066" BorderColor ="Transparent" />
                                <Columns>
                                  <asp:BoundField DataField="id_movimiento" HeaderText="id_movimiento"  
                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource1">
                                      <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                   <asp:BoundField DataField="tipo_movimiento" HeaderText="tipo_movimiento"  
                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource2">
                                       <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="id_solicitud" HeaderText="Solicitud"  
                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource3">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/flecha.gif" 
                                        ShowSelectButton="True" meta:resourcekey="CommandFieldResource1" />
                                  <asp:BoundField DataField="estado_solicitud" HeaderText="Estado"  ReadOnly="True" 
                                        meta:resourcekey="BoundFieldResource4">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                 <asp:BoundField DataField="usuario" HeaderText="Usuario"  ReadOnly="True" 
                                        meta:resourcekey="BoundFieldResource5">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                    <asp:BoundField DataField="observaciones" HeaderText="Observaciones"  
                                        ReadOnly="True" meta:resourcekey="BoundFieldResource6">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                    <asp:BoundField DataField="time_stamp" HeaderText="Fecha / Hora" 
                                        meta:resourcekey="BoundFieldResource7" />
                                    
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" BorderColor ="Transparent" />
                            </asp:GridView>
        </div>
      
    </div>
</div>
   
   
   
   
   
   
    <div id="divOculto" style="width: 958px; position:absolute; top: 0px; left: 0px;" >
           <asp:HiddenField ID="HiddenField1" runat="server" Visible="False" />        
           <asp:HiddenField ID="hdnAviso" runat="server" />       
           <asp:HiddenField ID="hdnBusquedaColores" runat="server" />       
           <asp:HiddenField ID="hdnModo" runat="server" Visible="False" />        
           <asp:HiddenField ID="hdnCodMarcaCaldera" runat="server" Visible="False" />    
           <asp:HiddenField ID="hdnCodProveedor" runat="server" Visible="False" />        
           <asp:HiddenField ID="hdnCodPerfil" runat="server" Visible="False" />        
           <asp:HiddenField ID="hdnPanelSeleccionado" runat="server" />
           <asp:HiddenField ID="hdnSeleccionadoAlta" runat="server" />
           <asp:HiddenField ID="hdnEstadoSol" runat="server" />
           <asp:HiddenField ID="hdnDesEstadoSol" runat="server" />
           <asp:HiddenField ID="hdnEsSMGAmpliado" runat="server" />
           <asp:HiddenField ID="hdnEsSMGAmpliadoIndependiente" runat="server" />
           <asp:HiddenField ID="hdnSubtipoAveriaSobreGC" runat="server" />
           <asp:HiddenField ID="hdnModificandoSolicitud" runat="server" />
           <input type="hidden" id="hdnCodVisita" runat="server" />
           <asp:DropDownList Visible="False" ID="ddl_Poblacion" runat="server" 
               CssClass="campoFormulario" AutoPostBack="True" Width="98px" 
               meta:resourcekey="ddl_PoblacionResource1"></asp:DropDownList>
    </div>
    <asp:HiddenField ID="hdnNumeroEquivalencias" runat="server" />
    <asp:HiddenField ID="hdnBuscarPulsado" runat="server" />

    
    <div id="divFiltros" 
        style="width: 972px; position:absolute; top: 0px; left: 0px; height: 71px;z-index:10">
        <asp:Panel ID="panelFiltro" runat="server" BorderStyle="Solid" 
            BorderWidth="1px" Height="91px" Width="973px" 
            meta:resourcekey="panelFiltroResource1">
                <div style="position:absolute; top: 11px; left: 2px; width:18px; height:60px;visibility:hidden" >
                    <img alt="Filtro Busqueda" src="./HTML/Images/FiltroBusqueda.jpg" />
                </div>
            
            <div style="position:absolute; top: 0px; left: 20px; height: 14px;">
                <table width="100%" style="height:100%" class="contenidoTabla" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="left" class="contenidoTabla">
                                <asp:Label ID="Label33" runat="server" Text="Cód.Contrato:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label33Resource1"></asp:Label>
                            </td>
                            <td class="contenidoTabla" align="left">
                                <asp:TextBox ID="txt_contrato" runat="server" Width="60px" 
                                    class="campoFormulario" MaxLength="10" meta:resourcekey="txt_contratoResource1"></asp:TextBox>
                            </td>
                            <td align="left" class="contenidoTabla">
                                <asp:Label ID="Label31" runat="server" Text="Solicitud:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label31Resource1"></asp:Label>
                            </td>
                            <td class="">
                                <asp:TextBox ID="txt_solicitud" runat="server" Width="60px" 
                                    class="campoFormulario" MaxLength="10" 
                                    meta:resourcekey="txt_solicitudResource1"></asp:TextBox>
                            </td>
                            <td align="left" class="contenidoTabla">
                                <asp:Label ID="Label38" runat="server" Text="Subtipo:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label38Resource1"></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="ddl_Subtipo" runat="server" CssClass="campoFormulario" 
                                    AutoPostBack="True" Width="198px" 
                                    onselectedindexchanged="ddl_Subtipo_SelectedIndexChanged" 
                                    meta:resourcekey="ddl_SubtipoResource1"></asp:DropDownList>
                            </td>
                            <td align="left" class="contenidoTabla" >
                                <asp:Label ID="Label32" runat="server" Text="Provincia:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label32Resource1"></asp:Label>
                            </td>
                            <td class="contenidoTabla">
                            <asp:DropDownList ID="ddl_Provincia" runat="server" CssClass="campoFormulario" 
                                    Width="120px" meta:resourcekey="ddl_ProvinciaResource1" ></asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label39" runat="server" Text="Fecha Alta Desde:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label39Resource1"></asp:Label></td>
                            <td class="contenidoTabla">
                                <asp:TextBox ID="txtFechaDesde" CssClass="campoFormulario" width="43px" runat="server" 
                                    MaxLength="10" 
                                    ToolTip="Fecha en la que se creó la solicitud"
                                    
                                    onkeypress="if(window.event.keyCode == 13){ RealizarConsulta();return false;}" meta:resourcekey="txtFechaDesdeResource1"
                                 />
                                <img id="imgFechaHasta" runat="server" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" onclick="ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaDesde');" >
                            </td>
                            <td align="left" class="style19">
                                <asp:Label ID="Label312" runat="server" Text="Fecha Alta Hasta:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label312Resource1"></asp:Label></td>
                            <td class="contenidoTabla">
                                <asp:TextBox ID="txtFechaHasta" CssClass="campoFormulario" width="52px" runat="server" 
                                    MaxLength="10" 
                                    ToolTip="Fecha en la que se creó la solicitud"
                                    
                                    onkeypress="if(window.event.keyCode == 13){ RealizarConsulta();return false;}" meta:resourcekey="txtFechaHastaResource1"
                                 />
                               <img id="img1" runat="server" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" onclick="ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaHasta');" >
                            </td>
                            <td align="left" class="contenidoTabla">
                                <asp:Label ID="Label37" runat="server" Text="Estado:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label37Resource1"></asp:Label>
                            </td>
                            <td class="contenidoTabla">
                                <asp:DropDownList ID="ddl_Estado" runat="server" CssClass="campoFormulario" 
                                    Width="198px" meta:resourcekey="ddl_EstadoResource1"></asp:DropDownList>
                            </td>
                            <td align="left" class="contenidoTabla"  >
                                <asp:Label ID="Label35" runat="server" Text="Ver Urgentes:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label35Resource1"></asp:Label>
                                <asp:CheckBox ID="chk_Urgente" runat="server" class="campoFormulario" 
                                    meta:resourcekey="chk_UrgenteResource1" />
                            </td>
                            <td align="left" class="contenidoTabla" >
                                <asp:Label ID="Label34" runat="server" Text="Ver Pendientes:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label34Resource1"></asp:Label> 
                                <asp:CheckBox ID="chk_Pendientes" Checked="True" runat="server" 
                                    class="campoFormulario" meta:resourcekey="chk_PendientesResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="contenidoTabla">
                                <asp:Label ID="Label30" runat="server" Text="Nombre Cliente:" 
                                    CssClass="labelFormulario" Font-Underline="False" 
                                    meta:resourcekey="Label30Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNombre" runat="server"  Width="80px" 
                                    class="campoFormulario" meta:resourcekey="txtNombreResource1"></asp:TextBox>
                            </td>
                           <td align="left" class="contenidoTabla">
                                <asp:Label ID="Label40" runat="server" Text="1º Apellido Cliente:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label40Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApellido1" runat="server"  Width="80px" 
                                    class="campoFormulario" meta:resourcekey="txtApellido1Resource1"></asp:TextBox>
                                </td>
                            <td align="left" class="contenidoTabla">
                                <asp:Label ID="Label41" runat="server" Text="2º Apellido Cliente:" 
                                    CssClass="labelFormulario" meta:resourcekey="Label41Resource1"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtApellido2" runat="server"  Width="80px" 
                                    class="campoFormulario" meta:resourcekey="txtApellido2Resource1"></asp:TextBox>
                            </td>
                        </tr>
                     <tr>
                            <td align="left" class="contenidoTabla">
                                <asp:Label ID="Label53" runat="server" Text="DNI:" meta:resourcekey="txtDNIResource"
                                    CssClass="labelFormulario" Font-Underline="False" 
                                    ></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDNI" runat="server"  Width="80px" 
                                    class="campoFormulario" ></asp:TextBox>
                                </td>
                           <td align="left" class="contenidoTabla">
                                <asp:Label ID="Label55" runat="server" Text="CUI:" meta:resourcekey="txtCUIResource"
                                    CssClass="labelFormulario" ></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCUI" runat="server"  Width="120px" 
                                    class="campoFormulario" ></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="position:absolute; top: 65px; left: 635px;">
                    
                    <asp:Button ID="btnBuscar" runat="server"  Text="Buscar" 
                        CssClass="botonFormulario" 
                        OnClientClick ="MostrarCapaEspera();InformarPulsadoBuscar();" 
                        onclick="btnBuscar_Click" meta:resourcekey="btnBuscarResource1" 
                        Width="99px"/>
                    
                    <asp:Button ID="btnBuscarCalderas" runat="server"  Text="GAS CONFORT" 
                        CssClass="botonFormulario" BackColor="#FF6600" ForeColor="White" 
                        OnClientClick ="MostrarCapaEspera();InformarPulsadoBuscar();"
                        onclick="btnBuscarCalderas_Click" 
                        meta:resourcekey="btnBuscarCalderasResource1" />
                    
                    <asp:Button ID="btnLimpiarFiltro" runat="server"  Text="Limpiar Filtro" 
                        CssClass="botonFormulario" OnClientClick="return Limpiar();" 
                        onclick="btnLimpiarFiltro_Click" meta:resourcekey="btnLimpiarFiltroResource1"/>
                        
                    
                    
                </div>
          </asp:Panel>
    </div>
    
    
    <div class="GridMovimientoCalderas" id="GridMovimientoCalderas" style="position:absolute;z-Index:89000;">
	<div class="GridDatosCalderas" id="GridDatosCalderas">

    <div id="divContador" 
        style="position:absolute; top: 357px; left: 5px; height: 18px; width: 549px;z-index:1" 
        align="center">
        <table width="100%" align="left">
            <tr>
                <td align="left">
                    <asp:PlaceHolder ID="placeHolderPaginacion" runat="server">
                    &nbsp;  
                    </asp:PlaceHolder>
                </td>
            
                <td align="left">
                <asp:Label CssClass="labelFormularioValor" ID="Label43" runat="server" 
                        Text="Num. Registros encontrados:" meta:resourcekey="Label43Resource1"></asp:Label>
                    <asp:Label CssClass="labelFormularioValor" ID="lblNumEncontrados" 
                        ForeColor="Red" runat="server" Text="0" 
                        meta:resourcekey="lblNumEncontradosResource1"></asp:Label>
                    <asp:Label CssClass="labelFormularioValor" ID="Label45" runat="server" 
                        Text=" - Mostrando del " meta:resourcekey="Label45Resource1"></asp:Label>
                    <asp:Label CssClass="labelFormularioValor" ID="lblNumEncontrados1" 
                        runat="server" Text="0" meta:resourcekey="lblNumEncontrados1Resource1"></asp:Label>
                    <asp:Label CssClass="labelFormulario" ID="lblContador" runat="server" 
                        Text=" al " meta:resourcekey="lblContadorResource1"></asp:Label>
                    <asp:Label CssClass="labelFormularioValor" ID="lblNumRegistros" runat="server" 
                        Text="0 " meta:resourcekey="lblNumRegistrosResource1"></asp:Label>
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
        <asp:Button ID="btnPaginacion" runat="server" style="visibility:hidden" OnClick="OnBtnPaginacion_Click" Width="0px" meta:resourcekey="btnPaginacionResource1"/> 
    </div>
    
    <!-- Div de los datos del contrato -->
    <div id="divDatosContrato" style="width: 973px; position:absolute; top: 0px; left: 0px; height: 103px;border:solid 1px #000000">
        <div style="border: 1px solid #000000;position:absolute; top: -1px; left: -1px; width:18px; height:103px" >
            <div style="position:absolute; top: 8px; left: 2px; width:18px; height:103px; visibility:hidden" >
                <img src="./HTML/Images/DatosContrato.jpg" alt="Datos Contrato" />
            </div>
        </div>
        <div class="panelTabla" style="position:absolute; top: 0px; left: 19px; background-color:White;border:0px;">
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="../Imagenes/fondo.png" 
                        Height="103px" Width="954px" BorderWidth="0px" BorderStyle="None" 
                meta:resourcekey="Panel1Resource1">
            <table width="100%" cellpadding="0" cellspacing="1" border="0" 
                style="height: 101px">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Font-Underline="False" 
                            ForeColor="#FFC080" CssClass="labelFormulario" Text="Cod.Contrato:" 
                            meta:resourcekey="Label1Resource1"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblCodContratoInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="0449873220" 
                            meta:resourcekey="lblCodContratoInfoResource1"></asp:Label>
                        <asp:Label ID="Label2" Font-Underline="False" runat="server" 
                            ForeColor="#FFC080" CssClass="labelFormulario" Text="Estado:" 
                            meta:resourcekey="Label2Resource1"></asp:Label>
                        <asp:Label ID="lblEstadoContratoInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="AL" 
                            meta:resourcekey="lblEstadoContratoInfoResource1"></asp:Label>
                        <asp:Label ID="Label22" Font-Underline="False" runat="server" 
                            ForeColor="#FFC080" CssClass="labelFormulario" Text="Telef. Contacto:" 
                            meta:resourcekey="Label22Resource1"></asp:Label>
                        <asp:Label ID="lblTelefonoContacto" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="AL" 
                            meta:resourcekey="lblTelefonoContactoResource1"></asp:Label>
                    </td>
                    <td>        
                        <asp:Label ID="Label3" Font-Underline="False" runat="server" 
                            ForeColor="#FFC080" CssClass="labelFormulario" Text="COD RECEPTOR:" 
                            meta:resourcekey="Label3Resource1"></asp:Label>                
                        <asp:Label ID="lblCODRECEPTORInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="ES0123456789012345678" 
                            meta:resourcekey="lblCODRECEPTORInfoResource1"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="Label6" Font-Underline="False" runat="server" CssClass="labelFormulario" 
                            Text="Cliente:" Font-Bold="True" ForeColor="#FFC080" 
                            meta:resourcekey="Label6Resource1"></asp:Label>
                        <asp:Label ID="lblClienteInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="AITOR ARTECHE DE MIGUEL" 
                            meta:resourcekey="lblClienteInfoResource1"></asp:Label>
                    </td>
                    <td colspan="2">  
                        <asp:Label ID="lblEmailLit" Font-Underline="False" runat="server" CssClass="labelFormulario" 
                            Text="Email Cli:" Font-Bold="True" ForeColor="#FFC080" 
                            meta:resourcekey="lblEmailLitResource1"></asp:Label>
                        <asp:Label ID="lblEmail" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="nombre@gmail.com" 
                            meta:resourcekey="lblEmailResource1"></asp:Label>
                    </td>
                    
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="Label8" Font-Underline="False" runat="server" CssClass="labelFormulario" 
                            Text="Dir. Suministro:" Font-Bold="True" ForeColor="#FFC080" 
                            meta:resourcekey="Label8Resource1"></asp:Label>
                        </td>
                        <td colspan="3">
                        <asp:Label ID="lblSuministroInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor" 
                            Text="CL Nueve de Octubre 0113, 5, 0, 12550, ALMASSORA, CASTELLÓN" 
                            meta:resourcekey="lblSuministroInfoResource1"></asp:Label>
                        <asp:Label ID="Label7" Font-Underline="False" Visible="False" 
                            ForeColor="#FFC080" runat="server" CssClass="labelFormulario" 
                            Text="Marca Caldera:" meta:resourcekey="Label7Resource1"></asp:Label>
                        <asp:Label ID="lblCalderaInfo" runat="server" Visible="False" ForeColor="White" 
                            CssClass="labelFormularioContratoValor" Text="Saunier" 
                            meta:resourcekey="lblCalderaInfoResource1"></asp:Label>
                            </td>
                        <td colspan="4">
                        <asp:Label ID="Label17" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Urgencia:" 
                            meta:resourcekey="Label17Resource1"></asp:Label>
                        <asp:Label ID="lblUrgenciaInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="BONIFICABLE" 
                            meta:resourcekey="lblUrgenciaInfoResource1"></asp:Label>
                    <asp:Label ID="Label18" runat="server" Font-Underline="False" ForeColor="#FFC080" 
                            CssClass="labelFormulario" Text="EFV:" meta:resourcekey="Label18Resource1"></asp:Label>
                    <asp:Label ID="lblEFVInfo" runat="server" ForeColor="White" 
                                CssClass="labelFormularioContratoValor" meta:resourcekey="lblEFVInfoResource1"></asp:Label>
                    <asp:Label ID="Label26" runat="server" Font-Underline="False" ForeColor="#FFC080" 
                            CssClass="labelFormulario" Text="Ave.Resueltas:" 
                            meta:resourcekey="Label26Resource1"></asp:Label>
                    <asp:Label ID="lblNAveriasResueltas" runat="server" ForeColor="White" 
                                CssClass="labelFormularioContratoValor" 
                            meta:resourcekey="lblNAveriasResueltasResource1"></asp:Label>
                    <asp:Label ID="lblLeyendaImporteSiBaja" Visible="False" runat="server" 
                            Font-Underline="False" ForeColor="Red" Font-Bold="True" 
                            CssClass="labelFormulario" Text="Regularización:" 
                            meta:resourcekey="lblLeyendaImporteSiBajaResource1"></asp:Label>
                    <asp:Label ID="lblImporteSiBaja" runat="server" ForeColor="White" 
                                CssClass="labelFormularioContratoValor" meta:resourcekey="lblImporteSiBajaResource1"></asp:Label>
                    </td>
                    <td>  
                        <asp:Label ID="Label20" Font-Underline="True" runat="server" CssClass="labelFormulario" 
                            Text="S.PROV.:" Font-Bold="True" ForeColor="#FF6600" 
                            meta:resourcekey="Label20Resource1"></asp:Label>
                        <asp:Label ID="lblScoring" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="S" 
                            meta:resourcekey="lblScoringResource1"></asp:Label>
                     </td>
                    <td>
                        <asp:Label ID="Label27" Font-Underline="True" runat="server" CssClass="labelFormulario" 
                            Text="Baja Sol.:" Font-Bold="True" ForeColor="#FFC080" 
                            meta:resourcekey="Label27Resource1"></asp:Label>
                        <asp:Label ID="lblBajaSolicitada" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor" meta:resourcekey="lblBajaSolicitadaResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Servicio" 
                            meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblServicioInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  
                            Text="Servicio Mantenimiento Gas Calefacción con pago fraccionado" 
                            meta:resourcekey="lblServicioInfoResource1"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Label10" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Fec.Alta Serv.:" 
                            meta:resourcekey="Label10Resource1"></asp:Label>
                        <asp:Label ID="lblFecAltaServInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="01/01/2012" 
                            meta:resourcekey="lblFecAltaServInfoResource1"></asp:Label>
                    </td>
                    <td colspan="2">        
                        <asp:Label ID="Label12" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Fec.Baja Serv.:" 
                            meta:resourcekey="Label12Resource1"></asp:Label>                
                        <asp:Label ID="lblFecBajaServInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="31/12/2012" 
                            meta:resourcekey="lblFecBajaServInfoResource1"></asp:Label>
                    </td>
                    <td colspan="2">      
                        <asp:Label ID="Label14" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Fec.Renovación:" 
                            meta:resourcekey="Label14Resource1"></asp:Label>                
                        <asp:Label ID="lblFecLimiteVisInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="31/12/2012" 
                            meta:resourcekey="lblFecLimiteVisInfoResource1"></asp:Label>
                      </td>
                    <td >
                        <asp:Label ID="Label29" Font-Underline="True" Font-Bold="True" 
                            ForeColor="#FF6600" runat="server" CssClass="labelFormulario" Text="S.DEF.:" 
                            meta:resourcekey="Label29Resource1"></asp:Label>                
                        <asp:Label ID="lblSolcent" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="N" 
                            meta:resourcekey="lblSolcentResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label19" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Proveedor:" 
                            meta:resourcekey="Label19Resource1"></asp:Label>
                    </td>
                    <td >
                        <asp:Label ID="lblProveedorInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="SIEL" 
                            meta:resourcekey="lblProveedorInfoResource1"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label21" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Telf:" 
                            meta:resourcekey="Label21Resource1"></asp:Label>
                        <asp:Label ID="lblProvTelInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="9465432142" 
                            meta:resourcekey="lblProvTelInfoResource1"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:Label ID="Label23" Font-Underline="False" ForeColor="#FFC080"  
                            runat="server" CssClass="labelFormulario" Text="Proveedor Averías:" 
                            meta:resourcekey="Label23Resource1"></asp:Label>
                        <asp:Label ID="lblProvAveriaInfo" runat="server" 
                            CssClass="labelFormularioContratoValor" ForeColor="White" Text="MAFRE" 
                            meta:resourcekey="lblProvAveriaInfoResource1"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label25" Font-Underline="False" runat="server" 
                            ForeColor="#FFC080" CssClass="labelFormulario" Text="Telf:" 
                            meta:resourcekey="Label25Resource1"></asp:Label>
                        <asp:Label ID="lblProvAveriaTelInfo" runat="server" 
                            CssClass="labelFormularioContratoValor" ForeColor="White" Text="9465432142" 
                            meta:resourcekey="lblProvAveriaTelInfoResource1"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label49" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Id. Solicitud:" 
                            meta:resourcekey="Label49Resource1"></asp:Label>
                        <asp:Label ID="lblIdSolicitudInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor" 
                            meta:resourcekey="lblIdSolicitudInfoResource1"></asp:Label>
                         </td>
                    <td>
                        <asp:Label ID="lblTitCategoriaVisita" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Cat. Vis:" 
                            meta:resourcekey="lblTitCategoriaVisitaResource1"></asp:Label>
                        <asp:Label ID="lblCategoriaVisita" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor" 
                            meta:resourcekey="lblCategoriaVisitaResource1"></asp:Label>
                    </td>
                 </tr>
                 <tr>
                    <td>
                        <asp:Label ID="Label28" Font-Underline="False" ForeColor="#FFC080" 
                            runat="server" CssClass="labelFormulario" Text="Observaciones:" 
                            meta:resourcekey="Label28Resource1"></asp:Label>
                    </td>
                    <td colspan="7">
                        <asp:Label ID="lblObservacionesInfo" runat="server" ForeColor="White" 
                            CssClass="labelFormularioContratoValor"  Text="Observaciones del contrato....." 
                            meta:resourcekey="lblObservacionesInfoResource1"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Mensaje" runat="server" ForeColor="Red" 
                            CssClass="labelFormulario" meta:resourcekey="lbl_MensajeResource1"></asp:Label></td>
                 </tr>
            </table>
            </asp:Panel>
        </div>
        
        <div style="position:absolute; top:105px; left: 620px;width: 135px;">
         <asp:Button BackColor="Black" ForeColor="#66CCFF" ID="btnAbrirSolicitudProveedor" 
                                            CssClass="botonFormulario" 
                                            OnClientClick="AbrirSolicitud();return false;" runat="server" 
                                            style="text-align:center;" Text="ALTA SOLICITUD" />
        </div>
          <div style="position:absolute; top:105px; left: 485px;width: 135px;">
         <asp:Button BackColor="Black" ForeColor="#66CCFF" ID="btnAbrirSolicitudInspeccionGas" Visible="false" 
                                            CssClass="botonFormulario" 
                                            OnClientClick="AbrirSolicitudInspeccionGas();return false;" runat="server" 
                                            style="text-align:center;" Text="ALTA INSPECCION" meta:resourcekey="btnAbrirSolicitudInspeccionGasResource1"/>
        </div>
        <div style="position:absolute; top:105px; left: 885px;width: 135px;">
         <asp:Button BackColor="Black" ForeColor="#66CCFF" ID="btnAltaGC" Visible="false" 
                                            CssClass="botonFormulario" OnClientClick ="MostrarCapaEspera();"
                                            onclick="btnAbrirSolicitudGC_Click" runat="server" 
                                            style="text-align:center;z-index:1" Text="ALTA GAS CONFORT" meta:resourcekey="btnAbrirSolicitudGCResource1"/>
        </div>
        <div style="position:absolute; top:105px; left: 380px;width: 100px;">
            <asp:Button ID="btnExportarExcelBeretta" runat="server" Visible="False" 
                BackColor="#FF6600"  ForeColor="White" CssClass="botonFormulario" 
				OnClientClick ="MostrarCapaEspera();"
                Text="EXPORTACION SOL GC" onclick="btnExportarExcelBeretta_Click" 
                Width="150px" meta:resourcekey="btnExportarExcelBerettaResource1" />
            
        </div>
        
        <div id="divBotonNuevo" style="position:absolute;top:105px; left: 735px;">                  
             <asp:Button ID="btnNuevaSolicitud" runat="server" Text="Nueva Solicitud" 
                 CssClass="botonFormulario" 
                 OnClientClick="onBtnNuevaSolicitudClick(); return false;" Enabled="False" 
                 meta:resourcekey="btnNuevaSolicitudResource1"/>
        </div>
        
        <div id="divBotonReclamacion" style="position:absolute;top:105px; left: 837px;">                  
             <asp:Button ID="btnNuevaReclamacion" runat="server" Text="Nueva Reclamacion" 
                 CssClass="botonFormulario" 
                 OnClientClick="onBtnNuevaSolicitudReclamacionClick(); return false;" 
                 Visible="False" meta:resourcekey="btnNuevaReclamacionResource1"/>
        </div>
        
        <%--<div id="divEFV" style="position:absolute;top:21px; left: 796px;">                  
            <asp:Label ID="Label18" runat="server" Font-Underline="false" ForeColor="#FFC080" CssClass="labelFormulario" Text="EFV:"></asp:Label>
            <asp:Label ID="lblEFVInfo" runat="server" ForeColor="white" CssClass="labelFormularioValor" Text=""></asp:Label>
        </div>--%>
        <div id="divBotonBajaServicio" 
            style="position:absolute;top:105px; left: 608px; width: 132px;">                  
             <asp:Button ID="btnBajaServicio" runat="server" Text="Dar de Baja el Servicio" 
                 CssClass="botonFormulario" OnClientClick="return Preguntar();" 
                 OnClick="onBtnBajaServicio_Click" visible="false" 
                 Width="131px" meta:resourcekey="btnBajaServicioResource1"/>
        </div>
        
        
        
        <div id="divUltimoMovimiento" runat="server" class="bottom-right jGrowl" style="position:absolute; top:345px; left: 625px; height:800px; width:330px;z-index:9">
    <div style="display: block;background-color:#003300;opacity:.85;">
    <%--<asp:label Font-Bold="true" ID="leyenda" runat="server" Text="Leyenda Colores (última modificación por parte del...):"></asp:label>--%>
            <asp:Button ID="btnTelefono" runat="server" Text="TELEFONO" BackColor="Transparent" 
                    BorderStyle="Double" Font-Bold="True" ForeColor="GreenYellow" 
                    Height="23px" style="cursor:pointer" onclick="btnTelefono_Click" 
            OnClientClick="InicializarSolicitud();" 
            meta:resourcekey="btnTelefonoResource1" />
                &nbsp;
                  
            <asp:Button ID="btnProveedor" runat="server" Text="PROVEEDOR" BackColor="Transparent" 
                    BorderStyle="Double" Font-Bold="True" ForeColor="RoyalBlue" 
                    Height="23px" style="cursor:pointer" onclick="btnProveedor_Click" 
            OnClientClick="InicializarSolicitud();" 
            meta:resourcekey="btnProveedorResource1" />
                &nbsp;

            <asp:Button ID="btnAdministrador" runat="server" Text="ADMINISTRADOR" BackColor="Transparent" 
                    BorderStyle="Double" Font-Bold="True" ForeColor="Red" Height="23px" 
                    Width="129px" style="cursor:pointer" 
            onclick="btnAdministrador_Click" OnClientClick="InicializarSolicitud();" 
            meta:resourcekey="btnAdministradorResource1" />
            
    </div>
</div>
    </div>
    
    <!-- Div de las pestañas-->
    <div id="divPestanias" style="width: 350px; position:absolute; top: 115px; left: 2px; height: 31px; z-index:12;width:100px">
        <div id="divPestaniaSolicitudes" style="position:absolute; height:3px; top: -1px; left: -2px; width:122px" >

                <asp:Button ID="btnPestanyaSolicitudes" runat="server" Text="Solicitudes" 
            CssClass="botonFormulario" OnClientClick="VerSolicitudes(); return false;" 
            AlternateText="Solicitudes" BackColor="#DCE4D9"
            meta:resourcekey="btnPestanyaSolicitudesResource1" Width="90px" Font-Bold="True" 
                    Height="20px" />
               
        </div>

        <div id="divPestaniaVisitas" style="position:absolute; top: 0px; left: 89px; width:129px; height:23px;width:100px" >
       <%-- <asp:Panel ID="btnPestanyaVisitas" runat="server" BackImageUrl="../Imagenes/fondo.png" 
                        Height="154px" Width="19px" ToolTip="AVERIAS"
                        onMouseOver ="this.style.backgroundImage='url(../Imagenes/Ffondo.gif)'"
                        onMouseOut ="this.style.backgroundImage='url(../Imagenes/fondo.png)'"
                        style="cursor:hand" 
                >--%>
                    <asp:ImageButton ID="btnPestanyaVisitas" runat="server" 
                    OnClientClick="VerVisitas(); return false;" 
                    ImageUrl="../UI/HTML/Images/VisitasDesactivado_HORIZONTAL.jpg" 
                    
                AlternateText="VISITAS" meta:resourcekey="btnPestanyaVisitasResource1" />
                        <%--</asp:Panel>--%>


        </div>
     </div>
    <input id="hdnIdSolicitud" type="hidden" runat="server"/>
    <input id="hdnIdSolicitudHistorico" type="hidden" runat="server"/>

    <!-- Div de Solicitudes -->    
    <div id="divBtnExcel" style="position:absolute;height:3px; width:20px; top: 107px; left: 260px;">
    <asp:Button ID="btnExportarExcelSolicitudes" runat="server" Text="Exportar a Excel" 
            CssClass="botonFormulario" onclick="btnExportarExcelSolicitudes_Click" 
            meta:resourcekey="btnExportarExcelSolicitudesResource1"/>
          <%--  <asp:ImageButton ID="btnExportarExcelSolicitudes" runat="server" 
                  
                 style="z-index:12px;border:solid 1px #000000" CssClass="botonFormulario"
                 ToolTip="Exportar a Excel las solicitudes" AlternateText="Exportar a Excel" 
                 Height="18px" Width="21px" enabled="true" OnClientClick="MostrarCapaEspera();return true;"
                onclick="btnExportarExcelSolicitudes_Click" />--%>
         </div>
    <div id="div3" style="position:absolute;height:3px; width:26px; top: 92px; left: 584px;">
    <asp:Button ID="btnExportarExcelPreciosCalderas" Width="150px" runat="server" 
            Text="Exportar GAS CONFORT" BackColor="#FF6600"  CssClass="botonFormulario" 
            Visible="False" onclick="btnExportarExcelPreciosCalderas_Click" 
            meta:resourcekey="btnExportarExcelPreciosCalderasResource1"/>
          <%--  <asp:ImageButton ID="btnExportarExcelSolicitudes" runat="server" 
                  
                 style="z-index:12px;border:solid 1px #000000" CssClass="botonFormulario"
                 ToolTip="Exportar a Excel las solicitudes" AlternateText="Exportar a Excel" 
                 Height="18px" Width="21px" enabled="true" OnClientClick="MostrarCapaEspera();return true;"
                onclick="btnExportarExcelSolicitudes_Click" />--%>
         </div>    
    <div id="divSolicitudes" 
                    style="visibility:visible;width: 973px; position:absolute; top: 133px; left: 0px; height: 399px;border:solid 1px #000000">
        <div style="position:absolute;overflow:scroll;top:5px;left:5px;height:220px;width:962px;border:solid 1px #000000" class="panelTabla">
                <asp:GridView ID="gvSolicitudes" runat="server" AutoGenerateColumns="False"
                            Width="925px"  CssClass="contenidoTabla" 
                    onrowdatabound="gvSolicitudes_RowDataBound" 
                    onselectedindexchanged="gvSolicitudes_SelectedIndexChanged" 
                    EnableModelValidation="True" meta:resourcekey="gvSolicitudesResource1"
                    >
                    <RowStyle CssClass="filaNormal" BorderStyle="Solid" BorderWidth="1"  />
                    <AlternatingRowStyle CssClass="filaAlterna" BorderStyle="Solid" BorderWidth="1"/>
                    <FooterStyle CssClass="cabeceraTabla" />
                    <PagerStyle  CssClass="cabeceraTabla" />
                    <HeaderStyle CssClass="cabeceraTabla" />     
                   
                     <Columns>
                                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/flecha.gif" 
                                                        ShowSelectButton="True" meta:resourcekey="CommandFieldResource2" />
                                                  <asp:BoundField DataField="ID_solicitud" HeaderText="COD. Solicitud"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource8">
                                                   <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="Cod_contrato" HeaderText="Contrato"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource9">
                                                   <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="tipo_solicitud" HeaderText="Tipo"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource10">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="subtipo_solicitud" 
                                                        HeaderText="subTipo"  ReadOnly="True" meta:resourcekey="BoundFieldResource11">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="estado_solicitud" 
                                                        HeaderText="estado_solicitud"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource12">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="Cod_contrato" 
                                                        HeaderText="Código contrato"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource13">
                                                   <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                   <asp:BoundField DataField="Des_Tipo_solicitud" HeaderText="Tipo"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource14">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  
                                                  
                                                  <asp:BoundField DataField="DESCRIPCION" HeaderText="Subtipo"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource15">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="desSolicitud" HeaderText="Estado"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource16">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  
                                                  
                                                  <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha creación"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource17">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="telefono_contacto" HeaderText="Teléfono contacto"  
                                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource18">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="Persona_contacto" HeaderText="Persona contacto"  
                                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource19">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="Des_averia" HeaderText="Motivo Avería"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource20">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="NOM_PROVINCIA" HeaderText="Provincia" 
                                                        meta:resourcekey="BoundFieldResource21" />
                                                  
                                                  <asp:TemplateField HeaderText="Observaciones" 
                                                        meta:resourcekey="TemplateFieldResource1">
                                                         <ItemTemplate>   
                                                            <asp:Label ID="lb_Observaciones" runat="server" 
                                                                 ToolTip='<%# Eval("Observaciones") %>' 
                                                                 Text='<%# Iberdrola.Commons.Utils.StringUtils.ReduceText(Eval("Observaciones")) %>'></asp:Label>  
                                                         </ItemTemplate>
                                                  </asp:TemplateField> 

                                                    <asp:CheckBoxField DataField="Urgente" HeaderText="URGENTE" 
                                                        meta:resourcekey="CheckBoxFieldResource1" />
                                                    <asp:BoundField DataField="Proveedor" HeaderText="PROVEEDOR" 
                                                        meta:resourcekey="BoundFieldResource22" />
                                                  
                                                    <%--<asp:BoundField DataField="Fec_visita" HeaderText="Fecha Visita Realizada" ReadOnly="True">
                                                      <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    
                                                    <%--<asp:TemplateField HeaderText="CUPS">
                                                         <ItemTemplate>   
                                                            <asp:Label ID="lb_CUPS" runat="server" ToolTip='<%# Eval("CUPS")%>' Text='<%# acorta(Eval("CUPS")) %>'></asp:Label>  
                                                         </ItemTemplate>
                                                  </asp:TemplateField>--%> 
                                                  
                                                  <asp:BoundField DataField="COD_PROVEEDOR" 
                                                        HeaderText="PROVEEDOR ULTIMO MOVIMIENTO"  ReadOnly="True" 
                                                        DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource23">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="EFV" HeaderText="EFV"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource24">
                                                      <HeaderStyle HorizontalAlign="Left" />
                                                      </asp:BoundField>
                                                  <asp:BoundField DataField="SCORING" HeaderText="SCORING"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource25">
                                                      <HeaderStyle HorizontalAlign="Left" />
                                                      </asp:BoundField>
                                                    <asp:BoundField DataField="facturarInspeccion" HeaderText="Facturar Inspeccion"  ReadOnly="True" >
                                                      <HeaderStyle HorizontalAlign="Left" />
                                                      </asp:BoundField>
                                                  <%--<asp:BoundField DataField="id_perfil" HeaderText="Perfil" />--%>
                                                </Columns>                
                   
                </asp:GridView>
         </div>
         
         <div style="position:absolute;overflow:scroll;top:255px;left:5px;height:140px;width:495px;border:solid 1px #000000" class="style32">
                <asp:GridView ID="gv_HistoricoSolicitudes" runat="server" BackColor="White" 
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                AutoGenerateColumns="False" Width="600px" CssClass="contenidoTabla"
                    onrowdatabound="gv_HistoricoSolicitudes_RowDataBound" 
                    EnableModelValidation="True" 
                    meta:resourcekey="gv_HistoricoSolicitudesResource1">
                                <RowStyle ForeColor="#000066" BorderColor ="Transparent" />
                                <Columns>
                                  <asp:BoundField DataField="id_movimiento" HeaderText="id_movimiento"  
                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource26">
                                      <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                   <asp:BoundField DataField="tipo_movimiento" HeaderText="tipo_movimiento"  
                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource27">
                                       <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="id_solicitud" HeaderText="Solicitud"  
                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource28">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/flecha.gif" 
                                        ShowSelectButton="True" meta:resourcekey="CommandFieldResource3" />
                                  <asp:BoundField DataField="estado_solicitud" HeaderText="Estado"  ReadOnly="True" 
                                        meta:resourcekey="BoundFieldResource29">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                 <asp:BoundField DataField="usuario" HeaderText="Usuario"  ReadOnly="True" 
                                        meta:resourcekey="BoundFieldResource30">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                    <asp:BoundField DataField="observaciones" HeaderText="Observaciones"  
                                        ReadOnly="True" meta:resourcekey="BoundFieldResource31">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                    <asp:BoundField DataField="time_stamp" HeaderText="Fecha / Hora" 
                                        meta:resourcekey="BoundFieldResource32" />
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" BorderColor ="Transparent" />
                            </asp:GridView>
        </div>
        
        <div style="position:absolute;overflow:scroll;top:255px;left:507px;height:140px;width:460px;border:solid 1px #000000" class="panelTabla">
            <asp:GridView ID="gv_historicoCaracteristicas" runat="server" BackColor="White" 
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                AutoGenerateColumns="False" Width="400px" 
                CssClass="contenidoTabla" EnableModelValidation="True" 
                meta:resourcekey="gv_historicoCaracteristicasResource1">
                                <RowStyle ForeColor="#000066" BorderColor ="Transparent" />
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" BorderColor ="Transparent" />
                                <Columns>
                                  <asp:BoundField DataField="id_caracteristica" HeaderText="id_caracteristica"  
                                        ReadOnly="True" meta:resourcekey="BoundFieldResource33">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                   <asp:BoundField DataField="id_movimiento" HeaderText="id_movimiento"  
                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource34">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="tipo_campo" HeaderText="Tipo campo"  ReadOnly="True" 
                                        meta:resourcekey="BoundFieldResource35">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                 <asp:BoundField DataField="valor" HeaderText="Valor"  ReadOnly="True" 
                                        meta:resourcekey="BoundFieldResource36">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                </Columns>
                                
                            </asp:GridView>
        </div>
    </div>
    
    <!-- Div de Visitas -->    
    <div id="divVisitas" 
       style="visibility:hidden; width: 973px; position:absolute; top: 133px; left: 0px; height: 399px; border:solid 1px #000000;z-index:2">
        <div style="position:absolute;overflow:scroll;top:5px;left:5px;height:383px;width:599px;border:solid 1px #000000" class="panelTabla">
                <asp:GridView ID="gvVisitas" runat="server" BackColor="White" 
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    AutoGenerateColumns="False" Width="843px" Height="85px" 
                    onrowdatabound="gvVisitas_RowDataBound" CssClass="contenidoTabla" 
                    EnableModelValidation="True" meta:resourcekey="gvVisitasResource1">
                    <RowStyle ForeColor="#000066" BorderColor ="Transparent" />
                    
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/flecha.gif" 
                            Text="Botón" meta:resourcekey="ButtonFieldResource1" />
                       <asp:BoundField DataField="COD_VISITA" HeaderText="Nº Visita"  ReadOnly="True" 
                            meta:resourcekey="BoundFieldResource37">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="COD_CONTRATO" HeaderText="Contrato"  ReadOnly="True" 
                            meta:resourcekey="BoundFieldResource38">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                     <asp:BoundField DataField="DES_ESTADO" HeaderText="Estado"  ReadOnly="True" 
                            meta:resourcekey="BoundFieldResource39">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="FEC_VISITA" HeaderText="Fecha de visita"  
                            ReadOnly="True" DataFormatString="{0:dd/MM/yyyy}" 
                            meta:resourcekey="BoundFieldResource40">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="FEC_LIMITE_VISITA" 
                            HeaderText="Fecha limite de visita"  ReadOnly="True" DataFormatString="{0:d}" 
                            meta:resourcekey="BoundFieldResource41">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="COD_PROVEEDOR" 
                            HeaderText="PROVEEDOR ULTIMO MOVIMIENTO"  ReadOnly="True" 
                            DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource42">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="DESC_CATEGORIA_VISITA" HeaderText="CATEGORIA VISITA"  
                            ReadOnly="True" DataFormatString="{0:d}" 
                            meta:resourcekey="BoundFieldResource43">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                        <asp:BoundField DataField="PRL" HeaderText="PRL" ReadOnly="True" meta:resourcekey="BoundFieldResource44">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" BorderColor ="Transparent"/>
                    
                    
                    
                </asp:GridView>
         </div>
         <div id="divDetalleVisita" style="width: 355px; position:absolute; top: 5px; left: 610px; height: 383px; border:solid 1px #000000">
            <table width="100%">
                <tr>
                    <td > 
                        <asp:Label ID="lblTituloDetalleVisita" runat="server" CssClass="divSubTituloVentana" 
                            Text="3" meta:resourcekey="lblTituloDetalleVisitaResource1"></asp:Label>
                        <asp:Label ID="lblNumeroVisita" runat="server" CssClass="labelFormularioValor" 
                            Text="3" meta:resourcekey="lblNumeroVisitaResource1"></asp:Label>
                    </td>
                    <td align="right" colspan="2">
                        <%--<asp:BoundField DataField="Fec_visita" HeaderText="Fecha Visita Realizada" ReadOnly="True">
                                                      <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                        <asp:Button ID="btnDetalleVisita" runat="server" Text="Detalle..." 
                            ForeColor="White" BackColor="Green" Width="97px" Height="32px" BorderWidth="1px" 
                            BorderColor="#262626" ToolTip="Detalle visita" 
                            onclick="btnDetalleVisita_Click" CssClass="botonFormulario" 
                            meta:resourcekey="btnDetalleVisitaResource1" />
                        <asp:ImageButton ID="btnAvisoVisita" runat="server" BackColor="Green" 
                            ImageUrl="./HTML/Images/aviso.png" Width="27px" Height="27px"  
                            BorderWidth="1px" BorderColor="#262626" ToolTip="Aviso" 
                            OnClientClick="MostrarAvisos();return false;" 
                            meta:resourcekey="btnAvisoVisitaResource1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label42" runat="server" CssClass="labelFormulario" 
                            Text="Estado:" meta:resourcekey="Label42Resource1"></asp:Label>                
                    </td>
                    <td >
                        <asp:Label ID="lblEstadoVisita" runat="server" CssClass="labelFormularioValor" 
                            Text="Pendiente" meta:resourcekey="lblEstadoVisitaResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label24" runat="server" CssClass="labelFormulario" Text="C.B.:" 
                            meta:resourcekey="Label24Resource1"></asp:Label>                
                    </td>
                    <td >
                        <asp:Label ID="lblCBVisita" runat="server" CssClass="labelFormularioValor" 
                            Text="0" meta:resourcekey="lblCBVisitaResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label44" runat="server" CssClass="labelFormulario" 
                            Text=" Tiene Reparación:" meta:resourcekey="Label44Resource1"></asp:Label>                
                    </td>
                    <td >
                        <asp:Label ID="lblTieneReparacionVisita" runat="server" 
                            CssClass="labelFormularioValor" Text="Sí" 
                            meta:resourcekey="lblTieneReparacionVisitaResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label46" runat="server" CssClass="labelFormulario" 
                            Text="Tipo Visita:" meta:resourcekey="Label46Resource1"></asp:Label>                
                    </td>
                    <td >
                        <asp:Label ID="lblTipoVisita" runat="server" 
                            CssClass="labelFormularioValor" Text="Descripción tipo reparación" 
                            meta:resourcekey="lblTipoVisitaResource1"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td>
                        <asp:Label ID="Label48" runat="server" CssClass="labelFormulario" 
                            Text="Fec. Visita:" meta:resourcekey="Label48Resource1"></asp:Label>                
                    </td>
                    <td>
                        <asp:Label ID="lblFechaVisita" runat="server" CssClass="labelFormularioValor" 
                            Text="02/12/2012" meta:resourcekey="lblFechaVisitaResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label50" runat="server" CssClass="labelFormulario" 
                            Text="Fec. Límite Visita:" meta:resourcekey="Label50Resource1"></asp:Label>                
                    </td>
                    <td>
                        <asp:Label ID="lblFechaLimiteVisita" runat="server" 
                            CssClass="labelFormularioValor" Text="01/12/2012" 
                            meta:resourcekey="lblFechaLimiteVisitaResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label52" runat="server" CssClass="labelFormulario" 
                            Text=" Tipo Caldera:" meta:resourcekey="Label52Resource1"></asp:Label>                
                    </td>
                    <td>
                        <asp:Label ID="lblTipoCaldera" runat="server" 
                            CssClass="labelFormularioValor" Text="2 horas" 
                            meta:resourcekey="lblTipoCalderaResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label54" runat="server" CssClass="labelFormulario" 
                            Text="Marca Caldera:" meta:resourcekey="Label54Resource1"></asp:Label>                
                    </td>
                    <td>
                        <asp:Label ID="lblMarcaCaldera" runat="server" 
                            CssClass="labelFormularioValor" Text="32.5" 
                            meta:resourcekey="lblMarcaCalderaResource1"></asp:Label><span class="labelFormulario"></span> 
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label56" runat="server" CssClass="labelFormulario" 
                            Text="Modelo Caldera:" meta:resourcekey="Label56Resource1"></asp:Label>                
                    </td>
                    <td>
                        <asp:Label ID="lblModeloCaldera" runat="server" 
                            CssClass="labelFormularioValor" Text="75,8" 
                            meta:resourcekey="lblModeloCalderaResource1"></asp:Label><span class="labelFormulario"></span> 
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label58" runat="server" CssClass="labelFormulario" 
                            Text=" Potencia:" meta:resourcekey="Label58Resource1"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPotencia" runat="server" CssClass="labelFormularioValor" 
                            Text="332.5" meta:resourcekey="lblPotenciaResource1"></asp:Label><span class="labelFormulario"></span> 
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label36" runat="server" CssClass="labelFormulario" 
                            Text=" Técnico:" meta:resourcekey="Label36Resource1"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTecnico" runat="server" CssClass="labelFormularioValor" 
                            Text="332.5" meta:resourcekey="lblTecnicoResource1"></asp:Label><span class="labelFormulario"></span> 
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPRL" runat="server" CssClass="labelFormulario" 
                            Text="PRL:" meta:resourcekey="lblPRLResource1"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblValorPRL" runat="server" CssClass="labelFormularioValor" 
                            Text="" meta:resourcekey="lblValorPRLResource1"></asp:Label><span class="labelFormulario"></span> 
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label60" runat="server" CssClass="labelFormulario" 
                            Text="Observaciones:" meta:resourcekey="Label60Resource1"></asp:Label>                
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblObservacionesVisita" runat="server" 
                            CssClass="labelFormularioValor" Text="Observaciones de la visita ..." 
                            meta:resourcekey="lblObservacionesVisitaResource1"></asp:Label>
                    </td>
                </tr>                
            </table>
         </div>
    </div>
    <!-- Div de la solicitud -->    
    <div id="divEditar" style="visibility:visible; z-index:13; background-color:white; background-image	: url(../imagenes/FONDO_SOL.jpg); background-position:center; background-repeat:no-repeat;
	  width: 978px; position:absolute; top: 92px; left: -1px; height: 420px; border:green 2px solid; ">
      <asp:Label ID="lblEstadoVisitaActual" runat="server" Text="Estado visita:" CssClass="labelFormulario" Font-Bold="true" forecolor="Red" meta:resourcekey="Label16Resource1"></asp:Label>
	  <div style="position:absolute; top: 26px; left: 37px;">
          <asp:Label ID="lblProceso" runat="server" Font-Bold="True" 
              Font-Names="Arial Black" Font-Size="8.05pt" ForeColor="White" 
              meta:resourcekey="lblProcesoResource1"></asp:Label></div>
        <div id="divCamposAlta" style="position:absolute; top: 70px; left: 19px;">
            <table width="90%" cellpadding="0" cellspacing="1"> 
                <tr style="height:40px">
                    <td>
                            

                    <asp:Label ID="lblEditarSubtipo" runat="server" Text="Subtipo:" 
                            CssClass="labelFormulario" meta:resourcekey="lblEditarSubtipoResource1"></asp:Label></td>
                    <td>
                    <asp:DropDownList ID="ddlSubtipo" BackColor="White" runat="server" 
                            CssClass="campoFormulario" AutoPostBack="True" Width="198px" 
                            onselectedindexchanged="ddlSubtipo_SelectedIndexChanged" 
                            meta:resourcekey="ddlSubtipoResource1"></asp:DropDownList>
                    </td>
                    <td><asp:Label ID="lblEditarEstado" runat="server" Text="Estado:" 
                            CssClass="labelFormulario" meta:resourcekey="lblEditarEstadoResource1"></asp:Label></td>
                    <td >
                    <asp:DropDownList ID="ddlEstado" BackColor="White" runat="server" 
                            CssClass="campoFormulario" AutoPostBack="True" Width="200px" 
                            onselectedindexchanged="ddlEstado_SelectedIndexChanged" 
                            meta:resourcekey="ddlEstadoResource1"></asp:DropDownList>
                    </td>
                    <td><asp:Label ID="Label51" runat="server" Text="Cancelación:" 
                            CssClass="labelFormulario" meta:resourcekey="Label51Resource1"></asp:Label></td>
                    <td >
                    <asp:DropDownList ID="ddlMotivoCancelacion" BackColor="White" runat="server" 
                            CssClass="campoFormulario" Width="218px" Enabled="False" 
                            meta:resourcekey="ddlMotivoCancelacionResource1"></asp:DropDownList>
                    </td>
                </tr>
                <tr style="height:40px">
                    <td><asp:Label ID="lbl" runat="server" Text="Teléfono:" CssClass="labelFormulario" 
                            meta:resourcekey="lblResource1"></asp:Label></td>
                    <td>
                    <asp:TextBox ID="txtEditarTelefono" MaxLength="18" BackColor="White" 
                            CssClass="campoFormulario" runat="server" Width="150px" 
                            meta:resourcekey="txtEditarTelefonoResource1"></asp:TextBox>
                    </td>
                    <td colspan="2"><asp:Label ID="Label5" runat="server" Text="Contacto:" 
                            CssClass="labelFormulario" meta:resourcekey="Label5Resource1"></asp:Label>
                    <asp:TextBox ID="txtPersonaContacto" BackColor="White" CssClass="campoFormulario" 
                            runat="server" Width="177px" 
                            meta:resourcekey="txtPersonaContactoResource1"></asp:TextBox>
                    </td>
                    <td><asp:Label ID="Label9" runat="server" Text="Horario:" 
                            CssClass="labelFormulario" meta:resourcekey="Label9Resource1"></asp:Label></td>
                    <td>
                    <asp:DropDownList ID="cmbHorarioContacto" BackColor="White" runat="server" 
                            CssClass="campoFormulario" Width="198px" 
                            meta:resourcekey="cmbHorarioContactoResource1">
                    <asp:ListItem Text="MAÑANA" Value="MAÑANA" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                            <asp:ListItem Text="TARDE" Value="TARDE" 
                            meta:resourcekey="ListItemResource2"></asp:ListItem>
                                            <asp:ListItem Text="NOCHE" Value="NOCHE" 
                            meta:resourcekey="ListItemResource3"></asp:ListItem>
                                            <asp:ListItem Text="CUALQUIERA" Value="CUALQUIERA" 
                            meta:resourcekey="ListItemResource4"></asp:ListItem>
                                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="height:40px">
                    <td><asp:Label ID="lblDescrAveria" ForeColor="Firebrick" runat="server" 
                            Text="Avería:" CssClass="labelFormulario" 
                            meta:resourcekey="lblDescrAveriaResource1"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="ddlAveria" BackColor="White" runat="server" 
                            CssClass="campoFormulario" Width="288px" 
                            OnSelectedIndexChanged="ddlAveria_SelectedIndexChanged" AutoPostBack="True" meta:resourcekey="ddlAveriaResource1"
                            ></asp:DropDownList>
                        <asp:DropDownList ID="ddlVisita" BackColor="White" runat="server" 
                            CssClass="campoFormulario" Width="288px" 
                            OnSelectedIndexChanged="ddlVisita_SelectedIndexChanged" AutoPostBack="True"  
                            Visible="False" meta:resourcekey="ddlVisitaResource1" ></asp:DropDownList></td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkUrgente" runat="server" Enabled="False" Text=" URGENTE" 
                            ForeColor="Firebrick" Font-Bold="True" meta:resourcekey="chkUrgenteResource1" />
                    </td>
                    <td><asp:Label ID="lblProveedorEditar" runat="server" Text="Proveedor:" 
                            CssClass="labelFormulario" meta:resourcekey="lblProveedorEditarResource1"></asp:Label></td></td>
                    <td><asp:TextBox ID="txtProveedor" Enabled="False" BackColor="White" runat="server" 
                            meta:resourcekey="txtProveedorResource1"></asp:TextBox>
                    <asp:DropDownList ID="cmbProveedor" runat="server" Visible="False" 
                            meta:resourcekey="cmbProveedorResource1"></asp:DropDownList></td>
                </tr>
                <tr style="height:40px">
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="Observaciones Anteriores:" 
                            CssClass="labelFormulario" meta:resourcekey="Label11Resource1"></asp:Label></td>
                    <td colspan="5">
                        <asp:TextBox ID="txtEditarObservacionesAnteriores" Enabled="False" 
                            BackColor="White" TextMode="MultiLine" runat="server" Rows="3" Columns="105" 
                            meta:resourcekey="txtEditarObservacionesAnterioresResource1"></asp:TextBox>
                    </td>                
                </tr>
                <tr style="height:40px">
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="Nuevas Observaciones:" 
                            CssClass="labelFormulario" meta:resourcekey="Label13Resource1"></asp:Label></td>
                    <td colspan="5">
                        <asp:TextBox ID="txtEditarObservaciones" BackColor="White" TextMode="MultiLine" 
                            runat="server" Rows="2" Columns="105" 
                            meta:resourcekey="txtEditarObservacionesResource1"></asp:TextBox>
                    </td>                
                </tr>
                <tr style="height:40px">
                    <td><asp:Label ID="Label15" runat="server" Text="Marca Caldera:" 
                            CssClass="labelFormulario" meta:resourcekey="Label15Resource1"></asp:Label></td>
                    <td>
                    <asp:TextBox ID="txtMarcaCaldera" Enabled="False" BackColor="White" runat="server" 
                            meta:resourcekey="txtMarcaCalderaResource1"></asp:TextBox>
                    </td>
                    <td><asp:Label ID="Label16" runat="server" Text="Modelo:" 
                            CssClass="labelFormulario" meta:resourcekey="Label16Resource1"></asp:Label></td>
                    <td>
                    <asp:TextBox ID="txtModelocaldera" BackColor="White" runat="server" 
                            meta:resourcekey="txtModelocalderaResource1"></asp:TextBox>
                            
                    </td>
                    <td><asp:Label ID="lblRetencion" runat="server" Text="Prioritario Retención:" 
                            CssClass="labelFormulario" Font-Bold="True" ForeColor="#990000" 
                            meta:resourcekey="lblRetencionResource1"></asp:Label></td>
                    <td>
                    <!--Retencion -->
                        <asp:CheckBox ID="ChkRetencion" runat="server" 
                            Text=" " ForeColor="Firebrick" Font-Bold="True" 
                            Font-Size="XX-Small" ToolTip="Prioritario Retención" 
                            meta:resourcekey="ChkRetencionResource1" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divBotonesEditar" style="position:absolute; top: 355px; left: 390px;">
            <asp:Button ID="btnAceptarEditarSolicitud" runat="server"  Text="Aceptar" 
                CssClass="botonFormulario" onclick="btnAceptarEditarSolicitud_Click" 
                meta:resourcekey="btnAceptarEditarSolicitudResource1" />
            <asp:Button ID="btnCancelarEditarSolicitud" 
                OnClientClick="VerSolicitudes(); return false;" runat="server"  Text="Cancelar" 
                CssClass="botonFormulario" 
                meta:resourcekey="btnCancelarEditarSolicitudResource1" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /><asp:Label ID="lblMensajeEditar" 
                runat="server" ForeColor="Red" CssClass="labelFormulario" 
                meta:resourcekey="lblMensajeEditarResource1"></asp:Label>
        </div>
        <div id="div1" style="position:absolute; top:28px;left:780px"> 
        <!-- BorderWidth="1" BorderColor="#262626" BackColor="#34721f"-->
            <%--<asp:TemplateField HeaderText="CUPS">
                                                         <ItemTemplate>   
                                                            <asp:Label ID="lb_CUPS" runat="server" ToolTip='<%# Eval("CUPS")%>' Text='<%# acorta(Eval("CUPS")) %>'></asp:Label>  
                                                         </ItemTemplate>
                                                  </asp:TemplateField>--%>
            <asp:ImageButton ID="btnAviso" BackColor="Green" 
                OnClientClick="MostrarAvisoSolicitud(); return false;" runat="server" 
                ImageUrl="./HTML/Images/aviso.png" Width="27px" Height="27px"  
                BorderWidth="1px" BorderColor="#262626" ToolTip="Enviar Aviso" 
                meta:resourcekey="btnAvisoResource1" />
            <asp:ImageButton ID="btnLLAMADA" BackColor="Green" runat="server" 
                ImageUrl="./HTML/Images/phone1.png" Width="27px" Height="27px" 
                BorderWidth="1px" BorderColor="#262626" ToolTip="Nueva Llamada Cliente" 
                meta:resourcekey="btnLLAMADAResource1" />
            
            &nbsp;
            <asp:Image ID="imgLlamadaCortesia" onClick="" 
                onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'" 
                AlternateText="Generada tras llamada de cortesía" 
                ImageUrl="./HTML/Images/smart_phone_warning.png" runat="server" Height="27px" 
                Width="27px" meta:resourcekey="imgLlamadaCortesiaResource1" />        
            <asp:Image onClick="javascript:Twitter();" ID="imgTwitter" 
                onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'" 
                AlternateText="SOLICITUD VIA TWITTER" ImageUrl="../Imagenes/twitter_Verde.png" runat="server" 
                Height="27px" Width="27px" meta:resourcekey="imgTwitterResource1" />
            <asp:Image onClick="javascript:Facebook();" ID="imgFacebook" 
                onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'" 
                AlternateText="SOLICITUD VIA FACEBOOK" 
                ImageUrl="../Imagenes/facebook_Verde.png" runat="server" 
                Height="27px" Width="27px" meta:resourcekey="imgFacebookResource1" />
            <asp:CheckBox ID="chkTwitter" style="visibility:hidden" runat="server" 
                meta:resourcekey="chkTwitterResource1" />
            <asp:CheckBox ID="chkFacebook" style="visibility:hidden" runat="server" 
                meta:resourcekey="chkFacebookResource1" />
                
            
        </div>
        <div id="div2" style="position:absolute; top:59px; left:830px">
            <asp:Label ID="lblTwitter" style="visibility:hidden" runat="server" 
                CssClass="labelFormulario" Text="SOLICITUD VIA TWITTER" ForeColor="Blue" 
                Font-Bold="True" meta:resourcekey="lblTwitterResource1" ></asp:Label>
        <asp:Label ID="lblFacebook" style="visibility:hidden" runat="server" 
                CssClass="labelFormulario" Text="SOLICITUD VIA FACEBOOK" ForeColor="Blue" 
                Font-Bold="True" meta:resourcekey="lblFacebookResource1" ></asp:Label></div>
    </div>
    
    </div>
<div style="position:absolute;top:123px; left:200px; width:69px;">
    <asp:Button Enabled="False" BackColor="White" ForeColor="Orange" Width="69px" 
        ID="btnCobertura" CssClass="botonFormulario" 
        OnClientClick="MostrarCoberturaServicio();return false;" runat="server" 
        style="text-align:center;" Text="Coberturas" 
        meta:resourcekey="btnCoberturaResource1" /></div>

            <div class="VariosClientesCalderas" id="VariosClientesCalderas" >
             
                <asp:Label ID="Label47" ForeColor="White" Font-Bold="True" 
                    CssClass="labelFormulario" runat="server" 
                    Text="Se han encontrado, mas de una coincidencia con los criterios de búsqueda." 
                    meta:resourcekey="Label47Resource1"></asp:Label>
                
                <div style="position:absolute;overflow:scroll;top:20px;left:5px;height:470px;width:945px;border:solid 1px #000000" class="">
                        <asp:GridView ID="gridContratosCalderas" runat="server" Width="764px" 
                            CssClass="contenidoTabla" EnableModelValidation="True" 
                            meta:resourcekey="gridContratosCalderasResource1" >
                            <RowStyle CssClass="filaNormal" BorderColor ="Transparent" />
                            <AlternatingRowStyle CssClass="filaAlterna"/>
                            <FooterStyle CssClass="cabeceraTabla" />
                            <PagerStyle  CssClass="cabeceraTabla" />
                            <HeaderStyle CssClass="cabeceraTabla" BorderColor ="Transparent" />   
                              <Columns>
                                  <asp:TemplateField meta:resourcekey="TemplateFieldResource2">
                                      <ItemTemplate>
                                            <asp:LinkButton ID="LinkSelect"
                                            OnClientClick='<%# Eval("CONTRATO", "VolverAtras(\"{0}\");return false;") %>' 
                                                runat="server" meta:resourcekey="LinkSelectResource1">Seleccionar</asp:LinkButton>
                                            
                                      </ItemTemplate>
                                 </asp:TemplateField>
                              </Columns>
                        </asp:GridView>
                 </div>
 		</div>
 		</div>
 
    <script type="text/javascript">
        function AbrirVentana(idSolicitud, codContrato) {
            //            alert(idSolicitud);
            //            alert(codContrato);
            abrirVentanaLocalizacion("frmModalModificarSolicitud.aspx?idSolicitud=" + idSolicitud + "&Contrato=" + codContrato, 800, 580, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_DATOS_SOLICITUD%>" + idSolicitud + "; CONTRATO: " + codContrato, "0", "1");
        }
        function AbrirVentanaCalderas(idSolicitud, codContrato) {
            //                        alert(idSolicitud);
            //            alert(codContrato);
            abrirVentanaLocalizacion("frmModalModificarSolicitud.aspx?idSolicitud=" + idSolicitud + "&Calderas=1&Contrato=" + codContrato, 800, 550, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_DATOS_SOLICITUD%>" + idSolicitud + "; CONTRATO: " + codContrato, "0", "1", true);
        }


        function AbrirVentana_SolAveGasConfort(codContrato) {
            //            alert(idSolicitud);
            //            alert(codContrato);
            //alert(document.getElementById('ctl00_ContentPlaceHolderContenido_lblTelefonoContacto').innerText.substring(0, 9));
            abrirVentanaLocalizacion("frmModalModificarSolicitud.aspx?SubtipoAveriaSobreGC=" + document.getElementById('ctl00_ContentPlaceHolderContenido_hdnSubtipoAveriaSobreGC').value + "&idSolicitud=&Contrato=" + codContrato + "&Telefono=" + document.getElementById('ctl00_ContentPlaceHolderContenido_lblTelefonoContacto').innerText, 800, 580, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_ALTA_AVERIA_GAS_CONFORT_TITULO%>" + codContrato, "0", "1");

        }
        function VerSolicitudes() {
            //alert("1");
            var btnVisitas = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPestanyaVisitas");
            var btnSolicitudes = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPestanyaSolicitudes");
            var divVisitas = document.getElementById("divVisitas");
            var divSolicitudes = document.getElementById("divSolicitudes");
            var divEditar = document.getElementById("divEditar");
            var divContador = document.getElementById("divContador");
            var divBotonNuevo = document.getElementById("divBotonNuevo");
            var divBtnExcel = document.getElementById("divBtnExcel");
            var divUltimoMov = document.getElementById("ctl00_ContentPlaceHolderContenido_divUltimoMovimiento");
            //var lblProceso = document.getElementById("ctl00_ContentPlaceHolderContenido_lblProceso");
            //lblProceso.innerText = '';
            //alert("111");

            divContador.style.visibility = "visible";
            divEditar.style.visibility = "hidden";
            divBotonNuevo.style.visibility = "visible";
            divBtnExcel.style.visibility = "visible";
            if (divUltimoMov != null) { divUltimoMov.style.visibility = "visible"; }
            //alert("1");
            btnVisitas.disabled = false;
            btnVisitas.src = "./HTML/Images/VisitasDesactivado_HORIZONTAL.jpg";
            divVisitas.style.visibility = "hidden";

            btnSolicitudes.disabled = true;
            btnSolicitudes.style.backgroundColor = "#DCE4D9";
           // btnSolicitudes.src = "./HTML/Images/SolicitudesActivo_HORIZONTAL.jpg";
            divSolicitudes.style.visibility = "visible";

            var hiddenPanelSeleccionado = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnPanelSeleccionado");
            hiddenPanelSeleccionado.value = "SOLICITUDES";
        }
    </script>
    
<script type="text/javascript">
    window.history.forward(1);

    $(function () {
        /********* BLOQUE DE CODIGO QUE PERMITE MOSTRAR EL DIV OCULTO ************/

        $("#S_explode").click(function () {
            $(".historic").show("explode", { pieces: 8 }, 900);
            return false;
        });
        $("#H_explode").click(function () {
            $(".historic").hide("explode", { pieces: 8 }, 10);

            return false;
        });
    });
</script>  

<script type="text/javascript">
    
   
   
    function AltaGasConfort() {
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText == '') {
            alert('<%=Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_CONTRATO%>');
            return false;
        }
        else {
            if (confirm("<%=Resources.TextosJavaScript.TEXTO_ALTA_GAS_CONFORT%>")) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    function AltaAveriaGasConfort() {
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText == '') {
            alert('<%=Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_CONTRATO%>');
            return false;
        }
        else {
            if (confirm('<%=Resources.TextosJavaScript.TEXTO_ALTA_AVERIA_GAS_CONFORT%>')) {
                return true;
            }
            else {
                return false;
            }
        }
    }
   

   
    function Salir(Id) {
        var hiddenAviso = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnAviso");
        hiddenAviso.value = Id;
    }


   

    function MostrarDatos(Dato, Id) {
        var lblEncontrados = document.getElementById("ctl00_ContentPlaceHolderContenido_lblNumEncontrados");
        //alert('1');
        var hiddenAviso = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnAviso");
        if (Id != hiddenAviso.value || lblEncontrados.innerText == "1") {
            //alert('2');
            $.jGrowl("close");
            $.jGrowl("close");
            $.jGrowl("close");
            $.jGrowl("<b><u><center>- AVISO -</center></u></b>" + Dato);
        }

        hiddenAviso.value = Dato;
    }

    function OnBtnBuscar_Click() {
        abrirVentanaLocalizacion("./FrmModalSeleccionarContrato.aspx", "800", "500", "SeleccionarContrato", "<%=Resources.TextosJavaScript.TEXTO_SELECCIONAR_CONTRATO%>", "", false);
    }

    
    //    // Muestra el panel de solicitudes
    //    function VerSolicitudes() {
    //        alert("1");
    //        var btnVisitas = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPestanyaVisitas");
    //        var btnSolicitudes = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPestanyaSolicitudes");
    //        var divVisitas = document.getElementById("divVisitas");
    //        var divSolicitudes = document.getElementById("divSolicitudes");
    //        var divEditar = document.getElementById("divEditar");
    //        var divContador = document.getElementById("divContador");
    //        var divBotonNuevo = document.getElementById("divBotonNuevo");
    //        var divBtnExcel = document.getElementById("divBtnExcel");
    //        var divUltimoMov = document.getElementById("ctl00_ContentPlaceHolderContenido_divUltimoMovimiento");
    //        //var lblProceso = document.getElementById("ctl00_ContentPlaceHolderContenido_lblProceso");
    //        //lblProceso.innerText = '';
    //        alert("111");

    //        divContador.style.visibility = "visible";
    //        divEditar.style.visibility = "hidden";
    //        divBotonNuevo.style.visibility = "visible";
    //        divBtnExcel.style.visibility = "visible";
    //        if (divUltimoMov != null) { divUltimoMov.style.visibility = "visible"; }
    //        alert("1");
    //        btnVisitas.disabled = false;
    //        btnVisitas.src = "./HTML/Images/VisitasDesactivado_HORIZONTAL.jpg";
    //        divVisitas.style.visibility = "hidden";

    //        btnSolicitudes.disabled = true;
    //        btnSolicitudes.src = "./HTML/Images/SolicitudesActivo_HORIZONTAL.jpg";
    //        divSolicitudes.style.visibility = "visible";

    //        var hiddenPanelSeleccionado = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnPanelSeleccionado");
    //        hiddenPanelSeleccionado.value = "SOLICITUDES";
    //    }

    // Muestra el panel de editar
 


    

    //    function CargarDatos(Valor) {
    //        //abrirVentanaEspera("frmEspera.aspx", 620, 400, "ventana-modal","","1","1");
    //        //abrirVentanaAlerta(620, 400, "ventana-modal","CARGANDO DATOS...","1","1");
    //        MostrarCapaEspera();

    //        //window.form1.style.visibility ="hidden";

    //        var hiddenVisita = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnCodVisita");
    //        if (hiddenVisita != null) {
    //            //alert(hiddenVisita.value);
    //            hiddenVisita.value = "";
    //            //alert(hiddenVisita.value);
    //        }

    //        var hiddenSolicitudH = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnIdSolicitudHistorico");
    //        if (hiddenSolicitudH != null) {
    //            //alert(hiddenSolicitudH.value);
    //            hiddenSolicitudH.value = "";
    //            //alert(hiddenSolicitudH.value);
    //        }
    //        var hiddenSeleccionadoAlta = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnSeleccionadoAlta");
    //        hiddenSeleccionadoAlta.value = "0";

    //        var hiddenSolicitud = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnIdSolicitud");
    //        hiddenSolicitud.value = Valor;
    //        javascript: __doPostBack('gv_Solicitudes', Valor);

    //    }

   


    function ClickPaginacion(ident) {
        document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud").value = "";
        document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud").value = "";
        MostrarCapaEspera();
        var botonPaginacion = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPaginacion");
        var hiddenPaginacion = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnPageIndex");
        hiddenPaginacion.value = ident;
        botonPaginacion.click();
        return false;
    }

    function MostrarAvisos() {
        var Contrato = document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo");

        abrirVentanaLocalizacion("../Pages/AvisoVisita.aspx?Contrato=" + Contrato.innerText + "&CodVisita=" + document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodVisita").value, 600, 350, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_NUEVO_AVISO%>", "0", "1");
    }

    function getXY(obj) {
        var curleft = 0;
        var curtop = obj.offsetHeight + 5;
        var border;
        if (obj.offsetParent) {
            do {
                // XXX: If the element is position: relative we have to add borderWidth
                if (getStyle(obj, 'position') == 'relative') {
                    if (border = _pub.getStyle(obj, 'border-top-width')) curtop += parseInt(border);
                    if (border = _pub.getStyle(obj, 'border-left-width')) curleft += parseInt(border);
                }
                curleft += obj.offsetLeft;
                curtop += obj.offsetTop;
            }
            while (obj = obj.offsetParent)
        }
        else if (obj.x) {
            curleft += obj.x;
            curtop += obj.y;
        }
        //Alert('x:' + curleft + '_y:' + curtop);
        return curtop;
    }

    function getStyle(obj, styleProp) {
        if (obj.currentStyle)
            return obj.currentStyle[styleProp];
        else if (window.getComputedStyle)
            return document.defaultView.getComputedStyle(obj, null).getPropertyValue(styleProp);
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
   function ExportarExcel() {
        abrirVentanaLocalizacion("../UI/AbrirExcel.aspx", 600, 350, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_EXPORTAR_EXCEL%>", "0", "1");
   }
   
   function VolverAtras(Id) {
        var hiddenNumeroCoincidencias = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnNumeroEquivalencias");
        hiddenNumeroCoincidencias.value = "1";
        var txtContrato = document.getElementById("ctl00_ContentPlaceHolderContenido_txt_contrato");
        //alert(txtContrato);
        //alert(hiddenVisita.value);
        txtContrato.innerText = Id;
        
        var hdnBuscarPulsado = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnBuscarPulsado");
        hdnBuscarPulsado.value = "1";
        
        var cmbSubtipo = document.getElementById("ctl00_ContentPlaceHolderContenido_ddl_Subtipo");
       if (cmbSubtipo.value == "008") {
            var botonCalderas = document.getElementById("ctl00_ContentPlaceHolderContenido_btnBuscarCalderas");
            botonCalderas.click();
        }
       else {
            var botonBuscar = document.getElementById("ctl00_ContentPlaceHolderContenido_btnBuscar");
            botonBuscar.click();
        }
   }
   
   function InformarPulsadoBuscar() {
        var hdnBuscarPulsado = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnBuscarPulsado");
        hdnBuscarPulsado.value = "1";
    
   }


    function CargarDatos(Valor) {
        //alert("1");
        //abrirVentanaEspera("frmEspera.aspx", 620, 400, "ventana-modal","","1","1");
        //abrirVentanaAlerta(620, 400, "ventana-modal","CARGANDO DATOS...","1","1");
        MostrarCapaEspera();
        //alert("2");
        //window.form1.style.visibility ="hidden";

        var hiddenVisita = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodVisita");//ctl00$ContentPlaceHolderContenido$hdnCodVisita");
        if (hiddenVisita != null) {
            //alert(hiddenVisita.value);
            hiddenVisita.value = "";
            //alert(hiddenVisita.value);
        }
        //alert(hiddenVisita);
        var hiddenSolicitudH = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitudHistorico");
        if (hiddenSolicitudH != null) {
            //alert(hiddenSolicitudH.value);
            hiddenSolicitudH.value = "";
            //alert(hiddenSolicitudH.value);
        }
        //alert(hiddenSolicitudH);
        var hiddenSeleccionadoAlta = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnSeleccionadoAlta");
        hiddenSeleccionadoAlta.value = "0";
        //alert(hiddenSeleccionadoAlta);
        var hiddenSolicitud = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud");
        hiddenSolicitud.value = Valor;
        javascript: __doPostBack('gv_Solicitudes', Valor);
        //alert(hiddenSeleccionadoAlta);
    }


    function refrescar() {
        var botonBuscar = document.getElementById("ctl00_ContentPlaceHolderContenido_btnBuscar");

        botonBuscar.click();
    }

    function onBtnNuevaSolicitudClick() {

        if (document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText == '') {
            alert('<%=Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_CONTRATO%>');
            return false;
        }
        else {
            //alert(document.getElementById("ctl00$ContentPlaceHolderContenido$hdnEsSMGAmpliado").value);
            if (document.getElementById("ctl00_ContentPlaceHolderContenido_hdnEsSMGAmpliado").value == '1'
            || document.getElementById("ctl00_ContentPlaceHolderContenido_hdnEsSMGAmpliadoIndependiente").value == '1') {

                if (document.getElementById("ctl00_ContentPlaceHolderContenido_lblNAveriasResueltas").innerText != '0'
               && document.getElementById("ctl00_ContentPlaceHolderContenido_lblNAveriasResueltas").innerText != '1'
               ) {

                    if (confirm("<%=Resources.TextosJavaScript.TEXTO_MAS_DE_DOS_AVERIAS%>")) {
                        //alert('5');
                        var lblProceso = document.getElementById("ctl00_ContentPlaceHolderContenido_lblProceso");
                        lblProceso.innerText = "<%=Resources.TextosJavaScript.ALTA_SOLICITUD%>";//Resources.ALTA_SOLICITUD
                        var hiddenSeleccionadoAlta = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnSeleccionadoAlta");
                        hiddenSeleccionadoAlta.value = "1";
                        //alert(document.getElementById("ctl00$ContentPlaceHolderContenido$hdnSeleccionadoAlta").value);
                        var hiddenSolicitud = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud");
                        hiddenSolicitud.value = "";
                        var hiddenVisita = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodVisita");
                        hiddenVisita.value = "";
                        var hiddenSolicitudH = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitudHistorico");
                        hiddenSolicitudH.value = "";


                        InicializarPanelNueva();

                        VerEditar();
                    }
                }
                else {

                    var lblProceso = document.getElementById("ctl00_ContentPlaceHolderContenido_lblProceso");
                    lblProceso.innerText = "<%=Resources.TextosJavaScript.ALTA_SOLICITUD%>";//Resources.ALTA_SOLICITUD; // 'ALTA SOLICITUD';
                    var hiddenSeleccionadoAlta = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnSeleccionadoAlta");
                    hiddenSeleccionadoAlta.value = "1";
                    //alert(document.getElementById("ctl00$ContentPlaceHolderContenido$hdnSeleccionadoAlta").value);
                    var hiddenSolicitud = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud");
                    hiddenSolicitud.value = "";
                    var hiddenVisita = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodVisita");
                    hiddenVisita.value = "";
                    var hiddenSolicitudH = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitudHistorico");
                    hiddenSolicitudH.value = "";


                    InicializarPanelNueva();

                    VerEditar();

                }

            }

            else {
                //alert('7');
                var lblProceso = document.getElementById("ctl00_ContentPlaceHolderContenido_lblProceso");
                lblProceso.innerText = "<%=Resources.TextosJavaScript.ALTA_SOLICITUD%>";//Resources.ALTA_SOLICITUD; // 'ALTA SOLICITUD';
                var hiddenSeleccionadoAlta = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnSeleccionadoAlta");
                hiddenSeleccionadoAlta.value = "1";
                //alert(document.getElementById("ctl00$ContentPlaceHolderContenido$hdnSeleccionadoAlta").value);
                var hiddenSolicitud = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud");
                hiddenSolicitud.value = "";
                var hiddenVisita = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodVisita");
                hiddenVisita.value = "";
                var hiddenSolicitudH = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitudHistorico");
                hiddenSolicitudH.value = "";
                var hiddenEstadoSol = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnDesEstadoSol");
                hiddenEstadoSol.value = "";

                InicializarPanelNueva();

                VerEditar();

            }
        }
    }

        function InicializarPanelNueva() {

        document.getElementById("ctl00_ContentPlaceHolderContenido_ddlSubtipo").selectedIndex = 0;
        document.getElementById("ctl00_ContentPlaceHolderContenido_ddlEstado").selectedIndex = -1;
        document.getElementById("ctl00_ContentPlaceHolderContenido_ddlEstado").Text = "";
        document.getElementById("ctl00_ContentPlaceHolderContenido_ddlMotivoCancelacion").selectedIndex = 0;
        document.getElementById("ctl00_ContentPlaceHolderContenido_ddlMotivoCancelacion").disabled = true;
        document.getElementById("ctl00_ContentPlaceHolderContenido_txtEditarTelefono").innerText = '';
        document.getElementById("ctl00_ContentPlaceHolderContenido_txtPersonaContacto").innerText = '';
        document.getElementById("ctl00_ContentPlaceHolderContenido_cmbHorarioContacto").selectedIndex = 0;
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_ddlAveria") != null) { document.getElementById("ctl00_ContentPlaceHolderContenido_ddlAveria").selectedIndex = 0; }
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_ddlVisita") != null) { document.getElementById("ctl00_ContentPlaceHolderContenido_ddlVisita").selectedIndex = 0; }
        document.getElementById("ctl00_ContentPlaceHolderContenido_chkUrgente").checked = false;
        document.getElementById("ctl00_ContentPlaceHolderContenido_txtEditarObservacionesAnteriores").innerText = '';
        document.getElementById("ctl00_ContentPlaceHolderContenido_txtEditarObservaciones").innerText = '';
        document.getElementById("ctl00_ContentPlaceHolderContenido_txtMarcaCaldera").innerText = '';
        document.getElementById("ctl00_ContentPlaceHolderContenido_txtModelocaldera").innerText = '';
        //Retencion
        //        document.getElementById("ctl00_ContentPlaceHolderContenido_chkRetencion").checked = false;


        document.getElementById("ctl00_ContentPlaceHolderContenido_btnAceptarEditarSolicitud").disabled = false;
        document.getElementById("ctl00_ContentPlaceHolderContenido_ddlSubtipo").disabled = false;
        document.getElementById("ctl00_ContentPlaceHolderContenido_ddlEstado").disabled = false;




        if (document.getElementById("ctl00_ContentPlaceHolderContenido_lbl_Mensaje").innerText == "<%=Resources.TextosJavaScript.SERVICIO_DADO_DE_BAJA%>") {//Resources.SERVICIO_DADO_DE_BAJA) {//"EL SERVICIO ESTA DADO DE BAJA") {

            var i = 0;
            var dll = document.getElementById("ctl00_ContentPlaceHolderContenido_ddlSubtipo");
            if (dll) {
                // Note: this loop starts at the bottom to avoid skipping any due to re-indexing as they are removed.
                for (i = (dll.options.length - 1) ; i >= 0; i--) {
                    if (dll.options[i].value != '003' && dll.options[i].value != '004') {
                        dll.remove(i);
                    }
                }
            }
        }
        else {
            var i = 0;
            var dll = document.getElementById("ctl00_ContentPlaceHolderContenido_ddlSubtipo");
            if (dll) {
                // Note: this loop starts at the bottom to avoid skipping any due to re-indexing as they are removed.
                for (i = (dll.options.length - 1) ; i >= 0; i--) {
                    if (dll.options[i].value == '005') {
                        dll.remove(i);
                    }
                }
            }
        }

    }

    function MostrarCoberturaServicio() {
        abrirVentanaLocalizacion1("FrmModalCoberturaServicio.aspx?codEFV=" + document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodEFV").value, 600, 230, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_COBERTURA_SERVICIO%>", "0", "1");
    }    

    function InicializarSolicitud() {
        document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud").value = "";
    }

    function onBtnNuevaSolicitudReclamacionClick() {
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText == '') {
            alert('<%=Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_CONTRATO%>');
            return false;
        }
        else {
            NuevaReclamacion('', '', document.getElementById("ctl00_ContentPlaceHolderContenido_lblProveedorInfo").innerText);
        }
    }

    function NuevaReclamacion(Id, Estado, Proveedor) {
        //alert(Proveedor);
        abrirVentanaLocalizacion("./FrmModalAltaReclamacion.aspx?Contrato=" + document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText + "&idSolicitud=" + Id + "&Estado=" + Estado + "&Proveedor=" + Proveedor, "460", "420", "Reclamacion", "<%=Resources.TextosJavaScript.TEXTO_SOLICITUD_RECLAMACION%>", "", false);
        return true;
    }

     function Preguntar() {
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_lblCodContratoInfo").innerText == '') {
            alert('<%=Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_CONTRATO%>');
            return false;
        }
        else {


            if (confirm("<%=Resources.TextosJavaScript.TEXTO_DESEA_REALMENTE_DAR_BAJA%>")) {
                if (confirm("<%=Resources.TextosJavaScript.TEXTO_ESTA_SEGURO%>")) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
    }

     function Limpiar() {
        document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud").value = "";
        document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud").value = "";
        //alert('');
        return true;
    }

     function MostrarAvisoSolicitud() {
        var solicitud = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud");

        if (solicitud.value != '' && solicitud.value != null) {
            abrirVentanaLocalizacion("../Pages/AvisoSolicitudes.aspx?Solicitud=" + solicitud.value + "&mail=", 600, 350, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_NUEVO_AVISO%>", "0", "1");
        }
    }

     function Twitter() {
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_lblTwitter").style.visibility == "visible") {
            document.getElementById("ctl00_ContentPlaceHolderContenido_lblTwitter").style.visibility = "hidden";
            document.getElementById("ctl00_ContentPlaceHolderContenido_chkTwitter").checked = false;
        }
        else {
            document.getElementById("ctl00_ContentPlaceHolderContenido_lblTwitter").style.visibility = "visible";
            document.getElementById("ctl00_ContentPlaceHolderContenido_chkTwitter").checked = true;
        }

    }
    function Facebook() {
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_lblFacebook").style.visibility == "visible") {
            document.getElementById("ctl00_ContentPlaceHolderContenido_lblFacebook").style.visibility = "hidden";
            document.getElementById("ctl00_ContentPlaceHolderContenido_chkFacebook").checked = false;
        }
        else {
            document.getElementById("ctl00_ContentPlaceHolderContenido_lblFacebook").style.visibility = "visible";
            document.getElementById("ctl00_ContentPlaceHolderContenido_chkFacebook").checked = true;
        }

    }

    
    // Muestra el panel de visitas
    function VerVisitas() {
        var btnVisitas = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPestanyaVisitas");
        var btnSolicitudes = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPestanyaSolicitudes");
        var divVisitas = document.getElementById("divVisitas");
        var divSolicitudes = document.getElementById("divSolicitudes");
        var divEditar = document.getElementById("divEditar");
        var divContador = document.getElementById("divContador");
        var divBotonNuevo = document.getElementById("divBotonNuevo");
        var divBtnExcel = document.getElementById("divBtnExcel");
        var divUltimoMov = document.getElementById("ctl00_ContentPlaceHolderContenido_divUltimoMovimiento");
        //var lblProceso = document.getElementById("ctl00_ContentPlaceHolderContenido_lblProceso");
        //lblProceso.innerText = '';
        //alert("2222");

        divContador.style.visibility = "visible";
        divEditar.style.visibility = "hidden";
        divBotonNuevo.style.visibility = "visible";
        divBtnExcel.style.visibility = "visible";
        if (divUltimoMov != null) { divUltimoMov.style.visibility = "hidden"; }

        btnVisitas.disabled = true;
        btnVisitas.src = "./HTML/Images/VisitasActivo_HORIZONTAL.jpg";
        divVisitas.style.visibility = "visible";

        btnSolicitudes.disabled = false;
        btnSolicitudes.style.backgroundColor = "#34721f";
        
        //btnSolicitudes.src = "./HTML/Images/SolicitudesDesactivado_HORIZONTAL.jpg";
        divSolicitudes.style.visibility = "hidden";

        var hiddenPanelSeleccionado = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnPanelSeleccionado");
        hiddenPanelSeleccionado.value = "VISITAS";

    }

     function CargarDatosVisita(Valor) {
        //abrirVentanaEspera("frmEspera.aspx", 620, 400, "ventana-modal","","1","1");
        //abrirVentanaAlerta(620, 400, "ventana-modal","CARGANDO DATOS...","1","1");
        MostrarCapaEspera();
        //window.form1.style.visibility ="hidden";

        var hiddenVisita = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodVisita");
        hiddenVisita.value = Valor;
        javascript: __doPostBack('gvVisitas', Valor);

    }

    function CargarDatosVisitaProveedor(Contrato, Visita, TotalVisitas) {
        //abrirVentanaEspera("frmEspera.aspx", 620, 400, "ventana-modal","","1","1");
        //abrirVentanaAlerta(620, 400, "ventana-modal","CARGANDO DATOS...","1","1");
        MostrarCapaEspera();
        //window.form1.style.visibility ="hidden";

        //        var hiddenVisita = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnCodVisita");
        //        hiddenVisita.value = Valor;
        var url = "FrmDetalleVisita.aspx?idContrato=" + Contrato + "&idVisita=" + Visita + "&TotalVisitas=" + TotalVisitas;
        window.location.href = url;
    }
</script>
</asp:content>
