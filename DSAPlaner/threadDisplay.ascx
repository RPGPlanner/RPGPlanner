<%@ Control Language="C#" AutoEventWireup="true" Inherits="threadDisplay" Codebehind="threadDisplay.ascx.cs" %>
<asp:HiddenField ID="hdThreadID" runat="server" />
<asp:HiddenField ID="hdPageNum" runat="server" />
<div runat="server" id="dvPaging1">Seite: &lt;
<asp:DataList ID="dlPaging1" runat="server" OnItemCommand="dlPaging_ItemCommand" RepeatDirection="Horizontal" RepeatLayout="Flow">
<ItemTemplate><asp:LinkButton EnableViewState="false" ID="btnPage" runat="server" CommandArgument='<%# Eval("Value") %>'><%# Eval("Text") %></asp:LinkButton> </ItemTemplate>
<EditItemTemplate><%# Eval("Text") %> </EditItemTemplate>
</asp:DataList>
    &gt;</div>
<asp:DataList ID="dlMessages" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnEditCommand="dlMessages_EditCommand">
    <ItemTemplate><div class="forum_message">
        <h4><%# Eval("creator.userName") %></h4>
        <asp:LinkButton ID="lbEdit" runat="server" Visible='<%# Convert.ToInt32(Eval("creator.dbID"))==CurrentUser.dbID %>' CssClass="forum_message_edit" CommandName="Edit" CommandArgument='<%# Eval("dbID") %>'>Bearbeiten</asp:LinkButton>
        <div class="forum_message_date"><%# Eval("datCreated") %></div>
        <div class="forum_message_text"><%# Eval("HTMLdescription") %></div>
        <span class="forum_message_remark"><%# Eval("editRemark") %></span>
    </div></ItemTemplate>
</asp:DataList>
<div runat="server" id="dvPaging2">Seite: &lt;<asp:DataList ID="dlPaging2" runat="server" OnItemCommand="dlPaging_ItemCommand" RepeatDirection="Horizontal" RepeatLayout="Flow">
<ItemTemplate><asp:LinkButton EnableViewState="false" ID="btnPage" runat="server" CommandArgument='<%# Eval("Value") %>'><%# Eval("Text") %></asp:LinkButton> </ItemTemplate>
<EditItemTemplate><%# Eval("Text") %> </EditItemTemplate>
</asp:DataList>&gt;</div>
<asp:TextBox ID="tbDescription" runat="server" Rows="8" TextMode="MultiLine" CssClass="forum_message_textbox"></asp:TextBox><br />
<asp:Button ID="btnDescription" runat="server" Text="Antwort eintragen" CommandArgument="-1" OnCommand="btnDescription_Command" />&nbsp;
<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Abbrechen"
    Visible="False" />
