Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty
Imports KeihiWeb.Report

Namespace Enty
    Public Class Ety_CT_CLOSE
        Inherits Ety_Base

        Private _Dt As New DS_CT_CLOSE.CT_CLOSEDataTable

        Private Structure Shimebi
            Dim StartDate As String
            Dim EndDate As String
        End Structure

        ''' <summary>
        ''' 締めテーブルより残高を取得する
        ''' </summary>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vCloseDate">締日</param>
        ''' <param name="vClassNo">業務種別　１：出納帳（省略時）　２：請求書</param>
        ''' <returns></returns>
        Public Function GetZandakaRec(ByVal vJibumonCd As String, ByVal vCloseDate As String, Optional ByVal vClassNo As Integer = 1) As DS_CT_CLOSE.CT_CLOSEDataTable
            Dim lRst As Decimal = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT TOP 1 ")
            lSql.AppendLine("     JIBUMON_CD")
            lSql.AppendLine("   , CLASS_NO")
            lSql.AppendLine("   , CLOSE_DATE")
            lSql.AppendLine("   , BALANSE")
            lSql.AppendLine("   , CANCEL_FLG")
            lSql.AppendLine("   , ACT_DATE")
            lSql.AppendLine(" FROM CT_CLOSE (NOLOCK)")
            lSql.AppendLine(" WHERE JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND CLOSE_DATE <= @close_date")
            lSql.AppendLine("   AND CLASS_NO = @class_no")
            lSql.AppendLine(" ORDER BY CLOSE_DATE DESC")

            Dim lDt As New DS_CT_CLOSE.CT_CLOSEDataTable

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
                    '締日
                    Dim close_date As New SqlParameter
                    close_date.SqlDbType = Uty_Dbinfo.ConvertToDbType(lDt.CLOSE_DATEColumn.DataType)
                    close_date.Size = lDt.CLOSE_DATEColumn.MaxLength
                    close_date.ParameterName = "@close_date"
                    close_date.Value = vCloseDate
                    lCmd.Parameters.Add(close_date)
                    '業務種別
                    Dim class_no As New SqlParameter
                    class_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(lDt.CLASS_NOColumn.DataType)
                    class_no.Size = lDt.CLASS_NOColumn.MaxLength
                    class_no.ParameterName = "@class_no"
                    class_no.Value = vClassNo
                    lCmd.Parameters.Add(class_no)

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
        ''' 最終締日を取得する
        ''' </summary>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vClassNo">業務種別　１：出納帳（省略時）　２：請求書</param>
        ''' <returns></returns>
        Public Function GetLastShimebi(ByVal vJibumonCd As String, Optional ByVal vClassNo As Integer = 1) As Date
            Dim lRst As New Date
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT ")
            lSql.AppendLine("   MAX(CLOSE_DATE) CLOSE_DATE")
            lSql.AppendLine(" FROM CT_CLOSE (NOLOCK)")
            lSql.AppendLine(" WHERE JIBUMON_CD = @jibumon_cd")
            lSql.AppendLine("   AND CLASS_NO = @class_no")
            'lSql.AppendLine("   AND CANCEL_FLG = 0")
            lSql.AppendLine(" GROUP BY JIBUMON_CD ")
            lSql.AppendLine(" ORDER BY CLOSE_DATE DESC")

            Dim lDt As New DS_CT_CLOSE.CT_CLOSEDataTable

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
                    '業務種別
                    Dim class_no As New SqlParameter
                    class_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(lDt.CLASS_NOColumn.DataType)
                    class_no.Size = lDt.CLASS_NOColumn.MaxLength
                    class_no.ParameterName = "@class_no"
                    class_no.Value = vClassNo
                    lCmd.Parameters.Add(class_no)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    lDb.Open()
                    lRst = lCmd.ExecuteScalar()

                End Using 'lCmd
            End Using 'lDb

            Return lRst
        End Function

        ''' <summary>
        ''' 当期残高を算出する
        ''' </summary>
        ''' <param name="vDate">対象日</param>
        ''' <returns></returns>
        Public Function ZandakaCalc(ByVal vJibumonCd As String, ByVal vDate As String) As Decimal
            Dim lRst As Decimal = 0
            Dim lParam As New Shimebi
            Dim lDt As New DS_CT_CLOSE.CT_CLOSEDataTable
            lDt = GetZandakaRec(vJibumonCd, vDate, 1)

            If lDt.Count = 0 Then
                '前期締めデータなし
                lParam.StartDate = "1900/01/01" ' 集計開始日は指定無し
                lParam.EndDate = vDate          ' 集計終了日は指定日
            Else
                lParam.StartDate = lDt(0).CLOSE_DATE.AddDays(1)     ' 集計開始日は前期の締め日の翌日
                lParam.EndDate = vDate                              ' 集計終了日は指定日
            End If


            Return lRst
        End Function

        Public Function InsertData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand, vDt As DS_CT_CLOSE.CT_CLOSEDataTable) As Integer

            Try
                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)


                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO CT_CLOSE (")
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


                rCmd.Parameters.Clear()
                '更新パラメータ
                Dim jibumon_cd As New SqlParameter
                jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
                jibumon_cd.Size = _Dt.JIBUMON_CDColumn.MaxLength
                jibumon_cd.ParameterName = "@jibumon_cd"
                rCmd.Parameters.Add(jibumon_cd)

                Dim class_no As New SqlParameter
                class_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CLASS_NOColumn.DataType)
                class_no.Size = _Dt.CLASS_NOColumn.MaxLength
                class_no.ParameterName = "@class_no"
                rCmd.Parameters.Add(class_no)

                Dim close_date As New SqlParameter
                close_date.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CLOSE_DATEColumn.DataType)
                close_date.Size = _Dt.CLOSE_DATEColumn.MaxLength
                close_date.ParameterName = "@close_date"
                rCmd.Parameters.Add(close_date)

                Dim balanse As New SqlParameter
                balanse.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BALANSEColumn.DataType)
                balanse.Size = _Dt.BALANSEColumn.MaxLength
                balanse.ParameterName = "@balanse"
                rCmd.Parameters.Add(balanse)

                Dim cancel_flg As New SqlParameter
                cancel_flg.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CANCEL_FLGColumn.DataType)
                cancel_flg.Size = _Dt.CANCEL_FLGColumn.MaxLength
                cancel_flg.ParameterName = "@cancel_flg"
                rCmd.Parameters.Add(cancel_flg)

                Dim act_date As New SqlParameter
                act_date.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ACT_DATEColumn.DataType)
                act_date.Size = _Dt.ACT_DATEColumn.MaxLength
                act_date.ParameterName = "@act_date"
                rCmd.Parameters.Add(act_date)

                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                '値をセットする
                For Each r As DS_CT_CLOSE.CT_CLOSERow In vDt.Rows
                    jibumon_cd.Value = r.JIBUMON_CD
                    class_no.Value = r.CLASS_NO
                    close_date.Value = r.CLOSE_DATE
                    balanse.Value = r.BALANSE
                    cancel_flg.Value = r.CANCEL_FLG
                    act_date.Value = r.ACT_DATE

                    lCnt += rCmd.ExecuteNonQuery()
                Next

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try
        End Function

        Public Function DeleteData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                   ByVal vJibumonCd As String, ByVal vClassNo As Integer, ByVal vCloseDate As String) As Integer

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder

            rCmd.Parameters.Clear()

            'SQL文を生成する
            lSql.AppendLine(" DELETE FROM CT_CLOSE")
            lSql.AppendLine(" WHERE JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("   AND CLASS_NO = @class_no ")
            lSql.AppendLine("   AND CLOSE_DATE = @close_date ")

            rCmd.Parameters.Clear()
            '更新パラメータ
            Dim jibumon_cd As New SqlParameter
            jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
            jibumon_cd.Size = _Dt.JIBUMON_CDColumn.MaxLength
            jibumon_cd.ParameterName = "@jibumon_cd"
            jibumon_cd.Value = vJibumonCd
            rCmd.Parameters.Add(jibumon_cd)

            Dim class_no As New SqlParameter
            class_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CLASS_NOColumn.DataType)
            class_no.Size = _Dt.CLASS_NOColumn.MaxLength
            class_no.ParameterName = "@class_no"
            class_no.Value = vClassNo
            rCmd.Parameters.Add(class_no)

            Dim close_date As New SqlParameter
            close_date.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CLOSE_DATEColumn.DataType)
            close_date.Size = _Dt.CLOSE_DATEColumn.MaxLength
            close_date.ParameterName = "@close_date"
            close_date.Value = vCloseDate
            rCmd.Parameters.Add(close_date)

            rCmd.CommandText = lSql.ToString
            rCmd.Connection = rDb

            lCnt = rCmd.ExecuteNonQuery()

            Return lCnt

        End Function

        ''' <summary>
        ''' GLOVIA出力済みか確認する
        ''' </summary>
        ''' <param name="vJibumonCd"></param>
        ''' <param name="vClassNo"></param>
        ''' <param name="vCloseDate"></param>
        ''' <returns>True:出力済み　Flase:未出力</returns>
        Public Function IsOutputGlovia(ByVal vJibumonCd As String, ByVal vClassNo As Integer, ByVal vCloseDate As String) As Boolean

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder

            'SQL文を生成する
            lSql.AppendLine(" SELECT CANCEL_FLG FROM CT_CLOSE")
            lSql.AppendLine(" WHERE JIBUMON_CD = @jibumon_cd ")
            lSql.AppendLine("   AND CLASS_NO = @class_no ")
            lSql.AppendLine("   AND CLOSE_DATE = @close_date ")

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand

                    lCmd.Parameters.Clear()
                    '更新パラメータ
                    Dim jibumon_cd As New SqlParameter
                    jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
                    jibumon_cd.Size = _Dt.JIBUMON_CDColumn.MaxLength
                    jibumon_cd.ParameterName = "@jibumon_cd"
                    jibumon_cd.Value = vJibumonCd
                    lCmd.Parameters.Add(jibumon_cd)

                    Dim class_no As New SqlParameter
                    class_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CLASS_NOColumn.DataType)
                    class_no.Size = _Dt.CLASS_NOColumn.MaxLength
                    class_no.ParameterName = "@class_no"
                    class_no.Value = vClassNo
                    lCmd.Parameters.Add(class_no)

                    Dim close_date As New SqlParameter
                    close_date.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CLOSE_DATEColumn.DataType)
                    close_date.Size = _Dt.CLOSE_DATEColumn.MaxLength
                    close_date.ParameterName = "@close_date"
                    close_date.Value = vCloseDate
                    lCmd.Parameters.Add(close_date)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    lDb.Open()
                    lCnt = lCmd.ExecuteScalar()

                End Using 'lCmd
            End Using 'lDb

            Return (lCnt > 0)

        End Function

        ''' <summary>
        ''' GLOVIA出力済みフラグを更新する（CANCEL_FLG=2）
        ''' </summary>
        ''' <param name="rDb">DB接続オブジェクト</param>
        ''' <param name="rCmd">コマンドオブジェクト</param>
        ''' <param name="vDt">データテーブル</param>
        ''' <returns></returns>
        Public Function UpdateOutputGlovia(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand, vDt As DS_CT_CLOSE.CT_CLOSEDataTable) As Integer

            Try
                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)

                'SQL文を生成する
                lSql.AppendLine(" UPDATE CT_CLOSE ")
                lSql.AppendLine(" SET CANCEL_FLG = @cancel_flg ")
                lSql.AppendLine("   , ACT_DATE = @act_date ")
                lSql.AppendLine(" WHERE JIBUMON_CD = @jibumon_cd")
                lSql.AppendLine("   AND CLASS_NO = @class_no")
                lSql.AppendLine("   AND CLOSE_DATE = @close_date")

                rCmd.Parameters.Clear()
                '更新パラメータ
                Dim jibumon_cd As New SqlParameter
                jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.JIBUMON_CDColumn.DataType)
                jibumon_cd.Size = _Dt.JIBUMON_CDColumn.MaxLength
                jibumon_cd.ParameterName = "@jibumon_cd"
                rCmd.Parameters.Add(jibumon_cd)

                Dim class_no As New SqlParameter
                class_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CLASS_NOColumn.DataType)
                class_no.Size = _Dt.CLASS_NOColumn.MaxLength
                class_no.ParameterName = "@class_no"
                rCmd.Parameters.Add(class_no)

                Dim close_date As New SqlParameter
                close_date.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CLOSE_DATEColumn.DataType)
                close_date.Size = _Dt.CLOSE_DATEColumn.MaxLength
                close_date.ParameterName = "@close_date"
                rCmd.Parameters.Add(close_date)

                Dim cancel_flg As New SqlParameter
                cancel_flg.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.CANCEL_FLGColumn.DataType)
                cancel_flg.Size = _Dt.CANCEL_FLGColumn.MaxLength
                cancel_flg.ParameterName = "@cancel_flg"
                rCmd.Parameters.Add(cancel_flg)

                Dim act_date As New SqlParameter
                act_date.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ACT_DATEColumn.DataType)
                act_date.Size = _Dt.ACT_DATEColumn.MaxLength
                act_date.ParameterName = "@act_date"
                rCmd.Parameters.Add(act_date)

                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                '値をセットする
                For Each r As DS_CT_CLOSE.CT_CLOSERow In vDt.Rows
                    jibumon_cd.Value = r.JIBUMON_CD
                    class_no.Value = r.CLASS_NO
                    close_date.Value = r.CLOSE_DATE
                    cancel_flg.Value = r.CANCEL_FLG
                    act_date.Value = r.ACT_DATE

                    lCnt += rCmd.ExecuteNonQuery()
                Next

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try
        End Function

    End Class
End Namespace
