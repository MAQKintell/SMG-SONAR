<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageResponsive.master" AutoEventWireup="true" CodeFile="FrmContratosNoFacturar.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmContratosNoFacturar" EnableEventValidation="false" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Import Namespace="Iberdrola.Commons.Web" %>

<asp:content id="FrmContratosNoFacturar" contentplaceholderid="MainContent" runat="server">


    <div id="rpn_main_pnpConsulta" class="dxpnlControl_MetropolisBlueIB" style="width: 100%; padding: 0px;">
        <div class="col-xs-12">
            <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-1" for="rpn_main_pnpConsulta_txtReferencia_I" Text="Referencia:" meta:resourcekey="LabelResource1" />
            <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-2" ID="rpn_main_pnpConsulta_txtReferencia_I" name="rpn_main$pnpConsulta$txtReferencia" MaxLength="23" meta:resourcekey="rpn_main_pnpConsulta_txtReferencia_IResource1" />

            <div id="rpn_main_pnpConsulta_btnConsultar" style="font-size: 10px; -moz-user-select: none; cursor: pointer;">
                <asp:Button ID="btnConsultar" runat="server" Text="BUSCAR" CssClass="dxbButton_MaterialCompact dxbButtonSys dxbTSys"  OnClick="rpn_main_pnpConsulta_btnConsultar_I_Click" meta:resourcekey="btnConsultarResource1"/>
            </div>
        </div>
    </div>

    <div id="rpn_main_CbpLoadSolicitud" class="dxpnlControl_MetropolisBlueIB" style="width: 100%; padding: 0px;">
        <div class="col-xs-12">
            <asp:Label runat="server" ID="rpn_main_CbpLoadSolicitud_pnpNumSolicitud_lblInfCli" class="dxsmLevel1Categorized_MetropolisBlueIB" Text="Información Cliente" Style="font-size: 10pt;" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpNumSolicitud_lblInfCliResource1" />
        </div>
    </div>

    <div runat="server" id="rpn_main_CbpLoadSolicitud_pnpDatosCliente" class="dxpnlControl_MetropolisBlueIB" style="width: 100%; padding: 0px;">
        <div class="row">
            <div class="col-xs-12">
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-1" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_cmbTipoDocumento_I" Text="Nombre:" meta:resourcekey="LabelResource2" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-6" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNombreCli_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtNombreCli" ReadOnly="True" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNombreCli_IResource1" />
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-2" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_cmbTipoDocumento_I" Text="Num. Documento:" meta:resourcekey="LabelResource3" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-2" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNif_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtNif" ReadOnly="True" value="" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNif_IResource1" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-1" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtDirSuministro_I" Text="Calle:" meta:resourcekey="LabelResource4" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-8" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtDirSuministro_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtDirSuministro" ReadOnly="True" value="" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtDirSuministro_IResource1" />
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-1" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtCPostal_I" Text="C. Postal:" meta:resourcekey="LabelResource5" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-1" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtCPostal_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtCPostal" ReadOnly="True" MaxLength="5" value="" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtCPostal_IResource1" />
                </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-1" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtProvincia_I" Text="Provincia:" meta:resourcekey="LabelResource6" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-3" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtProvincia_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtProvincia" ReadOnly="True" MaxLength="50" value="" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtProvincia_IResource1" />
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-1" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtPoblacion_I" Text="Población:" meta:resourcekey="LabelResource7" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-3" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtPoblacion_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtPoblacion" ReadOnly="True" MaxLength="50" value="" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtPoblacion_IResource1" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-1" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtTipoEFV_I" Text="EFV:" meta:resourcekey="LabelResource8" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-4" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtTipoEFV_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtTipoEFV" ReadOnly="True" value="" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtTipoEFV_IResource1" />
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-2" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaAltaServicio_I" Text="Fecha Alta Servicio:" meta:resourcekey="LabelResource9" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-1" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaAltaServicio_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtFechaAltaServicio" ReadOnly="True" value="" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaAltaServicio_IResource1" />
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-2" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaBajaServicio_I" Text="Fecha Baja Servicio:" meta:resourcekey="LabelResource10" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-1" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaBajaServicio_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtFechaBajaServicio" ReadOnly="True" value="" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaBajaServicio_IResource1" />
            </div>
        </div>
         <div class="row">
            <div class="col-xs-12">
                <asp:Label runat="server" class="dxsmLevel2Categorized_MetropolisBlueIB col-xs-1" for="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtMotivoBaja_I" Text="Motivo Baja:" meta:resourcekey="LabelResource11" />
                <asp:TextBox runat="server" class="dxeEditArea_MetropolisBlueIB dxeEditAreaSys dxeCaptionCell_MetropolisBlueIB col-xs-10" ID="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtMotivoBaja_I" name="rpn_main$CbpLoadSolicitud$pnpDatosCliente$txtMotivoBaja" value="" meta:resourcekey="rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaBajaServicio_IResource1" />
            </div>
        </div>
    </div>
    
    <div id="rpn_main_btnAceptar" class="dxpnlControl_MetropolisBlueIB" style="width: 100%; padding: 0px; margin-top: 5px;">
        <div class="row">
            <div class="col-xs-11">
                <div class="main-header__right">
                    <div id="rpn_main_btnAceptar_btn" style="font-size: 10px; -moz-user-select: none; cursor: pointer;">
                        <asp:Button ID="btnAceptar" runat="server" Text="ACEPTAR" CssClass=" dxbButton_MaterialCompact dxbButtonSys dxbTSys" OnClick="btnAceptar_Click" meta:resourcekey="btnAceptarResource1" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:content>


