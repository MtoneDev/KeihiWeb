Imports System.Data.Common
Imports System.Data.SqlClient

Imports KeihiWeb.Enty
Imports KeihiWeb.Common.uty

Namespace Ctrl

    Public Class CTL_M_MENU
        ''' <summary>
        ''' メニュー情報を取得する
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetMenu(Optional ByVal vParentNo As String = "0", Optional ByVal vPrivilege As String = "0") As String
            Dim lHtml As New StringBuilder
            Dim lEty As New Ety_M_MENU

            Dim dt As DS_M_MENU.M_MENUDtDataTable
            Try
                dt = lEty.SelectData(vParentNo, vPrivilege)
                For Each m As DS_M_MENU.M_MENUDtRow In dt.Rows
                    If m.CHILD = 0 Then
                        If m.PARENT_NO = 0 Then
                            Continue For
                        Else
                            Dim lUrl As String = HttpContext.Current.Request.Url.AbsoluteUri.Replace("/WebForm", "")
                            If lUrl.IndexOf("?") > 0 Then
                                lUrl = lUrl.Substring(0, lUrl.LastIndexOf("?"))
                            End If
                            Dim lCurrent As String = lUrl.Substring(0, lUrl.LastIndexOf("/"))
                            'lHtml.AppendLine("    <li><a href='" & m.URL & "?MENU_NO=" & m.MENU_NO & "' target='" & m.FORM_ID & "'>" & m.TITLE & "</a></li>")
                            lHtml.AppendLine("    <li><a href='" & lCurrent & m.URL & "?MENU_NO=" & m.MENU_NO & "'>" & m.TITLE & "</a></li>")
                        End If
                    Else
                        lHtml.AppendLine("<li><a href='" & m.URL & "?MENU_NO=" & m.MENU_NO & "'>" & m.TITLE & "</a>")
                        lHtml.AppendLine("  <ul>")
                        lHtml.Append(GetMenu(m.MENU_NO, vPrivilege))
                        lHtml.AppendLine("  </ul>")
                        lHtml.AppendLine("</li>")
                    End If
                Next
            Catch ex As Exception
                Throw New Exception("メニューデータの取得に失敗しました。（" & ex.Message & "）", ex)
            End Try
            Return lHtml.ToString
        End Function

        Public Shared Function GetMenu2(Optional ByVal vParentNo As String = "0", Optional ByVal vPrivilege As String = "0") As String
            Dim lHtml As New StringBuilder
            Dim lEty As New Ety_M_MENU

            Dim dt As DS_M_MENU.M_MENUDtDataTable
            Try
                dt = lEty.SelectData(vParentNo, vPrivilege)
                For Each m As DS_M_MENU.M_MENUDtRow In dt.Rows
                    If m.CHILD = 0 Then
                        If m.PARENT_NO = 0 Then
                            Continue For
                        Else
                            Dim lUrl As String = HttpContext.Current.Request.Url.AbsoluteUri.Replace("/WebForm", "")
                            Dim lCurrent As String = lUrl.Substring(0, lUrl.LastIndexOf("/"))
                            lHtml.AppendLine("    <li><a href='" & lCurrent & m.URL & "?MENU_NO=" & m.MENU_NO & "' class='btn btn-menu btn-lg'>" & m.TITLE & "</a></li>")
                        End If
                    Else
                        lHtml.AppendLine("<li><h4>" & m.TITLE & "</h4>")
                        lHtml.AppendLine("  <ul>")
                        lHtml.Append(GetMenu2(m.MENU_NO, vPrivilege))
                        lHtml.AppendLine("  </ul>")
                        lHtml.AppendLine("</li>")
                    End If
                Next
            Catch ex As Exception
                Throw New Exception("メニューデータの取得に失敗しました。（" & ex.Message & "）", ex)
            End Try
            Return lHtml.ToString
        End Function

        ''' <summary>
        ''' メニュー番号からURLを取得する
        ''' </summary>
        ''' <param name="vMenuNo">メニュー番号</param>
        ''' <param name="vPrivilege">権限</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetMenuURL(ByVal vMenuNo As String, ByVal vPrivilege As String, ByRef vTarget As String) As String
            Dim lRst As String = String.Empty
            Dim lEty As New Ety_M_MENU

            Try
                Dim lDt As DS_M_MENU.M_MENUDtDataTable
                lDt = lEty.GetMenuData(vMenuNo, vPrivilege)
                If lDt.Count > 0 Then
                    If lDt(0).URL <> "#" Then
                        lRst = lDt(0).URL & "?MENU_NO=" & lDt(0).MENU_NO
                        vTarget = lDt(0).FORM_ID
                    End If
                End If
            Catch ex As Exception
                Throw New Exception("メニューデータの取得に失敗しました。（" & ex.Message & "）", ex)
            End Try
            Return lRst
        End Function

        ''' <summary>
        ''' 使用権限フラグが１のときTrueを返す
        ''' </summary>
        ''' <param name="vMenuNo">メニュー番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsDispAuthFlg(ByVal vMenuNo As String) As Boolean
            Try
                Dim lM2Ety As New Ety_M_MENU
                Return lM2Ety.IsDispAuthFlg(vMenuNo)
            Catch ex As Exception
                Throw
            End Try
        End Function

        ''' <summary>
        ''' メニュー番号を取得する
        ''' </summary>
        ''' <param name="vFormId">Me.GetType.Name</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetMenuNo(vFormId As String) As String
            Try
                Dim s() As String = vFormId.Split("_")
                Dim lM2Ety As New Ety_M_MENU
                Return lM2Ety.GetMenuNo(s(1))
            Catch ex As Exception
                Throw
            End Try
        End Function

    End Class
End Namespace
