Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty
Imports KeihiWeb.Report

Namespace Enty
    Public Class Ety_CT_PAYMENT2
        Inherits Ety_Base

        Private _Dt1 As New DS_PAYMENT.CT_PAYMENT1DataTable
        Private _Dt2 As New DS_PAYMENT.CT_PAYMENT2DataTable

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vStartDate">開始日</param>
        ''' <param name="vEndDate">終了日</param>
        ''' <param name="vSlipFlg">伝票種別　1:入金　2:出金</param>
        ''' <returns></returns>
        Public Function SumKingaku(ByVal vJibumonCd As String, ByVal vStartDate As String, ByVal vEndDate As String, vSlipFlg As Integer) As Decimal
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT")
            lSql.AppendLine("   ISNULL(SUM(AMOUNT), 0) AS SUM_AMOUNT FROM CT_PAYMENT2 (NOLOCK)")
            lSql.AppendLine("   WHERE JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("     AND EXISTS ")
            lSql.AppendLine("     (SELECT * FROM CT_PAYMENT1 (NOLOCK) ")
            lSql.AppendLine("      WHERE CT_PAYMENT1.JIBUMON_CD = CT_PAYMENT2.JIBUMON_CD ")
            lSql.AppendLine("        AND CT_PAYMENT1.MANAGE_NO = CT_PAYMENT2.MANAGE_NO ")
            lSql.AppendLine("        AND CT_PAYMENT1.SLIP_FLG  = @slip_flg ")
            lSql.AppendLine("        AND CT_PAYMENT1.INPUT_DATE >= @input_date_from")
            lSql.AppendLine("        AND CT_PAYMENT1.INPUT_DATE < @input_date_To")
            lSql.AppendLine("     )")

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = _Dt1.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vJibumonCd
                    lCmd.Parameters.Add(jibumon_cd)
                    '入力日From
                    Dim input_date_from As New SqlParameter
                    input_date_from.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.INPUT_DATEColumn.DataType)
                    input_date_from.Size = _Dt1.INPUT_DATEColumn.MaxLength
                    input_date_from.ParameterName = "@input_date_from"
                    input_date_from.Value = vStartDate
                    lCmd.Parameters.Add(input_date_from)
                    '入力日To
                    Dim input_date_To As New SqlParameter
                    input_date_To.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.INPUT_DATEColumn.DataType)
                    input_date_To.Size = _Dt1.INPUT_DATEColumn.MaxLength
                    input_date_To.ParameterName = "@input_date_To"
                    input_date_To.Value = CDate(vEndDate).AddDays(1).ToString("yyyy/MM/dd")
                    lCmd.Parameters.Add(input_date_To)
                    '伝票種別
                    Dim slip_flg As New SqlParameter
                    slip_flg.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.SLIP_FLGColumn.DataType)
                    slip_flg.Size = _Dt1.SLIP_FLGColumn.MaxLength
                    slip_flg.ParameterName = "@slip_flg"
                    slip_flg.Value = vSlipFlg
                    lCmd.Parameters.Add(slip_flg)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    lDb.Open()
                    lRst = lCmd.ExecuteScalar()

                End Using 'lCmd
            End Using 'lDb

            Return lRst
        End Function

        Public Function InsertData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                     vSeihin As DS_PAYMENT.CT_PAYMENT2DataTable) As Integer

            Try

                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt2)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO CT_PAYMENT2 (")

                For i As Integer = 0 To ar.Count - 1
                    If i = 0 Then
                        lSql.AppendLine("   " & ar(i).ToString)
                    Else
                        lSql.AppendLine(" , " & ar(i).ToString)
                    End If
                Next
                lSql.AppendLine(") VALUES (")
                For i As Integer = 0 To ar.Count - 1
                    If i = 0 Then
                        lSql.AppendLine("   @" & ar(i).ToString.ToLower)
                    Else
                        lSql.AppendLine(" , @" & ar(i).ToString.ToLower)
                    End If
                Next
                lSql.AppendLine(")")


                '更新パラメータ
                Dim jibumoncd As New SqlParameter
                jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.JIBUMON_CDColumn.DataType)
                jibumoncd.Size = _Dt2.JIBUMON_CDColumn.MaxLength
                jibumoncd.ParameterName = "@jibumon_cd"
                rCmd.Parameters.Add(jibumoncd)

                Dim manageno As New SqlParameter
                manageno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.MANAGE_NOColumn.DataType)
                manageno.Size = _Dt2.MANAGE_NOColumn.MaxLength
                manageno.ParameterName = "@manage_no"
                rCmd.Parameters.Add(manageno)

                Dim subslipno As New SqlParameter
                subslipno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.SUB_SLIP_NOColumn.DataType)
                subslipno.Size = _Dt2.SUB_SLIP_NOColumn.MaxLength
                subslipno.ParameterName = "@sub_slip_no"
                rCmd.Parameters.Add(subslipno)


                Dim slipno As New SqlParameter
                slipno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.SLIP_NOColumn.DataType)
                slipno.Size = _Dt2.SLIP_NOColumn.MaxLength
                slipno.ParameterName = "@slip_no"
                rCmd.Parameters.Add(slipno)

                Dim bumoncd As New SqlParameter
                bumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.BUMON_CDColumn.DataType)
                bumoncd.Size = _Dt2.BUMON_CDColumn.MaxLength
                bumoncd.ParameterName = "@bumon_cd"
                rCmd.Parameters.Add(bumoncd)

                Dim bumonnm As New SqlParameter
                bumonnm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.BUMON_NMColumn.DataType)
                bumonnm.Size = _Dt2.BUMON_NMColumn.MaxLength
                bumonnm.ParameterName = "@bumon_nm"
                rCmd.Parameters.Add(bumonnm)

                Dim payee As New SqlParameter
                payee.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.PAYEEColumn.DataType)
                payee.Size = _Dt2.PAYEEColumn.MaxLength
                payee.ParameterName = "@payee"
                rCmd.Parameters.Add(payee)

                Dim kamokucd As New SqlParameter
                kamokucd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.KAMOKU_CDColumn.DataType)
                kamokucd.Size = _Dt2.KAMOKU_CDColumn.MaxLength
                kamokucd.ParameterName = "@kamoku_cd"
                rCmd.Parameters.Add(kamokucd)

                Dim kamokunm As New SqlParameter
                kamokunm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.KAMOKU_NMColumn.DataType)
                kamokunm.Size = _Dt2.KAMOKU_NMColumn.MaxLength
                kamokunm.ParameterName = "@kamoku_nm"
                rCmd.Parameters.Add(kamokunm)

                Dim uchicd As New SqlParameter
                uchicd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.UCHI_CDColumn.DataType)
                uchicd.Size = _Dt2.UCHI_CDColumn.MaxLength
                uchicd.ParameterName = "@uchi_cd"
                rCmd.Parameters.Add(uchicd)

                Dim uchinm As New SqlParameter
                uchinm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.UCHI_NMColumn.DataType)
                uchinm.Size = _Dt2.UCHI_NMColumn.MaxLength
                uchinm.ParameterName = "@uchi_nm"
                rCmd.Parameters.Add(uchinm)

                Dim notes As New SqlParameter
                notes.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.NOTESColumn.DataType)
                notes.Size = _Dt2.NOTESColumn.MaxLength
                notes.ParameterName = "@notes"
                rCmd.Parameters.Add(notes)


                Dim taxcd As New SqlParameter
                taxcd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.TAX_CDColumn.DataType)
                taxcd.Size = _Dt2.TAX_CDColumn.MaxLength
                taxcd.ParameterName = "@tax_cd"
                rCmd.Parameters.Add(taxcd)

                Dim taxnm As New SqlParameter
                taxnm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.TAX_NMColumn.DataType)
                taxnm.Size = _Dt2.TAX_NMColumn.MaxLength
                taxnm.ParameterName = "@tax_nm"
                rCmd.Parameters.Add(taxnm)

                Dim expense As New SqlParameter
                expense.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.EXPENSEColumn.DataType)
                expense.Size = _Dt2.EXPENSEColumn.MaxLength
                expense.ParameterName = "@expense"
                rCmd.Parameters.Add(expense)

                Dim taxamout As New SqlParameter
                taxamout.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.TAX_AMOUNTColumn.DataType)
                taxamout.Size = _Dt2.TAX_AMOUNTColumn.MaxLength
                taxamout.ParameterName = "@tax_amount"
                rCmd.Parameters.Add(taxamout)

                Dim amount As New SqlParameter
                amount.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.AMOUNTColumn.DataType)
                amount.Size = _Dt2.AMOUNTColumn.MaxLength
                amount.ParameterName = "@amount"
                rCmd.Parameters.Add(amount)

                Dim regdate As New SqlParameter
                regdate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.REG_DATEColumn.DataType)
                regdate.Size = _Dt2.REG_DATEColumn.MaxLength
                regdate.ParameterName = "@reg_date"
                rCmd.Parameters.Add(regdate)


                '値をセットする
                For Each row As DS_PAYMENT.CT_PAYMENT2Row In vSeihin

                    jibumoncd.Value = row.JIBUMON_CD
                    manageno.Value = row.MANAGE_NO
                    subslipno.Value = row.SUB_SLIP_NO
                    slipno.Value = row.SLIP_NO
                    bumoncd.Value = row.BUMON_CD
                    bumonnm.Value = row.BUMON_NM
                    payee.Value = row.PAYEE
                    kamokucd.Value = row.KAMOKU_CD
                    kamokunm.Value = row.KAMOKU_NM
                    uchicd.Value = row.UCHI_CD
                    uchinm.Value = row.UCHI_NM
                    notes.Value = row.NOTES
                    taxcd.Value = row.TAX_CD
                    taxnm.Value = row.TAX_NM
                    expense.Value = row.EXPENSE
                    taxamout.Value = row.TAX_AMOUNT
                    amount.Value = row.AMOUNT
                    regdate.Value = row.REG_DATE


                    rCmd.CommandText = lSql.ToString
                    rCmd.Connection = rDb

                    lCnt = rCmd.ExecuteNonQuery()
                Next


                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Private Function SetCondition(ByRef vCmd As SqlCommand, ByVal vdt As DS_PAYMENT.SearchParamDataTable, ByVal SLIPFLG As String) As String

            Try

                Dim lCond As New StringBuilder
                Dim lEscChar As String = Uty_Config.SQLESC

                vCmd.Parameters.Clear()

                '自部門
                If vdt(0).JIBUMON_CD <> String.Empty Then
                    lCond.AppendLine(" AND PAY1.JIBUMON_CD = @jibumon_cd")
                    Dim jibumoncd As New SqlParameter
                    jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.JIBUMON_CDColumn.DataType)
                    jibumoncd.Size = _Dt1.JIBUMON_CDColumn.MaxLength
                    jibumoncd.ParameterName = "@jibumon_cd"
                    jibumoncd.Value = vdt(0).JIBUMON_CD
                    vCmd.Parameters.Add(jibumoncd)

                End If

                '入力日from
                lCond.AppendLine(" AND INPUT_DATE >= @input_date_from")
                Dim inputdate_from As New SqlParameter
                inputdate_from.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.REG_DATEColumn.DataType)
                inputdate_from.Size = _Dt2.REG_DATEColumn.MaxLength
                inputdate_from.ParameterName = "@input_date_from"
                inputdate_from.Value = vdt(0).INPUT_DATE_FROM
                vCmd.Parameters.Add(inputdate_from)


                '入力日to
                lCond.AppendLine(" AND INPUT_DATE <= @input_date_to")
                Dim inputdate_to As New SqlParameter
                inputdate_to.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.REG_DATEColumn.DataType)
                inputdate_to.Size = _Dt2.REG_DATEColumn.MaxLength
                inputdate_to.ParameterName = "@input_date_to"
                inputdate_to.Value = vdt(0).INPUT_DATE_TO
                vCmd.Parameters.Add(inputdate_to)


                'ユーザー
                If vdt(0).EMP_CD <> String.Empty Then
                    lCond.AppendLine(" AND EMP_CD = @emp_cd")
                    Dim empcd As New SqlParameter
                    empcd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.EMP_CDColumn.DataType)
                    empcd.Size = _Dt1.EMP_CDColumn.MaxLength
                    empcd.ParameterName = "@emp_cd"
                    empcd.Value = vdt(0).EMP_CD
                    vCmd.Parameters.Add(empcd)

                End If

                '支払
                If vdt(0).PAYEE <> String.Empty Then
                    lCond.AppendLine(" AND PAYEE LIKE '%' + @payee + '%'")

                    Dim payee As New SqlParameter
                    payee.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.PAYEEColumn.DataType)
                    payee.Size = _Dt2.PAYEEColumn.MaxLength
                    payee.ParameterName = "@payee"
                    payee.Value = vdt(0).PAYEE
                    vCmd.Parameters.Add(payee)

                End If

                'FLG
                lCond.AppendLine(" AND SLIP_FLG = @slip_flg")
                Dim splflg As New SqlParameter
                splflg.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.SLIP_FLGColumn.DataType)
                splflg.Size = _Dt1.SLIP_FLGColumn.MaxLength
                splflg.ParameterName = "@slip_flg"
                splflg.Value = SLIPFLG
                vCmd.Parameters.Add(splflg)


                Return lCond.ToString

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Private Function SetCondition_List(ByRef vCmd As SqlCommand, ByVal vdt As DS_PAYMENT.SearchParam_ListDataTable, ByVal SLIPFLG As String) As String


            Dim lCond As New StringBuilder
            Dim lEscChar As String = Uty_Config.SQLESC

            vCmd.Parameters.Clear()

            '自部門
            If vdt(0).JIBUMON_CD <> String.Empty Then
                lCond.AppendLine(" AND PAY1.JIBUMON_CD = @jibumon_cd")
                Dim jibumoncd As New SqlParameter
                jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.JIBUMON_CDColumn.DataType)
                jibumoncd.Size = _Dt1.JIBUMON_CDColumn.MaxLength
                jibumoncd.ParameterName = "@jibumon_cd"
                jibumoncd.Value = vdt(0).JIBUMON_CD
                vCmd.Parameters.Add(jibumoncd)

            End If


            'マネジメント
            If vdt(0).MANAGE_NO <> String.Empty Then
                lCond.AppendLine(" AND PAY1.MANAGE_NO = @manage_no")
                Dim empcd As New SqlParameter
                empcd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.MANAGE_NOColumn.DataType)
                empcd.Size = _Dt1.MANAGE_NOColumn.MaxLength
                empcd.ParameterName = "@manage_no"
                empcd.Value = vdt(0).MANAGE_NO
                vCmd.Parameters.Add(empcd)

            End If


            'FLG
            lCond.AppendLine(" AND SLIP_FLG = @slip_flg")
            Dim splflg As New SqlParameter
            splflg.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.SLIP_FLGColumn.DataType)
            splflg.Size = _Dt1.SLIP_FLGColumn.MaxLength
            splflg.ParameterName = "@slip_flg"
            splflg.Value = SLIPFLG
            vCmd.Parameters.Add(splflg)


            Return lCond.ToString

        End Function
        Private Function SetCondition_List2(ByRef vCmd As SqlCommand, ByVal vdt As DS_PAYMENT.SearchParam_ListDataTable, ByVal SLIPFLG As String) As String


            Dim lCond As New StringBuilder
            Dim lEscChar As String = Uty_Config.SQLESC

            vCmd.Parameters.Clear()

            '自部門
            If vdt(0).JIBUMON_CD <> String.Empty Then
                lCond.AppendLine(" AND PAY2.JIBUMON_CD = @jibumon_cd")
                Dim jibumoncd As New SqlParameter
                jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.JIBUMON_CDColumn.DataType)
                jibumoncd.Size = _Dt2.JIBUMON_CDColumn.MaxLength
                jibumoncd.ParameterName = "@jibumon_cd"
                jibumoncd.Value = vdt(0).JIBUMON_CD
                vCmd.Parameters.Add(jibumoncd)

            End If


            'マネジメント
            If vdt(0).MANAGE_NO <> String.Empty Then
                lCond.AppendLine(" AND PAY2.MANAGE_NO = @manage_no")
                Dim empcd As New SqlParameter
                empcd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.MANAGE_NOColumn.DataType)
                empcd.Size = _Dt2.MANAGE_NOColumn.MaxLength
                empcd.ParameterName = "@manage_no"
                empcd.Value = vdt(0).MANAGE_NO
                vCmd.Parameters.Add(empcd)

            End If

            '1行
            lCond.AppendLine(" AND PAY2.SUB_SLIP_NO = 1")


            Return lCond.ToString

        End Function

        Public Function SelectData(ByVal vdt As DS_PAYMENT.SearchParamDataTable, ByVal SLIPFLG As String
                                   ) As DS_PAYMENT.CT_SELECTPAYMENTDataTable

            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder

            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     PAY1.JIBUMON_CD as JIBUMON_CD")
            lSql.AppendLine("   , PAY1.MANAGE_NO as MANAGE_NO")
            lSql.AppendLine("   , PAY1.EMP_CD as EMP_CD")
            lSql.AppendLine("   , PAY1.EMP_NM as EMP_NM")
            lSql.AppendLine("   , PAY1.INPUT_DATE as INPUT_DATE")
            lSql.AppendLine("   , PAY2.PAYEE as PAYEE")
            lSql.AppendLine("   , PAY2.SLIP_NO as SLIP_NO")
            lSql.AppendLine("   , PAY2.SUB_SLIP_NO as SUB_SLIP_NO")
            lSql.AppendLine("   , PAY2.BUMON_CD as BUMON_CD")
            lSql.AppendLine("   , PAY2.BUMON_NM as BUMON_NM")
            lSql.AppendLine("   , PAY2.KAMOKU_CD as KAMOKU_CD")
            lSql.AppendLine("   , PAY2.KAMOKU_NM as KAMOKU_NM")
            lSql.AppendLine("   , PAY2.UCHI_CD as UCHI_CD")
            lSql.AppendLine("   , PAY2.UCHI_NM as UCHI_NM")
            lSql.AppendLine("   , PAY2.NOTES as NOTES")
            lSql.AppendLine("   , PAY2.TAX_CD as TAX_CD")
            lSql.AppendLine("   , PAY2.TAX_NM as TAX_NM ")
            lSql.AppendLine("   , PAY2.EXPENSE as EXPENSE")
            lSql.AppendLine("   , PAY2.TAX_AMOUNT as TAX_AMOUNT")
            lSql.AppendLine("   , PAY2.AMOUNT as AMOUNT")
            lSql.AppendLine(" FROM CT_PAYMENT1 PAY1 (NOLOCK)")
            lSql.AppendLine(" LEFT OUTER JOIN CT_PAYMENT2 PAY2 (NOLOCK)")
            lSql.AppendLine("   ON PAY1.JIBUMON_CD = PAY2.JIBUMON_CD ")
            lSql.AppendLine("   AND PAY1.MANAGE_NO = PAY2.MANAGE_NO ")
            lSql.AppendLine(" ")


            Dim lDt As New DS_PAYMENT.CT_SELECTPAYMENTDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()
                    Dim lCondition As String = SetCondition(lCmd, vdt, SLIPFLG)
                    If lCondition <> "" Then
                        lCondition = " WHERE " & lCondition.Substring(5)
                    End If

                    lSql.AppendLine(lCondition)


                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    'データを取得する
                    Using Adapter As New SqlDataAdapter
                        Adapter.SelectCommand = lCmd
                        Adapter.Fill(lDt)
                    End Using 'Adapter

                End Using 'lCmd
            End Using 'lDb

            Return lDt
        End Function
        Public Function MeisaiSelectData(ByVal vdt As DS_PAYMENT.SearchParamDataTable, ByVal SLIPFLG As String) As DS_PAYMENT.CT_MEISAI_DISPDataTable
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder

            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     PAY1.JIBUMON_CD as JIBUMON_CD")
            lSql.AppendLine("   , PAY1.MANAGE_NO as MANAGE_NO")
            lSql.AppendLine("   , PAY1.EMP_CD as EMP_CD")
            lSql.AppendLine("   , PAY1.EMP_NM as EMP_NM")
            lSql.AppendLine("   , PAY1.INPUT_DATE as INPUT_DATE")
            lSql.AppendLine("   , PAY2.PAYEE as PAYEE")
            lSql.AppendLine("   , PAY2.SLIP_NO as SLIP_NO")
            lSql.AppendLine("   , PAY2.SUB_SLIP_NO as SUB_SLIP_NO")
            lSql.AppendLine("   , PAY2.BUMON_CD as BUMON_CD")
            lSql.AppendLine("   , PAY2.BUMON_NM as BUMON_NM")
            lSql.AppendLine("   , PAY2.KAMOKU_CD as KAMOKU_CD")
            lSql.AppendLine("   , PAY2.KAMOKU_NM as KAMOKU_NM")
            lSql.AppendLine("   , PAY2.UCHI_CD as UCHI_CD")
            lSql.AppendLine("   , PAY2.UCHI_NM as UCHI_NM")
            lSql.AppendLine("   , PAY2.NOTES as NOTES")
            lSql.AppendLine("   , PAY2.TAX_CD as TAX_CD")
            lSql.AppendLine("   , PAY2.TAX_NM as TAX_NM ")
            lSql.AppendLine("   , PAY2.EXPENSE as EXPENSE")
            lSql.AppendLine("   , PAY2.TAX_AMOUNT as TAX_AMOUNT")
            lSql.AppendLine("   , PAY2.AMOUNT as AMOUNT")
            lSql.AppendLine(" FROM CT_PAYMENT1 PAY1 (NOLOCK)")
            lSql.AppendLine(" LEFT OUTER JOIN CT_PAYMENT2 PAY2 (NOLOCK)")
            lSql.AppendLine("   ON PAY1.JIBUMON_CD = PAY2.JIBUMON_CD ")
            lSql.AppendLine("   AND PAY1.MANAGE_NO = PAY2.MANAGE_NO ")
            lSql.AppendLine(" ")


            Dim lDt As New DS_PAYMENT.CT_MEISAI_DISPDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()
                    Dim lCondition As String = SetCondition(lCmd, vdt, SLIPFLG)
                    If lCondition <> "" Then
                        lCondition = " WHERE " & lCondition.Substring(5)
                    End If

                    lSql.AppendLine(lCondition)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    'データを取得する
                    Using Adapter As New SqlDataAdapter
                        Adapter.SelectCommand = lCmd
                        Adapter.Fill(lDt)
                    End Using 'Adapter

                End Using 'lCmd
            End Using 'lDb

            Return lDt
        End Function


        Public Function SelectData2(ByVal vdt As DS_PAYMENT.SearchParamDataTable, ByVal SLIPFLG As String
                                   ) As DS_PAYMENT.CT_HEADER_DISPDataTable
            'ヘッダ表示用
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder


            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     PAY1.JIBUMON_CD as JIBUMON_CD")
            lSql.AppendLine("   , PAY1.MANAGE_NO as MANAGE_NO")
            lSql.AppendLine("   , MAX(PAY1.EMP_CD) as EMP_CD")
            lSql.AppendLine("   , MAX(PAY1.EMP_NM) as EMP_NM")
            lSql.AppendLine("   , MAX(PAY1.INPUT_DATE) as INPUT_DATE")
            lSql.AppendLine("   , MAX(PAY2.PAYEE) as PAYEE")
            lSql.AppendLine("   , MAX(PAY2.SLIP_NO) as SLIP_NO")
            lSql.AppendLine("   , SUM(PAY2.EXPENSE) as EXPENSE")
            lSql.AppendLine("   , SUM(PAY2.TAX_AMOUNT) as TAX_AMOUNT")
            lSql.AppendLine("   , SUM(PAY2.AMOUNT) as AMOUNT")
            lSql.AppendLine(" FROM CT_PAYMENT1 PAY1 (NOLOCK)")
            lSql.AppendLine(" LEFT OUTER JOIN CT_PAYMENT2 PAY2 (NOLOCK)")
            lSql.AppendLine("   ON PAY1.JIBUMON_CD = PAY2.JIBUMON_CD ")
            lSql.AppendLine("   AND PAY1.MANAGE_NO = PAY2.MANAGE_NO ")

            Dim lDt As New DS_PAYMENT.CT_HEADER_DISPDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()
                    Dim lCondition As String = SetCondition(lCmd, vdt, SLIPFLG)
                    If lCondition <> "" Then
                        lCondition = " WHERE " & lCondition.Substring(5)
                    End If
                    lCondition = lCondition & " GROUP BY PAY1.JIBUMON_CD ,PAY1.MANAGE_NO"

                    lSql.AppendLine(lCondition)


                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    'データを取得する
                    Using Adapter As New SqlDataAdapter
                        Adapter.SelectCommand = lCmd
                        Adapter.Fill(lDt)
                    End Using 'Adapter

                End Using 'lCmd
            End Using 'lDb

            Return lDt
        End Function


        Public Function SelectData_List(ByVal vdt As DS_PAYMENT.SearchParam_ListDataTable, ByVal SLIPFLG As String) As DS_PAYMENT.CT_SELECTPAYMENTDataTable
            'gridview1明細表示用
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder

            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     PAY1.JIBUMON_CD as JIBUMON_CD")
            lSql.AppendLine("   , PAY1.MANAGE_NO as MANAGE_NO")
            lSql.AppendLine("   , PAY1.EMP_CD as EMP_CD")
            lSql.AppendLine("   , PAY1.EMP_NM as EMP_NM")
            lSql.AppendLine("   , PAY1.INPUT_DATE as INPUT_DATE")
            lSql.AppendLine("   , PAY2.PAYEE as PAYEE")
            lSql.AppendLine("   , PAY2.SLIP_NO as SLIP_NO")
            lSql.AppendLine("   , PAY2.SUB_SLIP_NO as SUB_SLIP_NO")
            lSql.AppendLine("   , PAY2.BUMON_CD as BUMON_CD")
            lSql.AppendLine("   , PAY2.BUMON_NM as BUMON_NM")
            lSql.AppendLine("   , PAY2.KAMOKU_CD as KAMOKU_CD")
            lSql.AppendLine("   , PAY2.KAMOKU_NM as KAMOKU_NM")
            lSql.AppendLine("   , PAY2.UCHI_CD as UCHI_CD")
            lSql.AppendLine("   , PAY2.UCHI_NM as UCHI_NM")
            lSql.AppendLine("   , PAY2.NOTES as NOTES")
            lSql.AppendLine("   , PAY2.TAX_CD as TAX_CD")
            lSql.AppendLine("   , PAY2.TAX_NM as TAX_NM ")
            lSql.AppendLine("   , PAY2.EXPENSE as EXPENSE")
            lSql.AppendLine("   , PAY2.TAX_AMOUNT as TAX_AMOUNT")
            lSql.AppendLine("   , PAY2.AMOUNT as AMOUNT")
            lSql.AppendLine(" FROM CT_PAYMENT1 PAY1 (NOLOCK)")
            lSql.AppendLine(" LEFT OUTER JOIN CT_PAYMENT2 PAY2 (NOLOCK)")
            lSql.AppendLine("   ON PAY1.JIBUMON_CD = PAY2.JIBUMON_CD ")
            lSql.AppendLine("   AND PAY1.MANAGE_NO = PAY2.MANAGE_NO ")
            lSql.AppendLine(" ")


            Dim lDt As New DS_PAYMENT.CT_SELECTPAYMENTDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()
                    Dim lCondition As String = SetCondition_List(lCmd, vdt, SLIPFLG)
                    If lCondition <> "" Then
                        lCondition = " WHERE " & lCondition.Substring(5)
                    End If

                    lSql.AppendLine(lCondition)


                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    'データを取得する
                    Using Adapter As New SqlDataAdapter
                        Adapter.SelectCommand = lCmd
                        Adapter.Fill(lDt)
                    End Using 'Adapter

                End Using 'lCmd
            End Using 'lDb

            Return lDt
        End Function
        Public Function GetShiharai(ByVal vdt As DS_PAYMENT.SearchParam_ListDataTable, ByVal SLIPFLG As String) As DS_PAYMENT.CT_PAYMENT2DataTable
            'gridview1明細表示用
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder

            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     PAY2.JIBUMON_CD as JIBUMON_CD")
            lSql.AppendLine("   , PAY2.MANAGE_NO as MANAGE_NO")
            lSql.AppendLine("   , PAY2.SUB_SLIP_NO as SUB_SLIP_NO")
            lSql.AppendLine("   , PAY2.SLIP_NO as SLIP_NO")
            lSql.AppendLine("   , PAY2.BUMON_CD as BUMON_CD")
            lSql.AppendLine("   , PAY2.BUMON_NM as BUMON_NM")
            lSql.AppendLine("   , PAY2.PAYEE as PAYEE")
            lSql.AppendLine("   , PAY2.KAMOKU_CD as KAMOKU_CD")
            lSql.AppendLine("   , PAY2.KAMOKU_NM as KAMOKU_NM")
            lSql.AppendLine("   , PAY2.UCHI_CD as UCHI_CD")
            lSql.AppendLine("   , PAY2.UCHI_NM as UCHI_NM")
            lSql.AppendLine("   , PAY2.NOTES as NOTES")
            lSql.AppendLine("   , PAY2.TAX_CD as TAX_CD")
            lSql.AppendLine("   , PAY2.TAX_NM as TAX_NM ")
            lSql.AppendLine("   , PAY2.EXPENSE as EXPENSE")
            lSql.AppendLine("   , PAY2.TAX_AMOUNT as TAX_AMOUNT")
            lSql.AppendLine("   , PAY2.AMOUNT as AMOUNT")
            lSql.AppendLine("   , PAY2.REG_DATE as REG_DATE")
            lSql.AppendLine(" FROM CT_PAYMENT2 PAY2 (NOLOCK)")
            lSql.AppendLine(" ")


            Dim lDt As New DS_PAYMENT.CT_PAYMENT2DataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()
                    Dim lCondition As String = SetCondition_List2(lCmd, vdt, SLIPFLG)
                    If lCondition <> "" Then
                        lCondition = " WHERE " & lCondition.Substring(5)
                    End If

                    lSql.AppendLine(lCondition)


                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    'データを取得する
                    Using Adapter As New SqlDataAdapter
                        Adapter.SelectCommand = lCmd
                        Adapter.Fill(lDt)
                    End Using 'Adapter

                End Using 'lCmd
            End Using 'lDb

            Return lDt
        End Function


        Public Function DeleteData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                    ByVal vParam As DS_PAYMENT.SearchParam_ListDataTable) As Integer

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            rCmd.Parameters.Clear()


            'SQL文を生成する
            lSql.AppendLine(" DELETE FROM CT_PAYMENT2 ")
            lSql.AppendLine(" WHERE JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("   AND MANAGE_NO = @manage_no ")


            '検索パラメータ
            Dim jibumoncd As New SqlParameter
            jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.JIBUMON_CDColumn.DataType)
            jibumoncd.Size = _Dt2.JIBUMON_CDColumn.MaxLength
            jibumoncd.ParameterName = "@jibumon_cd"
            rCmd.Parameters.Add(jibumoncd)

            Dim manageno As New SqlParameter
            manageno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.MANAGE_NOColumn.DataType)
            manageno.Size = _Dt2.MANAGE_NOColumn.MaxLength
            manageno.ParameterName = "@manage_no"
            rCmd.Parameters.Add(manageno)


            '値をセットする
            jibumoncd.Value = vParam(0).JIBUMON_CD
            manageno.Value = vParam(0).MANAGE_NO

            rCmd.CommandText = lSql.ToString
            rCmd.Connection = rDb
            lCnt = rCmd.ExecuteNonQuery()

            Return lCnt
        End Function



    End Class
End Namespace
