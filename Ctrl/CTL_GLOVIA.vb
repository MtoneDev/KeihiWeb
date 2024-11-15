Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Security.Principal

Imports KeihiWeb.Common.Logging
Imports KeihiWeb.Common.uty
Imports KeihiWeb.Enty

Namespace Ctrl
    Public Class CTL_GLOVIA

#Region "偽装ファイルコピー"

        Private Shared LOGON32_LOGON_INTERACTIVE As Integer = 2
        Private Shared LOGON32_PROVIDER_DEFAULT As Integer = 0
        Private Shared impersonationContext As WindowsImpersonationContext
        Declare Function LogonUserA Lib "advapi32.dll" (ByVal lpszUsername As String,
                        ByVal lpszDomain As String,
                        ByVal lpszPassword As String,
                        ByVal dwLogonType As Integer,
                        ByVal dwLogonProvider As Integer,
                        ByRef phToken As IntPtr) As Integer
        Declare Auto Function DuplicateToken Lib "advapi32.dll" (
                        ByVal ExistingTokenHandle As IntPtr,
                        ByVal ImpersonationLevel As Integer,
                        ByRef DuplicateTokenHandle As IntPtr) As Integer
        Declare Auto Function RevertToSelf Lib "advapi32.dll" () As Long
        Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Long

        Public Shared Sub GisoCopy(ByVal vPath As String)
            Dim lDomain As String = Uty_Config.GloviaDmain
            Dim lUser As String = Uty_Config.GloviaUserId
            Dim lPass As String = Uty_Config.GloviaPass
            Dim lDir As String = Uty_Config.GloviaFolder

            Try
                If impersonateValidUser(lUser, lDomain, lPass) Then
                    'Insert your code that runs under the security context of a specific user here.
                    System.IO.File.Delete(lDir & Uty_File.GetFilename(vPath))
                    FileIO.FileSystem.CopyFile(vPath, lDir & Uty_File.GetFilename(vPath))
                    undoImpersonation()
                Else
                    'Your impersonation failed. Therefore, include a fail-safe mechanism here.
                End If
            Catch ex As Exception
                Logger.WriteErrLog(ex, "", Now.ToString, "")
            End Try
        End Sub
        'STS20220816
        'Public Shared Sub FisoCopy(ByVal vPath As String)
        '    Dim lDomain As String = Uty_Config.FreeeDmain
        '    Dim lUser As String = Uty_Config.FreeeUserId
        '    Dim lPass As String = Uty_Config.FreeePass
        '    Dim lDir As String = Uty_Config.FreeeFolder

        '    Try
        '        If impersonateValidUser(lUser, lDomain, lPass) Then
        '            'Insert your code that runs under the security context of a specific user here.
        '            System.IO.File.Delete(lDir & Uty_File.GetFilename(vPath))
        '            FileIO.FileSystem.CopyFile(vPath, lDir & Uty_File.GetFilename(vPath))
        '            undoImpersonation()
        '        Else
        '            'Your impersonation failed. Therefore, include a fail-safe mechanism here.
        '        End If
        '    Catch ex As Exception
        '        Logger.WriteErrLog(ex, "", Now.ToString, "")
        '    End Try
        'End Sub

        Private Shared Function impersonateValidUser(ByVal userName As String, ByVal domain As String, ByVal password As String) As Boolean
            Dim tempWindowsIdentity As WindowsIdentity
            Dim token As IntPtr = IntPtr.Zero
            Dim tokenDuplicate As IntPtr = IntPtr.Zero
            impersonateValidUser = False
            If RevertToSelf() Then
                If LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                     LOGON32_PROVIDER_DEFAULT, token) <> 0 Then
                    If DuplicateToken(token, 2, tokenDuplicate) <> 0 Then
                        tempWindowsIdentity = New WindowsIdentity(tokenDuplicate)
                        impersonationContext = tempWindowsIdentity.Impersonate()
                        If Not impersonationContext Is Nothing Then
                            impersonateValidUser = True
                        End If
                    End If
                End If
            End If
            If Not tokenDuplicate.Equals(IntPtr.Zero) Then
                CloseHandle(tokenDuplicate)
            End If
            If Not token.Equals(IntPtr.Zero) Then
                CloseHandle(token)
            End If
        End Function

        Private Shared Sub undoImpersonation()
            impersonationContext.Undo()
        End Sub
#End Region


