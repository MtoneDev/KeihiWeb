﻿Imports System.Web.Services

Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Common.Logging

Public Class From_GLOVIA_SUITOU
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
        lDt = CTL_M_BUMON.GetShimeInfo(txtShimeDate.Text, 1)

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
        radio15.Checked = False
        radio31.Checked = False

        If vShimebi.AddDays(1).Day <= 15 Then
            '締日設定
            txtShimeDate.Text = Uty_Common.GetGetsumatu(vShimebi.AddDays(1).AddMonths(-1).ToString("yyyyMM"))
            txtNen.Text = vShimebi.AddDays(1).AddMonths(-1).ToString("yyyy")
            txtTuki.Text = vShimebi.AddDays(1).AddMonths(-1).ToString("MM")
            radio15.Checked = False
            radio31.Checked = True
        Else
            '締日設定
            txtShimeDate.Text = vShimebi.ToString("yyyy/MM/15")
            txtNen.Text = vShimebi.ToString("yyyy")
            txtTuki.Text = vShimebi.ToString("MM")
            radio15.Checked = True
            radio31.Checked = False
        End If
    End Sub

    'Private Sub btnExec_Click(sender As Object, e As EventArgs) Handles btnExec.Click
    '    Try
    '        Exec_CreateGloviaFile()
    '    Catch ex As Exception
    '        Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
    '    End Try
    'End Sub

    'Private Sub Exec_CreateGloviaFile()
    '    Try
    '        Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Normal, "")

    '        If txtShimeDate.Text = "" Then
    '            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, "対象期間を正しく設定してください。")
    '        End If

    '        Dim lJibumonCd As String = ""
    '        Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
    '        If mpTextBox Is Nothing Then
    '            Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
    '        Else
    '            lJibumonCd = mpTextBox.Text
    '        End If

    '        '選択された部門取得
    '        Dim lBumonList As New ArrayList
    '        For Each r As ListViewDataItem In ListView1.Items
    '            Dim lChk As Boolean = CType(r.FindControl("chkSelect"), CheckBox).Checked
    '            If lChk Then
    '                lBumonList.Add(CType(r.FindControl("lblBUMON_CD"), Label).Text)
    '            End If
    '        Next
    '        If lBumonList.Count = 0 Then
    '            Throw New Exception("部門を選択してください。")
    '            Exit Sub
    '        End If

    '        'GLOVIA出力処理
    '        Dim lPath As String = CTL_GLOVIA.CreateSuitoFile(lBumonList, txtShimeDate.Text)

    '        Literal2.Text = ""
    '        Literal2.Text &= "<br /><a href='../" & Uty_Config.OutputDir & "/" & lPath.Substring(lPath.LastIndexOf("\" & lBumonList(0) & "\") + 1) & "'>GLOVIAファイル現金出納帳 (" & Uty_Config.GloviaKeihi & ")</a>"

    '        '締め日変数設定
    '        SetInit(lJibumonCd, Now())
    '    Catch ex As Exception
    '        Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
    '    End Try
    'End Sub

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
            If lToki.Day = 15 Then
                lShimebi = Uty_Common.GetGetsumatu(lToki.AddMonths(-1).ToString("yyyyMM")).ToString("yyyy/MM/dd")
            Else
                lShimebi = lToki.ToString("yyyy/MM/15")
            End If

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
            If lToki.Day = 15 Then
                lShimebi = Uty_Common.GetGetsumatu(lToki.ToString("yyyyMM")).ToString("yyyy/MM/dd")
            Else
                lShimebi = lToki.AddMonths(1).ToString("yyyy/MM/15")
            End If

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
            Dim lPath As String = CTL_GLOVIA.CreateSuitoFile(lBumonList, lParam(0))
            Dim lFilename As String = Uty_File.GetFilename(lPath)
            lRstMsg.AppendLine("<br /><a href='../" & Uty_Config.OutputDir & "/" & lPath.Substring(lPath.LastIndexOf("\" & lBumonList(0) & "\") + 1) & "'>GLOVIAファイル現金出納帳 (" & lFilename & ")</a>")

            'GLOVIA出力済みフラグ更新（CANCEL_FLG=2）
            Dim lCnt As Integer = CTL_GLOVIA.UpdateOutputGloviaFlg(lBumonList, lParam(0), 1)
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