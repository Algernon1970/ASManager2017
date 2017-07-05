Imports System.DirectoryServices.AccountManagement
Imports AshbyTools

Public Class SingleUserEdit


    Public Sub loadUser(ByVal user As String)
        AddHandler userDetailsPane.buttonClicked, AddressOf childButtonPressed
        AddHandler addgrps.addClicked, AddressOf addGroupsevent
        userNameBox.Text = user
        Try
            loadUserDetails(user)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub loadUserDetails(ByVal username As String)
        Dim user As UserPrincipalex = ADTools.getUserPrincipalexbyUsername(userCTX, username)
        Dim uGrps As List(Of String) = ADTools.getGroupsByUser(user.EmployeeId)
        employeeIdBox.Text = user.EmployeeId
        displayNameBox.Text = user.DisplayName
        userDetailsPane.GivenName.Text = user.GivenName
        userDetailsPane.MiddleName.Text = user.MiddleName
        userDetailsPane.Surname.Text = user.Surname
        userDetailsPane.Description.Text = user.Description
        userDetailsPane.HomeDirectory.Text = user.HomeDirectory
        userDetailsPane.ProfilePath.Text = user.ProfilePath
        userDetailsPane.Pager.Text = user.pager
        groupListBox.Items.Clear()
        For Each grp As String In uGrps
            groupListBox.Items.Add(grp)
        Next
    End Sub

    Private Sub addGroupsButton_Click(sender As Object, e As RoutedEventArgs) Handles addGroupsButton.Click
        addgrps.loadGroups()
        addgrps.Visibility = Visibility.Visible
    End Sub

    Private Sub childButtonPressed(ByVal caller As String)
        MsgBox(caller)
    End Sub

    Private Sub addGroupsEvent(ByVal glist As List(Of String))
        For Each grp As String In glist
            Try
                groupListBox.Items.Add(grp)
                Dim grpContext As GroupPrincipal = ADTools.getGroupPrincipalbyName(tlGroupsCTX, grp)
                grpContext.Members.Add(ADTools.getUserPrincipalByID(userCTX, employeeIdBox.Text))
                grpContext.Save()
            Catch ex As Exception
                MsgBox("Failed adding " & employeeIdBox.Text & " to " & grp & " : " & ex.Message)
            End Try

        Next
    End Sub
End Class
