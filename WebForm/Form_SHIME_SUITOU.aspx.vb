Imports System.Web.Services

Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Common.Logging

Public Class Form_SHIME_SUITOU
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim lJibumonCd As String = ""
        If IsNothing(Session("auth_user_id")) Then
            Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
        Else
            lJibumonCd = Session("auth_bumon_cd")
        End If

        Dim lShimebi As Date = CTL_CT_CLOSE.GetLastCloseDate(lJibumonCd)

        If Not IsPostBack Then
            '締め処理
            radioShime.Checked = True

            '画面初期化
            SetInit(lJibumonCd, lShimebi, 1)
        End If
    End Sub

    ''' <summary>
    ''' 画面を初期化する
    ''' </summary>
    ''' <param name="vJibumonCd">自部門コード</param>
    ''' <param name="vShimebi">締日</param>
    ''' <param name="vSyoriFlg">1:締め処理　0:取消</param>
    Private Sub SetInit(ByVal vJibumonCd As String, ByVal vShimebi As Date, ByVal vSyoriFlg As Integer)

        '締日設定のための算定基準日
        Dim lSanteibi As Date

        ' ==============================================================================
        ' 一度も締め処理されていない場合、入力可
        ' ==============================================================================
        If vShimebi <= "1900/1/1" Then
            '1900/1/1より以前の場合は、締め処理がされていないと判断
            radioShime.Checked = True
            radioCancel.Checked = False
            chkPrint.Visible = True
            chkPrint.Checked = True
            btnPrev.Visible = True
            btnNext.Visible = True

            '締め期間設定
            txtNen.Text = ""
            txtTuki.Text = ""
            radio15.Checked = False
            radio31.Checked = False
            'txtShimeDate.ReadOnly = False
            'txtShimeDate.BackColor = Drawing.Color.White

            If Now.Day <= 15 Then
                '締日設定
                lSanteibi = Now.AddMonths(-1)
                txtShimeDate.Text = Uty_Common.GetGetsumatu(lSanteibi.ToString("yyyyMM"))
                txtNen.Text = lSanteibi.ToString("yyyy")
                txtTuki.Text = lSanteibi.ToString("MM")
                radio15.Checked = False
                radio31.Checked = True
                'radio15.Enabled = True
                'radio31.Enabled = True
            Else
                '締日設定
                txtShimeDate.Text = Now.ToString("yyyy/MM/15")
                txtNen.Text = Now.ToString("yyyy")
                txtTuki.Text = Now.ToString("MM")
                radio15.Checked = True
                radio31.Checked = False
                'radio15.Enabled = True
                'radio31.Enabled = True
            End If

        Else
            ' ==============================================================================
            ' 締めまたは取消の期間設定
            ' ==============================================================================
            If vSyoriFlg = 1 Then
                '締め処理
                radioShime.Checked = True
                radioCancel.Checked = False
                chkPrint.Visible = True
                chkPrint.Checked = True

                If vShimebi.Day = 15 Then
                    '締め期間設定
                    txtNen.Text = vShimebi.Year
                    txtTuki.Text = vShimebi.Month
                    radio15.Checked = False
                    radio31.Checked = True

                    '締日設定
                    txtShimeDate.Text = Uty_Common.GetGetsumatu(vShimebi.ToString("yyyyMM"))
                Else
                    '締め期間設定
                    lSanteibi = vShimebi.AddMonths(1)
                    txtNen.Text = lSanteibi.Year
                    txtTuki.Text = lSanteibi.Month
                    radio31.Checked = False
                    radio15.Checked = True

                    '締日設定
                    txtShimeDate.Text = lSanteibi.ToString("yyyy/MM/15")
                End If

            Else

                '取消
                radioShime.Checked = False
                radioCancel.Checked = True
                chkPrint.Visible = False
                chkPrint.Checked = False

                txtShimeDate.Text = CTL_CT_CLOSE.GetLastCloseDate(vJibumonCd)
                txtNen.Text = vShimebi.Year
                txtTuki.Text = vShimebi.Month
                If vShimebi.Day = 15 Then
                    radio15.Checked = True
                    radio31.Checked = False
                Else
                    radio15.Checked = False
                    radio31.Checked = True
                End If
            End If
        End If
    End Sub

    Private Sub btnExec_Click(sender As Object, e As EventArgs) Handles btnExec.Click
        Try
            'Dim lStartDate As String = ""
            'Dim lEndDate As String = ""

            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Normal, "")

            If txtShimeDate.Text = "" Then
                Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, "対象期間を正しく設定してください。")
            End If

            '自部門コード取得
            Dim lJibumonCd As String = ""
            Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
            If mpTextBox Is Nothing Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lJibumonCd = mpTextBox.Text
            End If

            'ユーザーレベル取得
            Dim lUserLevel As String = ""
            Dim mpLebel As TextBox = CType(Master.FindControl("auth_user_level"), TextBox)
            If mpLebel Is Nothing Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lUserLevel = mpLebel.Text
            End If

            ' =================================================== 以下、WebMethodに変更
            'If radioShime.Checked Then
            ''=============================================
            ''締め処理実行
            ''=============================================

            ''締め処理期間取得
            'CTL_CT_CLOSE.GetCloseKikan(1, txtShimeDate.Text, lStartDate, lEndDate)

            ''当期残高算出
            'Dim lZandaka As Decimal = CTL_PAYMENT.CalcZandaka(lJibumonCd, txtShimeDate.Text, lStartDate, lEndDate)

            ''締め情報追加
            'Dim lCnt As Integer = CTL_CT_CLOSE.Insert(lJibumonCd, txtShimeDate.Text, lZandaka, 1)
            'If lCnt = 0 Then
            '    Throw New Exception("締めテーブルの登録に失敗しました。")
            'End If

            ''出納帳印刷
            'If chkPrint.Checked Then
            '    Dim lPath1 As String = CTL_PAYMENT.Print_Suitou(lJibumonCd, lStartDate, lEndDate, 1)
            '    'Uty_UI.DownloadFile(lPath)
            '    Dim lPath2 As String = CTL_PAYMENT.Print_Suitou_KamokuBetsu(lJibumonCd, lStartDate, lEndDate, 1)
            '    'Uty_UI.DownloadFile(lPath)

            '    Literal2.Text = ""
            '    Literal2.Text &= "<br /><a href='../" & Uty_Config.OutputDir & "/" & lPath1.Substring(lPath1.LastIndexOf("\") + 1) & "'>現金出納帳</a>"
            '    Literal2.Text &= "<br /><a href='../" & Uty_Config.OutputDir & "/" & lPath2.Substring(lPath2.LastIndexOf("\") + 1) & "'>科目別集計表</a>"
            'End If
            ' =================================================== 以上、WebMethodに変更


            If radioCancel.Checked Then
                '=============================================
                '取消処理実行
                '=============================================
                CTL_CT_CLOSE.Torikeshi(lUserLevel, lJibumonCd, 1, txtShimeDate.Text)
                Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Normal, "取消処理完了しました。")
            End If

            '締め日変数設定
            SetInit(lJibumonCd, CTL_CT_CLOSE.GetLastCloseDate(lJibumonCd), 1)

        Catch ex As Exception
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try

    End Sub

    Private Sub radioShime_CheckedChanged(sender As Object, e As EventArgs) Handles radioShime.CheckedChanged
        '自部門コード取得
        Dim lJibumonCd As String = ""
        Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
        If mpTextBox Is Nothing Then
            Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
        Else
            lJibumonCd = mpTextBox.Text
        End If

        Dim lShimebi As Date = CTL_CT_CLOSE.GetLastCloseDate(lJibumonCd)

        '画面初期化
        SetInit(lJibumonCd, lShimebi, 1)
    End Sub

    Private Sub radioCancel_CheckedChanged(sender As Object, e As EventArgs) Handles radioCancel.CheckedChanged
        '自部門コード取得
        Dim lJibumonCd As String = ""
        Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
        If mpTextBox Is Nothing Then
            Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
        Else
            lJibumonCd = mpTextBox.Text
        End If

        chkPrint.Checked = False

        Dim lShimebi As Date = CTL_CT_CLOSE.GetLastCloseDate(lJibumonCd)

        '画面初期化
        SetInit(lJibumonCd, lShimebi, 2)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Response.Redirect("../Default.aspx")
    End Sub

    <WebMethod()>
    Public Shared Function ExecShime(ByVal vParam As String) As String
        Try
            Dim lStartDate As String = ""
            Dim lEndDate As String = ""

            ' パラメータ取得（自部門コード、締日、印刷チェック）
            Dim lLink As New StringBuilder
            Dim lParam() As String = vParam.Split(",")

            '=============================================
            '締め処理実行
            '=============================================

            '締め処理期間取得
            CTL_CT_CLOSE.GetCloseKikan(1, lParam(1), lStartDate, lEndDate)

            '当期残高算出
            Dim lZandaka As Decimal = CTL_PAYMENT.CalcZandaka(lParam(0), lParam(1), lStartDate, lEndDate)

            '締め情報追加
            Dim lCnt As Integer = CTL_CT_CLOSE.Insert(lParam(0), lParam(1), lZandaka, 1)
            If lCnt = 0 Then
                Throw New Exception("締めテーブルの登録に失敗しました。")
            End If

            '出納帳印刷
            If lParam(2) = "true" Then
                Dim lPath1 As String = CTL_PAYMENT.Print_Suitou(lParam(0), lStartDate, lEndDate, 1)
                Dim lPath2 As String = CTL_PAYMENT.Print_Suitou_KamokuBetsu(lParam(0), lStartDate, lEndDate, 1)

                lLink.AppendLine("<br /><a href='../" & Uty_Config.OutputDir & "/" & lPath1.Substring(lPath1.LastIndexOf("\") + 1) & "'>現金出納帳</a>")
                lLink.AppendLine("<br /><a href='../" & Uty_Config.OutputDir & "/" & lPath2.Substring(lPath2.LastIndexOf("\") + 1) & "'>科目別集計表</a>")
            End If

            Return lLink.ToString
        Catch ex As Exception
            Logger.WriteErrLog(ex, HttpContext.Current.Session("auth_user_id"), "", "")
            Throw
        End Try
    End Function

    Private Sub radio15_CheckedChanged(sender As Object, e As EventArgs) Handles radio15.CheckedChanged
        Dim lShimebi As Date = txtShimeDate.Text
        txtShimeDate.Text = lShimebi.ToString("yyyy/MM/15")
    End Sub

    Private Sub radio31_CheckedChanged(sender As Object, e As EventArgs) Handles radio31.CheckedChanged
        Dim lShimebi As Date = txtShimeDate.Text
        txtShimeDate.Text = Uty_Common.GetGetsumatu(lShimebi.ToString("yyyy/MM"))
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
            SetInitMove(lJibumonCd, lShimebi)

        Catch ex As Exception
            Logger.WriteErrLog(ex, Session("auth_user_id"), "", "")
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
            SetInitMove(lJibumonCd, lShimebi)

        Catch ex As Exception
            Logger.WriteErrLog(ex, Session("auth_user_id"), "", "")
        End Try
    End Sub

    Private Sub SetInitMove(ByVal vJibumonCd As String, ByVal vShimebi As Date)
        '締め期間初期化
        txtNen.Text = ""
        txtTuki.Text = ""
        txtShimeDate.Text = ""
        radio15.Checked = False
        radio31.Checked = False
        '締め処理
        radioShime.Checked = True
        radioCancel.Checked = False

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

End Class