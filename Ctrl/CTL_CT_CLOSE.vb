Imports System.Data.Common
Imports System.Data.SqlClient

Imports KeihiWeb.Common.Logging
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Enty

Namespace Ctrl
    Public Class CTL_CT_CLOSE

        ''' <summary>
        ''' 最終締日を取得する
        ''' </summary>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vClassNo">業務種別　１：出納帳（省略時）　２：請求書</param>
        ''' <returns></returns>
        Public Shared Function GetLastCloseDate(ByVal vJibumonCd As String, Optional ByVal vClassNo As Integer = 1) As Date
            Dim lRst As Date
            Dim lEty As New Ety_CT_CLOSE
            lRst = lEty.GetLastShimebi(vJibumonCd, vClassNo)
            If lRst < "1900/1/1" Then
                lRst = "1900/1/1"
            End If
            Return lRst
        End Function

        ''' <summary>
        ''' 締めテーブルより残高を取得する
        ''' </summary>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vCloseDate">締日</param>
        ''' <param name="vClassNo">業務種別　１：出納帳（省略時）　２：請求書</param>
        ''' <returns></returns>
        Public Shared Function GetZandaka(ByVal vJibumonCd As String, ByVal vCloseDate As String, Optional ByVal vClassNo As Integer = 1) As Decimal
            Dim lRst As Decimal = 0
            Dim lDt As New DS_CT_CLOSE.CT_CLOSEDataTable
            Dim lEty As New Ety_CT_CLOSE
            lDt = lEty.GetZandakaRec(vJibumonCd, vCloseDate, vClassNo)
            If lDt.Count > 0 Then
                lRst = lDt(0).BALANSE
            Else
                lRst = 0
            End If
            Return lRst
        End Function

        ''' <summary>
        ''' データを生成してインサートする
        ''' </summary>
        ''' <param name="vJibumoncd">自部門コード</param>
        ''' <param name="vShimebi">締日</param>
        ''' <param name="vZandaka">残高</param>
        ''' <param name="vClassNo">業務種別　1：現金出納帳　2：請求書</param>
        ''' <returns></returns>
        Public Shared Function Insert(ByVal vJibumoncd As String, ByVal vShimebi As String, ByVal vZandaka As Decimal, vClassNo As Integer) As Integer
            Dim lCnt As Integer = 0
            Try
                Dim lDt As New DS_CT_CLOSE.CT_CLOSEDataTable
                Dim lRow As DS_CT_CLOSE.CT_CLOSERow = lDt.NewCT_CLOSERow
                lRow.JIBUMON_CD = vJibumoncd
                lRow.CLASS_NO = vClassNo
                lRow.CLOSE_DATE = vShimebi
                lRow.BALANSE = vZandaka
                lRow.CANCEL_FLG = 0
                lRow.ACT_DATE = Now()
                lDt.AddCT_CLOSERow(lRow)

                lCnt = Insert(lDt)

                Return lCnt

            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End Function

        Public Shared Function Insert(ByRef lData As DS_CT_CLOSE.CT_CLOSEDataTable) As Integer
            Try
                Dim lCnt As Integer = 0

                Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                    lDb.Open()

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                Dim lEty As New Ety_CT_CLOSE
                                lCnt = lEty.InsertData(lDb, lCmd, lData)
                                lTx.Commit()

                            Catch ex As Exception
                                lTx.Rollback()
                                Throw
                            End Try
                        End Using 'lCmd
                    End Using 'lTx

                End Using 'lDb

                Return lCnt

            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End Function

        ''' <summary>
        ''' 対象日の締め期間(From-To)を返す
        ''' </summary>
        ''' <param name="vClassNo">業務種別　1：出納帳　2：請求</param>
        ''' <param name="vTokiShimebi">当期締日</param>
        ''' <param name="rStartDate">締め期間開始日</param>
        ''' <param name="rEndDate">締め期間終了日</param>
        Public Shared Sub GetCloseKikan(ByVal vClassNo As Integer, ByVal vTokiShimebi As String, ByRef rStartDate As String, ByRef rEndDate As String)

            Dim lTokiShimebi As Date = CDate(vTokiShimebi)
            Dim lStartDate As Date
            Dim lEndDate As Date

            ' 対象日当期期間取得
            Select Case vClassNo
                Case 1         ' 出納
                    Dim lShimebi() As String = Uty_Config.ShimebiSuitou

                    '2000年以前なら締め処理されていないと判断する
                    If lTokiShimebi <= "2000/1/1" Then
                        lTokiShimebi = Now()
                    End If

                    If lTokiShimebi.Day <= lShimebi(0) Then
                        '15日締め
                        rStartDate = lTokiShimebi.ToString("yyyy/MM/01")
                        rEndDate = lTokiShimebi.ToString("yyyy/MM/" & lShimebi(0))
                    Else
                        '末締め
                        lStartDate = lTokiShimebi.ToString("yyyy/MM/" & lShimebi(0))
                        rStartDate = lStartDate.AddDays(1).ToString("yyyy/MM/dd")

                        lEndDate = lTokiShimebi.AddMonths(1).ToString("yyyy/MM/01")
                        rEndDate = lEndDate.AddDays(-1).ToString("yyyy/MM/dd")
                    End If


                Case 2          '請求
                    Dim lShimebi As String = Uty_Config.ShimebiSeikyu

                    '2000年以前なら締め処理されていないと判断する
                    If lTokiShimebi <= "2000/1/1" Then
                        lTokiShimebi = Now.ToString("yyyy/MM/" & lShimebi.PadLeft(2, "0"))
                    End If

                    If lShimebi = 31 Then
                        rStartDate = lTokiShimebi.ToString("yyyy/MM/01")
                        lEndDate = lTokiShimebi.AddMonths(1).ToString("yyyy/MM/01")
                        rEndDate = lEndDate.AddDays(-1).ToString("yyyy/MM/dd")
                    Else
                        rStartDate = lTokiShimebi.ToString("yyyy/MM/dd")
                        rEndDate = lTokiShimebi.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd")
                    End If
                Case Else
            End Select

        End Sub

        Public Shared Sub GetCloseKikan2(ByVal vClassNo As Integer, ByVal vTokiShimebi As String, ByRef rStartDate As String, ByRef rEndDate As String)

            Dim lTokiShimebi As Date = CDate(vTokiShimebi)
            Dim lStartDate As Date
            Dim lEndDate As Date

            ' 対象日当期期間取得
            Select Case vClassNo
                Case 1         ' 出納
                    Dim lShimebi() As String = Uty_Config.ShimebiSuitou

                    '2000年以前なら締め処理されていないと判断する
                    If lTokiShimebi <= "2000/1/1" Then
                        lTokiShimebi = Now()
                    End If

                    If lTokiShimebi.Day <= lShimebi(0) Then
                        '15日締め
                        rStartDate = lTokiShimebi.ToString("yyyy/MM/01")
                        rEndDate = lTokiShimebi.ToString("yyyy/MM/" & lShimebi(0))
                    Else
                        '末締め
                        lStartDate = lTokiShimebi.ToString("yyyy/MM/" & lShimebi(0))
                        rStartDate = lStartDate.AddDays(1).ToString("yyyy/MM/dd")

                        lEndDate = lTokiShimebi.AddMonths(1).ToString("yyyy/MM/01")
                        rEndDate = lEndDate.AddDays(-1).ToString("yyyy/MM/dd")
                    End If


                Case 2          '請求
                    Dim lShimebi As String = Uty_Config.ShimebiSeikyu

                    '2000年以前なら締め処理されていないと判断する
                    If lTokiShimebi <= "2000/1/1" Then
                        lTokiShimebi = Now.ToString("yyyy/MM/" & lShimebi.PadLeft(2, "0"))
                    End If

                    If lShimebi = 31 Then
                        rStartDate = lTokiShimebi.ToString("yyyy/MM/01")
                        lEndDate = lTokiShimebi.AddMonths(1).ToString("yyyy/MM/01")
                        rEndDate = lEndDate.AddDays(-1).ToString("yyyy/MM/dd")
                    Else
                        rStartDate = lTokiShimebi.AddMonths(-1).AddDays(1).ToString("yyyy/MM/dd")
                        rEndDate = lTokiShimebi.ToString("yyyy/MM/dd")
                    End If
                Case Else
            End Select

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="vUserLevel">ユーザーレベル</param>
        ''' <param name="vJibumonCd">自部門コード</param>
        ''' <param name="vClassNo">業務区分　１：出納　２：請求</param>
        ''' <param name="vCloseDate">締日</param>
        ''' <returns></returns>
        Public Shared Function Torikeshi(ByVal vUserLevel As Integer, ByVal vJibumonCd As String, ByVal vClassNo As Integer, ByVal vCloseDate As String) As Integer
            Try
                Dim lCnt As Integer = 0
                Dim lEty As New Ety_CT_CLOSE

                If vUserLevel < Common.Constants.UserLevel.Keihi Then
                    Dim lBool As Boolean = lEty.IsOutputGlovia(vJibumonCd, vClassNo, vCloseDate)
                    If lBool Then
                        Throw New Exception("GLOVIA出力済みの為、締め処理を取り消すことはできません。")
                    End If
                End If

                Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                    lDb.Open()

                    Using lTx As DbTransaction = lDb.BeginTransaction
                        Using lCmd As New SqlCommand
                            lCmd.Connection = lDb
                            lCmd.CommandTimeout = 60
                            lCmd.Transaction = lTx
                            Try
                                lCnt = lEty.DeleteData(lDb, lCmd, vJibumonCd, vClassNo, vCloseDate)
                                lTx.Commit()

                            Catch ex As Exception
                                lTx.Rollback()
                                Throw
                            End Try
                        End Using 'lCmd
                    End Using 'lTx

                End Using 'lDb

                Return lCnt

            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End Function

    End Class
End Namespace
