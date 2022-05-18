<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="FrmModalTicketCombustion.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalTicketCombustion" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>


<asp:Content ID="FrmModalTicketCombustion" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <script language="javascript">
        function GuardarDatos() {
            if (Page.IsValid) {
                MostrarCapaEspera();
                return true;
            }
            else
            {
                return false;
            }
        }
        function CrearVentanaVacia() {
            //window.document.forms[0].target = '_new';
            //setTimeout(function () { window.document.forms[0].target = ''; }, 500);
            
            alert("Hola");
            window.location.href = "MenuAcceso.aspx";
            window.open("https://www.google.es", "Diseo Web", "width=300, height=200", true);
        }
    </script>

    <style>
        div.fileinputs {
            position: relative;
            display: inline-block;
            width: 49%;
        }

        div.fakefile {
            position: absolute;
            top: 0px;
            left: 255px;
            z-index: 1;
        }

        input.file {
            position: relative;
            text-align: right;
            -moz-opacity: 0;
            filter: alpha(opacity: 0);
            opacity: 0;
            z-index: 2;
        }

        #divSplashScreen
        {
            position:absolute;
            top:130px;
            left:190px;
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

    <div id="divPanelTicketCombustion" style="position: absolute; top: 12px; left: 20px; width: 550px; height: 350px;">

        <div id="divSplashScreen">
            <table border="0" cellpadding="0" cellspacing="0" width="1%" align="center">
                <tr>
                    <td align="center">
                         <img src="HTML/Images/loading.gif" alt="Procesando..."/>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <span class="labelFormularioValor"> Procesando... </span>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divdgTicketsCombustion" style="border: 1px solid #000000; height:65px; Width:auto; display: none">
            <asp:GridView ID="dgTicketsCombustion" runat="server" AutoGenerateColumns="False" CssClass="contenidoTabla"
                    OnRowDataBound="dgTicketsCombustion_RowDataBound" EnableModelValidation="True" 
                    meta:resourcekey="dgTicketsCombustionResource1" OnDataBinding="dgTicketsCombustion_DataBinding">
                <RowStyle CssClass="filaNormal"/>
                <AlternatingRowStyle CssClass="filaAlterna"/>
                <Columns >
                    <asp:TemplateField HeaderText="Ticket" SortExpression="ID_TICKET_COMBUSTION" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="55px" 
                            HeaderStyle-Width="55px" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <div style="width: 30px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                <asp:LinkButton ID="lnk2" runat="server" Text='<%# Bind("ID_TICKET_COMBUSTION") %>' 
                                    OnClick="OnRowSelected_Click" meta:resourcekey="lnk2Resource1"></asp:LinkButton>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID_SOLICITUD" HeaderText="Id Sol." 
                            SortExpression="ID_SOLICITUD" ItemStyle-HorizontalAlign="Center" 
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="55px"
                            HeaderStyle-Width="55px" meta:resourcekey="BoundFieldResource1">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="COD_VISITA" HeaderText="Cod Visita" 
                            SortExpression="COD_VISITA" ItemStyle-HorizontalAlign="Center" 
                            HeaderStyle-HorizontalAlign="Center"  ItemStyle-Width="55px"
                            HeaderStyle-Width="55px" meta:resourcekey="BoundFieldResource1">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TIPO_EQUIPO" HeaderText="Tipo Equipo" 
                            SortExpression="TIPO_EQUIPO" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center" 
                             meta:resourcekey="BoundFieldResource2">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" Width="125px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" 
                            SortExpression="COMENTARIOS"  ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100%" 
                            HeaderStyle-Width="100%" meta:resourcekey="BoundFieldResource3">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <FooterStyle CssClass="cabeceraTabla"  />
                <PagerStyle  CssClass="cabeceraTabla" />
                <HeaderStyle CssClass="cabeceraTabla" />
            </asp:GridView>
            <div style="height: 12px;"></div>
        </div>

        <div style="height: 12px;  display: none"></div>
        <div>
            <asp:Label CssClass="labelFormulario" ID="lblDatosTicketCombustion" runat="server" Text="Ticket combustion"
                meta:resourcekey="lblDocumentosResource1" Font-Bold="true"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label CssClass="labelFormulario" ID="lblInformacion" runat="server" Text="Importante: Los decimales deben de ir con coma."
                meta:resourcekey="lblDocumentosResource1" Font-Bold="true" ForeColor="Red"></asp:Label>
        </div>

        <div id="divTicketCombustion" style="border: 1px solid #000000; height:196px; Width:auto">
            <div style="HEIGHT: 5px"></div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTipoEquipo" runat="server" 
                            Text="Tipo equipo: " meta:resourcekey="lblTipoEquipoResource1"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList 
                            ID="txtTipoEquipo" runat="server" Width="170px" TabIndex="1" 
                            meta:resourcekey="txtTipoEquipoResource1"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTemperaturaPDC" runat="server" 
                            Text="Temperatura PDC: " meta:resourcekey="lblTemperaturaPDCResource1"></asp:Label>  
                    </td>
                    <td >               
                        <asp:TextBox ID="txtTemperaturaPDC" runat="server" Width="40px" 
                            meta:resourcekey="txtTemperaturaPDCResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revTemperaturaPDC" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtTemperaturaPDC" />

                    </td>
                                                                        
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCOCorregido" runat="server" 
                            Text="CO Corregido: " meta:resourcekey="lblCOCorregidoResource1"></asp:Label>
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCOCorregido" runat="server" Width="40px" 
                            meta:resourcekey="txtCOCorregidoResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revCOCorregido" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtCOCorregido" />
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTiro" runat="server" 
                            Text="Tiro: " meta:resourcekey="lblTiroResource1"></asp:Label>      
                    </td>
                    <td >               
                        <asp:TextBox ID="txtTiro" runat="server" Width="40px" 
                            meta:resourcekey="txtTiroResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revTiro" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtTiro" />
                    </td>
                </tr>

                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblO2" runat="server" 
                            Text="O2: " meta:resourcekey="lblO2Resource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtO2" runat="server" Width="40px" 
                            meta:resourcekey="txtO2Resource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revO2" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtO2" />
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCO" runat="server" 
                            Text="CO: " meta:resourcekey="lblCOResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCO" runat="server" Width="40px" 
                            meta:resourcekey="txtCOResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revCO" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtCO" />
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCO2" runat="server" 
                            Text="CO2: " meta:resourcekey="lblCO2Resource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCO2" runat="server" Width="40px" 
                            meta:resourcekey="txtCO2Resource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revCO2" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtCO2" />
                    </td>
                </tr>

                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCOAmbiente" runat="server" 
                            Text="CO Ambiente: " meta:resourcekey="lblCOAmbienteResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCOAmbiente" runat="server" Width="40px" 
                            meta:resourcekey="txtCOAmbienteResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revCOAmbiente" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtCOAmbiente" />
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblLambda" runat="server" 
                            Text="Lambda: " meta:resourcekey="lblLambdaResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtLambda" runat="server" Width="40px" 
                            meta:resourcekey="txtLambdaResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revLambda" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtLambda" />
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblRendimiento" runat="server" 
                            Text="Rendimiento: " meta:resourcekey="lblRendimientoResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtRendimiento" runat="server" Width="40px" 
                            meta:resourcekey="txtRendimientoResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revRendimiento" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtRendimiento" />
                    </td>
                </tr>
                        
                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTemperaturaMaxACS" runat="server" 
                            Text="Temperatura Max ACS: " meta:resourcekey="lblTemperaturaMaxACSResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtTemperaturaMaxACS" runat="server" Width="40px" 
                            meta:resourcekey="txtTemperaturaMaxACSResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revTemperaturaMaxACS" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtTemperaturaMaxACS" />
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCaudalACS" runat="server" 
                            Text="Caudal ACS: " meta:resourcekey="lblCaudalACSResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCaudalACS" runat="server" Width="40px" 
                            meta:resourcekey="txtCaudalACSResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revCaudalACS" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtCaudalACS" />
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblPotenciaUtil" runat="server" 
                            Text="Potencia Util: " meta:resourcekey="lblPotenciaUtilResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtPotenciaUtil" runat="server" Width="40px" 
                            meta:resourcekey="txtPotenciaUtilResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revPotenciaUtil" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtPotenciaUtil" />
                    </td>
                </tr>

                <tr>
                    <td class="labelFormulario" colspan="2">
                        <asp:Label CssClass="labelFormulario" ID="lblTemperaturaEntradaSalidaACS" runat="server" 
                            Text="Temperatura ACS: " meta:resourcekey="lblTemperaturaEntradaSalidaACSResource1"></asp:Label>                                     
                    </td>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTemperaturaEntradaACS" runat="server" 
                            Text="Entrada: " meta:resourcekey="lblTemperaturaEntradaACSResource1"></asp:Label>                                     
                    </td>
                    <td>               
                        <asp:TextBox ID="txtTemperaturaEntradaACS" runat="server" Width="40px" 
                            meta:resourcekey="txtTemperaturaACSResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revTemperaturaEntradaACS" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtTemperaturaEntradaACS" />
                    </td>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTemperaturaSalidaACS" runat="server" 
                            Text="Salida: " meta:resourcekey="lblTemperaturaSalidaACSResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtTemperaturaSalidaACS" runat="server" Width="40px" 
                            meta:resourcekey="txtTemperaturaSalidaACSResource1"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revTemperaturaSalidaACS" runat="server" ValidationExpression="-?\d{1,4}(,\d{1,4})?" ErrorMessage="*" ControlToValidate="txtTemperaturaSalidaACS" />
                    </td>
                </tr>
                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblIdSolicitudAveria" runat="server" 
                            Text="Sol. Averia: " meta:resourcekey="lblIdSolicitudAveriaResource1" Visible="false"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="TxtIdSolicitudAveria" runat="server" Width="40px" 
                            meta:resourcekey="txtIdSolicitudAveriaResource1" Visible="false"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <div style="HEIGHT: 10px"></div>
            <div id="divComentarios">
                <div>
                    <asp:Label CssClass="labelFormulario" ID="lblComentarios" runat="server" Text="Comentarios"
                         meta:resourcekey="lblComentariosResource1" Font-Bold="true"></asp:Label>  
                </div>
                <div id="idComentario">
                    <asp:TextBox ID="txtComentarios" runat="server" TextMode="MultiLine" Height="50px" Width="98%" meta:resourcekey="txtComentariosResource1"></asp:TextBox>
                </div>
            </div>
        </div>

        <div style="HEIGHT: 10px"></div>
        <div>
            <asp:Label CssClass="labelFormulario" ID="lblDocumentos" runat="server" Text="Documentos" 
                meta:resourcekey="lblDocumentosResource1" Font-Bold="true"></asp:Label>
<%--            <div style="display: inline-block; width: 50%;">
                <asp:Label CssClass="labelFormulario" ID="lblDocumentos" runat="server" Text="Documentos"
                    meta:resourcekey="lblDocumentosResource1" Font-Bold="true"></asp:Label>
            </div>
            <div class="fileinputs">
                <asp:FileUpload ID="FileTicketCombustion" class="file" runat="server" ToolTip="Añadir fichero" Onchange="OnLoadFileUpload_Click"/>

                <div class="fakefile" style="display: block;">
                     <asp:image runat="server" src="../UI/HTML/Images/aniadir_nuevo_peq.jpg"/>
                </div>
            </div>--%>
        </div>
        <div id="divFicherosTicketCombustion" runat="server" style="border: 1px solid #000000; height:80px; Width:auto" visible="false">
            <div style="HEIGHT: 5px"></div>
            <div id="divFicherosVisualizar" runat="server" visible="false">
                <asp:PlaceHolder ID="phDocumentos" runat="server">
                </asp:PlaceHolder>
            </div>
            <div style="HEIGHT: 8px"></div>
            <div id="divFicherosAdjuntar" runat="server" visible="false">
                <div>
                    <asp:Label CssClass="labelFormulario" ID="lblFileTicketCombustion" runat="server" Text="Ticket combustion"
                        meta:resourcekey="lblFileTicketCombustionResource1" Width="100px"></asp:Label>
                    <asp:FileUpload ID="FileTicketCombustion" runat="server" Width="380px" Enabled="true"/>
                </div>
                            <div style="HEIGHT: 2px"></div>
                <div>
                    <asp:Label CssClass="labelFormulario" ID="lblFileConductoHumos" runat="server" Text="Conducto humos"
                        meta:resourcekey="lblFileConductoHumosResource1" Width="100px"></asp:Label>
                    <asp:FileUpload ID="FileConductoHumos" runat="server" Width="380px" Enabled="true"/>
                </div>
            </div>
        </div>
        <div style="HEIGHT: 15px"></div>
        <div id="divMostrarResultado" runat="server" visible="false">
            <asp:Label CssClass="labelFormulario" ID="lblMostrarResultado" runat="server" Text="" meta:resourcekey="lblComentariosResource1" Font-Bold="true" ForeColor="Red" Font-Size="11px"></asp:Label>
        </div>
    </div>
  
    <div id="divBotonAceptar" style="position: absolute; width: 315px; top: 385px; left: 203px;
        height: 22px; margin-right: 1px;">
        <asp:Button ID="btnCancelar" runat="server" CssClass="botonFormulario" Text="Cerrar"
            Width="75px" OnClick="OnBtnCancelar_Click" TabIndex="11" 
            meta:resourcekey="btnCancelarResource1" />
    </div>
    <div id="divBotonGuardar" style="position: absolute; width: 360px; top: 385px; left: 280px;
        height: 22px; margin-right: 1px;">
        <asp:Button ID="btnGuardar" runat="server" CssClass="botonFormulario" Text="Guardar"
            Width="75px" OnClientClick="return GuardarDatos();" OnClick="OnBtnGuardar_Click" TabIndex="1" 
            meta:resourcekey="btnGuardarResource1" />
    </div>
</asp:Content>
