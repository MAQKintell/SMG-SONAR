<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" CodeFile="FrmModalEquipamiento.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalEquipamiento" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="Iberdrola.SMG.BLL" %>

<asp:Content ID="FrmModalEquipamiento" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <asp:HiddenField ID="hdnCodigoContrato" runat="server" />
    <asp:HiddenField ID="hdnCalderaEliminada" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTieneCaldera" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEstadoEquipamiento" runat="server" />
    <asp:HiddenField ID="hdnIndexEquipamientoSeleccionado" runat="server" />
    
    
    

    <div id="divTxtMarcaCaldera">
        <asp:TextBox ID="txtMarcasCaldera" Visible="False" width="120px" runat="server" 
            TabIndex="2" MaxLength="50" meta:resourcekey="txtMarcasCalderaResource1"></asp:TextBox>
        <asp:CustomValidator ID="txtMarcasCalderaCustomValidator" 
            CssClass="errorFormulario" runat="server" ErrorMessage="***" 
            ControlToValidate="txtMarcasCaldera" 
            OnServerValidate="OnTxtMarcasCaldera_ServerValidate" ValidateEmptyText="True"
        ToolTip= "Debe seleccionar un valor" Enabled="False" 
            meta:resourcekey="txtMarcasCalderaCustomValidatorResource1"></asp:CustomValidator>
    </div>
    
    <div id="divCmbMarcaCaldera" style="z-index:1">
        <asp:DropDownList ID="cmbMarcasCaldera" style="width:140px" runat="server" 
            TabIndex="3" meta:resourcekey="cmbMarcasCalderaResource1">
        </asp:DropDownList>
    </div>
    
    
    <div id="divTxtPotenciaCaldera" 
        style="position:absolute; top: 65px; left: 187px; z-index:900">
        <asp:TextBox ID="txtPotencia" runat="server" Width="109px" TabIndex="6" 
            MaxLength="12" CssClass="campoFormularioNumerico" 
            meta:resourcekey="txtPotenciaResource1"></asp:TextBox>
                <asp:CustomValidator ID="txtPotenciaCustomValidator" 
            CssClass="errorFormulario" runat="server" ErrorMessage="*" 
            ControlToValidate="txtPotencia" OnServerValidate="OntxtPotencia_ServerValidate" ValidateEmptyText="True"
                 Enabled="False" 
            meta:resourcekey="txtPotenciaCustomValidatorResource1"></asp:CustomValidator>
    </div>
    
    <div id="divCmbPotenciaCaldera" 
        
        style="position:absolute; top: 67px; left: 192px; z-index:899; width: 118px;">
        <asp:DropDownList ID="cmbTipoPotencia" runat="server" TabIndex="6" Width="127px" 
            OnSelectedIndexChanged="CmbTipoPotencia_SelectedIndexChanged" 
            AutoPostBack="True" meta:resourcekey="cmbTipoPotenciaResource1">
                </asp:DropDownList>
    </div>
    
    
    
    <div id="divCaldera" style="position:absolute; top: 5px; left: 32px;" >
     <asp:Panel ID="PanelCaldera" runat="server" GroupingText="Datos Caldera" 
            Height="200px" width="569px" CssClass="labelFormulario" 
            meta:resourcekey="PanelCalderaResource1">
        <div id="divCaldera2" >
        <asp:Table ID="Table1" runat="server" HorizontalAlign="Center" 
                meta:resourcekey="Table1Resource1">
        <asp:TableRow ID="TableRow1" runat="server">
            <asp:TableCell ID="TableCell1" runat="server" >
            <asp:Label 
                CssClass="labelFormulario" ID="Label1" runat="server" Text="Tipo Caldera:" 
                meta:resourcekey="Label1Resource1"></asp:Label>
</asp:TableCell>
            <asp:TableCell ID="TableCell2" runat="server" ><asp:DropDownList 
                ID="cmbTiposCaldera" runat="server" Width="130px" TabIndex="1" 
                meta:resourcekey="cmbTiposCalderaResource1"></asp:DropDownList>
<asp:CustomValidator ID="cmbTiposCalderaCustomValidator" CssClass="errorFormulario" 
                runat="server" ErrorMessage="*" ControlToValidate="cmbTiposCaldera" 
                OnServerValidate="OncmbTiposCaldera_ServerValidate" ValidateEmptyText="True"
        ToolTip= "Debe seleccionar un valor"  Enabled="False" 
                meta:resourcekey="cmbTiposCalderaCustomValidatorResource1"></asp:CustomValidator>
</asp:TableCell>
            <asp:TableCell ID="TableCell3" runat="server" ><asp:Label 
                CssClass="labelFormulario" ID="Label4" runat="server" Text="Marca Caldera:" 
                meta:resourcekey="Label4Resource1"></asp:Label>
