<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="From_BumonChange.aspx.vb" Inherits="KeihiWeb.From_BumonChange" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
         /* --- マウスオーバー時 --- */
        table#highlight_after tr:hover {
            background-color: #CC99FF; /* 行の背景色 */
        }
    </style>
    <script type="text/javascript">
    </script>

    <div id="Container">
        <div class="index1">
            自部門変更
        </div>
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" HorizontalAlign="Center" Width="1100px" Height="500px" >
            <asp:ListView ID="ListView1" runat="server">
              <LayoutTemplate>
                <table id="highlight_after" class="fixdetail">
                  <thead>
                    <tr>
                        <th>選択</th>
                        <th>部門ＣＤ</th>
                        <th>部門名</th>
                        <th>債務部門</th>
                    </tr>
                  </thead>
                  <tbody>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                  </tbody>
                </table>
              </LayoutTemplate>
              <ItemTemplate>
                <tr id="<%# CType(Container, ListViewDataItem).DataItemIndex + 1 %>" class="color<%# (CType(Container, ListViewDataItem).DataItemIndex + 1) mod 2 %>">
                  <td>
                      <asp:Button ID="btnSelect" runat="server" Text="選択" CssClass="input-sm" CommandName="select" CommandArgument="<%# CType(Container, ListViewDataItem).DataItemIndex %>" />
                  </td>
                  <td>
                    <asp:Label ID="lblBUMON_CD" runat="server" Text='<%#Eval("BUMON_CD")%>' />
                  </td>
                  <td>
                    <asp:Label ID="lblBUMON_NM" runat="server" Text='<%#HttpUtility.HtmlEncode(Eval("BUMON_NM"))%>' />
                  </td>
                  <td>
                    <asp:Label ID="lblSAIMU_BMN" runat="server" Text='<%#Eval("SAIMU_BMN")%>' />
                  </td>
                </tr>
              </ItemTemplate>
            </asp:ListView>
        </asp:Panel>
        <div id="button_group">
            <asp:Button ID="btnClose" runat="server" CssClass="btn btn-default" Text="閉じる" />
        </div>    
        <div id="MessageArea">
            <asp:Label ID="LabelMessage" runat="server" />
        </div>
    </div>
</asp:Content>
