using WpfChooser.Enums;

namespace WpfChooser.Interfaces
{
    public interface IMessageBox
    {
        Result Show(string text);

        Result Show(string text, string title);

        Result Show(string text, string title, Mode mode);

        void ChangeMode(Mode mode);

        Result CheckResult { get; }
    }
}