using System;
using System.Data;

public class DBObjectFactory
{
#region Reflection stuff
    private delegate DBObject GetInstance(DataRow drData);

    private static GetInstance getFactoryMethod(DBObject.ObjType type)
    {
        System.Reflection.MethodInfo mi = DBObject.typEnum2Type(type).GetMethod("getInstanceByDataRow");
        return (GetInstance)Delegate.CreateDelegate(typeof(GetInstance), mi);
    }

    private static DBObject[] getArray(DBObject.ObjType type, int size)
    {
        Type arType = DBObject.typEnum2Type(type).MakeArrayType();
        System.Reflection.ConstructorInfo ac = arType.GetConstructor(new Type[] { typeof(int) });
        return (DBObject[])ac.Invoke(new object[] { size });
    }

    public static DBObject getObjectByID(int ID, DBObject.ObjType type)
    {
        string baseTable = DBObject.getBaseTable(type);
        GetInstance factoryMethod = getFactoryMethod(type);

        DataTable dtObj = DB_DAL.getTable("SELECT * FROM " +
            baseTable + " WHERE ID=" + ID.ToString());

        if (dtObj.Rows.Count <= 0)
            return null;

        return factoryMethod(dtObj.Rows[0]);
    }
#endregion

    public static DBObject[] getAllObjects(DBObject.ObjType type)
    {
        return getCustomObjects(type, "");
    }

    public static DBObject[] getCustomObjects(DBObject.ObjType type, string whereClause)
    {
        string baseTable = DBObject.getBaseTable(type);
        GetInstance factoryMethod = getFactoryMethod(type);

        DataTable dtObj = DB_DAL.getTable("SELECT * FROM " + baseTable
            + whereClause);

        DBObject[] allObj = getArray(type, dtObj.Rows.Count);

        for (int i = 0; i < dtObj.Rows.Count; ++i)
            allObj[i] = factoryMethod(dtObj.Rows[i]);

        return allObj;
    }
}
