Imports System.IO
Imports System.Net
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.Data.Sql
Imports System.Threading
Public Class _frmUpdateFile
    Dim db As New ConnectionString
    Dim xs As New Class1
    Dim dateToday As String
    Dim lastUpdated As String
    Dim au As New AutoUpdate
    Dim datetimeToday As String
    Dim listview1 As New ListView
    Dim trd As Thread
    Dim loadMax As Integer
    Dim trd1 As Thread
    Private Declare Function InternetCloseHandle Lib "wininet.dll" (ByVal HINet As Integer) As Integer

    Private Sub _frmUpdateFile_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _frmFTPSettings.modTag = 2
        _frmFTPSettings.Hide()
        'MsgBox("Update Logs")

        ProgressBar1.Minimum = 0


        xs.xml_Filename = Application.StartupPath & "\Mysettings.xml" '"Mysettings.xml"
        xs.value_path = "Configuration/Settings"
        fetchSql()

        datetimeToday = Date.Now.ToString("yyyy-MM-dd hh:mm:ss tt")
        dateToday = Date.Now.ToString("yyyy-MM-dd")


        GC.Collect()
        Control.CheckForIllegalCrossThreadCalls = False
        trd = New Thread(AddressOf ThreadTask)
        trd.IsBackground = True
        trd.Start()

   

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

            MsgBox(ex.Message, vbOKOnly, "SSIT UPDATE FILES")


        End Try

    End Function
    Private Sub ThreadTask1()
        Do
            Try
                ProgressBar1.Value = ProgressBar1.Value + 1

                If ProgressBar1.Value = loadMax Then
                    ProgressBar1.Value = 0
                    clickSSIT()
                    Me.Hide()
                    _frmSendLogs.Show()
                    trd.Abort()
                End If

             
            Catch ex As Exception
                'MsgBox("Time Settings is not Updated! ", MsgBoxStyle.Information, "Information")
                'Application.Exit()
                'System.Diagnostics.Process.Start(My.Settings.cardPath & "SSIT_HOME.exe")
            End Try
        Loop
    End Sub
    Private Sub ThreadTask()
        Do
            Try
                Dim statusUpdate As Boolean = au.updateFile

                If statusUpdate = True Then
                    Dim MyFilePath = Application.StartupPath & "\delete_Updater\UPDATES_UPDATER.exe"
                    Shell("RUNDLL32.EXE URL.DLL,FileProtocolHandler " & MyFilePath, vbMaximizedFocus)
                    Application.Exit()
                    trd.Abort()

                ElseIf statusUpdate = False Then
                    '   MsgBox("File will not be updated")
                    ' do nothing

                    
                    checkFile()

                End If

            Catch ex As Exception
                'MsgBox("Time Settings is not Updated! ", MsgBoxStyle.Information, "Information")
                'Application.Exit()
                'System.Diagnostics.Process.Start(My.Settings.cardPath & "SSIT_HOME.exe")
            End Try
        Loop
    End Sub
    Private Sub checkFile()
        Try


            GC.Collect()
            Control.CheckForIllegalCrossThreadCalls = False
            trd1 = New Thread(AddressOf ThreadTask1)
            trd1.IsBackground = True
            trd1.Start()


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
                If updateTag = 1 Then

                    Dim Cnt1 As String = db.putSingleValue("SELECT * from  SSUPDATELOGS WHERE KIOSK_IP = '" & terminalIP & "' and tag = '" & updateTag & "'  ")


                    ' citizen
                    If Cnt1 = "" Or Cnt1 = Nothing Then
                        xs.editSettings(xs.xml_Filename, xs.value_path, "Citizen_lastUpdated", item.SubItems.Item(1).Text)
                        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "charter\CITIZENS_CHARTER.pdf")
                        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Citizen_Charter\CITIZENS_CHARTER.pdf")
                        getFileFtp()
                        tag = 1
                        stat = 1
                        db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")

                        'MsgBox("updated citizen")
                    Else
                        lastUpdated = xs.readSettings(xs.xml_Filename, xs.value_path, "Citizen_lastUpdated")

                        If CDate(lastUpdated) < CDate(item.SubItems.Item(1).Text) Then
                            xs.editSettings(xs.xml_Filename, xs.value_path, "Citizen_lastUpdated", item.SubItems.Item(1).Text)
                            xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "charter\CITIZENS_CHARTER.pdf")
                            xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                            xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Citizen_Charter\CITIZENS_CHARTER.pdf")
                            'My.Settings.Citizen_lastUpdated = item.SubItems.Item(1).Text
                            'My.Settings.Save()
                            '' My.Settings.Reload()
                            'My.Settings.SSIT_Path = "charter\CITIZENS_CHARTER.pdf"  ' SSIT_SERVER/SSIT_SERVER.exe
                            'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                            'My.Settings.ftp_Path = "charter\CITIZENS_CHARTER.pdf"  'Application/SSIT_SERVER.exe

                            getFileFtp()
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
                        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Video\SSS.mp4")
                        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Video/SSS.mp4")


                        'My.Settings.Video_lastUpdated = item.SubItems.Item(1).Text
                        'My.Settings.Save()
                        'My.Settings.Reload()
                        'My.Settings.SSIT_Path = "Video\SSS.mp4"  ' SSIT_SERVER/SSIT_SERVER.exe
                        'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                        'My.Settings.ftp_Path = "Video/SSS.mp4"  'Application/SSIT_SERVER.exe
                        getFileFtp()
                        tag = 2
                        stat = 1
                        db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")
                        'MsgBox("updated video")

                    Else
                        lastUpdated = xs.readSettings(xs.xml_Filename, xs.value_path, "Video_lastUpdated")
                        If CDate(lastUpdated) < CDate(item.SubItems.Item(1).Text) Then
                            xs.editSettings(xs.xml_Filename, xs.value_path, "Video_lastUpdated", item.SubItems.Item(1).Text)
                            xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Video\SSS.mp4")
                            xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                            xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Video/SSS.mp4")


                            'My.Settings.Video_lastUpdated = item.SubItems.Item(1).Text
                            'My.Settings.Save()
                            'My.Settings.Reload()

                            'My.Settings.SSIT_Path = "Video\SSS.mp4"  ' SSIT_SERVER/SSIT_SERVER.exe
                            'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                            'My.Settings.ftp_Path = "Video/SSS.mp4"  'Application/SSIT_SERVER.exe
                            getFileFtp()
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
                        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "SSIT_SERVER\SSIT_SERVER.exe")
                        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\")
                        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Application/SSIT_SERVER.exe")


                        'My.Settings.Application_lastUpdated = item.SubItems.Item(1).Text
                        'My.Settings.Save()
                        '' My.Settings.Reload()

                        'My.Settings.SSIT_Path = "SSIT_SERVER\SSIT_SERVER.exe"  ' SSIT_SERVER/SSIT_SERVER.exe
                        'My.Settings.downloadedPath = "C:\Program Files\SSIT" 'C:\C:\Users\NCJALUAG\Desktop\JEan\08022014\NIKKI ETO NA\WindowsApplication22\WindowsApplication22\Connection\Program Files\SSIT
                        'My.Settings.ftp_Path = "Application/SSIT_SERVER.exe"  'Application/SSIT_SERVER.exe
                        getFileFtp()
                        tag = 3
                        stat = 1
                        db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")
                        '   MsgBox("updated application")
                    Else
                        'C:\Program Files\SSIT\SSIT_SERVER
                        lastUpdated = xs.readSettings(xs.xml_Filename, xs.value_path, "Application_lastUpdated")
                        If CDate(lastUpdated) < CDate(item.SubItems.Item(1).Text) Then
                            xs.editSettings(xs.xml_Filename, xs.value_path, "Application_lastUpdated", item.SubItems.Item(1).Text)
                            xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "SSIT_SERVER\SSIT_SERVER.exe")
                            xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\")
                            xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Application/SSIT_SERVER.exe")

                            'My.Settings.Application_lastUpdated = item.SubItems.Item(1).Text
                            'My.Settings.Save()
                            ' My.Settings.Reload()
                            'My.Settings.SSIT_Path = "SSIT_SERVER\SSIT_SERVER.exe"  ' SSIT_SERVER/SSIT_SERVER.exe
                            'My.Settings.downloadedPath = "C:\Program Files\SSIT" 'C:\Program Files\SSIT
                            'My.Settings.ftp_Path = "Application/SSIT_SERVER.exe"  'Application/SSIT_SERVER.exe
                            getFileFtp()
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
                        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "terms_and_conditions\terms_and_conditions.txt")
                        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Terms/terms_and_conditions.txt")


                        'My.Settings.Terms_lastUpdated = item.SubItems.Item(1).Text
                        'My.Settings.Save()
                        ''My.Settings.Reload()
                        'My.Settings.SSIT_Path = "terms_and_conditions\terms_and_conditions.txt"  ' SSIT_SERVER/SSIT_SERVER.exe
                        'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                        'My.Settings.ftp_Path = "Terms/terms_and_conditions.txt"  'Application/SSIT_SERVER.exe
                        getFileFtp()
                        tag = 5
                        stat = 1
                        db.ExecuteSQLQuery("insert into SSUPDATELOGS (KIOSK_ID, KIOSK_IP, KIOSK_BRANCH,DATE_UPDATED, TAG,STATUS)  values ('" & kioskID & "', '" & terminalIP & "','" & branch & "','" & getDateTime & "','" & tag & "','" & stat & "' )")
                        ' MsgBox("updated terms")
                    Else
                        lastUpdated = xs.readSettings(xs.xml_Filename, xs.value_path, "Terms_lastUpdated")
                        If CDate(lastUpdated) < CDate(item.SubItems.Item(1).Text) Then
                            xs.editSettings(xs.xml_Filename, xs.value_path, "Terms_lastUpdated", item.SubItems.Item(1).Text)
                            xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "terms_and_conditions\terms_and_conditions.txt")
                            xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "C:\Program Files\SSIT\SSIT_SERVER\")
                            xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Terms/terms_and_conditions.txt")

                            'My.Settings.Terms_lastUpdated = item.SubItems.Item(1).Text
                            'My.Settings.Save()
                            '' My.Settings.Reload()
                            'My.Settings.SSIT_Path = "terms_and_conditions\terms_and_conditions.txt"  ' SSIT_SERVER/SSIT_SERVER.exe
                            'My.Settings.downloadedPath = "C:\Program Files\SSIT\SSIT_SERVER\" 'C:\Program Files\SSIT
                            'My.Settings.ftp_Path = "Terms/terms_and_conditions.txt"  'Application/SSIT_SERVER.exe
                            getFileFtp()
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
            'Me.Hide()
            '_frmSendLogs.Show()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly, "SSIT UPDATE FILES")
        End Try

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
            loadMax = readCount

            ProgressBar1.Maximum = readCount

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
            MsgBox(ex.Message, vbOKOnly, "SSIT UPDATE FILES")

        End Try


    End Sub

    Private Function fetchSql()
        Dim settings_Path As String = "D:\SSIT\Settings"
        If (Not System.IO.Directory.Exists(settings_Path)) Then
            System.IO.Directory.CreateDirectory(settings_Path)
        End If

        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Settings/ORASettings.txt")
        xs.editSettings(xs.xml_Filename, xs.value_path, "downloadedPath", "D:\SSIT\")
        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Settings/ORASettings.txt")
        ' My.Settings.SSIT_Path = "Settings/ORASettings.txt"  ' SSIT_SERVER/SSIT_SERVER.exe
        'My.Settings.downloadedPath = "F:\SSIT\" 'C:\Program Files\SSIT
        'My.Settings.ftp_Path = "Settings/ORASettings.txt"  'Application/SSIT_SERVER.exe
        getFileFtp()

        xs.editSettings(xs.xml_Filename, xs.value_path, "SSIT_Path", "Settings/SQLSettings.txt")
        xs.editSettings(xs.xml_Filename, xs.value_path, "ftp_Path", "Settings/SQLSettings.txt")
        'My.Settings.SSIT_Path = "Settings/SQLSettings.txt"
        'My.Settings.ftp_Path = "Settings/SQLSettings.txt"  'Application/SSIT_SERVER.exe
        getFileFtp()
        getsqlSettings()
        '  getsqlSettings()
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
        xs.editSettings(xs.xml_Filename, xs.value_path, "db_UName", SQLuid)
        xs.editSettings(xs.xml_Filename, xs.value_path, "db_Pass", SQLpassword)
        xs.editSettings(xs.xml_Filename, xs.value_path, "db_Name", SQLdb)
        xs.editSettings(xs.xml_Filename, xs.value_path, "db_DSN", SQLdsn)
        xs.editSettings(xs.xml_Filename, xs.value_path, "FirstRun", "1")

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
        'My.Settings.db_DSN = SQLdsn
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

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If ProgressBar1.Value = loadMax Then
            ProgressBar1.Value = 0
        End If

    End Sub
End Class
