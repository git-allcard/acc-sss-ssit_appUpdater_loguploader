Imports System.IO
Imports System.Threading
Public Class _frmSendLogs
    Dim db As New ConnectionString
    Dim xs As New Class1
    Dim strFilename As String
    Dim copyFile As String
    Dim dtoday As String
    Dim currTime As String
    Dim trd As Thread
    Dim firstRun As String
    Dim statFile_Path As String
    Dim terminalIP As String
    Dim toBeSend As Boolean
    Dim logFiles(2) As String
    Dim txt4 As String
    Private Sub sendLogs()
        Try
            '  firstRun = 1
            _frmUpdateFile.Hide()
            _frmFTPSettings.Hide()

            Dim time_sent As String = db.putSingleValue("Select [Time] from SSINFOTERMDBSETTINGS where tag = '1' and type = 'SQL'")
            ' MsgBox(time_sent)
            ' STATUS 0(NOT YET SEND),1(ALREADY SEND)


            Dim lastUpdated As Date
            Dim dttime As DateTime = Convert.ToDateTime(time_sent)
            Dim timeSent1 As String = dttime.ToShortTimeString
            Dim todayTime As DateTime = Convert.ToDateTime(currTime)
            Dim timeToday As String = todayTime.ToShortTimeString
            Dim r As Integer
            'Dim kioskBranch As String

            Dim typeOfFile As String
            'MsgBox(kioskBranch)

            '----------------------

            For n = LBound(logFiles) To UBound(logFiles)
                If firstRun = 1 Then
                    If n = UBound(logFiles) Then
                        firstRun = 0
                    End If
                    Dim lUPdated As String
                    '   (1) = "Error"
                    '   (2) = "System"

                    If n = 0 Then
                        lUPdated = db.putSingleValue(" select max(ENCODE_DT) from  SSTRANSAT  where BRANCH_IP= '" & terminalIP & "' ")
                        typeOfFile = ".txt"
                    ElseIf n = 1 Then
                        lUPdated = db.putSingleValue(" select max(ENCODE_DT) from  SSTRANSERRORLOGS  where BRANCH_IP= '" & terminalIP & "' ")
                        typeOfFile = ".log"
                    ElseIf n = 2 Then
                        lUPdated = db.putSingleValue(" select max(ENCODE_DT) from  SSTRANSSYSTEMLOGS  where BRANCH_IP= '" & terminalIP & "' ")
                        typeOfFile = ".log"
                    Else
                        lUPdated = db.putSingleValue(" select max(ENCODE_DT) from  SSINFOTERMERRLOGS  where BRANCH_IP= '" & terminalIP & "' ")
                        typeOfFile = ".txt"
                    End If



                    ' MsgBox(lUPdated) ' IF NO DATE HAS FOUND IT MEANS THAT THE IP IS NOT CORRECT
                    '-------------

                    If lUPdated = "" Or lUPdated = Nothing Then
                        Dim prevDate22 As Date = DateAdd(DateInterval.Day, -1, Date.Today)
                        Dim getDay1 As String = prevDate22.Day
                        Dim getYear As String = prevDate22.Year
                        Dim getMonth As String = prevDate22.Month
                        Dim prevDate As String = getYear & "-" & getMonth & "-" & getDay1

                        lUPdated = prevDate
                        lastUpdated = CDate(lUPdated)
                    Else
                        lastUpdated = CDate(lUPdated)
                    End If
                    '------------
                    Dim daysOff As String = DateDiff(DateInterval.Day, lastUpdated, Now)
                    Dim getPrevDate1 As String = Format(lastUpdated, "dd")
                    Dim getYear1 As String = Format(lastUpdated, "yyyy")
                    Dim getMonth1 As String = Format(lastUpdated, "MM")
                    Dim dateStr As String = DateAdd(DateInterval.Day, 0, lastUpdated)
                    '-------------
                    ' MsgBox(lastUpdated)
                    ' MsgBox(daysOff)
                    For r = 0 To daysOff - 1

                        Dim getPrevDate2 As Date
                        getPrevDate2 = DateAdd(DateInterval.Day, r, lastUpdated)
                        getPrevDate1 = Format(getPrevDate2, "dd")
                        getYear1 = Format(getPrevDate2, "yyyy")
                        getMonth1 = Format(getPrevDate2, "MM")

                        Dim prevDate1 As String = getPrevDate1 & getMonth1 & getYear1
                        Dim getdate1 As String = Date.Today.ToString("ddMMyyyy") 'Date.Today.ToString("ddMMyyyy")

                        '   MsgBox(prevDate1 & " " & getdate1)
                        dateStr = DateAdd(DateInterval.Day, r, lastUpdated)
                        If Not Date.Now <= CDate(dateStr) Then

                            Dim statFile1 As String = "D:\SSIT\logs\" & prevDate1 & " " & kioskBranch

                            strFilename = "D:\SSIT\logs\" & prevDate1 & " " & kioskBranch & "\" & logFiles(n) & typeOfFile   '"\transaction_logs.txt"
                            copyFile = "D:\SSIT\logs\" & prevDate1 & " " & kioskBranch & "\" & logFiles(n) & "1" & typeOfFile '"\transaction_logs1.txt"
                            statFile_Path = statFile1 & "\" & logFiles(n) & "_status.txt" '& "\status_logs.txt"
                            Dim txt6 As String
                            'D:\SSIT\logs\06012015 C02
                            If (File.Exists(strFilename)) Then
                                If Not (File.Exists(statFile_Path)) Then
                                    putToFile(statFile1, n)
                                    statusLogs(statFile1, "1", n)
                                    File.Delete(copyFile)
                                    ' MsgBox("Done updating the logs for yesterday!")
                                Else
                                    Using SW As New IO.StreamReader(statFile_Path, False)
                                        txt6 = SW.ReadToEnd()
                                        SW.Close()
                                    End Using

                                    If txt6.Trim = 0 Then
                                        putToFile(statFile1, n)
                                        statusLogs(statFile1, "1", n)
                                        File.Delete(copyFile)
                                    Else
                                        '  MsgBox("Logs was updated yesterday!")
                                    End If
                                End If
                            Else
                                '   MsgBox("No Logs yesterday  ! " & r)
                            End If
                        End If
                    Next

                Else

                    'Dim lUPdated As String
                    '   (1) = "Error"
                    '   (2) = "System"

                    If n = 0 Then
                        'lUPdated = db.putSingleValue(" select max(ENCODE_DT) from  SSTRANSAT  where BRANCH_IP= '" & terminalIP & "' ")
                        typeOfFile = ".txt"
                    ElseIf n = 1 Then
                        ' lUPdated = db.putSingleValue(" select max(ENCODE_DT) from  SSTRANSERRORLOGS  where BRANCH_IP= '" & terminalIP & "' ")
                        typeOfFile = ".log"
                    ElseIf n = 2 Then
                        ' lUPdated = db.putSingleValue(" select max(ENCODE_DT) from  SSTRANSSYSTEMLOGS  where BRANCH_IP= '" & terminalIP & "' ")
                        typeOfFile = ".log"
                    Else
                        ' lUPdated = db.putSingleValue(" select max(ENCODE_DT) from  SSINFOTERMERRLOGS  where BRANCH_IP= '" & terminalIP & "' ")
                        typeOfFile = ".txt"
                    End If


                    Dim getPrevDate As String = Format(Date.Today, "dd")
                    Dim getYear As String = Format(Date.Today, "yyyy")
                    Dim getMonth As String = Format(Date.Today, "MM")
                    Dim prevDate As String = getPrevDate & getMonth & getYear
                    Dim getdate As String = Date.Today.ToString("ddMMyyyy") 'Date.Today.ToString("ddMMyyyy")
                    Dim statFile As String = "D:\SSIT\logs\" & prevDate & " " & kioskBranch
                    strFilename = "D:\SSIT\logs\" & prevDate & " " & kioskBranch & "\" & logFiles(n) & typeOfFile '"\transaction_logs.txt"
                    copyFile = "D:\SSIT\logs\" & prevDate & " " & kioskBranch & "\" & logFiles(n) & "1" & typeOfFile  ' "\transaction_logs1.txt"
                    statFile_Path = statFile & "\" & logFiles(n) & "_status.txt" '"\status_logs.txt"
                    Dim txt0 As String

                    '-------------
                    If Now >= CDate(Now.Date & " " & timeSent1) Then
                        If (File.Exists(strFilename)) Then
                            If Not (File.Exists(statFile_Path)) Then
                                putToFile(statFile, n)
                                statusLogs(statFile, "0", n)
                                ' MsgBox("Done UPDATING THE LOGS FOR TODAY!" & strFilename)
                                If n = UBound(logFiles) Then
                                    Application.Exit()
                                End If

                                bln = False

                            Else
                                Using SW As New IO.StreamReader(statFile_Path, False)
                                    txt0 = SW.ReadToEnd()
                                    SW.Close()
                                End Using

                                If txt0.Trim = 0 Then
                                    putToFile(statFile, n)
                                    statusLogs(statFile, "0", n)
                                    'trd.Abort()
                                    bln = False
                                    '  MsgBox("Done UPDATING THE LOGS FOR TODAY!" & strFilename)
                                    If n = UBound(logFiles) Then
                                        Application.Exit()
                                    End If
                                Else
                                    ' MsgBox("No Logs TODAY!" & strFilename)
                                    If n = UBound(logFiles) Then
                                        Application.Exit()
                                    End If
                                End If
                            End If
                        Else
                            '  MsgBox("No Logs for today(END ELSE)!" & statFile_Path)
                            If n = UBound(logFiles) Then
                                Application.Exit()
                            End If

                        End If
                    End If
                End If
            Next

            

            _frmUpdateFile.Hide()
            _frmFTPSettings.Hide()



        Catch ex As Exception

            ' MsgBox("Date of Last Update is empty.", "SSIT_UPDATER_SENDLOGS")
            MsgBox(ex.Message, vbOKOnly, "SSIT SEND LOGS SENDLOGS")
        End Try

    End Sub

    Private Sub statusLogs(ByVal statFile As String, ByVal statusValue As String, ByVal n As Integer)
        statFile_Path = statFile & "\" & logFiles(n) & "_status.txt" ' "\status_logs.txt"
        Using SW As New IO.StreamWriter(statFile_Path, False)
            Dim txt2 As String = statusValue
            SW.WriteLine(txt2)
            SW.Close()
        End Using
    End Sub

    Private Function putToFile(ByVal statFile As String, ByVal n As Integer)
        Try
            Dim txt3 As String
            'Dim aRow(12) As String
            Dim txtRead
            Dim txtRead1
            Dim readOrig
            Dim readOrig1
            Dim txt2 As String
            Dim truText As String
            _frmUpdateFile.Hide()
            _frmFTPSettings.Hide()

            If System.IO.File.Exists(copyFile) = False Then

                File.Copy(strFilename, copyFile)
                Dim read1 As StreamReader = New StreamReader(copyFile)
                txtRead = read1.ReadToEnd
                If n = 0 Or n = 1 Or n = 2 Then
                    txtRead1 = txtRead.ToString.Split(vbNewLine)
                Else

                    txtRead1 = txtRead.ToString.Split("**")
                End If

                'txtRead1 = txtRead.ToString.Split("**")
                txtRead = txtRead.ToString.Trim
                read1.Close()
                read1 = Nothing

                If txtRead <> "" Then
                    Dim txtString, txtString1 As String
                    Dim txt1 As String


                    For k = LBound(txtRead1) To UBound(txtRead1)

                        txt4 = txtRead1(k).ToString.Trim
                        If txt4 = "" Then
                        Else
                            txtString1 = txt4 & vbNewLine & vbNewLine
                            txtString = txtString & txtString1

                            Dim sAry As String() = Split(txt4.Replace("'", ""), "|")  ' separate the fields
                            If n = 0 Then
                                transLogs(sAry)
                            ElseIf n = 1 Then
                                CardErrorLogs(sAry)
                            ElseIf n = 2 Then
                                SystemLogs(sAry)
                            Else
                                kioskErrorLogs(sAry)
                            End If


                            Dim origRead As StreamReader = New StreamReader(strFilename)
                            readOrig = origRead.ReadToEnd
                            If n = 0 Or n = 1 Or n = 2 Then
                                readOrig1 = readOrig.ToString.Split(vbNewLine)
                                'ElseIf n = 1 Then
                            Else
                                readOrig1 = readOrig.ToString.Split("**")
                            End If

                            readOrig = readOrig.ToString.Trim
                            origRead.Close()
                            origRead = Nothing

                            Using SW As New IO.StreamWriter(strFilename, False)

                                txt3 = readOrig1(k)


                                txt2 = "1|" & txt3.Trim '& vbNewLine
                                truText = truText & txt2
                                txt1 = readOrig.ToString.Replace(txt3.Trim, txt2)
                                SW.WriteLine(txt1)
                                SW.Close()

                            End Using

                        End If

                    Next
                Else
                End If

            ElseIf System.IO.File.Exists(copyFile) = True Then
                Dim sendYesLogs As String
                Dim txtStr, txtStr1 As String
                Dim read1 As StreamReader = New StreamReader(strFilename)
                txtRead = read1.ReadToEnd
                If n = 0 Or n = 1 Or n = 2 Then
                    txtRead1 = txtRead.ToString.Split(vbNewLine)
                Else
                    txtRead1 = txtRead.ToString.Split("**")
                End If
                'txtRead1 = txtRead.ToString.Split(vbNewLine)
                txtRead = txtRead.ToString.Trim
                read1.Close()
                read1 = Nothing


                If txtRead <> "" Then
                    Dim yesLogs
                    yesLogs = txtRead1

                    For r = LBound(yesLogs) To UBound(yesLogs)
                        Dim yesLogs_delimit = yesLogs
                        txtStr = yesLogs_delimit(r)
                        If txtStr.Trim = "" Then
                        Else
                            txtStr = txtStr.ToString.Trim
                            Dim lAry As String() = Split(txtStr.ToString.Replace("'", ""), "|")  ' separate the fields

                            If lAry(0).Trim = "1" Then
                                ' do nothing
                            ElseIf lAry(0) <> "1" Then
                                If n = 0 Then
                                    transLogs(lAry)
                                ElseIf n = 1 Then
                                    CardErrorLogs(lAry)
                                ElseIf n = 2 Then
                                    SystemLogs(lAry)
                                Else
                                    kioskErrorLogs(lAry)
                                End If


                                Dim origRead As StreamReader = New StreamReader(strFilename)
                                readOrig = origRead.ReadToEnd
                                If n = 0 Or n = 1 Or n = 2 Then
                                    readOrig1 = readOrig.ToString.Split(vbNewLine)
                                Else
                                    readOrig1 = readOrig.ToString.Split("**")
                                End If

                                'readOrig1 = readOrig.ToString.Split(vbNewLine)
                                readOrig = readOrig.ToString.Trim
                                origRead.Close()
                                origRead = Nothing

                                Using SW As New IO.StreamWriter(strFilename, False)
                                    txt3 = readOrig1(r)

                                    txt2 = "1|" & txt3.Trim '& vbNewLine
                                    truText = truText & txt2
                                    sendYesLogs = readOrig.ToString.Replace(txt3.Trim, txt2)

                                    SW.WriteLine(sendYesLogs)

                                    SW.Close()
                                End Using
                            End If
                        End If
                    Next
                    File.Delete(copyFile)
                End If
            End If
            _frmUpdateFile.Hide()
            _frmFTPSettings.Hide()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly, "SSIT SEND LOGS")
        End Try


    End Function
    Private Sub transLogs(ByVal lAry As String())
        Try
            Dim aRow(12) As String
            aRow(0) = lAry(0)
            aRow(1) = lAry(1)
            aRow(2) = lAry(2)
            aRow(3) = lAry(3)
            aRow(4) = lAry(4)
            aRow(5) = lAry(5)
            aRow(6) = lAry(6)
            aRow(7) = lAry(7)
            aRow(8) = lAry(8)
            aRow(9) = lAry(9)
            aRow(10) = lAry(10)
            aRow(11) = lAry(11)
            aRow(12) = lAry(12)

            db.ExecuteSQLQuery("Insert Into SSTRANSAT (SSNUM,PROC_CD,TAG_MOD,ENCODE_DT,ENCODE_TME,BRANCH_IP,BRANCH_CD,CLSTR,DIVSN,MEMSTAT,CNT_PRNT,TRANSNUM,TRANS_DESC) values ('" & aRow(0) & "','" & aRow(1) & "','" & aRow(2) & "','" & aRow(3) & "','" & aRow(4) & "','" & aRow(5) & "','" & aRow(6) & "','" & aRow(7) & "','" & aRow(8) & "','" & aRow(9) & "','" & aRow(10) & "','" & aRow(11) & "','" & aRow(12) & "')")

        Catch ex As Exception
            'MsgBox(ex.Message, vbOKOnly, "SSIT SEND LOGS")
        End Try



    End Sub

    Private Sub SystemLogs(ByVal lAry As String()) ' Logs from sir edel system.txt
        Try

            Dim aRow(3) As String
            aRow(0) = lAry(0).Trim
            aRow(1) = lAry(1).Trim
            aRow(2) = lAry(2).Trim
            aRow(3) = lAry(3).Trim
            'aRow(4) = lAry(4)
            'aRow(5) = lAry(5)
            'aRow(6) = lAry(6)
            'aRow(7) = lAry(7)
            'aRow(8) = lAry(8)
            'aRow(9) = lAry(9)
            'aRow(10) = lAry(10)
            'aRow(11) = lAry(11)
            'aRow(12) = lAry(12)

            'db.ExecuteSQLQuery("Insert Into SSTRANSYSTEMLOGS (ENCODE_DT,TRANS_DESC,BRANCH_IP, BRANCH_CD) values ('" & aRow(0) & "','" & aRow(1) & "','" & aRow(2) & "','" & aRow(3) & "'")
            db.ExecuteSQLQuery("INSERT INTO SSTRANSSYSTEMLOGS (ENCODE_DT,TRANS_DESC,BRANCH_IP,BRANCH_CD) VALUES ('" & aRow(0) & "', '" & aRow(1) & "' , '" & aRow(2) & "', '" & aRow(3) & "')")
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly, "SSIT SEND LOGS")
        End Try



    End Sub
    Private Sub CardErrorLogs(ByVal lAry As String()) '  logs from sir edel for the card errors. error.txt
        Try

            Dim aRow(4) As String
            aRow(0) = lAry(0).Trim
            aRow(1) = lAry(1).Trim
            aRow(2) = lAry(2).Trim
            aRow(3) = lAry(3).Trim
            aRow(4) = lAry(4).Trim
            'aRow(5) = lAry(5)
            'aRow(6) = lAry(6)
            'aRow(7) = lAry(7)
            'aRow(8) = lAry(8)
            'aRow(9) = lAry(9)
            'aRow(10) = lAry(10)
            'aRow(11) = lAry(11)
            'aRow(12) = lAry(12)

            'db.ExecuteSQLQuery("Insert Into SSTRANSERRORLOGS (ENCODE_DT,TRANS_DESC,BRANCH_IP, BRANCH_CD) values ('" & aRow(0) & "','" & aRow(1) & "','" & aRow(2) & "','" & aRow(3) & "'")

            If (Not aRow(1) = Nothing) Or (Not aRow(1) = "") Then
                '    aRow(1) = "NO CARD DETECTED"
                db.ExecuteSQLQuery("INSERT INTO SSTRANSERRORLOGS (ENCODE_DT,TRANS_DESC,KIOSK_ID,BRANCH_CD,CARDTYPE) VALUES ('" & aRow(0) & "', '" & aRow(1) & "' , '" & aRow(2) & "', '" & aRow(3) & "', '" & aRow(4) & "')")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly, "SSIT SEND LOGS")
        End Try



    End Sub
    Private Sub kioskErrorLogs(ByVal lAry As String())
        Dim aRow(8) As String

        aRow(0) = lAry(0)
        aRow(1) = lAry(1)
        aRow(2) = lAry(2)
        aRow(3) = lAry(3)
        aRow(4) = lAry(4)
        aRow(5) = lAry(5)
        aRow(6) = lAry(6)
        aRow(7) = lAry(7)
        aRow(8) = lAry(8)
        'aRow(9) = lAry(9)
        'aRow(10) = lAry(10)
        'aRow(11) = lAry(11)
        'aRow(12) = lAry(12)

        'db.ExecuteSQLQuery("Insert into SSINFOTERMERRLOGS (BRANCH_IP,KIOSK_ID, KIOSK_NM,BRANCH_CD,LOGS,MODULE,PROC_NM, ENCODE_DT,ENCODE_TME) values ('" & aRow(0) & "','" & aRow(1) & "','" & aRow(2) & "','" & aRow(3) & "','" & aRow(4) & "','" & aRow(5) & "','" & aRow(6) & "','" & aRow(7) & "','" & aRow(8) & "')")
        db.ExecuteSQLQuery("Insert into SSINFOTERMERRLOGS (BRANCH_IP,KIOSK_ID, KIOSK_NM,BRANCH_CD,LOGS,MODULE,PROC_NM,ENCODE_DT,ENCODE_TME) VALUES ('" & aRow(0) & "', '" & aRow(1) & "', '" & aRow(2) & "', '" & aRow(3) & "', '" & aRow(4) & "', '" & aRow(5) & "', '" & aRow(6) & "', '" & aRow(7) & "', '" & aRow(8) & "')")


    End Sub

    Private bln As Boolean = True
    Private Sub ThreadTask()
        While bln
            Try
                runTime()
                sendLogs()

                _frmFTPSettings.Hide()
                System.Threading.Thread.Sleep(1000)
                'System.Threading.Thread.Sleep(1000 * 2400000)
            Catch ex As Exception

            End Try
        End While
    End Sub



    Public Sub runTime()
        Dim getTime As String = TimeOfDay.ToString("h:mm:ss tt")
        Dim getTimeTT As String = TimeOfDay.ToString("h:mm:ss tt")
        currTime = getTime
        _frmUpdateFile.Hide()
        _frmFTPSettings.Hide()
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

   

    Private Sub _frmSendLogs_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub



    Private Sub _frmSendLogs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' System.Diagnostics.Process.Start("C:\Program Files\SSIT\SSIT_SERVER\SSIT_SERVER.exe")
        'System.Diagnostics.Process.Start("C:\Program Files\SSIT\SSIT Application Checker\checker.exe")
        Try

            ' MsgBox("FORM Send Logs")
            toBeSend = False
            xs.xml_Filename = Application.StartupPath & "\Mysettings.xml" '"Mysettings.xml"
            xs.value_path = "Configuration/Settings"
            terminalIP = xs.readSettings(xs.xml_Filename, xs.value_path, "terminalIP")
            db_DSN = xs.readSettings(xs.xml_Filename, xs.value_path, "db_DSN")
            db_SERVER = xs.readSettings(xs.xml_Filename, xs.value_path, "db_Server")
            db_Name = xs.readSettings(xs.xml_Filename, xs.value_path, "db_Name")
            db_Uname = xs.readSettings(xs.xml_Filename, xs.value_path, "db_UName")
            db_Pass = xs.readSettings(xs.xml_Filename, xs.value_path, "db_Pass")

            kioskBranch = db.putSingleValue("select BRANCH_CD from SSINFOTERMKIOSK where BRANCH_IP  = '" & terminalIP & "'  ")
            xs.editSettings(xs.xml_Filename, xs.value_path, "kioskBranch", kioskBranch)
            xs.backUpSettings()
            _frmFTPSettings.modTag = 3


            logFiles(0) = "transaction_logs"

            logFiles(1) = "Error"
            logFiles(2) = "System"
            ' logFiles(3) = "InfoTerminal_logs"
            firstRun = 1
            Control.CheckForIllegalCrossThreadCalls = False
            trd = New Thread(AddressOf ThreadTask)
            trd.IsBackground = True
            trd.Start()

            _frmUpdateFile.Hide()
            _frmFTPSettings.Hide()
        Catch ex As Exception

        End Try

    End Sub
End Class