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
using System.Windows.Shapes;

namespace Index_Prototype.Pages.Add_Teacher
{
    /// <summary>
    /// Interaction logic for AddTeacher.xaml
    /// </summary>
    public partial class AddTeacher : Window
    {
        public class User
        {
            public string lastName { get; set; }
            public string firstName { get; set; }
            public string middleName { get; set; }
            public string password { get; set; }
            public string profile { get; set; }
        }
        public class Teacher : User
        {
            public Teacher(string lastName,string firstName, string middleName)
            {
                this.lastName = lastName;
                this.firstName = firstName;
                this.middleName = middleName;
            }
            public Teacher()
            {

            }
        }
        public AddTeacher()
        {
            InitializeComponent();
        }
    }
}
