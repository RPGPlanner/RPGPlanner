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

public partial class DSAMaster : System.Web.UI.MasterPage
{
    protected Person CurrentUser
    {
        get
        {
            return ((GenericDSAPage)mainContent.Page).CurrentUser;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string searchString = Page.AppRelativeVirtualPath.Substring(2);
        int fIndex = lMenu.InnerHtml.IndexOf(searchString, StringComparison.OrdinalIgnoreCase);
        if (fIndex > 0)
            lMenu.InnerHtml = lMenu.InnerHtml.Insert(fIndex + searchString.Length + 1, " class=\"selectedMenu\"");
    }
}
