<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_Print_SUITOU.aspx.vb" Inherits="KeihiWeb.Form_Print_SUITOU" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
      $(function() {
        $.datepicker.setDefaults( $.datepicker.regional[ "ja" ] );
        $( ".datepicker" ).datepicker();
      });
    </script>

    <div id="Container">
        <div class="index1">
            現金出納帳印刷
        </div>
        <div class="body">
            <div>
                <h4>伝票入力期間：</h4>
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="datepicker input-lg"></asp:TextBox>
                ～<asp:TextBox ID="txtEndDate" runat="server" CssClass="datepicker input-lg"></asp:TextBox>
            </div>
            <div class="dispmsg">伝票入力期間を入力して、帳票出力するボタンをクリックしてください。</div>
            <div id="button_group">
                <asp:Button ID="btnSuitoucho" runat="server" Text="出納帳出力" cssclass="btn btn-default" />
                <asp:Button ID="btnMeisai" runat="server" Text="明細表出力" cssclass="btn btn-default" />
                <asp:Button ID="btnClose" runat="server" Text="閉じる" CssClass="btn btn-default" />
            </div>
        </div>
        <div id="MessageArea">
            <asp:Label ID="LabelMessage" runat="server" />
        </div>
    </div>
</asp:Content>
