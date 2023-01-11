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
using System.Windows.Shapes;
using static Index_Prototype.Directory.DataTemplates;
using static Index_Prototype.Pages.Subject_List.SubjectList;

namespace Index_Prototype.Pages.Add_User
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window,INotifyPropertyChanged
    {


        private bool? _isAllSelected { get; set; } = false; 
        public bool? isAllSelected
        {
            get { return _isAllSelected; }
            set
            {
                _isAllSelected = value;
                if (value == null) return;
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].isSelected = (bool)value;
                }
            }
        }
        public string dataName { get; set; } = "User";
        public bool canCreate { get; set; }
        Action onCreate;
        public ObservableCollection<UserListVM> users { get; set; } = new ObservableCollection<UserListVM>();
        public List<User> selectedUsers = new List<User>();
        public AddUser(string dataName, List<User> users,List<User> exclude,Action onCreate = null)
        {

            InitializeComponent();
            this.dataName = dataName;
            this.onCreate = onCreate;
            canCreate = onCreate != null;
            users.ForEach((user) =>
            {
                if (exclude.Find((usr) => { return usr.uid == user.uid; }) == null) this.users.Add(new UserListVM(user));
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ItemToggle(object sender, RoutedEventArgs e)
        {
            int itemChecked = 0;
            foreach (var item in users)
            {
                if (item.isSelected) itemChecked++;
            }
            if (itemChecked == users.Count) isAllSelected = true;
            else if (itemChecked != 0) isAllSelected = null;
            else isAllSelected = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            selectedUsers.Clear();
            foreach (UserListVM user in users)
            {
                if(user.isSelected)
                selectedUsers.Add(user);
            }
            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            onCreate?.Invoke();
        }
    }
}
