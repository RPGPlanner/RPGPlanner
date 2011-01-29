<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="personalData" Title="Untitled Page" Codebehind="personalData.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    <br />
    <asp:Label ID="lbAnswer" CssClass="AnswerLabel" runat="server"></asp:Label>
    <table>
    <tr><td>Benutzername</td><td><asp:TextBox ID="tbUserName" runat="server" Width="100%"></asp:TextBox></td><td>Dieser Name wird bei deinen Eintr�gen angezeigt. Au�erdem meldest du dich auch damit an.</td></tr>
    <tr><td>Darstellung</td><td>
        <asp:RadioButtonList ID="rblDisplayPref" runat="server">
            <asp:ListItem Text="Chronologisch (so wie fr�her)" Value="0"></asp:ListItem>
            <asp:ListItem Text="Chronologisch (letzte Seite ge�ffnet)" Value="1"></asp:ListItem>
            <asp:ListItem Text="Chat-Modus: Neueste Nachricht oben" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
        </td><td>Bestimmt die Darstellung der Diskussions-Threads zu den Treffen (im Terminkalender)</td></tr>
    <tr><td>Emailadresse</td><td><asp:TextBox ID="tbEmail" runat="server" Width="100%"></asp:TextBox></td><td>Die Email-Adresse wird au�er zu den unten angegebenen Sachen auch noch verwendet, wenn du dein Passwort vergessen hast</td></tr>
    <!--tr><td>Emails</td><td>
        <asp:CheckBoxList ID="cbEmailPref" runat="server" RepeatLayout="Flow">
            <asp:ListItem Value="1">T&#228;gliche &#196;nderungen</asp:ListItem>
            <asp:ListItem Value="2">Meldung f&#252;r unbest&#228;tigte Treffen</asp:ListItem>
        </asp:CheckBoxList></td><td>"T�gliche �nderungen" ist eine Mail, die jede Nacht verschickt wird, wenn sich ein Termin oder im Forum was ge�ndert hat.
        <br />Bei "Meldung f�r unbest�tigte Treffen" wird eine Mail drei Tage vor einem Termin verschickt, wenn du noch nicht angegeben hast, ob du Zeit hast.</td></tr-->
    <tr><td>Passwort</td><td><a href="changepassword.aspx">Passwort �ndern</a></td><td>Wenn du dein Passwort �nderst, solltest du vorher auf Speichern dr�cken, soweit du �nderungen an dieser Seite gemacht hast! Ansonsten musst du deine �nderungen nochmal eintragen.</td></tr>
    <tr><td></td><td><asp:Button ID="btnSave" runat="server" Text="Speichern" OnClick="btnSave_Click" /></td><td>Dr�ck hier drauf, wenn du mit den �nderungen fertig bist, erst dann werden sie �bernommen.</td></tr>
    </table>
</asp:Content>

