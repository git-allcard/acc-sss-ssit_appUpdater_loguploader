Imports System.Net
Imports System.IO

Public Class _frmFTPSettings
    Public modTag As Integer
    Dim isConnected As Boolean
    Dim xs As New Class1
    ReadOnly ValidCharsNum As String = _
"0123456789. "
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
                checkCredentials()

                'MsgBox(isConnected)
                If isConnected = True Then
                    Me.Hide()
                    _frmUpdateFile.Show()
                Else
                    ' MsgBox(isConnected)
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "SSIT FTP SETTINGS")
        End Try
       

    End Sub

    Private Function checkCredentials()

        Try

            Dim request = _
    DirectCast(WebRequest.Create _
    ("ftp://" + txtIP.Text + "/"), FtpWebRequest)

            request.Credentials = _
            New NetworkCredential(txtUsername.Text, txtPassword.Text)
            request.Method = WebRequestMethods.Ftp.ListDirectory

            Dim response As FtpWebResponse = _
            DirectCast(request.GetResponse(), FtpWebResponse)
            ' Folder exists here

            ' If response.StatusCode = FtpStatusCode.OpeningData Then
            MsgBox("Connected!", vbInformation, "Information")

            xs.editSettings(xs.xml_Filename, xs.value_path, "ftpIP", txtIP.Text)
            xs.editSettings(xs.xml_Filename, xs.value_path, "ftpUserID", txtUsername.Text)
            xs.editSettings(xs.xml_Filename, xs.value_path, "ftpPassword", txtPassword.Text)
            xs.editSettings(xs.xml_Filename, xs.value_path, "terminalIP", txtTerminalIP.Text)
            xs.editSettings(xs.xml_Filename, xs.value_path, "runConfig", "1")
            isConnected = True
            '  End If

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
            xs.xml_Filename = Application.StartupPath & "\Mysettings.xml" '"Mysettings.xml"
            xs.value_path = "Configuration/Settings"
            xs.writeSettings(xs.xml_Filename)
            Dim runConfig As String = xs.readSettings(xs.xml_Filename, xs.value_path, "runConfig")

            If runConfig = 1 Then
                _frmUpdateFile.Show()
                Me.Hide()
            Else

            End If
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

End Class