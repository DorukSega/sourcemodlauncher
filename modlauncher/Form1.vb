Imports System.IO
Imports System.Text
Public Class Form1
    'FIX PARAMETERS
    'ADD INSTALLER
    Dim gamename As String
    Dim gameinfotext As String
    Dim modsfolder As DirectoryInfo
    Dim sourcemodsfolder As String 'sourcemods folder
    Dim client As New Net.WebClient 'WEB
    Dim areweonline As Boolean = False
    Dim installedmods As New List(Of String) 'ALL THE MOD NAMES CURRENTLY INSTALLED
    Dim installedwserverlink_names As New List(Of String) 'ALL THE MOD NAMES THAT HAVE LINKS ONLINE
    Dim installedwserverlink_directory As New List(Of String)
    Dim installedwserverlink_links As New List(Of String) 'LINKS OF ALL THE MODS THAT HAVE LINKS ONLINE
    Dim installedwserverlink_type As New List(Of String) 'TYPES OF ALL THE MODS THAT HAVE LINKS ONLINE
    Dim installedwserverlink_ids As New List(Of String) 'Ids OF ALL THE MODS THAT HAVE LINKS ONLINE
    Dim assingedidmodnames As New List(Of String) 'assigned ids using assign button
    Dim assingedidmodids As New List(Of String)
    Dim modfoundlink_names As New List(Of String) 'ALL THE MOD NAMES THAT WHEN SENT A GIT COMMAND RESPOND WITH LINKS
    Dim modfoundlink_directory As New List(Of String)
    Dim modfoundlink_links As New List(Of String) 'LINKS OF ALL THE MODS THAT WHEN SENT A GIT COMMAND RESPOND WITH LINKS
    Dim steamidknown_name As New List(Of String) 'known steamids online
    Dim steamidknown_id As New List(Of String)
    Dim installable_name As New List(Of String) 'install mods
    Dim installable_desc As New List(Of String)
    Dim installable_link As New List(Of String)
    Dim installable_id As New List(Of String)
    Dim installable_rep As New List(Of String)
    'BAT CODES HERE
    Dim drive As String
    Dim codecd As String = ":
cd "
    'PLAY
    Dim code1 As String = "start """" ""steam://rungameid/"
    Dim code2 As String = "//"
    Dim code3 As String = "/""
md endplay
exit"
    Dim parameters As String = ""
    'GETLINKS 
    Dim getlink1 As String = "
git remote get-url origin > lanlink.txt
md endidget
Exit"
    'RESET
    'git 'svn
    Dim reset1 As String = "
title Reset [DO NOT CLOSE THIS UNTIL IT IS DONE]
cls
@echo off
setlocal EnableDelayedExpansion  
echo Resetting
echo.
color 5f
git reset --hard
md  endreset
Pause"
    Dim reset12 As String = "
title Reset [DO NOT CLOSE THIS UNTIL IT IS DONE]
cls
@echo off
setlocal EnableDelayedExpansion  
echo Resetting
echo.
color 5f
git reset --hard
md  endreset
Exit"
    'INSTALL
    'GIT
    Dim ins1 As String = "
title Installer [DO NOT CLOSE THIS UNTIL IT IS DONE]
cls
@echo off
setlocal EnableDelayedExpansion  
echo Mod Installer
echo.
echo This might take some time, don't run the game or don't close the window while this is happening
echo If you have any issues on installation, make sure to reset or reinstall
echo Installing
echo.
color 5f
git clone --depth 1 --progress """
    Dim ins2 As String = """
md endinstall
echo Restart Steam after the installation is complete
Pause"
    Dim ins22 As String = """
md endinstall
echo Restart Steam after the installation is complete
Pause"
    'SVN
    Dim inssvn1 As String = "
title Installer [DO NOT CLOSE THIS UNTIL IT IS DONE]
cls
@echo off
setlocal EnableDelayedExpansion  
echo Mod Installer
echo.
echo This might take some time, don't run the game or don't close the window while this is happening
echo If you have any issues on installation, make sure to reset or reinstall
echo Installing
echo.
color 5f
git svn clone """
    Dim inssvn2 As String = """
md endinstall
echo Restart Steam after the installation is complete
Pause"
    Dim inssvn22 As String = """
md endinstall
echo Restart Steam after the installation is complete
Exit"
    'UPDATE
    'GIT
    Dim upd1 As String = "
title Updater [DO NOT CLOSE THIS UNTIL IT IS DONE]
cls
@echo off
setlocal EnableDelayedExpansion
color 5f  
echo Mod Updater
echo.
git fetch --depth 1
echo Checking for Updates
FOR /f %%i IN ('git rev-parse origin/master') DO set LatestRevision=%%i
git reset --hard !LatestRevision!
echo This might take some time, don't run the game while this is happening
echo.
echo Updating existing install
git pull --depth 1 --progress --force """
    Dim upd2 As String = """
md endupdate
Pause"
    Dim upd22 As String = """
