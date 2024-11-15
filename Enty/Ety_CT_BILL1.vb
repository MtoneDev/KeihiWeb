Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty
Imports KeihiWeb.Report

Namespace Enty
    Public Class Ety_CT_BILL1
        Inherits Ety_Base

        Private _Dt As New DS_BILL.RPT_SHIWAKEDataTable
        Private _DtBILL As New DS_BILL.CT_BILL1DataTable

        ''' <summary>
        ''' 請求入力仕訳データを取得する
        ''' </summary>
        ''' <param name="vParam">帳票パラメータ</param>
        ''' <returns></returns>
        Public Function GetRptData_Shiwake(ByVal vParam As Rpt_SeikyuShiwake.RptParam) As DS_BILL.RPT_SHIWAKEDataTable
            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT ")
            lSql.AppendLine("     T1.JIBUMON_CD")
            lSql.AppendLine("   , T1.MANAGE_NO")
            lSql.AppendLine("   , T2.SUB_BILL_NO")
            lSql.AppendLine("   , T1.INPUT_YM")
            lSql.AppendLine("   , T1.BILL_NO")
            lSql.AppendLine("   , T1.INPUT_DATE")
            lSql.AppendLine("   , T1.TRADE_CD")
            lSql.AppendLine("   , T1.TRADE_NM")
            lSql.AppendLine("   , T2.BUMON_CD")
            lSql.AppendLine("   , T2.KAMOKU_CD")
            lSql.AppendLine("   , T2.KAMOKU_NM")
            lSql.AppendLine("   , T2.UCHI_CD")
            lSql.AppendLine("   , T2.UCHI_NM")
            lSql.AppendLine("   , T2.NOTES")
            lSql.AppendLine("   , T2.TAX_CD")
            lSql.AppendLine("   , T2.TAX_NM")
            lSql.AppendLine("   , T2.EXPENSE")
            lSql.AppendLine("   , T2.TAX")
            lSql.AppendLine("   , (T2.EXPENSE + T2.TAX) AMOUNT")
            lSql.AppendLine("   , T1.CIR_NO")
            lSql.AppendLine(" FROM CT_BILL1 AS T1,CT_BILL2 AS T2 ")
            lSql.AppendLine(" WHERE T1.MANAGE_NO = T2.MANAGE_NO  ")
            lSql.AppendLine("   AND T1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND T2.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND T1.INPUT_DATE >= @input_date_from")
            lSql.AppendLine("   AND T1.INPUT_DATE < @input_date_to")
            lSql.AppendLine(" ORDER BY T1.MANAGE_NO, T2.SUB_BILL_NO")

            Dim lDt As New DS_BILL.RPT_SHIWAKEDataTable

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

        Public Function GetDenpyo_NO(ByVal vBumon_CD As String, ByVal vInput_YM As String) As DS_BILL.CT_BILL1_DENPYODataTable

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT")
            lSql.AppendLine("     BIL1.JIBUMON_CD")
            lSql.AppendLine("   , BIL1.INPUT_YM")
            lSql.AppendLine("   , MAX(BIL1.BILL_NO) as MAXBILL_NO")
            lSql.AppendLine(" FROM CT_BILL1 BIL1 (NOLOCK)")
            lSql.AppendLine(" WHERE BIL1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND BIL1.INPUT_YM = @input_ym")
            lSql.AppendLine("   GROUP BY BIL1.JIBUMON_CD,BIL1.INPUT_YM")

            Dim lDt As New DS_BILL.CT_BILL1_DENPYODataTable
            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = _DtBILL.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vBumon_CD
                    lCmd.Parameters.Add(jibumon_cd)
                    '入力日
                    Dim input_date_To As New SqlParameter
                    input_date_To.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.INPUT_YMColumn.DataType)
                    input_date_To.Size = _DtBILL.INPUT_YMColumn.MaxLength
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

        Public Function ExistDenpyo_NO(ByVal vBumon_CD As String, ByVal vInput_YM As String, ByVal vDen_NO As String) As DS_BILL.CT_BILL1DataTable

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT * ")
            lSql.AppendLine(" FROM CT_BILL1 BIL1 (NOLOCK)")
            lSql.AppendLine(" WHERE BIL1.JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND BIL1.INPUT_YM = @input_ym")
            lSql.AppendLine("   AND BIL1.BILL_NO = @bill_no")

            Dim lDt As New DS_BILL.CT_BILL1DataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    '自部門コード
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = _DtBILL.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vBumon_CD
                    lCmd.Parameters.Add(jibumon_cd)
                    '入力日
                    Dim input_date_To As New SqlParameter
                    input_date_To.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.INPUT_YMColumn.DataType)
                    input_date_To.Size = _DtBILL.INPUT_YMColumn.MaxLength
                    input_date_To.ParameterName = "@input_ym"
                    input_date_To.Value = vInput_YM
                    lCmd.Parameters.Add(input_date_To)

                    '伝票No
                    Dim slipno As New SqlParameter
                    slipno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.BILL_NOColumn.DataType)
                    slipno.Size = _DtBILL.BILL_NOColumn.MaxLength
                    slipno.ParameterName = "@bill_no"
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
                                     vSeihin As DS_BILL.CT_BILL1DataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_DtBILL)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO CT_BILL1 (")

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
                jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.JIBUMON_CDColumn.DataType)
                jibumoncd.Size = _DtBILL.JIBUMON_CDColumn.MaxLength
                jibumoncd.ParameterName = "@jibumon_cd"
                rCmd.Parameters.Add(jibumoncd)

                Dim manageno As New SqlParameter
                manageno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.MANAGE_NOColumn.DataType)
                manageno.Size = _DtBILL.MANAGE_NOColumn.MaxLength
                manageno.ParameterName = "@manage_no"
                rCmd.Parameters.Add(manageno)

                Dim inputym As New SqlParameter
                inputym.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.INPUT_YMColumn.DataType)
                inputym.Size = _DtBILL.INPUT_YMColumn.MaxLength
                inputym.ParameterName = "@input_ym"
                rCmd.Parameters.Add(inputym)

                Dim slipno As New SqlParameter
                slipno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.BILL_NOColumn.DataType)
                slipno.Size = _DtBILL.BILL_NOColumn.MaxLength
                slipno.ParameterName = "@bill_no"
                rCmd.Parameters.Add(slipno)

                Dim tradecd As New SqlParameter
                tradecd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.TRADE_CDColumn.DataType)
                tradecd.Size = _DtBILL.TRADE_CDColumn.MaxLength
                tradecd.ParameterName = "@trade_cd"
                rCmd.Parameters.Add(tradecd)

                Dim tradenm As New SqlParameter
                tradenm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.TRADE_NMColumn.DataType)
                tradenm.Size = _DtBILL.TRADE_NMColumn.MaxLength
                tradenm.ParameterName = "@trade_nm"
                rCmd.Parameters.Add(tradenm)

                Dim inputdate As New SqlParameter
                inputdate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.INPUT_DATEColumn.DataType)
                inputdate.Size = _Dt.INPUT_DATEColumn.MaxLength
                inputdate.ParameterName = "@input_date"
                rCmd.Parameters.Add(inputdate)

                Dim regdate As New SqlParameter
                regdate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.REG_DATEColumn.DataType)
                regdate.Size = _DtBILL.REG_DATEColumn.MaxLength
                regdate.ParameterName = "@reg_date"
                rCmd.Parameters.Add(regdate)


                Dim cirno As New SqlParameter
                cirno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.CIR_NOColumn.DataType)
                cirno.Size = _DtBILL.CIR_NOColumn.MaxLength
                cirno.ParameterName = "@cir_no"
                rCmd.Parameters.Add(cirno)

                '値をセットする

                jibumoncd.Value = vSeihin(0).JIBUMON_CD
                manageno.Value = vSeihin(0).MANAGE_NO
                inputym.Value = vSeihin(0).INPUT_YM
                slipno.Value = vSeihin(0).BILL_NO
                tradecd.Value = vSeihin(0).TRADE_CD
                tradenm.Value = vSeihin(0).TRADE_NM
                inputdate.Value = vSeihin(0).INPUT_DATE
                regdate.Value = vSeihin(0).REG_DATE
                cirno.Value = vSeihin(0).CIR_NO


                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                lCnt = rCmd.ExecuteNonQuery()

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Public Function DeleteData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                    ByVal vParam As DS_BILL.SearchParam_ListDataTable) As Integer

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder
            rCmd.Parameters.Clear()


            'SQL文を生成する
            lSql.AppendLine(" DELETE FROM CT_BILL1 ")
            lSql.AppendLine(" WHERE JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("   AND MANAGE_NO = @manage_no ")


            '検索パラメータ
            Dim jibumoncd As New SqlParameter
            jibumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.JIBUMON_CDColumn.DataType)
            jibumoncd.Size = _DtBILL.JIBUMON_CDColumn.MaxLength
            jibumoncd.ParameterName = "@jibumon_cd"
            rCmd.Parameters.Add(jibumoncd)

            Dim manageno As New SqlParameter
            manageno.SqlDbType = Uty_Dbinfo.ConvertToDbType(_DtBILL.MANAGE_NOColumn.DataType)
            manageno.Size = _DtBILL.MANAGE_NOColumn.MaxLength
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
