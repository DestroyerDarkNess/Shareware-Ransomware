Imports System.IO

Namespace Shareware.core

    Public Class UFuncs


        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="FileHide"/> event of Execute Payload.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="DirP">
        ''' File Path to Hide.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Shared Sub FileHide(ByVal DirP As String)
            File.SetAttributes(DirP, FileAttributes.Hidden)
        End Sub

        Public Shared Sub OpenAsAdmin(ByVal FilePth As String, ByVal Argument As String)
            Dim procStartInfo As New ProcessStartInfo
            Dim procExecuting As New Process

            With procStartInfo
                .UseShellExecute = True
                .FileName = FilePth
                .Arguments = Argument
                .WindowStyle = ProcessWindowStyle.Normal
                .Verb = "runas"
            End With

            procExecuting = Process.Start(procStartInfo)
            End
        End Sub

        Public Function ConvertFileToBase64(ByVal fileName As String) As String
            Return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName))
        End Function

        Public Function ConvertBase64ToFile(ByVal Bstr64 As String) As Byte
            Return Convert.ToByte(Bstr64)
        End Function
       
        Public Function EncodeBase64(ByVal input As String) As String
            Return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input))
        End Function

        Public Function DecodeBase64(ByVal input As String) As String
            Return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(input))
        End Function

    End Class

End Namespace

