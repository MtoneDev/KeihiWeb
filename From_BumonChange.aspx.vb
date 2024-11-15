Imports System.Drawing

Imports KeihiWeb.Common
Imports KeihiWeb.Data
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty
Public Class From_BumonChange
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                '部門マスタ
                Dim lDt As New M_BUMON.M_BUMONDataTable
                lDt = CTL_M_BUMON.GetData()
                ListView1.DataSource = lDt
                ListView1.DataBind()
            End If
        Catch ex As Exception
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Response.Redirect("./Default.aspx")
    End Sub

    'Private Sub ListView1_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles ListView1.ItemCommand
    '    Try
    '        Dim ttt As Label = ListView1.Items(e.CommandArgument).FindControl("lblBUMON_CD")
    '        LabelMessage.Text = ttt.Text
    '    Catch ex As Exception
    '        Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
    '    End Try
    'End Sub

    Private Sub ListView1_SelectedIndexChanging(sender As Object, e As ListViewSelectEventArgs) Handles ListView1.SelectedIndexChanging
        Try
            Dim lSelBumonCd As Label = ListView1.Items(e.NewSelectedIndex).FindControl("lblBUMON_CD")
            Dim mpAuthBumonCd As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
            Dim mpAuthBumonNm As Label = CType(Master.FindControl("lblBumonName"), Label)

            mpAuthBumonCd.Text = lSelBumonCd.Text
            mpAuthBumonNm.Text = CTL_M_BUMON.GetBumonName(lSelBumonCd.Text)
            Session("auth_bumon_cd") = mpAuthBumonCd.Text
            Session("auth_bumon_name") = mpAuthBumonNm.Text

        Catch ex As Exception
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try
    End Sub
End Class