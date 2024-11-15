Imports System.Drawing
Imports KeihiWeb.Data
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Common.uty

Public Class Form_SYIKINDN_TEST
    Inherits System.Web.UI.Page
    Public before_manage_no As String = ""
    Public before_jibumon_cd As String = ""
    Public before_input_date As String = ""
    Public before_slip_no As String = ""
    Public errflg As String = ""
    '出金フラグ
    Public SLIPFLG As String = "2"
    Public disp As String = ""
    Public dt2 As New DataTable
    Public dr2 As DataRow


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dt2.Columns.Add("部門ｺｰﾄﾞ")
        dt2.Columns.Add("部門名")
        dt2.Columns.Add("支払先")
        dt2.Columns.Add("科目ｺｰﾄﾞ")
        dt2.Columns.Add("科目名")
        dt2.Columns.Add("内訳ｺｰﾄﾞ")
        dt2.Columns.Add("内訳名")
        dt2.Columns.Add("摘要")
        dt2.Columns.Add("税区分")
        dt2.Columns.Add("税区分名称")
        dt2.Columns.Add("経費")
        dt2.Columns.Add("消費税")
        dt2.Columns.Add("合計金額")


        Dim query_disp As String = Request.QueryString("disp")
        disp = query_disp

        Dim query_manage_no As String = Request.QueryString("manageno")
        Dim query_jibumon_cd As String = Request.QueryString("jibumoncd")
        Dim query_slip_no As String = Request.QueryString("slipno")
        Dim query_input_date As String = Request.QueryString("inputdate")
        auth_user_id.Text = Session("auth_user_id")
        auth_user_name.Text = Session("auth_user_name")

        Dim dt As New DataTable
        dt.Columns.Add("部門ｺｰﾄﾞ")
        dt.Columns.Add("部門名")
        dt.Columns.Add("支払先")
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

        Dim dr As DataRow
        For i = 0 To 9
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



        'kounyu_date.Text = Now()



        '残高表示
        Dim auth_bumon_cd = Session("auth_bumon_cd")

        Dim CloseDate As DateTime = Ctrl.CTL_CT_CLOSE.GetLastCloseDate(auth_bumon_cd)
        'Dim startDate As String = CDate(CloseDate).AddDays(1).ToString("yyyy/MM/dd")
        Dim startDate As String = CDate(CloseDate).ToString("yyyy/MM/dd")
        Dim endDate As String = CDate(DateTime.Today).AddDays(1).ToString("yyyy/MM/dd")

        Dim zandakagoukei As Decimal = Ctrl.CTL_PAYMENT.CalcZandaka(auth_bumon_cd, CloseDate, startDate, endDate)

        zandaka.Text = zandakagoukei

        If IsPostBack Then

            Exit Sub

        End If

        '新規でない場合は検索再表示
        If Not IsNothing(query_manage_no) Then

            GridView_DispSelect(query_jibumon_cd, query_manage_no, query_slip_no)

        End If


    End Sub

    Private Sub GridView1_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated

        Dim row As GridViewRow = e.Row
        If row.RowType = DataControlRowType.Header Then


        End If

        For i = 0 To 12
            row.Cells(i).Wrap = False
        Next

        row.Cells(11).HorizontalAlign = HorizontalAlign.Right
        row.Cells(12).HorizontalAlign = HorizontalAlign.Right
        row.Cells(13).HorizontalAlign = HorizontalAlign.Right

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


    End Sub
    Private Sub GridView_DispSelect(ByVal JIBUMON As String, ByVal MANAGENO As String, ByVal SLIPNO As String)

        'ユーザー
        Dim ldt As New DS_PAYMENT.CT_SELECTPAYMENTDataTable
        Dim lParam As New DS_PAYMENT.SearchParam_ListDataTable


        Dim row As DS_PAYMENT.SearchParam_ListRow
        'ヘッダー項目セット
        row = lParam.NewRow
        row.JIBUMON_CD = JIBUMON
        row.MANAGE_NO = MANAGENO
        '日付け　文字に変換


        lParam.AddSearchParam_ListRow(row)

        ldt = Ctrl.CTL_PAYMENT.SelectData_List(lParam, SLIPFLG)


        'Debug.Print(ldt(0).USER_NAME)
        Dim locate As Integer = 0

        denban1.Text = SLIPNO



        For i = 0 To ldt.Count - 1

            GridView1.Rows(i).Cells(1).Text = ldt(i).BUMON_CD

            GridView1.Rows(i).Cells(2).Text = ldt(i).BUMON_NM

            GridView1.Rows(i).Cells(3).Text = ldt(i).PAYEE

            GridView1.Rows(i).Cells(4).Text = ldt(i).KAMOKU_CD
            GridView1.Rows(i).Cells(5).Text = ldt(i).KAMOKU_NM
            GridView1.Rows(i).Cells(6).Text = ldt(i).UCHI_CD
            GridView1.Rows(i).Cells(7).Text = ldt(i).UCHI_NM
            GridView1.Rows(i).Cells(8).Text = ldt(i).NOTES
            GridView1.Rows(i).Cells(9).Text = ldt(i).TAX_CD
            GridView1.Rows(i).Cells(10).Text = ldt(i).TAX_NM


            'カンマ修飾
            locate = ldt(i).EXPENSE.IndexOf(".")
            If locate > 0 Then
                GridView1.Rows(i).Cells(11).Text = ldt(i).EXPENSE.Substring(0, locate + 1)
                GridView1.Rows(i).Cells(11).Text = String.Format("{0:#,0}", CInt(GridView1.Rows(i).Cells(11).Text))
            End If

            locate = ldt(i).TAX_AMOUNT.IndexOf(".")
            If locate > 0 Then
                GridView1.Rows(i).Cells(12).Text = ldt(i).TAX_AMOUNT.Substring(0, locate + 1)
                GridView1.Rows(i).Cells(12).Text = String.Format("{0:#,0}", CInt(GridView1.Rows(i).Cells(12).Text))
            End If

            locate = ldt(i).AMOUNT.IndexOf(".")
            If locate > 0 Then
                GridView1.Rows(i).Cells(13).Text = ldt(i).AMOUNT.Substring(0, locate + 1)
                GridView1.Rows(i).Cells(13).Text = String.Format("{0:#,0}", CInt(GridView1.Rows(i).Cells(13).Text))
            End If



        Next


        Dim CloseDate As DateTime = Ctrl.CTL_CT_CLOSE.GetLastCloseDate(JIBUMON)
        Dim startDate As String = CDate(CloseDate).AddDays(1).ToString("yyyy/MM/dd")
        Dim endDate As String = kounyu_date.Text

        Dim zandakagoukei As Decimal = Ctrl.CTL_PAYMENT.CalcZandaka(JIBUMON, CloseDate, startDate, endDate)

        zandaka.Text = zandakagoukei


    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNDenpyo_NO(ByVal jibumoncd As String, ByVal ym As String) As List(Of Dictionary(Of String, Object))

        Dim ldt As New DS_PAYMENT.CT_PAYMENT1_DENPYODataTable

        Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
        Dim dt As New DataTable


        ldt = Ctrl.CTL_PAYMENT.GetDenpyo_NO(jibumoncd, ym)

        'jqGrid に渡すデータを格納
        '     Dim data As JQGridDataClass = New JQGridDataClass()

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
            row2.Add("MAXSLIP_NO", "0")
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

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_User() As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass

        Dim ldt As New M_USER.M_USERDataTable

        Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
        Dim dt As New DataTable

        Dim tencode As String = HttpContext.Current.Request.QueryString("tencode")
        Dim sortcode As String = HttpContext.Current.Request.QueryString("sortcode")
        Dim xsort As String = HttpContext.Current.Request.QueryString("xsort")
        Dim hidate As String = HttpContext.Current.Request.QueryString("hidate")

        ldt = Ctrl.CTL_M_USER.GetData()
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
    Public Shared Function SelectUserJOSNData(ByVal code As String) As List(Of Dictionary(Of String, Object))

        Dim ldt As New M_USER.M_USERDataTable

        Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
        Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))


        ldt = Ctrl.CTL_M_USER.SelectData(code)
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
            row2.Add("USER_NAME", "")
            rows_wk.Add(row2)

        End If

        Return rows_wk

    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_kamoku() As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass

        Dim ldt As New DS_M_KAMOKU.M_KAMOKUDataTable

        Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
        Dim dt As New DataTable

        Dim tencode As String = HttpContext.Current.Request.QueryString("tencode")
        Dim sortcode As String = HttpContext.Current.Request.QueryString("sortcode")
        Dim xsort As String = HttpContext.Current.Request.QueryString("xsort")
        Dim hidate As String = HttpContext.Current.Request.QueryString("hidate")

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
        'Public Shared Function GetJOSNData() As JQGridDataClass

        Dim ldt As New DS_M_UTIWAKE.M_UCHIWAKEDataTable

        Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString
        Dim dt As New DataTable
        Dim row As Dictionary(Of String, Object)
        Dim row2 As Dictionary(Of String, Object)
        Dim dr As DataRow
        Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

        'Dim tencode As String = HttpContext.Current.Request.QueryString("tencode")
        'Dim sortcode As String = HttpContext.Current.Request.QueryString("sortcode")
        'Dim xsort As String = HttpContext.Current.Request.QueryString("xsort")
        'Dim hidate As String = HttpContext.Current.Request.QueryString("hidate")

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

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function SelectUtiwakeJOSNData(ByVal kamoku As String, ByVal utiwake As String) As List(Of Dictionary(Of String, Object))

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

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GetJOSNData_Zeiritu(ByVal pdate As String) As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try

            Dim ldt As New DS_M_ZEIRITU.M_ZEIRITUDataTable

            Dim queryStrings As NameValueCollection = HttpContext.Current.Request.QueryString

            Dim tencode As String = HttpContext.Current.Request.QueryString("tencode")
            Dim sortcode As String = HttpContext.Current.Request.QueryString("sortcode")
            Dim xsort As String = HttpContext.Current.Request.QueryString("xsort")
            Dim hidate As String = HttpContext.Current.Request.QueryString("hidate")

            ldt = Ctrl.CTL_M_ZEIRITU.GetData()
            If ldt.Count > 0 Then

            End If


            'jqGrid に渡すデータを格納
            '     Dim data As JQGridDataClass = New JQGridDataClass()

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

            Throw New Exception(ex.Message, ex)

        End Try


    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function DeleteDenpyoJOSNData(ByVal jibumoncd As String, ByVal manageno As String) As List(Of Dictionary(Of String, Object))
        'Public Shared Function GetJOSNData() As JQGridDataClass
        Try


            Dim ldt As New DS_PAYMENT.SearchParam_ListDataTable
            Dim vparam As DS_PAYMENT.SearchParam_ListRow
            vparam = ldt.NewRow
            vparam.MANAGE_NO = manageno
            vparam.JIBUMON_CD = jibumoncd
            ldt.AddSearchParam_ListRow(vparam)


            Ctrl.CTL_PAYMENT.DeleteData(ldt)

            'jqGrid に渡すデータを格納
            '     Dim data As JQGridDataClass = New JQGridDataClass()

            Dim rows_wk As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            Dim row As Dictionary(Of String, Object)

            row = New Dictionary(Of String, Object)
            row.Add("KETUKA", "正常")
            rows_wk.Add(row)


            Return rows_wk

        Catch ex As Exception

            Throw New Exception(ex.Message, ex)

        End Try


    End Function

    Private Sub button_kousin_Click(sender As Object, e As EventArgs) Handles button_kousin.Click

        Try

            Dim before_date As String = ""
            Dim before_vjibumon As String = ""
            Dim before_vinputym As String = ""
            Dim before_vslipno As String = ""

            If syubetu.Text <> "新規" Then

                before_date = before_input_date
                before_vjibumon = before_jibumon_cd
                before_vinputym = before_input_date.Replace("/", "").Substring(2, 4)
                before_vslipno = before_slip_no

            End If



            Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
            Dim vjibumon As String = mpTextBox.Text
            Dim vinputym As String = kounyu_date.Text.Replace("/", "").Substring(2, 4)

            Dim vslipno As String = h_denban.Text
            Dim vdt As New DS_PAYMENT.SearchParam_ListDataTable

            Dim vdt1 As New DS_PAYMENT.CT_PAYMENT1DataTable
            Dim vdt2 As New DS_PAYMENT.CT_PAYMENT2DataTable

            Dim row As DS_PAYMENT.SearchParam_ListRow
            Dim row1 As DS_PAYMENT.CT_PAYMENT1Row
            Dim row2 As DS_PAYMENT.CT_PAYMENT2Row

            row = vdt.NewRow
            row.JIBUMON_CD = vjibumon
            row.MANAGE_NO = vinputym & vslipno.PadLeft(4, "0")
            vdt.AddSearchParam_ListRow(row)

            'ヘッダー項目セット
            row1 = vdt1.NewRow
            row1.JIBUMON_CD = vjibumon

            row1.MANAGE_NO = vinputym & vslipno.PadLeft(4, "0")
            row1.INPUT_YM = vinputym

            row1.SLIP_NO = vslipno
            row1.EMP_CD = Request.Form("d_user_cd")
            row1.EMP_NM = Request.Form("d_user_name")
            row1.INPUT_DATE = Uty_Common.ChangeStringToDate(kounyu_date.Text)

            row1.REG_DATE = Now
            row1.SLIP_FLG = 2

            vdt1.AddCT_PAYMENT1Row(row1)
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
                    row2.SUB_SLIP_NO = subslip.ToString
                    row2.SLIP_NO = vslipno

                    'row2.BUMON_CD = GridView1.Rows(i).Cells(1).Text.ToString
                    wkbumoncd = "h_bumon_cd" & cnt.ToString
                    wkbumonnm = "h_bumon_nm" & cnt.ToString
                    row2.BUMON_CD = Request.Form(wkbumoncd)
                    row2.BUMON_NM = Request.Form(wkbumonnm)

                    row2.PAYEE = Request.Form("h_shiharai" & cnt.ToString)
                    row2.KAMOKU_CD = Request.Form("h_kamoku_cd" & cnt.ToString)
                    row2.KAMOKU_NM = Request.Form("h_kamoku_nm" & cnt.ToString)
                    row2.UCHI_CD = Request.Form("h_uchiwake_cd" & cnt.ToString)
                    row2.UCHI_NM = Request.Form("h_uchiwake_nm" & cnt.ToString)
                    row2.NOTES = Request.Form("h_tekiyo" & cnt.ToString)
                    row2.TAX_CD = Request.Form("h_zkubun" & cnt.ToString)
                    row2.TAX_NM = Request.Form("h_zkubunnm" & cnt.ToString)
                    row2.EXPENSE = CInt(Request.Form("h_keihi" & cnt.ToString))
                    row2.TAX_AMOUNT = CInt(Request.Form("h_zei" & cnt.ToString))
                    row2.AMOUNT = CInt(Request.Form("h_kin" & cnt.ToString))
                    row2.REG_DATE = Now
                    vdt2.AddCT_PAYMENT2Row(row2)

                    subslip = subslip + 1

                End If


            Next



            '伝票の存在チェック
            Dim ldt As New DS_PAYMENT.CT_PAYMENT1DataTable
            ldt = CTL_PAYMENT.ExistDenpyo_NO(vjibumon, vinputym, vslipno)

            If syubetu.Text = "新規" Then

                If ldt.Count > 0 Then

                    msgbox.Text = "伝票が重複しています"
                    errflg = "1"

                Else

                    Ctrl.CTL_PAYMENT.InsertData(vdt1, vdt2)


                End If

            Else
                '更新
                If ldt.Count > 0 Then

                    Ctrl.CTL_PAYMENT.UpdateData(vdt, vdt1, vdt2)

                Else
                    '購入費変更時
                    Ctrl.CTL_PAYMENT.InsertData(vdt1, vdt2)
                    ldt = CTL_PAYMENT.ExistDenpyo_NO(before_vjibumon, before_vinputym, before_vslipno)
                    '削除
                    'If ldt.Count > 0 Then

                    '    row = vdt.NewRow
                    '    row.JIBUMON_CD = before_jibumon_cd
                    '    row.MANAGE_NO = before_manage_no
                    '    vdt.AddSearchParam_ListRow(row)
                    '    Ctrl.CTL_PAYMENT.DeleteData(vdt)

                    'End If


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
                        GridView1.Rows(i).Cells(3).Text = Request.Form("h_shiharai" & cnt.ToString)
                        GridView1.Rows(i).Cells(4).Text = Request.Form("h_kamoku_cd" & cnt.ToString)
                        GridView1.Rows(i).Cells(5).Text = Request.Form("h_kamoku_nm" & cnt.ToString)
                        GridView1.Rows(i).Cells(6).Text = Request.Form("h_uchiwake_cd" & cnt.ToString)
                        GridView1.Rows(i).Cells(7).Text = Request.Form("h_uchiwake_nm" & cnt.ToString)
                        GridView1.Rows(i).Cells(8).Text = Request.Form("h_tekiyo" & cnt.ToString)
                        GridView1.Rows(i).Cells(9).Text = Request.Form("h_zkubun" & cnt.ToString)
                        GridView1.Rows(i).Cells(10).Text = Request.Form("h_zkubunnm" & cnt.ToString)
                        GridView1.Rows(i).Cells(11).Text = Request.Form("h_keihi" & cnt.ToString)
                        GridView1.Rows(i).Cells(12).Text = CInt(Request.Form("h_zei" & cnt.ToString))
                        GridView1.Rows(i).Cells(13).Text = CInt(Request.Form("h_kin" & cnt.ToString))


                    Next

                    zei_goukeiran.Text = zei_goukei.Text
                    keihi_goukeiran.Text = keihi_goukei.Text
                    kin_goukeiran.Text = kin_goukei.Text


                End If

            Else

                GridView_DispSelect(vjibumon, vinputym & vslipno.PadLeft(4, "0"), vslipno)

            End If


        Catch ex As Exception

            Throw New Exception(ex.Message, ex)

        End Try


    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim url As String = ""
        If syubetu.Text = "新規" Then

            Response.Redirect("../Default.aspx")

        Else

            url = url + "Form_SYKINDN_List.aspx?disp=" & disp
            Response.Redirect(url)

        End If


    End Sub

    Private Sub gyo_insert_Click(sender As Object, e As EventArgs) Handles gyo_insert.Click

        If IsNothing(Session("table")) Then
        Else
            dt2 = Session("table")
        End If

        Dim wkgyo As String = Request.Form("wk_gyo")
        Dim wkdata As Integer = 0

        If wkgyo = "" Then

            dr2 = dt2.NewRow()
            dr2(0) = Request.Form("wk_bumoncd")
            dr2(1) = Request.Form("wk_bumonnm")
            dr2(2) = Request.Form("wk_shiharai")
            dr2(3) = Request.Form("wk_kamokucd")
            dr2(4) = Request.Form("wk_kamokunm")
            dr2(5) = Request.Form("wk_uchiwakecd")
            dr2(6) = Request.Form("wk_uchiwakenm")
            dr2(7) = Request.Form("wk_tekiyo")
            dr2(8) = Request.Form("wk_zkubun")
            dr2(9) = Request.Form("wk_zkubunnm")

            dr2(10) = Request.Form("wk_keihi")
            dr2(11) = Request.Form("wk_zei")
            dr2(12) = Request.Form("wk_kin")


            dt2.Rows.Add(dr2)

        Else


            dt2.Rows(CInt(wkgyo) - 1)(0) = Request.Form("wk_bumoncd")
            dt2.Rows(CInt(wkgyo) - 1)(1) = Request.Form("wk_bumonnm")
            dt2.Rows(CInt(wkgyo) - 1)(2) = Request.Form("wk_shiharai")
            dt2.Rows(CInt(wkgyo) - 1)(3) = Request.Form("wk_kamokucd")
            dt2.Rows(CInt(wkgyo) - 1)(4) = Request.Form("wk_kamokunm")

            dt2.Rows(CInt(wkgyo) - 1)(5) = Request.Form("wk_uchiwakecd")
            If Request.Form("wk_uchiwakecd") = "" Then
                dt2.Rows(CInt(wkgyo) - 1)(6) = ""
            Else
                dt2.Rows(CInt(wkgyo) - 1)(6) = Request.Form("wk_uchiwakenm")
            End If
            dt2.Rows(CInt(wkgyo) - 1)(7) = Request.Form("wk_tekiyo")
            dt2.Rows(CInt(wkgyo) - 1)(8) = Request.Form("wk_zkubun")
            dt2.Rows(CInt(wkgyo) - 1)(9) = Request.Form("wk_zkubunnm")
            dt2.Rows(CInt(wkgyo) - 1)(10) = Request.Form("wk_keihi")
            dt2.Rows(CInt(wkgyo) - 1)(11) = Request.Form("wk_zei")
            dt2.Rows(CInt(wkgyo) - 1)(12) = Request.Form("wk_kin")


        End If

        Dim wk01 As String = ""
        Dim wk01_int As Integer = 0
        Dim wk01_sum As Integer = 0
        Dim wk021 As String = ""
        Dim wk02_int As Integer = 0
        Dim wk02_sum As Integer = 0
        Dim wk03 As String = ""
        Dim wk03_int As Integer = 0
        Dim wk03_sum As Integer = 0


        For i = 0 To dt2.Rows.Count - 1
            If Integer.TryParse(dt2.Rows(i)(10).ToString.Replace(",", ""), wk01_int) = True Then

                wk01_sum = wk01_sum + wk01_int

            End If

            If Integer.TryParse(dt2.Rows(i)(11).ToString.Replace(",", ""), wk02_int) = True Then

                wk02_sum = wk02_sum + wk02_int

            End If

            If Integer.TryParse(dt2.Rows(i)(12).ToString.Replace(",", ""), wk03_int) = True Then

                wk03_sum = wk03_sum + wk03_int

            End If


        Next


        keihi_goukeiran.Text = wk01_sum.ToString("#,#")
        zei_goukeiran.Text = wk02_sum.ToString("#,#")
        kin_goukeiran.Text = wk03_sum.ToString("#,#")

        GridView1.DataSource = dt2
        GridView1.DataBind()
        Session("table") = dt2


    End Sub

    Private Sub gyo_delete_Click(sender As Object, e As EventArgs) Handles gyo_delete.Click


        Dim wkgyo As String = Request.Form("wk_gyo")
        Dim cnt As Integer = 1
        Dim cnt2 As Integer = 0
        For i = 1 To 10
            cnt2 = i

            If i <> CInt(wkgyo) Then

                GridView1.Rows(cnt).Cells(1).Text = Request.Form("h_bumon_cd" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(2).Text = Request.Form("h_bumon_nm" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(3).Text = Request.Form("h_shiharai" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(4).Text = Request.Form("h_kamoku_cd" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(5).Text = Request.Form("h_kamoku_nm" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(6).Text = Request.Form("h_uchiwake_cd" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(7).Text = Request.Form("h_uchiwake_nm" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(8).Text = Request.Form("h_tekiyo" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(9).Text = Request.Form("h_zkubun" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(10).Text = Request.Form("h_zkubunnm" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(11).Text = Request.Form("h_keihi" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(12).Text = Request.Form("h_zei" & cnt2.ToString)
                GridView1.Rows(cnt).Cells(13).Text = Request.Form("h_kin" & cnt2.ToString)

                cnt = cnt + 1

            End If

        Next

        'Dim wkdata As Integer = 0


        'Dim wk01 As String = ""
        'Dim wk01_int As Integer = 0
        'Dim wk01_sum As Integer = 0
        'Dim wk021 As String = ""
        'Dim wk02_int As Integer = 0
        'Dim wk02_sum As Integer = 0
        'Dim wk03 As String = ""
        'Dim wk03_int As Integer = 0
        'Dim wk03_sum As Integer = 0


        'For i = 0 To dt2.Rows.Count - 1
        '    If Integer.TryParse(dt2.Rows(i)(10).ToString.Replace(",", ""), wk01_int) = True Then

        '        wk01_sum = wk01_sum + wk01_int

        '    End If

        '    If Integer.TryParse(dt2.Rows(i)(11).ToString.Replace(",", ""), wk02_int) = True Then

        '        wk02_sum = wk02_sum + wk02_int

        '    End If

        '    If Integer.TryParse(dt2.Rows(i)(12).ToString.Replace(",", ""), wk03_int) = True Then

        '        wk03_sum = wk03_sum + wk03_int

        '    End If


        'Next


        'keihi_goukeiran.Text = wk01_sum.ToString("#,#")
        'zei_goukeiran.Text = wk02_sum.ToString("#,#")
        'kin_goukeiran.Text = wk03_sum.ToString("#,#")



    End Sub
End Class