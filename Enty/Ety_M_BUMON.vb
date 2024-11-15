Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty

Namespace Enty

    Public Class Ety_M_BUMON
        Inherits Ety_Base

        Private _Dt As New M_BUMON.M_BUMONDataTable



        Public Shadows Function GetData() As M_BUMON.M_BUMONDataTable

            Try

                Dim condition As String = ""
                Dim queryString As New StringBuilder
                queryString.Append(" SELECT " & Environment.NewLine)
                queryString.Append("     * " & Environment.NewLine)
                queryString.Append(" FROM M_BUMON " & Environment.NewLine)

                Dim dt As New M_BUMON.M_BUMONDataTable

                Using db As New SqlConnection(MyBase.pConnString)
                    Using cmd As New SqlCommand

                        '検索条件設定
                        If condition <> "" Then
                            condition = " WHERE " & condition.Substring(5) & " order by BUMON_CD"
                        End If
                        queryString.Append(condition)
                        cmd.CommandText = queryString.ToString
                        cmd.Connection = db

                        'データを取得する
                        Dim lRow As M_BUMON.M_BUMONRow
                        db.Open()
                        Dim reader As DbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While reader.Read()
                            lRow = dt.NewM_BUMONRow
                            lRow.BUMON_CD = reader("BUMON_CD")
                            lRow.BUMON_NM = reader("BUMON_NM")
                            lRow.SAIMU_BMN = reader("SAIMU_BMN")
                            dt.AddM_BUMONRow(lRow)
                        Loop

                    End Using 'cmd
                End Using 'db

                Return dt
            Catch ex As Exception


                Throw New Exception(ex.Message, ex)


            End Try


        End Function

        Public Function SelectData(ByVal vparam As String) As M_BUMON.M_BUMONDataTable

            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_BUMON (NOLOCK)")

                Using lDb As New SqlConnection(MyBase.pConnString)
                    Using lCmd As New SqlCommand
                        Dim lCondition As String = SetCondition(lCmd, vparam)
                        If lCondition <> "" Then
                            lCondition = " WHERE " & lCondition.Substring(5)
                        End If
                        lSql.AppendLine(lCondition)
                        'lSql.AppendLine(" ORDER BY KOJOCD ,MANUFACT_DATE ,LINE_NO ,SHO_CODE")
                        lCmd.CommandText = lSql.ToString
                        lCmd.Connection = lDb

                        'データを取得する
                        Using Adapter As New SqlDataAdapter
                            Adapter.SelectCommand = lCmd
                            Adapter.Fill(_Dt)
                        End Using 'Adapter
                    End Using 'cmd
                End Using 'db

                Return _Dt


            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try


        End Function

        Private Function SetCondition(ByRef vCmd As SqlCommand, ByVal code As String) As String


            Dim lCond As New StringBuilder
            Dim lEscChar As String = Uty_Config.SQLESC

            vCmd.Parameters.Clear()

            '得意先コード
            If code <> String.Empty Then
                lCond.AppendLine(" AND BUMON_CD = @bumon_cd")
                Dim tokucd As New SqlParameter
                tokucd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_CDColumn.DataType)
                tokucd.Size = _Dt.BUMON_CDColumn.MaxLength
                tokucd.ParameterName = "@bumon_cd"
                tokucd.Value = code
                vCmd.Parameters.Add(tokucd)

            End If

            Return lCond.ToString

        End Function

        Public Function GetBumonName(ByVal vBumonCd As String) As String
            Dim lRst As String = ""
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT BUMON_NM")
            lSql.AppendLine(" FROM M_BUMON (NOLOCK)")
            lSql.AppendLine(" WHERE BUMON_CD = @bumon_cd")

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    lCmd.Parameters.Clear()
                    '検索条件設定
                    Dim bumon_cd As New SqlParameter
                    bumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_CDColumn.DataType)
                    bumon_cd.Size = _Dt.BUMON_CDColumn.MaxLength
                    bumon_cd.ParameterName = "@bumon_cd"
                    bumon_cd.Value = vBumonCd
                    lCmd.Parameters.Add(bumon_cd)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    lDb.Open()
                    lRst = lCmd.ExecuteScalar
                End Using 'cmd
            End Using 'db

            Return lRst
        End Function

        ''' <summary>
        ''' 部門締め情報を取得する
        ''' </summary>
        ''' <param name="vCloseDate">締日</param>
        ''' <param name="vClassNo">業務種別　1:出納帳　2:請求書</param>
        ''' <returns></returns>
        Public Function GetShimeInfo(ByVal vCloseDate As String, ByVal vClassNo As Integer) As M_BUMON.M_BUMON_SHIME_INFODataTable

            Try
                Dim lDt As New M_BUMON.M_BUMON_SHIME_INFODataTable
                Dim lSql As New StringBuilder
                lSql.AppendLine(" SELECT M1.*,CONVERT(nvarchar,C1.CLOSE_DATE,111) CLOSE_DATE,C1.CANCEL_FLG  ")
                lSql.AppendLine("   FROM M_BUMON AS M1 ,CT_CLOSE AS C1  ")
                lSql.AppendLine("  WHERE C1.JIBUMON_CD = M1.BUMON_CD  ")
                lSql.AppendLine("    AND C1.CLASS_NO =  @class_no")
                lSql.AppendLine("    AND ( C1.CLOSE_DATE =  @close_date")
                lSql.AppendLine("      AND C1.CLOSE_DATE =  ")
                lSql.AppendLine("               ( SELECT MAX(C2.CLOSE_DATE) FROM CT_CLOSE C2  ")
                lSql.AppendLine("                                        WHERE C2.JIBUMON_CD = M1.BUMON_CD  ")
                lSql.AppendLine("                                          AND C1.JIBUMON_CD = C2.JIBUMON_CD  ")
                lSql.AppendLine("                                          AND C2.CLASS_NO =  @class_no")
                lSql.AppendLine("                ) ")
                lSql.AppendLine("         ) ")
                lSql.AppendLine(" UNION")
                lSql.AppendLine("  SELECT M1.*,CONVERT(nvarchar,C1.CLOSE_DATE,111) CLOSE_DATE,C1.CANCEL_FLG  ")
                lSql.AppendLine("    FROM M_BUMON AS M1 ,CT_CLOSE AS C1  ")
                lSql.AppendLine("   WHERE C1.JIBUMON_CD = M1.BUMON_CD  ")
                lSql.AppendLine("     AND C1.CLASS_NO   =  1")
                lSql.AppendLine("     AND C1.CLOSE_DATE =  @close_date")
                lSql.AppendLine("     AND C1.CANCEL_FLG = 0 ")


                'lSql.AppendLine("    OR  (C1.CLOSE_DATE =  @close_date")
                'lSql.AppendLine("     AND C1.CANCEL_FLG = 0  ")
                'lSql.AppendLine("     AND C1.CLOSE_DATE  =  ")
                'lSql.AppendLine("               ( SELECT MAX(C2.CLOSE_DATE) FROM CT_CLOSE C2  ")
                'lSql.AppendLine("                                        WHERE C2.JIBUMON_CD = M1.BUMON_CD  ")
                'lSql.AppendLine("                                          AND C1.JIBUMON_CD = C2.JIBUMON_CD  ")
                'lSql.AppendLine("                                          AND C2.CLASS_NO =  @class_no")
                'lSql.AppendLine("                ) ")
                'lSql.AppendLine("         )")
                ''lSql.AppendLine("")
                ''lSql.AppendLine(" UNION ")
                ''lSql.AppendLine(" SELECT M1.*,'',0 FROM M_BUMON AS M1   ")
                ''lSql.AppendLine("  WHERE  NOT EXISTS  ")
                ''lSql.AppendLine("         (SELECT * FROM CT_CLOSE AS C1  ")
                ''lSql.AppendLine("            WHERE C1.JIBUMON_CD = M1.BUMON_CD  ")
                ''lSql.AppendLine("               AND C1.CLASS_NO =  @class_no")
                ''lSql.AppendLine("         ) ")
                lSql.AppendLine(" ORDER BY BUMON_CD ")

                Using lDb As New SqlConnection(MyBase.pConnString)
                    Using lCmd As New SqlCommand

                        lCmd.Parameters.Clear()

                        '締日
                        Dim close_date As New SqlParameter
                        close_date.SqlDbType = SqlDbType.DateTime
                        close_date.Size = -1
                        close_date.ParameterName = "@close_date"
                        close_date.Value = vCloseDate
                        lCmd.Parameters.Add(close_date)
                        '業務種別
                        Dim class_no As New SqlParameter
                        class_no.SqlDbType = SqlDbType.Int
                        class_no.Size = -1
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
                    End Using 'cmd
                End Using 'db

                Return lDt

            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End Function
        Public Function ExistData(ByVal vparam As String) As M_BUMON.M_BUMONDataTable


            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_BUMON (NOLOCK)")

                Using lDb As New SqlConnection(MyBase.pConnString)
                    Using lCmd As New SqlCommand
                        Dim lCondition As String = SetCondition(lCmd, vparam)
                        If lCondition <> "" Then
                            lCondition = " WHERE " & lCondition.Substring(5)
                        End If
                        lSql.AppendLine(lCondition)
                        'lSql.AppendLine(" ORDER BY KOJOCD ,MANUFACT_DATE ,LINE_NO ,SHO_CODE")
                        lCmd.CommandText = lSql.ToString
                        lCmd.Connection = lDb

                        'データを取得する
                        Using Adapter As New SqlDataAdapter
                            Adapter.SelectCommand = lCmd
                            Adapter.Fill(_Dt)
                        End Using 'Adapter
                    End Using 'cmd
                End Using 'db

                Return _Dt


            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try


        End Function

        Public Function InsertData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                     vSeihin As M_BUMON.M_BUMONDataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO M_BUMON (")

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
                Dim bumon_cd As New SqlParameter
                bumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_CDColumn.DataType)
                bumon_cd.Size = _Dt.BUMON_NMColumn.MaxLength
                bumon_cd.ParameterName = "@bumon_cd"
                rCmd.Parameters.Add(bumon_cd)

                Dim bumon_name As New SqlParameter
                bumon_name.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_NMColumn.DataType)
                bumon_name.Size = _Dt.BUMON_NMColumn.MaxLength
                bumon_name.ParameterName = "@bumon_nm"
                rCmd.Parameters.Add(bumon_name)

                Dim saimu_cd As New SqlParameter
                saimu_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.SAIMU_BMNColumn.DataType)
                saimu_cd.Size = _Dt.SAIMU_BMNColumn.MaxLength
                saimu_cd.ParameterName = "@saimu_bmn"
                rCmd.Parameters.Add(saimu_cd)

                '値をセットする

                bumon_cd.Value = vSeihin(0).BUMON_CD
                bumon_name.Value = vSeihin(0).BUMON_NM
                saimu_cd.Value = vSeihin(0).SAIMU_BMN

                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                lCnt = rCmd.ExecuteNonQuery()

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Public Function UpDate(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                               ByRef ldt As M_BUMON.M_BUMONDataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" UPDATE M_BUMON SET")
                lSql.AppendLine("   bumon_nm = @bumon_nm")
                lSql.AppendLine("   ,saimu_bmn = @saimu_bmn")
                lSql.AppendLine(" WHERE bumon_cd = @bumon_cd")

                '更新パラメータ
                Dim bumon_cd As New SqlParameter
                bumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_CDColumn.DataType)
                bumon_cd.Size = _Dt.BUMON_CDColumn.MaxLength
                bumon_cd.ParameterName = "@bumon_cd"
                rCmd.Parameters.Add(bumon_cd)

                Dim bumon_name As New SqlParameter
                bumon_name.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_NMColumn.DataType)
                bumon_name.Size = _Dt.BUMON_NMColumn.MaxLength
                bumon_name.ParameterName = "@bumon_nm"
                rCmd.Parameters.Add(bumon_name)

                Dim saimu_cd As New SqlParameter
                saimu_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.SAIMU_BMNColumn.DataType)
                saimu_cd.Size = _Dt.SAIMU_BMNColumn.MaxLength
                saimu_cd.ParameterName = "@saimu_bmn"
                rCmd.Parameters.Add(saimu_cd)


                '値をセットする
                bumon_cd.Value = ldt(0).BUMON_CD
                bumon_name.Value = ldt(0).BUMON_NM
                saimu_cd.Value = ldt(0).SAIMU_BMN

                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                lCnt = rCmd.ExecuteNonQuery()

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function


        Public Function DeleteData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                  ByRef code As String) As Integer

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder

            rCmd.Parameters.Clear()

            'SQL文を生成する
            lSql.AppendLine(" DELETE FROM M_BUMON")
            lSql.AppendLine(" WHERE bumon_cd = @bumon_cd ")

            '更新パラメータ
            Dim bumon_cd As New SqlParameter
            bumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_CDColumn.DataType)
            bumon_cd.Size = _Dt.BUMON_CDColumn.MaxLength
            bumon_cd.ParameterName = "@bumon_cd"
            rCmd.Parameters.Add(bumon_cd)


            '値をセットする
            bumon_cd.Value = code

            rCmd.CommandText = lSql.ToString
            rCmd.Connection = rDb

            lCnt = rCmd.ExecuteNonQuery()

            Return lCnt

        End Function

    End Class

End Namespace
