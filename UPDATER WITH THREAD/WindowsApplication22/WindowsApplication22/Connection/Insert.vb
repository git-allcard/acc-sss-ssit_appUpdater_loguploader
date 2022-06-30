Public Class Insert
    Dim db As New ConnectionString
    Public Function getModuleLogs(ByVal ssNumber As String, ByVal affectedTable As String, ByVal tagModule As String, ByVal transDate As String _
                                   , ByVal transTime As String, ByVal BRANCH_IP As String, ByVal BRANCH_CODE As String, ByVal CLUSTER As String _
                                   , ByVal DIVISION As String, ByVal MEMBER_STATUS As String, ByVal countPrint As Integer, ByVal TRANS_NUM As String _
                                   , ByVal DESC As String)

        Try
            Dim getbranchCoDE As String = db.putSingleValue("select BRANCH_CD from SSINFOTERMBR where BRANCH_NM = '" & BRANCH_CODE & "'")
            Dim getkiosk_cluster As String = db.putSingleValue("select CLSTR_CD from SSINFOTERMCLSTR where CLSTR_NM = '" & CLUSTER & "'")
            Dim getkiosk_group As String = db.putSingleValue("select GROUP_CD from SSINFOTERMGROUP where GROUP_NM = '" & DIVISION & "'")

            'If db.checkExistence("select autogenID, tagModule from SSTRANSAT where autogenID ='" & autoGen & "' and tagModule = '" & tagModule & "' ") = True Then
            '    db.ExecuteSQLQuery("update SSTRANSAT set Task = '" & task & "', transDate = '" & transDate & "', transTime = '" & transTime & "' where autogenID = '" & autoGen & "' ")
            'Else
            db.sql = "Insert into SSTRANSAT (SSNUM,PROC_CD,TAG_MOD,ENCODE_DT,ENCODE_TME,COUNT,BRANCH_IP,BRANCH_CD,CLSTR,DIVSN,MEMSTAT,CNT_PRNT,TRANSNUM,TRANS_DESC)values('" _
                & ssNumber & "','" & affectedTable & "','" & tagModule & "','" & transDate & "','" & transTime & "', '" & "1" & "','" & BRANCH_IP & _
                "', '" & getbranchCoDE & "','" & getkiosk_cluster & "','" & getkiosk_group & "','" & MEMBER_STATUS & "','" & countPrint & "', '" & TRANS_NUM & "', '" & DESC & "')"
            db.ExecuteSQLQuery(db.sql)
            'End If

        Catch ex As Exception

        End Try

    End Function


    Public Function insertInfoLogs(ByVal KioskIp As String, ByVal kioskID As String, ByVal kioskName As String, ByVal sd As String, ByVal errorlogs As String)
        db.sql = "insert into SSINFOTERMERRLOGS values('" & KioskIp & "', '" & kioskID & "', '" & kioskName & "', '" & sd & "', '" & errorlogs _
            & "','" & "Form: Main Form" & "', '" & "Click PENSION button error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
        db.ExecuteSQLQuery(db.sql)

    End Function



    Public Function getnum(ByVal dToday As String, ByVal transType As String)
        Dim getTransNo As String
        Dim getbranchCoDE As String = db.putSingleValue("select BRANCH_CD from SSINFOTERMBR where BRANCH_NM = '" & My.Settings.kioskBranch & "'")
        Using SW As New IO.StreamReader(Application.StartupPath & "\" & "REF_NUM\" & "\" & "REF_NUM.txt", True)
            getTransNo = SW.ReadToEnd
        End Using

        If getTransNo = "" Or getTransNo = Nothing Then
            getTransNo = "0001"
            Using SW As New IO.StreamWriter(Application.StartupPath & "\" & "Ref_Num\" & "\" & "REF_NUM.txt", False)
                SW.WriteLine(getTransNo)
            End Using
        Else

            getTransNo = getTransNo.PadLeft(4, "0") + 1
            getTransNo = getTransNo.PadLeft(4, "0")

            Using SW As New IO.StreamWriter(Application.StartupPath & "\" & "Ref_Num\" & "\" & "REF_NUM.txt", False)
                SW.WriteLine(getTransNo)
            End Using
        End If


        getnum = getbranchCoDE & My.Settings.kioskBranch & transType & dToday & getTransNo
        Return getnum

    End Function


    Public Function createFile()
        Dim filepath As String = Application.StartupPath & "\REF_NUM" ' & "\" & "Ref_Num\" & "REF_NUM.txt"  '  "C:\Users\Nikki Cassandra\Desktop\sample.txt"
        '  D:\for SSS\WindowsApplication22\WindowsApplication22\bin\Debug\REF_NUM
        If System.IO.Directory.Exists(filepath) = False Then
            System.IO.Directory.CreateDirectory(filepath)
            System.IO.File.Create(filepath & "\REF_NUM.txt").Dispose()
        End If

    End Function
End Class
