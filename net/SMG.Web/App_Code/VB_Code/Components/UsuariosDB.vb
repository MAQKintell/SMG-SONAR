Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class UsuariosDB



    Public Function ValidaUsuario(ByVal usuario As String, ByVal password As String) As Boolean

        Dim usuarioValido As Boolean = False

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_ValidaUsuario", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterUserID As New SqlParameter("@UserID", SqlDbType.NVarChar, 7)
        parameterUserID.Value = usuario
        myCommand.SelectCommand.Parameters.Add(parameterUserID)

        Dim parameterPass As New SqlParameter("@Password", SqlDbType.NVarChar, 50)
        parameterPass.Value = password
        myCommand.SelectCommand.Parameters.Add(parameterPass)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        If (myDataSet.Tables.Count > 0 And myDataSet.Tables(0).Rows.Count > 0) Then

            '' mazo cutre pero son las 19.10...
            usuarioValido = True

        End If

        Return usuarioValido

    End Function

    Public Function GetProveedorUsuario(ByVal usuario As String) As SqlDataReader

        Dim proveedor As String = String.Empty

        ' Create Instance of Connection and Command Object
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_GetProveedorFromUser", myConnection)

        ' Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterUser As New SqlParameter("@User", SqlDbType.NVarChar, 50)
        parameterUser.Value = usuario
        myCommand.Parameters.Add(parameterUser)

        ' Execute the command
        myConnection.Open()
        Dim dr As SqlDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Return dr

    End Function

  

End Class
