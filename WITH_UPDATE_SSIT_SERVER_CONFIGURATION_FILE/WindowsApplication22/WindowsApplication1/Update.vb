Imports System.IO
Imports System.Net
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.Data.Sql
Public Class Update

    Public Sub update_Updater(ByVal fileName As String, ByVal updates_path As String)
        Try
            If System.IO.File.Exists(fileName) Then
                System.IO.File.Delete(fileName)
            End If
            System.IO.File.Copy(updates_path, fileName)
            Dim MyFilePath = "C:\Users\Public\Desktop\SSIT_UPDATER.lnk"
            Shell("RUNDLL32.EXE URL.DLL,FileProtocolHandler " & MyFilePath, vbMaximizedFocus)

            Application.Exit()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
       
    End Sub
    Public Sub openUpdater()
        Dim MyFilePath = "C:\Users\Public\Desktop\SSIT_UPDATER.lnk"
        Shell("RUNDLL32.EXE URL.DLL,FileProtocolHandler " & MyFilePath, vbMaximizedFocus)
    End Sub
End Class
