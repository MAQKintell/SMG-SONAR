Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports Iberdrola.SMG.BLL

Imports Iberdrola.Commons.Web

Partial Class Visitas_info_visita
    Inherits System.Web.UI.Page
    Private ds As DataSet


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '******************************************************************************************
        'Contador Contratos 09/12/2009
        If Not IsPostBack Then
            lblNumeroContratos.Text = ObtenerNumerosContratos()
        End If
        '******************************************************************************************
        Try
            Dim usuario As String = Session("usuarioValido").ToString()
        Catch ex As Exception
            Dim redirectURL As String = ConfigurationManager.AppSettings("url_Login").ToString()
            Response.Redirect(redirectURL, False)
        End Try

        ' controlamos que el perfil de la página es correcto 4 ó 3
        Dim IdPerfil As String = Session("IdPerfil")
        Dim perfilValido As Boolean = False

        If (String.IsNullOrEmpty(IdPerfil)) Then
            perfilValido = False
        Else
            If "4".Equals(IdPerfil) Or "3".Equals(IdPerfil) Then
                perfilValido = True
            Else
                perfilValido = False
            End If
        End If

        If Not perfilValido Then
            Dim redirectURL As String = ConfigurationManager.AppSettings("url_Login").ToString()
            Response.Redirect(redirectURL, False)
        End If



        If Not String.IsNullOrEmpty(Request.QueryString("contrato")) Then

            Dim contrato As String = Request.QueryString("contrato")

            If Not Me.IsPostBack Then
                txt_Contrato.Text = contrato
                lbl_error.Text = ""
                lbl_Mensaje.Text = ""

                'Kintell 08/09/2009
                'Buscamos el proveedor k sea logeado para controlar k solo pueda ver las suyas.
                Dim usuario As String = Session("usuarioValido").ToString()
                Dim objUsuariosDB As New UsuariosDB()
                Dim dr As SqlDataReader = objUsuariosDB.GetProveedorUsuario(usuario)
                dr.Read()
                Dim cod_proveedor As String = dr("proveedor").ToString()
                Dim nombre_proveedor As String = dr("nombre").ToString()

                ConVisitas(contrato, cod_proveedor)

                CargaSolicitudes(contrato)
            End If


        End If

        PonerFoco(txt_Contrato)

    End Sub

