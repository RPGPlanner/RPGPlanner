<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="errors" Title="DSA Planer - Fehler" Codebehind="errors.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
<h2>Fehler</h2>
    <p>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataSourceID="dsErrors">
            <Columns>
                <asp:BoundField DataField="name" HeaderText="Fehler" SortExpression="name" />
                <asp:BoundField DataField="datCreated" HeaderText="Datum des Auftretens" SortExpression="datCreated" />
                <asp:BoundField DataField="creator.userName" HeaderText="Verursacher" SortExpression="creator.userName" />
                <asp:TemplateField HeaderText="Volltext">
                    <ItemTemplate>
                        <a href='showError.aspx?ErrorID=<%# Eval("dbID") %>'>Zeigen</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="dsErrors" runat="server" SelectMethod="getAllDT" TypeName="LogEntry">
        </asp:ObjectDataSource>
    </p>

</asp:Content>

