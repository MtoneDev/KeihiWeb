<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="KeihiWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div>
            <ul id="SystemMenu">
                <asp:Literal ID="menu" runat="server"></asp:Literal>
            </ul>
        </div>
        <div class="clearLeft">&nbsp;</div>
        <div id="MessageArea">
            <asp:Label ID="LabelMessage" runat="server" />
        </div>
    </div>
</asp:Content>
