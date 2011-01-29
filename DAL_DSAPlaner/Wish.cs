using System;
using System.Data;

public class Wish : DBObject
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

    private string _name;
    public string name
    {
        get
        {
            return _name;
        }
    }

    public enum Status { wish = 0, open = 1, complete = 2, delayed = 3 };
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
    
    private DateTime _datCompleted;
    public DateTime datCompleted
    {
        get
        {
            return _datCompleted;
        }
    }
#endregion

    private Wish(DataRow drWish)
	{
        setBaseAttributes(drWish);

        _description = drWish["Description"].ToString();
        _name = drWish["name"].ToString();
        _status = (Status)Convert.ToInt32(drWish["Status"]);
        if (drWish["datCompleted"] != DBNull.Value)
            _datCompleted = Convert.ToDateTime(drWish["datCompleted"]);
    }

#region Creator functions
    public static Wish getInstanceByDataRow(DataRow drWish)
    {
        return new Wish(drWish);
    }

    public static Wish[] getAll()
    {
        return (Wish[])DBObjectFactory.getAllObjects(ObjType.Wish);
    }

    public static Wish[] getAllOpen()
    {
        return getAllByStatus(3);
        //return (Wish[])DBObjectFactory.getCustomObjects(Type.Wish,
        //    " WHERE status=0 OR status=1");
    }

    public static Wish[] getAllByStatus(int statusFlags)
    {
        string where = "";
        if (statusFlags != 15)
            where = statusFlags2whereClause(statusFlags);

        return (Wish[])DBObjectFactory.getCustomObjects(ObjType.Wish,where);
    }

    public static DataTable getAllByStatusDT(int statusFlags)
    {
        return list2DataTable(getAllByStatus(statusFlags), typeof(Wish));
    }
#endregion

#region DB changing methods
    public static void update(int dbID, string name, string description, int int_status)
    {
        DateTime datCompleted;
        if (int_status == 2)
            datCompleted = DateTime.Now;
        else
            datCompleted = DateTime.MinValue;

        using (IDbConnection con = DB_DAL.openConnection())
        using (IDbCommand com = con.CreateCommand())
        {
            com.CommandText = "UPDATE wishes SET Name=@name,Status=@status,Description=@description,"
                + "datCompleted=@datCompleted WHERE ID=@dbID";

            DB_DAL.addDBParameter(com, "@name", DbType.String, name);
            DB_DAL.addDBParameter(com, "@status", DbType.Int32, int_status);
            DB_DAL.addDBParameter(com, "@description", DbType.String, description);
            if (datCompleted == DateTime.MinValue)
                DB_DAL.addDBParameter(com, "@datCompleted", DbType.DateTime, DBNull.Value);
            else
                DB_DAL.addDBParameter(com, "@datCompleted", DbType.DateTime, datCompleted);
            DB_DAL.addDBParameter(com, "@dbID", DbType.Int32, dbID);

            com.ExecuteNonQuery();
        }
    }

    public static void delete(int dbID)
    {
        DB_DAL.execScalar("DELETE FROM wishes WHERE ID=" + dbID.ToString());
    }

    public static void insert(string name, string description, Status status, int creatorID)
    {
        string sQuery = "INSERT INTO wishes(Name,Description,Status,ID_Creator)"
            + " VALUES('" + name.Replace("'", "''") + "'"
            + ",'" + description.Replace("'", "''") + "'"
            + "," + Convert.ToInt32(status).ToString() +
            "," + creatorID.ToString() + ")";

        DB_DAL.execScalar(sQuery);
    }
#endregion
}
