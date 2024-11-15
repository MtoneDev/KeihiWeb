Imports System.IO
Imports System.Text
Imports System.Reflection.MethodBase

Imports KeihiWeb.Common
Imports KeihiWeb.Common.Logging
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Ctrl
Imports KeihiWeb.Enty

Imports OfficeOpenXml

Namespace Report

    Public Class Rpt_Suitou_KamokuBetsu

#Region "セル座標設定"
        Public Enum InitPoint
            Row = 6
            Col = 1
        End Enum
        ''' <summary>
        ''' 締め前には（仮）を出力する
        ''' </summary>
        Public Enum RptStatus
            Row = 2
            Col = 1
        End Enum

        Public Enum HeadBumonName
            Row = 1
            Col = 5
        End Enum

        Public Enum HeadShimebi
            Row = 4
            Col = 1
        End Enum

        Public Enum MeisaiCol
            KamokuCd = 1
            KamokuName = 2
            UchiwakeCd = 3
            UchiwakeName = 4
            BumonCd = 6
            BumonName = 7
            Kingaku = 10
        End Enum

        Public Enum TotalCol
            Kurikoshi = 1
            Nyukin = 3
            Shukkin = 6
            Zandaka = 9
        End Enum
#End Region

        ''' <summary>
        ''' 科目別集計表出力パラメータ
        ''' </summary>
        Public Structure RptParam
            Dim JIBUMON_CD As String
            Dim INPUT_DATE_FROM As String
            Dim INPUT_DATE_TO As String
            Dim HAKKO_FLG As String
        End Structure
        Private _RptParam As RptParam

        ''' <summary>
        ''' 科目別集計データ
        ''' </summary>
        Private _Dt As New DS_PAYMENT.RPT_KAMOKUBETUDataTable

        ''' <summary>
        ''' 前回繰越金
        ''' </summary>
        Private _Zandaka As Decimal = 0

        ''' <summary>
        ''' 帳票テンプレート
        ''' </summary>
        Private _TemplateFile As FileInfo

        ''' <summary>
        ''' 帳票出力のコンストラクタ
        ''' </summary>
        ''' <param name="vParam"></param>
        Public Sub New(vParam As RptParam)
            _RptParam = vParam
            Dim lEty As New Ety_CT_PAYMENT1
            _Dt = lEty.GetRptData_KamokuBetsu(_RptParam)
            _Zandaka = CTL_CT_CLOSE.GetZandaka(_RptParam.JIBUMON_CD, _RptParam.INPUT_DATE_FROM)
            _TemplateFile = New FileInfo(HttpContext.Current.Server.MapPath("~/report") & "\Rpt_Suitou_KamokuBetsu.xlsx")
        End Sub

        ''' <summary>
        ''' 科目別集計表出力
        ''' </summary>
        ''' <param name="vOutputPath"></param>
        Public Sub Output(ByVal vOutputPath As String)

            Using doc As New ExcelPackage(_TemplateFile)
                Dim workbook = doc.Workbook
                If workbook IsNot Nothing Then
                    If workbook.Worksheets.Count > 0 Then
                        Dim worksheet = workbook.Worksheets(1)
                        With worksheet
                            'ヘッダー情報を出力する
                            If 1 = 1 Then
                                Dim lCloseDate As Date = CTL_CT_CLOSE.GetLastCloseDate(_RptParam.JIBUMON_CD)
                                If _RptParam.HAKKO_FLG <> 1 Then
                                    If lCloseDate < Uty_Common.ChangeStringToDate(_RptParam.INPUT_DATE_FROM) Then
                                        .Cells(RptStatus.Row, RptStatus.Col).Value = "（仮）"
                                    Else
                                        .Cells(RptStatus.Row, RptStatus.Col).Value = "（再）"
                                    End If
                                End If
                            End If
                            .Cells(HeadBumonName.Row, HeadBumonName.Col).Value = CTL_M_BUMON.GetBumonName(_RptParam.JIBUMON_CD) & "(" & _RptParam.JIBUMON_CD & ")"
                            Dim lShime1 As String = Uty_Common.ChangeCharToDateFormat(_RptParam.INPUT_DATE_FROM, "yyyy年MM月dd日")
                            Dim lShime2 As String = Uty_Common.ChangeCharToDateFormat(_RptParam.INPUT_DATE_TO, "yyyy年MM月dd日")
                            .Cells(HeadShimebi.Row, HeadShimebi.Col).Value = lShime1 & "～" & lShime2 & IIf(_RptParam.HAKKO_FLG = 1, "締め分", "")

                            '明細を出力する
                            Dim lNyukinFlg As Boolean = False
                            Dim r As Integer = 0
                            For Each lRow As DS_PAYMENT.RPT_KAMOKUBETURow In _Dt.Rows
                                If lRow.SLIP_FLG = 1 And lNyukinFlg = False Then
                                    lNyukinFlg = True
                                    '入金明細を出力する
                                    r += 3
                                End If
                                .InsertRow(InitPoint.Row + r, 1, InitPoint.Row)
                                .Cells(InitPoint.Row + r, MeisaiCol.KamokuCd).Value = lRow.KAMOKU_CD
                                .Cells(InitPoint.Row + r, MeisaiCol.KamokuName).Value = lRow.KAMOKU_NM
                                .Cells(InitPoint.Row + r, MeisaiCol.UchiwakeCd).Value = lRow.UCHI_CD
                                .Cells(InitPoint.Row + r, MeisaiCol.UchiwakeName).Value = lRow.UCHI_NM
                                .Cells(InitPoint.Row + r, MeisaiCol.BumonCd).Value = lRow.BUMON_CD
                                .Cells(InitPoint.Row + r, MeisaiCol.BumonName).Value = lRow.BUMON_NM
                                .Cells(InitPoint.Row + r, MeisaiCol.Kingaku).Value = lRow.SUM_AMOUNT
                                SetMeisaiStyle(worksheet, InitPoint.Row + r)
                                r += 1
                            Next

                            If lNyukinFlg = False Then
                                r += 3
                            End If

                            '合計欄を出力する
                            r += 2
                            If _Dt.Rows.Count > 0 Then
                                .Cells(InitPoint.Row + r, TotalCol.Kurikoshi).Value = _Zandaka
                                .Cells(InitPoint.Row + r, TotalCol.Nyukin).Value = Uty_Common.IsDBNullToZero(_Dt.Compute("Sum(SUM_AMOUNT)", "SLIP_FLG = 1"))
                                .Cells(InitPoint.Row + r, TotalCol.Shukkin).Value = Uty_Common.IsDBNullToZero(_Dt.Compute("Sum(SUM_AMOUNT)", "SLIP_FLG = 2"))
                                .Cells(InitPoint.Row + r, TotalCol.Zandaka).Value = _Zandaka + .Cells(InitPoint.Row + r, TotalCol.Nyukin).Value - .Cells(InitPoint.Row + r, TotalCol.Shukkin).Value
                            End If

                        End With
                    End If
                End If

                ' 指定したファイルに保存する
                Dim lOutFile As FileInfo = New FileInfo(vOutputPath)
                doc.SaveAs(lOutFile)
            End Using
        End Sub

        Private Sub SetMeisaiStyle(ByRef rWs As ExcelWorksheet, ByVal vRow As Integer)
            'セル結合
            Using lRng1 As ExcelRange = rWs.Cells(vRow, MeisaiCol.UchiwakeName, vRow, MeisaiCol.UchiwakeName + 1)
                lRng1.Merge = True
            End Using
            Using lRng2 As ExcelRange = rWs.Cells(vRow, MeisaiCol.BumonName, vRow, MeisaiCol.BumonName + 2)
                lRng2.Merge = True
            End Using

            Using lRng As ExcelRange = rWs.Cells(vRow, MeisaiCol.KamokuCd, vRow, MeisaiCol.Kingaku)
                '罫線
                lRng.Style.Border.Top.Style = Style.ExcelBorderStyle.Hair
                lRng.Style.Border.Left.Style = Style.ExcelBorderStyle.Hair
                lRng.Style.Border.Bottom.Style = Style.ExcelBorderStyle.Hair
                lRng.Style.Border.Right.Style = Style.ExcelBorderStyle.Hair
                'フォント
                lRng.Style.Font.Name = "メイリオ"
                lRng.Style.Font.Size = 10
            End Using

            '行の高さ
            rWs.Row(vRow).Height = 18

        End Sub
    End Class
End Namespace
