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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }
        Action<Teacher> onLoginSucess;
        Action onExit;
        internal void OnLoginSucess(Action<Teacher> value)
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
    }
}
