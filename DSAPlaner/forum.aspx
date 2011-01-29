<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="forum" Title="DSA Planer - Forum" Codebehind="forum.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    <div class="forum_groups">
    <h3>Gruppen</h3>
    <asp:DataList ID="dlGroups" runat="server" CssClass="groupNavi" DataSourceID="dsGroups" RepeatLayout="Flow" DataKeyField="dbID" RepeatDirection="Horizontal">
        <SelectedItemStyle CssClass="forum_selectedGroup" />
        <ItemTemplate>
            <span class="forum_navGroup" onclick='self.location="forum.aspx?id=<%# Eval("dbID") %>";'>
                <h4><%# Eval("title") %></h4>
                <%# evalPicImgString((Forum_Group)Eval("thisGroup"),CurrentUser) %>
                <div class="group_Modifier"><h5>Letzer Beitrag:</h5> <%# Eval("datModified") %> von <%# Eval("modifier.userName") %></div>
                <div class="group_Threads"><h5>Anzahl Threads:</h5> <%# Eval("threads.Length") %></div>
            </span>
        </ItemTemplate>
    </asp:DataList>
        <br />
    <asp:ObjectDataSource ID="dsGroups" runat="server" SelectMethod="getPublic" TypeName="Forum_Group" />
    </div>
    <div class="forum_threads">
    <h3>Threads</h3>
    <asp:GridView ID="gvGroup" runat="server" AllowPaging="True"
        AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="dbID" DataSourceID="dsThreads" OnRowDataBound="gvGroup_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="Neu?">
            <ItemTemplate>
            <%# evalPicImgString((Forum_Thread)Eval("thisThread"),CurrentUser) %>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Titel" SortExpression="title">
                <ItemTemplate>
                    <asp:HyperLink ID="hlMain" runat="server" NavigateUrl='<%# Eval("dbID", "threads.aspx?id={0}") %>'
                        Text='<%# Eval("title") %>'></asp:HyperLink>
                    <asp:DataList ID="dlPaging" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <HeaderTemplate>(&nbsp;</HeaderTemplate>
                    <ItemTemplate><asp:HyperLink ID="hlPage" runat="server" NavigateUrl='<%# Eval("Value", "threads.aspx?{0}") %>'
                        Text='<%# Eval("Text") %>'></asp:HyperLink>&nbsp;</ItemTemplate>
                    <FooterTemplate>)</FooterTemplate>
                    </asp:DataList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="datCreated" HeaderText="Erstellt am" SortExpression="datCreated" />
            <asp:BoundField DataField="creator.userName" HeaderText="Erstellt von" SortExpression="creator.userName" />
            <asp:BoundField DataField="messages.Length" HeaderText="Nachrichten" SortExpression="messages.Length" />
            <asp:BoundField DataField="datModified" HeaderText="Letzte Nachricht" SortExpression="datModified" />
            <asp:BoundField DataField="modifier.userName" HeaderText="Letzter Beitrag" SortExpression="modifier.userName" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="dsThreads" runat="server" SelectMethod="getAllByGroupDT"
        TypeName="Forum_Thread">
        <SelectParameters>
            <asp:ControlParameter ControlID="dlGroups" Name="IDGroup" PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <div class="newItem" runat="server" id="newItem">
    <h4>Neuer Thread</h4>
    Titel:
    <asp:TextBox ID="tbThreadTitle" runat="server"></asp:TextBox><br />
    <asp:Button ID="btnNewThread" runat="server" Text="Neuen Thread anlegen" OnClick="btnNewThread_Click" />
    </div>
    </div>
</asp:Content>

