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

public partial class changelog : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["delete"] != null)
            ((CommandField)gvWishes.Columns[0]).ShowDeleteButton = Request.QueryString["delete"] == "true";
    }

    protected string status2string(object oStatus)
    {
        Wish.Status status = (Wish.Status)oStatus;
        switch (status)
        {
            case Wish.Status.wish:
                return "Wunsch";
            case Wish.Status.open:
                return "Offen";
            case Wish.Status.complete:
                return "Eingebaut";
            case Wish.Status.delayed:
                return "Zurückgestellt";
            default:
                throw new ArgumentOutOfRangeException("oStatus", oStatus, "Unknown wish status");
        }
    }

    protected void btnCreateWish_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(tbName.Text) && !string.IsNullOrEmpty(tbDescription.Text))
            Wish.insert(tbName.Text, tbDescription.Text, Wish.Status.wish, CurrentUser.dbID);

        gvWishes.DataBind();
    }

    protected void ddlView_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
