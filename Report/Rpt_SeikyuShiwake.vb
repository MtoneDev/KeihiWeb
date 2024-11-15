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

    Public Class Rpt_SeikyuShiwake
#Region "セル座標設定"

        ''' <summary>
        ''' 明細開始位置
        ''' </summary>
        Public Enum InitPoint
            Row = 5
            Col = 1
        End Enum

        ''' <summary>
        ''' レポート名締め前には（仮）を出力する
        ''' </summary>
        Public Enum RptTitle
            Row = 1
            Col = 1
        End Enum

        Public Enum HeadBumonName
            Row = 2
            Col = 7
        End Enum

        Public Enum HeadShimebi
            Row = 3
            Col = 1
        End Enum

        Public Enum MeisaiCol
            DenNo = 1
            TradeCd
            TradeNm
            BumonCd
            KamokuCd
            KamokuNm
            UchiCd
            UchiNm
            Notes
            ZeiKbn
            Expense
            Tax
            Amount
            CirNo
        End Enum
#End Region

        ''' <summary>
        ''' 請求入力仕訳出力パラメータ
        ''' </summary>
        Public Structure RptParam
            Dim JIBUMON_CD As String
            Dim INPUT_DATE_FROM As String
            Dim INPUT_DATE_TO As String
            Dim HAKKO_FLG As String
        End Structure
        Private _RptParam As RptParam

        ''' <summary>
        ''' 請求入力仕訳データ
        ''' </summary>
        Private _Dt As New DS_BILL.RPT_SHIWAKEDataTable

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
            Dim lEty As New Ety_CT_BILL1
            _Dt = lEty.GetRptData_Shiwake(_RptParam)
            _TemplateFile = New FileInfo(HttpContext.Current.Server.MapPath("~/report") & "\Rpt_SeikyuShiwake.xlsx")
        End Sub

        ''' <summary>
        ''' 現金出納帳出力
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
                                        .Cells(RptTitle.Row, RptTitle.Col).Value &= "（仮）"
                                    Else
                                        .Cells(RptTitle.Row, RptTitle.Col).Value &= "（再）"
                                    End If
                                End If
                            End If
                            .Cells(HeadBumonName.Row, HeadBumonName.Col).Value = "部門名：" & CTL_M_BUMON.GetBumonName(_RptParam.JIBUMON_CD) & "(" & _RptParam.JIBUMON_CD & ")"
                            Dim lShime1 As String = Uty_Common.ChangeCharToDateFormat(_RptParam.INPUT_DATE_FROM, "yyyy年MM月dd日")
                            Dim lShime2 As String = Uty_Common.ChangeCharToDateFormat(_RptParam.INPUT_DATE_TO, "yyyy年MM月dd日")
                            .Cells(HeadShimebi.Row, HeadShimebi.Col).Value = lShime1 & "～" & lShime2 & IIf(_RptParam.HAKKO_FLG = 1, "締め分", "")

                            '明細を出力する
                            Dim lNyukinFlg As Boolean = False
                            Dim r As Integer = 0
                            For Each lRow As DS_BILL.RPT_SHIWAKERow In _Dt.Rows
                                .InsertRow(InitPoint.Row + r, 1, InitPoint.Row)
                                .Cells(InitPoint.Row + r, MeisaiCol.DenNo).Value = lRow.BILL_NO
                                .Cells(InitPoint.Row + r, MeisaiCol.TradeCd).Value = lRow.TRADE_CD
                                .Cells(InitPoint.Row + r, MeisaiCol.TradeNm).Value = lRow.TRADE_NM
                                .Cells(InitPoint.Row + r, MeisaiCol.BumonCd).Value = lRow.BUMON_CD
                                .Cells(InitPoint.Row + r, MeisaiCol.KamokuCd).Value = lRow.KAMOKU_CD
                                .Cells(InitPoint.Row + r, MeisaiCol.KamokuNm).Value = lRow.KAMOKU_NM
                                .Cells(InitPoint.Row + r, MeisaiCol.UchiCd).Value = lRow.UCHI_CD
                                .Cells(InitPoint.Row + r, MeisaiCol.UchiNm).Value = lRow.UCHI_NM
                                .Cells(InitPoint.Row + r, MeisaiCol.Notes).Value = lRow.NOTES
                                .Cells(InitPoint.Row + r, MeisaiCol.ZeiKbn).Value = lRow.TAX_CD & ":" & lRow.TAX_NM
                                .Cells(InitPoint.Row + r, MeisaiCol.Expense).Value = lRow.EXPENSE
                                .Cells(InitPoint.Row + r, MeisaiCol.Tax).Value = lRow.TAX
                                .Cells(InitPoint.Row + r, MeisaiCol.Amount).Value = lRow.AMOUNT
                                .Cells(InitPoint.Row + r, MeisaiCol.CirNo).Value = lRow.CIR_NO

                                SetMeisaiStyle(worksheet, InitPoint.Row + r)
                                r += 1
                            Next

                            '合計欄を出力する
                            'r += 1
                            If _Dt.Rows.Count > 0 Then
                                .Cells(InitPoint.Row + r, MeisaiCol.Expense).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                .Cells(InitPoint.Row + r, MeisaiCol.Tax).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                .Cells(InitPoint.Row + r, MeisaiCol.Amount).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                            End If

                            SetTotalStyle(worksheet, InitPoint.Row + r)

                        End With
                    End If
                End If

                ' 指定したファイルに保存する
                Dim lOutFile As FileInfo = New FileInfo(vOutputPath)
                doc.SaveAs(lOutFile)
            End Using
        End Sub

        Private Sub SetMeisaiStyle(ByRef rWs As ExcelWorksheet, ByVal vRow As Integer)

            Using lRng As ExcelRange = rWs.Cells(vRow, MeisaiCol.DenNo, vRow, MeisaiCol.CirNo)
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

        Private Sub SetTotalStyle(ByRef rWs As ExcelWorksheet, ByVal vMaxRow As Integer)

            '外枠
            Using lRng As ExcelRange = rWs.Cells(InitPoint.Row, MeisaiCol.DenNo, vMaxRow, MeisaiCol.CirNo)
                lRng.Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
                lRng.Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
            End Using

            '合計行
            Using lRng As ExcelRange = rWs.Cells(vMaxRow, MeisaiCol.DenNo, vMaxRow, MeisaiCol.CirNo)
                lRng.Style.Border.Top.Style = Style.ExcelBorderStyle.Double
            End Using

            '稟議No列の書式設定
            Using lRng As ExcelRange = rWs.Cells(InitPoint.Row, MeisaiCol.CirNo, vMaxRow, MeisaiCol.CirNo)
                lRng.Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left    '左寄せ
                lRng.Style.ShrinkToFit = True                                           '縮小して全体を表示
            End Using

        End Sub
    End Class
End Namespace
