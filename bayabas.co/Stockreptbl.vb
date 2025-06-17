Imports MySql.Data.MySqlClient
Public Class Stockreptbl
    Private Sub IconButton3_Click(sender As Object, e As EventArgs) Handles IconButton3.Click
        Dim nextForm As New MainMenuf()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton15_Click(sender As Object, e As EventArgs) Handles IconButton15.Click
        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using statement ensures the connection is properly disposed of
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection

                ' Define the SQL query to join product and quantity tables
                Dim query As String = "SELECT producttbl.Barcode, Description, Cat_name, Color, ItemSize, Selling_price,Cost_price, Quantity " &
                                      "FROM producttbl " &
                                      "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                                      "LIMIT 1000;"
                Dim cmd As New MySqlCommand(query, con)

                ' Fill the data into a DataSet
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim ds As New DataSet()
                adapter.Fill(ds)

                ' Check if data is available and bind it to the DataGridView
                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    DataGridView1.DataSource = ds.Tables(0)
                Else

                End If
            End Using

        Catch ex As MySqlException
            ' Handle database-specific exceptions
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ' Handle other exceptions
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using statement ensures the connection is properly disposed of
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection

                ' Define the SQL query with filtering by Category_name
                Dim query As String = "SELECT producttbl.Barcode, Description, Cat_name, Color, ItemSize, Selling_price, Cost_price, Quantity " &
                              "FROM producttbl " &
                              "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                              "WHERE Cat_name = @Cat_name " &
                              "LIMIT 1000;"
                Dim cmd As New MySqlCommand(query, con)

                ' Add the selected category from ComboBox as a parameter
                cmd.Parameters.AddWithValue("@Cat_name", ComboBox1.Text)

                ' Fill the data into a DataSet
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim ds As New DataSet()
                adapter.Fill(ds)

                ' Check if data is available and bind it to the DataGridView
                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    DataGridView1.DataSource = ds.Tables(0)
                Else

                End If
            End Using

        Catch ex As MySqlException
            ' Handle database-specific exceptions
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ' Handle other exceptions
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try




    End Sub
    Private Sub opencon()
        If con.State = ConnectionState.Closed Then
            con.ConnectionString = "server=localhost;username=root;password=;database=bayabas_co"
            con.Open()
        End If
    End Sub
    Private Sub Stockreptbl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            ' Open the database connection
            opencon()

            ' Create a MySQL command to select all data from Categorytbl
            Dim commande As New MySqlCommand("SELECT * FROM Categorytbl", con)
            Dim adapterre As New MySqlDataAdapter(commande)
            Dim tableee As New DataTable()

            ' Fill the DataTable with data from the Categorytbl
            adapterre.Fill(tableee)

            ' Clear existing items in the ComboBox
            ComboBox1.Items.Clear()

            If tableee.Rows.Count > 0 Then
                ComboBox1.Text = "" ' Optionally clear the current selected value in the ComboBox
                For Each row As DataRow In tableee.Rows
                    ComboBox1.Items.Add(row("Category_name").ToString()) ' Add each category name to the ComboBox
                Next

                ' Optionally select the first item in the ComboBox
                ComboBox1.SelectedIndex = 0
            Else
                ' Optionally handle the case where no data is found

            End If


            con.Close()
        Catch ex As MySqlException
            ' Handle MySQL-specific exceptions
            MsgBox("MySQL Error: " & ex.Message)
        Catch ex As Exception
            ' Handle other exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try
        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using statement ensures the connection is properly disposed of
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection

                ' Define the SQL query to join product and quantity tables
                Dim query As String = "SELECT producttbl.Barcode, Description, Cat_name, Color, ItemSize, Selling_price,Cost_price, Quantity " &
                                      "FROM producttbl " &
                                      "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                                      "LIMIT 1000;"
                Dim cmd As New MySqlCommand(query, con)

                ' Fill the data into a DataSet
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim ds As New DataSet()
                adapter.Fill(ds)

                ' Check if data is available and bind it to the DataGridView
                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    DataGridView1.DataSource = ds.Tables(0)
                Else

                End If
            End Using

        Catch ex As MySqlException
            ' Handle database-specific exceptions
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ' Handle other exceptions
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)

    End Sub
    Private Sub labas()
        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using statement ensures the connection is properly disposed of
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection

                ' Define the SQL query with filtering by Category_name
                Dim query As String = "SELECT producttbl.Barcode, Description, Cat_name, Color, ItemSize, Selling_price, Cost_price, Quantity " &
                              "FROM producttbl " &
                              "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                              "WHERE Cat_name = @CategoryName " &
                              "LIMIT 1000;"
                Dim cmd As New MySqlCommand(query, con)

                ' Add the selected category from ComboBox as a parameter
                cmd.Parameters.AddWithValue("@CategoryName", ComboBox1.Text)

                ' Fill the data into a DataSet
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim ds As New DataSet()
                adapter.Fill(ds)

                ' Check if data is available and bind it to the DataGridView
                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    DataGridView1.DataSource = ds.Tables(0)
                Else

                End If
            End Using

        Catch ex As MySqlException
            ' Handle database-specific exceptions
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ' Handle other exceptions
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        labas()

    End Sub

End Class