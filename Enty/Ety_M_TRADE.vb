Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty
Namespace Enty

    Public Class Ety_M_TRADE
        Inherits Ety_Base
        Private _Dt As New DS_M_TRADE.M_TRADEDataTable

        Public Shadows Function GetData() As DS_M_TRADE.M_TRADEDataTable

            Try


                Dim condition As String = ""
                Dim queryString As New StringBuilder
                queryString.Append(" SELECT " & Environment.NewLine)
                queryString.Append("     * " & Environment.NewLine)
                queryString.Append(" FROM M_TRADE " & Environment.NewLine)

                Dim dt As New DS_M_TRADE.M_TRADEDataTable

                Using db As New SqlConnection(MyBase.pConnString)
                    Using cmd As New SqlCommand

                        '検索条件設定
                        If condition <> "" Then
                            condition = " WHERE " & condition.Substring(5) & " order by TRADE_CD"
                        End If
                        queryString.Append(condition)
                        cmd.CommandText = queryString.ToString
                        cmd.Connection = db

                        'データを取得する
                        Dim lRow As DS_M_TRADE.M_TRADERow
                        db.Open()
                        Dim reader As DbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While reader.Read()
                            lRow = dt.NewM_TRADERow
                            lRow.TRADE_CD = reader("TRADE_CD")
                            lRow.TRADE_KN = reader("TRADE_KN")
                            lRow.TRADE_NM = reader("TRADE_NM")
                            lRow.ADDRESS1 = reader("ADDRESS1")
                            lRow.ADDRESS2 = reader("ADDRESS2")
                            lRow.BANK_CD = reader("BANK_CD")
                            lRow.BRANCH_CD = reader("BRANCH_CD")
                            lRow.ACCOUNT_CD = reader("ACCOUNT_CD")
                            lRow.ACC_NAME = reader("ACC_NAME")
                            lRow.ACC_NUM = reader("ACC_NUM")
                            lRow.MIBARAI_CD = reader("MIBARAI_CD")
                            lRow.SHIHARAI_CD = reader("SHIHARAI_CD")
                            lRow.KEIYAKU_NO = reader("KEIYAKU_NO")

                            dt.AddM_TRADERow(lRow)
                        Loop

                    End Using 'cmd
                End Using 'db

                Return dt
            Catch ex As Exception


                Throw New Exception(ex.Message, ex)


            End Try


        End Function
        Public Shadows Function GetData(ByVal name As String, ByVal kana As String) As DS_M_TRADE.M_TRADEDataTable

            Try


                Dim condition As String = ""
                Dim queryString As New StringBuilder
                queryString.Append(" SELECT top 300 " & Environment.NewLine)
                queryString.Append("     * " & Environment.NewLine)
                queryString.Append(" FROM M_TRADE " & Environment.NewLine)
                If name <> "" Then

                    queryString.Append("where TRADE_NM LIKE '%" + name + "%'" & Environment.NewLine)

                    If kana <> "" Then
                        queryString.Append(" AND TRADE_KN LIKE '%" + kana + "%'" & Environment.NewLine)
                    End If

                Else

                    If kana <> "" Then
                        queryString.Append("where TRADE_KN LIKE '%" + kana + "%'" & Environment.NewLine)
                    End If

                End If


                Dim dt As New DS_M_TRADE.M_TRADEDataTable

                Using db As New SqlConnection(MyBase.pConnString)
                    Using cmd As New SqlCommand

                        '検索条件設定
                        'queryString.Append(condition)
                        cmd.CommandText = queryString.ToString
                        cmd.Connection = db

                        'データを取得する
                        Dim lRow As DS_M_TRADE.M_TRADERow
                        db.Open()
                        Dim reader As DbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While reader.Read()
                            lRow = dt.NewM_TRADERow
                            lRow.TRADE_CD = reader("TRADE_CD")
                            lRow.TRADE_KN = reader("TRADE_KN")
                            lRow.TRADE_NM = reader("TRADE_NM")
                            lRow.ADDRESS1 = reader("ADDRESS1")
                            lRow.ADDRESS2 = reader("ADDRESS2")
                            lRow.BANK_CD = reader("BANK_CD")
                            lRow.BRANCH_CD = reader("BRANCH_CD")
                            lRow.ACCOUNT_CD = reader("ACCOUNT_CD")
                            lRow.ACC_NAME = reader("ACC_NAME")
                            lRow.ACC_NUM = reader("ACC_NUM")
                            lRow.MIBARAI_CD = reader("MIBARAI_CD")
                            lRow.SHIHARAI_CD = reader("SHIHARAI_CD")
                            lRow.KEIYAKU_NO = reader("KEIYAKU_NO")

                            dt.AddM_TRADERow(lRow)
                        Loop

                    End Using 'cmd
                End Using 'db

                Return dt
            Catch ex As Exception


                Throw New Exception(ex.Message, ex)


            End Try


        End Function


        Public Function ExistData(ByVal vparam As String) As DS_M_TRADE.M_TRADEDataTable


            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_TRADE (NOLOCK)")

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
        Public Function SelectData(ByVal vparam As String) As DS_M_TRADE.M_TRADEDataTable

            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_TRADE (NOLOCK)")

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

            '取引先コード
            If code <> String.Empty Then
                lCond.AppendLine(" AND TRADE_CD = @trade_cd")
                Dim tokucd As New SqlParameter
                tokucd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TRADE_CDColumn.DataType)
                tokucd.Size = _Dt.TRADE_CDColumn.MaxLength
                tokucd.ParameterName = "@trade_cd"
                tokucd.Value = code
                vCmd.Parameters.Add(tokucd)

            End If

            Return lCond.ToString

        End Function


        Public Function InsertData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                     vSeihin As DS_M_TRADE.M_TRADEDataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO M_TRADE (")

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
                Dim trade_cd As New SqlParameter
                trade_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TRADE_CDColumn.DataType)
                trade_cd.Size = _Dt.TRADE_CDColumn.MaxLength
                trade_cd.ParameterName = "@trade_cd"
                rCmd.Parameters.Add(trade_cd)

                Dim trade_nm As New SqlParameter
                trade_nm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TRADE_NMColumn.DataType)
                trade_nm.Size = _Dt.TRADE_NMColumn.MaxLength
                trade_nm.ParameterName = "@trade_nm"
                rCmd.Parameters.Add(trade_nm)

                Dim trade_kn As New SqlParameter
                trade_kn.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TRADE_KNColumn.DataType)
                trade_kn.Size = _Dt.TRADE_KNColumn.MaxLength
                trade_kn.ParameterName = "@trade_kn"
                rCmd.Parameters.Add(trade_kn)

                Dim address1 As New SqlParameter
                address1.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ADDRESS1Column.DataType)
                address1.Size = _Dt.ADDRESS1Column.MaxLength
                address1.ParameterName = "@address1"
                rCmd.Parameters.Add(address1)

                Dim address2 As New SqlParameter
                address2.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ADDRESS2Column.DataType)
                address2.Size = _Dt.ADDRESS2Column.MaxLength
                address2.ParameterName = "@address2"
                rCmd.Parameters.Add(address2)

                Dim bank_cd As New SqlParameter
                bank_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BANK_CDColumn.DataType)
                bank_cd.Size = _Dt.BANK_CDColumn.MaxLength
                bank_cd.ParameterName = "@bank_cd"
                rCmd.Parameters.Add(bank_cd)

                Dim branch_cd As New SqlParameter
                branch_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BRANCH_CDColumn.DataType)
                branch_cd.Size = _Dt.BRANCH_CDColumn.MaxLength
                branch_cd.ParameterName = "@branch_cd"
                rCmd.Parameters.Add(branch_cd)

                Dim acc_cd As New SqlParameter
                acc_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ACCOUNT_CDColumn.DataType)
                acc_cd.Size = _Dt.ACCOUNT_CDColumn.MaxLength
                acc_cd.ParameterName = "@account_cd"
                rCmd.Parameters.Add(acc_cd)

                Dim acc_no As New SqlParameter
                acc_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ACC_NUMColumn.DataType)
                acc_no.Size = _Dt.ACC_NUMColumn.MaxLength
                acc_no.ParameterName = "@acc_num"
                rCmd.Parameters.Add(acc_no)


                Dim acc_nm As New SqlParameter
                acc_nm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ACC_NAMEColumn.DataType)
                acc_nm.Size = _Dt.ACC_NAMEColumn.MaxLength
                acc_nm.ParameterName = "@acc_name"
                rCmd.Parameters.Add(acc_nm)

                Dim mi_cd As New SqlParameter
                mi_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.MIBARAI_CDColumn.DataType)
                mi_cd.Size = _Dt.MIBARAI_CDColumn.MaxLength
                mi_cd.ParameterName = "@mibarai_cd"
                rCmd.Parameters.Add(mi_cd)

                Dim shi_cd As New SqlParameter
                shi_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.SHIHARAI_CDColumn.DataType)
                shi_cd.Size = _Dt.SHIHARAI_CDColumn.MaxLength
                shi_cd.ParameterName = "@shiharai_cd"
                rCmd.Parameters.Add(shi_cd)

                Dim kei_no As New SqlParameter
                kei_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KEIYAKU_NOColumn.DataType)
                kei_no.Size = _Dt.KEIYAKU_NOColumn.MaxLength
                kei_no.ParameterName = "@keiyaku_no"
                rCmd.Parameters.Add(kei_no)

                trade_cd.Value = vSeihin(0).TRADE_CD
                trade_nm.Value = vSeihin(0).TRADE_NM
                trade_kn.Value = vSeihin(0).TRADE_KN
                address1.Value = vSeihin(0).ADDRESS1
                address2.Value = vSeihin(0).ADDRESS2
                bank_cd.Value = vSeihin(0).BANK_CD
                branch_cd.Value = vSeihin(0).BRANCH_CD
                acc_cd.Value = vSeihin(0).ACCOUNT_CD
                acc_no.Value = vSeihin(0).ACC_NUM
                acc_nm.Value = vSeihin(0).ACC_NAME
                mi_cd.Value = vSeihin(0).MIBARAI_CD
                shi_cd.Value = vSeihin(0).SHIHARAI_CD
                kei_no.Value = vSeihin(0).KEIYAKU_NO

                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                lCnt = rCmd.ExecuteNonQuery()

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Public Function UpDate(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                               ByRef pdt As DS_M_TRADE.M_TRADEDataTable) As Integer

            Try

                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" UPDATE M_TRADE SET")
                lSql.AppendLine("   trade_nm = @trade_nm")
                lSql.AppendLine("   ,trade_kn = @trade_kn")
                lSql.AppendLine("   ,address1 = @address1")
                lSql.AppendLine("   ,address2 = @address2")
                lSql.AppendLine("   ,bank_cd = @bank_cd")
                lSql.AppendLine("   ,branch_cd = @branch_cd")
                lSql.AppendLine("   ,account_cd = @account_cd")
                lSql.AppendLine("   ,acc_num = @acc_num")
                lSql.AppendLine("   ,acc_name = @acc_name")
                lSql.AppendLine("   ,mibarai_cd = @mibarai_cd")
                lSql.AppendLine("   ,shiharai_cd = @shiharai_cd")
                lSql.AppendLine("   ,keiyaku_no = @keiyaku_no")
                lSql.AppendLine(" WHERE TRADE_CD = @trade_cd")

                '更新パラメータ
                Dim trade_cd As New SqlParameter
                trade_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TRADE_CDColumn.DataType)
                trade_cd.Size = _Dt.TRADE_CDColumn.MaxLength
                trade_cd.ParameterName = "@trade_cd"
                rCmd.Parameters.Add(trade_cd)

                Dim trade_nm As New SqlParameter
                trade_nm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TRADE_NMColumn.DataType)
                trade_nm.Size = _Dt.TRADE_NMColumn.MaxLength
                trade_nm.ParameterName = "@trade_nm"
                rCmd.Parameters.Add(trade_nm)

                Dim trade_kn As New SqlParameter
                trade_kn.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TRADE_KNColumn.DataType)
                trade_kn.Size = _Dt.TRADE_KNColumn.MaxLength
                trade_kn.ParameterName = "@trade_kn"
                rCmd.Parameters.Add(trade_kn)

                Dim address1 As New SqlParameter
                address1.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ADDRESS1Column.DataType)
                address1.Size = _Dt.ADDRESS1Column.MaxLength
                address1.ParameterName = "@address1"
                rCmd.Parameters.Add(address1)

                Dim address2 As New SqlParameter
                address2.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ADDRESS2Column.DataType)
                address2.Size = _Dt.ADDRESS2Column.MaxLength
                address2.ParameterName = "@address2"
                rCmd.Parameters.Add(address2)

                Dim bank_cd As New SqlParameter
                bank_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BANK_CDColumn.DataType)
                bank_cd.Size = _Dt.BANK_CDColumn.MaxLength
                bank_cd.ParameterName = "@bank_cd"
                rCmd.Parameters.Add(bank_cd)

                Dim branch_cd As New SqlParameter
                branch_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BRANCH_CDColumn.DataType)
                branch_cd.Size = _Dt.BRANCH_CDColumn.MaxLength
                branch_cd.ParameterName = "@branch_cd"
                rCmd.Parameters.Add(branch_cd)

                Dim acc_cd As New SqlParameter
                acc_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ACCOUNT_CDColumn.DataType)
                acc_cd.Size = _Dt.ACCOUNT_CDColumn.MaxLength
                acc_cd.ParameterName = "@account_cd"
                rCmd.Parameters.Add(acc_cd)

                Dim acc_no As New SqlParameter
                acc_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ACC_NUMColumn.DataType)
                acc_no.Size = _Dt.ACC_NUMColumn.MaxLength
                acc_no.ParameterName = "@acc_num"
                rCmd.Parameters.Add(acc_no)


                Dim acc_nm As New SqlParameter
                acc_nm.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ACC_NAMEColumn.DataType)
                acc_nm.Size = _Dt.ACC_NAMEColumn.MaxLength
                acc_nm.ParameterName = "@acc_name"
                rCmd.Parameters.Add(acc_nm)

                Dim mi_cd As New SqlParameter
                mi_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.MIBARAI_CDColumn.DataType)
                mi_cd.Size = _Dt.MIBARAI_CDColumn.MaxLength
                mi_cd.ParameterName = "@mibarai_cd"
                rCmd.Parameters.Add(mi_cd)

                Dim shi_cd As New SqlParameter
                shi_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.SHIHARAI_CDColumn.DataType)
                shi_cd.Size = _Dt.SHIHARAI_CDColumn.MaxLength
                shi_cd.ParameterName = "@shiharai_cd"
                rCmd.Parameters.Add(shi_cd)

                Dim kei_no As New SqlParameter
                kei_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KEIYAKU_NOColumn.DataType)
                kei_no.Size = _Dt.KEIYAKU_NOColumn.MaxLength
                kei_no.ParameterName = "@keiyaku_no"
                rCmd.Parameters.Add(kei_no)

                trade_cd.Value = pdt(0).TRADE_CD
                trade_nm.Value = pdt(0).TRADE_NM
                trade_kn.Value = pdt(0).TRADE_KN
                address1.Value = pdt(0).ADDRESS1
                address2.Value = pdt(0).ADDRESS2
                bank_cd.Value = pdt(0).BANK_CD
                branch_cd.Value = pdt(0).BRANCH_CD
                acc_cd.Value = pdt(0).ACCOUNT_CD
                acc_no.Value = pdt(0).ACC_NUM
                acc_nm.Value = pdt(0).ACC_NAME
                mi_cd.Value = pdt(0).MIBARAI_CD
                shi_cd.Value = pdt(0).SHIHARAI_CD
                kei_no.Value = pdt(0).KEIYAKU_NO

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
            lSql.AppendLine(" DELETE FROM M_TRADE")
            lSql.AppendLine(" WHERE TRADE_CD = @trade_cd")

            '更新パラメータ
            Dim trade_cd As New SqlParameter
            trade_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TRADE_CDColumn.DataType)
            trade_cd.Size = _Dt.TRADE_CDColumn.MaxLength
            trade_cd.ParameterName = "@trade_cd"
            rCmd.Parameters.Add(trade_cd)

            '値をセットする
            trade_cd.Value = code

            rCmd.CommandText = lSql.ToString
            rCmd.Connection = rDb

            lCnt = rCmd.ExecuteNonQuery()

            Return lCnt

        End Function



    End Class

End Namespace

