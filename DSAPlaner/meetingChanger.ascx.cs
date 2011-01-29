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

public partial class meetingChanger : System.Web.UI.UserControl
{
    public Person CurrentUser
    {
        get
        {
            return ((GenericDSAPage)Page).CurrentUser;
        }
    }

    public Meeting _metCurrent;
    public Meeting metCurrent
    {
        get
        {
            if (_metCurrent == null)
                _metCurrent = (Meeting)DBObjectFactory.getObjectByID(
                    Convert.ToInt32(hdMeetingID.Value), DBObject.ObjType.Meeting);

            return _metCurrent;
        }
        set
        {
            _metCurrent = value;
        }
    }

    protected string status2string(object oStatus)
    {
        MeetingPerson.Status status = (MeetingPerson.Status)oStatus;
        switch (status)
        {
            case MeetingPerson.Status.perhaps:
                return "Weiﬂ noch nicht";
            case MeetingPerson.Status.accept:
                return "Hat Zeit";
            case MeetingPerson.Status.atHome:
                return "Raum ist frei";
            case MeetingPerson.Status.decline:
                return "Keine Zeit";
            case MeetingPerson.Status.unknown:
                return "Unbekannt";
            default:
                throw new ArgumentOutOfRangeException("oStatus", oStatus, "Unknown person's status");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnDescription_Click(object sender, EventArgs e)
    {
        //metCurrent.description += "\n" +
        //    "//=== " + CurrentUser.userName + " ===\n|| " +
        //    tbDescription.Text.Replace("\n","\n||") +
        //    "\n\\\\===";

        Parent.Parent.DataBind();   // Parent.Parent = Repeater
    }

    protected void gvPersons_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.DataItem != null)
            e.Row.CssClass = "MeetingStatus_" + ((MeetingPerson)e.Row.DataItem).status.ToString();
    }
    
    protected void btnStatus_Command(object sender, CommandEventArgs e)
    {
        MeetingPerson.update(metCurrent.dbID, CurrentUser.dbID, Convert.ToInt32(e.CommandArgument));

        gvPersons.DataBind();      // Parent.Parent = Repeater
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        CheckBox cbDelete = (CheckBox)dvMeeting.FindControl("cbDelete");
        if (cbDelete.Checked)
        {
            metCurrent.delete();

            Parent.Parent.DataBind();      // Parent.Parent = Repeater
        }
    }
}
