using System.Windows;
using System.Windows.Input;
using WpfChooser.Enums;
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

        private Result result;

        public Result CheckResult
        {
            get
            {
                return result;
            }
        }

        public CustomMessageBox()
        {
            InitializeComponent();

            bCancel.Content = cancelCaption;
            bNo.Content = noCaption;
            bYes.Visibility = Visibility.Visible;
            result = Result.Default;
            Title = string.Empty;
        }

        public Result Show(string text)
        {
            return Show(text, "");
        }

        public Result Show(string text, string title)
        {
            return Show(text, title, Mode.Ok);
        }

        public Result Show(string text, string title, Mode mode)
        {//todo do it smart!!!(going about checing mode and other
            textBlock.Text = text;
            if (title == null)
                title = string.Empty;
            Title = title;
            ChangeMode(mode);
            this.ShowDialog();
            return result;
        }

        public void ChangeMode(Mode mode)
        {
            switch (mode)
            {
                case Mode.YesNo:
                    bYes.Content = yesCaption;
                    bNo.Visibility = Visibility.Visible;
                    bCancel.Visibility = Visibility.Hidden;
                    break;

                case Mode.YesNoCancel:
                    bYes.Content = yesCaption;
                    bNo.Visibility = Visibility.Visible;
                    bCancel.Visibility = Visibility.Visible;
                    break;

                case Mode.OkCancel:
                    bYes.Content = okCaption;
                    bNo.Visibility = Visibility.Hidden;
                    bCancel.Visibility = Visibility.Visible;
                    break;

                case Mode.Ok:
                    bYes.Content = okCaption;
                    bNo.Visibility = Visibility.Hidden;
                    bCancel.Visibility = Visibility.Hidden;
                    break;

                default:
                    break;
            }
        }

        private void bYes_Click(object sender, RoutedEventArgs e)
        {
            result = Result.Yes;
            Close();
        }

        private void bNo_Click(object sender, RoutedEventArgs e)
        {
            result = Result.No;
            Close();
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            result = Result.Cancel;
            Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                result = Result.Yes;
                Close();
            }
            else if (e.Key == Key.Escape)
            {
                result = Result.Cancel;
                Close();
            }
        }
    }
}