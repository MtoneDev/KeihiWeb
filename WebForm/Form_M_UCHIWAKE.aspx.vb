Imports KeihiWeb.Data
Imports System.Drawing
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty

Public Class Form_M_UCHIWAKE
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If IsPostBack Then

                Exit Sub

            End If

            Dim ldt As New DS_M_UTIWAKE.M_UCHIWAKE_MNTDataTable
            ldt = Ctrl.CTL_M_UTIWAKE.GetData2()
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

                row.Cells(1).Text = "科目コード"
                row.Cells(2).Text = "科目名"
                row.Cells(3).Text = "内訳コード"
                row.Cells(4).Text = "内訳名"
                row.Cells(5).Text = "内訳表示名"

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
    Private Sub closeuchiwake_Click(sender As Object, e As EventArgs) Handles closeuchiwake.Click

        Try

            Response.Redirect("../Default.aspx", False)

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try



    End Sub

    Public Shared Function Val_chk(ByRef err_msg As String, ByVal uchiwake_cd As String, ByVal uchiwake_name As String, ByVal kamoku_cd As String, ByVal msg_red As String) As Integer
        Try

            If msg_red.Length <> 0 Then

                err_msg = "内訳コードの値が不正です"

            End If


            If uchiwake_cd.Length = 0 Or uchiwake_cd.Length > 4 Then

                err_msg = "内訳コードの値が不正です"

            End If
            If Not Uty_Common.isHankaku(uchiwake_cd) Then

                err_msg = "内訳コードの値が不正です"

            End If

            If uchiwake_name.Length > 16 Then

                err_msg = "内訳名の値が不正です"

            End If


            If kamoku_cd.Length = 0 Or kamoku_cd.Length > 5 Then

                err_msg = "科目ｺｰﾄﾞの値が不正です"

            End If


            Return 0

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)


        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_Kamoku() As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try

            Dim ldt As New DS_M_KAMOKU.M_KAMOKUDataTable

            Dim dt As New DataTable


            ldt = CTL_M_KAMOKU.GetData()
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
    Public Shared Function InsertJOSNData(ByVal uchiwake_cd As String, ByVal uchiwake_name As String, ByVal uchiwake_disp As String, ByVal kamoku_cd As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New DS_M_UTIWAKE.M_UCHIWAKEDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As DS_M_UTIWAKE.M_UCHIWAKERow
            Dim err_msg As String = ""

            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, uchiwake_cd, uchiwake_name, kamoku_cd, msg_red)

            If uchiwake_name.Length = 0 Then

                uchiwake_name = ""

            End If

            If uchiwake_cd.Length = 0 Then

                uchiwake_cd = ""

            End If

            If err_msg = "" Then

                row = ldt.NewRow
                row.UCHI_CD = uchiwake_cd
                row.UCHI_NM = uchiwake_name
                row.UCHI_DISP = uchiwake_disp
                row.KAMOKU_CD = kamoku_cd
                ldt.AddM_UCHIWAKERow(row)

                Dim exist_rec As Boolean = CTL_M_UTIWAKE.Insert(ldt)

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
    Public Shared Function UpdateJOSNData(ByVal uchiwake_cd As String, ByVal uchiwake_name As String, ByVal uchiwake_disp As String, ByVal kamoku_cd As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New DS_M_UTIWAKE.M_UCHIWAKEDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As DS_M_UTIWAKE.M_UCHIWAKERow
            Dim err_msg As String = ""
            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, uchiwake_cd, uchiwake_name, uchiwake_disp, msg_red)

            If uchiwake_name.Length = 0 Then

                uchiwake_name = ""

            End If

            If kamoku_cd.Length = 0 Then

                kamoku_cd = ""

            End If

            If err_msg = "" Then

                row = ldt.NewRow
                row.KAMOKU_CD = kamoku_cd
                row.UCHI_CD = uchiwake_cd
                row.UCHI_NM = uchiwake_name
                row.UCHI_DISP = uchiwake_disp

                ldt.AddM_UCHIWAKERow(row)

                Dim exist_rec As Boolean = CTL_M_UTIWAKE.UpDate(ldt)

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
    Public Shared Function DeleteJOSNData(ByVal uchiwake_cd As String) As List(Of Dictionary(Of String, Object))
        Try

            'Public Shared Function UpdateJOSNData() As List(Of Dictionary(Of String, Object))
            'Public Shared Function GetJOSNData() As JQGridDataClass
            Dim ldt As New DS_M_UTIWAKE.M_UCHIWAKEDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString

            Dim exist_rec As Boolean = CTL_M_UTIWAKE.Delete(uchiwake_cd)

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