Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common
Imports KeihiWeb.Common.uty

Public Class Form_Print_Seikyu
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '<<ishi2016-10-5
        Dim lJibumonCd As String = Session("auth_bumon_cd")
        Dim lLastShimebi As Date = CTL_CT_CLOSE.GetLastCloseDate(lJibumonCd, 2)
        Dim rstartdate As String = ""
        Dim renddate As String = ""

        Ctrl.CTL_CT_CLOSE.GetCloseKikan(2, lLastShimebi.AddDays(1), rstartdate, renddate)

        If Not IsPostBack Then
            If txtStartDate.Text = "" Then
                '  txtStartDate.Text = Now.AddMonths(-1).ToString("yyyy/MM/07")
                '  txtEndDate.Text = Now.ToString("yyyy/MM/06")
                txtStartDate.Text = rstartdate
                txtEndDate.Text = renddate
                '>>ishi2016-10-5
            End If
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Response.Redirect("../Default.aspx")
    End Sub

    Private Sub btnShiwake_Click(sender As Object, e As EventArgs) Handles btnShiwake.Click
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

            Dim lPath As String = CTL_BILL.Print_SeikyuShiwake(lJibumonCd, txtStartDate.Text, txtEndDate.Text)

            Uty_UI.DownloadFile(lPath)

        Catch ex As Exception
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try
    End Sub

End Class