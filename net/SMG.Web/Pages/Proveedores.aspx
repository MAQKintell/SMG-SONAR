<%@ Page EnableEventValidation="true" debug="true" Language="VB" AutoEventWireup="false" CodeFile="Proveedores.aspx.vb" Inherits="Pages_Proveedores" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PROVEEDORES</title>
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
            border: 1px solid #003700;
            background-color: #dce4d9;/*#008000;/*#FFFFCC;/*#003399; #FFFFCC;*/
        }
        
        .style6
        {
            height: 30px;
            background-image:url(../Imagenes/fondo_submenu.gif);
            background-repeat:no-repeat;
	        background-color:#fafafa;
        }
        .style8
        {
            width: 900px;
            height: 30px;
        }
        .style10
        {
            width: 283px;
        }
        .style11
        {
            width: 80px;
        }
        .style14
        {
            width: 9px;
        }
        .style15
        {
            width: 110px;
        }
        .style16
        {
            width: 253px;
        }
        .style17
        {
            width: 115px;
        }
        .style19
        {
            width: 57px;
        }
        .style23
        {
            width: 174px;
        }
        .style25
        {
            width: 238px;
        }
        .style26
        {
            width: 177px;
        }
        .style30
        {
            width: 165px;
        }
        .style31
        {
            height: 30px;
            width: 534px;
        }
        .style32
        {
            width: 534px;
        }
        .style34
        {
            width: 815px;
        }
        .style35
        {
            width: 222px;
        }
        .style36
        {
            width: 52px;
        }
        .style37
        {
            width: 64px;
        }
        .style38
        {
        	font-family: Verdana;
            font-size: x-small;
            margin-left: 0px;
	        height: 26px;
	        width: 106px;
        }
        .style39
        {
            width: 266px;
        }
    </style>
    <link href="../Estilos.css" rel="stylesheet" type="text/css" />
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> <!--[if lt IE 7]><LINK 
href="../css/ventan-modal-ie.css" type=text/css rel=stylesheet><![endif]-->
	<script src="../js/ventana-modal.js" type="text/javascript"></script>
		
    <script type="text/javascript" >
    
    
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
      function ClickPaginacion(ident)
        {
            var botonPaginacion = document.getElementById("btnPaginacion");
            //var hiddenPaginacion = document.getElementById("ctl00$ContentPlaceHolderContenido$hdnPageIndex");
            document.getElementById("hiddenPaginacion").value = ident;
            //Alert(document.getElementById("hiddenPaginacion"));
            botonPaginacion.click();
            return false;
        }
      
      function CargarDatos(Valor)
      {
        
        //abrirVentanaEspera("frmEspera.aspx", 620, 400, "ventana-modal","","1","1");
        abrirVentanaAlerta(620, 400, "ventana-modal","CARGANDO DATOS...","1","1");
        window.form1.style.visibility ="hidden";
        javascript:__doPostBack('gv_Solicitudes',Valor);

      }  
      
      function Modificar()
      {
        //abrirVentanaLocalizacion("frmEspera.aspx", 620, 400, "ventana-modal","","1","1");
        abrirVentanaAlerta(620, 400, "ventana-modal","CARGANDO DATOS...","1","1");
        window.form1.style.visibility ="hidden";
        //abrirVentanaLocalizacion("frmEspera.aspx", 620, 400, "ventana-modal","","1","1");
//        var botonModificar = document.getElementById("btn_ModificarSolicitud");
//        botonModificar.click();
      }
        
        
        function Resaltar_On(GridView)
        {
            if(GridView != null)
            {
               GridView.originalBgColor = GridView.style.backgroundColor;
               GridView.style.backgroundColor="#DBE7F6";
               //Alert(GridView.ForeColor);
               //GridView.ForeColor="RED";
               
               GridView.ToolTip='<%#Eval("Observaciones")%>';
            }
        }

        function Resaltar_Off(GridView)
        {
            if(GridView != null)
            {
                GridView.style.backgroundColor = GridView.originalBgColor;
            }
        }

        function Button1_onclick() {
            abrirVentanaLocalizacion("ProcesoCalderas.aspx", 620, 400, "ventana-modal","PROCESO CALDERAS","0","1");
        }

        function Button2_onclick() {
            abrirVentanaLocalizacion("Info_visita_SinMenu.aspx?contrato=" + document.getElementById("txtContrato").value , 900, 700, "ventana-modal","DATOS CONTRATO","0","1");
        }



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
<body onunload="javascript:UnLoad();" style="background-color:white;">
    <form id="form1" runat="server">
    <%--<div id="div1" style="position:absolute;top:157px;left:918px;">
        <asp:LinkButton ID="LinkButton1" runat="server" class="desconectar" Font-Size="7" onclick="btnDesconectar_Click" >desconectar</asp:LinkButton>
   </div>--%>
    <asp:HiddenField ID="hiddenPaginacion" runat="server" />
    <div id="divCabecera" style="left: 8px;">
       <div id="cabeceraIzda"></div>
        <div id="cabeceraCentro" style="width:85%">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr> 
                        <td><img src="../Imagenes/logoIberdrola.jpg" alt="logo IBERDROLA"/></td>
                        <td></td>
                        <td class="textoTituloCabecera" align="left"><asp:Label CssClass="textoTituloCabecera" ID="lblTitulo" runat="server" text=" - OPERA SMG - "></asp:Label></td>
                        <td></td>
                        <td align="right">
                            <asp:LinkButton ID="btnMenu" class="desconectar" Font-Size="6.5" CausesValidation="false" runat="server"  onclick="btnMenu_Click">menu opciones</asp:LinkButton>
                            &nbsp;&nbsp;<asp:LinkButton class="desconectar" Font-Size="6.5" CausesValidation="false" ID="btnDesconectar" runat="server"  onclick="btnDesconectar_Click" >desconectar</asp:LinkButton>
                        </td>
                    </tr>
                </table>
        </div>
        <div id="cabeceraDcha"></div>    
    </div>
    <div style="border: 1px none #000000; position:absolute; top: 557px; left: 296px; width: 694px; z-index:9999">
            <asp:label Font-Bold="true" ID="leyenda" runat="server" Text="Leyenda Colores (última modificación por parte del...):"></asp:label>
        <asp:Button ID="Button1" runat="server" Text="TELEFONO" BackColor="GreenYellow" 
                BorderStyle="Double" Font-Bold="True" ForeColor="Blue" 
                Height="23px" style="cursor:default" />
            &nbsp;
              
        <asp:Button ID="Button2" runat="server" Text="PROVEEDOR" BackColor="RoyalBlue" 
                BorderStyle="Double" Font-Bold="True" ForeColor="Blue" 
                Height="23px" style="cursor:default" />
            &nbsp;

        <asp:Button ID="Button3" runat="server" Text="ADMINISTRADOR" BackColor="Red" 
                BorderStyle="Double" Font-Bold="True" ForeColor="Blue" Height="23px" 
                Width="129px" style="cursor:default" />
        
        
    </div>
    
    <div id="divAviso"
        
        style="z-index:99999;position: absolute; top: 663px; left: 52px;
        width: 874px; height: 33px; background-color:Red; border:thin solid Black; visibility:hidden;" >        
        <asp:Label CssClass="labelFormularioValor" ForeColor="White" ID="lblAviso" runat="server"
            Text=""></asp:Label><br />
        <asp:ImageButton ID="ImageButton1" runat="server" 
            ImageUrl="~/Imagenes/cerrar.gif" style="cursor:hand" 
            OnClientClick="Ocultar();return false;"  />
    </div>
    
    <div style="position:absolute; top: 51px; left: 20px;">
    <table class="style1" border="0" cellpadding="0" cellspacing="0" width="900">
        <tr>
            <td class="style6" valign="middle">
                <asp:Label ID="lbl_Titulo" runat="server" Text="Relación de solicitudes" CssClass="TITULO2"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
             <table cellpadding="0" cellspacing="0" class="style4" width="900">
                <tr>
                    <td class="style31" valign="middle">
                        <asp:Label ID="lblTitulo2" runat="server" Text="Filtros:" CssClass="TITULO2"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                <td class="style32">
                <table class="style1">
                    <tr>
                        <td align="left" class="style14">
                            &nbsp;<asp:Label ID="Label31" runat="server" Text="Solicitud:"></asp:Label></td>
                        <td class="style15">
                            <asp:TextBox ID="txt_solicitud" runat="server" CssClass="TEXTBOX" Width="79px"></asp:TextBox>
                        </td>
                        
                        <td align="left" class="style14">
                            &nbsp;<asp:Label ID="Label32" runat="server" Text="Proveedor:"></asp:Label></td>
                        <td class="style15">
                            <asp:TextBox ID="txt_Proveedor" runat="server" CssClass="TEXTBOX" Width="79px" 
                                Enabled=false></asp:TextBox>
                            <input type="hidden" id="hidden_proveedor" runat="server" />
                        </td>
                        <td align="left" class="style30">
                            &nbsp;<asp:Label ID="Label33" runat="server" Text="Código contrato:"></asp:Label></td>
                        <td class="style30" align="left">
                            <asp:TextBox ID="txt_contrato" runat="server" CssClass="TEXTBOX" Width="143px"></asp:TextBox>
                        </td>
                        <td align="left" class="style16" >
                            <asp:Label ID="Label34" runat="server" Text="Consulta pendientes:"></asp:Label> <asp:CheckBox ID="chk_Pendientes" runat="server" />
                            </td>
                        <td align="left" class="style16" >
                            <asp:Label ID="Label35" runat="server" Text="Mostrar Urgentes"></asp:Label> <asp:CheckBox ID="chk_Urgente" runat="server" />
                            </td>
                    </tr>
                </table>
                <table class="style1">
                    <tr>
                        <td align="left" class="style37">
                            &nbsp;<asp:Label ID="Label36" runat="server" Text="Tipo:"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddl_Tipo" runat="server" Class="style38" 
                                AutoPostBack=true Height="24px" Width="148px"></asp:DropDownList>
                        </td>
                        <td align="left" class="style36">
                            &nbsp;<asp:Label ID="Label37" runat="server" Text="Subtipo:"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddl_Subtipo" runat="server" Class="style38"  
                                AutoPostBack=true Height="24px" Width="198px"></asp:DropDownList>
                        </td>
                        <td align="left" width="100">
                            &nbsp;<asp:Label ID="Label38" runat="server" Text="Estado:"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddl_Estado" runat="server" Class="style38" AutoPostBack="true" Height="24px" Width="198px"></asp:DropDownList>
                        </td>
                        <td align="left" width="100">
                            &nbsp;<asp:Label ID="Label3" runat="server" Text="Provincia:"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddl_Provincia" runat="server" CssClass="TEXTBOX" AutoPostBack="true"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table class="style1">
                    <tr>
                        <td align="right" width="100">
                            &nbsp;<asp:Label ID="Label39" runat="server" Text="Fecha Creación:"></asp:Label></td>
                        
                        <td align="left" class="">
                            &nbsp;<asp:Label ID="Label311" runat="server" Text="Desde:"></asp:Label></td>
                        <td class="style35">
                            <asp:TextBox ID="txtDesde" runat="server" Width="102px" AutoPostBack="True"></asp:TextBox>
