Imports System
Imports System.Xml
Imports System.Configuration
Imports System.Reflection
Imports System.IO
Imports System.Xml.XPath


Public Class Class1
    Public xml_Filename, value_path, value_name As String
    Public Shared settings As New XmlWriterSettings()
    ' value_name  =  value name of the settings.
    ' value_path =   node path of the value.
    'xml_filename  =  the filename of the settings.
  
    Public Shared Function readSetting(ByVal key As String) As String
        '  Return ConfigurationSettings.AppSettings(key)
    End Function


    Public Sub writeSettings(ByVal xml_Filename As String)
        ' Dim doc As XmlDocument = loadconfig
        Try
            If Not File.Exists(xml_Filename) Then
                Dim XmlWrt As XmlWriter = XmlWriter.Create(xml_Filename, settings)


                With XmlWrt
                    .WriteStartDocument()

                    .WriteComment("XML Settings.")
                    .WriteStartElement("Configuration")

                    .WriteStartElement("Settings")


                    .WriteStartElement("db_DSN")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("db_Server")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("db_Name")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("db_UName")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("db_Pass")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("kioskBranch")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("transTag")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("FirstRun")
                    .WriteString("0")
                    .WriteEndElement()

                    .WriteStartElement("video_Path")
                    .WriteString("Video/SSS.mp4")
                    .WriteEndElement()

                    .WriteStartElement("charter_Path")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("SSIT_Path")
                    .WriteString("SSIT_SERVER/SSIT_SERVER.exe")
                    .WriteEndElement()

                    .WriteStartElement("setting_Path")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("ftpUserID")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("ftpPassword")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("downloadedPath")
                    .WriteString("C:\Program Files\SSIT")
                    .WriteEndElement()

                    .WriteStartElement("ftpIP")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("terms_Path")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("ftpUserID")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("ftpPassword")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("downloadedPath")
                    .WriteString("C:\Program Files\SSIT")
                    .WriteEndElement()

                    .WriteStartElement("ftpIP")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("terms_Path")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("SSIT_origPath")
                    .WriteString("C:\Program Files\SSIT\SSIT_SERVER.exe")
                    .WriteEndElement()

                    .WriteStartElement("sett")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("ftp_Path")
                    .WriteString("Application/SSIT_SERVER.exe")
                    .WriteEndElement()

                    .WriteStartElement("Settings_lastUpdated")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("Video_lastUpdated")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("Terms_lastUpdated")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("Application_lastUpdated")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("Citizen_lastUpdated")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("Updater_LastUpdated")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("Updater_FilePath")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("Updater_ftpPath")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("folderLastLogs")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("runConfig")
                    .WriteString("0")
                    .WriteEndElement()

                    .WriteStartElement("terminalIP")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("firstConfig")
                    .WriteString("0")
                    .WriteEndElement()

                    .WriteEndElement()
                    .WriteEndElement()
                    .WriteEndDocument()
                    .Close()
                End With
            End If
        Catch ex As Exception

        End Try
    End Sub


    Public Function editSettings(ByVal xml_Filename As String, ByVal xml_path As String, ByVal value_name As String, ByVal value1 As String)
        Try
            'GC.Collect()
            Dim xd As New XmlDocument()

            xd.Load(xml_Filename)

            Dim nod As XmlNode = xd.SelectSingleNode(xml_path & "/" & value_name)  '"Configuration/Settings/FirstName"
            If nod IsNot Nothing Then
                nod.InnerXml = value1
            Else
                nod.InnerXml = value1
            End If

            xd.Save(xml_Filename)


        Catch ex As Exception

        End Try
       
    End Function

    Public Function readSettings(ByVal xml_filame As String, ByVal xml_path As String, ByVal value_name As String) As String
        Dim return_value As String
        Try
            Dim a As String
            Dim xd As New XmlDocument
            xd.Load(xml_filame)
            Dim nod As XmlNode = xd.SelectSingleNode(xml_path & "/" & value_name)
            If nod IsNot Nothing Then
                a = nod.InnerXml
                return_value = a
            Else
                return_value = Nothing
            End If
            xd.Save(xml_filame)
        Catch ex As Exception
            return_value = ex.Message
        End Try
        Return return_value

    End Function

End Class
