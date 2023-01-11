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
using static Index_Prototype.Directory.DataTemplates;
using WpfApp2;

namespace Index_Prototype.Pages.Student_Info.EditStudentInfo
{
    /// <summary>
    /// Interaction logic for EditStudentInfo.xaml
    /// </summary>
    public partial class EditStudentInfo : Window
    {
        public DataTemplates.Student student { get; set; }
        public EditStudentInfo(Student student)
        {
            this.student = student;
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
            FillErrTxt.Visibility = !isAllFilled ? Visibility.Visible : Visibility.Collapsed;
            return isAllFilled;
        }

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isAllFilled()) return;
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
