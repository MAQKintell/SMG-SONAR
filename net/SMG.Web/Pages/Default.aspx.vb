Imports Iberdrola.Commons.Web

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Not Page.IsPostBack()) Then
        'Dim Cultur As AsignarCultura = New AsignarCultura()
        'Cultur.Asignar(Page)
        'End if
        Try
            Dim usuario As String = Session("usuarioValido").ToString()
        Catch ex As Exception
            Dim redirectURL As String = ConfigurationManager.AppSettings("url_Login").ToString()
            Response.Redirect(redirectURL, False)
        End Try

        ' controlamos que el perfil de la página es correcto 4
        Dim IdPerfil As String = Session("IdPerfil")
        Dim perfilValido As Boolean = False

        If (String.IsNullOrEmpty(IdPerfil)) Then
            perfilValido = False
        Else
            If "4".Equals(IdPerfil) Then
                perfilValido = True
            Else
                perfilValido = False
            End If
        End If

        If Not perfilValido Then
            Dim redirectURL As String = ConfigurationManager.AppSettings("url_Login").ToString()
            Response.Redirect(redirectURL, False)
        End If

    End Sub
End Class
