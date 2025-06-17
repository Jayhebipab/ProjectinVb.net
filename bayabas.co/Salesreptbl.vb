Imports MySql.Data.MySqlClient
Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices

Public Class Salesreptbl
    Private WithEvents printDoc As New Printing.PrintDocument()
    Private WithEvents printDlg As New PrintDialog()
    Private WithEvents printDoc1 As New Printing.PrintDocument()
    Private WithEvents printDlg1 As New PrintDialog()
    Dim printMode As String = String.Empty


    Private Sub IconButton3_Click(sender As Object, e As EventArgs) Handles IconButton3.Click
        Dim nextForm As New MainMenuf()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Salesreptbl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DateTimePicker2.Value = DateTime.Now.AddDays(1)


        DateTimePicker1.Value = New DateTime(2024, 11, 23)


        DateTimePicker1.MinDate = New DateTime(2024, 11, 23)


        ulit()

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub DateTimePicker2_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker2.ValueChanged
        ' Ensure that the date selected in DateTimePicker2 is not more than tomorrow's date
        If DateTimePicker2.Value.Date > DateTime.Now.Date.AddDays(1) Then
            ' Show a message if a future date is selected beyond tomorrow
            MsgBox("Please select a date on or before tomorrow's date.", MsgBoxStyle.Exclamation, "Invalid Date")

            ' Reset DateTimePicker2 to tomorrow's date
            DateTimePicker2.Value = DateTime.Now.Date.AddDays(1)
        Else
            ' Ensure that DateTimePicker2 is not before DateTimePicker1
            If DateTimePicker2.Value.Date < DateTimePicker1.Value.Date Then
                ' Show a message if DateTimePicker2 is before DateTimePicker1
                MsgBox("End date cannot be earlier than the start date.", MsgBoxStyle.Exclamation, "Invalid Date Range")

                ' Reset DateTimePicker2 to match DateTimePicker1 if invalid
                DateTimePicker2.Value = DateTimePicker1.Value.Date
            End If
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        Try
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            Using con As New MySqlConnection(connectionString)
                con.Open()

                Dim startDate As Date = DateTimePicker1.Value.Date
                Dim endDate As Date = DateTimePicker2.Value.Date

                If startDate > endDate Then
                    MsgBox("Start date cannot be greater than end date.", MsgBoxStyle.Exclamation, "Invalid Date Range")
                    Exit Sub
                End If

                Dim query As String = "SELECT Purchase_date, Barcode, Product_Name, Total, Quantity, Manual_Discount, Purchase_number, Employee " &
                                      "FROM ONtbl " &
                                      "WHERE Purchase_date BETWEEN @startDate AND @endDate"

                Using cmd As New MySqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@startDate", startDate)
                    cmd.Parameters.AddWithValue("@endDate", endDate)

                    Using dr As MySqlDataReader = cmd.ExecuteReader()
                        DataGridView1.Rows.Clear()
                        DataGridView1.Columns.Clear() ' Optional: clear columns first to avoid duplication

                        ' Optional: define column names manually if needed
                        DataGridView1.Columns.Add("Purchase_number", "Purchase Number")
                        DataGridView1.Columns.Add("Barcode", "Barcode")
                        DataGridView1.Columns.Add("Product_Name", "Product Name")
                        DataGridView1.Columns.Add("Total", "Total")
                        DataGridView1.Columns.Add("Quantity", "Quantity")
                        DataGridView1.Columns.Add("Manual_Discount", "Discount")
                        DataGridView1.Columns.Add("Purchase_date", "Purchase Date")
                        DataGridView1.Columns.Add("Employee", "Employee")

                        Dim grandTotal As Decimal = 0
                        Dim todayTotal As Decimal = 0
                        Dim todayDate As String = Date.Now.ToString("yyyy-MM-dd")

                        If dr.HasRows Then
                            While dr.Read()
                                Dim rowTotal As Decimal = Convert.ToDecimal(dr("Total"))
                                grandTotal += rowTotal

                                Dim purchaseDate As String = Convert.ToDateTime(dr("Purchase_date")).ToString("yyyy-MM-dd")
                                If purchaseDate = todayDate Then
                                    todayTotal += rowTotal
                                End If

                                DataGridView1.Rows.Add(
                                    dr("Purchase_number").ToString(),
                                    dr("Barcode").ToString(),
                                    dr("Product_Name").ToString(),
                                    rowTotal.ToString("N2"),
                                    dr("Quantity").ToString(),
                                    dr("Manual_Discount").ToString(),
                                    Convert.ToDateTime(dr("Purchase_date")).ToString("yyyy-MM-dd HH:mm:ss"),
                                    dr("Employee").ToString()
                                )
                            End While

                            ' ✅ Format after adding rows
                            DataGridView1.Columns("Total").DefaultCellStyle.Format = "N2"
                            DataGridView1.Columns("Purchase_date").DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss"
                            For Each column As DataGridViewColumn In DataGridView1.Columns
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                            Next
                            Label6.Text = "Grand Total: " & grandTotal.ToString("C2")
                            Label3.Text = "Today's Sales: " & todayTotal.ToString("C2")

                        Else

                        End If
                    End Using
                End Using
            End Using

        Catch ex As MySqlException
            MsgBox("Database error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub




    Private Sub printDoc_PrintPage1(sender As Object, e As Printing.PrintPageEventArgs) Handles printDoc.PrintPage
        ' Check if there are selected rows
        Dim printFont As New Font("Arial", 8)  ' Adjust font size here
        Dim lineHeight As Integer = 15  ' Line height (adjust for better spacing)
        Dim defaultColumnWidth As Integer = 100  ' Default column width

        ' Get the starting position for printing
        Dim yPos As Integer = e.MarginBounds.Top
        Dim xPos As Integer = e.MarginBounds.Left
        Dim columnCount As Integer = DataGridView1.Columns.Count

        ' Specify custom widths for certain columns (for example, "Employee" column)
        Dim columnWidths As New Dictionary(Of String, Integer) From {
        {"Item", 95},
        {"Employee", 93},  ' Example: Employee column is wider
        {"Date", 85}       ' Example: Date column is wider
    }

        If printMode = "single" Then
            ' Print the selected row
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Print the column headers
            For i As Integer = 0 To columnCount - 1
                Dim colName As String = DataGridView1.Columns(i).HeaderText
                Dim columnWidth As Integer

                ' Use custom column width if available, otherwise use default
                If columnWidths.ContainsKey(colName) Then
                    columnWidth = columnWidths(colName)
                Else
                    columnWidth = defaultColumnWidth
                End If

                e.Graphics.DrawString(colName, printFont, Brushes.Black, xPos + i * columnWidth, yPos)
            Next

            yPos += lineHeight  ' Move position after header row

            ' Print the cells in the selected row
            For i As Integer = 0 To columnCount - 1
                Dim colName As String = DataGridView1.Columns(i).HeaderText
                Dim columnWidth As Integer

                ' Use custom column width if available, otherwise use default
                If columnWidths.ContainsKey(colName) Then
                    columnWidth = columnWidths(colName)
                Else
                    columnWidth = defaultColumnWidth
                End If

                e.Graphics.DrawString(selectedRow.Cells(i).Value.ToString(), printFont, Brushes.Black, xPos + i * columnWidth, yPos)
            Next

        ElseIf printMode = "all" Then
            ' Print all rows in the DataGridView
            ' Print the column headers
            For i As Integer = 0 To columnCount - 1
                Dim colName As String = DataGridView1.Columns(i).HeaderText
                Dim columnWidth As Integer

                ' Use custom column width if available, otherwise use default
                If columnWidths.ContainsKey(colName) Then
                    columnWidth = columnWidths(colName)
                Else
                    columnWidth = defaultColumnWidth
                End If

                e.Graphics.DrawString(colName, printFont, Brushes.Black, xPos + i * columnWidth, yPos)
            Next

            yPos += lineHeight  ' Move position after header row

            ' Print each row in the DataGridView
            For Each row As DataGridViewRow In DataGridView1.Rows
                ' Skip the last empty row (if any)
                If row.IsNewRow Then Continue For

                ' Print each cell in the row
                For i As Integer = 0 To columnCount - 1
                    Dim colName As String = DataGridView1.Columns(i).HeaderText
                    Dim columnWidth As Integer

                    ' Use custom column width if available, otherwise use default
                    If columnWidths.ContainsKey(colName) Then
                        columnWidth = columnWidths(colName)
                    Else
                        columnWidth = defaultColumnWidth
                    End If

                    e.Graphics.DrawString(row.Cells(i).Value.ToString(), printFont, Brushes.Black, xPos + i * columnWidth, yPos)
                Next

                yPos += lineHeight  ' Move to the next line after each row

                ' Check if we need to continue to the next page
                If yPos > e.MarginBounds.Bottom Then
                    e.HasMorePages = True
                    Return
                End If
            Next
        End If

        ' After all rows are printed, set HasMorePages to False
        e.HasMorePages = False
    End Sub

    Private Sub IconButton1_Click(sender As Object, e As EventArgs) Handles IconButton1.Click
        ' Check if any row is selected
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Set printMode to "single" for printing a single row
            printMode = "single"

            ' Show the PrintDialog
            printDlg.Document = printDoc
            If printDlg.ShowDialog() = DialogResult.OK Then
                ' Start printing the selected row
                printDoc.Print()
            End If
        Else
            MsgBox("Please select a row to print.")
        End If
    End Sub
    Private Sub ExportToExcel()
        Try
            ' Create an Excel application object
            Dim excelApp As New Excel.Application()

            ' Make Excel visible
            excelApp.Visible = True

            ' Create a new workbook
            Dim workBook As Excel.Workbook = excelApp.Workbooks.Add()
            Dim workSheet As Excel.Worksheet = workBook.Sheets(1)

            ' Export the DataGridView columns to the Excel sheet
            For i As Integer = 0 To DataGridView1.Columns.Count - 1
                workSheet.Cells(1, i + 1) = DataGridView1.Columns(i).HeaderText
            Next

            ' Export the DataGridView rows to the Excel sheet
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                For j As Integer = 0 To DataGridView1.Columns.Count - 1
                    workSheet.Cells(i + 2, j + 1) = DataGridView1.Rows(i).Cells(j).Value.ToString()
                Next
            Next

            ' Adjust the column width to fit content
            workSheet.Columns.AutoFit()

            ' Calculate Grand Total for the 'Amount' column (Column 4, which is Column Index 3)
            Dim grandTotalRow As Integer = DataGridView1.Rows.Count + 2 ' Row where grand total will be displayed

            ' Add a label for "Grand Total"
            workSheet.Cells(grandTotalRow, 1) = "Grand Total"

            ' Column where the 'Amount' (or Total) is located, which is Column 4 (Index 3)
            Dim amountColumnIndex As Integer = 3 ' Column index for the 'Amount' (4th column, so index = 3)

            ' Calculate and sum the 'Amount' column (Total column)
            Dim grandTotal As Double = 0
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                If IsNumeric(DataGridView1.Rows(i).Cells(amountColumnIndex).Value) Then
                    grandTotal += Convert.ToDouble(DataGridView1.Rows(i).Cells(amountColumnIndex).Value)
                End If
            Next

            ' Write the grand total into the 'Amount' column of the last row
            workSheet.Cells(grandTotalRow, amountColumnIndex + 1) = grandTotal


            Marshal.ReleaseComObject(workSheet)
            Marshal.ReleaseComObject(workBook)
            Marshal.ReleaseComObject(excelApp)

        Catch ex As Exception
            MessageBox.Show("An error occurred while exporting to Excel: " & ex.Message)
        End Try
    End Sub


    Private Sub IconButton2_Click(sender As Object, e As EventArgs) Handles IconButton2.Click
        ExportToExcel()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs)

    End Sub
    Private Sub printDoc_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles printDoc1.PrintPage
        ' Set print font and line height
        Dim printFont As New Font("Arial", 8)  ' Adjust font size here
        Dim lineHeight As Integer = 15  ' Line height (adjust for better spacing)
        Dim defaultColumnWidth As Integer = 100  ' Default column width

        ' Get the starting position for printing
        Dim yPos As Integer = e.MarginBounds.Top
        Dim xPos As Integer = e.MarginBounds.Left
        Dim columnCount As Integer = DataGridView1.Columns.Count

        ' Specify custom widths for certain columns (for example, "Employee" column)
        Dim columnWidths As New Dictionary(Of String, Integer) From {
        {"Item", 95},
        {"Employee", 93},  ' Example: Employee column is wider
        {"Date", 85}       ' Example: Date column is wider
    }

        If printMode = "single" Then
            ' Print the selected row
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Print the column headers
            For i As Integer = 0 To columnCount - 1
                Dim colName As String = DataGridView1.Columns(i).HeaderText
                Dim columnWidth As Integer

                ' Use custom column width if available, otherwise use default
                If columnWidths.ContainsKey(colName) Then
                    columnWidth = columnWidths(colName)
                Else
                    columnWidth = defaultColumnWidth
                End If

                e.Graphics.DrawString(colName, printFont, Brushes.Black, xPos + i * columnWidth, yPos)
            Next

            yPos += lineHeight  ' Move position after header row

            ' Print the cells in the selected row
            For i As Integer = 0 To columnCount - 1
                Dim colName As String = DataGridView1.Columns(i).HeaderText
                Dim columnWidth As Integer

                ' Use custom column width if available, otherwise use default
                If columnWidths.ContainsKey(colName) Then
                    columnWidth = columnWidths(colName)
                Else
                    columnWidth = defaultColumnWidth
                End If

                e.Graphics.DrawString(selectedRow.Cells(i).Value.ToString(), printFont, Brushes.Black, xPos + i * columnWidth, yPos)
            Next

        ElseIf printMode = "all" Then
            ' Print all rows in the DataGridView
            ' Print the column headers
            For i As Integer = 0 To columnCount - 1
                Dim colName As String = DataGridView1.Columns(i).HeaderText
                Dim columnWidth As Integer

                ' Use custom column width if available, otherwise use default
                If columnWidths.ContainsKey(colName) Then
                    columnWidth = columnWidths(colName)
                Else
                    columnWidth = defaultColumnWidth
                End If

                e.Graphics.DrawString(colName, printFont, Brushes.Black, xPos + i * columnWidth, yPos)
            Next

            yPos += lineHeight  ' Move position after header row

            ' Print each row in the DataGridView
            For Each row As DataGridViewRow In DataGridView1.Rows
                ' Skip the last empty row (if any)
                If row.IsNewRow Then Continue For

                ' Print each cell in the row
                For i As Integer = 0 To columnCount - 1
                    Dim colName As String = DataGridView1.Columns(i).HeaderText
                    Dim columnWidth As Integer

                    ' Use custom column width if available, otherwise use default
                    If columnWidths.ContainsKey(colName) Then
                        columnWidth = columnWidths(colName)
                    Else
                        columnWidth = defaultColumnWidth
                    End If

                    e.Graphics.DrawString(row.Cells(i).Value.ToString(), printFont, Brushes.Black, xPos + i * columnWidth, yPos)
                Next

                yPos += lineHeight  ' Move to the next line after each row

                ' Check if we need to continue to the next page
                If yPos > e.MarginBounds.Bottom Then
                    e.HasMorePages = True
                    Return
                End If
            Next
        End If

        ' After all rows are printed, set HasMorePages to False
        e.HasMorePages = False
    End Sub


    Private Sub IconButton4_Click(sender As Object, e As EventArgs) Handles IconButton4.Click
        printMode = "all"

        ' Show the PrintDialog
        printDlg.Document = printDoc1
        If printDlg.ShowDialog() = DialogResult.OK Then
            ' Start printing all rows in the DataGridView
            printDoc1.Print()
        End If
    End Sub
    Private Sub ulit()
        Try
            Dim connectionString As String = "server=localhost;username=root;password=;database=bayabas_co"

            Using con As New MySqlConnection(connectionString)
                con.Open()

                Dim query As String = "SELECT Purchase_date, Barcode, Product_Name, Total, Quantity, Purchase_number, Manual_Discount, Employee FROM ONtbl"

                Using cmd As New MySqlCommand(query, con)
                    Using dr As MySqlDataReader = cmd.ExecuteReader()

                        DataGridView1.Rows.Clear()
                        DataGridView1.Columns.Clear() ' Clear previous columns if any

                        ' ✅ Add columns manually
                        DataGridView1.Columns.Add("Purchase_number", "Purchase Number")
                        DataGridView1.Columns.Add("Barcode", "Barcode")
                        DataGridView1.Columns.Add("Product_Name", "Product Name")
                        DataGridView1.Columns.Add("Total", "Total")
                        DataGridView1.Columns.Add("Quantity", "Quantity")
                        DataGridView1.Columns.Add("Manual_Discount", "Discount")
                        DataGridView1.Columns.Add("Purchase_date", "Purchase Date")
                        DataGridView1.Columns.Add("Employee", "Employee")

                        Dim grandTotal As Decimal = 0
                        Dim todayTotal As Decimal = 0
                        Dim todayDate As String = Date.Now.ToString("yyyy-MM-dd")
                        If dr.HasRows Then
                            While dr.Read()
                                Dim rowTotal As Decimal = Convert.ToDecimal(dr("Total"))
                                grandTotal += rowTotal
                                Dim purchaseDate As String = Convert.ToDateTime(dr("Purchase_date")).ToString("yyyy-MM-dd")
                                If purchaseDate = todayDate Then
                                    todayTotal += rowTotal
                                End If
                                DataGridView1.Rows.Add(
                                    dr("Purchase_number").ToString(),
                                    dr("Barcode").ToString(),
                                    dr("Product_Name").ToString(),
                                    rowTotal.ToString("N2"),
                                    dr("Quantity").ToString(),
                                    dr("Manual_Discount").ToString(),
                                    Convert.ToDateTime(dr("Purchase_date")).ToString("yyyy-MM-dd HH:mm:ss"),
                                    dr("Employee").ToString()
                                )
                            End While

                            Label6.Text = "Grand Total: " & grandTotal.ToString("C2")
                            Label3.Text = "Today's Sales: " & todayTotal.ToString("C2")

                            For Each column As DataGridViewColumn In DataGridView1.Columns
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                            Next
                            ' ✅ Apply formatting AFTER adding rows
                            DataGridView1.Columns("Total").DefaultCellStyle.Format = "N2"
                            DataGridView1.Columns("Purchase_date").DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss"


                        Else

                        End If
                    End Using
                End Using
            End Using

        Catch ex As MySqlException
            MsgBox("Database error occurred: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MsgBox("An error occurred while loading records: " & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub IconButton15_Click(sender As Object, e As EventArgs) Handles IconButton15.Click
        ulit()
    End Sub
End Class