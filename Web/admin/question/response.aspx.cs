using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;

using System.Web.UI.HtmlControls;
using HawaiiDBEDT.Domain.Enumerations;

namespace HawaiiDBEDT.Web.Admin
{
    using log4net;
    using utils;

    public partial class ResponseDetail : System.Web.UI.Page
	{
		int responseID;
		Response response;
		Question question;
		int questionID;

		private const string SEPARATOR = "_";
		private const string VALUE_PREFIX = "fieldvalue";
		private bool showSubQuestions = true;
        ILog log = LogManager.GetLogger(typeof(ResponseDetail));

		protected void Page_Load(object sender, EventArgs e)
		{
			GetResponse();
			if (!IsPostBack)
			{
				DisplayQuestions();
				DisplayPermitSets();
				DisplayResponse();
				DisplaySelectedPermitSets();
			}
			else
			{
				if (permitSetIDToDelete.Value != null)
				{
					RemoveSelectedPermit(permitSetIDToDelete.Value);
				}
			}
		}

		private void GetResponse()
		{
			questionID = int.Parse(Request.QueryString["qid"]);
			question = Data.Question.GetQuestion(questionID);
			showSubQuestions = (question.QuestionTypeID != (int)QuestionTypeEnum.Capacity);
			lblQuestion.Text = Data.Question.GetQuestion(questionID).Name;

			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				responseID = int.Parse(Request.QueryString["id"]);
				response = Data.Response.GetResponse(responseID);
			}
			else
			{
				responseID = 0;
				response = new Response();
				response.QuestionID = questionID;
			}
		}

		private void DisplayResponse()
		{
			if (responseID != 0)
			{
				txtName.Text = response.Name;
				reDescription.Text = HtmlEncoder.BetterHtmlDecode(response.Description);

				if (response.IsEndPoint)
				{
					rbEndPoint.SelectedIndex = 0;
					reEndPoint.Text = HtmlEncoder.BetterHtmlDecode(response.EndPointMessage);
					trPermits.Visible = false;
					trSubQuestion.Visible = false;
				}
				else
				{
					rbEndPoint.SelectedIndex = 1;
					if (response.SubQuestionID.HasValue)
					{
                        rcbSubQuestion.SelectedIndex = rcbSubQuestion.Items.IndexOf(rcbSubQuestion.Items.FindByValue(response.SubQuestionID.ToString()));
					}

					trSubQuestion.Visible = showSubQuestions;
				}
			}
			else
			{
				trSubQuestion.Visible = showSubQuestions;
			}
		}

		private void DisplayPermitSets()
		{
			List<PermitSet> permitSets = Data.PermitSet.GetPermitSets();

			rcbPermitSet.DataSource = permitSets;
			rcbPermitSet.DataBind();

			rcbPermitSet.Items.Insert(0, new ListItem("< select >", ""));
		}

		private void DisplaySelectedPermitSets()
		{
			if (responseID != 0)
			{
				if (response.RequiresPermits)
				{
					foreach (PermitSet permitSet in response.UniquePermitSets)
					{
						DisplayPermit(permitSet.Name, permitSet.ID.ToString()); 
					}
				}
			}
		}

		private void DisplayQuestions()
		{
			if (showSubQuestions)
			{
				List<Question> questions = Data.Question.GetQuestions();
				questions.RemoveAll(i => i.ID == questionID);		   // Remove the current question

				// Also remove any questions referenced by other responses
				// Remove any questions which lists the current question as a sub-question

				rcbSubQuestion.DataSource = questions;
				rcbSubQuestion.DataBind();

				rcbSubQuestion.Items.Insert(0, new ListItem("< select >", ""));
			}
		}

