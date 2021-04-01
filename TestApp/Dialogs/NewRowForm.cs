using System;
using System.Windows.Forms;

namespace TestApp.Dialogs
{
	public partial class NewRowForm : Form
	{
		public NewRowForm(Form1 mainForm)
		{
			InitializeComponent();

			rowPosition.Minimum = 0;
			rowPosition.Maximum = mainForm.customDataGridView.Rows.Count - 1;
		}

		private void confirmRow_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
