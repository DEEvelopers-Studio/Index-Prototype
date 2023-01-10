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
            onAccSelect((sender as System.Windows.Controls.ListViewItem)?.Content as User);
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            accounts.Clear();
            DatabaseHelper.getTeachers().ForEach(teacher =>
            {
                accounts.Add(teacher);
            });

        }
        private void SignUpBtn_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Add_Teacher.AddTeacher teacher = new Add_Teacher.AddTeacher();
            bool? dialogResult = teacher?.ShowDialog();
            switch (dialogResult)
            {
                case true:
                    LoadData();
                    break;
                case false:
                    // User canceled dialog box
                    break;
                default:
                    // Indeterminate
                    break;
            }
        }
    }
    
}
