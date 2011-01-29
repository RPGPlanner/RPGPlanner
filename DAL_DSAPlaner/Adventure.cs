using System;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for Adventure
/// </summary>
public class Adventure : DBObject
{
#region Database Properties
    private string _description;
    public string description
    {
        get
        {
            return _description;
        }
    }

    private string _title;
    public string title
    {
        get
        {
            return _title;
        }
    }

    public enum Status { not_started = 0, pending = 1, complete = 2 };
    private Status _status;
    public Status status
    {
        get
        {
            return _status;
        }
    }
    public int int_status
    {
        get
        {
            return Convert.ToInt32(status);
        }
    }
    
    private int _masterID;
    public int masterID
    {
        get
        {
            return _masterID;
        }
    }

    public Person master
    {
        get
        {
            return (Person)DBObjectFactory.getObjectByID(masterID,DBObject.ObjType.Person);
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

    private Adventure(DataRow drAdv)
	{
        setBaseAttributes(drAdv);

        _description = drAdv["Description"].ToString();
        _title = drAdv["Title"].ToString();
        _status = (Status)Convert.ToInt32(drAdv["Status"]);
        if (drAdv["ID_master"] is DBNull)
            _masterID = 0;
        else
            _masterID = Convert.ToInt32(drAdv["ID_master"]);
        _threadID = Convert.ToInt32(drAdv["ID_thread"]);
    }

#region Creator methods
    public static Adventure getInstanceByID(int dbID)
    {
        return (Adventure)DBObjectFactory.getObjectByID(dbID, ObjType.Adventure);
    }

    public static Adventure getInstanceByDataRow(DataRow drAdv)
    {
        return new Adventure(drAdv);
    }

    public static Adventure[] getAll()
    {
        return getAllByStatus(7);// (Adventure[])DBObjectFactory.getAllObjects(ObjType.Adventure);
    }

    public static Adventure[] getAllOpen()
    {
        return getAllByStatus(3);
        //return (Adventure[])DBObjectFactory.getCustomObjects(Type.Adventure,
        //    " WHERE status=0 OR status=1");
    }

    public static Adventure[] getAllByStatus(int statusFlags)
    {
        string where = "";
        if (statusFlags != 7)
            where = statusFlags2whereClause(statusFlags);

        return (Adventure[])DBObjectFactory.getCustomObjects(ObjType.Adventure, where + " ORDER BY title ASC");
    }

    public static DataTable getAllByStatusDT(int statusFlags)
    {
        DataTable dtRet = list2DataTable(getAllByStatus(statusFlags), typeof(Adventure));

            // For sorting abilities
        dtRet.Columns.Add(new DataColumn("master.userName",typeof(string)));
        foreach (DataRow drAdventure in dtRet.Rows)
            drAdventure["master.userName"] = ((Person)drAdventure["master"]).userName;

        return dtRet;
    }
#endregion

#region DB changing methods
    public static void update(int dbID, string title, string description,
        int int_status,int masterID)
    {
        string sQuery = "UPDATE adventures SET Title='" + title.Replace("'", "''") + "'"
           + ",Status=" + int_status.ToString() + 
           ",ID_master=" + masterID.ToString();

        if (description == null)
            sQuery += ",Description=''";
        else
            sQuery += ",Description='" + description.Replace("'", "''") + "'";

        DB_DAL.execScalar(sQuery + " WHERE ID=" + dbID.ToString());
    }

    public static void delete(int dbID)
    {
        //    // Perhaps we don't delete the forum entries
        //DB_DAL.execScalar("DELETE FROM forum_messages WHERE ID_thread = (SELECT ID_thread FROM adventures WHERE ID=" + dbID.ToString() + ");"
        //    + "DELETE FROM forum_threads WHERE ID = (SELECT ID_thread FROM adventures WHERE ID=" + dbID.ToString() + ");"
        //    + "DELETE FROM adventures WHERE ID=" + dbID.ToString());

        DB_DAL.execScalar("DELETE FROM adventures WHERE ID=" + dbID.ToString());
    }

    public static void insert(string title, string description,
        Status status, int masterID, int creatorID)
    {
        int threadID = Forum_Thread.insert(title, Consts.Sys_GroupID_Adventures, creatorID);

        try
        {
            string sQuery = "INSERT INTO adventures(Title,Description,Status,ID_Master,ID_Creator,ID_thread)"
                + " VALUES('" + title.Replace("'", "''") + "'"
                + ",'" + description.Replace("'", "''") + "'"
                + "," + Convert.ToInt32(status).ToString()
                + "," + masterID.ToString()
                + "," + creatorID.ToString()
                + "," + threadID.ToString() + ")";

            DB_DAL.execScalar(sQuery);
        }
        catch (Exception)
        {   // inserting the adventrue failed. Better remove the forum thread, too
            Forum_Thread.delete(threadID);
            throw;
        }
    }
#endregion
}
