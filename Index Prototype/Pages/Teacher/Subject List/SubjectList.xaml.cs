using ControlzEx.Theming;
using Index_Prototype.Pages.Home;
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
using WpfApp2;
using static Index_Prototype.Pages.Home.Accounts;

namespace Index_Prototype.Pages.Subject_List
{
    /// <summary>
    /// Interaction logic for SubjectList.xaml
    /// </summary>
    public partial class SubjectList : Page, INotifyPropertyChanged
    {
        public class SubjectVM : Subject.Subject, INotifyPropertyChanged
        {
            public bool isSelected { get; set; } = false;

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public ObservableCollection<SubjectVM> subjects { get; set; } = new ObservableCollection<SubjectVM>();
        private bool? _isAllSelected { get; set; } = false;
        public bool? isAllSelected
        {
            get { return _isAllSelected; }
            set
            {
                _isAllSelected = value;
                deleteBtn.Visibility = value != false?Visibility.Visible:Visibility.Hidden;
                if (value == null) return;
                    for (int i = 0; i < subjects.Count; i++)
                    {
                        subjects[i].isSelected = (bool)value;
                    }
            }
        }
        public SubjectList()
        {
            InitializeComponent();
            DatabaseHelper.getSubjects().ForEach(subject =>
            {
                if (subject == null) return;
                subjects.Add(new SubjectVM() { attendanceOnStart = subject.attendanceOnStart,id = subject.id,defaultStudentSelection = subject.defaultStudentSelection,section = subject.section,title = subject.title});
            });
        }
        Action<Subject.Subject> onSelectSubject;
        public void OnSelectSubject(Action<Subject.Subject> action)
        {
            onSelectSubject = action;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void SelectedSubject(object sender, MouseButtonEventArgs e)
        {
            SubjectVM selectedSubj = (sender as Border)?.Tag as SubjectVM;
            if (selectedSubj == null) return;
            //MainWindow.MainNavigationService.Navigate(new Subject.SubjectView(selectedSubj));
            MainWindow.MainNavigationService.Navigate(new Uri($"Pages/Subject/SubjectView.xaml?id={selectedSubj.id}", UriKind.Relative));
            //onSelectSubject?.Invoke(selectedSubj);
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

        private void SearchChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
