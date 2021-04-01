using SharpData;
using System;
using System.Windows.Forms;
using TestApp.Dialogs;

namespace TestApp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnAddRow_Click(object sender, EventArgs e)
		{
			NewRowForm newRowForm = new NewRowForm(this);
			if (newRowForm.ShowDialog() == DialogResult.OK)
			{
				DataGridManager dataManager = new DataGridManager(customDataGridView);
				dataManager.AddRow((int)newRowForm.rowPosition.Value, newRowForm.rowContent.Text);
			}
		}
		private void btnAddColumn_Click(object sender, EventArgs e)
		{
			NewColumnForm newColumnForm = new NewColumnForm(this);
			if (newColumnForm.ShowDialog() == DialogResult.OK)
			{
				DataGridManager dataManager = new DataGridManager(customDataGridView);
				dataManager.AddColumn((int)newColumnForm.columnPosition.Value, "New Column");
			}
		}
		private void btnDeleteRow_Click(object sender, EventArgs e)
		{

		}
		private void btnDeleteColumn_Click(object sender, EventArgs e)
		{

		}
	}
}
