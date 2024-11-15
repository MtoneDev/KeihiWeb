Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty


Namespace Enty
    Public Class Ety_AUTH
        Inherits Ety_Base

        Private _Dt As New M_USER.M_USERDataTable

        ''' <summary>
        ''' 認証情報を取得する
        ''' </summary>
        ''' <param name="vId">ユーザーID</param>
        ''' <param name="vPass">パスワード</param>
        ''' <returns></returns>
        Public Function GetAuthInfo(ByVal vId As String, ByVal vPass As String) As DS_AUTH.AUTH_INFODataTable

            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT ")
            lSql.AppendLine("     USER_ID")
            lSql.AppendLine("   , USER_NAME")
            lSql.AppendLine("   , USER_KANA")
            lSql.AppendLine("   , USER_LEVEL")
            lSql.AppendLine("   , BUMON_CD")
            lSql.AppendLine("   , ISNULL((SELECT BUMON_NM FROM M_BUMON (NOLOCK) WHERE BUMON_CD = M_USER.BUMON_CD), '') BUMON_NAME ")
            lSql.AppendLine(" FROM M_USER (NOLOCK)")
            lSql.AppendLine(" WHERE USER_ID = @user_id ")
            lSql.AppendLine("   AND PASSWORD = @password ")
            lSql.AppendLine("   AND DEL_FLG = 0 ")

            Dim lDt As New DS_AUTH.AUTH_INFODataTable

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand

                    '検索条件設定
                    lCmd.Parameters.Clear()
                    Dim user_id As New SqlParameter
                    user_id.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.USER_IDColumn.DataType)
                    user_id.Size = _Dt.USER_IDColumn.MaxLength
                    user_id.ParameterName = "@user_id"
                    user_id.Value = vId
                    lCmd.Parameters.Add(user_id)

                    Dim password As New SqlParameter
                    password.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.PASSWORDColumn.DataType)
                    password.Size = _Dt.PASSWORDColumn.MaxLength
                    password.ParameterName = "@password"
                    password.Value = vPass
                    lCmd.Parameters.Add(password)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    'データを取得する
                    Dim lRow As DS_AUTH.AUTH_INFORow
                    lDb.Open()
                    Dim reader As DbDataReader = lCmd.ExecuteReader(CommandBehavior.CloseConnection)
                    Do While reader.Read()
                        lRow = lDt.NewAUTH_INFORow
                        lRow.USER_ID = reader("USER_ID")
                        lRow.USER_NAME = reader("USER_NAME")
                        lRow.USER_KANA = reader("USER_KANA")
                        lRow.USER_LEVEL = reader("USER_LEVEL")
                        lRow.BUMON_CD = reader("BUMON_CD")
                        lRow.BUMON_NAME = reader("BUMON_NAME")
                        lDt.AddAUTH_INFORow(lRow)
                    Loop

                End Using 'cmd
            End Using 'db

            Return lDt

        End Function

    End Class
End Namespace
