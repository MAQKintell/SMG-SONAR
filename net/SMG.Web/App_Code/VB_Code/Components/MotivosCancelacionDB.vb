Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class MotivosCancelacionDB


    Public Function GetMotivosCancelacion(ByVal Subtipo As String, ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetMotivo_cancelaciones", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        'Kintell 12/12/2011
        Dim parameterCodSubtipo As New SqlParameter("@cod_Subtipo", SqlDbType.NVarChar, 3)
        parameterCodSubtipo.Value = Subtipo
        myCommand.SelectCommand.Parameters.Add(parameterCodSubtipo)

        Dim parameteridIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 3)
        parameteridIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameteridIdioma)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function


    Public Function GetSingleMotivoCancelacion(ByVal cod_mot As String, ByVal idIdioma As Integer) As SqlDataReader

        ' Create Instance of Connection and Command Object
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_GetSingleMotivo_cancelacion", myConnection)

        ' Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterCodMot As New SqlParameter("@cod_mot", SqlDbType.Int, 4)
        parameterCodMot.Value = Integer.Parse(cod_mot)
        myCommand.Parameters.Add(parameterCodMot)

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = Integer.Parse(idIdioma)
        myCommand.Parameters.Add(parameterIdIdioma)

        ' Execute the command
        myConnection.Open()
        Dim result As SqlDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

        ' Return the datareader 
        Return result

    End Function


End Class
