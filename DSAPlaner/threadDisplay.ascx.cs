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

public partial class threadDisplay : System.Web.UI.UserControl
{
    const string BTN_DESCRIPTION_ADD = "Antwort eintragen";
    const string BTN_DESCRIPTION_EDT = "Änderung speichern";

    public Person CurrentUser
    {
        get
        {
            return ((GenericDSAPage)Page).CurrentUser;
        }
    }

    protected int _currentPageNum = -1;
    public int currentPageNum
    {
        get
        {
            if (-1 == _currentPageNum)
                if (!string.IsNullOrEmpty(hdPageNum.Value))
                    _currentPageNum = int.Parse(hdPageNum.Value);
                else if (fFirstPageFirst)
                    _currentPageNum = 0;
                else
                    _currentPageNum = thread.pageCount - 1;
                

            return _currentPageNum;
        }
        set
        {
            _currentPageNum = value;
            hdPageNum.Value = value.ToString();
            reBindDL();
        }
    }

    protected Forum_Thread _thread;
    public Forum_Thread thread
    {
        get
        {
            if (_thread == null)
                _thread = (Forum_Thread)DBObjectFactory.getObjectByID(
                    Convert.ToInt32(hdThreadID.Value), DBObject.ObjType.Forum_Thread);

            return _thread;
        }
        set
        {
            _thread = value;
            hdThreadID.Value = value.dbID.ToString();
            reBindDL();
        }
    }

    /// <summary>
    /// Determines which flag in CurrentUser.displayPref is used to determine the value of fFirstPageFirst
    /// </summary>
    public int iPagingPreferenceFlag { get; set; }

    /// <summary>
    /// If this control loads for the first time, the chronologically
    /// first is displayed if this is true, and the last page otherwise
    /// </summary>
    private bool fFirstPageFirst { get { return (CurrentUser.displayPref & iPagingPreferenceFlag) != iPagingPreferenceFlag; } }

    private bool fFirstMessageFirst { get { return !CurrentUser.fMeetingThreadReversedPageOrder; } }

    //void threadDisplay_Command(object sender, CommandEventArgs e)
    //{
    //    currentPageNum = Convert.ToInt32(e.CommandArgument);
    //}

    private void reBindDL()
    {
        IEnumerable<Forum_Message> messages = thread.getMessagePage(currentPageNum);
        if (null != messages && !fFirstMessageFirst)
            messages = messages.Reverse();
        dlMessages.DataSource = messages;
        dlMessages.DataBind();
        if (thread.pageCount > 1)
        {
            dvPaging1.Visible = true;
            dvPaging2.Visible = true;

            ListItem[] pages = new ListItem[thread.pageCount];
            for (int i = 0; i < thread.pageCount; ++i)
                pages[i] = new ListItem((i + 1).ToString(), i.ToString());
            DataList[] phaPages = new DataList[] { dlPaging1, dlPaging2 };
            foreach (DataList dlCurrent in phaPages)
            {
                dlCurrent.DataSource = pages;
                dlCurrent.EditItemIndex = currentPageNum;
                dlCurrent.DataBind();
            }
            //{
            //    phPage.Controls.Clear();
            //    phPage.Visible = true;
            //    for (int i = 0; i < thread.pageCount; ++i)
            //    {
            //        Control ctrlCurrentPage;
            //        if (i == currentPageNum)
            //        {
            //            ctrlCurrentPage = new Label();
            //            ((Label)ctrlCurrentPage).Text = (i + 1).ToString();
            //            ((Label)ctrlCurrentPage).CssClass = "pagingCtrl";
            //        }
            //        else
            //        {
            //            ctrlCurrentPage = new LinkButton();
            //            ((LinkButton)ctrlCurrentPage).CommandArgument = i.ToString();
            //            ((LinkButton)ctrlCurrentPage).Text = (i + 1).ToString();
            //            ((LinkButton)ctrlCurrentPage).Command += new CommandEventHandler(threadDisplay_Command);
            //            ((LinkButton)ctrlCurrentPage).CssClass = "pagingCtrl";
            //        }
            //        phPage.Controls.Add(ctrlCurrentPage);
            //    }
            //}
        }
        else
        {
            dvPaging1.Visible = false;
            dvPaging2.Visible = false;
            //phPages1.Visible = false;
            //pgPages2.Visible = false;
        }

        btnCancel.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PreRender += new EventHandler(threadDisplay_PreRender);
    }

    void threadDisplay_PreRender(object sender, EventArgs e)
    {
        thread.markAsRead(CurrentUser);
    }

    protected void dlMessages_EditCommand(object source, CommandEventArgs e)
    {
        btnDescription.CommandArgument = e.CommandArgument.ToString();
        tbDescription.Text = ((Forum_Message)DBObjectFactory.getObjectByID(
            Convert.ToInt32(e.CommandArgument),DBObject.ObjType.Forum_Message)).description;

        btnCancel.Visible = true;
        btnDescription.Text = BTN_DESCRIPTION_EDT;
    }

    protected void btnDescription_Command(object sender, CommandEventArgs e)
    {
        if (string.IsNullOrEmpty(tbDescription.Text))
            return;

        int editTarget = Convert.ToInt32(e.CommandArgument);
        if (editTarget == -1)
            thread.addMessage(tbDescription.Text, CurrentUser.dbID);
        else
        {
            Forum_Message msg = (Forum_Message)DBObjectFactory.getObjectByID(editTarget,
                DBObject.ObjType.Forum_Message);
            if (msg.creatorID != CurrentUser.dbID)
            {
                LogEntry.insert("Message #" + msg.dbID.ToString() + " was tried to be edited",
                    "", 1, CurrentUser.dbID);
                return;
            }
            msg.description = tbDescription.Text;
            msg.thread.markAsRead(CurrentUser);
        }

        _thread = null;
        reBindDL();
        tbDescription.Text = string.Empty;
        btnDescription.Text = BTN_DESCRIPTION_ADD;
        btnDescription.CommandArgument = "-1";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnDescription.CommandArgument = "-1";
        btnDescription.Text = BTN_DESCRIPTION_ADD;
        tbDescription.Text = string.Empty;
        btnCancel.Visible = false;
    }

    protected void dlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (string.IsNullOrEmpty(e.CommandArgument.ToString()))
            currentPageNum = e.Item.ItemIndex;
        else
            currentPageNum = Convert.ToInt32(e.CommandArgument);
    }
}
