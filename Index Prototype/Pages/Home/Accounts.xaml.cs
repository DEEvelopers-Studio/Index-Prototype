using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp2;
using static Index_Prototype.Directory.DataTemplates;

namespace Index_Prototype.Pages.Home
{
    /// <summary>
    /// Interaction logic for TeacherList.xaml
    /// </summary>
    public partial class Accounts : Page
    {
        
        public ObservableCollection<User> accounts { get; set; } = new ObservableCollection<User>();

        public Accounts()
        {
            InitializeComponent();
            
        }
        Action<User> onAccSelect;
        /// <summary>
        /// fires when an account gets selected
        /// </summary>
        /// <param name="uid">account id on the database</param>
        internal void OnAccountSelect(Action<User> account)
        {
            onAccSelect = account;
        }
        private void SelectedAccount(object sender, MouseButtonEventArgs e)
        {
            onAccSelect(((sender as ListViewItem)?.Content as User));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            accounts.Add(new User() { firstName = "Arvin John", lastName = "Suyat", middleName = "" });
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            DatabaseHelper.getTeachers().ForEach(teacher =>
            {
                accounts.Add(teacher);
            });
        }
    }
    
}
