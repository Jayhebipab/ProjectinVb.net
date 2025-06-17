Public Class DTF
    Private Sub IconButton5_Click(sender As Object, e As EventArgs) Handles IconButton5.Click
        Dim nextForm As New MainMenuf()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton1_Click(sender As Object, e As EventArgs) Handles IconButton1.Click
        Dim nextForm As New LoginF()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton2_Click(sender As Object, e As EventArgs) Handles IconButton2.Click
        Dim nextForm As New LoginF()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton3_Click(sender As Object, e As EventArgs) Handles IconButton3.Click
        Dim nextForm As New Salesreptbl()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton4_Click(sender As Object, e As EventArgs) Handles IconButton4.Click
        Dim nextForm As New Salesreptbl()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton6_Click(sender As Object, e As EventArgs) Handles IconButton6.Click
        Dim nextForm As New InventoryF()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub IconButton7_Click(sender As Object, e As EventArgs) Handles IconButton7.Click
        Dim nextForm As New AuditF()

        ' Show the MainMenu form
        nextForm.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub DTF_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class