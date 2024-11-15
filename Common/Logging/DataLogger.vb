Imports System.IO
Imports System.Configuration.ConfigurationManager
Imports KeihiWeb.Common
Imports KeihiWeb.Common.uty

Namespace Common.Logging

    Public Class DataLogger

        ''' <summary>
        ''' データのログを記録する
        ''' </summary>
        ''' <param name="op">操作名、メソッド名など任意で設定する</param>
        ''' <param name="dt">データテーブル</param>
        ''' <param name="PrintCaption">
        ''' １行目に項目名を出力するかしないかを設定する（省略時は出力しない）<br />
        ''' True：項目名を出力する<br />
        ''' False：項目名を出力しない<br />
        ''' </param>
        ''' <remarks></remarks>
        Public Shared Sub DataLogWriter(ByVal op As String, ByVal dt As DataTable, Optional ByVal PrintCaption As Boolean = True)
            Dim sw2 As String = AppSettings("LogSwitch")
            If sw2.ToLower = "on" Then
                Logging.Logger.WriteLog(op)
            End If
            Dim sw As String = AppSettings("DataLogSwitch")
            If sw.ToLower = "on" Then
                Dim LogDirectory As String = AppSettings("DataLogFolder")
                Dim LogFileName As String = LogDirectory & "\BILLINGSYS_DATA_" & Format(Now, "yyyyMMdd") & "_log.log"

                If Not Directory.Exists(LogDirectory) Then
                    Directory.CreateDirectory(LogDirectory)
                End If
                If dt.Rows.Count <> 0 Then
                    dt.Rows(0).Item("D_REMARKS") = op
                End If
                Uty_TextFile.TextWriter(dt, LogFileName, PrintCaption, True)
            End If
        End Sub

    End Class
End Namespace
