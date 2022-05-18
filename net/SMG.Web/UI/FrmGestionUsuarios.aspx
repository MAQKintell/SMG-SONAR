<%@ Page Language="C#" MasterPageFile="~/UI/MasterPageMntoGasSinMenu.master" AutoEventWireup="true" CodeFile="FrmGestionUsuarios.aspx.cs" Inherits="Iberdrola.SMG.UI.frmGestionUsuarios" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Import Namespace="Iberdrola.Commons.Web" %>
        
<asp:Content ID="FrmGestionUsuarios" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
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
        
        function Buscar (opcion)
        {
            abrirVentanaLocalizacion("./FrmModalBuscarUsuario.aspx?OPCION=" + opcion , "650", "450", "ModalBuscarUsuario", "<%=Resources.TextosJavaScript.TEXTO_BUSCAR_USUARIO%>", "", false);       
        }
        
        function Limpiar()
        {
            document.getElementById('ctl00_ContentPlaceHolderContenido_txtCodigo').value="";
            document.getElementById('ctl00_ContentPlaceHolderContenido_txtCodigo').disabled = false;
            document.getElementById('ctl00_ContentPlaceHolderContenido_txtNombre').value="";
            document.getElementById('ctl00_ContentPlaceHolderContenido_txtContrasenia').value="";
            document.getElementById('ctl00_ContentPlaceHolderContenido_txtEmail').value="";
            //document.getElementById('ctl00_ContentPlaceHolderContenido_txtIDPerfil').value="";
            document.getElementById('ctl00_ContentPlaceHolderContenido_cbPermiso').checked = false;
            document.getElementById('ctl00_ContentPlaceHolderContenido_bAniadir').disabled = false;
            document.getElementById('ctl00_ContentPlaceHolderContenido_bEliminar').disabled = true;
            document.getElementById('ctl00_ContentPlaceHolderContenido_bModificar').disabled =true;
            document.getElementById('ctl00_ContentPlaceHolderContenido_hdnUserID').value ='';
            
            document.getElementById('ctl00_ContentPlaceHolderContenido_cboPerfiles').selectedIndex=0;//.value =;
            
            
            
            document.getElementById('ctl00_ContentPlaceHolderContenido_chkActivo').checked = false;
            //document.getElementById('ctl00_ContentPlaceHolderContenido_lblAntes').style.visibility ='hidden';
            //document.getElementById('ctl00_ContentPlaceHolderContenido_Image6').style.visibility ='hidden';
            //document.getElementById('ctl00_ContentPlaceHolderContenido_lblDespues').style.visibility ='hidden';
            //document.getElementById('MenuContextual').style.visibility ='hidden';
            //document.getElementById('ctl00$ContentPlaceHolderContenido$btnMenu').click();
        }
        
        </script>
    