#Region "出納帳GLOVIA出力"
        ''' <summary>
        ''' GLOVIAコンバートファイルを生成する
        ''' </summary>
        ''' <param name="vBumonCd">部門コード（配列）</param>
        ''' <param name="vCloseDate">締日</param>
        ''' <returns>出力ファイルパス</returns>
        Public Shared Function CreateSuitoFile(ByVal vBumonCd As ArrayList, ByVal vCloseDate As String) As String
            Dim lDt As New DS_GLOVIA.CONV_SUITOUDataTable
            Dim lStartDate As String = ""
            Dim lEndDate As String = ""
            Dim lSumShukin As Decimal = 0
            Dim lSumNyukin As Decimal = 0
            Dim lCnt As Integer = 0         '出力件数
            Dim lSeqNo As Integer = 0       '連番
            Dim lGyoNo As Integer = 0       '入出ごとの行番号
            Dim lNyuryokuNo As Integer = 0  '部門ごとの連番

            Logger.WriteLog("info", "***** GLOVIA出納帳出力開始 *****")

            '締め期間を取得する
            CTL_CT_CLOSE.GetCloseKikan(1, vCloseDate, lStartDate, lEndDate)

            Dim lStr As New StringBuilder

            For Each lBumonCd As String In vBumonCd
                If lBumonCd.Trim = "" Then Continue For

                lNyuryokuNo += 1

                Logger.WriteLog("info", "GLOVIA出納帳 部門：" & lBumonCd & "  出力開始")

                '*** 相手方用金額集計（出納のみ入出金の合計金額を集計）
                'Dim lEtyP2 As New Ety_CT_PAYMENT2
                'lSumNyukin = lEtyP2.SumKingaku(lBumonCd, lStartDate, lEndDate, 1)
                'lSumShukin = lEtyP2.SumKingaku(lBumonCd, lStartDate, lEndDate, 2)

                '*** 対象テーブル順次読み込み
                Dim lEtyGL As New Ety_GLOVIA
                lDt = lEtyGL.GetConvData_Suitou(lBumonCd, lStartDate, lEndDate)
                For Each lRow As DS_GLOVIA.CONV_SUITOURow In lDt.Rows
                    lGyoNo += 1
                    lCnt += 1   '出力件数

                    ' *** データ作成
                    lStr.AppendLine(ConvertSuitou(lNyuryokuNo, lGyoNo, vCloseDate, lRow))
                Next

                Logger.WriteLog("info", "GLOVIA出納帳 部門：" & lBumonCd & "  出力完了  件数＝" & lCnt)

            Next

            'ファイル出力
            Dim lCloseDate As String = CDate(vCloseDate).ToString("yyyyMMdd")
            Dim lDir As String = Uty_File.AddFolderMark(HttpContext.Current.Server.MapPath("../") & Uty_Config.OutputDir) & vBumonCd(0)
            Uty_File.CreateFolder(lDir)
            Dim lPath As String = lDir & "\" & Uty_File.GetFilename(Uty_Config.GloviaKeihi, False) & "_" & vBumonCd(0) & "_" & lCloseDate & "." & Uty_File.GetExtension(Uty_Config.GloviaKeihi)
            Uty_TextFile.TextWriter(lStr, False, lPath)
            GisoCopy(lPath)

            Logger.WriteLog("info", "***** GLOVIA出納帳出力完了 ***** ファイル名：" & lPath)

            Return lPath
        End Function
        Public Shared Function CreateSuitoFile_freee(ByVal vBumonCd As ArrayList, ByVal vCloseDate As String) As String
            Dim lDt As New DS_GLOVIA.CONV_SUITOU_FDataTable
            Dim lStartDate As String = ""
            Dim lEndDate As String = ""
            Dim lSumShukin As Decimal = 0
            Dim lSumNyukin As Decimal = 0
            Dim lCnt As Integer = 0         '出力件数
            Dim lSeqNo As Integer = 0       '連番
            Dim lGyoNo As Integer = 0       '入出ごとの行番号
            Dim lNyuryokuNo As Integer = 0  '部門ごとの連番

            Logger.WriteLog("info", "***** FREEE出納帳出力開始 *****")

            '締め期間を取得する
            CTL_CT_CLOSE.GetCloseKikan(1, vCloseDate, lStartDate, lEndDate)


            Dim lStr As New StringBuilder

            Dim title As String = ""

            title = "日付,伝票番号,決算整理仕訳,借方勘定科目,借方科目コード,借方補助科目,借方取引先,借方取引先コード,借方部門,借方品目,借方メモタグ,"
            title = title + "借方セグメント1,借方セグメント2,借方セグメント3,借方金額,借方税区分,借方税額,貸方勘定科目,貸方科目コード,貸方補助科目,"
            title = title + "貸方取引先,貸方取引先コード,貸方部門,貸方品目,貸方メモタグ,貸方セグメント1,貸方セグメント2,貸方セグメント3,貸方金額,貸方税区分,貸方税額,摘要"


            lStr.AppendLine(title)

            For Each lBumonCd As String In vBumonCd
                If lBumonCd.Trim = "" Then Continue For

                lNyuryokuNo += 1

                Logger.WriteLog("info", "FREEE出納帳 部門：" & lBumonCd & "  出力開始")

                '*** 相手方用金額集計（出納のみ入出金の合計金額を集計）
                'Dim lEtyP2 As New Ety_CT_PAYMENT2
                'lSumNyukin = lEtyP2.SumKingaku(lBumonCd, lStartDate, lEndDate, 1)
                'lSumShukin = lEtyP2.SumKingaku(lBumonCd, lStartDate, lEndDate, 2)

                '*** 対象テーブル順次読み込み
                Dim lEtyGL As New Ety_GLOVIA
                lDt = lEtyGL.GetConvData_Suitou_Freee(lBumonCd, lStartDate, lEndDate)

                For Each lRow As DS_GLOVIA.CONV_SUITOU_FRow In lDt.Rows
                    lGyoNo += 1
                    lCnt += 1   '出力件数

                    ' *** データ作成
                    lStr.AppendLine(ConvertSuitou_FREEE(lNyuryokuNo, lGyoNo, vCloseDate, lRow))
                Next

                Logger.WriteLog("info", "FREEE出納帳 部門：" & lBumonCd & "  出力完了  件数＝" & lCnt)

            Next

            'ファイル出力
            Dim lCloseDate As String = CDate(vCloseDate).ToString("yyyyMMdd")
            Dim lDir As String = Uty_File.AddFolderMark(HttpContext.Current.Server.MapPath("../") & Uty_Config.OutputDir) & vBumonCd(0)
            Uty_File.CreateFolder(lDir)
            Dim lPath As String = lDir & "\" & Uty_File.GetFilename(Uty_Config.FreeeKeihi, False) & "_" & vBumonCd(0) & "_" & lCloseDate & "." & Uty_File.GetExtension(Uty_Config.FreeeKeihi)
            Uty_TextFile.TextWriter(lStr, False, lPath)
            'GisoCopy(lPath)

            Logger.WriteLog("info", "***** FREEE出納帳出力完了 ***** ファイル名：" & lPath)

            Return lPath
        End Function


        ''' <summary>
        ''' GLOVIA Smart用データコンバート文字列作成 /　現金出納
        ''' </summary>
        ''' <param name="vNyuryokuNo">入力番号（伝票単位でユニーク）</param>
        ''' <param name="vGyoNo">明細行番号</param>
        ''' <param name="vShimebi">締日</param>
        ''' <param name="vRow">データRow</param>
        ''' <returns>コンバート文字列</returns>
        Public Shared Function ConvertSuitou(ByVal vNyuryokuNo As String, ByVal vGyoNo As Integer,
                                             ByVal vShimebi As String, ByVal vRow As DS_GLOVIA.CONV_SUITOURow) As String
            Try

                Dim sDat As New StringBuilder      ' 出力文字列

                '******* 伝票情報 *******
                sDat.Append(vNyuryokuNo.PadLeft(8, "0"))                    '入力番号（伝票単位でユニーク）
                sDat.Append("GEN")                                          '入力システム区分（省略時：BAT）
                sDat.Append(Uty_Config.GloviaKaishaCd.PadRight(12, " "))    '会社コード
                sDat.Append("99999999".PadRight(12, " "))                   '起票社員コード
                sDat.Append(vRow.JIBUMON_CD.PadRight(12, " "))              '起票部門コード
                sDat.Append("99999999".PadRight(12, " "))                   '承認社員コード
                '承認日付 15日締めの場合は25日固定
                If CDate(vShimebi).Day = 15 Then
                    sDat.Append(CDate(vShimebi).ToString("yyyyMM25"))
                Else
                    sDat.Append(CDate(vShimebi).ToString("yyyyMMdd"))
                End If
                sDat.Append("0")                                            '承認状態区分
                sDat.Append("00")                                           '仕訳種別区分
                '伝票日付 15日締めの場合は25日固定
                If CDate(vShimebi).Day = 15 Then
                    sDat.Append(CDate(vShimebi).ToString("yyyyMM25"))
                Else
                    sDat.Append(CDate(vShimebi).ToString("yyyyMMdd"))
                End If
                ' 伝票番号
                If CDate(vShimebi).Day = 15 Then
                    sDat.Append(vRow.JIBUMON_CD.PadLeft(5, "0") & vNyuryokuNo.PadLeft(3, "0"))
                Else
                    sDat.Append(vRow.JIBUMON_CD.PadLeft(5, "0") & (CInt(vNyuryokuNo) + 100).ToString.PadLeft(3, "0"))
                End If
                sDat.Append("0")                                            '伝票操作禁止区分
                sDat.Append("0")                                            '仕訳基準種別区分
                sDat.Append("".PadRight(20, "0"))                           'システムリザーブ
                sDat.Append("".PadRight(4, "0"))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           '修正事由コード
                sDat.Append("".PadRight(3, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(2, " "))                            'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(18, " "))                           'システムリザーブ
                sDat.Append("".PadRight(8, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(8, " "))                            '伝票ユーザ開放日付1
                sDat.Append("".PadRight(24, " "))                           '伝票ユーザ開放コード1
                sDat.Append("".PadRight(16, " "))                           '伝票ユーザ開放コード2
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(192, " "))                          '伝票備考
                sDat.Append("".PadRight(192, " "))                          '承認備考
                sDat.Append("".PadRight(192, " "))                          '伝票ユーザ開放域
                sDat.Append("".PadRight(60, " "))                           '伝票ユーザ開放域2

                '******* 明細情報 *******
                sDat.Append(Format(vGyoNo, "".PadRight(9, "0")))            '行番号
                sDat.Append(IIf(vRow.SLIP_FLG = 2, "0", "1"))               '伝票明細貸借区分 0:借　1:貸
                '勘定科目コード
                sDat.Append(vRow.KAMOKU_CD.PadRight(12, " "))
                '会計部門コード
                sDat.Append(vRow.BUMON_CD.PadRight(12, " "))                '
                sDat.Append("".PadRight(4, " "))                            '細目識別区分
                '細目コード
                sDat.Append(vRow.UCHI_CD.PadRight(12, " "))
                sDat.Append("".PadRight(4, " "))                            '内訳識別区分
                sDat.Append("".PadRight(12, " "))                           '内訳コード
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード1識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード1
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード2識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード2
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード3識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード3
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード4識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード4
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード5識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード5
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード1識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード1
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード2識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード2
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード3識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード3
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード4識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード4
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード5識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード5
                sDat.Append("".PadRight(12, " "))                           '取引先コード
                sDat.Append("".PadRight(12, " "))                           'セグメントコード
                sDat.Append("".PadRight(12, " "))                           '負担元コストセンタコード
                sDat.Append("".PadRight(12, " "))                           '請求支払先コード
                sDat.Append("".PadRight(12, " "))                           '事業セグメントコード
                sDat.Append("".PadRight(12, " "))                           '地域セグメントコード
                sDat.Append("".PadRight(12, " "))                           '顧客セグメントコード
                sDat.Append("".PadRight(12, " "))                           'ユーザ開放セグメントコード1
                sDat.Append("".PadRight(12, " "))                           'ユーザ開放セグメントコード2
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            '取引通貨コード
                sDat.Append("".PadRight(1, " "))                            '取引通貨為替レート識別区分
                sDat.Append("".PadRight(13, "0"))                           '取引通貨レート
                sDat.Append("".PadRight(1, " "))                            '表示通貨為替レート識別区分1
                sDat.Append("".PadRight(13, "0"))                           '表示通貨レート1
                sDat.Append("".PadRight(1, " "))                            '表示通貨為替レート識別区分2
                sDat.Append("".PadRight(13, "0"))                           '表示通貨レート2
                sDat.Append("".PadRight(1, " "))                            '表示通貨為替レート識別区分3
                sDat.Append("".PadRight(13, "0"))                           '表示通貨レート3
                sDat.Append("".PadRight(12, " "))                           '資金コード
                sDat.Append("".PadRight(4, " "))                            '消費税区分コード
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ

                sDat.Append("".PadRight(6, " "))                            '消費税率区分
                '機能通貨発生金額
                sDat.Append(IIf(vRow.SUM_EXPENSE < 0, "-", "+"))
                sDat.Append(CInt(vRow.SUM_EXPENSE).ToString.PadLeft(15, "0"))
                sDat.Append(".000")
                sDat.Append("".PadRight(20, "0"))                       '取引通貨発生金額
                sDat.Append("".PadRight(20, "0"))                       '参考消費税金額
                sDat.Append("".PadRight(20, "0"))                       'ユーザ開放数値1
                sDat.Append("3")                                        '課税区分

                sDat.Append("".PadRight(36, " "))                           '履歴物件コード
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, "0"))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(6, " "))                            'システムリザーブ
                sDat.Append("".PadRight(4, "0"))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(20, "0"))                           '数量
                sDat.Append("".PadRight(12, " "))                           '単位コード
                sDat.Append("".PadRight(20, "0"))                           '数量(副)
                sDat.Append("".PadRight(12, " "))                           '単位コード(副)
                sDat.Append("".PadRight(20, "0"))                           '機能通貨単価
                sDat.Append("".PadRight(20, "0"))                           '取引通貨単価
                sDat.Append("".PadRight(20, "0"))                           '拡張数値1
                sDat.Append("".PadRight(20, "0"))                           '拡張数値2
                sDat.Append("".PadRight(20, "0"))                           '拡張数値3
                sDat.Append("".PadRight(8, " "))                            'ユーザ開放日付1
                sDat.Append("".PadRight(12, " "))                           'ユーザ開放コード1
                sDat.Append("".PadRight(12, " "))                           'ユーザ開放コード2
                sDat.Append("".PadRight(4, " "))                            'ユーザ開放コード3
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放コード4
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放コード5
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放コード6
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放コード7
                sDat.Append("".PadRight(72, " "))                           'ユーザ開放域1
                sDat.Append("".PadRight(24, " "))                           'システムリザーブ
                sDat.Append("".PadRight(72, " "))                           'システムリザーブ
                sDat.Append("".PadRight(36, " "))                           'ユーザ開放域2
                sDat.Append("".PadRight(36, " "))                           'ユーザ開放域3
                sDat.Append("".PadRight(8, " "))                            'ユーザ開放コード8
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放域5
                sDat.Append("".PadRight(36, " "))                           'ユーザ開放域6
                sDat.Append("".PadRight(60, " "))                           'ユーザ開放域7
                sDat.Append("".PadRight(72, " "))                           'ユーザ開放域8
                sDat.Append("".PadRight(2, " "))                            'システムリザーブ
                sDat.Append("".PadRight(8, " "))                            'ユーザ開放日付2
                '文字摘要／手形備考
                If vRow.MIN_NOTES.Trim = "" Then
                    sDat.Append("".PadRight(192, " "))
                Else
                    sDat.Append(Uty_Common.StringCut(Uty_Common.StringCut(vRow.MIN_NOTES, 64), 192))
                End If
                sDat.Append("".PadRight(192, " "))                          '明細ユーザ開放域
                sDat.Append("".PadRight(384, " "))                          '明細ユーザ開放域2
                sDat.Append("".PadRight(12, " "))                           '個別消込キー
                sDat.Append("".PadRight(12, " "))                           '回収支払部門コード
                sDat.Append("".PadRight(12, " "))                           '契約番号
                sDat.Append("".PadRight(12, " "))                           'インボイス番号
                sDat.Append("".PadRight(8, " "))                            '回収支払予定日付
                sDat.Append("".PadRight(8, " "))                            '請求支払締め日付
                sDat.Append("".PadRight(1, " "))                            '更新サブシステム区分
                sDat.Append("".PadRight(36, " "))                           '物件管理番号
                sDat.Append("".PadRight(36, " "))                           '手形番号
                sDat.Append("".PadRight(1, " "))                            '手形種類区分
                sDat.Append("".PadRight(1, " "))                            '手形区分
                sDat.Append("".PadRight(2, " "))                            '推移区分
                sDat.Append("".PadRight(8, " "))                            '手形期日/期日現金決済日
                sDat.Append("".PadRight(1, " "))                            'システムリザーブ
                sDat.Append("".PadRight(8, " "))                            '取組日付/支払通知日付
                sDat.Append("".PadRight(8, " "))                            '現金化予定日付
                sDat.Append("".PadRight(4, " "))                            '手形サイト
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(15, " "))                           'システムリザーブ
                sDat.Append("".PadRight(192, " "))                          '振出人名称/自社銀行口座名義人/相手銀行口座名義人
                sDat.Append("".PadRight(12, " "))                           '支払場所銀行コード/相手銀行コード
                sDat.Append("".PadRight(96, " "))                           '支払場所
                sDat.Append("".PadRight(12, " "))                           '手形取組銀行コード/自社銀行コード
                sDat.Append("".PadRight(13, " "))                           '手形割引手数料
                sDat.Append("".PadRight(1, " "))                            '電信文書振込区分
                sDat.Append("".PadRight(1, " "))                            '手数料負担区分
                sDat.Append("".PadRight(1, " "))                            'FB振込処理区分
                sDat.Append("".PadRight(1, " "))                            '自社銀行口座種別
                sDat.Append("".PadRight(12, " "))                           '自社銀行口座番号
                sDat.Append("".PadRight(1, " "))                            '相手銀行口座種別
                sDat.Append("".PadRight(12, " "))                           '相手銀行口座番号
                sDat.Append("".PadRight(22, " "))                           'システムリザーブ
                sDat.Append("".PadRight(11, " "))                           'システムリザーブ

                Return sDat.ToString
            Catch ex As Exception
                Throw
            End Try

        End Function
