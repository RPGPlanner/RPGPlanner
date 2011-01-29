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

public partial class test : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lbAnswer.Text = "";
    }

    protected void btnChangeMail_Click(object sender, EventArgs e)
    {
        if (!CurrentUser.fIsAdmin)
        {
            lbAnswer.Text = "Fehlende Nutzerrechte für diese Aktion";
            return;
        }

        DAL_DSAPlaner.MailManager.getMailManager().sendChangeMail(DateTime.Now);
    }

    protected void btnRemind_Click(object sender, EventArgs e)
    {
        if (!CurrentUser.fIsAdmin)
        {
            lbAnswer.Text = "Fehlende Nutzerrechte für diese Aktion";
            return;
        }

        DAL_DSAPlaner.MailManager.getMailManager().sendRememberMail();
    }
}
