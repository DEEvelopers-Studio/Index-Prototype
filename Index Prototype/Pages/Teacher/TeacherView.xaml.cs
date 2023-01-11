using Index_Prototype.Directory;
using Index_Prototype.Pages.Home;
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
using static Index_Prototype.Directory.DataTemplates;

namespace Index_Prototype.Pages.Teacher
{
    /// <summary>
    /// Interaction logic for TeacherView.xaml
    /// </summary>
    public partial class TeacherView : Page, INotifyPropertyChanged
    {
        public User teacher { get; set; }

        public TeacherView()
        {
            //teacher = user;
            InitializeComponent();
            //button route check if active
            SubjectNavButton.navigationService = routerView.NavigationService;
            //SettingsNavButton.navigationService = routerView.NavigationService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            AccountAuth.Logout();
        }
    }
}