#End Region
        Public Shared Function ConvertSuitou_FREEE(ByVal vNyuryokuNo As String, ByVal vGyoNo As Integer,
                                             ByVal vShimebi As String, ByVal vRow As DS_GLOVIA.CONV_SUITOU_FRow) As String
            Try

                Dim dt_knk As New DS_M_KAMOKU.M_KAMOKUDataTable
                Dim dt_uti As New DS_M_UTIWAKE.M_UCHIWAKEDataTable
                Dim dt_bmn As New M_BUMON.M_BUMONDataTable


                dt_knk = CTL_M_KAMOKU.SelectData(vRow.KAMOKU_CD)
                dt_uti = CTL_M_UTIWAKE.SelectData(vRow.KAMOKU_CD, vRow.UCHI_CD)
                dt_bmn = CTL_M_BUMON.SelectData(vRow.BUMON_CD)


                Dim sDat As New StringBuilder      ' 出力文字列
                Dim ksflg As String = "2" '借方

                '******* 伝票情報 *******
                '日付 
                '伝票日付 15日締めの場合は25日固定
                If CDate(vShimebi).Day = 15 Then
                    sDat.Append(CDate(vShimebi).ToString("yyyyMM25"))
                Else
                    sDat.Append(CDate(vShimebi).ToString("yyyyMMdd"))
                End If
                sDat.Append(",")
                '伝票番号
                If CDate(vShimebi).Day = 15 Then
                    sDat.Append(vRow.JIBUMON_CD.PadLeft(5, "0") & vNyuryokuNo.PadLeft(3, "0"))
                Else
                    sDat.Append(vRow.JIBUMON_CD.PadLeft(5, "0") & (CInt(vNyuryokuNo) + 100).ToString.PadLeft(3, "0"))
                End If
                sDat.Append(",")

                '決算整理仕訳
                sDat.Append("")
                sDat.Append(",")

                '借方勘定科目
                If vRow.SLIP_FLG = ksflg Then
                    If dt_knk.Count > 0 Then
                        sDat.Append(dt_knk(0).KAMOKU_NM)
                    Else
                        sDat.Append("")
                    End If
                Else
                    sDat.Append("")
                End If
                sDat.Append(",")

                '借方科目コード 
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append(vRow.KAMOKU_CD)
                Else
                    sDat.Append("")
                End If
                sDat.Append(",")
                '借方補助科目
                sDat.Append("")
                sDat.Append(",")

                '借方取引先
                sDat.Append("")
                sDat.Append(",")

                '借方取引先コード
                sDat.Append("")
                sDat.Append(",")

                '借方部門 
                If vRow.SLIP_FLG = ksflg Then
                    If dt_bmn.Count > 0 Then
                        sDat.Append(dt_bmn(0).BUMON_NM)
                    Else
                        sDat.Append("")
                    End If
                Else
                    sDat.Append("")
                End If
                sDat.Append(",")

                '借方品目
                If vRow.SLIP_FLG = ksflg Then
                    If dt_uti.Count > 0 Then
                        sDat.Append(dt_uti(0).UCHI_DISP)
                    Else
                        sDat.Append("")
                    End If
                Else
                    sDat.Append("")
                End If
                sDat.Append(",")

                '借方メモタグ 
                sDat.Append("")
                sDat.Append(",")
                '借方セグメント1 
                sDat.Append("")
                sDat.Append(",")
                '借方セグメント2 
                sDat.Append("")
                sDat.Append(",")
                '借方セグメント3 
                sDat.Append("")
                sDat.Append(",")
                '借方金額 
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append(vRow.SUM_EXPENSE)
                Else
                    sDat.Append("")
                End If
                sDat.Append(",")

                '借方税区分
                If vRow.SLIP_FLG = ksflg Then

                    If dt_knk.Count > 0 Then
                        If dt_knk(0).TAX_CD = "1" Or dt_knk(0).TAX_CD = "2" Then

                            sDat.Append("課対仕入10%")
                            sDat.Append(",")
                        ElseIf dt_knk(0).TAX_CD = "3" Then
                            sDat.Append("非課仕入")
                            sDat.Append(",")
                        Else
                            sDat.Append("対象外")
                            sDat.Append(",")
                        End If

                    Else
                        sDat.Append("")
                        sDat.Append(",")
                    End If

                Else

                    sDat.Append("")
                    sDat.Append(",")

                End If

                '借方税額	
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append(vRow.SUM_TAX)
                Else
                    sDat.Append("")
                End If
                sDat.Append(",")

                '貸方勘定科目	貸方科目コード	貸方補助科目	貸方取引先	貸方取引先コード	貸方部門	貸方品目	貸方メモタグ	貸方セグメント1	貸方セグメント2	貸方セグメント3	貸方金額	貸方税区分	貸方税額	摘要
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append("")
                Else
                    If dt_knk.Count > 0 Then
                        sDat.Append(dt_knk(0).KAMOKU_NM)
                    Else
                        sDat.Append("")
                    End If
                End If
                sDat.Append(",")
                '貸方科目コード
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append("")
                Else
                    sDat.Append(vRow.KAMOKU_CD)
                End If
                sDat.Append(",")

                '貸方補助科目
                sDat.Append("")
                sDat.Append(",")

                '貸方取引先 
                sDat.Append("")
                sDat.Append(",")

                '貸方取引先コード
                sDat.Append("")
                sDat.Append(",")

                '貸方部門
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append("")
                Else
                    If dt_bmn.Count > 0 Then
                        sDat.Append(dt_bmn(0).BUMON_NM)
                    Else
                        sDat.Append("")
                    End If
                End If
                sDat.Append(",")

                '貸方品目
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append("")
                Else
                    If dt_uti.Count > 0 Then
                        sDat.Append(dt_uti(0).UCHI_DISP)
                    Else
                        sDat.Append("")
                    End If
                End If
                sDat.Append(",")

                '貸方メモタグ
                sDat.Append("")
                sDat.Append(",")
                '貸方セグメント1 
                sDat.Append("")
                sDat.Append(",")
                '貸方セグメント2 
                sDat.Append("")
                sDat.Append(",")
                '貸方セグメント3
                sDat.Append("")
                sDat.Append(",")
                '貸方金額 
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append("")
                Else
                    sDat.Append(vRow.SUM_EXPENSE)
                End If
                sDat.Append(",")

                '貸方税区分
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append("")
                    sDat.Append(",")
                Else
                    If dt_knk.Count > 0 Then
                        If dt_knk(0).TAX_CD = "1" Or dt_knk(0).TAX_CD = "2" Then

                            sDat.Append("課対仕入10%")
                            sDat.Append(",")
                        ElseIf dt_knk(0).TAX_CD = "3" Then
                            sDat.Append("非課仕入")
                            sDat.Append(",")
                        Else
                            sDat.Append("対象外")
                            sDat.Append(",")
                        End If

                    Else
                        sDat.Append("")
                        sDat.Append(",")
                    End If

                End If

                '貸方税額
                If vRow.SLIP_FLG = ksflg Then
                    sDat.Append("")
                Else
                    sDat.Append(vRow.SUM_TAX)
                End If
                sDat.Append(",")

                '摘要
                sDat.Append(vRow.MIN_NOTES)

                Return sDat.ToString
            Catch ex As Exception
                Throw
            End Try

        End Function

