Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient

Namespace Common.uty

    Public Class Uty_Dbinfo

        Private Shared dbTypeTable As Hashtable

        ''' <summary>
        ''' System.Type を SqlDataType に変換する
        ''' </summary>
        ''' <param name="t">System.Type</param>
        ''' <returns>
        ''' SqlDataType<br />
        ''' 一致しない場合、Variant を返す<br />
        ''' </returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertToDbType(ByVal t As Type) As SqlDbType

            If IsNothing(dbTypeTable) Then
                dbTypeTable = New Hashtable()
                dbTypeTable.Add(GetType(System.Boolean), SqlDbType.Bit)
                dbTypeTable.Add(GetType(System.Int16), SqlDbType.SmallInt)
                dbTypeTable.Add(GetType(System.Int32), SqlDbType.Int)
                dbTypeTable.Add(GetType(System.Int64), SqlDbType.BigInt)
                dbTypeTable.Add(GetType(System.Double), SqlDbType.Float)
                dbTypeTable.Add(GetType(System.Decimal), SqlDbType.Decimal)
                dbTypeTable.Add(GetType(System.String), SqlDbType.VarChar)
                dbTypeTable.Add(GetType(System.DateTime), SqlDbType.DateTime)
                dbTypeTable.Add(GetType(System.Byte()), SqlDbType.VarBinary)
                dbTypeTable.Add(GetType(System.Guid), SqlDbType.UniqueIdentifier)
                dbTypeTable.Add(GetType(System.Byte), SqlDbType.TinyInt)
            End If

            Dim dbtype As SqlDbType
            Try
                dbtype = DirectCast(dbTypeTable(t), SqlDbType)
            Catch ex As Exception
                dbtype = SqlDbType.Variant
            End Try

            Return dbtype

        End Function

        ''' <summary>
        ''' カラムの型が文字型かどうかを検査する
        ''' </summary>
        ''' <param name="t">カラム型</param>
        ''' <returns>True：文字型　False：数値型</returns>
        ''' <remarks></remarks>
        Public Shared Function IsDbTypeChar(ByVal t As Type) As Boolean
            Dim lRst As Boolean = False
            Try
                Select Case t.ToString
                    Case "System.String", "System.DateTime", "System.Byte", "System.Guid"
                        lRst = True
                    Case Else
                        lRst = False
                End Select
            Catch ex As System.Exception
                lRst = False
            End Try

            Return lRst
        End Function

        ''' <summary>
        ''' データテーブルのフィールド一覧を取得する
        ''' </summary>
        ''' <param name="paramValue">データテーブル</param>
        ''' <returns>フィールドのコレクションを返す</returns>
        ''' <remarks></remarks>
        Public Shared Function GetFieldName(ByVal paramValue As DataTable) As ArrayList

            Dim ret As New ArrayList

            For Each col As DataColumn In paramValue.Columns
                ret.Add(col.ColumnName)
            Next

            Return ret

        End Function

        ''' <summary>
        ''' SQLServerのLIKE句ワイルドカード文字列をエスケープする
        ''' </summary>
        ''' <param name="str">文字列</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function EscapeSQLLIKE(ByVal str As String, ByVal escChar As Char) As String
            str = str.Replace("%", escChar & "%")
            str = str.Replace("_", escChar & "_")
            str = str.Replace("[", escChar & "[")
            str = str.Replace("]", escChar & "]")
            str = str.Replace("^", escChar & "^")
            Return str
        End Function

        ''' <summary>
        ''' ストアドプロシージャを実行する
        ''' </summary>
        ''' <param name="param">ストアドに渡すパラメータ
        ''' 第１パラメータ：ストアドプロシージャ名（必須）
        ''' 第２パラメータ以降
        ''' 　　ストアドプロシージャで定義されているパラメータの数だけ指定する
        ''' </param>
        ''' <remarks></remarks>
        Public Shared Sub SpExecuteNonQuery(ByVal param() As String)
            Dim sp_Name As String = "[" & param(0) & "]"

            Using lDb As New SqlConnection(Uty_Config.ConnectionString)
                Using lCmd As New SqlCommand(sp_Name)
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure
                    lCmd.Connection = lDb
                    lCmd.CommandTimeout = Uty_Config.CommandTimeout
                    lCmd.Parameters.Clear()
                    lDb.Open()
                    Try
                        If param.Length > 1 Then
                            SqlCommandBuilder.DeriveParameters(lCmd)
                            For i = 0 To lCmd.Parameters.Count - 1
                                If (lCmd.Parameters(i).Direction =
                                    ParameterDirection.Input) Or
                                    (lCmd.Parameters(i).Direction =
                                    ParameterDirection.InputOutput) Then

                                    lCmd.Parameters(i).Value = param(i)
                                End If
                            Next
                        End If
                    Catch ex As Exception
                        Throw
                    End Try

                    lCmd.ExecuteNonQuery()
                    lDb.Close()
                End Using 'cmd
            End Using 'db
        End Sub

        ''' <summary>
        ''' ストアドプロシージャを実行する
        ''' </summary>
        ''' <param name="param">ストアドに渡すパラメータ
        ''' 第１パラメータ：ストアドプロシージャ名（必須）
        ''' 第２パラメータ以降
        ''' 　　ストアドプロシージャで定義されているパラメータの数だけ指定する
        ''' </param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Public Shared Function SpExecuteFill(ByVal param() As String) As DataTable
            Dim sp_Name As String = "[" & param(0) & "]"
            Dim lDt As New DataTable

            Using lDb As New SqlConnection(Uty_Config.ConnectionString)
                Using lCmd As New SqlCommand(sp_Name)
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure
                    lCmd.Connection = lDb
                    lCmd.CommandTimeout = Uty_Config.CommandTimeout
                    lCmd.Parameters.Clear()
                    lDb.Open()
                    Try
                        If param.Length > 1 Then
                            SqlCommandBuilder.DeriveParameters(lCmd)
                            For i = 0 To lCmd.Parameters.Count - 1
                                If (lCmd.Parameters(i).Direction =
                                    ParameterDirection.Input) Or
                                    (lCmd.Parameters(i).Direction =
                                    ParameterDirection.InputOutput) Then

                                    lCmd.Parameters(i).Value = param(i)
                                End If
                            Next
                        End If
                    Catch ex As Exception
                        Throw
                    End Try

                    'データを取得する
                    Using Adapter As New SqlDataAdapter
                        Adapter.SelectCommand = lCmd
                        Adapter.Fill(lDt)
                    End Using 'Adapter
                End Using 'cmd
            End Using 'db

            Return lDt
        End Function

        ''' <summary>
        ''' ストアドプロシージャを実行する（戻り値あり）
        ''' </summary>
        ''' <param name="param">ストアドに渡すパラメータ
        ''' 第１パラメータ：ストアドプロシージャ名（必須）
        ''' 第２パラメータ以降
        ''' 　　ストアドプロシージャで定義されているパラメータの数だけ指定する
        ''' </param>
        ''' <returns>ReturnValue</returns>
        ''' <remarks></remarks>
        Public Shared Function SpExecuteReturnValue(ByVal param() As String) As Integer
            Dim lRst As Integer = 0
            Dim sp_Name As String = "[" & param(0) & "]"

            Using lDb As New SqlConnection(Uty_Config.ConnectionString)
                Using lCmd As New SqlCommand(sp_Name)
                    lCmd.CommandType = System.Data.CommandType.StoredProcedure
                    lCmd.Connection = lDb
                    lCmd.CommandTimeout = Uty_Config.CommandTimeout
                    lCmd.Parameters.Clear()
                    lCmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
                    lCmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue
                    lDb.Open()
                    Try
                        If param.Length > 1 Then
                            SqlCommandBuilder.DeriveParameters(lCmd)
                            For i = 0 To param.Length - 2
                                If (lCmd.Parameters(i).Direction =
                                    ParameterDirection.Input) Or
                                    (lCmd.Parameters(i).Direction =
                                    ParameterDirection.InputOutput) Then

                                    lCmd.Parameters(i).Value = param(i + 1)
                                End If
                            Next
                        End If
                    Catch ex As Exception
                        Throw
                    End Try

                    lCmd.ExecuteNonQuery()
                    lDb.Close()
                    lRst = lCmd.Parameters("ReturnValue").Value
                    Return lRst
                End Using 'cmd
            End Using 'db
        End Function

        ''' <summary>
        ''' サーバー側の日時を取得する
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDate() As DateTime
            'データを取得する
            Using lDb As New SqlConnection(Uty_Config.ConnectionString)
                Using lCmd As New SqlCommand
                    lCmd.Connection = lDb
                    'lCmd.CommandTimeout = CommandTimeout

                    Dim lSql As String = "SELECT GETDATE()"
                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb
                    lDb.Open()
                    Return lCmd.ExecuteScalar

                End Using 'lCmd
            End Using 'lDb
        End Function

        ''' <summary>
        ''' ISNULL句を生成する
        ''' </summary>
        ''' <param name="aDt">データセット</param>
        ''' <param name="aColumnName">カラム名</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsNullSQL(aDt As DataTable, aColumnName As String) As String
            Dim lStr As String = String.Empty
            If ConvertToDbType(aDt.Columns(aColumnName).DataType) = SqlDbType.VarChar Then
                lStr = "ISNULL(" & aColumnName & ", '') " & aColumnName
            Else
                lStr = "ISNULL(" & aColumnName & ", 0) " & aColumnName
            End If
            Return lStr
        End Function


    End Class
End Namespace
