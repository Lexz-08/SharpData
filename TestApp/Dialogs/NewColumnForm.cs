using System;
using System.Windows.Forms;

namespace TestApp.Dialogs
{
	public partial class NewColumnForm : Form
	{
		public NewColumnForm(Form1 mainForm)
		{
			InitializeComponent();

			columnPosition.Minimum = 0;
			columnPosition.Maximum = mainForm.customDataGridView.Columns.Count - 1;
		}

		private void confirmRow_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
