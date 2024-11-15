Imports System.Configuration
Imports System.Configuration.ConfigurationManager

Imports KeihiWeb.Common.uty

Namespace Enty
    Public Class Ety_Base
        ''' <summary>
        ''' データベース接続文字列(レプリカ)
        ''' </summary>
        ''' <remarks></remarks>
        Protected pConnString As String = Uty_Config.ConnectionString("keihiSQL")

        ''' <summary>
        ''' データベース接続情報コレクション(レプリカ)
        ''' </summary>
        ''' <remarks></remarks>
        Protected pConnObj As ConnectionStringSettings = Uty_Config.ConnectionStrings("keihiSQL")

    End Class
End Namespace
