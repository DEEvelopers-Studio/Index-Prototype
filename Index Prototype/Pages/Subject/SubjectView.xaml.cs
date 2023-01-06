using Index_Prototype.Directory;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using WpfApp2;

namespace Index_Prototype.Pages.Subject
{
    /// <summary>
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class SubjectView : Page,INotifyPropertyChanged
    {
        public Subject subject { get; set; }
        public SubjectView()
        {

            InitializeComponent();

            StudentsNavButton.navigationService = routerView.NavigationService;
            ConfigNavButton.navigationService = routerView.NavigationService;
            TeacherNavButton.navigationService = routerView.NavigationService;
        }
        public SubjectView(Subject subject)
        {
            InitializeComponent();
            StudentsNavButton.navigationService = routerView.NavigationService;
            ConfigNavButton.navigationService = routerView.NavigationService;
            TeacherNavButton.navigationService = routerView.NavigationService;
            this.subject = subject;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.subject = DatabaseHelper.getSubject(NavigationHelper.getParams(MainWindow.MainNavigationService)["id"]);
        }

        private void TeacherButton_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.MainNavigationService.Navigate(new Teacher.TeacherView());
        }
    }
    public class Subject
    {
        public enum StudentSelect { Next,Random,RandomSequential}
        public string id { get; set; }
        public string title { get; set; }
        public string section { get; set; }
        public StudentSelect defaultStudentSelection { get; set; }
        public bool attendanceOnStart { get; set; }
    }
}
