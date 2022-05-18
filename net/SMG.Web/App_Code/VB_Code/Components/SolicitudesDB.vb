Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class SolicitudesDB



    Public Function EliminarSolicitud(ByVal id_solicitud As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_EliminarSolicitud", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@id_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.SelectCommand.Parameters.Add(parameterIDSolicitud)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetSolicitudesPorContrato(ByVal cod_contrato As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetSolicitudesPorContrato", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterCod_contrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 10)
        parameterCod_contrato.Value = cod_contrato
        myCommand.SelectCommand.Parameters.Add(parameterCod_contrato)

        Dim parameteridIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameteridIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameteridIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function



    Public Function GetSolicitudesPorIDSolicitud(ByVal idSolicitud As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetSolicitudesPorSolicitud", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterCod_contrato As New SqlParameter("@idSolicitud", SqlDbType.NVarChar, 10)
        parameterCod_contrato.Value = idSolicitud
        myCommand.SelectCommand.Parameters.Add(parameterCod_contrato)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function


    Public Function GetNumeroContratosActivos() As Integer

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetNumeroContratosActivos", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure



        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet.Tables(0).Rows(0).Item(0).ToString()

    End Function

    Public Function ObtenerProveedorUltimoMovimientoSolicitud(ByVal idSolicitud As Integer) As String

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("SP_GET_PROVEEDOR_ULTIMO_MOVIMIENTO_SOLICITUD", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterIdSolicitud As New SqlParameter("@ID_SOLICITUD", SqlDbType.Int)
        parameterIdSolicitud.Value = idSolicitud
        myCommand.SelectCommand.Parameters.Add(parameterIdSolicitud)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet.Tables(0).Rows(0).Item(0).ToString()

    End Function



    ' ''Public Function GetSolicitudesPorProveedor(ByVal proveedor As String) As DataSet

    ' ''    Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
    ' ''    Dim myCommand As New SqlDataAdapter("MTOGASBD_GetSolicitudesPorProveedor", myConnection)
    ' ''    myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

    ' ''    ' Add Parameters to SPROC
    ' ''    Dim parameterProveedor As New SqlParameter("@Proveedor", SqlDbType.NVarChar, 3)
    ' ''    parameterProveedor.Value = proveedor
    ' ''    myCommand.SelectCommand.Parameters.Add(parameterProveedor)


    ' ''    Dim myDataSet As New DataSet()
    ' ''    myCommand.Fill(myDataSet)

    ' ''    Return myDataSet

    ' ''End Function

    Public Function EjecutarSentencia(ByVal SQL As String)
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()

        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(SQL, myConnection)

            sqlC.CommandTimeout = 20000
            sqlC.ExecuteNonQuery()
            myConnection.Close()
        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try
    End Function

    Public Function ObtenerDatosDesdeSentencia(ByVal SQL As String) As DataSet
        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()

        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(SQL, myConnection)

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
        Return myDataSet
    End Function

    'Public Function GetSolicitudesPorProveedorContador(ByVal proveedor As String, ByVal cod_contrato As String, ByVal consultaPendietes As Boolean, ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal Provincia As String, ByVal Urgente As String, ByVal Perfil As String, ByVal id_solicitud As String) As Integer

    '    Dim myDataSet As New DataSet
    '    Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
    '    Dim adapter As New SqlDataAdapter()
    '    Dim cmdText As String = String.Empty
    '    cmdText += "SELECT Distinct count(*) "

    '    cmdText += " FROM [dbo].[Solicitudes] "
    '    cmdText += "left join motivo_cancelacion on Solicitudes.Mot_cancel=COALESCE (motivo_cancelacion.cod_Mot , motivo_cancelacion.cod_Mot) "
    '    cmdText += "left join tipo_solicitud on tipo_solicitud.codigo = solicitudes.tipo_solicitud "
    '    cmdText += "left join subtipo_solicitud on subtipo_solicitud.codigo = solicitudes.subtipo_solicitud "
    '    cmdText += "left join estado_solicitud on estado_solicitud.codigo = solicitudes.estado_solicitud "
    '    cmdText += "left join tipo_averia on tipo_averia.cod_averia = solicitudes.cod_averia "
    '    cmdText += "left join mantenimiento on mantenimiento.COD_CONTRATO_SIC = solicitudes.Cod_contrato "
    '    '09/08/2010
    '    'Kintell tema busqueda por cups.
    '    'If Not String.IsNullOrEmpty(cod_contrato) And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
    '    cmdText += "left join relacion_cups_contrato RCC on rcc.cups=mantenimiento.Cups "

    '    cmdText += "left join visita v on v.cod_contrato=mantenimiento.cod_contrato_sic and v.cod_visita=mantenimiento.cod_ultima_visita "
    '    cmdText += "left join tipo_estado_visita t on t.cod_estado=v.cod_estado_visita "
    '    cmdText += "LEFT JOIN TIPO_URGENCIA TU ON TU.ID_TIPO_URGENCIA=V.ID_TIPO_URGENCIA "
    '    'End If
    '    '********
    '    cmdText += "left join calderas on calderas.cod_contrato = solicitudes.Cod_contrato "
    '    '***********************************************************************************************
    '    cmdText += "left join TIPO_MARCA_CALDERA on TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "
    '    cmdText += "left join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
    '    cmdText += "left join poblacion on poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia "
    '    '***********************************************************************************************
    '    cmdText += "left join caracteristicas_solicitud on caracteristicas_solicitud.id_solicitud=solicitudes.id_solicitud and (caracteristicas_solicitud.tip_car='002' or caracteristicas_solicitud.tip_car='010')"

    '    'COLORES (perfiles)
    '    If cod_subtipo <> "006" And cod_subtipo <> "005" Then
    '        '24/04/2012 Kintell mail de Alex.
    '        'cmdText += "left join historico h on h.id_solicitud=Solicitudes.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=Solicitudes.id_solicitud) "
    '        'cmdText += "left join historico h on h.id_solicitud=Solicitudes.id_solicitud and h.time_stamp=(select min(time_stamp) from historico inner join estado_solicitud es on historico.estado_solicitud=es.codigo where es.estado_final=1 and historico.id_solicitud=Solicitudes.id_solicitud) "
    '        cmdText += "left join historico h on h.id_solicitud=Solicitudes.id_solicitud and h.time_stamp=(select min(time_stamp) from historico where id_solicitud=Solicitudes.id_solicitud and estado_solicitud in (SELECT top 1 estado_solicitud from historico where id_solicitud=Solicitudes.id_solicitud and (estado_solicitud is not null and estado_solicitud<>'' and estado_solicitud not in('021')) order by time_stamp desc)) "
    '    Else
    '        If cod_subtipo = "006" Then
    '            cmdText += "left join historico h on h.id_solicitud=solicitudes.id_solicitud and h.estado_solicitud='026' "
    '        Else
    '            cmdText += "left join historico h on h.id_solicitud=solicitudes.id_solicitud and h.estado_solicitud='014' "
    '        End If
    '    End If
    '    cmdText += "left join usuarios u on u.UserID=h.usuario "


    '    cmdText += "WHERE "

    '    If Not String.IsNullOrEmpty(proveedor) And Not UCase(proveedor) = "D" Then
    '        cmdText += "[Solicitudes].Proveedor = '" + proveedor + "' AND "
    '    End If

    '    If Not String.IsNullOrEmpty(cod_contrato) Then ' And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
    '        '09/08/2010
    '        'Kintell por tema busqueda por cups.
    '        'cmdText += "[Solicitudes].[Cod_contrato] like '%" + cod_contrato + "%' AND "
    '        cmdText += "[RCC].[Cod_contrato_SIC] = '" + cod_contrato + "' AND "
    '        '**********
    '    ElseIf Not String.IsNullOrEmpty(cod_contrato) Then
    '        cmdText += "[Solicitudes].[Cod_contrato] = '" + cod_contrato + "' AND "
    '    End If

    '    If Not String.IsNullOrEmpty(id_solicitud) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
    '        cmdText += "[Solicitudes].[id_solicitud] = " + id_solicitud + " AND "
    '    End If

    '    If Not String.IsNullOrEmpty(Urgente) Then
    '        If Not UCase(Urgente) = UCase("false") Then cmdText += "[Solicitudes].[Urgente] ='" + (Urgente) + "' AND "
    '    End If
    '    'If consultaPendietes Then
    '    '    Dim solicitudCerradaCancelada As String() = {"009", "010", "011", "012", "013", "014", "015", "17", "18"}
    '    '    For Each estado As String In solicitudCerradaCancelada
    '    '        cmdText += "[Solicitudes].Estado_solicitud <> '" + estado + "' AND "
    '    '    Next
    '    'End If

    '    'GGB
    '    If consultaPendietes Then
    '        ' para subtipo de avisos averia
    '        'cmdText += "(([Solicitudes].[subtipo_solicitud] = '001' or [Solicitudes].[subtipo_solicitud] = '004') AND [Solicitudes].Estado_solicitud <> '010' AND [Solicitudes].Estado_solicitud <> '011' AND [Solicitudes].Estado_solicitud <> '012' OR "
    '        ' para el sutbtipo de solicitud de visita
    '        'cmdText += "([Solicitudes].[subtipo_solicitud] = '002' or [Solicitudes].[subtipo_solicitud] = '003') AND [Solicitudes].Estado_solicitud <> '012' AND [Solicitudes].Estado_solicitud <> '014') AND "
    '        ''cmdText += "([Solicitudes].Estado_solicitud < '008' OR [Solicitudes].Estado_solicitud > '012') AND "
    '        cmdText += "([Solicitudes].Estado_solicitud <> '010' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '011' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '012' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '014' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '021' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '018' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '022' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '023' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '025' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '026' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '029' "
    '        cmdText += "AND [Solicitudes].Estado_solicitud <> '020') AND "
    '    End If

    '    'Kintell
    '    '18/08/2010
    '    If Not cod_tipo = "-1" Then
    '        cmdText += "[Solicitudes].[Tipo_solicitud] = '" + cod_tipo + "' AND "
    '    End If
    '    'cmdText += "[Solicitudes].[Tipo_solicitud] = '001' AND "

    '    If Not cod_subtipo = "-1" Then
    '        cmdText += "[Solicitudes].[subtipo_solicitud] = '" + cod_subtipo + "' AND "
    '    End If

    '    If Not cod_estado = "-1" Then
    '        cmdText += "[Solicitudes].[Estado_solicitud] = '" + cod_estado + "' AND "
    '    End If

    '    If Not String.IsNullOrEmpty(fechaDesde) Then
    '        cmdText += "[Solicitudes].[Fecha_creacion] >= '" + fechaDesde + "' AND "
    '    End If

    '    If Not String.IsNullOrEmpty(Provincia) Then
    '        'cmdText += "[provincia].[nombre] like '%" + Provincia + "%' AND "
    '        cmdText += "[provincia].[cod_provincia] = " + Provincia + " AND "
    '    End If

    '    'If Not String.IsNullOrEmpty(fechaHasta) Then
    '    '    cmdText += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + "' AND "
    '    'End If
    '    If Not String.IsNullOrEmpty(fechaHasta) Then
    '        cmdText += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + " 23:59:59.999' AND "
    '    End If

    '    'If Not caldera = "-1" Then
    '    '    cmdText += "[Calderas].[Marca_Caldera] = '" + caldera + "' AND "
    '    'End If

    '    If Not Perfil = "0" Then
    '        cmdText += "id_perfil in (" & Perfil & ") AND "
    '    End If

    '    If Right(cmdText.Trim(), 3) = "AND" Then
    '        cmdText = cmdText.Remove(cmdText.Length - 4)
    '    End If

    '    If Right(cmdText.Trim(), 5) = "WHERE" Then
    '        cmdText = cmdText.Remove(cmdText.Length - 6)
    '    End If

    '    'cmdText += "order by [Solicitudes].[Fecha_creacion] desc OPTION (FORCE ORDER)"
    '    Dim Contador As Integer = 0
    '    myConnection.Open()
    '    Try

    '        Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

    '        sqlC.CommandTimeout = 20000
    '        adapter.SelectCommand = sqlC 'New SqlCommand(cmdText, myConnection)
    '        adapter.Fill(myDataSet)
    '        If (myDataSet.Tables.Count > 0) Then
    '            If (myDataSet.Tables(0).Rows.Count > 0) Then
    '                Contador = myDataSet.Tables(0).Rows(0).Item(0)
    '            End If
    '        End If

    '        myConnection.Close()


    '    Catch ex As Exception
    '    Finally
    '        If (myConnection.State = ConnectionState.Open) Then
    '            myConnection.Close()
    '        End If

    '    End Try
    '    Return Contador
    'End Function

    Public Function GetSolicitudesPorProveedorContador(ByVal proveedor As String, ByVal cod_contrato As String, ByVal consultaPendietes As Boolean, ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal Provincia As String, ByVal Urgente As String, ByVal Desde As String, ByVal Hasta As String, ByVal Perfil As String, ByVal Id_Solicitud As String, Optional ByVal Nombre As String = "", Optional ByVal Apellido1 As String = "", Optional ByVal Apellido2 As String = "", Optional ByVal Colores As String = "0", Optional ByVal PAIS As String = "ES") As String

        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty
        cmdText = "select count (distinct v1.id_solicitud) from "
        cmdText += "(select v2.[id_solicitud] "
        cmdText += "from (select distinct [Solicitudes].[id_solicitud] "
        cmdText += "FROM [dbo].[Solicitudes] "
        'cmdText += "inner join mantenimiento on mantenimiento.COD_CONTRATO_SIC = solicitudes.Cod_contrato "
        'cmdText += "inner join relacion_cups_contrato RCC on rcc.COD_RECEPTOR=mantenimiento.COD_RECEPTOR "

        cmdText += "INNER JOIN relacion_cups_contrato RCC ON Solicitudes.Cod_contrato=RCC.COD_CONTRATO_SIC "
        cmdText += "INNER JOIN mantenimiento ON RCC.COD_RECEPTOR=MANTENIMIENTO.COD_RECEPTOR "

        cmdText += "inner join visita vi on vi.cod_contrato=mantenimiento.cod_contrato_sic " 'and vi.cod_visita=mantenimiento.cod_ultima_visita "
        'cmdText += "left join tipo_solicitud on tipo_solicitud.codigo = solicitudes.tipo_solicitud "
        'cmdText += "left join subtipo_solicitud on subtipo_solicitud.codigo = solicitudes.subtipo_solicitud "
        'cmdText += "left join estado_solicitud on estado_solicitud.codigo = solicitudes.estado_solicitud "
        'cmdText += "left join tipo_averia on tipo_averia.cod_averia = solicitudes.cod_averia "
        'cmdText += "left join calderas on calderas.cod_contrato = solicitudes.Cod_contrato "
        'cmdText += "left join TIPO_MARCA_CALDERA on TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "
        'cmdText += "left join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        'cmdText += "left join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia"
        'cmdText += ") "

        'cmdText += "left join historico h on h.id_solicitud=solicitudes.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=solicitudes.id_solicitud) "
        'cmdText += "left join usuarios u on u.UserID=h.usuario "


        cmdText += "WHERE "

        If Not Perfil = "0" And Not Perfil = "4" Then
            cmdText += "mantenimiento.PAIS='" + Left(PAIS, 2).ToUpper() + "' AND "

        End If



        If Not String.IsNullOrEmpty(proveedor) And Not UCase(proveedor) = "REC" And Not UCase(proveedor) = "TEL" And Not UCase(proveedor) = "D" And Not UCase(proveedor) = "ADICO" Then
            cmdText += "[Solicitudes].Proveedor = '" + proveedor + "' AND "
        End If

        'If Not Colores = "0" Then
        '    cmdText += "u.ID_Perfil in (" + Perfil + ") AND "
        'End If

        If Not String.IsNullOrEmpty(cod_contrato) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            'cmdText += "[RCC].[Cod_contrato_SIC] = '" + cod_contrato + "' AND "
            '28/10/2015
            cmdText += "[RCC].[Cod_contrato_SIC] in (select COD_CONTRATO_SIC from RELACION_CUPS_CONTRATO where COD_RECEPTOR in (select COD_RECEPTOR from RELACION_CUPS_CONTRATO where COD_CONTRATO_SIC='" + cod_contrato + "')) AND "
        ElseIf Not String.IsNullOrEmpty(cod_contrato) Then
            cmdText += "[Solicitudes].[Cod_contrato] = '" + cod_contrato + "' AND "
        End If

        If Not String.IsNullOrEmpty(Id_Solicitud) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[id_solicitud] = " + Id_Solicitud + " AND "
        End If

        If Not String.IsNullOrEmpty(Urgente) Then
            If Not UCase(Urgente) = UCase("false") Then cmdText += "[Solicitudes].[Urgente] ='" + (Urgente) + "' AND "
        End If

        If Not String.IsNullOrEmpty(Nombre) Then
            cmdText += "NOM_TITULAR like '%" + Nombre + "%' AND "
        End If
        If Not String.IsNullOrEmpty(Apellido1) Then
            cmdText += "APELLIDO1 like '%" + Apellido1 + "%' AND "
        End If
        If Not String.IsNullOrEmpty(Apellido2) Then
            cmdText += "APELLIDO2 like '%" + Apellido2 + "%' AND "
        End If
        'GGB
        If consultaPendietes Then
            If Not UCase(proveedor) = "TEL" Then
                ' para subtipo de avisos averia
                cmdText += "([Solicitudes].Estado_solicitud <> '010' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '011' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '012' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '014' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '021' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '018' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '022' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '023' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '025' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '026' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '029' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '036' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '037' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '043' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '047' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '032' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '049' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '050' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '051' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '052' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '053' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '054' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '055' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '056' "

                cmdText += "AND [Solicitudes].Estado_solicitud <> '020') AND "
            Else
                'cmdText += "[Solicitudes].[subtipo_solicitud] = '005' AND "
            End If
        End If

        If Not cod_tipo = "-1" Then
            cmdText += "[Solicitudes].[Tipo_solicitud] = '" + cod_tipo + "' AND "
        End If

        If Not cod_subtipo = "-1" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] = '" + cod_subtipo + "' AND "
        End If
        If UCase(proveedor) = "TEL" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] <> '009' AND "
        End If
        If UCase(proveedor) = "REC" Then
            'cmdText += "[Solicitudes].[subtipo_solicitud] = '009' AND "
        End If

        If Not cod_estado = "-1" Then
            cmdText += "[Solicitudes].[Estado_solicitud] = '" + cod_estado + "' AND "
        End If

        If Not String.IsNullOrEmpty(Provincia) Then
            cmdText += "[provincia].[cod_provincia] = " + Provincia + " AND "
        End If

        If Not String.IsNullOrEmpty(fechaDesde) Then
            cmdText += "[Solicitudes].[Fecha_creacion] >= '" + fechaDesde + "' AND "
        End If
        If Not String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + " 23:59:59.999' AND "
        End If

        If Right(cmdText.Trim(), 3) = "AND" Then
            cmdText = cmdText.Remove(cmdText.Length - 4)
        End If

        If Right(cmdText.Trim(), 5) = "WHERE" Then
            cmdText = cmdText.Remove(cmdText.Length - 6)
        End If

        cmdText += ") as V2 "

        If Not Perfil = "0" And Not Perfil = "4" Then
            'cmdText += "where id_perfil in (" & Perfil & ")"
        ElseIf Not Perfil = "4" Then
            cmdText += "inner join visita vi on vi.cod_contrato=v2.cod_contrato_sic and vi.cod_visita=v2.cod_ultima_visita "
            cmdText += "left join historico h on h.id_solicitud=v2.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=v2.id_solicitud) "
            cmdText += "left join usuarios u on u.UserID=h.usuario "
        End If
        If (cod_subtipo = "006" And cod_estado = "026") Or (cod_subtipo = "005" And cod_estado = "014") Then
            If (cod_subtipo = "006" And cod_estado = "026") Then
                cmdText += ")as V1 left join historico h on h.id_solicitud=v1.id_solicitud and h.estado_solicitud='026' "
            Else
                cmdText += ")as V1 left join historico h on h.id_solicitud=v1.id_solicitud and h.estado_solicitud='014' "
            End If
        Else
            cmdText += ")as V1 "
        End If

        Dim Contador As String = "0"
        Try
            myConnection.Open()

            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

            sqlC.CommandTimeout = 20000
            adapter.SelectCommand = sqlC 'New SqlCommand(cmdText, myConnection)
            adapter.Fill(myDataSet)
            If (myDataSet.Tables.Count > 0) Then
                If (myDataSet.Tables(0).Rows.Count > 0) Then
                    Contador = myDataSet.Tables(0).Rows(0).Item(0)
                End If
            End If

            myConnection.Close()


        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If

        End Try
        Return Contador

    End Function

    Public Function GetSolicitudesPorProveedorExcell(ByVal proveedor As String, ByVal cod_contrato As String, ByVal consultaPendietes As Boolean, ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal Provincia As String, ByVal Urgente As String, ByVal Perfil As String, ByVal id_solicitud As String, Optional ByVal idIdioma As String = "0", Optional ByVal PAIS As String = "0") As DataSet

        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty
        cmdText = "SELECT Distinct "
        cmdText += "[Solicitudes].[ID_solicitud]"
        cmdText += ",[Solicitudes].[Cod_contrato]"

        If cod_subtipo <> "008" Then
            cmdText += ",[Solicitudes].[Tipo_solicitud]"
            cmdText += ",tipo_solicitud.descripcion as Des_Tipo_solicitud"
            cmdText += ",[Solicitudes].[subtipo_solicitud]"
            cmdText += ",subtipo_solicitud.descripcion as Des_subtipo_solicitud"
            cmdText += ",[Solicitudes].[Fecha_creacion]"
            cmdText += ",h.time_stamp as Fecha_cierre"
            cmdText += ",[Solicitudes].[Estado_solicitud]"
            cmdText += ",estado_solicitud.descripcion as Des_Estado_solicitud"
            cmdText += ",[Solicitudes].[Subestado_solicitud]"
            cmdText += ",[Solicitudes].[telefono_contacto]"
            cmdText += ",[mantenimiento].NUM_MOVIL " 'TEL ADD:
            cmdText += ",tipo_averia.descripcion_averia as des_averia"
            cmdText += ",[Solicitudes].[Persona_contacto]"
            cmdText += ",[Solicitudes].[Observaciones]"
            cmdText += ",[Solicitudes].[Proveedor]"
            cmdText += ",case when [Solicitudes].[subtipo_solicitud] ='008' then (select Valor from caracteristicas_solicitud where id_solicitud=[Solicitudes].[ID_solicitud] and Activo=1 and Tip_car in ('067','071')) else [motivo_cancelacion].[Descripcion] end as [Descripción cancelación] "
            cmdText += ",mantenimiento.urgencia"
            cmdText += ",mantenimiento.FEC_LIMITE_VISITA"
            cmdText += ",TIPO_MARCA_CALDERA.desc_marca_caldera as Marca_Caldera"
            cmdText += ",provincia.NOMBRE AS NOM_PROVINCIA"
            cmdText += ",[Solicitudes].[Urgente]"
            cmdText += ",POBLACION.NOMBRE AS [NOM_POBLACION]"
            cmdText += ",[mantenimiento].[NOM_CALLE]"
            cmdText += ",[mantenimiento].[COD_PORTAL]"
            cmdText += ",[mantenimiento].[TIP_PISO]"
            cmdText += ",[mantenimiento].[TIP_MANO]"
            cmdText += ",[mantenimiento].[COD_POSTAL]"
            cmdText += ",[mantenimiento].[COD_RECEPTOR]"
            cmdText += ",[mantenimiento].[CONTADOR_AVERIAS]"
            cmdText += ",t.des_estado as 'Estado ultima visita'"
            cmdText += ",TU.DESC_TIPO_URGENCIA AS 'Urgencia ultima visita'"
            cmdText += ",mantenimiento.t1 as 'Mant. Gas Calefaccion',mantenimiento.t2 as 'Mant. Gas'"
            cmdText += ",[mantenimiento].[HORARIO_CONTACTO]"
            cmdText += ",v.OBSERVACIONESVISITA "
            If cod_subtipo = "006" Then
                cmdText += ",h.time_stamp as Fecha_Realizada_Visita"
            End If
            cmdText += ",mantenimiento.TELEFONO_CLIENTE1 "
            cmdText += ",TEFV.DESCRIPCION_EFV as EFV "
            cmdText += ",MANTENIMIENTO.SCORING as 'S.PROV' "
            cmdText += ",tcv.DESC_CATEGORIA_VISITA as 'CATEGORIA VISITA' "
            '//--**************

            '#2095: Pago Anticipado
            cmdText += ",tla.DESC_LUGAR_AVERIA AS 'LUGAR_AVERIA'"
        Else
            'EAF REM BEG 17/11/2015 No sacar caracteristicas de precio, modelo caldera
            'cmdText += ",caracteristicas_solicitud.valor"
            'cmdText += ",caracteristicas_solicitud.Tip_car "
            'EAF REM END 17/11/2015 No sacar caracteristicas de precio, modelo caldera
            cmdText += ",MANTENIMIENTO.CONTADOR_AVERIAS"
            'EAF REM BEG 17/11/2015 No sacar caracteristicas de precio, modelo caldera
            'cmdText += ",TC.Descripcion "
            'EAF REM END 17/11/2015 No sacar caracteristicas de precio, modelo caldera
            cmdText += ",case when v.TIPOVISITA ='1' then 'REDUCIDA' else 'RITE' end as TIPO_VISITA"
            cmdText += ",V.OBSERVACIONES as observacionesvisita"
            cmdText += ",[Solicitudes].NUM_FACTURA"
            'cmdText += ",CASE WHEN [Solicitudes].[subtipo_solicitud] = '008' THEN (SELECT top 1 Valor FROM caracteristicas_solicitud WHERE id_solicitud = [Solicitudes].[ID_solicitud] AND Tip_car IN ('073') order by fecha_car desc) ELSE '' END AS [Descripción caldera]"
            'cmdText += ",CASE WHEN [Solicitudes].[subtipo_solicitud] = '008' THEN (SELECT top 1 Valor FROM caracteristicas_solicitud WHERE id_solicitud = [Solicitudes].[ID_solicitud] AND Tip_car IN ('074') order by fecha_car desc) ELSE '' END AS [Precio caldera]"
            'cmdText += ",CASE WHEN [Solicitudes].[subtipo_solicitud] = '008' THEN (SELECT top 1 Valor FROM caracteristicas_solicitud WHERE id_solicitud = [Solicitudes].[ID_solicitud] AND Tip_car IN ('079') order by fecha_car desc) ELSE '' END AS [Precio instalación]"
            cmdText += ",tipo_solicitud.descripcion as Des_Tipo_solicitud"
            cmdText += ",subtipo_solicitud.descripcion as Des_subtipo_solicitud"
            cmdText += ",[Solicitudes].[Fecha_creacion]"
            cmdText += ",h.time_stamp as Fecha_cierre"

            cmdText += ",estado_solicitud.descripcion as Des_Estado_solicitud"

            cmdText += ",[Solicitudes].[telefono_contacto]"
            cmdText += ",[mantenimiento].NUM_MOVIL " 'TEL ADD:
            cmdText += ",[Solicitudes].[Persona_contacto]"

            cmdText += ",[Solicitudes].[Observaciones]"
            cmdText += ",[Solicitudes].[Proveedor]"
            ' cmdText += ",case when [Solicitudes].[subtipo_solicitud] ='008' then (select Valor from caracteristicas_solicitud where id_solicitud=[Solicitudes].[ID_solicitud] and Activo=1 and Tip_car in ('067','071')) else [motivo_cancelacion].[Descripcion] end as [Descripción cancelación] "

            cmdText += ",mantenimiento.FEC_LIMITE_VISITA"
            cmdText += ",TIPO_MARCA_CALDERA.desc_marca_caldera as Marca_Caldera"
            cmdText += ",provincia.NOMBRE AS NOM_PROVINCIA"

            cmdText += ",POBLACION.NOMBRE AS [NOM_POBLACION]"
            cmdText += ",[mantenimiento].[NOM_CALLE]"
            cmdText += ",[mantenimiento].[COD_PORTAL]"
            cmdText += ",[mantenimiento].[TIP_PISO]"
            cmdText += ",[mantenimiento].[TIP_MANO]"
            cmdText += ",[mantenimiento].[COD_POSTAL]"
            cmdText += ",[mantenimiento].[COD_RECEPTOR]"
            cmdText += ",[mantenimiento].[CONTADOR_AVERIAS]"
            cmdText += ",t.des_estado as 'Estado ultima visita'"
            cmdText += ",TU.DESC_TIPO_URGENCIA AS 'Urgencia ultima visita'"
            cmdText += ",mantenimiento.t1 as 'Mant. Gas Calefaccion',mantenimiento.t2 as 'Mant. Gas'"

            If cod_subtipo = "006" Then
                cmdText += ",h.time_stamp as Fecha_Realizada_Visita"
            End If




            cmdText += ",mantenimiento.TELEFONO_CLIENTE1 "
            cmdText += ",TEFV.DESCRIPCION_EFV as EFV "
            'cmdText += ",MANTENIMIENTO.SCORING as 'S.PROV' "
            cmdText += ",TIPO_SOLCENT.RESULTADO as 'S.DEF' "

            '#2095: Pago Anticipado
            cmdText += ",tla.DESC_LUGAR_AVERIA AS 'LUGAR_AVERIA'"

        End If


        cmdText += " FROM [dbo].[Solicitudes] "
        cmdText += " INNER JOIN relacion_cups_contrato RCC ON RCC.COD_CONTRATO_SIC = Solicitudes.Cod_contrato "
        cmdText += " INNER JOIN mantenimiento ON RCC.COD_RECEPTOR = MANTENIMIENTO.COD_RECEPTOR "
        cmdText += " LEFT JOIN TIPO_SOLCENT ON MANTENIMIENTO.SOLCENT = TIPO_SOLCENT.COD_SOLCENT "
        cmdText += " LEFT JOIN visita v ON v.cod_contrato = mantenimiento.cod_contrato_sic AND v.cod_visita = mantenimiento.cod_ultima_visita "
        cmdText += " LEFT JOIN motivo_cancelacion ON Solicitudes.Mot_cancel = COALESCE(motivo_cancelacion.cod_Mot, motivo_cancelacion.cod_Mot) and motivo_cancelacion.id_idioma=" + idIdioma + " "

        cmdText += " LEFT JOIN tipo_solicitud ON tipo_solicitud.codigo = solicitudes.tipo_solicitud "
        cmdText += " LEFT JOIN estado_solicitud ON estado_solicitud.codigo = solicitudes.estado_solicitud "
        cmdText += " LEFT JOIN TIPO_LUGAR_AVERIA tla ON tla.id_lugar_averia = solicitudes.tip_lugar_averia and tla.id_idioma=estado_solicitud.id_idioma "
        cmdText += " LEFT JOIN TIPO_CATEGORIA_VISITA tcv ON tcv.COD_CATEGORIA_VISITA = v.COD_CATEGORIA_VISITA and tcv.id_idioma =estado_solicitud.id_idioma "
        cmdText += " LEFT JOIN subtipo_solicitud ON subtipo_solicitud.codigo = solicitudes.subtipo_solicitud AND subtipo_solicitud.id_idioma = estado_solicitud.id_idioma "
        cmdText += " LEFT JOIN tipo_averia ON tipo_averia.cod_averia = solicitudes.cod_averia and ididioma=estado_solicitud.id_idioma "
        cmdText += " LEFT JOIN TIPO_EFV AS TEFV ON mantenimiento.EFV = TEFV.COD_EFV "
        cmdText += " LEFT JOIN tipo_estado_visita t ON t.cod_estado = v.cod_estado_visita and T.id_idioma =estado_solicitud.id_idioma "
        cmdText += " LEFT JOIN TIPO_URGENCIA TU ON TU.ID_TIPO_URGENCIA = V.ID_TIPO_URGENCIA and TU.id_idioma =estado_solicitud.id_idioma "
        cmdText += " LEFT JOIN calderas ON calderas.cod_contrato = solicitudes.Cod_contrato "
        cmdText += " LEFT JOIN TIPO_MARCA_CALDERA ON TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "
        cmdText += " LEFT JOIN provincia ON provincia.cod_provincia = mantenimiento.Cod_provincia "
        cmdText += " LEFT JOIN poblacion ON poblacion.cod_poblacion = mantenimiento.Cod_poblacion AND poblacion.cod_provincia = mantenimiento.Cod_provincia "

        'EAF REM BEG 17/11/2015 No sacar caracteristicas de precio, modelo caldera
        'cmdText += "left join caracteristicas_solicitud on caracteristicas_solicitud.id_solicitud=solicitudes.id_solicitud and (caracteristicas_solicitud.tip_car='002' or caracteristicas_solicitud.tip_car='010' or caracteristicas_solicitud.tip_car='073' or caracteristicas_solicitud.tip_car='074' or caracteristicas_solicitud.tip_car='079') "
        'cmdText += " LEFT JOIN tipo_Caracteristica TC ON caracteristicas_solicitud.Tip_car=TC.tip_car "
        'EAF REM END 17/11/2015 No sacar caracteristicas de precio, modelo caldera
        'COLORES (perfiles)
        If cod_subtipo <> "006" And cod_subtipo <> "005" Then
            '24/04/2012 Kintell mail de Alex.
            'cmdText += "left join historico h on h.id_solicitud=Solicitudes.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=Solicitudes.id_solicitud) "
            'cmdText += "left join historico h on h.id_solicitud=Solicitudes.id_solicitud and h.time_stamp=(select min(time_stamp) from historico inner join estado_solicitud es on historico.estado_solicitud=es.codigo where es.estado_final=1 and historico.id_solicitud=Solicitudes.id_solicitud) "
            cmdText += "left join historico h on h.id_solicitud=Solicitudes.id_solicitud and h.time_stamp=(select min(time_stamp) from historico where id_solicitud=Solicitudes.id_solicitud and estado_solicitud in (SELECT top 1 estado_solicitud from historico where id_solicitud=Solicitudes.id_solicitud and (estado_solicitud is not null and estado_solicitud<>'' and estado_solicitud<>'-1' and estado_solicitud not in('021')) order by time_stamp desc)) "
        Else
            If cod_subtipo = "006" Then
                cmdText += "left join historico h on h.id_solicitud=solicitudes.id_solicitud and h.estado_solicitud='026' "
            Else
                cmdText += "left join historico h on h.id_solicitud=solicitudes.id_solicitud and h.estado_solicitud='014' "
            End If
        End If
        cmdText += "left join usuarios u on u.UserID=h.usuario "


        cmdText += "WHERE "



        '07/06/2016 IDIOMA
        cmdText += "estado_solicitud.id_Idioma = '" + idIdioma + "' AND "
        If Not UCase(proveedor) = "D" And Not UCase(proveedor) = "" Then
            cmdText += "mantenimiento.PAIS='" + Left(PAIS, 2).ToUpper() + "' AND "
        End If




        If Not String.IsNullOrEmpty(proveedor) And Not UCase(proveedor) = "D" Then
            If Not UCase(proveedor) = "TEL" And Not UCase(proveedor) = "REC" Then
                cmdText += "[Solicitudes].Proveedor = '" + proveedor + "' AND "
            Else
                If Not cod_subtipo = "008" And Not UCase(proveedor) = "REC" Then
                    cmdText += "[Solicitudes].Proveedor = '" + proveedor + "' AND "
                End If
            End If
        End If

        If Not String.IsNullOrEmpty(cod_contrato) Then ' And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            '09/08/2010
            'Kintell por tema busqueda por cups.
            ''cmdText += "[Solicitudes].[Cod_contrato] like '%" + cod_contrato + "%' AND "
            'cmdText += "[RCC].[Cod_contrato_SIC] = '" + cod_contrato + "' AND "
            '28/10/2015
            cmdText += "[RCC].[Cod_contrato_SIC] in (select COD_CONTRATO_SIC from RELACION_CUPS_CONTRATO where COD_RECEPTOR in (select COD_RECEPTOR from RELACION_CUPS_CONTRATO where COD_CONTRATO_SIC='" + cod_contrato + "')) AND "
            '**********
        ElseIf Not String.IsNullOrEmpty(cod_contrato) Then
            cmdText += "[Solicitudes].[Cod_contrato] = '" + cod_contrato + "' AND "
        End If

        If Not String.IsNullOrEmpty(id_solicitud) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[id_solicitud] = " + id_solicitud + " AND "
        End If

        If Not String.IsNullOrEmpty(Urgente) Then
            If Not UCase(Urgente) = UCase("false") Then cmdText += "[Solicitudes].[Urgente] ='" + (Urgente) + "' AND "
        End If
        'If consultaPendietes Then
        '    Dim solicitudCerradaCancelada As String() = {"009", "010", "011", "012", "013", "014", "015", "17", "18"}
        '    For Each estado As String In solicitudCerradaCancelada
        '        cmdText += "[Solicitudes].Estado_solicitud <> '" + estado + "' AND "
        '    Next
        'End If

        'GGB
        If consultaPendietes Then
            ' para subtipo de avisos averia
            'cmdText += "(([Solicitudes].[subtipo_solicitud] = '001' or [Solicitudes].[subtipo_solicitud] = '004') AND [Solicitudes].Estado_solicitud <> '010' AND [Solicitudes].Estado_solicitud <> '011' AND [Solicitudes].Estado_solicitud <> '012' OR "
            ' para el sutbtipo de solicitud de visita
            'cmdText += "([Solicitudes].[subtipo_solicitud] = '002' or [Solicitudes].[subtipo_solicitud] = '003') AND [Solicitudes].Estado_solicitud <> '012' AND [Solicitudes].Estado_solicitud <> '014') AND "
            ''cmdText += "([Solicitudes].Estado_solicitud < '008' OR [Solicitudes].Estado_solicitud > '012') AND "
            cmdText += "([Solicitudes].Estado_solicitud <> '010' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '011' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '012' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '014' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '021' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '018' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '022' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '023' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '025' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '026' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '029' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '036' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '037' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '043' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '047' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '032' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '049' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '050' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '051' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '052' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '053' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '054' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '055' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '056' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '020') AND "
        End If

        'Kintell
        '18/08/2010
        If Not cod_tipo = "-1" Then
            cmdText += "[Solicitudes].[Tipo_solicitud] = '" + cod_tipo + "' AND "
        End If
        'cmdText += "[Solicitudes].[Tipo_solicitud] = '001' AND "

        If Not cod_subtipo = "-1" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] = '" + cod_subtipo + "' AND "
        End If
        If UCase(proveedor) = "TEL" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] <> '009' AND "
        End If
        If UCase(proveedor) = "REC" Then
            'cmdText += "[Solicitudes].[subtipo_solicitud] = '009' AND "
        End If

        If Not cod_estado = "-1" Then
            cmdText += "[Solicitudes].[Estado_solicitud] = '" + cod_estado + "' AND "
        End If

        If Not String.IsNullOrEmpty(fechaDesde) Then
            cmdText += "[Solicitudes].[Fecha_creacion] >= '" + fechaDesde + "' AND "
        End If

        If Not String.IsNullOrEmpty(Provincia) Then
            'cmdText += "[provincia].[nombre] like '%" + Provincia + "%' AND "
            cmdText += "[provincia].[cod_provincia] = " + Provincia + " AND "
        End If
        If UCase(proveedor) = "TEL" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] <> '009' AND "
        End If
        If UCase(proveedor) = "REC" Then
            'cmdText += "[Solicitudes].[subtipo_solicitud] = '009' AND "
        End If
        'If Not String.IsNullOrEmpty(fechaHasta) Then
        '    cmdText += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + "' AND "
        'End If
        If Not String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + " 23:59:59.999' AND "
        End If

        'If Not caldera = "-1" Then
        '    cmdText += "[Calderas].[Marca_Caldera] = '" + caldera + "' AND "
        'End If

        If Not Perfil = "0" Then
            'cmdText += "id_perfil in (" & Perfil & ") AND "
        End If

        If Right(cmdText.Trim(), 3) = "AND" Then
            cmdText = cmdText.Remove(cmdText.Length - 4)
        End If

        If Right(cmdText.Trim(), 5) = "WHERE" Then
            cmdText = cmdText.Remove(cmdText.Length - 6)
        End If

        cmdText += "order by [Solicitudes].[Fecha_creacion] desc OPTION (FORCE ORDER)"

        myConnection.Open()
        Try

            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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
        Return myDataSet
    End Function
    Public Function GetSolicitudesPorProveedor(ByVal proveedor As String, ByVal cod_contrato As String, ByVal consultaPendietes As Boolean, ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal Provincia As String, ByVal Urgente As String, ByVal Desde As String, ByVal Hasta As String, ByVal Perfil As String, ByVal Id_Solicitud As String, Optional ByVal Nombre As String = "", Optional ByVal Apellido1 As String = "", Optional ByVal Apellido2 As String = "", Optional ByVal Colores As String = "0") As DataSet

        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty

        'cmdText += "select distinct ID_solicitud,Cod_contrato,Tipo_solicitud,Des_Tipo_solicitud,"
        'cmdText += "subtipo_solicitud,Des_subtipo_solicitud,"
        'cmdText += "Fecha_creacion,Fecha_cierre,Estado_solicitud,"
        'cmdText += "Des_Estado_solicitud,Subestado_solicitud,"
        'cmdText += "telefono_contacto,Persona_contacto,des_averia,cod_averia,Observaciones,Proveedor,"
        'cmdText += "Mot_cancel,urgencia,FEC_LIMITE_VISITA,"
        'cmdText += "Marca_Caldera,NOM_PROVINCIA,Urgente,"
        'cmdText += "NOM_POBLACION,NOM_CALLE,COD_PORTAL,"
        'cmdText += "TIP_PISO, TIP_MANO, COD_POSTAL "
        'cmdText += "from ("
        'cmdText += "select row_number() over(order by [Solicitudes].[Fecha_creacion] desc) as fila, "


        ''cmdText += "SELECT Distinct "
        'cmdText += "[Solicitudes].[ID_solicitud]"
        'cmdText += ",[Solicitudes].[Cod_contrato]"
        'cmdText += ",[Solicitudes].[Tipo_solicitud]"
        'cmdText += ",tipo_solicitud.descripcion as Des_Tipo_solicitud"
        'cmdText += ",[Solicitudes].[subtipo_solicitud]"
        'cmdText += ",subtipo_solicitud.descripcion as Des_subtipo_solicitud"
        'cmdText += ",[Solicitudes].[Fecha_creacion]"
        'cmdText += ",[Solicitudes].[Fecha_cierre]"
        'cmdText += ",[Solicitudes].[Estado_solicitud]"
        'cmdText += ",estado_solicitud.descripcion as Des_Estado_solicitud"
        'cmdText += ",[Solicitudes].[Subestado_solicitud]"
        'cmdText += ",[Solicitudes].[telefono_contacto]"
        'cmdText += ",[Solicitudes].[Persona_contacto]"
        'cmdText += ",tipo_averia.descripcion_averia as des_averia"
        'cmdText += ",[Solicitudes].cod_averia"
        'cmdText += ",[Solicitudes].[Observaciones]"
        'cmdText += ",[Solicitudes].[Proveedor]"
        'cmdText += ",[Solicitudes].[Mot_cancel]"
        'cmdText += ",mantenimiento.urgencia"
        'cmdText += ",mantenimiento.FEC_LIMITE_VISITA"
        ' ''cmdText += ",calderas.id_Marca_Caldera as Marca_Caldera"
        ''cmdText += ",calderas.Marca_Caldera as Marca_Caldera"
        'cmdText += ",TIPO_MARCA_CALDERA.desc_marca_caldera as Marca_Caldera"
        'cmdText += ",NOM_PROVINCIA"
        'cmdText += ",[Solicitudes].[Urgente]"
        'cmdText += ",[NOM_POBLACION]"
        'cmdText += ",[mantenimiento].[NOM_CALLE]"
        'cmdText += ",[mantenimiento].[COD_PORTAL]"
        'cmdText += ",[mantenimiento].[TIP_PISO]"
        'cmdText += ",[mantenimiento].[TIP_MANO]"
        'cmdText += ",[mantenimiento].[COD_POSTAL]"

        'cmdText += " FROM [dbo].[Solicitudes] "
        'cmdText += "inner join tipo_solicitud on tipo_solicitud.codigo = solicitudes.tipo_solicitud "
        'cmdText += "inner join subtipo_solicitud on subtipo_solicitud.codigo = solicitudes.subtipo_solicitud "
        'cmdText += "inner join estado_solicitud on estado_solicitud.codigo = solicitudes.estado_solicitud "
        'cmdText += "left join tipo_averia on tipo_averia.cod_averia = solicitudes.cod_averia "
        ''Cuidado con el left join en vez de inner join.
        'cmdText += "inner join mantenimiento on mantenimiento.COD_CONTRATO_SIC = solicitudes.Cod_contrato "
        'cmdText += "left join calderas on calderas.cod_contrato = solicitudes.Cod_contrato "

        ''***********************************************************************************************
        'cmdText += "inner join TIPO_MARCA_CALDERA on TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "
        'cmdText += "inner join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        'cmdText += "inner join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia) "
        ''***********************************************************************************************


        'Kintell 12/01/10
        '17/08/2010
        'Kitamos Des_subtipo_solicitud y Des_Estado_solicitud.
        'cmdText = "select ID_solicitud,Cod_contrato,Tipo_solicitud,Des_Tipo_solicitud,subtipo_solicitud,Des_subtipo_solicitud,Fecha_creacion,Fecha_cierre,Estado_solicitud,Des_Estado_solicitud,Subestado_solicitud,telefono_contacto,Persona_contacto,des_averia,cod_averia,Observaciones,Proveedor,Mot_cancel,urgencia,FEC_LIMITE_VISITA,Marca_Caldera,NOM_PROVINCIA,Urgente,NOM_POBLACION,NOM_CALLE,COD_PORTAL,TIP_PISO, TIP_MANO, COD_POSTAL "
        If (cod_subtipo = "006" And cod_estado = "026") Or (cod_subtipo = "005" And cod_estado = "014") Then
            cmdText = "select DISTINCT h.time_stamp as fec_visita, V1.ID_solicitud,Cod_contrato,Tipo_solicitud,Des_Tipo_solicitud,subtipo_solicitud,Fecha_creacion,Fecha_cierre,V1.Estado_solicitud,Subestado_solicitud,telefono_contacto,Persona_contacto,des_averia,cod_averia,V1.Observaciones,Proveedor,Mot_cancel,urgencia,FEC_LIMITE_VISITA,Marca_Caldera,NOM_PROVINCIA,Urgente,NOM_POBLACION,NOM_CALLE,COD_PORTAL,TIP_PISO, TIP_MANO, COD_POSTAL"
        Else
            cmdText = "select DISTINCT V1.ID_solicitud as fec_visita,V1.ID_solicitud,Cod_contrato,Tipo_solicitud,Des_Tipo_solicitud,subtipo_solicitud,Fecha_creacion,Fecha_cierre,V1.Estado_solicitud,Subestado_solicitud,telefono_contacto,Persona_contacto,des_averia,cod_averia,V1.Observaciones,Proveedor,Mot_cancel,urgencia,FEC_LIMITE_VISITA,Marca_Caldera,NOM_PROVINCIA,Urgente,NOM_POBLACION,NOM_CALLE,COD_PORTAL,TIP_PISO, TIP_MANO, COD_POSTAL"
        End If

        cmdText += ", CUPS "
        cmdText += ", COD_RECEPTOR "
        'cmdText += ", usuario "

        cmdText += ",v1.id_perfil "


        cmdText += " from ("
        cmdText += "select row_number() over(order by [Fecha_creacion] desc) as fila, "
        '17/08/2010
        'Kitamos Des_subtipo_solicitud y Des_Estado_solicitud.
        'cmdText += "[ID_solicitud], [cod_contrato], [Tipo_solicitud], Des_Tipo_solicitud, [subtipo_solicitud], Des_subtipo_solicitud, [Fecha_creacion], [Fecha_cierre], [Estado_solicitud], Des_Estado_solicitud, [Subestado_solicitud], [telefono_contacto], [Persona_contacto], des_averia, cod_averia, [Observaciones], [proveedor], [Mot_cancel], urgencia, FEC_LIMITE_VISITA, Marca_Caldera,  NOM_PROVINCIA, [Urgente],  [NOM_POBLACION], [NOM_CALLE], [COD_PORTAL], [TIP_PISO], [TIP_MANO], [COD_POSTAL]"
        cmdText += "v2.[ID_solicitud], v2.[cod_contrato], v2.[Tipo_solicitud], Des_Tipo_solicitud, [subtipo_solicitud], [Fecha_creacion], [Fecha_cierre], v2.[Estado_solicitud], [Subestado_solicitud], [telefono_contacto], [Persona_contacto], des_averia, cod_averia, v2.[Observaciones], [proveedor], [Mot_cancel], urgencia, v2.FEC_LIMITE_VISITA, Marca_Caldera,  NOM_PROVINCIA, [Urgente],  [NOM_POBLACION], [NOM_CALLE], [COD_PORTAL], [TIP_PISO], [TIP_MANO], [COD_POSTAL]"
        cmdText += ", [CUPS] "
        cmdText += ", COD_RECEPTOR "

        cmdText += ",u.id_perfil "

        'cmdText += ", usuario "

        cmdText += " from ("
        '17/08/2010
        'Kitamos Des_subtipo_solicitud y Des_Estado_solicitud.
        'cmdText += "select distinct [Solicitudes].[ID_solicitud],[Solicitudes].[Cod_contrato],[Solicitudes].[Tipo_solicitud],tipo_solicitud.descripcion as Des_Tipo_solicitud,[Solicitudes].[subtipo_solicitud],subtipo_solicitud.descripcion as Des_subtipo_solicitud,[Solicitudes].[Fecha_creacion],[Solicitudes].[Fecha_cierre],[Solicitudes].[Estado_solicitud],estado_solicitud.descripcion as Des_Estado_solicitud,[Solicitudes].[Subestado_solicitud],[Solicitudes].[telefono_contacto],[Solicitudes].[Persona_contacto],tipo_averia.descripcion_averia as des_averia,[Solicitudes].cod_averia,[Solicitudes].[Observaciones],[Solicitudes].[Proveedor],[Solicitudes].[Mot_cancel],mantenimiento.urgencia,mantenimiento.FEC_LIMITE_VISITA,TIPO_MARCA_CALDERA.desc_marca_caldera as Marca_Caldera,provincia.nombre as NOM_PROVINCIA,[Solicitudes].[Urgente],poblacion.nombre as NOM_POBLACION,[mantenimiento].[NOM_CALLE],[mantenimiento].[COD_PORTAL],[mantenimiento].[TIP_PISO],[mantenimiento].[TIP_MANO],[mantenimiento].[COD_POSTAL] "
        cmdText += "select distinct [Solicitudes].[ID_solicitud],[Solicitudes].[Cod_contrato],[Solicitudes].[Tipo_solicitud],tipo_solicitud.descripcion as Des_Tipo_solicitud,[Solicitudes].[subtipo_solicitud],[Solicitudes].[Fecha_creacion],[Solicitudes].[Fecha_cierre],[Solicitudes].[Estado_solicitud],[Solicitudes].[Subestado_solicitud],[Solicitudes].[telefono_contacto],[Solicitudes].[Persona_contacto],tipo_averia.descripcion_averia as des_averia,[Solicitudes].cod_averia,[Solicitudes].[Observaciones],[Solicitudes].[Proveedor],[Solicitudes].[Mot_cancel],mantenimiento.urgencia,mantenimiento.FEC_LIMITE_VISITA,TIPO_MARCA_CALDERA.desc_marca_caldera as Marca_Caldera,provincia.nombre as NOM_PROVINCIA,[Solicitudes].[Urgente],poblacion.nombre as NOM_POBLACION,[mantenimiento].[NOM_CALLE],[mantenimiento].[COD_PORTAL],[mantenimiento].[TIP_PISO],[mantenimiento].[TIP_MANO],[mantenimiento].[COD_POSTAL]"
        cmdText += ",[mantenimiento].[CUPS] "
        cmdText += ",[mantenimiento].[COD_RECEPTOR] "
        cmdText += ",[mantenimiento].[CONTADOR_AVERIAS] "

        cmdText += ",mantenimiento.Cod_contrato_sic,mantenimiento.cod_ultima_visita "
        'cmdText += ", h.usuario as 'usuario' "


        cmdText += " FROM [dbo].[Solicitudes]"


        'cmdText += "inner join historico h on h.id_solicitud=solicitudes.id_solicitud "

        '****************************************
        'cmdText += "inner JOIN HISTORICO H ON H.ID_SOLICITUD=Solicitudes.ID_SOLICITUD "
        'cmdText += "inner join usuarios u on U.USERID=H.USUARIO "
        '****************************************

        cmdText += "left join tipo_solicitud on tipo_solicitud.codigo = solicitudes.tipo_solicitud "
        cmdText += "left join subtipo_solicitud on subtipo_solicitud.codigo = solicitudes.subtipo_solicitud "
        cmdText += "left join estado_solicitud on estado_solicitud.codigo = solicitudes.estado_solicitud "
        cmdText += "left join tipo_averia on tipo_averia.cod_averia = solicitudes.cod_averia "
        cmdText += "left join mantenimiento on mantenimiento.COD_CONTRATO_SIC = solicitudes.Cod_contrato "
        '09/08/2010
        'Kintell tema busqueda por cups.
        'If Not String.IsNullOrEmpty(cod_contrato) And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
        cmdText += "left join relacion_cups_contrato RCC on rcc.COD_RECEPTOR=mantenimiento.COD_RECEPTOR "
        'End If
        '********


        'cmdText += "left join visita vi on vi.cod_contrato=mantenimiento.cod_contrato_sic and vi.cod_visita=mantenimiento.cod_ultima_visita "


        cmdText += "left join calderas on calderas.cod_contrato = solicitudes.Cod_contrato "
        '***********************************************************************************************
        cmdText += "left join TIPO_MARCA_CALDERA on TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "
        cmdText += "left join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        cmdText += "left join poblacion on poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia "
        '***********************************************************************************************

        If Not Perfil = "0" Then
            cmdText = "select * from "
            cmdText += "(select row_number() over(order by [Fecha_creacion] desc) as fila, v2.[ID_solicitud], v2.[cod_contrato], v2.[Tipo_solicitud], Des_Tipo_solicitud, [subtipo_solicitud],descripcion, [Fecha_creacion], [Fecha_cierre], v2.[Estado_solicitud], [Subestado_solicitud], [telefono_contacto], [Persona_contacto], des_averia, cod_averia, v2.[Observaciones], [proveedor], [Mot_cancel], urgencia, v2.FEC_LIMITE_VISITA, Marca_Caldera,  NOM_PROVINCIA, [Urgente],  [NOM_POBLACION], [NOM_CALLE], [COD_PORTAL], [TIP_PISO], [TIP_MANO], [COD_POSTAL], [CUPS],COD_RECEPTOR ,id_perfil,desSolicitud  "
            cmdText += "from (select distinct [Solicitudes].[ID_solicitud],[Solicitudes].[Cod_contrato],[Solicitudes].[Tipo_solicitud],tipo_solicitud.descripcion as Des_Tipo_solicitud,[Solicitudes].[subtipo_solicitud],[Solicitudes].[Fecha_creacion],[Solicitudes].[Fecha_cierre],[Solicitudes].[Estado_solicitud],[Solicitudes].[Subestado_solicitud],[Solicitudes].[telefono_contacto],[Solicitudes].[Persona_contacto],tipo_averia.descripcion_averia as des_averia,[Solicitudes].cod_averia,[Solicitudes].[Observaciones],[Solicitudes].[Proveedor],[Solicitudes].[Mot_cancel],mantenimiento.urgencia,mantenimiento.FEC_LIMITE_VISITA,TIPO_MARCA_CALDERA.desc_marca_caldera as Marca_Caldera,provincia.nombre as NOM_PROVINCIA,[Solicitudes].[Urgente],poblacion.nombre as NOM_POBLACION,[mantenimiento].[NOM_CALLE],[mantenimiento].[COD_PORTAL],[mantenimiento].[TIP_PISO],[mantenimiento].[TIP_MANO],[mantenimiento].[COD_POSTAL],[mantenimiento].[CUPS],MANTENIMIENTO.COD_RECEPTOR , [mantenimiento].[COD_RECEPTOR] ,mantenimiento.Cod_contrato_sic,mantenimiento.cod_ultima_visita  "
            cmdText += ",u.id_perfil,subtipo_solicitud.descripcion,[Estado_solicitud].Descripcion as desSolicitud "
            cmdText += "FROM [dbo].[Solicitudes] "
            cmdText += "inner join mantenimiento on mantenimiento.COD_CONTRATO_SIC = solicitudes.Cod_contrato "
            cmdText += "inner join relacion_cups_contrato RCC on rcc.COD_RECEPTOR=mantenimiento.COD_RECEPTOR "
            cmdText += "inner join visita vi on vi.cod_contrato=mantenimiento.cod_contrato_sic and vi.cod_visita=mantenimiento.cod_ultima_visita "
            cmdText += "left join tipo_solicitud on tipo_solicitud.codigo = solicitudes.tipo_solicitud "
            cmdText += "left join subtipo_solicitud on subtipo_solicitud.codigo = solicitudes.subtipo_solicitud "
            cmdText += "left join estado_solicitud on estado_solicitud.codigo = solicitudes.estado_solicitud "
            cmdText += "left join tipo_averia on tipo_averia.cod_averia = solicitudes.cod_averia "
            cmdText += "left join calderas on calderas.cod_contrato = solicitudes.Cod_contrato "
            cmdText += "left join TIPO_MARCA_CALDERA on TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "
            cmdText += "left join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
            cmdText += "left join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia"
            cmdText += ") "

            cmdText += "left join historico h on h.id_solicitud=solicitudes.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=solicitudes.id_solicitud) "
            cmdText += "left join usuarios u on u.UserID=h.usuario "
        End If



        cmdText += "WHERE "

        If Not String.IsNullOrEmpty(proveedor) And Not UCase(proveedor) = "D" And Not UCase(proveedor) = "ADICO" Then
            cmdText += "[Solicitudes].Proveedor = '" + proveedor + "' AND "
        End If
        'If Not Perfil = 0 Then
        '    cmdText += "U.Id_Perfil = " + Perfil + " AND "
        'End If

        If Not String.IsNullOrEmpty(cod_contrato) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            '09/08/2010
            'Kintell por tema busqueda por cups.
            ''cmdText += "[Solicitudes].[Cod_contrato] like '%" + cod_contrato + "%' AND "
            'cmdText += "[RCC].[Cod_contrato_SIC] = '" + cod_contrato + "' AND "
            '28/10/2015
            cmdText += "[RCC].[Cod_contrato_SIC] in (select COD_CONTRATO_SIC from RELACION_CUPS_CONTRATO where COD_RECEPTOR in (select COD_RECEPTOR from RELACION_CUPS_CONTRATO where COD_CONTRATO_SIC='" + cod_contrato + "')) AND "
            '**********
        ElseIf Not String.IsNullOrEmpty(cod_contrato) Then
            cmdText += "[Solicitudes].[Cod_contrato] = '" + cod_contrato + "' AND "
        End If

        If Not String.IsNullOrEmpty(Id_Solicitud) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[id_solicitud] = " + Id_Solicitud + " AND "
        End If

        If Not String.IsNullOrEmpty(Urgente) Then
            If Not UCase(Urgente) = UCase("false") Then cmdText += "[Solicitudes].[Urgente] ='" + (Urgente) + "' AND "
        End If

        If Not String.IsNullOrEmpty(Nombre) Then
            cmdText += "NOM_TITULAR like '%" + Nombre + "%' AND "
        End If
        If Not String.IsNullOrEmpty(Apellido1) Then
            cmdText += "APELLIDO1 like '%" + Apellido1 + "%' AND "
        End If
        If Not String.IsNullOrEmpty(Apellido2) Then
            cmdText += "APELLIDO2 like '%" + Apellido2 + "%' AND "
        End If
        'If consultaPendietes Then
        '    Dim solicitudCerradaCancelada As String() = {"009", "010", "011", "012", "013", "014", "015", "17", "18"}
        '    For Each estado As String In solicitudCerradaCancelada
        '        cmdText += "[Solicitudes].Estado_solicitud <> '" + estado + "' AND "
        '    Next
        'End If

        'GGB
        If consultaPendietes Then
            If Not UCase(proveedor) = "TEL" And Not UCase(proveedor) = "REC" Then
                ' para subtipo de avisos averia
                'cmdText += "(([Solicitudes].[subtipo_solicitud] = '001' or [Solicitudes].[subtipo_solicitud] = '004') AND [Solicitudes].Estado_solicitud <> '010' AND [Solicitudes].Estado_solicitud <> '011' AND [Solicitudes].Estado_solicitud <> '012' OR "
                ' para el sutbtipo de solicitud de visita
                'cmdText += "([Solicitudes].[subtipo_solicitud] = '002' or [Solicitudes].[subtipo_solicitud] = '003') AND [Solicitudes].Estado_solicitud <> '012' AND [Solicitudes].Estado_solicitud <> '014') AND "
                ''cmdText += "([Solicitudes].Estado_solicitud < '008' OR [Solicitudes].Estado_solicitud > '012') AND "
                cmdText += "([Solicitudes].Estado_solicitud <> '010' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '011' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '012' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '014' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '021' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '018' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '022' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '023' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '025' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '026' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '029' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '036' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '037' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '043' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '047' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '032' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '049' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '050' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '051' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '052' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '053' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '054' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '055' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '056' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '020') AND "
            Else
                cmdText += "[Solicitudes].[subtipo_solicitud] = '005' AND "
            End If
        End If

        'Kintell
        '18/08/2010
        If Not cod_tipo = "-1" Then
            cmdText += "[Solicitudes].[Tipo_solicitud] = '" + cod_tipo + "' AND "
        End If
        'cmdText += "[Solicitudes].[Tipo_solicitud] = '001' AND "

        If Not cod_subtipo = "-1" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] = '" + cod_subtipo + "' AND "
        End If

        If Not cod_estado = "-1" Then
            cmdText += "[Solicitudes].[Estado_solicitud] = '" + cod_estado + "' AND "
        End If



        If Not String.IsNullOrEmpty(Provincia) Then
            'cmdText += "[provincia].[nombre] like '%" + Provincia + "%' AND "
            cmdText += "[provincia].[cod_provincia] = " + Provincia + " AND "
        End If


        If Not String.IsNullOrEmpty(fechaDesde) Then
            cmdText += "[Solicitudes].[Fecha_creacion] >= '" + fechaDesde + "' AND "
        End If
        If Not String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + " 23:59:59.999' AND "
        End If

        'If Not caldera = "-1" Then
        '    cmdText += "[Calderas].[Marca_Caldera] = '" + caldera + "' AND "
        'End If

        If Right(cmdText.Trim(), 3) = "AND" Then
            cmdText = cmdText.Remove(cmdText.Length - 4)
        End If

        If Right(cmdText.Trim(), 5) = "WHERE" Then
            cmdText = cmdText.Remove(cmdText.Length - 6)
        End If





        'cmdText += ")"
        'cmdText += "as V "
        'cmdText += "WHERE(FILA >= " & Desde & " And FILA < " & Hasta & ") "
        'cmdText += "order by Fecha_creacion desc"

        'Kintell 12/01/10

        cmdText += ") as V2 "


        If Not Perfil = "0" And Not Perfil = "4" Then
            'cmdText += "where id_perfil in (" & Perfil & ")"
        ElseIf Not Perfil = "4" Then
            cmdText += "inner join visita vi on vi.cod_contrato=v2.cod_contrato_sic and vi.cod_visita=v2.cod_ultima_visita "
            cmdText += "left join historico h on h.id_solicitud=v2.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=v2.id_solicitud) "
            cmdText += "left join usuarios u on u.UserID=h.usuario "
        End If




        If (cod_subtipo = "006" And cod_estado = "026") Or (cod_subtipo = "005" And cod_estado = "014") Then
            If (cod_subtipo = "006" And cod_estado = "026") Then
                cmdText += ")as V1 left join historico h on h.id_solicitud=v1.id_solicitud and h.estado_solicitud='026' "
            Else
                cmdText += ")as V1 left join historico h on h.id_solicitud=v1.id_solicitud and h.estado_solicitud='014' "
            End If
        Else
            cmdText += ")as V1 "
        End If

        cmdText += "WHERE FILA >= " & Desde + 1 & " And FILA < COALESCE(" & Hasta + 1 & ", " & Hasta + 1 & ") order by Fecha_creacion desc" ' OPTION (HASH GROUP)"

        Try
            myConnection.Open()

            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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
        Return myDataSet

    End Function

    Public Function GetSolicitudesPorProveedorCalderas(ByVal proveedor As String, ByVal cod_contrato As String, ByVal consultaPendietes As Boolean, ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal Provincia As String, ByVal Urgente As String, ByVal Desde As String, ByVal Hasta As String, ByVal Perfil As String, ByVal Id_Solicitud As String, Optional ByVal Nombre As String = "", Optional ByVal Apellido1 As String = "", Optional ByVal Apellido2 As String = "", Optional ByVal Colores As String = "0", Optional ByVal idIdioma As String = "0", Optional ByVal PAIS As String = "0", Optional ByVal DNI As String = "", Optional ByVal CUI As String = "") As DataSet

        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty



        cmdText = "select *, 'd' as COD_PROVEEDOR  from "
        cmdText += "(select row_number() over(order by [Fecha_creacion] desc) as fila,  ID_solicitud,Cod_contrato,tipo_solicitud,subtipo_solicitud,estado_solicitud,Des_Tipo_solicitud,DESCRIPCION,desSolicitud,fecha_creacion,telefono_contacto,Persona_contacto,Des_averia,NOM_PROVINCIA,Observaciones,Urgente,Proveedor,EFV,SCORING,facturarInspeccion "
        cmdText += "from (select distinct [Solicitudes].[ID_solicitud],[Solicitudes].[Cod_contrato],[Solicitudes].[Tipo_solicitud],tipo_solicitud.descripcion as Des_Tipo_solicitud,[Solicitudes].[subtipo_solicitud],[Solicitudes].[Fecha_creacion],[Solicitudes].[Estado_solicitud],[Solicitudes].[Subestado_solicitud],[Solicitudes].[telefono_contacto],[Solicitudes].[Persona_contacto],tipo_averia.descripcion_averia as des_averia,[Solicitudes].[Observaciones],[Solicitudes].[Proveedor],provincia.nombre as NOM_PROVINCIA,[Solicitudes].[Urgente],subtipo_solicitud.descripcion,[Estado_solicitud].Descripcion as desSolicitud "
        cmdText += ",TEFV.DESCRIPCION_EFV as EFV "
        cmdText += ",MANTENIMIENTO.SCORING as SCORING "
        'cmdText += ",MANTENIMIENTO.BAJA_RENOVACION "
        cmdText += ", CASE WHEN subtipo_solicitud = '016' THEN case WHEN (esExtraordinaria = 1 and COD_COBERTURA='MGI') or INSPESAOGAS=1  THEN '1' else '0' end else '0' END AS facturarInspeccion "

        cmdText += "FROM [dbo].[Solicitudes] "
        cmdText += "inner join relacion_cups_contrato RCC on rcc.COD_CONTRATO_SIC=solicitudes.cod_contrato "
        cmdText += "inner join mantenimiento on mantenimiento.COD_RECEPTOR = RCC.COD_RECEPTOR "
        cmdText += "left join visita vi on vi.cod_contrato=rcc.cod_contrato_sic and vi.cod_visita=mantenimiento.cod_ultima_visita "
        cmdText += "left join tipo_solicitud on tipo_solicitud.codigo = solicitudes.tipo_solicitud "
        cmdText += "left join estado_solicitud on estado_solicitud.codigo = solicitudes.estado_solicitud "
        cmdText += "left join subtipo_solicitud on subtipo_solicitud.codigo = solicitudes.subtipo_solicitud and subtipo_solicitud.id_idioma=estado_solicitud.id_idioma "
        cmdText += "left join tipo_averia on tipo_averia.cod_averia = solicitudes.cod_averia and ididioma=estado_solicitud.id_idioma "
        cmdText += "left join calderas on calderas.cod_contrato = solicitudes.Cod_contrato "
        cmdText += "left join TIPO_MARCA_CALDERA on TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "

        cmdText += "left join TIPO_EFV AS TEFV ON mantenimiento.EFV = TEFV.COD_EFV "

        cmdText += "left join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        cmdText += "left join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia"
        cmdText += ") "
        cmdText += "left join historico h on h.id_solicitud=solicitudes.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=solicitudes.id_solicitud) "
        cmdText += "left join usuarios u on u.UserID=h.usuario "


        cmdText += "WHERE "

        '07/06/2016 IDIOMA
        If Not Perfil = "0" And Not Perfil = "4" Then
            cmdText += "estado_solicitud.id_Idioma = '" + idIdioma + "' AND mantenimiento.PAIS='" + Left(PAIS, 2).ToUpper() + "' AND "
        Else
            cmdText += "estado_solicitud.id_Idioma = '" + idIdioma + "' AND "
        End If



        If Not String.IsNullOrEmpty(proveedor) And Not UCase(proveedor) = "REC" And Not UCase(proveedor) = "TEL" And Not UCase(proveedor) = "D" And Not UCase(proveedor) = "ADICO" Then
            If UCase(proveedor) = "PMA" Then
                cmdText += "[Solicitudes].Proveedor in ('" + proveedor + "','ISQ') AND "
            Else
                cmdText += "[Solicitudes].Proveedor = '" + proveedor + "' AND "
            End If
        End If
        'If Not Perfil = 0 Then
        '    cmdText += "U.Id_Perfil = " + Perfil + " AND "
        'End If

        '20211129 MOD BEG R#34610 - Modificar el botón de telefono de la web para solicitudes modificadas por el telefono
        If Not Colores = "0" Then
            If Colores = "3" Then
                cmdText += "u.ID_Perfil in (" + Perfil + ",12) AND "
            Else
                cmdText += "u.ID_Perfil in (" + Perfil + ") AND "
            End If
        End If
        '20211129 MOD END R#34610 - Modificar el botón de telefono de la web para solicitudes modificadas por el telefono

        If Not String.IsNullOrEmpty(cod_contrato) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            '09/08/2010
            'Kintell por tema busqueda por cups.
            ''cmdText += "[Solicitudes].[Cod_contrato] like '%" + cod_contrato + "%' AND "
            'cmdText += "[RCC].[Cod_contrato_SIC] = '" + cod_contrato + "' AND "
            cmdText += "[RCC].[Cod_contrato_SIC] in (select COD_CONTRATO_SIC from RELACION_CUPS_CONTRATO where COD_RECEPTOR in (select COD_RECEPTOR from RELACION_CUPS_CONTRATO where COD_CONTRATO_SIC='" + cod_contrato + "')) AND "
            '**********
        ElseIf Not String.IsNullOrEmpty(cod_contrato) Then
            cmdText += "[Solicitudes].[Cod_contrato] = '" + cod_contrato + "' AND "
        End If

        If Not String.IsNullOrEmpty(Id_Solicitud) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[id_solicitud] = " + Id_Solicitud + " AND "
        End If

        If Not String.IsNullOrEmpty(Urgente) Then
            If Not UCase(Urgente) = UCase("false") Then cmdText += "[Solicitudes].[Urgente] ='" + (Urgente) + "' AND "
        End If

        If Not String.IsNullOrEmpty(Nombre) Then
            cmdText += "NOM_TITULAR like '%" + Nombre + "%' AND "
        End If
        If Not String.IsNullOrEmpty(Apellido1) Then
            cmdText += "APELLIDO1 like '%" + Apellido1 + "%' AND "
        End If
        If Not String.IsNullOrEmpty(Apellido2) Then
            cmdText += "APELLIDO2 like '%" + Apellido2 + "%' AND "
        End If
        If Not String.IsNullOrEmpty(DNI) Then
            cmdText += "DNI like '%" + DNI + "%' AND "
        End If
        If Not String.IsNullOrEmpty(CUI) Then
            'cmdText += "CUI like '%" + CUI + "%' AND "
            cmdText += " (CUI like '%" + CUI + "%' OR mantenimiento.CUPS LIKE '%" + CUI + "%') AND "
        End If
        'If consultaPendietes Then
        '    Dim solicitudCerradaCancelada As String() = {"009", "010", "011", "012", "013", "014", "015", "17", "18"}
        '    For Each estado As String In solicitudCerradaCancelada
        '        cmdText += "[Solicitudes].Estado_solicitud <> '" + estado + "' AND "
        '    Next
        'End If

        'GGB
        If consultaPendietes Then
            If Not UCase(proveedor) = "TEL" Then
                ' para subtipo de avisos averia
                'cmdText += "(([Solicitudes].[subtipo_solicitud] = '001' or [Solicitudes].[subtipo_solicitud] = '004') AND [Solicitudes].Estado_solicitud <> '010' AND [Solicitudes].Estado_solicitud <> '011' AND [Solicitudes].Estado_solicitud <> '012' OR "
                ' para el sutbtipo de solicitud de visita
                'cmdText += "([Solicitudes].[subtipo_solicitud] = '002' or [Solicitudes].[subtipo_solicitud] = '003') AND [Solicitudes].Estado_solicitud <> '012' AND [Solicitudes].Estado_solicitud <> '014') AND "
                ''cmdText += "([Solicitudes].Estado_solicitud < '008' OR [Solicitudes].Estado_solicitud > '012') AND "
                cmdText += "([Solicitudes].Estado_solicitud <> '010' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '011' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '012' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '014' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '021' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '018' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '022' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '023' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '025' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '026' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '029' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '036' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '037' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '043' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '047' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '032' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '024' "

                cmdText += "AND [Solicitudes].Estado_solicitud <> '049' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '050' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '051' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '052' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '053' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '054' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '055' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '056' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '057' "
                ' Inspecsao gas
                cmdText += "AND [Solicitudes].Estado_solicitud <> '072' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '073' "
                cmdText += "AND [Solicitudes].Estado_solicitud <> '074' "

                cmdText += "AND [Solicitudes].Estado_solicitud <> '020') AND "
            Else
                'cmdText += "[Solicitudes].[subtipo_solicitud] = '005' AND "
            End If
        End If

        'Kintell
        '18/08/2010
        If Not cod_tipo = "-1" Then
            cmdText += "[Solicitudes].[Tipo_solicitud] = '" + cod_tipo + "' AND "
        End If
        'cmdText += "[Solicitudes].[Tipo_solicitud] = '001' AND "
        If UCase(proveedor) = "TEL" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] <> '009' AND "
        End If
        If UCase(proveedor) = "REC" Then
            'cmdText += "[Solicitudes].[subtipo_solicitud] = '009' AND "
        End If
        If Not cod_subtipo = "-1" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] = '" + cod_subtipo + "' AND "
        End If


        If Not UCase(proveedor) = "INC" And Not String.IsNullOrEmpty(proveedor) And Not UCase(proveedor) = "REC" And Not UCase(proveedor) = "TEL" And Not UCase(proveedor) = "D" And Not UCase(proveedor) = "ADICO" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] <> '006' AND "
        End If


        If Not cod_estado = "-1" Then
            cmdText += "[Solicitudes].[Estado_solicitud] = '" + cod_estado + "' AND "
        End If



        If Not String.IsNullOrEmpty(Provincia) Then
            'cmdText += "[provincia].[nombre] like '%" + Provincia + "%' AND "
            cmdText += "[provincia].[cod_provincia] = " + Provincia + " AND "
        End If


        If Not String.IsNullOrEmpty(fechaDesde) Then
            cmdText += "[Solicitudes].[Fecha_creacion] >= '" + fechaDesde + "' AND "
        End If
        If Not String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + " 23:59:59.999' AND "
        End If

        'If Not caldera = "-1" Then
        '    cmdText += "[Calderas].[Marca_Caldera] = '" + caldera + "' AND "
        'End If

        If Right(cmdText.Trim(), 3) = "AND" Then
            cmdText = cmdText.Remove(cmdText.Length - 4)
        End If

        If Right(cmdText.Trim(), 5) = "WHERE" Then
            cmdText = cmdText.Remove(cmdText.Length - 6)
        End If





        'cmdText += ")"
        'cmdText += "as V "
        'cmdText += "WHERE(FILA >= " & Desde & " And FILA < " & Hasta & ") "
        'cmdText += "order by Fecha_creacion desc"

        'Kintell 12/01/10

        cmdText += ") as V2 "


        If Not Perfil = "0" And Not Perfil = "4" Then
            'cmdText += "where id_perfil in (" & Perfil & ")"
        ElseIf Not Perfil = "4" Then
            cmdText += "inner join visita vi on vi.cod_contrato=v2.cod_contrato_sic and vi.cod_visita=v2.cod_ultima_visita "
            cmdText += "left join historico h on h.id_solicitud=v2.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=v2.id_solicitud) "
            cmdText += "left join usuarios u on u.UserID=h.usuario "
        End If



        If (cod_subtipo = "006" And cod_estado = "026") Or (cod_subtipo = "005" And cod_estado = "014") Then
            If (cod_subtipo = "006" And cod_estado = "026") Then
                cmdText += ")as V1 left join historico h on h.id_solicitud=v1.id_solicitud and h.estado_solicitud='026' "
            Else
                cmdText += ")as V1 left join historico h on h.id_solicitud=v1.id_solicitud and h.estado_solicitud='014' "
            End If
        Else
            cmdText += ")as V1 "
        End If

        cmdText += "WHERE FILA >= " & Desde + 1 & " And FILA < COALESCE(" & Hasta + 1 & ", " & Hasta + 1 & ") order by Fecha_creacion desc" ' OPTION (HASH GROUP)"

        Try
            myConnection.Open()

            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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
        Return myDataSet

    End Function

    Public Function ObtenerPerfilPorSolicitud(ByVal Solicitud As Integer) As Integer
        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty

        cmdText = "Select top 1 U.ID_Perfil from usuarios U INNER JOIN HISTORICO H ON H.USUARIO=U.USERID "
        'inner join solicitudes s ON S.ID_SOLICITUD=H.ID_SOLICITUD 
        cmdText &= " where H.id_solicitud = " & Solicitud
        cmdText &= " ORDER BY H.TIME_STAMP DESC"


        'cmdText = "Select ID_Perfil from usuarios inner join solicitudes s where S.id_solicitud=" & Solicitud
        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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

        If myDataSet.Tables.Count > 0 Then
            If myDataSet.Tables(0).Rows().Count > 0 Then
                Return myDataSet.Tables(0).Rows(0).Item(0)
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function

    Public Function ObtenerDescripcionSubtipo(ByVal IdSubtipo As String) As String
        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty

        cmdText = "Select Descripcion from subtipo_solicitud where codigo='" & IdSubtipo & "'"


        'cmdText = "Select ID_Perfil from usuarios inner join solicitudes s where S.id_solicitud=" & Solicitud
        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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

        If myDataSet.Tables.Count > 0 Then
            If myDataSet.Tables(0).Rows().Count > 0 Then
                Return myDataSet.Tables(0).Rows(0).Item(0)
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function

    Public Function ObtenerDescripcionEstado(ByVal IdEstado As String) As String
        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty

        cmdText = "Select Descripcion from estado_solicitud where codigo='" & IdEstado & "'"


        'cmdText = "Select ID_Perfil from usuarios inner join solicitudes s where S.id_solicitud=" & Solicitud
        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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

        If myDataSet.Tables.Count > 0 Then
            If myDataSet.Tables(0).Rows().Count > 0 Then
                Return myDataSet.Tables(0).Rows(0).Item(0)
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function



    Public Function ObtenerRegistrosTotales(ByVal Proveedor As String, ByVal consultaPendietes As Boolean, ByVal cod_contrato As String, ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal Urgente As String, ByVal Provincia As String, ByVal Perfil As String, ByVal id_solicitud As String) As Integer
        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty
        cmdText += "select COUNT(*) "
        '17/08/2010
        'Kitamos Des_subtipo_solicitud y Des_Estado_solicitud.
        'cmdText += "[ID_solicitud], [cod_contrato], [Tipo_solicitud], Des_Tipo_solicitud, [subtipo_solicitud], Des_subtipo_solicitud, [Fecha_creacion], [Fecha_cierre], [Estado_solicitud], Des_Estado_solicitud, [Subestado_solicitud], [telefono_contacto], [Persona_contacto], des_averia, cod_averia, [Observaciones], [proveedor], [Mot_cancel], urgencia, FEC_LIMITE_VISITA, Marca_Caldera,  NOM_PROVINCIA, [Urgente],  [NOM_POBLACION], [NOM_CALLE], [COD_PORTAL], [TIP_PISO], [TIP_MANO], [COD_POSTAL]"

        cmdText += " from ("
        '17/08/2010
        'Kitamos Des_subtipo_solicitud y Des_Estado_solicitud.
        'cmdText += "select distinct [Solicitudes].[ID_solicitud],[Solicitudes].[Cod_contrato],[Solicitudes].[Tipo_solicitud],tipo_solicitud.descripcion as Des_Tipo_solicitud,[Solicitudes].[subtipo_solicitud],subtipo_solicitud.descripcion as Des_subtipo_solicitud,[Solicitudes].[Fecha_creacion],[Solicitudes].[Fecha_cierre],[Solicitudes].[Estado_solicitud],estado_solicitud.descripcion as Des_Estado_solicitud,[Solicitudes].[Subestado_solicitud],[Solicitudes].[telefono_contacto],[Solicitudes].[Persona_contacto],tipo_averia.descripcion_averia as des_averia,[Solicitudes].cod_averia,[Solicitudes].[Observaciones],[Solicitudes].[Proveedor],[Solicitudes].[Mot_cancel],mantenimiento.urgencia,mantenimiento.FEC_LIMITE_VISITA,TIPO_MARCA_CALDERA.desc_marca_caldera as Marca_Caldera,provincia.nombre as NOM_PROVINCIA,[Solicitudes].[Urgente],poblacion.nombre as NOM_POBLACION,[mantenimiento].[NOM_CALLE],[mantenimiento].[COD_PORTAL],[mantenimiento].[TIP_PISO],[mantenimiento].[TIP_MANO],[mantenimiento].[COD_POSTAL] "
        'cmdText += "select distinct [Solicitudes].[ID_solicitud],[Solicitudes].[Cod_contrato],[Solicitudes].[Tipo_solicitud],tipo_solicitud.descripcion as Des_Tipo_solicitud,[Solicitudes].[subtipo_solicitud],[Solicitudes].[Fecha_creacion],[Solicitudes].[Fecha_cierre],[Solicitudes].[Estado_solicitud],[Solicitudes].[Subestado_solicitud],[Solicitudes].[telefono_contacto],[Solicitudes].[Persona_contacto],tipo_averia.descripcion_averia as des_averia,[Solicitudes].cod_averia,[Solicitudes].[Observaciones],[Solicitudes].[Proveedor],[Solicitudes].[Mot_cancel],mantenimiento.urgencia,mantenimiento.FEC_LIMITE_VISITA,TIPO_MARCA_CALDERA.desc_marca_caldera as Marca_Caldera,provincia.nombre as NOM_PROVINCIA,[Solicitudes].[Urgente],poblacion.nombre as NOM_POBLACION,[mantenimiento].[NOM_CALLE],[mantenimiento].[COD_PORTAL],[mantenimiento].[TIP_PISO],[mantenimiento].[TIP_MANO],[mantenimiento].[COD_POSTAL]"
        cmdText += "select distinct [Solicitudes].[ID_solicitud],mantenimiento.cod_contrato_sic,mantenimiento.cod_ultima_visita"
        'cmdText += ",[mantenimiento].[CUPS] "

        'cmdText += ",mantenimiento.Cod_contrato_sic,mantenimiento.cod_ultima_visita "
        ''cmdText += ", h.usuario as 'usuario' "

        If Not Perfil = "0" Then
            cmdText += ",u.id_perfil "
        End If

        cmdText += " FROM [dbo].[Solicitudes]"


        'cmdText += "inner join historico h on h.id_solicitud=solicitudes.id_solicitud "

        '****************************************
        'cmdText += "inner JOIN HISTORICO H ON H.ID_SOLICITUD=Solicitudes.ID_SOLICITUD "
        'cmdText += "inner join usuarios u on U.USERID=H.USUARIO "
        '****************************************
        cmdText += "INNER join mantenimiento on mantenimiento.COD_CONTRATO_SIC = solicitudes.Cod_contrato "
        '09/08/2010
        'Kintell tema busqueda por cups.
        'If Not String.IsNullOrEmpty(cod_contrato) And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
        cmdText += "INNER join relacion_cups_contrato RCC on rcc.COD_RECEPTOR=mantenimiento.COD_RECEPTOR "
        'End If
        '********
        cmdText += "INNER join visita VI1 on VI1.cod_contrato=mantenimiento.cod_contrato_sic and VI1.cod_visita=mantenimiento.cod_ultima_visita "

        cmdText += "left join tipo_solicitud on tipo_solicitud.codigo = solicitudes.tipo_solicitud "
        cmdText += "left join subtipo_solicitud on subtipo_solicitud.codigo = solicitudes.subtipo_solicitud "
        cmdText += "left join estado_solicitud on estado_solicitud.codigo = solicitudes.estado_solicitud "
        cmdText += "left join tipo_averia on tipo_averia.cod_averia = solicitudes.cod_averia "



        cmdText += "left join calderas on calderas.cod_contrato = solicitudes.Cod_contrato "
        '***********************************************************************************************
        cmdText += "left join TIPO_MARCA_CALDERA on TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "
        cmdText += "left join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        cmdText += "left join poblacion on poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia "
        '***********************************************************************************************

        If Not Perfil = "0" Then
            'COLORES (perfil).
            cmdText += "left join historico h on h.id_solicitud=Solicitudes.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=Solicitudes.id_solicitud) "
            cmdText += "left join usuarios u on u.UserID=h.usuario "
        End If

        cmdText += "WHERE "

        If Not String.IsNullOrEmpty(Proveedor) And Not UCase(Proveedor) = "D" And Not UCase(Proveedor) = "ADICO" Then
            cmdText += "[Solicitudes].Proveedor = '" + Proveedor + "' AND "
        End If
        'If Not Perfil = 0 Then
        '    cmdText += "U.Id_Perfil = " + Perfil + " AND "
        'End If
        If Not String.IsNullOrEmpty(id_solicitud) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[id_solicitud] = " + id_solicitud + " AND "
        End If
        If Not String.IsNullOrEmpty(cod_contrato) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            '09/08/2010
            'Kintell por tema busqueda por cups.
            ''cmdText += "[Solicitudes].[Cod_contrato] like '%" + cod_contrato + "%' AND "
            'cmdText += "[RCC].[Cod_contrato_SIC] = '" + cod_contrato + "' AND "
            '28/10/2015
            cmdText += "[RCC].[Cod_contrato_SIC] in (select COD_CONTRATO_SIC from RELACION_CUPS_CONTRATO where COD_RECEPTOR in (select COD_RECEPTOR from RELACION_CUPS_CONTRATO where COD_CONTRATO_SIC='" + cod_contrato + "')) AND "
            '**********
        ElseIf Not String.IsNullOrEmpty(cod_contrato) Then
            cmdText += "[Solicitudes].[Cod_contrato] = '" + cod_contrato + "' AND "
        End If

        If Not String.IsNullOrEmpty(Urgente) Then
            If Not UCase(Urgente) = UCase("false") Then cmdText += "[Solicitudes].[Urgente] ='" + (Urgente) + "' AND "
        End If
        'If consultaPendietes Then
        '    Dim solicitudCerradaCancelada As String() = {"009", "010", "011", "012", "013", "014", "015", "17", "18"}
        '    For Each estado As String In solicitudCerradaCancelada
        '        cmdText += "[Solicitudes].Estado_solicitud <> '" + estado + "' AND "
        '    Next
        'End If

        'GGB
        If consultaPendietes Then
            '' para subtipo de avisos averia
            ''cmdText += "(([Solicitudes].[subtipo_solicitud] = '001' or [Solicitudes].[subtipo_solicitud] = '004') AND [Solicitudes].Estado_solicitud <> '010' AND [Solicitudes].Estado_solicitud <> '011' AND [Solicitudes].Estado_solicitud <> '012' OR "
            '' para el sutbtipo de solicitud de visita
            ''cmdText += "([Solicitudes].[subtipo_solicitud] = '002' or [Solicitudes].[subtipo_solicitud] = '003') AND [Solicitudes].Estado_solicitud <> '012' AND [Solicitudes].Estado_solicitud <> '014') AND "
            ' ''cmdText += "([Solicitudes].Estado_solicitud < '008' OR [Solicitudes].Estado_solicitud > '012') AND "
            'cmdText += "([Solicitudes].Estado_solicitud <> '010' "
            'cmdText += "AND [Solicitudes].Estado_solicitud <> '011' "
            'cmdText += "AND [Solicitudes].Estado_solicitud <> '012' "
            'cmdText += "AND [Solicitudes].Estado_solicitud <> '014' "
            'cmdText += "AND [Solicitudes].Estado_solicitud <> '021' "
            'cmdText += "AND [Solicitudes].Estado_solicitud <> '018' "
            'cmdText += "AND [Solicitudes].Estado_solicitud <> '022' "
            'cmdText += "AND [Solicitudes].Estado_solicitud <> '029' "
            'cmdText += "AND [Solicitudes].Estado_solicitud <> '023' "
            'cmdText += "AND [Solicitudes].Estado_solicitud <> '020') AND "

            cmdText += "([Solicitudes].Estado_solicitud <> '010' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '011' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '012' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '014' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '021' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '018' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '022' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '023' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '025' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '026' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '029' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '032' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '036' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '037' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '043' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '047' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '049' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '050' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '051' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '052' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '053' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '054' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '055' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '056' "
            cmdText += "AND [Solicitudes].Estado_solicitud <> '020') AND "

        End If

        'Kintell
        '18/08/2010
        If Not cod_tipo = "-1" Then
            cmdText += "[Solicitudes].[Tipo_solicitud] = '" + cod_tipo + "' AND "
        End If
        'cmdText += "[Solicitudes].[Tipo_solicitud] = '001' AND "

        If Not cod_subtipo = "-1" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] = '" + cod_subtipo + "' AND "
        End If

        If Not cod_estado = "-1" Then
            cmdText += "[Solicitudes].[Estado_solicitud] = '" + cod_estado + "' AND "
        End If



        If Not String.IsNullOrEmpty(Provincia) Then
            'cmdText += "[provincia].[nombre] like '%" + Provincia + "%' AND "
            cmdText += "[provincia].[cod_provincia] = " + Provincia + " AND "
        End If


        If Not String.IsNullOrEmpty(fechaDesde) Then
            cmdText += "[Solicitudes].[Fecha_creacion] >= '" + fechaDesde + "' AND "
        End If
        If Not String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + " 23:59:59.999' AND "
        End If

        'If Not caldera = "-1" Then
        '    cmdText += "[Calderas].[Marca_Caldera] = '" + caldera + "' AND "
        'End If

        If Right(cmdText.Trim(), 3) = "AND" Then
            cmdText = cmdText.Remove(cmdText.Length - 4)
        End If

        If Right(cmdText.Trim(), 5) = "WHERE" Then
            cmdText = cmdText.Remove(cmdText.Length - 6)
        End If





        'cmdText += ")"
        'cmdText += "as V "
        'cmdText += "WHERE(FILA >= " & Desde & " And FILA < " & Hasta & ") "
        'cmdText += "order by Fecha_creacion desc"

        'Kintell 12/01/10

        cmdText += ") as V2 "


        If Not Perfil = "0" Then
            'cmdText += "where id_perfil in (" & Perfil & ")"
        Else
            cmdText += "left join visita VI2 on VI2.cod_contrato = COALESCE (v2.cod_contrato_sic , v2.cod_contrato_sic) and VI2.cod_visita >= v2.cod_ultima_visita AND VI2.cod_visita <= v2.cod_ultima_visita " 'and VI2.cod_visita=v2.cod_ultima_visita "
            'cmdText += "left join historico h on h.id_solicitud=v2.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=v2.id_solicitud) "
            'cmdText += "left join usuarios u on u.UserID=h.usuario "
        End If

        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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
        If myDataSet.Tables.Count > 0 Then
            If myDataSet.Tables(0).Rows().Count > 0 Then
                Return myDataSet.Tables(0).Rows(0).Item(0)
            Else
                Return 0
            End If
        Else
            Return 0
        End If



        'cmdText="select count(1) "
        'cmdText += "from ("
        'cmdText += "select row_number() over(order by [Fecha_creacion] desc) as fila, "
        'cmdText += "[ID_solicitud], [cod_contrato], [Tipo_solicitud], Des_Tipo_solicitud, [subtipo_solicitud], Des_subtipo_solicitud, [Fecha_creacion], [Fecha_cierre], [Estado_solicitud], Des_Estado_solicitud, [Subestado_solicitud], [telefono_contacto], [Persona_contacto], des_averia, cod_averia, [Observaciones], [proveedor], [Mot_cancel], urgencia, FEC_LIMITE_VISITA, Marca_Caldera,  NOM_PROVINCIA, [Urgente],  [NOM_POBLACION], [NOM_CALLE], [COD_PORTAL], [TIP_PISO], [TIP_MANO], [COD_POSTAL]"

        ''cmdText += ", usuario "

        'cmdText += "from ("
        cmdText += "select distinct count(*) " '[Solicitudes].[ID_solicitud],[Solicitudes].[Cod_contrato],[Solicitudes].[Tipo_solicitud],tipo_solicitud.descripcion as Des_Tipo_solicitud,[Solicitudes].[subtipo_solicitud],subtipo_solicitud.descripcion as Des_subtipo_solicitud,[Solicitudes].[Fecha_creacion],[Solicitudes].[Fecha_cierre],[Solicitudes].[Estado_solicitud],estado_solicitud.descripcion as Des_Estado_solicitud,[Solicitudes].[Subestado_solicitud],[Solicitudes].[telefono_contacto],[Solicitudes].[Persona_contacto],tipo_averia.descripcion_averia as des_averia,[Solicitudes].cod_averia,[Solicitudes].[Observaciones],[Solicitudes].[Proveedor],[Solicitudes].[Mot_cancel],mantenimiento.urgencia,mantenimiento.FEC_LIMITE_VISITA,TIPO_MARCA_CALDERA.desc_marca_caldera as Marca_Caldera,provincia.nombre as NOM_PROVINCIA,[Solicitudes].[Urgente],poblacion.nombre as NOM_POBLACION,[mantenimiento].[NOM_CALLE],[mantenimiento].[COD_PORTAL],[mantenimiento].[TIP_PISO],[mantenimiento].[TIP_MANO],[mantenimiento].[COD_POSTAL] "

        'cmdText += ", h.usuario as 'usuario' "


        cmdText += " FROM [dbo].[Solicitudes]"

        'cmdText += "inner join historico h on h.id_solicitud=solicitudes.id_solicitud "

        '****************************************
        'cmdText += "inner JOIN HISTORICO H ON H.ID_SOLICITUD=Solicitudes.ID_SOLICITUD "
        'cmdText += "inner join usuarios u on U.USERID=H.USUARIO "
        '****************************************


        cmdText += "left join tipo_solicitud on tipo_solicitud.codigo = solicitudes.tipo_solicitud "
        cmdText += "left join subtipo_solicitud on subtipo_solicitud.codigo = solicitudes.subtipo_solicitud "
        cmdText += "left join estado_solicitud on estado_solicitud.codigo = solicitudes.estado_solicitud "
        cmdText += "left join tipo_averia on tipo_averia.cod_averia = solicitudes.cod_averia "
        cmdText += "left join mantenimiento on mantenimiento.COD_CONTRATO_SIC = solicitudes.Cod_contrato "
        '09/08/2010
        'Kintell tema busqueda por cups.
        'If Not String.IsNullOrEmpty(cod_contrato) And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
        cmdText += "left join relacion_cups_contrato RCC on rcc.COD_RECEPTOR=mantenimiento.COD_RECEPTOR "
        'End If
        '********
        cmdText += "left join calderas on calderas.cod_contrato = solicitudes.Cod_contrato "
        '***********************************************************************************************

        cmdText += "left join TIPO_MARCA_CALDERA on TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "
        cmdText += "left join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        cmdText += "left join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia) "
        '***********************************************************************************************

        If Not Perfil = "0" Then
            'COLORES (perfil).
            cmdText += "left join historico h on h.id_solicitud=Solicitudes.id_solicitud and h.time_stamp=(select max(time_stamp) from historico where id_solicitud=Solicitudes.id_solicitud) "
            cmdText += "left join usuarios u on u.UserID=h.usuario "
        End If


        cmdText += "WHERE "

        If Not String.IsNullOrEmpty(Proveedor) And Not UCase(Proveedor) = "ADICO" Then
            cmdText += "[Solicitudes].Proveedor = '" + Proveedor + "' AND "
        End If

        'If Not Perfil = 0 Then
        '    cmdText += "U.Id_Perfil = " + Perfil + " AND "
        'End If

        If Not String.IsNullOrEmpty(cod_contrato) Then 'And cod_tipo = "-1" And cod_subtipo = "-1" And cod_estado = "-1" And String.IsNullOrEmpty(Provincia) And String.IsNullOrEmpty(fechaDesde) And String.IsNullOrEmpty(fechaHasta) Then
            '09/08/2010
            'Kintell por tema de CUPS.
            ''cmdText += "[Solicitudes].[Cod_contrato] like '%" + cod_contrato + "%' AND "
            'cmdText += "[RCC].[Cod_contrato_sic] = '" + cod_contrato + "' AND "
            '28/10/2015
            cmdText += "[RCC].[Cod_contrato_SIC] in (select COD_CONTRATO_SIC from RELACION_CUPS_CONTRATO where COD_RECEPTOR in (select COD_RECEPTOR from RELACION_CUPS_CONTRATO where COD_CONTRATO_SIC='" + cod_contrato + "')) AND "
            '********
            'ElseIf Not String.IsNullOrEmpty(cod_contrato)
            'cmdText += "[Solicitudes].[Cod_contrato] like '%" + cod_contrato + "%' AND "
        End If

        If Not String.IsNullOrEmpty(Urgente) Then
            If Not UCase(Urgente) = UCase("false") Then cmdText += "[Solicitudes].[Urgente] ='" + (Urgente) + "' AND "
        End If
        'If consultaPendietes Then
        '    Dim solicitudCerradaCancelada As String() = {"009", "010", "011", "012", "013", "014", "015", "17", "18"}
        '    For Each estado As String In solicitudCerradaCancelada
        '        cmdText += "[Solicitudes].Estado_solicitud <> '" + estado + "' AND "
        '    Next
        'End If

        'GGB
        'If consultaPendietes Then
        '    ' para subtipo de avisos averia
        '    cmdText += "(([Solicitudes].[subtipo_solicitud] = '001' AND ([Solicitudes].Estado_solicitud <> '010' AND [Solicitudes].Estado_solicitud <> '011' AND [Solicitudes].Estado_solicitud <> '012')) OR "
        '    ' para el sutbtipo de solicitud de visita
        '    cmdText += "([Solicitudes].[subtipo_solicitud] = '002' AND ([Solicitudes].Estado_solicitud <> '012' AND [Solicitudes].Estado_solicitud <> '014'))) AND "
        '    'cmdText += "([Solicitudes].Estado_solicitud < '008' OR [Solicitudes].Estado_solicitud > '012') AND "
        'End If

        If consultaPendietes Then
            ' para subtipo de avisos averia
            cmdText += "((([Solicitudes].[subtipo_solicitud] = '001' or [Solicitudes].[subtipo_solicitud] = '004') AND ([Solicitudes].Estado_solicitud <> '010' AND [Solicitudes].Estado_solicitud <> '011' AND [Solicitudes].Estado_solicitud <> '012')) OR "
            ' para el sutbtipo de solicitud de visita
            cmdText += "(([Solicitudes].[subtipo_solicitud] = '002' or [Solicitudes].[subtipo_solicitud] = '003') AND ([Solicitudes].Estado_solicitud <> '012' AND [Solicitudes].Estado_solicitud <> '014'))) AND "
            'cmdText += "([Solicitudes].Estado_solicitud < '008' OR [Solicitudes].Estado_solicitud > '012') AND "
        End If

        'Kintell
        '18/08/2010
        If Not cod_tipo = "-1" Then
            cmdText += "[Solicitudes].[Tipo_solicitud] = '" + cod_tipo + "' AND "
        End If
        'cmdText += "[Solicitudes].[Tipo_solicitud] = '001' AND "

        If Not cod_subtipo = "-1" Then
            cmdText += "[Solicitudes].[subtipo_solicitud] = '" + cod_subtipo + "' AND "
        End If

        If Not cod_estado = "-1" Then
            cmdText += "[Solicitudes].[Estado_solicitud] = '" + cod_estado + "' AND "
        End If



        If Not String.IsNullOrEmpty(Provincia) Then
            'cmdText += "[provincia].[nombre] like '%" + Provincia + "%' AND "
            cmdText += "[provincia].[cod_provincia] = " + Provincia + " AND "
        End If


        If Not String.IsNullOrEmpty(fechaDesde) Then
            cmdText += "[Solicitudes].[Fecha_creacion] >= '" + fechaDesde + "' AND "
        End If
        If Not String.IsNullOrEmpty(fechaHasta) Then
            cmdText += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + " 23:59:59.999' AND "
        End If

        'If Not caldera = "-1" Then
        '    cmdText += "[Calderas].[Marca_Caldera] = '" + caldera + "' AND "
        'End If

        If Not Perfil = "0" Then
            cmdText += " id_perfil in (" & Perfil & ") AND "
        End If


        If Right(cmdText.Trim(), 3) = "AND" Then
            cmdText = cmdText.Remove(cmdText.Length - 4)
        End If

        If Right(cmdText.Trim(), 5) = "WHERE" Then
            cmdText = cmdText.Remove(cmdText.Length - 6)
        End If





        ''cmdText += ")"
        ''cmdText += "as V "
        ''cmdText += "WHERE(FILA >= " & Desde & " And FILA < " & Hasta & ") "
        ''cmdText += "order by Fecha_creacion desc"


        'Kintell 10/08/10
        'cmdText += ") as V"
        'cmdText += ")as V1 "

        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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
        If myDataSet.Tables.Count > 0 Then
            If myDataSet.Tables(0).Rows().Count > 0 Then
                Return myDataSet.Tables(0).Rows(0).Item(0)
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function
    Public Function ObtenerRegistrosTotales_OLD(ByVal Proveedor As String, ByVal consultaPendietes As Boolean, ByVal cod_contrato As String, ByVal cod_tipo As String, ByVal cod_subtipo As String, ByVal cod_estado As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal Urgente As String, ByVal Provincia As String) As Integer
        Dim con As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))

        Dim selectSQL2 As String = "SELECT COUNT(*) AS TOTAL "
        selectSQL2 &= "FROM Solicitudes INNER JOIN Mantenimiento ON Mantenimiento.COD_CONTRATO_SIC = Solicitudes.Cod_contrato "
        '***********************************************************************************************
        selectSQL2 &= "inner join TIPO_MARCA_CALDERA on TIPO_MARCA_CALDERA.ID_MARCA_CALDERA = calderas.id_marca_caldera "
        selectSQL2 &= "inner join provincia on provincia.cod_provincia = mantenimiento.Cod_provincia "
        selectSQL2 &= "inner join poblacion on (poblacion.cod_poblacion = mantenimiento.Cod_poblacion and poblacion.cod_provincia = mantenimiento.Cod_provincia) "
        '***********************************************************************************************
        selectSQL2 &= "where "

        If Not Proveedor = "ADICO" Then
            selectSQL2 &= "Solicitudes.Proveedor='" & Proveedor & "' and  "
        End If

        If consultaPendietes Then
            ' para subtipo de avisos averia
            selectSQL2 &= "(([Solicitudes].[subtipo_solicitud] = '001' AND ([Solicitudes].Estado_solicitud <> '010' AND [Solicitudes].Estado_solicitud <> '011' AND [Solicitudes].Estado_solicitud <> '012')) OR "
            ' para el sutbtipo de solicitud de visita
            selectSQL2 &= "([Solicitudes].[subtipo_solicitud] = '002' AND ([Solicitudes].Estado_solicitud <> '012' AND [Solicitudes].Estado_solicitud <> '014'))) AND  "
            'cmdText += "([Solicitudes].Estado_solicitud < '008' OR [Solicitudes].Estado_solicitud > '012') AND "
        End If


        If Not String.IsNullOrEmpty(cod_contrato) Then
            selectSQL2 += "[Solicitudes].[Cod_contrato] like '%" + cod_contrato + "%' AND  "
        End If

        If Not String.IsNullOrEmpty(Urgente) Then
            If Not UCase(Urgente) = UCase("false") Then selectSQL2 += "[Solicitudes].[Urgente] ='" + (Urgente) + "' AND  "
        End If


        If Not cod_tipo = "-1" Then
            selectSQL2 += "[Solicitudes].[Tipo_solicitud] = '" + cod_tipo + "' AND  "
        End If

        If Not cod_subtipo = "-1" Then
            selectSQL2 += "[Solicitudes].[subtipo_solicitud] = '" + cod_subtipo + "' AND  "
        End If

        If Not cod_estado = "-1" Then
            selectSQL2 += "[Solicitudes].[Estado_solicitud] = '" + cod_estado + "' AND  "
        End If

        If Not String.IsNullOrEmpty(Provincia) Then
            'selectSQL2 += "[provincia].[nombre] like '%" + Provincia + "%' AND  "
            selectSQL2 += "[provincia].[cod_provincia] = " + Provincia + " AND "
        End If
        If Not String.IsNullOrEmpty(fechaDesde) Then
            selectSQL2 += "[Solicitudes].[Fecha_creacion] >= '" + fechaDesde + "' AND  "
        End If
        If Not String.IsNullOrEmpty(fechaHasta) Then
            selectSQL2 += "[Solicitudes].[Fecha_creacion] <= '" + fechaHasta + " 23:59:59.999' AND  "
        End If


        selectSQL2 = Mid(selectSQL2, 1, Len(selectSQL2) - 6)

        Dim cmd2 As New SqlCommand(selectSQL2, con)
        con.Open()
        Dim Reader As SqlDataReader
        Reader = cmd2.ExecuteReader()

        Reader.Read()
        Try
            Dim total As Integer = 0
            total = CInt(Reader("TOTAL").ToString())

            Return total

        Catch err As Exception
        Finally
            Reader.Close()
            con.Close()
        End Try

    End Function

    Public Function GetSingleSolicitudes(ByVal id_solicitud As String, ByVal idIdioma As Integer) As SqlDataReader

        ' Create Instance of Connection and Command Object
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_GetSingleSolicitudes", myConnection)

        ' Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterIdSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIdSolicitud.Value = Integer.Parse(id_solicitud)
        myCommand.Parameters.Add(parameterIdSolicitud)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = Integer.Parse(idIdioma)
        myCommand.Parameters.Add(parameterIdIdioma)

        ' Execute the command
        myConnection.Open()
        Dim result As SqlDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

        ' Return the datareader 
        Return result
    End Function

    Public Function GetDatosLiquidacionPorSolicitud(ByVal id_solicitud As String) As SqlDataReader

        ' Create Instance of Connection and Command Object
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("SP_GET_DATOS_LIQUIDACION_SOLICITUD", myConnection)

        ' Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterIdSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIdSolicitud.Value = Integer.Parse(id_solicitud)
        myCommand.Parameters.Add(parameterIdSolicitud)

        ' Execute the command
        myConnection.Open()
        Dim result As SqlDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

        ' Return the datareader 
        Return result
    End Function

    Public Function GetDatosLiquidacionPorVisita(ByVal cod_Contrato As String, ByVal cod_Visita As String) As SqlDataReader

        ' Create Instance of Connection and Command Object
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("SP_GET_DATOS_LIQUIDACION_VISITA", myConnection)

        ' Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterCodContrato As New SqlParameter("@cod_contrato", SqlDbType.NVarChar, 10)
        parameterCodContrato.Value = Integer.Parse(cod_Contrato)
        myCommand.Parameters.Add(parameterCodContrato)

        ' Add Parameters to SPROC
        Dim parameterCodVisita As New SqlParameter("@cod_Visita", SqlDbType.Int, 4)
        parameterCodVisita.Value = Integer.Parse(cod_Visita)
        myCommand.Parameters.Add(parameterCodVisita)

        ' Execute the command
        myConnection.Open()
        Dim result As SqlDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

        ' Return the datareader 
        Return result
    End Function

    Public Function AddSolicitud(ByVal cod_contrato As String, ByVal cod_tipo_solicitud As String, ByVal cod_subtipo_solicitud As String, ByVal cod_estado As String, ByVal telef_contacto As String, ByVal pers_contacto As String, ByVal cod_averia As String, ByVal observaciones As String, ByVal proveedor As String, ByVal Urgente As Boolean, ByVal Retencion As Boolean) As Integer
        Return AddSolicitud(cod_contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_averia, observaciones, proveedor, Urgente, Retencion, False)
    End Function
    Public Function AddSolicitud(ByVal cod_contrato As String, ByVal cod_tipo_solicitud As String, ByVal cod_subtipo_solicitud As String, ByVal cod_estado As String, ByVal telef_contacto As String, ByVal pers_contacto As String, ByVal cod_averia As String, ByVal observaciones As String, ByVal proveedor As String, ByVal Urgente As Boolean, ByVal Retencion As Boolean, ByVal esExtraordinaria As Boolean) As Integer

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_AddSolicitudes", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodContrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 10)
        parameterCodContrato.Value = cod_contrato
        myCommand.Parameters.Add(parameterCodContrato)

        Dim parameterTipoSolicitud As New SqlParameter("@Tipo_solicitud", SqlDbType.NVarChar, 3)
        parameterTipoSolicitud.Value = cod_tipo_solicitud
        myCommand.Parameters.Add(parameterTipoSolicitud)

        Dim parameterSubtipoSolicitud As New SqlParameter("@subtipo_solicitud", SqlDbType.NVarChar, 3)
        parameterSubtipoSolicitud.Value = cod_subtipo_solicitud
        myCommand.Parameters.Add(parameterSubtipoSolicitud)

        Dim parameterFechaCreacion As New SqlParameter("@Fecha_creacion", SqlDbType.DateTime)
        parameterFechaCreacion.Value = DateTime.Now
        myCommand.Parameters.Add(parameterFechaCreacion)

        Dim parameterEstadoSolicitud As New SqlParameter("@Estado_solicitud", SqlDbType.NVarChar, 3)
        parameterEstadoSolicitud.Value = cod_estado
        myCommand.Parameters.Add(parameterEstadoSolicitud)

        Dim parameterTelefono As New SqlParameter("@telefono_contacto", SqlDbType.NVarChar, 50)
        parameterTelefono.Value = telef_contacto
        myCommand.Parameters.Add(parameterTelefono)

        Dim parameterPersonaContacto As New SqlParameter("@Persona_contacto", SqlDbType.NVarChar, 50)
        parameterPersonaContacto.Value = pers_contacto
        myCommand.Parameters.Add(parameterPersonaContacto)

        Dim parameterCodAveria As New SqlParameter("@Cod_averia", SqlDbType.NVarChar, 50)
        parameterCodAveria.Value = cod_averia
        myCommand.Parameters.Add(parameterCodAveria)

        Dim parameterObservaciones As New SqlParameter("@Observaciones", SqlDbType.NVarChar, 255)
        parameterObservaciones.Value = observaciones
        myCommand.Parameters.Add(parameterObservaciones)

        Dim parameterProveedor As New SqlParameter("@Proveedor", SqlDbType.NChar, 3)
        parameterProveedor.Value = proveedor
        myCommand.Parameters.Add(parameterProveedor)

        Dim parameterUrgente As New SqlParameter("@Urgente", SqlDbType.Bit)
        parameterUrgente.Value = Urgente
        myCommand.Parameters.Add(parameterUrgente)


        Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Direction = ParameterDirection.Output
        myCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterRetencion As New SqlParameter("@Retencion", SqlDbType.Bit)
        parameterRetencion.Value = Retencion
        myCommand.Parameters.Add(parameterRetencion)

        Dim parameterExtraordinaria As New SqlParameter("@esExtraordinaria", SqlDbType.Bit)
        parameterExtraordinaria.Value = esExtraordinaria
        myCommand.Parameters.Add(parameterExtraordinaria)

        myConnection.Open()
        Try
            myCommand.CommandTimeout = 0
            myCommand.ExecuteNonQuery()
            myConnection.Close()
        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try

        Return CInt(parameterIDSolicitud.Value)

    End Function

    Public Sub AddSolicitudCortesia(ByVal cod_contrato As String, ByVal cod_tipo_solicitud As String, ByVal cod_subtipo_solicitud As String, ByVal telef_contacto As String)
        '10 Cod_contrato]     SI 
        '3 [Tipo_solicitud](SI) 
        '3 [subtipo_solicitud](SI) 
        '[Fecha_creacion] NO ponemos la fecha en la que ejecuten el programa de carga
        '[Fecha_cierre]  NO en blanco como cuando crean una en manual
        '3 [Estado_solicitud] me da igual, si prefieres que lo informen que lo informen, si no ponemos en automatico el primer estado posible para esa solicitud..
        '11 [telefono_contacto](SI)
        '3 (TEL) [Proveedor] 
        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_AddSolicitudesCortesia", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodContrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 10)
        parameterCodContrato.Value = cod_contrato
        myCommand.Parameters.Add(parameterCodContrato)

        Dim parameterTipoSolicitud As New SqlParameter("@Tipo_solicitud", SqlDbType.NVarChar, 3)
        parameterTipoSolicitud.Value = cod_tipo_solicitud
        myCommand.Parameters.Add(parameterTipoSolicitud)

        Dim parameterSubtipoSolicitud As New SqlParameter("@subtipo_solicitud", SqlDbType.NVarChar, 3)
        parameterSubtipoSolicitud.Value = cod_subtipo_solicitud
        myCommand.Parameters.Add(parameterSubtipoSolicitud)

        Dim parameterTelefono As New SqlParameter("@telefono_contacto", SqlDbType.NVarChar, 50)
        parameterTelefono.Value = telef_contacto
        myCommand.Parameters.Add(parameterTelefono)

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

    Public Function AddSolicitudAveriaGasConfort(ByVal cod_contrato As String, subtipo As String) As Integer
        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_AddSolicitudAveriaGasConfort", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodContrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 10)
        parameterCodContrato.Value = cod_contrato
        myCommand.Parameters.Add(parameterCodContrato)

        Dim parameterSubtipo As New SqlParameter("@Subtipo", SqlDbType.NVarChar, 3)
        parameterSubtipo.Value = subtipo
        myCommand.Parameters.Add(parameterSubtipo)

        Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Direction = ParameterDirection.Output
        myCommand.Parameters.Add(parameterIDSolicitud)

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

        Return CInt(parameterIDSolicitud.Value)
    End Function


    Public Sub UpdateSolicitudDesdeProveedor(ByVal id_solicitud As String, ByVal cod_mot_cancel As String)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_UpdateSolicitudesDesdeProveedor", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.Parameters.Add(parameterIDSolicitud)



        Dim parameterMotivoCancelacion As New SqlParameter("@Mot_cancel", SqlDbType.NVarChar, 3)
        If Not cod_mot_cancel = "-1" Then
            parameterMotivoCancelacion.Value = cod_mot_cancel
        Else
            parameterMotivoCancelacion.Value = System.DBNull.Value
        End If
        myCommand.Parameters.Add(parameterMotivoCancelacion)


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

    Public Sub UpdateSolicitud(ByVal id_solicitud As String, ByVal cod_contrato As String, ByVal cod_tipo_solicitud As String, ByVal cod_subtipo_solicitud As String, ByVal cod_estado As String, ByVal telef_contacto As String, ByVal pers_contacto As String, ByVal cod_averia As String, ByVal observaciones As String, ByVal proveedor As String, ByVal cod_mot_cancel As String, ByVal Urgente As String, ByVal Retencion As String)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_UpdateSolicitudes", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterCodContrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 10)
        parameterCodContrato.Value = cod_contrato
        myCommand.Parameters.Add(parameterCodContrato)

        Dim parameterTipoSolicitud As New SqlParameter("@Tipo_solicitud", SqlDbType.NVarChar, 3)
        If Not cod_tipo_solicitud = "-1" Then
            parameterTipoSolicitud.Value = cod_tipo_solicitud
        Else
            parameterTipoSolicitud.Value = System.DBNull.Value
        End If
        myCommand.Parameters.Add(parameterTipoSolicitud)

        Dim parameterSubtipoSolicitud As New SqlParameter("@subtipo_solicitud", SqlDbType.NVarChar, 3)
        If Not cod_subtipo_solicitud = "-1" Then
            parameterSubtipoSolicitud.Value = cod_subtipo_solicitud
        Else
            parameterSubtipoSolicitud.Value = System.DBNull.Value
        End If
        myCommand.Parameters.Add(parameterSubtipoSolicitud)

        Dim parameterFechaCreacion As New SqlParameter("@Fecha_creacion", SqlDbType.DateTime)
        parameterFechaCreacion.Value = DateTime.Now
        myCommand.Parameters.Add(parameterFechaCreacion)

        Dim parameterEstadoSolicitud As New SqlParameter("@Estado_solicitud", SqlDbType.NVarChar, 3)
        If Not cod_estado = "-1" Then
            parameterEstadoSolicitud.Value = cod_estado
        Else
            parameterEstadoSolicitud.Value = System.DBNull.Value
        End If
        myCommand.Parameters.Add(parameterEstadoSolicitud)

        Dim parameterTelefono As New SqlParameter("@telefono_contacto", SqlDbType.NVarChar, 50)
        parameterTelefono.Value = telef_contacto
        myCommand.Parameters.Add(parameterTelefono)

        Dim parameterPersonaContacto As New SqlParameter("@Persona_contacto", SqlDbType.NVarChar, 50)
        parameterPersonaContacto.Value = pers_contacto
        myCommand.Parameters.Add(parameterPersonaContacto)

        Dim parameterCodAveria As New SqlParameter("@Cod_averia", SqlDbType.NVarChar, 3)
        If Not cod_averia = "-1" Then
            parameterCodAveria.Value = cod_averia
        Else
            parameterCodAveria.Value = System.DBNull.Value
        End If
        myCommand.Parameters.Add(parameterCodAveria)

        Dim parameterObservaciones As New SqlParameter("@Observaciones", SqlDbType.NVarChar, 4000)
        parameterObservaciones.Value = observaciones
        myCommand.Parameters.Add(parameterObservaciones)

        Dim parameterProveedor As New SqlParameter("@Proveedor", SqlDbType.NChar, 3)
        If proveedor = "" Then
            parameterProveedor.Value = "D"
        Else
            parameterProveedor.Value = proveedor
        End If
        myCommand.Parameters.Add(parameterProveedor)

        Dim parameterMotivoCancelacion As New SqlParameter("@Mot_cancel", SqlDbType.NVarChar, 3)
        If Not cod_mot_cancel = "-1" And Not cod_mot_cancel = "" Then
            parameterMotivoCancelacion.Value = cod_mot_cancel
        Else
            parameterMotivoCancelacion.Value = System.DBNull.Value
        End If
        myCommand.Parameters.Add(parameterMotivoCancelacion)


        'Kintell 14/04/2009
        Dim parameterUrgente As New SqlParameter("@Urgente", SqlDbType.Bit)
        'If Not UCase(Urgente) = UCase("false") Then
        parameterUrgente.Value = Boolean.Parse(Urgente)
        'Else
        '    parameterUrgente.Value = System.DBNull.Value
        'End If
        myCommand.Parameters.Add(parameterUrgente)
        'If Not String.IsNullOrEmpty(Urgente) Then
        '    If Not UCase(Urgente) = UCase("false") Then cmdText += "[Solicitudes].[Urgente] ='" + (Urgente) + "' AND "
        'End If

        '17/09/2015 Comentado porke en el procedimiento no lo espera.
        'Dim parameterRetencion As New SqlParameter("@Retencion", SqlDbType.Bit)

        'parameterUrgente.Value = Boolean.Parse(Retencion)

        'myCommand.Parameters.Add(parameterRetencion)


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


    Public Sub UpdateEstadoSolicitud(ByVal id_solicitud As String, ByVal cod_estado As String, ByVal nTipLugarAveria As Integer)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_UpdateEstadoSolicitudes", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure

        Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterEstadoSolicitud As New SqlParameter("@Estado_solicitud", SqlDbType.NVarChar, 3)
        parameterEstadoSolicitud.Value = cod_estado
        myCommand.Parameters.Add(parameterEstadoSolicitud)

        'Dim parameterNumFactura As New SqlParameter("@NumFactura", SqlDbType.NVarChar, 250)
        'parameterNumFactura.Value = numFactura
        'myCommand.Parameters.Add(parameterNumFactura)

        Dim parameterTipLugarAveria As New SqlParameter("@TIP_LUGAR_AVERIA", SqlDbType.Int, 3)
        parameterTipLugarAveria.Value = nTipLugarAveria
        myCommand.Parameters.Add(parameterTipLugarAveria)

        myConnection.Open()
        Try
            myCommand.ExecuteNonQuery()
            myConnection.Close()
        Catch ex As Exception
            Dim s As String = ""
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try

    End Sub

    Public Sub InsertarSolicitudModificada(ByVal idSolicitud As String, ByVal tipoOperacion As Integer, ByVal Usuario As String, ByVal estadoAnterior As String)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_InsertarHistoricoCambiosEnSolicitud", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure

        Dim parameterIdSolicitud As New SqlParameter("@id_Solicitud", SqlDbType.NVarChar, 25)
        parameterIdSolicitud.Value = idSolicitud
        myCommand.Parameters.Add(parameterIdSolicitud)

        Dim parameterTipoOperacion As New SqlParameter("@tipoOperacion", SqlDbType.Int, 4)
        parameterTipoOperacion.Value = tipoOperacion
        myCommand.Parameters.Add(parameterTipoOperacion)

        Dim parameterUsuario As New SqlParameter("@Usuario", SqlDbType.NVarChar, 50)
        parameterUsuario.Value = Usuario
        myCommand.Parameters.Add(parameterUsuario)

        Dim parameterEstadoAnterior As New SqlParameter("@estadoAnterior", SqlDbType.NVarChar, 50)
        parameterEstadoAnterior.Value = estadoAnterior
        myCommand.Parameters.Add(parameterEstadoAnterior)


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

    Public Sub UpdateObservacionesSolicitud(ByVal id_solicitud As String, ByVal cod_estado As String)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_UpdateObservacionesSolicitudes", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterEstadoSolicitud As New SqlParameter("@Observaciones_solicitud", SqlDbType.NVarChar, 4000)
        parameterEstadoSolicitud.Value = cod_estado
        myCommand.Parameters.Add(parameterEstadoSolicitud)



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

    Public Sub InsertarCalderaContrato(ByVal id_contrato As String, ByVal Marca As Integer, ByVal Modelo As String)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_AddCaldera", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodigoContrato As New SqlParameter("@id_contrato", SqlDbType.NVarChar, 50)
        parameterCodigoContrato.Value = id_contrato
        myCommand.Parameters.Add(parameterCodigoContrato)

        Dim parameterMarcaCaldera As New SqlParameter("@Marca", SqlDbType.Int, 4)
        parameterMarcaCaldera.Value = Marca
        myCommand.Parameters.Add(parameterMarcaCaldera)

        Dim parameterModeloCaldera As New SqlParameter("@Modelo", SqlDbType.NVarChar, 50)
        parameterModeloCaldera.Value = Modelo
        myCommand.Parameters.Add(parameterModeloCaldera)



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

    Public Sub ActualizarCalderaContrato(ByVal id_contrato As String, ByVal Marca As Integer, ByVal Modelo As String, ByVal TipoCaldera As String, ByVal UsoCaldera As String, ByVal PotenciaCaldera As Integer, ByVal AñoCaldera As Integer)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_UpdateCaldera", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodigoContrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 50)
        parameterCodigoContrato.Value = id_contrato
        myCommand.Parameters.Add(parameterCodigoContrato)

        Dim parameterMarcaCaldera As New SqlParameter("@Marca_Caldera", SqlDbType.Int, 4)
        parameterMarcaCaldera.Value = Marca
        myCommand.Parameters.Add(parameterMarcaCaldera)

        Dim parameterModeloCaldera As New SqlParameter("@Modelo_Caldera", SqlDbType.NVarChar, 50)
        parameterModeloCaldera.Value = Modelo
        myCommand.Parameters.Add(parameterModeloCaldera)


        Dim parameterTipoCaldera As New SqlParameter("@Tipo_Caldera", SqlDbType.Int, 50)
        If TipoCaldera = "" Then TipoCaldera = 1
        parameterTipoCaldera.Value = TipoCaldera
        myCommand.Parameters.Add(parameterTipoCaldera)

        Dim parameterUsoCaldera As New SqlParameter("@Uso", SqlDbType.Int, 50)
        If UsoCaldera = "" Then UsoCaldera = 0
        parameterUsoCaldera.Value = UsoCaldera
        myCommand.Parameters.Add(parameterUsoCaldera)

        Dim parameterPotenciaCaldera As New SqlParameter("@Potencia", SqlDbType.Float)
        parameterPotenciaCaldera.Value = PotenciaCaldera
        myCommand.Parameters.Add(parameterPotenciaCaldera)

        Dim parameterAñoCaldera As New SqlParameter("@ANIo", SqlDbType.Int, 4)
        parameterAñoCaldera.Value = AñoCaldera
        myCommand.Parameters.Add(parameterAñoCaldera)


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

    Public Sub ActualizarNumeroAveriasPorcontrato(ByVal id_contrato As String)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_UpdateContadorAveriaPorContrato", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodigoContrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 50)
        parameterCodigoContrato.Value = id_contrato
        myCommand.Parameters.Add(parameterCodigoContrato)

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

    Public Sub ActualizarCalderaMarcaModeloByContrato(ByVal id_contrato As String, ByVal Marca As String, ByVal Modelo As String)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_UpdateMarcaModeloCaldera", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodigoContrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 50)
        parameterCodigoContrato.Value = id_contrato
        myCommand.Parameters.Add(parameterCodigoContrato)

        Dim parameterMarcaCaldera As New SqlParameter("@Marca_Caldera", SqlDbType.NVarChar, 50)
        parameterMarcaCaldera.Value = Marca
        myCommand.Parameters.Add(parameterMarcaCaldera)

        Dim parameterModeloCaldera As New SqlParameter("@Modelo_Caldera", SqlDbType.NVarChar, 50)
        parameterModeloCaldera.Value = Modelo
        myCommand.Parameters.Add(parameterModeloCaldera)


       

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

    Public Function ObtenerAvisoSolicitud(ByVal idSolicitud As String) As String
        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty
        cmdText = "select aviso_solicitud from solicitudes where id_solicitud='" & idSolicitud & "'"

        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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
        If myDataSet.Tables.Count > 0 Then
            If myDataSet.Tables(0).Rows().Count > 0 Then
                If Not IsDBNull(myDataSet.Tables(0).Rows(0).Item(0)) Then
                    Return myDataSet.Tables(0).Rows(0).Item(0)
                End If

            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function

    Public Function AddSolicitudReclamacion(ByVal cod_contrato As String, ByVal observaciones As String, ByVal usuario As String, ByVal Proveedor As String) As Integer

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("SP_INSERTAR_SOLICITUD_RECLAMACION", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodContrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 10)
        parameterCodContrato.Value = cod_contrato
        myCommand.Parameters.Add(parameterCodContrato)

        Dim parameterObservaciones As New SqlParameter("@Observaciones", SqlDbType.NVarChar, 500)
        parameterObservaciones.Value = observaciones
        myCommand.Parameters.Add(parameterObservaciones)

        Dim parameterProveedorID As New SqlParameter("@ProveedorID", SqlDbType.NVarChar, 3)
        parameterProveedorID.Value = Proveedor
        myCommand.Parameters.Add(parameterProveedorID)

        Dim parameterProveedor As New SqlParameter("@USUARIO", SqlDbType.NVarChar, 10)
        parameterProveedor.Value = usuario
        myCommand.Parameters.Add(parameterProveedor)

        myConnection.Open()
        Try
            myCommand.CommandTimeout = 0
            myCommand.ExecuteNonQuery()
            myConnection.Close()
        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try
        Return 0
    End Function

    Public Function ObtenerObservacionesAnterioresPorIdSolicitud(ByVal Solicitud As Integer) As String
        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty

        cmdText = "Select Observaciones from solicitudes "
        'inner join solicitudes s ON S.ID_SOLICITUD=H.ID_SOLICITUD 
        cmdText &= " where id_solicitud = " & Solicitud


        'cmdText = "Select ID_Perfil from usuarios inner join solicitudes s where S.id_solicitud=" & Solicitud
        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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

        If myDataSet.Tables.Count > 0 Then
            If myDataSet.Tables(0).Rows().Count > 0 Then
                Return myDataSet.Tables(0).Rows(0).Item(0).ToString()
            Else
                Return ""
            End If
        Else
            Return ""
        End If

    End Function

    Public Function ObtenerSubtipoPorIdSolicitud(ByVal Solicitud As Integer) As String
        Dim myDataSet As New DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim adapter As New SqlDataAdapter()
        Dim cmdText As String = String.Empty

        cmdText = "Select subtipo_solicitud from solicitudes "
        'inner join solicitudes s ON S.ID_SOLICITUD=H.ID_SOLICITUD 
        cmdText &= " where id_solicitud = " & Solicitud


        'cmdText = "Select ID_Perfil from usuarios inner join solicitudes s where S.id_solicitud=" & Solicitud
        myConnection.Open()
        Try
            Dim sqlC As SqlCommand = New SqlCommand(cmdText, myConnection)

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

        If myDataSet.Tables.Count > 0 Then
            If myDataSet.Tables(0).Rows().Count > 0 Then
                Return myDataSet.Tables(0).Rows(0).Item(0).ToString()
            Else
                Return ""
            End If
        Else
            Return ""
        End If

    End Function

    Public Function EvolutionStateSolicitudReclamacion(ByVal idSolicitud As String, ByVal observaciones As String, ByVal usuario As String, ByVal esProveedor As Boolean, ByVal estado As String, ByVal Proveedor As String) As Integer

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("SP_EVOLUCION_SOLICITUD_RECLAMACION", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterCodContrato As New SqlParameter("@ID_SOLICITUD", SqlDbType.Int)
        parameterCodContrato.Value = idSolicitud
        myCommand.Parameters.Add(parameterCodContrato)

        Dim parameterObservaciones As New SqlParameter("@Observaciones", SqlDbType.NVarChar, 4000)
        parameterObservaciones.Value = observaciones
        myCommand.Parameters.Add(parameterObservaciones)

        Dim parameterUsuario As New SqlParameter("@USUARIO", SqlDbType.NVarChar, 10)
        parameterUsuario.Value = usuario
        myCommand.Parameters.Add(parameterUsuario)

        Dim parameterProveedorID As New SqlParameter("@ProveedorID", SqlDbType.NVarChar, 3)
        parameterProveedorID.Value = Proveedor
        myCommand.Parameters.Add(parameterProveedorID)

        Dim parameterEsProveedor As New SqlParameter("@PROVEEDOR", SqlDbType.Bit)
        parameterEsProveedor.Value = esProveedor
        myCommand.Parameters.Add(parameterEsProveedor)

        Dim parameterEstado As New SqlParameter("@Estado", SqlDbType.NVarChar, 3)
        parameterEstado.Value = estado
        myCommand.Parameters.Add(parameterEstado)

        myConnection.Open()
        Try
            myCommand.CommandTimeout = 0
            myCommand.ExecuteNonQuery()
            myConnection.Close()
        Catch ex As Exception
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try
        Return 0
    End Function

    Public Function UpdateProveedorSolicitud(ByVal id_solicitud As String, ByVal proveedor As String)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("SP_UPDATE_PROVEEDOR_POR_SOLICITUD", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = id_solicitud
        myCommand.Parameters.Add(parameterIDSolicitud)

        Dim parameterEstadoSolicitud As New SqlParameter("@Proveedor", SqlDbType.NVarChar, 3)
        parameterEstadoSolicitud.Value = proveedor
        myCommand.Parameters.Add(parameterEstadoSolicitud)



        myConnection.Open()
        Try
            myCommand.ExecuteNonQuery()
            myConnection.Close()
        Catch ex As Exception
            Dim a As String
            a = ex.Message

        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try
        Return 0
    End Function

    Public Function GetPreciosCalderasExcel(ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("SP_GET_EXCEL_COSTE_E_INSTALACION_CALDERAS", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure
        myCommand.SelectCommand.CommandTimeout = 0

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet
    End Function

    Public Function GetCoberturaServicio(ByVal codEFV As String, idIdioma As Integer) As DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("SP_GET_COBERTURA_SERVICIO", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterCodEFV As New SqlParameter("@CODEFV", SqlDbType.NVarChar, 10)
        parameterCodEFV.Value = codEFV
        myCommand.SelectCommand.Parameters.Add(parameterCodEFV)

        Dim parameterIdioma As New SqlParameter("@ID_IDIOMA", SqlDbType.NVarChar, 10)
        parameterIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GuardarCaracteristicaWS(ByVal tipCar As String, ByVal valCar As String, ByVal idSolicitud As Integer)

        Dim myConnection As New SqlConnection(ConfigurationSettings.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("spAddCaracteristicaByWS", myConnection)
        myCommand.CommandType = CommandType.StoredProcedure


        Dim parameterTipCar As New SqlParameter("@tipCar", SqlDbType.NVarChar, 50)
        parameterTipCar.Value = tipCar
        myCommand.Parameters.Add(parameterTipCar)

        Dim parameterValCar As New SqlParameter("@valCar", SqlDbType.NVarChar, 50)
        parameterValCar.Value = valCar
        myCommand.Parameters.Add(parameterValCar)

        Dim parameterIDSolicitud As New SqlParameter("@ID_solicitud", SqlDbType.Int, 4)
        parameterIDSolicitud.Value = idSolicitud
        myCommand.Parameters.Add(parameterIDSolicitud)

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
        Return 0
    End Function

End Class
