<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModalResponsive.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="FrmModalTicketCombustionNew.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalTicketCombustionNew" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalTicketCombustion" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <div id="divPanelTicketCombustion" style="position: absolute; top: 12px; left: 20px; width: 550px; height: 350px;">
        <div id="divdgTicketsCombustion" style="border: 1px solid #000000; height:65px; Width:auto">
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
        <div style="height: 12px;"></div>
        <div id="divTicketCombustion" style="top: 18px;">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTipoEquipo" runat="server" 
                            Text="Tipo equipo: " meta:resourcekey="lblTipoEquipoResource1"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList 
                            ID="txtTipoEquipo" runat="server" Width="170px" TabIndex="1" 
                            meta:resourcekey="txtTipoEquipoResource1" disabled="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTemperaturaPDC" runat="server" 
                            Text="Temperatura PDC: " meta:resourcekey="lblTemperaturaPDCResource1"></asp:Label>  
                    </td>
                    <td >               
                        <asp:TextBox ID="txtTemperaturaPDC" runat="server" Width="30px" 
                            meta:resourcekey="txtTemperaturaPDCResource1" disabled="true"></asp:TextBox>
                    </td>
                                                                        
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCOCorregido" runat="server" 
                            Text="CO Corregido: " meta:resourcekey="lblCOCorregidoResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCOCorregido" runat="server" Width="30px" 
                            meta:resourcekey="txtCOCorregidoResource1" disabled="true"></asp:TextBox>
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTiro" runat="server" 
                            Text="Tiro: " meta:resourcekey="lblTiroResource1"></asp:Label>      
                    </td>
                    <td >               
                        <asp:TextBox ID="txtTiro" runat="server" Width="30px" 
                            meta:resourcekey="txtTiroResource1" disabled="true"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblO2" runat="server" 
                            Text="O2: " meta:resourcekey="lblO2Resource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtO2" runat="server" Width="30px" 
                            meta:resourcekey="txtO2Resource1" disabled="true"></asp:TextBox>
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCO" runat="server" 
                            Text="CO: " meta:resourcekey="lblCOResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCO" runat="server" Width="30px" 
                            meta:resourcekey="txtCOResource1" disabled="true"></asp:TextBox>
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCO2" runat="server" 
                            Text="CO2: " meta:resourcekey="lblCO2Resource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCO2" runat="server" Width="30px" 
                            meta:resourcekey="txtCO2Resource1" disabled="true"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCOAmbiente" runat="server" 
                            Text="CO Ambiente: " meta:resourcekey="lblCOAmbienteResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCOAmbiente" runat="server" Width="30px" 
                            meta:resourcekey="txtCOAmbienteResource1" disabled="true"></asp:TextBox>
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblLambda" runat="server" 
                            Text="Lambda: " meta:resourcekey="lblLambdaResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtLambda" runat="server" Width="30px" 
                            meta:resourcekey="txtLambdaResource1" disabled="true"></asp:TextBox>
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblRendimiento" runat="server" 
                            Text="Rendimiento: " meta:resourcekey="lblRendimientoResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtRendimiento" runat="server" Width="30px" 
                            meta:resourcekey="txtRendimientoResource1" disabled="true"></asp:TextBox>
                    </td>
                </tr>
                        
                <tr>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTemperaturaMaxACS" runat="server" 
                            Text="Temperatura Max ACS: " meta:resourcekey="lblTemperaturaMaxACSResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtTemperaturaMaxACS" runat="server" Width="30px" 
                            meta:resourcekey="txtTemperaturaMaxACSResource1" disabled="true"></asp:TextBox>
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblCaudalACS" runat="server" 
                            Text="Caudal ACS: " meta:resourcekey="lblCaudalACSResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtCaudalACS" runat="server" Width="30px" 
                            meta:resourcekey="txtCaudalACSResource1" disabled="true"></asp:TextBox>
                    </td>

                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblPotenciaUtil" runat="server" 
                            Text="Potencia Util: " meta:resourcekey="lblPotenciaUtilResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtPotenciaUtil" runat="server" Width="30px" 
                            meta:resourcekey="txtPotenciaUtilResource1" disabled="true"></asp:TextBox>
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
                        <asp:TextBox ID="txtTemperaturaEntradaACS" runat="server" Width="30px" 
                            meta:resourcekey="txtTemperaturaACSResource1" disabled="true"></asp:TextBox>
                    </td>
                    <td class="labelFormulario">
                        <asp:Label CssClass="labelFormulario" ID="lblTemperaturaSalidaACS" runat="server" 
                            Text="Salida: " meta:resourcekey="lblTemperaturaSalidaACSResource1"></asp:Label>                                     
                    </td>
                    <td >               
                        <asp:TextBox ID="txtTemperaturaSalidaACS" runat="server" Width="30px" 
                            meta:resourcekey="txtTemperaturaSalidaACSResource1" disabled="true"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <div style="HEIGHT: 12px"></div>
            <div id="divComentarios">
                <div>
                    <asp:Label CssClass="labelFormulario" ID="lblComentarios" runat="server" Text="Comentarios"
                         meta:resourcekey="lblComentariosResource1" Font-Bold="true"></asp:Label>  
                </div>
                <div id="idComentario">
                    <asp:TextBox ID="txtComentarios" runat="server" TextMode="MultiLine" Height="50px" Width="100%" meta:resourcekey="txtComentariosResource1" disabled="true"></asp:TextBox>
                </div>
            </div>
        </div>

        <div style="HEIGHT: 12px"></div>
        <div id="divFicherosTicketCombustion" runat="server">
            <div>
                <asp:Label CssClass="labelFormulario" ID="lblDocumentos" runat="server" Text="Documentos"
                    meta:resourcekey="lblDocumentosResource1" Font-Bold="true"></asp:Label>  
            </div>
            <div>
                <asp:PlaceHolder ID="phDocumentos" runat="server">
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
  
    <div id="divBotonAceptar" style="position: absolute; width: 360px; top: 385px; left: 280px;
        height: 22px; margin-right: 1px;">
        <asp:Button ID="btnCancelar" runat="server" CssClass="botonFormulario" Text="Cerrar"
            Width="75px" OnClick="OnBtnCancelar_Click" TabIndex="11" 
            meta:resourcekey="btnCancelarResource1" />
    </div>
</asp:Content>
