using System.ComponentModel;
using System.Reflection;

namespace SchoolsApp.Infrastructure
{
    [Obfuscation]
    public interface ISelectable : INotifyPropertyChanged
    {
        bool IsChecked { get; set; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
    }
}
