using System;
using System.Data;

public class MeetingPerson : Person
{
    public enum Status { unknown = -1 ,accept = 0, perhaps = 1, decline = 2, atHome = 3 };
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

    private int _meeting_ID;
    public int meeting_ID
    {
        get
        {
            return _meeting_ID;
        }
    }

    private Meeting _thisMeeting;
    public Meeting thisMeeting
    {
        get
        {
            if (_thisMeeting == null)
                _thisMeeting = (Meeting)DBObjectFactory.getObjectByID(meeting_ID, ObjType.Meeting);

            return _thisMeeting;
        }
    }

    private MeetingPerson(DataRow drUser)
        : base(drUser)
	{
        if (drUser["status"] is DBNull)
            _status = Status.unknown;
        else
            _status = (Status)Convert.ToInt32(drUser["status"]);
        _meeting_ID = Convert.ToInt32(drUser["ID_meeting"]);
	}

    public static new MeetingPerson getInstanceByDataRow(DataRow drUser)
    {
        return new MeetingPerson(drUser);
    }

    public static MeetingPerson[] getAllByMeeting(int IDMeeting)
    {
        return ((Meeting)DBObjectFactory.getObjectByID(IDMeeting, DBObject.ObjType.Meeting)).persons;
    }

    public static void update(int meeting_ID, int dbID, Status status)
    {
        update(meeting_ID, dbID, Convert.ToInt32(status));
    }

    public static void update(int meeting_ID, int dbID, int int_status)
    {
        int rowExist = (int)DB_DAL.execScalar("SELECT Count(*) FROM rel_persons_meetings"
            + " WHERE ID_person=" + dbID.ToString() + " AND ID_meeting=" + meeting_ID.ToString());

        if (rowExist < 1)  // This entry doesn't exist yet
        {
            DB_DAL.execScalar("INSERT INTO rel_persons_meetings(Status,ID_person,ID_meeting)"
            + " VALUES(" + int_status.ToString() + "," + dbID.ToString() + "," +
            meeting_ID.ToString() + ")");
        }
        else                // entry does exist
        {
            DB_DAL.execScalar("UPDATE rel_persons_meetings SET Status="
            + int_status.ToString() + " WHERE ID_person=" + dbID.ToString()
            + " AND ID_meeting=" + meeting_ID.ToString());
        }

        ((Meeting)DBObjectFactory.getObjectByID(meeting_ID, ObjType.Meeting)).updateModified();
    }

    public static void deleteByMeeting(int meeting_ID)
    {
        DB_DAL.execScalar("DELETE FROM rel_persons_meetings WHERE ID_meeting=" 
            + meeting_ID.ToString());
    }
}
