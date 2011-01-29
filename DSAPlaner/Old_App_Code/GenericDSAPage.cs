using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class GenericDSAPage : Page
{
    private Person _CurrentUser;
    public Person CurrentUser
    {
        get
        {
            if (_CurrentUser == null)
            {
               _CurrentUser = (Person)Session["CurrentUser"];
               if (_CurrentUser == null && User != null)
                {
                    CurrentUser = Person.getInstanceByName(User.Identity.Name);
                    _CurrentUser.updateLastLogin();
                }
            }

            return _CurrentUser;
        }
        set
        {
            _CurrentUser = value;
            Session["CurrentUser"] = value;
        }
    }

	public GenericDSAPage()
	{
	}
}
