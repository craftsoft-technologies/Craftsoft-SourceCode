using System.Net;
using System.Net.Mail;
using APP.Infrastructure.EmailManagement.Configuration;

namespace APP.Infrastructure.EmailManagement
{
    public class EmailManager : IEmailManager
    {
        private EmailManager() { }

        private static EmailManager instance = new EmailManager();
        public static EmailManager Instance
        {
            get { return instance; }
        }

        public void SendMail(string receiverEmail, string content, string subject)
        {
            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress(receiverEmail));
            message.From = new MailAddress(AppConfiguration.Instance.mailSetting.SenderNoReplyMailAddress);
            message.Subject = subject;

            // Create plain text mode for alternative view
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
            message.AlternateViews.Add(plainView);

            //// Create HTML email version
            //MailDefinition mailDef = new MailDefinition();
            //mailDef.BodyFileName = string.Format(@"{0}\{1}", templateDir, @"Email.html");
            //mailDef.IsBodyHtml = true;
            //mailDef.Subject = "Test_APP_HTML Version";

            //// Build replacement collection to replace fields in Email.htm file
            //// Use fields anywhere in the template file. I.e.   <%FRIENDNAME%>
            //ListDictionary replacements = new ListDictionary();
            //replacements.Add("<%NAME%>", recipient.Name);

            //// Use dummy control as owner (I.e. new System.Web.UI.Control()) as were in a class library.
            //// It's only use to determine where the access templates from as a relative base.
            //MailMessage msgHtml = mailDef.CreateMailMessage(recipient.Email, replacements, new System.Web.UI.Control());
            //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(msgHtml.Body, null, "text/html");

            // Add linked resources
            //AddLinkedResources(templateDir, ref htmlView);

            // Add HTML view
            // message.AlternateViews.Add(htmlView);

            SendMail(message);
        }

        private void SendMail(MailMessage mailMessage)
        {
            SmtpClient client = new SmtpClient(AppConfiguration.Instance.mailSetting.SmtpServer, AppConfiguration.Instance.mailSetting.SmtpServerPort);
            client.Credentials = new NetworkCredential(AppConfiguration.Instance.mailSetting.SmtpServerUsername, AppConfiguration.Instance.mailSetting.SmtpServerPassword);
            if (AppConfiguration.Instance.mailSetting.SmtpEnableSSL)
                client.EnableSsl = true;

            client.Send(mailMessage);
        }
    }
}
