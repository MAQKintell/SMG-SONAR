<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmModificarEstados.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModificarEstados" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmConsultas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <Link href="../css/ventana-modal.css" type="text/css" rel="stylesheet" /> 
    <Link href="../css/ventan-modal-ie.css" type="text/css" rel="stylesheet" />
    <Link href="HTML/CSS/css_cached.css" type="text/css" rel="stylesheet" />

    <script src="HTML/js/prototypeCambioEstados.js" type="text/javascript"></script>
    <%--<script src="HTML/js/portalCambioEstados.js" type="text/javascript"></script>--%>    
	<script src="../js/ventana-modal.js" type="text/javascript"></script>
     
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="MODIFICAR ESTADOS" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
    
     <div id="divFiltroBusqueda" style="position: absolute; top: 25px; width:955px">
        <input id="hdnSolicitud" type="hidden" runat="server" />
        <table width="100%">
            <tr>
                <td>
                    <asp:Label CssClass="campoFormulario" ID="lblContrato" runat="server" 
                        Text="Cód. Contrato: " meta:resourcekey="lblContratoResource1" ></asp:Label>                       
                    <asp:TextBox ID="txtContrato" CssClass="campoFormulario" runat="server" 
                    MaxLength="10" Width="60px" meta:resourcekey="txtContratoResource1"  
                   
                    ></asp:TextBox>
                    <asp:CustomValidator ID="txtContratoCustomValidator" CssClass="errorFormulario" 
                        runat="server" ErrorMessage="*" ControlToValidate="txtContrato" OnServerValidate="OnTxtContrato_ServerValidate" 
                      ValidateEmptyText="True" 
                        meta:resourcekey="txtContratoCustomValidatorResource1"></asp:CustomValidator>
                </td>
                <td>
                    <asp:Label CssClass="campoFormulario" ID="lblIdSolicitud" runat="server" 
                        Text="Id. Solicitud: " meta:resourcekey="lblIdSolicitudResource1" ></asp:Label>                        
                     <asp:TextBox ID="txtIdSolicitud" CssClass="campoFormulario" runat="server" 
                    MaxLength="10" Width="80px" meta:resourcekey="txtIdSolicitudResource1"  
                    
                    ></asp:TextBox>
                    <asp:CustomValidator ID="txtIdSolicitudCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtIdSolicitud" OnServerValidate="OnTxtIdSolicitud_ServerValidate" 
                      ValidateEmptyText="True" 
                        meta:resourcekey="txtIdSolicitudCustomValidatorResource1"></asp:CustomValidator>
                </td>
                <td align="right">
                    <asp:Button CssClass="botonFormulario" ID="btnConsultar" runat="server" Text="Buscar"
                    OnClick="OnBtnBuscar_Click" meta:resourcekey="btnConsultarResource1" />                    
                    <asp:Button CssClass="botonFormulario" ID="btnLimpiarFiltro" runat="server" Text="Limpiar Filtro"
                    OnClick="OnBtnLimpiar_Click" CausesValidation="False" 
                        meta:resourcekey="btnLimpiarFiltroResource1" />
                </td>
            </tr>
        </table>
    </div>
    
    <!-- Solicitudes --> 
    <div id="divTituloGridSolicitudes" style="position: absolute; top: 55px; left: 5px;">
    <asp:Label ID="lblTituloGridSolicitudes" runat="server" Text="Solicitudes" 
            CssClass="divSubTituloVentana" 
            meta:resourcekey="lblTituloGridSolicitudesResource1" ></asp:Label>    
    </div>
    
    <div id="divGridSolicitudes" style="z-index:9999; overflow: scroll; position: absolute; top: 70px; left: 5px; Height:230px; width: 955px" class="panelTabla">   
        <asp:GridView ID="dgSolicitudes" style="z-index:999" runat="server" CssClass="contenidoTabla" 
            OnRowCommand="dgSolicitudes_RowCommand"
            onrowdatabound="dgSolicitudes_RowDataBound" Width="3500px" 
            EnableModelValidation="True" meta:resourcekey="dgSolicitudesResource1"> 
            <RowStyle CssClass="filaNormal" Height="25px" Wrap="true"/>
            <AlternatingRowStyle CssClass="filaAlterna"/>
            <Columns>
                <asp:TemplateField HeaderText=""  ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15px" 
                    HeaderStyle-Width="15px" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEditar" runat="server" 
                                ImageUrl="HTML/Images/pencil.gif"  ToolTip="Modificar estado" 
                                CommandName="Editar" CommandArgument='<%# Bind("ID_solicitud") %>' 
                                meta:resourcekey="btnEditarResource1" />
                            </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>                                                 
            </Columns>
            <FooterStyle CssClass="cabeceraTabla"  />
            <PagerStyle  CssClass="cabeceraTabla" />
            <HeaderStyle CssClass="cabeceraTabla" />
        </asp:GridView>
    </div>
    
    <!-- Visitas --> 
    <div id="divTituloGridVisitas" style="position: absolute; top: 310px; left: 5px;">
        <asp:Label  ID="lblTituloVisitas" runat="server" Text="Visitas" 
            CssClass="divSubTituloVentana" meta:resourcekey="lblTituloVisitasResource1"></asp:Label>
    </div>
    
    <div id="divGridVisitas" style="z-index:9999; overflow: scroll; position: absolute; top: 325px; left: 5px; Height:230px; width: 955px" class="panelTabla">        
        <asp:GridView ID="dgVisitas" style="z-index:999" runat="server" CssClass="contenidoTabla" 
            OnRowCommand="dgVisitas_RowCommand"
            onrowdatabound="dgVisitas_RowDataBound" Width="1500px" 
            EnableModelValidation="True" meta:resourcekey="dgVisitasResource1"> 
            <RowStyle CssClass="filaNormal"/>
            <AlternatingRowStyle CssClass="filaAlterna"/>
            <Columns>
                <asp:TemplateField HeaderText=""  ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15px" 
                    HeaderStyle-Width="15px" meta:resourcekey="TemplateFieldResource2">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEditar" runat="server" 
                                ImageUrl="HTML/Images/pencil.gif"  ToolTip="Modificar estado" 
                                CommandName="Editar" CommandArgument='<%# Bind("COD_VISITA") %>' 
                                meta:resourcekey="btnEditarResource2" />
                            </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>                 
            </Columns>
            <FooterStyle CssClass="cabeceraTabla"  />
            <PagerStyle  CssClass="cabeceraTabla" />
            <HeaderStyle CssClass="cabeceraTabla" />
        </asp:GridView>
    </div>
   </asp:Content>
