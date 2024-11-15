Imports System.Drawing
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty

Public Class Form_M_KAMOKU
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack Then

                Exit Sub

            End If

            'ユーザー
            Dim ldt As New DS_M_KAMOKU.M_KAMOKUDataTable


            ldt = Ctrl.CTL_M_KAMOKU.GetData

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
            row.Cells(3).Width = 300
            row.Cells(4).Width = 60
            row.Cells(5).Width = 60
            row.Cells(6).Width = 60

            If row.RowType = DataControlRowType.Header Then

                row.Cells(1).Text = "科目コード"
                row.Cells(2).Text = "科目名"
                row.Cells(3).Text = "表示名"
                row.Cells(4).Text = "FLG1"
                row.Cells(5).Text = "FLG2"
                row.Cells(6).Text = "FLG3"
                row.Cells(7).Text = "税区分"
                row.Cells(8).Text = "消費税区分"


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

    Private Sub closekamoku_Click(sender As Object, e As EventArgs) Handles closekamoku.Click

        Try

            Response.Redirect("../Default.aspx", False)

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try



    End Sub


    Public Shared Function Val_chk(ByRef err_msg As String, ByVal kamoku_cd As String, ByVal kamoku_name As String, ByVal disp_name As String, ByVal flg1 As String, ByVal flg2 As String, ByVal flg3 As String, ByVal taxcd As String, ByVal zeiflg As String, ByVal msg_red As String) As Integer
        Try

            If msg_red.Length <> 0 Then

                err_msg = "科目コードの値が不正です"

            End If


            If kamoku_cd.Length = 0 Or kamoku_cd.Length > 5 Then

                err_msg = "科目ｺｰﾄﾞの値が不正です"

            End If
            If Not Uty_Common.isHankaku(kamoku_cd) Then

                err_msg = "科目コードの値が不正です"

            End If

            If kamoku_name.Length > 16 Then

                err_msg = "科目名の値が不正です"

            End If


            If disp_name.Length = 0 Or disp_name.Length > 64 Then

                err_msg = "表示名の値が不正です"

            End If

            If flg1 <> "0" And flg1 <> "1" Then

                err_msg = "FLG1の値が不正です"

            End If
            If flg2 <> "0" And flg2 <> "1" Then

                err_msg = "FLG2の値が不正です"

            End If

            If flg3 <> "0" And flg3 <> "1" Then

                err_msg = "FLG3の値が不正です"

            End If

            If zeiflg <> "0" And zeiflg <> "1" Then

                err_msg = "消費税区分の値が不正です"

            End If

            If taxcd <> "0" And taxcd <> "1" And taxcd <> "2" And taxcd <> "3" And taxcd <> "4" Then

                err_msg = "税区分の値が不正です"

            End If




            Return 0

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)


        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function InsertJOSNData(ByVal kamoku_cd As String, ByVal kamoku_name As String, ByVal disp_name As String, ByVal flg1 As String, ByVal flg2 As String, ByVal flg3 As String, ByVal tax_cd As String, ByVal zei_cd As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New DS_M_KAMOKU.M_KAMOKUDataTable

            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As DS_M_KAMOKU.M_KAMOKURow
            Dim err_msg As String = ""

            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, kamoku_cd, kamoku_name, disp_name, flg1, flg2, flg3, tax_cd, zei_cd, msg_red)

            If kamoku_name.Length = 0 Then

                kamoku_name = ""

            End If

            If disp_name.Length = 0 Then

                disp_name = ""

            End If

            If err_msg = "" Then

                row = ldt.NewRow
                row.KAMOKU_CD = kamoku_cd
                row.KAMOKU_NM = kamoku_name
                row.KAMOKU_DISP = disp_name
                row.FLG1 = flg1
                row.FLG2 = flg2
                row.FLG3 = flg3
                row.TAX_CD = tax_cd
                row.SHOHIZEI_FLG = zei_cd

                ldt.AddM_KAMOKURow(row)

                Dim exist_rec As Boolean = CTL_M_KAMOKU.Insert(ldt)


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
    Public Shared Function UpdateJOSNData(ByVal kamoku_cd As String, ByVal kamoku_name As String, ByVal disp_name As String, ByVal flg1 As String, ByVal flg2 As String, ByVal flg3 As String, ByVal tax_cd As String, ByVal zei_cd As String, ByVal msg_red As String) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New DS_M_KAMOKU.M_KAMOKUDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As DS_M_KAMOKU.M_KAMOKURow

            Dim err_msg As String = ""
            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result
            result = Val_chk(err_msg, kamoku_cd, kamoku_name, disp_name, flg1, flg2, flg3, tax_cd, zei_cd, msg_red)

            If kamoku_name.Length = 0 Then

                kamoku_name = ""

            End If

            If disp_name.Length = 0 Then

                disp_name = ""

            End If

            If err_msg = "" Then

                row = ldt.NewRow
                row.KAMOKU_CD = kamoku_cd
                row.KAMOKU_NM = kamoku_name
                row.KAMOKU_DISP = disp_name
                row.FLG1 = flg1
                row.FLG2 = flg2
                row.FLG3 = flg3
                row.TAX_CD = tax_cd
                row.SHOHIZEI_FLG = zei_cd

                ldt.AddM_KAMOKURow(row)

                Dim exist_rec As Boolean = CTL_M_KAMOKU.UpDate(ldt)



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
    Public Shared Function DeleteJOSNData(ByVal kamoku_cd As String) As List(Of Dictionary(Of String, Object))
        Try

            'Public Shared Function UpdateJOSNData() As List(Of Dictionary(Of String, Object))
            'Public Shared Function GetJOSNData() As JQGridDataClass
            Dim ldt As New DS_M_KAMOKU.M_KAMOKUDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString


            Dim exist_rec As Boolean = CTL_M_KAMOKU.Delete(kamoku_cd)

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