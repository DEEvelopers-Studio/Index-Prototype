using PropertyChanged;
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
using static Index_Prototype.Pages.Add_Teacher.AddTeacher;
using static Index_Prototype.Pages.Home.Accounts;

namespace Index_Prototype.Pages.Home
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// 
    [AddINotifyPropertyChangedInterface]
    public partial class Login : Page, INotifyPropertyChanged
    {
        public Login()
        {
            InitializeComponent();
        }
        private User _teacher;
        public User teacher { get { return _teacher; } private set { _teacher = value;teacherName.Text = value.getName(); }}
        Action onLoginSucess;
        Action onExit;

        public event PropertyChangedEventHandler PropertyChanged;

        public void setAccount(User teacher)
        {
            this.teacher = teacher;
        }
        internal void OnLoginSucess(Action value)
        {
            onLoginSucess = value;
        }
        internal void OnExit(Action value)
        {
            onExit = value;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            onExit.Invoke();
        }

        private void OnLogin(object sender, RoutedEventArgs e)
        {
            /* verify account and passowrd to database*/
            switch (DatabaseHelper.Login(teacher.uid, passwordBox.Password))
            {
                case DatabaseHelper.LoginState.SUCCESS:
                    onLoginSucess.Invoke();
                    break;
                case DatabaseHelper.LoginState.PASSWORDFAIL:
                    wrongPasswordHint.Visibility = Visibility.Visible;
                    break;
                case DatabaseHelper.LoginState.UIDFAIL:
                    break;
                case DatabaseHelper.LoginState.DBFAIL:
                    break;
            }
            
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            wrongPasswordHint.Visibility = Visibility.Hidden;
        }
    }
}
