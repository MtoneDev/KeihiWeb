Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty
Imports KeihiWeb.Ctrl

Namespace Enty
    Public Class Ety_GLOVIA
        Inherits Ety_Base

        Private Function ExpenseSum(ByVal vBumonCd As String, ByVal vStartDate As String, ByVal vEndDate As String,
                                    ByRef rSum1 As Decimal, ByRef rSum2 As Decimal) As Boolean
            Dim lRst As Boolean = False
            Dim lSql As New StringBuilder

            rSum1 = 0
            rSum2 = 0

            vEndDate = CDate(vEndDate).AddDays(1).ToString("yyyy/MM/dd")

            ' *** SQL文 貸借毎に集計
            lSql.AppendLine(" SELECT SLIP_FLG , (SUM(EXPENSE) + SUM(TAX_AMOUNT)) AS SUM_EXPENSE  ")
            lSql.AppendLine("   FROM CT_PAYMENT1 As T1, CT_PAYMENT2 As T2")
            lSql.AppendLine(" WHERE T1.MANAGE_NO = T2.MANAGE_NO  ")
            lSql.AppendLine("   AND T1.JIBUMON_CD = T2.JIBUMON_CD ")
            lSql.AppendLine("   AND INPUT_DATE >= @start_date")
            lSql.AppendLine("   AND INPUT_DATE < @end_date")
            lSql.AppendLine("   AND T1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND T2.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine(" GROUP BY T1.JIBUMON_CD,SLIP_FLG ")

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand

                    '検索条件設定
                    lCmd.Parameters.Clear()

                    Dim start_date As New SqlParameter
                    start_date.SqlDbType = SqlDbType.DateTime
                    start_date.Size = -1
                    start_date.ParameterName = "@start_date"
                    start_date.Value = vStartDate
                    lCmd.Parameters.Add(start_date)

                    Dim end_date As New SqlParameter
                    end_date.SqlDbType = SqlDbType.DateTime
                    end_date.Size = -1
                    end_date.ParameterName = "@end_date"
                    end_date.Value = vEndDate
                    lCmd.Parameters.Add(end_date)

                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = SqlDbType.NVarChar
                    jibumon_cd.Size = 5
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vBumonCd
                    lCmd.Parameters.Add(jibumon_cd)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    'データを取得する
                    lDb.Open()
                    Dim reader As DbDataReader = lCmd.ExecuteReader(CommandBehavior.CloseConnection)
                    Do While reader.Read()
                        If reader("SLIP_FLG") = 1 Then
                            rSum1 = reader("SUM_EXPENSE")
                        Else
                            rSum2 = reader("SUM_EXPENSE")
                        End If
                    Loop

                End Using 'cmd
            End Using 'db

            Return lRst
        End Function

        ''' <summary>
        ''' GLOVIA出力する出納帳のデータを集計する
        ''' </summary>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vStartDate">締日開始日</param>
        ''' <param name="vEndDate">締日終了日</param>
        ''' <returns></returns>
        Public Function GetConvData_Suitou(ByVal vJibumonCd As String, ByVal vStartDate As String, ByVal vEndDate As String) As DS_GLOVIA.CONV_SUITOUDataTable

            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT ")
            lSql.AppendLine("    T1.JIBUMON_CD")
            lSql.AppendLine("  , T1.SLIP_FLG")
            lSql.AppendLine("  , T2.BUMON_CD")
            lSql.AppendLine("  , KAMOKU_CD")
            lSql.AppendLine("  , UCHI_CD")
            lSql.AppendLine("  , SUM(AMOUNT) AS SUM_EXPENSE ")
            'lSql.AppendLine("  , '出納:' + ISNULL((SELECT TOP 1 CONVERT(VARCHAR(34), NOTES) NOTES FROM CT_PAYMENT2 AS TN2 ")
            lSql.AppendLine("  , '出納:' + ISNULL((SELECT TOP 1 NOTES FROM CT_PAYMENT2 AS TN2 ")
            lSql.AppendLine("      WHERE TN2.JIBUMON_CD  = @jibumon_cd")
            lSql.AppendLine("        AND TN2.BUMON_CD    = T2.BUMON_CD ")
            lSql.AppendLine("        AND TN2.KAMOKU_CD   = T2.KAMOKU_CD  ")
            lSql.AppendLine("        AND TN2.UCHI_CD     = T2.UCHI_CD  ")
            lSql.AppendLine("        AND TN2.MANAGE_NO   = MIN(T2.MANAGE_NO) ")
            lSql.AppendLine("   ), '') AS MIN_NOTES ")
            lSql.AppendLine("  FROM CT_PAYMENT1 AS T1, CT_PAYMENT2 AS T2")
            lSql.AppendLine("  WHERE T1.MANAGE_NO  =  T2.MANAGE_NO  ")
            lSql.AppendLine("    AND T1.JIBUMON_CD =  T2.JIBUMON_CD ")
            lSql.AppendLine("    AND T1.INPUT_DATE >= @start_date")
            lSql.AppendLine("    AND T1.INPUT_DATE <  @end_date")
            lSql.AppendLine("    AND T1.JIBUMON_CD =  @jibumon_cd")
            lSql.AppendLine("    AND T2.JIBUMON_CD =  @jibumon_cd")
            lSql.AppendLine("  GROUP BY T1.JIBUMON_CD, SLIP_FLG, T2.BUMON_CD, KAMOKU_CD, UCHI_CD")
            lSql.AppendLine("")
            lSql.AppendLine(" UNION ALL ")
            lSql.AppendLine("")
            lSql.AppendLine(" SELECT ")
            lSql.AppendLine("     T1.JIBUMON_CD")
            lSql.AppendLine("   , (CASE WHEN T1.SLIP_FLG = 2 THEN 1 ELSE 2 END) AS SLIP_FLG")
            lSql.AppendLine("   , T1.JIBUMON_CD BUMON_CD")
            lSql.AppendLine("   , '10020' AS KAMOKU_CD")
            lSql.AppendLine("   , '' AS UCHI_CD")
            lSql.AppendLine("   , (SUM(T2.EXPENSE) + SUM(T2.TAX_AMOUNT)) AS SUM_EXPENSE")
            lSql.AppendLine("   , '' AS MIN_NOTES  ")
            lSql.AppendLine("  FROM CT_PAYMENT1 AS T1,CT_PAYMENT2 AS T2 ")
            lSql.AppendLine(" WHERE T1.MANAGE_NO = T2.MANAGE_NO  ")
            lSql.AppendLine("   AND T1.JIBUMON_CD = T2.JIBUMON_CD ")
            lSql.AppendLine("   AND T1.INPUT_DATE >= @start_date")
            lSql.AppendLine("   AND T1.INPUT_DATE < @end_date")
            lSql.AppendLine("   AND T1.JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("   AND T2.JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine(" GROUP BY T1.JIBUMON_CD, T1.SLIP_FLG")
            lSql.AppendLine("")
            lSql.AppendLine(" ORDER BY 1,2,4,3")

            Dim lDt As New DS_GLOVIA.CONV_SUITOUDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(lDt.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = lDt.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vJibumonCd
                    lCmd.Parameters.Add(jibumon_cd)
                    '締日開始日
                    Dim start_date As New SqlParameter
                    start_date.SqlDbType = SqlDbType.DateTime
                    start_date.Size = -1
                    start_date.ParameterName = "@start_date"
                    start_date.Value = vStartDate
                    lCmd.Parameters.Add(start_date)
                    '締日終了日
                    Dim end_date As New SqlParameter
                    end_date.SqlDbType = SqlDbType.DateTime
                    end_date.Size = -1
                    end_date.ParameterName = "@end_date"
                    end_date.Value = CDate(vEndDate).AddDays(1).ToString("yyyy/MM/dd")
                    lCmd.Parameters.Add(end_date)

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
        ''' Freee出力する出納帳のデータを集計する
        ''' </summary>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vStartDate">締日開始日</param>
        ''' <param name="vEndDate">締日終了日</param>
        ''' <returns></returns>
        Public Function GetConvData_Suitou_Freee(ByVal vJibumonCd As String, ByVal vStartDate As String, ByVal vEndDate As String) As DS_GLOVIA.CONV_SUITOU_FDataTable

            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT ")
            lSql.AppendLine("    T1.JIBUMON_CD")
            lSql.AppendLine("  , T1.SLIP_FLG")
            lSql.AppendLine("  , T2.BUMON_CD")
            lSql.AppendLine("  , KAMOKU_CD")
            lSql.AppendLine("  , UCHI_CD")
            lSql.AppendLine("  , SUM(AMOUNT) AS SUM_EXPENSE ")
            lSql.AppendLine("  , SUM(TAX_AMOUNT) AS SUM_TAX ")
            'lSql.AppendLine("  , '出納:' + ISNULL((SELECT TOP 1 CONVERT(VARCHAR(34), NOTES) NOTES FROM CT_PAYMENT2 AS TN2 ")
            lSql.AppendLine("  , '出納:' + ISNULL((SELECT TOP 1 NOTES FROM CT_PAYMENT2 AS TN2 ")
            lSql.AppendLine("      WHERE TN2.JIBUMON_CD  = @jibumon_cd")
            lSql.AppendLine("        AND TN2.BUMON_CD    = T2.BUMON_CD ")
            lSql.AppendLine("        AND TN2.KAMOKU_CD   = T2.KAMOKU_CD  ")
            lSql.AppendLine("        AND TN2.UCHI_CD     = T2.UCHI_CD  ")
            lSql.AppendLine("        AND TN2.MANAGE_NO   = MIN(T2.MANAGE_NO) ")
            lSql.AppendLine("   ), '') AS MIN_NOTES ")
            lSql.AppendLine("  FROM CT_PAYMENT1 AS T1, CT_PAYMENT2 AS T2")
            lSql.AppendLine("  WHERE T1.MANAGE_NO  =  T2.MANAGE_NO  ")
            lSql.AppendLine("    AND T1.JIBUMON_CD =  T2.JIBUMON_CD ")
            lSql.AppendLine("    AND T1.INPUT_DATE >= @start_date")
            lSql.AppendLine("    AND T1.INPUT_DATE <  @end_date")
            lSql.AppendLine("    AND T1.JIBUMON_CD =  @jibumon_cd")
            lSql.AppendLine("    AND T2.JIBUMON_CD =  @jibumon_cd")
            lSql.AppendLine("  GROUP BY T1.JIBUMON_CD, SLIP_FLG, T2.BUMON_CD, KAMOKU_CD, UCHI_CD")
            lSql.AppendLine("")
            lSql.AppendLine(" UNION ALL ")
            lSql.AppendLine("")
            lSql.AppendLine(" SELECT ")
            lSql.AppendLine("     T1.JIBUMON_CD")
            lSql.AppendLine("   , (CASE WHEN T1.SLIP_FLG = 2 THEN 1 ELSE 2 END) AS SLIP_FLG")
            lSql.AppendLine("   , T1.JIBUMON_CD BUMON_CD")
            lSql.AppendLine("   , '10020' AS KAMOKU_CD")
            lSql.AppendLine("   , '' AS UCHI_CD")
            lSql.AppendLine("   , (SUM(T2.EXPENSE) + SUM(T2.TAX_AMOUNT)) AS SUM_EXPENSE")
            lSql.AppendLine("  , SUM(0) AS SUM_TAX ")
            lSql.AppendLine("   , '' AS MIN_NOTES  ")
            lSql.AppendLine("  FROM CT_PAYMENT1 AS T1,CT_PAYMENT2 AS T2 ")
            lSql.AppendLine(" WHERE T1.MANAGE_NO = T2.MANAGE_NO  ")
            lSql.AppendLine("   AND T1.JIBUMON_CD = T2.JIBUMON_CD ")
            lSql.AppendLine("   AND T1.INPUT_DATE >= @start_date")
            lSql.AppendLine("   AND T1.INPUT_DATE < @end_date")
            lSql.AppendLine("   AND T1.JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("   AND T2.JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine(" GROUP BY T1.JIBUMON_CD, T1.SLIP_FLG")
            lSql.AppendLine("")
            lSql.AppendLine(" ORDER BY 1,2,4,3")

            Dim lDt As New DS_GLOVIA.CONV_SUITOU_FDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(lDt.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = lDt.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vJibumonCd
                    lCmd.Parameters.Add(jibumon_cd)
                    '締日開始日
                    Dim start_date As New SqlParameter
                    start_date.SqlDbType = SqlDbType.DateTime
                    start_date.Size = -1
                    start_date.ParameterName = "@start_date"
                    start_date.Value = vStartDate
                    lCmd.Parameters.Add(start_date)
                    '締日終了日
                    Dim end_date As New SqlParameter
                    end_date.SqlDbType = SqlDbType.DateTime
                    end_date.Size = -1
                    end_date.ParameterName = "@end_date"
                    end_date.Value = CDate(vEndDate).AddDays(1).ToString("yyyy/MM/dd")
                    lCmd.Parameters.Add(end_date)

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
        ''' GLOVIA出力する請求書のデータを取得する
        ''' </summary>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vStartDate">締日開始日</param>
        ''' <param name="vEndDate">締日終了日</param>
        ''' <returns></returns>
        Public Function GetConvData_Seikyu(ByVal vJibumonCd As String, ByVal vStartDate As String, ByVal vEndDate As String) As DS_GLOVIA.CONV_SEIKYUDataTable

            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT T1.JIBUMON_CD, T1.MANAGE_NO, '0' AS SLIP_KBN, T1.TRADE_CD ")
            lSql.AppendLine(" , T2.SUB_BILL_NO, 0 AS KINGAKU_KBN, T2.BUMON_CD, T2.KAMOKU_CD, T2.UCHI_CD, T2.NOTES ")
            lSql.AppendLine(" , T2.EXPENSE, T2.TAX ")
            lSql.AppendLine(" , MB.SAIMU_BMN,MT.MIBARAI_CD, MT.KEIYAKU_NO     ")
            lSql.AppendLine(" FROM CT_BILL1 AS T1,CT_BILL2 AS T2,M_BUMON AS MB,View_M_TRADE AS MT   ")
            lSql.AppendLine(" WHERE T1.MANAGE_NO = T2.MANAGE_NO   AND T1.JIBUMON_CD = T2.JIBUMON_CD   ")
            lSql.AppendLine("   AND  INPUT_DATE >= @start_date")
            lSql.AppendLine("   AND  INPUT_DATE < @end_date")
            lSql.AppendLine("   AND T1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND T2.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND T2.BUMON_CD   = MB.BUMON_CD")
            lSql.AppendLine("   AND T1.TRADE_CD   = MT.TRADE_CD")
            lSql.AppendLine(" ")
            lSql.AppendLine(" UNION ALL  ")
            lSql.AppendLine(" ")
            lSql.AppendLine(" SELECT T1.JIBUMON_CD, T1.MANAGE_NO, '0' AS SLIP_KBN, T1.TRADE_CD ")
            lSql.AppendLine(" , T2.SUB_BILL_NO, 1 AS KINGAKU_KBN, 'B0000' AS BUMON_CD, '10350' AS KAMOKU_CD, '' AS UCHI_CD, '' AS NOTES ")
            lSql.AppendLine(" , (T2.TAX) AS EXPENSE, 0 AS TAX ")
            lSql.AppendLine(" , MB.SAIMU_BMN,MT.MIBARAI_CD, MT.KEIYAKU_NO     ")
            lSql.AppendLine(" FROM CT_BILL1 AS T1,CT_BILL2 AS T2,M_BUMON AS MB,View_M_TRADE AS MT   ")
            lSql.AppendLine(" WHERE T1.MANAGE_NO = T2.MANAGE_NO   AND T1.JIBUMON_CD = T2.JIBUMON_CD   ")
            lSql.AppendLine("   AND  INPUT_DATE >= @start_date")
            lSql.AppendLine("   AND  INPUT_DATE < @end_date")
            lSql.AppendLine("   AND T1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND T2.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND T2.BUMON_CD   = MB.BUMON_CD")
            lSql.AppendLine("   AND T1.TRADE_CD   = MT.TRADE_CD")
            lSql.AppendLine("   AND T2.TAX        <> 0   ")
            lSql.AppendLine(" ")
            lSql.AppendLine(" UNION ALL  ")
            lSql.AppendLine(" ")
            lSql.AppendLine(" SELECT T1.JIBUMON_CD, T1.MANAGE_NO, '1' AS SLIP_KBN, T1.TRADE_CD ")
            lSql.AppendLine(" , 0 AS SUB_BILL_NO, 2 AS KINGAKU_KBN, '' AS BUMON_CD, '' AS KAMOKU_CD, ISNULL(MI.UCHI_CD, '') AS UCHI_CD, '' AS NOTES ")
            lSql.AppendLine(" , SUM(T2.EXPENSE + T2.TAX) AS EXPENSE, 0 AS TAX ")
            lSql.AppendLine(" , MB.SAIMU_BMN,MT.MIBARAI_CD, MT.KEIYAKU_NO     ")
            lSql.AppendLine(" FROM CT_BILL1 AS T1, CT_BILL2 AS T2, M_BUMON AS MB, View_M_TRADE AS MT ")

            lSql.AppendLine(" LEFT OUTER JOIN M_MIBARAI MI")
            lSql.AppendLine("   ON  MT.TRADE_CD   = MI.TRADE_CD")
            lSql.AppendLine("   AND MT.MIBARAI_CD = MI.MIBARAI_CD")

            lSql.AppendLine(" WHERE T1.MANAGE_NO = T2.MANAGE_NO   AND T1.JIBUMON_CD = T2.JIBUMON_CD   ")
            lSql.AppendLine("   AND  INPUT_DATE >= @start_date")
            lSql.AppendLine("   AND  INPUT_DATE < @end_date")
            lSql.AppendLine("   AND T1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND T2.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND T1.JIBUMON_CD = MB.BUMON_CD")
            lSql.AppendLine("   AND T1.TRADE_CD   = MT.TRADE_CD")
            lSql.AppendLine(" GROUP BY T1.JIBUMON_CD, T1.MANAGE_NO, T1.TRADE_CD, MB.SAIMU_BMN, MT.MIBARAI_CD, MT.KEIYAKU_NO, MI.UCHI_CD ")

            lSql.AppendLine(" ORDER BY T1.MANAGE_NO,SLIP_KBN, SUB_BILL_NO, KINGAKU_KBN ")

            Dim lDt As New DS_GLOVIA.CONV_SEIKYUDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(lDt.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = lDt.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vJibumonCd
                    lCmd.Parameters.Add(jibumon_cd)
                    '締日開始日
                    Dim start_date As New SqlParameter
                    start_date.SqlDbType = SqlDbType.DateTime
                    start_date.Size = -1
                    start_date.ParameterName = "@start_date"
                    start_date.Value = vStartDate
                    lCmd.Parameters.Add(start_date)
                    '締日終了日
                    Dim end_date As New SqlParameter
                    end_date.SqlDbType = SqlDbType.DateTime
                    end_date.Size = -1
                    end_date.ParameterName = "@end_date"
                    end_date.Value = CDate(vEndDate).AddDays(1).ToString("yyyy/MM/dd")
                    lCmd.Parameters.Add(end_date)

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

    End Class
End Namespace
