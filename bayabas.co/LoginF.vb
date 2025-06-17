Imports MySql.Data.MySqlClient
Public Class LoginF
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            ' Create a new MySqlConnection object with the updated connection string
            Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                ' Open the connection
                con.Open()

                ' Adjust query to use correct column names as per your database schema
                Dim cmd As New MySqlCommand("SELECT User_type, Firstname, Username FROM usertbl WHERE Username=@Username AND Password=@Password", con)

                ' Add parameters for the query
                cmd.Parameters.AddWithValue("@Username", tbuser.Text.Trim())
                cmd.Parameters.AddWithValue("@Password", tbpass.Text.Trim())

                ' Use MySqlDataAdapter to fill a DataTable
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim myTable As New DataTable()
                adapter.Fill(myTable)

                ' Check if the query returned any results
                If myTable.Rows.Count > 0 Then
                    Dim userType As String = myTable.Rows(0)("User_type").ToString()
                    Dim firstname As String = myTable.Rows(0)("Firstname").ToString()
                    Dim firstnames As String = myTable.Rows(0)("Username").ToString()

                    ' Determine which form to show based on user type
                    If userType = "SuperAdmin" Then
                        names = firstnames
                        username1 = firstname
                        position = "SuperAdmin"
                        loginAsSuperAdmin = True
                        MainMenuf.Show()
                        Me.Hide()

                    ElseIf userType = "Admin" Then
                        names = firstnames
                        username1 = firstname
                        position = "Admin"
                        loginAsAdmin = True
                        MainMenuf.Show()
                        Me.Hide()

                    ElseIf userType = "Staff" Then
                        names = firstnames
                        username1 = firstname
                        position = "Staff"
                        loginAsStaff = True
                        MainMenuf.Show()
                        Me.Hide()

                    End If
                Else
                    MsgBox("Invalid Username or Password")
                End If
            End Using
        Catch ex As Exception
            ' Display any exceptions that occur
            MsgBox("Error: " & ex.Message)
        Finally
            ' Clear textboxes after the login attempt
            tbpass.Text = ""
            tbuser.Text = ""
        End Try

    End Sub
    Private Sub form()
        Dim bindingSource As New BindingSource()

        ' Load data sa DataTable
        Dim tabledqq As New DataTable()
        Dim adptdqq As New MySqlDataAdapter("SELECT * FROM usertbl", con)
        adptdqq.Fill(tabledqq)

        ' Ibind ang DataTable sa BindingSource
        bindingSource.DataSource = tabledqq

        ' Ibind ang BindingSource sa DataGridView
        DataGridView2.DataSource = bindingSource

        ' Isara ang koneksyon pagkatapos mag-load ng data
        con.Close()

        ' Halimbawa ng search function (kunyari may TextBox ka na tinatawag na txtSearch)
        Dim filter As String = "Username LIKE '%" & TextBox4.Text & "%'"

        ' I-set ang filter ng BindingSource
        bindingSource.Filter = filter

        ' Kapag may matching na username, kunin ang Full Name at ilagay sa ComboBox
        If bindingSource.Count > 0 Then
            ' I-clear ang ComboBox bago magdagdag ng bagong data
            cbfav.Items.Clear()

            ' Halimbawa, ang column na "fullname" ang naglalaman ng buong pangalan
            For Each row As DataRowView In bindingSource
                Dim fullname As String = row("Favorite").ToString()
                cbfav.Items.Add(fullname)
            Next
        End If
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TextBox2.Text = "Owner"
        TextBox1.Text = "Bayabas"
        opencon()

        Dim tabledq As New DataTable()
        Dim adptdq As New MySqlDataAdapter("SELECT * FROM usertbl ", con)
        adptdq.Fill(tabledq)

        DataGridView1.DataSource = tabledq
        con.Close()

        Try
            ' Define your MySQL database connection
            Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                con.Open()

                ' Query to count rows in the usertbl
                Dim query As String = "SELECT COUNT(*) FROM usertbl"
                Using cmd As New MySqlCommand(query, con)
                    Dim userCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    ' Check if the table is empty
                    If userCount = 0 Then
                        ' Show Panel3 if no data in usertbl
                        Panel3.Visible = True
                        Button3.Visible = True
                        Label6.Visible = True
                    Else
                        ' Hide Panel3 if data exists
                        Panel3.Visible = False

                        Button3.Visible = False
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Handle any errors
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            tbpass.UseSystemPasswordChar = False
        Else
            tbpass.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub tbpass_TextChanged(sender As Object, e As EventArgs) Handles tbpass.TextChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            ' Define your MySQL database connection
            Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                con.Open()

                ' Insert new user
                Using cmd As New MySqlCommand("INSERT INTO usertbl (Firstname, Lastname, Middle_name, User_type, Gender, Username, Password, Con_password) 
                                       VALUES (@Firstname, @Lastname, @Middle_name, @User_type, @Gender, @Username, @Password, @Con_password)", con)
                    ' Add parameters to avoid SQL injection
                    cmd.Parameters.AddWithValue("@Firstname", "")
                    cmd.Parameters.AddWithValue("@Lastname", "")
                    cmd.Parameters.AddWithValue("@Middle_name", "")
                    cmd.Parameters.AddWithValue("@User_type", "SuperAdmin")
                    cmd.Parameters.AddWithValue("@Gender", "")
                    cmd.Parameters.AddWithValue("@Username", TextBox2.Text.Trim()) ' Replace with your TextBox for username
                    cmd.Parameters.AddWithValue("@Password", TextBox1.Text.Trim()) ' Replace with your TextBox for password
                    cmd.Parameters.AddWithValue("@Con_password", TextBox1.Text.Trim()) ' Replace with your TextBox for confirm password

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery() ' Execute the insert command

                    ' Check if the insert was successful
                    If rowsAffected > 0 Then
                        MessageBox.Show("User successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Open the MainMenuf and make Panel6 visible
                        Dim nextForm As New MainMenuf()
                        nextForm.Panel6.Visible = True
                        nextForm.Show()

                        ' Hide the current form
                        Me.Hide()
                    Else
                        MessageBox.Show("Failed to add user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Handle any errors
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs)



    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Check if the fields are not empty or invalid
        If String.IsNullOrEmpty(TextBox4.Text) Then
            MessageBox.Show("Please enter a username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf String.IsNullOrEmpty(cbfav.Text) Then
            MessageBox.Show("Please select a favorite.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf String.IsNullOrEmpty(tbanswer.Text) Then
            MessageBox.Show("Please provide an answer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ' If all fields are filled, proceed to select data from usertbl
            Dim username As String = TextBox4.Text
            Dim favorite As String = cbfav.Text
            Dim answer As String = tbanswer.Text

            ' Define the connection string (adjust according to your database)
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Use MySQL Connection
            Using conn As New MySqlConnection(connectionString)
                Try
                    ' Open the connection
                    conn.Open()

                    ' SQL Query to select user data based on the provided username, favorite, and answer
                    Dim query As String = "SELECT * FROM usertbl WHERE username = @username AND favorite = @favorite AND answer = @answer"

                    ' Create the command with parameters
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@username", username)
                        cmd.Parameters.AddWithValue("@favorite", favorite)
                        cmd.Parameters.AddWithValue("@answer", answer)

                        ' Execute the query and read the result
                        Using reader As MySqlDataReader = cmd.ExecuteReader()
                            If reader.HasRows Then
                                TextBox3.Text = TextBox4.Text
                                Panel4.Visible = True

                            Else
                                MsgBox("Invalid Username, Favorite, or Answer", MsgBoxStyle.Critical)
                            End If
                        End Using
                    End Using
                Catch ex As Exception
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    conn.Close()
                End Try
            End Using
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

        ' Use MySQL Connection
        Using conn As New MySqlConnection(connectionString)
            Try
                ' Open the connection
                conn.Open()

                ' Define the command and query
                Dim cmd As New MySqlCommand("SELECT User_type, Firstname, Lastname, Username FROM usertbl WHERE Username = @username", conn)
                cmd.Parameters.AddWithValue("@username", TextBox3.Text) ' Replace this with actual user input

                ' Execute the query and read the result
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        ' Read the data from the first row
                        reader.Read() ' Move to the first row
                        Dim userType As String = reader("User_type").ToString()
                        Dim firstname As String = reader("Firstname").ToString()
                        Dim lastnames As String = reader("Lastname").ToString()
                        Dim usern As String = reader("Username").ToString()
                        Dim name As String = $"{firstname}"

                        ' Determine which form to show based on user type
                        If userType = "SuperAdmin" Then
                            username1 = name
                            names = usern
                            position = "SuperAdmin"
                            loginAsSuperAdmin = True
                            MainMenuf.Show()
                            Me.Hide()

                        ElseIf userType = "Admin" Then
                            username1 = name
                            names = usern
                            position = "Admin"
                            loginAsAdmin = True
                            MainMenuf.Show()
                            Me.Hide()

                        ElseIf userType = "Staff" Then
                            username1 = name
                            names = usern
                            position = "Staff"
                            loginAsStaff = True
                            MainMenuf.Show()
                            Me.Hide()

                        End If
                    Else
                        ' If no user found, show an error message
                        MessageBox.Show("User not found or invalid username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using

            Catch ex As MySqlException
                ' Handle MySQL-specific exceptions
                MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Catch ex As Exception
                ' Handle general exceptions
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Finally
                ' Ensure the connection is closed
                conn.Close()
            End Try
        End Using

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

        ' Ensure the username and new password inputs are not empty
        If String.IsNullOrEmpty(TextBox3.Text) OrElse String.IsNullOrEmpty(TextBox5.Text) Then
            MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Use MySQL Connection
        Using conn As New MySqlConnection(connectionString)
            Try
                ' Open the connection
                conn.Open()

                ' SQL Query to update the password directly without checking the current password
                Dim updateQuery As String = "UPDATE usertbl SET password = @newPassword, Con_password = @Con_password WHERE username = @username"

                ' Hash the new password before saving it to the database (use a proper hashing mechanism)
                Dim hashedNewPassword As String = TextBox5.Text ' Assuming it's already hashed

                ' Create the command with parameters to update the password
                Using updateCmd As New MySqlCommand(updateQuery, conn)
                    updateCmd.Parameters.AddWithValue("@newPassword", hashedNewPassword)
                    updateCmd.Parameters.AddWithValue("@Con_password", hashedNewPassword)
                    updateCmd.Parameters.AddWithValue("@username", TextBox3.Text)

                    ' Execute the update query
                    Dim rowsAffected As Integer = updateCmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        ' Define the command and query to fetch user information
                        Dim cmd As New MySqlCommand("SELECT User_type, Firstname, Lastname, Username FROM usertbl WHERE Username = @username", conn)
                        cmd.Parameters.AddWithValue("@username", TextBox3.Text)

                        ' Execute the query and read the result
                        Using reader As MySqlDataReader = cmd.ExecuteReader()
                            If reader.HasRows Then
                                ' Read the data from the first row
                                reader.Read() ' Move to the first row
                                Dim userType As String = reader("User_type").ToString()
                                Dim firstname As String = reader("Firstname").ToString()
                                Dim lastnames As String = reader("Lastname").ToString()
                                Dim usern As String = reader("Username").ToString()
                                Dim name As String = $"{firstname}"

                                ' Determine which form to show based on user type
                                If userType = "SuperAdmin" Then
                                    username1 = name
                                    names = usern
                                    position = "SuperAdmin"
                                    loginAsSuperAdmin = True
                                    MainMenuf.Show()
                                    Me.Hide()

                                ElseIf userType = "Admin" Then
                                    username1 = name
                                    names = usern
                                    position = "Admin"
                                    loginAsAdmin = True
                                    MainMenuf.Show()
                                    Me.Hide()

                                ElseIf userType = "Staff" Then
                                    username1 = name
                                    names = usern
                                    position = "Staff"
                                    loginAsStaff = True
                                    MainMenuf.Show()
                                    Me.Hide()

                                End If
                            Else
                                ' If no user found, show an error message
                                MessageBox.Show("User not found or invalid username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End Using
                    Else
                        MessageBox.Show("Error updating password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using

            Catch ex As MySqlException
                ' Handle MySQL-specific exceptions
                MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Catch ex As Exception
                ' Handle general exceptions
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Finally
                ' Ensure the connection is closed
                conn.Close()
            End Try
        End Using





    End Sub
    Private Sub TextBox4_KeyPressu(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        ' Check if the pressed key is a special character (not alphanumeric)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If TextBox4.Text.Length >= 15 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged

    End Sub
    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        ' Check if the pressed key is a special character (not alphanumeric)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If TextBox5.Text.Length >= 15 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        form()
    End Sub
    Private Sub tbanswer_KeyPresss(sender As Object, e As KeyPressEventArgs) Handles tbanswer.KeyPress
        ' Check if the pressed key is a special character (not alphanumeric)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If tbanswer.Text.Length >= 15 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub tbanswer_TextChanged(sender As Object, e As EventArgs) Handles tbanswer.TextChanged

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        Panel2.Visible = True
    End Sub

    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Button2.Click
        Panel2.Visible = False
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        Panel2.Visible = True
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            TextBox5.UseSystemPasswordChar = False
        Else
            TextBox5.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox1.Checked Then
            TextBox1.UseSystemPasswordChar = False
        Else
            TextBox1.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs) Handles Panel4.Paint

    End Sub
End Class
