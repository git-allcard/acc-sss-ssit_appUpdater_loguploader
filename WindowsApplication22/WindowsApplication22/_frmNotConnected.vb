Imports System.Net.NetworkInformation
Public Class _frmNotConnected

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Try
            If NetworkInterface.GetIsNetworkAvailable() = False Then
                MsgBox("Unable to Connect.", MsgBoxStyle.Information, "SSIT_SERVER")
             
            ElseIf NetworkInterface.GetIsNetworkAvailable() = True Then
                _frmFTPSettings.loadUpdater()
            End If

        Catch ex As Exception

        End Try

    End Sub
End Class