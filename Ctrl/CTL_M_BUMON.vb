Imports System.Data.Common
Imports System.Data.SqlClient
Imports KeihiWeb.Common.Logging
Imports KeihiWeb.Enty
Imports KeihiWeb.Common.uty

Namespace Ctrl
    Public Class CTL_M_BUMON
        Public Shared Function GetData() As M_BUMON.M_BUMONDataTable

            Dim lDt As New M_BUMON.M_BUMONDataTable
            Dim lEty As New Ety_M_BUMON

            Try
                'データを取得する
                lDt = lEty.GetData()

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

            Return lDt
        End Function

        Public Shared Function SelectData(ByVal code As String) As M_BUMON.M_BUMONDataTable

            Dim lDt As New M_BUMON.M_BUMONDataTable
            Dim lEty As New Ety_M_BUMON

            Try
                'データを取得する
                lDt = lEty.SelectData(code)

            Catch ex As Exception

                Throw New Exception(ex.Message, ex)

            End Try

            Return lDt
        End Function

        Public Shared Function GetBumonName(vBumonCocd As String) As String
            Dim lRst As String = ""
            Try
                'データを取得する
                Dim lEty As New Ety_M_BUMON
                lRst = lEty.GetBumonName(vBumonCocd)
            Catch ex As Exception
                Logger.WriteErrLog(ex, "", "", "")
                Throw New Exception(ex.Message, ex)
            End Try
            Return lRst
        End Function

        ''' <summary>
        ''' 部門締め情報を取得する
        ''' </summary>
        ''' <param name="vCloseDate">締日</param>
        ''' <param name="vClassNo">業務種別　1:出納帳　2:請求書</param>
        ''' <returns></returns>
        Public Shared Function GetShimeInfo(ByVal vCloseDate As String, ByVal vClassNo As Integer) As M_BUMON.M_BUMON_SHIME_INFODataTable

            Dim lDt As New M_BUMON.M_BUMON_SHIME_INFODataTable
            Dim lEty As New Ety_M_BUMON

            Try
                'データを取得する
                lDt = lEty.GetShimeInfo(vCloseDate, vClassNo)
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try

            Return lDt
        End Function
        Public Shared Function ExistData(ByVal code As String) As Boolean

            Dim lDt As New M_BUMON.M_BUMONDataTable
            Dim lEty As New Enty.Ety_M_BUMON
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


        Public Shared Function Insert(ByRef lData As M_BUMON.M_BUMONDataTable) As Boolean


            Try

                Dim exist As Boolean
                '存在チェック

                exist = CTL_M_BUMON.ExistData(lData(0).BUMON_CD)


                Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                    lDb.Open()

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                '実績更新
                                Dim lEty As New Enty.Ety_M_BUMON


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

        Public Shared Function UpDate(ByRef lData As M_BUMON.M_BUMONDataTable) As Boolean

            Try
                Dim exist As Boolean

                Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                    lDb.Open()

                    '存在チェック
                    exist = CTL_M_BUMON.ExistData(lData(0).BUMON_CD)

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                '実績更新
                                Dim lEty As New Enty.Ety_M_BUMON
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
                    exist = CTL_M_BUMON.ExistData(code)

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                '実績更新
                                Dim lEty As New Enty.Ety_M_BUMON
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
