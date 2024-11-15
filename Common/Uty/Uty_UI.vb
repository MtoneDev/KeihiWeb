Namespace Common.uty
    Public Class Uty_UI
        ''' <summary>
        ''' タブ識別IDを取得する
        ''' </summary>
        ''' <param name="sender">画面オブジェクト</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetTabId(ByVal sender As Object) As String
            Return sender.GetType.ToString & HttpContext.Current.Session.SessionID & Environment.TickCount.ToString
        End Function

        ''' <summary>
        ''' メッセージを表示する
        ''' </summary>
        ''' <param name="vStatusLabel">ラベルオブジェクト</param>
        ''' <param name="vMsgMode">メッセージモード</param>
        ''' <param name="vMessageText">メッセージ文字列</param>
        ''' <remarks></remarks>
        Public Shared Sub ShowMessage(ByRef vStatusLabel As System.Web.UI.WebControls.Label,
                                       ByVal vMsgMode As Integer, ByVal vMessageText As String)
            vStatusLabel.Text = vMessageText
            Select Case vMsgMode
                Case Constants.MessageMode.Normal
                    vStatusLabel.Text = vMessageText
                    vStatusLabel.CssClass = "normalForeColor"
                Case Constants.MessageMode.Err
                    vStatusLabel.Text = vMessageText
                    vStatusLabel.CssClass = "errorForeColor"
                Case Constants.MessageMode.Warn
                    vStatusLabel.Text = vMessageText
                    vStatusLabel.CssClass = "warnForeColor"
                Case Else
                    vStatusLabel.Text = vMessageText
                    vStatusLabel.CssClass = "normalForeColor"
            End Select
        End Sub

        Public Shared Sub DownloadFile(vPath As String)
            If vPath <> "" Then
                Dim lTargetFile As String = vPath.Substring(vPath.LastIndexOf("\") + 1)
                Dim lDownloadFileName As String = vPath

                HttpContext.Current.Response.Redirect("FileDL.aspx?FILENAME=" & lTargetFile & "&DLNAME=" & HttpContext.Current.Server.UrlEncode(lDownloadFileName))
                'Server.Transfer("Common/FileDL.aspx?FILENAME=" & lTargetFileName & "&DLNAME=" & lDownloadFileName)
            End If
        End Sub

    End Class
End Namespace
