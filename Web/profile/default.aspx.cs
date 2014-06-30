using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain.Enumerations;
using System.Web.Security;
using HawaiiDBEDT.Data;
using HawaiiDBEDT.Domain.Utilities;
using System.Configuration;

namespace HawaiiDBEDT.Web.Profile
{
    public partial class ProfileDefault : System.Web.UI.Page
    {
        private string loginInfoCookie = ConfigurationManager.AppSettings["loginInfoCookie"];
        private string encryptionKey = ConfigurationManager.AppSettings["encryptionKey"];
        Domain.User CurrentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetUser();
                PopulateLookups();
                DisplayUserInfo();
            }
        }

        private void GetUser()
        {
            if (HttpContext.Current.User.Identity.Name != "")
            {
                CurrentUser = Data.User.GetUser(int.Parse(HttpContext.Current.User.Identity.Name));
            }
            else
            {
                Response.Redirect("~/default.aspx?referrer=/profile/", true);
            }
        }

        private void PopulateLookups()
        {
            DisplayTechnology();
            DisplayLocation();
        }

        private void DisplayLocation()
        {
            rcbLocation.DataSource = DataAccess.LocationList;
            rcbLocation.DataBind();
        }

        private void DisplayTechnology()
        {
            rcbTechnology.DataSource = DataAccess.TechnologyList;
            rcbTechnology.DataBind();
        }

        private void DisplayUserInfo()
        {
            txtName.Text = CurrentUser.Name;
            txtOrganization.Text = CurrentUser.Organization;
            txtTitle.Text = CurrentUser.Title;
            txtTelephone.Text = CurrentUser.TelephoneNumber;

            rcbLocation.Items.FindByValue(CurrentUser.LocationID.ToString()).Selected = true;
            rcbTechnology.Items.FindByValue(CurrentUser.TechnologyID.ToString()).Selected = true;

            txtEmail.Text = CurrentUser.EmailAddress;
            txtPasswordUpdate.Text = "";
            txtPasswordUpdate2.Text = "";
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ProcessData();
            }
        }

        private bool IsExistingUser()
        {
            int existingUserID = Data.User.GetUserIDByEmail(txtEmail.Text.Trim());

            if (existingUserID == 0)
            {
                return false;
            }
            else if (existingUserID.ToString() != HttpContext.Current.User.Identity.Name)
            {
                return true;
            }
            return false;
        }

        protected void ProcessData()
        {
            pnlExistingUser.Visible = false;
            if (IsExistingUser())
            {
                pnlExistingUser.Visible = true;
            }
            else
            {
                Domain.User user = new Domain.User();
                user.ID = int.Parse(HttpContext.Current.User.Identity.Name);
                if (txtPasswordUpdate.Text != "")
                {
                    user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPasswordUpdate.Text, "sha1");
                }
                else
                {
                    user.Password = string.Empty;
                }
                user.Name = txtName.Text.Trim();
                user.EmailAddress = txtEmail.Text.Trim();
                user.Organization = txtOrganization.Text.Trim();
                user.Title = txtTitle.Text.Trim();
                user.TelephoneNumber = txtTelephone.Text.Trim();
                user.TechnologyID = int.Parse(rcbTechnology.SelectedValue);
                user.LocationID = int.Parse(rcbLocation.SelectedValue);
                Data.User.UpdateUser(user);

                pnlConfirmation.Visible = true;

                UpdateCookieInfo(user.EmailAddress, txtPasswordUpdate.Text);
            }
        }

        // If password is updated, update cookie
        // Cookie is created if user selects to remember login information on the login page
        private void UpdateCookieInfo(string username, string password)
        {
            var cookie = (HttpCookie)Request.Cookies[loginInfoCookie];
            if (cookie != null)
            {
                string cookieName = cookie["username"];
                if (cookieName != "")
                {
                    if (username != cookieName)
                    {
                        cookie["username"] = username;
                    }
                    if (password != "")
                    {
                        cookie["password"] = EncryptDecrypt.Encrypt(password, encryptionKey);
                    }

                    // This cookie should not expire right away
                    cookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(cookie);
                }
            }
        }

        protected void CustValConfirmPasswordValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !revPasswordUpdate.IsValid || String.IsNullOrEmpty(txtPasswordUpdate.Text) || !String.IsNullOrEmpty(txtPasswordUpdate2.Text);
        }

        protected void CustValOldPasswordValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(txtPasswordUpdate.Text))
            {
                if (!string.IsNullOrEmpty(txtOldPassword.Text))
                {
                    var userId = int.Parse(HttpContext.Current.User.Identity.Name);
                    Domain.User oldUser = Data.User.GetUser(userId);
                    string oldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtOldPassword.Text,
                                                                                                "sha1");
                    args.IsValid = oldUser.Password.Equals(oldPassword);
                }
                else
                {
                    args.IsValid = false;
                }
            } 
            else
            {
                args.IsValid = true;
            }
        }
    }
}