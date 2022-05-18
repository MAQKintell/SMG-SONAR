Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class ArgumentarioDB

    Public Function GetPadres() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetArgumentarioPadres", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetHijosPorPadre(ByVal IdPadre As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetArgumentarioHijosPorPadre", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterIDPadre As New SqlParameter("@Cod_Padre", SqlDbType.NVarChar, 10)
        parameterIDPadre.Value = IdPadre
        myCommand.SelectCommand.Parameters.Add(parameterIDPadre)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetRespuestas() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetArgumentarioRespuestas", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function InsertResultados(ByVal CodContrato As String, ByVal Valor As String, ByVal Usuario As String, ByVal OBSERVACIONES As String, ByVal VisitaRealizada As Boolean) As String
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))

        Try

            Dim SqlString As String
            Dim Fecha As String
            Fecha = Now.Month & "/" & Now.Day & "/" & Now.Year & " " & Now.Hour & ":" & Now.Minute & ":" & Now.Second

            Dim Visita As String = "0"
            If VisitaRealizada Then Visita = "1"
            SqlString = "INSERT INTO Resultados_Argumentario (COD_CONTRATO,ID_PREGUNTA,FECHA,USUARIO,OBSERVACIONES,VISITA) values('" & CodContrato & "','" & Valor & "','" & Fecha & "','" & Usuario & "','" & OBSERVACIONES & "','" & Visita & "')"

            myConnection.Open()
            Dim myCommand2 As New SqlCommand(SqlString, myConnection)
            myCommand2.ExecuteNonQuery()

            myConnection.Close()

            Return "1"
        Catch ex As Exception
            Return "0"
        Finally
            If (myConnection.State = ConnectionState.Open) Then
                myConnection.Close()
            End If
        End Try

    End Function

    Public Function ObtenerResultados(ByVal CodContrato As String, ByVal Todos As Boolean, ByVal FechaDesde As String, ByVal FechaHasta As String) As DataSet
        Try

            Dim SqlString As String
            'SqlString = "SELECT Resultados_Argumentario.COD_CONTRATO, Solicitudes.telefono_contacto, Resultados_Argumentario.FECHA, Preguntas_Argumentario.TEXTO_PREGUNTA FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) INNER JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato where Resultados_Argumentario.COD_CONTRATO='" & CodContrato & "'"
            'SqlString = "SELECT Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA, Resultados_Argumentario.FECHA, Preguntas_Argumentario.TEXTO_PREGUNTA, Resultados_Argumentario.ID_PREGUNTA, Preguntas_Argumentario.EPIGRAFE FROM Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA WHERE Preguntas_Argumentario.FECHABAJA='01/01/1900' AND Resultados_Argumentario.COD_CONTRATO='" & CodContrato & "' ORDER BY Resultados_Argumentario.FECHA, Resultados_Argumentario.ID_PREGUNTA"
            If Not Todos Then
                If FechaDesde = "" Then
                    SqlString = "SELECT Solicitudes.telefono_contacto, Resultados_Argumentario.ID_PREGUNTA, Resultados_Argumentario.FECHA, Preguntas_Argumentario.TEXTO_PREGUNTA, Resultados_Argumentario.ID_PREGUNTA, Preguntas_Argumentario.EPIGRAFE, Resultados_Argumentario.COD_CONTRATO as CONTRATO FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) LEFT JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato WHERE Resultados_Argumentario.COD_CONTRATO='" & CodContrato & "' AND Preguntas_Argumentario.FECHABAJA='01/01/1900' ORDER BY Resultados_Argumentario.FECHA,Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA"
                Else
                    SqlString = "SELECT Solicitudes.telefono_contacto, Resultados_Argumentario.ID_PREGUNTA, Resultados_Argumentario.FECHA, Preguntas_Argumentario.TEXTO_PREGUNTA, Resultados_Argumentario.ID_PREGUNTA, Preguntas_Argumentario.EPIGRAFE, Resultados_Argumentario.COD_CONTRATO as CONTRATO FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) LEFT JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato WHERE Resultados_Argumentario.COD_CONTRATO='" & CodContrato & "' AND Preguntas_Argumentario.FECHABAJA='01/01/1900' and and (fecha >= '" & FechaDesde & "' and fecha <= '" & FechaHasta & "') ORDER BY Resultados_Argumentario.FECHA,Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA"
                End If

            Else
                If FechaDesde = "" Then
                    SqlString = "SELECT Solicitudes.telefono_contacto, Resultados_Argumentario.ID_PREGUNTA, Resultados_Argumentario.FECHA, Preguntas_Argumentario.TEXTO_PREGUNTA, Resultados_Argumentario.ID_PREGUNTA, Preguntas_Argumentario.EPIGRAFE, Resultados_Argumentario.COD_CONTRATO as CONTRATO FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) LEFT JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato WHERE Preguntas_Argumentario.FECHABAJA='01/01/1900' ORDER BY Resultados_Argumentario.FECHA,Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA"
                Else
                    SqlString = "SELECT Solicitudes.telefono_contacto, Resultados_Argumentario.ID_PREGUNTA, Resultados_Argumentario.FECHA, Preguntas_Argumentario.TEXTO_PREGUNTA, Resultados_Argumentario.ID_PREGUNTA, Preguntas_Argumentario.EPIGRAFE, Resultados_Argumentario.COD_CONTRATO as CONTRATO FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) LEFT JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato WHERE Preguntas_Argumentario.FECHABAJA='01/01/1900' and (fecha >= '" & FechaDesde & "' and fecha <= '" & FechaHasta & "') ORDER BY Resultados_Argumentario.FECHA,Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA"
                End If

            End If
            Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
            Dim myCommand As New SqlDataAdapter(SqlString, myConnection)
            Dim myDataSet As New DataSet()
            myCommand.Fill(myDataSet)

            Return myDataSet
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Public Function ObtenerResultadosParaExcell(ByVal CodContrato As String, ByVal Todos As Boolean, ByVal FechaDesde As String, ByVal FechaHasta As String) As DataSet
        Try

            Dim SqlString As String
            'SqlString = "SELECT Resultados_Argumentario.COD_CONTRATO, Solicitudes.telefono_contacto, Resultados_Argumentario.FECHA, Preguntas_Argumentario.TEXTO_PREGUNTA FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) INNER JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato where Resultados_Argumentario.COD_CONTRATO='" & CodContrato & "'"
            'SqlString = "SELECT Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA, Resultados_Argumentario.FECHA, Preguntas_Argumentario.TEXTO_PREGUNTA, Resultados_Argumentario.ID_PREGUNTA, Preguntas_Argumentario.EPIGRAFE FROM Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA WHERE Preguntas_Argumentario.FECHABAJA='01/01/1900' AND Resultados_Argumentario.COD_CONTRATO='" & CodContrato & "' ORDER BY Resultados_Argumentario.FECHA, Resultados_Argumentario.ID_PREGUNTA"

            If Not Todos Then
                If FechaDesde = "" Then
                    SqlString = "SELECT Solicitudes.telefono_contacto as TELEFONO, Preguntas_Argumentario.EPIGRAFE,Preguntas_Argumentario.TEXTO_PREGUNTA as ELECCION, Resultados_Argumentario.FECHA as 'FECHA DEL PROCESO', Resultados_Argumentario.COD_CONTRATO as CONTRATO FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) LEFT JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato WHERE Resultados_Argumentario.COD_CONTRATO='" & CodContrato & "' AND Preguntas_Argumentario.FECHABAJA='01/01/1900' ORDER BY Resultados_Argumentario.FECHA,Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA"
                Else
                    SqlString = "SELECT Solicitudes.telefono_contacto as TELEFONO, Preguntas_Argumentario.EPIGRAFE,Preguntas_Argumentario.TEXTO_PREGUNTA as ELECCION, Resultados_Argumentario.FECHA as 'FECHA DEL PROCESO', Resultados_Argumentario.COD_CONTRATO as CONTRATO FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) LEFT JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato WHERE Resultados_Argumentario.COD_CONTRATO='" & CodContrato & "' AND Preguntas_Argumentario.FECHABAJA='01/01/1900' and (fecha >= '" & FechaDesde & "' and fecha <= '" & FechaHasta & "') ORDER BY Resultados_Argumentario.FECHA,Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA"
                End If
            Else
                If FechaDesde = "" Then
                    SqlString = "SELECT Solicitudes.telefono_contacto as TELEFONO, Preguntas_Argumentario.EPIGRAFE,Preguntas_Argumentario.TEXTO_PREGUNTA as ELECCION, Resultados_Argumentario.FECHA as 'FECHA DEL PROCESO', Resultados_Argumentario.COD_CONTRATO as CONTRATO FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) LEFT JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato WHERE Preguntas_Argumentario.FECHABAJA='01/01/1900' ORDER BY Resultados_Argumentario.FECHA,Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA"
                Else
                    SqlString = "SELECT Solicitudes.telefono_contacto as TELEFONO, Preguntas_Argumentario.EPIGRAFE,Preguntas_Argumentario.TEXTO_PREGUNTA as ELECCION, Resultados_Argumentario.FECHA as 'FECHA DEL PROCESO', Resultados_Argumentario.COD_CONTRATO as CONTRATO FROM (Preguntas_Argumentario INNER JOIN Resultados_Argumentario ON Preguntas_Argumentario.ID_PREGUNTA = Resultados_Argumentario.ID_PREGUNTA) LEFT JOIN Solicitudes ON Resultados_Argumentario.COD_CONTRATO = Solicitudes.Cod_contrato WHERE Preguntas_Argumentario.FECHABAJA='01/01/1900' and (fecha >= '" & FechaDesde & "' and fecha <= '" & FechaHasta & "') ORDER BY Resultados_Argumentario.FECHA,Resultados_Argumentario.COD_CONTRATO, Resultados_Argumentario.ID_PREGUNTA"

                End If
            End If
            Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
            Dim myCommand As New SqlDataAdapter(SqlString, myConnection)
            Dim myDataSet As New DataSet()
            myCommand.Fill(myDataSet)

            Return myDataSet
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

End Class
