Imports KeihiWeb.Data
Imports System.Drawing
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty
Public Class Form_M_BUMON
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack Then

                Exit Sub

            End If

            'ユーザー
            Dim ldt As New M_BUMON.M_BUMONDataTable
            ldt = Ctrl.CTL_M_BUMON.GetData()
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

            row.Cells(0).Width = 30
            row.Cells(1).Width = 100
            row.Cells(2).Width = 300
            row.Cells(3).Width = 100

            If row.RowType = DataControlRowType.Header Then

                row.Cells(1).Text = "部門コード"
                row.Cells(2).Text = "部門名"
                row.Cells(3).Text = "債務部門"


            End If

            ' データ行である場合に、onmouseover／onmouseout属性を追加（1）
            If row.RowType = DataControlRowType.DataRow Then

                row.Cells(1).HorizontalAlign = HorizontalAlign.Left
                row.Cells(2).HorizontalAlign = HorizontalAlign.Left
                row.Cells(3).HorizontalAlign = HorizontalAlign.Left

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

    Public Shared Function Val_chk(ByRef err_msg As String, ByVal bumon_cd As String, ByVal bumon_name As String, ByVal saimu_cd As String, ByVal msg_red As String) As Integer
        Try

            If msg_red.Length <> 0 Then

                err_msg = "部門コードの値が不正です"

            End If


            If bumon_cd.Length = 0 Or bumon_cd.Length > 5 Then

                err_msg = "部門コードの値が不正です"

            End If
            If Not Uty_Common.isHankaku(bumon_cd) Then

                err_msg = "部門コードの値が不正です"

            End If

            If bumon_name.Length > 64 Then

                err_msg = "部門名の値が不正です"

            End If


            If saimu_cd.Length = 0 Or saimu_cd.Length > 5 Then

                err_msg = "債務部門の値が不正です"

            End If


            Return 0

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)


        End Try

    End Function
    Private Sub closeuser_Click(sender As Object, e As EventArgs) Handles closebumon.Click

        Try

            Response.Redirect("../Default.aspx", False)

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try



    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function InsertJOSNData(ByVal bumon_cd As String, ByVal bumon_name As String, ByVal saimu_cd As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New M_BUMON.M_BUMONDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As M_BUMON.M_BUMONRow
            Dim err_msg As String = ""

            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, bumon_cd, bumon_name, saimu_cd, msg_red)

            If bumon_name.Length = 0 Then

                bumon_name = ""

            End If

            If saimu_cd.Length = 0 Then

                saimu_cd = ""

            End If

            If err_msg = "" Then

                row = ldt.NewRow
                row.BUMON_CD = bumon_cd
                row.BUMON_NM = bumon_name
                row.SAIMU_BMN = saimu_cd
                ldt.AddM_BUMONRow(row)

                Dim exist_rec As Boolean = CTL_M_BUMON.Insert(ldt)


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
    Public Shared Function UpdateJOSNData(ByVal bumon_cd As String, ByVal bumon_name As String, ByVal saimu_cd As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New M_BUMON.M_BUMONDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As M_BUMON.M_BUMONRow
            Dim err_msg As String = ""
            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, bumon_cd, bumon_name, saimu_cd, msg_red)

            If bumon_name.Length = 0 Then

                bumon_name = ""

            End If

            If saimu_cd.Length = 0 Then

                saimu_cd = ""

            End If

            If err_msg = "" Then

                row = ldt.NewRow
                row.BUMON_CD = bumon_cd
                row.BUMON_NM = bumon_name
                row.SAIMU_BMN = saimu_cd
                ldt.AddM_BUMONRow(row)

                Dim exist_rec As Boolean = CTL_M_BUMON.UpDate(ldt)



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
    Public Shared Function DeleteJOSNData(ByVal bumon_cd As String) As List(Of Dictionary(Of String, Object))
        Try

            'Public Shared Function UpdateJOSNData() As List(Of Dictionary(Of String, Object))
            'Public Shared Function GetJOSNData() As JQGridDataClass
            Dim ldt As New M_BUMON.M_BUMONDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString


            Dim exist_rec As Boolean = CTL_M_BUMON.Delete(bumon_cd)

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


End Class