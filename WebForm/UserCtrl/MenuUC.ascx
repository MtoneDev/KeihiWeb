<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MenuUC.ascx.vb" Inherits="KeihiWeb.MenuUC" %>

<script src="/Scripts/droppy/jquery.droppy.js" type="text/javascript"></script>
<link href="/Scripts/droppy/droppy.css" rel="stylesheet" type="text/css" />

<ul id='nav'>
  <asp:Literal ID="KeihiMenu" runat="server"></asp:Literal>
  <li><a href='<% =VirtualPathUtility.ToAbsolute("~/Default.aspx") %>'>メニュー</a></li>
  <li><a href='<% =VirtualPathUtility.ToAbsolute("~/Login.aspx") %>'>ログアウト</a></li>
</ul>

<script type='text/javascript'>
    $(function() {
        $('#nav').droppy();
    });
</script>
