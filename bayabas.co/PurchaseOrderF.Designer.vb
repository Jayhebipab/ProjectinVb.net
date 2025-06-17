<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PurchaseOrderF
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.DTP1 = New System.Windows.Forms.DateTimePicker()
        Me.qb = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.db = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.tab = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cpb = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.canb = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.snb = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cnb = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.IconButton1 = New FontAwesome.Sharp.IconButton()
        Me.IconButton15 = New FontAwesome.Sharp.IconButton()
        Me.IconButton7 = New FontAwesome.Sharp.IconButton()
        Me.IconButton21 = New FontAwesome.Sharp.IconButton()
        Me.IconButton5 = New FontAwesome.Sharp.IconButton()
        Me.IconButton6 = New FontAwesome.Sharp.IconButton()
        Me.IconButton3 = New FontAwesome.Sharp.IconButton()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(109, Byte), Integer), CType(CType(115, Byte), Integer), CType(CType(97, Byte), Integer))
        Me.Panel1.Controls.Add(Me.DTP1)
        Me.Panel1.Controls.Add(Me.qb)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.db)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.tab)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.cpb)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.canb)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.snb)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cnb)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Label21)
        Me.Panel1.Location = New System.Drawing.Point(21, 15)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1505, 715)
        Me.Panel1.TabIndex = 1
        '
        'DTP1
        '
        Me.DTP1.CalendarMonthBackground = System.Drawing.Color.WhiteSmoke
        Me.DTP1.CustomFormat = "yyyy-MM-dd"
        Me.DTP1.Font = New System.Drawing.Font("Yu Gothic UI", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DTP1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DTP1.Location = New System.Drawing.Point(264, 360)
        Me.DTP1.Margin = New System.Windows.Forms.Padding(4)
        Me.DTP1.Name = "DTP1"
        Me.DTP1.Size = New System.Drawing.Size(218, 38)
        Me.DTP1.TabIndex = 190
        '
        'qb
        '
        Me.qb.BackColor = System.Drawing.Color.WhiteSmoke
        Me.qb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.qb.Font = New System.Drawing.Font("Yu Gothic UI", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.qb.Location = New System.Drawing.Point(264, 417)
        Me.qb.Margin = New System.Windows.Forms.Padding(4)
        Me.qb.Name = "qb"
        Me.qb.Size = New System.Drawing.Size(218, 38)
        Me.qb.TabIndex = 189
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Cooper Black", 13.8!)
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(54, 422)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(123, 27)
        Me.Label7.TabIndex = 188
        Me.Label7.Text = "Quantity"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Cooper Black", 13.8!)
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(55, 370)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 27)
        Me.Label10.TabIndex = 186
        Me.Label10.Text = "Date"
        '
        'db
        '
        Me.db.BackColor = System.Drawing.Color.WhiteSmoke
        Me.db.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.db.Font = New System.Drawing.Font("Yu Gothic UI", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.db.Location = New System.Drawing.Point(264, 300)
        Me.db.Margin = New System.Windows.Forms.Padding(4)
        Me.db.Name = "db"
        Me.db.Size = New System.Drawing.Size(218, 38)
        Me.db.TabIndex = 185
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Cooper Black", 13.8!)
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(54, 305)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(156, 27)
        Me.Label11.TabIndex = 184
        Me.Label11.Text = "Description"
        '
        'tab
        '
        Me.tab.BackColor = System.Drawing.Color.WhiteSmoke
        Me.tab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tab.Font = New System.Drawing.Font("Yu Gothic UI", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tab.Location = New System.Drawing.Point(264, 532)
        Me.tab.Margin = New System.Windows.Forms.Padding(4)
        Me.tab.Name = "tab"
        Me.tab.Size = New System.Drawing.Size(218, 38)
        Me.tab.TabIndex = 181
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Cooper Black", 13.8!)
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(55, 537)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(183, 27)
        Me.Label5.TabIndex = 180
        Me.Label5.Text = "Total Amount"
        '
        'cpb
        '
        Me.cpb.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cpb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.cpb.Font = New System.Drawing.Font("Yu Gothic UI", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cpb.Location = New System.Drawing.Point(264, 473)
        Me.cpb.Margin = New System.Windows.Forms.Padding(4)
        Me.cpb.Name = "cpb"
        Me.cpb.Size = New System.Drawing.Size(218, 38)
        Me.cpb.TabIndex = 179
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Cooper Black", 13.8!)
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(55, 478)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(135, 27)
        Me.Label6.TabIndex = 178
        Me.Label6.Text = "Cost Price"
        '
        'canb
        '
        Me.canb.BackColor = System.Drawing.Color.WhiteSmoke
        Me.canb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.canb.Font = New System.Drawing.Font("Yu Gothic UI", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.canb.Location = New System.Drawing.Point(264, 240)
        Me.canb.Margin = New System.Windows.Forms.Padding(4)
        Me.canb.Name = "canb"
        Me.canb.Size = New System.Drawing.Size(218, 38)
        Me.canb.TabIndex = 177
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Cooper Black", 13.8!)
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(54, 244)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(199, 27)
        Me.Label3.TabIndex = 176
        Me.Label3.Text = "Category Name"
        '
        'snb
        '
        Me.snb.BackColor = System.Drawing.Color.WhiteSmoke
        Me.snb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.snb.Font = New System.Drawing.Font("Yu Gothic UI", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.snb.Location = New System.Drawing.Point(264, 184)
        Me.snb.Margin = New System.Windows.Forms.Padding(4)
        Me.snb.Name = "snb"
        Me.snb.Size = New System.Drawing.Size(218, 38)
        Me.snb.TabIndex = 175
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Cooper Black", 13.8!)
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(54, 188)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(195, 27)
        Me.Label2.TabIndex = 174
        Me.Label2.Text = "Supplier Name"
        '
        'cnb
        '
        Me.cnb.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cnb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.cnb.Font = New System.Drawing.Font("Yu Gothic UI", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cnb.Location = New System.Drawing.Point(264, 129)
        Me.cnb.Margin = New System.Windows.Forms.Padding(4)
        Me.cnb.Name = "cnb"
        Me.cnb.Size = New System.Drawing.Size(218, 38)
        Me.cnb.TabIndex = 173
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Cooper Black", 13.8!)
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(54, 134)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(203, 27)
        Me.Label1.TabIndex = 172
        Me.Label1.Text = "Company Name"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.IconButton1)
        Me.Panel2.Controls.Add(Me.DataGridView1)
        Me.Panel2.Controls.Add(Me.IconButton15)
        Me.Panel2.Controls.Add(Me.IconButton7)
        Me.Panel2.Controls.Add(Me.IconButton21)
        Me.Panel2.Controls.Add(Me.IconButton5)
        Me.Panel2.Controls.Add(Me.IconButton6)
        Me.Panel2.Controls.Add(Me.IconButton3)
        Me.Panel2.Location = New System.Drawing.Point(524, 90)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(951, 554)
        Me.Panel2.TabIndex = 171
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(182, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(144, Byte), Integer))
        Me.DataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.DataGridView1.ColumnHeadersHeight = 40
        Me.DataGridView1.Location = New System.Drawing.Point(27, 63)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(4)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.RowTemplate.Height = 35
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(894, 455)
        Me.DataGridView1.TabIndex = 78
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Cooper Black", 18.8!)
        Me.Label21.ForeColor = System.Drawing.Color.Black
        Me.Label21.Location = New System.Drawing.Point(21, 15)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(272, 36)
        Me.Label21.TabIndex = 1
        Me.Label21.Text = "Purchace Order"
        '
        'IconButton1
        '
        Me.IconButton1.BackColor = System.Drawing.Color.Transparent
        Me.IconButton1.FlatAppearance.BorderSize = 0
        Me.IconButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IconButton1.Font = New System.Drawing.Font("Cooper Black", 11.8!)
        Me.IconButton1.ForeColor = System.Drawing.Color.Black
        Me.IconButton1.IconChar = FontAwesome.Sharp.IconChar.Download
        Me.IconButton1.IconColor = System.Drawing.Color.Black
        Me.IconButton1.IconFont = FontAwesome.Sharp.IconFont.[Auto]
        Me.IconButton1.IconSize = 28
        Me.IconButton1.Location = New System.Drawing.Point(74, 11)
        Me.IconButton1.Margin = New System.Windows.Forms.Padding(4)
        Me.IconButton1.Name = "IconButton1"
        Me.IconButton1.Size = New System.Drawing.Size(102, 47)
        Me.IconButton1.TabIndex = 172
        Me.IconButton1.Text = "Print"
        Me.IconButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.IconButton1.UseVisualStyleBackColor = False
        '
        'IconButton15
        '
        Me.IconButton15.BackColor = System.Drawing.Color.Transparent
        Me.IconButton15.FlatAppearance.BorderSize = 0
        Me.IconButton15.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IconButton15.Font = New System.Drawing.Font("Cooper Black", 11.8!)
        Me.IconButton15.ForeColor = System.Drawing.Color.Black
        Me.IconButton15.IconChar = FontAwesome.Sharp.IconChar.History
        Me.IconButton15.IconColor = System.Drawing.Color.Black
        Me.IconButton15.IconFont = FontAwesome.Sharp.IconFont.[Auto]
        Me.IconButton15.IconSize = 28
        Me.IconButton15.Location = New System.Drawing.Point(13, 14)
        Me.IconButton15.Margin = New System.Windows.Forms.Padding(4)
        Me.IconButton15.Name = "IconButton15"
        Me.IconButton15.Size = New System.Drawing.Size(53, 38)
        Me.IconButton15.TabIndex = 123
        Me.IconButton15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.IconButton15.UseVisualStyleBackColor = False
        '
        'IconButton7
        '
        Me.IconButton7.BackColor = System.Drawing.Color.Transparent
        Me.IconButton7.FlatAppearance.BorderSize = 0
        Me.IconButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IconButton7.Font = New System.Drawing.Font("Cooper Black", 11.8!)
        Me.IconButton7.ForeColor = System.Drawing.Color.Black
        Me.IconButton7.IconChar = FontAwesome.Sharp.IconChar.Edit
        Me.IconButton7.IconColor = System.Drawing.Color.Black
        Me.IconButton7.IconFont = FontAwesome.Sharp.IconFont.[Auto]
        Me.IconButton7.IconSize = 28
        Me.IconButton7.Location = New System.Drawing.Point(193, 9)
        Me.IconButton7.Margin = New System.Windows.Forms.Padding(4)
        Me.IconButton7.Name = "IconButton7"
        Me.IconButton7.Size = New System.Drawing.Size(150, 52)
        Me.IconButton7.TabIndex = 81
        Me.IconButton7.Text = "Add Item"
        Me.IconButton7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.IconButton7.UseVisualStyleBackColor = False
        '
        'IconButton21
        '
        Me.IconButton21.BackColor = System.Drawing.Color.Transparent
        Me.IconButton21.FlatAppearance.BorderSize = 0
        Me.IconButton21.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IconButton21.Font = New System.Drawing.Font("Cooper Black", 11.8!)
        Me.IconButton21.ForeColor = System.Drawing.Color.Black
        Me.IconButton21.IconChar = FontAwesome.Sharp.IconChar.FileCircleXmark
        Me.IconButton21.IconColor = System.Drawing.Color.Black
        Me.IconButton21.IconFont = FontAwesome.Sharp.IconFont.[Auto]
        Me.IconButton21.IconSize = 28
        Me.IconButton21.Location = New System.Drawing.Point(709, 13)
        Me.IconButton21.Margin = New System.Windows.Forms.Padding(4)
        Me.IconButton21.Name = "IconButton21"
        Me.IconButton21.Size = New System.Drawing.Size(115, 47)
        Me.IconButton21.TabIndex = 150
        Me.IconButton21.Text = "Clear"
        Me.IconButton21.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.IconButton21.UseVisualStyleBackColor = False
        '
        'IconButton5
        '
        Me.IconButton5.BackColor = System.Drawing.Color.Transparent
        Me.IconButton5.FlatAppearance.BorderSize = 0
        Me.IconButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IconButton5.Font = New System.Drawing.Font("Cooper Black", 11.8!)
        Me.IconButton5.ForeColor = System.Drawing.Color.Black
        Me.IconButton5.IconChar = FontAwesome.Sharp.IconChar.Download
        Me.IconButton5.IconColor = System.Drawing.Color.Black
        Me.IconButton5.IconFont = FontAwesome.Sharp.IconFont.[Auto]
        Me.IconButton5.IconSize = 28
        Me.IconButton5.Location = New System.Drawing.Point(351, 11)
        Me.IconButton5.Margin = New System.Windows.Forms.Padding(4)
        Me.IconButton5.Name = "IconButton5"
        Me.IconButton5.Size = New System.Drawing.Size(181, 47)
        Me.IconButton5.TabIndex = 79
        Me.IconButton5.Text = " Update Item"
        Me.IconButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.IconButton5.UseVisualStyleBackColor = False
        '
        'IconButton6
        '
        Me.IconButton6.BackColor = System.Drawing.Color.Transparent
        Me.IconButton6.FlatAppearance.BorderSize = 0
        Me.IconButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IconButton6.Font = New System.Drawing.Font("Cooper Black", 11.8!)
        Me.IconButton6.ForeColor = System.Drawing.Color.Black
        Me.IconButton6.IconChar = FontAwesome.Sharp.IconChar.FileCircleXmark
        Me.IconButton6.IconColor = System.Drawing.Color.Black
        Me.IconButton6.IconFont = FontAwesome.Sharp.IconFont.[Auto]
        Me.IconButton6.IconSize = 28
        Me.IconButton6.Location = New System.Drawing.Point(540, 12)
        Me.IconButton6.Margin = New System.Windows.Forms.Padding(4)
        Me.IconButton6.Name = "IconButton6"
        Me.IconButton6.Size = New System.Drawing.Size(170, 47)
        Me.IconButton6.TabIndex = 80
        Me.IconButton6.Text = "Delete Item"
        Me.IconButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.IconButton6.UseVisualStyleBackColor = False
        '
        'IconButton3
        '
        Me.IconButton3.BackColor = System.Drawing.Color.Transparent
        Me.IconButton3.FlatAppearance.BorderSize = 0
        Me.IconButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IconButton3.Font = New System.Drawing.Font("Cooper Black", 11.8!)
        Me.IconButton3.ForeColor = System.Drawing.Color.Black
        Me.IconButton3.IconChar = FontAwesome.Sharp.IconChar.ReplyAll
        Me.IconButton3.IconColor = System.Drawing.Color.Black
        Me.IconButton3.IconFont = FontAwesome.Sharp.IconFont.[Auto]
        Me.IconButton3.IconSize = 28
        Me.IconButton3.Location = New System.Drawing.Point(821, 19)
        Me.IconButton3.Margin = New System.Windows.Forms.Padding(4)
        Me.IconButton3.Name = "IconButton3"
        Me.IconButton3.Size = New System.Drawing.Size(100, 39)
        Me.IconButton3.TabIndex = 103
        Me.IconButton3.Text = "Exit"
        Me.IconButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.IconButton3.UseVisualStyleBackColor = False
        '
        'PurchaseOrderF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(182, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(144, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1556, 762)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "PurchaseOrderF"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PurchaseOrderF"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents IconButton21 As FontAwesome.Sharp.IconButton
    Friend WithEvents IconButton15 As FontAwesome.Sharp.IconButton
    Friend WithEvents IconButton7 As FontAwesome.Sharp.IconButton
    Friend WithEvents IconButton6 As FontAwesome.Sharp.IconButton
    Friend WithEvents IconButton5 As FontAwesome.Sharp.IconButton
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents IconButton3 As FontAwesome.Sharp.IconButton
    Friend WithEvents Label21 As Label
    Friend WithEvents IconButton1 As FontAwesome.Sharp.IconButton
    Friend WithEvents qb As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents db As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents tab As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents cpb As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents canb As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents snb As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cnb As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents DTP1 As DateTimePicker
End Class
