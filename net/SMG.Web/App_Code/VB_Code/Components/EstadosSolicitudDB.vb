Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class EstadosSolicitudDB




    Public Function GetEstadosSolicitudPorTipoSubtipoCanal(ByVal tipo_solicitud As String, ByVal subtipo_solicitud As String, ByVal EstadoActual As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetEstadosSolicitudPorTipoSubtipoCanal", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterTipoSolicitud As New SqlParameter("@cod_tip_sol", SqlDbType.NVarChar, 3)
        If Not tipo_solicitud = "-1" Then
            parameterTipoSolicitud.Value = tipo_solicitud
        Else
            parameterTipoSolicitud.Value = System.DBNull.Value
        End If
        myCommand.SelectCommand.Parameters.Add(parameterTipoSolicitud)

        Dim parameterSubTipoSolicitud As New SqlParameter("@cod_sutip_sol", SqlDbType.NVarChar, 3)
        If Not subtipo_solicitud = "-1" Then
            parameterSubTipoSolicitud.Value = subtipo_solicitud
        Else
            parameterSubTipoSolicitud.Value = System.DBNull.Value
        End If
        myCommand.SelectCommand.Parameters.Add(parameterSubTipoSolicitud)

        Dim parameterEstadoSolicitud As New SqlParameter("@estado_actual", SqlDbType.NVarChar, 3)
        parameterEstadoSolicitud.Value = EstadoActual
        myCommand.SelectCommand.Parameters.Add(parameterEstadoSolicitud)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 3)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetEstadosSolicitudPorTipoSubtipo(ByVal tipo_solicitud As String, ByVal subtipo_solicitud As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetEstadosSolicitudPorTipoSubtipo", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterTipoSolicitud As New SqlParameter("@cod_tip_sol", SqlDbType.NVarChar, 3)
        If Not tipo_solicitud = "-1" Then
            parameterTipoSolicitud.Value = tipo_solicitud
        Else
            parameterTipoSolicitud.Value = System.DBNull.Value
        End If
        myCommand.SelectCommand.Parameters.Add(parameterTipoSolicitud)

        Dim parameterSubTipoSolicitud As New SqlParameter("@cod_sutip_sol", SqlDbType.NVarChar, 3)
        If Not subtipo_solicitud = "-1" Then
            parameterSubTipoSolicitud.Value = subtipo_solicitud
        Else
            parameterSubTipoSolicitud.Value = System.DBNull.Value
        End If
        myCommand.SelectCommand.Parameters.Add(parameterSubTipoSolicitud)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 3)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetEstadosSolicitudFuturos(ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal canal As Integer, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetEstadosSolicitudFuturo", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterTipo As New SqlParameter("@cod_tipo", SqlDbType.NVarChar, 3)
        parameterTipo.Value = cod_tipo
        myCommand.SelectCommand.Parameters.Add(parameterTipo)

        Dim parameterSubtipo As New SqlParameter("@cod_subtipo", SqlDbType.NVarChar, 3)
        parameterSubtipo.Value = cod_subtipo
        myCommand.SelectCommand.Parameters.Add(parameterSubtipo)

        Dim parameterEstado As New SqlParameter("@cod_estado", SqlDbType.NVarChar, 3)
        parameterEstado.Value = cod_estado
        myCommand.SelectCommand.Parameters.Add(parameterEstado)

        Dim parameterCanal As New SqlParameter("@CANAL", SqlDbType.Int, 1)
        parameterCanal.Value = canal
        myCommand.SelectCommand.Parameters.Add(parameterCanal)

        Dim parameterIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetEfvNumerovisitaSmgAmpliado(ByVal contrato As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("SP_GET_EFV_NUMEROVISITA_SMGAMPLIADO", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterTipo As New SqlParameter("@codcontrato", SqlDbType.NVarChar, 10)
        parameterTipo.Value = contrato
        myCommand.SelectCommand.Parameters.Add(parameterTipo)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function




End Class
