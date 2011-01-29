<%@ Page Language="C#" AutoEventWireup="true" Inherits="PasswordRecovery" Codebehind="PasswordRecovery.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lbAnswer" runat="server">Hier kannst du dir ein neues Passwort zuschicken lassen.</asp:Label><br />
        <br />
        <asp:Label ID="lbName" runat="server" Text="Benutzername: "></asp:Label>
        <asp:TextBox ID="tbName" runat="server"></asp:TextBox><br />
        <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" Text="Ich will ein neues Passwort" /><br />
        <br />
        <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/login.aspx">Zur Login-Seite</asp:HyperLink></div>
    </form>
</body>
</html>
