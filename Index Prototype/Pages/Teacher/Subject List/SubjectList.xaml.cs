using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static Index_Prototype.Pages.Home.Accounts;

namespace Index_Prototype.Pages.Subject_List
{
    /// <summary>
    /// Interaction logic for SubjectList.xaml
    /// </summary>
    public partial class SubjectList : Page, INotifyPropertyChanged
    {
        public class SubjectVM : Subject.Subject
        {
            public bool isSelected { get; set; } = false;
        }
        public ObservableCollection<SubjectVM> subjects { get; set; } = new ObservableCollection<SubjectVM>() { new SubjectVM() {isSelected = true,title="Math",section="CS301"} };
        private bool? _isAllSelected { get; set; } = false;
        public bool? isAllSelected
        {
            get { return _isAllSelected; }
            set
            {
                if (value != null)
                {
                    for (int i = 0; i < subjects.Count; i++)
                    {
                        subjects[i].isSelected = (bool)value;
                    }
                    
                }
                _isAllSelected = value;
            }
        }

        public SubjectList()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SelectedSubject(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(((sender as Border)?.Content as SubjectVM)?.isSelected+"");
            subjects.Add(new SubjectVM() { isSelected = true, title = "Math", section = "CS301" });
        }

        private void ItemToggle(object sender, RoutedEventArgs e)
        {
            int itemChecked = 0;
            foreach (var item in subjects)
            {
                if (item.isSelected) itemChecked++;
            }
            if (itemChecked == subjects.Count) isAllSelected = true;
            else if (itemChecked != 0) isAllSelected = null;
            else isAllSelected = false;
        }

    }
}
