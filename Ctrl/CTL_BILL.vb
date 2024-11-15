Imports KeihiWeb.Common.Logging
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Enty
Imports KeihiWeb.Report
Imports System.Data.Common
Imports System.Data.SqlClient

Namespace Ctrl
    Public Class CTL_BILL

        Public Shared Function Print_SeikyuShiwake(vJiBumonCd As String, vInputDateFrom As String, vInputDateTo As String, Optional vHakkoFlg As Integer = 0) As String
            Try
                Dim lShimebi As String = Uty_Common.ChangeDateToString(vInputDateTo)
                Dim lFilename As String = "Rpt_SeikyuShiwake_" & vJiBumonCd & "_" & lShimebi & "_" & vHakkoFlg & Now.ToString("ssfff") & ".xlsx"
                Dim lPath As String = Uty_File.AddFolderMark(HttpContext.Current.Server.MapPath("../") & Uty_Config.OutputDir) & lFilename
                Dim lParam As Rpt_SeikyuShiwake.RptParam
                lParam.JIBUMON_CD = vJiBumonCd
                lParam.INPUT_DATE_FROM = vInputDateFrom
                lParam.INPUT_DATE_TO = vInputDateTo
                lParam.HAKKO_FLG = vHakkoFlg

                Dim lRpt As New Rpt_SeikyuShiwake(lParam)
                lRpt.Output(lPath)

                Return lPath
            Catch ex As Exception
                Logger.WriteErrLog(ex, "", "", "")
                Throw
            End Try
        End Function

        Public Shared Function GetDenpyo_NO(vJiBumon_CD As String, vInput_YM As String) As DS_BILL.CT_BILL1_DENPYODataTable
            Dim lEty As New Ety_CT_BILL1
            Return lEty.GetDenpyo_NO(vJiBumon_CD, vInput_YM)
        End Function
        Public Shared Function ExistDenpyo_NO(ByVal vJiBumon_CD As String, ByVal vInput_YM As String, ByVal vSlip_NO As String) As DS_BILL.CT_BILL1DataTable

            Try

                Dim Ety As New Ety_CT_BILL1
                Dim lDt As New DS_BILL.CT_BILL1DataTable

                lDt = Ety.ExistDenpyo_NO(vJiBumon_CD, vInput_YM, vSlip_NO)

                Return lDt

            Catch ex As Exception
                Logger.WriteErrLog(ex, "", "", "")
                Throw
            End Try
        End Function


        Public Shared Sub InsertData(vParam1 As DS_BILL.CT_BILL1DataTable, vParam2 As DS_BILL.CT_BILL2DataTable)
            Try
                '更新日時、更新者IDを設定する


                Using lDb As New SqlConnection(Uty_Config.ConnectionString)
                    lDb.Open()

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 600
                            lCmd.Transaction = lTx
                            Try
                                Dim lEty1 As New Ety_CT_BILL1
                                Dim lEty2 As New Ety_CT_BILL2
                                '明細更新
                                lEty2.InsertData(lDb, lCmd, vParam2)
                                'ヘッダー更新
                                lEty1.InsertData(lDb, lCmd, vParam1)

                                lTx.Commit()
                            Catch ex As Exception
                                lTx.Rollback()
                                Throw
                            End Try
                        End Using 'lCmd
                    End Using 'lTx
                End Using 'lDb

            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Sub

        Public Shared Sub UpdateData(vParam As DS_BILL.SearchParam_ListDataTable, vParam1 As DS_BILL.CT_BILL1DataTable, vParam2 As DS_BILL.CT_BILL2DataTable)
            Try
                '更新日時、更新者IDを設定する


                Using lDb As New SqlConnection(Uty_Config.ConnectionString)
                    lDb.Open()

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 600
                            lCmd.Transaction = lTx
                            Try
                                Dim lEty1 As New Ety_CT_BILL1
                                Dim lEty2 As New Ety_CT_BILL2
                                '明細削除更新
                                lEty2.DeleteData(lDb, lCmd, vParam)
                                'ヘッダー更新
                                lEty1.DeleteData(lDb, lCmd, vParam)

                                '明細更新
                                lEty2.InsertData(lDb, lCmd, vParam2)
                                'ヘッダー更新
                                lEty1.InsertData(lDb, lCmd, vParam1)

                                lTx.Commit()
                            Catch ex As Exception
                                lTx.Rollback()
                                Throw
                            End Try
                        End Using 'lCmd
                    End Using 'lTx
                End Using 'lDb

            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Sub


        Public Shared Sub DeleteData(vParam As DS_BILL.SearchParam_ListDataTable)
            Try
                '更新日時、更新者IDを設定する


                Using lDb As New SqlConnection(Uty_Config.ConnectionString)
                    lDb.Open()

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 600
                            lCmd.Transaction = lTx
                            Try
                                Dim lEty1 As New Ety_CT_BILL1
                                Dim lEty2 As New Ety_CT_BILL2
                                '明細削除更新
                                lEty2.DeleteData(lDb, lCmd, vParam)
                                'ヘッダー更新
                                lEty1.DeleteData(lDb, lCmd, vParam)

                                lTx.Commit()
                            Catch ex As Exception
                                lTx.Rollback()
                                Throw
                            End Try
                        End Using 'lCmd
                    End Using 'lTx
                End Using 'lDb

            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Sub

        Public Shared Function MeisaiSelectData(vParam As DS_BILL.SearchParamDataTable) As DS_BILL.CT_MEISAI_DISPDataTable

            Try
                '更新日時、更新者IDを設定する

                Dim Ety As New Ety_CT_BILL2
                Dim lDt As New DS_BILL.CT_MEISAI_DISPDataTable

                lDt = Ety.MeisaiSelectData(vParam)

                Return lDt


            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Function

        Public Shared Function SelectData(vParam As DS_BILL.SearchParamDataTable) As DS_BILL.CT_SELECTBILLDataTable

            Try
                '更新日時、更新者IDを設定する

                Dim Ety As New Ety_CT_BILL2
                Dim lDt As New DS_BILL.CT_SELECTBILLDataTable

                lDt = Ety.SelectData(vParam)

                Return lDt


            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Function

        Public Shared Function SelectData2(vParam As DS_BILL.SearchParamDataTable) As DS_BILL.CT_HEADER_DISPDataTable
            'ヘッダ表示用
            Try
                '更新日時、更新者IDを設定する

                Dim Ety As New Ety_CT_BILL2
                Dim lDt As New DS_BILL.CT_HEADER_DISPDataTable

                lDt = Ety.SelectData2(vParam)

                Return lDt

            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Function

        Public Shared Function SelectData_List(vParam As DS_BILL.SearchParam_ListDataTable) As DS_BILL.CT_SELECTBILLDataTable

            Try
                '更新日時、更新者IDを設定する

                Dim Ety As New Ety_CT_BILL2
                Dim lDt As New DS_BILL.CT_SELECTBILLDataTable

                lDt = Ety.SelectData_List(vParam)

                Return lDt


            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Function





    End Class
End Namespace
