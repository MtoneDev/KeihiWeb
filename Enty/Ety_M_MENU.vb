Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text

Imports KeihiWeb.Common.uty

Namespace Enty
    Public Class Ety_M_MENU
        Inherits Ety_Base

        Private _Dt As New DS_M_MENU.M_MEMUDataTable

        ''' <summary>
        ''' IWViewerメニュー情報を取得する
        ''' </summary>
        ''' <param name="vParentNo">親No</param>
        ''' <param name="vPrivilege">権限</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectData(ByVal vParentNo As String, ByVal vPrivilege As String) As DS_M_MENU.M_MENUDtDataTable

            Dim dt As New DS_M_MENU.M_MENUDtDataTable
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT")
            lSql.AppendLine("   MENU_NO")
            lSql.AppendLine(" , PARENT_NO")
            lSql.AppendLine(" , FORM_ID")
            lSql.AppendLine(" , TITLE")
            lSql.AppendLine(" , URL")
            lSql.AppendLine(" , SORT_NO")
            lSql.AppendLine(" , PRIVILEGE")
            lSql.AppendLine(" , (")
            lSql.AppendLine(" 	SELECT COUNT(C.MENU_NO) FROM M_MEMU C (NOLOCK)")
            lSql.AppendLine(" 	WHERE C.PARENT_NO = PARENT.MENU_NO")
            lSql.AppendLine("     AND C.PRIVILEGE <= @privilege ")
            lSql.AppendLine(" ) AS CHILD ")
            lSql.AppendLine(" FROM M_MEMU PARENT (NOLOCK)")
            lSql.AppendLine(" WHERE PARENT_NO = @parent_no ")
            lSql.AppendLine("   AND PRIVILEGE <= @privilege ")
            lSql.AppendLine(" ORDER BY SORT_NO ")

            Using db As New SqlConnection(pConnString)
                Using cmd As New SqlCommand

                    cmd.Parameters.Clear()

                    Dim parent_no As New SqlParameter
                    parent_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(dt.PARENT_NOColumn.DataType)
                    parent_no.Size = dt.PARENT_NOColumn.MaxLength
                    parent_no.ParameterName = "@parent_no"
                    parent_no.Value = vParentNo
                    cmd.Parameters.Add(parent_no)

                    Dim privilege As New SqlParameter
                    privilege.SqlDbType = Uty_Dbinfo.ConvertToDbType(dt.PRIVILEGEColumn.DataType)
                    privilege.Size = dt.PRIVILEGEColumn.MaxLength
                    privilege.ParameterName = "@privilege"
                    privilege.Value = vPrivilege
                    cmd.Parameters.Add(privilege)

                    cmd.CommandText = lSql.ToString
                    cmd.CommandTimeout = Uty_Config.CommandTimeout
                    cmd.Connection = db

                    Using Adapter As New SqlDataAdapter
                        Adapter.SelectCommand = cmd
                        Adapter.Fill(dt)
                    End Using 'Adapter

                End Using 'cmd
            End Using 'db

            Return dt

        End Function

        ''' <summary>
        ''' メニュー番号を基にメニュー情報を取得する
        ''' </summary>
        ''' <param name="vMenuNo">メニュー番号</param>
        ''' <param name="vPrivilege">権限</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMenuData(ByVal vMenuNo As String, ByVal vPrivilege As String) As DS_M_MENU.M_MENUDtDataTable

            Dim dt As New DS_M_MENU.M_MENUDtDataTable
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT")
            lSql.AppendLine("   MENU_NO")
            lSql.AppendLine(" , PARENT_NO")
            lSql.AppendLine(" , FORM_ID")
            lSql.AppendLine(" , XTITLE")
            lSql.AppendLine(" , URL250")
            lSql.AppendLine(" , SORT_NO")
            lSql.AppendLine(" , PRIVILEGE")
            lSql.AppendLine(" , (")
            lSql.AppendLine(" 	SELECT COUNT(C.MENU_NO) FROM M_MENU C (NOLOCK)")
            lSql.AppendLine(" 	WHERE C.PARENT_NO = PARENT.MENU_NO")
            lSql.AppendLine("     AND C.PRIVILEGE <= @privilege ")
            lSql.AppendLine(" ) AS CHILD ")
            lSql.AppendLine(" FROM M_MENU PARENT (NOLOCK)")
            lSql.AppendLine(" WHERE MENU_NO = @menu_no ")
            lSql.AppendLine("   AND PRIVILEGE <= @privilege ")
            lSql.AppendLine(" ORDER BY SORT_NO ")

            Using db As New SqlConnection(pConnString)
                Using cmd As New SqlCommand

                    cmd.Parameters.Clear()

                    Dim menu_no As New SqlParameter
                    menu_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(dt.MENU_NOColumn.DataType)
                    menu_no.Size = dt.MENU_NOColumn.MaxLength
                    menu_no.ParameterName = "@menu_no"
                    menu_no.Value = vMenuNo
                    cmd.Parameters.Add(menu_no)

                    Dim privilege As New SqlParameter
                    privilege.SqlDbType = Uty_Dbinfo.ConvertToDbType(dt.PRIVILEGEColumn.DataType)
                    privilege.Size = dt.PRIVILEGEColumn.MaxLength
                    privilege.ParameterName = "@privilege"
                    privilege.Value = vPrivilege
                    cmd.Parameters.Add(privilege)

                    cmd.CommandText = lSql.ToString
                    cmd.CommandTimeout = Uty_Config.CommandTimeout
                    cmd.Connection = db

                    Using Adapter As New SqlDataAdapter
                        Adapter.SelectCommand = cmd
                        Adapter.Fill(dt)
                    End Using 'Adapter

                End Using 'cmd
            End Using 'db

            Return dt

        End Function

        ''' <summary>
        ''' 画面情報を取得する
        ''' </summary>
        ''' <param name="vFormId">フォームID</param>
        ''' <param name="vPrivilege">権限</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetForm(ByVal vFormId As String, ByVal vPrivilege As String) As DS_M_MENU.M_MEMUDataTable

            Dim dt As New DS_M_MENU.M_MEMUDataTable
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT * ")
            lSql.AppendLine(" FROM M_MENU (NOLOCK)")
            lSql.AppendLine(" WHERE FORM_ID = @form_id ")
            lSql.AppendLine("   AND PRIVILEGE <= @privilege ")

            Using db As New SqlConnection(pConnString)
                Using cmd As New SqlCommand

                    cmd.Parameters.Clear()

                    Dim form_id As New SqlParameter
                    form_id.SqlDbType = Uty_Dbinfo.ConvertToDbType(dt.FORM_IDColumn.DataType)
                    form_id.Size = dt.FORM_IDColumn.MaxLength
                    form_id.ParameterName = "@form_id"
                    form_id.Value = vFormId
                    cmd.Parameters.Add(form_id)

                    Dim privilege As New SqlParameter
                    privilege.SqlDbType = Uty_Dbinfo.ConvertToDbType(dt.PRIVILEGEColumn.DataType)
                    privilege.Size = dt.PRIVILEGEColumn.MaxLength
                    privilege.ParameterName = "@privilege"
                    privilege.Value = vPrivilege
                    cmd.Parameters.Add(privilege)

                    cmd.CommandText = lSql.ToString
                    cmd.CommandTimeout = Uty_Config.CommandTimeout
                    cmd.Connection = db

                    Using Adapter As New SqlDataAdapter
                        Adapter.SelectCommand = cmd
                        Adapter.Fill(dt)
                    End Using 'Adapter

                End Using 'cmd
            End Using 'db

            Return dt

        End Function

        ''' <summary>
        ''' 権限管理されているメニューであればTrue
        ''' </summary>
        ''' <param name="vMenuNo">メニュー番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsDispAuthFlg(ByVal vMenuNo As String) As Boolean

            Dim dt As New DS_M_MENU.M_MEMUDataTable
            Dim lRst As Integer = 0
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT")
            lSql.AppendLine("   DISPAUTH_FLG")
            lSql.AppendLine(" FROM M_MENU PARENT (NOLOCK)")
            lSql.AppendLine(" WHERE MENU_NO = @menu_no ")

            Using db As New SqlConnection(pConnString)
                Using cmd As New SqlCommand

                    cmd.Parameters.Clear()

                    Dim menu_no As New SqlParameter
                    menu_no.SqlDbType = Uty_Dbinfo.ConvertToDbType(dt.MENU_NOColumn.DataType)
                    menu_no.Size = dt.MENU_NOColumn.MaxLength
                    menu_no.ParameterName = "@menu_no"
                    menu_no.Value = vMenuNo
                    cmd.Parameters.Add(menu_no)

                    cmd.CommandText = lSql.ToString
                    cmd.CommandTimeout = Uty_Config.CommandTimeout
                    cmd.Connection = db

                    db.Open()
                    lRst = cmd.ExecuteScalar()

                End Using 'cmd
            End Using 'db

            Return (lRst = 1)

        End Function

        ''' <summary>
        ''' フォームIDからメニュー番号を取得する
        ''' </summary>
        ''' <param name="vFormId">フォームID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMenuNo(ByVal vFormId As String) As String
            Dim lRst As String = String.Empty
            Dim lSql As New StringBuilder
            lSql.AppendLine(" SELECT ")
            lSql.AppendLine("   MENU_NO ")
            lSql.AppendLine(" FROM M_MENU (NOLOCK)")

            Using lDb As New SqlConnection(MyBase.pConnString)
                Using lCmd As New SqlCommand

                    '検索条件設定
                    lCmd.Parameters.Clear()

                    'フォームID
                    lSql.AppendLine(" AND FORM_ID = @form_id ")
                    Dim form_id As New SqlParameter
                    form_id.SqlDbType = Uty_Dbinfo.ConvertToDbType(_Dt.FORM_IDColumn.DataType)
                    form_id.Size = _Dt.FORM_IDColumn.MaxLength
                    form_id.ParameterName = "@form_id"
                    form_id.Value = vFormId
                    lCmd.Parameters.Add(form_id)

                    lCmd.CommandText = lSql.ToString
                    lCmd.Connection = lDb

                    'データを取得する
                    lDb.Open()
                    lRst = lCmd.ExecuteScalar()

                End Using 'vCmd
            End Using 'db

            Return lRst
        End Function

    End Class
End Namespace
