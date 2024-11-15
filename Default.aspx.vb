Imports KeihiWeb.Common
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Ctrl

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If IsNothing(Session("auth_user_id")) Then
                Throw New Exception("正しくログインしてください。")
            End If

            If Not IsPostBack Then
                menu.Text = CTL_M_MENU.GetMenu2("0", Session("auth_user_level"))
            End If
        Catch ex As Exception
            Uty_UI.ShowMessage(LabelMessage, Constants.MessageMode.Err, ex.Message)
        End Try
    End Sub

End Class