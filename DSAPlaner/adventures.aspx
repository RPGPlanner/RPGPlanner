<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="adventures" Title="DSA Planer - Abenteuer" Codebehind="adventures.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    <asp:ObjectDataSource ID="dsAdventures" runat="server" SelectMethod="getAllByStatusDT"
        TypeName="Adventure" UpdateMethod="update" DeleteMethod="delete">
        <UpdateParameters>
            <asp:Parameter Name="dbID" Type="Int32" />
            <asp:Parameter Name="title" Type="String" />
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="int_status" Type="Int32" />
            <asp:Parameter Name="masterID" Type="Int32" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="dbID" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlView" Name="statusFlags" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="dsPersons" runat="server" SelectMethod="getAllObjects"
        TypeName="DBObjectFactory">
        <SelectParameters>
            <asp:Parameter DefaultValue="Person" Name="type" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    Filter für den Typ der Abenteuer: 
    <asp:DropDownList ID="ddlView" runat="server" AutoPostBack="True">
        <asp:ListItem Selected="True" Value="3">Offen</asp:ListItem>
        <asp:ListItem Value="1">Nicht angefangen</asp:ListItem>
        <asp:ListItem Value="2">Am Laufen</asp:ListItem>
        <asp:ListItem Value="4">Abgeschlossen</asp:ListItem>
        <asp:ListItem Value="7">Alle</asp:ListItem>
    </asp:DropDownList><br />
    <asp:Label ID="lbAnswer" runat="server" CssClass="AnswerLabel"></asp:Label><br />
    <br />
    <asp:GridView ID="gvAdventures" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataSourceID="dsAdventures" DataKeyNames="dbID" OnDataBound="gvAdventures_DataBound" AllowSorting="True">
        <RowStyle CssClass="advRow" />
        <EditRowStyle CssClass="advEditRow editRow" />
        <Columns>
            <asp:CommandField ShowEditButton="True" CancelText="Abbrechen" DeleteText="L&#246;schen" EditText="Bearbeiten" UpdateText="Speichern" />
            <asp:BoundField DataField="dbID" HeaderText="ID" ReadOnly="True" Visible="False" />
            <asp:TemplateField HeaderText="Abenteuer" SortExpression="title">
                <ItemTemplate>
                    <asp:HyperLink ID="hlAdventure" runat="server" NavigateUrl='<%# Eval("threadID", "threads.aspx?id={0}") %>'
                        Text='<%# Eval("title") %>'></asp:HyperLink>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbTitle" runat="server" Text='<%# Bind("title") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Meister" SortExpression="master.userName">
                <ItemTemplate>
                <%# Eval("master.userName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlMaster" runat="server" DataSourceID="dsPersons" DataTextField="userName" 
                     DataValueField="dbID" SelectedValue='<%# Bind("masterID") %>'>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                <%# StatusNr2String((int)Eval("status")) %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:RadioButtonList ID="rlStatus" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow" SelectedValue='<%# Bind("int_status") %>'>
                        <asp:ListItem Value="0">Nicht angefangen</asp:ListItem>
                        <asp:ListItem Value="1">Am Laufen</asp:ListItem>
                        <asp:ListItem Value="2">Abgeschlossen</asp:ListItem>
                    </asp:RadioButtonList>&nbsp;
                </EditItemTemplate>
                <ItemStyle CssClass="statusSelectCol" />
            </asp:TemplateField>
            <asp:BoundField DataField="description" HeaderText="Kommentar" />
        </Columns>
    </asp:GridView>
    <h3>Neues Abenteuer</h3>
        <table>
            <tr>
                <td>Abenteuer:
                </td>
                <td>
                    <asp:TextBox ID="tbAdv" runat="server" Width="300px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Meister:</td>
                <td>
                    <asp:DropDownList ID="ddlMaster" runat="server" DataSourceID="dsPersons" DataTextField="userName"
                        DataValueField="dbID" Width="100%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Status:</td>
                <td>
                    <asp:RadioButtonList ID="rlStatus" runat="server">
                        <asp:ListItem Value="0" Selected="True">Nicht angefangen</asp:ListItem>
                        <asp:ListItem Value="1">Am Laufen</asp:ListItem>
                        <asp:ListItem Value="2">Abgeschlossen</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    <asp:Button ID="btnInsert" runat="server" Text="Neues Abenteuer anlegen" OnClick="btnInsert_Click" /><br />
</asp:Content>

