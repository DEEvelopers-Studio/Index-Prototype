using Index_Prototype.Directory;
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
using System.Windows.Shapes;
using WpfApp2;

namespace Index_Prototype.Pages.Teacher.ChangePassword
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window,INotifyPropertyChanged
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!isAllFilled()) { MessageBox.Show("Fill up all feilds!");return; }
            if(newPwBox.Password.Length < 8) { MessageBox.Show("Minimun Password Length is 8");return; }
            if(newPwBox.Password != confNewPwBox.Password) { MessageBox.Show("Passwords do not match");return; }
            //check if password match
            if(oldPwBox.Password == newPwBox.Password) { MessageBox.Show("the old password is the same as the new one!");return; }
            string accUid = AccountAuth.account.uid;
            AccountAuth.ChangePassword( oldPwBox.Password,newPwBox.Password, () =>
            {
                MessageBox.Show("Password Successfully Changed!");
                Close();
            }, (err) =>
            {
                switch (err)
                {
                    case LoginExeption.PASSWORDFAIL:
                        MessageBox.Show("old password missmatch");
                        break;
                    case LoginExeption.UIDFAIL:
                        MessageBox.Show("User does not exist");
                        break;
                    case LoginExeption.DBFAIL:
                        MessageBox.Show("Database Failure");
                        break;
                    default:
                        break;
                }
                Close();
            });
        }
        private bool isAllFilled()
        {
            bool isAllFilled = true;
            if (String.IsNullOrWhiteSpace(oldPwBox.Password)) isAllFilled = false;
            if (String.IsNullOrWhiteSpace(newPwBox.Password)) isAllFilled = false;
            if (String.IsNullOrWhiteSpace(confNewPwBox.Password)) isAllFilled = false;
            return isAllFilled;
        }
    }
}