#Region "Ya estaba"


    Protected Sub btn_BAvanzada_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BAvanzada.Click
        Response.Redirect("Busqueda.aspx")
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("Solicitudes.aspx")
    End Sub

    Protected Sub btn_Consultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Consultar.Click

        'txt_EmailProDet = Nothing
        'txt_EstadoDet = Nothing
        lbl_Mensaje.Text = ""
        lbl_error.Text = ""
        Dim contrato As String = ""
        '11/08/2010
        'Kintell Desactivamos el botón de baja de facturación (tema CUPS).
        btnBajaContrato.Enabled = False
        '**************************
        contrato = txt_Contrato.Text.Trim()

        Try

            'Kintell 08/09/2009
            'Buscamos el proveedor k sea logeado para controlar k solo pueda ver las suyas.
            Dim usuario As String = Session("usuarioValido").ToString()
            Dim objUsuariosDB As New UsuariosDB()
            Dim dr As SqlDataReader = objUsuariosDB.GetProveedorUsuario(usuario)
            dr.Read()
            Dim cod_proveedor As String = dr("proveedor").ToString()
            Dim nombre_proveedor As String = dr("nombre").ToString()

            ConVisitas(contrato, cod_proveedor)
            contrato = txt_ContratoDet.Text.Trim()
            txt_Contrato.Text = contrato
            '*********************************************************************
        Catch ex As Exception

        End Try



        CargaSolicitudes(contrato)



    End Sub

    Protected Sub gv_Solicitudes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Solicitudes.RowDataBound
        ' FORMATEA ROWS
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' ASIGNA EVENTOS
            e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")
            e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")
            e.Row.Attributes("OnClick") = "Resaltar_OnOff(this," & e.Row.RowIndex.ToString & ");"
            'e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_Solicitudes, "Select$" + e.Row.RowIndex.ToString)

            For i = 0 To e.Row.Cells.Count - 1
                'e.Row.Cells(i).ToolTip = e.Row.Cells(15).Text
                e.Row.Cells(i).Attributes("style") &= "cursor:hand;"
                
                If i = 8 Then
                    Dim objSolicitudesDB As New SolicitudesDB()

                    e.Row.Cells(i).Text = objSolicitudesDB.ObtenerProveedorUltimoMovimientoSolicitud(e.Row.Cells(1).Text)
                    If e.Row.Cells(i).Text = "d" Then
                        e.Row.Cells(i).Text = "ADMINISTRACION"
                    End If
                End If
                
            Next
        End If


    End Sub

    Private Sub ConVisitas(ByVal contrato As String, ByVal CodigoProveedor As String)

        btn_CrearSolicitud.Visible = True
        btn_VerSolicitud.Visible = True
        btn_ModificarSolicitud.Visible = True
        lbl_Solicitudes.Visible = True
        gv_Solicitudes.Visible = True

        If Not String.IsNullOrEmpty(contrato) Then


        Else

            contrato = txt_Contrato.Text

        End If

        ''        Dim connectionString As String = _
        ''WebConfigurationManager.ConnectionStrings("OPCOMCMD").ConnectionString
        ''        Dim selectSQL As String = "SELECT dbo.MANTENIMIENTO.COD_CONTRATO_SIC, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.NOM_TITULAR, dbo.MANTENIMIENTO.APELLIDO1, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.APELLIDO2, dbo.MANTENIMIENTO.TIP_VIA_PUBLICA, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.NOM_CALLE, dbo.MANTENIMIENTO.NOM_POBLACION, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.NOM_PROVINCIA, dbo.MANTENIMIENTO.COD_PORTAL, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.TIP_PISO, dbo.MANTENIMIENTO.TIP_MANO, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.COD_POSTAL, dbo.MANTENIMIENTO.PROVEEDOR, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.DES_ESTADO, dbo.MANTENIMIENTO.URGENCIA, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.FEC_LIMITE_VISITA, dbo.MANTENIMIENTO.T1, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.T2, dbo.MANTENIMIENTO.T5, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.FEC_ALTA_SERVICIO, dbo.MANTENIMIENTO.FEC_BAJA_SERVICIO, "
        ''        selectSQL &= "dbo.MANTENIMIENTO.OBSERVACIONES "
        ''        selectSQL &= "FROM dbo.MANTENIMIENTO LEFT JOIN dbo.VISITAS "
        ''        selectSQL &= "ON dbo.VISITAS.COD_CONTRATO = dbo.MANTENIMIENTO.COD_CONTRATO_SIC "
        ''        selectSQL &= "WHERE dbo.MANTENIMIENTO.COD_CONTRATO_SIC = '" & contrato & "' "
        ''        selectSQL &= "ORDER BY dbo.VISITAS.FEC_LIMITE_VISITA DESC"

        Dim connectionString As String = _
        WebConfigurationManager.ConnectionStrings("OPCOMCMD").ConnectionString

        Dim selectSQL As String = "SELECT DISTINCT MANTENIMIENTO.COD_CONTRATO_SIC, "
        selectSQL &= "MANTENIMIENTO.NOM_TITULAR, MANTENIMIENTO.APELLIDO1, "
        selectSQL &= "MANTENIMIENTO.APELLIDO2,MANTENIMIENTO.TIP_VIA_PUBLICA, "
        selectSQL &= "MANTENIMIENTO.NOM_CALLE, poblacion.nombre as NOM_POBLACION, "
        selectSQL &= "provincia.nombre as NOM_PROVINCIA, MANTENIMIENTO.COD_PORTAL, "
        selectSQL &= "MANTENIMIENTO.TIP_PISO, MANTENIMIENTO.TIP_MANO, "
        selectSQL &= "MANTENIMIENTO.COD_POSTAL, MANTENIMIENTO.PROVEEDOR, "
        selectSQL &= "MANTENIMIENTO.ESTADO_CONTRATO as DES_ESTADO, tipo_urgencia.DESC_TIPO_URGENCIA as URGENCIA, "
        selectSQL &= "MANTENIMIENTO.FEC_LIMITE_VISITA, MANTENIMIENTO.T1, "
        selectSQL &= "MANTENIMIENTO.T2, MANTENIMIENTO.T5, "
        selectSQL &= "MANTENIMIENTO.FEC_ALTA_SERVICIO, MANTENIMIENTO.FEC_BAJA_SERVICIO, "
        selectSQL &= "MANTENIMIENTO.OBSERVACIONES "
        selectSQL &= ",MANTENIMIENTO.PROVEEDOR_AVERIA "
        selectSQL &= ",VISITA.FEC_LIMITE_VISITA "
        selectSQL &= ",MANTENIMIENTO.FECHA_HASTA_FACTURACION "
        selectSQL &= ",MANTENIMIENTO.BCS as BCS "
        selectSQL &= ",MANTENIMIENTO.CUPS as CUPS "
        selectSQL &= "FROM MANTENIMIENTO "
        selectSQL &= "INNER JOIN RELACION_CUPS_CONTRATO RCC ON RCC.CUPS=MANTENIMIENTO.CUPS "
        selectSQL &= "LEFT JOIN VISITA ON dbo.VISITA.COD_CONTRATO = MANTENIMIENTO.COD_CONTRATO_SIC "
        '***********************************************************************************************
        selectSQL &= "inner join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        selectSQL &= "inner join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia) "
        '***********************************************************************************************

        '***********************************************************************************************
        '30/05/2011
        selectSQL &= "left join tipo_urgencia on tipo_urgencia.ID_TIPO_URGENCIA=visita.ID_TIPO_URGENCIA "
        '***********************************************************************************************
        '08/08/2010
        'Kintell por el tema de busqueda por CUPS.
        'selectSQL &= "WHERE MANTENIMIENTO.COD_CONTRATO_SIC = '" & contrato & "' "
        selectSQL &= "WHERE RCC.COD_CONTRATO_SIC = '" & contrato & "' "
        '21/10/2010 KINTELL, TEMA CUPS, SOLO TIENEN K SALIR LOS DE FECHA_BAJA = NULL.
        ''selectSQL &= "WHERE RCC.COD_CONTRATO_SIC = '" & contrato & "' " 'AND MANTENIMIENTO.FEC_BAJA_SERVICIO IS NULL "
        '*************************************************************
        'Kintell 08/04/2009
        If Not CodigoProveedor = "" And Not UCase(CodigoProveedor) = "D" Then selectSQL &= "AND MANTENIMIENTO.PROVEEDOR = '" & CodigoProveedor & "' "
        '*************************************************************
        selectSQL &= "ORDER BY VISITA.FEC_LIMITE_VISITA DESC"

        Dim con As New SqlConnection(connectionString)
        Dim cmd As New SqlCommand(selectSQL, con)
        con.Open()
        Dim Reader As SqlDataReader

        Try

            Reader = cmd.ExecuteReader()

            Reader.Read()

            txt_ContratoDet.Text = Reader("COD_CONTRATO_SIC").ToString()
            

            Dim Nombre As String = Reader("NOM_TITULAR").ToString() + " " _
            + Reader("APELLIDO1").ToString() + " " + Reader("APELLIDO2").ToString()

            txt_ClienteDet.Text = Nombre

            Dim Suministro As String = Reader("TIP_VIA_PUBLICA").ToString() + " " _
            + Reader("NOM_CALLE").ToString() + " " + Reader("COD_PORTAL").ToString() + ", " _
            + Reader("TIP_PISO").ToString() + " " + Reader("TIP_MANO").ToString() + ", " _
            + Reader("COD_POSTAL").ToString() + ", " + Reader("NOM_POBLACION").ToString() + ", " _
            + Reader("NOM_PROVINCIA").ToString()

            txt_SuministroDet.Text = Suministro


            Dim proveedor As String
            proveedor = Reader("PROVEEDOR").ToString()

            If Not String.IsNullOrEmpty(proveedor) Then
                If Not String.IsNullOrEmpty(proveedor) Then
                    Dim Datos As New DataSet()
                    Datos = GetDatosProveedor(proveedor)
                    If Datos.Tables.Count > 0 Then
                        If Not IsDBNull(Datos.Tables(0).Rows(0).Item("EMAIL")) Then txt_EmailProDet.Text = Datos.Tables(0).Rows(0).Item("EMAIL")
                        If Not IsDBNull(Datos.Tables(0).Rows(0).Item("TELEFONO")) Then txt_TelProvDet.Text = Datos.Tables(0).Rows(0).Item("TELEFONO")

                        If Not IsDBNull(Datos.Tables(0).Rows(0).Item("NOMBRE")) Then txt_ProveedorDet.Text = Datos.Tables(0).Rows(0).Item("NOMBRE")
                    End If
                End If
            Else
                txt_ContratoDet.Text = ""
            End If


            'Kintell 14/01/10
            Dim proveedorAveria As String
            proveedorAveria = Reader("PROVEEDOR_AVERIA").ToString()
            If Not String.IsNullOrEmpty(proveedorAveria) Then
                If Not String.IsNullOrEmpty(proveedorAveria) Then
                    Dim Datos As New DataSet()
                    Datos = GetDatosProveedor(proveedorAveria)
                    If Datos.Tables.Count > 0 Then
                        txt_EmailProAveria.Text = Datos.Tables(0).Rows(0).Item("EMAIL")
                        txt_TelProvAveria.Text = Datos.Tables(0).Rows(0).Item("TELEFONO")

                        txt_ProveedorAveria.Text = Datos.Tables(0).Rows(0).Item("NOMBRE")
                    End If
                End If
            Else
                'txt_ContratoDet.Text = ""
            End If



            txt_EstadoDet.Text = Reader("DES_ESTADO").ToString()

            txt_UrgenciaDet.Text = Reader("URGENCIA").ToString()

            txt_CUPS.Text = Reader("CUPS").ToString()

            txt_FechaLimDet.Text = Left(Reader("FEC_LIMITE_VISITA").ToString(), 10)

            Dim servicio As String
            Dim T1 As String
            Dim T2 As String
            Dim T5 As String


            If Reader("T1").ToString() = "S" Then
                T1 = "Servicio Mantenimiento Gas Calefacción"
            Else
                T1 = ""
            End If

            If Reader("T2").ToString() = "S" Then
                T2 = "Servicio Mantenimiento Gas"
            Else
                T2 = ""
            End If

            If Reader("T5").ToString() = "S" Then
                T5 = "con pago fraccionado"
            Else
                T5 = ""
            End If

            servicio = T1 + T2 + " " + T5

            txt_ServicioDet.Text = servicio


            txt_MarcaCaldera.Text = ""
            txt_FecAltaSer.Text = Left(Reader("FEC_ALTA_SERVICIO").ToString(), 10)
            txt_FecBajaSer.Text = Left(Reader("FEC_BAJA_SERVICIO").ToString(), 10)
            If Not String.IsNullOrEmpty(txt_FecBajaSer.Text) And Not txt_FecBajaSer.Text = "01/01/1900" Then
                lbl_Mensaje.Text = "EL SERVICIO ESTA DADO DE BAJA"

                'Me.btn_CrearSolicitud.Enabled = False
                'Me.btn_ModificarSolicitud.Enabled = False
                Me.btn_CrearSolicitud.Enabled = True
                Me.btn_ModificarSolicitud.Enabled = True
            Else
                lbl_Mensaje.Text = ""
                Me.btn_CrearSolicitud.Enabled = True
                Me.btn_ModificarSolicitud.Enabled = True
            End If

            '11/08/2010 Kintell.
            'Activamos botón de baja de facturación (tema CUPS).
            lblMensajeFacturacion.Visible = False
            btnBajaContrato.Enabled = False
            If Not String.IsNullOrEmpty(Reader("BCS").ToString()) Then
                If UCase(Reader("BCS").ToString().Substring(0, 1)) = "C" And (String.IsNullOrEmpty(Reader("FEC_BAJA_SERVICIO").ToString()) Or txt_FecBajaSer.Text = "01/01/1900") Then
                    btnBajaContrato.Enabled = True
                    lblMensajeFacturacion.Visible = False
                    'ElseIf String.IsNullOrEmpty(Reader("BCS").ToString()) And String.IsNullOrEmpty(Reader("FECHA_HASTA_FACTURACION").ToString()) Then
                    '   btnBajaContrato.Enabled = True
                    '  lblMensajeFacturacion.Visible = False
                ElseIf Not String.IsNullOrEmpty(Reader("FECHA_HASTA_FACTURACION").ToString()) Then
                    lblMensajeFacturacion.Visible = True
                    lblMensajeFacturacion.Visible = False
                End If
            End If
            'If Not String.IsNullOrEmpty(Reader("FECHA_HASTA_FACTURACION").ToString()) Then
            '    lblMensajeFacturacion.Visible = True
            '    btnBajaContrato.Enabled = False
            'Else
            '    lblMensajeFacturacion.Visible = False
            'End If
            '**************************
            txt_ObsDet.Text = Reader("OBSERVACIONES").ToString()

            Grid(contrato)

        Catch err As Exception

            btn_CrearSolicitud.Visible = False
            btn_VerSolicitud.Visible = False
            btn_ModificarSolicitud.Visible = False
            lbl_Solicitudes.Visible = False
            gv_Solicitudes.Visible = False

            txt_ContratoDet.Text = Nothing
            txt_ClienteDet.Text = Nothing
            txt_SuministroDet.Text = Nothing
            txt_ProveedorDet.Text = Nothing
            txt_TelProvDet.Text = Nothing
            txt_EmailProDet.Text = Nothing
            txt_EstadoDet.Text = Nothing
            txt_UrgenciaDet.Text = Nothing
            txt_ServicioDet.Text = Nothing
            txt_MarcaCaldera.Text = Nothing
            txt_FechaLimDet.Text = Nothing
            txt_ObsDet.Text = Nothing
            txt_FecAltaSer.Text = Nothing
            txt_FecBajaSer.Text = Nothing



            GridView1.DataSource = Nothing
            GridView1.DataBind()

            txt_Estado.Text = Nothing
            txt_NVisita.Text = Nothing
            btnAviso.Enabled = False
            txt_FecVisita.Text = Nothing
            txt_FecLimVisita.Text = Nothing
            txt_Reparacion.Text = Nothing
            txt_TipoReparacion.Text = Nothing
            txt_TiempoManoObra.Text = Nothing
            txt_ImporteMOA.Text = Nothing
            txt_ImporteMAd.Text = Nothing
            txt_ImporteRep.Text = Nothing
            txt_Observaciones2.Text = Nothing


            lbl_error.Text = "Datos no disponibles. Error: "
            lbl_error.Text &= err.Message
            lbl_Mensaje.Text = "No existen datos para el nº de contrato introducido. Utilice la busqueda avanzada"
        Finally

            con.Close()
        End Try

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

    Private Sub Grid(ByVal contrato As String)


        Dim connectionString As String = _
  WebConfigurationManager.ConnectionStrings("OPCOMCMD").ConnectionString



        ''Dim selectSQL As String = "SELECT dbo.VISITAS.COD_CONTRATO, dbo.VISITAS.FEC_PLANIF_VISITA, "
        ''selectSQL &= "dbo.VISITAS.FEC_VISITA, dbo.VISITAS.FEC_LIMITE_VISITA, "
        ''selectSQL &= "dbo.VISITAS.DES_ESTADO, dbo.VISITAS.COD_VISITA "
        ''selectSQL &= "FROM dbo.VISITAS JOIN dbo.MANTENIMIENTO "
        ''selectSQL &= "ON dbo.VISITAS.COD_CONTRATO = dbo.MANTENIMIENTO.COD_CONTRATO_SIC "
        ''selectSQL &= "WHERE dbo.VISITAS.COD_CONTRATO = '" & contrato & "' "
        ''selectSQL &= "ORDER BY dbo.VISITAS.COD_VISITA DESC"

        Dim selectSQL As String = "SELECT distinct VISITA.COD_CONTRATO, " 'VISITA.FEC_PLANIF_VISITA, "
        selectSQL &= "VISITA.FEC_VISITA, VISITA.FEC_LIMITE_VISITA, "
        selectSQL &= "TIPO_ESTADO_VISITA.DES_ESTADO AS DES_ESTADO, VISITA.COD_VISITA "
        selectSQL &= ",'d' as cod_proveedor "
        selectSQL &= "FROM VISITA JOIN MANTENIMIENTO "
        selectSQL &= "ON VISITA.COD_CONTRATO = MANTENIMIENTO.COD_CONTRATO_SIC "
        '****************************************************************************************************
        'selectSQL &= "LEFT JOIN VISITA_HISTORICO H ON H.COD_CONTRATO=VISITA.COD_CONTRATO AND H.COD_VISITA=VISITA.COD_VISITA LEFT JOIN USUARIOS U ON H.USUARIO=U.USERID LEFT JOIN PERFILES P ON U.ID_PERFIL=P.ID_PERFIL "
        '***************************************************
        '08/08/2010.
        'Kintell por tema de busqueda por CUPS.
        selectSQL &= "INNER JOIN RELACION_CUPS_CONTRATO RCC ON RCC.CUPS=MANTENIMIENTO.CUPS "
        '******
        selectSQL &= "INNER JOIN TIPO_ESTADO_VISITA ON VISITA.COD_ESTADO_VISITA = TIPO_ESTADO_VISITA.COD_ESTADO "
        '***************************************************
        '08/08/2010.
        'Kintell por tema de busqueda por CUPS.
        'selectSQL &= "WHERE VISITA.COD_CONTRATO = '" & contrato & "' "
        '21/10/2010 KINTELL TEMA CUPS SOLO TIENEN K SALIR LOS DE FECHA_BAJA = NULL.
        selectSQL &= "WHERE RCC.COD_CONTRATO_SIC = '" & contrato & "'  " 'AND MANTENIMIENTO.FEC_BAJA_SERVICIO IS NULL "
        '********
        selectSQL &= "ORDER BY VISITA.COD_VISITA DESC"


        Dim con As New SqlConnection(connectionString)
        Dim cmd As New SqlCommand(selectSQL, con)
        Dim adapter As New SqlDataAdapter(cmd)
        ' Fill the DataSet.
        ds = New DataSet()

        Try
            btnAviso.Enabled = False
            txt_Estado.Text = Nothing
            txt_NVisita.Text = Nothing
            txt_FecVisita.Text = Nothing
            txt_FecLimVisita.Text = Nothing
            txt_Reparacion.Text = Nothing
            txt_TipoReparacion.Text = Nothing
            txt_TiempoManoObra.Text = Nothing
            txt_ImporteMOA.Text = Nothing
            txt_ImporteMAd.Text = Nothing
            txt_ImporteRep.Text = Nothing
            txt_Observaciones2.Text = Nothing
            adapter.Fill(ds, "Visitas")
            GridView1.DataSource = ds
        Catch err As Exception
            lbl_error.Text = "Consulta no realizada GRID. Error: "
            lbl_error.Text &= err.Message
        Finally
            GridView1.DataBind()
        End Try


    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        ' FORMATEA ROWS
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' ASIGNA EVENTOS
            e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")
            e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.GridView1, "Select$" + e.Row.RowIndex.ToString)
            For i = 0 To e.Row.Cells.Count - 1
                'e.Row.Cells(i).ToolTip = e.Row.Cells(15).Text
                e.Row.Cells(i).Attributes("style") &= "cursor:hand;"
                If (i = e.Row.Cells.Count - 1) Then
                    e.Row.Cells(i).Text = Visitas.ObtenerProveedorUltimoMovimiento(e.Row.Cells(2).Text, e.Row.Cells(1).Text)
                    If e.Row.Cells(i).Text = "d" Then
                        e.Row.Cells(i).Text = "ADMINISTRACION"
                    End If
                End If
            Next

        End If


    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

        Dim codvisita As String = GridView1.SelectedRow.Cells(1).Text
        Dim codcontrato As String = GridView1.SelectedRow.Cells(2).Text

        Dim connectionString As String = _
