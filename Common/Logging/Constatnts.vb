Public Class Constants

    ''' <summary>
    ''' システム名
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SystemName As String = "経費システム"

    ''' <summary>
    ''' 日時のフォーマット（ミリ秒）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DateTimeMSFormat As String = "yyyy/MM/dd HH:mm:ss.fff"

    ''' <summary>
    ''' 最大日付
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MaxDate As String = "99991231"

    ''' <summary>
    ''' 名称区分
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CMNAMKBN
        Bumon = 1
    End Enum

    ''' <summary>
    ''' IMEのモード設定
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum IMEMode
        IME_ON = 0
        IME_OFF
        IME_OFF_ALPHA
        NUMBER
    End Enum

    ''' <summary>
    ''' メッセージモード
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum MessageMode
        Normal = 0
        Err
        Warn
    End Enum

    ''' <summary>
    ''' 分類マスターの分類コード
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BunruiCd
        Take = 1
        Tomato
        Okayu
        Bin
        Drink
    End Enum

    ''' <summary>
    ''' 処理区分
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum SYORI_KBN
        Nebiki = 15
        Sosai = 14
        Keihi = 0
    End Enum

    ''' <summary>
    ''' MISHUHEDのUPDFLGX区分
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum MISHUHED_UPDFLGX
        Misosin = 0     '未送信
        Shorizumi = 1   '処理済み
        SosinErr = 9    '送信エラー
    End Enum

    ''' <summary>
    ''' 処理区分名称
    ''' </summary>
    ''' <param name="vValue">処理区分</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SyoriKbnString(ByVal vValue As Integer) As String
        Select Case vValue
            Case SYORI_KBN.Nebiki : Return "TBL値引"
            Case SYORI_KBN.Sosai : Return "TBL相殺"
            Case SYORI_KBN.Keihi : Return "TBL経費"
            Case Else : Return ""
        End Select
    End Function

End Class
