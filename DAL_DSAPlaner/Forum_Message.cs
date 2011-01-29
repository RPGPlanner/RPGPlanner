using System;
using System.Data;

public class Forum_Message : DBObject
{
#region Database Properties
    private string _description;
    public string description
    {
        get
        {
            return _description;
        }
        set
        {
            updateAttribute("description", value, DbType.String);
            thread.updateModified();
            _description = value;
        }
    }

    public string HTMLdescription
    {
        get
        {
            string htmlRet = description.Replace("\n","<br />\n");
            htmlRet = System.Text.RegularExpressions.Regex.Replace(htmlRet, 
                @"(?<link>https?://[^ \)<$]+)", "<a target=\"_blank\" href=\"${link}\">${link}</a>");
            return htmlRet;
        }
    }

    public string editRemark
    {
        get
        {
            if (datModified != datCreated)
                return "Geändert: " + datModified.ToString();
            else
                return string.Empty;
        }
    }

    private int _threadID;
    public int threadID
    {
        get
        {
            return _threadID;
        }
    }

    public Forum_Thread thread
    {
        get
        {
            return (Forum_Thread)DBObjectFactory.getObjectByID(threadID, DBObject.ObjType.Forum_Thread);
        }
    }
#endregion

    private Forum_Message(DataRow drMessage)
	{
        setBaseAttributes(drMessage);

        _description = drMessage["Description"].ToString();
        _threadID = Convert.ToInt32(drMessage["ID_thread"]);
    }

#region Creator methods
    public static Forum_Message getInstanceByDataRow(DataRow drMessage)
    {
        return new Forum_Message(drMessage);
    }

    public static Forum_Message[] getAll()
    {
        return (Forum_Message[])DBObjectFactory.getAllObjects(ObjType.Forum_Message);
    }

    public static Forum_Message[] getAllByThread(int IDThread)
    {
        Forum_Thread ft = (Forum_Thread)DBObjectFactory.getObjectByID(IDThread, ObjType.Forum_Thread);
        return ft.messages;
    }
#endregion

#region DB changing methods
    public static void insert(string description, int threadID, int creatorID)
    {
        string sQuery = "INSERT INTO forum_messages(Description,ID_Thread,ID_Creator)"
            + " VALUES('" + description.Replace("'", "''") + "'"
            + "," + threadID.ToString()
            + "," + creatorID.ToString() + ")";
        
        DB_DAL.execScalar(sQuery);
   }
#endregion
}