WebConfigurationManager.ConnectionStrings("OPCOMCMD").ConnectionString

        'Dim selectSQL As String = "SELECT dbo.VISITAS.REPARACION, dbo.VISITAS.TIPO_REPARACION, "
        'selectSQL &= "dbo.VISITAS.TIEMPO_MANO_OBRA, dbo.VISITAS.IMPORTE_REPARACION, "
        'selectSQL &= "dbo.VISITAS.IMPORTE_MANO_OBRA_ADICIONAL, dbo.VISITAS.IMPORTE_MATERIALES_ADICIONAL, "
        'selectSQL &= "dbo.VISITAS.OBSERVACIONES, dbo.VISITAS.DES_ESTADO,dbo.VISITAS.COD_VISITA, "
        'selectSQL &= "dbo.VISITAS.FEC_VISITA, dbo.VISITAS.FEC_LIMITE_VISITA "
        'selectSQL &= "FROM dbo.VISITAS "
        'selectSQL &= "WHERE dbo.VISITAS.COD_VISITA = '" & codvisita & "' AND COD_CONTRATO = '" & codcontrato & "'"

        Dim selectSQL As String = "SELECT REPARACION.ID_REPARACION, TIPO_REPARACION.DESC_TIPO_REPARACION, "
        selectSQL &= "TIPO_TIEMPO_MANO_OBRA.DESC_TIPO_TIEMPO_MANO_OBRA AS TIEMPO_MANO_OBRA, REPARACION.IMPORTE_REPARACION, "
        selectSQL &= "REPARACION.IMPORTE_MANO_OBRA_ADICIONAL, REPARACION.IMPORTE_MATERIALES_ADICIONAL, "
        selectSQL &= "VISITA.OBSERVACIONES, TIPO_ESTADO_VISITA.DES_ESTADO AS DES_ESTADO,VISITA.COD_VISITA, "
        selectSQL &= "VISITA.FEC_VISITA, VISITA.FEC_LIMITE_VISITA "
        selectSQL &= "FROM VISITA "
        '************************************************************
        selectSQL &= "LEFT JOIN REPARACION ON REPARACION.ID_REPARACION=VISITA.ID_REPARACION "
        selectSQL &= "LEFT JOIN TIPO_REPARACION ON REPARACION.ID_TIPO_REPARACION=TIPO_REPARACION.ID_TIPO_REPARACION "
        selectSQL &= "INNER JOIN TIPO_ESTADO_VISITA ON VISITA.COD_ESTADO_VISITA=TIPO_ESTADO_VISITA.COD_ESTADO "
        selectSQL &= "LEFT JOIN TIPO_TIEMPO_MANO_OBRA ON TIPO_TIEMPO_MANO_OBRA.ID_TIPO_TIEMPO_MANO_OBRA=REPARACION.ID_TIPO_TIEMPO_MANO_OBRA "
        '************************************************************
        selectSQL &= "WHERE VISITA.COD_VISITA = '" & codvisita & "' AND COD_CONTRATO = '" & codcontrato & "'"


        Dim con As New SqlConnection(connectionString)
        Dim cmd As New SqlCommand(selectSQL, con)
        con.Open()
        Dim Reader As SqlDataReader
        Reader = cmd.ExecuteReader()

        Reader.Read()

        Try
            'If Not GridView1.SelectedRow.Cells(3).Text = "Cerrada" And codvisita = GridView1.Rows.Count Then
            If codvisita >= GridView1.Rows.Count Then
                btnAviso.Enabled = True
            Else
                btnAviso.Enabled = False
            End If
            txt_Estado.Text = Reader("DES_ESTADO").ToString()
            txt_NVisita.Text = Reader("COD_VISITA").ToString()
            txt_FecVisita.Text = Left(Reader("FEC_VISITA").ToString(), 10)
            txt_FecLimVisita.Text = Left(Reader("FEC_LIMITE_VISITA").ToString(), 10)

            If Reader("ID_REPARACION").ToString() = "" Then
                txt_Reparacion.Text = "NO"
            Else
                txt_Reparacion.Text = "SI"
            End If


            txt_TipoReparacion.Text = Reader("DESC_TIPO_REPARACION").ToString()
            'txt_TipoReparacion.Text = contrato
            txt_TiempoManoObra.Text = Reader("TIEMPO_MANO_OBRA").ToString()

            txt_ImporteMOA.Text = Reader("IMPORTE_MANO_OBRA_ADICIONAL").ToString() + " €"

            txt_ImporteMAd.Text = Reader("IMPORTE_MATERIALES_ADICIONAL").ToString() + " €"

            txt_ImporteRep.Text = Reader("IMPORTE_REPARACION").ToString() + " €"

            txt_Observaciones2.Text = Reader("OBSERVACIONES").ToString()



        Catch err As Exception
            lbl_Mensaje.Text = "Datos no disponibles. Error: "
            'lbl_Mensaje.Text &= err.Message
        Finally

            Reader.Close()
            con.Close()
        End Try


    End Sub