		protected void rbEndPoint_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (rbEndPoint.SelectedIndex == 0)
			{
				trPermits.Visible = false;
				trSubQuestion.Visible = false;
			}
			else
			{
				trPermits.Visible = true;
				trSubQuestion.Visible = showSubQuestions;
			}
		}

		protected void rcbPermitSet_IndexChanged(object sender, EventArgs e)
		{
			string permitSetID = rcbPermitSet.SelectedValue;
			string permitSetName = rcbPermitSet.SelectedItem.Text;

			if (permitSetID != "")
			{
				DisplayPermit(permitSetName, permitSetID);
			}
		}

		private void DisplayPermit(string permitSetName, string psId)
		{
			int counter = 0;
		    string permitSetID = Server.HtmlEncode(psId);
			PlaceHolder panel = new PlaceHolder();
			panel.ID = "csPanel" + SEPARATOR + permitSetID;
			MyDCP.Controls.Add(panel);

			// Add header
			Label myLiteral2 = new Label();
			myLiteral2.ID = "tableRow" + permitSetID + SEPARATOR + counter++.ToString();
			panel.Controls.Add(myLiteral2);
			myLiteral2.Text = "<tr><td>" + permitSetName + " : <br />(";

			// Add link to remove field
			LinkButton linkButton = new LinkButton();
			linkButton.ID = "removeButton" + permitSetID + SEPARATOR + counter++.ToString();
			panel.Controls.Add(linkButton);
			linkButton.OnClientClick = "javascript:deleteField('" + permitSetID + "');";
			linkButton.Text = "Remove";
			linkButton.CausesValidation = false;

			Label myLiteral3 = new Label();
			myLiteral3.ID = "tableRow" + permitSetID + SEPARATOR + counter++.ToString();
			panel.Controls.Add(myLiteral3);
			myLiteral3.Text = ")</td><td>";
												  
			string checkboxFieldID = VALUE_PREFIX + SEPARATOR + permitSetID + SEPARATOR + "options";

			// Add select all link
			HyperLink hlSelectAll = new HyperLink();
			hlSelectAll.ID = "selectAll" + permitSetID + SEPARATOR +counter++.ToString();
			panel.Controls.Add(hlSelectAll);
			hlSelectAll.Attributes.Add("onclick", "checkAllLocationFields('" + checkboxFieldID + "');return false;");
			hlSelectAll.Text = "Select All";
			hlSelectAll.NavigateUrl = "#";

			Label myLiteral5 = new Label();
			myLiteral5.ID = "tableRowField" + permitSetID + SEPARATOR + counter++.ToString();
			panel.Controls.Add(myLiteral5);
			myLiteral5.Text = "</td><td>";

			// Add checkboxes
			//skmCheckBoxList cbOptions = new skmCheckBoxList();
			CheckBoxList cbOptions = new CheckBoxList();
			cbOptions.ID = checkboxFieldID;

			panel.Controls.Add(cbOptions);

			cbOptions.DataSource = HawaiiDBEDT.Data.DataAccess.LocationList;
			cbOptions.DataTextField = "Name";
			cbOptions.DataValueField = "ID";
			cbOptions.DataBind();

			cbOptions.CssClass = "noStyle";

			cbOptions.RepeatDirection = RepeatDirection.Horizontal;

			// Add selected options
			if (!IsPostBack && responseID != 0)
			{
				List<Location> selectedLocations = Data.ResponsePermitSet.GetLocationsForResponsePermitSet(responseID, int.Parse(permitSetID));
				foreach (Location location in selectedLocations)
				{
					try
					{
						cbOptions.Items.FindByValue(location.ID.ToString()).Selected = true;
					}
					catch (Exception ex)
					{
					    log.Error(ex);
					}
				}
			}

			Label myLiteral6= new Label();
			myLiteral6.ID = "tableRowEnd" + permitSetID + SEPARATOR + counter++.ToString();
			panel.Controls.Add(myLiteral6);
			myLiteral6.Text = "</td></tr>\n";

			pnlPermitSet.Visible = true;

			// Remove the selected field from list so that it can't be used again
            rcbPermitSet.Items.Remove(rcbPermitSet.Items.FindByValue(permitSetID));
			rcbPermitSet.ClearSelection();

			TogglePermitSetControls();
		}

		// Need to re-add the onclick attributes to the checkbox list items as these are not persisted during post back
		protected void DCP_PostRestore(object sender, System.EventArgs e) 
		{
			foreach (Control controlMain in MyDCP.Controls)
			{
				if (controlMain.ID.StartsWith("csPanel"))
				{
					foreach (Control control in controlMain.Controls)
					{
						if (control.ID.StartsWith(VALUE_PREFIX + SEPARATOR))
						{
							if (control.GetType().Name.Equals("CheckBoxList"))
							{
								string checkboxFieldID = control.ID;

								CheckBoxList cbOptions = ((CheckBoxList)control);

								if (MyDCP.FindControl(checkboxFieldID + "_0") != null)
								{
									((HtmlInputControl)MyDCP.FindControl(checkboxFieldID + "_0")).Attributes.Add("onclick", "checkAllLocationFields('" + checkboxFieldID + "_0', '" + checkboxFieldID + "');");

									// Deselecting any of the locations should deselect the All option - skip the first checkbox
									for (int i = 1; i < cbOptions.Items.Count; i++)
									{
										((HtmlInputControl)MyDCP.FindControl(checkboxFieldID + "_" + i.ToString())).Attributes.Add("onclick", "uncheckAllfield('" + checkboxFieldID + "_0');");
									}

								}
							}
						}
					}
				}
			}
		}

		private void TogglePermitSetControls()
		{
			if (rcbPermitSet.Items.Count <= 1)
			{
				rcbPermitSet.Visible = false;
			}
			else
			{
				rcbPermitSet.Visible = true;
			}

			if (MyDCP.Controls.Count == 0)
			{
				pnlPermitSet.Visible = false;
			}
			else
			{
				pnlPermitSet.Visible = true;
			}
		}

		private void RemoveSelectedPermit(string permitSetID)
		{
			foreach (Control control in MyDCP.Controls)
			{
				if (control.Controls.Count > 1)
				{
					if (control.ID.StartsWith("csPanel"))
					{
						string[] fieldArray = new string[3];

						fieldArray = control.ID.Split('_');
						string id = fieldArray[1];

						if (id == permitSetID)
						{
							MyDCP.Controls.Remove(control);

							// need to add this field back into available dropdownlist
							PermitSet permitSet = Data.PermitSet.GetPermitSet(int.Parse(permitSetID));
							rcbPermitSet.Items.Add(new ListItem(permitSet.Name, permitSetID));

							permitSetIDToDelete.Value = "";
							break;
						}
					}
				}
			}

			TogglePermitSetControls();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("edit.aspx?id=" + questionID, true);
		}

		private bool UpdateResponse()
		{
			Page.Validate();

			if (Page.IsValid)
			{
				response.Name = txtName.Text;
				response.Description = reDescription.Text;
				response.IsEndPoint = (rbEndPoint.SelectedValue == "true");
				response.EndPointMessage = (response.IsEndPoint ? reEndPoint.Text : "");
				response.QuestionID = questionID;
				response.PermitSets = new List<ResponsePermitSet>();

				UpdatePermitSets();

				if (rcbSubQuestion.SelectedIndex > 0)
				{
					response.SubQuestionID = int.Parse(rcbSubQuestion.SelectedValue);
				}
				else {
					response.SubQuestionID = null;
				}

				if (responseID != 0)
				{
					Data.Response.UpdateResponse(response);
				}
				else
				{
					responseID = Data.Response.AddResponse(response);
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		private void UpdatePermitSets()
		{
			if (response.IsEndPoint)
			{
				response.PermitSets = new List<ResponsePermitSet>();
			}
			else
			{
				foreach (Control controlMain in MyDCP.Controls)
				{
					if (controlMain.Controls.Count > 1)
					{
						// We have the panel here:
						// Determine the type
						if (controlMain.ID.StartsWith("csPanel"))
						{
							string[] fieldArray = new string[2];

							fieldArray = controlMain.ID.Split('_');
							int permitSetID = int.Parse(fieldArray[1]);

							foreach (Control control in controlMain.Controls)
							{
								if (control.GetType().Name.Equals("CheckBoxList"))
								{
									CheckBoxList cbList = (CheckBoxList)control;
									GetLocationsForPermitSets(permitSetID, cbList.Items);
								}
							}
						}
					}
				}
			}

			response.RequiresPermits = (response.PermitSets.Count > 0);
		}

		protected void GetLocationsForPermitSets(int permitSetID, ListItemCollection Items)
		{
			foreach (ListItem item in Items)
			{
				if (item.Selected && item.Value != "")
				{
					response.PermitSets.Add(new ResponsePermitSet(permitSetID, int.Parse(item.Value)));
				}
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			if (UpdateResponse())
			{
				Response.Redirect("edit.aspx?id=" + questionID.ToString(), true);
			}
		}

		protected void btnSubmitAnother_Click(object sender, EventArgs e)
		{
			if (UpdateResponse())
			{
				Response.Redirect("response.aspx?qid=" + questionID.ToString(), true);
			}
		}
	}
}