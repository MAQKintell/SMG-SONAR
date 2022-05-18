Imports System.Data
Imports System.Web.Configuration
Imports System.Data.SqlClient

Imports Iberdrola.Commons.Web

Partial Class Pages_Mantenimiento_Solicitudes
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Not Page.IsPostBack()) Then
        'Dim Cultur As AsignarCultura = New AsignarCultura()
        'Cultur.Asignar(Page)
        'End if

        Dim usuario As String = String.Empty

        Try
            usuario = Session("usuarioValido").ToString()
        Catch ex As Exception
            Dim redirectURL As String = ConfigurationManager.AppSettings("url_Login").ToString()
            Response.Redirect(redirectURL, False)
        End Try

        If Not Page.IsPostBack() Then
            Session("NumeroVuelta") = 1
            If Not String.IsNullOrEmpty(Request.QueryString("con")) And Not String.IsNullOrEmpty(Request.QueryString("mod")) Then

                Dim contrato As String = Request.QueryString("con")

                'primer combo necesario para los demas
                CargaComboTipoSolicitud()
                'Kintell 12/12/2011
                CargaComboMotivosCancelacion(ddl_Subtipo.SelectedValue)
                '******************************************************
                CargaComboTiposAveria()
                CargaComboTiposVisita()

                '31/05/2011
                'Cargamos los proveedores.
                CargaComboProveedores()

                'datos solicitudes
                If Not String.IsNullOrEmpty(Request.QueryString("sol")) Then

                    CargaDatosSolicitud(Request.QueryString("sol"))
                    CargaHistorico(Request.QueryString("sol"))
                Else
                    'resto de combos relacionados con solicitud
                    CargaComboSubtipoSolicitud(ddl_Tipo.SelectedValue)
                    CargaComboEstadoSolicitud(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue)
                End If


                



                
                hdnIdSolicitud.Value = Request.QueryString("sol")
                'Gorka: hasta que no tenemos los datos de los combos cargados
                ' no podemos cargar la información del contrato, ya que se está
                ' utilizando la información de los combos tipo y subtipo de solicitud
                ' para obtener los proveedores.
                

                'datos contrato
                CargaDatosContrato(contrato)

                'listado solicitudes
                CargaSolicitudes(contrato)


                

            Else
                Dim url_info_visita As String = ConfigurationManager.AppSettings("url_info_visita").ToString()
                Response.Redirect(url_info_visita, False)
            End If


            CargaComboCalderasNuevo()
        Else
            Session("NumeroVuelta") = Session("NumeroVuelta") + 1

        End If


        Dim modo As String = Request.QueryString("mod")

        If modo = "v" Then
            ModoVisualizar()
        ElseIf modo = "c" Then
            ModoCrear()
        ElseIf modo = "m" Then
            ModoModificar()
        End If

        lbl_mensajes.Text = String.Empty

        Dim info_visita As String = ConfigurationManager.AppSettings("url_info_visita").ToString()
        lnk_Volver.PostBackUrl = info_visita



        If Not Page.IsPostBack() Then
            If Request.QueryString("Abierta") = "1" Then
                Dim Script As String = "<script language='JavaScript'>"
                'Script &= "var str='aaaaa';"
                Script &= "alert('ATENCIÓN!!!\nTiene una avería Abierta actualmente.');</script>" 'AveriaAbierta()
                RegisterStartupScript("Averia Abierta", Script)
            End If
        End If

        'Kintell 06/04/2009
        If UCase(ddl_Estado.SelectedItem.Text) = UCase("reparado") Or UCase(ddl_Estado.SelectedItem.Text) = UCase("reparada") Or UCase(ddl_Estado.SelectedItem.Text) = UCase("Cancelada") Or UCase(ddl_Estado.SelectedItem.Text) = UCase("Reparada por telefono") Then
            btn_AltaSolicitud.Enabled = False
            btn_ModificarSolicitud.Enabled = False
            btnAviso.Enabled = False
            btnLLAMADA.Enabled = False
        End If

        'Kintell 20/04/2012 (evitar k el telefono meta observaciones en solicitudes ya cerradas).
        If UCase(ddl_Estado.SelectedItem.Text) = UCase("reparado") Or UCase(ddl_Estado.SelectedItem.Text) = UCase("reparada") Or UCase(ddl_Estado.SelectedItem.Text) = UCase("Cancelada") Or UCase(ddl_Estado.SelectedItem.Text) = UCase("Reparada por telefono") Or UCase(ddl_Estado.SelectedItem.Text) = UCase("Baja de Servicio") Or UCase(ddl_Estado.SelectedItem.Text) = UCase("Visita Realizada") Then
            txt_Observaciones.Enabled = False
        Else
            txt_Observaciones.Enabled = True
        End If


        chkUrgente.Enabled = False
        ddl_MarcaCaldera.Enabled = False
        ddl_MarcaCaldera1.Enabled = False

        'If Not Page.IsPostBack() Then
        '    If InStr(UCase(ddl_Subtipo.SelectedItem.Text), UCase("Incorr")) > 0 Then
        '        CargaComboTiposVisitaIncorrecta()
        '    Else
        '        CargaComboTiposVisita()
        '    End If
        'End If

        'Kintell 22/11/2011 Tema HORARIO CONTACTO.
        If Not Page.IsPostBack() Then
            Dim db As SolicitudesDB = New SolicitudesDB()
            Dim Contra As String = Request.QueryString("con")
            Dim datos As DataSet = db.ObtenerDatosDesdeSentencia("SELECT top 1 HORARIO_CONTACTO FROM MANTENIMIENTO WHERE COD_CONTRATO_SIC='" & Contra & "'")
            If datos.Tables(0).Rows.Count > 0 Then
                cmbHorarioContacto.SelectedValue = datos.Tables(0).Rows(0).Item(0)
            End If
        End If

        'Kintell 02/02/2012
        'Si esta de baja se permite operar con solicitudes de visita y averia incorrecta durante 6 meses. 
        If Not Page.IsPostBack() Then
            If Request.QueryString("Baja") = "1" Then
                Dim Script As String = "<script language='JavaScript'>"
                Dim SQL As String = "select case when dateadd(month,6,getdate()) > fec_baja_contrato then '1' else '0' end as Valido from mantenimiento where cod_contrato_sic='" & Request.QueryString("con") & "'"
                'Tiene k ser si se ha cerrado una en los ultimos 6 meses...
                'SQL = "select case when dateadd(month,6,getdate()) > max(time_stamp) then '1' else '0' end as Valido,MAX(subtipo_solicitud) as subtipo from solicitudes s inner join historico h on s.id_solicitud=h.id_solicitud inner join estado_solicitud e on h.estado_solicitud=e.codigo where cod_contrato='" & Request.QueryString("con") & "' and estado_final=1 and subtipo_solicitud IN ('001') "
                '19-07-2012
                'SQL = "select v.valido as val, v1.valido as val1 from(select case when dateadd(month,6,getdate()) > max(time_stamp) then '1' else '0' end as Valido,max(cod_contrato) as cod_contrato from solicitudes s inner join historico h on s.id_solicitud=h.id_solicitud inner join estado_solicitud e on h.estado_solicitud=e.codigo where cod_contrato='" & Request.QueryString("con") & "' and estado_final=1 and subtipo_solicitud IN ('001'))v inner join (select case when dateadd(month,6,getdate()) > max(h.fecha) then '1' else '0' end as Valido ,max(v.cod_contrato) as cod_contrato from visita v inner join visita_historico h on v.cod_contrato=h.cod_contrato and v.cod_visita=h.cod_visita inner join TIPO_ESTADO_VISITA e on h.COD_ESTADO_NUEVO=e.COD_ESTADO where v.cod_contrato='" & Request.QueryString("con") & "' and COD_ESTADO_NUEVO IN ('02','10'))v1 on v.cod_contrato=v1.cod_contrato "
                SQL = "select case when dateadd(month,6,max(time_stamp)) > getdate() then '1' else '0' end as val, (select case when dateadd(month,6,max(h.fecha)) > getdate() then '1' else '0' end as Valido from visita v inner join visita_historico h on v.cod_contrato=h.cod_contrato and v.cod_visita=h.cod_visita inner join TIPO_ESTADO_VISITA e on h.COD_ESTADO_NUEVO=e.COD_ESTADO where v.cod_contrato='" & Request.QueryString("con") & "' and COD_ESTADO_NUEVO IN ('02','10')) as val1 from solicitudes s inner join historico h on s.id_solicitud=h.id_solicitud inner join estado_solicitud e on h.estado_solicitud=e.codigo where cod_contrato='" & Request.QueryString("con") & "' and estado_final=1 and subtipo_solicitud IN ('001') "
                Dim db As SolicitudesDB = New SolicitudesDB()
                Dim datos As DataSet = db.ObtenerDatosDesdeSentencia(SQL)
                If (datos.Tables(0).Rows.Count > 0) Then
                    hdnSubtipo6meses.Value = datos.Tables(0).Rows(0).Item(0) & ";" & datos.Tables(0).Rows(0).Item(1)
                    If datos.Tables(0).Rows(0).Item(0) = "0" And datos.Tables(0).Rows(0).Item(1) = "0" Then
                        Script &= "alert('CONTRATO DADO DE BAJA HACE MAS DE 6 MESES\nNo se aceptan modificaciones');window.history.back();</script>" 'AveriaAbierta()
                        RegisterStartupScript("Contrato de baja hace mas de seis meses", Script)
                    Else
                        hdnBajaContrato.Value = Request.QueryString("Baja")

                        Script = "<script language='JavaScript'>"
                        Script &= "alert('ATENCIÓN!!!\nCONTRATO DADO DE BAJA\nSolo se permite operar con solicitudes de Visitas o Averias incorrectas.');</script>" 'AveriaAbierta()
                        RegisterStartupScript("Contrato de baja", Script)
                    End If
                Else
                    hdnBajaContrato.Value = Request.QueryString("Baja")
                    '1;0
                    Script = "<script language='JavaScript'>"
                    Script &= "alert('ATENCIÓN!!!\nCONTRATO DADO DE BAJA\nSolo se permite operar con solicitudes de Visitas o Averias incorrectas.');</script>" 'AveriaAbierta()
                    RegisterStartupScript("Contrato de baja", Script)
                End If
            End If
        End If
    End Sub