<div id="divTituloVentana">
        <asp:Label CssClass="divTituloVentana" ID="lblTituloVentana" runat="server" 
            Text="Gestion de Usuarios" meta:resourcekey="lblTituloVentanaResource1"></asp:Label>
    </div>
    
    <div id="div1" style="position: relative;">
     <asp:Panel ID="Panel1" runat="server" 
            Height="269px" width="562px" CssClass="labelFormulario" 
            meta:resourcekey="Panel1Resource1">

            <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblBajaAutomatica" runat="server" CssClass="campoFormulario" 
                        Font-Bold="True" ForeColor="Red" Visible="False" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="campoFormulario" 
                        Text="Login: " meta:resourcekey="Label2Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="campoFormulario" 
                        MaxLength="7" Width="250px" meta:resourcekey="txtCodigoResource1"></asp:TextBox>  
                    &nbsp;
                    <asp:CustomValidator ID="txtCodigoValidator" CssClass="errorFormulario" 
                        runat="server" ErrorMessage="*" ControlToValidate="txtCodigo" 
                        OnServerValidate="txtCodigo_ServerValidate" ValidateEmptyText="True" 
                        meta:resourcekey="txtCodigoValidatorResource1"></asp:CustomValidator>
                    <a href="javascript:Buscar('1');" style="text-decoration:none;">
                        <asp:Label ID="Label1" CssClass="botonFormulario" runat="server" Text="..." 
                        ToolTip="Buscar Usuario" meta:resourcekey="Label1Resource1"></asp:Label></a>
                    
                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" CssClass="campoFormulario" 
                        Text="Nombre y Apellidos: " meta:resourcekey="Label3Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="campoFormulario" 
                        MaxLength="50" Width="250px" meta:resourcekey="txtNombreResource1"></asp:TextBox>
                    &nbsp;
                    <asp:CustomValidator ID="txtNombreValidator" CssClass="errorFormulario" 
                        runat="server" ErrorMessage="*" ControlToValidate="txtNombre" 
                        OnServerValidate="txtNombre_ServerValidate" ValidateEmptyText="True" 
                        meta:resourcekey="txtNombreValidatorResource1"></asp:CustomValidator>
                    <a href="javascript:Buscar('2');" style="text-decoration:none;">
                    <asp:Label ID="Label8" CssClass="botonFormulario" runat="server" Text="..." 
                        ToolTip="Buscar Usuario" meta:resourcekey="Label8Resource1"></asp:Label></a>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" CssClass="campoFormulario" 
                        Text="Contraseña: " meta:resourcekey="Label4Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtContrasenia" runat="server" CssClass="campoFormulario" 
                        MaxLength="10" Width="250px" meta:resourcekey="txtContraseniaResource1"></asp:TextBox>
                    &nbsp;
                    <asp:CustomValidator ID="txtContraseniaValidator" CssClass="errorFormulario" 
                        runat="server" ErrorMessage="*" ControlToValidate="txtContrasenia" 
                        OnServerValidate="txtContrasenia_ServerValidate" ValidateEmptyText="True" 
                        meta:resourcekey="txtContraseniaValidatorResource1"></asp:CustomValidator>
                        
                </td>
            </tr>
                        <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" CssClass="campoFormulario" 
                        Text="Email: " meta:resourcekey="Label5Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="campoFormulario" 
                        MaxLength="50" Width="250px" meta:resourcekey="txtEmailResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" CssClass="campoFormulario" 
                        Text="Perfil: " meta:resourcekey="Label6Resource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtIDPerfil" runat="server" CssClass="campoFormulario" 
                        MaxLength="18" Width="73px" Visible="False" 
                        meta:resourcekey="txtIDPerfilResource1"></asp:TextBox>
                    <asp:DropDownList ID="cboPerfiles" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="cboPerfiles_SelectedIndexChanged" Width="170px" 
                        meta:resourcekey="cboPerfilesResource1">
                    </asp:DropDownList>    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" CssClass="campoFormulario" 
                        Text="Puede administrar usuarios: " meta:resourcekey="Label7Resource1"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="cbPermiso" runat="server" CssClass="campoFormulario" 
                        meta:resourcekey="cbPermisoResource1"/>
                </td>
            </tr>           
            <tr>
                <td>
                    <asp:Label ID="Label9" runat="server" CssClass="campoFormulario" 
                        Text="Usuario activo: " meta:resourcekey="Label9Resource1"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkActivo" runat="server" CssClass="campoFormulario" 
                        meta:resourcekey="chkActivoResource1"/>
                    <asp:Label ID="lblBajaInfo" runat="server" CssClass="campoFormulario" 
                        Text="Baja realizada por:" Visible="False" 
                        meta:resourcekey="lblBajaInfoResource1"></asp:Label>
                    <asp:Label ID="lblUsuarioBaja" runat="server" CssClass="campoFormulario" 
                        Visible="False" meta:resourcekey="lblUsuarioBajaResource1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPais" runat="server" CssClass="campoFormulario" 
                        Text="Pais: " meta:resourcekey="lblPaisResource1"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbPais" BackColor="White" runat="server" 
                            CssClass="campoFormulario" Width="198px" 
                            meta:resourcekey="cmbPaisResource1" />
                </td>
            </tr>      
            
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:HiddenField ID="hdnUserID" runat="server" />
                    <asp:Button ID="hdnBtnCargarUsuario" runat="server" CausesValidation="False" 
                        Height="0px" onclick="hdnBtnCargarUsuario_Click" Text="Button" Width="0px" 
                        meta:resourcekey="hdnBtnCargarUsuarioResource1" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="bAniadir" runat="server" CssClass="botonFormulario" 
                        TabIndex="1" 
                        Text="Añadir" onclick="bAniadir_Click" 
                        meta:resourcekey="bAniadirResource1" />
                    <asp:Button ID="bModificar" runat="server" CssClass="botonFormulario" 
                        OnClientClick="MostrarCapaEspera();" TabIndex="1" 
                        Text="Modificar" Enabled="False" onclick="bModificar_Click" 
                        meta:resourcekey="bModificarResource1" />
                    <asp:Button ID="bEliminar" runat="server" CssClass="botonFormulario" 
                         TabIndex="1" 
                        Text="Eliminar" Enabled="False" onclick="bEliminar_Click" 
                        OnClientClick="return Confirmacion()" meta:resourcekey="bEliminarResource1"/>
                    <input type="button" runat="server" id="bLimpiar1" class="botonFormulario" value="Limpiar" onclick="Limpiar()" />
                    
                &nbsp;</td>
            </tr>
            </table>


        </asp:Panel>           
    </div>
          <script type="text/javascript">
            function Confirmacion()
            {

                var Respuesta = confirm("<%=Resources.TextosJavaScript.TEXTO_ELIMINAR_USUARIO%>"); //"Va a eliminar el usuario, ¿Desea Continuar?");
                if (Respuesta ==false){ OcultarCapaEspera(); }
                else{MostrarCapaEspera();}
                return Respuesta;
            }
          </script>
     
     
</asp:Content>
