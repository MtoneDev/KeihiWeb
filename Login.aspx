<%@ Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="KeihiWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>経費システム</title>
    <link type="text/css" rel="stylesheet" href="~/Content/bootstrap.css" />
    <link type="text/css" rel="stylesheet" href="~/Content/Common.css" />
</head>
<body onload="javascript:document.getElementById('txtUserID').focus();">
    <form id="form1" runat="server">
        <div class="container">
            <div class="jumbotron">
                <h1>経費システム</h1>
                <p class="lead">当システムは株式会社さくらコマース専用の経費システムです。出納帳や請求書の管理を行います。</p>
            </div>
            <div class="center">
                <table class="default">
                    <tr>
                        <th>
                            <h4>ログインID&nbsp;</h4>
                        </th>
                        <td>
                            <asp:TextBox ID="txtUserID" runat="server" CssClass="input-lg"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <h4>パスワード&nbsp;</h4>
                        </th>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-lg"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="none">
                        </th>
                        <td class="none">
                            <asp:Button ID="cmdLogin" runat="server" cssclass="btn btn-default" Text="ログイン" />
                            <!--<asp:Button ID="cmdPassChange" runat="server" cssclass="btn btn-default" Text="パスワード変更" />-->
                        </td>
                    </tr>
                </table>
            </div>
            <div id="MessageArea" style="text-align:center;">
                <asp:Label ID="LabelMessage" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
