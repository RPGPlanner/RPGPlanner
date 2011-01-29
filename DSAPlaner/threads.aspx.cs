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

public partial class threads : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sthreadID = Request.QueryString["id"];

        if (string.IsNullOrEmpty(sthreadID))
        {
            Response.Status = "404 Not Found";
            Response.End();
        }

        Forum_Thread ft = (Forum_Thread)DBObjectFactory.getObjectByID(
            Convert.ToInt32(sthreadID), DBObject.ObjType.Forum_Thread);

        dispThread.thread = ft;
        lbTitle.Text = ft.title;
        hlGruppe.Text = ft.group.title;
        hlGruppe.NavigateUrl = "forum.aspx?id=" + ft.group.dbID.ToString();

        if (!IsPostBack)
        {
            string sPageNum = Request.QueryString["page"];
            if (!string.IsNullOrEmpty(sPageNum))
                dispThread.currentPageNum = int.Parse(sPageNum);
        }
    }
}
