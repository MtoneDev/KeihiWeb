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
    Public Class Rpt_Suitou
#Region "セル座標設定"

        ''' <summary>
        ''' 明細開始位置
        ''' </summary>
        Public Enum InitPoint
            Row = 7
            Col = 1
        End Enum

        ''' <summary>
        ''' レポート名締め前には（仮）を出力する
        ''' </summary>
        Public Enum RptTitle
            Row = 1
            Col = 1
        End Enum

        Public Enum HeadKaishaName
            Row = 1
            Col = 7
        End Enum

        Public Enum HeadBumonName
            Row = 2
            Col = 8
        End Enum

        Public Enum HeadShimebi
            Row = 3
            Col = 1
        End Enum

        Public Enum PrintKamoku
            Row = 5
            Col1 = 7
            Col2 = 8
            Col3 = 9
            Col4 = 10
            Col5 = 11
            Col6 = 12
            Col7 = 13
            sonota = 14
        End Enum

        Public Enum Zandaka
            Row = 6
            Col = 16
        End Enum

        Public Enum MeisaiCol
            Hiduke = 1
            Shiharaisaki
            Naiyou
            UchiCd
            BumonCd
            NyukinGaku
            Shukin1
            Shukin2
            Shukin3
            Shukin4
            Shukin5
            Shukin6
            Shukin7
            KamokuCd
            Sonota
            Zandaka
        End Enum
