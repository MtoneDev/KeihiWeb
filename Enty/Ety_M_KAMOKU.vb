Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text
Imports KeihiWeb.Common.uty
Namespace Enty

    Public Class Ety_M_KAMOKU
        Inherits Ety_Base
        Private _Dt As New DS_M_KAMOKU.M_KAMOKUDataTable
        Public Shadows Function GetData() As DS_M_KAMOKU.M_KAMOKUDataTable

            Try


                Dim condition As String = ""
                Dim queryString As New StringBuilder
                queryString.Append(" SELECT " & Environment.NewLine)
                queryString.Append("     * " & Environment.NewLine)
                queryString.Append(" FROM M_KAMOKU " & Environment.NewLine)

                Dim dt As New DS_M_KAMOKU.M_KAMOKUDataTable

                Using db As New SqlConnection(MyBase.pConnString)
                    Using cmd As New SqlCommand

                        '検索条件設定
                        If condition <> "" Then
                            condition = " WHERE " & condition.Substring(5) & " order by KAMOKU_CD"
                        End If
                        queryString.Append(condition)
                        cmd.CommandText = queryString.ToString
                        cmd.Connection = db

                        'データを取得する
                        Dim lRow As DS_M_KAMOKU.M_KAMOKURow
                        db.Open()
                        Dim reader As DbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While reader.Read()
                            lRow = dt.NewM_KAMOKURow
                            lRow.KAMOKU_CD = reader("KAMOKU_CD")
                            lRow.KAMOKU_NM = reader("KAMOKU_NM")
                            lRow.KAMOKU_DISP = reader("KAMOKU_DISP")
                            lRow.FLG1 = reader("FLG1")
                            lRow.FLG2 = reader("FLG2")
                            lRow.FLG3 = reader("FLG3")
                            lRow.TAX_CD = reader("TAX_CD")
                            lRow.SHOHIZEI_FLG = reader("SHOHIZEI_FLG")
                            dt.AddM_KAMOKURow(lRow)
                        Loop

                    End Using 'cmd
                End Using 'db

                Return dt
            Catch ex As Exception


                Throw New Exception(ex.Message, ex)


            End Try


        End Function

        Public Function SelectData(ByVal vparam As String) As DS_M_KAMOKU.M_KAMOKUDataTable

            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_KAMOKU (NOLOCK)")

                Using lDb As New SqlConnection(MyBase.pConnString)
                    Using lCmd As New SqlCommand    
                        Dim lCondition As String = SetCondition(lCmd, vparam)
                        If lCondition <> "" Then
                            lCondition = " WHERE " & lCondition.Substring(5)
                        End If
                        lSql.AppendLine(lCondition)
                        lSql.AppendLine(" ORDER BY KAMOKU_CD ")
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

            '科目コード
            If code <> String.Empty Then
                lCond.AppendLine(" AND KAMOKU_CD = @kamoku_cd")
                Dim tokucd As New SqlParameter
                tokucd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_CDColumn.DataType)
                tokucd.Size = _Dt.KAMOKU_CDColumn.MaxLength
                tokucd.ParameterName = "@kamoku_cd"
                tokucd.Value = code
                vCmd.Parameters.Add(tokucd)

            End If

            Return lCond.ToString

        End Function

        Public Function ExistData(ByVal vparam As String) As DS_M_KAMOKU.M_KAMOKUDataTable


            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_KAMOKU (NOLOCK)")

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
                                     vSeihin As DS_M_KAMOKU.M_KAMOKUDataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO M_KAMOKU (")

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

                Dim kamoku_name As New SqlParameter
                kamoku_name.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_NMColumn.DataType)
                kamoku_name.Size = _Dt.KAMOKU_NMColumn.MaxLength
                kamoku_name.ParameterName = "@kamoku_nm"
                rCmd.Parameters.Add(kamoku_name)

                Dim disp_name As New SqlParameter
                disp_name.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_DISPColumn.DataType)
                disp_name.Size = _Dt.KAMOKU_DISPColumn.MaxLength
                disp_name.ParameterName = "@kamoku_disp"
                rCmd.Parameters.Add(disp_name)

                Dim flg1 As New SqlParameter
                flg1.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.FLG1Column.DataType)
                flg1.Size = _Dt.FLG1Column.MaxLength
                flg1.ParameterName = "@flg1"
                rCmd.Parameters.Add(flg1)

                Dim flg2 As New SqlParameter
                flg2.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.FLG2Column.DataType)
                flg2.Size = _Dt.FLG2Column.MaxLength
                flg2.ParameterName = "@flg2"
                rCmd.Parameters.Add(flg2)

                Dim flg3 As New SqlParameter
                flg3.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.FLG3Column.DataType)
                flg3.Size = _Dt.FLG3Column.MaxLength
                flg3.ParameterName = "@flg3"
                rCmd.Parameters.Add(flg3)

                Dim taxcd As New SqlParameter
                taxcd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TAX_CDColumn.DataType)
                taxcd.Size = _Dt.TAX_CDColumn.MaxLength
                taxcd.ParameterName = "@tax_cd"
                rCmd.Parameters.Add(taxcd)

                Dim zeicd As New SqlParameter
                zeicd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.SHOHIZEI_FLGColumn.DataType)
                zeicd.Size = _Dt.SHOHIZEI_FLGColumn.MaxLength
                zeicd.ParameterName = "@SHOHIZEI_FLG"
                rCmd.Parameters.Add(zeicd)

                '値をセットする

                kamoku_cd.Value = vSeihin(0).KAMOKU_CD
                kamoku_name.Value = vSeihin(0).KAMOKU_NM
                disp_name.Value = vSeihin(0).KAMOKU_DISP
                flg1.Value = vSeihin(0).FLG1
                flg2.Value = vSeihin(0).FLG2
                flg3.Value = vSeihin(0).FLG3
                taxcd.Value = vSeihin(0).TAX_CD
                zeicd.Value = vSeihin(0).SHOHIZEI_FLG

                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                lCnt = rCmd.ExecuteNonQuery()

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Public Function UpDate(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                               ByRef ldt As DS_M_KAMOKU.M_KAMOKUDataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" UPDATE M_KAMOKU SET")
                lSql.AppendLine("   kamoku_nm = @kamoku_nm")
                lSql.AppendLine("   ,kamoku_disp = @kamoku_disp")
                lSql.AppendLine("   ,flg1 = @flg1")
                lSql.AppendLine("   ,flg2 = @flg2")
                lSql.AppendLine("   ,flg3 = @flg3")
                lSql.AppendLine("   ,tax_cd = @tax_cd")
                lSql.AppendLine("   ,shohizei_flg = @shohizei_flg")
                lSql.AppendLine(" WHERE kamoku_cd = @kamoku_cd")
                '更新パラメータ
                Dim kamoku_cd As New SqlParameter
                kamoku_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_CDColumn.DataType)
                kamoku_cd.Size = _Dt.KAMOKU_CDColumn.MaxLength
                kamoku_cd.ParameterName = "@kamoku_cd"
                rCmd.Parameters.Add(kamoku_cd)

                Dim kamoku_name As New SqlParameter
                kamoku_name.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_NMColumn.DataType)
                kamoku_name.Size = _Dt.KAMOKU_NMColumn.MaxLength
                kamoku_name.ParameterName = "@kamoku_nm"
                rCmd.Parameters.Add(kamoku_name)

                Dim disp_name As New SqlParameter
                disp_name.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_DISPColumn.DataType)
                disp_name.Size = _Dt.KAMOKU_DISPColumn.MaxLength
                disp_name.ParameterName = "@kamoku_disp"
                rCmd.Parameters.Add(disp_name)

                Dim flg1 As New SqlParameter
                flg1.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.FLG1Column.DataType)
                flg1.Size = _Dt.FLG1Column.MaxLength
                flg1.ParameterName = "@flg1"
                rCmd.Parameters.Add(flg1)

                Dim flg2 As New SqlParameter
                flg2.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.FLG2Column.DataType)
                flg2.Size = _Dt.FLG2Column.MaxLength
                flg2.ParameterName = "@flg2"
                rCmd.Parameters.Add(flg2)

                Dim flg3 As New SqlParameter
                flg3.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.FLG3Column.DataType)
                flg3.Size = _Dt.FLG3Column.MaxLength
                flg3.ParameterName = "@flg3"
                rCmd.Parameters.Add(flg3)

                Dim taxcd As New SqlParameter
                taxcd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.TAX_CDColumn.DataType)
                taxcd.Size = _Dt.TAX_CDColumn.MaxLength
                taxcd.ParameterName = "@tax_cd"
                rCmd.Parameters.Add(taxcd)

                Dim zeicd As New SqlParameter
                zeicd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.SHOHIZEI_FLGColumn.DataType)
                zeicd.Size = _Dt.SHOHIZEI_FLGColumn.MaxLength
                zeicd.ParameterName = "@SHOHIZEI_FLG"
                rCmd.Parameters.Add(zeicd)


                '値をセットする
                kamoku_cd.Value = ldt(0).KAMOKU_CD
                kamoku_name.Value = ldt(0).KAMOKU_NM
                disp_name.Value = ldt(0).KAMOKU_DISP
                flg1.Value = ldt(0).FLG1
                flg2.Value = ldt(0).FLG2
                flg3.Value = ldt(0).FLG3
                taxcd.Value = ldt(0).TAX_CD
                zeicd.Value = ldt(0).SHOHIZEI_FLG


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
            lSql.AppendLine(" DELETE FROM M_KAMOKU")
            lSql.AppendLine(" WHERE kamoku_cd = @kamoku_cd ")

            '更新パラメータ

            Dim kamoku_cd As New SqlParameter
            kamoku_cd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KAMOKU_CDColumn.DataType)
            kamoku_cd.Size = _Dt.KAMOKU_CDColumn.MaxLength
            kamoku_cd.ParameterName = "@kamoku_cd"
            rCmd.Parameters.Add(kamoku_cd)

            '値をセットする
            kamoku_cd.Value = code

            rCmd.CommandText = lSql.ToString
            rCmd.Connection = rDb

            lCnt = rCmd.ExecuteNonQuery()

            Return lCnt

        End Function


    End Class


End Namespace
