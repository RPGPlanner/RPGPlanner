using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Collections.Generic;

public partial class meetings : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (calMeetings.SelectedDate == DateTime.MinValue)
            calMeetings.VisibleDate = calMeetings.SelectedDate = DateTime.Now;

        calPreview.VisibleDate = calMeetings.VisibleDate.AddMonths(1);
    }

    private IList<Meeting> _relevantMeetings;
    protected IList<Meeting> relevantMeetings
    {
        get
        {
            if (null == _relevantMeetings)  // at most 1 week (+4d) from the month next has to be displayed
                _relevantMeetings = Meeting.getMeetingsByDateRange(
                    calMeetings.VisibleDate.AddDays(- calMeetings.VisibleDate.Day - 11),
                    calPreview.VisibleDate.AddDays(42 - calPreview.VisibleDate.Day));

            return _relevantMeetings;
        }
    }

    protected void calMeetings_DayRender(object sender, DayRenderEventArgs e)
    {
            // the selector function does only make problems if more than a whole year is displayed
        IEnumerable<Meeting> maTodayMeetings = relevantMeetings.Where(meeting => meeting.datMeeting.DayOfYear == e.Day.Date.DayOfYear); 
        foreach (Meeting thisMeeting in maTodayMeetings)
        {
            e.Cell.CssClass = "selectedMeeting";    // this is set if maTodayMeetings has at least one element
            if (thisMeeting.datModified.CompareTo(CurrentUser.lastLogin) > 0)
            {
                e.Cell.CssClass += " updatedMeeting";
                return;
            }
        }
            //e.Cell.Font.Bold = true;
    }

    protected void btnNeu_Click(object sender, EventArgs e)
    {
        DateTime time = Convert.ToDateTime(tbTime.Text);
        DateTime newDate = calMeetings.SelectedDate;

        newDate = newDate.AddTicks(time.TimeOfDay.Ticks - newDate.TimeOfDay.Ticks);

        int idMeetingNew = Meeting.insert(newDate,Consts.Sys_AdventureID_Default,CurrentUser.dbID);
        MeetingPerson.update(idMeetingNew, CurrentUser.dbID, MeetingPerson.Status.accept);

        repMeetings.DataBind();
    }

    protected void calMeetings_SelectionChanged(object sender, EventArgs e)
    {
        calMeetings.VisibleDate = calMeetings.SelectedDate;
        calPreview.VisibleDate = calMeetings.VisibleDate.AddMonths(1);
    }

    protected void calPreview_SelectionChanged(object sender, EventArgs e)
    {
        calMeetings.SelectedDate = calPreview.SelectedDate;
        calMeetings.VisibleDate = calPreview.SelectedDate;
        calPreview.VisibleDate = calMeetings.VisibleDate.AddMonths(1);
        calPreview.SelectedDate = DateTime.MinValue;
    }

    protected void calMeetings_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        calPreview.VisibleDate = calMeetings.VisibleDate.AddMonths(1);
    }

    protected void calPreview_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        calMeetings.VisibleDate = calPreview.VisibleDate.AddMonths(-1);
    }
}
