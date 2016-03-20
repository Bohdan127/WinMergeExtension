using System.Windows;
using WpfChooser.Interfaces;

namespace WpfChooser
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window, IMessageBox
    {
        public const string okCaption = "Ok";
        public const string yesCaption = "Yes";
        public const string noCaption = "No";
        public const string cancelCaption = "Cancel";

        public CustomMessageBox()
        {
            InitializeComponent();
        }
    }
}
