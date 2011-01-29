using System;
using System.Net.Mail;

namespace DAL_DSAPlaner
{
    /// <summary>
    /// Singleton class for sending mails
    /// </summary>
    public class MailManager
    {
        const int daysToPredict = 3;

        static volatile MailManager mm;

        SmtpClient smtp;

        private string[] extractMail(Person[] mailPersons, int mailPref)
        {
            System.Collections.ArrayList alPersons = new System.Collections.ArrayList(mailPersons.Length);

            for (int i = 0; i < mailPersons.Length; ++i)
                if ((mailPersons[i].emailPref & mailPref) == mailPref 
                  && mailPersons[i].email.IndexOf('@') > 0)
                    alPersons.Add(mailPersons[i].email);

            return (string[])alPersons.ToArray(typeof(string));
        }

        private MailManager()
        {
            string smptHost = Consts.Mail_SMTPHost;
            int smtpPort = Consts.Mail_SMTPPort;
            smtp = new SmtpClient(smptHost, smtpPort);

            smtp.Credentials = new System.Net.NetworkCredential(
                Consts.Mail_User, Consts.Mail_PW);
        }

        public void sendMail(string recipient, string subject, string body)
        {
            sendMail(new string[] { recipient }, subject, body);
        }

        public void sendMail(string[] recipients, string subject, string body)
        {
            if (recipients.Length == 0)
                return;
            MailMessage message = new MailMessage();
            
            message.From = new MailAddress(Consts.Mail_From);
            foreach(string recipient in recipients)
                message.To.Add(recipient);
            message.Subject = "DSA Planer: " + subject;
            message.Body = body + "\n\n------\n DSA Planer - " +
                Consts.DSAPlanerURL;

            string ReplyTo = Consts.Mail_ReplyTo;
            if (ReplyTo != string.Empty)
                message.ReplyTo = new MailAddress(ReplyTo);
            
            smtp.Send(message);
        }

        public void dailyMail(DateTime dtTargetDay)
        {
            sendChangeMail(dtTargetDay);
            sendRememberMail();
        }

        public void sendChangeMail(DateTime dtChanges)
        {
            string sBody = "Liebe Nutzer des DSA Planer,\n\ngestern hat sich am DSA Planer was getan!\n\n";
            bool fChanges = false;

            string sWhere = " WHERE CONVERT(nvarchar,datModified,112)='"
               + dtChanges.ToString("yyyyMMdd") + "'";

                // Threads
            DBObject[] changeObjects = DBObjectFactory.getCustomObjects(DBObject.ObjType.Forum_Thread
                , sWhere + " AND ID_group<>0");
            if (changeObjects.Length > 0)
            {
                fChanges = true;
                sBody += "Im Forum gab es folgende Änderungen:\n\n" +
                listModifications((IChangeListable[])changeObjects) + "\n";
            }

                // Meetings
            changeObjects = DBObjectFactory.getCustomObjects(DBObject.ObjType.Meeting
                , sWhere);
            if (changeObjects.Length > 0)
            {
                fChanges = true;
                sBody += "Die folgenden Termine haben sich geändert:\n\n" +
                listModifications((IChangeListable[])changeObjects) + "\n";
            }

            if (fChanges)
                sendMail(extractMail(Person.getAll(),1), "Aktuelle Änderungen", sBody);
        }

        public void sendRememberMail()
        {
            string sBody = "Hallo liebe Spätzünder,\n\nfür den folgenden Termin fehlt Eure Zu- oder Absage:\n\n";

            Meeting[] nextMeetings = (Meeting[])DBObjectFactory.getCustomObjects(DBObject.ObjType.Meeting,
                " WHERE 0 <= DATEDIFF(day, " + Consts.DB_GetDate + ", datMeeting) AND DATEDIFF(day, " + Consts.DB_GetDate + ", datMeeting) <= 3");

            foreach (Meeting currMeeting in nextMeetings)
            {
                const int mailPref = 2; // for the remebrance mails

                System.Collections.ArrayList alPersons = new System.Collections.ArrayList(8);

                MeetingPerson[] mailPersons = currMeeting.persons;

                for (int i = 0; i < mailPersons.Length; ++i)
                    if ((mailPersons[i].emailPref & mailPref) == mailPref
                      && mailPersons[i].email.IndexOf('@') > 0
                      && mailPersons[i].status == MeetingPerson.Status.unknown)
                        alPersons.Add(mailPersons[i].email);

                if (alPersons.Count > 0)
                    sendMail((string[])alPersons.ToArray(typeof(string)),
                        "Noch kein Status eingetragen",
                        sBody + currMeeting.ToString());
            }
        }

        private static string listModifications(IChangeListable[] changedObjects)
        {
            string retVal = string.Empty;
            foreach (IChangeListable oList in changedObjects)
                retVal += oList.getChangeEntry() + "\n";
            return retVal;
        }

        public static MailManager getMailManager()
        {
            if (mm == null)                 // this if is fast, but it can fail
                lock (typeof(MailManager))
                {
                    if (mm == null)         // This if is slow, but always right
                        mm = new MailManager();
                }

            return mm;
        }
    }
}
