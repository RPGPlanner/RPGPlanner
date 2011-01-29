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

public partial class personalData : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lbAnswer.Text = "";

        if (!IsPostBack)
        {
            tbUserName.Text = CurrentUser.userName;
            tbEmail.Text = CurrentUser.email;
            rblDisplayPref.SelectedValue = CurrentUser.displayPref.ToString();

            //foreach (ListItem li in cbEmailPref.Items)
            //    li.Selected = (int.Parse(li.Value) & CurrentUser.emailPref) == int.Parse(li.Value);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int newEmailPref = 0;
        //foreach (ListItem li in cbEmailPref.Items)
        //    if (li.Selected)
        //        newEmailPref |= Convert.ToInt32(li.Value);

        CurrentUser.update (tbUserName.Text, tbEmail.Text, CurrentUser.rights,
            newEmailPref, int.Parse(rblDisplayPref.SelectedValue));

        lbAnswer.Text = "Daten gespeichert!";
    }
}
