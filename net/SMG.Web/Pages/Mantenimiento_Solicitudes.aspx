<%@ Page Language="VB" ValidateRequest="false" debug="true" AutoEventWireup="false" CodeFile="Mantenimiento_Solicitudes.aspx.vb" Inherits="Pages_Mantenimiento_Solicitudes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Opera SMG</title>
    <link href="../css/master.css" type="text/css" rel="stylesheet" />
    <link href="../css/formularios.css" type="text/css" rel="stylesheet" />
        <style type="text/css">
        .style1
        {
            width: 900px;
            height: 30px;
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
        .style8
        {
            width: 900px;
            height: 30px;
        }
        .style10
        {
            width: 101px;
        }
        .style11
        {
            width: 135px;
        }
        .style12
        {
            width: 94px;
        }
        .style13
        {
            width: 280px;
        }
        .style14
        {
            width: 137px;
        }
        .style15
        {
            width: 138px;
        }
        .style16
        {
            width: 130px;
        }
        .style17
        {
            width: 213px;
        }
        .style18
        {
            width: 286px;
        }
        .style20
        {
            width: 129px;
        }
        .style22
        {
            width: 445px;
        }
        .style23
        {
            width: 122px;
        }
        .style25
        {
            width: 86px;
        }
        .style26
        {
            width: 92px;
        }
        .style27
        {
            width: 111px;
        }
    </style>
    <link href="../Estilos.css" rel="stylesheet" type="text/css" />
    <LINK href="../Css/ventana-modal.css" type="text/css" rel="stylesheet">
    <script src="../js/ventana-modal.js" type="text/javascript"></script>
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

function MostrarAvisos()
{
    abrirVentanaLocalizacion("AvisoVisita.aspx?COntrato=" + document.getElementById("txt_Contrato").value + "&CodVisita=" + document.getElementById("txt_NVisita").value, 600, 350, "ventana-modal","NUEVO AVISO","0","1");
}

function MostrarAvisos1(solicitud,mail) {
    abrirVentanaLocalizacion("AvisoSolicitudes.aspx?Solicitud=" + solicitud + "&mail=" + mail, 600, 350, "ventana-modal", "NUEVO AVISO", "0", "1");
}


function soloNumeros(evt)
{
// NOTE: Backspace = 8, Enter = 13, '0' = 48, '9' = 57
var key = evt.keyCode ? evt.keyCode : evt.which ;
return (key <= 40 || (key >= 48 && key <= 57)); } 


function Ocultar()
        {
            document.getElementById('divAviso').style.visibility ='hidden';
            return false;
        }
        function Mostrar()
        {
        //alert('qqq');
            document.getElementById('divAviso').style.visibility ='visible';
            return false;
        }
</script>
</head>
<body onunload="javascript:UnLoad();">
    <form id="form1" runat="server">
       <div id="divCabecera">
       <div id="cabeceraIzda"></div>
        <div id="cabeceraCentro">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr> 
                        <td><img src="../Imagenes/logoIberdrola.jpg" alt="logo IBERDROLA"/></td>
                        <td></td>
                        <td class="textoTituloCabecera" align="left">
                            <asp:Label ID="Label3" runat="server" Text=" - OPERA SMG - "></asp:Label></td>
                        <td></td>
                    </tr>
                </table>
        </div>
        <div id="cabeceraDcha"></div>    
    </div>
    
     <div id="divAviso"
        
        style="z-index:99999;position: absolute; top: 663px; left: 52px;
        width: 874px; height: 33px; background-color:Red; border:thin solid Black; visibility:hidden;" >        
        <asp:Label CssClass="labelFormularioValor" ForeColor="White" ID="lblAviso" runat="server"
            Text=""></asp:Label><br />
        <asp:ImageButton ID="ImageButton4" runat="server" 
            ImageUrl="~/Imagenes/cerrar.gif" style="cursor:hand" 
            OnClientClick="Ocultar();return false;"  />
    </div>
    
    
    
        <div style="position:absolute; top: 61px; left: 10px;">
<table class="style1" border="0" cellpadding="0" cellspacing="0" width="900">
        <tr>
            <td class="style6" valign="middle">
                <asp:Label ID="lbl_Titulo" runat="server" Text="Crear/Mantener Solicitudes" CssClass="TITULO2"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
             <table cellpadding="0" cellspacing="0" class="style4" width="900">
                <tr>
                <td>
                <table class="style1">
                    <tr>
                        <td align="left" class="style10">
                            &nbsp;<asp:Label ID="Label333" runat="server" Text="Contrato:"></asp:Label></td>
                        <td width="150">
                            <asp:TextBox ID="txt_Contrato" runat="server" CssClass="TEXTBOX" Width="172px" Enabled="false"></asp:TextBox>
                        </td>
                        <td width="70" align="left">
                            &nbsp;
                            <asp:Label ID="Label334" runat="server" Text="Cliente:"></asp:Label></td>
                        <td width="150">
                            <asp:TextBox ID="txt_Cliente" runat="server" CssClass="TEXTBOX" 
                                Width="300px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style1">
                    <tr>
                        <td align="left" class="style11">
                            &nbsp;<asp:Label ID="Label335" runat="server" Text="Dirección Suministro:"></asp:Label></td>
                        <td class="style22">
                            <asp:TextBox ID="txt_Direccion" runat="server" CssClass="TEXTBOX" 
                                Width="400px" Enabled="false"></asp:TextBox>
                        </td>
                        <td align="left" class="style12">
                            &nbsp;</td>
                        <td>
                        <asp:DropDownList ID="ddl_Caldera" runat="server" CssClass="TEXTBOX" 
                                Visible="False"  ></asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table  class="style1">
                    <tr>
                        <td align="left" width="117">
                            &nbsp;<asp:Label ID="Label336" runat="server" Text="Proveedor:"></asp:Label></td>
                        <td width="110">
                            <asp:TextBox ID="txt_Proveedor" runat="server" CssClass="TEXTBOX" 
                                Width="196px" Enabled="false"></asp:TextBox>
                                 <input type="hidden" id="hidden_proveedor" runat="server" />
                        </td>
                        <td width="135">
                            <asp:Label ID="Label337" runat="server" Text="Teléfono Prov.:"></asp:Label></td>
                        <td width="100">
                            <asp:TextBox ID="txt_TelProv" runat="server" CssClass="TEXTBOX" Enabled="false"></asp:TextBox>
                        </td>
                        <td class="style27">
                            <asp:Label ID="Label338" runat="server" Text="Mail Proveedor:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_EmailProv" runat="server" CssClass="TEXTBOX" Width="200px" 
                                Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                
                
                 <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td align="left" width="120">
                            &nbsp;&nbsp;<asp:Label ID="Label339" runat="server" Text="Proveedor Averia:"></asp:Label></td>
                        <td width="110">
                            <asp:TextBox ID="txt_ProveedorAveria" runat="server" CssClass="TEXTBOX" 
                                Width="200px" Enabled="false"></asp:TextBox>
                        </td>
                        <td width="140">
                            &nbsp;<asp:Label ID="Label340" runat="server" Text="Teléfono Prov. Averia:"></asp:Label></td>
                        <td align="left" width="100">
                            <asp:TextBox ID="txt_TelProvAveria" runat="server" CssClass="TEXTBOX" 
                                Width="109px" Enabled="false"></asp:TextBox>
                        </td>
                        <td width="120">
                            &nbsp;<asp:Label ID="Label341" runat="server" Text="Email Prov. Averia:"></asp:Label></td>
                        <td class="style2"">
                            <asp:TextBox ID="txt_EmailProAveria" runat="server" CssClass="TEXTBOX" 
                                Width="200px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                
                
                <table  class="style1">
                    <tr>
                        <td class="style23">
                            &nbsp;<asp:Label ID="Label342" runat="server" Text="Estado:"></asp:Label></td>
                        <td class="style13">
                            <asp:TextBox ID="txt_Estado" runat="server" CssClass="TEXTBOX" Width="240px" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label343" runat="server" Text="Urgencia:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_Urgencia" runat="server" CssClass="TEXTBOX" Width="145px" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label344" runat="server" Text="Fecha limite:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txt_FechaLimite" runat="server" CssClass="TEXTBOX" 
                                Width="130px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                                    <td class="style23">
                                        <asp:Label ID="Label345" runat="server" Text="Marca Caldera:"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddl_MarcaCaldera" runat="server" CssClass="TEXTBOX" 
                                            Height="16px" Width="344px" Enabled="False"></asp:DropDownList>
                                    </td>
                                    <td class="style26">
                                        <asp:Label ID="Label346" runat="server" Text="Modelo Caldera:"></asp:Label></td>
                                    <td class="style25">
                                        <asp:TextBox ID="txt_ModeloCaldera" runat="server" CssClass="TEXTBOX" 
                                            Width="146px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                 </table>
                 <table title="" class="style1">
                    <tr>
                        <td>
                             <table class="style1">
                                <tr>
                                    <td>
                                      <div style="border: thin solid #000000; overflow:scroll; height:200px">
                                         <asp:GridView ID="gv_Solicitudes" runat="server" BackColor="White" 
                                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                                AutoGenerateColumns="False" Width="800px" CellSpacing="1" >
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
                                                  <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha creación"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="telefono_contacto" HeaderText="Teléfono contacto"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="Persona_contacto" HeaderText="Persona contacto"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="Des_averia" HeaderText="Descripción Avería"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                 
                                                  
                                                  <asp:TemplateField HeaderText="Observaciones">
                                                         <ItemTemplate>   
                                                            <asp:Label ID="lb_Observaciones" runat="server" ToolTip='<%# Eval("Observaciones")%>' Text='<%# acorta(Eval("Observaciones")) %>'></asp:Label>  
                                                         </ItemTemplate>
                                                  </asp:TemplateField> 
                                                </Columns>
                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                             </table>
                             <table  class="style1">
                                <tr>
                                    <td align="left" width="100">
                                        &nbsp;<asp:Label ID="Label347" runat="server" Text="Tipo:"></asp:Label></td>
                                    <td class="style17">
                                        <asp:DropDownList ID="ddl_Tipo" runat="server" AutoPostBack=true></asp:DropDownList>
                                    </td>
                                    <td align="left" width="100">
                                        &nbsp;<asp:Label ID="Label348" runat="server" Text="Subtipo:"></asp:Label></td>
                                    <td width="250">
                                        <asp:DropDownList ID="ddl_Subtipo" runat="server" AutoPostBack=true 
                                            style="margin-left: 0px"></asp:DropDownList>
                                   </td>
                                </tr>
                             </table>
                            <table  class="style1">
                                <tr>
                                    <td align="left" class="style20">
                                        &nbsp;<asp:Label ID="Label349" runat="server" Text="Estado:"></asp:Label></td>
                                    <td class="style18">
                                        <asp:DropDownList ID="ddl_Estado" runat="server" AutoPostBack=true></asp:DropDownList>
                                    </td>
                                    <td align="left" class="style11">
                                        &nbsp;<asp:Label ID="Label350" runat="server" Text="Motivo cancelación:"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddl_MotivoCancel" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                                    </td>
                                </tr>
                            </table>
                            <table  class="style1">
                                <tr>
                                    <td align="left" class="style16"  >
                                        &nbsp;<asp:label runat="server" ID="lblDescrAveria" Text="Descripción Avería:"></asp:label>
                                        <asp:label runat="server" ID="lblDescrVisita" Visible="false" Text="Descripción Visita:"></asp:label>
                                        </td>
                                    <td >
                                        <asp:DropDownList ID="ddl_Averia" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        <asp:DropDownList ID="ddl_Visita" runat="server" AutoPostBack="true" Visible="false"></asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                    <asp:CheckBox ID="chkUrgente" name="chkUrgente" runat="server" ForeColor="Red" Font-Size="Large" Text="URGENTE" /></td>
                                    <td align="left" class="style16"  >
                                        &nbsp;<asp:label runat="server" ID="lblProveedor" Visible="false" Text="Proveedor:"></asp:label></td></td>
                                    <td><asp:DropDownList ID="cmbProveedor" runat="server" Visible="false"></asp:DropDownList></td>
                                </tr>
                            </table>
                             <table  class="style1">
                                <tr>
                                    <td class="style14">
                                        &nbsp;<asp:Label ID="Label351" runat="server" Text="Teléfono de contacto:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox onkeypress="return soloNumeros(event);" ID="txt_TelfContacto" runat="server" CssClass="TEXTBOX" MaxLength=9></asp:TextBox>
                                    </td>
                                    <td class="style15">
                                        <asp:Label ID="Label352" runat="server" Text="Persona de contacto:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txt_PersContacto" runat="server" CssClass="TEXTBOX" 
                                            Width="135px"></asp:TextBox>
                                    </td>
                                    
                                    <td class="style15">
                                        <asp:Label ID="Label4" Visible="true" runat="server" Text="Horario de contacto:"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="cmbHorarioContacto" runat="server" Visible="true">
                                            <asp:ListItem Text="MAÑANA" Value="MAÑANA"></asp:ListItem>
                                            <asp:ListItem Text="TARDE" Value="TARDE"></asp:ListItem>
                                            <asp:ListItem Text="NOCHE" Value="NOCHE"></asp:ListItem>
                                            <asp:ListItem Text="CUALQUIERA" Value="CUALQUIERA"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                             <table  class="style1">
                                <tr>
                                    <td width="70">
                                        <asp:Label ID="Label353" runat="server" Text="Observaciones Anteriores:"></asp:Label>
                                        <br />
                                        <br />
                                        <br />

                                        <br><asp:Label ID="Label354" runat="server" Text="NUEVAS Observaciones:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txt_ObservacionesAnteriores" runat="server" CssClass="TEXTBOX" 
                                            Width="541px" Height="81px" ReadOnly="True" TextMode="MultiLine" 
                                            ForeColor="gray"></asp:TextBox>
                                        <br />
                                        <asp:TextBox ID="txt_Observaciones" runat="server" CssClass="TEXTBOX" 
                                            Width="541px" Height="45px" TextMode="MultiLine"></asp:TextBox>
                                        <asp:Button ID="btnLLAMADA" runat="server" Font-Bold="True" 
                                            Height="48px" Text="Nueva llamada CLIENTE" Width="167px" 
                                            CssClass="BUTTON" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style23">
                                        <asp:Label ID="Label355" runat="server" Text="Marca Caldera:"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddl_MarcaCaldera1" runat="server" CssClass="style26" 
                                            Font-Bold="True"></asp:DropDownList>
                                    
                                    
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label356" runat="server" Text="Modelo Caldera:"></asp:Label>
                                    
                                        <asp:TextBox ID="txt_ModeloCaldera1" runat="server" CssClass="TEXTBOX" 
                                            Width="146px" Font-Bold="True"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table  class="style1">
                                <tr>
                                    <td class="style8">
                                        <asp:Button ID="btn_AltaSolicitud" runat="server" Text="Alta Solicitud" 
                                            CssClass="BUTTON" CausesValidation="False" Width="200px" />
                                    </td>
                                    <td class="style8">
                                        <asp:Button ID="btn_ModificarSolicitud" runat="server" Text="Modificar Solicitud" 
                                            CssClass="BUTTON" CausesValidation="False" Width="200px"/>
                                    </td>
                                    <td><asp:Button ID="btnAviso" runat="server" Text="Aviso" CssClass="BUTTON" Width="121px" enabled="false"/></td>
                                     <td class="style8">
                                            <!--<asp:LinkButton ID="lnk_Volver" runat="server" Text="Volver"></asp:LinkButton>-->
                                            &nbsp;<div style="position:absolute; top: 702px; left: 732px;">
                                                <asp:Button ID="btn_VolverBoton" runat="server" Text="Volver" 
                                                    BackColor="Transparent" BorderStyle="None" Font-Bold="True" 
                                                    ForeColor="#000099" /></div>
                                            <asp:ImageButton ID="ImageButton1" runat="server" 
                                                ImageUrl="~/Imagenes/icono_atras.gif" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" Height="33px" 
                                    ImageUrl="~/UI/HTML/Images/warning.png" Width="32px" 
                                    OnClientClick="Mostrar(); return false;" AlternateText="AVISOS" Visible="true" />
                                    </td>
                                </tr>
                            </table>
                            <table  class="style1">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_mensajes" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                  <table  class="style1">
                    <tr>
                        <td class="style6">
                            <asp:Label ID="lbl_HistoricoTitle" runat="server" Text="Histórico de solicitud:" 
                                CssClass="TITULO2"></asp:Label>
                           <asp:GridView ID="gv_HistoricoSolicitudes" runat="server" BackColor="White" 
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                AutoGenerateColumns="False" Width="600px" AllowPaging="True">
                                <RowStyle ForeColor="#000066" />
                                <Columns>
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/flecha.gif" 
                                        ShowSelectButton="True" />
                                  <asp:BoundField DataField="id_movimiento" HeaderText="id_movimiento"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                   <asp:BoundField DataField="tipo_movimiento" HeaderText="tipo_movimiento"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="id_solicitud" HeaderText="Solicitud"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                 <asp:BoundField DataField="usuario" HeaderText="Usuario"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="estado_solicitud" HeaderText="Estado"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                    <asp:BoundField DataField="observaciones" HeaderText="Observaciones"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                            </td>
                            <td style="border: thin double #000000;">
                            <asp:Label ID="Label2" runat="server" Text="Histórico de Llamadas:" 
                                CssClass="TITULO2"></asp:Label>
                           <asp:GridView ID="gv_LlamadasClientes" runat="server" BackColor="White" 
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                AutoGenerateColumns="False" Width="314px" AllowPaging="True">
                                <RowStyle ForeColor="#000066" />
                                <Columns>
                                  <asp:BoundField DataField="id_solicitud" HeaderText="Solicitud"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                 <asp:BoundField DataField="FechaLlamada" HeaderText="Fecha LLamada"  
                                        ReadOnly="True">
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
                        <td>
                            &nbsp;</td>
                    </tr>
                 </table>
                 <table  class="style1">
                    <tr>
                        <td class="style6">
                            <asp:Label ID="Label1" runat="server" Text="Histórico de caracteristicas:" 
                                CssClass="TITULO2"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:GridView ID="gv_historicoCaracteristicas" runat="server" BackColor="White" 
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                AutoGenerateColumns="False" Width="600px">
                                <RowStyle ForeColor="#000066" />
                                <Columns>
                                  <asp:BoundField DataField="id_caracteristica" HeaderText="id_caracteristica"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                   <asp:BoundField DataField="id_movimiento" HeaderText="id_movimiento"  
                                        ReadOnly="True" Visible="False">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="tipo_campo" HeaderText="Tipo campo"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                 <asp:BoundField DataField="valor" HeaderText="Valor"  ReadOnly="True">
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
                 </table>
                </td>
                </tr>
            </table>
          </td>
        </tr>
    </table>
    </div>
    <asp:HiddenField ID="hdnIdSolicitud" runat="server" />
    <asp:HiddenField ID="hdnBajaContrato" runat="server" />
    <asp:HiddenField ID="hdnSubtipo6meses" runat="server" />
    </form>
</body>
</html>
