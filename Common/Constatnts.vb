Namespace Common

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
        ''' ユーザーレベル
        ''' </summary>
        Public Enum UserLevel
            Normal = 1
            Keihi = 5
            Admin = 9
        End Enum
    End Class

End Namespace
