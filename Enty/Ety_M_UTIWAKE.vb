Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text
Imports KeihiWeb.Common.uty
Namespace Enty

    Public Class Ety_M_UTIWAKE
        Inherits Ety_Base
        Private _Dt As New DS_M_UTIWAKE.M_UCHIWAKEDataTable

        Public Shadows Function GetData2() As DS_M_UTIWAKE.M_UCHIWAKE_MNTDataTable

            Try

                Dim condition As String = ""
                Dim queryString As New StringBuilder
                queryString.Append(" SELECT " & Environment.NewLine)
                queryString.Append(" U.KAMOKU_CD" & Environment.NewLine)
                queryString.Append(",U.UCHI_CD" & Environment.NewLine)
                queryString.Append(",U.UCHI_NM" & Environment.NewLine)
                queryString.Append(",U.UCHI_DISP" & Environment.NewLine)
                queryString.Append(",K.KAMOKU_NM" & Environment.NewLine)
                queryString.Append(" FROM M_UCHIWAKE as U " & Environment.NewLine)
                queryString.Append("inner join M_KAMOKU as K ON U.KAMOKU_CD =K.KAMOKU_CD " & Environment.NewLine)
                Dim dt As New DS_M_UTIWAKE.M_UCHIWAKE_MNTDataTable
                Dim utiwake As String = String.Empty


                Using db As New SqlConnection(MyBase.pConnString)
                    Using cmd As New SqlCommand
                        'Dim lCondition As String = SetCondition(cmd, kamoku, utiwake)
                        Dim lCondition As String = ""

                        '検索条件設定
                        If lCondition <> "" Then
                            lCondition = " WHERE " & lCondition.Substring(5) & " order by UCHI_CD"
                        End If
                        queryString.Append(lCondition)
                        cmd.CommandText = queryString.ToString
                        cmd.Connection = db

                        'データを取得する
                        Dim lRow As DS_M_UTIWAKE.M_UCHIWAKE_MNTRow
                        db.Open()
                        Dim reader As DbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While reader.Read()
                            lRow = dt.NewM_UCHIWAKE_MNTRow
                            lRow.KAMOKU_CD = reader("KAMOKU_CD")
                            lRow.KAMOKU_NM = reader("KAMOKU_NM")
                            lRow.UCHI_CD = reader("UCHI_CD")
                            lRow.UCHI_DISP = reader("UCHI_DISP")
                            lRow.UCHI_NM = reader("UCHI_NM")
                            dt.AddM_UCHIWAKE_MNTRow(lRow)
                        Loop

                    End Using 'cmd
                End Using 'db

                Return dt
            Catch ex As Exception


                Throw New Exception(ex.Message, ex)


            End Try


        End Function

        Public Shadows Function GetData() As DS_M_UTIWAKE.M_UCHIWAKEDataTable

            Try

                Dim condition As String = ""
                Dim queryString As New StringBuilder
                queryString.Append(" SELECT " & Environment.NewLine)
                queryString.Append("     * " & Environment.NewLine)
                queryString.Append(" FROM M_UCHIWAKE " & Environment.NewLine)

                Dim dt As New DS_M_UTIWAKE.M_UCHIWAKEDataTable
                Dim utiwake As String = String.Empty


                Using db As New SqlConnection(MyBase.pConnString)
                    Using cmd As New SqlCommand
                        'Dim lCondition As String = SetCondition(cmd, kamoku, utiwake)
                        Dim lCondition As String = ""

                        '検索条件設定
                        If lCondition <> "" Then
                            lCondition = " WHERE " & lCondition.Substring(5) & " order by UCHI_CD"
                        End If
                        queryString.Append(lCondition)
                        cmd.CommandText = queryString.ToString
                        cmd.Connection = db

                        'データを取得する
                        Dim lRow As DS_M_UTIWAKE.M_UCHIWAKERow
                        db.Open()
                        Dim reader As DbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While reader.Read()
                            lRow = dt.NewM_UCHIWAKERow
                            lRow.KAMOKU_CD = reader("KAMOKU_CD")
                            lRow.UCHI_CD = reader("UCHI_CD")
                            lRow.UCHI_DISP = reader("UCHI_DISP")
                            lRow.UCHI_NM = reader("UCHI_NM")
                            dt.AddM_UCHIWAKERow(lRow)
                        Loop

                    End Using 'cmd
                End Using 'db

                Return dt
            Catch ex As Exception


                Throw New Exception(ex.Message, ex)


            End Try


        End Function

        Public Shadows Function GetData(ByVal kamoku As String) As DS_M_UTIWAKE.M_UCHIWAKEDataTable

            Try


                Dim condition As String = ""
                Dim queryString As New StringBuilder
                queryString.Append(" SELECT " & Environment.NewLine)
                queryString.Append("     * " & Environment.NewLine)
                queryString.Append(" FROM M_UCHIWAKE " & Environment.NewLine)

                Dim dt As New DS_M_UTIWAKE.M_UCHIWAKEDataTable
                Dim utiwake As String = String.Empty


                Using db As New SqlConnection(MyBase.pConnString)
                    Using cmd As New SqlCommand
                        Dim lCondition As String = SetCondition(cmd, kamoku, utiwake)

                        '検索条件設定
                        If lCondition <> "" Then
                            lCondition = " WHERE " & lCondition.Substring(5) & " order by UCHI_CD"
                        End If
                        queryString.Append(lCondition)
                        cmd.CommandText = queryString.ToString
                        cmd.Connection = db

                        'データを取得する
                        Dim lRow As DS_M_UTIWAKE.M_UCHIWAKERow
                        db.Open()
                        Dim reader As DbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While reader.Read()
                            lRow = dt.NewM_UCHIWAKERow
                            lRow.KAMOKU_CD = reader("KAMOKU_CD")
                            lRow.UCHI_CD = reader("UCHI_CD")
                            lRow.UCHI_DISP = reader("UCHI_DISP")
                            lRow.UCHI_NM = reader("UCHI_NM")
                            dt.AddM_UCHIWAKERow(lRow)
                        Loop

                    End Using 'cmd
                End Using 'db

                Return dt
            Catch ex As Exception


                Throw New Exception(ex.Message, ex)


            End Try


        End Function

        Public Function SelectData(ByVal kamoku As String, ByVal utiwake As String) As DS_M_UTIWAKE.M_UCHIWAKEDataTable

            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_UCHIWAKE (NOLOCK)")

                Using lDb As New SqlConnection(MyBase.pConnString)
                    Using lCmd As New SqlCommand
                        Dim lCondition As String = SetCondition(lCmd, kamoku, utiwake)
                        If lCondition <> "" Then
                            lCondition = " WHERE " & lCondition.Substring(5)
                        End If
                        lSql.AppendLine(lCondition)
                        lSql.AppendLine(" ORDER BY UCHI_CD ")
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

        Private Function SetCondition(ByRef vCmd As SqlCommand, ByVal kamoku As String, ByVal utiwake As String) As String


            Dim lCond As New StringBuilder
            Dim lEscChar As String = Uty_Config.SQLESC

            vCmd.Parameters.Clear()

            '科目コード
            If kamoku <> String.Empty Then
                lCond.AppendLine(" AND KAMOKU_CD = @kamoku_cd")
                Dim tokucd As New SqlParameter
                tokucd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_CDColumn.DataType)
                tokucd.Size = _Dt.KAMOKU_CDColumn.MaxLength
                tokucd.ParameterName = "@kamoku_cd"
                tokucd.Value = kamoku
                vCmd.Parameters.Add(tokucd)

            End If

            '内訳コード
            If utiwake <> String.Empty Then
                lCond.AppendLine(" AND UCHI_CD = @uchi_cd")
                Dim uticd As New SqlParameter
                uticd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.UCHI_CDColumn.DataType)
                uticd.Size = _Dt.UCHI_CDColumn.MaxLength
                uticd.ParameterName = "@uchi_cd"
                uticd.Value = utiwake
                vCmd.Parameters.Add(uticd)

            End If


            Return lCond.ToString

        End Function
        Private Function SetCondition(ByRef vCmd As SqlCommand, ByVal utiwake As String) As String


            Dim lCond As New StringBuilder
            Dim lEscChar As String = Uty_Config.SQLESC

            vCmd.Parameters.Clear()

            '内訳コード
            If utiwake <> String.Empty Then
                lCond.AppendLine(" AND UCHI_CD = @uchi_cd")
                Dim uticd As New SqlParameter
                uticd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.UCHI_CDColumn.DataType)
                uticd.Size = _Dt.UCHI_CDColumn.MaxLength
                uticd.ParameterName = "@uchi_cd"
                uticd.Value = utiwake
                vCmd.Parameters.Add(uticd)

            End If


            Return lCond.ToString

        End Function

        Public Function ExistData(ByVal vparam As String) As DS_M_UTIWAKE.M_UCHIWAKEDataTable

            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_UCHIWAKE (NOLOCK)")

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
                                     vSeihin As DS_M_UTIWAKE.M_UCHIWAKEDataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO M_UCHIWAKE (")

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
                Dim kamoku_cd As New SqlParameter
                kamoku_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_CDColumn.DataType)
                kamoku_cd.Size = _Dt.KAMOKU_CDColumn.MaxLength
                kamoku_cd.ParameterName = "@kamoku_cd"
                rCmd.Parameters.Add(kamoku_cd)

                Dim uchiwake_cd As New SqlParameter
                uchiwake_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.UCHI_CDColumn.DataType)
                uchiwake_cd.Size = _Dt.UCHI_CDColumn.MaxLength
                uchiwake_cd.ParameterName = "@uchi_cd"
                rCmd.Parameters.Add(uchiwake_cd)


                Dim uchiwake_nm As New SqlParameter
                uchiwake_nm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.UCHI_NMColumn.DataType)
                uchiwake_nm.Size = _Dt.UCHI_NMColumn.MaxLength
                uchiwake_nm.ParameterName = "@uchi_nm"
                rCmd.Parameters.Add(uchiwake_nm)

                Dim uchiwake_disp As New SqlParameter
                uchiwake_disp.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.UCHI_DISPColumn.DataType)
                uchiwake_disp.Size = _Dt.UCHI_DISPColumn.MaxLength
                uchiwake_disp.ParameterName = "@uchi_disp"
                rCmd.Parameters.Add(uchiwake_disp)

                '値をセットする
                kamoku_cd.Value = vSeihin(0).KAMOKU_CD
                uchiwake_cd.Value = vSeihin(0).UCHI_CD
                uchiwake_nm.Value = vSeihin(0).UCHI_NM
                uchiwake_disp.Value = vSeihin(0).UCHI_DISP

                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                lCnt = rCmd.ExecuteNonQuery()

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Public Function UpDate(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                               ByRef ldt As DS_M_UTIWAKE.M_UCHIWAKEDataTable) As Integer

            Try

                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" UPDATE M_UCHIWAKE SET")
                lSql.AppendLine("   kamoku_cd = @kamoku_cd")
                lSql.AppendLine("   ,UCHI_CD = @uchi_cd")
                lSql.AppendLine("   ,UCHI_NM = @uchi_nm")
                lSql.AppendLine("   ,UCHI_DISP = @uchi_disp")

                lSql.AppendLine(" WHERE UCHI_CD = @uchi_cd")

                '更新パラメータ
                Dim kamoku_cd As New SqlParameter
                kamoku_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_CDColumn.DataType)
                kamoku_cd.Size = _Dt.KAMOKU_CDColumn.MaxLength
                kamoku_cd.ParameterName = "@kamoku_cd"
                rCmd.Parameters.Add(kamoku_cd)

                Dim uchiwake_cd As New SqlParameter
                uchiwake_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.UCHI_CDColumn.DataType)
                uchiwake_cd.Size = _Dt.UCHI_CDColumn.MaxLength
                uchiwake_cd.ParameterName = "@uchi_cd"
                rCmd.Parameters.Add(uchiwake_cd)


                Dim uchiwake_nm As New SqlParameter
                uchiwake_nm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.UCHI_NMColumn.DataType)
                uchiwake_nm.Size = _Dt.UCHI_NMColumn.MaxLength
                uchiwake_nm.ParameterName = "@uchi_nm"
                rCmd.Parameters.Add(uchiwake_nm)

                Dim uchiwake_disp As New SqlParameter
                uchiwake_disp.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.UCHI_DISPColumn.DataType)
                uchiwake_disp.Size = _Dt.UCHI_DISPColumn.MaxLength
                uchiwake_disp.ParameterName = "@uchi_disp"
                rCmd.Parameters.Add(uchiwake_disp)

                '値をセットする
                kamoku_cd.Value = ldt(0).KAMOKU_CD
                uchiwake_cd.Value = ldt(0).UCHI_CD
                uchiwake_nm.Value = ldt(0).UCHI_NM
                uchiwake_disp.Value = ldt(0).UCHI_DISP


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
            lSql.AppendLine(" DELETE FROM M_UCHIWAKE")
            lSql.AppendLine(" WHERE UCHI_CD = @uchi_cd")

            '更新パラメータ
            Dim uchiwake_cd As New SqlParameter
            uchiwake_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.UCHI_CDColumn.DataType)
            uchiwake_cd.Size = _Dt.UCHI_CDColumn.MaxLength
            uchiwake_cd.ParameterName = "@uchi_cd"
            rCmd.Parameters.Add(uchiwake_cd)

            '値をセットする
            uchiwake_cd.Value = code

            rCmd.CommandText = lSql.ToString
            rCmd.Connection = rDb

            lCnt = rCmd.ExecuteNonQuery()

            Return lCnt

        End Function


    End Class

End Namespace
