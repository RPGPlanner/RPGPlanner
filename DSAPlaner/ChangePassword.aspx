<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="ChangePassword" Title="DSA Planer - Passwort ändern" Codebehind="ChangePassword.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
<h2>Passwort &auml;ndern</h2>
    <p>
        <asp:Label ID="lbAnswer" runat="server" CssClass="AnswerLabel"></asp:Label>&nbsp;</p>
    <p>
        <table>
            <tr>
                <td>Neues Passwort:
                </td>
                <td>
                    <asp:TextBox ID="tbPass1" runat="server" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Passwort wiederholen:</td>
                <td>
                    <asp:TextBox ID="tbPass2" runat="server" TextMode="Password"></asp:TextBox></td>
             </tr>
        </table>
    </p>
    <asp:Button ID="btnChange" runat="server" OnClick="btnChange_Click" Text="Neues Passwort setzen" />
</asp:Content>

