Imports System.Data.Common
Imports System.Data.SqlClient

Imports KeihiWeb.Common.Logging
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Enty

Namespace Ctrl
    Public Enum UserLevel
        ''' <summary>
        ''' 一般権限
        ''' </summary>
        Normal = 1

        ''' <summary>
        ''' 経理担当
        ''' </summary>
        Keiri = 5

        ''' <summary>
        ''' システム管理者
        ''' </summary>
        Admin = 9
    End Enum

    Public Class CTL_AUTH
        ''' <summary>
        ''' ログイン認証する
        ''' </summary>
        ''' <param name="vId">社員番号</param>
        ''' <param name="vPass">パスワード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Login(ByVal vId As String, ByVal vPass As String) As DS_AUTH.AUTH_INFODataTable
            If vId.Trim = "" Or Nothing Is vId Then
                Throw New Exception("ログインIDを入力してください。")
            ElseIf vPass.Trim = "" Or Nothing Is vPass Then
                Throw New Exception("パスワードを入力してください。")
            End If

            Dim lDt As New DS_AUTH.AUTH_INFODataTable
            Dim lEty As New Ety_AUTH
            Try
                lDt = lEty.GetAuthInfo(vId, vPass)
            Catch ex As Exception
                Logger.WriteErrLog(ex, "", "", "")
                Throw New Exception("データの取得に失敗しました。（" & ex.Message & "）", ex)
            End Try

            If lDt.Count = 0 Then
                Throw New Exception("ログインIDまたはパスワードを正しく入力してください。")
            End If

            '管理者がログインした場合に古いログファイルを削除する
            Dim logswitch As String = Uty_Config.LogSwitch
            If lDt(0).USER_LEVEL = UserLevel.Admin And logswitch <> "off" Then
                Dim ext As String = ".log"
                Dim logfolder As String = Uty_Config.LogFolder
                Dim savaterm As Integer = Integer.Parse(Uty_Config.LogSaveTerm)
                Try
                    ' Uty_TextFile.DeleteTemporary(logfolder, ext, savaterm)
                Catch ex As Exception
                    Logger.WriteErrLog(ex, "", "", "")
                End Try
            End If

            HttpContext.Current.Session("user_id") = lDt(0).USER_ID
            HttpContext.Current.Session("user_name") = lDt(0).USER_NAME
            HttpContext.Current.Session("user_kana") = lDt(0).USER_KANA
            HttpContext.Current.Session("user_level") = lDt(0).USER_LEVEL
            HttpContext.Current.Session("bumon_cd") = lDt(0).BUMON_CD
            HttpContext.Current.Session("bumon_name") = lDt(0).BUMON_NAME
            Return lDt
        End Function

    End Class

End Namespace
