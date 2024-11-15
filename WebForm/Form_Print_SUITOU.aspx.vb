Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common
Imports KeihiWeb.Common.uty

Public Class Form_Print_SUITOU
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            '<<ishi2016-10-5
            Dim lJibumonCd As String = Session("auth_bumon_cd")
            Dim lLastShimebi As Date = CTL_CT_CLOSE.GetLastCloseDate(lJibumonCd, 1)
            Dim rstartdate As String = ""
            Dim renddate As String = ""

            Ctrl.CTL_CT_CLOSE.GetCloseKikan(1, lLastShimebi.AddDays(1), rstartdate, renddate)

            If txtStartDate.Text = "" Then
                txtStartDate.Text = rstartdate
                txtEndDate.Text = renddate

                'If Now.Day > 15 Then
                '    txtStartDate.Text = Now.ToString("yyyy/MM/01")
                '    txtEndDate.Text = Now.ToString("yyyy/MM/15")
                'Else
                '    txtStartDate.Text = Now.AddMonths(-1).ToString("yyyy/MM/16")
                '    txtEndDate.Text = Uty_Common.GetGetsumatu(Now.AddMonths(-1).ToString("yyyyMM"))
                'End If

                '>>ishi2016-10-5

            End If
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Response.Redirect("../Default.aspx")
    End Sub

    Private Sub btnMeisai_Click(sender As Object, e As EventArgs) Handles btnMeisai.Click
        Try
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Normal, "")

            Dim lJibumonCd As String = ""
            Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
            If mpTextBox Is Nothing Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lJibumonCd = mpTextBox.Text
            End If

            If txtStartDate.Text = "" Or txtEndDate.Text = "" Then
                Throw New Exception("締め期間を指定してください。")
            ElseIf txtStartDate.Text > txtEndDate.Text Then
                Throw New Exception("締め期間の開始、終了範囲を正しく指定してください。")
            End If

            Dim lPath As String = CTL_PAYMENT.Print_Suitou_KamokuBetsu(lJibumonCd, txtStartDate.Text, txtEndDate.Text)

            Uty_UI.DownloadFile(lPath)

        Catch ex As Exception
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try
    End Sub

    Private Sub btnSuitoucho_Click(sender As Object, e As EventArgs) Handles btnSuitoucho.Click
        Try
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Normal, "")

            Dim lJibumonCd As String = ""
            Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
            If mpTextBox Is Nothing Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lJibumonCd = mpTextBox.Text
            End If

            If txtStartDate.Text = "" Or txtEndDate.Text = "" Then
                Throw New Exception("締め期間を指定してください。")
            ElseIf txtStartDate.Text > txtEndDate.Text Then
                Throw New Exception("締め期間の開始、終了範囲を正しく指定してください。")
            End If

            Dim lPath As String = CTL_PAYMENT.Print_Suitou(lJibumonCd, txtStartDate.Text, txtEndDate.Text)

            Uty_UI.DownloadFile(lPath)

        Catch ex As Exception
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try
    End Sub
End Class