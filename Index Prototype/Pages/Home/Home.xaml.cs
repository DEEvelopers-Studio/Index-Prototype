using ControlzEx.Standard;
using Index_Prototype.Pages.Subject_List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
                NavigationService.Navigate(new SubjectList());
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
