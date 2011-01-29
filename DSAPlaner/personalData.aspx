<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="personalData" Title="Untitled Page" Codebehind="personalData.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    <br />
    <asp:Label ID="lbAnswer" CssClass="AnswerLabel" runat="server"></asp:Label>
    <table>
    <tr><td>Benutzername</td><td><asp:TextBox ID="tbUserName" runat="server" Width="100%"></asp:TextBox></td><td>Dieser Name wird bei deinen Einträgen angezeigt. Außerdem meldest du dich auch damit an.</td></tr>
    <tr><td>Darstellung</td><td>
        <asp:RadioButtonList ID="rblDisplayPref" runat="server">
            <asp:ListItem Text="Chronologisch (so wie früher)" Value="0"></asp:ListItem>
            <asp:ListItem Text="Chronologisch (letzte Seite geöffnet)" Value="1"></asp:ListItem>
            <asp:ListItem Text="Chat-Modus: Neueste Nachricht oben" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
        </td><td>Bestimmt die Darstellung der Diskussions-Threads zu den Treffen (im Terminkalender)</td></tr>
    <tr><td>Emailadresse</td><td><asp:TextBox ID="tbEmail" runat="server" Width="100%"></asp:TextBox></td><td>Die Email-Adresse wird außer zu den unten angegebenen Sachen auch noch verwendet, wenn du dein Passwort vergessen hast</td></tr>
    <!--tr><td>Emails</td><td>
        <asp:CheckBoxList ID="cbEmailPref" runat="server" RepeatLayout="Flow">
            <asp:ListItem Value="1">T&#228;gliche &#196;nderungen</asp:ListItem>
            <asp:ListItem Value="2">Meldung f&#252;r unbest&#228;tigte Treffen</asp:ListItem>
        </asp:CheckBoxList></td><td>"Tägliche Änderungen" ist eine Mail, die jede Nacht verschickt wird, wenn sich ein Termin oder im Forum was geändert hat.
        <br />Bei "Meldung für unbestätigte Treffen" wird eine Mail drei Tage vor einem Termin verschickt, wenn du noch nicht angegeben hast, ob du Zeit hast.</td></tr-->
    <tr><td>Passwort</td><td><a href="changepassword.aspx">Passwort ändern</a></td><td>Wenn du dein Passwort änderst, solltest du vorher auf Speichern drücken, soweit du Änderungen an dieser Seite gemacht hast! Ansonsten musst du deine Änderungen nochmal eintragen.</td></tr>
    <tr><td></td><td><asp:Button ID="btnSave" runat="server" Text="Speichern" OnClick="btnSave_Click" /></td><td>Drück hier drauf, wenn du mit den Änderungen fertig bist, erst dann werden sie übernommen.</td></tr>
    </table>
</asp:Content>