md endupdate
Exit"
    'SVN
    Dim updsvn1 As String = "
title Updater [DO NOT CLOSE THIS UNTIL IT IS DONE]
cls
@echo off
color 5f 
setlocal EnableDelayedExpansion
echo Mod Updater
echo.
echo Checking for Updates
git svn fetch
echo.
echo This might take some time, don't run the game while this is happening
echo Updating existing install
git reset --hard
git svn rebase """
    Dim updsvn2 = """
md endupdate
Pause"
    Dim updsvn22 = """
md endupdate
Exit"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If client.DownloadString("https://www.google.com/") <> "" Then 'checks internet connection
                areweonline = True
            Else
                areweonline = False
                MsgBox("Internet is not connected.")
            End If
        Catch ex As Exception
            MsgBox("Internet is not connected.")
            areweonline = False
        End Try

        'config name
        If Not My.Computer.FileSystem.DirectoryExists("config") Then
            My.Computer.FileSystem.CreateDirectory("config")
            MsgBox("Install git scm if you didnt already, it is required for app to work: https://git-scm.com/download/win")
        End If
        If My.Computer.FileSystem.FileExists("config\parameterconfig.bin") Then 'loads parameters
            parameters = My.Computer.FileSystem.ReadAllText("config\parameterconfig.bin")
            TextBox3.Text = parameters
        End If

        '[MAIN]Check if sourcemods folder is defined, if not open a openfolder for it else just assign it to a variable and move on
        If Not My.Computer.FileSystem.FileExists("config\launcherconfig.bin") Then
            '[MAIN] Check if there is a sourcemods folder on places like C:\Program Files\Steam\steamapps\sourcemods
            If My.Computer.FileSystem.DirectoryExists("C:\Program Files\Steam\steamapps\sourcemods") Then
                sourcemodsfolder = "C:\Program Files\Steam\steamapps\sourcemods"
                My.Computer.FileSystem.WriteAllText("config\launcherconfig.bin", sourcemodsfolder, True)
                modsfolder = New DirectoryInfo(sourcemodsfolder)
                TextBox1.Text = sourcemodsfolder
            ElseIf My.Computer.FileSystem.DirectoryExists("D:\Program Files\Steam\steamapps\sourcemods") Then
                sourcemodsfolder = "D:\Program Files\Steam\steamapps\sourcemods"
                My.Computer.FileSystem.WriteAllText("config\launcherconfig.bin", sourcemodsfolder, True)
                modsfolder = New DirectoryInfo(sourcemodsfolder)
                TextBox1.Text = sourcemodsfolder
            ElseIf My.Computer.FileSystem.DirectoryExists("C:\Program Files (x86)\Steam\steamapps\sourcemods") Then
                sourcemodsfolder = "C:\Program Files (x86)\Steam\steamapps\sourcemods"
                My.Computer.FileSystem.WriteAllText("config\launcherconfig.bin", sourcemodsfolder, True)
                modsfolder = New DirectoryInfo(sourcemodsfolder)
                TextBox1.Text = sourcemodsfolder
            ElseIf My.Computer.FileSystem.DirectoryExists("D:\Program Files (x86)\Steam\steamapps\sourcemods") Then
                sourcemodsfolder = "D:\Program Files (x86)\Steam\steamapps\sourcemods"
                My.Computer.FileSystem.WriteAllText("config\launcherconfig.bin", sourcemodsfolder, True)
                modsfolder = New DirectoryInfo(sourcemodsfolder)
                TextBox1.Text = sourcemodsfolder
            Else
                'if it is unknown location
browser:
                Dim result As DialogResult = FolderBrowserDialog1.ShowDialog()
                If result = Windows.Forms.DialogResult.OK And My.Computer.FileSystem.DirectoryExists(FolderBrowserDialog1.SelectedPath) Then
                    Dim path As String = FolderBrowserDialog1.SelectedPath
                    sourcemodsfolder = FolderBrowserDialog1.SelectedPath
                    My.Computer.FileSystem.WriteAllText("config\launcherconfig.bin", sourcemodsfolder, True)
                    modsfolder = New DirectoryInfo(sourcemodsfolder)
                    TextBox1.Text = sourcemodsfolder
                Else
                    GoTo browser
                End If
            End If
        ElseIf Not My.Computer.FileSystem.ReadAllText("config\launcherconfig.bin") = Nothing Then
            sourcemodsfolder = My.Computer.FileSystem.ReadAllText("config\launcherconfig.bin")
            modsfolder = New DirectoryInfo(sourcemodsfolder)
            TextBox1.Text = sourcemodsfolder
        Else
            MsgBox("If you are seeing this, something fishy is going on. Make sure to contact Doruk.")
        End If
        '[MAIN] Load all the assigned ids and so on (Later)

        '[MAIN] search inside sourcemods folder, find all the mods, and list them
        Call Listing()
        If areweonline = True Then
            Call installlisting()
        End If
    End Sub
    Public Sub Listing()
        If Not My.Computer.FileSystem.DirectoryExists("config") Then
            My.Computer.FileSystem.CreateDirectory("config")
        End If
        '[MAIN]search inside sourcemods folder for gameinfo files
        drive = sourcemodsfolder.First
        Dim fi As List(Of FileInfo) = New List(Of FileInfo) 'fi is the list of files that are gameinfo.txt
        For Each File In modsfolder.GetFiles("*gameinfo.txt", SearchOption.AllDirectories)
            If (File IsNot Nothing) And Not File.DirectoryName.Contains("scripts") Then
                fi.Add(File)
            End If
        Next
        '[MAIN]open all the gameinfo files and parse them
        ComboBox1.Items.Clear()
        For Each Item In fi
            'info
            Dim gitlocale As String = Item.DirectoryName
            gameinfotext = Item.OpenText.ReadToEnd
            If gameinfotext <> "" Then
                'GAME NAME
                gamename = gameinfotext
                Dim a As Integer = gamename.IndexOf("title")
                If gamename.Contains("game") Then
                    If a >= 0 Then
                        gamename = gamename.Remove(a)
                    End If
                    Dim b As Integer = gamename.IndexOf("gamelogo")
                    If b >= 0 Then
                        gamename = gamename.Remove(b)
                    End If
                    Dim c As Integer = gamename.IndexOf("type")
                    If c >= 0 Then
                        gamename = gamename.Remove(c)
                    End If
                    gamename = gamename.Replace(Chr(9), "")
                    gamename = gamename.Replace("{", "")
                    gamename = gamename.Replace("// Don't edit this file. You WILL break the game.", "")
                    gamename = gamename.Replace("GameInfo", "")
                    gamename = gamename.Replace("game", "")
                    gamename = gamename.Replace("""", "")
                    gamename = gamename.Replace(vbCrLf, "")
                    gamename = gamename.TrimStart(Chr(32))
                    gamename = gamename.TrimStart(Chr(10))
                    gamename = gamename.TrimEnd(Chr(10))
                    gamename = gamename.TrimStart(Chr(13))
                    gamename = gamename.TrimStart(" ")
                    'gamename = gamename.Replace(Chr(10), "")
                    'gamename is ready
                    installedmods.Add(gamename) 'ALL GAMES
                    ComboBox1.Items.Add(gamename)
                End If
            End If
            If areweonline = True Then
                Dim gitlink As String = client.DownloadString("https://raw.githubusercontent.com/DorukSega/launcherdatabed/master/gitlinks.txt")
                Dim gitid As String
                If gitlink.Contains(gamename) Then
                    installedwserverlink_names.Add(gamename)
                    Dim d As Integer = gitlink.LastIndexOf(gamename)
                    If d >= 0 Then
                        gitlink = gitlink.Substring(d)
                    End If
                    Dim e As Integer = gitlink.IndexOf("END")
                    If e >= 0 Then
                        gitlink = gitlink.Remove(e)
                    End If
                    If gitlink.Contains("REPGIT") Then
                        installedwserverlink_type.Add("git")
                        gitlink = gitlink.Replace("REPGIT", "")
                    ElseIf gitlink.Contains("REPSVN") Then
                        installedwserverlink_type.Add("svn")
                        gitlink = gitlink.Replace("REPSVN", "")
                    End If
                    gitid = gitlink
                    Dim g As Integer = gitlink.IndexOf("STEAMID")
                    If g >= 0 Then
                        gitid = gitid.Substring(g + 7)
                    End If
                    installedwserverlink_ids.Add(gitid)
                    Dim f As Integer = gitlink.IndexOf("STEAMID")
                    If f >= 0 Then
                        gitlink = gitlink.Remove(f)
                    End If
                    gitlink = gitlink.Replace(vbCrLf, "")
                    gitlink = gitlink.Replace(gamename, "")
                    installedwserverlink_links.Add(gitlink)
                    installedwserverlink_directory.Add(Item.DirectoryName)
                Else
                    'MANUAL LINK GETTING
                    If My.Computer.FileSystem.DirectoryExists(Item.DirectoryName & "\.git") = True Then
                        If My.Computer.FileSystem.FileExists("idget.bat") Then
                            My.Computer.FileSystem.DeleteFile("idget.bat")
                        Else
                        End If
                        My.Computer.FileSystem.WriteAllText("idget.bat", drive & codecd & gitlocale & getlink1, True, Encoding.ASCII)
                        Shell("idget.bat", AppWinStyle.Hide)
                        Do Until My.Computer.FileSystem.DirectoryExists(gitlocale & "/" & "endidget")
                            Threading.Thread.Sleep(100)
                        Loop
                        My.Computer.FileSystem.DeleteDirectory(gitlocale & "/" & "endidget", FileIO.DeleteDirectoryOption.DeleteAllContents)
                        My.Computer.FileSystem.DeleteFile("idget.bat")
                        If My.Computer.FileSystem.ReadAllText(gitlocale & "/" & "lanlink.txt") <> "" Then
                            modfoundlink_names.Add(gamename)
                            modfoundlink_links.Add(My.Computer.FileSystem.ReadAllText(gitlocale & "/" & "lanlink.txt"))
                            modfoundlink_directory.Add(Item.DirectoryName)
                            My.Computer.FileSystem.DeleteFile(gitlocale & "/" & "lanlink.txt")
                        Else
                            My.Computer.FileSystem.DeleteFile(gitlocale & "/" & "lanlink.txt")
                        End If
                    End If
                    'idlist
                    Dim gitsecids As String = client.DownloadString("https://raw.githubusercontent.com/DorukSega/launcherdatabed/master/steamidsfornonlinked.txt")
                    If gitsecids.Contains(gamename) Then
                        steamidknown_name.Add(gamename)
                        Dim d As Integer = gitsecids.IndexOf(gamename)
                        If d >= 0 Then
                            gitsecids = gitsecids.Substring(d)
                        End If
                        gitsecids = gitsecids.Replace(gamename, "")
                        Do While gitsecids.Contains("END")
                            Dim e As Integer = gitsecids.LastIndexOf("END")
                            If e >= 0 Then
                                gitsecids = gitsecids.Remove(e)
                            End If
                        Loop
                        gitsecids = gitsecids.Replace(vbCrLf, "")
                        steamidknown_id.Add(gitsecids)
                    End If
                End If
            End If
        Next
        Call assingload()

    End Sub
    Public Sub assingload()
        If Not My.Computer.FileSystem.DirectoryExists("config") Then
            My.Computer.FileSystem.CreateDirectory("config")
        End If
        If My.Computer.FileSystem.FileExists("config\assignedids.bin") Then
            assingedidmodnames.Clear()
            assingedidmodids.Clear()
            Dim asnames As String = My.Computer.FileSystem.ReadAllText("config\assignedids.bin")
            Dim asids As String = My.Computer.FileSystem.ReadAllText("config\assignedids.bin")
            Do While asnames.Contains("NAME")
                Dim y As Integer = asnames.IndexOf("NAME")
                If y >= 0 Then
                    asnames = asnames.Substring(y + 4)
                End If
                Dim assname1 As String = ""
                Dim b As Integer = asnames.IndexOf("ID")
                If b >= 0 Then
                    assname1 = asnames.Remove(b)
                End If
                Dim h As Integer = asids.IndexOf("ID")
                If h >= 0 Then
                    asids = asids.Substring(h + 2)
                End If
                Dim assid1 As String = ""
                Dim v As Integer = asids.IndexOf("END")
                If v >= 0 Then
                    assid1 = asids.Remove(v)
                End If
                assingedidmodnames.Add(assname1)
                assingedidmodids.Add(assid1)
            Loop
        End If
    End Sub
    Public Sub Sourcemodsfolderassign()
        If Not My.Computer.FileSystem.DirectoryExists("config") Then
            My.Computer.FileSystem.CreateDirectory("config")
        End If
        If My.Computer.FileSystem.FileExists("config\launcherconfig.bin") Then
            My.Computer.FileSystem.DeleteFile("config\launcherconfig.bin")
        End If
