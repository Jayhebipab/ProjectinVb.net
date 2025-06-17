Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports FontAwesome.Sharp






Public Class POSF
    Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"
    Dim con As New MySqlConnection(connectionString)

    Private WithEvents printDoc As New PrintDocument

    Private Sub PrintReceipt(sender As Object, e As PrintPageEventArgs)
        Try
            ' Define the text for the receipt
            Dim receiptText As String = "Bayabas Roots Co."
            Dim address As String = "76 Tanyag St Manila, Metro Manila"
            Dim orNumber As String = "" & Label29.Text
            Dim dateText As String = "Transaction Date: " & DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")
            Dim cashierText As String = "Cashier: " & Label1.Text

            Dim x As Integer = 5
            Dim y As Integer = 5

            Dim largeFont As New Font("Arial", 12, FontStyle.Bold)
            Dim font As New Font("Arial", 7, FontStyle.Regular)
            Dim boldFont As New Font("Arial", 7, FontStyle.Bold)
            Dim largerFont As New Font("Arial", 10, FontStyle.Bold)

            ' Draw company name
            Dim companyNameWidth As Single = e.Graphics.MeasureString(receiptText, largeFont).Width
            Dim companyNameX As Integer = (e.PageBounds.Width - companyNameWidth) / 2
            e.Graphics.DrawString(receiptText, largeFont, Brushes.Black, companyNameX, y)
            y += 20

            ' Draw address
            If y + 12 < e.PageBounds.Height - 50 Then
                e.Graphics.DrawString(address, font, Brushes.Black, x, y)
                y += 12
            Else
                y = e.PageBounds.Height - 50
                e.Graphics.DrawString(address, font, Brushes.Black, x, y)
                y += 12
            End If

            ' Draw other details
            e.Graphics.DrawString(dateText, font, Brushes.Black, x, y)
            y += 12
            e.Graphics.DrawString(cashierText, font, Brushes.Black, x, y)
            y += 15
            Dim orNumberWidth As Single = e.Graphics.MeasureString("" & orNumber, largerFont).Width
            Dim orNumberX As Integer = (e.PageBounds.Width - orNumberWidth) / 2
            e.Graphics.DrawString("" & orNumber, largerFont, Brushes.Black, orNumberX, y)

            y += 20 ' Space after line before items

            ' Draw column headers
            e.Graphics.DrawString("Item.Name", boldFont, Brushes.Black, x, y)  ' Product Name
            e.Graphics.DrawString("Size", boldFont, Brushes.Black, x + 60, y)  ' Item Size
            e.Graphics.DrawString("Price", boldFont, Brushes.Black, x + 100, y)  ' Price
            e.Graphics.DrawString("Qty", boldFont, Brushes.Black, x + 150, y)  ' Quantity

            y += 12 ' Adjust space after header

            ' Draw item rows
            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow Then
                    ' Get item details
                    Dim productName As String = row.Cells(2).Value.ToString()
                    Dim itemSize As String = row.Cells(3).Value.ToString()
                    Dim price As String = row.Cells(4).Value.ToString()
                    Dim quantity As String = row.Cells(5).Value.ToString()

                    ' Draw item details
                    e.Graphics.DrawString(productName, font, Brushes.Black, x, y)
                    e.Graphics.DrawString(itemSize, font, Brushes.Black, x + 60, y)
                    e.Graphics.DrawString(price, font, Brushes.Black, x + 100, y)
                    e.Graphics.DrawString(quantity, font, Brushes.Black, x + 150, y)

                    y += 10 ' Space after each item row
                End If
            Next
            e.Graphics.DrawLine(Pens.Black, x, y + 5, e.PageBounds.Width - 5, y + 5)
            y += 15
            ' Calculate and format Total Amount
            Dim totalAmount As String = Label26.Text
            Dim formattedTotalAmount As String = ""
            If Decimal.TryParse(totalAmount, Nothing) Then
                formattedTotalAmount = Convert.ToDecimal(totalAmount).ToString("C2")
            Else
                formattedTotalAmount = "Invalid Amount"
            End If

            ' Calculate x for right-alignment (right side of the page)
            Dim totalWidth As Single = e.Graphics.MeasureString("Total: " & formattedTotalAmount, boldFont).Width
            Dim totalX As Integer = e.PageBounds.Width - totalWidth - 5 ' 5px margin from the right side

            ' Draw the Total Amount on the right side
            e.Graphics.DrawString("Total: " & formattedTotalAmount, boldFont, Brushes.Black, totalX, y)
            y += 12

            ' Format and Draw Cash Amount on the right side
            Dim cash As String = TextBox10.Text
            Dim formattedCash As String = ""
            If Decimal.TryParse(cash, Nothing) Then
                formattedCash = Convert.ToDecimal(cash).ToString("C2")
            Else
                formattedCash = "Invalid Cash"
            End If

            Dim cashWidth As Single = e.Graphics.MeasureString("Cash: " & formattedCash, boldFont).Width
            Dim cashX As Integer = e.PageBounds.Width - cashWidth - 5
            e.Graphics.DrawString("Cash: " & formattedCash, boldFont, Brushes.Black, cashX, y)
            y += 12

            ' Format and Draw Change Amount on the right side
            Dim change As String = Label27.Text
            Dim formattedChange As String = ""
            If Decimal.TryParse(change, Nothing) Then
                formattedChange = Convert.ToDecimal(change).ToString("C2")
            Else
                formattedChange = "Invalid Change"
            End If

            Dim changeWidth As Single = e.Graphics.MeasureString("Change: " & formattedChange, boldFont).Width
            Dim changeX As Integer = e.PageBounds.Width - changeWidth - 5
            e.Graphics.DrawString("Change: " & formattedChange, boldFont, Brushes.Black, changeX, y)
            y += 12

            ' Format and Draw Discount on the right side (only show if not "none" or empty)
            Dim disc As String = Label31.Text.Trim() ' Trim any extra spaces
            If Not String.IsNullOrEmpty(disc) AndAlso disc <> "none" Then
                Dim formattedDisc As String = ""
                If Decimal.TryParse(disc, Nothing) Then
                    formattedDisc = (Convert.ToDecimal(disc) * 1).ToString("0.00") & "%" ' Format as percentage
                Else
                    formattedDisc = "none"
                End If

                Dim discWidth As Single = e.Graphics.MeasureString("Discount: " & formattedDisc, boldFont).Width
                Dim discX As Integer = e.PageBounds.Width - discWidth - 5
                e.Graphics.DrawString("Discount: " & formattedDisc, boldFont, Brushes.Black, discX, y)
                y += 12
            End If

            y += 10
            ' Draw Vatable Sales on the left side below the discount
            Dim vatables As String = Label20.Text
            Dim formattedVatables As String = ""
            If Decimal.TryParse(vatables, Nothing) Then
                formattedVatables = Convert.ToDecimal(vatables).ToString("C2")
            Else
                formattedVatables = "Invalid Vatable Sales"
            End If

            ' Draw Vatable Sales
            e.Graphics.DrawString("Vatable Sales: " & formattedVatables, boldFont, Brushes.Black, x, y)
            y += 12

            ' Draw VAT Amount below Vatable Sales
            Dim vatAmount As String = Label21.Text
            e.Graphics.DrawString("VAT % : " & vatAmount, boldFont, Brushes.Black, x, y)
            y += 12
            y += 12
            y += 12

            Dim thankYouText As String = "****************************"
            Dim thankYouFont As New Font("Arial", 8, FontStyle.Italic)
            Dim thankYouWidth As Single = e.Graphics.MeasureString(thankYouText, thankYouFont).Width
            Dim thankYouX As Integer = (e.PageBounds.Width - thankYouWidth) / 2
            e.Graphics.DrawString(thankYouText, thankYouFont, Brushes.Black, thankYouX, y)

        Catch ex As Exception
            MessageBox.Show($"An error occurred while printing the receipt: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub kain()
        ' Your connection string for MySQL database
        Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

        ' SQL query to select records where Purchase_number is NULL or 0
        Dim query As String = "SELECT * FROM ontbl WHERE Purchase_number IS NULL OR Purchase_number = 0"

        ' Create a connection object for MySQL
        Using conn As New MySqlConnection(connectionString)
            ' Create a DataAdapter to fill the DataTable
            Dim adapter As New MySqlDataAdapter(query, conn)

            ' Create a DataTable to hold the results
            Dim dt As New DataTable()

            Try
                ' Open the connection
                conn.Open()

                ' Fill the DataTable with data from the query
                adapter.Fill(dt)

                ' Clear any existing rows in the DataGridView
                DataGridView1.Rows.Clear()

                ' Loop through the DataTable and add rows to DataGridView
                For Each dr As DataRow In dt.Rows
                    ' Add the desired columns to the DataGridView
                    DataGridView1.Rows.Add(
                         dr("order_number").ToString(),
                        dr("Barcode").ToString(),
                        dr("Product_Name").ToString(),
                        dr("ItemSize").ToString(),
                        dr("Selling_price").ToString(),
                        dr("Quantity").ToString(),
                        dr("Purchase_number").ToString()
                    )
                Next

            Catch ex As MySqlException
                ' Handle MySQL-specific exceptions
                MessageBox.Show("MySQL Error: " & ex.Message)
            Catch ex As Exception
                ' Handle any general exceptions
                MessageBox.Show("Error: " & ex.Message)
            Finally
                ' Ensure the connection is closed after the operation
                conn.Close()
            End Try
        End Using

    End Sub
    Private Sub POSF_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        kain()
        Dim connStringd As String = "server=localhost;username=root;password=;database=bayabas_co"

        ' Create the connection object for MySQL
        Using conn As New MySqlConnection(connStringd)
            ' Define your query to retrieve data from QuantityTbl1
            Dim query As String = "SELECT * FROM Ontbl"

            ' Create a command to execute the query
            Using cmd As New MySqlCommand(query, conn)
                ' Create a data adapter to fill the data into a DataTable
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim dt As New DataTable()

                Try
                    ' Open the connection
                    conn.Open()
                    ' Fill the DataTable with the result of the query
                    adapter.Fill(dt)

                    ' Bind the DataTable to the DataGridView
                    DataGridView5.DataSource = dt
                Catch ex As Exception
                    ' Handle any errors (you can log this exception or show a message)
                    MessageBox.Show("Error loading data: " & ex.Message)
                End Try
            End Using
        End Using

        Dim connString As String = "server=localhost;username=root;password=;database=bayabas_co"

        ' Create the connection object for MySQL
        Using conn As New MySqlConnection(connString)
            ' Define your query to retrieve data from QuantityTbl1
            Dim query As String = "SELECT * FROM QuantityTbl1"

            ' Create a command to execute the query
            Using cmd As New MySqlCommand(query, conn)
                ' Create a data adapter to fill the data into a DataTable
                Dim adapter As New MySqlDataAdapter(cmd)
                Dim dt As New DataTable()

                Try
                    ' Open the connection
                    conn.Open()
                    ' Fill the DataTable with the result of the query
                    adapter.Fill(dt)

                    ' Bind the DataTable to the DataGridView
                    DataGridView6.DataSource = dt
                Catch ex As Exception
                    ' Handle any errors (you can log this exception or show a message)
                    MessageBox.Show("Error loading data: " & ex.Message)
                End Try
            End Using
        End Using
        For Each row As DataGridViewRow In DataGridView2.Rows
            ' Reset the row's DefaultCellStyle to the default style
            row.DefaultCellStyle = New DataGridViewCellStyle()
        Next
        DataGridView1.ColumnHeadersVisible = False
        DataGridView2.Rows.Clear()


        Me.KeyPreview = True
        Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

        Using con As New MySqlConnection(connectionString)
            Try
                con.Open() ' Open the connection to the database

                ' Query to fetch VAT data from vattbl and populate DataGridView3
                Dim selectQueryv As String = "SELECT VAT FROM vattbl"

                Using cmd As New MySqlCommand(selectQueryv, con)
                    Using drProduct As MySqlDataReader = cmd.ExecuteReader()
                        ' Clear any existing rows in DataGridView3
                        DataGridView3.Rows.Clear()

                        ' Check if there are any rows returned
                        If drProduct.HasRows Then
                            While drProduct.Read()
                                ' Retrieve the VAT value from the first column (index 0)
                                Dim VATValue As String = drProduct(0).ToString()

                                ' Add the VAT value to DataGridView3
                                DataGridView3.Rows.Add(VATValue)
                            End While

                            ' Check if there is at least one row in DataGridView3
                            If DataGridView3.Rows.Count > 0 Then
                                ' Get the value of the first row and display it in Label21
                                Label21.Text = DataGridView3.Rows(0).Cells(0).Value.ToString()
                            End If
                        Else

                        End If
                    End Using
                End Using

                ' Query to fetch Barcode and Quantity data from quantitytbl1 and populate DataGridView1
                Dim selectQuery As String = "SELECT Barcode, Quantity FROM quantitytbl1"

                Using cmd As New MySqlCommand(selectQuery, con)
                    Using dr As MySqlDataReader = cmd.ExecuteReader()
                        ' Clear any existing rows in DataGridView1
                        DataGridView4.Rows.Clear()

                        If dr.HasRows Then
                            While dr.Read()
                                ' Add the Barcode and Quantity to DataGridView1
                                DataGridView4.Rows.Add(dr("Barcode").ToString(), dr("Quantity").ToString())
                            End While
                        Else

                        End If
                    End Using
                End Using
                Dim query As String = "SELECT IFNULL(MAX(Purchase_number), 0) AS LastPurchaseNumber FROM ONtbl"

                Using cmd As New MySqlCommand(query, con)
                    Dim lastPurchaseNumber As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    ' Increment the last Purchase_number by 1
                    Dim nextPurchaseNumber As Integer = lastPurchaseNumber + 1

                    ' Display the next Purchase_number in Label29
                    Label29.Text = "Order Number : " & nextPurchaseNumber.ToString()
                End Using


            Catch ex As MySqlException
                ' Handle database errors
                MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                ' Handle general errors
                MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

        Label1.Text = username1



    End Sub
    Private Sub LoadData()
        Try
            con.Open()
            Dim query As String = "SELECT producttbl.Barcode, producttbl.Product_Name, producttbl.Cat_name, " &
                                  "producttbl.Color, producttbl.ItemSize, producttbl.Selling_price, quantitytbl.Quantity " &
                                  "FROM producttbl " &
                                  "INNER JOIN quantitytbl ON producttbl.Barcode = quantitytbl.Barcode " &
                                  "LIMIT 1000;"

            Using cmd As New MySqlCommand(query, con)
                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    DataGridView2.Rows.Clear()

                    While dr.Read()
                        DataGridView2.Rows.Add(
                            dr("Barcode"),
                            dr("Product_Name"),
                            dr("Cat_name"),
                            dr("Color"),
                            dr("ItemSize"),
                            dr("Selling_price"),
                            dr("Quantity")
                        )
                    End While
                End Using
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
    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete


    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or control
        End If

        ' Prevent entering more than 4 characters
        If TextBox1.Text.Length >= 9 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 4

        End If
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        Dim searchKeyword As String = TextBox1.Text.Trim()

        ' Clear the DataGridView if the search box is empty
        If String.IsNullOrWhiteSpace(searchKeyword) Then
            DataGridView2.Rows.Clear()
            Return
        End If

        Try
            con.Open()

            ' Query to filter results based on the exact search keyword
            Dim searchQuery As String = "SELECT producttbl.Barcode, producttbl.Product_Name, producttbl.Cat_name, " &
                                "producttbl.Color, producttbl.ItemSize, producttbl.Selling_price, quantitytbl1.Quantity " &
                                "FROM producttbl " &
                                "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                                "WHERE producttbl.Product_Name = @Keyword OR " &
                                "producttbl.Barcode = @Keyword OR " &
                                "producttbl.Cat_name = @Keyword OR " &
                                "producttbl.Color = @Keyword OR " &
                                "producttbl.ItemSize = @Keyword " &
                                "LIMIT 1000;" ' Adjust fields to match the columns you want to search

            Using cmd As New MySqlCommand(searchQuery, con)
                ' Add the search keyword parameter (without % for exact match)
                cmd.Parameters.AddWithValue("@Keyword", searchKeyword)

                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    DataGridView2.Rows.Clear()

                    ' Check if any rows are returned
                    If dr.HasRows Then
                        While dr.Read()
                            ' Extract the Quantity value
                            Dim quantity As Integer = Convert.ToInt32(dr("Quantity"))

                            ' Initialize a new row
                            Dim newRow As New DataGridViewRow()

                            ' Add cells for each column
                            newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Barcode")})
                            newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Product_Name")})
                            newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Cat_name")})
                            newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Color")})
                            newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("ItemSize")})
                            newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Selling_price")})


                            Dim quantityCell As New DataGridViewTextBoxCell With {.Value = quantity}

                            ' Apply conditional formatting
                            If quantity < 5 Then
                                quantityCell.Style.BackColor = System.Drawing.Color.Red  ' Background color
                                quantityCell.Style.ForeColor = System.Drawing.Color.White  ' Text color
                            End If

                            ' Add the Quantity cell to the row
                            newRow.Cells.Add(quantityCell)

                            ' Add the new row to the DataGridView
                            DataGridView2.Rows.Add(newRow)
                            Label40.Visible = True
                        End While

                    End If

                End Using
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try



    End Sub

    Private Sub DataGridView2_DoubleClick(sender As Object, e As EventArgs) Handles DataGridView2.DoubleClick

        If DataGridView2.CurrentRow IsNot Nothing Then
            Dim selectedRow As DataGridViewRow = DataGridView2.CurrentRow
            Panel4.Visible = True

            Dim barcode As String = selectedRow.Cells("Barcode").Value.ToString()
            Dim productName As String = selectedRow.Cells("Product_Name").Value.ToString()
            Dim size As String = selectedRow.Cells("ItemSize").Value.ToString()
            Dim price As String = selectedRow.Cells("Selling_price").Value.ToString()

            ' Display the barcode in TextBox3
            TextBox3.Text = barcode

            ' Optionally display other values in other textboxes
            TextBox4.Text = productName  ' Example for product name
            TextBox5.Text = size         ' Example for size
            TextBox6.Text = price        ' Example for price
        Else
            MsgBox("Please select a valid row.")
        End If




    End Sub

    Private Sub IconButton2_Click(sender As Object, e As EventArgs) Handles IconButton2.Click
        Panel4.Visible = False
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub
    Private currentTransactionNumber As Integer = 1



    Private Sub IconButton1_Click(sender As Object, e As EventArgs)



    End Sub

    Private Sub LoadData1()

    End Sub
    Private Sub IconButton3_Click(sender As Object, e As EventArgs) Handles IconButton3.Click

        TextBox7.Text = TextBox2.Text

        Try
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"
            Using con As New MySqlConnection(connectionString)
                con.Open()

                Dim sufficientQuantity As Boolean = False

                ' Loop through DataGridView4 rows to check quantity
                For Each row As DataGridViewRow In DataGridView4.Rows
                    If row.Cells(0).Value.ToString() = TextBox3.Text.Trim() Then ' Match barcode
                        Dim availableQuantity As Integer = Convert.ToInt32(row.Cells(1).Value)
                        Dim requestedQuantity As Integer = Convert.ToInt32(TextBox7.Text.Trim())

                        If availableQuantity >= requestedQuantity Then
                            sufficientQuantity = True
                        Else
                            MsgBox("Insufficient stock for this barcode. Restock needed.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit Sub
                        End If
                    End If
                Next

                ' Exit if the barcode is not found in DataGridView4
                If Not sufficientQuantity Then
                    MsgBox("Barcode not found in stock or stock is insufficient. Restock needed.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                Dim checkQuery As String = "SELECT COUNT(*) FROM ONtbl WHERE Barcode = @Barcode AND Purchase_number = 0"
                Using cmdCheck As New MySqlCommand(checkQuery, con)
                    cmdCheck.Parameters.AddWithValue("@Barcode", TextBox3.Text.Trim())
                    Dim exists As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

                    If exists > 0 Then
                        ' Barcode exists with Purchase_number = 0, show message and exit early
                        MsgBox("The barcode already exists in the system with Purchase_number = 0.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub  ' Exit the subroutine, preventing further execution
                    End If
                End Using

                ' Prepare the SQL query to insert data if barcode doesn't exist with Purchase_number = 0
                Dim insertQuery As String = "INSERT INTO ONtbl (`Barcode`,`Product_name`,`ItemSize`,`Selling_price`,`Quantity`,`Employee`) VALUES (@Barcode,@Product_name,@ItemSize,@Selling_price,@Quantity,@Employee)"
                Using cmd As New MySqlCommand(insertQuery, con)
                    cmd.Parameters.AddWithValue("@Barcode", TextBox3.Text.Trim())
                    cmd.Parameters.AddWithValue("@Product_name", TextBox4.Text.Trim())
                    cmd.Parameters.AddWithValue("@ItemSize", TextBox5.Text.Trim())
                    cmd.Parameters.AddWithValue("@Selling_price", TextBox6.Text.Trim())
                    cmd.Parameters.AddWithValue("@Quantity", TextBox7.Text.Trim())
                    cmd.Parameters.AddWithValue("@Employee", Label1.Text.Trim()) ' Assuming Label1 contains the employee name or ID

                    cmd.ExecuteNonQuery()
                    MsgBox("Record successfully inserted!")
                End Using

                ' Load the last inserted record
                LoadLastInsertedRecord()
                DataGridView1.ColumnHeadersVisible = True
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            ' Clear the textboxes and hide the panel
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = ""
            TextBox7.Text = ""
            Panel4.Visible = False
        End Try

    End Sub



    Private Sub LoadLastInsertedRecord()
        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using block ensures the connection is properly disposed of
            Using con As New MySqlConnection(connectionString)
                con.Open() ' Open the database connection

                ' Query to select records where Purchase_number = 0
                Dim query As String = "SELECT order_number, Barcode, Product_Name, ItemSize, Selling_price, Quantity, Purchase_number " &
                                      "FROM ONtbl WHERE Purchase_number = 0"

                ' Execute the query and load the results into the DataGridView
                Using cmd As New MySqlCommand(query, con)
                    Using dr As MySqlDataReader = cmd.ExecuteReader()
                        DataGridView1.Rows.Clear() ' Clear existing rows in the DataGridView

                        If dr.HasRows Then
                            While dr.Read()
                                ' Check if the barcode already exists in the DataGridView and if Purchase_number is 0
                                Dim barcodeExists As Boolean = False
                                For Each row As DataGridViewRow In DataGridView1.Rows
                                    ' Check if the barcode exists and if Purchase_number is 0
                                    If row.Cells(1).Value.ToString() = dr("Barcode").ToString() AndAlso
                                       row.Cells(6).Value.ToString() = "0" Then  ' Checking Purchase_number column (index 6)
                                        barcodeExists = True
                                        Exit For
                                    End If
                                Next

                                ' If barcode does not exist and Purchase_number is 0, add the new row
                                If Not barcodeExists Then
                                    DataGridView1.Rows.Add(
                                        dr("order_number").ToString(),
                                        dr("Barcode").ToString(),
                                        dr("Product_Name").ToString(),
                                        dr("ItemSize").ToString(),
                                        dr("Selling_price").ToString(),
                                        dr("Quantity").ToString(),
                                        dr("Purchase_number").ToString()
                                    )
                                Else
                                    MsgBox("The barcode " & dr("Barcode").ToString() & " already exists in the grid with Purchase_number = 0.", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                                End If
                            End While
                        Else

                        End If
                    End Using
                End Using
            End Using

            ' Hide the order_number (index 0) and Purchase_number (index 6) columns
            DataGridView1.Columns(0).Visible = False ' Hide the order_number column
            DataGridView1.Columns(6).Visible = False ' Hide the Purchase_number column

        Catch ex As MySqlException
            ' Handle database-related errors
            MsgBox("Database error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            ' Handle other types of errors
            MsgBox("An error occurred while loading records: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub






    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        UpdateGrandTotal()
    End Sub

    ' Event to recalculate grand total when rows are added
    Private Sub DataGridView1_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        UpdateGrandTotal()
    End Sub

    ' Event to recalculate when rows are removed
    Private Sub DataGridView1_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved
        UpdateGrandTotal()
    End Sub

    ' Grand total calculation method
    Private Sub UpdateGrandTotal()
        Try
            Dim grandTotal As Decimal = 0

            ' Loop through rows 1 to 5 (or fewer if not available)
            For i As Integer = 0 To Math.Min(4, DataGridView1.Rows.Count - 1)
                Dim row As DataGridViewRow = DataGridView1.Rows(i)

                ' Retrieve and parse Selling Price and Quantity
                Dim sellingPrice As Decimal
                Dim quantity As Integer
                Decimal.TryParse(row.Cells(4).Value?.ToString(), sellingPrice) ' Selling Price
                Integer.TryParse(row.Cells(5).Value?.ToString(), quantity) ' Quantity

                ' Add the total for the row
                grandTotal += sellingPrice * quantity
            Next

            ' Display the grand total in Label7
            Label19.Text = grandTotal.ToString("F2")
            Label26.Text = grandTotal.ToString("F2")
        Catch ex As Exception
            MsgBox("An error occurred during total calculation: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs)

    End Sub





    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)




    End Sub

    Private Sub IconButton5_Click(sender As Object, e As EventArgs) 
        Me.Hide()

        ' Dispose the current form (optional, if you don't need the old form anymore)
        Me.Dispose()

        ' Create a new instance of the form (ensure MainMenuf is the correct form name)
        Dim newForm As New POSF() ' Replace MainMenuf with your actual form name
        newForm.Panel1.Visible = True
        newForm.Show()
    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            ' Ensure a row is selected
            If DataGridView1.SelectedRows.Count = 0 Then
                MsgBox("Please select an item to delete.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Confirm deletion
            Dim confirmDelete As DialogResult = MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If confirmDelete = DialogResult.No Then Return

            ' Get the selected row
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Retrieve the Order_number from the selected row
            Dim orderNumber As String = selectedRow.Cells(0).Value?.ToString() ' Replace "Order_number" with the correct column name

            If String.IsNullOrEmpty(orderNumber) Then
                MsgBox("The selected item does not have a valid order_number.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Connect to the database and delete the item
            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' Delete query
                Dim deleteQuery As String = "DELETE FROM ONtbl WHERE order_number = @order_number"
                Using cmd As New MySqlCommand(deleteQuery, con)
                    cmd.Parameters.AddWithValue("@order_number", orderNumber)

                    ' Execute the delete command
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    ' Check if the deletion was successful
                    If rowsAffected > 0 Then
                        ' Remove the row from the DataGridView
                        DataGridView1.Rows.Remove(selectedRow)

                    Else
                        MsgBox("Item could not be deleted. It may not exist in the database.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End Using
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select a row to update the quantity.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Show Panel8
        Panel8.Visible = True

        ' Get the selected row
        Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

        ' Retrieve the current quantity and order_number
        Dim quantity As Integer = Convert.ToInt32(selectedRow.Cells(5).Value)
        Dim orderNumber As Integer = Convert.ToInt32(selectedRow.Cells("order_number").Value)

        ' Display the quantity in the TextBox for editing
        TextBox9.Text = quantity.ToString()

        ' Save the order_number in a hidden control or variable for reference during update
        TextBox9.Tag = orderNumber
    End Sub
    Private Sub IconButton1_Click_1(sender As Object, e As EventArgs) Handles IconButton1.Click
        ' Check if any row is selected

        ' Validate the quantity input
        Dim newQuantity As Integer
        If Not Integer.TryParse(TextBox9.Text, newQuantity) OrElse newQuantity < 0 Then
            MsgBox("Invalid quantity. Please enter a valid positive number.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Retrieve the order_number from the Tag property
        Dim orderNumber As Integer = Convert.ToInt32(TextBox9.Tag)

        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Using block to ensure proper resource disposal
            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' Define the UPDATE query
                Dim query As String = "UPDATE ONtbl SET Quantity = @Quantity WHERE order_number = @order_number"

                ' Execute the query with parameters
                Using cmd As New MySqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@Quantity", newQuantity)
                    cmd.Parameters.AddWithValue("@order_number", orderNumber)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' Update the DataGridView with the new quantity
            For Each row As DataGridViewRow In DataGridView1.Rows
                If Convert.ToInt32(row.Cells("order_number").Value) = orderNumber Then
                    row.Cells(5).Value = newQuantity
                    Exit For
                End If
            Next

            ' Hide Panel8 after successful update
            Panel8.Visible = False

            ' Notify the user
            MsgBox("Quantity updated successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        Dim searchQuery As String = "SELECT producttbl.Barcode, producttbl.Product_Name, producttbl.Cat_name, " &
                            "producttbl.Color, producttbl.ItemSize, producttbl.Selling_price, quantitytbl1.Quantity " &
                            "FROM producttbl " &
                            "INNER JOIN quantitytbl1 ON producttbl.Barcode = quantitytbl1.Barcode " &
                            "LIMIT 1000;"  ' Limit the records to 1000 to avoid large results

        ' Use Using block to manage the connection resource
        Using con As New MySqlConnection("server=localhost;username=root;password=;database=bayabas_co")
            Try
                ' Open the connection to the database
                con.Open()

                ' Create the command with the query
                Using cmd As New MySqlCommand(searchQuery, con)
                    ' Execute the query and get the result using a DataReader
                    Using dr As MySqlDataReader = cmd.ExecuteReader()
                        DataGridView2.Rows.Clear()  ' Clear existing rows in DataGridView

                        ' Check if there are rows in the result set
                        If dr.HasRows Then
                            While dr.Read()
                                ' Read the Quantity from the result set
                                Dim quantity As Integer = Convert.ToInt32(dr("Quantity"))

                                ' Initialize a new row
                                Dim newRow As New DataGridViewRow()

                                ' Add cells to the row for each column in the result
                                newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Barcode")})
                                newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Product_Name")})
                                newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Cat_name")})
                                newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Color")})
                                newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("ItemSize")})
                                newRow.Cells.Add(New DataGridViewTextBoxCell With {.Value = dr("Selling_price")})

                                ' Add the quantity cell
                                Dim quantityCell As New DataGridViewTextBoxCell With {.Value = quantity}


                                If quantity < 5 Then
                                    quantityCell.Style.BackColor = System.Drawing.Color.Red  ' Background color
                                    quantityCell.Style.ForeColor = System.Drawing.Color.White  ' Text color
                                End If

                                newRow.Cells.Add(quantityCell) ' Add the styled quantity cell to the row

                                ' Add the new row to the DataGridView
                                DataGridView2.Rows.Add(newRow)
                            End While
                        End If
                    End Using
                End Using

            Catch ex As MySqlException
                MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using



    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If DataGridView1.Rows.Count = 0 Then

            Return
        End If

        ' Confirm deletion
        Dim confirmResult As DialogResult = MessageBox.Show("Are you sure you want to delete all records?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirmResult = DialogResult.No Then
            Return
        End If

        Try
            ' Open the connection to the database
            con.Open()

            ' Loop through all rows in DataGridView1
            For Each row As DataGridViewRow In DataGridView1.Rows
                ' Get the order_number value from the row
                Dim orderNumber As String = row.Cells("order_number").Value.ToString()

                ' Delete query
                Dim deleteQuery As String = "DELETE FROM ontbl WHERE order_number = @Order_number"

                Using cmd As New MySqlCommand(deleteQuery, con)
                    cmd.Parameters.AddWithValue("@order_number", orderNumber)
                    cmd.ExecuteNonQuery()
                End Using
            Next


            DataGridView1.Rows.Clear()
            TextBox10.Text = ""
            Label26.Text = ""
            Label27.Text = ""
            Label19.Text = ""
            Label20.Text = ""
            Label22.Text = ""


        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

    End Sub
    Private Sub POSF_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Check if the F1 key is pressed
        If e.KeyCode = Keys.F3 Then

            Try
                ' Define the connection string
                Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

                ' Open the connection within a 'Using' block
                Using con As New MySqlConnection(connectionString)
                    ' Open the connection
                    con.Open()

                    ' Step 1: Retrieve the global maximum Purchase_number
                    Dim newPurchaseNumber As Integer
                    Using cmd As New MySqlCommand("SELECT IFNULL(MAX(Purchase_number), 0) FROM ONtbl", con)
                        Dim result = cmd.ExecuteScalar()
                        If IsDBNull(result) OrElse result Is Nothing Then
                            newPurchaseNumber = 1 ' Start with 1 if no Purchase_numbers exist
                        Else
                            newPurchaseNumber = Convert.ToInt32(result) + 1 ' Increment for the new transaction
                        End If
                    End Using

                    ' Step 2: Get the current date and time
                    Dim processedDate As DateTime = DateTime.Now

                    ' Step 3: Start processing the rows
                    For Each row As DataGridViewRow In DataGridView1.Rows
                        If row.IsNewRow Then Continue For ' Skip the new row placeholder

                        ' Retrieve the barcode
                        Dim barcode As String = row.Cells(1).Value?.ToString()

                        ' Step 4: Update the database with the same Purchase_number and current date
                        Dim updateQuery As String = "UPDATE ONtbl SET Purchase_number = @Purchase_number, Purchase_date = @Purchase_date " &
                                            "WHERE Barcode = @Barcode AND Purchase_number = 0"
                        Using updateCmd As New MySqlCommand(updateQuery, con)
                            updateCmd.Parameters.AddWithValue("@Purchase_number", newPurchaseNumber) ' Use the same Purchase_number for this transaction
                            updateCmd.Parameters.AddWithValue("@Purchase_date", processedDate) ' Add the processing date and time
                            updateCmd.Parameters.AddWithValue("@Barcode", barcode)
                            updateCmd.ExecuteNonQuery()
                        End Using
                    Next

                    ' Step 5: Notify the user
                    MsgBox("Transaction completed! All rows updated with Purchase_number: " & newPurchaseNumber & vbCrLf & "Processed on: " & processedDate, MessageBoxButtons.OK, MessageBoxIcon.Information)

                End Using

            Catch ex As MySqlException
                MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
        If e.KeyCode = Keys.F9 Then
            If DataGridView1.Rows.Count = 0 Then

                Return
            End If

            ' Confirm deletion
            Dim confirmResult As DialogResult = MessageBox.Show("Are you sure you want to delete all records?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If confirmResult = DialogResult.No Then
                Return
            End If

            Try
                ' Open the connection to the database
                con.Open()

                ' Loop through all rows in DataGridView1
                For Each row As DataGridViewRow In DataGridView1.Rows
                    ' Get the order_number value from the row
                    Dim orderNumber As String = row.Cells("order_number").Value.ToString()

                    ' Delete query
                    Dim deleteQuery As String = "DELETE FROM ontbl WHERE order_number = @Order_number"

                    Using cmd As New MySqlCommand(deleteQuery, con)
                        cmd.Parameters.AddWithValue("@order_number", orderNumber)
                        cmd.ExecuteNonQuery()
                    End Using
                Next

                ' Clear DataGridView1 after deletion
                DataGridView1.Rows.Clear()

                MsgBox("All records have been deleted successfully.", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As MySqlException
                MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try
        End If
        If e.KeyCode = Keys.F3 Then
            Try
                ' Ensure a row is selected
                If DataGridView1.SelectedRows.Count = 0 Then
                    MsgBox("Please select an item to delete.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Confirm deletion
                Dim confirmDelete As DialogResult = MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If confirmDelete = DialogResult.No Then Return

                ' Get the selected row
                Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

                ' Retrieve the Order_number from the selected row
                Dim orderNumber As String = selectedRow.Cells(0).Value?.ToString() ' Replace "Order_number" with the correct column name

                If String.IsNullOrEmpty(orderNumber) Then
                    MsgBox("The selected item does not have a valid order_number.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' Define the connection string
                Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

                ' Connect to the database and delete the item
                Using con As New MySqlConnection(connectionString)
                    con.Open()

                    ' Delete query
                    Dim deleteQuery As String = "DELETE FROM ONtbl WHERE order_number = @order_number"
                    Using cmd As New MySqlCommand(deleteQuery, con)
                        cmd.Parameters.AddWithValue("@order_number", orderNumber)

                        ' Execute the delete command
                        Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                        ' Check if the deletion was successful
                        If rowsAffected > 0 Then
                            ' Remove the row from the DataGridView
                            DataGridView1.Rows.Remove(selectedRow)
                            MsgBox("Item deleted successfully.", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MsgBox("Item could not be deleted. It may not exist in the database.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using
                End Using

            Catch ex As MySqlException
                MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim nextForm As New MainMenuf()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Open the connection within a 'Using' block
            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' Loop through each row in DataGridView1
                For Each row As DataGridViewRow In DataGridView1.Rows
                    ' Skip the new row
                    If row.IsNewRow Then Continue For

                    ' Retrieve values from the DataGridView
                    Dim purchaseNumber As String = row.Cells(6).Value?.ToString()
                    If String.IsNullOrEmpty(purchaseNumber) Then
                        MsgBox("Purchase Number is missing", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Dim quantity As Decimal
                    If Not Decimal.TryParse(row.Cells(5).Value?.ToString(), quantity) Then
                        MsgBox("Invalid Quantity value in DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Dim unitPrice As Decimal
                    If Not Decimal.TryParse(row.Cells(4).Value?.ToString(), unitPrice) Then
                        MsgBox("Invalid Unit Price value in DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Dim amount As Decimal = quantity * unitPrice ' Calculate Amount

                    Dim item As String = row.Cells(5).Value?.ToString() ' Assuming "Item" is in row index 6
                    Dim dateReceived As DateTime = DateTime.Now ' Use the current date

                    ' Retrieve Employee and Net values from the Labels instead of DataGridView
                    Dim employee As String = If(Not String.IsNullOrEmpty(Label1.Text), Label1.Text, "Unknown Employee")
                    Dim net As Decimal
                    If Not Decimal.TryParse(Label26.Text, net) Then
                        MsgBox("Invalid Net value in Label26", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    ' Ensure Purchase_number is treated as an integer (remove leading zeroes)
                    Dim purchaseNumberInt As Integer = Convert.ToInt32(purchaseNumber)

                    ' Prepare the SQL INSERT query
                    Dim insertQuery As String = "INSERT INTO salesrtbl (Order_number, Amount, Item, Date, Employee, Net) " &
                            "VALUES (@Purchase_number, @Amount, @Item, @Date, @Employee, @Net)"

                    ' Create a new MySqlCommand to execute the query
                    Using cmd As New MySqlCommand(insertQuery, con)
                        ' Add parameters to the query to prevent SQL injection
                        cmd.Parameters.AddWithValue("@Purchase_number", purchaseNumberInt)
                        cmd.Parameters.AddWithValue("@Amount", amount)
                        cmd.Parameters.AddWithValue("@Item", item)
                        cmd.Parameters.AddWithValue("@Date", dateReceived)  ' Now includes both Date and Time
                        cmd.Parameters.AddWithValue("@Employee", employee)
                        cmd.Parameters.AddWithValue("@Net", net)

                        ' Execute the query to insert the data into the table
                        cmd.ExecuteNonQuery()
                    End Using
                Next
            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Open the connection within a 'Using' block
            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' Loop through DataGridView1 to match barcodes with DataGridView4
                For Each row1 As DataGridViewRow In DataGridView1.Rows
                    ' Skip new row placeholder
                    If row1.IsNewRow Then Continue For

                    Dim barcode1 As String = row1.Cells(1).Value.ToString()
                    Dim quantity1 As Integer = Convert.ToInt32(row1.Cells(5).Value)

                    For Each row4 As DataGridViewRow In DataGridView4.Rows
                        Dim barcode4 As String = row4.Cells(0).Value.ToString()

                        If barcode1 = barcode4 Then
                            Dim quantity4 As Integer = Convert.ToInt32(row4.Cells(1).Value)

                            ' Prevent subtraction if quantity in DataGridView4 is 1 or less
                            If quantity4 <= 0 Then
                                MsgBox("Quantity for barcode " & barcode4 & " is at the minimum value. You need to restock.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Exit Sub ' Exit the entire process
                            End If

                            ' Calculate the new quantity after subtraction
                            Dim newQuantity As Integer = quantity4 - quantity1

                            ' Ensure the new quantity doesn't go below 1


                            ' Update the value in DataGridView4
                            row4.Cells(1).Value = newQuantity

                            ' Prepare the UPDATE query to update the quantity in quantitytbl1 in the database
                            Dim updateQuery As String = "UPDATE quantitytbl1 SET Quantity = @newQuantity WHERE Barcode = @barcode"

                            Using cmd As New MySqlCommand(updateQuery, con)
                                cmd.Parameters.AddWithValue("@newQuantity", newQuantity)
                                cmd.Parameters.AddWithValue("@barcode", barcode4)

                                ' Execute the update query to update the quantity in the database
                                cmd.ExecuteNonQuery()
                            End Using

                            ' Exit the loop once the barcode is found and updated
                            Exit For
                        End If
                    Next
                Next

                Dim newPurchaseNumber As Integer
                Using cmd As New MySqlCommand("SELECT IFNULL(MAX(Purchase_number), 0) FROM ONtbl", con)
                    Dim result = cmd.ExecuteScalar()
                    newPurchaseNumber = If(IsDBNull(result), 1, Convert.ToInt32(result) + 1) ' Increment purchase number
                End Using

                ' Get the current date and time
                Dim processedDate As DateTime = DateTime.Now
                Dim Md As String = Label25.Text
                Dim Disc As String = Label31.Text
                Dim DiscWithPercentage As String = Disc & "%" ' Add % to Disc value
                Dim combinedDiscount As String = Md & ", " & DiscWithPercentage ' Combine Md and DiscWithPercentage

                ' Update ONtbl with new purchase number, date, and total calculation
                For Each row As DataGridViewRow In DataGridView1.Rows
                    If row.IsNewRow Then Continue For ' Skip new row placeholder

                    Dim barcode As String = row.Cells(1).Value?.ToString()
                    Dim sellingPrice As Decimal = Convert.ToDecimal(row.Cells(4).Value) ' Column index for Selling Price
                    Dim quantity As Integer = Convert.ToInt32(row.Cells(5).Value)      ' Column index for Quantity
                    Dim total As Decimal = sellingPrice * quantity                     ' Calculate Total

                    ' Update the record in ONtbl
                    Dim updateQuery As String = "UPDATE ONtbl SET Purchase_number = @Purchase_number, Manual_Discount = @Manual_Discount, Purchase_date = @Purchase_date, Total = @Total WHERE Barcode = @Barcode AND Purchase_number = 0"
                    Using updateCmd As New MySqlCommand(updateQuery, con)
                        updateCmd.Parameters.AddWithValue("@Purchase_number", newPurchaseNumber)
                        updateCmd.Parameters.AddWithValue("@Purchase_date", processedDate)
                        updateCmd.Parameters.AddWithValue("@Total", total)
                        updateCmd.Parameters.AddWithValue("@Barcode", barcode)
                        updateCmd.Parameters.AddWithValue("@Manual_Discount", combinedDiscount) ' Store Md and DiscWithPercentage combined
                        updateCmd.ExecuteNonQuery()
                    End Using
                Next


                MessageBox.Show("Transaction is successfully completed.", "Transaction Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                AddHandler printDoc.PrintPage, AddressOf PrintReceipt
                printDoc.Print()
                Button7.Enabled = True
                DataGridView1.Rows.Clear()

            End Using
        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Label27.Text = ""
        TextBox10.Text = ""

        Me.Hide()
        Me.Dispose()
        Dim newForm As New POSF()
        newForm.Show()

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)


    End Sub
    Private Sub ulit()
        Dim totalValue As Double = 0
        Double.TryParse(Label19.Text, totalValue)

        ' Retrieve the percentage from Label21 (e.g., "12%")
        Dim percentageText As String = Label21.Text
        Dim percentage As Double = 0

        ' Use Regular Expression to extract the numeric part of the percentage
        Dim match As Match = Regex.Match(percentageText, "\d+(\.\d+)?")
        If match.Success Then
            Double.TryParse(match.Value, percentage)
        End If

        ' Calculate the value to subtract (percentage of totalValue)
        Dim valueToSubtract As Double = (percentage / 100) * totalValue

        ' Subtract the valueToSubtract from the total value in Label19
        Dim result As Double = totalValue - valueToSubtract

        ' Display the result in Label20
        Label20.Text = result.ToString("F2") ' Format result to 2 decimal places

        ' Update Label21 to include the "%" symbol after the percentage value
        Label21.Text = percentage.ToString("F2") & "%" ' Ensure percentage has two decimal places and the "%" sign

        Dim textBoxValue As Double
        Dim labelValue As Double

        ' Try to parse TextBox10 and Label26 values
        If Double.TryParse(TextBox10.Text, textBoxValue) AndAlso Double.TryParse(Label26.Text, labelValue) Then
            ' Perform the subtraction and display the result in Label27
            Label27.Text = (textBoxValue - labelValue).ToString()
        Else
            ' Show an error message if the values cannot be parsed
            MsgBox("Please enter valid numeric values in TextBox10 and Label26.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        TextBox10.Enabled = True

        ' Retrieve the total value (Total Amount) from Label19
        Dim totalValue As Double = 0
        Double.TryParse(Label19.Text, totalValue) ' Parse the Total Amount from Label19

        ' Retrieve VAT rate from Label21 (e.g., "8.99%")
        Dim vatRatePercentage As String = Label21.Text
        Dim vatRate As Double = 0

        ' Use Regular Expression to extract the numeric part of the percentage (e.g., "8.99%" -> 8.99)
        Dim match As Match = Regex.Match(vatRatePercentage, "\d+(\.\d+)?")
        If match.Success Then
            Double.TryParse(match.Value, vatRate) ' Parse the numeric value of VAT rate
        End If

        ' Convert VAT Rate from percentage to decimal (e.g., 8.99% -> 0.0899)
        vatRate /= 100

        ' Calculate the Vatable Sales (pre-VAT price)
        Dim vatableSales As Double = totalValue / (1 + vatRate)

        ' Display the result in Label20
        Label20.Text = vatableSales.ToString("F2")


        Dim labelValue As Decimal
        Dim textboxValue As Decimal

        ' Try parsing the values from Label26 and TextBox10
        If Decimal.TryParse(Label26.Text, labelValue) AndAlso Decimal.TryParse(TextBox10.Text, textboxValue) Then
            ' Check if the value in Label26 (Total Price) is greater than the value in TextBox10 (Payment)
            If labelValue > textboxValue Then
                ' Show error message if Total Price is greater than Payment
                MessageBox.Show("The value in Total price is greater than the value in Payment.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                ' Perform the subtraction (Payment - Total Price) and store the result in Label27
                Label27.Text = (textboxValue - labelValue).ToString()
                Button2.Enabled = True
            End If
        Else
            ' Handle the case where the values could not be parsed as numbers
            MessageBox.Show("Invalid input. Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If


    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs)
        Try
            ' Define the connection string
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            ' Open the connection within a 'Using' block
            Using con As New MySqlConnection(connectionString)
                con.Open()

                ' Loop through each row in DataGridView1
                For Each row As DataGridViewRow In DataGridView1.Rows
                    ' Skip the new row
                    If row.IsNewRow Then Continue For

                    ' Retrieve values from the DataGridView
                    Dim purchaseNumber As String = row.Cells(6).Value?.ToString()
                    If String.IsNullOrEmpty(purchaseNumber) Then
                        MsgBox("Purchase Number is missing", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Dim quantity As Decimal
                    If Not Decimal.TryParse(row.Cells(5).Value?.ToString(), quantity) Then
                        MsgBox("Invalid Quantity value in DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Dim unitPrice As Decimal
                    If Not Decimal.TryParse(row.Cells(4).Value?.ToString(), unitPrice) Then
                        MsgBox("Invalid Unit Price value in DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Dim amount As Decimal = quantity * unitPrice ' Calculate Amount

                    Dim item As String = row.Cells(5).Value?.ToString() ' Assuming "Item" is in row index 6
                    Dim dateReceived As DateTime = DateTime.Now ' Use the current date

                    ' Retrieve Employee and Net values from the Labels instead of DataGridView
                    Dim employee As String = If(Not String.IsNullOrEmpty(Label1.Text), Label1.Text, "Unknown Employee")
                    Dim net As Decimal
                    If Not Decimal.TryParse(Label26.Text, net) Then
                        MsgBox("Invalid Net value in Label26", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    ' Prepare the SQL INSERT query
                    Dim insertQuery As String = "INSERT INTO salesrtbl (Order_number, Amount, Item, Date, Employee, Net) " &
                                        "VALUES (@Purchase_number, @Amount, @Item, @Date, @Employee, @Net)"

                    ' Create a new MySqlCommand to execute the query
                    Using cmd As New MySqlCommand(insertQuery, con)
                        ' Add parameters to the query to prevent SQL injection
                        cmd.Parameters.AddWithValue("@Purchase_number", purchaseNumber) ' Use Purchase_number from DataGridView
                        cmd.Parameters.AddWithValue("@Amount", amount) ' The calculated Amount
                        cmd.Parameters.AddWithValue("@Item", item)
                        cmd.Parameters.AddWithValue("@Date", dateReceived) ' Use current date
                        cmd.Parameters.AddWithValue("@Employee", employee) ' From Label1
                        cmd.Parameters.AddWithValue("@Net", net) ' From Label26

                        ' Execute the query to insert the data into the table
                        cmd.ExecuteNonQuery()
                    End Using
                Next

                ' Display a success message after all rows are inserted
                MsgBox("All orders inserted successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End Using

        Catch ex As MySqlException
            MsgBox("Database error: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try




    End Sub

    Private Sub Button8_Click_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click_3(sender As Object, e As EventArgs)
        AddHandler printDoc.PrintPage, AddressOf PrintReceipt
        printDoc.Print()
    End Sub

    Private Sub Button8_Click_2(sender As Object, e As EventArgs) Handles Button8.Click
        Label24.Text = "Manual Discount"

        Panel2.Visible = True
    End Sub

    Private Sub IconButton5_Click_1(sender As Object, e As EventArgs) Handles IconButton5.Click
        Label25.Text = TextBox18.Text
        Label31.Text = TextBox11.Text
        Label31.Visible = True
        Panel2.Visible = False

        Try
            ' Get the percentage value from Label31 (as a double)
            Dim percentage As Double = Convert.ToDouble(Label31.Text)

            ' Get the initial value from Label20 (assuming it's a number, e.g., 100)
            Dim initialValue As Double = Convert.ToDouble(Label19.Text)

            ' Calculate the value to add (percentage of the initial value)
            Dim valueToAdd As Double = (percentage / 100) * initialValue

            Dim difference As Double = valueToAdd

            ' Set the difference to Label22 (show the value added)
            Label22.Text = difference.ToString("F2") ' Display the amount added as the difference

        Catch ex As Exception
            ' Handle any errors, e.g., invalid inputs in the labels

        End Try
        Try
            ' Get the values from Label22 and Label19
            Dim value1 As Double = Convert.ToDouble(Label19.Text)
            Dim value2 As Double = Convert.ToDouble(Label22.Text)

            ' Perform the subtraction
            Dim result As Double = value1 - value2

            ' Display the result in Label26
            Label26.Text = result.ToString("F2") ' Format the result to 2 decimal places

        Catch ex As Exception
            ' Handle any errors (e.g., invalid inputs in the labels)

        End Try
        Try
            ' Get the values from TextBox10 and Label10
            Dim value1 As Double = Convert.ToDouble(TextBox10.Text)
            Dim value2 As Double = Convert.ToDouble(Label26.Text)

            ' Perform the subtraction
            Dim result As Double = value1 - value2

            ' Display the result in Label27
            Label27.Text = result.ToString("F2") ' Format the result to 2 decimal places

        Catch ex As Exception
            ' Handle any errors (e.g., invalid inputs in the label or textbox)

        End Try



    End Sub

    Private Sub IconButton6_Click(sender As Object, e As EventArgs) Handles IconButton6.Click
        Panel2.Visible = False
        Label24.Text = "None"
        TextBox11.Text = ""
        Label31.Text = ""
        UpdateGrandTotal()
        ulit()
        Label22.Text = "#"

    End Sub

    Private Sub TextBox11_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox11.KeyPress
        ' Allow only numeric characters (0-9) and control characters (e.g., backspace)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore any key that is not a digit or control character
        End If

        ' Ensure the length of the text in the TextBox does not exceed 2 characters
        If TextBox11.Text.Length >= 2 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 2
        End If
    End Sub

    Private Sub TextBox11_TextChanged(sender As Object, e As EventArgs) Handles TextBox11.TextChanged

        ' Check if TextBox11 is empty
        If String.IsNullOrWhiteSpace(TextBox11.Text) Then
            ' Enable IconButton5 when TextBox11 is empty
            IconButton5.Enabled = False
        Else
            ' Disable IconButton5 when TextBox11 has text
            IconButton5.Enabled = True
        End If
    End Sub

    Private Sub NumberButton_Click(sender As Object, e As EventArgs) _


        ' Declare a variable to store the value from the clicked button
        Dim value As String = ""

        ' Get the clicked button and assign the number or symbol to value
        If TypeOf sender Is IconButton Then
            ' Using Select Case to assign a value based on which button was clicked
            Select Case CType(sender, IconButton).Name
                Case "IconButton7" ' Button for "1"
                    value = "1"
                Case "IconButton23" ' Button for "2"
                    value = "2"
                Case "IconButton13" ' Button for "3"
                    value = "3"
                Case "IconButton8" ' Button for "4"
                    value = "4"
                Case "IconButton22" ' Button for "5"
                    value = "5"
                Case "IconButton11" ' Button for "6"
                    value = "6"
                Case "IconButton12" ' Button for "7"
                    value = "7"
                Case "IconButton21" ' Button for "8"
                    value = "8"
                Case "IconButton10" ' Button for "9"
                    value = "9"
                Case "IconButton19" ' Button for "0"
                    value = "0"
                Case "IconButton20" ' Button for "."
                    value = "."
            End Select
        End If

        Dim targetTextBox As TextBox = Nothing

        If ActiveControl Is TextBox1 Then
            targetTextBox = TextBox1
        ElseIf ActiveControl Is TextBox10 Then
            targetTextBox = TextBox10
        End If

        If targetTextBox IsNot Nothing Then
            targetTextBox.Text &= value
        End If
    End Sub


    Private Sub ClearButton_Click(sender As Object, e As EventArgs) _


        ' Clear the text in TextBox1
        TextBox1.Clear()
        TextBox11.Clear()
        TextBox10.Clear()
        TextBox2.Clear()
        TextBox9.Clear()
    End Sub
    Private Sub IconButton7_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub IconToolStripButton1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Panel13.Visible = False
    End Sub

    Private Sub UpdateDatabaseQuantity(barcode As String, newQuantity As Integer)
        Dim connString As String = "server=localhost;username=root;password=;database=bayabas_co"
        Using conn As New MySqlConnection(connString)
            Dim query As String = "UPDATE QuantityTbl1 SET Quantity = @Quantity WHERE Barcode = @Barcode"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Quantity", newQuantity)
                cmd.Parameters.AddWithValue("@Barcode", barcode)

                Try
                    conn.Open()
                    cmd.ExecuteNonQuery()  ' Execute the update query
                    MessageBox.Show("Database updated successfully.")
                Catch ex As Exception
                    MessageBox.Show("Error updating the database: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub


    Private Sub TextBox13_TextChanged(sender As Object, e As EventArgs) Handles TextBox13.TextChanged

        ' Check if the barcode field (TextBox13) is empty
        If String.IsNullOrEmpty(TextBox13.Text) Then
            ' If TextBox13 is empty, clear other textboxes
            TextBox14.Clear()
            TextBox15.Clear()
            TextBox16.Clear()
        Else
            ' Get the barcode entered by the user in TextBox13
            Dim barcode As String = TextBox13.Text
            ' Assuming purchase_number is stored in TextBox12
            Dim purchaseNumber As String = TextBox12.Text

            ' Define the connection string
            Dim connString As String = "server=localhost;username=root;password=;database=bayabas_co"  ' Replace with actual connection string

            Using conn As New MySqlConnection(connString)
                ' Define the query to retrieve product details based on both Barcode and purchase_number
                Dim query As String = "SELECT Product_name, ItemSize, Selling_price FROM Ontbl WHERE Barcode = @Barcode AND purchase_number = @PurchaseNumber"

                Using cmd As New MySqlCommand(query, conn)
                    ' Add the barcode and purchase_number parameters
                    cmd.Parameters.AddWithValue("@Barcode", barcode)
                    cmd.Parameters.AddWithValue("@PurchaseNumber", purchaseNumber)

                    Try
                        ' Open the database connection
                        conn.Open()

                        ' Execute the query and read the data
                        Using reader As MySqlDataReader = cmd.ExecuteReader()
                            If reader.HasRows Then
                                ' Read the data and assign it to the textboxes
                                reader.Read()
                                TextBox14.Text = reader("Product_name").ToString()   ' Product name goes to TextBox14
                                TextBox15.Text = reader("ItemSize").ToString()       ' ItemSize goes to TextBox15
                                TextBox16.Text = reader("Selling_price").ToString()  ' SellingPrice goes to TextBox16
                            Else
                                ' If no record is found, show a message and clear the textboxes
                                TextBox14.Clear()
                                TextBox15.Clear()
                                TextBox16.Clear()

                            End If
                        End Using
                    Catch ex As Exception
                        ' Handle any errors that occur during the database operation
                        MessageBox.Show("Error: " & ex.Message)
                    End Try
                End Using
            End Using
        End If

    End Sub




    Private Sub Button10_Click_1(sender As Object, e As EventArgs) Handles Button10.Click
        Panel13.Visible = True
    End Sub

    Private Sub Label34_Click(sender As Object, e As EventArgs) Handles Label34.Click

    End Sub

    Private Sub IconButton4_Click(sender As Object, e As EventArgs) Handles IconButton4.Click
        Panel8.Visible = False
    End Sub
    Private Sub textbox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        ' Allow only numeric characters (0-9)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or a control character (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If TextBox2.Text.Length >= 4 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub textbox9_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox9.KeyPress
        ' Allow only numeric characters (0-9)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or a control character (e.g., backspace)
        End If

        ' Ensure the length of the text in the TextBox does not exceed 15 characters
        If TextBox9.Text.Length >= 4 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged

    End Sub

    Private Sub TextBox17_TextChanged(sender As Object, e As EventArgs) Handles TextBox17.TextChanged

    End Sub


    Private Sub Button_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim connString As String = "server=localhost;username=root;password=;database=bayabas_co"  ' Replace with actual connection string

        ' Assuming TextBox13 contains the barcode
        Dim barcode As String = TextBox13.Text.Trim()

        ' Check if barcode is empty
        If String.IsNullOrEmpty(barcode) Then
            MessageBox.Show("Please enter a valid barcode.")
            Return
        End If

        ' Assuming TextBox17 contains the additional quantity to add to the current quantity
        Dim additionalQuantity As Integer

        ' Check if additional quantity entered is valid
        If Not Integer.TryParse(TextBox17.Text, additionalQuantity) Then
            MessageBox.Show("Please enter a valid quantity.")
            Return
        End If

        ' Step 1: Retrieve the current quantity from DataGridView5 based on the barcode
        Dim currentQuantityInDataGridView As Integer = 0
        Dim rowIndex As Integer = -1

        For Each row As DataGridViewRow In DataGridView5.Rows
            ' Skip the empty row
            If row.IsNewRow Then Continue For

            ' Check if barcode in DataGridView matches the barcode from TextBox13
            If row.Cells("Barcode").Value.ToString() = barcode Then
                rowIndex = row.Index
                currentQuantityInDataGridView = Convert.ToInt32(row.Cells("Quantity").Value)
                Exit For
            End If
        Next

        ' Step 2: Validate if the quantity in TextBox17 matches the quantity in DataGridView5
        If currentQuantityInDataGridView <> additionalQuantity Then
            MessageBox.Show("The quantity entered in TextBox17 does not match the quantity in DataGridView5 for this barcode.")
            Return
        End If

        ' Step 3: Retrieve the current quantity from the database (quantitytbl1) based on the barcode
        Dim currentQuantity As Integer = 0
        Dim query As String = "SELECT quantity FROM quantitytbl1 WHERE barcode = @barcode"

        Using conn As New MySqlConnection(connString)
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@barcode", barcode)

                Try
                    conn.Open()
                    Dim result As Object = cmd.ExecuteScalar()  ' Get the current quantity for the barcode

                    If result IsNot Nothing Then
                        currentQuantity = Convert.ToInt32(result)
                    Else
                        MessageBox.Show("Barcode not found in the database.")
                        Return
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                    Return
                End Try
            End Using
        End Using

        ' Step 4: Calculate the new quantity by adding the additional quantity
        Dim newQuantity As Integer = currentQuantity + additionalQuantity
        query = "UPDATE quantitytbl1 SET quantity = @newQuantity WHERE barcode = @barcode"

        Using conn As New MySqlConnection(connString)
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@newQuantity", newQuantity)
                cmd.Parameters.AddWithValue("@barcode", barcode)

                Try
                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show("Quantity updated successfully!")
                    Else
                        MessageBox.Show("No matching record found to update.")
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using
        End Using

        ' Step 6: If the entered quantity in TextBox17 matches the current quantity, delete the corresponding record in Ontbl
        If additionalQuantity = currentQuantityInDataGridView Then
            ' Assuming TextBox12 contains the purchase number
            Dim purchaseNumber As Integer = 0
            If Not Integer.TryParse(TextBox12.Text, purchaseNumber) Then
                MessageBox.Show("Please enter a valid purchase number.")
                Return
            End If

            ' Delete the record from Ontbl
            query = "DELETE FROM Ontbl WHERE barcode = @barcode AND purchase_number = @purchaseNumber"

            Using conn As New MySqlConnection(connString)
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@barcode", barcode)
                    cmd.Parameters.AddWithValue("@purchaseNumber", purchaseNumber)

                    Try
                        conn.Open()
                        Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                        If rowsAffected > 0 Then
                            MessageBox.Show("Record deleted successfully!")
                        Else
                            MessageBox.Show("No matching record found.")
                        End If
                    Catch ex As Exception
                        MessageBox.Show("Error: " & ex.Message)
                    End Try
                End Using
            End Using
        End If

        ' Optional: Clear TextBoxes after updating (if needed)
        TextBox13.Clear()
        TextBox17.Clear()
        TextBox12.Clear()



    End Sub

    ' Optional method to update the quantity in the database (if required)
    Private Sub UpdateDatabaseQuantity1(barcode As String, newQuantity As Integer)

    End Sub

    ' Helper method to clear form fields
    Private Sub ClearFormFields()
        TextBox12.Clear()
        TextBox13.Clear()
        TextBox14.Clear()
        TextBox15.Clear()
        TextBox16.Clear()
        TextBox17.Clear()
    End Sub
    Private Sub TextBox10_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox10.KeyPress
        ' Check if the pressed key is not a digit or not a control key (e.g., backspace)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the key if it is not a digit or control key
        End If

        If TextBox10.Text.Length >= 6 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore further key presses if the length exceeds 15
        End If
    End Sub
    Private Sub TextBox10_Click(sender As Object, e As EventArgs) Handles TextBox10.Click
        ' Show Panel3 when TextBox10 is clicked
        Panel12.Visible = False
        Panel11.Visible = True
    End Sub
    Private Sub TextBox1_Click(sender As Object, e As EventArgs) Handles TextBox1.Click
        ' Show Panel3 when TextBox10 is clicked
        Panel12.Visible = True
        Panel11.Visible = False
    End Sub


    Private Sub Button1_Click_4(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label15_Click(sender As Object, e As EventArgs) Handles Label15.Click

    End Sub
    ' Button clicks for TextBox1
    Private Sub IconButton7_Click_1(sender As Object, e As EventArgs) Handles IconButton7.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "1"
        Else

        End If
    End Sub

    Private Sub IconButton23_Click(sender As Object, e As EventArgs) Handles IconButton23.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "2"
        Else

        End If
    End Sub

    Private Sub IconButton13_Click(sender As Object, e As EventArgs) Handles IconButton13.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "3"
        Else

        End If
    End Sub

    Private Sub IconButton8_Click(sender As Object, e As EventArgs) Handles IconButton8.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "4"
        Else

        End If
    End Sub

    Private Sub IconButton22_Click(sender As Object, e As EventArgs) Handles IconButton22.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "5"
        Else

        End If
    End Sub

    Private Sub IconButton11_Click(sender As Object, e As EventArgs) Handles IconButton11.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "6"
        Else

        End If
    End Sub

    Private Sub IconButton12_Click(sender As Object, e As EventArgs) Handles IconButton12.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "7"
        Else

        End If
    End Sub

    Private Sub IconButton21_Click(sender As Object, e As EventArgs) Handles IconButton21.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "8"
        Else

        End If
    End Sub

    Private Sub IconButton10_Click(sender As Object, e As EventArgs) Handles IconButton10.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "9"
        Else

        End If
    End Sub

    Private Sub IconButton19_Click(sender As Object, e As EventArgs) Handles IconButton19.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "0"
        Else

        End If
    End Sub

    Private Sub IconButton20_Click(sender As Object, e As EventArgs) Handles IconButton20.Click
        If TextBox1.Text.Length < 9 Then
            TextBox1.Text &= "."
        Else

        End If
    End Sub


    ' Button clicks for TextBox10
    Private Sub IconButton30_Click(sender As Object, e As EventArgs) Handles IconButton30.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "1"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton26_Click(sender As Object, e As EventArgs) Handles IconButton26.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "2"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton17_Click(sender As Object, e As EventArgs) Handles IconButton17.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "3"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton29_Click(sender As Object, e As EventArgs) Handles IconButton29.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "4"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton25_Click(sender As Object, e As EventArgs) Handles IconButton25.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "5"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton16_Click(sender As Object, e As EventArgs) Handles IconButton16.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "6"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton28_Click(sender As Object, e As EventArgs) Handles IconButton28.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "7"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton24_Click(sender As Object, e As EventArgs) Handles IconButton24.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "8"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton15_Click(sender As Object, e As EventArgs) Handles IconButton15.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "9"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton27_Click(sender As Object, e As EventArgs) Handles IconButton27.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "0"
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton18_Click(sender As Object, e As EventArgs) Handles IconButton18.Click
        If TextBox10.Text.Length < 6 Then
            TextBox10.Text &= "."
        Else
            MsgBox("Maximum of 6 digits allowed.")
        End If
    End Sub

    Private Sub IconButton14_Click(sender As Object, e As EventArgs) Handles IconButton14.Click
        TextBox10.Text = ""
    End Sub


    Private Sub TextBox10_TextChanged(sender As Object, e As EventArgs) Handles TextBox10.TextChanged

    End Sub

    Private Sub IconButton9_Click(sender As Object, e As EventArgs) Handles IconButton9.Click
        TextBox1.Text = ""
    End Sub

    Private Sub Panel7_Paint(sender As Object, e As PaintEventArgs) Handles Panel7.Paint

    End Sub
End Class
