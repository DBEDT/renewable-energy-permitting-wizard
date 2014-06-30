namespace HawaiiDBEDT.Data
{
    using System;
    using System.Configuration;
    using System.Text;
    using System.Transactions;
    using System.Web;
    using System.Web.Security;
    using log4net;

    public class UserAccountManager
    {
        protected string FROM_EMAIL = ConfigurationManager.AppSettings["PasswordEmailSender"];
        private string CONTACT_EMAIL = ConfigurationManager.AppSettings["ApplicationContact"];
        private string MAIL_SERVER = ConfigurationManager.AppSettings["SmtpServer"];

        public UserAccountManager()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void ResetPasswordFor(int userId, string userEmailAddress)
        {
            ILog logger = LogManager.GetLogger(GetType());
            string newPassword = Domain.Utilities.RandomPassword.Generate(8, 12);
            string encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1");
            using (var transaction = new TransactionScope())
            {
                try
                {
                    User.UpdateUserPassword(userId, encryptedPassword);
                    SendEmail(ConfigurationManager.AppSettings["ApplicationUrl"], userEmailAddress, newPassword);
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    logger.Error("There was an exception during password reset.", e);
                }
            }
        }

        private void SendEmail(string appUrl, string username, string newPassword)
        {
            var sb = new StringBuilder();
            sb.Append("Your Hawaii DBEDT Renewable Energy Permitting Wizard password has been reset to:\n");
            sb.Append(newPassword);

            sb.Append("\n\nPlease go to " + appUrl + " to login with your new password.");
            sb.Append("\n\nTo change your password after logging in, please go to the \"My Profile\" page (" + appUrl + "profile/) after logging into the application.");

            Domain.Utilities.Email.SendMail(FROM_EMAIL, username, "", "Hawaii DBEDT Renewable Energy Permitting Wizard Password", sb.ToString(), MAIL_SERVER, false);
        }
    }
}