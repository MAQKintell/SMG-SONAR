<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmDetalleVisita.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmDetalleVisita" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmDetalleVisita" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <link href="../css/ventana-modal.css" type="text/css" rel="stylesheet" /> 
    <link href="../css/ventan-modal-ie.css" type="text/css" rel="stylesheet" />
    <script src="../js/ventana-modal.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ActualizarControlFoco(nombreControl) {
            var hdnControlFoco = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnControlFoco");
            if (hdnControlFoco) {
                hdnControlFoco.value = nombreControl;
            }
        }

        function EstadoVisitaSelectedChange() {
            combo = document.getElementById('ctl00_ContentPlaceHolderContenido_cmbEstadoVisita');
            indiceEstadoAnterior = document.getElementById('ctl00_ContentPlaceHolderContenido_hdnEstadoAnterior');

            realizarCambios = true;

            if (mostrarConfirm) {
                if (!confirm("<%=Resources.TextosJavaScript.TEXTO_DE_SEGUIR_BORRARAN_DATOS_VISITA%>")) {//'De seguir con el cambio se borraran los datos correspondientes a la vista\n¿Desea continuar?')) {
                    realizarCambios = false;
                }
            }

            if (realizarCambios) {
                indiceEstadoAnterior.value = combo.selectedIndex;
                setTimeout('__doPostBack(\'ctl00_ContentPlaceHolderContenido_cmbEstadoVisita\',\'\')', 0);
            }
            else {
                indice = parseInt(indiceEstadoAnterior.value);

                combo.options(indice).selected = true;
            }
        }

        //Kintell 16/07/2010
        function PosicionarTecnico() {
            var ValorABuscar = document.getElementById("ctl00_ContentPlaceHolderContenido_txtTecnico").value;
            var Nombres = ctl00_ContentPlaceHolderContenido_cboTecnico.getElementsByTagName("label");
            var Esta = false;
            //var cont=document.getElementById("ctl00$ContentPlaceHolderContenido$hdnContadorProvincia").value;

            for (var i = 0; i < 1000; i++) {
                var Nombre = Nombres[i].innerText;
                var a1 = Nombre.substring(0, ValorABuscar.length);
                var a2 = ValorABuscar;

                if ((a1.toUpperCase() == a2.toUpperCase() || a1 == a2) && Esta == false) {
                    //Posicionar.
                    ctl00_ContentPlaceHolderContenido_pnLista.scrollTop = getXY(Nombres[i]) - 402;
                    Esta = true;
                    break;
                }
            }
        }

        function PosicionarTecnico1() {
            var ValorABuscar = document.getElementById("ctl00_ContentPlaceHolderContenido_txtTecnico").value;
            var Nombres = ctl00_ContentPlaceHolderContenido_cboTecnico.getElementsByTagName("label");
            var Esta = false;
            //var cont=document.getElementById("ctl00$ContentPlaceHolderContenido$hdnContadorProvincia").value;

            for (var i = 0; i < 1000; i++) {
                var Nombre = Nombres[i].innerText;
                var a1 = Nombre.substring(0, ValorABuscar.length);
                var a2 = ValorABuscar;

                if ((a1.toUpperCase() == a2.toUpperCase() || a1 == a2) && Esta == false) {
                    //Posicionar.
                    ctl00_ContentPlaceHolderContenido_pnLista.scrollTop = getXY(Nombres[i]) - 402;
                    Esta = true;
                    break;
                }
            }
            document.getElementById('divVisita').style.visibility = 'hidden';
            document.getElementById('TecnicoVisita').style.visibility = 'visible';
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

        function verTecnico() {
            document.getElementById('divVisita').style.visibility = 'hidden';
            document.getElementById('TecnicoVisita').style.visibility = 'visible';
            if (document.getElementById('ctl00_ContentPlaceHolderContenido_cmbEstadoVisita').selectedIndex == 6
            || document.getElementById('ctl00_ContentPlaceHolderContenido_cmbEstadoVisita').selectedIndex == 7) {
                document.getElementById('trFichero').style.visibility = 'visible';
            }
        }

        function verDatos() {
            document.getElementById('TecnicoVisita').style.visibility = 'hidden';
            document.getElementById('divVisita').style.visibility = 'visible';
            document.getElementById('trFichero').style.visibility = 'hidden';
        }

        function Ocultar() {
            document.getElementById('divAviso').style.visibility = 'hidden';
            return false;
        }
        function Mostrar() {
            document.getElementById('divAviso').style.visibility = 'visible';
            return false;
        }
    </script>

    <div style="position:absolute; top: 209px; left: 386px; height: 76px; width: 195px;z-index:1000">
        &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" OnClientClick="verDatos(); return false;" 
            ImageUrl="~/Imagenes/solicitud.gif" ToolTip="Datos de la visita" 
            Height="23px" Width="23px" meta:resourcekey="ImageButton2Resource1" />
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="verTecnico(); return false;" 
            ImageUrl="~/Imagenes/contact_large.gif" ToolTip="Datos del técnico" 
            meta:resourcekey="ImageButton1Resource1" />
        <br />
        <asp:Label CssClass="labelFormulario" ID="Label9" runat="server" Text="Visita." 
            Font-Bold="True" ForeColor="Blue" meta:resourcekey="Label9Resource1" />
        &nbsp; <asp:Label CssClass="labelFormulario" ID="Label8" runat="server" 
            Text="Técnico." Font-Bold="True" ForeColor="Blue" 
            meta:resourcekey="Label8Resource1" />
    </div>
    <div id="TecnicoVisita" 
        style="position:absolute; top: 236px; left: 11px; Z-Index:1;visibility:hidden">
        <table>
            <tr>
                <td style="width: 102px">
                    <asp:Label ID="Label3" CssClass="labelFormulario" runat="server" 
                        Text="Tipo Visita:" meta:resourcekey="Label3Resource1"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cboTipoVisita" runat="server" 
                        meta:resourcekey="cboTipoVisitaResource1">
                        <%--<asp:ListItem Text="........." Value="0"></asp:ListItem>--%>
                        <asp:ListItem Text="DEFECTOS MENORES" Value="DEFECTOS MENORES" 
                            meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="ANOMALIA SECUNDARIA" Value="ANOMALIA SECUNDARIA" 
                            meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Text="PRECINTADA" Value="PRECINTADO DE INSTALACION" 
                            meta:resourcekey="ListItemResource3"></asp:ListItem>
                        <asp:ListItem Text="SIN DEFECTOS" Value="SIN DEFECTOS" Selected="True" 
                            meta:resourcekey="ListItemResource4"></asp:ListItem>
                        <asp:ListItem Text="VISITA SIN ANOMALIA CON VALORES FUERA DE RANGO" Value="VISITA SIN ANOMALIA CON VALORES FUERA DE RANGO" 
                            meta:resourcekey="ListItemResourceTipoVisita5"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 102px">
                    <asp:Label ID="Label5" CssClass="labelFormulario" runat="server" 
                        Text="Técnico:" meta:resourcekey="Label5Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTecnico" onkeyup="PosicionarTecnico();" 
                        runat="server" Width="298px" meta:resourcekey="txtTecnicoResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 102px"></td>
                <td>
                    <asp:Panel ID="pnLista" runat="server" Height="117px" ScrollBars="Auto" 
                        Wrap="False" Width="298px" BackColor="White" BorderStyle="Solid" 
                        BorderWidth="1px" meta:resourcekey="pnListaResource1">
                        <asp:RadioButtonList ID="cboTecnico" runat="server" 
                            AutoPostBack="True" 
                            TabIndex="5" onFocus="ActualizarControlFoco(this.id);" 
                            CssClass="campoFormulario" BackColor="White" Height="16px" Width="288px" 
                            onselectedindexchanged="cboTecnico_SelectedIndexChanged" 
                            meta:resourcekey="cboTecnicoResource1">
                        </asp:RadioButtonList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 102px">
                    <asp:Label ID="Label6" CssClass="labelFormulario" runat="server" 
                        Text="Empresa:" meta:resourcekey="Label6Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmpresa" Enabled="False" runat="server" Width="250px" 
                        meta:resourcekey="txtEmpresaResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 102px">
                    <asp:Label ID="Label7" CssClass="labelFormulario" runat="server" 
                        Text="Observ. técnico:" meta:resourcekey="Label7Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtObservacionesVisita"  runat="server" TextMode="MultiLine" 
                        Height="32px" Width="289px" 
                        meta:resourcekey="txtObservacionesVisitaResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 102px">
                    <asp:Label ID="Label10" CssClass="labelFormulario" runat="server" 
                        Text="Contador Interior" meta:resourcekey="Label10Resource1"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbContador" runat="server" 
                        meta:resourcekey="cmbContadorResource1">
                    <asp:ListItem Text="....." Value="..." meta:resourcekey="ListItemResource5"></asp:ListItem>
                    <asp:ListItem Text="SI" Value="si" meta:resourcekey="ListItemResource6"></asp:ListItem>
                    <asp:ListItem Text="NO" Value="no" meta:resourcekey="ListItemResource7"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="errorFormulario" 
                        ErrorMessage="Debe de seleccionar si el contador es interno o externo" 
                        onservervalidate="CustomValidator1_ServerValidate" 
                        ControlToValidate="cmbContador" meta:resourcekey="CustomValidator1Resource1"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 102px">
                    <asp:Label ID="Label11" CssClass="labelFormulario" runat="server" 
                        Text="Código interno:" meta:resourcekey="Label11Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCodigoAlfanumerico" runat="server" Width="250px" 
                        meta:resourcekey="txtCodigoAlfanumericoResource1"></asp:TextBox>
                </td>
            </tr>
            <tr id="trFichero" style="visibility:hidden">
           
            <td>
                <asp:Label CssClass="labelFormulario" ID="lblFichero" runat="server" 
                                            Text="Fichero a Subir: " meta:resourcekey="lblFicheroResource1"></asp:Label>
                                            </td><td>
                <asp:FileUpLoad id="flFicheroVisita" runat="server" 
                    meta:resourcekey="flFicheroVisitaResource1" />
                    <asp:RegularExpressionValidator ID="reFicheroVisita" runat="server" ControlToValidate="flFicheroVisita"
             ErrorMessage="*" ToolTip="Solo se acpetan ficheros Tiff o Pdf"
             ValidationExpression="(.*\.([pP][dD][fF])|.*\.([tT][iI][fF][fF])$)" 
                    meta:resourcekey="reFicheroVisitaResource1"></asp:RegularExpressionValidator>
                    </td>
            </tr>
        </table>
    </div>
   
    <asp:ScriptManager ID="SM" runat= "server"></asp:ScriptManager>
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Visita asociada a la Instalación de Gas" 
            meta:resourcekey="lblTituloVentanaResource1" ></asp:Label>
    </div>
    <asp:HiddenField ID="hdnEstadoAnteriorVisita" runat="server" />
    <asp:HiddenField ID="hdnIdReparacion" runat="server"/>
    <asp:HiddenField ID="hdnControlFoco" runat="server" Value="ctl00_ContentPlaceHolderContenido_cmbEstadoVisita"/>
    <asp:HiddenField ID="hdnTecnicoValido" runat="server"/>
    <asp:HiddenField ID="hdnEsProteccion" runat="server"/>
    
    <div style="position:absolute; top: 39px; left: 126px; width: 473px; height: 33px; visibility:hidden"><div id="divEFV" style="position: relative; float: left; width: 115px;
                top: 10px; height: 91px; left: 10px;">
                <asp:Label CssClass="labelFormulario" ID="Label12" runat="server" Text="EFV:" 
                    meta:resourcekey="Label12Resource1"></asp:Label><br />
                <br />
            </div>
            <div id="divInformacionEFV" style="position: relative; top: 10px; float: left;
                left: 10px; height: 66px; width: 100px;">
                <asp:Label CssClass="labelFormularioValor" ID="ldlEFV" runat="server" 
                    meta:resourcekey="ldlEFVResource1"></asp:Label><br />
                <br />
                <br />
            </div></div>
    
    <div id="divPanelInformacionVisita" style="position: absolute; width: 450px; top: 35px;
        left: 4px; height: 85px;">
        <asp:HiddenField ID="hdnConfirm" runat="server" Value="true" />
        <asp:HiddenField ID="hdnEstadoAnterior" runat="server" />
        <asp:Panel ID="panelVisita" runat="server" GroupingText="Datos del contrato" CssClass="labelFormulario" Height="16px" Width="450px" meta:resourcekey="panelVisitaResource1">
            <div id="divLiteralesContrato" style="position: relative; float: left; width: 115px;
                top: 10px; height: 130px; left: 10px;">
                <asp:Label CssClass="labelFormulario" ID="lblContrato" runat="server" 
                    Text="Código Contrato:" meta:resourcekey="lblContratoResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormulario" ID="lblCampana" runat="server" 
                    Text="Código Campaña: " meta:resourcekey="lblCampanaResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormulario" ID="lblVisita" runat="server" 
                    Text="Código Visita: " meta:resourcekey="lblVisitaResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormulario" ID="lblLote" runat="server" 
                    Text="Código Lote: " meta:resourcekey="lblLoteResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormulario" ID="lblCodReceptor" runat="server" 
                    Text="Código Receptor: " meta:resourcekey="lblCodReceptorResource1"></asp:Label><br />
                <br />
            </div>
            <div id="divInformacionContrato" style="position: relative; top: 10px; float: left;
                left: 10px; height: 66px; width: 100px;">
                <asp:Label CssClass="labelFormularioValor" ID="lblIdContrato" runat="server" 
                    meta:resourcekey="lblIdContratoResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormularioValor" ID="lblIdCampana" runat="server" 
                    meta:resourcekey="lblIdCampanaResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormularioValor" ID="lblIdVisita" runat="server" 
                    meta:resourcekey="lblIdVisitaResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormularioValor" ID="lblIdLote" Width="370px" 
                    runat="server" Height="16px" meta:resourcekey="lblIdLoteResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormularioValor" ID="lblIdCUPS" runat="server" 
                    meta:resourcekey="lblIdCUPSResource1"></asp:Label><br />
                <br />
                <br />
            </div>
        </asp:Panel>
    </div>
    
    <div style="position:absolute; top: 192px; left: 5px;">
        <asp:Label CssClass="labelFormulario" ID="lbl3Dias" Visible="False" 
            runat="server" 
            Text="* No puede introducir Reparaciones debido a que desde la última vez en que modifico el estado de la visita a cerrada, han pasado mas de 3 dias" 
            Font-Bold="True" ForeColor="Red" meta:resourcekey="lbl3DiasResource1"></asp:Label></div>
    
    
    
    <div id="divBotonEquipamiento" style="position: relative; top: 125px; width: auto; height: 21px; left: 5px;"> <!--width: 358px; -->
                <asp:Button ID="btnHistoricoVisita" runat="server" Text="Historico visita" CssClass="botonFormulario"
                    Width="140px" OnClick="OnbtnHistoricoVisita_Click" 
                    meta:resourcekey="btnHistoricoVisitaResource1"  />
                <asp:Button ID="btnEquipamiento" runat="server" Text="Caldera / Equipamiento" CssClass="botonFormulario"
                    Width="150px" OnClick="OnBtnEquipamiento_Click" 
                    meta:resourcekey="btnEquipamientoResource1" />
                <asp:Button ID="btnTicketCombustion" runat="server" Text="Ticket combustion" CssClass="botonFormulario"
                    Width="140px" OnClick="OnbtnTicketCombustion_Click" 
                    meta:resourcekey="btnTicketCombustionResource1" visible="true"/>
                <asp:Button ID="btnTicketCombustionNew" runat="server" Text="Ticket combustion New" CssClass="botonFormulario"
                    Width="150px" OnClick="OnbtnTicketCombustionNew_Click" 
                    meta:resourcekey="btnTicketCombustionNewResource1" visible="false"/>
            </div>
    
    <div id="divPanelInformacionTitular" style="position: absolute; width: 966px; top: 35px;
        left: 460px; height: 110px;">
        <asp:Panel ID="panelTitular" runat="server" GroupingText="Datos del titular" Width="510px"
            Height="123px" CssClass="labelFormulario" 
            meta:resourcekey="panelTitularResource1">
            <div id="divLiteralesTitular" style="position: relative; float: left; top: 10px; height:130px;
                left: 10px;">
                <asp:Label CssClass="labelFormulario" ID="lblTitular" runat="server" 
                    Text="Nombre del Titular: " meta:resourcekey="lblTitularResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormulario" ID="lblDomicilio" runat="server" 
                    Text="Domicilio:" meta:resourcekey="lblDomicilioResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormulario" ID="lblTelefono" runat="server" 
                    Text="Teléfono: " meta:resourcekey="lblTelefonoResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormulario" ID="lblProvincia" runat="server" 
                    Text="Provincia: " meta:resourcekey="lblProvinciaResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormulario" ID="lblPoblacion" runat="server" 
                    Text="Población: " meta:resourcekey="lblPoblacionResource1"></asp:Label><br />
            </div>
            <div id="divInformacionTitular" style="position: relative; float: left; top: 10px;
                left: 34px;">
                <asp:Label CssClass="labelFormularioValor" ID="lblInfoTitular" runat="server" 
                    meta:resourcekey="lblInfoTitularResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormularioValor" ID="lblInfoDomicilio" runat="server" 
                    meta:resourcekey="lblInfoDomicilioResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormularioValor" ID="lblInfoTelefono" runat="server" 
                    meta:resourcekey="lblInfoTelefonoResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormularioValor" ID="lblInfoProvincia" runat="server" 
                    meta:resourcekey="lblInfoProvinciaResource1"></asp:Label><br />
                <asp:Label CssClass="labelFormularioValor" ID="lblInfoPoblacion" runat="server" 
                    meta:resourcekey="lblInfoPoblacionResource1"></asp:Label><br />
                <br />
                <br />
            </div>
            
            <div style="position:absolute; top: 71px; left: 244px; width: 417px;">
                <asp:Label CssClass="labelFormulario" ID="Label1" runat="server" 
                    Text="Teléfono Contacto 1: " meta:resourcekey="Label1Resource1"></asp:Label>
                <asp:TextBox ID="txtTelefono1" runat="server" 
                    meta:resourcekey="txtTelefono1Resource1"></asp:TextBox>
                <br /><asp:Label CssClass="labelFormulario" ID="Label2" runat="server" 
                    Text="Teléfono Contacto 2: " meta:resourcekey="Label2Resource1"></asp:Label>
                <asp:TextBox ID="txtTelefono2" runat="server" 
                    meta:resourcekey="txtTelefono2Resource1"></asp:TextBox>
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnActualizarTelefonos" runat="server" OnClick="OnBtnActualizarTelefonos_Click" 
                    CssClass="botonFormulario" Text="Actualizar Telefonos" 
                    meta:resourcekey="btnActualizarTelefonosResource1" />
            </div>
        </asp:Panel>
    </div>
    <div id="divScoring" 
        style="position: relative; float: left; top: 24px; left: 369px;">
        <asp:Label ID="Label20" Font-Underline="True" runat="server" 
            CssClass="labelFormularioValor" Text="SCORING:" Font-Bold="True" 
            ForeColor="#FF6600" meta:resourcekey="Label20Resource1"></asp:Label>
        <asp:Label ID="lblScoring" runat="server" ForeColor="Black" 
            CssClass="labelFormularioValor"  Text="S" 
            meta:resourcekey="lblScoringResource1"></asp:Label>
    </div>
            
    <div id="divTipoMantenimiento" style="position: absolute; top: 48px; left: 7px;
        width: 812px; height: 33px;">
        <asp:Label CssClass="labelFormulario" ForeColor="Red" ID="lblTipoMantenimiento" 
            runat="server" Text="Tipo de Mantenimiento: " 
            meta:resourcekey="lblTipoMantenimientoResource1"></asp:Label>
        <asp:Label CssClass="labelFormularioValor" ID="lblInfoTipoMantenimiento" runat="server"
            Text="MTO Gas Calefacción " 
            meta:resourcekey="lblInfoTipoMantenimientoResource1"></asp:Label></div>
    
    <div id="divAviso" 
        style="position: absolute; top: 87px; left: 76px;
        width: 874px; height: 33px; background-color:Red; border:thin solid Black; visibility:hidden;" >        
        <asp:Label CssClass="labelFormularioValor" ForeColor="White" ID="lblAviso" 
            runat="server" meta:resourcekey="lblAvisoResource1"></asp:Label><br />
        <asp:ImageButton ID="ImageButton3" runat="server" 
            ImageUrl="~/Imagenes/cerrar.gif" style="cursor:pointer" 
            OnClientClick="Ocultar();return false;" 
            meta:resourcekey="ImageButton3Resource1"  />
    </div>
    
    <div id="divPanelVisita" style="position: absolute; top: 200px; width: 98%; height:800px; left: 5px;z-index:0">
        <asp:Panel ID="panel1" runat="server" GroupingText="Datos de la visita" Width="100%" Height="100%"
            CssClass="labelFormulario" meta:resourcekey="panel1Resource1">
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
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
        </asp:Panel>
    </div>
    
    <div id="divContenidoPanelVisita" style="position: absolute; top: 200px; width: 100%;
        left: 5px; height: 342px;">
        <asp:UpdatePanel ID="UPContenidoReparacion" runat="server" >
            <ContentTemplate>
                <div id="divVisita" style="position: relative; width: 430px; top: 36px; left: 7px;
                    height: 260px; margin-bottom: 0px;visibility:visible;">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="lblEstadoVisita" runat="server" 
                                    Text="Estado Visita: " meta:resourcekey="lblEstadoVisitaResource1"></asp:Label>
                            </td>
                            <td class="campoFormulario"  colspan="2">
                                <asp:DropDownList ID="cmbEstadoVisita" runat="server" 
                                    
                                    onchange="EstadoVisitaSelectedChange()" 
                                    onFocus="ActualizarControlFoco(this.id);" 
                                    onselectedindexchanged="cmbEstadoVisita_SelectedIndexChanged" 
                                    meta:resourcekey="cmbEstadoVisitaResource1">
                                </asp:DropDownList>
                                 <asp:CustomValidator ID="cmbEstadoVisitaCustomValidator" 
                                    CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                                    ControlToValidate="cmbEstadoVisita" 
                                    OnServerValidate="OnCmbEstadoVisita_ServerValidate" ValidateEmptyText="True" 
                                    meta:resourcekey="cmbEstadoVisitaCustomValidatorResource1"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="lblFechaVisita" runat="server" 
                                    Text="Fecha Visita: " meta:resourcekey="lblFechaVisitaResource1"></asp:Label>
                                     
                            </td>
                            <td colspan="2">
                               
                                <asp:TextBox ID="txtFechaVisita" CssClass="campoFormularioFechaDetalleVisita" runat="server" 
                                    AutoPostBack="True" ontextchanged="txtFechaVisita_TextChanged" 
                                    onFocus="ActualizarControlFoco(this.id);" MaxLength="10"  
                                    meta:resourcekey="txtFechaVisitaResource1"></asp:TextBox><img id="imgFechaVisita" src="./HTML/IMAGES/imagenCalendario.gif" onclick="if (!document.getElementById('ctl00_ContentPlaceHolderContenido_txtFechaVisita').disabled)ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaVisita');">
                                
                                <asp:CustomValidator ID="txtFechaVisitaCustomValidator" 
                                    CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                                    ControlToValidate="txtFechaVisita" 
                                    OnServerValidate="OnTxtFechaVisita_ServerValidate" ValidateEmptyText="True" 
                                    meta:resourcekey="txtFechaVisitaCustomValidatorResource1"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="lblFechaLimite" runat="server" 
                                    Text="Fecha Límite: " meta:resourcekey="lblFechaLimiteResource1"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtFechaLimite" class="campoFormularioFecha" runat="server" 
                                    ReadOnly="True" Enabled="False" MaxLength="10" 
                                    meta:resourcekey="txtFechaLimiteResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="lblUrgencia" runat="server" 
                                    Text="Urgencia: " meta:resourcekey="lblUrgenciaResource1"></asp:Label><br />
                            </td>
                            <td>
                                <asp:Label CssClass="labelFormularioValor" ID="lblValorUrgencia" runat="server" 
                                    Text="Urgente" meta:resourcekey="lblValorUrgenciaResource1"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkPRL" Enabled="False" 
                                    CssClass="campoFormulario" runat="server" Text="PRL" 
                                    meta:resourcekey="chkPRLResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="lblObservaciones" runat="server" 
                                    Text="Observaciones: " meta:resourcekey="lblObservacionesResource1"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtObservaciones" runat="server" Width="292px" TextMode="MultiLine"
                                    Rows="2" CssClass="campoFormulario" 
                                    onFocus="ActualizarControlFoco(this.id);" MaxLength="255" 
                                    meta:resourcekey="txtObservacionesResource1"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton4" runat="server" Height="20px" 
                                    ImageUrl="~/UI/HTML/Images/warning.png" Width="17px" 
                                    OnClientClick="Mostrar();" AlternateText="AVISOS" Visible="False" 
                                    meta:resourcekey="ImageButton4Resource1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                &nbsp;
                            </td>
                            <td class="labelFormulario"  colspan="2">
                                <asp:CheckBox ID="chkRecepcionComprobante" runat="server" 
                                    Text="Recepción del comprobante" CssClass="campoFormulario"  
                                    AutoPostBack="True" meta:resourcekey="chkRecepcionComprobanteResource1"/>
                                    
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                &nbsp;
                            </td>
                            <td class="labelFormulario"  colspan="2">
                                  <asp:CheckBox ID="chkFacturadoProveedor" runat="server" 
                                      Text="Facturado por el proveedor" CssClass="campoFormulario"  
                                      AutoPostBack="True" 
                                      oncheckedchanged="chkFacturadoProveedor_CheckedChanged" 
                                      meta:resourcekey="chkFacturadoProveedorResource1"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="lblFechaFactura" runat="server" 
                                    Text="Fecha Factura: " meta:resourcekey="lblFechaFacturaResource1"></asp:Label>
                            </td>
                            <td  colspan="2">
                                <asp:TextBox CssClass="campoFormularioFecha" ID="txtFechaFactura" onFocus="ActualizarControlFoco(this.id);"
                                    runat="server" OnTextChanged="txtFechaFactura_TextChanged" 
                                    AutoPostBack="True" MaxLength="10" Enabled="False" 
                                    meta:resourcekey="txtFechaFacturaResource1"></asp:TextBox>
                                <img id="imgFechaFactura" src="./HTML/IMAGES/imagenCalendario.gif" 
                                    onclick="if (!document.getElementById('ctl00_ContentPlaceHolderContenido_txtFechaFactura').disabled)ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaFactura');" 
                                    style="visibility: hidden"/>

                                <asp:CustomValidator ID="txtFechaFacturaCustomValidator" 
                                    CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                                    ControlToValidate="txtFechaFactura" 
                                    OnServerValidate="OnTxtFechaFactura_ServerValidate" ValidateEmptyText="True" 
                                    meta:resourcekey="txtFechaFacturaCustomValidatorResource1"></asp:CustomValidator>

                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="lblNumFactura" runat="server" 
                                    Text="Número Factura: " meta:resourcekey="lblNumFacturaResource1"></asp:Label>
                            </td>
                            <td  colspan="2">
                                <asp:TextBox ID="txtNumFactura" Enabled="False" runat="server" Width="75px" 
                                    AutoPostBack="True" onFocus="ActualizarControlFoco(this.id);" MaxLength="50" 
                                    meta:resourcekey="txtNumFacturaResource1" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="lblCodigoBarras" runat="server" 
                                    Text="Código de Barras: " Font-Bold="False" Font-Italic="False" 
                                    Font-Underline="False" meta:resourcekey="lblCodigoBarrasResource1"></asp:Label>
                            </td>
                            <td  colspan="2">
                                <asp:TextBox ID="txtInfoCodigoBarras" runat="server" Width="236px" 
                                    onFocus="ActualizarControlFoco(this.id);" MaxLength="14" 
                                    meta:resourcekey="txtInfoCodigoBarrasResource1"></asp:TextBox>
                                <asp:RegularExpressionValidator id="txtInfoCodigoBarras_validation" 
                                     runat="server" 
                                     ControlToValidate="txtInfoCodigoBarras" 
                                     ErrorMessage="*" 
                                     ToolTip = "Introduzca solo caracteres numericos" 
                                     ValidationExpression="[0-9]*" 
                                     CssClass="errorFormulario" enabled="false"
                                    meta:resourcekey="txtInfoCodigoBarras_validationResource1"></asp:RegularExpressionValidator> 
                                    
                                <asp:RegularExpressionValidator id="txtInfoCodigoBarras_13digitos" 
                                     ControlToValidate="txtInfoCodigoBarras" 
                                     ValidationExpression="\d{14}"
                                     ErrorMessage="*" enabled="false"
                                     ToolTip = "Código de Barras debe tener 14 dígitos"
                                     runat="server"
                                     CssClass="errorFormulario" 
                                    meta:resourcekey="txtInfoCodigoBarras_13digitosResource1"/>
                                </asp:RegularExpressionValidator> 
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                
                            </td>
                            <td  colspan="2"></td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="Label4" runat="server" 
                                    Text="Fecha Envío Carta: " meta:resourcekey="Label4Resource1"></asp:Label>
                            </td>
                            <td  colspan="2">                                
                                <asp:TextBox ID="txtFechaEnvioCarta" runat="server" 
                                    CssClass="campoFormularioFecha" Enabled="False" MaxLength="10" 
                                    onFocus="ActualizarControlFoco(this.id);" ReadOnly="True" 
                                    meta:resourcekey="txtFechaEnvioCartaResource1"></asp:TextBox>
                                
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td  colspan="2">
                                <asp:CheckBox ID="chkInformacionVisita" Enabled="False" 
                                    CssClass="campoFormulario" runat="server" Text="Información Recibida" 
                                    meta:resourcekey="chkInformacionVisitaResource1" />
                                <asp:CheckBoxList ID="chkListCartaEnviada" runat="server" CssClass="campoFormulario"
                                    AutoPostBack="True" 
                                    onselectedindexchanged="chkListCartaEnviada_SelectedIndexChanged" 
                                    onFocus="ActualizarControlFoco(this.id);" 
                                    meta:resourcekey="chkListCartaEnviadaResource1">
                                    <asp:ListItem meta:resourcekey="ListItemResource8">Carta enviada</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelFormulario">
                                <asp:Label CssClass="labelFormulario" ID="Label13" runat="server" 
                                    Text="Categoria Visita: " meta:resourcekey="Label13Resource1"></asp:Label>
                            </td>
                            <td  colspan="2">                                
                                <asp:TextBox ID="txtCategoriaVisita" runat="server" 
                                    CssClass="campoFormularioFecha" Enabled="False" MaxLength="50" 
                                    onFocus="ActualizarControlFoco(this.id);" ReadOnly="True" Width="149px" 
                                    meta:resourcekey="txtCategoriaVisitaResource1"></asp:TextBox>
                                
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div id="divContenidoReparacion" style="position: absolute; width: 428px; top: 14px;
                    left: 487px; height: 312px;z-index:1500">
                    <table style="width: 478px; height: 306px;">
                        <tr>
                            <td valign ="bottom" style="width: 14px">
                                <asp:RadioButtonList ID="rdbReparacion" runat="server" CssClass="campoFormulario"
                                    AutoPostBack="True" Width="51px" onFocus="ActualizarControlFoco(this.id);" 
                                    onselectedindexchanged="rdbReparacion_SelectedIndexChanged" 
                                    meta:resourcekey="rdbReparacionResource1" >
                                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource9">No</asp:ListItem>
                                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource10">Si</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                           
                            <td colspan="2"><asp:CheckBox ID="chkInformacionReparacion" Enabled="False" 
                                    CssClass="campoFormulario" runat="server" Text="Información Recibida" 
                                    meta:resourcekey="chkInformacionReparacionResource1" /></td>
                            </tr>
                        <tr>
                            <td valign="bottom" style="width: 26px" colspan="2">
                                                        <asp:Label CssClass="labelFormulario" ID="lblTipoReparacion" runat="server" 
                                    Text="Tipo reparación: " meta:resourcekey="lblTipoReparacionResource1"></asp:Label>    

                            
                            
                            </td><td colspan="2">
                            
                                <asp:DropDownList ID="cmbTipoReparacion" runat="server" 
                                    Width="246px" 
                                    onselectedindexchanged="cmbTipoReparacion_SelectedIndexChanged" 
                                    onFocus="ActualizarControlFoco(this.id);" 
                                    meta:resourcekey="cmbTipoReparacionResource1">
                                </asp:DropDownList>
                                
                                <asp:RangeValidator CssClass="errorFormulario" 
                                    ToolTip="Debe Seleccionar un valor" ErrorMessage="*" ControlToValidate="cmbTipoReparacion"
 MaximumValue ="9999999" MinimumValue="0" ID="RangeValidator1" runat="server" 
                                    meta:resourcekey="RangeValidator1Resource1"></asp:RangeValidator>

                                
                                
                            </td>
                            </tr>
                        <tr>
                            <td style="width: 14px">
                            </td>
                            <td style="width: 118px">
                                <asp:Label CssClass="labelFormulario" ID="lblFechaReparacion" runat="server" 
                                    Text="Fecha de reparación: " meta:resourcekey="lblFechaReparacionResource1"></asp:Label>
                            </td>
                            <td>
                             <asp:TextBox CssClass="campoFormularioFechaDetalleVisita" ID="txtFechaReparacion" 
                                    runat="server" AutoPostBack="True"  onFocus="ActualizarControlFoco(this.id);"
                                    ontextchanged="txtFechaReparacion_TextChanged" MaxLength="10" 
                                    meta:resourcekey="txtFechaReparacionResource1"></asp:TextBox>
                                <img alt="Fecha Reparacion" id="imgFechaReparacion" src="./HTML/IMAGES/imagenCalendario.gif" onclick="if (!document.getElementById('ctl00_ContentPlaceHolderContenido_txtFechaReparacion').disabled)ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaReparacion');"/>
                                <asp:CustomValidator ID="txtFechaReparacionValidator" runat="server" 
                                    ControlToValidate="cmbEstadoVisita" CssClass="errorFormulario" ErrorMessage="*" 
                                    OnServerValidate="OnCmbEstadoVisita_ServerValidate" 
                                    ValidateEmptyText="True" 
                                    meta:resourcekey="txtFechaReparacionValidatorResource1"></asp:CustomValidator>
                                
                                    
                            </td>
                            </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label CssClass="labelFormulario" ID="lblServicio" runat="server" 
                                    Text="Incluido en el servicio " meta:resourcekey="lblServicioResource1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 14px">
                                &nbsp;
                            </td>
                            <td style="width: 118px">
                                <asp:Label CssClass="labelFormulario" ID="lblTiempoManoObra" runat="server" 
                                    Text="Tiempo Mano de Obra: " onFocus="ActualizarControlFoco(this.id);" 
                                    meta:resourcekey="lblTiempoManoObraResource1" Width="150"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbTiempoManoObra" runat="server" 
                                    onselectedindexchanged="cmbTiempoManoObra_SelectedIndexChanged"  
                                    onFocus="ActualizarControlFoco(this.id);" 
                                    meta:resourcekey="cmbTiempoManoObraResource1">
                                </asp:DropDownList>
                               
                                <asp:RangeValidator CssClass="errorFormulario" 
                                    ToolTip="Debe Seleccionar un valor" ErrorMessage="*" ControlToValidate="cmbTiempoManoObra"
 MaximumValue ="9999999" MinimumValue="0" ID="RegularExpressionValidator3" runat="server" 
                                    meta:resourcekey="RegularExpressionValidator3Resource1"></asp:RangeValidator>
                                
    
                                
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 14px">
                                &nbsp;
                            </td>
                            <td style="width: 118px">
                                <asp:Label CssClass="labelFormulario" ID="lblCosteMariales" runat="server" 
                                    Text="Coste de materiales: " meta:resourcekey="lblCosteMarialesResource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCosteMateriales" runat="server" Width="70px" 
                                    ontextchanged="txtCosteMateriales_TextChanged"  
                                    onFocus="ActualizarControlFoco(this.id);" MaxLength="12" 
                                    CssClass="campoFormularioNumerico" 
                                    meta:resourcekey="txtCosteMaterialesResource1"></asp:TextBox>
                               <span class="labelFormulario">€</span>
                               &nbsp;&nbsp;<asp:RegularExpressionValidator CssClass="errorFormulario" 
                                    ID="RegularExpressionValidatorCoste" runat="server" 
                                              ErrorMessage="*" ToolTip="El valor debe de ser númerico" 
                                              ControlToValidate="txtCosteMateriales" 
                                              ValidationExpression="\d*\,?\d*" 
                                    meta:resourcekey="RegularExpressionValidatorCosteResource1"></asp:RegularExpressionValidator>

                                    
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label CssClass="labelFormulario" ID="lblAdicionalCliente" runat="server" 
                                    Text="Adicional Cobrado al Cliente " 
                                    meta:resourcekey="lblAdicionalClienteResource1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 14px">
                                &nbsp;
                            </td>
                            <td style="width: 118px">
                                <asp:Label CssClass="labelFormulario" ID="lblTiempoManoObraCliente" runat="server"
                                    Text="Importe Mano de Obra: " meta:resourcekey="lblTiempoManoObraClienteResource1"
                                    Width="150"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtImporteManoObra" runat="server" Width="70px" 
                                    ontextchanged="txtImporteManoObra_TextChanged"  
                                    onFocus="ActualizarControlFoco(this.id);" MaxLength="12" 
                                    CssClass="campoFormularioNumerico" 
                                    meta:resourcekey="txtImporteManoObraResource1"></asp:TextBox>
                                <span class="labelFormulario">€</span>
                               &nbsp;&nbsp;<asp:RegularExpressionValidator CssClass="errorFormulario" 
                                    ID="RegularExpressionValidator1" runat="server" 
                                              ErrorMessage="*" ToolTip="El valor debe de ser númerico" 
                                              ControlToValidate="txtImporteManoObra" 
                                              ValidationExpression="\d*\,?\d*" 
                                    meta:resourcekey="RegularExpressionValidator1Resource1"></asp:RegularExpressionValidator>
                               
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 14px">
                                &nbsp;
                            </td>
                            <td style="width: 118px">
                                <asp:Label CssClass="labelFormulario" ID="lblCosteMaterialesCliente" 
                                    runat="server" Text="Coste de materiales: " 
                                    meta:resourcekey="lblCosteMaterialesClienteResource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCosteMaterialesAdicional" runat="server" Width="70px" 
                                    ontextchanged="txtCosteMaterialesAdicional_TextChanged"  
                                    onFocus="ActualizarControlFoco(this.id);" MaxLength="12" 
                                    CssClass="campoFormularioNumerico" 
                                    meta:resourcekey="txtCosteMaterialesAdicionalResource1"></asp:TextBox>
                                <span class="labelFormulario">€</span>
                                &nbsp;&nbsp;<asp:RegularExpressionValidator CssClass="errorFormulario" 
                                    ID="RegularExpressionValidator2" runat="server" 
                                              ErrorMessage="*" ToolTip="El valor debe de ser númerico" 
                                              ControlToValidate="txtCosteMaterialesAdicional" 
                                              ValidationExpression="\d*\,?\d*" 
                                    meta:resourcekey="RegularExpressionValidator2Resource1"></asp:RegularExpressionValidator>
                               
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="3">
                                <asp:Label CssClass="labelFormulario" ID="lblFactura" runat="server" 
                                    Text="Información Factura " meta:resourcekey="lblFacturaResource1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 14px">
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label CssClass="labelFormulario" ID="lblFechaFacturaReparacion" runat="server"
                                                Text="Fecha Factura: " Width="100%" meta:resourcekey="lblFechaFacturaReparacionResource1"></asp:Label>
                                      </td><td>
                                            <asp:TextBox ID="txtFechaFacturaReparacion" runat="server"  Width="70px" 
                                                onFocus="ActualizarControlFoco(this.id);" MaxLength="10" 
                                                CssClass="campoFormularioFecha" 
                                                ontextchanged="txtFechaFacturaReparacion_TextChanged" Enabled="False" 
                                                meta:resourcekey="txtFechaFacturaReparacionResource1"></asp:TextBox>
                                            <img alt="calendario" id="img1" src="./HTML/IMAGES/imagenCalendario.gif" 
                                                onclick="if (!document.getElementById('ctl00_ContentPlaceHolderContenido_txtFechaReparacion').disabled)ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaFacturaReparacion');" 
                                                style="visibility: hidden"/>
                                        </td>
                                        <td>
                                            <asp:Label CssClass="labelFormulario" ID="lblNumeroFacturaReparacion" 
                                                runat="server" Text="Número de Factura: " Width="100%"
                                                meta:resourcekey="lblNumeroFacturaReparacionResource1"></asp:Label>
                                        </td><td>
                                            <asp:TextBox ID="txtNumeroFacturaReparacion" runat="server" Width="70px" 
                                                onFocus="ActualizarControlFoco(this.id);" MaxLength="12" 
                                                CssClass="campoFormularioFecha" 
                                                meta:resourcekey="txtNumeroFacturaReparacionResource1"></asp:TextBox>
                                            &nbsp;<asp:RegularExpressionValidator CssClass="errorFormulario" 
                                                ID="RegularExpressionValidator5" runat="server" 
                                                          ErrorMessage="*" ToolTip="El valor debe de ser númerico" 
                                                          ControlToValidate="txtNumeroFacturaReparacion" 
                                                          ValidationExpression="\d*\,?\d*" 
                                                meta:resourcekey="RegularExpressionValidator5Resource1"></asp:RegularExpressionValidator>
                                        </td>
                                   </tr>
                                </table>
                             </td>
                           </tr>
                        <tr>
                            <td style="width: 14px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="labelFormulario" ID="lblCodigoBarrasReparacion" 
                                    runat="server" Text="Código de barras: " Font-Bold="False" 
                                    Font-Italic="False" Font-Underline="False" 
                                    meta:resourcekey="lblCodigoBarrasReparacionResource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCodigoBarrasReparacion" runat="server" Width="236px" 
                                    onFocus="ActualizarControlFoco(this.id);" MaxLength="50" 
                                    CssClass="campoFormularioFecha" 
                                    meta:resourcekey="txtCodigoBarrasReparacionResource1"></asp:TextBox>
                                              
                               
                            </td>
                        
                        
                         
                            
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <div id="divBotonAceptarContratos" >
        <asp:Button CssClass="botonFormulario" ID="btnAceptar" runat="server" Text="Aceptar"
            OnClick="OnBtnAceptar_Click" meta:resourcekey="btnAceptarResource1" />
    </div>
    
    <div id="divBotonVolverContratos" style="position:absolute">
        <asp:Button CssClass="botonFormulario" CausesValidation="False" ID="btnVolver" 
            runat="server" Text="Volver"
            OnClick="OnBtnVolver_Click" meta:resourcekey="btnVolverResource1" />
    </div>

    <script type="text/javascript">
        function OnBtnEquipamiento_Click() {
            abrirVentanaLocalizacion("./FrmModalEquipamiento.aspx", "650", "450", "ModalEquipamiento", "<%=Resources.TextosJavaScript.TEXTO_EQUIPAMIENTO%>", "", true);
        }
        function OnbtnTicketCombustion_Click() {
            abrirVentanaLocalizacion("./FrmModalTicketCombustion.aspx", "600", "450", "ModalTicketCombustion", "<%=Resources.TextosJavaScript.TEXTO_EQUIPAMIENTO%>", "", true);
        }
        function OnbtnTicketCombustionNew_Click() {
            abrirVentanaLocalizacion("./FrmModalTicketCombustionNew.aspx", "600", "450", "ModalTicketCombustion", "<%=Resources.TextosJavaScript.TEXTO_EQUIPAMIENTO%>", "", true);
        }
        // carga de todos los calendarios

    </script>
    <asp:PlaceHolder ID="_PlaceHolderScript" runat="server"></asp:PlaceHolder>
</asp:Content>