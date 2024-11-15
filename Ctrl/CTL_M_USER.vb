﻿Imports System.Data.Common
Imports System.Data.SqlClient

Imports KeihiWeb.Enty
Imports KeihiWeb.Common.uty

Namespace Ctrl

    Public Class CTL_M_USER

        Public Shared Function GetData() As M_USER.M_USERDataTable

            Dim lDt As New M_USER.M_USERDataTable
            Dim lEty As New Ety_M_USER

            Try
                'データを取得する
                lDt = lEty.GetData()

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

            Return lDt
        End Function
        Public Shared Function SelectData(ByVal code As String) As M_USER.M_USERDataTable

            Dim lDt As New M_USER.M_USERDataTable
            Dim lEty As New Ety_M_USER

            Try
                'データを取得する
                lDt = lEty.SelectData(code)

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

            Return lDt
        End Function
        Public Shared Function SelectData2(ByVal code As String, ByVal jibumon As String) As M_USER.M_USERDataTable

            Dim lDt As New M_USER.M_USERDataTable
            Dim lEty As New Ety_M_USER

            Try
                'データを取得する
                lDt = lEty.SelectData2(code, jibumon)

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

            Return lDt
        End Function
        Public Shared Function SelectData_bunmon(ByVal code As String) As M_USER.M_USERDataTable

            Dim lDt As New M_USER.M_USERDataTable
            Dim lEty As New Ety_M_USER

            Try
                'データを取得する
                lDt = lEty.SelectData_bumon(code)

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

            Return lDt
        End Function


        Public Shared Function ExistData(ByVal code As String) As Boolean

            Dim lDt As New M_USER.M_USERDataTable
            Dim lEty As New Enty.Ety_M_USER
            Dim result As Boolean
            result = False

            Try
                'データを取得する
                lDt = lEty.ExistData(code)
                If lDt.Count > 0 Then
                    result = True
                End If

            Catch ex As Exception

                Throw New Exception("データ検索に失敗しました。", ex)

            End Try

            Return result

        End Function
        Public Shared Function Insert(ByRef lData As M_USER.M_USERDataTable) As Boolean


            Try

                Dim exist As Boolean
                '存在チェック

                exist = CTL_M_USER.ExistData(lData(0).USER_ID)


                Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                    lDb.Open()

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                '実績更新
                                Dim lEty As New Enty.Ety_M_USER


                                If Not exist Then

                                    lEty.InsertData(lDb, lCmd, lData)
                                    lTx.Commit()

                                End If

                            Catch ex As Exception
                                lTx.Rollback()
                                Throw
                            End Try
                        End Using 'lCmd
                    End Using 'lTx

                End Using 'lDb


                Dim wk As Boolean = False

                If exist = False Then

                    wk = True

                End If

                Return wk


            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End Function

        Public Shared Function UpDate(ByRef lData As M_USER.M_USERDataTable) As Boolean

            Try
                Dim exist As Boolean

                Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                    lDb.Open()

                    '存在チェック
                    exist = CTL_M_USER.ExistData(lData(0).USER_ID)

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                '実績更新
                                Dim lEty As New Enty.Ety_M_USER
                                If exist Then

                                    '明細更新
                                    lEty.UpDate(lDb, lCmd, lData)

                                End If
                                lTx.Commit()

                            Catch ex As Exception
                                lTx.Rollback()
                                Throw
                            End Try
                        End Using 'lCmd
                    End Using 'lTx
                End Using 'lDb

                Dim wk As Boolean = False

                If exist = True Then

                    wk = True

                End If

                Return wk

            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End Function

        Public Shared Function Delete(ByRef code As String) As Boolean

            Try
                Dim exist As Boolean

                Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                    lDb.Open()

                    '存在チェック
                    exist = CTL_M_USER.ExistData(code)

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                '実績更新
                                Dim lEty As New Enty.Ety_M_USER
                                If exist Then

                                    '明細更新
                                    lEty.DeleteData(lDb, lCmd, code)

                                End If
                                lTx.Commit()

                            Catch ex As Exception
                                lTx.Rollback()
                                Throw
                            End Try
                        End Using 'lCmd
                    End Using 'lTx
                End Using 'lDb

                Dim wk As Boolean = False

                If exist = True Then

                    wk = True

                End If

                Return wk


            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End Function



    End Class
End Namespace