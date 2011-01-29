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

public partial class persons : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["delete"] != null)
            ((CommandField)gvPersons.Columns[0]).Visible = Request.QueryString["delete"] == "true";

        lbAnswer.Text = "";
        newUser.Visible = CurrentUser.fIsAdmin;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(tbName.Text) && !string.IsNullOrEmpty(tbPassword.Text))
        {
            Person.insert(tbName.Text, tbEmail.Text, tbPassword.Text,CurrentUser.dbID);
            gvPersons.DataBind();
        }
        else
            lbAnswer.Text = "Neuer Nutzer kann nicht angelegt werden: Name- oder Passwortfeld ist leer.";
    }
}
