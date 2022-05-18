Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class ProveedoresDB

    Public Function GetProveedorPorTipoSubtipo(ByVal Tipo As String, ByVal Subtipo As String, ByVal Contrato As String) As String

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetProveedorPorTipoSubtipo", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterTipo_Solicitud As New SqlParameter("@TIPOSOLICITUD", SqlDbType.NVarChar, 3)
        parameterTipo_Solicitud.Value = Tipo
        myCommand.SelectCommand.Parameters.Add(parameterTipo_Solicitud)

        Dim parameterSubTipo_Solicitud As New SqlParameter("@SUBTIPOSOLICITUD", SqlDbType.NVarChar, 3)
        parameterSubTipo_Solicitud.Value = Subtipo
        myCommand.SelectCommand.Parameters.Add(parameterSubTipo_Solicitud)

        Dim parameterContrato_Solicitud As New SqlParameter("@CONTRATO", SqlDbType.NVarChar, 10)
        parameterContrato_Solicitud.Value = Contrato
        myCommand.SelectCommand.Parameters.Add(parameterContrato_Solicitud)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)
        If myDataSet.Tables.Count > 0 Then
            Return myDataSet.Tables(0).Rows(0).Item(0).ToString() & ";" & myDataSet.Tables(0).Rows(0).Item(1).ToString()
        End If

    End Function

    Public Function GetProveedorPorTipoSubtipoMantenimiento(ByVal Tipo As String, ByVal Subtipo As String, ByVal Contrato As String) As String

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetProveedorPorTipoSubtipoMantenimiento", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterTipo_Solicitud As New SqlParameter("@TIPOSOLICITUD", SqlDbType.NVarChar, 3)
        parameterTipo_Solicitud.Value = Tipo
        myCommand.SelectCommand.Parameters.Add(parameterTipo_Solicitud)

        Dim parameterSubTipo_Solicitud As New SqlParameter("@SUBTIPOSOLICITUD", SqlDbType.NVarChar, 3)
        parameterSubTipo_Solicitud.Value = Subtipo
        myCommand.SelectCommand.Parameters.Add(parameterSubTipo_Solicitud)

        Dim parameterContrato_Solicitud As New SqlParameter("@CONTRATO", SqlDbType.NVarChar, 10)
        parameterContrato_Solicitud.Value = Contrato
        myCommand.SelectCommand.Parameters.Add(parameterContrato_Solicitud)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)
        If myDataSet.Tables.Count > 0 Then
            Return myDataSet.Tables(0).Rows(0).Item(0).ToString() & ";" & myDataSet.Tables(0).Rows(0).Item(1).ToString()
        End If

    End Function

    Public Function ObtenerNombreProveedor(ByVal codigo As String) As String
        If Not codigo = "" Then
            Dim myDataSet As New DataSet
            Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
            Dim adapter As New SqlDataAdapter()
            Dim cmdText As String = "SELECT nombre from proveedores where cod_proveedor='" & Mid(codigo, 1, 3) & "'"

            myConnection.Open()
            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

            Try
                sqlC.CommandTimeout = 20000
                adapter.SelectCommand = sqlC 'New SqlCommand(cmdText, myConnection)
                adapter.Fill(myDataSet)
                myConnection.Close()
            Catch ex As Exception
            Finally
                If (myConnection.State = ConnectionState.Open) Then
                    myConnection.Close()
                End If
            End Try

            Return myDataSet.Tables(0).Rows(0).Item(0).ToString()
        Else
            Return ""
        End If
    End Function

    Public Function GetCodigoProveedorPorNombre(ByVal nombre_proveedor As String) As SqlDataReader

        ' Create Instance of Connection and Command Object
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_GetSingleProveedores", myConnection)

        ' Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterNombre As New SqlParameter("@nombre", SqlDbType.NVarChar, 50)
        parameterNombre.Value = nombre_proveedor
        myCommand.Parameters.Add(parameterNombre)

        ' Execute the command
        myConnection.Open()
        Dim result As SqlDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

        ' Return the datareader 
        Return result

    End Function

End Class
