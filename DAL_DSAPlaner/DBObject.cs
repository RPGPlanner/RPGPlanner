using System;
using System.Data;
using System.Reflection;

/// <summary>
/// Base class for adventures, players, meetings and heroes
/// </summary>
public abstract class DBObject
{
    public enum ObjType { Adventure, Meeting, Person, Hero, Wish, 
        LogEntry, Forum_Group, Forum_Thread, Forum_Message, MeetingPerson };

#region Database Properties

    protected int _dbID;
    public int dbID
    {
        get
        {
            return _dbID;
        }
    }

    private DateTime _datCreated;
    public DateTime datCreated
    {
        get
        {
            return _datCreated;
        }
    }

    protected DateTime _datModified;
    public DateTime datModified
    {
        get
        {
            return _datModified;
        }
    }

    private int _creatorID;
    public int creatorID
    {
        get
        {
            return _creatorID;
        }
    }

    public Person creator
    {
        get
        {
            return (Person)DBObjectFactory.getObjectByID(creatorID, DBObject.ObjType.Person);
        }
    }

#endregion

    protected void updateAttribute(string updateField, object updateValue, DbType type)
    {
        using (IDbConnection con = DB_DAL.openConnection())
        using (IDbCommand com = con.CreateCommand())
        {
            com.CommandText = "UPDATE " + getBaseTable(GetObjType()) +
                                " SET " + updateField
                                + "=@param,datModified=" + Consts.DB_GetDate + " WHERE ID=@dbID";

            DB_DAL.addDBParameter(com, "@param", type, updateValue);
            DB_DAL.addDBParameter(com, "@dbID", DbType.Int32, dbID);

            com.ExecuteNonQuery();
        }
        _datModified = DateTime.Now;
    }

    public static string getBaseTable(ObjType type)
    {
        switch (type)
        {
            case ObjType.Hero:
            case ObjType.Wish:
                return type.ToString() + "es";
            default:
                return type.ToString() + "s";
        }
    }

    public static Type typEnum2Type(DBObject.ObjType type)
    {
        return Type.GetType(type.ToString());
    }

    public static ObjType type2typEnum(Type type)
    {
        return (ObjType)Enum.Parse(typeof(ObjType), type.ToString());
        //foreach (ObjType ot in ObjType)     // not so nice...
        //    if (typEnum2Type(ot) is Type)
        //        return ot;

        //throw new ArgumentOutOfRangeException("type", type, "Unrecognized Type, not a DBObject");
    }

    public ObjType GetObjType()
    {
        return type2typEnum(GetType());
    }

    public void updateModified()
    {
        DB_DAL.execScalar("UPDATE " + getBaseTable(type2typEnum(this.GetType()))
            + " SET datModified=" + Consts.DB_GetDate + " WHERE ID=" + dbID.ToString());
        _datModified = DateTime.Now;
    }

    public bool isNew(Person user)
    {
        return datModified.CompareTo(user.lastLogin) > 0;
    }

    protected void setBaseAttributes(DataRow drObject)
    {
        this._dbID = Convert.ToInt32(drObject["ID"]);
        this._datCreated = Convert.ToDateTime(drObject["datCreated"]);
        this._datModified = Convert.ToDateTime(drObject["datModified"]);
        this._creatorID = Convert.ToInt32(drObject["ID_creator"]);
    }

#region StatusFlag stuff
    protected static int statusValues2statusFlags(int[] statusValues)
    {
        int retVal = 0;
        foreach (int statusValue in statusValues)
            retVal += Convert.ToInt32(Math.Pow(2,statusValue));
        return retVal;
    }

    protected static int[] statusFlags2statusValues(int statusFlags)
    {
        System.Collections.ArrayList alValues = new System.Collections.ArrayList(4);

        for (int i = 0; i < 4; ++i)
            if (((statusFlags >> i) & 1) == 1)
                alValues.Add(i);

        return (int[])alValues.ToArray(typeof(int));
    }

    protected static string statusFlags2whereClause(int statusFlags)
    {
        string where = " WHERE ";
        foreach (int i in statusFlags2statusValues(statusFlags))
            where += "status=" + i.ToString() + " OR ";
        return where.Substring(0, where.Length - 4);   // last " OR " must go
    }
#endregion

    protected static DataTable referenceInDT(DataTable dt, string objectName, string attributeName)
    {
        string columnName = objectName + "." + attributeName;
        dt.Columns.Add(new DataColumn(columnName, typeof(string)));
        foreach (DataRow drObject in dt.Rows)
        {
            PropertyInfo attribInfo = drObject[objectName].GetType().GetProperty(attributeName);
            drObject[columnName] = attribInfo.GetValue(drObject[objectName], null);
        }

        return dt;
    }

    protected static DataTable list2DataTable(System.Collections.IList list, Type type)
    {
        DataTable dt = new DataTable();
        PropertyInfo[] pi = type.GetProperties();

        foreach (PropertyInfo p in pi)
            dt.Columns.Add(new DataColumn(p.Name, p.PropertyType));

        foreach (object obj in list)
        {
            object[] row = new object[pi.Length];

            int i = 0;

            foreach (PropertyInfo p in pi)
            {
                object rowVal = p.GetValue(obj, null);
                if (rowVal is DateTime)
                    if ((DateTime)rowVal == DateTime.MinValue)
                        rowVal = DBNull.Value;
                row[i++] = rowVal;
            }
            dt.Rows.Add(row);
        }

        dt = referenceInDT(dt, "creator", "userName");
        //dt.Columns.Add(new DataColumn("creator.userName", typeof(string)));
        //foreach (DataRow drObject in dt.Rows)
        //    drObject["creator.userName"] = ((Person)drObject["creator"]).userName;

        return dt;
    }

#region Construction methods
    protected DBObject[] getAssociated(ObjType type, string refCol)
    {
        return DBObjectFactory.getCustomObjects(type, " WHERE " + refCol
            + "=" + dbID.ToString());
    }
#endregion
}
