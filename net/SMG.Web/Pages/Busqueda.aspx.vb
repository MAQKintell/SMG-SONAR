Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Web.UI.WebControls.WebControl

Imports Iberdrola.Commons.Web

Partial Class Pages_Busqueda
    Inherits System.Web.UI.Page
    Private ds As DataSet
    Private index As Integer

    Protected Overrides Function SaveViewState() As Object
        ViewState("ds") = ds
        ViewState("index") = index
        Return MyBase.SaveViewState()
    End Function

    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        ds = CObj(ViewState("ds"))
        index = CInt(ViewState("index"))
    End Sub




    Protected Sub btn_Busqueda_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Busqueda.Click

        lbl_Registros.Text = ""

        If txt_NombreBus.Text = "" And txt_Apellido1Bus.Text = "" And txt_Apellido2Bus.Text = "" _
        And txt_CalleBus.Text = "" And txt_PoblacionBus.Text = "" And txt_ContratoBus0.Text = "" Then

            lbl_Registros.Text = "Debe rellenar algún campo para ejecutarse la consulta"
            'MsgBox("Debe rellenar algún campo")
            'Response.Write("<script>alert('Debe rellenar algún campo');</script>")
            'Response.Redirect("busqueda.aspx")

        Else

            busCliente()

        End If




    End Sub

    Private Sub busCliente()

        Dim nombre As String
        Dim apellido1 As String
        Dim apellido2 As String
        Dim calle As String
        Dim poblacion As String
        Dim contratobus As String

        If txt_NombreBus.Text = "" Then

            nombre = ""

        Else

            nombre = txt_NombreBus.Text

        End If

        If txt_Apellido1Bus.Text = "" Then

            apellido1 = ""

        Else

            apellido1 = " AND MANTENIMIENTO.APELLIDO1 LIKE '%" & txt_Apellido1Bus.Text & "%' COLLATE SQL_Latin1_General_CP1253_CI_AI"

        End If

        If txt_Apellido2Bus.Text = "" Then

            apellido2 = ""

        Else

            apellido2 = " AND MANTENIMIENTO.APELLIDO2 LIKE '%" & txt_Apellido2Bus.Text & "%' COLLATE SQL_Latin1_General_CP1253_CI_AI"

        End If

        If txt_CalleBus.Text = "" Then

            calle = ""

        Else

            calle = " AND MANTENIMIENTO.NOM_CALLE LIKE '%" & txt_CalleBus.Text & "%' COLLATE SQL_Latin1_General_CP1253_CI_AI"

        End If

        If txt_PoblacionBus.Text = "" Then

            poblacion = ""

        Else

            poblacion = " AND Poblacion.nombre LIKE '%" & txt_PoblacionBus.Text & "%' COLLATE SQL_Latin1_General_CP1253_CI_AI"

        End If

        If txt_ContratoBus0.Text = "" Then

            contratobus = ""

        Else

            contratobus = " AND rcc.COD_CONTRATO_SIC LIKE '%" & txt_ContratoBus0.Text & "%' COLLATE SQL_Latin1_General_CP1253_CI_AI"

        End If


        ''*************************************************************
        ''Kintell 15/04/2009
        ''Buscamos el proveedor k sea logeado para controlar k solo pueda ver las suyas.
        'Dim usuario As String = Session("usuarioValido").ToString()
        'Dim objUsuariosDB As New UsuariosDB()
        'Dim dr As SqlDataReader = objUsuariosDB.GetProveedorUsuario(usuario)
        'dr.Read()
        'Dim cod_proveedor As String = dr("proveedor").ToString()
        'Dim nombre_proveedor As String = dr("nombre").ToString()
        'Dim Proveedor As String = ""
        'If Not cod_proveedor = "" And Not cod_proveedor = "d" Then Proveedor &= " AND dbo.MANTENIMIENTO.PROVEEDOR = '" & nombre_proveedor & "' "
        ''*************************************************************





        Dim connectionString As String = _
