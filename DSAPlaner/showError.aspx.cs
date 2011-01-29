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

public partial class showError : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        object oID = Request.QueryString["ErrorID"];
        if (oID == null)
        {
            Response.Status = "404 Not Found";
            Response.End();
        }

        int dbID = Convert.ToInt32(oID);

        LogEntry log = (LogEntry)DBObjectFactory.getObjectByID(dbID, DBObject.ObjType.LogEntry);

        lbMeldung.Text = log.description;
    }
}
