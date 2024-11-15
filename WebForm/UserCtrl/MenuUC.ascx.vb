Imports KeihiWeb.Ctrl

Public Class MenuUC
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            KeihiMenu.Text = CTL_M_MENU.GetMenu("0", Session("auth_user_level"))
        End If
    End Sub

End Class