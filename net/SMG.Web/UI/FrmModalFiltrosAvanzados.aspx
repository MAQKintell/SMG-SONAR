<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true"
    CodeFile="FrmModalFiltrosAvanzados.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalFiltrosAvanzados" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalGenerarVisitas" ContentPlaceHolderID="ContentPlaceHolderContenido"
    runat="server">

    <script src="HTML/Js/ventana-modal.js" type="text/javascript"></script>
    <script src="HTML/Js/calendar.js" type="text/javascript"></script>
    <script src="HTML/Js/calendar-setup.js" type="text/javascript"></script>
    <script src="HTML/Js/utf8calendar-es.js" type="text/javascript"></script>
    <script src="HTML/Js/formularios.js" type="text/javascript"></script>
    
    <script src="HTML/Js/rich_calendar/rich_calendar.js" type="text/javascript"></script>
    <script src="HTML/Js/rich_calendar/rc_lang_es.js" type="text/javascript"></script>
    <script src="HTML/Js/rich_calendar/rich_calendar_utils.js" type="text/javascript"></script>
    <link href="HTML/Js/rich_calendar/rich_calendar.css" type="text/css" rel="stylesheet" />
    
    <link href="HTML/Css/formularios.css" type="text/css" rel="stylesheet" />
     <link href="HTML/Css/master.css" type="text/css" rel="stylesheet" />
    <link href="HTML/Css/tablas.css" type="text/css" rel="stylesheet" />
   
    <link href="HTML/Css/ventana-modal.css" type="text/css" rel="stylesheet" />
    <link href="HTML/Css/calendar-blue.css" type="text/css" rel="stylesheet" />     
    <script>
        function ActualizarControlFoco(nombreControl)
        {
            var hdnControlFoco = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnControlFoco");
            if (hdnControlFoco)
            {
                hdnControlFoco.value = nombreControl;
            }
        }
    </script>
    <script>
        //Kintell 15/04/2010
        function PosicionarProvincia()
        {
            //var charCode = (window.event.which) ? window.event.which : event.keyCode
            //if not a digit or arrow key abort
            //Alert(charCode);
            //Alert(event.keyCode);
            //Alert(window.event);
            // Alert('1');
            var ValorABuscar=document.getElementById("ctl00$ContentPlaceHolderContenido$txtBuscarProvincia").value;
            var Nombres= ctl00_ContentPlaceHolderContenido_chkProvincias.getElementsByTagName("label");
            var Esta=false;
            var cont=document.getElementById("ctl00$ContentPlaceHolderContenido$hdnContadorProvincia").value;

            for(var i=0;i<cont;i++)
            {
                var Nombre=Nombres[i].innerText;
                var a1=Nombre.substring(0,ValorABuscar.length);
                var a2=ValorABuscar;
                
                if ((a1.toUpperCase() == a2.toUpperCase() || a1==a2) && Esta==false)
                {
                    //Posicionar.
                    ctl00_ContentPlaceHolderContenido_pnLista.scrollTop=getXY(Nombres[i])-125;
                    Esta=true;
                    break;
                }
            }
        }
        function PosicionarEstados()
        {
            var ValorABuscar=document.getElementById("ctl00$ContentPlaceHolderContenido$txtEstados").value;
            var Nombres= ctl00_ContentPlaceHolderContenido_cblFiltrosColumna.getElementsByTagName("label");
            var Esta=false;
            var cont=document.getElementById("ctl00_ContentPlaceHolderContenido_hdnContadorEstado").value;
            
            for(var i=0;i<cont;i++)
            {
                var Nombre=Nombres[i].innerText;
                var a1=Nombre.substring(0,ValorABuscar.length);
                var a2=ValorABuscar;
                
                if ((a1.toUpperCase() == a2.toUpperCase() || a1==a2) && Esta==false)
                {
                    //Posicionar.
                    ctl00_ContentPlaceHolderContenido_Panel4.scrollTop=getXY(Nombres[i])-125;
                    Esta=true;
                    break;
                }
            }
        }
        
        function PosicionarPoblacion()
        {
            var ValorABuscar=document.getElementById("ctl00$ContentPlaceHolderContenido$txtBuscarPoblacion").value;
            var Nombres= ctl00_ContentPlaceHolderContenido_chkPoblaciones.getElementsByTagName("label");
            var Esta=false;
            var cont=document.getElementById("ctl00_ContentPlaceHolderContenido_hdnContadorPoblacion").value;

            for(var i=0;i<cont;i++)
            {
                var Nombre=Nombres[i].innerText;
                var a1=Nombre.substring(0,ValorABuscar.length);
                var a2=ValorABuscar;
//a1=encodeURIComponent(a1);

                if ((a1.toUpperCase() == a2.toUpperCase() || a1==a2) && Esta==false)
                {
                    //Posicionar.
                    ctl00_ContentPlaceHolderContenido_Panel3.scrollTop=getXY(Nombres[i])-125;
                    //ctl00_ContentPlaceHolderContenido_Panel4.scrollTop=getXY(Nombres[i])-125;
                    Esta=true;
                    break;
                }
            }
        }
        
        
        function getXY(obj)
        {
          var curleft = 0;
          var curtop = obj.offsetHeight + 5;
          var border;
          if (obj.offsetParent)
          {
            do
            {
              // XXX: If the element is position: relative we have to add borderWidth
              if (getStyle(obj, 'position') == 'relative')
              {
                if (border = _pub.getStyle(obj, 'border-top-width')) curtop += parseInt(border);
                if (border = _pub.getStyle(obj, 'border-left-width')) curleft += parseInt(border);
              }
              curleft += obj.offsetLeft;
              curtop += obj.offsetTop;
            }
            while (obj = obj.offsetParent)
          }
          else if (obj.x)
          {
            curleft += obj.x;
            curtop += obj.y;
          }
          //Alert('x:' + curleft + '_y:' + curtop);
          return curtop;
        }

        function getStyle(obj, styleProp)
        {
          if (obj.currentStyle)
            return obj.currentStyle[styleProp];
          else if (window.getComputedStyle)
            return document.defaultView.getComputedStyle(obj,null).getPropertyValue(styleProp);
        }
    </script>
    <asp:ScriptManager ID="SM" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdnControlFoco" runat="server" Value="ctl00_ContentPlaceHolderContenido_txtContrato"/>
    <input id="hdnContadorProvincia" type="hidden" runat="server" />
    <input id="hdnContadorEstado" type="hidden" runat="server" />
    <input id="hdnContadorPoblacion" type="hidden" runat="server" value="1000" />
    <div id="divPanelInformacionContrato" style="position: absolute; top: 2px; left: 9px;
        width: 150px; height: 183px; right: 745px;">
        <asp:Panel ID="pnInfoContrato" runat="server" 
            GroupingText="Filtros de Contrato" Width="776px" CssClass="labelFormulario" 
            Height="221px" meta:resourcekey="pnInfoContratoResource1">
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
    <div id="divEstadoSolicitud" style="position: absolute; width: 285px; top: 242px; left: 457px; height: 164px;">
        <table cellpadding="0" cellspacing="0" style="width: 289px">
            <tr>
                <td class="labelTabla" style="width: 50px; height: 135px;">
                    <asp:Label CssClass="labelFormulario" ID="Label13" height="100%" runat="server" 
                        Text="Estado: " meta:resourcekey="Label13Resource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 100; height: 80px;">
                    <asp:TextBox ID="txtEstados" runat="server" onKeyUp="PosicionarEstados();" 
                        Width="180px" meta:resourcekey="txtEstadosResource1"></asp:TextBox>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel4" runat="server" Height="117px" ScrollBars="Auto" 
                                Wrap="False" Width="236px" BackColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" meta:resourcekey="Panel4Resource1">
                                    <asp:CheckBoxList ID="cblFiltrosColumna" runat="server" 
                                        TabIndex="5" onFocus="ActualizarControlFoco(this.id);" 
                                        CssClass="campoFormulario" BackColor="White" Height="16px" Width="232px" 
                                        meta:resourcekey="cblFiltrosColumnaResource1">
                                    </asp:CheckBoxList>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="divInformacionContrato" 
        style="position: absolute; width: 1713px; top: 17px;left: 24px; height: 164px;">
        <table cellpadding="0" cellspacing="0" 
            style="width: 717px; margin-right: 54px;">
            <tr>
                <td class="labelTabla" style="width: 90px">
                    <asp:Label CssClass="labelFormulario" ID="lblcontrato" runat="server" 
                        Text="Contrato: " meta:resourcekey="lblcontratoResource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 133px">
                    <asp:TextBox ID="txtContrato" runat="server" MaxLength="10" Width="60px" 
                        TabIndex="1" meta:resourcekey="txtContratoResource1"></asp:TextBox>
                </td>
                <td class="labelTabla" style="width: 110px" >
                    <asp:Label CssClass="labelFormulario" ID="Label1" runat="server" 
                        Text="Código Proveedor:" meta:resourcekey="Label1Resource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 177px">
                    <asp:TextBox ID="txtProveedor" runat="server" Width="50px" MaxLength="6" 
                        TabIndex="2" meta:resourcekey="txtProveedorResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="labelTabla" style="width: 50px">
                    <asp:Label CssClass="labelFormulario" ID="lblNombreTitular" runat="server" 
                        Text="Nombre: " meta:resourcekey="lblNombreTitularResource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 133px">
                    <asp:TextBox ID="txtInfoNombre" runat="server" Width="180px" MaxLength="50" 
                        TabIndex="2" meta:resourcekey="txtInfoNombreResource1"></asp:TextBox>
                </td>
                <td class="labelTabla" style="width: 90px">
                    <asp:Label CssClass="labelFormulario" ID="lblCodigoPostal" runat="server" 
                        Text="Código Postal: " meta:resourcekey="lblCodigoPostalResource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 577px">
                    <asp:TextBox ID="txtCodPostal" runat="server" Width="173px" TabIndex="7" 
                        meta:resourcekey="txtCodPostalResource1"></asp:TextBox>&nbsp;
                    <asp:Label CssClass="labelFormulario" ID="Label5" runat="server" 
                        Text=" Separe los diferentes C.P. por ','" meta:resourcekey="Label5Resource1"></asp:Label>
                    <asp:CustomValidator ID="txtCodPostalCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtCodPostal" OnServerValidate="OnCodPostal_ServerValidate" 
                        ValidateEmptyText="True" 
                        meta:resourcekey="txtCodPostalCustomValidatorResource1"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td class="labelTabla" style="width: 90px">
                    <asp:Label CssClass="labelFormulario" ID="lblApellido1" runat="server" 
                        Text="Apellido 1: " meta:resourcekey="lblApellido1Resource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 133px">
                    <asp:TextBox ID="txtInfoApellido1" runat="server" MaxLength="50" Width="180px" 
                        TabIndex="3" meta:resourcekey="txtInfoApellido1Resource1"></asp:TextBox>
                </td>
                <td class="labelTabla" style="width: 110px">
                    <asp:Label CssClass="labelFormulario" ID="lblApellido2" runat="server" 
                        Text="Apellido 2: " meta:resourcekey="lblApellido2Resource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 177px">
                    <asp:TextBox ID="txtInfoApellido2" runat="server" Width="180px" MaxLength="50" 
                        TabIndex="4" meta:resourcekey="txtInfoApellido2Resource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="labelTabla" style="width: 100px; height: 135px;">
                    <asp:Label CssClass="labelFormulario" ID="lblProvincia" height="100%" 
                        runat="server" Text="Provincia: " meta:resourcekey="lblProvinciaResource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 133px; height: 80px;">
                <asp:TextBox ID="txtBuscarProvincia" runat="server" 
                        onkeyup="PosicionarProvincia();" Width="180px" 
                        meta:resourcekey="txtBuscarProvinciaResource1"></asp:TextBox>
                    <asp:UpdatePanel ID="UPProvincia" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnLista" runat="server" Height="117px" ScrollBars="Auto" 
                                Wrap="False" Width="180px" BackColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" meta:resourcekey="pnListaResource1">
                                    <asp:CheckBoxList ID="chkProvincias" runat="server" 
                                        AutoPostBack="True" OnSelectedIndexChanged="cmbProvincias_SelectedIndexChanged"
                                        TabIndex="5" onFocus="ActualizarControlFoco(this.id);" 
                                        CssClass="campoFormulario" BackColor="White" Height="16px" Width="145px" 
                                        meta:resourcekey="chkProvinciasResource1">
                                    </asp:CheckBoxList>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="labelTabla" style="width: 110px; height: 135px;">
                    <asp:Label CssClass="labelFormulario" height="100%" ID="lblPoblacion" 
                        runat="server" Text="Población: " meta:resourcekey="lblPoblacionResource1"></asp:Label>
                </td>
                <td class="campoTabla" 
                    style="width: 177px; margin-left: 40px; height: 80px;">
                    <asp:TextBox ID="txtBuscarPoblacion" runat="server" 
                        onkeyup="PosicionarPoblacion();" Width="180px" 
                        meta:resourcekey="txtBuscarPoblacionResource1"></asp:TextBox>
                    <asp:UpdatePanel ID="UpdatePanelPoblaciones" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel3" runat="server" Height="117px" ScrollBars="Auto" 
                                Wrap="False" Width="180px" BackColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" meta:resourcekey="Panel3Resource1">
                                    <asp:CheckBoxList ID="chkPoblaciones" runat="server" 
                                        TabIndex="5" onFocus="ActualizarControlFoco(this.id);" 
                                        CssClass="campoFormulario" BackColor="White" Height="16px" Width="145px" 
                                        meta:resourcekey="chkPoblacionesResource1">
                                    </asp:CheckBoxList>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
  
    <div id="div1" 
        
        
        style="position: absolute; top: 227px; left: 9px; width: 441px; height: 136px;">
        <asp:Panel ID="Panel1" runat="server" GroupingText="Filtros de Visita" Width="442px"
            CssClass="labelFormulario" Height="131px" 
            meta:resourcekey="Panel1Resource1">
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            
        </asp:Panel>
    </div>
    <div id="divInformacionVisita" style="position: absolute; width: 426px; top: 241px;
        left: 24px; height: 53px; margin-right: 1px; max-width: 609px;">
        <table cellpadding="0" cellspacing="0" style="width: 424px">
            <tr>
                <td class="labelTabla">
                    <asp:Label CssClass="labelFormulario" ID="lblCampana" runat="server" 
                        Text="Campaña: " meta:resourcekey="lblCampanaResource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 118px">
                    <asp:DropDownList ID="cmbCampania" runat="server" TabIndex="10" 
                        meta:resourcekey="cmbCampaniaResource1">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="labelFormulario">
                    <asp:Label CssClass="labelFormulario" ID="lblFechaVisita" runat="server" 
                        Text="Fecha Límite Desde: " meta:resourcekey="lblFechaVisitaResource1"></asp:Label>
                </td>
                <td style="width: 118px">
                    <asp:TextBox id="txtFechaVisitaDesde" CssClass="campoFormularioFecha" 
                        runat="server" onFocus="ActualizarControlFoco(this.id);" MaxLength="10" 
                        meta:resourcekey="txtFechaVisitaDesdeResource1"></asp:TextBox>
                    <img id="imgFechaVisita" src="./HTML/IMAGES/imagenCalendario.gif"   onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtFechaVisitaDesde');">
                    <asp:CustomValidator ID="txtFechaVisitaDesdeCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtFechaVisitaDesde" 
                        OnServerValidate="OnTxtFechaVisitaDesde_ServerValidate" 
                        ValidateEmptyText="True" 
                        meta:resourcekey="txtFechaVisitaDesdeCustomValidatorResource1"></asp:CustomValidator>
                </td>
                <td class="labelFormulario" style="width: 71px">
                    <asp:Label CssClass="labelFormulario" ID="Label2" runat="server" 
                        Text="Fecha Hasta:" meta:resourcekey="Label2Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaVisitaHasta" CssClass="campoFormularioFecha" 
                        runat="server" onFocus="ActualizarControlFoco(this.id);" MaxLength="10" 
                        meta:resourcekey="txtFechaVisitaHastaResource1"></asp:TextBox>
                    <img id="img1" src="./HTML/IMAGES/imagenCalendario.gif"   onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtFechaVisitaHasta');">
                    <script type="text/javascript">                                                                          
                        
                    </script>
                    <asp:CustomValidator ID="txtFechaVisitaHastaCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtFechaVisitaHasta" 
                        OnServerValidate="OnTxtFechaVisitaHasta_ServerValidate" 
                        ValidateEmptyText="True" 
                        meta:resourcekey="txtFechaVisitaHastaCustomValidatorResource1"></asp:CustomValidator>
                </td>
                
            </tr>
            <tr>
                <td class="labelTabla" style="width: 122px; height: 24px;">
                    <asp:Label CssClass="labelFormulario" ID="lblUrgencia" runat="server" 
                        Text="Urgencia: " meta:resourcekey="lblUrgenciaResource1"></asp:Label>
                </td>
                <td class="campoTabla" style="width: 118px">

                    <div id="div4" 
                        
                        style="position: absolute; width: 90px; top: 45px; left: 121px; height: 77px;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel5" runat="server" Height="78px" ScrollBars="Auto" 
                                    Wrap="False" Width="133px" BackColor="White" BorderStyle="Solid" 
                                    BorderWidth="1px" meta:resourcekey="Panel5Resource1">
                                    <asp:CheckBoxList CssClass="campoFormulario" BackColor="White" 
                                        ID="cmbTiposUrgencia" runat="server" TabIndex="8" 
                                        meta:resourcekey="cmbTiposUrgenciaResource1">
                                        <asp:ListItem meta:resourcekey="ListItemResource1">Urgente</asp:ListItem>
                                        <asp:ListItem meta:resourcekey="ListItemResource2">Muy urgente</asp:ListItem>
                                        <asp:ListItem meta:resourcekey="ListItemResource3">Vencido</asp:ListItem>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td class="labelTabla">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label CssClass="labelFormulario" 
                        ID="lblLote" runat="server" Text="Lote: " meta:resourcekey="lblLoteResource1"></asp:Label>
                </td>
                <td class="campoTabla">
                    <br /><asp:TextBox ID="txtLote" runat="server" Width="109px" MaxLength="18" 
                        TabIndex="9" meta:resourcekey="txtLoteResource1"></asp:TextBox>
                    <asp:CustomValidator ID="txtLoteCustomValidator" CssClass="errorFormulario" 
                        runat="server" ErrorMessage="*" ControlToValidate="txtLote" 
                        OnServerValidate="OnNumeroLote_ServerValidate" ValidateEmptyText="True" 
                        meta:resourcekey="txtLoteCustomValidatorResource1"></asp:CustomValidator>
                </td>
            </tr>
        </table>
    </div>
    <div id="div2" style="position: absolute; width: 437px; top: 363px;
        left: 9px; height: 53px; margin-right: 1px; max-width: 609px;">
        <asp:Panel ID="Panel2" runat="server" GroupingText="Filtros resto de Fechas" Width="442px"
            CssClass="labelFormulario" Height="89px" 
            meta:resourcekey="Panel2Resource1">
            <br />
            <br />
            <br />
        </asp:Panel>
    </div>
    <div id="div3" style="position: absolute; width: 421px; top: 376px;
        left: 24px; height: 50px; margin-right: 1px; max-width: 609px;">
          <table cellpadding="0" cellspacing="0" style="width: 416px">
            <tr>
                <td class="labelFormulario" style="width: 122px">
                    <asp:Label CssClass="labelFormulario" ID="Label6" runat="server" 
                        Text="Fecha Lote Desde: " meta:resourcekey="Label6Resource1"></asp:Label>
                </td>
                <td style="width: 120px">
                    <asp:TextBox id="txtFechaLoteDesde" CssClass="campoFormularioFecha" 
                        runat="server" onFocus="ActualizarControlFoco(this.id);" MaxLength="10" 
                        meta:resourcekey="txtFechaLoteDesdeResource1"></asp:TextBox>
                                       
                    <img id="img2" src="./HTML/IMAGES/imagenCalendario.gif" onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtFechaLoteDesde');">
                    <script type="text/javascript">                                                                          
                        
                    </script>
                    <asp:CustomValidator ID="txtFechaLoteDesdeCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtFechaLoteDesde" 
                        OnServerValidate="OnTxtFechaLoteDesde_ServerValidate" 
                        ValidateEmptyText="True" 
                        meta:resourcekey="txtFechaLoteDesdeCustomValidatorResource1"></asp:CustomValidator>
                </td>
                <td class="labelFormulario" style="width: 71px">
                    <asp:Label CssClass="labelFormulario" ID="Label7" runat="server" 
                        Text="Fecha Hasta: " meta:resourcekey="Label7Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaLoteHasta" CssClass="campoFormularioFecha" 
                        runat="server" onFocus="ActualizarControlFoco(this.id);" MaxLength="10" 
                        meta:resourcekey="txtFechaLoteHastaResource1"></asp:TextBox>
                    <img id="img3" src="./HTML/IMAGES/imagenCalendario.gif"   onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtFechaLoteHasta');">
                    <script type="text/javascript">                                                                          
                        
                    </script>
                    <asp:CustomValidator ID="txtFechaLoteHastaCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtFechaLoteHasta" 
                        OnServerValidate="OnTxtFechaLoteHasta_ServerValidate" 
                        ValidateEmptyText="True" 
                        meta:resourcekey="txtFechaLoteHastaCustomValidatorResource1"></asp:CustomValidator>
                </td>
                
            </tr>
            <tr>
                <td class="labelFormulario" style="width: 122px">
                    <asp:Label CssClass="labelFormulario" ID="Label3" runat="server" 
                        Text="Fecha Servicio Desde: " meta:resourcekey="Label3Resource1"></asp:Label>
                </td>
                <td style="width: 120px">
                    <asp:TextBox id="txtFechaServicioDesde" CssClass="campoFormularioFecha" 
                        runat="server" onFocus="ActualizarControlFoco(this.id);" MaxLength="10" 
                        meta:resourcekey="txtFechaServicioDesdeResource1"></asp:TextBox>
                    <img id="img4" src="./HTML/IMAGES/imagenCalendario.gif"   onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtFechaServicioDesde');">
                    <script type="text/javascript">                                                                          
                        
                    </script>
                    <asp:CustomValidator ID="txtFechaServicioDesdeCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtFechaServicioDesde" 
                        OnServerValidate="OnTxtFechaServicioDesde_ServerValidate" 
                        ValidateEmptyText="True" 
                        meta:resourcekey="txtFechaServicioDesdeCustomValidatorResource1"></asp:CustomValidator>
                </td>
                <td class="labelFormulario" style="width: 71px">
                    <asp:Label CssClass="labelFormulario" ID="Label4" runat="server" 
                        Text="Fecha Hasta: " meta:resourcekey="Label4Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaServicioHasta" CssClass="campoFormularioFecha" 
                        runat="server" onFocus="ActualizarControlFoco(this.id);" MaxLength="10" 
                        meta:resourcekey="txtFechaServicioHastaResource1"></asp:TextBox>
                    <img id="img5" src="./HTML/IMAGES/imagenCalendario.gif"   onclick="ShowCalendar(this, 'ctl00$ContentPlaceHolderContenido$txtFechaServicioHasta');">
                    <script type="text/javascript">                                                                          
                        
                    </script>
                    <asp:CustomValidator ID="txtFechaServicioHastaCustomValidator" 
                        CssClass="errorFormulario" runat="server" ErrorMessage="*" 
                        ControlToValidate="txtFechaServicioHasta" 
                        OnServerValidate="OnTxtFechaServicioHasta_ServerValidate" 
                        ValidateEmptyText="True" 
                        meta:resourcekey="txtFechaServicioHastaCustomValidatorResource1"></asp:CustomValidator>
                </td>
                
            </tr>            
        </table>
    </div>
    <div id="divBotonAceptar" style="position: absolute; width: 300px; top: 412px; left: 537px;
        height: 22px; margin-right: 1px;">
        <asp:Button ID="btnAceptar" runat="server" CssClass="botonFormulario" Text="Aceptar"
            Width="75px" OnClick="OnBtnAceptar_Click" TabIndex="11" 
            meta:resourcekey="btnAceptarResource1" />
        <asp:Button ID="btnLimpiar" runat="server" CssClass="botonFormulario" Text="Limpiar"
            Width="75px" OnClick="OnBtnLimpiar_Click" TabIndex="11" 
            meta:resourcekey="btnLimpiarResource1" />
    </div>
</asp:Content>
