<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGasSinMenuCartasEnviadas.master" AutoEventWireup="true" CodeFile="FrmCartasEnviadasViaExcel.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmCartasEnviadasViaExcel" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="Iberdrola.Commons.Web" %>

<asp:Content ID="FrmContratos" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <script src="HTML/Js/jquery.js" type="text/javascript"></script>
    <script src="HTML/Js/jquery.filestyle.mini.js" type="text/javascript"></script>
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> 
    <LINK href="../css/ventan-modal-ie.css" type="text/css" rel="stylesheet">
	<script src="../js/ventana-modal.js" type="text/javascript"></script>    
	
    <script type="text/javascript">
    <% 
        if(NavigationController.IsBackward())
        { 
    %>
            MostrarCapaEspera();
    <%
        }
    %>
    
        function ClickPaginacion(ident)
        {
            var botonPaginacion = document.getElementById("ctl00$ContentPlaceHolderContenido$btnPaginacion");
            var hiddenPaginacion = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnPageIndex");
            hiddenPaginacion.value = ident;
            botonPaginacion.click();
            return false;
        }
        
        
    </script>
   

    
    <style type="text/css">
    .file_1 
    {
	    BORDER-TOP-WIDTH: 1px; 
	    BORDER-RIGHT-WIDTH: 1px; 
	    BORDER-LEFT-WIDTH: 1px; 
	    BORDER-BOTTOM-WIDTH: 1px; 
	    BORDER-TOP-COLOR: #808000; 
	    BORDER-RIGHT-COLOR: #808000; 
	    BORDER-LEFT-COLOR: #808000; 
	    BORDER-BOTTOM-COLOR: #808000; 
	    BORDER-TOP-STYLE: solid; 
	    BORDER-RIGHT-STYLE: solid; 
	    BORDER-LEFT-STYLE: solid; 
	    BORDER-BOTTOM-STYLE: solid
    }
    </style>

    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Cartas enviadas" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
    
    <div id="divFiltroBusqueda" style="position: absolute; top: 66px; left: 182px; height: 256px; width: 573px;">
     <asp:Panel ID="pnBusqueda" runat="server" 
            Height="137px" width="562px" CssClass="labelFormulario" 
            meta:resourcekey="pnBusquedaResource1">
        <div style="width: 460px; position:absolute; top: -24px; left: 56px; background-color: #FFFFFF; height: 142px;">
            &nbsp;<br /><br />
            <asp:FileUpload class="file_1" ID="FileUpload1" runat="server" 
                Width="199px" BackColor="White" CssClass="file_1" 
                ForeColor="Black" meta:resourcekey="FileUpload1Resource1" />
                <br /><br />
            <asp:CheckBox Visible="False" ID="chkBorrar" runat="server" 
                Text =" Borrar el archivo una vez procesado." Font-Bold="True" 
                ForeColor="Firebrick" meta:resourcekey="chkBorrarResource1" />
            <br />
            &nbsp;<br />
            <asp:Button ID="Button1" runat="server" Text="PROCESAR ARCHIVO" 
                onclick="Button1_Click" OnClientClick="MostrarCapaEspera();" 
                meta:resourcekey="Button1Resource1" />
                <br /><br />
           
        </div>
        <div style="position:absolute; top: -24px; left: -144px; width: 199px; height: 138px; background-color: #FFFFFF;">
            <asp:Image ID="Image1" runat="server" 
                ImageUrl="~/Imagenes/fileupload.jpg" meta:resourcekey="Image1Resource1" /></div>

      </asp:Panel>
    </div>
    
    <div style="height: 287px; position:absolute; top: 201px; left: 44px; width: 893px;">
    <!--<asp:Panel ID="Panel1" runat="server" Height="293px" ScrollBars="Auto" 
        Width="954px" CssClass="panelTabla" HorizontalAlign="Left" Wrap="False">-->
        <asp:Label ID="lblFechaDesde" runat="server" Text="Fecha Desde:" CssClass="labelFormulario"></asp:Label>
        <asp:TextBox ID="txtFechaDesde" CssClass="campoFormulario" width="63px" runat="server" MaxLength="10" onkeypress="if(window.event.keyCode == 13){ RealizarConsulta();return false;}"/>
        <img id="imgFechaDesde" runat="server" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" onclick="ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaDesde');" >
        <asp:Label ID="lblFechaHasta" runat="server" Text="Fecha Hasta:" CssClass="labelFormulario"></asp:Label>
        <asp:TextBox ID="txtFechaHasta" CssClass="campoFormulario" width="63px" runat="server" MaxLength="10" onkeypress="if(window.event.keyCode == 13){ RealizarConsulta();return false;}"/>
        <img id="imgfechaHasta" runat="server" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" onclick="ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaHasta');" >
        <asp:Button ID="Button2" CssClass="botonFormulario" runat="server" 
            onclick="Button2_Click" Text="Buscar" Width="325px" 
            meta:resourcekey="Button2Resource1" />
        <br /><br />
        <div style="border: 1px solid #000000; overflow:scroll; height:294px">
        <asp:GridView ID="dgDatos" runat="server" CssClass="contenidoTabla" Width="889px" 
                Height="16px" EnableModelValidation="True" meta:resourcekey="dgDatosResource1" > 
            <RowStyle CssClass="filaNormal"/>
            <AlternatingRowStyle CssClass="filaAlterna"/>
            
            <FooterStyle CssClass="cabeceraTabla"  />
            <PagerStyle  CssClass="cabeceraTabla" />
            <HeaderStyle CssClass="cabeceraTabla" />
        </asp:GridView>
        </div>
      <!--</asp:Panel>-->
    </div>

    <div id="divContador" 
        style="position:absolute; top: 534px; left: 48px; height: 18px; width: 329px" 
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
        <asp:HiddenField ID="hdnSortColumn" runat="server" />
        <asp:HiddenField ID="hdnPageCount" runat="server" />
        <asp:HiddenField ID="hdnPageIndex" runat="server" />
        <asp:HiddenField ID="hdnRecargarUltimaConsulta" runat="server" />
        <asp:Button ID="btnPaginacion" runat="server"  OnClick="OnBtnPaginacion_Click" 
            Width="0px" meta:resourcekey="btnPaginacionResource1"/> 
    </div>
    
    
    <div id="divBotonExcel" style="position:absolute; top: 545px; left: 350px; height: 50px; width: 115px;">
         <input runat="server" type=button id="Button3" name="btnInputExcel" class="botonFormulario" value="Exportar a Excel" onclick="OnBtnInputExcel1('excel')" tabindex="5" />
                
         <asp:Button ID="btnExcel1" runat="server" CssClass="botonFormulario" 
             Text="Exportar a Excel" Width="0px"  Height="0px" 
             OnClientClick="javascript:ExportarExcel();return false;" 
             meta:resourcekey="btnExcel1Resource1" />
    </div>
    
    
    
        
    <script type="text/javascript">
        $("input[type=file]").filestyle({ 
            image: "HTML/Images/choose-file.gif",
            imageheight : 22,
            imagewidth : 90
            ,width : 234
        });
    </script>
    <script type="text/javascript">
        function ExportarExcel() 
        {
            abrirVentanaLocalizacion("../UI/AbrirExcel.aspx", 600, 350, "ventana-modal","EXPORTAR EXCEL","0","1");
        }
           
        function OnBtnInputExcel1(accion)
        {
            var hdnRecargarUltimaConsulta = document.getElementById('ctl00$ContentPlaceHolderContenido$hdnRecargarUltimaConsulta');
            hdnRecargarUltimaConsulta.value = 'TRUE';
            
            if (accion == "excel")
            {
                var btnAccion = document.getElementById("ctl00$ContentPlaceHolderContenido$btnExcel1");
            }
            btnAccion.click();
        }
    </script>
    
</asp:Content>