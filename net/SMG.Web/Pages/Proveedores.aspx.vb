Imports System.Data
Imports System.Web.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Globalization

Imports System.Data.Common

Imports Iberdrola.Commons.Web


Partial Class Pages_Proveedores
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Not Page.IsPostBack()) Then
        '          Dim Cultur As AsignarCultura = New AsignarCultura()
        '          Cultur.Asignar(Page)
        'End if
        'AddHandler gv_Solicitudes.Grid.ItemDataBound, AddressOf ItemDataBound

        If (Not Page.IsPostBack()) Then

            'Dim Cultur As AsignarCultura = New AsignarCultura()
            'Cultur.Asignar(Page)



            hiddenPaginacion.Value = "1"
            btnCaldera0.Visible = False
            'btnCalFechaCierre.Attributes.Add("onclick", "MostrarCalendario()") ''Calendario.aspx?Formulario=Form3&Control=txtFechaLimite', 250, 260)") ', False, False, False, False, False, False))
            'btnCalFechaCierre.Attributes.Add("onclick", "MostrarCalendario('Calendario.aspx?Formulario=Form3&Control=TbFechaCierre', 250, 260)") ', False, False, False, False, False, False))
            'cal_Desde.Visible = False
            Session("NumeroRegistrosAVisualizar") = CInt(ConfigurationManager.AppSettings("NumeroRegistrosAVisualizar").ToString())
            Session("DesdePagina") = 0
            Session("HastaPagina") = CInt(Session("NumeroRegistrosAVisualizar")) '5
            Dim usuario As String = String.Empty

            Try

                usuario = Session("usuarioValido").ToString()

            Catch ex As Exception
                Dim redirectURL As String = ConfigurationManager.AppSettings("url_Login").ToString()
                Response.Redirect(redirectURL, False)
            End Try


            ' controlamos que el perfil de la página es correcto 5, 6, 4, 1 ó 2 (vamos todos menos el teléfono).
            Dim IdPerfil As String = Session("IdPerfil")
            Dim perfilValido As Boolean = False

            If (String.IsNullOrEmpty(IdPerfil)) Then
                perfilValido = False
            Else
                If "4".Equals(IdPerfil) Or "1".Equals(IdPerfil) Or "2".Equals(IdPerfil) Or "5".Equals(IdPerfil) Or "6".Equals(IdPerfil) Or "7".Equals(IdPerfil) Then
                    perfilValido = True
                Else
                    perfilValido = False
                End If
            End If

            btnCargarSolictudesTelefono.Visible = False
            FileUpload1.Visible = False
            If Not perfilValido Then
                '   'En caso de ser teléfono habría que sacar el tema de Nivel de satisfacción, 
                '   'bien en esta o en otra página.
                '   '30/12/2010.
                ''Button1.Enabled = False
                ''Button2.Enabled = False
                ''Button3.Enabled = False
                '    Dim redirectURL As String = ConfigurationManager.AppSettings("url_Login").ToString()
                '    Response.Redirect(redirectURL, False)
                btnCargarSolictudesTelefono.Visible = True
                FileUpload1.Visible = True
            End If

            If Not String.IsNullOrEmpty(usuario) Then
                Dim objUsuariosDB As New UsuariosDB()
                Dim dr As SqlDataReader = objUsuariosDB.GetProveedorUsuario(usuario)
                dr.Read()
                Dim cod_proveedor As String = dr("proveedor").ToString()
                Dim nombre_proveedor As String = dr("nombre").ToString()
                CargaDatosFiltro(cod_proveedor, nombre_proveedor)
                chk_Pendientes.Checked = True
                'CargaSolicitudes()
                Me.txtDesde.Text = ""
                Me.txtHasta.Text = ""
            End If

            CargaComboSubtipoSolicitudTodas()

            lblTotal.Visible = False
            lblTotal0.Visible = False
            lblPaginas.Visible = False
            lblPaginaActual.Visible = False

        End If

        lbl_mensajes.Text = String.Empty

        PonerFoco(txt_contrato)





        'If Not Request.QueryString("VolverACargar") = Nothing Then

        '    Dim idSolicitud As String = Request.QueryString("VolverACargar")
        '    Dim i As Integer = InStr(idSolicitud, "#")
        '    idSolicitud = Mid(idSolicitud, 1, i - 1)
        '    CargaDatosSolicitud(idSolicitud)
        'End If
    End Sub


    Private Function PonerFoco(ByVal ctrl As Control)
        Dim focusScript As String = "<script language='JavaScript'>" & _
    "document.getElementById('" + ctrl.ClientID & _
    "').focus();</script>"

        Page.RegisterStartupScript("FocusScript", focusScript)
    End Function

