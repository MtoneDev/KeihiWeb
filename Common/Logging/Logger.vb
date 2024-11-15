Imports System
Imports System.IO
Imports System.Web

Imports KeihiWeb.Common
Imports KeihiWeb.Common.uty

Namespace Common.Logging

    ''' <summary>
    ''' ログ出力クラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Logger

        ''' <summary>
        ''' CSVファイルにログを出力する
        ''' </summary>
        ''' <param name="loginfo">ログ情報</param>
        ''' <remarks></remarks>
        Public Shared Sub CSVLogWriter(ByVal loginfo As LogInfoVal)

            If loginfo.LogKind = "" Then
                Exit Sub
            End If

            Dim LogDirectory As String = Uty_Config.LogFolder
            Dim LogFileName As String = LogDirectory & "\" & Uty_Config.Logname & Format(Now, "yyyyMMdd") & "_log.log"

            If Not Directory.Exists(LogDirectory) Then
                Directory.CreateDirectory(LogDirectory)
            End If
            Try
                Using LogF As New StreamWriter(LogFileName, True, Encoding.GetEncoding("shift-jis"))
                    Try
                        LogF.AutoFlush = True
                        LogF.WriteLine(loginfo.ToCSV)
                    Finally
                        LogF.Close()
                    End Try
                End Using
            Catch ex As Exception

            End Try

        End Sub

        ''' <summary>
        ''' ログ情報を取得する
        ''' </summary>
        ''' <param name="kind">ログ種別：HttpApplication オブジェクトから発生するイベント名</param>
        ''' <returns>ログ情報をLogData型で返す</returns>
        ''' <remarks></remarks>
        Public Shared Function GetLoginfo(ByVal kind As String) As LogInfoVal
            'HttpContextを取得する
            Dim context As HttpContext = HttpContext.Current
            Dim loginfo As New LogInfoVal()

            Select Case Mid(context.Request.AppRelativeCurrentExecutionFilePath, InStrRev(context.Request.AppRelativeCurrentExecutionFilePath, ".") + 1)
                Case "htc", "js", "css", "gif", "jpg", "jpeg", "axd"
                    '上記の拡張子はログを出力しない
                Case Else
                    'ログ情報を設定する
                    loginfo.SystemID = Constants.SystemName
                    loginfo.LogKind = kind
                    If HttpContext.Current.Session Is Nothing Then
                        loginfo.SessionId = ""
                    Else
                        loginfo.SessionId = HttpContext.Current.Session.SessionID
                    End If
                    loginfo.HttpMethod = context.Request.HttpMethod
                    loginfo.RequestFilePath = context.Request.AppRelativeCurrentExecutionFilePath
                    loginfo.UserHostAddress = context.Request.UserHostAddress
                    loginfo.UserHostName = context.Request.UserHostName
                    If context.Request.IsAuthenticated Then
                        loginfo.LogonUserID = context.Request.LogonUserIdentity.Name
                    Else
                        loginfo.LogonUserID = String.Empty
                    End If
                    loginfo.LoginID = ""
                    'If kind = "PreRequestHandlerExecute" Or kind = "Error" Then
                    If Not IsNothing(context.Session) Then
                        If Not IsNothing(context.Session("AuthInfo")) Then
                            ' Dim authDt As AuthDs.AuthInfoDataTable = context.Session("AuthInfo")
                            ' loginfo.LoginID = authDt(0).TAN_CODE
                        End If
                    End If
                    'End If
            End Select

            Return loginfo

        End Function

        ''' <summary>
        ''' エラーログの情報を取得する
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetErrLogInfo() As LogInfoVal
            'HttpContextを取得する
            Dim context As HttpContext = HttpContext.Current
            Dim err As New System.Exception
            Dim loginfo As New LogInfoVal
            'ログ情報を設定する
            loginfo = Logging.Logger.GetLoginfo("Error")
            err = context.Server.GetLastError()
            loginfo.MessageString = err.Message
            loginfo.MethodName = err.TargetSite.Name
            loginfo.StackTrace = err.StackTrace

            Return loginfo
        End Function

        ''' <summary>
        ''' エラーログを記録する
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub WriteErrLog(ByVal ex As System.Exception, ShainCode As String, KonyuDate As String, DenpyoNo As String)
            Dim logswitch As String = Uty_Config.LogSwitch
            If logswitch = "on" Then
                Dim loginfo As New LogInfoVal
                loginfo = Logging.Logger.GetLoginfo("Error")
                loginfo.MessageString = ex.Message.ToString()
                If IsNothing(ex.TargetSite) Then
                    loginfo.MethodName = String.Empty
                Else
                    loginfo.MethodName = ex.TargetSite.Name
                End If
                loginfo.StackTrace = ex.StackTrace

                loginfo.Code = "担当者ｺｰﾄﾞ:" & ShainCode
                loginfo.KonyuDate = "購入日:" & KonyuDate
                loginfo.DenpyoNo = "伝票番号:" & DenpyoNo

                Logging.Logger.CSVLogWriter(loginfo)
            End If
        End Sub

        ''' <summary>
        ''' ログを記録する
        ''' </summary>
        ''' <param name="kind">種別（任意）</param>
        ''' <remarks></remarks>
        Public Shared Sub WriteLog(ByVal kind As String)
            ' 各要求の開始時に呼び出されます。
            Dim logswitch As String = Uty_Config.LogSwitch
            If logswitch.ToLower = "on" Then
                'ログ情報を設定する
                Dim loginfo As Logging.LogInfoVal = Logging.Logger.GetLoginfo(kind)
                'ログをファイルに書き出す
                Logging.Logger.CSVLogWriter(loginfo)
            End If
        End Sub

        ''' <summary>
        ''' ログを記録する
        ''' </summary>
        ''' <param name="kind">ログ種別</param>
        ''' <param name="msg">メッセージ</param>
        ''' <remarks></remarks>
        Public Shared Sub WriteLog(ByVal kind As String, ByVal msg As String)
            ' 各要求の開始時に呼び出されます。
            Dim logswitch As String = Uty_Config.LogSwitch
            If logswitch.ToLower = "on" Then
                'ログ情報を設定する

                Dim loginfo As Logging.LogInfoVal = Logging.Logger.GetLoginfo(kind)
                loginfo.MessageString = msg
                'ログをファイルに書き出す
                Logging.Logger.CSVLogWriter(loginfo)
            End If
        End Sub

    End Class
End Namespace
