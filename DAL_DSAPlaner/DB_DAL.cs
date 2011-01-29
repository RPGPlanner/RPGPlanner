using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Configuration;

/// <summary>
/// A Singleton Database Access Layer
/// </summary>
public static class DB_DAL
{
    public static DbProviderFactory factory { get; private set; }

    static DB_DAL()
    {
        factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["DSAPlanerConnectionString"].ProviderName);
    }

    public static IDbConnection openConnection()
    {
        IDbConnection con = factory.CreateConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["DSAPlanerConnectionString"].ConnectionString;
        con.Open();

        return con;
    }

    public static DataTable getTable(string Query)
    {
        DataSet setRet = new DataSet();
        IDbDataAdapter da = factory.CreateDataAdapter();
        using (IDbConnection con = openConnection())
        using (da.SelectCommand = con.CreateCommand())
        {
            da.SelectCommand.CommandText = Query;
            da.Fill(setRet);
            return setRet.Tables[0];
        }
    }

    public static void addDBParameter(IDbCommand com, string strParameterName, DbType type, object value)
    {
        IDataParameter paramNew = com.CreateParameter();
        paramNew.ParameterName = strParameterName;
        paramNew.DbType = type;
        paramNew.Value = value;
        com.Parameters.Add(paramNew);
    }

    public static object execScalar(string Query)
    {
        using (IDbConnection con = openConnection())
        using (IDbCommand oc = con.CreateCommand())
        {
            oc.CommandText = Query;
            return oc.ExecuteScalar();
        }
    }
}