&nbsp;
							<asp:imagebutton id="btnCalFechaLimite" runat="server" 
                                ImageUrl="~/Imagenes/transparent.gif" AlternateText="Seleccione la fecha en el calendario"
								CausesValidation="False" style="height: 16px" Height="1px" Width="1px"></asp:imagebutton>
							<asp:imagebutton id="btnCalFechaLimite0" runat="server" 
                                ImageUrl="../Imagenes/calendar.gif" AlternateText="Seleccione la fecha en el calendario"
								CausesValidation="False" style="height: 16px; width: 16px;"></asp:imagebutton>
							<asp:imagebutton id="btnCalFechaLimite1" runat="server" 
                                ImageUrl="~/Imagenes/cerrar.gif" AlternateText="Borrar la fecha seleccionada"
								CausesValidation="False" style="height: 16px; "></asp:imagebutton>
                            <asp:Calendar ID="cal_Desde" runat="server" BackColor="White" 
                                BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" 
                                DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" 
                                ForeColor="#003399" Height="139px" Visible="False" Width="213px">
                                <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                <WeekendDayStyle BackColor="#CCCCFF" />
                                <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                                <OtherMonthDayStyle ForeColor="#999999" />
                                <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                                <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                                <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" 
                                    Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                            </asp:Calendar>
                        </td>
                        <td align="left" class="style19">
                            &nbsp;<asp:Label ID="Label312" runat="server" Text="Hasta:"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtHasta" runat="server" Width="104px"></asp:TextBox>
