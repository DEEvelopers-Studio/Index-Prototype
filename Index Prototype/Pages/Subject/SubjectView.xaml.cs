using Index_Prototype.Directory;
using System;
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
        public static SubjectView Instance;
        public DataTemplates.Subject subject { get; set; }
        public SubjectView()
        {
            Instance = this;
            InitializeComponent();

            StudentsNavButton.navigationService = routerView.NavigationService;
            ConfigNavButton.navigationService = routerView.NavigationService;
            TeacherNavButton.navigationService = routerView.NavigationService;
        }
        //public SubjectView(DataTemplates.Subject subject)
        //{
        //    InitializeComponent();
        //    StudentsNavButton.navigationService = routerView.NavigationService;
        //    ConfigNavButton.navigationService = routerView.NavigationService;
        //    TeacherNavButton.navigationService = routerView.NavigationService;
        //    this.subject = subject;
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.subject = DatabaseHelper.getSubject(NavigationHelper.getParams(MainWindow.MainNavigationService)["SubjectId"]);
        }

        private void TeacherButton_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.MainNavigationService.Navigate(new Teacher.TeacherView());
        }

    }
    
}
