Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty

Namespace Enty
    Public Class Ety_M_USER
        Inherits Ety_Base

        Private _Dt As New M_USER.M_USERDataTable

        Public Shadows Function GetData() As M_USER.M_USERDataTable

            Try


                Dim condition As String = ""
                Dim queryString As New StringBuilder
                queryString.Append(" SELECT " & Environment.NewLine)
                queryString.Append("     * " & Environment.NewLine)
                queryString.Append(" FROM M_USER " & Environment.NewLine)

                Dim dt As New M_USER.M_USERDataTable

                Using db As New SqlConnection(MyBase.pConnString)
                    Using cmd As New SqlCommand

                        '検索条件設定
                        If condition <> "" Then
                            condition = " WHERE " & condition.Substring(5) & " order by USER_ID"
                        End If
                        queryString.Append(condition)
                        cmd.CommandText = queryString.ToString
                        cmd.Connection = db

                        'データを取得する
                        Dim lRow As M_USER.M_USERRow
                        db.Open()
                        Dim reader As DbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While reader.Read()
                            lRow = dt.NewM_USERRow
                            lRow.USER_ID = reader("USER_ID")
                            lRow.USER_NAME = reader("USER_NAME")
                            lRow.USER_KANA = reader("USER_KANA")
                            lRow.BUMON_CD = reader("BUMON_CD")
                            lRow.PASSWORD = reader("PASSWORD")
                            lRow.USER_LEVEL = reader("USER_LEVEL")
                            lRow.DEL_FLG = reader("DEL_FLG")
                            dt.AddM_USERRow(lRow)
                        Loop

                    End Using 'cmd
                End Using 'db

                Return dt
            Catch ex As Exception


                Throw New Exception(ex.Message, ex)


            End Try


        End Function

        Public Function ExistData(ByVal vparam As String) As M_USER.M_USERDataTable


            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_USER (NOLOCK)")

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
        Public Function SelectData(ByVal vparam As String) As M_USER.M_USERDataTable

            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_USER (NOLOCK)")

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

            'ユーザーID
            If code <> String.Empty Then
                lCond.AppendLine(" AND USER_ID = @user_id")
                Dim user_id As New SqlParameter
                user_id.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_IDColumn.DataType)
                user_id.Size = _Dt.USER_IDColumn.MaxLength
                user_id.ParameterName = "@user_id"
                user_id.Value = code
                vCmd.Parameters.Add(user_id)
            End If

            Return lCond.ToString

        End Function
        Public Function SelectData_bumon(ByVal vparam As String) As M_USER.M_USERDataTable

            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_USER (NOLOCK)")

                Using lDb As New SqlConnection(MyBase.pConnString)
                    Using lCmd As New SqlCommand
                        Dim lCondition As String = SetCondition_bumon(lCmd, vparam)
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

        Private Function SetCondition_bumon(ByRef vCmd As SqlCommand, ByVal code As String) As String


            Dim lCond As New StringBuilder
            Dim lEscChar As String = Uty_Config.SQLESC

            vCmd.Parameters.Clear()

            '部門コード
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

        Public Function SelectData2(ByVal code As String, ByVal jibumoncd As String) As M_USER.M_USERDataTable

            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_USER (NOLOCK)")

                Using lDb As New SqlConnection(MyBase.pConnString)
                    Using lCmd As New SqlCommand
                        Dim lCondition As String = SetCondition2(lCmd, code, jibumoncd)
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


        Private Function SetCondition2(ByRef vCmd As SqlCommand, ByVal code As String, ByVal jibumoncd As String) As String


            Dim lCond As New StringBuilder
            Dim lEscChar As String = Uty_Config.SQLESC

            vCmd.Parameters.Clear()

            '得意先コード
            If code <> String.Empty Then
                lCond.AppendLine(" AND USER_ID = @user_id")
                Dim tokucd As New SqlParameter
                tokucd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_IDColumn.DataType)
                tokucd.Size = _Dt.USER_IDColumn.MaxLength
                tokucd.ParameterName = "@user_id"
                tokucd.Value = code
                vCmd.Parameters.Add(tokucd)

            End If
            '自部門コード
            If jibumoncd <> String.Empty Then
                lCond.AppendLine(" AND BUMON_CD = @bumon_cd")
                Dim jibumon_cd As New SqlParameter
                jibumon_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_CDColumn.DataType)
                jibumon_cd.Size = _Dt.BUMON_CDColumn.MaxLength
                jibumon_cd.ParameterName = "@bumon_cd"
                jibumon_cd.Value = jibumoncd
                vCmd.Parameters.Add(jibumon_cd)

            End If

            Return lCond.ToString

        End Function
        Public Function InsertData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                     vSeihin As M_USER.M_USERDataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO M_USER (")

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
                Dim user_id As New SqlParameter
                user_id.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_IDColumn.DataType)
                user_id.Size = _Dt.USER_IDColumn.MaxLength
                user_id.ParameterName = "@user_id"
                rCmd.Parameters.Add(user_id)

                Dim user_name As New SqlParameter
                user_name.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_NAMEColumn.DataType)
                user_name.Size = _Dt.USER_NAMEColumn.MaxLength
                user_name.ParameterName = "@user_name"
                rCmd.Parameters.Add(user_name)

                Dim user_kana As New SqlParameter
                user_kana.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_KANAColumn.DataType)
                user_kana.Size = _Dt.USER_KANAColumn.MaxLength
                user_kana.ParameterName = "@user_kana"
                rCmd.Parameters.Add(user_kana)

                Dim user_lvl As New SqlParameter
                user_lvl.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_LEVELColumn.DataType)
                user_lvl.Size = _Dt.USER_LEVELColumn.MaxLength
                user_lvl.ParameterName = "@user_level"
                rCmd.Parameters.Add(user_lvl)

                Dim pas As New SqlParameter
                pas.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.PASSWORDColumn.DataType)
                pas.Size = _Dt.PASSWORDColumn.MaxLength
                pas.ParameterName = "@password"
                rCmd.Parameters.Add(pas)

                Dim bumoncd As New SqlParameter
                bumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_CDColumn.DataType)
                bumoncd.Size = _Dt.BUMON_CDColumn.MaxLength
                bumoncd.ParameterName = "@bumon_cd"
                rCmd.Parameters.Add(bumoncd)

                Dim delflg As New SqlParameter
                delflg.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.DEL_FLGColumn.DataType)
                delflg.Size = _Dt.DEL_FLGColumn.MaxLength
                delflg.ParameterName = "@del_flg"
                rCmd.Parameters.Add(delflg)

                '値をセットする

                user_id.Value = vSeihin(0).USER_ID
                user_name.Value = vSeihin(0).USER_NAME
                user_kana.Value = vSeihin(0).USER_KANA
                user_lvl.Value = vSeihin(0).USER_LEVEL
                pas.Value = vSeihin(0).PASSWORD
                bumoncd.Value = vSeihin(0).BUMON_CD
                delflg.Value = vSeihin(0).DEL_FLG


                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                lCnt = rCmd.ExecuteNonQuery()

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Public Function UpDate(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                               ByRef ldt As M_USER.M_USERDataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" UPDATE M_USER SET")
                lSql.AppendLine("   user_name = @user_name")
                lSql.AppendLine("   ,user_kana = @user_kana")
                lSql.AppendLine("   ,user_level = @user_level")
                lSql.AppendLine("   ,password = @password")
                lSql.AppendLine("   ,bumon_cd = @bumon_cd")
                lSql.AppendLine("   ,del_flg = @del_flg")
                lSql.AppendLine(" WHERE user_id = @user_id")

                '更新パラメータ
                Dim user_id As New SqlParameter
                user_id.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_IDColumn.DataType)
                user_id.Size = _Dt.USER_IDColumn.MaxLength
                user_id.ParameterName = "@user_id"
                rCmd.Parameters.Add(user_id)

                Dim user_name As New SqlParameter
                user_name.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_NAMEColumn.DataType)
                user_name.Size = _Dt.USER_NAMEColumn.MaxLength
                user_name.ParameterName = "@user_name"
                rCmd.Parameters.Add(user_name)

                Dim user_kana As New SqlParameter
                user_kana.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_KANAColumn.DataType)
                user_kana.Size = _Dt.USER_KANAColumn.MaxLength
                user_kana.ParameterName = "@user_kana"
                rCmd.Parameters.Add(user_kana)

                Dim user_lvl As New SqlParameter
                user_lvl.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_LEVELColumn.DataType)
                user_lvl.Size = _Dt.USER_LEVELColumn.MaxLength
                user_lvl.ParameterName = "@user_level"
                rCmd.Parameters.Add(user_lvl)

                Dim pas As New SqlParameter
                pas.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.PASSWORDColumn.DataType)
                pas.Size = _Dt.PASSWORDColumn.MaxLength
                pas.ParameterName = "@password"
                rCmd.Parameters.Add(pas)

                Dim bumoncd As New SqlParameter
                bumoncd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUMON_CDColumn.DataType)
                bumoncd.Size = _Dt.BUMON_CDColumn.MaxLength
                bumoncd.ParameterName = "@bumon_cd"
                rCmd.Parameters.Add(bumoncd)

                Dim delflg As New SqlParameter
                delflg.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.DEL_FLGColumn.DataType)
                delflg.Size = _Dt.DEL_FLGColumn.MaxLength
                delflg.ParameterName = "@del_flg"
                rCmd.Parameters.Add(delflg)

                '値をセットする
                user_id.Value = ldt(0).USER_ID
                user_name.Value = ldt(0).USER_NAME
                user_kana.Value = ldt(0).USER_KANA
                user_lvl.Value = ldt(0).USER_LEVEL
                pas.Value = ldt(0).PASSWORD
                bumoncd.Value = ldt(0).BUMON_CD
                delflg.Value = ldt(0).DEL_FLG


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
            lSql.AppendLine(" DELETE FROM M_USER")
            lSql.AppendLine(" WHERE user_id = @user_id ")

            '更新パラメータ

            Dim user_id As New SqlParameter
            user_id.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_IDColumn.DataType)
            user_id.Size = _Dt.USER_IDColumn.MaxLength
            user_id.ParameterName = "@user_id"
            rCmd.Parameters.Add(user_id)

            '値をセットする
            user_id.Value = code

            rCmd.CommandText = lSql.ToString
            rCmd.Connection = rDb

            lCnt = rCmd.ExecuteNonQuery()

            Return lCnt

        End Function

        ''' <summary>
        ''' ユーザーレベルを取得する
        ''' </summary>
        ''' <param name="vUserId">担当者コード</param>
        ''' <returns></returns>
        Public Function GetUserLevel(ByVal vUserId As String) As Integer
            Dim lRst As Integer = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT ")
            lSql.AppendLine("   USER_LEVEL")
            lSql.AppendLine(" FROM M_USER (NOLOCK)")
            lSql.AppendLine(" WHERE USER_ID = @user_id")

            Dim lDt As New M_USER.M_USERDataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    '--------------------------------------------------------------
                    '検索条件設定
                    '--------------------------------------------------------------
                    lCmd.Parameters.Clear()

                    'ユーザーID
                    Dim user_id As New SqlParameter
                    user_id.SqlDbType = Uty_Dbinfo.ConvertToDbType(lDt.USER_IDColumn.DataType)
                    user_id.Size = lDt.USER_IDColumn.MaxLength
                    user_id.ParameterName = "@user_id"
                    user_id.Value = vUserId
                    lCmd.Parameters.Add(user_id)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    lDb.Open()
                    lRst = lCmd.ExecuteScalar()

                End Using 'lCmd
            End Using 'lDb

            Return lRst
        End Function




    End Class

End Namespace


