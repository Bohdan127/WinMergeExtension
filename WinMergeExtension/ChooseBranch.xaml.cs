using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinMergeExtension
{
	/// <summary>
	/// Interaction logic for ChooseBranch.xaml
	/// </summary>
	public partial class ChooseBranch : UserControl
	{
		public List<string> Branches {get; set;}

		public string SelectedBranch { get; set; }

		public ChooseBranch()
		{
			InitializeComponent();

			Branches = new List<string>();
			SelectedBranch = string.Empty;
		}
	}
}
