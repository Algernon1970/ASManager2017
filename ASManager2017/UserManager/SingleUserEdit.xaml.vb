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
        ElseIf caller.Equals("mapdrive") Then
            callmapdrive()
        ElseIf caller.Equals("resetprofile") Then
            resetprofile()
        ElseIf caller.Equals("openhomedrive") Then
            openHomeDrive()
        Else
            passwdctl.Visibility = Visibility.Hidden
            MsgBox("Unknown function " & caller)
        End If
    End Sub

    Private Sub openHomeDrive()
        Process.Start("explorer.exe", userDetailsPane.HomeDirectory.Text)
    End Sub
    Private Sub resetprofile()
        Dim profileExt As String = userDetailsPane.profileVersionComboBox.SelectedValue
        Dim profileLocation As String = userDetailsPane.ProfilePath.Text
        Dim newPath As String
        Try
            If profileExt.EndsWith("old") Then
                newPath = profileLocation & profileExt.Replace("old", "")
                If IO.Directory.Exists(newPath) Then
                    Utils.deleteDir(newPath)
                End If
                IO.Directory.Move(newPath & "old", newPath)
                MsgBox(String.Format("Move {0} to {1}", newPath & "old", newPath))
            Else
                newPath = profileLocation & profileExt
                If IO.Directory.Exists(newPath & "old") Then
                    Utils.deleteDir(newPath & "old")
                End If
                IO.Directory.Move(newPath, newPath & "old")
                MsgBox(String.Format("Move {0} to {1}", newPath, newPath & "old"))
            End If
        Catch ex As Exception
            MsgBox("Failed to reset profile - " & ex.Message)
        End Try

        userDetailsPane.profileVersionComboBox.Items.Clear()
    End Sub

    Private Sub callmapdrive()
        Dim uName As String = user.SamAccountName
        Dim dLetter As String = userDetailsPane.DriveLettercomboBox.SelectedValue.ToString.Substring(0, 2)
        Try
            Mapper.unmapdrive(dLetter)
        Catch ex As Exception

        End Try
        Try
            Dim url As String = String.Format("https://ashbyschool-my.sharepoint.com/personal/{0}_ashbyschool_org_uk/", uName)
            If (mapdrive(dLetter, url)) Then
                Process.Start("explorer.exe", String.Format("{0}\documents", dLetter))
            End If
        Catch ex As Exception
            MsgBox(String.Format("Unable to map {1} ({0})", ex.Message, dLetter))
        End Try
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

    Private Sub DeleteGrpsButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteGrpsButton.Click
        MsgBox("Deleting " & groupListBox.SelectedItems.Count & " Items")
        For Each grpname In groupListBox.SelectedItems
            Dim res As String = removeUserFromGroup(tlGroupsCTX, user.SamAccountName, grpname)
            If Not res = "ok" Then
                MsgBox(String.Format("Failed to remove from Group {1} - ({0})", res, grpname))
            End If
        Next
        loadUserDetails(user.SamAccountName)
    End Sub

End Class
