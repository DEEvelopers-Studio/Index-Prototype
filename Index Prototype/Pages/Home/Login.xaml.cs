using Index_Prototype.Directory;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp2;
using static Index_Prototype.Directory.DataTemplates;

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
            LoginAccount();


        }
        private void LoginAccount()
        {
            AccountAuth.Login(teacher.uid, passwordBox.Password, (User account) =>
            {
                onLoginSucess.Invoke();
            }, (LoginExeption err) =>
            {
                switch (err)
                {
                    case LoginExeption.PASSWORDFAIL:
                        wrongPasswordHint.Visibility = Visibility.Visible;
                        break;
                    case LoginExeption.UIDFAIL:
                        MessageBox.Show("User does not Exist", "ok");
                        break;
                    case LoginExeption.DBFAIL:
                        MessageBox.Show("Problem Authenticating, please try again", "ok");
                        break;
                }
            });


        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            wrongPasswordHint.Visibility = Visibility.Hidden;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                LoginAccount();
            }
        }
    }
}
