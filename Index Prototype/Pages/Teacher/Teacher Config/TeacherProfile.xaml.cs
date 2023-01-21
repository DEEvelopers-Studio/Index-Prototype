using Index_Prototype.Directory;
using Index_Prototype.Pages.Teacher.ChangePassword;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace Index_Prototype.Pages.Teacher.Teacher_Config
{
    /// <summary>
    /// Interaction logic for TeacherProfile.xaml
    /// </summary>
    public partial class TeacherProfile : Page,INotifyPropertyChanged
    {
        public TeacherProfile()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword.ChangePassword changePassword = new ChangePassword.ChangePassword();
            changePassword.ShowDialog();

        }
        public DataTemplates.User teacher { get; set; } = AccountAuth.account;

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {

            
        }
        private bool isAllFilled()
        {
            bool isAllFilled = true;
            if (String.IsNullOrWhiteSpace(teacher.firstName)) isAllFilled = false;
            if (String.IsNullOrWhiteSpace(teacher.lastName)) isAllFilled = false;
            return isAllFilled;
        }

        private void PWBox_KeyDown(object sender, KeyEventArgs e)
        {
            PwNotMatchErrTxt.Visibility = Visibility.Hidden;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            bool allFilled = isAllFilled();
            FillErrTxt.Visibility = !allFilled ? Visibility.Visible : Visibility.Hidden;
            if (!allFilled) return;
            DatabaseHelper.UpdateTeacher(teacher);
            MessageBox.Show("Account Updated Successfully!");
        }
    }
}
