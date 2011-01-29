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

public partial class ChangePassword : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnChange_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbPass1.Text) || string.IsNullOrEmpty(tbPass2.Text))
        {
            lbAnswer.Text = "Bitte beide Felder ausfüllen!";
            return;
        }

        if (tbPass1.Text != tbPass2.Text)
        {
            lbAnswer.Text = "Die Passwörter stimmen nicht überein!";
            return;
        }

        CurrentUser.changePassword(tbPass1.Text);
        lbAnswer.Text = "Passwort geändert";
    }
}