#Region "Funciones"
    Private Sub CargaComboCalderasAbajo(ByVal Contrato As String)

        ddl_MarcaCaldera1.Items.Clear()

        Dim objCalderasDB As New CalderasDB()

        ddl_MarcaCaldera.SelectedValue = Nothing
        ddl_MarcaCaldera1.DataSource = objCalderasDB.GetMarcaCalderas()
        ddl_MarcaCaldera1.DataTextField = "descripcion"
        ddl_MarcaCaldera1.DataValueField = "cod_marca"
        ddl_MarcaCaldera1.DataBind()

        'Dim defaultItem As New ListItem
        'defaultItem.Value = "-1"
        'defaultItem.Text = "Seleccione una marca de caldera"
        'ddl_MarcaCaldera1.Items.Insert(0, defaultItem)


        Dim defaultItem As New ListItem
        Dim ds As New DataSet
        Dim cod_contrato As String = txt_Contrato.Text 'gv_Solicitudes.SelectedRow.Cells(2).Text.ToString()
        Dim i As Integer = 0
        Dim Esta As Boolean = False

        ds = objCalderasDB.GetCalderasPorContrato(cod_contrato)
        If Not ds.Tables(0).Rows.Count = 0 Then
            For i = 0 To ddl_MarcaCaldera1.Items.Count - 1
                If UCase(ddl_MarcaCaldera1.Items(i).Text) = UCase(ds.Tables(0).Rows(0).Item(4)) Then
                    Esta = True
                End If
            Next
            If Not Esta Then
                defaultItem = New ListItem
                defaultItem.Value = ds.Tables(0).Rows(0).Item(2)
                defaultItem.Text = ds.Tables(0).Rows(0).Item(3)
                ddl_MarcaCaldera1.Items.Insert(0, defaultItem)
            End If

            ddl_MarcaCaldera1.SelectedValue = ds.Tables(0).Rows(0).Item(2)
            Me.txt_ModeloCaldera1.Text = ds.Tables(0).Rows(0).Item(4)
            ComprobarCalderaINMERGAS()
        End If


        'ddl_MarcaCaldera1.SelectedValue = ds.Tables(0).Rows(0).Item(2)
        'Me.txt_ModeloCaldera1.Text = ds.Tables(0).Rows(0).Item(4)



    End Sub
    Protected Function acorta(ByVal texto As Object) As String
        If Len(texto.ToString()) > 31 Then
            Return texto.ToString().Substring(0, 30) & "..."
        Else
            Return texto.ToString()
        End If
    End Function

    Private Sub ComprobarCalderaINMERGAS()
        Try
            Dim i As Int16 = 0
            If ddl_MarcaCaldera1.SelectedItem.Text = "INMEGAS" Then
                If ddl_Estado.Items.Count > 0 Then
                    For i = 0 To ddl_Estado.Items.Count - 1
                        If ddl_Estado.Items(i).Text.IndexOf(" SAT") > 0 Then
                            ddl_Estado.Items.RemoveAt(i)
                            i = i - 1
                            If ddl_Estado.Items.Count <= 0 Then Exit For
                            If i < 0 Then i = 0

                        End If
                    Next
                End If
                Dim Script As String = "<script language='JavaScript'>"
                Script &= "alert('ATENCIÓN!!!\nESTA SOLICITUD NO PUEDE DERIVARSE AL SAT AL SER CALDERA INMERGAS.');</script>" 'AveriaAbierta()
                RegisterStartupScript("INMERGAS", Script)
            End If
        Catch ex As exception
        End Try
    End Sub

    Private Sub CargaDatosContrato(ByVal contrato As String)

        Dim objContratosDB As New ContratosDB()
        Dim dr As SqlDataReader = objContratosDB.GetSingleContrato(contrato)
        dr.Read()

        txt_Contrato.Text = dr("COD_CONTRATO_SIC").ToString()
        txt_Cliente.Text = dr("NOM_TITULAR").ToString() + " " + dr("APELLIDO1").ToString() + " " + dr("APELLIDO2").ToString()

        txt_Direccion.Text = dr("TIP_VIA_PUBLICA").ToString() + " " + dr("NOM_CALLE").ToString() + " " + dr("COD_PORTAL").ToString() + " " + dr("TIP_ESCALERA").ToString() + " " + dr("TIP_MANO").ToString() + " - " + dr("NOM_POBLACION").ToString() + ", " + dr("COD_POSTAL").ToString() + ", " + dr("NOM_PROVINCIA").ToString()
        CargaComboCalderas()

        ddl_Caldera.SelectedValue = dr("marca_caldera").ToString()

        Dim objProveedoresDB As New ProveedoresDB()

        Dim tipoSolicitud As String
        Dim subTipoSolicitud As String

        ' Gorka: cuando no hay valores en los combos se están cambiando elos datos entre
        ' el proveedor y el proveedor_avería.
        If (ddl_Tipo.SelectedValue = "" Or ddl_Tipo.SelectedValue = "-1" Or ddl_Tipo.SelectedIndex = -1) Then
            tipoSolicitud = "001"
        Else
            tipoSolicitud = ddl_Tipo.SelectedValue
        End If

        If (ddl_Subtipo.SelectedValue = "" Or ddl_Subtipo.SelectedValue = "-1" Or ddl_Subtipo.SelectedIndex = -1) Then
            subTipoSolicitud = "002"
        Else
            subTipoSolicitud = ddl_Subtipo.SelectedValue
        End If

        Dim Proveedores() As String
        If ddl_Tipo.SelectedValue = "-1" Then
            Proveedores = objProveedoresDB.GetProveedorPorTipoSubtipo(tipoSolicitud, subTipoSolicitud, txt_Contrato.Text).Split(";")
        Else
            Proveedores = objProveedoresDB.GetProveedorPorTipoSubtipoMantenimiento(tipoSolicitud, subTipoSolicitud, txt_Contrato.Text).Split(";")
        End If
        hidden_proveedor.Value = Proveedores(0)
        txt_Proveedor.Text = objProveedoresDB.ObtenerNombreProveedor(hidden_proveedor.Value) 'dr("nombre_proveedor").ToString()

        ''hidden_proveedor.Value = dr("proveedor").ToString()



        'Kintell 14/01/10

        'txt_TelProv.Text = dr("telefono_proveedor").ToString()
        'txt_EmailProv.Text = dr("email_proveedor").ToString()
        Dim Datos As New DataSet()
        If Not hidden_proveedor.Value = "" Then
            Datos = GetDatosProveedor(hidden_proveedor.Value)
            If Datos.Tables.Count > 0 Then
                txt_EmailProv.Text = Datos.Tables(0).Rows(0).Item("EMAIL")
                txt_TelProv.Text = Datos.Tables(0).Rows(0).Item("TELEFONO")
                txt_Proveedor.Text = Datos.Tables(0).Rows(0).Item("nombre")
            End If
        End If

        Dim ProveedorAveria As String = Proveedores(1)
        Datos = New DataSet()
        If Not ProveedorAveria = "" Then
            Datos = GetDatosProveedor(ProveedorAveria)
            If Datos.Tables.Count > 0 Then
                txt_EmailProAveria.Text = Datos.Tables(0).Rows(0).Item("EMAIL")
                txt_TelProvAveria.Text = Datos.Tables(0).Rows(0).Item("TELEFONO")
                txt_ProveedorAveria.Text = Datos.Tables(0).Rows(0).Item("nombre")
            End If
        End If
        '****************************************************

        txt_Estado.Text = dr("DES_ESTADO").ToString()
        txt_Urgencia.Text = dr("URGENCIA").ToString()


        Try
            txt_FechaLimite.Text = DateTime.Parse(dr("FEC_LIMITE_VISITA").ToString()).ToShortDateString()
        Catch ex As Exception
            txt_FechaLimite.Text = dr("FEC_LIMITE_VISITA").ToString()
        End Try

        'Kintell 23/04/2009
        CargaComboCalderasAbajo(txt_Contrato.Text)
    End Sub


    Public Function GetDatosProveedor(ByVal Proveedor As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetDatosProveedor", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterTipoSolicitud As New SqlParameter("@Proveedor", SqlDbType.NVarChar, 3)
        parameterTipoSolicitud.Value = Proveedor
        myCommand.SelectCommand.Parameters.Add(parameterTipoSolicitud)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Private Sub CargaSolicitudes(ByVal contrato As String)

        Dim objSolicitudesDB As New SolicitudesDB()
        Dim dsSolicitudes As DataSet = objSolicitudesDB.GetSolicitudesPorContrato(contrato, 1)


        Session("dsSolicitudes") = dsSolicitudes

        '' las pongo visible true porque sino no me hace el bind
        gv_Solicitudes.Columns(0).Visible = True
        gv_Solicitudes.Columns(1).Visible = True

        gv_Solicitudes.DataSource = dsSolicitudes
        gv_Solicitudes.DataBind()

        '' las oculto despues del databind para que me mantenga los datos
        'gv_Solicitudes.Columns(0).Visible = False
        gv_Solicitudes.Columns(1).Visible = False

        Me.txt_Observaciones.Text = ""

        CargaComboCalderasNuevo()
        ''Kintell 14/04/2009
        'Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        'Dim Sql As String
        'Dim id_solicitud As String = gv_Solicitudes.SelectedRow.Cells(1).Text
        'Sql = "Select id_solicitud, FechaLlamada from LlamadasClientes where id_solicitud=" & id_solicitud & " order by FechaLlamada desc"
        'Dim myDataSet As New DataSet()
        'Dim myCommand As New SqlDataAdapter(Sql, myConnection)

        'myCommand.Fill(myDataSet)
        'gv_LlamadasClientes.DataSource = myDataSet
        'gv_LlamadasClientes.DataBind()

        'Label2.Text = "Histórico de Llamadas: " & myDataSet.Tables(0).Rows.Count

    End Sub


    Private Sub CargaDatosSolicitud(ByVal id_solicitud As String)

        Session("SOLICITUD_SELECCIONADA") = id_solicitud
        Dim objSolicitudesDB As New SolicitudesDB()
        Dim dr As SqlDataReader = Nothing

        dr = objSolicitudesDB.GetSingleSolicitudes(id_solicitud, 1)
        dr.Read()

        '' Tipo solicitud
        If (Not ddl_Tipo.SelectedItem Is Nothing) Then
            ddl_Tipo.SelectedItem.Selected = False
        End If
        Try
            ddl_Tipo.Items.FindByValue(dr("Tipo_solicitud").ToString()).Selected = True
        Catch ex As Exception
        End Try


        'Kintell. 14/04/2009
        'Ponermos si es urgente o no la solicitud.
        Me.chkUrgente.Checked = dr("URGENTE").ToString()

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

        '' Subtipo solicitud
        CargaComboSubtipoSolicitud(ddl_Tipo.SelectedValue)

        If (Not ddl_Subtipo.SelectedItem Is Nothing) Then
            ddl_Subtipo.SelectedItem.Selected = False
        End If
        Try
            ddl_Subtipo.Items.FindByValue(dr("subtipo_solicitud").ToString()).Selected = True
        Catch ex As Exception
        End Try


        '' Estado solicitud
        CargaComboEstadoSolicitud(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue)

        If (Not ddl_Estado.SelectedItem Is Nothing) Then
            ddl_Estado.SelectedItem.Selected = False
        End If

        Dim estadoSolicitud As String = dr("Estado_solicitud").ToString()

        Try
            ddl_Estado.Items.FindByValue(estadoSolicitud).Selected = True
        Catch ex As Exception

            'Kintell 23/04/2009
            Dim myConnection1 As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
            Dim Sql1 As String
            Sql1 = "Select Codigo,Descripcion from Estado_solicitud where Codigo='" & estadoSolicitud & "'"
            Dim myDataSet1 As New DataSet()
            Dim myCommand1 As New SqlDataAdapter(Sql1, myConnection1)

            myCommand1.Fill(myDataSet1)
            Dim Nuevo As New WebControls.ListItem
            Nuevo.Value = myDataSet1.Tables(0).Rows(0).Item(0)
            Nuevo.Text = myDataSet1.Tables(0).Rows(0).Item(1)
            ddl_Estado.Items.Add(Nuevo)
            ddl_Estado.Items.FindByValue(myDataSet1.Tables(0).Rows(0).Item(0)).Selected = True

        End Try


        Dim solicitudCerrada As String() = {"016", "017"}
        If estadoSolicitud = "016" Or estadoSolicitud = "017" Then
            '' si la solicitud esta cerrada no se puede modificar
            btn_ModificarSolicitud.Enabled = False
            btnAviso.Enabled = False
            'Kintell 14/04/2009
            Me.btnLLAMADA.Enabled = False
        End If




        '' Motivo cancelacion
        If (Not ddl_MotivoCancel.SelectedItem Is Nothing) Then
            ddl_MotivoCancel.SelectedItem.Selected = False
        End If
        Try
            ddl_MotivoCancel.Items.FindByValue(dr("Mot_cancel").ToString()).Selected = True
        Catch ex As Exception
        End Try

        ValidaComboMotiviCancelacion(dr("Estado_solicitud").ToString())


        '' Tipo averia
        If (Not ddl_Averia.SelectedItem Is Nothing) Then
            ddl_Averia.SelectedItem.Selected = False
        End If
        Try
            ddl_Averia.Items.FindByValue(dr("cod_averia").ToString()).Selected = True
        Catch ex As Exception

        End Try
        'Tipo visita

        If InStr(UCase(ddl_Subtipo.SelectedItem.Text), UCase("Incorr")) > 0 Then
            CargaComboTiposVisitaIncorrecta()
        Else
            CargaComboTiposVisita()
        End If
        ddl_Visita.SelectedIndex = -1 '.Items(0).Selected = False
        'ddl_Visita.ClearSelection()


        If (Not ddl_Visita.SelectedItem Is Nothing) Then
            ddl_Visita.SelectedItem.Selected = False
        End If
        Try
            ddl_Visita.SelectedValue = dr("cod_averia").ToString() '.Selected = True
            Dim a As String = "1"
        Catch ex As Exception

        End Try

        txt_TelfContacto.Text = dr("telefono_contacto").ToString()
        txt_PersContacto.Text = dr("Persona_contacto").ToString()
        txt_ObservacionesAnteriores.Text = dr("Observaciones").ToString()


        'CargaComboCalderasNuevo()

        'Kintell 14/04/2009
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim Sql As String
        Sql = "Select id_solicitud, FechaLlamada from LlamadasClientes where id_solicitud=" & id_solicitud & " order by FechaLlamada desc"
        Dim myDataSet As New DataSet()
        Dim myCommand As New SqlDataAdapter(Sql, myConnection)

        myCommand.Fill(myDataSet)
        gv_LlamadasClientes.DataSource = myDataSet
        gv_LlamadasClientes.DataBind()

        Label2.Text = "Histórico de Llamadas: " & myDataSet.Tables(0).Rows.Count



        'Seleccionamos el proveedor.
        'Kintell 31/05/2011
        cmbProveedor.SelectedValue = dr("proveedor").ToString()


        'Kintell 08/06/2011
        'Seleccionamos tipo averia o tipo visita.
        If Not InStr(UCase(ddl_Subtipo.SelectedItem.Text), UCase("Aver")) > 0 Then
            If Not InStr(UCase(ddl_Subtipo.SelectedItem.Text), UCase("Aceptaci")) > 0 Then
                ddl_Averia.Visible = False
                lblDescrAveria.Visible = False
                'Kintell 07/06/2011
                ddl_Visita.Visible = True
                lblDescrVisita.Visible = True
            Else
                ddl_Averia.Visible = False
                lblDescrAveria.Visible = False
                'Kintell 07/06/2011
                ddl_Visita.Visible = False
                lblDescrVisita.Visible = False
            End If
        Else
            ddl_Averia.Visible = True
            lblDescrAveria.Visible = True
            'Kintell 07/06/2011
            ddl_Visita.Visible = False
            lblDescrVisita.Visible = False
        End If
    End Sub

    Private Sub CargaComboCalderasNuevo()

        ddl_MarcaCaldera.Items.Clear()

        Dim objCalderasDB As New CalderasDB()


        ddl_MarcaCaldera.SelectedValue = Nothing

        ddl_MarcaCaldera.DataSource = objCalderasDB.GetMarcaCalderas()
        ddl_MarcaCaldera.DataTextField = "descripcion"
        ddl_MarcaCaldera.DataValueField = "cod_marca"
        ddl_MarcaCaldera.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "Seleccione una marca de caldera"
        ddl_MarcaCaldera.Items.Insert(0, defaultItem)



        Dim ds As New DataSet
        Dim cod_contrato As String = txt_Contrato.Text 'gv_Solicitudes.SelectedRow.Cells(2).Text.ToString()
        Dim i As Integer = 0
        Dim Esta As Boolean = False

        ds = objCalderasDB.GetCalderasPorContrato(cod_contrato)
        If Not ds.Tables(0).Rows.Count = 0 Then
            For i = 0 To ddl_MarcaCaldera.Items.Count - 1
                If UCase(ddl_MarcaCaldera.Items(i).Text) = UCase(ds.Tables(0).Rows(0).Item(3)) Then
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


    Private Sub CargaHistorico(ByVal id_solicitud As String)

        Dim objHistoricoDB As New HistoricoDB()
        Dim dsHistorico As DataSet = objHistoricoDB.GetHistoricoSolicitud(id_solicitud, 1)


        gv_HistoricoSolicitudes.Columns(0).Visible = True
        gv_HistoricoSolicitudes.Columns(1).Visible = True
        gv_HistoricoSolicitudes.Columns(2).Visible = True

        gv_HistoricoSolicitudes.DataSource = dsHistorico
        gv_HistoricoSolicitudes.DataBind()


        'gv_HistoricoSolicitudes.Columns(0).Visible = False
        gv_HistoricoSolicitudes.Columns(1).Visible = False
        gv_HistoricoSolicitudes.Columns(2).Visible = False



    End Sub

    Private Sub CargaHistoricoCaracteristicas(ByVal id_movimiento As String)

        Dim objHistoricoDB As New HistoricoDB()
        Dim dsHistorico As DataSet = objHistoricoDB.GetHistoricoCaracteristicasPorMovimiento(id_movimiento, 1)

        gv_historicoCaracteristicas.Columns(0).Visible = True

        gv_historicoCaracteristicas.DataSource = dsHistorico
        gv_historicoCaracteristicas.DataBind()

        gv_historicoCaracteristicas.Columns(0).Visible = False

    End Sub

    Private Sub CargaComboCalderas()

        ddl_Caldera.Items.Clear()

        Dim objCalderasDB As New CalderasDB()

        ''ddl_Caldera.DataSource = objCalderasDB.GetCalderasPorContrato(contrato)
        ''ddl_Caldera.DataTextField = "Marca_Caldera"
        ''ddl_Caldera.DataValueField = "cod_marca"
        ddl_Caldera.DataSource = objCalderasDB.GetMarcaCalderas()
        ddl_Caldera.DataTextField = "descripcion"
        ddl_Caldera.DataValueField = "cod_marca"
        ddl_Caldera.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "Seleccione una caldera"
        ddl_Caldera.Items.Insert(0, defaultItem)


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
        defaultItem.Text = "Seleccione un tipo de solicitud"
        ddl_Tipo.Items.Insert(0, defaultItem)

    End Sub

    Private Sub CargaComboSubtipoSolicitud(ByVal cod_tipo_solicitud As String)

        ddl_Subtipo.Items.Clear()

        Dim objTipoSolicitudesDB As New TiposSolicitudDB()

        ddl_Subtipo.DataSource = objTipoSolicitudesDB.GetSubtipoSolicitudesPorTipo(cod_tipo_solicitud, 1)
        ddl_Subtipo.DataTextField = "descripcion"
        ddl_Subtipo.DataValueField = "codigo"
        ddl_Subtipo.DataBind()

        ''Kintell 21/06/2011
        ''Revision por precinte.
        'If ddl_Subtipo.Items.Count > 0 Then
        '    ddl_Subtipo.Items.RemoveAt(4)
        'End If

        If hdnBajaContrato.Value = "1" Then
            ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Avería Mantenimiento de Gas"))
            ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Solicitud de Visita SMG"))
            ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Revisión por Precinte"))
            ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Visita supervision"))
            ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Aceptacion de presupuesto"))
            Dim Valores() As String = hdnSubtipo6meses.Value.ToString().Split(";")
            'Dejamos meter solo si han tenido alguna de este tipo hace 6 meses
            If Valores.Length > 1 Then
                If Valores(0) = "0" Then
                    ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Avería incorrecta"))
                Else
                    If Valores(1) = "0" Then
                        ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Visita incorrecta"))
                    End If
                End If
            Else
                ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Avería incorrecta"))
                ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Visita incorrecta"))
            End If
            'If hdnSubtipo6meses.Value = "004" Then
            'ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Visita incorrecta"))
            'Else
            'ddl_Subtipo.Items.Remove(ddl_Subtipo.Items.FindByText("Avería incorrecta"))
            'End If
        End If

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "Seleccione un subtipo de solicitud"
        ddl_Subtipo.Items.Insert(0, defaultItem)


    End Sub

    Private Sub CargaComboEstadoSolicitud(ByVal cod_tipo As String, ByVal cod_subtipo As String)

        Dim objEstadoSolicitudesDB As New EstadosSolicitudDB()

        If Not Session("SOLICITUD_SELECCIONADA") = Nothing Then
            Dim objSolicitudesDB As New SolicitudesDB()
            Dim dr As SqlDataReader = Nothing
            dr = objSolicitudesDB.GetSingleSolicitudes(Session("SOLICITUD_SELECCIONADA"), 1)
            dr.Read()

            Dim cod_estado As String = dr("Estado_solicitud").ToString()

            dr.Close()
            ddl_Estado.Items.Clear()
            ddl_Estado.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudPorTipoSubtipoCanal(cod_tipo, cod_subtipo, cod_estado, 1)
            ddl_Estado.DataTextField = "descripcion"
            ddl_Estado.DataValueField = "codigo"
            ddl_Estado.DataBind()

            If InStr(UCase(ddl_Subtipo.SelectedItem.Text), UCase("Aver")) > 0 And ddl_Estado.Items.FindByText("Transferido a SAT") Is Nothing Then
                Dim TransferidoaSAT As New ListItem()
                TransferidoaSAT.Value = "024"
                TransferidoaSAT.Text = "Transferido a SAT"
                ddl_Estado.Items.Add(TransferidoaSAT)
            End If

        Else

            ddl_Estado.Items.Clear()

            ddl_Estado.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudPorTipoSubtipoCanal(cod_tipo, cod_subtipo, "000", 1)
            ddl_Estado.DataTextField = "descripcion"
            ddl_Estado.DataValueField = "codigo"
            ddl_Estado.DataBind()

            If Not (ddl_Estado.Items.Count = 0) And (ddl_Estado.Items.Count > 2) Then
                'ddl_Estado.Items.RemoveAt(0)
                'EN PRO
                ddl_Estado.Items.RemoveAt(2)
            End If
        End If
        If (ddl_Estado.Items.Count = 0) Then

            Dim defaultItem As New ListItem
            defaultItem.Value = "-1"
            defaultItem.Text = "Ninguno"
            ddl_Estado.Items.Insert(0, defaultItem)

            'Else
            '    ddl_Estado.Items.RemoveAt(0)
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
        defaultItem.Value = "-1"
        defaultItem.Text = "Seleccione un motivo de cancelacion"
        ddl_MotivoCancel.Items.Insert(0, defaultItem)


        '' por defecto esta desactivado
        ddl_MotivoCancel.Enabled = False




    End Sub

    Private Sub CargaComboTiposAveria()

        ddl_Averia.Items.Clear()

        Dim objTiposAveriaDB As New TiposAveriaDB()

        ddl_Averia.DataSource = objTiposAveriaDB.GetTiposAveria(1)
        ddl_Averia.DataTextField = "descripcion_averia"
        ddl_Averia.DataValueField = "cod_averia"
        ddl_Averia.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "Seleccione un tipo de averia"
        ddl_Averia.Items.Insert(0, defaultItem)

    End Sub

    Private Sub CargaComboTiposVisita()

        ddl_Visita.Items.Clear()

        Dim objTiposVisitaDB As New TiposVisitaDB()

        ddl_Visita.DataSource = objTiposVisitaDB.GetTiposVisita(1)
        ddl_Visita.DataTextField = "descripcion_averia"
        ddl_Visita.DataValueField = "cod_averia"
        ddl_Visita.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "Seleccione un tipo de visita"
        ddl_Visita.Items.Insert(0, defaultItem)

    End Sub

    Private Sub CargaComboProveedores()
        cmbProveedor.Items.Clear()



        Dim defaultItem As New ListItem

        'SIEL
        defaultItem = New ListItem
        defaultItem.Value = "SIE"
        defaultItem.Text = "SIEL"
        cmbProveedor.Items.Insert(0, defaultItem)
        'MGA
        defaultItem = New ListItem
        defaultItem.Value = "MGA"
        defaultItem.Text = "MGA"
        cmbProveedor.Items.Insert(0, defaultItem)
        'MAPFRE
        defaultItem = New ListItem
        defaultItem.Value = "MAP"
        defaultItem.Text = "MAPFRE"
        cmbProveedor.Items.Insert(0, defaultItem)
        'ICISA
        defaultItem = New ListItem
        defaultItem.Value = "ICI"
        defaultItem.Text = "ICISA"
        cmbProveedor.Items.Insert(0, defaultItem)
        'ACTIVAIS
        defaultItem = New ListItem
        defaultItem.Value = "ACT"
        defaultItem.Text = "ACTIVAIS"
        cmbProveedor.Items.Insert(0, defaultItem)
        ''Vacio
        'defaultItem = New ListItem
        'defaultItem.Value = "-1"
        'defaultItem.Text = "Seleccione un proveedor"
        'cmbProveedor.Items.Insert(0, defaultItem)


    End Sub


    Private Sub ValidaComboMotiviCancelacion(ByVal estadoSolicitud As String)
        Try
            'Kintell 12/12/2011
            CargaComboMotivosCancelacion(ddl_Subtipo.SelectedValue)

            Dim solicitudCancelada As String() = {"009", "010", "011", "012", "013", "014", "015"}

            If estadoSolicitud = "009" Or estadoSolicitud = "009" Or estadoSolicitud = "010" Or estadoSolicitud = "011" Or estadoSolicitud = "012" Or estadoSolicitud = "013" Or estadoSolicitud = "014" Or estadoSolicitud = "015" Then
                ddl_MotivoCancel.Enabled = True
            Else
                If Not ddl_MotivoCancel.SelectedItem Is Nothing Then
                    ddl_MotivoCancel.SelectedItem.Selected = False
                End If
                ddl_MotivoCancel.Items(0).Selected = True
                ddl_MotivoCancel.Enabled = False
            End If

            If estadoSolicitud = "001" Then
                'Estado Inicial.
                'Habilitar asignación de proveedor manualmente.
                lblProveedor.Visible = True
                cmbProveedor.Visible = True

                Dim contrato As String = Request.QueryString("con")
                Dim objProveedoresDB As New ProveedoresDB()
                Dim Proveedores() As String = objProveedoresDB.GetProveedorPorTipoSubtipo(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, contrato).Split(";")

                ''hidden_proveedor.Value = objProveedoresDB.GetProveedorPorTipoSubtipo(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, contrato)

                Dim proveedor As String = Mid(Proveedores(0), 1, 3)
                cmbProveedor.SelectedValue = proveedor.ToUpper()
            Else
                lblProveedor.Visible = False
                cmbProveedor.Visible = False
            End If

        Catch ex As Exception
            '' por si acaso
        End Try

    End Sub


    Private Sub ModoVisualizar()

        btn_AltaSolicitud.Enabled = False
        btn_ModificarSolicitud.Enabled = False
        btnAviso.Enabled = False
        btnLLAMADA.Enabled = False
        cmbProveedor.Enabled = False

    End Sub


    Private Sub ModoCrear()

        btn_AltaSolicitud.Enabled = True
        btn_ModificarSolicitud.Enabled = False
        btnAviso.Enabled = False
        btnLLAMADA.Enabled = False
        cmbProveedor.Enabled = False

    End Sub

    Private Sub ModoModificar()

        ddl_Tipo.Enabled = False
        ddl_Subtipo.Enabled = False

        btn_AltaSolicitud.Enabled = False
        btn_ModificarSolicitud.Enabled = True
        btnAviso.Enabled = True
        btnAviso.Enabled = True
        btnLLAMADA.Enabled = True
        cmbProveedor.Enabled = True

    End Sub

#End Region

#Region "Eventos"

    Protected Sub btn_AltaSolicitud_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AltaSolicitud.Click

        Try

            Dim contrato As String = Request.QueryString("con")
            Dim cod_tipo_solicitud As String = ddl_Tipo.SelectedValue
            Dim cod_subtipo_solicitud As String = ddl_Subtipo.SelectedValue
            Dim cod_estado As String = ddl_Estado.SelectedValue
            Dim telef_contacto As String = txt_TelfContacto.Text
            Dim pers_contacto As String = txt_PersContacto.Text
            Dim cod_averia As String = ddl_Averia.SelectedValue
            Dim cod_visita As String = ddl_Visita.SelectedValue
            Dim observaciones As String = "" 'txt_ObservacionesAnteriores.Text & Chr(13) & txt_Observaciones.Text

            Dim objProveedoresDB As New ProveedoresDB()
            Dim Proveedores() As String = objProveedoresDB.GetProveedorPorTipoSubtipo(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, contrato).Split(";")
            hidden_proveedor.Value = Proveedores(0)
            ''hidden_proveedor.Value = objProveedoresDB.GetProveedorPorTipoSubtipo(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, contrato)

            Dim proveedor As String = Mid(hidden_proveedor.Value, 1, 3)

            If cmbProveedor.Visible = True Then
                proveedor = cmbProveedor.SelectedValue
            End If


            Dim usuario As String = String.Empty

            'Comprobamos k si es cancelada haya seleccionado un motivo de cancelación.
            If Me.ddl_Estado.SelectedItem.Text = "Cancelada" Then
                If Me.ddl_MotivoCancel.SelectedValue = "-1" Then
                    Dim Script As String
                    Script = "<script language='JavaScript'>alert('Debe seleccionar un motivo de cancelación.');</script>"
                    Page.RegisterStartupScript("Error Fecha", Script)
                    Exit Sub
                End If
            End If


            If Not String.IsNullOrEmpty(telef_contacto) Then
                Dim Script As String = ""
                If Not Len(telef_contacto) = 9 Then
                    Script = "<script language='Javascript'>alert('El telefono debe de tener 9 dígitos');</script>"
                Else
                    If Mid(telef_contacto, 1, 1) = "7" Or Mid(telef_contacto, 1, 1) = "6" Or Mid(telef_contacto, 1, 1) = "9" Or Mid(telef_contacto, 1, 1) = "8" Then
                    Else
                        Script = "<script language='Javascript'>alert('El telefono debe de comenzar por el digito 6, 7, 9 o 8');</script>"
                    End If
                End If
                If Not Script = "" Then
                    Page.RegisterStartupScript("Error", Script)
                    Exit Sub
                End If
            End If



            Try
                usuario = Session("usuarioValido").ToString()
                Dim Horas As String = DateTime.Now.Hour.ToString()
                If Len(Horas) = 1 Then Horas = "0" & Horas
                Dim Minutos As String = DateTime.Now.Minute.ToString()
                If Len(Minutos) = 1 Then Minutos = "0" & Minutos

                observaciones = "[" & Mid(Now(), 1, 10) & "-" & Horas & ":" & Minutos & "] " & usuario & ": " & txt_Observaciones.Text & Chr(13) & txt_ObservacionesAnteriores.Text
            Catch ex As Exception

            End Try

            If Not String.IsNullOrEmpty(contrato) And cod_tipo_solicitud <> "-1" And cod_subtipo_solicitud <> "-1" And cod_estado <> "-1" And Not String.IsNullOrEmpty(telef_contacto) And Not String.IsNullOrEmpty(pers_contacto) And Not String.IsNullOrEmpty(proveedor) Then
                If (cod_subtipo_solicitud = "1" Or cod_subtipo_solicitud = "001" Or cod_subtipo_solicitud = "4" Or cod_subtipo_solicitud = "004") And cod_averia = "-1" Then
                    lbl_mensajes.Text = "El código de averia es obligatorio."
                    Exit Sub
                End If

                If (cod_subtipo_solicitud = "2" Or cod_subtipo_solicitud = "002" Or cod_subtipo_solicitud = "3" Or cod_subtipo_solicitud = "003") And cod_visita = "-1" Then
                    lbl_mensajes.Text = "El código de visita es obligatorio."
                    Exit Sub
                End If

                Dim objSolicitudesDB As New SolicitudesDB()
                'If cod_averia = -1 Then cod_averia = System.DBNull.Value.ToString

                Dim id_solicitud As String

                If (ddl_Averia.Visible) Then
                    id_solicitud = objSolicitudesDB.AddSolicitud(contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_averia, observaciones, proveedor, Me.chkUrgente.Checked, False)
                Else
                    id_solicitud = objSolicitudesDB.AddSolicitud(contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_visita, observaciones, proveedor, Me.chkUrgente.Checked, False)
                End If

                'Kintell 22/11/2011 Tema HORARIO CONTACTO.
                ActualizarHorariocontacto("UPDATE MANTENIMIENTO SET HORARIO_CONTACTO='" & cmbHorarioContacto.SelectedValue & "' WHERE COD_CONTRATO_SIC='" & contrato & "'")


                'Kintell 08/04/2007
                'Comprobamos k existe o no una caldera para este contrato, para insertar una nueva o modificar la existente.
                Dim objCalderasDB As New CalderasDB()

                Dim ds As New DataSet
                ds = objCalderasDB.GetCalderasPorContrato(contrato)
                If ds.Tables(0).Rows.Count = 0 Then
                    objSolicitudesDB.InsertarCalderaContrato(contrato, Me.ddl_MarcaCaldera1.SelectedValue.ToString, txt_ModeloCaldera1.Text)
                Else
                    objSolicitudesDB.ActualizarCalderaContrato(contrato, Me.ddl_MarcaCaldera1.SelectedValue.ToString, txt_ModeloCaldera1.Text, "", "", 0, 0)
                End If


                Dim objHistoricoDB As New HistoricoDB()
                objHistoricoDB.AddHistoricoSolicitud(id_solicitud, "001", usuario, cod_estado, observaciones,proveedor)

                Dim url_info_visita As String = ConfigurationManager.AppSettings("url_info_visita").ToString()
                url_info_visita += "?contrato=" + contrato

                ClientScript.RegisterStartupScript(Page.GetType(), "PopupScript", "<script language='javascript'>alert('Se ha dado de alta la solicitud " + id_solicitud + "'); document.location.href('" + url_info_visita + "');</script>")

            Else
                lbl_mensajes.Text = "No se ha dado de alta su solicitud. Todos los campos excepto 'Observaciones' son obligatorios."
            End If

        Catch ex As Exception
            lbl_mensajes.Text = "No se ha podido dar de alta la solicitud. Codigo: " + ex.Message
        End Try

    End Sub

    Protected Sub btn_ModificarSolicitud_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ModificarSolicitud.Click

        Try

            If Not ddl_Estado.SelectedValue = "-1" Then



                'Comprobamos k si es cancelada haya seleccionado un motivo de cancelación.
                If Me.ddl_Estado.SelectedItem.Text = "Cancelada" Then
                    If Me.ddl_MotivoCancel.SelectedValue = "-1" Then
                        Dim Script As String
                        Script = "<script language='JavaScript'>alert('Debe seleccionar un motivo de cancelación.');</script>"
                        Page.RegisterStartupScript("Error Fecha", Script)
                        Exit Sub
                    End If
                End If



                Dim id_solicitud As String = String.Empty
                If (gv_Solicitudes.SelectedRow Is Nothing) Then
                    If Not String.IsNullOrEmpty(Request.QueryString("sol")) Then
                        id_solicitud = Request.QueryString("sol")
                        'este caso solo se da si alguien viene de otra pantalla a modificar los datos
                        'de una solicitud concreta sin utilizar el grid
                    End If
                Else
                    id_solicitud = gv_Solicitudes.SelectedRow.Cells(1).Text
                End If
                hdnIdSolicitud.Value = id_solicitud
                Dim contrato As String = Request.QueryString("con")
                Dim cod_tipo_solicitud As String = ddl_Tipo.SelectedValue
                Dim cod_subtipo_solicitud As String = ddl_Subtipo.SelectedValue
                Dim cod_estado As String = ddl_Estado.SelectedValue
                Dim telef_contacto As String = txt_TelfContacto.Text
                Dim pers_contacto As String = txt_PersContacto.Text
                Dim cod_averia As String = ddl_Averia.SelectedValue
                Dim cod_visita As String = ddl_Visita.SelectedValue
                Dim observaciones As String = "" 'txt_ObservacionesAnteriores.Text & Chr(13) & txt_Observaciones.Text


                Dim objProveedoresDB As New ProveedoresDB()
                Dim Proveedores() As String = objProveedoresDB.GetProveedorPorTipoSubtipoMantenimiento(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, contrato).Split(";")
                hidden_proveedor.Value = Proveedores(0)
                Dim proveedor As String = hidden_proveedor.Value

                If cmbProveedor.Enabled = True Then
                    proveedor = cmbProveedor.SelectedValue
                End If

                Dim mot_cancel As String = ddl_MotivoCancel.SelectedValue

                If Not String.IsNullOrEmpty(telef_contacto) Then
                    Dim Script As String = ""
                    If Not Len(telef_contacto) = 9 Then
                        Script = "<script language='Javascript'>alert('El telefono debe de tener 9 dígitos');</script>"
                    Else
                        If Mid(telef_contacto, 1, 1) = "6" Or Mid(telef_contacto, 1, 1) = "9" Or Mid(telef_contacto, 1, 1) = "8" Then
                        Else
                            Script = "<script language='Javascript'>alert('El telefono debe de comenzar por el digito 6, 9 o 8');</script>"
                        End If
                    End If
                    If Not Script = "" Then
                        Page.RegisterStartupScript("Error", Script)
                        Exit Sub
                    End If
                End If

                Dim usuario As String = String.Empty

                Try
                    usuario = Session("usuarioValido").ToString()
                Catch ex As Exception

                End Try

                Dim objSolicitudesDB As New SolicitudesDB()

                'Kintell 22/04/2009
                'observaciones = txt_ObservacionesAnteriores.Text & Chr(13) & txt_Observaciones.Text
                Dim Horas As String = DateTime.Now.Hour.ToString()
                Horas.PadLeft(2, "0")
                Dim Minutos As String = DateTime.Now.Minute.ToString()
                Minutos.PadLeft(2, "0")

                If Not txt_Observaciones.Text = "" Then
                    observaciones = "[" & Mid(Now(), 1, 10) & "-" & Horas & ":" & Minutos & "] " & usuario & ": " & txt_Observaciones.Text & Chr(13) & txt_ObservacionesAnteriores.Text

                    objSolicitudesDB.UpdateObservacionesSolicitud(id_solicitud, observaciones)
                Else
                    observaciones = txt_ObservacionesAnteriores.Text
                End If




                'Kintell 14/04/2009.
                'Actualizamos el estado de Urgente.
                If ddl_Averia.Visible Then
                    objSolicitudesDB.UpdateSolicitud(id_solicitud, contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_averia, observaciones, proveedor, mot_cancel, Me.chkUrgente.Checked, False)
                Else
                    objSolicitudesDB.UpdateSolicitud(id_solicitud, contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_visita, observaciones, proveedor, mot_cancel, Me.chkUrgente.Checked, False)
                End If

                'Kintell 22/11/2011 Tema HORARIO CONTACTO.
                ActualizarHorariocontacto("UPDATE MANTENIMIENTO SET HORARIO_CONTACTO='" & cmbHorarioContacto.SelectedValue & "' WHERE COD_CONTRATO_SIC='" & contrato & "'")




                'Kintell 08/04/2009
                'Comprobamos k existe o no una caldera para este contrato, para insertar una nueva o modificar la existente.
                Dim objCalderasDB As New CalderasDB()

                Dim ds As New DataSet
                ds = objCalderasDB.GetCalderasPorContrato(contrato)
                If ds.Tables(0).Rows.Count = 0 Then
                    objSolicitudesDB.InsertarCalderaContrato(contrato, Me.ddl_MarcaCaldera1.SelectedValue.ToString, txt_ModeloCaldera1.Text)
                Else
                    objSolicitudesDB.ActualizarCalderaContrato(contrato, Me.ddl_MarcaCaldera1.SelectedValue.ToString, txt_ModeloCaldera1.Text, "", "", 0, 0)
                End If




                Dim objHistoricoDB As New HistoricoDB()
                objHistoricoDB.AddHistoricoSolicitud(id_solicitud, "002", usuario, cod_estado, observaciones,proveedor)


                'despues de modificar recargamos el datagrid
                CargaSolicitudes(contrato)


                '***********************************************************
                'Cargamos los datos modificados de la solicitud.
                CargaDatosSolicitud(id_solicitud)
                CargaHistorico(id_solicitud)
                '***********************************************************

                ClientScript.RegisterStartupScript(Page.GetType(), "PopupScript", "<script language='javascript'>alert('Se ha modificado la solicitud'); </script>")


            Else
                lbl_mensajes.Text = "Debe seleccionar un estado de solicitud."
            End If

        Catch ex As Exception
            lbl_mensajes.Text = "No se ha podido modificar la solicitud. Codigo: " + ex.Message
        End Try
    End Sub

    'Protected Sub gv_Solicitudes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Solicitudes.PageIndexChanging

    '    Try


    '        Dim dsSolicitudes As DataSet = Session("dsSolicitudes")


    '        '' las pongo visible true porque sino no me hace el bind
    '        gv_Solicitudes.Columns(0).Visible = True
    '        gv_Solicitudes.Columns(1).Visible = True

    '        gv_Solicitudes.PageIndex = e.NewPageIndex

    '        gv_Solicitudes.DataSource = dsSolicitudes
    '        gv_Solicitudes.DataBind()

    '        '' las oculto despues del databind para que me mantenga los datos
    '        gv_Solicitudes.Columns(0).Visible = False
    '        gv_Solicitudes.Columns(1).Visible = False

    '    Catch ex As Exception
    '        lbl_mensajes.Text = "No se ha podido recargar el grid"
    '    End Try

    'End Sub

    Protected Sub gv_Solicitudes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Solicitudes.SelectedIndexChanged

        Dim id_solicitud As String = gv_Solicitudes.SelectedRow.Cells(1).Text
        CargaDatosSolicitud(id_solicitud)
        CargaHistorico(id_solicitud)

    End Sub
    Protected Sub gv_Solicitudes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Solicitudes.RowDataBound
        ' FORMATEA ROWS
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' ASIGNA EVENTOS
            'Si no es para crear nueva solicitud permitimos que seleccione un registro dl grid.
            Dim modo As String = Request.QueryString("mod")
            If Not modo = "c" Then
                e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")
                e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")
                e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_Solicitudes, "Select$" + e.Row.RowIndex.ToString)

                For i = 0 To e.Row.Cells.Count - 1
                    'e.Row.Cells(i).ToolTip = DirectCast(DirectCast(DirectCast(e.Row.Cells(15), System.Web.UI.WebControls.TableCell).Controls(1), System.Web.UI.Control), System.Web.UI.WebControls.Label).ToolTip 'e.Row.Cells(15).Text
                    e.Row.Cells(i).Attributes("style") &= "cursor:hand;"
                Next
            End If
        End If


    End Sub

    Protected Sub gv_HistoricoSolicitudes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_HistoricoSolicitudes.PageIndexChanging
        Try
            Dim id_solicitud As String = gv_Solicitudes.Rows(0).Cells(1).Text  'SelectedRow.Cells(1).Text
            Dim objHistoricoDB As New HistoricoDB()
            Dim dsHistorico As DataSet = objHistoricoDB.GetHistoricoSolicitud(id_solicitud, 1)


            gv_HistoricoSolicitudes.Columns(0).Visible = True
            gv_HistoricoSolicitudes.Columns(1).Visible = True
            gv_HistoricoSolicitudes.Columns(2).Visible = True

            gv_HistoricoSolicitudes.DataSource = dsHistorico
            gv_HistoricoSolicitudes.PageIndex = e.NewPageIndex
            gv_HistoricoSolicitudes.DataBind()


            'gv_HistoricoSolicitudes.Columns(0).Visible = False
            gv_HistoricoSolicitudes.Columns(1).Visible = False
            gv_HistoricoSolicitudes.Columns(2).Visible = False
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub gv_HistoricoSolicitudes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_HistoricoSolicitudes.RowDataBound
        ' FORMATEA ROWS
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' ASIGNA EVENTOS
            e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")
            e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_HistoricoSolicitudes, "Select$" + e.Row.RowIndex.ToString)
        End If


    End Sub

    Protected Sub ddl_Tipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Tipo.SelectedIndexChanged

        CargaComboSubtipoSolicitud(ddl_Tipo.SelectedValue)
        CargaComboEstadoSolicitud(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue)
        ValidaComboMotiviCancelacion(ddl_Estado.SelectedValue)

        'KINTELL.
        '10/05/2011.
        'PONER AUTOMATICAMENTE URGENTE.
        'DESHABILITAMOS PARA QUE NO PUEDA CAMBIARLO ¿?.



    End Sub

    Private Sub CargaComboTiposVisitaIncorrecta()

        ddl_Visita.Items.Clear()

        Dim objTiposVisitaDB As New TiposVisitaDB()

        ddl_Visita.DataSource = objTiposVisitaDB.GetTiposVisitaIncorrecta(1)
        ddl_Visita.DataTextField = "descripcion_averia"
        ddl_Visita.DataValueField = "cod_averia"
        ddl_Visita.DataBind()

        Dim defaultItem As New ListItem
        defaultItem.Value = "-1"
        defaultItem.Text = "Seleccione un tipo de visita"
        ddl_Visita.Items.Insert(0, defaultItem)

    End Sub

    Protected Sub ddl_Subtipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Subtipo.SelectedIndexChanged
        If Not InStr(UCase(ddl_Subtipo.SelectedItem.Text), UCase("Aver")) > 0 Then
            If Not InStr(UCase(ddl_Subtipo.SelectedItem.Text), UCase("Aceptaci")) > 0 Then
                ddl_Averia.Visible = False
                lblDescrAveria.Visible = False
                'Kintell 07/06/2011
                ddl_Visita.Visible = True
                lblDescrVisita.Visible = True

                If InStr(UCase(ddl_Subtipo.SelectedItem.Text), UCase("Incorr")) > 0 Then
                    CargaComboTiposVisitaIncorrecta()
                Else
                    CargaComboTiposVisita()
                End If
            Else
                ddl_Averia.Visible = False
                lblDescrAveria.Visible = False
                'Kintell 07/06/2011
                ddl_Visita.Visible = False
                lblDescrVisita.Visible = False
            End If
        Else
            ddl_Averia.Visible = True
            lblDescrAveria.Visible = True
            'Kintell 07/06/2011
            ddl_Visita.Visible = False
            lblDescrVisita.Visible = False


        End If
        CargaComboEstadoSolicitud(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue)
        ValidaComboMotiviCancelacion(ddl_Estado.SelectedValue)
        If ddl_Subtipo.SelectedItem.Value = "004" Then
            chkUrgente.Checked = True
        Else
            chkUrgente.Checked = False
        End If
        chkUrgente.Enabled = False
    End Sub

    Protected Sub ddl_Estado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Estado.SelectedIndexChanged

        ValidaComboMotiviCancelacion(ddl_Estado.SelectedValue)

    End Sub

    Protected Sub gv_HistoricoSolicitudes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_HistoricoSolicitudes.SelectedIndexChanged
        Dim id_movimiento As String = gv_HistoricoSolicitudes.SelectedRow.Cells(1).Text
        CargaHistoricoCaracteristicas(id_movimiento)

    End Sub
