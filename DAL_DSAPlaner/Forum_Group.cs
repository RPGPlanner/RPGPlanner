using System;
using System.Data;

public class Forum_Group : DBObject, IReadable 
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

    private Forum_Thread[] _threads;
    public Forum_Thread[] threads
    {
        get
        {
            if (_threads == null)
                _threads = (Forum_Thread[])getAssociated(ObjType.Forum_Thread, "ID_group");

            return _threads;
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

    //private Forum_Thread _lastModifiedThread;
    //public Forum_Thread lastModifiedThread
    //{
    //    get
    //    {
    //        if (_lastModifiedThread == null)
    //        {

    //        }

    //        return _lastModifiedThread;
    //    }
    //}
#endregion

    /// <summary>
    /// This function is for Eval, which shall return the bound meeting in forum.aspx
    /// </summary>
    public Forum_Group thisGroup
    {
        get
        {
            return this;
        }
    }

    public bool isReader(Person forum_user)
    {
        bool retVal = true;
        foreach (Forum_Thread thread in threads)
            if (!(retVal &= thread.isReader(forum_user)))
                break;
        return retVal;
    }

    public void markAsRead(Person forum_user)
    {
        throw new NotImplementedException("This does not work for groups");
    }

    private void calculateModifier()
    {
        DateTime lastModified = DateTime.MinValue;
        foreach (Forum_Thread thread in threads)
            if (thread.datModified.CompareTo(lastModified) >= 0)
            {
                _modifierID = thread.modifierID;
                lastModified = thread.datModified;
            }

        if (_modifierID == -1)
            _modifierID = creatorID;
    }

    private Forum_Group(DataRow drGroup)
	{
        setBaseAttributes(drGroup);

        _title = drGroup["Title"].ToString();
    }

#region Creator methods
    public static Forum_Group getInstanceByDataRow(DataRow drGroup)
    {
        return new Forum_Group(drGroup);
    }

    public static Forum_Group[] getAll()
    {
        return (Forum_Group[])DBObjectFactory.getAllObjects(ObjType.Forum_Group);
    }

    public static Forum_Group[] getPublic()
    {
        return getCustom(" WHERE ID <> " + Consts.Sys_GroupID_Meetings.ToString());
    }

    public static Forum_Group[] getCustom(string whereClause)
    {
        return (Forum_Group[])DBObjectFactory.getCustomObjects(ObjType.Forum_Group, whereClause);
    }
#endregion

#region DB changing methods
    public void addThread(string title, int creatorID)
    {
        Forum_Thread.insert(title, dbID, creatorID);
        updateModified();
    }
#endregion
}
