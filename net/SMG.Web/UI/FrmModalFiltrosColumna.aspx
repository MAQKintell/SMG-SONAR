<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageModal.master" AutoEventWireup="true"
    CodeFile="FrmModalFiltrosColumna.aspx.cs" Inherits="Iberdrola.SMG.UI.FrmModalFiltrosColumna" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="FrmModalGenerarVisitas" ContentPlaceHolderID="ContentPlaceHolderContenido"
    runat="server">

    <script type="text/javascript">
       
        function checkUncheckAll(check) 
        {
            var formulario = document.forms["aspnetForm"];
            var z = 0;
	        for(z=0; z<formulario.length;z++)
	        {
                if(formulario[z].type == 'checkbox')
                {
	                formulario[z].checked = check;
	            }
            }
        }

    </script>
    <asp:ScriptManager ID="SM" runat="server">
    </asp:ScriptManager>

    <div id="divPanelFiltroColumna" style="position: absolute; top: 18px; left: 20px;
        width: 605px; height: 320px;">
        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Width="603px" 
            meta:resourcekey="Panel1Resource1">
            <input type="button" name="bSeleccionarTodo" class="botonFormulario" value="Seleccionar Todo" onClick="checkUncheckAll(true)" />
            <input type="button" name="bDeselecionarTodo" class="botonFormulario" value="Deselecionar Todo" onClick="checkUncheckAll(false)" />
        </asp:Panel>
        <asp:Panel ID="pnFiltroColumna" runat="server" GroupingText="Valores posibles del campo "
            Width="600px" CssClass="labelFormulario" Height="329px" 
            meta:resourcekey="pnFiltroColumnaResource1">
            <asp:Panel ID="pnLista" runat="server" Height="294px" ScrollBars="Auto" 
                Wrap="False" Width="595px" meta:resourcekey="pnListaResource1">
                <asp:CheckBoxList ID="cblFiltrosColumna" runat="server" 
                    cssClass="campoFormulario" meta:resourcekey="cblFiltrosColumnaResource1">
                </asp:CheckBoxList>

            </asp:Panel>
            
        </asp:Panel>
    </div>

  
    <div id="divBotonAceptar" style="position: absolute; width: 161px; top: 382px; left: 225px;
        height: 22px; margin-right: 1px;">
        <asp:Button ID="btnAceptar" runat="server" CssClass="botonFormulario" Text="Aceptar"
            Width="75px" OnClick="OnBtnAceptar_Click" TabIndex="11" 
            meta:resourcekey="btnAceptarResource1" />
        <asp:Button ID="btnCancelar" runat="server" CssClass="botonFormulario" Text="Cancelar"
            Width="75px" OnClick="OnBtnCancelar_Click" TabIndex="11" 
            meta:resourcekey="btnCancelarResource1" />
    </div>
</asp:Content>
