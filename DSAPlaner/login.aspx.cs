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

public partial class login : GenericDSAPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lgLogin_Authenticate(object sender, AuthenticateEventArgs e)
    {
        Person loginPerson = Person.getInstanceByName(lgLogin.UserName);

        if (loginPerson == null)
            e.Authenticated = false;
        else
            e.Authenticated = loginPerson.login(lgLogin.Password);

        if (e.Authenticated)
            CurrentUser = loginPerson;
        else
            CurrentUser = null;
    }
}
