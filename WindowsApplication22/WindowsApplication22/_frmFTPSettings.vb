Imports System.Net
Imports System.IO
Imports System.Net.NetworkInformation
'Imports System.Xml
'Imports System.Xml.Xsl
Public Class _frmFTPSettings
    Public modTag As Integer
    Dim isConnected As Boolean
    Dim xs As New Class1
    ReadOnly ValidCharsNum As String = _
"0123456789. "
    Dim ifConfig As Integer

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If txtIP.Text = "" Then
                MsgBox("IP address is required. ")
            ElseIf txtPassword.Text = "" Then
                MsgBox("FTP Password is required. ")
            ElseIf txtUsername.Text = "" Then
                MsgBox("FTP Username is required. ")
            ElseIf txtTerminalIP.Text = "" Then
                MsgBox("Terminal IP address is required. ")
            Else
                checkCredentials(txtTerminalIP.Text, txtUsername.Text, txtPassword.Text, txtIP.Text, 0)
                Dim pathA As String = Application.StartupPath & "\MySettings.xml" '  source filename
                Dim pathB As String = Application.StartupPath & "\Updates\MySettings.xml" '  destination file name
                If Not File.Exists(pathB) Then
                    System.IO.File.Copy(pathA, pathB)
                End If



                If isConnected = True Then
                    Me.Hide()
                    _frmUpdateFile.Show()
                Else
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "SSIT FTP SETTINGS")
        End Try


    End Sub

    Private Function checkCredentials(ByVal terminalIP As String, ByVal Username As String, ByVal Password As String, ByVal ftpIP As String, ByVal ifConfig As Integer)

        Try

            Dim request = _
    DirectCast(WebRequest.Create _
    ("ftp://" + ftpIP + "/"), FtpWebRequest)

            request.Credentials = _
            New NetworkCredential(Username, Password)
            request.Method = WebRequestMethods.Ftp.ListDirectory

            Dim response As FtpWebResponse = _
            DirectCast(request.GetResponse(), FtpWebResponse)
            ' Folder exists here

            ' If response.StatusCode = FtpStatusCode.OpeningData Then
            If ifConfig = 0 Then
                MsgBox("Connected!", vbInformation, "Information")
                xs.editSettings(xs.xml_Filename, xs.value_path, "ftpIP", ftpIP)
                xs.backUpSettings()
                xs.editSettings(xs.xml_Filename, xs.value_path, "ftpUserID", Username)
                xs.backUpSettings()
                xs.editSettings(xs.xml_Filename, xs.value_path, "ftpPassword", Password)
                xs.backUpSettings()
                xs.editSettings(xs.xml_Filename, xs.value_path, "terminalIP", terminalIP)
                xs.backUpSettings()
                xs.editSettings(xs.xml_Filename, xs.value_path, "runConfig", "1")
                xs.backUpSettings()
            Else
                ' do nothing
            End If
          
            isConnected = True


        Catch ex As WebException
            Dim response As FtpWebResponse = _
            DirectCast(ex.Response, FtpWebResponse)
            'Does not exist
            If response.StatusCode = _
            FtpStatusCode.ActionNotTakenFileUnavailable Then
                MsgBox("Unable to connect. Please check your parameters.", vbInformation, "Information")
            ElseIf response.StatusCode = FtpStatusCode.ServiceTemporarilyNotAvailable Then
                MsgBox("Can't connect to FTP.", vbInformation, "Information")
            ElseIf response.StatusCode = FtpStatusCode.SendPasswordCommand Then
                MsgBox("Incorrect Password.", vbInformation, "Information")
            ElseIf response.StatusCode = FtpStatusCode.NotLoggedIn Then
                MsgBox("Unable to connect. Please check your parameters.", vbInformation, "Information")
            ElseIf response.StatusCode = FtpStatusCode.ServiceNotAvailable Then
                MsgBox("Can't connect to FTP.", vbInformation, "Information")
            Else
                ' MsgBox("Invalid Permission.", vbInformation, "Information")
                MsgBox(ex.Message, vbOKOnly, "SSIT FTP SETTINGS")
                Application.Exit()
            End If
            isConnected = False

        End Try

        Return isConnected

    End Function

    Private Sub _frmFTPSettings_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub
    Private Sub _frmFTPSettings_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'My.Settings.runConfig = 0
        'Me.modTag = 1
        Try
            Me.Hide()
            Me.WindowState = FormWindowState.Minimized
            Timer1.Start()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "SSIT FTP SETTINGS")



        End Try
       
    End Sub

    
    Private Sub txtTerminalIP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTerminalIP.KeyPress
        e.Handled = Not (ValidCharsNum.IndexOf(e.KeyChar) > -1 _
    OrElse e.KeyChar = Convert.ToChar(Keys.Back))
    End Sub

    Private Sub txtIP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtIP.KeyPress
        e.Handled = Not (ValidCharsNum.IndexOf(e.KeyChar) > -1 _
          OrElse e.KeyChar = Convert.ToChar(Keys.Back))
    End Sub

    Public Sub loadUpdater()
        

            xs.xml_Filename = Application.StartupPath & "\Mysettings.xml" '"Mysettings.xml"
            xs.value_path = "Configuration/Settings"
            xs.writeSettings(xs.xml_Filename)
            'xs.editSettings(xs.xml_Filename, xs.value_path, "ftpIP", txtIP.Text)
            Dim statusSettings = xs.CheckSettings(xs.xml_Filename, xs.value_path, "runConfig")

            Dim runConfig As String = xs.readSettings(xs.xml_Filename, xs.value_path, "runConfig")
            If runConfig = 1 Then
                Dim ftpIP As String = xs.readSettings(xs.xml_Filename, xs.value_path, "ftpIP")
                Dim userName As String = xs.readSettings(xs.xml_Filename, xs.value_path, "ftpUserID")
                Dim ftpPassword As String = xs.readSettings(xs.xml_Filename, xs.value_path, "ftpPassword")
                Dim terminalIp As String = xs.readSettings(xs.xml_Filename, xs.value_path, "terminalIP")

            ' If checkCredentials(terminalIp, userName, ftpPassword, ftpIP, 1) Then

            '  If isConnected = True Then
            _frmUpdateFile.Show()
            Me.Hide()
            'End If

            'End If
        Else
            Me.WindowState = FormWindowState.Normal
            ' Me.Show()
        End If


    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Interval = Timer1.Interval + 1000
        If Timer1.Interval = 3000 Then
            If NetworkInterface.GetIsNetworkAvailable() = False Then
                'MsgBox("The system is not connected to the network. Please make sure the network cable is connected.", MsgBoxStyle.Information, "SSIT_SERVER")

                _frmNotConnected.Visible = True
                Me.Hide()
            ElseIf NetworkInterface.GetIsNetworkAvailable() = True Then
                loadUpdater()
            End If
            Timer1.Stop()
        End If
    End Sub
End Class