#End Region

    Protected Sub btnLLAMADA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLLAMADA.Click
        Try
            Dim id_solicitud As String = String.Empty
            If (gv_Solicitudes.SelectedRow Is Nothing) Then
                If Not String.IsNullOrEmpty(Request.QueryString("sol")) Then
                    id_solicitud = Request.QueryString("sol")
                    'este caso solo se da si alguien viene de otra pantalla a modificar los datos
                    'de una solicitud concreta sin utilizar el grid
                End If
            Else
                id_solicitud = gv_Solicitudes.SelectedRow.Cells(1).Text
            End If


            Dim Sql As String = ""
            Dim myDataSet As New DataSet
            Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))

            Sql = "Update Solicitudes set NLLAMADA= NLLAMADA + 1 where id_solicitud=" & id_solicitud
            Dim myCommand As New SqlCommand(Sql, myConnection)



            myConnection.Open()
            Try
                myCommand.ExecuteNonQuery()

                Sql = "insert into LlamadasClientes(Id_solicitud,FechaLlamada) values(" & id_solicitud & ",CURRENT_TIMESTAMP)"
                Dim myCommand2 As New SqlCommand(Sql, myConnection)
                myCommand2.ExecuteNonQuery()

                myConnection.Close()
            Catch ex As Exception
            Finally
                If (myConnection.State = ConnectionState.Open) Then
                    myConnection.Close()
                End If
            End Try


            'Dim objHistoricoDB As New HistoricoDB()
            'objHistoricoDB.AddHistoricoSolicitud(id_solicitud, "002", usuario, cod_estado, observaciones)


            'despues de modificar recargamos el datagrid

            Dim contrato As String = Request.QueryString("con")
            CargaSolicitudes(contrato)


            'Kintell 14/04/2009
            myConnection = New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
            Sql = "Select id_solicitud, FechaLlamada from LlamadasClientes where id_solicitud=" & id_solicitud & " order by FechaLlamada desc"
            myDataSet = New DataSet()
            Dim myCommand1 As New SqlDataAdapter(Sql, myConnection)

            myCommand1.Fill(myDataSet)
            gv_LlamadasClientes.DataSource = myDataSet
            gv_LlamadasClientes.DataBind()

            Label2.Text = "Histórico de Llamadas: " & myDataSet.Tables(0).Rows.Count





            ClientScript.RegisterStartupScript(Page.GetType(), "PopupScript", "<script language='javascript'>alert('Se ha archivado la llamada del cliente'); </script>")

        Catch ex As Exception
            lbl_mensajes.Text = "No se ha podido modificar la solicitud. Codigo: " + ex.Message
        End Try
    End Sub

    Private Sub ActualizarHorariocontacto(ByVal Sql As String)
        Dim Db As SolicitudesDB = New SolicitudesDB()
        Db.EjecutarSentencia(Sql)
    End Sub

    Protected Sub gv_LlamadasClientes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_LlamadasClientes.PageIndexChanging
        Try
            'Kintell 14/04/2009
            Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
            Dim Sql As String

            Sql = "Select id_solicitud, FechaLlamada from LlamadasClientes where id_solicitud=" & Session("SOLICITUD_SELECCIONADA") & " order by FechaLlamada desc"
            Dim myDataSet As New DataSet()
            Dim myCommand As New SqlDataAdapter(Sql, myConnection)

            myCommand.Fill(myDataSet)
            gv_LlamadasClientes.DataSource = myDataSet


            Label2.Text = "Histórico de Llamadas: " & myDataSet.Tables(0).Rows.Count

            gv_LlamadasClientes.PageIndex = e.NewPageIndex
            gv_LlamadasClientes.DataBind()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Dim url_info_visita As String = ConfigurationManager.AppSettings("url_info_visita").ToString()
        Response.Redirect(url_info_visita & "?contrato=" & txt_Contrato.Text, False)
    End Sub

    Protected Sub btn_VolverBoton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_VolverBoton.Click
        Dim url_info_visita As String = ConfigurationManager.AppSettings("url_info_visita").ToString()
        Response.Redirect(url_info_visita & "?contrato=" & txt_Contrato.Text, False)
    End Sub

    Protected Sub ddl_MotivoCancel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_MotivoCancel.SelectedIndexChanged

    End Sub

    Protected Sub ddl_Averia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Averia.SelectedIndexChanged

        If ddl_Subtipo.SelectedItem.Value = "004" Then
            chkUrgente.Checked = True
        Else
            If ddl_Subtipo.SelectedItem.Value = "001" Then
                chkUrgente.Checked = False
                If ddl_Averia.SelectedItem.Value = "16 " Or ddl_Averia.SelectedItem.Value = "1  " Or ddl_Averia.SelectedItem.Value = "2  " Or ddl_Averia.SelectedItem.Value = "3  " Or ddl_Averia.SelectedItem.Value = "4  " Then
                    chkUrgente.Checked = True
                End If
            End If
        End If
    End Sub

    Protected Sub ddl_Visita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Visita.SelectedIndexChanged
        If ddl_Subtipo.SelectedItem.Value = "004" Then
            chkUrgente.Checked = True
        Else
            If Not ddl_Subtipo.SelectedItem.Value = "001" Then
                chkUrgente.Checked = False
                If ddl_Visita.SelectedItem.Value = "20 " Or ddl_Visita.SelectedItem.Value = "18 " Or ddl_Visita.SelectedItem.Value = "22 " Or ddl_Visita.SelectedItem.Value = "23 " Or ddl_Visita.SelectedItem.Value = "19 " Or ddl_Visita.SelectedItem.Value = "21 " Then
                    chkUrgente.Checked = True
                End If
            End If
        End If
    End Sub

    Protected Sub btnAviso_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAviso.Click
        Dim Destino As String = ""
        Dim Datos As New DataSet()
        Datos = GetDatosProveedor(cmbProveedor.SelectedValue)
        If Datos.Tables.Count > 0 Then
            Destino = Datos.Tables(0).Rows(0).Item("email_aviso")
        End If
        Dim Mostrar As String = "<script type='text/javascript'>MostrarAvisos1('" & hdnIdSolicitud.Value & "','" & Destino & "');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AVISO", Mostrar, False)
    End Sub

End Class
