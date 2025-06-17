Imports MySql.Data.MySqlClient
Public Class PurchaseOrderF
    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
    Private Sub po()
        opencon()

        Dim table As New DataTable()
        Dim adpt As New MySqlDataAdapter("SELECT * FROM Purchaseordertbl", con)
        adpt.Fill(table)

        DataGridView1.DataSource = table
        DataGridView1.ClearSelection()
        con.Close()
    End Sub
    Private Sub IconButton3_Click(sender As Object, e As EventArgs) Handles IconButton3.Click
        Dim nextForm As New MainMenuf()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton7_Click(sender As Object, e As EventArgs) Handles IconButton7.Click


        If cnb.Text = "" OrElse snb.Text = "" OrElse canb.Text = "" OrElse db.Text = "" OrElse DTP1.Text = "" OrElse qb.Text = "" OrElse cpb.Text = "" OrElse tab.Text = "" Then
            MsgBox("MISSING INPUT FOR PURCHASE ORDER")
        Else
            Try
                opencon()
                Using cmd As New MySqlCommand("INSERT INTO Purchaseordertbl (PO_ID, Company_name, Supplier_name, Category_name, Description, Date, Quantity, Cost_price, Total_amount) VALUES (@PO_ID, @Company_name, @Supplier_name, @Category_name, @Description, @Date, @Quantity, @Cost_price, @Total_amount)", con)
                    cmd.Parameters.AddWithValue("@PO_ID", Guid.NewGuid().ToString()) ' Assuming PO_ID is a unique identifier
                    cmd.Parameters.AddWithValue("@Company_name", cnb.Text)
                    cmd.Parameters.AddWithValue("@Supplier_name", snb.Text)
                    cmd.Parameters.AddWithValue("@Category_name", canb.Text)
                    cmd.Parameters.AddWithValue("@Description", db.Text)
                    cmd.Parameters.AddWithValue("@Date", DTP1.Value) ' Use .Value for DateTime
                    cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(qb.Text)) ' Ensure Quantity is an integer
                    cmd.Parameters.AddWithValue("@Cost_price", Convert.ToDecimal(cpb.Text)) ' Ensure Cost Price is a decimal
                    cmd.Parameters.AddWithValue("@Total_amount", Convert.ToDecimal(tab.Text)) ' Ensure Total Amount is a decimal

                    cmd.ExecuteNonQuery()
                End Using

                MsgBox("Purchase Order Successfully Added!")
            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
            Finally
                con.Close()
            End Try
        End If
        po()
    End Sub

    Private Sub IconButton15_Click(sender As Object, e As EventArgs) Handles IconButton15.Click
        po()
    End Sub

    Private Sub IconButton6_Click(sender As Object, e As EventArgs) Handles IconButton6.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Retrieve the PO_ID from the selected row
            Dim poID As String = If(selectedRow.Cells("PO_ID").Value?.ToString(), "")

            ' Confirm deletion
            If MsgBox($"Are you sure you want to delete the purchase order with ID: {poID}?", MsgBoxStyle.YesNo, "Confirm Delete") = MsgBoxResult.No Then
                Return
            End If

            Try
                opencon() ' Ensure this function properly opens the connection

                Using cmd As New MySqlCommand("DELETE FROM Purchaseordertbl WHERE PO_ID = @PO_ID", con)
                    cmd.Parameters.AddWithValue("@PO_ID", poID) ' Use PO_ID as the unique identifier

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MsgBox("Purchase Order successfully deleted!")
                        ' Optionally, refresh the DataGridView to reflect the changes
                        ' Assuming you have a method to reload data
                    Else
                        MsgBox("No purchase order found with that ID.")
                    End If
                End Using

            Catch ex As MySqlException
                MsgBox("Database error: " & ex.Message)
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message)
            Finally
                If con.State = ConnectionState.Open Then
                    con.Close() ' Ensure the connection is closed
                End If
            End Try
        Else
            MsgBox("Please select a purchase order to delete.")
        End If
        po()
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Populate TextBoxes with the values from the selected row
            cnb.Text = selectedRow.Cells("Company_name").Value.ToString()
            snb.Text = selectedRow.Cells("Supplier_name").Value.ToString()
            canb.Text = selectedRow.Cells("Category_name").Value.ToString()
            db.Text = selectedRow.Cells("Description").Value.ToString()
            DTP1.Value = Convert.ToDateTime(selectedRow.Cells("Date").Value) ' Assuming this is a DateTime column
            qb.Text = selectedRow.Cells("Quantity").Value.ToString()
            cpb.Text = selectedRow.Cells("Cost_price").Value.ToString()
            tab.Text = selectedRow.Cells("Total_amount").Value.ToString()
        Else
            ' Clear TextBoxes if no row is selected
            cnb.Text = ""
            snb.Text = ""
            canb.Text = ""
            db.Text = ""
            DTP1.Value = DateTime.Now ' Reset to current date or a default value
            qb.Text = ""
            cpb.Text = ""
            tab.Text = ""
        End If
    End Sub

    Private Sub IconButton5_Click(sender As Object, e As EventArgs) Handles IconButton5.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Retrieve the PO_ID from the selected row
            Dim poID As String = If(selectedRow.Cells("PO_ID").Value?.ToString(), "")

            ' Confirm update
            If MsgBox($"Are you sure you want to update the purchase order with ID: {poID}?", MsgBoxStyle.YesNo, "Confirm Update") = MsgBoxResult.No Then
                Return
            End If

            ' Validate inputs
            If String.IsNullOrWhiteSpace(cnb.Text) OrElse
               String.IsNullOrWhiteSpace(snb.Text) OrElse
               String.IsNullOrWhiteSpace(canb.Text) OrElse
               String.IsNullOrWhiteSpace(db.Text) OrElse
               String.IsNullOrWhiteSpace(qb.Text) OrElse
               String.IsNullOrWhiteSpace(cpb.Text) OrElse
               String.IsNullOrWhiteSpace(tab.Text) Then

                MsgBox("MISSING INPUT FOR PURCHASE ORDER")
                Return
            End If

            Try
                opencon() ' Ensure this function properly opens the connection

                ' Prepare the SQL command to update the purchase order
                Using cmd As New MySqlCommand("UPDATE Purchaseordertbl SET Company_name = @Company_name, Supplier_name = @Supplier_name, Category_name = @Category_name, Description = @Description, Date = @Date, Quantity = @Quantity, Cost_price = @Cost_price, Total_amount = @Total_amount WHERE PO_ID = @PO_ID", con)

                    ' Assign values to the parameters
                    cmd.Parameters.AddWithValue("@PO_ID", poID) ' Unique identifier for the purchase order
                    cmd.Parameters.AddWithValue("@Company_name", cnb.Text)
                    cmd.Parameters.AddWithValue("@Supplier_name", snb.Text)
                    cmd.Parameters.AddWithValue("@Category_name", canb.Text)
                    cmd.Parameters.AddWithValue("@Description", db.Text)
                    cmd.Parameters.AddWithValue("@Date", DTP1.Value) ' Use .Value for DateTime
                    cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(qb.Text)) ' Convert Quantity to integer
                    cmd.Parameters.AddWithValue("@Cost_price", Convert.ToDecimal(cpb.Text)) ' Convert Cost Price to decimal
                    cmd.Parameters.AddWithValue("@Total_amount", Convert.ToDecimal(tab.Text)) ' Convert Total Amount to decimal

                    ' Execute the command
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MsgBox("Purchase Order successfully updated!")
                    Else
                        MsgBox("No purchase order found with that ID.")
                    End If
                End Using

            Catch ex As MySqlException
                MsgBox("Database error: " & ex.Message)
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message)
            Finally
                If con.State = ConnectionState.Open Then
                    con.Close() ' Ensure the connection is closed
                End If
            End Try
        Else
            MsgBox("Please select a purchase order to update.")
        End If
        po()
    End Sub

    Private Sub IconButton21_Click(sender As Object, e As EventArgs) Handles IconButton21.Click
        cnb.Text = ""
        snb.Text = ""
        canb.Text = ""
        db.Text = ""
        DTP1.Value = DateTime.Now ' Reset to current date or a default value
        qb.Text = ""
        cpb.Text = ""
        tab.Text = ""

        ' Clear any selection in DataGridView
        DataGridView1.ClearSelection()
    End Sub
End Class