Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Imports Iberdrola.Commons.Web

Partial Class Pages_Argumentario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Not Page.IsPostBack()) Then
        'Dim Cultur As AsignarCultura = New AsignarCultura()
        '          'Cultur.Asignar(Page)
        'End if
        Try
            If Request.QueryString("Contrato") = "" Then
                Dim script As String = "<script language='Javascript'>Alert('Debe Seleccionar un Contrato.');window.close();</script>"
                Page.RegisterStartupScript("Alerta", script)
            Else

                Me.txtContrato.Text = Request.QueryString("Contrato")
                'Comprobamos k existe el Contrato.
                Dim objArgumentarioDB As New ArgumentarioDB()

                'Comprobamos k existe el Contrato en la B.D.
                '21/10/2010 KINTELL TEMA CUPS SOLO TIENEN K SALIR LOS DE FECHA_BAJA = NULL.
                Dim SelectSQl As String = "SELECT MANTENIMIENTO.COD_CONTRATO_SIC,PROVEEDOR FROM MANTENIMIENTO INNER JOIN RELACION_CUPS_CONTRATO RCC ON RCC.CUPS=MANTENIMIENTO.CUPS WHERE RCC.COD_CONTRATO_SIC = '" & txtContrato.Text & "' AND MANTENIMIENTO.FEC_BAJA_SERVICIO IS NULL"
                Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))

                Dim myDataSet As New DataSet
                Dim adapter As New SqlDataAdapter()
                myConnection.Open()
                Try
                    adapter.SelectCommand = New SqlCommand(SelectSQl, myConnection)
                    adapter.Fill(myDataSet)
                    myConnection.Close()
                Catch ex As Exception
                Finally
                    If (myConnection.State = ConnectionState.Open) Then
                        myConnection.Close()
                    End If
                End Try

                If myDataSet.Tables(0).Rows().Count = 0 Then
                    Dim script As String = "<script language='Javascript'>Alert('Contrato Inexistente en la B.D.');window.close();</script>"
                    Page.RegisterStartupScript("Alerta", script)
                Else
                    lblProveedor.Text = myDataSet.Tables(0).Rows(0).Item("proveedor")

                    '09/08/2010
                    Me.txtContrato.Text = myDataSet.Tables(0).Rows(0).Item("COD_CONTRATO_SIC")
                    '************
                End If


            End If
            If Not IsPostBack Then
                Dim dtsPadres As New DataSet
                Dim objArgumentarioDB As New ArgumentarioDB()
                dtsPadres = objArgumentarioDB.GetPadres()

                'rdbPreguntas.DataTextField = "TEXTO_PREGUNTA"
                'rdbPreguntas.DataValueField = "ID_PREGUNTA"
                'rdbPreguntas.ToolTip = "PADRES"
                'rdbPreguntas.DataSource = dtsPadres
                'rdbPreguntas.DataBind()

                For i = 0 To dtsPadres.Tables(0).Rows.Count - 1
                    Dim a As New ListItem
                    a.Value = dtsPadres.Tables(0).Rows(i).Item(0)
                    a.Text = dtsPadres.Tables(0).Rows(i).Item(1)
                    rdbPreguntas.Items.Add(a)
                    lblLeyenda.Text = dtsPadres.Tables(0).Rows(i).Item(2)
                Next
                Session("ResultadosArgumentario") = ""
            End If
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub rdbPreguntas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbPreguntas.SelectedIndexChanged
        Try
            btnAtras.Visible = True
            Dim Valor As String = rdbPreguntas.SelectedValue

            Session("ResultadosArgumentario") = Session("ResultadosArgumentario") & ";" & Valor
            Dim dts As New DataSet
            Dim objArgumentarioDB As New ArgumentarioDB()


            dts = objArgumentarioDB.GetHijosPorPadre(Valor)
            If Not dts.Tables(0).Rows.Count = 0 Then
                rdbPreguntas.Items.Clear()
            Else
                btnGuardar.Visible = True
                txtObservaciones.Visible = True
                lblObservaciones.Visible = True
                chkVisita.Visible = True

                rdbPreguntas.Enabled = False
            End If
            'rdbPreguntas.ToolTip = "HIJOS"

            For i = 0 To dts.Tables(0).Rows.Count - 1
                Dim a As New ListItem
                a.Value = dts.Tables(0).Rows(i).Item(0)
                a.Text = dts.Tables(0).Rows(i).Item(1)
                lblLeyenda.Text = dts.Tables(0).Rows(i).Item(2)
                rdbPreguntas.Items.Add(a)
            Next







            ' ''Proceso de Insert.
            'If dts.Tables(0).Rows.Count = 0 Then
            '    'Comprobamos k existe el Contrato en la B.D.
            '    Dim SelectSQl As String = "SELECT MANTENIMIENTOS_EX.COD_CONTRATO_SIC FROM dbo.MANTENIMIENTOS_EX WHERE dbo.MANTENIMIENTOS_EX.COD_CONTRATO_SIC = '" & txtContrato.Text & "'"
            '    Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))

            '    Dim myDataSet As New DataSet
            '    Dim adapter As New SqlDataAdapter()
            '    myConnection.Open()
            '    adapter.SelectCommand = New SqlCommand(SelectSQl, myConnection)
            '    Adapter.Fill(myDataSet)
            '    myConnection.Close()

            '    If myDataSet.Tables(0).Rows().Count = 0 Then
            '        btnGuardar.Visible = True
            '        Dim script As String = "<script language='Javascript'>Alert('Contrato Inexistente en la B.D.');</script>"
            '        Page.RegisterStartupScript("Alerta", script)
            '    Else

            '        Dim Valores() As String = Split(Session("ResultadosArgumentario"), ";")

            '        For i = 0 To Valores.Length - 1
            '            If Not Valores(i) = "" Then
            '                Dim Correcto As String = objArgumentarioDB.InsertResultados(txtContrato.Text, Valores(i), Session("usuarioValido"))
            '            End If
            '        Next
            '        Dim script As String = "<script language='Javascript'>Alert('Proceso COMPLETADO');window.close();</script>"
            '        Page.RegisterStartupScript("Cerrar", script)
            '        Session("ResultadosArgumentario") = ""
            '    End If
            'End If




        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            Dim objArgumentarioDB As New ArgumentarioDB()

            'Comprobamos k existe el Contrato en la B.D.
            Dim SelectSQl As String = "SELECT MANTENIMIENTO.COD_CONTRATO_SIC FROM MANTENIMIENTO WHERE MANTENIMIENTO.COD_CONTRATO_SIC = '" & txtContrato.Text & "'"
            Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))

            Dim myDataSet As New DataSet
            Dim adapter As New SqlDataAdapter()
            myConnection.Open()
            Try
                adapter.SelectCommand = New SqlCommand(SelectSQl, myConnection)
                adapter.Fill(myDataSet)
                myConnection.Close()
            Catch ex As Exception
            Finally
                If (myConnection.State = ConnectionState.Open) Then
                    myConnection.Close()
                End If
            End Try

            If myDataSet.Tables(0).Rows().Count = 0 Then
                btnGuardar.Visible = True
                txtObservaciones.Visible = True
                lblObservaciones.Visible = True
                chkVisita.Visible = True
                Dim script As String = "<script language='Javascript'>Alert('Contrato Inexistente en la B.D.');</script>"
                Page.RegisterStartupScript("Alerta", script)
            Else

                Dim Valores() As String = Split(Session("ResultadosArgumentario"), ";")

                For i = 0 To Valores.Length - 1
                    If Not Valores(i) = "" Then
                        Dim Correcto As String = objArgumentarioDB.InsertResultados(txtContrato.Text, Valores(i), Session("usuarioValido"), txtObservaciones.Text, chkVisita.Checked)
                    End If
                Next
                Dim script As String = "<script language='Javascript'>Alert('Proceso COMPLETADO');window.close();</script>"
                Page.RegisterStartupScript("Cerrar", script)
                Session("ResultadosArgumentario") = ""
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnAtras_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAtras.Click
        Try
            Dim Veces As Integer = 1



            rdbPreguntas.Enabled = True
            'txtObservaciones.Visible = False
            'lblObservaciones.Visible = False
            btnGuardar.Visible = False
            'chkVisita.Visible = False
            rdbPreguntas.Items.Clear()


            If btnGuardar.Visible Then veces = 2
            For j = 1 To Veces

                Dim Valores() As String = Split(Session("ResultadosArgumentario"), ";")
                Session("ResultadosArgumentario") = ""
                For i = 0 To Valores.Length - 2
                    If Not Valores(i) = "" Then Session("ResultadosArgumentario") = Session("ResultadosArgumentario") & ";" & Valores(i)
                Next


                Dim Valor As String = Valores(Valores.Length - 2) 'rdbPreguntas.SelectedValue

                If Valor = "" Then
                    Dim dtsPadres As New DataSet
                    Dim objArgumentarioDB As New ArgumentarioDB()
                    dtsPadres = objArgumentarioDB.GetPadres()

                    For i = 0 To dtsPadres.Tables(0).Rows.Count - 1
                        Dim a As New ListItem
                        a.Value = dtsPadres.Tables(0).Rows(i).Item(0)
                        a.Text = dtsPadres.Tables(0).Rows(i).Item(1)
                        rdbPreguntas.Items.Add(a)
                        lblLeyenda.Text = dtsPadres.Tables(0).Rows(i).Item(2)
                    Next
                    Session("ResultadosArgumentario") = ""
                Else

                    Dim dts As New DataSet
                    Dim objArgumentarioDB As New ArgumentarioDB()


                    dts = objArgumentarioDB.GetHijosPorPadre(Valor)
                    If Not dts.Tables(0).Rows.Count = 0 Then
                        rdbPreguntas.Items.Clear()
                    Else
                        btnGuardar.Visible = True
                        txtObservaciones.Visible = True
                        lblObservaciones.Visible = True
                        chkVisita.Visible = True
                    End If
                    'rdbPreguntas.ToolTip = "HIJOS"

                    For i = 0 To dts.Tables(0).Rows.Count - 1
                        Dim a As New ListItem
                        a.Value = dts.Tables(0).Rows(i).Item(0)
                        a.Text = dts.Tables(0).Rows(i).Item(1)
                        lblLeyenda.Text = dts.Tables(0).Rows(i).Item(2)
                        rdbPreguntas.Items.Add(a)
                    Next
                End If
            Next

            If Session("ResultadosArgumentario") = "" Then
                btnAtras.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
