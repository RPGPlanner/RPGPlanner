<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="test" Title="Untitled Page" Codebehind="test.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    <asp:Label ID="lbAnswer" runat="server"></asp:Label><br />
    <br />
    <asp:Button ID="btnChangeMail" runat="server" OnClick="btnChangeMail_Click" Text="Change Mail for Today" />
    <br />
    <asp:Button ID="btnRemind" runat="server" OnClick="btnRemind_Click" Text="Reminder Mail for next Days" />
</asp:Content>

