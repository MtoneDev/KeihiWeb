Public Class Form_SYKINDB_New2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim rb(10) As RadioButton

        syubetu.Text = "新規"

        For i = 0 To 9

            rb(i) = New RadioButton

        Next


        Dim dt As New DataTable
        dt.Columns.Add("行")
        dt.Columns.Add("部門ｺｰﾄﾞ")
        dt.Columns.Add("支払先")
        dt.Columns.Add("科目ｺｰﾄﾞ")
        dt.Columns.Add("科目名")
        dt.Columns.Add("内訳ｺｰﾄﾞ")
        dt.Columns.Add("内訳名")
        dt.Columns.Add("摘要")
        dt.Columns.Add("経費")
        dt.Columns.Add("消費税")
        dt.Columns.Add("合計金額")

        Dim dr As DataRow
        For i = 0 To 9
            dr = dt.NewRow()
            dr("行") = i + 1
            dr("部門ｺｰﾄﾞ") = " "
            dr("支払先") = " "
            dr("科目ｺｰﾄﾞ") = " "
            dr("科目名") = " "
            dr("内訳ｺｰﾄﾞ") = " "
            dr("内訳名") = " "
            dr("摘要") = " "
            dr("経費") = " "
            dr("消費税") = " "
            dr("合計金額") = " "

            dt.Rows.Add(dr)
        Next

        '残高表示
        Dim auth_bumon_cd = Session("auth_bumon_cd")

        'For i = 0 To 9
        '    dr = dt.NewRow()
        '    dr("行") = i + 1
        '    dr("部門ｺｰﾄﾞ") = " "
        '    dt.Rows.Add(dr)
        'Next

        ListView1.DataSource = dt
        ListView1.DataBind()



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

        Dim tencode As String = HttpContext.Current.Request.QueryString("tencode")
        Dim sortcode As String = HttpContext.Current.Request.QueryString("sortcode")
        Dim xsort As String = HttpContext.Current.Request.QueryString("xsort")
        Dim hidate As String = HttpContext.Current.Request.QueryString("hidate")

        ldt = Ctrl.CTL_M_UTIWAKE.GetData(code)
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
    Private Sub button_kousin_Click(sender As Object, e As EventArgs) Handles button_kousin.Click

        Try

            Dim mpTextBox As TextBox = CType(Master.FindControl("auth_bumon_cd"), TextBox)
            Dim vjibumon As String = mpTextBox.Text
            Dim vinputym As String = kounyu_date.Text.Replace("/", "").Substring(4, 4)

            Dim vslipno As String = denban1.Text

            Dim vdt1 As New DS_PAYMENT.CT_PAYMENT1DataTable
            Dim vdt2 As New DS_PAYMENT.CT_PAYMENT2DataTable

            Dim row1 As DS_PAYMENT.CT_PAYMENT1Row
            Dim row2 As DS_PAYMENT.CT_PAYMENT2Row
            'ヘッダー項目セット
            row1 = vdt1.NewRow
            row1.JIBUMON_CD = vjibumon
            row1.MANAGE_NO = vinputym & vslipno

            row1.INPUT_YM = vinputym
            row1.SLIP_NO = vslipno
            row1.EMP_CD = Request.Form("d_user_cd")


            row1.EMP_NM = Request.Form("d_user_name")
            row1.INPUT_DATE = Now
            row1.REG_DATE = Now
            row1.SLIP_FLG = 1

            vdt1.AddCT_PAYMENT1Row(row1)
            '明細項目セット

            Dim subslip As Integer = 0
            Dim cnt As Integer = 0
            For i As Integer = 0 To 10
                row2 = vdt2.NewRow

                row2.JIBUMON_CD = vjibumon
                row2.MANAGE_NO = vinputym & vslipno
                row2.SUB_SLIP_NO = subslip.ToString
                row2.SLIP_NO = vslipno
                cnt = i + 1
                row2.BUMON_CD = Request("h_bumon_cd" & cnt.ToString)
                row2.BUMON_NM = ""
                row2.PAYEE = Request.Form("shiharai01")
                'row2.KAMOKU_CD = GridView1.Rows(i).Cells(3).Text
                'row2.KAMOKU_NM = GridView1.Rows(i).Cells(4).Text
                'row2.UCHI_CD = GridView1.Rows(i).Cells(5).Text
                'row2.UCHI_NM = GridView1.Rows(i).Cells(6).Text
                'row2.NOTES = GridView1.Rows(i).Cells(7).Text
                'row2.TAX_CD = "1"
                'row2.TAX_NM = "1"
                'row2.EXPENSE = CInt(GridView1.Rows(i).Cells(8).Text.Replace(",", ""))
                'row2.TAX_AMOUNT = CInt(GridView1.Rows(i).Cells(9).Text.Replace(",", ""))
                'row2.AMOUNT = CInt(GridView1.Rows(i).Cells(10).Text.Replace(",", ""))
                'row2.REG_DATE = Now
                'vdt2.AddCT_PAYMENT2Row(row2)

                subslip = subslip + 1

            Next


            '伝票の存在チェック
            Dim ldt As New DS_PAYMENT.CT_PAYMENT1DataTable
            ldt = Ctrl.CTL_PAYMENT.ExistDenpyo_NO(vjibumon, vinputym, vslipno)
            If ldt.Count > 0 Then
            Else

                Ctrl.CTL_PAYMENT.InsertData(vdt1, vdt2)


            End If

        Catch ex As Exception

            Throw New Exception(ex.Message, ex)

        End Try


    End Sub


End Class