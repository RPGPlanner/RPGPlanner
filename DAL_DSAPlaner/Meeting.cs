using System;
using System.Data;

/// <summary>
/// Summary description for Meeting
/// </summary>
public class Meeting : DBObject, IChangeListable
{
#region Database Properties

    private DateTime _datMeeting;
    public DateTime datMeeting
    {
        get
        {
            return _datMeeting;
        }
        set
        {
            _datMeeting = value;
            updateAttribute("datMeeting", value, DbType.DateTime);
        }
    }

    private int _advID;
    public int advID
    {
        get
        {
            return _advID;
        }
        set
        {
            _advID = value;
            updateAttribute("ID_adventure", value, DbType.Int32);
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

    public Adventure adventure
    {
        get
        {
            return (Adventure)DBObjectFactory.getObjectByID(advID,DBObject.ObjType.Adventure);
        }
    }
    
    /// <summary>
    /// pending + completed, if meeting is in the past;
    /// pending + not started, if meeting is in the future
    /// </summary>
    public Adventure[] possibleAdventures
    {
        get
        {
            Adventure[] retAdv = Adventure.getAllByStatus(statusValues2statusFlags( new int[] { Convert.ToInt32(Adventure.Status.pending), 
                   Convert.ToInt32(DateTime.Now.CompareTo(datMeeting)<0?Adventure.Status.not_started:Adventure.Status.complete) }));
            bool fAdvInList = false;        // adventure of this meeting in list?
            foreach (Adventure adv in retAdv)
                if (fAdvInList = (adv.dbID == adventure.dbID))
                    break;

            if (!fAdvInList)                // if it is not in list, add it
            {
                Adventure[] ret2Adv = new Adventure[retAdv.Length + 1];
                for (int i = 0; i < retAdv.Length; ++i)
                    ret2Adv[i] = retAdv[i];
                ret2Adv[ret2Adv.Length - 1] = adventure;
                return ret2Adv;
            }
            else
                return retAdv;
        }
    }

    public MeetingPerson[] persons
    {
        get
        {
            return (MeetingPerson[])getAssociated(ObjType.MeetingPerson, "ID_meeting");
            //DataTable dtMPs = DB_DAL.getTable("SELECT * FROM MeetingPersons WHERE ID_meeting="
            //    + dbID.ToString());

            //MeetingPerson[] retAr = new MeetingPerson[dtMPs.Rows.Count];
            //for (int i = 0; i < dtMPs.Rows.Count; ++i)
            //    retAr[i] = MeetingPerson.getInstanceByDataRow(dtMPs.Rows[i],dbID);

            //return retAr;
        }
    }

    #region Eval and DataSource Hacks
    /// <summary>
    /// This function is for Eval, which shall return the bound meeting in meetings.aspx
    /// </summary>
    public Meeting thisMeeting
    {
        get
        {
            return this;
        }
    }

    /// <summary>
    /// dataSources need static methods
    /// </summary>
    public static Adventure[] getPossibleAdventures(int meetingID)
    {
        Meeting involvedMeeting = (Meeting)DBObjectFactory.getObjectByID(meetingID, ObjType.Meeting);
        return involvedMeeting.possibleAdventures;
    }

    public string adventureMaster
    {
        get
        {
            return adventure.master.userName;
        }
    }

    public string adventureDescription
    {
        get
        {
            return adventure.description;
        }
    }
    #endregion Eval and DataSource Hacks

    private Meeting(DataRow drMeeting)
	{
        setBaseAttributes(drMeeting);

        this._datMeeting = Convert.ToDateTime(drMeeting["datMeeting"]);
        this._advID = Convert.ToInt32(drMeeting["ID_adventure"]);
        this._threadID = Convert.ToInt32(drMeeting["ID_thread"]);
    }

    public override string ToString()
    {
        return "Das Treffen am " + datMeeting.ToLongDateString() + " um " +
            datMeeting.ToLongTimeString() + " Uhr (Geplantes Abenteuer ist \"" + adventure.title
           + "\")";
    }

#region Construction methods
    public static Meeting getInstanceByID(int ID)
    {
        return (Meeting)DBObjectFactory.getObjectByID(ID, ObjType.Meeting);
    }

    public static Meeting getInstanceByDataRow(DataRow drMeeting)
    {
        return new Meeting(drMeeting);
    }

    public static Meeting[] getMeetingsByDay(DateTime datMeeting)
    {
        DateTime datStartOfDay = datMeeting.AddTicks(-datMeeting.TimeOfDay.Ticks);
        return getMeetingsByDateRange(datStartOfDay, datStartOfDay.AddDays(1));
    }

    public static Meeting[] getMeetingsByDateRange(DateTime datStart, DateTime datEnd)
    {
        if (datEnd.Year < 2000 || datStart.Year > 2050)
            return new Meeting[0];

            // Get SQL Data
        DataTable dtMeetings;

        using (IDbConnection con = DB_DAL.openConnection())
        using (IDbCommand com = con.CreateCommand())
        {
            com.CommandText = "SELECT * FROM meetings " +
                "WHERE datMeeting>=@datStart AND datMeeting<=@datEnd";

            DB_DAL.addDBParameter(com, "@datStart", DbType.DateTime, datStart);
            DB_DAL.addDBParameter(com, "@datEnd", DbType.DateTime, datEnd);

            //    "WHERE YEAR(datMeeting)>=@StartYear AND YEAR(datMeeting)<=@EndYear " +
            //    "AND DATEPART(dayofyear,datMeeting)>=@StartDayOfYear AND DATEPART(dayofyear,datMeeting)<=@EndDayOfYear";

            //DB_DAL.addDBParameter(com, "@StartYear", DbType.Int32, datStart.Year);
            //DB_DAL.addDBParameter(com, "@EndYear", DbType.Int32, datEnd.Year);
            //DB_DAL.addDBParameter(com, "@StartDayOfYear", DbType.Int32, datStart.DayOfYear);
            //DB_DAL.addDBParameter(com, "@EndDayOfYear", DbType.Int32, datEnd.DayOfYear);

            IDbDataAdapter da = DB_DAL.factory.CreateDataAdapter();
            da.SelectCommand = com;

            DataSet dsMeetings = new DataSet();
            da.Fill(dsMeetings);
            dtMeetings = dsMeetings.Tables[0];
        }
        
            // Convert the data into objects
        Meeting[] rangeMeetings = new Meeting[dtMeetings.Rows.Count];
        for (int i = 0; i < dtMeetings.Rows.Count; ++i)
            rangeMeetings[i] = new Meeting(dtMeetings.Rows[i]);

        return rangeMeetings;
    }
#endregion

#region DB changing methods
    public static int insert(DateTime datMeeting, int adventureID, int creatorID)
    {
        int threadID = Forum_Thread.insert("Termin " + datMeeting.ToString("yyyy-MM-dd"),
            Consts.Sys_GroupID_Meetings, creatorID);

        using (IDbConnection con = DB_DAL.openConnection())
        using (IDbCommand com = con.CreateCommand())
        {
            com.CommandText = "INSERT INTO meetings(datMeeting,ID_adventure,ID_thread,ID_creator)"
             + " VALUES(@datMeeting,@adventureID,@threadID,@creatorID);SELECT @@IDENTITY";

            DB_DAL.addDBParameter(com, "@datMeeting", DbType.DateTime, datMeeting);
            DB_DAL.addDBParameter(com, "@adventureID", DbType.Int32, adventureID);
            DB_DAL.addDBParameter(com, "@threadID", DbType.Int32, threadID);
            DB_DAL.addDBParameter(com, "@creatorID", DbType.Int32, creatorID);

            return Convert.ToInt32(com.ExecuteScalar());
        }
    }

    public void delete()
    {
        MeetingPerson.deleteByMeeting(dbID);
        DB_DAL.execScalar("DELETE FROM meetings WHERE ID=" + dbID.ToString());
    }

    public static void update(DateTime datMeeting, int advID, int dbID)
    {
        Meeting currentMeeting = getInstanceByID(dbID);
        currentMeeting.advID = advID;

            // "currentMeeting.datMeeting.TimeOfDay = datMeeting.TimeOfDay"
            // is the meaning of the following code (but TimeOfDay is readonly :-()
        DateTime newMeetingTime = currentMeeting.datMeeting;
        newMeetingTime = newMeetingTime.Subtract(newMeetingTime.TimeOfDay);
        newMeetingTime = newMeetingTime.Add(datMeeting.TimeOfDay);
        currentMeeting.datMeeting = newMeetingTime;
    }
#endregion

    #region IChangeListable Members

    public string getChangeEntry()
    {
        return ToString() + " wurde verändert.";
    }

    #endregion
}