#Region "請求書GLOVIA出力"
        ''' <summary>
        ''' GLOVIAコンバートファイルを生成する（請求仕訳）
        ''' </summary>
        ''' <param name="vBumonCd">部門コード（配列）</param>
        ''' <param name="vCloseDate">締日</param>
        ''' <returns>出力ファイルパス</returns>
        Public Shared Function CreateSeikyuFile(ByVal vBumonCd As ArrayList, ByVal vCloseDate As String) As String
            Dim lDt As New DS_GLOVIA.CONV_SEIKYUDataTable
            Dim lStartDate As String = ""
            Dim lEndDate As String = ""
            Dim lSumShukin As Decimal = 0
            Dim lSumNyukin As Decimal = 0
            Dim lCnt As Integer = 0         '出力件数
            Dim lSeqNo As Integer = 0       '連番
            Dim lGyoNo As Integer = 0       '明細行番号
            Dim lNyuryokuNo As Integer = 0  '伝票連番

            Logger.WriteLog("info", "***** GLOVIA請求出力開始 *****")

            '締め期間を取得する
            CTL_CT_CLOSE.GetCloseKikan2(2, vCloseDate, lStartDate, lEndDate)

            Dim lStr As New StringBuilder

            For Each lBumonCd As String In vBumonCd
                If lBumonCd.Trim = "" Then Continue For

                Dim sOldNo As String = ""

                Logger.WriteLog("info", "GLOVIA請求 部門：" & lBumonCd & "  出力開始")

                '*** 対象テーブル順次読み込み
                Dim lEtyGL As New Ety_GLOVIA
                lDt = lEtyGL.GetConvData_Seikyu(lBumonCd, lStartDate, lEndDate)
                For Each lRow As DS_GLOVIA.CONV_SEIKYURow In lDt.Rows
                    ' 伝票番号が変わるタイミングでｶｳﾝﾀｰ加算
                    If lRow.MANAGE_NO <> sOldNo Then
                        lNyuryokuNo += 1   '伝票番号加算
                        sOldNo = lRow.MANAGE_NO
                        lGyoNo = 0         ' 明細行番号０クリア
                    End If
                    lGyoNo += 1
                    lCnt += 1   '出力件数

                    ' *** データ作成
                    lStr.AppendLine(ConvertSeikyu(lNyuryokuNo, lGyoNo, vCloseDate, lRow))
                Next

                Logger.WriteLog("info", "GLOVIA請求 部門：" & lBumonCd & "  出力完了  件数＝" & lCnt)

            Next

            'ファイル出力
            Dim lCloseDate As String = CDate(vCloseDate).ToString("yyyyMMdd")
            Dim lDir As String = Uty_File.AddFolderMark(HttpContext.Current.Server.MapPath("../") & Uty_Config.OutputDir) & vBumonCd(0)
            Uty_File.CreateFolder(lDir)
            Dim lPath As String = lDir & "\" & Uty_File.GetFilename(Uty_Config.GloviaSeikyu, False) & "_" & vBumonCd(0) & "_" & lCloseDate & "." & Uty_File.GetExtension(Uty_Config.GloviaSeikyu)
            Uty_TextFile.TextWriter(lStr, False, lPath)
            'FileIO.FileSystem.CopyFile(lPath, "\\192.168.2.20\BATDAT\E0\IN\" & Uty_File.GetFilename(lPath))
            GisoCopy(lPath)

            Logger.WriteLog("info", "***** GLOVIA請求出力完了 ***** ファイル名：" & lPath)

            Return lPath
        End Function

        ''' <summary>
        ''' GLOVIA Smart用データコンバート文字列作成 /　請求書
        ''' </summary>
        ''' <param name="vNyuryokuNo">入力番号（伝票単位でユニーク）</param>
        ''' <param name="vGyoNo">明細行番号</param>
        ''' <param name="vShimebi">締日</param>
        ''' <param name="vRow">データRow</param>
        ''' <returns>コンバート文字列</returns>
        Public Shared Function ConvertSeikyu(ByVal vNyuryokuNo As String, ByVal vGyoNo As Integer,
                                             ByVal vShimebi As String, ByVal vRow As DS_GLOVIA.CONV_SEIKYURow) As String
            Try

                Dim sDat As New StringBuilder      ' 出力文字列

                '******* 伝票情報 *******
                sDat.Append(vNyuryokuNo.PadLeft(8, "0"))                    '入力番号（伝票単位でユニーク）
                sDat.Append("SEI")                                          '入力システム区分（省略時：BAT）
                sDat.Append(Uty_Config.GloviaKaishaCd.PadRight(12, " "))    '会社コード
                sDat.Append("99999999".PadRight(12, " "))                   '起票社員コード
                sDat.Append(vRow.JIBUMON_CD.PadRight(12, " "))              '起票部門コード
                sDat.Append("99999999".PadRight(12, " "))                   '承認社員コード
                '承認日付
                sDat.Append(Uty_Common.GetGetsumatu(CDate(vShimebi).AddMonths(-1).ToString("yyyyMM")).ToString("yyyyMMdd"))
                sDat.Append("0")                                            '承認状態区分
                sDat.Append("00")                                           '仕訳種別区分
                '伝票日付
                sDat.Append(Uty_Common.GetGetsumatu(CDate(vShimebi).AddMonths(-1).ToString("yyyyMM")).ToString("yyyyMMdd"))
                ' 伝票番号
                sDat.Append(vRow.JIBUMON_CD.PadLeft(5, "0") & (CInt(vNyuryokuNo) + 200).ToString.PadLeft(3, "0"))
                sDat.Append("0")                                            '伝票操作禁止区分
                sDat.Append("0")                                            '仕訳基準種別区分
                sDat.Append("".PadRight(20, "0"))                           'システムリザーブ
                sDat.Append("".PadRight(4, "0"))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           '修正事由コード
                sDat.Append("".PadRight(3, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(2, " "))                            'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(18, " "))                           'システムリザーブ
                sDat.Append("".PadRight(8, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(8, " "))                            '伝票ユーザ開放日付1
                sDat.Append("".PadRight(24, " "))                           '伝票ユーザ開放コード1
                sDat.Append("".PadRight(16, " "))                           '伝票ユーザ開放コード2
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(192, " "))                          '伝票備考
                sDat.Append("".PadRight(192, " "))                          '承認備考
                sDat.Append("".PadRight(192, " "))                          '伝票ユーザ開放域
                sDat.Append("".PadRight(60, " "))                           '伝票ユーザ開放域2

                '******* 明細情報 *******
                Select Case vRow.SLIP_KBN
                    Case "0"
                        '******* 借方 *******
                        sDat.Append(Format(vGyoNo, "".PadRight(9, "0")))    '行番号
                        sDat.Append(vRow.SLIP_KBN)                          '伝票明細貸借区分 0:借　1:貸
                        sDat.Append(vRow.KAMOKU_CD.PadRight(12, " "))       '勘定科目コード
                        sDat.Append(vRow.BUMON_CD.PadRight(12, " "))        '会計部門コード
                        sDat.Append("".PadRight(4, " "))                    '細目識別区分
                        sDat.Append(vRow.UCHI_CD.PadRight(12, " "))         '細目コード
                    Case "1"
                        '******* 貸方 *******
                        sDat.Append("1".PadLeft(9, "0"))                    '行番号
                        sDat.Append(vRow.SLIP_KBN)                          '伝票明細貸借区分 0:借　1:貸
                        sDat.Append(vRow.MIBARAI_CD.PadRight(12, " "))      '勘定科目コード
                        sDat.Append(vRow.SAIMU_BMN.PadRight(12, " "))       '会計部門コード
                        sDat.Append("".PadRight(4, " "))                    '細目識別区分
                        'sDat.Append("".PadRight(12, " "))                   '細目コード
                        sDat.Append(vRow.UCHI_CD.PadRight(12, " "))         '細目コード
                End Select
                sDat.Append("".PadRight(4, " "))                            '内訳識別区分
                sDat.Append("".PadRight(12, " "))                           '内訳コード
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード1識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード1
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード2識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード2
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード3識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード3
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード4識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード4
                sDat.Append("".PadRight(4, " "))                            '集計拡張コード5識別区分
                sDat.Append("".PadRight(12, " "))                           '集計拡張コード5
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード1識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード1
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード2識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード2
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード3識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード3
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード4識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード4
                sDat.Append("".PadRight(4, " "))                            '検索拡張コード5識別区分
                sDat.Append("".PadRight(12, " "))                           '検索拡張コード5
                '未払いの仕訳の時のみ設定する
                If vRow.SLIP_KBN = "1" Then
                    If vRow.MIBARAI_CD = "42030" Then
                        sDat.Append("".PadRight(12, " "))                   '取引先コード
                    Else
                        sDat.Append(vRow.TRADE_CD.PadRight(12, " "))        '取引先コード
                    End If
                Else
                    sDat.Append("".PadRight(12, " "))                       '取引先コード
                End If
                sDat.Append("".PadRight(12, " "))                           'セグメントコード
                sDat.Append("".PadRight(12, " "))                           '負担元コストセンタコード
                sDat.Append("".PadRight(12, " "))                           '請求支払先コード
                sDat.Append("".PadRight(12, " "))                           '事業セグメントコード
                sDat.Append("".PadRight(12, " "))                           '地域セグメントコード
                sDat.Append("".PadRight(12, " "))                           '顧客セグメントコード
                sDat.Append("".PadRight(12, " "))                           'ユーザ開放セグメントコード1
                sDat.Append("".PadRight(12, " "))                           'ユーザ開放セグメントコード2
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            '取引通貨コード
                sDat.Append("".PadRight(1, " "))                            '取引通貨為替レート識別区分
                sDat.Append("".PadRight(13, "0"))                           '取引通貨レート
                sDat.Append("".PadRight(1, " "))                            '表示通貨為替レート識別区分1
                sDat.Append("".PadRight(13, "0"))                           '表示通貨レート1
                sDat.Append("".PadRight(1, " "))                            '表示通貨為替レート識別区分2
                sDat.Append("".PadRight(13, "0"))                           '表示通貨レート2
                sDat.Append("".PadRight(1, " "))                            '表示通貨為替レート識別区分3
                sDat.Append("".PadRight(13, "0"))                           '表示通貨レート3
                sDat.Append("".PadRight(12, " "))                           '資金コード
                sDat.Append("".PadRight(4, " "))                            '消費税区分コード
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(6, " "))                            '消費税率区分
                Select Case vRow.KINGAKU_KBN
                    Case 0
                        '******* 借方 *******
                        sDat.Append(IIf(vRow.EXPENSE < 0, "-", "+"))
                        sDat.Append(CInt(vRow.EXPENSE).ToString.PadLeft(15, "0"))   '機能通貨発生金額
                        sDat.Append(".000")
                        sDat.Append("".PadRight(20, "0"))                           '取引通貨発生金額
                        sDat.Append(IIf(vRow.TAX < 0, "-", "+"))
                        sDat.Append(CInt(vRow.TAX).ToString.PadLeft(15, "0"))       '参考消費税金額
                        sDat.Append(".000")
                        sDat.Append("".PadRight(20, "0"))                           'ユーザ開放数値1
                        sDat.Append("0")                                            '課税区分
                    Case 1
                        '******* 消費税仕訳 *******
                        sDat.Append(IIf(vRow.EXPENSE < 0, "-", "+"))
                        sDat.Append(CInt(vRow.EXPENSE).ToString.PadLeft(15, "0"))   '機能通貨発生金額
                        sDat.Append(".000")
                        sDat.Append("".PadRight(20, "0"))                           '取引通貨発生金額
                        sDat.Append("".PadRight(20, "0"))                           '参考消費税金額
                        sDat.Append("".PadRight(20, "0"))                           'ユーザ開放数値1
                        sDat.Append("0")                                            '課税区分
                    Case 2
                        '******* 貸方 *******
                        sDat.Append(IIf(vRow.EXPENSE < 0, "-", "+"))
                        sDat.Append(CInt(vRow.EXPENSE + vRow.TAX).ToString.PadLeft(15, "0"))   '機能通貨発生金額
                        sDat.Append(".000")
                        sDat.Append("".PadRight(20, "0"))                           '取引通貨発生金額
                        sDat.Append("".PadRight(20, "0"))                           '参考消費税金額
                        sDat.Append("".PadRight(20, "0"))                           'ユーザ開放数値1
                        sDat.Append("0")                                            '課税区分
                End Select
                sDat.Append("".PadRight(36, " "))                           '履歴物件コード
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, "0"))                           'システムリザーブ
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(6, " "))                            'システムリザーブ
                sDat.Append("".PadRight(4, "0"))                            'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(12, " "))                           'システムリザーブ
                sDat.Append("".PadRight(20, "0"))                           '数量
                sDat.Append("".PadRight(12, " "))                           '単位コード
                sDat.Append("".PadRight(20, "0"))                           '数量(副)
                sDat.Append("".PadRight(12, " "))                           '単位コード(副)
                sDat.Append("".PadRight(20, "0"))                           '機能通貨単価
                sDat.Append("".PadRight(20, "0"))                           '取引通貨単価
                sDat.Append("".PadRight(20, "0"))                           '拡張数値1
                sDat.Append("".PadRight(20, "0"))                           '拡張数値2
                sDat.Append("".PadRight(20, "0"))                           '拡張数値3
                sDat.Append("".PadRight(8, " "))                            'ユーザ開放日付1
                sDat.Append("".PadRight(12, " "))                           'ユーザ開放コード1
                sDat.Append("".PadRight(12, " "))                           'ユーザ開放コード2
                sDat.Append("".PadRight(4, " "))                            'ユーザ開放コード3
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放コード4
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放コード5
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放コード6
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放コード7
                sDat.Append("".PadRight(72, " "))                           'ユーザ開放域1
                sDat.Append("".PadRight(24, " "))                           'システムリザーブ
                sDat.Append("".PadRight(72, " "))                           'システムリザーブ
                sDat.Append("".PadRight(36, " "))                           'ユーザ開放域2
                sDat.Append("".PadRight(36, " "))                           'ユーザ開放域3
                sDat.Append("".PadRight(8, " "))                            'ユーザ開放コード8
                sDat.Append("".PadRight(24, " "))                           'ユーザ開放域5
                sDat.Append("".PadRight(36, " "))                           'ユーザ開放域6
                sDat.Append("".PadRight(60, " "))                           'ユーザ開放域7
                sDat.Append("".PadRight(72, " "))                           'ユーザ開放域8
                sDat.Append("".PadRight(2, " "))                            'システムリザーブ
                sDat.Append("".PadRight(8, " "))                            'ユーザ開放日付2
                '文字摘要／手形備考
                If vRow.NOTES.Trim = "" Then
                    sDat.Append("".PadRight(192, " "))
                Else
                    sDat.Append(Uty_Common.StringCut(Uty_Common.StringCut(vRow.NOTES, 64), 192))
                End If
                sDat.Append("".PadRight(192, " "))                          '明細ユーザ開放域
                sDat.Append("".PadRight(384, " "))                          '明細ユーザ開放域2
                sDat.Append("".PadRight(12, " "))                           '個別消込キー
                sDat.Append("".PadRight(12, " "))                           '回収支払部門コード
                sDat.Append(vRow.KEIYAKU_NO.PadRight(12, " "))              '契約番号
                sDat.Append("".PadRight(12, " "))                           'インボイス番号
                sDat.Append("".PadRight(8, " "))                            '回収支払予定日付
                sDat.Append("".PadRight(8, " "))                            '請求支払締め日付
                sDat.Append("".PadRight(1, " "))                            '更新サブシステム区分
                sDat.Append("".PadRight(36, " "))                           '物件管理番号
                sDat.Append("".PadRight(36, " "))                           '手形番号
                sDat.Append("".PadRight(1, " "))                            '手形種類区分
                sDat.Append("".PadRight(1, " "))                            '手形区分
                sDat.Append("".PadRight(2, " "))                            '推移区分
                sDat.Append("".PadRight(8, " "))                            '手形期日/期日現金決済日
                sDat.Append("".PadRight(1, " "))                            'システムリザーブ
                sDat.Append("".PadRight(8, " "))                            '取組日付/支払通知日付
                sDat.Append("".PadRight(8, " "))                            '現金化予定日付
                sDat.Append("".PadRight(4, " "))                            '手形サイト
                sDat.Append("".PadRight(4, " "))                            'システムリザーブ
                sDat.Append("".PadRight(15, " "))                           'システムリザーブ
                sDat.Append("".PadRight(192, " "))                          '振出人名称/自社銀行口座名義人/相手銀行口座名義人
                sDat.Append("".PadRight(12, " "))                           '支払場所銀行コード/相手銀行コード
                sDat.Append("".PadRight(96, " "))                           '支払場所
                sDat.Append("".PadRight(12, " "))                           '手形取組銀行コード/自社銀行コード
                sDat.Append("".PadRight(13, " "))                           '手形割引手数料
                sDat.Append("".PadRight(1, " "))                            '電信文書振込区分
                sDat.Append("".PadRight(1, " "))                            '手数料負担区分
                sDat.Append("".PadRight(1, " "))                            'FB振込処理区分
                sDat.Append("".PadRight(1, " "))                            '自社銀行口座種別
                sDat.Append("".PadRight(12, " "))                           '自社銀行口座番号
                sDat.Append("".PadRight(1, " "))                            '相手銀行口座種別
                sDat.Append("".PadRight(12, " "))                           '相手銀行口座番号
                sDat.Append("".PadRight(22, " "))                           'システムリザーブ
                sDat.Append("".PadRight(11, " "))                           'システムリザーブ

                Return sDat.ToString
            Catch ex As Exception
                Throw
            End Try

        End Function
