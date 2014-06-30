using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;


namespace HawaiiDBEDT.Web.Admin
{
    using utils;
    using Westwind.Web.Utilities;

    public partial class DepartmentDetail : System.Web.UI.Page
	{
		protected int departmentID;
		protected Department department;
		protected string ConfirmationText;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetDepartment();
			ConfirmationText = "";

			if (!IsPostBack)
			{
				DisplayDepartment();
			}
		}

		private void GetDepartment()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				departmentID = int.Parse(Request.QueryString["id"]);
				department = Data.Department.GetDepartment(departmentID);				
			}
			else
			{
				departmentID = 0;
				department = new Department();
			}
		}

		private void DisplayDepartment()
		{
			if (departmentID != 0)
			{
				txtName.Text = department.Name;
				reDescription.Text = HtmlEncoder.BetterHtmlDecode(department.Description);
			}
		}
		#region Update

		private bool UpdateDepartment()
		{
			Page.Validate();

			if (Page.IsValid)
			{
				department.Name = txtName.Text;
				department.Description = reDescription.Text;
				if (departmentID != 0)
				{
					Data.Department.UpdateDepartment(department);
				}
				else
				{
					departmentID = Data.Department.AddDepartment(department);
				}

				return true;
			}
			return false;
		}
		#endregion

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("default.aspx", true);
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			if (UpdateDepartment())
			{
				Response.Redirect("default.aspx?action=edit&result=success", true);
			}
		}

		// Used to let admins traverse through records
		public List<int> DepartmentIDList
		{
			get
			{
				if (Session["DepartmentIDList"] != null)
				{
					return (List<int>)Session["DepartmentIDList"];
				}
				else
				{
					return new List<int>();
				}
			}
			set
			{
				Session["DepartmentIDList"] = value;
			}
		}

		public int GetNextDepartmentID(int departmentID)
		{
			if (DepartmentIDList.Count == 0)
			{
				return -1;
			}

			// current submission is last element
			if (departmentID == DepartmentIDList[DepartmentIDList.Count - 1])
			{
				return -1;
			}

			for (int i = 0; i < DepartmentIDList.Count - 1; i++)
			{
				if (DepartmentIDList[i] == departmentID)
				{
					return DepartmentIDList[i + 1];
				}
			}

			// not found
			return -1;
		}

		public int GetPreviousDepartmentID(int departmentID)
		{
			if (DepartmentIDList.Count == 0)
			{
				return -1;
			}
			// current submission is first submission
			if (departmentID == DepartmentIDList[0])
			{
				return -1;
			}

			for (int i = 1; i < DepartmentIDList.Count; i++)
			{
				if (DepartmentIDList[i] == departmentID)
				{
					return DepartmentIDList[i - 1];
				}
			}

			// not found
			return -1;
		}
	}
}