Imports System.IO
Imports System.Net
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.Data.Sql
Public Class _frmUpdateFile
    Dim m_CountTo As Integer = 0
    Dim db As New ConnectionString
    Dim xs As New Class1
    Dim dateToday As String
    Dim lastUpdated As String
    Dim au As New AutoUpdate
    Dim datetimeToday As String
    Dim listview1 As New ListView

    Dim lCount_to As Integer
    Private Declare Function InternetCloseHandle Lib "wininet.dll" (ByVal HINet As Integer) As Integer

    Private Sub _frmUpdateFile_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Control.CheckForIllegalCrossThreadCalls = False

        _frmFTPSettings.modTag = 2
        _frmFTPSettings.Hide()
        ''MsgBox("Update Logs")

        xs.xml_Filename = Application.StartupPath & "\Mysettings.xml" '"Mysettings.xml"
        xs.value_path = "Configuration/Settings"
        bWorker2.RunWorkerAsync()
        'fetchSql()

        datetimeToday = Date.Now.ToString("yyyy-MM-dd hh:mm:ss tt")
        dateToday = Date.Now.ToString("yyyy-MM-dd")

    End Sub

    Private Function clickSSIT()
        Try
            Dim listbox1 As New ListBox
            Dim myPath As String = Application.StartupPath & "\files"


            listbox1.Items.Clear()
            For Each p As Process In Process.GetProcesses
                If p.MainWindowTitle = String.Empty = False Then
                    listbox1.Items.Add(p.MainWindowTitle)
                End If
            Next

            'Dim Mystring1 As String = "SSIT SERVER"
            Dim MyFilePath = "C:\Users\Public\Desktop\SSIT_SERVER.lnk"
            Shell("RUNDLL32.EXE URL.DLL,FileProtocolHandler " & MyFilePath, vbMaximizedFocus)

        Catch ex As Exception

            MsgBox(ex.Message, vbOKOnly, "SSIT UPDATE FILES CLICKSSIT")


        End Try

    End Function

    Private Sub checkFile()
        Try
            _frmFTPSettings.Hide()
            '"ftp://10.0.202.97/CITIZEN_CHARTER/SAMPLE101.txt"
            Dim newLview As New ListView
            Dim updateTag As Integer
            Dim ctr As Integer
            Dim branch, kioskID As String
            Dim getDateTime As String = Date.Now.ToString("MM/dd/yyyy hh:mm:ss tt")
            Dim stat, tag As Integer ' status if it is successfull 1 - success, 0  - failed.
            Dim lastUpdated As String
            '  db.FillListView(db.ExecuteSQLQuery("select tag,dateupdated  from SSINFOTERMUPDATE where DATEUPDATED  = '" & dateToday & "'"), listview1)
            Dim terminalIP As String = xs.readSettings(xs.xml_Filename, xs.value_path, "terminalIP")

            db.FillListView(db.ExecuteSQLQuery("select tag,dateupdated  from SSINFOTERMUPDATE "), listview1)

            branch = db.putSingleValue("select branch_cd  from ssinfotermkiosk where BRANCH_IP ='" & terminalIP & "'")
            kioskID = db.putSingleValue("select kiosk_id from ssinfotermkiosk where BRANCH_IP  = '" & terminalIP & "'")
            Dim lvcount As Integer = listview1.Items.Count
            'MsgBox(lvcount)


            For Each item As ListViewItem In listview1.Items
                updateTag = item.Text
                lCount_to = lCount_to + 1
                If updateTag = 1 Then

                    Dim Cnt1 As String = db.putSingleValue("SELECT * from  SSUPDATELOGS WHERE KIOSK_IP = '" & terminalIP & "' and tag = '" & updateTag & "'  ")


                    ' citizen
                    If Cnt1 = "" Or Cnt1 = Nothing Then
                        xs.editSettings(xs.xml_Filename, xs.value_path, "Citizen_lastUpdated", item.SubItems.Item(1).Text)
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "charter\CITIZENS_CHARTER.pdf")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Citizen_Charter\CITIZENS_CHARTER.pdf")
                        xs.backUpSettings()

                        getFileFtp("1")
                        tag = 1
                        stat = 1
                        db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")

                        'MsgBox("updated citizen")
                    Else
                        lastUpdated = xs.readSettings(xs.xml_Filename, xs.value_path, "Citizen_lastUpdated")

                        If CDate(lastUpdated) < CDate(item.SubItems.Item(1).Text) Then
                            xs.editSettings(xs.xml_Filename, xs.value_path, "Citizen_lastUpdated", item.SubItems.Item(1).Text)
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "charter\CITIZENS_CHARTER.pdf")
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Citizen_Charter\CITIZENS_CHARTER.pdf")
                            xs.backUpSettings()
                            'My.Settings.Citizen_lastUpdated = item.SubItems.Item(1).Text
                            'My.Settings.Save()
                            '' My.Settings.Reload()
                            'My.Settings.SSIT_Path = "charter\CITIZENS_CHARTER.pdf"  ' SSIT_SERVER/SSIT_SERVER.exe
                            'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                            'My.Settings.ftp_Path = "charter\CITIZENS_CHARTER.pdf"  'Application/SSIT_SERVER.exe

                            getFileFtp("1")
                            tag = 1
                            stat = 1
                            db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")

                            'MsgBox("updated citizen")
                        End If

                    End If

                ElseIf updateTag = 2 Then
                    ' video
                    'C:\Program Files\SSIT\SSIT_SERVER\videos

                    Dim Cnt1 As String = db.putSingleValue("SELECT * from  SSUPDATELOGS WHERE KIOSK_IP = '" & terminalIP & "' and tag = '" & updateTag & "'  ")


                    Dim vidPath As String = "C:\Program Files\SSIT\SSIT_SERVER\Video"
                    If (Not System.IO.Directory.Exists(vidPath)) Then
                        System.IO.Directory.CreateDirectory(vidPath)
                    End If

                    If Cnt1 = "" Or Cnt1 = Nothing Then

                        'C:\Program Files\SSIT\SSIT_SERVER\videos\SSS.mp4
                        xs.editSettings(xs.xml_Filename, xs.value_path, "Video_lastUpdated", item.SubItems.Item(1).Text)
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Video\SSS.mp4")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Video/SSS.mp4")

                        xs.backUpSettings()
                        'My.Settings.Video_lastUpdated = item.SubItems.Item(1).Text
                        'My.Settings.Save()
                        'My.Settings.Reload()
                        'My.Settings.SSIT_Path = "Video\SSS.mp4"  ' SSIT_SERVER/SSIT_SERVER.exe
                        'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                        'My.Settings.ftp_Path = "Video/SSS.mp4"  'Application/SSIT_SERVER.exe


                        getFileFtp("1")
                        tag = 2
                        stat = 1
                        db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")
                        'MsgBox("updated video")

                    Else
                        lastUpdated = xs.readSettings(xs.xml_Filename, xs.value_path, "Video_lastUpdated")
                        If CDate(lastUpdated) < CDate(item.SubItems.Item(1).Text) Then
                            xs.editSettings(xs.xml_Filename, xs.value_path, "Video_lastUpdated", item.SubItems.Item(1).Text)
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Video\SSS.mp4")
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Video/SSS.mp4")
                            xs.backUpSettings()

                            'My.Settings.Video_lastUpdated = item.SubItems.Item(1).Text
                            'My.Settings.Save()
                            'My.Settings.Reload()

                            'My.Settings.SSIT_Path = "Video\SSS.mp4"  ' SSIT_SERVER/SSIT_SERVER.exe
                            'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                            'My.Settings.ftp_Path = "Video/SSS.mp4"  'Application/SSIT_SERVER.exe
                            getFileFtp("1")
                            tag = 2
                            stat = 1
                            db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")
                            'MsgBox("updated video")
                        End If

                    End If


                ElseIf updateTag = 3 Then
                    ' application
                    Dim Cnt1 As String = db.putSingleValue("SELECT * from  SSUPDATELOGS WHERE KIOSK_IP = '" & terminalIP & "' and tag = '" & updateTag & "'  ")

                    If Cnt1 = "" Or Cnt1 = Nothing Then
                        xs.editSettings(xs.xml_Filename, xs.value_path, "Application_lastUpdated", item.SubItems.Item(1).Text)
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "SSIT_SERVER\SSIT_SERVER.exe")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Application/SSIT_SERVER.exe")
                        xs.backUpSettings()

                        'My.Settings.Application_lastUpdated = item.SubItems.Item(1).Text
                        'My.Settings.Save()
                        '' My.Settings.Reload()

                        'My.Settings.SSIT_Path = "SSIT_SERVER\SSIT_SERVER.exe"  ' SSIT_SERVER/SSIT_SERVER.exe
                        'My.Settings.downloadedPath = "C:\Program Files\SSIT" 'C:\C:\Users\NCJALUAG\Desktop\JEan\08022014\NIKKI ETO NA\WindowsApplication22\WindowsApplication22\Connection\Program Files\SSIT
                        'My.Settings.ftp_Path = "Application/SSIT_SERVER.exe"  'Application/SSIT_SERVER.exe


                        'bWorker2.RunWorkerAsync()
                        getFileFtp("1")
                        tag = 3
                        stat = 1
                        db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")
                        '   MsgBox("updated application")
                    Else
                        'C:\Program Files\SSIT\SSIT_SERVER
                        lastUpdated = xs.readSettings(xs.xml_Filename, xs.value_path, "Application_lastUpdated")
                        If CDate(lastUpdated) < CDate(item.SubItems.Item(1).Text) Then
                            xs.editSettings(xs.xml_Filename, xs.value_path, "Application_lastUpdated", item.SubItems.Item(1).Text)
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "SSIT_SERVER\SSIT_SERVER.exe")
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\")
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Application/SSIT_SERVER.exe")
                            xs.backUpSettings()

                            'My.Settings.Application_lastUpdated = item.SubItems.Item(1).Text
                            'My.Settings.Save()
                            ' My.Settings.Reload()
                            'My.Settings.SSIT_Path = "SSIT_SERVER\SSIT_SERVER.exe"  ' SSIT_SERVER/SSIT_SERVER.exe
                            'My.Settings.downloadedPath = "C:\Program Files\SSIT" 'C:\Program Files\SSIT
                            'My.Settings.ftp_Path = "Application/SSIT_SERVER.exe"  'Application/SSIT_SERVER.exe


                            ' bWorker1.RunWorkerAsync()
                            getFileFtp("1")
                            tag = 3
                            stat = 1
                            db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")
                            '   MsgBox("updated application")
                        End If

                    End If
                ElseIf updateTag = 4 Then
                    ' do nothing
                ElseIf updateTag = 5 Then
                    ' terms
                    Dim Cnt1 As String = db.putSingleValue("SELECT * from  SSUPDATELOGS WHERE KIOSK_IP = '" & terminalIP & "' and tag = '" & updateTag & "'  ")

                    If Cnt1 = "" Or Cnt1 = Nothing Then
                        xs.editSettings(xs.xml_Filename, xs.value_path, "Terms_lastUpdated", item.SubItems.Item(1).Text)
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "terms_and_conditions\terms_and_conditions.txt")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                        xs.backUpSettings()
                        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Terms/terms_and_conditions.txt")
                        xs.backUpSettings()

                        'My.Settings.Terms_lastUpdated = item.SubItems.Item(1).Text
                        'My.Settings.Save()
                        ''My.Settings.Reload()
                        'My.Settings.SSIT_Path = "terms_and_conditions\terms_and_conditions.txt"  ' SSIT_SERVER/SSIT_SERVER.exe
                        'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                        'My.Settings.ftp_Path = "Terms/terms_and_conditions.txt"  'Application/SSIT_SERVER.exe
                        getFileFtp("1")
                        tag = 5
                        stat = 1
                        db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")
                        ' MsgBox("updated terms")
                    Else
                        lastUpdated = xs.readSettings(xs.xml_Filename, xs.value_path, "Terms_lastUpdated")
                        If CDate(lastUpdated) < CDate(item.SubItems.Item(1).Text) Then
                            xs.editSettings(xs.xml_Filename, xs.value_path, "Terms_lastUpdated", item.SubItems.Item(1).Text)
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "terms_and_conditions\terms_and_conditions.txt")
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                            xs.backUpSettings()
                            xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Terms/terms_and_conditions.txt")
                            xs.backUpSettings()


                            'My.Settings.Terms_lastUpdated = item.SubItems.Item(1).Text
                            'My.Settings.Save()
                            '' My.Settings.Reload()
                            'My.Settings.SSIT_Path = "terms_and_conditions\terms_and_conditions.txt"  ' SSIT_SERVER/SSIT_SERVER.exe
                            'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                            'My.Settings.ftp_Path = "Terms/terms_and_conditions.txt"  'Application/SSIT_SERVER.exe
                            getFileFtp("1")
                            tag = 5
                            stat = 1
                            db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")
                            '          MsgBox("updated terms")
                        End If
                    End If

                End If


            Next


            'Dim xmlFile As String = Application.StartupPath & "\Mysettings.xml"
            'Dim backUpFile As String = Application.StartupPath & "\config_backup"
            'Dim firstConfig As String = xs.readSettings(xs.xml_Filename, xs.value_path, "firstConfig")
            'If firstConfig = 0 Then
            '    System.IO.File.Copy(xmlFile, backUpFile)
            'End If


            ' MsgBox("DONE UPDATING FILES FROM FTP. NEXT SEND TRANS LOGS.")

            'Dim pathA As String = Application.StartupPath & "\MySettings.xml" '  source filename
            'Dim pathB As String = Application.StartupPath & "\Updates\MySettings.xml" '  destination file name
            'If Not File.Exists(pathB) Then
            '    System.IO.File.Copy(pathA, pathB)
            'Else
            '    System.IO.File.Delete(pathB)
            '    System.IO.File.Copy(pathA, pathB)
            'End If


            'Me.Hide()
            '_frmSendLogs.Show()

        Catch ex As Exception
            MsgBox(ex.Message & ex.StackTrace, vbOKOnly, "SSIT UPDATE FILES CHECKFILES")
        End Try

    End Sub
    Private Sub getFileFtp(ByVal cnt As Integer) ' cnt 0 - FetchSql , 1 - checkFile
        Try


            FTPSettings.IP = xs.readSettings(xs.xml_Filename, xs.value_path, "ftpIP")
            FTPSettings.UserID = xs.readSettings(xs.xml_Filename, xs.value_path, "ftpUserID")
            FTPSettings.Password = xs.readSettings(xs.xml_Filename, xs.value_path, "ftpPassword")
            Dim userID As String = FTPSettings.UserID
            Dim Password As String = FTPSettings.Password
            Dim IP As String = FTPSettings.IP

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
            Dim netCred As New NetworkCredential(userID, Password)

            Dim uri As New Uri("ftp://" + FTPSettings.IP + "/" + ftpPath)
            Dim testFileSize As Long = ReturnRemoteFileSize(uri, netCred)


            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile

            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(FTPSettings.UserID, FTPSettings.Password)
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            ftpStream = response.GetResponseStream()
            Dim cl As Long = response.ContentLength

            Dim bufferSize As Integer = 2048
            Dim readCount As Integer
            Dim buffer1() As Byte = New Byte(testFileSize) {}
            Dim buffer As Byte() = New Byte(bufferSize - 1) {}
            Dim PathTxt As String = "ftp://" & FTPSettings.IP & "/" + ftpPath
            '  Dim fileByte() As Byte '= System.IO.File.ReadAllBytes(PathTxt)


            ' MsgBox(testFileSize & "- File Size " & vbNewLine & "ftp://" + FTPSettings.IP + "/" + ftpPath & " -URI" & _
            '       vbNewLine & userID & "-UserID" & Password & "-Password")

            readCount = ftpStream.Read(buffer, 0, bufferSize)
            Dim fileByte As Byte


            Dim k As Integer = readCount
            ProgressBar1.Value = 0
            Dim infoReader As System.IO.FileInfo

            Dim size_1 As Integer
            While readCount > 0
                ' ProgressBar1.Value = k
                ' Dim bWork As New 

                ' Dim fileStream() As Byte = System.IO.File.ReadAllBytes(filePath + "\" + fileName)
                '   MsgBox(readCount & "- readcount enter" & vbNewLine & filePath & "File Path" & vbNewLine & fileName & "File Name")

                outputStream.Write(buffer, 0, readCount)
                readCount = ftpStream.Read(buffer, 0, bufferSize)
                'fileByte = ftpStream.Length

                infoReader = My.Computer.FileSystem.GetFileInfo(filePath + "\" + fileName)
                size_1 = infoReader.Length
                '   MsgBox(size_1 & "- Size" & vbNewLine & buffer1.Length.ToString & "-Buffer Length" & vbNewLine & ProgressBar1.Maximum.ToString & "-Progress Bar Maximum")
                If cnt = 0 Then
                    ' bWorker2.ReportProgress(CType(size_1 * ProgressBar1.Maximum / buffer1.Length, Integer))
                    bWorker2.ReportProgress(CType(((size_1 / buffer1.Length) * ProgressBar1.Maximum), Integer))
                Else
                    '   bWorker1.ReportProgress(CType(size_1 * ProgressBar1.Maximum / buffer1.Length, Integer))
                    bWorker1.ReportProgress(CType(((size_1 / buffer1.Length) * ProgressBar1.Maximum), Integer))

                End If
                'readCount = ftpStream.Read(buffer1, 0, bufferSize)
                k = k - 1
            End While


            ftpStream.Close()
            outputStream.Close()
            response.Close()

            '   MsgBox(readCount & "- readcount 2nd")


            infoReader = My.Computer.FileSystem.GetFileInfo(filePath + "\" + fileName)
            size_1 = infoReader.Length
            If cnt = 0 Then
                ' bWorker2.ReportProgress(CType(size_1 * ProgressBar1.Maximum / buffer1.Length, Integer))
                bWorker2.ReportProgress(CType(((size_1 / buffer1.Length) * ProgressBar1.Maximum), Integer))
            Else
                '   bWorker1.ReportProgress(CType(size_1 * ProgressBar1.Maximum / buffer1.Length, Integer))
                bWorker1.ReportProgress(CType(((size_1 / buffer1.Length) * ProgressBar1.Maximum), Integer))

            End If

            'MsgBox(readCount & "- readcount outside")

            _frmFTPSettings.Hide()
            ' MsgBox("Done Updating!")
            'updateFile(My.Settings.SSIT_origPath, Path.Combine(filePath, fileName))
        Catch ex As Exception
            MsgBox(ex.Message & ex.StackTrace, vbOKOnly, "SSIT UPDATE FILES GETFILEFTP")

        End Try


    End Sub

    
    Private Function ReturnRemoteFileSize(ByVal uri As Uri, _
                                      ByVal cred As NetworkCredential) As Long

        Dim retVal As Long = 0

        Try
            Dim thisFTP As FtpWebRequest = DirectCast(WebRequest.Create(uri), FtpWebRequest)

            With thisFTP
                .Credentials = cred
                .Method = WebRequestMethods.Ftp.GetFileSize
                .Proxy = Nothing
                Using ftpResponse As FtpWebResponse = DirectCast(thisFTP.GetResponse, FtpWebResponse)
                    retVal = ftpResponse.ContentLength
                End Using
            End With


        Catch ex As Exception
            retVal = -1

            MsgBox(ex.Message)
        End Try

        Return retVal

    End Function
    Private Function fetchSql()
        Try

      
        Dim settings_Path As String = "D:\SSIT\Settings"
        If (Not System.IO.Directory.Exists(settings_Path)) Then
            System.IO.Directory.CreateDirectory(settings_Path)
        End If

        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Settings/ORASettings.txt")
        xs.backUpSettings()
        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "D:\SSIT\")
        xs.backUpSettings()
        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Settings/ORASettings.txt")
        xs.backUpSettings()
        ' My.Settings.SSIT_Path = "Settings/ORASettings.txt"  ' SSIT_SERVER/SSIT_SERVER.exe
        'My.Settings.downloadedPath = "F:\SSIT\" 'C:\Program Files\SSIT
        'My.Settings.ftp_Path = "Settings/ORASettings.txt"  'Application/SSIT_SERVER.exe

        ' bWorker1.RunWorkerAsync()
        getFileFtp("0")

        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Settings/SQLSettings.txt")
        xs.backUpSettings()
        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Settings/SQLSettings.txt")
        xs.backUpSettings()
        'My.Settings.SSIT_Path = "Settings/SQLSettings.txt"
        'My.Settings.ftp_Path = "Settings/SQLSettings.txt"  'Application/SSIT_SERVER.exe

        ' bWorker1.RunWorkerAsync()
        getFileFtp("0")
        getsqlSettings()
            '  getsqlSettings()

        Catch ex As Exception
            'MsgBox("Not connected to the network1")
        End Try
    End Function

    Private Function getsqlSettings()
        Dim siStr, siStr2, SQLdsn, SQLserver, SQLuid, SQLpassword, SQLdb As String


        If (File.Exists(Path.Combine(filePath, ftpPath))) Then
            Dim settingsInfo As String
            Dim downloadedPath, ftp_Path As String
            downloadedPath = xs.readSettings(xs.xml_Filename, xs.value_path, "downloadedPath")
            ftp_Path = xs.readSettings(xs.xml_Filename, xs.value_path, "ftp_Path")
            Using SW As New IO.StreamReader(Path.Combine(downloadedPath, ftp_Path), False)
                settingsInfo = SW.ReadToEnd

                Dim _split3 As String() = settingsInfo.Split(New Char() {"|"c})

                For J = 0 To _split3.Length - 1
                    siStr2 = _split3(J)

                    If siStr2.Contains("DSN") Then
                        siStr2 = siStr2.Trim
                        SQLdsn = siStr2.Remove(0, 5)
                    ElseIf siStr2.Contains("Server") Then
                        siStr2 = siStr2.Trim
                        SQLserver = siStr2.Remove(0, 8)
                    ElseIf siStr2.Contains("Username") Then
                        siStr2 = siStr2.Trim
                        SQLuid = siStr2.Remove(0, 10)
                    ElseIf siStr2.Contains("Password") Then
                        siStr2 = siStr2.Trim
                        SQLpassword = siStr2.Remove(0, 10)
                    ElseIf siStr2.Contains("Database") Then
                        siStr2 = siStr2.Trim
                        SQLdb = siStr2.Remove(0, 10)
                    End If

                Next
                SW.Close()

            End Using
        End If

        xs.editSettings(xs.xml_Filename, xs.value_path, "db_Server", SQLserver)
        xs.backUpSettings()
        xs.editSettings(xs.xml_Filename, xs.value_path, "db_UName", SQLuid)
        xs.backUpSettings()
        xs.editSettings(xs.xml_Filename, xs.value_path, "db_Pass", SQLpassword)
        xs.backUpSettings()
        xs.editSettings(xs.xml_Filename, xs.value_path, "db_Name", SQLdb)
        xs.backUpSettings()
        xs.editSettings(xs.xml_Filename, xs.value_path, "db_DSN", SQLdsn)
        xs.backUpSettings()
        xs.editSettings(xs.xml_Filename, xs.value_path, "FirstRun", "1")
        xs.backUpSettings()

        'db_DSN, db_SERVER, db_Name, db_Uname, db_Pass As String
        db_DSN = xs.readSettings(xs.xml_Filename, xs.value_path, "db_DSN")
        db_SERVER = xs.readSettings(xs.xml_Filename, xs.value_path, "db_Server")
        db_Name = xs.readSettings(xs.xml_Filename, xs.value_path, "db_Name")
        db_Uname = xs.readSettings(xs.xml_Filename, xs.value_path, "db_UName")
        db_Pass = xs.readSettings(xs.xml_Filename, xs.value_path, "db_Pass")




        'My.Settings.db_Server = SQLserver
        'My.Settings.db_UName = SQLuid
        'My.Settings.db_Pass = SQLpassword
        'My.Settings.db_Name = SQLdb
        'My.Settings.db_DSN = SQLdsnf0
        'My.Settings.FirstRun = 1
        'My.Settings.Save()
        '  My.Settings.Reload()

        Dim temp As String = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
        'Dim temp As String = "SERVER=" & TextBox2.Text & ";DATABASE=" & TextBox5.Text & ";UID=" & TextBox3.Text & ";PWD=" & TextBox4.Text & ""
        ''Dim temp As String = "Data Source=" & TextBox2.Text & ";Initial Catalog=" & TextBox5.Text & ";User ID=" & TextBox3.Text & ";Password=" & TextBox4.Text & ""

        'If db.webisconnected(temp) Then
        '    MsgBox("Parameters are correct" & vbNewLine & "You are now connected to server", MsgBoxStyle.Information)


        '    'getsqlSettings()
        '    ' btnsave1.Enabled = True
        'Else
        '    ' MsgBox("Unable to connect!" & vbNewLine & "Please check if all the parameters are correct", MsgBoxStyle.Exclamation)
        '    'btnsave1.Enabled = False
        'End If


    End Function

    Private Function updateFile(ByVal filePath As String, ByVal fileDownloaded As String)
        ' filePath - path of the file to be updated. filedownloaded - path of the file downloaded from the ftp.
        File.Copy(fileDownloaded, filePath, True)

        'MsgBox("Done Updating!")
    End Function

    

   
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'BackgroundWorker1.RunWorkerAsync()
        'Dim statusUpdate As Boolean = au.updateFile

        'If statusUpdate = True Then
        '    Dim MyFilePath = Application.StartupPath & "\delete_Updater\UPDATES_UPDATER.exe"
        '    Shell("RUNDLL32.EXE URL.DLL,FileProtocolHandler " & MyFilePath, vbMaximizedFocus)
        '    Application.Exit()
        'ElseIf statusUpdate = False Then
        '    '   MsgBox("File will not be updated")
        '    ' do nothing 
        '    bWorker1.RunWorkerAsync()

        '    'clickSSIT()
        'End If


        xs.xml_Filename = Application.StartupPath & "\Mysettings.xml" '"Mysettings.xml"
        xs.value_path = "Configuration/Settings"
        bWorker2.RunWorkerAsync()
        '  fetchSql()

        datetimeToday = Date.Now.ToString("yyyy-MM-dd hh:mm:ss tt")
        dateToday = Date.Now.ToString("yyyy-MM-dd")



    End Sub

    Private Sub bWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bWorker1.DoWork
        checkFile()
    End Sub

    Private Sub bWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        lblPercent.Text = e.ProgressPercentage & " %"

    End Sub

    Private Sub bWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bWorker1.RunWorkerCompleted
        ' MsgBox("File uploaded.", MsgBoxStyle.Information)
        'If IsProgramRunning("old_sss_decrypt.exe") = 0 Then
        '    OpenOldSSSDecrypt()
        'End If

        clickSSIT()
        Me.Hide()
        _frmSendLogs.Show()

    End Sub
    Public Shared Function IsProgramRunning(ByVal Program As String) As Short
        Dim p() As Process
        p = Process.GetProcessesByName(Program.Replace(".exe", "").Replace(".EXE", ""))

        Return p.Length
    End Function

    Public Shared Sub OpenOldSSSDecrypt()
        Dim strFile As String = "C:\Program Files\SSIT\SSIT_SERVER\sss card\old_sss_decrypt.exe"



        If IO.File.Exists(strFile) Then
            Dim startInfo As New ProcessStartInfo(strFile)
            startInfo.WindowStyle = ProcessWindowStyle.Hidden
            startInfo.Arguments = "iL91*z(YoO@a"
            Process.Start(startInfo)
        Else
            MsgBox(strFile & " does not exist")
        End If
    End Sub


    Private Sub bWorker2_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bWorker2.DoWork
        fetchSql()
    End Sub

    Private Sub bWorker2_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bWorker2.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        lblPercent.Text = e.ProgressPercentage & " %"
    End Sub
  
    Private Function cred() As ICredentials
        Throw New NotImplementedException
    End Function

    
    Private Sub _frmUpdateFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _frmUpdateFiles.Click
        'checkFile()
        bWorker1.RunWorkerAsync()
    End Sub

    Private Sub bWorker2_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bWorker2.RunWorkerCompleted
        Dim statusUpdate As Boolean = au.updateFile

        If statusUpdate = True Then
            Dim MyFilePath = Application.StartupPath & "\delete_Updater\UPDATES_UPDATER.exe"
            Shell("RUNDLL32.EXE URL.DLL,FileProtocolHandler " & MyFilePath, vbMaximizedFocus)
            Application.Exit()
        ElseIf statusUpdate = False Then
            '   MsgBox("File will not be updated")
            ' do nothing 
            'BackgroundWorker1.RunWorkerAsync()
            bWorker1.RunWorkerAsync()
            ' checkFile()
            '  clickSSIT()
        End If

    End Sub

    Private Sub _frmUpdateFile_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseHover
        Me.Cursor = Cursors.No
    End Sub
End Class
