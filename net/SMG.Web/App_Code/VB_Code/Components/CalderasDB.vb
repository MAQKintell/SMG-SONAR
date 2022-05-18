Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class CalderasDB



    Public Function GetMarcaCalderas() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetMarca_calderas", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

    Public Function GetCalderasPorContrato(ByVal cod_contrato As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetCalderasPorContrato", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterCod_contrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 10)
        parameterCod_contrato.Value = cod_contrato
        myCommand.SelectCommand.Parameters.Add(parameterCod_contrato)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function

End Class