#End Region

#Region "Funciones"

    Private Function ObtenerNumerosContratos() As Integer
        Dim objSolicitudesDB As New SolicitudesDB()
        Dim NumeroContratos As Integer = objSolicitudesDB.GetNumeroContratosActivos()

        Return NumeroContratos
    End Function

    Private Sub CargaSolicitudes(ByVal contrato As String)

        Dim objSolicitudesDB As New SolicitudesDB()

        Dim dsSolicitudes As DataSet = objSolicitudesDB.GetSolicitudesPorContrato(contrato, 1)

        gv_Solicitudes.Columns(0).Visible = True
        gv_Solicitudes.Columns(1).Visible = True

        Session("dsSolicitudes") = dsSolicitudes

        gv_Solicitudes.DataSource = dsSolicitudes
        gv_Solicitudes.DataBind()

        '' las oculto despues del databind para que me mantenga los datos
        'gv_Solicitudes.Columns(0).Visible = False
        gv_Solicitudes.Columns(1).Visible = False


    End Sub

#End Region

#Region "Eventos"

    Private Function ComprobarAveriasAbiertas() As Boolean

        Try
            Dim i As Integer = 0
            Dim Abierta As Boolean = False

            For i = 0 To Me.gv_Solicitudes.Rows.Count - 1
                If Not InStr(UCase(Me.gv_Solicitudes.Rows(i).Cells(5).Text), UCase("Pendiente")) = 0 Then
                    Abierta = True
                    Exit For
                End If
            Next
            Return Abierta

        Catch ex As Exception

        End Try
    End Function

    Protected Sub btn_CrearSolicitud_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CrearSolicitud.Click
        Session("SOLICITUD_SELECCIONADA") = Nothing



        Dim contrato As String = String.Empty

        If Not String.IsNullOrEmpty(txt_Contrato.Text.Trim()) Then
            contrato = txt_Contrato.Text.Trim()
        ElseIf Not String.IsNullOrEmpty(Request.QueryString("contrato")) Then
            contrato = Request.QueryString("contrato")
        End If

        If Not String.IsNullOrEmpty(contrato) Then
            Dim mantenimientoSolicudes As String = ConfigurationManager.AppSettings.Get("url_mantenimientoSolicitudes")
            Dim Abierta As String = "0"
            Dim Baja As String = "0"
            If ComprobarAveriasAbiertas() = True Then Abierta = "1"
            If Not String.IsNullOrEmpty(txt_FecBajaSer.Text) And txt_FecBajaSer.Text <> "01/01/1900" Then Baja = "1"
            Response.Redirect(mantenimientoSolicudes + "?mod=c" + "&con=" + contrato + "&Abierta=" + Abierta + "&Baja=" + Baja, False)
        End If
    End Sub

    Protected Sub btn_VerSolicitud_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_VerSolicitud.Click

        Dim contrato As String = String.Empty

        If Not String.IsNullOrEmpty(txt_Contrato.Text.Trim()) Then
            contrato = txt_Contrato.Text.Trim()
        ElseIf Not String.IsNullOrEmpty(Request.QueryString("contrato")) Then
            contrato = Request.QueryString("contrato")
        End If



        If Not String.IsNullOrEmpty(contrato) Then
            Dim mantenimientoSolicudes As String = ConfigurationManager.AppSettings.Get("url_mantenimientoSolicitudes")
            Dim solicitudSeleccionada As String = String.Empty

            If Not txtOculto.Value = "" Then
                solicitudSeleccionada = gv_Solicitudes.Rows(txtOculto.Value).Cells(1).Text
            End If

            Response.Redirect(mantenimientoSolicudes + "?mod=v" + "&sol=" + solicitudSeleccionada + "&con=" + contrato, False)
        End If
    End Sub

    Private Function PonerFoco(ByVal ctrl As Control)
        Dim focusScript As String = "<script language='JavaScript'>" & _
    "document.getElementById('" + ctrl.ClientID & _
    "').focus();</script>"

        Page.RegisterStartupScript("FocusScript", focusScript)
    End Function

    Protected Sub btn_ModificarSolicitud_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ModificarSolicitud.Click


        If (Not txtOculto.Value = "") Then
            If gv_Solicitudes.Rows(txtOculto.Value).Cells(4).Text.IndexOf("Precinte") < 0 And gv_Solicitudes.Rows(txtOculto.Value).Cells(4).Text.IndexOf("supervisi") < 0 Then


                Dim mantenimientoSolicudes As String = ConfigurationManager.AppSettings.Get("url_mantenimientoSolicitudes")
                Dim solicitudSeleccionada As String = gv_Solicitudes.Rows(txtOculto.Value).Cells(1).Text
                Dim contrato As String = gv_Solicitudes.Rows(txtOculto.Value).Cells(2).Text
                Dim Baja As String = "0"

                If Not String.IsNullOrEmpty(txt_FecBajaSer.Text) And txt_FecBajaSer.Text <> "01/01/1900" Then Baja = "1"
                Response.Redirect(mantenimientoSolicudes + "?mod=m" + "&sol=" + solicitudSeleccionada + "&con=" + contrato + "&Baja=" + Baja, False)
            Else
                Dim Mensaje As String = "<script language='JavaScript'>alert('No puede modificar este tipo de solicitud');</script>"

                Page.RegisterStartupScript("Mensaje", Mensaje)
            End If
        End If

    End Sub



    'Protected Sub gv_Solicitudes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Solicitudes.PageIndexChanging

    '    Try

    '        Dim dsSolicitudes As DataSet = Session("dsSolicitudes")

    '        gv_Solicitudes.Columns(0).Visible = True
    '        gv_Solicitudes.Columns(1).Visible = True

    '        gv_Solicitudes.PageIndex = e.NewPageIndex

    '        gv_Solicitudes.DataSource = dsSolicitudes
    '        gv_Solicitudes.DataBind()

    '        '' las oculto despues del databind para que me mantenga los datos
    '        gv_Solicitudes.Columns(0).Visible = False
    '        gv_Solicitudes.Columns(1).Visible = False

    '    Catch ex As Exception
    '        lbl_error.Text = "No se ha podido recargar el grid"
    '    End Try

    'End Sub

