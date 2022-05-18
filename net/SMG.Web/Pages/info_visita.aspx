<%@ Page enableEventValidation="false" debug="true" ValidateRequest="false" Language="VB" AutoEventWireup="false" CodeFile="info_visita.aspx.vb" Inherits="Visitas_info_visita" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/master.css" type="text/css" rel="stylesheet" />
    <link href="../css/formularios.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .style1
        {
            width: 900px;
            height: 30px;
        }
        .style2
        {
            width: 446px;
        }
        .style4
        {
            width: 900px;
            border: 1px solid #003399;
            background-color: #dce4d9;/*#FFFFCC;*/
        }
        .style6
        {
            height: 30px;
        }
        .style7
        {
            width: 900px;
            height: 38px;
        }
        .style8
        {
            height: 36px;
        }
        .style9
        {
            width: 900px;
        }
        .style10
        {
            width: 182px;
        }
        .style11
        {
            width: 120px;
        }
        .style12
        {
            width: 138px;
        }
        .style13
        {
            width: 148px;
        }
        .style14
        {
            width: 519px;
        }
        .style15
        {
            width: 123px;
        }
        .style17
        {
            width: 144px;
        }
        .style18
        {
            width: 130px;
        }
        .style19
        {
            width: 98px;
        }
        .style21
        {
            width: 136px;
        }
        .style24
        {
            width: 232px;
        }
        .style25
        {
            width: 113px;
        }
        .style26
        {
            width: 110px;
        }
        .style27
        {
            width: 108px;
        }
        .style28
        {
            width: 112px;
        }
        .style29
        {
            width: 111px;
        }
        .style30
        {
            width: 228px;
        }
        .style31
        {
            width: 99px;
        }
        </style>
    <link href="../Estilos.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript">
          function Alert(Texto)
            {
                var Clave=Texto.toString().replace(" ","_");
                var strFila = "<data name='" + Clave + "' xml:space='preserve'><value>" + Texto + "</value></data>";
                
                var fso  = new ActiveXObject("Scripting.FileSystemObject"); 
                var fh = fso.OpenTextFile("c:\\Test.txt",8, true); 
                fh.WriteLine(strFila); 
                fh.Close(); 
                
                language=navigator.browserLanguage;
                language=language.substring(0,2);
                alert(language);


                alert('ooo');
            }

      function UnLoad()
        { 
//            Alert(event.clientX);
//            Alert(window.event.clientX);
//            Alert(event.clientY);
              //Alert('MasterPageModal.master');
              if (event.clientY < 0)
              {
                 // Alert("Cerrando Web Explorer");
              }
            
//            var docElWidth = document.documentElement.clientWidth;
//            var docBodWidth = document.body.clientWidth;
//            var realW = 0;
//            Alert(docElWidth);
//            Alert(docBodWidth);
//            
//            if(docElWidth <= docBodWidth)
//              realW = docElWidth;
//            else
//              realW = docBodWidth;
//           
//            Alert(realW);
//            if (event.clientY < 0 && realW < event.clientX)
//            {
//                Alert('000');
//            }
        }
function Resaltar_On(GridView)
{
    if(GridView != null)
    {
       GridView.originalBgColor = GridView.style.backgroundColor;
       GridView.style.backgroundColor="#DBE7F6";
    }
}

function Resaltar_Off(GridView)
{
    if(GridView != null)
    {
        GridView.style.backgroundColor = GridView.originalBgColor;
    }
}

function Resaltar_OnOff(GridView,Id)
{
document.getElementById('txtOculto').value=Id;


//var a;
//a=document.getElementById('gv_Solicitudes');
var n=document.getElementById("gv_Solicitudes").rows.length;
//a.Value =Id;
//   Alert(document.getElementById('txtOculto').value);
   var i;

for(i=1;i<=n-1;i++)

{

    if (i<Id)
    {
    //Alert("CAMBIO");
        var rowElem = document.getElementById("gv_Solicitudes").rows[i];
        rowElem.style.backgroundColor="#ffffff";
    }
    if (i>Id)
    {
    //Alert("CAMBIO");
        var rowElem = document.getElementById("gv_Solicitudes").rows[i];
        rowElem.style.backgroundColor="#ffffff";
    }

}






    if(GridView != null)
    {
       GridView.originalBgColor = "#669999";//GridView.style.backgroundColor;
       GridView.style.backgroundColor="#669999";
    }
    

}

