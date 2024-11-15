Imports System.IO
Imports KeihiWeb.Common

Namespace Common.uty

    Public Class Uty_Common
        Private Shared sjisEnc As Encoding = Encoding.GetEncoding("Shift_JIS")

        Public Shared Sub SetModeTextBox(ByVal ptxtOBJ As TextBox, ByVal pstrMODE As String)

            Select Case pstrMODE
                Case "IME_ON"
                    ptxtOBJ.Style.Add("ime-mode", "active")
                Case "IME_OFF"
                    ptxtOBJ.Attributes.Add("onBlur", "this.value = this.value.toHankaku();")
                    ptxtOBJ.Style.Add("ime-mode", "disabled")
                Case "IME_OFF_ALPHA"
                    ptxtOBJ.Attributes.Add("onBlur", "this.value = this.value.toLargeAlpha();")
                    ptxtOBJ.Style.Add("ime-mode", "disabled")
                Case "NUMBER"
                    ptxtOBJ.Attributes.Add("onkeypress", "return CheckNumNumelic();")
                    ptxtOBJ.Style.Add("ime-mode", "disabled")
                Case "NUMBER2"
                    'STS20211020-不具合対応
                    'ptxtOBJ.Attributes.Add("onkeypress", "return CheckNumNumelic2(this.value,'');")
                    ptxtOBJ.Attributes.Add("onkeypress", "return CheckNumNumelic2();")
                    ptxtOBJ.Style.Add("ime-mode", "disabled")
                Case "NUMBER3"
                    ptxtOBJ.Attributes.Add("onkeypress", "return CheckNumNumelicSpace2();")
                    ptxtOBJ.Style.Add("ime-mode", "disabled")
                Case "NUMBER7"
                    ptxtOBJ.Attributes.Add("onkeypress", "return CheckNumNumelic7(this.value);")
                    ptxtOBJ.Style.Add("ime-mode", "disabled")
                    '20210210-改修
                Case "INACTIVE"
                    ptxtOBJ.Style.Add("ime-mode", "inactive")

            End Select

        End Sub



        ''' <summary>
        ''' 文字列が全て全角かどうかを判定する
        ''' </summary>
        ''' <param name="vStr">文字列</param>
        ''' <returns>全て全角ならTrue、そうでなければFalse</returns>
        ''' <remarks></remarks>
        Public Shared Function isZenkaku(ByVal vStr As String) As Boolean
            Dim num As Integer = sjisEnc.GetByteCount(vStr)
            Return num = vStr.Length * 2
        End Function

        ''' <summary>
        ''' 文字列が全て半角かどうかを判定する
        ''' </summary>
        ''' <param name="vStr">文字列</param>
        ''' <returns>全て半角ならTrue、そうでなければFalse</returns>
        ''' <remarks></remarks>
        Public Shared Function isHankaku(ByVal vStr As String) As Boolean
            Dim num As Integer = sjisEnc.GetByteCount(vStr)
            Return num = vStr.Length
        End Function

        ''' <summary>
        ''' 文字列中にスペースが含まれているか判定する
        ''' </summary>
        ''' <param name="vStr">文字列</param>
        ''' <returns>文字列中にスペースが含まれていればTrueを返す</returns>
        ''' <remarks></remarks>
        Public Shared Function IsExistSpace(ByVal vStr As String) As Boolean
            If vStr.TrimEnd.IndexOf(" ") > -1 Then
                Return True
            End If
            If vStr.TrimEnd.IndexOf("　") > -1 Then
                Return True
            End If
            If vStr.TrimEnd.IndexOf(vbTab) > -1 Then
                Return True
            End If
            Return False
        End Function

        ''' <summary>
        ''' SOK、EOKを考慮した全角、半角混じり文字列のバイト数を取得する
        ''' </summary>
        ''' <param name="vStr">文字列</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetByteCountToHost(ByVal vStr As String) As Integer
            Dim lCnt As Integer = 0
            Dim lWideFlag As Boolean = False
            For Each c As String In vStr
                If isZenkaku(c) Then
                    If Not lWideFlag Then
                        lCnt += 2       'SOK／EOKにかかる1バイトを加算
                        lWideFlag = True
                    End If
                    lCnt += 2
                Else
                    lWideFlag = False
                    lCnt += 1
                End If
            Next
            Return lCnt
        End Function

        ''' <summary>
        ''' 半角全角交じり文字列のバイト数を取得する
        ''' </summary>
        ''' <param name="vStr">文字列</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetByteCount(ByVal vStr As String) As Integer
            Dim lCnt As Integer = 0
            Dim lWideFlag As Boolean = False
            For Each c As String In vStr
                If isZenkaku(c) Then
                    lCnt += 2
                Else
                    lCnt += 1
                End If
            Next
            Return lCnt
        End Function

        ''' <summary>
        ''' 文字列をHTMLエンコードする
        ''' </summary>
        ''' <param name="v">文字列</param>
        ''' <returns>HTMLエンコードされた文字列</returns>
        ''' <remarks>半角スペースは＆nbsp;に変換される</remarks>
        Public Shared Function htmlenc(ByVal v As String) As String
            Dim buf As String = HttpUtility.HtmlEncode(v)
            If buf.Trim = "" Or buf Is Nothing Then
                Return "&nbsp;"
            Else
                buf = buf.Replace(" ", "&nbsp;")
                buf = buf.Replace(vbCrLf, "<br />")
                Return buf
            End If
        End Function

        ''' ------------------------------------------------------------------------
        ''' <summary>
        '''     指定した精度の数値に切り上げします。</summary>
        ''' <param name="dValue">
        '''     丸め対象の倍精度浮動小数点数。</param>
        ''' <param name="iDigits">
        '''     戻り値の有効桁数の精度。</param>
        ''' <returns>
        '''     iDigits に等しい精度の数値に切り上げられた数値。</returns>
        ''' ------------------------------------------------------------------------
        Public Shared Function ToRoundUp(ByVal dValue As Double, ByVal iDigits As Integer) As Double
            Dim dCoef As Double = System.Math.Pow(10, iDigits)

            If dValue > 0 Then
                Return System.Math.Ceiling(dValue * dCoef) / dCoef
            Else
                Return System.Math.Floor(dValue * dCoef) / dCoef
            End If
        End Function

        ''' ------------------------------------------------------------------------
        ''' <summary>
        '''     指定した精度の数値に切り捨てします。</summary>
        ''' <param name="dValue">
        '''     丸め対象の倍精度浮動小数点数。</param>
        ''' <param name="iDigits">
        '''     戻り値の有効桁数の精度。</param>
        ''' <returns>
        '''     iDigits に等しい精度の数値に切り捨てられた数値。</returns>
        ''' ------------------------------------------------------------------------
        Public Shared Function ToRoundDown(ByVal dValue As Double, ByVal iDigits As Integer) As Double
            Dim dCoef As Double = System.Math.Pow(10, iDigits)

            If dValue > 0 Then
                Return System.Math.Floor(dValue * dCoef) / dCoef
            Else
                Return System.Math.Ceiling(dValue * dCoef) / dCoef
            End If
        End Function

        ''' ------------------------------------------------------------------------
        ''' <summary>
        '''     指定した精度の数値に四捨五入します。</summary>
        ''' <param name="dValue">
        '''     丸め対象の倍精度浮動小数点数。</param>
        ''' <param name="iDigits">
        '''     戻り値の有効桁数の精度。</param>
        ''' <returns>
        '''     iDigits に等しい精度の数値に四捨五入された数値。</returns>
        ''' ------------------------------------------------------------------------
        Public Shared Function ToHalfAdjust(ByVal dValue As Double, ByVal iDigits As Integer) As Double
            Dim dCoef As Double = System.Math.Pow(10, iDigits)

            If dValue > 0 Then
                Return System.Math.Floor((dValue * dCoef) + 0.5) / dCoef
            Else
                Return System.Math.Ceiling((dValue * dCoef) - 0.5) / dCoef
            End If
        End Function

        ''' <summary>
        ''' CheckBoxのCheckedプロパティの値を変換する
        ''' </summary>
        ''' <param name="Value">Checkedプロパティ値</param>
        ''' <returns>
        ''' 引数がTrueの場合、1を返す<br />
        ''' 引数がFalseの場合、0を返す<br />
        ''' </returns>
        ''' <remarks></remarks>
        Public Shared Function GetCheckedValue(ByVal Value As Boolean) As Integer
            If Value Then
                Return 1
            Else
                Return 0
            End If
        End Function

        ''' <summary>
        ''' 引数に渡された文字列をCheckBoxのCheckedプロパティに渡す値に変換する
        ''' </summary>
        ''' <param name="Value">"1"、または"0"を渡す</param>
        ''' <returns>
        ''' 引数が1の場合、Trueを返す<br />
        ''' 引数が0の場合、Falseを返す<br />
        ''' </returns>
        ''' <remarks></remarks>
        Public Shared Function SetCheckedValue(ByVal Value As Integer) As Boolean
            If Value = 1 Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' 6桁または8桁の数値の連続で表現された日付をyyyy/MM/dd形式に変換する
        ''' </summary>
        ''' <param name="value">日付文字列</param>
        ''' <param name="formatString">出力フォーマット（ToString準拠）</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ChangeCharToDateFormat(ByVal value As String, Optional ByVal formatString As String = "yyyy/MM/dd") As String

            Dim ret As String = String.Empty
            Dim dt As New DateTime
            Try
                If value.Trim <> String.Empty Then
                    If value.Length = 6 Then
                        dt = DateTime.ParseExact(value, "yyMMdd", Nothing)
                        ret = dt.ToString(formatString)

                    ElseIf value.Length = 8 Then
                        dt = DateTime.ParseExact(value, "yyyyMMdd", Nothing)
                        ret = dt.ToString(formatString)

                    ElseIf value.Length = 10 Then
                        If DateTime.TryParse(value, dt) Then
                            ret = dt.ToString(formatString)
                        End If
                    ElseIf value = "0" Then
                        ret = String.Empty
                    End If
                End If
            Catch ex As System.FormatException
                ret = value
            End Try

            Return ret

        End Function

        ''' <summary>
        ''' 文字列yyyyMMddをyyyy/MM/ddの日付型に変換する
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ChangeStringToDate(ByVal value As String) As Date

            Dim dt As DateTime = Nothing

            If value.Trim <> "" Then
                If value.Length = 6 Then
                    dt = DateTime.ParseExact(value, "yyMMdd", Nothing)

                ElseIf value.Length = 8 Then
                    dt = DateTime.ParseExact(value, "yyyyMMdd", Nothing)

                Else
                    dt = DateTime.ParseExact(value, "yyyy/MM/dd", Nothing)
                End If
            End If

            Return dt

        End Function

        ''' <summary>
        ''' 年月に矛盾がないか確認する
        ''' </summary>
        ''' <param name="value">年月を年4桁、月2桁で渡す（例：201102）</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ValidateNengetu(ByVal value As String) As Boolean
            Dim lRet As Boolean = False
            Dim dt As DateTime = Nothing
            If value.Trim <> "" Then
                value &= "01"
                dt = DateTime.ParseExact(value, "yyyyMMdd", Nothing)
                lRet = True
            End If
            Return lRet
        End Function

        ''' <summary>
        ''' 指定した文字列が日付か検査する
        ''' </summary>
        ''' <param name="vValue">文字列</param>
        ''' <returns>True：日付　False：日付でない</returns>
        ''' <remarks></remarks>
        Public Shared Function IsDate(ByVal vValue As String) As Boolean
            Dim lRet As Boolean = False
            Dim dt As DateTime = Nothing
            If vValue.IndexOf("/") > -1 Then
                lRet = DateTime.TryParseExact(vValue, "yyyy/MM/dd", Nothing, Nothing, dt)
            ElseIf vValue.Trim <> "" Then
                lRet = DateTime.TryParseExact(vValue, "yyyyMMdd", Nothing, Nothing, dt)
            End If
            Return lRet
        End Function

        ''' <summary>
        ''' 文字列yyyy/MM/ddをyyyyMMddの日付型に変換する
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ChangeDateToString(ByVal value As Object) As String

            Dim ret As String = ""
            Dim dt As Date = Nothing

            If value.ToString.Trim <> "" Then

                If Date.TryParse(value, dt) = True Then
                    ret = dt.ToString("yyyyMMdd")
                End If

            End If

            Return ret

        End Function

        ''' <summary>
        ''' デリミタをVB形式に変換する
        ''' </summary>
        ''' <param name="s">デリミタ文字列</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertDelim(ByVal s As String) As String
            Select Case s
                Case "," : Return s
                Case "\t" : Return vbTab
                Case Else : Return s
            End Select
        End Function

        ''' <summary>
        ''' 文字列をスペースで分ける
        ''' </summary>
        ''' <param name="vWords">文字列</param>
        ''' <returns>スペースで分けられた文字列が格納されたジェネリックコレクションを返す</returns>
        ''' <remarks>スペースを文字列として格納したい場合はダブルクォートで括る</remarks>
        Public Shared Function SeparateWords(ByVal vWords As String) As List(Of String)
            Dim lWords As New List(Of String)
            Dim lQuoteFlg As Integer = 0
            Dim i As Integer = 0

            If vWords <> String.Empty Then
                Dim lStr As String = String.Empty
                For Each c As Char In vWords
                    'If c = "'" Then
                    '    lQuoteFlg = Not lQuoteFlg
                    '    Continue For
                    'End If
                    If lQuoteFlg = 0 And (c = " " Or c = "　") Then
                        lWords.Add(lStr)
                        lStr = String.Empty
                        Continue For
                    End If
                    lStr &= c
                Next
                If lStr <> String.Empty Then
                    lWords.Add(lStr)
                End If
            End If

            Return lWords
        End Function

        ''' <summary>
        ''' 指定した桁数ごとに文字列を区切る
        ''' </summary>
        ''' <param name="vWord">文字列</param>
        ''' <param name="vSepareter">区切り文字</param>
        ''' <param name="vDigit">桁数</param>
        ''' <returns>指定した桁数ごとに指定した文字で区切られた文字列を返す</returns>
        ''' <remarks>例）SeparateWords(20135606, ":" , 2)　結果＝20:13:56:06</remarks>
        Public Shared Function SeparateWords(ByVal vWord As String, ByVal vSepareter As String, ByVal vDigit As Integer) As String
            Dim lAr As New ArrayList
            For i As Integer = 0 To vWord.Length - 1 Step vDigit
                lAr.Add(vWord.Substring(i, vDigit))
            Next
            Return Join(lAr.ToArray, vSepareter)
        End Function

        ''' <summary>
        ''' 引数が文字列またはブランクの時0を返す
        ''' </summary>
        ''' <param name="vText">文字列</param>
        ''' <returns>数値</returns>
        ''' <remarks></remarks>
        Public Shared Function GetBlankToZero(ByVal vText As String) As Long
            Dim lRst As Long = 0
            If Long.TryParse(vText.Replace(",", "").Replace("%", "").Replace("％", "").Replace("\", ""), lRst) Then
                Return lRst
            Else
                Return 0
            End If
        End Function

        ''' <summary>
        ''' DBNullを0に変換
        ''' </summary>
        ''' <param name="vText">DBNullの可能性があるオブジェクト</param>
        ''' <returns></returns>
        Public Shared Function IsDBNullToZero(ByVal vText As Object) As Object
            Dim lRst As Long = 0
            If IsDBNull(vText) Then
                Return 0
            Else
                Return vText
            End If
        End Function

        ''' <summary>
        ''' 指定月の月末日を取得する
        ''' </summary>
        ''' <param name="vYM">年月をyyyyMMで指定する</param>
        ''' <returns>月末日（Date型）</returns>
        Public Shared Function GetGetsumatu(ByVal vYM As String) As Date
            Dim lRst As New Date
            Dim lTogetsu As Date = Uty_Common.ChangeStringToDate(vYM & "01")
            lRst = lTogetsu.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd")
            Return lRst
        End Function

        ''' <summary>
        ''' 文字列を指定の幅(バイト数)にカットする（漢字分断回避）
        ''' </summary>
        ''' <param name="vString">文字列</param>
        ''' <param name="vLen">桁数</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function StringCut(ByVal vString As String, ByVal vLen As Integer) As String
            '文字列を指定のバイト数にカットする関数(漢字分断回避）
            Dim sjis As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
            Dim TempLen As Integer = sjis.GetByteCount(vString)
            If vLen < 1 Or vString.Length < 1 Then Return vString
            If TempLen <= vLen Then   '文字列が指定のバイト数未満の場合スペースを付加する
                Return vString.PadRight(vLen - (TempLen - vString.Length), CChar(" "))
            End If
            Dim tempByt() As Byte = sjis.GetBytes(vString)
            Dim strTemp As String = sjis.GetString(tempByt, 0, vLen)
            '末尾が漢字分断されたら半角スペースと置き換え(VB2005="・" で.NET2003=NullChar になります）
            If strTemp.EndsWith(ControlChars.NullChar) Or strTemp.EndsWith("・") Then
                strTemp = sjis.GetString(tempByt, 0, vLen - 1) & " "
            End If
            Return strTemp
        End Function

        Public Shared Function StringUpper(ByVal vString As String) As String
            Dim restring As String = ""
            restring = vString.ToUpper

            Return restring

        End Function


    End Class
End Namespace
