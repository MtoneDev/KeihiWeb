Imports KeihiWeb.Data
Imports System.Drawing
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty
Public Class Form_M_TRADE
    Inherits System.Web.UI.Page
    Public Enum trade
        TRADE_CD = 0
        TRADE_NM
        TRADE_KN
        ADDRESS1
        ADDRESS2
        BANK_CD
        BRANCH_CD
        ACCOUNT_CD
        ACC_NUM
        ACC_NAME
        MIBARAI_CD
        SHIHARAI_CD
        KEIYAKU_NO
        MSG_RED
    End Enum


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If IsPostBack Then

                Exit Sub

            End If


            Dim ldt As New DS_M_TRADE.M_TRADEDataTable
            ldt = CTL_M_TRADE.GetData()
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
            row.Cells(1).Width = 60
            row.Cells(2).Width = 550
            row.Cells(3).Width = 350
            row.Cells(4).Width = 700
            row.Cells(5).Width = 500
            row.Cells(6).Width = 60
            row.Cells(7).Width = 60
            row.Cells(8).Width = 60
            row.Cells(9).Width = 60
            row.Cells(10).Width = 300
            row.Cells(11).Width = 60
            row.Cells(12).Width = 60
            row.Cells(13).Width = 60

            If row.RowType = DataControlRowType.Header Then

                row.Cells(1).Text = "取引先ｺｰﾄﾞ"
                row.Cells(2).Text = "取引先名"
                row.Cells(3).Text = "取引先カナ"
                row.Cells(4).Text = "住所１"
                row.Cells(5).Text = "住所２"
                row.Cells(6).Text = "銀行ｺｰﾄﾞ"
                row.Cells(7).Text = "支店ｺｰﾄﾞ"
                row.Cells(8).Text = "登録CD"
                row.Cells(9).Text = "登録No"
                row.Cells(10).Text = "登録名"
                row.Cells(11).Text = "未払ｺｰﾄﾞ"
                row.Cells(12).Text = "支払先ｺｰﾄﾞ"
                row.Cells(13).Text = "契約No"


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

    Private Sub closetrade_Click(sender As Object, e As EventArgs) Handles closetrade.Click

        Try

            Response.Redirect("../Default.aspx", False)

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try



    End Sub

    Public Shared Function Val_chk(ByRef err_msg As String, ByVal pdt As String()) As Integer
        Try

            If pdt(trade.MSG_RED).Length <> 0 Then

                err_msg = "取引先コードの値が不正です"

            End If


            If pdt(trade.TRADE_CD).Length = 0 Or pdt(trade.TRADE_CD).Length > 6 Then

                err_msg = "取引先コードの値が不正です"

            End If
            If Not Uty_Common.isHankaku(pdt(trade.TRADE_CD)) Then

                err_msg = "取引先コードの値が不正です"

            End If

            If pdt(trade.TRADE_NM).Length > 50 Then

                err_msg = "取引先名の値が不正です"

            End If


            Return 0

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message.ToString, ex)


        End Try

    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function InsertJOSNData(ByVal test As String()) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New DS_M_TRADE.M_TRADEDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As DS_M_TRADE.M_TRADERow
            Dim err_msg As String = ""

            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'エラーチェック
            Dim result

            If test.Length > 0 Then

                result = Val_chk(err_msg, test)

                If err_msg = "" Then

                    row = ldt.NewRow
                    row.TRADE_CD = test(trade.TRADE_CD)
                    row.TRADE_NM = test(trade.TRADE_NM)
                    row.TRADE_KN = test(trade.TRADE_KN)
                    row.ADDRESS1 = test(trade.ADDRESS1)
                    row.ADDRESS2 = test(trade.ADDRESS2)
                    row.BANK_CD = test(trade.BANK_CD)
                    row.BRANCH_CD = test(trade.BRANCH_CD)
                    If test(trade.ACCOUNT_CD) = String.Empty Then
                        row.ACCOUNT_CD = 0
                    Else
                        row.ACCOUNT_CD = test(trade.ACCOUNT_CD)
                    End If
                    row.ACC_NUM = test(trade.ACC_NUM)
                    row.ACC_NAME = test(trade.ACC_NAME)
                    row.MIBARAI_CD = test(trade.MIBARAI_CD)
                    row.SHIHARAI_CD = test(trade.SHIHARAI_CD)
                    row.KEIYAKU_NO = test(trade.KEIYAKU_NO)

                    ldt.AddM_TRADERow(row)

                    Dim exist_rec As Boolean = CTL_M_TRADE.Insert(ldt)

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

            End If

            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Throw New Exception(ex.Message.ToString, ex)

        End Try


    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function UpdateJOSNData(ByVal test As String()) As List(Of Dictionary(Of String, Object))
        Try


            Dim pdt As New DS_M_TRADE.M_TRADEDataTable
            Dim ldt As New DS_M_TRADE.M_TRADEDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim row As DS_M_TRADE.M_TRADERow
            Dim err_msg As String = ""
            Dim row_result As Dictionary(Of String, Object)
            row_result = New Dictionary(Of String, Object)
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))
            'エラーチェック
            Dim result
            result = Val_chk(err_msg, test)

            If err_msg = "" Then

                row = ldt.NewRow
                row.TRADE_CD = test(trade.TRADE_CD)
                row.TRADE_NM = test(trade.TRADE_NM)
                row.TRADE_KN = test(trade.TRADE_KN)
                row.ADDRESS1 = test(trade.ADDRESS1)
                row.ADDRESS2 = test(trade.ADDRESS2)
                row.BANK_CD = test(trade.BANK_CD)
                row.BRANCH_CD = test(trade.BRANCH_CD)
                If test(trade.ACCOUNT_CD) = String.Empty Then
                    row.ACCOUNT_CD = 0
                Else
                    row.ACCOUNT_CD = test(trade.ACCOUNT_CD)
                End If

                row.ACC_NUM = test(trade.ACC_NUM)
                row.ACC_NAME = test(trade.ACC_NAME)
                row.MIBARAI_CD = test(trade.MIBARAI_CD)
                row.SHIHARAI_CD = test(trade.SHIHARAI_CD)
                row.KEIYAKU_NO = test(trade.KEIYAKU_NO)

                ldt.AddM_TRADERow(row)

                Dim exist_rec As Boolean = CTL_M_TRADE.UpDate(ldt)

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
    Public Shared Function DeleteJOSNData(ByVal trade_cd As String) As List(Of Dictionary(Of String, Object))
        Try

            'Public Shared Function UpdateJOSNData() As List(Of Dictionary(Of String, Object))
            'Public Shared Function GetJOSNData() As JQGridDataClass
            Dim ldt As New DS_M_TRADE.M_TRADEDataTable
            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString

            Dim exist_rec As Boolean = CTL_M_TRADE.Delete(trade_cd)

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