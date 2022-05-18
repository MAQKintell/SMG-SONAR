Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class CaracteristicasDB


    Public Function GetCaracteristicasPorTipo(ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetTipoCarasteristicasPorTipoSol", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterTipo As New SqlParameter("@Cod_tipo", SqlDbType.NVarChar, 3)
        parameterTipo.Value = cod_tipo
        myCommand.SelectCommand.Parameters.Add(parameterTipo)

        Dim parameterSubtipo As New SqlParameter("@Cod_subtipo", SqlDbType.NVarChar, 3)
        parameterSubtipo.Value = cod_subtipo
        myCommand.SelectCommand.Parameters.Add(parameterSubtipo)

        Dim parameterEstado As New SqlParameter("@Cod_estado", SqlDbType.NVarChar, 3)
        parameterEstado.Value = cod_estado
        myCommand.SelectCommand.Parameters.Add(parameterEstado)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 3)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetCaracteristicasPorMotivoCancelacion(ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetTipoCarasteristicasclienteNoAceptaPresupuesto", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterTipo As New SqlParameter("@Cod_tipo", SqlDbType.NVarChar, 3)
        parameterTipo.Value = cod_tipo
        myCommand.SelectCommand.Parameters.Add(parameterTipo)

        Dim parameterSubtipo As New SqlParameter("@Cod_subtipo", SqlDbType.NVarChar, 3)
        parameterSubtipo.Value = cod_subtipo
        myCommand.SelectCommand.Parameters.Add(parameterSubtipo)

        Dim parameterEstado As New SqlParameter("@Cod_estado", SqlDbType.NVarChar, 3)
        parameterEstado.Value = cod_estado
        myCommand.SelectCommand.Parameters.Add(parameterEstado)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 3)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetCaracteristicasPorTipoTelefono() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetTipoCarasteristicasPorTipoSolTelefono", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function




    Public Function EliminarCaracteristica_e_Historico_Solicitud(ByVal id_solicitud As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_Eliminar_Caracteristicas_e_Historico_Solicitud", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@id_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.SelectCommand.Parameters.Add(parameterIDSolicitud)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function


    Public Function GetCaracteristicasSolicitud(ByVal id_solicitud As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetCarasteristicasSolicitud", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@id_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.SelectCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetMarcaModeloBySolicitud(ByVal id_solicitud As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GET_MARCA_MODELO_CARACTERISTICAS", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@id_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.SelectCommand.Parameters.Add(parameterIDSolicitud)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function


    Public Function GetCaracteristicasSolicitudGasConfortPdteAceptacionFinTrabajos(ByVal id_solicitud As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetCarasteristicasSolicitudGasConfortPdteAceptacionFinTrabajos", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@id_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.SelectCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetCaracteristicasSolicitudTelefono(ByVal id_solicitud As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetCarasteristicasSolicitudTelefono", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@id_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.SelectCommand.Parameters.Add(parameterIDSolicitud)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function


    'Resta 1 al contador de averias, para el subtipo 012 (sol averia Gas Confort) siempre que la el lugar
    'de la averia haya sido en el aparato.
    Public Function RestarContadorAveriaSegunLugarAveria(ByVal id_solicitd As Integer)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Try
            Dim myCommand As New SqlCommand("MTOGASBD_RestarContAv_SegunLugarAv", myConnection)
            myCommand.CommandType = CommandType.StoredProcedure


            Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
            parameterIDSolicitud.Value = id_solicitd
            myCommand.Parameters.Add(parameterIDSolicitud)



            myConnection.Open()
            myCommand.ExecuteNonQuery()
            myConnection.Close()


        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try




    End Function



    Public Function AddCaracteristica(ByVal id_solicitd As String, ByVal tip_car As String, ByVal valor As String, ByVal fecha_car As String) As Integer

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Try
            Dim myCommand As New SqlCommand("MTOGASBD_AddCarasteristicaSol", myConnection)
            myCommand.CommandType = CommandType.StoredProcedure


            Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
            parameterIDSolicitud.Value = id_solicitd
            myCommand.Parameters.Add(parameterIDSolicitud)

            Dim parameterTipo As New SqlParameter("@Tip_car", SqlDbType.NVarChar, 3)
            parameterTipo.Value = tip_car
            myCommand.Parameters.Add(parameterTipo)

            Dim parameterValor As New SqlParameter("@Valor", SqlDbType.NVarChar, 50)
            parameterValor.Value = valor
            myCommand.Parameters.Add(parameterValor)

            Dim parameterFechaCar As New SqlParameter("@Fecha_car", SqlDbType.DateTime)
            parameterFechaCar.Value = CType(fecha_car, DateTime)
            myCommand.Parameters.Add(parameterFechaCar)

            Dim parameterIDCaracteristica As New SqlParameter("@ID_caracteristica", SqlDbType.Int, 4)
            parameterIDCaracteristica.Direction = ParameterDirection.Output
            myCommand.Parameters.Add(parameterIDCaracteristica)

            myConnection.Open()
            myCommand.ExecuteNonQuery()
            myConnection.Close()

            Return CType(parameterIDCaracteristica.Value, Integer)
        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try




    End Function


    Public Sub UpdateCaracteristica(ByVal id_caracteristica As String, ByVal id_solicitd As String, ByVal tip_car As String, ByVal valor As String, ByVal fecha_car As String)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_UpdateCarasteristicaSol", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDCaracteristica As New SqlParameter("@ID_caracteristica", SqlDbType.Int, 4)
        parameterIDCaracteristica.Value = id_caracteristica
        myCommand.Parameters.Add(parameterIDCaracteristica)

        Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitd
        myCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterTipo As New SqlParameter("@Tip_car", SqlDbType.NVarChar, 3)
        parameterTipo.Value = tip_car
        myCommand.Parameters.Add(parameterTipo)

        Dim parameterValor As New SqlParameter("@Valor", SqlDbType.NVarChar, 50)
        parameterValor.Value = valor
        myCommand.Parameters.Add(parameterValor)

        Dim parameterFechaCar As New SqlParameter("@Fecha_car", SqlDbType.DateTime)
        parameterFechaCar.Value = CType(fecha_car, DateTime)
        myCommand.Parameters.Add(parameterFechaCar)



        myConnection.Open()
        Try
            myCommand.ExecuteNonQuery()
            myConnection.Close()
        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try






    End Sub

End Class
