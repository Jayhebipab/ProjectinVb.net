Imports MySql.Data.MySqlClient
Public Class InventoryF
    Private Sub IconButton3_Click(sender As Object, e As EventArgs) Handles IconButton3.Click
        Dim nextForm As New MainMenuf()
        nextForm.Show()
        Me.Hide()
        For Each row As DataGridViewRow In DataGridView4.Rows
            If Not row.IsNewRow Then
                ' Get the barcode value from DataGridView
                Dim barcodeInGrid As String = row.Cells(0).Value.ToString()

                ' Compare with the TextBox
                If barcodeInGrid = TextBox13.Text Then
                    ' Use MySQL connection
                    Dim conn As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                    Dim cmd As New MySqlCommand("DELETE FROM quantitytbl1 WHERE barcode = @barcode", conn)
                    cmd.Parameters.AddWithValue("@barcode", barcodeInGrid)
                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                    Finally
                        conn.Close()
                    End Try

                    Exit For
                End If
            End If
        Next

        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' Get barcode from TextBox15
                Dim barcodeInput As String = TextBox15.Text.Trim()
                ' Check if barcode exists
                Dim checkCmd As New MySqlCommand("SELECT COUNT(*) FROM quantitytbl1 WHERE Barcode = @Barcode", con)
                checkCmd.Parameters.AddWithValue("@Barcode", barcodeInput)
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                If count = 0 Then
                    Return
                End If

                ' Delete the entry from quantitytbl1
                Dim deleteCmd As New MySqlCommand("DELETE FROM quantitytbl1 WHERE Barcode = @Barcode", con)
                deleteCmd.Parameters.AddWithValue("@Barcode", barcodeInput)
                deleteCmd.ExecuteNonQuery()



            End Using

            ' Clear UI and refresh DataGridView
            TextBox15.Clear()
            DataGridView1.DataSource = Nothing
            DataGridView1.Rows.Clear()
            DataGridView1.Columns.Clear()

        Catch ex As MySqlException


        Catch ex As Exception

        End Try


    End Sub
    Private Sub dep()
        opencon()

        Dim table As New DataTable()
        Dim adpt As New MySqlDataAdapter("SELECT * FROM quantitytbl", con)
        adpt.Fill(table)

        DataGridView3.DataSource = table
        con.Close()
    End Sub

    Private Sub list()
        opencon()

        Dim tabledq As New DataTable()
        Dim adptdq As New MySqlDataAdapter("SELECT * FROM deliveyltbl ", con)
        adptdq.Fill(tabledq)

        DataGridView6.DataSource = tabledq
        con.Close()
    End Sub
    Private Sub DateTimePicker2_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker2.ValueChanged
        ' Get the selected date from DateTimePicker2
        Dim selectedDate As Date = DateTimePicker2.Value.Date ' We use .Date to ignore the time part of the date

        ' Check if the selected date is a future date
        If selectedDate > DateTime.Now.Date Then
            MsgBox("You cannot select a future date.", MessageBoxIcon.Warning)
            ' Reset the DateTimePicker to today's date
            DateTimePicker2.Value = DateTime.Now
            Return
        End If
        DTP1.Value = DateTimePicker2.Value
        ' Add any additional code to handle the date selection here

    End Sub
    Private Sub cp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cp.KeyPress
        If e.KeyChar = "."c AndAlso cp.Text.Length = 0 Then
            e.Handled = True ' Ignore the key if it's a decimal point and the TextBox is empty
            Return
        End If

        ' Allow only numeric characters and a single decimal point
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit, decimal point, or control (e.g., backspace)
        End If

        ' Ensure the length of the text does not exceed 6 digits before the decimal point
        If cp.Text.Length >= 6 AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 6 before the decimal point
        End If

        ' Ensure only one decimal point is allowed
        If cp.Text.Contains("."c) AndAlso e.KeyChar = "."c Then
            e.Handled = True ' Ignore the key if another decimal point is entered
        End If
    End Sub
    Private Sub quant()
        opencon()

        Dim tablepq As New DataTable()
        Dim adptpq As New MySqlDataAdapter("SELECT * FROM quantitytbl1", con)
        adptpq.Fill(tablepq)

        DataGridView4.DataSource = tablepq
        con.Close()
    End Sub
    Private Sub restock()
        opencon()

        Dim tabledqr As New DataTable()
        Dim adptdqr As New MySqlDataAdapter("SELECT * FROM restocktbl ", con)
        adptdqr.Fill(tabledqr)

        DataGridView7.DataSource = tabledqr
        con.Close()
    End Sub
    Private Sub hulog()

        Try
            opencon()

            For Each row1 As DataGridViewRow In DataGridView1.Rows
                If row1.IsNewRow Then Continue For

                Dim qty1 As Integer = Convert.ToInt32(row1.Cells("Quantity").Value)
                Dim barcode1 As String = row1.Cells("Barcode").Value.ToString().Trim().ToLower()

                ' If quantity is 0
                If qty1 = 0 Then
                    For Each row7 As DataGridViewRow In DataGridView7.Rows
                        If row7.IsNewRow Then Continue For

                        Dim barcode7 As String = row7.Cells("Barcode").Value.ToString().Trim().ToLower()

                        If barcode1 = barcode7 Then
                            ' Step 1: Get quantity, cost, and selling price from DataGridView7
                            Dim restockQty As Integer = Convert.ToInt32(row7.Cells("Quantity").Value)
                            Dim restockCostPrice As Decimal = Convert.ToDecimal(row7.Cells(6).Value) ' Cost_price column
                            Dim restockSellPrice As Decimal = Convert.ToDecimal(row7.Cells(7).Value) ' Selling_price column

                            ' Step 2: Update quantitytbl1 - Quantity only
                            Dim updateQtyCmd As New MySqlCommand("
                        UPDATE quantitytbl1 
                        SET Quantity = @Quantity 
                        WHERE Barcode = @Barcode", con)
                            updateQtyCmd.Parameters.AddWithValue("@Quantity", restockQty)
                            updateQtyCmd.Parameters.AddWithValue("@Barcode", barcode1)
                            updateQtyCmd.ExecuteNonQuery()

                            ' Step 3: Update producttbl using prices from DataGridView7
                            Dim updatePriceCmd As New MySqlCommand("
                        UPDATE producttbl 
                        SET Cost_Price = @Cost_Price, Selling_price = @Selling_price 
                        WHERE Barcode = @Barcode", con)
                            updatePriceCmd.Parameters.AddWithValue("@Cost_Price", restockCostPrice)
                            updatePriceCmd.Parameters.AddWithValue("@Selling_price", restockSellPrice)
                            updatePriceCmd.Parameters.AddWithValue("@Barcode", barcode1)
                            updatePriceCmd.ExecuteNonQuery()

                            ' Step 4: Update DataGridView1 UI
                            row1.Cells("Quantity").Value = restockQty
                            row1.Cells(6).Value = restockCostPrice
                            row1.Cells(5).Value = restockSellPrice

                            ' Step 5: Delete from restocktbl
                            Dim deleteCmd As New MySqlCommand("DELETE FROM restocktbl WHERE Barcode = @Barcode", con)
                            deleteCmd.Parameters.AddWithValue("@Barcode", barcode1)
                            deleteCmd.ExecuteNonQuery()

                        End If
                    Next
                End If
            Next

        Catch ex As Exception

        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try





    End Sub
    Private Sub InventoryF_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If DataGridView1.Columns.Contains("Cost_Price") Then
            DataGridView1.Columns("Cost_Price").DefaultCellStyle.Format = "N2"
        End If
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridView1.AllowUserToResizeColumns = False
        DataGridView1.MultiSelect = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DateTimePicker2.MinDate = New Date(2024, 11, 23)
        DateTimePicker1.MinDate = New Date(2024, 11, 23)
        TextBox11.Text = "1"
        IconButton2.Enabled = False
        IconButton7.Enabled = False
        IconButton6.Enabled = False
        IconButton18.Enabled = False
        IconButton5.Enabled = False
        IconButton1.Enabled = False
        Label17.Text = username1
        Label19.Text = position
        Try
            ' Ensure the connection is open
            opencon() ' Ensure this method correctly opens the connection to the database

            ' Get the barcode value from TextBox1
            Dim barcode As String = TextBox3.Text



            ' Loop through the rows in DataGridView6 to find the barcode
            For Each row As DataGridViewRow In DataGridView6.Rows
                ' Check if the current row's barcode matches the entered barcode (TextBox1)
                If row.Cells("Barcode").Value.ToString() = barcode Then
                    ' If barcode matches, get the corresponding Batch_No from the same row
                    Dim batchNo As Integer = Convert.ToInt32(row.Cells("Batch_No").Value)

                    ' Set the Batch_No in TextBox7
                    TextBox7.Text = batchNo.ToString()

                    ' Set barcodeFound to True, since we found the barcode

                End If
            Next

            ' If the barcode was not found, display a message


        Catch ex As Exception
            ' Handle any other exceptions
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try


        list()

        quant()
        restock()

        opencon()

        Dim tabledq As New DataTable()
        Dim adptdq As New MySqlDataAdapter("SELECT * FROM deliveyltbl ", con)
        adptdq.Fill(tabledq)

        DataGridView6.DataSource = tabledq
        con.Close()


        opencon()

        Dim tablep As New DataTable()
        Dim adptp As New MySqlDataAdapter("SELECT * FROM producttbl", con)
        adptp.Fill(tablep)

        DataGridView5.DataSource = tablep
        con.Close()
        opencon()

        Dim table As New DataTable()
        Dim adpt As New MySqlDataAdapter("SELECT * FROM deliveryrtbl", con)
        adpt.Fill(table)

        DataGridView2.DataSource = table
        con.Close()
        opencon()
        Dim commande As New MySqlCommand("SELECT * FROM suppliertbl", con)
        Dim adapterre As New MySqlDataAdapter(commande)
        Dim tableee As New DataTable()
        adapterre.Fill(tableee) ' Fixed typo here

        ' Clear existing items in the ComboBox
        CNAM.Items.Clear()
        con.Close()
        Try
            ' Open the database connection
            opencon()

            ' Create a command to fetch data from the suppliertbl table
            Dim commanded As New MySqlCommand("SELECT * FROM suppliertbl", con)
            Dim adapterred As New MySqlDataAdapter(commanded)

            ' Create a DataTable to hold the results of the query
            Dim tableeed As New DataTable()
            adapterred.Fill(tableeed)

            ' Clear existing items in the ComboBox
            CNAM.Items.Clear()

            ' Check if the DataTable contains rows
            If tableeed.Rows.Count > 0 Then
                CNAM.Text = ""  ' Clear the text box in the ComboBox

                ' Loop through each row in the DataTable
                For Each row As DataRow In tableeed.Rows
                    ' Add the Company_name value to the ComboBox
                    CNAM.Items.Add(row("Company_name").ToString())
                Next

                ' Optionally select the first item in the ComboBox
                CNAM.SelectedIndex = 0
            Else
                ' Handle case when no data is returned (optional)

            End If

            ' Close the connection after the operation is completed
            con.Close()

        Catch ex As MySqlException
            ' Handle database-related errors
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            ' Handle any other unexpected errors
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure the connection is always closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try


        Try
            ' Open the database connection
            opencon()

            ' Create a new MySQL command to select data from the category table
            Dim commander As New MySqlCommand("SELECT * FROM categorytbl", con)

            ' Create a new DataAdapter for fetching the data
            Dim adapterrer As New MySqlDataAdapter(commander)

            ' Create a DataTable to hold the data
            Dim tableeer As New DataTable()

            ' Fill the DataTable with the results from the query
            adapterrer.Fill(tableeer)

            ' Clear existing items in the ComboBox
            ComboBox3.Items.Clear()

            ' Check if any data was returned from the database
            If tableeer.Rows.Count > 0 Then
                ComboBox3.Text = ""  ' Clear the text in ComboBox

                ' Loop through the rows and add the category name to the ComboBox
                For Each row As DataRow In tableeer.Rows
                    ComboBox3.Items.Add(row("category_name").ToString())
                Next

                ' Optionally, select the first item in the ComboBox
                ComboBox3.SelectedIndex = 0
            End If

            ' Close the connection after operation
            con.Close()

        Catch ex As MySqlException
            ' Handle MySQL database connection errors
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            ' Handle other errors
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure that the connection is closed in case of an error or completion
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

        Try
            ' Open the database connection
            opencon()

            ' Create the command to fetch data from the supplier table
            Dim commandes As New MySqlCommand("SELECT * FROM suppliertbl", con)

            ' Create the data adapter
            Dim adapterres As New MySqlDataAdapter(commandes)

            ' Create a new DataTable to hold the data
            Dim tableees As New DataTable()

            ' Fill the DataTable with the data from the query
            adapterres.Fill(tableees)  ' Corrected variable name to adapterres

            ' Clear existing items in the ComboBox
            ComboBox2.Items.Clear()

            ' Check if any data was returned
            If tableees.Rows.Count > 0 Then
                ComboBox2.Text = ""  ' Clear the text in ComboBox
                For Each row As DataRow In tableees.Rows
                    ' Add each company name to the ComboBox
                    ComboBox2.Items.Add(row("Company_name").ToString())
                Next
                ComboBox2.SelectedIndex = 0  ' Optionally, select the first item in the ComboBox
            End If

            ' Close the connection
            con.Close()

        Catch ex As MySqlException
            ' Handle MySQL database connection errors
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            ' Handle general errors
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Ensure that the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

        Try
            ' Open the database connection if it's not already open
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If

            ' Query to get the last inserted Delivery_id
            Dim queryCmd As New MySqlCommand("SELECT MAX(Delivery_receipt) FROM deliveyltbl", con) ' Renamed 'cmd' to 'queryCmd'
            Dim currentId As Object = queryCmd.ExecuteScalar()

            ' Check if a value was returned
            If currentId IsNot Nothing AndAlso currentId IsNot DBNull.Value Then
                ' Add 1 to the current Delivery_id to get the next one
                Dim nextId As Integer = Convert.ToInt32(currentId) + 1
                TextBox2.Text = nextId.ToString()
            Else
                ' If no ID is found (empty table), set it to 1 as the first ID
                TextBox2.Text = "1"
            End If

        Catch ex As MySqlException
            ' Database error (connection, SQL issues)
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            ' General error
            MsgBox("An error occurred: " & ex.Message)
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub IconButton4_Click(sender As Object, e As EventArgs) Handles IconButton4.Click
        cp.Text = ""
        barc.Text = ""
        QU.Text = ""
        Panel4.Visible = True
        DataGridView1.ClearSelection()
    End Sub

    Private Sub IconButton10_Click(sender As Object, e As EventArgs) Handles IconButton10.Click
        Panel4.Visible = False
    End Sub

    Private Sub IconButton9_Click(sender As Object, e As EventArgs) Handles IconButton9.Click
        Try
            If String.IsNullOrWhiteSpace(CNAM.Text) OrElse
       String.IsNullOrWhiteSpace(DateTimePicker2.Text) Then
                MsgBox("MISSING INPUT")
                Exit Sub
            End If

            Dim deliveryDate As Date
            If Not DateTime.TryParse(DateTimePicker2.Text, deliveryDate) Then
                MsgBox("Invalid delivery date format.")
                Exit Sub
            End If

            Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                con.Open()

                ' 1. Insert new delivery record
                Using cmd As New MySqlCommand("INSERT INTO deliveryrtbl (Company_name, Delivery_date) VALUES (@company_name, @delivery_date)", con)
                    cmd.Parameters.AddWithValue("@company_name", CNAM.Text)
                    cmd.Parameters.AddWithValue("@delivery_date", deliveryDate)
                    cmd.ExecuteNonQuery()
                End Using

                ' 2. Get next Delivery_receipt
                Using cmd As New MySqlCommand("SELECT IFNULL(MAX(Delivery_receipt), 0) + 1 FROM deliveyltbl", con)
                    TextBox8.Text = cmd.ExecuteScalar().ToString()
                End Using

                ' 3. Get last inserted Company_name
                Using cmd As New MySqlCommand("SELECT Company_name FROM deliveryrtbl ORDER BY Delivery_id DESC LIMIT 1", con)
                    Dim result = cmd.ExecuteScalar()
                    TextBox1.Text = If(result IsNot Nothing, result.ToString(), "")
                End Using

                MsgBox("Delivery successfully added!")
                DataGridView1.DataSource = Nothing
                DataGridView1.Rows.Clear()
                DataGridView1.Columns.Clear()

            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        End Try

        Panel4.Visible = False
        CNAM.Items.Clear()
        TA.Text = ""
        IconButton4.Enabled = False
        DataGridView1.Enabled = False
        IconButton7.Enabled = False
        IconButton6.Enabled = False
        IconButton5.Enabled = False
        IconButton1.Enabled = True
        IconButton15.Enabled = False
        IconButton18.Enabled = False
        IconButton2.Enabled = False
    End Sub


    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Try
            Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
                con.Open()
                Dim command As New MySqlCommand("SELECT * FROM suppliertbl WHERE Company_name = @Company_name", con)
                command.Parameters.AddWithValue("@Company_name", TextBox1.Text.Trim())

                Dim adapter As New MySqlDataAdapter(command)
                Dim table As New DataTable()
                adapter.Fill(table)

                If table.Rows.Count > 0 Then
                    Dim row As DataRow = table.Rows(0)
                    SN.Text = If(row("Supplier_name") IsNot DBNull.Value, row("Supplier_name").ToString(), String.Empty)
                    CNUM.Text = If(row("Contact_number") IsNot DBNull.Value, row("Contact_number").ToString(), String.Empty)
                    ADD.Text = If(row("Address") IsNot DBNull.Value, row("Address").ToString(), String.Empty)
                Else
                    ' Clear textboxes if no matching record is found
                    SN.Clear()
                    CNUM.Clear()
                    ADD.Clear()
                End If
            End Using
        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub barc_TextChanged(sender As Object, e As EventArgs) Handles barc.TextChanged
        ' Hakbang 1: Kunin ang barcode mula sa TextBox1
        Dim barcode As String = barc.Text.Trim()

        If Not String.IsNullOrEmpty(barcode) Then
            ' Hakbang 2: I-prepare ang connection string at query
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co" ' Ipalit ang iyong connection string
            Dim query As String = "SELECT Selling_price FROM producttbl WHERE barcode = @barcode"

            Using conn As New MySqlConnection(connectionString)
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@barcode", barcode)

                Try
                    ' Hakbang 3: Buksan ang koneksyon at kunin ang selling_price
                    conn.Open()
                    Dim result As Object = cmd.ExecuteScalar()

                    ' Hakbang 4: Ipakita ang selling price sa TextBox12
                    If result IsNot Nothing Then
                        TextBox12.Text = result.ToString()
                    Else
                        TextBox12.Text = "Product not found."
                    End If
                Catch ex As Exception
                    ' Ipakita ang error kung may mangyaring problema
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using
        Else
            ' Kung walang laman ang TextBox1, linisin ang TextBox12
            TextBox12.Clear()
        End If
        Try
            opencon()
            Dim commandK As New MySqlCommand("SELECT * FROM producttbl", con)
            Dim adapterK As New MySqlDataAdapter(commandK)
            Dim tableeeK As New DataTable()
            adapterK.Fill(tableeeK)

            Dim searchedBarcode As String = barc.Text.Trim() ' Get barcode from the TextBox

            ' Clear existing text in the TextBoxes and ComboBox
            Desc.Clear()
            CN.SelectedIndex = -1 ' Clear the selected item in the ComboBox

            ' Check if the table has rows
            If tableeeK.Rows.Count > 0 Then
                For Each row As DataRow In tableeeK.Rows
                    If row("Barcode").ToString() = searchedBarcode Then
                        ' If barcode is found, display corresponding Category_name and Description
                        Dim categoryName As String = row("Cat_name").ToString()
                        Desc.Text = row("Product_Name").ToString()
                        ' Set the ComboBox value if it matches any item in the ComboBox
                        If Not CN.Items.Contains(categoryName) Then
                            CN.Items.Add(categoryName) ' Add if not present
                        End If
                        CN.SelectedItem = categoryName ' Select the category

                        Exit For ' Exit the loop after finding the first match
                    End If
                Next
            End If

        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Ensure the connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try



    End Sub

    Private Sub IconButton1_Click(sender As Object, e As EventArgs) Handles IconButton1.Click
        Panel2.Visible = True
    End Sub

    Private Sub Panel11_Paint(sender As Object, e As PaintEventArgs)

    End Sub
    Private Sub IBAR_KeyPress(sender As Object, e As KeyPressEventArgs) Handles IBAR.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or control
        End If

        ' Prevent entering more than 4 characters
        If IBAR.Text.Length >= 10 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 4

        End If
    End Sub

    Private Sub IBAR_TextChanged(sender As Object, e As EventArgs) Handles IBAR.TextChanged


        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection

                ' Query to select all products
                Dim commandK As New MySqlCommand("SELECT * FROM producttbl", con)
                Dim adapterK As New MySqlDataAdapter(commandK)
                Dim tableeeK As New DataTable()
                adapterK.Fill(tableeeK)

                Dim searchedBarcode As String = IBAR.Text.Trim() ' Get barcode from the TextBox

                ' Clear existing text in the TextBoxes and ComboBox
                Descr.Clear()
                ComboBox1.SelectedIndex = -1 ' Clear the selected item in the ComboBox

                ' Check if the table has rows
                If tableeeK.Rows.Count > 0 Then
                    ' Use DataRowCollection's Find method for faster lookups (if you set up a primary key)
                    For Each row As DataRow In tableeeK.Rows
                        If row("Barcode").ToString() = searchedBarcode Then
                            ' If barcode is found, display corresponding Category_name and Description
                            Dim categoryName As String = row("Cat_name").ToString()
                            Descr.Text = row("Product_Name").ToString()

                            ' Set the ComboBox value if it matches any item in the ComboBox
                            If Not ComboBox1.Items.Contains(categoryName) Then
                                ComboBox1.Items.Add(categoryName) ' Add if not present
                            End If
                            ComboBox1.SelectedItem = categoryName ' Select the category

                            Exit For ' Exit the loop after finding the first match
                        End If
                    Next
                Else
                    MessageBox.Show("No products found in the database.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        End Try

        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using statement to ensure proper resource management
            Using con As New MySqlConnection(connectionString)
                con.Open()  ' Open the connection

                ' Query to get the last inserted barcode
                Dim cmd As New MySqlCommand("SELECT Barcode FROM quantitytbl1 ORDER BY Barcode DESC LIMIT 1", con)
                Dim barcode As Object = cmd.ExecuteScalar()

                ' Check if a barcode was retrieved
                If barcode IsNot Nothing Then
                    barc.Text = barcode.ToString()
                Else
                    barc.Text = String.Empty  ' Clear the textbox if no barcode is found
                End If
            End Using
        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        End Try





    End Sub

    Private Sub PAI_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub IconButton14_Click(sender As Object, e As EventArgs) Handles IconButton14.Click
        Try
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' Validate quantity is numeric
                If Not Integer.TryParse(Quan.Text, Nothing) Then
                    MsgBox("Please enter a valid quantity.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Validate barcode is not empty
                If String.IsNullOrWhiteSpace(IBAR.Text) Then
                    MsgBox("Please enter a valid barcode.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' ✅ Step 0: Check if barcode already exists in quantitytbl1
                Dim checkCmd As New MySqlCommand("SELECT COUNT(*) FROM quantitytbl1 WHERE Barcode = @Barcode", con)
                checkCmd.Parameters.AddWithValue("@Barcode", IBAR.Text.Trim())

                Dim existingCount As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If existingCount > 0 Then
                    MsgBox("This barcode already has a quantity entry. Duplicate entries are not allowed.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' ✅ Step 1: Insert the quantity
                Using cmd As New MySqlCommand("INSERT INTO quantitytbl1 (`Barcode`, `Quantity`) VALUES (@Barcode, @Quantity)", con)
                    cmd.Parameters.AddWithValue("@Barcode", IBAR.Text.Trim())
                    cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(Quan.Text))
                    cmd.ExecuteNonQuery()

                    MsgBox("Quantity successfully added!", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Update UI state
                    IconButton1.Enabled = False
                    IconButton7.Enabled = False
                    IconButton7.Enabled = True
                    IconButton3.Enabled = True
                    Panel2.Visible = False
                End Using

                ' ✅ Step 2: Set values to other controls
                barc.Text = IBAR.Text.Trim()
                TextBox15.Text = IBAR.Text.Trim()
                TextBox13.Text = IBAR.Text.Trim()
                QU.Text = Quan.Text

                ' ✅ Step 3: Audit Trail
                Using auditCmd As New MySqlCommand("INSERT INTO audittbl (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, @Action)", con)
                    auditCmd.Parameters.AddWithValue("@User_type", Label19.Text)
                    auditCmd.Parameters.AddWithValue("@Name", Label17.Text)
                    auditCmd.Parameters.AddWithValue("@Action", "Added quantity record for barcode: " & IBAR.Text & ", Quantity: " & Quan.Text)
                    auditCmd.ExecuteNonQuery()
                End Using
            End Using

            ' ✅ Step 4: Refresh DataGridView1 to show only the newly added barcode entry
            Using con2 As New MySqlConnection(connectionString)
                con2.Open()

                Dim query As String = "SELECT producttbl.Barcode, Product_Name, Cat_name, Color, ItemSize, Selling_price, Cost_price, Quantity " &
                              "FROM producttbl " &
                              "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                              "WHERE producttbl.Barcode = @Barcode"

                Using cmd2 As New MySqlCommand(query, con2)
                    cmd2.Parameters.AddWithValue("@Barcode", IBAR.Text.Trim())

                    Dim adapter As New MySqlDataAdapter(cmd2)
                    Dim ds As New DataSet()
                    adapter.Fill(ds)

                    If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                        DataGridView1.DataSource = ds.Tables(0)

                        For Each column As DataGridViewColumn In DataGridView1.Columns
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        Next
                    Else
                        DataGridView1.DataSource = Nothing
                        MsgBox("No matching product found after insert.", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Optional cleanup here
        End Try



    End Sub

    Private Sub IconButton13_Click(sender As Object, e As EventArgs) Handles IconButton13.Click
        Panel2.Visible = False
    End Sub

    Private Sub IconButton5_Click(sender As Object, e As EventArgs) Handles IconButton5.Click
        Try
            ' --- Connection string ---
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' --- User and input info ---
                Dim barcode As String = barc.Text.Trim()
                Dim userType As String = Label19.Text
                Dim userName As String = Label17.Text

                ' --- Parse & Validate Input Fields ---
                Dim newCostPrice As Decimal
                Dim newSellingPrice As Decimal
                Dim newQuantity As Integer

                ' Validate Cost Price
                If Not Decimal.TryParse(cp.Text, newCostPrice) Then
                    MsgBox("Invalid cost price input.", MsgBoxStyle.Critical, "Input Error")
                    Return
                End If

                ' Validate Selling Price
                If Not Decimal.TryParse(TextBox12.Text, newSellingPrice) Then
                    MsgBox("Invalid selling price input.", MsgBoxStyle.Critical, "Input Error")
                    Return
                End If

                ' Validate Quantity
                If Not Integer.TryParse(QU.Text, newQuantity) Then
                    MsgBox("Invalid quantity input.", MsgBoxStyle.Critical, "Input Error")
                    Return
                End If

                ' --- STEP 1: Fetch existing values ---
                Dim oldCostPrice As Decimal = 0
                Dim oldSellingPrice As Decimal = 0
                Dim oldQuantity As Integer = -1

                ' Fetch prices from producttbl
                Using cmd As New MySqlCommand("SELECT Cost_Price, Selling_price FROM producttbl WHERE Barcode = @Barcode", con)
                    cmd.Parameters.AddWithValue("@Barcode", barcode)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            oldCostPrice = Convert.ToDecimal(reader("Cost_Price"))
                            oldSellingPrice = Convert.ToDecimal(reader("Selling_price"))
                        Else
                            MsgBox("Barcode not found in producttbl.", MsgBoxStyle.Exclamation, "Not Found")
                            Return
                        End If
                    End Using
                End Using

                ' Fetch quantity from quantitytbl1
                Using cmdQty As New MySqlCommand("SELECT Quantity FROM quantitytbl1 WHERE Barcode = @Barcode", con)
                    cmdQty.Parameters.AddWithValue("@Barcode", barcode)
                    Dim result = cmdQty.ExecuteScalar()
                    If result IsNot Nothing Then
                        oldQuantity = Convert.ToInt32(result)
                    Else
                        MsgBox("Barcode not found in quantitytbl1. Cannot update quantity.", MsgBoxStyle.Exclamation, "Not Found")
                        Return
                    End If
                End Using

                ' --- STEP 2: Validate price logic ---
                If newSellingPrice < newCostPrice Then
                    MsgBox("Selling Price must be higher than Cost Price.", MsgBoxStyle.Critical, "Invalid Input")
                    Return
                ElseIf newSellingPrice = newCostPrice Then
                    MsgBox("Selling Price cannot be equal to Cost Price.", MsgBoxStyle.Critical, "Invalid Input")
                    Return
                End If

                ' --- STEP 3: Update product prices if changed ---
                If newCostPrice <> oldCostPrice OrElse newSellingPrice <> oldSellingPrice Then
                    Dim updateProductQuery As String = "UPDATE producttbl SET Cost_Price = @Cost_Price, Selling_price = @Selling_price WHERE Barcode = @Barcode"
                    Using cmd As New MySqlCommand(updateProductQuery, con)
                        cmd.Parameters.AddWithValue("@Cost_Price", newCostPrice)
                        cmd.Parameters.AddWithValue("@Selling_price", newSellingPrice)
                        cmd.Parameters.AddWithValue("@Barcode", barcode)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Log price updates
                    If newCostPrice <> oldCostPrice Then
                        Using auditCmd As New MySqlCommand("INSERT INTO audittbl (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @type, @name, @action)", con)
                            auditCmd.Parameters.AddWithValue("@type", userType)
                            auditCmd.Parameters.AddWithValue("@name", userName)
                            auditCmd.Parameters.AddWithValue("@action", $"Updated Cost Price for Barcode: {barcode} from {oldCostPrice:F2} to {newCostPrice:F2}")
                            auditCmd.ExecuteNonQuery()
                        End Using
                    End If

                    If newSellingPrice <> oldSellingPrice Then
                        Using auditCmd As New MySqlCommand("INSERT INTO audittbl (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @type, @name, @action)", con)
                            auditCmd.Parameters.AddWithValue("@type", userType)
                            auditCmd.Parameters.AddWithValue("@name", userName)
                            auditCmd.Parameters.AddWithValue("@action", $"Updated Selling Price for Barcode: {barcode} from {oldSellingPrice:F2} to {newSellingPrice:F2}")
                            auditCmd.ExecuteNonQuery()
                        End Using
                    End If

                    ito() ' Refresh view or data
                End If

                ' --- STEP 4: Update quantity if changed ---
                If newQuantity <> oldQuantity Then
                    Using updateQtyCmd As New MySqlCommand("UPDATE quantitytbl1 SET Quantity = @Quantity WHERE Barcode = @Barcode", con)
                        updateQtyCmd.Parameters.AddWithValue("@Quantity", newQuantity)
                        updateQtyCmd.Parameters.AddWithValue("@Barcode", barcode)
                        updateQtyCmd.ExecuteNonQuery()
                    End Using

                    ' Audit log for quantity change
                    Using auditQty As New MySqlCommand("INSERT INTO audittbl (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @type, @name, @action)", con)
                        auditQty.Parameters.AddWithValue("@type", userType)
                        auditQty.Parameters.AddWithValue("@name", userName)
                        auditQty.Parameters.AddWithValue("@action", $"Updated Quantity for Barcode: {barcode} from {oldQuantity} to {newQuantity}")
                        auditQty.ExecuteNonQuery()
                    End Using

                    ito()
                End If
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MsgBoxStyle.Critical, "MySQL Error")

        Catch ex As FormatException
            MsgBox("Input format error: " & ex.Message, MsgBoxStyle.Critical, "Format Error")

        Catch ex As Exception
            MsgBox("Unexpected error: " & ex.Message, MsgBoxStyle.Critical, "Unknown Error")

        End Try



        If Not String.IsNullOrWhiteSpace(TextBox16.Text) Then
            Dim newForm As New InventoryF()
            newForm.Show()
            newForm.TextBox16.Text = TextBox15.Text
            newForm.IconButton11.PerformClick()
            Me.Close()
        Else
            Dim newForm As New InventoryF()
            newForm.Show()
            newForm.IconButton15.PerformClick()
            Me.Close()
        End If
        checker1()
    End Sub

    Private Sub Descr_TextChanged(sender As Object, e As EventArgs) Handles Descr.TextChanged
        If Descr.Text.Trim() <> "" Then
            IconButton14.Enabled = True
        Else
            IconButton14.Enabled = False
        End If
    End Sub

    Private Sub Quan_TextChanged(sender As Object, e As EventArgs) Handles Quan.TextChanged




    End Sub

    Private Sub QU_KeyPress(sender As Object, e As KeyPressEventArgs) Handles QU.KeyPress
        ' Check if the pressed key is a special character (not alphanumeric)
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is a special character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If QU.Text.Length >= 5 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub QU_TextChanged(sender As Object, e As EventArgs) Handles QU.TextChanged
        Try

            Dim sellingPrice As Decimal
            Dim quantity As Integer

            ' Try to parse the values
            If Decimal.TryParse(cp.Text, sellingPrice) AndAlso Integer.TryParse(QU.Text, quantity) Then
                ' Calculate the total amount
                Dim totalAmount As Decimal = sellingPrice * quantity

                ' Display the result in TA.Text
                TA.Text = totalAmount.ToString("F2") ' Format to 2 decimal places
            Else

            End If
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged

    End Sub

    Private Sub IconButton7_Click(sender As Object, e As EventArgs) Handles IconButton7.Click
        Try
            ' Validate input fields before proceeding
            If String.IsNullOrWhiteSpace(SN.Text) Then
                MsgBox("Please enter a delivery receipt number.")
                Return
            End If

            If String.IsNullOrWhiteSpace(cp.Text) Then
                MsgBox("Please enter a valid Cost Price.")
                Return
            End If

            ' Check if Cost Price is 0 or less
            If Convert.ToDecimal(cp.Text) <= 0 Then
                MsgBox("Cost Price cannot be 0 or less.", MsgBoxStyle.Critical, "Invalid Input")
                Return
            End If

            If String.IsNullOrWhiteSpace(barc.Text) Then
                MsgBox("Please enter a valid Barcode.")
                Return
            End If

            ' Open the database connection
            opencon()

            Dim getSellingPriceQuery As String = "SELECT Selling_price FROM producttbl WHERE Barcode = @Barcode"
            Dim sellingPrice As Decimal = 0

            Using cmd As New MySqlCommand(getSellingPriceQuery, con)
                cmd.Parameters.AddWithValue("@Barcode", barc.Text.Trim())

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        sellingPrice = Convert.ToDecimal(reader("Selling_price"))
                    End If
                End Using
            End Using

            ' Step 2: Validate if Selling_Price is greater than Cost_Price
            If sellingPrice <= Convert.ToDecimal(cp.Text) Then
                ' Show error message if Selling_Price is less than or equal to Cost_Price
                MsgBox("Selling Price must be greater than Cost Price.", MsgBoxStyle.Critical, "Invalid Input")
                Return ' Exit the function to prevent further execution
            Else
                ' Step 3: If validation passes, proceed with the update for Cost_Price
                Dim updateProductQuery As String = "UPDATE producttbl SET Cost_Price = @Cost_Price WHERE Barcode = @Barcode"

                Using cmd As New MySqlCommand(updateProductQuery, con)
                    ' Add the parameters for the update query
                    cmd.Parameters.AddWithValue("@Barcode", barc.Text.Trim())
                    cmd.Parameters.AddWithValue("@Cost_Price", Convert.ToDecimal(cp.Text)) ' Ensure cost price is a decimal

                    ' Execute the update query
                    cmd.ExecuteNonQuery()

                End Using
            End If

            Using cmd As New MySqlCommand("INSERT INTO deliveyltbl (`Delivery_Receipt`,`Batch_No`, `Company_name`, `Barcode`, `Date_received`, `Quantity`, `Cost_price`, `Total_amount`) VALUES (@Delivery_Receipt,@Batch_No,@Company_name, @Barcode, @Date_received, @Quantity, @Cost_price, @Total_amount)", con)
                ' Add parameters for the delivery table
                cmd.Parameters.AddWithValue("@Delivery_Receipt", TextBox8.Text)
                cmd.Parameters.AddWithValue("@Batch_No", TextBox11.Text)
                cmd.Parameters.AddWithValue("@Company_name", TextBox1.Text)
                cmd.Parameters.AddWithValue("@Barcode", barc.Text)
                cmd.Parameters.AddWithValue("@Date_received", DTP1.Value.ToString("yyyy-MM-dd HH:mm:ss"))
                cmd.Parameters.AddWithValue("@Quantity", QU.Text)
                cmd.Parameters.AddWithValue("@Cost_price", cp.Text)
                cmd.Parameters.AddWithValue("@Total_amount", TA.Text)

                ' Execute the query
                cmd.ExecuteNonQuery()
                MsgBox("Delivery information added successfully.")
            End Using

            ' Insert an audit trail entry for the action performed
            Using auditCmd As New MySqlCommand("INSERT INTO `audittbl` (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, @Action)", con)
                ' Add parameters for the audit trail
                auditCmd.Parameters.AddWithValue("@User_type", Label19.Text) ' Dynamically set User_type based on the logged-in user
                auditCmd.Parameters.AddWithValue("@Name", Label17.Text) ' User's name
                auditCmd.Parameters.AddWithValue("@Action", "Added delivery and updated cost price for barcode " & barc.Text) ' Action description

                ' Execute the audit trail insert
                auditCmd.ExecuteNonQuery()
            End Using

        Catch ex As MySqlException
            ' Handle database-specific errors
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


        IconButton3.Enabled = True
        TextBox1.Text = ""
        TextBox8.Text = ""
        cp.Text = ""
        TA.Text = ""
        barc.Text = ""
        QU.Text = ""
        IconButton4.Enabled = True
        DataGridView1.Enabled = True
        IconButton6.Enabled = True
        IconButton5.Enabled = True
        IconButton18.Enabled = True
        ' ito()
        IconButton15.Enabled = True
        IconButton1.Enabled = False
        IconButton7.Enabled = False
        IconButton2.Enabled = True
        test()
        'Dim newForm As New InventoryF()
        'newForm.Show()
        ' newForm.IconButton15.PerformClick()
        ' Me.Close()


    End Sub
    Private Sub ito()

        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using statement ensures the connection is properly disposed of
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection

                ' Define the SQL query to join product and quantity tables
                Dim query As String = "SELECT producttbl.Barcode, Product_Name, Cat_name, Color, ItemSize, Selling_price,Cost_price, Quantity " &
                                      "FROM producttbl " &
                                      "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                                      "LIMIT 1000;"
                Dim cmd As New MySqlCommand(query, con)

                ' Fill the data into a DataSet
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim ds As New DataSet()
                adapter.Fill(ds)
                For Each column As DataGridViewColumn In DataGridView1.Columns
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Next
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
    Private Sub IconButton15_Click(sender As Object, e As EventArgs) Handles IconButton15.Click
        TextBox16.Text = ""
        checker1()
        hulog()
        ito()
        IconButton7.Enabled = False
        IconButton6.Enabled = True
        IconButton5.Enabled = True
        IconButton18.Enabled = True
        IconButton2.Enabled = True

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs)
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Populate TextBoxes with the values from the selected row, checking for DBNull values
            barc.Text = If(selectedRow.Cells("Barcode").Value IsNot DBNull.Value, selectedRow.Cells("Barcode").Value.ToString(), "")
            Desc.Text = If(selectedRow.Cells("Product_Name").Value IsNot DBNull.Value, selectedRow.Cells("Product_Name").Value.ToString(), "")
            CN.Text = If(selectedRow.Cells("Cat_name").Value IsNot DBNull.Value, selectedRow.Cells("Cat_name").Value.ToString(), "")
            cp.Text = If(selectedRow.Cells("Cost_Price").Value IsNot DBNull.Value, selectedRow.Cells("Cost_Price").Value.ToString(), "0")
            QU.Text = If(selectedRow.Cells("Quantity").Value IsNot DBNull.Value, selectedRow.Cells("Quantity").Value.ToString(), "0")

        Else
            ' Clear TextBoxes if no row is selected
            barc.Text = ""
            Desc.Text = ""
            CN.Text = ""
            QU.Text = ""
            cp.Text = "0" ' Defaulting Cost Price to "0" if no row is selected
        End If

    End Sub

    Private Sub IconButton6_Click(sender As Object, e As EventArgs) Handles IconButton6.Click
        Try
            ' Validate input before proceeding
            If String.IsNullOrWhiteSpace(barc.Text) Then
                MsgBox("Please enter a valid barcode.")
                Return
            End If

            ' Prompt the user with a Yes/No confirmation dialog
            Dim result As DialogResult = MsgBox("Are you sure you want to delete the quantity record for barcode: " & barc.Text & "?", MsgBoxStyle.YesNo, "Delete Confirmation")

            ' If the user clicked "Yes", proceed with the delete operation
            If result = DialogResult.Yes Then
                ' Define the connection string
                Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

                ' Create a new MySqlConnection
                Using con As New MySqlConnection(connectionString)
                    Try
                        con.Open() ' Open the connection

                        ' Define the SQL delete query for quantitytbl1
                        Dim deleteQuery As String = "DELETE FROM quantitytbl1 WHERE Barcode = @Barcode"

                        ' Create the MySqlCommand for deletion
                        Using deleteCmd As New MySqlCommand(deleteQuery, con)
                            ' Assign the value to the @Barcode parameter
                            deleteCmd.Parameters.AddWithValue("@Barcode", barc.Text.Trim())

                            ' Execute the delete command
                            Dim rowsAffected As Integer = deleteCmd.ExecuteNonQuery()

                            ' Check if the delete was successful
                            If rowsAffected > 0 Then
                                MsgBox("Quantity record deleted successfully.")
                            Else
                                MsgBox("No record found for the provided barcode.")
                            End If
                        End Using

                        ' Step 2: Update the Cost_Price to 0 for the deleted barcode in the producttbl
                        Dim updateCostPriceQuery As String = "UPDATE producttbl SET Cost_Price = 0 WHERE Barcode = @Barcode"

                        Using updateCmd As New MySqlCommand(updateCostPriceQuery, con)
                            ' Assign the barcode parameter to the update query
                            updateCmd.Parameters.AddWithValue("@Barcode", barc.Text.Trim())

                            ' Execute the update command to set Cost_Price to 0
                            updateCmd.ExecuteNonQuery()
                        End Using

                        ' Step 3: Insert an audit trail entry for the action performed
                        Using auditCmd As New MySqlCommand("INSERT INTO audittbl (`DateTime`, `User_type`, `Name`, `Action`) VALUES (NOW(), @User_type, @Name, @Action)", con)
                            ' Add parameters for the audit trail
                            auditCmd.Parameters.AddWithValue("@User_type", Label19.Text) ' User type (e.g., Admin, User)
                            auditCmd.Parameters.AddWithValue("@Name", Label17.Text) ' User's name
                            auditCmd.Parameters.AddWithValue("@Action", "Deleted quantity record and updated cost price for barcode: " & barc.Text) ' Description of the action

                            ' Execute the audit trail insert
                            auditCmd.ExecuteNonQuery()
                        End Using

                        ' Optional: Refresh the form or reset controls
                        ' You can reset barcode input field if needed
                        barc.Text = "" ' Reset barcode input (if needed)

                        ' Optionally, you can refresh a DataGridView or any other data-bound controls
                        ' DataGridView1.DataSource = Nothing
                        ' LoadData() ' Custom method to reload data (you should have a function to load data into your controls)

                        ' You can optionally refresh the form (if required)
                        ' Me.Refresh() ' This will refresh the entire form and its controls.

                        ' If you want to open a new form and simulate a button click:
                        ' Uncomment the lines below if you want to show a new form and simulate a button click on that form.
                        Dim newForm As New InventoryF()  ' Create a new instance of the InventoryF form
                        newForm.Show()
                        newForm.IconButton15.PerformClick()
                        Me.Close()

                        ' Simulate the button click

                    Catch ex As MySqlException
                        ' Handle database-specific errors
                        MsgBox("Database error: " & ex.Message)
                    Catch ex As Exception
                        ' Handle any other general errors
                        MsgBox("An unexpected error occurred: " & ex.Message)
                    End Try
                End Using ' Automatically closes the connection

            Else
                ' If the user clicked "No", show a message or handle the cancellation (optional)
                MsgBox("Deletion canceled.")
            End If

        Catch ex As Exception
            ' Handle any other general errors that might occur outside the database operations
            MsgBox("An unexpected error occurred: " & ex.Message)
        End Try


        ito()
    End Sub

    Private Sub IconButton2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub IconButton11_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub DataGridView3_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView3.CellContentClick

    End Sub
    Private Sub cp_TextChanged(sender As Object, e As EventArgs) Handles cp.TextChanged

        Try
            ' Ensure SP.Text and QU.Text are numeric and can be parsed
            Dim sellingPrice As Decimal
            Dim quantity As Integer

            ' Try to parse the values
            If Decimal.TryParse(cp.Text, sellingPrice) AndAlso Integer.TryParse(QU.Text, quantity) Then
                ' Calculate the total amount
                Dim totalAmount As Decimal = sellingPrice * quantity

                ' Display the result in TA.Text
                TA.Text = totalAmount.ToString("F2") ' Format to 2 decimal places
            Else

            End If
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub IconButton12_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub DataGridView1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_SelectionChanged_1(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Populate TextBoxes with values from the selected row, checking for DBNull values
            barc.Text = If(selectedRow.Cells("Barcode").Value IsNot DBNull.Value, selectedRow.Cells("Barcode").Value.ToString(), "")
            Desc.Text = If(selectedRow.Cells("Product_Name").Value IsNot DBNull.Value, selectedRow.Cells("Product_Name").Value.ToString(), "")
            CN.Text = If(selectedRow.Cells("Cat_name").Value IsNot DBNull.Value, selectedRow.Cells("Cat_name").Value.ToString(), "")
            TextBox12.Text = If(selectedRow.Cells("Selling_price").Value IsNot DBNull.Value, selectedRow.Cells("Selling_price").Value.ToString(), "0")
            cp.Text = If(selectedRow.Cells("Cost_Price").Value IsNot DBNull.Value, selectedRow.Cells("Cost_Price").Value.ToString(), "0")
            QU.Text = If(selectedRow.Cells("Quantity").Value IsNot DBNull.Value, selectedRow.Cells("Quantity").Value.ToString(), "0")
        Else
            ' Clear TextBoxes if no row is selected
            barc.Text = ""
            Desc.Text = ""
            CN.Text = ""
            QU.Text = ""
            TextBox12.Text = "0"
            cp.Text = "0" ' Default value for Cost Price
        End If
    End Sub

    Private Sub IconButton18_Click(sender As Object, e As EventArgs) Handles IconButton18.Click
        ' Check if any row is selected in the DataGridView
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Get the selected row (you can use the first selected row or modify as needed)
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' For example, you can retrieve data from a specific column (e.g., Barcode or Quantity)
            Dim selectedBarcode As String = selectedRow.Cells("Barcode").Value.ToString()
            Dim selectedpn As String = selectedRow.Cells("Product_Name").Value.ToString()
            Dim selectedsp As String = selectedRow.Cells("Selling_price").Value.ToString()
            Dim selectedcn As String = selectedRow.Cells("Cat_name").Value.ToString()
            Dim selectedcp As String = selectedRow.Cells("Cost_price").Value.ToString()



            ' Now you can perform actions with the selected data, such as showing Panel8
            ' Set the data or controls inside Panel8 (optional)
            TextBox14.Text = selectedsp
            TextBox3.Text = selectedBarcode
            TextBox4.Text = selectedpn
            ComboBox3.Text = selectedcn
            TextBox10.Text = selectedcp
            ' Example of setting a value in a textbox inside Panel8
            ' Example of setting a value in another textbox

            ' Show Panel8
            Panel1.Visible = True  ' Make Panel8 visible when IconButton18 is clicked
        Else
            ' If no row is selected, show an error or message
            MessageBox.Show("Please select a row first.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
    Private Sub Textbox10_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox10.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or control
        End If

        ' Prevent entering more than 4 characters
        If TextBox10.Text.Length >= 7 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 4

        End If
    End Sub
    Private Sub TextBox10_TextChanged(sender As Object, e As EventArgs) Handles TextBox10.TextChanged

    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        Try

            Dim sellingPrice As Decimal
            Dim quantity As Integer

            ' Try to parse the values
            If Decimal.TryParse(TextBox10.Text, sellingPrice) AndAlso Integer.TryParse(TextBox6.Text, quantity) Then
                ' Calculate the total amount
                Dim totalAmount As Decimal = sellingPrice * quantity

                ' Display the result in TA.Text
                TextBox9.Text = totalAmount.ToString("F2") ' Format to 2 decimal places
            Else

            End If
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub IconButton16_Click(sender As Object, e As EventArgs) Handles IconButton16.Click
        Try
            opencon()

            ' Validate required fields
            If String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
       String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
       String.IsNullOrWhiteSpace(TextBox6.Text) OrElse
       String.IsNullOrWhiteSpace(TextBox9.Text) OrElse
       String.IsNullOrWhiteSpace(TextBox10.Text) OrElse
       String.IsNullOrWhiteSpace(TextBox14.Text) Then

                MsgBox("Please fill in all the required fields.")
                Exit Sub
            End If

            ' Parse numeric fields
            Dim quantity As Integer
            If Not Integer.TryParse(TextBox6.Text, quantity) Then
                MsgBox("Invalid quantity.")
                Exit Sub
            End If

            Dim costPrice As Decimal
            If Not Decimal.TryParse(TextBox10.Text, costPrice) Then
                MsgBox("Invalid cost price.")
                Exit Sub
            End If

            Dim totalAmount As Decimal
            If Not Decimal.TryParse(TextBox9.Text, totalAmount) Then
                MsgBox("Invalid total amount.")
                Exit Sub
            End If

            Dim sellingPrice As Decimal
            If Not Decimal.TryParse(TextBox14.Text, sellingPrice) Then
                MsgBox("Invalid selling price.")
                Exit Sub
            End If

            If costPrice >= sellingPrice Then
                MsgBox("Cost price should not be equal to or higher than selling price.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            Dim barcode As String = TextBox3.Text.Trim()

            ' ✅ STRICT CHECK: Prevent duplicates in DataGridView7
            For Each row As DataGridViewRow In DataGridView7.Rows
                If row.IsNewRow Then Continue For
                Dim existingBarcode As String = row.Cells("Barcode").Value.ToString().Trim().ToLower()
                If existingBarcode = barcode.ToLower() Then
                    MsgBox("This barcode already exists in the restock list. Duplicate not allowed.", MsgBoxStyle.Exclamation)
                    Exit Sub
                End If
            Next

            ' INSERT INTO deliveyltbl
            Using cmd As New MySqlCommand("
        INSERT INTO deliveyltbl 
        (Delivery_Receipt, Batch_No, Company_name, Barcode, Date_received, Quantity, Cost_price, Total_amount) 
        VALUES 
        (@Delivery_Receipt, @Batch_No, @Company_name, @Barcode, @Date_received, @Quantity, @Cost_price, @Total_amount)", con)

                cmd.Parameters.AddWithValue("@Delivery_Receipt", TextBox2.Text)
                cmd.Parameters.AddWithValue("@Batch_No", TextBox7.Text)
                cmd.Parameters.AddWithValue("@Company_name", ComboBox2.Text)
                cmd.Parameters.AddWithValue("@Barcode", barcode)
                cmd.Parameters.AddWithValue("@Date_received", DateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss"))
                cmd.Parameters.AddWithValue("@Quantity", quantity)
                cmd.Parameters.AddWithValue("@Cost_price", costPrice)
                cmd.Parameters.AddWithValue("@Total_amount", totalAmount)
                cmd.ExecuteNonQuery()

                MsgBox("Delivery information added successfully.")
            End Using

            ' Get current values from DataGridView1
            Dim originalCost As Decimal = 0
            Dim originalSell As Decimal = 0

            If DataGridView1.CurrentRow IsNot Nothing Then
                originalCost = Convert.ToDecimal(DataGridView1.CurrentRow.Cells("Cost_price").Value)
                originalSell = Convert.ToDecimal(DataGridView1.CurrentRow.Cells("Selling_price").Value)
            End If

            ' Check if values changed
            Dim newCost As Decimal = costPrice
            Dim newSell As Decimal = sellingPrice

            If newCost <> originalCost OrElse newSell <> originalSell Then
                ' Insert into restocktbl
                Using restockCmd As New MySqlCommand("
            INSERT INTO restocktbl 
            (Barcode, Company_name, Date_delivered, Product_name, Category_name, Quantity, Cost_price, Selling_price) 
            VALUES 
            (@Barcode, @Company_name, @Date_delivered, @Product_name, @Category_name, @Quantity, @Cost_price, @Selling_price)", con)

                    restockCmd.Parameters.AddWithValue("@Barcode", barcode)
                    restockCmd.Parameters.AddWithValue("@Company_name", ComboBox2.Text)
                    restockCmd.Parameters.AddWithValue("@Date_delivered", DateTimePicker1.Value)
                    restockCmd.Parameters.AddWithValue("@Product_name", TextBox4.Text)
                    restockCmd.Parameters.AddWithValue("@Category_name", ComboBox3.Text)
                    restockCmd.Parameters.AddWithValue("@Quantity", quantity)
                    restockCmd.Parameters.AddWithValue("@Cost_price", newCost)
                    restockCmd.Parameters.AddWithValue("@Selling_price", newSell)
                    restockCmd.ExecuteNonQuery()
                End Using
            Else
                ' Only update quantity if no price change
                Dim getQtyCmd As New MySqlCommand("SELECT Quantity FROM quantitytbl1 WHERE LOWER(Barcode) = LOWER(@Barcode)", con)
                getQtyCmd.Parameters.AddWithValue("@Barcode", barcode)
                Dim result As Object = getQtyCmd.ExecuteScalar()

                If result IsNot Nothing Then
                    Dim currentQty As Integer = Convert.ToInt32(result) + quantity

                    Dim updateQtyCmd As New MySqlCommand("UPDATE quantitytbl1 SET Quantity = @Quantity WHERE Barcode = @Barcode", con)
                    updateQtyCmd.Parameters.AddWithValue("@Quantity", currentQty)
                    updateQtyCmd.Parameters.AddWithValue("@Barcode", barcode)
                    updateQtyCmd.ExecuteNonQuery()
                    checker1
                    Panel1.Visible = False
                    TextBox6.Text = ""
                Else
                    MsgBox("No record found for the provided barcode.", MsgBoxStyle.Critical)
                End If
            End If

            ' Audit trail
            Try
                Using auditCon As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co"),
              auditCmd As New MySqlCommand("INSERT INTO audittbl (DateTime, User_type, Name, Action) VALUES (NOW(), @User_type, @Name, @Action)", auditCon)

                    auditCon.Open()
                    auditCmd.Parameters.AddWithValue("@User_type", Label19.Text)
                    auditCmd.Parameters.AddWithValue("@Name", Label17.Text)
                    auditCmd.Parameters.AddWithValue("@Action", $"Stock updated for Barcode {barcode}")
                    auditCmd.ExecuteNonQuery()
                End Using
            Catch auditEx As Exception
                MsgBox("Audit log failed: " & auditEx.Message)
            End Try

        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
        restock()
        list()
        ito()
        Dim newForm As New InventoryF()
        newForm.Show()
        newForm.IconButton15.PerformClick()
        Me.Close()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Panel8_Paint(sender As Object, e As PaintEventArgs)

    End Sub
    Private Sub checker1()
        Try
            opencon()

            ' Make sure a row is selected in both DataGridView1 and DataGridView7
            If DataGridView1.SelectedRows.Count > 0 AndAlso DataGridView7.SelectedRows.Count > 0 Then
                ' Get Barcode from the selected rows
                Dim barcode1 As String = DataGridView1.SelectedRows(0).Cells("Barcode").Value.ToString()
                Dim barcode7 As String = DataGridView7.SelectedRows(0).Cells("Barcode").Value.ToString()

                ' Check if the Barcodes match
                If barcode1 = barcode7 Then
                    ' Get the Selling_price and Cost_price from both DataGridView1 and DataGridView7
                    Dim sellingPrice1 As Decimal = Convert.ToDecimal(DataGridView1.SelectedRows(0).Cells("Selling_price").Value)
                    Dim costPrice1 As Decimal = Convert.ToDecimal(DataGridView1.SelectedRows(0).Cells("Cost_price").Value)

                    Dim sellingPrice7 As Decimal = Convert.ToDecimal(DataGridView7.SelectedRows(0).Cells("Selling_price").Value)
                    Dim costPrice7 As Decimal = Convert.ToDecimal(DataGridView7.SelectedRows(0).Cells("Cost_price").Value)

                    ' Check if both Selling_price and Cost_price match
                    If sellingPrice1 = sellingPrice7 AndAlso costPrice1 = costPrice7 Then
                        ' If they are the same, get the quantity from restocktbl and update quantitytbl1
                        Dim getQtyCmd As New MySqlCommand("SELECT Quantity FROM restocktbl WHERE Barcode = @Barcode", con)
                        getQtyCmd.Parameters.AddWithValue("@Barcode", barcode1)
                        Dim restockQuantity As Object = getQtyCmd.ExecuteScalar()

                        If restockQuantity IsNot Nothing Then
                            Dim quantityToAdd As Integer = Convert.ToInt32(restockQuantity)

                            ' Add the quantity from restocktbl into quantitytbl1
                            Dim updateQtyCmd As New MySqlCommand("UPDATE quantitytbl1 SET Quantity = Quantity + @Quantity WHERE Barcode = @Barcode", con)
                            updateQtyCmd.Parameters.AddWithValue("@Quantity", quantityToAdd)
                            updateQtyCmd.Parameters.AddWithValue("@Barcode", barcode1)
                            updateQtyCmd.ExecuteNonQuery()

                            ' Now delete the record from restocktbl
                            Using deleteCmd As New MySqlCommand("DELETE FROM restocktbl WHERE Barcode = @Barcode", con)
                                deleteCmd.Parameters.AddWithValue("@Barcode", barcode1)
                                deleteCmd.ExecuteNonQuery()


                            End Using
                        Else

                        End If
                    Else

                    End If
                Else

                End If
            Else

            End If

        Catch ex As Exception

        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
        ito()
    End Sub
    Private Sub IconButton17_Click(sender As Object, e As EventArgs) Handles IconButton17.Click
        Panel1.Visible = False
        ito()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        If Not String.IsNullOrEmpty(TextBox3.Text) Then
            Try
                Dim maxBatchNo As Integer = 0 ' Variable to hold the highest Batch_No found for the barcode

                ' Loop through all rows in the DataGridView to find the matching barcode
                For Each row As DataGridViewRow In DataGridView6.Rows
                    ' Ensure the row's Barcode cell is not empty or Nothing
                    If row.Cells("Barcode").Value IsNot Nothing AndAlso row.Cells("Barcode").Value.ToString() = TextBox3.Text Then
                        ' Get the corresponding Batch_No value for this barcode
                        Dim batchNo As Object = row.Cells("Batch_No").Value

                        ' Check if the Batch_No is valid (numeric)
                        If batchNo IsNot Nothing AndAlso IsNumeric(batchNo) Then
                            ' Convert the Batch_No to an integer and find the highest Batch_No
                            Dim currentBatchNo As Integer = Convert.ToInt32(batchNo)
                            If currentBatchNo > maxBatchNo Then
                                maxBatchNo = currentBatchNo ' Update maxBatchNo to the highest value
                            End If
                        End If
                    End If
                Next

                ' If we found any Batch_No for the barcode, increment the highest one by 1
                If maxBatchNo > 0 Then
                    TextBox7.Text = (maxBatchNo + 1).ToString()
                Else
                    ' If no Batch_No was found, set it to 1 (first batch for this barcode)
                    TextBox7.Text = "1"
                End If

            Catch ex As Exception
                ' Display an error message in case of an exception
                MsgBox("An error occurred: " & ex.Message)
            End Try
        Else
            ' Clear TextBox7 if TextBox3 is empty
            TextBox7.Clear()
        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub CNAM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CNAM.SelectedIndexChanged

    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        Dim selectedDate As Date = DateTimePicker1.Value.Date '
        If selectedDate > DateTime.Now.Date Then
            MsgBox("You cannot select a future date.", MessageBoxIcon.Warning)
            ' Reset the DateTimePicker to today's date
            DateTimePicker1.Value = DateTime.Now
            Return
        End If
    End Sub
    Private Sub quan_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Quan.KeyPress
        ' Allow only numeric characters (0-9)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or a control character (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If Quan.Text.Length >= 4 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub textbox6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox6.KeyPress
        ' Allow only numeric characters (0-9)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or a control character (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If TextBox6.Text.Length >= 4 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub Panel1_Paint_1(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Panel2_Paint_1(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub IconButton2_Click_1(sender As Object, e As EventArgs) Handles IconButton2.Click
        checker1()

        hulog()
        Panel8.Visible = True
        restock()
    End Sub

    Private Sub IconButton8_Click(sender As Object, e As EventArgs) Handles IconButton8.Click
        Panel8.Visible = False
    End Sub
    Private Sub Textbox14_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox14.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or control
        End If

        ' Prevent entering more than 4 characters
        If TextBox14.Text.Length >= 7 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 4

        End If
    End Sub
    Private Sub TextBox14_TextChanged(sender As Object, e As EventArgs) Handles TextBox14.TextChanged

    End Sub
    Private Sub test()
        TextBox16.Text = TextBox15.Text.Trim()
        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using statement ensures the connection is properly disposed of
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection

                ' Get barcode value from TextBox15
                Dim barcodeInput As String = TextBox15.Text.Trim()

                ' Define the SQL query to filter by barcode
                Dim query As String = "SELECT producttbl.Barcode, Product_Name, Cat_name, Color, ItemSize, Selling_price, Cost_price, Quantity " &
                              "FROM producttbl " &
                              "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                              "WHERE producttbl.Barcode = @Barcode"

                Dim cmd As New MySqlCommand(query, con)
                cmd.Parameters.AddWithValue("@Barcode", barcodeInput) ' Use parameter to avoid SQL injection

                ' Fill the data into a DataSet
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim ds As New DataSet()
                adapter.Fill(ds)

                ' Check if data is available and bind it to the DataGridView
                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    DataGridView1.DataSource = ds.Tables(0)

                    ' Center align all columns
                    For Each column As DataGridViewColumn In DataGridView1.Columns
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Next
                Else
                    DataGridView1.DataSource = Nothing

                End If
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub TextBox15_TextChanged(sender As Object, e As EventArgs) Handles TextBox15.TextChanged
        test()

    End Sub

    Private Sub IconButton11_Click_1(sender As Object, e As EventArgs) Handles IconButton11.Click
        sb()
    End Sub
    Private Sub sb()

        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using statement ensures the connection is properly disposed of
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the connection

                ' Get barcode value from TextBox15
                Dim barcodeInput As String = TextBox16.Text.Trim()

                ' Define the SQL query to filter by barcode
                Dim query As String = "SELECT producttbl.Barcode, Product_Name, Cat_name, Color, ItemSize, Selling_price, Cost_price, Quantity " &
                              "FROM producttbl " &
                              "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                              "WHERE producttbl.Barcode = @Barcode"

                Dim cmd As New MySqlCommand(query, con)
                cmd.Parameters.AddWithValue("@Barcode", barcodeInput) ' Use parameter to avoid SQL injection

                ' Fill the data into a DataSet
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim ds As New DataSet()
                adapter.Fill(ds)

                ' Check if data is available and bind it to the DataGridView
                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    DataGridView1.DataSource = ds.Tables(0)

                    ' Center align all columns
                    For Each column As DataGridViewColumn In DataGridView1.Columns
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Next
                Else
                    DataGridView1.DataSource = Nothing

                End If
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class