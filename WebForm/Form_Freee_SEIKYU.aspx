<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_Freee_SEIKYU.aspx.vb" Inherits="KeihiWeb.Form_Freee_SEIKYU" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#btnExec').click(function () {
                var msg = $('#<%=LabelMessage.ClientID%>');
                msg[0].innerText = '';

                // 認証チェック
                var bumon = $('#<%=Master.FindControl("auth_bumon_cd").ClientID%>');
                if (bumon[0].value == '') {
                    msg[0].innerText = 'ログインしてください。';
                };

                // 締日取得
                var shimebi = $('#<%=txtShimeDate.ClientID%>').val();
                if (shimebi == '') {
                    msg[0].innerText = '締日を正しく入力してください。';
                    return false;
                }

                // 選択された部門を取得
                var chkRec = $('input[name*=chkSelect]:checked').parents('tr');
                if (chkRec.length == 0) {
                    alert('締め処理を行う部門を選択してください');
                    return false;
                }

                // paramに締日、部門コードを格納する
                var param = shimebi + ',';
                for (i = 0; i < chkRec.length; i++) {
                    var rec = chkRec[i];
                    param += $.trim($(rec).children("td:nth-child(2)")[0].innerText) + ',';
                }

                // 処理中メッセージ表示
                $.blockUI({
                    message: '<div><img src="/Content/image/indicator.gif" style="margin-right:5px;">Freee連携ファイルを生成しています。</div>',
                    css: {
                        padding: '25px'
                    }
                });

                // GLOVIAファイル生成処理実行
                $.ajax({
                    type: "POST",
                    url: "Form_Freee_SEIKYU.aspx/CreateFreeeFile",
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
                    }
                });
            });
        });
    </script>

    <div id="Container">
        <div class="index1">
            請求仕訳　Freee出力
        </div>
        <div class="body">
            <div class="groupbox">
                <h4>処理期間</h4>
                <asp:TextBox ID="txtNen" runat="server" CssClass="input-sm displayBackColor" Width="6em" ReadOnly="true"></asp:TextBox>年
                <asp:TextBox ID="txtTuki" runat="server" CssClass="input-sm displayBackColor" Width="4em" ReadOnly="true"></asp:TextBox>月
                <h4>締日：</h4>
                <asp:TextBox ID="txtShimeDate" runat="server" CssClass="input-sm displayBackColor" ReadOnly="true"></asp:TextBox>
                <asp:Button ID="btnPrev" runat="server" Text="前期" CssClass="btn btn-default" />
                <asp:Button ID="btnNext" runat="server" Text="次期" CssClass="btn btn-default" />
            </div>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" HorizontalAlign="Center" Width="500px" Height="300px" >
                <asp:ListView ID="ListView1" runat="server">
                  <LayoutTemplate>
                    <table id="highlight_after" class="fixdetail">
                      <thead>
                        <tr>
                            <th>選択</th>
                            <th>部門ＣＤ</th>
                            <th>部門名</th>
                            <th>前回締め日</th>
                        </tr>
                      </thead>
                      <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                      </tbody>
                    </table>
                  </LayoutTemplate>
                  <ItemTemplate>
                    <tr id="<%# CType(Container, ListViewDataItem).DataItemIndex + 1 %>" class="color<%# (CType(Container, ListViewDataItem).DataItemIndex + 1) mod 2 %>">
                      <td style="text-align:center;">
                          <asp:CheckBox ID="chkSelect" runat="server" />
                      </td>
                      <td>
                        <asp:Label ID="lblBUMON_CD" runat="server" Text='<%#Eval("BUMON_CD")%>' />
                      </td>
                      <td>
                        <asp:Label ID="lblBUMON_NM" runat="server" Text='<%#HttpUtility.HtmlEncode(Eval("BUMON_NM"))%>' />
                      </td>
                      <td>
                        <asp:Label ID="lblZENKAI_SHIMEBI" runat="server" Text='<%#Eval("CLOSE_DATE")%>' />
                     </td>
                    </tr>
                  </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
            <div id="button_group">
                <input type="button" ID="btnExec" class="btn btn-default" value="実行" />
                <asp:Button ID="btnClose" runat="server" CssClass="btn btn-default" Text="閉じる" />
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
