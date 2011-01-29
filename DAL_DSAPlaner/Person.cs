using System;
using System.Data;
using DAL_DSAPlaner;

/// <summary>
/// this class represents a real person
/// </summary>
public class Person : DBObject
{
    protected bool fLoggedIn = false;

#region Database Properties
    private string _UserName;
    public string userName
    {
        get
        {
            return _UserName;
        }
    }

    private string _Email;
    public string email
    {
        get
        {
            return _Email;
        }
    }

    private int _EmailPref;
    public int emailPref
    {
        get
        {
            return _EmailPref;
        }
    }

    private int _DisplayPref;
    public int displayPref
    {
        get
        {
            return _DisplayPref;
        }
    }

    private int _Rights;
    public int rights
    {
        get
        {
            return _Rights;
        }
    }

    private DateTime _LastLogin;
    public DateTime lastLogin
    {
        get
        {
            return _LastLogin;
        }
    }
#endregion

    public bool fDailyMail
    {
        get
        {
            return (emailPref & 1) == 1;
        }
    }

    public bool fNoClickMail
    {
        get
        {
            return (emailPref & 2) == 2;
        }
    }

    public bool fIsAdmin
    {
        get
        {
            return (rights & 1) == 1;
        }
    }

    public bool fMeetingThreadReversedMessageOrder
    {
        get
        {
            return (displayPref & 1) == 1;
        }
    }

    public bool fMeetingThreadReversedPageOrder
    {
        get
        {
            return (displayPref & 2) == 2;
        }
    }

    protected Person(DataRow drUser)
	{
        setBaseAttributes(drUser);

        _UserName = drUser["Name"].ToString();
        _Email = drUser["Email"].ToString();
        _Rights = Convert.ToInt32(drUser["Rights"]);
        _EmailPref = Convert.ToInt32(drUser["EmailPref"]);
        _DisplayPref = Convert.ToInt32(drUser["DisplayPref"]);
        if (!(drUser["datLastLogin"] is DBNull))
            _LastLogin = Convert.ToDateTime(drUser["datLastLogin"]);
    }

#region Construction methods
    public static Person getInstanceByDataRow(DataRow drUser)
    {
        return new Person(drUser);
    }

    public static Person getInstanceByName(string Name)
    {
        DataTable dtPer = DB_DAL.getTable("SELECT * FROM persons WHERE LOWER(name)='"
    + Name.ToLower().Replace("'", "''") + "'");

        if (dtPer.Rows.Count <= 0)
            return null;

        return new Person(dtPer.Rows[0]);
    }

    public static Person[] getAll()
    {
        return (Person[])DBObjectFactory.getAllObjects(ObjType.Person);
    }

    public static DataTable getAllDT()
    {
        return list2DataTable(DBObjectFactory.getAllObjects(ObjType.Person),typeof(Person));
    }
#endregion

    public bool login(string Password)
    {
        string sQuery = "SELECT Count(*) FROM persons WHERE password='"
            + CryptoCA.CryptLib.SHA1HashBase64(Password).Replace("'","''") + 
            "' AND name='" + userName.Replace("'","''") + "'";
        fLoggedIn = Convert.ToInt32(DB_DAL.execScalar(sQuery)) > 0;
        
            // save the date of the last login
        if (fLoggedIn)
            updateLastLogin();

        return fLoggedIn;
    }

    public void sendMail(string subject, string body)
    {
        if (email == string.Empty)
            return;
        else
            MailManager.getMailManager().sendMail(email, subject, body);
    }

#region DB changing methods
    public static void update(int dbID, string userName, string email, int rights, int emailPref, int displayPref)
    {
        string sQuery = "UPDATE persons SET Name='"
         + userName.Replace("'", "''") + "',Rights=" + rights.ToString()
         + ",DisplayPref=" + displayPref.ToString() + ",EmailPref=" + emailPref.ToString();

        if (email == null)
            sQuery+=",Email=''";
        else
            sQuery+=",Email='" + email.Replace("'", "''") + "'";

        DB_DAL.execScalar(sQuery + " WHERE ID=" + dbID.ToString());
    }

    public void update(string userName, string email, int rights, int emailPref, int displayPref)
    {
        update(dbID, userName, email, rights, emailPref, displayPref);
        this._UserName = userName;
        this._Email = email;
        this._Rights = rights;
        this._EmailPref = emailPref;
        this._DisplayPref = displayPref;
    }

    public void updateLastLogin()
    {
        DB_DAL.execScalar("UPDATE persons SET datLastLogin=" + Consts.DB_GetDate + " WHERE ID="
            + dbID.ToString());
    }

    public static void delete(int dbID)
    {
        DB_DAL.execScalar("DELETE FROM persons WHERE ID=" + dbID.ToString());
    }

    public static void insert(string userName, string email, string password, int creatorID)
    {
        string sQuery = "INSERT INTO persons(Name,ID_creator,Email,Password) VALUES('"
         + userName.Replace("'", "''") + "'," + creatorID.ToString();

        if (email == null)
            sQuery += ",''";
        else
            sQuery += ",'" + email.Replace("'", "''") + "'";

        sQuery += ",'" + CryptoCA.CryptLib.SHA1HashBase64(password).Replace("'", "''")
            + "')";

        DB_DAL.execScalar(sQuery);
    }

    public void changePassword(string password)
    {
        DB_DAL.execScalar("UPDATE persons SET Password='" + 
         CryptoCA.CryptLib.SHA1HashBase64(password).Replace("'","''") + "' WHERE ID=" + 
         dbID.ToString());
    }
#endregion
}
