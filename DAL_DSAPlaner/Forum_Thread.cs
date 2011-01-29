using System;
using System.Data;
using System.Linq;

public class Forum_Thread : DBObject, IChangeListable, IReadable
{
#region Database Properties
    private string _title;
    public string title
    {
        get
        {
            return _title;
        }
    }

    private int _groupID;
    public int groupID
    {
        get
        {
            return _groupID;
        }
    }

    public Forum_Group group
    {
        get
        {
            return (Forum_Group)DBObjectFactory.getObjectByID(groupID, DBObject.ObjType.Forum_Group);
        }
    }

    private Forum_Message[] _messages;
    public Forum_Message[] messages
    {
        get
        {
            if (_messages == null)
                _messages = (Forum_Message[])getAssociated(ObjType.Forum_Message, "ID_thread");

            return _messages;
        }
    }

    private int _modifierID = -1;
    public int modifierID
    {
        get
        {
            if (_modifierID == -1)
                calculateModifier();

            return _modifierID;
        }
    }

    public Person modifier
    {
        get
        {
            return (Person)DBObjectFactory.getObjectByID(modifierID, DBObject.ObjType.Person);
        }
    }

    private System.Collections.Generic.Dictionary<int, int> _readerIDs;
    public System.Collections.Generic.Dictionary<int, int> readerIDs
    {
        get
        {
            if (_readerIDs == null)
            {
                DataTable dtIDs = DB_DAL.getTable(
                    "SELECT ID_person FROM rel_persons_threads WHERE ID_thread=" + dbID.ToString());
                _readerIDs = new System.Collections.Generic.Dictionary<int, int>(10);
                foreach (DataRow drReader in dtIDs.Rows)
                    _readerIDs.Add(Convert.ToInt32(drReader["ID_person"]), 0);
            }

            return _readerIDs;
        }
    }
#endregion

    public const int messagesPerPage = 15;
    public int pageCount
    {
        get
        {
            if (0 == messages.Length)
                return 0;
            return (messages.Length - 1) / messagesPerPage + 1;
        }
    }
    public Forum_Message[] getMessagePage(int pageNum)
    {
        int pageSize;       // number of messages on this page

        if (pageNum > pageCount - 1|| pageNum < 0)     // out of range
            return null;

        if (pageNum == pageCount - 1)  // last page?
        {
            pageSize = messages.Length % messagesPerPage;
            if (pageSize == 0)
                pageSize = messagesPerPage;
        }
        else
            pageSize = messagesPerPage;

        Forum_Message[] retMsg = new Forum_Message[pageSize];

        for (int i = 0; i < pageSize; ++i)
            retMsg[i] = messages[i + pageNum * messagesPerPage];

        return retMsg;
    }

    public bool isReader(Person forum_user)
    {
        return readerIDs.ContainsKey(forum_user.dbID);
    }

    public void markAsRead(Person forum_user)
    {
        markAsRead(forum_user.dbID);
    }

    private void markAsRead(int forum_user_id)
    {
        if (!readerIDs.ContainsKey(forum_user_id))
        {
            readerIDs.Add(forum_user_id, 0);
            try
            {
                DB_DAL.execScalar("INSERT INTO rel_persons_threads(ID_thread,ID_person)"
                   + " VALUES(" + dbID.ToString() + "," + forum_user_id.ToString() + ")");
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                if (sqlEx.Message.IndexOf("PK_rel_persons_threads") == -1)
                    throw;
            }
        }
    }

    private void calculateModifier()
    {
        DateTime lastModified = DateTime.MinValue;
        foreach (Forum_Message message in messages)
            if (message.datModified.CompareTo(lastModified) >= 0)
            {
                _modifierID = message.creatorID;
                lastModified = message.datModified;
            }

        if (_modifierID == -1)
            _modifierID = creatorID;
    }

    /// <summary>
    /// This function is for Eval, which shall return the bound meeting in forum.aspx
    /// </summary>
    public Forum_Thread thisThread
    {
        get
        {
            return this;
        }
    }

    private Forum_Thread(DataRow drThread)
	{
        setBaseAttributes(drThread);

        _title = drThread["Title"].ToString();
        _groupID = Convert.ToInt32(drThread["ID_group"]);
    }

#region Creator methods

    public static Forum_Thread getInstanceByDataRow(DataRow drThread)
    {
        return new Forum_Thread(drThread);
    }

    public static Forum_Thread[] getAll()
    {
        return (Forum_Thread[])DBObjectFactory.getAllObjects(ObjType.Forum_Thread);
    }

    public static Forum_Thread[] getAllByGroup(int IDGroup)
    {
        Forum_Group fg = (Forum_Group)DBObjectFactory.getObjectByID(IDGroup, ObjType.Forum_Group);
        return fg.threads;
    }

    public static DataTable getAllByGroupDT(int IDGroup)
    {
        Forum_Group fg = (Forum_Group)DBObjectFactory.getObjectByID(IDGroup, ObjType.Forum_Group);
        DataTable dtRet = list2DataTable(fg.threads,typeof(Forum_Thread));
        dtRet = referenceInDT(dtRet, "messages", "Length");
        dtRet = referenceInDT(dtRet, "modifier", "userName");
        return dtRet;
    }

    public new void updateModified()
    {
        base.updateModified();
        Meeting[] metAsso = (Meeting[])getAssociated(ObjType.Meeting, "ID_thread");
        foreach (Meeting met in metAsso)    // Maximum is 1, but you never know...
            met.updateModified();
        group.updateModified();

        readerIDs.Clear();
        DB_DAL.execScalar("DELETE FROM rel_persons_threads WHERE ID_thread="
            + dbID.ToString());
    }
#endregion

    #region DB changing methods
    public void addMessage(string description, int creatorID)
    {
        if (messages
                .Where((message, index) => index + 3 >= messages.Length)   // Only the last 3 messages shall be checked
                .Any(message => creatorID == message.creatorID && description == message.description))
            return;     // Probably pressed F5, in which case the same message is posted again
        Forum_Message.insert(description, dbID, creatorID);
        updateModified();

        markAsRead(creatorID);
    }

    public static int insert(string title, int groupID, int creatorID)
    {
        string sQuery = "INSERT INTO forum_threads(Title,ID_Group,ID_Creator)"
    + " VALUES('" + title.Replace("'", "''") + "'"
    + "," + groupID.ToString()
    + "," + creatorID.ToString() + ");SELECT @@IDENTITY";

        return Convert.ToInt32(DB_DAL.execScalar(sQuery));
    }

    public static void delete(int dbID)
    {
        DB_DAL.execScalar("DELETE FROM forum_threads WHERE ID = " + dbID.ToString());
    }        

    #endregion

    #region IChangeListable Members

    public string getChangeEntry()
    {
        return modifier.userName + " hat einen Eintrag im Thread \"" + title +
            "\" gemacht. Direkter Link: " + Consts.DSAPlanerURL
            + "threads.aspx?id=" + dbID.ToString();
    }

    #endregion
}
