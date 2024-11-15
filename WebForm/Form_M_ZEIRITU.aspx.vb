Imports KeihiWeb.Data
Imports System.Drawing
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty

Public Class Form_M_ZEIRITU
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If IsPostBack Then

                Exit Sub

            End If

            Dim dt As New DataTable
            dt.Columns.Add("分類")
            dt.Columns.Add("開始日")
            dt.Columns.Add("税率")
            dt.Columns.Add("終了日")
            GridView1.ShowHeaderWhenEmpty = True
            Dim dr As DataRow
            Dim maxgyowk = CInt(Uty_Config.ZeiMaster_MaxGYO)

            For i = 0 To maxgyowk
                dr = dt.NewRow()
                dr("分類") = ""
                dt.Rows.Add(dr)
            Next

            GridView1.DataSource = dt
            GridView1.DataBind()
            GridView1.Visible = True

            'ユーザー
            Dim ldt As New DS_M_ZEIRITU.M_ZEIRITUDataTable
            ldt = Ctrl.CTL_M_ZEIRITU.GetData()


            For i = 0 To ldt.Count - 1

                GridView1.Rows(i).Cells(1).Text = ldt(i).BUNRUICD
                GridView1.Rows(i).Cells(2).Text = CDate(ldt(i).KIRIKAEDT).ToString("yyyy/MM/dd")
                GridView1.Rows(i).Cells(3).Text = ldt(i).ZEIRITU
                GridView1.Rows(i).Cells(4).Text = CDate(ldt(i).KIGENDT).ToString("yyyy/MM/dd")

            Next i



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
            row.Cells(2).Width = 150
            row.Cells(3).Width = 100
            row.Cells(4).Width = 150

            If row.RowType = DataControlRowType.Header Then

                'row.Cells(1).Text = "分類"
                'row.Cells(2).Text = "開始日"
                'row.Cells(3).Text = "税率"
                'row.Cells(4).Text = "終了日"


            End If

            ' データ行である場合に、onmouseover／onmouseout属性を追加（1）
            If row.RowType = DataControlRowType.DataRow Then

                row.Cells(1).HorizontalAlign = HorizontalAlign.Left
                row.Cells(2).HorizontalAlign = HorizontalAlign.Left
                row.Cells(3).HorizontalAlign = HorizontalAlign.Right
                row.Cells(4).HorizontalAlign = HorizontalAlign.Left

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
    Private Sub closeuser_Click(sender As Object, e As EventArgs) Handles closezei.Click

        Try

            Response.Redirect("../Default.aspx", False)

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try



    End Sub
    Public Shared Function Val_chk(ByRef err_msg As String, ByVal bunrui As String, ByVal startdate As String, ByVal zeiritu As String, ByVal enddate As String, ByVal msg_red As String) As Integer
        Try

            If msg_red.Length <> 0 Then

                err_msg = "分類の値が不正です"

            End If

            If bunrui.Length > 10 Then

                err_msg = "分類の値が不正です"

            End If
            If Not Uty_Common.isHankaku(bunrui) Then

                err_msg = "分類の値が不正です"

            End If

            Dim wk1 As DateTime

            If Not DateTime.TryParse(startdate, wk1) Then

                err_msg = "開始日の値が不正です"

            End If

            Dim wk2 As DateTime
            If Not DateTime.TryParse(enddate, wk2) Then

                err_msg = "終了日の値が不正です"

            End If

            If (wk1 > wk2) Then

                err_msg = "開始日終了日の大小関係が不正です"

            End If

            Return 0

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)


        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function InsertJOSNData(ByVal bunrui As String, ByVal startdate As String, ByVal zeiritu As String, ByVal enddate As String, ByVal bunrui_old As String, ByVal startdate_old As String, ByVal enddate_old As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        'Public Shared Function UpdateJOSNData() As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try

            Dim ldt As New DS_M_ZEIRITU.M_ZEIRITUDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As DS_M_ZEIRITU.M_ZEIRITURow
            Dim err_msg As String = ""

            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, bunrui, startdate, zeiritu, enddate, msg_red)

            If bunrui.Length = 0 Then

                bunrui = ""

            End If



            If err_msg = "" Then


                row = ldt.NewRow
                row.BUNRUICD = bunrui
                row.KIRIKAEDT = Uty_Common.ChangeStringToDate(startdate)
                row.ZEIRITU = zeiritu
                row.KIGENDT = Uty_Common.ChangeStringToDate(enddate)
                ldt.AddM_ZEIRITURow(row)

                Dim exist_rec As Boolean = CTL_M_ZEIRITU.Insert(ldt)


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
    Public Shared Function UpdateJOSNData(ByVal bunrui As String, ByVal startdate As String, ByVal zeiritu As String, ByVal enddate As String, ByVal bunrui_old As String, ByVal startdate_old As String, ByVal enddate_old As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New DS_M_ZEIRITU.M_ZEIRITUDataTable
            Dim ldt_old As New DS_M_ZEIRITU.M_ZEIRITUDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As DS_M_ZEIRITU.M_ZEIRITURow
            Dim row_old As DS_M_ZEIRITU.M_ZEIRITURow
            Dim err_msg As String = ""
            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, bunrui, startdate, zeiritu, enddate, msg_red)


            If err_msg = "" Then

                row = ldt.NewRow
                row.BUNRUICD = bunrui
                row.KIRIKAEDT = Uty_Common.ChangeStringToDate(startdate)
                row.ZEIRITU = zeiritu
                row.KIGENDT = Uty_Common.ChangeStringToDate(enddate)
                ldt.AddM_ZEIRITURow(row)

                row_old = ldt_old.NewRow
                row_old.BUNRUICD = bunrui_old
                row_old.KIRIKAEDT = Uty_Common.ChangeStringToDate(startdate_old)
                row_old.ZEIRITU = zeiritu
                row_old.KIGENDT = Uty_Common.ChangeStringToDate(enddate_old)
                ldt_old.AddM_ZEIRITURow(row_old)


                Dim exist_rec As Boolean = CTL_M_ZEIRITU.UpDate(ldt_old, ldt)

                row_result.Add("result", "OK")
                row_result.Add("msg", "正常終了")


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

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound


    End Sub
End Class