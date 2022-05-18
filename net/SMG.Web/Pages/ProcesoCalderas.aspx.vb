Imports System.Data
Imports System.Web.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Globalization

Imports Iberdrola.Commons.Web

Partial Class Pages_ProcesoCalderas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Not Page.IsPostBack()) Then
        'Dim Cultur As AsignarCultura = New AsignarCultura()
        'Cultur.Asignar(Page)
        'End if
        Try

            lblCodContrato.Text = Session("CodigoContrato")
            Dim objCalderasDB As New CalderasDB()

            Dim ds As New DataSet
            ds = objCalderasDB.GetCalderasPorContrato(lblCodContrato.Text)

            If Not ds.Tables(0).Rows().Count = 0 Then
                PANELNO.Visible = False
                PANELSI.Visible = True
                Me.txtTipo.Text = ds.Tables(0).Rows(0).Item(1).ToString
                Me.txtUso.Text = ds.Tables(0).Rows(0).Item(5).ToString
                Me.txtPotencia.Text = ds.Tables(0).Rows(0).Item(6).ToString
                Me.txtModelo.Text = ds.Tables(0).Rows(0).Item(4).ToString
                Me.txtMarca.Text = ds.Tables(0).Rows(0).Item(3).ToString
                Me.txtAño.Text = ds.Tables(0).Rows(0).Item(7).ToString

            Else
                PANELNO.Visible = True
                PANELSI.Visible = False
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
