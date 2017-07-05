Imports AshbyTools
Imports System.DirectoryServices.AccountManagement
Imports System.ComponentModel

Public Class UserList
    Dim Lusers As List(Of UserPrincipalex)
    Dim _domainString As String
    Dim _ouString As String
    Dim _treeviewNodes As New TreeViewItem()
    Dim loadedFlag As Boolean = False


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

    Public Sub loadUsers()
        If loadedFlag Then Return
        loadedFlag = True
        Dim treeHead As New TreeViewItem()
        treeHead.Header = "AD Users"
        Dim userCTX As PrincipalContext = ADTools.getConnection(domainString, ouString)
        Dim lUsers As List(Of UserPrincipalex) = getAllUsers(userCTX)
        Dim treePath As String()
        For Each user As UserPrincipalex In lUsers
            treePath = DN2Tree(user.DistinguishedName)
            insertToTree(treePath, treeHead)
        Next
        myTreeView.Items.Add(treeHead)
    End Sub

    Private Sub insertToTree(ByVal path As String(), ByRef treeHead As TreeViewItem)
        Dim treeNode As TreeViewItem = treeHead
        For Each element As String In path
            treeNode = getTreeViewItem(treeNode, element)
        Next
    End Sub

    Private Function getTreeViewItem(ByRef treeNode As TreeViewItem, ByVal element As String) As TreeViewItem
        For Each node As TreeViewItem In treeNode.Items
            If node.Header.Equals(element) Then
                Return node
            End If
        Next
        Dim newTreeViewItem As New TreeViewItem()
        newTreeViewItem.Header = element
        treeNode.Items.Add(newTreeViewItem)
        Return newTreeViewItem
    End Function

    Private Function DN2Tree(ByVal DNPath As String) As String()
        Dim parts As String() = DNPath.Split(",")
        Dim strap As String() = arrayReverse(parts)
        Dim ret(strap.Count - 1) As String
        For i As Integer = 0 To strap.Count - 1
            ret(i) = strap(i).Substring(3)
        Next
        Return ret
    End Function

    Private Sub reloadButton_Click(sender As Object, e As RoutedEventArgs) Handles reloadButton.Click
        loadedFlag = False
        myTreeView.Items.Clear()
        loadUsers()
    End Sub
End Class