WebConfigurationManager.ConnectionStrings("OPCOMCMD").ConnectionString
        Dim con As New SqlConnection(connectionString)


        Dim selectSQL2 As String = "SELECT distinct COUNT(mantenimiento.COD_CONTRATO_SIC) AS TOTAL "
        selectSQL2 &= "FROM dbo.MANTENIMIENTO "
        '***********************************************************************************************
        '09/08/2010.
        'Kintell tema CUPS.
        selectSQL2 &= "inner join relacion_cups_contrato RCC on rcc.cups = mantenimiento.cups "
        '******
        selectSQL2 &= "inner join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        selectSQL2 &= "inner join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia) "
        '***********************************************************************************************
        '21/10/2010 KINTELL TEMA CUPS SOLO TIENEN K SALIR LOS DE FECHA_BAJA = NULL.
        selectSQL2 &= "WHERE MANTENIMIENTO.NOM_TITULAR LIKE '%" & nombre & "%' COLLATE SQL_Latin1_General_CP1253_CI_AI" & apellido1 & ""
        selectSQL2 &= "" & apellido2 & "" & calle & "" & poblacion & "" & contratobus & "" ' & Proveedor & ""

        Dim cmd2 As New SqlCommand(selectSQL2, con)
        con.Open()
        Dim Reader As SqlDataReader
        Reader = cmd2.ExecuteReader()
        btn_linkVisitas.Visible = True
        Reader.Read()
        Try
            Dim total As String
            total = Reader("TOTAL").ToString()

            If total = 0 Then
                lbl_Registros.Text = "No existen clientes para la consulta seleccionada"
                btn_linkVisitas.Visible = False
            Else
                lbl_Registros.Text = "Se han encontrado " + total + " clientes en la consulta seleccionada"
            End If


        Catch err As Exception
            lbl_Mensaje.Text = "Consulta no realizada. Error: "
            lbl_Mensaje.Text &= err.Message
        Finally
            Reader.Close()
            con.Close()
        End Try



        Dim selectSQL As String = "SELECT DISTINCT MANTENIMIENTO.COD_CONTRATO_SIC, "
        selectSQL &= "MANTENIMIENTO.NOM_TITULAR, MANTENIMIENTO.APELLIDO1, "
        selectSQL &= "MANTENIMIENTO.APELLIDO2, MANTENIMIENTO.NOM_CALLE, "
        selectSQL &= "POBLACION.NOMBRE as NOM_POBLACION "
        selectSQL &= "FROM MANTENIMIENTO "
        '***********************************************************************************************
        '09/08/2010.
        'Kintell tema CUPS.
        selectSQL &= "inner join relacion_cups_contrato RCC on rcc.cups = mantenimiento.cups "
        '******
        selectSQL &= "inner join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        selectSQL &= "inner join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia) "
        '***********************************************************************************************

        selectSQL &= "WHERE MANTENIMIENTO.NOM_TITULAR LIKE '%" & nombre & "%' COLLATE SQL_Latin1_General_CP1253_CI_AI " & apellido1 & ""
        selectSQL &= "" & apellido2 & "" & calle & "" & poblacion & "" & contratobus & "" ' & Proveedor & ""
        selectSQL &= " ORDER BY MANTENIMIENTO.NOM_TITULAR"
        Dim cmd As New SqlCommand(selectSQL, con)
        Dim adapter As New SqlDataAdapter(cmd)
        ' Fill the DataSet.
        ds = New DataSet()

        Try
            adapter.Fill(ds, "Visitas")
            GridView1.DataSource = ds
        Catch err As Exception
            lbl_Mensaje.Text = "Consulta no realizada. Error: "
            lbl_Mensaje.Text &= err.Message
        Finally
            GridView1.DataBind()
        End Try

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

        Dim contrato As String = GridView1.SelectedRow.Cells(1).Text

        Response.Redirect("info_visita.aspx?contrato=" & contrato & "")

    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging

        GridView1.PageIndex = e.NewPageIndex
        Dim dw As New DataView
        dw.Table = ds.Tables(0)
        GridView1.DataSource = dw
        GridView1.DataBind()

    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try

        
            ' FORMATEA ROWS
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' ASIGNA EVENTOS
                e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")
                e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")
                'Dim contrato As String = e.Row.Cells(1).Text
                e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.GridView1, "Select$" + e.Row.RowIndex.ToString)

                For i = 0 To e.Row.Cells.Count - 1
                    'e.Row.Cells(i).ToolTip = e.Row.Cells(15).Text
                    e.Row.Cells(i).Attributes("style") &= "cursor:pointer;"
                Next
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btn_linkVisitas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_linkVisitas.Click

        Response.Redirect("info_visita.aspx")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Not Page.IsPostBack()) Then
        '    Dim Cultur As AsignarCultura = New AsignarCultura()
        '    Cultur.Asignar(Page)
        'End If
    End Sub
End Class
