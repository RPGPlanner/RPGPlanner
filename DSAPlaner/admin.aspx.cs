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

public partial class admin : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        DAL_DSAPlaner.MailManager.getMailManager().sendChangeMail(DateTime.Now);
    }
}