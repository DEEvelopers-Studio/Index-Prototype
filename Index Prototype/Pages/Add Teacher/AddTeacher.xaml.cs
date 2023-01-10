
using Index_Prototype.Directory;
using Index_Prototype.Directory.ViewModels;
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

namespace Index_Prototype.Pages.Add_Teacher
{
    /// <summary>
    /// Interaction logic for AddTeacher.xaml
    /// </summary>
    public partial class AddTeacher : Window, INotifyPropertyChanged
    {
        public DataTemplates.User teacher {get;set;} = new DataTemplates.User();
        public AddTeacher()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {

            bool allFilled = isAllFilled();
            FillErrTxt.Visibility = !allFilled ? Visibility.Visible : Visibility.Hidden;
            if (!allFilled) return;
            if (!PasswordMatches()) return;
            bool PwShort = PWBox.Password.Length < 8;
            PwMinErrTxt.Visibility = PwShort ? Visibility.Visible : Visibility.Hidden;
            if (PwShort) return;
            DatabaseHelper.PutTeacher(new DataTemplates.User(Guid.NewGuid().ToString(), FNameBox.Text, LNameBox.Text, MNameBox.Text), PWBox.Password);
            MessageBox.Show("Account Added Successfully!");
            DialogResult = true;
            Close();
        }
        private bool isAllFilled()
        {
            bool isAllFilled = true;
            if (String.IsNullOrWhiteSpace(FNameBox.Text)) isAllFilled = false; 
            if (String.IsNullOrWhiteSpace(LNameBox.Text)) isAllFilled = false; 
            if (String.IsNullOrWhiteSpace(MNameBox.Text)) isAllFilled = false;
            if (String.IsNullOrWhiteSpace(PWBox.Password)) isAllFilled = false;
            if (String.IsNullOrWhiteSpace(PWConfirmBox.Password)) isAllFilled = false; 
            return isAllFilled;
        }
        private bool PasswordMatches()
        {
            bool passwordMatch = PWBox.Password.Equals(PWConfirmBox.Password);
            PwNotMatchErrTxt.Visibility = !passwordMatch?Visibility.Visible:Visibility.Hidden;
            return passwordMatch;
        }

        private void PWBox_KeyDown(object sender, KeyEventArgs e)
        {
            PwNotMatchErrTxt.Visibility = Visibility.Hidden;
        }
    }
}