#End Region


#Region "請求書GLOVIA出力"
        ''' <summary>
        ''' GLOVIAコンバートファイルを生成する（請求仕訳）
        ''' </summary>
        ''' <param name="vBumonCd">部門コード（配列）</param>
        ''' <param name="vCloseDate">締日</param>
        ''' <returns>出力ファイルパス</returns>
        Public Shared Function CreateSeikyuFile_Freee(ByVal vBumonCd As ArrayList, ByVal vCloseDate As String) As String
            Dim lDt As New DS_GLOVIA.CONV_SEIKYUDataTable
            Dim lStartDate As String = ""
            Dim lEndDate As String = ""
            Dim lSumShukin As Decimal = 0
            Dim lSumNyukin As Decimal = 0
            Dim lCnt As Integer = 0         '出力件数
            Dim lSeqNo As Integer = 0       '連番
            Dim lGyoNo As Integer = 0       '明細行番号
            Dim lNyuryokuNo As Integer = 0  '伝票連番

            Logger.WriteLog("info", "***** FREEE請求出力開始 *****")

            '締め期間を取得する
            CTL_CT_CLOSE.GetCloseKikan2(2, vCloseDate, lStartDate, lEndDate)

            Dim lStr As New StringBuilder

            Dim title As String = ""
            title = "収支区分,管理番号,発生日,決済期日,取引先コード,取引先,勘定科目,税区分,金額,税計算区分,税額,備考,品目,部門,メモタグ（複数指定可、カンマ区切り）,決済日,決済口座,決済金額"
            lStr.AppendLine(title)


            For Each lBumonCd As String In vBumonCd
                If lBumonCd.Trim = "" Then Continue For

                Dim sOldNo As String = ""

                Logger.WriteLog("info", "FREEE請求 部門：" & lBumonCd & "  出力開始")

                '*** 対象テーブル順次読み込み
                Dim lEtyGL As New Ety_GLOVIA
                lDt = lEtyGL.GetConvData_Seikyu(lBumonCd, lStartDate, lEndDate)

                lNyuryokuNo = CInt(lBumonCd & "000")

                For Each lRow As DS_GLOVIA.CONV_SEIKYURow In lDt.Rows
                    'STS20220511未払い費用スキップ
                    If lRow.KAMOKU_CD <> "" Then


                        ' 伝票番号が変わるタイミングでｶｳﾝﾀｰ加算
                        If lRow.MANAGE_NO <> sOldNo Then
                            lNyuryokuNo += 1   '伝票番号加算
                            'sOldNo = lRow.MANAGE_NO
                            lGyoNo = 0         ' 明細行番号０クリア
                        End If
                        lGyoNo += 1
                        lCnt += 1   '出力件数

                        '仮払消費税は除く
                        If lRow.KAMOKU_CD = "10350" And lRow.NOTES.Trim = "" And lRow.BUMON_CD = "B0000" Then
                        Else

                            ' *** データ作成
                            lStr.AppendLine(ConvertSeikyu_FREEE(lNyuryokuNo, lGyoNo, vCloseDate, lRow))
                        End If

                        sOldNo = lRow.MANAGE_NO

                    End If

                Next

                Logger.WriteLog("info", "FREEE請求 部門：" & lBumonCd & "  出力完了  件数＝" & lCnt)

            Next

            'ファイル出力
            Dim lCloseDate As String = CDate(vCloseDate).ToString("yyyyMMdd")
            Dim lDir As String = Uty_File.AddFolderMark(HttpContext.Current.Server.MapPath("../") & Uty_Config.OutputDir) & vBumonCd(0)
            Uty_File.CreateFolder(lDir)
            Dim lPath As String = lDir & "\" & Uty_File.GetFilename(Uty_Config.FreeeSeikyu, False) & "_" & vBumonCd(0) & "_" & lCloseDate & "." & Uty_File.GetExtension(Uty_Config.FreeeSeikyu)
            Uty_TextFile.TextWriter(lStr, False, lPath)
            'FileIO.FileSystem.CopyFile(lPath, "\\192.168.2.20\BATDAT\E0\IN\" & Uty_File.GetFilename(lPath))
            'GisoCopy(lPath)

            Logger.WriteLog("info", "***** FREEE請求出力完了 ***** ファイル名：" & lPath)

            Return lPath
        End Function

        ''' <summary>
        ''' GLOVIA Smart用データコンバート文字列作成 /　請求書
        ''' </summary>
        ''' <param name="vNyuryokuNo">入力番号（伝票単位でユニーク）</param>
        ''' <param name="vGyoNo">明細行番号</param>
        ''' <param name="vShimebi">締日</param>
        ''' <param name="vRow">データRow</param>
        ''' <returns>コンバート文字列</returns>
        Public Shared Function ConvertSeikyu_FREEE(ByVal vNyuryokuNo As String, ByVal vGyoNo As Integer,
                                             ByVal vShimebi As String, ByVal vRow As DS_GLOVIA.CONV_SEIKYURow) As String
            Try

                Dim dt_knk As New DS_M_KAMOKU.M_KAMOKUDataTable
                Dim dt_knk_1 As New DS_M_KAMOKU.M_KAMOKUDataTable
                Dim dt_uti As New DS_M_UTIWAKE.M_UCHIWAKEDataTable
                Dim dt_uti_1 As New DS_M_UTIWAKE.M_UCHIWAKEDataTable
                Dim dt_bmn As New M_BUMON.M_BUMONDataTable
                Dim dt_bmn_1 As New M_BUMON.M_BUMONDataTable
                Dim dt_tri As New DS_M_TRADE.M_TRADEDataTable


                dt_knk = CTL_M_KAMOKU.SelectData(vRow.KAMOKU_CD)
                dt_knk_1 = CTL_M_KAMOKU.SelectData("42030")
                dt_uti = CTL_M_UTIWAKE.SelectData(vRow.KAMOKU_CD, vRow.UCHI_CD)
                dt_uti_1 = CTL_M_UTIWAKE.SelectData("42030", "KK06")
                dt_bmn = CTL_M_BUMON.SelectData(vRow.BUMON_CD)
                dt_bmn_1 = CTL_M_BUMON.SelectData(vRow.SAIMU_BMN)

                dt_tri = CTL_M_TRADE.SelectData(vRow.TRADE_CD)

                Dim sDat As New StringBuilder      ' 出力文字列
                Dim ksflg As String = "1" '借方


                '******* 伝票情報 *******
                '収支区分
                If vGyoNo = 1 Then
                    sDat.Append("支出")
                Else
                    sDat.Append("")
                End If
                sDat.Append(",")


                '管理番号
                sDat.Append(vNyuryokuNo)
                sDat.Append(",")
                '発生日
                sDat.Append(Uty_Common.GetGetsumatu(CDate(vShimebi).AddMonths(-1).ToString("yyyyMM")).ToString("yyyyMMdd"))
                sDat.Append(",")

                '決済期日
                sDat.Append("")
                sDat.Append(",")
                '取引先コード
                '未払いの仕訳の時のみ設定する
                'If vRow.SLIP_KBN = "1" Then
                '    If vRow.MIBARAI_CD = "42030" Then
                '        sDat.Append("")                   '取引先コード
                '        sDat.Append(",")
                '        sDat.Append("")                   '取引先名
                '    Else
                '        sDat.Append(vRow.TRADE_CD)        '取引先コード
                '        sDat.Append(",")
                '        If dt_tri.Count > 0 Then
                '            sDat.Append(dt_tri(0).TRADE_NM)        '取引先名
                '        Else
                '            sDat.Append("")        '取引先名
                '        End If
                '    End If
                'Else
                '    sDat.Append("")                       '取引先コード
                '    sDat.Append(",")
                '    sDat.Append("")                       '取引先コード
                'End If
                'STS20220511
                sDat.Append(vRow.TRADE_CD)        '取引先コード
                sDat.Append(",")
                If dt_tri.Count > 0 Then
                    sDat.Append(dt_tri(0).TRADE_NM)        '取引先名
                Else
                    sDat.Append("")        '取引先名
                End If


                sDat.Append(",")

                '勘定科目
                Select Case vRow.SLIP_KBN
                    Case "0"
                        '******* 借方 *******
                        If dt_knk.Count > 0 Then
                            sDat.Append(dt_knk(0).KAMOKU_NM)
                        Else
                            sDat.Append("")
                        End If

                    Case "1"
                        '******* 貸方 *******
                        '******* 未払費用 *******
                        If dt_knk_1.Count > 0 Then
                            sDat.Append(dt_knk_1(0).KAMOKU_NM)
                        Else
                            sDat.Append("")
                        End If
                End Select
                sDat.Append(",")

                '税区分
                If dt_knk.Count > 0 Then
                    If dt_knk(0).TAX_CD = "1" Or dt_knk(0).TAX_CD = "2" Then

                        sDat.Append("課対仕入10%")
                        sDat.Append(",")
                    ElseIf dt_knk(0).TAX_CD = "3" Then
                        sDat.Append("非課仕入")
                        sDat.Append(",")
                    Else
                        sDat.Append("対象外")
                        sDat.Append(",")
                    End If

                Else
                    sDat.Append("")
                    sDat.Append(",")
                End If
                '金額
                Select Case vRow.KINGAKU_KBN
                    Case 0
                        '******* 借方 *******
                        'STS20220513 
                        'sDat.Append(vRow.EXPENSE)   '機能通貨発生金額
                        sDat.Append(vRow.EXPENSE + vRow.TAX)   '機能通貨発生金額

                    Case 1
                        '******* 消費税仕訳 *******
                        sDat.Append(vRow.EXPENSE)   '機能通貨発生金額
                    Case 2
                        '******* 貸方 *******
                        sDat.Append(vRow.EXPENSE + vRow.TAX)   '機能通貨発生金額
                End Select
                sDat.Append(",")

                '税計算区分
                If dt_knk.Count > 0 Then
                    Select Case dt_knk(0).TAX_CD
                        Case "1"
                            sDat.Append("内税")
                            sDat.Append(",")
                        Case "2"
                            sDat.Append("外税")
                            sDat.Append(",")
                            'Case "3"
                            ' sDat.Append("非課税")
                            ' sDat.Append(",")
                        Case Else
                            'sDat.Append("その他")
                            sDat.Append("")
                            sDat.Append(",")
                    End Select
                Else
                    sDat.Append("")
                    sDat.Append(",")
                End If
                '税額

                Select Case vRow.KINGAKU_KBN
                    Case 0
                        '******* 借方 *******
                        sDat.Append(vRow.TAX)       '参考消費税金額
                    Case 1
                        '******* 消費税仕訳 *******
                        sDat.Append("")       '参考消費税金額
                    Case 2
                        '******* 貸方 *******
                        sDat.Append("")       '参考消費税金額
                End Select
                sDat.Append(",")

                '備考
                sDat.Append(vRow.NOTES)
                sDat.Append(",")
                '品目
                Select Case vRow.SLIP_KBN
                    Case "0"
                        '******* 借方 *******
                        If dt_uti.Count > 0 Then
                            sDat.Append(dt_uti(0).UCHI_DISP.TrimEnd)
                        Else
                            sDat.Append("")
                        End If
                        sDat.Append(",")
                        '部門
                        If dt_bmn.Count > 0 Then
                            sDat.Append(dt_bmn(0).BUMON_NM)
                            sDat.Append(",")
                        Else
                            sDat.Append("")
                            sDat.Append(",")
                        End If


                    Case "1"
                        '******* 貸方 *******
                        '******* 未払費用 *******
                        If dt_uti_1.Count > 0 Then
                            sDat.Append(dt_uti_1(0).UCHI_DISP.TrimEnd)
                        Else
                            sDat.Append("")
                        End If
                        sDat.Append(",")
                        '部門
                        If dt_bmn_1.Count > 0 Then
                            sDat.Append(dt_bmn_1(0).BUMON_NM)
                        Else
                            sDat.Append("")
                        End If

                End Select
                sDat.Append(",")

                'メモタグ（複数指定可、カンマ区切り）
                sDat.Append("")
                sDat.Append(",")
                '決済日
                sDat.Append("")
                sDat.Append(",")
                '決済口座
                sDat.Append("")
                sDat.Append(",")
                '決済金額
                sDat.Append("")
                sDat.Append(",")

                Return sDat.ToString
            Catch ex As Exception
                Throw
            End Try

        End Function
