<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true" CodeFile="FrmModalFakeComboLotes.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalFakeComboLotes" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalGenerarVisitas" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
    <div id="divInformacionLote" 
        
        
        style="position:absolute; width: 614px; top: 12px; left: 10px; height: 40px; margin-right: 1px;">
        <asp:Label ID="lblDescLote" runat="server" Text="Descripción Lote" 
            CssClass="labelFormulario" meta:resourcekey="lblDescLoteResource1"></asp:Label>
        <asp:TextBox ID="txtDescLote" runat="server" CssClass="campoFormulario" 
            MaxLength="255" Width="300px" meta:resourcekey="txtDescLoteResource1"></asp:TextBox>
        <asp:Button ID="btnBuscarLotes" runat="server" Text="Filtrar" 
            CssClass="botonFormulario"  OnClick="OnBtnBuscarLotes_Click" 
            meta:resourcekey="btnBuscarLotesResource1"/>
    </div>
    
   <div id="divListadoResultado" 
    style="position:absolute; width: 614px; top: 63px; left: 10px; height: 348px; margin-right: 1px;" >
        <div class="labelFormularioValor">Resultados</div>

       <div style="border: 1px solid #000000; overflow:scroll; height:325px">
            <asp:GridView ID="grdLotes" runat="server" AutoGenerateColumns="False" 
                CssClass="contenidoTabla" EnableModelValidation="True" 
                meta:resourcekey="grdLotesResource1">
                <Columns>
                    <asp:TemplateField HeaderText="Código Lote" SortExpression="ID_LOTE" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="90px" 
                        HeaderStyle-Width="90px" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk2" runat="server" Text='<%# Bind("ID_LOTE") %>' 
                                OnClick="OnRowSelected_Click" meta:resourcekey="lnk2Resource1" ></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>   
                    <asp:BoundField DataField="DESC_LOTE" HeaderText="Descripción Lote" 
                        SortExpression="DESC_LOTE" ItemStyle-HorizontalAlign="Center" 
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="500px" 
                        HeaderStyle-Width="500px" meta:resourcekey="BoundFieldResource1">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <RowStyle CssClass="filaNormal"/>
                <AlternatingRowStyle CssClass="filaAlterna"/>
                <FooterStyle CssClass="cabeceraTabla"  />
                <PagerStyle  CssClass="cabeceraTabla" />
                <HeaderStyle CssClass="cabeceraTabla" />
            </asp:GridView>
            </div>
          </div>

<script type="text/javascript">
    function SeleccionarLote(idLote, descLote)
    {
        var txtIdLote = parent.document.getElementById("ctl00$ContentPlaceHolderContenido$txtIdLote");
        var txtDescLote = parent.document.getElementById("ctl00$ContentPlaceHolderContenido$txtDescripcionLote");
        
        txtIdLote.value = idLote;
        //Se lanza el evento para que este rellene la descripcion
        txtIdLote.onchange();

        parent.VentanaModal.cerrar();
    }
</script>
</asp:Content>
