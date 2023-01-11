using Index_Prototype.Pages.Home;
using Index_Prototype.Pages.Student_Info;
using System;
using System.Collections.Generic;
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

namespace Index_Prototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Load();
        }
        async void Load()
        {
            DatabaseHelper.Initialize(async connection =>
            {
                await Task.Delay(1000);
                InitializeComponent();
            });
        }
        public static NavigationService MainNavigationService;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            MainNavigationService = routerView.NavigationService;
            //MainNavigationService.LoadCompleted += (object sndr, NavigationEventArgs a) =>
            //{
            //    string str = (string)a.ExtraData;
            //    MessageBox.Show(str);
            //};
            //    navigationService.Navigate(new Home());
        }
        public void CloseAllWindow()
        {
            StudentInfo.Instance?.Close();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            CloseAllWindow();
        }
    }
}
