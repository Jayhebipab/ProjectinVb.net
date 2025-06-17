Imports MySql.Data.MySqlClient
Public Class AuditF
    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)
    End Sub

    Private Sub IconButton3_Click(sender As Object, e As EventArgs) Handles IconButton3.Click
        Dim nextForm As New MainMenuf()
        nextForm.Show()
        Me.Hide()
    End Sub
    Private Sub show()
        Try
            ' Open the connection to the database
            opencon()

            ' Create a new DataTable to hold the data from the query
            Dim table As New DataTable()

            ' Create a MySqlDataAdapter to execute the query and fill the DataTable
            Dim adpt As New MySqlDataAdapter("SELECT * FROM audittbl", con)

            ' Fill the DataTable with data from the audittbl table
            adpt.Fill(table)

            ' Bind the DataTable to the DataGridView
            DataGridView1.DataSource = table

            ' Clear any current selection in the DataGridView
            DataGridView1.ClearSelection()

            ' Optionally, resize all columns to fit their content
            DataGridView1.AutoResizeColumns()

            ' Alternatively, resize just the "Action" column manually
            ' Assuming the "Action" column is the fourth column (index 3), you can adjust the width:
            If DataGridView1.Columns.Contains("Action") Then
                DataGridView1.Columns("Action").Width = 900 ' Set the width of the Action column (adjust as needed)
            End If

        Catch ex As MySqlException
            ' Handle MySQL-specific errors
            MsgBox("Database error: " & ex.Message)

        Catch ex As Exception
            ' Handle any other general errors
            MsgBox("An error occurred: " & ex.Message)

        Finally
            ' Close the database connection
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

    End Sub
    Private Sub IconButton15_Click(sender As Object, e As EventArgs) Handles IconButton15.Click
        show()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs)

    End Sub
    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        ' Check if the selected date is in the future
        If DateTimePicker1.Value > DateTime.Now Then
            ' Show a message if the user selects a future date
            MsgBox("Please select a date on or before today's date.", MsgBoxStyle.Exclamation, "Invalid Date")

            ' Reset the DateTimePicker value to today's date
            DateTimePicker1.Value = DateTime.Now
            Return ' Exit the method to prevent further processing
        End If

        ' Proceed with the database query if the selected date is valid (not in the future)
        Dim con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
        Try
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If

            ' Get the selected date and format it as "yyyy-MM-dd"
            Dim selectedDate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
            Dim query As String = "SELECT * FROM audittbl WHERE DATE(DateTime) = @selectedDate"

            Using cmd As New MySqlCommand(query, con)
                cmd.Parameters.AddWithValue("@selectedDate", selectedDate)

                ' Fill the DataGridView with the data returned from the query
                Dim dt As New DataTable()
                Using da As New MySqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
                DataGridView1.DataSource = dt
            End Using
        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub


    Private Sub AuditF_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        show()
        DateTimePicker1.MinDate = New DateTime(2024, 11, 23)
    End Sub
End Class
