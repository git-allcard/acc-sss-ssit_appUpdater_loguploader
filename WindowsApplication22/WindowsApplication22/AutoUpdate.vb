Imports System.IO
Imports System.Net
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.Data.Sql
Public Class AutoUpdate
    Dim db As New ConnectionString
    Dim xs As New Class1

    Public Function updateFile() As Boolean
        Try
            Dim lastUpdated As String
            Dim isFileBeUpdated As Boolean
            Dim branch, kioskID As String
            Dim getDateTime As String = Date.Now.ToString("MM/dd/yyyy hh:mm:ss tt")

            xs.xml_Filename = Application.StartupPath & "\Mysettings.xml" '"Mysettings.xml"
            xs.value_path = "Configuration/Settings"

            Dim terminalIP As String = xs.readSettings(xs.xml_Filename, xs.value_path, "terminalIP")
            'Dim updaterTag As String = db.putSingleValue("Select * from SSINFOTERMUPDATE where tag ='6'")
            branch = db.putSingleValue("select branch_cd  from ssinfotermkiosk where BRANCH_IP ='" & terminalIP & "'")
            kioskID = db.putSingleValue("select kiosk_id from ssinfotermkiosk where BRANCH_IP  = '" & terminalIP & "'")

            Dim listview1 As New ListView
            db.FillListView(db.ExecuteSQLQuery("select tag,dateupdated  from SSINFOTERMUPDATE where tag = '6' "), listview1)


            Dim stat As String
            stat = 0
            If listview1.Items.Count <> 0 Then
                Dim Cnt1 As String = db.putSingleValue("SELECT * from  SSUPDATELOGS WHERE KIOSK_IP = '" & terminalIP & "' and tag = '6'  ")

                If Cnt1 = "" Or Cnt1 = Nothing Then
                    xs.editSettings(xs.xml_Filename, xs.value_path, "Updater_LastUpdated", listview1.Items(0).SubItems(1).Text)
                    xs.backUpSettings()
                    xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Updates\SSIT_UPDATER.exe")
                    xs.backUpSettings()
                    xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", Class1.appRepo & "\SSIT_UPDATER\")
                    xs.backUpSettings()
                    xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Updater/SSIT_UPDATER.exe")
                    xs.backUpSettings()
                    getFileFtp()
                    stat = "1"
                    db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','6','" & stat & "' )")

                    isFileBeUpdated = True
                Else
                    lastUpdated = xs.readSettings(xs.xml_Filename, xs.value_path, "Updater_LastUpdated")

                    If CDate(lastUpdated) < CDate(listview1.Items(0).SubItems(1).Text) Then
                        xs.editSettings(xs.xml_Filename, xs.value_path, "Updater_LastUpdated", listview1.Items(0).SubItems(1).Text)
                        xs.backUpSettings()

                        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Updates/SSIT_UPDATER.exe")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", Class1.appRepo & "\SSIT_UPDATER\")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Updater/SSIT_UPDATER.exe")
                        xs.backUpSettings()


                        getFileFtp()
                        stat = 1
                        db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','6','" & stat & "' )")
                        isFileBeUpdated = True
                        'MsgBox("updated citizen")
                    Else
                        isFileBeUpdated = False
                    End If
                End If
            Else
                isFileBeUpdated = False
            End If

            Return isFileBeUpdated
        Catch ex As Exception

        End Try
     
    End Function
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
