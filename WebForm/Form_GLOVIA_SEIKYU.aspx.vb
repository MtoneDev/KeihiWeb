Imports System.Web.Services

Imports KeihiWeb.Common
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Common.Logging

Public Class Form_GLOVIA_SEIKYU
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim lJibumonCd As String = ""
            If IsNothing(Session("auth_user_id")) Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lJibumonCd = Session("auth_bumon_cd")
            End If

            If Not IsPostBack Then
                '画面初期化
                SetInit(lJibumonCd, Now())

                '部門情報表示
                DispData()
            End If
        Catch ex As Exception
            Logger.WriteErrLog(ex, Session("auth_user_id"), "", "")
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 部門情報を表示する
    ''' </summary>
    Private Sub DispData()
        Dim lDt As New M_BUMON.M_BUMON_SHIME_INFODataTable
        lDt = CTL_M_BUMON.GetShimeInfo(txtShimeDate.Text, 2)

        ListView1.DataSource = lDt
        ListView1.DataBind()
    End Sub

    ''' <summary>
    ''' 画面を初期化する
    ''' </summary>
    ''' <param name="vJibumonCd">自部門コード</param>
    ''' <param name="vShimebi">締日</param>
    Private Sub SetInit(ByVal vJibumonCd As String, ByVal vShimebi As Date)
        '締め期間初期化
        txtNen.Text = ""
        txtTuki.Text = ""
        txtShimeDate.Text = ""

        '締日設定
        Dim dd As String = Uty_Config.ShimebiSeikyu
        txtShimeDate.Text = vShimebi.ToString("yyyy/MM/" & dd.PadLeft(2, "0"))
        txtNen.Text = vShimebi.ToString("yyyy")
        txtTuki.Text = vShimebi.AddMonths(-1).ToString("MM")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Response.Redirect("../Default.aspx")
    End Sub

    Private Sub ListView1_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles ListView1.ItemDataBound
        Try
            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim lChkSel As CheckBox = CType(e.Item.FindControl("chkSelect"), CheckBox)
                Dim lCloseDate As Label = CType(e.Item.FindControl("lblZENKAI_SHIMEBI"), Label)
                If lCloseDate.Text = "" Then
                    lChkSel.Visible = False
                End If

                If e.Item.DataItem("CANCEL_FLG") = 2 Then
                    lCloseDate.Font.Bold = True
                    lCloseDate.ForeColor = Drawing.Color.Blue
                End If
            End If
        Catch ex As Exception
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        Try
            Dim lJibumonCd As String = ""
            Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
            If mpTextBox Is Nothing Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lJibumonCd = mpTextBox.Text
            End If

            Dim lShimebi As String = ""
            Dim lToki As Date = CDate(txtShimeDate.Text)
            lShimebi = lToki.AddMonths(-1).ToString("yyyy/MM/dd")

            '締め期間設定
            SetInit(lJibumonCd, lShimebi)

            '部門情報表示
            DispData()
        Catch ex As Exception
            Logger.WriteErrLog(ex, Session("auth_user_id"), "", "")
        End Try
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Dim lJibumonCd As String = ""
            Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
            If mpTextBox Is Nothing Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lJibumonCd = mpTextBox.Text
            End If

            Dim lShimebi As String = ""
            Dim lToki As Date = CDate(txtShimeDate.Text)
            lShimebi = lToki.AddMonths(1).ToString("yyyy/MM/dd")

            '締め期間設定
            SetInit(lJibumonCd, lShimebi)

            '部門情報表示
            DispData()
        Catch ex As Exception
            Logger.WriteErrLog(ex, Session("auth_user_id"), "", "")
        End Try
    End Sub

    <WebMethod()>
    Public Shared Function CreateGloviaFile(ByVal vParam As String) As String
        Try
            Dim lParam() As String = vParam.Split(",")
            Dim lBumonList As New ArrayList
            For i As Integer = 1 To lParam.Length - 1
                lBumonList.Add(lParam(i))
            Next

            Dim lRstMsg As New StringBuilder

            'GLOVIA出力処理
            Dim lPath As String = CTL_GLOVIA.CreateSeikyuFile(lBumonList, lParam(0))
            Dim lFilename As String = Uty_File.GetFilename(lPath)
            lRstMsg.AppendLine("<br /><a href='../" & Uty_Config.OutputDir & "/" & lPath.Substring(lPath.LastIndexOf("\" & lBumonList(0) & "\") + 1) & "'>GLOVIAファイル請求書 (" & lFilename & ")</a>")

            'GLOVIA出力済みフラグ更新（CANCEL_FLG=2）
            Dim lCnt As Integer = CTL_GLOVIA.UpdateOutputGloviaFlg(lBumonList, lParam(0), 2)
            If lCnt <> lBumonList.Count - 1 Then
                lRstMsg.AppendLine("<p style='color:red;'>GLOVIA出力済みフラグの更新が正しくありませんでした。システム担当に連絡してください。</p>")
            End If

            Return lRstMsg.ToString
        Catch ex As Exception
            Logger.WriteErrLog(ex, HttpContext.Current.Session("auth_user_id"), "", "")
            Throw
        End Try
    End Function

End Class