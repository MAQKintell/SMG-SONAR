<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPlantillaCierreDatosVisita.aspx.cs" Inherits="UI_FrmPlantillaCierreDatosVisita" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CIERRE DATOS VISITA</title>
    <style type="text/css">
        .labelFormulario
        {
            font-family:Trebuchet MS;
	        text-decoration:none;
	        color:#262626;
            font-size:11px;
        }
        .botonFormulario
        {
            font-family:Trebuchet MS;
            border-style:outset;
            border-color:#34721f;
            background-color:#34721f;
            border:1px solid #262626; 
            color:white;    
            font-size:11px;
            height:20px;
            font-weight:bold;
        }
        #Text1
        {
            width: 119px;
        }
        #txtNombre
        {
            width: 609px;
        }
        #txtDNI
        {
            width: 163px;
        }
        #txtEmail
        {
            width: 608px;
        }
        #txtcontrato
        {
            width: 267px;
        }
        #txtModalidad
        {
            width: 237px;
        }
        #txtdireccion
        {
            width: 598px;
        }
        #txtNumero
        {
            width: 49px;
        }
        #txtPiso
        {
            width: 53px;
        }
        #txtFechavisita
        {
            width: 114px;
        }
        #txtPoblacion
        {
            width: 234px;
        }
        .style1
        {
            width: 430px;
        }
        .labelFormularioValor
        {
            font-family:Trebuchet MS;
	        text-decoration:none;
            font-size:17px;
            font-weight:bold;
            font-style:italic;
            text-decoration:underline;
        }
    </style>
    <script src="../js/ventana-modal.js" type="text/javascript"></script>
