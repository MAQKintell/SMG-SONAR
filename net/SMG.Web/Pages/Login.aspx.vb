Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Web.UI.WebControls.WebControl

Imports Iberdrola.Commons.Web

Partial Class Pages_Login
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Not Page.IsPostBack()) Then
        'Dim Cultur As AsignarCultura = New AsignarCultura()
        'Cultur.Asignar(Page)
        'End if

        '' por defecto quitamos el mensaje de error
        lbl_error.Visible = False

        PonerFoco(txt_Login)

    End Sub

    Private Function PonerFoco(ByVal ctrl As Control)
        Dim focusScript As String = "<script language='JavaScript'>" & _
    "document.getElementById('" + ctrl.ClientID & _
    "').focus();</script>"

        Page.RegisterStartupScript("FocusScript", focusScript)
    End Function

    Protected Sub btn_Entrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Entrar.Click

        Dim login As String = txt_Login.Text.Trim()
        Dim pass As String = txt_Password.Text.Trim()

        If Not String.IsNullOrEmpty(login) And Not String.IsNullOrEmpty(pass) Then

            Dim objUsuariosDB As New UsuariosDB()
            Dim userValido As Boolean = objUsuariosDB.ValidaUsuario(login, pass)

            If userValido Then
                Session("usuarioValido") = login

                'Kintell 17/04/2009.
                'Obtenemos el perfil del usuario logeado para redireccionarle a una página u a otra.
                Dim connectionString As String = WebConfigurationManager.ConnectionStrings("OPCOMCMD").ConnectionString
                Dim con As New SqlConnection(connectionString)
                Dim selectSQL As String = "SELECT Id_perfil from usuarios where userid='" & login & "' and password='" & pass & "'"
                Dim cmd2 As New SqlCommand(selectSQL, con)
                Dim Reader As SqlDataReader
                Dim IdPerfil As String

                con.Open()
                Try
                    Reader = cmd2.ExecuteReader()
                    Reader.Read()
                    IdPerfil = Reader("Id_perfil").ToString()
                    Session("IdPerfil") = IdPerfil
                    Reader.Close()
                    con.Close()
                Catch ex As Exception
                Finally
                    If (con.State = ConnectionState.Open) Then
                        con.Close()
                    End If
                End Try
                '*****************************************************************************
                If IdPerfil = "4" Then
                    'Administrador. Acceso a elegir donde kiere ir.
                    Dim urlRedirect As String = ConfigurationManager.AppSettings("url_fromLogin").ToString()
                    Response.Redirect(urlRedirect, False)
                Else
                    If IdPerfil = "3" Then
                        'Telefono. Acceso a Info_Visitas.
                        Dim urlRedirect As String = ConfigurationManager.AppSettings("url_info_visita").ToString()
                        Response.Redirect(urlRedirect, False)
                    Else
                        '1 y 2.
                        'Proveedores. Acceso a Proveedores.
                        Dim urlRedirect As String = ConfigurationManager.AppSettings("url_Proveedores").ToString()
                        Response.Redirect(urlRedirect, False)
                    End If
                End If


                'Script = "abrirVentanaLocalizacion(" & urlRedirect & ", 700, 630, 'ventana-modal','ELEGIR ACCIÓN','0');"

                'Page.RegisterStartupScript("QQ", "<Script language='JavaScript'>AbrirVentana('" & urlRedirect & "');</script>") ' & urlRedirect & "');</script>")
            Else
                lbl_error.Visible = True
            End If
            End If

    End Sub

  
End Class
