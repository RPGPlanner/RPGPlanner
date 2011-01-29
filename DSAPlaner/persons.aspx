<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="persons" Title="DSA Planer - Benutzer" Codebehind="persons.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    <p>Das Email-Adress-Feld existiert, weil man so wieder in's System kommt, wenn man sein Passwort vergessen hat. Terminerinnerungen, Änderungsbenachrichtigungen und sowas soll auch noch kommen (Siehe <a href="changelog.aspx">ChangeLog</a>).</p>
    <asp:GridView ID="gvPersons" runat="server" AutoGenerateColumns="False" DataSourceID="dsPerson" DataKeyNames="dbID" AllowSorting="True">
    <RowStyle CssClass="personRow" />
    <EditRowStyle CssClass="personEditRow editRow" />
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                Visible="False" />
            <asp:BoundField DataField="dbID" HeaderText="ID" ReadOnly="True" InsertVisible="False" Visible="False" />
            <asp:BoundField DataField="userName" HeaderText="Name" SortExpression="userName" />
            <asp:BoundField DataField="email" HeaderText="Email-Adresse" SortExpression="email" />
            <asp:BoundField DataField="lastLogin" HeaderText="Letze Anmeldung" ReadOnly="True" SortExpression="lastLogin" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="dsPerson" runat="server" SelectMethod="getAllDT" TypeName="Person" UpdateMethod="update" DeleteMethod="delete">
        <UpdateParameters>
            <asp:Parameter Name="dbID" Type="Int32" />
            <asp:Parameter Name="userName" Type="String" />
            <asp:Parameter Name="email" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="dbID" Type="Int32" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    <div runat="server" id="newUser" visible="false">
    <h3>Neuer Nutzer</h3>
        <asp:Label ID="lbAnswer" runat="server" CssClass="AnswerLabel"></asp:Label>
        <table>
            <tr>
                <td>Name:
                </td>
                <td>
                    <asp:TextBox ID="tbName" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Email:</td>
                <td>
                    <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Passwort:</td>
                <td>
                    <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" Width="100%"></asp:TextBox></td>
            </tr>
        </table>
    <br />
    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Neuen Benutzer anlegen" />
    </div>
</asp:Content>

