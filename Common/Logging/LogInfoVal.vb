Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics

Imports KeihiWeb.Common

Namespace Common.Logging

    ''' <summary>
    ''' ログの出力情報を管理する
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class LogInfoVal

        'システムログ
        Private _Timestamp As String = ""
        Private _ComputerName As String = ""
        Private _ProcessID As String = ""
        Private _LogKind As String = ""
        Private _SessionId As String = ""

        'クライアントログ
        Private _SystemID As String = ""
        Private _RequestFilePath As String = ""
        Private _UserHostAddress As String = ""
        Private _UserHostName As String = ""
        Private _LogonUserID As String = ""
        Private _HttpMethod As String = ""
        Private _LoginID As String = ""
        Private _MethodName As String = ""
        Private _MessageCode As String = ""
        Private _MessageString As String = ""
        Private _StackTrace As String = ""
        Private _DenpyoNo As String = ""
        Private _KonyuDate As String = ""
        Private _Code As String = ""


        Public Sub New()

            _Timestamp = DateTime.Now.ToString(Constants.DateTimeMSFormat)
            _ComputerName = Environment.MachineName
            _ProcessID = Process.GetCurrentProcess().Id.ToString()

        End Sub

        ''' <summary>
        ''' ログ出力日時
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Timestamp() As String
            Get
                Return _Timestamp
            End Get
            Set(ByVal value As String)
                _Timestamp = value
            End Set
        End Property

        ''' <summary>
        ''' サーバー名
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ComputerName() As String
            Get
                Return _ComputerName
            End Get
            Set(ByVal value As String)
                _ComputerName = value
            End Set
        End Property

        ''' <summary>
        ''' プロセスID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProcessID() As String
            Get
                Return _ProcessID
            End Get
            Set(ByVal value As String)
                _ProcessID = value
            End Set
        End Property

        ''' <summary>
        ''' メソッド名
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MethodName() As String
            Get
                Return _MethodName
            End Get
            Set(ByVal value As String)
                _MethodName = value
            End Set
        End Property

        ''' <summary>
        ''' ログ種別
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LogKind() As String
            Get
                Return _LogKind
            End Get
            Set(ByVal value As String)
                _LogKind = value
            End Set
        End Property

        ''' <summary>
        ''' セッションID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SessionId() As String
            Get
                Return _SessionId
            End Get
            Set(ByVal value As String)
                _SessionId = value
            End Set
        End Property

        ''' <summary>
        ''' システムID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SystemID() As String
            Get
                Return _SystemID
            End Get
            Set(ByVal value As String)
                _SystemID = value
            End Set
        End Property

        ''' <summary>
        ''' リクエストされたURLパス
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RequestFilePath() As String
            Get
                Return _RequestFilePath
            End Get
            Set(ByVal value As String)
                _RequestFilePath = value
            End Set
        End Property

        ''' <summary>
        ''' ユーザーホストIPアドレス
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserHostAddress() As String
            Get
                Return _UserHostAddress
            End Get
            Set(ByVal value As String)
                _UserHostAddress = value
            End Set
        End Property

        ''' <summary>
        ''' ユーザーホスト名
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserHostName() As String
            Get
                Return _UserHostName
            End Get
            Set(ByVal value As String)
                _UserHostName = value
            End Set
        End Property

        ''' <summary>
        ''' WindowsログオンID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LogonUserID() As String
            Get
                Return _LogonUserID
            End Get
            Set(ByVal value As String)
                _LogonUserID = value
            End Set
        End Property

        ''' <summary>
        ''' HTTPメソッド
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property HttpMethod() As String
            Get
                Return _HttpMethod
            End Get
            Set(ByVal value As String)
                _HttpMethod = value
            End Set
        End Property

        ''' <summary>
        ''' アプリケーションへのログインID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LoginID() As String
            Get
                Return _LoginID
            End Get
            Set(ByVal value As String)
                _LoginID = value
            End Set
        End Property

        ''' <summary>
        ''' メッセージコード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MessageCode() As String
            Get
                Return _MessageCode
            End Get
            Set(ByVal value As String)
                _MessageCode = value
            End Set
        End Property

        ''' <summary>
        ''' ログ出力するメッセージ内容
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MessageString() As String
            Get
                Return _MessageString
            End Get
            Set(ByVal value As String)
                _MessageString = value
            End Set
        End Property

        ''' <summary>
        ''' スタックとレース
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StackTrace() As String
            Get
                Return _StackTrace
            End Get
            Set(ByVal value As String)
                _StackTrace = value
            End Set
        End Property
        Public Property Code() As String
            Get
                Return _Code
            End Get
            Set(ByVal value As String)
                _Code = value
            End Set
        End Property
        Public Property DenpyoNo() As String
            Get
                Return _DenpyoNo
            End Get
            Set(ByVal value As String)
                _DenpyoNo = value
            End Set
        End Property
        Public Property KonyuDate() As String
            Get
                Return _KonyuDate
            End Get
            Set(ByVal value As String)
                _KonyuDate = value
            End Set
        End Property

        ''' <summary>
        ''' ログ情報をCSV形式にして返す
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToCSV()

            Dim s() As String = {_Timestamp, _ComputerName, _ProcessID, _LogKind, SessionId,
                                _SystemID, _HttpMethod, _RequestFilePath,
                                _UserHostAddress, _UserHostName, _LogonUserID, _LoginID,
                                _MethodName, _MessageString, _StackTrace, _Code, _KonyuDate, _DenpyoNo}
            Return String.Join(",", s)

        End Function

    End Class

End Namespace