#Region "Funciones"

    Private Sub CargaDatosFiltro(ByVal cod_proveedor As String, ByVal nombre_proveedor As String)

        hidden_proveedor.Value = cod_proveedor
        txt_Proveedor.Text = nombre_proveedor

        CargaComboTipoSolicitud()
        cargarcomboprovincias()
        CargaComboSubtipoSolicitud(ddl_Tipo.SelectedValue)
        CargaComboEstadoSolicitud(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue)
        ''CargaComboCalderas()
        'CargaComboMotivosCancelacion(ddl_Subtipo.SelectedValue)
        CargaComboEstadosFuturos(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, ddl_Estado.SelectedValue, "Ninguno")

    End Sub

    Private Sub CargarNivelSatisfaccion()
        Try
            'Kintell 30/12/2010.
            IncializarControles()


            Dim objProveedoresDB As New ProveedoresDB()
            Dim dr As SqlDataReader = objProveedoresDB.GetCodigoProveedorPorNombre(txt_Proveedor.Text)
            dr.Read()
            hidden_proveedor.Value = dr("proveedor").ToString()

            Dim cod_proveedor As String = hidden_proveedor.Value
            Dim cod_contrato As String = txt_contrato.Text
            Dim consultaPendietes As Boolean = chk_Pendientes.Checked
            Dim cod_tipo As String = ddl_Tipo.SelectedValue
            Dim cod_subtipo As String = ddl_Subtipo.SelectedValue
            Dim cod_estado As String = ddl_Estado.SelectedValue
            'Dim caldera As String = ddl_MarcaCaldera.Text

            Dim fechaDesde As String = cal_Desde.SelectedDate.ToString("yyyy/MM/dd") 'txtDesde.Text
            Dim fechaHasta As String = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd") 'txtHasta.Text

            If Not txtDesde.Text = "" And Not txtHasta.Text = "" Then
                txtDesde.Text = cal_Desde.SelectedDate.ToString("dd/MM/yyyy")
                'fechaHasta = txtHasta.Text
                If txtHasta.Text = "" Then txtHasta.Text = cal_Hasta.SelectedDate.ToString("dd/MM/yyyy")
                If txtHasta.Text = "01/01/0001" Then
                    txtHasta.Text = Mid(Now, 1, 10)
                Else
                    cal_Hasta.SelectedDate = txtHasta.Text

                    fechaHasta = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd")
                End If


                If fechaDesde = "0001/01/01" Then
                    fechaDesde = String.Empty
                End If
                If fechaHasta = "0001/01/01" Then
                    fechaHasta = String.Empty
                End If
            Else
                fechaDesde = String.Empty
                fechaHasta = String.Empty
            End If

            Dim objSolicitudesDB As New SolicitudesDB()
            Dim Urgente As String = "False"
            If Me.chk_Urgente.Checked Then Urgente = "True"

            Dim dsSolicitudes As DataSet = objSolicitudesDB.GetSolicitudesPorProveedor(cod_proveedor, cod_contrato, consultaPendietes, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue, Urgente, Session("DesdePagina"), Session("HastaPagina"), Session("IdPerfilColores"), txt_solicitud.Text)
            Session("RegistrosTotales") = objSolicitudesDB.ObtenerRegistrosTotales(cod_proveedor, consultaPendietes, cod_contrato, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, Urgente, ddl_Provincia.SelectedValue, Session("IdPerfilColores"), txt_solicitud.Text)




            Session("dsSolicitudes") = dsSolicitudes

            gv_Solicitudes.Columns(0).Visible = True
            gv_Solicitudes.Columns(1).Visible = True
            gv_Solicitudes.Columns(2).Visible = True
            gv_Solicitudes.Columns(3).Visible = True
            gv_Solicitudes.Columns(4).Visible = True

            gv_Solicitudes.DataSource = dsSolicitudes
            gv_Solicitudes.DataBind()

            '' las oculto despues del databind para que me mantenga los datos
            'gv_Solicitudes.Columns(0).Visible = False
            'gv_Solicitudes.Columns(1).Visible = False
            gv_Solicitudes.Columns(2).Visible = False
            gv_Solicitudes.Columns(3).Visible = False
            gv_Solicitudes.Columns(4).Visible = False

            gv_Solicitudes.Columns(5).Visible = False


            'Kintell 14/04/2009
            If Not UCase(txt_Proveedor.Text) = UCase("admin") Then
                gv_Solicitudes.Columns(17).Visible = False

                If Not UCase(txt_Proveedor.Text) = UCase("") Then
                    gv_Solicitudes.Columns(17).Visible = False

                    If Not UCase(txt_Proveedor.Text) = UCase("adico") Then gv_Solicitudes.Columns(17).Visible = False
                End If

            End If

            txt_Observaciones.Text = ""
            'gv_Solicitudes.ToolTip = Eval("Observaciones") ' gv_Solicitudes.Columns(15).ToString
        Catch ex As Exception
            gv_Solicitudes.DataSource = Nothing
            gv_Solicitudes.DataBind()
        End Try
    End Sub

    Private Sub CargaSolicitudes()

        Try
            'Kintell 22/04/2009.
            IncializarControles()


            Dim objProveedoresDB As New ProveedoresDB()
            Dim dr As SqlDataReader = objProveedoresDB.GetCodigoProveedorPorNombre(txt_Proveedor.Text)
            dr.Read()
            hidden_proveedor.Value = dr("proveedor").ToString()
            dr.Close()
            Dim cod_proveedor As String = hidden_proveedor.Value


            '*************************************************************************************
            'Kintell 28/01/2011 Teléfono.- Llamadas de cortesia
            If UCase(txt_Proveedor.Text) = "TELEFONO" And String.IsNullOrEmpty(cod_proveedor) Then
                cod_proveedor = "TEL"
            End If
            '*************************************************************************************


            Dim cod_contrato As String = txt_contrato.Text
            Dim consultaPendietes As Boolean = chk_Pendientes.Checked
            Dim cod_tipo As String = ddl_Tipo.SelectedValue
            Dim cod_subtipo As String = ddl_Subtipo.SelectedValue
            Dim cod_estado As String = ddl_Estado.SelectedValue
            'Dim caldera As String = ddl_MarcaCaldera.Text

            Dim fechaDesde As String = cal_Desde.SelectedDate.ToString("yyyy/MM/dd") 'txtDesde.Text
            Dim fechaHasta As String = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd") 'txtHasta.Text

            If Not txtDesde.Text = "" And Not txtHasta.Text = "" Then
                txtDesde.Text = cal_Desde.SelectedDate.ToString("dd/MM/yyyy")
                'fechaHasta = txtHasta.Text
                If txtHasta.Text = "" Then txtHasta.Text = cal_Hasta.SelectedDate.ToString("dd/MM/yyyy")
                If txtHasta.Text = "01/01/0001" Then
                    txtHasta.Text = Mid(Now, 1, 10)
                Else
                    cal_Hasta.SelectedDate = txtHasta.Text

                    fechaHasta = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd")
                End If


                If fechaDesde = "0001/01/01" Then
                    fechaDesde = String.Empty
                End If
                If fechaHasta = "0001/01/01" Then
                    fechaHasta = String.Empty
                End If
            Else
                fechaDesde = String.Empty
                fechaHasta = String.Empty
            End If

            Dim objSolicitudesDB As New SolicitudesDB()
            Dim Urgente As String = "False"
            If Me.chk_Urgente.Checked Then Urgente = "True"

            Dim dsSolicitudes As DataSet = objSolicitudesDB.GetSolicitudesPorProveedor(cod_proveedor, cod_contrato, consultaPendietes, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue, Urgente, Session("DesdePagina"), Session("HastaPagina"), Session("IdPerfilColores"), txt_solicitud.Text)
            Session("RegistrosTotales") = objSolicitudesDB.ObtenerRegistrosTotales(cod_proveedor, consultaPendietes, cod_contrato, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, Urgente, ddl_Provincia.SelectedValue, Session("IdPerfilColores"), txt_solicitud.Text)

            'dsSolicitudes.Tables(0).Rows(0).Item(0)'Id Solicitud
            

            Session("dsSolicitudes") = dsSolicitudes

            gv_Solicitudes.Columns(0).Visible = True
            gv_Solicitudes.Columns(1).Visible = True
            gv_Solicitudes.Columns(2).Visible = True
            gv_Solicitudes.Columns(3).Visible = True
            gv_Solicitudes.Columns(4).Visible = True

            gv_Solicitudes.DataSource = dsSolicitudes
            gv_Solicitudes.DataBind()

            '' las oculto despues del databind para que me mantenga los datos
            'gv_Solicitudes.Columns(0).Visible = False
            'gv_Solicitudes.Columns(1).Visible = False
            gv_Solicitudes.Columns(2).Visible = False
            gv_Solicitudes.Columns(3).Visible = False
            gv_Solicitudes.Columns(4).Visible = False

            gv_Solicitudes.Columns(5).Visible = False

            If (cod_subtipo = "006" And cod_estado = "026") Or (cod_subtipo = "005" And cod_estado = "014") Then
                gv_Solicitudes.Columns(18).Visible = True
            Else
                gv_Solicitudes.Columns(18).Visible = False
            End If

            'Kintell 14/04/2009
            If Not UCase(txt_Proveedor.Text) = UCase("admin") Then
                gv_Solicitudes.Columns(17).Visible = False

                If Not UCase(txt_Proveedor.Text) = UCase("") Then
                    gv_Solicitudes.Columns(17).Visible = False

                    If Not UCase(txt_Proveedor.Text) = UCase("adico") Then gv_Solicitudes.Columns(17).Visible = False
                End If

            End If

            txt_Observaciones.Text = ""
            'gv_Solicitudes.ToolTip = Eval("Observaciones") ' gv_Solicitudes.Columns(15).ToString


        Catch ex As Exception
            Response.Write(ex.Message)
            gv_Solicitudes.DataSource = Nothing
            gv_Solicitudes.DataBind()

        End Try

    End Sub

    Private Sub CargaSolicitudesPorColores(ByVal IdPerfil As String)

        Try
            'Kintell 22/04/2009.
            IncializarControles()
            Session("IdPerfilColores") = IdPerfil

            Dim objProveedoresDB As New ProveedoresDB()
            Dim dr As SqlDataReader = objProveedoresDB.GetCodigoProveedorPorNombre(txt_Proveedor.Text)
            dr.Read()
            hidden_proveedor.Value = dr("proveedor").ToString()

            Dim cod_proveedor As String = hidden_proveedor.Value
            Dim cod_contrato As String = txt_contrato.Text
            Dim consultaPendientes As Boolean = chk_Pendientes.Checked
            Dim cod_tipo As String = ddl_Tipo.SelectedValue
            Dim cod_subtipo As String = ddl_Subtipo.SelectedValue
            Dim cod_estado As String = ddl_Estado.SelectedValue
            'Dim caldera As String = ddl_MarcaCaldera.Text

            Dim fechaDesde As String = cal_Desde.SelectedDate.ToString("yyyy/MM/dd") 'txtDesde.Text
            Dim fechaHasta As String = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd") 'txtHasta.Text

            If Not txtDesde.Text = "" And Not txtHasta.Text = "" Then
                txtDesde.Text = cal_Desde.SelectedDate.ToString("dd/MM/yyyy")
                'fechaHasta = txtHasta.Text
                If txtHasta.Text = "" Then txtHasta.Text = cal_Hasta.SelectedDate.ToString("dd/MM/yyyy")
                If txtHasta.Text = "01/01/0001" Then
                    txtHasta.Text = Mid(Now, 1, 10)
                Else
                    cal_Hasta.SelectedDate = txtHasta.Text

                    fechaHasta = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd")
                End If


                If fechaDesde = "0001/01/01" Then
                    fechaDesde = String.Empty
                End If
                If fechaHasta = "0001/01/01" Then
                    fechaHasta = String.Empty
                End If
            Else
                fechaDesde = String.Empty
                fechaHasta = String.Empty
            End If

            Dim objSolicitudesDB As New SolicitudesDB()
            Dim Urgente As String = "False"
            If Me.chk_Urgente.Checked Then Urgente = "True"

            '*********************************************************
            'Obtener registros totales.
            Session("NumeroRegistrosAVisualizar") = CInt(ConfigurationManager.AppSettings("NumeroRegistrosAVisualizar").ToString())
            Session("DesdePagina") = 0
            Session("HastaPagina") = CInt(Session("NumeroRegistrosAVisualizar")) '5

            'Tema Nivel de satisfacción, Kintell (05-01-2011).
            If UCase(txt_Proveedor.Text) = "ADMIN" Then txt_Proveedor.Text = "ADICO"
            '********************************************************
            Dim Proveedor As String = txt_Proveedor.Text
            If Not UCase(txt_Proveedor.Text) = "ADICO" Then Proveedor = Left(txt_Proveedor.Text, 3)
            Session("RegistrosTotales") = objSolicitudesDB.ObtenerRegistrosTotales(Proveedor, consultaPendientes, cod_contrato, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, Urgente, ddl_Provincia.SelectedValue, Session("IdPerfilColores"), txt_IdSolicitud.Text)


            lblTotal.Text = "Nº de Registros: " & Session("RegistrosTotales")

            
            If CInt(Session("RegistrosTotales")) > CInt(Session("NumeroRegistrosAVisualizar")) Then
                lblTotal0.Text = " Mostrando 1 hasta " & CInt(Session("NumeroRegistrosAVisualizar"))
                If CInt(Session("NumeroRegistrosAVisualizar")) < 1 Then lblTotal0.Text = " No hay registros"
                'ImageButton2.Visible = True
            Else
                lblTotal0.Text = " Mostrando 1 hasta " & CInt(Session("RegistrosTotales"))
                If CInt(Session("RegistrosTotales")) < 1 Then lblTotal0.Text = " No hay registros"
            End If

            Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
            Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
            Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

            If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

            lblPaginas.Text = "Nº de Páginas: " & NumeroPaginas
            Me.hiddenPaginacion.Value = "1"
            CrearPaginador(NumeroPaginas)
            '******************************************************


            Dim dsSolicitudes As DataSet = objSolicitudesDB.GetSolicitudesPorProveedor(cod_proveedor, cod_contrato, consultaPendientes, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue, Urgente, Session("DesdePagina"), Session("HastaPagina"), IdPerfil, txt_solicitud.Text)

            'dsSolicitudes.Tables(0).Rows(0).Item(0)'Id Solicitud


            Session("dsSolicitudes") = dsSolicitudes

            gv_Solicitudes.Columns(0).Visible = True
            gv_Solicitudes.Columns(1).Visible = True
            gv_Solicitudes.Columns(2).Visible = True
            gv_Solicitudes.Columns(3).Visible = True
            gv_Solicitudes.Columns(4).Visible = True

            gv_Solicitudes.DataSource = dsSolicitudes
            gv_Solicitudes.DataBind()

            '' las oculto despues del databind para que me mantenga los datos
            'gv_Solicitudes.Columns(0).Visible = False
            'gv_Solicitudes.Columns(1).Visible = False
            gv_Solicitudes.Columns(2).Visible = False
            gv_Solicitudes.Columns(3).Visible = False
            gv_Solicitudes.Columns(4).Visible = False

            gv_Solicitudes.Columns(5).Visible = False


            'Kintell 14/04/2009
            If Not UCase(txt_Proveedor.Text) = UCase("admin") Then
                gv_Solicitudes.Columns(17).Visible = False

                If Not UCase(txt_Proveedor.Text) = UCase("") Then
                    gv_Solicitudes.Columns(17).Visible = False

                    If Not UCase(txt_Proveedor.Text) = UCase("adico") Then gv_Solicitudes.Columns(17).Visible = False
                End If

            End If

            txt_Observaciones.Text = ""
            'gv_Solicitudes.ToolTip = Eval("Observaciones") ' gv_Solicitudes.Columns(15).ToString
        Catch ex As Exception

            gv_Solicitudes.DataSource = Nothing
            gv_Solicitudes.DataBind()

        End Try

    End Sub

    Private Function IncializarControles()
        On Error Resume Next

        btnCaldera.Visible = False
        chkUrgente.Visible = False
        txt_ModeloCaldera.Text = ""
        ddl_EstadoSol.Items.Clear()
        'ddl_EstadoSol.SelectedItem.Selected = False
        ddl_MarcaCaldera.SelectedItem.Selected = False
        txt_Suministro.Text = ""
        txt_Estado.Text = ""
        txt_Urgencia.Text = ""
        txt_IdSolicitud.Text = ""

        txt_FechaLimite.Text = ""

        txt_telfContacto.Text = ""
        txt_persContacto.Text = ""
        txt_ObservacionesAteriores.Text = ""
        txt_Servicio.Text = ""
        ddl_MotivoCancel.SelectedItem.Selected = False
    End Function

    Private Sub CargaDatosSolicitud(ByVal id_solicitud As String)

        Dim objSolicitudesDB As New SolicitudesDB()
        Dim dr As SqlDataReader = Nothing
        Me.txt_ModeloCaldera.Text = ""

        dr = objSolicitudesDB.GetSingleSolicitudes(id_solicitud, 1)
        dr.Read()


        '
        txtNombre.Text = dr("NOM_TITULAR").ToString() + " " + dr("APELLIDO1").ToString() + " " + dr("APELLIDO2").ToString()
        'txt_Suministro.Text = dr("TIP_VIA_PUBLICA").ToString() + " " + dr("NOM_CALLE").ToString() + " " + dr("COD_PORTAL").ToString() + " " + dr("TIP_ESCALERA").ToString() + " " + dr("TIP_MANO").ToString() + " - " + dr("NOM_POBLACION").ToString() + ", " + dr("COD_POSTAL").ToString() + ", " + dr("NOM_PROVINCIA").ToString()
        txt_Suministro.Text = dr("TIP_VIA_PUBLICA").ToString() + " " + dr("NOM_CALLE").ToString() + " " + dr("COD_PORTAL").ToString() + ", " + dr("TIP_PISO").ToString() + " " + dr("TIP_MANO").ToString() + ", " + dr("COD_POSTAL").ToString() + ", " + dr("NOM_POBLACION").ToString() + ", " + dr("NOM_PROVINCIA").ToString()
        txt_Estado.Text = dr("Des_Estado_solicitud").ToString()
        txt_Urgencia.Text = dr("Urgencia").ToString()
        txt_IdSolicitud.Text = dr("Id_Solicitud").ToString()

        'Mostrar aviso 24/05/2012. Kintell
        lblAviso.Text = dr("visita_aviso").ToString().Replace(";#", Chr(13))
        If Not lblAviso.Text = "" Then
            Dim Mostrar As String = "<script type='text/javascript'>Mostrar();</script>"

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AVISO", Mostrar, False)
            ImageButton4.Visible = True
        Else
            ImageButton4.Visible = False
        End If
        '*****************

        txtContrato.Text = gv_Solicitudes.SelectedRow.Cells(6).Text.ToString()


        Try
            txt_FechaLimite.Text = DateTime.Parse(dr("FEC_LIMITE_VISITA").ToString()).ToShortDateString()
        Catch ex As Exception
            txt_FechaLimite.Text = dr("FEC_LIMITE_VISITA").ToString()
        End Try

        txt_telfContacto.Text = dr("telefono_contacto").ToString()
        txt_persContacto.Text = dr("Persona_contacto").ToString()
        txt_ObservacionesAteriores.Text = dr("Observaciones").ToString()
        'txt_Observaciones.Text = dr("Observaciones").ToString()

        Dim servicio As String
        Dim T1 As String = String.Empty
        Dim T2 As String = String.Empty
        Dim T5 As String = String.Empty

        If dr("T1").ToString() = "S" Then
            T1 = "Servicio Mantenimiento Gas Calefacción"
        End If

        If dr("T2").ToString() = "S" Then
            T2 = "Servicio Mantenimiento Gas"
        End If

        If dr("T5").ToString() = "S" Then
            T5 = "con pago fraccionado"
        End If

        servicio = T1 + T2 + " " + T5
        txt_Servicio.Text = servicio

        CargaComboEstadosFuturos(dr("Tipo_solicitud").ToString(), dr("Subtipo_solicitud").ToString(), dr("Estado_solicitud").ToString(), dr("Des_Estado_solicitud").ToString())

        If (Not ddl_MotivoCancel.SelectedItem Is Nothing) Then
            ddl_MotivoCancel.SelectedItem.Selected = False
        End If
        Try
            ddl_MotivoCancel.Items.FindByValue(dr("Mot_cancel").ToString()).Selected = True
        Catch ex As Exception
        End Try

        ValidaComboMotiviCancelacion(dr("Estado_solicitud").ToString())
        ' GGB
        'If dr("Estado_solicitud").ToString() = "012" Then
        '    ddl_MotivoCancel.Enabled = False
        'End If

        '''' despues de cargar los datos cargamos las caracteristicas
        If Session("IdPerfil") <> "3" Then
            CargaCaracteristicasSolicitud(id_solicitud, dr("Subtipo_solicitud").ToString())
        Else
            'CargarCarateristicasTelefono(id_solicitud)
            ddl_MarcaCaldera.Enabled = False
            'ddl_EstadoSol.Enabled = False
            txt_ModeloCaldera.Enabled = False
            'txt_Observaciones.Enabled = False
        End If



        'Kintell 07/04/2009
        CargaComboCalderas()
        btnCaldera.Visible = True
        Session("CodigoContrato") = gv_Solicitudes.SelectedRow.Cells(2).Text.ToString()
        CargaHistorico(id_solicitud)

        'Kintell. 14/04/2009
        'Ponermos si es urgente o no la solicitud.
        Me.chkUrgente.Checked = dr("URGENTE").ToString()
        Me.chkUrgente.Enabled = False
        Me.chkUrgente.Visible = True


        'CargaHistoricoCaracteristicas()

        'Kintell 22/11/2011 Tema HORARIO CONTACTO.
        Dim db As SolicitudesDB = New SolicitudesDB()
        Dim Contra As String = txtContrato.Text
        Dim datos As DataSet = db.ObtenerDatosDesdeSentencia("SELECT top 1 HORARIO_CONTACTO FROM MANTENIMIENTO WHERE COD_CONTRATO_SIC='" & Contra & "'")
        If datos.Tables(0).Rows.Count > 0 Then
            cmbHorarioContacto.SelectedValue = datos.Tables(0).Rows(0).Item(0)
        End If

    End Sub


    Private Sub CargarCarateristicasTelefono(ByVal id_solicitud As String)
        Dim objCaracteristicasDB As New CaracteristicasDB()
        Dim dsCaracteristicas As DataSet = objCaracteristicasDB.GetCaracteristicasSolicitudTelefono(id_solicitud)

        Dim dsCaracteristicasAntiguas As DataSet = dsCaracteristicas.Copy()

        Session("dsCaracteristicas") = dsCaracteristicas
        Session("dsCaracteristicasAntiguas") = dsCaracteristicasAntiguas

        gv_Caracteristicas.Columns(0).Visible = True

        gv_Caracteristicas.DataSource = dsCaracteristicas
        gv_Caracteristicas.DataBind()

        ' las oculto despues del databind para que me mantenga los datos
        'gv_Caracteristicas.Columns(0).Visible = False
        gv_Caracteristicas.Columns(1).Visible = False

        gv_Caracteristicas.Columns(2).ItemStyle.Font.Size = 0
        gv_Caracteristicas.Columns(2).ShowHeader = False

        If dsCaracteristicas.Tables(0).Rows.Count = 0 Then
            CargaCaracteristicasPorTipoSolicitudTelefono()
        End If
    End Sub

    Private Sub CargaCaracteristicasPorTipoSolicitudTelefono()

        Dim objCaracteristicasDB As New CaracteristicasDB()
        Dim dsCaracteristicas As DataSet = objCaracteristicasDB.GetCaracteristicasPorTipoTelefono()

        Session("dsCaracteristicas") = dsCaracteristicas

        gv_Caracteristicas.Columns(0).Visible = True

        gv_Caracteristicas.DataSource = dsCaracteristicas
        gv_Caracteristicas.DataBind()

        ' las oculto despues del databind para que me mantenga los datos
        'gv_Caracteristicas.Columns(0).Visible = False


    End Sub






    Private Sub CargaCaracteristicasSolicitud(ByVal id_solicitud As String, ByVal Subtipo As String)

        Dim objCaracteristicasDB As New CaracteristicasDB()
        Dim dsCaracteristicas As DataSet = objCaracteristicasDB.GetCaracteristicasSolicitud(id_solicitud, 1)

        Dim dsCaracteristicasAntiguas As DataSet = dsCaracteristicas.Copy()

        Session("dsCaracteristicas") = dsCaracteristicas
        Session("dsCaracteristicasAntiguas") = dsCaracteristicasAntiguas

        gv_Caracteristicas.Columns(0).Visible = True


        'If Subtipo = "006" Then
        '    Dim Row As DataRow
        '    Row = dsCaracteristicas.Tables(0).NewRow()
        '    Row.Item(1) = "Fecha última visita"
        '    Row.Item(2) = "01/01/2011"

        '    dsCaracteristicas.Tables(0).Rows.Add(Row)
        'End If



        gv_Caracteristicas.DataSource = dsCaracteristicas
        gv_Caracteristicas.DataBind()

        'If Subtipo = "006" Then
        '    gv_Caracteristicas.Rows(2).Enabled = False
        'End If
        

        ' las oculto despues del databind para que me mantenga los datos
        'gv_Caracteristicas.Columns(0).Visible = False
        gv_Caracteristicas.Columns(1).Visible = False

        gv_Caracteristicas.Columns(2).ItemStyle.Font.Size = 0
        gv_Caracteristicas.Columns(2).ShowHeader = False



    End Sub

    'Private Sub CargaCaracteristicasPorTipoSolicitud(ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String)

    '    Dim objCaracteristicasDB As New CaracteristicasDB()
    '    'Dim dsCaracteristicas As DataSet = objCaracteristicasDB.GetCaracteristicasPorTipo(cod_tipo, cod_subtipo, cod_estado, 1)

    '    Session("dsCaracteristicas") = dsCaracteristicas

    '    gv_Caracteristicas.Columns(0).Visible = True

    '    gv_Caracteristicas.DataSource = dsCaracteristicas
    '    gv_Caracteristicas.DataBind()

    '    ' las oculto despues del databind para que me mantenga los datos
    '    'gv_Caracteristicas.Columns(0).Visible = False


    'End Sub

    Private Sub CargaHistorico(ByVal id_solicitud As String)

        Dim objHistoricoDB As New HistoricoDB()
        Dim dsHistorico As DataSet = objHistoricoDB.GetHistoricoSolicitud(id_solicitud, 1)

        gv_HistoricoSolicitudes.Columns(0).Visible = True
        gv_HistoricoSolicitudes.Columns(1).Visible = True

        gv_HistoricoSolicitudes.DataSource = dsHistorico
        gv_HistoricoSolicitudes.DataBind()

        '' las oculto despues del databind para que me mantenga los datos
        gv_HistoricoSolicitudes.Columns(0).Visible = False
        gv_HistoricoSolicitudes.Columns(1).Visible = False




    End Sub
    Private Sub CargarComboProvincias()
        ddl_Provincia.Items.Clear()

        Dim objTipoSolicitudesDB As New TiposSolicitudDB()


        Dim objProveedoresDB As New ProveedoresDB()
        Dim dr As SqlDataReader = objProveedoresDB.GetCodigoProveedorPorNombre(txt_Proveedor.Text)
        dr.Read()
        hidden_proveedor.Value = dr("proveedor").ToString()

        Dim cod_proveedor As String = hidden_proveedor.Value




        ddl_Provincia.DataSource = objTipoSolicitudesDB.GetProvincias(cod_proveedor, 1)
        ddl_Provincia.DataTextField = "NOM_PROVINCIA"
        ddl_Provincia.DataValueField = "COD_PROVINCIA"
        ddl_Provincia.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = ""
        defaultItem.Text = "TODAS" '"Seleccione un tipo de solicitud"
        ddl_Provincia.Items.Insert(0, defaultItem)
    End Sub
    Private Sub CargaComboTipoSolicitud()

        ddl_Tipo.Items.Clear()

        Dim objTipoSolicitudesDB As New TiposSolicitudDB()

        ddl_Tipo.DataSource = objTipoSolicitudesDB.GetTipoSolicitudes()
        ddl_Tipo.DataTextField = "descripcion"
        ddl_Tipo.DataValueField = "codigo"
        ddl_Tipo.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "TODOS" '"Seleccione un tipo de solicitud"
        ddl_Tipo.Items.Insert(0, defaultItem)

    End Sub

    Private Sub CargaComboSubtipoSolicitud(ByVal cod_tipo_solicitud As String)

        ddl_Subtipo.Items.Clear()

        Dim objTipoSolicitudesDB As New TiposSolicitudDB()

        ddl_Subtipo.DataSource = objTipoSolicitudesDB.GetSubtipoSolicitudesPorTipo(cod_tipo_solicitud, 1)
        ddl_Subtipo.DataTextField = "descripcion"
        ddl_Subtipo.DataValueField = "codigo"
        ddl_Subtipo.DataBind()

        'Kintell 21/06/2011
        'revisión por precinte
        Dim RevisionPrecinte As New ListItem()
        RevisionPrecinte.Value = "005"
        RevisionPrecinte.Text = "Revisión por Precinte"
        ddl_Subtipo.Items.Add(RevisionPrecinte)

        Dim VisitaSupervision As New ListItem()
        VisitaSupervision.Value = "006"
        VisitaSupervision.Text = "Visita supervision"
        ddl_Subtipo.Items.Add(VisitaSupervision)

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "TODOS" '"Seleccione un subtipo de solicitud"
        ddl_Subtipo.Items.Insert(0, defaultItem)

    End Sub
    Private Sub CargaComboSubtipoSolicitudTodas()

        ddl_Subtipo.Items.Clear()

        Dim objTipoSolicitudesDB As New TiposSolicitudDB()

        ddl_Subtipo.DataSource = objTipoSolicitudesDB.GetSubtipoSolicitudes(1)
        ddl_Subtipo.DataTextField = "descripcion"
        ddl_Subtipo.DataValueField = "codigo"
        ddl_Subtipo.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "TODOS" '"Seleccione un subtipo de solicitud"
        ddl_Subtipo.Items.Insert(0, defaultItem)

    End Sub


    Private Sub CargaComboEstadoSolicitud(ByVal cod_tipo As String, ByVal cod_subtipo As String)
        Dim objEstadoSolicitudesDB As New EstadosSolicitudDB()

        ddl_Estado.Items.Clear()
        ddl_Estado.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudPorTipoSubtipo(cod_tipo, cod_subtipo, 1)
        ddl_Estado.DataTextField = "descripcion"
        ddl_Estado.DataValueField = "codigo"
        ddl_Estado.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "TODOS" '"Seleccione un estado de solicitud"
        ddl_Estado.Items.Insert(0, defaultItem)
    End Sub

    Private Sub CargaComboEstadosFuturos(ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal des_estado As String)
        '********************************
        ddl_EstadoSol.Enabled = True
        Me.gv_Caracteristicas.Enabled = True
        txt_Observaciones.Enabled = True
        '********************************

        Dim objEstadoSolicitudesDB As New EstadosSolicitudDB()

        ddl_EstadoSol.Items.Clear()
        ddl_EstadoSol.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudFuturos(cod_tipo, cod_subtipo, cod_estado, 2, 1)
        ddl_EstadoSol.DataTextField = "descripcion"
        ddl_EstadoSol.DataValueField = "codigo"
        ddl_EstadoSol.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = des_estado
        ddl_EstadoSol.Items.Insert(0, defaultItem)


        'Kintell 06/04/2009
        'ESTADOS FINALES!!!!!!!!
        'If UCase(defaultItem.Text) = UCase("reparado") Or UCase(defaultItem.Text) = UCase("reparada") Or UCase(defaultItem.Text) = UCase("Cancelada") Or UCase(defaultItem.Text) = UCase("Reparada por telefono") Then
        If UCase(defaultItem.Text) = UCase("Cancelado transferido a SAT") Or UCase(defaultItem.Text) = UCase("Cancelada por Reasignacion") Or UCase(defaultItem.Text) = UCase("Reparada con documentacion") Or UCase(defaultItem.Text) = UCase("Cancelada") Or UCase(defaultItem.Text) = UCase("Reparada por telefono") Then
            ddl_EstadoSol.Enabled = False
            Me.gv_Caracteristicas.Enabled = False
            Me.ddl_MotivoCancel.Enabled = False
            txt_Observaciones.Enabled = False

            Me.ddl_MarcaCaldera.Enabled = False
            Me.txt_ModeloCaldera.Enabled = False
        ElseIf UCase(defaultItem.Text) = UCase("reparada") Then
            Me.gv_Caracteristicas.Enabled = False
            Me.ddl_MotivoCancel.Enabled = False
            txt_Observaciones.Enabled = False

            Me.ddl_MarcaCaldera.Enabled = False
            Me.txt_ModeloCaldera.Enabled = False
        End If
    End Sub



    Private Sub CargaComboMotivosCancelacion(ByVal Subtipo As String)

        ddl_MotivoCancel.Items.Clear()

        Dim objMotivosCancelacionDB As New MotivosCancelacionDB()
        'Kintell 12/12/2011
        ddl_MotivoCancel.DataSource = objMotivosCancelacionDB.GetMotivosCancelacion(Subtipo, 1)
        ddl_MotivoCancel.DataTextField = "descripcion"
        ddl_MotivoCancel.DataValueField = "cod_mot"
        ddl_MotivoCancel.DataBind()

        Dim defaultItem As New ListItem
        'defaultItem.Value = "-1"
        'defaultItem.Text = "Ninguno"
        'ddl_MotivoCancel.Items.Insert(0, defaultItem)


        '' por defecto esta desactivado
        ddl_MotivoCancel.Enabled = False

    End Sub

    Private Sub CargaComboCalderas()

        ddl_MarcaCaldera.Items.Clear()

        Dim objCalderasDB As New CalderasDB()

        ddl_MarcaCaldera.DataSource = objCalderasDB.GetMarcaCalderas()
        ddl_MarcaCaldera.DataTextField = "descripcion"
        ddl_MarcaCaldera.DataValueField = "cod_marca"
        ddl_MarcaCaldera.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "Seleccione una marca de caldera"
        ddl_MarcaCaldera.Items.Insert(0, defaultItem)



        Dim ds As New DataSet
        Dim cod_contrato As String = gv_Solicitudes.SelectedRow.Cells(2).Text.ToString()
        Dim i As Integer = 0
        Dim Esta As Boolean = False

        ds = objCalderasDB.GetCalderasPorContrato(cod_contrato)
        If Not ds.Tables(0).Rows.Count = 0 Then
            For i = 0 To ddl_MarcaCaldera.Items.Count - 1
                If UCase(ddl_MarcaCaldera.Items(i).Text) = UCase(ds.Tables(0).Rows(0).Item(4)) Then
                    Esta = True
                End If
            Next
            If Not Esta Then
                defaultItem = New ListItem
                defaultItem.Value = ds.Tables(0).Rows(0).Item(2)
                defaultItem.Text = ds.Tables(0).Rows(0).Item(3)
                ddl_MarcaCaldera.Items.Insert(0, defaultItem)
            End If

            ddl_MarcaCaldera.SelectedValue = ds.Tables(0).Rows(0).Item(2)
            Me.txt_ModeloCaldera.Text = ds.Tables(0).Rows(0).Item(4)
        End If


    End Sub

    Private Function ExportarDatagrid(ByVal dtSolicitudes As DataTable) As String
        '***************
        Dim nombreExcel As String = "Solicitudes_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xls"
        Dim sb As New StringBuilder
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)
        Dim a As New DataGrid
        Dim Page As New Page
        Dim form As New HtmlForm


        a.DataSource = dtSolicitudes
        a.DataBind()

        a.RenderControl(htw)

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=fichero.xls")
        HttpContext.Current.Response.Charset = "UTF-8"
        HttpContext.Current.Response.ContentEncoding = Encoding.Default
        HttpContext.Current.Response.Write("SOLICITUDES")
        HttpContext.Current.Response.Write(sb.ToString())
        HttpContext.Current.Response.End()

        Return nombreExcel
    End Function
    'Private Function ExportarExcel(ByVal dtSolicitudes As DataTable) As String
    '    Try
    '        Dim rutaExcelFinal As String = String.Empty
    '        Dim nombreExcel As String = "Solicitudes_" + DateTime.Now.ToString("ddmmyy_hhmm") + ".xls"

    '        If Not dtSolicitudes Is Nothing And dtSolicitudes.Rows.Count > 0 Then
    '            ErrorLog(Server.MapPath("Logs/ErrorLog"), "Crear Excel.")
    '            Dim apliExcel As New Excel.Application()

    '            Dim missing As Object = Type.Missing

    '            Dim libro As Excel.Workbook = apliExcel.Workbooks.Add(missing)

    '            Dim hoja1 As New Excel.Worksheet()

    '            hoja1 = CType(libro.Sheets.Add(missing, missing, missing, missing), Excel.Worksheet)


    '            '' Rellenamos la hoja con los datos de la tabla
    '            ErrorLog(Server.MapPath("Logs/ErrorLog"), "Rellenar Datos.")
    '            For i As Integer = 1 To dtSolicitudes.Rows.Count
    '                For u As Integer = 1 To dtSolicitudes.Columns.Count

    '                    Dim nombreColumna As String = dtSolicitudes.Columns(u - 1).ColumnName.ToString()

    '                    If i = 1 Then

    '                        Select Case (nombreColumna)

    '                            Case "ID_solicitud"
    '                                hoja1.Cells(i, u) = "ID Solicitud"
    '                            Case "Cod_contrato"
    '                                hoja1.Cells(i, u) = "Código de contrato"
    '                            Case "Des_Tipo_solicitud"
    '                                hoja1.Cells(i, u) = "Tipo"
    '                            Case "Des_subtipo_solicitud"
    '                                hoja1.Cells(i, u) = "Subtipo"
    '                            Case "Des_Estado_solicitud"
    '                                hoja1.Cells(i, u) = "Estado"
    '                            Case "Fecha_creacion"
    '                                hoja1.Cells(i, u) = "Fecha creación"
    '                            Case "telefono_contacto"
    '                                hoja1.Cells(i, u) = "Telefono contacto"
    '                            Case "Persona_contacto"
    '                                hoja1.Cells(i, u) = "Persona contacto"
    '                            Case "des_averia"
    '                                hoja1.Cells(i, u) = "Descripción averia"
    '                            Case "Observaciones"
    '                                hoja1.Cells(i, u) = "Observaciones"
    '                            Case "NOM_POBLACION"
    '                                hoja1.Cells(i, u) = "Poblacion"
    '                            Case "NOM_CALLE"
    '                                hoja1.Cells(i, u) = "Calle"
    '                            Case "COD_PORTAL"
    '                                hoja1.Cells(i, u) = "Portal"
    '                            Case "TIP_PISO"
    '                                hoja1.Cells(i, u) = "Piso"
    '                            Case "TIP_MANO"
    '                                hoja1.Cells(i, u) = "Mano"
    '                            Case "COD_POSTAL"
    '                                hoja1.Cells(i, u) = "Cod. Postal"
    '                        End Select

    '                        Dim celdaActiva As Excel.Range = CType(hoja1.Cells(i, u), Excel.Range)
    '                        celdaActiva.EntireRow.Font.Bold = True
    '                        celdaActiva.EntireRow.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White)
    '                        celdaActiva.EntireRow.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue)


    '                    End If

    '                    Select Case (nombreColumna)

    '                        Case "ID_solicitud"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "Cod_contrato"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "Des_Tipo_solicitud"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "Des_subtipo_solicitud"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "Des_Estado_solicitud"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "Fecha_creacion"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "telefono_contacto"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "Persona_contacto"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "des_averia"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "Observaciones"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "NOM_POBLACION"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "NOM_CALLE"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "COD_PORTAL"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "TIP_PISO"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "TIP_MANO"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                        Case "COD_POSTAL"
    '                            hoja1.Cells(i + 1, u) = dtSolicitudes.Rows(i - 1)(u - 1).ToString()
    '                    End Select

    '                Next
    '            Next

    '            ErrorLog(Server.MapPath("Logs/ErrorLog"), "A Guardar.")

    '            Dim rutaGuardarExcel As String = Server.MapPath(Request.ApplicationPath) & "\Pages\excel\" '"excel\" 'ConfigurationManager.AppSettings("rutaGuardarExcel").ToString()
    '            'Dim rutaLeerExcel As String = ConfigurationManager.AppSettings("rutaLeerExcel").ToString()
    '            Dim rutaExcelFinalObj As Object = rutaGuardarExcel & nombreExcel
    '            ErrorLog(Server.MapPath("Logs/ErrorLog"), rutaExcelFinalObj)
    '            apliExcel.Workbooks(1).SaveAs(rutaExcelFinalObj, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing)
    '            apliExcel.Workbooks.Close()
    '            apliExcel.Quit()

    '            '' si llega aqui esque se ha guardado sin dar error
    '            'rutaExcelFinal = rutaLeerExcel + nombreExcel
    '            rutaExcelFinal = rutaExcelFinalObj '"/excel/" + nombreExcel


    '        End If

    '        Return nombreExcel 'rutaExcelFinal
    '    Catch ex As Exception
    '        ErrorLog(Server.MapPath("Logs/ErrorLog"), ex.Message & "--" & ex.StackTrace)
    '    End Try
    'End Function


    Private Sub ValidaComboMotiviCancelacion(ByVal estadoSolicitud As String)
        Try

            ' GGB comento por petición de Ricardo

            'Dim solicitudCancelada As String() = {"009", "010", "011", "012", "013", "014", "015"}

            'If solicitudCancelada.Contains(estadoSolicitud) Then
            '    ddl_MotivoCancel.Enabled = True
            'Else
            '    If Not ddl_MotivoCancel.SelectedItem Is Nothing Then
            '        ddl_MotivoCancel.SelectedItem.Selected = False
            '    End If
            '    ddl_MotivoCancel.Items(0).Selected = True
            '    ddl_MotivoCancel.Enabled = False
            'End If

            ' GGB por petición de Ricardo 

            If ("012".Equals(estadoSolicitud) Or "025".Equals(estadoSolicitud)) Then
                ddl_MotivoCancel.Enabled = True
                'Cancelada Visita supervisión.
                If "025".Equals(estadoSolicitud) Then
                    'Cliente no desea visita.

                    'Cliente ilocalizable.

                End If
            Else
                ddl_MotivoCancel.Enabled = False
            End If

            Dim li As ListItem = Nothing
            li = ddl_MotivoCancel.Items.FindByText("Seleccione el motivo de la cancelación")
            If (li Is Nothing) Then
                li = New ListItem
                li.Text = "Seleccione el motivo de la cancelación"
                li.Value = -1
                ddl_MotivoCancel.Items.Insert(0, li)
            End If

            If (ddl_MotivoCancel.SelectedItem Is Nothing) Then
                ddl_MotivoCancel.Items(0).Selected = True
            End If





        Catch ex As Exception
            '' por si acaso
        End Try


    End Sub

#End Region

#Region "Eventos"

    Protected Sub btn_ModificarSolicitud_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ModificarSolicitud.Click
        Try

            'Probando modificar varias filas
            'Probando()
            '********************************************************************
            If Not gv_Solicitudes.SelectedRow Is Nothing Then

                '*********************************************************
                'Kintell 07/04/2009.
                'Proceso de actualizar quitado del botón de "edit"

                Dim dsCaracteristicas As DataSet = CType(Session("dsCaracteristicas"), DataSet)
                Dim test As String = ""
                If gv_Caracteristicas.Rows.Count > 0 Then
                    For i = 0 To gv_Caracteristicas.Rows.Count - 1
                        Dim pos As Integer = i
                        If Not gv_Caracteristicas.SelectedIndex = -1 Then pos = gv_Caracteristicas.SelectedIndex
                        If Not IsNothing(gv_Caracteristicas.Rows(pos).Cells(2).FindControl("lbl_valor")) Then

                            test = CType(gv_Caracteristicas.Rows(pos).Cells(2).FindControl("lbl_valor"), System.Web.UI.WebControls.Label).Text
                            'If InStr(UCase(gv_Caracteristicas.Rows(pos).Cells(3).Text), UCase("fecha")) > 0 Then
                            '    Dim d As Date
                            '    If Not Date.TryParseExact(test, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, d) Then
                            '        Dim Script As String
                            '        Script = "<script language='JavaScript'>alert('Compruebe la fecha introducida.');</script>"
                            '        Page.RegisterStartupScript("Error Fecha", Script)
                            '        Exit Sub
                            '    End If
                            'End If


                            dsCaracteristicas.Tables(0).Rows(pos)("valor") = test
                            Session("dsCaracteristicas") = dsCaracteristicas
                            gv_Caracteristicas.Columns(0).Visible = True
                            gv_Caracteristicas.Columns(1).Visible = True
                            gv_Caracteristicas.EditIndex = -1
                            gv_Caracteristicas.DataSource = dsCaracteristicas
                            gv_Caracteristicas.DataBind()
                            'gv_Caracteristicas.Columns(0).Visible = False
                            gv_Caracteristicas.Columns(1).Visible = False
                        End If
                    Next
                End If
                '*********************************************************


                Dim id_solicitud As String = gv_Solicitudes.SelectedRow.Cells(1).Text.ToString()
                Dim cod_contrato As String = gv_Solicitudes.SelectedRow.Cells(2).Text.ToString()
                Dim cod_estado As String = gv_Solicitudes.SelectedRow.Cells(5).Text.ToString()
                Dim usuario As String = String.Empty

                Dim objSolicitudesDB As New SolicitudesDB()
                Dim objCaracteristicasDB As New CaracteristicasDB()
                Dim objHistoricoDB As New HistoricoDB()
                Dim row As GridViewRow
                Dim dtRow As DataRow
                Dim valor As String = String.Empty
                Dim tip_car As String = String.Empty
                Dim id_caracteristica As String = String.Empty

                Dim caracteristicasRellenas As Boolean = True
                Dim valorAntiguo As String = String.Empty

                'Comprobamos k si es cancelada haya seleccionado un motivo de cancelación.
                If ddl_EstadoSol.SelectedItem.Text = "Cancelada" Then
                    If Me.ddl_MotivoCancel.SelectedValue = "-1" Then
                        Dim Script As String
                        Script = "<script language='JavaScript'>alert('Debe seleccionar un motivo de cancelación.');</script>"
                        Page.RegisterStartupScript("Error Fecha", Script)
                        Exit Sub
                    End If
                End If


                If Not ddl_EstadoSol.SelectedValue = "-1" And gv_Caracteristicas.Rows.Count > 0 Then
                    ''si cambia el estado, primero recorremos las caracteristicas para ver si estan rellenas
                    For Each row In gv_Caracteristicas.Rows
                        valor = CType(row.FindControl("lbl_Valor"), System.Web.UI.WebControls.Label).Text
                        If String.IsNullOrEmpty(valor) Then
                            caracteristicasRellenas = False
                            lbl_mensajes.Text = "Se deben rellenar todas las características."
                        End If
                    Next
                End If
                Dim Observaciones As String
                Dim Horas As String = DateTime.Now.Hour.ToString()
                If Len(Horas) = 1 Then Horas = "0" & Horas
                Dim Minutos As String = DateTime.Now.Minute.ToString()
                If Len(Minutos) = 1 Then Minutos = "0" & Minutos
                usuario = Session("usuarioValido").ToString()
                Observaciones = "[" & Mid(Now(), 1, 10) & "-" & Horas & ":" & Minutos & "] " & usuario & ": " & txt_Observaciones.Text & Chr(13) & txt_ObservacionesAteriores.Text




                ' si todas estan rellenas
                If caracteristicasRellenas Then

                    'Modificamos el estado.
                    If Not ddl_EstadoSol.SelectedValue = "-1" Then
                        'cambiamos el estado
                        cod_estado = ddl_EstadoSol.SelectedValue


                        'objSolicitudesDB.UpdateEstadoSolicitud(id_solicitud, cod_estado)

                        'modificadoEstado = True

                    End If


                    Try

                        'Dim contrato As String = cod_contrato
                        'Dim cod_tipo_solicitud As String = ddl_Tipo.SelectedValue
                        'Dim cod_subtipo_solicitud As String = ddl_Subtipo.SelectedValue
                        'Dim telef_contacto As String = txt_telfContacto.Text
                        'Dim pers_contacto As String = txt_persContacto.Text
                        'Dim proveedor As String = hidden_proveedor.Value
                        Dim mot_cancel As String = ddl_MotivoCancel.SelectedValue

                        objSolicitudesDB.UpdateSolicitudDesdeProveedor(id_solicitud, mot_cancel)

                    Catch ex As Exception

                    End Try
                    'grabamos el historico
                    Dim id_movimiento As Integer = objHistoricoDB.AddHistoricoSolicitud(id_solicitud, "002", usuario, cod_estado, Observaciones, "")

                    'Kintell(7 / 4 / 2009)
                    If Not txt_Observaciones.Text = "" Then objSolicitudesDB.UpdateObservacionesSolicitud(id_solicitud, Observaciones)

                    'Comprobamos k existe o no una caldera para este contrato, para insertar una nueva o modificar la existente.
                    Dim objCalderasDB As New CalderasDB()

                    Dim ds As New DataSet
                    ds = objCalderasDB.GetCalderasPorContrato(cod_contrato)
                    If ds.Tables(0).Rows.Count = 0 Then
                        objSolicitudesDB.InsertarCalderaContrato(cod_contrato, Me.ddl_MarcaCaldera.SelectedValue.ToString, txt_ModeloCaldera.Text)
                    Else
                        objSolicitudesDB.ActualizarCalderaContrato(cod_contrato, Me.ddl_MarcaCaldera.SelectedValue.ToString, txt_ModeloCaldera.Text, "", "", 0, 0)
                    End If



                    'If Not gv_Caracteristicas.Rows.Count = 0 Then
                    'Borramos las Caracerísticas.
                    Dim Sql As String = ""
                    Dim myDataSet As New DataSet
                    Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))

                    'If Session("IdPerfil") <> "3" Then
                    Sql = "Update caracteristicas_solicitud set Activo=0 where id_solicitud=" & id_solicitud
                    Dim myCommand As New SqlCommand(Sql, myConnection)
                    myConnection.Open()
                    Try
                        myCommand.ExecuteNonQuery()
                        myConnection.Close()
                    Catch ex As Exception
                    Finally
                        If (myConnection.State = ConnectionState.Open) Then
                            myConnection.Close()
                        End If
                    End Try
                    'End If
                    'End If
                    ' si estan rellenas o si son para añadir nuevas, nos recorremos el gridview.
                    For Each row In gv_Caracteristicas.Rows
                        id_caracteristica = row.Cells(1).Text
                        tip_car = row.Cells(2).Text

                        valor = CType(row.FindControl("lbl_Valor"), System.Web.UI.WebControls.Label).Text
                        'CType(row.FindControl("lbl_Valor"), System.Web.UI.WebControls.Label).Text
                        If Not valor = "" Then

                            If Session("IdPerfil") <> "3" Then
                                If Not ddl_EstadoSol.SelectedValue = "-1" Then
                                    ' si son nuevas añadimos
                                    id_caracteristica = objCaracteristicasDB.AddCaracteristica(id_solicitud, tip_car, valor, Now.ToShortDateString())
                                    objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica, id_movimiento, tip_car, valor)
                                    'id_movimiento = id_movimiento + 1
                                Else
                                    ' si son antiguas recuperamos las antiguas para ver cuales se han modificado
                                    Dim dsCaracteristicasAntiguas As DataSet = Session("dsCaracteristicasAntiguas")

                                    If Not dsCaracteristicasAntiguas Is Nothing And dsCaracteristicasAntiguas.Tables.Count > 0 Then

                                        ' nos recorremos la tabla de las antiguas
                                        For Each dtRow In dsCaracteristicasAntiguas.Tables(0).Rows

                                            ' comprobamso que sea la misma caracteristica
                                            If id_caracteristica = dtRow("id_caracteristica").ToString() Then

                                                ' si el valor es distinto esque se ha modificado
                                                If valor <> dtRow("valor").ToString() Then

                                                    objCaracteristicasDB.UpdateCaracteristica(id_caracteristica, id_solicitud, tip_car, valor, Now.ToShortDateString())
                                                    objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica, id_movimiento, tip_car, valor)
                                                    'id_movimiento = id_movimiento + 1
                                                End If

                                                ' si es la misma nos salimos y no seguimos recorriendo las antiguas
                                                Exit For

                                            End If

                                        Next

                                    End If
                                End If
                            Else
                                Dim dsCaracteristicasAntiguas As DataSet = Session("dsCaracteristicasAntiguas")
                                'If dsCaracteristicasAntiguas.Tables(0).Rows.Count = 0 Then
                                ' si son nuevas añadimos
                                id_caracteristica = objCaracteristicasDB.AddCaracteristica(id_solicitud, tip_car, valor, Now.ToShortDateString())
                                objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica, id_movimiento, tip_car, valor)
                                'Else
                                'For Each dtRow In dsCaracteristicasAntiguas.Tables(0).Rows
                                '    ' comprobamso que sea la misma caracteristica
                                '    If id_caracteristica = dtRow("id_caracteristica").ToString() Then

                                '        ' si el valor es distinto esque se ha modificado
                                '        If valor <> dtRow("valor").ToString() Then

                                '            objCaracteristicasDB.UpdateCaracteristica(id_caracteristica, id_solicitud, tip_car, valor, Now.ToShortDateString())
                                '            objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica, id_movimiento, tip_car, valor)
                                '            'id_movimiento = id_movimiento + 1
                                '        End If

                                '        ' si es la misma nos salimos y no seguimos recorriendo las antiguas
                                '        Exit For

                                '    End If

                                'Next
                                'End If
                            End If


                        End If

                    Next

                    ClientScript.RegisterStartupScript(Page.GetType(), "PopupScript", "<script language='javascript'>alert('Se ha modificado la solicitud');</script>")



                    'Kintell 22/11/2011 Tema HORARIO CONTACTO.
                    ActualizarHorariocontacto("UPDATE MANTENIMIENTO SET HORARIO_CONTACTO='" & cmbHorarioContacto.SelectedValue & "' WHERE COD_CONTRATO_SIC='" & txtContrato.Text & "'")



                    CargaSolicitudes()
                    CargaDatosSolicitud(id_solicitud)

                End If


            End If

        Catch ex As Exception
            lbl_mensajes.Text = "No se ha podido modificar la solicitud."
        Finally
            'Response.Redirect("Proveedores.aspx#PosicionModificar")
        End Try

    End Sub

    Private Sub ActualizarHorariocontacto(ByVal Sql As String)
        Dim Db As SolicitudesDB = New SolicitudesDB()
        Db.EjecutarSentencia(Sql)
    End Sub

    Protected Sub ddl_Tipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Tipo.SelectedIndexChanged

        CargaComboSubtipoSolicitud(ddl_Tipo.SelectedValue)
        CargaComboEstadoSolicitud(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue)

        gv_Solicitudes.DataSource = Nothing
        gv_Solicitudes.DataBind()

        lblBuscar.Visible = True
        lblTotal.Visible = False
        lblTotal0.Visible = False
        lblPaginas.Visible = False
        lblPaginaActual.Visible = False

        'Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        'Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        'Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        'If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        'CrearPaginador(NumeroPaginas)

        'CargaSolicitudes()

    End Sub

    Protected Sub ddl_Subtipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Subtipo.SelectedIndexChanged

        CargaComboEstadoSolicitud(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue)


        gv_Solicitudes.DataSource = Nothing
        gv_Solicitudes.DataBind()
        lblBuscar.Visible = True
        lblTotal.Visible = False
        lblTotal0.Visible = False
        lblPaginas.Visible = False
        lblPaginaActual.Visible = False
        'Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        'Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        'Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        'If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        'CrearPaginador(NumeroPaginas)

        ' ''CargaSolicitudes()

    End Sub


    Protected Sub ddl_Estado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Estado.SelectedIndexChanged
        'CargaSolicitudes()

        gv_Solicitudes.DataSource = Nothing
        gv_Solicitudes.DataBind()
        lblBuscar.Visible = True
        lblTotal.Visible = False
        lblTotal0.Visible = False
        lblPaginas.Visible = False
        lblPaginaActual.Visible = False
        'Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        'Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        'Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        'If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        'CrearPaginador(NumeroPaginas)
    End Sub

    Protected Sub ddl_EstadoSol_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_EstadoSol.SelectedIndexChanged
        Try
            ''If UCase(ddl_EstadoSol.SelectedItem.Text) = UCase("Reparada con documentacion") And Session("IdPerfil") <> "6" Then
            'gv_Caracteristicas.Enabled = True
            ''End If
            'Dim cod_tipo As String = gv_Solicitudes.SelectedRow.Cells(3).Text.ToString()
            'Dim cod_subtipo As String = gv_Solicitudes.SelectedRow.Cells(4).Text.ToString()
            'Dim cod_estado As String = ddl_EstadoSol.SelectedValue


            'ValidaComboMotiviCancelacion(cod_estado)

            'CargaCaracteristicasPorTipoSolicitud(cod_tipo, cod_subtipo, cod_estado)
            ''ViewState("dsCaracteristicas") = gv_Caracteristicas.DataSource
            ''If UCase(ddl_EstadoSol.SelectedItem.Text) = UCase("Reparada con documentacion") And Session("IdPerfil") = "6" Then
            ''    ''si cambia el estado, primero recorremos las caracteristicas para ver si estan rellenas
            ''    For Each row In gv_Caracteristicas.Rows
            ''        CType(row.FindControl("lbl_Valor"), System.Web.UI.WebControls.Label).Text = " "
            ''    Next
            ''End If
        Catch ex As Exception
            MostrarMensaje("Vuelva a seleccionar la solicitud en la busqueda")
        End Try

    End Sub


    'Protected Sub gv_Solicitudes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Solicitudes.PageIndexChanging

    '    Try


    '        Dim dsSolicitudes As DataSet = Session("dsSolicitudes")

    '        gv_Solicitudes.Columns(0).Visible = True
    '        gv_Solicitudes.Columns(1).Visible = True
    '        gv_Solicitudes.Columns(2).Visible = True
    '        gv_Solicitudes.Columns(3).Visible = True
    '        gv_Solicitudes.Columns(4).Visible = True

    '        gv_Solicitudes.PageIndex = e.NewPageIndex

    '        gv_Solicitudes.DataSource = dsSolicitudes
    '        gv_Solicitudes.DataBind()

    '        '' las oculto despues del databind para que me mantenga los datos
    '        gv_Solicitudes.Columns(0).Visible = False
    '        gv_Solicitudes.Columns(1).Visible = False
    '        gv_Solicitudes.Columns(2).Visible = False
    '        gv_Solicitudes.Columns(3).Visible = False
    '        gv_Solicitudes.Columns(4).Visible = False

    '    Catch ex As Exception
    '        lbl_mensajes.Text = "No se ha podido recargar el grid"
    '    End Try

    'End Sub

    Protected Sub gv_Solicitudes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not gv_Solicitudes.Rows.Count = 0 Then
            Dim id_solicitud As String = gv_Solicitudes.SelectedRow.Cells(1).Text.ToString()

            CargaDatosSolicitud(id_solicitud)
        End If

    End Sub




    Protected Sub gv_Caracteristicas_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gv_Caracteristicas.RowCancelingEdit

        'Dim dsCaracteristicas As DataSet = CType(Session("dsCaracteristicas"), DataSet)

        'gv_Caracteristicas.Columns(0).Visible = True
        'gv_Caracteristicas.Columns(1).Visible = True

        'gv_Caracteristicas.EditIndex = -1
        'gv_Caracteristicas.DataSource = dsCaracteristicas
        'gv_Caracteristicas.DataBind()

        ''gv_Caracteristicas.Columns(0).Visible = False
        'gv_Caracteristicas.Columns(1).Visible = False



    End Sub


    Protected Sub gv_Caracteristicas_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gv_Caracteristicas.RowUpdating
        'Dim id_solicitud As String = gv_Solicitudes.SelectedRow.Cells(1).Text.ToString()
        'Dim cod_contrato As String = gv_Solicitudes.SelectedRow.Cells(2).Text.ToString()
        'Dim cod_estado As String = gv_Solicitudes.SelectedRow.Cells(5).Text.ToString()
        'Dim usuario As String = String.Empty

        'Dim objSolicitudesDB As New SolicitudesDB()
        'Dim objCaracteristicasDB As New CaracteristicasDB()
        'Dim objHistoricoDB As New HistoricoDB()
        'Dim row As GridViewRow
        'Dim dtRow As DataRow
        'Dim valor As String = String.Empty
        'Dim tip_car As String = String.Empty
        'Dim id_caracteristica As String = String.Empty

        'Dim caracteristicasRellenas As Boolean = True
        'Dim valorAntiguo As String = String.Empty

        'Dim dsCaracteristicas As DataSet = CType(Session("dsCaracteristicas"), DataSet)





        'Dim test As String = ""
        'If gv_Caracteristicas.Rows.Count > 0 Then
        '    Dim pos As Integer = 0
        '    If Not gv_Caracteristicas.SelectedIndex = -1 Then pos = gv_Caracteristicas.SelectedIndex
        '    If Not IsNothing(gv_Caracteristicas.Rows(pos).Cells(2).FindControl("txt_ValorCaracteristica")) Then

        '        test = CType(gv_Caracteristicas.Rows(pos).Cells(2).FindControl("txt_ValorCaracteristica"), System.Web.UI.WebControls.TextBox).Text
        '        If InStr(UCase(gv_Caracteristicas.Rows(pos).Cells(3).Text), UCase("fecha")) > 0 Then
        '            Dim d As Date
        '            If Not Date.TryParseExact(test, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, d) Then
        '                Dim Script As String
        '                Script = "<script language='JavaScript'>Alert('Compruebe la fecha introducida.');</script>"
        '                Page.RegisterStartupScript("Error Fecha", Script)
        '                Exit Sub
        '            End If
        '        End If


        '        dsCaracteristicas.Tables(0).Rows(pos)("valor") = test
        '        Session("dsCaracteristicas") = dsCaracteristicas
        '        gv_Caracteristicas.Columns(0).Visible = True
        '        gv_Caracteristicas.Columns(1).Visible = True
        '        gv_Caracteristicas.EditIndex = -1
        '        gv_Caracteristicas.DataSource = dsCaracteristicas
        '        gv_Caracteristicas.DataBind()
        '        'gv_Caracteristicas.Columns(0).Visible = False
        '        gv_Caracteristicas.Columns(1).Visible = False
        '    End If
        'End If
        ''******************************************************************************************
        ''Comprobamos k el valor no viene en blanco.
        'Select Case e.RowIndex
        '    Case 0
        '        'Fecha.
        '        valor = CType(row.FindControl("lbl_Valor"), System.Web.UI.WebControls.Label).Text
        '        If String.IsNullOrEmpty(valor) Then
        '            caracteristicasRellenas = False
        '            lbl_mensajes.Text = "Se deben rellenar todas las caracteristicas."
        '            'Else
        '            'Session("CaracSeleccionada") = Session("CaracSeleccionada") & i & "-" & valor & ";"
        '        End If
        '    Case 1
        '        'Importe.
        '        valor = CType(row.FindControl("txt_ValorCaracteristica"), System.Web.UI.WebControls.TextBox).Text
        '        If String.IsNullOrEmpty(valor) Then
        '            caracteristicasRellenas = False
        '            lbl_mensajes.Text = "Se deben rellenar todas las caracteristicas."
        '            'Else
        '            'Session("CaracSeleccionada") = Session("CaracSeleccionada") & i & "-" & valor & ";"
        '        End If
        '    Case 2
        '        'Texto.
        '        valor = CType(row.FindControl("lbl_Valor"), System.Web.UI.WebControls.Label).Text
        '        If String.IsNullOrEmpty(valor) Then
        '            caracteristicasRellenas = False
        '            lbl_mensajes.Text = "Se deben rellenar todas las caracteristicas."
        '            'Else
        '            'Session("CaracSeleccionada") = Session("CaracSeleccionada") & i & "-" & valor & ";"
        '        End If
        'End Select




        '' si todas estan rellenas
        'If caracteristicasRellenas Then
        '    Dim Observaciones As String
        '    Observaciones = "[" & Mid(Now(), 1, 10) & "-" & Now.Hour & ":" & Now.Minute & "] " & usuario & ": " & txt_Observaciones.Text & Chr(13) & txt_ObservacionesAteriores.Text
        '    Dim id_movimiento As Integer = objHistoricoDB.AddHistoricoSolicitud(id_solicitud, "002", usuario, cod_estado, Observaciones)

        '    ' si estan rellenas o si son para añadir nuevas nos recorremos el gridview
        '    For Each row In gv_Caracteristicas.Rows
        '        id_caracteristica = row.Cells(1).Text
        '        tip_car = row.Cells(2).Text

        '        valor = test
        '        'CType(row.FindControl("lbl_Valor"), System.Web.UI.WebControls.Label).Text
        '        If Not valor = "" Then
        '            If Not ddl_EstadoSol.SelectedValue = "-1" Then
        '                ' si son nuevas añadimos
        '                id_caracteristica = objCaracteristicasDB.AddCaracteristica(id_solicitud, tip_car, valor, Now.ToShortDateString())
        '                objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica, id_movimiento, "", valor)
        '            Else
        '                ' si son antiguas recuperamos las antiguas para ver cuales se han modificado
        '                Dim dsCaracteristicasAntiguas As DataSet = Session("dsCaracteristicasAntiguas")

        '                If Not dsCaracteristicasAntiguas Is Nothing And dsCaracteristicasAntiguas.Tables.Count > 0 Then

        '                    ' nos recorremos la tabla de las antiguas
        '                    For Each dtRow In dsCaracteristicasAntiguas.Tables(0).Rows

        '                        ' comprobamso que sea la misma caracteristica
        '                        If id_caracteristica = dtRow("id_caracteristica").ToString() Then

        '                            ' si el valor es distinto esque se ha modificado
        '                            If valor <> dtRow("valor").ToString() Then
        '                                objCaracteristicasDB.UpdateCaracteristica(id_caracteristica, id_solicitud, tip_car, valor, Now.ToShortDateString())
        '                                objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica, id_movimiento, tip_car, valor)
        '                            End If

        '                            ' si es la misma nos salimos y no seguimos recorriendo las antiguas
        '                            Exit For

        '                        End If

        '                    Next

        '                End If
        '            End If
        '        End If

        '    Next

        'End If

        Try

        
            Dim test As String = CType(gv_Caracteristicas.Rows(e.RowIndex).Cells(2).FindControl("txt_ValorCaracteristica"), System.Web.UI.WebControls.TextBox).Text

            Dim dsCaracteristicas As DataSet = CType(Session("dsCaracteristicas"), DataSet)


            'Kintell 10/02/2010
            'La fecha sea mayor que la fecha creación y la diferencia menos de 90.
            Dim Fecha As DateTime
            Dim Correcto As Boolean = False
            Dim FechaCorrecta As Boolean = True
            Dim ScriptFecha As String = ""
            If (InStr(UCase(dsCaracteristicas.Tables(0).Rows(e.RowIndex).Item(1)), "FECHA") > 0) Then
                Dim FechaCreacion As DateTime
                Try
                    FechaCreacion = gv_Solicitudes.SelectedRow.Cells(10).Text.ToString()
                    Fecha = test
                Catch ex As Exception

                    'ScriptFecha = "<script language='JavaScript'>Alert('" & ex.Message & "');</script>"
                    ScriptFecha = "<script language='JavaScript'>alert('" & ex.Message.Replace("'", "") & "');</script>"
                    'Response.Write(ScriptFecha)
                    'Page.RegisterStartupScript("Error Fechas Introducidas", ScriptFecha)
                    'Exit Sub
                    FechaCorrecta = False
                End Try


                'Comprobación para todas las fechas
                'para fecha visita también
                '¿comprobar exactamente si es "visita realizada"?
                If ddl_EstadoSol.SelectedValue <> "014" And ddl_EstadoSol.SelectedValue <> "010" Then
                    If Fecha.Date >= FechaCreacion.Date And Fecha.Date <= FechaCreacion.Date.AddDays(90) Then Correcto = True
                Else
                    If Fecha.Date >= FechaCreacion.Date And Fecha.Date <= Now() Then Correcto = True
                End If
            Else
                Correcto = True
            End If
            If FechaCorrecta Then
                If Correcto Then

                    dsCaracteristicas.Tables(0).Rows(e.RowIndex)("valor") = test

                    Session("dsCaracteristicas") = dsCaracteristicas

                    gv_Caracteristicas.Columns(0).Visible = True
                    gv_Caracteristicas.Columns(1).Visible = True

                    gv_Caracteristicas.EditIndex = -1
                    gv_Caracteristicas.DataSource = dsCaracteristicas
                    gv_Caracteristicas.DataBind()

                    'gv_Caracteristicas.Columns(0).Visible = False
                    gv_Caracteristicas.Columns(1).Visible = False
                Else
                    'Mostrar mensaje de problema de fechas.
                    Dim Script As String
                    If ddl_EstadoSol.SelectedValue <> "014" And ddl_EstadoSol.SelectedValue <> "010" Then
                        Script = "<script language='JavaScript'>alert('\t\t\tCompruebe la fecha introducida. \n(Debe de ser mayor o igual a la fecha de creación y NO superior a 90 dias desde la fecha de creación de la solicitud)');</script>"
                    Else
                        Script = "<script language='JavaScript'>alert('\t\t\tCompruebe la fecha introducida. \n(Debe de ser mayor o igual a la fecha de creación y NO superior al día de hoy)');</script>"
                    End If


                    Page.RegisterStartupScript("Error Fecha", Script)
                End If
            Else
                Page.RegisterStartupScript("Error Fecha", ScriptFecha)
            End If
            gv_Caracteristicas.Focus()
            btn_ModificarSolicitud.Focus()

        Catch ex As Exception
            Dim ErrorStr As String = ""
            ErrorStr = "<script language='JavaScript'>alert('" & ex.Message.Replace("'", "") & "');</script>"
            Page.RegisterStartupScript("Error Fecha", ErrorStr)
        End Try
    End Sub

    'Protected Function Prueba(ByVal Valor As String) As String
    '    If Valor = "Llamada Cortesia 1" Then
    '        Return "<asp:TextBox ID='txt_ValorCaracteristica' runat='server' Width='150px' Text='" + Valor + "'></asp:TextBox>"
    '        'Return "<input name='txt_ValorCaracteristica' runat='server' type='text' id='txt_ValorCaracteristica' style='width:150px;' />"
    '    Else
    '        Return "<asp:Button ID='a' runat='server' Visible='True' Width='90000px' />"
    '    End If
    'End Function

    Protected Sub gv_Caracteristicas_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_Caracteristicas.RowEditing
        'Try
        '    ErrorLog(Server.MapPath("Logs/ErrorLog"), "gv_Caracteristicas_RowEditing")

        '    Dim cod_tipo As String = gv_Solicitudes.SelectedRow.Cells(3).Text.ToString()
        '    Dim cod_subtipo As String = gv_Solicitudes.SelectedRow.Cells(4).Text.ToString()
        '    Dim cod_estado As String
        '    If ddl_EstadoSol.SelectedValue = -1 Then
        '        cod_estado = gv_Solicitudes.SelectedRow.Cells(5).Text.ToString()
        '    Else
        '        cod_estado = ddl_EstadoSol.SelectedValue
        '    End If

        '    Dim objCaracteristicasDB As New CaracteristicasDB()
        '    Dim dsCaracteristicas As DataSet
        '    If Session("IdPerfil") <> "3" Then
        '        dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasPorTipo(cod_tipo, cod_subtipo, cod_estado, 1)
        '    Else
        '        dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasPorTipoTelefono()
        '    End If

        '    'If cod_subtipo = "006" Then
        '    '    Dim Row As DataRow
        '    '    Row = dsCaracteristicas.Tables(0).NewRow()
        '    '    Row.Item(1) = "Fecha última visita"
        '    '    Row.Item(2) = "01/01/2011"

        '    '    dsCaracteristicas.Tables(0).Rows.Add(Row)
        '    'End If

        '    'Dim dsCaracteristicas As DataSet = CType(Session("dsCaracteristicas"), DataSet)
        '    ''Dim objCaracteristicasDB As New CaracteristicasDB()
        '    ''Dim dsCaracteristicas As DataSet = objCaracteristicasDB.GetCaracteristicasSolicitud(txt_IdSolicitud.Text)

        '    ErrorLog(Server.MapPath("Logs/ErrorLog"), "Cargado Dataset")
        '    gv_Caracteristicas.Columns(0).Visible = True
        '    gv_Caracteristicas.Columns(1).Visible = True

        '    'If Not ViewState("dsCaracteristicas") Is Nothing Then
        '    '    gv_Caracteristicas.DataSource = CType(ViewState("dsCaracteristicas"), DataSet)
        '    'Else
        '    '
        '    'End If

        '    gv_Caracteristicas.EditIndex = e.NewEditIndex()
        '    gv_Caracteristicas.DataSource = dsCaracteristicas
        '    gv_Caracteristicas.DataBind()
        '    ErrorLog(Server.MapPath("Logs/ErrorLog"), "Datasource del Grid")
        '    gv_Caracteristicas.Columns(1).Visible = False
        '    'gv_Caracteristicas.Focus()
        '    Me.btn_ModificarSolicitud.Focus()
        'Catch ex As Exception

        'End Try
    End Sub

    Protected Sub gv_Solicitudes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        ' FORMATEA ROWS
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' ASIGNA EVENTOS
            e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")
            e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_Solicitudes, "Select$" + e.Row.RowIndex.ToString)


            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = Tildes(DirectCast(DirectCast(DirectCast(e.Row.Cells(15), System.Web.UI.WebControls.TableCell).Controls(1), System.Web.UI.Control), System.Web.UI.WebControls.Label).ToolTip) 'e.Row.Cells(15).Text

                e.Row.Cells(i).Attributes("style") &= "cursor:hand;"
            Next

        End If

    End Sub

    Private Function Tildes(ByVal Texto As String) As String
        'Texto = Texto.Replace("á", "&#225;")
        'Texto = Texto.Replace("é", "&#233;")
        'Texto = Texto.Replace("í", "&#237;")
        'Texto = Texto.Replace("ó", "&#243;")
        'Texto = Texto.Replace("ú", "&#250;")
        'Texto = Texto.Replace("ñ", "&#241;")
        Return Texto
    End Function

    Protected Function acorta(ByVal texto As Object) As String
        If Len(texto.ToString()) > 31 Then
            Return texto.ToString().Substring(0, 30) & "..."
        Else
            Return texto.ToString()
        End If
    End Function

    Private Sub ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        e.Item.ToolTip = e.Item.Cells(9).Text

    End Sub

    'Protected Sub btn_exportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_exportar.Click

    '    Try
    '        ErrorLog(Server.MapPath("Logs/ErrorLog"), "btn_exportar_Click()")
    '        If Not Session("dsSolicitudes") Is Nothing Then

    '            Dim dsSolicitudes As DataSet = Session("dsSolicitudes")

    '            If Not dsSolicitudes Is Nothing Then

    '                If dsSolicitudes.Tables.Count > 0 Then
    '                    ErrorLog(Server.MapPath("Logs/ErrorLog"), "Llamada a ExportarExcel")
    '                    Dim rutaExcel As String = ExportarExcel(dsSolicitudes.Tables(0))

    '                    lnk_Excel.OnClientClick = "javascript:window.open('excel/" & rutaExcel & "');return false"
    '                    'Response.Write("<script language='javascript'>window.open('excel/" & rutaExcel & "');</script>")
    '                    lnk_Excel.Text = rutaExcel
    '                End If

    '            End If
    '            ErrorLog(Server.MapPath("Logs/ErrorLog"), "Todo OK.")
    '        End If
    '    Catch ex As Exception
    '        ErrorLog(Server.MapPath("Logs/ErrorLog"), ex.Message)
    '        lbl_mensajes.Text = "No existen solicitudes a exportar"
    '    End Try
    'End Sub

    Protected Sub chk_Pendientes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Pendientes.CheckedChanged

        'CargaSolicitudes()

    End Sub

    Protected Sub cal_Desde_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cal_Desde.SelectionChanged
        txtDesde.Text = cal_Desde.SelectedDate.ToString("dd/MM/yyyy")
        'txtHasta.Text = cal_Hasta.SelectedDate.ToString("dd/MM/yyyy")
        'CargaSolicitudes()
        cal_Desde.Visible = False

        gv_Solicitudes.DataSource = Nothing
        gv_Solicitudes.DataBind()
        lblBuscar.Visible = True
        lblTotal.Visible = False
        lblTotal0.Visible = False
        lblPaginas.Visible = False
        lblPaginaActual.Visible = False
        'gv_Solicitudes.DataSource = Nothing
        'gv_Solicitudes.DataBind()
        'Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        'Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        'Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        'If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        'CrearPaginador(NumeroPaginas)
    End Sub

    Protected Sub cal_Hasta_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cal_Hasta.SelectionChanged
        'txtDesde.Text = cal_Desde.SelectedDate.ToString("dd/MM/yyyy")
        txtHasta.Text = cal_Hasta.SelectedDate.ToString("dd/MM/yyyy")
        'CargaSolicitudes()
        cal_Hasta.Visible = False
        gv_Solicitudes.DataSource = Nothing
        gv_Solicitudes.DataBind()
        lblBuscar.Visible = True
        lblTotal.Visible = False
        lblTotal0.Visible = False
        lblPaginas.Visible = False
        lblPaginaActual.Visible = False
        'Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        'Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        'Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        'If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        'CrearPaginador(NumeroPaginas)
    End Sub

#End Region


    Protected Sub txt_contrato_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_contrato.TextChanged
        'CargaSolicitudes()
        gv_Solicitudes.DataSource = Nothing
        gv_Solicitudes.DataBind()
        lblBuscar.Visible = True
        lblTotal.Visible = False
        lblTotal0.Visible = False
        lblPaginas.Visible = False
        lblPaginaActual.Visible = False
        'Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        'Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        'Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        'If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        'CrearPaginador(NumeroPaginas)
    End Sub

    Protected Sub btnCalFechaLimite_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCalFechaLimite.Click
        'cal_Desde.Visible = True

    End Sub

    Protected Sub btnCalFechaCierre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCalFechaCierre.Click
        cal_Hasta.Visible = True
    End Sub

    Protected Sub ddl_Provincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Provincia.SelectedIndexChanged
        'Kintell 07/04/2009.
        'CargaSolicitudes()
        gv_Solicitudes.DataSource = Nothing
        gv_Solicitudes.DataBind()
        lblBuscar.Visible = True
        lblTotal.Visible = False
        lblTotal0.Visible = False
        lblPaginas.Visible = False
        lblPaginaActual.Visible = False
        'Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        'Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        'Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        'If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        'CrearPaginador(NumeroPaginas)
    End Sub

    Protected Sub chk_Urgente_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Urgente.CheckedChanged
        'Kintell 08/04/2009.
        'CargaSolicitudes()
    End Sub

    Protected Sub btnCalFechaLimite0_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCalFechaLimite0.Click
        cal_Desde.Visible = True
    End Sub

    Protected Sub btnCalFechaLimite1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCalFechaLimite1.Click
        'Kintell 14/04/2009.
        txtDesde.Text = ""
        'txtHasta.Text = ""
        cal_Desde.SelectedDate = Nothing


        'CargaSolicitudes()
    End Sub

    Protected Sub btnCalFechaLimite2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCalFechaLimite2.Click
        'Kintell 14/04/2009.
        txtHasta.Text = ""
        'txtDesde.Text = ""
        cal_Hasta.SelectedDate = Nothing

        'CargaSolicitudes()
    End Sub

    Protected Sub txtDesde_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDesde.TextChanged
        If Not (Me.txtDesde.Text.Equals(String.Empty)) Then
            cal_Desde.SelectedDate = Me.txtDesde.Text
        Else
            cal_Desde.SelectedDate = Now() '"01/01/0001"
        End If
        gv_Solicitudes.DataSource = Nothing
        gv_Solicitudes.DataBind()
        lblBuscar.Visible = True
        lblTotal.Visible = False
        lblTotal0.Visible = False
        lblPaginas.Visible = False
        lblPaginaActual.Visible = False
        'Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        'Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        'Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        'If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        'CrearPaginador(NumeroPaginas)
        'CargaSolicitudes()

    End Sub

    Protected Sub txtHasta_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHasta.TextChanged
        If Not (Me.txtDesde.Text.Equals(String.Empty)) Then
            cal_Hasta.SelectedDate = Me.txtHasta.Text
        Else
            cal_Hasta.SelectedDate = Now() ' "01/01/0001"
        End If
        gv_Solicitudes.DataSource = Nothing
        gv_Solicitudes.DataBind()
        lblBuscar.Visible = True
        lblTotal.Visible = False
        lblTotal0.Visible = False
        lblPaginas.Visible = False
        lblPaginaActual.Visible = False
        'Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        'Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        'Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        'If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        'lblPaginas.Text = "Nº de Páginas: " & NumeroPaginas
        'Me.hiddenPaginacion.Value = "1"
        'Page.RegisterStartupScript("IniciarContador", "<script language='javascript'>return ClickPaginacion('1');</script>")
        'CrearPaginador(NumeroPaginas)
        'CargaSolicitudes()
    End Sub

#Region " LOG "
    Private Function ErrorLog(ByVal sPathName As String, ByVal sErrMsg As String)
        'Dim sw As StreamWriter = New StreamWriter(sPathName & DateTime.Now.Year.ToString() & DateTime.Now.Month.ToString() & DateTime.Now.Day.ToString() & ".txt", True)
        'sw.WriteLine(DateTime.Now.ToShortDateString().ToString() & " " & DateTime.Now.ToLongTimeString().ToString() & " ==> " & sErrMsg)
        'sw.Flush()
        'sw.Close()
    End Function


#End Region

    'Protected Sub gv_Caracteristicas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Caracteristicas.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        ' ASIGNA EVENTOS
    '        e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")
    '        e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")
    '        e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_Solicitudes, "Select$" + e.Row.RowIndex.ToString)


    '        For i = 0 To e.Row.Cells.Count - 1
    '            e.Row.Cells(i).ToolTip = Tildes(DirectCast(DirectCast(DirectCast(e.Row.Cells(15), System.Web.UI.WebControls.TableCell).Controls(1), System.Web.UI.Control), System.Web.UI.WebControls.Label).ToolTip) 'e.Row.Cells(15).Text

    '            e.Row.Cells(i).Attributes("style") &= "cursor:hand;"
    '        Next

    '    End If
    'End Sub







    'Private tableCopied As Boolean = False
    'Private originalDataTable As System.Data.DataTable

    'Protected Sub gv_Caracteristicas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Caracteristicas.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        If Not tableCopied Then
    '            originalDataTable = CType(e.Row.DataItem, System.Data.DataRowView).Row.Table.Copy()
    '            ViewState("originalValuesDataTable") = originalDataTable
    '            tableCopied = True
    '        End If
    '    End If
    'End Sub
    'Private Function Probando()
    '    originalDataTable = CType(ViewState("originalValuesDataTable"), System.Data.DataTable)

    '    For Each r As GridViewRow In gv_Caracteristicas.Rows
    '        If IsRowModified(r) Then gv_Caracteristicas.UpdateRow(r.RowIndex, False)
    '    Next

    '    ' Rebind the Grid to repopulate the original values table.
    '    tableCopied = False
    '    gv_Caracteristicas.DataBind()

    'End Function

    'Protected Function IsRowModified(ByVal r As GridViewRow) As Boolean
    '    Dim currentID As Integer
    '    Dim currentTitleOfCourtesy As String
    '    Dim currentLastName As String
    '    Dim currentFirstName As String
    '    Dim currentTitle As String
    '    Dim currentExtension As String

    '    currentID = Convert.ToInt32(gv_Caracteristicas.DataKeys(0).Value)

    '    currentTitleOfCourtesy = CType(r.FindControl("txt_ValorCaracteristica"), TextBox).Text
    '    'currentLastName = CType(r.FindControl("LastNameTextBox"), TextBox).Text
    '    'currentFirstName = CType(r.FindControl("FirstNameTextBox"), TextBox).Text
    '    'currentTitle = CType(r.FindControl("TitleTextBox"), TextBox).Text
    '    'currentExtension = CType(r.FindControl("ExtensionTextBox"), TextBox).Text

    '    Dim row As System.Data.DataRow = _
    '        originalDataTable.Select(String.Format("Id_Caracteristica = {0}", currentID))(0)

    '    If Not currentTitleOfCourtesy.Equals(row("txt_ValorCaracteristica").ToString()) Then Return True
    '    'If Not currentLastName.Equals(row("LastName").ToString()) Then Return True
    '    'If Not currentFirstName.Equals(row("FirstName").ToString()) Then Return True
    '    'If Not currentTitle.Equals(row("Title").ToString()) Then Return True
    '    'If Not currentExtension.Equals(row("Extension").ToString()) Then Return True

    '    Return False
    'End Function

    Protected Sub btn_exportar0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_exportar0.Click
        Try
            ErrorLog(Server.MapPath("Logs/ErrorLog"), "btn_exportar_Click()")

            Dim objProveedoresDB As New ProveedoresDB()
            Dim dr As SqlDataReader = objProveedoresDB.GetCodigoProveedorPorNombre(txt_Proveedor.Text)
            dr.Read()

            'hidden_proveedor.Value = objProveedoresDB.GetProveedorPorTipoSubtipo(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, txt_contrato.Text)

            hidden_proveedor.Value = dr("proveedor").ToString()

            Dim cod_proveedor As String = hidden_proveedor.Value
            Dim cod_contrato As String = txt_contrato.Text
            Dim consultaPendietes As Boolean = chk_Pendientes.Checked
            Dim cod_tipo As String = ddl_Tipo.SelectedValue
            Dim cod_subtipo As String = ddl_Subtipo.SelectedValue
            Dim cod_estado As String = ddl_Estado.SelectedValue
            'Dim caldera As String = ddl_MarcaCaldera.Text

            Dim fechaDesde As String = cal_Desde.SelectedDate.ToString("yyyy/MM/dd") 'txtDesde.Text
            Dim fechaHasta As String = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd") 'txtHasta.Text

            If Not txtDesde.Text = "" And Not txtHasta.Text = "" Then
                txtDesde.Text = cal_Desde.SelectedDate.ToString("dd/MM/yyyy")
                txtHasta.Text = cal_Hasta.SelectedDate.ToString("dd/MM/yyyy")
                If txtHasta.Text = "01/01/0001" Then txtHasta.Text = Mid(Now, 1, 10)

                If fechaDesde = "0001/01/01" Then
                    fechaDesde = String.Empty
                End If
                If fechaHasta = "0001/01/01" Then
                    fechaHasta = String.Empty
                End If
            Else
                fechaDesde = String.Empty
                fechaHasta = String.Empty
            End If

            Dim objSolicitudesDB As New SolicitudesDB()
            Dim Urgente As String = "False"
            If Me.chk_Urgente.Checked Then Urgente = "True"




            Session("dsSolicitudes") = objSolicitudesDB.GetSolicitudesPorProveedorExcell(cod_proveedor, cod_contrato, consultaPendietes, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue, Urgente, Session("IdPerfilColores"), txt_IdSolicitud.Text) 'dsSolicitudes
            If Not Session("dsSolicitudes") Is Nothing Then

                Dim dsSolicitudes As DataSet = Session("dsSolicitudes")

                If Not dsSolicitudes Is Nothing Then

                    If dsSolicitudes.Tables.Count > 0 Then
                        ErrorLog(Server.MapPath("Logs/ErrorLog"), "Llamada a ExportarExcel")
                        Dim rutaExcel As String = ExportarDatagrid(dsSolicitudes.Tables(0))

                        lnk_Excel.OnClientClick = "javascript:window.open('excel/" & rutaExcel & "');return false"
                        'Response.Write("<script language='javascript'>window.open('excel/" & rutaExcel & "');</script>")
                        lnk_Excel.Text = rutaExcel
                    End If

                End If
                ErrorLog(Server.MapPath("Logs/ErrorLog"), "Todo OK.")
            End If
        Catch ex As Exception
            ErrorLog(Server.MapPath("Logs/ErrorLog"), ex.Message)
            lbl_mensajes.Text = "No existen solicitudes a exportar"
        End Try

    End Sub

    'Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    Try
    '        CargaSolicitudes()
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Protected Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        Try
            Session("DesdePagina") = Session("DesdePagina") + CInt(Session("NumeroRegistrosAVisualizar"))
            Session("HastaPagina") = Session("DesdePagina") + CInt(Session("NumeroRegistrosAVisualizar"))
            If Session("HastaPagina") >= Session("RegistrosTotales") Then
                Session("HastaPagina") = Session("RegistrosTotales")
                'ImageButton2.Visible = False
            End If
            'ImageButton3.Visible = True
            CargaSolicitudes()

            lblTotal0.Text = " Mostrando " & Session("DesdePagina") + 1 & " hasta " & Session("HastaPagina")

            Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
            Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
            Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

            If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

            lblPaginas.Text = "Nº de Páginas: " & NumeroPaginas

            Me.hiddenPaginacion.Value = Session("HastaPagina") \ NumeroPorPagina
            CrearPaginador(NumeroPaginas)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ImageButton3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton3.Click
        Try
            Session("DesdePagina") = Session("DesdePagina") - CInt(Session("NumeroRegistrosAVisualizar"))
            Session("HastaPagina") = Session("DesdePagina") + CInt(Session("NumeroRegistrosAVisualizar"))
            If Session("DesdePagina") <= 0 Then
                Session("DesdePagina") = 0
                Session("HastaPagina") = CInt(Session("NumeroRegistrosAVisualizar"))
            End If
            'If Session("DesdePagina") = 0 Then ImageButton3.Visible = False
            'ImageButton2.Visible = True
            CargaSolicitudes()

            lblTotal0.Text = " Mostrando " & Session("DesdePagina") + 1 & " hasta " & Session("HastaPagina")

            Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
            Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
            Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

            If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

            lblPaginas.Text = "Nº de Páginas: " & NumeroPaginas

            Me.hiddenPaginacion.Value = Session("HastaPagina") \ NumeroPorPagina

            CrearPaginador(NumeroPaginas)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Session("NumeroRegistrosAVisualizar") = CInt(ConfigurationManager.AppSettings("NumeroRegistrosAVisualizar").ToString())
            Session("DesdePagina") = 0
            Session("IdPerfilColores") = 0
            Session("HastaPagina") = CInt(Session("NumeroRegistrosAVisualizar")) '5

            txtContrato.Text = ""
            txtNombre.Text = ""
            gv_historicoCaracteristicas.Visible = False
            gv_Caracteristicas.DataSource = Nothing
            gv_Caracteristicas.DataBind()
            gv_HistoricoSolicitudes.DataSource = Nothing
            gv_HistoricoSolicitudes.DataBind()


            lblBuscar.Visible = False
            lblTotal.Visible = True
            lblTotal0.Visible = True
            lblPaginas.Visible = True
            lblPaginaActual.Visible = True

            lblTotal.Text = ""
            lblTotal0.Text = ""
            lblPaginas.Text = ""
            lblPaginaActual.Text = ""


            Dim cod_proveedor As String = hidden_proveedor.Value
            Dim cod_contrato As String = txt_contrato.Text
            Dim cod_tipo As String = ddl_Tipo.SelectedValue
            Dim cod_subtipo As String = ddl_Subtipo.SelectedValue
            Dim cod_estado As String = ddl_Estado.SelectedValue
            'Dim caldera As String = ddl_MarcaCaldera.Text

            Dim fechaDesde As String = cal_Desde.SelectedDate.ToString("yyyy/MM/dd") 'txtDesde.Text
            Dim fechaHasta As String = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd") 'txtHasta.Text

            If Not txtDesde.Text = "" And Not txtHasta.Text = "" Then
                txtDesde.Text = cal_Desde.SelectedDate.ToString("dd/MM/yyyy")
                'fechaHasta = txtHasta.Text
                If txtHasta.Text = "" Then txtHasta.Text = cal_Hasta.SelectedDate.ToString("dd/MM/yyyy")
                If txtHasta.Text = "01/01/0001" Then
                    txtHasta.Text = Mid(Now, 1, 10)
                Else
                    cal_Hasta.SelectedDate = txtHasta.Text

                    fechaHasta = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd")
                End If


                If fechaDesde = "0001/01/01" Then
                    fechaDesde = String.Empty
                End If
                If fechaHasta = "0001/01/01" Then
                    fechaHasta = String.Empty
                End If
            Else
                fechaDesde = String.Empty
                fechaHasta = String.Empty
            End If

            Dim Urgente As String = "False"
            If Me.chk_Urgente.Checked Then Urgente = "True"

            Dim objSolicitudesDB As New SolicitudesDB()
            Dim consultaPendietes As Boolean = chk_Pendientes.Checked
            Dim Proveedor As String = txt_Proveedor.Text
            If Not UCase(txt_Proveedor.Text) = "ADICO" Then Proveedor = Left(txt_Proveedor.Text, 3)
            'Session("RegistrosTotales") = objSolicitudesDB.ObtenerRegistrosTotales(Proveedor, consultaPendietes, cod_contrato, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, Urgente, ddl_Provincia.SelectedValue, Session("IdPerfilColores"))



            'If Session("IdPerfil") <> "3" Then
            ''ImageButton3.Visible = False
            CargaSolicitudes()
            'Else
            'CargarNivelSatisfaccion()
            'End If





            lblTotal.Text = "Nº de Registros: " & Session("RegistrosTotales")

            If CInt(Session("RegistrosTotales")) > CInt(Session("NumeroRegistrosAVisualizar")) Then
                lblTotal0.Text = " Mostrando 1 hasta " & CInt(Session("NumeroRegistrosAVisualizar"))
                If CInt(Session("NumeroRegistrosAVisualizar")) < 1 Then lblTotal0.Text = " No hay registros"
                'ImageButton2.Visible = True
            Else
                lblTotal0.Text = " Mostrando 1 hasta " & CInt(Session("RegistrosTotales"))
                If CInt(Session("RegistrosTotales")) < 1 Then lblTotal0.Text = " No hay registros"
            End If

            Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
            Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
            Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

            If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

            lblPaginas.Text = "Nº de Páginas: " & NumeroPaginas
            Me.hiddenPaginacion.Value = "1"
            CrearPaginador(NumeroPaginas)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CrearPaginador(ByVal NumeroPaginas As Integer)
        Try


            Dim intNumPaginas As Integer = CInt(NumeroPaginas)
            Dim intPaginaActual As Integer = CInt(Me.hiddenPaginacion.Value)
            lblPaginaActual.Text = "Página actual: " & intPaginaActual
            If CInt(NumeroPaginas) = 0 Then lblPaginaActual.Text = "Página actual: 0"

            Dim numPaginasMostradas As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
            Dim rangoActual As Integer = intPaginaActual \ numPaginasMostradas

            Dim limiteSuperior As Integer = (rangoActual + 1) * numPaginasMostradas

            Dim limiteInferior As Integer

            limiteInferior = intPaginaActual - 4
            If limiteInferior < 1 Then limiteInferior = 1
            limiteSuperior = intPaginaActual + 4


            If (limiteSuperior > intNumPaginas) Then
                limiteSuperior = intNumPaginas
            End If

            'limiteInferior = (rangoActual * numPaginasMostradas) + 1

            If (limiteInferior = intPaginaActual And intPaginaActual > 1) Then

                limiteInferior -= numPaginasMostradas
                If (limiteInferior < 0) Then
                    limiteInferior = 1
                End If
                If limiteSuperior = intNumPaginas Then
                    limiteSuperior = (rangoActual) * numPaginasMostradas
                Else
                    limiteSuperior -= numPaginasMostradas
                End If
            End If
            Me.pnlPaginas.Controls.Clear()

            If intPaginaActual > 1 Then '((limiteInferior) > numPaginasMostradas) Then
                Dim lb As Button = New Button()
                lb.ID = "hlPaginacion1"
                lb.CommandName = "hlPaginacion1"
                lb.CommandArgument = 1.ToString()
                lb.OnClientClick = "return ClickPaginacion('1')"
                lb.BackColor = Drawing.Color.Transparent
                lb.ForeColor = Drawing.Color.Blue
                lb.Font.Underline = True
                lb.Font.Bold = True
                lb.Font.Size = 10
                lb.Style.Value &= ";cursor:hand;"
                lb.BorderStyle = BorderStyle.None
                lb.CssClass = "linkPaginacion"
                lb.Text = ("<<").ToString()
                Me.pnlPaginas.Controls.Add(lb)
            End If


            If intPaginaActual > 1 Then
                Dim lb As Button = New Button()
                lb = New Button()
                lb.ID = "hlPaginacionAnterior"
                lb.CommandName = "hlPaginacioAnterior"
                lb.CommandArgument = 1.ToString()
                lb.OnClientClick = "return ClickPaginacion('" & intPaginaActual - 1 & "')"
                lb.BackColor = Drawing.Color.Transparent
                lb.ForeColor = Drawing.Color.Blue
                lb.Font.Underline = True
                lb.Font.Bold = True
                lb.Font.Size = 10
                lb.BorderStyle = BorderStyle.None
                lb.CssClass = "linkPaginacion"
                lb.Style.Value &= ";cursor:hand;"
                lb.Text = (" <").ToString()
                Me.pnlPaginas.Controls.Add(lb)
            End If


            Dim i As Integer = limiteInferior
            For i = limiteInferior To limiteSuperior
                Dim lb As Button = New Button()
                lb.ID = "hlPaginacion" & i
                lb.CommandName = "hlPaginacion" & i
                lb.CommandArgument = i.ToString()
                lb.OnClientClick = "return ClickPaginacion('" & i & "')"
                lb.CssClass = "linkPaginacion"
                lb.Text = " " & (i).ToString()
                lb.BackColor = Drawing.Color.Transparent
                lb.ForeColor = Drawing.Color.Blue
                lb.Style.Value &= ";cursor:hand;"
                lb.Font.Underline = True
                lb.Font.Size = 10
                lb.Font.Bold = True
                lb.BorderStyle = BorderStyle.None

                If (i = intPaginaActual) Then
                    lb.Enabled = False
                Else
                    lb.Enabled = True
                End If
                Me.pnlPaginas.Controls.Add(lb)
            Next


            If intPaginaActual < NumeroPaginas Then
                Dim lb As Button = New Button()
                lb = New Button()
                lb.ID = "hlPaginacionSiguiente"
                lb.CommandName = "hlPaginacioSiguiente"
                lb.CommandArgument = 1.ToString()
                lb.OnClientClick = "return ClickPaginacion('" & intPaginaActual + 1 & "')"
                lb.BackColor = Drawing.Color.Transparent
                lb.ForeColor = Drawing.Color.Blue
                lb.Font.Underline = True
                lb.Font.Bold = True
                lb.Style.Value &= ";cursor:hand;"
                lb.Font.Size = 10
                lb.BorderStyle = BorderStyle.None
                lb.CssClass = "linkPaginacion"
                lb.Text = (" >").ToString()
                Me.pnlPaginas.Controls.Add(lb)
            End If


            If intPaginaActual < NumeroPaginas Then '(limiteSuperior < intNumPaginas) Then
                Dim lb As Button = New Button()
                lb = New Button()
                lb.ID = "hlPaginacion" & intNumPaginas
                lb.CommandName = "hlPaginacion" & intNumPaginas
                lb.CommandArgument = intNumPaginas.ToString()
                lb.OnClientClick = "return ClickPaginacion('" & intNumPaginas & "')"
                lb.CssClass = "linkPaginacion"
                lb.BackColor = Drawing.Color.Transparent
                lb.ForeColor = Drawing.Color.Blue
                lb.Font.Underline = True
                lb.Style.Value &= ";cursor:hand;"
                lb.Font.Bold = True
                lb.Font.Size = 10
                lb.BorderStyle = BorderStyle.None
                lb.Text = (" >>").ToString()
                Me.pnlPaginas.Controls.Add(lb)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CrearPaginador1(ByVal NumeroPAginasTotales As Integer)
        Try
            Dim i As Integer = 0
            For i = 0 To pnlPaginas.Controls.Count - 1
                pnlPaginas.Controls.RemoveAt(i)
            Next

            For i = 0 To NumeroPAginasTotales - 1
                Dim a As New LinkButton
                a.Text = i + 1 & " "
                a.ID = i + 1
                a.OnClientClick = "return ClickPaginacion('" & i + 1 & "');"
                a.ForeColor = System.Drawing.Color.Blue
                pnlPaginas.Controls.Add(a)
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Function PAginacion(ByVal Pagina As Integer)
        Try
            Session("DesdePagina") = (Pagina * CInt(Session("NumeroRegistrosAVisualizar"))) - CInt(Session("NumeroRegistrosAVisualizar"))
            'Session("DesdePagina") = Session("DesdePagina") + CInt(Session("NumeroRegistrosAVisualizar"))
            Session("HastaPagina") = Session("DesdePagina") + CInt(Session("NumeroRegistrosAVisualizar"))
            If Session("HastaPagina") >= Session("RegistrosTotales") Then
                Session("HastaPagina") = Session("RegistrosTotales")
                'ImageButton2.Visible = False
            End If
            'ImageButton3.Visible = True
            CargaSolicitudes()

            lblTotal0.Text = " Mostrando " & Session("DesdePagina") + 1 & " hasta " & Session("HastaPagina")
        Catch ex As Exception

        End Try
    End Function



    Private Sub CargaHistoricoCaracteristicas(ByVal id_movimiento As String)

        Dim objHistoricoDB As New HistoricoDB()
        Dim dsHistorico As DataSet = objHistoricoDB.GetHistoricoCaracteristicasPorMovimiento(id_movimiento, 1)

        gv_historicoCaracteristicas.Columns(0).Visible = True

        gv_historicoCaracteristicas.DataSource = dsHistorico
        gv_historicoCaracteristicas.DataBind()

        gv_historicoCaracteristicas.Columns(0).Visible = False

    End Sub

    Protected Sub gv_HistoricoSolicitudes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_HistoricoSolicitudes.RowDataBound
        ' FORMATEA ROWS
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' ASIGNA EVENTOS
            e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")
            e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_HistoricoSolicitudes, "Select$" + e.Row.RowIndex.ToString)

            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).Attributes("style") &= "cursor:hand;"
            Next
        End If
    End Sub

    Protected Sub gv_HistoricoSolicitudes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_HistoricoSolicitudes.SelectedIndexChanged
        gv_historicoCaracteristicas.Visible = True
        Dim id_movimiento As String = gv_HistoricoSolicitudes.SelectedRow.Cells(0).Text
        CargaHistoricoCaracteristicas(id_movimiento)

        Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
        Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
        Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

        If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

        CrearPaginador(NumeroPaginas)
    End Sub


    Protected Sub gv_Solicitudes_RowDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Solicitudes.RowDataBound
        ' FORMATEA ROWS
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' ASIGNA EVENTOS
            e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")
            e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")
            'e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_Solicitudes, "Select$" + e.Row.RowIndex.ToString)

            'btnTiempo.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_Solicitudes, "Select$" + e.Row.RowIndex.ToString)


            e.Row.Attributes.Add("OnClick", "CargarDatos('Select$" + e.Row.RowIndex.ToString + "');")


            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = Tildes(DirectCast(DirectCast(DirectCast(e.Row.Cells(15), System.Web.UI.WebControls.TableCell).Controls(1), System.Web.UI.Control), System.Web.UI.WebControls.Label).ToolTip) 'e.Row.Cells(15).Text
                e.Row.Cells(i).Attributes("style") &= "cursor:hand;"

                If i = 1 Then
                    Dim objSolicitudesDB As New SolicitudesDB()
                    Dim Perfil As Integer = objSolicitudesDB.ObtenerPerfilPorSolicitud(e.Row.Cells(i).Text)

                    If Perfil = 3 Then
                        'PERFIL TELEFONO.
                        e.Row.BackColor = Drawing.Color.GreenYellow
                    ElseIf Perfil = 4 Then
                        'PERFIL ADICO.
                        e.Row.BackColor = Drawing.Color.Red
                    ElseIf Perfil > 0 Then
                        'PERFILES PROVEEDORES.
                        e.Row.BackColor = Drawing.Color.RoyalBlue
                        'e.Row.ForeColor = Drawing.Color.Wheat
                    End If
                End If


                'Kintell 18/08/2010.
                'Pintamos la descripción del subtipo (8).
                If i = 8 Then
                    Dim objSolicitudesDB As New SolicitudesDB()
                    e.Row.Cells(i).Text = objSolicitudesDB.ObtenerDescripcionSubtipo(e.Row.Cells(i).Text)
                End If
                'Kintell 18/08/2010.
                'Pintamos la descripción del estado (9).
                If i = 9 Then
                    Dim objSolicitudesDB As New SolicitudesDB()
                    e.Row.Cells(i).Text = objSolicitudesDB.ObtenerDescripcionEstado(e.Row.Cells(i).Text)
                End If
            Next

        End If
    End Sub

    Protected Sub gv_Solicitudes_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Solicitudes.SelectedIndexChanged
        Try
            If Not gv_Solicitudes.Rows.Count = 0 Then
                btnCaldera0.Visible = True
                txtContrato.Text = ""
                txtNombre.Text = ""
                gv_historicoCaracteristicas.Visible = False


                Dim id_solicitud As String = gv_Solicitudes.SelectedRow.Cells(1).Text.ToString()


                'Kintell 12/12/2011
                CargaComboMotivosCancelacion(gv_Solicitudes.SelectedRow.Cells(4).Text.ToString())
                '*********************************************************************************
                Dim li As ListItem = Nothing
                li = ddl_MotivoCancel.Items.FindByText("Seleccione el motivo de la cancelación")
                If (li Is Nothing) Then
                    li = New ListItem
                    li.Text = "Seleccione el motivo de la cancelación"
                    li.Value = -1
                    ddl_MotivoCancel.Items.Insert(0, li)
                End If

                CargaDatosSolicitud(id_solicitud)


                Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
                Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
                If Not NumeroTotales = 0 And Not NumeroPorPagina = 0 Then
                    Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

                    If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

                    CrearPaginador(NumeroPaginas)
                End If


            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub OnBtnPaginacion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaginacion.Click
        Dim i As Integer = 0
        Dim PaginaActual As Integer = CInt(Me.hiddenPaginacion.Value)

        Try
            Session("DesdePagina") = (PaginaActual * CInt(Session("NumeroRegistrosAVisualizar"))) - CInt(Session("NumeroRegistrosAVisualizar"))
            'Session("DesdePagina") = Session("DesdePagina") + CInt(Session("NumeroRegistrosAVisualizar"))
            Session("HastaPagina") = Session("DesdePagina") + CInt(Session("NumeroRegistrosAVisualizar"))
            If Session("HastaPagina") >= Session("RegistrosTotales") Then
                Session("HastaPagina") = Session("RegistrosTotales")
                'ImageButton2.Visible = False
            End If
            'ImageButton3.Visible = True
            CargaSolicitudes()

            lblTotal0.Text = " Mostrando " & Session("DesdePagina") + 1 & " hasta " & Session("HastaPagina")


            Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
            Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
            Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

            If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

            lblPaginas.Text = "Nº de Páginas: " & NumeroPaginas

            CrearPaginador(NumeroPaginas)


        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnDesconectar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDesconectar.Click
        Try
            HttpContext.Current.User = Nothing

            Response.Redirect("../Login.aspx")
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMenu.Click
        Try
            HttpContext.Current.User = Nothing

            Response.Redirect("../MenuAcceso.aspx")
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'TELEFONO.
        lblBuscar.Visible = False
        lblTotal.Visible = True
        lblTotal0.Visible = True
        lblPaginas.Visible = True
        lblPaginaActual.Visible = True
        CargaSolicitudesPorColores("3")
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        'PROVEEDOR.
        lblBuscar.Visible = False
        lblTotal.Visible = True
        lblTotal0.Visible = True
        lblPaginas.Visible = True
        lblPaginaActual.Visible = True
        CargaSolicitudesPorColores("1,2,5,6")
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        'ADMINISTRADOR.
        lblBuscar.Visible = False
        lblTotal.Visible = True
        lblTotal0.Visible = True
        lblPaginas.Visible = True
        lblPaginaActual.Visible = True
        CargaSolicitudesPorColores("4")
    End Sub

    Private Function Buscar()
        Try
            Session("NumeroRegistrosAVisualizar") = CInt(ConfigurationManager.AppSettings("NumeroRegistrosAVisualizar").ToString())
            Session("DesdePagina") = 0
            Session("HastaPagina") = CInt(Session("NumeroRegistrosAVisualizar")) '5

            txtContrato.Text = ""
            txtNombre.Text = ""
            gv_historicoCaracteristicas.Visible = False
            gv_Caracteristicas.DataSource = Nothing
            gv_Caracteristicas.DataBind()
            gv_HistoricoSolicitudes.DataSource = Nothing
            gv_HistoricoSolicitudes.DataBind()


            lblBuscar.Visible = False
            lblTotal.Visible = True
            lblTotal0.Visible = True
            lblPaginas.Visible = True
            lblPaginaActual.Visible = True

            lblTotal.Text = ""
            lblTotal0.Text = ""
            lblPaginas.Text = ""
            lblPaginaActual.Text = ""


            Dim cod_proveedor As String = hidden_proveedor.Value
            Dim cod_contrato As String = txt_contrato.Text
            Dim cod_tipo As String = ddl_Tipo.SelectedValue
            Dim cod_subtipo As String = ddl_Subtipo.SelectedValue
            Dim cod_estado As String = ddl_Estado.SelectedValue
            'Dim caldera As String = ddl_MarcaCaldera.Text

            Dim fechaDesde As String = cal_Desde.SelectedDate.ToString("yyyy/MM/dd") 'txtDesde.Text
            Dim fechaHasta As String = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd") 'txtHasta.Text

            If Not txtDesde.Text = "" And Not txtHasta.Text = "" Then
                txtDesde.Text = cal_Desde.SelectedDate.ToString("dd/MM/yyyy")
                'fechaHasta = txtHasta.Text
                If txtHasta.Text = "" Then txtHasta.Text = cal_Hasta.SelectedDate.ToString("dd/MM/yyyy")
                If txtHasta.Text = "01/01/0001" Then
                    txtHasta.Text = Mid(Now, 1, 10)
                Else
                    cal_Hasta.SelectedDate = txtHasta.Text

                    fechaHasta = cal_Hasta.SelectedDate.ToString("yyyy/MM/dd")
                End If


                If fechaDesde = "0001/01/01" Then
                    fechaDesde = String.Empty
                End If
                If fechaHasta = "0001/01/01" Then
                    fechaHasta = String.Empty
                End If
            Else
                fechaDesde = String.Empty
                fechaHasta = String.Empty
            End If

            Dim Urgente As String = "False"
            If Me.chk_Urgente.Checked Then Urgente = "True"

            Dim objSolicitudesDB As New SolicitudesDB()
            Dim consultaPendietes As Boolean = chk_Pendientes.Checked
            Dim Proveedor As String = txt_Proveedor.Text
            If Not UCase(txt_Proveedor.Text) = "ADICO" Then Proveedor = Left(txt_Proveedor.Text, 3)
            Session("RegistrosTotales") = objSolicitudesDB.ObtenerRegistrosTotales(Proveedor, consultaPendietes, cod_contrato, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, Urgente, ddl_Provincia.SelectedValue, Session("IdPerfilColores"), txt_IdSolicitud.Text)


            lblTotal.Text = "Nº de Registros: " & Session("RegistrosTotales")

            'ImageButton3.Visible = False
            CargaSolicitudes()
            If CInt(Session("RegistrosTotales")) > CInt(Session("NumeroRegistrosAVisualizar")) Then
                lblTotal0.Text = " Mostrando 1 hasta " & CInt(Session("NumeroRegistrosAVisualizar"))
                If CInt(Session("NumeroRegistrosAVisualizar")) < 1 Then lblTotal0.Text = " No hay registros"
                'ImageButton2.Visible = True
            Else
                lblTotal0.Text = " Mostrando 1 hasta " & CInt(Session("RegistrosTotales"))
                If CInt(Session("RegistrosTotales")) < 1 Then lblTotal0.Text = " No hay registros"
            End If

            Dim NumeroTotales As Integer = CInt(Session("RegistrosTotales"))
            Dim NumeroPorPagina As Integer = CInt(Session("NumeroRegistrosAVisualizar"))
            Dim NumeroPaginas As Integer = CInt(NumeroTotales \ NumeroPorPagina)

            If Not (NumeroTotales Mod NumeroPorPagina) = 0 Then NumeroPaginas = NumeroPaginas + 1

            lblPaginas.Text = "Nº de Páginas: " & NumeroPaginas
            Me.hiddenPaginacion.Value = "1"
            CrearPaginador(NumeroPaginas)
        Catch ex As Exception

        End Try
    End Function

    Protected Sub btnCargarSolictudesTelefono_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargarSolictudesTelefono.Click
        If File.Exists(Server.MapPath("~/SolicitudesCortesia") & "\\" & FileUpload1.FileName) = False Then
            FileUpload1.SaveAs(Server.MapPath("~/SolicitudesCortesia") & "\\" & FileUpload1.FileName)
        End If
        Leer(FileUpload1.FileName)
        File.Delete(Server.MapPath("~/SolicitudesCortesia") & "\\" & FileUpload1.FileName)
    End Sub

    Protected Sub Leer(ByVal NombreArchivo As String)
        Dim ruta As String = Server.MapPath("~/SolicitudesCortesia") & "\\" & NombreArchivo
        Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & ruta & "';Extended Properties=""Excel 12.0;HDR=NO;IMEX=1"""
        Dim factory As DbProviderFactory = DbProviderFactories.GetFactory("System.Data.OleDb")
        Dim connection As DbConnection = factory.CreateConnection()
        Dim command As DbCommand = connection.CreateCommand()

        connection.ConnectionString = connectionString
        command.CommandText = "SELECT * from [Hoja1$]"
        Try
            connection.Open()
            Dim dr1 As DbDataReader = command.ExecuteReader()
            Dim i As Integer = 0

            While dr1.Read()
                Dim sol As SolicitudesDB = New SolicitudesDB()
                sol.AddSolicitudCortesia(dr1(0).ToString(), dr1(1).ToString(), dr1(2).ToString(), dr1(3).ToString())
            End While
            dr1.Close()
            MostrarMensaje("Proceso Completado")
        Catch ex As Exception
            MostrarMensaje(ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    Private Function MostrarMensaje(ByVal Mensaje As String)

        Dim alerta As String = "<script type='text/javascript'>alert('" + Mensaje + "');</script>"

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ALERTA", alerta, False)
    End Function

End Class