</asp:TableCell>
            <asp:TableCell ID="TableCell4" runat="server" >
                &nbsp;
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server" meta:resourcekey="TableRow2Resource1">
            <asp:TableCell ID="TableCell5" runat="server" ><asp:Label 
                CssClass="labelFormulario" ID="Label2" runat="server" Text="Modelo Caldera:" 
                meta:resourcekey="Label2Resource1"></asp:Label>
</asp:TableCell>
            <asp:TableCell ID="TableCell6" runat="server" ><asp:TextBox 
                CssClass="campoFormulario" ID="txtModeloCaldera" runat="server" Width="125px" 
                TabIndex="4" MaxLength="50" meta:resourcekey="txtModeloCalderaResource1"></asp:TextBox>
</asp:TableCell>
            <asp:TableCell ID="TableCell7" runat="server" ><asp:Label 
                CssClass="labelFormulario" ID="Label5" runat="server" Text="Uso:" 
                meta:resourcekey="Label5Resource1"></asp:Label>
</asp:TableCell>
            <asp:TableCell ID="TableCell8" runat="server" ><asp:DropDownList ID="cmbUsoCaldera" 
                runat="server" TabIndex="5" meta:resourcekey="cmbUsoCalderaResource1"></asp:DropDownList>
<asp:CustomValidator ID="cmbUsoCalderaCustomValidator" CssClass="errorFormulario" 
                runat="server" ErrorMessage="*" ControlToValidate="cmbUsoCaldera" 
                OnServerValidate="OncmbUsoCaldera_ServerValidate" ValidateEmptyText="True"
                     Enabled="False" 
                meta:resourcekey="cmbUsoCalderaCustomValidatorResource1"></asp:CustomValidator>
</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow3" runat="server" meta:resourcekey="TableRow3Resource1">
            <asp:TableCell ID="TableCell9" runat="server" ><asp:Label 
                CssClass="labelFormulario" ID="Label3" runat="server" Text="Potencia:" 
                meta:resourcekey="Label3Resource1"></asp:Label>
</asp:TableCell>
            <asp:TableCell ID="TableCell10" runat="server" ></asp:TableCell>
            <asp:TableCell ID="TableCell11" runat="server" ><asp:Label 
                CssClass="labelFormulario" ID="Label6" runat="server" Text="Año:" 
                meta:resourcekey="Label6Resource1"></asp:Label>
</asp:TableCell>
            <asp:TableCell ID="TableCell12" runat="server" ><asp:TextBox ID="txtAnio" 
                runat="server" Width="25px" TabIndex="7" MaxLength="4" 
                meta:resourcekey="txtAnioResource1"></asp:TextBox>
</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
     
    </div>
        <div id="botonLimpiar" align="right">
        
        <asp:Button CssClass="botonFormulario" ID="bntLimpiarCaldera" runat="server" style="text-align: center" 
            Text="Limpiar Información Caldera" Width="183px"  
                OnClick="OnBntLimpiarCaldera_Click" TabIndex="8" 
                meta:resourcekey="bntLimpiarCalderaResource1"/>
           </div>
        <br />
    </asp:Panel>
    </div>

    <div id="divEquipamiento" style="position:absolute; top: 170px; left: 28px;" >
    <asp:Panel ID="pnEquipamientos" runat="server" GroupingText="Equipamientos"  
            Height="180px" width="290px" CssClass="labelFormulario" 
            meta:resourcekey="pnEquipamientosResource1">
        <asp:Panel ID="pnGridEquipamientos" runat="server" Height="130px" 
            ScrollBars="Auto" Width="142px" CssClass="panelTabla" 
            meta:resourcekey="pnGridEquipamientosResource1">
            <asp:GridView ID="grdEquipamientos" runat="server" AutoGenerateColumns="False" 
                CssClass="contenidoTabla" OnRowDataBound="OnGrdEquipamientos_RowDataBound" 
                OnSelectedIndexChanged="OnRowSelected_Click" EnableModelValidation="True" 
                meta:resourcekey="grdEquipamientosResource1">
                <AlternatingRowStyle CssClass="filaAlterna" />
                <Columns>
                    <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                        <asp:HiddenField ID="hdnIdEquipamiento" Value='<%#Bind("Id")%>">' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DescripcionTipoEquipamiento" 
                        HeaderText="Tipo Equipm." SortExpression="DescripcionTipoEquipamiento" 
                        meta:resourcekey="BoundFieldResource1" >
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Potencia" HeaderText="Potencia (KW)" 
                        SortExpression="Potencia" meta:resourcekey="BoundFieldResource2" >
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
                <EditRowStyle CssClass="cabeceraTabla"  />
                <FooterStyle CssClass="cabeceraTabla" />
                <HeaderStyle CssClass="cabeceraTabla" />
                <PagerStyle CssClass="cabeceraTabla" />
                <RowStyle CssClass="filaNormal" />
                <SelectedRowStyle CssClass="filaSeleccionada" />
            </asp:GridView>
         </asp:Panel>   
    <div id="divOpcionesEquipamiento" style="position:absolute; top: 20px; left: 175px; height: 93px; width: 107px;">
     <asp:Table ID="tbOperacionesEquipamiento" runat="server" Height="81px" 
            Width="60px" meta:resourcekey="tbOperacionesEquipamientoResource1">
                <asp:TableRow ID="TableRow4" runat="server" >
                    <asp:TableCell ID="TableCell13" runat="server" ><asp:Button 
                        CssClass="botonFormulario" ID="btAnadirEquipamiento" runat="server" 
                        Text="Añadir" Width="100px" onclick="btAnadirEquipamiento_Click" TabIndex="9" 
                        meta:resourcekey="btAnadirEquipamientoResource1"/>
