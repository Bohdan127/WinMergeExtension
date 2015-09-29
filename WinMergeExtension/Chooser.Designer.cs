using System.Windows.Forms;
namespace WinMergeExtension
{
	partial class Chooser
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bChoose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(152, 148);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(187, 120);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += bCancel_Click;
            // 
            // bChoose
            // 
            this.bChoose.Location = new System.Drawing.Point(187, 149);
            this.bChoose.Name = "bChoose";
            this.bChoose.Size = new System.Drawing.Size(75, 23);
            this.bChoose.TabIndex = 2;
            this.bChoose.Text = "Choose";
            this.bChoose.UseVisualStyleBackColor = true;
            this.bChoose.Click += bChoose_Click;
            // 
            // Chooser
            // 
            this.ClientSize = new System.Drawing.Size(274, 188);
            this.Controls.Add(this.bChoose);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Chooser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose branch for comparing";
            this.ResumeLayout(false);

		}
		#endregion

		private ListBox listBox1;
		private Button bCancel;
		private Button bChoose;
	}
}