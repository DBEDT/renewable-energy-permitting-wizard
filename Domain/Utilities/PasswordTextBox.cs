using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace HawaiiDBEDT.Domain.Utilities
{
	public class PasswordTextBox : TextBox
	{
		public PasswordTextBox()
		{
			TextMode = TextBoxMode.Password;
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;

				Attributes["value"] = value;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Attributes["value"] = Text;
		}
	}
}
