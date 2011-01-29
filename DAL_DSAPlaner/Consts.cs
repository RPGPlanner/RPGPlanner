using System;
using System.Configuration;

public class Consts
{
    private Consts()        // just static methods
    {
    }

    private static string getConfig(string configName)
    {
        return ConfigurationManager.AppSettings[configName];
    }

#region web.config
    public static string Mail_SMTPHost {
        get {
            return getConfig("Mail_SMTPHost"); 
        } 
    }

    public static int Mail_SMTPPort
    {
        get
        {
            return Convert.ToInt32(getConfig("Mail_SMTPPort"));
        }
    }

    public static string Mail_ReplyTo {
        get {
            return getConfig("Mail_ReplyTo");
        } 
    }
    public static string Mail_From {
        get {
            return getConfig("Mail_From");
        } 
    }

    public static string Mail_User {
        get {
            return getConfig("Mail_User");
        } 
    }

    public static string Mail_PW {
        get {
            return getConfig("Mail_PW");
        } 
    }

    public static string DSAPlanerURL {
        get {
            return getConfig("DSAPlanerURL");
        } 
    }

    public static int Sys_GroupID_Meetings {
         get {
            return Convert.ToInt32(getConfig("Sys_GroupID_Meetings"));
        } 
    }

    public static int Sys_GroupID_Adventures
    {
        get
        {
            return Convert.ToInt32(getConfig("Sys_GroupID_Adventures"));
        }
    }

    public static int Sys_AdventureID_Default
    {
        get
        {
            return Convert.ToInt32(getConfig("Sys_AdventureID_Default"));
        }
    }    

    public static int URL_SecurePort
    {
        get
        {
            return Convert.ToInt32(getConfig("URL_SecurePort"));
        }
    }

    public static bool URL_RequireSSL
    {
        get
        {
            return Convert.ToBoolean(getConfig("URL_RequireSSL"));
        }
    }

    public static string DB_GetDate
    {
        get
        {
            return getConfig("DB_GetDate");
        }
    }

#endregion
}
