using System.ComponentModel;
namespace Index_Prototype.Directory.ViewModels
{

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
