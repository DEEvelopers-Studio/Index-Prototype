using System;
using System.Collections.Generic;
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
using static Index_Prototype.Directory.DataTemplates;

namespace Index_Prototype.Pages.Subject
{
    /// <summary>
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class SubjectView : Page,INotifyPropertyChanged
    {
        public Subject subject { get; set; }

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
