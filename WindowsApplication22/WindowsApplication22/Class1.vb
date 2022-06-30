
Imports System.Xml
Imports System.IO


Public Class Class1

    Public Shared appRepo As String = "C:\Program Files (x86)\SSIT"

    Public xml_Filename, value_path, value_name As String
    Public Shared settings As New XmlWriterSettings()
    ' value_name  =  value name of the settings.
    ' value_path =   node path of the value.
    'xml_filename  =  the filename of the settings.

    'Public Shared Function readSetting(ByVal key As String) As String
    '    '  Return ConfigurationSettings.AppSettings(key)
    'End Function


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



                    .WriteStartElement("ftpIP")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("terms_Path")
                    .WriteString("")
                    .WriteEndElement()

                    '.WriteStartElement("downloadedPath")
                    '.WriteString(Class1.appRepo & "")
                    '.WriteEndElement()

                    '.WriteStartElement("ftpUserID")
                    '.WriteString("")
                    '.WriteEndElement()

                    '.WriteStartElement("ftpPassword")
                    '.WriteString("")
                    '.WriteEndElement()

                    '.WriteStartElement("ftpIP")
                    '.WriteString("")
                    '.WriteEndElement()


                    .WriteStartElement("downloadedPath")
                    '.WriteString(Class1.appRepo & "")
                    .WriteString(Class1.appRepo)
                    .WriteEndElement()


                    .WriteStartElement("terms_Path")
                    .WriteString("")
                    .WriteEndElement()

                    .WriteStartElement("SSIT_origPath")
                    '.WriteString(Class1.appRepo & "\SSIT_SERVER.exe")
                    .WriteString(Class1.appRepo & "\SSIT_SERVER.exe")
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

                    .WriteStartElement("deleteFile")
                    .WriteString("0")
                    .WriteEndElement()

                    .WriteStartElement("firstConfig")
                    .WriteString("0")
                    .WriteEndElement()

                    .WriteEndElement()
                    .WriteEndElement()
                    .WriteEndDocument()
                    .Flush()
                    .Close()
                End With
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Public Function editSettings(ByVal xml_Filename As String, ByVal xml_path As String, ByVal value_name As String, ByVal value1 As String)
        ' Try
        'GC.Collect()
        Dim xd As New XmlDocument()

        xd.Load(xml_Filename)
        Dim nod As XmlNode


        Try
            nod = xd.SelectSingleNode(xml_path & "/" & value_name)  '"Configuration/Settings/FirstName"
        Catch ex As Exception
            Dim pathA As String = Application.StartupPath & "\Updates\MySettings.xml"  '  source filename
            Dim pathB As String = Application.StartupPath & "\MySettings.xml" '  destination file name
            If File.Exists(pathB) Then
                System.IO.File.Delete(pathB)
                System.IO.File.Copy(pathA, pathB)
            End If
        End Try

        nod = xd.SelectSingleNode(xml_path & "/" & value_name)  '"Configuration/Settings/FirstName"
        If nod IsNot Nothing Then
            nod.InnerXml = value1
        Else
            nod.InnerXml = value1
        End If

        xd.Save(xml_Filename)



        'Catch ex As Exception
        '    'Dim pathA As String = Application.StartupPath & "\Updates\MySettings.xml"  '  source filename
        '    'Dim pathB As String = Application.StartupPath & "\MySettings.xml" '  destination file name
        '    'If File.Exists(pathB) Then
        '    '    System.IO.File.Delete(pathB)
        '    '    System.IO.File.Copy(pathA, pathB)
        '    'End If

        'End Try


    End Function

    Public Function readSettings(ByVal xml_filame As String, ByVal xml_path As String, ByVal value_name As String) As String
        Dim return_value As String

        Dim a As String
        Dim xd As New XmlDocument
        Dim inText As String

        'inText = xd.
        Dim nod As XmlNode
        xd.Load(xml_filame)
        Try
            nod = xd.SelectSingleNode(xml_path & "/" & value_name)
        Catch ex As Exception

            ' return_value = ex.Message

            Dim pathA As String = Application.StartupPath & "\Updates\MySettings.xml"  '  source filename
            Dim pathB As String = Application.StartupPath & "\MySettings.xml" '  destination file name
            If File.Exists(pathB) Then
                System.IO.File.Delete(pathB)
                System.IO.File.Copy(pathA, pathB)
            End If

        End Try
        nod = xd.SelectSingleNode(xml_path & "/" & value_name)
        If nod IsNot Nothing Then
            a = nod.InnerXml
            return_value = a
        Else
            return_value = Nothing
        End If


        xd.Save(xml_filame)


        Return return_value

    End Function

    Public Function CheckSettings(ByVal xml_filame As String, ByVal xml_path As String, ByVal value_name As String) As String
        Dim return_value As String

        Dim a As String
        Dim xd As New XmlDocument


        'inText = xd.
        Try
            xd.Load(xml_filame)
        Catch ex As Exception

            return_value = "1" 'ex.Message

            ' MsgBox(ex.Message, MsgBoxStyle.Information, "READ SETTINGS")
            Dim pathA As String = Application.StartupPath & "\Updates\MySettings.xml"  '  source filename
            Dim pathB As String = Application.StartupPath & "\MySettings.xml" '  destination file name
            If File.Exists(pathB) Then
                System.IO.File.Delete(pathB)
                System.IO.File.Copy(pathA, pathB)
            End If

        End Try



    End Function



    Public Sub backUpSettings()
        Try
            Dim pathA As String = Application.StartupPath & "\MySettings.xml" '  source filename
            Dim pathB As String = Application.StartupPath & "\Updates\MySettings.xml" '  destination file name
            ' If Not File.Exists(pathB) Then
            System.IO.File.Copy(pathA, pathB)
            ' End If

        Catch ex As Exception

        End Try

    End Sub


End Class
