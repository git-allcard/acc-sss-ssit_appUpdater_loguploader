Imports System.Threading
Public Class _frmUpdateUpdater
    Dim up As New Update
    Dim trd As Thread
    Dim updater_exe_path, updates_path As String
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Control.CheckForIllegalCrossThreadCalls = False
        'Try
        '    updater_exe_path = Class1.appRepo & "\SSIT_UPDATER\SSIT_UPDATER.exe"
        '    updates_path = Class1.appRepo & "\SSIT_UPDATER\Updates\SSIT_UPDATER.exe"

        '    System.Threading.Thread.Sleep(8000)
        '    trd = New Thread(AddressOf threadTask)
        '    trd.IsBackground = True
        '    trd.Start()

        'Catch ex As Exception
        '    MsgBox(ex.Message, MsgBoxStyle.Information, "DELETE_UPDATER")
        'End Try


    End Sub

    Private Sub threadTask()
        Try

            up.update_Updater(updater_exe_path, updates_path)
            ' System.Threading.Thread.Sleep(5000)
            ' up.openUPDATER()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "DELETE_UPDATER")
        End Try
    End Sub
End Class
