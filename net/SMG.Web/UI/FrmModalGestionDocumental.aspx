<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModalResponsive.master" AutoEventWireup="true" CodeFile="FrmModalGestionDocumental.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalGestionDocumental" EnableEventValidation="false" Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Import Namespace="Iberdrola.Commons.Web" %>

<asp:Content ID="FrmModalGestionDocumental" ContentPlaceHolderID="MainContent" runat="server">


    <div id="rpn_main_pnpConsulta" class="dxpnlControl_MetropolisBlueIB" style="width: 100%; padding: 0px;">
        <div class="col-xs-12">
            <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-2" for="rpn_main_pnpConsulta_txtReferencia_I" Text="Referencia: " meta:resourcekey="LabelResource1" />
            <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-2" ID="rpn_main_pnpConsulta_txtReferencia_I" name="rpn_main$pnpConsulta$txtReferencia" MaxLength="15" Enabled="False" meta:resourcekey="rpn_main_pnpConsulta_txtReferencia_IResource1"/>
            <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-2" for="rpn_main_pnpConsulta_txtIdSolicitud_I" Text="ID Solicitud: " meta:resourcekey="LabelResource2"  />
            <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-2" ID="rpn_main_pnpConsulta_txtIdSolicitud_I" name="rpn_main$pnpConsulta$txtIdSolicitud" MaxLength="15" Enabled="False" meta:resourcekey="rpn_main_pnpConsulta_txtIdSolicitud_IResource1" />
        </div>
    </div>

    <div id="rpn_main_CbpLoadDocumentos" class="dxpnlControl_MetropolisBlueIB" style="width: 100%; padding: 0px;">
        <div class="col-xs-12">
            <asp:Label runat="server" ID="Label1" class="dxsmLevel1Categorized_MetropolisBlueIB" Text="Documentos por solicitud" Style="font-size: 10pt;" meta:resourcekey="Label1Resource1"  />
        </div>
    </div>
    <div runat="server" id="rpn_main_CbpLoadDocumentos_pnpDocumentos" class="dxpnlControl_MetropolisBlueIB" style="width: 98%; padding: 0px;">
        <div class="col-xs-12">
            <asp:GridView ID="grdListado" runat="server" CssClass="dxfmControl_MetropolisBlueIB" AutoGenerateColumns="False"
                DataKeyNames="IdDocumento" OnRowDataBound="OnGrdListado_RowDataBound" OnRowCommand="OnGrdListado_RowCommand" meta:resourcekey="grdListadoResource1" >

                <HeaderStyle HorizontalAlign="Center" CssClass="dxfmGridHeader" />
                <RowStyle CssClass="dxfmGridTableRow" />
                <%--<AlternatingRowStyle CssClass="dxfm-fileSI" />
                <SelectedRowStyle CssClass="dxgvFocusedRow_MetropolisBlue" />
                <FooterStyle CssClass="dxgvHeader_MetropolisBlue" />
                <PagerStyle CssClass="dxgvHeader_MetropolisBlue" />--%>
                <Columns>
                    <asp:TemplateField HeaderText="" meta:resourcekey="TemplateFieldResource1" >
                        <ItemTemplate>
                            <asp:ImageButton ID="btnVerDocRow" runat="server"
                                ImageUrl="HTML/Images/Consulta.png"
                                ToolTip="Ver Documento"
                                CommandName="VerDocumento" OnClientClick="window.document.forms[0].target='_new'; setTimeout(function(){window.document.forms[0].target='';}, 500);"
                                CommandArgument='<%# Bind("IdDocumento") %>' meta:resourcekey="btnVerDocRowResource1"  />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="25px" />
                        <ItemStyle HorizontalAlign="Center" Width="25px" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="IdDocumento" HeaderText="IdDocumento" SortExpression="IdDocumento" ReadOnly="True" meta:resourcekey="BoundFieldResource1" >
                        <ItemStyle HorizontalAlign="Right" Width="85px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CodContrato" HeaderText="CodContrato" SortExpression="CodContrato" ReadOnly="True" meta:resourcekey="BoundFieldResource2" >
                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="IdSolicitud" HeaderText="IdSolicitud" SortExpression="IdSolicitud" ReadOnly="True" meta:resourcekey="BoundFieldResource3" >
                        <ItemStyle HorizontalAlign="Right" Width="70px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NombreDocumento" HeaderText="Nombre Documento" SortExpression="NombreDocumento" ReadOnly="True" meta:resourcekey="BoundFieldResource4" >
                        <ItemStyle HorizontalAlign="Right" Width="350px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CodTipoDocumento" HeaderText="Cod Tipo Documento" SortExpression="CodTipoDocumento" ReadOnly="True" meta:resourcekey="BoundFieldResource5" >
                        <ItemStyle HorizontalAlign="Right" Width="80px"  />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DescTipoDocumento" HeaderText="Desc Tipo Documento" SortExpression="DescTipoDocumento" ReadOnly="True" meta:resourcekey="BoundFieldResource6" >
                        <ItemStyle HorizontalAlign="Right" Width="350px"  />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FechaEnvioDelta" HeaderText="Fecha Envio Delta" SortExpression="FechaEnvioDelta" ReadOnly="True" meta:resourcekey="BoundFieldResource7" >
                        <ItemStyle HorizontalAlign="Right" Width="120px"  />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>


