Imports AshbyTools
Public Class userDetails
    Public watch As Boolean = False
    Public Event buttonClicked(ByVal caller As String)

    Private Sub detailsChanged(sender As Object, e As TextChangedEventArgs)
        If watch Then
            SaveButton.IsEnabled = True
        End If
    End Sub

    Private Sub Grid_Loaded(sender As Object, e As RoutedEventArgs)
        watch = True
    End Sub

    Private Sub ChangePwdButton_Click(sender As Object, e As RoutedEventArgs) Handles ChangePwdButton.Click
        RaiseEvent buttonClicked("passwd")
    End Sub

    Private Sub OpenDocs_Click(sender As Object, e As RoutedEventArgs) Handles OpenDocs.Click
        RaiseEvent buttonClicked("open")
    End Sub

    Private Sub ResetProfile_Click(sender As Object, e As RoutedEventArgs) Handles ResetProfile.Click
        RaiseEvent buttonClicked("reset")
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        RaiseEvent buttonClicked("save")
    End Sub

    Private Sub mapOneDriveButton_Click(sender As Object, e As RoutedEventArgs) Handles mapOneDriveButton.Click
        RaiseEvent buttonClicked("mapdrive")
    End Sub

    Private Sub DriveLettercomboBox_DropDownOpened(sender As Object, e As EventArgs) Handles DriveLettercomboBox.DropDownOpened
        Dim driveLetters As List(Of String) = FileOperations.getAvailableDriveLetters()
        DriveLettercomboBox.ItemsSource = driveLetters
    End Sub
End Class
