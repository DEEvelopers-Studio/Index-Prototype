using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp2;
using static Index_Prototype.Pages.Add_Teacher.AddTeacher;

namespace Index_Prototype.Pages.Home
{
    /// <summary>
    /// Interaction logic for TeacherList.xaml
    /// </summary>
    public partial class Accounts : Page
    {
        public class User
        {
            public enum NameType { FIRST,LAST,MIDDLE,COMMA};
            public string uid { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string middleName { get; set; }
            public string profileLocation { get; set; }
            public string getName(NameType[] format)
            {

                string name = "";

                for (int i = 0; i < format.Length; i++)
                {
                    ///dont add space on start and when comma will be next
                    if (!(format.Length == 0 || format[i] == NameType.COMMA)) name += " ";
                    ///stitch the name according to the order
                    switch (format[i])
                    {
                        case NameType.FIRST:
                            name += firstName;
                            break;
                        case NameType.LAST:

                            name += lastName;
                            break;
                        case NameType.MIDDLE:

                            name += middleName;
                            break;

                        case NameType.COMMA:

                            name += ",";
                            break;
                    }
                }
                return name;
            }
            /// <summary>
            /// get the name of the account
            /// </summary>
            /// <returns>returns a name in LASTNAME, FIRSTNAME order</returns>
            public string getName()
            {
                return getName(new NameType[] { NameType.LAST, NameType.COMMA, NameType.FIRST });
            }
        }
        public ObservableCollection<User> accounts { get; set; } = new ObservableCollection<User>();

        public Accounts()
        {
            InitializeComponent();
            DatabaseHelper.getTeachers().ForEach(teacher =>
            {
                accounts.Add(teacher);
            });
            
        }
        Action<User> onAccSelect;
        /// <summary>
        /// fires when an account gets selected
        /// </summary>
        /// <param name="uid">account id on the database</param>
        internal void OnAccountSelect(Action<User> account)
        {
            onAccSelect = account;
        }
        private void SelectedAccount(object sender, MouseButtonEventArgs e)
        {
            onAccSelect(((sender as ListViewItem)?.Content as User));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            accounts.Add(new User() { firstName = "Arvin John", lastName = "Suyat", middleName = "" });
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
