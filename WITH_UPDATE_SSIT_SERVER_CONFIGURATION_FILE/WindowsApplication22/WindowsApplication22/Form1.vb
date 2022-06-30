Imports System.IO
Imports System.Net
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.Data.Sql
Public Class Form1
    Private Declare Function ReplaceFileW Lib "Kernel32.dll" (ByVal lpReplacedFileName As Long, ByVal lpReplacementFileName As Long, _
    ByVal lpBackupFileName As Long, ByVal dwReplaceFlags As Long, ByVal lpExclude As Long, ByVal lpReserved As Long) As Boolean
    Dim xs As New Class1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
       
        ''C:\Program Files\SSIT\SSIT_UPDATER\Updates

        'Dim commandLine As String
        'Dim mAU As New AutoUpdate

        'If mAU.AutoUpdate(commandLine) Then Exit Sub

        'xs.xml_Filename = Application.StartupPath & "\Mysettings.xml" '"Mysettings.xml"
        'xs.value_path = "Configuration/Settings"
        'xs.editSettings(xs.xml_Filename, xs.value_path, "Updater_LastUpdated", Date.Today)
        'xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Updates\WindowsApplication22.exe")
        'xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", Application.StartupPath)
        'xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Updater\WindowsApplication22.exe")

        'getFileFtp()


        'Dim command1() As String = Split(Microsoft.VisualBasic.Command(), "|")

        'MsgBox(command1)
    End Sub
    Private Sub getFileFtp()
        Try

            FTPSettings.IP = xs.readSettings(xs.xml_Filename, xs.value_path, "ftpIP")
            FTPSettings.UserID = xs.readSettings(xs.xml_Filename, xs.value_path, "ftpUserID")
            FTPSettings.Password = xs.readSettings(xs.xml_Filename, xs.value_path, "ftpPassword")

            ' Dim fileName As String = "Video/How-To-Video_MySSS_with_subtitle.mp4"
            'Dim fileName As String = My.Settings.SSIT_Path  '  FOLDER PATH FOR THE EXE OR TEXT FILE
            'Dim filePath As String = My.Settings.downloadedPath ' DIRECTORY AND DRIVE EX(C:\Program Files\SSIT)

            fileName = xs.readSettings(xs.xml_Filename, xs.value_path, "SSIT_Path")
            filePath = xs.readSettings(xs.xml_Filename, xs.value_path, "downloadedPath")


            Dim reqFTP As FtpWebRequest = Nothing
            Dim ftpStream As Stream = Nothing
            ftpPath = xs.readSettings(xs.xml_Filename, xs.value_path, "ftp_Path")



            If System.IO.File.Exists(Path.Combine(filePath, fileName)) Then
                System.IO.File.Delete(Path.Combine(filePath, fileName))
            End If

            Dim outputStream As New FileStream(filePath + "\" + fileName, FileMode.Create)
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri("ftp://" + FTPSettings.IP + "/" + ftpPath)), FtpWebRequest)

            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile
            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(FTPSettings.UserID, FTPSettings.Password)
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            ftpStream = response.GetResponseStream()
            Dim cl As Long = response.ContentLength
            Dim bufferSize As Integer = 2048
            Dim readCount As Integer
            Dim buffer As Byte() = New Byte(bufferSize - 1) {}


            readCount = ftpStream.Read(buffer, 0, bufferSize)
            While readCount > 0
                outputStream.Write(buffer, 0, readCount)
                readCount = ftpStream.Read(buffer, 0, bufferSize)
            End While

            ftpStream.Close()
            outputStream.Close()
            response.Close()
            _frmFTPSettings.Hide()
            ' MsgBox("Done Updating!")
            '  updateFile(My.Settings.SSIT_origPath, Path.Combine(filePath, fileName))
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

   
End Class