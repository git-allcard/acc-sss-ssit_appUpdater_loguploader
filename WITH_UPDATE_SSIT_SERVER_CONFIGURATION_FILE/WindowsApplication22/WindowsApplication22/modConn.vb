
Imports System
Imports System.Xml
Imports System.Configuration
Imports System.Reflection
Imports System.IO
Imports System.Xml.XPath
'Imports Oracle.DataAccess.Client

Imports System.ComponentModel
Imports System.Text

Imports System.Threading
Imports System.Data.Odbc
Imports Microsoft.Win32

Imports System.AssemblyLoadEventArgs
Imports System.Web.Services
Module modConn

    Dim assembly__1 = Assembly.GetExecutingAssembly()
    Dim resourceName = assembly__1.getname.name & "." & "SSIT_SERVER.exe.config" '"SampleText.txt" ' "MyCompany.MyProduct.MyFile.txt" SAMPLE\Sample101

    Public db_DSN, db_SERVER, db_Name, db_Uname, db_Pass As String
    Public kioskBranch As String


    Public Sub UpdateConfigFile()
        Try
            Dim result As String
            Using stream As Stream = assembly__1.GetManifestResourceStream(resourceName)
                Using reader As New StreamReader(stream)
                    result = reader.ReadToEnd()
                    'MsgBox(result)
                End Using
            End Using

            testXML(result)
        Catch ex As Exception

        End Try

    End Sub
    Private Function testXML(ByVal result As String)
        Try
            Dim doc As New XmlDocument()
            doc.LoadXml(result)
            File.Delete("C:\Program Files\SSIT\SSIT_SERVER") '(Application.StartupPath & "\SSIT_SERVER.exe.config")
            doc.Save("SSIT_SERVER.exe.config")
            '  MsgBox("Done Saving!")
        Catch ex As Exception

        End Try

    End Function


    Private Function checkNode()

        Dim xmlNode As String = "system.serviceModel"
        Dim mDoc As New XmlDocument()

        '-- Load the actual xml file
        mDoc.Load("C:\Program Files\SSIT\SSIT_SERVER")

        '-- See if the node exist - input a name in the textbox to     search.
        If mDoc.GetElementsByTagName(xmlNode).Count > 0 Then
            MessageBox.Show(xmlNode & " node does exist.", "Node Info...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            '-- XML Node doesn't exist
            MessageBox.Show(xmlNode & " node does not exist.", "Node Info...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Function
End Module
