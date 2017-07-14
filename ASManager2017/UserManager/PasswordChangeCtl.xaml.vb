Public Class PasswordChangeCtl

    Public Event buttonClicked(ByVal caller As String)

    Private Sub cancelButton_Click(sender As Object, e As RoutedEventArgs) Handles cancelButton.Click
        RaiseEvent buttonClicked("passwdcancel")
    End Sub

    Private Sub saveButton_Click(sender As Object, e As RoutedEventArgs) Handles saveButton.Click
        If new1PasswordBox.Password.Equals(new2PasswordBox.Password) Then
            RaiseEvent buttonClicked("passwdsave")
        Else
            MsgBox("Passwords do not match.")
        End If


    End Sub
End Class
