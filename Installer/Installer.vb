Imports IWshRuntimeLibrary
Public Class Installer
    Dim pickedlocation As String
    Dim installlocation As String
    Dim client As New Net.WebClient

    Private Sub Installer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub
    Public Sub CreateShortCut()
        Try
            Dim WshShell As New WshShell
            Dim MyShortcut As IWshRuntimeLibrary.IWshShortcut
            MyShortcut = CType(WshShell.CreateShortcut(My.Computer.FileSystem.SpecialDirectories.Desktop & "\Sourcemod Launcher.lnk"), IWshRuntimeLibrary.IWshShortcut)
            MyShortcut.TargetPath = installlocation & "\Sourcemod Launcher.exe"
            MyShortcut.Save()
        Catch ex As System.Exception
            MsgBox("Could not create the shortcut")
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
browser:
        Dim result As DialogResult = FolderBrowserDialog1.ShowDialog()
        If result = Windows.Forms.DialogResult.OK And My.Computer.FileSystem.DirectoryExists(FolderBrowserDialog1.SelectedPath) Then
            Dim path As String = FolderBrowserDialog1.SelectedPath
            pickedlocation = FolderBrowserDialog1.SelectedPath
        Else
            GoTo browser
        End If
        If My.Computer.FileSystem.DirectoryExists(pickedlocation & "\sourcemodlauncher") Then
            My.Computer.FileSystem.DeleteDirectory(pickedlocation & "\sourcemodlauncher", FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(pickedlocation & "\sourcemodlauncher")
        Else
            My.Computer.FileSystem.CreateDirectory(pickedlocation & "\sourcemodlauncher")
        End If
        installlocation = pickedlocation & "\sourcemodlauncher"
        client.DownloadFile("https://github.com/DorukSega/launcherdatabed/raw/master/Sourcemod%20Launcher.exe", installlocation & "\Sourcemod Launcher.exe")
        If CheckBox1.Checked Then
            Call CreateShortCut()
        Else
        End If

    End Sub
End Class
