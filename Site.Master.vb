Imports KeihiWeb.Ctrl

Public Class SiteMaster
    Inherits MasterPage

    Private Sub SiteMaster_Init(sender As Object, e As EventArgs) Handles Me.Init
        If IsNothing(Session("user_id")) Then
            'セッションが空ならログイン画面に遷移する
            Response.Redirect("~/Login.aspx?mode=timeout")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
        End If

        ' メニュー情報を読み込む
        Dim ucMenu As Control = Page.LoadControl("~/WebForm/UserCtrl/MenuUC.ascx")
        Me.HeaderPlaceHolder1.Controls.Add(ucMenu)

        auth_user_id.Text = Session("auth_user_id")
        auth_user_name.Text = Session("auth_user_name")
        auth_user_kana.Text = Session("auth_user_kana")
        auth_user_level.Text = Session("auth_user_level")
        auth_bumon_cd.Text = Session("auth_bumon_cd")
        lblBumonName.Text = Session("auth_bumon_name")
        lblLoginUser.Text = auth_user_name.Text
    End Sub

End Class