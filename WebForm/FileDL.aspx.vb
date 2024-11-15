Imports KeihiWeb.Common.uty

Public Class FileDL
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '生成されたファイルをダウンロードするダイアログボックスを表示する
        Try
            'ファイルをダウンロードする
            Dim FileID As String = Request.QueryString("FILENAME")
            Dim DLFilename As String = Request.QueryString("DLNAME")
            If DLFilename Is Nothing Then
                DLFilename = FileID.Substring(FileID.LastIndexOf("\") + 1)
            ElseIf DLFilename.Trim = "" Then
                DLFilename = FileID.Substring(FileID.LastIndexOf("\") + 1)
            End If
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.ContentEncoding = System.Text.Encoding.GetEncoding(Uty_Config.ENC)
            Response.AppendHeader("Content-Disposition", "attachment; filename=" & HttpUtility.UrlEncode(FileID))
            'Response.AppendHeader("Content-Transfer-Encoding", "Base64")
            Response.WriteFile(DLFilename)
            Response.Flush()
            'Response.Close()

            ' テンポラリファイル削除 
            System.IO.File.Delete(DLFilename)
            Response.ClearContent()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
            Response.End()

        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class