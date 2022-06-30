Module modFTP
    Public fileName As String '= My.Settings.SSIT_Path
    Public filePath As String ' = My.Settings.downloadedPath
    Public ftpPath As String
    Public NotInheritable Class FTPSettings
        Private Sub New()
        End Sub
        Public Shared Property IP() As String
            Get
                Return m_IP
            End Get
            Set(ByVal value As String)
                m_IP = value
            End Set
        End Property
        Private Shared m_IP As String
        Public Shared Property UserID() As String
            Get
                Return m_UserID
            End Get
            Set(ByVal value As String)
                m_UserID = value
            End Set
        End Property
        Private Shared m_UserID As String
        Public Shared Property Password() As String
            Get
                Return m_Password
            End Get
            Set(ByVal value As String)
                m_Password = value
            End Set
        End Property
        Private Shared m_Password As String
    End Class
End Module
