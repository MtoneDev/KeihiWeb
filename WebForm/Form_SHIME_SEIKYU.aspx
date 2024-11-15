<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_SHIME_SEIKYU.aspx.vb" Inherits="KeihiWeb.Form_SHIME_SEIKYU" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#title').css('background-color', '#5BC85B');
            $('#title').css('border-left-color', 'Green');

            $('#<%=btnExec.ClientID%>').click(function () {

                // 取消の場合、以下の処理を中止してポストバックする
                var tori = $('#<%=radioCancel.ClientID%>');
                if (tori[0].checked == true) {
                    return true;
                }

                var msg = $('#<%=LabelMessage.ClientID%>');
                msg[0].innerText = '';

                // 認証チェック
                var jibumon = $('#<%=Master.FindControl("auth_bumon_cd").ClientID%>').val();
                if (jibumon == '') {
                    msg[0].innerText = 'ログインしてください。';
                }

                // 締日取得
                var shimebi = $('#<%=txtShimeDate.ClientID%>').val();
                if (shimebi == '') {
                    msg[0].innerText = '締日を正しく入力してください。';
                    return false;
                }

                // 印刷チェックボックス
                var print = $('#<%=chkPrint.ClientID%>');

                // paramに締日、部門コード、印刷チェックボックスを格納する
                var param = jibumon + ',' + shimebi + ',' + print[0].checked;

                // 処理中メッセージ表示
                $.blockUI({
                    message: '<div><img src="/Content/image/indicator.gif" style="margin-right:5px;">締め処理を実行しています。</div>',
                    css: {
                        padding: '25px'
                    }
                });

                // GLOVIAファイル生成処理実行
                $.ajax({
                    type: "POST",
                    url: "Form_SHIME_SEIKYU.aspx/ExecShime",
                    data: JSON.stringify({ vParam: param }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (responses) {
                        $.unblockUI();
                        var link = $('#literal');
                        link[0].innerHTML = responses.d;
                    },
                    error: function () {
                        $.unblockUI();
                        alert("エラーが発生しました");
                    },
                    complete: function () {
                        return false;
                    }
                });
                $('#<%=btnExec.ClientID%>').prop("disabled", true);
                return false;
            });
        });
    </script>

    <div id="Container">
        <div id="title" class="index1">
            請求書　締め処理
        </div>
        <div class="body">
            <div class="groupbox">
                <asp:RadioButton ID="radioShime" runat="server" GroupName="PROCESS" Text="締め処理" AutoPostBack="true" />
                <asp:RadioButton ID="radioCancel" runat="server" GroupName="PROCESS" Text="締め取消" AutoPostBack="true" />
            </div>  
            <div class="groupbox">
                <h4>締め期間</h4>
                <asp:TextBox ID="txtNen" runat="server" CssClass="input-sm displayBackColor" Width="6em" ReadOnly="true"></asp:TextBox>年
                <asp:TextBox ID="txtTuki" runat="server" CssClass="input-sm displayBackColor" Width="4em" ReadOnly="true"></asp:TextBox>月
                <h4>締日：</h4>
                <asp:TextBox ID="txtShimeDate" runat="server" CssClass="input-sm displayBackColor" ReadOnly="true"></asp:TextBox>
                <asp:CheckBox ID="chkPrint" runat="server" Text="印刷" />
                <asp:Button ID="btnPrev" runat="server" Text="前期" CssClass="btn btn-default" Visible="false" />
                <asp:Button ID="btnNext" runat="server" Text="次期" CssClass="btn btn-default" Visible="false" />
            </div>
            <div id="button_group">
                <asp:Button ID="btnExec" runat="server" Text="実行" CssClass="btn btn-default" />
                <asp:Button ID="btnClose" runat="server" Text="閉じる" CssClass="btn btn-default" />
            </div>
        </div>
        <div id="literal">
            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
        </div>
        <div id="MessageArea">
            <asp:Label ID="LabelMessage" runat="server" />
        </div>
    </div>
</asp:Content>
