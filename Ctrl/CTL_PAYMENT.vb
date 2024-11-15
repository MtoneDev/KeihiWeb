Imports KeihiWeb.Common.Logging
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Enty
Imports KeihiWeb.Report
Imports System.Data.Common
Imports System.Data.SqlClient

Namespace Ctrl
    Public Class CTL_PAYMENT

        ''' <summary>
        ''' 科目別出納帳を出力する（xslx）
        ''' </summary>
        ''' <param name="vJiBumonCd">自部門コード</param>
        ''' <param name="vInputDateFrom">締め期間開始日</param>
        ''' <param name="vInputDateTo">締め期間終了日</param>
        ''' <param name="vHakkoFlg">1:本発行　0:仮または再発行</param>
        ''' <returns>ファイルパス</returns>
        Public Shared Function Print_Suitou_KamokuBetsu(vJiBumonCd As String, vInputDateFrom As String, vInputDateTo As String, Optional vHakkoFlg As Integer = 0) As String
            Try
                Dim lShimebi As String = Uty_Common.ChangeDateToString(vInputDateTo)
                Dim lFilename As String = "Rpt_Suitou_KamokuBetsu_" & vJiBumonCd & "_" & lShimebi & "_" & vHakkoFlg & Now.ToString("ssfff") & ".xlsx"
                Dim lPath As String = Uty_File.AddFolderMark(HttpContext.Current.Server.MapPath("../") & Uty_Config.OutputDir) & lFilename
                Dim lParam As Rpt_Suitou_KamokuBetsu.RptParam
                lParam.JIBUMON_CD = vJiBumonCd
                lParam.INPUT_DATE_FROM = vInputDateFrom
                lParam.INPUT_DATE_TO = vInputDateTo
                lParam.HAKKO_FLG = vHakkoFlg

                Dim lRpt As New Rpt_Suitou_KamokuBetsu(lParam)
                lRpt.Output(lPath)

                Return lPath
            Catch ex As Exception
                Logger.WriteErrLog(ex, "", "", "")
                Throw
            End Try
        End Function

        Public Shared Function Print_Suitou(vJiBumonCd As String, vInputDateFrom As String, vInputDateTo As String, Optional vHakkoFlg As Integer = 0) As String
            Try
                Dim lShimebi As String = Uty_Common.ChangeDateToString(vInputDateTo)
                Dim lFilename As String = "Rpt_Suitou_" & vJiBumonCd & "_" & lShimebi & "_" & vHakkoFlg & Now.ToString("ssfff") & ".xlsx"
                Dim lPath As String = Uty_File.AddFolderMark(HttpContext.Current.Server.MapPath("../") & Uty_Config.OutputDir) & lFilename
                Dim lParam As Rpt_Suitou.RptParam
                lParam.JIBUMON_CD = vJiBumonCd
                lParam.INPUT_DATE_FROM = vInputDateFrom
                lParam.INPUT_DATE_TO = vInputDateTo
                lParam.HAKKO_FLG = vHakkoFlg

                Dim lRpt As New Rpt_Suitou(lParam)
                lRpt.Output(lPath)

                Return lPath
            Catch ex As Exception
                Logger.WriteErrLog(ex, "", "", "")
                Throw
            End Try
        End Function

        Public Shared Function ExistDenpyo_NO(ByVal vJiBumon_CD As String, ByVal vInput_YM As String, ByVal vSlip_NO As String) As DS_PAYMENT.CT_PAYMENT1DataTable

            Try

                Dim Ety As New Ety_CT_PAYMENT1
                Dim lDt As New DS_PAYMENT.CT_PAYMENT1DataTable

                lDt = Ety.ExistDenpyo_NO(vJiBumon_CD, vInput_YM, vSlip_NO)

                Return lDt

            Catch ex As Exception
                Logger.WriteErrLog(ex, "", "", "")
                Throw
            End Try
        End Function

        Public Shared Function GetDenpyo_NO(vJiBumon_CD As String, vInput_YM As String) As DS_PAYMENT.CT_PAYMENT1_DENPYODataTable
            Dim lEty As New Ety_CT_PAYMENT1
            Return lEty.GetDenpyo_NO(vJiBumon_CD, vInput_YM)
        End Function

        ''' <summary>
        ''' 当期残高を取得する
        ''' </summary>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vShimebi">締日</param>
        ''' <param name="vStartDate">締め期間開始日</param>
        ''' <param name="vEndDate">締め期間終了日</param>
        ''' <returns></returns>
        Public Shared Function CalcZandaka(ByVal vJibumonCd As String, ByVal vShimebi As String,
                                           ByVal vStartDate As String, ByVal vEndDate As String) As Decimal
            Dim lRst As Decimal = 0
            Try
                '前期繰越残高
                Dim lZenzan As Decimal = CTL_CT_CLOSE.GetZandaka(vJibumonCd, vShimebi)

                '入出金額集計
                'Dim lStartDate As String = CDate(vShimebi).AddDays(1).ToString("yyyy/MM/dd")
                'Dim lEnwdDate As String = CDate(vEndDate).AddDays(1).ToString("yyyy/MM/dd")
                Dim lStartDate As String = vStartDate
                Dim lEndDate As String = vEndDate

                Dim lEty As New Ety_CT_PAYMENT2
                Dim lNyukin As Decimal = lEty.SumKingaku(vJibumonCd, lStartDate, lEndDate, 1)
                Dim lShukin As Decimal = lEty.SumKingaku(vJibumonCd, lStartDate, lEndDate, 2)

                lRst = lZenzan + lNyukin - lShukin
            Catch ex As Exception
                Throw
            End Try

            Return lRst
        End Function

        Public Shared Sub InsertData(vParam1 As DS_PAYMENT.CT_PAYMENT1DataTable, vParam2 As DS_PAYMENT.CT_PAYMENT2DataTable)
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
                                Dim lEty1 As New Ety_CT_PAYMENT1
                                Dim lEty2 As New Ety_CT_PAYMENT2
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

        Public Shared Sub UpdateData(vParam As DS_PAYMENT.SearchParam_ListDataTable, vParam1 As DS_PAYMENT.CT_PAYMENT1DataTable, vParam2 As DS_PAYMENT.CT_PAYMENT2DataTable)
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
                                Dim lEty1 As New Ety_CT_PAYMENT1
                                Dim lEty2 As New Ety_CT_PAYMENT2
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


        Public Shared Sub DeleteData(vParam As DS_PAYMENT.SearchParam_ListDataTable)
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
                                Dim lEty1 As New Ety_CT_PAYMENT1
                                Dim lEty2 As New Ety_CT_PAYMENT2
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

        Public Shared Function MeisaiSelectData(vParam As DS_PAYMENT.SearchParamDataTable, SLIPFLG As String) As DS_PAYMENT.CT_MEISAI_DISPDataTable

            Try
                '更新日時、更新者IDを設定する

                Dim Ety As New Ety_CT_PAYMENT2
                Dim lDt As New DS_PAYMENT.CT_MEISAI_DISPDataTable

                lDt = Ety.MeisaiSelectData(vParam, SLIPFLG)

                Return lDt


            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Function

        Public Shared Function SelectData(vParam As DS_PAYMENT.SearchParamDataTable, SLIPFLG As String) As DS_PAYMENT.CT_SELECTPAYMENTDataTable

            Try
                '更新日時、更新者IDを設定する

                Dim Ety As New Ety_CT_PAYMENT2
                Dim lDt As New DS_PAYMENT.CT_SELECTPAYMENTDataTable

                lDt = Ety.SelectData(vParam, SLIPFLG)

                Return lDt


            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Function

        Public Shared Function SelectData2(vParam As DS_PAYMENT.SearchParamDataTable, SLIPFLG As String) As DS_PAYMENT.CT_HEADER_DISPDataTable
            'ヘッダ表示用
            Try
                '更新日時、更新者IDを設定する

                Dim Ety As New Ety_CT_PAYMENT2
                Dim lDt As New DS_PAYMENT.CT_HEADER_DISPDataTable

                lDt = Ety.SelectData2(vParam, SLIPFLG)

                Return lDt


            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Function

        Public Shared Function GetShiharai(vParam As DS_PAYMENT.SearchParam_ListDataTable, SLIPFLG As String) As DS_PAYMENT.CT_PAYMENT2DataTable
            '支払先
            Try

                Dim Ety As New Ety_CT_PAYMENT2
                Dim lDt As New DS_PAYMENT.CT_PAYMENT2DataTable

                lDt = Ety.GetShiharai(vParam, SLIPFLG)

                Return lDt


            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Function


        Public Shared Function SelectData_List(vParam As DS_PAYMENT.SearchParam_ListDataTable, SLIPFLG As String) As DS_PAYMENT.CT_SELECTPAYMENTDataTable

            Try
                '更新日時、更新者IDを設定する

                Dim Ety As New Ety_CT_PAYMENT2
                Dim lDt As New DS_PAYMENT.CT_SELECTPAYMENTDataTable

                lDt = Ety.SelectData_List(vParam, SLIPFLG)

                Return lDt


            Catch ex As Exception
                Throw New Exception("データ登録処理に失敗しました。", ex)
            End Try
        End Function



    End Class
End Namespace
