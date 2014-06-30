namespace HawaiiDBEDT.Domain.Utilities
{
    using System;
    using System.Net.Mail;

    public class Email
	{
		public static void SendMail(string from, string to, string cc, string subject, string body, string smtpServer, bool isHTML)
		{
		    using (var mailMessage = new MailMessage())
		    {
		        mailMessage.From = new MailAddress(from);

		        if (cc != string.Empty)
		        {
		            mailMessage.CC.Add(cc.Replace(";", ","));
		        }

		        mailMessage.To.Add(to.Replace(';', ','));
		        mailMessage.Subject = subject;
		        mailMessage.Body = body;
		        mailMessage.IsBodyHtml = isHTML;

		        var client = new SmtpClient(smtpServer);
		        client.Send(mailMessage);
		    }
		}
	}
}
