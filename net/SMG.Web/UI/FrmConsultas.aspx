<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmConsultas.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmConsultas" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmConsultas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> 
    <LINK href="../css/ventan-modal-ie.css" type="text/css" rel="stylesheet">
	<script src="../js/ventana-modal.js" type="text/javascript"></script>
        <script type="text/javascript">

        function ExportarExcel() {
            //alert('qqq');
            //alert('II');
//            var btnAccion = document.getElementById("ctl00$ContentPlaceHolderContenido$btnExcel");
//           
//            btnAccion.click();
            
            abrirVentanaLocalizacion("../UI/AbrirExcel.aspx", 600, 350, "ventana-modal","EXPORTAR EXCEL","0","1");
           }
        </script>
    <div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Consultas" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
    <asp:ScriptManager ID="SM" runat="server">
    </asp:ScriptManager>
    <div id="divFiltros" style="width: 954px; position: absolute; top: 30px; left: 6px;
        height: 242px;">
        <asp:Panel ID="Panel2" CssClass="labelFormulario" runat="server" GroupingText="Consultas"
            Width="961px" meta:resourcekey="Panel2Resource1">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:RadioButtonList ID="RadioConsultas" runat="server" OnSelectedIndexChanged="OnSelectedIndexChanged_RadioConsultas"
                        AutoPostBack="True" CssClass="campoFormulario" Height="121px" 
                        meta:resourcekey="RadioConsultasResource1">
                        <asp:ListItem Selected="True" Value="1" meta:resourcekey="ListItemResource1">Contratos dados de baja</asp:ListItem>
                        <asp:ListItem Value="2" Enabled="False" meta:resourcekey="ListItemResource2">Mantenimientos por provincia</asp:ListItem>
                        <asp:ListItem Value="3" Enabled="False" meta:resourcekey="ListItemResource3">Cambio de teléfonos sobre visitas Canceladas por cliente no localizable</asp:ListItem>
                        <asp:ListItem Value="4" Enabled="False" meta:resourcekey="ListItemResource4">Vencidos posibles por mes</asp:ListItem>
                        <asp:ListItem Value="5" Enabled="False" meta:resourcekey="ListItemResource5">Contratos facturados por proveedor</asp:ListItem>
                        <asp:ListItem Value="6" meta:resourcekey="ListItemResource6">Contratos dados de baja POR FECHAS</asp:ListItem>
                        <asp:ListItem Value="7" Enabled="False" meta:resourcekey="ListItemResource7">Contratos sin ningún teléfono de contacto</asp:ListItem>
                        <asp:ListItem Value="8" meta:resourcekey="ListItemResource8">Contratos dado de baja CON VISITA POR FECHAS</asp:ListItem>
                        <asp:ListItem Value="9" meta:resourcekey="ListItemResource9">Visitas con PRL</asp:ListItem>
                    </asp:RadioButtonList>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="divCodContrato" style="position: absolute; top: 17px; left: 420px">
                        <asp:PlaceHolder ID="placeControlesOp1" runat="server" Visible="False">
                            <asp:Label ID="lblCodContrato" runat="server" Text="Código de contrato: " 
                                meta:resourcekey="lblCodContratoResource1"></asp:Label>
                            <asp:TextBox ID="txtCodContrato" CssClass="campoFormulario" Width="60px" runat="server"
                                MaxLength="10" meta:resourcekey="txtCodContratoResource1"></asp:TextBox>
                            <asp:CustomValidator CssClass="errorFormulario" ID="txtCodContratoValidatorCustomValidator"
                                runat="server" ErrorMessage="*" 
                                OnServerValidate="txtCodContratoCustomValidate" 
                                meta:resourcekey="txtCodContratoValidatorCustomValidatorResource1"></asp:CustomValidator>
                        </asp:PlaceHolder>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            
             <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div id="div2" style="position: absolute; top: 181px; left: 420px">
                        <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="False">
                            <asp:Label ID="Label3" runat="server" Text="Desde: " 
                                meta:resourcekey="Label3Resource1"></asp:Label>
                            <asp:TextBox ID="txtDesde0" CssClass="campoFormulario" Width="60px" runat="server"
                                MaxLength="10" meta:resourcekey="txtDesde0Resource1"></asp:TextBox>
                                <img id="img3" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" 
                                onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtDesde0');" >
                                &nbsp;&nbsp;
                            <asp:Label ID="Label4" runat="server" Text="Hasta: " 
                                meta:resourcekey="Label4Resource1"></asp:Label>
                            <asp:TextBox ID="txtHasta0" CssClass="campoFormulario" Width="60px" runat="server"
                                MaxLength="10" meta:resourcekey="txtHasta0Resource1"></asp:TextBox>
                                <img id="img4" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" 
                                onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtHasta0');" >
                        </asp:PlaceHolder>
                    </div>
                    <div ID="div1" style="position: absolute; top: 131px; left: 420px">
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="False">
                            <asp:Label ID="Label1" runat="server" Text="Desde: " 
                                meta:resourcekey="Label1Resource1"></asp:Label>
                            <asp:TextBox ID="txtDesde" runat="server" CssClass="campoFormulario" 
                                MaxLength="10" Width="60px" meta:resourcekey="txtDesdeResource1"></asp:TextBox>
                            <img ID="img2" alt="Calendario" 
                                onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtDesde');" 
                                src="HTML/IMAGES/imagenCalendario.gif"> &nbsp;&nbsp;
                            <asp:Label ID="Label2" runat="server" Text="Hasta: " 
                                meta:resourcekey="Label2Resource1"></asp:Label>
                            <asp:TextBox ID="txtHasta" runat="server" CssClass="campoFormulario" 
                                MaxLength="10" Width="60px" meta:resourcekey="txtHastaResource1"></asp:TextBox>
                            <img ID="img1" alt="Calendario" 
                                onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtHasta');" 
                                src="HTML/IMAGES/imagenCalendario.gif">
                            </img></img></asp:PlaceHolder>
                    </div>
                    <div id="div3" style="position: absolute; top: 210px; left: 420px">
                        <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible="False">
                            <asp:Label ID="Label5" runat="server" Text="Desde: " 
                                meta:resourcekey="Label3Resource1"></asp:Label>
                            <asp:TextBox ID="txtDesdePRL" CssClass="campoFormulario" Width="60px" runat="server"
                                MaxLength="10" meta:resourcekey="txtDesde0Resource1"></asp:TextBox>
                                <img id="img3" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" 
                                onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtDesdePRL');" >
                                &nbsp;&nbsp;
                            <asp:Label ID="Label6" runat="server" Text="Hasta: " 
                                meta:resourcekey="Label4Resource1"></asp:Label>
                            <asp:TextBox ID="txtHastaPRL" CssClass="campoFormulario" Width="60px" runat="server"
                                MaxLength="10" meta:resourcekey="txtHasta0Resource1"></asp:TextBox>
                                <img id="img4" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" 
                                onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtHastaPRL');" >
                        </asp:PlaceHolder>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
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
    <div id="divBotonRealizarConsulta" style="position: absolute; top: 545px; left: 393px;">
        <asp:Button CssClass="botonFormulario" ID="btnRealizarConsulta" runat="server" Text="Realizar Consulta"
            OnClientClick="MostrarCapaEspera();" OnClick="OnRealizarConsulta_Click" 
            TabIndex="1" meta:resourcekey="btnRealizarConsultaResource1" />
    </div>
    <div id="divBotonVolver">
        <asp:Button CssClass="botonFormulario" ID="btnVolver" runat="server" Text="Volver"
            OnClick="OnBtnVolver_Click" TabIndex="2" 
            meta:resourcekey="btnVolverResource1" />
    </div>
</asp:Content>
