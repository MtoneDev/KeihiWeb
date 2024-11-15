Imports KeihiWeb.Common.uty
Imports System.Drawing
Imports KeihiWeb.Data
Imports KeihiWeb.Ctrl


Public Class Form_SYKINDN_List
    Inherits System.Web.UI.Page
    '出金フラグ
    Public SLIPFLG As String = "2"
    Public shaincd As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim lJibumonCd As String = ""
            Dim shiharai As String = ""
            shaincd = ""

            If IsNothing(Session("auth_user_id")) Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lJibumonCd = Session("auth_bumon_cd")
            End If
            Dim lLastShimebi As Date = CTL_CT_CLOSE.GetLastCloseDate(lJibumonCd, 1)


            If IsPostBack Then

                Exit Sub

            End If

            Dim query_disp As String = Request.QueryString("disp")

            If IsNothing(Request.QueryString("disp")) Then
            Else
                disp.Text = Request.QueryString("disp")
            End If


            '氏名表示 
            shaincd = Request.QueryString("shaincd")
            If IsNothing(shaincd) Then

                shaincd = ""
                h_shaincd.Text = ""

            Else

                h_shaincd.Text = shaincd


            End If

            Dim rstartdate As String = ""
            Dim renddate As String = ""

            'Ctrl.CTL_CT_CLOSE.GetCloseKikan(1, Date.Today, rstartdate, renddate)
            Ctrl.CTL_CT_CLOSE.GetCloseKikan(1, lLastShimebi.AddDays(1), rstartdate, renddate)


            '常に初期値キープ
            Dim query_input_date1 As String = Request.QueryString("input_date1")
            Dim query_input_date2 As String = Request.QueryString("input_date2")

            If IsNothing(query_input_date1) Then
                h_date_from.Text = rstartdate
                h_date_to.Text = renddate
            Else
                h_date_from.Text = query_input_date1
                h_date_to.Text = query_input_date2
            End If


            shiharai = Request.QueryString("shiharai")
            If IsNothing(shiharai) Then

                shiharai = ""

            End If

            date_from.Text = h_date_from.Text
            date_to.Text = h_date_to.Text
            d_shiharai.Text = shiharai



            'GridView_DispSelect(auth_user_id.Text, "", rstartdate, renddate)
            'GridView_DispSelect2(auth_user_id.Text, "", rstartdate, renddate)
            GridView_DispSelect(h_shaincd.Text, shiharai, h_date_from.Text, h_date_to.Text)
            GridView_DispSelect2(h_shaincd.Text, shiharai, h_date_from.Text, h_date_to.Text)


        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try


    End Sub
    Private Sub GridView_DispSelect(ByVal EMPCD As String, ByVal PAYEE As String, ByVal date1 As String, ByVal date2 As String)
        Try

            '明細表示用
            Dim lJibumonCd As String = ""
            If IsNothing(Session("auth_user_id")) Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lJibumonCd = Session("auth_bumon_cd")
            End If

            'ユーザー
            Dim ldt As New DS_PAYMENT.CT_MEISAI_DISPDataTable
            Dim lParam As New DS_PAYMENT.SearchParamDataTable

            Dim row As DS_PAYMENT.SearchParamRow
            'ヘッダー項目セット
            row = lParam.NewRow
            row.JIBUMON_CD = lJibumonCd
            row.EMP_CD = EMPCD
            row.PAYEE = PAYEE
            '日付け　文字に変換

            row.INPUT_DATE_FROM = Uty_Common.ChangeStringToDate(date1)
            row.INPUT_DATE_TO = Uty_Common.ChangeStringToDate(date2)

            lParam.AddSearchParamRow(row)

            ldt = Ctrl.CTL_PAYMENT.MeisaiSelectData(lParam, SLIPFLG)

            GridView1.DataSource = ldt
            GridView1.DataBind()


            'Debug.Print(ldt(0).USER_NAME)
            Dim locate As Integer = 0

            For i = 0 To GridView1.Rows.Count - 1


                GridView1.Rows(i).Cells(4).Text = GridView1.Rows(i).Cells(4).Text.Substring(0, 10)
                GridView1.Rows(i).Cells(8).Text = GridView1.Rows(i).Cells(8).Text

                locate = GridView1.Rows(i).Cells(16).Text.IndexOf(".")
                If locate > 0 Then
                    GridView1.Rows(i).Cells(16).Text = GridView1.Rows(i).Cells(16).Text.Substring(0, locate + 1)
                    GridView1.Rows(i).Cells(16).Text = String.Format("{0:#,0}", CInt(GridView1.Rows(i).Cells(16).Text))
                End If

                locate = GridView1.Rows(i).Cells(17).Text.IndexOf(".")
                If locate > 0 Then
                    GridView1.Rows(i).Cells(17).Text = GridView1.Rows(i).Cells(17).Text.Substring(0, locate + 1)
                    GridView1.Rows(i).Cells(17).Text = String.Format("{0:#,0}", CInt(GridView1.Rows(i).Cells(17).Text))
                End If

                locate = GridView1.Rows(i).Cells(18).Text.IndexOf(".")
                If locate > 0 Then
                    GridView1.Rows(i).Cells(18).Text = GridView1.Rows(i).Cells(18).Text.Substring(0, locate + 1)
                    GridView1.Rows(i).Cells(18).Text = String.Format("{0:#,0}", CInt(GridView1.Rows(i).Cells(18).Text))
                End If

                '非表示
                GridView1.Rows(i).Cells(2).Visible = False
                GridView1.Rows(i).Cells(3).Visible = False


            Next


        Catch ex As Exception
            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)

        End Try

    End Sub


    Private Sub GridView_DispSelect2(ByVal EMPCD As String, ByVal PAYEE As String, ByVal date1 As String, ByVal date2 As String)

        Try

            'ヘッダ表示用
            Dim lJibumonCd As String = ""
            If IsNothing(Session("auth_user_id")) Then
                Throw New Exception("認証情報が取得できませんでした。ログインし直してください。")
            Else
                lJibumonCd = Session("auth_bumon_cd")
            End If

            'ユーザー
            Dim ldt As New DS_PAYMENT.CT_HEADER_DISPDataTable
            Dim lParam As New DS_PAYMENT.SearchParamDataTable

            Dim row As DS_PAYMENT.SearchParamRow
            'ヘッダー項目セット
            row = lParam.NewRow
            row.JIBUMON_CD = lJibumonCd
            row.EMP_CD = EMPCD
            row.PAYEE = PAYEE
            '日付け　文字に変換

            row.INPUT_DATE_FROM = Uty_Common.ChangeStringToDate(date1)
            row.INPUT_DATE_TO = Uty_Common.ChangeStringToDate(date2)

            lParam.AddSearchParamRow(row)

            ldt = Ctrl.CTL_PAYMENT.SelectData2(lParam, SLIPFLG)


            GridView2.DataSource = ldt
            GridView2.DataBind()


            'Debug.Print(ldt(0).USER_NAME)
            Dim locate As Integer = 0



            For i = 0 To GridView2.Rows.Count - 1
                Dim ss As New DS_PAYMENT.SearchParam_ListDataTable
                Dim ssrow As DS_PAYMENT.SearchParam_ListRow
                Dim ddwk As New DS_PAYMENT.CT_PAYMENT2DataTable

                ssrow = ss.NewSearchParam_ListRow
                ssrow.JIBUMON_CD = lJibumonCd
                ssrow.MANAGE_NO = ldt(i).MANAGE_NO
                ss.AddSearchParam_ListRow(ssrow)
                ddwk = CTL_PAYMENT.GetShiharai(ss, SLIPFLG)

                GridView2.Rows(i).Cells(5).Text = ddwk(0).PAYEE

                ddwk.Clear()
                ss.Clear()

                GridView2.Rows(i).Cells(4).Text = GridView2.Rows(i).Cells(4).Text.Substring(0, 10)

                locate = GridView2.Rows(i).Cells(6).Text.IndexOf(".")
                If locate > 0 Then
                    GridView2.Rows(i).Cells(6).Text = GridView2.Rows(i).Cells(6).Text.Substring(0, locate + 1)
                    GridView2.Rows(i).Cells(6).Text = String.Format("{0:#,0}", CInt(GridView2.Rows(i).Cells(6).Text))
                End If

                locate = GridView2.Rows(i).Cells(7).Text.IndexOf(".")
                If locate > 0 Then
                    GridView2.Rows(i).Cells(7).Text = GridView2.Rows(i).Cells(7).Text.Substring(0, locate + 1)
                    GridView2.Rows(i).Cells(7).Text = String.Format("{0:#,0}", CInt(GridView2.Rows(i).Cells(7).Text))
                End If

                locate = GridView2.Rows(i).Cells(8).Text.IndexOf(".")
                If locate > 0 Then
                    GridView2.Rows(i).Cells(8).Text = GridView2.Rows(i).Cells(8).Text.Substring(0, locate + 1)
                    GridView2.Rows(i).Cells(8).Text = String.Format("{0:#,0}", CInt(GridView2.Rows(i).Cells(8).Text))
                End If


            Next

        Catch ex As Exception
            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)

        End Try

    End Sub

    Private Sub GridView1_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated

        Try

            Dim row As GridViewRow = e.Row


            If row.RowType = DataControlRowType.Header Then


                row.Cells(1).Text = "伝票番号"

                row.Cells(2).Visible = False
                row.Cells(3).Visible = False

                row.Cells(4).Text = "購入日"
                row.Cells(5).Text = "支払先"
                row.Cells(6).Text = "行番号"
                row.Cells(7).Text = "部門ｺｰﾄﾞ"
                row.Cells(8).Text = "部門名"
                row.Cells(9).Text = "科目ｺｰﾄﾞ"
                row.Cells(10).Text = "科目名"
                row.Cells(11).Text = "内訳ｺｰﾄﾞ"
                row.Cells(12).Text = "内訳名"
                row.Cells(13).Text = "備考"
                row.Cells(14).Text = "税区分"
                row.Cells(15).Text = "税名称"
                row.Cells(16).Text = "経費"
                row.Cells(17).Text = "消費税"
                row.Cells(18).Text = "金額"
                row.Cells(19).Text = "自部門"
                row.Cells(20).Text = "管理番号"


            End If


            If row.RowType = DataControlRowType.DataRow Then

                row.Cells(16).HorizontalAlign = HorizontalAlign.Right
                row.Cells(17).HorizontalAlign = HorizontalAlign.Right
                row.Cells(18).HorizontalAlign = HorizontalAlign.Right


                row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                row.Cells(2).HorizontalAlign = HorizontalAlign.Right

                row.Cells(3).HorizontalAlign = HorizontalAlign.Right
                '購入日    
                row.Cells(4).HorizontalAlign = HorizontalAlign.Center
                row.Cells(5).HorizontalAlign = HorizontalAlign.Left
                row.Cells(6).HorizontalAlign = HorizontalAlign.Right
                row.Cells(7).HorizontalAlign = HorizontalAlign.Right
                row.Cells(8).HorizontalAlign = HorizontalAlign.Right
                row.Cells(9).HorizontalAlign = HorizontalAlign.Left
                row.Cells(10).HorizontalAlign = HorizontalAlign.Left
                row.Cells(11).HorizontalAlign = HorizontalAlign.Left
                row.Cells(12).HorizontalAlign = HorizontalAlign.Left
                row.Cells(13).HorizontalAlign = HorizontalAlign.Left
                row.Cells(14).HorizontalAlign = HorizontalAlign.Left
                row.Cells(15).HorizontalAlign = HorizontalAlign.Left

            End If


            ' データ行である場合に、onmouseover／onmouseout属性を追加（1）
            If row.RowType = DataControlRowType.DataRow Then


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

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try


    End Sub

    Private Sub GridView2_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView2.RowCreated


        Try

            Dim row As GridViewRow = e.Row
            row.Cells(0).Width = 60
            row.Cells(1).Width = 80
            row.Cells(2).Width = 90
            row.Cells(3).Width = 150
            row.Cells(4).Width = 120
            row.Cells(5).Width = 150
            row.Height = 29


            If row.RowType = DataControlRowType.Header Then

                'row.Cells(1).Text = "自部門"
                'row.Cells(2).Text = "管理番号"
                'row.Cells(3).Text = "伝票番号"
                'row.Cells(4).Text = "社員番号"
                'row.Cells(5).Text = "氏名"
                'row.Cells(6).Text = "購入日"
                'row.Cells(7).Text = "経費"
                'row.Cells(8).Text = "消費税"
                'row.Cells(9).Text = "金額"

                row.Cells(1).Text = "伝票番号"
                row.Cells(2).Text = "社員番号"
                row.Cells(3).Text = "氏名"
                row.Cells(4).Text = "購入日"
                row.Cells(5).Text = "支払先"
                row.Cells(6).Text = "経費"
                row.Cells(7).Text = "消費税"
                row.Cells(8).Text = "金額"
                row.Cells(9).Text = "自部門"
                row.Cells(10).Text = "管理番号"


            End If


            row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            row.Cells(8).HorizontalAlign = HorizontalAlign.Right

            row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            row.Cells(3).HorizontalAlign = HorizontalAlign.Left
            row.Cells(4).HorizontalAlign = HorizontalAlign.Center

            row.Cells(5).HorizontalAlign = HorizontalAlign.Left


            ' データ行である場合に、onmouseover／onmouseout属性を追加（1）
            If row.RowType = DataControlRowType.DataRow Then


                ' onmouseover属性を設定
                row.Attributes("onmouseover") = "setBg(this, '#CC99FF')"
                'row.Attributes("OnClick") = "setData(this)"

                If row.RowState = DataControlRowState.Normal Then
                    row.Attributes("onmouseout") =
                  String.Format("setBg(this, '{0}')",
                    ColorTranslator.ToHtml(GridView2.RowStyle.BackColor))
                Else
                    row.Attributes("onmouseout") =
                  String.Format("setBg(this, '{0}')",
                    ColorTranslator.ToHtml(
                      GridView2.AlternatingRowStyle.BackColor))
                End If

                GridView2.AlternatingRowStyle.BackColor = Color.LightBlue


            End If

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try

    End Sub

    Protected Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        Try

            '検索ボタン

            Dim cd As String = Request.Form("d_user_cd")
            Dim name As String = Request.Form("d_user_name")

            GridView_DispSelect(cd, d_shiharai.Text, input_date1.Text, input_date2.Text)
            GridView_DispSelect2(cd, d_shiharai.Text, input_date1.Text, input_date2.Text)

            date_from.Text = input_date1.Text
            date_to.Text = input_date2.Text

            'auth_user_id.Text = cd
            'auth_user_name.Text = name
            h_shaincd.Text = cd
            h_shainname.Text = name

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try


    End Sub

    Protected Sub Button_Disp_Click(sender As Object, e As EventArgs) Handles Button_Disp.Click
        Try

            Dim manage_no As String = ""
            Dim jibumon_cd As String = ""
            Dim slip_no As String = ""
            Dim input_date As String = ""

            Dim gyo As Integer = 0
            Dim gyowk As String = Request.Form("d_gyo")
            Dim disp2 As String = disp.Text
            Dim dispwk As String = ""
            Dim ldt As New DS_PAYMENT.CT_SELECTPAYMENTDataTable
            Dim lparam As New DS_PAYMENT.SearchParam_ListDataTable
            Dim lrow As DS_PAYMENT.SearchParam_ListRow
            lrow = lparam.NewSearchParam_ListRow()

            h_shaincd.Text = Request.Form("d_user_cd")


            If Not IsNothing(gyowk) And gyowk <> "" Then
                gyo = CInt(gyowk)
                gyo = gyo - 1
                If disp2 = "1" Or disp2 = "" Then
                    ' manage_no = GridView1.Rows(gyo).Cells(2).Text
                    ' jibumon_cd = GridView1.Rows(gyo).Cells(1).Text
                    'slip_no = GridView1.Rows(gyo).Cells(7).Text
                    'input_date = GridView1.Rows(gyo).Cells(5).Text
                    manage_no = GridView1.Rows(gyo).Cells(20).Text
                    jibumon_cd = GridView1.Rows(gyo).Cells(19).Text
                    slip_no = GridView1.Rows(gyo).Cells(1).Text
                    input_date = GridView1.Rows(gyo).Cells(4).Text

                    lrow.JIBUMON_CD = jibumon_cd
                    lrow.MANAGE_NO = manage_no

                    lparam.AddSearchParam_ListRow(lrow)


                    dispwk = "1"
                Else

                    manage_no = GridView2.Rows(gyo).Cells(10).Text
                    jibumon_cd = GridView2.Rows(gyo).Cells(9).Text
                    slip_no = GridView2.Rows(gyo).Cells(1).Text
                    input_date = GridView2.Rows(gyo).Cells(4).Text

                    lrow.JIBUMON_CD = jibumon_cd
                    lrow.MANAGE_NO = manage_no

                    lparam.AddSearchParam_ListRow(lrow)

                    dispwk = "2"

                End If

                ldt = Ctrl.CTL_PAYMENT.SelectData_List(lparam, "2")


                Dim url As String = ""

                url = url + "Form_SYKINDN_New.aspx?manageno=" & manage_no + "&jibumoncd=" + jibumon_cd + "&slipno=" + slip_no + "&inputdate=" + input_date + "&disp=" + dispwk + "&userid=" + ldt(0).EMP_CD
                url = url + "&username=" + ldt(0).EMP_NM + "&input_date1=" + input_date1.Text + "&input_date2=" + input_date2.Text
                url = url + "&shiharai=" + d_shiharai.Text + "&shaincd=" + h_shaincd.Text

                Response.Redirect(url, False)


            End If

        Catch ex As Exception
            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try

    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
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