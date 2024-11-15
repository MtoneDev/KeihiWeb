Imports System.Data.Common
Imports System.Data.SqlClient
Imports KeihiWeb.Common.Logging
Imports KeihiWeb.Enty
Imports KeihiWeb.Common.uty

Namespace Ctrl
    Public Class CTL_M_ZEIRITU

        Public Shared Function GetData() As DS_M_ZEIRITU.M_ZEIRITUDataTable

            Dim lDt As New DS_M_ZEIRITU.M_ZEIRITUDataTable
            Dim lEty As New Ety_M_ZEIRITU

            Try
                'データを取得する
                lDt = lEty.GetData()

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

            Return lDt
        End Function

        Public Shared Function GetZeiritu(ByRef vparam As DS_M_ZEIRITU.M_ZEIRITUKeyDataTable) As DS_M_ZEIRITU.M_ZEIRITUDataTable

            Dim lDt As New DS_M_ZEIRITU.M_ZEIRITUDataTable
            Dim lEty As New Ety_M_ZEIRITU

            Try
                'データを取得する
                lDt = lEty.GetZeiritu(vparam)

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

            Return lDt
        End Function


        Public Shared Function ExistData(ByRef vparam As DS_M_ZEIRITU.M_ZEIRITUDataTable) As Boolean

            Dim lDt As New DS_M_ZEIRITU.M_ZEIRITUDataTable
            Dim lEty As New Enty.Ety_M_ZEIRITU
            Dim result As Boolean
            result = False

            Try
                'データを取得する
                lDt = lEty.ExistData(vparam)
                If lDt.Count > 0 Then
                    result = True
                End If

            Catch ex As Exception

                Throw New Exception("データ検索に失敗しました。", ex)

            End Try

            Return result

        End Function
        Public Shared Function Insert(ByRef lData As DS_M_ZEIRITU.M_ZEIRITUDataTable) As Boolean


            Try

                Dim exist As Boolean
                '存在チェック

                exist = CTL_M_ZEIRITU.ExistData(lData)


                Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                    lDb.Open()

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                '実績更新
                                Dim lEty As New Enty.Ety_M_ZEIRITU


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


        Public Shared Function UpDate(ByRef lData_old As DS_M_ZEIRITU.M_ZEIRITUDataTable, ByRef lData_new As DS_M_ZEIRITU.M_ZEIRITUDataTable) As Boolean


            Try

                Dim exist As Boolean
                '存在チェック

                exist = CTL_M_ZEIRITU.ExistData(lData_new)


                Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                    lDb.Open()

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                '実績更新
                                Dim lEty As New Enty.Ety_M_ZEIRITU


                                ' If Not exist Then

                                'lEty.InsertData(lDb, lCmd, lData_new)
                                'lTx.Commit()
                                'Else
                                lEty.DeleteData(lDb, lCmd, lData_old)
                                lEty.InsertData(lDb, lCmd, lData_new)
                                lTx.Commit()

                                'End If

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


    End Class



End Namespace
