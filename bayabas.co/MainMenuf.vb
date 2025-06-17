Imports MySql.Data.MySqlClient
Imports System.Text
Imports System.Security.Cryptography
Imports ZXing
Imports System.IO
Imports System.Drawing.Printing
Imports System.Drawing

Public Class MainMenuf
    Private receiptContent As String = ""
    Private WithEvents printDoc As New PrintDocument
    Private Sub PrintPageHandler(sender As Object, e As PrintPageEventArgs)
        Dim font As New Font("Courier New", 10)
        Dim x As Single = e.MarginBounds.Left
        Dim y As Single = e.MarginBounds.Top
        e.Graphics.DrawString(receiptContent, font, Brushes.Black, x, y)
    End Sub

    ' Button Click to Generate and Print the Receipt


    Private Sub IconButton1_Click(sender As Object, e As EventArgs) Handles IconButton1.Click
        Panel3.Visible = True
        Panel2.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel6.Visible = False
        Panel28.Visible = False
        Panel33.Visible = False
    End Sub

    Private Sub IconButton7_Click(sender As Object, e As EventArgs) Handles IconButton7.Click
        Panel3.Visible = False
        Panel2.Visible = True
        Panel4.Visible = False
        Panel5.Visible = False
        Panel6.Visible = False
        Panel28.Visible = False
        Panel33.Visible = False
    End Sub

    Private Sub IconButton4_Click(sender As Object, e As EventArgs) Handles IconButton4.Click

        If DataGridView1.Rows.Count = 0 Then
            CheckBox3.Enabled = False
        Else
            CheckBox3.Enabled = True
        End If

        CN1.SelectedIndex = -1
        CN.SelectedIndex = -1
        COR.SelectedIndex = -1
        COR1.SelectedIndex = -1
        Size.SelectedIndex = -1
        Size1.SelectedIndex = -1
        Panel3.Visible = False
        Panel2.Visible = False
        Panel4.Visible = True
        Panel5.Visible = False
        Panel6.Visible = False
        Panel28.Visible = False
        Panel33.Visible = False

    End Sub

    Private Sub Panel6_Paint(sender As Object, e As PaintEventArgs) Handles Panel6.Paint

    End Sub

    Private Sub IconButton2_Click(sender As Object, e As EventArgs) Handles IconButton2.Click
        Panel5.Visible = True
        Panel3.Visible = False
        Panel2.Visible = False
        Panel4.Visible = False
        Panel6.Visible = False
        Panel28.Visible = False
        Panel33.Visible = False

    End Sub

    Private Sub IconButton3_Click(sender As Object, e As EventArgs) Handles IconButton3.Click

        Panel3.Visible = False
        Panel2.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel6.Visible = True
        Panel28.Visible = False
        Panel33.Visible = False

    End Sub

    Private Sub IconButton24_Click(sender As Object, e As EventArgs)



    End Sub

    Private Sub ret()

    End Sub
    Private Sub IconButton9_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub IconButton23_Click(sender As Object, e As EventArgs)
        If DataGridView2.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView2.SelectedRows(0)
            Dim categoryNameToDelete As String = selectedRow.Cells(0).Value.ToString()

            Dim result As DialogResult = MsgBox("Are you sure you want to delete the selected category?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Deletion")
            If result = DialogResult.No Then
                Exit Sub
            End If

            Try
                If con.State <> ConnectionState.Open Then
                    con.Open()
                End If

                Using cmd As New MySqlCommand("DELETE FROM categorytbl WHERE category_name = @CategoryName", con)
                    cmd.Parameters.AddWithValue("@CategoryName", categoryNameToDelete)
                    cmd.ExecuteNonQuery()

                    Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, 'Deleted category')", con)
                        auditCmd.Parameters.AddWithValue("@User_type", Label45.Text)
                        auditCmd.Parameters.AddWithValue("@Name", Label26.Text)
                        auditCmd.ExecuteNonQuery()
                    End Using

                    MsgBox("Category successfully deleted!")
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

        Else
            MsgBox("Please select a category to delete.")
        End If
        Me.Hide()

        ' Dispose the current form (optional, if you don't need the old form anymore)
        Me.Dispose()

        ' Create a new instance of the form (ensure MainMenuf is the correct form name)
        Dim newForm As New MainMenuf() ' Replace MainMenuf with your actual form name
        newForm.Panel3.Visible = True
        newForm.Show()

    End Sub
    Private Sub prod()
        opencon()

        Dim table As New DataTable()
        Dim adpt As New MySqlDataAdapter("SELECT * FROM producttbl", con)
        adpt.Fill(table)

        DataGridView1.DataSource = table
        DataGridView1.ClearSelection()
        con.Close()
    End Sub
    Private Sub sup()
        opencon()

        Dim table As New DataTable()
        Dim adpt As New MySqlDataAdapter("SELECT * FROM suppliertbl", con)
        adpt.Fill(table)

        DataGridView3.DataSource = table
        DataGridView3.ClearSelection()
        con.Close()
    End Sub
    Private Sub IconButton12_Click(sender As Object, e As EventArgs)
        prod()
    End Sub

    Private Sub IconButton20_Click(sender As Object, e As EventArgs)
        ' Validate all fields, including the image
        If String.IsNullOrWhiteSpace(Barc.Text) OrElse
   String.IsNullOrWhiteSpace(Desc.Text) OrElse
   String.IsNullOrWhiteSpace(PN.Text) OrElse
   String.IsNullOrWhiteSpace(CN.Text) OrElse
   String.IsNullOrWhiteSpace(Size.Text) OrElse
   String.IsNullOrWhiteSpace(SP.Text) OrElse
   String.IsNullOrWhiteSpace(COR.Text) OrElse
   PictureBox2.Image Is Nothing Then ' Check if an image is uploaded
            MsgBox("Please fill in all fields and upload an image.")
            Return
        End If

        Try
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If

            ' Convert the image to a byte array
            Dim imgBytes As Byte()
            Using ms As New MemoryStream()
                ' Save the image to the MemoryStream (no format check)
                PictureBox2.Image.Save(ms, PictureBox2.Image.RawFormat)
                imgBytes = ms.ToArray()
            End Using

            ' Check for duplicate Barcode
            Dim duplicateCheckCmd As New MySqlCommand("SELECT COUNT(*) FROM producttbl WHERE Barcode = @Barcode", con)
            duplicateCheckCmd.Parameters.AddWithValue("@Barcode", Barc.Text)
            Dim barcodeCount As Integer = Convert.ToInt32(duplicateCheckCmd.ExecuteScalar())
            If barcodeCount > 0 Then
                MsgBox("A product with this Barcode already exists.")
                Return
            End If

            ' Insert data into the database
            Using cmd As New MySqlCommand()
                cmd.Connection = con
                cmd.CommandText = "INSERT INTO producttbl (`Barcode`, `Product_Name`, `Description`, `Category_name`, `Size`, `Selling_price`, `Color`, `Image`) VALUES (@Barcode, @Product_Name, @Description, @Category_name, @Size, @Selling_price, @Color, @Image)"
                cmd.Parameters.AddWithValue("@Barcode", Barc.Text)
                cmd.Parameters.AddWithValue("@Product_Name", PN.Text)
                cmd.Parameters.AddWithValue("@Description", Desc.Text)
                cmd.Parameters.AddWithValue("@Category_name", CN.Text)
                cmd.Parameters.AddWithValue("@Size", Size.Text)
                cmd.Parameters.AddWithValue("@Selling_price", Convert.ToDecimal(SP.Text))
                cmd.Parameters.AddWithValue("@Color", COR.Text)
                cmd.Parameters.AddWithValue("@Image", imgBytes) ' Add the image as a byte array

                cmd.ExecuteNonQuery()
                MsgBox("Product successfully added!")
            End Using

            ' Refresh your DataGridView or clear input fields here

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

        Dim writer As New BarcodeWriter
        writer.Format = BarcodeFormat.CODE_128 ' Ensure BarcodeFormat is recognized

        ' Get the input text
        Dim inputText As String = Barc.Text

        ' Validate the input text: Check if it's empty or contains non-integer characters
        If String.IsNullOrWhiteSpace(inputText) Then
            MessageBox.Show("Please enter valid text for the barcode.")
            Exit Sub
        End If

        If Not IsNumeric(inputText) OrElse inputText.Contains(".") Then
            MessageBox.Show("Only whole numbers (integers) are allowed.")
            Exit Sub
        End If

        ' Generate and display the barcode
        Try
            PIC1.Image = writer.Write(inputText)
            PIC1.Refresh() ' Ensure the PictureBox refreshes to show the image
        Catch ex As Exception
            MessageBox.Show("Error generating barcode: " & ex.Message)
        End Try
        prod()

    End Sub

    Private Sub IconButton19_Click(sender As Object, e As EventArgs)
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select a product to delete.")
            Return
        End If

        Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
        Dim barcode As String = selectedRow.Cells("Barcode").Value.ToString()

        If MsgBox($"Are you sure you want to delete the product with barcode '{barcode}'?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Return
        End If

        Try
            Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                con.Open()

                Using cmd As New MySqlCommand("DELETE FROM producttbl WHERE Barcode = @Barcode", con)
                    cmd.Parameters.AddWithValue("@Barcode", barcode)
                    cmd.ExecuteNonQuery()
                    MsgBox("Product successfully deleted!")
                End Using

                ' Optionally refresh your DataGridView or other UI elements here
                prod()

            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub IconButton21_Click(sender As Object, e As EventArgs)
        prod()
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs)
        ' Check if there are any selected rows
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Get the first selected row
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Safely populate the input fields with data from the selected row
            Barc.Text = If(selectedRow.Cells("Barcode").Value?.ToString(), "")
            Desc.Text = If(selectedRow.Cells("Description").Value?.ToString(), "")
            PN.Text = If(selectedRow.Cells("Product_name").Value?.ToString(), "")

            ' Set the ComboBox selected value for Category_name
            Dim categoryName As String = If(selectedRow.Cells("Category_name").Value?.ToString(), "")
            If CN.Items.Contains(categoryName) Then
                CN.SelectedItem = categoryName
            End If

            ' Set the ComboBox selected value for Size
            Dim sizeValue As String = If(selectedRow.Cells("Size").Value?.ToString(), "")
            If Size.Items.Contains(sizeValue) Then
                Size.SelectedItem = sizeValue
            End If

            SP.Text = If(selectedRow.Cells("Selling_price").Value?.ToString(), "")
            COR.Text = If(selectedRow.Cells("Color").Value?.ToString(), "")



            Barc.Text = ""
            Desc.Text = ""
            PN.Text = ""
            CN.SelectedIndex = -1  ' Clear the ComboBox selection
            Size.SelectedIndex = -1 ' Clear the ComboBox selection
            SP.Text = ""
            COR.Text = ""

        End If

    End Sub

    Private Sub IconButton22_Click(sender As Object, e As EventArgs)
        ' Check if a row is selected in the DataGridView
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim barcode As String = If(selectedRow.Cells("Barcode").Value?.ToString(), "")

            ' Validate input fields
            If String.IsNullOrWhiteSpace(Barc.Text) OrElse
       String.IsNullOrWhiteSpace(Desc.Text) OrElse
       String.IsNullOrWhiteSpace(PN.Text) OrElse
       String.IsNullOrWhiteSpace(CN.Text) OrElse
       String.IsNullOrWhiteSpace(Size.Text) OrElse
       String.IsNullOrWhiteSpace(SP.Text) OrElse
       String.IsNullOrWhiteSpace(COR.Text) Then
                MsgBox("Please fill in all fields.")
                Return
            End If

            ' Validate image
            If PictureBox2.Image Is Nothing Then
                MsgBox("Please upload an image.")
                Return
            End If

            Try
                ' Open the connection
                If con.State <> ConnectionState.Open Then
                    con.Open()
                End If

                ' Convert the image to a byte array
                Dim imgBytes As Byte()
                Using ms As New MemoryStream()
                    Try
                        ' Ensure the image is not null
                        If PictureBox2.Image Is Nothing Then
                            Throw New InvalidOperationException("No image found in the PictureBox.")
                        End If

                        ' Save the image in its current format (no format restriction)
                        PictureBox2.Image.Save(ms, PictureBox2.Image.RawFormat)
                        imgBytes = ms.ToArray()
                    Catch ex As Exception
                        MsgBox("Error while converting image: " & ex.Message)
                        Return
                    End Try
                End Using

                ' Update the database
                Using cmd As New MySqlCommand()
                    cmd.Connection = con
                    cmd.CommandText = "UPDATE producttbl SET `Product_Name` = @Product_Name, `Description` = @Description, " &
                              "`Category_name` = @Category_name, `Size` = @Size, `Selling_price` = @Selling_price, " &
                              "`Color` = @Color, `Image` = @Image WHERE `Barcode` = @Barcode"

                    ' Add parameters
                    cmd.Parameters.AddWithValue("@Product_Name", PN.Text)
                    cmd.Parameters.AddWithValue("@Description", Desc.Text)
                    cmd.Parameters.AddWithValue("@Category_name", CN.Text)
                    cmd.Parameters.AddWithValue("@Size", Size.Text)
                    cmd.Parameters.AddWithValue("@Selling_price", Convert.ToDecimal(SP.Text))
                    cmd.Parameters.AddWithValue("@Color", COR.Text) ' Ensure this is correct
                    cmd.Parameters.AddWithValue("@Barcode", barcode)
                    cmd.Parameters.AddWithValue("@Image", imgBytes)

                    ' Execute the query
                    cmd.ExecuteNonQuery()
                    MsgBox("Product successfully updated!")
                End Using
            Catch ex As MySqlException
                MsgBox("Database error: " & ex.Message)
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message)
            Finally
                ' Close the connection
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try
        Else
            MsgBox("Please select a product to update.")
        End If

        prod()
    End Sub

    Private Sub DataGridView2_SelectionChanged(sender As Object, e As EventArgs)
        If DataGridView2.SelectedRows.Count > 0 Then
            ' Get the first selected row
            Dim selectedRow As DataGridViewRow = DataGridView2.SelectedRows(0)

            ' Safely populate the input fields with data from the selected row
            TextBox1.Text = If(selectedRow.Cells("Category_name").Value?.ToString(), "")


        Else
            ' Clear the input fields if no row is selected
            TextBox1.Text = ""

        End If
    End Sub

    Private Sub IconButton31_Click(sender As Object, e As EventArgs) Handles IconButton31.Click
        ' Ensure the TextBox input is not empty before proceeding
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MsgBox("MISSING INPUT")
            Exit Sub
        End If

        ' Check if a row is selected in DataGridView2
        If DataGridView2.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView2.SelectedRows(0)

            ' Retrieve the current category name from the selected row
            Dim currentCategoryName As String = selectedRow.Cells(0).Value.ToString()

            ' Check if the new category name already exists (excluding the current category)
            For Each row As DataGridViewRow In DataGridView2.Rows
                ' Skip the selected row itself while checking for duplicates
                If row.Cells(0).Value IsNot Nothing AndAlso row.Cells(0).Value.ToString() = TextBox1.Text AndAlso row.Cells(0).Value.ToString() <> currentCategoryName Then
                    MsgBox("Category Name Already Exists")
                    Exit Sub
                End If
            Next

            Try
                ' Open the database connection if it is not already open
                If con.State <> ConnectionState.Open Then
                    con.Open()
                End If

                ' Create and execute the UPDATE command for the category
                Using cmd As New MySqlCommand("UPDATE categorytbl SET category_name = @NewCatName WHERE category_name = @CurrentCatName", con)
                    ' Add parameters for the update
                    cmd.Parameters.AddWithValue("@NewCatName", TextBox1.Text)
                    cmd.Parameters.AddWithValue("@CurrentCatName", currentCategoryName)

                    ' Execute the command
                    cmd.ExecuteNonQuery()

                    ' Log the action into the audit trail
                    Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, 'Updated category')", con)
                        ' Add parameters for the audit trail
                        auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' You can dynamically set the User_type based on the logged-in user
                        auditCmd.Parameters.AddWithValue("@Name", Label26.Text)

                        ' Execute the audit trail insert
                        auditCmd.ExecuteNonQuery()
                    End Using

                    ' Display success message
                    MsgBox("Category successfully updated!")

                End Using

            Catch ex As MySqlException
                ' Handle MySQL-specific exceptions
                MsgBox("Database error: " & ex.Message)
            Catch ex As Exception
                ' Handle other exceptions
                MsgBox("An error occurred: " & ex.Message)
            Finally
                ' Ensure the database connection is closed properly
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try

        Else
            MsgBox("Please select a category to update.")
        End If


        Me.Hide()

        ' Dispose the current form (optional)
        Me.Dispose()

        ' Create a new instance of the form
        Dim newForm As New MainMenuf()  ' Replace Form1 with the actual name of your form
        newForm.Panel3.Visible = True
        ' Show the new instance of the form
        newForm.Show()
    End Sub


    Private Sub IconButton13_Click(sender As Object, e As EventArgs)
        sup()
    End Sub


    Private Sub IconButton17_Click(sender As Object, e As EventArgs) Handles IconButton17.Click
        Panel8.Visible = False
        Panel20.Visible = True
    End Sub

    Private Sub DataGridView3_SelectionChanged(sender As Object, e As EventArgs)
        ' Check if there are any selected rows
        If DataGridView3.SelectedRows.Count > 0 Then
            ' Get the first selected row
            Dim selectedRow As DataGridViewRow = DataGridView3.SelectedRows(0)

            ' Safely populate the input fields with data from the selected row

            SN.Text = If(selectedRow.Cells("Supplier_name").Value?.ToString(), "")
            CNAME.Text = If(selectedRow.Cells("Company_name").Value?.ToString(), "")

            CADD.Text = If(selectedRow.Cells("Address").Value?.ToString(), "")
            CNUM.Text = If(selectedRow.Cells("Contact_number").Value?.ToString(), "")
            ' 
        Else
            ' Clear the input fields if no row is selected
            SN.Text = ""
            CNAME.Text = ""
            CADD.Text = ""
            CNUM.Text = ""
        End If
    End Sub

    Private Sub Panel5_Paint(sender As Object, e As PaintEventArgs) Handles Panel5.Paint

    End Sub

    Private Sub IconButton18_Click(sender As Object, e As EventArgs) Handles IconButton18.Click
        ' Check if any required field is empty
        If String.IsNullOrWhiteSpace(SN.Text) OrElse String.IsNullOrWhiteSpace(CNAME.Text) OrElse String.IsNullOrWhiteSpace(CNUM.Text) OrElse String.IsNullOrWhiteSpace(CADD.Text) Then
            MsgBox("Please fill all fields.")
            Return
        End If

        ' Validate contact number (must be exactly 11 digits and start with "09")
        Dim phoneRegex As New System.Text.RegularExpressions.Regex("^09\d{9}$")
        If Not phoneRegex.IsMatch(CNUM.Text) Then
            MsgBox("Contact number must start with 09 and be exactly 11 digits.")
            Return
        End If

        ' Check if the user has selected a row in the DataGridView
        If DataGridView3.SelectedRows.Count = 0 Then
            MsgBox("Please select a row to update.")
            Return
        End If

        Try
            ' Open the database connection if not already open
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If

            ' Start a transaction to ensure atomicity of the update
            Dim transaction As MySqlTransaction = con.BeginTransaction()

            Try
                ' Get the SM_ID from the selected row in DataGridView
                Dim selectedRow As DataGridViewRow = DataGridView3.SelectedRows(0)
                Dim smID As Integer = Convert.ToInt32(selectedRow.Cells("SM_ID").Value)

                ' Check if the address is already in use (ignoring the current supplier)
                Dim checkAddressCmd As New MySqlCommand("SELECT COUNT(*) FROM suppliertbl WHERE LOWER(Address) = LOWER(@Address) AND SM_ID <> @SM_ID", con, transaction)
                checkAddressCmd.Parameters.AddWithValue("@Address", CADD.Text)
                checkAddressCmd.Parameters.AddWithValue("@SM_ID", smID)
                Dim addressCount As Integer = Convert.ToInt32(checkAddressCmd.ExecuteScalar())

                If addressCount > 0 Then
                    MsgBox("This address is already in use by another supplier. Please enter a different address.")
                    Return
                End If

                ' Check if the company name is already in use (ignoring the current supplier)
                Dim checkNameCmd As New MySqlCommand("SELECT COUNT(*) FROM suppliertbl WHERE Company_name = @Company_name AND SM_ID <> @SM_ID", con, transaction)
                checkNameCmd.Parameters.AddWithValue("@Company_name", CNAME.Text)
                checkNameCmd.Parameters.AddWithValue("@SM_ID", smID)
                Dim nameCount As Integer = Convert.ToInt32(checkNameCmd.ExecuteScalar())

                If nameCount > 0 Then
                    MsgBox("This supplier name is already in use by another supplier. Please enter a different name.")
                    Return
                End If

                ' Check if the contact number is already in use (ignoring the current supplier)
                Dim checkContactCmd As New MySqlCommand("SELECT COUNT(*) FROM suppliertbl WHERE Contact_number = @Contact_number AND SM_ID <> @SM_ID", con, transaction)
                checkContactCmd.Parameters.AddWithValue("@Contact_number", CNUM.Text)
                checkContactCmd.Parameters.AddWithValue("@SM_ID", smID)
                Dim contactCount As Integer = Convert.ToInt32(checkContactCmd.ExecuteScalar())

                If contactCount > 0 Then
                    MsgBox("This contact number is already in use by another supplier. Please enter a different contact number.")
                    Return
                End If

                ' Prepare the update query to update the supplier in the database, including Supplier_name
                Dim cmd As New MySqlCommand("UPDATE suppliertbl SET Supplier_name = @Supplier_name, Company_name = @Company_name, Contact_number = @Contact_number, Address = @Address WHERE SM_ID = @SM_ID", con, transaction)
                cmd.Parameters.AddWithValue("@SM_ID", smID)
                cmd.Parameters.AddWithValue("@Supplier_name", SN.Text) ' Supplier name is updated here
                cmd.Parameters.AddWithValue("@Company_name", CNAME.Text)
                cmd.Parameters.AddWithValue("@Contact_number", CNUM.Text)
                cmd.Parameters.AddWithValue("@Address", CADD.Text)

                ' Execute the update command
                cmd.ExecuteNonQuery()

                ' Log the action in the audit trail
                Using auditCmd As New MySqlCommand("INSERT INTO audittbl (DateTime, User_type, Name, Action) VALUES (NOW(), @User_type, @Name, @Action)", con, transaction)
                    auditCmd.Parameters.AddWithValue("@User_type", Label45.Text)
                    auditCmd.Parameters.AddWithValue("@Name", Label26.Text)

                    ' Set the Action to describe the specific update action happening
                    Dim actionDescription As String = "Updated supplier information for Supplier: " & SN.Text
                    auditCmd.Parameters.AddWithValue("@Action", actionDescription)

                    auditCmd.ExecuteNonQuery()
                End Using

                ' Commit the transaction if all operations are successful
                transaction.Commit()

                ' Show success message
                MsgBox("Supplier updated successfully.")

                ' Manually update the DataGridView (replace old row with new data)
                For Each row As DataGridViewRow In DataGridView3.Rows
                    If row.Cells("SM_ID").Value.ToString() = smID.ToString() Then
                        row.Cells("SM_ID").Value = smID
                        row.Cells("Supplier_name").Value = SN.Text ' Update Supplier name in the DataGridView
                        row.Cells("Company_name").Value = CNAME.Text
                        row.Cells("Contact_number").Value = CNUM.Text
                        row.Cells("Address").Value = CADD.Text
                        Exit For
                    End If
                Next

                ' Hide the panel after the update
                Panel8.Visible = False

            Catch ex As Exception
                ' If an error occurs, rollback the transaction and show an error message
                transaction.Rollback()
                MsgBox("An error occurred: " & ex.Message)
            End Try

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Finally
            ' Ensure the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

    End Sub

    Private Sub IconButton28_Click(sender As Object, e As EventArgs) Handles IconButton28.Click

        Dim con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")

        ' Check if all required fields are filled
        If String.IsNullOrWhiteSpace(Uname.Text) OrElse
   String.IsNullOrWhiteSpace(Fname.Text) OrElse
   String.IsNullOrWhiteSpace(Lname.Text) OrElse
   String.IsNullOrWhiteSpace(Pwd.Text) OrElse
   String.IsNullOrWhiteSpace(Cpwd.Text) Then
            MsgBox("MISSING INPUT")
            Return
        End If

        If Pwd.Text.Length < 7 Then
            MsgBox("Password must be at least 7 characters.")
            Return
        End If

        ' Check if passwords match
        If Pwd.Text <> Cpwd.Text Then
            MessageBox.Show("Password and Confirm Password do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Cpwd.Focus()
            Return
        End If

        ' Check if the username already exists
        Dim usernameCheckQuery As String = "SELECT COUNT(*) FROM usertbl WHERE Username = @Username"
        Try
            con.Open()
            Using usernameCheckCmd As New MySqlCommand(usernameCheckQuery, con)
                usernameCheckCmd.Parameters.AddWithValue("@Username", Uname.Text)
                Dim userExists As Integer = Convert.ToInt32(usernameCheckCmd.ExecuteScalar())
                If userExists > 0 Then
                    MsgBox("Username already exists.")
                    Return
                End If
            End Using
        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
            Return
        Finally
            con.Close()
        End Try

        ' Determine gender
        Dim gender As String = If(Male.Checked, "Male", "Female")

        Try
            con.Open()

            ' Handle optional fields: Favorite and Answer
            Dim favorite As Object = If(String.IsNullOrWhiteSpace(cbfav.Text), DBNull.Value, cbfav.SelectedItem.ToString())
            Dim answer As Object = If(String.IsNullOrWhiteSpace(tbanswer.Text), DBNull.Value, tbanswer.Text)

            ' Insert new user
            Using cmd As New MySqlCommand("INSERT INTO usertbl (Firstname, Lastname, Middle_name, User_type, Gender, Username, Password, Con_password, Favorite, Answer) 
                                  VALUES (@Firstname, @Lastname, @Middle_name, @User_type, @Gender, @Username, @Password, @Con_password, @Favorite ,@Answer)", con)
                cmd.Parameters.AddWithValue("@Firstname", Fname.Text)
                cmd.Parameters.AddWithValue("@Lastname", Lname.Text)
                cmd.Parameters.AddWithValue("@Middle_name", Mname.Text)
                cmd.Parameters.AddWithValue("@User_type", CBuser.SelectedItem.ToString())
                cmd.Parameters.AddWithValue("@Gender", gender)
                cmd.Parameters.AddWithValue("@Username", Uname.Text)
                cmd.Parameters.AddWithValue("@Password", Pwd.Text)
                cmd.Parameters.AddWithValue("@Con_password", Cpwd.Text)
                cmd.Parameters.AddWithValue("@Favorite", favorite)
                cmd.Parameters.AddWithValue("@Answer", answer)

                cmd.ExecuteNonQuery() ' Execute the insert command
            End Using

            ' Log the action in the audit trail
            Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) 
                                       VALUES (NOW(), @User_type, @Name, 'Added new user')", con)
                auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' Assuming Label45 contains the user type
                auditCmd.Parameters.AddWithValue("@Name", Label26.Text)    ' Assuming Label26 contains the user name
                auditCmd.ExecuteNonQuery()
            End Using

            MsgBox("User Added Successfully!")
            ' Mask the password and confirm password before adding to DataGridView
            Dim maskedPwd As String = New String("*"c, Pwd.Text.Length)
            Dim maskedCpwd As String = New String("*"c, Cpwd.Text.Length)

            ' Add user to DataGridView
            DataGridView5.Rows.Add(Fname.Text, Lname.Text, Mname.Text, CBuser.SelectedItem.ToString(), gender, Uname.Text, maskedPwd, maskedCpwd)

        Catch ex As MySqlException

        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

        Me.Hide()
        Me.Dispose()
        Dim newForm As New MainMenuf() ' Replace MainMenuf with the name of your new form
        newForm.Panel6.Visible = True ' Make sure Panel6 is visible
        newForm.Show()
        ClearFormFields()
    End Sub

    ' Clear the form fields
    Private Sub ClearFormFields()
        Uname.Text = ""
        Fname.Text = ""
        Lname.Text = ""
        Mname.Text = ""
        CBuser.SelectedItem = Nothing
        Male.Checked = False
        Female.Checked = False
        Pwd.Text = ""
        Cpwd.Text = ""
        Uname1.Text = ""
        Fname1.Text = ""
        Lname1.Text = ""
        Mname1.Text = ""
        tbanswer.Text = ""
        cbfav.SelectedItem = Nothing
        CBuser1.SelectedItem = Nothing
        Male1.Checked = False
        Female1.Checked = False
        Pwd1.Text = ""
        Cpwd1.Text = ""
        Panel7.Visible = False
    End Sub

    Private Sub IconButton29_Click(sender As Object, e As EventArgs) Handles IconButton29.Click
        Panel7.Visible = False
        ClearFormFields()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Dim nextForm As New LoginF()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton6_Click(sender As Object, e As EventArgs)
        Dim nextForm As New InventoryF()

        nextForm.Show()

        Me.Hide()
    End Sub

    Private Sub IconButton6_Click_1(sender As Object, e As EventArgs)
        Dim nextForm As New InventoryF()

        nextForm.Show()

        Me.Hide()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        reset()
        Me.Dispose()
        Dim newForm As New LoginF()
        newForm.Show()
        Me.Hide()

    End Sub

    Private Sub IconButton6_Click_2(sender As Object, e As EventArgs) Handles IconButton6.Click
        Dim nextForm As New InventoryF()

        nextForm.Show()

        Me.Hide()
    End Sub
    Private Sub sm()
        Dim con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")

        Try
            ' Open the connection
            con.Open()



            ' Query for suppliertbl
            Dim supplierQuery As String = "SELECT Supplier_name, Company_name, Address, Contact_number FROM suppliertbl"
            Using supplierCmd As New MySqlCommand(supplierQuery, con)
                ' Execute the query for suppliertbl
                Using dr As MySqlDataReader = supplierCmd.ExecuteReader()
                    While dr.Read()
                        ' Add the supplier details to DataGridView3
                        DataGridView3.Rows.Add(dr.Item("Supplier_name"), dr.Item("Company_name"), dr.Item("Address"), dr.Item("Contact_number"))
                    End While
                End Using
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub
    Private Sub tryd()
        Dim con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")

        Try


            Dim supplierQuery As String = "SELECT Supplier_name, Company_name, Address, Contact_number FROM suppliertbl"
            Using supplierCmd As New MySqlCommand(supplierQuery, con)
                ' Execute the query for suppliertbl
                Using dr As MySqlDataReader = supplierCmd.ExecuteReader()
                    While dr.Read()
                        ' Add the supplier details to DataGridView3
                        DataGridView3.Rows.Add(dr.Item("Supplier_name"), dr.Item("Company_name"), dr.Item("Address"), dr.Item("Contact_number"))
                    End While
                End Using
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub
    Private Sub check()
        Dim con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
        Dim query As String = "SELECT Firstname, Lastname, Username, User_type FROM usertbl"

        Try
            con.Open()
            Dim cmd As New MySqlCommand(query, con)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            ' Clear any previous data in DataGridView5
            DataGridView5.Rows.Clear()

            ' Loop through the data and add it to the DataGridView
            While reader.Read()
                ' Add user to DataGridView
                DataGridView5.Rows.Add(reader("Firstname").ToString(), reader("Lastname").ToString(), reader("Username").ToString(), reader("User_type").ToString())
            End While
        Catch ex As MySqlException
            MsgBox("Error loading data: " & ex.Message)
        Finally
            con.Close()
        End Try

        ' Check if "SuperAdmin" already exists in the DataGridView5
        Dim superAdminExists As Boolean = False
        For Each row As DataGridViewRow In DataGridView5.Rows
            If row.Cells("User_type").Value.ToString() = "SuperAdmin" Then
                superAdminExists = True
                Exit For ' Exit once we find the SuperAdmin
            End If
        Next

        ' If "SuperAdmin" exists, remove it from the ComboBox
        If superAdminExists Then
            If CBuser.Items.Contains("SuperAdmin") Then
                CBuser.Items.Remove("SuperAdmin")
            End If
        Else
            ' Add "SuperAdmin" to ComboBox if not already present
            If Not CBuser.Items.Contains("SuperAdmin") Then
                CBuser.Items.Add("SuperAdmin")
            End If
        End If

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint


    End Sub
    Private Sub pabsd()
        Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

        Using con As New MySqlConnection(connectionString)
            con.Open() ' Open the connection

            ' Define the SQL query to join product and quantity tables
            Dim query As String = "SELECT producttbl.Barcode, Product_Name, Cat_name, Color, ItemSize, Selling_price, Cost_price, Quantity " &
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
                DataGridView10.DataSource = ds.Tables(0)

                ' Loop through each row in DataGridView10 to check the quantity
                For Each row As DataGridViewRow In DataGridView10.Rows
                    Dim quantity As Integer
                    If Integer.TryParse(row.Cells("Quantity").Value?.ToString(), quantity) Then
                        ' If quantity is less than 5, show the message box
                        If quantity < 5 Then
                            MsgBox("You need to restock quantity for product: " & row.Cells("Product_Name").Value.ToString(),
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit For ' Exit the loop after showing the message
                        End If
                    End If
                Next
            Else

            End If
        End Using


    End Sub
    Private Sub cons()
        Dim hasDataRow As Boolean = False

        For Each row As DataGridViewRow In DataGridView1.Rows
            If Not row.IsNewRow Then
                hasDataRow = True
                Exit For
            End If
        Next

        If hasDataRow Then

            TextBox10.Text = ""
            TextBox10.Enabled = True
        Else

            TextBox10.Text = "1"
            TextBox10.Enabled = False
        End If
    End Sub
    Private Sub vats()
        Dim hideCheckBox As Boolean = False

        ' Loop through all rows and check if any value is exactly "00.00"
        For Each row As DataGridViewRow In DataGridView9.Rows
            If Not row.IsNewRow Then
                Dim cellValue As String = row.Cells(0).Value.ToString().Trim()

                If cellValue = "00.00" OrElse cellValue = "0" OrElse cellValue = "0.00" Then
                    hideCheckBox = True
                    Exit For
                End If
            End If
        Next

        If hideCheckBox Then
            CheckBox5.Visible = False
        Else
            CheckBox5.Visible = True
        End If

    End Sub

    Private Sub MainMenuf_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        check()
        pabsd()
        DateTimePicker1.MinDate = New Date(2024, 11, 23)
        DataGridView5.Columns(5).Visible = False
        DataGridView5.Columns(6).Visible = False
        DataGridView5.Columns(7).Visible = False
        DataGridView3.Columns(0).Visible = False
        DataGridView5.Columns(8).Visible = False
        DataGridView5.Columns(9).Visible = False
        Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

        Using con As New MySqlConnection(connectionString)

            Try
                con.Open() ' Open the database connection
                Dim productQuery As String = "SELECT Barcode, Product_Name, Description, Cat_name, Color, ItemSize, Selling_price, Image1 FROM producttbl"
                Using productCmd As New MySqlCommand(productQuery, con)
                    Using drProduct As MySqlDataReader = productCmd.ExecuteReader()
                        DataGridView1.Rows.Clear() ' Clear existing rows in DataGridView1

                        While drProduct.Read()
                            ' Retrieve image byte array from the database
                            If drProduct("Image1") IsNot DBNull.Value Then
                                ' Convert byte array to image if Image1 is not DBNull
                                Dim imageBytes As Byte() = CType(drProduct("Image1"), Byte())
                                Using ms As New MemoryStream(imageBytes)
                                    Dim image As Image = Image.FromStream(ms)

                                    ' Add the row to the DataGridView, including the image
                                    DataGridView1.Rows.Add(
                                drProduct("Barcode"),
                                drProduct("Product_Name"),
                                drProduct("Description"),
                                drProduct("Cat_name"),
                                drProduct("Color"),
                                drProduct("ItemSize"),
                                drProduct("Selling_price"),
                                image ' Use the image loaded from byte array
                            )
                                End Using
                            Else
                                ' If no image is available, you can add a placeholder or leave the image cell empty
                                DataGridView1.Rows.Add(
                            drProduct("Barcode"),
                            drProduct("Product_Name"),
                            drProduct("Description"),
                            drProduct("Cat_name"),
                            drProduct("Color"),
                            drProduct("ItemSize"),
                            drProduct("Selling_price"),
                            Nothing ' No image to display
                        )
                            End If
                        End While
                    End Using
                End Using

                Dim tabled As New DataTable()
                Dim adptd As New MySqlDataAdapter("SELECT * FROM deliveyltbl", con)
                adptd.Fill(tabled)

                DataGridView8.DataSource = tabled

                For Each column As DataGridViewColumn In DataGridView8.Columns
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Next
                ' Format numeric columns
                If DataGridView8.Columns.Contains("Cost_price") Then
                    DataGridView8.Columns("Cost_price").DefaultCellStyle.Format = "N2"
                End If

                If DataGridView8.Columns.Contains("Total_amount") Then
                    DataGridView8.Columns("Total_amount").DefaultCellStyle.Format = "N2"
                End If

                ' Format date with time
                If DataGridView8.Columns.Contains("Date_received") Then
                    DataGridView8.Columns("Date_received").DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss"
                End If

                ' Fetch users and populate DataGridView5
                Dim userQuery As String = "SELECT Firstname, Lastname, Middle_name, User_type, Gender, Username, Password, Con_password, Favorite, Answer FROM usertbl"
                Using cmd As New MySqlCommand(userQuery, con)
                    Using dr As MySqlDataReader = cmd.ExecuteReader()
                        DataGridView5.Rows.Clear() ' Clear existing rows in DataGridView5

                        While dr.Read()
                            ' Mask password and confirm password
                            Dim maskedPassword As String = New String("*"c, dr("Password").ToString().Length)
                            Dim maskedConfirmPassword As String = New String("*"c, dr("Con_password").ToString().Length)

                            ' Add user details to DataGridView5
                            DataGridView5.Rows.Add(
                        dr("Firstname"),
                        dr("Lastname"),
                        dr("Middle_name"),
                        dr("User_type"),
                        dr("Gender"),
                        dr("Username"),
                        maskedPassword,
                        maskedConfirmPassword,
                        dr("Favorite"),
                        dr("Answer")
                    )
                        End While
                    End Using
                End Using

                ' Fetch categories and populate DataGridView2
                Dim categoryQuery As String = "SELECT category_name FROM categorytbl"
                Using categoryCmd As New MySqlCommand(categoryQuery, con)
                    Using dr As MySqlDataReader = categoryCmd.ExecuteReader()
                        DataGridView2.Rows.Clear() ' Clear existing rows in DataGridView2

                        While dr.Read()
                            ' Add category_name to DataGridView2
                            DataGridView2.Rows.Add(dr("category_name"))
                        End While
                    End Using
                End Using
                ' Fetch categories and populate DataGridView2
                Dim ColorQuery As String = "SELECT ItemColor FROM Colortbl"
                Using ColorCmd As New MySqlCommand(ColorQuery, con)
                    Using dr As MySqlDataReader = ColorCmd.ExecuteReader()
                        DataGridView6.Rows.Clear() ' Clear existing rows in DataGridView2

                        While dr.Read()
                            ' Add category_name to DataGridView2
                            DataGridView6.Rows.Add(dr("ItemColor"))
                        End While
                    End Using
                End Using

                Dim ColoreQuery As String = "SELECT Item_Size FROM Sizetbl"
                Using ColoreCmd As New MySqlCommand(ColoreQuery, con)
                    Using dr As MySqlDataReader = ColoreCmd.ExecuteReader()
                        DataGridView7.Rows.Clear() ' Clear existing rows in DataGridView2

                        While dr.Read()
                            ' Add category_name to DataGridView2
                            DataGridView7.Rows.Add(dr("Item_Size"))
                        End While
                    End Using
                End Using
                Dim selectQuery As String = "SELECT VAT FROM vattbl"

                ' Fetch suppliers and populate DataGridView3
                Dim supplierQuery As String = "SELECT SM_ID, Supplier_name, Company_name, Address, Contact_number FROM suppliertbl"
                Using supplierCmd As New MySqlCommand(supplierQuery, con)
                    Using dr As MySqlDataReader = supplierCmd.ExecuteReader()
                        DataGridView3.Rows.Clear() ' Clear existing rows in DataGridView3

                        While dr.Read()
                            ' Add supplier details to DataGridView3
                            DataGridView3.Rows.Add(
                                dr("SM_ID"),
                            dr("Supplier_name"),
                        dr("Company_name"),
                        dr("Address"),
                        dr("Contact_number")
                    )
                        End While
                    End Using
                End Using
                Dim selectQueryv As String = "SELECT VAT FROM vattbl"

                Using cmd As New MySqlCommand(selectQueryv, con)
                    Using drProduct As MySqlDataReader = cmd.ExecuteReader()
                        ' Clear any existing rows in the DataGridView
                        DataGridView9.Rows.Clear()

                        ' Check if there are any rows returned
                        If drProduct.HasRows Then
                            ' Loop through each row
                            While drProduct.Read()
                                ' Retrieve the VAT value from the first column (index 0)
                                Dim VATValue As String = drProduct(0).ToString()

                                ' Add the VAT value to the DataGridView9
                                DataGridView9.Rows.Add(VATValue)
                            End While
                        Else

                        End If
                    End Using
                End Using
                If DataGridView9.Rows.Count > 0 Then
                    IconButton63.Enabled = False ' Assuming IconButtonInsert is your insert button
                Else
                    IconButton63.Enabled = True
                End If
            Catch ex As MySqlException
                MsgBox("Database error: " & ex.Message, MessageBoxIcon.Error)
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message, MessageBoxIcon.Error)
            End Try
        End Using ' Connection automatically closes here



        Label26.Text = username1
        Label45.Text = position
        Label49.Text = names
        If loginAsSuperAdmin Then

        ElseIf loginAsAdmin Then
            IconButton3.Enabled = False
        ElseIf loginAsStaff Then
            IconButton3.Enabled = False
            IconButton11.Enabled = False
            IconButton15.Enabled = False
            IconButton8.Enabled = False
            IconButton5.Enabled = False

        End If

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
            CN.Items.Clear()

            If tableee.Rows.Count > 0 Then
                CN.Text = "" ' Optionally clear the current selected value in the ComboBox
                For Each row As DataRow In tableee.Rows
                    CN.Items.Add(row("Category_name").ToString()) ' Add each category name to the ComboBox
                Next

                ' Optionally select the first item in the ComboBox
                CN.SelectedIndex = 0
            Else
                ' Optionally handle the case where no data is found

            End If

            ' Close the connection
            con.Close()
        Catch ex As MySqlException
            ' Handle MySQL-specific exceptions
            MsgBox("MySQL Error: " & ex.Message)
        Catch ex As Exception
            ' Handle other exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try
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
            CN1.Items.Clear()

            If tableee.Rows.Count > 0 Then
                CN1.Text = "" ' Optionally clear the current selected value in the ComboBox
                For Each row As DataRow In tableee.Rows
                    CN1.Items.Add(row("Category_name").ToString()) ' Add each category name to the ComboBox
                Next

                ' Optionally select the first item in the ComboBox
                CN1.SelectedIndex = 0
            Else
                ' Optionally handle the case where no data is found

            End If

            ' Close the connection
            con.Close()
        Catch ex As MySqlException
            ' Handle MySQL-specific exceptions
            MsgBox("MySQL Error: " & ex.Message)
        Catch ex As Exception
            ' Handle other exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try
        Try
            ' Open the database connection
            opencon()

            ' Create a MySQL command to select all data from Categorytbl
            Dim commandec As New MySqlCommand("SELECT * FROM Colortbl", con)
            Dim adapterrec As New MySqlDataAdapter(commandec)
            Dim tableeec As New DataTable()

            ' Fill the DataTable with data from the Categorytbl
            adapterrec.Fill(tableeec)

            ' Clear existing items in the ComboBox
            COR.Items.Clear()
            COR1.Items.Clear()
            If tableeec.Rows.Count > 0 Then
                COR1.Text = "" ' Optionally clear the current selected value in the ComboBox
                For Each row As DataRow In tableeec.Rows
                    COR1.Items.Add(row("ItemColor").ToString()) ' Add each category name to the ComboBox
                Next

                ' Optionally select the first item in the ComboBox
                COR1.SelectedIndex = 0
            Else
                ' Optionally handle the case where no data is found

            End If

            If tableeec.Rows.Count > 0 Then
                COR.Text = "" ' Optionally clear the current selected value in the ComboBox
                For Each row As DataRow In tableeec.Rows
                    COR.Items.Add(row("ItemColor").ToString()) ' Add each category name to the ComboBox
                Next

                ' Optionally select the first item in the ComboBox
                COR.SelectedIndex = 0
            Else
                ' Optionally handle the case where no data is found

            End If


            ' Close the connection
            con.Close()
        Catch ex As MySqlException
            ' Handle MySQL-specific exceptions
            MsgBox("MySQL Error: " & ex.Message)
        Catch ex As Exception
            ' Handle other exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try

        Try
            ' Open the database connection
            opencon()

            ' Create a MySQL command to select all data from Categorytbl
            Dim commandes As New MySqlCommand("SELECT * FROM Sizetbl", con)
            Dim adapterres As New MySqlDataAdapter(commandes)
            Dim tableees As New DataTable()

            ' Fill the DataTable with data from the Categorytbl
            adapterres.Fill(tableees)

            ' Clear existing items in the ComboBox
            Size.Items.Clear()
            Size.Items.Clear()
            If tableees.Rows.Count > 0 Then
                Size.Text = "" ' Optionally clear the current selected value in the ComboBox
                For Each row As DataRow In tableees.Rows
                    Size.Items.Add(row("Item_Size").ToString()) ' Add each category name to the ComboBox
                Next

                ' Optionally select the first item in the ComboBox
                Size.SelectedIndex = 0
            Else
                ' Optionally handle the case where no data is found

            End If

            If tableees.Rows.Count > 0 Then
                Size1.Text = "" ' Optionally clear the current selected value in the ComboBox
                For Each row As DataRow In tableees.Rows
                    Size1.Items.Add(row("Item_Size").ToString()) ' Add each category name to the ComboBox
                Next

                ' Optionally select the first item in the ComboBox
                Size1.SelectedIndex = 0
            Else
                ' Optionally handle the case where no data is found

            End If


            ' Close the connection
            con.Close()
        Catch ex As MySqlException
            ' Handle MySQL-specific exceptions
            MsgBox("MySQL Error: " & ex.Message)
        Catch ex As Exception
            ' Handle other exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub
    Private Sub vatts()
        Dim selectQueryv As String = "SELECT VAT FROM vattbl"

        Try
            con.Open() ' OPEN the connection before using it

            Using cmd As New MySqlCommand(selectQueryv, con)
                Using drProduct As MySqlDataReader = cmd.ExecuteReader()
                    ' Clear any existing rows in the DataGridView
                    DataGridView9.Rows.Clear()

                    ' Check if there are any rows returned
                    If drProduct.HasRows Then
                        ' Loop through each row
                        While drProduct.Read()
                            ' Retrieve the VAT value from the first column (index 0)
                            Dim VATValue As String = drProduct(0).ToString()

                            ' Add the VAT value to the DataGridView9
                            DataGridView9.Rows.Add(VATValue)
                        End While
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading VAT: " & ex.Message)

        Finally
            con.Close() ' ALWAYS close the connection
        End Try
    End Sub


    Private Sub IconButton5_Click(sender As Object, e As EventArgs) Handles IconButton5.Click
        vats()
        Panel12.Visible = True

    End Sub

    Private Sub IconButton10_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub IconButton8_Click(sender As Object, e As EventArgs) Handles IconButton8.Click
        Panel13.Visible = True
    End Sub
    Private Sub DataGridView5_SelectionChanged_1(sender As Object, e As EventArgs)
        If DataGridView5.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView5.SelectedRows(0)

            ' Assuming you have text boxes named Fname, Lname, Uname, etc.
            Fname.Text = selectedRow.Cells("Firstname").Value.ToString()
            Lname.Text = selectedRow.Cells("Lastname").Value.ToString()
            Uname.Text = selectedRow.Cells("Username").Value.ToString()
            Mname.Text = selectedRow.Cells("Middle_name").Value.ToString()
            Pwd.Text = selectedRow.Cells("Password").Value.ToString()
            Cpwd.Text = selectedRow.Cells("Con_password").Value.ToString()
            ' Populate other controls as needed
            If selectedRow.Cells("Gender").Value.ToString() = "Male" Then
                Male.Checked = True
            Else
                Female.Checked = True
            End If

            ' Handle User Type if needed
            CBuser.SelectedItem = selectedRow.Cells("User_type").Value.ToString()
        End If
    End Sub

    Private Sub IconButton34_Click(sender As Object, e As EventArgs)




    End Sub

    Private Sub IconButton33_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub IconButton32_Click(sender As Object, e As EventArgs) Handles IconButton32.Click
        Panel12.Visible = False
    End Sub

    Private Sub IconButton37_Click(sender As Object, e As EventArgs) Handles IconButton37.Click
        Dim nextForm As New Salesreptbl()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton36_Click(sender As Object, e As EventArgs) Handles IconButton36.Click
        Dim nextForm As New Stockreptbl()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton38_Click(sender As Object, e As EventArgs) Handles IconButton38.Click
        Panel13.Visible = False
    End Sub

    Private Sub IconButton35_Click(sender As Object, e As EventArgs) Handles IconButton35.Click
        Dim nextForm As New AuditF()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub Label48_Click(sender As Object, e As EventArgs) Handles Label48.Click

    End Sub

    Private Sub IconButton42_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub IconButton43_Click(sender As Object, e As EventArgs)
        ' Create a BarcodeWriter instance

    End Sub

    Private Sub Barc_TextChanged(sender As Object, e As EventArgs) Handles Barc.TextChanged

        Try
            ' Create a new BarcodeWriter instance
            Dim writer As New BarcodeWriter
            writer.Format = BarcodeFormat.CODE_128
            PictureBox4.Image = writer.Write(Barc.Text)

        Catch ex As Exception
            ' Handle errors gracefully
            MessageBox.Show($"An error occurred while generating the barcode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
        ' Check if the clicked cell is part of the Delete button column
        If e.ColumnIndex >= 0 AndAlso DataGridView2.Columns(e.ColumnIndex).Name = "Delete" Then
            ' Confirm the deletion action
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this category?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                Try
                    ' Get the category name to delete
                    Dim categoryNameToDelete As String = DataGridView2.Rows(e.RowIndex).Cells("Category_name").Value.ToString()

                    ' Check if the category is in use in any products
                    Dim isCategoryInUse As Boolean = False
                    If con.State <> ConnectionState.Open Then
                        con.Open()
                    End If

                    ' Query to check if the category is being used by any products
                    Using cmd As New MySqlCommand("SELECT COUNT(*) FROM producttbl WHERE cat_name = @Cat_name", con)
                        cmd.Parameters.AddWithValue("@Cat_name", categoryNameToDelete)
                        Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                        If count > 0 Then
                            ' Category is in use, show a message and cancel deletion
                            MessageBox.Show("This category is currently being used by one or more products and cannot be deleted.", "Deletion Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit Sub
                        End If
                    End Using

                    ' Proceed with deleting the category since it's not in use
                    Using cmd As New MySqlCommand("DELETE FROM categorytbl WHERE category_name = @CategoryName", con)
                        cmd.Parameters.AddWithValue("@CategoryName", categoryNameToDelete)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Remove the row from DataGridView
                    DataGridView2.Rows.RemoveAt(e.RowIndex)

                    MessageBox.Show("Category successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    ' Close connection if it's still open
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End If
        End If



        ' Check if the clicked column is the "Update" button column (replace "update" with the actual name of your column)
        If e.ColumnIndex >= 0 AndAlso DataGridView2.Columns(e.ColumnIndex).Name = "Update" Then
            ' Hide the other panel (Panel17) to make space for the update panel (Panel10)
            Panel17.Visible = False

            ' Retrieve the clicked row from DataGridView2
            Dim selectedRow As DataGridViewRow = DataGridView2.Rows(e.RowIndex)

            ' Get the category name (or any other data from the row)
            Dim categoryName As String = selectedRow.Cells("Category_name").Value.ToString()

            ' Check if the category name is in use in producttbl (or any other related table)
            Dim isCategoryInUse As Boolean = False
            Try
                ' Open the database connection
                Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                    con.Open()

                    ' Query to check if the category name is used in producttbl (you can change the query as needed)
                    Dim checkQuery As String = "SELECT COUNT(*) FROM producttbl WHERE Cat_name = @Cat_name"
                    Using checkCmd As New MySqlCommand(checkQuery, con)
                        checkCmd.Parameters.AddWithValue("@Cat_name", categoryName)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If count > 0 Then
                            ' Category is in use, show the warning message and prevent the update
                            MessageBox.Show("This category is currently in use and cannot be updated.",
                                            "Update Forbidden",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning)

                            ' Exit the subroutine to prevent further execution
                            Exit Sub
                        End If
                    End Using
                End Using
            Catch ex As MySqlException
                ' Handle MySQL-specific exceptions
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Catch ex As Exception
                ' Handle other exceptions
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

            ' Show Panel10 for updating
            Panel10.Visible = True

            ' Populate the TextBox with the category name for editing
            TextBox1.Text = categoryName
        End If


    End Sub

    Private Sub IconButton9_Click_1(sender As Object, e As EventArgs) Handles IconButton9.Click
        Panel10.Visible = False
    End Sub

    Private Sub IconButton25_Click(sender As Object, e As EventArgs) Handles IconButton25.Click
        Panel10.Visible = False
        Panel17.Visible = True
    End Sub

    Private Sub IconButton23_Click_1(sender As Object, e As EventArgs) Handles IconButton23.Click
        Panel17.Visible = False
    End Sub

    Private Sub IconButton24_Click_1(sender As Object, e As EventArgs) Handles IconButton24.Click
        For Each row As DataGridViewRow In DataGridView2.Rows
            If row.Cells(0).Value.ToString() = TextBox2.Text Then
                MsgBox("Category Name Already Exists")
                Exit Sub
            End If
        Next

        ' Ensure the category name is not empty
        If String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MsgBox("MISSING INPUT")
            Exit Sub
        End If

        Try
            ' Open the database connection if not already open
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If

            ' INSERT new category
            Using cmd As New MySqlCommand("INSERT INTO categorytbl (`category_name`) VALUES (@category_name)", con)
                cmd.Parameters.AddWithValue("@category_name", TextBox2.Text)
                cmd.ExecuteNonQuery()

                ' Log the action in the audit trail
                Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, 'Added new category')", con)
                    auditCmd.Parameters.AddWithValue("@User_type", Label45.Text)
                    auditCmd.Parameters.AddWithValue("@Name", Label26.Text)
                    auditCmd.ExecuteNonQuery()
                End Using

                MsgBox("Category Successfully Added!")
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
        Me.Hide()

        ' Dispose the current form (optional)
        Me.Dispose()

        ' Create a new instance of the form
        Dim newForm As New MainMenuf()  ' Replace Form1 with the actual name of your form
        newForm.Panel3.Visible = True

        ' Show the new instance of the form
        newForm.Show()
    End Sub
    Private Sub DataGridView3_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView3.CellContentClick
        If e.ColumnIndex = DataGridView3.Columns("SMdelete").Index Then
            ' Confirm before deleting
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this supplier?", "Delete Supplier", MessageBoxButtons.YesNo)

            If result = DialogResult.Yes Then
                Try
                    ' Get the Supplier name from the row (or you can use an ID if available)
                    Dim supplierName As String = DataGridView3.Rows(e.RowIndex).Cells("Supplier_name").Value.ToString()

                    ' Open database connection
                    If con.State <> ConnectionState.Open Then
                        con.Open()
                    End If

                    ' Create a DELETE command to remove the supplier from the database
                    Dim cmd As New MySqlCommand("DELETE FROM suppliertbl WHERE Supplier_name = @Supplier_name", con)
                    cmd.Parameters.AddWithValue("@Supplier_name", supplierName)
                    cmd.ExecuteNonQuery()

                    ' Remove the row from the DataGridView
                    DataGridView3.Rows.RemoveAt(e.RowIndex)

                    ' Optionally log the deletion in the audit trail
                    Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, 'Deleted supplier: " & supplierName & "')", con)
                        auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' Assuming Label45 contains the user type
                        auditCmd.Parameters.AddWithValue("@Name", Label26.Text)    ' Assuming Label26 contains the user name
                        auditCmd.ExecuteNonQuery()
                    End Using

                    MsgBox("Supplier Deleted Successfully!")

                Catch ex As MySqlException
                    MsgBox("Database error: " & ex.Message)
                Catch ex As Exception
                    MsgBox("An error occurred: " & ex.Message)
                Finally
                    ' Close the connection if it's open
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End If
        End If

        If e.ColumnIndex >= 0 AndAlso DataGridView3.Columns(e.ColumnIndex).Name = "SMupdate" Then
            Panel7.Visible = False
            Dim selectedRow As DataGridViewRow = DataGridView3.Rows(e.RowIndex)

            ' Retrieve values from the selected row
            Dim supplierName As String = selectedRow.Cells("Supplier_name").Value.ToString()
            Dim companyName As String = selectedRow.Cells("Company_name").Value.ToString()
            Dim contactNumber As String = selectedRow.Cells("Contact_number").Value.ToString()
            Dim address As String = selectedRow.Cells("Address").Value.ToString()
            Dim smID As String = selectedRow.Cells("SM_ID").Value.ToString() ' Retrieve SM_ID from the selected row

            Panel8.Visible = True

            ' Populate the textboxes with the current supplier's details
            SN.Text = supplierName
            CNAME.Text = companyName
            CNUM.Text = contactNumber
            CADD.Text = address
            TextBox7.Text = smID ' Set the SM_ID in TextBox7
        End If

    End Sub

    Private Sub IconButton46_Click(sender As Object, e As EventArgs) Handles IconButton46.Click
        Panel20.Visible = False
    End Sub

    Private Sub IconButton13_Click_1(sender As Object, e As EventArgs) Handles IconButton13.Click
        Panel8.Visible = False
    End Sub

    Private Sub IconButton47_Click(sender As Object, e As EventArgs) Handles IconButton47.Click
        If String.IsNullOrWhiteSpace(SN1.Text) OrElse
   String.IsNullOrWhiteSpace(CNAME1.Text) OrElse
   String.IsNullOrWhiteSpace(CNUM1.Text) OrElse
   String.IsNullOrWhiteSpace(CADD1.Text) Then
            MsgBox("MISSING INPUT")
            Return
        End If

        ' Validate that the contact number (CNUM1.Text) is exactly 11 digits long
        If CNUM1.Text.Length <> 11 OrElse Not CNUM1.Text.All(AddressOf Char.IsDigit) Then
            MsgBox("Contact number must be exactly 11 digits.")
            Return
        End If

        For Each row As DataGridViewRow In DataGridView3.Rows
            If row.IsNewRow Then Continue For

            Dim existingCompany As String = row.Cells("Company_name").Value.ToString().Trim()
            Dim existingAddress As String = row.Cells("Address").Value.ToString().Trim()
            Dim existingContact As String = row.Cells("Contact_number").Value.ToString().Trim()

            If existingCompany.Equals(CNAME1.Text.Trim(), StringComparison.OrdinalIgnoreCase) Then
                MsgBox("This company name already exists in the list.")
                Return
            End If

            If existingAddress.Equals(CADD1.Text.Trim(), StringComparison.OrdinalIgnoreCase) Then
                MsgBox("This address already exists in the list.")
                Return
            End If

            If existingContact.Equals(CNUM1.Text.Trim(), StringComparison.OrdinalIgnoreCase) Then
                MsgBox("This contact number already exists in the list.")
                Return
            End If
        Next
        Try
            ' Open the database connection if not already open
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If

            ' Check if the address already exists in the database
            Dim addressCheckQuery As String = "SELECT COUNT(*) FROM suppliertbl WHERE Address = @Address"
            Using checkCmd As New MySqlCommand(addressCheckQuery, con)
                checkCmd.Parameters.AddWithValue("@Address", CADD1.Text)
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                If count > 0 Then
                    ' If the address exists, show a message and return
                    MsgBox("This address already exists in the database!")
                    Return
                End If
            End Using

            ' Insert the new supplier into the database without the SM_ID (auto-increment will take care of it)
            Using cmd As New MySqlCommand("INSERT INTO suppliertbl (Supplier_name, Company_name, Contact_number, Address) VALUES (@Supplier_name, @Company_name, @Contact_number, @Address)", con)
                cmd.Parameters.AddWithValue("@Supplier_name", SN1.Text)
                cmd.Parameters.AddWithValue("@Company_name", CNAME1.Text)
                cmd.Parameters.AddWithValue("@Contact_number", CNUM1.Text)
                cmd.Parameters.AddWithValue("@Address", CADD1.Text)
                cmd.ExecuteNonQuery()

                ' Log the action in the audit trail
                Using auditCmd As New MySqlCommand("INSERT INTO audittbl (DateTime, User_type, Name, Action) VALUES (NOW(), @User_type, @Name, 'Added new supplier')", con)
                    auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' Assuming Label45 contains the user type
                    auditCmd.Parameters.AddWithValue("@Name", Label26.Text)    ' Assuming Label26 contains the user name
                    auditCmd.ExecuteNonQuery()
                End Using

                MsgBox("Supplier Added Successfully!")
            End Using

            ' Refresh or update the DataGridView
            ' After inserting, retrieve the latest data for the DataGridView (including the auto-generated SM_ID)
            Dim getNewSupplierQuery As String = "SELECT SM_ID, Supplier_name, Company_name, Address, Contact_number FROM suppliertbl WHERE Supplier_name = @Supplier_name"
            Using getNewSupplierCmd As New MySqlCommand(getNewSupplierQuery, con)
                getNewSupplierCmd.Parameters.AddWithValue("@Supplier_name", SN1.Text)
                Using reader As MySqlDataReader = getNewSupplierCmd.ExecuteReader()
                    If reader.Read() Then
                        ' Adding the new supplier's details into the DataGridView
                        DataGridView3.Rows.Add(reader("SM_ID"), reader("Supplier_name"), reader("Company_name"), reader("Address"), reader("Contact_number"))
                    End If
                End Using
            End Using

        Catch ex As MySqlException
            ' Handle MySQL-specific errors
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            ' Handle any other errors
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try


        Panel20.Visible = False
        CADD1.Text = ""
        SN1.Text = ""
        CNUM1.Text = ""
        CNAME1.Text = ""
    End Sub

    Private Sub DataGridView5_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView5.CellContentClick

        If e.RowIndex >= 0 AndAlso DataGridView5.Columns(e.ColumnIndex).Name = "UMdelete" Then
            ' Get the User_type from the selected row
            Dim userType As String = DataGridView5.Rows(e.RowIndex).Cells("User_type").Value.ToString()

            ' Prevent deletion if the user is "SuperAdmin"
            If userType = "SuperAdmin" Then
                MessageBox.Show("You cannot delete a SuperAdmin user!", "Delete Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return ' Exit the method to prevent deletion
            End If

            ' Confirm deletion
            Dim result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                Try
                    ' Get the Username from the selected row
                    Dim username As String = DataGridView5.Rows(e.RowIndex).Cells("Username").Value.ToString()

                    ' Delete from the database
                    Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"
                    Using con As New MySqlConnection(connectionString)
                        con.Open()
                        Using deleteCmd As New MySqlCommand("DELETE FROM usertbl WHERE Username = @Username", con)
                            deleteCmd.Parameters.AddWithValue("@Username", username)
                            deleteCmd.ExecuteNonQuery()
                        End Using
                    End Using

                    ' Remove the row from the DataGridView
                    DataGridView5.Rows.RemoveAt(e.RowIndex)

                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As MySqlException
                    MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End If
        ' Check if the clicked cell is in the "UMupdate" column (update button column)
        ' Check if the clicked cell is in the "UMupdate" column (update button column)
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            If DataGridView5.Columns(e.ColumnIndex).Name = "UMupdate" Then
                ' Hide the panel for other purposes
                Panel20.Visible = False

                ' Ensure the selected row is valid
                Dim selectedRow As DataGridViewRow = DataGridView5.Rows(e.RowIndex)

                ' Populate fields for update
                Fname1.Text = selectedRow.Cells("Firstname").Value?.ToString()
                Lname1.Text = selectedRow.Cells("Lastname").Value?.ToString()
                Mname1.Text = selectedRow.Cells("Middle_name").Value?.ToString()
                Uname1.Text = selectedRow.Cells("Username").Value?.ToString()

                ' Set gender radio buttons
                Dim gender As String = selectedRow.Cells("Gender").Value?.ToString()
                If gender = "Male" Then
                    Male1.Checked = True
                ElseIf gender = "Female" Then
                    Female1.Checked = True
                End If


                Dim currentUserType As String = selectedRow.Cells("User_type").Value?.ToString()

                ' Clear current items in ComboBox
                CBuser1.Items.Clear()

                ' Handle ComboBox items based on the User_type
                If currentUserType = "SuperAdmin" Then
                    ' If User_type is SuperAdmin, only show SuperAdmin in ComboBox
                    CBuser1.Items.Add("SuperAdmin")
                    CBuser1.SelectedItem = "SuperAdmin"
                ElseIf currentUserType = "Staff" Then
                    ' If User_type is Staff, allow selecting Admin or Staff
                    CBuser1.Items.Add("Admin")
                    CBuser1.Items.Add("Staff")
                    CBuser1.SelectedItem = "Staff"
                ElseIf currentUserType = "Admin" Then
                    ' If User_type is Admin, allow selecting Admin or Staff
                    CBuser1.Items.Add("Admin")
                    CBuser1.Items.Add("Staff")
                    CBuser1.SelectedItem = "Admin"
                End If

                ' Show the update panel
                Panel23.Visible = True

                ' Set focus on the first field
                Fname1.Focus()
            End If
        End If



    End Sub






    Private Sub Panel23_Paint(sender As Object, e As PaintEventArgs) Handles Panel23.Paint


    End Sub
    Private Sub DataGridView5_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView5.SelectionChanged
        If DataGridView5.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView5.SelectedRows(0)

            ' Populate the fields with the selected user's details
            Fname1.Text = selectedRow.Cells("Firstname").Value?.ToString()
            Lname1.Text = selectedRow.Cells("Lastname").Value?.ToString()
            Mname1.Text = selectedRow.Cells("Middle_name").Value?.ToString()
            Uname1.Text = selectedRow.Cells("Username").Value?.ToString()

            ' Set the gender radio buttons based on the selected value
            Dim gender As String = selectedRow.Cells("Gender").Value?.ToString()
            If gender = "Male" Then
                Male1.Checked = True
            ElseIf gender = "Female" Then
                Female1.Checked = True
            End If

            ' Populate the password fields (assuming "Password" and "Con_password" columns exist)
            Dim password As String = selectedRow.Cells("Password").Value?.ToString()
            Dim confirmPassword As String = selectedRow.Cells("Con_password").Value?.ToString()

            ' Optionally mask the passwords (display as asterisks)
            Pwd1.Text = password  ' You can mask this if needed
            Cpwd1.Text = confirmPassword  ' You can mask this if needed

            ' Populate the Favorite and Answer fields
            cbfav1.SelectedItem = selectedRow.Cells("Favorite").Value?.ToString()  ' Assuming this is a ComboBox
            tbanswer1.Text = selectedRow.Cells("Answer").Value?.ToString()         ' Assuming this is a TextBox

            ' Get the current User_type from the selected row
            Dim currentUserType As String = selectedRow.Cells("User_type").Value?.ToString()

            ' Clear current items in ComboBox
            CBuser1.Items.Clear()

            ' Handle ComboBox items based on the User_type
            If currentUserType = "SuperAdmin" Then
                ' If User_type is SuperAdmin, only show SuperAdmin in ComboBox
                CBuser1.Items.Add("SuperAdmin")
                CBuser1.SelectedItem = "SuperAdmin" ' Ensure SuperAdmin is selected
            ElseIf currentUserType = "Staff" Then
                ' If User_type is Staff, allow selecting Admin or Staff
                CBuser1.Items.Add("Admin")
                CBuser1.Items.Add("Staff")
                CBuser1.SelectedItem = "Staff" ' Ensure Staff is selected
            ElseIf currentUserType = "Admin" Then
                ' If User_type is Admin, allow selecting Admin or Staff
                CBuser1.Items.Add("Admin")
                CBuser1.Items.Add("Staff")
                CBuser1.SelectedItem = "Admin" ' Ensure Admin is selected
            End If
        End If



    End Sub

    Private Sub IconButton48_Click(sender As Object, e As EventArgs) Handles IconButton48.Click
        If String.IsNullOrWhiteSpace(Uname1.Text) OrElse
     String.IsNullOrWhiteSpace(Fname1.Text) OrElse
     String.IsNullOrWhiteSpace(Lname1.Text) OrElse
     String.IsNullOrWhiteSpace(Pwd1.Text) OrElse
     String.IsNullOrWhiteSpace(Cpwd1.Text) Then
            MsgBox("Please fill in all required fields.")
            Return
        End If

        ' Validate password length and match
        If Pwd1.Text.Length < 7 Then
            MsgBox("Password must be at least 7 characters.")
            Return
        End If

        If Pwd1.Text <> Cpwd1.Text Then
            MessageBox.Show("Password and Confirm Password do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Cpwd1.Focus()
            Return
        End If

        Try
            ' Open the database connection if not already open
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If

            ' Prepare the update query to update the user in the database
            Dim query As String = "UPDATE usertbl SET Firstname = @Firstname, Lastname = @Lastname, Middle_name = @Middle_name, " &
                                  "User_type = @User_type, Gender = @Gender, Password = @Password, Con_password = @ConPassword" &
                                  If(Not String.IsNullOrWhiteSpace(cbfav1.Text), ", Favorite = @Favorite", "") &
                                  If(Not String.IsNullOrWhiteSpace(tbanswer1.Text), ", Answer = @Answer", "") &
                                  " WHERE Username = @OriginalUsername"

            ' Prepare the command with the query
            Dim cmd As New MySqlCommand(query, con)

            ' Add parameters for the SQL query
            cmd.Parameters.AddWithValue("@Firstname", Fname1.Text)
            cmd.Parameters.AddWithValue("@Lastname", Lname1.Text)
            cmd.Parameters.AddWithValue("@Middle_name", Mname1.Text)
            cmd.Parameters.AddWithValue("@User_type", CBuser1.SelectedItem.ToString())
            cmd.Parameters.AddWithValue("@Gender", If(Male1.Checked, "Male", "Female"))
            cmd.Parameters.AddWithValue("@Password", Pwd1.Text) ' Use the raw password text here
            cmd.Parameters.AddWithValue("@ConPassword", Cpwd1.Text) ' Use the raw confirm password text here
            cmd.Parameters.AddWithValue("@OriginalUsername", Uname1.Text)

            ' Optional parameters
            If Not String.IsNullOrWhiteSpace(cbfav1.Text) Then
                cmd.Parameters.AddWithValue("@Favorite", cbfav1.Text)
            End If
            If Not String.IsNullOrWhiteSpace(tbanswer1.Text) Then
                cmd.Parameters.AddWithValue("@Answer", tbanswer1.Text)
            End If

            ' Execute the update command
            cmd.ExecuteNonQuery()

            ' Log the action in the audit trail
            Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) " &
                                               "VALUES (NOW(), @User_type, @Name, CONCAT('Updated user: ', @Username))", con)
                auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' Assuming Label45 contains the user type
                auditCmd.Parameters.AddWithValue("@Name", Label26.Text)     ' Assuming Label26 contains the user name
                auditCmd.Parameters.AddWithValue("@Username", Uname1.Text) ' Username being updated
                auditCmd.ExecuteNonQuery()
            End Using

            MsgBox("User updated successfully.")

            ' Manually update the DataGridView (replace old row with new data)
            For Each row As DataGridViewRow In DataGridView5.Rows
                If row.Cells("Username").Value.ToString() = Uname1.Text Then
                    row.Cells("Firstname").Value = Fname1.Text
                    row.Cells("Lastname").Value = Lname1.Text
                    row.Cells("Middle_name").Value = Mname1.Text
                    row.Cells("User_type").Value = CBuser1.SelectedItem?.ToString()
                    row.Cells("Gender").Value = If(Male1.Checked, "Male", "Female")

                    ' Mask the password and confirm password with the correct length for display
                    Dim maskedPwd As String = New String("*"c, Pwd1.Text.Length)
                    Dim maskedCpwd As String = New String("*"c, Cpwd1.Text.Length)

                    row.Cells("Password").Value = maskedPwd
                    row.Cells("Con_password").Value = maskedCpwd

                    ' Update the Favorite and Answer columns in DataGridView if applicable
                    If cbfav1.SelectedItem IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(tbanswer1.Text) Then
                        row.Cells("Favorite").Value = cbfav1.SelectedItem.ToString()
                        row.Cells("Answer").Value = tbanswer1.Text
                    End If

                    Exit For
                End If
            Next

            ' Hide the panel after update
            Panel23.Visible = False  ' Hide the panel after the update

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try





    End Sub


    Private Sub IconButton16_Click(sender As Object, e As EventArgs) Handles IconButton16.Click
        Panel23.Visible = False
    End Sub

    Private Sub IconButton14_Click(sender As Object, e As EventArgs) Handles IconButton14.Click
        Panel23.Visible = False
        Panel7.Visible = True
    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs) Handles Panel4.Paint

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        ' Check if the Delete button was clicked
        If e.ColumnIndex = DataGridView1.Columns("PMdelete").Index AndAlso e.RowIndex >= 0 Then
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this row?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                ' Get the primary key or unique identifier for the row
                Dim barcode As String = DataGridView1.Rows(e.RowIndex).Cells("Barcode").Value.ToString()
                Dim productName As String = DataGridView1.Rows(e.RowIndex).Cells("Product_Name").Value.ToString()

                ' Check if the barcode exists in quantitytbl
                Try
                    Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                        con.Open()

                        ' Query to check if there is any quantity record for the product
                        Dim checkQuantityQuery As String = "SELECT COUNT(*) FROM quantitytbl1 WHERE Barcode = @Barcode"
                        Using cmd As New MySqlCommand(checkQuantityQuery, con)
                            cmd.Parameters.AddWithValue("@Barcode", barcode)

                            Dim quantityCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                            ' If there is a quantity record, prevent deletion
                            If quantityCount > 0 Then
                                MessageBox.Show("Product cannot be deleted because it has existing quantity records.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return ' Exit if the product cannot be deleted
                            End If
                        End Using

                        ' Begin a transaction for atomicity
                        Using transaction = con.BeginTransaction()
                            ' Delete query
                            Dim deleteQuery As String = "DELETE FROM producttbl WHERE Barcode = @Barcode"
                            Using deleteCmd As New MySqlCommand(deleteQuery, con, transaction)
                                deleteCmd.Parameters.AddWithValue("@Barcode", barcode)
                                deleteCmd.ExecuteNonQuery()
                            End Using

                            ' Insert into audit trail
                            Dim auditQuery As String = "INSERT INTO audittbl (DateTime, User_type, Name, Action) VALUES (NOW(), @User_type, @Name, @Action)"
                            Using auditCmd As New MySqlCommand(auditQuery, con, transaction)
                                auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' Assuming Label45 contains the user type
                                auditCmd.Parameters.AddWithValue("@Name", Label26.Text)    ' Assuming Label26 contains the username
                                auditCmd.Parameters.AddWithValue("@Action", $"Deleted product: {productName} (Barcode: {barcode})")
                                auditCmd.ExecuteNonQuery()
                            End Using

                            ' Commit the transaction
                            transaction.Commit()
                        End Using
                    End Using

                    ' Remove the row from the DataGridView
                    DataGridView1.Rows.RemoveAt(e.RowIndex)


                Catch ex As MySqlException
                    MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End If


        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            ' Check if the clicked column is the "PMupdate" column
            If DataGridView1.Columns(e.ColumnIndex).Name = "PMupdate" Then
                Panel26.Visible = False
                ' Retrieve the selected row
                Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                ' Populate the fields with the selected row's values
                Barc.Text = selectedRow.Cells("Barcode").Value?.ToString()
                PN.Text = selectedRow.Cells("Product_Name").Value?.ToString()
                Desc.Text = selectedRow.Cells("Description").Value?.ToString()
                CN.Text = selectedRow.Cells("Cat_name").Value?.ToString()

                ' Debugging: Check the values in the selected row
                Dim itemSize As String = selectedRow.Cells("ItemSize").Value?.ToString()
                Dim color As String = selectedRow.Cells("Color").Value?.ToString()

                Debug.WriteLine("ItemSize: " & itemSize)  ' Output to debug
                Debug.WriteLine("Color: " & color)      ' Output to debug

                ' Check if ItemSize exists in the ComboBox and set it as the selected item
                If Size.Items.Contains(itemSize) Then
                    Size.SelectedItem = itemSize
                Else
                    Debug.WriteLine("ItemSize not found in ComboBox")  ' Output to debug
                End If

                ' Check if Color exists in the ComboBox and set it as the selected item
                If COR.Items.Contains(color) Then
                    COR.SelectedItem = color
                Else
                    Debug.WriteLine("Color not found in ComboBox")  ' Output to debug
                End If

                SP.Text = selectedRow.Cells("Selling_price").Value?.ToString()

                ' Check if the row contains image data and load it into PictureBox3
                If selectedRow.Cells("Image1").Value IsNot DBNull.Value Then
                    ' Retrieve the image directly
                    PictureBox3.Image = CType(selectedRow.Cells("Image1").Value, Image)
                Else
                    ' If no image is available, clear the PictureBox
                    PictureBox3.Image = Nothing
                End If

                ' Display the update panel or section (if applicable)
                Panel9.Visible = True

                ' Set focus on the first field
                Barc.Focus()
            End If
        End If



    End Sub


    Private Sub IconButton27_Click(sender As Object, e As EventArgs) Handles IconButton27.Click
        Try
            ' Validate required fields
            If String.IsNullOrWhiteSpace(Barc.Text) OrElse
               String.IsNullOrWhiteSpace(PN.Text) OrElse
               String.IsNullOrWhiteSpace(Desc.Text) OrElse
               String.IsNullOrWhiteSpace(CN.Text) OrElse
               Size.SelectedIndex = -1 OrElse
               COR.SelectedIndex = -1 OrElse
               String.IsNullOrWhiteSpace(SP.Text) Then
                MsgBox("Please fill in all fields.", MessageBoxIcon.Warning)
                Return
            End If

            ' Validate selling price
            Dim sellingPrice As Decimal
            If Not Decimal.TryParse(SP.Text, sellingPrice) Then
                MsgBox("Invalid selling price. Please enter a valid number.", MessageBoxIcon.Warning)
                Return
            End If

            ' Get selected ComboBox values
            Dim selectedSize As String = Size.SelectedItem?.ToString()
            Dim selectedColor As String = COR.SelectedItem?.ToString()

            ' Convert image to byte array if it's set in PictureBox3
            Dim imageBytes As Byte() = Nothing
            If PictureBox3.Image IsNot Nothing Then
                Using ms As New MemoryStream()
                    PictureBox3.Image.Save(ms, PictureBox3.Image.RawFormat)  ' Save the image into memory stream
                    imageBytes = ms.ToArray()  ' Convert it to byte array
                End Using
            End If

            ' Database update logic
            Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co"),
                  cmd As New MySqlCommand("UPDATE producttbl 
                                   SET Product_Name = @Product_Name, 
                                       Description = @Description, 
                                       Cat_name = @Cat_name, 
                                       ItemSize = @ItemSize, 
                                       Selling_price = @Selling_price, 
                                       Color = @Color, 
                                       Image1 = @Image1
                                   WHERE Barcode = @Barcode", con)
                con.Open()

                ' Add parameters to the SQL command
                cmd.Parameters.AddWithValue("@Barcode", Barc.Text.Trim())
                cmd.Parameters.AddWithValue("@Product_Name", PN.Text.Trim())
                cmd.Parameters.AddWithValue("@Description", Desc.Text.Trim())
                cmd.Parameters.AddWithValue("@Cat_name", CN.Text.Trim())
                cmd.Parameters.AddWithValue("@ItemSize", selectedSize)
                cmd.Parameters.AddWithValue("@Selling_price", sellingPrice)
                cmd.Parameters.AddWithValue("@Color", selectedColor)

                ' Add the image as a parameter (it could be DBNull if no image)
                If imageBytes IsNot Nothing Then
                    cmd.Parameters.AddWithValue("@Image1", imageBytes)  ' Set the byte array as the image data
                Else
                    cmd.Parameters.AddWithValue("@Image1", DBNull.Value)  ' If no image, set DBNull
                End If

                ' Execute the SQL query
                cmd.ExecuteNonQuery()
            End Using

            ' Log action to the audit trail
            Try
                Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co"),
                      auditCmd As New MySqlCommand("INSERT INTO audittbl (DateTime, User_type, Name, Action) 
                                        VALUES (NOW(), @User_type, @Name, @Action)", con)
                    con.Open()

                    ' Add audit trail parameters
                    auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' Replace with logged-in user type
                    auditCmd.Parameters.AddWithValue("@Name", Label26.Text)     ' Replace with logged-in user name
                    auditCmd.Parameters.AddWithValue("@Action", $"Updated product with Barcode: {Barc.Text}")
                    auditCmd.ExecuteNonQuery()
                End Using
            Catch auditEx As MySqlException
                MsgBox("Audit trail logging failed: " & auditEx.Message, MessageBoxIcon.Warning)
            End Try

            ' Show confirmation message and label
            Panel9.Visible = False

            ' Update the image in DataGridView1
            For Each row As DataGridViewRow In DataGridView1.Rows
                ' Check if the Barcode matches the current Barcode in the DataGridView
                If row.Cells("Barcode").Value.ToString() = Barc.Text Then
                    ' Replace the row data with the new values
                    row.Cells("Product_Name").Value = PN.Text
                    row.Cells("Description").Value = Desc.Text
                    row.Cells("Cat_name").Value = CN.Text
                    row.Cells("ItemSize").Value = Size.SelectedItem?.ToString()
                    row.Cells("Color").Value = COR.SelectedItem?.ToString()
                    row.Cells("Selling_price").Value = Convert.ToDecimal(SP.Text)

                    ' Update the image in the DataGridView (if available)
                    If imageBytes IsNot Nothing Then
                        Using ms As New MemoryStream(imageBytes)
                            row.Cells("Image1").Value = Image.FromStream(ms)
                        End Using
                    Else
                        row.Cells("Image1").Value = Nothing ' Set image cell to nothing if no image
                    End If

                    ' Optional: Show confirmation or refresh the UI
                    MsgBox("Product details updated in the grid.", MessageBoxIcon.Information)
                    Exit For
                End If
            Next
        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxIcon.Error)
        Catch ex As FormatException
            MsgBox("Formatting error: " & ex.Message, MessageBoxIcon.Warning)
        Catch ex As Exception
            MsgBox("An unexpected error occurred: " & ex.Message, MessageBoxIcon.Error)
        End Try


    End Sub

    Private Sub IconButton26_Click(sender As Object, e As EventArgs) Handles IconButton26.Click
        Panel9.Visible = False
    End Sub

    Private Sub IconButton30_Click(sender As Object, e As EventArgs) Handles IconButton30.Click
        cons()
        Panel9.Visible = False
        Panel26.Visible = True
    End Sub

    Private Sub IconButton49_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub IconButton49_Click_1(sender As Object, e As EventArgs) Handles IconButton49.Click
        Panel26.Visible = False
    End Sub
    Private Sub Us_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles SP1.KeyPress
        ' Define a string containing the allowed characters
        Dim allowedChars As String = "1234567890."

        ' Check if the pressed key is not one of the allowed characters or a control key
        If Not allowedChars.Contains(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            ' Cancel the key press
            e.Handled = True
        End If
    End Sub
    Private Sub U_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles SP.KeyPress
        ' Define a string containing the allowed characters
        Dim allowedChars As String = "1234567890."

        ' Check if the pressed key is not one of the allowed characters or a control key
        If Not allowedChars.Contains(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            ' Cancel the key press
            e.Handled = True
        End If
    End Sub
    Private Sub Unmedrs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Barc.KeyPress
        ' Define a string containing the allowed characters
        Dim allowedChars As String = "1234567890"

        ' Check if the pressed key is not one of the allowed characters or a control key
        If Not allowedChars.Contains(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            ' Cancel the key press
            e.Handled = True
        End If
    End Sub
    Private Sub Unmedr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        ' Define a string containing the allowed characters
        Dim allowedChars As String = "1234567890"

        ' Check if the pressed key is not one of the allowed characters or a control key
        If Not allowedChars.Contains(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            ' Cancel the key press
            e.Handled = True
        End If
    End Sub
    Private Sub IconButton50_Click(sender As Object, e As EventArgs) Handles IconButton50.Click

        If String.IsNullOrWhiteSpace(Desc1.Text) OrElse
   String.IsNullOrWhiteSpace(PN1.Text) OrElse
   String.IsNullOrWhiteSpace(CN1.Text) OrElse
   String.IsNullOrWhiteSpace(Size1.Text) OrElse
   String.IsNullOrWhiteSpace(SP1.Text) OrElse
   String.IsNullOrWhiteSpace(COR1.Text) Then
            MsgBox("Please fill in all fields.")
            Return
        End If

        ' Validate Selling Price as an integer
        If Not IsNumeric(SP1.Text) OrElse Convert.ToInt32(SP1.Text) < 1 Then
            MsgBox("Please enter a valid integer for Selling Price.")
            Return
        End If

        Try
            ' Open the database connection if it's not already open
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If
            Dim newBarcode As Integer
            Dim predefinedBarcode As String

            ' Always generate the predefined barcode with the current month and year (regardless of TextBox10 or checkbox)
            Dim currentMonth As String = DateTime.Now.ToString("MM") ' Get the current month (2 digits)
            Dim currentYear As String = DateTime.Now.ToString("yyyy") ' Get the current year (4 digits)
            predefinedBarcode = currentYear & currentMonth ' Example: "042025" for April 2025

            ' Check if the checkbox is unchecked (use TextBox10 barcode) or auto-generate the barcode if checked
            If Not CheckBox3.Checked AndAlso Not String.IsNullOrWhiteSpace(TextBox10.Text) Then
                ' Use the barcode from TextBox10 if the checkbox is unchecked and TextBox10 has a value
                newBarcode = Convert.ToInt32(TextBox10.Text)
            Else
                ' Auto-generate the barcode if the checkbox is checked or TextBox10 is empty
                ' Get all barcodes from the database with the current YYYYMM prefix
                Dim getAllBarcodesCmd As New MySqlCommand("SELECT Barcode FROM producttbl WHERE Barcode LIKE @predefinedBarcode ORDER BY Barcode", con)
                getAllBarcodesCmd.Parameters.AddWithValue("@predefinedBarcode", predefinedBarcode & "%") ' Match the prefix YYYYMM%
                Dim reader As MySqlDataReader = getAllBarcodesCmd.ExecuteReader()

                Dim existingBarcodes As New List(Of Integer)()

                ' Collect all existing barcode numbers (after the YYYYMM prefix)
                While reader.Read()
                    Dim barcode As String = reader("Barcode").ToString()
                    Dim numericPart As Integer = Convert.ToInt32(barcode.Substring(6)) ' Extract the numeric part after YYYYMM
                    existingBarcodes.Add(numericPart)
                End While
                reader.Close()

                ' Find the first missing number in the sequence
                Dim missingBarcodeFound As Boolean = False

                ' Start from 1 and check until we find the missing number
                For i As Integer = 1 To existingBarcodes.Max() + 1 ' Check up to one more than the current max
                    If Not existingBarcodes.Contains(i) Then
                        newBarcode = i ' Found the missing barcode number
                        missingBarcodeFound = True
                        Exit For
                    End If
                Next

                ' If no missing barcode was found (all numbers are filled), use the next highest number + 1
                If Not missingBarcodeFound Then
                    newBarcode = existingBarcodes.Max() + 1
                End If
            End If

            ' Generate the full barcode with the predefined barcode prefix and the new incremented number
            Dim fullBarcode As String = predefinedBarcode & newBarcode.ToString()
            Using cmd As New MySqlCommand()
                cmd.Connection = con
                cmd.CommandText = "INSERT INTO producttbl (`Barcode`, `Product_Name`, `Description`, `Cat_name`, `ItemSize`, `Selling_price`, `Color`, `Image1`) 
                           VALUES (@Barcode, @Product_Name, @Description, @Cat_name, @ItemSize, @Selling_price, @Color, @Image1)"
                cmd.Parameters.AddWithValue("@Barcode", fullBarcode) ' Use the full concatenated barcode
                cmd.Parameters.AddWithValue("@Product_Name", PN1.Text)
                cmd.Parameters.AddWithValue("@Description", Desc1.Text)
                cmd.Parameters.AddWithValue("@Cat_name", CN1.Text)
                cmd.Parameters.AddWithValue("@ItemSize", Size1.Text)
                cmd.Parameters.AddWithValue("@Selling_price", Convert.ToInt32(SP1.Text))
                cmd.Parameters.AddWithValue("@Color", COR1.Text)

                ' Convert the image in PictureBox to a byte array
                If PictureBox2.Image IsNot Nothing Then
                    Dim ms As New MemoryStream()
                    PictureBox2.Image.Save(ms, PictureBox2.Image.RawFormat)
                    cmd.Parameters.AddWithValue("@Image1", ms.ToArray()) ' Store the image as a byte array
                Else
                    ' If no image is selected, store NULL or a default image
                    cmd.Parameters.AddWithValue("@Image1", DBNull.Value)
                End If

                ' Execute the query to insert the product data
                cmd.ExecuteNonQuery()
                MsgBox("Product successfully added!")
            End Using


            DataGridView1.Rows.Add(fullBarcode, PN1.Text, Desc1.Text, CN1.Text, COR1.Text, Size1.Text, SP1.Text, PictureBox2.Image)

            ' Log the action in the audit trail
            Using auditCon As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                auditCon.Open()
                Using auditCmd As New MySqlCommand("INSERT INTO audittbl (DateTime, User_type, Name, Action) VALUES (NOW(), @User_type, @Name, @Action)", auditCon)
                    auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' User type
                    auditCmd.Parameters.AddWithValue("@Name", Label26.Text)     ' User name
                    auditCmd.Parameters.AddWithValue("@Action", $"Added new product with Barcode: {fullBarcode}")
                    auditCmd.ExecuteNonQuery()
                End Using
            End Using

            ' Hide the panel after insertion
            Panel26.Visible = False

            ' Optionally, reset fields
            PN1.Text = ""
            Desc1.Text = ""
            CN1.SelectedIndex = -1
            COR1.SelectedIndex = -1
            SP1.Text = ""
            Size1.SelectedIndex = -1
            PictureBox2.Image = Nothing ' Reset the image after the action is completed

        Catch ex As MySqlException
            MsgBox($"Error: {ex.Message}")
        Catch ex As Exception
            MsgBox($"Unexpected error: {ex.Message}")
        Finally
            ' Close the database connection if it's still open
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try


    End Sub


    Private Sub IconButton54_Click(sender As Object, e As EventArgs) Handles IconButton54.Click
        For Each row As DataGridViewRow In DataGridView6.Rows
            If row.Cells(0).Value.ToString() = TextBox9.Text Then
                MsgBox("Color Already Exists")
                Exit Sub
            End If
        Next

        ' Ensure the category name is not empty
        If String.IsNullOrWhiteSpace(TextBox9.Text) Then
            MsgBox("MISSING INPUT")
            Exit Sub
        End If

        Try
            ' Open the database connection if not already open
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If

            ' INSERT new category
            Using cmd As New MySqlCommand("INSERT INTO Colortbl (`ItemColor`) VALUES (@ItemColor)", con)
                cmd.Parameters.AddWithValue("@ItemColor", TextBox9.Text)
                cmd.ExecuteNonQuery()

                ' Log the action in the audit trail
                Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, 'Added new category')", con)
                    auditCmd.Parameters.AddWithValue("@User_type", Label45.Text)
                    auditCmd.Parameters.AddWithValue("@Name", Label26.Text)
                    auditCmd.ExecuteNonQuery()
                End Using

                MsgBox("Color Successfully Added!")
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
        Me.Hide()

        ' Dispose the current form (optional)
        Me.Dispose()

        ' Create a new instance of the form
        Dim newForm As New MainMenuf()  ' Replace Form1 with the actual name of your form
        newForm.Panel28.Visible = True

        ' Show the new instance of the form
        newForm.Show()
    End Sub

    Private Sub DataGridView6_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView6.CellContentClick

        If e.ColumnIndex = DataGridView6.Columns("Cdelete").Index AndAlso e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView6.Rows(e.RowIndex)
            Dim itemColor As String = selectedRow.Cells("ItemColor").Value.ToString()

            ' Check if ItemColor is in use (for example, in a product or other related table)
            Dim isItemColorInUse As Boolean = False
            Try
                ' Open the database connection
                Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                    con.Open()

                    ' Query to check if ItemColor is used in any other table (e.g., producttbl)
                    Dim checkQuery As String = "SELECT COUNT(*) FROM producttbl WHERE Color = @Color"
                    Using checkCmd As New MySqlCommand(checkQuery, con)
                        checkCmd.Parameters.AddWithValue("@Color", itemColor)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If count > 0 Then
                            ' ItemColor is in use, show a message and prevent deletion
                            MessageBox.Show("This color is currently in use and cannot be deleted.", "Deletion Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit Sub
                        End If
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

            ' Confirm deletion
            Dim result As DialogResult = MessageBox.Show($"Are you sure you want to delete '{itemColor}'?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                Try
                    ' Open the database connection again for deletion
                    Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                        con.Open()

                        ' Delete the record from the database
                        Dim deleteQuery As String = "DELETE FROM Colortbl WHERE ItemColor = @ItemColor"
                        Using deleteCmd As New MySqlCommand(deleteQuery, con)
                            deleteCmd.Parameters.AddWithValue("@ItemColor", itemColor)
                            deleteCmd.ExecuteNonQuery()
                        End Using

                        ' Add the deletion action to the audit trail
                        Dim auditQuery As String = "INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, @Action)"
                        Using auditCmd As New MySqlCommand(auditQuery, con)
                            auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' User type (e.g., Admin)
                            auditCmd.Parameters.AddWithValue("@Name", Label26.Text)     ' User's name
                            auditCmd.Parameters.AddWithValue("@Action", $"Deleted color: {itemColor}") ' Custom action message
                            auditCmd.ExecuteNonQuery()
                        End Using
                    End Using

                    ' Refresh the DataGridView (or remove the row manually)
                    DataGridView6.Rows.RemoveAt(e.RowIndex)
                    MessageBox.Show("Color successfully deleted and logged!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As MySqlException
                    MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End If


        If e.ColumnIndex = DataGridView6.Columns("Cupdate").Index AndAlso e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView6.Rows(e.RowIndex)
            Dim itemColor As String = selectedRow.Cells("ItemColor").Value.ToString()

            ' Check if the ItemColor is in use in producttbl (or any other related table)
            Dim isItemColorInUse As Boolean = False
            Try
                ' Open the database connection
                Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                    con.Open()

                    ' Query to check if the ItemColor is used in the producttbl
                    Dim checkQuery As String = "SELECT COUNT(*) FROM producttbl WHERE Color = @Color"
                    Using checkCmd As New MySqlCommand(checkQuery, con)
                        checkCmd.Parameters.AddWithValue("@Color", itemColor)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If count > 0 Then
                            ' ItemColor is in use, disable the Cupdate button in DataGridView for this row
                            DataGridView6.Rows(e.RowIndex).Cells("Cupdate").ReadOnly = True
                            MessageBox.Show("This color is currently in use and cannot be updated.", "Update Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit Sub
                        Else
                            ' Enable the Cupdate button if the color is not in use
                            DataGridView6.Rows(e.RowIndex).Cells("Cupdate").ReadOnly = False
                        End If
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

            ' Show Panel29 for updating
            Panel31.Visible = False
            Panel29.Visible = True
            TextBox3.Text = itemColor
        End If

    End Sub

    Private Sub IconButton51_Click(sender As Object, e As EventArgs)

        Try
            ' Validate the input in TextBox3 (new color)
            If String.IsNullOrWhiteSpace(TextBox3.Text) Then
                MessageBox.Show("Please enter a valid color.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim newColor As String = TextBox3.Text.Trim() ' New color from TextBox3

            ' Open the database connection
            Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                con.Open()

                ' Update the record in the database
                Dim updateQuery As String = "UPDATE Colortbl SET ItemColor = @NewColor WHERE ItemColor = @NewColor"
                Using updateCmd As New MySqlCommand(updateQuery, con)
                    updateCmd.Parameters.AddWithValue("@NewColor", newColor)

                    Dim rowsAffected As Integer = updateCmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        ' Log the update in the audit trail
                        Dim auditQuery As String = "INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, @Action)"
                        Using auditCmd As New MySqlCommand(auditQuery, con)
                            auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' User type (e.g., Admin)
                            auditCmd.Parameters.AddWithValue("@Name", Label26.Text)     ' User's name
                            auditCmd.Parameters.AddWithValue("@Action", $"Updated color to {newColor}")
                            auditCmd.ExecuteNonQuery()
                        End Using

                        ' Show success message
                        MessageBox.Show("Color updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("No record found with the specified color.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End Using
            End Using

            ' Optionally refresh the DataGridView here if needed
            ' Example: RefreshDataGrid()

            ' Hide Panel29 after updating
            Panel29.Visible = False
        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub IconButton51_Click_1(sender As Object, e As EventArgs) Handles IconButton51.Click

        If String.IsNullOrWhiteSpace(TextBox3.Text) Then
            MsgBox("MISSING INPUT")
            Exit Sub
        End If

        ' Check if a row is selected in DataGridView6
        If DataGridView6.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView6.SelectedRows(0)
            Dim oldColor As String = selectedRow.Cells(0).Value.ToString()

            ' Check if the new ItemColor already exists
            For Each row As DataGridViewRow In DataGridView6.Rows
                ' Skip the selected row itself while checking for duplicates
                If row.Cells(0).Value IsNot Nothing AndAlso row.Cells(0).Value.ToString() = TextBox3.Text Then
                    MsgBox("Color Already Exists")
                    Exit Sub
                End If
            Next

            ' Check if the ItemColor is in use in another table (e.g., producttbl)
            Dim isItemColorInUse As Boolean = False
            Try
                ' Open the database connection if it is not already open
                If con.State <> ConnectionState.Open Then
                    con.Open()
                End If

                ' Query to check if the new color exists in a table like producttbl
                Dim checkQuery As String = "SELECT COUNT(*) FROM producttbl WHERE Color = @Color"
                Using checkCmd As New MySqlCommand(checkQuery, con)
                    checkCmd.Parameters.AddWithValue("@Color", TextBox3.Text)
                    Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                    If count > 0 Then
                        ' ItemColor is in use, show a message and prevent update
                        MsgBox("This color is currently in use and cannot be updated.", MsgBoxStyle.Exclamation)
                        Exit Sub
                    End If
                End Using

                ' Proceed to update the color in Colortbl only if the color is not in use in producttbl
                If oldColor <> TextBox3.Text Then
                    ' Create and execute the UPDATE command for the ItemColor
                    Using cmd As New MySqlCommand("UPDATE Colortbl SET ItemColor = @ItemColor WHERE ItemColor = @OldColor", con)
                        ' Add parameters for the update
                        cmd.Parameters.AddWithValue("@ItemColor", TextBox3.Text)
                        cmd.Parameters.AddWithValue("@OldColor", oldColor)

                        ' Execute the command
                        cmd.ExecuteNonQuery()

                        ' Log the action into the audit trail
                        Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, 'Updated category')", con)
                            ' Add parameters for the audit trail
                            auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' User type (e.g., Admin)
                            auditCmd.Parameters.AddWithValue("@Name", Label26.Text)     ' User's name

                            ' Execute the audit trail insert
                            auditCmd.ExecuteNonQuery()
                        End Using

                        ' Display success message
                        MsgBox("Color successfully updated!")
                    End Using
                Else
                    MsgBox("No changes were made as the color is the same as the old one.")
                End If

            Catch ex As MySqlException
                ' Handle MySQL-specific exceptions
                MsgBox("Database error: " & ex.Message)
            Catch ex As Exception
                ' Handle other exceptions
                MsgBox("An error occurred: " & ex.Message)
            Finally
                ' Ensure the database connection is closed properly
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try

        Else
            MsgBox("Please select a Color to update.")
        End If



        ' Hide and dispose the current form
        Me.Hide()
        Me.Dispose()

        ' Create a new instance of the form
        Dim newForm As New MainMenuf()  ' Replace Form1 with the actual name of your form
        newForm.Panel28.Visible = True
        ' Show the new instance of the form
        newForm.Show()
    End Sub

    Private Sub IconButton53_Click(sender As Object, e As EventArgs) Handles IconButton53.Click
        Panel31.Visible = False
    End Sub

    Private Sub IconButton40_Click(sender As Object, e As EventArgs) Handles IconButton40.Click
        Panel29.Visible = False
    End Sub

    Private Sub IconButton52_Click(sender As Object, e As EventArgs) Handles IconButton52.Click
        Panel29.Visible = False
        Panel31.Visible = True
    End Sub

    Private Sub IconButton57_Click(sender As Object, e As EventArgs) Handles IconButton57.Click
        Panel34.Visible = False
        Panel36.Visible = True
    End Sub

    Private Sub IconButton58_Click(sender As Object, e As EventArgs) Handles IconButton58.Click
        Panel36.Visible = False
    End Sub

    Private Sub IconButton55_Click(sender As Object, e As EventArgs) Handles IconButton55.Click
        Panel34.Visible = False
    End Sub

    Private Sub IconButton59_Click(sender As Object, e As EventArgs) Handles IconButton59.Click
        For Each row As DataGridViewRow In DataGridView7.Rows
            If row.Cells(0).Value.ToString() = TextBox5.Text Then
                MsgBox("Size Already Exists")
                Exit Sub
            End If
        Next

        ' Ensure the category name is not empty
        If String.IsNullOrWhiteSpace(TextBox5.Text) Then
            MsgBox("MISSING INPUT")
            Exit Sub
        End If

        Try
            ' Open the database connection if not already open
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If

            ' INSERT new category
            Using cmd As New MySqlCommand("INSERT INTO Sizetbl (`Item_Size`) VALUES (@Item_Size)", con)
                cmd.Parameters.AddWithValue("@Item_Size", TextBox5.Text)
                cmd.ExecuteNonQuery()

                ' Log the action in the audit trail
                Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, 'Added new category')", con)
                    auditCmd.Parameters.AddWithValue("@User_type", Label45.Text)
                    auditCmd.Parameters.AddWithValue("@Name", Label26.Text)
                    auditCmd.ExecuteNonQuery()
                End Using

                MsgBox("Size Successfully Added!")
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
        Me.Hide()

        ' Dispose the current form (optional)
        Me.Dispose()

        ' Create a new instance of the form
        Dim newForm As New MainMenuf()  ' Replace Form1 with the actual name of your form
        newForm.Panel33.Visible = True

        ' Show the new instance of the form
        newForm.Show()
    End Sub

    Private Sub DataGridView7_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView7.CellContentClick
        If e.ColumnIndex = DataGridView7.Columns("Sdelete").Index AndAlso e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView7.Rows(e.RowIndex) ' Corrected to refer to DataGridView7
            Dim ItemSize As String = selectedRow.Cells("Item_Size").Value.ToString() ' ItemSize is the column to delete

            ' Check if ItemSize is in use (for example, in a product or order table)
            Dim isItemSizeInUse As Boolean = False
            Try
                ' Open the database connection
                Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                    con.Open()

                    ' Query to check if ItemSize is used in any other table (e.g., producttbl)
                    Dim checkQuery As String = "SELECT COUNT(*) FROM producttbl WHERE ItemSize = @ItemSize"
                    Using checkCmd As New MySqlCommand(checkQuery, con)
                        checkCmd.Parameters.AddWithValue("@ItemSize", ItemSize)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If count > 0 Then
                            ' ItemSize is in use, show a message and prevent deletion
                            MessageBox.Show("This size is currently in use and cannot be deleted.", "Deletion Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit Sub
                        End If
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

            ' Confirm deletion
            Dim result As DialogResult = MessageBox.Show($"Are you sure you want to delete '{ItemSize}'?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                Try
                    ' Open the database connection again for deletion
                    Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                        con.Open()

                        ' Delete the record from the database
                        Dim deleteQuery As String = "DELETE FROM Sizetbl WHERE Item_Size = @Item_Size"
                        Using deleteCmd As New MySqlCommand(deleteQuery, con)
                            deleteCmd.Parameters.AddWithValue("@Item_Size", ItemSize) ' Use ItemSize here
                            deleteCmd.ExecuteNonQuery()
                        End Using

                        ' Add the deletion action to the audit trail
                        Dim auditQuery As String = "INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, @Action)"
                        Using auditCmd As New MySqlCommand(auditQuery, con)
                            auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' User type (e.g., Admin)
                            auditCmd.Parameters.AddWithValue("@Name", Label26.Text)     ' User's name
                            auditCmd.Parameters.AddWithValue("@Action", $"Deleted size: {ItemSize}") ' Custom action message
                            auditCmd.ExecuteNonQuery()
                        End Using
                    End Using

                    ' Refresh the DataGridView (or remove the row manually)
                    DataGridView7.Rows.RemoveAt(e.RowIndex)
                    MessageBox.Show("Size successfully deleted and logged!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As MySqlException
                    MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End If


        If e.ColumnIndex = DataGridView7.Columns("Supdate").Index AndAlso e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView7.Rows(e.RowIndex)
            Dim itemSize As String = selectedRow.Cells("Item_Size").Value.ToString()

            ' Check if the ItemSize is in use in producttbl (or any other related table)
            Dim isItemSizeInUse As Boolean = False
            Try
                ' Open the database connection
                Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                    con.Open()

                    ' Query to check if the ItemSize is used in the producttbl
                    Dim checkQuery As String = "SELECT COUNT(*) FROM producttbl WHERE ItemSize = @ItemSize"
                    Using checkCmd As New MySqlCommand(checkQuery, con)
                        checkCmd.Parameters.AddWithValue("@ItemSize", itemSize)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If count > 0 Then
                            ' ItemSize is in use, disable the Supdate button in DataGridView for this row
                            DataGridView7.Rows(e.RowIndex).Cells("Supdate").ReadOnly = True
                            MessageBox.Show("This item size is currently in use and cannot be updated.", "Update Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit Sub
                        Else
                            ' Enable the Supdate button if the item size is not in use
                            DataGridView7.Rows(e.RowIndex).Cells("Supdate").ReadOnly = False
                        End If
                    End Using
                End Using
            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

            ' Show Panel34 for updating
            Panel36.Visible = False
            Panel34.Visible = True
            TextBox4.Text = itemSize
        End If

    End Sub

    Private Sub IconButton56_Click(sender As Object, e As EventArgs) Handles IconButton56.Click
        ' Ensure the TextBox input is not empty before proceeding
        If String.IsNullOrWhiteSpace(TextBox4.Text) Then
            MsgBox("MISSING INPUT")
            Exit Sub
        End If

        ' Check if a row is selected in DataGridView7
        If DataGridView7.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView7.SelectedRows(0)

            ' Check if the new size already exists
            For Each row As DataGridViewRow In DataGridView7.Rows
                ' Skip the selected row itself while checking for duplicates
                If row.Cells(0).Value IsNot Nothing AndAlso row.Cells(0).Value.ToString() = TextBox4.Text Then
                    MsgBox("Size Already Exists")
                    Exit Sub
                End If
            Next

            Try
                ' Open the database connection if it is not already open
                If con.State <> ConnectionState.Open Then
                    con.Open()
                End If

                ' Create and execute the UPDATE command for the size
                Using cmd As New MySqlCommand("UPDATE Sizetbl SET Item_Size = @Item_Size WHERE Item_Size = @OldSize", con)
                    ' Add parameters for the update
                    cmd.Parameters.AddWithValue("@Item_Size", TextBox4.Text)  ' Corrected parameter name
                    cmd.Parameters.AddWithValue("@OldSize", selectedRow.Cells(0).Value.ToString())

                    ' Execute the command
                    cmd.ExecuteNonQuery()

                    ' Log the action into the audit trail
                    Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, 'Updated size')", con)
                        ' Add parameters for the audit trail
                        auditCmd.Parameters.AddWithValue("@User_type", Label45.Text) ' Dynamically set User_type based on the logged-in user
                        auditCmd.Parameters.AddWithValue("@Name", Label26.Text) ' User's name

                        ' Execute the audit trail insert
                        auditCmd.ExecuteNonQuery()
                    End Using

                    ' Display success message
                    MsgBox("Size successfully updated!")

                End Using

            Catch ex As MySqlException
                ' Handle MySQL-specific exceptions
                MsgBox("Database error: " & ex.Message)
            Catch ex As Exception
                ' Handle other exceptions
                MsgBox("An error occurred: " & ex.Message)
            Finally
                ' Ensure the database connection is closed properly
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try

        Else
            MsgBox("Please select a Size to update.")
        End If

        ' Optionally, you can refresh the DataGridView or take any other action before hiding the form
        ' RefreshDataGrid() ' You can implement this method if needed to reload the DataGridView

        ' Hide and dispose the current form
        Me.Hide()
        Me.Dispose()

        ' Create a new instance of the form
        Dim newForm As New MainMenuf()  ' Replace Form1 with the actual name of your form
        newForm.Panel33.Visible = True
        ' Show the new instance of the form
        newForm.Show()
    End Sub

    Private Sub CN1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CN1.SelectedIndexChanged

    End Sub

    Private Sub IconButton41_Click(sender As Object, e As EventArgs) Handles IconButton41.Click
        Panel28.Visible = True
        Panel33.Visible = False
        Panel3.Visible = False
        Panel2.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel6.Visible = False
    End Sub

    Private Sub IconButton39_Click(sender As Object, e As EventArgs) Handles IconButton39.Click
        Panel28.Visible = False
        Panel33.Visible = True
        Panel3.Visible = False
        Panel2.Visible = False
        Panel4.Visible = False
        Panel5.Visible = False
        Panel6.Visible = False
    End Sub

    Private Sub IconButton11_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub IconButton61_Click(sender As Object, e As EventArgs) Handles IconButton61.Click
        Panel38.Visible = False
    End Sub

    Private Sub IconButton15_Click(sender As Object, e As EventArgs) Handles IconButton15.Click
        Panel38.Visible = True
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Open the connection within a 'Using' block
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection to the database

                ' Prepare the insert query to insert VAT value into the vattbl table
                Dim insertQuery As String = "INSERT INTO vattbl (VAT) VALUES (@VAT)"

                ' Create the MySqlCommand using the insert query
                Using cmd As New MySqlCommand(insertQuery, con)
                    ' Add the VAT parameter from the TextBox (TextBox6.Text)
                    cmd.Parameters.AddWithValue("@VAT", TextBox6.Text.Trim())

                    ' Execute the query to insert the VAT value
                    cmd.ExecuteNonQuery()

                    ' Notify the user that the VAT has been successfully inserted
                    MsgBox("VAT value successfully inserted into the database!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using

                ' After inserting the VAT, retrieve the inserted value from the database
                Dim selectQuery As String = "SELECT VAT FROM vattbl ORDER BY VAT DESC LIMIT 1"
                Using cmdSelect As New MySqlCommand(selectQuery, con)
                    Using reader As MySqlDataReader = cmdSelect.ExecuteReader()
                        If reader.HasRows Then
                            reader.Read()
                            ' Add the retrieved VAT value to DataGridView9
                            DataGridView9.Rows.Add(reader("VAT").ToString())
                        End If
                    End Using
                End Using

                ' Audit Trail: Log the action of inserting VAT into the audit table
                Dim auditQuery As String = "INSERT INTO audittbl (DateTime, User_type, Name, Action) VALUES (NOW(), @User_type, @Name, @Action)"
                Using cmdAudit As New MySqlCommand(auditQuery, con)
                    ' Add parameters for the audit trail
                    cmdAudit.Parameters.AddWithValue("@User_type", Label45.Text)  ' Replace with logged-in user type
                    cmdAudit.Parameters.AddWithValue("@Name", Label26.Text)      ' Replace with logged-in user name
                    cmdAudit.Parameters.AddWithValue("@Action", "Inserted VAT value: " & TextBox6.Text.Trim())

                    ' Execute the audit trail query
                    cmdAudit.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As MySqlException
            ' Handle database errors
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ' Handle general errors
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Hide the panel after operation
        Panel40.Visible = False

        ' Enable or disable IconButton63 based on DataGridView9 row count
        If DataGridView9.Rows.Count > 0 Then
            IconButton63.Enabled = False
        Else
            IconButton63.Enabled = True
        End If
    End Sub

    Private Sub PI_TextChanged(sender As Object, e As EventArgs) Handles PI.TextChanged

    End Sub

    Private Sub Panel12_Paint(sender As Object, e As PaintEventArgs) Handles Panel12.Paint

    End Sub


    ' Ensure the clicked cell is not from a new row or invalid row
    Private Sub DataGridView9_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView9.CellContentClick
        ' Check if the clicked cell is part of the Delete button column
        If e.ColumnIndex >= 0 AndAlso DataGridView9.Columns(e.ColumnIndex).Name = "Vdelete" Then
            Panel39.Visible = False
            ' Confirm the deletion action
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this VAT record?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                Try
                    ' Get the VAT value to delete (assuming it's in the second column, adjust if needed)
                    Dim VATValueToDelete As String = DataGridView9.Rows(e.RowIndex).Cells("VAT").Value.ToString()

                    ' Delete the VAT record from the database
                    If con.State <> ConnectionState.Open Then
                        con.Open()
                    End If

                    Using cmd As New MySqlCommand("DELETE FROM vattbl WHERE VAT = @VAT", con)
                        cmd.Parameters.AddWithValue("@VAT", VATValueToDelete)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Remove the row from DataGridView
                    DataGridView9.Rows.RemoveAt(e.RowIndex)

                    MessageBox.Show("VAT record successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    ' Close connection if it's still open
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End If
        End If
        If DataGridView9.Rows.Count > 0 Then
            IconButton63.Enabled = False
        Else
            IconButton63.Enabled = True
        End If
        If e.ColumnIndex >= 0 AndAlso DataGridView9.Columns(e.ColumnIndex).Name = "Vupdate" Then

            Panel39.Visible = True


            Dim vatValueToUpdate As String = DataGridView9.Rows(e.RowIndex).Cells("VAT").Value.ToString()
            PI.Text = vatValueToUpdate
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Try
            ' Get the new VAT value from TextBox10
            Dim newVAT As String = PI.Text.Trim()

            ' Validate if the new VAT value is not empty or invalid
            If String.IsNullOrEmpty(newVAT) Then
                MessageBox.Show("Please enter a valid VAT value.", "Invalid VAT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Get the current VAT value that is being updated
            Dim oldVAT As String = DataGridView9.CurrentRow.Cells("VAT").Value.ToString()

            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' Create the SQL UPDATE command to update the VAT value in the database
                Dim updateQuery As String = "UPDATE vattbl SET VAT = @NewVAT WHERE VAT = @OldVAT"

                Using cmd As New MySqlCommand(updateQuery, con)
                    cmd.Parameters.AddWithValue("@NewVAT", newVAT)
                    cmd.Parameters.AddWithValue("@OldVAT", oldVAT)

                    ' Execute the query to update the VAT in the database
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' Update the corresponding VAT value in the DataGridView9
            DataGridView9.CurrentRow.Cells("VAT").Value = newVAT

            ' Optionally, hide Panel39 after the update is complete
            Panel39.Visible = False

            ' Show success message
            MessageBox.Show("VAT value updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            ' Handle any exceptions
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Panel33_Paint(sender As Object, e As PaintEventArgs) Handles Panel33.Paint

    End Sub

    Private Sub IconButton33_Click_1(sender As Object, e As EventArgs)
        Dim nextForm As New POSF()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton11_Click_1(sender As Object, e As EventArgs) Handles IconButton11.Click
        Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

        Using con As New MySqlConnection(connectionString)
            con.Open() ' Open the connection

            ' Define the SQL query to join product and quantity tables
            Dim query As String = "SELECT producttbl.Barcode, Product_Name, Cat_name, Color, ItemSize, Selling_price, Cost_price, Quantity " &
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
                DataGridView10.DataSource = ds.Tables(0)

                ' Loop through each row in DataGridView10 to check the quantity
                For Each row As DataGridViewRow In DataGridView10.Rows
                    Dim quantity As Integer
                    If Integer.TryParse(row.Cells("Quantity").Value?.ToString(), quantity) Then
                        ' If quantity is less than 5, show the message box
                        If quantity < 5 Then
                            MsgBox("You need to restock quantity for product: " & row.Cells("Product_Name").Value.ToString(),
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit For ' Exit the loop after showing the message
                        End If
                    End If
                Next
            Else

            End If
        End Using
        Dim nextForm As New POSF()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub PIC1_Click(sender As Object, e As EventArgs) Handles PIC1.Click

    End Sub

    Private Sub IconButton44_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub GenerateBarcode()


    End Sub
    Private Sub PrintBarcode(sender As Object, e As PrintPageEventArgs)
        Try
            ' Check if the PictureBox has an image to print
            If PIC1.Image IsNot Nothing Then
                ' Get the barcode image
                Dim barcodeImage As Image = PIC1.Image

                ' Define the paper size (2 inches wide in pixels, assuming 96 DPI)
                Dim paperWidth As Integer = 80 ' 2 inches * 96 DPI
                Dim paperHeight As Integer = e.PageBounds.Height

                ' Calculate the x-coordinate to center the barcode
                Dim x As Integer = (paperWidth - barcodeImage.Width) / 2
                Dim y As Integer = 10 ' Top margin

                ' Draw the barcode image on the page
                e.Graphics.DrawImage(barcodeImage, x, y, barcodeImage.Width, barcodeImage.Height)
            Else
                MessageBox.Show("No barcode image to print.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show($"An error occurred while printing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Barc1_TextChanged(sender As Object, e As EventArgs)

        GenerateBarcode()

    End Sub



    Private Sub IconButton62_Click(sender As Object, e As EventArgs) Handles IconButton62.Click
        Try
            ' Define the predefined barcode part (e.g., 202506)
            Dim predefinedBarcode As String = ""

            ' Get the user input (e.g., 1110)
            Dim userInput As String = Barc.Text ' Assuming Barc is the TextBox for user input

            ' Combine the predefined part with the user input to form the final barcode
            Dim fullBarcode As String = predefinedBarcode & userInput

            ' Create a new BarcodeWriter instance
            Dim writer As New BarcodeWriter()
            writer.Format = BarcodeFormat.CODE_128

            ' Generate the barcode using the combined string
            PictureBox4.Image = writer.Write(fullBarcode)

            ' Set up the PrintDocument for printing
            AddHandler printDoc.PrintPage, AddressOf PrintBarcode1 ' Attach event to handle printing
            printDoc.Print() ' Trigger the print process

        Catch ex As Exception
            ' Handle errors gracefully
            MessageBox.Show($"An error occurred while generating the barcode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub PrintBarcode1(sender As Object, e As PrintPageEventArgs)
        Try
            ' Check if the PictureBox has an image to print
            If PictureBox4.Image IsNot Nothing Then
                ' Get the barcode image from the PictureBox
                Dim barcodeImage As Image = PictureBox4.Image

                ' Define the label size in millimeters (40mm x 20mm)
                Dim labelWidthMm As Double = 40  ' 40mm width
                Dim labelHeightMm As Double = 28 ' 20mm height

                ' Define the DPI (assuming 96 DPI)
                Dim dpi As Double = 100

                ' Convert the label size from mm to pixels
                Dim labelWidth As Integer = CInt(labelWidthMm * dpi / 25.4)
                Dim labelHeight As Integer = CInt(labelHeightMm * dpi / 25.4)

                ' Calculate the scale factor to maintain the aspect ratio (fit the image inside the label)
                ' This will scale the image to fit within the available label space without distortion
                Dim scaleFactor As Double = Math.Min(labelWidth / barcodeImage.Width, labelHeight / barcodeImage.Height)

                ' Calculate the new width and height of the barcode image
                Dim newWidth As Integer = CInt(barcodeImage.Width * scaleFactor)
                Dim newHeight As Integer = CInt(barcodeImage.Height * scaleFactor)

                ' Ensure the new width and height don't exceed the label size (although the scale factor should prevent this)
                If newWidth > labelWidth Then
                    newWidth = labelWidth
                End If
                If newHeight > labelHeight Then
                    newHeight = labelHeight
                End If

                ' Calculate the x and y coordinates to center the barcode image on the label
                ' Center the image horizontally and vertically
                Dim x As Integer = (labelWidth - newWidth) / 2 ' Center horizontally
                Dim y As Integer = (labelHeight - newHeight) / 2 ' Center vertically

                ' Draw the barcode image on the page with the new size (scaled to fit the label)
                e.Graphics.DrawImage(barcodeImage, x, y, newWidth, newHeight)

            Else
                ' If there's no image to print, show a message box
                MessageBox.Show("No barcode image to print.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            ' Handle any unexpected errors during the print process
            MessageBox.Show($"An error occurred while printing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try



    End Sub
    Private Sub textbox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        ' Check if the pressed key is a special character (not alphanumeric)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If TextBox4.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub IconButton63_Click(sender As Object, e As EventArgs) Handles IconButton63.Click
        Panel40.Visible = True
    End Sub
    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        ' Get the selected date from DateTimePicker1
        Dim selectedDate As Date = DateTimePicker1.Value.Date ' We use .Date to ignore the time part of the date

        ' Check if the selected date is a future date
        If selectedDate > DateTime.Now.Date Then
            MsgBox("You cannot select a future date.", MessageBoxIcon.Warning)
            ' Reset the DateTimePicker to today's date
            DateTimePicker1.Value = DateTime.Now
            Return
        End If

        ' Create a new connection to the database
        Dim con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")

        Try
            ' Open the database connection
            con.Open()

            ' Query to fetch records from 'deliverytbl' where the delivery date matches the selected date
            Dim query As String = "SELECT * FROM deliveyltbl WHERE Date_received = @Date_received"

            Using cmd As New MySqlCommand(query, con)
                ' Add the parameter with the selected date
                cmd.Parameters.AddWithValue("@Date_received", selectedDate)

                ' Execute the query and load the results into a DataTable
                Dim dt As New DataTable()
                Using adapter As New MySqlDataAdapter(cmd)
                    adapter.Fill(dt)
                End Using

                ' Bind the DataTable to DataGridView8
                DataGridView8.DataSource = dt
            End Using
        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub
    Private Sub IconButton45_Click(sender As Object, e As EventArgs) Handles IconButton45.Click
        Dim openFileDialog1 As New OpenFileDialog()

        ' Set the filter to only allow image files
        openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"

        ' Show the dialog and check if the user selected a file
        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            ' Display the selected image in the PictureBox
            PictureBox2.Image = Image.FromFile(openFileDialog1.FileName)
        End If
        ' Optionally, you can display the file path in a Label or TextBox
        ' lblFilePath.Text = openFileDialog.FileName
    End Sub

    Private Sub IconButton33_Click_2(sender As Object, e As EventArgs) Handles IconButton33.Click
        Dim openFileDialog1 As New OpenFileDialog()

        ' Set the filter to only allow image files
        openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"

        ' Show the dialog and check if the user selected a file
        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            ' Display the selected image in the PictureBox
            PictureBox3.Image = Image.FromFile(openFileDialog1.FileName)
        End If
        ' Optionally, you can display the file path in a Label or TextBox
        ' lblFilePath.Text = openFileDialog.FileName
    End Sub

    Private Sub Panel26_Paint(sender As Object, e As PaintEventArgs) Handles Panel26.Paint


    End Sub

    Private Sub Panel9_Paint(sender As Object, e As PaintEventArgs) Handles Panel9.Paint

    End Sub

    Private Sub IconButton12_Click_1(sender As Object, e As EventArgs) Handles IconButton12.Click
        Try
            ' Get the new VAT value from TextBox10
            Dim newVAT As String = PI.Text.Trim()

            ' Validate if the new VAT value is not empty or invalid
            If String.IsNullOrEmpty(newVAT) Then
                MessageBox.Show("Please enter a valid VAT value.", "Invalid VAT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Check if the VAT value is a valid decimal number
            Dim vatValue As Decimal
            If Not Decimal.TryParse(newVAT, vatValue) Then
                MessageBox.Show("Please enter a valid number for VAT.", "Invalid VAT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Check if the VAT value is between 0 and 99.99
            If vatValue < 0 OrElse vatValue > 99.99D Then
                MessageBox.Show("VAT value must be between 0 and 99.99.", "Invalid VAT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Get the current VAT value that is being updated
            Dim oldVAT As String = DataGridView9.CurrentRow.Cells("VAT").Value.ToString()

            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' Create the SQL UPDATE command to update the VAT value in the database
                Dim updateQuery As String = "UPDATE vattbl SET VAT = @NewVAT WHERE VAT = @OldVAT"

                Using cmd As New MySqlCommand(updateQuery, con)
                    cmd.Parameters.AddWithValue("@NewVAT", vatValue) ' Use decimal value instead of string
                    cmd.Parameters.AddWithValue("@OldVAT", oldVAT)

                    ' Execute the query to update the VAT in the database
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' Update the corresponding VAT value in the DataGridView9
            DataGridView9.CurrentRow.Cells("VAT").Value = vatValue.ToString("0.00") ' Format as 2 decimal places

            ' Optionally, hide Panel39 after the update is complete
            Panel39.Visible = False

            ' Show success message
            MessageBox.Show("VAT value updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Audit Trail: Log the action of updating VAT in the audit table
            Dim auditQuery As String = "INSERT INTO audittbl (DateTime, User_type, Name, Action) VALUES (NOW(), @User_type, @Name, @Action)"
            Using auditCon As New MySqlConnection(connectionString)
                auditCon.Open()

                Using cmdAudit As New MySqlCommand(auditQuery, auditCon)
                    ' Add parameters for the audit trail
                    cmdAudit.Parameters.AddWithValue("@User_type", Label45.Text)  ' Replace with logged-in user type
                    cmdAudit.Parameters.AddWithValue("@Name", Label26.Text)      ' Replace with logged-in user name
                    cmdAudit.Parameters.AddWithValue("@Action", $"Updated VAT from {oldVAT} to {vatValue.ToString("0.00")}")

                    ' Execute the audit trail query
                    cmdAudit.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As Exception
            ' Handle any exceptions
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub
    Private Sub PrintPage(sender As Object, e As Printing.PrintPageEventArgs)
        ' I-define ang starting position para sa text
        Dim yPos As Integer = 100
        Dim leftMargin As Integer = e.MarginBounds.Left
        Dim lineHeight As Integer = 20
        Dim font As New Font("Arial", 10)

        ' I-print ang header ng DataGridView
        For i As Integer = 0 To DataGridView8.Columns.Count - 1
            e.Graphics.DrawString(DataGridView8.Columns(i).HeaderText, font, Brushes.Black, leftMargin + i * 100, yPos)
        Next

        ' I-print ang bawat row ng DataGridView
        yPos += lineHeight
        For Each row As DataGridViewRow In DataGridView8.Rows
            For i As Integer = 0 To DataGridView8.Columns.Count - 1
                If Not row.IsNewRow Then
                    e.Graphics.DrawString(row.Cells(i).Value.ToString(), font, Brushes.Black, leftMargin + i * 100, yPos)
                End If
            Next
            yPos += lineHeight
        Next
    End Sub
    Private Sub IconButton60_Click(sender As Object, e As EventArgs) Handles IconButton60.Click
        Dim printDoc As New Printing.PrintDocument()

        ' I-hook ang PrintPage event
        AddHandler printDoc.PrintPage, AddressOf PrintPage

        ' I-trigger ang print dialog at print
        Dim printDialog As New PrintDialog()
        printDialog.Document = printDoc

        ' Ipakita ang print dialog
        If printDialog.ShowDialog() = DialogResult.OK Then
            printDoc.Print()
        End If
    End Sub
    Private Sub PrintSelectedRows(sender As Object, e As Printing.PrintPageEventArgs)
        ' Define the starting position for the text
        Dim yPos As Integer = 100
        Dim leftMargin As Integer = e.MarginBounds.Left
        Dim lineHeight As Integer = 20
        Dim font As New Font("Arial", 10)


        If DataGridView8.SelectedRows.Count > 0 Then

            For i As Integer = 0 To DataGridView8.Columns.Count - 1
                e.Graphics.DrawString(DataGridView8.Columns(i).HeaderText, font, Brushes.Black, leftMargin + i * 100, yPos)
            Next


            yPos += lineHeight
            For Each row As DataGridViewRow In DataGridView8.SelectedRows
                For i As Integer = 0 To DataGridView8.Columns.Count - 1

                    If Not row.IsNewRow Then
                        e.Graphics.DrawString(row.Cells(i).Value.ToString(), font, Brushes.Black, leftMargin + i * 100, yPos)
                    End If
                Next
                yPos += lineHeight
            Next
        Else

            e.Graphics.DrawString("No rows selected.", font, Brushes.Black, leftMargin, yPos)
        End If

    End Sub
    Private Sub IconButton20_Click_1(sender As Object, e As EventArgs) Handles IconButton20.Click
        Dim printDoc As New Printing.PrintDocument()

        ' I-hook ang PrintPage event
        AddHandler printDoc.PrintPage, AddressOf PrintSelectedRows

        ' I-trigger ang print dialog at print
        Dim printDialog As New PrintDialog()
        printDialog.Document = printDoc

        ' Ipakita ang print dialog
        If printDialog.ShowDialog() = DialogResult.OK Then
            printDoc.Print()
        End If
    End Sub

    Private Sub CNUM1_TextChanged(sender As Object, e As EventArgs) Handles CNUM1.TextChanged

    End Sub

    Private Sub Panel7_Paint(sender As Object, e As PaintEventArgs) Handles Panel7.Paint

    End Sub

    Private Sub Label88_Click(sender As Object, e As EventArgs) Handles Label88.Click

    End Sub

    Private Sub cbfav_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbfav.SelectedIndexChanged

    End Sub
    Private Sub textbox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        ' Check if the pressed key is a special character (not alphanumeric)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If TextBox5.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub TextBox9_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox9.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter or control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 10 characters
        If TextBox9.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 10
        End If
    End Sub
    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter or control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 10 characters
        If TextBox3.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 10
        End If
    End Sub
    Private Sub Fname1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Fname1.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter or control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 10 characters
        If Fname1.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 10
        End If
    End Sub
    Private Sub Fname1_TextChanged(sender As Object, e As EventArgs) Handles Fname1.TextChanged

    End Sub
    Private Sub Lname1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Lname1.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter or control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 10 characters
        If Lname1.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 10
        End If
    End Sub
    Private Sub Lname1_TextChanged(sender As Object, e As EventArgs) Handles Lname1.TextChanged

    End Sub
    Private Sub Mname1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Mname1.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter or control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 10 characters
        If Mname1.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 10
        End If
    End Sub
    Private Sub Mname1_TextChanged(sender As Object, e As EventArgs) Handles Mname1.TextChanged

    End Sub
    Private Sub Uname1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Uname1.KeyPress
        ' Check if the pressed key is a special character (not alphanumeric)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If Uname1.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub Uname1_TextChanged(sender As Object, e As EventArgs) Handles Uname1.TextChanged

    End Sub
    Private Sub Pwd1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Pwd1.KeyPress
        ' Check if the pressed key is a special character (not alphanumeric)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If Pwd1.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub Pwd1_TextChanged(sender As Object, e As EventArgs) Handles Pwd1.TextChanged

    End Sub
    Private Sub Cpwd1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cpwd1.KeyPress
        ' Check if the pressed key is a special character (not alphanumeric)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If Cpwd1.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub Cpwd1_TextChanged(sender As Object, e As EventArgs) Handles Cpwd1.TextChanged

    End Sub
    Private Sub Fname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Fname.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter or control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 10 characters
        If Fname.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 10
        End If
    End Sub
    Private Sub Fname_TextChanged(sender As Object, e As EventArgs) Handles Fname.TextChanged

    End Sub
    Private Sub Lname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Lname.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter or control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 10 characters
        If Lname.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 10
        End If
    End Sub
    Private Sub Lname_TextChanged(sender As Object, e As EventArgs) Handles Lname.TextChanged

    End Sub
    Private Sub Mname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Mname.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter or control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 10 characters
        If Mname.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 10
        End If
    End Sub
    Private Sub Uname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Uname.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If Uname1.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub Pwd_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Pwd.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If Pwd.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub Cpwd_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cpwd.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If Cpwd.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub tbanswer1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbanswer1.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If tbanswer1.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub tbanswer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbanswer.KeyPress
        ' Check if the pressed key is not a letter (only allow alphabetic characters)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If tbanswer.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub tbanswer_TextChanged(sender As Object, e As EventArgs) Handles tbanswer.TextChanged

    End Sub
    Private Sub SN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles SN.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If SN.Text.Length >= 15 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub SN1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles SN1.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If SN1.Text.Length >= 15 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub CNAME_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CNAME.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If CNAME.Text.Length >= 20 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub CNAME1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CNAME1.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If CNAME1.Text.Length >= 20 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub CADD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CADD.KeyPress
        ' Check if the pressed key is not a number, a letter, #, ,, space, or a control character
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsLetter(e.KeyChar) AndAlso
       e.KeyChar <> "#"c AndAlso e.KeyChar <> ","c AndAlso e.KeyChar <> " "c AndAlso
       Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a number, letter, #, , space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If CADD.Text.Length >= 20 AndAlso e.KeyChar <> "#"c AndAlso e.KeyChar <> ","c AndAlso
       e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub

    Private Sub CADD1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CADD1.KeyPress
        ' Check if the pressed key is not a number, a letter, #, , or a control character
        ' Check if the pressed key is not a number, a letter, #, ,, space, or a control character
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsLetter(e.KeyChar) AndAlso
       e.KeyChar <> "#"c AndAlso e.KeyChar <> ","c AndAlso e.KeyChar <> " "c AndAlso
       Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a number, letter, #, , space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If CADD1.Text.Length >= 20 AndAlso e.KeyChar <> "#"c AndAlso e.KeyChar <> ","c AndAlso
       e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub CNUM_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CNUM.KeyPress
        ' Allow only numeric characters and control characters (backspace, etc.)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or control
        End If

        ' Ensure that the text starts with "09"
        If CNUM.Text.Length = 0 AndAlso e.KeyChar <> "0"c Then
            e.Handled = True ' Prevent input if the first character is not 0
        End If

        If CNUM.Text.Length = 1 AndAlso e.KeyChar <> "9"c Then
            e.Handled = True ' Prevent input if the second character is not 9
        End If

        ' Ensure the length does not exceed 11 characters (including the starting "09")
        If CNUM.Text.Length >= 11 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 11
        End If
    End Sub


    Private Sub CNUM1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CNUM1.KeyPress
        ' Allow only numeric characters and control characters (backspace, etc.)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or control
        End If

        ' Ensure that the text starts with "09"
        If CNUM1.Text.Length = 0 AndAlso e.KeyChar <> "0"c Then
            e.Handled = True ' Prevent input if the first character is not 0
        End If

        If CNUM1.Text.Length = 1 AndAlso e.KeyChar <> "9"c Then
            e.Handled = True ' Prevent input if the second character is not 9
        End If

        ' Ensure the length does not exceed 11 characters (including the starting "09")
        If CNUM1.Text.Length >= 11 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 11
        End If
    End Sub

    Private Sub Texbox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If TextBox1.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub Texbox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If TextBox2.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub PN1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PN1.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If PN1.Text.Length >= 15 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub PN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PN.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If PN.Text.Length >= 15 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub Desc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Desc.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If Desc.Text.Length >= 15 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub Desc1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Desc1.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If Desc1.Text.Length >= 15 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub PN1_TextChanged(sender As Object, e As EventArgs) Handles PN1.TextChanged

    End Sub
    Private Sub SP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles SP.KeyPress
        If e.KeyChar = "."c AndAlso SP.Text.Length = 0 Then
            e.Handled = True ' Ignore the key if it's a decimal point and the TextBox is empty
            Return
        End If

        ' Allow only numeric characters and a single decimal point
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit, decimal point, or control (e.g., backspace)
        End If

        ' Ensure the length of the text does not exceed 6 digits before the decimal point
        If SP.Text.Length >= 6 AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 6 before the decimal point
        End If

        ' Ensure only one decimal point is allowed
        If SP.Text.Contains("."c) AndAlso e.KeyChar = "."c Then
            e.Handled = True ' Ignore the key if another decimal point is entered
        End If
    End Sub
    Private Sub SP1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles SP1.KeyPress
        If e.KeyChar = "."c AndAlso SP1.Text.Length = 0 Then
            e.Handled = True ' Ignore the key if it's a decimal point and the TextBox is empty
            Return
        End If

        ' Allow only numeric characters and a single decimal point
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit, decimal point, or control (e.g., backspace)
        End If

        ' Ensure the length of the text does not exceed 6 digits before the decimal point
        If SP1.Text.Length >= 6 AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 6 before the decimal point
        End If

        ' Ensure only one decimal point is allowed
        If SP1.Text.Contains("."c) AndAlso e.KeyChar = "."c Then
            e.Handled = True ' Ignore the key if another decimal point is entered
        End If
    End Sub
    Private Sub PI_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PI.KeyPress
        If e.KeyChar = "."c AndAlso PI.Text.Length = 0 Then
            e.Handled = True ' Ignore the key if it's a decimal point and the TextBox is empty
            Return
        End If

        ' Allow only numeric characters and a single decimal point
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit, decimal point, or control (e.g., backspace)
        End If

        ' Ensure the length of the text does not exceed 6 digits before the decimal point
        If PI.Text.Length >= 5 AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 6 before the decimal point
        End If

        ' Ensure only one decimal point is allowed
        If PI.Text.Contains("."c) AndAlso e.KeyChar = "."c Then
            e.Handled = True ' Ignore the key if another decimal point is entered
        End If
    End Sub
    Private Sub Textbox6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox6.KeyPress
        If e.KeyChar = "."c AndAlso TextBox6.Text.Length = 0 Then
            e.Handled = True ' Ignore the key if it's a decimal point and the TextBox is empty
            Return
        End If

        ' Allow only numeric characters and a single decimal point
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit, decimal point, or control (e.g., backspace)
        End If

        ' Ensure the length of the text does not exceed 6 digits before the decimal point
        If TextBox6.Text.Length >= 5 AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 6 before the decimal point
        End If

        ' Ensure only one decimal point is allowed
        If TextBox6.Text.Contains("."c) AndAlso e.KeyChar = "."c Then
            e.Handled = True ' Ignore the key if another decimal point is entered
        End If
    End Sub
    Private Sub SP1_TextChanged(sender As Object, e As EventArgs) Handles SP1.TextChanged

    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = ChrW(Keys.Space) Then
            e.Handled = True ' I-block ang pag-input ng space
        End If
    End Sub
    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub IconButton10_Click_1(sender As Object, e As EventArgs) Handles IconButton10.Click
        Panel40.Visible = False
    End Sub

    Private Sub IconButton21_Click_1(sender As Object, e As EventArgs) Handles IconButton21.Click
        Panel39.Visible = False
    End Sub

    Private Sub IconButton19_Click_1(sender As Object, e As EventArgs) Handles IconButton19.Click
        Try
            ' Get the new VAT value from TextBox6
            Dim newVAT As String = TextBox6.Text.Trim()

            ' Validate if the new VAT value is not empty
            If String.IsNullOrEmpty(newVAT) Then
                MessageBox.Show("Please enter a valid VAT value.", "Invalid VAT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Check if the VAT value is a valid decimal number
            Dim vatValue As Decimal
            If Not Decimal.TryParse(newVAT, vatValue) Then
                MessageBox.Show("Please enter a valid number for VAT.", "Invalid VAT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Check if the VAT value is between 0 and 99.99
            If vatValue < 0 OrElse vatValue > 99.99D Then
                MessageBox.Show("VAT value must be between 0 and 99.99.", "Invalid VAT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Open the connection within a 'Using' block
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection to the database

                ' Prepare the insert query to insert VAT value into the vattbl table
                Dim insertQuery As String = "INSERT INTO vattbl (VAT) VALUES (@VAT)"

                ' Create the MySqlCommand using the insert query
                Using cmd As New MySqlCommand(insertQuery, con)
                    ' Add the VAT parameter from the validated value
                    cmd.Parameters.AddWithValue("@VAT", vatValue)

                    ' Execute the query to insert the VAT value
                    cmd.ExecuteNonQuery()

                    ' Notify the user that the VAT has been successfully inserted
                    MsgBox("VAT value successfully inserted into the database!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using

                ' After inserting the VAT, retrieve the inserted value from the database
                Dim selectQuery As String = "SELECT VAT FROM vattbl ORDER BY VAT DESC LIMIT 1"
                Using cmdSelect As New MySqlCommand(selectQuery, con)
                    Using reader As MySqlDataReader = cmdSelect.ExecuteReader()
                        If reader.HasRows Then
                            reader.Read()
                            ' Add the retrieved VAT value to DataGridView9
                            DataGridView9.Rows.Add(reader("VAT").ToString())
                        End If
                    End Using
                End Using

                ' Audit Trail: Log the action of inserting VAT into the audit table
                Dim auditQuery As String = "INSERT INTO audittbl (DateTime, User_type, Name, Action) VALUES (NOW(), @User_type, @Name, @Action)"
                Using cmdAudit As New MySqlCommand(auditQuery, con)
                    ' Add parameters for the audit trail
                    cmdAudit.Parameters.AddWithValue("@User_type", Label45.Text)  ' Replace with logged-in user type
                    cmdAudit.Parameters.AddWithValue("@Name", Label26.Text)      ' Replace with logged-in user name
                    cmdAudit.Parameters.AddWithValue("@Action", "Inserted VAT value: " & vatValue.ToString("0.00"))

                    ' Execute the audit trail query
                    cmdAudit.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As MySqlException
            ' Handle database errors
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ' Handle general errors
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Hide the panel after operation
        Panel40.Visible = False

        ' Enable or disable IconButton63 based on DataGridView9 row count
        If DataGridView9.Rows.Count > 0 Then
            IconButton63.Enabled = False
        Else
            IconButton63.Enabled = True
        End If

    End Sub

    Private Sub DataGridView10_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView10.CellContentClick

    End Sub



    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            tbanswer.UseSystemPasswordChar = False
        Else
            tbanswer.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            tbanswer1.UseSystemPasswordChar = False
        Else
            tbanswer1.UseSystemPasswordChar = True
        End If
    End Sub



    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        ' Check if the CheckBox is checked
        If CheckBox3.Checked Then
            ' Enable the TextBox if CheckBox is checked
            TextBox10.Enabled = False
            TextBox10.Text = ""
        Else
            ' Disable the TextBox if CheckBox is unchecked
            TextBox10.Enabled = True
            TextBox10.Text = ""
        End If
    End Sub

    Private Sub TextBox10_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox10.KeyPress
        ' Check if the pressed key is not a letter, a number, or a space, and not a control character
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a letter, number, space, or a control (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 20 characters
        If TextBox10.Text.Length >= 4 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 20
        End If
    End Sub
    Private Sub TextBox10_TextChanged(sender As Object, e As EventArgs) Handles TextBox10.TextChanged

    End Sub

    Private Sub Panel38_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Panel20_Paint(sender As Object, e As PaintEventArgs) Handles Panel20.Paint

    End Sub
    Private Sub chk()
        If DataGridView1.Rows.Count = 0 Then
            ' Enable the checkbox if there are no rows
            CheckBox3.Enabled = True
        Else
            ' Disable the checkbox if there are rows
            CheckBox2.Enabled = False
        End If
    End Sub
    Private Sub Panel45_Paint(sender As Object, e As PaintEventArgs) Handles Panel45.Paint

    End Sub

    Private Sub IconButton22_Click_1(sender As Object, e As EventArgs)
        cons()
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Space) Then
            e.Handled = True ' I-block ang pag-input ng space
        End If
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub



    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then

            IconButton63.Enabled = False
            DataGridView9.Enabled = False

            If DataGridView1.Rows.Count > 0 Then
                For i As Integer = 0 To DataGridView1.Columns.Count - 1
                    DataGridView1.Rows(0).Cells(i).Value = 0
                Next
            End If

            ' Update database: set VAT = 0
            Try
                Dim con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                con.Open()

                Dim cmd As New MySqlCommand("UPDATE vattbl SET VAT = 0", con)
                cmd.ExecuteNonQuery()

                con.Close()
            Catch ex As Exception
                MessageBox.Show("Error updating VAT: " & ex.Message)
            End Try
        Else
            DataGridView9.Enabled = True
        End If
        vatts()
    End Sub
End Class