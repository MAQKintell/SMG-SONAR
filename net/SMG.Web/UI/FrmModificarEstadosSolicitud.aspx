<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmModificarEstadosSolicitud.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModificarEstadosSolicitud" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmConsultas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <Link href="../css/ventana-modal.css" type="text/css" rel="stylesheet" /> 
    <Link href="../css/ventan-modal-ie.css" type="text/css" rel="stylesheet" />
    <Link href="HTML/CSS/css_cached.css" type="text/css" rel="stylesheet" />

    <script src="HTML/js/prototypeCambioEstados.js" type="text/javascript"></script>
    <%--<script src="HTML/js/portalCambioEstados.js" type="text/javascript"></script>--%>    
	<script src="../js/ventana-modal.js" type="text/javascript"></script>
    <script type="text/javascript">
      function Preguntar()
      {
         if (document.getElementById("ctl00_ContentPlaceHolderContenido_hdnLiquidado").value == '1') 
         {
             if (confirm("<%=Resources.TextosJavaScript.TEXTO_YA_LIQUIDADO_CAMBIAR_ESTADO%>"))//'¿SOLICITUD/VISITA YA LIQUIDADA, QUIERE CAMBIAR EL ESTADO?'))
             {
//                if (confirm('¿ESTA SEGURO? EL CONTRATO DEJARA DE ESTAR OPERATIVO')) {
//                    return true;
//                }
//                else {
//                    return false;
                //                }
                return true;
            }
            else {
                return false;
            }
        }
    }

    function PreguntarEliminar() {
        if (document.getElementById("ctl00_ContentPlaceHolderContenido_hdnLiquidado").value == '1') {
            if (confirm("<%=Resources.TextosJavaScript.TEXTO_YA_LIQUIDADO_ELIMINAR%>")) {//'¿SOLICITUD YA LIQUIDADA, ESTA SEGURO QUE QUIERE ELIMINARLA?')) {
                //                if (confirm('¿ESTA SEGURO? EL CONTRATO DEJARA DE ESTAR OPERATIVO')) {
                //                    return true;
                //                }
                //                else {
                //                    return false;
                //                }
                return true;
            }
            else {
                return false;
            }
        }
    }


    </script>
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="MODIFICAR ESTADO <<tipo>>" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
        
     <!-- Grid histórico de solicitud --> 
    <div id="div1" style="position: absolute; top: 30px; left: 5px;">
        <input id="hdnIdSolicitud" type="hidden" runat="server" />
        <input id="hdnIdVisita" type="hidden" runat="server" />
        <input id="hdnCodContrato" type="hidden" runat="server" />
        <input id="hdnLiquidado" type="hidden" runat="server" />
        <input id="hdnCodEstadoActual" type="hidden" runat="server" />
        <input id="hdnCodEstadoVisitaActual" type="hidden" runat="server" />
        <asp:Label ID="lblSubtitulo" runat="server" Text="Histórico de la <<tipo>>" 
            CssClass="divSubTituloVentana" meta:resourcekey="lblSubtituloResource1"></asp:Label>    
    </div>
    
    <div id="divGridSolicitudes" style="z-index:9999; overflow: scroll; position: absolute; top: 50px; left: 5px; Height:230px; width: 955px" class="panelTabla">   
        <asp:GridView ID="dgSolicitudesHistorico" style="z-index:999" runat="server" CssClass="contenidoTabla" 
            onrowdatabound="dgSolicitudesHistorico_RowDataBound" Width="3500px" 
            EnableModelValidation="True" meta:resourcekey="dgSolicitudesHistoricoResource1"> 
            <RowStyle CssClass="filaNormal" Height="25px" Wrap="true"/>
            <AlternatingRowStyle CssClass="filaAlterna"/>            
            <FooterStyle CssClass="cabeceraTabla"  />
            <PagerStyle  CssClass="cabeceraTabla" />
            <HeaderStyle CssClass="cabeceraTabla" />
        </asp:GridView>
    </div>
    
    <!-- Estados --> 
    <div id="divTituloGridSolicitudes" style="position: absolute; top: 300px; left: 5px; width:950px" >
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Estado Actual:"  
                        CssClass="campoFormulario" meta:resourcekey="Label2Resource1"></asp:Label>                
                </td>
                <td>
                    <asp:Label ID="lblEstadoActual" runat="server"  CssClass="labelFormularioValor" 
                        meta:resourcekey="lblEstadoActualResource1"></asp:Label>                                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Observaciones:"  
                        CssClass="campoFormulario" meta:resourcekey="Label3Resource1" ></asp:Label>                
                </td>
                <td>
                    <asp:TextBox ID="txtObservaciones" runat="server" CssClass="campoFormulario" 
                        MaxLength="255" Width="750px" meta:resourcekey="txtObservacionesResource1"></asp:TextBox>
                    <asp:CustomValidator ID="txtObservacionesCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtObservaciones" OnServerValidate="OnTxtObservaciones_ServerValidate" 
                    ValidateEmptyText="True" 
                        meta:resourcekey="txtObservacionesCustomValidatorResource1"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTituloGridSolicitudes" runat="server" Text="Nuevo estado:"  
                        CssClass="campoFormulario" meta:resourcekey="lblTituloGridSolicitudesResource1"></asp:Label>                
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlEstadosSolicitud" 
                        CausesValidation="True" meta:resourcekey="ddlEstadosSolicitudResource1"></asp:DropDownList>
                    <asp:CustomValidator ID="ddlEstadosSolicitudCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="ddlEstadosSolicitud" OnServerValidate="OnDdlEstadosSolicitud_ServerValidate" 
                    ValidateEmptyText="True" 
                        meta:resourcekey="ddlEstadosSolicitudCustomValidatorResource1"></asp:CustomValidator>
                </td>
            </tr>
              <tr>
                <td>
                    <asp:Label ID="lblNumFactura" runat="server" Text="Numero Factura:"  
                        CssClass="campoFormulario" meta:resourcekey="lblNumFacturaResource1" ></asp:Label>                
                </td>
                <td>
                    <asp:TextBox ID="txtNumFactura" runat="server" CssClass="campoFormulario" 
                        MaxLength="255" Width="200px" meta:resourcekey="txtNumFacturaResource1"></asp:TextBox>
                    <asp:CustomValidator ID="txtCustomValidatorNumFactura" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtNumFactura" OnServerValidate="OnTxtNumFactura_ServerValidate" 
                    ValidateEmptyText="True" 
                        meta:resourcekey="txtCustomValidatorNumFacturaResource1"></asp:CustomValidator>
                </td>
                <td><asp:Label ID="lblAvisoNumFactura" runat="server"  CssClass="campoFormulario" 
                        meta:resourcekey="lblAvisoNumFacturaResource1" ></asp:Label></td>                
            </tr>
        </table>
    
        <asp:Button CssClass="botonFormulario" ID="btnConsultar" runat="server" 
            Text="Modificar estado" Style="position:relative; left:425px; top:25px"
          OnClientClick="return Preguntar();" OnClick="OnBtnModificar_Click" 
            meta:resourcekey="btnConsultarResource1" />  
          
          <asp:Button CssClass="botonFormulario" ID="btnEliminar" runat="server" 
            Text="Eliminar Solicitud" Style="position:relative; left:468px; top:25px"
          OnClientClick="return PreguntarEliminar();" 
            OnClick="OnBtnEliminarSolicitud_Click" 
            meta:resourcekey="btnEliminarResource1" />
    </div>
    
    <div id="divBotonVolver">
        <asp:Button CssClass="botonFormulario" ID="btnVolver" runat="server" Text="Volver"
         OnClick="OnBtnVolver_Click" TabIndex="2" 
            meta:resourcekey="btnVolverResource1" />
    </div>        
   </asp:Content>
   