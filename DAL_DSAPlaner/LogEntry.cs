using System;
using System.Data;

/// <summary>
/// Summary description for Error
/// </summary>
public class LogEntry : DBObject
{
    private string _description;
    public string description
    {
        get
        {
            return _description;
        }
    }

    private string _name;
    public string name
    {
        get
        {
            return _name;
        }
    }

    private LogEntry(DataRow drLog)
	{
        setBaseAttributes(drLog);

        _description = drLog["Description"].ToString();
        _name = drLog["Name"].ToString();
    }

#region Creator methods
    public static LogEntry getInstanceByDataRow(DataRow drLog)
    {
        return new LogEntry(drLog);
    }

    public static LogEntry[] getAll()
    {
        return (LogEntry[])DBObjectFactory.getAllObjects(ObjType.LogEntry);
    }
       
    public static DataTable getAllDT()
    {
        return list2DataTable(getAll(), typeof(LogEntry));
    }
#endregion

#region DB changing methods
    public static void insert(string message, string description, int creatorID)
    {
        insert(message, description, 0, creatorID);
    }

    public static void insert(string message, string description, int logType, int creatorID)
    {
        string sQuery = "INSERT INTO log(name,description,logType,ID_creator) VALUES('" +
         message.Replace("'", "''") + "','" + description.Replace("'", "''") +
             "'," + logType.ToString() + "," + creatorID.ToString() + ")";

        DB_DAL.execScalar(sQuery);
    }
#endregion
}
