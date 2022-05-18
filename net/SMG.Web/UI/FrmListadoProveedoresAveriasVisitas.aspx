<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGasSinMenu.master" AutoEventWireup="true" CodeFile="FrmListadoProveedoresAveriasVisitas.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmListadoProveedoresAveriasVisitas" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
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
            //alert('1');
            //alert(event.keyCode);
            //alert(window.event);
      
            var ValorABuscar=document.getElementById("ctl00_ContentPlaceHolderContenido_txtBuscarProvincia").value;
            var Nombres= ctl00_ContentPlaceHolderContenido_chkProvincias.getElementsByTagName("label");
            var Esta=false;
            var cont=document.getElementById("ctl00_ContentPlaceHolderContenido_hdnContadorProvincia").value;

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
            var ValorABuscar=document.getElementById("ctl00_ContentPlaceHolderContenido_txtBuscarPoblacion").value;
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
          //alert('x:' + curleft + '_y:' + curtop);
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
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Listado de Proveedores Averias y Visitas" 
            meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>

    <asp:ScriptManager ID="SM" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="Resultados" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divScroll" 
                style="position:absolute;overflow:scroll;width: 961px;height: 271px; top: 232px; left: 2px; border: 1px solid #000000; z-index:9995" 
                runat="server">
                <table ID="Table2" runat="server">
                    <tr runat="server">
                        <td runat="server"><asp:Label ID="Label2" CssClass="labelFormulario" runat="server" 
                                Text="PROVEEDOR: " Font-Bold="True"></asp:Label></td>
                        <td runat="server"><asp:TextBox ID="txtProveedor" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr runat="server">
                        <td runat="server"><asp:Label ID="Label3" CssClass="labelFormulario" runat="server" 
                                Text="PROVEEDOR AVERIA " Font-Bold="True"></asp:Label></td>
                        <td runat="server"><asp:TextBox ID="txtProveedorAveria" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAvisoSupervision1" CssClass="labelFormulario" runat="server" 
                                Text="PROVEEDOR SUPERVISION VISITAS: " Font-Bold="True"></asp:Label>&nbsp&nbsp&nbsp&nbsp
                            <asp:Label ID="lblAvisoSupervision2" CssClass="labelFormulario" runat="server" 
                                Text="APPLUS " Font-Bold="True" Font-Size="16px"></asp:Label>
                            <asp:Label ID="lblAvisoSupervision3" CssClass="labelFormulario" ForeColor="Red" runat="server" 
                                Text="ES EL PROVEEDOR ACTUAL DE LAS VISITAS DE SUPERVISION DEL SERVICIO MANTENIMIENTO GAS IBERDROLA" Font-Bold="True"></asp:Label>
                        </td>
                   </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="div3" 
    style="position: absolute; top: 25px; left: 535px; width: 154px;">
            <asp:PlaceHolder ID="PlaceHolder5" runat="server">
            <table ID="Table1" runat="server">
            <tr>
                <td><asp:Label ID="Label1" CssClass="labelFormulario" visible="true" runat="server" Text="C.P.: "></asp:Label></td>
                <td><asp:TextBox ID="txtCP" runat="server"></asp:TextBox></td>
            </tr>
            </table>
            </asp:PlaceHolder>
        </div>
     <div id="divBotonRealizarConsulta" 
        style="position: absolute; top: 117px; left: 847px;">
        <asp:Button CssClass="botonFormulario" ID="btnLimpiar" runat="server" Text="Limpiar"
            TabIndex="1"  OnClientClick="MostrarCapaEspera();" 
             onclick="btnLimpiar_Click" meta:resourcekey="btnLimpiarResource1"/> <br /><br />
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
    <div id="divBotonExcel" style="visibility:hidden;position:absolute; top: 534px; left: 369px; height: 23px; width: 258px;">
         <input type="button" id="btnInputExcel" visible="false" name="btnInputExcel" class="botonFormulario" value="Exportar a Excel" onclick="javascript:ExportarExcel();return false;" tabindex="5" onclick="return btnInputExcel_onclick()" onclick="return btnInputExcel_onclick()" />
    </div>
    
    <div style="position:absolute">
        <table>
            <tr>
                <td class="labelTabla" style="width: 53px; height: 135px;">
                    <asp:Label CssClass="labelFormulario" ID="lblProvincia" height="100%" 
                        runat="server" Text="Provincia: " meta:resourcekey="lblProvinciaResource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 100; height: 80px;">
                
                    <asp:UpdatePanel ID="UPProvincia" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                        <asp:TextBox ID="txtBuscarProvincia" runat="server" 
                                onkeyup="PosicionarProvincia();" Width="180px" 
                                meta:resourcekey="txtBuscarProvinciaResource1"></asp:TextBox>
                            <asp:Panel ID="pnLista" runat="server" Height="117px" ScrollBars="Auto" 
                                Wrap="False" Width="180px" BackColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" meta:resourcekey="pnListaResource1">
                                    <asp:RadioButtonList ID="chkProvincias" runat="server" 
                                        AutoPostBack="True" OnSelectedIndexChanged="cmbProvincias_SelectedIndexChanged"
                                        TabIndex="5" onFocus="ActualizarControlFoco(this.id);" 
                                        CssClass="campoFormulario" BackColor="White" Height="16px" Width="145px" 
                                        meta:resourcekey="chkProvinciasResource1">
                                    </asp:RadioButtonList>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="labelTabla" style="width: 51px; height: 135px;">
                    <asp:Label CssClass="labelFormulario" height="100%" ID="lblPoblacion" 
                        runat="server" Text="Población: " meta:resourcekey="lblPoblacionResource1"></asp:Label>
                </td>
                <td class="campoTabla" 
                    style="width: 100px; margin-left: 40px; height: 80px;">
                    <asp:TextBox ID="txtBuscarPoblacion" runat="server" 
                        onkeyup="PosicionarPoblacion();" Width="180px" 
                        meta:resourcekey="txtBuscarPoblacionResource1"></asp:TextBox>
                    <asp:UpdatePanel ID="UpdatePanelPoblaciones" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel3" runat="server" Height="117px" ScrollBars="Auto" 
                                Wrap="False" Width="180px" BackColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" meta:resourcekey="Panel3Resource1">
                                    <asp:RadioButtonList ID="chkPoblaciones" runat="server" 
                                        TabIndex="5" onFocus="ActualizarControlFoco(this.id);" 
                                        CssClass="campoFormulario" BackColor="White" Height="16px" Width="145px" 
                                        onselectedindexchanged="chkPoblaciones_SelectedIndexChanged" 
                                        meta:resourcekey="chkPoblacionesResource1">
                                    </asp:RadioButtonList>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
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
        function btnInputExcel_onclick() {

        }
    </script>
</asp:Content>