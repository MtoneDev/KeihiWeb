Imports KeihiWeb.Data
Imports System.Drawing
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty


Public Class Form_M_USER
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If IsPostBack Then

                Exit Sub

            End If

            'ユーザー
            Dim ldt As New M_USER.M_USERDataTable
            ldt = CTL_M_USER.GetData()
            GridView1.DataSource = ldt
            GridView1.DataBind()


        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)

        End Try


    End Sub
    Private Sub GridView1_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated


        Try

            Dim row As GridViewRow = e.Row

            row.Cells(5).Width = 70
            row.Cells(7).Width = 50

            If row.RowType = DataControlRowType.Header Then

                row.Cells(1).Text = "ユーザーID"
                row.Cells(2).Text = "ユーザー名"
                row.Cells(3).Text = "カナ名称"
                row.Cells(4).Text = "パスワード"
                row.Cells(5).Text = "ユーザーレベル"
                row.Cells(6).Text = "部門コード"
                row.Cells(7).Text = "DEL_FLG"


            End If

            ' データ行である場合に、onmouseover／onmouseout属性を追加（1）
            If row.RowType = DataControlRowType.DataRow Then
                row.Cells(1).HorizontalAlign = HorizontalAlign.Left
                row.Cells(2).HorizontalAlign = HorizontalAlign.Left
                row.Cells(3).HorizontalAlign = HorizontalAlign.Left
                row.Cells(4).HorizontalAlign = HorizontalAlign.Left
                row.Cells(5).HorizontalAlign = HorizontalAlign.Center
                row.Cells(6).HorizontalAlign = HorizontalAlign.Left
                row.Cells(7).HorizontalAlign = HorizontalAlign.Center

                ' onmouseover属性を設定
                row.Attributes("onmouseover") = "setBg(this, '#CC99FF')"
                'row.Attributes("OnClick") = "setData(this)"

                If row.RowState = DataControlRowState.Normal Then
                    row.Attributes("onmouseout") =
                  String.Format("setBg(this, '{0}')",
                    ColorTranslator.ToHtml(GridView1.RowStyle.BackColor))
                Else
                    row.Attributes("onmouseout") =
                  String.Format("setBg(this, '{0}')",
                    ColorTranslator.ToHtml(
                      GridView1.AlternatingRowStyle.BackColor))
                End If

                GridView1.AlternatingRowStyle.BackColor = Color.LightBlue


            End If

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)

        End Try


    End Sub

    Public Shared Function Val_chk(ByRef err_msg As String, ByVal user_id As String, ByVal user_name As String, ByVal user_kana As String, ByVal user_level As String, ByVal password As String, ByVal bumon_cd As String, ByVal del_flg As String, ByVal msg_red As String) As Integer
        Try

            If msg_red.Length <> 0 Then

                err_msg = "部門コードの値が不正です"

            End If


            If user_level.Length = 0 Then

                err_msg = "ユーザーレベルの値が不正です"

            End If


            If Not IsNumeric(user_level) Then

                err_msg = "ユーザーレベルの値が不正です"

            End If


            Dim wk As Integer = 0

            If Integer.TryParse(user_level, wk) Then

                If wk < 0 Or wk > 9 Then

                    err_msg = "ユーザーレベルは1～9の値を指定"

                End If
            Else

                err_msg = "ユーザーレベルの値が不正です"


            End If


            If user_id.Length = 0 Or user_id.Length > 6 Then

                err_msg = "ユーザーIDの値が不正です"

            End If
            If Not Uty_Common.isHankaku(user_id) Then

                err_msg = "ユーザーIDの値が不正です"

            End If

            If user_name.Length > 64 Then

                err_msg = "ユーザー名の値が不正です"

            End If

            If user_kana.Length > 64 Then

                err_msg = "カナ名の値が不正です"

            End If

            If password.Length > 20 Then

                err_msg = "パスワードの値が不正です"

            End If


            If del_flg <> "0" And del_flg <> "1" Then

                err_msg = "DEL_FLGの値が不正です"

            End If


            If bumon_cd.Length = 0 Or bumon_cd.Length > 5 Then

                err_msg = "部門コードの値が不正です"

            End If

            Return 0

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)


        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function SelectBumonJOSNData(ByVal code As String) As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try

            Dim ldt As New M_BUMON.M_BUMONDataTable

            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            'Dim dt As New DataTable
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))


            ldt = CTL_M_BUMON.SelectData(code)
            Dim row As Dictionary(Of String, Object)
            Dim row2 As Dictionary(Of String, Object)
            Dim dr As DataRow

            If ldt.Count > 0 Then


                'jqGrid に渡すデータを格納
                '     Dim data As JQGridDataClass = New JQGridDataClass()

                For Each dr In ldt.Rows

                    row = New Dictionary(Of String, Object)

                    For Each col As DataColumn In ldt.Columns

                        row.Add(col.ColumnName, dr(col))

                    Next
                    rows_wk.Add(row)

                Next
            Else
                row2 = New Dictionary(Of String, Object)
                row2.Add("BUMON_NM", "")
                rows_wk.Add(row2)

            End If

            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)

        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_Bumon() As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try

            Dim ldt As New M_BUMON.M_BUMONDataTable

            Dim dt As New DataTable


            ldt = CTL_M_BUMON.GetData()
            If ldt.Count > 0 Then

                dt = ldt

            End If


            'jqGrid に渡すデータを格納
            '     Dim data As JQGridDataClass = New JQGridDataClass()

            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            Dim row As Dictionary(Of String, Object)
            Dim dr As DataRow

            For Each dr In ldt.Rows

                row = New Dictionary(Of String, Object)

                For Each col As DataColumn In ldt.Columns

                    row.Add(col.ColumnName, dr(col))

                Next
                rows_wk.Add(row)

            Next


            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)

        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData() As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try

            Dim ldt As New M_USER.M_USERDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim dt As New DataTable

            Dim tencode As String = HttpContext.Current.Request.QueryString("tencode")
            Dim sortcode As String = HttpContext.Current.Request.QueryString("sortcode")
            Dim xsort As String = HttpContext.Current.Request.QueryString("xsort")
            Dim hidate As String = HttpContext.Current.Request.QueryString("hidate")

            ldt = CTL_M_USER.GetData()
            If ldt.Count > 0 Then

                dt = ldt

            End If


            'jqGrid に渡すデータを格納
            '     Dim data As JQGridDataClass = New JQGridDataClass()

            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            Dim row As Dictionary(Of String, Object)
            Dim dr As DataRow

            For Each dr In ldt.Rows

                row = New Dictionary(Of String, Object)

                For Each col As DataColumn In ldt.Columns

                    row.Add(col.ColumnName, dr(col))

                Next
                rows_wk.Add(row)

            Next


            Return rows_wk
        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)

        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function InsertJOSNData(ByVal user_id As String, ByVal user_name As String, ByVal user_kana As String, ByVal user_level As String, ByVal password As String, ByVal bumon_cd As String, ByVal del_flg As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        'Public Shared Function UpdateJOSNData() As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try



            Dim ldt As New M_USER.M_USERDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As M_USER.M_USERRow
            Dim err_msg As String = ""

            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, user_id, user_name, user_kana, user_level, password, bumon_cd, del_flg, msg_red)

            If user_name.Length = 0 Then

                user_name = ""

            End If

            If user_kana.Length = 0 Then

                user_kana = ""

            End If


            If password.Length = 0 Then

                password = ""

            End If


            If err_msg = "" Then


                row = ldt.NewRow
                row.USER_ID = user_id
                row.USER_NAME = user_name
                row.USER_KANA = user_kana
                row.USER_LEVEL = user_level
                row.PASSWORD = password
                row.BUMON_CD = bumon_cd
                row.DEL_FLG = del_flg
                ldt.AddM_USERRow(row)

                Dim exist_rec As Boolean = CTL_M_USER.Insert(ldt)


                If exist_rec = True Then


                    row_result.Add("result", "OK")
                    row_result.Add("msg", "正常終了")

                Else

                    row_result.Add("result", "NG")
                    row_result.Add("msg", "既にレコードが存在します")

                End If

            Else

                row_result.Add("result", "NG")
                row_result.Add("msg", err_msg)


            End If

            rows_wk.Add(row_result)
            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Throw New Exception(ex.Message.ToString, ex)

        End Try


    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function UpdateJOSNData(ByVal user_id As String, ByVal user_name As String, ByVal user_kana As String, ByVal user_level As String, ByVal password As String, ByVal bumon_cd As String, ByVal del_flg As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New M_USER.M_USERDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As M_USER.M_USERRow
            Dim err_msg As String = ""
            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, user_id, user_name, user_kana, user_level, password, bumon_cd, del_flg, msg_red)

            If user_name.Length = 0 Then

                user_name = ""

            End If

            If user_kana.Length = 0 Then

                user_kana = ""

            End If


            If password.Length = 0 Then

                password = ""

            End If

            If err_msg = "" Then

                row = ldt.NewRow
                row.USER_ID = user_id
                row.USER_NAME = user_name
                row.USER_KANA = user_kana
                row.USER_LEVEL = user_level
                row.PASSWORD = password
                row.BUMON_CD = bumon_cd
                row.DEL_FLG = del_flg
                ldt.AddM_USERRow(row)

                Dim exist_rec As Boolean = CTL_M_USER.UpDate(ldt)



                If exist_rec = True Then

                    row_result.Add("result", "OK")
                    row_result.Add("msg", "正常終了")


                Else

                    row_result.Add("result", "NG")
                    row_result.Add("msg", "レコードが存在しません")

                End If

            Else

                row_result.Add("result", "NG")
                row_result.Add("msg", err_msg)

            End If

            rows_wk.Add(row_result)

            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message, ex)


        End Try


    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function DeleteJOSNData(ByVal user_id As String) As List(Of Dictionary(Of String, Object))
        Try

            'Public Shared Function UpdateJOSNData() As List(Of Dictionary(Of String, Object))
            'Public Shared Function GetJOSNData() As JQGridDataClass
            Dim ldt As New M_USER.M_USERDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString


            Dim exist_rec As Boolean = CTL_M_USER.Delete(user_id)

            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))


            If exist_rec = True Then

                row_result.Add("result", "OK")
                row_result.Add("msg", "正常終了")


            Else

                row_result.Add("result", "NG")
                row_result.Add("msg", "レコードが存在しません")

            End If

            rows_wk.Add(row_result)

            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)


        End Try


    End Function

    Protected Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub closeuser_Click(sender As Object, e As EventArgs) Handles closeuser.Click

        Try

            Response.Redirect("../Default.aspx", False)

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try



    End Sub
End Class