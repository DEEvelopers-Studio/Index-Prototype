using System;
using System.Security;
using System.Windows.Controls;
using System.Windows.Navigation;
using static Index_Prototype.Directory.DataTemplates;

namespace Index_Prototype.Pages.Home
{
    /// <summary>
    /// Interaction logic for TeacherLogin.xaml
    /// </summary>
    public partial class Home : Page
    {
        private User account;
        private SecureString password;
        public Home()
        {
            InitializeComponent();
            Accounts Accounts = new Accounts();
            Login login = new Login();
            login.OnExit(() =>
            {
                routerView.Navigate(Accounts);
            });
            login.OnLoginSucess(() =>
            {
                MainWindow.MainNavigationService.Navigate(new Uri($"Pages/Teacher/TeacherView.xaml?uid={account.uid}",UriKind.Relative), account);
            });
            routerView.Navigate(Accounts);
            Accounts.OnAccountSelect(account =>
            {
                this.account = account;
                login.setAccount(account);
                routerView.Navigate(login);
            });
        }
    }
}
