Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Imports Iberdrola.Commons.Web

Partial Class Pages_DatosArgumentario
    Inherits System.Web.UI.Page

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Session("VecesPasado") = 0
        Session("VecesPasadoEstaVez") = 0
        Dim i As Integer = Session("VecesPasado")
        Me.ImageButton3.Visible = False
        Dim VecesPasado As Integer = 0
        Try
            Dim dtsResultado As New DataSet
            Dim objArgumentarioDB As New ArgumentarioDB()
            dtsResultado = objArgumentarioDB.ObtenerResultados(txtContrato.Text, Me.chkTodos.Checked, txtDesde.Text, txtHasta.Text)
            Dim numPages = dtsResultado.Tables(0).Rows.Count / 4
            Session("DtsDatosArgumentario") = dtsResultado

            If Not dtsResultado.Tables(0).Rows.Count = 0 Then
                'For i = 0 To dtsResultado.Tables(0).Rows.Count Step 4


                If Not IsDBNull(dtsResultado.Tables(0).Rows(i).Item(0)) Then Me.lblTelefono1.Text = dtsResultado.Tables(0).Rows(i).Item(0)
                Me.lblFecha1.Text = dtsResultado.Tables(0).Rows(i).Item(2)
                Me.lblConContrato1.Text = dtsResultado.Tables(0).Rows(i).Item(6)

                'Me.lblGrupoBaja1.Text = dtsResultado.Tables(0).Rows(0).Item(5) & "/" & dtsResultado.Tables(0).Rows(0).Item(3)
                'Me.lblMotivoBaja1.Text = dtsResultado.Tables(0).Rows(1).Item(5) & "/" & dtsResultado.Tables(0).Rows(1).Item(3)
                'Me.lblAccion1.Text = dtsResultado.Tables(0).Rows(2).Item(5) & "/" & dtsResultado.Tables(0).Rows(2).Item(3)
                'Me.lblResultado1.Text = dtsResultado.Tables(0).Rows(3).Item(5) & "/" & dtsResultado.Tables(0).Rows(3).Item(3)

                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "A" Then
                    Me.lblGrupoBaja1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i + 1
                    vecespasado = vecespasado + 1
                Else
                    'Exit Try
                End If
                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "B" Then
                    Me.lblMotivoBaja1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i + 1
                    VecesPasado = VecesPasado + 1
                Else
                    'Exit Try
                End If
                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "C" Then
                    Me.lblAccion1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i + 1
                    VecesPasado = VecesPasado + 1
                Else
                    'Exit Try
                End If
                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "D" Then
                    Me.lblResultado1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i + 1
                    VecesPasado = VecesPasado + 1
                Else
                    'Exit Try
                End If
                'Next
            End If
            If dtsResultado.Tables(0).Rows.Count >= 4 Then ImageButton2.Visible = True

        Catch ex As Exception
        Finally
            Session("VecesPasado") = i
            Session("VecesPasadoEstaVez") = VecesPasado
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Not Page.IsPostBack()) Then
        'Dim Cultur As AsignarCultura = New AsignarCultura()
        'Cultur.Asignar(Page)
        'End if

        Me.lblTelefono.Text = "TELEFONO: "
        Me.lblFecha.Text = "FECHA LLAMADA: "
        Me.lblGrupoBaja.Text = "GRUPO MOTIVO BAJA: "
        Me.lblMotivoBaja.Text = "MOTIVO BAJA: "
        Me.lblAccion.Text = "ACCION DESENCADENADA: "
        Me.lblResultado.Text = "RESULTADO: "

        PonerFoco(Me.txtContrato)
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Try
            Dim dtsResultado As New DataSet
            Dim objArgumentarioDB As New ArgumentarioDB()
            dtsResultado = objArgumentarioDB.ObtenerResultadosParaExcell(txtContrato.Text, Me.chkTodos.Checked, txtDesde.Text, txtHasta.Text)

            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/vnd.ms-excel"
            Response.Charset = ""
            Me.EnableViewState = False
            Dim oStringWriter As System.IO.StringWriter = New System.IO.StringWriter()
            Dim oHtmlTextWriter As System.Web.UI.HtmlTextWriter = New System.Web.UI.HtmlTextWriter(oStringWriter)
            Dim dg As New DataGrid
            dg.DataSource = dtsResultado
            dg.DataBind()
            dg.RenderControl(oHtmlTextWriter)
            Response.Write(oStringWriter.ToString())
            Response.End()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        ImageButton3.Visible = True
        Dim i As Integer = Session("VecesPasado")
        Dim VecesPasado As Integer = 0

        Try
            Inicializarmuestra()
            Dim dtsResultado As New DataSet
            Dim objArgumentarioDB As New ArgumentarioDB()
            dtsResultado = Session("DtsDatosArgumentario") 'objArgumentarioDB.ObtenerResultados(txtContrato.Text, Me.chkTodos.Checked)
            Dim numPages = dtsResultado.Tables(0).Rows.Count / 4

            If Not dtsResultado.Tables(0).Rows.Count = 0 Then
                'For i = 0 To dtsResultado.Tables(0).Rows.Count Step 4


                If Not IsDBNull(dtsResultado.Tables(0).Rows(i).Item(0)) Then Me.lblTelefono1.Text = dtsResultado.Tables(0).Rows(i).Item(0)
                Me.lblFecha1.Text = dtsResultado.Tables(0).Rows(i).Item(2)
                Me.lblConContrato1.Text = dtsResultado.Tables(0).Rows(i).Item(6)

                'Me.lblGrupoBaja1.Text = dtsResultado.Tables(0).Rows(0).Item(5) & "/" & dtsResultado.Tables(0).Rows(0).Item(3)
                'Me.lblMotivoBaja1.Text = dtsResultado.Tables(0).Rows(1).Item(5) & "/" & dtsResultado.Tables(0).Rows(1).Item(3)
                'Me.lblAccion1.Text = dtsResultado.Tables(0).Rows(2).Item(5) & "/" & dtsResultado.Tables(0).Rows(2).Item(3)
                'Me.lblResultado1.Text = dtsResultado.Tables(0).Rows(3).Item(5) & "/" & dtsResultado.Tables(0).Rows(3).Item(3)

                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "A" Then
                    Me.lblGrupoBaja1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i + 1
                    vecespasado = vecespasado + 1
                Else
                    'Exit Try
                End If
                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "B" Then
                    Me.lblMotivoBaja1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i + 1
                    vecespasado = vecespasado + 1
                Else
                    'Exit Try
                End If
                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "C" Then
                    Me.lblAccion1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i + 1
                    vecespasado = vecespasado + 1
                Else
                    'Exit Try
                End If
                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "D" Then
                    Me.lblResultado1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i + 1
                    vecespasado = vecespasado + 1
                Else
                    'Exit Try
                End If
                'Next
            End If
            If i = dtsResultado.Tables(0).Rows.Count Then ImageButton2.Visible = False
        Catch ex As Exception
        Finally
            Session("VecesPasado") = i
            Session("VecesPasadoEstaVez") = VecesPasado
        End Try
    End Sub

    Private Function InicializarMuestra()
        Try
            Me.lblTelefono1.Text = ""
            Me.lblFecha1.Text = ""
            Me.lblGrupoBaja1.Text = ""
            Me.lblMotivoBaja1.Text = ""
            Me.lblAccion1.Text = ""
            Me.lblResultado1.Text = ""
        Catch ex As Exception

        End Try
    End Function

    Protected Sub ImageButton3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton3.Click
        ImageButton2.Visible = True
        Dim i As Integer = Session("VecesPasado") - Session("VecesPasadoEstaVez") - 1
        Dim j As Integer = i
        'If i Mod 4 = 0 Then
        '    i = i - 8
        'Else
        '    Exit Sub
        'End If
        Dim VecesPasado As Integer = 0

        Try
            InicializarMuestra()
            Dim dtsResultado As New DataSet
            Dim objArgumentarioDB As New ArgumentarioDB()
            dtsResultado = Session("DtsDatosArgumentario") 'objArgumentarioDB.ObtenerResultados(txtContrato.Text, Me.chkTodos.Checked)
            Dim numPages = dtsResultado.Tables(0).Rows.Count / 4

            If Not dtsResultado.Tables(0).Rows.Count = 0 Then
                'For i = 0 To dtsResultado.Tables(0).Rows.Count Step 4


                If Not IsDBNull(dtsResultado.Tables(0).Rows(i).Item(0)) Then Me.lblTelefono1.Text = dtsResultado.Tables(0).Rows(i).Item(0)
                Me.lblFecha1.Text = dtsResultado.Tables(0).Rows(i).Item(2)
                Me.lblConContrato1.Text = dtsResultado.Tables(0).Rows(i).Item(6)

                'Me.lblGrupoBaja1.Text = dtsResultado.Tables(0).Rows(0).Item(5) & "/" & dtsResultado.Tables(0).Rows(0).Item(3)
                'Me.lblMotivoBaja1.Text = dtsResultado.Tables(0).Rows(1).Item(5) & "/" & dtsResultado.Tables(0).Rows(1).Item(3)
                'Me.lblAccion1.Text = dtsResultado.Tables(0).Rows(2).Item(5) & "/" & dtsResultado.Tables(0).Rows(2).Item(3)
                'Me.lblResultado1.Text = dtsResultado.Tables(0).Rows(3).Item(5) & "/" & dtsResultado.Tables(0).Rows(3).Item(3)




                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "D" Then
                    Me.lblResultado1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i - 1
                    VecesPasado = VecesPasado + 1
                Else
                    'Exit Try
                End If
                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "C" Then
                    Me.lblAccion1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i - 1
                    VecesPasado = VecesPasado + 1
                Else
                    'Exit Try
                End If
                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "B" Then
                    Me.lblMotivoBaja1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i - 1
                    VecesPasado = VecesPasado + 1
                Else
                    'Exit Try
                End If
                If Left(dtsResultado.Tables(0).Rows(i).Item(1), 1) = "A" Then
                    Me.lblGrupoBaja1.Text = dtsResultado.Tables(0).Rows(i).Item(3)
                    i = i - 1
                    VecesPasado = VecesPasado + 1
                Else
                    'Exit Try
                End If
                'Next
            End If
        Catch ex As Exception
        Finally
            If j <= 4 Then ImageButton3.Visible = False
            Session("VecesPasado") = j + 1
            Session("VecesPasadoEstaVez") = VecesPasado
        End Try
    End Sub

    Private Function PonerFoco(ByVal ctrl As Control)
        Dim focusScript As String = "<script language='JavaScript'>" & _
    "document.getElementById('" + ctrl.ClientID & _
    "').focus();</script>"

        Page.RegisterStartupScript("FocusScript", focusScript)
    End Function

    Protected Sub CalendarDesde_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalendarDesde.SelectionChanged
        Me.txtDesde.Text = Me.CalendarDesde.SelectedDate
        Me.CalendarDesde.Visible = False
    End Sub

    Protected Sub CalendarHasta_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalendarHasta.SelectionChanged
        Me.txtHasta.Text = Me.CalendarHasta.SelectedDate
        Me.CalendarHasta.Visible = False
    End Sub

    Protected Sub ImageButton4_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton4.Click
        Me.CalendarDesde.Visible = True
        Me.CalendarHasta.Visible = False
    End Sub

    Protected Sub ImageButton5_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton5.Click
        Me.CalendarHasta.Visible = True
        Me.CalendarDesde.Visible = False
    End Sub
End Class
