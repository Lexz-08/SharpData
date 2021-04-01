
namespace TestApp.Dialogs
{
	partial class NewRowForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.rowContent = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.rowPosition = new System.Windows.Forms.NumericUpDown();
			this.confirmRow = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.rowPosition)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(92, 19);
			this.label1.TabIndex = 0;
			this.label1.Text = "Row Content:";
			// 
			// rowContent
			// 
			this.rowContent.Location = new System.Drawing.Point(114, 12);
			this.rowContent.Name = "rowContent";
			this.rowContent.Size = new System.Drawing.Size(267, 25);
			this.rowContent.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 19);
			this.label2.TabIndex = 2;
			this.label2.Text = "Row Position:";
			// 
			// rowPosition
			// 
			this.rowPosition.Location = new System.Drawing.Point(114, 43);
			this.rowPosition.Name = "rowPosition";
			this.rowPosition.Size = new System.Drawing.Size(267, 25);
			this.rowPosition.TabIndex = 3;
			// 
			// confirmRow
			// 
			this.confirmRow.Location = new System.Drawing.Point(12, 74);
			this.confirmRow.Name = "confirmRow";
			this.confirmRow.Size = new System.Drawing.Size(369, 35);
			this.confirmRow.TabIndex = 4;
			this.confirmRow.Text = "Confirm";
			this.confirmRow.UseVisualStyleBackColor = true;
			this.confirmRow.Click += new System.EventHandler(this.confirmRow_Click);
			// 
			// NewRowForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(393, 121);
			this.Controls.Add(this.confirmRow);
			this.Controls.Add(this.rowPosition);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.rowContent);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "NewRowForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "New Row";
			((System.ComponentModel.ISupportInitialize)(this.rowPosition)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button confirmRow;
		public System.Windows.Forms.TextBox rowContent;
		public System.Windows.Forms.NumericUpDown rowPosition;
	}
}