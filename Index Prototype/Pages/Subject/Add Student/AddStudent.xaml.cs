using Index_Prototype.Directory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using static Index_Prototype.Directory.DataTemplates;

namespace Index_Prototype.Pages.Subject.Add_Student
{
    /// <summary>
    /// Interaction logic for AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        public DataTemplates.Student student { get; set; }
        public AddStudent()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        private bool isAllFilled()
        {
            bool isAllFilled = true;
            if (String.IsNullOrWhiteSpace(FNameBox.Text)) isAllFilled = false;
            if (String.IsNullOrWhiteSpace(LNameBox.Text)) isAllFilled = false;
            if (String.IsNullOrWhiteSpace(MNameBox.Text)) isAllFilled = false;
            if (String.IsNullOrWhiteSpace(StudUidBox.Text)) isAllFilled = false;
            FillErrTxt.Visibility = !isAllFilled ? Visibility.Visible : Visibility.Collapsed;
            return isAllFilled;
        }

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            bool minuiderr = StudUidBox.Text.Length != 11;
            UidMinErrTxt.Visibility = minuiderr ? Visibility.Visible:Visibility.Collapsed;
            if (!isAllFilled()) return;
            if (minuiderr) return;
            student = new Student() { firstName = FNameBox.Text,lastName = LNameBox.Text,middleName = MNameBox.Text,section = SectionBox.Text,uid = StudUidBox.Text };
            bool uidExists = DatabaseHelper.getStudent(StudUidBox.Text) != null;
            UidExists.Visibility = uidExists ? Visibility.Visible : Visibility.Collapsed;
            if (uidExists) return;
            DatabaseHelper.PutStudent(student);
            DialogResult = true;
            Close();
        }

        private void StudUidBox_KeyDown(object sender, KeyEventArgs e)
        {
            UidMinErrTxt.Visibility = Visibility.Hidden;
            UidExists.Visibility = Visibility.Hidden;
        }

        private void StudUidBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}
