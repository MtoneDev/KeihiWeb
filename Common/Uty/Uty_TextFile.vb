Imports System.IO
Imports KeihiWeb.Common

Namespace Common.uty

    Public Class Uty_TextFile
        Private Shared _enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")

        ''' <summary>
        ''' IEnumerableを取得して、カンマ区切りの文字列を返す
        ''' </summary>
        ''' <param name="enumerable">コレクション、または配列</param>
        ''' <param name="separator">デリミタ</param>
        ''' <param name="quote">文字列をダブルクォーテーションで囲む場合はTrueを渡す</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Join(ByVal enumerable As IEnumerable, ByVal separator As String, Optional ByVal quote As Boolean = False) As String

            Dim builder As New StringBuilder
            Dim first As Boolean = True

            For Each item As Object In enumerable
                ' 一回目を除きseparatorを出力
                If first Then
                    first = False
                Else
                    builder.Append(separator)
                End If
                If quote Then
                    builder.Append("""" & item.ToString() & """")
                Else
                    builder.Append(item.ToString())
                End If
            Next

            Return builder.ToString()

        End Function

        ''' <summary>
        ''' 文字列をテキストファイルに書き出す
        ''' </summary>
        ''' <param name="text">文字列</param>
        ''' <param name="fullpath">フルパス</param>
        ''' <param name="OverWrite">上書きのときTrueを指定する</param>
        ''' <remarks></remarks>
        Public Shared Sub TextWriter(ByVal text As String, ByVal fullpath As String, Optional ByVal OverWrite As Boolean = False)

            Using sw As New StreamWriter(fullpath, OverWrite, _enc)
                Try
                    sw.AutoFlush = True
                    sw.Write(text)
                Finally
                    sw.Close()
                End Try
            End Using

        End Sub

        ''' <summary>
        ''' テキストファイルに出力する
        ''' </summary>
        ''' <param name="vText">文字列（StringBuilder型）</param>
        ''' <param name="vAppend">True:追記　False:上書き</param>
        ''' <param name="fullpath">フルパス名</param>
        ''' <remarks></remarks>
        Public Shared Sub TextWriter(ByVal vText As StringBuilder, vAppend As Boolean, ByVal fullpath As String)

            Using sw As New StreamWriter(fullpath, vAppend, _enc)
                Try
                    sw.AutoFlush = True
                    sw.Write(vText)
                Finally
                    sw.Close()
                End Try
            End Using

        End Sub

        ''' <summary>
        ''' テキストファイルに出力する
        ''' </summary>
        ''' <param name="dt">DataTable</param>
        ''' <param name="fullpath">フルパス名</param>
        ''' <remarks></remarks>
        Public Shared Sub TextWriter(ByVal dt As DataTable, ByVal fullpath As String, Optional ByVal PrintCaption As Boolean = False, Optional ByVal OverWrite As Boolean = False)

            Dim delim As String = Uty_Common.ConvertDelim(Uty_Config.Delimiter)

            Using sw As New System.IO.StreamWriter(fullpath, OverWrite, _enc)
                Try
                    sw.AutoFlush = True
                    Dim colCount As Integer = dt.Columns.Count
                    Dim lastColIndex As Integer = colCount - 1

                    '列見出しとして、カラムキャプションを出力する
                    If PrintCaption Then
                        For j As Integer = 0 To colCount - 1
                            Dim colname As String = dt.Columns(j).Caption
                            sw.Write(colname)
                            If lastColIndex > j Then
                                sw.Write(delim)
                            End If
                        Next
                        sw.Write(vbCrLf)
                        PrintCaption = False
                    End If

                    'レコードを書き込む
                    Dim row As DataRow
                    For Each row In dt.Rows
                        For i As Integer = 0 To colCount - 1
                            'フィールドの取得
                            Dim field As String = row(i).ToString().TrimEnd
                            'フィールドを書き込む
                            sw.Write(field)
                            'デリミタを書き込む
                            If lastColIndex > i Then
                                sw.Write(delim)
                            End If
                        Next i
                        '改行する
                        sw.Write(vbCrLf)
                    Next row

                Catch ex As System.Exception
                    Throw
                Finally
                    sw.Close()
                End Try
            End Using

        End Sub

        Public Shared Function ReadAllText(ByVal vFullpath As String) As String
            'テキストファイルの中身をすべて読み込む
            Return System.IO.File.ReadAllText(vFullpath, _enc)
        End Function

        Public Shared Function ReadAllLines(ByVal vFullpath As String) As String()
            'テキストファイルの中身をすべて読み込む
            Return System.IO.File.ReadAllLines(vFullpath, _enc)
        End Function

        ''' <summary>
        ''' 1行当たりの桁数が指定桁数より多い時、Trueを返す
        ''' </summary>
        ''' <param name="vPath">ファイルパス</param>
        ''' <param name="vLength">桁数</param>
        ''' <returns>桁数を超えた：True　桁数以内：False</returns>
        ''' <remarks></remarks>
        Public Shared Function IsOverTextLength(ByVal vPath As String, ByVal vLength As Integer) As Boolean
            '桁数が0以下の時はチェックしない
            If vLength <= 0 Then
                Return False
            End If
            Using sr As New StreamReader(vPath, _enc)
                Try
                    While sr.Peek() >= 0
                        '一行読み込んで表示する
                        Dim lLine As String = sr.ReadLine()
                        If lLine.Length > vLength Then
                            Return True
                        End If
                    End While
                    Return False
                Finally
                    sr.Close()
                End Try
            End Using
        End Function


    End Class
End Namespace
