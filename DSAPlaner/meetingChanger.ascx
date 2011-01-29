<%@ Control Language="C#" AutoEventWireup="true" Inherits="meetingChanger" Codebehind="meetingChanger.ascx.cs" %>
<%@ Register Src="threadDisplay.ascx" TagName="threadDisplay" TagPrefix="uc1" %>
<div class="meetingBox">
    <asp:HiddenField ID="hdAdvID" runat="server" Value='<%# Eval("adventure.dbID") %>' />
    <asp:HiddenField ID="hdMeetingID" runat="server" Value='<%# Eval("dbID") %>' />
   <h3><%# Eval("adventure.title") %></h3>    
    <div class="meetingStatus">
<h4> Verfügbarkeit der Personen:</h4>
<asp:GridView ID="gvPersons" runat="server" AutoGenerateColumns="False" DataSourceID="dsCurrentPersons" DataKeyNames="dbID,meeting_ID" OnRowDataBound="gvPersons_RowDataBound">
        <EditRowStyle CssClass="statusEditRow editRow" />
        <Columns>
            <asp:CommandField ShowEditButton="True" CancelText=" Abbrechen" DeleteText="L&#246;schen" EditText="Bearbeiten" UpdateText="Speichern" />
            <asp:BoundField DataField="meeting_ID" HeaderText="meeting_ID" Visible="False" ReadOnly="True" />
            <asp:BoundField DataField="dbID" HeaderText="PersonID" Visible="False" ReadOnly="True" />
            <asp:BoundField DataField="userName" HeaderText="Name" ReadOnly="True" />
            <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <%# status2string(Eval("status")) %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:RadioButtonList ID="rlStatus" runat="server" RepeatDirection="vertical" RepeatLayout="Flow" SelectedValue='<%# Bind("int_status") %>'>
                    <asp:ListItem Text="Unbekannt" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Hat Zeit" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Weiß nicht" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Keine Zeit" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Raum ist frei" Value="3"></asp:ListItem>
                </asp:RadioButtonList>
            </EditItemTemplate>
                <ItemStyle CssClass="statusSelectCol" />
            </asp:TemplateField>
        </Columns>
</asp:GridView>  
<ul class="statusChooser">
<li><asp:Button ID="btnAccept" runat="server" Text="Ich hab Zeit :-)" CommandArgument="0" CommandName="changeStatus" OnCommand="btnStatus_Command" /></li>
<li><asp:Button ID="btnUnknown" runat="server" Text="Weiß noch nicht" CommandArgument="1" CommandName="changeStatus" OnCommand="btnStatus_Command" /></li>
<li><asp:Button ID="btnDecline" runat="server" Text="Leider keine Zeit :-(" CommandArgument="2" OnCommand="btnStatus_Command" /></li>
<li><asp:Button ID="btnAtHome" runat="server" Text="Wir können bei mir spielen" CommandArgument="3" CommandName="changeStatus" OnCommand="btnStatus_Command" /></li>
</ul>
<asp:ObjectDataSource ID="dsCurrentPersons" runat="server" SelectMethod="getAllByMeeting"
    TypeName="MeetingPerson" UpdateMethod="update">
    <SelectParameters>
        <asp:ControlParameter ControlID="hdMeetingID" Name="IDMeeting" PropertyName="Value"
            Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="meeting_ID" Type="Int32" />
        <asp:Parameter Name="dbID" Type="Int32" />
        <asp:Parameter Name="int_status" Type="Int32" />
    </UpdateParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="dsPossAdv" runat="server" SelectMethod="getPossibleAdventures" TypeName="Meeting">
    <SelectParameters>
        <asp:ControlParameter ControlID="hdMeetingID" Name="meetingID" PropertyName="Value"
            Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
</div>
<div class="meetingInfo">
    <h4>Informationen zum Treffen</h4>
    <asp:DetailsView ID="dvMeeting" runat="server" AutoGenerateRows="False" DataSourceID="dsCurrentMeeting" DataKeyNames="dbID">
        <Fields>
            <asp:TemplateField HeaderText="Abenteuer:">
            <ItemTemplate><%# Eval("adventure.title") %></ItemTemplate>
            <EditItemTemplate><asp:DropDownList ID="ddlChangeAdv" runat="server" DataSourceID="dsPossAdv" DataTextField="title" DataValueField="dbID" SelectedValue='<%# Bind("advID") %>'>
              </asp:DropDownList>
            </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Meister:" ReadOnly="True" DataField="adventureMaster" />
            <asp:BoundField HeaderText="Abenteuer-Kommentar:" ReadOnly="True" DataField="adventureDescription" />
            <asp:BoundField HeaderText="Uhrzeit:" DataField="datMeeting" DataFormatString="{0:t}" ApplyFormatInEditMode="True" HtmlEncode="False" />
            <asp:TemplateField HeaderText="Termin l&#246;schen">
            <ItemTemplate>
                    <asp:CheckBox ID="cbDelete" runat="server" Text="Wirklich löschen?" />
        <asp:Button
            ID="btnDelete" runat="server" Text="Löschen" OnClick="btnDelete_Click" />
            </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField CancelText="Abbrechen" EditText="Bearbeiten" ShowEditButton="True"
                UpdateText="Speichern" />
        </Fields>
    </asp:DetailsView>
    <asp:ObjectDataSource ID="dsCurrentMeeting" runat="server" SelectMethod="getInstanceByID"
        TypeName="Meeting" UpdateMethod="update">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdMeetingID" Name="ID" PropertyName="Value" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="datMeeting" Type="DateTime" />
            <asp:Parameter Name="advID" Type="Int32" />
            <asp:Parameter Name="dbID" Type="Int32" />
        </UpdateParameters>
    </asp:ObjectDataSource>
 </div>
<div class="meetingComment">
<h4>Thread zum Treffen</h4>
<uc1:threadDisplay id="tdComments" runat="server" thread='<%# Eval("thread") %>' iPagingPreferenceFlag='1' >
</uc1:threadDisplay>
</div>
</div>