<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmPrueba.aspx.cs" Inherits="Iberdrola.SMG.UI_PruebasDiseño.frmPrueba" MasterPageFile="MasterPagePrueba.master" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="Iberdrola.Commons.Web" %>


<asp:Content ID="frmPrueba" ContentPlaceHolderID="ContentPlaceHolderContenido" runat="server">
<div>
<asp:Button runat="server" Text="QQQ" />
 <asp:GridView ID="gvSolicitudes" runat="server" AutoGenerateColumns="False"
                            Width="925px"  CssClass="contenidoTabla" 
                    EnableModelValidation="True" meta:resourcekey="gvSolicitudesResource1"
                    >
                    <RowStyle CssClass="filaNormal" BorderColor ="Transparent" />
                    <AlternatingRowStyle CssClass="filaAlterna"/>
                    <FooterStyle CssClass="cabeceraTabla" />
                    <PagerStyle  CssClass="cabeceraTabla" />
                    <HeaderStyle CssClass="cabeceraTabla" BorderColor ="Transparent" />     
                   
                     <Columns>
                                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/flecha.gif" 
                                                        ShowSelectButton="True" meta:resourcekey="CommandFieldResource2" />
                                                  <asp:BoundField DataField="ID_solicitud" HeaderText="COD. Solicitud"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource8">
                                                   <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="Cod_contrato" HeaderText="Contrato"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource9">
                                                   <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="tipo_solicitud" HeaderText="Tipo"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource10">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="subtipo_solicitud" 
                                                        HeaderText="subTipo"  ReadOnly="True" meta:resourcekey="BoundFieldResource11">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="estado_solicitud" 
                                                        HeaderText="estado_solicitud"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource12">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField Visible="false" DataField="Cod_contrato" 
                                                        HeaderText="Código contrato"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource13">
                                                   <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                   <asp:BoundField DataField="Des_Tipo_solicitud" HeaderText="Tipo"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource14">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  
                                                  
                                                  <asp:BoundField DataField="DESCRIPCION" HeaderText="Subtipo"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource15">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="desSolicitud" HeaderText="Estado"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource16">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  
                                                  
                                                  <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha creación"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource17">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                  <asp:BoundField DataField="telefono_contacto" HeaderText="Teléfono contacto"  
                                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource18">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="Persona_contacto" HeaderText="Persona contacto"  
                                                        ReadOnly="True" Visible="False" meta:resourcekey="BoundFieldResource19">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="Des_averia" HeaderText="Motivo Avería"  
                                                        ReadOnly="True" meta:resourcekey="BoundFieldResource20">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="NOM_PROVINCIA" HeaderText="Provincia" 
                                                        meta:resourcekey="BoundFieldResource21" />
                                                  
                                                  <asp:TemplateField HeaderText="Observaciones" 
                                                        meta:resourcekey="TemplateFieldResource1">
                                                         <ItemTemplate>   
                                                            <asp:Label ID="lb_Observaciones" runat="server" 
                                                                 ToolTip='<%# Eval("Observaciones") %>' 
                                                                 Text='<%# Iberdrola.Commons.Utils.StringUtils.ReduceText(Eval("Observaciones")) %>'></asp:Label>  
                                                         </ItemTemplate>
                                                  </asp:TemplateField> 

                                                    <asp:CheckBoxField DataField="Urgente" HeaderText="URGENTE" 
                                                        meta:resourcekey="CheckBoxFieldResource1" />
                                                    <asp:BoundField DataField="Proveedor" HeaderText="PROVEEDOR" 
                                                        meta:resourcekey="BoundFieldResource22" />
                                                  
                                                    <%--<asp:BoundField DataField="Fec_visita" HeaderText="Fecha Visita Realizada" ReadOnly="True">
                                                      <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    
                                                    <%--<asp:TemplateField HeaderText="CUPS">
                                                         <ItemTemplate>   
                                                            <asp:Label ID="lb_CUPS" runat="server" ToolTip='<%# Eval("CUPS")%>' Text='<%# acorta(Eval("CUPS")) %>'></asp:Label>  
                                                         </ItemTemplate>
                                                  </asp:TemplateField>--%> 
                                                  
                                                  <asp:BoundField DataField="COD_PROVEEDOR" 
                                                        HeaderText="PROVEEDOR ULTIMO MOVIMIENTO"  ReadOnly="True" 
                                                        DataFormatString="{0:d}" meta:resourcekey="BoundFieldResource23">
                                                  <HeaderStyle HorizontalAlign="Left" />
                                                  </asp:BoundField>
                                                    <asp:BoundField DataField="EFV" HeaderText="EFV"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource24">
                                                      <HeaderStyle HorizontalAlign="Left" />
                                                      </asp:BoundField>
                                                  <asp:BoundField DataField="SCORING" HeaderText="SCORING"  ReadOnly="True" 
                                                        meta:resourcekey="BoundFieldResource25">
                                                      <HeaderStyle HorizontalAlign="Left" />
                                                      </asp:BoundField>
                                                  <%--<asp:BoundField DataField="id_perfil" HeaderText="Perfil" />--%>
                                                </Columns>                
                   
                </asp:GridView>

                           </div>
</asp:Content>
