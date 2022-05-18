<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGas.master" AutoEventWireup="true" CodeFile="FrmContratos.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmContratos" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="Iberdrola.Commons.Web" %>
        
<asp:Content ID="FrmContratos" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <LINK href="../css/ventana-modal.css" type="text/css" rel="stylesheet"> 
    <LINK href="../css/ventan-modal-ie.css" type="text/css" rel="stylesheet">
	<script src="../js/ventana-modal.js" type="text/javascript"></script>
        <script type="text/javascript">
        <% 
            if(NavigationController.IsBackward())
            { 
        %>
                MostrarCapaEspera();
        <%
            }
        %>
        function ExportarExcel() {
            //alert('qqq');
            //alert('II');
//            var btnAccion = document.getElementById("ctl00_ContentPlaceHolderContenido_btnExcel");
//           
//            btnAccion.click();
            
            abrirVentanaLocalizacion("../UI/AbrirExcel.aspx", 600, 350, "ventana-modal","<%=Resources.TextosJavaScript.TEXTO_EXPORTAR_EXCEL%>","0","1");
           }
        </script>
        
    <asp:ScriptManager ID="SM" runat= "server"></asp:ScriptManager>
    <asp:UpdatePanel ID= "UPFiltros" runat="server" >
    <ContentTemplate>
        <div id="divFiltros" 
            style="width: 954px; position:absolute; top: 5px; left: 12px; height: 242px;">
        <asp:Panel ID="Panel2" CssClass="labelFormulario" runat="server"  
                GroupingText="Filtros de la información" Width="956px" 
                meta:resourcekey="Panel2Resource1">
            <asp:RadioButtonList ID="radioFiltros" runat="server" OnSelectedIndexChanged="OnSelectedIndexChanged_RadioFiltros"
                CssClass="campoFormulario" Height="121px" AutoPostBack="True" TabIndex="1" 
                meta:resourcekey="radioFiltrosResource1">
                <asp:ListItem Selected="True" Value="1" meta:resourcekey="ListItemResource1">Listado de mantenimientos sin filtrar</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ListItemResource2">Filtrado por fecha de última visita</asp:ListItem>
                <asp:ListItem Value="3" meta:resourcekey="ListItemResource3">Filtrado por lote</asp:ListItem>
                <asp:ListItem Value="4" meta:resourcekey="ListItemResource4">Filtrado de visitas no cerradas por urgencia</asp:ListItem>
                <asp:ListItem Value="5" meta:resourcekey="ListItemResource5">Filtrado de visitas en ejecución o aplazadas sin servicio</asp:ListItem>
                <asp:ListItem Value="6" meta:resourcekey="ListItemResource6">Filtrar por Contrato</asp:ListItem>
            </asp:RadioButtonList>
            <div id= "divBotonFiltrosAvanzados" 
                style="position:absolute; top: 18px; left: 674px; height: 41px; width: 278px;" 
                dir="rtl" align="center">
               
                <input id="btnFiltrosAvanzados" type="button" value="Definir" class="botonFormulario" onclick="OnBtnFiltrosAvanzados_Click()" TabIndex="2"/>
                <asp:CheckBox ID="chkAplicarFiltrosAvanzados" runat="server"  cssClass="campoFormulario"
                    Text="Aplicar filtros avanzados" TextAlign="Left" TabIndex="3" 
                    meta:resourcekey="chkAplicarFiltrosAvanzadosResource1" />
            </div>
        </asp:Panel>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    
        <div id="divControlesFiltros" style="width: 620px; position:absolute; top: 15px; left: 320px; height: 220px;">
        
            <asp:UpdatePanel ID= "UpdatePanel1" runat="server" >
    <ContentTemplate>
        <div id="divControlesOp2" style="position:absolute; top: 31px; left: 3px;">
        <asp:PlaceHolder ID="placeControlesOp2" runat="server" Visible="False">
            <asp:Label ID="lblFechaDesdeOp2" CssClass="labelFormulario" runat="server" 
                Text="Fecha Desde: " meta:resourcekey="lblFechaDesdeOp2Resource1"></asp:Label>
            <asp:TextBox ID="txtFechaDesdeOp2" CssClass="campoFormulario" width="60px" 
                runat="server" MaxLength="10" meta:resourcekey="txtFechaDesdeOp2Resource1"></asp:TextBox>
            <asp:CustomValidator CssClass="errorFormulario" 
                ID="txtFechaDesdeOp2ValidatorCustomValidator" runat="server" ErrorMessage="*" 
                OnServerValidate="TxtFechaDesdeOp2CustomValidate" 
                meta:resourcekey="txtFechaDesdeOp2ValidatorCustomValidatorResource1"></asp:CustomValidator>
            <img id="imgFechaVisita" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" onclick="ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaDesdeOp2');" >
            
            <asp:Label ID="lblFechaHastaOp2" CssClass="labelFormulario" runat="server" 
                Text="  Fecha Hasta: " meta:resourcekey="lblFechaHastaOp2Resource1"></asp:Label>
            <asp:TextBox ID="txtFechaHastaOp2" CssClass="campoFormulario" width="60px" 
                runat="server" MaxLength="10" meta:resourcekey="txtFechaHastaOp2Resource1" ></asp:TextBox>
            <asp:CustomValidator CssClass="errorFormulario" ID="txtFechaHastaOp2Validator" 
                runat="server" ErrorMessage="*" 
                OnServerValidate="TxtFechaHastaOp2CustomValidate" 
                meta:resourcekey="txtFechaHastaOp2ValidatorResource1"></asp:CustomValidator>
            <img id="imgFechaVisita2" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" onclick="ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaHastaOp2');">
            </asp:PlaceHolder>
        </div>
          </ContentTemplate>
    </asp:UpdatePanel>
        
            <asp:UpdatePanel ID= "UpdatePanel2" runat="server" >
    <ContentTemplate>
        <div id="divControlesOp3" style="position:absolute; top: 57px; left: 3px;">
        <asp:PlaceHolder ID="placeControlesOp3" runat="server" Visible="False">
            <asp:Label ID="lblFechaDesdeOp3" CssClass="labelFormulario" runat="server" 
                Text="Fecha Desde: " meta:resourcekey="lblFechaDesdeOp3Resource1"></asp:Label>
            <asp:TextBox ID="txtFechaDesdeOp3" CssClass="campoFormulario" width="60px" 
                runat="server" MaxLength="10" meta:resourcekey="txtFechaDesdeOp3Resource1" ></asp:TextBox>
            <asp:CustomValidator CssClass="errorFormulario" 
                ID="txtFechaDesdeOp3ValidatorCustomValidator" runat="server" ErrorMessage="*" 
                OnServerValidate="TxtFechaDesdeOp3CustomValidate" 
                meta:resourcekey="txtFechaDesdeOp3ValidatorCustomValidatorResource1"></asp:CustomValidator>
            <img id="imgFechaVisita3" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" onclick="ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaDesdeOp3');">

            <asp:Label ID="lblFechaHastaOp3" CssClass="labelFormulario" runat="server" 
                Text="  Fecha Hasta: " meta:resourcekey="lblFechaHastaOp3Resource1"></asp:Label>
            <asp:TextBox ID="txtFechaHastaOp3" CssClass="campoFormulario" width="60px" 
                runat="server" MaxLength="10" meta:resourcekey="txtFechaHastaOp3Resource1" ></asp:TextBox>
            <asp:CustomValidator CssClass="errorFormulario" ID="txtFechaHastaOp3Validator" 
                runat="server" ErrorMessage="*" 
                OnServerValidate="TxtFechaHastaOp3CustomValidate" 
                meta:resourcekey="txtFechaHastaOp3ValidatorResource1"></asp:CustomValidator>
            <img id="imgFechaVisita4" src="./HTML/IMAGES/imagenCalendario.gif" alt="Calendario" onclick="ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_txtFechaHastaOp3');">
            </asp:PlaceHolder>
        </div>
          </ContentTemplate>
    </asp:UpdatePanel>
  
    <asp:UpdatePanel ID= "UpdatePanel3" runat="server" >
        <ContentTemplate>
            <div id="divControlesOp4" style="position:absolute; top: 77px; left: 3px; ">
                <asp:PlaceHolder ID="placeControlesOp4" runat="server" Visible="False"> 
                    <asp:Label ID="lblEstadoVisita" CssClass="labelFormulario" runat="server" 
                        Text="Estado de la visita: " meta:resourcekey="lblEstadoVisitaResource1"></asp:Label>        
                    <asp:DropDownList ID="cmbEstadoVisita" CssClass="campoFormulario" 
                        runat="server" meta:resourcekey="cmbEstadoVisitaResource1"></asp:DropDownList>
                </asp:PlaceHolder>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID= "UpdatePanel4" runat="server" >
        <ContentTemplate>
            <div id="divControlesOp7" style="position:absolute; top: 57px; left: 3px;" >
                <asp:PlaceHolder ID="placeControlesOp7" runat="server" Visible="False"> 
                    <asp:Label ID="lblFakeComboLotes"  CssClass="labelFormulario" runat="server" 
                        Text="Lote:" meta:resourcekey="lblFakeComboLotesResource1"></asp:Label>
                    <asp:TextBox ID="txtIdLote" AutoPostBack="True" CssClass="campoFormulario" 
                        runat="server" MaxLength="18" Width="60px" 
                        OnTextChanged="OnTxtIdLote_TextChanged" meta:resourcekey="txtIdLoteResource1" ></asp:TextBox>&nbsp;
                    <asp:CustomValidator CssClass="errorFormulario" ID="txtIdLoteCustomValidator" 
                        runat="server" ErrorMessage="*" OnServerValidate="TxtIdLoteCustomValidate" 
                        meta:resourcekey="txtIdLoteCustomValidatorResource1"></asp:CustomValidator>
                    <asp:TextBox ID="txtDescripcionLote" AutoPostBack="True" 
                        CssClass="campoFormulario" MaxLength="255"  Width="120px" 
                        OnTextChanged="OnTxtDescripcionLote_TextChanged" runat="server" 
                        meta:resourcekey="txtDescripcionLoteResource1"></asp:TextBox>&nbsp;
                    <asp:Button ID="btnDetalleLote" AutoPostBack="true" CssClass="botonFormulario" 
                        runat="server" Text="..." ToolTip="Buscar Lote" 
                        OnClientClick="OnBtnAbrirFakeComboLotes_Click()" 
                        meta:resourcekey="btnDetalleLoteResource1"/>
                </asp:PlaceHolder>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
 
    <asp:UpdatePanel ID= "UpdatePanel5" runat="server" >
        <ContentTemplate>
            <div id="divControlesOp9" style="position:absolute; top: 128px; left: 3px;">
            <asp:PlaceHolder ID="placeControlesOp9" runat="server" Visible="False"> 
                <asp:Label ID="lblContrato" CssClass="labelFormulario" runat="server" 
                    Text="Código Contrato: " meta:resourcekey="lblContratoResource1"></asp:Label>
                   <asp:TextBox ID="txtCodigoContrato" CssClass="campoFormulario" width="120px" 
                    runat="server" MaxLength="20" meta:resourcekey="txtCodigoContratoResource1" ></asp:TextBox>
                <asp:Label ID="Label1" Visible="False" runat="server" 
                    CssClass="labelFormulario" ForeColor="Red" Font-Bold="True" 
                    Text=" Para PEGAR un contrato pulse a la vez las teclas(Ctrl + V)." 
                    meta:resourcekey="Label1Resource1"></asp:Label>
            </asp:PlaceHolder>
            </div>
              <div ID="divControlesOp8" style="position:absolute; top: 76px; left: 3px;">
                  <asp:PlaceHolder ID="placeControlesOp8" runat="server" Visible="False">
                      <asp:Label ID="lblUrgencia" runat="server" CssClass="labelFormulario" 
                          Text="Caracter de la urgencia: " meta:resourcekey="lblUrgenciaResource1"></asp:Label>
                      <asp:DropDownList ID="cmbTipoUrgencia" runat="server" 
                          CssClass="campoFormulario" meta:resourcekey="cmbTipoUrgenciaResource1">
                      </asp:DropDownList>
                  </asp:PlaceHolder>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        
    </div> 
 
     <div id= "divBotonBuscar"
        style="position:absolute; top: 141px; left: 840px; height: 19px; width: 63px;" 
        align="left">
        <asp:Button ID="btnBuscar" CssClass="botonFormulario" runat="server" 
             Text="Buscar" OnClick="BtnBuscar_OnClick" OnClientClick="MostrarCapaEspera();" 
             TabIndex="4" meta:resourcekey="btnBuscarResource1"/>
        <asp:Button ID="hdnBtnBuscar" runat="server" Text="Buscar" 
            OnClick="BuscarFiltroColumna_OnClick" OnClientClick="MostrarCapaEspera();" 
            TabIndex="4" Height="0px" Width="0px" 
             meta:resourcekey="hdnBtnBuscarResource1"/>
     </div>
    
    <script language="javascript" >
        function DoScroll()
        {
             if (document.getElementById("ctl00_ContentPlaceHolderContenido_DataGridCabecera"))
             {
                document.all('ctl00_ContentPlaceHolderContenido_DataGridCabecera').style.pixelLeft = ctl00_ContentPlaceHolderContenido_divScroll.scrollLeft * -1;
                //alert(parseInt(document.getElementById('MenuContextual').style.top));
                document.getElementById('MenuContextual').style.top=parseInt(document.getElementById('MenuContextual').style.top) -4;
                //alert(parseInt(document.getElementById('MenuContextual').style.top));
             }
        }
        function Menu(){
            document.oncontextmenu = function(){return false}
            //alert(event.clientY);// + document.body.scrollTop);
            //alert(event.clientX);// + document.body.scrollLeft);
            //alert('1');
            var FilaSeleccionada=parseInt(document.getElementById('ctl00_ContentPlaceHolderContenido_HDFilaSeleccionada').value);
            var CeldaSeleccionada=parseInt(document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value);
            //alert(document.getElementById('ctl00_ContentPlaceHolderContenido_HDFilaSeleccionada'));
            var topY=event.clientY-220;// - document.body.scrollTop;
            var topX=event.clientX;// - + document.body.scrollLeft;
            if(CeldaSeleccionada==48)
            {
                topX=topX-200;
            }
            //alert(event.button);
            if (event.button==2){
                //alert(CeldaSeleccionada);
                //alert(document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado')).Rows[FilaSeleccionada + 1].cells[CeldaSeleccionada]);
                document.getElementById('ctl00_ContentPlaceHolderContenido_HDValorSeleccionado').value=document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado').rows[FilaSeleccionada + 1].cells[CeldaSeleccionada].innerText;
                document.getElementById('ctl00_ContentPlaceHolderContenido_lblIgual').innerText="Igual a '" + document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado').rows[FilaSeleccionada + 1].cells[CeldaSeleccionada].innerText + "'";
                document.getElementById('ctl00_ContentPlaceHolderContenido_lblNoIgual').innerText="No es igual a '" + document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado').rows[FilaSeleccionada + 1].cells[CeldaSeleccionada].innerText + "'";
                document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').innerText="En o antes de '" + document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado').rows[FilaSeleccionada + 1].cells[CeldaSeleccionada].innerText + "'";
                document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').innerText="En o despues de '" + document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado').rows[FilaSeleccionada + 1].cells[CeldaSeleccionada].innerText + "'";
                document.getElementById('ctl00_ContentPlaceHolderContenido_lblCopiar').innerText="Copiar '" + document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado').rows[FilaSeleccionada + 1].cells[CeldaSeleccionada].innerText + "'";
                //alert('B');
                
               
                
                
                if (CeldaSeleccionada==1 || CeldaSeleccionada==3 || CeldaSeleccionada==32 || CeldaSeleccionada==33 || CeldaSeleccionada==34 || CeldaSeleccionada==38 || CeldaSeleccionada==46)
                {
                    document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='visible';
                    document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='visible';
                    
                    document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='visible';
                    document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='visible';
                }
                else
                {
                    document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
                    document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
                    
                    document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
                    document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
                }
                
                
                var Titulo=document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado').rows[0].cells[CeldaSeleccionada].innerText;
                if(Titulo.substring(0,5)=='Fecha')
                {
                    document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='visible';
                    document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='visible';
                    
                    document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='visible';
                    document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='visible';
                }
                     
                
                document.getElementById('MenuContextual').style.top = topY + "px";
                document.getElementById('MenuContextual').style.left = topX + "px";
                //document.getElementById('MenuContextual').style.zIndex ="1";
                document.getElementById('MenuContextual').style.visibility ='visible';
                //alert('C');
                return false;
            }
            else
            {
                document.oncontextmenu = function(){return true}
            }
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
        }

        function ObtenerCelda(NumeroCelda)
        {
            //alert(NumeroCelda);
            document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value=NumeroCelda;
        }
        
        function Resaltar_On1(GridView)
        {
            //alert('11');
            if(GridView != null)
            {
                GridView.originalBgColor = GridView.style.backgroundColor;
                GridView.style.backgroundColor="#DBE7F6";
                document.getElementById('ctl00_ContentPlaceHolderContenido_HDFilaSeleccionada').value=GridView.style.value;
    //                    alert(GridView.style.value0);
    //                    alert(GridView.style.value1);
    //                    alert(GridView.style.value2);
    //                    alert(GridView.style.value3);
    //                    alert(GridView.style.value4);
    //                    alert(GridView.style.value5);
    //                    alert(GridView.style.value6);
            }
        }
            
            
    </script>
    
    <script language ='javascript'>
        //Funciones del menu cotextual.
        function Copiar()
        {
            var s=document.getElementById('ctl00_ContentPlaceHolderContenido_HDValorSeleccionado').value;//document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado').rows[FilaSeleccionada].cells[CeldaSeleccionada].innerText;
            
            clipboardData.setData("Text", s);
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
            document.oncontextmenu = function(){return true}
        }
        
        function LlamarAOrdenarAZ()
        {
            document.getElementById('ctl00_ContentPlaceHolderContenido_HDMenuSeleccionado').value='1' + ';' + document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value;
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
            MostrarCapaEspera();
            document.getElementById('ctl00_ContentPlaceHolderContenido_btnMenu').click();
            document.oncontextmenu = function(){return true}
        }
        
        function LlamarAOrdenarZA()
        {
            document.getElementById('ctl00_ContentPlaceHolderContenido_HDMenuSeleccionado').value='2' + ';' + document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value;
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
            MostrarCapaEspera();
            document.getElementById('ctl00_ContentPlaceHolderContenido_btnMenu').click();
            document.oncontextmenu = function(){return true}
        }

        function LlamarAIgualA()
        {
            document.getElementById('ctl00_ContentPlaceHolderContenido_HDMenuSeleccionado').value='3' + ';' + document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value;
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
            MostrarCapaEspera();
            document.getElementById('ctl00_ContentPlaceHolderContenido_btnMenu').click();
            document.oncontextmenu = function(){return true}
        }
        
        function LlamarADiferenteA()
        {
            document.getElementById('ctl00_ContentPlaceHolderContenido_HDMenuSeleccionado').value='4' + ';' + document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value;
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
            MostrarCapaEspera();
            document.getElementById('ctl00_ContentPlaceHolderContenido_btnMenu').click();
            document.oncontextmenu = function(){return true}
        }

        function LlamarAEnAntesDe()
        {
            document.getElementById('ctl00_ContentPlaceHolderContenido_HDMenuSeleccionado').value='5' + ';' + document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value;
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
            MostrarCapaEspera();
            document.getElementById('ctl00_ContentPlaceHolderContenido_btnMenu').click();
            document.oncontextmenu = function(){return true}
        }
        
        function LlamarAEnDespuesDe()
        {
            document.getElementById('ctl00_ContentPlaceHolderContenido_HDMenuSeleccionado').value='6' + ';' + document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value;
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
            MostrarCapaEspera();
            document.getElementById('ctl00_ContentPlaceHolderContenido_btnMenu').click();
            document.oncontextmenu = function(){return true}
        }
        
        function Propiedades()
        {
            var FilaSeleccionada=parseInt(document.getElementById('ctl00_ContentPlaceHolderContenido_HDFilaSeleccionada').value);
            var CeldaSeleccionada=parseInt(document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value);
              
            var btnRowClick = document.getElementById("ctl00_ContentPlaceHolderContenido_btnRowClick");
            var hdnClickedCellValue = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnClickedCellValue");
            hdnClickedCellValue.value = document.getElementById('ctl00_ContentPlaceHolderContenido_grdListado').rows[FilaSeleccionada + 1].cells[0].innerText;
            MostrarCapaEspera();
            btnRowClick.click();
            document.oncontextmenu = function(){return true}
            return false;
        }
        
        function LlamarAQuitarFiltro()
        {
            document.getElementById('ctl00_ContentPlaceHolderContenido_HDMenuSeleccionado').value='7' + ';' + document.getElementById('ctl00_ContentPlaceHolderContenido_HDColumnaSeleccionada').value;
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
            MostrarCapaEspera();
            document.getElementById('ctl00_ContentPlaceHolderContenido_btnMenu').click();
            document.oncontextmenu = function(){return true}
        }
        function Cerrar()
        {
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image5').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            document.getElementById('MenuContextual').style.visibility ='hidden';
            document.oncontextmenu = function(){return true}
        }
    </script>
        
    <STYLE>
    .Esconder 
    {
    	zoom:100%;
        visibility :hidden;
        font-size:0.8em;
        font-family:Verdana;
	    color:#34721f;
	    text-decoration:none;
	    background-color:#bbbbbb;
	    height :1px;
    }
    
        #divFiltrosColum
        {
            height: 198px;
        }
    .style3
        {
            width: 364px;
        }
    
    </STYLE>
         
    <div id="tablaContratos" 
            style="position:absolute; top: 177px; left: 12px; height: 331px;">
    
        <asp:HiddenField ID="HDFilaSeleccionada" runat="server" />
        <asp:HiddenField ID="HDColumnaSeleccionada" runat="server" />
        <asp:HiddenField ID="HDValorSeleccionado" runat="server" />
        <asp:HiddenField ID="HDMenuSeleccionado" runat="server" />
        
    
        <!--<asp:Panel ID="Panel1" runat="server" Height="293px" ScrollBars="Auto" Width="954px" CssClass="panelTabla" HorizontalAlign="Left" Wrap="False">-->
        
        
            <div id="Datos" style="z-index:9999; overflow: hidden; position:relative;width: 938px;Height:15px; top: 1px; left: 0px;border-left : 1px solid #000000;">
                <asp:GridView ID="DataGridCabecera" 
                    style="z-index:999;position:relative; top: -1px; left: 0px; height: 100px;border:1px solid #000000;" 
                    runat="server" CssClass="contenidoTabla" AutoGenerateColumns="False" 
                    OnRowDataBound="OnGrdCabecera_RowDataBound" AllowSorting="True" 
                    OnRowCreated="GridView_Merge_Header_RowCreated"
                    onsorting="DataGridCabecera_Sorting" EnableViewState="False" 
                    EnableModelValidation="True" meta:resourcekey="DataGridCabeceraResource1">
                    
                    <RowStyle CssClass="filaNormal" />
                    <AlternatingRowStyle CssClass="filaAlterna" />
                    <HeaderStyle HorizontalAlign="Center" CssClass="cabeceraTablaLink"/>
                    <FooterStyle CssClass="cabeceraTabla" />
                    <PagerStyle  CssClass="cabeceraTabla" />
                </asp:GridView>
            </div>
        
            <div id="divScroll" onscroll="javascript:DoScroll();" 
                style="position:absolute;overflow:scroll;width: 954px;height: 325px; top: 0px; left: 0px;border: 1px solid #000000;" 
                runat="server" onmousedown="javascript:Menu();">
                 <asp:GridView ID="grdListado" runat="server" AutoGenerateColumns="False" CssClass="contenidoTabla" 
                     OnRowDataBound="OnGrdListado_RowDataBound" onsorting="DataGridCabecera_Sorting" 
                     AllowSorting="True" EnableViewState="False" EnableModelValidation="True" 
                     meta:resourcekey="grdListadoResource1">
                     
                     <HeaderStyle HorizontalAlign="Center" CssClass="cabeceraTablaLink"/>
                     <RowStyle CssClass="filaNormal" />
                     <AlternatingRowStyle CssClass="filaAlterna" />
                     <SelectedRowStyle CssClass="filaSeleccionada"  />
                     <FooterStyle CssClass="cabeceraTabla" />
                     <PagerStyle  CssClass="cabeceraTabla" />
                    
                    
                </asp:GridView>
            </div>
            
       <!--</asp:Panel>-->
    </div>
    
    <div id="divContador" 
        style="position:absolute; top: 515px; left: 10px; height: 18px; width: 329px" 
        align="center">
        <table width="100%" align="left">
            <tr>
                <td align="left" colspan ="2">
                    <asp:PlaceHolder ID="placeHolderPaginacion" runat="server">
                    &nbsp;  
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label CssClass="labelFormularioValor" ID="lblNumEncontrados" 
                        runat="server" Text="0" meta:resourcekey="lblNumEncontradosResource1"></asp:Label>
                    <asp:Label CssClass="labelFormularioValor" ID="lblNumEncontrados1" 
                        runat="server" Text="0" meta:resourcekey="lblNumEncontrados1Resource1"></asp:Label>
                    <asp:Label CssClass="labelFormulario" ID="lblContador" runat="server" 
                        Text="regs. encontrados de" meta:resourcekey="lblContadorResource1"></asp:Label>
                    <asp:Label CssClass="labelFormularioValor" ID="lblNumRegistros" runat="server" 
                        Text="0 " meta:resourcekey="lblNumRegistrosResource1"></asp:Label>
                </td>
                <td align="left" style="width:40px">        
                </td>

            </tr>
        </table> 
        <asp:HiddenField ID="hdnSortDirection" value="ASC" runat="server" />
        <asp:HiddenField ID="hdnSortColumn" runat="server" />
        <asp:HiddenField ID="hdnPageCount" runat="server" />
        <asp:HiddenField ID="hdnPageIndex" runat="server" />
        <asp:HiddenField ID="hdnRowIndex" runat="server" />
        <asp:HiddenField ID="hdnClickedCellValue" runat="server" />
        <asp:HiddenField ID="hdnRecargarUltimaConsulta" runat="server" />
        <asp:Button ID="btnPaginacion" runat="server"  OnClick="OnBtnPaginacion_Click" 
            Width="0px" meta:resourcekey="btnPaginacionResource1"/> 
        <asp:Button ID="btnRowClick" runat="server"  OnClick="OnBtnRowClick_Click" 
            Width="0px" meta:resourcekey="btnRowClickResource1"/>        
    </div>
    
    <div id="divBotonExcel" 
            style="position:absolute; top: 545px; left: 350px; height: 50px; width: 115px;">
               
                    
                    <input type=button runat="server" id="Button1" name="btnInputExcel" class="botonFormulario" 
                value="Exportar a Excel" onclick="OnBtnInputExcel1('excel')" tabindex="5" />
                
                    <asp:Button ID="btnExcel1" runat="server" CssClass="botonFormulario" 
                    Text="Exportar a Excel" Width="0px"  
                    Height="0px" OnClientClick="javascript:ExportarExcel();return false;" 
                        meta:resourcekey="btnExcel1Resource1" />
                    
                
    </div>
     
    <div id="divBotonVisitas" style="position:absolute; top: 545px; left: 500px; height: 18px; width: 115px;">
        
        <input type=button runat="server" id="btnInputVisitas" name="btnInputVisitas" class="botonFormulario" value="Generar Visitas" onclick="OnBtnInputExcel('visitas')" tabindex="6" />
        <asp:Button ID="btnVisitas" runat="server" Text="Generar Visitas"  
            onclick="OnBtnVisitas_Click" CssClass="botonFormulario" Height="0px" 
            meta:resourcekey="btnVisitasResource1" />
    </div> 
    
    <div id="divBotonVolver" style="position:absolute">
        
        <asp:Button CssClass="botonFormulario" ID="btnVolver" runat="server" 
            Text="Volver" OnClick="OnBtnVolver_Click" TabIndex="7" 
            meta:resourcekey="btnVolverResource1"/>
        <asp:Button Width="0px" ID="btnRowSelected" runat="server" Text="Volver" 
            OnClick="OnBtnRowSelected_Click" TabIndex="7" 
            meta:resourcekey="btnRowSelectedResource1"/>
    </div>
    
    <div id="divBotonExcel1" 
            
        style="position:absolute; top: 745px; left: 150px; height: 50px; width: 115px; visibility: hidden;">
             <input type=button id="btnInputExcel" name="btnInputExcel" class="botonFormulario" value="Exportar a Excel" onclick="OnBtnInputExcel('excel')" tabindex="5"/>
                <asp:Button ID="btnExcel" runat="server" CssClass="botonFormulario" 
                    Text="Exportar a Excel" Width="0px" onclick="OnBtnExcel_Click" 
                    Height="0px" meta:resourcekey="btnExcelResource1"  />
            </div>