&nbsp;
							<asp:imagebutton id="btnCalFechaCierre" runat="server" 
                                ImageUrl="../Imagenes/calendar.gif" AlternateText="Seleccione la fecha en el calendario"
								CausesValidation="False"></asp:imagebutton>
							<asp:imagebutton id="btnCalFechaLimite2" runat="server" 
                                ImageUrl="~/Imagenes/cerrar.gif" AlternateText="Borrar la fecha seleccionada"
								CausesValidation="False" style="height: 16px; "></asp:imagebutton>
                            <asp:Calendar ID="cal_Hasta" runat="server" BackColor="White" 
                                BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" 
                                DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" 
                                ForeColor="#003399" Height="119px" Visible="False" Width="227px">
                                <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                <WeekendDayStyle BackColor="#CCCCFF" />
                                <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                                <OtherMonthDayStyle ForeColor="#999999" />
                                <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                                <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                                <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" 
                                    Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                            </asp:Calendar>
                        <div style="position:absolute; top: 141px; left: 844px;">
                            <asp:Button ID="btnBuscar" runat="server" Font-Bold="True" Text="Buscar" /></div>
                        <div style="position:absolute; top: 141px; left: 645px;z-index: 1;">
                            <asp:FileUpload ID="FileUpload1" runat="server" /> </div>
                        <div style="position:absolute; top: 166px; left: 754px;z-index: 1;">
                            <asp:Button ID="btnCargarSolictudesTelefono" runat="server" Font-Bold="True" 
                                Text="Cargar Solicitudes" ForeColor="Red" BackColor="white" />
                            </div>
                        
                    </tr>
                    <tr><td colspan="4">
                        <div style="width: 361px; top: 297px; left: 4px;">
                        <asp:Label ID="lblBuscar" ForeColor="Red" runat="server" Text="Seleecione los filtros deseados y pulse el botón Buscar"></asp:Label>
                        <asp:Label Visible=false ID="lblTotal" ForeColor="Red" runat="server" Text="Nº de Registros: "></asp:Label>
                        &nbsp;<asp:Label ID="lblTotal0" runat="server" ForeColor="Red" Visible=false></asp:Label>
                        <br />
                        <asp:Label ID="lblPaginas" runat="server" ForeColor="Red" Text="Nº de Páginas: " Visible=false></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblPaginaActual" Visible=false ForeColor="Red" runat="server" Text="Página Actual: "></asp:Label>
                        
                        <div>
                        </div>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <div>
                        </div></td></tr>
                </table>
                <div style="position:absolute; top: 124px; left: 786px; width: 96px; height: 59px; width: 57px">
                    <asp:ImageButton ID="ImageButton3" runat="server" 
            ImageUrl="~/Imagenes/flecha_Iz.gif" ToolTip="Anterior" BorderColor="Black" 
                        BorderStyle="None" BorderWidth="1px" Visible="False" />
        &nbsp;
        <asp:ImageButton ID="ImageButton2" runat="server" Height="50px" 
            ImageUrl="~/Imagenes/flecha_Der.gif" style="height: 40px" Width="18px" 
            ToolTip="Siguiente" BorderColor="Black" BorderStyle="None" BorderWidth="1px" 
                        Visible="False" />
                     </div>
    
                        <div style="position:absolute; top: 126px; left: 371px; width: 366px;">
                            <asp:Button ID="btnPaginacion" runat="server" Text="" OnClick="OnBtnPaginacion_Click" Width="0"/> 
                            <asp:Panel ID="pnlPaginas" runat="server">
                            &nbsp;</asp:Panel>
                        </div>
                <table title="" class="style1">
                    <tr>
                        <td>
                             <table class="style1">
                                <tr>
                                    <td>
                                        <div style="border: thin solid #000000; overflow:scroll; height:300px">
                                           <asp:GridView ID="gv_Solicitudes" runat="server" BackColor="White" 
                                                BorderColor="#336699" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                                                AutoGenerateColumns="False" Width="800px" CellSpacing="1" 
                                                Font-Bold="False">
                                                <RowStyle ForeColor="#000066" />
                                                <Columns>
                                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/flecha.gif" 
                                                        ShowSelectButton="True" />
                                                  <asp:BoundField DataField="ID_solicitud" HeaderText="COD. Solicitud"  
                                                        ReadOnly="True">
                                                   <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="Cod_contrato" HeaderText="Cod_contrato"  ReadOnly="True">
                                                   <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="tipo_solicitud" HeaderText="Tipo"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="subtipo_solicitud" HeaderText="subTipo"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="estado_solicitud" HeaderText="estado_solicitud"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="Cod_contrato" HeaderText="Código contrato"  ReadOnly="True">
                                                   <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                   <asp:BoundField DataField="Des_Tipo_solicitud" HeaderText="Tipo"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  
                                                  
                                                  <asp:BoundField DataField="subtipo_solicitud" HeaderText="Subtipo"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="Estado_solicitud" HeaderText="Estado"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  
                                                  
                                                  <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha creación"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="telefono_contacto" HeaderText="Teléfono contacto"  
                                                        ReadOnly="True" Visible="False">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="Persona_contacto" HeaderText="Persona contacto"  
                                                        ReadOnly="True" Visible="False">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="Des_averia" HeaderText="Descripción avería"  ReadOnly="True">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="NOM_PROVINCIA" HeaderText="Provincia" />
                                                   
                                                  
                                                  
                                                  
                                                  <asp:TemplateField HeaderText="Observaciones">
                                                         <ItemTemplate>   
                                                            <asp:Label ID="lb_Observaciones" runat="server" ToolTip='<%# Eval("Observaciones")%>' Text='<%# acorta(Eval("Observaciones")) %>'></asp:Label>  
                                                         </ItemTemplate>
                                                  </asp:TemplateField> 
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                    <asp:CheckBoxField DataField="Urgente" HeaderText="URGENTE" />
                                                    <asp:BoundField DataField="Proveedor" HeaderText="PROVEEDOR" />
                                                  
                                                    <asp:BoundField DataField="Fec_visita" HeaderText="Fecha Visita Realizada" ReadOnly="True">
                                                      <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    
                                                    <%--<asp:TemplateField HeaderText="CUPS">
                                                         <ItemTemplate>   
                                                            <asp:Label ID="lb_CUPS" runat="server" ToolTip='<%# Eval("CUPS")%>' Text='<%# acorta(Eval("CUPS")) %>'></asp:Label>  
                                                         </ItemTemplate>
                                                  </asp:TemplateField>--%> 
                                                  
                                                  
                                                  
                                                  
                                                  <%--<asp:BoundField DataField="id_perfil" HeaderText="Perfil" />--%>
                                                </Columns>
                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="WHITE" />
                                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                             </table>
                             <table  class="style1">
                                <tr>
                                    <td>
                                            <asp:Button ID="btn_exportar" runat="server" Text="Exportar excel" 
                                            CssClass="BUTTON" CausesValidation="False" Width="100px" Visible="False"/>
                                            <asp:Button ID="btn_exportar0" runat="server" Text="Exportar excel" 
                                            CssClass="BUTTON" CausesValidation="False" Width="100px"/>
                                    &nbsp;        
                                            <input runat="server" id="btnCaldera0" type="button" class="BUTTON"  
                                            value="DATOS CONTRATO" onclick="return Button2_onclick()" 
                                            visible="true" />
                                    </td>
                                    
                                </tr>
                            </table>
                            <div style="background-color:WhiteSmoke">
                             <table>
                                <tr>
                                    <td class="style39">
                                        <input runat="server" id="btnCaldera" type="button" class="BUTTON"  
                                            value="VISUALIZAR DATOS CALDERA" onclick="return Button1_onclick()" 
                                            visible="False" />
                                    </td>
                                    <td colspan="5" class="style34">
                                       <asp:LinkButton ID="lnk_Excel" runat="server"></asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkUrgente" name="chkUrgente" runat="server" 
                                            ForeColor="Red" Font-Size="Large" Text="URGENTE" Visible="False" />
                                            
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label313" runat="server" Text="COD. SOLICITUD:"></asp:Label>&nbsp;&nbsp; <asp:TextBox ID="txt_IdSolicitud" runat="server" CssClass="TITULO1" 
                                            Width="112px" Enabled="false" BackColor="Transparent" BorderStyle="None" 
                                            BorderWidth="0px" Font-Bold="True" ForeColor="Red"></asp:TextBox>
                                            <asp:Label ID="Label314" runat="server" Text="CONTRATO:"></asp:Label>&nbsp;&nbsp;<asp:TextBox ID="txtContrato" runat="server" CssClass="TITULO1" 
                                            Width="112px" Enabled="false" BackColor="Transparent" BorderStyle="None" 
                                            BorderWidth="0px" Font-Bold="True" ForeColor="Red"></asp:TextBox>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td colspan="6" >
                                        &nbsp;<asp:Label ID="Label315" runat="server" Text="Suministro:"></asp:Label>&nbsp; <asp:TextBox ID="txtNombre" CssClass="TEXTBOX" 
                                            Width="218px" Enabled="false" Font-Bold="True" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txt_Suministro" runat="server" CssClass="TEXTBOX" 
                                            Width="365px" Enabled="false" Font-Bold="True"></asp:TextBox>
                                    </td>
                                   
                                </tr>
                             </table>
                             <table>
                                <tr>
                                    
                                    <td >
                                                                                &nbsp;&nbsp;<asp:Label ID="Label316" runat="server" Text="Prioridad:"></asp:Label>                                <asp:TextBox ID="txt_Urgencia" runat="server" CssClass="TEXTBOX" Width="146px" 
                                            Enabled="false" Font-Bold="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label317" runat="server" Text="Fecha limite:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txt_FechaLimite" runat="server" CssClass="TEXTBOX" 
                                            Width="130px" Enabled="false" Font-Bold="True"></asp:TextBox>
                                            
                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label318" runat="server" Text="Persona de contacto:"></asp:Label> &nbsp;&nbsp;<asp:TextBox ID="txt_persContacto" runat="server" CssClass="TEXTBOX" 
                                            Width="135px" Enabled="false" Font-Bold="True"></asp:TextBox>
                                    </td>
                                </tr>
                             </table>
                             <table>
                                <tr>
                                    <td width="70">
                                        &nbsp;<asp:Label ID="Label319" runat="server" Text="Servicio:"></asp:Label></td>
                                    <td class="style10">
                                        <asp:TextBox ID="txt_Servicio" runat="server" CssClass="TEXTBOX" Width="455px" 
                                            Enabled="false" Height="16px" Font-Bold="True"></asp:TextBox>
                                    </td>
                                    <td class="style11">
                                        Teléfono de
                                        <br />
                                        contacto:</td>
                                    <td>
                                        <asp:TextBox ID="txt_telfContacto" runat="server" CssClass="TEXTBOX" 
                                            MaxLength=9 Enabled="false" Font-Bold="True"></asp:TextBox>
                                    </td>
                                    <td>
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
                            <table>
                                <tr>
                                    <td width="70">
                                        <asp:Label ID="Label320" runat="server" Text="Observaciones Anteriores:"></asp:Label>
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br><asp:Label ID="Label321" runat="server" Text="NUEVAS Observaciones:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txt_ObservacionesAteriores" runat="server" CssClass="TEXTBOX" 
                                            Width="685px" Height="122px" ReadOnly="True" TextMode="MultiLine" 
                                            ForeColor="gray" Font-Bold="True" MaxLength="5000"></asp:TextBox>
                                        <br />
                                        <asp:TextBox ID="txt_Observaciones" runat="server" CssClass="TEXTBOX" 
                                            Width="685px" Height="43px" TextMode="MultiLine"></asp:TextBox>
                                            <asp:ImageButton ID="ImageButton4" runat="server" Height="33px" 
                                    ImageUrl="~/UI/HTML/Images/warning.png" Width="32px" 
                                    OnClientClick="Mostrar(); return false;" AlternateText="AVISOS" Visible="true" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td class="style23">
                                        &nbsp;<asp:Label ID="Label322" runat="server" Text="Estado Actual:"></asp:Label></td>
                                    <td class="style10">
                                        <asp:TextBox ID="txt_Estado" runat="server" CssClass="TEXTBOX" Width="240px" 
                                            Enabled="false" Font-Bold="True"></asp:TextBox>
                                    </td>
                                    <td align="left" class="style26">
                                        &nbsp;<asp:Label ID="Label323" runat="server" Text="Nuevo Estado:"></asp:Label></td>
                                    <td class="style25">
                                        <asp:DropDownList ID="ddl_EstadoSol" runat="server" AutoPostBack=true 
                                            Font-Bold="True" Height="38px" Width="301px" Font-Size="Small"></asp:DropDownList>
                                    </td>
                                    <td align="left" class="style17">
                                        &nbsp;</td>
                                    <td width="250">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style23">
                                        <asp:Label ID="Label324" runat="server" Text="Marca Caldera:"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddl_MarcaCaldera" Enabled="false" runat="server" CssClass="style26" 
                                            Font-Bold="True"></asp:DropDownList>
                                    </td>
                                    <td class="style26">
                                        <asp:Label ID="Label325" runat="server" Text="Modelo Caldera:"></asp:Label></td>
                                    <td class="style25">
                                        <asp:TextBox ID="txt_ModeloCaldera" runat="server" CssClass="TEXTBOX" 
                                            Width="293px" Font-Bold="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr><td><asp:Label ID="Label326" runat="server" Text="Motivo Cancelación:"></asp:Label></td><td>
                                                            <asp:DropDownList ID="ddl_MotivoCancel" 
                                        runat="server" 
                                            Font-Bold="True" Height="46px" Width="301px" Font-Size="Small"></asp:DropDownList></td></tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="left" width="100">
                                            &nbsp;<asp:Label ID="Label327" runat="server" Text="Características:"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:GridView ID="gv_Caracteristicas" runat="server" BackColor="White" 
                                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                            AutoGenerateColumns="False" Width="600px">
                                            <RowStyle ForeColor="#000066" />
                                            <Columns>
                                                <asp:CommandField EditImageUrl="~/Imagenes/flecha.gif" 
                                                    ShowEditButton="True" ButtonType="Image" 
                                                    CancelImageUrl="~/Imagenes/transparent.gif" 
                                                    UpdateImageUrl="~/Imagenes/icono_actualizar.jpg" 
                                                    ShowCancelButton="False" />
                                            <asp:BoundField DataField="ID_caracteristica" HeaderText="ID_caracteristica"  
                                                    ReadOnly="True" Visible="False">
                                              </asp:BoundField>
                                              <asp:BoundField DataField="tip_car"  ReadOnly="True">
                                                  <FooterStyle Height="0px" Width="0px" />
                                                  <HeaderStyle Height="0px" Width="1px" />
                                                  <ItemStyle ForeColor="White" Height="0px" Width="1px" Wrap="True" />
                                              </asp:BoundField>
                                               <asp:BoundField DataField="Descripcion" HeaderText="Descripción"  ReadOnly="True">
                                              <HeaderStyle HorizontalAlign="Left" />
                                              </asp:BoundField>
                                              <asp:TemplateField HeaderText="Valor">
                                              <ItemTemplate>
                                                <asp:Label ID="lbl_Valor" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.valor") %>' ></asp:Label>
                                              </ItemTemplate>
                                                
                                                

                                                <EditItemTemplate>
                                                <%--<%#Prueba(DataBinder.Eval(Container.DataItem, "Descripcion"))%>--%>

                                                    <asp:TextBox ID="txt_ValorCaracteristica" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.valor") %>'></asp:TextBox>
                                                
                                                </EditItemTemplate>
                                               
                                                
                                                
                                              </asp:TemplateField>
                                              
                                              

                                            </Columns>
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle Wrap="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                             </table>
                              <table>
                                <tr>
                                    <td class="style8">
                                        <asp:Button ID="btn_ModificarSolicitud" runat="server" Text="Modificar" 
                                            CssClass="BUTTON" CausesValidation="False" Width="200px" OnClientClick="javascript:Modificar();return true;"/>
                                        <asp:HyperLink ID="PosicionModificar" runat="server"></asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td class="style8">
                                       <asp:Label ID="lbl_mensajes" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            </div>
                        </td>
                    </tr>
                </table>
                  <table  class="style1">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Historico SOLICITUD"></asp:Label>
                           <asp:GridView ID="gv_HistoricoSolicitudes" runat="server" BackColor="White" 
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                AutoGenerateColumns="False" Width="600px">
                                <RowStyle ForeColor="#000066" />
                                <Columns>
                                  <asp:BoundField DataField="id_movimiento" HeaderText="id_movimiento"  
                                        ReadOnly="True" Visible="False">
                                      <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                   <asp:BoundField DataField="tipo_movimiento" HeaderText="tipo_movimiento"  
                                        ReadOnly="True" Visible="False">
                                       <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="id_solicitud" HeaderText="Solicitud"  
                                        ReadOnly="True" Visible="False">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/flecha.gif" 
                                        ShowSelectButton="True" />
                                  <asp:BoundField DataField="estado_solicitud" HeaderText="Estado"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                 <asp:BoundField DataField="usuario" HeaderText="Usuario"  ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                    <asp:BoundField DataField="observaciones" HeaderText="Observaciones"  
                                        ReadOnly="True">
                                  <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                    <asp:BoundField DataField="time_stamp" HeaderText="Fecha / Hora" />
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                        </td>
                        <td>
                        </td>
                    </tr>
                 </table>
                </td>
                <tr><td class="style32">
                    
                <asp:Label ID="Label2" runat="server" Text="Historico CARACTERÍSTICAS"></asp:Label>
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
                    
                <br />
                    
                    </td></tr>
                </tr>
            </table>
          </td>
        </tr>
    </table>
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
   
    
    </div>
    </form>
</body>
</html>
