
Imports System.Configuration
Imports System.Configuration.ConfigurationManager

Namespace Common.uty

    Public Class Uty_Config
        Private Shared DbName As String = "KeihiSQL"

        ''' <summary>
        ''' データベース接続情報を取得する
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property ConnectionStrings() As ConnectionStringSettings
            Get
                Return (ConfigurationManager.ConnectionStrings(DbName))
            End Get
        End Property

        ''' <summary>
        ''' データベース接続文字列を取得する
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property ConnectionString() As String
            Get
                Return (ConfigurationManager.ConnectionStrings(DbName).ConnectionString)
            End Get
        End Property

        ''' <summary>
        ''' データベース接続文字列を取得する
        ''' </summary>
        ''' <param name="vDbName">データベース名</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property ConnectionString(ByVal vDbName As String) As String
            Get
                Return (ConfigurationManager.ConnectionStrings(vDbName).ConnectionString)
            End Get
        End Property

        ''' <summary>
        ''' データベース接続文字列を取得する
        ''' </summary>
        ''' <param name="vDbName">データベース名</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property ConnectionStrings(ByVal vDbName As String) As ConnectionStringSettings
            Get
                Return (ConfigurationManager.ConnectionStrings(vDbName))
            End Get
        End Property

        ''' <summary>
        ''' バージョン番号
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Version() As String
            Get
                Return (AppSettings("Version"))
            End Get
        End Property

        ''' <summary>
        ''' SQL用エスケープ文字
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property SQLESC() As String
            Get
                Return (AppSettings("SQLESC"))
            End Get
        End Property

        ''' <summary>
        ''' SQLコマンドタイムアウト
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property CommandTimeout() As String
            Get
                Return (AppSettings("CommandTimeout"))
            End Get
        End Property

        ''' <summary>
        ''' ログ出力先フォルダ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property LogFolder() As String
            Get
                Return (AppSettings("LogFolder"))
            End Get
        End Property
        Public Shared ReadOnly Property LogSwitch() As String
            Get
                Return (AppSettings("LogSwitch"))
            End Get
        End Property

        Public Shared ReadOnly Property LogSaveTerm() As String
            Get
                Return (AppSettings("LogSaveTerm"))
            End Get
        End Property

        Public Shared ReadOnly Property Logname() As String
            Get
                Return (AppSettings("LogName"))
            End Get
        End Property

        ''' <summary>
        ''' ファイル出力区切り文字
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Delimiter() As String
            Get
                Return (AppSettings("Delimiter"))
            End Get
        End Property

        ''' <summary>
        ''' データフォルダ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property DataFolder() As String
            Get
                Return (AppSettings("DataFolder"))
            End Get
        End Property

        ''' <summary>
        ''' xlsxダウンロードファイル保存先フォルダ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property XlFolder() As String
            Get
                Return (AppSettings("XlFolder"))
            End Get
        End Property

        ''' <summary>
        ''' ワークフォルダ
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property WorkFolder() As String
            Get
                Return (AppSettings("WorkFolder"))
            End Get
        End Property

        ''' <summary>
        ''' ダウンロードファイル保存先フォルダ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property OutputDir() As String
            Get
                Return (AppSettings("OutputDir"))
            End Get
        End Property

        ''' <summary>
        ''' エンコード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property ENC() As String
            Get
                Dim r As String = AppSettings("ENC")
                Return (IIf(r = "", "Shift_JIS", r))
            End Get
        End Property

        ''' <summary>
        ''' 現金出納帳締日
        ''' </summary>
        ''' <value></value>
        ''' <returns>配列</returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property ShimebiSuitou() As String()
            Get
                Dim r As String = AppSettings("ShimebiSuitou")
                Dim a() As String = r.Split(",")
                Return a
            End Get
        End Property

        ''' <summary>
        ''' 請求締日
        ''' </summary>
        ''' <value></value>
        ''' <returns>配列</returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property ShimebiSeikyu() As String
            Get
                Return AppSettings("ShimebiSeikyu")
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAドメイン名
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property GloviaDmain() As String
            Get
                Return (AppSettings("GloviaDmain"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAユーザーID
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property GloviaUserId() As String
            Get
                Return (AppSettings("GloviaUserId"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAパスワード
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property GloviaPass() As String
            Get
                Return (AppSettings("GloviaPass"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAインターフェースファイルコピー先フォルダ
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property GloviaFolder() As String
            Get
                Return (AppSettings("GloviaFolder"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAインターフェースファイル名（経費）
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property GloviaKeihi() As String
            Get
                Return (AppSettings("GloviaKeihi"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAインターフェースファイル名（請求）
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property GloviaSeikyu() As String
            Get
                Return (AppSettings("GloviaSeikyu"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAインターフェース用会社コード
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property GloviaKaishaCd() As String
            Get
                Return (AppSettings("GloviaKaishaCd"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAインターフェース用会社名
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property GloviaKaishaName() As String
            Get
                Return (AppSettings("GloviaKaishaName"))
            End Get
        End Property

        Public Shared ReadOnly Property FreeeDmain() As String
            Get
                Return (AppSettings("FreeeDmain"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAユーザーID
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property FreeeUserId() As String
            Get
                Return (AppSettings("FreeeUserId"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAパスワード
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property FreeePass() As String
            Get
                Return (AppSettings("FreeePass"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAインターフェースファイルコピー先フォルダ
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property FreeeFolder() As String
            Get
                Return (AppSettings("FreeeFolder"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAインターフェースファイル名（経費）
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property FreeeKeihi() As String
            Get
                Return (AppSettings("FreeeKeihi"))
            End Get
        End Property

        ''' <summary>
        ''' GLOVIAインターフェースファイル名（請求）
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property FreeeSeikyu() As String
            Get
                Return (AppSettings("FreeeSeikyu"))
            End Get
        End Property


        ''' <summary>
        ''' 伝票入力
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property NyuSyukin_MaxGYO() As String
            Get
                Return (AppSettings("NyuSyukin_MaxGYO"))
            End Get
        End Property
        Public Shared ReadOnly Property Seikyu_MaxGYO() As String
            Get
                Return (AppSettings("Seikyu_MaxGYO"))
            End Get
        End Property
        Public Shared ReadOnly Property ZeiMaster_MaxGYO() As String
            Get
                Return (AppSettings("ZeiMaster_MaxGYO"))
            End Get
        End Property

        Public Shared ReadOnly Property SeikyuShimebi() As String
            Get
                Return (AppSettings("ShimebiSeikyu"))
            End Get
        End Property

    End Class

End Namespace
