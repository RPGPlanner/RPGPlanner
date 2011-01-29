<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="changelog" Title="DSA Planer - ChangeLog" Codebehind="changelog.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    Filter für den Typ der Planungseinträge: 
    <asp:DropDownList ID="ddlView" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlView_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="3">Nicht fertig</asp:ListItem>
        <asp:ListItem Value="4">Erledigt</asp:ListItem>
        <asp:ListItem Value="8">Zurückgestellt</asp:ListItem>
        <asp:ListItem Value="15">Alles</asp:ListItem>
    </asp:DropDownList><br />
    <br />
       <asp:GridView ID="gvWishes" runat="server" AutoGenerateColumns="False" DataSourceID="dsWishes" DataKeyNames="dbID" AllowSorting="True" AllowPaging="True">
           <EditRowStyle CssClass="wisdhEditRow editRow" />
           <Columns>
               <asp:CommandField CancelText="Abbrechen" DeleteText="L&#246;schen" EditText="Bearbeiten"
                   ShowEditButton="True" UpdateText="Speichern" />
               <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" />
               <asp:BoundField DataField="description" HeaderText="Beschreibung" />
               <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <%# status2string(Eval("status")) %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:RadioButtonList ID="rlStatus" RepeatDirection="Vertical" runat="server" RepeatLayout="Flow" SelectedValue='<%# Bind("int_status") %>'>
                    <asp:ListItem Text="Wunsch" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Offen" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Eingebaut" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Zurückgestellt" Value="3"></asp:ListItem>
                </asp:RadioButtonList>
            </EditItemTemplate>
                   <ItemStyle CssClass="statusSelectCol" />
               </asp:TemplateField>
               <asp:BoundField DataField="datCreated" HeaderText="Erstellungsdatum" ReadOnly="True" SortExpression="datCreated" />
               <asp:BoundField DataField="datCompleted" HeaderText="Fertigstellung" ReadOnly="True" SortExpression="datCompleted" />
           </Columns>
        </asp:GridView>
    <asp:ObjectDataSource ID="dsWishes" runat="server" DeleteMethod="delete" SelectMethod="getAllByStatusDT"
        TypeName="Wish" UpdateMethod="update">
        <DeleteParameters>
            <asp:Parameter Name="dbID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="dbID" Type="Int32" />
            <asp:Parameter Name="name" Type="String" />
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="int_status" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlView" Name="statusflags" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
<h3>Neuer Wunsch</h3>
<table>
<tr><td>Name:</td><td>
    <asp:TextBox ID="tbName" runat="server" Columns="60"></asp:TextBox></td></tr>
<tr><td style="height: 26px">Beschreibung:</td><td style="height: 26px">
    <asp:TextBox ID="tbDescription" runat="server" Rows="4" TextMode="MultiLine" Width="100%"></asp:TextBox></td></tr>
</table>
    <asp:Button ID="btnCreateWish" runat="server" Text="Wunsch eintragen" OnClick="btnCreateWish_Click" />
</asp:Content>

