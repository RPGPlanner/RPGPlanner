using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Code Behind file for Global.asax; stores some info about the requester in log4net
/// </summary>
public partial class Global : HttpApplication
{
    public Global()
    {
        this.BeginRequest += delegate(Object sender, EventArgs e)
            {
                if (!this.Request.IsSecureConnection && Consts.URL_RequireSSL)
                {
                    UriBuilder secureLocation = new UriBuilder(Request.Url);
                    if (-1 != secureLocation.Port || 443 != Consts.URL_SecurePort)
                        secureLocation.Port = Consts.URL_SecurePort;
                    secureLocation.Scheme = "https";
                    Response.Redirect(secureLocation.Uri.ToString(), true);
                }
            };
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        HttpContext.Current.Response.Clear();
        Server.Transfer("ErrorPage.aspx");
    }
}