</head>
<body style="background:url(../Imagenes/Certificado SMG 15-12-14-page-001.jpg);">
    <form id="form1"  runat="server">
        <asp:HiddenField runat="server" ID="hdnCodVisita" Value="0" />   

        <div id="divCCBB" 
            style="position:absolute; top: 26px; left: 736px; width: 298px; height: 85px;">
            <table>
                <tr>
                    <td>
                        <asp:Image runat="server" ImageUrl='../Imagenes/CCBB.png' Height="53px" 
                            Width="301px" meta:resourcekey="ImageResource1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox runat="server" ID="txtCCBB_1" Width="244px" 
                            meta:resourcekey="txtCCBB_1Resource1"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divFechaVisita" style="position:absolute; top: 194px; left: 1022px;">
            <asp:TextBox runat="server" id="txtFechavisita_23" type="text" Width="127px" 
                meta:resourcekey="txtFechavisita_23Resource1" />
        </div>

        <div id="divDatosCliente" style="position:absolute; top: 270px; left: 255px; width: 906px; height: 167px;">
        <table style="width: 100%;">
            <tr>
                <td class="style1" >
                    <asp:TextBox runat="server" id="txtNombre" 
                        meta:resourcekey="txtNombreResource1"  />
                </td>
                <td colspan="2">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" id="txtDNI" meta:resourcekey="txtDNIResource1"  />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:TextBox runat="server" id="txtEmail_3" 
                        meta:resourcekey="txtEmail_3Resource1"  />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Checkbox runat="server" id="chkNoEmail" 
                        meta:resourcekey="chkNoEmailResource1"  />
                </td>

            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;&nbsp;<asp:TextBox runat="server" id="txtcontrato" 
                        meta:resourcekey="txtcontratoResource1"  />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" id="txtModalidad" 
                        meta:resourcekey="txtModalidadResource1"  />
                </td>
            </tr>
            <tr>
                <td class="style1">
                    <asp:TextBox runat="server" id="txtdireccion" 
                        meta:resourcekey="txtdireccionResource1"  />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" id="txtNumero" 
                        meta:resourcekey="txtNumeroResource1"  />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" id="txtPiso" meta:resourcekey="txtPisoResource1"  />
                </td>
            </tr>
             <tr>
                <td colspan="2">
                    <asp:TextBox runat="server" id="txtPoblacion" 
                        meta:resourcekey="txtPoblacionResource1"  />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" id="txtProvincia" 
                        meta:resourcekey="txtProvinciaResource1" />
                </td>
                <td>
                    <asp:TextBox runat="server" id="txtCP" meta:resourcekey="txtCPResource1"  />
                </td>
            </tr>
             <tr>
                <td colspan="3" class="style1">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Checkbox runat="server" id="chkContadorDentro_2" 
                        meta:resourcekey="chkContadorDentro_2Resource1"  />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Checkbox runat="server" id="chkContadorFuera_2" 
                        meta:resourcekey="chkContadorFuera_2Resource1"  />
                </td>
                
            </tr>
        </table>
   </div>

        <div id="divInstalador" 
        
        style="position:absolute; top: 476px; left: 255px; width: 906px; height: 122px;">
        <table>
            <tr>
                <td >
                    <asp:DropDownList ID="cmbTecnico_10" runat="server" 
                        meta:resourcekey="cmbTecnico_10Resource1" > 
                        <asp:ListItem Value="-1" 
                            Text="DEBE DE SELECCIONAR EL TECNICO QUE HA REALIZADO LA VISITA" 
                            meta:resourcekey="ListItemResource1"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" id="txtNIFTecnico_4" Width="138px" 
                        meta:resourcekey="txtNIFTecnico_4Resource1"  />
                </td>
            </tr>
            <tr>
                <td >
                    <asp:TextBox runat="server" id="txtNumInstalador_9" Width="225px" 
                        meta:resourcekey="txtNumInstalador_9Resource1"  />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" id="txtCategoriaInstalador_8" Width="243px" 
                        meta:resourcekey="txtCategoriaInstalador_8Resource1"  />
                </td>
            </tr>
            <tr>
                <td >
                    <asp:TextBox runat="server" id="txtEmpresaInstalador_7" Width="442px" 
                        meta:resourcekey="txtEmpresaInstalador_7Resource1"  />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" id="txtCIFEmpresaInstalador_6" Width="129px" 
                        meta:resourcekey="txtCIFEmpresaInstalador_6Resource1"  />
                </td>
            </tr>
            <tr>
                <td >
                    &nbsp;&nbsp;<asp:TextBox runat="server" id="txtNumeroEmpresaMantenedora_5" 
                        Width="631px" meta:resourcekey="txtNumeroEmpresaMantenedora_5Resource1"  />
                </td>
            </tr>
        </table>
   </div>

        <div id="divDatosinstalacionAparatos" 
        
        style="position:absolute; top: 660px; left: 93px; width: 906px; height: 83px;">
        <table>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txtCodigoInstalacion1_14" Width="63px" 
                        meta:resourcekey="txtCodigoInstalacion1_14Resource1"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="cmbMarcaCalderaInstalacion1_11" AutoPostBack="True" 
                        runat="server" meta:resourcekey="cmbMarcaCalderaInstalacion1_11Resource1" > 
                        <asp:ListItem Value="-1" Text="SELECCIONE LA MARCA" 
                            meta:resourcekey="ListItemResource2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cmbModeloCalderaInstalacion1_93" runat="server" 
                        meta:resourcekey="cmbModeloCalderaInstalacion1_93Resource1" > 
                        <asp:ListItem Value="-1" Text="SELECCIONE EL MODELO" 
                            meta:resourcekey="ListItemResource3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtAnhioInstalacion1_12" Width="63px" 
                        meta:resourcekey="txtAnhioInstalacion1_12Resource1"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPotenciaInstalacion1_13" Width="123px" 
                        meta:resourcekey="txtPotenciaInstalacion1_13Resource1"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCOInstalacion1" Width="80px" 
                        meta:resourcekey="txtCOInstalacion1Resource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txtCodigoInstalacion2_18" Width="63px" 
                        meta:resourcekey="txtCodigoInstalacion2_18Resource1"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="cmbMarcaCalderaInstalacion2_15" AutoPostBack="True" 
                        runat="server" meta:resourcekey="cmbMarcaCalderaInstalacion2_15Resource1" > 
                        <asp:ListItem Value="-1" Text="SELECCIONE LA MARCA" 
                            meta:resourcekey="ListItemResource4"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cmbModeloCalderaInstalacion2_94" runat="server" 
                        meta:resourcekey="cmbModeloCalderaInstalacion2_94Resource1" > 
                        <asp:ListItem Value="-1" Text="SELECCIONE EL MODELO" 
                            meta:resourcekey="ListItemResource5"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtAnhioInstalacion2_16" Width="63px" 
                        meta:resourcekey="txtAnhioInstalacion2_16Resource1"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPotenciaInstalacion2_17" Width="123px" 
                        meta:resourcekey="txtPotenciaInstalacion2_17Resource1"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCOInstalacion2" Width="80px" 
                        meta:resourcekey="txtCOInstalacion2Resource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txtCodigoInstalacion3_22" Width="63px" 
                        meta:resourcekey="txtCodigoInstalacion3_22Resource1"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="cmbMarcaCalderaInstalacion3_19" AutoPostBack="True" 
                        runat="server" meta:resourcekey="cmbMarcaCalderaInstalacion3_19Resource1" > 
                        <asp:ListItem Value="-1" Text="SELECCIONE LA MARCA" 
                            meta:resourcekey="ListItemResource6"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cmbModeloCalderaInstalacion3_95" runat="server" 
                        meta:resourcekey="cmbModeloCalderaInstalacion3_95Resource1" > 
                        <asp:ListItem Value="-1" Text="SELECCIONE EL MODELO" 
                            meta:resourcekey="ListItemResource7"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtAnhioInstalacion3_20" Width="63px" 
                        meta:resourcekey="txtAnhioInstalacion3_20Resource1"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPotenciaInstalacion3_21" Width="123px" 
                        meta:resourcekey="txtPotenciaInstalacion3_21Resource1"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCOInstalacion3" Width="80px" 
                        meta:resourcekey="txtCOInstalacion3Resource1"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>

        <div id="divDatosinstalacionTemperaturas" style="position:absolute; top: 747px; left: 377px; width: 627px; height: 96px;">
        <table>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txtTemperaturaMaximaACS_24" Width="38px" 
                        meta:resourcekey="txtTemperaturaMaximaACS_24Resource1"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtCaudalACS_25" Width="38px" 
                        meta:resourcekey="txtCaudalACS_25Resource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txtTemperaturaGases" Width="38px" 
                        meta:resourcekey="txtTemperaturaGasesResource1"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtPotenciaUtil_26" Width="38px" 
                        meta:resourcekey="txtPotenciaUtil_26Resource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txtTemperaturaAmbiente_27" Width="38px" 
                        meta:resourcekey="txtTemperaturaAmbiente_27Resource1"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtTemperaturaGeneradorEntrada_28" Width="14px" 
                        meta:resourcekey="txtTemperaturaGeneradorEntrada_28Resource1"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtTemperaturaGeneradorsalida_29" Width="14px" 
                        meta:resourcekey="txtTemperaturaGeneradorsalida_29Resource1"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>

        <div id="divOperacionesRealizadasRite" 
        
        style="position:absolute; top: 900px; left: 433px; width: 492px; height: 60px;">
        <table>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="chkLimpiezaQuemadorLimpiezaSI_48" 
                        meta:resourcekey="chkLimpiezaQuemadorLimpiezaSI_48Resource1" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkLimpiezaQuemadorLimpiezaNO_48" 
                        meta:resourcekey="chkLimpiezaQuemadorLimpiezaNO_48Resource1" />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkRegulacionAparatosSI_50" 
                        meta:resourcekey="chkRegulacionAparatosSI_50Resource1" />
                    &nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkRegulacionAparatosNO_50" 
                        meta:resourcekey="chkRegulacionAparatosNO_50Resource1" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="chkRevisionVasoExpansionSI_49" 
                        meta:resourcekey="chkRevisionVasoExpansionSI_49Resource1" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkRevisionVasoExpansionNO_49" 
                        meta:resourcekey="chkRevisionVasoExpansionNO_49Resource1" />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkEstanqueidadSI_54" 
                        meta:resourcekey="chkEstanqueidadSI_54Resource1" />
                    &nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkEstanqueidadNO_54" 
                        meta:resourcekey="chkEstanqueidadNO_54Resource1" />
                </td>
            </tr>
        </table>
    
    </div>

        <div id="divOperacionesRealizadasDiagnostico" 
        
        style="position:absolute; top: 982px; left: 433px; width: 492px; height: 200px;">
        <table>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="chkRevisiongeneralSI_52" 
                        meta:resourcekey="chkRevisiongeneralSI_52Resource1" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkRevisiongeneralNO_52" 
                        meta:resourcekey="chkRevisiongeneralNO_52Resource1" />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkTiroEvacuacionSI_57" 
                        meta:resourcekey="chkTiroEvacuacionSI_57Resource1" />
                    &nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkTiroEvacuacionNO_57" 
                        meta:resourcekey="chkTiroEvacuacionNO_57Resource1" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="chkHidraulicoSI_89" 
                        meta:resourcekey="chkHidraulicoSI_89Resource1" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkHidraulicoNO_89" 
                        meta:resourcekey="chkHidraulicoNO_89Resource1" />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkAislamientoTermicoSI_58" 
                        meta:resourcekey="chkAislamientoTermicoSI_58Resource1" />
                    &nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkAislamientoTermicoNO_58" 
                        meta:resourcekey="chkAislamientoTermicoNO_58Resource1" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="chkEstanqueidadConexionAparatosSI_88" 
                        meta:resourcekey="chkEstanqueidadConexionAparatosSI_88Resource1" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkEstanqueidadConexionAparatosNO_88" 
                        meta:resourcekey="chkEstanqueidadConexionAparatosNO_88Resource1" />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkAnalisisCombustionSI_59" 
                        meta:resourcekey="chkAnalisisCombustionSI_59Resource1" />
                    &nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkAnalisisCombustionNO_59" 
                        meta:resourcekey="chkAnalisisCombustionNO_59Resource1" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="chkEstanqueidadConductoEvacuacionSI_90" 
                        meta:resourcekey="chkEstanqueidadConductoEvacuacionSI_90Resource1" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkEstanqueidadConductoEvacuacionNO_90" 
                        meta:resourcekey="chkEstanqueidadConductoEvacuacionNO_90Resource1" />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkCaudalACSyPotenciaUtilSI_60" 
                        meta:resourcekey="chkCaudalACSyPotenciaUtilSI_60Resource1" />
                    &nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkCaudalACSyPotenciaUtilNO_60" 
                        meta:resourcekey="chkCaudalACSyPotenciaUtilNO_60Resource1" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="chkNivelesAguaSI_56" 
                        meta:resourcekey="chkNivelesAguaSI_56Resource1" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkNivelesAguaNO_56" 
                        meta:resourcekey="chkNivelesAguaNO_56Resource1" />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkRevisionSistemaControlSI_61" 
                        meta:resourcekey="chkRevisionSistemaControlSI_61Resource1" />
                    &nbsp;&nbsp;
                    <asp:CheckBox runat="server" ID="chkRevisionSistemaControlNO_61" 
                        meta:resourcekey="chkRevisionSistemaControlNO_61Resource1" />
                </td>
            </tr>
        </table>
    
    </div>

        <div id="divResultadoVisita" 
            
            
            style="position:absolute; top: 1140px; left: 94px; width: 1077px; height: 567px;">
            <table>
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chkVisitasinAnomlaias_62" 
                            meta:resourcekey="chkVisitasinAnomlaias_62Resource1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chkAnomaliasDetectadas_63" 
                            meta:resourcekey="chkAnomaliasDetectadas_63Resource1" />
                    </td>
                </tr>
                
                <tr>
                
                    <td>
                        <br />
                        <asp:textbox TextMode="MultiLine" runat="server" ID="txtAnomaliasPrincipales_64" 
                            Width="517px" Height="35px" 
                            meta:resourcekey="txtAnomaliasPrincipales_64Resource1" />
                    </td>
                    <td>
                        <br />&nbsp;&nbsp;
                        <asp:textbox TextMode="MultiLine" runat="server" 
                            ID="txtAnomaliasSecundarias_65" Width="510px" Height="35px" 
                            meta:resourcekey="txtAnomaliasSecundarias_65Resource1"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chkAnomaliaEmpresaDistribuidora_66" 
                            meta:resourcekey="chkAnomaliaEmpresaDistribuidora_66Resource1" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chkentregadoPresupuesto_67" 
                            meta:resourcekey="chkentregadoPresupuesto_67Resource1" />
                    </td>
                </tr>
                <tr>
                
                    <td colspan="2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox runat="server" ID="chkAnomaliasReparadas_69" 
                            meta:resourcekey="chkAnomaliasReparadas_69Resource1" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:textbox runat="server" ID="txtAnomaliasReparadas_70" Width="715px" 
                            meta:resourcekey="txtAnomaliasReparadas_70Resource1" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox runat="server" ID="chkAnomaliasNOReparadas" 
                            meta:resourcekey="chkAnomaliasNOReparadasResource1" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:textbox runat="server" ID="txtAnomaliasNOReparadas_71" Width="715px" 
                            meta:resourcekey="txtAnomaliasNOReparadas_71Resource1" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox runat="server" ID="chkclienteNOAcpetaPresupuesto_72" 
                            meta:resourcekey="chkclienteNOAcpetaPresupuesto_72Resource1" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox runat="server" ID="chkReparacionImposible_74" 
                            meta:resourcekey="chkReparacionImposible_74Resource1" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox runat="server" ID="chkReparacionPendienteNuevaCita_73" 
                            meta:resourcekey="chkReparacionPendienteNuevaCita_73Resource1" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox runat="server" ID="chkOtros_75" 
                            meta:resourcekey="chkOtros_75Resource1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:CheckBox runat="server" ID="chkInstalacionPrecintada_91" 
                            meta:resourcekey="chkInstalacionPrecintada_91Resource1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chkClienteNOPermitePrecinte_92" 
                            meta:resourcekey="chkClienteNOPermitePrecinte_92Resource1" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="3"><br /><br />
                        <asp:textbox TextMode="MultiLine" runat="server" 
                            ID="txtObservacionesDelInstalador_78" Width="1043px" Height="54px" 
                            meta:resourcekey="txtObservacionesDelInstalador_78Resource1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br /><br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:textbox runat="server" ID="txtDNIConformeCliente_81" 
                            meta:resourcekey="txtDNIConformeCliente_81Resource1" />
                    </td>
                    <td>
                    <br /><br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:textbox runat="server" TextMode="MultiLine" ID="txtObservacionesCliente_79" 
                            Height="43px" Width="449px" 
                            meta:resourcekey="txtObservacionesCliente_79Resource1" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;
                        <asp:textbox runat="server" ID="txtNombreCliente_80" Width="397px" 
                            meta:resourcekey="txtNombreCliente_80Resource1" />
                    </td>
                </tr>
            </table>
         </div>

        <div id="divTicketCombustion" style="position:absolute; top: 673px; left: 926px; width: 278px; height: 167px;">
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblTGasesCombustionAP1" 
                            CssClass="labelFormulario" Text="Tª Gases Combustión AP1:" 
                            meta:resourcekey="lblTGasesCombustionAP1Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtTGasesCombustionAP1_30" Width="44px" 
                            meta:resourcekey="txtTGasesCombustionAP1_30Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCOCorregidoAP1" CssClass="labelFormulario" 
                            Text="CO Corregido AP1:" meta:resourcekey="lblCOCorregidoAP1Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtCOCorregidoAP1_31" Width="44px" 
                            meta:resourcekey="txtCOCorregidoAP1_31Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCOAmbienteAP1" CssClass="labelFormulario" 
                            Text="CO Ambiente AP1:" meta:resourcekey="lblCOAmbienteAP1Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtCOAmbienteAP1_32" Width="44px" 
                            meta:resourcekey="txtCOAmbienteAP1_32Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblLambdaAP1" CssClass="labelFormulario" 
                            Text="Lambda AP1:" meta:resourcekey="lblLambdaAP1Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtLambdaAP1_33" Width="44px" 
                            meta:resourcekey="txtLambdaAP1_33Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblTiroAP1" CssClass="labelFormulario" 
                            Text="Tiro AP1:" meta:resourcekey="lblTiroAP1Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtTiroAP1_34" Width="44px" 
                            meta:resourcekey="txtTiroAP1_34Resource1"></asp:TextBox>
                   
                        <asp:Label runat="server" ID="lblCO2AP1" CssClass="labelFormulario" 
                            Text="CO2 AP1:" meta:resourcekey="lblCO2AP1Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtCO2AP1_35" Width="44px" 
                            meta:resourcekey="txtCO2AP1_35Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblO2AP1" CssClass="labelFormulario" 
                            Text="O2 AP1:" meta:resourcekey="lblO2AP1Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtO2AP1_36" Width="44px" 
                            meta:resourcekey="txtO2AP1_36Resource1"></asp:TextBox>
                    
                        <asp:Label runat="server" ID="lblQaAP1" CssClass="labelFormulario" 
                            Text="Qa AP1:" meta:resourcekey="lblQaAP1Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtQaAp1_37" Width="44px" 
                            meta:resourcekey="txtQaAp1_37Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblRendimientoAP1" CssClass="labelFormulario" 
                            Text="Rendimiento AP1:" meta:resourcekey="lblRendimientoAP1Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtrendimeintoAP1_38" Width="44px" 
                            meta:resourcekey="txtrendimeintoAP1_38Resource1"></asp:TextBox><br />
                    </td>
                </tr>
                 <tr>
                    <td>
                    <br />
                        <asp:Label runat="server" ID="lblTGasesCombustionAP2" 
                            CssClass="labelFormulario" Text="Tª Gases Combustión AP2:" 
                            meta:resourcekey="lblTGasesCombustionAP2Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtTGasesCombustionAP2_39" Width="44px" 
                            meta:resourcekey="txtTGasesCombustionAP2_39Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCOCorregidoAP2" CssClass="labelFormulario" 
                            Text="CO Corregido AP2:" meta:resourcekey="lblCOCorregidoAP2Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtCOCorregidoAP2_40" Width="44px" 
                            meta:resourcekey="txtCOCorregidoAP2_40Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCOAmbienteAP2" CssClass="labelFormulario" 
                            Text="CO Ambiente AP2:" meta:resourcekey="lblCOAmbienteAP2Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtCOAmbienteAP2_41" Width="44px" 
                            meta:resourcekey="txtCOAmbienteAP2_41Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblLambdaAP2" CssClass="labelFormulario" 
                            Text="Lambda AP2:" meta:resourcekey="lblLambdaAP2Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtLambdaAP2_42" Width="44px" 
                            meta:resourcekey="txtLambdaAP2_42Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblTiroAP2" CssClass="labelFormulario" 
                            Text="Tiro AP2:" meta:resourcekey="lblTiroAP2Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtTiroAP2_43" Width="44px" 
                            meta:resourcekey="txtTiroAP2_43Resource1"></asp:TextBox>
                    
                        <asp:Label runat="server" ID="lblCO2AP2" CssClass="labelFormulario" 
                            Text="CO2 AP2:" meta:resourcekey="lblCO2AP2Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtCO2AP2_44" Width="44px" 
                            meta:resourcekey="txtCO2AP2_44Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblO2AP2" CssClass="labelFormulario" 
                            Text="O2 AP2:" meta:resourcekey="lblO2AP2Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtO2AP2_45" Width="44px" 
                            meta:resourcekey="txtO2AP2_45Resource1"></asp:TextBox>
                    
                        <asp:Label runat="server" ID="lblQaAP2" CssClass="labelFormulario" 
                            Text="Qa AP2:" meta:resourcekey="lblQaAP2Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtQaAP2_46" Width="44px" 
                            meta:resourcekey="txtQaAP2_46Resource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblRendimientoAP2" CssClass="labelFormulario" 
                            Text="Rendimiento AP2:" meta:resourcekey="lblRendimientoAP2Resource1"></asp:Label>
                        <asp:TextBox runat="server" ID="txtrendimeintoAP2_47" Width="44px" 
                            meta:resourcekey="txtrendimeintoAP2_47Resource1"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divNumeroSolicitudAveria" style="position:absolute; top: 1101px; left: 571px; width: 298px; height: 51px;">
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblSolicitudAveria" ForeColor="#5E823C" 
                            text="Nº Solicitud Averia:"  CssClass="labelFormularioValor" 
                            meta:resourcekey="lblSolicitudAveriaResource1"></asp:Label>
                        &nbsp;<asp:TextBox runat="server" ID="txtSolicitudAveria_68" Width="109px" 
                            meta:resourcekey="txtSolicitudAveria_68Resource1"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divBotones" 
            style="position:absolute; top: 1754px; left: 1032px; width: 142px; height: 35px;">
            <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" 
                CssClass="botonFormulario" onclick="btnAceptar_Click" 
                meta:resourcekey="btnAceptarResource1" />
            <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" 
                CssClass="botonFormulario" onclick="btnCancelar_Click" 
                meta:resourcekey="btnCancelarResource1" />
        </div>

        <div id="divCopyright" style="position:absolute; top: 1701px; left: 86px; width: 1080px; height: 35px;"></div>
     </form>
</body>
</html>