<div id="MenuContextual" style="border-style: solid; border-width: 1px; position:absolute; z-index:1; visibility:hidden; top: 92px; left: 383px; width: 211px;" >
            <table style="background:url('HTML/Images/Menu.png'); width: 210px; background-repeat: repeat;" 
                bgcolor="White">
                <%--<tr>
                    <td width="749"  >
                        <a href="#"><asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/deleted.png" /></a> 
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                        <a href='#'><asp:Label ID="Label1" Font-Names="Tahoma" Font-Size="8.5pt" runat="server" Text="Cortar" ></asp:Label></a>
                    </td>
                </tr>--%>
                <tr height="10">
                    <td height="10">
                        <a href="javascript:Copiar();" style="text-decoration:none;">
                        <asp:Image ID="Image7" runat="server" 
                            ImageUrl="HTML/Iconos/copy_clipboard16.ico" meta:resourcekey="Image7Resource1"/></a> 
                        &nbsp;&nbsp;
                        <a href="javascript:Copiar();" style="text-decoration:none;" >
                        <asp:Label ID="lblCopiar" ForeColor="Black" Font-Names="Tahoma" 
                            Font-Size="8.5pt" runat="server" Text="Copiar" 
                            meta:resourcekey="lblCopiarResource1" ></asp:Label></a>
                    </td>
                </tr>
                <%--<tr>
                    <td width="749" >
                        <a href='#'><asp:Image ID="Image8" runat="server" ImageUrl="~/Imagenes/deleted.png" /></a> 
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                        <a href='#'><asp:Label ID="Label4" Font-Names="Tahoma" Font-Size="8.5pt" runat="server" Text="Pegar" ></asp:Label></a>
                    </td>
                </tr>--%>
                <%--<tr><td  height="10"><hr /></td></tr>
                <tr>
                    <td  height="10">
                        <a href="javascript:LlamarAOrdenarAZ();" style="text-decoration:none;"><asp:Image ID="Image9" runat="server" ImageUrl="HTML/Iconos/arrow_down.ico" /></a> 
                        &nbsp;&nbsp;
                        <a href="javascript:LlamarAOrdenarAZ();" style="text-decoration:none;"><asp:Label ForeColor="Black" ID="Label5" Font-Names="Tahoma" Font-Size="8.5pt" runat="server" Text="Ordenar A/Z" ></asp:Label></a>
                    </td>
                </tr>
                <tr>
                    <td  height="10">
                        <a href="javascript:LlamarAOrdenarZA();" style="text-decoration:none;"><asp:Image ID="Image10" runat="server" ImageUrl="HTML/Iconos/arrow_up.ico" /></a> 
                        &nbsp;&nbsp;
                        <a href="javascript:LlamarAOrdenarZA();" style="text-decoration:none;"><asp:Label ForeColor="Black" ID="Label6" Font-Names="Tahoma" Font-Size="8.5pt" runat="server" Text="Ordenar Z/A" ></asp:Label></a>
                    </td>
                </tr>--%>
                <tr><td  height="10"><hr /></td></tr>
               <%-- <tr>
                    <td width="749" >
                        <a href='#'><asp:Image ID="Image11" runat="server" ImageUrl="~/Imagenes/search.png" /></a> 
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                        <a href='#'><asp:Label ID="Label7" Font-Names="Tahoma" Font-Size="8.5pt" runat="server" Text="Quitar filtro de " ></asp:Label></a>
                    </td>
                </tr>
                <tr>
                    <td width="749" >
                        <a href='#'><asp:Image ID="Image12" runat="server" ImageUrl="~/Imagenes/search.png" /></a> 
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                        <a href='#'><asp:Label ID="Label8" Font-Names="Tahoma" Font-Size="8.5pt" runat="server" Text="Filtros de fechas" ></asp:Label></a>
                    </td>
                </tr>
                <tr><td><hr /></td></tr>--%>
                <tr>
                    <td  height="10">
                        <a href="javascript:LlamarAIgualA();" style="text-decoration:none;">
                        <asp:Image ID="Image2" runat="server" ImageUrl="HTML/Iconos/ok.ico" 
                            meta:resourcekey="Image2Resource1" /></a>
                        &nbsp;&nbsp;
                        <a href="javascript:LlamarAIgualA();" style="text-decoration:none;">
                        <asp:Label ForeColor="Black" ID="lblIgual" runat="server" Text="Igual a XXXXX" 
                            Font-Names="Tahoma" Font-Size="8.5pt" 
                            meta:resourcekey="lblIgualResource1" ></asp:Label></a>
                    </td>
                </tr>
                <tr>
                    <td  height="10">
                        <a href="javascript:LlamarADiferenteA();" style="text-decoration:none;">
                        <asp:Image ID="Image4" runat="server" ImageUrl="HTML/Iconos/cross.ico" 
                            meta:resourcekey="Image4Resource1" /></a> 
                        &nbsp;&nbsp;
                        <a href="javascript:LlamarADiferenteA();" style="text-decoration:none;">
                        <asp:Label ForeColor="Black" ID="lblNoIgual" Font-Names="Tahoma" 
                            Font-Size="8.5pt" runat="server" Text="No es igual a " 
                            meta:resourcekey="lblNoIgualResource1" ></asp:Label></a>
                    </td>
                </tr>
                <tr>
                    <td  height="10">
                        <a href="javascript:LlamarAEnAntesDe();" style="text-decoration:none;">
                        <asp:Image ID="Image5" runat="server" ImageUrl="HTML/Iconos/Antes.ico" 
                            meta:resourcekey="Image5Resource1" /></a>
                        &nbsp;&nbsp;
                        <a href="javascript:LlamarAEnAntesDe();" style="text-decoration:none;">
                        <asp:Label ForeColor="Black" ID="lblAntes" Font-Names="Tahoma" 
                            Font-Size="8.5pt" runat="server" Text="En o antes de " 
                            meta:resourcekey="lblAntesResource1" ></asp:Label></a>
                    </td>
                </tr>
                <tr>
                    <td  height="10">
                        <a href="javascript:LlamarAEnDespuesDe();" style="text-decoration:none;">
                        <asp:Image ID="Image6" runat="server" ImageUrl="HTML/Iconos/Despues.ico" 
                            meta:resourcekey="Image6Resource1" /></a>
                        &nbsp;&nbsp;
                        <a href="javascript:LlamarAEnDespuesDe();" style="text-decoration:none;">
                        <asp:Label  ForeColor="Black" ID="lblDespues" Font-Names="Tahoma" 
                            Font-Size="8.5pt" runat="server" Text="En o despues de " 
                            meta:resourcekey="lblDespuesResource1" ></asp:Label></a>
                    </td>
                </tr>
                <%--<tr><td  height="10"><hr style="height: -15px" /></td></tr>
                <tr>
                    <td  height="10">
                        <a href="javascript:Propiedades();" style="text-decoration:none;"><asp:Image ID="Image3" runat="server" ImageUrl="HTML/Iconos/info.ico" /></a> 
                        &nbsp;&nbsp;
                        <a href="javascript:Propiedades();" style="text-decoration:none;"><asp:Label ID="Label3" ForeColor="Black" Font-Names="Tahoma" Font-Size="8.5pt" runat="server" Text="Propiedades" ></asp:Label></a>
                    </td>
                </tr>--%>
                <tr><td  height="10"><hr style="height: -15px" /></td></tr>
                <tr>
                    <td  height="10">
                        <a href="javascript:LlamarAQuitarFiltro();" style="text-decoration:none;">
                        <asp:Image ID="Image1" runat="server" ImageUrl="HTML/Iconos/filterb16_h.ico" 
                            meta:resourcekey="Image1Resource1" /></a> 
                        &nbsp;&nbsp;
                        <a href="javascript:LlamarAQuitarFiltro();" style="text-decoration:none;">
                        <asp:Label ID="lblQuitarFiltro" ForeColor="Black" Font-Names="Tahoma" 
                            Font-Size="8.5pt" runat="server" Text="Quitar Filtro" 
                            meta:resourcekey="lblQuitarFiltroResource1" ></asp:Label></a>
                    </td>
                </tr>
                <tr>
                    <td  height="10">
                        <a href="javascript:Cerrar();" style="text-decoration:none;">
                        <asp:Image ID="Image3" runat="server" ImageUrl="HTML/Iconos/no.ico" 
                            meta:resourcekey="Image3Resource1" /></a> 
                        &nbsp;&nbsp;
                        <a href="javascript:Cerrar();" style="text-decoration:none;">
                        <asp:Label ID="Label3" ForeColor="Black" Font-Names="Tahoma" Font-Size="8.5pt" 
                            runat="server" Text="Cerrar" meta:resourcekey="Label3Resource1" ></asp:Label></a>
                    </td>
                </tr>
            </table>
        </div>
        
        <asp:Button ID="btnMenu" Width="0px" Height="0px" runat="server" Text="Button" 
            onclick="btnMenu_Click" meta:resourcekey="btnMenuResource1" />
        

    <script type="text/javascript">
        function OnBtnAbrirFakeComboLotes_Click() {
            abrirVentanaLocalizacion("./FrmModalFakeComboLotes.aspx", "650", "450", "ModalFiltrosAvanzados", "<%=Resources.TextosJavaScript.TEXTO_FILTROS_AVANZADOS%>", "", false);
        }
        
        function OnBtnFiltrosAvanzados_Click() {
            abrirVentanaLocalizacion("./FrmModalFiltrosAvanzados.aspx", "800", "500", "ModalFiltrosAvanzados", "<%=Resources.TextosJavaScript.TEXTO_FILTROS_AVANZADOS%>", "", false);
        }
        
        function OnBtnFiltrosColumna_Click(numColumna) {
            var grid = document.getElementById("ctl00_ContentPlaceHolderContenido_grdListado");
            var descripcionColumnaFiltro = grid.rows[0].cells[numColumna].innerText;

            abrirVentanaLocalizacion("./FrmModalFiltrosColumna.aspx?NUM_COLUMNA_FILTRO=" + numColumna + "&DESCRIPCION_COLUMNA_FILTRO=" + descripcionColumnaFiltro, "650", "450", "ModalFiltrosColumna", "<%=Resources.TextosJavaScript.TEXTO_FILTROS_COLUMNA%>", "", false);
        }        
       
        function GridViewRowClick(ident)
        {
            var btnRowClick = document.getElementById("ctl00_ContentPlaceHolderContenido_btnRowClick");
            var hdnClickedCellValue = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnClickedCellValue");
            hdnClickedCellValue.value = ident;
            btnRowClick.click();
            return false;
        }
         
        function ClickPaginacion(ident)
        {
            var botonPaginacion = document.getElementById("ctl00_ContentPlaceHolderContenido_btnPaginacion");
            var hiddenPaginacion = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnPageIndex");
            hiddenPaginacion.value = ident;
            botonPaginacion.click();
            return false;
        }
        
        function OnDataGridLink_ClientClick(ident)
        {
            var botonRowSelected = document.getElementById("ctl00_ContentPlaceHolderContenido_btnRowSelected");
            var hiddenRowIndex = document.getElementById("ctl00_ContentPlaceHolderContenido_hdnRowIndex");
            hiddenRowIndex.value = ident;
            botonRowSelected.click();
            return true;
        }
        
        function OnBtnInputExcel(accion)
        {
            var hdnRecargarUltimaConsulta = document.getElementById('ctl00_ContentPlaceHolderContenido_hdnRecargarUltimaConsulta');
            hdnRecargarUltimaConsulta.value = 'TRUE';
            
            if (accion == "excel")
            {
                var btnAccion = document.getElementById("ctl00_ContentPlaceHolderContenido_btnExcel");
            }
            else
            {
                var btnAccion = document.getElementById("ctl00_ContentPlaceHolderContenido_btnVisitas");
            }
            btnAccion.click();
        }
        
        function OnBtnInputExcel1(accion)
        {
            var hdnRecargarUltimaConsulta = document.getElementById('ctl00_ContentPlaceHolderContenido_hdnRecargarUltimaConsulta');
            hdnRecargarUltimaConsulta.value = 'TRUE';
            
            if (accion == "excel")
            {
                var btnAccion = document.getElementById("ctl00_ContentPlaceHolderContenido_btnExcel1");
            }
            else
            {
                var btnAccion = document.getElementById("ctl00_ContentPlaceHolderContenido_btnVisitas");
            }
            btnAccion.click();
        }
    </script>
</asp:Content>