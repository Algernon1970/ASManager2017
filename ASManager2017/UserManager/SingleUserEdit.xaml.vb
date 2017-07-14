Imports System.DirectoryServices.AccountManagement
Imports AshbyTools

Public Class SingleUserEdit
    Dim user As UserPrincipalex

    Public Sub loadUser(ByVal username As String)
        AddHandler userDetailsPane.buttonClicked, AddressOf childButtonPressed
        AddHandler passwdctl.buttonClicked, AddressOf childButtonPressed
        AddHandler addgrps.addClicked, AddressOf addGroupsEvent
        userNameBox.Text = username
        Try
            loadUserDetails(username)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub loadUserDetails(ByVal username As String)
        user = ADTools.getUserPrincipalexbyUsername(userCTX, username)
        Dim uGrps As List(Of String) = Nothing
        Try
            uGrps = ADTools.getGroupsByUser(user.EmployeeId)
        Catch ex As Exception

        End Try

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
        If Not IsNothing(uGrps) Then
            For Each grp As String In uGrps
                groupListBox.Items.Add(grp)
            Next
        End If

    End Sub

    Private Sub addGroupsButton_Click(sender As Object, e As RoutedEventArgs) Handles addGroupsButton.Click
        addgrps.loadGroups()
        addgrps.Visibility = Visibility.Visible
    End Sub

    Private Sub childButtonPressed(ByVal caller As String)
        If caller.Equals("passwd") Then
            passwdctl.Visibility = Visibility.Visible
        ElseIf caller.Equals("passwdcancel") Then
            passwdctl.Visibility = Visibility.Hidden
        ElseIf caller.Equals("passwdsave") Then
            changePassword()

        Else
            passwdctl.Visibility = Visibility.Hidden
            MsgBox(caller)
        End If
    End Sub

    Private Sub changePassword()
        Try
            user.SetPassword(passwdctl.new1PasswordBox.Password)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        passwdctl.Visibility = Visibility.Hidden
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
