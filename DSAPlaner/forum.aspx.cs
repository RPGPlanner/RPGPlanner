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

public partial class forum : GenericDSAPage
{
    protected int groupID
    {
        get
        {
            if (Convert.ToInt32(dlGroups.SelectedValue) != -1)
                return Convert.ToInt32(dlGroups.SelectedValue);
            else
                return 0;
        }
    }
    protected Forum_Group group
    {
        get
        {
            return (Forum_Group)DBObjectFactory.getObjectByID(groupID, DBObject.ObjType.Forum_Group);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        object oID = Request.QueryString["id"];
        if (oID != null && !IsPostBack)
        {
            int x = dlGroups.Items.Count;   // Bug in .NET: DataKeys.Count will be 0 otherwise
            for (int i = 0; i < dlGroups.DataKeys.Count; ++i)
                if (Convert.ToInt32(dlGroups.DataKeys[i]) == Convert.ToInt32(oID))
                {
                    dlGroups.SelectedIndex = i;
                    break;
                }
        }
        if (dlGroups.SelectedIndex == -1)
            dlGroups.SelectedIndex = 0;
        newItem.Visible = (Consts.Sys_GroupID_Adventures != groupID);
        if (!IsPostBack)
            gvGroup.Sort("datModified", SortDirection.Descending);
    }

    //protected void setNewThreadVisibility()
    //{
    //    if (groupID == 0)    // Meeting-Group
    //        newItem.Visible = false;
    //    else
    //        newItem.Visible = true;
    //}

    protected string evalPicImgString(IReadable forumObj, Person user) 
    {
        if (forumObj.isReader(user))                       // read
            return "";
        else if (forumObj.isNew(user))                     // new & unread
            return "<img src='pic/GelberStern.png' />";
        else                                               // only unread
            return "<img src='pic/GruenerStern.png' />";
    }

    protected void btnNewThread_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(tbThreadTitle.Text))
        {
            group.addThread(tbThreadTitle.Text, CurrentUser.dbID);
            gvGroup.DataBind();
        }
    }
    protected void gvGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataList dlPaging = (DataList)e.Row.FindControl("dlPaging");
            DataRowView drvThread = (DataRowView)e.Row.DataItem;
            int pageCount = Convert.ToInt32(drvThread["pageCount"]);
            if (pageCount < 2)
                dlPaging.Visible = false;
            else
            {
                ListItem[] pages = new ListItem[pageCount];
                for (int i = 1; i < pageCount; ++i)
                    pages[i] = new ListItem((i + 1).ToString(), "id=" + drvThread["dbID"].ToString() + "&page=" + i.ToString());
                dlPaging.DataSource = pages;
                dlPaging.DataBind();
            }
        }
    }
}