browser:
        Dim result As DialogResult = FolderBrowserDialog1.ShowDialog()
        If result = Windows.Forms.DialogResult.OK And My.Computer.FileSystem.DirectoryExists(FolderBrowserDialog1.SelectedPath) Then
            Dim path As String = FolderBrowserDialog1.SelectedPath
            sourcemodsfolder = FolderBrowserDialog1.SelectedPath
            TextBox1.Text = sourcemodsfolder
            My.Computer.FileSystem.WriteAllText("config\launcherconfig.bin", sourcemodsfolder, True)
            modsfolder = New DirectoryInfo(sourcemodsfolder)
        Else
            GoTo browser
        End If
        Call Listing()
    End Sub
    Private Sub FolderSearchButton_Click(sender As Object, e As EventArgs) Handles FolderSearchButton.Click
        Call Sourcemodsfolderassign()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        'check if it is in assigned mod names or installedwserverlink_names or modfoundlink_names
        TextBox2.Text = ""
        Button4.Enabled = True
        If installedwserverlink_names.Contains(ComboBox1.Text) Or modfoundlink_names.Contains(ComboBox1.Text) Then
            Button1.Enabled = True
            Button2.Enabled = True
        Else
            Button1.Enabled = False
            Button2.Enabled = False
        End If
        If installedwserverlink_names.Contains(ComboBox1.Text) Then
            Dim z As Integer = installedwserverlink_names.IndexOf(ComboBox1.Text)
            If z >= 0 Then
                TextBox2.Text = installedwserverlink_ids.Item(z)
                Button4.Enabled = False
                ' PictureBox1.Image = Image.FromFile(installedwserverlink_directory.Item(z) & "/resource/game.tga")
            End If
        ElseIf assingedidmodnames.Contains(ComboBox1.Text) Then
            Dim z As Integer = assingedidmodnames.IndexOf(ComboBox1.Text)
            If z >= 0 Then
                TextBox2.Text = assingedidmodids.Item(z)
            End If
        ElseIf steamidknown_name.Contains(ComboBox1.Text) Then
            Dim z As Integer = steamidknown_name.IndexOf(ComboBox1.Text)
            If z >= 0 Then
                TextBox2.Text = steamidknown_id.Item(z)
                Button4.Enabled = False
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click  'launch
        'get parameters and assign them, maybe somewhere else

        'add a option to check for updates every launch
        If CheckBox1.CheckState = CheckState.Checked Then
            Call updatemod()
        End If
        If TextBox2.Text <> "" And ComboBox1.Text <> "" Then
            If My.Computer.FileSystem.FileExists("gamelaunch.bat") Then
                My.Computer.FileSystem.DeleteFile("gamelaunch.bat")
            Else
            End If
            My.Computer.FileSystem.WriteAllText("gamelaunch.bat", code1 & TextBox2.Text & code2 & parameters & code3, True, Encoding.ASCII)
            Shell("gamelaunch.bat", AppWinStyle.Hide)
            Do Until My.Computer.FileSystem.DirectoryExists("endplay")
                Threading.Thread.Sleep(100)
            Loop
            My.Computer.FileSystem.DeleteDirectory("endplay", FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.DeleteFile("gamelaunch.bat")
        Else
            If ComboBox1.Text = "" Then
                MsgBox("There is no mod picked", 6, "Launch")
            Else
                MsgBox("There is no steamid assigned, go to steam and create a shortcut of the mod and go to the properties of the shortcut you created and copy the url after steam://rungameid/ then paste it to steam id text box and then press assign to save it for later", 6, "Launch")
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click 'assign
        If TextBox2.Text <> "" Then
            assingedidmodnames.Add(ComboBox1.Text)
            assingedidmodids.Add(TextBox2.Text)
            'local save
            If My.Computer.FileSystem.FileExists("config\assignedids.bin") Then
                If My.Computer.FileSystem.ReadAllText("config\assignedids.bin").Contains(ComboBox1.Text) Then
                    Dim changetext As String = My.Computer.FileSystem.ReadAllText("config\assignedids.bin")
                    changetext = changetext.Replace("NAME" & ComboBox1.Text & "ID" & assingedidmodids.Item(assingedidmodnames.IndexOf(ComboBox1.Text)) & "END", "")
                    My.Computer.FileSystem.DeleteFile("config\assignedids.bin")
                    My.Computer.FileSystem.WriteAllText("config\assignedids.bin", changetext, True)
                End If
            End If
            My.Computer.FileSystem.WriteAllText("config\assignedids.bin", "NAME" & ComboBox1.Text & "ID" & TextBox2.Text & "END", True)
            Call assingload()
        Else
            MsgBox("There is no steamid written", 6, "Steamid")
        End If
    End Sub

    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click  'RESET
        If installedwserverlink_names.Contains(ComboBox1.Text) Then
            Dim z As Integer = installedwserverlink_names.IndexOf(ComboBox1.Text)
            If z >= 0 Then
                Dim curlink As String = installedwserverlink_links.Item(z)
                Dim curdirectory As String = installedwserverlink_directory.Item(z)
                Dim curtype As String = installedwserverlink_type.Item(z)
                If curtype = "git" Then
                    'add bash code here
                    If My.Computer.FileSystem.FileExists("reset.bat") Then
                        My.Computer.FileSystem.DeleteFile("reset.bat")
                    Else
                    End If
                    My.Computer.FileSystem.WriteAllText("reset.bat", drive & codecd & curdirectory & reset1, True, Encoding.ASCII)
                    'add a option to change to hidden
                    If CheckBox4.CheckState = CheckState.Checked Then
                        Shell("reset.bat", AppWinStyle.Hide)
                    Else
                        Shell("reset.bat", AppWinStyle.NormalFocus)
                    End If
                    Do Until My.Computer.FileSystem.DirectoryExists(curdirectory & "\endreset")
                        Threading.Thread.Sleep(100)
                    Loop
                    My.Computer.FileSystem.DeleteDirectory(curdirectory & "\endreset", FileIO.DeleteDirectoryOption.DeleteAllContents)
                    My.Computer.FileSystem.DeleteFile("reset.bat")
                ElseIf curtype = "svn" Then
                    'add bash code here
                    If My.Computer.FileSystem.FileExists("reset.bat") Then
                        My.Computer.FileSystem.DeleteFile("reset.bat")
                    Else
                    End If
                    'add a option to change to hidden
                    If CheckBox4.CheckState = CheckState.Checked Then
                        My.Computer.FileSystem.WriteAllText("reset.bat", drive & codecd & curdirectory & reset12, True, Encoding.ASCII)
                        Shell("reset.bat", AppWinStyle.Hide)
                    Else
                        My.Computer.FileSystem.WriteAllText("reset.bat", drive & codecd & curdirectory & reset1, True, Encoding.ASCII)
                        Shell("reset.bat", AppWinStyle.NormalFocus)
                    End If
                    Do Until My.Computer.FileSystem.DirectoryExists(curdirectory & "\endreset")
                        Threading.Thread.Sleep(100)
                    Loop
                    My.Computer.FileSystem.DeleteDirectory(curdirectory & "\endreset", FileIO.DeleteDirectoryOption.DeleteAllContents)
                    My.Computer.FileSystem.DeleteFile("reset.bat")
                End If
            End If
        ElseIf modfoundlink_names.Contains(ComboBox1.Text) Then
            Dim z As Integer = modfoundlink_names.IndexOf(ComboBox1.Text)
            If z >= 0 Then
                Dim curlink As String = modfoundlink_links.Item(z)
                Dim curdirectory As String = modfoundlink_directory.Item(z)
                'add bash code here
                If My.Computer.FileSystem.FileExists("reset.bat") Then
                    My.Computer.FileSystem.DeleteFile("reset.bat")
                Else
                End If
                If CheckBox4.CheckState = CheckState.Checked Then
                    My.Computer.FileSystem.WriteAllText("reset.bat", drive & codecd & curdirectory & reset12, True, Encoding.ASCII)
                    Shell("reset.bat", AppWinStyle.Hide)
                Else
                    My.Computer.FileSystem.WriteAllText("reset.bat", drive & codecd & curdirectory & reset1, True, Encoding.ASCII)
                    Shell("reset.bat", AppWinStyle.NormalFocus)
                End If
                Do Until My.Computer.FileSystem.DirectoryExists(curdirectory & "\endreset")
                    Threading.Thread.Sleep(100)
                Loop
                My.Computer.FileSystem.DeleteDirectory(curdirectory & "\endreset", FileIO.DeleteDirectoryOption.DeleteAllContents)
                My.Computer.FileSystem.DeleteFile("reset.bat")
            End If
        End If
    End Sub
    Public Sub updatemod()
        If installedwserverlink_names.Contains(ComboBox1.Text) Then
            Dim z As Integer = installedwserverlink_names.IndexOf(ComboBox1.Text)
            If z >= 0 Then
                Dim curlink As String = installedwserverlink_links.Item(z)
                Dim curdirectory As String = installedwserverlink_directory.Item(z)
                Dim curtype As String = installedwserverlink_type.Item(z)
                If curtype = "git" Then
                    'add bash code here
                    If My.Computer.FileSystem.FileExists("update.bat") Then
                        My.Computer.FileSystem.DeleteFile("update.bat")
                    Else
                    End If

                    'add a option to change to hidden
                    If CheckBox2.CheckState = CheckState.Checked Then
                        My.Computer.FileSystem.WriteAllText("update.bat", drive & codecd & curdirectory & upd1 & curlink & upd22, True, Encoding.ASCII)
                        Shell("update.bat", AppWinStyle.Hide)
                    Else
                        My.Computer.FileSystem.WriteAllText("update.bat", drive & codecd & curdirectory & upd1 & curlink & upd2, True, Encoding.ASCII)
                        Shell("update.bat", AppWinStyle.NormalFocus)
                    End If
                    Do Until My.Computer.FileSystem.DirectoryExists(curdirectory & "\endupdate")
                        Threading.Thread.Sleep(100)
                    Loop
                    My.Computer.FileSystem.DeleteDirectory(curdirectory & "\endupdate", FileIO.DeleteDirectoryOption.DeleteAllContents)
                    My.Computer.FileSystem.DeleteFile("update.bat")
                ElseIf curtype = "svn" Then
                    'add bash code here
                    If My.Computer.FileSystem.FileExists("update.bat") Then
                        My.Computer.FileSystem.DeleteFile("update.bat")
                    Else
                    End If

                    'add a option to change to hidden
                    If CheckBox2.CheckState = CheckState.Checked Then
                        My.Computer.FileSystem.WriteAllText("update.bat", drive & codecd & curdirectory & updsvn1 & curlink & updsvn22, True, Encoding.ASCII)
                        Shell("update.bat", AppWinStyle.Hide)
                    Else
                        My.Computer.FileSystem.WriteAllText("update.bat", drive & codecd & curdirectory & updsvn1 & curlink & updsvn2, True, Encoding.ASCII)
                        Shell("update.bat", AppWinStyle.NormalFocus)
                    End If
                    Do Until My.Computer.FileSystem.DirectoryExists(curdirectory & "\endupdate")
                        Threading.Thread.Sleep(100)
                    Loop
                    My.Computer.FileSystem.DeleteDirectory(curdirectory & "\endupdate", FileIO.DeleteDirectoryOption.DeleteAllContents)
                    My.Computer.FileSystem.DeleteFile("update.bat")
                End If
            End If
        ElseIf modfoundlink_names.Contains(ComboBox1.Text) Then
            Dim z As Integer = modfoundlink_names.IndexOf(ComboBox1.Text)
            If z >= 0 Then
                Dim curlink As String = modfoundlink_links.Item(z)
                Dim curdirectory As String = modfoundlink_directory.Item(z)
                'add bash code here
                If My.Computer.FileSystem.FileExists("update.bat") Then
                    My.Computer.FileSystem.DeleteFile("update.bat")
                Else
                End If
                If CheckBox2.CheckState = CheckState.Checked Then
                    My.Computer.FileSystem.WriteAllText("update.bat", drive & codecd & curdirectory & upd1 & curlink & upd22, True, Encoding.ASCII)
                    Shell("update.bat", AppWinStyle.Hide)
                Else
                    My.Computer.FileSystem.WriteAllText("update.bat", drive & codecd & curdirectory & upd1 & curlink & upd2, True, Encoding.ASCII)
                    Shell("update.bat", AppWinStyle.NormalFocus)
                End If
                Do Until My.Computer.FileSystem.DirectoryExists(curdirectory & "\endupdate")
                    Threading.Thread.Sleep(100)
                Loop
                My.Computer.FileSystem.DeleteDirectory(curdirectory & "\endupdate", FileIO.DeleteDirectoryOption.DeleteAllContents)
                My.Computer.FileSystem.DeleteFile("update.bat")
            End If
        End If
    End Sub
    Public Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click  'update
        Call updatemod()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        parameters = TextBox3.Text
        If Not My.Computer.FileSystem.FileExists("config\parameterconfig.bin") Then
            My.Computer.FileSystem.WriteAllText("config\parameterconfig.bin", parameters, True, Encoding.ASCII)
        Else
            My.Computer.FileSystem.DeleteFile("config\parameterconfig.bin")
            My.Computer.FileSystem.WriteAllText("config\parameterconfig.bin", parameters, True, Encoding.ASCII)
        End If
    End Sub
    Public Sub installlisting()
        'add install stuff
        Dim insname As String = client.DownloadString("https://raw.githubusercontent.com/DorukSega/launcherdatabed/master/installablestuff.txt")
        Dim insname1 As String = ""
        Dim insdesc As String = client.DownloadString("https://raw.githubusercontent.com/DorukSega/launcherdatabed/master/installablestuff.txt")
        Dim insdesc1 As String = ""
        Dim inslink As String = client.DownloadString("https://raw.githubusercontent.com/DorukSega/launcherdatabed/master/installablestuff.txt")
        Dim inslink1 As String = ""
        Dim insids As String = client.DownloadString("https://raw.githubusercontent.com/DorukSega/launcherdatabed/master/installablestuff.txt")
        Dim insids1 As String = ""
        Do While insname.Contains("NAME")
            Dim y As Integer = insname.IndexOf("NAME")
            If y >= 0 Then
                insname = insname.Substring(y + 4)
            End If
            Dim b As Integer = insname.IndexOf("DESC")
            If b >= 0 Then
                insname1 = insname.Remove(b)
            End If
            Dim h As Integer = insdesc.IndexOf("DESC")
            If h >= 0 Then
                insdesc = insdesc.Substring(h + 4)
            End If
            Dim v As Integer = insdesc.IndexOf("LINK")
            If v >= 0 Then
                insdesc1 = insdesc.Remove(v)
            End If
            Dim h1 As Integer = inslink.IndexOf("LINK")
            If h1 >= 0 Then
                inslink = inslink.Substring(h1 + 4)
            End If
            Dim v2 As Integer = inslink.IndexOf("ID")
            If v2 >= 0 Then
                inslink1 = inslink.Remove(v2)
            End If
            Dim he As Integer = insids.IndexOf("ID")
            If he >= 0 Then
                insids = insids.Substring(he + 2)
            End If
            Dim ve As Integer = insids.IndexOf("END")
            If ve >= 0 Then
                insids1 = insids.Remove(ve)
            End If
            If insids1.Contains("REPGIT") Then
                insids1 = insids1.Replace("REPGIT", "")
                installable_rep.Add("git")
            Else
                insids1 = insids1.Replace("REPSVN", "")
                installable_rep.Add("svn")
            End If
            installable_name.Add(insname1)
            installable_desc.Add(insdesc1)
            installable_link.Add(inslink1)
            installable_id.Add(insids1)
            ComboBox2.Items.Add(insname1)
        Loop
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        RichTextBox1.Text = ""
        Button5.Enabled = True
        If installedmods.Contains(ComboBox2.Text) Then
            Button5.Enabled = False
        Else
        End If
        Dim index As Integer = installable_name.IndexOf(ComboBox2.Text)
        If index >= 0 Then
            RichTextBox1.Text = installable_desc.Item(index)
            TextBox4.Text = installable_link.Item(index)
            TextBox5.Text = installable_id.Item(index)
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Button5.Enabled = False
        Dim z As Integer = installable_name.IndexOf(ComboBox2.Text)
        If z >= 0 Then
            Dim curlink As String = installable_link.Item(z)
            Dim curtype As String = installable_rep.Item(z)
            If curtype = "git" Then
                'add bash code here
                If My.Computer.FileSystem.FileExists("install.bat") Then
                    My.Computer.FileSystem.DeleteFile("install.bat")
                Else
                End If
                'add a option to change to hidden
                If CheckBox3.CheckState = CheckState.Checked Then
                    My.Computer.FileSystem.WriteAllText("install.bat", drive & codecd & sourcemodsfolder & ins1 & curlink & ins22, True, Encoding.ASCII)
                    Shell("install.bat", AppWinStyle.Hide)
                Else
                    My.Computer.FileSystem.WriteAllText("install.bat", drive & codecd & sourcemodsfolder & ins1 & curlink & ins2, True, Encoding.ASCII)
                    Shell("install.bat", AppWinStyle.NormalFocus)
                End If
                Do Until My.Computer.FileSystem.DirectoryExists(sourcemodsfolder & "\endinstall")
                    Threading.Thread.Sleep(100)
                Loop
                My.Computer.FileSystem.DeleteDirectory(sourcemodsfolder & "\endinstall", FileIO.DeleteDirectoryOption.DeleteAllContents)
                My.Computer.FileSystem.DeleteFile("install.bat")
                MsgBox("Restart Steam before launching")
            ElseIf curtype = "svn" Then
                'add bash code here
                If My.Computer.FileSystem.FileExists("install.bat") Then
                    My.Computer.FileSystem.DeleteFile("install.bat")
                Else
                End If
                'add a option to change to hidden
                If CheckBox3.CheckState = CheckState.Checked Then
                    My.Computer.FileSystem.WriteAllText("install.bat", drive & codecd & sourcemodsfolder & inssvn1 & curlink & inssvn22, True, Encoding.ASCII)
                    Shell("install.bat", AppWinStyle.Hide)
                Else
                    My.Computer.FileSystem.WriteAllText("install.bat", drive & codecd & sourcemodsfolder & inssvn1 & curlink & inssvn2, True, Encoding.ASCII)
                    Shell("install.bat", AppWinStyle.NormalFocus)
                End If
                Do Until My.Computer.FileSystem.DirectoryExists(sourcemodsfolder & "\endinstall")
                    Threading.Thread.Sleep(100)
                Loop
                My.Computer.FileSystem.DeleteDirectory(sourcemodsfolder & "\endinstall", FileIO.DeleteDirectoryOption.DeleteAllContents)
                My.Computer.FileSystem.DeleteFile("install.bat")
                MsgBox("Restart Steam before launching")
            End If
        End If
        Button5.Enabled = True
        Call Listing()
    End Sub
End Class
