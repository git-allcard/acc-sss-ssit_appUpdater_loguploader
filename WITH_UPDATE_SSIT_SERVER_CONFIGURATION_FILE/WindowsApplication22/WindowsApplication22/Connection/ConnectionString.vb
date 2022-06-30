Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Sql
Imports System.Text.RegularExpressions
Imports System.Data.Odbc

Public Class ConnectionString
    Implements IDisposable

    Dim xs As New Class1
    Public sql As String
    Public task As String
    'Public db_DSN, db_SERVER, db_Name, db_Uname, db_Pass As String
    'Public connString1 As String = My.Settings.Setting
    'Public connstring1 As String = "DSN=" & My.Settings.db_DSN & ";SERVER=" & My.Settings.db_Server & ";DATABASE=" & My.Settings.db_Name & ";UID=" & My.Settings.db_UName & ";PWD=" & My.Settings.db_Pass & ""
    'Public connString1 As String = "Data Source=" & tpd.DecryptData(My.Settings.db_Server) & ";Initial Catalog=" & tpd.DecryptData(My.Settings.db_Name) & ";User ID=" & tpd.DecryptData(My.Settings.db_UName) & ";Password=" & tpd.DecryptData(My.Settings.db_Pass) & ";"
    ' Public connString1 As String = "Data Source=" & (My.Settings.db_Server) & ";Initial Catalog=" & (My.Settings.db_Name) & ";User ID=" & (My.Settings.db_UName) & ";Password=" & (My.Settings.db_Pass) & ";"
    'Public conn As SqlConnection = New SqlConnection(connString1)

    '  db_DSN1 = xs.readSettings(xs.xml_Filename, xs.value_path, "FirstName")

    Public connstring1 As String '= "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
    Public conn As OdbcConnection ' = New OdbcConnection(connstring1)

    Private _connectionError As String

    Public Property connectionError() As String
        Get
            Return _connectionError
        End Get
        Set(ByVal value As String)
            _connectionError = value
        End Set
    End Property

    Public Function getDataTable(ByVal sql As String, ByVal tbl As String) As DataTable
        Dim dt As New DataTable
        connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
        conn = New OdbcConnection(connstring1)
        Try

            If conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Open()
            Else
                conn.Open()
            End If
            Dim da As OdbcDataAdapter = New OdbcDataAdapter(sql, conn)
            Dim ds As New DataSet
            da.Fill(ds, tbl)
            dt = ds.Tables(tbl)
        Catch ex As Exception
            'MsgBox("Error:" & ex.tostring)
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)

            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)

        Finally
            conn.Close()
        End Try
        Return dt
    End Function

    Public Function ExecuteSQLQuery(ByVal SQLQuery As String) As DataTable
        Dim sqlDT As New DataTable
        Try
            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)

            Dim sqlCon As New OdbcConnection(connstring1)
            Dim sqlDA As New OdbcDataAdapter(SQLQuery, sqlCon)
            Dim sqlCB As New OdbcCommandBuilder(sqlDA)
            sqlDA.Fill(sqlDT)


        Catch ex As Exception
            'MsgBox("Program Error: " & ex.tostring, MsgBoxStyle.Critical)
        End Try

        Return sqlDT
    End Function

    Public Sub FillListView(ByVal sqlData As DataTable, ByVal lvList As ListView)
        Try
            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)

            lvList.Items.Clear()
            lvList.Columns.Clear()
            Dim i As Integer
            Dim j As Integer
            For i = 0 To sqlData.Columns.Count - 1
                lvList.Columns.Add(sqlData.Columns(i).ColumnName)
            Next i
            For i = 0 To sqlData.Rows.Count - 1
                lvList.Items.Add(sqlData.Rows(i).Item(0))
                For j = 1 To sqlData.Columns.Count - 1
                    If Not IsDBNull(sqlData.Rows(i).Item(j)) Then
                        lvList.Items(i).SubItems.Add(sqlData.Rows(i).Item(j))
                    Else
                        lvList.Items(i).SubItems.Add("")
                    End If

                Next j
            Next i
            For i = 0 To sqlData.Columns.Count - 1
                lvList.Columns(i).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
            Next i
        Catch ex As Exception
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        End Try

    End Sub

    Public Function doNonQuery(ByVal sql As String, Optional ByVal process As String = "") As Boolean
        Try
            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)

            If conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Open()
            Else
                conn.Open()
            End If
            Dim cmd As OdbcCommand = New OdbcCommand(sql, conn)
            cmd.ExecuteNonQuery()
            doNonQuery = True
        Catch ex As Exception
            doNonQuery = False

            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        Finally
            conn.Close()
        End Try
    End Function
    Public Function checkExistence(ByVal sql As String) As Boolean
        Dim ans As Boolean = False
        Try
            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)

            If conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Open()
            Else
                conn.Open()
            End If
            Dim cmd As OdbcCommand = New OdbcCommand(sql, conn)
            Dim rdr As OdbcDataReader = cmd.ExecuteReader
            If rdr.Read Then
                ans = True
            End If
        Catch ex As Exception
            'MsgBox("Unable to connect to server" & vbNewLine & "Please check your database connection", MsgBoxStyle.Exclamation)
            'MsgBox("Error on(checkExistence): " & ex.tostring)
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        Finally
            conn.Close()
        End Try
        Return ans
    End Function

    Public Function checkExistence2(ByVal sql As String) As Boolean
        Dim ans As Boolean = False
        Try
            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)

            If conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Open()
            Else
                conn.Open()
            End If
            Dim cmd As OdbcCommand = New OdbcCommand(sql, conn)
            Dim rdr As OdbcDataReader = cmd.ExecuteReader
            If rdr.Read Then
                ans = True
            End If
        Catch ex As Exception
            'MsgBox("Unable to connect to server" & vbNewLine & "Please check your database connection", MsgBoxStyle.Exclamation)
            'MsgBox("Error on(checkExistence): " & ex.tostring)
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        Finally
            conn.Close()
        End Try
        Return ans
    End Function

    Public Function putSingleValue(ByVal sql As String, Optional ByVal tbl As String = "") As String
        Dim result As String = ""
        connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
        conn = New OdbcConnection(connstring1)
        Try


            If tbl <> "" Then
                Dim dt As DataTable = getDataTable(sql, tbl)
                If dt.Rows.Count <> 0 Then
                    If Not IsDBNull(dt.Rows(0)(0)) Then
                        result = dt.Rows(dt.Rows.Count - 1)(0)
                    End If
                End If
            Else
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                    conn.Open()
                Else
                    conn.Open()
                End If
                Dim cmd As OdbcCommand = New OdbcCommand(sql, conn)
                Dim rdr As OdbcDataReader = cmd.ExecuteReader
                If rdr.Read Then
                    If Not IsDBNull(rdr(0)) Then
                        result = rdr(0)
                    End If
                End If
            End If
        Catch ex As Exception
            'MsgBox("Error 1: " & ex.tostring)
            result = ""
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        Finally
            conn.Close()
        End Try
        Return result
    End Function
    Dim conStr As String
    Function webisconnected(ByVal constring As String) As Boolean
        conStr = constring
        Return isconnected()
    End Function

    Function isconnected() As Boolean
        Try
            If DBConnectionStatus() = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            'MsgBox(ex.tostring)
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        End Try

    End Function

    Private Function DBConnectionStatus() As Boolean
        Try
            Using conStr1 As New OdbcConnection(conStr)
                '("Server=" & txtServer.Text & ";" & _
                '"uid=" & txtLogin.Text & ";pwd=" & txtPassword.Text & "")
                conStr1.Open()
                Return (conStr1.State = ConnectionState.Open)
            End Using
        Catch e1 As OdbcException
            Return False
        Catch e2 As Exception
            Return False
        End Try
    End Function

    Public Sub FillListBox(ByVal sqlData As DataTable, ByVal lvList As ListBox)
        Try
            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)

            lvList.Items.Clear()
            Dim i As Integer
            Dim j As Integer
            For i = 0 To sqlData.Columns.Count - 1
                lvList.Items.Add(sqlData.Columns(i).ColumnName)
            Next i
            For i = 0 To sqlData.Rows.Count - 1
                If IsDBNull(sqlData.Rows(i).Item(0)) Then
                    lvList.Items.Add("")
                Else
                    lvList.Items.Add(sqlData.Rows(i).Item(0))
                End If

                For j = 1 To sqlData.Columns.Count - 1
                    If Not IsDBNull(sqlData.Rows(i).Item(j)) Then
                        'lvList.Items(i).SubItems.Add(sqlData.Rows(i).Item(j))
                    Else
                        'lvList.Items(i).SubItems.Add("")
                    End If
                Next j
            Next i
            'For i = 0 To sqlData.Columns.Count - 1
            '    lvList.Items(i).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
            'Next i
        Catch ex As Exception
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        End Try
    End Sub

    Public Sub fillComboBox(ByVal dt As DataTable, ByVal cb As ComboBox)
        Try

            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)
            cb.Items.Clear()
            For row As Integer = 0 To dt.Rows.Count - 1
                cb.Items.Add(dt.Rows(row)(0))
            Next
        Catch ex As Exception
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        End Try
    End Sub

    Function EmailAddressCheck(ByVal emailAddress As String) As Boolean
        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
        If emailAddressMatch.Success Then
            EmailAddressCheck = True
        Else
            EmailAddressCheck = False
        End If
    End Function

    Public Function EncryptText(ByVal sSTR As System.String) As String
        Dim sTmp As System.String
        Dim sResult As System.String
        Dim iCnt As System.Int32

        sTmp = StrReverse(sSTR)
        sResult = ""
        For iCnt = 1 To Len(sTmp)
            sResult = sResult & Chr(Asc(Mid(sTmp, iCnt, 1)) + Asc("g"))
        Next
        EncryptText = sResult
    End Function

    Public Function DecryptText(ByVal sSTR As String) As String

        Dim sTmp As String
        Dim sResult As String
        Dim icnt As Integer

        sTmp = StrReverse(sSTR)

        sResult = ""

        For icnt = 1 To Len(sTmp)
            sResult = sResult & Chr(Asc(Mid(sTmp, icnt, 1)) - Asc("g"))
        Next

        DecryptText = sResult

    End Function

    Public Function putSingleNumber(ByVal sql As String, Optional ByVal tbl As String = "") As String
        Dim result As Double = 0.0
        Try
            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)

            If tbl <> "" Then
                Dim dt As DataTable = getDataTable(sql, tbl)
                If dt.Rows.Count <> 0 Then
                    If Not IsDBNull(dt.Rows(0)(0)) Then
                        result = Val(dt.Rows(dt.Rows.Count - 1)(0))
                    End If
                End If
            Else
                conn.Open()
                Dim cmd As OdbcCommand = New OdbcCommand(sql, conn)
                Dim rdr As OdbcDataReader = cmd.ExecuteReader
                If rdr.Read Then
                    If Not IsDBNull(rdr(0)) Then
                        result = rdr(0)
                    End If
                End If
            End If

        Catch ex As Exception
            'MsgBox("Error 1: " & ex.tostring)
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        Finally
            conn.Close()
        End Try
        Return result
    End Function

    Public Function selectTable(ByVal sql As String) As Boolean
        Dim ans As Boolean = False
        Try
            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)

            conn.Open()
            Dim cmd As OdbcCommand = New OdbcCommand(sql, conn)
            Dim rdr As OdbcDataReader = cmd.ExecuteReader
            If rdr.Read Then
                ans = True
            End If
        Catch ex As Exception
            'MsgBox("Error on(selectTable): " & ex.tostring)
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        Finally
            conn.Close()
        End Try
        Return ans
    End Function

    Public Sub FillDataGridView(ByVal selectCommand As String, ByVal dgv As DataGridView)

        Try
            connstring1 = "DSN=" & db_DSN & ";SERVER=" & db_SERVER & ";DATABASE=" & db_Name & ";UID=" & db_Uname & ";PWD=" & db_Pass & ""
            conn = New OdbcConnection(connstring1)

            Dim cnn As String = connstring1
            Dim da = New OdbcDataAdapter(selectCommand, cnn)
            Dim cb As New OdbcCommandBuilder(da)
            Dim tbl As New DataTable()
            tbl.Locale = System.Globalization.CultureInfo.InvariantCulture
            da.Fill(tbl)
            dgv.DataSource = tbl
            'dgv.AutoResizeColumns( _
            '    DataGridViewAutoSizeColumnsMode.ColumnHeader)
        Catch ex As OdbcException
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        End Try
    End Sub

    Public Function GenRecID(ByVal getGen As String)
        Try


            Dim id As String = ""
            For x As Integer = getGen To 999999
                Dim tempID As String = x
                If checkExistence("select * from SSTRANSAT where ID='" & tempID & "'") = False Then
                    id = tempID
                    Exit For
                End If
            Next

            Return id
        Catch ex As Exception
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        End Try
    End Function

    Public Function GenRecIDMaternity(ByVal getGen As String)
        Try


            Dim id As String = ""
            For x As Integer = getGen To 999999
                Dim tempID As String = x
                If checkExistence("select * from SSTRANSINFORTERMMN where transactionIDmaternity='" & tempID & "'") = False Then
                    id = tempID
                    Exit For
                End If
            Next
            Return id
        Catch ex As Exception
            'Dim errorLogs As String = ex.tostring
            'errorLogs = errorLogs.Trim
            'sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
            '    & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
            'ExecuteSQLQuery(sql)
            'MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
        End Try
    End Function

    'Public Function GenFeedback(ByVal getGen As String)
    '    Try

    '        Dim id As String = ""
    '        For x As Integer = getGen To 999999
    '            Dim tempID As String = x
    '            If checkExistence("select * from tbl_Kiosk_feedback where autogenIDfeedback='" & tempID & "'") = False Then
    '                id = tempID
    '                Exit For
    '            End If
    '        Next
    '        Return id
    '    Catch ex As Exception
    '        Dim errorLogs As String = ex.tostring
    '        errorLogs = errorLogs.Trim
    '        sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
    '            & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
    '        ExecuteSQLQuery(sql)
    '        MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
    '    End Try
    'End Function

    'Public Function GenRegistration(ByVal getGen As String)
    '    Try

    '        Dim id As String = ""
    '        For x As Integer = getGen To 999999
    '            Dim tempID As String = x
    '            If checkExistence("select * from tbl_Kiosk_feedback where autogenIDregistration='" & tempID & "'") = False Then
    '                id = tempID
    '                Exit For
    '            End If
    '        Next
    '        Return id
    '    Catch ex As Exception
    '        Dim errorLogs As String = ex.tostring
    '        errorLogs = errorLogs.Trim
    '        sql = "insert into SSINFOTERMERRLOGS values('" & My.Settings.kioskIP & "', '" & My.Settings.kioskID & "', '" & My.Settings.kioskName & "', '" & My.Settings.kioskBranch & "', '" & errorLogs _
    '            & "','" & "Class: Connection String" & "', '" & "Database connection error" & "', '" & Date.Today.ToShortDateString & "', '" & TimeOfDay & "') "
    '        ExecuteSQLQuery(sql)
    '        MsgBox("Database connection error, Please contact system Administrator! ", MsgBoxStyle.Information)
    '    End Try
    'End Function


    Public Sub AuditTrail(ByVal user_ID As String, ByVal username As String, ByVal _module As String, ByVal task As String, ByVal affected_Table As String, ByVal status As String, ByVal transaction_Date As String, ByVal transaction_Time As String)
        ExecuteSQLQuery("Insert into tbl_Audit_Trail(User_ID, User_Name, Module, Task, Affected_Table, Status, Transaction_Date, Transaction_Time) values('" & user_ID & "', '" & username & "', '" & _module & "', '" & task & "', '" & affected_Table & "', '" & status & "', '" & transaction_Date & "', '" & transaction_Time & "')")
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region




End Class
