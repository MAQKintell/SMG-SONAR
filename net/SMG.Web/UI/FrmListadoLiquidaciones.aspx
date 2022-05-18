<%@ Page Language="C#" MasterPageFile="MasterPageMntoGasSinMenu.master" AutoEventWireup="true" CodeFile="FrmListadoLiquidaciones.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmListadoLiquidaciones" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="Iberdrola.Commons.Web" %>


<asp:Content ID="FrmListadoProveedores" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> 
    <LINK href="../css/ventan-modal-ie.css" type="text/css" rel="stylesheet">
	<script src="../js/ventana-modal.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="../css/styles.css" />
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../js/jquery.flip.min.js"></script>
    <script type="text/javascript" src="../js/script.js"></script>

    <script type="text/javascript">
    function ExportarExcel() {
        //Alert('qqq');
        //alert(document.getElementById("ctl00_ContentPlaceHolderContenido_lblNumEncontrados").innerText);
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
    function ExportarExcelReparacion() {
        //alert('qqq');
        //alert(document.getElementById("ctl00_ContentPlaceHolderContenido_lblNumEncontrados").innerText);
        var lblRegistros = document.getElementById("ctl00_ContentPlaceHolderContenido_lblNumEncontradosReparacion");
        if(lblRegistros.innerText<=0)
        {
            alert("No se puede exportar a excel porque no se han encontrado registros.");
        }
        else
        {
            abrirVentanaLocalizacion("../UI/AbrirExcel.aspx?tipo=Reparacion", 600, 350, "ventana-modal","EXPORTAR EXCEL","0","1");
        }
    }
    function btnInputExcel_onclick() {

    }
    function btnInputExcelReparacion_onclick() {

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
    <script>
        function ActualizarControlFoco(nombreControl)
        {
            var hdnControlFoco = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnControlFoco");
            if (hdnControlFoco)
            {
                hdnControlFoco.value = nombreControl;
            }
        }
    </script>
    <script>
        //Kintell 15/04/2010
        function PosicionarProvincia()
        {
            //var charCode = (window.event.which) ? window.event.which : event.keyCode
            //if not a digit or arrow key abort
            //Alert('1');
            //Alert(event.keyCode);
            //Alert(window.event);
      
            var ValorABuscar=document.getElementById("ctl00$ContentPlaceHolderContenido$txtBuscarProvincia").value;
            var Nombres= ctl00_ContentPlaceHolderContenido_chkProvincias.getElementsByTagName("label");
            var Esta=false;
            var cont=document.getElementById("ctl00$ContentPlaceHolderContenido$hdnContadorProvincia").value;

            for(var i=0;i<cont;i++)
            {
                var Nombre=Nombres[i].innerText;
                var a1=Nombre.substring(0,ValorABuscar.length);
                var a2=ValorABuscar;
                
                if ((a1.toUpperCase() == a2.toUpperCase() || a1==a2) && Esta==false)
                {
                    //Posicionar.
                    ctl00_ContentPlaceHolderContenido_pnLista.scrollTop=getXY(Nombres[i])-150;
                    Esta=true;
                    break;
                }
            }
        }
        function PosicionarPoblacion()
        {
            var ValorABuscar=document.getElementById("ctl00$ContentPlaceHolderContenido$txtBuscarPoblacion").value;
            var Nombres= ctl00_ContentPlaceHolderContenido_chkPoblaciones.getElementsByTagName("label");
            var Esta=false;
            var cont=document.getElementById("ctl00_ContentPlaceHolderContenido_hdnContadorPoblacion").value;

            for(var i=0;i<cont;i++)
            {
                var Nombre=Nombres[i].innerText;
                var a1=Nombre.substring(0,ValorABuscar.length);
                var a2=ValorABuscar;
                
                if ((a1.toUpperCase() == a2.toUpperCase() || a1==a2) && Esta==false)
                {
                    //Posicionar.
                    ctl00_ContentPlaceHolderContenido_Panel3.scrollTop=getXY(Nombres[i])-160;
                    Esta=true;
                    break;
                }
            }
        }
        
        
        function getXY(obj)
        {
          var curleft = 0;
          var curtop = obj.offsetHeight + 5;
          var border;
          if (obj.offsetParent)
          {
            do
            {
              // XXX: If the element is position: relative we have to add borderWidth
              if (getStyle(obj, 'position') == 'relative')
              {
                if (border = _pub.getStyle(obj, 'border-top-width')) curtop += parseInt(border);
                if (border = _pub.getStyle(obj, 'border-left-width')) curleft += parseInt(border);
              }
              curleft += obj.offsetLeft;
              curtop += obj.offsetTop;
            }
            while (obj = obj.offsetParent)
          }
          else if (obj.x)
          {
            curleft += obj.x;
            curtop += obj.y;
          }
          //Alert('x:' + curleft + '_y:' + curtop);
          return curtop;
        }

        function getStyle(obj, styleProp)
        {
          if (obj.currentStyle)
            return obj.currentStyle[styleProp];
          else if (window.getComputedStyle)
            return document.defaultView.getComputedStyle(obj,null).getPropertyValue(styleProp);
        }
    </script>
    <script type="text/javascript">
        function PanelClick(Panel) { 
        
//            switch(Panel)
//            {
//                case "pnlVisita":
//                    document.getElementById('GridMovimiento').title="Pulsa para cambiar entre Visitas y Reparaciones";
//                    if(document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaVisita').value != "" && document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaVisita').value != document.getElementById('ctl00_ContentPlaceHolderContenido_lblFactura').innerText) { __doPostBack(Panel, 'Click'); }
//                    else{ return false; }
//                  break;
//                case "pnlSolVisita":
//                    if(document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaSolVisita').value != "" && document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaSolVisita').value != document.getElementById ('ctl00_ContentPlaceHolderContenido_lblFactura').innerText) { __doPostBack(Panel, 'Click'); }
//                    else{ return false; }
//                  break;
//                case "pnlSolVisitaIncorrecta":
//                    if(document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaVisitaInco').value != "" && document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaVisitaInco').value != document.getElementById ('ctl00_ContentPlaceHolderContenido_lblFactura').innerText) { __doPostBack(Panel, 'Click'); }
//                    else{ return false; }
//                  break;
//                case "pnlSolReviPrecinte":
//                    if(document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaRevPrecinte').value != "" && document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaRevPrecinte').value != document.getElementById ('ctl00_ContentPlaceHolderContenido_lblFactura').innerText) { __doPostBack(Panel, 'Click'); }
//                    else{ return false; }
//                  break;
//                case "pnlSolAveria":
//                    if(document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaSolAveria').value != "" && document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaSolAveria').value != document.getElementById ('ctl00_ContentPlaceHolderContenido_lblFactura').innerText) { __doPostBack(Panel, 'Click'); }
//                    else{ return false; }
//                  break;
//                case "pnlSolAveriaIncorrecta":
//                    if(document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaSolAveriaInco').value != "" && document.getElementById ('ctl00_ContentPlaceHolderContenido_txtFacturaSolAveriaInco').value != document.getElementById ('ctl00_ContentPlaceHolderContenido_lblFactura').innerText) { __doPostBack(Panel, 'Click'); }
//                    else{ return false; }
//                  break;
//                case "pnlReparaciones":
//                     __doPostBack(Panel, 'Click'); 
//                  break;
//            }
        } 
        
        function Movimiento() { 
        //alert(document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText);
            if(document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText=="Visitas"){
                document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText="Reparaciones";
                document.getElementById('divContador').style.visibility="hidden";
                document.getElementById('divContadorReparacion').style.visibility="visible";
                
                document.getElementById('divBotonExcel').style.visibility="hidden";
                document.getElementById('divBotonExcelReparacion').style.visibility="visible";
            }
            else if(document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText=="Reparaciones"){
                document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText="Visitas";
                document.getElementById('divContadorReparacion').style.visibility="hidden";
                document.getElementById('divContador').style.visibility="visible";
                
                document.getElementById('divBotonExcel').style.visibility="visible";
                document.getElementById('divBotonExcelReparacion').style.visibility="hidden";
            }
        }
        
        function MovimientoReparacion() { 
        //alert(document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText);
            if(document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText=="Visitas"){
                document.getElementById('divContador').style.visibility="visible";
                document.getElementById('divContadorReparacion').style.visibility="hidden";
                
                document.getElementById('divBotonExcel').style.visibility="visible";
                document.getElementById('divBotonExcelReparacion').style.visibility="hidden";
            }
            else{
                document.getElementById('divContador').style.visibility="hidden";
                document.getElementById('divContadorReparacion').style.visibility="visible";
                
                document.getElementById('divBotonExcel').style.visibility="hidden";
                document.getElementById('divBotonExcelReparacion').style.visibility="visible";
                Movimiento();
//                var elem = $(this);
//	                elem.flip({
//		                direction:'lr',
//		                speed: 300,
//		                onBefore: function(){
//			                elem.html(elem.siblings('.reparacion').html());
//		                }
//	                });
//	                elem.data('girada',true);
            }
            //Movimiento();
//            else if(document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText=="Reparaciones"){
//                Movimiento();
//                document.getElementById('divContadorReparacion').style.visibility="visible";
//                document.getElementById('divContador').style.visibility="hidden";
//                
//                document.getElementById('divGridReparaciones').style.visibility="visible";
//                document.getElementById('divGrid').style.visibility="hidden";
//            }
        }
    </script> 
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Listado de liquidacion" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
		
    <asp:ScriptManager ID="SM" runat="server"></asp:ScriptManager>
<div style="position:absolute; top: 7px; left: 352px; height: 14px; width: 389px;">
    <asp:Label ID="Label14" CssClass="labelFormulario" runat="server" 
        Text="Penalizar 0= BONIFICADAS, Penalizar 1= PENALIZADAS, Penalizar 2= NBNP" 
        Font-Bold="True" ForeColor="Red" meta:resourcekey="Label14Resource1"></asp:Label></div>

		<div class="GridMovimiento" id="GridMovimiento" style="position:absolute;" onclick="Movimiento();return false;" title="Pulsa para cambiar entre Visitas y Reparaciones">
			<div class="GridDatos">
	             <div id="divGrid" class="panelTabla" style="overflow:scroll;border: thin solid #000000; position:absolute; height: 376px; width: 958px;">
                    <asp:GridView ID="grdDatos" runat="server" CssClass="contenidoTabla" 
                         EnableViewState="False" Height="6px" EnableModelValidation="True" 
                         meta:resourcekey="grdDatosResource1">
                        <RowStyle CssClass="filaNormal"/>
                        <AlternatingRowStyle CssClass="filaAlterna"/>
                        <FooterStyle CssClass="cabeceraTabla"  />
                        <PagerStyle  CssClass="cabeceraTabla" />
                        <HeaderStyle CssClass="cabeceraTabla" />
                    </asp:GridView>
                </div>
            </div>
            <div class="reparacion">
				 <div id="divGridReparaciones" class="panelTabla" style="overflow:scroll;border: thin solid #000000; position:absolute; height: 376px; width: 958px;">
                    <asp:GridView ID="grdReparaciones" runat="server" CssClass="contenidoTabla" 
                         EnableViewState="False" Height="6px" EnableModelValidation="True" 
                         meta:resourcekey="grdReparacionesResource1">
                        <RowStyle CssClass="filaNormal"/>
                        <AlternatingRowStyle CssClass="filaAlterna"/>
                        <FooterStyle CssClass="cabeceraTabla"  />
                        <PagerStyle  CssClass="cabeceraTabla" />
                        <HeaderStyle CssClass="cabeceraTabla" />
                    </asp:GridView>
                </div>
			</div>
		</div>

        <div id="divContador" style="position:absolute; top: 530px; left: 3px; height: 51px; width: 329px" align="center">
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
            <asp:Button ID="btnPaginacion" runat="server" OnClick="OnBtnPaginacion_Click" 
                Width="0px" meta:resourcekey="btnPaginacionResource1"/> 
        </div>

        <div id="divContadorReparacion" style="visibility:hidden;position:absolute; top: 530px; left: 3px; height: 51px; width: 329px" align="center">
            <table width="100%" align="left">
                <tr>
                    <td align="left" colspan ="2">
                        <asp:PlaceHolder ID="placeHolderPaginacionReparacion" runat="server">
                        &nbsp;  
                        </asp:PlaceHolder>
                    </td>
                </tr>
                 <tr>
                    <td align="left">
                        <asp:Label CssClass="labelFormularioValor" ID="lblNumEncontradosReparacion" 
                            runat="server" Text="0" meta:resourcekey="lblNumEncontradosReparacionResource1"></asp:Label>
                        <asp:Label CssClass="labelFormulario" ID="lblContadorReparacion" runat="server" 
                            Text="regs. encontrados." meta:resourcekey="lblContadorReparacionResource1"></asp:Label>
                        
                    </td>
                    <td align="left" style="width:40px">        
                    </td>

                </tr>
            </table> 
            <asp:HiddenField ID="hdnSortDirectionReparacion" value="ASC" runat="server" />
            <asp:HiddenField ID="hdnSortColumnReparacion" runat="server" />
            <asp:HiddenField ID="hdnPageCountReparacion" runat="server" />
            <asp:HiddenField ID="hdnPageIndexReparacion" runat="server" />
            <asp:HiddenField ID="hdnRowIndexReparacion" runat="server" />
            <asp:HiddenField ID="hdnClickedCellValueReparacion" runat="server" />
            <asp:HiddenField ID="hdnRecargarUltimaConsultaReparacion" runat="server" />
            <asp:Button ID="btnPaginacionReparacion" runat="server" 
                OnClick="OnBtnPaginacionReparacion_Click" Width="0px" 
                meta:resourcekey="btnPaginacionReparacionResource1"/> 
        </div>
                
                
                
    <div id="divProcesoSeleccionado" style="position:absolute; top: 526px; left: 716px; height: 23px; width: 249px;">
        <asp:Label CssClass="labelFormulario" ID="Label2" runat="server" 
            Font-Bold="True" ForeColor="Black" 
                            Text="Proceso:" meta:resourcekey="Label2Resource1" ></asp:Label>
                        &nbsp;
                        <asp:Label ID="lblProceso" CssClass="labelFormulario" 
            runat="server" Font-Bold="True" 
            ForeColor="#009933" meta:resourcekey="lblProcesoResource1"></asp:Label>
        <br />
        <asp:Label CssClass="labelFormulario" ID="Label3" runat="server" 
            Font-Bold="True" ForeColor="Black" 
                            Text="Número Factura:" meta:resourcekey="Label3Resource1" ></asp:Label>
                        &nbsp;
                <asp:Label ID="lblFactura" CssClass="labelFormulario" runat="server" Font-Bold="True" 
                ForeColor="#009933" meta:resourcekey="lblFacturaResource1"></asp:Label>
    </div>
    
    
    <div id="divBotonExcel" style="position:absolute; top: 534px; left: 369px; height: 23px; width: 258px;">
         <input type="button" id="btnInputExcel" visible="false" name="btnInputExcel" class="botonFormulario" value="Exportar a Excel" onclick="javascript:ExportarExcel();return false;" tabindex="5" onclick="return btnInputExcel_onclick()" onclick="return btnInputExcel_onclick()" />
    </div>
    <div id="divBotonExcelReparacion" style="visibility:hidden;position:absolute; top: 534px; left: 369px; height: 23px; width: 258px;">
         <input type="button" id="btnInputExcelReparacion" visible="false" name="btnInputExcel" class="botonFormulario" value="Exportar a Excel" onclick="javascript:ExportarExcelReparacion();return false;" tabindex="5" onclick="return btnInputExcelReparacion_onclick()" onclick="return btnInputExcelReparacion_onclick()" />
    </div>
    
    <div style="border: thin double #006600; position:absolute; top: 25px; left: 757px; width: 202px; height: 105px; right: 10px;">
        <asp:Panel ID="Panel7" runat="server"  
                        Height="108px" Width="201px" BackImageUrl="../Imagenes/fondo.png"
                        style="cursor:pointer" 
            onclick="PanelClick('pnlSolAveriaIncorrecta');" 
            meta:resourcekey="Panel7Resource1">
        <table>
            <tr>
                <td class="labelTabla" style="width: 244px; height: 25px;">
                    <asp:RadioButtonList ID="rdbPenalizables" runat="server" 
                        CssClass="campoFormulario"  AutoPostBack="True"
                        onselectedindexchanged="rdbPenalizables_SelectedIndexChanged" 
                        ForeColor="White" meta:resourcekey="rdbPenalizablesResource1">
                        <asp:ListItem Selected="True" Value="3" meta:resourcekey="ListItemResource1">TODAS</asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="ListItemResource2">Penalizables</asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="ListItemResource3">Bonificables</asp:ListItem>
                        <asp:ListItem Value="2" meta:resourcekey="ListItemResource4">NP/NB</asp:ListItem>
                    </asp:RadioButtonList>
                    
                </td>
            </tr>
            </table>
        </asp:panel>
    </div>
    <div style="border-style: none; border-color: inherit; border-width: medium; position:absolute; top: -7px; left: 754px; width: 189px; height: 32px; right: 32px;">
    <table><tr><td class="labelTabla" style="width: 244px; height: 36px;">
                <asp:Label ID="Label7" runat="server" CssClass="campoFormulario" 
                    Text="PROVEEDOR" ForeColor="Black" meta:resourcekey="Label7Resource1"></asp:Label>
                <asp:DropDownList ID="cmbProveedor" runat="server" AutoPostBack ="True"
                    onselectedindexchanged="cmbProveedor_SelectedIndexChanged" 
                    meta:resourcekey="cmbProveedorResource1">
                </asp:DropDownList>
                </td></tr>
        </table></div>
        
		

        
        
        <div  style="position:absolute; top: 92px; left: 640px; height: 46px; width: 86px; display:none;">
		            <asp:Panel BorderWidth="2px" BorderColor="#FFC080" ID="pnlReparaciones" 
                        Visible="False" runat="server" BackImageUrl="../Imagenes/fondo.png" 
                        Height="40px" Width="106px"
                        onMouseOver ="this.style.backgroundImage='url(../Imagenes/Ffondo.gif)'"
                        onMouseOut ="this.style.backgroundImage='url(../Imagenes/fondo.png)'"
                        style="cursor:pointer" onClientClick="MostrarAvisos();return false;" 
                        onclick="PanelClick('pnlReparaciones');" 
                        meta:resourcekey="pnlReparacionesResource1">
                        <table width="100%" height="100%">
                        <tr align="center" valign="middle">
                        <td align="center" valign="middle" >
                        
                            <asp:Label CssClass="labelFormulario"  ID="Label10" runat="server" 
                                Font-Bold="True" ForeColor="#FFC080" 
                            Text="MOSTRAR REPARACIONES" meta:resourcekey="Label10Resource1"></asp:Label>
                        </td>
                        </tr></table></asp:Panel>	
                        </div>	
        
        
        
    <div style="border: thin double #006600; position:absolute; top: 23px; left: 8px; width: 618px; height: 110px; right: 343px;">
        <table>
            <tr>
                <td class="labelTabla" style="width: 176px; height: 36px;">
                    <asp:Panel ID="Panel1" runat="server" BackImageUrl="../Imagenes/fondo.png" 
                        Height="50px" Width="201px"
                        onMouseOver ="this.style.backgroundImage='url(../Imagenes/Ffondo.gif)'"
                        onMouseOut ="this.style.backgroundImage='url(../Imagenes/fondo.png)'"
                        style="cursor:pointer" onClientClick="MostrarAvisos();return false;" 
                        onclick="PanelClick('pnlVisita');" meta:resourcekey="Panel1Resource1">
                        <table>
                        <tr>
                        <td>
                            <asp:ImageButton onClientClick="MostrarCapaEspera();" ID="ImageButton1" 
                                runat="server" Height="41px" 
                                ImageUrl="../Imagenes/solicitud.gif" Width="39px" ToolTip="Visitas" 
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="0px" 
                                onclick="ImageButton1_Click" meta:resourcekey="ImageButton1Resource1" />
                        </td>
                        
                        <td style="width: 61px" >
                        <asp:Label CssClass="labelFormulario"  ID="Label4" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Proceso:" meta:resourcekey="Label4Resource1"></asp:Label>
                        &nbsp;
                        <asp:Label CssClass="labelFormulario"  ID="Label6" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Núm.Factura:" meta:resourcekey="Label6Resource1"></asp:Label>
                        </td>
                        <td style="width: 77px">
                        <asp:Label CssClass="labelFormulario"  ID="Label5" runat="server" ForeColor="White" 
                                Text="Visita" meta:resourcekey="Label5Resource1"></asp:Label>
                        &nbsp;
                        <asp:TextBox ID="txtFacturaVisita" runat="server" Width="70px" 
                                meta:resourcekey="txtFacturaVisitaResource1"></asp:TextBox>
                        </td>
                        
                        </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td class="campoTabla" style="width: 100; height: 36px;">
                
                    <asp:Panel ID="Panel2" runat="server" BackImageUrl="../Imagenes/fondo.png" 
                        Height="50px" Width="201px"
                        onMouseOver ="this.style.backgroundImage='url(../Imagenes/Ffondo.gif)'"
                        onMouseOut ="this.style.backgroundImage='url(../Imagenes/fondo.png)'"
                        style="cursor:pointer" onclick="PanelClick('pnlSolVisita');" 
                        meta:resourcekey="Panel2Resource1">
                        <table style="width: 203px">
                        <tr>
                        <td>
                            <asp:ImageButton onClientClick="MostrarCapaEspera();" ID="ImageButton2" 
                                runat="server" Height="43px" 
                                ImageUrl="../Imagenes/solicitud.gif" Width="39px" 
                                ToolTip="Solicitudes de Visita" BorderColor="Black" BorderStyle="Solid" 
                                BorderWidth="0px" onclick="ImageButton2_Click" 
                                meta:resourcekey="ImageButton2Resource1" />
                        </td>
                        
                        
                        <td style="width: 62px" >
                        <asp:Label CssClass="labelFormulario"  ID="Label1" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Proceso:" meta:resourcekey="Label1Resource1"></asp:Label>
                        &nbsp;
                        <asp:Label CssClass="labelFormulario"  ID="Label8" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Núm.Factura:" meta:resourcekey="Label8Resource1"></asp:Label>
                        </td>
                        <td style="width: 79px">
                        <asp:Label CssClass="labelFormulario"  ID="Label9" runat="server" ForeColor="White" 
                                Text="Solicitud Visita" meta:resourcekey="Label9Resource1"></asp:Label>
                            <asp:TextBox ID="txtFacturaSolVisita" runat="server" Width="70px" 
                                meta:resourcekey="txtFacturaSolVisitaResource1"></asp:TextBox>
                        </td>
                        
                        </tr>
                        </table>
                    </asp:Panel></td>
            
                <td class="labelTabla" style="width: 53px; height: 36px;">
                    <asp:Panel ID="Panel3" runat="server" BackImageUrl="../Imagenes/fondo.png" 
                        Height="50px" Width="201px"
                        onMouseOver ="this.style.backgroundImage='url(../Imagenes/Ffondo.gif)'"
                        onMouseOut ="this.style.backgroundImage='url(../Imagenes/fondo.png)'"
                        style="cursor:pointer" onclick="PanelClick('pnlSolVisitaIncorrecta');" 
                        meta:resourcekey="Panel3Resource1">
                        <table>
                        <tr>
                        <td>
                            <asp:ImageButton onClientClick="MostrarCapaEspera();" ID="ImageButton3" 
                                runat="server" Height="43px" 
                                ImageUrl="../Imagenes/solicitud.gif" Width="39px" 
                                ToolTip="Solicitudes de Visita Incorrecta" BorderColor="Black" 
                                BorderStyle="Solid" BorderWidth="0px" onclick="ImageButton3_Click" 
                                meta:resourcekey="ImageButton3Resource1" />
                        </td>
                        
                        
                        <td style="width: 60px" >
                        <asp:Label CssClass="labelFormulario"  ID="Label11" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Proceso:" meta:resourcekey="Label11Resource1"></asp:Label>
                        &nbsp;
                        <asp:Label CssClass="labelFormulario"  ID="Label12" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Núm.Factura:" meta:resourcekey="Label12Resource1"></asp:Label>
                        </td>
                        <td style="width: 85px">
                        <asp:Label CssClass="labelFormulario"  ID="Label13" runat="server" 
                                ForeColor="White" Text="Sol.Vis. Inco." meta:resourcekey="Label13Resource1"></asp:Label>
                        <asp:TextBox ID="txtFacturaVisitaInco" runat="server" Width="70px" 
                                meta:resourcekey="txtFacturaVisitaIncoResource1"></asp:TextBox>
                        </td>
                        
                        </tr>
                        </table>
                    </asp:Panel>
                </td>
                </tr>
                <tr>
                <td class="campoTabla" style="width: 176px; height: 36px;">
                
                    <asp:Panel ID="Panel4" runat="server" BackImageUrl="../Imagenes/fondo.png" 
                        Height="50px" Width="201px"
                        onMouseOver ="this.style.backgroundImage='url(../Imagenes/Ffondo.gif)'"
                        onMouseOut ="this.style.backgroundImage='url(../Imagenes/fondo.png)'"
                        style="cursor:pointer" onclick="PanelClick('pnlSolReviPrecinte');" 
                        meta:resourcekey="Panel4Resource1">
                        <table>
                        <tr>
                        <td>
                            <asp:ImageButton onClientClick="MostrarCapaEspera();" ID="ImageButton6" 
                                runat="server" Height="43px" 
                                ImageUrl="../Imagenes/solicitud.gif" Width="39px" 
                                ToolTip="Solicitudes de Revisión tras Precinte" BorderColor="Black" 
                                BorderStyle="Solid" BorderWidth="0px" onclick="ImageButton6_Click" 
                                meta:resourcekey="ImageButton6Resource1" />
                        </td>
                        
                        
                        <td style="width: 60px" >
                        <asp:Label CssClass="labelFormulario"  ID="Label15" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Proceso:" meta:resourcekey="Label15Resource1"></asp:Label>
                        &nbsp;
                        <asp:Label CssClass="labelFormulario"  ID="Label16" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Núm.Factura:" meta:resourcekey="Label16Resource1"></asp:Label>
                        </td>
                        <td style="width: 62px">
                        <asp:Label CssClass="labelFormulario"  ID="Label17" runat="server" 
                                ForeColor="White" Text="Sol.Rev.Precin." meta:resourcekey="Label17Resource1"></asp:Label>
                        <asp:TextBox ID="txtFacturaRevPrecinte" runat="server" Width="70px" 
                                meta:resourcekey="txtFacturaRevPrecinteResource1"></asp:TextBox>
                        </td>
                        
                        </tr>
                        </table>
                    </asp:Panel></td>
            
                <td class="labelTabla" style="width: 53px; height: 36px;">
                    <asp:Panel ID="Panel5" runat="server" BackImageUrl="../Imagenes/fondo.png" 
                        Height="50px" Width="201px"
                        onMouseOver ="this.style.backgroundImage='url(../Imagenes/Ffondo.gif)'"
                        onMouseOut ="this.style.backgroundImage='url(../Imagenes/fondo.png)'"
                        style="cursor:pointer" onclick="PanelClick('pnlSolAveria');" 
                        meta:resourcekey="Panel5Resource1">
                        <table>
                        <tr>
                        <td>
                            <asp:ImageButton onClientClick="MostrarCapaEspera();" ID="ImageButton5" 
                                runat="server" Height="43px" 
                                ImageUrl="../Imagenes/solicitud.gif" Width="39px" 
                                ToolTip="Solicitudes de Averia" BorderColor="Black" BorderStyle="Solid" 
                                BorderWidth="0px" onclick="ImageButton5_Click" 
                                meta:resourcekey="ImageButton5Resource1" />
                        </td>
                        
                        
                        <td style="width: 62px" >
                        <asp:Label CssClass="labelFormulario"  ID="Label19" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Proceso:" meta:resourcekey="Label19Resource1"></asp:Label>
                        &nbsp;
                        <asp:Label CssClass="labelFormulario"  ID="Label20" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Núm.Factura:" meta:resourcekey="Label20Resource1"></asp:Label>
                        </td>
                        <td style="width: 82px">
                        <asp:Label CssClass="labelFormulario"  ID="Label21" runat="server" 
                                ForeColor="White" Text="Sol. Averia" meta:resourcekey="Label21Resource1"></asp:Label>
                        <asp:TextBox ID="txtFacturaSolAveria" runat="server" Width="70px" 
                                meta:resourcekey="txtFacturaSolAveriaResource1"></asp:TextBox>
                        </td>
                        
                        </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td class="campoTabla" style="width: 100; height: 36px;">
                
                    <asp:Panel ID="Panel6" runat="server"  
                        Height="50px" Width="201px" BackImageUrl="../Imagenes/fondo.png"
                        onMouseOver ="this.style.backgroundImage='url(../Imagenes/Ffondo.gif)'"
                        onMouseOut ="this.style.backgroundImage='url(../Imagenes/fondo.png)'"
                        style="cursor:pointer" onclick="PanelClick('pnlSolAveriaIncorrecta');" meta:resourcekey="Panel6Resource1"
                        >
                        <table>
                        <tr>
                        <td>
                            <asp:ImageButton onClientClick="MostrarCapaEspera();" ID="ImageButton4" 
                                runat="server" Height="43px" 
                                ImageUrl="../Imagenes/solicitud.gif" Width="39px" 
                                ToolTip="Solicitudes de Averia Incorrecta" BorderColor="Black" 
                                BorderStyle="Solid" BorderWidth="0px" onclick="ImageButton4_Click" 
                                meta:resourcekey="ImageButton4Resource1" />
                        </td>
                        
                        
                        <td style="width: 62px" >
                        <asp:Label CssClass="labelFormulario"  ID="Label23" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Proceso:" meta:resourcekey="Label23Resource1"></asp:Label>
                        &nbsp;
                        <asp:Label CssClass="labelFormulario"  ID="Label24" runat="server" Font-Bold="True" ForeColor="#FFC080" 
                            Text="Núm.Factura:" meta:resourcekey="Label24Resource1"></asp:Label>
                        </td>
                        <td style="width: 66px">
                        <asp:Label CssClass="labelFormulario"  ID="Label25" runat="server" 
                                ForeColor="White" Text="Sol.Averia.Inco." meta:resourcekey="Label25Resource1"></asp:Label>
                        <asp:TextBox ID="txtFacturaSolAveriaInco" runat="server" Width="70px" 
                                meta:resourcekey="txtFacturaSolAveriaIncoResource1"></asp:TextBox>
                        </td>
                        
                        </tr>
                        </table>
                    </asp:Panel></td>
            </tr>
            </table>
        </div>
        <input id="hdnContadorProvincia" type="hidden" runat="server" />
    <input id="hdnContadorPoblacion" type="hidden" runat="server" value="1000" />
    
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
        function ClickPaginacionReparacion(ident)
        {
            MostrarCapaEspera();
            
            var botonPaginacion = document.getElementById("ctl00$ContentPlaceHolderContenido$btnPaginacionReparacion");
            var hiddenPaginacion = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnPageIndexReparacion");
            hiddenPaginacion.value = ident;
            botonPaginacion.click();
            return false;
        }
        function btnInputExcel_onclick() {

        }
        function btnInputExcelReparacion_onclick() {

        }
    </script>
</asp:Content>