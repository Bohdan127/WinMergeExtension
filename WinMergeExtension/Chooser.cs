using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMergeExtension
{
	public partial class Chooser : Form
	{
		public string SelectedElement { get; private set; }


		public Chooser(List<string> branches)
		{
			InitializeComponent();
			listBox1.Items.AddRange(branches.ToArray());						
		}

		void bChoose_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		void bCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SelectedElement = listBox1.SelectedItem.ToString();
		}
	}
}