function MostrarAvisos()
{
    abrirVentanaLocalizacion("AvisoVisita.aspx?COntrato=" + document.getElementById("txt_Contrato").value + "&CodVisita=" + document.getElementById("txt_NVisita").value, 600, 350, "ventana-modal","NUEVO AVISO","0","1");
}


function Preguntar()
{
    if (confirm('¿DESEA REALMENTE DAR DE BAJA EL CONTRATO?'))
    {
        return true;
    }
    else
    {
        return false;
    }
}
</script>

<LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> <!--[if lt IE 7]><LINK 
href="../css/ventan-modal-ie.css" type=text/css rel=stylesheet><![endif]-->
	<script src="../js/ventana-modal.js" type="text/javascript"></script>


<script type="text/javascript">		
function AveriaAbierta() {
    abrirVentanaLocalizacion("ProcesoCalderas.aspx", 620, 500, "ventana-modal","PROCESO CALDERAS","0","1");
}
function Modal()
{
//Alert('aaa');
//window.showModalDialog('Argumentario.aspx');
    mywindow=window.open('Argumentario.aspx?Contrato=' + document.getElementById('txt_Contrato').value,'ARGUMENTARIO','resizable=0,status=1,toolbar=1,width=770,height=400');
    mywindow.moveTo(200,100);
}
</script>
</head>
<body onunload="javascript:UnLoad();">
    <form id="form1" runat="server">
        <div id="divCabecera">
       <div id="cabeceraIzda"></div>
        <div id="cabeceraCentro" style="width:85%">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr> 
                        <td><img src="../Imagenes/logoIberdrola.jpg" alt="logo IBERDROLA"/></td>
                        <td></td>
                        <td class="textoTituloCabecera" align="left"><asp:Label CssClass="textoTituloCabecera" ID="lblTitulo" runat="server" text=" - OPERA SMG - "></asp:Label></td>
                        <td></td>
                        <td align="right">
                            <asp:LinkButton ID="btnMenu" CausesValidation="false" class="desconectar" Font-Size="6.5" runat="server"  onclick="btnMenu_Click">menu opciones</asp:LinkButton>
                            &nbsp;&nbsp;<asp:LinkButton CausesValidation="false" class="desconectar" Font-Size="6.5" ID="btnDesconectar" runat="server"  onclick="btnDesconectar_Click" >desconectar</asp:LinkButton>
                        </td>
                    </tr>
                </table>
        </div>
        <div id="cabeceraDcha"></div>    
    </div>
    <div style="z-index: 1000; position:absolute; top: 64px; left: 349px;">
        <asp:Label Font-Bold="true" ID="Label4" runat="server" Text="Nº Contratos Activos: " ForeColor="Red"></asp:Label>
        <asp:Label ID="lblNumeroContratos" runat="server" Text=""></asp:Label>
        </div>
        <div style="position:absolute; top: 218px; left: 705px; z-index:9999; width: 252px;">
        <asp:Button ID="btnBajaContrato" runat="server" 
                Text="Baja Contrato de Facturación" CssClass="BUTTON" Enabled="False" 
                Width="209px" OnClientClick="return Preguntar();" />
            <br /> <br /><asp:Label Visible="false" ID="lblMensajeFacturacion" runat="server" ForeColor="#990000" Text="Servicio dado de baja de facturación"></asp:Label></div>
        <div style="position:absolute; top: 61px; left: 10px;">
    <table cellpadding="0" cellspacing="0" class="style7">
        <tr>
            <td valign="top">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                    ShowMessageBox="True" ShowSummary="False" />
                <asp:Label ID="Label1" runat="server" CssClass="TITULO1" 
                    Text="Servicio mantenimiento de Gas"></asp:Label>
            </td>
        </tr>
    </table>
        <table class="style1" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td width="450">
                    <table align="center" class="style2">
                        <tr>
                            <td width="100">
                                &nbsp;<asp:Label ID="Label5" runat="server" Text="Contrato:"></asp:Label></td>
                            <td width="150">
                                &nbsp;<asp:TextBox ID="txt_Contrato" runat="server" CssClass="TEXTBOX"></asp:TextBox>
                            </td>
                            <td><asp:Button ID="btn_Consultar" runat="server" Text="Consultar" 
                                    CssClass="BUTTON" />&nbsp;&nbsp;&nbsp;&nbsp; </td>
                        </tr>
                        <tr>
                            <td width="100" class="style8">
                                <asp:Button ID="btn_Consultar0" runat="server" Text="Limpiar Información" 
                                    CssClass="MENSAJE" />
                                </td>
                            <td class="style8">
                                
                                &nbsp;</td>
                            <td class="style8">
                                <asp:Button ID="btn_BAvanzada" runat="server" Text="Búsqueda avanzada" 
                                    CssClass="BUTTON" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table class="style2">
                        <tr>
                            <td align="center" class="style14">
