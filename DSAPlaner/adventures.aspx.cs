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

public partial class adventures : GenericDSAPage
{
    protected string StatusNr2String(int statusNr)
    {
        switch (statusNr)
        {
            case 0: return "Nicht angefangen";
            case 1: return "Am Laufen";
            case 2: return "Abgeschlossen";
            default: 
                throw new ArgumentOutOfRangeException("statusNr", statusNr, "Unrecognized Status");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["delete"] != null)
            ((CommandField)gvAdventures.Columns[0]).ShowDeleteButton = Request.QueryString["delete"] == "true";
        
        lbAnswer.Text = "";
    }

    protected void gvAdventures_DataBound(object sender, EventArgs e)
    {
        GridView gvCurrent = (GridView)sender; // = gvAdventures

        if (gvCurrent.EditIndex >= 0)
        {
            // Calculate the edited adventure
            int advID = Convert.ToInt32(gvCurrent.DataKeys[gvCurrent.EditIndex].Value);
            Adventure advSource = (Adventure)DBObjectFactory.getObjectByID(advID, DBObject.ObjType.Adventure);

            DropDownList ddlMaster = (DropDownList)gvCurrent.Rows[gvCurrent.EditIndex].FindControl("ddlMaster");
            ddlMaster.SelectedValue = advSource.masterID.ToString();

            //RadioButtonList rlStatus = (RadioButtonList)gvAdventures.Rows[gvAdventures.EditIndex].FindControl("rlStatus");
            //rlStatus.SelectedValue = Convert.ToInt32(advSource.status).ToString();
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbAdv.Text) || string.IsNullOrEmpty(tbAdv.Text))
        {
            lbAnswer.Text = "Du hast noch gar keinen Namen für das neue Abenteuer eingegeben!";
            return;
        }

        try
        {
            Adventure.insert(tbAdv.Text, string.Empty, (Adventure.Status)Convert.ToInt32(rlStatus.SelectedValue),
                Convert.ToInt32(ddlMaster.SelectedValue), CurrentUser.dbID);
            
            tbAdv.Text = string.Empty;
        }
        catch (System.Data.SqlClient.SqlException sqlEx)
        {
            if (sqlEx.Message.IndexOf("IX_adventures") == -1)
                throw;
            else
            {
                lbAnswer.Text = "Ein Abenteuer diesen Namens gibt es schon!";
                return;
            }
        }

        gvAdventures.DataBind();
    }
}
