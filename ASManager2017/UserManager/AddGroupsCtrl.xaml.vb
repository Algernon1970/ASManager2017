Imports AshbyTools
Imports System.DirectoryServices.AccountManagement
Imports System.ComponentModel

Public Class AddGroupsCtrl
    Dim _domainString As String
    Dim _ouString As String

    Public Event addClicked(ByVal glist As List(Of String))

    <Description("ouString"), DisplayName("OU String"), Category("Data")>
    Public Property ouString As String
        Get
            Return _ouString
        End Get
        Set(value As String)
            _ouString = value
        End Set
    End Property

    <Description("domainString"), DisplayName("Domain String"), Category("Data")>
    Public Property domainString As String
        Get
            Return _domainString
        End Get
        Set(value As String)
            _domainString = value
        End Set
    End Property
    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        Me.Visibility = Visibility.Hidden
    End Sub

    Public Sub loadGroups()
        grouplistBox.Items.Clear()
        Dim groupCTX As PrincipalContext = ADTools.getConnection(domainString, ouString)
        Dim gList As List(Of GroupPrincipal) = ADTools.getManagedGroups(groupCTX)
        Dim gsList As New List(Of String)
        For Each grp As GroupPrincipal In gList
            If Not (grp.DistinguishedName.Contains("Staff Groups") Or grp.DistinguishedName.Contains("Subject Groups") Or grp.DistinguishedName.Contains("Tutor Groups")) Then
                gsList.Add(grp.Name)
            End If
        Next

        gsList.Sort()

        For Each grpn As String In gsList
            grouplistBox.Items.Add(grpn)
        Next
    End Sub

    Private Sub addButton_Click(sender As Object, e As RoutedEventArgs) Handles addButton.Click
        Dim ret As New List(Of String)
        For Each grp In grouplistBox.SelectedItems
            ret.Add(grp.ToString)
        Next
        RaiseEvent addClicked(ret)
    End Sub
End Class
