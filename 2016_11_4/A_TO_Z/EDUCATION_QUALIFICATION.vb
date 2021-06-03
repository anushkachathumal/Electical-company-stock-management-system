Public Class EDUCATION_QUALIFICATION

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Dim A As String
        A = MsgBox("Are you sure you want to exit this", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Exit .......")
        If A = vbYes Then
            Me.Close()
        End If
    End Sub
End Class