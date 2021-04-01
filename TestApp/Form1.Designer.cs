
namespace TestApp
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.customDataGridView = new System.Windows.Forms.DataGridView();
			this.btnAddRow = new System.Windows.Forms.Button();
			this.btnAddColumn = new System.Windows.Forms.Button();
			this.btnDeleteColumn = new System.Windows.Forms.Button();
			this.btnDeleteRow = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.customDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// customDataGridView
			// 
			this.customDataGridView.AllowUserToAddRows = false;
			this.customDataGridView.AllowUserToDeleteRows = false;
			this.customDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.customDataGridView.Location = new System.Drawing.Point(12, 12);
			this.customDataGridView.Name = "customDataGridView";
			this.customDataGridView.Size = new System.Drawing.Size(776, 379);
			this.customDataGridView.TabIndex = 0;
			// 
			// btnAddRow
			// 
			this.btnAddRow.Location = new System.Drawing.Point(12, 397);
			this.btnAddRow.Name = "btnAddRow";
			this.btnAddRow.Size = new System.Drawing.Size(383, 26);
			this.btnAddRow.TabIndex = 1;
			this.btnAddRow.Text = "New Row";
			this.btnAddRow.UseVisualStyleBackColor = true;
			this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
			// 
			// btnAddColumn
			// 
			this.btnAddColumn.Location = new System.Drawing.Point(12, 429);
			this.btnAddColumn.Name = "btnAddColumn";
			this.btnAddColumn.Size = new System.Drawing.Size(383, 26);
			this.btnAddColumn.TabIndex = 3;
			this.btnAddColumn.Text = "New Column";
			this.btnAddColumn.UseVisualStyleBackColor = true;
			this.btnAddColumn.Click += new System.EventHandler(this.btnAddColumn_Click);
			// 
			// btnDeleteColumn
			// 
			this.btnDeleteColumn.Location = new System.Drawing.Point(405, 429);
			this.btnDeleteColumn.Name = "btnDeleteColumn";
			this.btnDeleteColumn.Size = new System.Drawing.Size(383, 26);
			this.btnDeleteColumn.TabIndex = 5;
			this.btnDeleteColumn.Text = "Remove Column";
			this.btnDeleteColumn.UseVisualStyleBackColor = true;
			this.btnDeleteColumn.Click += new System.EventHandler(this.btnDeleteColumn_Click);
			// 
			// btnDeleteRow
			// 
			this.btnDeleteRow.Location = new System.Drawing.Point(405, 397);
			this.btnDeleteRow.Name = "btnDeleteRow";
			this.btnDeleteRow.Size = new System.Drawing.Size(383, 26);
			this.btnDeleteRow.TabIndex = 4;
			this.btnDeleteRow.Text = "Remove Row";
			this.btnDeleteRow.UseVisualStyleBackColor = true;
			this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
			// 
			// Form1
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(800, 467);
			this.Controls.Add(this.btnDeleteColumn);
			this.Controls.Add(this.btnDeleteRow);
			this.Controls.Add(this.btnAddColumn);
			this.Controls.Add(this.btnAddRow);
			this.Controls.Add(this.customDataGridView);
			this.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.customDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button btnAddRow;
		private System.Windows.Forms.Button btnAddColumn;
		private System.Windows.Forms.Button btnDeleteColumn;
		private System.Windows.Forms.Button btnDeleteRow;
		public System.Windows.Forms.DataGridView customDataGridView;
	}
}

