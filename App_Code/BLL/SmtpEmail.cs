using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EBA.Desktop;

/// <summary>
/// Summary description for SmtpEmail
/// </summary>
public class SmtpEmail
{
	public SmtpEmail()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    MailMessage mailmsg = new MailMessage();
    MailAddressCollection mailTo = new MailAddressCollection();

    public bool Send(string eFrom, string eTo, string subj, string body, int woNum, int woRespNum, string proj)
    {
        ldapClient userObject = new ldapClient();
        UserRecord ud;
        ud = userObject.SearchUser(eFrom.ToString());
        eFrom = ud.Email.ToString();
        ud = new UserRecord();
        ud = userObject.SearchUser(eTo.ToString());
        eTo = ud.Email;
        LoadCC(woNum, proj);
        //MailMessage mailmsg = new MailMessage();
        MailAddress mailFrom = new MailAddress(eFrom); //+ "@acme.com"
        MailAddress mailRecip = new MailAddress(eTo); //+ "@acme.com"

        if (mailmsg.CC.Count > 0)
        {
            MailAddress[] MAarray = new MailAddress[0];
            
            foreach (MailAddress ma in mailmsg.CC)
            {
                if (ma.Address == mailRecip.Address || ma.Address == mailFrom.Address) 
                {
                    Array.Resize(ref MAarray, MAarray.Length + 1);
                    MAarray[MAarray.Length - 1] = ma;
                    //Array.Copy(ma,MAarray,1);
                }
            }
            if (MAarray.Length > 0)
            {
                foreach (MailAddress ma1 in MAarray)
                {
                    mailmsg.CC.Remove(ma1);
                }
            }
        }

        mailmsg.From = mailFrom;
        mailmsg.ReplyTo = mailFrom;
        //mailTo.Add(mailRecip);
        mailmsg.To.Add(mailRecip);
        
        mailmsg.Subject = subj;
        mailmsg.Body = body;
        SmtpClient mailClient = new SmtpClient();
        
        mailClient.UseDefaultCredentials = true;
        mailClient.Port = 25;
       

        try
        {
            mailClient.Send(mailmsg);

            WorkOrderBLL WOobj = new WorkOrderBLL();
            int eMailNum = WOobj.GetMaxEmailNum(woNum, proj) + 1;
            bool YNemail = WOobj.InsertEmailRecord(eMailNum, woNum, woRespNum, proj, 
                mailmsg.From.ToString(), mailmsg.To.ToString(), mailmsg.CC.ToString(), 
                mailmsg.Subject, mailmsg.Body, DateTime.Now);

        }
        catch (SmtpFailedRecipientsException recExc)
        {
            for (int i = 0; i < recExc.InnerExceptions.Length; i++)
            {
                SmtpStatusCode statCd = recExc.InnerExceptions[i].StatusCode;
                if ((statCd == SmtpStatusCode.MailboxBusy)||(statCd == SmtpStatusCode.MailboxUnavailable))
                {
                    System.Threading.Thread.Sleep(5000);
                    mailClient.Send(mailmsg);
                }
            }
        }
        catch (SmtpException smtpEx)
        {
            string errmsg = smtpEx.InnerException.Message;
            string errmsg2 = smtpEx.Message;
            string statusCd = smtpEx.StatusCode.ToString();
            return false;
        }
        catch (Exception Ex)
        {
            string ExMsg = Ex.Message;
        }

        return true;

    }

    private void LoadCC(int woNum, string proj)
    {
        RolesBLL roleLogic = new RolesBLL();
        WorkOrder.WOroleTextDisplayDataTable rolesDT = roleLogic.GetWOrolesByWOnumProj(woNum,proj);
        string ls_cc;
        ldapClient userObject = new ldapClient();
        UserRecord ud;


        foreach (WorkOrder.WOroleTextDisplayRow roleRow in rolesDT)
        {
            if (roleRow.worl_email)
            {
                ud = userObject.SearchUser(roleRow.worl_uid.ToString());
                ls_cc = ud.Email.ToString();

                mailmsg.CC.Add(ls_cc);
            }

        }
        
        
    }
}