#End Region


        ''' <summary>
        ''' GLOVIA出力済みフラグを更新する（CANCEL_FLG=2）
        ''' </summary>
        ''' <param name="vBumonCd">自部門コード（配列）</param>
        ''' <param name="vCloseDate">締日</param>
        ''' <param name="vClassNo">業務種別　1:出納　2:請求</param>
        ''' <returns></returns>
        Public Shared Function UpdateOutputGloviaFlg(ByVal vBumonCd As ArrayList, ByVal vCloseDate As String, ByVal vClassNo As Integer) As Integer
            Dim lCnt As Integer = 0
            Dim lUpdate As Date = Now()

            Dim lDt As New DS_CT_CLOSE.CT_CLOSEDataTable
            For Each lJibumon As String In vBumonCd
                Dim lRow As DS_CT_CLOSE.CT_CLOSERow = lDt.NewCT_CLOSERow
                lRow.JIBUMON_CD = lJibumon
                lRow.CLASS_NO = vClassNo
                lRow.CLOSE_DATE = vCloseDate
                lRow.CANCEL_FLG = 2
                lRow.ACT_DATE = lUpdate
                lDt.AddCT_CLOSERow(lRow)
            Next

            Using lDb As New SqlConnection(Uty_Config.ConnectionString("keihiSQL"))
                lDb.Open()

                Using lTx As DbTransaction = lDb.BeginTransaction
                    Using lCmd As New SqlCommand
                        lCmd.Connection = lDb
                        lCmd.CommandTimeout = 60
                        lCmd.Transaction = lTx
                        Try
                            Dim lEty As New Ety_CT_CLOSE
                            lCnt = lEty.UpdateOutputGlovia(lDb, lCmd, lDt)
                            lTx.Commit()

                        Catch ex As Exception
                            lTx.Rollback()
                            Throw
                        End Try
                    End Using 'lCmd
                End Using 'lTx

            End Using 'lDb

            Return lCnt
        End Function

    End Class
End Namespace
