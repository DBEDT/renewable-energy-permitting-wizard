using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HawaiiDBEDT.Domain.Utilities;
using System.Configuration;
using HawaiiDBEDT.Data;

namespace HawaiiDBEDT.Web.Profile
{
	public partial class Register : BasePage
	{
		protected string loginInfoCookie = ConfigurationManager.AppSettings["loginInfoCookie"];
		private string encryptionKey = ConfigurationManager.AppSettings["encryptionKey"];

		protected string AdditionalConfirmationMessage = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				PopulateLookups();
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

            rcbLocation.Items.Insert(0, new ListItem("< please select >", ""));
		}

		private void DisplayTechnology()
		{
			rcbTechnology.DataSource = DataAccess.TechnologyList;
			rcbTechnology.DataBind();

            rcbTechnology.Items.Insert(0, new ListItem("< please select >", ""));
		}

		#region Registration				

		protected void rcbTechnology_Validate(object sender, ServerValidateEventArgs e)
		{
			if (rcbTechnology.SelectedValue != "")
			{
				e.IsValid = true;
			}
			else
			{
				e.IsValid = false;
			}
		}

		protected void rcbLocation_Validate(object sender, ServerValidateEventArgs e)
		{
			if (rcbLocation.SelectedValue != "")
			{
				e.IsValid = true;
			}
			else
			{
				e.IsValid = false;
			}
		}

		protected void ProcessData()
		{
			AdditionalConfirmationMessage = "";

			pnlExistingUser.Visible = false;
			if (IsExistingUser())
			{
				pnlExistingUser.Visible = true;
			}
			else
			{
				Domain.User user = new Domain.User();
				user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPasswordRegistration.Text, "sha1");
				user.Name = txtName.Text.Trim();
				user.EmailAddress = txtEmailAddressRegistration.Text.Trim();
				user.Organization = txtOrganization.Text.Trim();
				user.Title = txtTitle.Text.Trim();
				user.TelephoneNumber = txtTelephone.Text.Trim();

				user.TechnologyID = int.Parse(rcbTechnology.SelectedValue);
				user.LocationID = int.Parse(rcbLocation.SelectedValue);

				// Add to database
				int userID = Data.User.AddUser(user);
				// login user automatically
				SetUserAuthentication(userID.ToString());

				// if there are any evaluations in progress, save it
				if (CurrentEvaluation.TechnologyID != 0 && CurrentEvaluation.UserID == 0)
				{
					CurrentEvaluation.UserID = userID;
					SaveCurrentEvaluationComplete();
				}

				Response.Redirect("register_confirm.aspx", true);
			}
		}

		private bool IsExistingUser()
		{
			return Data.User.IsExistingUser(txtEmailAddressRegistration.Text);
		}

		protected void btnRegister_Click(object sender, EventArgs e)
		{
			Page.Validate();

			// Process contact info
			if (Page.IsValid)
			{
				ProcessData();
			}
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			Response.Redirect(Request.Url.AbsolutePath.ToString(), true);
		}
		
		private void SetUserAuthentication(string userID)
		{
			// Create a new ticket used for authentication
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userID,
			    DateTime.Now,
			    DateTime.Now.AddMinutes(600), // 600 minutes(10 hours) so user only has to log in once a day
			    true, // true for persistent user cookie
			    userID);
			string hash = FormsAuthentication.Encrypt(ticket);

			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
			// You must expire the cookie. 
			// Otherwise, the user will never have to authenticate against the database again.
			cookie.Expires = ticket.Expiration;

			// Add the cookie to the list for outgoing response
			Response.Cookies.Add(cookie);
		}
		#endregion
	}
}