#End Region



    Protected Sub btn_Consultar0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Consultar0.Click
        Try
            txt_Contrato.Text = Nothing
            btn_CrearSolicitud.Visible = False
            btn_VerSolicitud.Visible = False
            btn_ModificarSolicitud.Visible = False
            lbl_Solicitudes.Visible = False
            gv_Solicitudes.Visible = False

            txt_ContratoDet.Text = Nothing
            txt_ClienteDet.Text = Nothing
            txt_SuministroDet.Text = Nothing
            txt_ProveedorDet.Text = Nothing
            txt_TelProvDet.Text = Nothing
            txt_EmailProDet.Text = Nothing
            txt_EstadoDet.Text = Nothing
            txt_UrgenciaDet.Text = Nothing
            txt_ServicioDet.Text = Nothing
            txt_MarcaCaldera.Text = Nothing
            txt_FechaLimDet.Text = Nothing
            txt_ObsDet.Text = Nothing
            txt_FecAltaSer.Text = Nothing
            txt_FecBajaSer.Text = Nothing

            'Kintell 14/01/10
            txt_ProveedorAveria.Text = Nothing
            txt_TelProvAveria.Text = Nothing
            txt_EmailProAveria.Text = Nothing




            GridView1.DataSource = Nothing
            GridView1.DataBind()

            txt_Estado.Text = Nothing
            txt_NVisita.Text = Nothing
            btnAviso.Enabled = False
            txt_FecVisita.Text = Nothing
            txt_FecLimVisita.Text = Nothing
            txt_Reparacion.Text = Nothing
            txt_TipoReparacion.Text = Nothing
            txt_TiempoManoObra.Text = Nothing
            txt_ImporteMOA.Text = Nothing
            txt_ImporteMAd.Text = Nothing
            txt_ImporteRep.Text = Nothing
            txt_Observaciones2.Text = Nothing


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

    Protected Sub btnBajaContrato_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBajaContrato.Click
        '11/08/2010 Kintell.
        '**************************************
        'Botón para dar de baja en contrato de facturación.
        Dim Contrato As String = txt_ContratoDet.Text
        'Hay k mirar si tenemos k actualizar la fecha de baja o la fecha_hasta_facturacion.....
        'Mantenimiento.ActualizarFechaHastaFacturacion(Contrato)
        Dim usuario As String = Session("usuarioValido").ToString()
        Mantenimiento.ActualizarFechaBajaServicio(Contrato, usuario)


        'FACTURACION BAJAS 11/12/2012.
        Mantenimiento.InsertarFacturacionBajas(Contrato, Int32.Parse(GridView1.Rows(0).Cells(1).Text.ToString()))


        'BAJAS DONDE MIGUEL FERRER 13/05/2011
        Dim cONEXION As String ' = "Data Source=CLSQLX64E0B\PROEB;Initial Catalog=OPCOMCMD;Persist Security Info=True;User ID=CSCADMN;Password=J_hUdn9Z;Pooling='false'"
        cONEXION = WebConfigurationManager.ConnectionStrings("OPCOMCMDMIGUELFERRER").ConnectionString

        Dim myConnection As New SqlConnection(cONEXION)
        Dim Sql As String = ""


        Try
            myConnection.Open()
            Sql = "INSERT INTO GFI_OPCOM.ALARMAS.BAJAS_ALARMAS (COD_CONTRATO,TIPO,SUBTIPO) VALUES('" + Contrato + "','01','33')"
            Dim myCommand As New SqlCommand(Sql, myConnection)
            myCommand.ExecuteNonQuery()

            myConnection.Close()
        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try
        '**********************************************
       
        'Buscamos otra vez
        lbl_Mensaje.Text = ""
        lbl_error.Text = ""
        Contrato = ""
        btnBajaContrato.Enabled = False
        Contrato = txt_Contrato.Text.Trim()
        Try
            'Dim usuario As String = Session("usuarioValido").ToString()
            Dim objUsuariosDB As New UsuariosDB()
            Dim dr As SqlDataReader = objUsuariosDB.GetProveedorUsuario(usuario)
            dr.Read()
            Dim cod_proveedor As String = dr("proveedor").ToString()
            Dim nombre_proveedor As String = dr("nombre").ToString()

            ConVisitas(Contrato, cod_proveedor)

            Contrato = txt_ContratoDet.Text.Trim()
            txt_Contrato.Text = Contrato
        Catch ex As Exception
        End Try
        CargaSolicitudes(Contrato)
        '**************************************
    End Sub

    Protected Sub btnAviso_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAviso.Click

    End Sub
End Class