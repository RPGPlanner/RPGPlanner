<%@ Page Language="C#" AutoEventWireup="true" Inherits="login" Codebehind="login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>DSA Planer - Login</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Login ID="lgLogin" runat="server" FailureText="Da ist was falsch. Wenn du's nicht hinbekommst, frag den Christoph"
            OnAuthenticate="lgLogin_Authenticate" PasswordLabelText="Passwort:" PasswordRequiredErrorMessage="Du hast kein Passwort eingegeben"
            RememberMeText="Nächstes Mal automatisch einloggen" TitleText="DSA Planer Anmeldung"
            UserNameLabelText="Dein Name:" UserNameRequiredErrorMessage="Du hast keinen Namen eingegeben" DestinationPageUrl="meetings.aspx" PasswordRecoveryText="Ich hab mein Passwort vergessen" PasswordRecoveryUrl="~/PasswordRecovery.aspx">
        </asp:Login>
     </div>
    </form>
</body>
</html>
