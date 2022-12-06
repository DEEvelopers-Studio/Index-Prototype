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
    /// Interaction logic for TeacherList.xaml
    /// </summary>
    public partial class Accounts : Page
    {
        public static List<Teacher> teachers { get; set; } = new List<Teacher> { new Teacher() { firstName = "Arvin John", lastName = "Suyat", middleName = "", password = "asd" }, new Teacher() { firstName = "Jermaine", lastName = "Marabe", middleName = "", password = "asd" }, new Teacher() { firstName = "John Rafael", lastName = "Dee", middleName = "M", password = "asd" }, new Teacher() { firstName = "asd", lastName = "dee", middleName = "M", password = "asd" }, new Teacher() { firstName = "asd", lastName = "dee", middleName = "M", password = "asd" }, };

        public Accounts()
        {
            InitializeComponent();
        }
    }
}
