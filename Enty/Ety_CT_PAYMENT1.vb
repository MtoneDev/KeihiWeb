Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty
Imports KeihiWeb.Report

Namespace Enty
    Public Class Ety_CT_PAYMENT1
        Inherits Ety_Base

        Private _Dt As New DS_PAYMENT.CT_PAYMENT1DataTable

        ''' <summary>
        ''' 現金出納科目別集計表
        ''' </summary>
        ''' <param name="vParam"></param>
        ''' <returns></returns>
        Public Function GetRptData_KamokuBetsu(ByVal vParam As Rpt_Suitou_KamokuBetsu.RptParam) As DS_PAYMENT.RPT_KAMOKUBETUDataTable
            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     PAY1.JIBUMON_CD")
            lSql.AppendLine("   , PAY1.SLIP_FLG")
            lSql.AppendLine("   , PAY2.BUMON_CD")
            lSql.AppendLine("   , PAY2.BUMON_NM")
            lSql.AppendLine("   , PAY2.KAMOKU_CD")
            lSql.AppendLine("   , PAY2.KAMOKU_NM")
            lSql.AppendLine("   , PAY2.UCHI_CD")
            lSql.AppendLine("   , PAY2.UCHI_NM")
            lSql.AppendLine("   , SUM(PAY2.AMOUNT) SUM_AMOUNT ")
            lSql.AppendLine(" FROM CT_PAYMENT1 PAY1 (NOLOCK)")
            lSql.AppendLine(" ")
            lSql.AppendLine(" INNER JOIN CT_PAYMENT2 PAY2 (NOLOCK)")
            lSql.AppendLine("   ON PAY1.JIBUMON_CD = PAY2.JIBUMON_CD ")
            lSql.AppendLine("   AND PAY1.MANAGE_NO = PAY2.MANAGE_NO ")
            lSql.AppendLine(" ")
            lSql.AppendLine(" WHERE PAY1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND PAY1.INPUT_DATE >= @input_date_from")
            lSql.AppendLine("   AND PAY1.INPUT_DATE < @input_date_to")
            lSql.AppendLine(" ")
            lSql.AppendLine(" GROUP BY")
            lSql.AppendLine("     PAY1.JIBUMON_CD")
            lSql.AppendLine("   , PAY1.SLIP_FLG")
            lSql.AppendLine("   , PAY2.BUMON_CD")
            lSql.AppendLine("   , PAY2.BUMON_NM")
            lSql.AppendLine("   , PAY2.KAMOKU_CD")
            lSql.AppendLine("   , PAY2.KAMOKU_NM")
            lSql.AppendLine("   , PAY2.UCHI_CD")
            lSql.AppendLine("   , PAY2.UCHI_NM")
            lSql.AppendLine(" ")
            lSql.AppendLine(" ORDER BY SLIP_FLG DESC, KAMOKU_CD, UCHI_CD ")

            Dim lDt As New DS_PAYMENT.RPT_KAMOKUBETUDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = _Dt.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vParam.JIBUMON_CD
                    lCmd.Parameters.Add(jibumon_cd)
                    '入力日From
                    Dim input_date_from As New SqlParameter
                    input_date_from.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.INPUT_DATEColumn.DataType)
                    input_date_from.Size = _Dt.INPUT_DATEColumn.MaxLength
                    input_date_from.ParameterName = "@input_date_from"
                    input_date_from.Value = vParam.INPUT_DATE_FROM
                    lCmd.Parameters.Add(input_date_from)
                    '入力日To
                    Dim input_date_To As New SqlParameter
                    input_date_To.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.INPUT_DATEColumn.DataType)
                    input_date_To.Size = _Dt.INPUT_DATEColumn.MaxLength
                    input_date_To.ParameterName = "@input_date_To"
                    input_date_To.Value = CDate(vParam.INPUT_DATE_TO).AddDays(1).ToString("yyyy/MM/dd")
                    lCmd.Parameters.Add(input_date_To)

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

        ''' <summary>
        ''' 現金出納帳
        ''' </summary>
        ''' <param name="vParam"></param>
        ''' <returns></returns>
        Public Function GetRptData_Suitou(ByVal vParam As Rpt_Suitou.RptParam) As DS_PAYMENT.RPT_SUITOUDataTable
            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT")
            lSql.AppendLine("	  T1.JIBUMON_CD")
            lSql.AppendLine("	, T1.MANAGE_NO ")
            lSql.AppendLine("	, T1.SLIP_NO ")
            lSql.AppendLine("	, T2.SUB_SLIP_NO ")
            lSql.AppendLine("	, T1.SLIP_FLG ")
            lSql.AppendLine("	, T1.INPUT_DATE ")
            lSql.AppendLine("	, T2.PAYEE")
            lSql.AppendLine("	, T2.NOTES ")
            lSql.AppendLine("	, T2.KAMOKU_CD ")
            lSql.AppendLine("	, T2.KAMOKU_NM ")
            lSql.AppendLine("	, T2.UCHI_CD ")
            lSql.AppendLine("	, T2.BUMON_CD ")
            lSql.AppendLine("	, T2.AMOUNT ")
            lSql.AppendLine("  FROM CT_PAYMENT1 AS T1,CT_PAYMENT2 AS T2")
            lSql.AppendLine(" WHERE T1.MANAGE_NO = T2.MANAGE_NO  ")
            lSql.AppendLine("   AND T1.JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("   AND T2.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND  INPUT_DATE >= @input_date_from")
            lSql.AppendLine("   AND  INPUT_DATE < @input_date_to")
            lSql.AppendLine(" ORDER BY T1.SLIP_FLG DESC, T1.SLIP_NO, INPUT_DATE, BUMON_CD, SUB_SLIP_NO")

            Dim lDt As New DS_PAYMENT.RPT_SUITOUDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = _Dt.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vParam.JIBUMON_CD
                    lCmd.Parameters.Add(jibumon_cd)
                    '入力日From
                    Dim input_date_from As New SqlParameter
                    input_date_from.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.INPUT_DATEColumn.DataType)
                    input_date_from.Size = _Dt.INPUT_DATEColumn.MaxLength
                    input_date_from.ParameterName = "@input_date_from"
                    input_date_from.Value = vParam.INPUT_DATE_FROM
                    lCmd.Parameters.Add(input_date_from)
                    '入力日To
                    Dim input_date_To As New SqlParameter
                    input_date_To.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.INPUT_DATEColumn.DataType)
                    input_date_To.Size = _Dt.INPUT_DATEColumn.MaxLength
                    input_date_To.ParameterName = "@input_date_To"
                    input_date_To.Value = CDate(vParam.INPUT_DATE_TO).AddDays(1).ToString("yyyy/MM/dd")
                    lCmd.Parameters.Add(input_date_To)

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

        Public Function GetDenpyo_NO(ByVal vBumon_CD As String, ByVal vInput_YM As String) As DS_PAYMENT.CT_PAYMENT1_DENPYODataTable

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     PAY1.JIBUMON_CD")
            lSql.AppendLine("   , PAY1.INPUT_YM")
            lSql.AppendLine("   , MAX(PAY1.SLIP_NO) as MAXSLIP_NO")
            lSql.AppendLine(" FROM CT_PAYMENT1 PAY1 (NOLOCK)")
            lSql.AppendLine(" WHERE PAY1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND PAY1.INPUT_YM = @input_ym")
            lSql.AppendLine("   GROUP BY PAY1.JIBUMON_CD,PAY1.INPUT_YM")

            Dim lDt As New DS_PAYMENT.CT_PAYMENT1_DENPYODataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = _Dt.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vBumon_CD
                    lCmd.Parameters.Add(jibumon_cd)
                    '入力日
                    Dim input_date_To As New SqlParameter
                    input_date_To.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.INPUT_YMColumn.DataType)
                    input_date_To.Size = _Dt.INPUT_YMColumn.MaxLength
                    input_date_To.ParameterName = "@input_ym"
                    input_date_To.Value = vInput_YM
                    lCmd.Parameters.Add(input_date_To)

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
        Public Function ExistDenpyo_NO(ByVal vBumon_CD As String, ByVal vInput_YM As String, ByVal vDen_NO As String) As DS_PAYMENT.CT_PAYMENT1DataTable

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT * ")
            lSql.AppendLine(" FROM CT_PAYMENT1 PAY1 (NOLOCK)")
            lSql.AppendLine(" WHERE PAY1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND PAY1.INPUT_YM = @input_ym")
            lSql.AppendLine("   AND PAY1.SLIP_NO = @slip_no")

            Dim lDt As New DS_PAYMENT.CT_PAYMENT1DataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = _Dt.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vBumon_CD
                    lCmd.Parameters.Add(jibumon_cd)
                    '入力日
                    Dim input_date_To As New SqlParameter
                    input_date_To.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.INPUT_YMColumn.DataType)
                    input_date_To.Size = _Dt.INPUT_YMColumn.MaxLength
                    input_date_To.ParameterName = "@input_ym"
                    input_date_To.Value = vInput_YM
                    lCmd.Parameters.Add(input_date_To)

                    '伝票No
                    Dim slipno As New SqlParameter
                    slipno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.SLIP_NOColumn.DataType)
                    slipno.Size = _Dt.SLIP_NOColumn.MaxLength
                    slipno.ParameterName = "@slip_no"
                    slipno.Value = vDen_NO
                    lCmd.Parameters.Add(slipno)


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

        Public Function InsertData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                     vSeihin As DS_PAYMENT.CT_PAYMENT1DataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO CT_PAYMENT1 (")

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
                jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
                jibumoncd.Size = _Dt.JIBUMON_CDColumn.MaxLength
                jibumoncd.ParameterName = "@jibumon_cd"
                rCmd.Parameters.Add(jibumoncd)

                Dim manageno As New SqlParameter
                manageno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.MANAGE_NOColumn.DataType)
                manageno.Size = _Dt.MANAGE_NOColumn.MaxLength
                manageno.ParameterName = "@manage_no"
                rCmd.Parameters.Add(manageno)

                Dim inputym As New SqlParameter
                inputym.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.INPUT_YMColumn.DataType)
                inputym.Size = _Dt.INPUT_YMColumn.MaxLength
                inputym.ParameterName = "@input_ym"
                rCmd.Parameters.Add(inputym)

                Dim slipno As New SqlParameter
                slipno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.SLIP_NOColumn.DataType)
                slipno.Size = _Dt.SLIP_NOColumn.MaxLength
                slipno.ParameterName = "@slip_no"
                rCmd.Parameters.Add(slipno)

                Dim empcd As New SqlParameter
                empcd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.EMP_CDColumn.DataType)
                empcd.Size = _Dt.EMP_CDColumn.MaxLength
                empcd.ParameterName = "@emp_cd"
                rCmd.Parameters.Add(empcd)

                Dim empnm As New SqlParameter
                empnm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.EMP_NMColumn.DataType)
                empnm.Size = _Dt.EMP_NMColumn.MaxLength
                empnm.ParameterName = "@emp_nm"
                rCmd.Parameters.Add(empnm)

                Dim inputdate As New SqlParameter
                inputdate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.INPUT_DATEColumn.DataType)
                inputdate.Size = _Dt.INPUT_DATEColumn.MaxLength
                inputdate.ParameterName = "@input_date"
                rCmd.Parameters.Add(inputdate)

                Dim regdate As New SqlParameter
                regdate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.REG_DATEColumn.DataType)
                regdate.Size = _Dt.REG_DATEColumn.MaxLength
                regdate.ParameterName = "@reg_date"
                rCmd.Parameters.Add(regdate)


                Dim slipflg As New SqlParameter
                slipflg.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.SLIP_FLGColumn.DataType)
                slipflg.Size = _Dt.SLIP_FLGColumn.MaxLength
                slipflg.ParameterName = "@slip_flg"
                rCmd.Parameters.Add(slipflg)

                '値をセットする

                jibumoncd.Value = vSeihin(0).JIBUMON_CD
                manageno.Value = vSeihin(0).MANAGE_NO
                inputym.Value = vSeihin(0).INPUT_YM
                slipno.Value = vSeihin(0).SLIP_NO
                empcd.Value = vSeihin(0).EMP_CD
                empnm.Value = vSeihin(0).EMP_NM
                inputdate.Value = vSeihin(0).INPUT_DATE
                regdate.Value = vSeihin(0).REG_DATE
                slipflg.Value = vSeihin(0).SLIP_FLG


                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                lCnt = rCmd.ExecuteNonQuery()

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function


        Public Function DeleteData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                    ByVal vParam As DS_PAYMENT.SearchParam_ListDataTable) As Integer

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            rCmd.Parameters.Clear()


            'SQL文を生成する
            lSql.AppendLine(" DELETE FROM CT_PAYMENT1 ")
            lSql.AppendLine(" WHERE JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("   AND MANAGE_NO = @manage_no ")


            '検索パラメータ
            Dim jibumoncd As New SqlParameter
            jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
            jibumoncd.Size = _Dt.JIBUMON_CDColumn.MaxLength
            jibumoncd.ParameterName = "@jibumon_cd"
            rCmd.Parameters.Add(jibumoncd)

            Dim manageno As New SqlParameter
            manageno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.MANAGE_NOColumn.DataType)
            manageno.Size = _Dt.MANAGE_NOColumn.MaxLength
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
