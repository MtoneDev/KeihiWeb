Imports System.Drawing
Imports KeihiWeb.Data
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty

Public Class Form_SEIKYUDN_New
    Inherits System.Web.UI.Page
    Public before_manage_no As String = ""
    Public before_jibumon_cd As String = ""
    Public before_input_date As String = ""
    Public before_slip_no As String = ""
    Public errflg As String = ""
    Public disp As String = ""
    Public MAXGYO As Integer = 0

    Public SLIPFLG As String = "2"

    Public input_date1 As String = ""
    Public input_date2 As String = ""
    Public tradecd As String = ""
    Public tradename As String = ""
    Public tradeaddress As String = ""
    Public cirno As String = ""
    Public kensakucd As String = ""
    Public shimebi As String = "06"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            shimebi = Uty_Config.SeikyuShimebi
            shimebi = shimebi.PadLeft(2, "0")


            Dim query_disp As String = Request.QueryString("disp")
            disp = query_disp

            input_date1 = Request.QueryString("input_date1")
            input_date2 = Request.QueryString("input_date2")
            tradecd = Request.QueryString("tradecd")
            tradename = Request.QueryString("tradename")
            tradeaddress = Request.QueryString("tradeaddress")
            cirno = Request.QueryString("cir_no")
            kensakucd = Request.QueryString("kensakucd")

            Dim query_manage_no As String = Request.QueryString("manageno")
            Dim query_jibumon_cd As String = Request.QueryString("jibumoncd")
            Dim query_slip_no As String = Request.QueryString("slipno")
            Dim query_input_date As String = Request.QueryString("inputdate")
            auth_user_id.Text = Session("auth_user_id")
            auth_user_name.Text = Session("auth_user_name")


            If IsNothing(Request.Form("d_trade_cd")) Then
                h_trade_cd.Text = tradecd
                h_trade_name.Text = tradename
                h_trade_address.Text = tradeaddress

            Else

                h_trade_cd.Text = Request.Form("d_trade_cd")
                h_trade_name.Text = Request.Form("d_trade_name")
                h_trade_address.Text = Request.Form("d_trade_address")


                '       after_input_date.Text = kounyu_date.Text

            End If


            If IsNothing(Request.Form("d_cir_no")) Then
                h_cir_no.Text = cirno
            Else

                h_cir_no.Text = Request.Form("d_cir_no")

            End If


            Dim dt As New DataTable
            dt.Columns.Add("部門ｺｰﾄﾞ")
            dt.Columns.Add("部門名")
            dt.Columns.Add("科目ｺｰﾄﾞ")
            dt.Columns.Add("科目名")
            dt.Columns.Add("内訳ｺｰﾄﾞ")
            dt.Columns.Add("内訳名")
            dt.Columns.Add("摘要")
            dt.Columns.Add("税区分")
            dt.Columns.Add("税区分名称")
            dt.Columns.Add("経費")
            dt.Columns.Add("消費税")
            dt.Columns.Add("合計金額")
            GridView1.ShowHeaderWhenEmpty = True
            Dim maxgyowk = CInt(Uty_Config.Seikyu_MaxGYO)
            MAXGYO = maxgyowk

            'スクリプトで行数チェックに使用
            h_maxgyo.Text = MAXGYO

            Dim dr As DataRow
            For i = 0 To MAXGYO
                dr = dt.NewRow()
                dr("部門ｺｰﾄﾞ") = " "
                dt.Rows.Add(dr)
            Next

            GridView1.DataSource = dt
            GridView1.DataBind()
            GridView1.Visible = True

            If IsNothing(query_manage_no) Then
                syubetu.Text = "新規"
            Else

                syubetu.Text = "訂正"
                If IsPostBack Then

                Else

                    kounyu_date.Text = query_input_date

                End If


                before_jibumon_cd = query_jibumon_cd
                before_manage_no = query_manage_no
                before_slip_no = query_slip_no
                before_input_date = query_input_date

                jibumoncd.Text = before_jibumon_cd
                manageno.Text = before_manage_no


            End If

            Dim auth_bumon_cd = Session("auth_bumon_cd")


            If IsPostBack Then

                Exit Sub

            End If

            '新規でない場合は検索再表示
            If Not IsNothing(query_manage_no) Then

                GridView_DispSelect(query_jibumon_cd, query_manage_no, query_slip_no)

            End If

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

            For i = 0 To 11

                row.Cells(i).Wrap = False

            Next
            row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            row.Cells(11).HorizontalAlign = HorizontalAlign.Right
            row.Cells(12).HorizontalAlign = HorizontalAlign.Right


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

                For i = 1 To 9

                    row.Cells(i).HorizontalAlign = HorizontalAlign.Left

                Next


            End If

        Catch ex As Exception

            Throw New Exception(ex.Message, ex)


        End Try

    End Sub
    Private Sub GridView_DispSelect(ByVal JIBUMON As String, ByVal MANAGENO As String, ByVal BILLNO As String)

        Try

            'ユーザー
            Dim ldt As New DS_BILL.CT_SELECTBILLDataTable
            Dim lParam As New DS_BILL.SearchParam_ListDataTable


            Dim row As DS_BILL.SearchParam_ListRow
            'ヘッダー項目セット
            row = lParam.NewRow
            row.JIBUMON_CD = JIBUMON
            row.MANAGE_NO = MANAGENO


            lParam.AddSearchParam_ListRow(row)

            ldt = Ctrl.CTL_BILL.SelectData_List(lParam)


            'Debug.Print(ldt(0).USER_NAME)
            Dim locate As Integer = 0

            denban1.Text = BILLNO



            For i = 0 To ldt.Count - 1

                GridView1.Rows(i).Cells(1).Text = ldt(i).BUMON_CD

                GridView1.Rows(i).Cells(2).Text = ldt(i).BUMON_NM

                GridView1.Rows(i).Cells(3).Text = ldt(i).KAMOKU_CD
                GridView1.Rows(i).Cells(4).Text = ldt(i).KAMOKU_NM
                GridView1.Rows(i).Cells(5).Text = ldt(i).UCHI_CD
                GridView1.Rows(i).Cells(6).Text = ldt(i).UCHI_NM
                GridView1.Rows(i).Cells(7).Text = ldt(i).NOTES
                GridView1.Rows(i).Cells(8).Text = ldt(i).TAX_CD
                GridView1.Rows(i).Cells(9).Text = ldt(i).TAX_NM



                'カンマ修飾
                locate = ldt(i).EXPENSE.IndexOf(".")
                If locate > 0 Then
                    GridView1.Rows(i).Cells(10).Text = ldt(i).EXPENSE.Substring(0, locate + 1)
                    GridView1.Rows(i).Cells(10).Text = String.Format("{0:#,0}", CInt(GridView1.Rows(i).Cells(10).Text))
                End If

                locate = ldt(i).TAX_AMOUNT.IndexOf(".")
                If locate > 0 Then
                    GridView1.Rows(i).Cells(11).Text = ldt(i).TAX_AMOUNT.Substring(0, locate + 1)
                    GridView1.Rows(i).Cells(11).Text = String.Format("{0:#,0}", CInt(GridView1.Rows(i).Cells(11).Text))
                End If

                locate = ldt(i).AMOUNT.IndexOf(".")
                If locate > 0 Then
                    GridView1.Rows(i).Cells(12).Text = ldt(i).AMOUNT.Substring(0, locate + 1)
                    GridView1.Rows(i).Cells(12).Text = String.Format("{0:#,0}", CInt(GridView1.Rows(i).Cells(12).Text))
                End If



            Next


        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")


            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url)


        End Try

    End Sub
    <System.Web.Services.WebMethod()>
    Public Shared Function GetCloseKikan(ByVal code As String) As List(Of Dictionary(Of String, Object))

        Dim rstartdate As String = ""
        Dim renddate As String = ""

        Dim lJibumonCd As String = code
        Dim lLastShimebi As Date = CTL_CT_CLOSE.GetLastCloseDate(lJibumonCd, 2)

        Ctrl.CTL_CT_CLOSE.GetCloseKikan(2, lLastShimebi.AddDays(1), rstartdate, renddate)


        Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

        Dim row As Dictionary(Of String, Object)

        row = New Dictionary(Of String, Object)

        Dim CloseDate_S As String = rstartdate
        Dim CloseDate_E As String = renddate

        row.Add("CLOSEDATE_S", CloseDate_S)
        row.Add("CLOSEDATE_E", CloseDate_E)

        rows_wk.Add(row)

        Return rows_wk

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetLastCloseDate(ByVal code As String) As List(Of Dictionary(Of String, Object))

        Dim ldt As New DS_CT_CLOSE.CT_CLOSEDataTable

        Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
        Dim dt As New DataTable
        Dim wkdate As New Date

        wkdate = Ctrl.CTL_CT_CLOSE.GetLastCloseDate(code, 2)

        Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

        Dim row As Dictionary(Of String, Object)


        row = New Dictionary(Of String, Object)

        Dim CloseDate_S As String = CDate(wkdate).ToString("yyyy/MM/dd")

        row.Add("CLOSEDATE", CloseDate_S)

        rows_wk.Add(row)

        Return rows_wk

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNDenpyo_NO(ByVal jibumoncd As String, ByVal ym As String) As List(Of Dictionary(Of String, Object))
        Try

            Dim ldt As New DS_BILL.CT_BILL1_DENPYODataTable

            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim dt As New DataTable


            ldt = Ctrl.CTL_BILL.GetDenpyo_NO(jibumoncd, ym)


            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            Dim row As Dictionary(Of String, Object)
            Dim row2 As Dictionary(Of String, Object)
            Dim dr As DataRow


            If ldt.Count > 0 Then

                For Each dr In ldt.Rows

                    row = New Dictionary(Of String, Object)

                    For Each col As DataColumn In ldt.Columns

                        row.Add(col.ColumnName, dr(col))

                    Next
                    rows_wk.Add(row)

                Next

            Else
                row2 = New Dictionary(Of String, Object)
                row2.Add("MAXBILL_NO", "0")
                rows_wk.Add(row2)

            End If


            For Each dr In ldt.Rows

                row = New Dictionary(Of String, Object)

                For Each col As DataColumn In ldt.Columns

                    row.Add(col.ColumnName, dr(col))

                Next
                rows_wk.Add(row)

            Next

            Return rows_wk

        Catch ex As Exception

            Throw New Exception(ex.Message, ex)


        End Try

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_Trade() As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try

            Dim ldt As New DS_M_TRADE.M_TRADEDataTable

            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim dt As New DataTable


            ldt = Ctrl.CTL_M_TRADE.GetData()
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

            Throw New Exception(ex.Message, ex)

        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_Trade2(ByVal name As String, ByVal kana As String) As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try

            Dim ldt As New DS_M_TRADE.M_TRADEDataTable

            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim dt As New DataTable


            ldt = Ctrl.CTL_M_TRADE.GetData(name, kana)
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

            Throw New Exception(ex.Message, ex)

        End Try

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function SelectTradeJOSNData(ByVal code As String) As List(Of Dictionary(Of String, Object))

        Dim ldt As New DS_M_TRADE.M_TRADEDataTable

        Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
        Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))


        ldt = Ctrl.CTL_M_TRADE.SelectData(code)
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
            row2.Add("TRADE_NM", "")
            row2.Add("ADDRESS1", "")
            rows_wk.Add(row2)

        End If

        Return rows_wk

    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_kamoku() As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass

        Dim ldt As New DS_M_KAMOKU.M_KAMOKUDataTable

        Dim dt As New DataTable


        ldt = Ctrl.CTL_M_KAMOKU.GetData()
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

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function SelectKamokuJOSNData(ByVal code As String) As List(Of Dictionary(Of String, Object))

        Dim ldt As New DS_M_KAMOKU.M_KAMOKUDataTable

        Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
        Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))


        ldt = Ctrl.CTL_M_KAMOKU.SelectData(code)
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
            row2.Add("KAMOKU_NM", "")
            rows_wk.Add(row2)

        End If

        Return rows_wk

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_Utiwake(ByVal code As String) As List(Of Dictionary(Of String, Object))

        Try


            Dim ldt As New DS_M_UTIWAKE.M_UCHIWAKEDataTable

            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim dt As New DataTable
            Dim row As Dictionary(Of String, Object)
            Dim row2 As Dictionary(Of String, Object)
            Dim dr As DataRow
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))


            ldt = Ctrl.CTL_M_UTIWAKE.GetData(code)
            If ldt.Count > 0 Then

                For Each dr In ldt.Rows

                    row = New Dictionary(Of String, Object)

                    For Each col As DataColumn In ldt.Columns

                        row.Add(col.ColumnName, dr(col))

                    Next
                    rows_wk.Add(row)

                Next


            Else
                row2 = New Dictionary(Of String, Object)
                row2.Add("UCHI_NM", "")
                row2.Add("TAX_CD", "")
                rows_wk.Add(row2)

            End If



            'jqGrid に渡すデータを格納
            '     Dim data As JQGridDataClass = New JQGridDataClass()



            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message, ex)

        End Try


    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function SelectUtiwakeJOSNData(ByVal kamoku As String, ByVal utiwake As String) As List(Of Dictionary(Of String, Object))

        Try

            Dim ldt As New DS_M_UTIWAKE.M_UCHIWAKEDataTable

            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))


            ldt = Ctrl.CTL_M_UTIWAKE.SelectData(kamoku, utiwake)
            Dim row As Dictionary(Of String, Object)
            Dim row2 As Dictionary(Of String, Object)
            Dim dr As DataRow

            If ldt.Count > 0 Then


                For Each dr In ldt.Rows

                    row = New Dictionary(Of String, Object)

                    For Each col As DataColumn In ldt.Columns

                        row.Add(col.ColumnName, dr(col))

                    Next
                    rows_wk.Add(row)

                Next
            Else
                row2 = New Dictionary(Of String, Object)
                row2.Add("UCHI_NM", "")
                row2.Add("TAX_CD", "")
                rows_wk.Add(row2)

            End If

            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message, ex)

        End Try

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_Zeiritu(ByVal pdate As String) As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try


            Dim ldt As New DS_M_ZEIRITU.M_ZEIRITUDataTable
            Dim param As New DS_M_ZEIRITU.M_ZEIRITUKeyDataTable
            Dim paramRow As DS_M_ZEIRITU.M_ZEIRITUKeyRow

            paramRow = param.NewRow

            paramRow.BUNRUICD = ""

            paramRow.INPUTDATE = Uty_Common.ChangeStringToDate(pdate)
            param.AddM_ZEIRITUKeyRow(paramRow)


            ldt = Ctrl.CTL_M_ZEIRITU.GetZeiritu(param)


            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            Dim row As Dictionary(Of String, Object)
            Dim dr As DataRow
            Dim wk As String

            For Each dr In ldt.Rows

                wk = dr(1).ToString

                row = New Dictionary(Of String, Object)

                For Each col As DataColumn In ldt.Columns

                    row.Add(col.ColumnName, dr(col))

                Next
                rows_wk.Add(row)

            Next
            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message, ex)

        End Try


    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function DeleteDenpyoJOSNData(ByVal jibumoncd As String, ByVal manageno As String) As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try


            Dim ldt As New DS_BILL.SearchParam_ListDataTable
            Dim vparam As DS_BILL.SearchParam_ListRow
            vparam = ldt.NewRow
            vparam.MANAGE_NO = manageno
            vparam.JIBUMON_CD = jibumoncd
            ldt.AddSearchParam_ListRow(vparam)


            Ctrl.CTL_BILL.DeleteData(ldt)

            'jqGrid に渡すデータを格納
            '     Dim data As JQGridDataClass = New JQGridDataClass()

            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            Dim row As Dictionary(Of String, Object)

            row = New Dictionary(Of String, Object)
            row.Add("KETUKA", "正常")
            rows_wk.Add(row)


            Return rows_wk

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")
            Throw New Exception(ex.Message, ex)

        End Try


    End Function

    Public Function GetDenpyo_NO(ByVal jibumoncd As String, ByVal ym As String) As Integer

        Try


            Dim ldt As New DS_BILL.CT_BILL1_DENPYODataTable

            Dim dt As New DataTable


            ldt = Ctrl.CTL_BILL.GetDenpyo_NO(jibumoncd, ym)

            'jqGrid に渡すデータを格納
            '     Dim data As JQGridDataClass = New JQGridDataClass()

            Dim wk As Integer = 0
            If ldt.Count > 0 Then
                wk = ldt(0).MAXBILL_NO
            Else
                wk = 0

            End If

            Return wk

        Catch ex As Exception

            Throw New Exception(ex.Message, ex)


        End Try

    End Function
    Private Function create_yymm(ByVal wk As String) As String
        Dim result As String = ""
        Dim kounyu_ddwk As String = wk.Replace("/", "").Substring(6, 2)
        Dim kounyu_mmwk As String = wk.Replace("/", "").Substring(4, 2)
        Dim kounyu_yyyywk As String = wk.Replace("/", "").Substring(0, 4)

        If kounyu_mmwk = "01" Then
            kounyu_yyyywk = (CInt(kounyu_yyyywk) - 1).ToString
            kounyu_mmwk = "12"

        Else

            kounyu_mmwk = (CInt(kounyu_mmwk) - 1).ToString()
            If kounyu_mmwk.Length = 1 Then

                kounyu_mmwk = "0" + kounyu_mmwk

            End If

        End If


        result = (kounyu_yyyywk + kounyu_mmwk + kounyu_ddwk).Substring(2, 4)


        Return result


    End Function




    Private Sub button_kousin_Click(sender As Object, e As EventArgs) Handles button_kousin.Click

        Try
            Dim wk As Date

            If Not Date.TryParse(kounyu_date.Text, wk) Then


                Exit Sub

            End If


            Dim before_date As String = ""
            Dim before_vjibumon As String = ""
            Dim before_vinputym As String = ""
            Dim before_vslipno As String = ""

            If syubetu.Text <> "新規" Then

                before_date = before_input_date
                Dim before_kounyu_ddwk As String = before_input_date.Replace("/", "").Substring(6, 2)



                before_vjibumon = before_jibumon_cd
                before_vinputym = before_input_date.Replace("/", "").Substring(2, 4)

                If before_kounyu_ddwk > shimebi Then


                Else


                    before_vinputym = create_yymm(before_input_date)


                End If


                before_vslipno = before_slip_no

            End If

            Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
            Dim vjibumon As String = mpTextBox.Text

            '-----20161228 sts ishida
            Dim vinputym As String = kounyu_date.Text.Replace("/", "").Substring(2, 4)

            Dim kounyu_ddwk As String = kounyu_date.Text.Replace("/", "").Substring(6, 2)
            If kounyu_ddwk > shimebi Then


            Else


                vinputym = create_yymm(kounyu_date.Text)


            End If



            '伝票番号作成
            Dim vslipno As String = h_denban.Text

            If syubetu.Text <> "新規" Then
                '購入日の月度変更同月でない場合は伝票番号を採番しなおす
                If vinputym <> before_vinputym Then

                    vslipno = GetDenpyo_NO(vjibumon, vinputym) + 1

                End If

            End If


            Dim vdt As New DS_BILL.SearchParam_ListDataTable

            Dim vdt1 As New DS_BILL.CT_BILL1DataTable
            Dim vdt2 As New DS_BILL.CT_BILL2DataTable

            Dim row As DS_BILL.SearchParam_ListRow
            Dim row1 As DS_BILL.CT_BILL1Row
            Dim row2 As DS_BILL.CT_BILL2Row

            row = vdt.NewRow
            row.JIBUMON_CD = vjibumon
            row.MANAGE_NO = vinputym & vslipno.PadLeft(4, "0")
            vdt.AddSearchParam_ListRow(row)

            'ヘッダー項目セット
            row1 = vdt1.NewRow
            row1.JIBUMON_CD = vjibumon

            row1.MANAGE_NO = vinputym & vslipno.PadLeft(4, "0")
            row1.INPUT_YM = vinputym

            row1.BILL_NO = vslipno
            row1.TRADE_CD = Request.Form("d_trade_cd")
            row1.TRADE_NM = Request.Form("d_trade_name")
            row1.INPUT_DATE = Uty_Common.ChangeStringToDate(kounyu_date.Text)
            row1.CIR_NO = Request.Form("d_cir_no")
            row1.REG_DATE = Now

            vdt1.AddCT_BILL1Row(row1)
            '明細項目セット

            Dim subslip As Integer = 1
            Dim wkbumoncd As String
            Dim wkbumonnm As String
            Dim cnt As Integer = 0
            For i As Integer = 0 To GridView1.Rows.Count - 1
                cnt = i + 1
                If Not IsNothing(Request.Form("h_bumon_cd" & cnt.ToString)) Then

                    row2 = vdt2.NewRow

                    row2.JIBUMON_CD = vjibumon
                    row2.MANAGE_NO = vinputym & vslipno.PadLeft(4, "0")
                    row2.SUB_BILL_NO = subslip.ToString
                    row2.BILL_NO = vslipno

                    'row2.BUMON_CD = GridView1.Rows(i).Cells(1).Text.ToString
                    wkbumoncd = "h_bumon_cd" & cnt.ToString
                    wkbumonnm = "h_bumon_nm" & cnt.ToString
                    row2.BUMON_CD = Request.Form(wkbumoncd)
                    row2.BUMON_NM = Request.Form(wkbumonnm)

                    row2.KAMOKU_CD = Request.Form("h_kamoku_cd" & cnt.ToString)
                    row2.KAMOKU_NM = Request.Form("h_kamoku_nm" & cnt.ToString)
                    row2.UCHI_CD = Uty_Common.StringUpper(Request.Form("h_uchiwake_cd" & cnt.ToString))
                    row2.UCHI_NM = Request.Form("h_uchiwake_nm" & cnt.ToString)
                    row2.NOTES = Request.Form("h_tekiyo" & cnt.ToString)
                    row2.TAX_CD = Request.Form("h_zkubun" & cnt.ToString)
                    row2.TAX_NM = Request.Form("h_zkubunnm" & cnt.ToString)
                    row2.EXPENSE = CInt(Request.Form("h_keihi" & cnt.ToString))
                    row2.TAX = CInt(Request.Form("h_zei" & cnt.ToString))
                    row2.REG_DATE = Now
                    vdt2.AddCT_BILL2Row(row2)

                    subslip = subslip + 1

                End If


            Next


            '伝票の存在チェック
            Dim ldt As New DS_BILL.CT_BILL1DataTable
            ldt = CTL_BILL.ExistDenpyo_NO(vjibumon, vinputym, vslipno)

            If syubetu.Text = "新規" Then

                If ldt.Count > 0 Then

                    msgbox.Text = "伝票が重複しています"
                    errflg = "1"

                Else

                    Ctrl.CTL_BILL.InsertData(vdt1, vdt2)


                End If

            Else
                '更新
                If ldt.Count > 0 Then

                    Ctrl.CTL_BILL.UpdateData(vdt, vdt1, vdt2)

                Else
                    '購入日変更時
                    Ctrl.CTL_BILL.InsertData(vdt1, vdt2)
                    Dim paramlist As New DS_BILL.SearchParam_ListDataTable
                    Dim listrow As DS_BILL.SearchParam_ListRow

                    listrow = paramlist.NewSearchParam_ListRow
                    listrow.JIBUMON_CD = before_vjibumon
                    listrow.MANAGE_NO = before_manage_no
                    paramlist.AddSearchParam_ListRow(listrow)


                    '購入日の月度変更同月でない場合は変更前データの削除
                    If vinputym <> before_vinputym Then

                        Ctrl.CTL_BILL.DeleteData(paramlist)

                    End If


                    ldt = CTL_BILL.ExistDenpyo_NO(before_vjibumon, before_vinputym, before_vslipno)

                End If

            End If

            ' msgbox.Text = "伝票が重複しています"
            ' errflg = "1"

            If errflg <> "1" Then

                msgbox.Text = "伝票番号 : " & vslipno & "  登録完了"


            End If

            If syubetu.Text = "新規" Then

                If errflg <> "1" Then

                    GridView_DispSelect(vjibumon, "2099999999", "")

                Else

                    'エラーの時は画面をそのままの状態にもどす
                    cnt = 0
                    For i As Integer = 0 To GridView1.Rows.Count - 1

                        cnt = cnt + 1


                        wkbumoncd = "h_bumon_cd" & cnt.ToString
                        wkbumonnm = "h_bumon_nm" & cnt.ToString
                        If IsNothing(Request.Form(wkbumoncd)) Then

                            Exit For

                        End If
                        GridView1.Rows(i).Cells(1).Text = Request.Form(wkbumoncd)
                        GridView1.Rows(i).Cells(2).Text = Request.Form(wkbumonnm)
                        GridView1.Rows(i).Cells(3).Text = Request.Form("h_kamoku_cd" & cnt.ToString)
                        GridView1.Rows(i).Cells(4).Text = Request.Form("h_kamoku_nm" & cnt.ToString)
                        GridView1.Rows(i).Cells(5).Text = Request.Form("h_uchiwake_cd" & cnt.ToString)
                        GridView1.Rows(i).Cells(6).Text = Request.Form("h_uchiwake_nm" & cnt.ToString)
                        GridView1.Rows(i).Cells(7).Text = Request.Form("h_tekiyo" & cnt.ToString)
                        GridView1.Rows(i).Cells(8).Text = Request.Form("h_zkubun" & cnt.ToString)
                        GridView1.Rows(i).Cells(9).Text = Request.Form("h_zkubunnm" & cnt.ToString)
                        GridView1.Rows(i).Cells(10).Text = CInt(Request.Form("h_keihi" & cnt.ToString))
                        GridView1.Rows(i).Cells(11).Text = CInt(Request.Form("h_zei" & cnt.ToString))
                        GridView1.Rows(i).Cells(12).Text = CInt(Request.Form("h_kin" & cnt.ToString))


                    Next

                    zei_goukeiran.Text = zei_goukei.Text
                    keihi_goukeiran.Text = keihi_goukei.Text
                    kin_goukeiran.Text = kin_goukei.Text


                End If

            Else

                GridView_DispSelect(vjibumon, vinputym & vslipno.PadLeft(4, "0"), vslipno)

            End If


        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, Request.Form("d_trade_cd"), kounyu_date.Text, h_denban.Text)
            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url)


        End Try


    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try

            Dim url As String = ""
            If syubetu.Text = "新規" Then

                Response.Redirect("../Default.aspx", False)

            Else

                url = url + "Form_SEIKYUDN_List.aspx?disp=" & disp & "&input_date1=" + input_date1 + "&input_date2=" + input_date2
                url = url + "&tradecd=" + tradecd + "&tradenm=" + tradename + "&kensakucd=" + kensakucd

                Response.Redirect(url, False)

            End If

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)

        End Try


    End Sub

    Private Sub move_itiran_Click(sender As Object, e As EventArgs) Handles move_itiran.Click
        Try

            Dim url As String = ""
            url = url + "Form_SEIKYUDN_List.aspx?disp=" & disp & "&input_date1=" + input_date1 + "&input_date2=" + input_date2
            url = url + "&tradecd=" + tradecd + "&tradenm=" + tradename + "&kensakucd=" + kensakucd

            Response.Redirect(url, False)

        Catch ex As Exception

            Common.Logging.Logger.WriteErrLog(ex, "", "", "")

            Dim url As String = ""
            url = url + "Form_ERR_PAGE.aspx"
            Response.Redirect(url, False)


        End Try

    End Sub



End Class