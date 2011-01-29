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

public partial class ErrorPage : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();

        do
        {
            if (ex is System.Web.HttpUnhandledException)
                ex = ex.InnerException;
        }
        while (ex is System.Web.HttpUnhandledException);

        if (ex == null)
        {
            Response.Status = "404 Not Found";
            Response.End();
        }

        displayException(ex);

        try
        {
            LogEntry.insert(ex.Message, lbError.Text, CurrentUser.dbID);
        }
        catch
        {
            lbError.Text += "<br>... and this error couldn't be logged, too!";
        }
    }

    protected void displayException(Exception ex)
    {
        lbError.Text += "\n<h2>" + ex.Message + "</h2>";
        lbError.Text += "\n<p class=\"error\">";
        lbError.Text += "\n<h4>Typ</h4>" + ex.GetType().ToString();
        lbError.Text += "\n<h4>Source</h4>" + ex.Source;
        lbError.Text += "\n<h4>StackTrace</h4>" + ex.StackTrace.Replace(" at ", "<br />");
        lbError.Text += "\n</p>\n";

        if (ex.InnerException != null)
            displayException(ex.InnerException);
    }
}