</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow5" runat="server" >
                    <asp:TableCell ID="TableCell14" runat="server" ><asp:Button 
                        CssClass="botonFormulario" ID="btnModificarEquipamiento" runat="server" 
                        Text="Modificar"  Width="100px" onclick="btModificarEquipamiento_Click" 
                        Enabled = "False" TabIndex="10" 
                        meta:resourcekey="btnModificarEquipamientoResource1"/>
</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow6" runat="server" >
                    <asp:TableCell ID="TableCell15" runat="server" ><asp:Button 
                        CssClass="botonFormulario" ID="btnBorrarEquipamiento" runat="server" 
                        Text="Borrar"  Width="100px" onclick="btnBorrarEquipamiento_Click"  
                        Enabled = "False" TabIndex="11" 
                        meta:resourcekey="btnBorrarEquipamientoResource1" />
</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow7" runat="server" >
                    <asp:TableCell ID="TableCell16" runat="server" HorizontalAlign="Center" ><asp:Label 
                        CssClass="labelFormulario" ID="lblNumeroEquipamientos" runat="server" 
                        Text="Número de Equipamientos: "  Width="100px" 
                        meta:resourcekey="lblNumeroEquipamientosResource1" />
<asp:Label CssClass="labelFormularioValor" ID="lblNumeroEquipamientosValor" runat="server"  
                        Width="100px" meta:resourcekey="lblNumeroEquipamientosValorResource1" />
</asp:TableCell></asp:TableRow></asp:Table></div></asp:Panel></div><div id="divDetalleEquipamiento" 
        style="position:absolute; top: 170px; left: 320px; height: 200px; width: 278px;">
      <asp:Panel 
            ID="pnDetalleEquipamiento" runat="server" Width="277px" Height="180px" 
            CssClass="labelFormulario" GroupingText="Detalle de Equipamiento" 
            Visible="False" meta:resourcekey="pnDetalleEquipamientoResource1"><asp:Table 
                ID="tbDatosEquipamiento" runat="server" HorizontalAlign="Center" 
                meta:resourcekey="tbDatosEquipamientoResource1"><asp:TableRow ID="TableRow8" 
                    runat="server"><asp:TableCell 
                        ID="TableCell17" runat="server"><asp:Label 
                        CssClass="labelFormulario" ID="Label7" runat="server" Text="Tipo Equipamiento:" 
                        meta:resourcekey="Label7Resource1"></asp:Label>
</asp:TableCell><asp:TableCell ID="TableCell18" runat="server" ><asp:DropDownList 
                        ID="cmbTipoEquipamiento" autopostback="True" runat="server" TabIndex="12" 
                        OnSelectedIndexChanged="CmbTipoEquipamiento_SelectedIndexChanged" 
                        meta:resourcekey="cmbTipoEquipamientoResource1"></asp:DropDownList>
<asp:CustomValidator ID="cmbTipoEquipamientoCustomValidator" CssClass="errorFormulario" 
                        runat="server" ErrorMessage="*" ControlToValidate="cmbTipoEquipamiento" 
                        OnServerValidate="OncmbTipoEquipamiento_ServerValidate" ValidateEmptyText="True"
                 Enabled="False" meta:resourcekey="cmbTipoEquipamientoCustomValidatorResource1"></asp:CustomValidator>
</asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow9" runat="server" ><asp:TableCell ID="TableCell19" 
                        runat="server" ><asp:Label 
                        CssClass="labelFormulario" ID="Label8" runat="server" Text="Nº Fuegos:" 
                        meta:resourcekey="Label8Resource1"></asp:Label>
