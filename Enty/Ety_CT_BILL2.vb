Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty
Imports KeihiWeb.Report

Namespace Enty
    Public Class Ety_CT_BILL2
        Inherits Ety_Base

        Private _Dt1 As New DS_BILL.CT_BILL1DataTable
        Private _Dt2 As New DS_BILL.CT_BILL2DataTable

        Public Function SumKingaku(ByVal vJibumonCd As String, ByVal vStartDate As String, ByVal vEndDate As String, vSlipFlg As Integer) As Decimal
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT")
            lSql.AppendLine("   ISNULL(SUM(AMOUNT), 0) AS SUM_AMOUNT FROM CT_BILL2 (NOLOCK)")
            lSql.AppendLine("   WHERE JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("     AND EXISTS ")
            lSql.AppendLine("     (SELECT * FROM CT_BILL1 (NOLOCK) ")
            lSql.AppendLine("      WHERE CT_BILL1.JIBUMON_CD = CT_BILL2.JIBUMON_CD ")
            lSql.AppendLine("        AND CT_BILL1.MANAGE_NO = CT_BILL2.MANAGE_NO ")
            lSql.AppendLine("        AND CT_BILL1.INPUT_DATE >= @input_date_from")
            lSql.AppendLine("        AND CT_BILL1.INPUT_DATE < @input_date_To")
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
                    input_date_To.Value = vEndDate
                    lCmd.Parameters.Add(input_date_To)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    lDb.Open()
                    lRst = lCmd.ExecuteScalar()

                End Using 'lCmd
            End Using 'lDb

            Return lRst
        End Function

        Public Function InsertData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                     vSeihin As DS_BILL.CT_BILL2DataTable) As Integer

            Try

                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt2)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO CT_BILL2 (")

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

                Dim subbillno As New SqlParameter
                subbillno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.SUB_BILL_NOColumn.DataType)
                subbillno.Size = _Dt2.SUB_BILL_NOColumn.MaxLength
                subbillno.ParameterName = "@sub_bill_no"
                rCmd.Parameters.Add(subbillno)


                Dim billno As New SqlParameter
                billno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.BILL_NOColumn.DataType)
                billno.Size = _Dt2.BILL_NOColumn.MaxLength
                billno.ParameterName = "@bill_no"
                rCmd.Parameters.Add(billno)

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
                taxamout.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.TAXColumn.DataType)
                taxamout.Size = _Dt2.TAXColumn.MaxLength
                taxamout.ParameterName = "@tax"
                rCmd.Parameters.Add(taxamout)


                Dim regdate As New SqlParameter
                regdate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt2.REG_DATEColumn.DataType)
                regdate.Size = _Dt2.REG_DATEColumn.MaxLength
                regdate.ParameterName = "@reg_date"
                rCmd.Parameters.Add(regdate)


                '値をセットする
                For Each row As DS_BILL.CT_BILL2Row In vSeihin

                    jibumoncd.Value = row.JIBUMON_CD
                    manageno.Value = row.MANAGE_NO
                    subbillno.Value = row.SUB_BILL_NO
                    billno.Value = row.BILL_NO
                    bumoncd.Value = row.BUMON_CD
                    bumonnm.Value = row.BUMON_NM
                    kamokucd.Value = row.KAMOKU_CD
                    kamokunm.Value = row.KAMOKU_NM
                    uchicd.Value = row.UCHI_CD
                    uchinm.Value = row.UCHI_NM
                    notes.Value = row.NOTES
                    taxcd.Value = row.TAX_CD
                    taxnm.Value = row.TAX_NM
                    expense.Value = row.EXPENSE
                    taxamout.Value = row.TAX
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

        Private Function SetCondition(ByRef vCmd As SqlCommand, ByVal vdt As DS_BILL.SearchParamDataTable) As String

            Try

                Dim lCond As New StringBuilder
                Dim lEscChar As String = Uty_Config.SQLESC

                vCmd.Parameters.Clear()

                '自部門
                If vdt(0).JIBUMON_CD <> String.Empty Then
                    lCond.AppendLine(" AND BILL1.JIBUMON_CD = @jibumon_cd")
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
                If vdt(0).TRADE_CD <> String.Empty Then
                    lCond.AppendLine(" AND TRADE_CD = @trade_cd")
                    Dim tradecd As New SqlParameter
                    tradecd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.TRADE_CDColumn.DataType)
                    tradecd.Size = _Dt1.TRADE_CDColumn.MaxLength
                    tradecd.ParameterName = "@trade_cd"
                    tradecd.Value = vdt(0).TRADE_CD
                    vCmd.Parameters.Add(tradecd)

                End If


                Return lCond.ToString

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Private Function SetCondition_List(ByRef vCmd As SqlCommand, ByVal vdt As DS_BILL.SearchParam_ListDataTable) As String


            Dim lCond As New StringBuilder
            Dim lEscChar As String = Uty_Config.SQLESC

            vCmd.Parameters.Clear()

            '自部門
            If vdt(0).JIBUMON_CD <> String.Empty Then
                lCond.AppendLine(" AND BILL1.JIBUMON_CD = @jibumon_cd")
                Dim jibumoncd As New SqlParameter
                jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.JIBUMON_CDColumn.DataType)
                jibumoncd.Size = _Dt1.JIBUMON_CDColumn.MaxLength
                jibumoncd.ParameterName = "@jibumon_cd"
                jibumoncd.Value = vdt(0).JIBUMON_CD
                vCmd.Parameters.Add(jibumoncd)

            End If


            'マネジメント
            If vdt(0).MANAGE_NO <> String.Empty Then
                lCond.AppendLine(" AND BILL1.MANAGE_NO = @manage_no")
                Dim empcd As New SqlParameter
                empcd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt1.MANAGE_NOColumn.DataType)
                empcd.Size = _Dt1.MANAGE_NOColumn.MaxLength
                empcd.ParameterName = "@manage_no"
                empcd.Value = vdt(0).MANAGE_NO
                vCmd.Parameters.Add(empcd)

            End If


            Return lCond.ToString

        End Function

        Public Function SelectData(ByVal vdt As DS_BILL.SearchParamDataTable) As DS_BILL.CT_SELECTBILLDataTable

            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder

            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     BILL1.JIBUMON_CD as JIBUMON_CD")
            lSql.AppendLine("   , BILL1.MANAGE_NO as MANAGE_NO")
            lSql.AppendLine("   , BILL1.TRADE_CD as TRADE_CD")
            lSql.AppendLine("   , BILL1.TRADE_NM as TRADE_NM")
            lSql.AppendLine("   , BILL1.INPUT_DATE as INPUT_DATE")
            lSql.AppendLine("   , BILL2.BILL_NO as BILL_NO")
            lSql.AppendLine("   , BILL2.SUB_BILL_NO as SUB_BILL_NO")
            lSql.AppendLine("   , BILL2.BUMON_CD as BUMON_CD")
            lSql.AppendLine("   , BILL2.BUMON_NM as BUMON_NM")
            lSql.AppendLine("   , BILL2.KAMOKU_CD as KAMOKU_CD")
            lSql.AppendLine("   , BILL2.KAMOKU_NM as KAMOKU_NM")
            lSql.AppendLine("   , BILL2.UCHI_CD as UCHI_CD")
            lSql.AppendLine("   , BILL2.UCHI_NM as UCHI_NM")
            lSql.AppendLine("   , BILL2.NOTES as NOTES")
            lSql.AppendLine("   , BILL2.TAX_CD as TAX_CD")
            lSql.AppendLine("   , BILL2.TAX_NM as TAX_NM ")
            lSql.AppendLine("   , BILL2.EXPENSE as EXPENSE")
            lSql.AppendLine("   , BILL2.TAX as TAX")
            lSql.AppendLine("   , BILL2.EXPENSE  + BILL2.TAX as AMOUNT")
            lSql.AppendLine(" FROM CT_BILL1 BILL1 (NOLOCK)")
            lSql.AppendLine(" LEFT OUTER JOIN CT_BILL2 BILL2 (NOLOCK)")
            lSql.AppendLine("   ON BILL1.JIBUMON_CD = BILL2.JIBUMON_CD ")
            lSql.AppendLine("   AND BILL1.MANAGE_NO = BILL2.MANAGE_NO ")
            lSql.AppendLine(" ")


            Dim lDt As New DS_BILL.CT_SELECTBILLDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()
                    Dim lCondition As String = SetCondition(lCmd, vdt)
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
        Public Function MeisaiSelectData(ByVal vdt As DS_BILL.SearchParamDataTable) As DS_BILL.CT_MEISAI_DISPDataTable
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder

            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     BILL1.JIBUMON_CD as JIBUMON_CD")
            lSql.AppendLine("   , BILL1.MANAGE_NO as MANAGE_NO")
            lSql.AppendLine("   , BILL1.TRADE_CD as TRADE_CD")
            lSql.AppendLine("   , BILL1.TRADE_NM as TRADE_NM")
            lSql.AppendLine("   , BILL1.INPUT_DATE as INPUT_DATE")
            lSql.AppendLine("   , BILL1.CIR_NO as CIR_NO")
            lSql.AppendLine("   , BILL2.BILL_NO as BILL_NO")
            lSql.AppendLine("   , BILL2.SUB_BILL_NO as SUB_BILL_NO")
            lSql.AppendLine("   , BILL2.BUMON_CD as BUMON_CD")
            lSql.AppendLine("   , BILL2.BUMON_NM as BUMON_NM")
            lSql.AppendLine("   , BILL2.KAMOKU_CD as KAMOKU_CD")
            lSql.AppendLine("   , BILL2.KAMOKU_NM as KAMOKU_NM")
            lSql.AppendLine("   , BILL2.UCHI_CD as UCHI_CD")
            lSql.AppendLine("   , BILL2.UCHI_NM as UCHI_NM")
            lSql.AppendLine("   , BILL2.NOTES as NOTES")
            lSql.AppendLine("   , BILL2.TAX_CD as TAX_CD")
            lSql.AppendLine("   , BILL2.TAX_NM as TAX_NM ")
            lSql.AppendLine("   , BILL2.EXPENSE as EXPENSE")
            lSql.AppendLine("   , BILL2.TAX as TAX")
            lSql.AppendLine("   , BILL2.EXPENSE + BILL2.TAX as AMOUNT")
            lSql.AppendLine(" FROM CT_BILL1 BILL1 (NOLOCK)")
            lSql.AppendLine(" LEFT OUTER JOIN CT_BILL2 BILL2 (NOLOCK)")
            lSql.AppendLine("   ON BILL1.JIBUMON_CD = BILL2.JIBUMON_CD ")
            lSql.AppendLine("   AND BILL1.MANAGE_NO = BILL2.MANAGE_NO ")
            lSql.AppendLine(" ")


            Dim lDt As New DS_BILL.CT_MEISAI_DISPDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()
                    Dim lCondition As String = SetCondition(lCmd, vdt)
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


        Public Function SelectData2(ByVal vdt As DS_BILL.SearchParamDataTable) As DS_BILL.CT_HEADER_DISPDataTable
            'ヘッダ表示用
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder


            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     BILL1.JIBUMON_CD as JIBUMON_CD")
            lSql.AppendLine("   , BILL1.MANAGE_NO as MANAGE_NO")
            lSql.AppendLine("   , MAX(CONVERT(nvarchar,BILL1.CIR_NO)) as CIR_NO")
            lSql.AppendLine("   , MAX(BILL1.TRADE_CD) as TRADE_CD")
            lSql.AppendLine("   , MAX(BILL1.TRADE_NM) as TRADE_NM")
            lSql.AppendLine("   , MAX(BILL1.INPUT_DATE) as INPUT_DATE")
            lSql.AppendLine("   , MAX(BILL2.BILL_NO) as BILL_NO")
            lSql.AppendLine("   , SUM(BILL2.EXPENSE) as EXPENSE")
            lSql.AppendLine("   , SUM(BILL2.TAX) as TAX")
            lSql.AppendLine("   , SUM(BILL2.TAX) + SUM(BILL2.EXPENSE) as AMOUNT")
            lSql.AppendLine(" FROM CT_BILL1 BILL1 (NOLOCK)")
            lSql.AppendLine(" LEFT OUTER JOIN CT_BILL2 BILL2 (NOLOCK)")
            lSql.AppendLine("   ON BILL1.JIBUMON_CD = BILL2.JIBUMON_CD ")
            lSql.AppendLine("   AND BILL1.MANAGE_NO = BILL2.MANAGE_NO ")

            Dim lDt As New DS_BILL.CT_HEADER_DISPDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()
                    Dim lCondition As String = SetCondition(lCmd, vdt)
                    If lCondition <> "" Then
                        lCondition = " WHERE " & lCondition.Substring(5)
                    End If
                    lCondition = lCondition & " GROUP BY BILL1.JIBUMON_CD ,BILL1.MANAGE_NO"

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


        Public Function SelectData_List(ByVal vdt As DS_BILL.SearchParam_ListDataTable) As DS_BILL.CT_SELECTBILLDataTable
            'gridview1明細表示用
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder

            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     BILL1.JIBUMON_CD as JIBUMON_CD")
            lSql.AppendLine("   , BILL1.MANAGE_NO as MANAGE_NO")
            lSql.AppendLine("   , BILL1.CIR_NO as CIR_NO")
            lSql.AppendLine("   , BILL1.TRADE_CD as TRADE_CD")
            lSql.AppendLine("   , BILL1.TRADE_NM as TRADE_NM")
            lSql.AppendLine("   , BILL1.INPUT_DATE as INPUT_DATE")
            lSql.AppendLine("   , BILL2.BILL_NO as BILL_NO")
            lSql.AppendLine("   , BILL2.SUB_BILL_NO as SUB_BILL_NO")
            lSql.AppendLine("   , BILL2.BUMON_CD as BUMON_CD")
            lSql.AppendLine("   , BILL2.BUMON_NM as BUMON_NM")
            lSql.AppendLine("   , BILL2.KAMOKU_CD as KAMOKU_CD")
            lSql.AppendLine("   , BILL2.KAMOKU_NM as KAMOKU_NM")
            lSql.AppendLine("   , BILL2.UCHI_CD as UCHI_CD")
            lSql.AppendLine("   , BILL2.UCHI_NM as UCHI_NM")
            lSql.AppendLine("   , BILL2.NOTES as NOTES")
            lSql.AppendLine("   , BILL2.TAX_CD as TAX_CD")
            lSql.AppendLine("   , BILL2.TAX_NM as TAX_NM ")
            lSql.AppendLine("   , BILL2.EXPENSE as EXPENSE")
            lSql.AppendLine("   , BILL2.TAX as TAX_AMOUNT")
            lSql.AppendLine("   , (BILL2.EXPENSE + BILL2.TAX) as AMOUNT")
            lSql.AppendLine(" FROM CT_BILL1 BILL1 (NOLOCK)")
            lSql.AppendLine(" LEFT OUTER JOIN CT_BILL2 BILL2 (NOLOCK)")
            lSql.AppendLine("   ON BILL1.JIBUMON_CD = BILL2.JIBUMON_CD ")
            lSql.AppendLine("   AND BILL1.MANAGE_NO = BILL2.MANAGE_NO ")
            lSql.AppendLine(" ")


            Dim lDt As New DS_BILL.CT_SELECTBILLDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()
                    Dim lCondition As String = SetCondition_List(lCmd, vdt)
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
                                    ByVal vParam As DS_BILL.SearchParam_ListDataTable) As Integer

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            rCmd.Parameters.Clear()


            'SQL文を生成する
            lSql.AppendLine(" DELETE FROM CT_BILL2 ")
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
