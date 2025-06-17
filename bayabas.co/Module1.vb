Imports MySql.Data.MySqlClient
Imports System.IO

Module Module1

    Public con As New MySqlConnection
    Public cmd As New MySqlCommand
    Public vattax As Integer = 0
    Public loginAsSuperAdmin As Boolean = False
    Public loginAsAdmin As Boolean = False
    Public loginAsStaff As Boolean = False
    Public username1 As String = ""
    Public position As String = ""
    Public names As String = ""
    Public VAT As String = ""


    Sub opencon()
        con.ConnectionString = "server=localhost;username=root;password=;database=bayabas_co"
        con.Open()
    End Sub


    Sub reset()
        loginAsSuperAdmin = False
        loginAsAdmin = False
        loginAsStaff = False
        username1 = ""
        position = ""
        names = ""
    End Sub
End Module