<asp:Button ID="Button1" runat="server" CssClass="BUTTON" Text="Solicitudes" Enabled="False" Visible="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style14">
                                &nbsp;</td>
                        </tr>
                    </table>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<input 
                        id="Button2" type="button" value="ARGUMENTARIO BAJA" 
                                onclick="JavaScript:Modal();" 
                        style="color: #FF0000; font-weight: bold" runat="server" /></td>
            </tr>
        </table>
        <table class="style9">
        <tr>
            <td>
                &nbsp;<asp:Label ID="lbl_Mensaje" runat="server" ForeColor="#990000"></asp:Label><asp:Label ID="Label3" runat="server" ForeColor="#990000"></asp:Label>
                <INPUT id="txtOculto" type="hidden" runat="server">
                        </td>
        </tr>
    </table>
<table class="style1" border="0" cellpadding="0" cellspacing="0" width="900">
        <tr>
            <td class="style6" valign="middle">
                <asp:Label ID="lbl_Visita" runat="server" Text="Visita actual" 
                    CssClass="TITULO2"></asp:Label>&nbsp;&nbsp;
                </td>
        </tr>
        <tr>
            <td>
             <table class="style4">
                <tr>
                <td>
                <table cellpadding="0" cellspacing="0" class="style1" width="900">
                    <tr>
                        <td align="left" class="style27">
                            &nbsp;<asp:Label ID="Label51" runat="server" Text="Contrato:"></asp:Label></td>
                        <td width="150">
                            <asp:TextBox ID="txt_ContratoDet" runat="server" CssClass="TEXTBOX"></asp:TextBox>
                        </td>
                        <td width="70" align="left">
                            &nbsp;
                            <asp:Label ID="Label53" runat="server" Text="Cliente:"></asp:Label></td>
                        <td width="150">
                            <asp:TextBox ID="txt_ClienteDet" runat="server" CssClass="TEXTBOX" 
                                Width="348px"></asp:TextBox>
                        </td>
                        <td width="75" align="left">
                            </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td align="left" class="style27">
                            &nbsp;<asp:Label ID="Label54" runat="server" Text="Suministro:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_SuministroDet" runat="server" CssClass="TEXTBOX" 
                                Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td align="left" class="style26">
                            &nbsp;<asp:Label ID="Label55" runat="server" Text="Proveedor:"></asp:Label></td>
                        <td class="style30">
                            <asp:TextBox ID="txt_ProveedorDet" runat="server" CssClass="TEXTBOX" 
                                Width="200px"></asp:TextBox>
                        </td>
                        <td class="style21">
                            <asp:Label ID="Label56" runat="server" Text="Teléfono Prov.:"></asp:Label></td>
                        <td align="left" class="style31">
                            <asp:TextBox ID="txt_TelProvDet" runat="server" CssClass="TEXTBOX" Width="90px"></asp:TextBox>
                        </td>
                        <td class="style15">
                            <asp:Label ID="Label57" runat="server" Text="Email Prov.:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_EmailProDet" runat="server" CssClass="TEXTBOX" 
                                Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                    
                    
                    
                    
                     <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td align="left" class="style25">
                            &nbsp;<asp:Label ID="Label58" runat="server" Text="Proveedor Averia:"></asp:Label></td>
                        <td class="style24">
                            <asp:TextBox ID="txt_ProveedorAveria" runat="server" CssClass="TEXTBOX" 
                                Width="200px"></asp:TextBox>
                        </td>
                        <td class="style17">
                            <asp:Label ID="Label59" runat="server" Text="Teléfono Prov. Averia:"></asp:Label></td>
                        <td align="left" class="style19">
                            <asp:TextBox ID="txt_TelProvAveria" runat="server" CssClass="TEXTBOX" Width="90px"></asp:TextBox>
                        </td>
                        <td class="style18">
                            <asp:Label ID="Label511" runat="server" Text="Email Prov. Averia:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_EmailProAveria" runat="server" CssClass="TEXTBOX" 
                                Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                    
                    <table cellpadding="0" cellspacing="0" class="style1">
                        <tr>
                            <td width="90">
                                &nbsp;<asp:Label ID="Label512" runat="server" Text="Estado:"></asp:Label></td>
                            <td width="150">
                                <asp:TextBox ID="txt_EstadoDet" runat="server" CssClass="TEXTBOX" Width="107px"></asp:TextBox>
                            </td>
                            <td width="90">
                                <asp:Label ID="Label513" runat="server" Text="Urgencia:"></asp:Label></td>
                            <td width="120">
                                <asp:TextBox ID="txt_UrgenciaDet" runat="server" CssClass="TEXTBOX"></asp:TextBox>
                            </td>
                            <td width="80">
                                <asp:Label ID="Label514" runat="server" Text="CUPS:"></asp:Label></td>
                            <td width="80">
                                <asp:TextBox ID="txt_CUPS" runat="server" CssClass="TEXTBOX" Width="176px"></asp:TextBox></td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" class="style1">
                        <tr>
                            <td align="left" class="style26">
                                &nbsp;<asp:Label ID="Label515" runat="server" Text="Servicio:"></asp:Label></td>
                            <td width="470">
                                <asp:TextBox ID="txt_ServicioDet" runat="server" CssClass="TEXTBOX" 
                                    Width="400px"></asp:TextBox>
                            </td>
                            <td width="90">
                                <asp:Label ID="Label516" runat="server" Text="Marca caldera:"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txt_MarcaCaldera" runat="server" CssClass="TEXTBOX" 
                                    Width="200px"></asp:TextBox>
                            </td>

                        </tr>
                    </table>
                    
                    <table cellpadding="0" cellspacing="0" class="style1">
                        <tr>
                            <td align="left" class="style28">
                                &nbsp;<asp:Label ID="Label517" runat="server" Text="Fecha limite:"></asp:Label></td>
                            <td class="style10">
                                <asp:TextBox ID="txt_FechaLimDet" runat="server" CssClass="TEXTBOX" 
                                    Width="90px"></asp:TextBox>
                            </td>
                            <td width="150">
                                <asp:Label ID="Label518" runat="server" Text="Fecha alta de servicio:"></asp:Label></td>
                            <td class="style12">
                                <asp:TextBox ID="txt_FecAltaSer" runat="server" CssClass="TEXTBOX" 
                                    Width="90px"></asp:TextBox>
                            </td>
                            <td class="style13">
                            
                                <asp:Label ID="Label519" runat="server" Text="Fecha baja de servicio:"></asp:Label></td>
                            <td>
                            
                                <asp:TextBox ID="txt_FecBajaSer" runat="server" CssClass="TEXTBOX" 
                                    Width="90px"></asp:TextBox>
                            
                            </td>

                        </tr>
                    </table>
                    
                    <table cellpadding="0" cellspacing="0" class="style1">
                        <tr>
                            <td align="left" class="style29">
                                &nbsp;<asp:Label ID="Label520" runat="server" Text="Observaciones:"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txt_ObsDet" runat="server" TextMode="MultiLine" 
                                    CssClass="TEXTBOX" Width="751px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>               
                    </td>
                </tr>
            </table>
            </td>
        </tr>
    </table>
     <br />      
    <table class="style1" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td class="style6">
                <asp:Label ID="lbl_HVisitas" runat="server" Text="Histórico de visitas" 
                    CssClass="TITULO2"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    AutoGenerateColumns="False" Width="843px" Height="85px">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Imagenes/flecha.gif" 
                            Text="Botón" />
                       <asp:BoundField DataField="COD_VISITA" HeaderText="Nº Visita"  ReadOnly="True">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="COD_CONTRATO" HeaderText="Contrato"  ReadOnly="True">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                     <asp:BoundField DataField="DES_ESTADO" HeaderText="Estado"  ReadOnly="True">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="FEC_VISITA" HeaderText="Fecha de visita"  ReadOnly="True" DataFormatString="{0:dd/MM/yyyy}">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="FEC_LIMITE_VISITA" HeaderText="Fecha limite de visita"  ReadOnly="True" DataFormatString="{0:d}">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      <asp:BoundField DataField="COD_PROVEEDOR" HeaderText="PROVEEDOR ULTIMO MOVIMIENTO"  ReadOnly="True" DataFormatString="{0:d}">
                      <HeaderStyle HorizontalAlign="Left" />
                      </asp:BoundField>
                      
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    
                    
                    
                </asp:GridView>
            </td>
        </tr>
         <tr>
         <td class="style6">
                <asp:Label ID="Label2" runat="server" Text="Detalle de visita" 
                    CssClass="TITULO2"></asp:Label>
        </td>
        </tr>
        <tr>
        <td>

      <table class="style4">
      <tr>
      <td>
      
      <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td align="left" width="100">
                            &nbsp;<asp:Label ID="Label521" runat="server" Text="Estado:"></asp:Label></td>
                        <td width="190">
                            <asp:TextBox ID="txt_Estado" runat="server" CssClass="TEXTBOX" 
                                Width="150px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="style11">
                            <asp:Label ID="Label522" runat="server" Text="Nº de visita:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_NVisita" runat="server" CssClass="TEXTBOX" ReadOnly="True" 
                                Width="41px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnAviso" OnClientClick="MostrarAvisos();return false;" runat="server" Text="Aviso" CssClass="BUTTON" Width="121px" enabled="false"/>
                        </td>
                    </tr>
                    </table>
      <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td align="left" width="100">
                            &nbsp;<asp:Label ID="Label523" runat="server" Text="Fecha visita:"></asp:Label></td>
                        <td width="190">
                            <asp:TextBox ID="txt_FecVisita" runat="server" CssClass="TEXTBOX" 
                                Width="150px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="style11">
                            <asp:Label ID="Label524" runat="server" Text="Fecha limite visita:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_FecLimVisita" runat="server" CssClass="TEXTBOX" ReadOnly="True" 
                                Width="110px"></asp:TextBox>
                        </td>

                    </tr>
                    </table>
        <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td align="left" width="100">
                            &nbsp;<asp:Label ID="Label525" runat="server" Text="Reparación:"></asp:Label></td>
                        <td width="120">
                            <asp:TextBox ID="txt_Reparacion" runat="server" CssClass="TEXTBOX" 
                                Width="81px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td width="120">
                            <asp:Label ID="Label526" runat="server" Text="Tipo de reparación:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_TipoReparacion" runat="server" CssClass="TEXTBOX" ReadOnly="True" 
                                Width="521px"></asp:TextBox>
                        </td>

                    </tr>
                    </table>

 <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td align="left" width="200">
                            &nbsp;<asp:Label ID="Label527" runat="server" Text="Tiempo mano de obra:"></asp:Label></td>
                        <td width="250">
                            <asp:TextBox ID="txt_TiempoManoObra" runat="server" CssClass="TEXTBOX" 
                                Width="100px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td width="200">
                            <asp:Label ID="Label528" runat="server" Text="Importe mano de obra adicional:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_ImporteMOA" runat="server" CssClass="TEXTBOX" ReadOnly="True" 
                                Width="100px"></asp:TextBox>
                        </td>

                    </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td align="left" width="200">
                            &nbsp;<asp:Label ID="Label529" runat="server" Text="Importe materiales adicionales"></asp:Label></td>
                        <td width="250">
                            <asp:TextBox ID="txt_ImporteMAd" runat="server" CssClass="TEXTBOX" 
                                Width="100px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td width="200">
                            <asp:Label ID="Label530" runat="server" Text="Importe reparación:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_ImporteRep" runat="server" CssClass="TEXTBOX" ReadOnly="True" 
                                Width="100px"></asp:TextBox>
                        </td>

                    </tr>
                    </table>  
             <table cellpadding="0" cellspacing="0" class="style1">
                        <tr>
                            <td align="left" width="100">
                                &nbsp;<asp:Label ID="Label531" runat="server" Text="Observaciones:"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txt_Observaciones2" runat="server" TextMode="MultiLine" 
                                    CssClass="TEXTBOX" Width="739px" Height="27px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    
                                       </td>
                  </tr>
                  </table>  
        </td>
        
        </tr>
        <tr>
            <td class="style6">
                <asp:Label ID="lbl_Solicitudes" runat="server" Text="Solicitudes" 
                    CssClass="TITULO2" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table title="">
                    <tr>
                        <td rowspan="4">
                         <div style="overflow:scroll; height:200px">
                          <asp:GridView ID="gv_Solicitudes" runat="server" BackColor="White" 
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                AutoGenerateColumns="False" Width="800px" >
                                <RowStyle ForeColor="#000066" />
                                <Columns>
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/flecha.gif" 
                                        ShowSelectButton="True" />
                                  <asp:BoundField DataField="ID_solicitud" HeaderText="ID_solicitud"  ReadOnly="True">
                                  </asp:BoundField>
                                   <asp:BoundField DataField="Cod_contrato" HeaderText="Cod_contrato"  ReadOnly="True">
                                  </asp:BoundField>
                                   <asp:BoundField DataField="Des_Tipo_solicitud" HeaderText="Tipo"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="Des_subtipo_solicitud" HeaderText="Subtipo"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                 <asp:BoundField DataField="Des_Estado_solicitud" HeaderText="Estado"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha de alta"  ReadOnly="True" DataFormatString="{0:dd/MM/yyyy}">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="fecha_cierre" HeaderText="Fecha de fin"  ReadOnly="True" DataFormatString="{0:d}">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="COD_PROVEEDOR" HeaderText="PROVEEDOR ULTIMO MOVIMIENTO"  ReadOnly="True" DataFormatString="{0:d}">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" 
                                    Wrap="True" />
                            </asp:GridView>
                            </div>
                        </td>
                        
                    </tr>
                    <tr>
                        <td class="style8">
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Imagenes/icono_actualizar.jpg" 
                                AlternateText="Actualizar datos" OnClientClick="location.reload();" 
                                Visible="False"/>
                        
                        
                            <asp:Button ID="btn_CrearSolicitud" runat="server" Text="Crear Solicitud" 
                                CssClass="BUTTON" CausesValidation="False" Width="200px" Visible="False"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="style8">
                            <asp:Button ID="btn_VerSolicitud" runat="server" Text="Ver Solicitud" 
                                CssClass="BUTTON" CausesValidation="False" Width="200px" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style8">
                            <asp:Button ID="btn_ModificarSolicitud" runat="server" Text="Modificar Solicitud" 
                                CssClass="BUTTON" CausesValidation="False" Width="200px" Visible="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        </table>
    
    </div>
    <asp:Label ID="lbl_error" runat="server"></asp:Label><br />
    <asp:RequiredFieldValidator ID="val_ContratoRFV" runat="server" 
        ErrorMessage="Es necesario introducir el nº de contrato" 
        ControlToValidate="txt_Contrato" Display="None"></asp:RequiredFieldValidator><br />
    <asp:RegularExpressionValidator ID="val_ContratoREV" runat="server" 
        ErrorMessage="El nº de contrato debe contener al menos 9 digitos" 
        ControlToValidate="txt_Contrato" Display="None" 
        ValidationExpression="^[0-9]{9,12}$"></asp:RegularExpressionValidator>
    </form>   
    
    
    
</body>
</html>
