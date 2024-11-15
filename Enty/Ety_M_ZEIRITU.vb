Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty

Namespace Enty
    Public Class Ety_M_ZEIRITU
        Inherits Ety_Base
        Private _Dt As New DS_M_ZEIRITU.M_ZEIRITUDataTable

        Public Shadows Function GetData() As DS_M_ZEIRITU.M_ZEIRITUDataTable

            Try

                Dim condition As String = ""
                Dim queryString As New StringBuilder
                queryString.Append(" SELECT " & Environment.NewLine)
                queryString.Append("     * " & Environment.NewLine)
                queryString.Append(" FROM M_ZEIRITU " & Environment.NewLine)

                Dim dt As New DS_M_ZEIRITU.M_ZEIRITUDataTable

                Using db As New SqlConnection(MyBase.pConnString)
                    Using cmd As New SqlCommand

                        '検索条件設定
                        'If condition <> "" Then
                        '    condition = " WHERE " & condition.Substring(5) & " order by USER_ID"
                        'End If
                        queryString.Append(condition)
                        cmd.CommandText = queryString.ToString
                        cmd.Connection = db

                        'データを取得する
                        Dim lRow As DS_M_ZEIRITU.M_ZEIRITURow
                        db.Open()
                        Dim reader As DbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While reader.Read()
                            lRow = dt.NewM_ZEIRITURow
                            lRow.BUNRUICD = reader("BUNRUICD")
                            lRow.ZEIRITU = reader("ZEIRITU")
                            lRow.KIRIKAEDT = reader("KIRIKAEDT")
                            lRow.KIGENDT = reader("KIGENDT")
                            dt.AddM_ZEIRITURow(lRow)
                        Loop

                    End Using 'cmd
                End Using 'db

                Return dt
            Catch ex As Exception


                Throw New Exception(ex.Message, ex)


            End Try


        End Function


        Public Function GetZeiritu(ByVal vParam As DS_M_ZEIRITU.M_ZEIRITUKeyDataTable) As DS_M_ZEIRITU.M_ZEIRITUDataTable
            Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
            Dim lSql As New StringBuilder

            lSql.AppendLine("SELECT *")
            lSql.AppendLine(" FROM M_ZEIRITU (NOLOCK)")
            lSql.AppendLine(" WHERE BUNRUICD = @bunruicd ")
            lSql.AppendLine("   AND KIRIKAEDT <= @inputdate ")
            lSql.AppendLine("   AND KIGENDT >= @inputdate ")


            '検索パラメータ


            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand
                    lCmd.Parameters.Clear()

                    Dim bunruicd As New SqlParameter
                    bunruicd.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUNRUICDColumn.DataType)
                    bunruicd.Size = _Dt.BUNRUICDColumn.MaxLength
                    bunruicd.ParameterName = "@bunruicd"
                    lCmd.Parameters.Add(bunruicd)
                    bunruicd.Value = vParam(0).BUNRUICD

                    Dim inputdt As New SqlParameter
                    inputdt.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KIGENDTColumn.DataType)
                    inputdt.Size = _Dt.KIRIKAEDTColumn.MaxLength
                    inputdt.ParameterName = "@inputdate"
                    lCmd.Parameters.Add(inputdt)
                    inputdt.Value = vParam(0).INPUTDATE


                    '値をセットする


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

        End Function
        Private Function SetCondition(ByRef vCmd As SqlCommand, ByRef vparam As DS_M_ZEIRITU.M_ZEIRITUDataTable) As String


            Dim lCond As New StringBuilder
            Dim lEscChar As String = Uty_Config.SQLESC

            vCmd.Parameters.Clear()

            '分類
            If vparam(0).BUNRUICD <> String.Empty Then
                lCond.AppendLine(" AND BUNRUICD = @bunruicd")
                Dim bunrui As New SqlParameter
                bunrui.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUNRUICDColumn.DataType)
                bunrui.Size = _Dt.BUNRUICDColumn.MaxLength
                bunrui.ParameterName = "@bunruicd"
                bunrui.Value = vparam(0).BUNRUICD
                vCmd.Parameters.Add(bunrui)

            End If

            lCond.AppendLine(" AND KIRIKAEDT = @kirikaedt")
            Dim startdate As New SqlParameter
            startdate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KIRIKAEDTColumn.DataType)
            startdate.Size = _Dt.KIRIKAEDTColumn.MaxLength
            startdate.ParameterName = "@kirikaedt"
            startdate.Value = vparam(0).KIRIKAEDT
            vCmd.Parameters.Add(startdate)

            lCond.AppendLine(" AND KIGENDT = @kigendt")
            Dim enddate As New SqlParameter
            enddate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KIGENDTColumn.DataType)
            enddate.Size = _Dt.KIGENDTColumn.MaxLength
            enddate.ParameterName = "@kigendt"
            enddate.Value = vparam(0).KIGENDT
            vCmd.Parameters.Add(enddate)


            Return lCond.ToString

        End Function

        Public Function ExistData(ByRef vparam As DS_M_ZEIRITU.M_ZEIRITUDataTable) As DS_M_ZEIRITU.M_ZEIRITUDataTable

            Try

                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)
                Dim lSql As New StringBuilder

                lSql.AppendLine("SELECT * ")
                lSql.AppendLine(" FROM M_ZEIRITU (NOLOCK)")

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
                                     vSeihin As DS_M_ZEIRITU.M_ZEIRITUDataTable) As Integer

            Try


                Dim lCnt As Integer = 0
                Dim lSql As New StringBuilder
                Dim ar As ArrayList = Uty_Dbinfo.GetFieldName(_Dt)

                rCmd.Parameters.Clear()

                'SQL文を生成する
                lSql.AppendLine(" INSERT INTO M_ZEIRITU (")

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
                Dim bunrui As New SqlParameter
                bunrui.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUNRUICDColumn.DataType)
                bunrui.Size = _Dt.BUNRUICDColumn.MaxLength
                bunrui.ParameterName = "@bunruicd"
                rCmd.Parameters.Add(bunrui)

                Dim startdate As New SqlParameter
                startdate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KIRIKAEDTColumn.DataType)
                startdate.Size = _Dt.KIRIKAEDTColumn.MaxLength
                startdate.ParameterName = "@kirikaedt"
                rCmd.Parameters.Add(startdate)

                Dim zeiritu As New SqlParameter
                zeiritu.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.ZEIRITUColumn.DataType)
                zeiritu.Size = _Dt.ZEIRITUColumn.MaxLength
                zeiritu.ParameterName = "@zeiritu"
                rCmd.Parameters.Add(zeiritu)

                Dim enddate As New SqlParameter
                enddate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KIGENDTColumn.DataType)
                enddate.Size = _Dt.KIGENDTColumn.MaxLength
                enddate.ParameterName = "@kigendt"
                rCmd.Parameters.Add(enddate)

                '値をセットする

                bunrui.Value = vSeihin(0).BUNRUICD
                startdate.Value = vSeihin(0).KIRIKAEDT
                zeiritu.Value = vSeihin(0).ZEIRITU
                enddate.Value = vSeihin(0).KIGENDT

                rCmd.CommandText = lSql.ToString
                rCmd.Connection = rDb

                lCnt = rCmd.ExecuteNonQuery()

                Return lCnt

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

        End Function

        Public Function DeleteData(ByRef rDb As SqlConnection, ByRef rCmd As SqlCommand,
                                  vSeihin As DS_M_ZEIRITU.M_ZEIRITUDataTable) As Integer

            Dim lCnt As Integer = 0
            Dim lSql As New StringBuilder

            rCmd.Parameters.Clear()

            'SQL文を生成する
            lSql.AppendLine(" DELETE FROM M_ZEIRITU")
            lSql.AppendLine(" WHERE bunruicd = @bunruicd ")
            lSql.AppendLine(" AND kirikaedt = @kirikaedt ")
            lSql.AppendLine(" AND kigendt = @kigendt ")

            '更新パラメータ

            Dim bunrui As New SqlParameter
            bunrui.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.BUNRUICDColumn.DataType)
            bunrui.Size = _Dt.BUNRUICDColumn.MaxLength
            bunrui.ParameterName = "@bunruicd"
            rCmd.Parameters.Add(bunrui)

            Dim startdate As New SqlParameter
            startdate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KIRIKAEDTColumn.DataType)
            startdate.Size = _Dt.KIRIKAEDTColumn.MaxLength
            startdate.ParameterName = "@kirikaedt"
            rCmd.Parameters.Add(startdate)

            Dim enddate As New SqlParameter
            enddate.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.KIGENDTColumn.DataType)
            enddate.Size = _Dt.KIGENDTColumn.MaxLength
            enddate.ParameterName = "@kigendt"
            rCmd.Parameters.Add(enddate)

            '値をセットする
            bunrui.Value = vSeihin(0).BUNRUICD
            startdate.Value = vSeihin(0).KIRIKAEDT
            enddate.Value = vSeihin(0).KIGENDT


            rCmd.CommandText = lSql.ToString
            rCmd.Connection = rDb

            lCnt = rCmd.ExecuteNonQuery()

            Return lCnt

        End Function


    End Class
End Namespace
