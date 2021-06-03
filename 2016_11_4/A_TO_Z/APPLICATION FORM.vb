Public Class APPLICATION_FORM

    Private Sub OPR0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OPR0.Click

    End Sub

    Private Sub UltraLabel22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraLabel22.Click

    End Sub

    Private Sub UltraLabel16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub UltraLabel18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub UltraLabel21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Dim A As String
        A = MsgBox("Are you sure you want to exit this", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Exit .......")
        If A = vbYes Then
            Me.Close()
        End If
    End Sub

    Private Sub txtDate1_BeforeDropDown(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtDate1.BeforeDropDown

    End Sub

    Private Sub UltraCombo4_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs)

    End Sub

    Private Sub APPLICATION_FORM_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    
    Private Sub UltraLabel25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub UltraCalendarCombo1_BeforeDropDown(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles UltraCalendarCombo1.BeforeDropDown

    End Sub

    Private Sub UltraLabel3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraLabel3.Click

    End Sub
End Class