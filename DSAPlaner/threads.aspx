<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="threads" Title="DSA Planer - Forum" Codebehind="threads.aspx.cs" %>

<%@ Register Src="threadDisplay.ascx" TagName="threadDisplay" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    <h2>
        <asp:Label ID="lbTitle" runat="server"></asp:Label></h2>
    <p>
        Gruppe:
        <asp:HyperLink ID="hlGruppe" runat="server" NavigateUrl="~/forum.aspx">[hlGruppe]</asp:HyperLink></p>
    <div class="forum_thread_display">
    <uc1:threadDisplay ID="dispThread" runat="server" iPagingPreferenceFlag="4" />
    </div>
</asp:Content>

