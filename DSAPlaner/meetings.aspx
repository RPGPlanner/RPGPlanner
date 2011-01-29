<%@ Page Language="C#" MasterPageFile="~/DSAMaster.master" AutoEventWireup="true" Inherits="meetings" Title="DSA Planer - Termine" Codebehind="meetings.aspx.cs" %>
<%@ Register Src="~/meetingChanger.ascx" TagPrefix="dsa" TagName="meetingChanger" %>
<asp:Content ID="meetingContent" ContentPlaceHolderID="mainContent" Runat="Server">
    <div class="calendars">
    <asp:Calendar ID="calMeetings" runat="server" BackColor="White" BorderColor="#999999"
        CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
        ForeColor="Black" Height="180px" OnDayRender="calMeetings_DayRender" Width="200px" OnSelectionChanged="calMeetings_SelectionChanged" OnVisibleMonthChanged="calMeetings_VisibleMonthChanged">
        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
        <SelectorStyle BackColor="#CCCCCC" />
        <WeekendDayStyle BackColor="#FFFFCC" />
        <OtherMonthDayStyle ForeColor="Gray" />
        <NextPrevStyle VerticalAlign="Bottom" />
        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
    </asp:Calendar>
    <asp:Calendar ID="calPreview" runat="server" BackColor="White" BorderColor="#999999"
        CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
        ForeColor="Black" Height="180px" OnDayRender="calMeetings_DayRender" Width="200px" OnSelectionChanged="calPreview_SelectionChanged" OnVisibleMonthChanged="calPreview_VisibleMonthChanged">
        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
        <SelectorStyle BackColor="#CCCCCC" />
        <WeekendDayStyle BackColor="#FFFFCC" />
        <OtherMonthDayStyle ForeColor="Gray" />
        <NextPrevStyle VerticalAlign="Bottom" />
        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
    </asp:Calendar>
</div>
    <asp:ObjectDataSource ID="todayMeetings" runat="server" SelectMethod="getMeetingsByDay"
        TypeName="Meeting">
        <SelectParameters>
            <asp:ControlParameter ControlID="calMeetings" Name="datMeeting" PropertyName="SelectedDate"
                Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="dsOpenAdv" runat="server" SelectMethod="getAllOpen" TypeName="Adventure">
    </asp:ObjectDataSource>
<div class="meetings">
    <h2>Abenteuer am ausgewählten Tag</h2>
    <asp:Repeater ID="repMeetings" runat="server" DataSourceID="todayMeetings" >
    <ItemTemplate>
        <dsa:meetingChanger ID="metCh" runat="server" metCurrent='<%# Eval("thisMeeting") %>' />
    </ItemTemplate>
    </asp:Repeater>
</div>
<div class="newMeeting">
    <h2>Neuer Termin</h2>
    Uhrzeit: <asp:TextBox ID="tbTime" runat="server" Columns="5" MaxLength="5" Text="20:00"></asp:TextBox><br />
    <asp:Button ID="btnNeu" runat="server" OnClick="btnNeu_Click" Text="Termin eintragen" />
</div>
</asp:Content>

