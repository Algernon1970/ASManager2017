Class MainWindow
    Private Sub UserLister_Loaded(sender As Object, e As RoutedEventArgs) Handles UserLister.Loaded
        UserLister.loadUsers()
    End Sub

    Private Sub UserLister_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles UserLister.MouseDoubleClick
        Dim name As String = UserLister.myTreeView.SelectedItem.header
        Dim uEdit As New SingleUserEdit
        uEdit.loadUser(name)
        uEdit.ShowDialog()
    End Sub
End Class
