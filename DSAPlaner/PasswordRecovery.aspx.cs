using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class PasswordRecovery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbName.Text))
        {
            lbAnswer.Text = "Du musst schon was eingeben";
            return;
        }
        
        Person resetPerson = Person.getInstanceByName(tbName.Text);

        if (null == resetPerson)
        {
            lbAnswer.Text = "Hab noch nie von dir gehört. Hat dich irgendwer umbenannt? Red Mal mit dem Christoph.";
            return;
        }

        if (string.IsNullOrEmpty(resetPerson.email))
        {
            lbAnswer.Text = "Ich kenne deine Email-Adresse nicht. So kann ich dir auch nix zuschicken. Rede Mal mit dem Christoph, der hilft dir.";
            return;
        }

        string newPassword = createPassword();
        resetPerson.changePassword(newPassword);
        resetPerson.sendMail("Passwort zurückgesetzt", "Hallo "
            + resetPerson.userName   
            + ",\n\ndein Passwort wurde zurückgesetzt. Dein neues Passwort ist \""
            + newPassword + "\" (Groß- und Kleinschreibung ist wichtig!) ohne die \"\". Dein Benutzername ist \""
            + resetPerson.userName + "\" (Groß- und Kleinschreibung ist egal) ohne die \"\". Hier nochmal der Link zum DSA Planer:\n\n"
            + ConfigurationManager.AppSettings["DSAPlanerURL"]
            + "\n\nProbier's am Besten gleich aus und ändere dein Passwort auf was Vernünftiges.");

        lbAnswer.Text = "Sie haben Post!";
        tbName.Visible = lbName.Visible = btnSend.Visible = false;
    }

    protected string createPassword()
    {
        const string passChars = "2345678abcdefghkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
        const int passLength = 8;

        System.Text.StringBuilder newPass = new System.Text.StringBuilder(passLength);
        Random rnd = new Random();
        for (int i = 0; i < passLength; ++i)
        {
            int nxtChar = rnd.Next(passChars.Length - 1);
            newPass.Append(passChars[nxtChar]);
        }

        return newPass.ToString();
    }
}
