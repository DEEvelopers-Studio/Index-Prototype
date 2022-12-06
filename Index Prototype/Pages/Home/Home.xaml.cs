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
using static Index_Prototype.Pages.Add_Teacher.AddTeacher;

namespace Index_Prototype.Pages.Home
{
    /// <summary>
    /// Interaction logic for TeacherLogin.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            Accounts Accounts = new Accounts();
            routerView.Navigate(Accounts);
            Accounts.OnAccountSelect(acc =>
            {
                Login login = new Login();
                routerView.Navigate(login);
                login.OnExit(() =>
                {
                    routerView.Navigate(Accounts);
                });
            });
        }
    }
}
