Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class ContratosDB


    Public Function GetSingleContrato(ByVal cod_contrato As Integer) As SqlDataReader

        ' Create Instance of Connection and Command Object
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlCommand("MTOGASBD_GetSingleContrato", myConnection)

        ' Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure

        ' Add Parameters to SPROC
        Dim parameterCodContrato As New SqlParameter("@Cod_contrato", SqlDbType.NVarChar, 10)
        parameterCodContrato.Value = cod_contrato
        myCommand.Parameters.Add(parameterCodContrato)

        ' Execute the command
        myConnection.Open()
        Dim result As SqlDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

        ' Return the datareader 
        Return result

    End Function


End Class
