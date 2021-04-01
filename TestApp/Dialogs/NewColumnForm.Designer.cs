
namespace TestApp.Dialogs
{
	partial class NewColumnForm
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
			this.label2 = new System.Windows.Forms.Label();
			this.columnPosition = new System.Windows.Forms.NumericUpDown();
			this.confirmRow = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.columnPosition)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 19);
			this.label2.TabIndex = 2;
			this.label2.Text = "Column Position:";
			// 
			// columnPosition
			// 
			this.columnPosition.Location = new System.Drawing.Point(134, 12);
			this.columnPosition.Name = "columnPosition";
			this.columnPosition.Size = new System.Drawing.Size(247, 25);
			this.columnPosition.TabIndex = 3;
			// 
			// confirmRow
			// 
			this.confirmRow.Location = new System.Drawing.Point(12, 43);
			this.confirmRow.Name = "confirmRow";
			this.confirmRow.Size = new System.Drawing.Size(369, 35);
			this.confirmRow.TabIndex = 4;
			this.confirmRow.Text = "Confirm";
			this.confirmRow.UseVisualStyleBackColor = true;
			this.confirmRow.Click += new System.EventHandler(this.confirmRow_Click);
			// 
			// NewColumnForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(393, 91);
			this.Controls.Add(this.confirmRow);
			this.Controls.Add(this.columnPosition);
			this.Controls.Add(this.label2);
			this.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "NewColumnForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "New Row";
			((System.ComponentModel.ISupportInitialize)(this.columnPosition)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button confirmRow;
		public System.Windows.Forms.NumericUpDown columnPosition;
	}
}