#End Region

        ''' <summary>
        ''' 現金出納帳出力パラメータ
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
        Private _Dt As New DS_PAYMENT.RPT_SUITOUDataTable

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
            _Dt = lEty.GetRptData_Suitou(_RptParam)
            _Zandaka = CTL_CT_CLOSE.GetZandaka(_RptParam.JIBUMON_CD, _RptParam.INPUT_DATE_FROM)
            _TemplateFile = New FileInfo(HttpContext.Current.Server.MapPath("~/report") & "\Rpt_Suitou.xlsx")
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
                            .Cells(HeadKaishaName.Row, HeadKaishaName.Col).Value = Uty_Config.GloviaKaishaName
                            .Cells(HeadBumonName.Row, HeadBumonName.Col).Value = CTL_M_BUMON.GetBumonName(_RptParam.JIBUMON_CD) & "(" & _RptParam.JIBUMON_CD & ")"
                            Dim lShime1 As String = Uty_Common.ChangeCharToDateFormat(_RptParam.INPUT_DATE_FROM, "yyyy年MM月dd日")
                            Dim lShime2 As String = Uty_Common.ChangeCharToDateFormat(_RptParam.INPUT_DATE_TO, "yyyy年MM月dd日")
                            .Cells(HeadShimebi.Row, HeadShimebi.Col).Value = lShime1 & "～" & lShime2 & IIf(_RptParam.HAKKO_FLG = 1, "締め分", "")

                            '繰越残高
                            .Cells(Zandaka.Row, Zandaka.Col).Value = _Zandaka

                            '明細を出力する
                            Dim lNyukinFlg As Boolean = False
                            Dim r As Integer = 0
                            For Each lRow As DS_PAYMENT.RPT_SUITOURow In _Dt.Rows
                                .InsertRow(InitPoint.Row + r, 1, InitPoint.Row)
                                .Cells(InitPoint.Row + r, MeisaiCol.Hiduke).Value = lRow.INPUT_DATE.Day
                                .Cells(InitPoint.Row + r, MeisaiCol.Shiharaisaki).Value = lRow.PAYEE
                                .Cells(InitPoint.Row + r, MeisaiCol.Naiyou).Value = lRow.NOTES
                                .Cells(InitPoint.Row + r, MeisaiCol.UchiCd).Value = lRow.UCHI_CD
                                .Cells(InitPoint.Row + r, MeisaiCol.BumonCd).Value = lRow.BUMON_CD

                                If lRow.SLIP_FLG = 1 Then
                                    '入金額
                                    .Cells(InitPoint.Row + r, MeisaiCol.NyukinGaku).Value = lRow.AMOUNT
                                    _Zandaka = _Zandaka + lRow.AMOUNT
                                    .Cells(InitPoint.Row + r, MeisaiCol.Zandaka).Value = _Zandaka
                                Else
                                    '出金額
                                    '科目
                                    Select Case lRow.KAMOKU_CD
                                        Case .Cells(PrintKamoku.Row, PrintKamoku.Col1).Value
                                            .Cells(InitPoint.Row + r, MeisaiCol.Shukin1).Value = lRow.AMOUNT

                                        Case .Cells(PrintKamoku.Row, PrintKamoku.Col2).Value
                                            .Cells(InitPoint.Row + r, MeisaiCol.Shukin2).Value = lRow.AMOUNT

                                        Case .Cells(PrintKamoku.Row, PrintKamoku.Col3).Value
                                            .Cells(InitPoint.Row + r, MeisaiCol.Shukin3).Value = lRow.AMOUNT

                                        Case .Cells(PrintKamoku.Row, PrintKamoku.Col4).Value
                                            .Cells(InitPoint.Row + r, MeisaiCol.Shukin4).Value = lRow.AMOUNT

                                        Case .Cells(PrintKamoku.Row, PrintKamoku.Col5).Value
                                            .Cells(InitPoint.Row + r, MeisaiCol.Shukin5).Value = lRow.AMOUNT

                                        Case .Cells(PrintKamoku.Row, PrintKamoku.Col6).Value
                                            .Cells(InitPoint.Row + r, MeisaiCol.Shukin6).Value = lRow.AMOUNT

                                        Case .Cells(PrintKamoku.Row, PrintKamoku.Col7).Value
                                            .Cells(InitPoint.Row + r, MeisaiCol.Shukin7).Value = lRow.AMOUNT

                                        Case Else
                                            'その他
                                            .Cells(InitPoint.Row + r, MeisaiCol.KamokuCd).Value = lRow.KAMOKU_CD
                                            .Cells(InitPoint.Row + r, MeisaiCol.Sonota).Value = lRow.AMOUNT
                                    End Select

                                    _Zandaka = _Zandaka - lRow.AMOUNT
                                    .Cells(InitPoint.Row + r, MeisaiCol.Zandaka).Value = _Zandaka

                                End If
                                SetMeisaiStyle(worksheet, InitPoint.Row + r)
                                r += 1
                            Next

                            '合計欄を出力する
                            'r += 1
                            If _Dt.Rows.Count > 0 Then
                                .Cells(InitPoint.Row + r, MeisaiCol.NyukinGaku).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                .Cells(InitPoint.Row + r, MeisaiCol.Shukin1).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                .Cells(InitPoint.Row + r, MeisaiCol.Shukin2).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                .Cells(InitPoint.Row + r, MeisaiCol.Shukin3).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                .Cells(InitPoint.Row + r, MeisaiCol.Shukin4).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                .Cells(InitPoint.Row + r, MeisaiCol.Shukin5).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                .Cells(InitPoint.Row + r, MeisaiCol.Shukin6).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                .Cells(InitPoint.Row + r, MeisaiCol.Shukin7).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                'その他
                                .Cells(InitPoint.Row + r, MeisaiCol.Sonota).FormulaR1C1 = "=SUM(R[-" & r & "]C:R[-1]C)"
                                '残高
                                .Cells(InitPoint.Row + r, MeisaiCol.Zandaka).FormulaR1C1 = "=R[-" & r + 1 & "]C+RC[-10]-SUM(RC[-9]:RC[-1])"
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

            Using lRng As ExcelRange = rWs.Cells(vRow, MeisaiCol.Hiduke, vRow, MeisaiCol.Zandaka)
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
            Using lRng As ExcelRange = rWs.Cells(InitPoint.Row, MeisaiCol.Hiduke, vMaxRow, MeisaiCol.Zandaka)
                lRng.Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
                lRng.Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
            End Using

            '繰越行
            Using lRng As ExcelRange = rWs.Cells(InitPoint.Row, MeisaiCol.Hiduke, InitPoint.Row, MeisaiCol.Zandaka)
                lRng.Style.Border.Top.Style = Style.ExcelBorderStyle.Medium
            End Using

            '合計行
            Using lRng As ExcelRange = rWs.Cells(vMaxRow, MeisaiCol.Hiduke, vMaxRow, MeisaiCol.Zandaka)
                lRng.Style.Border.Top.Style = Style.ExcelBorderStyle.Medium
            End Using

            ''入金額
            'Using lRng As ExcelRange = rWs.Cells(InitPoint.Row, MeisaiCol.NyukinGaku, vMaxRow, MeisaiCol.NyukinGaku)
            '    lRng.Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
            '    lRng.Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
            'End Using

            ''その他科目縦線
            'Using lRng As ExcelRange = rWs.Cells(InitPoint.Row, MeisaiCol.KamokuCd, vMaxRow, MeisaiCol.Sonota)
            '    lRng.Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
            '    lRng.Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
            'End Using

        End Sub
    End Class
End Namespace
