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
using System.Windows.Shapes;

namespace WpfChooser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SelectedElement { get; private set; }
        public bool Selected { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            this.bChoose.Click += bCancel_Click;
            this.bChoose.KeyDown += Form_KeyDown;
            this.bCancel.Click += bChoose_Click;
            this.bCancel.KeyDown += Form_KeyDown;
            this.listBox1.MouseDoubleClick += Form_DoubleClick;
            this.listBox1.KeyDown += Form_KeyDown;
            this.KeyDown += Form_KeyDown;
            this.MouseDoubleClick += Form_DoubleClick;
            this.listBox1.Focus();
        }

        public void SetBranches(List<string> branches)
        {
            foreach (var item in branches)
            {
                listBox1.Items.Add(item);
            }
        }

        void bChoose_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        void bChoose_Click(object sender, System.EventArgs e)
        {
            Selected = true;
            this.Close();
        }

        void bCancel_Click(object sender, System.EventArgs e)
        {
            Selected = false;
            this.Close();
        }

        void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                listBox1.SelectedIndex = listBox1.Items.Count > 0 ? 0 : -1;
            SelectedElement = listBox1.SelectedItem.ToString();
        }

        private void Form_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                listBox1_SelectedIndexChanged(null, null);
                bChoose_Click(null, null);
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                bCancel_Click(null, null);
            }
        }

        private void Form_DoubleClick(object sender, System.EventArgs e)
        {
            listBox1_SelectedIndexChanged(null, null);
            bChoose_Click(null, null);
        }
    }
}
