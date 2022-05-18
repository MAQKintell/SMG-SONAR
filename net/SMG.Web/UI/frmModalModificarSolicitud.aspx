<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" CodeFile="frmModalModificarSolicitud.aspx.cs" Inherits="Iberdrola.SMG.UI.frmModalModificarSolicitud" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalGenerarVisitas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">

	<script src="../js/ventana-modal.js" type="text/javascript"></script>
    <script language="javascript">

        function InformarTermostato() {
            //R#36525
            if (document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica40").checked)
            {
                document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica41").style.visibility = "visible";
                document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica42").style.visibility = "visible";
                document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica43").style.visibility = "visible";
                document.getElementById("ctl00_ContentPlaceHolderContenido_labelCaracteristica41").style.visibility = "visible";
                document.getElementById("ctl00_ContentPlaceHolderContenido_labelCaracteristica42").style.visibility = "visible";
                document.getElementById("ctl00_ContentPlaceHolderContenido_labelCaracteristica43").style.visibility = "visible";
            }
            else
            {
                document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica41").style.visibility = "hidden";
                document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica42").style.visibility = "hidden";
                document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica43").style.visibility = "hidden";
                document.getElementById("ctl00_ContentPlaceHolderContenido_labelCaracteristica41").style.visibility = "hidden";
                document.getElementById("ctl00_ContentPlaceHolderContenido_labelCaracteristica42").style.visibility = "hidden";
                document.getElementById("ctl00_ContentPlaceHolderContenido_labelCaracteristica43").style.visibility = "hidden";
            }
    }

    function InformarNombreFichero() {
        var input = document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica2");
        document.getElementById("ctl00_ContentPlaceHolderContenido_hdnNombreficheroInspeccion").value = input.value;
    }

//    function MostrarCapaEspera() {

//        var div = document.getElementById("divSplashScreen");

//        div.style.visibility = "visible";

////        var div1 = document.getElementById("divbotonIniciarSesion");
////        div1.style.visibility = "hidden";
//    }
    
    function GuardarDatos() {
        MostrarCapaEspera();
        var input = document.getElementById("ctl00_ContentPlaceHolderContenido_btnAceptar");
        input.style.visibility = "hidden";

        var total = parseInt(document.getElementById("ctl00_ContentPlaceHolderContenido_hdnTotalCaracteristicas").value);
        var i = 0;
        document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCaracteristicasValores").value = "";
        document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCaracteristicasTitulo").value = "";

        for (i = 0; i < total; i++) {
            if (document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica" + i) != null) {
                if (document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica" + i).type == "checkbox") {
                    var valorCarac = document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica" + i).checked;
                }
                else {
                    if (document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica" + i).type == "select-one") {
                        var valorCarac = document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica" + i)[document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica" + i).selectedIndex].innerText;
                    }
                    else {
                        var valorCarac = document.getElementById("ctl00_ContentPlaceHolderContenido_Caracteristica" + i).value;
                    }
                }
                var labelCarac = document.getElementById("ctl00_ContentPlaceHolderContenido_labelCaracteristica" + i).innerText;

                document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCaracteristicasValores").value = valorCarac + ";" + document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCaracteristicasValores").value;
                document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCaracteristicasTitulo").value = labelCarac + ";" + document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCaracteristicasTitulo").value;
            }
        }
       // OcultarCapaEspera();
        input.style.visibility = "visible";
        return false;
    }

    function MostrarCodificacionAveria() {
        abrirVentanaLocalizacion1("FrmModalCodificacionAverias.aspx", 600, 400, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_CODIFICACION_AVERIAS%>", "0", "1");
    }

    function AbrirSolicitud(idSolicitud, codContrato) {
        abrirVentanaLocalizacion1("FrmModalAbrirSolicitudProveedor.aspx?telefono=" + document.getElementById("ctl00_ContentPlaceHolderContenido_txtTelefono").value + "&Contrato=" + document.getElementById("ctl00_ContentPlaceHolderContenido_hdnContrato").value, 400, 200, "ventana-modal", "ALTA", "1", "0");
    }

    function OnbtnTicketCombustion_Click() {
        abrirVentanaLocalizacion("./FrmModalTicketCombustion.aspx", "600", "450", "ModalTicketCombustion", "<%=Resources.TextosJavaScript.TEXTO_EQUIPAMIENTO%>", "", true);
    }

    function Cargacombo(ID, idOBJETO) {
        //alert('111');
        var seleccionado = document.getElementById("ctl00_ContentPlaceHolderContenido_" + idOBJETO).value;
        //alert(seleccionado);
        //alert(ID);
        document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodigoOperacion").value = ID;
        //alert(document.getElementById("ctl00_ContentPlaceHolderContenido_hdnCodigoOperacion").value);
        __doPostBack(ID, seleccionado);
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

    function AbrirVentanaAveriaCalderas(idSolicitud, codContrato) {
        //                        alert(idSolicitud);
        //            alert(codContrato);
        abrirVentanaLocalizacion(".aspx?idSolicitud=" + idSolicitud + "&Contrato=" + codContrato, 800, 550, "ventana-modal", "<%=Resources.TextosJavaScript.TEXTO_DATOS_SOLICITUD%>" + idSolicitud + "; CONTRATO: " + codContrato, "0", "1", true);
    }
    </script>
	<style>
     #divSplashScreen
    {
        position:absolute;
        top:175px;
        left:175px;
        height:100px;
        width:200px;
        border: solid 1px #000000;
        background-color:#ffffff;
        padding:5px 5px 5px 5px;
        z-index:100001;
        visibility:hidden;
        cursor:wait;
    }
</style>
 <div id="divSplashScreen">
        <table border="0" cellpadding="0" cellspacing="0" width="1%" align="center">
            <tr>
                <td align="center">
                     <img src="HTML/Images/loading.gif" alt="Cargando..."/>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <span class="labelFormularioValor"> Cargando... </span>
                </td>
            </tr>
        </table>
    </div>

    <asp:HiddenField ID="hdnSubtipo" runat="server" />
    <asp:HiddenField ID="hdnIdSolicitud" runat="server" />
    <asp:HiddenField ID="hdnCaracteristicaCalderasSeleccionado" runat="server" />
    <asp:HiddenField ID="hdnVieneDeAltaCalderas" runat="server" />
    <asp:HiddenField ID="hdnContrato" runat="server" />
    <asp:HiddenField ID="hdnMarcaCaldera" runat="server" />
    <asp:HiddenField ID="hdnDesTipoCaldera" runat="server" />
    <asp:HiddenField ID="hdnTipologiaCalderaGC" runat="server" />
    <asp:HiddenField ID="hdnTotalCaracteristicas" runat="server" />
    <asp:HiddenField ID="hdnCaracteristicasValores" runat="server" />
    <asp:HiddenField ID="hdnCaracteristicasTitulo" runat="server" />
    <asp:HiddenField ID="hdnEstadoInicialSolicitud" runat="server" />
    <asp:HiddenField ID="hdnAltaIncidenciaGasConfort" Value="0" runat="server" />
    <asp:HiddenField ID="hdnSolCent" runat="server" />
    <asp:HiddenField ID="hdnMostrarDatosBancarios" runat="server" />
    <asp:HiddenField ID="hdnMostrarDiaPago" runat="server" />
    <asp:HiddenField ID="hdnNombreficheroInspeccion" runat="server" />

    <asp:HiddenField ID="hdnSubtipoAveriaSobreGC" runat="server" />
    <asp:HiddenField ID="hdnCodigoOperacion" runat="server" />
    <asp:HiddenField ID="hdnVisibleUnidades1" runat="server" />
    <asp:HiddenField ID="hdnVisibleUnidades2" runat="server" />
    <asp:HiddenField ID="hdnInspeccion" runat="server" />
    <asp:HiddenField ID="hdnInspeccionExtra" runat="server" />
    <asp:HiddenField ID="hdnCodContrato" runat="server" />
    <asp:HiddenField ID="hdnNumSerie" runat="server" />
    <asp:HiddenField ID="hdnCampania" runat="server" />
    <asp:HiddenField ID="hdnMostrarFornecimientoGas" runat="server" />


    <div>
        <div class="panelTabla" style="border-style: none; border-color: inherit; border-width: 0px; position:absolute; top: 0px; left: 0px; background-color:White; height: 267px;">
            <asp:Panel BackColor="#DCE4D9" ID="Panel1" runat="server" Height="265px" 
                Width="790px" BorderWidth="0px" BorderStyle="None" 
                meta:resourcekey="Panel1Resource1">
            <table>
                <tr>
                    <td >
                        <asp:Label CssClass="labelFormulario" ID="Label7" runat="server" 
                            Text="Estado actual:" meta:resourcekey="Label7Resource1"></asp:Label>
                        <asp:TextBox ID="txtEstadoActual" runat="server" Width="175px" TabIndex="1" 
                            meta:resourcekey="txtEstadoActualResource1"></asp:TextBox>
                    </td>
                    <td >
                        <asp:Label CssClass="labelFormulario" ID="Label1" runat="server" 
                            Text="Motivo cancelación:" meta:resourcekey="Label1Resource1"></asp:Label>
                        <asp:DropDownList ID="ddl_MotivoCancel" runat="server" Font-Bold="True" onselectedindexchanged="ddl_MotivoCancel_SelectedIndexChanged" AutoPostBack="True"
                            Width="261px" Font-Size="Small" 
                            meta:resourcekey="ddl_MotivoCancelResource1"></asp:DropDownList>
                    </td>
                    <td  class="campoFormulario" colspan="2">
                        <asp:Label CssClass="labelFormulario" ID="Label2" runat="server" 
                            Text="Nuevo estado:" ForeColor="Red" Font-Bold="True" Font-Underline="True" 
                            meta:resourcekey="Label2Resource1"></asp:Label>
                      
                        <asp:DropDownList ID="ddl_EstadoSol" runat="server" AutoPostBack="True" 
                            Font-Size="Small" onselectedindexchanged="ddl_EstadoSol_SelectedIndexChanged" 
                            Width="215px" meta:resourcekey="ddl_EstadoSolResource1">
                        </asp:DropDownList>
                    </td>
                    <td > 
                         <asp:Label ID="lblProveedor" runat="server" CssClass="labelFormulario" 
                            Text="Proveedor: " visible="False" meta:resourcekey="lblProveedorResource1"></asp:Label>
                        <asp:DropDownList ID="cmbProveedor" runat="server" Visible="False" 
                             meta:resourcekey="cmbProveedorResource1"> </asp:DropDownList>
                    </td>
                    <td>
                     
                        </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label CssClass="labelFormulario" ID="lblMarca" runat="server" 
                            Text="Marca caldera:" meta:resourcekey="lblMarcaResource1"></asp:Label>
                        <asp:TextBox ID="txt_MarcaCaldera" runat="server" CssClass="TEXTBOX" 
                            Width="175px" Font-Bold="True" meta:resourcekey="txt_MarcaCalderaResource1"></asp:TextBox>
                    </td>
                    <td  >
                        <asp:Label CssClass="labelFormulario" ID="lblModelo" runat="server" 
                            Text="Modelo caldera:" meta:resourcekey="lblModeloResource1"></asp:Label>
                        <asp:TextBox ID="txt_ModeloCaldera" runat="server" CssClass="TEXTBOX" 
                            Width="221px" Font-Bold="True" meta:resourcekey="txt_ModeloCalderaResource1"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label CssClass="labelFormulario" ID="Label5" runat="server" 
                            Text="Horario contacto:" meta:resourcekey="Label5Resource1"></asp:Label>
                        <asp:DropDownList ID="cmbHorarioContacto" runat="server" Width="200px" 
                            meta:resourcekey="cmbHorarioContactoResource1">
                            <asp:ListItem Text="MAÑANA" Value="MAÑANA" meta:resourcekey="ListItemResource1"></asp:ListItem>
                            <asp:ListItem Text="TARDE" Value="TARDE" meta:resourcekey="ListItemResource2"></asp:ListItem>
                            <asp:ListItem Text="NOCHE" Value="NOCHE" meta:resourcekey="ListItemResource3"></asp:ListItem>
                            <asp:ListItem Text="CUALQUIERA" Value="CUALQUIERA" 
                                meta:resourcekey="ListItemResource4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td colspan="2" >
                        <asp:Label CssClass="labelFormulario" ID="Label6" runat="server" 
                            Text="Teléfono contacto:" meta:resourcekey="Label6Resource1"></asp:Label>
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="TEXTBOX" Width="100px" 
                            Font-Bold="True" Height="20px" MaxLength="18" 
                            meta:resourcekey="txtTelefonoResource1"></asp:TextBox>
                    </td>
                </tr>
            </table>
            
            <table>
                <tr>
                    
                    <td>
                        <asp:TextBox ID="txt_ObservacionesAteriores" runat="server" CssClass="TEXTBOX" 
                            Width="764px" Height="92px" ReadOnly="True" TextMode="MultiLine" 
                            ForeColor="Gray" Font-Bold="True" MaxLength="5000" 
                            meta:resourcekey="txt_ObservacionesAterioresResource1"></asp:TextBox>
                        
                        <asp:TextBox ID="txt_Observaciones" runat="server" CssClass="TEXTBOX" 
                            Width="764px" Height="53px" TextMode="MultiLine" 
                            meta:resourcekey="txt_ObservacionesResource1"></asp:TextBox>
                    </td>
                </tr>
            </table>
            
            </asp:Panel>
        </div>
        
        
        <div class="panelTabla" style="border-style: none; border-color: inherit; border-width: 0px; position:absolute; top: 267px; left: 0px; background-color:White; height: 190px;">
            <asp:Panel ID="Panel2" runat="server"  BackColor="#DCE4D9" Height="190px" 
                Width="782px" BorderWidth="0px" BorderStyle="Solid" BorderColor="White" 
                meta:resourcekey="Panel2Resource1">
                <table width="100%">
                    <tr>
                        <td align="left">                        
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label327" CssClass="labelFormulario" runat="server" 
                                            Text="CARACTERISTICAS" Font-Bold="True" meta:resourcekey="Label327Resource1"></asp:Label>       
                                    </td>
                                    <td align="right">
                                    <asp:Button BackColor="Black" ForeColor="#66CCFF" ID="btnCodificacionAverias0" 
                                            CssClass="botonFormulario" 
                                            OnClientClick="AbrirSolicitud();return false;" runat="server" 
                                            style="text-align:center;" Text="ALTA SOLICITUD" 
                                            meta:resourcekey="btnAltaSolicitudResource" Visible="false" />
                                        <asp:Button ID="btnCodificacionAverias" runat="server" BackColor="White" 
                                            CssClass="botonFormulario" ForeColor="Orange" 
                                            meta:resourcekey="btnCodificacionAveriasResource1" 
                                            OnClientClick="MostrarCodificacionAveria();return false;" 
                                            style="text-align:center;" Text="Código Averias" />
                                        <asp:Button ID="btnAltaAveriaCaldera" runat="server" Visible="False" 
                                            BackColor="Red"  ForeColor="White" CssClass="botonFormulario" 
                                            Text="AVERIA GAS CONFORT" Width="161px" 
                                            onclick="btnAltaAveriaCaldera_Click" 
                                            meta:resourcekey="btnAltaAveriaCalderaResource1" />
                                        <asp:Button ID="btnMostrarHistoricoCaracteristicas" runat="server" 
                                            Text="Ver Histórico" CssClass="botonFormulario" 
                                            OnClientClick="return MostrarOcultarHistoricoCaracteristicas();" 
                                            meta:resourcekey="btnMostrarHistoricoCaracteristicasResource1" />
                                        <asp:Button ID="btnTicketCombustion" runat="server" Text="Ticket combustion" CssClass="botonFormulario"
                                            Width="171px" OnClick="OnbtnTicketCombustion_Click" 
                                            meta:resourcekey="btnTicketCombustionResource1" visible="true"/>
                                        <asp:Button ID="btnGestionDocumental" runat="server" Text="Gestion Documental" CssClass="botonFormulario"
                                            Width="171px" OnClientClick="AbrirGestionDocumental();return false;"
                                            meta:resourcekey="btnGestionDocumentalResource1" visible="true"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        
                            <asp:Panel ID="pnlCaracteristicas" BackColor="White" runat="server" 
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" ScrollBars="Both" 
                                Height="166px" Width="780px" meta:resourcekey="pnlCaracteristicasResource1">
                                <asp:Table ID="tblCaracteristicas" runat="server" 
                                    style="background-color:White;border-color:#CCCCCC;border-width:1px;border-style:None;width:750px;border-collapse:collapse;" 
                                    meta:resourcekey="tblCaracteristicasResource1">
                                    <asp:TableHeaderRow runat="server" meta:resourcekey="TableHeaderRowResource1">
                                        <asp:TableHeaderCell BorderColor="#CCCCCC" BorderWidth="1px" 
                                            BorderStyle="Solid" BackColor="#006699" Font-Bold="True" ForeColor="White" 
                                            Width="500px" HorizontalAlign="Left" runat="server" 
                                            meta:resourcekey="TableHeaderCellResource1">Descripción</asp:TableHeaderCell>
                                        <asp:TableHeaderCell BorderColor="#CCCCCC" BorderWidth="1px" 
                                            BorderStyle="Solid" BackColor="#006699" Font-Bold="True" ForeColor="White" 
                                            Width="200px" HorizontalAlign="Center" runat="server" 
                                            meta:resourcekey="TableHeaderCellResource2">Valor</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                    <asp:TableRow  ID="lineaCaracteristica" runat="server" BackColor="White" 
                                        ForeColor="Black" meta:resourcekey="lineaCaracteristicaResource1">
                                        
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                            
                        </td>
                    </tr>
             </table>
            </asp:Panel>
        </div>
    

        <div style="position:absolute;top:465px;left:0px">
        <hr />
               <asp:Label CssClass="labelFormulario" ID="lblTipLugarAveria" runat="server" 
                Text="Lugar de la Avería:"  ForeColor="Red" Font-Bold="True" 
                Font-Underline="True" Visible="False" 
                meta:resourcekey="lblTipLugarAveriaResource1"></asp:Label>
              <asp:DropDownList ID="dll_tipLugarAveria" runat="server" Visible="False" 
                Width="200px" meta:resourcekey="dll_tipLugarAveriaResource1">
                        </asp:DropDownList>
            <asp:Label CssClass="labelFormulario" ID="lblContratoGC" runat="server" 
                Text="Contrato GC:"  ForeColor="Red" Font-Bold="True" 
                Font-Underline="True" Visible="False" 
                meta:resourcekey="lblContratoGC"></asp:Label>
              <asp:FileUpload class="file_1" ID="FileGC" runat="server" Width="199px" BackColor="White" CssClass="file_1" ForeColor="Black" visible="False" />

            <br />
            <asp:Label ID="lblClienteInfo"  CssClass="labelFormulario" runat="server" 
                ForeColor="#34721F" Font-Bold="True" meta:resourcekey="lblClienteInfoResource1"></asp:Label>&nbsp;
            <asp:Label ID="lblSuministroInfo" CssClass="labelFormulario" runat="server" 
                meta:resourcekey="lblSuministroInfoResource1"></asp:Label>&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblServicioInfo" CssClass="labelFormulario" runat="server" 
                ForeColor="#34721F" Font-Italic="True" 
                meta:resourcekey="lblServicioInfoResource1"></asp:Label>
        </div>
    
    <div style="position:absolute;top:519px; left:220px; width: 123px;">
    <asp:Image ID="imgLlamadaCortesia" onClick="" 
            onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'" 
            AlternateText="Generada tras llamada de cortesía" 
            ImageUrl="./HTML/Images/smart_phone_warning.png" runat="server" Height="27px" 
            Width="27px" meta:resourcekey="imgLlamadaCortesiaResource1" />        
        <asp:Image ID="imgTwitter" onClick="javascript:Twitter();" 
            onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'" 
            AlternateText="SOLICITUD VIA TWITTER" ImageUrl="../Imagenes/twitter_verde.png" 
            runat="server" Height="27px" Width="27px" 
            meta:resourcekey="imgTwitterResource1" />
        <asp:Image ID="imgFacebook" onClick="javascript:Facebook();" 
            onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'" 
            AlternateText="SOLICITUD VIA FACEBOOK" 
            ImageUrl="../Imagenes/facebook_verde.png" runat="server" Height="27px" 
            Width="27px" meta:resourcekey="imgFacebookResource1" />
        <asp:CheckBox ID="chkFacebook" style="visibility:hidden" runat="server" 
            meta:resourcekey="chkFacebookResource1" />
        <asp:CheckBox ID="chkTwitter" style="visibility:hidden" runat="server" 
            meta:resourcekey="chkTwitterResource1" />
        
     </div>

        
     <div id="div2" style="position:absolute; top:506px; left:85px; width: 211px;">
     <asp:Label ID="lblTwitter" style="visibility:hidden" runat="server" 
             CssClass="labelFormulario" Text="SOLICITUD VIA TWITTER" ForeColor="Blue" 
             Font-Bold="True" meta:resourcekey="lblTwitterResource1" ></asp:Label>
     <asp:Label ID="lblFacebook" style="visibility:hidden" runat="server" 
             CssClass="labelFormulario" Text="SOLICITUD VIA FACEBOOK" ForeColor="Blue" 
             Font-Bold="True" meta:resourcekey="lblFacebookResource1" ></asp:Label></div>
   
    
     <div style="position:absolute;top:525px;left:297px; width: 244px;">
            <asp:Button ID="btnAceptar" runat="server" CssClass="botonFormulario" 
                Text="Aceptar" Width="115px" Height="21px" TabIndex="3" 
                onclick="btnAceptar_Click" OnClientClick="GuardarDatos();" 
                meta:resourcekey="btnAceptarResource1"/>
                
            <asp:Button ID="btnCancelar" runat="server" CssClass="botonFormulario" 
                Text="Cancelar" Width="115px" Height="21px" TabIndex="4" 
                onclick="btnCancelar_Click" CausesValidation="false" meta:resourcekey="btnCancelarResource1"/>
        </div>
    </div>
    
    <div id="divHistoricoCaracteristicas" style="overflow: scroll;position:absolute;top:5px; left:2px; width: 780px; height:255px; display:none" class="panelTabla">
       <asp:GridView ID="dgHistorico" style="z-index:999" runat="server" CssClass="contenidoTabla" 
            Width="760px" EnableModelValidation="True" 
            meta:resourcekey="dgHistoricoResource1"> 
            <RowStyle CssClass="filaNormal" Height="25px" Wrap="true"/>
            <AlternatingRowStyle CssClass="filaAlterna"/>
            <FooterStyle CssClass="cabeceraTabla"  />
            <PagerStyle  CssClass="cabeceraTabla" />
            <HeaderStyle CssClass="cabeceraTabla" />
        </asp:GridView>
    </div>

    
    <script>

        // Para mostrar u ocultar el div que contiene el grid de el listado del 
        // histórico de las caracterísitcas
        function MostrarOcultarHistoricoCaracteristicas() {

            // Recuperamos tanto el div como el botón que la muestra/oculta.
            var divHistorico = document.getElementById("divHistoricoCaracteristicas");
            var btnMostrar = document.getElementById("ctl00_ContentPlaceHolderContenido_btnMostrarHistoricoCaracteristicas");

            // Si está oculto lo mostramos.
            if (divHistorico.style.display == "none") {
                divHistorico.style.display = "block";
                btnMostrar.value = "<%=Resources.TextosJavaScript.TEXTO_OCULTAR_HISTORICO%>"//"Ocultar Histórico"
            }
            // En caso contrario, estará visible, con lo que lo ocultamos.
            else {

                divHistorico.style.display = "none";
                btnMostrar.value = "<%=Resources.TextosJavaScript.TEXTO_VER_HISTORICO%>"//"Ver Histórico"
            }

            // Retornamos false para que no se lance el evento de servidor del botón
            // que llama a esta función.
            return false;
        }

        function AbrirGestionDocumental()
        {
            var url = "./FrmModalGestionDocumental.aspx?COD_CONTRATO=" + document.getElementById("ctl00_ContentPlaceHolderContenido_hdnContrato").value + "&ID_SOLICITUD=" + document.getElementById("ctl00_ContentPlaceHolderContenido_hdnIdSolicitud").value;
            window.open(url, "GestionDocumental", "resizable=yes,top=250,left=150,width=700,height=400", false);
        }

    </script>
</asp:Content>