</asp:TableCell><asp:TableCell ID="TableCell20" runat="server"  ><asp:TextBox 
                        ID="txtPotenciaEquipamiento" runat="server" Width="70px" TabIndex="13" 
                        MaxLength="12" CssClass="campoFormularioNumerico" 
                        meta:resourcekey="txtPotenciaEquipamientoResource1"></asp:TextBox>
<asp:CustomValidator ID="txtPotenciaEquipamientoCustomValidator" CssClass="errorFormulario" 
                        runat="server" ErrorMessage="*" ControlToValidate="txtPotenciaEquipamiento" 
                        OnServerValidate="txtPotenciaEquipamiento_ServerValidate" ValidateEmptyText="True"
                 Enabled="False" 
                        meta:resourcekey="txtPotenciaEquipamientoCustomValidatorResource1"></asp:CustomValidator>
</asp:TableCell></asp:TableRow></asp:Table><div style="text-align: center">    
               <asp:Button 
                    CssClass="botonFormulario" ID="btnAceptarEquipamiento" runat="server" 
                    Text="Aceptar" onclick="OnBtnAceptarEquipamiento_Click" TabIndex="14" 
                    meta:resourcekey="btnAceptarEquipamientoResource1" /><asp:Button 
                    CssClass="botonFormulario" ID="btnCancelarEquipamiento" runat="server" 
                    Text="Cancelar" onclick="OnBtnCancelarEquipamiento_Click" TabIndex="15" 
                    meta:resourcekey="btnCancelarEquipamientoResource1" /></div>     
            <div style="text-align: center;height:26px;"></div>
    </asp:Panel>
    </div>

    <div id="divBotonAceptar" 
        style="position: absolute; top: 368px; left: 120px; width: 157px; height: 20px;">
        <asp:Button 
            CssClass="botonFormulario" ID="btnAceptar" runat="server" Height="22px" Text="Salvar Cambios" 
            Width="154px" OnClick="OnBtnAceptar_Click" TabIndex="16" 
            meta:resourcekey="btnAceptarResource1"/></div>
    
    <div id="divBotonCancelar" 
        
        style="position: absolute; top: 368px; left: 360px; width: 163px; height: 24px;">
        <asp:Button 
            CssClass="botonFormulario" ID="btnCancelar" runat="server" Height="22px" 
            Text="Cancelar cambios" Width="154px" OnClick="OnBtnCancelar_Click" 
            TabIndex="17" meta:resourcekey="btnCancelarResource1" /></div>
    
    <script language="javascript" type="text/javascript">
        function DisplayText() {
            var txt = document.getElementById("ctl00$ContentPlaceHolderContenido$txtMarcasCaldera");
            var cmb = document.getElementById("ctl00$ContentPlaceHolderContenido$cmbMarcasCaldera");

            if (txt && cmb && cmb.selectedIndex > -1) {
                txt.value = cmb.options[cmb.selectedIndex].text;
                txt.focus();
            }
        }

        function EsCalderaValida() {

            var cmbTipoCaldera = document.getElementById("ctl00$ContentPlaceHolderContenido$cmbTiposCaldera");
            var cmbUso = document.getElementById("ctl00$ContentPlaceHolderContenido$cmbUsoCaldera");
            var txtPotencia = document.getElementById("ctl00$ContentPlaceHolderContenido$txtPotencia");
            var txtMarcasCaldera = document.getElementById("ctl00$ContentPlaceHolderContenido$txtMarcasCaldera");

            if (cmbTipoCaldera.selectedIndex < 1) {
                return false;
            }
            if (cmbUso.selectedIndex < 1) {
                return false;
            }
            if (!txtPotencia.value) {
                return false;
            }
            if (!txtMarcasCaldera.value) {
                return false;
            }

            return true;
        }

        function ValidarCaldera() {
            var hdnTieneCaldera = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnTieneCaldera");
            var hdnEliminarCaldera = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnEliminarCaldera");


            if (hdnTieneCaldera.value == '1') {
                if (!EsCalderaValida()) {
                    return confirm("<%=Resources.TextosJavaScript.TEXTO_DATOS_CALDERAS_NO_COMPLETOS%>"); //"Los datos de la caldera no están completos, no se insertará la caldera.¿Desea continuar?");
                }
            }

        }

        // ponemos el foco en el primer control
        var cmbTipoCaldera = document.getElementById("ctl00$ContentPlaceHolderContenido$cmbTiposCaldera");

        if (cmbTipoCaldera) {
            try {
                //cmbTipoCaldera.focus();
            }
            catch (ex) {
                // no hacemos nada, si salta es porque no se le puede poner el foco.
            }
        }
    </script>
</asp:Content>