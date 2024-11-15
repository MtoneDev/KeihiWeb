Imports System.IO
Imports System.Reflection.MethodBase

Imports KeihiWeb.Common

Namespace Common.uty
    Public Class Uty_File
        ''' <summary>
        ''' カレントフォルダ以下にある、保存期間以前の指定した拡張子を持つファイルを削除する
        ''' </summary>
        ''' <param name="currentfolder">カレントフォルダ</param>
        ''' <param name="extention">削除するファイルの拡張子</param>
        ''' <param name="savemonth">ファイルの保存月数</param>
        ''' <remarks></remarks>
        Public Shared Sub DeleteTemporary(ByVal currentfolder As String,
                                                    ByVal extention As String,
                                                    ByVal savemonth As Integer)

            Dim ext As String = ""
            If extention.Chars(0) = "." Then
                ext = "*" & extention
            Else
                ext = "*." & extention
            End If

            Try
                '指定された拡張子を持つファイルのみ削除する
                Dim files As String() = Directory.GetFiles(currentfolder, ext, SearchOption.AllDirectories)
                If files.Length > 0 Then
                    For Each s As String In files
                        Dim dt As Date = File.GetLastWriteTime(s)
                        If dt.AddMonths(savemonth) < Now() Then
                            File.Delete(s)
                        End If
                    Next
                End If
            Catch ex As System.Exception
                Throw New Exception("ファイルの削除に失敗しました。", ex)
            End Try

            'セッションステートモードがInProcの場合、フォルダが削除されると
            'ワーカースレッドが再起動されセッション状態が保持されなくなる
            'フォルダを削除する必要がある場合には、セッションステートモードを
            'StateServerに設定してください
            'Try
            '    'フォルダの中が空の場合に削除する
            '    Dim subFolders As String() = Directory.GetDirectories(currentfolder, "*", SearchOption.AllDirectories)
            '    For Each d As String In subFolders
            '        Dim f As String() = Directory.GetFiles(d, "*", SearchOption.AllDirectories)
            '        If f.Length = 0 And Directory.Exists(d) Then
            '            Directory.Delete(d)
            '        End If
            '    Next
            'Catch ex As Exception
            '    Throw New TenposysException("フォルダの削除に失敗しました。", ex)
            'End Try

        End Sub

        ''' <summary>
        ''' 指定したフォルダを削除する
        ''' </summary>
        ''' <param name="currentfolder">フォルダパス</param>
        ''' <remarks></remarks>
        Public Shared Sub DeleteFolder(ByVal currentfolder As String)

            'セッションステートモードがInProcの場合、フォルダが削除されると
            'ワーカースレッドが再起動されセッション状態が保持されなくなる
            'フォルダを削除する必要がある場合には、セッションステートモードを
            'StateServerに設定してください
            Try
                'フォルダの中が空の場合に削除する
                Dim subFolders As String() = Directory.GetDirectories(currentfolder, "*", SearchOption.AllDirectories)
                For Each d As String In subFolders
                    Dim f As String() = Directory.GetFiles(d, "*", SearchOption.AllDirectories)
                    If f.Length = 0 And Directory.Exists(d) Then
                        Directory.Delete(d)
                    End If
                Next
                Dim pf As String() = Directory.GetFiles(currentfolder, "*", SearchOption.AllDirectories)
                If pf.Length = 0 And Directory.Exists(currentfolder) Then
                    Directory.Delete(currentfolder)
                End If
            Catch ex As System.Exception
                Throw New Exception("フォルダの削除に失敗しました。", ex)
            End Try

        End Sub

        ''' <summary>
        ''' 指定したファイルを削除する
        ''' </summary>
        ''' <param name="vPath">フルパス</param>
        ''' <remarks></remarks>
        Public Shared Sub DeleteFile(ByVal vPath As String)
            Try
                If File.Exists(vPath) Then
                    File.Delete(vPath)
                Else
                    Throw New Exception("指定されたファイルが見つかりません。")
                End If
            Catch ex As System.Exception
                Throw New Exception("ファイルの削除に失敗しました。", ex)
            End Try
        End Sub

        ''' <summary>
        ''' ファイル名から拡張子を取得する
        ''' </summary>
        ''' <param name="vFilename">パス</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetExtension(ByVal vFilename As String) As String
            Try
                Return vFilename.Substring(vFilename.LastIndexOf(".") + 1)
            Catch ex As System.Exception
                Throw New Exception(GetCurrentMethod.Name & ":" & ex.Message, ex)
            End Try
        End Function

        ''' <summary>
        ''' パスからファイル名を取得する
        ''' </summary>
        ''' <param name="vPath">パス</param>
        ''' <param name="vIsExt">True：拡張子付　False：拡張子なし</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFilename(ByVal vPath As String, Optional ByVal vIsExt As Boolean = True) As String
            Try
                Dim lRet As String = vPath
                If vPath.LastIndexOf("\") <> -1 Then
                    lRet = vPath.Substring(vPath.LastIndexOf("\") + 1)
                    If Not vIsExt Then
                        lRet = lRet.Substring(0, lRet.LastIndexOf("."))
                    End If
                Else
                    If Not vIsExt Then
                        lRet = lRet.Substring(0, lRet.LastIndexOf("."))
                    End If
                End If
                Return lRet
            Catch ex As System.Exception
                Throw New Exception(GetCurrentMethod.Name & ":" & ex.Message, ex)
            End Try
        End Function

        ''' <summary>
        ''' パスからフォルダ名を取得する
        ''' </summary>
        ''' <param name="vPath">パス</param>
        ''' <param name="vMark">最後の\マークが必要な場合、1を渡す</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFolder(ByVal vPath As String, Optional ByVal vMark As Integer = 0) As String
            Try
                Return vPath.Substring(0, vPath.LastIndexOf("\") + vMark)
            Catch ex As System.Exception
                Throw New Exception(GetCurrentMethod.Name & ":" & ex.Message, ex)
            End Try
        End Function

        ''' <summary>
        ''' フォルダ名の最後に\がなければ追記する
        ''' </summary>
        ''' <param name="vFolder">フォルダ名</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function AddFolderMark(ByVal vFolder As String) As String
            Try
                'EventLog.WriteEntry("AddFolderMark", "Entering AddFolderMark()" & vFolder)

                Dim lRet As String = vFolder
                If vFolder.Substring(vFolder.Length - 1) <> "\" Then
                    lRet &= "\"
                End If
                Return lRet
            Catch ex As System.Exception
                'EventLog.WriteEntry("AddFolderMark", ex.ToString)
                Throw New Exception(GetCurrentMethod.Name & ":" & ex.ToString, ex)
            End Try
        End Function

        ''' <summary>
        ''' フォルダが存在していなければ作成する
        ''' </summary>
        ''' <param name="vFolder"></param>
        ''' <remarks></remarks>
        Public Shared Sub CreateFolder(ByVal vFolder As String)
            Try
                If Not Directory.Exists(vFolder) Then
                    Directory.CreateDirectory(vFolder)
                End If
            Catch ex As System.Exception
                Throw New Exception(GetCurrentMethod.Name & ":" & ex.Message, ex)
            End Try
        End Sub

        ''' <summary>
        ''' ファイル名に日付を付加する
        ''' </summary>
        ''' <param name="vFile">ファイル名</param>
        ''' <param name="vFormat">日付書式（省略時：yyyyMMddHHmmssfff）</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function RenameAddDate(ByVal vFile As String, Optional ByVal vFormat As String = "yyyyMMddHHmmssfffffff") As String
            Dim lExt As String = Uty_File.GetExtension(vFile)
            Return GetFilename(vFile.Substring(0, vFile.LastIndexOf(".")) & "_" & Now.ToString("yyyyMMddHHmmssfffffff") & "." & lExt)
        End Function

        ''' <summary>
        ''' ファイルをコピーする。コピー先にファイルが存在した場合、指定回数までファイル名末尾にカウンタを付加してコピーする。
        ''' </summary>
        ''' <param name="vSrcFile">コピー元パス</param>
        ''' <param name="vTargetFile">コピー先パス</param>
        ''' <param name="vCount">回数</param>
        ''' <returns>コピー出来た場合：True　コピー出来なかった場合：False</returns>
        ''' <remarks></remarks>
        Public Shared Function CountFileCopy(ByVal vSrcFile As String, ByRef vTargetFile As String, ByVal vCount As Integer) As Boolean
            Dim lRst As Boolean = False
            Dim lExt As String = Uty_File.GetExtension(vTargetFile)
            For i As Integer = 0 To vCount
                Dim f As String = IIf(i = 0, vTargetFile, vTargetFile.Substring(0, vTargetFile.LastIndexOf(".")) & "_" & i & "." & lExt)
                If Not File.Exists(f) Then
                    FileIO.FileSystem.CopyFile(vSrcFile, f)
                    vTargetFile = f
                    lRst = True
                    Exit For
                End If
            Next
            Return lRst
        End Function

        ''' <summary>
        ''' 指定したパスが存在していた場合、指定回数分リネームする
        ''' </summary>
        ''' <param name="vPath">パス（戻り値）</param>
        ''' <param name="vCount">回数</param>
        ''' <returns>True：リネーム成功　False：リネーム失敗</returns>
        ''' <remarks></remarks>
        Public Shared Function ExistRename(ByRef vPath As String, ByVal vCount As Integer) As Boolean
            Dim lRst As Boolean = False
            Dim lDir As String = GetFolder(vPath, 1)
            Dim lFilename As String = GetFilename(vPath, False)
            Dim lExt As String = GetExtension(vPath)
            Dim lPath As String = vPath

            For i As Integer = 1 To vCount
                If System.IO.File.Exists(lPath) Then
                    lRst = False
                    lPath = lDir & lFilename & "_" & i & "." & lExt
                Else
                    lRst = True
                    vPath = lPath
                End If
            Next

            Return lRst
        End Function
    End Class
End Namespace
