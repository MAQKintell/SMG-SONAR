Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Public Class TiposAveriaDB




    Public Function GetTiposAveria(ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetTipo_averias", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function


    Public Function GetTipoAveriaMasDe21DiasDesdeCierreVisita(ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetTipo_averia_Mas_de_21Dias_desde_cierre_visita", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)

        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)

        Return myDataSet

    End Function


    '#2094: Pago Anticipado
    Public Function GetTipoLugarAveria(ByVal idIdioma As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("connectionString"))
        Dim myCommand As New SqlDataAdapter("MTOGASBD_GetTipoLugarAveria", myConnection)
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure


        Dim parameterIdIdioma As New SqlParameter("@idIDIOMA", SqlDbType.Int, 4)
        parameterIdIdioma.Value = idIdioma
        myCommand.SelectCommand.Parameters.Add(parameterIdIdioma)


        Dim myDataSet As New DataSet()
        myCommand.Fill(myDataSet)



        Return myDataSet

    End Function





End Class
