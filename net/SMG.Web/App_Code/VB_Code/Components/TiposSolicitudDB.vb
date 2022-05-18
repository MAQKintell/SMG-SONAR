Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class TiposSolicitudDB


    Public Function GetTipoSolicitudes() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetTipo_solicitudes", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetProvincias(ByVal Proveedor As String, ByVal idIdioma As Integer) As DataSet
        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()

        Dim sql As String = ""
        'sql = "	SELECT distinct Provincia.nombre as [NOM_PROVINCIA]"

        'sql &= "FROM [dbo].[Solicitudes] "
        'sql &= "inner join tipo_solicitud on tipo_solicitud.codigo = solicitudes.tipo_solicitud "
        'sql &= "inner join subtipo_solicitud on subtipo_solicitud.codigo = solicitudes.subtipo_solicitud "
        'sql &= "inner join estado_solicitud on estado_solicitud.codigo = solicitudes.estado_solicitud "
        'sql &= "inner join tipo_averia on tipo_averia.cod_averia = solicitudes.cod_averia "
        'sql &= "inner join mantenimiento on mantenimiento.COD_CONTRATO_SIC = solicitudes.Cod_contrato "
        ''***********************************************************************************************
        'sql &= "inner join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        'sql &= "inner join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia) "
        ''***********************************************************************************************


        'If Not String.IsNullOrEmpty(Proveedor) And Not Proveedor = "d" Then sql &= "where [Solicitudes].Proveedor = '" & Proveedor & "' "

        'sql &= "order by [NOM_PROVINCIA]"


        sql = "select distinct nombre as NOM_PROVINCIA,COD_PROVINCIA FROM provincia WHERE ID_IDIOMA='" & idIdioma & "' "
        sql &= "order by NOM_PROVINCIA"


        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(sql, myConnection)

            sqlC.CommandTimeout = 20000

            adapter.SelectCommand = sqlC
            adapter.Fill(myDataSet)
            myConnection.Close()
        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try

        'Dim myCommand As New SqlDataAdapter("MTOGASBD_GetProvincias", myConnection)
        'myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        'Dim parameterProveedor As New SqlParameter("@Proveedor", SqlDbType.NVarChar, 3)
        'parameterProveedor.Value = Proveedor
        'myCommand.SelectCommand.Parameters.Add(parameterProveedor)


        'Dim myDataSet As New DataSet()
        'myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetSubtipoSolicitudesPorEFV(ByVal codEFV As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetSubtipoSolicitudesPorEFV", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterCodEFV As New SqlParameter("@codEFV", SqlDbType.NVarChar, 10)
        parameterCodEFV.Value = codEFV
        myCommand.SelectCommand.Parameters.Add(parameterCodEFV)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 3)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function


    Public Function GetSubtipoSolicitudesPorTipo(ByVal tipo_solicitud As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetSubtipoSolicitudesPorTipo", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterTipoSolicitud As New SqlParameter("@cod_tip_sol", SqlDbType.NVarChar, 3)
        parameterTipoSolicitud.Value = tipo_solicitud
        myCommand.SelectCommand.Parameters.Add(parameterTipoSolicitud)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 3)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetSubtipoSolicitudes(ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetSubtipoSolicitudes", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 3)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetSubtipoSolicitudesPuedeAbrirProveedor(ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetSubtipoSolicitudesPuedeAbrirProveedor", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 3)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

End Class
