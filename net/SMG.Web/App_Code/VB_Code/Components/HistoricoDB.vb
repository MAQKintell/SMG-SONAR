Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class HistoricoDB




    Public Function EliminarHistoricoSolicitud(ByVal id_solicitud As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_EliminarHistoricosSolicitud", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@id_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.SelectCommand.Parameters.Add(parameterIDSolicitud)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function


   



    Public Function GetHistoricoSolicitud(ByVal id_solicitud As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetHistoricosSolicitud", myConnection)
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

    Public Function GetHistoricoCaracteristicasPorMovimiento(ByVal id_movimiento As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetHistorico_caracteristicas_PorMovimiento", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDMovimiento As New SqlParameter("@id_movimiento", SqlDbType.Int, 4)
        parameterIDMovimiento.Value = id_movimiento
        myCommand.SelectCommand.Parameters.Add(parameterIDMovimiento)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet


    End Function

    Public Function AddHistoricoSolicitud(ByVal id_solicitud As String, ByVal tipo_movimiento As String, ByVal usuario As String, ByVal cod_est_sol As String, ByVal observaciones As String, ByVal Proveedor As String) As Integer

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_AddHistorico", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@id_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterTipoMovimiento As New SqlParameter("@tipo_movimiento", SqlDbType.NVarChar, 3)
        parameterTipoMovimiento.Value = tipo_movimiento
        myCommand.Parameters.Add(parameterTipoMovimiento)

        Dim parameterUsuario As New SqlParameter("@usuario", SqlDbType.NChar, 10)
        parameterUsuario.Value = usuario
        myCommand.Parameters.Add(parameterUsuario)

        Dim parameterEstadoSolicitud As New SqlParameter("@estado_solicitud", SqlDbType.NVarChar, 3)
        parameterEstadoSolicitud.Value = cod_est_sol
        myCommand.Parameters.Add(parameterEstadoSolicitud)

        Dim parameterObservaciones As New SqlParameter("@observaciones", SqlDbType.NVarChar, 4000)
        parameterObservaciones.Value = observaciones
        myCommand.Parameters.Add(parameterObservaciones)

        Dim parameterProveedor As New SqlParameter("@proveedor", SqlDbType.NVarChar, 10)
        parameterProveedor.Value = Proveedor
        myCommand.Parameters.Add(parameterProveedor)

        Dim parameterIDMovimiento As New SqlParameter("@id_movimiento", SqlDbType.Int, 4)
        parameterIDMovimiento.Direction = ParameterDirection.Output
        myCommand.Parameters.Add(parameterIDMovimiento)

        Dim parameterFecha As New SqlParameter("@HORA", SqlDbType.DateTime, 20)
        parameterFecha.Value = Now()
        myCommand.Parameters.Add(parameterFecha)

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

        Return CType(parameterIDMovimiento.Value, Integer)

    End Function

    Public Function UpdateEstadoHistoricoSolicitud(ByVal id_solicitud As String, ByVal cod_est_sol_ant As String, ByVal cod_est_sol As String, ByVal observaciones As String, ByVal usuario As String) As Integer
        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("SP_UPDATE_ESTADO_HISTORICO", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@id_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterEstadoSolicitud As New SqlParameter("@estado_solicitud", SqlDbType.NVarChar, 3)
        parameterEstadoSolicitud.Value = cod_est_sol
        myCommand.Parameters.Add(parameterEstadoSolicitud)

        Dim parameterEstadoAnteriorSolicitud As New SqlParameter("@estadoAnterior_solicitud", SqlDbType.NVarChar, 3)
        parameterEstadoAnteriorSolicitud.Value = cod_est_sol_ant
        myCommand.Parameters.Add(parameterEstadoAnteriorSolicitud)

        Dim parameterObservaciones As New SqlParameter("@observaciones", SqlDbType.NVarChar, 4000)
        parameterObservaciones.Value = observaciones
        myCommand.Parameters.Add(parameterObservaciones)

        Dim parameterUsuario As New SqlParameter("@usuario", SqlDbType.NVarChar, 50)
        parameterUsuario.Value = usuario
        myCommand.Parameters.Add(parameterUsuario)

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

        Return 1
    End Function

    Public Function UpdateEstadoHistoricoVisita(ByVal cod_contrato As String, ByVal cod_Visita As Integer, ByVal cod_est_sol_ant As String, ByVal cod_est_sol As String) As Integer
        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("SP_UPDATE_ESTADO_HISTORICO_VISITA", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodContrato As New SqlParameter("@cod_contrato", SqlDbType.NVarChar, 10)
        parameterCodContrato.Value = cod_contrato
        myCommand.Parameters.Add(parameterCodContrato)

        Dim parameterCodVisita As New SqlParameter("@cod_visita", SqlDbType.Int, 4)
        parameterCodVisita.Value = cod_Visita
        myCommand.Parameters.Add(parameterCodVisita)

        Dim parameterEstadoSolicitud As New SqlParameter("@estado_solicitud", SqlDbType.NVarChar, 3)
        parameterEstadoSolicitud.Value = cod_est_sol
        myCommand.Parameters.Add(parameterEstadoSolicitud)

        Dim parameterEstadoAnteriorSolicitud As New SqlParameter("@estadoAnterior_solicitud", SqlDbType.NVarChar, 3)
        parameterEstadoSolicitud.Value = cod_est_sol_ant
        myCommand.Parameters.Add(parameterEstadoAnteriorSolicitud)

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

        Return 1
    End Function

    Public Sub AddHistoricoCaracteristicas(ByVal id_caracteristica As Integer, ByVal id_movimiento As String, ByVal tipo_campo As String, ByVal valor As String)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_AddHistorico_caracteristicas", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDMovimiento As New SqlParameter("@id_movimiento", SqlDbType.NChar, 10)
        parameterIDMovimiento.Value = id_movimiento
        myCommand.Parameters.Add(parameterIDMovimiento)

        Dim parameterTipoCampo As New SqlParameter("@tipo_campo", SqlDbType.NVarChar, 3)
        parameterTipoCampo.Value = tipo_campo
        myCommand.Parameters.Add(parameterTipoCampo)

        Dim parameterValor As New SqlParameter("@valor", SqlDbType.NVarChar, 50)
        parameterValor.Value = valor
        myCommand.Parameters.Add(parameterValor)


        Dim parameterIDCaracteristica As New SqlParameter("@id_caracteristica", SqlDbType.Int, 4)
        parameterIDCaracteristica.Value = id_caracteristica
        myCommand.Parameters.Add(parameterIDCaracteristica)


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
