Imports System.Runtime.InteropServices
Imports System.Text

Namespace Shareware.core

    Public Class SharewareHook

        Public Class UserHWID
            <Flags()>
            Private Enum DockInfo
                DOCKINFO_DOCKED = &H2
                DOCKINFO_UNDOCKED = &H1
                DOCKINFO_USER_SUPPLIED = &H4
                DOCKINFO_USER_DOCKED = &H5
                DOCKINFO_USER_UNDOCKED = &H6
            End Enum

            <StructLayout(LayoutKind.Sequential)>
            Public Class HW_PROFILE_INFO
                <MarshalAs(UnmanagedType.U4)>
                Public dwDockInfo As Int32

                <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=39)>
                Public szHwProfileGuid As String

                <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)>
                Public szHwProfileName As String
            End Class

            <DllImport("advapi32.dll", SetLastError:=True)>
            Private Shared Function GetCurrentHwProfile(ByVal lpHwProfileInfo As IntPtr) As Boolean
            End Function

            Declare Function GetVolumeInformationA Lib "kernel32" (
                ByVal PathName As String,
                ByVal VolumeNameBuffer As StringBuilder,
                ByVal VolumeNameSize As Int32,
                ByRef VolumeSerialNumber As Int32,
                ByRef MaximumComponentLength As Int32,
                ByRef FileSystemFlags As Int32,
                ByVal FileSystemNameBuffer As StringBuilder,
                ByVal FileSystemNameSize As Int32) As Long

            Private Shared Function ProfileInfo() As HW_PROFILE_INFO
                Dim profile As HW_PROFILE_INFO
                Dim profilePtr As IntPtr = IntPtr.Zero
                Try
                    profile = New HW_PROFILE_INFO()
                    profilePtr = Marshal.AllocHGlobal(Marshal.SizeOf(profile))
                    Marshal.StructureToPtr(profile, profilePtr, False)

                    If Not GetCurrentHwProfile(profilePtr) Then
                        Throw New Exception("Error cant get current hw profile!")
                    Else
                        Marshal.PtrToStructure(profilePtr, profile)
                        Return profile
                    End If
                Catch e As Exception
                    Throw New Exception(e.ToString())
                Finally
                    If profilePtr <> IntPtr.Zero Then
                        Marshal.FreeHGlobal(profilePtr)
                    End If
                End Try
            End Function

            Private Shared Function GetVolumeSerial(ByVal strDriveLetter As String) As String
                Dim serNum As Integer = 0
                Dim maxCompLen As Integer
                Dim VolLabel As New StringBuilder(256)
                Dim VolFlags As Integer
                Dim FSName As New StringBuilder(256)
                Dim Ret As Long = GetVolumeInformationA(strDriveLetter + ":", VolLabel, VolLabel.Capacity, serNum, maxCompLen, VolFlags,
                 FSName, FSName.Capacity)
                Return Convert.ToString(serNum)
            End Function

            Shared Function getHWID() As String
                Dim info As HW_PROFILE_INFO = ProfileInfo()
                Dim GUID As String = info.szHwProfileGuid.ToString()
                Dim volumeserial As String = GetVolumeSerial(Environment.SystemDirectory.Substring(0, 1))

                Dim md5Hasher As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5.Create()
                Dim data As Byte() = md5Hasher.ComputeHash(Encoding.Default.GetBytes(GUID + volumeserial))
                Dim sBuilder As New StringBuilder()

                For i = 0 To data.Length - 1
                    sBuilder.Append(data(i).ToString("x2"))
                Next i

                getHWID = sBuilder.ToString()
            End Function

        End Class

    End Class

End Namespace

