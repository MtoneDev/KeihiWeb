Imports KeihiWeb.Common
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty

Public Class Login
    Inherits System.Web.UI.Page
    Private Sub Login_Init(sender As Object, e As EventArgs) Handles Me.Init
        Session("auth_user_id") = Nothing
        Session("auth_user_name") = Nothing
        Session("auth_user_kana") = Nothing
        Session("auth_user_level") = Nothing
        Session("auth_bumon_cd") = Nothing
        Session("auth_bumon_name") = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
        End If
    End Sub

    Private Sub cmdLogin_Click(sender As Object, e As EventArgs) Handles cmdLogin.Click
        Try
            Dim lDt As New DS_AUTH.AUTH_INFODataTable
            lDt = CTL_AUTH.Login(txtUserID.Text, txtPassword.Text)
            Session("auth_user_id") = lDt(0).USER_ID
            Session("auth_user_name") = lDt(0).USER_NAME
            Session("auth_user_kana") = lDt(0).USER_KANA
            Session("auth_user_level") = lDt(0).USER_LEVEL
            Session("auth_bumon_cd") = lDt(0).BUMON_CD
            Session("auth_bumon_name") = lDt(0).BUMON_NAME
            Response.Redirect("Default.aspx")

        Catch ex As Exception
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try

    End Sub

End Class